namespace DrawXXL
{
    using UnityEngine;

    public class DimensionOf_PointOfInterest
    {
        public enum LineExtent { axisToPoint, throughWholeChart_ifOtherDimensionsValueIsInsideChart, throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart }

        private static float sizeOfLabeltext_relToChartHeight = 0.02f;
        public static float SizeOfLabeltext_relToChartHeight  //this changes globally for all PointsOfInterest 
        {
            get { return sizeOfLabeltext_relToChartHeight; }
            set { sizeOfLabeltext_relToChartHeight = Mathf.Max(value, 0.002f); }
        }

        private static float sizeOfCoordinateText_relToChartHeight = 0.02f;
        public static float SizeOfCoordinateText_relToChartHeight  //this changes globally for all PointsOfInterest 
        {
            get { return sizeOfCoordinateText_relToChartHeight; }
            set { sizeOfCoordinateText_relToChartHeight = Mathf.Max(value, 0.002f); }
        }

        public float position;
        public DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid;
        public float lineWidth_relToChartHeight = 0.0f;
        public float linestylePatternScaleFactor = 1.0f;
        public string labelText; //is only drawn if lineStyle is not invisible
        public bool drawCoordinateAsText = true; //"lineStyle is invisible" also disables the drawing of the coordinate value
        public Color color;
        public LineExtent lineExtent = LineExtent.throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart;
        UtilitiesDXXL_Math.Dimension representedDimension;
        ChartDrawing chart_thisPointIsPartOf;
        public DimensionOf_PointOfInterest theOtherDimension;
        float currLineWidth_inWorldUnits;
        public bool placeTextTowardsOutsideOfChart = false; //By default the labelText at the horizontal or vertical lines is mounted at the x respectively y axis and extends towards the inside of the chart. Setting "placeTextTowardsOutsideOfChart" to "true" shifts the "labelText", so it extends towards the outside of the chart. It is useful if you have many labelTexts from different points of interest in the chart, that intersect each other and are therefore not readable anymore. In such cases you gain some more breathing space for displayed texts.

        public DimensionOf_PointOfInterest(float position, Color color, UtilitiesDXXL_Math.Dimension representedDimension, ChartDrawing chart_thisPointIsPartOf)
        {
            //use ChartDrawing.AddPointOfInterest() instead
            this.position = position;
            this.color = color;
            this.representedDimension = representedDimension;
            this.chart_thisPointIsPartOf = chart_thisPointIsPartOf;
        }

        public void Draw(float durationInSec, bool hiddenByNearerObjects)
        {
            //use ChartDrawing.Draw() instead
            if (representedDimension == UtilitiesDXXL_Math.Dimension.x)
            {
                bool lineHasBeenDrawn = DrawVertLineAtXPos(durationInSec, hiddenByNearerObjects);
                DrawCoordinateText_vertForXValues(lineHasBeenDrawn, durationInSec, hiddenByNearerObjects);
                DrawLabelText_vertForXValues(lineHasBeenDrawn, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                bool lineHasBeenDrawn = DrawHorizLineAtYPos(durationInSec, hiddenByNearerObjects);
                DrawCoordinateText_horizForYValues(lineHasBeenDrawn, durationInSec, hiddenByNearerObjects);
                DrawLabelText_horizForYValues(lineHasBeenDrawn, durationInSec, hiddenByNearerObjects);
            }
        }

        bool DrawVertLineAtXPos(float durationInSec, bool hiddenByNearerObjects)
        {
            //function returns if line was drawn
            if (lineStyle != DrawBasics.LineStyle.invisible)
            {
                if (UtilitiesDXXL_Math.FloatIsValid(position))
                {
                    Vector2 start_inChartspace = new Vector2(position, chart_thisPointIsPartOf.yAxis.ValueMarkingLowerEndOfTheAxis);
                    Vector2 end_inChartspace;
                    bool shouldBeDrawn_becauseItHasPartsInsideTheChartArea;
                    if (UtilitiesDXXL_Math.FloatIsValid(theOtherDimension.position))
                    {
                        switch (lineExtent)
                        {
                            case LineExtent.axisToPoint:
                                GetLineDrawSpecs_for_vertLineAtXPos_extentCase_axisToPoint(out shouldBeDrawn_becauseItHasPartsInsideTheChartArea, out end_inChartspace);
                                break;
                            case LineExtent.throughWholeChart_ifOtherDimensionsValueIsInsideChart:
                                GetLineDrawSpecs_for_vertLineAtXPos_extentCase_throughWholeChart_ifOtherDimensionsValueIsInsideChart(out shouldBeDrawn_becauseItHasPartsInsideTheChartArea, out end_inChartspace);
                                break;
                            case LineExtent.throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart:
                                GetLineDrawSpecs_for_vertLineAtXPos_extentCase_throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart(out shouldBeDrawn_becauseItHasPartsInsideTheChartArea, out end_inChartspace);
                                break;
                            default:
                                UtilitiesDXXL_Log.PrintErrorCode("10-" + lineExtent);
                                return false;
                        }
                    }
                    else
                    {
                        //other dimensions position is invalid:
                        GetLineDrawSpecs_for_vertLineAtXPos_extentCase_throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart(out shouldBeDrawn_becauseItHasPartsInsideTheChartArea, out end_inChartspace);
                    }
                    return TryDrawLine(shouldBeDrawn_becauseItHasPartsInsideTheChartArea, start_inChartspace, end_inChartspace, durationInSec, hiddenByNearerObjects);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        void GetLineDrawSpecs_for_vertLineAtXPos_extentCase_axisToPoint(out bool shouldBeDrawn_becauseItHasPartsInsideTheChartArea, out Vector2 lineEnd_inChartspace)
        {
            shouldBeDrawn_becauseItHasPartsInsideTheChartArea = false;
            lineEnd_inChartspace = new Vector2(position, theOtherDimension.position);
            if (chart_thisPointIsPartOf.xAxis.IsInsideDisplayedSpan(position))
            {
                if (theOtherDimension.position > chart_thisPointIsPartOf.yAxis.ValueMarkingLowerEndOfTheAxis)
                {
                    shouldBeDrawn_becauseItHasPartsInsideTheChartArea = true;
                    if (theOtherDimension.position > chart_thisPointIsPartOf.yAxis.ValueMarkingUpperEndOfTheAxis)
                    {
                        lineEnd_inChartspace = new Vector2(position, chart_thisPointIsPartOf.yAxis.ValueMarkingUpperEndOfTheAxis);
                    }
                }
            }
        }

        void GetLineDrawSpecs_for_vertLineAtXPos_extentCase_throughWholeChart_ifOtherDimensionsValueIsInsideChart(out bool shouldBeDrawn_becauseItHasPartsInsideTheChartArea, out Vector2 lineEnd_inChartspace)
        {
            shouldBeDrawn_becauseItHasPartsInsideTheChartArea = false;
            lineEnd_inChartspace = new Vector2(position, chart_thisPointIsPartOf.yAxis.ValueMarkingUpperEndOfTheAxis);
            if (chart_thisPointIsPartOf.xAxis.IsInsideDisplayedSpan(position))
            {
                if (chart_thisPointIsPartOf.yAxis.IsInsideDisplayedSpan(theOtherDimension.position))
                {
                    shouldBeDrawn_becauseItHasPartsInsideTheChartArea = true;
                }
            }
        }

        void GetLineDrawSpecs_for_vertLineAtXPos_extentCase_throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart(out bool shouldBeDrawn_becauseItHasPartsInsideTheChartArea, out Vector2 lineEnd_inChartspace)
        {
            shouldBeDrawn_becauseItHasPartsInsideTheChartArea = false;
            lineEnd_inChartspace = new Vector2(position, chart_thisPointIsPartOf.yAxis.ValueMarkingUpperEndOfTheAxis);
            if (chart_thisPointIsPartOf.xAxis.IsInsideDisplayedSpan(position))
            {
                shouldBeDrawn_becauseItHasPartsInsideTheChartArea = true;
            }
        }

        bool DrawHorizLineAtYPos(float durationInSec, bool hiddenByNearerObjects)
        {
            //function returns if line was drawn
            if (lineStyle != DrawBasics.LineStyle.invisible)
            {
                if (UtilitiesDXXL_Math.FloatIsValid(position))
                {
                    Vector2 start_inChartspace = new Vector2(chart_thisPointIsPartOf.xAxis.ValueMarkingLowerEndOfTheAxis, position);
                    Vector2 end_inChartspace;
                    bool shouldBeDrawn_becauseItHasPartsInsideTheChartArea;
                    if (UtilitiesDXXL_Math.FloatIsValid(theOtherDimension.position))
                    {
                        switch (lineExtent)
                        {
                            case LineExtent.axisToPoint:
                                GetLineDrawSpecs_for_horizLineAtYPos_extentCase_axisToPoint(out shouldBeDrawn_becauseItHasPartsInsideTheChartArea, out end_inChartspace);
                                break;
                            case LineExtent.throughWholeChart_ifOtherDimensionsValueIsInsideChart:
                                GetLineDrawSpecs_for_horizLineAtYPos_extentCase_throughWholeChart_ifOtherDimensionsValueIsInsideChart(out shouldBeDrawn_becauseItHasPartsInsideTheChartArea, out end_inChartspace);
                                break;
                            case LineExtent.throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart:
                                GetLineDrawSpecs_for_horizLineAtYPos_extentCase_throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart(out shouldBeDrawn_becauseItHasPartsInsideTheChartArea, out end_inChartspace);
                                break;
                            default:
                                UtilitiesDXXL_Log.PrintErrorCode("11-" + lineExtent);
                                return false;
                        }
                    }
                    else
                    {
                        //other dimensions position is invalid:
                        GetLineDrawSpecs_for_horizLineAtYPos_extentCase_throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart(out shouldBeDrawn_becauseItHasPartsInsideTheChartArea, out end_inChartspace);
                    }
                    return TryDrawLine(shouldBeDrawn_becauseItHasPartsInsideTheChartArea, start_inChartspace, end_inChartspace, durationInSec, hiddenByNearerObjects);
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        void GetLineDrawSpecs_for_horizLineAtYPos_extentCase_axisToPoint(out bool shouldBeDrawn_becauseItHasPartsInsideTheChartArea, out Vector2 lineEnd_inChartspace)
        {
            shouldBeDrawn_becauseItHasPartsInsideTheChartArea = false;
            lineEnd_inChartspace = new Vector2(theOtherDimension.position, position);
            if (chart_thisPointIsPartOf.yAxis.IsInsideDisplayedSpan(position))
            {
                if (theOtherDimension.position > chart_thisPointIsPartOf.xAxis.ValueMarkingLowerEndOfTheAxis)
                {
                    shouldBeDrawn_becauseItHasPartsInsideTheChartArea = true;
                    if (theOtherDimension.position > chart_thisPointIsPartOf.xAxis.ValueMarkingUpperEndOfTheAxis)
                    {
                        lineEnd_inChartspace = new Vector2(chart_thisPointIsPartOf.xAxis.ValueMarkingUpperEndOfTheAxis, position);
                    }
                }
            }
        }

        void GetLineDrawSpecs_for_horizLineAtYPos_extentCase_throughWholeChart_ifOtherDimensionsValueIsInsideChart(out bool shouldBeDrawn_becauseItHasPartsInsideTheChartArea, out Vector2 lineEnd_inChartspace)
        {
            shouldBeDrawn_becauseItHasPartsInsideTheChartArea = false;
            lineEnd_inChartspace = new Vector2(chart_thisPointIsPartOf.xAxis.ValueMarkingUpperEndOfTheAxis, position);
            if (chart_thisPointIsPartOf.yAxis.IsInsideDisplayedSpan(position))
            {
                if (chart_thisPointIsPartOf.xAxis.IsInsideDisplayedSpan(theOtherDimension.position))
                {
                    shouldBeDrawn_becauseItHasPartsInsideTheChartArea = true;
                }
            }
        }

        void GetLineDrawSpecs_for_horizLineAtYPos_extentCase_throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart(out bool shouldBeDrawn_becauseItHasPartsInsideTheChartArea, out Vector2 lineEnd_inChartspace)
        {
            shouldBeDrawn_becauseItHasPartsInsideTheChartArea = false;
            lineEnd_inChartspace = new Vector2(chart_thisPointIsPartOf.xAxis.ValueMarkingUpperEndOfTheAxis, position);
            if (chart_thisPointIsPartOf.yAxis.IsInsideDisplayedSpan(position))
            {
                shouldBeDrawn_becauseItHasPartsInsideTheChartArea = true;
            }
        }

        bool TryDrawLine(bool shouldBeDrawn_becauseItHasPartsInsideTheChartArea, Vector2 start_inChartspace, Vector2 end_inChartspace, float durationInSec, bool hiddenByNearerObjects)
        {
            //function returns if line was drawn
            if (shouldBeDrawn_becauseItHasPartsInsideTheChartArea || chart_thisPointIsPartOf.drawValuesOutsideOfChartArea)
            {
                Vector3 start_inWorldspace = chart_thisPointIsPartOf.ChartSpace_to_WorldSpace(start_inChartspace);
                Vector3 end_inWorldspace = chart_thisPointIsPartOf.ChartSpace_to_WorldSpace(end_inChartspace);
                Vector3 customAmplitudeAndTextDir = chart_thisPointIsPartOf.xAxis.AxisVector_normalized_inWorldSpace + chart_thisPointIsPartOf.yAxis.AxisVector_normalized_inWorldSpace;

                float stylePatternScaleFactor_inWorldspace;
                if (UtilitiesDXXL_Math.ApproximatelyZero(linestylePatternScaleFactor))
                {
                    stylePatternScaleFactor_inWorldspace = 0.0f;
                }
                else
                {
                    stylePatternScaleFactor_inWorldspace = linestylePatternScaleFactor * chart_thisPointIsPartOf.Height_inWorldSpace;
                }
                float widened_stylePatternScaleFactor_inWorldspace = GetStylePatternScaleFactor_widenedForDottedStyles_inWorldspace(stylePatternScaleFactor_inWorldspace);

                if (UtilitiesDXXL_Math.ApproximatelyZero(lineWidth_relToChartHeight))
                {
                    currLineWidth_inWorldUnits = 0.0f;
                }
                else
                {
                    currLineWidth_inWorldUnits = lineWidth_relToChartHeight * chart_thisPointIsPartOf.Height_inWorldSpace;
                }
                lineStyle = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(lineStyle);
                Line_fadeableAnimSpeed.InternalDraw(start_inWorldspace, end_inWorldspace, color, currLineWidth_inWorldUnits, null, lineStyle, widened_stylePatternScaleFactor_inWorldspace, 0.0f, null, customAmplitudeAndTextDir, true, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, true, true);
                return true;
            }
            else
            {
                return false;
            }
        }

        void DrawCoordinateText_vertForXValues(bool vertLineHasBeenDrawnfloat, float durationInSec, bool hiddenByNearerObjects)
        {
            if (vertLineHasBeenDrawnfloat)
            {
                if (drawCoordinateAsText)
                {
                    Vector2 position_chartspace = new Vector2(position, chart_thisPointIsPartOf.yAxis.ValueMarkingLowerEndOfTheAxis);
                    Vector3 posOffset_forMoreLeveledDistanceToLine = (-chart_thisPointIsPartOf.xAxis.AxisVector_normalized_inWorldSpace) * (chart_thisPointIsPartOf.yAxis.Length_inWorldSpace * sizeOfCoordinateText_relToChartHeight * relAdditionalShiftOfText);
                    float absHalfLineWidth_inWorldUnits = Mathf.Abs(0.5f * currLineWidth_inWorldUnits);
                    Vector3 posOffset_forDifferentLineWidths;
                    DrawText.TextAnchorDXXL textAnchor;
                    if (labelText == null || labelText == "")
                    {
                        textAnchor = placeTextTowardsOutsideOfChart ? DrawText.TextAnchorDXXL.LowerRight : DrawText.TextAnchorDXXL.LowerLeft;
                        posOffset_forDifferentLineWidths = (-chart_thisPointIsPartOf.xAxis.AxisVector_normalized_inWorldSpace) * absHalfLineWidth_inWorldUnits;
                    }
                    else
                    {
                        textAnchor = placeTextTowardsOutsideOfChart ? DrawText.TextAnchorDXXL.UpperRight : DrawText.TextAnchorDXXL.UpperLeft;
                        posOffset_forDifferentLineWidths = chart_thisPointIsPartOf.xAxis.AxisVector_normalized_inWorldSpace * absHalfLineWidth_inWorldUnits;
                    }
                    Vector3 position_worldspace = chart_thisPointIsPartOf.ChartSpace_to_WorldSpace(position_chartspace) + posOffset_forMoreLeveledDistanceToLine + posOffset_forDifferentLineWidths;
                    float textSize = chart_thisPointIsPartOf.Height_inWorldSpace * sizeOfCoordinateText_relToChartHeight;
                    Vector3 textDir_normalized = chart_thisPointIsPartOf.yAxis.AxisVector_normalized_inWorldSpace;
                    Vector3 textUp_normalized = -chart_thisPointIsPartOf.xAxis.AxisVector_normalized_inWorldSpace;
                    UtilitiesDXXL_Text.Write("" + position, position_worldspace, color, textSize, textDir_normalized, textUp_normalized, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, chart_thisPointIsPartOf.autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects, false, false, true);
                }
            }
        }

        float relAdditionalShiftOfText = 0.3f;
        void DrawCoordinateText_horizForYValues(bool horizLineHasBeenDrawn, float durationInSec, bool hiddenByNearerObjects)
        {
            if (horizLineHasBeenDrawn)
            {
                if (drawCoordinateAsText)
                {
                    Vector2 position_chartspace = new Vector2(chart_thisPointIsPartOf.xAxis.ValueMarkingLowerEndOfTheAxis, position);
                    Vector3 posOffset_forMoreLeveledDistanceToLine = chart_thisPointIsPartOf.yAxis.AxisVector_inWorldSpace * sizeOfCoordinateText_relToChartHeight * relAdditionalShiftOfText;
                    float absHalfLineWidth_inWorldUnits = Mathf.Abs(0.5f * currLineWidth_inWorldUnits);
                    Vector3 posOffset_forDifferentLineWidths;
                    DrawText.TextAnchorDXXL textAnchor;
                    if (labelText == null || labelText == "")
                    {
                        textAnchor = placeTextTowardsOutsideOfChart ? DrawText.TextAnchorDXXL.LowerRight : DrawText.TextAnchorDXXL.LowerLeft;
                        posOffset_forDifferentLineWidths = chart_thisPointIsPartOf.yAxis.AxisVector_normalized_inWorldSpace * absHalfLineWidth_inWorldUnits;
                    }
                    else
                    {
                        textAnchor = placeTextTowardsOutsideOfChart ? DrawText.TextAnchorDXXL.UpperRight : DrawText.TextAnchorDXXL.UpperLeft;
                        posOffset_forDifferentLineWidths = (-chart_thisPointIsPartOf.yAxis.AxisVector_normalized_inWorldSpace) * absHalfLineWidth_inWorldUnits;
                    }
                    Vector3 position_worldspace = chart_thisPointIsPartOf.ChartSpace_to_WorldSpace(position_chartspace) + posOffset_forMoreLeveledDistanceToLine + posOffset_forDifferentLineWidths;
                    float textSize = chart_thisPointIsPartOf.Height_inWorldSpace * sizeOfCoordinateText_relToChartHeight;
                    Vector3 textDir_normalized = chart_thisPointIsPartOf.xAxis.AxisVector_normalized_inWorldSpace;
                    Vector3 textUp_normalized = chart_thisPointIsPartOf.yAxis.AxisVector_normalized_inWorldSpace;
                    UtilitiesDXXL_Text.Write("" + position, position_worldspace, color, textSize, textDir_normalized, textUp_normalized, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, chart_thisPointIsPartOf.autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects, false, false, true);
                }
            }
        }

        void DrawLabelText_vertForXValues(bool vertLineHasBeenDrawn, float durationInSec, bool hiddenByNearerObjects)
        {
            if (vertLineHasBeenDrawn)
            {
                if (labelText != null && labelText != "")
                {
                    Vector2 position_chartspace = new Vector2(position, chart_thisPointIsPartOf.yAxis.ValueMarkingLowerEndOfTheAxis);
                    Vector3 posOffset_forMoreLeveledDistanceToLine = (-chart_thisPointIsPartOf.xAxis.AxisVector_normalized_inWorldSpace) * (chart_thisPointIsPartOf.yAxis.Length_inWorldSpace * sizeOfCoordinateText_relToChartHeight * relAdditionalShiftOfText);
                    float absHalfLineWidth_inWorldUnits = Mathf.Abs(0.5f * currLineWidth_inWorldUnits);
                    Vector3 posOffset_forDifferentLineWidths = (-chart_thisPointIsPartOf.xAxis.AxisVector_normalized_inWorldSpace) * absHalfLineWidth_inWorldUnits;
                    Vector3 position_worldspace = chart_thisPointIsPartOf.ChartSpace_to_WorldSpace(position_chartspace) + posOffset_forMoreLeveledDistanceToLine + posOffset_forDifferentLineWidths;
                    float textSize = chart_thisPointIsPartOf.Height_inWorldSpace * sizeOfLabeltext_relToChartHeight;
                    Vector3 textDir_normalized = chart_thisPointIsPartOf.yAxis.AxisVector_normalized_inWorldSpace;
                    Vector3 textUp_normalized = -chart_thisPointIsPartOf.xAxis.AxisVector_normalized_inWorldSpace;
                    DrawText.TextAnchorDXXL textAnchor = placeTextTowardsOutsideOfChart ? DrawText.TextAnchorDXXL.LowerRight : DrawText.TextAnchorDXXL.LowerLeft;
                    UtilitiesDXXL_Text.Write(labelText, position_worldspace, color, textSize, textDir_normalized, textUp_normalized, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, chart_thisPointIsPartOf.autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects, false, false, true);
                }
            }
        }

        void DrawLabelText_horizForYValues(bool horizLineHasBeenDrawn, float durationInSec, bool hiddenByNearerObjects)
        {
            if (horizLineHasBeenDrawn)
            {
                if (labelText != null && labelText != "")
                {
                    Vector2 position_chartspace = new Vector2(chart_thisPointIsPartOf.xAxis.ValueMarkingLowerEndOfTheAxis, position);
                    Vector3 posOffset_forMoreLeveledDistanceToLine = chart_thisPointIsPartOf.yAxis.AxisVector_inWorldSpace * sizeOfCoordinateText_relToChartHeight * relAdditionalShiftOfText;
                    float absHalfLineWidth_inWorldUnits = Mathf.Abs(0.5f * currLineWidth_inWorldUnits);
                    Vector3 posOffset_forDifferentLineWidths = chart_thisPointIsPartOf.yAxis.AxisVector_normalized_inWorldSpace * absHalfLineWidth_inWorldUnits;
                    Vector3 position_worldspace = chart_thisPointIsPartOf.ChartSpace_to_WorldSpace(position_chartspace) + posOffset_forMoreLeveledDistanceToLine + posOffset_forDifferentLineWidths;
                    float textSize = chart_thisPointIsPartOf.Height_inWorldSpace * sizeOfLabeltext_relToChartHeight;
                    Vector3 textDir_normalized = chart_thisPointIsPartOf.xAxis.AxisVector_normalized_inWorldSpace;
                    Vector3 textUp_normalized = chart_thisPointIsPartOf.yAxis.AxisVector_normalized_inWorldSpace;
                    DrawText.TextAnchorDXXL textAnchor = placeTextTowardsOutsideOfChart ? DrawText.TextAnchorDXXL.LowerRight : DrawText.TextAnchorDXXL.LowerLeft;
                    UtilitiesDXXL_Text.Write(labelText, position_worldspace, color, textSize, textDir_normalized, textUp_normalized, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, chart_thisPointIsPartOf.autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects, false, false, true);
                }
            }
        }

        float GetStylePatternScaleFactor_widenedForDottedStyles_inWorldspace(float unwidened_stylePatternScaleFactor_inWorldspace)
        {
            if (lineStyle == DrawBasics.LineStyle.dotted)
            {
                return unwidened_stylePatternScaleFactor_inWorldspace * 5.0f;
            }
            if (lineStyle == DrawBasics.LineStyle.dottedDense)
            {
                return unwidened_stylePatternScaleFactor_inWorldspace * 5.0f;
            }
            if (lineStyle == DrawBasics.LineStyle.dottedWide)
            {
                return unwidened_stylePatternScaleFactor_inWorldspace * 5.0f;
            }
            if (lineStyle == DrawBasics.LineStyle.dotDash)
            {
                return unwidened_stylePatternScaleFactor_inWorldspace * 5.0f;
            }
            if (lineStyle == DrawBasics.LineStyle.dotDashLong)
            {
                return unwidened_stylePatternScaleFactor_inWorldspace * 5.0f;
            }
            if (lineStyle == DrawBasics.LineStyle.twoDash)
            {
                return unwidened_stylePatternScaleFactor_inWorldspace * 5.0f;
            }
            if (lineStyle == DrawBasics.LineStyle.dashed)
            {
                return unwidened_stylePatternScaleFactor_inWorldspace * 5.0f;
            }
            if (lineStyle == DrawBasics.LineStyle.dashedLong)
            {
                return unwidened_stylePatternScaleFactor_inWorldspace * 5.0f;
            }
            return unwidened_stylePatternScaleFactor_inWorldspace;
        }

    }

}
