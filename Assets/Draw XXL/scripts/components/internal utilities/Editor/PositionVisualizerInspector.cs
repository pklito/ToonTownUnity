namespace DrawXXL
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(PositionVisualizer))]
    [CanEditMultipleObjects]
    public class PositionVisualizerInspector : VisualizerParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("position");

            SerializedProperty sP_global = serializedObject.FindProperty("global");
            SerializedProperty sP_local = serializedObject.FindProperty("local");
            SerializedProperty sP_allParents = serializedObject.FindProperty("allParents");

            bool hasNoParents = (transform_onVisualizerObject.parent == null);
            if (hasNoParents)
            {
                sP_global.boolValue = true;
                sP_local.boolValue = false;
                sP_allParents.boolValue = false;
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(sP_global, new GUIContent("Global"));
                EditorGUILayout.PropertyField(sP_local, new GUIContent("Local"));
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUILayout.PropertyField(sP_global, new GUIContent("Global"));
                EditorGUILayout.PropertyField(sP_local, new GUIContent("Local"));
            }

            if (sP_local.boolValue == false) { sP_allParents.boolValue = false; }

            EditorGUI.BeginDisabledGroup(!sP_local.boolValue);
            EditorGUILayout.PropertyField(sP_allParents, new GUIContent("For all parents"));
            EditorGUI.EndDisabledGroup();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("lineWidth"), new GUIContent("Line width"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("color"), new GUIContent("Color"));

            DrawTextInputInclMarkupHelper();
            DrawCheckboxFor_drawOnlyIfSelected("position");
            DrawCheckboxFor_hiddenByNearerObjects("position");

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }
    }
#endif
}
