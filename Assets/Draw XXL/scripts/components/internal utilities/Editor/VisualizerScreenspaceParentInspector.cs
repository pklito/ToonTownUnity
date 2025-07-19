namespace DrawXXL
{
    using System;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(VisualizerScreenspaceParent))]
    [CanEditMultipleObjects]
    public class VisualizerScreenspaceParentInspector : VisualizerParentInspector
    {
        public string tooltip_explaining_relativeToViewPortHeight = "This is relative to the viewport height.";
        public VisualizerScreenspaceParent visualizerScreenspaceParentMonoBehaviour_unserialized;
        void OnEnable()
        {
            OnEnable_base();
            OnEnable_ofScreenspaceParent();
        }

        public void OnEnable_ofScreenspaceParent()
        {
            visualizerScreenspaceParentMonoBehaviour_unserialized = (VisualizerScreenspaceParent)target;
        }

        public bool DrawCameraChooser(bool drawWarningBox_forDisabledCamComponents)
        {
            SerializedProperty sP_screenspaceDefiningCamera = serializedObject.FindProperty("screenspaceDefiningCamera");
            SerializedProperty sP_usedCameraIsAvailable = serializedObject.FindProperty("usedCameraIsAvailable");
            EditorGUILayout.PropertyField(sP_screenspaceDefiningCamera, new GUIContent("Camera for drawing", "The viewport space of this camera is used for drawing."));

            switch (sP_screenspaceDefiningCamera.enumValueIndex)
            {
                case (int)VisualizerScreenspaceParent.ScreenspaceDefiningCameras.cameraAtThisGameobject:
                    if (sP_usedCameraIsAvailable.boolValue == false)
                    {
                        EditorGUILayout.HelpBox("Drawing is skipped: There is no camera component attached to this gameobject.", MessageType.Warning, true);
                        return false;
                    }
                    break;
                case (int)VisualizerScreenspaceParent.ScreenspaceDefiningCameras.manuallyAssignedCamera:
                    SerializedProperty sP_manuallyAssignedCamera = serializedObject.FindProperty("manuallyAssignedCamera");
                    EditorGUILayout.PropertyField(sP_manuallyAssignedCamera, new GUIContent("Manually assigned camera"));
                    if (sP_manuallyAssignedCamera.objectReferenceValue == null)
                    {
                        EditorGUILayout.HelpBox("Assign a camera to start drawing.", MessageType.Info, true);
                        return false;
                    }
                    break;
                case (int)VisualizerScreenspaceParent.ScreenspaceDefiningCameras.automaticallySearchForMainGameViewCamera:
                    if (sP_usedCameraIsAvailable.boolValue == false)
                    {
                        EditorGUILayout.HelpBox("Drawing is skipped: No camera could be found.", MessageType.Warning, true);
                        return false;
                    }
                    break;
                case (int)VisualizerScreenspaceParent.ScreenspaceDefiningCameras.sceneViewCamera:
                    if (sP_usedCameraIsAvailable.boolValue == false)
                    {
                        EditorGUILayout.HelpBox("Drawing is skipped: There is no Scene View Window available.", MessageType.Warning, true);
                        return false;
                    }
                    else
                    {
                        EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("alwaysChangeToCurrentlyActiveSceneView_insteadOfStayingAtTheSelectedOne"), new GUIContent("Always change to the currently active Scene View.", "Always change to the currently active Scene View:" + Environment.NewLine + Environment.NewLine + "This affects the situation when there is more then one Scene View window docked in the Editor." + Environment.NewLine + Environment.NewLine + "If it is disabled:" + Environment.NewLine + "The visualized camera will always remain the one of the same Scene View window, also if the focus changes between multiple Scene View windows." + Environment.NewLine + Environment.NewLine + "If it is enabled:" + Environment.NewLine + "The visualized camera will ongoingly switch to be the one of the Scene View window that has focus."));
                        EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                    }
                    break;
                default:
                    break;
            }

            if (drawWarningBox_forDisabledCamComponents)
            {
                if (visualizerScreenspaceParentMonoBehaviour_unserialized.CheckIf_usedCameraIsActiveAndEnabled() == false)
                {
                    EditorGUILayout.HelpBox("The drawing may be invisible, because the used camera component is inactive or disabled.", MessageType.Warning, true);
                }
            }

            return true;
        }

    }
#endif
}
