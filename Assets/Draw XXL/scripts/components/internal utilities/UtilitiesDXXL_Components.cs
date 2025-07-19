namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class UtilitiesDXXL_Components
    {
        public static int frameCount_forWhichAnEditorFrameStep_hasBeenSheduled_byAChart = -10; //this could also be a member of the chart components itself, but then multiple frame steps will be executed onPause if multiple charts exist in the scene.
        public static int frameCount_forWhichAnEditorFrameStep_hasBeenSheduled_byADrawerComponent = -10; //this could also be a member of the drawer components itself, but then multiple frame steps will be executed onPause if multiple drawer components exist in the scene.

        public static void TryProceedOneSheduledFrameStep()
        {
#if UNITY_EDITOR
            //sidenote:
            //-> it seems that "EditorApplication.Step()" cannot be called from inside "OnDrawGizmos". Unity prints several errors surrounding the problem "recursive OnGUI rendering".
            //-> it works without errors when called from a "EditorApplication.update"-delegate
            //-> multiple components that call this function still result in only one frame step, because after "UnityEditor.EditorApplication.Step()" the "Time.frameCount" is not the same any more.

            if (UnityEditor.EditorApplication.isPlaying && UnityEditor.EditorApplication.isPaused)
            {
                bool hasAlreadyStepped_dueToTheFlagConcerningCharts = false;

                if (DrawCharts.chartInspectorComponentsAutomaticallyProceedOneFrameStepOnPauseStarts_toPreventFrozenOverlayDraw)
                {
                    if (frameCount_forWhichAnEditorFrameStep_hasBeenSheduled_byAChart == Time.frameCount)
                    {
                        hasAlreadyStepped_dueToTheFlagConcerningCharts = true;
                        UnityEditor.EditorApplication.Step();
                    }
                }

                if (DrawBasics.drawerComponentsAutomaticallyProceedOneFrameStepOnPauseStarts_toPreventFrozenOverlayDraw)
                {
                    if (hasAlreadyStepped_dueToTheFlagConcerningCharts == false)
                    {
                        if (frameCount_forWhichAnEditorFrameStep_hasBeenSheduled_byADrawerComponent == Time.frameCount)
                        {
                            UnityEditor.EditorApplication.Step();
                        }
                    }
                }
            }
#endif
        }

        public static float Get_inspector_singleLineHeightInclSpacingBetweenLines()
        {
#if UNITY_EDITOR
            return (UnityEditor.EditorGUIUtility.singleLineHeight + UnityEditor.EditorGUIUtility.standardVerticalSpacing);
#else
            return 18.0f;
#endif
        }

        static List<int> id_ofMonobehavioursThatDrawViaOnDrawGizmos = new List<int>();
#if UNITY_EDITOR
        static int highestUsed_i_inMonoBList_sinceLastDrawnLinesCounterReset = -1;
#endif
        public static bool currentVirtualOnDrawGizmoCycle_shouldRepaintAllViews = false;
        public static int virtualGizmoCycleCount;

        public static void ReportOnDrawGizmosCycleOfAMonoBehaviour(int id_ofReportingMonoB)
        {
#if UNITY_EDITOR

            ///Problem:
            //The maxLinesPerFrame-limit to prevent editor freezing during playmode depends on "Time.frameCount"
            //outside playmode or inside playmodePauses "Time.frameCount" is not reliable

            //These are the currently known options for an maxLinesPerFrame-limit outside (nonPaused)playmode:

            ///Using "Time.frameCount"
            //-> it is not incremented in pause phases
            //-> it is sometimes incremented in edit mode, and sometimes not. Is not reliable and not documented by Unity for edit mode.

            ///Using "Time.renderedFrameCount"
            //-> according to forum posts this is incremented in pause phases. I couldn't reproduce this: It didn't increment during pauses in my case
            //-> it is not documented by Unity and could change it's behaviour without notice, or even disappear completely

            ///Using "EditorApplication.timeSinceStartup"
            //-> as "Time.realtimeSinceStartup" it is dependent from the platform and may sometimes not update regularly.
            //-> doesn't help anyway, because it measures "time", but doesn't say anything about when a new OnDrawGizmos-cycle starts. E.g. allowing a limitedNumberOfLines(L) inside a timespan(x) doesn't prevent Editor freeze, because the drawnLines itself can raise the OnDrawGizmos-executionTime, and then after the timespan(x) has passed (but still inside the same OnDrawGizmo) a new set of limitedNumberOfLines(L) is allowed to be drawn, raising the execution time of the current OnDrawGizmo once again. This can endlessly repeat in the same OnDrawGizmos and freeze the Editor.

            ///Using a registered callback from "EditorApplication.update"
            //-> EditorApplication.update has no guaranteed frequency and can (according to Unitys documentation) be called multiple times per frame update. 
            //-> It would still need a managing object that oversees all drawing MonoBehaviours (like this class here)
            //-> Cannot be used for static-only drawing, but needs a MonoBehaviour in the scene. Or doesn't it? -> https://docs.unity3d.com/Manual/RunningEditorCodeOnLaunch.html

            ///Each drawing MonoBehaviour resets the counter after his own draw operation
            //-> is at least SOME protection, because
            //---> it will work if a single drawnObject exceeds the limit by itself
            //---> it will not work if multiple drawnObject exceeds the limit with their cumulated lines
            //---> though in most cases new drawingBehaviours will get added "gradually", so the editor slow down is also gradually and the user will notice that something is going wrong, so he can at least react. That may be acceptable, because the maxLines-limit is not for preventing slowdown, but for preventing freeze that forces the user to forceQuit the whole Unity application.
            //It is similar in the special case of Charts:
            //-> "OnDrawGizmos()" gets called for every existing "DrawXXLChartInspector"-gameObject in the scene, so the maxAllowedLinesPerFrame get summed up and become "maxLinesPerFrameLimit * chartsThatAreCurrentlyInspectedViaComponent"
            //-> In many cases this should be no problem, because if a user calls "CreateChartInspectionGameobject(true)" for multiple charts at once then he has drawn them before the pause already all inside "Update()" (where the maxLinesPerFrame-limit works correctly), so the maxLinesPerFrame-limit would have spoken already.
            //-> The loophole is this:
            //---> The maxLinesLimit has not spoken before the pause because only a small section of the lines were displayed. Then during pause the user zooms out to display more.
            //---> But there is still a security:
            //-----> 1) If the user raises the linesPerChart very fast for 1 chart: Then the maxLinesLimit kicks in for this chart and prevents performance freezing.
            //-----> 2) If he raises the linesPerChart for one chart after another to slighly below the maxLinesPerFrame limit, then it's a "gradual performance slowdown". He will realize it and stop the process before the whole system freezes.

            ///Central MonoBehaviour-OnDrawGizmosCalls-managingClass [= implemented solution below]
            //-> the linesCounter gets reset as soon as any drawing MonoBehaviour comes back for a second draw (each MonoBehaviour reports only once inside its OnDrawGizmos. Custom MonoBehaviours from users don't participate in this, but are represented by the "DrawXXL_LinesManager"-singleton). In the meantime other drawing MonoBehaviour may have drawn. This is in most cases one OnDrawGizmo-cycle
            //-> the execution order of different MonoBehaviour types stays the same according to https://docs.unity3d.com/Manual/class-MonoManager.html
            //-> if there is still a mixup of the order of MonoBehaviours then the introduced error is not big, assumed to be not more than factor 2. This is still ok in respect of the purpose of the whole maxLines-mechanic, which is not preventing "editor-slowdown" (which is annoying but can be handled), but preventing "editor-freeze" (which may force the user to force-quit the whole Unity application).
            //-> using only the DrawXXL_LinesManager may be not sufficient, since its "OnDrawGizmos()" event is not guaranteed to be fired always. (sidenote: It may be guaranteed due to "ExpandComponentInInspector")


            for (int i = 0; i <= highestUsed_i_inMonoBList_sinceLastDrawnLinesCounterReset; i++)
            {
                if (id_ofMonobehavioursThatDrawViaOnDrawGizmos[i] == id_ofReportingMonoB)
                {
                    highestUsed_i_inMonoBList_sinceLastDrawnLinesCounterReset = -1;
                    InitializeNewVirtualOnDrawGizmoCycle();
                    break;
                }
            }

            highestUsed_i_inMonoBList_sinceLastDrawnLinesCounterReset++;
            if (id_ofMonobehavioursThatDrawViaOnDrawGizmos.Count <= highestUsed_i_inMonoBList_sinceLastDrawnLinesCounterReset)
            {
                id_ofMonobehavioursThatDrawViaOnDrawGizmos.Add(id_ofReportingMonoB);
            }
            else
            {
                id_ofMonobehavioursThatDrawViaOnDrawGizmos[highestUsed_i_inMonoBList_sinceLastDrawnLinesCounterReset] = id_ofReportingMonoB;
            }
#endif
        }

        static void InitializeNewVirtualOnDrawGizmoCycle()
        {
            TryRepaintAllViews(); //this is called BEFORE "ResetLinesPerFrameCounter()", because it needs an uncleared "DrawnLinesSinceFrameStart" value
            DXXLWrapperForUntiysBuildInDrawLines.ResetLinesPerFrameCounter();
            virtualGizmoCycleCount++;
            if (Application.isPlaying == false) { virtualGizmoCycleCount = 0; } //-> not used outside playmode
        }

        static void TryRepaintAllViews()
        {
#if UNITY_EDITOR
            //-> The scene view and game view windows are continuously repainted during playmode
            //-> Though during edit mode or game pauses they are not 
            //-> The drawn shapes should continuously be updated on the screen, also outside playmode: Think of a "BoolDisplayer". It should always show the most current value. Also any simple line can change its position and should appear with its current shape.
            //-> It is expensive, but doing it inside "InitializeNewVirtualOnDrawGizmoCycle()" at least ensures that it is called only once per OnDrawGizmo-cycle.
            //-> And "currentVirtualOnDrawGizmoCycle_shouldRepaintAllViews" prevents some more repaints, mostly by detecting if the drawn lines came from user code calls and not by drawer components. Drawer components anyway automatically repaint the views, but only if their drawn shape as changed/one of their inspector values has changed.

            if (currentVirtualOnDrawGizmoCycle_shouldRepaintAllViews)
            {
                if ((UnityEditor.EditorApplication.isPlaying == false) || UnityEditor.EditorApplication.isPaused)
                {
                    if (DXXLWrapperForUntiysBuildInDrawLines.DrawnLinesSinceFrameStart > 0)
                    {
                        UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
                    }
                }
            }
            currentVirtualOnDrawGizmoCycle_shouldRepaintAllViews = false;
#endif
        }

        public static void ExpandComponentInInspector(MonoBehaviour componentToExpand)
        {
            //Reference: https://answers.unity.com/questions/801692/query-whether-inspector-is-folded.html
            //-> The problem is that "OnDrawGizmos()" (and along with it all Gizmo lines) is only fired when the component is expanded in the inspector. The gameobject doesn't have to be selected, but the component has to be expanded (that means you have to "leave" the selected gameobject (via selecting an other gameobject) also in a moment where the component is expanded).
            //-> This disturbs drawing in edit mode. The components already have toggles for "enable/disable" and "draw only if selected", but on top of this the user has to be aware of this "component has to be expanded"-requirement, and may be irritated why his drawer component stops working sometimes.
            //-> It gets more irritating for the user if the DrawXXL_LinesManager component is collapsed due to whatever reason, because then the user isn't event working with components but only with static code calls.
            //-> The reference link above describes two approaches how to force a component to "expanded": One is via "UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded()", the other is more complicated via reflection.
            //-> Luckily it turns out that: The "UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded()" function doesn't work for it's "actual task", that means it doesn't expand the components, BUT: It seems to do something in the background, which solves the actual problem here: Despite the components expanded state seeming to be not affected in the visual appearance of the inspector, the "OnDrawGizmos()"-function DOES think that it is expanded and does fire.
            //-> This is even better than forcing the visual appearance in the inspector, because now the ability for the user to collapse or expand the component is preserved.
            //-> There seem to be situations, where this "hidden" expanded state does transform to be "really expanding the components also visually". One such situation is when a new additional component (of any (other) type) is added to the gameobject. This may be a downside of the solution: It could annoy the user that he has to manually collapse the components over and over again, when he wants them to not take display space in the inspector.
            //---> One such situation is when a new additional component (of any (other) type) is added to the gameobject
            //---> Also when a component is removed
            //---> Restarting the Unity Editor (which by the way means, that this "hidden expanded state" is preserved even when the Unity Editor is closed)
            //-> This "hidden expanded" state seems also not to get overwritten when the component is collapsed via mouse click on the small triangle symbol in the top left corner of the component inspector window. Or else it could be that it does get overwritten in this case, but "OnDrawGizmos" may get called one last time and rescues the expanded state by once again calling "ExpandComponentInInspector()"
            //-> Though some uncertainty remains if this solution will always work, also in future versions of Unity, because it's an undocumented feaure.

#if UNITY_EDITOR
            UnityEditorInternal.InternalEditorUtility.SetIsInspectorExpanded(componentToExpand, true);
#endif
        }

    }

}
