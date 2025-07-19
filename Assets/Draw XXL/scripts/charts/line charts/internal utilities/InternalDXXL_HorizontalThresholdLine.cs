namespace DrawXXL
{
    using UnityEngine;

    public class InternalDXXL_HorizontalThresholdLine
    {
        float yPosition;
        bool lineItselfCountsToLowerArea;
        ChartLine line_thisThresholdIsPartOf;

        public InternalDXXL_HorizontalThresholdLine(float yPosition, ChartLine line_thisThresholdIsPartOf, bool lineItselfCountsToLowerArea)
        {
            this.yPosition = yPosition;
            this.line_thisThresholdIsPartOf = line_thisThresholdIsPartOf;
            this.lineItselfCountsToLowerArea = lineItselfCountsToLowerArea;
        }

        public PointOfInterest CheckIntersection(InternalDXXL_DataPointOfChartLine previousDataPoint, InternalDXXL_DataPointOfChartLine currDataPoint)
        {
            if (previousDataPoint.validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid && currDataPoint.validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid)
            {
                if (lineItselfCountsToLowerArea)
                {
                    //lineItselfCountsTo-LOWER-area:
                    if (previousDataPoint.yValue <= yPosition)
                    {
                        if (currDataPoint.yValue > yPosition)
                        {
                            return CreateIntersectionPoint(previousDataPoint, currDataPoint);
                        }
                    }
                    else
                    {
                        if (currDataPoint.yValue <= yPosition)
                        {
                            return CreateIntersectionPoint(previousDataPoint, currDataPoint);
                        }
                    }
                }
                else
                {
                    //lineItselfCountsTo-HIGHER-area:
                    if (previousDataPoint.yValue < yPosition)
                    {
                        if (currDataPoint.yValue >= yPosition)
                        {
                            return CreateIntersectionPoint(previousDataPoint, currDataPoint);
                        }
                    }
                    else
                    {
                        if (currDataPoint.yValue < yPosition)
                        {
                            return CreateIntersectionPoint(previousDataPoint, currDataPoint);
                        }
                    }
                }
            }
            return null;
        }

        PointOfInterest CreateIntersectionPoint(InternalDXXL_DataPointOfChartLine previousDataPoint, InternalDXXL_DataPointOfChartLine currDataPoint)
        {
            previousDataPoint.hasLittleEmphasizingCircleAroundPoint = true;
            currDataPoint.hasLittleEmphasizingCircleAroundPoint = true;

            float x_ofIntersectionPos;
            if (line_thisThresholdIsPartOf.lineConnectionsType == ChartLine.LineConnectionsType.horizPlateauTillNextPoint)
            {
                x_ofIntersectionPos = currDataPoint.xValue;
            }
            else
            {
                InternalDXXL_Line2D line2D_betweenDataPoints = new InternalDXXL_Line2D();
                Vector2 previousDataPoint_asV2 = new Vector2(previousDataPoint.xValue, previousDataPoint.yValue);
                Vector2 currDataPoint_asV2 = new Vector2(currDataPoint.xValue, currDataPoint.yValue);
                line2D_betweenDataPoints.Recalc_line_throughTwoPoints_returnSteepForVertLines(previousDataPoint_asV2, currDataPoint_asV2);
                x_ofIntersectionPos = line2D_betweenDataPoints.GetXatY(yPosition);
            }

            Color colorOfGeneratedIntersectionPoints = UtilitiesDXXL_Colors.GetSimilarColorWithSlightlyOtherBrightnessValue(line_thisThresholdIsPartOf.Color);
            colorOfGeneratedIntersectionPoints.a = 1.0f;

            string notificationText;
            if (previousDataPoint.yValue < currDataPoint.yValue)
            {
                Color risingFlankColor = Color.Lerp(UtilitiesDXXL_Colors.green_boolTrue, colorOfGeneratedIntersectionPoints, 0.18f);
                notificationText = "Line '" + line_thisThresholdIsPartOf.Name + "':<br><color=#" + ColorUtility.ToHtmlStringRGBA(risingFlankColor) + ">" + DrawText.MarkupIcon(DrawBasics.IconType.arrowUp) + "</color>Rising flank intersects threshold at<br>x = " + x_ofIntersectionPos + " / y = " + yPosition + "<br>Data point before intersection:<br>x = " + previousDataPoint.xValue + " / y = " + previousDataPoint.yValue + "<br>Data point after intersection:<br>x = " + currDataPoint.xValue + " / y = " + currDataPoint.yValue;
            }
            else
            {
                Color fallingFlankColor = Color.Lerp(UtilitiesDXXL_Colors.red_boolFalse, colorOfGeneratedIntersectionPoints, 0.18f);
                notificationText = "Line '" + line_thisThresholdIsPartOf.Name + "':<br><color=#" + ColorUtility.ToHtmlStringRGBA(fallingFlankColor) + ">" + DrawText.MarkupIcon(DrawBasics.IconType.arrowDown) + "</color>Falling flank intersects threshold at<br>x = " + x_ofIntersectionPos + " / y = " + yPosition + "<br>Data point before intersection:<br>x = " + previousDataPoint.xValue + " / y = " + previousDataPoint.yValue + "<br>Data point after intersection:<br>x = " + currDataPoint.xValue + " / y = " + currDataPoint.yValue;
            }

            PointOfInterest pointOfInterest_thatIndicatesTheThresholdCrossing = new PointOfInterest(x_ofIntersectionPos, yPosition, colorOfGeneratedIntersectionPoints, line_thisThresholdIsPartOf.Chart_thisLineIsPartOf, line_thisThresholdIsPartOf, notificationText);
            pointOfInterest_thatIndicatesTheThresholdCrossing.drawTextBoxIfPointIsOutsideOfChartArea = true;
            pointOfInterest_thatIndicatesTheThresholdCrossing.isDeletedOnClear = true;
            pointOfInterest_thatIndicatesTheThresholdCrossing.forceColorOfParent = false;
            pointOfInterest_thatIndicatesTheThresholdCrossing.xValue.lineStyle = DrawBasics.LineStyle.invisible;
            pointOfInterest_thatIndicatesTheThresholdCrossing.yValue.lineStyle = DrawBasics.LineStyle.invisible;
            return pointOfInterest_thatIndicatesTheThresholdCrossing;
        }

    }

}
