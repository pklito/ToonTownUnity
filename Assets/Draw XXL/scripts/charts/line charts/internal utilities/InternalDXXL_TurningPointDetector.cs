namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class InternalDXXL_TurningPointDetector
    {
        List<InternalDXXL_TurningPoint> allUpperTurningPoints = new List<InternalDXXL_TurningPoint>();
        List<InternalDXXL_TurningPoint> allLowerTurningPoints = new List<InternalDXXL_TurningPoint>();
        List<InternalDXXL_TurningPoint> turningPoints_thatHighlightOnlyTheOverallHighestYValue = new List<InternalDXXL_TurningPoint>();
        List<InternalDXXL_TurningPoint> turningPoints_thatHighlightOnlyTheOverallLowestYValue = new List<InternalDXXL_TurningPoint>();

        int i_ofDatapoint_thatIsCurrentCandidateFor_startOfMaxPlateau;
        int i_ofDatapoint_thatIsCurrentCandidateFor_endOfMaxPlateau;
        int i_ofDatapoint_thatIsCurrentCandidateFor_startOfMinPlateau;
        int i_ofDatapoint_thatIsCurrentCandidateFor_endOfMinPlateau;
        int i_ofPrecedingDatapoint;

        ChartLine line_thisDetectorIsPartOf;

        public InternalDXXL_TurningPointDetector(ChartLine line_thisDetectorIsPartOf)
        {
            this.line_thisDetectorIsPartOf = line_thisDetectorIsPartOf;
            DiscardPrecedingPointsFromComparison();
        }

        public void Clear()
        {
            allUpperTurningPoints.Clear();
            allLowerTurningPoints.Clear();
            turningPoints_thatHighlightOnlyTheOverallHighestYValue.Clear();
            turningPoints_thatHighlightOnlyTheOverallLowestYValue.Clear();
            DiscardPrecedingPointsFromComparison();
        }

        public void DiscardPrecedingPointsFromComparison()
        {
            i_ofPrecedingDatapoint = -1;
            i_ofDatapoint_thatIsCurrentCandidateFor_startOfMaxPlateau = -1;
            i_ofDatapoint_thatIsCurrentCandidateFor_endOfMaxPlateau = -1;
            i_ofDatapoint_thatIsCurrentCandidateFor_startOfMinPlateau = -1;
            i_ofDatapoint_thatIsCurrentCandidateFor_endOfMinPlateau = -1;
        }

        public void AddNewDatapoint(int i_newDatapoint)
        {
            if (line_thisDetectorIsPartOf.dataPoints[i_newDatapoint].validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid)
            {
                if (i_ofPrecedingDatapoint >= 0)
                {
                    CheckMaxPlateau(i_newDatapoint);
                    CheckMinPlateau(i_newDatapoint);
                }
                i_ofPrecedingDatapoint = i_newDatapoint;
            }
            else
            {
                //New datapoint is invalid:
                DiscardPrecedingPointsFromComparison();
            }
        }

        void CheckMaxPlateau(int i_newDatapoint)
        {
            if (line_thisDetectorIsPartOf.dataPoints[i_newDatapoint].yValue > line_thisDetectorIsPartOf.dataPoints[i_ofPrecedingDatapoint].yValue)
            {
                i_ofDatapoint_thatIsCurrentCandidateFor_startOfMaxPlateau = i_newDatapoint;
                i_ofDatapoint_thatIsCurrentCandidateFor_endOfMaxPlateau = i_newDatapoint;
            }
            else
            {
                if (line_thisDetectorIsPartOf.dataPoints[i_newDatapoint].yValue == line_thisDetectorIsPartOf.dataPoints[i_ofPrecedingDatapoint].yValue)
                {
                    i_ofDatapoint_thatIsCurrentCandidateFor_endOfMaxPlateau = i_newDatapoint;
                }
                else
                {
                    if (i_ofDatapoint_thatIsCurrentCandidateFor_startOfMaxPlateau > 0)
                    {
                        CreateTurningPoint_atMaxPlateau();
                    }
                    i_ofDatapoint_thatIsCurrentCandidateFor_startOfMaxPlateau = -1;
                    i_ofDatapoint_thatIsCurrentCandidateFor_endOfMaxPlateau = -1;
                }
            }
        }

        void CreateTurningPoint_atMaxPlateau()
        {
            bool isPlateauConsistingOfTwoInsteadOfOnlyOnePointOfInterest = (i_ofDatapoint_thatIsCurrentCandidateFor_endOfMaxPlateau != i_ofDatapoint_thatIsCurrentCandidateFor_startOfMaxPlateau);
            //string labelText_atVertLine = isPlateauConsistingOfTwoInsteadOfOnlyOnePointOfInterest ? "start of max" : null; //->would be too packed in many situations
            InternalDXXL_TurningPoint turningPoint_atStartOfMaxPlateau = CreateTurningPoint(i_ofDatapoint_thatIsCurrentCandidateFor_startOfMaxPlateau, "max", false);
            InternalDXXL_TurningPoint turningPoint_atEndOfMaxPlateau = null;
            if (isPlateauConsistingOfTwoInsteadOfOnlyOnePointOfInterest)
            {
                turningPoint_atEndOfMaxPlateau = CreateTurningPoint(i_ofDatapoint_thatIsCurrentCandidateFor_endOfMaxPlateau, null, true);
            }

            bool addToListWithOverallHighestTurningPoints = CompareWith_overallHighestValues();

            allUpperTurningPoints.Add(turningPoint_atStartOfMaxPlateau);
            if (addToListWithOverallHighestTurningPoints) { turningPoints_thatHighlightOnlyTheOverallHighestYValue.Add(turningPoint_atStartOfMaxPlateau); }
            line_thisDetectorIsPartOf.AddPointOfInterest(turningPoint_atStartOfMaxPlateau.pointOfInterest_thatRepresentsThisTurningPoint);

            if (turningPoint_atEndOfMaxPlateau != null)
            {
                allUpperTurningPoints.Add(turningPoint_atEndOfMaxPlateau);
                if (addToListWithOverallHighestTurningPoints) { turningPoints_thatHighlightOnlyTheOverallHighestYValue.Add(turningPoint_atEndOfMaxPlateau); }
                line_thisDetectorIsPartOf.AddPointOfInterest(turningPoint_atEndOfMaxPlateau.pointOfInterest_thatRepresentsThisTurningPoint);
            }
        }

        bool CompareWith_overallHighestValues()
        {
            if (turningPoints_thatHighlightOnlyTheOverallHighestYValue.Count > 0)
            {
                if (line_thisDetectorIsPartOf.dataPoints[i_ofDatapoint_thatIsCurrentCandidateFor_startOfMaxPlateau].yValue > turningPoints_thatHighlightOnlyTheOverallHighestYValue[0].pointOfInterest_thatRepresentsThisTurningPoint.yValue.position)
                {
                    turningPoints_thatHighlightOnlyTheOverallHighestYValue.Clear();
                    return true;
                }
                else
                {
                    if (line_thisDetectorIsPartOf.dataPoints[i_ofDatapoint_thatIsCurrentCandidateFor_startOfMaxPlateau].yValue == turningPoints_thatHighlightOnlyTheOverallHighestYValue[0].pointOfInterest_thatRepresentsThisTurningPoint.yValue.position)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return true;
            }
        }

        void CheckMinPlateau(int i_newDatapoint)
        {
            if (line_thisDetectorIsPartOf.dataPoints[i_newDatapoint].yValue < line_thisDetectorIsPartOf.dataPoints[i_ofPrecedingDatapoint].yValue)
            {
                i_ofDatapoint_thatIsCurrentCandidateFor_startOfMinPlateau = i_newDatapoint;
                i_ofDatapoint_thatIsCurrentCandidateFor_endOfMinPlateau = i_newDatapoint;
            }
            else
            {
                if (line_thisDetectorIsPartOf.dataPoints[i_newDatapoint].yValue == line_thisDetectorIsPartOf.dataPoints[i_ofPrecedingDatapoint].yValue)
                {
                    i_ofDatapoint_thatIsCurrentCandidateFor_endOfMinPlateau = i_newDatapoint;
                }
                else
                {
                    if (i_ofDatapoint_thatIsCurrentCandidateFor_startOfMinPlateau > 0)
                    {
                        CreateTurningPointOf_atMinPlateau();
                    }
                    i_ofDatapoint_thatIsCurrentCandidateFor_startOfMinPlateau = -1;
                    i_ofDatapoint_thatIsCurrentCandidateFor_endOfMinPlateau = -1;
                }
            }
        }

        void CreateTurningPointOf_atMinPlateau()
        {
            bool isPlateauConsistingOfTwoInsteadOfOnlyOnePointOfInterest = (i_ofDatapoint_thatIsCurrentCandidateFor_endOfMinPlateau != i_ofDatapoint_thatIsCurrentCandidateFor_startOfMinPlateau);
            //string labelText_atVertLine = isPlateauConsistingOfTwoInsteadOfOnlyOnePointOfInterest ? "start of min" : null;//->would be too packed in many situations
            InternalDXXL_TurningPoint turningPoint_atStartOfMinPlateau = CreateTurningPoint(i_ofDatapoint_thatIsCurrentCandidateFor_startOfMinPlateau, "min", false);
            InternalDXXL_TurningPoint turningPoint_atEndOfMinPlateau = null;
            if (isPlateauConsistingOfTwoInsteadOfOnlyOnePointOfInterest)
            {
                turningPoint_atEndOfMinPlateau = CreateTurningPoint(i_ofDatapoint_thatIsCurrentCandidateFor_endOfMinPlateau, null, true);
            }

            bool addToListWithOverallLowestTurningPoints = CompareWith_overallLowestValues();

            allLowerTurningPoints.Add(turningPoint_atStartOfMinPlateau);
            if (addToListWithOverallLowestTurningPoints) { turningPoints_thatHighlightOnlyTheOverallLowestYValue.Add(turningPoint_atStartOfMinPlateau); }
            line_thisDetectorIsPartOf.AddPointOfInterest(turningPoint_atStartOfMinPlateau.pointOfInterest_thatRepresentsThisTurningPoint);

            if (turningPoint_atEndOfMinPlateau != null)
            {
                allLowerTurningPoints.Add(turningPoint_atEndOfMinPlateau);
                if (addToListWithOverallLowestTurningPoints) { turningPoints_thatHighlightOnlyTheOverallLowestYValue.Add(turningPoint_atEndOfMinPlateau); }
                line_thisDetectorIsPartOf.AddPointOfInterest(turningPoint_atEndOfMinPlateau.pointOfInterest_thatRepresentsThisTurningPoint);
            }
        }

        bool CompareWith_overallLowestValues()
        {
            if (turningPoints_thatHighlightOnlyTheOverallLowestYValue.Count > 0)
            {
                if (line_thisDetectorIsPartOf.dataPoints[i_ofDatapoint_thatIsCurrentCandidateFor_startOfMinPlateau].yValue < turningPoints_thatHighlightOnlyTheOverallLowestYValue[0].pointOfInterest_thatRepresentsThisTurningPoint.yValue.position)
                {
                    turningPoints_thatHighlightOnlyTheOverallLowestYValue.Clear();
                    return true;
                }
                else
                {
                    if (line_thisDetectorIsPartOf.dataPoints[i_ofDatapoint_thatIsCurrentCandidateFor_startOfMinPlateau].yValue == turningPoints_thatHighlightOnlyTheOverallLowestYValue[0].pointOfInterest_thatRepresentsThisTurningPoint.yValue.position)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            else
            {
                return true;
            }
        }

        InternalDXXL_TurningPoint CreateTurningPoint(int i_ofDatapointWhereToCreate, string labelText_atHorizLine, bool isEndOfPlateau)
        {
            float position_x = line_thisDetectorIsPartOf.dataPoints[i_ofDatapointWhereToCreate].xValue;
            float position_y = line_thisDetectorIsPartOf.dataPoints[i_ofDatapointWhereToCreate].yValue;
            PointOfInterest createdPointOfInterest = new PointOfInterest(position_x, position_y, DrawBasics.defaultColor, line_thisDetectorIsPartOf.Chart_thisLineIsPartOf, line_thisDetectorIsPartOf, null);
            createdPointOfInterest.drawTextBoxIfPointIsOutsideOfChartArea = false;
            createdPointOfInterest.isDeletedOnClear = true;
            createdPointOfInterest.forceColorOfParent = true;
            //createdPointOfInterest.xValue.labelText = labelText_atVertLine;
            createdPointOfInterest.xValue.labelText = null;
            createdPointOfInterest.xValue.drawCoordinateAsText = true;
            createdPointOfInterest.xValue.lineExtent = DimensionOf_PointOfInterest.LineExtent.axisToPoint;
            createdPointOfInterest.xValue.linestylePatternScaleFactor = 0.8f;
            createdPointOfInterest.yValue.labelText = labelText_atHorizLine + " = " + line_thisDetectorIsPartOf.dataPoints[i_ofDatapointWhereToCreate].yValue;
            createdPointOfInterest.yValue.drawCoordinateAsText = false;
            createdPointOfInterest.yValue.lineExtent = DimensionOf_PointOfInterest.LineExtent.axisToPoint;

            InternalDXXL_TurningPoint createdTurningPoint = new InternalDXXL_TurningPoint();
            createdTurningPoint.pointOfInterest_thatRepresentsThisTurningPoint = createdPointOfInterest;
            createdTurningPoint.isTheEndOfAPlateau = isEndOfPlateau;

            return createdTurningPoint;
        }

        public void SetIfVisualizationIsDisplayed()
        {
            SetIfVisualizationOfYMaximumTurningPointsIsDisplayed();
            SetIfVisualizationOfYMinimumTurningPointsIsDisplayed();
            SetIfVisualizationOfSingleHighestValueIsDisplayed();
            SetIfVisualizationOfSingleLowestValueIsDisplayed();
        }

        void SetIfVisualizationOfYMaximumTurningPointsIsDisplayed()
        {
            if (line_thisDetectorIsPartOf.markAllYMaximumTurningPoints)
            {
                SetVisibleState_forListOf_pointsOfInterest(true, allUpperTurningPoints, true, false);
            }
            else
            {
                SetVisibleState_forListOf_pointsOfInterest(false, allUpperTurningPoints, true, false);
            }
        }

        void SetIfVisualizationOfYMinimumTurningPointsIsDisplayed()
        {
            if (line_thisDetectorIsPartOf.markAllYMinimumTurningPoints)
            {
                SetVisibleState_forListOf_pointsOfInterest(true, allLowerTurningPoints, false, false);
            }
            else
            {
                SetVisibleState_forListOf_pointsOfInterest(false, allLowerTurningPoints, false, false);
            }
        }

        void SetIfVisualizationOfSingleHighestValueIsDisplayed()
        {
            if (line_thisDetectorIsPartOf.markAllYMaximumTurningPoints == false)
            {
                //-> all turningPoints are turned to invisible, if arriving here
                if (UtilitiesDXXL_Math.ApproximatelyZero(line_thisDetectorIsPartOf.alpha_ofMaxiumumYValueMarker) == false)
                {
                    SetVisibleState_forListOf_pointsOfInterest(true, turningPoints_thatHighlightOnlyTheOverallHighestYValue, true, true);
                }
            }
        }

        void SetIfVisualizationOfSingleLowestValueIsDisplayed()
        {
            if (line_thisDetectorIsPartOf.markAllYMinimumTurningPoints == false)
            {
                //-> all turningPoints are turned to invisible, if arriving here
                if (UtilitiesDXXL_Math.ApproximatelyZero(line_thisDetectorIsPartOf.alpha_ofMinimumYValueMarker) == false)
                {
                    SetVisibleState_forListOf_pointsOfInterest(true, turningPoints_thatHighlightOnlyTheOverallLowestYValue, false, true);
                }
            }
        }

        void SetVisibleState_forListOf_pointsOfInterest(bool shouldBeVisible, List<InternalDXXL_TurningPoint> concernedTurningPoints, bool isHighTurningPoint_notLow, bool forceInvisibleIfItIsNotTheMostExtremePointOfTheWholeLine)
        {
            if (shouldBeVisible)
            {
                MarkPointsIfTheyAre_theMostRightOf_theNonPlataueEndTurningPointsAtSameYHeight(concernedTurningPoints);
                for (int i = 0; i < concernedTurningPoints.Count; i++)
                {
                    if (isHighTurningPoint_notLow)
                    {
                        //high turning points:
                        if (forceInvisibleIfItIsNotTheMostExtremePointOfTheWholeLine)
                        {
                            //-> The first and the last datapoint or datapoints beside invalid values can be more extreme but don't count as turning points
                            float yOfTurningPoint = concernedTurningPoints[i].pointOfInterest_thatRepresentsThisTurningPoint.yValue.position;
                            if (yOfTurningPoint < line_thisDetectorIsPartOf.HighestYValue) { continue; }
                        }
                        concernedTurningPoints[i].pointOfInterest_thatRepresentsThisTurningPoint.xValue.color = new Color(1, 1, 1, line_thisDetectorIsPartOf.alpha_ofMaxiumumYValueMarker); //-> only for setting alpha. The actual color gets forced from lineParent
                        concernedTurningPoints[i].pointOfInterest_thatRepresentsThisTurningPoint.yValue.color = new Color(1, 1, 1, line_thisDetectorIsPartOf.alpha_ofMaxiumumYValueMarker); //-> only for setting alpha. The actual color gets forced from lineParent
                    }
                    else
                    {
                        //low turning points:
                        if (forceInvisibleIfItIsNotTheMostExtremePointOfTheWholeLine)
                        {
                            //-> The first and the last datapoint or datapoints beside invalid values can be more extreme but don't count as turning points
                            float yOfTurningPoint = concernedTurningPoints[i].pointOfInterest_thatRepresentsThisTurningPoint.yValue.position;
                            if (yOfTurningPoint > line_thisDetectorIsPartOf.LowestYValue) { continue; }
                        }
                        concernedTurningPoints[i].pointOfInterest_thatRepresentsThisTurningPoint.xValue.color = new Color(1, 1, 1, line_thisDetectorIsPartOf.alpha_ofMinimumYValueMarker); //-> only for setting alpha. The actual color gets forced from lineParent
                        concernedTurningPoints[i].pointOfInterest_thatRepresentsThisTurningPoint.yValue.color = new Color(1, 1, 1, line_thisDetectorIsPartOf.alpha_ofMinimumYValueMarker); //-> only for setting alpha. The actual color gets forced from lineParent
                    }

                    DrawBasics.LineStyle linestyle_forHorizLine = DrawBasics.LineStyle.solid;
                    if (concernedTurningPoints[i].isTheEndOfAPlateau) { linestyle_forHorizLine = DrawBasics.LineStyle.invisible; }
                    if (concernedTurningPoints[i].isTheMostRightOf_theNonPlataueEndTurningPointsAtSameYHeight == false) { linestyle_forHorizLine = DrawBasics.LineStyle.invisible; } //-> This prevents multiple points at the same yHeight from adding up their alpha from the horizontal low alpha turningPointMarkerLines until it is almost alpha=1 and is not distinguishable anymore from the dataLine itself.
                    if (line_thisDetectorIsPartOf.disableMinMaxYVisualizers_dueTo_lineRepresentsBoolValues) { linestyle_forHorizLine = DrawBasics.LineStyle.invisible; }

                    concernedTurningPoints[i].pointOfInterest_thatRepresentsThisTurningPoint.xValue.lineStyle = DrawBasics.LineStyle.dashed;
                    concernedTurningPoints[i].pointOfInterest_thatRepresentsThisTurningPoint.yValue.lineStyle = linestyle_forHorizLine;
                }
            }
            else
            {
                for (int i = 0; i < concernedTurningPoints.Count; i++)
                {
                    concernedTurningPoints[i].pointOfInterest_thatRepresentsThisTurningPoint.xValue.lineStyle = DrawBasics.LineStyle.invisible;
                    concernedTurningPoints[i].pointOfInterest_thatRepresentsThisTurningPoint.yValue.lineStyle = DrawBasics.LineStyle.invisible;
                }
            }
        }

        void MarkPointsIfTheyAre_theMostRightOf_theNonPlataueEndTurningPointsAtSameYHeight(List<InternalDXXL_TurningPoint> concernedTurningPoints)
        {
            for (int i_currCheckedTurningPoint = 0; i_currCheckedTurningPoint < concernedTurningPoints.Count; i_currCheckedTurningPoint++)
            {
                if (concernedTurningPoints[i_currCheckedTurningPoint].isTheEndOfAPlateau)
                {
                    concernedTurningPoints[i_currCheckedTurningPoint].isTheMostRightOf_theNonPlataueEndTurningPointsAtSameYHeight = false;
                    continue;
                }
                else
                {
                    concernedTurningPoints[i_currCheckedTurningPoint].isTheMostRightOf_theNonPlataueEndTurningPointsAtSameYHeight = true;
                }

                for (int i_currLowerComparedTurningPoint = 0; i_currLowerComparedTurningPoint < i_currCheckedTurningPoint; i_currLowerComparedTurningPoint++)
                {
                    if (concernedTurningPoints[i_currLowerComparedTurningPoint].isTheEndOfAPlateau)
                    {
                        continue;
                    }
                    else
                    {
                        float y_ofCurrCheckedTurningPoint = concernedTurningPoints[i_currCheckedTurningPoint].pointOfInterest_thatRepresentsThisTurningPoint.yValue.position;
                        float y_ofCurrLowerComparedTurningPoint = concernedTurningPoints[i_currLowerComparedTurningPoint].pointOfInterest_thatRepresentsThisTurningPoint.yValue.position;
                        if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(y_ofCurrCheckedTurningPoint, y_ofCurrLowerComparedTurningPoint))
                        {
                            float x_ofCurrCheckedTurningPoint = concernedTurningPoints[i_currCheckedTurningPoint].pointOfInterest_thatRepresentsThisTurningPoint.xValue.position;
                            float x_ofCurrLowerComparedTurningPoint = concernedTurningPoints[i_currLowerComparedTurningPoint].pointOfInterest_thatRepresentsThisTurningPoint.xValue.position;
                            if (x_ofCurrCheckedTurningPoint > x_ofCurrLowerComparedTurningPoint)
                            {
                                concernedTurningPoints[i_currLowerComparedTurningPoint].isTheMostRightOf_theNonPlataueEndTurningPointsAtSameYHeight = false;
                            }
                            else
                            {
                                concernedTurningPoints[i_currCheckedTurningPoint].isTheMostRightOf_theNonPlataueEndTurningPointsAtSameYHeight = false;
                                break;
                            }
                        }
                    }
                }
            }
        }

    }

}
