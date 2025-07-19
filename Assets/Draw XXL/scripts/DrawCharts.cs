namespace DrawXXL
{
    using UnityEngine;

    public class DrawCharts
    {
       
        public static bool chartInspectorComponentsAutomaticallyProceedOneFrameStepOnPauseStarts_toPreventFrozenOverlayDraw = true;

        public static ChartDrawing premadeChart0; 
        public static ChartDrawing premadeChart1;
        public static ChartDrawing premadeChart2;
        public static ChartDrawing premadeChart3;
        public static ChartDrawing premadeChart4;
        public static ChartDrawing premadeChart5;
        public static ChartDrawing premadeChart6;
        public static ChartDrawing premadeChart7;
        public static ChartDrawing premadeChart8;
        public static ChartDrawing premadeChart9;

        public static PieChartDrawing premadePieChart0;
        public static PieChartDrawing premadePieChart1; 
        public static PieChartDrawing premadePieChart2;
        public static PieChartDrawing premadePieChart3;
        public static PieChartDrawing premadePieChart4;
        public static PieChartDrawing premadePieChart5;
        public static PieChartDrawing premadePieChart6;
        public static PieChartDrawing premadePieChart7;
        public static PieChartDrawing premadePieChart8; 
        public static PieChartDrawing premadePieChart9;

        public static Vector3 positionOfPremadeCharts = Vector3.zero;
        public static int premadeChartThatIsRotationPivot = 0;

        const float default_widthOfPremadeCharts = 1.7f;
        const float default_heightOfPremadeCharts = 0.8f;
        const float default_diameterOfPremadePieCharts = 0.65f;
        static float factor_fromLineChartWidth_toPieChartDiameter_soThatTheyAppearWithApproxTheSameWidth = 0.382353f;
        static float current_widthOfPremadeCharts = default_widthOfPremadeCharts;
        static float current_heightOfPremadeCharts = default_heightOfPremadeCharts;

        static float xOffsetBetweenPremadeCharts_caseOf_onlyPieCharts = 1.06f;
    //    static float xOffsetBetweenPremadeCharts_caseOf_noNeedFor_pointOfInterestTextBoxes = 1.35f;
        static float xOffsetBetweenPremadeCharts_caseOf_noNeedFor_pointOfInterestTextBoxes = 1.25f;
        static float xOffsetBetweenPremadeCharts_caseOf_needs_pointOfInterestTextBoxes_onOneSide = 1.85f;
        static float xOffsetBetweenPremadeCharts_caseOf_needs_pointOfInterestTextBoxes_onBothSides = 2.44f;
        static float current_horizontalDistance_fromChartToChart_relativeToChartWidth = xOffsetBetweenPremadeCharts_caseOf_noNeedFor_pointOfInterestTextBoxes;
        static float used_xOffsetBetweenPremadeCharts = default_widthOfPremadeCharts * current_horizontalDistance_fromChartToChart_relativeToChartWidth;

        static DrawCharts()
        {
            premadeChart0 = new ChartDrawing("premade chart #0");
            premadeChart1 = new ChartDrawing("premade chart #1");
            premadeChart2 = new ChartDrawing("premade chart #2");
            premadeChart3 = new ChartDrawing("premade chart #3");
            premadeChart4 = new ChartDrawing("premade chart #4");
            premadeChart5 = new ChartDrawing("premade chart #5");
            premadeChart6 = new ChartDrawing("premade chart #6");
            premadeChart7 = new ChartDrawing("premade chart #7");
            premadeChart8 = new ChartDrawing("premade chart #8");
            premadeChart9 = new ChartDrawing("premade chart #9");

            premadePieChart0 = new PieChartDrawing("pie chart #0");
            premadePieChart1 = new PieChartDrawing("pie chart #1");
            premadePieChart2 = new PieChartDrawing("pie chart #2");
            premadePieChart3 = new PieChartDrawing("pie chart #3");
            premadePieChart4 = new PieChartDrawing("pie chart #4");
            premadePieChart5 = new PieChartDrawing("pie chart #5");
            premadePieChart6 = new PieChartDrawing("pie chart #6");
            premadePieChart7 = new PieChartDrawing("pie chart #7");
            premadePieChart8 = new PieChartDrawing("pie chart #8");
            premadePieChart9 = new PieChartDrawing("pie chart #9");

            RevertPremadeChartsToAutomaticPositionLayout();
            SetWidth_forAllPremadeLineCharts();
            SetHeight_forAllPremadeLineCharts();
            SetSizeOfPieCircleDiameter_forAllPremadePieCharts();
        }

        public static Vector3 GetAutoLayoutedPositionOfPremadeLineChart(int indexNumberOfPremadeChart)
        {
            //The charts are arranged horizontally, because they potentially grow in their y-extent due to the tower of PointOfInterest-TextBoxes. Otherwise the could potentially intersect each other:

            float horizontalOffsetDistance;
            Vector3 offsetVectorBetweenCharts_normalized;

            if (premadeChartThatIsRotationPivot < 0)
            {
                horizontalOffsetDistance = used_xOffsetBetweenPremadeCharts * indexNumberOfPremadeChart;
                offsetVectorBetweenCharts_normalized = Vector3.right;
            }
            else
            {
                UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out Vector3 observerCamForward_normalized, out Vector3 observerCamUp_normalized, out Vector3 observerCamRight_normalized, out Vector3 cam_to_lineCenter, positionOfPremadeCharts, Vector3.zero, null);
                horizontalOffsetDistance = used_xOffsetBetweenPremadeCharts * (indexNumberOfPremadeChart - premadeChartThatIsRotationPivot);
                offsetVectorBetweenCharts_normalized = observerCamRight_normalized;
            }

            return (positionOfPremadeCharts + offsetVectorBetweenCharts_normalized * horizontalOffsetDistance);
        }

        public static Vector3 GetAutoLayoutedPositionOfPremadePieChart(PieChartDrawing requestingPieChartDrawing)
        {
            float yOffset; //-> shifting the pie charts to below of the line charts. The line charts potentially grow in size towards the top (due to the tower of PointOfInterest-textBoxes), while the pie charts potentially grow in size towards the downside, due to the segment name list, the grows downward. This way the line charts and pie charts don't intersect each other.
            if (UtilitiesDXXL_Math.ApproximatelyZero(requestingPieChartDrawing.mostRecent_vertDistance_fromCircleCenter_toUpperBounderySquare))
            {
                yOffset = -2.2f * requestingPieChartDrawing.Size_ofPieCircleDiameter;
            }
            else
            {
                yOffset = (-requestingPieChartDrawing.mostRecent_vertDistance_fromCircleCenter_toUpperBounderySquare - requestingPieChartDrawing.Size_ofPieCircleDiameter);
            }

            float xPos_offset = requestingPieChartDrawing.Size_ofPieCircleDiameter;//-> shift offset, so the pie charts are vertically aligned with with the line charts. This works only correctly, if "pieChart.RelSize_ofSegmentsNameTexts" hasn't been altered.
            float horizontalOffsetDistance;
            Vector3 offsetVectorBetweenCharts_normalized;

            UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out Vector3 observerCamForward_normalized, out Vector3 observerCamUp_normalized, out Vector3 observerCamRight_normalized, out Vector3 cam_to_lineCenter, positionOfPremadeCharts, Vector3.zero, null);
            if (premadeChartThatIsRotationPivot < 0)
            {
                horizontalOffsetDistance = used_xOffsetBetweenPremadeCharts * requestingPieChartDrawing.internal_indexNumberOfPremadeChart + xPos_offset;
                offsetVectorBetweenCharts_normalized = Vector3.right;
            }
            else
            {
                horizontalOffsetDistance = used_xOffsetBetweenPremadeCharts * (requestingPieChartDrawing.internal_indexNumberOfPremadeChart - premadeChartThatIsRotationPivot) + xPos_offset;
                offsetVectorBetweenCharts_normalized = observerCamRight_normalized;
            }

            return (positionOfPremadeCharts + offsetVectorBetweenCharts_normalized * horizontalOffsetDistance + observerCamUp_normalized * yOffset);
        }

        public static void SetDistanceBetweenNeighboringPremadeChartsTo_customDistance(float horizontalDistance_fromChartToChart_relativeToChartWidth)
        {
            current_horizontalDistance_fromChartToChart_relativeToChartWidth = horizontalDistance_fromChartToChart_relativeToChartWidth;
            Apply_current_horizontalDistance_fromChartToChart_relativeToChartWidth();
        }

        public static void SetDistanceBetweenNeighboringPremadeChartsToFitCaseOf_noNeedForPointOfInterestTextBoxes()
        {
            current_horizontalDistance_fromChartToChart_relativeToChartWidth = xOffsetBetweenPremadeCharts_caseOf_noNeedFor_pointOfInterestTextBoxes;
            Apply_current_horizontalDistance_fromChartToChart_relativeToChartWidth();
        }

        public static void SetDistanceBetweenNeighboringPremadeChartsToFitCaseOf_needsPointOfInterestTextBoxesOnOneSide()
        {
            current_horizontalDistance_fromChartToChart_relativeToChartWidth = xOffsetBetweenPremadeCharts_caseOf_needs_pointOfInterestTextBoxes_onOneSide;
            Apply_current_horizontalDistance_fromChartToChart_relativeToChartWidth();
        }

        public static void SetDistanceBetweenNeighboringPremadeChartsToFitCaseOf_needsPointOfInterestTextBoxesOnBothSides()
        {
            current_horizontalDistance_fromChartToChart_relativeToChartWidth = xOffsetBetweenPremadeCharts_caseOf_needs_pointOfInterestTextBoxes_onBothSides;
            Apply_current_horizontalDistance_fromChartToChart_relativeToChartWidth();
        }

        public static void SetDistanceBetweenNeighboringPremadeChartsToFitCaseOf_onlyPieCharts()
        {
            current_horizontalDistance_fromChartToChart_relativeToChartWidth = xOffsetBetweenPremadeCharts_caseOf_onlyPieCharts;
            Apply_current_horizontalDistance_fromChartToChart_relativeToChartWidth();
        }

        static void Apply_current_horizontalDistance_fromChartToChart_relativeToChartWidth()
        {
            used_xOffsetBetweenPremadeCharts = current_widthOfPremadeCharts * current_horizontalDistance_fromChartToChart_relativeToChartWidth;
        }

        public static void RevertPremadeChartsToAutomaticPositionLayout(bool revert_premadeChart0 = true, bool revert_premadeChart1 = true, bool revert_premadeChart2 = true, bool revert_premadeChart3 = true, bool revert_premadeChart4 = true, bool revert_premadeChart5 = true, bool revert_premadeChart6 = true, bool revert_premadeChart7 = true, bool revert_premadeChart8 = true, bool revert_premadeChart9 = true, bool revert_premadePieChart0 = true, bool revert_premadePieChart1 = true, bool revert_premadePieChart2 = true, bool revert_premadePieChart3 = true, bool revert_premadePieChart4 = true, bool revert_premadePieChart5 = true, bool revert_premadePieChart6 = true, bool revert_premadePieChart7 = true, bool revert_premadePieChart8 = true, bool revert_premadePieChart9 = true)
        {
            RevertPremadeLineChartsToAutomaticPositionLayout(revert_premadeChart0, revert_premadeChart1, revert_premadeChart2, revert_premadeChart3, revert_premadeChart4, revert_premadeChart5, revert_premadeChart6, revert_premadeChart7, revert_premadeChart8, revert_premadeChart9);
            RevertPremadePieChartsToAutomaticPositionLayout(revert_premadePieChart0, revert_premadePieChart1, revert_premadePieChart2, revert_premadePieChart3, revert_premadePieChart4, revert_premadePieChart5, revert_premadePieChart6, revert_premadePieChart7, revert_premadePieChart8, revert_premadePieChart9);
        }

        public static void RevertPremadeLineChartsToAutomaticPositionLayout(bool revert_premadeChart0 = true, bool revert_premadeChart1 = true, bool revert_premadeChart2 = true, bool revert_premadeChart3 = true, bool revert_premadeChart4 = true, bool revert_premadeChart5 = true, bool revert_premadeChart6 = true, bool revert_premadeChart7 = true, bool revert_premadeChart8 = true, bool revert_premadeChart9 = true)
        {
            if (revert_premadeChart0) { premadeChart0.internal_indexNumberOfPremadeChart = 0; }
            if (revert_premadeChart1) { premadeChart1.internal_indexNumberOfPremadeChart = 1; }
            if (revert_premadeChart2) { premadeChart2.internal_indexNumberOfPremadeChart = 2; }
            if (revert_premadeChart3) { premadeChart3.internal_indexNumberOfPremadeChart = 3; }
            if (revert_premadeChart4) { premadeChart4.internal_indexNumberOfPremadeChart = 4; }
            if (revert_premadeChart5) { premadeChart5.internal_indexNumberOfPremadeChart = 5; }
            if (revert_premadeChart6) { premadeChart6.internal_indexNumberOfPremadeChart = 6; }
            if (revert_premadeChart7) { premadeChart7.internal_indexNumberOfPremadeChart = 7; }
            if (revert_premadeChart8) { premadeChart8.internal_indexNumberOfPremadeChart = 8; }
            if (revert_premadeChart9) { premadeChart9.internal_indexNumberOfPremadeChart = 9; }
        }

        public static void RevertPremadePieChartsToAutomaticPositionLayout(bool revert_premadePieChart0 = true, bool revert_premadePieChart1 = true, bool revert_premadePieChart2 = true, bool revert_premadePieChart3 = true, bool revert_premadePieChart4 = true, bool revert_premadePieChart5 = true, bool revert_premadePieChart6 = true, bool revert_premadePieChart7 = true, bool revert_premadePieChart8 = true, bool revert_premadePieChart9 = true)
        {
            if (revert_premadePieChart0) { premadePieChart0.internal_indexNumberOfPremadeChart = 0; }
            if (revert_premadePieChart1) { premadePieChart1.internal_indexNumberOfPremadeChart = 1; }
            if (revert_premadePieChart2) { premadePieChart2.internal_indexNumberOfPremadeChart = 2; }
            if (revert_premadePieChart3) { premadePieChart3.internal_indexNumberOfPremadeChart = 3; }
            if (revert_premadePieChart4) { premadePieChart4.internal_indexNumberOfPremadeChart = 4; }
            if (revert_premadePieChart5) { premadePieChart5.internal_indexNumberOfPremadeChart = 5; }
            if (revert_premadePieChart6) { premadePieChart6.internal_indexNumberOfPremadeChart = 6; }
            if (revert_premadePieChart7) { premadePieChart7.internal_indexNumberOfPremadeChart = 7; }
            if (revert_premadePieChart8) { premadePieChart8.internal_indexNumberOfPremadeChart = 8; }
            if (revert_premadePieChart9) { premadePieChart9.internal_indexNumberOfPremadeChart = 9; }
        }

        public static void SetSize_forAllPremadeCharts(float newWidthPerChart)
        {
            SetWidth_forAllPremadeLineCharts(newWidthPerChart, true);
            SetSizeOfPieCircleDiameter_forAllPremadePieCharts(newWidthPerChart * factor_fromLineChartWidth_toPieChartDiameter_soThatTheyAppearWithApproxTheSameWidth);
        }

        public static void SetWidth_forAllPremadeLineCharts(float newWidth = default_widthOfPremadeCharts, bool scaleHeightAsWell = true)
        {
            float scaleFactor_comparedToBefore = newWidth / current_widthOfPremadeCharts;
            current_widthOfPremadeCharts = newWidth;
            Apply_current_horizontalDistance_fromChartToChart_relativeToChartWidth();

            if (premadeChart0.internal_indexNumberOfPremadeChart != (-1)) { premadeChart0.Width_inWorldSpace = newWidth; }
            if (premadeChart1.internal_indexNumberOfPremadeChart != (-1)) { premadeChart1.Width_inWorldSpace = newWidth; }
            if (premadeChart2.internal_indexNumberOfPremadeChart != (-1)) { premadeChart2.Width_inWorldSpace = newWidth; }
            if (premadeChart3.internal_indexNumberOfPremadeChart != (-1)) { premadeChart3.Width_inWorldSpace = newWidth; }
            if (premadeChart4.internal_indexNumberOfPremadeChart != (-1)) { premadeChart4.Width_inWorldSpace = newWidth; }
            if (premadeChart5.internal_indexNumberOfPremadeChart != (-1)) { premadeChart5.Width_inWorldSpace = newWidth; }
            if (premadeChart6.internal_indexNumberOfPremadeChart != (-1)) { premadeChart6.Width_inWorldSpace = newWidth; }
            if (premadeChart7.internal_indexNumberOfPremadeChart != (-1)) { premadeChart7.Width_inWorldSpace = newWidth; }
            if (premadeChart8.internal_indexNumberOfPremadeChart != (-1)) { premadeChart8.Width_inWorldSpace = newWidth; }
            if (premadeChart9.internal_indexNumberOfPremadeChart != (-1)) { premadeChart9.Width_inWorldSpace = newWidth; }

            if (scaleHeightAsWell)
            {
                SetHeight_forAllPremadeLineCharts(current_heightOfPremadeCharts * scaleFactor_comparedToBefore, false);
            }
        }

        public static void SetHeight_forAllPremadeLineCharts(float newHeight = default_heightOfPremadeCharts, bool scaleWidthAsWell = true)
        {
            float scaleFactor_comparedToBefore = newHeight / current_heightOfPremadeCharts;
            current_heightOfPremadeCharts = newHeight;

            if (premadeChart0.internal_indexNumberOfPremadeChart != (-1)) { premadeChart0.Height_inWorldSpace = newHeight; }
            if (premadeChart1.internal_indexNumberOfPremadeChart != (-1)) { premadeChart1.Height_inWorldSpace = newHeight; }
            if (premadeChart2.internal_indexNumberOfPremadeChart != (-1)) { premadeChart2.Height_inWorldSpace = newHeight; }
            if (premadeChart3.internal_indexNumberOfPremadeChart != (-1)) { premadeChart3.Height_inWorldSpace = newHeight; }
            if (premadeChart4.internal_indexNumberOfPremadeChart != (-1)) { premadeChart4.Height_inWorldSpace = newHeight; }
            if (premadeChart5.internal_indexNumberOfPremadeChart != (-1)) { premadeChart5.Height_inWorldSpace = newHeight; }
            if (premadeChart6.internal_indexNumberOfPremadeChart != (-1)) { premadeChart6.Height_inWorldSpace = newHeight; }
            if (premadeChart7.internal_indexNumberOfPremadeChart != (-1)) { premadeChart7.Height_inWorldSpace = newHeight; }
            if (premadeChart8.internal_indexNumberOfPremadeChart != (-1)) { premadeChart8.Height_inWorldSpace = newHeight; }
            if (premadeChart9.internal_indexNumberOfPremadeChart != (-1)) { premadeChart9.Height_inWorldSpace = newHeight; }

            if (scaleWidthAsWell)
            {
                SetWidth_forAllPremadeLineCharts(current_widthOfPremadeCharts * scaleFactor_comparedToBefore, false);
            }
        }

        public static void SetSizeOfPieCircleDiameter_forAllPremadePieCharts(float newDiameter = default_diameterOfPremadePieCharts)
        {
            if (premadePieChart0.internal_indexNumberOfPremadeChart != (-1)) { premadePieChart0.Size_ofPieCircleDiameter = newDiameter; }
            if (premadePieChart1.internal_indexNumberOfPremadeChart != (-1)) { premadePieChart1.Size_ofPieCircleDiameter = newDiameter; }
            if (premadePieChart2.internal_indexNumberOfPremadeChart != (-1)) { premadePieChart2.Size_ofPieCircleDiameter = newDiameter; }
            if (premadePieChart3.internal_indexNumberOfPremadeChart != (-1)) { premadePieChart3.Size_ofPieCircleDiameter = newDiameter; }
            if (premadePieChart4.internal_indexNumberOfPremadeChart != (-1)) { premadePieChart4.Size_ofPieCircleDiameter = newDiameter; }
            if (premadePieChart5.internal_indexNumberOfPremadeChart != (-1)) { premadePieChart5.Size_ofPieCircleDiameter = newDiameter; }
            if (premadePieChart6.internal_indexNumberOfPremadeChart != (-1)) { premadePieChart6.Size_ofPieCircleDiameter = newDiameter; }
            if (premadePieChart7.internal_indexNumberOfPremadeChart != (-1)) { premadePieChart7.Size_ofPieCircleDiameter = newDiameter; }
            if (premadePieChart8.internal_indexNumberOfPremadeChart != (-1)) { premadePieChart8.Size_ofPieCircleDiameter = newDiameter; }
            if (premadePieChart9.internal_indexNumberOfPremadeChart != (-1)) { premadePieChart9.Size_ofPieCircleDiameter = newDiameter; }
        }

        public static void ClearAllPremadeCharts(bool clear_premadeChart0 = true, bool clear_premadeChart1 = true, bool clear_premadeChart2 = true, bool clear_premadeChart3 = true, bool clear_premadeChart4 = true, bool clear_premadeChart5 = true, bool clear_premadeChart6 = true, bool clear_premadeChart7 = true, bool clear_premadeChart8 = true, bool clear_premadeChart9 = true, bool clear_premadePieChart0 = true, bool clear_premadePieChart1 = true, bool clear_premadePieChart2 = true, bool clear_premadePieChart3 = true, bool clear_premadePieChart4 = true, bool clear_premadePieChart5 = true, bool clear_premadePieChart6 = true, bool clear_premadePieChart7 = true, bool clear_premadePieChart8 = true, bool clear_premadePieChart9 = true)
        {
            ClearAllPremadeLineCharts(clear_premadeChart0, clear_premadeChart1, clear_premadeChart2, clear_premadeChart3, clear_premadeChart4, clear_premadeChart5, clear_premadeChart6, clear_premadeChart7, clear_premadeChart8, clear_premadeChart9);
            ClearAllPremadePieCharts(clear_premadePieChart0, clear_premadePieChart1, clear_premadePieChart2, clear_premadePieChart3, clear_premadePieChart4, clear_premadePieChart5, clear_premadePieChart6, clear_premadePieChart7, clear_premadePieChart8, clear_premadePieChart9);
        }

        public static void ClearAllPremadeLineCharts(bool clear_premadeChart0 = true, bool clear_premadeChart1 = true, bool clear_premadeChart2 = true, bool clear_premadeChart3 = true, bool clear_premadeChart4 = true, bool clear_premadeChart5 = true, bool clear_premadeChart6 = true, bool clear_premadeChart7 = true, bool clear_premadeChart8 = true, bool clear_premadeChart9 = true)
        {
            if (clear_premadeChart0) { premadeChart0.Clear(); }
            if (clear_premadeChart1) { premadeChart1.Clear(); }
            if (clear_premadeChart2) { premadeChart2.Clear(); }
            if (clear_premadeChart3) { premadeChart3.Clear(); }
            if (clear_premadeChart4) { premadeChart4.Clear(); }
            if (clear_premadeChart5) { premadeChart5.Clear(); }
            if (clear_premadeChart6) { premadeChart6.Clear(); }
            if (clear_premadeChart7) { premadeChart7.Clear(); }
            if (clear_premadeChart8) { premadeChart8.Clear(); }
            if (clear_premadeChart9) { premadeChart9.Clear(); }
        }

        public static void ClearAllPremadePieCharts(bool clear_premadePieChart0 = true, bool clear_premadePieChart1 = true, bool clear_premadePieChart2 = true, bool clear_premadePieChart3 = true, bool clear_premadePieChart4 = true, bool clear_premadePieChart5 = true, bool clear_premadePieChart6 = true, bool clear_premadePieChart7 = true, bool clear_premadePieChart8 = true, bool clear_premadePieChart9 = true)
        {
            if (clear_premadePieChart0) { premadePieChart0.Clear(); }
            if (clear_premadePieChart1) { premadePieChart1.Clear(); }
            if (clear_premadePieChart2) { premadePieChart2.Clear(); }
            if (clear_premadePieChart3) { premadePieChart3.Clear(); }
            if (clear_premadePieChart4) { premadePieChart4.Clear(); }
            if (clear_premadePieChart5) { premadePieChart5.Clear(); }
            if (clear_premadePieChart6) { premadePieChart6.Clear(); }
            if (clear_premadePieChart7) { premadePieChart7.Clear(); }
            if (clear_premadePieChart8) { premadePieChart8.Clear(); }
            if (clear_premadePieChart9) { premadePieChart9.Clear(); }
        }

    }

}
