namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class PieChartDrawing
    {
        public enum RotationSource
        {
            screen,  
            screen_butVerticalInWorldSpace,   
            userDefinedFixedRotation 
        }
        public RotationSource rotationSource = RotationSource.screen; 

        public int internal_indexNumberOfPremadeChart = -1; //-> this is used internally for the automatic layout of the premade chart, whose position hasn't been explicitly set by the user.
        private Vector3 position_worldspace = Vector3.zero;  
        public Vector3 Position_worldspace
        {
            get
            {
                if (internal_indexNumberOfPremadeChart == (-1))
                {
                    return position_worldspace;
                }
                else
                {
                    return DrawCharts.GetAutoLayoutedPositionOfPremadePieChart(this);
                }
            }
            set
            {
                position_worldspace = value;
                internal_indexNumberOfPremadeChart = -1; //-> if a user sets the position, then the chart is not affected anymore by the automatic layouting of the premade charts.
            }
        }

        public Quaternion fixedRotation = Quaternion.identity;
        public Quaternion internalRotation = Quaternion.identity; //This is the internally used rotation. The user should use "fixedRotation" instead. "fixedRotation" does nothing but getting filled into this before drawing if "rotationSource=userDefinedFixedRotation"
        private float size_ofPieCircleDiameter = 1.0f; 
        public float Size_ofPieCircleDiameter
        {
            get { return size_ofPieCircleDiameter; }
            set { size_ofPieCircleDiameter = Mathf.Clamp(value, 0.001f, 100000.0f); }
        }

        public Vector2 position_inCamViewportspace = new Vector2(0.41f, 0.41f);
        private float size_ofPieCircleDiameter_relToCamViewport = 0.45f;
        public float Size_ofPieCircleDiameter_relToCamViewport
        {
            get { return size_ofPieCircleDiameter_relToCamViewport; }
            set { size_ofPieCircleDiameter_relToCamViewport = Mathf.Clamp(value, 0.01f, 2.0f); }
        }

        private Vector3 right_normalized;
        public Vector3 Right_normalized
        {
            get { return right_normalized; }
            set { Debug.LogError("Setting 'Right_normalized' directly is not supported. Use 'rotationSource' and 'fixedRotation' instead."); }
        }

        private Vector3 up_normalized;
        public Vector3 Up_normalized
        {
            get { return up_normalized; }
            set { Debug.LogError("Setting 'Up_normalized' directly is not supported. Use 'rotationSource' and 'fixedRotation' instead."); }
        }

        private Vector3 forward_normalized;
        public Vector3 Forward_normalized
        {
            get { return forward_normalized; }
            set { Debug.LogError("Setting 'Forward_normalized' directly is not supported. Use 'rotationSource' and 'fixedRotation' instead."); }
        }

        public float mostRecent_vertDistance_fromCircleCenter_toUpperBounderySquare = 0.0f;

        public enum SegmentSorting
        {
            decreasingSize_clockwise,
            decreasingSize_counterClockwise,
            creationOrder_clockwise, 
            creationOrder_counterClockwise
        }
        public SegmentSorting segmentSorting = SegmentSorting.decreasingSize_clockwise;
        public float angleDegCCfromUp_whereMainSegmentStarts = 0.0f; 
        private string title;
        private string title_insideMarkup;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                title_insideMarkup = "<sw=50000>" + title + "</sw>";
            }
        }
        public string subTitle; 
        static float default_luminanceOfSegmentColors = 0.5f;
        private float luminanceOfSegmentColors = default_luminanceOfSegmentColors;
        public float LuminanceOfSegmentColors
        {
            get { return luminanceOfSegmentColors; }
            set
            {
                value = Mathf.Clamp01(value);
                ReassignLuminanceToSegmentColors(luminanceOfSegmentColors, value);
                luminanceOfSegmentColors = value;
            }
        }

        public Color color = DrawBasics.defaultColor;  
        private float relSize_ofSegmentsNameTexts = 1.0f;
        public float RelSize_ofSegmentsNameTexts  
        {
            get { return relSize_ofSegmentsNameTexts; }
            set { relSize_ofSegmentsNameTexts = Mathf.Clamp(value, 0.01f, 10.0f); }
        }

        public int postDecimalPositions_ofPercentageDisplay = 1;

        public bool trackNegativeValues = false; //The pie chart cannot display negative values. Though it is possible that the value of a segment ends up in the negative area, e.g. by adding negative values via "AddValue()" or frequent calling of "DecrementValue()". The chart will not display segments with negative values, but it can keep track of negative values. Example: The "DecrementValue()" function is called five times on a segment. So the value is at "-5". If "trackNegativeValues" is true, then it needs 6 calls of "IncrementValue()" for the segment value to reach "1" (the first value where it can be displayed again in the chart). If "trackNegativeValues" is set to "false" then all negative values are discarded and it needs only one call of "IncrementValue" to reach the segment value of "1".
        private float percentageThreshold_belowWhichSegmentsGetCombinedInto_othersSection = 1.0f;
        public float PercentageThreshold_belowWhichSegmentsGetCombinedInto_othersSection
        {
            get { return percentageThreshold_belowWhichSegmentsGetCombinedInto_othersSection; }
            set { percentageThreshold_belowWhichSegmentsGetCombinedInto_othersSection = Mathf.Clamp(value, 0.01f, 99.0f); }
        }

        List<PieChartSegment> segments_inCreationOrder = new List<PieChartSegment>();
        List<PieChartSegment> segments_orderedForDrawing = new List<PieChartSegment>();
        List<PieChartSegment> segments_orderedWithDecreasingSize = new List<PieChartSegment>();
        List<InternalDXXL_PieAngleSpan> angleSpans_thatAreAlreadyCoveredWithText = new List<InternalDXXL_PieAngleSpan>();
        PieChartSegment combinedOthersSegment;

        public bool mentionZeroSegmentsInLegend = true; //This determines if empty segments with value of 0 are displayed in the text legend column on the right side of the chart.
        public bool showSegmentNames = true;
        public bool showSegmentValues = true;
        public bool showSegmentPercentages = true;
        public bool autoFlipAllText_toFitObsererCamera = true;

        public PieChartDrawing(string title = null)
        {
            Title = title;
            combinedOthersSegment = new PieChartSegment("Others", this, 1000000);
            combinedOthersSegment.color = SeededColorGenerator.ForceApproxLuminance(Color.gray, default_luminanceOfSegmentColors);
        }

        public void Draw(float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            DXXLWrapperForUntiysBuildInDrawLines.currentlyDrawingPieChart = this;

            ApplyInternalRotation(); // <-"DXXLWrapperForUntiyDebugDraw.CheckIfDrawingIsCurrentlySkipped" uses the here applied rotation already in it's fallback

            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

            float sum_ofAllNonNegativeSegments = Get_sum_ofAllNonNegativeSegments();
            bool sum_ofAllNonNegativeSegments_isZero = UtilitiesDXXL_Math.ApproximatelyZero(sum_ofAllNonNegativeSegments);
            TryDrawNotificationForEmptyChart(sum_ofAllNonNegativeSegments_isZero, durationInSec, hiddenByNearerObjects);
            bool mentionOthersSection_inRightTextColumn = RecalcSegmentsDrawProperties(sum_ofAllNonNegativeSegments);
            if (sum_ofAllNonNegativeSegments_isZero == false) { SortSegmentsForDrawing(); }
            float vertOffset_ofNextTextBlock_fromRightColumnsTop = DrawSegments(mentionOthersSection_inRightTextColumn, durationInSec, hiddenByNearerObjects);

            float size_ofPieCircleRadius = 0.5f * size_ofPieCircleDiameter;
            float distance_circleCenter_toLeftBorder = size_ofPieCircleRadius + size_ofPieCircleRadius * 1.0f * relSize_ofSegmentsNameTexts;
            float distance_circleCenter_toRightBorder = size_ofPieCircleRadius + size_ofPieCircleRadius * 2.25f * relSize_ofSegmentsNameTexts;
            float distance_circleCenter_toLowEndOfTitleTexts = size_ofPieCircleRadius + size_ofPieCircleRadius * 0.65f * relSize_ofSegmentsNameTexts;
            float vertDistance_fromCircleCenter_toLowCenterPosOfNextTitleText = distance_circleCenter_toLowEndOfTitleTexts;
            mostRecent_vertDistance_fromCircleCenter_toUpperBounderySquare = TryDrawChartTitles(distance_circleCenter_toLeftBorder, distance_circleCenter_toRightBorder, vertDistance_fromCircleCenter_toLowCenterPosOfNextTitleText, durationInSec, hiddenByNearerObjects);
            DrawBoundarySquare(vertOffset_ofNextTextBlock_fromRightColumnsTop, distance_circleCenter_toLowEndOfTitleTexts, distance_circleCenter_toLeftBorder, distance_circleCenter_toRightBorder, size_ofPieCircleRadius, durationInSec, hiddenByNearerObjects);
            DXXLWrapperForUntiysBuildInDrawLines.currentlyDrawingPieChart = null;
        }

        public void DrawScreenspace(bool chartSize_isDefinedRelTo_cameraWidth_notCameraHeight = false, float durationInSec = 0.0f)
        {
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "PieChartDrawing.DrawScreenspace") == false) { return; }
            DrawScreenspace(automaticallyFoundCamera, chartSize_isDefinedRelTo_cameraWidth_notCameraHeight, durationInSec);
        }

        public void DrawScreenspace(Camera targetCamera, bool chartSize_isDefinedRelTo_cameraWidth_notCameraHeight = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return; }

            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_DrawScreenspacePieChart.Add(new DrawScreenspacePieChart(targetCamera,  chartSize_isDefinedRelTo_cameraWidth_notCameraHeight ,  durationInSec, this));
                return;
            }

            Vector3 chartPosition_beforeScreenspaceDrawing = Position_worldspace;
            Quaternion fixedChartRotation_beforeScreenspaceDrawing = fixedRotation;
            float size_ofPieCircleDiameter_beforeScreenspaceDrawing = size_ofPieCircleDiameter;
            RotationSource rotationSource_beforeScreenspaceDrawing = rotationSource;
            autoFlipAllText_toFitObsererCamera = false;

            try
            {
                Position_worldspace = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(targetCamera, position_inCamViewportspace, false);
                fixedRotation = targetCamera.transform.rotation;
                rotationSource = RotationSource.userDefinedFixedRotation;
                if (chartSize_isDefinedRelTo_cameraWidth_notCameraHeight)
                {
                    size_ofPieCircleDiameter = UtilitiesDXXL_Screenspace.HorizExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, InternalDXXL_BoundsCamViewportSpace.viewportCenter, true, size_ofPieCircleDiameter_relToCamViewport);
                }
                else
                {
                    size_ofPieCircleDiameter = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, InternalDXXL_BoundsCamViewportSpace.viewportCenter, true, size_ofPieCircleDiameter_relToCamViewport);
                }
                Draw(durationInSec, false);
            }
            catch { }

            Position_worldspace = chartPosition_beforeScreenspaceDrawing;
            fixedRotation = fixedChartRotation_beforeScreenspaceDrawing;
            size_ofPieCircleDiameter = size_ofPieCircleDiameter_beforeScreenspaceDrawing;
            rotationSource = rotationSource_beforeScreenspaceDrawing;
            autoFlipAllText_toFitObsererCamera = true;
        }

        void ApplyInternalRotation()
        {
            Vector3 observerCamForward_normalized;
            Vector3 observerCamUp_normalized;
            Vector3 observerCamRight_normalized;
            Vector3 cam_to_lineCenter;

            switch (rotationSource)
            {
                case RotationSource.screen:
                    UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter, Position_worldspace, Vector3.zero, null);
                    internalRotation = Quaternion.LookRotation(observerCamForward_normalized, observerCamUp_normalized);
                    break;
                case RotationSource.screen_butVerticalInWorldSpace:
                    UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter, Position_worldspace, Vector3.zero, null);
                    UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.GetUpAndTextDir_withoutCallerSpecifiedPreference_independentFromTooShortLineDir_alignedVertical(out Vector3 chartUp_normalized, out Vector3 chartRight_normalized, observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                    Vector3 chartForward_normalized = Vector3.Cross(chartRight_normalized, chartUp_normalized);
                    internalRotation = Quaternion.LookRotation(chartForward_normalized, chartUp_normalized);
                    break;
                case RotationSource.userDefinedFixedRotation:
                    internalRotation = fixedRotation;
                    break;
                default:
                    internalRotation = fixedRotation;
                    Debug.LogError("rotationSource of '" + rotationSource + "' not implemented.");
                    break;
            }

            right_normalized = internalRotation * Vector3.right;
            up_normalized = internalRotation * Vector3.up;
            forward_normalized = internalRotation * Vector3.forward;
        }

        float Get_sum_ofAllNonNegativeSegments()
        {
            float sum_ofAllNonNegativeSegments = 0.0f;
            for (int i_segment = 0; i_segment < segments_inCreationOrder.Count; i_segment++)
            {
                if (segments_inCreationOrder[i_segment].ValueAsFloat > 0.0f)
                {
                    sum_ofAllNonNegativeSegments = sum_ofAllNonNegativeSegments + segments_inCreationOrder[i_segment].ValueAsFloat;
                }
            }
            return sum_ofAllNonNegativeSegments;
        }

        void TryDrawNotificationForEmptyChart(bool sum_ofAllNonNegativeSegments_isZero, float durationInSec, bool hiddenByNearerObjects)
        {
            if (sum_ofAllNonNegativeSegments_isZero)
            {
                float autoLineBreakWidth_ofNoNonZeroSegmentsText = 1.8f * size_ofPieCircleDiameter;
                float size_ofNoNonZeroSegmentsText = 0.057f * size_ofPieCircleDiameter;
                UtilitiesDXXL_Text.Write("<size=60><color=#adadadFF><icon=logMessage></color></size><br>This pie chart doesn't contain<br>any segment with a value bigger<br>than 0.<br> ", Position_worldspace, color, size_ofNoNonZeroSegmentsText, right_normalized, up_normalized, DrawText.TextAnchorDXXL.LowerCenter, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, autoLineBreakWidth_ofNoNonZeroSegmentsText, autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects, false, false, true);
            }
        }

        bool RecalcSegmentsDrawProperties(float sum_ofAllNonNegativeSegments)
        {
            for (int i_segment = 0; i_segment < segments_inCreationOrder.Count; i_segment++)
            {
                segments_inCreationOrder[i_segment].CalcPercentage(sum_ofAllNonNegativeSegments);
            }

            int numberOfSegments_inCombinedOthersSection = 0;
            int numberOfNonZeroSegments_inCombinedOthersSection = 0;
            for (int i_segment = 0; i_segment < segments_inCreationOrder.Count; i_segment++)
            {
                if (segments_inCreationOrder[i_segment].isTooSmallToBeDrawn)
                {
                    numberOfSegments_inCombinedOthersSection++;
                    if (segments_inCreationOrder[i_segment].ValueAsFloat > 0.0f)
                    {
                        numberOfNonZeroSegments_inCombinedOthersSection++;
                    }
                }
            }

            combinedOthersSegment.SetValue(0.0f);
            combinedOthersSegment.isTooSmallToBeDrawn = true;
            bool mentionOthersSection_inRightTextColumn;

            if (numberOfSegments_inCombinedOthersSection <= 1)
            {
                mentionOthersSection_inRightTextColumn = false;
                for (int i_segment = 0; i_segment < segments_inCreationOrder.Count; i_segment++)
                {
                    segments_inCreationOrder[i_segment].textInRightColumn_shouldBeInsideTheOthersSection = false;
                    if (segments_inCreationOrder[i_segment].angleDeg_insidePie > 0.0f)
                    {
                        segments_inCreationOrder[i_segment].isTooSmallToBeDrawn = false;
                    }
                }
            }
            else
            {
                //-> "othersSection" contains at least two other segments
                //-> (but all these contained segments may be 0)

                for (int i_segment = 0; i_segment < segments_inCreationOrder.Count; i_segment++)
                {
                    if (segments_inCreationOrder[i_segment].isTooSmallToBeDrawn)
                    {
                        if (segments_inCreationOrder[i_segment].ValueAsFloat > 0.0f)
                        {
                            combinedOthersSegment.AddValue(segments_inCreationOrder[i_segment].ValueAsFloat);
                        }
                    }
                }

                combinedOthersSegment.CalcPercentage(sum_ofAllNonNegativeSegments);
                if (combinedOthersSegment.angleDeg_insidePie > 0.0f)
                {
                    combinedOthersSegment.isTooSmallToBeDrawn = false; //-> this overwrites "percentageThreshold_belowWhichSegmentsGetCombinedInto_othersSection"
                    mentionOthersSection_inRightTextColumn = true;
                }
                else
                {
                    if (mentionZeroSegmentsInLegend)
                    {
                        mentionOthersSection_inRightTextColumn = true;
                    }
                    else
                    {
                        mentionOthersSection_inRightTextColumn = false;
                    }
                }

                if (mentionZeroSegmentsInLegend)
                {
                    combinedOthersSegment.name = "" + numberOfSegments_inCombinedOthersSection + " Others";
                }
                else
                {
                    combinedOthersSegment.name = "" + numberOfNonZeroSegments_inCombinedOthersSection + " Others";
                }
            }

            return mentionOthersSection_inRightTextColumn;
        }

        float DrawSegments(bool mentionOthersSection_inRightTextColumn, float durationInSec, bool hiddenByNearerObjects)
        {
            //-> the order of calling of the following functions matters, because
            // 1) The elements are z-fighting. Later calls are drawn on top of earlier calls.
            // 2) The earlier calls fill class members inside "PieChartSegment" that are used by the later calls.

            Color color_ofLastDrawnSegment = TryDrawCircledLines(durationInSec, hiddenByNearerObjects);
            TryDrawStraightSegmentBorderLines(color_ofLastDrawnSegment, durationInSec, hiddenByNearerObjects);
            PrepareTextDrawing();
            TryDrawTextsBesideSegments(durationInSec, hiddenByNearerObjects);
            float vertOffset_ofNextTextBlock_fromRightColumnsTop = TryDrawTextsInRightColumn(mentionOthersSection_inRightTextColumn, durationInSec, hiddenByNearerObjects);
            return vertOffset_ofNextTextBlock_fromRightColumnsTop;
        }

        Color TryDrawCircledLines(float durationInSec, bool hiddenByNearerObjects)
        {
            Color color_ofPrecedingSegment = default;
            float startingAngleDegCCFromUp_ofCurrSegment = angleDegCCfromUp_whereMainSegmentStarts;
            for (int i_segment = 0; i_segment < segments_orderedForDrawing.Count; i_segment++)
            {
                startingAngleDegCCFromUp_ofCurrSegment = segments_orderedForDrawing[i_segment].TryDrawCircledLines(startingAngleDegCCFromUp_ofCurrSegment, durationInSec, hiddenByNearerObjects);
                if (segments_orderedForDrawing[i_segment].isTooSmallToBeDrawn == false) { color_ofPrecedingSegment = segments_orderedForDrawing[i_segment].color; }
            }
            combinedOthersSegment.TryDrawCircledLines(startingAngleDegCCFromUp_ofCurrSegment, durationInSec, hiddenByNearerObjects);
            if (combinedOthersSegment.isTooSmallToBeDrawn == false) { color_ofPrecedingSegment = combinedOthersSegment.color; }
            return color_ofPrecedingSegment;
        }

        void TryDrawStraightSegmentBorderLines(Color color_ofLastDrawnSegment, float durationInSec, bool hiddenByNearerObjects)
        {
            Color color_ofPrecedingSegment = color_ofLastDrawnSegment;
            for (int i_segment = 0; i_segment < segments_orderedForDrawing.Count; i_segment++)
            {
                segments_orderedForDrawing[i_segment].TryDrawStraightSegmentBorderLine(color_ofPrecedingSegment, durationInSec, hiddenByNearerObjects);
                if (segments_orderedForDrawing[i_segment].isTooSmallToBeDrawn == false) { color_ofPrecedingSegment = segments_orderedForDrawing[i_segment].color; }
            }
            combinedOthersSegment.TryDrawStraightSegmentBorderLine(color_ofPrecedingSegment, durationInSec, hiddenByNearerObjects);
        }

        void PrepareTextDrawing()
        {
            for (int i_segment = 0; i_segment < segments_orderedForDrawing.Count; i_segment++)
            {
                segments_orderedForDrawing[i_segment].PrepareTextDrawing();
            }
            combinedOthersSegment.PrepareTextDrawing();
        }

        void TryDrawTextsBesideSegments(float durationInSec, bool hiddenByNearerObjects)
        {
            angleSpans_thatAreAlreadyCoveredWithText.Clear();
            for (int i_segment = 0; i_segment < segments_orderedWithDecreasingSize.Count; i_segment++)
            {
                if (segments_orderedWithDecreasingSize[i_segment].isTooSmallToBeDrawn)
                {
                    segments_orderedWithDecreasingSize[i_segment].ReportThat_textShouldBeDrawnIntoRightColumn_becauseSpaceAtChartWasTooSmall();
                }
                else
                {
                    segments_orderedWithDecreasingSize[i_segment].TryDrawTextBesideSegment(durationInSec, hiddenByNearerObjects);
                }
            }

            if (combinedOthersSegment.ValueAsFloat > 0.0f)
            {
                combinedOthersSegment.TryDrawTextBesideSegment(durationInSec, hiddenByNearerObjects);
            }
            else
            {
                combinedOthersSegment.ReportThat_textShouldBeDrawnIntoRightColumn_becauseSpaceAtChartWasTooSmall();
            }
        }

        float TryDrawTextsInRightColumn(bool mentionOthersSection_inRightTextColumn, float durationInSec, bool hiddenByNearerObjects)
        {
            float vertOffset_ofNextTextBlock_fromRightColumnsTop = 0.0f;
            for (int i_segment = 0; i_segment < segments_orderedForDrawing.Count; i_segment++)
            {
                if (mentionOthersSection_inRightTextColumn == false) { segments_orderedForDrawing[i_segment].textInRightColumn_shouldBeInsideTheOthersSection = false; } //-> this is for the case where the othersSection would contain only 1 segment, and therefore this segment gets drawn instead of the otherSection. Without this it would not appear in the right column
                vertOffset_ofNextTextBlock_fromRightColumnsTop = segments_orderedForDrawing[i_segment].TryDrawTextInRightColumnOutsideOthersSection(false, vertOffset_ofNextTextBlock_fromRightColumnsTop, durationInSec, hiddenByNearerObjects);
            }

            if (mentionOthersSection_inRightTextColumn)
            {
                //-> others section contains at least two other segments
                //-> at least one contained segment ist non-zero OR "mentionZeroSegmentsInLegend" is activated

                vertOffset_ofNextTextBlock_fromRightColumnsTop = combinedOthersSegment.TryDrawTextInRightColumnOutsideOthersSection(true, vertOffset_ofNextTextBlock_fromRightColumnsTop, durationInSec, hiddenByNearerObjects);
                for (int i_segment = 0; i_segment < segments_orderedForDrawing.Count; i_segment++)
                {
                    vertOffset_ofNextTextBlock_fromRightColumnsTop = segments_orderedForDrawing[i_segment].TryDrawTextInRightColumnInsideOthersSection(vertOffset_ofNextTextBlock_fromRightColumnsTop, combinedOthersSegment.color, durationInSec, hiddenByNearerObjects);
                }
            }
            return vertOffset_ofNextTextBlock_fromRightColumnsTop;
        }

        float TryDrawChartTitles(float distance_circleCenter_toLeftBorder, float distance_circleCenter_toRightBorder, float vertDistance_fromCircleCenter_toLowCenterPosOfNextTitleText, float durationInSec, bool hiddenByNearerObjects)
        {
            float widthOfBoundarySquare = distance_circleCenter_toLeftBorder + distance_circleCenter_toRightBorder;
            float halfWidthOfBoundarySquare = 0.5f * widthOfBoundarySquare;
            float autoLineBreakWidth = 0.9f * widthOfBoundarySquare;

            if (subTitle != null && subTitle != "")
            {
                float size_ofSubTitleText = 0.06f * size_ofPieCircleDiameter;
                Vector3 lowCenterPos_ofNextTitleText = Position_worldspace + up_normalized * vertDistance_fromCircleCenter_toLowCenterPosOfNextTitleText + right_normalized * ((-distance_circleCenter_toLeftBorder) + halfWidthOfBoundarySquare);
                UtilitiesDXXL_Text.Write(subTitle, lowCenterPos_ofNextTitleText, color, size_ofSubTitleText, right_normalized, up_normalized, DrawText.TextAnchorDXXL.LowerCenter, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, autoLineBreakWidth, autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects, false, false, true);
                vertDistance_fromCircleCenter_toLowCenterPosOfNextTitleText = vertDistance_fromCircleCenter_toLowCenterPosOfNextTitleText + (DrawText.parsedTextSpecs.height_wholeTextBlock + size_ofSubTitleText);
            }

            if (title != null && title != "")
            {
                float size_ofTitleText = 0.18f * size_ofPieCircleDiameter;
                Vector3 lowCenterPos_ofNextTitleText = Position_worldspace + up_normalized * vertDistance_fromCircleCenter_toLowCenterPosOfNextTitleText + right_normalized * ((-distance_circleCenter_toLeftBorder) + halfWidthOfBoundarySquare);
                UtilitiesDXXL_Text.Write(title_insideMarkup, lowCenterPos_ofNextTitleText, color, size_ofTitleText, right_normalized, up_normalized, DrawText.TextAnchorDXXL.LowerCenter, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, autoLineBreakWidth, autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects, false, false, true);
                vertDistance_fromCircleCenter_toLowCenterPosOfNextTitleText = vertDistance_fromCircleCenter_toLowCenterPosOfNextTitleText + DrawText.parsedTextSpecs.height_wholeTextBlock;
            }

            return vertDistance_fromCircleCenter_toLowCenterPosOfNextTitleText;
        }

        void DrawBoundarySquare(float vertOffset_ofNextTextBlock_fromRightColumnsTop, float distance_circleCenter_toLowEndOfTitleTexts, float distance_circleCenter_toLeftBorder, float distance_circleCenter_toRightBorder, float size_ofPieCircleRadius, float durationInSec, bool hiddenByNearerObjects)
        {
            float offset_fromCircleCenter_toLowestPosOfRightColumn = Get_vertOffset_fromCircleCenter_toTopRightPosOfRightColumn() + vertOffset_ofNextTextBlock_fromRightColumnsTop;
            float offset_fromCircleCenter_toLowestPosOfRightColumnInclOffset = offset_fromCircleCenter_toLowestPosOfRightColumn - size_ofPieCircleRadius * 0.25f * relSize_ofSegmentsNameTexts;
            float minOffset_fromCircleCenter_toLowerBounderySquare = (-distance_circleCenter_toLowEndOfTitleTexts); //-> naming confusion: It contains "min", but the "min" would only be correct if the value would be an abs/positive value. Since this offset is always negative, it is actually the "max" value, but this would be an unintuitive name as well.

            Vector3 circleCenter_to_upperBorderOfBoundarySquare = up_normalized * mostRecent_vertDistance_fromCircleCenter_toUpperBounderySquare;
            Vector3 circleCenter_to_lowerBorderOfBoundarySquare = up_normalized * Mathf.Min(minOffset_fromCircleCenter_toLowerBounderySquare, offset_fromCircleCenter_toLowestPosOfRightColumnInclOffset);//-> since the searched value is always a negative one this is actually looking for the "max-negative".
            Vector3 circleCenter_to_leftBorderOfBoundarySquare = right_normalized * (-distance_circleCenter_toLeftBorder);
            Vector3 circleCenter_to_rightBorderOfBoundarySquare = right_normalized * distance_circleCenter_toRightBorder;

            Vector3 topLeftCorner_ofBoundarySquare = Position_worldspace + circleCenter_to_upperBorderOfBoundarySquare + circleCenter_to_leftBorderOfBoundarySquare;
            Vector3 topRightCorner_ofBoundarySquare = Position_worldspace + circleCenter_to_upperBorderOfBoundarySquare + circleCenter_to_rightBorderOfBoundarySquare;
            Vector3 lowLeftCorner_ofBoundarySquare = Position_worldspace + circleCenter_to_lowerBorderOfBoundarySquare + circleCenter_to_leftBorderOfBoundarySquare;
            Vector3 lowRightCorner_ofBoundarySquare = Position_worldspace + circleCenter_to_lowerBorderOfBoundarySquare + circleCenter_to_rightBorderOfBoundarySquare;

            Line_fadeableAnimSpeed.InternalDraw(topLeftCorner_ofBoundarySquare, topRightCorner_ofBoundarySquare, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(topRightCorner_ofBoundarySquare, lowRightCorner_ofBoundarySquare, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(lowRightCorner_ofBoundarySquare, lowLeftCorner_ofBoundarySquare, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(lowLeftCorner_ofBoundarySquare, topLeftCorner_ofBoundarySquare, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
        }

        public void AddValue(string segmentName, float addedValue)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                GetSegment(segmentName, true).AddValue(addedValue);
            }
        }

        public void AddValue(string segmentName, int addedValue)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                GetSegment(segmentName, true).AddValue(addedValue);
            }
        }

        public void AddValueToAllSegments(float addedValue)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                for (int i = 0; i < segments_inCreationOrder.Count; i++)
                {
                    segments_inCreationOrder[i].AddValue(addedValue);
                }
            }
        }

        public void AddValueToAllSegments(int addedValue)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                for (int i = 0; i < segments_inCreationOrder.Count; i++)
                {
                    segments_inCreationOrder[i].AddValue(addedValue);
                }
            }
        }

        public void IncrementValue(string segmentName)
        {
            GetSegment(segmentName, true).AddValue(1);
        }

        public void IncrementValuesOfAllSegments()
        {
            for (int i = 0; i < segments_inCreationOrder.Count; i++)
            {
                segments_inCreationOrder[i].AddValue(1);
            }
        }

        public void DecrementValue(string segmentName)
        {
            GetSegment(segmentName, true).AddValue(-1);
        }

        public void DecrementValuesOfAllSegments()
        {
            for (int i = 0; i < segments_inCreationOrder.Count; i++)
            {
                segments_inCreationOrder[i].AddValue(-1);
            }
        }

        public void SetValue(string segmentName, float newValue)
        {
            GetSegment(segmentName, true).SetValue(newValue);
        }

        public void SetValueOfAllSegments(float newValue)
        {
            for (int i = 0; i < segments_inCreationOrder.Count; i++)
            {
                segments_inCreationOrder[i].SetValue(newValue);
            }
        }

        public void SetValue(string segmentName, int newValue)
        {
            GetSegment(segmentName, true).SetValue(newValue);
        }

        public void SetValueOfAllSegments(int newValue)
        {
            for (int i = 0; i < segments_inCreationOrder.Count; i++)
            {
                segments_inCreationOrder[i].SetValue(newValue);
            }
        }

        public void AddValues_eachIndexIsASegment(List<float> valuesToAdd)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                for (int i = 0; i < valuesToAdd.Count; i++)
                {
                    GetSegment("i=" + i, true).AddValue(valuesToAdd[i]);
                }
            }
        }

        public void AddValues_eachIndexIsASegment(float[] valuesToAdd)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                for (int i = 0; i < valuesToAdd.Length; i++)
                {
                    GetSegment("i=" + i, true).AddValue(valuesToAdd[i]);
                }
            }
        }

        public void AddValues_eachIndexIsASegment(List<int> valuesToAdd)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                for (int i = 0; i < valuesToAdd.Count; i++)
                {
                    GetSegment("i=" + i, true).AddValue(valuesToAdd[i]);
                }
            }
        }

        public void AddValues_eachIndexIsASegment(int[] valuesToAdd)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                for (int i = 0; i < valuesToAdd.Length; i++)
                {
                    GetSegment("i=" + i, true).AddValue(valuesToAdd[i]);
                }
            }
        }

        public void IncrementValues_eachIndexIsASegment(List<bool> incrementedListSlots)
        {
            for (int i = 0; i < incrementedListSlots.Count; i++)
            {
                if (incrementedListSlots[i] == true)
                {
                    IncrementValue("i=" + i);
                }
            }
        }

        public void IncrementValues_eachIndexIsASegment(bool[] incrementedArraySlots)
        {
            for (int i = 0; i < incrementedArraySlots.Length; i++)
            {
                if (incrementedArraySlots[i] == true)
                {
                    IncrementValue("i=" + i);
                }
            }
        }

        public void DecrementValues_eachIndexIsASegment(List<bool> decrementedListSlots)
        {
            for (int i = 0; i < decrementedListSlots.Count; i++)
            {
                if (decrementedListSlots[i] == true)
                {
                    DecrementValue("i=" + i);
                }
            }
        }

        public void DecrementValues_eachIndexIsASegment(bool[] decrementedArraySlots)
        {
            for (int i = 0; i < decrementedArraySlots.Length; i++)
            {
                if (decrementedArraySlots[i] == true)
                {
                    DecrementValue("i=" + i);
                }
            }
        }

        public void SetValues_eachIndexIsASegment(List<float> valuesToSet)
        {
            for (int i = 0; i < valuesToSet.Count; i++)
            {
                GetSegment("i=" + i, true).SetValue(valuesToSet[i]);
            }
        }

        public void SetValues_eachIndexIsASegment(float[] valuesToSet)
        {
            for (int i = 0; i < valuesToSet.Length; i++)
            {
                GetSegment("i=" + i, true).SetValue(valuesToSet[i]);
            }
        }

        public void SetValues_eachIndexIsASegment(List<int> valuesToSet)
        {
            for (int i = 0; i < valuesToSet.Count; i++)
            {
                GetSegment("i=" + i, true).SetValue(valuesToSet[i]);
            }
        }

        public void SetValues_eachIndexIsASegment(int[] valuesToSet)
        {
            for (int i = 0; i < valuesToSet.Length; i++)
            {
                GetSegment("i=" + i, true).SetValue(valuesToSet[i]);
            }
        }

        public void Clear()
        {
            segments_inCreationOrder.Clear();
            segments_orderedForDrawing.Clear();
            segments_orderedWithDecreasingSize.Clear();
        }

        public PieChartSegment GetSegment(string segmentName, bool createSegmentIfItDoesntExist)
        {
            if (segmentName == null || segmentName == "")
            {
                Debug.LogError("'GetSegment()' failed, because the requested 'segmentName' is null or empty.");
                return null;
            }

            for (int i = 0; i < segments_inCreationOrder.Count; i++)
            {
                if (segments_inCreationOrder[i].name != null)
                {
                    if (segments_inCreationOrder[i].name == segmentName)
                    {
                        return segments_inCreationOrder[i];
                    }
                }
            }

            //A segment with this name doesn't exist yet:
            if (createSegmentIfItDoesntExist)
            {
                PieChartSegment newlyCreatedPieChartSegment = new PieChartSegment(segmentName, this, segments_inCreationOrder.Count);
                segments_inCreationOrder.Add(newlyCreatedPieChartSegment);
                segments_orderedForDrawing.Add(segments_inCreationOrder[segments_inCreationOrder.Count - 1]);
                segments_orderedWithDecreasingSize.Add(segments_inCreationOrder[segments_inCreationOrder.Count - 1]);
                TryReassignRainbowColorsToAllSegments();
                return segments_inCreationOrder[segments_inCreationOrder.Count - 1];
            }
            else
            {
                return null;
            }
        }

        public bool OrderingIsClockwise()
        {
            return ((segmentSorting == SegmentSorting.creationOrder_clockwise) || (segmentSorting == SegmentSorting.decreasingSize_clockwise));
        }

        void SortSegmentsForDrawing()
        {
            segments_orderedWithDecreasingSize.Sort(BiggerValueLeadsToLowerIndex);
            switch (segmentSorting)
            {
                case SegmentSorting.decreasingSize_clockwise:
                    segments_orderedForDrawing.Sort(BiggerValueLeadsToLowerIndex);
                    break;
                case SegmentSorting.decreasingSize_counterClockwise:
                    segments_orderedForDrawing.Sort(BiggerValueLeadsToLowerIndex);
                    break;
                case SegmentSorting.creationOrder_clockwise:
                    Fill_segments_fromListInCreationOrder_toListOrderedForDrawing();
                    break;
                case SegmentSorting.creationOrder_counterClockwise:
                    Fill_segments_fromListInCreationOrder_toListOrderedForDrawing();
                    break;
                default:
                    segments_orderedForDrawing.Sort(BiggerValueLeadsToLowerIndex);
                    Debug.LogError("segmentSorting of '" + segmentSorting + "' not implemented.");
                    break;
            }
        }

        int BiggerValueLeadsToLowerIndex(PieChartSegment segment1, PieChartSegment segment2)
        {
            if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(segment1.ValueAsFloat, segment2.ValueAsFloat))
            {
                //-> this prevents ongoing flickering change of the order of (zero) segments inside the right column
                if (segment1.i_ofThisSegment_insideCreationOrderedList < segment2.i_ofThisSegment_insideCreationOrderedList)
                {
                    return (-1);
                }
                else
                {
                    return (1);
                }
            }
            else
            {
                if (segment1.ValueAsFloat > segment2.ValueAsFloat)
                {
                    return (-1);
                }
                else
                {
                    return (1);
                }
            }
        }

        void Fill_segments_fromListInCreationOrder_toListOrderedForDrawing()
        {
            for (int i = 0; i < segments_inCreationOrder.Count; i++)
            {
                segments_orderedForDrawing[i] = segments_inCreationOrder[i];
            }
        }

        public bool CheckIfAngleForTextIsAlreadyCovered(float startAngleDegCCFromUp_ofText, float endAngleDegCCFromUp_ofText)
        {
            for (int i = 0; i < angleSpans_thatAreAlreadyCoveredWithText.Count; i++)
            {
                if (angleSpans_thatAreAlreadyCoveredWithText[i].DoesIntersect(startAngleDegCCFromUp_ofText, endAngleDegCCFromUp_ofText))
                {
                    return true;
                }
            }
            return false;
        }

        public void MarkAngleSpanAsCoveredWithText(float startAngleDegCCFromUp_ofText, float endAngleDegCCFromUp_ofText)
        {
            InternalDXXL_PieAngleSpan reservedAngleSpan = new InternalDXXL_PieAngleSpan();
            reservedAngleSpan.startAngleDegCCFromUp = startAngleDegCCFromUp_ofText;
            reservedAngleSpan.endAngleDegCCFromUp = endAngleDegCCFromUp_ofText;
            angleSpans_thatAreAlreadyCoveredWithText.Add(reservedAngleSpan);

            if (startAngleDegCCFromUp_ofText < 0.0f)
            {
                InternalDXXL_PieAngleSpan reservedAngleSpan_copy1 = new InternalDXXL_PieAngleSpan();
                reservedAngleSpan_copy1.startAngleDegCCFromUp = startAngleDegCCFromUp_ofText + 360.0f;
                reservedAngleSpan_copy1.endAngleDegCCFromUp = endAngleDegCCFromUp_ofText + 360.0f;
                angleSpans_thatAreAlreadyCoveredWithText.Add(reservedAngleSpan_copy1);
            }

            if (endAngleDegCCFromUp_ofText > 360.0f)
            {
                InternalDXXL_PieAngleSpan reservedAngleSpan_copy2 = new InternalDXXL_PieAngleSpan();
                reservedAngleSpan_copy2.startAngleDegCCFromUp = startAngleDegCCFromUp_ofText - 360.0f;
                reservedAngleSpan_copy2.endAngleDegCCFromUp = endAngleDegCCFromUp_ofText - 360.0f;
                angleSpans_thatAreAlreadyCoveredWithText.Add(reservedAngleSpan_copy2);
            }
        }

        public float Get_vertOffset_fromCircleCenter_toTopRightPosOfRightColumn()
        {
            float size_ofPieCircleRadius = 0.5f * size_ofPieCircleDiameter;
            return (size_ofPieCircleRadius + size_ofPieCircleRadius * 0.4f * relSize_ofSegmentsNameTexts);
        }

        int numberOfSegments_inMomentOfLastAutomaticRainbowColorReassignment = 0;
        int colorSeedFactor = 11; //if this is set to 1, then the segment colors are one continuous color spectrum pass from the first created segment to the last created segment. This may help to indicate the order in which the segments were created, but in the not-so-seldom-case where the segments size is also ordered (like "first creeated segment is biggest/smallest), then it gets harder to distinguish the color of the segments and associate them with the corresponding text display, since neighboring colors get more and more similar. Setting this "colorSeedFactor" value to something else than 1 "scatters" the color distribution, so that chances are higher that neighboring segments have a clearly different color.
        void TryReassignRainbowColorsToAllSegments()
        {
            for (int i_segment = 0; i_segment < segments_inCreationOrder.Count; i_segment++)
            {
                bool isTheNewlyCreatedSegment = (i_segment == (segments_inCreationOrder.Count - 1));
                if (isTheNewlyCreatedSegment)
                {
                    segments_inCreationOrder[i_segment].color = SeededColorGenerator.GetRainbowColor(i_segment * colorSeedFactor, 1.0f, segments_inCreationOrder.Count, luminanceOfSegmentColors);
                }
                else
                {
                    Color expectedColorOfSegment_beforeCurrReassignment_ifColorWasAutogenerated = SeededColorGenerator.GetRainbowColor(i_segment * colorSeedFactor, 1.0f, numberOfSegments_inMomentOfLastAutomaticRainbowColorReassignment, luminanceOfSegmentColors);
                    bool colorWasManuallySetByUser = (UtilitiesDXXL_Colors.IsApproxSameColor(segments_inCreationOrder[i_segment].color, expectedColorOfSegment_beforeCurrReassignment_ifColorWasAutogenerated) == false);
                    if (colorWasManuallySetByUser == false)
                    {
                        segments_inCreationOrder[i_segment].color = SeededColorGenerator.GetRainbowColor(i_segment * colorSeedFactor, 1.0f, segments_inCreationOrder.Count, luminanceOfSegmentColors);
                    }
                }
            }
            numberOfSegments_inMomentOfLastAutomaticRainbowColorReassignment = segments_inCreationOrder.Count;
        }

        void ReassignLuminanceToSegmentColors(float oldLuminance, float newLuminance)
        {
            Color colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated;
            Color colorToAssign_ifAutogenerated;

            for (int i_segment = 0; i_segment < segments_inCreationOrder.Count; i_segment++)
            {
                colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetRainbowColor(i_segment * colorSeedFactor, 1.0f, numberOfSegments_inMomentOfLastAutomaticRainbowColorReassignment, oldLuminance);
                colorToAssign_ifAutogenerated = SeededColorGenerator.GetRainbowColor(i_segment * colorSeedFactor, 1.0f, numberOfSegments_inMomentOfLastAutomaticRainbowColorReassignment, newLuminance);
                ReassignLuminanceToSegmentColor(segments_inCreationOrder[i_segment], colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);
            }

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.ForceApproxLuminance(Color.gray, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.ForceApproxLuminance(Color.gray, newLuminance);
            ReassignLuminanceToSegmentColor(combinedOthersSegment, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);
        }

        void ReassignLuminanceToSegmentColor(PieChartSegment concernedSegment, Color colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, Color colorToAssign_ifAutogenerated, float newLuminance)
        {
            bool colorWasAutogenerated = UtilitiesDXXL_Colors.IsApproxSameColor(concernedSegment.color, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated);
            if (colorWasAutogenerated)
            {
                concernedSegment.color = colorToAssign_ifAutogenerated;
            }
            else
            {
                //color has been manually set by user:
                if (UtilitiesDXXL_Math.ApproximatelyZero(concernedSegment.color.r) && UtilitiesDXXL_Math.ApproximatelyZero(concernedSegment.color.g) && UtilitiesDXXL_Math.ApproximatelyZero(concernedSegment.color.b))
                {
                    //-> black colors cannot be forced with luminance, therefore: slight lift, to make it grey, which can be forced:
                    concernedSegment.color = new Color(0.01f, 0.01f, 0.01f, concernedSegment.color.a);
                }
                concernedSegment.color = SeededColorGenerator.ForceApproxLuminance(concernedSegment.color, newLuminance);
            }
        }

        public void DrawWarningForMaxLinesPerFrame()
        {
            string warningText = "<color=#e2aa00FF><size=33><icon=warning></size></color><br><color=red>Max lines exceeded<br>(see log)</color><br> ";
            float size_ofMaxLinesText = 0.09f * size_ofPieCircleDiameter;
            UtilitiesDXXL_Text.WriteFramed(warningText, Position_worldspace, color, size_ofMaxLinesText, internalRotation, DrawText.TextAnchorDXXL.LowerCenter, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, autoFlipAllText_toFitObsererCamera, 0.0f, false);
        }

    }

}
