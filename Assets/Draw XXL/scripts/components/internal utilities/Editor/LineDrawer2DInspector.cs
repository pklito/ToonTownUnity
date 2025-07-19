namespace DrawXXL
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(LineDrawer2D))]
    [CanEditMultipleObjects]
    public class LineDrawer2DInspector : LineDrawerInspector
    {
        void OnEnable()
        {
            OnEnable_base();
            OnEnable_base_atLineDrawer();
        }

        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("line2D");

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
                TryDrawConeConfig(sP_lineType);
                Draw_endPlatesConfig();
                TryDrawAlphaFadeOut(sP_lineType);
            }

            TryDrawNormalizedMarkerCheckBox(sP_lineType);

            DrawZPosChooserFor2D();
            DrawTextSpecs(sP_lineType, true);
            DrawCheckboxFor_drawOnlyIfSelected("line");
            DrawCheckboxFor_hiddenByNearerObjects("line");

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }

        public override void Draw_startPositionSection(SerializedProperty sP_lineDefinitionSection1_isOutfolded, GUIStyle style_ofFoldoutWithRichtext)
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
                        Draw_DrawPosition2DOffset(false, null, "Global Offset", "Local Offset", true);
                        break;
                    case (int)LineDrawer.PositionDefinitionOption.positionOfOtherGameobjectPlusOffset:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("partnerGameobject"), new GUIContent("Other Gameobject"));
                        Draw_DrawPosition2DOffset_ofPartnerGameobject(false, null, "Global Offset", "Local Offset", true);

                        EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("coordinateSpaceForLocalOffsetOnOtherGameobject_forStartPos"), new GUIContent("Local Offset Space"));
                        EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                        break;
                    case (int)LineDrawer.PositionDefinitionOption.chooseFree:
                        DrawSpecificationOf_customVector2_3(null, false, null, false, false, false, false, true, false);
                        break;
                    default:
                        break;
                }

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }
        }

        public override void Draw_endPositionSection(SerializedProperty sP_lineDefinitionSection2_isOutfolded, GUIStyle style_ofFoldoutWithRichtext)
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
                        Draw_DrawPosition2DOffset_independentAlternativeValue(false, null, "Global Offset", "Local Offset", true);
                        break;
                    case (int)LineDrawer.PositionDefinitionOption.positionOfOtherGameobjectPlusOffset:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("partnerGameobject_independentAlternativeValue"), new GUIContent("Other Gameobject"));
                        Draw_DrawPosition2DOffset_ofPartnerGameobject_independentAlternativeValue(false, null, "Global Offset", "Local Offset", true);

                        EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("coordinateSpaceForLocalOffsetOnOtherGameobject_forEndPos"), new GUIContent("Local Offset Space"));
                        EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                        break;
                    case (int)LineDrawer.PositionDefinitionOption.chooseFree:
                        DrawSpecificationOf_customVector2_4(null, false, null, false, false, false, false, true, false);
                        break;
                    default:
                        break;
                }

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }
        }

        public override void Draw_lineVectorSection(SerializedProperty sP_lineDefinitionSection_isOutfolded, string headline, GUIStyle style_ofFoldoutWithRichtext)
        {
            sP_lineDefinitionSection_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_lineDefinitionSection_isOutfolded.boolValue, headline, true, style_ofFoldoutWithRichtext);
            if (sP_lineDefinitionSection_isOutfolded.boolValue)
            {
                DrawSpecificationOf_customVector2_1(null, false, null, false, false, false, false, true);
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }
        }

    }
#endif
}
