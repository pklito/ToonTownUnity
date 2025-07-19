namespace DrawXXL
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(DotProductVisualizer))]
    [CanEditMultipleObjects]
    public class DotProductVisualizerInspector : VisualizerParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("dot product");

            SerializedProperty sP_colorOfVector1_forDotProduct = serializedObject.FindProperty("colorOfVector1_forDotProduct");
            SerializedProperty sP_colorOfVector2_forDotProduct = serializedObject.FindProperty("colorOfVector2_forDotProduct");
          
            DrawSpecificationOf_customVector3_1("<b>Input Vector 1   <color=#" + ColorUtility.ToHtmlStringRGBA(sP_colorOfVector1_forDotProduct.colorValue) + ">lhs</color></b>", false, null, false, false, true, false);
            DrawSpecificationOf_customVector3_2("<b>Input Vector 2   <color=#" + ColorUtility.ToHtmlStringRGBA(sP_colorOfVector2_forDotProduct.colorValue) + ">rhs</color></b>", false, null, false, false, true, false);

            GUIStyle style_ofResultHeadline = new GUIStyle();
            style_ofResultHeadline.fontStyle = FontStyle.Bold;
            EditorGUILayout.LabelField("Dot Product Result", style_ofResultHeadline);

            EditorGUI.indentLevel++;
            
            Vector3 vector1_lhs = visualizerParentMonoBehaviour_unserialized.Get_customVector3_1_inGlobalSpaceUnits();
            Vector3 vector2_rhs = visualizerParentMonoBehaviour_unserialized.Get_customVector3_2_inGlobalSpaceUnits();
            float dotProductResult = Vector3.Dot(vector1_lhs, vector2_rhs);
            EditorGUILayout.FloatField(dotProductResult);
         
            EditorGUI.indentLevel--;

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            DrawColorSection(sP_colorOfVector1_forDotProduct, sP_colorOfVector2_forDotProduct);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("linesWidth"), new GUIContent("Lines width"));

            Draw_DrawPosition3DOffset();
            DrawCheckboxFor_drawOnlyIfSelected("dot product");
            DrawCheckboxFor_hiddenByNearerObjects("dot product");

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }

        void DrawColorSection(SerializedProperty sP_colorOfVector1_forDotProduct, SerializedProperty sP_colorOfVector2_forDotProduct)
        {
            SerializedProperty sP_colorSection_isOutfolded = serializedObject.FindProperty("colorSection_isOutfolded");
            sP_colorSection_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_colorSection_isOutfolded.boolValue, "Colors", true);
            if (sP_colorSection_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel++;

                SerializedProperty sP_colorOfAngle_forDotProduct = serializedObject.FindProperty("colorOfAngle_forDotProduct");
                SerializedProperty sP_colorOfResult_forDotProduct = serializedObject.FindProperty("colorOfResult_forDotProduct");

                EditorGUILayout.PropertyField(sP_colorOfVector1_forDotProduct, new GUIContent("Input Vector 1 (lhs)"));
                EditorGUILayout.PropertyField(sP_colorOfVector2_forDotProduct, new GUIContent("Input Vector 2 (rhs)"));
                EditorGUILayout.PropertyField(sP_colorOfResult_forDotProduct, new GUIContent("Result Text"));
                EditorGUILayout.PropertyField(sP_colorOfAngle_forDotProduct, new GUIContent("Angle"));

                EditorGUI.indentLevel--;
                
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }
        }
    }
#endif
}
