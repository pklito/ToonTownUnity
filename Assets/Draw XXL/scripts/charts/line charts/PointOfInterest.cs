namespace DrawXXL
{
    using UnityEngine;

    public class PointOfInterest
    {
        private static float textSize_relToChartHeight = 0.04f;
        public static float TextSize_relToChartHeight //This changes globally for all PointsOfInterest. This only affects the text in the textbox that is connected with a pointer. For the textsize right at the horizontal und vertical line throught the point: see "DimensionOf_PointOfInterest.SizeOfLabeltext_relToChartHeight" and "DimensionOf_PointOfInterest.SizeOfCoordinateText_relToChartHeight".
        {
            get { return textSize_relToChartHeight; }
            set { textSize_relToChartHeight = Mathf.Max(value, 0.002f); }
        }
        public static float distanceBetweenTextBoxes_relToChartHeight = 0.06f;
        public static float pointerConeLength_relToChartHeight = 0.05f;

        public DimensionOf_PointOfInterest xValue;
        public DimensionOf_PointOfInterest yValue;
        public string text; //if you want to let the textbox pointer point horizontally or vertically into the chart (indicating an x-value or y-value rather than an xy-position) you can set one of the two dimensions(xValue/yValue).position to float.NaN
        public Color colorOfPointerTextBox; //this only sets the color of the pointer and the text box. If you want to set the color of the horizonal and vertical line use "SetWholeColor()" or "xValue/yValue.color"
        public bool drawTextBoxIfPointIsOutsideOfChartArea = true;
        public bool isDeletedOnClear = true;
        public bool forceColorOfParent = false; //if this is enabled then "field.colorOfPointerTextBox" and "xValue/yValue.color" and "SetWholeColor" will have no effect anymore but it gets always the color used of the chart or the line this POI is part of. The alpha value is untouched and not forced from the parents.default is false.
        public ChartDrawing chart_thisPointIsPartOf;
        public ChartLine chartLine_thisPointIsPartOf;
        bool isDrawnInNextPass;
        public bool internal_isPOIthatCommunicatesTheHiddenPOIs = false; //"POI" = "point of interest"

        public PointOfInterest(float position_x, float position_y, Color color, ChartDrawing chart_thisPointIsPartOf, ChartLine chartLine_thisPointIsPartOf, string textToDisplay)
        {
            //use ChartDrawing.AddPointOfInterest() instead
            //use ChartLine.AddPointOfInterest() instead
            this.chart_thisPointIsPartOf = chart_thisPointIsPartOf;
            this.chartLine_thisPointIsPartOf = chartLine_thisPointIsPartOf;
            if (chart_thisPointIsPartOf == null && chartLine_thisPointIsPartOf != null)
            {
                this.chart_thisPointIsPartOf = chartLine_thisPointIsPartOf.Chart_thisLineIsPartOf;
            }
            this.colorOfPointerTextBox = color;
            xValue = new DimensionOf_PointOfInterest(position_x, color, UtilitiesDXXL_Math.Dimension.x, chart_thisPointIsPartOf);
            yValue = new DimensionOf_PointOfInterest(position_y, color, UtilitiesDXXL_Math.Dimension.y, chart_thisPointIsPartOf);
            xValue.theOtherDimension = yValue;
            yValue.theOtherDimension = xValue;
            text = textToDisplay;
        }

        public Vector3 TryDraw(Vector3 lowAnchorPositionOfText_inWorldspace, UtilitiesDXXL_Math.SkewedDirection cornerOfChartWhereTextBoxIsMounted, float durationInSec, bool hiddenByNearerObjects)
        {
            //use ChartDrawing.Draw() instead

            //"cornerOfChartWhereTextBoxIsMounted": only "upLeft" and "upRight" are implemented
            if (isDrawnInNextPass)
            {
                TryForceColorFromParent(false);
                xValue.Draw(durationInSec, hiddenByNearerObjects);
                yValue.Draw(durationInSec, hiddenByNearerObjects);
                Vector3 lowAnchorPositionOfNextText_inWorldspace = TryDrawTextBoxAndPointer(lowAnchorPositionOfText_inWorldspace, cornerOfChartWhereTextBoxIsMounted, durationInSec, hiddenByNearerObjects);
                return lowAnchorPositionOfNextText_inWorldspace;
            }
            else
            {
                Vector3 lowAnchorPositionOfNextText_inWorldspace = lowAnchorPositionOfText_inWorldspace;
                return lowAnchorPositionOfNextText_inWorldspace;
            }
        }

        Vector3 TryDrawTextBoxAndPointer(Vector3 lowAnchorPositionOfText_inWorldspace, UtilitiesDXXL_Math.SkewedDirection cornerOfChartWhereTextBoxIsMounted, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 lowAnchorPositionOfNextText_inWorldspace;
            if (HasTextBox())
            {
                GetIfAndHowPointerIsDrawn(out Vector2 posOfPointOfInterest_chartSpace, out bool oneDimIsValid_theOtherOneIsInvalid, out UtilitiesDXXL_Math.DimensionNullable theSingleValidDimension, out bool textboxIsDrawn, out bool pointerIsDrawn, out string printedText);
                if (textboxIsDrawn)
                {
                    lowAnchorPositionOfNextText_inWorldspace = DrawTextBoxAndPointer(pointerIsDrawn, lowAnchorPositionOfText_inWorldspace, posOfPointOfInterest_chartSpace, oneDimIsValid_theOtherOneIsInvalid, theSingleValidDimension, printedText, cornerOfChartWhereTextBoxIsMounted, durationInSec, hiddenByNearerObjects);
                }
                else
                {
                    lowAnchorPositionOfNextText_inWorldspace = lowAnchorPositionOfText_inWorldspace;
                }
            }
            else
            {
                lowAnchorPositionOfNextText_inWorldspace = lowAnchorPositionOfText_inWorldspace;
            }
            return lowAnchorPositionOfNextText_inWorldspace;
        }

        UtilitiesDXXL_Math.DimensionNullable GetTheSingleValidDimension(bool oneDimIsValid_theOtherOneIsInvalid, bool xDim_isValid)
        {
            UtilitiesDXXL_Math.DimensionNullable theSingleValidDimension = UtilitiesDXXL_Math.DimensionNullable.none;
            if (oneDimIsValid_theOtherOneIsInvalid)
            {
                if (xDim_isValid)
                {
                    theSingleValidDimension = UtilitiesDXXL_Math.DimensionNullable.x;
                }
                else
                {
                    theSingleValidDimension = UtilitiesDXXL_Math.DimensionNullable.y;
                }
            }
            return theSingleValidDimension;
        }


        bool CheckIf_isInsideChartArea(bool bothDimValuesAreInvalid, bool bothDimValuesAreValid, UtilitiesDXXL_Math.DimensionNullable theSingleValidDimension, Vector2 posOfPointOfInterest_chartSpace)
        {
            if (bothDimValuesAreInvalid)
            {
                return false;
            }
            else
            {
                if (bothDimValuesAreValid)
                {
                    return chart_thisPointIsPartOf.IsInsideDrawnChartArea(posOfPointOfInterest_chartSpace);
                }
                else
                {
                    if (theSingleValidDimension == UtilitiesDXXL_Math.DimensionNullable.x)
                    {
                        return chart_thisPointIsPartOf.xAxis.IsInsideDisplayedSpan(xValue.position);
                    }
                    else
                    {
                        return chart_thisPointIsPartOf.yAxis.IsInsideDisplayedSpan(yValue.position);
                    }
                }
            }
        }

        public int Internal_Set_isDrawnInNextPass(int stillAvailableTextBoxes)
        {
            if (HasTextBox())
            {
                if (TextBoxWillAnywayNotGetDrawn_becauseOfItsSettingThatHidesItOutsideOfTheChartArea())
                {
                    isDrawnInNextPass = true; //Enable the possibility that a horiz/vert-crosshair may get drawn...
                    return stillAvailableTextBoxes; //...but don't affect the "available text boxes"-count
                }
                else
                {
                    isDrawnInNextPass = (stillAvailableTextBoxes > 0);
                    return (stillAvailableTextBoxes - 1);
                }
            }
            else
            {
                isDrawnInNextPass = true;
                return stillAvailableTextBoxes;
            }
        }

        bool TextBoxWillAnywayNotGetDrawn_becauseOfItsSettingThatHidesItOutsideOfTheChartArea()
        {
            GetIfAndHowPointerIsDrawn(out Vector2 posOfPointOfInterest_chartSpace, out bool oneDimIsValid_theOtherOneIsInvalid, out UtilitiesDXXL_Math.DimensionNullable theSingleValidDimension, out bool textboxIsDrawn, out bool pointerIsDrawn, out string printedText);
            return (!textboxIsDrawn);
        }

        void GetIfAndHowPointerIsDrawn(out Vector2 posOfPointOfInterest_chartSpace, out bool oneDimIsValid_theOtherOneIsInvalid, out UtilitiesDXXL_Math.DimensionNullable theSingleValidDimension, out bool textboxIsDrawn, out bool pointerIsDrawn, out string printedText)
        {
            posOfPointOfInterest_chartSpace = new Vector2(xValue.position, yValue.position);
            bool xDim_isValid = UtilitiesDXXL_Math.FloatIsValid(xValue.position);
            bool yDim_isValid = UtilitiesDXXL_Math.FloatIsValid(yValue.position);
            bool atLeastOneDimValueIsValid = (xDim_isValid || yDim_isValid);
            bool bothDimValuesAreValid = (xDim_isValid && yDim_isValid);
            bool bothDimValuesAreInvalid = ((xDim_isValid == false) && (yDim_isValid == false));
            oneDimIsValid_theOtherOneIsInvalid = (xDim_isValid != yDim_isValid);
            theSingleValidDimension = GetTheSingleValidDimension(oneDimIsValid_theOtherOneIsInvalid, xDim_isValid);
            bool isInsideChartArea = CheckIf_isInsideChartArea(bothDimValuesAreInvalid, bothDimValuesAreValid, theSingleValidDimension, posOfPointOfInterest_chartSpace);
            GetIfAndHowPointerIsDrawn(out textboxIsDrawn, out pointerIsDrawn, out printedText, isInsideChartArea, atLeastOneDimValueIsValid);
        }

        void GetIfAndHowPointerIsDrawn(out bool textboxIsDrawn, out bool pointerIsDrawn, out string printedText, bool isInsideChartArea, bool atLeastOneDimValueIsValid)
        {
            printedText = null;
            if (internal_isPOIthatCommunicatesTheHiddenPOIs)
            {
                textboxIsDrawn = true;
                pointerIsDrawn = false;
                printedText = text;
            }
            else
            {
                if (isInsideChartArea || chart_thisPointIsPartOf.drawValuesOutsideOfChartArea)
                {
                    textboxIsDrawn = true;
                    pointerIsDrawn = atLeastOneDimValueIsValid ? true : false;
                    printedText = text;
                }
                else
                {
                    //->"is OUTSIDE of chart area" and "chart DOESN'T draw values outside of the chart area"
                    pointerIsDrawn = false;
                    if (drawTextBoxIfPointIsOutsideOfChartArea)
                    {
                        textboxIsDrawn = true;
                        printedText = "[<color=#adadadFF><icon=logMessage></color> Is outside of chart area at<br>( " + xValue.position + " / " + yValue.position + " )]<br>" + text;
                    }
                    else
                    {
                        textboxIsDrawn = false;
                    }
                }
            }
        }

        Vector3 DrawTextBoxAndPointer(bool pointerIsDrawn, Vector3 lowAnchorPositionOfText_inWorldspace, Vector2 posOfPointOfInterest_chartSpace, bool oneDimIsValid_theOtherOneIsInvalid, UtilitiesDXXL_Math.DimensionNullable theSingleValidDimension, string printedText, UtilitiesDXXL_Math.SkewedDirection cornerOfChartWhereTextBoxIsMounted, float durationInSec, bool hiddenByNearerObjects)
        {
            float textSize = textSize_relToChartHeight * chart_thisPointIsPartOf.Height_inWorldSpace;
            DrawText.TextAnchorDXXL textAnchor;
            if (cornerOfChartWhereTextBoxIsMounted == UtilitiesDXXL_Math.SkewedDirection.upLeft)
            {
                //topLeftCorner:
                textAnchor = DrawText.TextAnchorDXXL.LowerRight;
            }
            else
            {
                //topRightCorner:
                textAnchor = DrawText.TextAnchorDXXL.LowerLeft;
            }
            float enclosingBox_paddingSize_relToTextSize = 0.0f;
            float autoLineBreakWidth = 0.7f * chart_thisPointIsPartOf.Width_inWorldSpace;

            UtilitiesDXXL_Text.WriteFramed(printedText, lowAnchorPositionOfText_inWorldspace, colorOfPointerTextBox, textSize, chart_thisPointIsPartOf.InternalRotation, textAnchor, DrawBasics.LineStyle.solid, 0.0f, enclosingBox_paddingSize_relToTextSize, 0.0f, 0.0f, autoLineBreakWidth, chart_thisPointIsPartOf.autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects);
            if (pointerIsDrawn)
            {
                DrawPointer(posOfPointOfInterest_chartSpace, oneDimIsValid_theOtherOneIsInvalid, theSingleValidDimension, cornerOfChartWhereTextBoxIsMounted, durationInSec, hiddenByNearerObjects);
            }

            Vector3 highCorner_ofTextBox_worldSpace;
            if (cornerOfChartWhereTextBoxIsMounted == UtilitiesDXXL_Math.SkewedDirection.upLeft)
            {
                //boxes in topLeftCorner:
                highCorner_ofTextBox_worldSpace = DrawText.parsedTextSpecs.upperRightPos_ofEnclosingBox;
            }
            else
            {
                //boxes in topRightCorner:
                highCorner_ofTextBox_worldSpace = DrawText.parsedTextSpecs.upperLeftPos_ofEnclosingBox;
            }
            return (highCorner_ofTextBox_worldSpace + chart_thisPointIsPartOf.yAxis.AxisVector_inWorldSpace * distanceBetweenTextBoxes_relToChartHeight);
        }

        void DrawPointer(Vector2 posOfPointOfInterest_chartSpace, bool oneDimIsValid_theOtherOneIsInvalid, UtilitiesDXXL_Math.DimensionNullable theSingleValidDimension, UtilitiesDXXL_Math.SkewedDirection cornerOfChartWhereTextBoxIsMounted, float durationInSec, bool hiddenByNearerObjects)
        {
            //-> already guaranteed here: at least one dimension value is valid:

            Vector3 lowInnerCorner_ofTextBox_worldSpace;
            Vector3 lowOuterCorner_ofTextBox_worldSpace;
            if (cornerOfChartWhereTextBoxIsMounted == UtilitiesDXXL_Math.SkewedDirection.upLeft)
            {
                //boxes in topLeftCorner:
                lowInnerCorner_ofTextBox_worldSpace = DrawText.parsedTextSpecs.lowRightPos_ofEnclosingBox;
                lowOuterCorner_ofTextBox_worldSpace = DrawText.parsedTextSpecs.lowLeftPos_ofEnclosingBox;
            }
            else
            {
                //boxes in topRightCorner:
                lowInnerCorner_ofTextBox_worldSpace = DrawText.parsedTextSpecs.lowLeftPos_ofEnclosingBox;
                lowOuterCorner_ofTextBox_worldSpace = DrawText.parsedTextSpecs.lowRightPos_ofEnclosingBox;
            }

            float forceFixedConeLength = chart_thisPointIsPartOf.Height_inWorldSpace * pointerConeLength_relToChartHeight;

            bool pointerIsDrawnWithKink_thenAxisParallelIntoChart = oneDimIsValid_theOtherOneIsInvalid;
            if (pointerIsDrawnWithKink_thenAxisParallelIntoChart)
            {
                DrawPointerWithKink(lowInnerCorner_ofTextBox_worldSpace, lowOuterCorner_ofTextBox_worldSpace, theSingleValidDimension, cornerOfChartWhereTextBoxIsMounted, forceFixedConeLength, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                DrawPointerWithoutKink(posOfPointOfInterest_chartSpace, lowInnerCorner_ofTextBox_worldSpace, forceFixedConeLength, durationInSec, hiddenByNearerObjects);
            }
        }

        void DrawPointerWithKink(Vector3 lowInnerCorner_ofTextBox_worldSpace, Vector3 lowOuterCorner_ofTextBox_worldSpace, UtilitiesDXXL_Math.DimensionNullable theSingleValidDimension, UtilitiesDXXL_Math.SkewedDirection cornerOfChartWhereTextBoxIsMounted, float forceFixedConeLength, float durationInSec, bool hiddenByNearerObjects)
        {
            float lengthFactor_forAxisParallelPointerSection = 0.1f;
            Vector3 customAmplitudeAndTextDir;
            Vector3 startPosAtTextBoxCorner_worldSpace;
            Vector3 kinkPosToFinalPointer_worldSpace;
            Vector3 pointerTargetPos_worldSpace;

            if (theSingleValidDimension == UtilitiesDXXL_Math.DimensionNullable.x)
            {
                customAmplitudeAndTextDir = chart_thisPointIsPartOf.xAxis.AxisVector_normalized_inWorldSpace;
                startPosAtTextBoxCorner_worldSpace = lowInnerCorner_ofTextBox_worldSpace;
                Vector2 pointerTargetPos_chartSpace = new Vector2(xValue.position, chart_thisPointIsPartOf.yAxis.ValueMarkingUpperEndOfTheAxis);
                pointerTargetPos_worldSpace = chart_thisPointIsPartOf.ChartSpace_to_WorldSpace(pointerTargetPos_chartSpace) - chart_thisPointIsPartOf.yAxis.AxisVector_normalized_inWorldSpace * (0.5f * pointerConeLength_relToChartHeight);
                kinkPosToFinalPointer_worldSpace = pointerTargetPos_worldSpace + chart_thisPointIsPartOf.yAxis.AxisVector_inWorldSpace * lengthFactor_forAxisParallelPointerSection;
            }
            else
            {
                customAmplitudeAndTextDir = chart_thisPointIsPartOf.yAxis.AxisVector_normalized_inWorldSpace;
                startPosAtTextBoxCorner_worldSpace = lowOuterCorner_ofTextBox_worldSpace;
                if (cornerOfChartWhereTextBoxIsMounted == UtilitiesDXXL_Math.SkewedDirection.upLeft)
                {
                    Vector2 pointerTargetPos_chartSpace = new Vector2(chart_thisPointIsPartOf.xAxis.ValueMarkingLowerEndOfTheAxis, yValue.position);
                    pointerTargetPos_worldSpace = chart_thisPointIsPartOf.ChartSpace_to_WorldSpace(pointerTargetPos_chartSpace);
                    kinkPosToFinalPointer_worldSpace = pointerTargetPos_worldSpace - chart_thisPointIsPartOf.xAxis.AxisVector_inWorldSpace * lengthFactor_forAxisParallelPointerSection;
                }
                else
                {
                    Vector2 pointerTargetPos_chartSpace = new Vector2(chart_thisPointIsPartOf.xAxis.ValueMarkingUpperEndOfTheAxis, yValue.position);
                    pointerTargetPos_worldSpace = chart_thisPointIsPartOf.ChartSpace_to_WorldSpace(pointerTargetPos_chartSpace);
                    kinkPosToFinalPointer_worldSpace = pointerTargetPos_worldSpace + chart_thisPointIsPartOf.xAxis.AxisVector_inWorldSpace * lengthFactor_forAxisParallelPointerSection;
                }
            }

            Line_fadeableAnimSpeed.InternalDraw(startPosAtTextBoxCorner_worldSpace, kinkPosToFinalPointer_worldSpace, colorOfPointerTextBox, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            bool setConeLengthToRelative_notToAbsolute = UtilitiesDXXL_Math.ApproximatelyZero(forceFixedConeLength);
            float coneLength_ifSetToRelative = 0.17f;
            float coneLength_ifSetToAbsolute = forceFixedConeLength;
            float coneLength = UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(setConeLengthToRelative_notToAbsolute, coneLength_ifSetToRelative, coneLength_ifSetToAbsolute);
            DrawBasics.Vector(kinkPosToFinalPointer_worldSpace, pointerTargetPos_worldSpace, colorOfPointerTextBox, 0.0f, null, coneLength, false, true, customAmplitudeAndTextDir, false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
        }

        void DrawPointerWithoutKink(Vector2 posOfPointOfInterest_chartSpace, Vector3 lowInnerCorner_ofTextBox_worldSpace, float forceFixedConeLength, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 customAmplitudeAndTextDir = chart_thisPointIsPartOf.xAxis.AxisVector_normalized_inWorldSpace;
            Vector3 startPosAtTextBoxCorner_worldSpace = lowInnerCorner_ofTextBox_worldSpace;
            Vector3 pointerTargetPos_worldSpace = chart_thisPointIsPartOf.ChartSpace_to_WorldSpace(posOfPointOfInterest_chartSpace);

            bool setConeLengthToRelative_notToAbsolute = UtilitiesDXXL_Math.ApproximatelyZero(forceFixedConeLength);
            float coneLength_ifSetToRelative = 0.17f;
            float coneLength_ifSetToAbsolute = forceFixedConeLength;
            float coneLength = UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(setConeLengthToRelative_notToAbsolute, coneLength_ifSetToRelative, coneLength_ifSetToAbsolute);
            DrawBasics.Vector(startPosAtTextBoxCorner_worldSpace, pointerTargetPos_worldSpace, colorOfPointerTextBox, 0.0f, null, coneLength, false, true, customAmplitudeAndTextDir, false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
        }

        public void SetWholeColor(Color newColor)
        {
            //This not only sets the color of the poiner and the text box (as setting field.colorOfPointerTextBox would) but also of the horizonal and vertical line.
            colorOfPointerTextBox = newColor;
            xValue.color = newColor;
            yValue.color = newColor;
        }

        void TryForceColorFromParent(bool alsoForceAlpha = false)
        {
            if (forceColorOfParent)
            {
                if (chartLine_thisPointIsPartOf != null)
                {
                    colorOfPointerTextBox = new Color(chartLine_thisPointIsPartOf.Color.r, chartLine_thisPointIsPartOf.Color.g, chartLine_thisPointIsPartOf.Color.b, alsoForceAlpha ? chartLine_thisPointIsPartOf.Color.a : colorOfPointerTextBox.a);
                    xValue.color = new Color(chartLine_thisPointIsPartOf.Color.r, chartLine_thisPointIsPartOf.Color.g, chartLine_thisPointIsPartOf.Color.b, alsoForceAlpha ? chartLine_thisPointIsPartOf.Color.a : xValue.color.a);
                    yValue.color = new Color(chartLine_thisPointIsPartOf.Color.r, chartLine_thisPointIsPartOf.Color.g, chartLine_thisPointIsPartOf.Color.b, alsoForceAlpha ? chartLine_thisPointIsPartOf.Color.a : yValue.color.a);
                }
                else
                {
                    colorOfPointerTextBox = new Color(chart_thisPointIsPartOf.color.r, chart_thisPointIsPartOf.color.g, chart_thisPointIsPartOf.color.b, alsoForceAlpha ? chart_thisPointIsPartOf.color.a : colorOfPointerTextBox.a);
                    xValue.color = new Color(chart_thisPointIsPartOf.color.r, chart_thisPointIsPartOf.color.g, chart_thisPointIsPartOf.color.b, alsoForceAlpha ? chart_thisPointIsPartOf.color.a : xValue.color.a);
                    yValue.color = new Color(chart_thisPointIsPartOf.color.r, chart_thisPointIsPartOf.color.g, chart_thisPointIsPartOf.color.b, alsoForceAlpha ? chart_thisPointIsPartOf.color.a : yValue.color.a);
                }
            }
        }

        bool HasTextBox()
        {
            return (text != null && text != "");
        }

    }

}
