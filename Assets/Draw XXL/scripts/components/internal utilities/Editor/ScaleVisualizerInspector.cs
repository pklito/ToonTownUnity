namespace DrawXXL
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(ScaleVisualizer))]
    [CanEditMultipleObjects]
    public class ScaleVisualizerInspector : VisualizerParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("scale");

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("scaleType"), new GUIContent("Scale Type", "This is automatically set depending on if this gameObject has a parent or not."));
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("drawXDim"), new GUIContent("X"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("drawYDim"), new GUIContent("Y"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("drawZDim"), new GUIContent("Z"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("relSizeOfPlanes"), new GUIContent("Planes size"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("lineWidth"), new GUIContent("Line width"));

            GUILayout.BeginHorizontal();
            SerializedProperty sP_force_overwriteColor = serializedObject.FindProperty("force_overwriteColor");
            EditorGUILayout.PropertyField(sP_force_overwriteColor, new GUIContent("Force color"));
            EditorGUI.BeginDisabledGroup(!sP_force_overwriteColor.boolValue);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("overwriteColor"), GUIContent.none);
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            Draw_DrawPosition3DOffset();
            DrawTextInputInclMarkupHelper();
            DrawCheckboxFor_drawOnlyIfSelected("scale");
            DrawCheckboxFor_hiddenByNearerObjects("scale");

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }
    }
#endif
}
