namespace DrawXXL
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomPropertyDrawer(typeof(InternalDXXL_NeighboringDatapointForChartInspector))]
    public class NeighboringDatapointForInspectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUIStyle labelStyle = new GUIStyle(EditorStyles.label);
            labelStyle.richText = true;
            EditorGUI.LabelField(position, label, labelStyle);

            float previousLabelWidth = EditorGUIUtility.labelWidth;

            float xStartPos = position.x + 0.25f * position.width;
            float width_allThree = position.width - xStartPos + EditorGUIUtility.singleLineHeight;
            float xWidth = width_allThree * 0.28f;
            EditorGUIUtility.labelWidth = 40.0f;
            Rect space_ofX = new Rect(xStartPos, position.y, xWidth + 1.3f * EditorGUIUtility.singleLineHeight, position.height);
            EditorGUI.PropertyField(space_ofX, property.FindPropertyRelative("x"), new GUIContent("X"));

            float yStartPos = xStartPos + xWidth;
            float yWidth = width_allThree * 0.28f;
            EditorGUIUtility.labelWidth = 40.0f;
            Rect space_ofY = new Rect(yStartPos, position.y, yWidth + 1.3f * EditorGUIUtility.singleLineHeight, position.height);
            EditorGUI.PropertyField(space_ofY, property.FindPropertyRelative("y"), new GUIContent("Y"));

            float dStartPos = yStartPos + yWidth;
            float dWidth = width_allThree * 0.44f;
            EditorGUIUtility.labelWidth = 72.0f;
            Rect space_ofDelta = new Rect(dStartPos, position.y, dWidth, position.height);
            EditorGUI.PropertyField(space_ofDelta, property.FindPropertyRelative("deltaSincePrecedingY"), new GUIContent("delta Y"));

            EditorGUIUtility.labelWidth = previousLabelWidth;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
#endif
}
