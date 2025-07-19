namespace DrawXXL
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(CoordinateAxesGizmoVisualizer))]
    [CanEditMultipleObjects]
    public class CoordinateAxesGizmoVisualizerInspector : VisualizerParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("gizmo");

            SerializedProperty sP_visualizedSpace = serializedObject.FindProperty("visualizedSpace");
            EditorGUILayout.PropertyField(sP_visualizedSpace, new GUIContent("Visualized space"));
            bool visualizedSpace_isTheGlobalSpace = sP_visualizedSpace.enumValueIndex == (int)CoordinateAxesGizmoVisualizer.VisualizedSpace.global;
            bool isFallback_fromParentSpace_toGlobalSpace = false;
            if (sP_visualizedSpace.enumValueIndex == (int)CoordinateAxesGizmoVisualizer.VisualizedSpace.localDefinedByParent)
            {
                if (transform_onVisualizerObject.parent == null)
                {
                    EditorGUILayout.HelpBox("No parent available. Fallback to visualizing global space.", MessageType.None, true);
                    isFallback_fromParentSpace_toGlobalSpace = true;
                }
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("drawXYZchars"), new GUIContent("Draw 'X', 'Y' and 'Z' chars"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("skipConeDrawing"), new GUIContent("Hide cones"));

            SerializedProperty sP_forceAllAxesLength = serializedObject.FindProperty("forceAllAxesLength");
            string label_for_forceAllAxesLength = "Force fixed axis length";
            if (visualizedSpace_isTheGlobalSpace || isFallback_fromParentSpace_toGlobalSpace)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.Toggle(new GUIContent(label_for_forceAllAxesLength, "This is only available in local space."), true);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUILayout.PropertyField(sP_forceAllAxesLength, new GUIContent(label_for_forceAllAxesLength));
            }

            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            bool lengthChooser_isGreyedOut = (visualizedSpace_isTheGlobalSpace == false) && (isFallback_fromParentSpace_toGlobalSpace == false) && (sP_forceAllAxesLength.boolValue == false);
            EditorGUI.BeginDisabledGroup(lengthChooser_isGreyedOut);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("forceAllAxesLength_lengthValue"), new GUIContent("Forced length (global units)"));
            EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

            EditorGUILayout.PropertyField(serializedObject.FindProperty("lineWidth"), new GUIContent("Line width"));

            Draw_DrawPosition3DOffset();
            DrawTextInputInclMarkupHelper();
            DrawCheckboxFor_drawOnlyIfSelected("gizmo");
            DrawCheckboxFor_hiddenByNearerObjects("gizmo");

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }
    }
#endif
}
