namespace DrawXXL
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(BoundsVisualizer))]
    [CanEditMultipleObjects]
    public class BoundsVisualizerInspector : VisualizerParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("bounds");

            EditorGUILayout.PropertyField(serializedObject.FindProperty("global"), new GUIContent("Global"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("local"), new GUIContent("Local"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("includeChildren"), new GUIContent("Include children"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("lineWidth"), new GUIContent("Line width"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("color"), new GUIContent("Color"));

            DrawTextSpecs();
            DrawCheckboxFor_drawOnlyIfSelected("bounds");
            DrawCheckboxFor_hiddenByNearerObjects("bounds");

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }


        void DrawTextSpecs()
        {
            //-> this fakes the appearance of beeing inside the parents text specification foldout
            bool emptyLineAtEndIfOutfolded = false;
            DrawTextInputInclMarkupHelper(true, false, null, emptyLineAtEndIfOutfolded);

            if (serializedObject.FindProperty("textSection_isOutfolded").boolValue == true)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                DrawTextSizeContentChooser();
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        void DrawTextSizeContentChooser()
        {
            EditorGUILayout.LabelField("Text Size");

            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            SerializedProperty sP_attachedTextsizeReferenceContext = serializedObject.FindProperty("attachedTextsizeReferenceContext");
            EditorGUILayout.PropertyField(sP_attachedTextsizeReferenceContext, new GUIContent("Relative to"));

            switch (sP_attachedTextsizeReferenceContext.enumValueIndex)
            {
                case (int)BoundsVisualizer.AttachedTextsizeReferenceContext.extentOfBounds:
                    break;
                case (int)BoundsVisualizer.AttachedTextsizeReferenceContext.globalSpace:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("textSize_value"), new GUIContent("Size per letter", "Text size in world units"));
                    break;
                case (int)BoundsVisualizer.AttachedTextsizeReferenceContext.sceneViewWindowSize:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("textSize_value_relToScreen"), new GUIContent("Size per letter", "Text size relative to Scene View window size."));
                    break;
                case (int)BoundsVisualizer.AttachedTextsizeReferenceContext.gameViewWindowSize:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("textSize_value_relToScreen"), new GUIContent("Size per letter", "Text size relative to Game View window size."));
                    break;
                default:
                    break;
            }

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

    }
#endif
}
