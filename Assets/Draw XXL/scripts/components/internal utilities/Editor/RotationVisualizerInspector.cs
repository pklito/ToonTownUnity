namespace DrawXXL
{
    using UnityEngine;
    using System;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(RotationVisualizer))]
    [CanEditMultipleObjects]
    public class RotationVisualizerInspector : VisualizerParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("rotation");

            SerializedProperty sP_rotationType = serializedObject.FindProperty("rotationType");
            EditorGUILayout.PropertyField(sP_rotationType, new GUIContent("Rotation type"));
            bool displayIsQuaternion = ((sP_rotationType.enumValueIndex == (int)RotationVisualizer.RotationType.quaternionGlobal) || (sP_rotationType.enumValueIndex == (int)RotationVisualizer.RotationType.quaternionLocal));
            bool displayIsEuler = !displayIsQuaternion;
            bool isLocal = ((sP_rotationType.enumValueIndex == (int)RotationVisualizer.RotationType.quaternionLocal) || (sP_rotationType.enumValueIndex == (int)RotationVisualizer.RotationType.eulerAnglesLocal));

            if (displayIsQuaternion)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("color_ofTurnAxis"), new GUIContent("Turn axis color"));
            }

            if (displayIsEuler)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay"), new GUIContent("Angles from code instead from inspector", "Angles from code instead from inspector:" + Environment.NewLine + Environment.NewLine + "The same overall rotation can be created by different sets of x/y/z-angles. For example (100/0/0) ends up as the same overall rotation than (80/180/180). Unity uses a different set of x/y/z-angles for display in the inspectors transform component than what transform.eulerAngles returns in code, thought in the end it's both the same overall rotation."));
            }

            SerializedProperty sP_length_ofUpAndForwardVectors;
            if (displayIsQuaternion)
            {
                sP_length_ofUpAndForwardVectors = serializedObject.FindProperty("length_ofUpAndForwardVectors_caseQuaternion");
            }
            else
            {
                sP_length_ofUpAndForwardVectors = serializedObject.FindProperty("length_ofUpAndForwardVectors_caseEuler");
            }

            GUIContent guiContent_ofUpForwardLength;
            if (isLocal)
            {
                guiContent_ofUpForwardLength = new GUIContent("Rotated Up/Forward (length)", "This is in local units." + Environment.NewLine + Environment.NewLine + "You can disable the display of up and forward vectors by setting this value to 0.");
            }
            else
            {
                guiContent_ofUpForwardLength = new GUIContent("Rotated Up/Forward (length)", "You can disable the display of up and forward vectors by setting this value to 0.");
            }

            EditorGUILayout.PropertyField(sP_length_ofUpAndForwardVectors, guiContent_ofUpForwardLength);
            if (sP_length_ofUpAndForwardVectors.floatValue < 0.001f)
            {
                sP_length_ofUpAndForwardVectors.floatValue = 0.0f;
            }

            if (displayIsEuler)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUI.BeginDisabledGroup(UtilitiesDXXL_Math.ApproximatelyZero(sP_length_ofUpAndForwardVectors.floatValue));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("alpha_ofSquareSpannedByForwardAndUp"), new GUIContent("Alpha (up/forward square)", "Is only available if length (up/forward) is bigger than 0."));
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }

            SerializedProperty sP_drawCustomRotatedVector;
            if (displayIsQuaternion)
            {
                sP_drawCustomRotatedVector = serializedObject.FindProperty("drawCustomRotatedVector_caseQuaternion");
            }
            else
            {
                sP_drawCustomRotatedVector = serializedObject.FindProperty("drawCustomRotatedVector_caseEuler");
            }

            DrawSpecificationOf_customVector3_1("Rotate custom vector", true, sP_drawCustomRotatedVector, false, false, true, false);

            if (displayIsQuaternion)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("lineWidth"), new GUIContent("Lines width"));
            }

            if (displayIsEuler)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("alpha_ofUnrotatedGimbalAxes"), new GUIContent("Alpha (unrotated axes)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("gimbalSize"), new GUIContent("Gimbal size", "This doesn't affect the display of the rotated vectors but only scales the size of the three main axes."));
            }

            Draw_DrawPosition3DOffset();
            DrawTextInputInclMarkupHelper();
            DrawCheckboxFor_drawOnlyIfSelected("rotation");
            DrawCheckboxFor_hiddenByNearerObjects("rotation");

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }
    }
#endif
}
