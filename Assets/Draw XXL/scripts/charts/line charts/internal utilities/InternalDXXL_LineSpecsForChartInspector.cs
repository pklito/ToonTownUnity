namespace DrawXXL
{
    using System;
    using UnityEngine;

    [Serializable]
    public class InternalDXXL_LineSpecsForChartInspector
    {
        static int default_numberOfDisplayedDatapointsInArray = 7;
        public static bool lineCompoundNames_haveSpaces_betweenTheNamePartConnectingMinus = true;

        public ChartLine line_theseSpecsBelongTo;
        public bool currentHideLineState;
        public bool currentHideCursorXState_duringComponentInspectionPhase = true;
        public bool currentHideCursorYState_duringComponentInspectionPhase = false;
        public Color lineColor;
        public ChartLine.LineConnectionsType connectionsType;
        public ChartLine.DataPointVisualization dataPointVisualization;
        [Range(0.002f, 0.3f)] public float dataPointVisualization_size;
        [Range(0.0f, 1.0f)] public float alpha_ofVertFillLines;
        [Range(0.0f, 0.2f)] public float lineWidth;
        public string linesCompoundName;
        [SerializeField] int numberOfDisplayedDatapointsInArray = default_numberOfDisplayedDatapointsInArray;
        [SerializeField] InternalDXXL_NeighboringDatapointForChartInspector[] neighboringDatapointValues;
        [SerializeField] int i_insideShortenedForInspectorArray_markingTheValueAtCursor;
        [SerializeField] bool lineSection_isExpanded = false;
        [SerializeField] bool datapointValuesSection_isExpanded = false;
        int i_ofValueInsideLinesDatapointListThatIsXNearestToCursor;
        public bool hasValidDatapointThatIsXNearestToCursor = false;
        public Vector2 posInChartspace_ofDatapointThatIsNearestToCursor;
        public int i_ofThisLineSpec_insideChartInspectorsLinesSpecsArray;

        public void TryRefill_arrayWithNeighboringDatapointValues()
        {
            TryReconstruct_arrayWithNeighboringDatapointValues();
            if (lineSection_isExpanded && datapointValuesSection_isExpanded)
            {
                for (int i_currentlyFilledSlot_inShortenedInspectorArray = 0; i_currentlyFilledSlot_inShortenedInspectorArray < neighboringDatapointValues.Length; i_currentlyFilledSlot_inShortenedInspectorArray++)
                {
                    if (hasValidDatapointThatIsXNearestToCursor)
                    {
                        int slotsDiffernceToCursorSlot = i_currentlyFilledSlot_inShortenedInspectorArray - i_insideShortenedForInspectorArray_markingTheValueAtCursor;
                        int i_insideLinesDatapointList = i_ofValueInsideLinesDatapointListThatIsXNearestToCursor + slotsDiffernceToCursorSlot;
                        if (i_insideLinesDatapointList < 0 || i_insideLinesDatapointList >= line_theseSpecsBelongTo.dataPoints.Count)
                        {
                            neighboringDatapointValues[i_currentlyFilledSlot_inShortenedInspectorArray].x = float.NaN;
                            neighboringDatapointValues[i_currentlyFilledSlot_inShortenedInspectorArray].y = float.NaN;
                            neighboringDatapointValues[i_currentlyFilledSlot_inShortenedInspectorArray].deltaSincePrecedingY = float.NaN;
                        }
                        else
                        {
                            neighboringDatapointValues[i_currentlyFilledSlot_inShortenedInspectorArray].x = line_theseSpecsBelongTo.dataPoints[i_insideLinesDatapointList].xValue;
                            neighboringDatapointValues[i_currentlyFilledSlot_inShortenedInspectorArray].y = line_theseSpecsBelongTo.dataPoints[i_insideLinesDatapointList].yValue;

                            if (i_insideLinesDatapointList < 1)
                            {
                                neighboringDatapointValues[i_currentlyFilledSlot_inShortenedInspectorArray].deltaSincePrecedingY = float.NaN;
                            }
                            else
                            {
                                neighboringDatapointValues[i_currentlyFilledSlot_inShortenedInspectorArray].deltaSincePrecedingY = line_theseSpecsBelongTo.dataPoints[i_insideLinesDatapointList].yValue - line_theseSpecsBelongTo.dataPoints[i_insideLinesDatapointList - 1].yValue;
                            }
                        }
                    }
                    else
                    {
                        //Obtainment of "i_ofValueInsideLinesDatapointListThatIsXNearestToCursor" failed:
                        neighboringDatapointValues[i_currentlyFilledSlot_inShortenedInspectorArray].x = float.NaN;
                        neighboringDatapointValues[i_currentlyFilledSlot_inShortenedInspectorArray].y = float.NaN;
                        neighboringDatapointValues[i_currentlyFilledSlot_inShortenedInspectorArray].deltaSincePrecedingY = float.NaN;
                    }
                }
            }
        }

        void TryReconstruct_arrayWithNeighboringDatapointValues()
        {
            numberOfDisplayedDatapointsInArray = Mathf.Max(numberOfDisplayedDatapointsInArray, 3);
            numberOfDisplayedDatapointsInArray = Mathf.Min(numberOfDisplayedDatapointsInArray, 200); //-> why not exporting to CSV if many datapoints are needed? The editor will have performance problems with higher values
            if (numberOfDisplayedDatapointsInArray != neighboringDatapointValues.Length) { Reconstruct_arrayWithNeighboringDatapointValues(); }
        }

        public void Reconstruct_arrayWithNeighboringDatapointValues()
        {
            neighboringDatapointValues = new InternalDXXL_NeighboringDatapointForChartInspector[numberOfDisplayedDatapointsInArray];
            i_insideShortenedForInspectorArray_markingTheValueAtCursor = Mathf.RoundToInt(0.5f * (neighboringDatapointValues.Length - 1));
            for (int i = 0; i < neighboringDatapointValues.Length; i++)
            {
                neighboringDatapointValues[i] = new InternalDXXL_NeighboringDatapointForChartInspector();
            }
        }

        public void Recalc_posThatIsXNearestToCursor(float xValueOfCursor_inChartspace)
        {
            Calc_i_ofValueInsideLinesDatapointListThatIsXNearestToCursor(xValueOfCursor_inChartspace);
            hasValidDatapointThatIsXNearestToCursor = (i_ofValueInsideLinesDatapointListThatIsXNearestToCursor >= 0);
            if (hasValidDatapointThatIsXNearestToCursor)
            {
                posInChartspace_ofDatapointThatIsNearestToCursor = new Vector2(line_theseSpecsBelongTo.dataPoints[i_ofValueInsideLinesDatapointListThatIsXNearestToCursor].xValue, line_theseSpecsBelongTo.dataPoints[i_ofValueInsideLinesDatapointListThatIsXNearestToCursor].yValue);
            }
            else
            {
                posInChartspace_ofDatapointThatIsNearestToCursor = new Vector2(xValueOfCursor_inChartspace, 0.5f);
            }
        }

        void Calc_i_ofValueInsideLinesDatapointListThatIsXNearestToCursor(float xValueOfCursor_inChartspace)
        {
            if (line_theseSpecsBelongTo.AllXValuesCameFromAutomaticSource)
            {
                //performance saving assumption: the x values always rise
                int i_nearestBelowCursor = -1;
                int i_nearestAboveCursor = line_theseSpecsBelongTo.dataPoints.Count;
                RestrictPossibleCandidates(10000, out i_nearestBelowCursor, out i_nearestAboveCursor, i_nearestBelowCursor, i_nearestAboveCursor, xValueOfCursor_inChartspace);
                RestrictPossibleCandidates(1000, out i_nearestBelowCursor, out i_nearestAboveCursor, i_nearestBelowCursor, i_nearestAboveCursor, xValueOfCursor_inChartspace);
                RestrictPossibleCandidates(100, out i_nearestBelowCursor, out i_nearestAboveCursor, i_nearestBelowCursor, i_nearestAboveCursor, xValueOfCursor_inChartspace);
                RestrictPossibleCandidates(10, out i_nearestBelowCursor, out i_nearestAboveCursor, i_nearestBelowCursor, i_nearestAboveCursor, xValueOfCursor_inChartspace);

                if ((i_nearestBelowCursor < 0) && (i_nearestAboveCursor >= line_theseSpecsBelongTo.dataPoints.Count))
                {
                    //pre-checks that restrict the datapoint candidates failed:
                    i_ofValueInsideLinesDatapointListThatIsXNearestToCursor = Get_i_datapointThatIsXNearestToCursor_viaBruteForceCheckAllDatapoints(xValueOfCursor_inChartspace);
                }
                else
                {
                    if (i_nearestBelowCursor == i_nearestAboveCursor) { i_ofValueInsideLinesDatapointListThatIsXNearestToCursor = i_nearestBelowCursor; return; }

                    //cursor is left of whole line:
                    if (i_nearestAboveCursor <= 0) { i_ofValueInsideLinesDatapointListThatIsXNearestToCursor = 0; return; }

                    //cursor is right of whole line:
                    if (i_nearestBelowCursor >= (line_theseSpecsBelongTo.dataPoints.Count - 1)) { i_ofValueInsideLinesDatapointListThatIsXNearestToCursor = (line_theseSpecsBelongTo.dataPoints.Count - 1); return; }

                    i_nearestBelowCursor = Mathf.Max(i_nearestBelowCursor, 0);
                    i_nearestAboveCursor = Mathf.Min(i_nearestAboveCursor, (line_theseSpecsBelongTo.dataPoints.Count - 1));
                    i_ofValueInsideLinesDatapointListThatIsXNearestToCursor = Get_i_datapointThatIsXNearestToCursor_ofDatapointSpan(xValueOfCursor_inChartspace, i_nearestBelowCursor, i_nearestAboveCursor);
                }
            }
            else
            {
                //no assumption that the x values always rise:
                i_ofValueInsideLinesDatapointListThatIsXNearestToCursor = Get_i_datapointThatIsXNearestToCursor_viaBruteForceCheckAllDatapoints(xValueOfCursor_inChartspace);
            }
        }

        void RestrictPossibleCandidates(int indexDistanceBetweenEachCheckedDatapoint, out int i_nearestBelowCursor, out int i_nearestAboveCursor, int i_nearestBelowCursor_atStartOfCheck, int i_nearestAboveCursor_atStartOfCheck, float xValueOfCursor_inChartspace)
        {
            i_nearestBelowCursor = i_nearestBelowCursor_atStartOfCheck;
            i_nearestAboveCursor = i_nearestAboveCursor_atStartOfCheck;

            indexDistanceBetweenEachCheckedDatapoint = Mathf.Max(indexDistanceBetweenEachCheckedDatapoint, 1);
            for (int i_currentlyCheckedDatapoint = i_nearestBelowCursor_atStartOfCheck; i_currentlyCheckedDatapoint <= i_nearestAboveCursor_atStartOfCheck;)
            {
                if (i_currentlyCheckedDatapoint >= 0 && i_currentlyCheckedDatapoint < line_theseSpecsBelongTo.dataPoints.Count)
                {
                    if (line_theseSpecsBelongTo.dataPoints[i_currentlyCheckedDatapoint].validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid)
                    {
                        if (line_theseSpecsBelongTo.dataPoints[i_currentlyCheckedDatapoint].xValue < xValueOfCursor_inChartspace)
                        {
                            i_nearestBelowCursor = i_currentlyCheckedDatapoint;
                        }
                        else
                        {
                            i_nearestAboveCursor = i_currentlyCheckedDatapoint;
                            break;
                        }
                    }
                }
                i_currentlyCheckedDatapoint = i_currentlyCheckedDatapoint + indexDistanceBetweenEachCheckedDatapoint;
            }
        }

        int Get_i_datapointThatIsXNearestToCursor_viaBruteForceCheckAllDatapoints(float xValueOfCursor_inChartspace)
        {
            return Get_i_datapointThatIsXNearestToCursor_ofDatapointSpan(xValueOfCursor_inChartspace, 0, line_theseSpecsBelongTo.dataPoints.Count - 1);
        }

        int Get_i_datapointThatIsXNearestToCursor_ofDatapointSpan(float xValueOfCursor_inChartspace, int i_lowSpanEnd, int i_highSpanEnd)
        {
            //Debug.Log("span: <b>" + (i_highSpanEnd - i_lowSpanEnd) + "</b>    i_lowSpanEnd : "+ i_lowSpanEnd + "    i_highSpanEnd: "+ i_highSpanEnd);
            int i_ofCurrentlyNearest = -1;
            float absDistance_ofCurrentlyNearest = float.PositiveInfinity;
            for (int i_currentlyCheckedDatapoint = i_lowSpanEnd; i_currentlyCheckedDatapoint <= i_highSpanEnd; i_currentlyCheckedDatapoint++)
            {
                if (line_theseSpecsBelongTo.dataPoints[i_currentlyCheckedDatapoint].validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid)
                {
                    //some aggressive inlining:
                    float absDistanceToCursor = line_theseSpecsBelongTo.dataPoints[i_currentlyCheckedDatapoint].xValue - xValueOfCursor_inChartspace;
                    if (absDistanceToCursor < 0.0f)
                    {
                        absDistanceToCursor = -absDistanceToCursor;
                    }

                    if (absDistanceToCursor < absDistance_ofCurrentlyNearest)
                    {
                        absDistance_ofCurrentlyNearest = absDistanceToCursor;
                        i_ofCurrentlyNearest = i_currentlyCheckedDatapoint;
                    }
                }
            }
            return i_ofCurrentlyNearest;
        }

        public void DrawCursorNearestPoint(float sizeOfDatapointVisualization, bool hiddenByNearerObjects)
        {
            if (currentHideLineState == false)
            {
                if ((currentHideCursorXState_duringComponentInspectionPhase == false) || (currentHideCursorYState_duringComponentInspectionPhase == false))
                {
                    if (hasValidDatapointThatIsXNearestToCursor)
                    {
                        ChartDrawing parentChart = line_theseSpecsBelongTo.Chart_thisLineIsPartOf;
                        if (parentChart.IsInsideDrawnChartArea(posInChartspace_ofDatapointThatIsNearestToCursor) || parentChart.drawValuesOutsideOfChartArea)
                        {
                            Vector3 position_worldSpace = parentChart.ChartSpace_to_WorldSpace(posInChartspace_ofDatapointThatIsNearestToCursor);
                            float sizeOfMarkingCross = parentChart.Height_inWorldSpace * (0.01f + 0.6f * sizeOfDatapointVisualization);
                            Color datapointColor = line_theseSpecsBelongTo.Color;
                            DrawBasics.Point(position_worldSpace, datapointColor, sizeOfMarkingCross, parentChart.InternalRotation, 0.0f, null, datapointColor, false, false, true, 0.0f, hiddenByNearerObjects);

                            float circleRadius = 0.14f * sizeOfMarkingCross;
                            DrawShapes.Decagon(position_worldSpace, circleRadius, datapointColor, parentChart.InternalRotation, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, 0.0f, hiddenByNearerObjects);

                            float textSize = 0.3f * sizeOfMarkingCross;
                            float dimensionSpecifyingTextSize = textSize * 0.5f;
                            Color color_ofDimensionSpecifier = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(datapointColor, 0.4f);

                            if (currentHideCursorYState_duringComponentInspectionPhase == false)
                            {
                                UtilitiesDXXL_Text.WriteFramed(" y=", position_worldSpace, color_ofDimensionSpecifier, dimensionSpecifyingTextSize, parentChart.InternalRotation, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, parentChart.autoFlipAllText_toFitObsererCamera, 0.0f, hiddenByNearerObjects);
                                UtilitiesDXXL_Text.WriteFramed("  " + posInChartspace_ofDatapointThatIsNearestToCursor.y, position_worldSpace, datapointColor, textSize, parentChart.InternalRotation, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, parentChart.autoFlipAllText_toFitObsererCamera, 0.0f, hiddenByNearerObjects);
                            }

                            if (currentHideCursorXState_duringComponentInspectionPhase == false)
                            {
                                Vector3 textDir_forXText_normalized = parentChart.yAxis.AxisVector_normalized_inWorldSpace;
                                Vector3 textUp_forXText_normalized = -parentChart.xAxis.AxisVector_normalized_inWorldSpace;
                                UtilitiesDXXL_Text.Write(" x=", position_worldSpace, color_ofDimensionSpecifier, dimensionSpecifyingTextSize, textDir_forXText_normalized, textUp_forXText_normalized, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, parentChart.autoFlipAllText_toFitObsererCamera, 0.0f, hiddenByNearerObjects, false, false, true);
                                UtilitiesDXXL_Text.Write("  " + posInChartspace_ofDatapointThatIsNearestToCursor.x, position_worldSpace, datapointColor, textSize, textDir_forXText_normalized, textUp_forXText_normalized, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, parentChart.autoFlipAllText_toFitObsererCamera, 0.0f, hiddenByNearerObjects, false, false, true);
                            }
                        }
                    }
                }
            }
        }
    }

}
