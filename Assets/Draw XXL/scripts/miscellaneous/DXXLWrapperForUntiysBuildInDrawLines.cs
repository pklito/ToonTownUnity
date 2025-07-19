//#define DRAWXXLGIZMODEBUG       //-> delete the two slashes ("//") at the start of this line so that "//#define DRAWXXLGIZMODEBUG" becomes "#define DRAWXXLGIZMODEBUG" to start debugging. To end debugging simply add the two slashes as they were before.

//-> Debugging with "#define DRAWXXLGIZMODEBUG" can help to fix errors like "ArgumentException: Gizmo drawing functions can only be used in OnDrawGizmos and OnDrawGizmosSelected."
//-> make sure to draw only one line when using "#define DRAWXXLGIZMODEBUG", respectively two lines: One from "Update()" and one from "OnDrawGizmos()". Otherwise there can be log spam that significantly slows down the Unity Editor.
//-> if you add a Gizmo Line Count Manager beforehand via "automatic" you may see more what's going on, because of the continuous repaint in Edit mode.
//-> the following snippet is handy for drawing the line [while I recommend to change the two "Vector3.zero" and the color to distinguish between the "Update()"-line and the "OnDrawGizmos()"-line]:
//   DrawBasics.Line(Vector3.zero, Vector3.zero + new Vector3(Mathf.Sin((float)UnityEditor.EditorApplication.timeSinceStartup), Mathf.Cos((float)UnityEditor.EditorApplication.timeSinceStartup), 0), Color.red);

namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class DXXLWrapperForUntiysBuildInDrawLines
    {
        static int mostRecentFrameCount_forWhichLinesPerFrameHasBeenZeroed = -1;
        static bool drawingStopped_dueToTooManyDrawnLinesPerFrame = false;
        static bool maxLinesPerFrameWarningTextDraw_hasAlreadyBeenStarted_duringCurrFrame = false;
        static bool maxLinesPerFrameWarningTextDraw_hasAlreadyBeenFinished_duringCurrFrame = false;
        static int frameCount_ofFrameInsidePausePhaseThatCanSafelyUseGizmoLines;
        static int virtualGizmoCycleCount_inTheMomentOfLastClickOnPauseOrStepButton;
        static int frameCountAfterStepDuringPausePhase_inWhichTheGizmoCycleCounterStarted;

        private static int drawnLinesSinceFrameStart = 0;
        public static int DrawnLinesSinceFrameStart
        {
            get { return drawnLinesSinceFrameStart; }
            set { Debug.Log("Not allowed to set 'DrawnLinesSinceFrameStart' manually."); }
        }

        private static long drawnLinesSinceStart = 0;
        public static long DrawnLinesSinceStart
        {
            get { return drawnLinesSinceStart; }
            set { Debug.Log("Not allowed to set 'DrawnLinesSinceStart' manually."); }
        }

        public static float globalAlphaFactor = 1.0f; //Don't change this value here. Use "DrawBasics.GlobalAlphaFactor" instead.
        public static bool globalAlphaFactor_is0 = false;
        public static bool globalAlphaFactor_is1 = true;
        public static ChartDrawing currentlyDrawingChart = null;
        public static PieChartDrawing currentlyDrawingPieChart = null;

#if UNITY_EDITOR
        static DXXLWrapperForUntiysBuildInDrawLines()
        {
            //-> should not need the [InitializeOnLoad] attribute since it only affects static functions inside this class, see also https://docs.unity3d.com/Manual/RunningEditorCodeOnLaunch.html
            //-> is also called onEnterPlaymode if DomainReload is deactivated

#if DRAWXXLGIZMODEBUG
            Debug.Log("[Draw XXL] Register Pause-Callback     fCount: " + Time.frameCount + "     fCount_safelyG: " + frameCount_ofFrameInsidePausePhaseThatCanSafelyUseGizmoLines + "     fCount_dueToStep: " + frameCountAfterStepDuringPausePhase_inWhichTheGizmoCycleCounterStarted + "        gCount: " + UtilitiesDXXL_Components.virtualGizmoCycleCount + "    gCount_onStep: " + virtualGizmoCycleCount_inTheMomentOfLastClickOnPauseOrStepButton);
#endif

            UnityEditor.EditorApplication.pauseStateChanged -= OnPauseStateChangedCallback;
            UnityEditor.EditorApplication.pauseStateChanged += OnPauseStateChangedCallback;
        }

        private static void OnPauseStateChangedCallback(UnityEditor.PauseState state)
        {
            if (state == UnityEditor.PauseState.Paused)
            {
                //-> This fires when the pausePhase starts, but not when clicking on the "Step" button during a pausePhase (also if the "Step" button acutally proceeded the game and one "Update"-cycle happened as if it were Playmode)
                frameCount_ofFrameInsidePausePhaseThatCanSafelyUseGizmoLines = Time.frameCount; //-> During pauses "Time.frameCount" remains at the value which it had in the frame before the pause. Though it increments during the pause phase if the "Step"-Button is clicked. The first frame of a pause phase can already safely use gizmo lines, because the final "Update()" before the pause has already finished and only after that "UnityEditor.EditorApplication.isPaused" became "true".
                frameCountAfterStepDuringPausePhase_inWhichTheGizmoCycleCounterStarted = Time.frameCount; //-> Setting "frameCountAfterStepDuringPausePhase_inWhichTheGizmoCycleCounterStarted" to a dismantled state here to prevent unwanted coincidences due to undefined value.
            }

#if DRAWXXLGIZMODEBUG
            Debug.Log("[Draw XXL] PAUSE state changed to: " + state + "     fCount: " + Time.frameCount + "     fCount_safelyG: " + frameCount_ofFrameInsidePausePhaseThatCanSafelyUseGizmoLines + "     fCount_dueToStep: " + frameCountAfterStepDuringPausePhase_inWhichTheGizmoCycleCounterStarted + "        gCount: " + UtilitiesDXXL_Components.virtualGizmoCycleCount + "    gCount_onStep: " + virtualGizmoCycleCount_inTheMomentOfLastClickOnPauseOrStepButton);
#endif

        }
#endif

        public static void TryDrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
        {
            //-> all draw operations of Draw XXL arrive here
            //-> from here the lines may be forwarded to a wrapper that displays lines based on other preferred technology (e.g. optimized meshes).

            if (globalAlphaFactor_is1 == false) { color.a = color.a * globalAlphaFactor; }

            if (drawingStopped_dueToTooManyDrawnLinesPerFrame == false)
            {
                switch (DrawBasics.usedUnityLineDrawingMethod)
                {
                    case DrawBasics.UsedUnityLineDrawingMethod.debugLinesInPlayMode_gizmoLinesInEditModeAndPlaymodePauses:
                        ChooseDebugOrGizmoLines_dependingOnPlayModeState(start, end, color, duration, depthTest);
                        break;
                    case DrawBasics.UsedUnityLineDrawingMethod.debugLines:
                        DrawLine_viaDebugDrawLine(start, end, color, duration, depthTest);
                        break;
                    case DrawBasics.UsedUnityLineDrawingMethod.gizmoLines:
                        DrawLine_viaGizmosDrawLine(start, end, color);
                        break;
                    case DrawBasics.UsedUnityLineDrawingMethod.handlesLines:
                        DrawLine_viaHandlesDrawLine(start, end, color);
                        break;
                    case DrawBasics.UsedUnityLineDrawingMethod.wireMesh:
                        AddLineToCacheForMesh(start, end, color, duration, depthTest);
                        break;
                    default:
                        //-> do not throw a log here, since it could happen poentially ten thousands of times per frame
                        //UtilitiesDXXL_Log.PrintErrorCode("**-" + DrawBasics.usedUnityLineDrawingMethod); //-> "DrawBasics.UsedUnityLineDrawingMethod.disabled" should already have been prevented earlier in "CheckIfDrawingIsCurrentlySkipped()"
                        break;
                }
                IncrementDrawnLinesPerFrameAndPreventEditorFreeze();
            }
        }

        static void ChooseDebugOrGizmoLines_dependingOnPlayModeState(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
        {
#if UNITY_EDITOR
            //General note: Below in the explanation "Update()" and "OnDrawGizmos()" are mentioned. This is only for simplicity so that the notes don't become long and unreadable. If "Update()" is mentioned, then also functions like "FixedUpdate()" are meant. If "OnDrawGizmos()" is mentioned then also "OnDrawGizmosSelected()" is meant.

            if (UnityEditor.EditorApplication.isPlaying)
            {
                if (UnityEditor.EditorApplication.isPaused)
                {
                    //-> The reason for this complicated construct with flags and if-trees: "UnityEditor.EditorApplication.isPaused" remains "true", if during a pause phase the "(Frame)Step"-functionality triggers a single Update()-cycle, and as a result the "ArgumentException: Gizmo drawing functions can only be used in OnDrawGizmos and OnDrawGizmosSelected."-error can happen. So "UnityEditor.EditorApplication.isPaused" is not sufficient to safely distinguish between playmode and pausePhases.
                    //-> it gets more undefinably because the "Step"-functionality behaves different, depending on how it is used:
                    //---> if the Button in the toolbar is used then everything behaves as described above
                    //---> if "UnityEditor.EditorApplication.Step()" is called from script (from inside "OnDrawGizmos()"), then the stacktrace says that "Update()" happens "inside" "OnDrawGizmos()". One side effect of this is that this "Update()" now CAN draw via "Gizmos.DrawLine()", without throwing the "ArgumentException: Gizmo drawing functions can only be used in OnDrawGizmos and OnDrawGizmosSelected."-error, supposedly because it IS now "inside" "OnDrawGizmos()".
                    //---> if "UnityEditor.EditorApplication.Step()" is called from script from other functions than "OnDrawGizmos()": Not tested. Anyway, where should it be called? The other Unity Event functions don't fire in pauses...

                    if (frameCount_ofFrameInsidePausePhaseThatCanSafelyUseGizmoLines == Time.frameCount)
                    {
                        //-> the pause phase hasn't seen a click on the "Step"-button for a sufficient time
                        //-> the lines cannot come from "Update()", since "Update()" isn't fired in pause phases
                        //---> so there is no danger of a "Gizmo lines only allowed from OnDrawGizmos" exception
                        //---> we can use gizmo lines instead of debug lines to enable changing the drawn shapes during pause phases

#if DRAWXXLGIZMODEBUG
                        Debug.Log("[Draw XXL] Wrapper-G1:   fCount: " + Time.frameCount + "     fCount_safelyG: " + frameCount_ofFrameInsidePausePhaseThatCanSafelyUseGizmoLines + "     fCount_dueToStep: " + frameCountAfterStepDuringPausePhase_inWhichTheGizmoCycleCounterStarted + "        gCount: " + UtilitiesDXXL_Components.virtualGizmoCycleCount + "    gCount_onStep: " + virtualGizmoCycleCount_inTheMomentOfLastClickOnPauseOrStepButton);
#endif

                        DrawLine_viaGizmosDrawLine(start, end, color);
                    }
                    else
                    {
                        //-> the "Step"-button has been clicked during a pause phase
                        if (frameCountAfterStepDuringPausePhase_inWhichTheGizmoCycleCounterStarted == Time.frameCount)
                        {
                            //-> at least one line has already been drawn after the "Step"-button has been clicked, and along with that first drawn line the gizmoCycleCounter has been started

                            if (UtilitiesDXXL_Components.virtualGizmoCycleCount == virtualGizmoCycleCount_inTheMomentOfLastClickOnPauseOrStepButton)
                            {
                                //-> more lines get drawn after the "Step"-button has been clicked, but the gizmoCycleCounter didn't progress yet
                                //-> we still have to use debug lines (as described in the [//-> this is the first drawn line after the "Step"-button has been clicked]-thread) since we still don't know if the "Update()", that has been triggerd by the "Step"-button is already finished (in other words: we still don't know if the line that should get drawn here came from "Update()" or from "OnDrawGizmos()")

#if DRAWXXLGIZMODEBUG
                                Debug.Log("[Draw XXL] Wrapper-D1:   fCount: " + Time.frameCount + "     fCount_safelyG: " + frameCount_ofFrameInsidePausePhaseThatCanSafelyUseGizmoLines + "     fCount_dueToStep: " + frameCountAfterStepDuringPausePhase_inWhichTheGizmoCycleCounterStarted + "        gCount: " + UtilitiesDXXL_Components.virtualGizmoCycleCount + "    gCount_onStep: " + virtualGizmoCycleCount_inTheMomentOfLastClickOnPauseOrStepButton);
#endif

                                DrawLine_viaDebugDrawLine(start, end, color, duration, depthTest);
                            }
                            else
                            {
                                //-> after the "Step"-button has been clicked time has passed and an "OnDrawGizmos()"-cycle has finished. Since "OnDrawGizmos()" is fired AFTER "Update()" we now can say, that the "Update()", that has been triggerd by the "Step"-button is finished. See also https://docs.unity3d.com/Manual/ExecutionOrder.html
                                //-> We are now safe to use gizmo lines again without the risk of the "Gizmo lines only allowed from OnDrawGizmos" exception. This enables being able to change the drawn lines/shapes during the pause phase.

#if DRAWXXLGIZMODEBUG
                                Debug.Log("[Draw XXL] Wrapper-G2:   fCount: " + Time.frameCount + "     fCount_safelyG: " + frameCount_ofFrameInsidePausePhaseThatCanSafelyUseGizmoLines + "     fCount_dueToStep: " + frameCountAfterStepDuringPausePhase_inWhichTheGizmoCycleCounterStarted + "        gCount: " + UtilitiesDXXL_Components.virtualGizmoCycleCount + "    gCount_onStep: " + virtualGizmoCycleCount_inTheMomentOfLastClickOnPauseOrStepButton);
#endif

                                //-> The frame that was triggered by the "Step"-button since is now considered to be finished. So the flag gets prepared here for a possible upcoming next click on the "Step"-button:
                                frameCount_ofFrameInsidePausePhaseThatCanSafelyUseGizmoLines = Time.frameCount;

                                DrawLine_viaGizmosDrawLine(start, end, color);
                            }
                        }
                        else
                        {
                            //-> this is the first drawn line after the "Step"-button has been clicked during a pause phase
                            //-> we cannot say if the line came from "Update()" or from "OnDrawGizmos()"
                            //---> if it came from "Update()":
                            //------> Using debug lines is the right thing. The user seems to debug his code on a frame by frame base and seems wanting to see which difference each progressed frame makes.
                            //---> if it came from "OnDrawGizmos()":
                            //------> The user may want to debug a gameobject and CHANGE that gameobject during the pause phase, and therefore uses "OnDrawGizmos()"
                            //------> Though we cannot safely use "OnDrawGizmos()" here, because we don't surely know if the lines came from "Update()" instead. This would throw the "Gizmo lines only allowed from OnDrawGizmos" exception.
                            //------> The here used debug line will not clear until the pause ends or until the next "Step"-button click. Though if the user wants to change the lines during the pause phase he can still see the change, because we can switch to gizmo lines in the next virtualGizmoCycle. The only difference is, that he will additionally see the debug lines/shapes (like frozen) as they were in the moment of clicking the "Step"-button. And the user can easily get rid of the "frozen" line by clicking on the "Step"-button. I'm not sure why this works, but it could indicate that "Debug.DrawLine()" internally inside Unity falls back to "Gizmo.DrawLine()", if it is called inside a "Step"-frame, meaning a frame that happens during a pause phase.

#if DRAWXXLGIZMODEBUG
                            Debug.Log("[Draw XXL] Wrapper-D2:   fCount: " + Time.frameCount + "     fCount_safelyG: " + frameCount_ofFrameInsidePausePhaseThatCanSafelyUseGizmoLines + "     fCount_dueToStep: " + frameCountAfterStepDuringPausePhase_inWhichTheGizmoCycleCounterStarted + "        gCount: " + UtilitiesDXXL_Components.virtualGizmoCycleCount + "    gCount_onStep: " + virtualGizmoCycleCount_inTheMomentOfLastClickOnPauseOrStepButton);

#endif

                            frameCountAfterStepDuringPausePhase_inWhichTheGizmoCycleCounterStarted = Time.frameCount;
                            virtualGizmoCycleCount_inTheMomentOfLastClickOnPauseOrStepButton = UtilitiesDXXL_Components.virtualGizmoCycleCount;

                            DrawLine_viaDebugDrawLine(start, end, color, duration, depthTest);
                        }
                    }
                }
                else
                {
                    //-> Unpaused Play mode always uses debug lines:
                    //-> It doesn't seem to harm if debug lines are drawn from inside "OnDrawGizmos()", but it does harm the other way round, if gizmo lines are drawn from "Update()", which throws the "Gizmo lines only allowed from OnDrawGizmos" exception.
                    //-> Moreover debug lines have more features, namely "duration" and "depthTest"
                    DrawLine_viaDebugDrawLine(start, end, color, duration, depthTest);
                }
            }
            else
            {
                //-> Edit mode always uses gizmo lines:
                //-> caller has to take care that he calls only from "OnDrawGizmos(Selected)"
                //-> the main Unity event functions ("Update", "FixedUpdate", ...) don't get called outside playmode (except for constructs like with the [ExecuteInEditMode] attribute), so there will be no "Gizmo lines only allowed from OnDrawGizmos" exceptions.
                DrawLine_viaGizmosDrawLine(start, end, color);
            }
#endif
        }

        static void DrawLine_viaDebugDrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
        {
            float used_duration = overwrite_durationInSec_globally ? globalOverwriteValueOf_durationInSec : duration;
            bool used_depthTest = overwrite_hiddenByNearerObjects_globally ? globalOverwriteValueOf_hiddenByNearerObjects : depthTest;
            Debug.DrawLine(start, end, color, used_duration, used_depthTest);
        }

        static void DrawLine_viaGizmosDrawLine(Vector3 start, Vector3 end, Color color)
        {
            Color gizmoColor_before = Gizmos.color;
            Gizmos.color = color;
            Gizmos.DrawLine(start, end);
            Gizmos.color = gizmoColor_before;
        }

        static void DrawLine_viaHandlesDrawLine(Vector3 start, Vector3 end, Color color)
        {
            //-> lines with non-0-width could possibly be drawn cheaper when using Handles by using the "thickness"-parameter in "Handles.DrawLine()"
            //-> but this thickness paramter isn't there in older Unity versions
            //-> Unity's code on Github for "Handles.DrawLine()" has a comment, that describes that thick lines sometimes don't work: "can happen when editor is actually using OpenGL ES 2 (no instancing)"

#if UNITY_EDITOR
            Color handlesColor_before = UnityEditor.Handles.color;
            UnityEditor.Handles.color = color;
            UnityEditor.Handles.DrawLine(start, end);
            UnityEditor.Handles.color = handlesColor_before;
#endif
        }

        static void AddLineToCacheForMesh(Vector3 start, Vector3 end, Color color, float duration, bool depthTest)
        {
            if (Application.isPlaying)
            {
                if (depthTest)
                {
                    DrawXXL_LinesManager.instance.lineStartAndEndPoints_asMeshVertices.Add(start);
                    DrawXXL_LinesManager.instance.lineStartAndEndPoints_asMeshVertices.Add(end);
                    DrawXXL_LinesManager.instance.colors_perMeshVertex.Add(color);
                    DrawXXL_LinesManager.instance.colors_perMeshVertex.Add(color);
                }
                else
                {
                    DrawXXL_LinesManager.instance.lineStartAndEndPoints_asMeshVertices_overlay.Add(start);
                    DrawXXL_LinesManager.instance.lineStartAndEndPoints_asMeshVertices_overlay.Add(end);
                    DrawXXL_LinesManager.instance.colors_perMeshVertex_overlay.Add(color);
                    DrawXXL_LinesManager.instance.colors_perMeshVertex_overlay.Add(color);
                }

                if (duration > 0.0f)
                {
                    //implementation of "duration" combined with "overlay" is not existing yet. "duration" already works, but if an overlay shader will be activated in "DrawXXL_LinesManager.RecreateMaterialsArrayToFitTheSubMeshes" the duration-using-lines may get assigned to the wrong shader.

                    //flip-flop-kindOfThing of "_version1" and "_version2" to prevent high number of expensive "list.Insert()" and "list.RemoveAt()":
                    float time_whenMeshLinesDelayedDisplayEnds = Time.time + duration;

                    if (DrawXXL_LinesManager.instance.activeDurationCacheVersion == 1)
                    {
                        DrawXXL_LinesManager.instance.lineStartAndEndPoints_asMeshVertices_delayed_version1.Add(start);
                        DrawXXL_LinesManager.instance.lineStartAndEndPoints_asMeshVertices_delayed_version1.Add(end);
                        DrawXXL_LinesManager.instance.colors_perMeshVertex_delayed_version1.Add(color);
                        DrawXXL_LinesManager.instance.colors_perMeshVertex_delayed_version1.Add(color); //-> the second "Add()" is only to keep the lists symetric to the "lineStartAndEndPoints_asMeshVertices_delayed_version*"-lists, but is actually not necessary
                        DrawXXL_LinesManager.instance.time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_version1.Add(time_whenMeshLinesDelayedDisplayEnds);
                        DrawXXL_LinesManager.instance.time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_version1.Add(time_whenMeshLinesDelayedDisplayEnds); //-> the second "Add()" is only to keep the lists symetric to the "lineStartAndEndPoints_asMeshVertices_delayed_version*"-lists, but is actually not necessary
                    }

                    if (DrawXXL_LinesManager.instance.activeDurationCacheVersion == 2)
                    {
                        DrawXXL_LinesManager.instance.lineStartAndEndPoints_asMeshVertices_delayed_version2.Add(start);
                        DrawXXL_LinesManager.instance.lineStartAndEndPoints_asMeshVertices_delayed_version2.Add(end);
                        DrawXXL_LinesManager.instance.colors_perMeshVertex_delayed_version2.Add(color);
                        DrawXXL_LinesManager.instance.colors_perMeshVertex_delayed_version2.Add(color); //-> the second "Add()" is only to keep the lists symetric to the "lineStartAndEndPoints_asMeshVertices_delayed_version*"-lists, but is actually not necessary
                        DrawXXL_LinesManager.instance.time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_version2.Add(time_whenMeshLinesDelayedDisplayEnds);
                        DrawXXL_LinesManager.instance.time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_version2.Add(time_whenMeshLinesDelayedDisplayEnds); //-> the second "Add()" is only to keep the lists symetric to the "lineStartAndEndPoints_asMeshVertices_delayed_version*"-lists, but is actually not necessary
                    }
                }
            }
            else
            {
                DrawLine_viaDebugDrawLine(start, end, color, duration, depthTest);
            }
        }

        public static bool CheckIfDrawingIsCurrentlySkipped()
        {
            if (DrawBasics.usedUnityLineDrawingMethod == DrawBasics.UsedUnityLineDrawingMethod.disabled)
            {
                return true;
            }
            else
            {
                if (globalAlphaFactor_is0)
                {
                    return true;
                }
                else
                {
#if UNITY_EDITOR
                    //-> when using only "usedUnityLineDrawingMethod == debugLines" then a gameobject instance would actually be not necessary. Though a distinction in code would potentially become complex for situations where the usedUnityLineDrawingMethod gets changed in all thinkable directions and at all thinkable times (also with and without domain reloads on enterPlaymode/reloadScripts)
                    DrawXXL_LinesManager.TryCreate();
#else
                    DrawBasics.usedUnityLineDrawingMethod = DrawBasics.UsedUnityLineDrawingMethod.wireMesh;
                    DrawXXL_LinesManager.TryCreate();
#endif
                    TryResetLinesPerFrameCounter(); //-> has only effect in the first call inside each FrameUpdate
                    return drawingStopped_dueToTooManyDrawnLinesPerFrame;
                }
            }
        }

        static Camera[] activeCamerasOfTheScene = null;
        static void IncrementDrawnLinesPerFrameAndPreventEditorFreeze()
        {
            drawnLinesSinceFrameStart++;
            drawnLinesSinceStart++;

            TryResetLinesPerFrameCounter();
            if (drawnLinesSinceFrameStart >= DrawBasics.MaxAllowedDrawnLinesPerFrame)
            {
                if (maxLinesPerFrameWarningTextDraw_hasAlreadyBeenStarted_duringCurrFrame == false)
                {
                    maxLinesPerFrameWarningTextDraw_hasAlreadyBeenStarted_duringCurrFrame = true;
                    try
                    {
                        if (activeCamerasOfTheScene == null) { activeCamerasOfTheScene = UnityEngine.Object.FindObjectsOfType<Camera>(); }
                        if (activeCamerasOfTheScene != null)
                        {
                            int numberOfCamerasThatGetTheWarningDrawing = Mathf.Min(activeCamerasOfTheScene.Length, 25);
                            for (int i = 0; i < numberOfCamerasThatGetTheWarningDrawing; i++)
                            {
                                if (activeCamerasOfTheScene[i] != null)
                                {
                                    if (activeCamerasOfTheScene[i].gameObject.activeInHierarchy)
                                    {
                                        if (activeCamerasOfTheScene[i].enabled)
                                        {
                                            if (i > 0)
                                            {
                                                //other cameras than first:
                                                switch (DrawBasics.maxLinesExceededNotificationOnScreenType)
                                                {
                                                    case DrawBasics.MaxLinesExceededNotificationOnScreenType.None:
                                                        break;

                                                    default:
                                                        UtilitiesDXXL_Text.WriteScreenspace(activeCamerasOfTheScene[i], "<color=#ed4731FF><icon=warning></color>", new Vector2(1.0f, 1.0f), default, 0.2f, 0.0f, DrawText.TextAnchorDXXL.UpperRight, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, true, 0.0f, false, 0.0f, false);
                                                        break;
                                                }
                                            }
                                            else
                                            {
                                                //first camera:
                                                switch (DrawBasics.maxLinesExceededNotificationOnScreenType)
                                                {
                                                    case DrawBasics.MaxLinesExceededNotificationOnScreenType.ExplanationText:
                                                        UtilitiesDXXL_Text.WriteScreenspace(activeCamerasOfTheScene[i], "<color=#ed4731FF><color=#e2aa00FF><icon=warning></color>Draw XXL skips drawing of some lines because the adjustable limit of maxLinesPerFrame (currently set to '" + DrawBasics.MaxAllowedDrawnLinesPerFrame + "') was exceeded.)</color>", new Vector2(1.0f, 1.0f), Color.red, 0.03f, 0.0f, DrawText.TextAnchorDXXL.UpperRight, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, true, 0.0f, false, 0.0f, false);
                                                        break;

                                                    case DrawBasics.MaxLinesExceededNotificationOnScreenType.WarningSymbol:
                                                        UtilitiesDXXL_Text.WriteScreenspace(activeCamerasOfTheScene[i], "<color=#ed4731FF><icon=warning></color>", new Vector2(1.0f, 1.0f), default, 0.2f, 0.0f, DrawText.TextAnchorDXXL.UpperRight, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, true, 0.0f, false, 0.0f, false);
                                                        break;

                                                    case DrawBasics.MaxLinesExceededNotificationOnScreenType.None:
                                                        break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch
                    {
                        UtilitiesDXXL_Log.PrintErrorCode("12");
                    }

                    if (currentlyDrawingChart != null) { currentlyDrawingChart.DrawWarningForMaxLinesPerFrame(); }
                    if (currentlyDrawingPieChart != null) { currentlyDrawingPieChart.DrawWarningForMaxLinesPerFrame(); }
                    maxLinesPerFrameWarningTextDraw_hasAlreadyBeenFinished_duringCurrFrame = true;

                    string message = (DrawBasics.maxLinesExceededNotificationInLogConsoleType == DrawBasics.MaxLinesExceededNotificationInLogConsoleType.None) ? null : GetErrorLogStringFor_maxLinesExceeded();
                    switch (DrawBasics.maxLinesExceededNotificationInLogConsoleType)
                    {
                        case DrawBasics.MaxLinesExceededNotificationInLogConsoleType.Log:
                            Debug.Log(message);
                            break;
                        case DrawBasics.MaxLinesExceededNotificationInLogConsoleType.Warning:
                            Debug.LogWarning(message);
                            break;
                        case DrawBasics.MaxLinesExceededNotificationInLogConsoleType.Error:
                            Debug.LogError(message);
                            break;
                        case DrawBasics.MaxLinesExceededNotificationInLogConsoleType.None:
                            break;
                    }
                }
                else
                {
                    if (maxLinesPerFrameWarningTextDraw_hasAlreadyBeenFinished_duringCurrFrame)
                    {
                        drawingStopped_dueToTooManyDrawnLinesPerFrame = true;
                    }
                }
            }
        }

        static string GetErrorLogStringFor_maxLinesExceeded()
        {
            string textWithoutGizmoManagerComponentSuffix = "Draw XXL skips drawing of some lines because the limit of maxLinesPerFrame (currently set to '" + DrawBasics.MaxAllowedDrawnLinesPerFrame + "') was exceeded. This is done to prevent accidentally freezing the editor by massive use of draw operations. You can increase this threshold via 'DrawXXL.DrawBasics.MaxAllowedDrawnLinesPerFrame' if your computer can handle more calculations, though at your own risk.";
            return textWithoutGizmoManagerComponentSuffix;
        }

        public static void TryResetLinesPerFrameCounter()
        {
            if (Time.frameCount != mostRecentFrameCount_forWhichLinesPerFrameHasBeenZeroed)
            {
                ResetLinesPerFrameCounter();
            }       
        }

        public static void ResetLinesPerFrameCounter()
        {
            mostRecentFrameCount_forWhichLinesPerFrameHasBeenZeroed = Time.frameCount;
            drawingStopped_dueToTooManyDrawnLinesPerFrame = false;
            drawnLinesSinceFrameStart = 0;
            maxLinesPerFrameWarningTextDraw_hasAlreadyBeenStarted_duringCurrFrame = false;
            maxLinesPerFrameWarningTextDraw_hasAlreadyBeenFinished_duringCurrFrame = false;

            if (DrawXXL_LinesManager.instance != null)
            {
                if (DrawXXL_LinesManager.instance.lineStartAndEndPoints_asMeshVertices != null)
                {
                    DrawXXL_LinesManager.instance.lineStartAndEndPoints_asMeshVertices.Clear();
                    DrawXXL_LinesManager.instance.lineStartAndEndPoints_asMeshVertices_overlay.Clear();
                    DrawXXL_LinesManager.instance.colors_perMeshVertex.Clear();
                    DrawXXL_LinesManager.instance.colors_perMeshVertex_overlay.Clear();

                    if (DrawBasics.usedUnityLineDrawingMethod == DrawBasics.UsedUnityLineDrawingMethod.wireMesh)
                    {
                        TryDrawEnduringLines();
                    }
                }
            }
           
        }

        static void TryDrawEnduringLines()
        {
            if (DrawXXL_LinesManager.instance.activeDurationCacheVersion == 1)
            {
                DrawEnduringLinesAndFlipLinesCache(ref DrawXXL_LinesManager.instance.lineStartAndEndPoints_asMeshVertices_delayed_version1, ref DrawXXL_LinesManager.instance.colors_perMeshVertex_delayed_version1, ref DrawXXL_LinesManager.instance.time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_version1, ref DrawXXL_LinesManager.instance.lineStartAndEndPoints_asMeshVertices_delayed_version2, ref DrawXXL_LinesManager.instance.colors_perMeshVertex_delayed_version2, ref DrawXXL_LinesManager.instance.time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_version2);
            }
            else
            {
                if (DrawXXL_LinesManager.instance.activeDurationCacheVersion == 2)
                {
                    DrawEnduringLinesAndFlipLinesCache(ref DrawXXL_LinesManager.instance.lineStartAndEndPoints_asMeshVertices_delayed_version2, ref DrawXXL_LinesManager.instance.colors_perMeshVertex_delayed_version2, ref DrawXXL_LinesManager.instance.time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_version2, ref DrawXXL_LinesManager.instance.lineStartAndEndPoints_asMeshVertices_delayed_version1, ref DrawXXL_LinesManager.instance.colors_perMeshVertex_delayed_version1, ref DrawXXL_LinesManager.instance.time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_version1);
                }
            }
        }

        static void DrawEnduringLinesAndFlipLinesCache(ref List<Vector3> lineStartAndEndPoints_asMeshVertices_delayed_activeVersionUntilNow,ref List<Color> colors_perMeshVertex_delayed_activeVersionUntilNow, ref List<float> time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_activeVersionUntilNow, ref List<Vector3> lineStartAndEndPoints_asMeshVertices_delayed_activeVersionFromNowOn, ref List<Color> colors_perMeshVertex_delayed_activeVersionFromNowOn, ref List<float> time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_activeVersionFromNowOn)
        {
            //This uses a flip-flop kind of thing of "_version1" and "_version2" to prevent high numbers of expensive "list.Insert()" and "list.RemoveAt()", which would be there for only one delayed-lines cache list
            
            //Clear the upcoming lists, so they only contain the lines from the untilNowActive-list that still persist after the time check of the untilNowActive-lists:
            lineStartAndEndPoints_asMeshVertices_delayed_activeVersionFromNowOn.Clear();
            colors_perMeshVertex_delayed_activeVersionFromNowOn.Clear();
            time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_activeVersionFromNowOn.Clear();

            float currentTime = Time.time;
            for (int i_currentlyTreatedLineStartSlot = 0; i_currentlyTreatedLineStartSlot < time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_activeVersionUntilNow.Count; i_currentlyTreatedLineStartSlot++)
            {
                float ceasingTimeOfCurrentlyTreatedLine = time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_activeVersionUntilNow[i_currentlyTreatedLineStartSlot];
                if (currentTime <= ceasingTimeOfCurrentlyTreatedLine)
                {
                    Vector3 startPos_ofStillEnduringLine = lineStartAndEndPoints_asMeshVertices_delayed_activeVersionUntilNow[i_currentlyTreatedLineStartSlot];
                    Vector3 endPos_ofStillEnduringLine = lineStartAndEndPoints_asMeshVertices_delayed_activeVersionUntilNow[i_currentlyTreatedLineStartSlot + 1];
                    Color color_ofStillEnduringLine = colors_perMeshVertex_delayed_activeVersionUntilNow[i_currentlyTreatedLineStartSlot];

                    //This line from a previous frame still exists and therefore will be drawn now right at the beginning of this frame, so its existence persist for now:
                    float durationInSec = 0.0f; //-> since it lives on in the "time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_version*"-lists this redrawing will be done without another "durationInSec"-sheduling
                    bool hiddenByNearerObjects = true; //-> "hiddenByNearerObjects" is not finally implemented for "usedUnityLineDrawingMethod = mesh", therefore the default value is used here.
                    UtilitiesDXXL_DrawBasics.Line(startPos_ofStillEnduringLine, endPos_ofStillEnduringLine, color_ofStillEnduringLine, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f, 1.0f);

                    //The line still exists and another ceasedExistence-check will be done with the other version of "time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_version*" in the next frame.
                    lineStartAndEndPoints_asMeshVertices_delayed_activeVersionFromNowOn.Add(startPos_ofStillEnduringLine);
                    lineStartAndEndPoints_asMeshVertices_delayed_activeVersionFromNowOn.Add(endPos_ofStillEnduringLine);
                    colors_perMeshVertex_delayed_activeVersionFromNowOn.Add(color_ofStillEnduringLine);
                    colors_perMeshVertex_delayed_activeVersionFromNowOn.Add(color_ofStillEnduringLine);
                    time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_activeVersionFromNowOn.Add(ceasingTimeOfCurrentlyTreatedLine);
                    time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_activeVersionFromNowOn.Add(ceasingTimeOfCurrentlyTreatedLine);
                }
                else
                {
                    //-> The time of this line has run up
                    //-> It will not be drawn in this frame
                    //-> It will not be transferred to the next frame, but cease its existence with the list.Clear() below.
                }

                i_currentlyTreatedLineStartSlot++; //-> additional proceed, because: each line consists of two slots in the cache lists (one for "line start" and one for "line end")
            }

            if (DrawXXL_LinesManager.instance.activeDurationCacheVersion == 1)
            {
                DrawXXL_LinesManager.instance.activeDurationCacheVersion = 2;
            }
            else
            {
                if (DrawXXL_LinesManager.instance.activeDurationCacheVersion == 2)
                {
                    DrawXXL_LinesManager.instance.activeDurationCacheVersion = 1;
                }
            }
        }

        static bool overwrite_durationInSec_globally = false;
        static float globalOverwriteValueOf_durationInSec = 0.0f;
        public static void ToggleGlobalOverwriteFor_durationInSec(bool globalOverwriteIsEnabled, float valueOf_durationInSec_thatShouldAlwaysBeEnforced = 0.0f)
        {
            overwrite_durationInSec_globally = globalOverwriteIsEnabled;
            globalOverwriteValueOf_durationInSec = valueOf_durationInSec_thatShouldAlwaysBeEnforced;
        }

        static bool overwrite_hiddenByNearerObjects_globally = false;
        static bool globalOverwriteValueOf_hiddenByNearerObjects = true;
        public static void ToggleGlobalOverwriteFor_hiddenByNearerObjects(bool globalOverwriteIsEnabled, bool valueOf_hiddenByNearerObjects_thatShouldAlwaysBeEnforced = true)
        {
            overwrite_hiddenByNearerObjects_globally = globalOverwriteIsEnabled;
            globalOverwriteValueOf_hiddenByNearerObjects = valueOf_hiddenByNearerObjects_thatShouldAlwaysBeEnforced;
        }

    }

}
