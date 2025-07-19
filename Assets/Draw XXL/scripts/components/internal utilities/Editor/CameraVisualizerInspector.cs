namespace DrawXXL
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(CameraVisualizer))]
    [CanEditMultipleObjects]
    public class CameraVisualizerInspector : VisualizerScreenspaceParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("camera");
            if (DrawCameraChooser(false))
            {
                GUILayout.Space(0.75f * EditorGUIUtility.singleLineHeight);

                DrawCameraSpecs();
                DrawFrustumSpecs();

                DrawTextSpecs();
                DrawCheckboxFor_drawOnlyIfSelected("camera");
                DrawCheckboxFor_hiddenByNearerObjects("camera");
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }

        void DrawCameraSpecs()
        {
            SerializedProperty sP_drawCamera = serializedObject.FindProperty("drawCamera");
            EditorGUILayout.PropertyField(sP_drawCamera, new GUIContent("Draw camera"));
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            EditorGUI.BeginDisabledGroup(!sP_drawCamera.boolValue);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("linesWidth_camera"), new GUIContent("Lines width"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("color_ofCamera_enabledCam"), new GUIContent("Color (enabled)"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("color_ofCamera_disabledCam"), new GUIContent("Color (disabled)"));
            EditorGUI.EndDisabledGroup();

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

            GUILayout.Space(0.75f * EditorGUIUtility.singleLineHeight);
        }

        void DrawFrustumSpecs()
        {
            SerializedProperty sP_drawFrustum = serializedObject.FindProperty("drawFrustum");
            EditorGUILayout.PropertyField(sP_drawFrustum, new GUIContent("Draw frustum"));
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            EditorGUI.BeginDisabledGroup(!sP_drawFrustum.boolValue);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("linesWidth_frustum"), new GUIContent("Lines width (edges)"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("color_ofFrustum_enabledCam"), new GUIContent("Color (enabled)"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("color_ofFrustum_disabledCam"), new GUIContent("Color (disabled)"));

            GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

            EditorGUILayout.LabelField("Boundary planes:");
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            EditorGUILayout.PropertyField(serializedObject.FindProperty("alphaFactor_forBoundarySurfaceLines"), new GUIContent("Alpha factor"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("linesPerBoundarySurface"), new GUIContent("Thin Lines (per plane)"));
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

            GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

            DrawSectionFor_additionalFlexibleHighlighterPlane();

            EditorGUI.EndDisabledGroup();

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

            GUILayout.Space(0.75f * EditorGUIUtility.singleLineHeight);
        }


        void DrawSectionFor_additionalFlexibleHighlighterPlane()
        {
            EditorGUILayout.LabelField("Additional flexible highlighted Plane:");
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            SerializedProperty sP_highlightedPlaneDefintionType = serializedObject.FindProperty("highlightedPlaneDefintionType");
            EditorGUILayout.PropertyField(sP_highlightedPlaneDefintionType, GUIContent.none);

            switch (sP_highlightedPlaneDefintionType.enumValueIndex)
            {
                case (int)CameraVisualizer.HighlightedPlaneDefintionType.disabled:
                    break;
                case (int)CameraVisualizer.HighlightedPlaneDefintionType.definedByDistanceFromCamera:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("distanceOfHighlightedPlane"), new GUIContent("Distance", "The additional flexible highlighted plane is only drawn if its 'Distance' is at least the cameras near clip plane distance."));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("drawFrustumsHighlightedPlaneAlsoIfFarerThanFarClipPlane"), new GUIContent("Draw if farer than far clip plane", "This specifies whether the flexible plane should also be drawn if it is farer than the cameras far clip plane."));
            DrawChooserLineFor_overwriteColorForFrustumsHighlightedPlane();
                    break;
                case (int)CameraVisualizer.HighlightedPlaneDefintionType.definedByAPosition:
                    SerializedProperty sP_highlightedPlaneViaPosDefintionType = serializedObject.FindProperty("highlightedPlaneViaPosDefintionType");
                    EditorGUILayout.PropertyField(sP_highlightedPlaneViaPosDefintionType, new GUIContent("Plane Anchor Source"));

                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                    switch (sP_highlightedPlaneViaPosDefintionType.enumValueIndex)
                    {
                        case (int)CameraVisualizer.HighlightedPlaneViaPosDefintionType.fixedPosition:
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("vector3_thatSpecifiesThePosOfTheAdditionalFrustumPlane"),new GUIContent("Position that the flexible plane should contain", "This doesn't have to be inside the frustum itself, but the flexible plane will only be drawn if this position is not behind the near clip plane of the camera."));
                            break;
                        case (int)CameraVisualizer.HighlightedPlaneViaPosDefintionType.gameobject:
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("gameobject_thatSpecifiesThePosOfTheAdditionalFrustumPlane"), new GUIContent("Gameobject that the flexible plane should contain", "This doesn't have to be inside the frustum itself, but the flexible plane will only be drawn if this position is not behind the near clip plane of the camera."));
                            break;
                        default:
                            break;
                    }

                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

                    EditorGUILayout.PropertyField(serializedObject.FindProperty("distanceOfHighlightedPlane_offsetFromPosition"), new GUIContent("Additional offset along Cameras View Direction", "This shifts the plane to farer or nearer relative to the position. The additional flexible highlighted plane is only drawn if the final distance is at least the cameras near clip plane distance."));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("drawFrustumsHighlightedPlaneAlsoIfFarerThanFarClipPlane"), new GUIContent("Draw if farer than far clip plane", "This specifies whether the flexible plane should also be drawn if it is farer than the cameras far clip plane."));
            DrawChooserLineFor_overwriteColorForFrustumsHighlightedPlane();
                    break;
                default:
                    break;
            }

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

        void DrawChooserLineFor_overwriteColorForFrustumsHighlightedPlane()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("doOverwriteColorForFrustumsHighlightedPlane"), new GUIContent("Custom Plane Color", "The default color of highlighted planes is the frustum color, but with an adjusted brightness. Though you can overwrite the color here."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("overwriteColorForFrustumsHighlightedPlane"), GUIContent.none);
            EditorGUILayout.EndHorizontal();
        }

        void DrawTextSpecs()
        {
            //-> this fakes the appearance of beeing inside the parents text specification foldout
            bool emptyLineAtEndIfOutfolded = false;
            DrawTextInputInclMarkupHelper(true, false, null, emptyLineAtEndIfOutfolded);

            if (serializedObject.FindProperty("textSection_isOutfolded").boolValue == true)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("forceTextOnNearPlaneUnmirroredTowardsCam"), new GUIContent("Force text to be unmirrored towards visualized camera", "This overwrites the global behaviour where text always appear unmirrored in the observer camera, which could e.g. be the Scene view camera instead of the herewith visualized camera."));
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

    }
#endif
}
