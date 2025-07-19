namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class UtilitiesDXXL_Screenspace
    {
        public static List<Vector2> vertices_inViewportSpace0to1 = new List<Vector2>();
        public static InternalDXXL_Plane camPlane = new InternalDXXL_Plane();

        public static Vector2 WorldPos_to_ViewportPos0to1(Camera camera, Vector3 worldPos, bool clampPosBetween0and1)
        {
            if (camera == null)
            {
                return default(Vector2);
            }
            else
            {
                Vector2 viewportPos_0to1 = camera.WorldToViewportPoint(worldPos);
                if (clampPosBetween0and1)
                {
                    return new Vector2(Mathf.Clamp01(viewportPos_0to1.x), Mathf.Clamp01(viewportPos_0to1.y));
                }
                else
                {
                    return viewportPos_0to1;
                }
            }
        }

        public static Vector3 ViewportSpacePos_to_WorldPosOnDrawPlane(Camera camera, Vector2 viewportPos0to1, bool clampPosBetween0and1BeforeTransforming)
        {
            if (clampPosBetween0and1BeforeTransforming)
            {
                viewportPos0to1 = new Vector2(Mathf.Clamp01(viewportPos0to1.x), Mathf.Clamp01(viewportPos0to1.y));
            }
            return camera.ViewportToWorldPoint(new Vector3(viewportPos0to1.x, viewportPos0to1.y, camera.nearClipPlane + DrawScreenspace.drawOffsetBehindCamsNearPlane));
        }

        public static Vector3 ViewportSpacePos_to_WorldPosOnDrawPlane_customClamp(Camera camera, Vector2 viewportPos0to1, float minX, float maxX, float minY, float maxY)
        {
            viewportPos0to1 = new Vector2(Mathf.Clamp(viewportPos0to1.x, minX, maxX), Mathf.Clamp(viewportPos0to1.y, minY, maxY));
            return camera.ViewportToWorldPoint(new Vector3(viewportPos0to1.x, viewportPos0to1.y, camera.nearClipPlane + DrawScreenspace.drawOffsetBehindCamsNearPlane));
        }

        public static float VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(Camera camera, Vector2 posOnViewportWhereToConvert_0to1, bool clampPosBetween0and1BeforeTransforming, float extentInsideViewport0to1)
        {
            if (clampPosBetween0and1BeforeTransforming)
            {
                posOnViewportWhereToConvert_0to1 = new Vector2(Mathf.Clamp01(posOnViewportWhereToConvert_0to1.x), Mathf.Clamp01(posOnViewportWhereToConvert_0to1.y));
            }
            float halfExtentInsideViewport = 0.5f * extentInsideViewport0to1;
            float nearClipPlane_plus_offset = camera.nearClipPlane + DrawScreenspace.drawOffsetBehindCamsNearPlane;

            //Known issue:
            //-> The following three code lines with their "ViewportToWorldPoint()" and ".magnitude" produce in some situations slightly different values, probably due to limited float precision. 
            //-> When drawing in sceenspace it is sufficient that the screenspace-camera moves or rotates for this error to appear
            //-> The error forwards and magnifies till the "UtilitiesDXXL_LineStyles.GetAnimationProgess_for*()"-functions and can lead to jittery animation of Screenspace lines.
           
            Vector3 highEndWorldPos = camera.ViewportToWorldPoint(new Vector3(posOnViewportWhereToConvert_0to1.x, posOnViewportWhereToConvert_0to1.y + halfExtentInsideViewport, nearClipPlane_plus_offset));
            Vector3 lowEndWorldPos = camera.ViewportToWorldPoint(new Vector3(posOnViewportWhereToConvert_0to1.x, posOnViewportWhereToConvert_0to1.y - halfExtentInsideViewport, nearClipPlane_plus_offset));
            return (highEndWorldPos - lowEndWorldPos).magnitude;
        }

        public static float HorizExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(Camera camera, Vector2 posOnViewportWhereToConvert_0to1, bool clampPosBetween0and1BeforeTransforming, float extentInsideViewport0to1)
        {
            if (clampPosBetween0and1BeforeTransforming)
            {
                posOnViewportWhereToConvert_0to1 = new Vector2(Mathf.Clamp01(posOnViewportWhereToConvert_0to1.x), Mathf.Clamp01(posOnViewportWhereToConvert_0to1.y));
            }
            float halfExtentInsideViewport = 0.5f * extentInsideViewport0to1;
            float nearClipPlane_plus_offset = camera.nearClipPlane + DrawScreenspace.drawOffsetBehindCamsNearPlane;
            Vector3 rightEndWorldPos = camera.ViewportToWorldPoint(new Vector3(posOnViewportWhereToConvert_0to1.x + halfExtentInsideViewport, posOnViewportWhereToConvert_0to1.y, nearClipPlane_plus_offset));
            Vector3 leftEndWorldPos = camera.ViewportToWorldPoint(new Vector3(posOnViewportWhereToConvert_0to1.x - halfExtentInsideViewport, posOnViewportWhereToConvert_0to1.y, nearClipPlane_plus_offset));
            return (rightEndWorldPos - leftEndWorldPos).magnitude;
        }

        public static float Get_vertExtentOfViewport_at_distanceFromCam(Camera camera, float distanceFromCam)
        {
            if (camera != null)
            {
                Vector3 lowerScreenCenter_atDistanceFromCam = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.0f, distanceFromCam));
                Vector3 upperScreenCenter_atDistanceFromCam = camera.ViewportToWorldPoint(new Vector3(0.5f, 1.0f, distanceFromCam));
                return (lowerScreenCenter_atDistanceFromCam - upperScreenCenter_atDistanceFromCam).magnitude;
            }
            else
            {
                return 1.0f;
            }
        }

        public static float Get_horizExtentOfViewport_at_distanceFromCam(Camera camera, float distanceFromCam)
        {
            if (camera != null)
            {
                Vector3 leftScreenEnd_atDistanceFromCam = camera.ViewportToWorldPoint(new Vector3(0.0f, 0.5f, distanceFromCam));
                Vector3 rightScreenEnd_atDistanceFromCam = camera.ViewportToWorldPoint(new Vector3(1.0f, 0.5f, distanceFromCam));
                return (leftScreenEnd_atDistanceFromCam - rightScreenEnd_atDistanceFromCam).magnitude;
            }
            else
            {
                return 1.0f;
            }
        }

        public static float Get_diagonalExtentOfViewport_at_distanceFromCam(Camera camera, float distanceFromCam)
        {
            if (camera != null)
            {
                Vector3 lowLeftScreenCorner_atDistanceFromCam = camera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, distanceFromCam));
                Vector3 topRightScreenCorner_atDistanceFromCam = camera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, distanceFromCam));
                return (lowLeftScreenCorner_atDistanceFromCam - topRightScreenCorner_atDistanceFromCam).magnitude;
            }
            else
            {
                return 1.0f;
            }
        }

        public static float WorldSpaceExtent_to_viewportSpaceExtentRelToScreenHeight(Camera camera, Vector3 worldSpacePos_whereWorldSpaceExtentIsMounted, float worldSpaceExtentToConvert)
        {
            if (camera != null)
            {
                float half_worldSpaceExtent_asFloat = 0.5f * worldSpaceExtentToConvert;
                Vector3 half_worldSpaceExtent_asVector3 = camera.transform.up * half_worldSpaceExtent_asFloat;
                Vector3 upperEndOfExtent_inWorldSpace = worldSpacePos_whereWorldSpaceExtentIsMounted + half_worldSpaceExtent_asVector3;
                Vector3 lowerEndOfExtent_inWorldSpace = worldSpacePos_whereWorldSpaceExtentIsMounted - half_worldSpaceExtent_asVector3;
                Vector2 upperEndOfExtent_inViewportSpace0to1 = WorldPos_to_ViewportPos0to1(camera, upperEndOfExtent_inWorldSpace, false);
                Vector2 lowerEndOfExtent_inViewportSpace0to1 = WorldPos_to_ViewportPos0to1(camera, lowerEndOfExtent_inWorldSpace, false);

                //-> the line is always vertical in screenspace, so we can discard the x-component:
                return (upperEndOfExtent_inViewportSpace0to1.y - lowerEndOfExtent_inViewportSpace0to1.y);
            }
            else
            {
                return 0.0f;
            }
        }

        public static bool CheckIfViewportIsTooSmall(Camera camera)
        {
            //Unity behaves like this:
            //-> The viewport rect can theoretically be set to sizes where it protrudes the screen.
            //-> The inspector shows these rect component values as if the rect protrudes the screen
            //-> If a script accesses the camera.rect component values then also the values are returned that protrude the screen
            //BUT:
            //-> Internally the viewport is clamped to the screen
            //-> The actual rendering is done to an other viewport which has been shrinked so it doesn't protrude the screen anymore
            //-> The conversion functions like "ViewportToWorldPoint" or "WorldToViewportPoint" work with the "shrinked" viewport rect.

            Rect viewportRect = camera.rect;

            if (viewportRect.x > 0.9975f)
            {
                Debug.LogError("Camera viewport rect too small (too far right): Draw operation not executed.");
                return true;
            }

            if (viewportRect.y > 0.9975f)
            {
                Debug.LogError("Camera viewport rect too small (too high): Draw operation not executed.");
                return true;
            }

            if (viewportRect.width < 0.0025f)
            {
                Debug.LogError("Camera viewport rect too small (too small width): Draw operation not executed.");
                return true;
            }

            if (viewportRect.height < 0.0025f)
            {
                Debug.LogError("Camera viewport rect too small (too small height): Draw operation not executed.");
                return true;
            }

            if ((viewportRect.x + viewportRect.width) < 0.0025f)
            {
                Debug.LogError("Camera viewport rect too small (too far left): Draw operation not executed.");
                return true;
            }

            if ((viewportRect.y + viewportRect.height) < 0.0025f)
            {
                Debug.LogError("Camera viewport rect too small (too low): Draw operation not executed.");
                return true;
            }

            return false;
        }

        public static bool GetAutomaticCameraForDrawing(out Camera camera, string nameOfRequestingFunction, bool omitErrors = false)
        {
            //function returns whether such a "active enabled main camera" exists.
            if (DrawScreenspace.defaultCameraForDrawing != null)
            {
                if (DrawScreenspace.defaultCameraForDrawing.gameObject.activeInHierarchy)
                {
                    if (DrawScreenspace.defaultCameraForDrawing.enabled)
                    {
                        camera = DrawScreenspace.defaultCameraForDrawing;
                        return true;
                    }
                    else
                    {
                        return Search_theEnabledMainCameraOfTheScene(out camera, nameOfRequestingFunction, omitErrors);
                    }
                }
                else
                {
                    return Search_theEnabledMainCameraOfTheScene(out camera, nameOfRequestingFunction, omitErrors);
                }
            }
            else
            {
                return Search_theEnabledMainCameraOfTheScene(out camera, nameOfRequestingFunction, omitErrors);
            }
        }

        static Camera[] activeCamerasOfTheScene;
        static bool obtainmentOf_sceneViewCam_hasFailed;
        static bool obtainmentOf_gameViewCam_hasFailed;
        static bool Search_theEnabledMainCameraOfTheScene(out Camera camera, string nameOfRequestingFunction, bool omitErrors)
        {
            obtainmentOf_sceneViewCam_hasFailed = false;
            obtainmentOf_gameViewCam_hasFailed = false;
            if (DrawScreenspace.defaultScreenspaceWindowForDrawing == DrawScreenspace.DefaultScreenspaceWindowForDrawing.sceneViewWindow)
            {
                return Search_theEnabledMainSceneViewCameraOfTheScene(out camera, nameOfRequestingFunction, omitErrors);
            }
            else
            {
                return Search_theEnabledMainGameViewCameraOfTheScene(out camera, nameOfRequestingFunction, omitErrors);
            }
        }

        static bool Search_theEnabledMainSceneViewCameraOfTheScene(out Camera camera, string nameOfRequestingFunction, bool omitErrors)
        {
#if UNITY_EDITOR
            if (UnityEditor.SceneView.lastActiveSceneView == null)
            {
                return FallbackToSearchingGameViewCam_orRejectDrawingWithLogNotification(out camera, nameOfRequestingFunction, omitErrors);
            }
            else
            {
                camera = UnityEditor.SceneView.lastActiveSceneView.camera;
                return true;
            }
#else
            return FallbackToSearchingGameViewCam_orRejectDrawingWithLogNotification(out camera, nameOfRequestingFunction, omitErrors);
#endif

        }

        static bool FallbackToSearchingGameViewCam_orRejectDrawingWithLogNotification(out Camera camera, string nameOfRequestingFunction, bool omitErrors)
        {
            obtainmentOf_sceneViewCam_hasFailed = true;
            if (obtainmentOf_gameViewCam_hasFailed)
            {
                return RejectDrawing_andNotifyUserThatThereIsNoCamera(out camera, nameOfRequestingFunction, omitErrors);
            }
            else
            {
                return Search_theEnabledMainGameViewCameraOfTheScene(out camera, nameOfRequestingFunction, omitErrors);
            }
        }

        static bool Search_theEnabledMainGameViewCameraOfTheScene(out Camera camera, string nameOfRequestingFunction, bool omitErrors)
        {
            Camera cameraMain_cached = Camera.main; //"Camera.main" is slow prior to Unity 2020.2
            if (cameraMain_cached != null)
            {
                camera = cameraMain_cached;
                return true;
            }
            else
            {
                if (Camera.allCamerasCount > 0)
                {
                    camera = Camera.allCameras[0];
                    return true;
                }
                else
                {
                    //"FindObjectsOfType<Camera>()" doesn't return the camera if the gameObject (or any parent) is deactivated, but it does return the camera if only the camera component is disabled.
                    activeCamerasOfTheScene = UnityEngine.Object.FindObjectsOfType<Camera>();
                    if (activeCamerasOfTheScene == null)
                    {
                        return FallbackToSearchingSceneViewCam_orRejectDrawingWithLogNotification(out camera, nameOfRequestingFunction, omitErrors);
                    }
                    else
                    {
                        if (activeCamerasOfTheScene.Length == 0)
                        {
                            return FallbackToSearchingSceneViewCam_orRejectDrawingWithLogNotification(out camera, nameOfRequestingFunction, omitErrors);
                        }
                        else
                        {
                            for (int i = 0; i < activeCamerasOfTheScene.Length; i++)
                            {
                                if (activeCamerasOfTheScene[i] != null)
                                {
                                    if (activeCamerasOfTheScene[i].gameObject.activeInHierarchy)
                                    {
                                        if (activeCamerasOfTheScene[i].enabled)
                                        {
                                            camera = activeCamerasOfTheScene[i];
                                            return true;
                                        }
                                    }
                                }
                            }
                            return FallbackToSearchingSceneViewCam_orRejectDrawingWithLogNotification(out camera, nameOfRequestingFunction, omitErrors);
                        }
                    }
                }
            }
        }

        static bool FallbackToSearchingSceneViewCam_orRejectDrawingWithLogNotification(out Camera camera, string nameOfRequestingFunction, bool omitErrors)
        {
            obtainmentOf_gameViewCam_hasFailed = true;
            if (obtainmentOf_sceneViewCam_hasFailed)
            {
                return RejectDrawing_andNotifyUserThatThereIsNoCamera(out camera, nameOfRequestingFunction, omitErrors);
            }
            else
            {
                return Search_theEnabledMainSceneViewCameraOfTheScene(out camera, nameOfRequestingFunction, omitErrors);
            }
        }

        static bool RejectDrawing_andNotifyUserThatThereIsNoCamera(out Camera camera, string nameOfRequestingFunction, bool omitErrors)
        {
            if (omitErrors == false)
            {
                if (nameOfRequestingFunction == null)
                {
                    Debug.LogError("Draw XXL: Cannot draw because there is no active camera in the scene.");
                }
                else
                {
                    Debug.LogError("Draw XXL: " + nameOfRequestingFunction + "() cannot draw because there is no active camera in the scene.");
                }
            }
            camera = null;
            return false;
        }

        public static bool HasDefaultViewPortRect(Camera camera)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(camera.rect.x))
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(camera.rect.y))
                {
                    if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(camera.rect.width, 1.0f))
                    {
                        if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(camera.rect.height, 1.0f))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static float animationSpeedConversionFactor_viewportToWorldSpace = 0.25f;
        static InternalDXXL_LineParamsFromCamViewportSpace lineParams = new InternalDXXL_LineParamsFromCamViewportSpace();
        public static InternalDXXL_LineParamsFromCamViewportSpace GetLineParamsFromCamViewportSpace(Camera camera, Vector2 start, Vector2 end, float width_relToViewportHeight, DrawBasics.LineStyle style, float stylePatternScaleFactor, float enlargeSmallTextToThisMinRelTextSize, float animationSpeed_viewportSpace, float endPlatesSize_relToViewportHeight)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(camera, "camera")) { return null; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width_relToViewportHeight, "width_relToViewportHeight")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(stylePatternScaleFactor, "stylePatternScaleFactor")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(enlargeSmallTextToThisMinRelTextSize, "enlargeSmallTextToThisMinRelTextSize")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(animationSpeed_viewportSpace, "animationSpeed_viewportSpace")) { return null; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(end, "end")) { return null; }

            //DO NOT fallback to "PointScreenSpace()" in case of "line with zero lenght", because "PointScreenSpace()" calls "LineScreenSpace()" again, which can create an endless loop.

            lineParams.startAnchor_worldSpace = ViewportSpacePos_to_WorldPosOnDrawPlane(camera, start, false);
            lineParams.endAnchor_worldSpace = ViewportSpacePos_to_WorldPosOnDrawPlane(camera, end, false);
            Vector2 middleV2 = 0.5f * (start + end);
            Vector2 middleV2_clamped01 = new Vector2(Mathf.Clamp01(middleV2.x), Mathf.Clamp01(middleV2.y));

            if (UtilitiesDXXL_Math.ApproximatelyZero(width_relToViewportHeight))
            {
                lineParams.width_worldSpace = 0.0f;
            }
            else
            {
                lineParams.width_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, middleV2_clamped01, false, width_relToViewportHeight);
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(enlargeSmallTextToThisMinRelTextSize))
            {
                lineParams.enlargeSmallTextToThisMinTextSize_worldSpace = 0.0f;
            }
            else
            {
                lineParams.enlargeSmallTextToThisMinTextSize_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, middleV2_clamped01, false, enlargeSmallTextToThisMinRelTextSize);
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(endPlatesSize_relToViewportHeight))
            {
                lineParams.endPlatesSize_inAbsoluteWorldSpaceUnits = 0.0f;
            }
            else
            {
                lineParams.endPlatesSize_inAbsoluteWorldSpaceUnits = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, middleV2_clamped01, false, endPlatesSize_relToViewportHeight);
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(animationSpeed_viewportSpace))
            {
                lineParams.animationSpeed_worldSpace = 0.0f;
            }
            else
            {
                float animationDirection = Mathf.Sign(animationSpeed_viewportSpace);
                animationSpeed_viewportSpace = animationSpeedConversionFactor_viewportToWorldSpace * animationSpeed_viewportSpace;
                lineParams.animationSpeed_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, middleV2_clamped01, false, animationSpeed_viewportSpace);
                lineParams.animationSpeed_worldSpace = animationDirection * lineParams.animationSpeed_worldSpace;
            }

            lineParams.lineStyleForcedTo2D = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(style);
            lineParams.patternScaleFactor_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, middleV2_clamped01, false, stylePatternScaleFactor);
            lineParams.patternScaleFactor_worldSpace = Mathf.Max(lineParams.patternScaleFactor_worldSpace, UtilitiesDXXL_LineStyles.minStylePatternScaleFactor);
            lineParams.camPlane.Recreate(camera.transform.position, camera.transform.forward);

            return lineParams;
        }

        public static void DrawShape(Camera camera, Vector2 centerPosition, DrawShapes.Shape2DType baseShape, Color colorForShape, Color colorForText, float width_relToViewportHeight, float height_relToViewportHeight, float zRotationDeg, float linesWidth_relToViewportHeight, string text, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool drawPointerIfOffscreen, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec, float relTextSizeScaling, string headerText, bool drawHullEdgeLines_forScreenEncasingShapes)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(camera, "camera")) { return; }
            if (CheckIfViewportIsTooSmall(camera)) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width_relToViewportHeight, "width_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(height_relToViewportHeight, "height_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(zRotationDeg, "zRotationDeg")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth_relToViewportHeight, "linesWidth_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(stylePatternScaleFactor, "stylePatternScaleFactor")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(centerPosition, "centerPosition")) { return; }

            linesWidth_relToViewportHeight = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth_relToViewportHeight);
            if (UtilitiesDXXL_Math.ApproximatelyZero(height_relToViewportHeight) && UtilitiesDXXL_Math.ApproximatelyZero(width_relToViewportHeight))
            {
                PointFallback(camera, centerPosition, "[<color=#adadadFF><icon=logMessage></color> ShapeScreenspace with extent of 0]<br>" + text, colorForShape, linesWidth_relToViewportHeight, durationInSec);
                return;
            }

            lineStyle = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(lineStyle);
            Vector3 centerPosition_worldSpace = ViewportSpacePos_to_WorldPosOnDrawPlane(camera, centerPosition, false);
            float width_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, centerPosition, true, width_relToViewportHeight);
            float height_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, centerPosition, true, height_relToViewportHeight);
            float patternScaleFactor_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, centerPosition, true, stylePatternScaleFactor);
            float linesWidth_worldSpace = 0.0f;
            if (UtilitiesDXXL_Math.ApproximatelyZero(linesWidth_relToViewportHeight) == false)
            {
                linesWidth_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, centerPosition, true, linesWidth_relToViewportHeight);
            }

            Vector3 up_worldSpace_normalized = camera.transform.up;
            if (UtilitiesDXXL_Math.ApproximatelyZero(zRotationDeg) == false)
            {
                Quaternion rotation = Quaternion.AngleAxis(zRotationDeg, camera.transform.forward);
                up_worldSpace_normalized = rotation * camera.transform.up;
            }

            int usedSlotsIn_verticesGlobal = DrawShapes.FlatShape(centerPosition_worldSpace, baseShape, width_worldSpace, height_worldSpace, colorForShape, camera.transform.forward, up_worldSpace_normalized, linesWidth_worldSpace, null, lineStyle, patternScaleFactor_worldSpace, true, DrawBasics.LineStyle.invisible, false, durationInSec, false);
            if (usedSlotsIn_verticesGlobal <= 0)
            {
                UtilitiesDXXL_Log.PrintErrorCode("27-" + usedSlotsIn_verticesGlobal);
                return;
            }

            if (fillStyle != DrawBasics.LineStyle.invisible)
            {
                fillStyle = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(fillStyle);
                camPlane.Recreate(centerPosition_worldSpace, camera.transform.forward);
                float distanceBetweenLines_viewportSpace = 0.01f;
                float distanceBetweenLines_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, centerPosition, true, distanceBetweenLines_viewportSpace);
                UtilitiesDXXL_Shapes.DrawShapeFilling(baseShape, fillStyle, usedSlotsIn_verticesGlobal, distanceBetweenLines_worldSpace, colorForShape, up_worldSpace_normalized, patternScaleFactor_worldSpace, camPlane, durationInSec, false);
            }

            if (drawPointerIfOffscreen || (text != null && text != "") || (headerText != null && headerText != ""))
            {
                for (int i = 0; i < usedSlotsIn_verticesGlobal; i++)
                {
                    UtilitiesDXXL_List.AddToAVector2List(ref vertices_inViewportSpace0to1, WorldPos_to_ViewportPos0to1(camera, UtilitiesDXXL_Shapes.verticesGlobal[i], false), i);
                }
                TagPointCollection(camera, text, headerText, usedSlotsIn_verticesGlobal, 0.3f * linesWidth_relToViewportHeight, colorForShape, colorForText, drawPointerIfOffscreen, addTextForOutsideDistance_toOffscreenPointer, durationInSec, relTextSizeScaling, drawHullEdgeLines_forScreenEncasingShapes);
            }

        }

        public static void Capsule(Camera camera, Vector2 posOfCircle1, Vector2 posOfCircle2, float radius_relToViewportHeight, Color color, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(camera, "camera")) { return; }
            if (CheckIfViewportIsTooSmall(camera)) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius_relToViewportHeight, "radius_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth_relToViewportHeight, "linesWidth_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(stylePatternScaleFactor, "stylePatternScaleFactor")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(posOfCircle1, "posOfCircle1")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(posOfCircle2, "posOfCircle2")) { return; }

            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(posOfCircle1, posOfCircle2) && UtilitiesDXXL_Math.ApproximatelyZero(radius_relToViewportHeight))
            {
                PointFallback(camera, posOfCircle1, "[<color=#adadadFF><icon=logMessage></color> CapsuleScreenspace with extent of 0]<br>" + text, color, linesWidth_relToViewportHeight, durationInSec);
                return;
            }

            Vector2 centerPosition = 0.5f * (posOfCircle1 + posOfCircle2);
            Vector3 posOfCircle1_worldSpace = ViewportSpacePos_to_WorldPosOnDrawPlane(camera, posOfCircle1, false);
            Vector3 posOfCircle2_worldSpace = ViewportSpacePos_to_WorldPosOnDrawPlane(camera, posOfCircle2, false);
            float patternScaleFactor_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, centerPosition, true, stylePatternScaleFactor);

            float radius_worldSpace = 0.0f;
            if (UtilitiesDXXL_Math.ApproximatelyZero(radius_relToViewportHeight) == false)
            {
                radius_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, centerPosition, true, radius_relToViewportHeight);
            }

            float lineWidth_worldSpace = 0.0f;
            if (UtilitiesDXXL_Math.ApproximatelyZero(linesWidth_relToViewportHeight) == false)
            {
                lineWidth_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, centerPosition, true, linesWidth_relToViewportHeight);
            }

            lineStyle = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(lineStyle);
            fillStyle = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(fillStyle);
            float distanceBetweenFillLines_viewportSpace = 0.01f;
            float distanceBetweenFillLines_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, centerPosition, true, distanceBetweenFillLines_viewportSpace);
            int usedSlotsIn_verticesGlobal = UtilitiesDXXL_Shapes.FlatCapsule(posOfCircle1_worldSpace, posOfCircle2_worldSpace, radius_worldSpace, color, camera.transform.forward, lineWidth_worldSpace, null, lineStyle, patternScaleFactor_worldSpace, fillStyle, false, false, durationInSec, false, distanceBetweenFillLines_worldSpace, camera.transform.up);
            DrawTextAtCapsule(camera, drawPointerIfOffscreen, text, usedSlotsIn_verticesGlobal, linesWidth_relToViewportHeight, color, addTextForOutsideDistance_toOffscreenPointer, durationInSec, true);
        }

        public static void Capsule(Camera camera, Vector2 centerPosition, Vector2 size_relToViewportHeight, Color color, CapsuleDirection2D capsuleDirection, float zRotationDeg, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec, bool drawHullEdgeLines_forScreenEncasingShapes)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(camera, "camera")) { return; }
            if (CheckIfViewportIsTooSmall(camera)) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(zRotationDeg, "zRotationDeg")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth_relToViewportHeight, "linesWidth_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(stylePatternScaleFactor, "stylePatternScaleFactor")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(centerPosition, "centerPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(size_relToViewportHeight, "size_relToViewportHeight")) { return; }

            if (UtilitiesDXXL_Math.ApproximatelyZero(size_relToViewportHeight))
            {
                PointFallback(camera, centerPosition, "[<color=#adadadFF><icon=logMessage></color> CapsuleScreenspace with extent of 0]<br>" + text, color, linesWidth_relToViewportHeight, durationInSec);
                return;
            }

            Vector3 centerPosition_worldSpace = ViewportSpacePos_to_WorldPosOnDrawPlane(camera, centerPosition, false);
            float patternScaleFactor_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, centerPosition, true, stylePatternScaleFactor);

            float width_worldSpace = 0.0f;
            if (UtilitiesDXXL_Math.ApproximatelyZero(size_relToViewportHeight.x) == false)
            {
                width_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, centerPosition, true, size_relToViewportHeight.x);
            }

            float height_worldSpace = 0.0f;
            if (UtilitiesDXXL_Math.ApproximatelyZero(size_relToViewportHeight.y) == false)
            {
                height_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, centerPosition, true, size_relToViewportHeight.y);
            }

            float lineWidth_worldSpace = 0.0f;
            if (UtilitiesDXXL_Math.ApproximatelyZero(linesWidth_relToViewportHeight) == false)
            {
                lineWidth_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, centerPosition, true, linesWidth_relToViewportHeight);
            }

            Vector3 upAlongVertInsideCapsulePlane_worldSpace = camera.transform.up;
            if (UtilitiesDXXL_Math.ApproximatelyZero(zRotationDeg) == false)
            {
                Quaternion rotation = Quaternion.AngleAxis(zRotationDeg, camera.transform.forward);
                upAlongVertInsideCapsulePlane_worldSpace = rotation * camera.transform.up;
            }

            lineStyle = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(lineStyle);
            fillStyle = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(fillStyle);
            float distanceBetweenFillLines_viewportSpace = 0.01f;
            float distanceBetweenFillLines_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, centerPosition, true, distanceBetweenFillLines_viewportSpace);
            int usedSlotsIn_verticesGlobal = UtilitiesDXXL_Shapes.FlatCapsule(centerPosition_worldSpace, width_worldSpace, height_worldSpace, color, camera.transform.forward, upAlongVertInsideCapsulePlane_worldSpace, capsuleDirection, lineWidth_worldSpace, null, lineStyle, patternScaleFactor_worldSpace, fillStyle, false, false, durationInSec, false, distanceBetweenFillLines_worldSpace);
            DrawTextAtCapsule(camera, drawPointerIfOffscreen, text, usedSlotsIn_verticesGlobal, linesWidth_relToViewportHeight, color, addTextForOutsideDistance_toOffscreenPointer, durationInSec, drawHullEdgeLines_forScreenEncasingShapes);
        }

        static void DrawTextAtCapsule(Camera camera, bool drawPointerIfOffscreen, string text, int usedSlotsIn_verticesGlobal, float linesWidth_relToViewportHeight, Color color, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec, bool drawHullEdgeLines_forScreenEncasingShapes)
        {
            if (usedSlotsIn_verticesGlobal > 1) //-> preceding Draw.FlatCapsule-Call didn't return with abortDueToError
            {
                if (drawPointerIfOffscreen || (text != null && text != ""))
                {
                    for (int i = 0; i < usedSlotsIn_verticesGlobal; i++)
                    {
                        UtilitiesDXXL_List.AddToAVector2List(ref vertices_inViewportSpace0to1, WorldPos_to_ViewportPos0to1(camera, UtilitiesDXXL_Shapes.verticesGlobal[i], false), i);
                    }
                    TagPointCollection(camera, text, null, usedSlotsIn_verticesGlobal, 0.3f * linesWidth_relToViewportHeight, color, color, drawPointerIfOffscreen, addTextForOutsideDistance_toOffscreenPointer, durationInSec, 1.0f, drawHullEdgeLines_forScreenEncasingShapes);
                }
            }
        }

        public static void PointFallback(Camera camera, Vector2 position, string text = null, Color color = default(Color), float markingCrossLinesWidth_relToViewportHeight = 0.0f, float durationInSec = 0.0f)
        {
            DrawScreenspace.Point(camera, position, text, color, 0.1f, markingCrossLinesWidth_relToViewportHeight, 0.0f, false, true, true, true, durationInSec);
        }

        static InternalDXXL_BoundsCamViewportSpace boundsViewportSpace = new InternalDXXL_BoundsCamViewportSpace();
        public static void TagPointCollection(Camera camera, string text, string headerText, int usedSlotsInVerticesViewportSpaceList, float linesWidth_relToViewportHeight, Color colorForLinesAndHeader, Color colorForText, bool drawPointerIfOffscreen, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec, float relTextSizeScaling, bool drawHullEdgeLines_forScreenEncasingShapes)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (usedSlotsInVerticesViewportSpaceList <= 0)
            {
                UtilitiesDXXL_Log.PrintErrorCode("30-" + usedSlotsInVerticesViewportSpaceList);
                return;
            }

            if (drawPointerIfOffscreen || (text != null && text != "") || (headerText != null && headerText != ""))
            {
                boundsViewportSpace.Recreate(vertices_inViewportSpace0to1[0], Vector2.zero);
                for (int i = 1; i < usedSlotsInVerticesViewportSpaceList; i++)
                {
                    boundsViewportSpace.Encapsulate(vertices_inViewportSpace0to1[i]);
                }

                Vector2 taggedPos;
                if (boundsViewportSpace.IsCompletelyInsideViewport())
                {
                    // Vector2 camCenterNearestBoundsCorner = boundsViewportSpace.GetNearestCorner(InternalDXXL_BoundsCamViewportSpace.viewportCenter); //-> makes the textPos flicker in common cases where symetrical verticesCenterPos is on a viewport0.5-axis
                    Vector2 camCenterNearestBoundsCorner = boundsViewportSpace.GetNearestCorner(new Vector2(0.51f, 0.51f)); //-> prevent textPos-flicker of common case where symetrical verticesCenterPos is on a viewport0.5-axis
                    taggedPos = UtilitiesDXXL_Math.GetNearestVertex(camCenterNearestBoundsCorner, vertices_inViewportSpace0to1, usedSlotsInVerticesViewportSpaceList);
                }
                else
                {
                    if ((boundsViewportSpace.HasCornerInsideViewport() == false) && boundsViewportSpace.HasEdgePartInsideViewport())
                    {
                        taggedPos = boundsViewportSpace.GetPosOnMostCenteredViewportCrossingEdge(0.55f);
                        if (drawHullEdgeLines_forScreenEncasingShapes)
                        {
                            boundsViewportSpace.DrawViewportCrossingEdges(camera, colorForLinesAndHeader, linesWidth_relToViewportHeight, durationInSec);
                        }
                    }
                    else
                    {
                        if (boundsViewportSpace.CompletelyEncapsulatesViewport())
                        {
                            // taggedPos = InternalDXXL_BoundsCamViewportSpace.GetViewportCenterPlumbIntersectionWithViewportBorderShifted(boundsViewportSpace.center, -0.01f); //-> makes the textPos flicker in common cases where symetrical verticesCenterPos is on a viewport0.5-axis
                            taggedPos = InternalDXXL_BoundsCamViewportSpace.GetViewportCenterPlumbIntersectionWithViewportBorderShifted(boundsViewportSpace.center + new Vector2(0.01f, 0.01f), -0.01f);//-> prevent textPos-flicker of common case where symetrical verticesCenterPos is on a viewport0.5-axis
                            if (drawHullEdgeLines_forScreenEncasingShapes)
                            {
                                InternalDXXL_BoundsCamViewportSpace.DrawViewportBorder(camera, colorForLinesAndHeader, linesWidth_relToViewportHeight, 0.01f, durationInSec);
                            }
                        }
                        else
                        {
                            //"hasCornerInsideViewport" or "completelyOutsideViewport"
                            // taggedPos = DSU_Math.GetNearestVertex(InternalDXXL_BoundsCamViewportSpace.viewportCenter, vertices_inViewportSpace0to1, usedSlotsInVerticesViewportSpaceList); //-> makes the textPos flicker in common cases where symetrical verticesCenterPos is on a viewport0.5-axis
                            taggedPos = UtilitiesDXXL_Math.GetNearestVertex(new Vector2(0.51f, 0.51f), vertices_inViewportSpace0to1, usedSlotsInVerticesViewportSpaceList); //-> prevent textPos-flicker of common case where symetrical verticesCenterPos is on a viewport0.5-axis
                        }
                    }
                }

                bool taggedPosIsOutsideOfViewport = InternalDXXL_BoundsCamViewportSpace.IsOutsideViewportExclBorder(taggedPos);
                bool forcePointerDueToOffscreen = (taggedPosIsOutsideOfViewport && drawPointerIfOffscreen);
                if (forcePointerDueToOffscreen || (text != null && text != "") || (headerText != null && headerText != ""))
                {
                    bool withPointer = taggedPosIsOutsideOfViewport;
                    PointTag(camera, taggedPos, text, headerText, colorForLinesAndHeader, colorForText, drawPointerIfOffscreen, linesWidth_relToViewportHeight, 0.2f, default(Vector2), relTextSizeScaling, !withPointer, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
                }
            }
        }

        public static void PointTag(Camera camera, Vector2 position, string text, string titleText, Color colorForLinesAndTitle, Color colorForText, bool drawPointerIfOffscreen, float linesWidth_relToViewportHeight, float size_asTextOffsetDistance_relToViewportHeight, Vector2 textOffsetDirection, float textSizeScaleFactor, bool skipConeDrawing, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec, Vector2 customTowardsPoint_ofDefaultTextOffsetDirection = default(Vector2))
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(camera, "camera")) { return; }
            if (CheckIfViewportIsTooSmall(camera)) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth_relToViewportHeight, "linesWidth_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(size_asTextOffsetDistance_relToViewportHeight, "size_asTextOffsetDistance_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(textSizeScaleFactor, "textSizeScaleFactor")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(textOffsetDirection, "textOffsetDirection")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(customTowardsPoint_ofDefaultTextOffsetDirection, "customTowardsPoint_ofDefaultTextOffsetDirection")) { return; }

            //DO NOT fallback to "PointScreenSpace()" here, because "PointScreenSpace()" calls "PointTagScreenSpace()" again, which can create an endless loop.

            if (drawPointerIfOffscreen == false)
            {
                if (InternalDXXL_BoundsCamViewportSpace.IsOutsideViewportWithPadding(position, 1.5f))
                {
                    return;
                }
            }

            colorForLinesAndTitle = UtilitiesDXXL_Colors.OverwriteDefaultColor(colorForLinesAndTitle);
            colorForText = UtilitiesDXXL_Colors.OverwriteDefaultColor(colorForText);
            Vector2 position_viewportSpace = position;
            UtilitiesDXXL_Math.SkewedDirection quadrant = GetQuadrant(position_viewportSpace);
            bool customTextOffsetDir;
            Vector2 textOffsetDir_viewportSpace;
            if (UtilitiesDXXL_Math.IsDefaultVector(textOffsetDirection))
            {
                Vector2 towardsPoint_ofDefaultTextOffsetDir = UtilitiesDXXL_Math.OverwriteDefaultVectors(customTowardsPoint_ofDefaultTextOffsetDirection, InternalDXXL_BoundsCamViewportSpace.viewportCenter);
                textOffsetDir_viewportSpace = towardsPoint_ofDefaultTextOffsetDir - position_viewportSpace;
                if (UtilitiesDXXL_Math.ApproximatelyZero(textOffsetDir_viewportSpace))
                {
                    textOffsetDir_viewportSpace = new Vector2(0.5f, 1.0f);
                }
                customTextOffsetDir = false;
            }
            else
            {
                textOffsetDir_viewportSpace = textOffsetDirection;
                customTextOffsetDir = true;
            }

            linesWidth_relToViewportHeight = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth_relToViewportHeight);
            float lineWidth_worldSpace = 0.0f;
            if (UtilitiesDXXL_Math.ApproximatelyZero(linesWidth_relToViewportHeight) == false)
            {
                lineWidth_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, position_viewportSpace, true, linesWidth_relToViewportHeight);
            }

            size_asTextOffsetDistance_relToViewportHeight = Mathf.Abs(size_asTextOffsetDistance_relToViewportHeight);
            size_asTextOffsetDistance_relToViewportHeight = UtilitiesDXXL_Math.Max(size_asTextOffsetDistance_relToViewportHeight, 3.0f * linesWidth_relToViewportHeight, 0.035f);
            float textOffsetDistance_worldSpace = VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, position_viewportSpace, true, size_asTextOffsetDistance_relToViewportHeight);
            textSizeScaleFactor = Mathf.Abs(textSizeScaleFactor);
            textSizeScaleFactor = Mathf.Max(textSizeScaleFactor, 0.01f);

            Vector3 postion_worldSpace = ViewportSpacePos_to_WorldPosOnDrawPlane(camera, position_viewportSpace, false);
            Vector3 aPosTowardsTextStartPos_worldSpace = ViewportSpacePos_to_WorldPosOnDrawPlane(camera, position_viewportSpace + textOffsetDir_viewportSpace, false);
            Vector3 textOffsetDir_worldSpace = aPosTowardsTextStartPos_worldSpace - postion_worldSpace;
            Vector3 textOffsetDir_worldSpace_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(textOffsetDir_worldSpace);
            Vector3 pos_to_startOfUnderLine_worldSpace = textOffsetDir_worldSpace_normalized * textOffsetDistance_worldSpace;

            float coneHeight_worldSpace = 0.2f * textOffsetDistance_worldSpace;
            coneHeight_worldSpace = Mathf.Max(coneHeight_worldSpace, 2.4f * lineWidth_worldSpace);

            if (drawPointerIfOffscreen)
            {
                if (InternalDXXL_BoundsCamViewportSpace.IsInsideViewportExclBorder(position_viewportSpace) == false)
                {
                    skipConeDrawing = false;
                    if (addTextForOutsideDistance_toOffscreenPointer)
                    {
                        Vector2 projectionOntoScreenBorder = InternalDXXL_BoundsCamViewportSpace.ClampIntoViewport(position_viewportSpace);
                        float distance = (position_viewportSpace - projectionOntoScreenBorder).magnitude;

                        if (text != null && text != "")
                        {
                            text = "[distance = " + distance.ToString("F2") + "]<br>" + text;
                        }
                        else
                        {
                            text = "[distance = " + distance.ToString("F2") + "]";
                        }
                    }
                }

                float maxOutsideScreen0to1 = customTextOffsetDir ? (-0.1f * coneHeight_worldSpace) : 0.3f * coneHeight_worldSpace;
                postion_worldSpace = ViewportSpacePos_to_WorldPosOnDrawPlane_customClamp(camera, position_viewportSpace, -maxOutsideScreen0to1, 1.0f + maxOutsideScreen0to1, -maxOutsideScreen0to1, 1.0f + maxOutsideScreen0to1);
            }

            Vector3 startOfTextUnderline_worldSpace = postion_worldSpace + pos_to_startOfUnderLine_worldSpace;
            Vector2 startOfTextUnderline_viewportSpace = WorldPos_to_ViewportPos0to1(camera, startOfTextUnderline_worldSpace, false);

            if (skipConeDrawing)
            {
                Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, position_viewportSpace, startOfTextUnderline_viewportSpace, colorForLinesAndTitle, linesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
            }
            else
            {
                float offsetDistance_forStartAnchorOfLineToText_worldSpace = (3.0f * lineWidth_worldSpace);
                offsetDistance_forStartAnchorOfLineToText_worldSpace = Mathf.Min(offsetDistance_forStartAnchorOfLineToText_worldSpace, coneHeight_worldSpace);
                Vector3 offsettedStartAnchor_ofLineToText_worldSpace = postion_worldSpace + textOffsetDir_worldSpace_normalized * offsetDistance_forStartAnchorOfLineToText_worldSpace;
                Vector2 offsettedStartAnchor_ofLineToText_viewportSpace = WorldPos_to_ViewportPos0to1(camera, offsettedStartAnchor_ofLineToText_worldSpace, false);
                Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, offsettedStartAnchor_ofLineToText_viewportSpace, startOfTextUnderline_viewportSpace, colorForLinesAndTitle, linesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
            }

            float underlineLength_viewportSpace = 0.2f * size_asTextOffsetDistance_relToViewportHeight;
            float textSize_relToViewportHeight = UtilitiesDXXL_DrawBasics.pointTagsTextSize_relToOffset * textSizeScaleFactor * size_asTextOffsetDistance_relToViewportHeight;
            Vector2 textPosition_viewportSpace = startOfTextUnderline_viewportSpace + Vector2.up * (0.3f * textSize_relToViewportHeight + 0.5f * linesWidth_relToViewportHeight);
            textSize_relToViewportHeight = Mathf.Max(textSize_relToViewportHeight, DrawScreenspace.minTextSize_relToViewportHeight);
            if (text != null && text != "")
            {
                DrawText.TextAnchorDXXL textAnchor = GetTextAnchor(quadrant);
                UtilitiesDXXL_Text.WriteScreenspace(camera, text, textPosition_viewportSpace, colorForText, textSize_relToViewportHeight, 0.0f, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, true, 0.0f, false, durationInSec, false);
                float lengthOfLongestLine_inText_viewportSpace = DrawText.parsedTextSpecs.widthOfLongestLine;
                underlineLength_viewportSpace = Mathf.Max(underlineLength_viewportSpace, lengthOfLongestLine_inText_viewportSpace);
            }

            if (titleText != null && titleText != "")
            {
                //-> no strokeWidth-markup: trading execution time and code readability for GC.Alloc()-prevention:
                // Color titleTextColor = DSU_Colors.Get_color_darkenedFromGivenColor(colorForLinesAndTitle, 1.3f);
                DrawText.TextAnchorDXXL titleTextAnchor = GetTitleTextAnchor(quadrant);
                Vector2 offsetForDoubledPrint_viewportSpace = camera.transform.right * textSize_relToViewportHeight * 0.11f;
                Vector2 offsetForTripledPrint_viewportSpace = camera.transform.right * textSize_relToViewportHeight * 0.055f + camera.transform.up * textSize_relToViewportHeight * 0.08f;
                UtilitiesDXXL_Text.WriteScreenspace(camera, titleText, textPosition_viewportSpace, colorForLinesAndTitle, textSize_relToViewportHeight, 0.0f, titleTextAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, true, 0.0f, false, durationInSec, false);
                float lengthOfLongestLine_inTitleText_viewportSpace = DrawText.parsedTextSpecs.widthOfLongestLine;
                UtilitiesDXXL_Text.WriteScreenspace(camera, titleText, textPosition_viewportSpace + offsetForDoubledPrint_viewportSpace, colorForLinesAndTitle, textSize_relToViewportHeight, 0.0f, titleTextAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, true, 0.0f, false, durationInSec, false);
                UtilitiesDXXL_Text.WriteScreenspace(camera, titleText, textPosition_viewportSpace + offsetForTripledPrint_viewportSpace, colorForLinesAndTitle, textSize_relToViewportHeight, 0.0f, titleTextAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, true, 0.0f, false, durationInSec, false);
                underlineLength_viewportSpace = Mathf.Max(underlineLength_viewportSpace, lengthOfLongestLine_inTitleText_viewportSpace);
            }

            Vector2 textDir_viewportSpace_normalized = (quadrant == UtilitiesDXXL_Math.SkewedDirection.upLeft || quadrant == UtilitiesDXXL_Math.SkewedDirection.downLeft) ? Vector2.right : Vector2.left;
            Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, startOfTextUnderline_viewportSpace, startOfTextUnderline_viewportSpace + textDir_viewportSpace_normalized * underlineLength_viewportSpace, colorForLinesAndTitle, linesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);

            if (skipConeDrawing == false)
            {
                float coneAngleDeg = 25.0f;
                Vector3 upVector_ofConeBaseRect = camera.transform.forward;
                DrawShapes.ConeFilled(postion_worldSpace, coneHeight_worldSpace, pos_to_startOfUnderLine_worldSpace, upVector_ofConeBaseRect, 0.0f, coneAngleDeg, colorForLinesAndTitle, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, false);
            }
        }

        static UtilitiesDXXL_Math.SkewedDirection GetQuadrant(Vector2 position_viewportSpace)
        {
            //float viewportCenter_1D = 0.5f; //-> makes the textPos flicker in common cases where the taggedPosition is on a viewport0.5-axis
            float viewportCenter_1D = 0.505f; //-> prevent textPos-flicker of common case where the taggedPosition is on a viewport0.5-axis

            if (position_viewportSpace.x <= viewportCenter_1D)
            {
                if (position_viewportSpace.y < viewportCenter_1D)
                {
                    return UtilitiesDXXL_Math.SkewedDirection.downLeft;
                }
                else
                {
                    return UtilitiesDXXL_Math.SkewedDirection.upLeft;
                }
            }
            else
            {
                if (position_viewportSpace.y < viewportCenter_1D)
                {
                    return UtilitiesDXXL_Math.SkewedDirection.downRight;
                }
                else
                {
                    return UtilitiesDXXL_Math.SkewedDirection.upRight;
                }
            }
        }

        static DrawText.TextAnchorDXXL GetTextAnchor(UtilitiesDXXL_Math.SkewedDirection quadrant)
        {
            switch (quadrant)
            {
                case UtilitiesDXXL_Math.SkewedDirection.downLeft:
                    return DrawText.TextAnchorDXXL.LowerLeft;

                case UtilitiesDXXL_Math.SkewedDirection.upLeft:
                    return DrawText.TextAnchorDXXL.UpperLeft;

                case UtilitiesDXXL_Math.SkewedDirection.downRight:
                    return DrawText.TextAnchorDXXL.LowerRight;

                case UtilitiesDXXL_Math.SkewedDirection.upRight:
                    return DrawText.TextAnchorDXXL.UpperRight;

                default:
                    return DrawText.TextAnchorDXXL.UpperLeft;
            }
        }

        static DrawText.TextAnchorDXXL GetTitleTextAnchor(UtilitiesDXXL_Math.SkewedDirection quadrant)
        {
            switch (quadrant)
            {
                case UtilitiesDXXL_Math.SkewedDirection.downLeft:
                    return DrawText.TextAnchorDXXL.UpperLeft;

                case UtilitiesDXXL_Math.SkewedDirection.upLeft:
                    return DrawText.TextAnchorDXXL.LowerLeft;

                case UtilitiesDXXL_Math.SkewedDirection.downRight:
                    return DrawText.TextAnchorDXXL.UpperRight;

                case UtilitiesDXXL_Math.SkewedDirection.upRight:
                    return DrawText.TextAnchorDXXL.LowerRight;

                default:
                    return DrawText.TextAnchorDXXL.LowerLeft;
            }
        }

    }

}
