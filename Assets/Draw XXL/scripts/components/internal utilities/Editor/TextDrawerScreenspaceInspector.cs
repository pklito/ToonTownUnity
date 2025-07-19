namespace DrawXXL
{
    using UnityEngine;
    using System;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(TextDrawerScreenspace))]
    [CanEditMultipleObjects]
    public class TextDrawerScreenspaceInspector : VisualizerScreenspaceParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("text");
            if (DrawCameraChooser(true))
            {
                GUILayout.Space(EditorGUIUtility.singleLineHeight);

                DrawTextInputInclMarkupHelper(true, true, "Drawn text:");
                DrawSpecificationOf_customVector2_1("Text direction", false, null, true, true, true, false);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("color"), new GUIContent("Color", "This can be overwritten by 'Text/Style/Colored', but then still defines the color of a frame box."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("size_relToViewportHeight"), new GUIContent("Size", "This is the width per letter, relative to the viewport height."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("textAnchor"), new GUIContent("Text anchor"));
                DrawFrameBoxSpecs();
                Draw_ToggleableFloatSliders();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("autoLineBreakAtScreenBorder"), new GUIContent("Automatic line break at viewport border"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("autoFlipTextToPreventUpsideDown"), new GUIContent("Automatic horizontal flip to prevent upside down"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("positionInsideViewport0to1"), new GUIContent("Position (inside viewport)"));
                DrawCheckboxFor_drawOnlyIfSelected("screenspace text");
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
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
                EditorGUILayout.PropertyField(serializedObject.FindProperty("enclosingBox_lineWidth_relToTextSize"), new GUIContent("Width"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("enclosingBox_paddingSize_relToTextSize"), new GUIContent("Padding"));
                EditorGUI.EndDisabledGroup();

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        public void Draw_ToggleableFloatSliders()
        {
            SerializedProperty sP_autoLineBreakWidth_relToViewportWidth_isOutfolded = serializedObject.FindProperty("autoLineBreakWidth_relToViewportWidth_isOutfolded");
            SerializedProperty sP_autoLineBreakWidth_relToViewportWidth = serializedObject.FindProperty("autoLineBreakWidth_relToViewportWidth");
            SerializedProperty sP_autoLineBreakWidth_relToViewportWidth_value = serializedObject.FindProperty("autoLineBreakWidth_relToViewportWidth_value");

            SerializedProperty sP_forceTextEnlargementToThisMinWidth_relToViewportWidth_isOutfolded = serializedObject.FindProperty("forceTextEnlargementToThisMinWidth_relToViewportWidth_isOutfolded");
            SerializedProperty sP_forceTextEnlargementToThisMinWidth_relToViewportWidth = serializedObject.FindProperty("forceTextEnlargementToThisMinWidth_relToViewportWidth");
            SerializedProperty sP_forceTextEnlargementToThisMinWidth_relToViewportWidth_value = serializedObject.FindProperty("forceTextEnlargementToThisMinWidth_relToViewportWidth_value");

            SerializedProperty sP_forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth_isOutfolded = serializedObject.FindProperty("forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth_isOutfolded");
            SerializedProperty sP_forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth = serializedObject.FindProperty("forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth");
            SerializedProperty sP_forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth_value = serializedObject.FindProperty("forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth_value");

            GUIContent used_GUIContent;
            GUIContent used_GUIContentOfEnableToggle;

            string helpBoxText_for_autoLineBreakWidth = "This automatically inserts line breaks into the text, so that the width of the whole text block doesn't exceed the specified width.";
            used_GUIContent = new GUIContent("Automatic line breaks");
            used_GUIContentOfEnableToggle = new GUIContent("Force line breaks after width span");
            Draw_ToggleableFloatSlider(used_GUIContent, used_GUIContentOfEnableToggle, "Maximum line width (relative to screen width)", helpBoxText_for_autoLineBreakWidth, sP_autoLineBreakWidth_relToViewportWidth_isOutfolded, sP_autoLineBreakWidth_relToViewportWidth, sP_autoLineBreakWidth_relToViewportWidth_value);

            string helpBoxText_for_forceTextEnlargementToThisMinWidth = "This overwrites the text size and rescales the text, so that the whole text block has at least the specified width.";
            used_GUIContent = new GUIContent("Minimum text block width");
            used_GUIContentOfEnableToggle = new GUIContent("Force text enlargement to reach a minimum text block width");
            EditorGUI.BeginChangeCheck();
            Draw_ToggleableFloatSlider(used_GUIContent, used_GUIContentOfEnableToggle, "Minimum text block width (relative to screen width)", helpBoxText_for_forceTextEnlargementToThisMinWidth, sP_forceTextEnlargementToThisMinWidth_relToViewportWidth_isOutfolded, sP_forceTextEnlargementToThisMinWidth_relToViewportWidth, sP_forceTextEnlargementToThisMinWidth_relToViewportWidth_value);
            bool minWidth_changed = EditorGUI.EndChangeCheck();

            string helpBoxText_for_forceRestrictTextSizeToThisMaxTextWidth = "This overwrites the text size and rescales the text, so that the whole text block is not wider then the specified width." + Environment.NewLine + Environment.NewLine + "If you want restrict the text block width but keep the text size you can use 'Automatic line breaks'.";
            used_GUIContent = new GUIContent("Maximum text block width");
            used_GUIContentOfEnableToggle = new GUIContent("Force restrict the text block width to a maximum");
            EditorGUI.BeginChangeCheck();
            Draw_ToggleableFloatSlider(used_GUIContent, used_GUIContentOfEnableToggle, "Maximum text block width (relative to screen width)", helpBoxText_for_forceRestrictTextSizeToThisMaxTextWidth, sP_forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth_isOutfolded, sP_forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth, sP_forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth_value);
            bool maxWidth_changed = EditorGUI.EndChangeCheck();

            if (minWidth_changed)
            {
                sP_forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth_value.floatValue = Mathf.Max(sP_forceTextEnlargementToThisMinWidth_relToViewportWidth_value.floatValue, sP_forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth_value.floatValue);
            }
            else
            {
                if (maxWidth_changed)
                {
                    sP_forceTextEnlargementToThisMinWidth_relToViewportWidth_value.floatValue = Mathf.Min(sP_forceTextEnlargementToThisMinWidth_relToViewportWidth_value.floatValue, sP_forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth_value.floatValue);
                }
            }
        }

        void Draw_ToggleableFloatSlider(GUIContent used_GUIContentOfHeadline, GUIContent used_GUIContentOfEnableToggle, string used_nameString_ofValue, string helpBoxText, SerializedProperty sP_isOutfoldedProperty, SerializedProperty sP_boolProperty, SerializedProperty sP_floatProperty)
        {
            sP_isOutfoldedProperty.boolValue = EditorGUILayout.Foldout(sP_isOutfoldedProperty.boolValue, used_GUIContentOfHeadline, true);
            if (sP_isOutfoldedProperty.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                EditorGUILayout.HelpBox(helpBoxText, MessageType.None, true);
                EditorGUILayout.PropertyField(sP_boolProperty, used_GUIContentOfEnableToggle);

                EditorGUI.BeginDisabledGroup(!sP_boolProperty.boolValue);
                EditorGUILayout.PropertyField(sP_floatProperty, new GUIContent(used_nameString_ofValue));
                EditorGUI.EndDisabledGroup();

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

    }
#endif
}
