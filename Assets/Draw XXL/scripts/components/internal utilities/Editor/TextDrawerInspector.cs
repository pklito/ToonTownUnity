namespace DrawXXL
{
    using UnityEngine;
    using System;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(TextDrawer))]
    [CanEditMultipleObjects]
    public class TextDrawerInspector : VisualizerParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("text");
            DrawTextInputInclMarkupHelper(true, true, "Drawn text:");
            SerializedProperty sP_forceTextTo_facingToSceneViewCam = serializedObject.FindProperty("forceTextTo_facingToSceneViewCam");
            SerializedProperty sP_forceTextTo_facingToGameViewCam = serializedObject.FindProperty("forceTextTo_facingToGameViewCam");
            DrawOrientationSpecs(sP_forceTextTo_facingToSceneViewCam, sP_forceTextTo_facingToGameViewCam);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("color"), new GUIContent("Color", "This can be overwritten by 'Text/Style/Colored', but then still defines the color of a frame box."));
            SerializedProperty sP_sizeInterpretation = serializedObject.FindProperty("sizeInterpretation");
            SerializedProperty sP_forceTextEnlargementToThisMinWidth = serializedObject.FindProperty("forceTextEnlargementToThisMinWidth");
            SerializedProperty sP_forceRestrictTextSizeToThisMaxTextWidth = serializedObject.FindProperty("forceRestrictTextSizeToThisMaxTextWidth");
            DrawSizeSpecs(sP_sizeInterpretation, sP_forceTextEnlargementToThisMinWidth, sP_forceRestrictTextSizeToThisMaxTextWidth, "The here used gameobject size is defined by the biggest absolute global scale dimension of the transform.");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("textAnchor"), new GUIContent("Text anchor"));
            DrawFrameBoxSpecs();
            Draw_TextBlockConstraints(sP_sizeInterpretation, sP_forceTextEnlargementToThisMinWidth, sP_forceRestrictTextSizeToThisMaxTextWidth);
            DrawAutoFlipCheckbox(sP_forceTextTo_facingToSceneViewCam, sP_forceTextTo_facingToGameViewCam);
            Draw_DrawPosition3DOffset();
            DrawCheckboxFor_drawOnlyIfSelected("text");
            DrawCheckboxFor_hiddenByNearerObjects("text");

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }

        void DrawOrientationSpecs(SerializedProperty sP_forceTextTo_facingToSceneViewCam, SerializedProperty sP_forceTextTo_facingToGameViewCam)
        {
            bool forceToSceneViewCam_isGreyedOut = false;
            bool forceToGameViewCam_isGreyedOut = false;

            if (sP_forceTextTo_facingToGameViewCam.boolValue == true)
            {
                sP_forceTextTo_facingToSceneViewCam.boolValue = false;
                forceToSceneViewCam_isGreyedOut = true;
            }

            if (sP_forceTextTo_facingToSceneViewCam.boolValue == true)
            {
                sP_forceTextTo_facingToGameViewCam.boolValue = false;
                forceToGameViewCam_isGreyedOut = true;
            }

            EditorGUI.BeginDisabledGroup(forceToSceneViewCam_isGreyedOut);
            EditorGUILayout.PropertyField(sP_forceTextTo_facingToSceneViewCam, new GUIContent("Force facing to Scene view camera"));
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(forceToGameViewCam_isGreyedOut);
            EditorGUILayout.PropertyField(sP_forceTextTo_facingToGameViewCam, new GUIContent("Force facing to Game view camera"));
            EditorGUI.EndDisabledGroup();

            if (sP_forceTextTo_facingToGameViewCam.boolValue == true)
            {
                visualizerParentMonoBehaviour_unserialized.observerCamera_ofCustomVector3_3 = DrawBasics.CameraForAutomaticOrientation.gameViewCamera;
                visualizerParentMonoBehaviour_unserialized.observerCamera_ofCustomVector3_4 = DrawBasics.CameraForAutomaticOrientation.gameViewCamera;
            }

            if (sP_forceTextTo_facingToSceneViewCam.boolValue == true)
            {
                visualizerParentMonoBehaviour_unserialized.observerCamera_ofCustomVector3_3 = DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;
                visualizerParentMonoBehaviour_unserialized.observerCamera_ofCustomVector3_4 = DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;
            }

            visualizerParentMonoBehaviour_unserialized.source_ofCustomVector3_3 = VisualizerParent.CustomVector3Source.observerCameraRight;
            visualizerParentMonoBehaviour_unserialized.source_ofCustomVector3_4 = VisualizerParent.CustomVector3Source.observerCameraUp;

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            bool customVectorPickers_useGreyedOutVectors_3and4 = ((sP_forceTextTo_facingToGameViewCam.boolValue == true) || (sP_forceTextTo_facingToSceneViewCam.boolValue == true));
            if (customVectorPickers_useGreyedOutVectors_3and4)
            {
                EditorGUI.BeginDisabledGroup(true);
                DrawSpecificationOf_customVector3_3("Text direction", false, null, true, true, true, false);
                DrawSpecificationOf_customVector3_4("Text upward direction", false, null, true, true, true, true);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                DrawSpecificationOf_customVector3_1("Text direction", false, null, true, true, true, false);
                DrawSpecificationOf_customVector3_2("Text upward direction", false, null, true, true, true, true);
            }
        }

        public void DrawSizeSpecs(SerializedProperty sP_sizeInterpretation, SerializedProperty sP_forceTextEnlargementToThisMinWidth, SerializedProperty sP_forceRestrictTextSizeToThisMaxTextWidth, string tooltip_for_relToGamebobjecSizeInterpretation)
        {
            SerializedProperty sP_size_isOutfolded = serializedObject.FindProperty("size_isOutfolded");
            sP_size_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_size_isOutfolded.boolValue, "Text Size", true);
            if (sP_size_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.HelpBox("This defines the width per letter.", MessageType.None, true);

                EditorGUILayout.PropertyField(sP_sizeInterpretation, new GUIContent("Relative to", "The text size can be specified relative to a context of interest. The following size value will be scaled according to this interpretation setting here."));

                string name_ofSizeLine = "Size value";
                switch (sP_sizeInterpretation.enumValueIndex)
                {
                    case (int)TextDrawer.SizeInterpretation.globalSpace:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("size"), new GUIContent(name_ofSizeLine));
                        break;
                    case (int)TextDrawer.SizeInterpretation.sizeOfGameobject:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("size"), new GUIContent(name_ofSizeLine, tooltip_for_relToGamebobjecSizeInterpretation));
                        break;
                    case (int)TextDrawer.SizeInterpretation.sceneViewWindowWidth:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("size_relToScreen"), new GUIContent(name_ofSizeLine));
                        break;
                    case (int)TextDrawer.SizeInterpretation.gameViewWindowWidth:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("size_relToScreen"), new GUIContent(name_ofSizeLine));
                        break;
                    default:
                        break;
                }

                if (sP_forceTextEnlargementToThisMinWidth.boolValue || sP_forceRestrictTextSizeToThisMaxTextWidth.boolValue)
                {
                    EditorGUILayout.HelpBox("This text size may get overwritten due to the activated 'Minimum/Maximum text block width'.", MessageType.None, false);
                }

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

                GUILayout.Space(0.75f * EditorGUIUtility.singleLineHeight);
            }
        }

        public void DrawFrameBoxSpecs()
        {
            SerializedProperty sP_enclosingBox_isOutfolded = serializedObject.FindProperty("enclosingBox_isOutfolded");
            GUIContent guiContent_ofFrameBoxHeader = new GUIContent("Frame box", "If you want to have a different color for the box and the text you can use 'Text/Style/Colored' to overwrite the text color.");
            sP_enclosingBox_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_enclosingBox_isOutfolded.boolValue, guiContent_ofFrameBoxHeader, true);
            if (sP_enclosingBox_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                SerializedProperty sP_enclosingBoxLineStyle = serializedObject.FindProperty("enclosingBoxLineStyle");
                EditorGUILayout.PropertyField(sP_enclosingBoxLineStyle, new GUIContent("Style"));
                bool enclosingBoxIsDisabled = sP_enclosingBoxLineStyle.enumValueIndex == (int)DrawBasics.LineStyle.invisible;

                EditorGUI.BeginDisabledGroup(enclosingBoxIsDisabled);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("enclosingBox_lineWidth_relToTextSize"), new GUIContent("Width", "This is relative to the text size."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("enclosingBox_paddingSize_relToTextSize"), new GUIContent("Padding", "This is relative to the text size."));
                EditorGUI.EndDisabledGroup();

                GUILayout.Space(EditorGUIUtility.singleLineHeight);

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        public void Draw_TextBlockConstraints(SerializedProperty sP_sizeInterpretation, SerializedProperty sP_forceTextEnlargementToThisMinWidth, SerializedProperty sP_forceRestrictTextSizeToThisMaxTextWidth)
        {
            SerializedProperty sP_autoLineBreakWidth_isOutfolded = serializedObject.FindProperty("autoLineBreakWidth_isOutfolded");
            SerializedProperty sP_autoLineBreakWidth = serializedObject.FindProperty("autoLineBreakWidth");
            SerializedProperty sP_autoLineBreakWidth_value = serializedObject.FindProperty("autoLineBreakWidth_value");
            SerializedProperty sP_autoLineBreakWidth_value_relToScreen = serializedObject.FindProperty("autoLineBreakWidth_value_relToScreen");
            SerializedProperty sP_autoLineBreakWidth_interpretation = serializedObject.FindProperty("autoLineBreakWidth_interpretation");

            SerializedProperty sP_forceTextEnlargementToThisMinWidth_isOutfolded = serializedObject.FindProperty("forceTextEnlargementToThisMinWidth_isOutfolded");
            SerializedProperty sP_forceTextEnlargementToThisMinWidth_value = serializedObject.FindProperty("forceTextEnlargementToThisMinWidth_value");
            SerializedProperty sP_forceTextEnlargementToThisMinWidth_value_relToScreen = serializedObject.FindProperty("forceTextEnlargementToThisMinWidth_value_relToScreen");
            SerializedProperty sP_forceTextEnlargementToThisMinWidth_interpretation = serializedObject.FindProperty("forceTextEnlargementToThisMinWidth_interpretation");

            SerializedProperty sP_forceRestrictTextSizeToThisMaxTextWidth_isOutfolded = serializedObject.FindProperty("forceRestrictTextSizeToThisMaxTextWidth_isOutfolded");
            SerializedProperty sP_forceRestrictTextSizeToThisMaxTextWidth_value = serializedObject.FindProperty("forceRestrictTextSizeToThisMaxTextWidth_value");
            SerializedProperty sP_forceRestrictTextSizeToThisMaxTextWidth_value_relToScreen = serializedObject.FindProperty("forceRestrictTextSizeToThisMaxTextWidth_value_relToScreen");
            SerializedProperty sP_forceRestrictTextSizeToThisMaxTextWidth_interpretation = serializedObject.FindProperty("forceRestrictTextSizeToThisMaxTextWidth_interpretation");

            GUIContent used_GUIContent;
            GUIContent used_GUIContentOfEnableToggle;

            string helpBoxText_for_autoLineBreakWidth = "This automatically inserts line breaks into the text, so that the width of the whole text block doesn't exceed the specified width.";
            used_GUIContent = new GUIContent("Automatic line breaks");
            used_GUIContentOfEnableToggle = new GUIContent("Force line breaks after width span");
            Draw_TextBlockConstraint(used_GUIContent, used_GUIContentOfEnableToggle, "Maximum line width", helpBoxText_for_autoLineBreakWidth, sP_autoLineBreakWidth_isOutfolded, sP_autoLineBreakWidth, sP_autoLineBreakWidth_value, sP_autoLineBreakWidth_value_relToScreen, sP_autoLineBreakWidth_interpretation, sP_sizeInterpretation);

            string helpBoxText_for_forceTextEnlargementToThisMinWidth = "This overwrites the text size and rescales the text, so that the whole text block has at least the specified width.";
            used_GUIContent = new GUIContent("Minimum text block width");
            used_GUIContentOfEnableToggle = new GUIContent("Force text enlargement to reach a minimum text block width");
            EditorGUI.BeginChangeCheck();
            Draw_TextBlockConstraint(used_GUIContent, used_GUIContentOfEnableToggle, "Minimum text block width", helpBoxText_for_forceTextEnlargementToThisMinWidth, sP_forceTextEnlargementToThisMinWidth_isOutfolded, sP_forceTextEnlargementToThisMinWidth, sP_forceTextEnlargementToThisMinWidth_value, sP_forceTextEnlargementToThisMinWidth_value_relToScreen, sP_forceTextEnlargementToThisMinWidth_interpretation, sP_sizeInterpretation);
            bool minWidth_changed = EditorGUI.EndChangeCheck();

            string helpBoxText_for_forceRestrictTextSizeToThisMaxTextWidth = "This overwrites the text size and rescales the text, so that the whole text block is not wider then the specified width." + Environment.NewLine + "If you want restrict the text block width but keep the text size you can use 'Automatic line breaks'.";
            used_GUIContent = new GUIContent("Maximum text block width");
            used_GUIContentOfEnableToggle = new GUIContent("Force restrict the text block width to a maximum");
            EditorGUI.BeginChangeCheck();
            Draw_TextBlockConstraint(used_GUIContent, used_GUIContentOfEnableToggle, "Maximum text block width", helpBoxText_for_forceRestrictTextSizeToThisMaxTextWidth, sP_forceRestrictTextSizeToThisMaxTextWidth_isOutfolded, sP_forceRestrictTextSizeToThisMaxTextWidth, sP_forceRestrictTextSizeToThisMaxTextWidth_value, sP_forceRestrictTextSizeToThisMaxTextWidth_value_relToScreen, sP_forceRestrictTextSizeToThisMaxTextWidth_interpretation, sP_sizeInterpretation);
            bool maxWidth_changed = EditorGUI.EndChangeCheck();

            if (minWidth_changed)
            {
                sP_forceRestrictTextSizeToThisMaxTextWidth_value.floatValue = Mathf.Max(sP_forceTextEnlargementToThisMinWidth_value.floatValue, sP_forceRestrictTextSizeToThisMaxTextWidth_value.floatValue);
            }
            else
            {
                if (maxWidth_changed)
                {
                    sP_forceTextEnlargementToThisMinWidth_value.floatValue = Mathf.Min(sP_forceTextEnlargementToThisMinWidth_value.floatValue, sP_forceRestrictTextSizeToThisMaxTextWidth_value.floatValue);
                }
            }
        }

        void Draw_TextBlockConstraint(GUIContent used_GUIContentOfHeadline, GUIContent used_GUIContentOfEnableToggle, string used_nameString_ofValue, string helpBoxText, SerializedProperty sP_isOutfoldedProperty, SerializedProperty sP_boolProperty, SerializedProperty sP_floatProperty, SerializedProperty sP_floatProperty_relToScreen, SerializedProperty sP_blockConstraintInterpretationProperty, SerializedProperty sP_sizeInterpretation)
        {
            sP_isOutfoldedProperty.boolValue = EditorGUILayout.Foldout(sP_isOutfoldedProperty.boolValue, used_GUIContentOfHeadline, true);
            if (sP_isOutfoldedProperty.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                EditorGUILayout.HelpBox(helpBoxText, MessageType.None, true);
                EditorGUILayout.PropertyField(sP_boolProperty, used_GUIContentOfEnableToggle);
                EditorGUI.BeginDisabledGroup(!sP_boolProperty.boolValue);

                if (CheckIf_blockWidthConstraintValue_isDependentOn_screenWindowSize(sP_blockConstraintInterpretationProperty, sP_sizeInterpretation))
                {
                    EditorGUILayout.PropertyField(sP_floatProperty_relToScreen, new GUIContent(used_nameString_ofValue));
                }
                else
                {
                    EditorGUILayout.PropertyField(sP_floatProperty, new GUIContent(used_nameString_ofValue));
                }

                EditorGUILayout.PropertyField(sP_blockConstraintInterpretationProperty, new GUIContent("Value interpretation", "The value can be specified relative to a context of interest." + Environment.NewLine + "The '" + used_nameString_ofValue + "' value from the previous line will be scaled according to this interpretation setting here."));

                if (sP_blockConstraintInterpretationProperty.enumValueIndex == (int)TextDrawer.SizeInterpretationInclFallback.relativeToTheSameAsTextSize)
                {
                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.PropertyField(sP_sizeInterpretation, new GUIContent("Text Size is relative to"));
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                }

                EditorGUI.EndDisabledGroup();

                GUILayout.Space(EditorGUIUtility.singleLineHeight);

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        bool CheckIf_blockWidthConstraintValue_isDependentOn_screenWindowSize(SerializedProperty sP_blockConstraintInterpretationProperty, SerializedProperty sP_sizeInterpretation)
        {
            switch (sP_blockConstraintInterpretationProperty.enumValueIndex)
            {
                case (int)TextDrawer.SizeInterpretationInclFallback.relativeToTheSameAsTextSize:
                    switch (sP_sizeInterpretation.enumValueIndex)
                    {
                        case (int)TextDrawer.SizeInterpretation.globalSpace:
                            return false;
                        case (int)TextDrawer.SizeInterpretation.sizeOfGameobject:
                            return false;
                        case (int)TextDrawer.SizeInterpretation.sceneViewWindowWidth:
                            return true;
                        case (int)TextDrawer.SizeInterpretation.gameViewWindowWidth:
                            return true;
                        default:
                            return false;
                    }
                case (int)TextDrawer.SizeInterpretationInclFallback.absoluteUnits:
                    return false;
                case (int)TextDrawer.SizeInterpretationInclFallback.relativeToGameobjectSize:
                    return false;
                case (int)TextDrawer.SizeInterpretationInclFallback.relativeToTheSceneViewWindowWidth:
                    return true;
                case (int)TextDrawer.SizeInterpretationInclFallback.relativeToTheGameViewWindowWidth:
                    return true;
                default:
                    return false;
            }
        }

        void DrawAutoFlipCheckbox(SerializedProperty sP_forceTextTo_facingToSceneViewCam, SerializedProperty sP_forceTextTo_facingToGameViewCam)
        {
            if ((sP_forceTextTo_facingToGameViewCam.boolValue == true) || (sP_forceTextTo_facingToSceneViewCam.boolValue == true))
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.Toggle(new GUIContent("Auto flip to prevent mirror inverted", "Only available if 'Force facing to a camera' is disabled."), true);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("autoFlipToPreventMirrorInverted"), new GUIContent("Auto flip to prevent mirror inverted", "This flips the text horizontally so it is always non-mirrored readable in the camera specified by the 'Force facing to a camera' settings above or else by 'DrawBasics.cameraForAutomaticOrientation'."));
            }
        }

    }
#endif
}
