namespace DrawXXL
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(CameraGridVisualizer))]
    [CanEditMultipleObjects]
    public class CameraGridVisualizerInspector : VisualizerScreenspaceParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("camera grid");
            if (DrawCameraChooser(false))
            {
                GUILayout.Space(0.75f * EditorGUIUtility.singleLineHeight);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("color"), new GUIContent("Color"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("linesWidth_relToViewportHeight"), new GUIContent("Lines width", "This is relative to the viewport height."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("drawTenthLines"), new GUIContent("Lines at Tenth"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("drawHundredthLines"), new GUIContent("Lines at Hundredth"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gridScreenspaceMode"), new GUIContent("Mode"));
                DrawCheckboxFor_drawOnlyIfSelected("camera grid");
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }

    }
#endif
}
