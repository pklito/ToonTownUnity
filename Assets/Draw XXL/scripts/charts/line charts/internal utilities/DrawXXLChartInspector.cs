namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Internal Not For Manual Creation/Draw XXL Chart Inspector")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    [ExecuteInEditMode]
    public class DrawXXLChartInspector : MonoBehaviour
    {
        public bool hasBeenManuallyCreated = true;
        public bool hasBeenCreatedOutsidePlaymode;
        public ChartDrawing chart_thisInspectorIsAttachedTo;
        [SerializeField] InternalDXXL_LineSpecsForChartInspector[] specsForInspector_forEachDrawnLine = new InternalDXXL_LineSpecsForChartInspector[0];
        public bool theChartIsDrawnInScreenspace;
        public Camera screenSpaceTargetCamera;
        public bool chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight;
        public bool nonScreenspaceDrawing_happensWith_drawConfigOf_hiddenByNearerObjects;
#if UNITY_EDITOR
        [SerializeField] bool forceSceneViewCamToFollowChart = false;
#endif
        static float default_sizeArbUnits_ofSceneViewCam = 0.6f;
        [SerializeField] [Range(0.0f, 1.0f)] float sizeArbUnits_ofSceneViewCam = default_sizeArbUnits_ofSceneViewCam;

        [SerializeField] public bool hideHandles_andInsteadUseInspectorSlidersForZoomAndScroll = false;
        [SerializeField] public bool sceneViewCamSection_isExpanded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool cursorSection_isExpanded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool axesSection_isExpanded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool otherSection_isExpanded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool csvExportSection_isExpanded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool clearButtonSection_isExpanded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.

        public float curr_cursorPosition_as0to1OfChartWidth;
        bool cursorPos_hasBeenCalcedAtLeastOnce = false;
        [SerializeField] float curr_cursorPosition_inChartspaceUnits;
        float prev_cursorPosition_inChartspaceUnits;
        [SerializeField] [Range(0.0f, 1.0f)] public float cursorPos0to1_raw = 0.5f;
        [SerializeField] [Range(-0.025f, 0.025f)] public float cursorPos0to1_finetune = 0.0f;
        [SerializeField] [Range(-0.001f, 0.001f)] public float cursorPos0to1_superFinetune = 0.0f;

        [SerializeField] [Range(0.0f, 1.0f)] float sizeOfDatapointVisualization = 0.3f;
        [SerializeField] public bool alwaysEncapsulateAllValues = false;

        float xAxisLowEnd_inMomentOfComponentCreation;
        float xAxisUpperEnd_inMomentOfComponentCreation;
        float yAxisLowEnd_inMomentOfComponentCreation;
        float yAxisUpperEnd_inMomentOfComponentCreation;

        [Range(0.001f, 1.0f)] public float curr_luminanceOfLineColors_accordingToSlider; //"0.001f" instead of "0.0f": The luminance jumps to "0.5" if the slider is at "0.0f", maybe due to a rounding error in "Mathf.Clamp01"
        public float prev_luminanceOfLineColors_accordingToSlider;
        public float prev_luminanceOfLineColors_accordingToChartSetting;
        [SerializeField] string csvFileName = "" + InternalDXXL_ChartToCSVfileWriter.default_csvFileName;
        public ChartLine.NamePosition lineNamePositions;
        [Range(0.0f, 12.0f)] public float lineNames_sizeScaleFactor;
        [SerializeField] int drawnSmallStraightLines_duringLastDrawRun;
        bool isFirstUpdateCycleAfterGamePause = false;
#if UNITY_EDITOR
        int mostCurrentFrameCountDuringGamePause = -10;
        bool editorUpdateCallback_hasBeenRegistered = false;
#endif

        void Start()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update += EditorUpdateCallback;
            editorUpdateCallback_hasBeenRegistered = true;
            mostCurrentFrameCountDuringGamePause = -10;
#endif
        }

        void OnDestroy()
        {
#if UNITY_EDITOR
            if (editorUpdateCallback_hasBeenRegistered)
            {
                UnityEditor.EditorApplication.update -= EditorUpdateCallback;
                editorUpdateCallback_hasBeenRegistered = false;
            }
            TrySetChartsNonScreenspacePosToValues_fromBeforeComponentInspectionPhase_caseComponentInspectionPhaseDrewToScreenspace();
#endif
        }

        void EditorUpdateCallback()
        {
#if UNITY_EDITOR

            if ((UnityEditor.EditorApplication.isPlaying == false) || UnityEditor.EditorApplication.isPaused)
            {
                if (GUIUtility.hotControl != 0) //mouse button is held down on a handle
                {
                    //this ensures that "OnDrawGizmos" is called at fluent regular intervals also during pauseState of the game.
                    //Without this the inspector sliders for "zoom" and "scroll" would be jittering: 
                    //During pauseGame the "OnDrawGizmos" function is only called if something in the component changes, e.g. a slider position.
                    //The sliders for "zoom" and "scroll" have in some situations effect on the axis scaling also if they don't change, namely when the user drags them with their mouse to the side and then holds them at this sidePosition via keepMousePressed.
                    //Since in these situations "nothing in the editor changes" (the mouse position stays the same, and the mouseClickState stays the  same) nothing would trigger "OnDrawGizmos", but in "OnDrawGizmos" happens the actual updating of the axis scaling.
                    //An alternative would be to make this "SetDirty" in "OnInspectorGUI" but then it's still jittery, because "OnInspectorGUI" is obviously called far less often than "EditorApplication.update", and also less often than the normal Update-Cylce during gameRunsPhases.
                    UnityEditor.EditorUtility.SetDirty(this);
                }
            }

            UtilitiesDXXL_Components.TryProceedOneSheduledFrameStep();
#endif
        }

        Vector3 nonScreenspaceChartPosition_beforeComponentInspectionPhaseThatDrawsToScreenspace;
        Quaternion nonScreenspaceChartFixedRotation_beforeComponentInspectionPhaseThatDrawsToScreenspace;
        float nonScreenspaceChartWidthWorldspace_beforeComponentInspectionPhaseThatDrawsToScreenspace;
        float nonScreenspaceChartHeigthWorldspace_beforeComponentInspectionPhaseThatDrawsToScreenspace;
        public void AssignChart(ChartDrawing chart_thisInspectorShouldBeAttachedTo)
        {
            chart_thisInspectorIsAttachedTo = chart_thisInspectorShouldBeAttachedTo;
            FetchAxisScaleInMomentOfComponentCreation();

            if (theChartIsDrawnInScreenspace)
            {
                nonScreenspaceChartPosition_beforeComponentInspectionPhaseThatDrawsToScreenspace = chart_thisInspectorIsAttachedTo.Position_worldspace;
                nonScreenspaceChartFixedRotation_beforeComponentInspectionPhaseThatDrawsToScreenspace = chart_thisInspectorIsAttachedTo.fixedRotation;
                nonScreenspaceChartWidthWorldspace_beforeComponentInspectionPhaseThatDrawsToScreenspace = chart_thisInspectorIsAttachedTo.Width_inWorldSpace;
                nonScreenspaceChartHeigthWorldspace_beforeComponentInspectionPhaseThatDrawsToScreenspace = chart_thisInspectorIsAttachedTo.Height_inWorldSpace;

                if (screenSpaceTargetCamera == null)
                {
                    Debug.LogError("Chart: CreateChartInspectionGameobject errorneous, because targetCamera is null.");
                }
            }
        }

        void TrySetChartsNonScreenspacePosToValues_fromBeforeComponentInspectionPhase_caseComponentInspectionPhaseDrewToScreenspace()
        {
            if (theChartIsDrawnInScreenspace)
            {
                if (chart_thisInspectorIsAttachedTo != null)
                {
                    chart_thisInspectorIsAttachedTo.Position_worldspace = nonScreenspaceChartPosition_beforeComponentInspectionPhaseThatDrawsToScreenspace;
                    chart_thisInspectorIsAttachedTo.fixedRotation = nonScreenspaceChartFixedRotation_beforeComponentInspectionPhaseThatDrawsToScreenspace;
                    chart_thisInspectorIsAttachedTo.Width_inWorldSpace = nonScreenspaceChartWidthWorldspace_beforeComponentInspectionPhaseThatDrawsToScreenspace;
                    chart_thisInspectorIsAttachedTo.Height_inWorldSpace = nonScreenspaceChartHeigthWorldspace_beforeComponentInspectionPhaseThatDrawsToScreenspace;
                }
            }
        }

        void FetchAxisScaleInMomentOfComponentCreation()
        {
            if (UtilitiesDXXL_Math.FloatIsValid(chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingLowerEndOfTheAxis))
            {
                xAxisLowEnd_inMomentOfComponentCreation = chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingLowerEndOfTheAxis;
            }
            else
            {
                xAxisLowEnd_inMomentOfComponentCreation = -100.0f;
            }

            if (UtilitiesDXXL_Math.FloatIsValid(chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingUpperEndOfTheAxis))
            {
                xAxisUpperEnd_inMomentOfComponentCreation = chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingUpperEndOfTheAxis;
            }
            else
            {
                xAxisUpperEnd_inMomentOfComponentCreation = 100.0f;
            }

            if (UtilitiesDXXL_Math.FloatIsValid(chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingLowerEndOfTheAxis))
            {
                yAxisLowEnd_inMomentOfComponentCreation = chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingLowerEndOfTheAxis;
            }
            else
            {
                yAxisLowEnd_inMomentOfComponentCreation = -100.0f;
            }

            if (UtilitiesDXXL_Math.FloatIsValid(chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingUpperEndOfTheAxis))
            {
                yAxisUpperEnd_inMomentOfComponentCreation = chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingUpperEndOfTheAxis;
            }
            else
            {
                yAxisUpperEnd_inMomentOfComponentCreation = 100.0f;
            }
        }

        void LateUpdate()
        {
#if UNITY_EDITOR
            if (TryDestroyThisComponentIfItWasManuallyCreated()) { return; }
            if (TryDestroyComonent_ifChartGotLost()) { return; }

            if (this.enabled)
            {
                if (UnityEditor.EditorApplication.isPlaying && (UnityEditor.EditorApplication.isPaused == false))
                {
                    if (TrySkipDrawingBecauseItIsTheFirstFrameAfterAPausePhase()) { return; }
                    if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

                    UtilitiesDXXL_Components.ExpandComponentInInspector(this);

                    UtilitiesDXXL_DrawBasics.Set_usedLineDrawingMethod_reversible(DrawBasics.UsedUnityLineDrawingMethod.debugLines);
                    DrawChartAndCursor();
                    UtilitiesDXXL_DrawBasics.Reverse_usedLineDrawingMethod();
                }
            }
#endif
        }

        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            if (TryDestroyThisComponentIfItWasManuallyCreated()) { return; }
            if (TryDestroyComonent_ifChartGotLost()) { return; }

            if (this.enabled)
            {
                if ((UnityEditor.EditorApplication.isPlaying == false) || UnityEditor.EditorApplication.isPaused)
                {
                    UtilitiesDXXL_Components.currentVirtualOnDrawGizmoCycle_shouldRepaintAllViews = true;
                    UtilitiesDXXL_Components.ReportOnDrawGizmosCycleOfAMonoBehaviour(this.GetInstanceID());

                    if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

                    UtilitiesDXXL_Components.ExpandComponentInInspector(this);

                    UtilitiesDXXL_DrawBasics.Set_usedLineDrawingMethod_reversible(DrawBasics.UsedUnityLineDrawingMethod.gizmoLines); //-> "debugLinesInPlayMode_gizmoLinesInEditModeAndPlaymodePauses" is not an option here, because "DXXLWrapperForUntiysBuildInDrawLines.ChooseDebugOrGizmoLines_dependingOnPlayModeState()" forces to debug lines in some cases even if "pause == true"
                    UtilitiesDXXL_DrawBasics.Set_gizmoMatrix_reversible(Matrix4x4.identity);
                    DrawChartAndCursor();
                    UtilitiesDXXL_DrawBasics.Reverse_gizmoMatrix();
                    UtilitiesDXXL_DrawBasics.Reverse_usedLineDrawingMethod();
                }
            }

            TrySheduleAutomaticFrameStepAtTheStartOfPausePhases();
#endif
        }

        void DrawChartAndCursor()
        {
            long drawnLines_beforeCurrDrawRun = DXXLWrapperForUntiysBuildInDrawLines.DrawnLinesSinceStart; //using "DXXLWrapperForUntiyDebugDraw.DrawnLinesSinceStart" instead of "DXXLWrapperForUntiyDebugDraw.DrawnLinesSinceFrameStart" because if this is the only drawn thing inside the frame, then the following "DrawChart()" resets the "DrawnLinesSinceFrameStart"-counter (because it's the first drawn thing since "Time.frameCount" was incremented), so the herewith obtained "lineCounter_beforeDrawing" is not accurate anymore.
            TrySetLuminanceOfLineColors();
            SetLineNamePositionsAndSize();
            RecalcCursorPosition();
            ScaleAxes(); //-> this can also call "RecalcCursorPosition()" again

            DrawTheChart();

            TryNote_chartWorldSpacePosRotScale_beforeChangingItToScreenspace();
            try
            {
                if (theChartIsDrawnInScreenspace) { UtilitiesDXXL_ChartDrawing.SetPosRotScaleOfChart_toScreenspace(chart_thisInspectorIsAttachedTo, screenSpaceTargetCamera, chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight, true); }
                DrawCursor();
                TryDrawUnified45degAxisForHandles();
                SetTransformToChartPos();
                TryPlaceSceneviewCam();
            }
            catch { }
            TrySetBack_chartWorldSpacePosRotScale_afterUsingItInScreenspace();

            RefillArraysWithCursorNeighboringDatapointsForEachLine();

            drawnSmallStraightLines_duringLastDrawRun = (int)(DXXLWrapperForUntiysBuildInDrawLines.DrawnLinesSinceStart - drawnLines_beforeCurrDrawRun);
        }

        void RecalcCursorPosition()
        {
            if (cursorPos_hasBeenCalcedAtLeastOnce)
            {
                if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(prev_cursorPosition_inChartspaceUnits, curr_cursorPosition_inChartspaceUnits) == false)
                {
                    //cursor pos has been set without slider, but via float number field:
                    ForceCursorSliders_toFitGivenChartspaceXPos(curr_cursorPosition_inChartspaceUnits);
                }
            }

            if (UtilitiesDXXL_Math.FloatIsInvalid(cursorPos0to1_raw)) { cursorPos0to1_raw = 0.5f; }
            if (UtilitiesDXXL_Math.FloatIsInvalid(cursorPos0to1_finetune)) { cursorPos0to1_finetune = 0.0f; }
            if (UtilitiesDXXL_Math.FloatIsInvalid(cursorPos0to1_superFinetune)) { cursorPos0to1_superFinetune = 0.0f; }
            curr_cursorPosition_as0to1OfChartWidth = cursorPos0to1_raw + cursorPos0to1_finetune + cursorPos0to1_superFinetune;
            curr_cursorPosition_as0to1OfChartWidth = Mathf.Clamp01(curr_cursorPosition_as0to1OfChartWidth);
            if (UtilitiesDXXL_Math.FloatIsInvalid(curr_cursorPosition_as0to1OfChartWidth)) { curr_cursorPosition_as0to1OfChartWidth = 0.5f; }
            curr_cursorPosition_inChartspaceUnits = ConvertXPos_from_0to1units_to_chartspaceUnits(curr_cursorPosition_as0to1OfChartWidth);
            if (UtilitiesDXXL_Math.FloatIsInvalid(curr_cursorPosition_inChartspaceUnits)) { curr_cursorPosition_inChartspaceUnits = 0.0f; }

            TryRecalc_posThatIsNearestToCursor_forEachLine();
            prev_cursorPosition_inChartspaceUnits = curr_cursorPosition_inChartspaceUnits;
            cursorPos_hasBeenCalcedAtLeastOnce = true;
        }

        void ForceCursorSliders_toFitGivenChartspaceXPos(float new_cursorPosition_inChartspaceUnits)
        {
            float new_cursorPosition_as0to1OfChartWidth = ConvertXPos_from_chartspaceUnits_to_0to1units(new_cursorPosition_inChartspaceUnits);
            ForceCursorSliders_toFitGiven0to1XPos(new_cursorPosition_as0to1OfChartWidth);
        }

        public void ForceCursorSliders_toFitGiven0to1XPos(float new_cursorPosition_as0to1OfChartWidth)
        {
            new_cursorPosition_as0to1OfChartWidth = Mathf.Clamp01(new_cursorPosition_as0to1OfChartWidth);

            //Start with trying to change only the raw-slider:
            cursorPos0to1_raw = new_cursorPosition_as0to1OfChartWidth - cursorPos0to1_finetune - cursorPos0to1_superFinetune;

            if ((cursorPos0to1_raw < 0.0f) || (cursorPos0to1_raw > 1.0f))
            {
                //-> deplete the lower ranked finetune-sliders, if the raw slider overshot the 0to1-range
                //-> if this would be done each time without the preceding if-check, then the finetune sliders would not be dragable for activated "alwaysEncapsulateAllValues"
                cursorPos0to1_finetune = 0.0f;
                cursorPos0to1_superFinetune = 0.0f;
                cursorPos0to1_raw = new_cursorPosition_as0to1OfChartWidth;
            }
        }

        float ConvertXPos_from_chartspaceUnits_to_0to1units(float xPositionInChartspaceUnits_toConvert)
        {
            float lowerEndOfXAxis_to_upperEndOfXAxis_inChartspaceUnits = chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingUpperEndOfTheAxis - chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingLowerEndOfTheAxis;
            return ((xPositionInChartspaceUnits_toConvert - chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingLowerEndOfTheAxis) / lowerEndOfXAxis_to_upperEndOfXAxis_inChartspaceUnits);
        }

        float ConvertXPos_from_0to1units_to_chartspaceUnits(float xPositionAs0to1OfChartWidth_toConvert)
        {
            float lowerEndOfXAxis_to_upperEndOfXAxis_inChartspaceUnits = chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingUpperEndOfTheAxis - chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingLowerEndOfTheAxis;
            return (chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingLowerEndOfTheAxis + lowerEndOfXAxis_to_upperEndOfXAxis_inChartspaceUnits * xPositionAs0to1OfChartWidth_toConvert);
        }

        float ConvertXSpan_from_0to1units_to_chartspaceUnits(float xSpanAs0to1OfChartWidth_toConvert)
        {
            float lowerEndOfXAxis_to_upperEndOfXAxis_inChartspaceUnits = chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingUpperEndOfTheAxis - chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingLowerEndOfTheAxis;
            return (lowerEndOfXAxis_to_upperEndOfXAxis_inChartspaceUnits * xSpanAs0to1OfChartWidth_toConvert);
        }

        float ConvertYSpan_from_0to1units_to_chartspaceUnits(float ySpanAs0to1OfChartWidth_toConvert)
        {
            float lowerEndOfYAxis_to_upperEndOfYAxis_inChartspaceUnits = chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingUpperEndOfTheAxis - chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingLowerEndOfTheAxis;
            return (lowerEndOfYAxis_to_upperEndOfYAxis_inChartspaceUnits * ySpanAs0to1OfChartWidth_toConvert);
        }

        void TryRecalc_posThatIsNearestToCursor_forEachLine()
        {
            bool cursorPosHasChanged = (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(curr_cursorPosition_inChartspaceUnits, prev_cursorPosition_inChartspaceUnits) == false);
            if (cursorPosHasChanged || (cursorPos_hasBeenCalcedAtLeastOnce == false))
            {
                for (int i = 0; i < specsForInspector_forEachDrawnLine.Length; i++)
                {
                    specsForInspector_forEachDrawnLine[i].Recalc_posThatIsXNearestToCursor(curr_cursorPosition_inChartspaceUnits);
                }
            }
        }

        public void ResetAxesScalingTo_stateInMomentOfComponentCreation()
        {
            chart_thisInspectorIsAttachedTo.xAxis.SetAxisScalingDuringInspectionComponentPhases(xAxisLowEnd_inMomentOfComponentCreation, xAxisUpperEnd_inMomentOfComponentCreation);
            chart_thisInspectorIsAttachedTo.yAxis.SetAxisScalingDuringInspectionComponentPhases(yAxisLowEnd_inMomentOfComponentCreation, yAxisUpperEnd_inMomentOfComponentCreation);

#if UNITY_EDITOR
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
#endif
        }

        public void ResetAxesScalingTo_encapsulateAllValues()
        {
            chart_thisInspectorIsAttachedTo.xAxis.GetAxisEndsIfAllNonHiddenValuesAreEncapsulated(out float lowerXAxisEnd_ifAllNonHiddenValuesAreEncapsulated, out float upperXAxisEnd_ifAllNonHiddenValuesAreEncapsulated);
            chart_thisInspectorIsAttachedTo.xAxis.SetAxisScalingDuringInspectionComponentPhases(lowerXAxisEnd_ifAllNonHiddenValuesAreEncapsulated, upperXAxisEnd_ifAllNonHiddenValuesAreEncapsulated);

            chart_thisInspectorIsAttachedTo.yAxis.GetAxisEndsIfAllNonHiddenValuesAreEncapsulated(out float lowerYAxisEnd_ifAllNonHiddenValuesAreEncapsulated, out float upperYAxisEnd_ifAllNonHiddenValuesAreEncapsulated);
            chart_thisInspectorIsAttachedTo.yAxis.SetAxisScalingDuringInspectionComponentPhases(lowerYAxisEnd_ifAllNonHiddenValuesAreEncapsulated, upperYAxisEnd_ifAllNonHiddenValuesAreEncapsulated);

            ForceCursorSliders_toFitGivenChartspaceXPos(curr_cursorPosition_inChartspaceUnits);
            RecalcCursorPosition();

#if UNITY_EDITOR
            UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
#endif
        }

        //"Zoom" and "Scroll":
        [SerializeField] [Range(-1.0f, 1.0f)] public float zoomForBothAxesToApply_fromInspectorSlider_m1_to_p1 = 0.0f;
        [SerializeField] [Range(-1.0f, 1.0f)] public float xAxisZoomToApply_fromInspectorSlider_m1_to_p1 = 0.0f;
        [SerializeField] [Range(-1.0f, 1.0f)] public float yAxisZoomToApply_fromInspectorSlider_m1_to_p1 = 0.0f;
        [SerializeField] [Range(-1.0f, 1.0f)] public float xAxisScrollToApply_fromInspectorSlider_m1_to_p1 = 0.0f;
        [SerializeField] [Range(-1.0f, 1.0f)] public float yAxisScrollToApply_fromInspectorSlider_m1_to_p1 = 0.0f;
        public float xAxisZoomSinceMouseDown_fromHandleSlider_m1_to_p1 = 0.0f;
        public float yAxisZoomSinceMouseDown_fromHandleSlider_m1_to_p1 = 0.0f;
        public float bothAxesZoomSinceMouseDown_fromHandleSlider_m1_to_p1 = 0.0f;
        public float scrollSinceMouseDown_fromOnlyXDimHandleSlider_asTravelledWorldSpaceDistanceAlongSliderDirection = 0.0f;
        public float scrollSinceMouseDown_fromOnlyYDimHandleSlider_asTravelledWorldSpaceDistanceAlongSliderDirection = 0.0f;
        public float scrollSinceMouseDown_fromUnifiedHandleSlider_asTravelledWorldSpaceDistanceAlongChartsXDirection = 0.0f;
        public float scrollSinceMouseDown_fromUnifiedHandleSlider_asTravelledWorldSpaceDistanceAlongChartsYDirection = 0.0f;

        static float zoomSpeed_forInspectorSlider = 0.04f;
        static float scrollSpeed_forInspectorSlider = 0.08f;
        static float max_displayedAxisSpan_factor = 10.0f;
        static float min_displayedAxisSpan_inChartspaceUnits = 0.0002f; //a value of "0.0001f" has already once lead to "locked zoom" (probably due to float calculation rounding errors)

        static float zoomOutSpeedFactor_forHandleDrag = 2.0f;
        static float zoomInSpeedFactor_forHandleDrag = 2.0f; //has to be bigger than 1
        bool zoomRespScroll_viaHandle_isInvalid;

        float xSpan_fromAxisLowerEnd_toUpperEnd_inChartspaceUnits_duringMouseDown;
        float xSpan_fromLowerAxisEnd_toCursor_inChartspaceUnits_duringMouseDown;
        float xSpan_fromCursor_toUpperAxisEnd_inChartspaceUnits_duringMouseDown;
        float xValueMarkingLowerEndOfTheAxis_duringMouseDown;
        float xValueMarkingUpperEndOfTheAxis_duringMouseDown;

        float cursorVirtualYPosition_inChartspaceUnits_duringMouseDown;
        float ySpan_fromAxisLowerEnd_toUpperEnd_inChartspaceUnits_duringMouseDown;
        float ySpan_fromLowerAxisEnd_toCursor_inChartspaceUnits_duringMouseDown;
        float ySpan_fromCursor_toUpperAxisEnd_inChartspaceUnits_duringMouseDown;
        float yValueMarkingLowerEndOfTheAxis_duringMouseDown;
        float yValueMarkingUpperEndOfTheAxis_duringMouseDown;

        void ScaleAxes()
        {
            if (alwaysEncapsulateAllValues)
            {
                ResetAxesScalingTo_encapsulateAllValues();
            }
            else
            {
                if (hideHandles_andInsteadUseInspectorSlidersForZoomAndScroll)
                {
                    if (UtilitiesDXXL_Math.ApproximatelyZero(zoomForBothAxesToApply_fromInspectorSlider_m1_to_p1) == false)
                    {
                        xAxisZoomToApply_fromInspectorSlider_m1_to_p1 = zoomForBothAxesToApply_fromInspectorSlider_m1_to_p1;
                        yAxisZoomToApply_fromInspectorSlider_m1_to_p1 = zoomForBothAxesToApply_fromInspectorSlider_m1_to_p1;
                    }

                    if (UtilitiesDXXL_Math.ApproximatelyZero(xAxisZoomToApply_fromInspectorSlider_m1_to_p1) == false) { ApplyXAxisZoom_fromInspectorSlider(); }
                    if (UtilitiesDXXL_Math.ApproximatelyZero(xAxisScrollToApply_fromInspectorSlider_m1_to_p1) == false) { ApplyXAxisScroll_fromInspectorSlider(); }
                    if (UtilitiesDXXL_Math.ApproximatelyZero(yAxisZoomToApply_fromInspectorSlider_m1_to_p1) == false) { ApplyYAxisZoom_fromInspectorSlider(); }
                    if (UtilitiesDXXL_Math.ApproximatelyZero(yAxisScrollToApply_fromInspectorSlider_m1_to_p1) == false) { ApplyYAxisScroll_fromInspectorSlider(); }
                }
                else
                {
                    if (UtilitiesDXXL_Math.ApproximatelyZero(bothAxesZoomSinceMouseDown_fromHandleSlider_m1_to_p1) == false)
                    {
                        xAxisZoomSinceMouseDown_fromHandleSlider_m1_to_p1 = bothAxesZoomSinceMouseDown_fromHandleSlider_m1_to_p1;
                        yAxisZoomSinceMouseDown_fromHandleSlider_m1_to_p1 = bothAxesZoomSinceMouseDown_fromHandleSlider_m1_to_p1;
                    }

                    if (UtilitiesDXXL_Math.ApproximatelyZero(scrollSinceMouseDown_fromUnifiedHandleSlider_asTravelledWorldSpaceDistanceAlongChartsXDirection) == false)
                    {
                        scrollSinceMouseDown_fromOnlyXDimHandleSlider_asTravelledWorldSpaceDistanceAlongSliderDirection = scrollSinceMouseDown_fromUnifiedHandleSlider_asTravelledWorldSpaceDistanceAlongChartsXDirection;
                    }

                    if (UtilitiesDXXL_Math.ApproximatelyZero(scrollSinceMouseDown_fromUnifiedHandleSlider_asTravelledWorldSpaceDistanceAlongChartsYDirection) == false)
                    {
                        scrollSinceMouseDown_fromOnlyYDimHandleSlider_asTravelledWorldSpaceDistanceAlongSliderDirection = scrollSinceMouseDown_fromUnifiedHandleSlider_asTravelledWorldSpaceDistanceAlongChartsYDirection;
                    }

                    if (UtilitiesDXXL_Math.ApproximatelyZero(xAxisZoomSinceMouseDown_fromHandleSlider_m1_to_p1) == false) { ApplyXAxisZoom_fromHandle(); }
                    if (UtilitiesDXXL_Math.ApproximatelyZero(scrollSinceMouseDown_fromOnlyXDimHandleSlider_asTravelledWorldSpaceDistanceAlongSliderDirection) == false) { ApplyXAxisScroll_fromHandle(); }
                    if (UtilitiesDXXL_Math.ApproximatelyZero(yAxisZoomSinceMouseDown_fromHandleSlider_m1_to_p1) == false) { ApplyYAxisZoom_fromHandle(); }
                    if (UtilitiesDXXL_Math.ApproximatelyZero(scrollSinceMouseDown_fromOnlyYDimHandleSlider_asTravelledWorldSpaceDistanceAlongSliderDirection) == false) { ApplyYAxisScroll_fromHandle(); }
                }
            }
        }

        public void SaveZoomAndScrollState_onMouseDown()
        {
            zoomRespScroll_viaHandle_isInvalid = false;
            if (UtilitiesDXXL_Math.FloatIsInvalid(chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingLowerEndOfTheAxis)) { ErrorLogForInvalidZoomRespScroll(); return; }
            if (UtilitiesDXXL_Math.FloatIsInvalid(chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingUpperEndOfTheAxis)) { ErrorLogForInvalidZoomRespScroll(); return; }
            if (UtilitiesDXXL_Math.FloatIsInvalid(chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingLowerEndOfTheAxis)) { ErrorLogForInvalidZoomRespScroll(); return; }
            if (UtilitiesDXXL_Math.FloatIsInvalid(chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingUpperEndOfTheAxis)) { ErrorLogForInvalidZoomRespScroll(); return; }

            xSpan_fromAxisLowerEnd_toUpperEnd_inChartspaceUnits_duringMouseDown = chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingUpperEndOfTheAxis - chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingLowerEndOfTheAxis;
            xSpan_fromLowerAxisEnd_toCursor_inChartspaceUnits_duringMouseDown = curr_cursorPosition_inChartspaceUnits - chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingLowerEndOfTheAxis;
            xSpan_fromCursor_toUpperAxisEnd_inChartspaceUnits_duringMouseDown = chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingUpperEndOfTheAxis - curr_cursorPosition_inChartspaceUnits;
            xValueMarkingLowerEndOfTheAxis_duringMouseDown = chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingLowerEndOfTheAxis;
            xValueMarkingUpperEndOfTheAxis_duringMouseDown = chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingUpperEndOfTheAxis;

            cursorVirtualYPosition_inChartspaceUnits_duringMouseDown = Get_curr_cursorVirtualYPosition_inChartspaceUnits();
            ySpan_fromAxisLowerEnd_toUpperEnd_inChartspaceUnits_duringMouseDown = chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingUpperEndOfTheAxis - chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingLowerEndOfTheAxis;
            ySpan_fromLowerAxisEnd_toCursor_inChartspaceUnits_duringMouseDown = cursorVirtualYPosition_inChartspaceUnits_duringMouseDown - chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingLowerEndOfTheAxis;
            ySpan_fromCursor_toUpperAxisEnd_inChartspaceUnits_duringMouseDown = chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingUpperEndOfTheAxis - cursorVirtualYPosition_inChartspaceUnits_duringMouseDown;
            yValueMarkingLowerEndOfTheAxis_duringMouseDown = chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingLowerEndOfTheAxis;
            yValueMarkingUpperEndOfTheAxis_duringMouseDown = chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingUpperEndOfTheAxis;
        }

        void ErrorLogForInvalidZoomRespScroll()
        {
            zoomRespScroll_viaHandle_isInvalid = true;
            Debug.LogError("Draw XXL: Chart handle zoom/scroll is invalid.");
        }

        void ApplyXAxisZoom_fromHandle()
        {
            if (zoomRespScroll_viaHandle_isInvalid == false)
            {
                float zoomChangeFactor_unclamped;
                if (xAxisZoomSinceMouseDown_fromHandleSlider_m1_to_p1 < 0.0f)
                {
                    zoomChangeFactor_unclamped = 1.0f - zoomOutSpeedFactor_forHandleDrag * xAxisZoomSinceMouseDown_fromHandleSlider_m1_to_p1; //-> raises to higher thatn 1, since "xAxisZoomSinceMouseDown_fromHandleSlider_m1_to_p1" is negative here
                }
                else
                {
                    zoomChangeFactor_unclamped = Mathf.Pow(zoomInSpeedFactor_forHandleDrag, (-xAxisZoomSinceMouseDown_fromHandleSlider_m1_to_p1));
                }
                ApplyXAxisZoom(zoomChangeFactor_unclamped, xSpan_fromAxisLowerEnd_toUpperEnd_inChartspaceUnits_duringMouseDown, xSpan_fromLowerAxisEnd_toCursor_inChartspaceUnits_duringMouseDown, xSpan_fromCursor_toUpperAxisEnd_inChartspaceUnits_duringMouseDown);
            }
        }

        void ApplyXAxisZoom_fromInspectorSlider()
        {
            if (UtilitiesDXXL_Math.FloatIsInvalid(chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingLowerEndOfTheAxis)) { return; }
            if (UtilitiesDXXL_Math.FloatIsInvalid(chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingUpperEndOfTheAxis)) { return; }

            float old_xSpan_fromAxisLowerEnd_toUpperEnd = chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingUpperEndOfTheAxis - chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingLowerEndOfTheAxis;
            float old_xSpan_fromLowerAxisEnd_toCursor = curr_cursorPosition_inChartspaceUnits - chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingLowerEndOfTheAxis;
            float old_xSpan_fromCursor_toUpperAxisEnd = chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingUpperEndOfTheAxis - curr_cursorPosition_inChartspaceUnits;

            float zoomXAxis_sign = Mathf.Sign(xAxisZoomToApply_fromInspectorSlider_m1_to_p1);
            float zoomXAxis_modulated = zoomXAxis_sign * (xAxisZoomToApply_fromInspectorSlider_m1_to_p1 * xAxisZoomToApply_fromInspectorSlider_m1_to_p1);
            float zoomChangeFactor_unclamped = 1.0f - (zoomSpeed_forInspectorSlider * zoomXAxis_modulated);

            ApplyXAxisZoom(zoomChangeFactor_unclamped, old_xSpan_fromAxisLowerEnd_toUpperEnd, old_xSpan_fromLowerAxisEnd_toCursor, old_xSpan_fromCursor_toUpperAxisEnd);
        }

        void ApplyXAxisZoom(float zoomChangeFactor_unclamped, float old_xSpan_fromAxisLowerEnd_toUpperEnd, float old_xSpan_fromLowerAxisEnd_toCursor, float old_xSpan_fromCursor_toUpperAxisEnd)
        {
            float new_xSpan_fromLowerToUpperAxisEnd_unclamped = old_xSpan_fromAxisLowerEnd_toUpperEnd * zoomChangeFactor_unclamped;
            chart_thisInspectorIsAttachedTo.xAxis.GetAxisEndsIfAllNonHiddenValuesAreEncapsulated(out float lowerAxisEnd_ifAllNonHiddenValuesAreEncapsulated, out float upperAxisEnd_ifAllNonHiddenValuesAreEncapsulated);
            float xSpan_fromAxisLowerEnd_toUpperEnd_ifAllNonHiddenValuesAreEncapsulated = upperAxisEnd_ifAllNonHiddenValuesAreEncapsulated - lowerAxisEnd_ifAllNonHiddenValuesAreEncapsulated;
            float max_xSpan = max_displayedAxisSpan_factor * xSpan_fromAxisLowerEnd_toUpperEnd_ifAllNonHiddenValuesAreEncapsulated;
            float new_xSpan_fromLowerToUpperAxisEnd_clamped = Mathf.Clamp(new_xSpan_fromLowerToUpperAxisEnd_unclamped, min_displayedAxisSpan_inChartspaceUnits, max_xSpan);
            float zoomChangeFactor_clamped = zoomChangeFactor_unclamped * (new_xSpan_fromLowerToUpperAxisEnd_clamped / new_xSpan_fromLowerToUpperAxisEnd_unclamped);

            float new_xSpan_fromLowerAxisEnd_toCursor = old_xSpan_fromLowerAxisEnd_toCursor * zoomChangeFactor_clamped;
            float new_xSpan_fromCursor_toUpperAxisEnd = old_xSpan_fromCursor_toUpperAxisEnd * zoomChangeFactor_clamped;
            float new_valueOf_xAxisLowEnd = curr_cursorPosition_inChartspaceUnits - new_xSpan_fromLowerAxisEnd_toCursor;
            float new_valueOf_xAxisUpperEnd = curr_cursorPosition_inChartspaceUnits + new_xSpan_fromCursor_toUpperAxisEnd;

            chart_thisInspectorIsAttachedTo.xAxis.SetAxisScalingDuringInspectionComponentPhases(new_valueOf_xAxisLowEnd, new_valueOf_xAxisUpperEnd);
        }

        void ApplyYAxisZoom_fromHandle()
        {
            if (zoomRespScroll_viaHandle_isInvalid == false)
            {
                float zoomChangeFactor_unclamped;
                if (yAxisZoomSinceMouseDown_fromHandleSlider_m1_to_p1 < 0.0f)
                {
                    zoomChangeFactor_unclamped = 1.0f - zoomOutSpeedFactor_forHandleDrag * yAxisZoomSinceMouseDown_fromHandleSlider_m1_to_p1; //-> raises to higher thatn 1, since "yAxisZoomSinceMouseDown_fromHandleSlider_m1_to_p1" is negative here
                }
                else
                {
                    zoomChangeFactor_unclamped = Mathf.Pow(zoomInSpeedFactor_forHandleDrag, (-yAxisZoomSinceMouseDown_fromHandleSlider_m1_to_p1));
                }
                ApplyYAxisZoom(zoomChangeFactor_unclamped, cursorVirtualYPosition_inChartspaceUnits_duringMouseDown, ySpan_fromAxisLowerEnd_toUpperEnd_inChartspaceUnits_duringMouseDown, ySpan_fromLowerAxisEnd_toCursor_inChartspaceUnits_duringMouseDown, ySpan_fromCursor_toUpperAxisEnd_inChartspaceUnits_duringMouseDown);
            }
        }

        void ApplyYAxisZoom_fromInspectorSlider()
        {
            if (UtilitiesDXXL_Math.FloatIsInvalid(chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingLowerEndOfTheAxis)) { return; }
            if (UtilitiesDXXL_Math.FloatIsInvalid(chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingUpperEndOfTheAxis)) { return; }

            float curr_cursorVirtualYPosition_inChartspaceUnits = Get_curr_cursorVirtualYPosition_inChartspaceUnits();

            float old_ySpan_fromAxisLowerEnd_toUpperEnd = chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingUpperEndOfTheAxis - chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingLowerEndOfTheAxis;
            float old_ySpan_fromLowerAxisEnd_toCursor = curr_cursorVirtualYPosition_inChartspaceUnits - chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingLowerEndOfTheAxis;
            float old_ySpan_fromCursor_toUpperAxisEnd = chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingUpperEndOfTheAxis - curr_cursorVirtualYPosition_inChartspaceUnits;

            float zoomYAxis_sign = Mathf.Sign(yAxisZoomToApply_fromInspectorSlider_m1_to_p1);
            float zoomYAxis_modulated = zoomYAxis_sign * (yAxisZoomToApply_fromInspectorSlider_m1_to_p1 * yAxisZoomToApply_fromInspectorSlider_m1_to_p1);
            float zoomChangeFactor_unclamped = 1.0f - (zoomSpeed_forInspectorSlider * zoomYAxis_modulated);

            ApplyYAxisZoom(zoomChangeFactor_unclamped, curr_cursorVirtualYPosition_inChartspaceUnits, old_ySpan_fromAxisLowerEnd_toUpperEnd, old_ySpan_fromLowerAxisEnd_toCursor, old_ySpan_fromCursor_toUpperAxisEnd);
        }

        void ApplyYAxisZoom(float zoomChangeFactor_unclamped, float curr_cursorVirtualYPosition_inChartspaceUnits, float old_ySpan_fromAxisLowerEnd_toUpperEnd, float old_ySpan_fromLowerAxisEnd_toCursor, float old_ySpan_fromCursor_toUpperAxisEnd)
        {
            float new_ySpan_fromLowerToUpperAxisEnd_unclamped = old_ySpan_fromAxisLowerEnd_toUpperEnd * zoomChangeFactor_unclamped;
            chart_thisInspectorIsAttachedTo.yAxis.GetAxisEndsIfAllNonHiddenValuesAreEncapsulated(out float lowerAxisEnd_ifAllNonHiddenValuesAreEncapsulated, out float upperAxisEnd_ifAllNonHiddenValuesAreEncapsulated);
            float ySpan_fromAxisLowerEnd_toUpperEnd_ifAllNonHiddenValuesAreEncapsulated = upperAxisEnd_ifAllNonHiddenValuesAreEncapsulated - lowerAxisEnd_ifAllNonHiddenValuesAreEncapsulated;
            float max_ySpan = max_displayedAxisSpan_factor * ySpan_fromAxisLowerEnd_toUpperEnd_ifAllNonHiddenValuesAreEncapsulated;
            float new_ySpan_fromLowerToUpperAxisEnd_clamped = Mathf.Clamp(new_ySpan_fromLowerToUpperAxisEnd_unclamped, min_displayedAxisSpan_inChartspaceUnits, max_ySpan);
            float zoomChangeFactor_clamped = zoomChangeFactor_unclamped * (new_ySpan_fromLowerToUpperAxisEnd_clamped / new_ySpan_fromLowerToUpperAxisEnd_unclamped);

            float new_ySpan_fromLowerAxisEnd_toCursor = old_ySpan_fromLowerAxisEnd_toCursor * zoomChangeFactor_clamped;
            float new_ySpan_fromCursor_toUpperAxisEnd = old_ySpan_fromCursor_toUpperAxisEnd * zoomChangeFactor_clamped;
            float new_valueOf_yAxisLowEnd = curr_cursorVirtualYPosition_inChartspaceUnits - new_ySpan_fromLowerAxisEnd_toCursor;
            float new_valueOf_yAxisUpperEnd = curr_cursorVirtualYPosition_inChartspaceUnits + new_ySpan_fromCursor_toUpperAxisEnd;

            chart_thisInspectorIsAttachedTo.yAxis.SetAxisScalingDuringInspectionComponentPhases(new_valueOf_yAxisLowEnd, new_valueOf_yAxisUpperEnd);
        }

        void ApplyXAxisScroll_fromHandle()
        {
            if (zoomRespScroll_viaHandle_isInvalid == false)
            {
                float xAxisScrollSinceMouseDown_fromHandleSlider_as0to1ofChart = scrollSinceMouseDown_fromOnlyXDimHandleSlider_asTravelledWorldSpaceDistanceAlongSliderDirection / chart_thisInspectorIsAttachedTo.Width_inWorldSpace;
                float xAxisScrollSinceMouseDown_fromHandleSlider_inChartspaceUnits = ConvertXSpan_from_0to1units_to_chartspaceUnits(xAxisScrollSinceMouseDown_fromHandleSlider_as0to1ofChart);
                ApplyXAxisScroll(xAxisScrollSinceMouseDown_fromHandleSlider_inChartspaceUnits, xSpan_fromAxisLowerEnd_toUpperEnd_inChartspaceUnits_duringMouseDown, xValueMarkingLowerEndOfTheAxis_duringMouseDown, xValueMarkingUpperEndOfTheAxis_duringMouseDown);
            }
        }

        void ApplyXAxisScroll_fromInspectorSlider()
        {
            if (UtilitiesDXXL_Math.FloatIsInvalid(chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingLowerEndOfTheAxis)) { return; }
            if (UtilitiesDXXL_Math.FloatIsInvalid(chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingUpperEndOfTheAxis)) { return; }

            float scrollXAxis_sign = Mathf.Sign(xAxisScrollToApply_fromInspectorSlider_m1_to_p1);
            float scrollXAxis_modulated = scrollXAxis_sign * (xAxisScrollToApply_fromInspectorSlider_m1_to_p1 * xAxisScrollToApply_fromInspectorSlider_m1_to_p1);
            float xSpan_fromAxisLowerEnd_toUpperEnd_inChartspaceUnits = chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingUpperEndOfTheAxis - chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingLowerEndOfTheAxis;
            float chartWorldsizeAspect_inverse_heightToWidth = Get_chartWorldsizeAspect_inverse_heightToWidth(); //-> adapt scroll speed for charts with non-1 aspect and non-equal axis graduation, so that the scroll speed is homogenuous between the x and the y direction.
            float scrollSpan_inChartspaceUnits = xSpan_fromAxisLowerEnd_toUpperEnd_inChartspaceUnits * scrollXAxis_modulated * scrollSpeed_forInspectorSlider * chartWorldsizeAspect_inverse_heightToWidth;

            ApplyXAxisScroll(scrollSpan_inChartspaceUnits, xSpan_fromAxisLowerEnd_toUpperEnd_inChartspaceUnits, chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingLowerEndOfTheAxis, chart_thisInspectorIsAttachedTo.xAxis.ValueMarkingUpperEndOfTheAxis);
        }

        void ApplyXAxisScroll(float scrollSpan_inChartspaceUnits, float xSpan_fromAxisLowerEnd_toUpperEnd, float old_xValueMarkingLowerEndOfTheAxis, float old_xValueMarkingUpperEndOfTheAxis)
        {
            float newValueOf_xAxisLowEnd = old_xValueMarkingLowerEndOfTheAxis - scrollSpan_inChartspaceUnits;
            float newValueOf_xAxisUpperEnd = old_xValueMarkingUpperEndOfTheAxis - scrollSpan_inChartspaceUnits;
            ClampXScroll_soThatItDoesntMoveAwayFromTheDataValueArea(ref newValueOf_xAxisLowEnd, ref newValueOf_xAxisUpperEnd, xSpan_fromAxisLowerEnd_toUpperEnd);

            chart_thisInspectorIsAttachedTo.xAxis.SetAxisScalingDuringInspectionComponentPhases(newValueOf_xAxisLowEnd, newValueOf_xAxisUpperEnd);
            ForceCursorSliders_toFitGivenChartspaceXPos(curr_cursorPosition_inChartspaceUnits);
            RecalcCursorPosition();
        }

        void ClampXScroll_soThatItDoesntMoveAwayFromTheDataValueArea(ref float newValueOf_xAxisLowEnd, ref float newValueOf_xAxisUpperEnd, float xSpan_fromAxisLowerEnd_toUpperEnd)
        {
            if (newValueOf_xAxisLowEnd > chart_thisInspectorIsAttachedTo.overallMaxXValue_includingHiddenLines)
            {
                newValueOf_xAxisLowEnd = chart_thisInspectorIsAttachedTo.overallMaxXValue_includingHiddenLines;
                newValueOf_xAxisUpperEnd = newValueOf_xAxisLowEnd + xSpan_fromAxisLowerEnd_toUpperEnd;
            }
            else
            {
                if (newValueOf_xAxisUpperEnd < chart_thisInspectorIsAttachedTo.overallMinXValue_includingHiddenLines)
                {
                    newValueOf_xAxisUpperEnd = chart_thisInspectorIsAttachedTo.overallMinXValue_includingHiddenLines;
                    newValueOf_xAxisLowEnd = newValueOf_xAxisUpperEnd - xSpan_fromAxisLowerEnd_toUpperEnd;
                }
            }
        }

        void ApplyYAxisScroll_fromHandle()
        {
            if (zoomRespScroll_viaHandle_isInvalid == false)
            {
                float yAxisScrollSinceMouseDown_fromHandleSlider_as0to1ofChart = scrollSinceMouseDown_fromOnlyYDimHandleSlider_asTravelledWorldSpaceDistanceAlongSliderDirection / chart_thisInspectorIsAttachedTo.Height_inWorldSpace;
                float yAxisScrollSinceMouseDown_fromHandleSlider_inChartspaceUnits = ConvertYSpan_from_0to1units_to_chartspaceUnits(yAxisScrollSinceMouseDown_fromHandleSlider_as0to1ofChart);
                ApplyYAxisScroll(yAxisScrollSinceMouseDown_fromHandleSlider_inChartspaceUnits, ySpan_fromAxisLowerEnd_toUpperEnd_inChartspaceUnits_duringMouseDown, yValueMarkingLowerEndOfTheAxis_duringMouseDown, yValueMarkingUpperEndOfTheAxis_duringMouseDown);
            }
        }

        void ApplyYAxisScroll_fromInspectorSlider()
        {
            if (UtilitiesDXXL_Math.FloatIsInvalid(chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingLowerEndOfTheAxis)) { return; }
            if (UtilitiesDXXL_Math.FloatIsInvalid(chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingUpperEndOfTheAxis)) { return; }

            float scrollYAxis_sign = Mathf.Sign(yAxisScrollToApply_fromInspectorSlider_m1_to_p1);
            float scrollYAxis_modulated = scrollYAxis_sign * (yAxisScrollToApply_fromInspectorSlider_m1_to_p1 * yAxisScrollToApply_fromInspectorSlider_m1_to_p1);
            float ySpan_fromAxisLowerEnd_toUpperEnd_inChartspaceUnits = chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingUpperEndOfTheAxis - chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingLowerEndOfTheAxis;
            float scrollSpan_inChartspaceUnits = ySpan_fromAxisLowerEnd_toUpperEnd_inChartspaceUnits * scrollYAxis_modulated * scrollSpeed_forInspectorSlider;

            ApplyYAxisScroll(scrollSpan_inChartspaceUnits, ySpan_fromAxisLowerEnd_toUpperEnd_inChartspaceUnits, chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingLowerEndOfTheAxis, chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingUpperEndOfTheAxis);
        }

        void ApplyYAxisScroll(float scrollSpan_inChartspaceUnits, float ySpan_fromAxisLowerEnd_toUpperEnd, float old_yValueMarkingLowerEndOfTheAxis, float old_yValueMarkingUpperEndOfTheAxis)
        {
            float newValueOf_yAxisLowEnd = old_yValueMarkingLowerEndOfTheAxis - scrollSpan_inChartspaceUnits;
            float newValueOf_yAxisUpperEnd = old_yValueMarkingUpperEndOfTheAxis - scrollSpan_inChartspaceUnits;
            ClampYScroll_soThatItDoesntMoveAwayFromTheDataValueArea(ref newValueOf_yAxisLowEnd, ref newValueOf_yAxisUpperEnd, ySpan_fromAxisLowerEnd_toUpperEnd);
            chart_thisInspectorIsAttachedTo.yAxis.SetAxisScalingDuringInspectionComponentPhases(newValueOf_yAxisLowEnd, newValueOf_yAxisUpperEnd);
        }

        void ClampYScroll_soThatItDoesntMoveAwayFromTheDataValueArea(ref float newValueOf_yAxisLowEnd, ref float newValueOf_yAxisUpperEnd, float ySpan_fromAxisLowerEnd_toUpperEnd)
        {
            if (newValueOf_yAxisLowEnd > chart_thisInspectorIsAttachedTo.overallMaxYValue_includingHiddenLines)
            {
                newValueOf_yAxisLowEnd = chart_thisInspectorIsAttachedTo.overallMaxYValue_includingHiddenLines;
                newValueOf_yAxisUpperEnd = newValueOf_yAxisLowEnd + ySpan_fromAxisLowerEnd_toUpperEnd;
            }
            else
            {
                if (newValueOf_yAxisUpperEnd < chart_thisInspectorIsAttachedTo.overallMinYValue_includingHiddenLines)
                {
                    newValueOf_yAxisUpperEnd = chart_thisInspectorIsAttachedTo.overallMinYValue_includingHiddenLines;
                    newValueOf_yAxisLowEnd = newValueOf_yAxisUpperEnd - ySpan_fromAxisLowerEnd_toUpperEnd;
                }
            }
        }

        float Get_curr_cursorVirtualYPosition_inChartspaceUnits()
        {
            int numberOf_allDisplayedLinesThatHaveTheirYPosInsideDrawnChartArea = 0;
            float sumOfAll_cursorYPositionsInChartspace_forAllDisplayedLinesThatHaveTheirYPosInsideDrawnChartArea = 0.0f;
            for (int i_line = 0; i_line < specsForInspector_forEachDrawnLine.Length; i_line++)
            {
                if (specsForInspector_forEachDrawnLine[i_line].currentHideLineState == false)
                {
                    if ((specsForInspector_forEachDrawnLine[i_line].currentHideCursorXState_duringComponentInspectionPhase == false) || (specsForInspector_forEachDrawnLine[i_line].currentHideCursorYState_duringComponentInspectionPhase == false))
                    {
                        if (specsForInspector_forEachDrawnLine[i_line].hasValidDatapointThatIsXNearestToCursor)
                        {
                            if (chart_thisInspectorIsAttachedTo.IsInsideDrawnChartArea(specsForInspector_forEachDrawnLine[i_line].posInChartspace_ofDatapointThatIsNearestToCursor))
                            {
                                numberOf_allDisplayedLinesThatHaveTheirYPosInsideDrawnChartArea++;
                                sumOfAll_cursorYPositionsInChartspace_forAllDisplayedLinesThatHaveTheirYPosInsideDrawnChartArea = sumOfAll_cursorYPositionsInChartspace_forAllDisplayedLinesThatHaveTheirYPosInsideDrawnChartArea + specsForInspector_forEachDrawnLine[i_line].posInChartspace_ofDatapointThatIsNearestToCursor.y;
                            }
                        }
                    }
                }
            }

            if (numberOf_allDisplayedLinesThatHaveTheirYPosInsideDrawnChartArea == 0)
            {
                float middleOfYAxis = 0.5f * (chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingLowerEndOfTheAxis + chart_thisInspectorIsAttachedTo.yAxis.ValueMarkingUpperEndOfTheAxis);
                return middleOfYAxis;
            }
            else
            {
                return (sumOfAll_cursorYPositionsInChartspace_forAllDisplayedLinesThatHaveTheirYPosInsideDrawnChartArea / (float)numberOf_allDisplayedLinesThatHaveTheirYPosInsideDrawnChartArea);
            }
        }

        float Get_chartWorldsizeAspect_inverse_heightToWidth()
        {
            if (theChartIsDrawnInScreenspace)
            {
                float charts_worldspaceHeight = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(screenSpaceTargetCamera, InternalDXXL_BoundsCamViewportSpace.viewportCenter, true, chart_thisInspectorIsAttachedTo.Height_relToCamViewportHeight);
                float charts_worldspaceWidth;
                if (chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight)
                {
                    charts_worldspaceWidth = UtilitiesDXXL_Screenspace.HorizExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(screenSpaceTargetCamera, InternalDXXL_BoundsCamViewportSpace.viewportCenter, true, chart_thisInspectorIsAttachedTo.Width_relToCamViewport);
                }
                else
                {
                    charts_worldspaceWidth = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(screenSpaceTargetCamera, InternalDXXL_BoundsCamViewportSpace.viewportCenter, true, chart_thisInspectorIsAttachedTo.Width_relToCamViewport);
                }
                return (charts_worldspaceHeight / charts_worldspaceWidth);
            }
            else
            {
                return (chart_thisInspectorIsAttachedTo.Height_inWorldSpace / chart_thisInspectorIsAttachedTo.Width_inWorldSpace);
            }
        }

        public float GetBacksnapSliderReferenceLength_inWorldspaceUnits()
        {
            return 0.2f * (chart_thisInspectorIsAttachedTo.Width_inWorldSpace + chart_thisInspectorIsAttachedTo.Height_inWorldSpace);
        }

        void DrawTheChart()
        {
            if (theChartIsDrawnInScreenspace)
            {
                chart_thisInspectorIsAttachedTo.Internal_DrawScreenspace(screenSpaceTargetCamera, chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight, 0.0f);
            }
            else
            {
                chart_thisInspectorIsAttachedTo.Internal_Draw(0.0f, nonScreenspaceDrawing_happensWith_drawConfigOf_hiddenByNearerObjects);
            }
        }

        Vector3 chartPosition_beforeScreenspaceDrawing;
        Quaternion chartFixedRotation_beforeScreenspaceDrawing;
        float chartWidthWorldspace_beforeScreenspaceDrawing;
        float chartHeigthWorldspace_beforeScreenspaceDrawing;
        ChartDrawing.RotationSource chartRotationSource_beforeScreenspaceDrawing;

        public void TryNote_chartWorldSpacePosRotScale_beforeChangingItToScreenspace()
        {
            if (theChartIsDrawnInScreenspace)
            {
                chartPosition_beforeScreenspaceDrawing = chart_thisInspectorIsAttachedTo.Position_worldspace;
                chartFixedRotation_beforeScreenspaceDrawing = chart_thisInspectorIsAttachedTo.fixedRotation;
                chartWidthWorldspace_beforeScreenspaceDrawing = chart_thisInspectorIsAttachedTo.Width_inWorldSpace;
                chartHeigthWorldspace_beforeScreenspaceDrawing = chart_thisInspectorIsAttachedTo.Height_inWorldSpace;
                chartRotationSource_beforeScreenspaceDrawing = chart_thisInspectorIsAttachedTo.rotationSource;
                chart_thisInspectorIsAttachedTo.autoFlipAllText_toFitObsererCamera = false;
            }
        }

        public void TrySetBack_chartWorldSpacePosRotScale_afterUsingItInScreenspace()
        {
            if (theChartIsDrawnInScreenspace)
            {
                chart_thisInspectorIsAttachedTo.Position_worldspace = chartPosition_beforeScreenspaceDrawing;
                chart_thisInspectorIsAttachedTo.fixedRotation = chartFixedRotation_beforeScreenspaceDrawing;
                chart_thisInspectorIsAttachedTo.Width_inWorldSpace = chartWidthWorldspace_beforeScreenspaceDrawing;
                chart_thisInspectorIsAttachedTo.Height_inWorldSpace = chartHeigthWorldspace_beforeScreenspaceDrawing;
                chart_thisInspectorIsAttachedTo.rotationSource = chartRotationSource_beforeScreenspaceDrawing;
                chart_thisInspectorIsAttachedTo.autoFlipAllText_toFitObsererCamera = true;
            }
        }

        void DrawCursor()
        {
            DrawVerticalCursorLine();
            DrawCursorNearestPointsOfLines();
        }

        void DrawVerticalCursorLine()
        {
            if (chart_thisInspectorIsAttachedTo.IsEmptyWithNoLinesToDraw == false)
            {
                bool cursorDrawing_is_hiddenByNearerObjects = CheckIfDrawingHappesWithConfigOf_hiddenByNearerObjects();

                Vector3 lowerEndVertexOfSlidersVertLine = GetCursorPosOnLowerEndOfChart();
                Vector3 higherEndVertexOfSlidersVertLine = GetCursorPosOnHigherEndOfChart();
                Color color = chart_thisInspectorIsAttachedTo.color;
                Line_fadeableAnimSpeed.InternalDraw(lowerEndVertexOfSlidersVertLine, higherEndVertexOfSlidersVertLine, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, 0.0f, cursorDrawing_is_hiddenByNearerObjects, false, false);

                Vector3 center_ofBasePlane = lowerEndVertexOfSlidersVertLine;
                float height = GetHeightOfCursorPyramid();
                float width_ofBase = 0.0f;
                float length_ofBase = 2.0f * height;
                Vector3 normal_ofBaseTowardsApex = chart_thisInspectorIsAttachedTo.yAxis.AxisVector_normalized_inWorldSpace;
                Vector3 up_insideBasePlane = chart_thisInspectorIsAttachedTo.xAxis.AxisVector_normalized_inWorldSpace;
                DrawShapes.Pyramid(center_ofBasePlane, height, width_ofBase, length_ofBase, color, normal_ofBaseTowardsApex, up_insideBasePlane, DrawShapes.Shape2DType.circle, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, 0.0f, cursorDrawing_is_hiddenByNearerObjects);

                center_ofBasePlane = higherEndVertexOfSlidersVertLine;
                normal_ofBaseTowardsApex = -chart_thisInspectorIsAttachedTo.yAxis.AxisVector_normalized_inWorldSpace;
                DrawShapes.Pyramid(center_ofBasePlane, height, width_ofBase, length_ofBase, color, normal_ofBaseTowardsApex, up_insideBasePlane, DrawShapes.Shape2DType.circle, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, 0.0f, cursorDrawing_is_hiddenByNearerObjects);
            }
        }

        public Vector3 GetCursorPosOnLowerEndOfChart()
        {
            return (chart_thisInspectorIsAttachedTo.Position_worldspace + chart_thisInspectorIsAttachedTo.xAxis.AxisVector_inWorldSpace * curr_cursorPosition_as0to1OfChartWidth);
        }

        public Vector3 GetCursorPosOnHigherEndOfChart()
        {
            return (GetCursorPosOnLowerEndOfChart() + chart_thisInspectorIsAttachedTo.yAxis.AxisVector_inWorldSpace);
        }

        public float GetHeightOfCursorPyramid()
        {
            return (chart_thisInspectorIsAttachedTo.Height_inWorldSpace * 0.03f);
        }

        void DrawCursorNearestPointsOfLines()
        {
            bool cursorDrawing_is_hiddenByNearerObjects = CheckIfDrawingHappesWithConfigOf_hiddenByNearerObjects();
            for (int i_line = 0; i_line < specsForInspector_forEachDrawnLine.Length; i_line++)
            {
                specsForInspector_forEachDrawnLine[i_line].DrawCursorNearestPoint(sizeOfDatapointVisualization, cursorDrawing_is_hiddenByNearerObjects);
            }
        }

        void TryDrawUnified45degAxisForHandles()
        {
#if UNITY_EDITOR
            if (hideHandles_andInsteadUseInspectorSlidersForZoomAndScroll == false)
            {
                if (UnityEditor.Selection.Contains(gameObject.GetInstanceID()))
                {
                    Vector3 startOfLine = chart_thisInspectorIsAttachedTo.Position_worldspace;
                    Vector3 endOfLine = startOfLine + chart_thisInspectorIsAttachedTo.Get_unified45degAxis_forHandleSliders_normalized() * chart_thisInspectorIsAttachedTo.Get_unified45degAxis_length();
                    bool hiddenByNearerObjects = CheckIfDrawingHappesWithConfigOf_hiddenByNearerObjects();
                    DrawBasics.Line(startOfLine, endOfLine, chart_thisInspectorIsAttachedTo.color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, default(Vector3), false, 0.0f, 0.0f, 0.0f, 0.0f, hiddenByNearerObjects);
                }
            }
#endif
        }

        bool CheckIfDrawingHappesWithConfigOf_hiddenByNearerObjects()
        {
            if (theChartIsDrawnInScreenspace)
            {
                return false;
            }
            else
            {
                return nonScreenspaceDrawing_happensWith_drawConfigOf_hiddenByNearerObjects;
            }
        }

        void RefillArraysWithCursorNeighboringDatapointsForEachLine()
        {
            if (CheckIf_reconstructWithNewLength_theArrayWithLineSpecsForEachLine())
            {
                ReconstructWithNewLength_theArrayWithLineSpecsForEachLine();
            }

            for (int i = 0; i < specsForInspector_forEachDrawnLine.Length; i++)
            {
                specsForInspector_forEachDrawnLine[i].TryRefill_arrayWithNeighboringDatapointValues();
            }
        }

        bool CheckIf_reconstructWithNewLength_theArrayWithLineSpecsForEachLine()
        {
            int current_numberOfAllHiddenAndUnhiddenLines_withAtLeastOneValidOrInvalidDataPoint = chart_thisInspectorIsAttachedTo.lines.Get_numberOf_allHiddenAndUnhiddenLines_withAtLeastOneValidOrInvalidDatapoint(false);
            if (current_numberOfAllHiddenAndUnhiddenLines_withAtLeastOneValidOrInvalidDataPoint != specsForInspector_forEachDrawnLine.Length)
            {
                return true;
            }
            else
            {
                for (int i_line = 0; i_line < specsForInspector_forEachDrawnLine.Length; i_line++)
                {
                    if (specsForInspector_forEachDrawnLine[i_line].line_theseSpecsBelongTo.IsHiddenOrUnhidden_butWithAtLeastOneValidOrInvalidDatapoint() == false)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        void ReconstructWithNewLength_theArrayWithLineSpecsForEachLine()
        {
            List<ChartLine> currentlyActiveLines = chart_thisInspectorIsAttachedTo.lines.Get_all_hiddenAndUnhiddenLines_withAtLeastOneValidOrInvalidDatapoint(false);
            specsForInspector_forEachDrawnLine = new InternalDXXL_LineSpecsForChartInspector[currentlyActiveLines.Count];
            bool firstNonHiddenLineHasAlreadyBeenFound = false;
            for (int i = 0; i < specsForInspector_forEachDrawnLine.Length; i++)
            {
                specsForInspector_forEachDrawnLine[i] = currentlyActiveLines[i].lineSpecsForInspector;
                specsForInspector_forEachDrawnLine[i].i_ofThisLineSpec_insideChartInspectorsLinesSpecsArray = i;
                specsForInspector_forEachDrawnLine[i].Recalc_posThatIsXNearestToCursor(curr_cursorPosition_inChartspaceUnits);

                if (firstNonHiddenLineHasAlreadyBeenFound == false)
                {
                    if (specsForInspector_forEachDrawnLine[i].currentHideLineState == false)
                    {
                        specsForInspector_forEachDrawnLine[i].currentHideCursorXState_duringComponentInspectionPhase = false;
                        firstNonHiddenLineHasAlreadyBeenFound = true;
                    }
                }
            }
        }

        void SetTransformToChartPos()
        {
            transform.position = chart_thisInspectorIsAttachedTo.Position_worldspace;
            transform.rotation = chart_thisInspectorIsAttachedTo.InternalRotation;
            transform.localScale = new Vector3(chart_thisInspectorIsAttachedTo.Width_inWorldSpace, chart_thisInspectorIsAttachedTo.Height_inWorldSpace, 1.0f);
        }

        void TryPlaceSceneviewCam()
        {
#if UNITY_EDITOR
            UnityEditor.SceneView activeSceneView = null;
            if (UnityEditor.SceneView.currentDrawingSceneView != null)
            {
                activeSceneView = UnityEditor.SceneView.currentDrawingSceneView;
            }
            else
            {
                if (UnityEditor.SceneView.lastActiveSceneView != null)
                {
                    activeSceneView = UnityEditor.SceneView.lastActiveSceneView;
                }
            }

            if (activeSceneView != null)
            {
                TrySetSceneViewCamToChart_dueToCheckedForceStayToggle(activeSceneView);
            }
#endif
        }

#if UNITY_EDITOR
        public void TrySetSceneViewCamToChart_dueToButtonClick()
        {
            if (UnityEditor.SceneView.lastActiveSceneView != null)
            {
                TryNote_chartWorldSpacePosRotScale_beforeChangingItToScreenspace();
                try
                {
                    if (theChartIsDrawnInScreenspace) { UtilitiesDXXL_ChartDrawing.SetPosRotScaleOfChart_toScreenspace(chart_thisInspectorIsAttachedTo, screenSpaceTargetCamera, chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight, true); }
                    SetSceneViewCameraPos(UnityEditor.SceneView.lastActiveSceneView, false);
                }
                catch { }
                TrySetBack_chartWorldSpacePosRotScale_afterUsingItInScreenspace();

                UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            }
        }

        void TrySetSceneViewCamToChart_dueToCheckedForceStayToggle(UnityEditor.SceneView activeSceneView)
        {
            if (forceSceneViewCamToFollowChart == true)
            {
                SetSceneViewCameraPos(activeSceneView, true);
            }
        }

        void SetSceneViewCameraPos(UnityEditor.SceneView concernedSceneView, bool useSliderValue_forDistanceToChart)
        {
            //could be refactored using "SceneView.Frame()" or "SceneView.FrameSelected()"
            concernedSceneView.pivot = chart_thisInspectorIsAttachedTo.GetCenterPos();
            concernedSceneView.rotation = chart_thisInspectorIsAttachedTo.InternalRotation;
            if (useSliderValue_forDistanceToChart)
            {
                concernedSceneView.size = chart_thisInspectorIsAttachedTo.GetDiagonalSize() * (0.04f + (1.0f - sizeArbUnits_ofSceneViewCam));
            }
            else
            {
                concernedSceneView.size = chart_thisInspectorIsAttachedTo.GetDiagonalSize() * (0.04f + (1.0f - default_sizeArbUnits_ofSceneViewCam));
            }
        }

#endif

        public bool ContainsOnly1Line_hiddenOrUnhidden()
        {
            return (specsForInspector_forEachDrawnLine.Length == 1);
        }

        public bool AllOtherLinesAreHidden(int i_requestingLine)
        {
            for (int i = 0; i < specsForInspector_forEachDrawnLine.Length; i++)
            {
                if (i != i_requestingLine)
                {
                    if (specsForInspector_forEachDrawnLine[i].currentHideLineState == false) { return false; }
                }
            }
            return true;
        }

        public bool AllOtherCursorsAreHidden(int i_requestingLine)
        {
            for (int i = 0; i < specsForInspector_forEachDrawnLine.Length; i++)
            {
                if (i != i_requestingLine)
                {
                    if (specsForInspector_forEachDrawnLine[i].currentHideCursorXState_duringComponentInspectionPhase == false) { return false; }
                    if (specsForInspector_forEachDrawnLine[i].currentHideCursorYState_duringComponentInspectionPhase == false) { return false; }
                }
            }
            return true;
        }

        public void HideAllOtherLines(int i_ofOnlyNonHiddenLine)
        {
            for (int i = 0; i < specsForInspector_forEachDrawnLine.Length; i++)
            {
                if (i == i_ofOnlyNonHiddenLine)
                {
                    specsForInspector_forEachDrawnLine[i].currentHideLineState = false;
                    specsForInspector_forEachDrawnLine[i].currentHideCursorXState_duringComponentInspectionPhase = false;
                    specsForInspector_forEachDrawnLine[i].currentHideCursorYState_duringComponentInspectionPhase = false;
                }
                else
                {
                    specsForInspector_forEachDrawnLine[i].currentHideLineState = true;
                }
            }
        }

        public void HideAllOtherCursors(int i_ofOnlyNonCursorHiddenLine)
        {
            for (int i = 0; i < specsForInspector_forEachDrawnLine.Length; i++)
            {
                if (i == i_ofOnlyNonCursorHiddenLine)
                {
                    specsForInspector_forEachDrawnLine[i].currentHideCursorXState_duringComponentInspectionPhase = false;
                    specsForInspector_forEachDrawnLine[i].currentHideCursorYState_duringComponentInspectionPhase = false;
                }
                else
                {
                    specsForInspector_forEachDrawnLine[i].currentHideCursorXState_duringComponentInspectionPhase = true;
                    specsForInspector_forEachDrawnLine[i].currentHideCursorYState_duringComponentInspectionPhase = true;
                }
            }
        }
        public void UnhideAllLines()
        {
            for (int i = 0; i < specsForInspector_forEachDrawnLine.Length; i++)
            {
                specsForInspector_forEachDrawnLine[i].currentHideLineState = false;
            }
        }

        public void UnhideAllCursors(int i_singleLineThatHasXCursorEnabled)
        {
            for (int i = 0; i < specsForInspector_forEachDrawnLine.Length; i++)
            {
                if (i == i_singleLineThatHasXCursorEnabled)
                {
                    specsForInspector_forEachDrawnLine[i].currentHideCursorXState_duringComponentInspectionPhase = false;
                    specsForInspector_forEachDrawnLine[i].currentHideCursorYState_duringComponentInspectionPhase = false;
                }
                else
                {
                    specsForInspector_forEachDrawnLine[i].currentHideCursorXState_duringComponentInspectionPhase = true;
                    specsForInspector_forEachDrawnLine[i].currentHideCursorYState_duringComponentInspectionPhase = false;
                }
            }
        }

        void TrySetLuminanceOfLineColors()
        {
            //Known issue:
            //-> Calling "Undo" after changing the slider value will undo the luminance change, but the slider will not be set back to it's previous position.
            //-> It may have something to do with Unitys "Undo Groups"...what is inside such a group? How can it be inspected?
            //-> It seems in this case that the Undo-Actions sets back
            //---> "prev_luminanceOfLineColors_accordingToChartSetting"
            //---> "prev_luminanceOfLineColors_accordingToSlider"
            //---> "curr_luminanceOfLineColors_accordingToSlider"
            //---> The color values of the lines
            //-> But it does not set back the property value of "chart_thisInspectorIsAttachedTo.LuminanceOfLineColors". So this unchanged "chart_thisInspectorIsAttachedTo.LuminanceOfLineColors" will revert the slider to the position before the undo, but will not revert the colorChangeMadeByUndo.
            //-> If it is done multiple times this can lead to line colors that get shifted out of the Luminance0to1-Range and stay white/black onFutureLuminanceChanges. 

            if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(prev_luminanceOfLineColors_accordingToChartSetting, chart_thisInspectorIsAttachedTo.LuminanceOfLineColors))
            {
                //luminance has not been set via API call:
                if (false == UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(prev_luminanceOfLineColors_accordingToSlider, curr_luminanceOfLineColors_accordingToSlider))
                {
                    //luminance has been set via slider:
                    chart_thisInspectorIsAttachedTo.LuminanceOfLineColors = curr_luminanceOfLineColors_accordingToSlider;
                    prev_luminanceOfLineColors_accordingToSlider = curr_luminanceOfLineColors_accordingToSlider;
                }
            }
            else
            {
                //luminance has been set via API call:
                curr_luminanceOfLineColors_accordingToSlider = chart_thisInspectorIsAttachedTo.LuminanceOfLineColors; //-> "Clamp01" has already been made inside "chart_thisInspectorIsAttachedTo.LuminanceOfLineColors"
                prev_luminanceOfLineColors_accordingToSlider = curr_luminanceOfLineColors_accordingToSlider;
            }
            prev_luminanceOfLineColors_accordingToChartSetting = chart_thisInspectorIsAttachedTo.LuminanceOfLineColors;
        }

        void SetLineNamePositionsAndSize()
        {
            //-> not yet caring to set back the previous values of the lines after deletion of component, if the values have been different from line to line.
            chart_thisInspectorIsAttachedTo.SetLineNamesPositions(lineNamePositions);
            chart_thisInspectorIsAttachedTo.SetLineNamesSize(lineNames_sizeScaleFactor);
        }

        public bool DatapointVisualizerOfLineAreInvisible(int i_concernedLine)
        {
            return (specsForInspector_forEachDrawnLine[i_concernedLine].dataPointVisualization == ChartLine.DataPointVisualization.invisible);
        }

        public void ExportCSVFile(string fileName)
        {
            chart_thisInspectorIsAttachedTo.ExportToCSVfile(fileName);
        }

        public int Get_maxDisplayedPointOfInterestTextBoxesPerSide()
        {
            return chart_thisInspectorIsAttachedTo.MaxDisplayedPointOfInterestTextBoxesPerSide;
        }

        public void Set_maxDisplayedPointOfInterestTextBoxesPerSide(int newValue)
        {
            chart_thisInspectorIsAttachedTo.MaxDisplayedPointOfInterestTextBoxesPerSide = newValue;
        }

        public void ClearLineData()
        {
            chart_thisInspectorIsAttachedTo.Clear();
        }

        bool TrySkipDrawingBecauseItIsTheFirstFrameAfterAPausePhase()
        {
            //returns "doSkipDrawing"

            //-> this prevents drawing in the first frame after pause phases, to prevent the additional frozen overdraw during pause phases, caused by using "Debug.DrawLine()", which doesn't get cleared during pause phases.
            //-> see also "TrySheduleAutomaticFrameStepAtTheStartOfPausePhases()"
            //-> for more details: See documentation of "DrawCharts.chartInspectorComponentsAutomaticallyProceedOneFrameStepOnPauseStarts_toPreventFrozenOverlayDraw"
            //-> it also preservers the ability of the user to use the "Step"-functionality via the right one of the three play/pause buttons on top of the Unity window.

            //-> addendum: It seems that this function isn't necessary at all, because if you click on "Step" during a pausePhase, then one "Update/LateUpdate"-cycle is executed (as if there would be a short phase of "pause == false"), but "UnityEditor.EditorApplication.isPaused" doesn't become "false" in this step-frame. So the "LateUpdate()" of this class will anyway not call this "TrySkipDrawingBecauseItIsTheFirstFrameAfterAPausePhase()" function. But it doesn't harm, and it is maybe a good preparation if Unity version appear that DO set "UnityEditor.EditorApplication.isPaused" to "false" during this Step-Update-Cyle.

            if (isFirstUpdateCycleAfterGamePause)
            {
                isFirstUpdateCycleAfterGamePause = false;
                return true;
            }
            else
            {
                isFirstUpdateCycleAfterGamePause = false;
                return false;
            }
        }

        void TrySheduleAutomaticFrameStepAtTheStartOfPausePhases()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying && UnityEditor.EditorApplication.isPaused)
            {
                isFirstUpdateCycleAfterGamePause = true; //-> prepare for upcoming Update-cycles

                //-> the additional frame gives the component the chance to skip drawing with "Debug.DrawLine()" in the frame before pause phases, so there are no frozen uncleared debugLines present during the pause phase
                //-> see also "TrySkipDrawingBecauseItIsTheFirstFrameAfterAPausePhase()"
                //-> for more details: See documentation of "DrawCharts.chartInspectorComponentsAutomaticallyProceedOneFrameStepOnPauseStarts_toPreventFrozenOverlayDraw"
                if (mostCurrentFrameCountDuringGamePause <= (Time.frameCount - 2))
                {
                    //-> is first arrival after pausing the game
                    //-> at least two Update cycles happened since the previous game pause (or alternatively the playmode has been startet with "pause" already activated)
                    //-> cannot arrive here after proceeding only a single frame step between two pause phases
                    //-> related topic: "DXXLWrapperForUntiysBuildInDrawLines.ChooseDebugOrGizmoLines_dependingOnPlayModeState()"
                    //("Time.frameCount" doesn't increase during pause phases)
                    UtilitiesDXXL_Components.frameCount_forWhichAnEditorFrameStep_hasBeenSheduled_byAChart = Time.frameCount; //-> sheduling, because calling "UnityEditor.EditorApplication.Step()" from here causes Unity to print "recursive GUI rendering" errors
                }
                mostCurrentFrameCountDuringGamePause = Time.frameCount;
            }
#endif
        }

        bool TryDestroyThisComponentIfItWasManuallyCreated()
        {
            //returns "hasBeenDestroyed", which is the same as "shouldStopExecutingTheCallingFunction"
#if UNITY_EDITOR
            if (hasBeenManuallyCreated == true)
            {
                Debug.LogError("Do not create 'Draw XXL Chart Inspector' manually. Instead call 'CreateChartInspectionGameobject()' from script on the chart you want to inspect.");
                if (editorUpdateCallback_hasBeenRegistered)
                {
                    UnityEditor.EditorApplication.update -= EditorUpdateCallback;
                    editorUpdateCallback_hasBeenRegistered = false;
                }

                DeleteThisInspectorComponent();
                return true;
            }
            else
            {
                return false;
            }
#else
            return true;
#endif
        }

        bool TryDestroyComonent_ifChartGotLost()
        {
            //returns "shouldStopExecutingTheCallingFunction"
            //losing the chart reference happens in some cases, e.g. when a chart inspector has been created outside playmode and then playmode is entered, which destroys the referenced "chart_thisInspectorIsAttachedTo"

            if (chart_thisInspectorIsAttachedTo == null)
            {
#if UNITY_EDITOR
                if (transform.childCount == 0)
                {
                    if (transform.parent == null)
                    {
                        Component[] otherComponentsOnThisGameobject = GetComponents<Component>(); //this is expensive, but it is only in the rare case where a user has taken the auto-generated ChartInspectorGameobject and attached own additional components or did parenting of some sort. And it indicates a state that anyway should be resolved and should not be permanently there.
                        if (otherComponentsOnThisGameobject != null)
                        {
                            if (otherComponentsOnThisGameobject.Length <= 2) //only 2 components: "Transform" and "Draw XXL Chart Inspector"
                            {
                                //This is the expected case: The gameobject that hosts the chartInspectionComponent has not been modified by the user, that means no other components have been added and no parenting has been applied.
                                //-> the whole gameobject can be safely destroyed, and doesn't remain in the chart as a forgotten object that does nothing
                                DeleteNotOnlyThisInspectorComponentButAlsoTheHostingGameobject();
                            }
                        }
                        else
                        {
                            UtilitiesDXXL_Log.PrintErrorCode("82");
                        }
                    }
                }
#else
                DeleteThisInspectorComponent();
#endif
                return true;
            }
            else
            {
                return false;
            }
        }

        void DeleteThisInspectorComponent()
        {
            if (Application.isPlaying)
            {
                Destroy(this);
            }
            else
            {
                DestroyImmediate(this);
            }
        }

        void DeleteNotOnlyThisInspectorComponentButAlsoTheHostingGameobject()
        {
            //Debug.Log("Draw XXL Chart Inspector gameobject has been automatically deleted because the referenced ChartDrawing got lost."); //is this auto-delete case too frequent to spam the users log with this message?
            if (Application.isPlaying)
            {
                Destroy(this.gameObject);
            }
            else
            {
                DestroyImmediate(this.gameObject);
            }
        }

        public bool CheckIfReferencedChartGotLost()
        {
            return (chart_thisInspectorIsAttachedTo == null);
        }

    }

}
