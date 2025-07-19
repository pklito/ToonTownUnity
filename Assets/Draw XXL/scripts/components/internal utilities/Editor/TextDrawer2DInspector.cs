namespace DrawXXL
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(TextDrawer2D))]
    [CanEditMultipleObjects]
    public class TextDrawer2DInspector : TextDrawerInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("text");
            DrawTextInputInclMarkupHelper(true, true, "Drawn text:");
            DrawSpecificationOf_customVector2_1("Text direction", false, null, true, true, true, false);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("color"), new GUIContent("Color", "This can be overwritten by 'Text/Style/Colored', but then still defines the color of a frame box."));
            SerializedProperty sP_sizeInterpretation = serializedObject.FindProperty("sizeInterpretation");
            SerializedProperty sP_forceTextEnlargementToThisMinWidth = serializedObject.FindProperty("forceTextEnlargementToThisMinWidth");
            SerializedProperty sP_forceRestrictTextSizeToThisMaxTextWidth = serializedObject.FindProperty("forceRestrictTextSizeToThisMaxTextWidth");
            DrawSizeSpecs(sP_sizeInterpretation, sP_forceTextEnlargementToThisMinWidth, sP_forceRestrictTextSizeToThisMaxTextWidth, "The here used gameobject size is defined by the biggest absolute global scale dimension (excluding z) of the transform.");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("textAnchor"), new GUIContent("Text anchor"));
            DrawFrameBoxSpecs();
            Draw_TextBlockConstraints(sP_sizeInterpretation, sP_forceTextEnlargementToThisMinWidth, sP_forceRestrictTextSizeToThisMaxTextWidth);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("autoFlipToPreventMirrorInverted"), new GUIContent("Auto flip to prevent mirror inverted", "This flips the text horizontally so it is always non-mirrored readable in the camera specified by the settings above or else by 'DrawBasics.cameraForAutomaticOrientation'."));
            Draw_DrawPosition2DOffset();
            DrawZPosChooserFor2D();
            DrawCheckboxFor_drawOnlyIfSelected("text");
            DrawCheckboxFor_hiddenByNearerObjects("text");

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }
    }
#endif
}
