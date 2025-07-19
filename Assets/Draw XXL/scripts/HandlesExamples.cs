namespace DrawXXL
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

    public class HandlesExamples
    {
#if UNITY_EDITOR
        //use Draw XXL draw operations only from inside "OnSceneGUI()" after you have set "DrawBasics.usedUnityLineDrawingMethod" to "handlesLines" (as it happens in "ConfigureDrawXXLsGlobalSettingsForDrawingHandles()").
        //-> This also ensures that the function calls will be automatically stripped from the code when building, which prevents build errors, since the functions inside this HandlesExamples-class are not present outside of the Editor.

        //Analyzing the code in this class probably only be useful for you if you are already familiar with using custom Handles/Unitys Handles class. If not, this blog post provides good insights on the way of thinking behind it: https://blog.unity.com/technology/going-deep-with-imgui-and-editor-customization

        static DrawBasics.UsedUnityLineDrawingMethod usedLineDrawingMethod_before;
        static DrawText.AutomaticTextOrientation automaticTextOrientation_before;
        static DrawBasics.CameraForAutomaticOrientation cameraForAutomaticOrientation_before;
        static DrawShapes.AutomaticOrientationOfFlatShapes automaticOrientationOfFlatShapes_before;
        public static void ConfigureDrawXXLsGlobalSettingsForDrawingHandles()
        {
            //-> "ConfigureDrawXXLsGlobalSettingsForDrawingHandles()" is setting some global Draw XXL settings so that drawing with Handles can be done with less thinking. The affected settings are "DrawBasics.usedLineDrawingMethod", "DrawText.automaticTextOrientation" and "DrawBasics.cameraForAutomaticOrientation". Moreover the settings as they were before get saved, so they can be easily restored afterwards by "RevertDrawXXLsGlobalHandleSettingsToWhatTheyWereBefore()". This ensures that these settings don't interfere with other code that draws with Draw XXL, e.g. from within "Update()".
            //-> This function is recommended to be used in conjunction with the "RevertDrawXXLsGlobalHandleSettingsToWhatTheyWereBefore()" function, to "encapsulate" everything that Draw XXL draws for Handles.
            //-> When drawing with Draw XXL inside "OnSceneGUI()" the recommended pattern is to call "ConfigureDrawXXLsGlobalSettingsForDrawingHandles()", then use the Draw XXL drawing functions to draw whatever you like, and then calling "RevertDrawXXLsGlobalHandleSettingsToWhatTheyWereBefore()"

            //Saving the settings as the were before:
            usedLineDrawingMethod_before = DrawBasics.usedUnityLineDrawingMethod;
            automaticTextOrientation_before = DrawText.automaticTextOrientation;
            cameraForAutomaticOrientation_before = DrawBasics.cameraForAutomaticOrientation;
            automaticOrientationOfFlatShapes_before = DrawShapes.automaticOrientationOfFlatShapes;

            //Instructing Draw XXL which configuration to use for the current Handle drawing:
            DrawBasics.usedUnityLineDrawingMethod = DrawBasics.UsedUnityLineDrawingMethod.handlesLines; //-> this cannot interfere with the automatic fallback of "usedUnityLineDrawingMethod" to "mesh" in builds, since the whole "HandlesExamples" class is stripped in builds.
            DrawText.automaticTextOrientation = DrawText.AutomaticTextOrientation.screen;
            DrawBasics.cameraForAutomaticOrientation = DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;
            DrawShapes.automaticOrientationOfFlatShapes = DrawShapes.AutomaticOrientationOfFlatShapes.screen;

        }

        public static void RevertDrawXXLsGlobalHandleSettingsToWhatTheyWereBefore()
        {
            //-> This funciton reverts the global Draw XXL settings which "ConfigureDrawXXLsGlobalSettingsForDrawingHandles()" has adjusted to what they were before.
            //-> This function should only be used in conjunction with the "ConfigureDrawXXLsGlobalSettingsForDrawingHandles()" function, to "encapsulate" everything that Draw XXL draws for Handles.
            //-> When drawing with Draw XXL inside "OnSceneGUI()" the recommended pattern is to call "ConfigureDrawXXLsGlobalSettingsForDrawingHandles()", then use the Draw XXL drawing functions to draw whatever you like, and then calling "RevertDrawXXLsGlobalHandleSettingsToWhatTheyWereBefore()"

            //Reverting the global configuration values that have been set before by "ConfigureDrawXXLsGlobalSettingsForDrawingHandles()":
            DrawBasics.usedUnityLineDrawingMethod = usedLineDrawingMethod_before; //-> this cannot interfere with the automatic fallback of "usedUnityLineDrawingMethod" to "wireMesh" in builds, since the whole "HandlesExamples" class is stripped in builds.
            DrawText.automaticTextOrientation = automaticTextOrientation_before;
            DrawBasics.cameraForAutomaticOrientation = cameraForAutomaticOrientation_before;
            DrawShapes.automaticOrientationOfFlatShapes = automaticOrientationOfFlatShapes_before;
        }

        public static void DrawLine(Vector3 start, Vector3 end, float thickness = 0.0f, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, string text = null)
        {
            //This funcion is like "Handles.DrawLine()" but with two additinal optional parameters that can change the style of the line and add a text along the line.
            if (Event.current.type == EventType.Repaint) //-> Draw with Draw XXL only inside the "Repaint"-event, otherwise the "DrawBasics.MaxAllowedDrawnLinesPerFrame"-mechanic will get confused and restricts the drawing earlier than neccessary.
            {
                Vector3 theMiddleOfTheLine = 0.5f * (start + end);
                float screenSizeThatAHandleWouldHaveAtTheLine = HandleUtility.GetHandleSize(theMiddleOfTheLine);
                float stylePatternScaleFactor = 5.0f * screenSizeThatAHandleWouldHaveAtTheLine; //-> this lets the line style pattern appear approximately constant in screenspace, no matter how far the line is away from the camera, which may be desired if the line is part of a handle whose size is constant in screenspace.
                float width_worldSpace = 0.01f * thickness * screenSizeThatAHandleWouldHaveAtTheLine; //-> converting the width to be constant in screenspace, though here in somehow arbitrary units.
                Color color = Handles.color; //-> When Draw XXL draws with Unitys Handles-Lines it doesn't automatically detect the "Handles.color", but instead takes the color that is specified as parameter argument in the draw function, or else falls back to "DrawBasics.defaultColor"

                ConfigureDrawXXLsGlobalSettingsForDrawingHandles(); //-> This function is recommended to be used in conjunction with the "RevertDrawXXLsGlobalHandleSettingsToWhatTheyWereBefore()" function, to "encapsulate" everything that Draw XXL draws for Handles.
                DrawBasics.Line(start, end, color, width_worldSpace, text, style, stylePatternScaleFactor);
                RevertDrawXXLsGlobalHandleSettingsToWhatTheyWereBefore(); //-> This function should be used in conjunction with the "ConfigureDrawXXLsGlobalSettingsForDrawingHandles()" function, to "encapsulate" everything that Draw XXL draws for Handles.
            }
        }

        static Vector3 sliderPosition_duringMouseDown;
        public static Vector3 SliderWithOffsetDisplay(Vector3 position, Vector3 direction)
        {
            return SliderWithOffsetDisplay(position, direction, HandleUtility.GetHandleSize(position), Handles.ArrowHandleCap, -1.0f);
        }

        public static Vector3 SliderWithOffsetDisplay(Vector3 position, Vector3 direction, float size, Handles.CapFunction capFunction, float snap)
        {
            //this is like Unitys "Handles.Slider" function, but additionally the dragged offset value is displayed together with a green or red arrow that indicates if the value change is positive or negative
            //Sidenote: As can be seen in Unitys Editor source code (link zu  https://github.com/Unity-Technologies/UnityCsReference/blob/2022.2/Editor/Mono/Handles.cs ) starting with Unity2022.2 Unitys "Handles" class has an additional overload for the "PositionHandle()" function, that takes the "PositionHandleIds ids"-parameter. This opens the possibility to extend this "SliderWithOffsetDisplay()" function to the whole position handle with all of it's three axes, without having to recreate the whole position handle by yourself.

            Event currentEvent = Event.current;

            if (currentEvent.type == EventType.MouseDown) //-> Detect the "MouseDown"-event BEFORE it gets eaten via "event.Use()" inside "Handles.Slider()". It will not be there anymore if this code is placed BELOW the "Handles.Slider" function call.
            {
                sliderPosition_duringMouseDown = position;
            }

            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            Vector3 returnedPositionFromUnitysBuildInSlider = Handles.Slider(controlID, position, direction, size, capFunction, snap);

            if (currentEvent.type == EventType.Repaint) //-> Draw with Draw XXL only inside the "Repaint"-event, otherwise the "DrawBasics.MaxAllowedDrawnLinesPerFrame"-mechanic will get confused and restricts the drawing earlier than neccessary.
            {
                bool sliderIsCurrentlyGrabbedByTheMouse = (GUIUtility.hotControl == controlID);
                if (sliderIsCurrentlyGrabbedByTheMouse)
                {
                    ConfigureDrawXXLsGlobalSettingsForDrawingHandles(); //-> This function is recommended to be used in conjunction with the "RevertDrawXXLsGlobalHandleSettingsToWhatTheyWereBefore()" function, to "encapsulate" everything that Draw XXL draws for Handles.
                    DrawOffsetValueAsText_viaDrawXXL(returnedPositionFromUnitysBuildInSlider, direction, size, capFunction);
                    RevertDrawXXLsGlobalHandleSettingsToWhatTheyWereBefore(); //-> This function should be used in conjunction with the "ConfigureDrawXXLsGlobalSettingsForDrawingHandles()" function, to "encapsulate" everything that Draw XXL draws for Handles.
                }
            }

            return returnedPositionFromUnitysBuildInSlider;
        }

        static void DrawOffsetValueAsText_viaDrawXXL(Vector3 currentPositionOfSlider, Vector3 direction, float size, Handles.CapFunction capFunction)
        {
            string displayedText = GetDisplayedText(currentPositionOfSlider, direction, capFunction);
            Vector3 textPosition;
            DrawText.TextAnchorDXXL textAnchor;
            if (capFunction == Handles.ArrowHandleCap) //-> this is the default handle cap if none is specified
            {
                textPosition = currentPositionOfSlider + direction.normalized * (1.2f * size);
                textAnchor = Direction_goesFromLeftToRight_insideSceneViewScreen(direction) ? DrawText.TextAnchorDXXL.MiddleLeft : DrawText.TextAnchorDXXL.MiddleRight;
            }
            else
            {
                textPosition = currentPositionOfSlider;
                textAnchor = DrawText.TextAnchorDXXL.LowerLeft;
            }

            Color textColor = Handles.selectedColor; //-> When Draw XXL draws with Unitys Handles-Lines it doesn't automatically detect the "Handles.color", but instead takes the color that is specified as parameter argument in the draw function, or else falls back to "DrawBasics.defaultColor"
            float textSize = 0.2f * size;
            Vector3 textDirection = Direction_goesFromLeftToRight_insideSceneViewScreen(direction) ? direction : (-direction);
            bool autoFlipTheTextToPreventMirrorInvertedDisplay = true;
            UtilitiesDXXL_Text.WriteFramed(displayedText, textPosition, textColor, textSize, textDirection, default(Vector3), textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, autoFlipTheTextToPreventMirrorInvertedDisplay, 0.0f, true);
        }

        static string GetDisplayedText(Vector3 currentPositionOfSlider, Vector3 direction, Handles.CapFunction capFunction)
        {
            string displayedText;

            float absoluteDraggedDistancesSinceMouseDown = (sliderPosition_duringMouseDown - currentPositionOfSlider).magnitude;
            if (currentPositionOfSlider == sliderPosition_duringMouseDown)
            {
                displayedText = "" + absoluteDraggedDistancesSinceMouseDown;
            }
            else
            {
                Vector3 initialPosition_to_currentPosition = currentPositionOfSlider - sliderPosition_duringMouseDown;
                bool valueChangeIsPositive = Vector3.Dot(initialPosition_to_currentPosition, direction) >= 0.0f;
                string stringThatWillAppearAsGreenOrRedArrowInTheFinalTextDisplay = DrawText.MarkupBoolArrow(valueChangeIsPositive);
                displayedText = stringThatWillAppearAsGreenOrRedArrowInTheFinalTextDisplay + absoluteDraggedDistancesSinceMouseDown;
            }

            if (capFunction != Handles.ArrowHandleCap)
            {
                string emptySpaceBetweenTextAndHandleCap = DrawText.MarkupCustomHeightEmptyLine(2); //-> shifting the text away from the handle cap, so they don't occlude each other
                displayedText = displayedText + emptySpaceBetweenTextAndHandleCap;
            }

            return displayedText;
        }

        static Vector2 mousePositionInScreenspace_duringMouseDown;
        static Vector2 current_mousePositionInScreenspace;
        static float relativeSizeOfPercentageSliderCap = 0.2f;
        static float valueToSlide_duringMouseDown;

        public static float PercentageSlider(float valueToSlide, float valueThatDefines100percent, Vector3 position, Vector3 direction)
        {
            return PercentageSlider(valueToSlide, valueThatDefines100percent, position, direction, HandleUtility.GetHandleSize(position), Handles.CylinderHandleCap, -1.0f);
        }

        public static float PercentageSlider(float valueToSlide, float valueThatDefines100percent, Vector3 position, Vector3 direction, DrawBasics.IconType icon)
        {
            return PercentageSlider(valueToSlide, valueThatDefines100percent, position, direction, HandleUtility.GetHandleSize(position), Handles.CylinderHandleCap, -1.0f, icon);
        }

        public static float PercentageSlider(float valueToSlide, float valueThatDefines100percent, Vector3 position, Vector3 direction, string text)
        {
            return PercentageSlider(valueToSlide, valueThatDefines100percent, position, direction, HandleUtility.GetHandleSize(position), Handles.CylinderHandleCap, -1.0f, text);
        }

        public static float PercentageSlider(float valueToSlide, float valueThatDefines100percent, Vector3 position, Vector3 direction, float size, Handles.CapFunction capFunction, float snap, DrawBasics.IconType icon)
        {
            float iconSizeScaleFactor = 5.0f;
            int boldStrokeWidth_asPPMofSize = 25000;
            string textThatConsistsOfOnlyOneLetterWhichIsTheIconItself = DrawText.MarkupIcon(icon);
            string textThatConsistsOfOnlyOneLetterWhichIsTheIconItself_withBolderFont = DrawText.MarkupStrokeWidth(textThatConsistsOfOnlyOneLetterWhichIsTheIconItself, boldStrokeWidth_asPPMofSize);
            string textThatConsistsOfOnlyOneLetterWhichIsTheIconItself_withBolderFont_andScaledBigger = DrawText.MarkupSize(textThatConsistsOfOnlyOneLetterWhichIsTheIconItself_withBolderFont, iconSizeScaleFactor);
            return PercentageSlider(valueToSlide, valueThatDefines100percent, position, direction, size, capFunction, snap, textThatConsistsOfOnlyOneLetterWhichIsTheIconItself_withBolderFont_andScaledBigger);
        }

        public static float PercentageSlider(float valueToSlide, float valueThatDefines100percent, Vector3 position, Vector3 direction, float size, Handles.CapFunction capFunction, float snap, string text = null)
        {
            //This is similar to Unitys "Handles.ScaleSlider" function, but the handle doesn't snap back after the mouse drag ended and it adds a percentage value display. Moreover an optional text can be added.

            if (Mathf.Approximately(valueThatDefines100percent, 0.0f))
            {
                Debug.LogError("Cannot display 'PercentageSlider' if 'valueThatDefines100percent' is 0.");
                return valueToSlide;
            }

            Event currentEvent = Event.current;

            if (currentEvent.type == EventType.MouseDown) //-> Detect the "MouseDown"-event BEFORE it gets eaten via "event.Use()" inside "Handles.Slider()". It will not be there anymore if this code is placed BELOW the "Handles.Slider" function call.
            {
                valueToSlide_duringMouseDown = valueToSlide;
                valueToSlide_duringMouseDown = ForceAwayFromZero(valueToSlide_duringMouseDown, valueThatDefines100percent); //-> Preventing slider deadlock at "valueToSlide == 0"
                mousePositionInScreenspace_duringMouseDown = currentEvent.mousePosition;
                current_mousePositionInScreenspace = currentEvent.mousePosition;
            }

            float valueToSlide_asPercentageFrom0to1 = valueToSlide / valueThatDefines100percent;
            float magnifiedSize = 1.5f * size; //-> Displaying the percentage slider bigger than Unitys default display of Handles
            Vector3 direction_normalized = direction.normalized;
            Vector3 currentOffsetOfHandleCap = direction_normalized * magnifiedSize * valueToSlide_asPercentageFrom0to1;
            Vector3 positionOfHandleCap = position + currentOffsetOfHandleCap;
            float sizeOfHandleCap = relativeSizeOfPercentageSliderCap * size;

            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            bool sliderIsCurrentlyGrabbedByTheMouse = (GUIUtility.hotControl == controlID);

            DrawPercentageElements_viaDrawXXL(valueToSlide, valueThatDefines100percent, position, direction_normalized, size, text, sliderIsCurrentlyGrabbedByTheMouse, positionOfHandleCap, magnifiedSize);
            Handles.Slider(controlID, positionOfHandleCap, direction, sizeOfHandleCap, capFunction, snap); //-> Draw the Handle cap with Unitys build-in Handles

            if (sliderIsCurrentlyGrabbedByTheMouse)
            {
                return GetScaledValue(valueThatDefines100percent, position, direction_normalized, size);
            }
            else
            {
                return valueToSlide;
            }
        }

        static void DrawPercentageElements_viaDrawXXL(float valueToSlide, float valueThatDefines100percent, Vector3 position, Vector3 direction_normalized, float size, string text, bool sliderIsCurrentlyGrabbedByTheMouse, Vector3 positionOfHandleCap, float magnifiedSize)
        {
            Event currentEvent = Event.current;
            if (currentEvent.type == EventType.Repaint) //-> Draw with Draw XXL only inside the "Repaint"-event, otherwise the "DrawBasics.MaxAllowedDrawnLinesPerFrame"-mechanic will get confused and restricts the drawing earlier than neccessary.
            {
                float worldSpaceLength_of100percentSpan = magnifiedSize;
                Vector3 endPosition_of100percentSpan = position + direction_normalized * worldSpaceLength_of100percentSpan;
                Color colorOfSpring_independentFromStretchTension = sliderIsCurrentlyGrabbedByTheMouse ? Handles.selectedColor : Handles.color; //-> When Draw XXL draws with Unitys Handles-Lines it doesn't automatically detect the "Handles.color", but instead takes the color that is specified as parameter argument in the draw function, or else falls back to "DrawBasics.defaultColor"

                ConfigureDrawXXLsGlobalSettingsForDrawingHandles(); //-> This function is recommended to be used in conjunction with the "RevertDrawXXLsGlobalHandleSettingsToWhatTheyWereBefore()" function, to "encapsulate" everything that Draw XXL draws for Handles.
                Draw100percentReferenceLengthDisplay_viaDrawXXL(position, endPosition_of100percentSpan, colorOfSpring_independentFromStretchTension); //-> This draws the 100percent-stretch-reference-display manually instead of using the "alphaOfReferenceLengthDisplay" parameter of the "DrawBasics.LineUnderTension()" below (inside "DrawPercentageValueSpiral_viaDrawXXL()"), because the "alphaOfReferenceLengthDisplay" parameter is not fit for negative values.
                DrawPercentageValueSpiral_viaDrawXXL(position, positionOfHandleCap, worldSpaceLength_of100percentSpan, colorOfSpring_independentFromStretchTension);
                DrawPercentageValueDisplayText_viaDrawXXL(valueToSlide, valueThatDefines100percent, endPosition_of100percentSpan, direction_normalized, size, colorOfSpring_independentFromStretchTension, text);
                RevertDrawXXLsGlobalHandleSettingsToWhatTheyWereBefore(); //-> This function should be used in conjunction with the "ConfigureDrawXXLsGlobalSettingsForDrawingHandles()" function, to "encapsulate" everything that Draw XXL draws for Handles.
            }
        }

        static void Draw100percentReferenceLengthDisplay_viaDrawXXL(Vector3 positionOfTheHandle, Vector3 endPosition_of100percentSpan, Color colorOfSpring_independentFromStretchTension)
        {
            Color color_ofReferenceLengthDisplay = colorOfSpring_independentFromStretchTension; //-> When Draw XXL draws with Unitys Handles-Lines it doesn't automatically detect the "Handles.color", but instead takes the color that is specified as parameter argument in the draw function, or else falls back to "DrawBasics.defaultColor"
            float alphaOf100percentReferenceLengthDisplay = 0.5f;
            color_ofReferenceLengthDisplay.a = alphaOf100percentReferenceLengthDisplay;
            bool flattenThickRoundLineIntoAmplitudePlane = true;
            float endPlates_size = 0.1f;

            DrawBasics.LengthInterpretation endPlates_sizeInterpretation_before = DrawBasics.endPlates_sizeInterpretation;
            DrawBasics.endPlates_sizeInterpretation = DrawBasics.LengthInterpretation.relativeToLineLength; //-> Instruct Draw XXL how to interpret the "endPlates_size" parameter for all following draw operations
            DrawBasics.Line(positionOfTheHandle, endPosition_of100percentSpan, color_ofReferenceLengthDisplay, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, default(Vector3), flattenThickRoundLineIntoAmplitudePlane, endPlates_size);
            DrawBasics.endPlates_sizeInterpretation = endPlates_sizeInterpretation_before; //-> Revert the Draw XXL setting to what it was before
        }

        static void DrawPercentageValueSpiral_viaDrawXXL(Vector3 positionOfTheHandle, Vector3 positionOfHandleCap, float worldSpaceLength_of100percentSpan, Color colorOfSpring_independentFromStretchTension)
        {
            DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.spiral;
            float relaxedLength = worldSpaceLength_of100percentSpan;
            Vector3 theMiddleOfTheLine = 0.5f * (positionOfTheHandle + positionOfHandleCap);
            float sizeOfTheHandleInScreenspace = HandleUtility.GetHandleSize(theMiddleOfTheLine);
            float stylePatternScaleFactor = 5.0f * sizeOfTheHandleInScreenspace; //-> this lets the line style pattern appear approximately constant in screenspace, no matter how far the line is away from the camera.
            float stretchFactor_forStretchedTensionColor = 2.0f; //-> this is unused, since the color does not depend on the stretch tension
            float stretchFactor_forSqueezedTensionColor = 0.0f; //-> this is unused, since the color does not depend on the stretch tension
            string text_ofStretchedSpring = null;
            float width_worldSpace = 0.0f; //-> If a width other than 0 is wanted: The width value is in world space units, in contrast to the "Handles.DrawLine(thickness)" paramter, which is in UI points. For a suggestion how to convert it: See "HandlesExamples.DrawLine()"
            float alphaOfReferenceLengthDisplay = 0.0f; //-> this is disabled here, since it already has been drawn separately further up

            float stylePatternScaleFactor_alongLineDir_ignoringAmplitude_before = DrawBasics.StylePatternScaleFactor_alongLineDir_ignoringAmplitude;
            DrawBasics.StylePatternScaleFactor_alongLineDir_ignoringAmplitude = 0.5f; //-> Instruct Draw XXL how to use this style scaling for all following draw operations
            DrawBasics.LineUnderTension(positionOfTheHandle, positionOfHandleCap, relaxedLength, colorOfSpring_independentFromStretchTension, lineStyle, stretchFactor_forStretchedTensionColor, colorOfSpring_independentFromStretchTension, stretchFactor_forSqueezedTensionColor, colorOfSpring_independentFromStretchTension, width_worldSpace, text_ofStretchedSpring, alphaOfReferenceLengthDisplay, stylePatternScaleFactor);
            DrawBasics.StylePatternScaleFactor_alongLineDir_ignoringAmplitude = stylePatternScaleFactor_alongLineDir_ignoringAmplitude_before; //-> Revert the Draw XXL setting to what it was before
        }

        static void DrawPercentageValueDisplayText_viaDrawXXL(float valueToSlide, float valueThatDefines100percent, Vector3 endPosition_of100percentSpan, Vector3 direction_normalized, float size, Color colorOfSpring_independentFromStretchTension, string text)
        {
            float percentageOfCurrentSlidedValue = 100.0f * (valueToSlide / valueThatDefines100percent);
            int percentageOfCurrentSlidedValue_rounded = Mathf.RoundToInt(percentageOfCurrentSlidedValue);
            string emptyTextSpaceBelowText_soThatTheTextIsNotOccludedByTheHandleCapOrTheSpring = DrawText.MarkupCustomHeightEmptyLine(3);
            string displayedText = text + "<br>" + percentageOfCurrentSlidedValue_rounded + "%" + emptyTextSpaceBelowText_soThatTheTextIsNotOccludedByTheHandleCapOrTheSpring;
            Vector3 textPosition = endPosition_of100percentSpan;
            float textSize = 0.1f * size;
            Vector3 textDirection = Direction_goesFromLeftToRight_insideSceneViewScreen(direction_normalized) ? direction_normalized : (-direction_normalized);
            Color textColor = colorOfSpring_independentFromStretchTension; //-> When Draw XXL draws with Unitys Handles-Lines it doesn't automatically detect the "Handles.color", but instead takes the color that is specified as parameter argument in the draw function, or else falls back to "DrawBasics.defaultColor"
            DrawText.TextAnchorDXXL textAnchor = DrawText.TextAnchorDXXL.LowerCenter;
            UtilitiesDXXL_Text.WriteFramed(displayedText, textPosition, textColor, textSize, textDirection, default(Vector3), textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, 0.0f, true);
        }

        static float ForceAwayFromZero(float valueToSlide_duringMouseDown, float valueThatDefines100percent)
        {
            float smallestAllowedPercentage_duringMouseDown = 0.001f;
            float valueToSlide_duringMouseDown_absolute = Mathf.Abs(valueToSlide_duringMouseDown);
            float valueThatDefines100percent_absolute = Mathf.Abs(valueThatDefines100percent);
            float smallestAllowedValue_absolute = smallestAllowedPercentage_duringMouseDown * valueThatDefines100percent_absolute;

            if (valueToSlide_duringMouseDown_absolute < smallestAllowedValue_absolute)
            {
                return smallestAllowedValue_absolute;
            }
            else
            {
                return valueToSlide_duringMouseDown;
            }
        }

        static float GetScaledValue(float valueThatDefines100percent, Vector3 position, Vector3 direction_normalized, float size)
        {
            //calculate the scaled value similar as "Handles.ScaleSlider()" does it:      
            Event currentEvent = Event.current;
            float percentageValueToSlide_duringMouseDown_asPercentageFrom0to1 = valueToSlide_duringMouseDown / valueThatDefines100percent;
            current_mousePositionInScreenspace += currentEvent.delta;
            float divisor = size * percentageValueToSlide_duringMouseDown_asPercentageFrom0to1;
            float distanceHowMuchTheHandleHasBeenDraggedWithTheMouse = relativeSizeOfPercentageSliderCap * HandleUtility.CalcLineTranslation(mousePositionInScreenspace_duringMouseDown, current_mousePositionInScreenspace, position, direction_normalized) / divisor;
            float returnedScaleValue = valueToSlide_duringMouseDown * (1.0f + distanceHowMuchTheHandleHasBeenDraggedWithTheMouse);
            return returnedScaleValue;
        }

        static float valueToShiftInsideRange_duringMouseDown;

        public static float RangeSlider(float value, float lowerEndOfRange, float upperEndOfRange, Vector3 position, Vector3 direction)
        {
            return RangeSlider(value, lowerEndOfRange, upperEndOfRange, position, direction, HandleUtility.GetHandleSize(position), Handles.SphereHandleCap, -1.0f, null);
        }

        public static float RangeSlider(float value, float lowerEndOfRange, float upperEndOfRange, Vector3 position, Vector3 direction, DrawBasics.IconType icon)
        {
            return RangeSlider(value, lowerEndOfRange, upperEndOfRange, position, direction, HandleUtility.GetHandleSize(position), Handles.SphereHandleCap, -1.0f, icon);
        }

        public static float RangeSlider(float value, float lowerEndOfRange, float upperEndOfRange, Vector3 position, Vector3 direction, string text)
        {
            return RangeSlider(value, lowerEndOfRange, upperEndOfRange, position, direction, HandleUtility.GetHandleSize(position), Handles.SphereHandleCap, -1.0f, text);
        }

        public static float RangeSlider(float value, float lowerEndOfRange, float upperEndOfRange, Vector3 position, Vector3 direction, float size, Handles.CapFunction capFunction, float snap, DrawBasics.IconType icon)
        {
            float iconSizeScaleFactor = 2.8f;
            int boldStrokeWidth_asPPMofSize = 25000;
            string textThatConsistsOfOnlyOneLetterWhichIsTheIconItself = DrawText.MarkupIcon(icon);
            string textThatConsistsOfOnlyOneLetterWhichIsTheIconItself_withBolderFont = DrawText.MarkupStrokeWidth(textThatConsistsOfOnlyOneLetterWhichIsTheIconItself, boldStrokeWidth_asPPMofSize);
            string textThatConsistsOfOnlyOneLetterWhichIsTheIconItself_withBolderFont_andScaledBigger = DrawText.MarkupSize(textThatConsistsOfOnlyOneLetterWhichIsTheIconItself_withBolderFont, iconSizeScaleFactor);
            float relative_verticalTextPositionOffsetDistance_fromRangeLine = 1.25f;
            return RangeSlider(value, lowerEndOfRange, upperEndOfRange, position, direction, size, capFunction, snap, textThatConsistsOfOnlyOneLetterWhichIsTheIconItself_withBolderFont_andScaledBigger, relative_verticalTextPositionOffsetDistance_fromRangeLine);
        }

        public static float RangeSlider(float value, float lowerEndOfRange, float upperEndOfRange, Vector3 position, Vector3 direction, float size, Handles.CapFunction capFunction, float snap, string text = null)
        {
            float relative_verticalTextPositionOffsetDistance_fromRangeLine = 2.0f;
            return RangeSlider(value, lowerEndOfRange, upperEndOfRange, position, direction, size, capFunction, snap, text, relative_verticalTextPositionOffsetDistance_fromRangeLine);
        }

        static float RangeSlider(float value, float lowerEndOfRange, float upperEndOfRange, Vector3 position, Vector3 direction, float size, Handles.CapFunction capFunction, float snap, string text, float relative_verticalTextPositionOffsetDistance_fromRangeLine)
        {
            //A slider handle where the value is clamped inside an allowed range, as it is frequently seen in Unitys Inspector Window.
            //"position": more precisely: it is the position of the lowerEndOfTheRange

            if (Mathf.Approximately(lowerEndOfRange, upperEndOfRange))
            {
                Debug.LogError("Cannot display 'RangeSlider' with range span of 0.");
                return value;
            }

            TryFlipValues_inCaseLowerEndIsBiggerThanUpperEnd(ref lowerEndOfRange, ref upperEndOfRange);
            float value_clamped = Mathf.Clamp(value, lowerEndOfRange, upperEndOfRange);

            Event currentEvent = Event.current;

            if (currentEvent.type == EventType.MouseDown) //-> Detect the "MouseDown"-event BEFORE it gets eaten via "event.Use()" inside "Handles.Slider()". It will not be there anymore if this code is placed BELOW the "Handles.Slider" function call.
            {
                valueToShiftInsideRange_duringMouseDown = value_clamped;
                mousePositionInScreenspace_duringMouseDown = currentEvent.mousePosition;
                current_mousePositionInScreenspace = currentEvent.mousePosition;
            }

            float rangeSpan = upperEndOfRange - lowerEndOfRange;
            float value_portionThatIsBiggerThanLowerEndOfRange = value_clamped - lowerEndOfRange;
            float valueToSlide_as0to1insideRange = value_portionThatIsBiggerThanLowerEndOfRange / rangeSpan;
            float magnifiedSize = 2.0f * size; //-> Displaying the range slider bigger than Unitys default display of Handles
            Vector3 direction_normalized = direction.normalized;
            Vector3 rangeStart_to_rangeEnd = direction_normalized * magnifiedSize;
            Vector3 currentOffsetOfHandleCap = rangeStart_to_rangeEnd * valueToSlide_as0to1insideRange;
            Vector3 positionOfHandleCap = position + currentOffsetOfHandleCap;
            Vector3 positionOfRangeEnd = position + rangeStart_to_rangeEnd;
            float sizeOfHandleCap = relativeSizeOfPercentageSliderCap * size;

            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            bool sliderIsCurrentlyGrabbedByTheMouse = (GUIUtility.hotControl == controlID);

            DrawRangeElements_viaDrawXXL(value_clamped, lowerEndOfRange, upperEndOfRange, positionOfHandleCap, position, positionOfRangeEnd, direction_normalized, size, magnifiedSize, sizeOfHandleCap, sliderIsCurrentlyGrabbedByTheMouse, text, relative_verticalTextPositionOffsetDistance_fromRangeLine);
            Handles.Slider(controlID, positionOfHandleCap, direction, sizeOfHandleCap, capFunction, snap); //-> Draw the Handle cap with Unitys build-in Handles

            if (sliderIsCurrentlyGrabbedByTheMouse)
            {
                return GetScaledValueInsideRange(lowerEndOfRange, upperEndOfRange, position, direction_normalized, size);
            }
            else
            {
                return value_clamped;
            }
        }

        static void TryFlipValues_inCaseLowerEndIsBiggerThanUpperEnd(ref float lowerEndOfRange, ref float upperEndOfRange)
        {
            if (lowerEndOfRange > upperEndOfRange)
            {
                float clipboard = lowerEndOfRange;
                lowerEndOfRange = upperEndOfRange;
                upperEndOfRange = clipboard;
            }
        }

        static void DrawRangeElements_viaDrawXXL(float value_clamped, float lowerEndOfRange, float upperEndOfRange, Vector3 positionOfHandleCap, Vector3 positionOfRangeStart, Vector3 positionOfRangeEnd, Vector3 direction_normalized, float size, float lenghtOfTheRangeLine, float sizeOfHandleCap, bool sliderIsCurrentlyGrabbedByTheMouse, string textFromUser, float relative_verticalTextPositionOffsetDistance_fromRangeLine)
        {
            Event currentEvent = Event.current;
            if (currentEvent.type == EventType.Repaint) //-> Draw with Draw XXL only inside the "Repaint"-event, otherwise the "DrawBasics.MaxAllowedDrawnLinesPerFrame"-mechanic will get confused and restricts the drawing earlier than neccessary.
            {
                ConfigureDrawXXLsGlobalSettingsForDrawingHandles(); //-> This function is recommended to be used in conjunction with the "RevertDrawXXLsGlobalHandleSettingsToWhatTheyWereBefore()" function, to "encapsulate" everything that Draw XXL draws for Handles.

                Color color = sliderIsCurrentlyGrabbedByTheMouse ? Handles.selectedColor : Handles.color; //-> When Draw XXL draws with Unitys Handles-Lines it doesn't automatically detect the "Handles.color", but instead takes the color that is specified as parameter argument in the draw function, or else falls back to "DrawBasics.defaultColor"
                float half_sizeOfHandleCap = 0.5f * sizeOfHandleCap;
                Vector3 position_ofRangeStartPlate = positionOfRangeStart - direction_normalized * half_sizeOfHandleCap;
                Vector3 position_ofRangeEndPlate = positionOfRangeEnd + direction_normalized * half_sizeOfHandleCap;
                float textSize = 0.15f * size;

                DrawRangeLine_viaDrawXXL(position_ofRangeStartPlate, position_ofRangeEndPlate, color);
                Vector3 usedTextDirection = DrawRangeValuesAsText_viaDrawXXL(value_clamped, lowerEndOfRange, upperEndOfRange, positionOfHandleCap, position_ofRangeStartPlate, position_ofRangeEndPlate, direction_normalized, textSize, color);
                DrawRangeSliderName_viaDrawXXL(usedTextDirection, positionOfRangeStart, positionOfRangeEnd, direction_normalized, lenghtOfTheRangeLine, textFromUser, textSize, relative_verticalTextPositionOffsetDistance_fromRangeLine, color);

                RevertDrawXXLsGlobalHandleSettingsToWhatTheyWereBefore(); //-> This function should be used in conjunction with the "ConfigureDrawXXLsGlobalSettingsForDrawingHandles()" function, to "encapsulate" everything that Draw XXL draws for Handles.
            }
        }

        static void DrawRangeLine_viaDrawXXL(Vector3 position_ofRangeStartPlate, Vector3 position_ofRangeEndPlate, Color color)
        {
            float width_worldSpace = 0.0f; //-> If a width other than 0 is wanted: The width value is in world space units, in contrast to the "Handles.DrawLine(thickness)" paramter, which is in UI points. For a suggestion how to convert it: See "HandlesExamples.DrawLine()"
            DrawBasics.LineStyle style = DrawBasics.LineStyle.solid;
            bool flattenThickRoundLineIntoAmplitudePlane = false; //-> "false" makes the endPlates to round discs, instead as flat lines
            float endPlates_size = 0.1f;

            DrawBasics.LengthInterpretation endPlates_sizeInterpretation_before = DrawBasics.endPlates_sizeInterpretation;
            DrawBasics.endPlates_sizeInterpretation = DrawBasics.LengthInterpretation.relativeToLineLength; //-> Instruct Draw XXL how to interpret the "endPlates_size" parameter for all following draw operations
            DrawBasics.Line(position_ofRangeStartPlate, position_ofRangeEndPlate, color, width_worldSpace, null, style, 1.0f, 0.0f, default(Vector3), flattenThickRoundLineIntoAmplitudePlane, endPlates_size);
            DrawBasics.endPlates_sizeInterpretation = endPlates_sizeInterpretation_before; //-> Revert the Draw XXL setting to what it was before
        }

        static Vector3 DrawRangeValuesAsText_viaDrawXXL(float value_clamped, float lowerEndOfRange, float upperEndOfRange, Vector3 positionOfHandleCap, Vector3 position_ofRangeStartPlate, Vector3 position_ofRangeEndPlate, Vector3 direction_normalized, float textSize, Color color)
        {
            //-> Simple trick: Adding blank text space in front of the value to have some distance from the range line, so it doesn't intersect with the range line end plates or the handle cap. This works only if we specify the textAnchor to be on the left side, which we do below.
            string lowerRangeEnd_asTextString = "  " + lowerEndOfRange;
            string upperRangeEnd_asTextString = "  " + upperEndOfRange;
            string currentValue_asTextString = "  " + value_clamped;

            int boldStrokeWidth_asPPMofSize = 60000;
            string lowerRangeEnd_asTextString_inBoldText = DrawText.MarkupStrokeWidth(lowerRangeEnd_asTextString, boldStrokeWidth_asPPMofSize);
            string upperRangeEnd_asTextString_inBoldText = DrawText.MarkupStrokeWidth(upperRangeEnd_asTextString, boldStrokeWidth_asPPMofSize);

            Vector3 textUpward_forValueTexts = Direction_goesFromLeftToRight_insideSceneViewScreen(direction_normalized) ? (-direction_normalized) : direction_normalized;
            Vector3 textDirection_forValueTexts = default(Vector3); //-> If a text orientation vector is not specified then Draw XXL will automatically try to align the text according to static global setting "DrawText.automaticTextOrientation". In this case only "textUp" is specified and constrains the text orientation, so that the text will be perpendicular to the range line.
            DrawText.TextAnchorDXXL textAnchor_forValueTexts = DrawText.TextAnchorDXXL.MiddleLeft;
            bool autoFlipTheTextToPreventMirrorInvertedDisplay = true;

            UtilitiesDXXL_Text.WriteFramed(lowerRangeEnd_asTextString_inBoldText, position_ofRangeStartPlate, color, textSize, textDirection_forValueTexts, textUpward_forValueTexts, textAnchor_forValueTexts, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, autoFlipTheTextToPreventMirrorInvertedDisplay, 0.0f, true);
            UtilitiesDXXL_Text.WriteFramed(upperRangeEnd_asTextString_inBoldText, position_ofRangeEndPlate, color, textSize, textDirection_forValueTexts, textUpward_forValueTexts, textAnchor_forValueTexts, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, autoFlipTheTextToPreventMirrorInvertedDisplay, 0.0f, true);
            UtilitiesDXXL_Text.WriteFramed(currentValue_asTextString, positionOfHandleCap, color, textSize, textDirection_forValueTexts, textUpward_forValueTexts, textAnchor_forValueTexts, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, autoFlipTheTextToPreventMirrorInvertedDisplay, 0.0f, true);

            Vector3 textDirectionOfTheValueTexts_thatHasBeenFinallyUsedByTheAutomaticTextOrientation_normalized = DrawText.parsedTextSpecs.used_textDirection_normalized; //-> After each call of "DrawText.Write()" some infos about the most currently written text can be accessed via the global static field "DrawText.parsedTextSpecs". In our case we request the "textDirection_forValueTexts", which we further up didn't specify by ourselfes to make use of Draw XXLs automatic text orientation. With that direction we can conveniently add an offset to the "textFromUser", which gets drawn in "DrawRangeSliderName_viaDrawXXL()".
            return textDirectionOfTheValueTexts_thatHasBeenFinallyUsedByTheAutomaticTextOrientation_normalized;
        }

        static void DrawRangeSliderName_viaDrawXXL(Vector3 textDirectionOfTheValueTexts_normalized, Vector3 positionOfRangeStart, Vector3 positionOfRangeEnd, Vector3 direction_normalized, float lenghtOfTheRangeLine, string textFromUser, float textSize, float relative_verticalTextPositionOffsetDistance_fromRangeLine, Color color)
        {
            Vector3 centerPosition_ofTheRangeLine = 0.5f * (positionOfRangeStart + positionOfRangeEnd);
            float textPositionOffsetDistance_fromRangeLine = relative_verticalTextPositionOffsetDistance_fromRangeLine * textSize;
            Vector3 textPositionOffset_fromRangeLine = (-textDirectionOfTheValueTexts_normalized) * textPositionOffsetDistance_fromRangeLine;
            Vector3 position_ofTextFromUser = centerPosition_ofTheRangeLine + textPositionOffset_fromRangeLine;
            Vector3 textDirection_forTextFromUser = Direction_goesFromLeftToRight_insideSceneViewScreen(direction_normalized) ? direction_normalized : (-direction_normalized);
            Vector3 textUp_forTextFromUser = default(Vector3); //-> If "textUp" is not specified, then it will be automatically aligned according to "DrawText.automaticTextOrientation". In this case only "textDirection_forTextFromUser" constrains the text orientation, while in "DrawRangeValuesAsText_viaDrawXXL()" only "textUp" constrains the text orientation.
            DrawText.TextAnchorDXXL textAnchor_forTextFromUser = DrawText.TextAnchorDXXL.UpperCenter;
            DrawBasics.LineStyle enclosingBoxLineStyle = DrawBasics.LineStyle.solid;
            float enclosingBox_lineWidth_relToTextSize = 0.0f;
            float enclosingBox_paddingSize_relToTextSize = 0.3f;
            float autoLineBreakWidth = lenghtOfTheRangeLine; //-> Ensure that text doesn't get bigger than the range span itself. This could also be achieved with the "forceRestrictTextBlockSizeToThisMaxTextWidth"-parameter, in which case the text doesn't make a line break, but becomes smaller to fit into the wanted size span.
            bool autoFlipTheTextToPreventMirrorInvertedDisplay = true;
            UtilitiesDXXL_Text.WriteFramed(textFromUser, position_ofTextFromUser, color, textSize, textDirection_forTextFromUser, textUp_forTextFromUser, textAnchor_forTextFromUser, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, 0.0f, 0.0f, autoLineBreakWidth, autoFlipTheTextToPreventMirrorInvertedDisplay, 0.0f, true);
        }

        static float GetScaledValueInsideRange(float lowerEndOfRange, float upperEndOfRange, Vector3 position, Vector3 direction_normalized, float size)
        {
            //calculate the scaled value similar as "Handles.ScaleSlider()" does it:      
            Event currentEvent = Event.current;
            current_mousePositionInScreenspace += currentEvent.delta;
            float distanceHowMuchTheHandleHasBeenDraggedWithTheMouse = relativeSizeOfPercentageSliderCap * HandleUtility.CalcLineTranslation(mousePositionInScreenspace_duringMouseDown, current_mousePositionInScreenspace, position, direction_normalized) / size;
            float rangeSpan = upperEndOfRange - lowerEndOfRange;
            float returnedValue = valueToShiftInsideRange_duringMouseDown + rangeSpan * distanceHowMuchTheHandleHasBeenDraggedWithTheMouse;
            float returnedValue_clamped = Mathf.Clamp(returnedValue, lowerEndOfRange, upperEndOfRange);
            return returnedValue_clamped;
        }

        static Vector3 startPosition_onDiscRadius;
        static float discsTurnAngleInDegrees_sinceMouseDown;
        public static Quaternion DiscWithAngleDisplay(Quaternion rotation, Vector3 position, Vector3 axis, float size, bool cutoffPlane, float snap)
        {
            //this is like Unitys "Handles.Disc" function, but additionally displays the changing angle as text.
            //Sidenote: As can be seen in Unitys Editor source code(link zu  https://github.com/Unity-Technologies/UnityCsReference/blob/2022.2/Editor/Mono/Handles.cs ) starting with Unity2022.2 Unitys "Handles" class has an additional overload for the "RotationHandle()" function, that takes the "RotationHandleIds ids"-parameter. This opens the possibility to extend this "DiscWithAngleDisplay()" function to the whole rotation handle with all of it's circles, without having to recreate the whole rotation handle by yourself.

            Event currentEvent = Event.current;

            if (currentEvent.type == EventType.MouseDown) //-> Detect the "MouseDown"-event BEFORE it gets eaten via "event.Use()" inside "Handles.Disc()". It will not be there anymore if this code is placed BELOW the "Handles.Disc" function call.
            {
                mousePositionInScreenspace_duringMouseDown = currentEvent.mousePosition;
                current_mousePositionInScreenspace = currentEvent.mousePosition;
                discsTurnAngleInDegrees_sinceMouseDown = 0.0f;
                startPosition_onDiscRadius = Get_startPosition_onDiscRadius(position, axis, size, cutoffPlane);
            }

            int controlID = GUIUtility.GetControlID(FocusType.Passive);
            bool discIsCurrentlyGrabbedByTheMouse = (GUIUtility.hotControl == controlID);

            if (currentEvent.type == EventType.MouseDrag) //-> Detect the "MouseDrag"-event BEFORE it gets eaten via "event.Use()" inside "Handles.Disc()". It will not be there anymore if this code is placed BELOW the "Handles.Disc" function call.
            {
                discsTurnAngleInDegrees_sinceMouseDown = Get_discsTurnAngleInDegrees_sinceMouseDown(position, axis, size, discIsCurrentlyGrabbedByTheMouse);
            }

            Quaternion returnedQuaternion = Handles.Disc(controlID, rotation, position, axis, size, cutoffPlane, snap);

            if (currentEvent.type == EventType.Repaint) //-> Draw with Draw XXL only inside the "Repaint"-event, otherwise the "DrawBasics.MaxAllowedDrawnLinesPerFrame"-mechanic will get confused and restricts the drawing earlier than neccessary.
            {
                if (discIsCurrentlyGrabbedByTheMouse)
                {
                    ConfigureDrawXXLsGlobalSettingsForDrawingHandles(); //-> This function is recommended to be used in conjunction with the "RevertDrawXXLsGlobalHandleSettingsToWhatTheyWereBefore()" function, to "encapsulate" everything that Draw XXL draws for Handles.
                    DrawAngleTextDisplay_viaDrawXXL(position, axis);
                    RevertDrawXXLsGlobalHandleSettingsToWhatTheyWereBefore(); //-> This function should be used in conjunction with the "ConfigureDrawXXLsGlobalSettingsForDrawingHandles()" function, to "encapsulate" everything that Draw XXL draws for Handles.
                }
            }

            return returnedQuaternion;

        }

        static Vector3 Get_startPosition_onDiscRadius(Vector3 position, Vector3 axis, float size, bool cutoffPlane)
        {
            if (cutoffPlane)
            {
                Vector3 forwardDirection_ofCurrentCamera = Camera.current != null ? Camera.current.transform.forward : Vector3.forward;
                Vector3 aVectorInsideTheDiscPlane = Vector3.Cross(axis, forwardDirection_ofCurrentCamera).normalized;
                return HandleUtility.ClosestPointToArc(position, axis, aVectorInsideTheDiscPlane, 180.0f, size);
            }
            else
            {
                return HandleUtility.ClosestPointToDisc(position, axis, size);
            }
        }

        static float Get_discsTurnAngleInDegrees_sinceMouseDown(Vector3 position, Vector3 axis, float size, bool discIsCurrentlyGrabbedByTheMouse)
        {
            if (discIsCurrentlyGrabbedByTheMouse)
            {
                //calculate the angle in the same way as "Handles.Disc()" does it:
                Event currentEvent = Event.current;
                Vector3 direction_normalized = Vector3.Cross(axis, position - startPosition_onDiscRadius).normalized;
                current_mousePositionInScreenspace += currentEvent.delta;
                return HandleUtility.CalcLineTranslation(mousePositionInScreenspace_duringMouseDown, current_mousePositionInScreenspace, startPosition_onDiscRadius, direction_normalized) / size * 30.0f;
            }
            return 0.0f;
        }

        static void DrawAngleTextDisplay_viaDrawXXL(Vector3 position, Vector3 axis)
        {
            string textOnCircledVector = Mathf.Abs(discsTurnAngleInDegrees_sinceMouseDown) + "°";
            string textOnCircledVector_enlarged = "<size=22>" + textOnCircledVector + "</size>"; //-> "size=11" is the default size, so this enlarges the text by a factor of 2.
            Color colorOfCircledVectorAndText = Handles.selectedColor; //-> When Draw XXL draws with Unitys Handles-Lines it doesn't automatically detect the "Handles.color", but instead takes the color that is specified as parameter argument in the draw function, or else falls back to "DrawBasics.defaultColor"
            float coneLength = 0.3f;
            bool skipFallbackDisplayOfZeroAngles = true; //-> If this is be "false", then Draw XXL would display a fallback text for angles of zero or nearby. In the use case here this fallback is not wanted.
            float minAngleDeg_withoutTextLineBreak = 120.0f; //-> If the angle is small, then the displayed text covers a bigger angle span than the angle itself. This would lead to automatic line breaks in the text display, which are prevented by setting this "minAngleDeg_withoutTextLineBreak" setting to a higher value.
            DrawBasics.VectorCircled(startPosition_onDiscRadius, position, axis, -discsTurnAngleInDegrees_sinceMouseDown, colorOfCircledVectorAndText, 0.0f, textOnCircledVector_enlarged, coneLength, false, skipFallbackDisplayOfZeroAngles, true, minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine);
        }

        public static float AnalogJoystickSlider(Vector3 position, Vector3 direction)
        {
            return AnalogJoystickSlider(position, direction, HandleUtility.GetHandleSize(position), Handles.SphereHandleCap);
        }


        static float travelledWorldSpaceDistance_sinceMouseDown;
        public static float AnalogJoystickSlider(Vector3 position, Vector3 direction, float size, Handles.CapFunction capFunction)
        {
            //This simulates the behaviour of an one-dimensional analog joystick. You can click and drag the slider and it will return a value between -1 and 1, depending on how much you protrude it from the center. As soon as you lift the mouse the slider will snap back to the zero position.

            int control_ID = GUIUtility.GetControlID(FocusType.Passive);
            Event currentEvent = Event.current;

            if (GUIUtility.hotControl == 0) { travelledWorldSpaceDistance_sinceMouseDown = 0.0f; }

            Vector3 direction_normalized; //-> is only calculated later for some events, to save some performance
            size = size * relativeSizeOfPercentageSliderCap;
            float maximumProtrusion_inWorldSpaceUnits = 10.0f * size;

            switch (currentEvent.GetTypeForControl(control_ID))
            {
                case EventType.MouseDown:
                    if ((HandleUtility.nearestControl == control_ID) && (currentEvent.button == 0) && (currentEvent.alt == false))
                    {
                        GUIUtility.hotControl = control_ID;
                        mousePositionInScreenspace_duringMouseDown = currentEvent.mousePosition;
                        current_mousePositionInScreenspace = currentEvent.mousePosition;
                        currentEvent.Use();
                        EditorGUIUtility.SetWantsMouseJumping(1);
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == control_ID)
                    {
                        travelledWorldSpaceDistance_sinceMouseDown = 0.0f;
                        GUIUtility.hotControl = 0;
                        currentEvent.Use();
                        EditorGUIUtility.SetWantsMouseJumping(0);
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == control_ID)
                    {
                        current_mousePositionInScreenspace = current_mousePositionInScreenspace + currentEvent.delta;
                        travelledWorldSpaceDistance_sinceMouseDown = HandleUtility.CalcLineTranslation(mousePositionInScreenspace_duringMouseDown, current_mousePositionInScreenspace, position, direction.normalized); //-> It's undocumented, but "HandleUtility.CalcLineTranslation()" expects a NORMALIZED direction. 
                        travelledWorldSpaceDistance_sinceMouseDown = Mathf.Clamp(travelledWorldSpaceDistance_sinceMouseDown, -maximumProtrusion_inWorldSpaceUnits, maximumProtrusion_inWorldSpaceUnits);
                        GUI.changed = true;
                        currentEvent.Use();
                    }
                    break;
                case EventType.Repaint:
                    Color color_before = Handles.color;
                    TrySetColorDuringMouseInteraction(control_ID, currentEvent);
                    direction_normalized = CalculateDirectionNormalized_forJoystickSlider(control_ID, direction);

                    float radiusOfMountingPointDisc = 0.2f * size;
                    Handles.DrawSolidDisc(position, GetSceneViewCamerasForwardDirection(), radiusOfMountingPointDisc);
                    TryDrawJoystickSpiralSpring(control_ID, travelledWorldSpaceDistance_sinceMouseDown, position, direction_normalized, maximumProtrusion_inWorldSpaceUnits);
                    DrawJoystickSliderCap(control_ID, position, direction_normalized, size, capFunction, EventType.Repaint);

                    Handles.color = color_before;
                    break;
                case EventType.Layout:
                    direction_normalized = CalculateDirectionNormalized_forJoystickSlider(control_ID, direction);
                    DrawJoystickSliderCap(control_ID, position, direction_normalized, size, capFunction, EventType.Layout);
                    break;
                default:
                    break;
            }

            return (travelledWorldSpaceDistance_sinceMouseDown / maximumProtrusion_inWorldSpaceUnits);
        }

        static void TrySetColorDuringMouseInteraction(int control_ID, Event currentEvent)
        {
            if (control_ID == GUIUtility.hotControl)
            {
                Handles.color = Handles.selectedColor;
            }
            else
            {
                if (IsHovering(control_ID, currentEvent))
                {
                    Handles.color = Handles.preselectionColor;
                }
            }
        }

        static bool IsHovering(int control_ID, Event currentEvent)
        {
            return ((GUIUtility.hotControl == 0) && (control_ID == HandleUtility.nearestControl) && (currentEvent.alt == false));
        }

        static Vector3 CalculateDirectionNormalized_forJoystickSlider(int control_ID, Vector3 direction)
        {
            if (control_ID == GUIUtility.hotControl)
            {
                return direction.normalized;
            }
            else
            {
                return direction; //-> is not required as normalized when the handle is not selected
            }
        }

        static void DrawJoystickSliderCap(int control_ID, Vector3 position, Vector3 direction_normalized, float size, Handles.CapFunction capFunction, EventType eventType)
        {
            Vector3 currentPosition_ofJoystickSlider = Get_currentPosition_ofJoystickSlider(travelledWorldSpaceDistance_sinceMouseDown, position, direction_normalized);
            Quaternion rotation = Quaternion.LookRotation(direction_normalized);
            capFunction(control_ID, currentPosition_ofJoystickSlider, rotation, size, eventType);
        }

        static void TryDrawJoystickSpiralSpring(int control_ID, float travelledWorldSpaceDistance_sinceMouseDown, Vector3 position, Vector3 direction_normalized, float maximumProtrusion_inWorldSpaceUnits)
        {
            if (GUIUtility.hotControl == control_ID)
            {
                if (Mathf.Approximately(travelledWorldSpaceDistance_sinceMouseDown, 0.0f) == false)
                {
                    DrawJoystickSpiralSpring(travelledWorldSpaceDistance_sinceMouseDown, position, direction_normalized, maximumProtrusion_inWorldSpaceUnits);
                }
            }
        }

        static void DrawJoystickSpiralSpring(float travelledWorldSpaceDistance_sinceMouseDown, Vector3 position, Vector3 direction_normalized, float maximumProtrusion_inWorldSpaceUnits)
        {
            ConfigureDrawXXLsGlobalSettingsForDrawingHandles(); //-> This function is recommended to be used in conjunction with the "RevertDrawXXLsGlobalHandleSettingsToWhatTheyWereBefore()" function, to "encapsulate" everything that Draw XXL draws for Handles.

            //Generated via code snippet:
            Vector3 start_of_spiral = position;
            Vector3 end_of_spiral = Get_currentPosition_ofJoystickSlider(travelledWorldSpaceDistance_sinceMouseDown, position, direction_normalized);
            float relaxedLength_of_spiral = maximumProtrusion_inWorldSpaceUnits;
            Color relaxedColor_of_spiral = Handles.color;
            DrawBasics.LineStyle style_of_spiral = DrawBasics.LineStyle.spiral;
            float stretchFactor_forStretchedTensionColor_of_spiral = 2.0f;
            Color color_forStretchedTension_of_spiral = Handles.color;
            float stretchFactor_forSqueezedTensionColor_of_spiral = 0.0f;
            Color color_forSqueezedTension_of_spiral = Handles.color;
            float width_of_spiral = 0.0f;
            string text_of_spiral = null;
            float alphaOfReferenceLengthDisplay_of_spiral = 0.5f;
            float stylePatternScaleFactor_of_spiral = 3.5f * maximumProtrusion_inWorldSpaceUnits;
            Vector3 customAmplitudeAndTextDir_of_spiral = default(Vector3);
            bool flattenThickRoundLineIntoAmplitudePlane_of_spiral = false;
            float endPlates_size_of_spiral = 0.0f;
            float enlargeSmallTextToThisMinTextSize_of_spiral = 0.0f;
            float durationInSec_of_spiral = 0.0f;
            bool hiddenByNearerObjects_of_spiral = true;
            bool skipPatternEnlargementForLongLines_of_spiral = true;
            bool skipPatternEnlargementForShortLines_of_spiral = true;

            float stylePatternScaleFactor_alongLineDir_ignoringAmplitude_before = DrawBasics.StylePatternScaleFactor_alongLineDir_ignoringAmplitude;
            DrawBasics.StylePatternScaleFactor_alongLineDir_ignoringAmplitude = 0.6f; //-> Instruct Draw XXL how to use this style scaling for all following draw operations
            DrawBasics.LineUnderTension(start_of_spiral, end_of_spiral, relaxedLength_of_spiral, relaxedColor_of_spiral, style_of_spiral, stretchFactor_forStretchedTensionColor_of_spiral, color_forStretchedTension_of_spiral, stretchFactor_forSqueezedTensionColor_of_spiral, color_forSqueezedTension_of_spiral, width_of_spiral, text_of_spiral, alphaOfReferenceLengthDisplay_of_spiral, stylePatternScaleFactor_of_spiral, customAmplitudeAndTextDir_of_spiral, flattenThickRoundLineIntoAmplitudePlane_of_spiral, endPlates_size_of_spiral, enlargeSmallTextToThisMinTextSize_of_spiral, durationInSec_of_spiral, hiddenByNearerObjects_of_spiral, skipPatternEnlargementForLongLines_of_spiral, skipPatternEnlargementForShortLines_of_spiral);
            DrawBasics.StylePatternScaleFactor_alongLineDir_ignoringAmplitude = stylePatternScaleFactor_alongLineDir_ignoringAmplitude_before; //-> Revert the Draw XXL setting to what it was before

            RevertDrawXXLsGlobalHandleSettingsToWhatTheyWereBefore(); //-> This function should be used in conjunction with the "ConfigureDrawXXLsGlobalSettingsForDrawingHandles()" function, to "encapsulate" everything that Draw XXL draws for Handles.
        }

        static Vector3 Get_currentPosition_ofJoystickSlider(float travelledWorldSpaceDistance_sinceMouseDown, Vector3 position, Vector3 direction_normalized)
        {
            return (position + direction_normalized * travelledWorldSpaceDistance_sinceMouseDown);
        }

        static Vector3 GetSceneViewCamerasForwardDirection()
        {
            if (SceneView.lastActiveSceneView == null)
            {
                return Vector3.forward;
            }
            else
            {
                return SceneView.lastActiveSceneView.camera.transform.forward;
            }
        }

        static bool Direction_goesFromLeftToRight_insideSceneViewScreen(Vector3 direction)
        {
            if (SceneView.lastActiveSceneView == null)
            {
                return false;
            }
            else
            {
                return (Vector3.Dot(direction, SceneView.lastActiveSceneView.camera.transform.right) > 0.0f);
            }
        }

#endif

    }

}
