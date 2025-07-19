namespace DrawXXL
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(CrossProductVisualizer))]
    [CanEditMultipleObjects]
    public class CrossProductVisualizerInspector : VisualizerParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("cross product");

            SerializedProperty sP_colorOfVector1_forCrossProduct = serializedObject.FindProperty("colorOfVector1_forCrossProduct");
            SerializedProperty sP_colorOfVector2_forCrossProduct = serializedObject.FindProperty("colorOfVector2_forCrossProduct");
            SerializedProperty sP_colorOfResultVector_forCrossProduct = serializedObject.FindProperty("colorOfResultVector_forCrossProduct");

            DrawSpecificationOf_customVector3_1("<b>Input Vector 1   <color=#" + ColorUtility.ToHtmlStringRGBA(sP_colorOfVector1_forCrossProduct.colorValue) + ">lhs (thumb of left hand)</color></b>", false, null, false, false, true, false);
            DrawSpecificationOf_customVector3_2("<b>Input Vector 2   <color=#" + ColorUtility.ToHtmlStringRGBA(sP_colorOfVector2_forCrossProduct.colorValue) + ">rhs (index finger of left hand)</color></b>", false, null, false, false, true, false);

            GUIStyle style_ofResultHeadline = new GUIStyle();
            style_ofResultHeadline.richText = true;
            EditorGUILayout.LabelField("<b>Cross Product Result   <color=#" + ColorUtility.ToHtmlStringRGBA(sP_colorOfResultVector_forCrossProduct.colorValue) + ">(middle finger of left hand)</color></b>", style_ofResultHeadline);

            EditorGUI.indentLevel++;

            Vector3 vector1_lhs_leftThumb = visualizerParentMonoBehaviour_unserialized.Get_customVector3_1_inGlobalSpaceUnits();
            Vector3 vector2_rhs_leftIndexFinger = visualizerParentMonoBehaviour_unserialized.Get_customVector3_2_inGlobalSpaceUnits();
            Vector3 crossProductResult = Vector3.Cross(vector1_lhs_leftThumb, vector2_rhs_leftIndexFinger);
            EditorGUILayout.Vector3Field(GUIContent.none, crossProductResult);

            EditorGUI.indentLevel--;

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            DrawColorSection(sP_colorOfVector1_forCrossProduct, sP_colorOfVector2_forCrossProduct, sP_colorOfResultVector_forCrossProduct);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("linesWidth"), new GUIContent("Lines width"));

            Draw_DrawPosition3DOffset();
            DrawCheckboxFor_drawOnlyIfSelected("cross product");
            DrawCheckboxFor_hiddenByNearerObjects("cross product");

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }

        void DrawColorSection(SerializedProperty sP_colorOfVector1_forCrossProduct, SerializedProperty sP_colorOfVector2_forCrossProduct, SerializedProperty sP_colorOfResultVector_forCrossProduct)
        {
            SerializedProperty sP_colorSection_isOutfolded = serializedObject.FindProperty("colorSection_isOutfolded");
            sP_colorSection_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_colorSection_isOutfolded.boolValue, "Colors", true);
            if (sP_colorSection_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel++;

                SerializedProperty sP_colorOfAngle_forCrossProduct = serializedObject.FindProperty("colorOfAngle_forCrossProduct");
                SerializedProperty sP_colorOfResultText_forCrossProduct = serializedObject.FindProperty("colorOfResultText_forCrossProduct");
                SerializedProperty sP_colorOfResultPlane_forCrossProduct = serializedObject.FindProperty("colorOfResultPlane_forCrossProduct");

                EditorGUILayout.PropertyField(sP_colorOfVector1_forCrossProduct, new GUIContent("Input Vector 1 (lhs)"));
                EditorGUILayout.PropertyField(sP_colorOfVector2_forCrossProduct, new GUIContent("Input Vector 2 (rhs)"));
                EditorGUILayout.PropertyField(sP_colorOfResultVector_forCrossProduct, new GUIContent("Result Vector"));
                EditorGUILayout.PropertyField(sP_colorOfResultText_forCrossProduct, new GUIContent("Result Text"));
                EditorGUILayout.PropertyField(sP_colorOfResultPlane_forCrossProduct, new GUIContent("Result Plane"));
                EditorGUILayout.PropertyField(sP_colorOfAngle_forCrossProduct, new GUIContent("Angle"));

                EditorGUI.indentLevel--;

                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }
        }

    }
#endif
}
