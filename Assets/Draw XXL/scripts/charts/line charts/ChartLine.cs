namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class ChartLine
    {
        public enum LineConnectionsType { straightFromPointToPoint, horizPlateauTillNextPoint, invisible }
        public enum DataPointVisualization
        {
            invisible,
            cross,
            cross45deg,
            square,
            squareCrossed,
            triangle,
            triangleCrossed,
            pentagon,
            pentagonCrossed,
            circle,
            circleFilled,
            star4corners,
            star4CornersCrossed,
            star5corners,
            star5CornersCrossed,
            heart,
            customIconSymbol
        }

        public enum NamePosition
        {
            dynamicallyMoving_atLineEnd_towardsRight,
            dynamicallyMoving_atLineEnd_towardsLeft,
            dynamic_atLowestLinePos,
            dynamic_atHighestLinePos,
            fixedPosAtRight_outsideOfChart, 
            fixedPosAtRight_insideChart, 
            fixedPosAtLeft_insideChart
        }

        public InternalDXXL_LineSpecsForChartInspector lineSpecsForInspector;

        public LineConnectionsType lineConnectionsType //if you want to set the lineConnectionsType for all lines of the chart at once you can use chartDrawing.SetLineConnectionsType 
        {
            get { return lineSpecsForInspector.connectionsType; }
            set { lineSpecsForInspector.connectionsType = value; }
        }

        public DataPointVisualization dataPointVisualization //if you want to set the dataPointVisualization for all lines of the chart at once you can use chartDrawing.SetDataPointVisualization
        {
            get { return lineSpecsForInspector.dataPointVisualization; }
            set { lineSpecsForInspector.dataPointVisualization = value; }
        }

        public NamePosition namePosition; //if you want to set the namePosition for all lines of the chart at once you can use chartDrawing.SetLineNamesPosition.
        private float nameText_sizeScaleFactor; //default is 1
        public float NameText_sizeScaleFactor
        {
            get { return nameText_sizeScaleFactor; }
            set { nameText_sizeScaleFactor = Mathf.Max(0.01f, value); }
        }

        public float alpha_ofHighlighterForMostCurrentValue_xDim;//if you want to set the alpha_ofHighlighterForMostCurrentValue_xDim for all lines of the chart at once you can use chartDrawing.Set_alpha_ofHighlighterForMostCurrentValue_xDim. With this you can set the intensity or toggle on and off the vertical line that highlights the x position of the most current value. 0 disables the highlighting. If the value was added using "AddValues_eachIndexIsALine" then the highlighting lines are only displayed if "chart.displayHighlightingOfMostCurrentValues_forLinesFromLists" has to been set to "true".
        public float alpha_ofHighlighterForMostCurrentValue_yDim;//if you want to set the alpha_ofHighlighterForMostCurrentValue_yDim for all lines of the chart at once you can use chartDrawing.Set_alpha_ofHighlighterForMostCurrentValue_yDim. With this you can set the intensity or toggle on and off the horizontal line that highlights the y position of the most current value. 0 disables the highlighting. If the value was added using "AddValues_eachIndexIsALine" then the highlighting lines are only displayed if "chart.displayHighlightingOfMostCurrentValues_forLinesFromLists" has to been set to "true".
        public bool displayDeltaAtHighlighterForMostCurrentValue;//if you want to set the displayDeltaAtHighlighterForMostCurrentValue for all lines of the chart at once you can use chartDrawing.Set_displayDeltaAtHighlighterForMostCurrentValue.
        public float alpha_ofMaxiumumYValueMarker;//if you want to set the alpha_ofMaxiumumYValueMarker for all lines of the chart at once you can use chartDrawing.Set_alpha_ofMaxiumumYValueMarker. 0 means disabled. default is 0,3. Displays only turning points, so it is not displayed when the first or the most current value is the maximum value. If you want to highlight not just the highest, but all upper turning points you can use "markAllYMaximumTurningPoints".  Can be easily set for all lines from lists via "chartDrawing.Set_alphaOfMaxiumumYValueMarker_forLinesFromLists"
        public float alpha_ofMinimumYValueMarker;//if you want to set the alpha_ofMinimumYValueMarker for all lines of the chart at once you can use chartDrawing.Set_alpha_ofMinimumYValueMarker. 0 means disabled. default is 0,3. Displays only turning points, so it is not displayed when the first or the most current value is the maximum value. If you want to highlight not just the lowest, but all lower turning points you can use "markAllYMinimumTurningPoints".  Can be easily set for all lines from lists via "chartDrawing.Set_alphaOfMinimumYValueMarker_forLinesFromLists"
        public bool markAllYMaximumTurningPoints;//if you want to set the markAllYMaximumTurningPoints for all lines of the chart at once you can use chartDrawing.Set_markAllYMaximumTurningPoints. default is false. Displays only turning points, so it is not displayed when the most current value is the maximum value. If you want to highlight only the single highest point of all and not every turning point you can use "alpha_ofMaxiumumYValueMarker". The points are displayed with the alpha specified by "alpha_ofMaximumYValueMarker" and will not be visible if this value is 0.  Can be easily set for all lines from lists via "chartDrawing.Set_markAllYMaximumTurningPoints_forLinesFromLists"
        public bool markAllYMinimumTurningPoints;//if you want to set the markAllYMinimumTurningPoints for all lines of the chart at once you can use chartDrawing.Set_markAllYMinimumTurningPoints. default is false. Displays only turning points, so it is not displayed when the most current value is the maximum value. If you want to highlight only the single lowest point of all and not every turning point you can use "alpha_ofMinimumYValueMarker". The points are displayed with the alpha specified by "alpha_ofMinimumYValueMarker" and will not be visible if this value is 0. Can be easily set for all lines from lists via "chartDrawing.Set_markAllYMinimumTurningPoints_forLinesFromLists"
        public DrawBasics.IconType customIconAsDatapointVisualization = DrawBasics.IconType.car;

        public bool Hide
        {
            get { return lineSpecsForInspector.currentHideLineState; }
            set { lineSpecsForInspector.currentHideLineState = value; }
        }

        public Color Color
        {
            get { return lineSpecsForInspector.lineColor; }
            set { lineSpecsForInspector.lineColor = value; }
        }

        private string name = null;
        public string Name //you can use rich text markups inside the name string, so the line name display can e.g. be bold, magnified in size or contain icons.
        {
            get { return name; }
            set
            {
                name = value;
                lineSpecsForInspector.linesCompoundName = GetNameCompound(InternalDXXL_LineSpecsForChartInspector.lineCompoundNames_haveSpaces_betweenTheNamePartConnectingMinus);
            }
        }

        private string nameExtraInfo = null;
        public string NameExtraInfo
        {
            get { return nameExtraInfo; }
            set
            {
                nameExtraInfo = value;
                lineSpecsForInspector.linesCompoundName = GetNameCompound(InternalDXXL_LineSpecsForChartInspector.lineCompoundNames_haveSpaces_betweenTheNamePartConnectingMinus);
            }
        }

        public float Alpha_ofVerticalAreaFillLines //This specifies weather the area below the line should be colored. It can be used to resemble the look of a area chart or stacked area chart. It has also the advantage that makes it more obvious where in the line the actual data points are located. 1 Means full alpha and the area of latest line overdraws the area of the previous lines, so the overlapping area is filled with the color of the last drawn line. 0 means disabled. For values in between it can be seen if the area of seperate lines overlap, but the color differences are only sufficient for some color combinations. The value doesn't define the absolute alpha but the alpha relative to the alpha of the line color. If you want to set the alpha_ofVerticalFillAreaLines for all lines of the chart at once you can use chartDrawing.SetAlphaOfVerticalFillAreaLines. 
        {
            get { return lineSpecsForInspector.alpha_ofVertFillLines; }
            set { lineSpecsForInspector.alpha_ofVertFillLines = value; }
        }

        public float SizeOfPoints_relToChartHeight
        {
            //if you want to set the SizeOfPoints_relToChartHeight for all lines of the chart at once you can use chartDrawing.Set_SizeOfPoints_relToChartHeight
            get { return lineSpecsForInspector.dataPointVisualization_size; }
            set
            {
                float min = 0.001f;
                if (value < min)
                {
                    Debug.LogError("Setting 'SizeOfPoints_relToChartHeight' failed. It should be at least " + min + ". If you want to disable the point visualization then set 'dataPointVisualization' to 'invisible'.");
                }
                else
                {
                    lineSpecsForInspector.dataPointVisualization_size = value;
                }
            }
        }

        public float LineWidth_relToChartHeight //if you want to set the lineWidth_relToChartHeight for all lines of the chart at once you can use chartDrawing.Set_lineWidth_relToChartHeight. default is 0. warning: raising this may consume many linesPerFrame.
        {
            get { return lineSpecsForInspector.lineWidth; }
            set { lineSpecsForInspector.lineWidth = value; }
        }

        public float pointVisualisationLineWidth_relToChartHeight;//if you want to set the pointVisualisationLineWidth_relToChartHeight for all lines of the chart at once you can use chartDrawing.Set_pointVisualisationLineWidth_relToChartHeight. default is 0. warning: raising this may consume many linesPerFrame. Maybe a 'filled' DataPointVisualization choice already does the job.

        private GameObject gameobject_thatThisLineCurrentlyRepresents = null;
        public GameObject Gameobject_thatThisLineCurrentlyRepresents
        {
            get { return gameobject_thatThisLineCurrentlyRepresents; }
            set { Debug.LogError("Setting 'Gameobject_thatThisLineCurrentlyRepresents' manually is not supported."); }
        }

        /// other:
        public List<InternalDXXL_DataPointOfChartLine> dataPoints = new List<InternalDXXL_DataPointOfChartLine>();
        List<PointOfInterest> pointsOfInterest = new List<PointOfInterest>();
        List<PointOfInterest> pointsOfInterest_fromIntersectionsWithHorizLines = new List<PointOfInterest>();
        List<InternalDXXL_HorizontalThresholdLine> horizontalThresholdLines = new List<InternalDXXL_HorizontalThresholdLine>();
        PointOfInterest pointOfInterest_thatHighlightsTheMostCurrentValue;
        public InternalDXXL_TurningPointDetector turningPointDetector; 
        public bool disableMinMaxYVisualizers_dueTo_lineRepresentsBoolValues = false; //Disables the display of "maximum y value" and "minimum y value", since for bools such a display doesn't have surplus value but only obscures the actual line.
        public bool representsValuesFromAddedLists = false; 
        private bool allXValuesCameFromAutomaticSource;
        public bool AllXValuesCameFromAutomaticSource
        {
            get { return allXValuesCameFromAutomaticSource; }
            set { Debug.LogError("Setting 'AllXValuesCameFromAutomaticSource' manually is not supported."); }
        }

        private float highestYValue = float.NegativeInfinity;
        private float lowestYValue = float.PositiveInfinity;
        private float highestXValue = float.NegativeInfinity;
        private float lowestXValue = float.PositiveInfinity;
        public float HighestYValue
        {
            get { return highestYValue; }
            set { Debug.LogError("Setting 'HighestYValue' manually is not supported. Did you mean 'yAxis.fixedUpperEndValueOfScale' instead? (which by the way works only with 'yAxis.scaling = fixed_absolute')"); }
        }
        public float LowestYValue
        {
            get { return lowestYValue; }
            set { Debug.LogError("Setting 'LowestYValue' manually is not supported. Did you mean 'yAxis.fixedLowerEndValueOfScale' instead? (which by the way works only with 'yAxis.scaling = fixed_absolute')"); }
        }
        public float HighestXValue
        {
            get { return highestXValue; }
            set { Debug.LogError("Setting 'HighestXValue' manually is not supported. Did you mean 'xAxis.fixedUpperEndValueOfScale' instead? (which by the way works only with 'xAxis.scaling = fixed_absolute')"); }
        }
        public float LowestXValue
        {
            get { return lowestXValue; }
            set { Debug.LogError("Setting 'LowestXValue' manually is not supported. Did you mean 'xAxis.fixedLowerEndValueOfScale' instead? (which by the way works only with 'xAxis.scaling = fixed_absolute')"); }
        }

        private bool hasAtLeastOneValuePairOfValidData = false;
        public bool HasAtLeastOneValuePairOfValidData
        {
            get { return hasAtLeastOneValuePairOfValidData; }
            set { Debug.LogError("Setting 'HasAtLeastOneValuePairOfValidData' manually is not supported."); }
        }

        private ChartDrawing chart_thisLineIsPartOf;
        public ChartDrawing Chart_thisLineIsPartOf
        {
            get { return chart_thisLineIsPartOf; }
            set { Debug.LogError("Setting 'Chart_thisLineIsPartOf' manually is not supported."); }
        }

        UtilitiesDXXL_ChartLine.IsDrawnBecause_theSingleComponentOfMulticomponentData_thisLineRepresents_isEnabledChecker IsDrawnBecause_theSingleComponentOfMulticomponentData_thisLineRepresents_isEnabled = UtilitiesDXXL_ChartLine.DoDrawBecauseLineDoesntRepresentMultiComponentData;
        float mostCurrentValidYValue = float.NaN;
        float mostCurrentValidXValue = float.NaN;

        public ChartLine(Color lineColor, ChartDrawing chart_thisLineIsPartOf)
        {
            this.chart_thisLineIsPartOf = chart_thisLineIsPartOf;

            lineSpecsForInspector = new InternalDXXL_LineSpecsForChartInspector();
            lineSpecsForInspector.lineColor = lineColor;
            lineSpecsForInspector.line_theseSpecsBelongTo = this;
            lineSpecsForInspector.currentHideLineState = false;
            lineSpecsForInspector.connectionsType = chart_thisLineIsPartOf.default_lineConnectionsType;
            lineSpecsForInspector.dataPointVisualization = chart_thisLineIsPartOf.default_dataPointVisualization;
            lineSpecsForInspector.dataPointVisualization_size = chart_thisLineIsPartOf.default_SizeOfPoints_relToChartHeight;
            lineSpecsForInspector.alpha_ofVertFillLines = chart_thisLineIsPartOf.default_alpha_ofVerticalAreaFillLines;
            lineSpecsForInspector.lineWidth = chart_thisLineIsPartOf.default_lineWidth_relToChartHeight;

            namePosition = chart_thisLineIsPartOf.default_lineNamePosition;
            nameText_sizeScaleFactor = chart_thisLineIsPartOf.default_lineNameText_sizeScaleFactor;
            alpha_ofHighlighterForMostCurrentValue_xDim = chart_thisLineIsPartOf.default_alpha_ofHighlighterForMostCurrentValue_xDim;
            alpha_ofHighlighterForMostCurrentValue_yDim = chart_thisLineIsPartOf.default_alpha_ofHighlighterForMostCurrentValue_yDim;
            displayDeltaAtHighlighterForMostCurrentValue = chart_thisLineIsPartOf.default_displayDeltaAtHighlighterForMostCurrentValue;
            alpha_ofMaxiumumYValueMarker = chart_thisLineIsPartOf.default_alpha_ofMaxiumumYValueMarker;
            alpha_ofMinimumYValueMarker = chart_thisLineIsPartOf.default_alpha_ofMinimumYValueMarker;
            markAllYMaximumTurningPoints = chart_thisLineIsPartOf.default_markAllYMaximumTurningPoints;
            markAllYMinimumTurningPoints = chart_thisLineIsPartOf.default_markAllYMinimumTurningPoints;
            pointVisualisationLineWidth_relToChartHeight = chart_thisLineIsPartOf.default_pointVisualisationLineWidth_relToChartHeight;

            allXValuesCameFromAutomaticSource = true;
            pointOfInterest_thatHighlightsTheMostCurrentValue = CreatePointOfInterestThatHighlightsTheMostCurrentValue();
            pointOfInterest_thatHighlightsTheMostCurrentValue.xValue.placeTextTowardsOutsideOfChart = true;
            pointOfInterest_thatHighlightsTheMostCurrentValue.yValue.placeTextTowardsOutsideOfChart = true;
            AddPointOfInterest(pointOfInterest_thatHighlightsTheMostCurrentValue);
            turningPointDetector = new InternalDXXL_TurningPointDetector(this);
            if (chart_thisLineIsPartOf.chartInspector_component != null) { InitInspectionViaComponent(); }
        }

        public void Clear()
        {
            //use ChartDrawing.Clear() instead
            allXValuesCameFromAutomaticSource = true;
            hasAtLeastOneValuePairOfValidData = false;
            dataPoints.Clear();
            highestYValue = float.NegativeInfinity;
            lowestYValue = float.PositiveInfinity;
            highestXValue = float.NegativeInfinity;
            lowestXValue = float.PositiveInfinity;
            mostCurrentValidYValue = float.NaN;
            mostCurrentValidXValue = float.NaN;
            gameobject_thatThisLineCurrentlyRepresents = null;
            markNextNewlyCreatedPointWithEmphasizingCircle = false;
            forceUpcomingNextCreatedConnectionLine_toLowAlpha = false;
            DeletePointsOfInterestOnClear();
        }

        public bool Draw(InternalDXXL_Plane chartPlane, int numberOfLinesWithValidDataPointsThatHaveAlreadyBeenDrawn, float valueMarkingLowerEndOf_XAxis, float valueMarkingUpperEndOf_XAxis, float valueMarkingLowerEndOf_YAxis, float valueMarkingUpperEndOf_YAxis, float durationInSec, bool hiddenByNearerObjects)
        {
            //use ChartDrawing.Draw() instead

            thePointsOfInterestOfThisLineArePartOfTheChart = false;
            if (CheckIfLineIsDrawn())
            {
                if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return (dataPoints.Count > 0); }
                DXXLWrapperForUntiysBuildInDrawLines.currentlyDrawingChart = chart_thisLineIsPartOf;
                SetIfVisualizationOfSpecialApexPointsIsDisplayed();
                float absSizeOfPointVisualization_inWorldSpace = Mathf.Abs(chart_thisLineIsPartOf.Height_inWorldSpace * SizeOfPoints_relToChartHeight);
                bool atLeastOneDatapointIsValid = false;
                bool atLeastOneDrawnPointHasBeenFound = false;
                int i_ofTheMostCurrentDrawnPoint = -1;
                Vector3 mostRecentDrawnDataPoint_worldSpace = chart_thisLineIsPartOf.Position_worldspace;
                Vector3 highestDrawnPoint_worldSpace = default;
                Vector3 lowestDrawnPoint_worldSpace = default;
                float heightOfHighestDrawnPoint_chartSpace = default;
                float heightOfLowestDrawnPoint_chartSpace = default;
                float absWidthOfConnectionLines_worldSpace = Get_absWidthOfConnectionLines_worldSpace();
                float absWidthOfPointVisualisatorLines_worldSpace = Get_absWidthOfPointVisualisatorLines_worldSpace();
                Vector3 amplitudeDir_forNonZeroWidthLines = Get_amplitudeDir_forNonZeroWidthLines(chartPlane);
                bool thereHaveBeenValidPointsOutsideTheDrawnChartArea_sinceMostCurrentDrawnPoint = false;

                for (int i = 0; i < dataPoints.Count; i++)
                {
                    if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return (dataPoints.Count > 0); }
                    if (dataPoints[i].validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid) { atLeastOneDatapointIsValid = true; }

                    dataPoints[i].DetermineIfAndHowPointIsDrawn(valueMarkingLowerEndOf_XAxis, valueMarkingUpperEndOf_XAxis, valueMarkingLowerEndOf_YAxis, valueMarkingUpperEndOf_YAxis, i_ofTheMostCurrentDrawnPoint, thereHaveBeenValidPointsOutsideTheDrawnChartArea_sinceMostCurrentDrawnPoint);
                    if (dataPoints[i].isDrawn)
                    {
                        mostRecentDrawnDataPoint_worldSpace = dataPoints[i].positionInWorldSpace;
                        TryDrawConnectionLineFromPrecedingPoint(i, absWidthOfConnectionLines_worldSpace, amplitudeDir_forNonZeroWidthLines, durationInSec, hiddenByNearerObjects);
                        dataPoints[i].DrawPointVisualization(absSizeOfPointVisualization_inWorldSpace, absWidthOfConnectionLines_worldSpace, absWidthOfPointVisualisatorLines_worldSpace, amplitudeDir_forNonZeroWidthLines, durationInSec, hiddenByNearerObjects);
                        if (atLeastOneDrawnPointHasBeenFound == false)
                        {
                            highestDrawnPoint_worldSpace = dataPoints[i].positionInWorldSpace;
                            lowestDrawnPoint_worldSpace = dataPoints[i].positionInWorldSpace;
                            heightOfHighestDrawnPoint_chartSpace = dataPoints[i].yValue;
                            heightOfLowestDrawnPoint_chartSpace = dataPoints[i].yValue;
                            atLeastOneDrawnPointHasBeenFound = true;
                        }
                        else
                        {
                            if (dataPoints[i].yValue > heightOfHighestDrawnPoint_chartSpace)
                            {
                                highestDrawnPoint_worldSpace = dataPoints[i].positionInWorldSpace;
                                heightOfHighestDrawnPoint_chartSpace = dataPoints[i].yValue;
                            }

                            if (dataPoints[i].yValue < heightOfLowestDrawnPoint_chartSpace)
                            {
                                lowestDrawnPoint_worldSpace = dataPoints[i].positionInWorldSpace;
                                heightOfLowestDrawnPoint_chartSpace = dataPoints[i].yValue;
                            }
                        }
                        i_ofTheMostCurrentDrawnPoint = i;
                        thereHaveBeenValidPointsOutsideTheDrawnChartArea_sinceMostCurrentDrawnPoint = false;
                    }
                    else
                    {
                        if (dataPoints[i].validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid) { thereHaveBeenValidPointsOutsideTheDrawnChartArea_sinceMostCurrentDrawnPoint = true; } //-> current datapoint is valid, but not drawn (because it is outide of the drawn area)
                    }
                }

                bool lineHasAtLeastOneDatapoint_validOrInvalid = dataPoints.Count > 0;
                thePointsOfInterestOfThisLineArePartOfTheChart = lineHasAtLeastOneDatapoint_validOrInvalid;
                if (lineHasAtLeastOneDatapoint_validOrInvalid)
                {
                    DrawNameAsText(!atLeastOneDatapointIsValid, atLeastOneDrawnPointHasBeenFound, numberOfLinesWithValidDataPointsThatHaveAlreadyBeenDrawn, mostRecentDrawnDataPoint_worldSpace, highestDrawnPoint_worldSpace, lowestDrawnPoint_worldSpace, absWidthOfConnectionLines_worldSpace, durationInSec, hiddenByNearerObjects);
                }
                DXXLWrapperForUntiysBuildInDrawLines.currentlyDrawingChart = null;
                return lineHasAtLeastOneDatapoint_validOrInvalid;
            }
            else
            {
                //-> 'dataComponentsThatAreDrawn' or 'hide' has disabled this line from drawing
                bool lineIsDrawn = false;
                return lineIsDrawn;
            }
        }

        void TryDrawConnectionLineFromPrecedingPoint(int i, float absWidthOfConnectionLines_worldSpace, Vector3 amplitudeDir_forNonZeroWidthLines, float durationInSec, bool hiddenByNearerObjects)
        {
            if (dataPoints[i].HasAPrecedingPointFromWhichLineCanBeDrawn())
            {
                Vector3 worldPos_ofPrecedingPointFromWhichLineCanBeDrawn = Get_worldPos_ofPrecedingPointFromWhichLineCanBeDrawn(i);
                float yValue_ofPrecedingPointFromWhichLineCanBeDrawn_inChartSpace = Get_yValue_ofPrecedingPointFromWhichLineCanBeDrawn_inChartSpace(i);
                bool lineFromPrecedingDrawnPoint_shouldBeLowerAlphaIndicatingAnInvalidPhase = CheckIf_lineFromPrecedingDrawnPoint_shouldBeLowerAlphaIndicatingAnInvalidPhase(i);
                dataPoints[i].DrawConnectionLineFromPrecedingPoint(worldPos_ofPrecedingPointFromWhichLineCanBeDrawn, yValue_ofPrecedingPointFromWhichLineCanBeDrawn_inChartSpace, lineFromPrecedingDrawnPoint_shouldBeLowerAlphaIndicatingAnInvalidPhase, absWidthOfConnectionLines_worldSpace, amplitudeDir_forNonZeroWidthLines, durationInSec, hiddenByNearerObjects);
            }
        }
        Vector3 Get_worldPos_ofPrecedingPointFromWhichLineCanBeDrawn(int i_ofPointForWhichToCheckThePrecedingDrawnOne)
        {
            return dataPoints[dataPoints[i_ofPointForWhichToCheckThePrecedingDrawnOne].i_ofPrecedingPointFromWhichLineCanBeDrawn].positionInWorldSpace;
        }

        float Get_yValue_ofPrecedingPointFromWhichLineCanBeDrawn_inChartSpace(int i_ofPointForWhichToCheckThePrecedingDrawnOne)
        {
            return dataPoints[dataPoints[i_ofPointForWhichToCheckThePrecedingDrawnOne].i_ofPrecedingPointFromWhichLineCanBeDrawn].yValue;

        }
        bool CheckIf_lineFromPrecedingDrawnPoint_shouldBeLowerAlphaIndicatingAnInvalidPhase(int i_ofPointForWhichToCheckThePrecedingDrawnOne)
        {
            if ((dataPoints[i_ofPointForWhichToCheckThePrecedingDrawnOne].i_ofPrecedingPointFromWhichLineCanBeDrawn + 1) == i_ofPointForWhichToCheckThePrecedingDrawnOne)
            {
                //the two valid points are direct neighbors:
                return false;
            }
            else
            {
                //there is a gap of invalid points between the two valid points:
                //(this happens because of invalid X slots of the inbetween points, in which case these invalid points are not even drawn as low alpha (despite if they were only invalid in their Y slot))
                return true;
            }
        }

        void DrawNameAsText(bool lineHasDatapointsButNoneOfThemIsValid, bool atLeastOneDrawnPointHasBeenFound, int numberOfLinesWithValidDataPointsThatHaveAlreadyBeenDrawn, Vector3 mostRecentDrawnDataPoint_worldSpace, Vector3 highestDrawnPoint_worldSpace, Vector3 lowestDrawnPoint_worldSpace, float absWidthOfConnectionLines_worldSpace, float durationInSec, bool hiddenByNearerObjects)
        {
            if (name == null || name == "")
            {
                if (nameExtraInfo != null && nameExtraInfo != "")
                {
                    name = "[nameless line]";
                }
            }

            if (name != null && name != "")
            {
                float textSize = 0.04f * chart_thisLineIsPartOf.Height_inWorldSpace * nameText_sizeScaleFactor;
                Vector3 textPosition;
                DrawText.TextAnchorDXXL textAnchor;
                Vector3 extraTextPosition;
                DrawText.TextAnchorDXXL extraTextAnchor;
                Vector3 topRightPos_ofChart;

                switch (namePosition)
                {
                    case NamePosition.dynamicallyMoving_atLineEnd_towardsRight:
                        if (atLeastOneDrawnPointHasBeenFound)
                        {
                            textPosition = mostRecentDrawnDataPoint_worldSpace;
                            textAnchor = DrawText.TextAnchorDXXL.MiddleLeft;
                            DrawMainName(lineHasDatapointsButNoneOfThemIsValid, textPosition, textSize, textAnchor, durationInSec, hiddenByNearerObjects);
                            if (nameExtraInfo != null && nameExtraInfo != "")
                            {
                                extraTextPosition = textPosition + (DrawText.parsedTextSpecs.widthOfLongestLine + 0.16f * DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine) * chart_thisLineIsPartOf.xAxis.AxisVector_normalized_inWorldSpace;
                                extraTextAnchor = DrawText.TextAnchorDXXL.UpperLeft;
                                DrawNameExtraInfo(extraTextPosition, extraTextAnchor, durationInSec, hiddenByNearerObjects); //relies on the preceding "DrawMainName" (due to using "DrawText.parsedTextSpecs")
                            }
                        }
                        else
                        {
                            DrawNames_for_fixedPosAtLeft_insideChart(lineHasDatapointsButNoneOfThemIsValid, textSize, numberOfLinesWithValidDataPointsThatHaveAlreadyBeenDrawn, durationInSec, hiddenByNearerObjects);
                        }
                        break;
                    case NamePosition.dynamicallyMoving_atLineEnd_towardsLeft:
                        if (atLeastOneDrawnPointHasBeenFound)
                        {
                            textPosition = mostRecentDrawnDataPoint_worldSpace;
                            textAnchor = DrawText.TextAnchorDXXL.MiddleRight;
                            DrawMainName(lineHasDatapointsButNoneOfThemIsValid, textPosition, textSize, textAnchor, durationInSec, hiddenByNearerObjects);
                            if (nameExtraInfo != null && nameExtraInfo != "")
                            {
                                extraTextPosition = textPosition - (DrawText.parsedTextSpecs.widthOfLongestLine + 0.16f * DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine) * chart_thisLineIsPartOf.xAxis.AxisVector_normalized_inWorldSpace;
                                extraTextAnchor = DrawText.TextAnchorDXXL.UpperRight;
                                DrawNameExtraInfo(extraTextPosition, extraTextAnchor, durationInSec, hiddenByNearerObjects); //relies on the preceding "DrawMainName" (due to using "DrawText.parsedTextSpecs")
                            }
                        }
                        else
                        {
                            DrawNames_for_fixedPosAtLeft_insideChart(lineHasDatapointsButNoneOfThemIsValid, textSize, numberOfLinesWithValidDataPointsThatHaveAlreadyBeenDrawn, durationInSec, hiddenByNearerObjects);
                        }
                        break;
                    case NamePosition.dynamic_atLowestLinePos:
                        if (atLeastOneDrawnPointHasBeenFound)
                        {
                            textPosition = lowestDrawnPoint_worldSpace + chart_thisLineIsPartOf.yAxis.AxisVector_normalized_inWorldSpace * 0.5f * textSize - chart_thisLineIsPartOf.yAxis.AxisVector_normalized_inWorldSpace * (0.5f * absWidthOfConnectionLines_worldSpace);
                            textAnchor = DrawText.TextAnchorDXXL.UpperCenter;
                            DrawMainName(lineHasDatapointsButNoneOfThemIsValid, textPosition, textSize, textAnchor, durationInSec, hiddenByNearerObjects);
                            if (nameExtraInfo != null && nameExtraInfo != "")
                            {
                                extraTextPosition = textPosition + (0.5f * DrawText.parsedTextSpecs.widthOfLongestLine + 0.16f * DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine) * chart_thisLineIsPartOf.xAxis.AxisVector_normalized_inWorldSpace - DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine * UtilitiesDXXL_Text.relLineDistance * chart_thisLineIsPartOf.yAxis.AxisVector_normalized_inWorldSpace;
                                extraTextAnchor = DrawText.TextAnchorDXXL.LowerLeft;
                                DrawNameExtraInfo(extraTextPosition, extraTextAnchor, durationInSec, hiddenByNearerObjects); //relies on the preceding "DrawMainName" (due to using "DrawText.parsedTextSpecs")
                            }
                        }
                        else
                        {
                            DrawNames_for_fixedPosAtLeft_insideChart(lineHasDatapointsButNoneOfThemIsValid, textSize, numberOfLinesWithValidDataPointsThatHaveAlreadyBeenDrawn, durationInSec, hiddenByNearerObjects);
                        }
                        break;
                    case NamePosition.dynamic_atHighestLinePos:
                        if (atLeastOneDrawnPointHasBeenFound)
                        {
                            textPosition = highestDrawnPoint_worldSpace + chart_thisLineIsPartOf.yAxis.AxisVector_normalized_inWorldSpace * (0.5f * absWidthOfConnectionLines_worldSpace);
                            textAnchor = DrawText.TextAnchorDXXL.LowerCenter;
                            DrawMainName(lineHasDatapointsButNoneOfThemIsValid, textPosition, textSize, textAnchor, durationInSec, hiddenByNearerObjects);
                            if (nameExtraInfo != null && nameExtraInfo != "")
                            {
                                extraTextPosition = textPosition + (0.5f * DrawText.parsedTextSpecs.widthOfLongestLine + 0.16f * DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine) * chart_thisLineIsPartOf.xAxis.AxisVector_normalized_inWorldSpace;
                                extraTextAnchor = DrawText.TextAnchorDXXL.LowerLeft;
                                DrawNameExtraInfo(extraTextPosition, extraTextAnchor, durationInSec, hiddenByNearerObjects); //relies on the preceding "DrawMainName" (due to using "DrawText.parsedTextSpecs")
                            }
                        }
                        else
                        {
                            DrawNames_for_fixedPosAtLeft_insideChart(lineHasDatapointsButNoneOfThemIsValid, textSize, numberOfLinesWithValidDataPointsThatHaveAlreadyBeenDrawn, durationInSec, hiddenByNearerObjects);
                        }
                        break;
                    case NamePosition.fixedPosAtRight_outsideOfChart:
                        topRightPos_ofChart = chart_thisLineIsPartOf.Position_worldspace + chart_thisLineIsPartOf.xAxis.AxisVector_inWorldSpace + chart_thisLineIsPartOf.yAxis.AxisVector_inWorldSpace;
                        textPosition = topRightPos_ofChart - chart_thisLineIsPartOf.yAxis.AxisVector_normalized_inWorldSpace * 1.75f * textSize * numberOfLinesWithValidDataPointsThatHaveAlreadyBeenDrawn;
                        textAnchor = DrawText.TextAnchorDXXL.MiddleLeft;
                        DrawMainName(lineHasDatapointsButNoneOfThemIsValid, textPosition, textSize, textAnchor, durationInSec, hiddenByNearerObjects);
                        if (nameExtraInfo != null && nameExtraInfo != "")
                        {
                            extraTextPosition = textPosition + (DrawText.parsedTextSpecs.widthOfLongestLine + 0.16f * DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine) * chart_thisLineIsPartOf.xAxis.AxisVector_normalized_inWorldSpace;
                            extraTextAnchor = DrawText.TextAnchorDXXL.UpperLeft;
                            DrawNameExtraInfo(extraTextPosition, extraTextAnchor, durationInSec, hiddenByNearerObjects); //relies on the preceding "DrawMainName" (due to using "DrawText.parsedTextSpecs")
                        }
                        break;
                    case NamePosition.fixedPosAtRight_insideChart:
                        topRightPos_ofChart = chart_thisLineIsPartOf.Position_worldspace + chart_thisLineIsPartOf.xAxis.AxisVector_inWorldSpace + chart_thisLineIsPartOf.yAxis.AxisVector_inWorldSpace;
                        textPosition = topRightPos_ofChart - chart_thisLineIsPartOf.yAxis.AxisVector_normalized_inWorldSpace * 1.75f * textSize * numberOfLinesWithValidDataPointsThatHaveAlreadyBeenDrawn;
                        textAnchor = DrawText.TextAnchorDXXL.MiddleRight;
                        DrawMainName(lineHasDatapointsButNoneOfThemIsValid, textPosition, textSize, textAnchor, durationInSec, hiddenByNearerObjects);
                        if (nameExtraInfo != null && nameExtraInfo != "")
                        {
                            extraTextPosition = textPosition - (DrawText.parsedTextSpecs.widthOfLongestLine + 0.16f * DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine) * chart_thisLineIsPartOf.xAxis.AxisVector_normalized_inWorldSpace;
                            extraTextAnchor = DrawText.TextAnchorDXXL.UpperRight;
                            DrawNameExtraInfo(extraTextPosition, extraTextAnchor, durationInSec, hiddenByNearerObjects); //relies on the preceding "DrawMainName" (due to using "DrawText.parsedTextSpecs")
                        }
                        break;
                    case NamePosition.fixedPosAtLeft_insideChart:
                        DrawNames_for_fixedPosAtLeft_insideChart(lineHasDatapointsButNoneOfThemIsValid, textSize, numberOfLinesWithValidDataPointsThatHaveAlreadyBeenDrawn, durationInSec, hiddenByNearerObjects);
                        break;
                    default:
                        UtilitiesDXXL_Log.PrintErrorCode("7");
                        textPosition = chart_thisLineIsPartOf.Position_worldspace;
                        textAnchor = DrawText.TextAnchorDXXL.LowerLeft;
                        DrawMainName(lineHasDatapointsButNoneOfThemIsValid, textPosition, textSize, textAnchor, durationInSec, hiddenByNearerObjects);
                        if (nameExtraInfo != null && nameExtraInfo != "")
                        {
                            extraTextPosition = textPosition + (DrawText.parsedTextSpecs.widthOfLongestLine + 0.16f * DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine) * chart_thisLineIsPartOf.xAxis.AxisVector_normalized_inWorldSpace;
                            extraTextAnchor = DrawText.TextAnchorDXXL.LowerLeft;
                            DrawNameExtraInfo(extraTextPosition, extraTextAnchor, durationInSec, hiddenByNearerObjects); //relies on the preceding "DrawMainName" (due to using "DrawText.parsedTextSpecs")
                        }
                        break;
                }
            }
        }

        void DrawMainName(bool lineHasDatapointsButNoneOfThemIsValid, Vector3 textPosition, float textSize, DrawText.TextAnchorDXXL textAnchor, float durationInSec, bool hiddenByNearerObjects)
        {
            float autoLineBreakWidth = 0.0f;
            if (lineHasDatapointsButNoneOfThemIsValid)
            {
                UtilitiesDXXL_Text.WriteFramed(name + " [<color=#ce0e0eFF><icon=logMessageError></color> All " + dataPoints.Count + " datapoints of this line are invalid (meaning 'NaN' or 'Infinity')]", textPosition, Color, textSize, chart_thisLineIsPartOf.InternalRotation, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, autoLineBreakWidth, chart_thisLineIsPartOf.autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                UtilitiesDXXL_Text.WriteFramed(name, textPosition, Color, textSize, chart_thisLineIsPartOf.InternalRotation, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, autoLineBreakWidth, chart_thisLineIsPartOf.autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects);
            }
        }

        void DrawNameExtraInfo(Vector3 extraTextPosition, DrawText.TextAnchorDXXL extraTextAnchor, float durationInSec, bool hiddenByNearerObjects)
        {
            float extraTextSize = 0.01f * chart_thisLineIsPartOf.Height_inWorldSpace * nameText_sizeScaleFactor;
            float autoLineBreakWidth_ofExtraText = 0.0f;
            UtilitiesDXXL_Text.WriteFramed(nameExtraInfo, extraTextPosition, Color, extraTextSize, chart_thisLineIsPartOf.InternalRotation, extraTextAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, autoLineBreakWidth_ofExtraText, chart_thisLineIsPartOf.autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects);
        }

        void DrawNames_for_fixedPosAtLeft_insideChart(bool lineHasDatapointsButNoneOfThemIsValid, float textSize, int numberOfLinesWithValidDataPointsThatHaveAlreadyBeenDrawn, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 topLeftPos_ofChart = chart_thisLineIsPartOf.Position_worldspace + chart_thisLineIsPartOf.yAxis.AxisVector_inWorldSpace;
            Vector3 textPosition = topLeftPos_ofChart - chart_thisLineIsPartOf.yAxis.AxisVector_normalized_inWorldSpace * 1.75f * textSize * numberOfLinesWithValidDataPointsThatHaveAlreadyBeenDrawn;
            DrawText.TextAnchorDXXL textAnchor = DrawText.TextAnchorDXXL.MiddleLeft;
            DrawMainName(lineHasDatapointsButNoneOfThemIsValid, textPosition, textSize, textAnchor, durationInSec, hiddenByNearerObjects);
            if (nameExtraInfo != null && nameExtraInfo != "")
            {
                Vector3 extraTextPosition = textPosition + (DrawText.parsedTextSpecs.widthOfLongestLine + 0.16f * DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine) * chart_thisLineIsPartOf.xAxis.AxisVector_normalized_inWorldSpace;
                DrawText.TextAnchorDXXL extraTextAnchor = DrawText.TextAnchorDXXL.UpperLeft;
                DrawNameExtraInfo(extraTextPosition, extraTextAnchor, durationInSec, hiddenByNearerObjects); //relies on the preceding "DrawMainName" (due to using "DrawText.parsedTextSpecs")
            }
        }

        public void AddValue(float yValue)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                InternalAdd(true, false, GetCurrentAutomaticXValue(), yValue);
            }
        }

        public void AddValue(bool yValue)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                float yValue_asFloat = (yValue == true) ? 1.0f : 0.0f;
                InternalAdd(true, false, GetCurrentAutomaticXValue(), yValue_asFloat);
            }
        }

        public void AddXYValue(Vector2 xyValueOfTheNewDataPoint)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                InternalAdd(false, false, xyValueOfTheNewDataPoint.x, xyValueOfTheNewDataPoint.y);
            }
        }

        public void AddXYValue(float xValue, bool yValue)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                float yValue_asFloat = (yValue == true) ? 1.0f : 0.0f;
                InternalAdd(false, false, xValue, yValue_asFloat);
            }
        }

        public void AddXYValue(float xValue, float yValue)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                InternalAdd(false, false, xValue, yValue);
            }
        }

        public void InternalAddPlaceholderDatapointForNonExistingListSlot(float xValue_fromOtherLineInsideListFromWhereThisPlaceholderGetsFilled)
        {
            InternalAdd(true, true, xValue_fromOtherLineInsideListFromWhereThisPlaceholderGetsFilled, float.NaN);
        }

        public void InternalAddFromList(float yValue)
        {
            InternalAdd(true, false, GetCurrentAutomaticXValue(), yValue);
        }

        void InternalAdd(bool xValueIsFromAutomaticSource, bool newDatapoint_isPlaceholderDuringPhasesWhereThisLineOfListDidntExistsDueToListChangedItsCount_existsOnlyToStayInXSyncWithOtherLinesOfList, float xValue, float yValue)
        {
            if (xValueIsFromAutomaticSource == false) { chart_thisLineIsPartOf.xAxis.ReportXValueFromNonAutomaticSource(); allXValuesCameFromAutomaticSource = false; }
            InternalDXXL_DataPointOfChartLine newDataPoint = new InternalDXXL_DataPointOfChartLine();
            newDataPoint.validState = InternalDXXL_DataPointOfChartLine.GetValidStateOf_datapointToBeCreated(newDatapoint_isPlaceholderDuringPhasesWhereThisLineOfListDidntExistsDueToListChangedItsCount_existsOnlyToStayInXSyncWithOtherLinesOfList, xValue, yValue);
            bool newPointIsValid = (newDataPoint.validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid);
            newDataPoint.xValue = xValue;
            newDataPoint.yValue = yValue;
            TryAddNotificationPoint_atTransitionFromInvalidPhaseAtLineStartToFirstValidPhase(newPointIsValid, xValue, yValue);
            TryAddNotificationPoint_atChangeOfInvalidTypeInsideInvalidPhase(newPointIsValid, newDataPoint);
            if (newPointIsValid) { mostCurrentValidXValue = xValue; mostCurrentValidYValue = yValue; }
            TryAddNotificationPoint_atTransitionFromValidPhaseToInvalidPhase(newPointIsValid, newDataPoint);
            newDataPoint.i_ofThisPointInsideContainingLine = dataPoints.Count;
            newDataPoint.line_thisPointIsPartOf = this;
            TryMarkPointWithEmphasizingCircle_dueTo_startOrEndOfValidPhase(ref newDataPoint);
            TryMarkPointWithEmphasizingCircle_dueTo_forcedByUser(ref newDataPoint);
            CheckIntersectionWithHorizThresholdLines(newDataPoint);
            if (forceUpcomingNextCreatedConnectionLine_toLowAlpha == true) { newDataPoint.forceConnectionLineToLowAlpha = true; forceUpcomingNextCreatedConnectionLine_toLowAlpha = false; }
            UpdatePosOfVisualizationOfMostCurrentValue(newDataPoint);
            dataPoints.Add(newDataPoint);
            turningPointDetector.AddNewDatapoint(dataPoints.Count - 1);
            NoteIf_hasAtLeastOneValuePairOfValidData(newPointIsValid);
            Update_highestYValue(yValue, newPointIsValid);
            Update_lowestYValue(yValue, newPointIsValid);
            Update_highestXValue(xValue, newPointIsValid);
            Update_lowestXValue(xValue, newPointIsValid);
        }

        void TryAddNotificationPoint_atTransitionFromInvalidPhaseAtLineStartToFirstValidPhase(bool newPointIsValid, float x_ofCurrentlyAddedPoint, float y_ofCurrentlyAddedPoint)
        {
            if (newPointIsValid)
            {
                if (float.IsNaN(mostCurrentValidYValue))
                {
                    //-> Currently added point is the first valid point of the line
                    if (dataPoints.Count > 0)
                    {
                        //-> The first valid dataPoint is not the first added point. There are invalid dataPoints with lower point-index:
                        InternalDXXL_DataPointOfChartLine thePrecedingDatapoint_thisIsGuaranteedInvalid = dataPoints[dataPoints.Count - 1];
                        if (thePrecedingDatapoint_thisIsGuaranteedInvalid.validState == InternalDXXL_DataPointOfChartLine.ValidState.isPlaceholderDuringPhasesWhereThisLineOfListDidntExistsDueToListChangedItsCount_existsOnlyToStayInXSyncWithOtherLinesOfList)
                        {
                            AddPointOfInterestWithoutHorizOrVertLine(x_ofCurrentlyAddedPoint, y_ofCurrentlyAddedPoint, "<color=#adadadFF><icon=logMessage></color> Line '" + name + "':<br>The index of this line didn't exist in the collection before this point (=> the Length/Count of the collection was raised).");
                        }
                        else
                        {
                            if (dataPoints.Count == 1)
                            {
                                AddPointOfInterestWithoutHorizOrVertLine(x_ofCurrentlyAddedPoint, y_ofCurrentlyAddedPoint, "<color=#ce0e0eFF><icon=logMessageError></color> Line '" + name + "':<br>The value before this point is invalid:<br>" + thePrecedingDatapoint_thisIsGuaranteedInvalid.GetInvalidTypeString());
                            }
                            else
                            {
                                AddPointOfInterestWithoutHorizOrVertLine(x_ofCurrentlyAddedPoint, y_ofCurrentlyAddedPoint, "<color=#ce0e0eFF><icon=logMessageError></color> Line '" + name + "':<br>The values before this point are invalid. The preceding point has:<br>" + thePrecedingDatapoint_thisIsGuaranteedInvalid.GetInvalidTypeString());
                            }
                        }
                    }
                }
            }
        }

        void TryAddNotificationPoint_atChangeOfInvalidTypeInsideInvalidPhase(bool newPointIsValid, InternalDXXL_DataPointOfChartLine newDataPoint)
        {
            if (float.IsNaN(mostCurrentValidYValue) == false) //this ensures also "(dataPoints.Count > 0)"
            {
                //->There has already been a valid phase:
                InternalDXXL_DataPointOfChartLine precedingDatapoint = dataPoints[dataPoints.Count - 1];
                if (newPointIsValid == false)
                {
                    if (precedingDatapoint.validState != InternalDXXL_DataPointOfChartLine.ValidState.isValid)
                    {
                        //->Currently added point and preceding point are both invalid:
                        if (precedingDatapoint.validState == InternalDXXL_DataPointOfChartLine.ValidState.isPlaceholderDuringPhasesWhereThisLineOfListDidntExistsDueToListChangedItsCount_existsOnlyToStayInXSyncWithOtherLinesOfList && newDataPoint.validState != InternalDXXL_DataPointOfChartLine.ValidState.isPlaceholderDuringPhasesWhereThisLineOfListDidntExistsDueToListChangedItsCount_existsOnlyToStayInXSyncWithOtherLinesOfList)
                        {
                            AddPointOfInterestWithoutHorizOrVertLine(newDataPoint.xValue, mostCurrentValidYValue, "<color=#ce0e0eFF><icon=logMessageError></color> Line '" + name + "':<br>The line started again here (=> the Length/Count of the collection was raised), but it starts with this invalid value:<br>" + newDataPoint.GetInvalidTypeString());
                        }

                        if (precedingDatapoint.validState != InternalDXXL_DataPointOfChartLine.ValidState.isPlaceholderDuringPhasesWhereThisLineOfListDidntExistsDueToListChangedItsCount_existsOnlyToStayInXSyncWithOtherLinesOfList && newDataPoint.validState == InternalDXXL_DataPointOfChartLine.ValidState.isPlaceholderDuringPhasesWhereThisLineOfListDidntExistsDueToListChangedItsCount_existsOnlyToStayInXSyncWithOtherLinesOfList)
                        {
                            AddPointOfInterestWithoutHorizOrVertLine(precedingDatapoint.xValue, mostCurrentValidYValue, "<color=#adadadFF><icon=logMessage></color> Line '" + name + "':<br>This is the last point of the line before the index of the line doesn't exist in the collection anymore. (=> the Length/Count of the collection was lowered).");
                        }
                    }
                }
            }
            else
            {
                //-> There was no valid phase before the currently added invalid point:
                //No notification is drawn, because
                //1) There is not yet a yValue availalbe where the pointer could be mounted.
                //2) "TryAddNotificationPoint_atTransitionFromInvalidPhaseAtLineStartToFirstValidPhase" will supply some information later as soon as the first valid phase starts, which answers at least the question: "Did the line start here (because the collection grew in Length/Count)?" or "Is the previous value NaN or Infinity?"
            }
        }

        void TryAddNotificationPoint_atTransitionFromValidPhaseToInvalidPhase(bool newPointIsValid, InternalDXXL_DataPointOfChartLine newDataPoint)
        {
            if (newPointIsValid == false)
            {
                if (float.IsNaN(mostCurrentValidYValue) == false) //this ensures also "(dataPoints.Count > 0)"
                {
                    //-> The currently added data point is invalid, but the line already contains a valid point:
                    InternalDXXL_DataPointOfChartLine thePrecedingDatapoint = dataPoints[dataPoints.Count - 1];
                    if (thePrecedingDatapoint.validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid)
                    {
                        InternalDXXL_DataPointOfChartLine thePrecedingDatapoint_thisIsGuaranteedValid = thePrecedingDatapoint;
                        if (newDataPoint.validState == InternalDXXL_DataPointOfChartLine.ValidState.isPlaceholderDuringPhasesWhereThisLineOfListDidntExistsDueToListChangedItsCount_existsOnlyToStayInXSyncWithOtherLinesOfList)
                        {
                            AddPointOfInterestWithoutHorizOrVertLine(thePrecedingDatapoint_thisIsGuaranteedValid.xValue, thePrecedingDatapoint_thisIsGuaranteedValid.yValue, "<color=#adadadFF><icon=logMessage></color> Line '" + name + "':<br>The index of this line doesn't exist in the collection anymore after this point. (=> the Length/Count of the collection was lowered).");
                        }
                        else
                        {
                            AddPointOfInterestWithoutHorizOrVertLine(thePrecedingDatapoint_thisIsGuaranteedValid.xValue, thePrecedingDatapoint_thisIsGuaranteedValid.yValue, "<color=#ce0e0eFF><icon=logMessageError></color> Line '" + name + "':<br>At least one value after this one is invalid. The following value has:<br>" + newDataPoint.GetInvalidTypeString());
                        }
                    }
                }
            }
        }

        void TryMarkPointWithEmphasizingCircle_dueTo_startOrEndOfValidPhase(ref InternalDXXL_DataPointOfChartLine theNewDataPoint)
        {
            if (dataPoints.Count > 0)
            {
                InternalDXXL_DataPointOfChartLine thePrecedingDatapoint = dataPoints[dataPoints.Count - 1];
                if (theNewDataPoint.validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid)
                {
                    if (thePrecedingDatapoint.validState != InternalDXXL_DataPointOfChartLine.ValidState.isValid)
                    {
                        theNewDataPoint.hasLittleEmphasizingCircleAroundPoint = true;
                    }
                }
                else
                {
                    if (thePrecedingDatapoint.validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid)
                    {
                        thePrecedingDatapoint.hasLittleEmphasizingCircleAroundPoint = true;
                    }
                }
            }
        }

        void TryMarkPointWithEmphasizingCircle_dueTo_forcedByUser(ref InternalDXXL_DataPointOfChartLine theNewDataPoint)
        {
            if (markNextNewlyCreatedPointWithEmphasizingCircle)
            {
                if (theMarkingOfTheNextNewlyCreatedPointWithEmphasizingCircle_isOnlyDoneIfThisPointIsValid)
                {
                    if (theNewDataPoint.validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid)
                    {
                        theNewDataPoint.hasLittleEmphasizingCircleAroundPoint = true;
                    }
                }
                else
                {
                    theNewDataPoint.hasLittleEmphasizingCircleAroundPoint = true;
                }
                markNextNewlyCreatedPointWithEmphasizingCircle = false;
            }
        }

        void NoteIf_hasAtLeastOneValuePairOfValidData(bool newPointIsValid)
        {
            if (hasAtLeastOneValuePairOfValidData == false)
            {
                if (newPointIsValid)
                {
                    hasAtLeastOneValuePairOfValidData = true;
                }
            }
        }

        void Update_highestYValue(float new_yValue, bool newPointIsValid)
        {
            if (newPointIsValid)
            {
                highestYValue = Mathf.Max(highestYValue, new_yValue);

                if (chart_thisLineIsPartOf.overallMaxYValue_includingHiddenLines == float.NaN)
                {
                    //-> is the first added data point of all lines
                    chart_thisLineIsPartOf.overallMaxYValue_includingHiddenLines = new_yValue;
                }
                else
                {
                    chart_thisLineIsPartOf.overallMaxYValue_includingHiddenLines = Mathf.Max(chart_thisLineIsPartOf.overallMaxYValue_includingHiddenLines, new_yValue);
                }
            }
        }

        void Update_lowestYValue(float new_yValue, bool newPointIsValid)
        {
            if (newPointIsValid)
            {
                lowestYValue = Mathf.Min(lowestYValue, new_yValue);

                if (chart_thisLineIsPartOf.overallMinYValue_includingHiddenLines == float.NaN)
                {
                    //-> is the first added data point of all lines
                    chart_thisLineIsPartOf.overallMinYValue_includingHiddenLines = new_yValue;
                }
                else
                {
                    chart_thisLineIsPartOf.overallMinYValue_includingHiddenLines = Mathf.Min(chart_thisLineIsPartOf.overallMinYValue_includingHiddenLines, new_yValue);
                }
            }
        }

        void Update_highestXValue(float new_xValue, bool newPointIsValid)
        {
            if (newPointIsValid)
            {
                highestXValue = Mathf.Max(highestXValue, new_xValue);

                if (chart_thisLineIsPartOf.overallMaxXValue_includingHiddenLines == float.NaN)
                {
                    //-> is the first added data point of all lines
                    chart_thisLineIsPartOf.overallMaxXValue_includingHiddenLines = new_xValue;
                }
                else
                {
                    chart_thisLineIsPartOf.overallMaxXValue_includingHiddenLines = Mathf.Max(chart_thisLineIsPartOf.overallMaxXValue_includingHiddenLines, new_xValue);
                }
            }
        }

        void Update_lowestXValue(float new_xValue, bool newPointIsValid)
        {
            if (newPointIsValid)
            {
                lowestXValue = Mathf.Min(lowestXValue, new_xValue);

                if (chart_thisLineIsPartOf.overallMinXValue_includingHiddenLines == float.NaN)
                {
                    //-> is the first added data point of all lines
                    chart_thisLineIsPartOf.overallMinXValue_includingHiddenLines = new_xValue;
                }
                else
                {
                    chart_thisLineIsPartOf.overallMinXValue_includingHiddenLines = Mathf.Min(chart_thisLineIsPartOf.overallMinXValue_includingHiddenLines, new_xValue);
                }
            }
        }

        public float GetMostCurrentValidXValue()
        {
            if (float.IsNaN(mostCurrentValidXValue))
            {
                UtilitiesDXXL_Log.PrintErrorCode("5"); //Why? The calling function already ensured that there is at least one valid data pair...
                return 0.0f;
            }
            else
            {
                return mostCurrentValidXValue;
            }
        }

        public float GetMostCurrentValidYValue()
        {
            if (float.IsNaN(mostCurrentValidYValue))
            {
                UtilitiesDXXL_Log.PrintErrorCode("6"); //Why? The calling function already ensured that there is at least one valid data pair...
                return 0.0f;
            }
            else
            {
                return mostCurrentValidYValue;
            }
        }

        public float GetLowestXValue()
        {
            return lowestXValue;
        }

        public float GetLowestYValue()
        {
            return lowestYValue;
        }

        public float GetHighestXValue()
        {
            return highestXValue;
        }

        public float GetHighestYValue()
        {
            return highestYValue;
        }

        public float GetLowestXValue_insideRestricedYSpan(float minAllowedY, float maxAllowedY)
        {
            float lowestXValue = float.PositiveInfinity;
            for (int i = 0; i < dataPoints.Count; i++)
            {
                float yValue_ofCurrentDatapoint = dataPoints[i].yValue;
                if ((yValue_ofCurrentDatapoint >= minAllowedY) && (yValue_ofCurrentDatapoint <= maxAllowedY))
                {
                    float xValue_ofCurrentDatapoint = dataPoints[i].xValue;
                    if (xValue_ofCurrentDatapoint < lowestXValue) { lowestXValue = xValue_ofCurrentDatapoint; }
                }
            }
            return lowestXValue;
        }

        public float GetLowestYValue_insideRestricedXSpan(float minAllowedX, float maxAllowedX)
        {
            float lowestYValue = float.PositiveInfinity;
            for (int i = 0; i < dataPoints.Count; i++)
            {
                float xValue_ofCurrentDatapoint = dataPoints[i].xValue;
                if ((xValue_ofCurrentDatapoint >= minAllowedX) && (xValue_ofCurrentDatapoint <= maxAllowedX))
                {
                    float yValue_ofCurrentDatapoint = dataPoints[i].yValue;
                    if (yValue_ofCurrentDatapoint < lowestYValue) { lowestYValue = yValue_ofCurrentDatapoint; }
                }
            }
            return lowestYValue;
        }

        public float GetHighestXValue_insideRestricedYSpan(float minAllowedY, float maxAllowedY)
        {
            float highestXValue = float.NegativeInfinity;
            for (int i = 0; i < dataPoints.Count; i++)
            {
                float yValue_ofCurrentDatapoint = dataPoints[i].yValue;
                if ((yValue_ofCurrentDatapoint >= minAllowedY) && (yValue_ofCurrentDatapoint <= maxAllowedY))
                {
                    float xValue_ofCurrentDatapoint = dataPoints[i].xValue;
                    if (xValue_ofCurrentDatapoint > highestXValue) { highestXValue = xValue_ofCurrentDatapoint; }
                }
            }
            return highestXValue;
        }

        public float GetHighestYValue_insideRestricedXSpan(float minAllowedX, float maxAllowedX)
        {
            float highestYValue = float.NegativeInfinity;
            for (int i = 0; i < dataPoints.Count; i++)
            {
                float xValue_ofCurrentDatapoint = dataPoints[i].xValue;
                if ((xValue_ofCurrentDatapoint >= minAllowedX) && (xValue_ofCurrentDatapoint <= maxAllowedX))
                {
                    float yValue_ofCurrentDatapoint = dataPoints[i].yValue;
                    if (yValue_ofCurrentDatapoint > highestYValue) { highestYValue = yValue_ofCurrentDatapoint; }
                }
            }
            return highestYValue;
        }

        public float GetCurrentAutomaticXValue()
        {
            switch (chart_thisLineIsPartOf.xAxis.sourceOfAutomaticValues)
            {
                case ChartAxis.SourceOfAutomaticValues.fixedStepForEachValueAdding:
                    return (float)dataPoints.Count;
                case ChartAxis.SourceOfAutomaticValues.fixedStep_followingTheManualIncrementFunction:
                    return (float)chart_thisLineIsPartOf.GetManuallyIncrementedXPos();
                case ChartAxis.SourceOfAutomaticValues.frameCount:
                    return (float)Time.frameCount;
                case ChartAxis.SourceOfAutomaticValues.fixedTime:
                    return Time.fixedTime;
                case ChartAxis.SourceOfAutomaticValues.fixedUnscaledTime:
                    return Time.fixedUnscaledTime;
                case ChartAxis.SourceOfAutomaticValues.realtimeSinceStartup:
                    return Time.realtimeSinceStartup;
                case ChartAxis.SourceOfAutomaticValues.time:
                    return Time.time;
                case ChartAxis.SourceOfAutomaticValues.timeSinceLevelLoad:
                    return Time.timeSinceLevelLoad;
                case ChartAxis.SourceOfAutomaticValues.unscaledTime:
                    return Time.unscaledTime;
                case ChartAxis.SourceOfAutomaticValues.editorTimeSinceStartup:
#if UNITY_EDITOR
                    return (float)UnityEditor.EditorApplication.timeSinceStartup;
#else
                    return Time.realtimeSinceStartup;
#endif
                default:
                    Debug.LogError("XValuesSource of " + chart_thisLineIsPartOf.xAxis.sourceOfAutomaticValues + " not implemented.");
                    return 0.0f;
            }
        }

        public void Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.IsDrawnBecause_theSingleComponentOfMulticomponentData_thisLineRepresents_isEnabledChecker IsDrawnCheckerFunction_thatRepresentsThe_singleComponentOfMulticomponentData)
        {
            IsDrawnBecause_theSingleComponentOfMulticomponentData_thisLineRepresents_isEnabled = IsDrawnCheckerFunction_thatRepresentsThe_singleComponentOfMulticomponentData;
        }

        public bool CheckIfLineIsDrawn()
        {
            return (lineSpecsForInspector.currentHideLineState == false) && CheckIfLineIsDrawn_accordingToTheEnabledStateOfThe_singleComponentOfMulticomponentDataThisLineRepresents();
        }

        bool CheckIfLineIsDrawn_accordingToTheEnabledStateOfThe_singleComponentOfMulticomponentDataThisLineRepresents()
        {
            return IsDrawnBecause_theSingleComponentOfMulticomponentData_thisLineRepresents_isEnabled(chart_thisLineIsPartOf.dataComponentsThatAreDrawn);
        }

        public bool IsHiddenOrUnhidden_butWithAtLeastOneValidOrInvalidDatapoint(bool includeLinesThatRepresentDisabledMultiComponentTypes = false)
        {
            if (includeLinesThatRepresentDisabledMultiComponentTypes || CheckIfLineIsDrawn_accordingToTheEnabledStateOfThe_singleComponentOfMulticomponentDataThisLineRepresents())
            {
                if (dataPoints.Count > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsUnhidden_andWithAtLeastOneValidOrInvalidDatapoint(bool includeLinesThatRepresentDisabledMultiComponentTypes = false)
        {
            if (includeLinesThatRepresentDisabledMultiComponentTypes || CheckIfLineIsDrawn_accordingToTheEnabledStateOfThe_singleComponentOfMulticomponentDataThisLineRepresents())
            {
                if (dataPoints.Count > 0)
                {
                    if (lineSpecsForInspector.currentHideLineState == false)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void AddPointOfInterest(PointOfInterest newPointOfInterest)
        {
            newPointOfInterest.chart_thisPointIsPartOf = chart_thisLineIsPartOf;
            newPointOfInterest.chartLine_thisPointIsPartOf = this;
            pointsOfInterest.Add(newPointOfInterest);
        }

        public PointOfInterest AddPointOfInterest(Vector2 position, string textToDisplay = null, DrawBasics.LineStyle horizLinestyle = DrawBasics.LineStyle.invisible, DrawBasics.LineStyle vertLinestyle = DrawBasics.LineStyle.invisible, float alphaOfColor_relToParent = 1.0f, bool getsDeletedOnClear = true)
        {
            return AddPointOfInterest(position.x, position.y, textToDisplay, horizLinestyle, vertLinestyle, alphaOfColor_relToParent, getsDeletedOnClear);
        }

        public PointOfInterest AddPointOfInterest(float position_x, float position_y, string textToDisplay = null, DrawBasics.LineStyle horizLinestyle = DrawBasics.LineStyle.invisible, DrawBasics.LineStyle vertLinestyle = DrawBasics.LineStyle.invisible, float alphaOfColor_relToParent = 1.0f, bool getsDeletedOnClear = true)
        {
            //the function returns the new point of interest, so it can then be accessed and be further modified.
            if (textToDisplay == null || textToDisplay == "")
            {
                if (horizLinestyle == DrawBasics.LineStyle.invisible) { horizLinestyle = DrawBasics.LineStyle.solid; }
                if (vertLinestyle == DrawBasics.LineStyle.invisible) { vertLinestyle = DrawBasics.LineStyle.solid; }
            }

            Color colorOfPoint = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(Color, alphaOfColor_relToParent);
            PointOfInterest newPointOfInterest = new PointOfInterest(position_x, position_y, colorOfPoint, chart_thisLineIsPartOf, this, textToDisplay);
            newPointOfInterest.isDeletedOnClear = getsDeletedOnClear;
            newPointOfInterest.xValue.lineStyle = vertLinestyle;
            newPointOfInterest.yValue.lineStyle = horizLinestyle;
            pointsOfInterest.Add(newPointOfInterest);
            return newPointOfInterest;
        }

        void DeletePointsOfInterestOnClear()
        {
            for (int i = pointsOfInterest.Count - 1; i >= 0; i--)
            {
                if (pointsOfInterest[i].isDeletedOnClear)
                {
                    pointsOfInterest.RemoveAt(i);
                }
            }
            turningPointDetector.Clear();
        }

        bool thePointsOfInterestOfThisLineArePartOfTheChart;
        public int Internal_Set_isDrawnInNextPass_forAllPointsOfInterest(int stillAvailableTextBoxes_inUpperRightCorner)
        {
            //Needs "line.Draw()" called beforehand (which sets "thePointsOfInterestOfThisLineArePartOfTheChart")
            if (thePointsOfInterestOfThisLineArePartOfTheChart)
            {
                for (int i = pointsOfInterest.Count - 1; i >= 0; i--) //-> counting DOWNWARD means: only the NEWEST textBoxes get drawn
                {
                    stillAvailableTextBoxes_inUpperRightCorner = pointsOfInterest[i].Internal_Set_isDrawnInNextPass(stillAvailableTextBoxes_inUpperRightCorner);
                }
            }
            return stillAvailableTextBoxes_inUpperRightCorner;
        }

        public Vector3 DrawPointsOfInterest(Vector3 next_lowAnchorPositionOfText_inWorldspace, float durationInSec, bool hiddenByNearerObjects)
        {
            //use ChartDrawing.Draw() instead

            //Use ChartDrawing.Draw()(<-link) instead.
            //Needs "line.Draw()" called beforehand (which sets "thePointsOfInterestOfThisLineArePartOfTheChart")
            if (thePointsOfInterestOfThisLineArePartOfTheChart)
            {
                UpdateColorsOfIntersectionPointMarkers(); //-> cannot be set onSetColor, because Unity serialization doesn't support C# properties
                UtilitiesDXXL_Math.SkewedDirection cornerOfChartWhereTextBoxIsMounted = UtilitiesDXXL_Math.SkewedDirection.upRight;
                for (int i = pointsOfInterest.Count - 1; i >= 0; i--) //-> counting DOWNWARDS, because this produces less crossings of the pointerLines in most cases for PointOfInterestBoxes on the upperLeftSide. Though this has the disadvantage, that the "newest/oldest"-ordering is the other way round than the PointOfInterestBoxes on the upperRightSide.
                {
                    next_lowAnchorPositionOfText_inWorldspace = pointsOfInterest[i].TryDraw(next_lowAnchorPositionOfText_inWorldspace, cornerOfChartWhereTextBoxIsMounted, durationInSec, hiddenByNearerObjects);
                }
            }
            return next_lowAnchorPositionOfText_inWorldspace;
        }

        void AddPointOfInterestWithoutHorizOrVertLine(float xPos, float yPos, string textToDisplay)
        {
            PointOfInterest newPointOfInterest = AddPointOfInterest(xPos, yPos, textToDisplay, DrawBasics.LineStyle.invisible, DrawBasics.LineStyle.invisible, 1.0f, true);
            newPointOfInterest.drawTextBoxIfPointIsOutsideOfChartArea = true;
            newPointOfInterest.forceColorOfParent = true;
        }


        public void AddHorizontalThresholdLine(float yPosition, bool lineItselfCountsToLowerArea = false, Color colorOfHorizLine = default(Color), bool hideThresholdLineAndShowOnlyTheIntersectionPointers = false)
        {
            //if you want to add the threshold to all lines at once you can use the "AddHorizontalThresholdLine"-version in "chartDrawing"
            //If you don't want to see the intersection points and only want to draw a horizontal line that doesn't do anything you can use 'chart.AddFixedHorizLine' instead.
            //"forceColor": if the color should be different from the line color
            // "lineItselfCountsToLowerArea" determines if it counts as an intersection if the line doesn't cross the threshold but runs exactly onto it.

            if (UtilitiesDXXL_Math.FloatIsInvalid(yPosition))
            {
                Debug.LogError("Cannot create threshold line at " + yPosition);
                return;
            }

            if (hideThresholdLineAndShowOnlyTheIntersectionPointers == false)
            {
                if (UtilitiesDXXL_Colors.IsDefaultColor(colorOfHorizLine))
                {
                    colorOfHorizLine = UtilitiesDXXL_Colors.GetSimilarColorWithSlightlyOtherBrightnessValue(Color);
                }

                PointOfInterest pointOfInterst_visualizingTheHorizThresholdLine = new PointOfInterest(0.0f, yPosition, colorOfHorizLine, Chart_thisLineIsPartOf, this, null);
                pointOfInterst_visualizingTheHorizThresholdLine.isDeletedOnClear = false;
                pointOfInterst_visualizingTheHorizThresholdLine.forceColorOfParent = false;
                pointOfInterst_visualizingTheHorizThresholdLine.xValue.lineStyle = DrawBasics.LineStyle.invisible;
                pointOfInterst_visualizingTheHorizThresholdLine.yValue.lineStyle = DrawBasics.LineStyle.solid;
                pointOfInterst_visualizingTheHorizThresholdLine.yValue.labelText = "Threshold";
                pointOfInterst_visualizingTheHorizThresholdLine.yValue.drawCoordinateAsText = true;
                pointOfInterst_visualizingTheHorizThresholdLine.yValue.lineExtent = DimensionOf_PointOfInterest.LineExtent.throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart;
                pointsOfInterest.Add(pointOfInterst_visualizingTheHorizThresholdLine);
            }

            InternalDXXL_HorizontalThresholdLine newHorizThresholdLine = new InternalDXXL_HorizontalThresholdLine(yPosition, this, lineItselfCountsToLowerArea);
            horizontalThresholdLines.Add(newHorizThresholdLine);
        }

        void CheckIntersectionWithHorizThresholdLines(InternalDXXL_DataPointOfChartLine theNewDataPoint)
        {
            if (dataPoints.Count > 0)
            {
                InternalDXXL_DataPointOfChartLine thePrecedingDatapoint = dataPoints[dataPoints.Count - 1];
                for (int i = 0; i < horizontalThresholdLines.Count; i++)
                {
                    PointOfInterest createdIntersection = horizontalThresholdLines[i].CheckIntersection(thePrecedingDatapoint, theNewDataPoint);
                    if (createdIntersection != null)
                    {
                        pointsOfInterest.Add(createdIntersection);
                        pointsOfInterest_fromIntersectionsWithHorizLines.Add(createdIntersection);
                    }
                }
            }
        }

        void UpdateColorsOfIntersectionPointMarkers()
        {
            if (pointsOfInterest_fromIntersectionsWithHorizLines.Count > 0)
            {
                Color colorOfGeneratedIntersectionPoints = UtilitiesDXXL_Colors.GetSimilarColorWithSlightlyOtherBrightnessValue(Color);
                colorOfGeneratedIntersectionPoints.a = 1.0f;
                for (int i = 0; i < pointsOfInterest_fromIntersectionsWithHorizLines.Count; i++)
                {
                    pointsOfInterest_fromIntersectionsWithHorizLines[i].SetWholeColor(colorOfGeneratedIntersectionPoints);
                }
            }
        }

        public void InternalAssignRepresentedGameobject(GameObject newGameobject)
        {
            gameobject_thatThisLineCurrentlyRepresents = newGameobject;
        }

        public void ForceMostCurrentConnectionLine_toLowAlpha()
        {
            if (dataPoints.Count > 0)
            {
                dataPoints[dataPoints.Count - 1].forceConnectionLineToLowAlpha = true;
            }
        }

        bool forceUpcomingNextCreatedConnectionLine_toLowAlpha = false;
        public void ForceUpcomingNextCreatedConnectionLine_toLowAlpha()
        {
            forceUpcomingNextCreatedConnectionLine_toLowAlpha = true;
        }

        bool markNextNewlyCreatedPointWithEmphasizingCircle = false;
        bool theMarkingOfTheNextNewlyCreatedPointWithEmphasizingCircle_isOnlyDoneIfThisPointIsValid = true;
        public void AddEmphasizingCircleToMostCurrentPoint(bool onlyIfPointIsValid = true, bool andMarkNextNewlyCreatedPointAsWell = false)
        {
            //if you want to mark only the next data point that gets created you can use "AddEmphasizingCircleToUpcomingNextDatapointThatGetsCreated"
            AddEmphasizingCircleToAPoint(dataPoints.Count - 1, onlyIfPointIsValid);
            markNextNewlyCreatedPointWithEmphasizingCircle = andMarkNextNewlyCreatedPointAsWell;
            theMarkingOfTheNextNewlyCreatedPointWithEmphasizingCircle_isOnlyDoneIfThisPointIsValid = onlyIfPointIsValid;
        }

        public void AddEmphasizingCircleToUpcomingNextDatapointThatGetsCreated(bool onlyIfPointIsValid = true)
        {
            //if you want to mark the most current already existing data point you can use "AddEmphasizingCircleToMostCurrentPoint"
            markNextNewlyCreatedPointWithEmphasizingCircle = true;
            theMarkingOfTheNextNewlyCreatedPointWithEmphasizingCircle_isOnlyDoneIfThisPointIsValid = onlyIfPointIsValid;
        }

        public void AddEmphasizingCircleToAPoint(int i_ofDataPoint, bool onlyIfPointIsValid = true)
        {
            if (i_ofDataPoint >= 0)
            {
                if (i_ofDataPoint < dataPoints.Count)
                {
                    if (onlyIfPointIsValid)
                    {
                        if (dataPoints[i_ofDataPoint].validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid)
                        {
                            dataPoints[i_ofDataPoint].hasLittleEmphasizingCircleAroundPoint = true;
                        }
                    }
                    else
                    {
                        dataPoints[i_ofDataPoint].hasLittleEmphasizingCircleAroundPoint = true;
                    }
                }
            }
        }

        void SetIfVisualizationOfSpecialApexPointsIsDisplayed()
        {
            SetIfVisualizationOfMostCurrentValueIsDisplayed();
            turningPointDetector.SetIfVisualizationIsDisplayed();
        }

        void SetIfVisualizationOfMostCurrentValueIsDisplayed()
        {
            if (dataPoints.Count > 0)
            {
                if (representsValuesFromAddedLists && (chart_thisLineIsPartOf.displayHighlightingOfMostCurrentValues_forLinesFromLists == false))
                {
                    pointOfInterest_thatHighlightsTheMostCurrentValue.xValue.lineStyle = DrawBasics.LineStyle.invisible;
                    pointOfInterest_thatHighlightsTheMostCurrentValue.yValue.lineStyle = DrawBasics.LineStyle.invisible;
                    return;
                }

                if (UtilitiesDXXL_Math.ApproximatelyZero(alpha_ofHighlighterForMostCurrentValue_xDim))
                {
                    pointOfInterest_thatHighlightsTheMostCurrentValue.xValue.lineStyle = DrawBasics.LineStyle.invisible;
                }
                else
                {
                    pointOfInterest_thatHighlightsTheMostCurrentValue.xValue.lineStyle = DrawBasics.LineStyle.solid;
                    pointOfInterest_thatHighlightsTheMostCurrentValue.xValue.color = new Color(1, 1, 1, alpha_ofHighlighterForMostCurrentValue_xDim); //-> only for setting alpha. The actual color gets forced from lineParent
                }

                if (UtilitiesDXXL_Math.ApproximatelyZero(alpha_ofHighlighterForMostCurrentValue_yDim))
                {
                    pointOfInterest_thatHighlightsTheMostCurrentValue.yValue.lineStyle = DrawBasics.LineStyle.invisible;
                }
                else
                {
                    pointOfInterest_thatHighlightsTheMostCurrentValue.yValue.lineStyle = DrawBasics.LineStyle.solid;
                    pointOfInterest_thatHighlightsTheMostCurrentValue.yValue.color = new Color(1, 1, 1, alpha_ofHighlighterForMostCurrentValue_yDim); //-> only for setting alpha. The actual color gets forced from lineParent
                }
                if (disableMinMaxYVisualizers_dueTo_lineRepresentsBoolValues) { pointOfInterest_thatHighlightsTheMostCurrentValue.yValue.lineStyle = DrawBasics.LineStyle.invisible; }
            }
            else
            {
                pointOfInterest_thatHighlightsTheMostCurrentValue.xValue.lineStyle = DrawBasics.LineStyle.invisible;
                pointOfInterest_thatHighlightsTheMostCurrentValue.yValue.lineStyle = DrawBasics.LineStyle.invisible;
            }
        }

        PointOfInterest CreatePointOfInterestThatHighlightsTheMostCurrentValue()
        {
            PointOfInterest created_pointOfInterest = new PointOfInterest(0.0f, 0.0f, DrawBasics.defaultColor, chart_thisLineIsPartOf, this, null);
            created_pointOfInterest.drawTextBoxIfPointIsOutsideOfChartArea = false;
            created_pointOfInterest.isDeletedOnClear = false;
            created_pointOfInterest.forceColorOfParent = true;

            created_pointOfInterest.xValue.lineStyle = DrawBasics.LineStyle.solid;
            created_pointOfInterest.xValue.drawCoordinateAsText = true;
            created_pointOfInterest.xValue.lineExtent = DimensionOf_PointOfInterest.LineExtent.axisToPoint;

            created_pointOfInterest.yValue.lineStyle = DrawBasics.LineStyle.solid;
            created_pointOfInterest.yValue.drawCoordinateAsText = false; //-> coordinate is coded into the labelText
            created_pointOfInterest.yValue.lineExtent = DimensionOf_PointOfInterest.LineExtent.axisToPoint;

            return created_pointOfInterest;
        }

        void UpdatePosOfVisualizationOfMostCurrentValue(InternalDXXL_DataPointOfChartLine newDataPoint)
        {
            if (newDataPoint.validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid)
            {
                pointOfInterest_thatHighlightsTheMostCurrentValue.xValue.position = newDataPoint.xValue;
                pointOfInterest_thatHighlightsTheMostCurrentValue.yValue.position = newDataPoint.yValue;

                string labelText_atHorizLine = null;
                if (displayDeltaAtHighlighterForMostCurrentValue)
                {
                    if (dataPoints.Count > 0)
                    {
                        InternalDXXL_DataPointOfChartLine thePrecedingDatapoint = dataPoints[dataPoints.Count - 1];
                        if (thePrecedingDatapoint.validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid)
                        {
                            float deltaSincePrecedingValue = newDataPoint.yValue - thePrecedingDatapoint.yValue;
                            if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(newDataPoint.yValue, thePrecedingDatapoint.yValue) || UtilitiesDXXL_Math.ApproximatelyZero(deltaSincePrecedingValue))
                            {
                                labelText_atHorizLine = "" + newDataPoint.yValue + "<size=4>  (<icon=arrowRight> +/- 0)</size>";
                            }
                            else
                            {
                                if (deltaSincePrecedingValue > 0.0f)
                                {
                                    Color risingValueColor = UtilitiesDXXL_Colors.green_boolTrue;
                                    risingValueColor.a = alpha_ofHighlighterForMostCurrentValue_yDim * 2.0f;
                                    labelText_atHorizLine = "" + newDataPoint.yValue + "<size=4>  (<color=#" + ColorUtility.ToHtmlStringRGBA(risingValueColor) + "><icon=arrowUp></color>" + deltaSincePrecedingValue + ")</size>";
                                }
                                else
                                {
                                    Color fallingValueColor = UtilitiesDXXL_Colors.red_boolFalse;
                                    fallingValueColor.a = alpha_ofHighlighterForMostCurrentValue_yDim * 2.0f;
                                    labelText_atHorizLine = "" + newDataPoint.yValue + "<size=4>  (<color=#" + ColorUtility.ToHtmlStringRGBA(fallingValueColor) + "><icon=arrowDown></color>" + deltaSincePrecedingValue + ")</size>";
                                }
                            }
                        }
                    }
                }

                if (labelText_atHorizLine == null) { labelText_atHorizLine = "" + newDataPoint.yValue; }
                pointOfInterest_thatHighlightsTheMostCurrentValue.yValue.labelText = labelText_atHorizLine;
            }
        }

        float Get_absWidthOfConnectionLines_worldSpace()
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(LineWidth_relToChartHeight))
            {
                return 0.0f;
            }
            else
            {
                return Mathf.Abs(LineWidth_relToChartHeight * Chart_thisLineIsPartOf.Height_inWorldSpace);
            }
        }

        float Get_absWidthOfPointVisualisatorLines_worldSpace()
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(pointVisualisationLineWidth_relToChartHeight))
            {
                return 0.0f;
            }
            else
            {
                return Mathf.Abs(pointVisualisationLineWidth_relToChartHeight * Chart_thisLineIsPartOf.Height_inWorldSpace);
            }
        }

        Vector3 Get_amplitudeDir_forNonZeroWidthLines(InternalDXXL_Plane chartPlane)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(LineWidth_relToChartHeight) && UtilitiesDXXL_Math.ApproximatelyZero(pointVisualisationLineWidth_relToChartHeight))
            {
                return default(Vector3);
            }
            else
            {
                if (UtilitiesDXXL_Math.IsQuaternionIdentity(chart_thisLineIsPartOf.InternalRotation))
                {
                    return default(Vector3);
                }
                else
                {
                    Vector3 aVectorInsideTheChartPlane = chartPlane.Get_projectionOfVectorOntoPlane(UtilitiesDXXL_Math.arbitrarySeldomDir_normalized_precalced);
                    Vector3 aVectorInsideTheChartPlane_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(aVectorInsideTheChartPlane);
                    Vector3 aVectorInsideTheChartPlane_veryLong = aVectorInsideTheChartPlane_normalized * 10000000.0f;
                    //-> Making it very long reduces jitter artifacts, that sometimes occur when drawing nonZeroWidth-Lines:
                    //-> Reproducing unwanted jitter:
                    //---> Turn the chart to non-default-orientation
                    //---> Make the chart size very small.
                    //---> Draw a line that makes a smooth round curve and therefore tangents all possible directions (like a parabola)
                    //---> Raise the lineWidth
                    //---> To emphasize the jitter: Scroll X during ComponentInspectionPhase.
                    //-> The problem may come from: float calculation imprecision during automatic amplitude calculation in "UtilitiesDXXL_LineAmplitudeAndTextDirCalculation"
                    //-> A clean solution would be not to use "arbitrarySeldomDir_normalized_precalced" but calculate "amplitudeDir_forNonZeroWidthLines" per datapoint via cross product, but this can get expensive for many datapoints. 
                    return aVectorInsideTheChartPlane_veryLong;
                }
            }
        }

        public void InitInspectionViaComponent()
        {
            lineSpecsForInspector.Reconstruct_arrayWithNeighboringDatapointValues();
        }

        public string GetNameCompound(bool addSpacesBeside_namePartsConnectingMinus)
        {
            if (addSpacesBeside_namePartsConnectingMinus)
            {
                if (name == null || name == "")
                {
                    if (nameExtraInfo == null || nameExtraInfo == "")
                    {
                        return "[nameless]";
                    }
                    else
                    {
                        return ("[nameless] - " + nameExtraInfo);
                    }
                }
                else
                {
                    if (nameExtraInfo == null || nameExtraInfo == "")
                    {
                        return name;
                    }
                    else
                    {
                        return (name + " - " + nameExtraInfo);
                    }
                }
            }
            else
            {
                if (name == null || name == "")
                {
                    if (nameExtraInfo == null || nameExtraInfo == "")
                    {
                        return "[nameless]";
                    }
                    else
                    {
                        return ("[nameless]-" + nameExtraInfo);
                    }
                }
                else
                {
                    if (nameExtraInfo == null || nameExtraInfo == "")
                    {
                        return name;
                    }
                    else
                    {
                        return (name + "-" + nameExtraInfo);
                    }
                }
            }
        }

    }

}
