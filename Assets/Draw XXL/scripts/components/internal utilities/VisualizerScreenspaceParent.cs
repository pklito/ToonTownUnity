namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Internal Not For Manual Creation/Visualizer Screenspace Parent")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class VisualizerScreenspaceParent : VisualizerParent
    {
        public enum ScreenspaceDefiningCameras { cameraAtThisGameobject, manuallyAssignedCamera, automaticallySearchForMainGameViewCamera, sceneViewCamera };
        [SerializeField] public ScreenspaceDefiningCameras screenspaceDefiningCamera = ScreenspaceDefiningCameras.cameraAtThisGameobject;
        ScreenspaceDefiningCameras screenspaceDefiningCamera_duringLastCall = ScreenspaceDefiningCameras.cameraAtThisGameobject;

        Camera cameraComponentOnThisGameobject;
        Camera sceneViewCamera_thatStaysEvenIfTheFocusChangesToAnotherSceneViewWindow;
        [SerializeField] public Camera manuallyAssignedCamera;
        [SerializeField] public bool alwaysChangeToCurrentlyActiveSceneView_insteadOfStayingAtTheSelectedOne = false;
        Camera usedCamera;
        [SerializeField] public bool usedCameraIsAvailable;
        public Vector2 positionInsideViewport0to1 = new Vector2(0.5f, 0.5f);
        public Vector2 positionInsideViewport0to1_v2 = new Vector2(0.5f, 0.5f);

        public void TryFetchCamOnThisGO_andDecideScreenspaceDefiningCamera()
        {
            //-> is also called when components or gameobjects (containing this component) get copied und therefore the reference is updated to the new camera component on the new gameobject
            if (screenspaceDefiningCamera == ScreenspaceDefiningCameras.cameraAtThisGameobject)
            {
                this.TryGetComponent(out cameraComponentOnThisGameobject);
                if (cameraComponentOnThisGameobject == null)
                {
                    screenspaceDefiningCamera = ScreenspaceDefiningCameras.automaticallySearchForMainGameViewCamera;
                    screenspaceDefiningCamera_duringLastCall = ScreenspaceDefiningCameras.automaticallySearchForMainGameViewCamera;
                }
            }
        }

        public Camera Get_usedCamera(string nameOfComponentForErrorLog)
        {
            switch (screenspaceDefiningCamera)
            {
                case ScreenspaceDefiningCameras.cameraAtThisGameobject:
                    if (cameraComponentOnThisGameobject == null)
                    {
                        //this is for the case when a camera component is created at the gameobject AFTER the drawer component has been created
                        //it is accepted to call "TryGetComponent" here frequently (per Update-loop), because it is anyway only during an invalid case ("no camera found") on which the user gets notified to fix it via inspector help box.
                        this.TryGetComponent(out cameraComponentOnThisGameobject);
                    }
                    usedCamera = cameraComponentOnThisGameobject;
                    break;
                case ScreenspaceDefiningCameras.manuallyAssignedCamera:
                    usedCamera = manuallyAssignedCamera;
                    break;
                case ScreenspaceDefiningCameras.automaticallySearchForMainGameViewCamera:
                    UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out usedCamera, nameOfComponentForErrorLog, true);
                    break;
                case ScreenspaceDefiningCameras.sceneViewCamera:
#if UNITY_EDITOR
                    if (UnityEditor.SceneView.lastActiveSceneView != null)
                    {
                        if (sceneViewCamera_thatStaysEvenIfTheFocusChangesToAnotherSceneViewWindow == null) { sceneViewCamera_thatStaysEvenIfTheFocusChangesToAnotherSceneViewWindow = UnityEditor.SceneView.lastActiveSceneView.camera; }

                        if ((screenspaceDefiningCamera_duringLastCall != ScreenspaceDefiningCameras.sceneViewCamera) || (alwaysChangeToCurrentlyActiveSceneView_insteadOfStayingAtTheSelectedOne == true))
                        {
                            sceneViewCamera_thatStaysEvenIfTheFocusChangesToAnotherSceneViewWindow = UnityEditor.SceneView.lastActiveSceneView.camera;
                        }

                        if (alwaysChangeToCurrentlyActiveSceneView_insteadOfStayingAtTheSelectedOne == false)
                        {
                            usedCamera = sceneViewCamera_thatStaysEvenIfTheFocusChangesToAnotherSceneViewWindow;
                        }
                        else
                        {
                            usedCamera = UnityEditor.SceneView.lastActiveSceneView.camera;
                        }
                    }
                    else
                    {
                        usedCamera = null;
                    }
#else
                    UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out usedCamera, nameOfComponentForErrorLog, true);
#endif
                    break;
                default:
                    usedCamera = null;
                    break;
            }

            screenspaceDefiningCamera_duringLastCall = screenspaceDefiningCamera;
            usedCameraIsAvailable = (usedCamera != null);
            return usedCamera;
        }

        public bool CheckIf_usedCameraIsActiveAndEnabled()
        {
            if (usedCamera != null)
            {
                bool isSceneViewCamera = false;
#if UNITY_EDITOR
                if (UnityEditor.SceneView.lastActiveSceneView != null)
                {
                    isSceneViewCamera = (usedCamera == UnityEditor.SceneView.lastActiveSceneView.camera);
                }
#endif
                return (isSceneViewCamera || usedCamera.isActiveAndEnabled);
            }
            else
            {
                return false;
            }
        }

    }

}
