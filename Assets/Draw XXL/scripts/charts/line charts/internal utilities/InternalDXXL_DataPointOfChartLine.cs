namespace DrawXXL
{
    using UnityEngine;

    public class InternalDXXL_DataPointOfChartLine
    {
        public enum ValidState { isValid, isNaN_atX, isNaN_atY, isPositiveInfinity_atX, isPositiveInfinity_atY, isNegativeInfinity_atX, isNegativeInfinity_atY, isPlaceholderDuringPhasesWhereThisLineOfListDidntExistsDueToListChangedItsCount_existsOnlyToStayInXSyncWithOtherLinesOfList }

        ///[Set during point creation]:
        public float xValue; //-> is only expected to be "invalid" when using an "AddXYValue()"-function (which doesn't use the automatic x-value-source). So it is not expected for complexer data types (like Vector/Quaternion/Color, which consist of more than 1 line), and also it is not expected for anything that comes from "AddValues_eachIndexIsALine" where many lines are grouped.
        public float yValue;
        public ValidState validState; //if both values (x and y) are "NaN" (or Infintiy) then this is set to the "_atX" variants.
        public int i_ofThisPointInsideContainingLine;
        public ChartLine line_thisPointIsPartOf;
        public bool hasLittleEmphasizingCircleAroundPoint = false;
        public bool forceConnectionLineToLowAlpha;

        ///[Set during drawing]:
        public bool isInsideChartRect;
        public bool isInsideChartsXSpan;
        public bool isDrawn;
        public int i_ofPrecedingPointFromWhichLineCanBeDrawn;
        public Vector3 positionInWorldSpace;
        public Vector3 positionInWorldSpace_atYHeightOfValidPrecedingPoint;
        public Vector3 positionInWorldSpace_atYHeightOfLowerEndOf_rgbColorUnderlay;
        public Vector3 positionInWorldSpace_atYHeightOfUpperEndOf_rgbColorUnderlay;

        //Others:
        static float alphaFactor_indicatingVertConnectionLines = 0.2f;
        static float alphaFactor_indicatingNonExistingValues = 0.25f;

        public void DetermineIfAndHowPointIsDrawn(float valueMarkingLowerEndOf_XAxis, float valueMarkingUpperEndOf_XAxis, float valueMarkingLowerEndOf_YAxis, float valueMarkingUpperEndOf_YAxis, int i_ofTheMostCurrentDrawnPoint, bool thereHaveBeenValidPointsOutsideTheDrawnChartArea_sinceMostCurrentDrawnPoint)
        {
            if (validState == ValidState.isValid)
            {
                isInsideChartRect = true;
                if (xValue < valueMarkingLowerEndOf_XAxis) { isInsideChartRect = false; }
                if (xValue > valueMarkingUpperEndOf_XAxis) { isInsideChartRect = false; }
                if (yValue < valueMarkingLowerEndOf_YAxis) { isInsideChartRect = false; }
                if (yValue > valueMarkingUpperEndOf_YAxis) { isInsideChartRect = false; }
                bool isInDrawnArea = isInsideChartRect || line_thisPointIsPartOf.Chart_thisLineIsPartOf.drawValuesOutsideOfChartArea;
                isDrawn = isInDrawnArea;
                i_ofPrecedingPointFromWhichLineCanBeDrawn = i_ofTheMostCurrentDrawnPoint;
                if ((line_thisPointIsPartOf.Chart_thisLineIsPartOf.drawValuesOutsideOfChartArea == false) && thereHaveBeenValidPointsOutsideTheDrawnChartArea_sinceMostCurrentDrawnPoint)
                {
                    i_ofPrecedingPointFromWhichLineCanBeDrawn = -1;
                }
                positionInWorldSpace = line_thisPointIsPartOf.Chart_thisLineIsPartOf.ChartSpace_to_WorldSpace(new Vector2(xValue, yValue));
            }
            else
            {
                isInsideChartRect = false;
                isDrawn = false;
            }
        }

        public void DetermineIfAndHowPointIsDrawn_forRGBColorUnderlay(float valueMarkingLowerEndOf_XAxis, float valueMarkingUpperEndOf_XAxis, float valueMarkingLowerEndOf_YAxis, float valueMarkingUpperEndOf_YAxis)
        {
            //-> is only called for valid datapoints
            isInsideChartsXSpan = true;
            if (xValue < valueMarkingLowerEndOf_XAxis) { isInsideChartsXSpan = false; }
            if (xValue > valueMarkingUpperEndOf_XAxis) { isInsideChartsXSpan = false; }
            float yGap_toXAxis = 0.01f * (valueMarkingUpperEndOf_YAxis - valueMarkingLowerEndOf_YAxis);
            positionInWorldSpace_atYHeightOfLowerEndOf_rgbColorUnderlay = line_thisPointIsPartOf.Chart_thisLineIsPartOf.ChartSpace_to_WorldSpace(new Vector2(xValue, valueMarkingLowerEndOf_YAxis + yGap_toXAxis));
            positionInWorldSpace_atYHeightOfUpperEndOf_rgbColorUnderlay = line_thisPointIsPartOf.Chart_thisLineIsPartOf.ChartSpace_to_WorldSpace(new Vector2(xValue, valueMarkingUpperEndOf_YAxis));
        }

        public void DrawConnectionLineFromPrecedingPoint(Vector3 worldPos_ofPrecedingDrawnPoint, float yValue_ofPrecedingDrawnPoint_inChartSpace, bool lineFromPrecedingDrawnPoint_shouldBeLowerAlphaIndicatingAnInvalidPhase, float absLineWidth_worldSpace, Vector3 amplitudeDir_forNonZeroWidthLines, float durationInSec, bool hiddenByNearerObjects)
        {
            switch (line_thisPointIsPartOf.lineConnectionsType)
            {
                case ChartLine.LineConnectionsType.straightFromPointToPoint:
                    DrawLineFromPrecedingToCurrPoint(worldPos_ofPrecedingDrawnPoint, lineFromPrecedingDrawnPoint_shouldBeLowerAlphaIndicatingAnInvalidPhase, absLineWidth_worldSpace, amplitudeDir_forNonZeroWidthLines, durationInSec, hiddenByNearerObjects);
                    break;
                case ChartLine.LineConnectionsType.horizPlateauTillNextPoint:
                    DrawHorizLineFromPrecedingPointThenLowAlphaVertToCurrent(worldPos_ofPrecedingDrawnPoint, yValue_ofPrecedingDrawnPoint_inChartSpace, lineFromPrecedingDrawnPoint_shouldBeLowerAlphaIndicatingAnInvalidPhase, absLineWidth_worldSpace, amplitudeDir_forNonZeroWidthLines, durationInSec, hiddenByNearerObjects);
                    break;
                case ChartLine.LineConnectionsType.invisible:
                    break;
                default:
                    break;
            }
        }

        void DrawLineFromPrecedingToCurrPoint(Vector3 worldPos_ofPrecedingDrawnPoint, bool lineFromPrecedingDrawnPoint_shouldBeLowerAlphaIndicatingAnInvalidPhase, float absLineWidth_worldSpace, Vector3 amplitudeDir_forNonZeroWidthLines, float durationInSec, bool hiddenByNearerObjects)
        {
            if (lineFromPrecedingDrawnPoint_shouldBeLowerAlphaIndicatingAnInvalidPhase || forceConnectionLineToLowAlpha)
            {
                DrawLineFromPrecedingToCurrPoint_lowAlphaIndicatingNonExistingValues(worldPos_ofPrecedingDrawnPoint, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                DrawLineFromPrecedingToCurrPoint_fullAlpha(worldPos_ofPrecedingDrawnPoint, absLineWidth_worldSpace, amplitudeDir_forNonZeroWidthLines, durationInSec, hiddenByNearerObjects);
            }
        }

        void DrawLineFromPrecedingToCurrPoint_fullAlpha(Vector3 worldPos_ofPrecedingDrawnPoint, float absLineWidth_worldSpace, Vector3 amplitudeDir_forNonZeroWidthLines, float durationInSec, bool hiddenByNearerObjects)
        {
            Line_fadeableAnimSpeed.InternalDraw(worldPos_ofPrecedingDrawnPoint, positionInWorldSpace, line_thisPointIsPartOf.Color, absLineWidth_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, amplitudeDir_forNonZeroWidthLines, true, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
        }

        void DrawLineFromPrecedingToCurrPoint_lowAlphaIndicatingNonExistingValues(Vector3 worldPos_ofPrecedingDrawnPoint, float durationInSec, bool hiddenByNearerObjects)
        {
            Color color_loweredAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(line_thisPointIsPartOf.Color, alphaFactor_indicatingNonExistingValues);
            Line_fadeableAnimSpeed.InternalDraw(worldPos_ofPrecedingDrawnPoint, positionInWorldSpace, color_loweredAlpha, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
        }

        void DrawHorizLineFromPrecedingPointThenLowAlphaVertToCurrent(Vector3 worldPos_ofPrecedingDrawnPoint, float yValue_ofPrecedingDrawnPoint_inChartSpace, bool lineFromPrecedingDrawnPoint_shouldBeLowerAlphaIndicatingAnInvalidPhase, float absLineWidth_worldSpace, Vector3 amplitudeDir_forNonZeroWidthLines, float durationInSec, bool hiddenByNearerObjects)
        {
            if (lineFromPrecedingDrawnPoint_shouldBeLowerAlphaIndicatingAnInvalidPhase || forceConnectionLineToLowAlpha)
            {
                DrawHorizLineFromPrecedingPointThenLowAlphaVertToCurrent_lowAlphaConfigIndicatingNonExistingValues(worldPos_ofPrecedingDrawnPoint, yValue_ofPrecedingDrawnPoint_inChartSpace, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                DrawHorizLineFromPrecedingPointThenLowAlphaVertToCurrent_defaultAlphaConfig(worldPos_ofPrecedingDrawnPoint, yValue_ofPrecedingDrawnPoint_inChartSpace, absLineWidth_worldSpace, amplitudeDir_forNonZeroWidthLines, durationInSec, hiddenByNearerObjects);
            }
        }

        void DrawHorizLineFromPrecedingPointThenLowAlphaVertToCurrent_defaultAlphaConfig(Vector3 worldPos_ofPrecedingDrawnPoint, float yValue_ofPrecedingDrawnPoint_inChartSpace, float absLineWidth_worldSpace, Vector3 amplitudeDir_forNonZeroWidthLines, float durationInSec, bool hiddenByNearerObjects)
        {
            //horiz Line:
            CalcWorldPosAtYHeightOfValidPrecedingPoint(yValue_ofPrecedingDrawnPoint_inChartSpace);
            Line_fadeableAnimSpeed.InternalDraw(worldPos_ofPrecedingDrawnPoint, positionInWorldSpace_atYHeightOfValidPrecedingPoint, line_thisPointIsPartOf.Color, absLineWidth_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, amplitudeDir_forNonZeroWidthLines, true, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            //vert Line:
            Color color_ofVertConnectionLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(line_thisPointIsPartOf.Color, alphaFactor_indicatingVertConnectionLines);
            Line_fadeableAnimSpeed.InternalDraw(positionInWorldSpace_atYHeightOfValidPrecedingPoint, positionInWorldSpace, color_ofVertConnectionLine, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
        }

        void DrawHorizLineFromPrecedingPointThenLowAlphaVertToCurrent_lowAlphaConfigIndicatingNonExistingValues(Vector3 worldPos_ofPrecedingDrawnPoint, float yValue_ofPrecedingDrawnPoint_inChartSpace, float durationInSec, bool hiddenByNearerObjects)
        {
            //horiz Line:
            CalcWorldPosAtYHeightOfValidPrecedingPoint(yValue_ofPrecedingDrawnPoint_inChartSpace);
            Color colorHoriz_loweredAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(line_thisPointIsPartOf.Color, alphaFactor_indicatingNonExistingValues);
            Line_fadeableAnimSpeed.InternalDraw(worldPos_ofPrecedingDrawnPoint, positionInWorldSpace_atYHeightOfValidPrecedingPoint, colorHoriz_loweredAlpha, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            //vert Line:
            Color color_ofVertConnectionLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(line_thisPointIsPartOf.Color, alphaFactor_indicatingVertConnectionLines);
            Color colorVert_loweredAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofVertConnectionLine, 0.5f);
            Line_fadeableAnimSpeed.InternalDraw(positionInWorldSpace_atYHeightOfValidPrecedingPoint, positionInWorldSpace, colorVert_loweredAlpha, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
        }

        void CalcWorldPosAtYHeightOfValidPrecedingPoint(float yValue_ofPrecedingDrawnPoint_inChartSpace)
        {
            positionInWorldSpace_atYHeightOfValidPrecedingPoint = line_thisPointIsPartOf.Chart_thisLineIsPartOf.ChartSpace_to_WorldSpace(new Vector2(xValue, yValue_ofPrecedingDrawnPoint_inChartSpace));
        }

        public void DrawPointVisualization(float absSizeOfPointVisualization_inWorldSpace, float absLineWidth_forLineItself_worldSpace, float absLineWidth_forPointVisualisators_worldSpace, Vector3 amplitudeDir_forNonZeroWidthLines, float durationInSec, bool hiddenByNearerObjects)
        {
            TryDrawLittleEmphasizingCircleAroundPoint(absLineWidth_forLineItself_worldSpace, durationInSec, hiddenByNearerObjects);
            if (validState == ValidState.isValid)
            {
                DrawVerticalFillLineFromXAxisToPoint(durationInSec, hiddenByNearerObjects);
                float halfAbsSizeOfPointVisualization_inWorldSpace;
                bool filledWithSpokes;
                int numberOfCornersOfStarShape;
                switch (line_thisPointIsPartOf.dataPointVisualization)
                {
                    case ChartLine.DataPointVisualization.invisible:
                        break;
                    case ChartLine.DataPointVisualization.cross:
                        halfAbsSizeOfPointVisualization_inWorldSpace = 0.5f * absSizeOfPointVisualization_inWorldSpace;
                        //horiz crossline:
                        Vector3 leftCrossEnd_worldSpace = positionInWorldSpace - line_thisPointIsPartOf.Chart_thisLineIsPartOf.xAxis.AxisVector_normalized_inWorldSpace * halfAbsSizeOfPointVisualization_inWorldSpace;
                        Vector3 rightCrossEnd_worldSpace = positionInWorldSpace + line_thisPointIsPartOf.Chart_thisLineIsPartOf.xAxis.AxisVector_normalized_inWorldSpace * halfAbsSizeOfPointVisualization_inWorldSpace;
                        Line_fadeableAnimSpeed.InternalDraw(leftCrossEnd_worldSpace, rightCrossEnd_worldSpace, line_thisPointIsPartOf.Color, absLineWidth_forPointVisualisators_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, amplitudeDir_forNonZeroWidthLines, true, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

                        //vert crossline:
                        Vector3 lowerCrossEnd_worldSpace = positionInWorldSpace - line_thisPointIsPartOf.Chart_thisLineIsPartOf.yAxis.AxisVector_normalized_inWorldSpace * halfAbsSizeOfPointVisualization_inWorldSpace;
                        Vector3 upperCrossEnd_worldSpace = positionInWorldSpace + line_thisPointIsPartOf.Chart_thisLineIsPartOf.yAxis.AxisVector_normalized_inWorldSpace * halfAbsSizeOfPointVisualization_inWorldSpace;
                        Line_fadeableAnimSpeed.InternalDraw(lowerCrossEnd_worldSpace, upperCrossEnd_worldSpace, line_thisPointIsPartOf.Color, absLineWidth_forPointVisualisators_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, amplitudeDir_forNonZeroWidthLines, true, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                        break;
                    case ChartLine.DataPointVisualization.cross45deg:
                        float halfAbsLengthOfCrossLines_projectedOntoChartAxis_inWorldSpace = 0.5f * absSizeOfPointVisualization_inWorldSpace * UtilitiesDXXL_Math.inverseSqrtOf2_precalced;
                        //horiz crossline:
                        Vector3 lowLeftCrossEnd_worldSpace = positionInWorldSpace - line_thisPointIsPartOf.Chart_thisLineIsPartOf.xAxis.AxisVector_normalized_inWorldSpace * halfAbsLengthOfCrossLines_projectedOntoChartAxis_inWorldSpace - line_thisPointIsPartOf.Chart_thisLineIsPartOf.yAxis.AxisVector_normalized_inWorldSpace * halfAbsLengthOfCrossLines_projectedOntoChartAxis_inWorldSpace;
                        Vector3 topRightCrossEnd_worldSpace = positionInWorldSpace + line_thisPointIsPartOf.Chart_thisLineIsPartOf.xAxis.AxisVector_normalized_inWorldSpace * halfAbsLengthOfCrossLines_projectedOntoChartAxis_inWorldSpace + line_thisPointIsPartOf.Chart_thisLineIsPartOf.yAxis.AxisVector_normalized_inWorldSpace * halfAbsLengthOfCrossLines_projectedOntoChartAxis_inWorldSpace;
                        Line_fadeableAnimSpeed.InternalDraw(lowLeftCrossEnd_worldSpace, topRightCrossEnd_worldSpace, line_thisPointIsPartOf.Color, absLineWidth_forPointVisualisators_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, amplitudeDir_forNonZeroWidthLines, true, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

                        //vert crossline:
                        Vector3 topLeftCrossEnd_worldSpace = positionInWorldSpace - line_thisPointIsPartOf.Chart_thisLineIsPartOf.xAxis.AxisVector_normalized_inWorldSpace * halfAbsLengthOfCrossLines_projectedOntoChartAxis_inWorldSpace + line_thisPointIsPartOf.Chart_thisLineIsPartOf.yAxis.AxisVector_normalized_inWorldSpace * halfAbsLengthOfCrossLines_projectedOntoChartAxis_inWorldSpace;
                        Vector3 lowRightCrossEnd_worldSpace = positionInWorldSpace + line_thisPointIsPartOf.Chart_thisLineIsPartOf.xAxis.AxisVector_normalized_inWorldSpace * halfAbsLengthOfCrossLines_projectedOntoChartAxis_inWorldSpace - line_thisPointIsPartOf.Chart_thisLineIsPartOf.yAxis.AxisVector_normalized_inWorldSpace * halfAbsLengthOfCrossLines_projectedOntoChartAxis_inWorldSpace;
                        Line_fadeableAnimSpeed.InternalDraw(topLeftCrossEnd_worldSpace, lowRightCrossEnd_worldSpace, line_thisPointIsPartOf.Color, absLineWidth_forPointVisualisators_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, amplitudeDir_forNonZeroWidthLines, true, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                        break;
                    case ChartLine.DataPointVisualization.square:
                        filledWithSpokes = false;
                        DrawShapes.Square(positionInWorldSpace, absSizeOfPointVisualization_inWorldSpace, line_thisPointIsPartOf.Color, line_thisPointIsPartOf.Chart_thisLineIsPartOf.InternalRotation, absLineWidth_forPointVisualisators_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, filledWithSpokes, false, durationInSec, hiddenByNearerObjects);
                        break;
                    case ChartLine.DataPointVisualization.squareCrossed:
                        filledWithSpokes = true;
                        DrawShapes.Square(positionInWorldSpace, absSizeOfPointVisualization_inWorldSpace, line_thisPointIsPartOf.Color, line_thisPointIsPartOf.Chart_thisLineIsPartOf.InternalRotation, absLineWidth_forPointVisualisators_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, filledWithSpokes, false, durationInSec, hiddenByNearerObjects);
                        break;
                    case ChartLine.DataPointVisualization.triangle:
                        filledWithSpokes = false;
                        halfAbsSizeOfPointVisualization_inWorldSpace = 0.5f * absSizeOfPointVisualization_inWorldSpace;
                        DrawShapes.Triangle(positionInWorldSpace, halfAbsSizeOfPointVisualization_inWorldSpace, line_thisPointIsPartOf.Color, line_thisPointIsPartOf.Chart_thisLineIsPartOf.InternalRotation, absLineWidth_forPointVisualisators_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, filledWithSpokes, false, durationInSec, hiddenByNearerObjects);
                        break;
                    case ChartLine.DataPointVisualization.triangleCrossed:
                        filledWithSpokes = true;
                        halfAbsSizeOfPointVisualization_inWorldSpace = 0.5f * absSizeOfPointVisualization_inWorldSpace;
                        DrawShapes.Triangle(positionInWorldSpace, halfAbsSizeOfPointVisualization_inWorldSpace, line_thisPointIsPartOf.Color, line_thisPointIsPartOf.Chart_thisLineIsPartOf.InternalRotation, absLineWidth_forPointVisualisators_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, filledWithSpokes, false, durationInSec, hiddenByNearerObjects);
                        break;
                    case ChartLine.DataPointVisualization.pentagon:
                        filledWithSpokes = false;
                        halfAbsSizeOfPointVisualization_inWorldSpace = 0.5f * absSizeOfPointVisualization_inWorldSpace;
                        DrawShapes.Pentagon(positionInWorldSpace, halfAbsSizeOfPointVisualization_inWorldSpace, line_thisPointIsPartOf.Color, line_thisPointIsPartOf.Chart_thisLineIsPartOf.InternalRotation, absLineWidth_forPointVisualisators_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, filledWithSpokes, false, durationInSec, hiddenByNearerObjects);
                        break;
                    case ChartLine.DataPointVisualization.pentagonCrossed:
                        filledWithSpokes = true;
                        halfAbsSizeOfPointVisualization_inWorldSpace = 0.5f * absSizeOfPointVisualization_inWorldSpace;
                        DrawShapes.Pentagon(positionInWorldSpace, halfAbsSizeOfPointVisualization_inWorldSpace, line_thisPointIsPartOf.Color, line_thisPointIsPartOf.Chart_thisLineIsPartOf.InternalRotation, absLineWidth_forPointVisualisators_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, filledWithSpokes, false, durationInSec, hiddenByNearerObjects);
                        break;
                    case ChartLine.DataPointVisualization.circle:
                        halfAbsSizeOfPointVisualization_inWorldSpace = 0.5f * absSizeOfPointVisualization_inWorldSpace;
                        filledWithSpokes = false;
                        DrawShapes.Decagon(positionInWorldSpace, halfAbsSizeOfPointVisualization_inWorldSpace, line_thisPointIsPartOf.Color, line_thisPointIsPartOf.Chart_thisLineIsPartOf.InternalRotation, absLineWidth_forPointVisualisators_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, filledWithSpokes, false, durationInSec, hiddenByNearerObjects);
                        break;
                    case ChartLine.DataPointVisualization.circleFilled:
                        halfAbsSizeOfPointVisualization_inWorldSpace = 0.5f * absSizeOfPointVisualization_inWorldSpace;
                        filledWithSpokes = true;
                        DrawShapes.Decagon(positionInWorldSpace, halfAbsSizeOfPointVisualization_inWorldSpace, line_thisPointIsPartOf.Color, line_thisPointIsPartOf.Chart_thisLineIsPartOf.InternalRotation, absLineWidth_forPointVisualisators_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, filledWithSpokes, false, durationInSec, hiddenByNearerObjects);
                        break;
                    case ChartLine.DataPointVisualization.star4corners:
                        filledWithSpokes = false;
                        numberOfCornersOfStarShape = 4;
                        halfAbsSizeOfPointVisualization_inWorldSpace = 0.5f * absSizeOfPointVisualization_inWorldSpace;
                        DrawShapes.Star(positionInWorldSpace, halfAbsSizeOfPointVisualization_inWorldSpace, line_thisPointIsPartOf.Color, numberOfCornersOfStarShape, 0.5f, line_thisPointIsPartOf.Chart_thisLineIsPartOf.InternalRotation, absLineWidth_forPointVisualisators_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, filledWithSpokes, false, durationInSec, hiddenByNearerObjects);
                        break;
                    case ChartLine.DataPointVisualization.star4CornersCrossed:
                        filledWithSpokes = true;
                        numberOfCornersOfStarShape = 4;
                        halfAbsSizeOfPointVisualization_inWorldSpace = 0.5f * absSizeOfPointVisualization_inWorldSpace;
                        DrawShapes.Star(positionInWorldSpace, halfAbsSizeOfPointVisualization_inWorldSpace, line_thisPointIsPartOf.Color, numberOfCornersOfStarShape, 0.5f, line_thisPointIsPartOf.Chart_thisLineIsPartOf.InternalRotation, absLineWidth_forPointVisualisators_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, filledWithSpokes, false, durationInSec, hiddenByNearerObjects);
                        break;
                    case ChartLine.DataPointVisualization.star5corners:
                        filledWithSpokes = false;
                        numberOfCornersOfStarShape = 5;
                        halfAbsSizeOfPointVisualization_inWorldSpace = 0.5f * absSizeOfPointVisualization_inWorldSpace;
                        DrawShapes.Star(positionInWorldSpace, halfAbsSizeOfPointVisualization_inWorldSpace, line_thisPointIsPartOf.Color, numberOfCornersOfStarShape, 0.5f, line_thisPointIsPartOf.Chart_thisLineIsPartOf.InternalRotation, absLineWidth_forPointVisualisators_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, filledWithSpokes, false, durationInSec, hiddenByNearerObjects);
                        break;
                    case ChartLine.DataPointVisualization.star5CornersCrossed:
                        filledWithSpokes = true;
                        numberOfCornersOfStarShape = 5;
                        halfAbsSizeOfPointVisualization_inWorldSpace = 0.5f * absSizeOfPointVisualization_inWorldSpace;
                        DrawShapes.Star(positionInWorldSpace, halfAbsSizeOfPointVisualization_inWorldSpace, line_thisPointIsPartOf.Color, numberOfCornersOfStarShape, 0.5f, line_thisPointIsPartOf.Chart_thisLineIsPartOf.InternalRotation, absLineWidth_forPointVisualisators_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, filledWithSpokes, false, durationInSec, hiddenByNearerObjects);
                        break;
                    case ChartLine.DataPointVisualization.heart:
                        float heartIconSize = absSizeOfPointVisualization_inWorldSpace * 1.25f;
                        int strokeWidth_asPPMofSize_ofHeartIcon = CalcIconWidth_asPPMofSize(heartIconSize, absLineWidth_forPointVisualisators_worldSpace);
                        DrawBasics.Icon(positionInWorldSpace, DrawBasics.IconType.heart, line_thisPointIsPartOf.Color, heartIconSize, null, line_thisPointIsPartOf.Chart_thisLineIsPartOf.InternalRotation, strokeWidth_asPPMofSize_ofHeartIcon, false, durationInSec, hiddenByNearerObjects);
                        break;
                    case ChartLine.DataPointVisualization.customIconSymbol:
                        float iconSize = absSizeOfPointVisualization_inWorldSpace * 1.35f;
                        int strokeWidth_asPPMofSize = CalcIconWidth_asPPMofSize(iconSize, absLineWidth_forPointVisualisators_worldSpace);
                        DrawBasics.Icon(positionInWorldSpace, line_thisPointIsPartOf.customIconAsDatapointVisualization, line_thisPointIsPartOf.Color, iconSize, null, line_thisPointIsPartOf.Chart_thisLineIsPartOf.InternalRotation, strokeWidth_asPPMofSize, false, durationInSec, hiddenByNearerObjects);
                        break;
                    default:
                        break;
                }
            }
        }

        int CalcIconWidth_asPPMofSize(float iconSize, float absLineWidth_forPointVisualisators_worldSpace)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(absLineWidth_forPointVisualisators_worldSpace))
            {
                return 0;
            }
            else
            {
                return Mathf.RoundToInt((absLineWidth_forPointVisualisators_worldSpace / iconSize) * 1000000.0f);
            }
        }

        void DrawVerticalFillLineFromXAxisToPoint(float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(line_thisPointIsPartOf.Alpha_ofVerticalAreaFillLines) == false)
            {
                if (line_thisPointIsPartOf.Alpha_ofVerticalAreaFillLines > 0.0f)
                {
                    Color colorOfVertLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(line_thisPointIsPartOf.Color, line_thisPointIsPartOf.Alpha_ofVerticalAreaFillLines);
                    Vector2 positionsVertProjectionOntoXAxis_inChartSpace = new Vector2(xValue, line_thisPointIsPartOf.Chart_thisLineIsPartOf.yAxis.ValueMarkingLowerEndOfTheAxis);
                    Vector3 positionsVertProjectionOntoXAxis_inWorldSpace = line_thisPointIsPartOf.Chart_thisLineIsPartOf.ChartSpace_to_WorldSpace(positionsVertProjectionOntoXAxis_inChartSpace);
                    Line_fadeableAnimSpeed.InternalDraw(positionInWorldSpace, positionsVertProjectionOntoXAxis_inWorldSpace, colorOfVertLine, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                }
            }
        }

        void TryDrawLittleEmphasizingCircleAroundPoint(float absLineWidth_worldSpace, float durationInSec, bool hiddenByNearerObjects)
        {
            if (hasLittleEmphasizingCircleAroundPoint)
            {
                float radius = line_thisPointIsPartOf.Chart_thisLineIsPartOf.Height_inWorldSpace * 0.005f + 0.5f * absLineWidth_worldSpace;
                DrawShapes.Decagon(positionInWorldSpace, radius, line_thisPointIsPartOf.Color, line_thisPointIsPartOf.Chart_thisLineIsPartOf.InternalRotation, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
            }
        }

        public bool HasAPrecedingPointFromWhichLineCanBeDrawn()
        {
            //needs "DetermineIfAndHowPointIsDrawn" called beforehand
            return (i_ofPrecedingPointFromWhichLineCanBeDrawn >= 0);
        }

        public string GetInvalidTypeString()
        {
            switch (validState)
            {
                case ValidState.isValid:
                    return "Is valid: " + GetStringTheDisplaysTheXandYvalue();
                case ValidState.isNaN_atX:
                    return GetStringTheDisplaysTheXandYvalue();
                case ValidState.isNaN_atY:
                    return GetStringTheDisplaysTheXandYvalue();
                case ValidState.isPositiveInfinity_atX:
                    return GetStringTheDisplaysTheXandYvalue();
                case ValidState.isPositiveInfinity_atY:
                    return GetStringTheDisplaysTheXandYvalue();
                case ValidState.isNegativeInfinity_atX:
                    return GetStringTheDisplaysTheXandYvalue();
                case ValidState.isNegativeInfinity_atY:
                    return GetStringTheDisplaysTheXandYvalue();
                case ValidState.isPlaceholderDuringPhasesWhereThisLineOfListDidntExistsDueToListChangedItsCount_existsOnlyToStayInXSyncWithOtherLinesOfList:
                    return "Collection index didn't exist at 'AddValues_eachIndexIsALine()'-call (Length/Count was too small).";
                default:
                    return "[<color=#ce0e0eFF><icon=logMessageError></color> ValidState of " + validState + " not implemented]";
            }
        }

        string GetStringTheDisplaysTheXandYvalue()
        {
            return "x = " + xValue + " / y = " + yValue;
        }

        public static ValidState GetValidStateOf_datapointToBeCreated(bool newDatapoint_isPlaceholderDuringPhasesWhereThisLineOfListDidntExistsDueToListChangedItsCount_existsOnlyToStayInXSyncWithOtherLinesOfList, float xValue, float yValue)
        {
            ValidState validStateOfNewDataPoint = ValidState.isValid;
            if (newDatapoint_isPlaceholderDuringPhasesWhereThisLineOfListDidntExistsDueToListChangedItsCount_existsOnlyToStayInXSyncWithOtherLinesOfList)
            {
                validStateOfNewDataPoint = ValidState.isPlaceholderDuringPhasesWhereThisLineOfListDidntExistsDueToListChangedItsCount_existsOnlyToStayInXSyncWithOtherLinesOfList;
            }
            else
            {
                if (float.IsNaN(xValue))
                {
                    validStateOfNewDataPoint = ValidState.isNaN_atX;
                }
                else
                {
                    if (float.IsPositiveInfinity(xValue))
                    {
                        validStateOfNewDataPoint = ValidState.isPositiveInfinity_atX;
                    }
                    else
                    {
                        if (float.IsNegativeInfinity(xValue))
                        {
                            validStateOfNewDataPoint = ValidState.isNegativeInfinity_atX;
                        }
                        else
                        {
                            if (float.IsNaN(yValue))
                            {
                                validStateOfNewDataPoint = ValidState.isNaN_atY;
                            }
                            else
                            {
                                if (float.IsPositiveInfinity(yValue))
                                {
                                    validStateOfNewDataPoint = ValidState.isPositiveInfinity_atY;
                                }
                                else
                                {
                                    if (float.IsNegativeInfinity(yValue))
                                    {
                                        validStateOfNewDataPoint = ValidState.isNegativeInfinity_atY;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return validStateOfNewDataPoint;
        }

    }

}
