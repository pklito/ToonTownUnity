namespace DrawXXL
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
    using DrawXXL;

    public class InternalDXXL_ChartHandles
    {
        static float valueToSlide_duringMouseDown;
        static Vector2 currentMousePosition;
        static Vector2 mousePosition_duringMouseDown;
        static Vector3 sliderPosition_worldSpace_duringMouseDown;

        public static float CursorSlider(bool isAtLowerEndOfCursorsVertLine_notAtUpperEnd, float valueToSlide, DrawXXLChartInspector theDrawXXLChartInspector_unserializedMonoB)
        {
            int control_ID = GUIUtility.GetControlID(FocusType.Passive);
            Event currentEvent = Event.current;
            Vector3 current_sliderPosition_worldSpace = Get_current_cursorSliderPosition_worldSpace(isAtLowerEndOfCursorsVertLine_notAtUpperEnd, theDrawXXLChartInspector_unserializedMonoB);
            switch (currentEvent.GetTypeForControl(control_ID))
            {
                case EventType.MouseDown:
                    if ((HandleUtility.nearestControl == control_ID) && (currentEvent.button == 0) && (currentEvent.alt == false))
                    {
                        GUIUtility.hotControl = control_ID;
                        valueToSlide_duringMouseDown = valueToSlide;
                        mousePosition_duringMouseDown = currentEvent.mousePosition;
                        currentMousePosition = currentEvent.mousePosition;
                        sliderPosition_worldSpace_duringMouseDown = current_sliderPosition_worldSpace;
                        currentEvent.Use();
                        EditorGUIUtility.SetWantsMouseJumping(1);
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == control_ID)
                    {
                        GUIUtility.hotControl = 0;
                        currentEvent.Use();
                        EditorGUIUtility.SetWantsMouseJumping(0);
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == control_ID)
                    {
                        currentMousePosition = currentMousePosition + currentEvent.delta;
                        Vector3 cursorSlideDirection = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.xAxis.AxisVector_normalized_inWorldSpace;
                        float travelledDistance_sinceMouseDown = HandleUtility.CalcLineTranslation(mousePosition_duringMouseDown, currentMousePosition, sliderPosition_worldSpace_duringMouseDown, cursorSlideDirection);
                        valueToSlide = valueToSlide_duringMouseDown + travelledDistance_sinceMouseDown;
                        GUI.changed = true;
                        currentEvent.Use();
                    }
                    break;
                case EventType.Repaint:
                    Color color_before = Handles.color;
                    TrySetColorDuringMouseInteraction(control_ID, theDrawXXLChartInspector_unserializedMonoB, currentEvent);
                    DrawConeCap(isAtLowerEndOfCursorsVertLine_notAtUpperEnd, control_ID, theDrawXXLChartInspector_unserializedMonoB, EventType.Repaint);
                    Handles.color = color_before;
                    break;
                case EventType.Layout:
                    DrawConeCap(isAtLowerEndOfCursorsVertLine_notAtUpperEnd, control_ID, theDrawXXLChartInspector_unserializedMonoB, EventType.Layout);
                    break;
                default:
                    break;
            }
            return valueToSlide;
        }

        static Vector3 Get_current_cursorSliderPosition_worldSpace(bool isAtLowerEndOfCursorsVertLine_notAtUpperEnd, DrawXXLChartInspector theDrawXXLChartInspector_unserializedMonoB)
        {
            return (isAtLowerEndOfCursorsVertLine_notAtUpperEnd ? theDrawXXLChartInspector_unserializedMonoB.GetCursorPosOnLowerEndOfChart() : theDrawXXLChartInspector_unserializedMonoB.GetCursorPosOnHigherEndOfChart());
        }

        static void DrawConeCap(bool isAtLowerEndOfCursorsVertLine_notAtUpperEnd, int control_ID, DrawXXLChartInspector theDrawXXLChartInspector_unserializedMonoB, EventType eventType)
        {
            Matrix4x4 matrix_before = Handles.matrix;

            float sizeOfCursorHandleCone = theDrawXXLChartInspector_unserializedMonoB.GetHeightOfCursorPyramid();
            Vector3 position_ofCustomMatrixSpace = Get_current_cursorSliderPosition_worldSpace(isAtLowerEndOfCursorsVertLine_notAtUpperEnd, theDrawXXLChartInspector_unserializedMonoB);
            Vector3 positionOfCone_insideCustomMatrixSpace = Vector3.forward * (0.5f * sizeOfCursorHandleCone);//-> shifting the cone, so that his base it at the "position_ofCustomMatrixSpace", and not his center
            Vector3 forwardOfCustomMatrixSpace = isAtLowerEndOfCursorsVertLine_notAtUpperEnd ? (theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.yAxis.AxisVector_normalized_inWorldSpace) : (-theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.yAxis.AxisVector_normalized_inWorldSpace);
            Quaternion rotation_ofCustomMatrixSpace = Quaternion.LookRotation(forwardOfCustomMatrixSpace);
            Quaternion rotationOfCone_insideCustomMatrixSpace = Quaternion.identity; //-> no rotation inside the custom matrix space. The cone rotation is already done by rotation the whole custom matrix space.
            float coneBase_scaleFactor = 3.0f;
            Vector3 scale_ofCustomMatrixSpace = new Vector3(coneBase_scaleFactor, coneBase_scaleFactor, 1.0f); //-> This is the reason for the whole custom matrix: Warping the cone shape, so that appears less pointy
            Matrix4x4 warpedMatrix_oriniatingAtConeBase = Matrix4x4.TRS(position_ofCustomMatrixSpace, rotation_ofCustomMatrixSpace, scale_ofCustomMatrixSpace);

            Handles.matrix = warpedMatrix_oriniatingAtConeBase;
            Handles.ConeHandleCap(control_ID, positionOfCone_insideCustomMatrixSpace, rotationOfCone_insideCustomMatrixSpace, sizeOfCursorHandleCone, eventType);
            Handles.matrix = matrix_before;
        }

        public static float OneDirectionalBacksnapSlider(float travelledWorldSpaceDistanceAlongDirection_sinceMouseDown, Vector3 restingPosition, DrawXXLChartInspector theDrawXXLChartInspector_unserializedMonoB, Vector3 sliderDirection_worldSpace_normalized, Handles.CapFunction capFunction, float capSizeScaleFactor, GUIContent icon, float iconSizeFactor, Vector2 iconPositionOffset_inScreenspace_relToHandleCapSize, bool clampTravelledDistance_inThePositiveDirection, bool drawSpiralSpring)
        {
            //-> see "HandlesExamples.AnalogJoystickSlider()" for a more generic version of this function

            int control_ID = GUIUtility.GetControlID(FocusType.Passive);
            Event currentEvent = Event.current;
            switch (currentEvent.GetTypeForControl(control_ID))
            {
                case EventType.MouseDown:
                    if ((HandleUtility.nearestControl == control_ID) && (currentEvent.button == 0) && (currentEvent.alt == false))
                    {
                        GUIUtility.hotControl = control_ID;
                        mousePosition_duringMouseDown = currentEvent.mousePosition;
                        currentMousePosition = currentEvent.mousePosition;
                        theDrawXXLChartInspector_unserializedMonoB.SaveZoomAndScrollState_onMouseDown();
                        currentEvent.Use();
                        EditorGUIUtility.SetWantsMouseJumping(1);
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == control_ID)
                    {
                        travelledWorldSpaceDistanceAlongDirection_sinceMouseDown = 0.0f;
                        GUIUtility.hotControl = 0;
                        currentEvent.Use();
                        EditorGUIUtility.SetWantsMouseJumping(0);
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == control_ID)
                    {
                        currentMousePosition = currentMousePosition + currentEvent.delta;
                        travelledWorldSpaceDistanceAlongDirection_sinceMouseDown = HandleUtility.CalcLineTranslation(mousePosition_duringMouseDown, currentMousePosition, restingPosition, sliderDirection_worldSpace_normalized);
                        travelledWorldSpaceDistanceAlongDirection_sinceMouseDown = TryClampTravelledDistanceInThePositiveDirection(clampTravelledDistance_inThePositiveDirection, travelledWorldSpaceDistanceAlongDirection_sinceMouseDown, theDrawXXLChartInspector_unserializedMonoB);
                        GUI.changed = true;
                        currentEvent.Use();
                    }
                    break;
                case EventType.Repaint:
                    Color color_before = Handles.color;
                    TrySetColorDuringMouseInteraction(control_ID, theDrawXXLChartInspector_unserializedMonoB, currentEvent);
                    TryDrawSpiralSpring(control_ID, drawSpiralSpring, travelledWorldSpaceDistanceAlongDirection_sinceMouseDown, restingPosition, theDrawXXLChartInspector_unserializedMonoB, sliderDirection_worldSpace_normalized);
                    DrawOneDimensionalBacksnapSliderCap(control_ID, travelledWorldSpaceDistanceAlongDirection_sinceMouseDown, restingPosition, theDrawXXLChartInspector_unserializedMonoB, sliderDirection_worldSpace_normalized, capFunction, capSizeScaleFactor, EventType.Repaint);
                    Handles.color = color_before;
                    break;
                case EventType.Layout:
                    DrawOneDimensionalBacksnapSliderCap(control_ID, travelledWorldSpaceDistanceAlongDirection_sinceMouseDown, restingPosition, theDrawXXLChartInspector_unserializedMonoB, sliderDirection_worldSpace_normalized, capFunction, capSizeScaleFactor, EventType.Layout);
                    break;
                default:
                    break;
            }

            DrawIcon_onOneDimensionalBacksnapSlider(travelledWorldSpaceDistanceAlongDirection_sinceMouseDown, restingPosition, theDrawXXLChartInspector_unserializedMonoB, sliderDirection_worldSpace_normalized, icon, iconSizeFactor, iconPositionOffset_inScreenspace_relToHandleCapSize);
            return travelledWorldSpaceDistanceAlongDirection_sinceMouseDown;
        }

        static float TryClampTravelledDistanceInThePositiveDirection(bool clampTravelledDistance_inThePositiveDirection, float travelledWorldSpaceDistanceAlongDirection_sinceMouseDown, DrawXXLChartInspector theDrawXXLChartInspector_unserializedMonoB)
        {
            if (clampTravelledDistance_inThePositiveDirection)
            {
                if (travelledWorldSpaceDistanceAlongDirection_sinceMouseDown > 0.0f)
                {
                    return Mathf.Min(travelledWorldSpaceDistanceAlongDirection_sinceMouseDown, theDrawXXLChartInspector_unserializedMonoB.GetBacksnapSliderReferenceLength_inWorldspaceUnits());
                }
            }
            return travelledWorldSpaceDistanceAlongDirection_sinceMouseDown;
        }

        static void TryDrawSpiralSpring(int control_ID, bool drawSpiralSpring, float travelledWorldSpaceDistanceAlongDirection_sinceMouseDown, Vector3 restingPosition, DrawXXLChartInspector theDrawXXLChartInspector_unserializedMonoB, Vector3 sliderDirection_worldSpace_normalized)
        {
            if (drawSpiralSpring)
            {
                if (GUIUtility.hotControl == control_ID)
                {
                    HandlesExamples.ConfigureDrawXXLsGlobalSettingsForDrawingHandles();

                    //Generated via code snippet/live template:
                    Vector3 start_of_spiral = restingPosition;
                    Vector3 end_of_spiral = Get_currentPosition_ofOneDimensionalBacksnapSlider(travelledWorldSpaceDistanceAlongDirection_sinceMouseDown, restingPosition, sliderDirection_worldSpace_normalized);
                    float relaxedLength_of_spiral = theDrawXXLChartInspector_unserializedMonoB.GetBacksnapSliderReferenceLength_inWorldspaceUnits();
                    Color relaxedColor_of_spiral = Handles.color;
                    DrawBasics.LineStyle style_of_spiral = DrawBasics.LineStyle.spiral;
                    float stretchFactor_forStretchedTensionColor_of_spiral = 2.0f;
                    Color color_forStretchedTension_of_spiral = Handles.color;
                    float stretchFactor_forSqueezedTensionColor_of_spiral = 0.0f;
                    Color color_forSqueezedTension_of_spiral = Handles.color;
                    float width_of_spiral = 0.0f;
                    string text_of_spiral = null;
                    float alphaOfReferenceLengthDisplay_of_spiral = 0.5f;
                    float stylePatternScaleFactor_of_spiral = 0.7f * (theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.Width_inWorldSpace + theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.Height_inWorldSpace);
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

                    //Generated via code snippet/live template:
                    Vector3 position_of_dot = restingPosition;
                    float radius_of_dot = 0.003f * stylePatternScaleFactor_of_spiral;
                    Vector3 normal_of_dot = default(Vector3); //-> this will automatically point to the Scene View Camera, because "DrawShapes.automaticOrientationOfFlatShapes" has been set accordingly inside "HandlesExamples.ConfigureDrawXXLsGlobalSettingsForDrawingHandles()"
                    Color color_of_dot = Handles.color;
                    string text_of_dot = null;
                    float density_of_dot = 1.0f;
                    float durationInSec_of_dot = 0.0f;
                    bool hiddenByNearerObjects_of_dot = true;
                    DrawBasics.Dot(position_of_dot, radius_of_dot, normal_of_dot, color_of_dot, text_of_dot, density_of_dot, durationInSec_of_dot, hiddenByNearerObjects_of_dot);

                    HandlesExamples.RevertDrawXXLsGlobalHandleSettingsToWhatTheyWereBefore();
                }
            }
        }

        static void DrawOneDimensionalBacksnapSliderCap(int control_ID, float travelledWorldSpaceDistanceAlongDirection_sinceMouseDown, Vector3 restingPosition, DrawXXLChartInspector theDrawXXLChartInspector_unserializedMonoB, Vector3 sliderDirection_worldSpace_normalized, Handles.CapFunction capFunction, float capSizeScaleFactor, EventType eventType)
        {
            Vector3 currentPosition_ofBacksnapSlider = Get_currentPosition_ofOneDimensionalBacksnapSlider(travelledWorldSpaceDistanceAlongDirection_sinceMouseDown, restingPosition, sliderDirection_worldSpace_normalized);
            float size = GetSizeOfButtonCaps_withoutWeightFactorFromCapTypeApplied(theDrawXXLChartInspector_unserializedMonoB) * capSizeScaleFactor;
            Quaternion rotation = Quaternion.LookRotation(sliderDirection_worldSpace_normalized);
            capFunction(control_ID, currentPosition_ofBacksnapSlider, rotation, size, eventType);
        }

        static void DrawIcon_onOneDimensionalBacksnapSlider(float travelledWorldSpaceDistanceAlongDirection_sinceMouseDown, Vector3 restingPosition, DrawXXLChartInspector theDrawXXLChartInspector_unserializedMonoB, Vector3 sliderDirection_worldSpace_normalized, GUIContent icon, float iconSizeFactor, Vector2 iconPositionOffset_inScreenspace_relToHandleCapSize)
        {
            if (SceneView.lastActiveSceneView != null)
            {
                float handleCapSize_inWorldSpace = GetSizeOfButtonCaps_withoutWeightFactorFromCapTypeApplied(theDrawXXLChartInspector_unserializedMonoB);

                Vector3 worldSpaceOffset_thatShiftsTheIconAlongScreenspaceXDir = SceneView.lastActiveSceneView.camera.transform.right * handleCapSize_inWorldSpace * iconPositionOffset_inScreenspace_relToHandleCapSize.x;
                Vector3 worldSpaceOffset_thatShiftsTheIconAlongScreenspaceYDir = SceneView.lastActiveSceneView.camera.transform.up * handleCapSize_inWorldSpace * iconPositionOffset_inScreenspace_relToHandleCapSize.y;
                Vector3 iconPosition = Get_currentPosition_ofOneDimensionalBacksnapSlider(travelledWorldSpaceDistanceAlongDirection_sinceMouseDown, restingPosition, sliderDirection_worldSpace_normalized) + worldSpaceOffset_thatShiftsTheIconAlongScreenspaceXDir + worldSpaceOffset_thatShiftsTheIconAlongScreenspaceYDir;
                float iconSize_asFloat_inWorldSpace = handleCapSize_inWorldSpace * iconSizeFactor;
                Vector2 iconSize_inPixels_asVector2 = ConvertIconSize_fromWorldspaceSize_toPixelSize(iconSize_asFloat_inWorldSpace, iconPosition);

                Vector2 iconSize_before = EditorGUIUtility.GetIconSize();
                EditorGUIUtility.SetIconSize(iconSize_inPixels_asVector2);
                Handles.Label(iconPosition, icon);
                EditorGUIUtility.SetIconSize(iconSize_before);
            }
        }

        static Vector3 Get_currentPosition_ofOneDimensionalBacksnapSlider(float travelledWorldSpaceDistanceAlongDirection_sinceMouseDown, Vector3 restingPosition, Vector3 sliderDirection_worldSpace_normalized)
        {
            return (restingPosition + sliderDirection_worldSpace_normalized * travelledWorldSpaceDistanceAlongDirection_sinceMouseDown);
        }

        public static void TwoDirectionalBacksnapSlider(ref float travelledWorldSpaceDistanceAlongChartsXDirection_sinceMouseDown, ref float travelledWorldSpaceDistanceAlongChartsYDirection_sinceMouseDown, Vector3 restingPosition, DrawXXLChartInspector theDrawXXLChartInspector_unserializedMonoB, float capSizeScaleFactor, GUIContent icon, float iconSizeFactor, Vector2 iconPositionOffset_inScreenspace_relToHandleCapSize)
        {
            int control_ID = GUIUtility.GetControlID(FocusType.Passive);
            Event currentEvent = Event.current;
            switch (currentEvent.GetTypeForControl(control_ID))
            {
                case EventType.MouseDown:
                    if ((HandleUtility.nearestControl == control_ID) && (currentEvent.button == 0) && (currentEvent.alt == false))
                    {
                        GUIUtility.hotControl = control_ID;
                        mousePosition_duringMouseDown = currentEvent.mousePosition;
                        currentMousePosition = currentEvent.mousePosition;
                        theDrawXXLChartInspector_unserializedMonoB.SaveZoomAndScrollState_onMouseDown();
                        currentEvent.Use();
                        EditorGUIUtility.SetWantsMouseJumping(1);
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == control_ID)
                    {
                        travelledWorldSpaceDistanceAlongChartsXDirection_sinceMouseDown = 0.0f;
                        travelledWorldSpaceDistanceAlongChartsYDirection_sinceMouseDown = 0.0f;
                        GUIUtility.hotControl = 0;
                        currentEvent.Use();
                        EditorGUIUtility.SetWantsMouseJumping(0);
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == control_ID)
                    {
                        currentMousePosition = currentMousePosition + currentEvent.delta;
                        travelledWorldSpaceDistanceAlongChartsXDirection_sinceMouseDown = HandleUtility.CalcLineTranslation(mousePosition_duringMouseDown, currentMousePosition, restingPosition, theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.xAxis.AxisVector_normalized_inWorldSpace);
                        travelledWorldSpaceDistanceAlongChartsYDirection_sinceMouseDown = HandleUtility.CalcLineTranslation(mousePosition_duringMouseDown, currentMousePosition, restingPosition, theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.yAxis.AxisVector_normalized_inWorldSpace);
                        GUI.changed = true;
                        currentEvent.Use();
                    }
                    break;
                case EventType.Repaint:
                    Color color_before = Handles.color;
                    TrySetColorDuringMouseInteraction(control_ID, theDrawXXLChartInspector_unserializedMonoB, currentEvent);
                    DrawTwoDimensionalBacksnapSliderCap(control_ID, travelledWorldSpaceDistanceAlongChartsXDirection_sinceMouseDown, travelledWorldSpaceDistanceAlongChartsYDirection_sinceMouseDown, restingPosition, theDrawXXLChartInspector_unserializedMonoB, capSizeScaleFactor, EventType.Repaint);
                    Handles.color = color_before;
                    break;
                case EventType.Layout:
                    DrawTwoDimensionalBacksnapSliderCap(control_ID, travelledWorldSpaceDistanceAlongChartsXDirection_sinceMouseDown, travelledWorldSpaceDistanceAlongChartsYDirection_sinceMouseDown, restingPosition, theDrawXXLChartInspector_unserializedMonoB, capSizeScaleFactor, EventType.Layout);
                    break;
                default:
                    break;
            }

            DrawIcon_onTwoDimensionalBacksnapSlider(travelledWorldSpaceDistanceAlongChartsXDirection_sinceMouseDown, travelledWorldSpaceDistanceAlongChartsYDirection_sinceMouseDown, restingPosition, theDrawXXLChartInspector_unserializedMonoB, icon, iconSizeFactor, iconPositionOffset_inScreenspace_relToHandleCapSize);
        }

        static void DrawTwoDimensionalBacksnapSliderCap(int control_ID, float travelledWorldSpaceDistanceAlongChartsXDirection_sinceMouseDown, float travelledWorldSpaceDistanceAlongChartsYDirection_sinceMouseDown, Vector3 restingPosition, DrawXXLChartInspector theDrawXXLChartInspector_unserializedMonoB, float capSizeScaleFactor, EventType eventType)
        {
            Vector3 currentPosition_ofBacksnapSlider = Get_currentPosition_ofTwoDimensionalBacksnapSlider(travelledWorldSpaceDistanceAlongChartsXDirection_sinceMouseDown, travelledWorldSpaceDistanceAlongChartsYDirection_sinceMouseDown, restingPosition, theDrawXXLChartInspector_unserializedMonoB);
            float size = GetSizeOfButtonCaps_withoutWeightFactorFromCapTypeApplied(theDrawXXLChartInspector_unserializedMonoB) * capSizeScaleFactor;
            Handles.SphereHandleCap(control_ID, currentPosition_ofBacksnapSlider, Quaternion.identity, size, eventType);
        }

        static float GetSizeOfButtonCaps_withoutWeightFactorFromCapTypeApplied(DrawXXLChartInspector theDrawXXLChartInspector_unserializedMonoB)
        {
            return theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.xAxis.Get_fixedConeLength_forBothAxisVectors();
        }

        static void DrawIcon_onTwoDimensionalBacksnapSlider(float travelledWorldSpaceDistanceAlongChartsXDirection_sinceMouseDown, float travelledWorldSpaceDistanceAlongChartsYDirection_sinceMouseDown, Vector3 restingPosition, DrawXXLChartInspector theDrawXXLChartInspector_unserializedMonoB, GUIContent icon, float iconSizeFactor, Vector2 iconPositionOffset_inScreenspace_relToHandleCapSize)
        {
            if (SceneView.lastActiveSceneView != null)
            {
                float handleCapSize_inWorldSpace = GetSizeOfButtonCaps_withoutWeightFactorFromCapTypeApplied(theDrawXXLChartInspector_unserializedMonoB);

                Vector3 worldSpaceOffset_thatShiftsTheIconAlongScreenspaceXDir = SceneView.lastActiveSceneView.camera.transform.right * handleCapSize_inWorldSpace * iconPositionOffset_inScreenspace_relToHandleCapSize.x;
                Vector3 worldSpaceOffset_thatShiftsTheIconAlongScreenspaceYDir = SceneView.lastActiveSceneView.camera.transform.up * handleCapSize_inWorldSpace * iconPositionOffset_inScreenspace_relToHandleCapSize.y;
                Vector3 iconPosition = Get_currentPosition_ofTwoDimensionalBacksnapSlider(travelledWorldSpaceDistanceAlongChartsXDirection_sinceMouseDown, travelledWorldSpaceDistanceAlongChartsYDirection_sinceMouseDown, restingPosition, theDrawXXLChartInspector_unserializedMonoB) + worldSpaceOffset_thatShiftsTheIconAlongScreenspaceXDir + worldSpaceOffset_thatShiftsTheIconAlongScreenspaceYDir;
                float iconSize_asFloat_inWorldSpace = handleCapSize_inWorldSpace * iconSizeFactor;
                Vector2 iconSize_inPixels_asVector2 = ConvertIconSize_fromWorldspaceSize_toPixelSize(iconSize_asFloat_inWorldSpace, iconPosition);

                Vector2 iconSize_before = EditorGUIUtility.GetIconSize();
                EditorGUIUtility.SetIconSize(iconSize_inPixels_asVector2);
                Handles.Label(iconPosition, icon);
                EditorGUIUtility.SetIconSize(iconSize_before);
            }
        }

        static Vector3 Get_currentPosition_ofTwoDimensionalBacksnapSlider(float travelledWorldSpaceDistanceAlongChartsXDirection_sinceMouseDown, float travelledWorldSpaceDistanceAlongChartsYDirection_sinceMouseDown, Vector3 restingPosition, DrawXXLChartInspector theDrawXXLChartInspector_unserializedMonoB)
        {
            Vector3 dragOffset_alongXAxis = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.xAxis.AxisVector_normalized_inWorldSpace * travelledWorldSpaceDistanceAlongChartsXDirection_sinceMouseDown;
            Vector3 dragOffset_alongYAxis = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.yAxis.AxisVector_normalized_inWorldSpace * travelledWorldSpaceDistanceAlongChartsYDirection_sinceMouseDown;
            return (restingPosition + dragOffset_alongXAxis + dragOffset_alongYAxis);
        }


        static Vector2 ConvertIconSize_fromWorldspaceSize_toPixelSize(float iconSize_asFloat_inWorldSpace, Vector3 iconPosition)
        {
            float iconSize_asFloat_inScreenspace0to1 = UtilitiesDXXL_Screenspace.WorldSpaceExtent_to_viewportSpaceExtentRelToScreenHeight(SceneView.lastActiveSceneView.camera, iconPosition, iconSize_asFloat_inWorldSpace);
            float iconSize_inPixels = iconSize_asFloat_inScreenspace0to1 * SceneView.lastActiveSceneView.camera.pixelHeight;
            return new Vector2(iconSize_inPixels, iconSize_inPixels);
        }

        static void TrySetColorDuringMouseInteraction(int control_ID, DrawXXLChartInspector theDrawXXLChartInspector_unserializedMonoB, Event currentEvent)
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
                else
                {
                    Handles.color = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.color;
                }
            }
        }

        static bool IsHovering(int control_ID, Event currentEvent)
        {
            return ((GUIUtility.hotControl == 0) && (control_ID == HandleUtility.nearestControl) && (currentEvent.alt == false));
        }

        public static bool ShowAllButton(bool checkmarkState, Vector3 position, DrawXXLChartInspector theDrawXXLChartInspector_unserializedMonoB, float sizeScaleFactor, GUIContent checkmarkSymbol, GUIContent crossSymbol, float sizeFactor_forCheckmarkIcon, float sizeFactor_forCrossIcon, Vector2 checkmarkIconPositionOffset_inScreenspace_relToHandleCapSize, Vector2 crossIconPositionOffset_inScreenspace_relToHandleCapSize)
        {
            if (SceneView.lastActiveSceneView == null)
            {
                return checkmarkState;
            }
            else
            {
                Color handlesColor_before = Handles.color;

                float size_withoutWeightFactorFromCapTypeApplied = GetSizeOfButtonCaps_withoutWeightFactorFromCapTypeApplied(theDrawXXLChartInspector_unserializedMonoB);
                float size_withWeightFactorFromCapTypeAlreadyApplied = size_withoutWeightFactorFromCapTypeApplied * sizeScaleFactor;
                float radius_ofButton = 0.5f * size_withWeightFactorFromCapTypeAlreadyApplied;//"0.5f" factor because "Handles.CircleHandleCap" as a 2D cap seems to interpret the specified size as "radius" differently form the 3D caps that interpret it as "diameter".
                Handles.color = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.color;
                Vector3 normalOfSolidDisc = (-SceneView.lastActiveSceneView.camera.transform.forward);
                Handles.DrawSolidDisc(position, normalOfSolidDisc, radius_ofButton);

                GUIContent displayedIcon;
                Vector3 worldSpaceOffset_thatShiftsTheIconAlongScreenspaceXDir;
                Vector3 worldSpaceOffset_thatShiftsTheIconAlongScreenspaceYDir;
                float iconSize_asFloat_inWorldSpace;
                if (checkmarkState == true)
                {
                    worldSpaceOffset_thatShiftsTheIconAlongScreenspaceXDir = SceneView.lastActiveSceneView.camera.transform.right * size_withoutWeightFactorFromCapTypeApplied * checkmarkIconPositionOffset_inScreenspace_relToHandleCapSize.x;
                    worldSpaceOffset_thatShiftsTheIconAlongScreenspaceYDir = SceneView.lastActiveSceneView.camera.transform.up * size_withoutWeightFactorFromCapTypeApplied * checkmarkIconPositionOffset_inScreenspace_relToHandleCapSize.y;
                    displayedIcon = checkmarkSymbol;
                    iconSize_asFloat_inWorldSpace = size_withoutWeightFactorFromCapTypeApplied * sizeFactor_forCheckmarkIcon;
                }
                else
                {
                    worldSpaceOffset_thatShiftsTheIconAlongScreenspaceXDir = SceneView.lastActiveSceneView.camera.transform.right * size_withoutWeightFactorFromCapTypeApplied * crossIconPositionOffset_inScreenspace_relToHandleCapSize.x;
                    worldSpaceOffset_thatShiftsTheIconAlongScreenspaceYDir = SceneView.lastActiveSceneView.camera.transform.up * size_withoutWeightFactorFromCapTypeApplied * crossIconPositionOffset_inScreenspace_relToHandleCapSize.y;
                    displayedIcon = crossSymbol;
                    iconSize_asFloat_inWorldSpace = size_withoutWeightFactorFromCapTypeApplied * sizeFactor_forCrossIcon;
                }
                Vector3 posOfDisplayedIcon = position + worldSpaceOffset_thatShiftsTheIconAlongScreenspaceXDir + worldSpaceOffset_thatShiftsTheIconAlongScreenspaceYDir;
                Vector2 iconSize_inPixels_asVector2 = ConvertIconSize_fromWorldspaceSize_toPixelSize(iconSize_asFloat_inWorldSpace, posOfDisplayedIcon);

                Vector2 iconSize_before = EditorGUIUtility.GetIconSize();
                EditorGUIUtility.SetIconSize(iconSize_inPixels_asVector2);
                Handles.Label(posOfDisplayedIcon, displayedIcon);
                EditorGUIUtility.SetIconSize(iconSize_before);

                GUIStyle styleFor_showAllText = new GUIStyle();
                float textSize_asFloat_inScreenspace0to1 = UtilitiesDXXL_Screenspace.WorldSpaceExtent_to_viewportSpaceExtentRelToScreenHeight(SceneView.lastActiveSceneView.camera, position, 0.7f * size_withoutWeightFactorFromCapTypeApplied);
                float textSize_inPixels = textSize_asFloat_inScreenspace0to1 * SceneView.lastActiveSceneView.camera.pixelHeight;
                styleFor_showAllText.fontSize = (int)textSize_inPixels;
                //styleFor_showAllText.alignment = TextAnchor.UpperCenter; //-> is this a bug in Unity? All anchors with "right" behave as it would be "left". Also "Center" is not really the center, but slightly shifted. I place the text at a manually shifted position as a fallback.
                string showAll_labelText = DrawText.MarkupColor("Show all", theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.color);
                Vector3 positionOfText = position - SceneView.lastActiveSceneView.camera.transform.right * 3.4f * size_withWeightFactorFromCapTypeAlreadyApplied+ SceneView.lastActiveSceneView.camera.transform.up * 0.4f * size_withWeightFactorFromCapTypeAlreadyApplied;
                Handles.Label(positionOfText, showAll_labelText, styleFor_showAllText);

                Handles.color = UtilitiesDXXL_Colors.GetSimilarColorWithOtherBrightnessValue(theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.color);
                Quaternion rotation_ofButton = Quaternion.LookRotation(normalOfSolidDisc);
                bool buttonHasBeenClicked = Handles.Button(position, rotation_ofButton, radius_ofButton, radius_ofButton, Handles.CircleHandleCap);
                if (buttonHasBeenClicked)
                {
                    checkmarkState = !checkmarkState;
                }

                Handles.color = handlesColor_before;

                return checkmarkState;
            }
        }

    }

#endif
}
