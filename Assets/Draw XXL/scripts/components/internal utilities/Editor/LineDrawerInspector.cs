namespace DrawXXL
{
    using System;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(LineDrawer))]
    [CanEditMultipleObjects]
    public class LineDrawerInspector : VisualizerParentInspector
    {
        LineDrawer lineDrawerMonoBehaviour_unserialized;

        void OnEnable()
        {
            OnEnable_base();
            OnEnable_base_atLineDrawer();
        }

        public void OnEnable_base_atLineDrawer()
        {
            lineDrawerMonoBehaviour_unserialized = (LineDrawer)target;
        }

        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("line");

            SerializedProperty sP_lineType = serializedObject.FindProperty("lineType");
            EditorGUILayout.PropertyField(sP_lineType, new GUIContent("Line type"));

            SerializedProperty sP_lineDefinitionMode = serializedObject.FindProperty("lineDefinitionMode");
            EditorGUILayout.PropertyField(sP_lineDefinitionMode, new GUIContent("Line definition mode"));

            GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

            Draw_startPos_endPos_lineDirection_definingSection(sP_lineDefinitionMode);
            TryDrawTensionSpecs(sP_lineType);
            DrawColors(sP_lineType);
            DrawWidthAndDependentParameters(sP_lineType);
            TryDrawExtentionLength(sP_lineType);
            TryDrawLineStyleAndDependentParameters(sP_lineType);
            TryDrawAnimationSection(sP_lineType);

            if (sP_lineType.enumValueIndex != (int)LineDrawer.LineType.vectorWithExtention)
            {
                TryDrawFlatLineSection(sP_lineType);
                TryDrawConeConfig(sP_lineType);
                Draw_endPlatesConfig();
                TryDrawAlphaFadeOut(sP_lineType);
            }

            TryDrawNormalizedMarkerCheckBox(sP_lineType);

            DrawTextSpecs(sP_lineType, false);
            DrawCheckboxFor_drawOnlyIfSelected("line");
            DrawCheckboxFor_hiddenByNearerObjects("line");

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }

        public void Draw_startPos_endPos_lineDirection_definingSection(SerializedProperty sP_lineDefinitionMode)
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

        public virtual void Draw_startPositionSection(SerializedProperty sP_lineDefinitionSection1_isOutfolded, GUIStyle style_ofFoldoutWithRichtext)
        {
            sP_lineDefinitionSection1_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_lineDefinitionSection1_isOutfolded.boolValue, "<b>Start Position</b>", true, style_ofFoldoutWithRichtext);
            if (sP_lineDefinitionSection1_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                SerializedProperty sP_positionDefinitionOption_ofStartPos = serializedObject.FindProperty("positionDefinitionOption_ofStartPos");
                EditorGUILayout.PropertyField(sP_positionDefinitionOption_ofStartPos, new GUIContent("Position Definition"));
                switch (sP_positionDefinitionOption_ofStartPos.enumValueIndex)
                {
                    case (int)LineDrawer.PositionDefinitionOption.positionOfThisGameobjectPlusOffset:
                        Draw_DrawPosition3DOffset(false, null, "Global Offset", "Local Offset", true);
                        break;
                    case (int)LineDrawer.PositionDefinitionOption.positionOfOtherGameobjectPlusOffset:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("partnerGameobject"), new GUIContent("Other Gameobject"));
                        Draw_DrawPosition3DOffset_ofPartnerGameobject(false, null, "Global Offset", "Local Offset", true);

                        EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("coordinateSpaceForLocalOffsetOnOtherGameobject_forStartPos"), new GUIContent("Local Offset Space"));
                        EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                        break;
                    case (int)LineDrawer.PositionDefinitionOption.chooseFree:
                        DrawSpecificationOf_customVector3_3(null, false, null, false, false, false, false, true, false);
                        break;
                    default:
                        break;
                }

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }
        }

        public virtual void Draw_endPositionSection(SerializedProperty sP_lineDefinitionSection2_isOutfolded, GUIStyle style_ofFoldoutWithRichtext)
        {
            sP_lineDefinitionSection2_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_lineDefinitionSection2_isOutfolded.boolValue, "<b>End Position</b>", true, style_ofFoldoutWithRichtext);
            if (sP_lineDefinitionSection2_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                SerializedProperty sP_positionDefinitionOption_ofEndPos = serializedObject.FindProperty("positionDefinitionOption_ofEndPos");
                EditorGUILayout.PropertyField(sP_positionDefinitionOption_ofEndPos, new GUIContent("Position Definition"));
                switch (sP_positionDefinitionOption_ofEndPos.enumValueIndex)
                {
                    case (int)LineDrawer.PositionDefinitionOption.positionOfThisGameobjectPlusOffset:
                        Draw_DrawPosition3DOffset_independentAlternativeValue(false, null, "Global Offset", "Local Offset", true);
                        break;
                    case (int)LineDrawer.PositionDefinitionOption.positionOfOtherGameobjectPlusOffset:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("partnerGameobject_independentAlternativeValue"), new GUIContent("Other Gameobject"));
                        Draw_DrawPosition3DOffset_ofPartnerGameobject_independentAlternativeValue(false, null, "Global Offset", "Local Offset", true);

                        EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("coordinateSpaceForLocalOffsetOnOtherGameobject_forEndPos"), new GUIContent("Local Offset Space"));
                        EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                        break;
                    case (int)LineDrawer.PositionDefinitionOption.chooseFree:
                        DrawSpecificationOf_customVector3_4(null, false, null, false, false, false, false, true, false);
                        break;
                    default:
                        break;
                }

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }
        }

        public virtual void Draw_lineVectorSection(SerializedProperty sP_lineDefinitionSection_isOutfolded, string headline, GUIStyle style_ofFoldoutWithRichtext)
        {
            sP_lineDefinitionSection_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_lineDefinitionSection_isOutfolded.boolValue, headline, true, style_ofFoldoutWithRichtext);
            if (sP_lineDefinitionSection_isOutfolded.boolValue)
            {
                DrawSpecificationOf_customVector3_1(null, false, null, false, false, false, false, true);
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }
        }

        public void TryDrawTensionSpecs(SerializedProperty sP_lineType)
        {
            if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.lineUnderTension)
            {
                SerializedProperty sP_relaxedLength = serializedObject.FindProperty("relaxedLength");
                float lineLength = lineDrawerMonoBehaviour_unserialized.GetLineLength();

                EditorGUILayout.PropertyField(sP_relaxedLength, new GUIContent("Relaxed Length", "This is the reference length for how the tension appears. If the line has a length of this value, then it appears with its relaxed color and the line style isn't stretched or squeezed. Other line lengths will stretch and squeeze the line style and change the color, as if the line is under tension like a spring."));
                sP_relaxedLength.floatValue = Mathf.Max(sP_relaxedLength.floatValue, 0.001f);

                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.FloatField(new GUIContent("Current Line Length"), lineLength);
                EditorGUILayout.FloatField(new GUIContent("Current Stretch Factor"), lineLength / sP_relaxedLength.floatValue);
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("alphaOfReferenceLengthDisplay"), new GUIContent("Alpha of reference length display"));

                GUILayout.Space(EditorGUIUtility.singleLineHeight);

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        public void DrawColors(SerializedProperty sP_lineType)
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

        public void DrawWidthAndDependentParameters(SerializedProperty sP_lineType)
        {
            if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.movingArrowsLine)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("lineWidth_ofMovingArrowsLine"), new GUIContent("Width"));

                string autoRoundingExplanationTooltip = "Very small (or very big) values may get rounded up (down) internally to prevent an explosive raise of drawn lines, see also 'DrawXXL.DrawBasics.MaxAllowedDrawnLinesPerFrame'." + Environment.NewLine + Environment.NewLine + "This automatic rounding has interdepency with 'Width' and ";

                SerializedProperty sP_distanceBetweenArrows = serializedObject.FindProperty("distanceBetweenArrows");
                SerializedProperty sP_lengthOfArrows = serializedObject.FindProperty("lengthOfArrows");

                EditorGUILayout.PropertyField(sP_distanceBetweenArrows, new GUIContent("Distance between arrows", autoRoundingExplanationTooltip + "'Length of Arrows'."));
                sP_distanceBetweenArrows.floatValue = Mathf.Max(sP_distanceBetweenArrows.floatValue, 0.002f);

                EditorGUILayout.PropertyField(sP_lengthOfArrows, new GUIContent("Length of Arrows", autoRoundingExplanationTooltip + "'Distance between arrows'."));
                sP_lengthOfArrows.floatValue = Mathf.Max(sP_lengthOfArrows.floatValue, 0.001f);
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("lineWidth"), new GUIContent("Width"));

                if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.lineWithAlternatingColors)
                {
                    SerializedProperty sP_lengthOfStripes = serializedObject.FindProperty("lengthOfStripes");
                    EditorGUILayout.PropertyField(sP_lengthOfStripes, new GUIContent("Length of Stripes", "This may get rounded internally to prevent an explosive raise of drawn lines, see also 'DrawXXL.DrawBasics.MaxAllowedDrawnLinesPerFrame'." + Environment.NewLine + "It also may get raised automatically for larger line widths."));
                    sP_lengthOfStripes.floatValue = Mathf.Max(sP_lengthOfStripes.floatValue, UtilitiesDXXL_DrawBasics.min_lengthOfStripes_ofAlternatingColorLine);
                }
            }
        }

        public void TryDrawExtentionLength(SerializedProperty sP_lineType)
        {
            if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.vectorWithExtention)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("extentionLength"), new GUIContent("Extention length"));

                GUILayout.BeginHorizontal();
                SerializedProperty sP_forceFixedConeLength = serializedObject.FindProperty("forceFixedConeLength");
                EditorGUILayout.PropertyField(sP_forceFixedConeLength, new GUIContent("Fixed cone length (in world units)", "Makes the cone length independent from the vector length."));
                EditorGUI.BeginDisabledGroup(!sP_forceFixedConeLength.boolValue);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("forceFixedConeLength_value"), GUIContent.none);
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();
            }
        }

        public void TryDrawLineStyleAndDependentParameters(SerializedProperty sP_lineType)
        {
            if ((sP_lineType.enumValueIndex == (int)LineDrawer.LineType.standardLine) || (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.blinkingLine) || (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.lineUnderTension))
            {
                SerializedProperty sP_lineStyle;
                if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.lineUnderTension)
                {
                    sP_lineStyle = serializedObject.FindProperty("lineStyle_underTension");
                }
                else
                {
                    sP_lineStyle = serializedObject.FindProperty("lineStyle");
                }

                EditorGUILayout.PropertyField(sP_lineStyle, new GUIContent("Style"));

                bool lineStyle_usesPatternScaling = UtilitiesDXXL_LineStyles.CheckIfLineStyleUsesPatternScaling((DrawBasics.LineStyle)sP_lineStyle.enumValueIndex);
                if (lineStyle_usesPatternScaling)
                {
                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                    SerializedProperty sP_stylePatternScaleFactor = serializedObject.FindProperty("stylePatternScaleFactor");
                    EditorGUILayout.PropertyField(sP_stylePatternScaleFactor, new GUIContent("Style Pattern Scaling", "This may sometimes get automatically rounded up for line widths that are bigger than 0. See also the 'Skip automatic pattern enlargement for * lines'-fields."));
                    sP_stylePatternScaleFactor.floatValue = Mathf.Max(sP_stylePatternScaleFactor.floatValue, UtilitiesDXXL_LineStyles.minStylePatternScaleFactor);

                    DrawSkipPatternEnlargementCheckBoxes();
                    GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);
                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                }
            }

            if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.lineWithAlternatingColors)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                DrawSkipPatternEnlargementCheckBoxes();
                GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        void DrawSkipPatternEnlargementCheckBoxes()
        {
            SerializedProperty sP_skipPatternEnlargementForLongLines = serializedObject.FindProperty("skipPatternEnlargementForLongLines");
            sP_skipPatternEnlargementForLongLines.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Skip automatic pattern enlargement for long lines", "For long lines the style pattern is automatically enlarged. Otherwise there would be a higher risk of Editor performance slowdown, because a single long styled line could accidentally use up many small straight lines, from which it is composed." + Environment.NewLine + Environment.NewLine + "You can skip this automatic enlargement at your own risk. 'DrawXXL.DrawBasics.MaxAllowedDrawnLinesPerFrame' will still protect you from Editor freeze."), sP_skipPatternEnlargementForLongLines.boolValue);

            SerializedProperty sP_skipPatternEnlargementForShortLines = serializedObject.FindProperty("skipPatternEnlargementForShortLines");
            sP_skipPatternEnlargementForShortLines.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Skip automatic pattern enlargement for short lines", "The style pattern of some lines gets automatically enlarged when the line becomes very short or for large line widths. This reduces the risk of Editor performance slowdown, because a single styled line could accidentally use up many small straight lines, from which it is composed, mainly when 'Style Pattern Scaling' has a very small value." + Environment.NewLine + "It also keeps the style patterns recognizable when a large line width would let 'dashes' appear as 'dots'." + Environment.NewLine + Environment.NewLine + "This enlargement only affects line styles with gaps, like 'dotted' or 'dashed'. It also only affects lines whose width is bigger than zero." + Environment.NewLine + Environment.NewLine + "You can skip this automatic enlargement at your own risk. 'DrawXXL.DrawBasics.MaxAllowedDrawnLinesPerFrame' will still protect you from Editor freeze."), sP_skipPatternEnlargementForShortLines.boolValue);
        }

        public void TryDrawAnimationSection(SerializedProperty sP_lineType)
        {
            bool lineIsAnimatable = lineDrawerMonoBehaviour_unserialized.DrawnLineUsesAnimation(false);
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

        void TryDrawFlatLineSection(SerializedProperty sP_lineType)
        {
            bool lineCanBeAffectedByFlattenBool = lineDrawerMonoBehaviour_unserialized.CheckIf_lineCanBeAffectedByFlattenBool();
            GUIContent guiContent_ofFlatToggle;
            if (lineCanBeAffectedByFlattenBool)
            {
                guiContent_ofFlatToggle = new GUIContent("Flat", "This makes the line flat instead of round. It only affects lines with a width that is bigger than zero, or if 'end plates' or 'end cones' are used.");
            }
            else
            {
                guiContent_ofFlatToggle = new GUIContent("Flat", "This only has effect if the line width is bigger than zero, or if 'end plates' or 'end cones' are used.");
            }

            bool lineIsFlat;
            EditorGUI.BeginDisabledGroup(!lineCanBeAffectedByFlattenBool);
            if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.movingArrowsLine)
            {
                SerializedProperty sP_flattenThickRoundLineIntoAmplitudePlane_ofMovingArrowsLine = serializedObject.FindProperty("flattenThickRoundLineIntoAmplitudePlane_ofMovingArrowsLine");
                EditorGUILayout.PropertyField(sP_flattenThickRoundLineIntoAmplitudePlane_ofMovingArrowsLine, guiContent_ofFlatToggle);
                lineIsFlat = sP_flattenThickRoundLineIntoAmplitudePlane_ofMovingArrowsLine.boolValue;
            }
            else
            {
                SerializedProperty sP_flattenThickRoundLineIntoAmplitudePlane = serializedObject.FindProperty("flattenThickRoundLineIntoAmplitudePlane");
                EditorGUILayout.PropertyField(sP_flattenThickRoundLineIntoAmplitudePlane, guiContent_ofFlatToggle);
                lineIsFlat = sP_flattenThickRoundLineIntoAmplitudePlane.boolValue;
            }
            EditorGUI.EndDisabledGroup();

            if (lineCanBeAffectedByFlattenBool)
            {
                if (lineIsFlat)
                {
                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                    DrawAmplitudeChooser("Alignment of flat line in 3D space", "Custom Flat Amplitude Direction");
                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                }
            }
        }

        public void TryDrawConeConfig(SerializedProperty sP_lineType)
        {
            if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.vector)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("conesConfig"), new GUIContent("Cones"));

                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                Draw_coneLengthInclSpaceInterpretation_forStraightVectors();
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);
            }
        }

        public void Draw_endPlatesConfig()
        {
            SerializedProperty sP_endPlatesConfig = serializedObject.FindProperty("endPlatesConfig");
            EditorGUILayout.PropertyField(sP_endPlatesConfig, new GUIContent("End Plates"));

            if (sP_endPlatesConfig.enumValueIndex != (int)LineDrawer.EndPlatesConfig.disabled)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                Draw_endPlatesSizeInclSpaceInterpretation();
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);
            }
        }

        public void TryDrawAlphaFadeOut(SerializedProperty sP_lineType)
        {
            if ((sP_lineType.enumValueIndex == (int)LineDrawer.LineType.standardLine) || (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.blinkingLine) || (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.lineWithAlternatingColors))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("alphaFadeOutLength_0to1"), new GUIContent("Fade Out Ends"));
            }
        }

        public void TryDrawNormalizedMarkerCheckBox(SerializedProperty sP_lineType)
        {
            if ((sP_lineType.enumValueIndex == (int)LineDrawer.LineType.vector) || (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.vectorWithExtention))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("addNormalizedMarkingText"), new GUIContent("Normalization Marker", "This adds the the text 'normalized' to the vector at the position where it is 1 unit long measured from the start position."));
            }
        }

        public void DrawTextSpecs(SerializedProperty sP_lineType, bool is2D)
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

                GUILayout.BeginHorizontal();
                SerializedProperty sP_enlargeSmallTextToThisMinTextSize = serializedObject.FindProperty("enlargeSmallTextToThisMinTextSize");
                EditorGUILayout.PropertyField(sP_enlargeSmallTextToThisMinTextSize, new GUIContent("Minimum Text Size", "Width per letter in world units." + Environment.NewLine + Environment.NewLine + "This is intended for cases where the line is so short that the text isn't well readable any more."));
                EditorGUI.BeginDisabledGroup(!sP_enlargeSmallTextToThisMinTextSize.boolValue);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("enlargeSmallTextToThisMinTextSize_value"), GUIContent.none);
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();

                if (sP_lineType.enumValueIndex != (int)LineDrawer.LineType.vectorWithExtention)
                {
                    if (sP_lineType.enumValueIndex == (int)LineDrawer.LineType.vector)
                    {
                        string displayName_of_writeComponentValuesAsText = is2D ? "Add x/y length to text" : "Add x/y/z length to text";
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("writeComponentValuesAsText"), new GUIContent(displayName_of_writeComponentValuesAsText));
                    }

                    if (is2D)
                    {
                        GUILayout.Space(EditorGUIUtility.singleLineHeight);
                    }
                    else
                    {
                        DrawAmplitudeChooser("Alignment of text in 3D space", "Custom Text Upward Direction");
                        if (serializedObject.FindProperty("customVector3_2_picker_isOutfolded").boolValue == false)
                        {
                            GUILayout.Space(EditorGUIUtility.singleLineHeight);
                        }
                    }
                }
                else
                {
                    GUILayout.Space(EditorGUIUtility.singleLineHeight);
                }

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        void DrawAmplitudeChooser(string alignmentSourceEnum_displayName, string vectorPicker_displayName)
        {
            SerializedProperty sP_amplitudeAndTextAlignment = serializedObject.FindProperty("amplitudeAndTextAlignment");
            EditorGUILayout.PropertyField(sP_amplitudeAndTextAlignment, new GUIContent(alignmentSourceEnum_displayName));

            if (sP_amplitudeAndTextAlignment.enumValueIndex == (int)LineDrawer.AmplitudeAndTextAlignment.customAmplitudeDirection)
            {
                DrawSpecificationOf_customVector3_2(vectorPicker_displayName, false, null, false, false, true, false);
            }
        }

    }
#endif
}
