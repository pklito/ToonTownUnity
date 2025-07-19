namespace DrawXXL
{
    using UnityEngine;
    using System;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(TagDrawer))]
    [CanEditMultipleObjects]
    public class TagDrawerInspector : VisualizerParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("tag");

            EditorGUILayout.HelpBox("If you want to tag multiple gameobjects at once or want a pointer for offscreen objects you can use the 'Tag Drawer Screenspace' component instead.", MessageType.None, true);

            GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

            SerializedProperty sP_tagStyle = serializedObject.FindProperty("tagStyle");
            EditorGUILayout.PropertyField(sP_tagStyle, new GUIContent("Tag style"));

            GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

            string displayNameOfMainColor = null;
            switch (sP_tagStyle.enumValueIndex)
            {
                case (int)TagDrawer.TagStyle.pointer:
                    DrawSpecs_forPointer();
                    displayNameOfMainColor = "Color";
                    break;
                case (int)TagDrawer.TagStyle.boxed:
                    DrawSpecs_forBoxed();
                    displayNameOfMainColor = "Text color";
                    break;
                default:
                    break;
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("colorForText"), new GUIContent(displayNameOfMainColor));
            DrawCheckboxFor_drawOnlyIfSelected("tag");
            DrawCheckboxFor_hiddenByNearerObjects("tag");

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }

        void DrawSpecs_forPointer()
        {
            SerializedProperty sP_pointerSizeInterpretation = serializedObject.FindProperty("pointerSizeInterpretation");

            DrawTextSpecs(sP_pointerSizeInterpretation);
            Draw_DrawPosition3DOffset(false, "Tagged Position: Offset From Transform");
            DrawSize_ofPointer(sP_pointerSizeInterpretation);
            DrawCoordinatesOptions_ofPointer(sP_pointerSizeInterpretation);

            SerializedProperty sP_forcePointerDirection = serializedObject.FindProperty("forcePointerDirection");
            DrawSpecificationOf_customVector3_1("Force pointer direction", true, sP_forcePointerDirection, true, true, false, false);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("skipConeDrawing"), new GUIContent("Hide cone"));
            Draw_sizeInterpretationDependentLine("linesWidth", "Lines width", "Lines width", null, sP_pointerSizeInterpretation);
        }

        void DrawTextSpecs(SerializedProperty sP_pointerSizeInterpretation)
        {
            //-> this fakes the appearance of beeing inside the parents text specification foldout
            bool emptyLineAtEndIfOutfolded = false;
            DrawTextInputInclMarkupHelper(true, false, null, emptyLineAtEndIfOutfolded);

            if (serializedObject.FindProperty("textSection_isOutfolded").boolValue == true)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                DrawFixedTextSize_ofPointer(sP_pointerSizeInterpretation);
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        void DrawFixedTextSize_ofPointer(SerializedProperty sP_pointerSizeInterpretation)
        {
            if ((sP_pointerSizeInterpretation == null) || CheckIf_pointerSizeInterpretaion_isDependentOn_screenspace(sP_pointerSizeInterpretation) == false)
            {
                EditorGUILayout.LabelField("Text Size");

                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                SerializedProperty sP_attachedTextsizeReferenceContext = serializedObject.FindProperty("attachedTextsizeReferenceContext");
                EditorGUILayout.PropertyField(sP_attachedTextsizeReferenceContext, new GUIContent("Relative to"));

                switch (sP_attachedTextsizeReferenceContext.enumValueIndex)
                {
                    case (int)TagDrawer.AttachedTextsizeReferenceContext.extentOfTag:
                        break;
                    case (int)TagDrawer.AttachedTextsizeReferenceContext.globalSpace:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("textSize_value"), new GUIContent("Size per letter", "Text size in world units"));
                        break;
                    case (int)TagDrawer.AttachedTextsizeReferenceContext.sceneViewWindowSize:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("textSize_value_relToScreen"), new GUIContent("Size per letter", "Text size relative to Scene View window size."));
                        break;
                    case (int)TagDrawer.AttachedTextsizeReferenceContext.gameViewWindowSize:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("textSize_value_relToScreen"), new GUIContent("Size per letter", "Text size relative to Game View window size."));
                        break;
                    default:
                        break;
                }

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        void DrawCoordinatesOptions_ofPointer(SerializedProperty sP_pointerSizeInterpretation)
        {
            SerializedProperty sP_drawGlobalCoordinates = serializedObject.FindProperty("drawGlobalCoordinates");
            SerializedProperty sP_drawLocalCoordinates = serializedObject.FindProperty("drawLocalCoordinates");
            EditorGUILayout.PropertyField(sP_drawGlobalCoordinates, new GUIContent("Draw global coordinates"));

            EditorGUI.BeginDisabledGroup(visualizerParentMonoBehaviour_unserialized.transform.parent == null);
            EditorGUILayout.PropertyField(sP_drawLocalCoordinates, new GUIContent("Draw local coordinates"));
            EditorGUI.EndDisabledGroup();

            DrawSizeAndTextBoldness_ofMarkingCross_ofPointer(sP_drawGlobalCoordinates, sP_drawLocalCoordinates, sP_pointerSizeInterpretation);
        }

        void DrawSizeAndTextBoldness_ofMarkingCross_ofPointer(SerializedProperty sP_drawGlobalCoordinates, SerializedProperty sP_drawLocalCoordinates, SerializedProperty sP_pointerSizeInterpretation)
        {
            bool sizeOfMarkingCross_isGreyedOut = ((sP_drawGlobalCoordinates.boolValue == false) && (sP_drawLocalCoordinates.boolValue == false));
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            EditorGUI.BeginDisabledGroup(sizeOfMarkingCross_isGreyedOut);
            Draw_sizeInterpretationDependentLine("sizeOfMarkingCross", "Size of coordinates", "Size of coordinates", null, sP_pointerSizeInterpretation);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("strokeWidth_forCoordinateTexts_onPointVisualiation_in0to1"), new GUIContent("Bold Text"));
            EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

        void DrawSize_ofPointer(SerializedProperty sP_pointerSizeInterpretation)
        {
            SerializedProperty sP_textOffsetDistance_isOutfolded = serializedObject.FindProperty("textOffsetDistance_isOutfolded");
            sP_textOffsetDistance_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_textOffsetDistance_isOutfolded.boolValue, "Pointer Size", true);
            if (sP_textOffsetDistance_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                EditorGUILayout.PropertyField(sP_pointerSizeInterpretation, new GUIContent("Size interpretation", "The pointer size can be specified relative to a context of interest. The 'Length value' and along with it the text size will be interpreted according to the setting here."));

                SerializedProperty sP_cameraForSizeDefinitionIsAvailable = serializedObject.FindProperty("cameraForSizeDefinitionIsAvailable");
                if (sP_cameraForSizeDefinitionIsAvailable.boolValue == false)
                {
                    if (sP_pointerSizeInterpretation.enumValueIndex == (int)TagDrawer.PointerSizeInterpretation.relativeToTheSceneViewWindowSize)
                    {
                        EditorGUILayout.HelpBox("Scene View Camera Window is not available", MessageType.Warning, true);
                    }

                    if (sP_pointerSizeInterpretation.enumValueIndex == (int)TagDrawer.PointerSizeInterpretation.relativeToTheGameViewWindowSize)
                    {
                        EditorGUILayout.HelpBox("No Game View Camera found.", MessageType.Warning, true);
                    }
                }

                Draw_sizeInterpretationDependentLine("textOffsetDistance", "Length", "Length / Text size", null, sP_pointerSizeInterpretation);

                GUILayout.Space(EditorGUIUtility.singleLineHeight);

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        bool CheckIf_pointerSizeInterpretaion_isDependentOn_screenspace(SerializedProperty sP_pointerSizeInterpretation)
        {
            return ((sP_pointerSizeInterpretation.enumValueIndex == (int)TagDrawer.PointerSizeInterpretation.relativeToTheSceneViewWindowSize) || (sP_pointerSizeInterpretation.enumValueIndex == (int)TagDrawer.PointerSizeInterpretation.relativeToTheGameViewWindowSize));
        }

        void Draw_sizeInterpretationDependentLine(string fieldName_withoutRelToScreenSuffix, string displayName, string displayName_ifRelToScreenSize, string tooltip, SerializedProperty sP_pointerSizeInterpretation)
        {
            GUIContent guiContent;
            string toolTipSuffix = "This is relative to the reference frame defined by 'Size interpretation'";
            string used_displayName = CheckIf_pointerSizeInterpretaion_isDependentOn_screenspace(sP_pointerSizeInterpretation) ? displayName_ifRelToScreenSize : displayName;
            if (tooltip == null)
            {
                guiContent = new GUIContent(used_displayName, toolTipSuffix);
            }
            else
            {
                guiContent = new GUIContent(used_displayName, tooltip + Environment.NewLine + Environment.NewLine + toolTipSuffix);
            }

            switch ((TagDrawer.PointerSizeInterpretation)sP_pointerSizeInterpretation.enumValueIndex)
            {
                case TagDrawer.PointerSizeInterpretation.absoluteUnits:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(fieldName_withoutRelToScreenSuffix), guiContent);
                    break;
                case TagDrawer.PointerSizeInterpretation.relativeToGameobjectSize:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(fieldName_withoutRelToScreenSuffix), guiContent);
                    break;
                case TagDrawer.PointerSizeInterpretation.relativeToTheSceneViewWindowSize:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(fieldName_withoutRelToScreenSuffix + "_relToScreen"), guiContent);
                    break;
                case TagDrawer.PointerSizeInterpretation.relativeToTheGameViewWindowSize:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(fieldName_withoutRelToScreenSuffix + "_relToScreen"), guiContent);
                    break;
                default:
                    break;
            }
        }

        void DrawSpecs_forBoxed()
        {
            DrawTextSpecs(null);
            DrawDifferentBoxColor_ofBoxed();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("encapsulateChildren"), new GUIContent("Encapsulate children"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("textBlockAboveLine"), new GUIContent("Text block above line"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("linesWidth"), new GUIContent("Lines width"));
        }

        void DrawDifferentBoxColor_ofBoxed()
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
