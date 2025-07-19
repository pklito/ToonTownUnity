namespace DrawXXL
{
    using UnityEngine;
    public class PieChartSegment
    {
        private float valueAsFloat;
        public float ValueAsFloat
        {
            get { return valueAsFloat; }
            set { Debug.LogError("'" + name + ".ValueAsFloat' cannot be set directly. Use 'pieChart.SetValue(" + name + " , valueToSet)' instead"); }
        }

        int valueAsInt;
        float percentage_0to1;
        public string name;
        public Color color;
        bool hasAtLeastBeenFilledOnceWithFloatInsteadOfInt = false;
        PieChartDrawing chart_thisSegmentIsPartOf;
        public bool isTooSmallToBeDrawn;
        public float angleDeg_insidePie;
        public int i_ofThisSegment_insideCreationOrderedList;
        float radius_ofPieCircle;
        float startingAngleDegCCFromUp;
        float turnAngleDegCC;
        Vector3 center_to_startPosOfThinOuterBorderLine;
        string drawnText;
        bool textShouldBeDrawnIntoRightColumn_becauseSpaceAtChartWasTooSmall;
        public bool textInRightColumn_shouldBeInsideTheOthersSection;
        float startAngleDegCCFromUp_ofTextBlock;
        float endAngleDegCCFromUp_ofTextBlock;
        Vector3 positionOfText;
        float sizeOfText;
        DrawText.TextAnchorDXXL textAnchor;

        public PieChartSegment(string name, PieChartDrawing parentChart, int i_ofThisSegment_insideCreationOrderedList)
        {
            this.name = name;
            hasAtLeastBeenFilledOnceWithFloatInsteadOfInt = false;
            chart_thisSegmentIsPartOf = parentChart;
            this.i_ofThisSegment_insideCreationOrderedList = i_ofThisSegment_insideCreationOrderedList;
        }

        public void AddValue(float addedValue)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                if (UtilitiesDXXL_Math.FloatIsValid(addedValue))
                {
                    hasAtLeastBeenFilledOnceWithFloatInsteadOfInt = true;
                    if (chart_thisSegmentIsPartOf.trackNegativeValues == false) { valueAsFloat = Mathf.Max(valueAsFloat, 0.0f); }
                    valueAsFloat = valueAsFloat + addedValue;
                    if (chart_thisSegmentIsPartOf.trackNegativeValues == false) { valueAsFloat = Mathf.Max(valueAsFloat, 0.0f); }
                    valueAsInt = Mathf.RoundToInt(valueAsFloat);
                }
                else
                {
                    Debug.LogError("Add value to pie chart segment '" + name + "' failed, because added value is " + addedValue);
                }
            }
        }

        public void AddValue(int addedValue)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                if (hasAtLeastBeenFilledOnceWithFloatInsteadOfInt)
                {
                    AddValue((float)addedValue);
                }
                else
                {
                    if (chart_thisSegmentIsPartOf.trackNegativeValues == false) { valueAsInt = Mathf.Max(valueAsInt, 0); }
                    valueAsInt = valueAsInt + addedValue;
                    if (chart_thisSegmentIsPartOf.trackNegativeValues == false) { valueAsInt = Mathf.Max(valueAsInt, 0); }
                    valueAsFloat = (float)valueAsInt;
                }
            }
        }

        public void SetValue(float newValue)
        {
            if (UtilitiesDXXL_Math.FloatIsValid(newValue))
            {
                hasAtLeastBeenFilledOnceWithFloatInsteadOfInt = true;
                valueAsFloat = newValue;
                if (chart_thisSegmentIsPartOf.trackNegativeValues == false) { valueAsFloat = Mathf.Max(valueAsFloat, 0.0f); }
                valueAsInt = Mathf.RoundToInt(newValue);
            }
            else
            {
                Debug.LogError("Set value of pie chart segment '" + name + "' failed, because new value is " + newValue);
            }
        }

        public void SetValue(int newValue)
        {
            hasAtLeastBeenFilledOnceWithFloatInsteadOfInt = false;
            valueAsInt = newValue;
            if (chart_thisSegmentIsPartOf.trackNegativeValues == false) { valueAsInt = Mathf.Max(valueAsInt, 0); }
            valueAsFloat = (float)newValue;
        }

        public void CalcPercentage(float sum_ofAllNonNegativeSegments)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(sum_ofAllNonNegativeSegments))
            {
                percentage_0to1 = 0.0f;
            }
            else
            {
                percentage_0to1 = valueAsFloat / sum_ofAllNonNegativeSegments;
            }
            isTooSmallToBeDrawn = ((100.0f * percentage_0to1) < chart_thisSegmentIsPartOf.PercentageThreshold_belowWhichSegmentsGetCombinedInto_othersSection);
            angleDeg_insidePie = 360.0f * percentage_0to1;
        }

        public float TryDrawCircledLines(float startingAngleDegCCFromUp, float durationInSec, bool hiddenByNearerObjects)
        {
            this.startingAngleDegCCFromUp = startingAngleDegCCFromUp;
            radius_ofPieCircle = 0.5f * chart_thisSegmentIsPartOf.Size_ofPieCircleDiameter;

            if (isTooSmallToBeDrawn == false)
            {
                float radius_ofBroadFillLine = radius_ofPieCircle * 0.55f;
                float width_ofBroadFillLine = radius_ofPieCircle * 0.9f;

                Quaternion rotation_fromPieUp_to_segmentStart = Quaternion.AngleAxis(startingAngleDegCCFromUp, chart_thisSegmentIsPartOf.Forward_normalized);
                Vector3 center_to_startPosOfBroadLine_normalized = rotation_fromPieUp_to_segmentStart * chart_thisSegmentIsPartOf.Up_normalized;
                Vector3 center_to_startPosOfBroadLine = center_to_startPosOfBroadLine_normalized * radius_ofBroadFillLine;
                Vector3 startPosOfBroadLine = chart_thisSegmentIsPartOf.Position_worldspace + center_to_startPosOfBroadLine;
                turnAngleDegCC = chart_thisSegmentIsPartOf.OrderingIsClockwise() ? (-angleDeg_insidePie) : angleDeg_insidePie;
                DrawBasics.LineCircled(startPosOfBroadLine, chart_thisSegmentIsPartOf.Position_worldspace, chart_thisSegmentIsPartOf.Forward_normalized, turnAngleDegCC, color, width_ofBroadFillLine, null, false, true, 45.0f, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, durationInSec, hiddenByNearerObjects);

                center_to_startPosOfThinOuterBorderLine = center_to_startPosOfBroadLine_normalized * radius_ofPieCircle;
                Vector3 startPosOfThinOuterBorderLine = chart_thisSegmentIsPartOf.Position_worldspace + center_to_startPosOfThinOuterBorderLine;
                DrawBasics.LineCircled(startPosOfThinOuterBorderLine, chart_thisSegmentIsPartOf.Position_worldspace, chart_thisSegmentIsPartOf.Forward_normalized, turnAngleDegCC, color, 0.0f, null, false, true, 45.0f, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, durationInSec, hiddenByNearerObjects);

                float endingAngleDegCCFromUp = startingAngleDegCCFromUp + turnAngleDegCC;
                return endingAngleDegCCFromUp;
            }
            else
            {
                float endingAngleDegCCFromUp = startingAngleDegCCFromUp;
                return endingAngleDegCCFromUp;
            }
        }

        public void TryDrawStraightSegmentBorderLine(Color color_ofPrecedingSegment, float durationInSec, bool hiddenByNearerObjects)
        {
            if (isTooSmallToBeDrawn == false)
            {
                float lengthOfStripes = 0.03f * radius_ofPieCircle;
                LineWithAlternatingColors_fadeableAnimSpeed.InternalDraw(chart_thisSegmentIsPartOf.Position_worldspace, chart_thisSegmentIsPartOf.Position_worldspace + center_to_startPosOfThinOuterBorderLine, color_ofPrecedingSegment, color, 0.0f, lengthOfStripes, null, 0.0f, null, default(Vector3), true, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, true, true);
            }
        }

        public void PrepareTextDrawing()
        {
            sizeOfText = 0.05f * radius_ofPieCircle * chart_thisSegmentIsPartOf.RelSize_ofSegmentsNameTexts;
            drawnText = GetDrawnText();
            if (drawnText == null) { return; }

            if (isTooSmallToBeDrawn == false)
            {
                textInRightColumn_shouldBeInsideTheOthersSection = false;

                float segmentMiddleAngleDegCCFromUp = startingAngleDegCCFromUp + 0.5f * turnAngleDegCC;
                float segmentMiddleAngleDegCCFromUp_loopedToSpanOf_0to360 = UtilitiesDXXL_Math.Loop_floatIntoSpanFrom_0_to_x(segmentMiddleAngleDegCCFromUp, 360.0f);

                Quaternion rotation_fromPieUp_to_segmentCenter = Quaternion.AngleAxis(segmentMiddleAngleDegCCFromUp, chart_thisSegmentIsPartOf.Forward_normalized);

                Vector3 center_to_segmentMiddle_normalized = rotation_fromPieUp_to_segmentCenter * chart_thisSegmentIsPartOf.Up_normalized;

                //This doesn't write yet, but only fills parsedTextSpecs:
                UtilitiesDXXL_Text.Write(drawnText, Vector3.zero, default(Color), sizeOfText, Vector3.right, Vector3.up, DrawText.TextAnchorDXXL.MiddleCenter, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, false, 0.0f, true, true, false, true);
                float width_ofTextBlock = DrawText.parsedTextSpecs.widthOfLongestLine;
                float height_ofTextBlock = DrawText.parsedTextSpecs.height_wholeTextBlock;

                Quaternion rotation_aroundGlobalForward_fromGlobalUp_toSegmentMiddleIfChartIsUnrotatedInsideXYPlane = Quaternion.AngleAxis(segmentMiddleAngleDegCCFromUp, Vector3.forward);
                Vector2 center_to_segmentMiddle_unrotatedV3 = rotation_aroundGlobalForward_fromGlobalUp_toSegmentMiddleIfChartIsUnrotatedInsideXYPlane * Vector3.up;
                Vector2 center_to_segmentMiddle_unrotatedV2 = new Vector2(center_to_segmentMiddle_unrotatedV3.x, center_to_segmentMiddle_unrotatedV3.y);
                Vector2 intersectionOn2DUnitBoundsWithLowLeftAtZero_towardsCircleCenter = InternalDXXL_BoundsCamViewportSpace.GetViewportCenterPlumbIntersectionWithViewportBorder(InternalDXXL_BoundsCamViewportSpace.viewportCenter - center_to_segmentMiddle_unrotatedV2);
                Vector2 center_of2DTextBoundsWithLowLeftAtZero = 0.5f * new Vector2(width_ofTextBlock, height_ofTextBlock);
                Vector2 intersectionOn2DTextBoundsWithLowLeftAtZero_towardsCircleCenter = new Vector2(intersectionOn2DUnitBoundsWithLowLeftAtZero_towardsCircleCenter.x * width_ofTextBlock, intersectionOn2DUnitBoundsWithLowLeftAtZero_towardsCircleCenter.y * height_ofTextBlock);
                float distance_fromTextBlockBoundsIntersectionTowardsCircleCenter_toTextCenter = (intersectionOn2DTextBoundsWithLowLeftAtZero_towardsCircleCenter - center_of2DTextBoundsWithLowLeftAtZero).magnitude;
                float distanceOfTextToCircleCenter = radius_ofPieCircle * 1.1f + distance_fromTextBlockBoundsIntersectionTowardsCircleCenter_toTextCenter;
                positionOfText = chart_thisSegmentIsPartOf.Position_worldspace + center_to_segmentMiddle_normalized * distanceOfTextToCircleCenter;
                textAnchor = DrawText.TextAnchorDXXL.MiddleCenter; //-> this prevents jumping of the text position, compared to a solution where the anchor is at the side of the textBlock that is nearest to the circleCenter

                float enclosingBox_paddingSize_relToTextSize = 0.0f;//-> this defines the enclosingBoxVertices that get used by "Get_angleDeg_thatTextWouldCover()"
                //again: This doesn't write yet, but only fills parsedTextSpecs:
                UtilitiesDXXL_Text.Write(drawnText, positionOfText, default(Color), sizeOfText, chart_thisSegmentIsPartOf.Right_normalized, chart_thisSegmentIsPartOf.Up_normalized, textAnchor, DrawBasics.LineStyle.solid, 0.0f, enclosingBox_paddingSize_relToTextSize, 0.0f, 0.0f, 0.0f, chart_thisSegmentIsPartOf.autoFlipAllText_toFitObsererCamera, 0.0f, true, true, false, true);
                float angleDeg_thatTextWouldCover = Get_angleDeg_thatTextWouldCover(segmentMiddleAngleDegCCFromUp_loopedToSpanOf_0to360); //-> this depends on the preceding fake "Write()"-call
                float halfAngleDeg_thatTextWouldCover = 0.5f * angleDeg_thatTextWouldCover;
                startAngleDegCCFromUp_ofTextBlock = segmentMiddleAngleDegCCFromUp_loopedToSpanOf_0to360 - halfAngleDeg_thatTextWouldCover;
                endAngleDegCCFromUp_ofTextBlock = segmentMiddleAngleDegCCFromUp_loopedToSpanOf_0to360 + halfAngleDeg_thatTextWouldCover;
            }
            else
            {
                textShouldBeDrawnIntoRightColumn_becauseSpaceAtChartWasTooSmall = true;
                textInRightColumn_shouldBeInsideTheOthersSection = true;
            }
        }

        public void TryDrawTextBesideSegment(float durationInSec, bool hiddenByNearerObjects)
        {
            //-> caller guarantees: "isTooSmallToBeDrawn == false" or "isOthersSection"
            if (drawnText == null) { return; }
            if (chart_thisSegmentIsPartOf.CheckIfAngleForTextIsAlreadyCovered(startAngleDegCCFromUp_ofTextBlock, endAngleDegCCFromUp_ofTextBlock))
            {
                textShouldBeDrawnIntoRightColumn_becauseSpaceAtChartWasTooSmall = true;
            }
            else
            {
                UtilitiesDXXL_Text.Write(drawnText, positionOfText, color, sizeOfText, chart_thisSegmentIsPartOf.Right_normalized, chart_thisSegmentIsPartOf.Up_normalized, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, chart_thisSegmentIsPartOf.autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects, false, false, true);
                textShouldBeDrawnIntoRightColumn_becauseSpaceAtChartWasTooSmall = false;
                chart_thisSegmentIsPartOf.MarkAngleSpanAsCoveredWithText(startAngleDegCCFromUp_ofTextBlock, endAngleDegCCFromUp_ofTextBlock);
            }
        }

        public void ReportThat_textShouldBeDrawnIntoRightColumn_becauseSpaceAtChartWasTooSmall()
        {
            textShouldBeDrawnIntoRightColumn_becauseSpaceAtChartWasTooSmall = true;
        }

        public float TryDrawTextInRightColumnOutsideOthersSection(bool thisIsTheOthersSegment, float vertOffset_ofTextBlock, float durationInSec, bool hiddenByNearerObjects)
        {
            float vertOffset_ofNextTextBlock = vertOffset_ofTextBlock;
            if (drawnText == null) { return vertOffset_ofNextTextBlock; }
            if (CheckIfTextIsDrawn_inRightColumnOutsideOthersSection() || thisIsTheOthersSegment)
            {
                Vector3 topLeftPos_ofTextBlock = Get_topLeftPos_ofTextBlock(vertOffset_ofTextBlock);
                UtilitiesDXXL_Text.Write(drawnText, topLeftPos_ofTextBlock, color, sizeOfText, chart_thisSegmentIsPartOf.Right_normalized, chart_thisSegmentIsPartOf.Up_normalized, DrawText.TextAnchorDXXL.UpperLeft, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, chart_thisSegmentIsPartOf.autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects, false, false, true);
                vertOffset_ofNextTextBlock = Get_vertOffset_ofNextTextBlock(vertOffset_ofTextBlock);
            }
            return vertOffset_ofNextTextBlock;
        }

        bool CheckIfTextIsDrawn_inRightColumnOutsideOthersSection()
        {
            if (textShouldBeDrawnIntoRightColumn_becauseSpaceAtChartWasTooSmall)
            {
                if (textInRightColumn_shouldBeInsideTheOthersSection == false)
                {
                    if (chart_thisSegmentIsPartOf.mentionZeroSegmentsInLegend || (valueAsFloat > 0.0f))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public float TryDrawTextInRightColumnInsideOthersSection(float vertOffset_ofTextBlock, Color color_ofOthersSegment, float durationInSec, bool hiddenByNearerObjects)
        {
            float vertOffset_ofNextTextBlock = vertOffset_ofTextBlock;
            if (drawnText == null) { return vertOffset_ofNextTextBlock; }
            if (textShouldBeDrawnIntoRightColumn_becauseSpaceAtChartWasTooSmall)
            {
                if (textInRightColumn_shouldBeInsideTheOthersSection)
                {
                    if (chart_thisSegmentIsPartOf.mentionZeroSegmentsInLegend || (valueAsFloat > 0.0f))
                    {
                        Vector3 topLeftPos_ofTextBlock = Get_topLeftPos_ofTextBlock(vertOffset_ofTextBlock);

                        bool autoFlipToPreventMirrorInverted = false;
                        UtilitiesDXXL_Text.Write("<icon=arrowRight>", topLeftPos_ofTextBlock, color_ofOthersSegment, 2.0f * sizeOfText, chart_thisSegmentIsPartOf.Right_normalized, chart_thisSegmentIsPartOf.Up_normalized, DrawText.TextAnchorDXXL.UpperLeft, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, false, false, true);

                        float horizOffset_dueToArrow = 4.0f * sizeOfText;
                        Vector3 posOfTextBlock = topLeftPos_ofTextBlock + chart_thisSegmentIsPartOf.Right_normalized * horizOffset_dueToArrow - chart_thisSegmentIsPartOf.Up_normalized * (0.85f * sizeOfText);
                        UtilitiesDXXL_Text.Write(drawnText, posOfTextBlock, color, sizeOfText, chart_thisSegmentIsPartOf.Right_normalized, chart_thisSegmentIsPartOf.Up_normalized, DrawText.TextAnchorDXXL.UpperLeft, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, chart_thisSegmentIsPartOf.autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects, false, false, true);
                        vertOffset_ofNextTextBlock = Get_vertOffset_ofNextTextBlock(vertOffset_ofTextBlock);
                    }
                }
            }
            return vertOffset_ofNextTextBlock;
        }

        float Get_angleDeg_thatTextWouldCover(float segmentMiddleAngleDegCCFromUp_loopedToSpanOf_0to360)
        {
            Vector3 maxAngleDefining_textBlockCorner1;
            Vector3 maxAngleDefining_textBlockCorner2;

            if (segmentMiddleAngleDegCCFromUp_loopedToSpanOf_0to360 < 90.0f)
            {
                maxAngleDefining_textBlockCorner1 = DrawText.parsedTextSpecs.lowLeftPos_ofEnclosingBox;
                maxAngleDefining_textBlockCorner2 = DrawText.parsedTextSpecs.upperRightPos_ofEnclosingBox;
            }
            else
            {
                if (segmentMiddleAngleDegCCFromUp_loopedToSpanOf_0to360 < 180.0f)
                {
                    maxAngleDefining_textBlockCorner1 = DrawText.parsedTextSpecs.lowRightPos_ofEnclosingBox;
                    maxAngleDefining_textBlockCorner2 = DrawText.parsedTextSpecs.upperLeftPos_ofEnclosingBox;
                }
                else
                {
                    if (segmentMiddleAngleDegCCFromUp_loopedToSpanOf_0to360 < 270.0f)
                    {
                        maxAngleDefining_textBlockCorner1 = DrawText.parsedTextSpecs.lowLeftPos_ofEnclosingBox;
                        maxAngleDefining_textBlockCorner2 = DrawText.parsedTextSpecs.upperRightPos_ofEnclosingBox;
                    }
                    else
                    {
                        maxAngleDefining_textBlockCorner1 = DrawText.parsedTextSpecs.lowRightPos_ofEnclosingBox;
                        maxAngleDefining_textBlockCorner2 = DrawText.parsedTextSpecs.upperLeftPos_ofEnclosingBox;
                    }
                }
            }

            Vector3 circleCenter_toTextBlockCorner1 = maxAngleDefining_textBlockCorner1 - chart_thisSegmentIsPartOf.Position_worldspace;
            Vector3 circleCenter_toTextBlockCorner2 = maxAngleDefining_textBlockCorner2 - chart_thisSegmentIsPartOf.Position_worldspace;
            return Vector3.Angle(circleCenter_toTextBlockCorner1, circleCenter_toTextBlockCorner2);
        }

        Vector3 Get_topLeftPos_ofTextBlock(float vertOffset_ofTextBlock)
        {
            float size_ofPieCircleRadius = 0.5f * chart_thisSegmentIsPartOf.Size_ofPieCircleDiameter;
            float offsetDistance_upward = chart_thisSegmentIsPartOf.Get_vertOffset_fromCircleCenter_toTopRightPosOfRightColumn() + vertOffset_ofTextBlock;
            float offsetDistance_sideward = (size_ofPieCircleRadius + size_ofPieCircleRadius * 1.05f * chart_thisSegmentIsPartOf.RelSize_ofSegmentsNameTexts);
            return (chart_thisSegmentIsPartOf.Position_worldspace + chart_thisSegmentIsPartOf.Up_normalized * offsetDistance_upward + chart_thisSegmentIsPartOf.Right_normalized * offsetDistance_sideward);
        }

        float Get_vertOffset_ofNextTextBlock(float vertOffset_ofCurrTextBlock)
        {
            float relDistanceBetweenTextsInsideRightColumn = 0.025f;
            return (vertOffset_ofCurrTextBlock - (DrawText.parsedTextSpecs.height_wholeTextBlock + relDistanceBetweenTextsInsideRightColumn * chart_thisSegmentIsPartOf.Size_ofPieCircleDiameter));
        }

        string GetDrawnText()
        {
            if ((chart_thisSegmentIsPartOf.showSegmentNames == true) && (chart_thisSegmentIsPartOf.showSegmentValues == true) && (chart_thisSegmentIsPartOf.showSegmentPercentages == true))
            {
                //-> show all
                return (name + "<br>" + (hasAtLeastBeenFilledOnceWithFloatInsteadOfInt ? valueAsFloat : valueAsInt) + "<br>" + GetPercentageDisplay());
            }
            else
            {
                if ((chart_thisSegmentIsPartOf.showSegmentNames == false) && (chart_thisSegmentIsPartOf.showSegmentValues == true) && (chart_thisSegmentIsPartOf.showSegmentPercentages == true))
                {
                    return ("" + (hasAtLeastBeenFilledOnceWithFloatInsteadOfInt ? valueAsFloat : valueAsInt) + "<br>" + GetPercentageDisplay());
                }
                else
                {
                    if ((chart_thisSegmentIsPartOf.showSegmentNames == true) && (chart_thisSegmentIsPartOf.showSegmentValues == false) && (chart_thisSegmentIsPartOf.showSegmentPercentages == true))
                    {
                        return (name + "<br>" + GetPercentageDisplay());
                    }
                    else
                    {
                        if ((chart_thisSegmentIsPartOf.showSegmentNames == true) && (chart_thisSegmentIsPartOf.showSegmentValues == true) && (chart_thisSegmentIsPartOf.showSegmentPercentages == false))
                        {
                            return (name + "<br>" + (hasAtLeastBeenFilledOnceWithFloatInsteadOfInt ? valueAsFloat : valueAsInt));
                        }
                        else
                        {
                            if ((chart_thisSegmentIsPartOf.showSegmentNames == true) && (chart_thisSegmentIsPartOf.showSegmentValues == false) && (chart_thisSegmentIsPartOf.showSegmentPercentages == false))
                            {
                                return name;
                            }
                            else
                            {
                                if ((chart_thisSegmentIsPartOf.showSegmentNames == false) && (chart_thisSegmentIsPartOf.showSegmentValues == true) && (chart_thisSegmentIsPartOf.showSegmentPercentages == false))
                                {
                                    return ("" + (hasAtLeastBeenFilledOnceWithFloatInsteadOfInt ? valueAsFloat : valueAsInt));
                                }
                                else
                                {
                                    if ((chart_thisSegmentIsPartOf.showSegmentNames == false) && (chart_thisSegmentIsPartOf.showSegmentValues == false) && (chart_thisSegmentIsPartOf.showSegmentPercentages == true))
                                    {
                                        return GetPercentageDisplay();
                                    }
                                    else
                                    {
                                        //-> show nothing
                                        if (((chart_thisSegmentIsPartOf.showSegmentNames == false) && (chart_thisSegmentIsPartOf.showSegmentValues == false) && (chart_thisSegmentIsPartOf.showSegmentPercentages == false)) == false)
                                        {
                                            UtilitiesDXXL_Log.PrintErrorCode("29-" + chart_thisSegmentIsPartOf.showSegmentNames + "-" + chart_thisSegmentIsPartOf.showSegmentValues + "-" + chart_thisSegmentIsPartOf.showSegmentPercentages);
                                        }
                                        return null;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        string GetPercentageDisplay()
        {
            float value;
            if (chart_thisSegmentIsPartOf.postDecimalPositions_ofPercentageDisplay <= 0)
            {
                value = Mathf.Round(100.0f * percentage_0to1);
            }
            else
            {
                float postDecimalPositions_ofPercentageDisplay_asFloat = (float)chart_thisSegmentIsPartOf.postDecimalPositions_ofPercentageDisplay;
                float factor1 = Mathf.Pow(0.1f, postDecimalPositions_ofPercentageDisplay_asFloat);
                float factor2 = 100.0f * Mathf.Pow(10.0f, postDecimalPositions_ofPercentageDisplay_asFloat);
                value = factor1 * Mathf.Round(factor2 * percentage_0to1);
            }
            return "<size=15>" + value + " %</size>";
        }

    }

}
