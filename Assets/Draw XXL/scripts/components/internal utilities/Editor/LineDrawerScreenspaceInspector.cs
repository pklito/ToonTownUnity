namespace DrawXXL
{
    using System;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(LineDrawerScreenspace))]
    [CanEditMultipleObjects]
    public class LineDrawerScreenspaceInspector : VisualizerScreenspaceParentInspector
    {
        LineDrawerScreenspace lineDrawerScreenspaceMonoBehaviour_unserialized;
        void OnEnable()
        {
            OnEnable_base();
            OnEnable_ofScreenspaceParent();
            OnEnable_ofLineDrawerScreenspaceInspector();
        }

        void OnEnable_ofLineDrawerScreenspaceInspector()
        {
            lineDrawerScreenspaceMonoBehaviour_unserialized = (LineDrawerScreenspace)target;
        }

        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("screenspace line");
            if (DrawCameraChooser(true))
            {
                GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

                SerializedProperty sP_lineType = serializedObject.FindProperty("lineType");
                EditorGUILayout.PropertyField(sP_lineType, new GUIContent("Line type"));

                SerializedProperty sP_lineDefinitionMode = serializedObject.FindProperty("lineDefinitionMode");
                EditorGUILayout.PropertyField(sP_lineDefinitionMode, new GUIContent("Line definition mode"));

                GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

                Draw_startPos_endPos_lineDirection_definingSection(sP_lineDefinitionMode);
                TryDrawTensionSpecs(sP_lineType);
                DrawColors(sP_lineType);
                DrawWidthAndDependentParameters(sP_lineType);
                TryDrawExtendedLines_checkBoxForDistanceOutsideScreen(sP_lineType);
                TryDrawLineStyleAndDependentParameters(sP_lineType);
                TryDrawAnimationSection(sP_lineType);

                if (sP_lineType.enumValueIndex != (int)LineDrawer.LineType.vectorWithExtention)
                {
                    TryDrawConeConfig(sP_lineType);
                    Draw_endPlatesConfig();
                    TryDrawAlphaFadeOut(sP_lineType);
                }

                DrawTextSpecs(sP_lineType);
                DrawCheckboxFor_drawOnlyIfSelected("screenspace line");
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }

        void Draw_startPos_endPos_lineDirection_definingSection(SerializedProperty sP_lineDefinitionMode)
        {
            SerializedProperty sP_lineDefinitionSection1_isOutfolded = serializedObject.FindProperty("lineDefinitionSection1_isOutfolded");
            SerializedProperty sP_lineDefinitionSection2_isOutfolded = serializedObject.FindProperty("lineDefinitionSection2_isOutfolded");

            GUIStyle style_ofFoldoutWithRichtext = new GUIStyle(EditorStyles.foldout);
            style_ofFoldoutWithRichtext.richText = true;
            string headline_of_directionVectorChooserSections = "<b>Vector to End Position</b>";
            switch (sP_lineDefinitionMode.enumValueIndex)
            {
                case (int)LineDrawer.LineDefinitionMode.startPositionAndEndPosition:
                    Draw_startPositionSection(sP_lineDefinitionSection1_isOutfolded, style_ofFoldoutWithRichtext);
                    Draw_endPositionSection(sP_lineDefinitionSection2_isOutfolded, style_ofFoldoutWithRichtext);
                    break;
                case (int)LineDrawer.LineDefinitionMode.startPositionAndDirectionVectorToEndPosition:
                    Draw_startPositionSection(sP_lineDefinitionSection1_isOutfolded, style_ofFoldoutWithRichtext);
                    Draw_lineVectorSection(sP_lineDefinitionSection2_isOutfolded, headline_of_directionVectorChooserSections, style_ofFoldoutWithRichtext);
                    break;
                case (int)LineDrawer.LineDefinitionMode.endPositionAndDirectionVectorToIt:
                    Draw_lineVectorSection(sP_lineDefinitionSection1_isOutfolded, headline_of_directionVectorChooserSections, style_ofFoldoutWithRichtext);
                    Draw_endPositionSection(sP_lineDefinitionSection2_isOutfolded, style_ofFoldoutWithRichtext);
                    break;
                default:
                    break;
            }

            if (sP_lineDefinitionSection2_isOutfolded.boolValue == false)
            {
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }
        }

        void Draw_startPositionSection(SerializedProperty sP_lineDefinitionSection1_isOutfolded, GUIStyle style_ofFoldoutWithRichtext)
        {
            sP_lineDefinitionSection1_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_lineDefinitionSection1_isOutfolded.boolValue, "<b>Start Position</b>", true, style_ofFoldoutWithRichtext);
            if (sP_lineDefinitionSection1_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("positionInsideViewport0to1"), new GUIContent("Position (inside viewport)"));
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);
            }
        }

        void Draw_endPositionSection(SerializedProperty sP_lineDefinitionSection2_isOutfolded, GUIStyle style_ofFoldoutWithRichtext)
        {
            sP_lineDefinitionSection2_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_lineDefinitionSection2_isOutfolded.boolValue, "<b>End Position</b>", true, style_ofFoldoutWithRichtext);
            if (sP_lineDefinitionSection2_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("positionInsideViewport0to1_v2"), new GUIContent("Position (inside viewport)"));
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);
            }
        }

        void Draw_lineVectorSection(SerializedProperty sP_lineDefinitionSection_isOutfolded, string headline, GUIStyle style_ofFoldoutWithRichtext)
        {
            sP_lineDefinitionSection_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_lineDefinitionSection_isOutfolded.boolValue, headline, true, style_ofFoldoutWithRichtext);
            if (sP_lineDefinitionSection_isOutfolded.boolValue)
            {
                if (serializedObject.FindProperty("source_ofCustomVector2_1").enumValueIndex != (int)VisualizerParent.CustomVector2Source.rotationAroundZStartingFromRight)
                {
                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("interpretDirectionAsUnwarped"), new GUIContent("Unwarped direction interpretation", "In most cases the viewport space from 0 to 1 is not a square but a rectangle with an uneven aspect ratio (since the display itself is mostly not a square). That means a vector like ( x=1 / y=1 ) does not raise with 45° but appears somehow warped to a different angle." + Environment.NewLine + Environment.NewLine + "You can activate this if you want the values here to be interpreted as if they would be in a square space, so the ( x=1 / y=1 ) vector will appear as a 45° line."));
                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                }

                DrawSpecificationOf_customVector2_1(null, false, null, false, false, false, false, true, true, true);
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }
        }

        void TryDrawTensionSpecs(SerializedProperty sP_lineType)
        {
            if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.lineUnderTension)
            {
                SerializedProperty sP_relaxedLength_relToViewportHeight = serializedObject.FindProperty("relaxedLength_relToViewportHeight");
                float lineLength_relToViewportHeight = lineDrawerScreenspaceMonoBehaviour_unserialized.GetLineLength_relToViewportHeight();

                EditorGUILayout.PropertyField(sP_relaxedLength_relToViewportHeight, new GUIContent("Relaxed Length", tooltip_explaining_relativeToViewPortHeight + Environment.NewLine + Environment.NewLine + "This is the reference length for how the tension appears. If the line has a length of this value, then it appears with its relaxed color and the line style isn't stretched or squeezed. Other line lengths will stretch and squeeze the line style and change the color, as if the line is under tension like a spring."));

                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.FloatField(new GUIContent("Current Line Length", tooltip_explaining_relativeToViewPortHeight), lineLength_relToViewportHeight);
                EditorGUILayout.FloatField(new GUIContent("Current Stretch Factor"), lineLength_relToViewportHeight / sP_relaxedLength_relToViewportHeight.floatValue);
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("alphaOfReferenceLengthDisplay"), new GUIContent("Alpha of reference length display"));

                GUILayout.Space(EditorGUIUtility.singleLineHeight);

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        void DrawColors(SerializedProperty sP_lineType)
        {
            if (sP_lineType.enumValueIndex != (int)LineDrawer.LineType.lineUnderTension)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("startColor"), new GUIContent("Color"));
            }

            if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.standardLine)
            {
                GUILayout.BeginHorizontal();
                SerializedProperty sP_useDifferentEndColor = serializedObject.FindProperty("useDifferentEndColor");
                EditorGUILayout.PropertyField(sP_useDifferentEndColor, new GUIContent("End Color"));
                EditorGUI.BeginDisabledGroup(!sP_useDifferentEndColor.boolValue);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("endColor"), GUIContent.none);
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();
            }

            if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.blinkingLine)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("blinkColor"), new GUIContent("Blink Color"));
            }

            if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.lineUnderTension)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("relaxedColor"), new GUIContent("Relaxed Color"));

                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("color_forStretchedTension"), new GUIContent("Stretched Color"));
                SerializedProperty sP_stretchFactor_forStretchedTensionColor = serializedObject.FindProperty("stretchFactor_forStretchedTensionColor");
                EditorGUILayout.PropertyField(sP_stretchFactor_forStretchedTensionColor, new GUIContent("appearing at stretch factor of"));
                sP_stretchFactor_forStretchedTensionColor.floatValue = Mathf.Max(sP_stretchFactor_forStretchedTensionColor.floatValue, 1.001f);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("color_forSqueezedTension"), new GUIContent("Squeezed Color"));
                SerializedProperty sP_stretchFactor_forSqueezedTensionColor = serializedObject.FindProperty("stretchFactor_forSqueezedTensionColor");
                EditorGUILayout.PropertyField(sP_stretchFactor_forSqueezedTensionColor, new GUIContent("appearing at stretch factor of"));
                sP_stretchFactor_forSqueezedTensionColor.floatValue = Mathf.Max(sP_stretchFactor_forSqueezedTensionColor.floatValue, 0.0f);
                sP_stretchFactor_forSqueezedTensionColor.floatValue = Mathf.Min(sP_stretchFactor_forSqueezedTensionColor.floatValue, 0.999f);
                GUILayout.EndHorizontal();

                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }

            if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.lineWithAlternatingColors)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("alternatingColor"), new GUIContent("Alternating Color"));
            }
        }

        void DrawWidthAndDependentParameters(SerializedProperty sP_lineType)
        {
            if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.movingArrowsLine)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("lineWidth_ofMovingArrowsLine_relToViewportHeight"), new GUIContent("Width", tooltip_explaining_relativeToViewPortHeight));

                string autoRoundingExplanationTooltip = "Very small (or very big) values may get rounded up (down) internally to prevent an explosive raise of drawn lines, see also 'DrawXXL.DrawBasics.MaxAllowedDrawnLinesPerFrame'." + Environment.NewLine + Environment.NewLine + "This automatic rounding has interdepency with 'Width' and ";

                SerializedProperty sP_distanceBetweenArrows_relToViewportHeight = serializedObject.FindProperty("distanceBetweenArrows_relToViewportHeight");
                SerializedProperty sP_lengthOfArrows_relToViewportHeight = serializedObject.FindProperty("lengthOfArrows_relToViewportHeight");

                EditorGUILayout.PropertyField(sP_distanceBetweenArrows_relToViewportHeight, new GUIContent("Distance between arrows", tooltip_explaining_relativeToViewPortHeight + Environment.NewLine + Environment.NewLine + autoRoundingExplanationTooltip + "'Length of Arrows'."));
                EditorGUILayout.PropertyField(sP_lengthOfArrows_relToViewportHeight, new GUIContent("Length of Arrows", tooltip_explaining_relativeToViewPortHeight + Environment.NewLine + Environment.NewLine + autoRoundingExplanationTooltip + "'Distance between arrows'."));
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("lineWidth_relToViewportHeight"), new GUIContent("Width", tooltip_explaining_relativeToViewPortHeight));

                if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.lineWithAlternatingColors)
                {
                    SerializedProperty sP_lengthOfStripes_relToViewportHeight = serializedObject.FindProperty("lengthOfStripes_relToViewportHeight");
                    EditorGUILayout.PropertyField(sP_lengthOfStripes_relToViewportHeight, new GUIContent("Length of Stripes", tooltip_explaining_relativeToViewPortHeight + Environment.NewLine + Environment.NewLine + "This may get rounded internally to prevent an explosive raise of drawn lines, see also 'DrawXXL.DrawBasics.MaxAllowedDrawnLinesPerFrame'." + Environment.NewLine + "It also may get raised automatically for larger line widths."));
                    sP_lengthOfStripes_relToViewportHeight.floatValue = Mathf.Max(sP_lengthOfStripes_relToViewportHeight.floatValue, UtilitiesDXXL_DrawBasics.min_lengthOfStripes_ofAlternatingColorLine);
                }
            }
        }

        void TryDrawExtendedLines_checkBoxForDistanceOutsideScreen(SerializedProperty sP_lineType)
        {
            if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.vectorWithExtention)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("displayDistanceOutsideScreenBorder"), new GUIContent("Display distance if outside screen"));
            }
        }

        void TryDrawLineStyleAndDependentParameters(SerializedProperty sP_lineType)
        {
            if ((sP_lineType.enumValueIndex == (int)LineDrawer.LineType.standardLine) || (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.blinkingLine))
            {
                SerializedProperty sP_lineStyle = serializedObject.FindProperty("lineStyle");
                EditorGUILayout.PropertyField(sP_lineStyle, new GUIContent("Style"));

                bool lineStyle_usesPatternScaling = UtilitiesDXXL_LineStyles.CheckIfLineStyleUsesPatternScaling((DrawBasics.LineStyle)sP_lineStyle.enumValueIndex);
                if (lineStyle_usesPatternScaling)
                {
                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("stylePatternScaleFactor"), new GUIContent("Style Pattern Scaling", "This may sometimes get automatically rounded up for line widths that are bigger than 0."));
                    GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);
                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                }
            }
        }

        void TryDrawAnimationSection(SerializedProperty sP_lineType)
        {
            bool lineIsAnimatable = lineDrawerScreenspaceMonoBehaviour_unserialized.DrawnLineUsesAnimation(false);
            if (lineIsAnimatable)
            {
                if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.movingArrowsLine)
                {
                    DrawAnimationSpeed(serializedObject.FindProperty("animationSpeed_ofMovingArrowsLine"), new GUIContent("Animation speed"));

                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("backwardAnimationFlipsArrowDirection"), new GUIContent("Flip arrows during backward animation", "This has only effect if 'Animation speed' is negative. If it is disabled then the arrows are always forced to point towards the line end and they appear to move backwards."));
                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                }

                if ((sP_lineType.enumValueIndex == (int)LineDrawer.LineType.standardLine) || (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.lineWithAlternatingColors))
                {
                    DrawAnimationSpeed(serializedObject.FindProperty("animationSpeed"), new GUIContent("Animation speed"));
                }

                if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.blinkingLine)
                {
                    SerializedProperty sP_blinkDurationInSec = serializedObject.FindProperty("blinkDurationInSec");
                    DrawAnimationSpeed(sP_blinkDurationInSec, new GUIContent("Blink duration", "In seconds"));
                    sP_blinkDurationInSec.floatValue = Mathf.Max(sP_blinkDurationInSec.floatValue, UtilitiesDXXL_DrawBasics.min_blinkDurationInSec);
                }
            }
        }

        void DrawAnimationSpeed(SerializedProperty sP_used_animationSpeedField, GUIContent displayName)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sP_used_animationSpeedField, displayName);
            if (Application.isPlaying == false)
            {
                SerializedProperty sP_animationDuringEditMode = serializedObject.FindProperty("animationDuringEditMode");
                sP_animationDuringEditMode.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Also in Edit Mode", "Animation ouside playmode is convenient, but forces the Editor Windows to continuously redraw, which may have a negative effect on the Editor performance." + Environment.NewLine + Environment.NewLine + "Note: The setting here may be without effect if a 'Draw XXL Gizmo Line Count Manager' component already redraws the Scene and Game view."), sP_animationDuringEditMode.boolValue);
            }
            GUILayout.EndHorizontal();
        }

        void TryDrawConeConfig(SerializedProperty sP_lineType)
        {
            if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.vector)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("conesConfig"), new GUIContent("Cones"));
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("coneLength_relToViewportHeight"), new GUIContent("Cone Length", tooltip_explaining_relativeToViewPortHeight));
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        void Draw_endPlatesConfig()
        {
            SerializedProperty sP_endPlatesConfig = serializedObject.FindProperty("endPlatesConfig");
            EditorGUILayout.PropertyField(sP_endPlatesConfig, new GUIContent("End Plates"));

            if (sP_endPlatesConfig.enumValueIndex != (int)LineDrawer.EndPlatesConfig.disabled)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("endPlatesSize_relToViewportHeight"), new GUIContent("End Plates Size", tooltip_explaining_relativeToViewPortHeight));
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        void TryDrawAlphaFadeOut(SerializedProperty sP_lineType)
        {
            if ((sP_lineType.enumValueIndex == (int)LineDrawer.LineType.standardLine) || (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.blinkingLine) || (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.lineWithAlternatingColors))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("alphaFadeOutLength_0to1"), new GUIContent("Fade Out Ends"));
            }
        }

        void DrawTextSpecs(SerializedProperty sP_lineType)
        {
            //-> this fakes the appearance of beeing inside the parents text specification foldout
            bool emptyLineAtEndIfOutfolded = false;
            bool displaySizeScalingStyleOption = false; //-> the text size markups don't work well for text on lines, since the text size anyway gets forced to a portion of the line length
            DrawTextInputInclMarkupHelper(true, false, null, emptyLineAtEndIfOutfolded, displaySizeScalingStyleOption);

            if (serializedObject.FindProperty("textSection_isOutfolded").boolValue == true)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                EditorGUILayout.PropertyField(serializedObject.FindProperty("relSizeOfTextOnLines"), new GUIContent("Text Area Size", "This is relative to the line length."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("shiftTextPosOnLines_toNonIntersecting"), new GUIContent("Shift text upward till non-intersecting", "This only has effect if the text contains line breaks. In such cases it will force all text lines to be on the same side of the drawn line."));

                if ((sP_lineType.enumValueIndex == (int)LineDrawer.LineType.standardLine) || (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.blinkingLine))
                {
                    GUILayout.BeginHorizontal();
                    SerializedProperty sP_enlargeSmallTextToThisMinTextSize = serializedObject.FindProperty("enlargeSmallTextToThisMinTextSize");
                    EditorGUILayout.PropertyField(sP_enlargeSmallTextToThisMinTextSize, new GUIContent("Minimum Text Size", "Width per letter in world units." + Environment.NewLine + Environment.NewLine + tooltip_explaining_relativeToViewPortHeight));
                    EditorGUI.BeginDisabledGroup(!sP_enlargeSmallTextToThisMinTextSize.boolValue);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("enlargeSmallTextToThisMinRelTextSize_value"), GUIContent.none);
                    EditorGUI.EndDisabledGroup();
                    GUILayout.EndHorizontal();
                }

                if (sP_lineType.enumValueIndex != (int)LineDrawer.LineType.vectorWithExtention)
                {
                    if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.vector)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("writeComponentValuesAsText"), new GUIContent("Add x/y length to text"));
                    }
                }

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }
        }

    }
#endif
}
