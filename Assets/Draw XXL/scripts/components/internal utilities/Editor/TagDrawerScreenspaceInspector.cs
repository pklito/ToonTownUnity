namespace DrawXXL
{
    using UnityEngine;
    using System;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(TagDrawerScreenspace))]
    [CanEditMultipleObjects]
    public class TagDrawerScreenspaceInspector : VisualizerScreenspaceParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("tag");
            if (DrawCameraChooser(true))
            {
                GUILayout.Space(EditorGUIUtility.singleLineHeight);

                SerializedProperty sP_taggedPositionType = serializedObject.FindProperty("taggedPositionType");
                EditorGUILayout.PropertyField(sP_taggedPositionType, new GUIContent("Tagged position type"));

                GUILayout.Space(EditorGUIUtility.singleLineHeight);

                string displayNameOfMainColor = null;
                DrawIndividualSpecsForEachTaggedPositionType(sP_taggedPositionType, ref displayNameOfMainColor);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("linesWidth_relToViewportHeight"), new GUIContent("Lines width", "This is relative to the viewport height."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("colorForText"), new GUIContent(displayNameOfMainColor));
                DrawCheckboxFor_drawOnlyIfSelected("screenspace tag");
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }

        void DrawIndividualSpecsForEachTaggedPositionType(SerializedProperty sP_taggedPositionType, ref string displayNameOfMainColor)
        {
            switch (sP_taggedPositionType.enumValueIndex)
            {
                case (int)TagDrawerScreenspace.TaggedPositionType.positionOnViewport:
                    DrawSpecsFor_positionOnViewport(ref displayNameOfMainColor);
                    break;
                case (int)TagDrawerScreenspace.TaggedPositionType.aGameobject:
                    DrawSpecsFor_aGameobject(ref displayNameOfMainColor);
                    break;
                case (int)TagDrawerScreenspace.TaggedPositionType.multipleGameobjects:
                    DrawSpecsFor_multipleGameobjects(ref displayNameOfMainColor);
                    break;
                default:
                    break;
            }
        }

        void DrawSpecsFor_positionOnViewport(ref string displayNameOfMainColor)
        {
            DrawTextInputInclMarkupHelper(true, true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("positionInsideViewport0to1"), new GUIContent("Position (inside viewport)"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("textOffsetDistance_relToViewportHeight"), new GUIContent("Pointer size", "This is relative to the viewport height."));
            Draw_pointerDirectionSpecificationType();
            DrawForceTextSize_caseFlexiblePointerLength();
            Draw_offScreenBehaviour_forCaseOf_positionOnViewport();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("skipConeDrawing"), new GUIContent("Hide cone", "This only applies if the position is inside of the viewport."));
            displayNameOfMainColor = "Color";
        }

        void Draw_pointerDirectionSpecificationType()
        {
            SerializedProperty sP_pointerDirectionSpecificationType = serializedObject.FindProperty("pointerDirectionSpecificationType");
            EditorGUILayout.PropertyField(sP_pointerDirectionSpecificationType, new GUIContent("Pointer direction"));

            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            switch (sP_pointerDirectionSpecificationType.enumValueIndex)
            {
                case (int)TagDrawerScreenspace.PointerDirectionSpecificationType.fixedAngle:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("fixedPointerDiretion_angledDegCC"), new GUIContent("Angle"));
                    break;
                case (int)TagDrawerScreenspace.PointerDirectionSpecificationType.vanishingPointPosition:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("positionInsideViewport0to1_v2"), new GUIContent("Vanishing point"));
                    break;
                default:
                    break;
            }
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

        void Draw_offScreenBehaviour_forCaseOf_positionOnViewport()
        {
            SerializedProperty sP_drawPointerIfOffscreen = serializedObject.FindProperty("drawPointerIfOffscreen");
            EditorGUILayout.PropertyField(sP_drawPointerIfOffscreen, new GUIContent("Draw indicator if off screen"));

            EditorGUI.BeginDisabledGroup(!sP_drawPointerIfOffscreen.boolValue);
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            GUIContent guiContent_for_addTextForOutsideDistance_toOffscreenPointer = new GUIContent("Add text for outside screen distance", "This only applies if the position is outside of the viewport.");
            if (sP_drawPointerIfOffscreen.boolValue)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("addTextForOutsideDistance_toOffscreenPointer"), guiContent_for_addTextForOutsideDistance_toOffscreenPointer);
            }
            else
            {
                EditorGUILayout.Toggle(guiContent_for_addTextForOutsideDistance_toOffscreenPointer, false);
            }
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            EditorGUI.EndDisabledGroup();
        }

        void DrawSpecsFor_aGameobject(ref string displayNameOfMainColor)
        {
            DrawTextInputInclMarkupHelper(true, true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("partnerGameobject"), new GUIContent("Tagged gameobject"));
            DrawDifferentBoxColor();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("encapsulateChildren"), new GUIContent("Encapsulate children"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("drawPointerIfOffscreen"), new GUIContent("Draw indicator if off screen"));
            displayNameOfMainColor = "Text color";
        }

        void DrawSpecsFor_multipleGameobjects(ref string displayNameOfMainColor)
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("taggedScreenspaceObjects"), new GUIContent("Tagged gameobjects"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("encapsulateChildren"), new GUIContent("Encapsulate children"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("drawPointerIfOffscreen"), new GUIContent("Draw indicator if off screen"));
            displayNameOfMainColor = "Text color";
        }

        void DrawForceTextSize_caseFlexiblePointerLength()
        {
            SerializedProperty sP_forceTextSize = serializedObject.FindProperty("forceTextSize");

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sP_forceTextSize, new GUIContent("Fixed text size", "This prevents the text size from scaling with the pointer length." + Environment.NewLine + Environment.NewLine + "The text size can still be scaled via 'Text/Style/Size scaling'."));
            EditorGUI.BeginDisabledGroup(!sP_forceTextSize.boolValue);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("forceTextSize_value"), GUIContent.none);
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
        }

        void DrawDifferentBoxColor()
        {
            SerializedProperty sP_differentBoxColor = serializedObject.FindProperty("differentBoxColor");

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sP_differentBoxColor, new GUIContent("Custom box color", "With this you can define a color for the box that differs from the text color."));
            EditorGUI.BeginDisabledGroup(!sP_differentBoxColor.boolValue);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("differentBoxColor_value"), GUIContent.none);
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
        }
    }
#endif
}
