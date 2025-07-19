namespace DrawXXL
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomPropertyDrawer(typeof(InternalDXXL_TaggedScreenspaceObject))]
    public class InternalDXXL_TaggedScreenspaceObjectDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty sP_gameobject = property.FindPropertyRelative("gameobject");
            Rect space_ofGameobject = new Rect(position.x, position.y, 0.4f * position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(space_ofGameobject, sP_gameobject, GUIContent.none);

            Rect space_ofTextLabel = new Rect(position.x + 0.4f * position.width, position.y, 0.2f * position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.LabelField(space_ofTextLabel, new GUIContent("Text:", "You can insert insert line breaks by typing <br> inside the text."));

            SerializedProperty sP_text = property.FindPropertyRelative("text");
            Rect space_ofText = new Rect(position.x + 0.50f * position.width, position.y, 0.3f * position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(space_ofText, sP_text, GUIContent.none);

            SerializedProperty sP_color = property.FindPropertyRelative("color");
            Rect space_ofColor = new Rect(position.x + 0.8f * position.width, position.y, 0.2f * position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(space_ofColor, sP_color, GUIContent.none);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
    }
#endif
}
