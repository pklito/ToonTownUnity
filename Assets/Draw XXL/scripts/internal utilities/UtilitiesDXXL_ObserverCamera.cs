namespace DrawXXL
{
    using UnityEngine;

    public class UtilitiesDXXL_ObserverCamera
    {

#if UNITY_EDITOR
        static bool obtainmentOf_sceneViewCam_hasFailed;
#endif
        
        static bool obtainmentOf_gameViewCam_hasFailed;
        public static void GetObserverCamSpecs(out Vector3 observerCamForward_normalized, out Vector3 observerCamUp_normalized, out Vector3 observerCamRight_normalized, out Vector3 cam_to_lineCenter, Vector3 lineStartPos, Vector3 line_startToEnd, Camera cameraFrom_DrawScreenspaceCall)
        {
#if UNITY_EDITOR
            obtainmentOf_sceneViewCam_hasFailed = false;
#endif
            obtainmentOf_gameViewCam_hasFailed = false;

            if (cameraFrom_DrawScreenspaceCall != null)
            {
                observerCamForward_normalized = cameraFrom_DrawScreenspaceCall.transform.forward;
                observerCamUp_normalized = cameraFrom_DrawScreenspaceCall.transform.up;
                observerCamRight_normalized = cameraFrom_DrawScreenspaceCall.transform.right;
                cam_to_lineCenter = cameraFrom_DrawScreenspaceCall.transform.forward;
            }
            else
            {
                if (DrawBasics.cameraForAutomaticOrientation == DrawBasics.CameraForAutomaticOrientation.sceneViewCamera)
                {
                    GetObserverCamSpecs_fromSceneViewCam(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter, lineStartPos, line_startToEnd);
                }
                else
                {
                    GetObserverCamSpecs_fromGameViewCam(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter, lineStartPos, line_startToEnd);
                }
            }
        }

        public static void GetObserverCamSpecs(out Vector3 observerCamForward_normalized, out Vector3 observerCamUp_normalized, out Vector3 observerCamRight_normalized, out Vector3 cam_to_observedPosition, Vector3 observedPosition, DrawBasics.CameraForAutomaticOrientation observerCamera)
        {
            //-> This overload doesn't use the global setting of "DrawBasics.cameraForAutomaticOrientation" but the wanted observer camera can be explicitly defined

#if UNITY_EDITOR
            obtainmentOf_sceneViewCam_hasFailed = false;
#endif
            obtainmentOf_gameViewCam_hasFailed = false;

            if (observerCamera == DrawBasics.CameraForAutomaticOrientation.sceneViewCamera)
            {
                GetObserverCamSpecs_fromSceneViewCam(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, observedPosition, Vector3.zero);
            }
            else
            {
                GetObserverCamSpecs_fromGameViewCam(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, observedPosition, Vector3.zero);
            }
        }

        static void GetObserverCamSpecs_fromSceneViewCam(out Vector3 observerCamForward_normalized, out Vector3 observerCamUp_normalized, out Vector3 observerCamRight_normalized, out Vector3 cam_to_lineCenter, Vector3 lineStartPos, Vector3 line_startToEnd)
        {
#if UNITY_EDITOR
            if (obtainmentOf_sceneViewCam_hasFailed == false)
            {
                //known issue: In some cases after startPlayMode the "lastActiveSceneView" is not null, but delivers default values that don't represent it's actual position/rotation
                //-> It seems to occur, when the sceneView-tab is not part of the main Unity Window but is in a separate window e.g. on a separate screen
                //-> It is fixed as soon as the scene view gets selected

                if (UnityEditor.SceneView.lastActiveSceneView != null)
                {
                    GetObserverCamSpecs_fromNonNullLastActiveSceneViewCam(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter, lineStartPos, line_startToEnd);
                }
                else
                {
                    if (UnityEditor.SceneView.currentDrawingSceneView != null)
                    {
                        GetObserverCamSpecs_fromNonNullCam(UnityEditor.SceneView.currentDrawingSceneView.camera, out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter, lineStartPos, line_startToEnd);
                        //LogCamSpecsToConsole("sceneView-currentDrawing", observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                    }
                    else
                    {
                        obtainmentOf_sceneViewCam_hasFailed = true;
                        //string textSuffix_thatCommunicatesTheFallbackToGameViewCameras = null;
                        //if (obtainmentOf_gameViewCam_hasFailed == false) { textSuffix_thatCommunicatesTheFallbackToGameViewCameras = " -> Now trying fallback to Game view camera."; }
                        //Debug.Log("Draw XXL: automaticTextDirectionOfLines: No Scene view camera was found" + textSuffix_thatCommunicatesTheFallbackToGameViewCameras);
                        GetObserverCamSpecs_fromGameViewCam(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter, lineStartPos, line_startToEnd);
                    }
                }
            }
            else
            {
                ErrorLog_forNoSceneViewAndNoGameViewCameraFound();
                GetFallbackObserverCamSpecs_forNoCamFound(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter);
            }
#else
            GetFallbackObserverCamSpecs_forNoCamFound(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter);
#endif
        }

        static void GetObserverCamSpecs_fromNonNullLastActiveSceneViewCam(out Vector3 observerCamForward_normalized, out Vector3 observerCamUp_normalized, out Vector3 observerCamRight_normalized, out Vector3 cam_to_lineCenter, Vector3 lineStartPos, Vector3 line_startToEnd)
        {
#if UNITY_EDITOR
            if (SceneViewCamHasUninitializedPosAndRotAfterStartingPlayMode(UnityEditor.SceneView.lastActiveSceneView.camera))
            {
                //(the described problem is not always reproducible, sometimes it works correctly also without this workaround)
                ///Problem, if
                //-> the opened Unity editor has more than one scene view tabs
                //-> one of the tabs is docked somewhere in the main window, but hidden, because an other tab of the dock is selected
                //-> the other scene view tab is not in the main window but e.g. on a separate screen
                //-> then OnStartPlaymode the hidden scene view of the main window gets selected as "lastActiveSceneView"
                //-> this now selected scene view tab doesn't deliver the correct values of his camera transform, but unitialized default values (probably because it is hidden/not seen)
                //-> the other scene view tab on the other monitor is prominently seen by the user, but the automatic text alignment appears wrong there, because the default pos/rot of hidden scene view's camera is used.
                //-> the problem is fixed as soon as the user selects the prominent scene view on the other monitor. 
                //-> this block tries to avoid the necessity for the user to select to prominent scene view by automatically falling back to it's values
                //-> this workaround may get confused if more than two scene view tabs are present
                //-> this workaround has the problem, that if a scene view camera is "intentionally" placed at the default pos/rot, then it will forceSelect the other scene view. This is probalby a negligibly seldom case.

                for (int i = 0; i < UnityEditor.SceneView.sceneViews.Count; i++)
                {
                    UnityEditor.SceneView currentlyChecked_sceneView = (UnityEditor.SceneView)UnityEditor.SceneView.sceneViews[i];
                    if (currentlyChecked_sceneView != null)
                    {
                        if (currentlyChecked_sceneView.camera != null)
                        {
                            if (SceneViewCamHasUninitializedPosAndRotAfterStartingPlayMode(currentlyChecked_sceneView.camera) == false)
                            {
                                GetObserverCamSpecs_fromNonNullCam(currentlyChecked_sceneView.camera, out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter, lineStartPos, line_startToEnd);
                                //LogCamSpecsToConsole("sceneView-nonDefaultFromList i_" + i + " / " + UnityEditor.SceneView.sceneViews.Count, observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                                return;
                            }
                        }
                    }
                }
            }

            GetObserverCamSpecs_fromNonNullCam(UnityEditor.SceneView.lastActiveSceneView.camera, out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter, lineStartPos, line_startToEnd);
            //LogCamSpecsToConsole("sceneView-lastActive", observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
#else
            GetFallbackObserverCamSpecs_forNoCamFound(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter);
#endif
        }

        static bool SceneViewCamHasUninitializedPosAndRotAfterStartingPlayMode(Camera nonNullCamera_toCheck)
        {
            return (UtilitiesDXXL_Math.IsDefaultVector(nonNullCamera_toCheck.transform.position) && UtilitiesDXXL_Math.IsQuaternionIdentity(nonNullCamera_toCheck.transform.rotation));
        }

        static void GetObserverCamSpecs_fromGameViewCam(out Vector3 observerCamForward_normalized, out Vector3 observerCamUp_normalized, out Vector3 observerCamRight_normalized, out Vector3 cam_to_lineCenter, Vector3 lineStartPos, Vector3 line_startToEnd)
        {
            if (obtainmentOf_gameViewCam_hasFailed == false)
            {
                UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawALine", true);

                if (automaticallyFoundCamera != null)
                {
                    GetObserverCamSpecs_fromNonNullCam(automaticallyFoundCamera, out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter, lineStartPos, line_startToEnd);
                    //LogCamSpecsToConsole("gameView", observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                }
                else
                {
                    obtainmentOf_gameViewCam_hasFailed = true;
                    //string textSuffix_thatCommunicatesTheFallbackToSceneViewCameras = null;
                    //if (obtainmentOf_sceneViewCam_hasFailed == false) { textSuffix_thatCommunicatesTheFallbackToSceneViewCameras = " -> Now trying fallback to Scene view camera."; }
                    //Debug.Log("Draw XXL: automaticTextDirectionOfLines: The Game view camera could not be found." + textSuffix_thatCommunicatesTheFallbackToSceneViewCameras);
                    GetObserverCamSpecs_fromSceneViewCam(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter, lineStartPos, line_startToEnd);
                }
            }
            else
            {
                ErrorLog_forNoSceneViewAndNoGameViewCameraFound();
                GetFallbackObserverCamSpecs_forNoCamFound(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter);
            }
        }

        static void GetObserverCamSpecs_fromNonNullCam(Camera nonNullCamera_fromWhichToGetTheSpecs, out Vector3 observerCamForward_normalized, out Vector3 observerCamUp_normalized, out Vector3 observerCamRight_normalized, out Vector3 cam_to_lineCenter, Vector3 lineStartPos, Vector3 line_startToEnd)
        {
            observerCamForward_normalized = nonNullCamera_fromWhichToGetTheSpecs.transform.forward;
            observerCamUp_normalized = nonNullCamera_fromWhichToGetTheSpecs.transform.up;
            observerCamRight_normalized = nonNullCamera_fromWhichToGetTheSpecs.transform.right;

            //Note: "cam_to_lineCenter" might be 0 in seldom cases. The using code handles this already.
            if (nonNullCamera_fromWhichToGetTheSpecs.orthographic)
            {
                //"cam_to_lineCenter" is strictly speaking a misnomer in orthographic mode. "camPlane_to_lineCenter" would be more fitting. Anyway: The rest of the class expects "cam.tranform.forward" in this orthographic case.
                cam_to_lineCenter = nonNullCamera_fromWhichToGetTheSpecs.transform.forward;
            }
            else
            {
                Vector3 lineCenter = GetLineCenter(lineStartPos, line_startToEnd);
                cam_to_lineCenter = lineCenter - nonNullCamera_fromWhichToGetTheSpecs.transform.position;
            }
        }

        static Vector3 GetLineCenter(Vector3 lineStartPos, Vector3 line_startToEnd)
        {
            return (lineStartPos + 0.5f * line_startToEnd);
        }

        static void ErrorLog_forNoSceneViewAndNoGameViewCameraFound()
        {
            Debug.LogError("Draw XXL: Neither a Scene view camera nor a Game view camera was found -> automaticTextDirectionOfLines is not possible.");
        }

        static void GetFallbackObserverCamSpecs_forNoCamFound(out Vector3 observerCamForward_normalized, out Vector3 observerCamUp_normalized, out Vector3 observerCamRight_normalized, out Vector3 cam_to_lineCenter)
        {
            observerCamForward_normalized = Vector3.forward;
            observerCamUp_normalized = Vector3.up;
            observerCamRight_normalized = Vector3.right;
            cam_to_lineCenter = Vector3.forward;
            //LogCamSpecsToConsole("noCamFound", observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
        }

        static void LogCamSpecsToConsole(string source, Vector3 observerCamForward_normalized, Vector3 observerCamUp_normalized, Vector3 observerCamRight_normalized, Vector3 cam_to_lineCenter)
        {
            Debug.Log("Observer camera specs:    source: " + source + "    observerCamForward_normalized: " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(observerCamForward_normalized) + "    observerCamUp_normalized: " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(observerCamUp_normalized) + "    observerCamRight_normalized: " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(observerCamRight_normalized) + "    cam_to_lineCenter: " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(cam_to_lineCenter));
        }

    }

}
