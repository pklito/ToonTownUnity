namespace DrawXXL
{
    using System;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(VisualizerParent))]
    public class VisualizerParentInspector : Editor
    {
        public VisualizerParent visualizerParentMonoBehaviour_unserialized;
        public Transform transform_onVisualizerObject;

        void OnEnable()
        {
            OnEnable_base();
        }

        public void OnEnable_base()
        {
            visualizerParentMonoBehaviour_unserialized = (VisualizerParent)target;
            transform_onVisualizerObject = visualizerParentMonoBehaviour_unserialized.transform;
        }

        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            EditorGUILayout.HelpBox("This is the parent script that does nothing. Don't create it manually. You can delete this component.", MessageType.Info, true);

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }

        public float DrawConsumedLines(string nameOfVisualizedObject)
        {
            SerializedProperty sP_drawnLinesPerPass = serializedObject.FindProperty("drawnLinesPerPass");
            GUIStyle style_ofConsumedLinesLabel = new GUIStyle();
            style_ofConsumedLinesLabel.richText = true;
            float allowedConsumedLines_0to1 = (float)sP_drawnLinesPerPass.intValue / (float)DrawBasics.MaxAllowedDrawnLinesPerFrame;
            float hueValueOfColor = 0.33333f * (1.0f - allowedConsumedLines_0to1);
            Color color_visualizingWarningForTooManyLines = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(hueValueOfColor, 0.5f, 1.0f);
            string coloredLineNumber = "<color=#" + ColorUtility.ToHtmlStringRGBA(color_visualizingWarningForTooManyLines) + ">" + sP_drawnLinesPerPass.intValue + "</color>";
            string tooltipForBoth = "This " + nameOfVisualizedObject + " visualization is drawn with single straight lines (like from 'Debug.DrawLine()'). If too many of these straight lines are drawn it may hit the Editor execution performance." + Environment.NewLine + Environment.NewLine + "The color of this line count becomes more red as the number reaches a critical area." + Environment.NewLine + Environment.NewLine + "(see also 'DrawXXL.DrawBasics.MaxAllowedDrawnLinesPerFrame')";
            GUIContent drawnLinesWord = new GUIContent("Drawn lines:", tooltipForBoth);
            GUIContent drawnLinesNumber = new GUIContent(coloredLineNumber, tooltipForBoth);
            EditorGUILayout.LabelField(drawnLinesWord, drawnLinesNumber, style_ofConsumedLinesLabel);
            return allowedConsumedLines_0to1;
        }

        public void Draw_coneConfig_insideFoldout_forStraightVectors()
        {
            SerializedProperty sP_coneLength_forStraightVectors_isOutfolded = serializedObject.FindProperty("coneLength_forStraightVectors_isOutfolded");
            sP_coneLength_forStraightVectors_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_coneLength_forStraightVectors_isOutfolded.boolValue, "Cone length", true);
            if (sP_coneLength_forStraightVectors_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                Draw_coneLengthInclSpaceInterpretation_forStraightVectors();
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);
            }
        }

        public void Draw_coneLengthInclSpaceInterpretation_forStraightVectors()
        {
            SerializedProperty sP_coneLength_forStraightVectors = serializedObject.FindProperty("coneLength_forStraightVectors");
            SerializedProperty sP_coneLength_interpretation_forStraightVectors = serializedObject.FindProperty("coneLength_interpretation_forStraightVectors");
            EditorGUILayout.PropertyField(sP_coneLength_interpretation_forStraightVectors, new GUIContent("Length interpretation", "You can change the initial value of this via 'DrawXXL.DrawBasics.coneLength_interpretation_forStraightVectors'."));
            switch (sP_coneLength_interpretation_forStraightVectors.enumValueIndex)
            {
                case (int)DrawBasics.LengthInterpretation.relativeToLineLength:
                    sP_coneLength_forStraightVectors.floatValue = Mathf.Clamp(sP_coneLength_forStraightVectors.floatValue, UtilitiesDXXL_DrawBasics.min_relConeLengthForVectors, UtilitiesDXXL_DrawBasics.max_relConeLengthForVectors);
                    sP_coneLength_forStraightVectors.floatValue = EditorGUILayout.Slider("Length value", sP_coneLength_forStraightVectors.floatValue, UtilitiesDXXL_DrawBasics.min_relConeLengthForVectors, UtilitiesDXXL_DrawBasics.max_relConeLengthForVectors);
                    break;
                case (int)DrawBasics.LengthInterpretation.absoluteUnits:
                    EditorGUILayout.PropertyField(sP_coneLength_forStraightVectors, new GUIContent("Length value"));
                    sP_coneLength_forStraightVectors.floatValue = Mathf.Max(sP_coneLength_forStraightVectors.floatValue, 0.0f);
                    break;
                default:
                    break;
            }
        }

        public void Draw_coneLength_forCircledVectors()
        {
            SerializedProperty sP_coneLength_forCircledVectors_isOutfolded = serializedObject.FindProperty("coneLength_forCircledVectors_isOutfolded");
            sP_coneLength_forCircledVectors_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_coneLength_forCircledVectors_isOutfolded.boolValue, "Cone length", true);
            if (sP_coneLength_forCircledVectors_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                SerializedProperty sP_coneLength_forCircledVectors = serializedObject.FindProperty("coneLength_forCircledVectors");
                SerializedProperty sP_coneLength_interpretation_forCircledVectors = serializedObject.FindProperty("coneLength_interpretation_forCircledVectors");
                EditorGUILayout.PropertyField(sP_coneLength_interpretation_forCircledVectors, new GUIContent("Value interpretation", "You can change the initial value of this via 'DrawXXL.DrawBasics.coneLength_interpretation_forCircledVectors'."));
                switch (sP_coneLength_interpretation_forCircledVectors.enumValueIndex)
                {
                    case (int)DrawBasics.LengthInterpretation.relativeToLineLength:
                        sP_coneLength_forCircledVectors.floatValue = Mathf.Clamp01(sP_coneLength_forCircledVectors.floatValue);
                        sP_coneLength_forCircledVectors.floatValue = EditorGUILayout.Slider("Relative to radius", sP_coneLength_forCircledVectors.floatValue, 0.0f, 1.0f);
                        break;
                    case (int)DrawBasics.LengthInterpretation.absoluteUnits:
                        EditorGUILayout.PropertyField(sP_coneLength_forCircledVectors, new GUIContent("Absolute length"));
                        break;
                    default:
                        break;
                }

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);
            }
        }

        public void Draw_endPlatesConfig_insideFoldout()
        {
            SerializedProperty sP_endPlates_size_isOutfolded = serializedObject.FindProperty("endPlates_size_isOutfolded");
            sP_endPlates_size_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_endPlates_size_isOutfolded.boolValue, "End plates size", true);
            if (sP_endPlates_size_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                Draw_endPlatesSizeInclSpaceInterpretation();
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);
            }
        }

        public void Draw_endPlatesSizeInclSpaceInterpretation()
        {
            SerializedProperty sP_endPlates_sizeInterpretation = serializedObject.FindProperty("endPlates_sizeInterpretation");
            SerializedProperty sP_endPlates_size = serializedObject.FindProperty("endPlates_size");

            EditorGUILayout.PropertyField(sP_endPlates_sizeInterpretation, new GUIContent("Size interpretation", "You can change the initial value of this via 'DrawXXL.DrawBasics.endPlates_sizeInterpretation'."));
            switch (sP_endPlates_sizeInterpretation.enumValueIndex)
            {
                case (int)DrawBasics.LengthInterpretation.relativeToLineLength:
                    sP_endPlates_size.floatValue = Mathf.Clamp01(sP_endPlates_size.floatValue);
                    sP_endPlates_size.floatValue = EditorGUILayout.Slider("Size of End Plates", sP_endPlates_size.floatValue, 0.0f, 1.0f);
                    break;
                case (int)DrawBasics.LengthInterpretation.absoluteUnits:
                    EditorGUILayout.PropertyField(sP_endPlates_size, new GUIContent("Size of End Plates"));
                    sP_endPlates_size.floatValue = Mathf.Max(sP_endPlates_size.floatValue, 0.0f);
                    break;
                default:
                    break;
            }
        }

        static string firstLine_ofTooltip_forLocal_caseFromThisTransform = "This is the local space defined by this transform.";
        static string firstLine_ofTooltip_forLocal_caseFromOtherTransform = "This is the local space defined by the transform of the other object.";

        public void Draw_DrawPosition3DOffset(bool skipEmptyLineAtEndOfExpandedBlock = false, string overwrite_foldoutLabel = null, string displayNameOf_globalOffset = null, string displayNameOf_localOffset = null, bool withoutHeadline_norIntend_norSpaceAtEnd = false)
        {
            string firstLine_ofTooltip_forLocal = firstLine_ofTooltip_forLocal_caseFromThisTransform;
            SerializedProperty sP_drawPosOffset3DSection_isOutfolded = serializedObject.FindProperty("drawPosOffset3DSection_isOutfolded");
            SerializedProperty sP_drawPosOffset3D_global = serializedObject.FindProperty("drawPosOffset3D_global");
            SerializedProperty sP_drawPosOffset3D_local = serializedObject.FindProperty("drawPosOffset3D_local");
            Draw_DrawPosition3DOffset(skipEmptyLineAtEndOfExpandedBlock, overwrite_foldoutLabel, firstLine_ofTooltip_forLocal, sP_drawPosOffset3DSection_isOutfolded, sP_drawPosOffset3D_global, sP_drawPosOffset3D_local, displayNameOf_globalOffset, displayNameOf_localOffset, withoutHeadline_norIntend_norSpaceAtEnd);
        }

        public void Draw_DrawPosition3DOffset_independentAlternativeValue(bool skipEmptyLineAtEndOfExpandedBlock = false, string overwrite_foldoutLabel = null, string displayNameOf_globalOffset = null, string displayNameOf_localOffset = null, bool withoutHeadline_norIntend_norSpaceAtEnd = false)
        {
            string firstLine_ofTooltip_forLocal = firstLine_ofTooltip_forLocal_caseFromThisTransform;
            SerializedProperty sP_drawPosOffset3DSection_isOutfolded_independentAlternativeValue = serializedObject.FindProperty("drawPosOffset3DSection_isOutfolded_independentAlternativeValue");
            SerializedProperty sP_drawPosOffset3D_global_independentAlternativeValue = serializedObject.FindProperty("drawPosOffset3D_global_independentAlternativeValue");
            SerializedProperty sP_drawPosOffset3D_local_independentAlternativeValue = serializedObject.FindProperty("drawPosOffset3D_local_independentAlternativeValue");
            Draw_DrawPosition3DOffset(skipEmptyLineAtEndOfExpandedBlock, overwrite_foldoutLabel, firstLine_ofTooltip_forLocal, sP_drawPosOffset3DSection_isOutfolded_independentAlternativeValue, sP_drawPosOffset3D_global_independentAlternativeValue, sP_drawPosOffset3D_local_independentAlternativeValue, displayNameOf_globalOffset, displayNameOf_localOffset, withoutHeadline_norIntend_norSpaceAtEnd);
        }

        public void Draw_DrawPosition3DOffset_ofPartnerGameobject(bool skipEmptyLineAtEndOfExpandedBlock = false, string overwrite_foldoutLabel = null, string displayNameOf_globalOffset = null, string displayNameOf_localOffset = null, bool withoutHeadline_norIntend_norSpaceAtEnd = false)
        {
            string firstLine_ofTooltip_forLocal = firstLine_ofTooltip_forLocal_caseFromOtherTransform;
            SerializedProperty sP_drawPosOffset3DSection_isOutfolded = serializedObject.FindProperty("drawPosOffset3DSection_ofPartnerGameobject_isOutfolded");
            SerializedProperty sP_drawPosOffset3D_global = serializedObject.FindProperty("drawPosOffset3D_ofPartnerGameobject_global");
            SerializedProperty sP_drawPosOffset3D_local = serializedObject.FindProperty("drawPosOffset3D_ofPartnerGameobject_local");
            Draw_DrawPosition3DOffset(skipEmptyLineAtEndOfExpandedBlock, overwrite_foldoutLabel, firstLine_ofTooltip_forLocal, sP_drawPosOffset3DSection_isOutfolded, sP_drawPosOffset3D_global, sP_drawPosOffset3D_local, displayNameOf_globalOffset, displayNameOf_localOffset, withoutHeadline_norIntend_norSpaceAtEnd);
        }

        public void Draw_DrawPosition3DOffset_ofPartnerGameobject_independentAlternativeValue(bool skipEmptyLineAtEndOfExpandedBlock = false, string overwrite_foldoutLabel = null, string displayNameOf_globalOffset = null, string displayNameOf_localOffset = null, bool withoutHeadline_norIntend_norSpaceAtEnd = false)
        {
            string firstLine_ofTooltip_forLocal = firstLine_ofTooltip_forLocal_caseFromOtherTransform;
            SerializedProperty sP_drawPosOffset3DSection_isOutfolded_independentAlternativeValue = serializedObject.FindProperty("drawPosOffset3DSection_ofPartnerGameobject_isOutfolded_independentAlternativeValue");
            SerializedProperty sP_drawPosOffset3D_global_independentAlternativeValue = serializedObject.FindProperty("drawPosOffset3D_ofPartnerGameobject_global_independentAlternativeValue");
            SerializedProperty sP_drawPosOffset3D_local_independentAlternativeValue = serializedObject.FindProperty("drawPosOffset3D_ofPartnerGameobject_local_independentAlternativeValue");
            Draw_DrawPosition3DOffset(skipEmptyLineAtEndOfExpandedBlock, overwrite_foldoutLabel, firstLine_ofTooltip_forLocal, sP_drawPosOffset3DSection_isOutfolded_independentAlternativeValue, sP_drawPosOffset3D_global_independentAlternativeValue, sP_drawPosOffset3D_local_independentAlternativeValue, displayNameOf_globalOffset, displayNameOf_localOffset, withoutHeadline_norIntend_norSpaceAtEnd);
        }

        void Draw_DrawPosition3DOffset(bool skipEmptyLineAtEndOfExpandedBlock, string overwrite_foldoutLabel, string firstLine_ofTooltip_forLocal, SerializedProperty sP_drawPosOffset3DSection_isOutfolded, SerializedProperty sP_drawPosOffset3D_global, SerializedProperty sP_drawPosOffset3D_local, string displayNameOf_globalOffset = null, string displayNameOf_localOffset = null, bool withoutHeadline_norIntend_norSpaceAtEnd = false)
        {
            if (withoutHeadline_norIntend_norSpaceAtEnd)
            {
                Draw_DrawPosition3DOffset_insideIndentedFoldout(firstLine_ofTooltip_forLocal, sP_drawPosOffset3D_global, sP_drawPosOffset3D_local, displayNameOf_globalOffset, displayNameOf_localOffset);
            }
            else
            {
                string used_foldoutLabel = (overwrite_foldoutLabel == null) ? "Draw Position Offset" : overwrite_foldoutLabel;
                sP_drawPosOffset3DSection_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_drawPosOffset3DSection_isOutfolded.boolValue, used_foldoutLabel, true);
                if (sP_drawPosOffset3DSection_isOutfolded.boolValue)
                {
                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                    Draw_DrawPosition3DOffset_insideIndentedFoldout(firstLine_ofTooltip_forLocal, sP_drawPosOffset3D_global, sP_drawPosOffset3D_local, displayNameOf_globalOffset, displayNameOf_localOffset);
                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                    if (skipEmptyLineAtEndOfExpandedBlock == false) { GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight); }
                }
            }
        }

        void Draw_DrawPosition3DOffset_insideIndentedFoldout(string firstLine_ofTooltip_forLocal, SerializedProperty sP_drawPosOffset3D_global, SerializedProperty sP_drawPosOffset3D_local, string displayNameOf_globalOffset, string displayNameOf_localOffset)
        {
            string used_displayNameOf_globalOffset = (displayNameOf_globalOffset == null) ? "global" : displayNameOf_globalOffset;
            string used_displayNameOf_localOffset = (displayNameOf_localOffset == null) ? "local" : displayNameOf_localOffset;

            EditorGUILayout.PropertyField(sP_drawPosOffset3D_global, new GUIContent(used_displayNameOf_globalOffset));
            EditorGUILayout.PropertyField(sP_drawPosOffset3D_local, new GUIContent(used_displayNameOf_localOffset, firstLine_ofTooltip_forLocal + Environment.NewLine + Environment.NewLine + "It may lead to wrong results if a parent has a non-uniform scale."));
        }

        public void Draw_DrawPosition2DOffset(bool skipEmptyLineAtEndOfExpandedBlock = false, string overwrite_foldoutLabel = null, string displayNameOf_globalOffset = null, string displayNameOf_localOffset = null, bool withoutHeadline_norIntend_norSpaceAtEnd = false)
        {
            string firstLine_ofTooltip_forLocal = firstLine_ofTooltip_forLocal_caseFromThisTransform;
            SerializedProperty sP_drawPosOffset2DSection_isOutfolded = serializedObject.FindProperty("drawPosOffset2DSection_isOutfolded");
            SerializedProperty sP_drawPosOffset2D_global = serializedObject.FindProperty("drawPosOffset2D_global");
            SerializedProperty sP_drawPosOffset2D_local = serializedObject.FindProperty("drawPosOffset2D_local");
            Transform transformThatDefinesTheLocalSpace = transform_onVisualizerObject;
            Draw_DrawPosition2DOffset(skipEmptyLineAtEndOfExpandedBlock, overwrite_foldoutLabel, firstLine_ofTooltip_forLocal, sP_drawPosOffset2DSection_isOutfolded, sP_drawPosOffset2D_global, sP_drawPosOffset2D_local, transformThatDefinesTheLocalSpace, displayNameOf_globalOffset, displayNameOf_localOffset, withoutHeadline_norIntend_norSpaceAtEnd);
        }

        public void Draw_DrawPosition2DOffset_independentAlternativeValue(bool skipEmptyLineAtEndOfExpandedBlock = false, string overwrite_foldoutLabel = null, string displayNameOf_globalOffset = null, string displayNameOf_localOffset = null, bool withoutHeadline_norIntend_norSpaceAtEnd = false)
        {
            string firstLine_ofTooltip_forLocal = firstLine_ofTooltip_forLocal_caseFromThisTransform;
            SerializedProperty sP_drawPosOffset2DSection_isOutfolded_independentAlternativeValue = serializedObject.FindProperty("drawPosOffset2DSection_isOutfolded_independentAlternativeValue");
            SerializedProperty sP_drawPosOffset2D_global_independentAlternativeValue = serializedObject.FindProperty("drawPosOffset2D_global_independentAlternativeValue");
            SerializedProperty sP_drawPosOffset2D_local_independentAlternativeValue = serializedObject.FindProperty("drawPosOffset2D_local_independentAlternativeValue");
            Transform transformThatDefinesTheLocalSpace = transform_onVisualizerObject;
            Draw_DrawPosition2DOffset(skipEmptyLineAtEndOfExpandedBlock, overwrite_foldoutLabel, firstLine_ofTooltip_forLocal, sP_drawPosOffset2DSection_isOutfolded_independentAlternativeValue, sP_drawPosOffset2D_global_independentAlternativeValue, sP_drawPosOffset2D_local_independentAlternativeValue, transformThatDefinesTheLocalSpace, displayNameOf_globalOffset, displayNameOf_localOffset, withoutHeadline_norIntend_norSpaceAtEnd);
        }

        public void Draw_DrawPosition2DOffset_ofPartnerGameobject(bool skipEmptyLineAtEndOfExpandedBlock = false, string overwrite_foldoutLabel = null, string displayNameOf_globalOffset = null, string displayNameOf_localOffset = null, bool withoutHeadline_norIntend_norSpaceAtEnd = false)
        {
            if (visualizerParentMonoBehaviour_unserialized.partnerGameobject != null)
            {
                string firstLine_ofTooltip_forLocal = firstLine_ofTooltip_forLocal_caseFromOtherTransform;
                SerializedProperty sP_drawPosOffset2DSection_isOutfolded = serializedObject.FindProperty("drawPosOffset2DSection_ofPartnerGameobject_isOutfolded");
                SerializedProperty sP_drawPosOffset2D_global = serializedObject.FindProperty("drawPosOffset2D_ofPartnerGameobject_global");
                SerializedProperty sP_drawPosOffset2D_local = serializedObject.FindProperty("drawPosOffset2D_ofPartnerGameobject_local");
                Transform transformThatDefinesTheLocalSpace = visualizerParentMonoBehaviour_unserialized.partnerGameobject.transform;
                Draw_DrawPosition2DOffset(skipEmptyLineAtEndOfExpandedBlock, overwrite_foldoutLabel, firstLine_ofTooltip_forLocal, sP_drawPosOffset2DSection_isOutfolded, sP_drawPosOffset2D_global, sP_drawPosOffset2D_local, transformThatDefinesTheLocalSpace, displayNameOf_globalOffset, displayNameOf_localOffset, withoutHeadline_norIntend_norSpaceAtEnd);
            }
        }

        public void Draw_DrawPosition2DOffset_ofPartnerGameobject_independentAlternativeValue(bool skipEmptyLineAtEndOfExpandedBlock = false, string overwrite_foldoutLabel = null, string displayNameOf_globalOffset = null, string displayNameOf_localOffset = null, bool withoutHeadline_norIntend_norSpaceAtEnd = false)
        {
            if (visualizerParentMonoBehaviour_unserialized.partnerGameobject != null)
            {
                string firstLine_ofTooltip_forLocal = firstLine_ofTooltip_forLocal_caseFromOtherTransform;
                SerializedProperty sP_drawPosOffset2DSection_isOutfolded_independentAlternativeValue = serializedObject.FindProperty("drawPosOffset2DSection_ofPartnerGameobject_isOutfolded_independentAlternativeValue");
                SerializedProperty sP_drawPosOffset2D_global_independentAlternativeValue = serializedObject.FindProperty("drawPosOffset2D_ofPartnerGameobject_global_independentAlternativeValue");
                SerializedProperty sP_drawPosOffset2D_local_independentAlternativeValue = serializedObject.FindProperty("drawPosOffset2D_ofPartnerGameobject_local_independentAlternativeValue");
                Transform transformThatDefinesTheLocalSpace = visualizerParentMonoBehaviour_unserialized.partnerGameobject.transform;
                Draw_DrawPosition2DOffset(skipEmptyLineAtEndOfExpandedBlock, overwrite_foldoutLabel, firstLine_ofTooltip_forLocal, sP_drawPosOffset2DSection_isOutfolded_independentAlternativeValue, sP_drawPosOffset2D_global_independentAlternativeValue, sP_drawPosOffset2D_local_independentAlternativeValue, transformThatDefinesTheLocalSpace, displayNameOf_globalOffset, displayNameOf_localOffset, withoutHeadline_norIntend_norSpaceAtEnd);
            }
        }

        void Draw_DrawPosition2DOffset(bool skipEmptyLineAtEndOfExpandedBlock, string overwrite_foldoutLabel, string firstLine_ofTooltip_forLocal, SerializedProperty sP_drawPosOffset2DSection_isOutfolded, SerializedProperty sP_drawPosOffset2D_global, SerializedProperty sP_drawPosOffset2D_local, Transform transformThatDefinesTheLocalSpace, string displayNameOf_globalOffset = null, string displayNameOf_localOffset = null, bool withoutHeadline_norIntend_norSpaceAtEnd = false)
        {
            if (withoutHeadline_norIntend_norSpaceAtEnd)
            {
                Draw_DrawPosition2DOffset_insideIndentedFoldout(firstLine_ofTooltip_forLocal, sP_drawPosOffset2D_global, sP_drawPosOffset2D_local, transformThatDefinesTheLocalSpace, displayNameOf_globalOffset, displayNameOf_localOffset);
            }
            else
            {
                string used_foldoutLabel = (overwrite_foldoutLabel == null) ? "Draw Position Offset" : overwrite_foldoutLabel;
                sP_drawPosOffset2DSection_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_drawPosOffset2DSection_isOutfolded.boolValue, used_foldoutLabel, true);
                if (sP_drawPosOffset2DSection_isOutfolded.boolValue)
                {
                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                    Draw_DrawPosition2DOffset_insideIndentedFoldout(firstLine_ofTooltip_forLocal, sP_drawPosOffset2D_global, sP_drawPosOffset2D_local, transformThatDefinesTheLocalSpace, displayNameOf_globalOffset, displayNameOf_localOffset);
                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                    if (skipEmptyLineAtEndOfExpandedBlock == false) { GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight); }
                }
            }
        }

        void Draw_DrawPosition2DOffset_insideIndentedFoldout(string firstLine_ofTooltip_forLocal, SerializedProperty sP_drawPosOffset2D_global, SerializedProperty sP_drawPosOffset2D_local, Transform transformThatDefinesTheLocalSpace, string displayNameOf_globalOffset, string displayNameOf_localOffset)
        {
            string used_displayNameOf_globalOffset = (displayNameOf_globalOffset == null) ? "global" : displayNameOf_globalOffset;
            string used_displayNameOf_localOffset = (displayNameOf_localOffset == null) ? "local" : displayNameOf_localOffset;

            EditorGUILayout.PropertyField(sP_drawPosOffset2D_global, new GUIContent(used_displayNameOf_globalOffset));
            EditorGUILayout.PropertyField(sP_drawPosOffset2D_local, new GUIContent(used_displayNameOf_localOffset, firstLine_ofTooltip_forLocal + Environment.NewLine + Environment.NewLine + "This may lead to wrong results if a parent has a non-uniform scale."));

            if (UtilitiesDXXL_EngineBasics.CheckIf_transformOrAParentHasNonUniformScale_2D(transformThatDefinesTheLocalSpace.parent))
            {
                EditorGUILayout.HelpBox("A parent transform has a non-uniform scale. This may lead to wrong or weird results.", MessageType.Warning, true);
            }

            if (UtilitiesDXXL_EngineBasics.CheckIfThisOrAParentHasANonZRotation_2D(transformThatDefinesTheLocalSpace))
            {
                EditorGUILayout.HelpBox("The transform or a parent has a non-z rotation. This may lead to wrong or weird results in 2D mode.", MessageType.Warning, true);
            }
        }

        public void DrawZPosChooserFor2D(bool skipEmptyLineAtEndOfExpandedBlock = false)
        {
            SerializedProperty sP_customZPos_for2D_isOutfolded = serializedObject.FindProperty("customZPos_for2D_isOutfolded");
            sP_customZPos_for2D_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_customZPos_for2D_isOutfolded.boolValue, "Z position", true);
            if (sP_customZPos_for2D_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                SerializedProperty sP_zPosSource = serializedObject.FindProperty("zPosSource");
                EditorGUILayout.PropertyField(sP_zPosSource, new GUIContent("Source of Z Position", "'Default' refers to the setting of 'DrawXXL.DrawBasics2D.Default_zPos_forDrawing'"));
                switch (sP_zPosSource.enumValueIndex)
                {
                    case (int)VisualizerParent.ZPosSource.transformPositionPlusOffset:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("customZPos_offsetValue"), new GUIContent("Offset from this transform", "In global units."));
                        break;
                    case (int)VisualizerParent.ZPosSource.setAbsolute:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("customZPos_absValue"), new GUIContent("Absolute z position", "In global units."));
                        break;
                    case (int)VisualizerParent.ZPosSource.defaultDrawZPosFromGlobalDrawXxlSettings:
                        EditorGUI.BeginDisabledGroup(true);
                        EditorGUILayout.FloatField(new GUIContent("Global default z position", "According to 'DrawBasics2D.Default_zPos_forDrawing'."), DrawBasics2D.Default_zPos_forDrawing);
                        EditorGUI.EndDisabledGroup();
                        break;
                    default:
                        break;
                }

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

                if (skipEmptyLineAtEndOfExpandedBlock == false) { GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight); }
            }
        }

        public void DrawCheckboxFor_drawOnlyIfSelected(string nameOfVisualizedObject)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("drawOnlyIfSelected"), new GUIContent("Draw only if selected", "When this is enabled then the " + nameOfVisualizedObject + " will only be drawn if this gameobject is selected in the editor." + Environment.NewLine + Environment.NewLine + "You can set the initial value of this for newly created components via 'DrawXXL.DrawBasics.initial_drawOnlyIfSelected_forComponents'."));
        }

        public void DrawCheckboxFor_hiddenByNearerObjects(string nameOfVisualizedObject)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("hiddenByNearerObjects"), new GUIContent("Hidden by nearer objects", "This is the same as the 'depthTest' parameter from 'Debug.DrawLine()'. It defines how much the drawn " + nameOfVisualizedObject + " shines through other objects that hide it." + Environment.NewLine + Environment.NewLine + "It has only effect in playmode."));
        }

        public void DrawSpecificationOf_customVector3_1(string foldoutRespCheckboxName, bool asCheckbox_insteadOfAsFoldout, SerializedProperty sP_customVectorPicker_isChecked, bool hideEverythingThatConcernsLength, bool hideDisplayOfReadOnlyFinalVectorValues, bool emptyLineAtEnd_ifOutfolded, bool emptyLineAtEnd_ifCollapsed, bool skipHeadline = false, bool fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition = true)
        {
            SerializedProperty sP_source_ofCustomVector3_1 = serializedObject.FindProperty("source_ofCustomVector3_1");
            SerializedProperty sP_customVector3_1_clipboardForManualInput = serializedObject.FindProperty("customVector3_1_clipboardForManualInput");
            SerializedProperty sP_customVector3_1_targetGameObject = serializedObject.FindProperty("customVector3_1_targetGameObject");
            SerializedProperty sP_customVector3_1_hasForcedAbsLength = serializedObject.FindProperty("customVector3_1_hasForcedAbsLength");
            SerializedProperty sP_customVector3_1_picker_isOutfolded = serializedObject.FindProperty("customVector3_1_picker_isOutfolded");
            SerializedProperty sP_forcedAbsLength_ofCustomVector3_1 = serializedObject.FindProperty("forcedAbsLength_ofCustomVector3_1");
            SerializedProperty sP_lengthRelScaleFactor_ofCustomVector3_1 = serializedObject.FindProperty("lengthRelScaleFactor_ofCustomVector3_1");
            SerializedProperty sP_vectorInterpretation_ofCustomVector3_1 = serializedObject.FindProperty("vectorInterpretation_ofCustomVector3_1");
            SerializedProperty sP_observerCamera_ofCustomVector3_1 = serializedObject.FindProperty("observerCamera_ofCustomVector3_1");

            DrawSpecificationOf_aCustomVector3(foldoutRespCheckboxName, asCheckbox_insteadOfAsFoldout, sP_customVector3_1_picker_isOutfolded, sP_customVectorPicker_isChecked, sP_source_ofCustomVector3_1, sP_customVector3_1_clipboardForManualInput, sP_customVector3_1_targetGameObject, sP_customVector3_1_hasForcedAbsLength, sP_forcedAbsLength_ofCustomVector3_1, sP_lengthRelScaleFactor_ofCustomVector3_1, sP_vectorInterpretation_ofCustomVector3_1, sP_observerCamera_ofCustomVector3_1, visualizerParentMonoBehaviour_unserialized.Get_customVector3_1_inGlobalSpaceUnits, visualizerParentMonoBehaviour_unserialized.Get_customVector3_1_inLocalSpaceDefinedByParentUnits, hideEverythingThatConcernsLength, hideDisplayOfReadOnlyFinalVectorValues, emptyLineAtEnd_ifOutfolded, emptyLineAtEnd_ifCollapsed, skipHeadline, fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition);
        }

        public void DrawSpecificationOf_customVector3_2(string foldoutRespCheckboxName, bool asCheckbox_insteadOfAsFoldout, SerializedProperty sP_customVectorPicker_isChecked, bool hideEverythingThatConcernsLength, bool hideDisplayOfReadOnlyFinalVectorValues, bool emptyLineAtEnd_ifOutfolded, bool emptyLineAtEnd_ifCollapsed, bool skipHeadline = false, bool fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition = true)
        {
            SerializedProperty sP_source_ofCustomVector3_2 = serializedObject.FindProperty("source_ofCustomVector3_2");
            SerializedProperty sP_customVector3_2_clipboardForManualInput = serializedObject.FindProperty("customVector3_2_clipboardForManualInput");
            SerializedProperty sP_customVector3_2_targetGameObject = serializedObject.FindProperty("customVector3_2_targetGameObject");
            SerializedProperty sP_customVector3_2_hasForcedAbsLength = serializedObject.FindProperty("customVector3_2_hasForcedAbsLength");
            SerializedProperty sP_customVector3_2_picker_isOutfolded = serializedObject.FindProperty("customVector3_2_picker_isOutfolded");
            SerializedProperty sP_forcedAbsLength_ofCustomVector3_2 = serializedObject.FindProperty("forcedAbsLength_ofCustomVector3_2");
            SerializedProperty sP_lengthRelScaleFactor_ofCustomVector3_2 = serializedObject.FindProperty("lengthRelScaleFactor_ofCustomVector3_2");
            SerializedProperty sP_vectorInterpretation_ofCustomVector3_2 = serializedObject.FindProperty("vectorInterpretation_ofCustomVector3_2");
            SerializedProperty sP_observerCamera_ofCustomVector3_2 = serializedObject.FindProperty("observerCamera_ofCustomVector3_2");

            DrawSpecificationOf_aCustomVector3(foldoutRespCheckboxName, asCheckbox_insteadOfAsFoldout, sP_customVector3_2_picker_isOutfolded, sP_customVectorPicker_isChecked, sP_source_ofCustomVector3_2, sP_customVector3_2_clipboardForManualInput, sP_customVector3_2_targetGameObject, sP_customVector3_2_hasForcedAbsLength, sP_forcedAbsLength_ofCustomVector3_2, sP_lengthRelScaleFactor_ofCustomVector3_2, sP_vectorInterpretation_ofCustomVector3_2, sP_observerCamera_ofCustomVector3_2, visualizerParentMonoBehaviour_unserialized.Get_customVector3_2_inGlobalSpaceUnits, visualizerParentMonoBehaviour_unserialized.Get_customVector3_2_inLocalSpaceDefinedByParentUnits, hideEverythingThatConcernsLength, hideDisplayOfReadOnlyFinalVectorValues, emptyLineAtEnd_ifOutfolded, emptyLineAtEnd_ifCollapsed, skipHeadline, fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition);
        }

        public void DrawSpecificationOf_customVector3_3(string foldoutRespCheckboxName, bool asCheckbox_insteadOfAsFoldout, SerializedProperty sP_customVectorPicker_isChecked, bool hideEverythingThatConcernsLength, bool hideDisplayOfReadOnlyFinalVectorValues, bool emptyLineAtEnd_ifOutfolded, bool emptyLineAtEnd_ifCollapsed, bool skipHeadline = false, bool fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition = true)
        {
            SerializedProperty sP_source_ofCustomVector3_3 = serializedObject.FindProperty("source_ofCustomVector3_3");
            SerializedProperty sP_customVector3_3_clipboardForManualInput = serializedObject.FindProperty("customVector3_3_clipboardForManualInput");
            SerializedProperty sP_customVector3_3_targetGameObject = serializedObject.FindProperty("customVector3_3_targetGameObject");
            SerializedProperty sP_customVector3_3_hasForcedAbsLength = serializedObject.FindProperty("customVector3_3_hasForcedAbsLength");
            SerializedProperty sP_customVector3_3_picker_isOutfolded = serializedObject.FindProperty("customVector3_3_picker_isOutfolded");
            SerializedProperty sP_forcedAbsLength_ofCustomVector3_3 = serializedObject.FindProperty("forcedAbsLength_ofCustomVector3_3");
            SerializedProperty sP_lengthRelScaleFactor_ofCustomVector3_3 = serializedObject.FindProperty("lengthRelScaleFactor_ofCustomVector3_3");
            SerializedProperty sP_vectorInterpretation_ofCustomVector3_3 = serializedObject.FindProperty("vectorInterpretation_ofCustomVector3_3");
            SerializedProperty sP_observerCamera_ofCustomVector3_3 = serializedObject.FindProperty("observerCamera_ofCustomVector3_3");

            DrawSpecificationOf_aCustomVector3(foldoutRespCheckboxName, asCheckbox_insteadOfAsFoldout, sP_customVector3_3_picker_isOutfolded, sP_customVectorPicker_isChecked, sP_source_ofCustomVector3_3, sP_customVector3_3_clipboardForManualInput, sP_customVector3_3_targetGameObject, sP_customVector3_3_hasForcedAbsLength, sP_forcedAbsLength_ofCustomVector3_3, sP_lengthRelScaleFactor_ofCustomVector3_3, sP_vectorInterpretation_ofCustomVector3_3, sP_observerCamera_ofCustomVector3_3, visualizerParentMonoBehaviour_unserialized.Get_customVector3_3_inGlobalSpaceUnits, visualizerParentMonoBehaviour_unserialized.Get_customVector3_3_inLocalSpaceDefinedByParentUnits, hideEverythingThatConcernsLength, hideDisplayOfReadOnlyFinalVectorValues, emptyLineAtEnd_ifOutfolded, emptyLineAtEnd_ifCollapsed, skipHeadline, fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition);
        }

        public void DrawSpecificationOf_customVector3_4(string foldoutRespCheckboxName, bool asCheckbox_insteadOfAsFoldout, SerializedProperty sP_customVectorPicker_isChecked, bool hideEverythingThatConcernsLength, bool hideDisplayOfReadOnlyFinalVectorValues, bool emptyLineAtEnd_ifOutfolded, bool emptyLineAtEnd_ifCollapsed, bool skipHeadline = false, bool fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition = true)
        {
            SerializedProperty sP_source_ofCustomVector3_4 = serializedObject.FindProperty("source_ofCustomVector3_4");
            SerializedProperty sP_customVector3_4_clipboardForManualInput = serializedObject.FindProperty("customVector3_4_clipboardForManualInput");
            SerializedProperty sP_customVector3_4_targetGameObject = serializedObject.FindProperty("customVector3_4_targetGameObject");
            SerializedProperty sP_customVector3_4_hasForcedAbsLength = serializedObject.FindProperty("customVector3_4_hasForcedAbsLength");
            SerializedProperty sP_customVector3_4_picker_isOutfolded = serializedObject.FindProperty("customVector3_4_picker_isOutfolded");
            SerializedProperty sP_forcedAbsLength_ofCustomVector3_4 = serializedObject.FindProperty("forcedAbsLength_ofCustomVector3_4");
            SerializedProperty sP_lengthRelScaleFactor_ofCustomVector3_4 = serializedObject.FindProperty("lengthRelScaleFactor_ofCustomVector3_4");
            SerializedProperty sP_vectorInterpretation_ofCustomVector3_4 = serializedObject.FindProperty("vectorInterpretation_ofCustomVector3_4");
            SerializedProperty sP_observerCamera_ofCustomVector3_4 = serializedObject.FindProperty("observerCamera_ofCustomVector3_4");

            DrawSpecificationOf_aCustomVector3(foldoutRespCheckboxName, asCheckbox_insteadOfAsFoldout, sP_customVector3_4_picker_isOutfolded, sP_customVectorPicker_isChecked, sP_source_ofCustomVector3_4, sP_customVector3_4_clipboardForManualInput, sP_customVector3_4_targetGameObject, sP_customVector3_4_hasForcedAbsLength, sP_forcedAbsLength_ofCustomVector3_4, sP_lengthRelScaleFactor_ofCustomVector3_4, sP_vectorInterpretation_ofCustomVector3_4, sP_observerCamera_ofCustomVector3_4, visualizerParentMonoBehaviour_unserialized.Get_customVector3_4_inGlobalSpaceUnits, visualizerParentMonoBehaviour_unserialized.Get_customVector3_4_inLocalSpaceDefinedByParentUnits, hideEverythingThatConcernsLength, hideDisplayOfReadOnlyFinalVectorValues, emptyLineAtEnd_ifOutfolded, emptyLineAtEnd_ifCollapsed, skipHeadline, fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition);
        }

        public void DrawSpecificationOf_customVector3ofPartnerGameobject(string foldoutRespCheckboxName, bool asCheckbox_insteadOfAsFoldout, SerializedProperty sP_customVectorPicker_isChecked, bool hideEverythingThatConcernsLength, bool hideDisplayOfReadOnlyFinalVectorValues, bool emptyLineAtEnd_ifOutfolded, bool emptyLineAtEnd_ifCollapsed)
        {
            SerializedProperty sP_source_ofCustomVector3ofPartnerGameobject = serializedObject.FindProperty("source_ofCustomVector3ofPartnerGameobject");
            SerializedProperty sP_customVector3ofPartnerGameobject_clipboardForManualInput = serializedObject.FindProperty("customVector3ofPartnerGameobject_clipboardForManualInput");
            SerializedProperty sP_customVector3ofPartnerGameobject_targetGameObject = serializedObject.FindProperty("customVector3ofPartnerGameobject_targetGameObject");
            SerializedProperty sP_customVector3ofPartnerGameobject_hasForcedAbsLength = serializedObject.FindProperty("customVector3ofPartnerGameobject_hasForcedAbsLength");
            SerializedProperty sP_customVector3ofPartnerGameobject_picker_isOutfolded = serializedObject.FindProperty("customVector3ofPartnerGameobject_picker_isOutfolded");
            SerializedProperty sP_forcedAbsLength_ofCustomVector3ofPartnerGameobject = serializedObject.FindProperty("forcedAbsLength_ofCustomVector3ofPartnerGameobject");
            SerializedProperty sP_lengthRelScaleFactor_ofCustomVector3ofPartnerGameobject = serializedObject.FindProperty("lengthRelScaleFactor_ofCustomVector3ofPartnerGameobject");
            SerializedProperty sP_vectorInterpretation_ofCustomVector3ofPartnerGameobject = serializedObject.FindProperty("vectorInterpretation_ofCustomVector3ofPartnerGameobject");
            SerializedProperty sP_observerCamera_ofCustomVector3ofPartnerGameobject = serializedObject.FindProperty("observerCamera_ofCustomVector3ofPartnerGameobject");

            DrawSpecificationOf_aCustomVector3(foldoutRespCheckboxName, asCheckbox_insteadOfAsFoldout, sP_customVector3ofPartnerGameobject_picker_isOutfolded, sP_customVectorPicker_isChecked, sP_source_ofCustomVector3ofPartnerGameobject, sP_customVector3ofPartnerGameobject_clipboardForManualInput, sP_customVector3ofPartnerGameobject_targetGameObject, sP_customVector3ofPartnerGameobject_hasForcedAbsLength, sP_forcedAbsLength_ofCustomVector3ofPartnerGameobject, sP_lengthRelScaleFactor_ofCustomVector3ofPartnerGameobject, sP_vectorInterpretation_ofCustomVector3ofPartnerGameobject, sP_observerCamera_ofCustomVector3ofPartnerGameobject, visualizerParentMonoBehaviour_unserialized.Get_customVector3ofPartnerGameobject_inGlobalSpaceUnits, visualizerParentMonoBehaviour_unserialized.Get_customVector3ofPartnerGameobject_inLocalSpaceDefinedByParentUnits, hideEverythingThatConcernsLength, hideDisplayOfReadOnlyFinalVectorValues, emptyLineAtEnd_ifOutfolded, emptyLineAtEnd_ifCollapsed, false, true);
        }

        void DrawSpecificationOf_aCustomVector3(string foldoutRespCheckboxName, bool asCheckbox_insteadOfAsFoldout, SerializedProperty sP_customVectorPicker_isOutfolded, SerializedProperty sP_customVectorPicker_isChecked, SerializedProperty sP_source_ofCustomVector3, SerializedProperty sP_customVector3_clipboardForManualInput, SerializedProperty sP_customVector_targetGameObject, SerializedProperty sP_customVector_hasForcedAbsLength, SerializedProperty sP_forcedAbsLength_ofCustomVector, SerializedProperty sP_lengthRelScaleFactor_ofCustomVector, SerializedProperty sP_vectorInterpretation_ofCustomVector, SerializedProperty sP_observerCamera_ofCustomVector, VisualizerParent.FlexibleGetCustomVector3 Get_concernedCustomVector3_inGlobalSpaceUnits, VisualizerParent.FlexibleGetCustomVector3 Get_concernedCustomVector3_inLocalSpaceDefinedByParentUnits, bool hideEverythingThatConcernsLength, bool hideDisplayOfReadOnlyFinalVectorValues, bool emptyLineAtEnd_ifOutfolded, bool emptyLineAtEnd_ifCollapsed, bool skipHeadline, bool fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition)
        {
            if (asCheckbox_insteadOfAsFoldout)
            {
                GUIStyle style_ofPickerHeadline = new GUIStyle(EditorStyles.toggle);
                style_ofPickerHeadline.richText = true;
                sP_customVectorPicker_isChecked.boolValue = EditorGUILayout.Toggle(foldoutRespCheckboxName, sP_customVectorPicker_isChecked.boolValue, style_ofPickerHeadline);

                EditorGUI.BeginDisabledGroup(!sP_customVectorPicker_isChecked.boolValue);
                DrawContentOfACustomVector3Specification(sP_source_ofCustomVector3, sP_customVector3_clipboardForManualInput, sP_customVector_targetGameObject, sP_customVector_hasForcedAbsLength, sP_forcedAbsLength_ofCustomVector, sP_lengthRelScaleFactor_ofCustomVector, sP_vectorInterpretation_ofCustomVector, sP_observerCamera_ofCustomVector, sP_customVectorPicker_isChecked, Get_concernedCustomVector3_inGlobalSpaceUnits, Get_concernedCustomVector3_inLocalSpaceDefinedByParentUnits, hideEverythingThatConcernsLength, hideDisplayOfReadOnlyFinalVectorValues, emptyLineAtEnd_ifOutfolded, fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                GUIStyle style_ofPickerHeadline = new GUIStyle(EditorStyles.foldout);
                style_ofPickerHeadline.richText = true;

                bool used_isOutfolded = true;
                if (skipHeadline == false)
                {
                    sP_customVectorPicker_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_customVectorPicker_isOutfolded.boolValue, foldoutRespCheckboxName, true, style_ofPickerHeadline);
                    used_isOutfolded = sP_customVectorPicker_isOutfolded.boolValue;
                }

                if (used_isOutfolded)
                {
                    DrawContentOfACustomVector3Specification(sP_source_ofCustomVector3, sP_customVector3_clipboardForManualInput, sP_customVector_targetGameObject, sP_customVector_hasForcedAbsLength, sP_forcedAbsLength_ofCustomVector, sP_lengthRelScaleFactor_ofCustomVector, sP_vectorInterpretation_ofCustomVector, sP_observerCamera_ofCustomVector, sP_customVectorPicker_isChecked, Get_concernedCustomVector3_inGlobalSpaceUnits, Get_concernedCustomVector3_inLocalSpaceDefinedByParentUnits, hideEverythingThatConcernsLength, hideDisplayOfReadOnlyFinalVectorValues, emptyLineAtEnd_ifOutfolded, fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition);
                }
                else
                {
                    if (emptyLineAtEnd_ifCollapsed)
                    {
                        GUILayout.Space(EditorGUIUtility.singleLineHeight);
                    }
                }
            }
        }

        void DrawContentOfACustomVector3Specification(SerializedProperty sP_source_ofCustomVector3, SerializedProperty sP_customVector3_clipboardForManualInput, SerializedProperty sP_customVector_targetGameObject, SerializedProperty sP_customVector_hasForcedAbsLength, SerializedProperty sP_forcedAbsLength_ofCustomVector, SerializedProperty sP_lengthRelScaleFactor_ofCustomVector, SerializedProperty sP_vectorInterpretation_ofCustomVector, SerializedProperty sP_observerCamera_ofCustomVector, SerializedProperty sP_customVectorPicker_isChecked, VisualizerParent.FlexibleGetCustomVector3 Get_concernedCustomVector3_inGlobalSpaceUnits, VisualizerParent.FlexibleGetCustomVector3 Get_concernedCustomVector3_inLocalSpaceDefinedByParentUnits, bool hideEverythingThatConcernsLength, bool hideDisplayOfReadOnlyFinalVectorValues, bool emptyLineAtEnd_ifOutfolded, bool fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition)
        {
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            EditorGUILayout.PropertyField(sP_source_ofCustomVector3, new GUIContent("Vector source"));

            bool forceInterpretationToAlwaysGlobal;
            switch (sP_source_ofCustomVector3.enumValueIndex)
            {
                case (int)VisualizerParent.CustomVector3Source.manualInput:
                    EditorGUILayout.PropertyField(sP_customVector3_clipboardForManualInput, new GUIContent("Input"));
                    forceInterpretationToAlwaysGlobal = false;
                    break;
                case (int)VisualizerParent.CustomVector3Source.toOtherGameobject:
                    EditorGUILayout.PropertyField(sP_customVector_targetGameObject, new GUIContent("Other GameObject"));
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector3Source.fromOtherGameobject:
                    EditorGUILayout.PropertyField(sP_customVector_targetGameObject, new GUIContent("Other GameObject"));
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector3Source.transformsForward:
                    forceInterpretationToAlwaysGlobal = false;
                    break;
                case (int)VisualizerParent.CustomVector3Source.transformsUp:
                    forceInterpretationToAlwaysGlobal = false;
                    break;
                case (int)VisualizerParent.CustomVector3Source.transformsRight:
                    forceInterpretationToAlwaysGlobal = false;
                    break;
                case (int)VisualizerParent.CustomVector3Source.transformsBack:
                    forceInterpretationToAlwaysGlobal = false;
                    break;
                case (int)VisualizerParent.CustomVector3Source.transformsDown:
                    forceInterpretationToAlwaysGlobal = false;
                    break;
                case (int)VisualizerParent.CustomVector3Source.transformsLeft:
                    forceInterpretationToAlwaysGlobal = false;
                    break;
                case (int)VisualizerParent.CustomVector3Source.globalForward:
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector3Source.globalUp:
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector3Source.globalRight:
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector3Source.globalBack:
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector3Source.globalDown:
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector3Source.globalLeft:
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector3Source.observerCameraForward:
                    EditorGUILayout.PropertyField(sP_observerCamera_ofCustomVector, new GUIContent("Camera to use"));
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector3Source.observerCameraUp:
                    EditorGUILayout.PropertyField(sP_observerCamera_ofCustomVector, new GUIContent("Camera to use"));
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector3Source.observerCameraRight:
                    EditorGUILayout.PropertyField(sP_observerCamera_ofCustomVector, new GUIContent("Camera to use"));
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector3Source.observerCameraBack:
                    EditorGUILayout.PropertyField(sP_observerCamera_ofCustomVector, new GUIContent("Camera to use"));
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector3Source.observerCameraDown:
                    EditorGUILayout.PropertyField(sP_observerCamera_ofCustomVector, new GUIContent("Camera to use"));
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector3Source.observerCameraLeft:
                    EditorGUILayout.PropertyField(sP_observerCamera_ofCustomVector, new GUIContent("Camera to use"));
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector3Source.observerCameraToThisGameobject:
                    EditorGUILayout.PropertyField(sP_observerCamera_ofCustomVector, new GUIContent("Camera to use"));
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                default:
                    forceInterpretationToAlwaysGlobal = true;
                    break;
            }

            Draw_interpretationChooser_forGlobalOrLocal_V3(sP_vectorInterpretation_ofCustomVector, sP_customVectorPicker_isChecked, forceInterpretationToAlwaysGlobal, sP_source_ofCustomVector3, hideEverythingThatConcernsLength);
            if (hideEverythingThatConcernsLength == false)
            {
                DrawLengthAdjustmentForCustomVector(sP_customVector_hasForcedAbsLength, sP_forcedAbsLength_ofCustomVector, sP_lengthRelScaleFactor_ofCustomVector, fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition);
            }
            if (hideDisplayOfReadOnlyFinalVectorValues == false)
            {
                string displayName_ofFinalVectorGlobal = fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition ? "Final vector (global) (read only)" : "Final position (global) (read only)";
                EditorGUILayout.Vector3Field(new GUIContent(displayName_ofFinalVectorGlobal, "This is in units of the global space." + Environment.NewLine + "It is read only."), Get_concernedCustomVector3_inGlobalSpaceUnits());

                string displayName_ofFinalVectorLocal = fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition ? "Final vector (local) (read only)" : "Final position (local) (read only)";
                EditorGUILayout.Vector3Field(new GUIContent(displayName_ofFinalVectorLocal, "This is in units of the local space defined by the parent transform." + Environment.NewLine + Environment.NewLine + "It is read only."), Get_concernedCustomVector3_inLocalSpaceDefinedByParentUnits());
            }

            if (emptyLineAtEnd_ifOutfolded)
            {
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

        void Draw_interpretationChooser_forGlobalOrLocal_V3(SerializedProperty sP_vectorInterpretation_ofCustomVector, SerializedProperty sP_customVectorPicker_isChecked, bool alwaysGreyOutAndReturnFalse_whichMeansLeaveItInGlobalSpace, SerializedProperty sP_source_ofCustomVector3, bool hideEverythingThatConcernsLength)
        {
            if (hideEverythingThatConcernsLength)
            {
                if (sP_source_ofCustomVector3.enumValueIndex == (int)VisualizerParent.CustomVector3Source.manualInput)
                {
                    EditorGUILayout.PropertyField(sP_vectorInterpretation_ofCustomVector, new GUIContent("Vector source interpretation"));
                    TryDraw_warningHelpBox_forNonUniformParentScale_V3(sP_vectorInterpretation_ofCustomVector, sP_customVectorPicker_isChecked, alwaysGreyOutAndReturnFalse_whichMeansLeaveItInGlobalSpace);
                }
            }
            else
            {
                if (alwaysGreyOutAndReturnFalse_whichMeansLeaveItInGlobalSpace)
                {
                    EditorGUI.BeginDisabledGroup(alwaysGreyOutAndReturnFalse_whichMeansLeaveItInGlobalSpace);
                }

                if (alwaysGreyOutAndReturnFalse_whichMeansLeaveItInGlobalSpace)
                {
                    EditorGUILayout.EnumPopup(new GUIContent("Vector source interpretation"), VisualizerParent.VectorInterpretation.globalSpace);
                }
                else
                {
                    EditorGUILayout.PropertyField(sP_vectorInterpretation_ofCustomVector, new GUIContent("Vector source interpretation"));
                }

                if (alwaysGreyOutAndReturnFalse_whichMeansLeaveItInGlobalSpace)
                {
                    EditorGUI.EndDisabledGroup();
                }
                TryDraw_warningHelpBox_forNonUniformParentScale_V3(sP_vectorInterpretation_ofCustomVector, sP_customVectorPicker_isChecked, alwaysGreyOutAndReturnFalse_whichMeansLeaveItInGlobalSpace);
            }
        }

        void TryDraw_warningHelpBox_forNonUniformParentScale_V3(SerializedProperty sP_vectorInterpretation_ofCustomVector, SerializedProperty sP_customVectorPicker_isChecked, bool alwaysGreyOutAndReturnFalse_whichMeansLeaveItInGlobalSpace)
        {
            if (transform_onVisualizerObject.parent != null)
            {
                if ((sP_customVectorPicker_isChecked == null) || sP_customVectorPicker_isChecked.boolValue)
                {
                    if (alwaysGreyOutAndReturnFalse_whichMeansLeaveItInGlobalSpace == false)
                    {
                        if ((VisualizerParent.VectorInterpretation)sP_vectorInterpretation_ofCustomVector.enumValueIndex == VisualizerParent.VectorInterpretation.localSpaceDefinedByParent)
                        {
                            if (UtilitiesDXXL_EngineBasics.CheckIf_transformOrAParentHasNonUniformScale(transform_onVisualizerObject.parent))
                            {
                                EditorGUILayout.HelpBox("A parent transform has a non-uniform scale. This may lead to wrong or weird results.", MessageType.Warning, true);
                            }
                        }
                    }
                }
            }
        }

        public void DrawSpecificationOf_customVector2_1(string foldoutRespCheckboxName, bool asCheckbox_insteadOfAsFoldout, SerializedProperty sP_customVectorPicker_isChecked, bool hideEverythingThatConcernsLength, bool hideDisplayOfReadOnlyFinalVectorValues, bool emptyLineAtEnd_ifOutfolded, bool emptyLineAtEnd_ifCollapsed, bool skipHeadline = false, bool fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition = true, bool hideEverythingThatConcernsLocalSpace = false)
        {
            SerializedProperty sP_source_ofCustomVector2_1 = serializedObject.FindProperty("source_ofCustomVector2_1");
            SerializedProperty sP_customVector2_1_clipboardForManualInput = serializedObject.FindProperty("customVector2_1_clipboardForManualInput");
            SerializedProperty sP_customVector2_1_targetGameObject = serializedObject.FindProperty("customVector2_1_targetGameObject");
            SerializedProperty sP_customVector2_1_hasForcedAbsLength = serializedObject.FindProperty("customVector2_1_hasForcedAbsLength");
            SerializedProperty sP_customVector2_1_picker_isOutfolded = serializedObject.FindProperty("customVector2_1_picker_isOutfolded");
            SerializedProperty sP_rotationFromRight_ofCustomVector2_1 = serializedObject.FindProperty("rotationFromRight_ofCustomVector2_1");
            SerializedProperty sP_forcedAbsLength_ofCustomVector2_1 = serializedObject.FindProperty("forcedAbsLength_ofCustomVector2_1");
            SerializedProperty sP_lengthRelScaleFactor_ofCustomVector2_1 = serializedObject.FindProperty("lengthRelScaleFactor_ofCustomVector2_1");
            SerializedProperty sP_vectorInterpretation_ofCustomVector2_1 = serializedObject.FindProperty("vectorInterpretation_ofCustomVector2_1");

            DrawSpecificationOf_aCustomVector2(foldoutRespCheckboxName, asCheckbox_insteadOfAsFoldout, sP_customVector2_1_picker_isOutfolded, sP_customVectorPicker_isChecked, sP_rotationFromRight_ofCustomVector2_1, sP_source_ofCustomVector2_1, sP_customVector2_1_clipboardForManualInput, sP_customVector2_1_targetGameObject, sP_customVector2_1_hasForcedAbsLength, sP_forcedAbsLength_ofCustomVector2_1, sP_lengthRelScaleFactor_ofCustomVector2_1, sP_vectorInterpretation_ofCustomVector2_1, visualizerParentMonoBehaviour_unserialized.Get_customVector2_1_inGlobalSpaceUnits, visualizerParentMonoBehaviour_unserialized.Get_customVector2_1_inLocalSpaceDefinedByParentUnits, hideEverythingThatConcernsLength, hideDisplayOfReadOnlyFinalVectorValues, emptyLineAtEnd_ifOutfolded, emptyLineAtEnd_ifCollapsed, skipHeadline, fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition, hideEverythingThatConcernsLocalSpace);
        }

        public void DrawSpecificationOf_customVector2_2(string foldoutRespCheckboxName, bool asCheckbox_insteadOfAsFoldout, SerializedProperty sP_customVectorPicker_isChecked, bool hideEverythingThatConcernsLength, bool hideDisplayOfReadOnlyFinalVectorValues, bool emptyLineAtEnd_ifOutfolded, bool emptyLineAtEnd_ifCollapsed, bool skipHeadline = false, bool fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition = true, bool hideEverythingThatConcernsLocalSpace = false)
        {
            SerializedProperty sP_source_ofCustomVector2_2 = serializedObject.FindProperty("source_ofCustomVector2_2");
            SerializedProperty sP_customVector2_2_clipboardForManualInput = serializedObject.FindProperty("customVector2_2_clipboardForManualInput");
            SerializedProperty sP_customVector2_2_targetGameObject = serializedObject.FindProperty("customVector2_2_targetGameObject");
            SerializedProperty sP_customVector2_2_hasForcedAbsLength = serializedObject.FindProperty("customVector2_2_hasForcedAbsLength");
            SerializedProperty sP_customVector2_2_picker_isOutfolded = serializedObject.FindProperty("customVector2_2_picker_isOutfolded");
            SerializedProperty sP_rotationFromRight_ofCustomVector2_2 = serializedObject.FindProperty("rotationFromRight_ofCustomVector2_2");
            SerializedProperty sP_forcedAbsLength_ofCustomVector2_2 = serializedObject.FindProperty("forcedAbsLength_ofCustomVector2_2");
            SerializedProperty sP_lengthRelScaleFactor_ofCustomVector2_2 = serializedObject.FindProperty("lengthRelScaleFactor_ofCustomVector2_2");
            SerializedProperty sP_vectorInterpretation_ofCustomVector2_2 = serializedObject.FindProperty("vectorInterpretation_ofCustomVector2_2");

            DrawSpecificationOf_aCustomVector2(foldoutRespCheckboxName, asCheckbox_insteadOfAsFoldout, sP_customVector2_2_picker_isOutfolded, sP_customVectorPicker_isChecked, sP_rotationFromRight_ofCustomVector2_2, sP_source_ofCustomVector2_2, sP_customVector2_2_clipboardForManualInput, sP_customVector2_2_targetGameObject, sP_customVector2_2_hasForcedAbsLength, sP_forcedAbsLength_ofCustomVector2_2, sP_lengthRelScaleFactor_ofCustomVector2_2, sP_vectorInterpretation_ofCustomVector2_2, visualizerParentMonoBehaviour_unserialized.Get_customVector2_2_inGlobalSpaceUnits, visualizerParentMonoBehaviour_unserialized.Get_customVector2_2_inLocalSpaceDefinedByParentUnits, hideEverythingThatConcernsLength, hideDisplayOfReadOnlyFinalVectorValues, emptyLineAtEnd_ifOutfolded, emptyLineAtEnd_ifCollapsed, skipHeadline, fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition, hideEverythingThatConcernsLocalSpace);
        }

        public void DrawSpecificationOf_customVector2_3(string foldoutRespCheckboxName, bool asCheckbox_insteadOfAsFoldout, SerializedProperty sP_customVectorPicker_isChecked, bool hideEverythingThatConcernsLength, bool hideDisplayOfReadOnlyFinalVectorValues, bool emptyLineAtEnd_ifOutfolded, bool emptyLineAtEnd_ifCollapsed, bool skipHeadline = false, bool fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition = true, bool hideEverythingThatConcernsLocalSpace = false)
        {
            SerializedProperty sP_source_ofCustomVector2_3 = serializedObject.FindProperty("source_ofCustomVector2_3");
            SerializedProperty sP_customVector2_3_clipboardForManualInput = serializedObject.FindProperty("customVector2_3_clipboardForManualInput");
            SerializedProperty sP_customVector2_3_targetGameObject = serializedObject.FindProperty("customVector2_3_targetGameObject");
            SerializedProperty sP_customVector2_3_hasForcedAbsLength = serializedObject.FindProperty("customVector2_3_hasForcedAbsLength");
            SerializedProperty sP_customVector2_3_picker_isOutfolded = serializedObject.FindProperty("customVector2_3_picker_isOutfolded");
            SerializedProperty sP_rotationFromRight_ofCustomVector2_3 = serializedObject.FindProperty("rotationFromRight_ofCustomVector2_3");
            SerializedProperty sP_forcedAbsLength_ofCustomVector2_3 = serializedObject.FindProperty("forcedAbsLength_ofCustomVector2_3");
            SerializedProperty sP_lengthRelScaleFactor_ofCustomVector2_3 = serializedObject.FindProperty("lengthRelScaleFactor_ofCustomVector2_3");
            SerializedProperty sP_vectorInterpretation_ofCustomVector2_3 = serializedObject.FindProperty("vectorInterpretation_ofCustomVector2_3");

            DrawSpecificationOf_aCustomVector2(foldoutRespCheckboxName, asCheckbox_insteadOfAsFoldout, sP_customVector2_3_picker_isOutfolded, sP_customVectorPicker_isChecked, sP_rotationFromRight_ofCustomVector2_3, sP_source_ofCustomVector2_3, sP_customVector2_3_clipboardForManualInput, sP_customVector2_3_targetGameObject, sP_customVector2_3_hasForcedAbsLength, sP_forcedAbsLength_ofCustomVector2_3, sP_lengthRelScaleFactor_ofCustomVector2_3, sP_vectorInterpretation_ofCustomVector2_3, visualizerParentMonoBehaviour_unserialized.Get_customVector2_3_inGlobalSpaceUnits, visualizerParentMonoBehaviour_unserialized.Get_customVector2_3_inLocalSpaceDefinedByParentUnits, hideEverythingThatConcernsLength, hideDisplayOfReadOnlyFinalVectorValues, emptyLineAtEnd_ifOutfolded, emptyLineAtEnd_ifCollapsed, skipHeadline, fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition, hideEverythingThatConcernsLocalSpace);
        }

        public void DrawSpecificationOf_customVector2_4(string foldoutRespCheckboxName, bool asCheckbox_insteadOfAsFoldout, SerializedProperty sP_customVectorPicker_isChecked, bool hideEverythingThatConcernsLength, bool hideDisplayOfReadOnlyFinalVectorValues, bool emptyLineAtEnd_ifOutfolded, bool emptyLineAtEnd_ifCollapsed, bool skipHeadline = false, bool fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition = true, bool hideEverythingThatConcernsLocalSpace = false)
        {
            SerializedProperty sP_source_ofCustomVector2_4 = serializedObject.FindProperty("source_ofCustomVector2_4");
            SerializedProperty sP_customVector2_4_clipboardForManualInput = serializedObject.FindProperty("customVector2_4_clipboardForManualInput");
            SerializedProperty sP_customVector2_4_targetGameObject = serializedObject.FindProperty("customVector2_4_targetGameObject");
            SerializedProperty sP_customVector2_4_hasForcedAbsLength = serializedObject.FindProperty("customVector2_4_hasForcedAbsLength");
            SerializedProperty sP_customVector2_4_picker_isOutfolded = serializedObject.FindProperty("customVector2_4_picker_isOutfolded");
            SerializedProperty sP_rotationFromRight_ofCustomVector2_4 = serializedObject.FindProperty("rotationFromRight_ofCustomVector2_4");
            SerializedProperty sP_forcedAbsLength_ofCustomVector2_4 = serializedObject.FindProperty("forcedAbsLength_ofCustomVector2_4");
            SerializedProperty sP_lengthRelScaleFactor_ofCustomVector2_4 = serializedObject.FindProperty("lengthRelScaleFactor_ofCustomVector2_4");
            SerializedProperty sP_vectorInterpretation_ofCustomVector2_4 = serializedObject.FindProperty("vectorInterpretation_ofCustomVector2_4");

            DrawSpecificationOf_aCustomVector2(foldoutRespCheckboxName, asCheckbox_insteadOfAsFoldout, sP_customVector2_4_picker_isOutfolded, sP_customVectorPicker_isChecked, sP_rotationFromRight_ofCustomVector2_4, sP_source_ofCustomVector2_4, sP_customVector2_4_clipboardForManualInput, sP_customVector2_4_targetGameObject, sP_customVector2_4_hasForcedAbsLength, sP_forcedAbsLength_ofCustomVector2_4, sP_lengthRelScaleFactor_ofCustomVector2_4, sP_vectorInterpretation_ofCustomVector2_4, visualizerParentMonoBehaviour_unserialized.Get_customVector2_4_inGlobalSpaceUnits, visualizerParentMonoBehaviour_unserialized.Get_customVector2_4_inLocalSpaceDefinedByParentUnits, hideEverythingThatConcernsLength, hideDisplayOfReadOnlyFinalVectorValues, emptyLineAtEnd_ifOutfolded, emptyLineAtEnd_ifCollapsed, skipHeadline, fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition, hideEverythingThatConcernsLocalSpace);
        }

        public void DrawSpecificationOf_customVector2ofPartnerGameobject(string foldoutRespCheckboxName, bool asCheckbox_insteadOfAsFoldout, SerializedProperty sP_customVectorPicker_isChecked, bool hideEverythingThatConcernsLength, bool hideDisplayOfReadOnlyFinalVectorValues, bool emptyLineAtEnd_ifOutfolded, bool emptyLineAtEnd_ifCollapsed)
        {
            SerializedProperty sP_source_ofCustomVector2ofPartnerGameobject = serializedObject.FindProperty("source_ofCustomVector2ofPartnerGameobject");
            SerializedProperty sP_customVector2ofPartnerGameobject_clipboardForManualInput = serializedObject.FindProperty("customVector2ofPartnerGameobject_clipboardForManualInput");
            SerializedProperty sP_customVector2ofPartnerGameobject_targetGameObject = serializedObject.FindProperty("customVector2ofPartnerGameobject_targetGameObject");
            SerializedProperty sP_customVector2ofPartnerGameobject_hasForcedAbsLength = serializedObject.FindProperty("customVector2ofPartnerGameobject_hasForcedAbsLength");
            SerializedProperty sP_customVector2ofPartnerGameobject_picker_isOutfolded = serializedObject.FindProperty("customVector2ofPartnerGameobject_picker_isOutfolded");
            SerializedProperty sP_rotationFromRight_ofCustomVector2ofPartnerGameobject = serializedObject.FindProperty("rotationFromRight_ofCustomVector2ofPartnerGameobject");
            SerializedProperty sP_forcedAbsLength_ofCustomVector2ofPartnerGameobject = serializedObject.FindProperty("forcedAbsLength_ofCustomVector2ofPartnerGameobject");
            SerializedProperty sP_lengthRelScaleFactor_ofCustomVector2ofPartnerGameobject = serializedObject.FindProperty("lengthRelScaleFactor_ofCustomVector2ofPartnerGameobject");
            SerializedProperty sP_vectorInterpretation_ofCustomVector2ofPartnerGameobject = serializedObject.FindProperty("vectorInterpretation_ofCustomVector2ofPartnerGameobject");

            DrawSpecificationOf_aCustomVector2(foldoutRespCheckboxName, asCheckbox_insteadOfAsFoldout, sP_customVector2ofPartnerGameobject_picker_isOutfolded, sP_customVectorPicker_isChecked, sP_rotationFromRight_ofCustomVector2ofPartnerGameobject, sP_source_ofCustomVector2ofPartnerGameobject, sP_customVector2ofPartnerGameobject_clipboardForManualInput, sP_customVector2ofPartnerGameobject_targetGameObject, sP_customVector2ofPartnerGameobject_hasForcedAbsLength, sP_forcedAbsLength_ofCustomVector2ofPartnerGameobject, sP_lengthRelScaleFactor_ofCustomVector2ofPartnerGameobject, sP_vectorInterpretation_ofCustomVector2ofPartnerGameobject, visualizerParentMonoBehaviour_unserialized.Get_customVector2ofPartnerGameobject_inGlobalSpaceUnits, visualizerParentMonoBehaviour_unserialized.Get_customVector2ofPartnerGameobject_inLocalSpaceDefinedByParentUnits, hideEverythingThatConcernsLength, hideDisplayOfReadOnlyFinalVectorValues, emptyLineAtEnd_ifOutfolded, emptyLineAtEnd_ifCollapsed, false, true, false);
        }

        void DrawSpecificationOf_aCustomVector2(string foldoutRespCheckboxName, bool asCheckbox_insteadOfAsFoldout, SerializedProperty sP_customVectorPicker_isOutfolded, SerializedProperty sP_customVectorPicker_isChecked, SerializedProperty sP_rotationFromRight_ofCustomVector2, SerializedProperty sP_source_ofCustomVector2, SerializedProperty sP_customVector2_clipboardForManualInput, SerializedProperty sP_customVector_targetGameObject, SerializedProperty sP_customVector_hasForcedAbsLength, SerializedProperty sP_forcedAbsLength_ofCustomVector, SerializedProperty sP_lengthRelScaleFactor_ofCustomVector, SerializedProperty sP_vectorInterpretation_ofCustomVector, VisualizerParent.FlexibleGetCustomVector2 Get_concernedCustomVector2_inGlobalSpaceUnits, VisualizerParent.FlexibleGetCustomVector2 Get_concernedCustomVector2_inLocalSpaceDefinedByParentUnits, bool hideEverythingThatConcernsLength, bool hideDisplayOfReadOnlyFinalVectorValues, bool emptyLineAtEnd_ifOutfolded, bool emptyLineAtEnd_ifCollapsed, bool skipHeadline, bool fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition, bool hideEverythingThatConcernsLocalSpace)
        {
            if (asCheckbox_insteadOfAsFoldout)
            {
                GUIStyle style_ofPickerHeadline = new GUIStyle(EditorStyles.toggle);
                style_ofPickerHeadline.richText = true;
                sP_customVectorPicker_isChecked.boolValue = EditorGUILayout.Toggle(foldoutRespCheckboxName, sP_customVectorPicker_isChecked.boolValue, style_ofPickerHeadline);

                EditorGUI.BeginDisabledGroup(!sP_customVectorPicker_isChecked.boolValue);
                DrawContentOfACustomVector2Specification(sP_rotationFromRight_ofCustomVector2, sP_source_ofCustomVector2, sP_customVector2_clipboardForManualInput, sP_customVector_targetGameObject, sP_customVector_hasForcedAbsLength, sP_forcedAbsLength_ofCustomVector, sP_lengthRelScaleFactor_ofCustomVector, sP_vectorInterpretation_ofCustomVector, sP_customVectorPicker_isChecked, Get_concernedCustomVector2_inGlobalSpaceUnits, Get_concernedCustomVector2_inLocalSpaceDefinedByParentUnits, hideEverythingThatConcernsLength, hideDisplayOfReadOnlyFinalVectorValues, emptyLineAtEnd_ifOutfolded, fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition, hideEverythingThatConcernsLocalSpace);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                GUIStyle style_ofPickerHeadline = new GUIStyle(EditorStyles.foldout);
                style_ofPickerHeadline.richText = true;

                bool used_isOutfolded = true;
                if (skipHeadline == false)
                {
                    sP_customVectorPicker_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_customVectorPicker_isOutfolded.boolValue, foldoutRespCheckboxName, true, style_ofPickerHeadline);
                    used_isOutfolded = sP_customVectorPicker_isOutfolded.boolValue;
                }

                if (used_isOutfolded)
                {
                    DrawContentOfACustomVector2Specification(sP_rotationFromRight_ofCustomVector2, sP_source_ofCustomVector2, sP_customVector2_clipboardForManualInput, sP_customVector_targetGameObject, sP_customVector_hasForcedAbsLength, sP_forcedAbsLength_ofCustomVector, sP_lengthRelScaleFactor_ofCustomVector, sP_vectorInterpretation_ofCustomVector, sP_customVectorPicker_isChecked, Get_concernedCustomVector2_inGlobalSpaceUnits, Get_concernedCustomVector2_inLocalSpaceDefinedByParentUnits, hideEverythingThatConcernsLength, hideDisplayOfReadOnlyFinalVectorValues, emptyLineAtEnd_ifOutfolded, fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition, hideEverythingThatConcernsLocalSpace);
                }
                else
                {
                    if (emptyLineAtEnd_ifCollapsed)
                    {
                        GUILayout.Space(EditorGUIUtility.singleLineHeight);
                    }
                }
            }
        }

        void DrawContentOfACustomVector2Specification(SerializedProperty sP_rotationFromRight_ofCustomVector2, SerializedProperty sP_source_ofCustomVector2, SerializedProperty sP_customVector2_clipboardForManualInput, SerializedProperty sP_customVector_targetGameObject, SerializedProperty sP_customVector_hasForcedAbsLength, SerializedProperty sP_forcedAbsLength_ofCustomVector, SerializedProperty sP_lengthRelScaleFactor_ofCustomVector, SerializedProperty sP_vectorInterpretation_ofCustomVector, SerializedProperty sP_customVectorPicker_isChecked, VisualizerParent.FlexibleGetCustomVector2 Get_concernedCustomVector2_inGlobalSpaceUnits, VisualizerParent.FlexibleGetCustomVector2 Get_concernedCustomVector2_inLocalSpaceDefinedByParentUnits, bool hideEverythingThatConcernsLength, bool hideDisplayOfReadOnlyFinalVectorValues, bool emptyLineAtEnd_ifOutfolded, bool fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition, bool hideEverythingThatConcernsLocalSpace)
        {
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            EditorGUILayout.PropertyField(sP_source_ofCustomVector2, new GUIContent("Vector source"));

            bool forceInterpretationToAlwaysGlobal;
            switch (sP_source_ofCustomVector2.enumValueIndex)
            {
                case (int)VisualizerParent.CustomVector2Source.rotationAroundZStartingFromRight:
                    EditorGUILayout.PropertyField(sP_rotationFromRight_ofCustomVector2, new GUIContent("Z Angle"));
                    forceInterpretationToAlwaysGlobal = false;
                    break;
                case (int)VisualizerParent.CustomVector2Source.manualInput:
                    EditorGUILayout.PropertyField(sP_customVector2_clipboardForManualInput, new GUIContent("Input"));
                    forceInterpretationToAlwaysGlobal = false;
                    break;
                case (int)VisualizerParent.CustomVector2Source.toOtherGameobject:
                    EditorGUILayout.PropertyField(sP_customVector_targetGameObject, new GUIContent("Other GameObject"));
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector2Source.fromOtherGameobject:
                    EditorGUILayout.PropertyField(sP_customVector_targetGameObject, new GUIContent("Other GameObject"));
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector2Source.transformsUp:
                    forceInterpretationToAlwaysGlobal = false;
                    break;
                case (int)VisualizerParent.CustomVector2Source.transformsRight:
                    forceInterpretationToAlwaysGlobal = false;
                    break;
                case (int)VisualizerParent.CustomVector2Source.transformsDown:
                    forceInterpretationToAlwaysGlobal = false;
                    break;
                case (int)VisualizerParent.CustomVector2Source.transformsLeft:
                    forceInterpretationToAlwaysGlobal = false;
                    break;
                case (int)VisualizerParent.CustomVector2Source.globalUp:
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector2Source.globalRight:
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector2Source.globalDown:
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                case (int)VisualizerParent.CustomVector2Source.globalLeft:
                    forceInterpretationToAlwaysGlobal = true;
                    break;
                default:
                    forceInterpretationToAlwaysGlobal = true;
                    break;
            }

            if (hideEverythingThatConcernsLocalSpace == false)
            {
                Draw_interpretationChooser_forGlobalOrLocal_V2(sP_vectorInterpretation_ofCustomVector, sP_customVectorPicker_isChecked, forceInterpretationToAlwaysGlobal, sP_source_ofCustomVector2, hideEverythingThatConcernsLength);
            }

            if (hideEverythingThatConcernsLength == false)
            {
                DrawLengthAdjustmentForCustomVector(sP_customVector_hasForcedAbsLength, sP_forcedAbsLength_ofCustomVector, sP_lengthRelScaleFactor_ofCustomVector, fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition);
            }
            if (hideDisplayOfReadOnlyFinalVectorValues == false)
            {
                string displayName_ofFinalVectorGlobal = fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition ? "Final vector (global) (read only)" : "Final position (global) (read only)";
                EditorGUILayout.Vector2Field(new GUIContent(displayName_ofFinalVectorGlobal, "This is in units of the global space." + Environment.NewLine + "It is read only."), Get_concernedCustomVector2_inGlobalSpaceUnits());

                if (hideEverythingThatConcernsLocalSpace == false)
                {
                    string displayName_ofFinalVectorLocal = fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition ? "Final vector (local) (read only)" : "Final position (local) (read only)";
                    EditorGUILayout.Vector2Field(new GUIContent(displayName_ofFinalVectorLocal, "This is in units of the local space defined by the parent transform." + Environment.NewLine + Environment.NewLine + "It is read only."), Get_concernedCustomVector2_inLocalSpaceDefinedByParentUnits());
                }
            }
            if (hideEverythingThatConcernsLength == false)
            {
                TryDraw_warningHelpBoxes_forNonUniformParentScale_orNonZRotation_V2(sP_customVectorPicker_isChecked);
            }

            if (emptyLineAtEnd_ifOutfolded)
            {
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

        void Draw_interpretationChooser_forGlobalOrLocal_V2(SerializedProperty sP_vectorInterpretation_ofCustomVector, SerializedProperty sP_customVectorPicker_isChecked, bool alwaysGreyOutAndReturnFalse_whichMeansLeaveItInGlobalSpace, SerializedProperty sP_source_ofCustomVector2, bool hideEverythingThatConcernsLength)
        {
            if (hideEverythingThatConcernsLength)
            {
                if ((sP_source_ofCustomVector2.enumValueIndex == (int)VisualizerParent.CustomVector2Source.rotationAroundZStartingFromRight) || (sP_source_ofCustomVector2.enumValueIndex == (int)VisualizerParent.CustomVector2Source.manualInput))
                {
                    EditorGUILayout.PropertyField(sP_vectorInterpretation_ofCustomVector, new GUIContent("Vector source interpretation"));
                }
            }
            else
            {
                if (alwaysGreyOutAndReturnFalse_whichMeansLeaveItInGlobalSpace)
                {
                    EditorGUI.BeginDisabledGroup(alwaysGreyOutAndReturnFalse_whichMeansLeaveItInGlobalSpace);
                }

                if (alwaysGreyOutAndReturnFalse_whichMeansLeaveItInGlobalSpace)
                {
                    EditorGUILayout.EnumPopup(new GUIContent("Vector source interpretation"), VisualizerParent.VectorInterpretation.globalSpace);
                }
                else
                {
                    EditorGUILayout.PropertyField(sP_vectorInterpretation_ofCustomVector, new GUIContent("Vector source interpretation"));
                }

                if (alwaysGreyOutAndReturnFalse_whichMeansLeaveItInGlobalSpace)
                {
                    EditorGUI.EndDisabledGroup();
                }
            }
        }

        void TryDraw_warningHelpBoxes_forNonUniformParentScale_orNonZRotation_V2(SerializedProperty sP_customVectorPicker_isChecked)
        {
            if ((sP_customVectorPicker_isChecked == null) || sP_customVectorPicker_isChecked.boolValue)
            {
                if (UtilitiesDXXL_EngineBasics.CheckIf_transformOrAParentHasNonUniformScale_2D(transform_onVisualizerObject.parent))
                {
                    EditorGUILayout.HelpBox("A parent transform has a non-uniform scale. This may lead to wrong or weird results.", MessageType.Warning, true);
                }

                if (UtilitiesDXXL_EngineBasics.CheckIfThisOrAParentHasANonZRotation_2D(transform_onVisualizerObject))
                {
                    EditorGUILayout.HelpBox("This transform or a parent has a non-z rotation. This may lead to wrong or weird results in 2D mode.", MessageType.Warning, true);
                }
            }
        }

        void DrawLengthAdjustmentForCustomVector(SerializedProperty sP_customVector_hasForcedAbsLength, SerializedProperty sP_forcedAbsLength_ofCustomVector, SerializedProperty sP_lengthRelScaleFactor_ofCustomVector, bool fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition)
        {
            GUIStyle style_ofLengthHeadline = new GUIStyle();
            //style_ofLengthHeadline.fontStyle = FontStyle.Bold;

            string displayName_ofHeadline = fieldDisplayNames_fitVectorMeaningAsDirection_notAsPosition ? "Length" : "Distance from origin";
            EditorGUILayout.LabelField(displayName_ofHeadline, style_ofLengthHeadline);

            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sP_customVector_hasForcedAbsLength, new GUIContent("Force absolute", "This absolute length value is in units of the space as defined by 'Vector interpretation'."));
            EditorGUI.BeginDisabledGroup(!sP_customVector_hasForcedAbsLength.boolValue);
            EditorGUILayout.PropertyField(sP_forcedAbsLength_ofCustomVector, GUIContent.none);
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            EditorGUI.BeginDisabledGroup(sP_customVector_hasForcedAbsLength.boolValue);
            EditorGUILayout.PropertyField(sP_lengthRelScaleFactor_ofCustomVector, new GUIContent("Scale relative"));
            EditorGUI.EndDisabledGroup();

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

        public void DrawTextInputInclMarkupHelper(bool inputTextAs_multiLineArea = true, bool emptyLineAtEndEvenIfCollapsed = false, string overwrite_nameOfTextField = null, bool emptyLineAtEndIfOutfolded = true, bool displaySizeScalingStyleOption = true)
        {
            SerializedProperty sP_text_exclGlobalMarkupTags = serializedObject.FindProperty("text_exclGlobalMarkupTags");
            SerializedProperty sP_text_inclGlobalMarkupTags = serializedObject.FindProperty("text_inclGlobalMarkupTags");
            SerializedProperty sP_textSection_isOutfolded = serializedObject.FindProperty("textSection_isOutfolded");
            SerializedProperty sP_textStyleSection_isOutfolded = serializedObject.FindProperty("textStyleSection_isOutfolded");
            SerializedProperty sP_textMarkupHelperSection_isOutfolded = serializedObject.FindProperty("textMarkupHelperSection_isOutfolded");

            SerializedProperty sP_globalText_isBold = serializedObject.FindProperty("globalText_isBold");

            SerializedProperty sP_globalText_isStrokeWidthModified = serializedObject.FindProperty("globalText_isStrokeWidthModified");
            SerializedProperty sP_curr_strokeWidthSize0to1_forGlobalMarkup = serializedObject.FindProperty("curr_strokeWidthSize0to1_forGlobalMarkup");

            SerializedProperty sP_globalText_isItalic = serializedObject.FindProperty("globalText_isItalic");
            SerializedProperty sP_globalText_isUnderlined = serializedObject.FindProperty("globalText_isUnderlined");
            SerializedProperty sP_globalText_isDeleted = serializedObject.FindProperty("globalText_isDeleted");

            SerializedProperty sP_globalText_isSizeModified = serializedObject.FindProperty("globalText_isSizeModified");
            SerializedProperty sP_curr_sizeScaleFactor_forGlobalMarkup = serializedObject.FindProperty("curr_sizeScaleFactor_forGlobalMarkup");

            SerializedProperty sP_globalText_isColorModified = serializedObject.FindProperty("globalText_isColorModified");
            SerializedProperty sP_curr_color_forGlobalMarkup = serializedObject.FindProperty("curr_color_forGlobalMarkup");

            if (sP_text_inclGlobalMarkupTags.stringValue == null) { sP_text_inclGlobalMarkupTags.stringValue = MarkupGlobalText(sP_text_exclGlobalMarkupTags.stringValue, sP_globalText_isBold.boolValue, sP_globalText_isItalic.boolValue, sP_globalText_isUnderlined.boolValue, sP_globalText_isDeleted.boolValue, sP_globalText_isSizeModified.boolValue, sP_globalText_isColorModified.boolValue, sP_globalText_isStrokeWidthModified.boolValue, sP_curr_sizeScaleFactor_forGlobalMarkup.floatValue, sP_curr_color_forGlobalMarkup.colorValue, sP_curr_strokeWidthSize0to1_forGlobalMarkup.floatValue); }

            GUIStyle style_forFoldoutLine = new GUIStyle(EditorStyles.foldout);
            sP_textSection_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_textSection_isOutfolded.boolValue, "Text", true, style_forFoldoutLine);
            if (sP_textSection_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                string tooltip_for_inputTextField = "You can use rich text markup here, for example <b> </b> to make words bold.";
                EditorGUI.BeginChangeCheck();
                if (inputTextAs_multiLineArea)
                {
                    string used_textFieldName = (overwrite_nameOfTextField == null) ? "Drawn text tag:" : overwrite_nameOfTextField;
                    EditorGUILayout.LabelField(new GUIContent(used_textFieldName, tooltip_for_inputTextField));
                    sP_text_exclGlobalMarkupTags.stringValue = EditorGUILayout.TextArea(sP_text_exclGlobalMarkupTags.stringValue);
                }
                else
                {
                    string used_textFieldName = (overwrite_nameOfTextField == null) ? "Drawn text tag" : overwrite_nameOfTextField;
                    EditorGUILayout.PropertyField(sP_text_exclGlobalMarkupTags, new GUIContent(used_textFieldName, tooltip_for_inputTextField));
                }
                bool mainInputText_changed = EditorGUI.EndChangeCheck();
                if (mainInputText_changed) { sP_text_inclGlobalMarkupTags.stringValue = MarkupGlobalText(sP_text_exclGlobalMarkupTags.stringValue, sP_globalText_isBold.boolValue, sP_globalText_isItalic.boolValue, sP_globalText_isUnderlined.boolValue, sP_globalText_isDeleted.boolValue, sP_globalText_isSizeModified.boolValue, sP_globalText_isColorModified.boolValue, sP_globalText_isStrokeWidthModified.boolValue, sP_curr_sizeScaleFactor_forGlobalMarkup.floatValue, sP_curr_color_forGlobalMarkup.colorValue, sP_curr_strokeWidthSize0to1_forGlobalMarkup.floatValue); }

                sP_textStyleSection_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_textStyleSection_isOutfolded.boolValue, "Style", true);
                if (sP_textStyleSection_isOutfolded.boolValue)
                {
                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                    sP_text_inclGlobalMarkupTags.stringValue = DrawGlobalTextStyleOptions(sP_text_exclGlobalMarkupTags, sP_text_inclGlobalMarkupTags.stringValue, sP_globalText_isBold, sP_globalText_isStrokeWidthModified, sP_curr_strokeWidthSize0to1_forGlobalMarkup, sP_globalText_isItalic, sP_globalText_isUnderlined, sP_globalText_isDeleted, sP_globalText_isSizeModified, sP_curr_sizeScaleFactor_forGlobalMarkup, sP_globalText_isColorModified, sP_curr_color_forGlobalMarkup, displaySizeScalingStyleOption);
                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                }

                sP_textMarkupHelperSection_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_textMarkupHelperSection_isOutfolded.boolValue, "Markup snippet creator", true);
                if (sP_textMarkupHelperSection_isOutfolded.boolValue)
                {
                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                    DrawSnippetMarkupCreator();
                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                }

                if (emptyLineAtEndIfOutfolded)
                {
                    GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);
                }

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
            else
            {
                if (emptyLineAtEndEvenIfCollapsed)
                {
                    GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);
                }
            }
        }

        string DrawGlobalTextStyleOptions(SerializedProperty sP_text_exclGlobalMarkupTags, string text_inclGlobalMarkupTags, SerializedProperty sP_globalText_isBold, SerializedProperty sP_globalText_isStrokeWidthModified, SerializedProperty sP_curr_strokeWidthSize0to1_forGlobalMarkup, SerializedProperty sP_globalText_isItalic, SerializedProperty sP_globalText_isUnderlined, SerializedProperty sP_globalText_isDeleted, SerializedProperty sP_globalText_isSizeModified, SerializedProperty sP_curr_sizeScaleFactor_forGlobalMarkup, SerializedProperty sP_globalText_isColorModified, SerializedProperty sP_curr_color_forGlobalMarkup, bool displaySizeScalingStyleOption)
        {
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(sP_globalText_isBold, new GUIContent("Bold"));

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sP_globalText_isStrokeWidthModified, new GUIContent("Custom stroke width"));
            EditorGUI.BeginDisabledGroup(!sP_globalText_isStrokeWidthModified.boolValue);
            sP_curr_strokeWidthSize0to1_forGlobalMarkup.floatValue = EditorGUILayout.Slider(sP_curr_strokeWidthSize0to1_forGlobalMarkup.floatValue, 0.0f, 1.0f);
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(sP_globalText_isItalic, new GUIContent("Italic"));

            EditorGUILayout.PropertyField(sP_globalText_isUnderlined, new GUIContent("Underlined"));

            EditorGUILayout.PropertyField(sP_globalText_isDeleted, new GUIContent("Deleted"));

            if (displaySizeScalingStyleOption)
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(sP_globalText_isSizeModified, new GUIContent("Size scaling"));
                EditorGUI.BeginDisabledGroup(!sP_globalText_isSizeModified.boolValue);
                sP_curr_sizeScaleFactor_forGlobalMarkup.floatValue = EditorGUILayout.Slider(sP_curr_sizeScaleFactor_forGlobalMarkup.floatValue, 0.1f, 20.0f);
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sP_globalText_isColorModified, new GUIContent("Colored"));
            EditorGUI.BeginDisabledGroup(!sP_globalText_isColorModified.boolValue);
            sP_curr_color_forGlobalMarkup.colorValue = EditorGUILayout.ColorField(sP_curr_color_forGlobalMarkup.colorValue);
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            bool globalMarkupConfig_changed = EditorGUI.EndChangeCheck();
            if (globalMarkupConfig_changed) { text_inclGlobalMarkupTags = MarkupGlobalText(sP_text_exclGlobalMarkupTags.stringValue, sP_globalText_isBold.boolValue, sP_globalText_isItalic.boolValue, sP_globalText_isUnderlined.boolValue, sP_globalText_isDeleted.boolValue, sP_globalText_isSizeModified.boolValue, sP_globalText_isColorModified.boolValue, sP_globalText_isStrokeWidthModified.boolValue, sP_curr_sizeScaleFactor_forGlobalMarkup.floatValue, sP_curr_color_forGlobalMarkup.colorValue, sP_curr_strokeWidthSize0to1_forGlobalMarkup.floatValue); }

            GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);
            return text_inclGlobalMarkupTags;
        }

        string MarkupGlobalText(string text_withoutMarkupTags, bool globalText_isBold, bool globalText_isItalic, bool globalText_isUnderlined, bool globalText_isDeleted, bool globalText_isSizeModified, bool globalText_isColorModified, bool globalText_isStrokeWidthModified, float curr_sizeScaleFactor_forGlobalMarkup, Color curr_color_forGlobalMarkup, float curr_strokeWidthSize0to1_forGlobalMarkup)
        {
            string resultingText_withMarkupTags = null;
            MarkupTextSnippet(false, ref resultingText_withMarkupTags, text_withoutMarkupTags, globalText_isBold, globalText_isItalic, globalText_isUnderlined, globalText_isDeleted, globalText_isSizeModified, globalText_isColorModified, globalText_isStrokeWidthModified, curr_sizeScaleFactor_forGlobalMarkup, curr_color_forGlobalMarkup, curr_strokeWidthSize0to1_forGlobalMarkup, false, false, false, false);
            if (resultingText_withMarkupTags == null) { resultingText_withMarkupTags = ""; }
            return resultingText_withMarkupTags;
        }

        void DrawSnippetMarkupCreator()
        {
            SerializedProperty sP_markupSnippetText_isBold = serializedObject.FindProperty("markupSnippetText_isBold");
            SerializedProperty sP_markupSnippetText_escapesBold = serializedObject.FindProperty("markupSnippetText_escapesBold");

            SerializedProperty sP_markupSnippetText_isStrokeWidthModified = serializedObject.FindProperty("markupSnippetText_isStrokeWidthModified");
            SerializedProperty sP_curr_strokeWidthSize0to1_forMarkupSnippet = serializedObject.FindProperty("curr_strokeWidthSize0to1_forMarkupSnippet");

            SerializedProperty sP_markupSnippetText_isItalic = serializedObject.FindProperty("markupSnippetText_isItalic");
            SerializedProperty sP_markupSnippetText_escapesItalic = serializedObject.FindProperty("markupSnippetText_escapesItalic");

            SerializedProperty sP_markupSnippetText_isUnderlined = serializedObject.FindProperty("markupSnippetText_isUnderlined");
            SerializedProperty sP_markupSnippetText_escapesUnderlined = serializedObject.FindProperty("markupSnippetText_escapesUnderlined");

            SerializedProperty sP_markupSnippetText_isDeleted = serializedObject.FindProperty("markupSnippetText_isDeleted");
            SerializedProperty sP_markupSnippetText_escapesDeleted = serializedObject.FindProperty("markupSnippetText_escapesDeleted");

            SerializedProperty sP_markupSnippetText_isSizeModified = serializedObject.FindProperty("markupSnippetText_isSizeModified");
            SerializedProperty sP_curr_sizeScaleFactor_forMarkupSnippet = serializedObject.FindProperty("curr_sizeScaleFactor_forMarkupSnippet");

            SerializedProperty sP_markupSnippetText_isColorModified = serializedObject.FindProperty("markupSnippetText_isColorModified");
            SerializedProperty sP_curr_color_forMarkupSnippet = serializedObject.FindProperty("curr_color_forMarkupSnippet");

            SerializedProperty sP_curr_heightOfEmptyLine_forMarkupCreator = serializedObject.FindProperty("curr_heightOfEmptyLine_forMarkupCreator");

            EditorGUILayout.HelpBox("You can use this to easily create text snippets inside markup tags that you can copy into the drawn text field (or into your code)." + Environment.NewLine + Environment.NewLine + "With the created snippets you can have for each word (or letter) different styles that differ from to the global 'Style' settings above.", MessageType.None, true);
            GUILayout.Space(spaceBetweenMarkupStyleOptions);

            SerializedProperty sP_textSnippet_toPutInMarkupTags = serializedObject.FindProperty("textSnippet_toPutInMarkupTags");
            SerializedProperty sP_curr_iconType_forMarkupCreator = serializedObject.FindProperty("curr_iconType_forMarkupCreator");
            SerializedProperty sP_curr_textMarkupInputSnippetOptions = serializedObject.FindProperty("curr_textMarkupInputSnippetOptions");
            SerializedProperty sP_curr_logType_forMarkupCreator = serializedObject.FindProperty("curr_logType_forMarkupCreator");

            SerializedProperty sP_resultTextSnippet_insideMarkupTags_forTextInput = serializedObject.FindProperty("resultTextSnippet_insideMarkupTags_forTextInput");
            SerializedProperty sP_resultTextSnippet_insideMarkupTags_forIconInput = serializedObject.FindProperty("resultTextSnippet_insideMarkupTags_forIconInput");
            SerializedProperty sP_resultTextSnippet_insideMarkupTags_forCustomHeightEmptyLineInput = serializedObject.FindProperty("resultTextSnippet_insideMarkupTags_forCustomHeightEmptyLineInput");
            SerializedProperty sP_resultTextSnippet_insideMarkupTags_forLogSymbolInput = serializedObject.FindProperty("resultTextSnippet_insideMarkupTags_forLogSymbolInput");

            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(sP_curr_textMarkupInputSnippetOptions, new GUIContent("Input type"));

            GUILayout.Space(spaceBetweenMarkupStyleOptions);

            GUIStyle style_ofSnippetCreatorSubHeadlines = new GUIStyle();
            style_ofSnippetCreatorSubHeadlines.fontStyle = FontStyle.Bold;
            EditorGUILayout.LabelField("Snippet", style_ofSnippetCreatorSubHeadlines);

            switch (sP_curr_textMarkupInputSnippetOptions.enumValueIndex)
            {
                case ((int)VisualizerParent.TextMarkupInputSnippetOptions.text):
                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                    EditorGUILayout.PropertyField(sP_textSnippet_toPutInMarkupTags, new GUIContent("Input"));
                    EditorGUILayout.TextField("Output", sP_resultTextSnippet_insideMarkupTags_forTextInput.stringValue);
                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                    DrawSnippetOptionsForInputTypeOf_text(style_ofSnippetCreatorSubHeadlines, sP_markupSnippetText_isBold, sP_markupSnippetText_escapesBold, sP_markupSnippetText_isStrokeWidthModified, sP_curr_strokeWidthSize0to1_forMarkupSnippet, sP_markupSnippetText_isItalic, sP_markupSnippetText_escapesItalic, sP_markupSnippetText_isUnderlined, sP_markupSnippetText_escapesUnderlined, sP_markupSnippetText_isDeleted, sP_markupSnippetText_escapesDeleted, sP_markupSnippetText_isSizeModified, sP_curr_sizeScaleFactor_forMarkupSnippet, sP_markupSnippetText_isColorModified, sP_curr_color_forMarkupSnippet);
                    break;
                case ((int)VisualizerParent.TextMarkupInputSnippetOptions.icon):
                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                    EditorGUILayout.PropertyField(sP_curr_iconType_forMarkupCreator, new GUIContent("Icon type"));
                    EditorGUILayout.TextField("Output", sP_resultTextSnippet_insideMarkupTags_forIconInput.stringValue);
                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                    DrawSnippetOptionsForInputTypeOf_text(style_ofSnippetCreatorSubHeadlines, sP_markupSnippetText_isBold, sP_markupSnippetText_escapesBold, sP_markupSnippetText_isStrokeWidthModified, sP_curr_strokeWidthSize0to1_forMarkupSnippet, sP_markupSnippetText_isItalic, sP_markupSnippetText_escapesItalic, sP_markupSnippetText_isUnderlined, sP_markupSnippetText_escapesUnderlined, sP_markupSnippetText_isDeleted, sP_markupSnippetText_escapesDeleted, sP_markupSnippetText_isSizeModified, sP_curr_sizeScaleFactor_forMarkupSnippet, sP_markupSnippetText_isColorModified, sP_curr_color_forMarkupSnippet);
                    break;
                case ((int)VisualizerParent.TextMarkupInputSnippetOptions.customHeightEmptyLine):
                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                    EditorGUILayout.TextField("Output", sP_resultTextSnippet_insideMarkupTags_forCustomHeightEmptyLineInput.stringValue);
                    DrawSnippetOptionsForInputTypeOf_customHeightEmptyLine(sP_curr_heightOfEmptyLine_forMarkupCreator);
                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                    break;
                case ((int)VisualizerParent.TextMarkupInputSnippetOptions.logSymbol):
                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                    EditorGUILayout.PropertyField(sP_curr_logType_forMarkupCreator, new GUIContent("Log type"));
                    EditorGUILayout.TextField("Output", sP_resultTextSnippet_insideMarkupTags_forLogSymbolInput.stringValue);
                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                    DrawSnippetOptionsForInputTypeOf_logSymbol(style_ofSnippetCreatorSubHeadlines, sP_markupSnippetText_isBold, sP_markupSnippetText_escapesBold, sP_markupSnippetText_isStrokeWidthModified, sP_curr_strokeWidthSize0to1_forMarkupSnippet, sP_markupSnippetText_isItalic, sP_markupSnippetText_escapesItalic, sP_markupSnippetText_isUnderlined, sP_markupSnippetText_escapesUnderlined, sP_markupSnippetText_isDeleted, sP_markupSnippetText_escapesDeleted, sP_markupSnippetText_isSizeModified, sP_curr_sizeScaleFactor_forMarkupSnippet);
                    break;
                case ((int)VisualizerParent.TextMarkupInputSnippetOptions.lineBreak):
                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                    EditorGUILayout.TextField("Output", "<br>");
                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                    break;
                default:
                    break;
            }
            bool markupSnippedInputConfig_changed = EditorGUI.EndChangeCheck();
            if (markupSnippedInputConfig_changed) { MarkupSnippets(ref sP_resultTextSnippet_insideMarkupTags_forTextInput, ref sP_resultTextSnippet_insideMarkupTags_forIconInput, ref sP_resultTextSnippet_insideMarkupTags_forCustomHeightEmptyLineInput, ref sP_resultTextSnippet_insideMarkupTags_forLogSymbolInput, sP_textSnippet_toPutInMarkupTags.stringValue, (DrawBasics.IconType)sP_curr_iconType_forMarkupCreator.enumValueIndex, (LogType)sP_curr_logType_forMarkupCreator.enumValueIndex, sP_curr_heightOfEmptyLine_forMarkupCreator.floatValue, sP_markupSnippetText_isBold.boolValue, sP_markupSnippetText_isItalic.boolValue, sP_markupSnippetText_isUnderlined.boolValue, sP_markupSnippetText_isDeleted.boolValue, sP_markupSnippetText_isSizeModified.boolValue, sP_markupSnippetText_isColorModified.boolValue, sP_markupSnippetText_isStrokeWidthModified.boolValue, sP_curr_sizeScaleFactor_forMarkupSnippet.floatValue, sP_curr_color_forMarkupSnippet.colorValue, sP_curr_strokeWidthSize0to1_forMarkupSnippet.floatValue, sP_markupSnippetText_escapesBold.boolValue, sP_markupSnippetText_escapesItalic.boolValue, sP_markupSnippetText_escapesUnderlined.boolValue, sP_markupSnippetText_escapesDeleted.boolValue); }
        }

        void MarkupSnippets(ref SerializedProperty sP_resultTextSnippet_insideMarkupTags_forTextInput, ref SerializedProperty sP_resultTextSnippet_insideMarkupTags_forIconInput, ref SerializedProperty sP_resultTextSnippet_insideMarkupTags_forCustomHeightEmptyLineInput, ref SerializedProperty sP_resultTextSnippet_insideMarkupTags_forLogSymbolInput, string textSnippet_toPutInMarkupTags, DrawBasics.IconType curr_iconType_forMarkupCreator, LogType curr_logType_forMarkupCreator, float curr_heightOfEmptyLine_forMarkupCreator, bool markupSnippetText_isBold, bool markupSnippetText_isItalic, bool markupSnippetText_isUnderlined, bool markupSnippetText_isDeleted, bool markupSnippetText_isSizeModified, bool markupSnippetText_isColorModified, bool markupSnippetText_isStrokeWidthModified, float curr_sizeScaleFactor_forMarkupSnippet, Color curr_color_forMarkupSnippet, float curr_strokeWidthSize0to1_forMarkupSnippet, bool markupSnippetText_escapesBold, bool markupSnippetText_escapesItalic, bool markupSnippetText_escapesUnderlined, bool markupSnippetText_escapesDeleted)
        {
            string resultTextSnippet_insideMarkupTags_forTextInput = null;
            MarkupTextSnippet(true, ref resultTextSnippet_insideMarkupTags_forTextInput, textSnippet_toPutInMarkupTags, markupSnippetText_isBold, markupSnippetText_isItalic, markupSnippetText_isUnderlined, markupSnippetText_isDeleted, markupSnippetText_isSizeModified, markupSnippetText_isColorModified, markupSnippetText_isStrokeWidthModified, curr_sizeScaleFactor_forMarkupSnippet, curr_color_forMarkupSnippet, curr_strokeWidthSize0to1_forMarkupSnippet, markupSnippetText_escapesBold, markupSnippetText_escapesItalic, markupSnippetText_escapesUnderlined, markupSnippetText_escapesDeleted);
            if (resultTextSnippet_insideMarkupTags_forTextInput == null) { resultTextSnippet_insideMarkupTags_forTextInput = ""; }
            sP_resultTextSnippet_insideMarkupTags_forTextInput.stringValue = resultTextSnippet_insideMarkupTags_forTextInput;

            string resultTextSnippet_insideMarkupTags_forIconInput = null;
            MarkupTextSnippet(true, ref resultTextSnippet_insideMarkupTags_forIconInput, DrawText.MarkupIcon(curr_iconType_forMarkupCreator), markupSnippetText_isBold, markupSnippetText_isItalic, markupSnippetText_isUnderlined, markupSnippetText_isDeleted, markupSnippetText_isSizeModified, markupSnippetText_isColorModified, markupSnippetText_isStrokeWidthModified, curr_sizeScaleFactor_forMarkupSnippet, curr_color_forMarkupSnippet, curr_strokeWidthSize0to1_forMarkupSnippet, markupSnippetText_escapesBold, markupSnippetText_escapesItalic, markupSnippetText_escapesUnderlined, markupSnippetText_escapesDeleted);
            if (resultTextSnippet_insideMarkupTags_forIconInput == null) { resultTextSnippet_insideMarkupTags_forIconInput = ""; }
            sP_resultTextSnippet_insideMarkupTags_forIconInput.stringValue = resultTextSnippet_insideMarkupTags_forIconInput;

            string resultTextSnippet_insideMarkupTags_forLogSymbolInput = null;
            MarkupTextSnippet(true, ref resultTextSnippet_insideMarkupTags_forLogSymbolInput, DrawText.MarkupLogSymbol(curr_logType_forMarkupCreator), markupSnippetText_isBold, markupSnippetText_isItalic, markupSnippetText_isUnderlined, markupSnippetText_isDeleted, markupSnippetText_isSizeModified, false, markupSnippetText_isStrokeWidthModified, curr_sizeScaleFactor_forMarkupSnippet, default(Color), curr_strokeWidthSize0to1_forMarkupSnippet, markupSnippetText_escapesBold, markupSnippetText_escapesItalic, markupSnippetText_escapesUnderlined, markupSnippetText_escapesDeleted);
            if (resultTextSnippet_insideMarkupTags_forLogSymbolInput == null) { resultTextSnippet_insideMarkupTags_forLogSymbolInput = ""; }
            sP_resultTextSnippet_insideMarkupTags_forLogSymbolInput.stringValue = resultTextSnippet_insideMarkupTags_forLogSymbolInput;

            sP_resultTextSnippet_insideMarkupTags_forCustomHeightEmptyLineInput.stringValue = DrawText.MarkupCustomHeightEmptyLine(curr_heightOfEmptyLine_forMarkupCreator);
        }

        void MarkupTextSnippet(bool isForMarkupSnippet, ref string stringToFill, string text_withoutMarkupTags, bool isBold, bool isItalic, bool isUnderlined, bool isDeleted, bool isSizeModified, bool isColorModified, bool isStrokeWidthModified, float sizeScaleFactor, Color color, float strokeWidthSizeInPPMofSize, bool markupSnippetText_escapesBold, bool markupSnippetText_escapesItalic, bool markupSnippetText_escapesUnderlined, bool markupSnippetText_escapesDeleted)
        {
            if (text_withoutMarkupTags != null && text_withoutMarkupTags != "")
            {
                stringToFill = "" + text_withoutMarkupTags;

                if (isBold)
                {
                    stringToFill = DrawText.MarkupBold(stringToFill);
                }
                else
                {
                    if (isForMarkupSnippet)
                    {
                        if (markupSnippetText_escapesBold)
                        {
                            stringToFill = DrawText.MarkupBoldEscape(stringToFill);
                        }
                    }
                }

                if (isItalic)
                {
                    stringToFill = DrawText.MarkupItalic(stringToFill);
                }
                else
                {
                    if (isForMarkupSnippet)
                    {
                        if (markupSnippetText_escapesItalic)
                        {
                            stringToFill = DrawText.MarkupItalicEscape(stringToFill);
                        }
                    }
                }

                if (isUnderlined)
                {
                    stringToFill = DrawText.MarkupUnderlined(stringToFill);
                }
                else
                {
                    if (isForMarkupSnippet)
                    {
                        if (markupSnippetText_escapesUnderlined)
                        {
                            stringToFill = DrawText.MarkupUnderlinedEscape(stringToFill);
                        }
                    }
                }

                if (isDeleted)
                {
                    stringToFill = DrawText.MarkupDeleted(stringToFill);
                }
                else
                {
                    if (isForMarkupSnippet)
                    {
                        if (markupSnippetText_escapesDeleted)
                        {
                            stringToFill = DrawText.MarkupDeletedEscape(stringToFill);
                        }
                    }
                }

                if (isSizeModified)
                {
                    stringToFill = DrawText.MarkupSize(stringToFill, sizeScaleFactor);
                }

                if (isColorModified)
                {
                    stringToFill = DrawText.MarkupColor(stringToFill, color);
                }

                if (isStrokeWidthModified)
                {
                    stringToFill = DrawText.MarkupStrokeWidth(stringToFill, Float0to1_to_intStrokeWidthInPPMofSize(strokeWidthSizeInPPMofSize));
                }
            }
        }

        int Float0to1_to_intStrokeWidthInPPMofSize(float float0to1)
        {
            return Mathf.RoundToInt(float0to1 * UtilitiesDXXL_Text.maxRelStrokeWidth_inPPMofSize);
        }

        float spaceBetweenMarkupStyleOptions = 0.5f * EditorGUIUtility.singleLineHeight;
        void DrawSnippetOptionsForInputTypeOf_text(GUIStyle style_ofSnippetCreatorSubHeadlines, SerializedProperty sP_markupSnippetText_isBold, SerializedProperty sP_markupSnippetText_escapesBold, SerializedProperty sP_markupSnippetText_isStrokeWidthModified, SerializedProperty sP_curr_strokeWidthSize0to1_forMarkupSnippet, SerializedProperty sP_markupSnippetText_isItalic, SerializedProperty sP_markupSnippetText_escapesItalic, SerializedProperty sP_markupSnippetText_isUnderlined, SerializedProperty sP_markupSnippetText_escapesUnderlined, SerializedProperty sP_markupSnippetText_isDeleted, SerializedProperty sP_markupSnippetText_escapesDeleted, SerializedProperty sP_markupSnippetText_isSizeModified, SerializedProperty sP_curr_sizeScaleFactor_forMarkupSnippet, SerializedProperty sP_markupSnippetText_isColorModified, SerializedProperty sP_curr_color_forMarkupSnippet)
        {
            GUILayout.Space(spaceBetweenMarkupStyleOptions);
            DrawSnippetOption_bold(style_ofSnippetCreatorSubHeadlines, sP_markupSnippetText_isBold, sP_markupSnippetText_escapesBold);
            GUILayout.Space(spaceBetweenMarkupStyleOptions);
            DrawSnippetOption_strokeWidth(style_ofSnippetCreatorSubHeadlines, sP_markupSnippetText_isStrokeWidthModified, sP_curr_strokeWidthSize0to1_forMarkupSnippet);
            GUILayout.Space(spaceBetweenMarkupStyleOptions);
            DrawSnippetOption_italic(style_ofSnippetCreatorSubHeadlines, sP_markupSnippetText_isItalic, sP_markupSnippetText_escapesItalic);
            GUILayout.Space(spaceBetweenMarkupStyleOptions);
            DrawSnippetOption_underlined(style_ofSnippetCreatorSubHeadlines, sP_markupSnippetText_isUnderlined, sP_markupSnippetText_escapesUnderlined);
            GUILayout.Space(spaceBetweenMarkupStyleOptions);
            DrawSnippetOption_deleted(style_ofSnippetCreatorSubHeadlines, sP_markupSnippetText_isDeleted, sP_markupSnippetText_escapesDeleted);
            GUILayout.Space(spaceBetweenMarkupStyleOptions);
            DrawSnippetOption_sizeScaling(style_ofSnippetCreatorSubHeadlines, sP_markupSnippetText_isSizeModified, sP_curr_sizeScaleFactor_forMarkupSnippet);
            GUILayout.Space(spaceBetweenMarkupStyleOptions);
            DrawSnippetOption_color(style_ofSnippetCreatorSubHeadlines, sP_markupSnippetText_isColorModified, sP_curr_color_forMarkupSnippet);
        }

        void DrawSnippetOptionsForInputTypeOf_customHeightEmptyLine(SerializedProperty sP_curr_heightOfEmptyLine_forMarkupCreator)
        {
            EditorGUILayout.PropertyField(sP_curr_heightOfEmptyLine_forMarkupCreator, new GUIContent("Gap size", "A value of 1 means the gap has the height of 1 line."));
        }

        void DrawSnippetOptionsForInputTypeOf_logSymbol(GUIStyle style_ofSnippetCreatorSubHeadlines, SerializedProperty sP_markupSnippetText_isBold, SerializedProperty sP_markupSnippetText_escapesBold, SerializedProperty sP_markupSnippetText_isStrokeWidthModified, SerializedProperty sP_curr_strokeWidthSize0to1_forMarkupSnippet, SerializedProperty sP_markupSnippetText_isItalic, SerializedProperty sP_markupSnippetText_escapesItalic, SerializedProperty sP_markupSnippetText_isUnderlined, SerializedProperty sP_markupSnippetText_escapesUnderlined, SerializedProperty sP_markupSnippetText_isDeleted, SerializedProperty sP_markupSnippetText_escapesDeleted, SerializedProperty sP_markupSnippetText_isSizeModified, SerializedProperty sP_curr_sizeScaleFactor_forMarkupSnippet)
        {
            GUILayout.Space(spaceBetweenMarkupStyleOptions);
            DrawSnippetOption_bold(style_ofSnippetCreatorSubHeadlines, sP_markupSnippetText_isBold, sP_markupSnippetText_escapesBold);
            GUILayout.Space(spaceBetweenMarkupStyleOptions);
            DrawSnippetOption_strokeWidth(style_ofSnippetCreatorSubHeadlines, sP_markupSnippetText_isStrokeWidthModified, sP_curr_strokeWidthSize0to1_forMarkupSnippet);
            GUILayout.Space(spaceBetweenMarkupStyleOptions);
            DrawSnippetOption_italic(style_ofSnippetCreatorSubHeadlines, sP_markupSnippetText_isItalic, sP_markupSnippetText_escapesItalic);
            GUILayout.Space(spaceBetweenMarkupStyleOptions);
            DrawSnippetOption_underlined(style_ofSnippetCreatorSubHeadlines, sP_markupSnippetText_isUnderlined, sP_markupSnippetText_escapesUnderlined);
            GUILayout.Space(spaceBetweenMarkupStyleOptions);
            DrawSnippetOption_deleted(style_ofSnippetCreatorSubHeadlines, sP_markupSnippetText_isDeleted, sP_markupSnippetText_escapesDeleted);
            GUILayout.Space(spaceBetweenMarkupStyleOptions);
            DrawSnippetOption_sizeScaling(style_ofSnippetCreatorSubHeadlines, sP_markupSnippetText_isSizeModified, sP_curr_sizeScaleFactor_forMarkupSnippet);
        }

        void DrawSnippetOption_bold(GUIStyle style_ofSnippetCreatorSubHeadlines, SerializedProperty sP_markupSnippetText_isBold, SerializedProperty sP_markupSnippetText_escapesBold)
        {
            EditorGUILayout.LabelField("Bold", style_ofSnippetCreatorSubHeadlines);
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            EditorGUI.BeginDisabledGroup(sP_markupSnippetText_escapesBold.boolValue);
            EditorGUILayout.PropertyField(sP_markupSnippetText_isBold, new GUIContent("Enable"));
            EditorGUI.EndDisabledGroup();
            GUIContent guiContent_for_escapeToggle = new GUIContent("Escape enclosing", "If you copy the created snippet into a text that is already bold, then this will remove the enclosing bold style from the snippet." + Environment.NewLine + Environment.NewLine + "Warning: If the enclosing text is NOT bold this will lead to a confusing faulty final text.");
            EditorGUILayout.PropertyField(sP_markupSnippetText_escapesBold, guiContent_for_escapeToggle);
            if (sP_markupSnippetText_escapesBold.boolValue) { sP_markupSnippetText_isBold.boolValue = false; }
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

        void DrawSnippetOption_strokeWidth(GUIStyle style_ofSnippetCreatorSubHeadlines, SerializedProperty sP_markupSnippetText_isStrokeWidthModified, SerializedProperty sP_curr_strokeWidthSize0to1_forMarkupSnippet)
        {
            EditorGUILayout.LabelField("Stroke width", style_ofSnippetCreatorSubHeadlines);
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sP_markupSnippetText_isStrokeWidthModified, new GUIContent("Enable"));
            EditorGUI.BeginDisabledGroup(!sP_markupSnippetText_isStrokeWidthModified.boolValue);
            sP_curr_strokeWidthSize0to1_forMarkupSnippet.floatValue = EditorGUILayout.Slider(sP_curr_strokeWidthSize0to1_forMarkupSnippet.floatValue, 0.0f, 1.0f);
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

        void DrawSnippetOption_italic(GUIStyle style_ofSnippetCreatorSubHeadlines, SerializedProperty sP_markupSnippetText_isItalic, SerializedProperty sP_markupSnippetText_escapesItalic)
        {
            EditorGUILayout.LabelField("Italic", style_ofSnippetCreatorSubHeadlines);
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            EditorGUI.BeginDisabledGroup(sP_markupSnippetText_escapesItalic.boolValue);
            EditorGUILayout.PropertyField(sP_markupSnippetText_isItalic, new GUIContent("Enable"));
            EditorGUI.EndDisabledGroup();
            GUIContent guiContent_for_escapeToggle = new GUIContent("Escape enclosing", "If you copy the created snippet into a text that is already italic, then this will remove the enclosing italic style from the snippet." + Environment.NewLine + Environment.NewLine + "Warning: If the enclosing text is NOT italic this will lead to a confusing faulty final text.");
            EditorGUILayout.PropertyField(sP_markupSnippetText_escapesItalic, guiContent_for_escapeToggle);
            if (sP_markupSnippetText_escapesItalic.boolValue) { sP_markupSnippetText_isItalic.boolValue = false; }
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

        void DrawSnippetOption_underlined(GUIStyle style_ofSnippetCreatorSubHeadlines, SerializedProperty sP_markupSnippetText_isUnderlined, SerializedProperty sP_markupSnippetText_escapesUnderlined)
        {
            EditorGUILayout.LabelField("Underlined", style_ofSnippetCreatorSubHeadlines);
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            EditorGUI.BeginDisabledGroup(sP_markupSnippetText_escapesUnderlined.boolValue);
            EditorGUILayout.PropertyField(sP_markupSnippetText_isUnderlined, new GUIContent("Enable"));
            EditorGUI.EndDisabledGroup();
            GUIContent guiContent_for_escapeToggle = new GUIContent("Escape enclosing", "If you copy the created snippet into a text that is already underlined, then this will remove the enclosing underlined style from the snippet." + Environment.NewLine + Environment.NewLine + "Warning: If the enclosing text is NOT underlined this will lead to a confusing faulty final text.");
            EditorGUILayout.PropertyField(sP_markupSnippetText_escapesUnderlined, guiContent_for_escapeToggle);
            if (sP_markupSnippetText_escapesUnderlined.boolValue) { sP_markupSnippetText_isUnderlined.boolValue = false; }
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

        void DrawSnippetOption_deleted(GUIStyle style_ofSnippetCreatorSubHeadlines, SerializedProperty sP_markupSnippetText_isDeleted, SerializedProperty sP_markupSnippetText_escapesDeleted)
        {
            EditorGUILayout.LabelField("Deleted", style_ofSnippetCreatorSubHeadlines);
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            EditorGUI.BeginDisabledGroup(sP_markupSnippetText_escapesDeleted.boolValue);
            EditorGUILayout.PropertyField(sP_markupSnippetText_isDeleted, new GUIContent("Enable"));
            EditorGUI.EndDisabledGroup();
            GUIContent guiContent_for_escapeToggle = new GUIContent("Escape enclosing", "If you copy the created snippet into a text that is already deleted, then this will remove the enclosing deleted style from the snippet." + Environment.NewLine + Environment.NewLine + "Warning: If the enclosing text is NOT deleted this will lead to a confusing faulty final text.");
            EditorGUILayout.PropertyField(sP_markupSnippetText_escapesDeleted, guiContent_for_escapeToggle);
            if (sP_markupSnippetText_escapesDeleted.boolValue) { sP_markupSnippetText_isDeleted.boolValue = false; }
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

        void DrawSnippetOption_sizeScaling(GUIStyle style_ofSnippetCreatorSubHeadlines, SerializedProperty sP_markupSnippetText_isSizeModified, SerializedProperty sP_curr_sizeScaleFactor_forMarkupSnippet)
        {
            EditorGUILayout.LabelField("Size scaling", style_ofSnippetCreatorSubHeadlines);
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sP_markupSnippetText_isSizeModified, new GUIContent("Enable"));
            EditorGUI.BeginDisabledGroup(!sP_markupSnippetText_isSizeModified.boolValue);
            sP_curr_sizeScaleFactor_forMarkupSnippet.floatValue = EditorGUILayout.Slider(sP_curr_sizeScaleFactor_forMarkupSnippet.floatValue, 0.1f, 20.0f);
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

        void DrawSnippetOption_color(GUIStyle style_ofSnippetCreatorSubHeadlines, SerializedProperty sP_markupSnippetText_isColorModified, SerializedProperty sP_curr_color_forMarkupSnippet)
        {
            EditorGUILayout.LabelField("Color", style_ofSnippetCreatorSubHeadlines);
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sP_markupSnippetText_isColorModified, new GUIContent("Enable"));
            EditorGUI.BeginDisabledGroup(!sP_markupSnippetText_isColorModified.boolValue);
            sP_curr_color_forMarkupSnippet.colorValue = EditorGUILayout.ColorField(sP_curr_color_forMarkupSnippet.colorValue);
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

    }
#endif
}
