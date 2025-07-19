namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;
    
    public class ChartAxis
    {
        public enum Scaling
        {
            dynamic_encapsulateAllValues_includingThoseOutsideOfOtherAxisDisplay,
            dynamic_encapsulateAllValues_butOnlyThoseInsideOfOtherAxisDisplay,
            fixed_relativeToHighestValue,
            fixed_relativeToLowestValue, 
            fixed_relativeToMostCurrentValue,
            fixed_absolute 
        } 

        public enum SourceOfAutomaticValues
        {
            fixedStepForEachValueAdding,
            fixedStep_followingTheManualIncrementFunction,
            frameCount,
            fixedTime, 
            fixedUnscaledTime, 
            realtimeSinceStartup,
            time,
            timeSinceLevelLoad,
            unscaledTime,
            editorTimeSinceStartup
        }


        public float lineWidth_relToChartHeight = 0.0f;
        ChartDrawing chart_thisAxisIsPartOf;
        UtilitiesDXXL_Math.Dimension cartesianDimension;
        public ChartAxis theOtherAxis;
        private string name = null;
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                nameHasBeenManuallySet = true;
            }
        }
        bool nameHasBeenManuallySet = false;
        public float nameText_scaleFactor = 1.0f;
        public Scaling scaling = Scaling.dynamic_encapsulateAllValues_butOnlyThoseInsideOfOtherAxisDisplay;
        public SourceOfAutomaticValues sourceOfAutomaticValues = SourceOfAutomaticValues.fixedStepForEachValueAdding;
        bool allXValuesCameFromAutomaticSource;//only used for x-axis //ignored for y-axis
        public float fixedLowerEndValueOfScale = 0.0f; 
        public float fixedUpperEndValueOfScale = 100.0f;  
        public float fixedDisplayedSpan_belowAnchorValue = 500.0f;  
        public float fixedDisplayedSpan_aboveAnchorValue = 100.0f;   
        float fixedLowerEndValueOfScale_duringInspectionComponentPhases;
        float fixedUpperEndValueOfScale_duringInspectionComponentPhases;
        public int approxNumberOfGraduationIntervals = 5; //has only effect, when "graduationIntervalSource" is "axisSpanDividedByFixedNumberofIntervals", otherwise ignored. The interval number in the chart may in some cases be some more or less than this number (up to doulbe of this number), so it's just a raw specification
        public int ApproxNumberOfGraduationIntervals
        {
            get { return approxNumberOfGraduationIntervals; }
            set { approxNumberOfGraduationIntervals = Mathf.Clamp(value, 2, 10000); }
        }

        public float alphaOfGraduationLinesInsideDataArea = 0.2f; //can be used to switch off or accentuate the graduation lines that extend into the data area of the chart.
        public float alphaOfGraduationNotation = 1.0f; //can be used to switch off or tone down the numbers notation on the axis graduation marks. default is 1
        public bool graduationTypesetting_isManually = false; //if you set this to true, then you can use "AngleDeg_ofAxisGraduationNotationText" and "GraduationTextSize_relToAxisLength".
        float used_angleDeg_ofAxisGraduationNotationText; //only used for xAxis
        private float angleDeg_ofAxisGraduationNotationText = 0.0f;
        public float AngleDeg_ofAxisGraduationNotationText
        {
            //only used for the x-axis. Ignored for the y-axis
            //can be used to change the direction of the notation text at the graduation marks on the x axis. Positive angles turn the text counter clockwise, negative angles turn it clockwise. Is constrained between -90 and +90.
            //has only effect, if "graduationTypesetting_isManually" has been set to "true", otherwise it's ignored
            get { return angleDeg_ofAxisGraduationNotationText; }
            set
            {
                angleDeg_ofAxisGraduationNotationText = Mathf.Clamp(value, -90.0f, 90.0f);
                if (graduationTypesetting_isManually == false)
                {
                    Debug.LogWarning("Setting 'AngleDeg_ofAxisGraduationNotationText' will only have effect if 'graduationTypesetting_isManually' is 'true' (which is not the case at the moment.)");
                }
            }
        }
        private float graduationTextSize_relToAxisLength = 0.02f;
        public float GraduationTextSize_relToAxisLength
        {
            //has only effect, if "graduationTypesetting_isManually" has been set to "true", otherwise it's ignored
            get { return graduationTextSize_relToAxisLength; }
            set
            {
                graduationTextSize_relToAxisLength = Mathf.Clamp(value, 0.001f, 0.3f);
                if (graduationTypesetting_isManually == false)
                {
                    Debug.LogWarning("Setting 'GraduationTextSize_relToAxisLength' will only have effect if 'graduationTypesetting_isManually' is 'true' (which is not the case at the moment.)");
                }
            }
        }

        private float lengthConversionFactor_fromChartScaling_toWorldScaling;
        public float LengthConversionFactor_fromChartScaling_toWorldScaling
        {
            get { return lengthConversionFactor_fromChartScaling_toWorldScaling; }
            set { Debug.LogError("Setting 'LengthConversionFactor_fromChartScaling_toWorldScaling' manually is not supported."); }
        }
        private float valueMarkingTheLowerAxisEnd_convertedToUnitsOfTheUnwarpedUnscaledWorldSpace;
        public float ValueMarkingTheLowerAxisEnd_convertedToUnitsOfTheUnwarpedUnscaledWorldSpace
        {
            get { return valueMarkingTheLowerAxisEnd_convertedToUnitsOfTheUnwarpedUnscaledWorldSpace; }
            set { Debug.LogError("Setting 'ValueMarkingTheLowerAxisEnd_convertedToUnitsOfTheUnwarpedUnscaledWorldSpace' manually is not supported."); }
        }

        Vector3 unrotated_axisVector_normalized_inWorldSpace;
        private Vector3 axisVector_inWorldSpace;
        public Vector3 AxisVector_inWorldSpace
        {
            get { return axisVector_inWorldSpace; }
            set
            {
                if (cartesianDimension == UtilitiesDXXL_Math.Dimension.x)
                {
                    Debug.LogError("Setting 'xAxis.AxisVector_inWorldSpace' manually is not supported. Use 'chartDrawing.Rotation' and 'chartDrawing.Width_inWorldSpace' instead.");
                }
                else
                {
                    Debug.LogError("Setting 'yAxis.AxisVector_inWorldSpace' manually is not supported. Use 'chartDrawing.Rotation' and 'chartDrawing.Height_inWorldSpace' instead.");
                }
            }
        }

        private Vector3 axisVector_normalized_inWorldSpace;
        public Vector3 AxisVector_normalized_inWorldSpace
        {
            get { return axisVector_normalized_inWorldSpace; }
            set { Debug.LogError("Setting 'chartAxis.AxisVector_normalized_inWorldSpace' manually is not supported. Use 'chartDrawing.Rotation' instead."); }
        }

        private float valueMarkingLowerEndOfTheAxis;
        private float valueMarkingUpperEndOfTheAxis;
        public float ValueMarkingLowerEndOfTheAxis
        {
            get { return valueMarkingLowerEndOfTheAxis; }
            set { Debug.LogError("Setting 'valueMarkingLowerEndOfTheAxis' manually is not supported. You can use 'fixedLowerEndValueOfScale' instead (after setting 'scaling' to 'fixed_absolute')."); }
        }
        public float ValueMarkingUpperEndOfTheAxis
        {
            get { return valueMarkingUpperEndOfTheAxis; }
            set { Debug.LogError("Setting 'valueMarkingUpperEndOfTheAxis' manually is not supported. You can use 'fixedUpperEndValueOfScale' instead (after setting 'scaling' to 'fixed_absolute')."); }
        }

        float lowerEndOfAxisToUpperEnd_inChartUnits;

        private float length_inWorldSpace = 1.0f;
        public float Length_inWorldSpace 
        {
            get { return length_inWorldSpace; }
            set
            {
                if (cartesianDimension == UtilitiesDXXL_Math.Dimension.x)
                {
                    Debug.LogError("Setting 'xAxis.length_inWorldSpace' manually is not supported. You can use 'chartDrawing.Width_inWorldSpace' instead.");
                }
                else
                {
                    Debug.LogError("Setting 'yAxis.length_inWorldSpace' manually is not supported. You can use 'chartDrawing.Height_inWorldSpace' instead.");
                }
            }
        }

        List<float> posOfGraduationMarks_inChartSpace = new List<float>();
        List<string> graduationMarksTexts = new List<string>();
        public bool drawZeroPositionAsDottedLine; //default is true for the yAxis and false for the xAxis
        PointOfInterest pointOfInterest_displayingTheZeroPositionLine;
        PointOfInterest pointOfInterest_displayingTheZeroPositionLine_lowAlphaUnderlay;

        public ChartAxis(Vector3 unrotated_axisVector_normalized_inWorldSpace, ChartDrawing chart_thisAxisIsPartOf, UtilitiesDXXL_Math.Dimension cartesianDimension)
        {
            this.unrotated_axisVector_normalized_inWorldSpace = unrotated_axisVector_normalized_inWorldSpace;
            axisVector_inWorldSpace = this.unrotated_axisVector_normalized_inWorldSpace;
            axisVector_normalized_inWorldSpace = this.unrotated_axisVector_normalized_inWorldSpace;
            this.chart_thisAxisIsPartOf = chart_thisAxisIsPartOf;
            this.cartesianDimension = cartesianDimension;
            if (this.cartesianDimension == UtilitiesDXXL_Math.Dimension.x)
            {
                allXValuesCameFromAutomaticSource = true;
                name = GetAxisNameForAutomaticSources();
                drawZeroPositionAsDottedLine = false;
            }
            else
            {
                drawZeroPositionAsDottedLine = true;
            }
            CreatePointsOfInterest_thatDisplaysTheZeroPositionAsDottedLine(out pointOfInterest_displayingTheZeroPositionLine, out pointOfInterest_displayingTheZeroPositionLine_lowAlphaUnderlay);
            chart_thisAxisIsPartOf.AddPointOfInterest(pointOfInterest_displayingTheZeroPositionLine);
            chart_thisAxisIsPartOf.AddPointOfInterest(pointOfInterest_displayingTheZeroPositionLine_lowAlphaUnderlay);
        }

        public void Clear()
        {
            allXValuesCameFromAutomaticSource = true;
        }

        public void RecalcScaling()
        {
            CalcMinMaxOfAxis();
            lowerEndOfAxisToUpperEnd_inChartUnits = valueMarkingUpperEndOfTheAxis - valueMarkingLowerEndOfTheAxis;
            lengthConversionFactor_fromChartScaling_toWorldScaling = length_inWorldSpace / lowerEndOfAxisToUpperEnd_inChartUnits;
            valueMarkingTheLowerAxisEnd_convertedToUnitsOfTheUnwarpedUnscaledWorldSpace = valueMarkingLowerEndOfTheAxis * lengthConversionFactor_fromChartScaling_toWorldScaling;
            SetIf_pointOfInterestDisplayingTheZeroPositionLine_isDisplayed();
        }

        public void Draw(InternalDXXL_Plane chartPlane, float durationInSec, bool hiddenByNearerObjects)
        {
            //use chartDrawing.Draw() instead.
            DXXLWrapperForUntiysBuildInDrawLines.currentlyDrawingChart = chart_thisAxisIsPartOf;
            float fixedConeLength_forBothAxisVectors = Get_fixedConeLength_forBothAxisVectors();
            float lineWidth_worldSpace = 0.0f;
            if (UtilitiesDXXL_Math.ApproximatelyZero(lineWidth_relToChartHeight) == false)
            {
                lineWidth_worldSpace = lineWidth_relToChartHeight * chart_thisAxisIsPartOf.Height_inWorldSpace;
            }

            UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
            UtilitiesDXXL_DrawBasics.VectorFrom(chart_thisAxisIsPartOf.Position_worldspace, axisVector_inWorldSpace, chart_thisAxisIsPartOf.color, lineWidth_worldSpace, null, fixedConeLength_forBothAxisVectors, false, true, false, 0.0f, false, durationInSec, hiddenByNearerObjects, chartPlane, false, 0.0f);
            UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();

            if (chart_thisAxisIsPartOf.IsEmptyWithNoLinesToDraw == false)
            {
                DrawAxisGraduation(durationInSec, hiddenByNearerObjects);
                DrawChartBoundaryLine_inTheStyleOfAGraduationLine(durationInSec, hiddenByNearerObjects);
            }
            DrawAxisName(durationInSec, hiddenByNearerObjects);
            DXXLWrapperForUntiysBuildInDrawLines.currentlyDrawingChart = null;
        }

        public float Get_fixedConeLength_forBothAxisVectors()
        {
            return (0.05f * Mathf.Max(length_inWorldSpace, theOtherAxis.length_inWorldSpace));
        }

        void CalcMinMaxOfAxis()
        {
            //-> lowestValue is guaranteed lower than highestValue after this function
            Scaling used_scaling = (chart_thisAxisIsPartOf.chartInspector_component == null) ? scaling : Scaling.fixed_absolute;
            switch (used_scaling)
            {
                case Scaling.dynamic_encapsulateAllValues_includingThoseOutsideOfOtherAxisDisplay:
                    GetAxisEndsIfAllNonHiddenValuesAreEncapsulated(out valueMarkingLowerEndOfTheAxis, out valueMarkingUpperEndOfTheAxis);
                    return;
                case Scaling.dynamic_encapsulateAllValues_butOnlyThoseInsideOfOtherAxisDisplay:
                    if (theOtherAxis.HasAFixedScalingType())
                    {
                        GetAxisEndsIfAllNonHiddenValuesInsideOtherAxisSpanAreEncapsulated(out valueMarkingLowerEndOfTheAxis, out valueMarkingUpperEndOfTheAxis);
                    }
                    else
                    {
                        GetAxisEndsIfAllNonHiddenValuesAreEncapsulated(out valueMarkingLowerEndOfTheAxis, out valueMarkingUpperEndOfTheAxis);
                    }
                    return;
                case Scaling.fixed_relativeToHighestValue:
                    float highestValueOfAllLines = GetHighestValueOfAllLines();
                    TryExpandInvalidZeroSpanBesideAnchor();
                    valueMarkingLowerEndOfTheAxis = GetMinValueBesideAnchor(highestValueOfAllLines);
                    valueMarkingUpperEndOfTheAxis = GetMaxValueBesideAnchor(highestValueOfAllLines);
                    return;
                case Scaling.fixed_relativeToLowestValue:
                    float lowestValueOfAllLines = GetLowestValueOfAllLines();
                    TryExpandInvalidZeroSpanBesideAnchor();
                    valueMarkingLowerEndOfTheAxis = GetMinValueBesideAnchor(lowestValueOfAllLines);
                    valueMarkingUpperEndOfTheAxis = GetMaxValueBesideAnchor(lowestValueOfAllLines);
                    return;
                case Scaling.fixed_relativeToMostCurrentValue:
                    float mostCurrentValue = GetMostCurrentValueOfAllLines();
                    TryExpandInvalidZeroSpanBesideAnchor();
                    valueMarkingLowerEndOfTheAxis = GetMinValueBesideAnchor(mostCurrentValue);
                    valueMarkingUpperEndOfTheAxis = GetMaxValueBesideAnchor(mostCurrentValue);
                    return;
                case Scaling.fixed_absolute:
                    float used_fixedLowerEndValueOfScale = (chart_thisAxisIsPartOf.chartInspector_component == null) ? fixedLowerEndValueOfScale : fixedLowerEndValueOfScale_duringInspectionComponentPhases;
                    float used_fixedUpperEndValueOfScale = (chart_thisAxisIsPartOf.chartInspector_component == null) ? fixedUpperEndValueOfScale : fixedUpperEndValueOfScale_duringInspectionComponentPhases;
                    if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(used_fixedLowerEndValueOfScale, used_fixedUpperEndValueOfScale))
                    {
                        Debug.LogWarning("Chart: Don't set 'fixedLowerEndValueOfScale' and 'fixedUpperEndValueOfScale' to the same value. Both are currently " + used_fixedLowerEndValueOfScale + ". Fallback -> automatic widening.");
                        float paddingAtEachSide = 1.6f;
                        valueMarkingLowerEndOfTheAxis = used_fixedLowerEndValueOfScale - paddingAtEachSide;
                        valueMarkingUpperEndOfTheAxis = used_fixedLowerEndValueOfScale + paddingAtEachSide;
                    }
                    else
                    {
                        if (used_fixedLowerEndValueOfScale < used_fixedUpperEndValueOfScale)
                        {
                            valueMarkingLowerEndOfTheAxis = used_fixedLowerEndValueOfScale;
                            valueMarkingUpperEndOfTheAxis = used_fixedUpperEndValueOfScale;
                        }
                        else
                        {
                            valueMarkingLowerEndOfTheAxis = used_fixedUpperEndValueOfScale;
                            valueMarkingUpperEndOfTheAxis = used_fixedLowerEndValueOfScale;
                        }
                    }
                    return;
                default:
                    valueMarkingLowerEndOfTheAxis = 0.0f;
                    valueMarkingUpperEndOfTheAxis = 1.0f;
                    Debug.LogError("ScalingType of " + used_scaling + " not implemented.");
                    return;
            }
        }

        public bool HasAFixedScalingType()
        {
            //-> this function is currently only fit for cases without chart components.
            //-> cases with chart components always have a fixed scaling.

            switch (scaling)
            {
                case Scaling.dynamic_encapsulateAllValues_includingThoseOutsideOfOtherAxisDisplay:
                    return false;
                case Scaling.dynamic_encapsulateAllValues_butOnlyThoseInsideOfOtherAxisDisplay:
                    return false;
                case Scaling.fixed_relativeToHighestValue:
                    return true;
                case Scaling.fixed_relativeToLowestValue:
                    return true;
                case Scaling.fixed_relativeToMostCurrentValue:
                    return true;
                case Scaling.fixed_absolute:
                    return true;
                default:
                    return true;
            }
        }

        float GetMostCurrentValueOfAllLines()
        {
            if (cartesianDimension == UtilitiesDXXL_Math.Dimension.x)
            {
                return chart_thisAxisIsPartOf.lines.GetMostCurrentXValueOfAllLines();
            }
            else
            {
                return chart_thisAxisIsPartOf.lines.GetMostCurrentYValueOfAllLines();
            }
        }

        float GetLowestValueOfAllLines()
        {
            if (cartesianDimension == UtilitiesDXXL_Math.Dimension.x)
            {
                return chart_thisAxisIsPartOf.lines.GetLowestXValueOfAllLines();
            }
            else
            {
                return chart_thisAxisIsPartOf.lines.GetLowestYValueOfAllLines();
            }
        }

        float GetHighestValueOfAllLines()
        {
            if (cartesianDimension == UtilitiesDXXL_Math.Dimension.x)
            {
                return chart_thisAxisIsPartOf.lines.GetHighestXValueOfAllLines();
            }
            else
            {
                return chart_thisAxisIsPartOf.lines.GetHighestYValueOfAllLines();
            }
        }

        float GetLowestValueOfAllLines_insideSpanOfOtherAxis()
        {
            if (cartesianDimension == UtilitiesDXXL_Math.Dimension.x)
            {
                return chart_thisAxisIsPartOf.lines.GetLowestXValueOfAllLines_insideRestricedYSpan(theOtherAxis.valueMarkingLowerEndOfTheAxis, theOtherAxis.valueMarkingUpperEndOfTheAxis);
            }
            else
            {
                return chart_thisAxisIsPartOf.lines.GetLowestYValueOfAllLines_insideRestricedXSpan(theOtherAxis.valueMarkingLowerEndOfTheAxis, theOtherAxis.valueMarkingUpperEndOfTheAxis);
            }
        }

        float GetHighestValueOfAllLines_insideSpanOfOtherAxis()
        {
            if (cartesianDimension == UtilitiesDXXL_Math.Dimension.x)
            {
                return chart_thisAxisIsPartOf.lines.GetHighestXValueOfAllLines_insideRestricedYSpan(theOtherAxis.valueMarkingLowerEndOfTheAxis, theOtherAxis.valueMarkingUpperEndOfTheAxis);
            }
            else
            {
                return chart_thisAxisIsPartOf.lines.GetHighestYValueOfAllLines_insideRestricedXSpan(theOtherAxis.valueMarkingLowerEndOfTheAxis, theOtherAxis.valueMarkingUpperEndOfTheAxis);
            }
        }

        void TryExpandInvalidZeroSpanBesideAnchor()
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(fixedDisplayedSpan_belowAnchorValue))
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(fixedDisplayedSpan_aboveAnchorValue))
                {
                    Debug.LogWarning("Chart: Don't set 'displayedSpan_belowAnchorValue' and 'displayedSpan_aboveAnchorValue' both to 0. Fallback -> automatic widening.");
                    float fallbackSpanForEachSide = 1.6f;
                    fixedDisplayedSpan_belowAnchorValue = fallbackSpanForEachSide;
                    fixedDisplayedSpan_aboveAnchorValue = fallbackSpanForEachSide;
                }
            }
        }

        float GetMinValueBesideAnchor(float anchorValue)
        {
            return (anchorValue - Mathf.Abs(fixedDisplayedSpan_belowAnchorValue));
        }

        float GetMaxValueBesideAnchor(float anchorValue)
        {
            return (anchorValue + Mathf.Abs(fixedDisplayedSpan_aboveAnchorValue));
        }

        void DrawAxisGraduation(float durationInSec, bool hiddenByNearerObjects)
        {
            posOfGraduationMarks_inChartSpace.Clear();
            float approxGraduationInterval = lowerEndOfAxisToUpperEnd_inChartUnits / (float)approxNumberOfGraduationIntervals;
            bool calculationOf_orderOfMagnitudeWasSuccesful;
            float decimalOrderOfMagnitude_markingLowerEndOfGraduationInterval = UtilitiesDXXL_Math.GetDecimalOrderOfMagnitudeAtLowerEnd(approxGraduationInterval, out float inverseOf_decimalOrderOfMagnitude_markingLowerEndOfGraduationInterval, out calculationOf_orderOfMagnitudeWasSuccesful);
            if (UtilitiesDXXL_Math.ApproximatelyZero(approxGraduationInterval) || (calculationOf_orderOfMagnitudeWasSuccesful == false))
            {
                Debug.LogWarning("Chart: Failed to draw graduation marks. Approximate graduation interval: " + approxGraduationInterval);
            }
            else
            {
                float approxGraduationInterval_withShiftedDecimalSeparatorTillItsInside1toExcl10 = approxGraduationInterval * inverseOf_decimalOrderOfMagnitude_markingLowerEndOfGraduationInterval;
                float approxGraduationInterval_inside1toExcl10_roundedToInt = Mathf.Floor(approxGraduationInterval_withShiftedDecimalSeparatorTillItsInside1toExcl10);
                float graduationInterval_inChartSpaceUnits = approxGraduationInterval_inside1toExcl10_roundedToInt * decimalOrderOfMagnitude_markingLowerEndOfGraduationInterval;

                //lowest mark:
                float lowEndOfAxis_butDecimalSeparatorHasBeenShiftedTheNumberOfTimesThatTheIntervalNeededTillItReached1toExcl10 = valueMarkingLowerEndOfTheAxis * inverseOf_decimalOrderOfMagnitude_markingLowerEndOfGraduationInterval;
                float lowEndOfAxis_shifted_rounded = Mathf.Floor(lowEndOfAxis_butDecimalSeparatorHasBeenShiftedTheNumberOfTimesThatTheIntervalNeededTillItReached1toExcl10);
                float graduationAnchorPos_below_lowEndOfAxis = lowEndOfAxis_shifted_rounded * decimalOrderOfMagnitude_markingLowerEndOfGraduationInterval;
                int maxNumberOfGraduationMarks = approxNumberOfGraduationIntervals * 2 + 5;
                for (int i = 0; i < maxNumberOfGraduationMarks; i++)
                {
                    float currGraduationMarkPos = graduationAnchorPos_below_lowEndOfAxis + graduationInterval_inChartSpaceUnits * i;
                    if (currGraduationMarkPos >= valueMarkingLowerEndOfTheAxis && currGraduationMarkPos < valueMarkingUpperEndOfTheAxis)
                    {
                        //fixing values like "0.0999999999" to "0.1":
                        float currGraduationMarkPos_butDecimalSeparatorHasBeenShiftedTheNumberOfTimesThatTheIntervalNeededTillItReached1toExcl10 = currGraduationMarkPos * inverseOf_decimalOrderOfMagnitude_markingLowerEndOfGraduationInterval;
                        float currGraduationMarkPos_shifted_rounded = Mathf.Round(currGraduationMarkPos_butDecimalSeparatorHasBeenShiftedTheNumberOfTimesThatTheIntervalNeededTillItReached1toExcl10);
                        decimal currGraduationMarkPos_rounded = (decimal)currGraduationMarkPos_shifted_rounded * (decimal)decimalOrderOfMagnitude_markingLowerEndOfGraduationInterval;
                        posOfGraduationMarks_inChartSpace.Add((float)currGraduationMarkPos_rounded);
                    }

                    if (currGraduationMarkPos >= valueMarkingUpperEndOfTheAxis)
                    {
                        break;
                    }
                }

                if (posOfGraduationMarks_inChartSpace.Count > 0)
                {
                    DrawGraduationMarks(graduationInterval_inChartSpaceUnits, durationInSec, hiddenByNearerObjects);
                }
            }
        }

        float maxTextSize_relToGraduationInterval = 0.45f;
        float graduationTextExtent_perpToAxis;
        void DrawGraduationMarks(float graduationInterval_inChartSpaceUnits, float durationInSec, bool hiddenByNearerObjects)
        {
            float lengthOfFullAlphaGraduationLine = 0.03f * length_inWorldSpace;
            Color textColor = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(chart_thisAxisIsPartOf.color, alphaOfGraduationNotation);
            Color color_ofLowAlphaGraduationLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(chart_thisAxisIsPartOf.color, alphaOfGraduationLinesInsideDataArea);
            Quaternion rotation_fromChartRotation_toSkewedText = default;
            float textSize_worldSpace = 0.0f;
            bool textIsDrawn = !UtilitiesDXXL_Math.ApproximatelyZero(alphaOfGraduationNotation);
            float graduationInterval_inWorldSpaceUnits = graduationInterval_inChartSpaceUnits * lengthConversionFactor_fromChartScaling_toWorldScaling;
            bool textDirIsSkewed_inChartsLocalSpace = false;
            graduationTextExtent_perpToAxis = 0.0f;

            if (textIsDrawn)
            {
                if (graduationTypesetting_isManually)
                {
                    textSize_worldSpace = graduationTextSize_relToAxisLength * length_inWorldSpace;
                }
                else
                {
                    textSize_worldSpace = 0.02f * length_inWorldSpace;
                }

                if (cartesianDimension == UtilitiesDXXL_Math.Dimension.y)
                {
                    if (graduationTypesetting_isManually == false)
                    {
                        textSize_worldSpace = Mathf.Min(textSize_worldSpace, maxTextSize_relToGraduationInterval * graduationInterval_inWorldSpaceUnits);
                    }
                }

                int numberOfLetters_ofLongestGraduationText = 0;
                graduationMarksTexts.Clear();
                for (int i = 0; i < posOfGraduationMarks_inChartSpace.Count; i++)
                {
                    graduationMarksTexts.Add("" + posOfGraduationMarks_inChartSpace[i]);
                    numberOfLetters_ofLongestGraduationText = Mathf.Max(numberOfLetters_ofLongestGraduationText, graduationMarksTexts[i].Length);
                }
                float length_ofLongestGraduationText_worldSpace_preFinalTextSizeForce = textSize_worldSpace * numberOfLetters_ofLongestGraduationText;
                graduationTextExtent_perpToAxis = length_ofLongestGraduationText_worldSpace_preFinalTextSizeForce; //used by y-axis. x-axis overwrites below

                if (cartesianDimension == UtilitiesDXXL_Math.Dimension.x)
                {
                    if (graduationTypesetting_isManually)
                    {
                        used_angleDeg_ofAxisGraduationNotationText = angleDeg_ofAxisGraduationNotationText;
                    }
                    else
                    {
                        if (length_ofLongestGraduationText_worldSpace_preFinalTextSizeForce < (0.9f * graduationInterval_inWorldSpaceUnits))
                        {
                            used_angleDeg_ofAxisGraduationNotationText = 0.0f;
                        }
                        else
                        {
                            if (posOfGraduationMarks_inChartSpace.Count <= 18)
                            {
                                used_angleDeg_ofAxisGraduationNotationText = -45.0f;
                                textSize_worldSpace = Mathf.Min(textSize_worldSpace, 0.8f * maxTextSize_relToGraduationInterval * graduationInterval_inWorldSpaceUnits);
                            }
                            else
                            {
                                used_angleDeg_ofAxisGraduationNotationText = -90.0f;
                                textSize_worldSpace = Mathf.Min(textSize_worldSpace, maxTextSize_relToGraduationInterval * graduationInterval_inWorldSpaceUnits);
                            }
                        }
                    }

                    textDirIsSkewed_inChartsLocalSpace = !UtilitiesDXXL_Math.ApproximatelyZero(used_angleDeg_ofAxisGraduationNotationText);
                    if (textDirIsSkewed_inChartsLocalSpace)
                    {
                        rotation_fromChartRotation_toSkewedText = Quaternion.AngleAxis(used_angleDeg_ofAxisGraduationNotationText, Vector3.forward);
                        float length_ofLongestGraduationText_worldSpace_postFinalTextSizeForce = textSize_worldSpace * numberOfLetters_ofLongestGraduationText;
                        float absSine = Mathf.Sin(Mathf.Deg2Rad * Mathf.Abs(used_angleDeg_ofAxisGraduationNotationText));
                        graduationTextExtent_perpToAxis = textSize_worldSpace + length_ofLongestGraduationText_worldSpace_postFinalTextSizeForce * absSine;
                    }
                    else
                    {
                        graduationTextExtent_perpToAxis = textSize_worldSpace * UtilitiesDXXL_Text.relLineDistance;
                    }
                }
            }

            for (int i = 0; i < posOfGraduationMarks_inChartSpace.Count; i++)
            {
                if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
                DrawGraduationMark(i, posOfGraduationMarks_inChartSpace[i], textIsDrawn, textSize_worldSpace, lengthOfFullAlphaGraduationLine, color_ofLowAlphaGraduationLine, textColor, textDirIsSkewed_inChartsLocalSpace, rotation_fromChartRotation_toSkewedText, durationInSec, hiddenByNearerObjects);
            }
        }


        void DrawGraduationMark(int i_graduationMark, float graduationMarkPosition_chartSpace, bool textIsDrawn, float textSize_worldSpace, float lengthOfFullAlphaGraduationLine, Color color_ofLowAlphaGraduationLine, Color textColor, bool textDirIsSkewed_inChartsLocalSpace, Quaternion rotation_fromChartRotation_toSkewedText, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector2 start_ofFullAlphaGraduationLine_chartSpace;
            if (cartesianDimension == UtilitiesDXXL_Math.Dimension.x)
            {
                start_ofFullAlphaGraduationLine_chartSpace = new Vector2(graduationMarkPosition_chartSpace, theOtherAxis.ValueMarkingLowerEndOfTheAxis);
            }
            else
            {
                start_ofFullAlphaGraduationLine_chartSpace = new Vector2(theOtherAxis.ValueMarkingLowerEndOfTheAxis, graduationMarkPosition_chartSpace);
            }

            Vector3 start_ofFullAlphaGraduationLine_worldSpace = chart_thisAxisIsPartOf.ChartSpace_to_WorldSpace(start_ofFullAlphaGraduationLine_chartSpace);
            Vector3 end_ofFullAlphaGraduationLine_worldSpace = start_ofFullAlphaGraduationLine_worldSpace - lengthOfFullAlphaGraduationLine * theOtherAxis.AxisVector_normalized_inWorldSpace;
            Vector3 end_ofLowAlphaGraduationLine_worldSpace = start_ofFullAlphaGraduationLine_worldSpace + theOtherAxis.axisVector_inWorldSpace;

            Line_fadeableAnimSpeed.InternalDraw(start_ofFullAlphaGraduationLine_worldSpace, end_ofFullAlphaGraduationLine_worldSpace, chart_thisAxisIsPartOf.color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            if (alphaOfGraduationLinesInsideDataArea > 0.0f)
            {
                Vector3 start_ofLowAlphaGraduationLine_worldSpace = start_ofFullAlphaGraduationLine_worldSpace;
                Line_fadeableAnimSpeed.InternalDraw(start_ofLowAlphaGraduationLine_worldSpace, end_ofLowAlphaGraduationLine_worldSpace, color_ofLowAlphaGraduationLine, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            if (textIsDrawn)
            {
                DrawTextAtGraduationMark(i_graduationMark, textSize_worldSpace, textColor, textDirIsSkewed_inChartsLocalSpace, end_ofFullAlphaGraduationLine_worldSpace, rotation_fromChartRotation_toSkewedText, durationInSec, hiddenByNearerObjects);
            }
        }

        void DrawTextAtGraduationMark(int i_graduationMark, float textSize_worldSpace, Color textColor, bool textDirIsSkewed_inChartsLocalSpace, Vector3 end_ofFullAlphaGraduationLine_worldSpace, Quaternion rotation_fromChartRotation_toSkewedText, float durationInSec, bool hiddenByNearerObjects)
        {
            string notationText = graduationMarksTexts[i_graduationMark];
            if (cartesianDimension == UtilitiesDXXL_Math.Dimension.x)
            {
                Vector3 textPosition = end_ofFullAlphaGraduationLine_worldSpace;
                Quaternion textRotation;
                DrawText.TextAnchorDXXL textAnchor;
                if (textDirIsSkewed_inChartsLocalSpace)
                {
                    textRotation = chart_thisAxisIsPartOf.InternalRotation * rotation_fromChartRotation_toSkewedText;
                    if (used_angleDeg_ofAxisGraduationNotationText < 0.0f)
                    {
                        //clockwise rotation:
                        textAnchor = DrawText.TextAnchorDXXL.MiddleLeft;
                    }
                    else
                    {
                        //counter clockwise rotation:
                        textAnchor = DrawText.TextAnchorDXXL.MiddleRight;
                    }
                }
                else
                {
                    textRotation = chart_thisAxisIsPartOf.InternalRotation;
                    textAnchor = DrawText.TextAnchorDXXL.UpperCenter;
                }
                UtilitiesDXXL_Text.WriteFramed(notationText, textPosition, textColor, textSize_worldSpace, textRotation, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, chart_thisAxisIsPartOf.autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                Vector3 textPosition = end_ofFullAlphaGraduationLine_worldSpace - 0.01f * length_inWorldSpace * theOtherAxis.AxisVector_normalized_inWorldSpace;
                DrawText.TextAnchorDXXL textAnchor = DrawText.TextAnchorDXXL.MiddleRight;
                UtilitiesDXXL_Text.WriteFramed(notationText, textPosition, textColor, textSize_worldSpace, chart_thisAxisIsPartOf.InternalRotation, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, chart_thisAxisIsPartOf.autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects);
            }
        }

        void DrawChartBoundaryLine_inTheStyleOfAGraduationLine(float durationInSec, bool hiddenByNearerObjects)
        {
            Color color_ofLowAlphaGraduationLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(chart_thisAxisIsPartOf.color, alphaOfGraduationLinesInsideDataArea);
            Vector3 startPos_worldSpace = chart_thisAxisIsPartOf.Position_worldspace + AxisVector_inWorldSpace;
            Vector3 endPos_worldSpace = startPos_worldSpace + theOtherAxis.AxisVector_inWorldSpace;
            Line_fadeableAnimSpeed.InternalDraw(startPos_worldSpace, endPos_worldSpace, color_ofLowAlphaGraduationLine, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
        }

        void DrawAxisName(float durationInSec, bool hiddenByNearerObjects)
        {
            string drawnName = null;
            if (nameHasBeenManuallySet)
            {
                drawnName = name;
            }
            else
            {
                if (cartesianDimension == UtilitiesDXXL_Math.Dimension.x)
                {
                    if (allXValuesCameFromAutomaticSource)
                    {
                        drawnName = GetAxisNameForAutomaticSources();
                    }
                    else
                    {
                        drawnName = "[unknown unit]";
                    }
                }
            }

            if (drawnName != null && drawnName != "")
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(nameText_scaleFactor) == false)
                {
                    Vector3 textPosition_worldSpace;
                    DrawText.TextAnchorDXXL textAnchor;
                    float autoLineBreakWidth = length_inWorldSpace;
                    float textSize = Mathf.Abs(nameText_scaleFactor) * 0.03f * length_inWorldSpace;
                    switch (cartesianDimension)
                    {
                        case UtilitiesDXXL_Math.Dimension.x:
                            textPosition_worldSpace = chart_thisAxisIsPartOf.Position_worldspace + 0.5f * axisVector_inWorldSpace - theOtherAxis.AxisVector_normalized_inWorldSpace * (graduationTextExtent_perpToAxis + 0.05f * length_inWorldSpace);
                            textAnchor = DrawText.TextAnchorDXXL.UpperCenter;
                            UtilitiesDXXL_Text.WriteFramed(drawnName, textPosition_worldSpace, chart_thisAxisIsPartOf.color, textSize, chart_thisAxisIsPartOf.InternalRotation, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, autoLineBreakWidth, chart_thisAxisIsPartOf.autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects);
                            break;
                        case UtilitiesDXXL_Math.Dimension.y:
                            textPosition_worldSpace = chart_thisAxisIsPartOf.Position_worldspace + 0.5f * axisVector_inWorldSpace - theOtherAxis.AxisVector_normalized_inWorldSpace * (graduationTextExtent_perpToAxis + 0.07f * length_inWorldSpace);
                            textAnchor = DrawText.TextAnchorDXXL.LowerCenter;
                            Quaternion rotationLocalInsideChart = Quaternion.AngleAxis(90.0f, Vector3.forward);
                            Quaternion textRotation = chart_thisAxisIsPartOf.InternalRotation * rotationLocalInsideChart;
                            UtilitiesDXXL_Text.WriteFramed(drawnName, textPosition_worldSpace, chart_thisAxisIsPartOf.color, textSize, textRotation, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, autoLineBreakWidth, chart_thisAxisIsPartOf.autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        string GetAxisNameForAutomaticSources()
        {
            switch (sourceOfAutomaticValues)
            {
                case SourceOfAutomaticValues.fixedStepForEachValueAdding:
                    return "data points";
                case SourceOfAutomaticValues.fixedStep_followingTheManualIncrementFunction:
                    return "manually triggered increments";
                case SourceOfAutomaticValues.frameCount:
                    return "frame count (Update)";
                case SourceOfAutomaticValues.fixedTime:
                    return "fixed time [seconds]";
                case SourceOfAutomaticValues.fixedUnscaledTime:
                    return "fixed unscaled time [seconds]";
                case SourceOfAutomaticValues.realtimeSinceStartup:
                    return "realtime since startup [seconds]";
                case SourceOfAutomaticValues.time:
                    return "time [seconds]";
                case SourceOfAutomaticValues.timeSinceLevelLoad:
                    return "time since level load [seconds]";
                case SourceOfAutomaticValues.unscaledTime:
                    return "unscaled time [seconds]";
                case SourceOfAutomaticValues.editorTimeSinceStartup:
                    return "Editor time since startup [seconds]";
                default:
                    return null;
            }
        }

        public void ReportXValueFromNonAutomaticSource()
        {
            allXValuesCameFromAutomaticSource = false;
        }

        public void ChartUpdatesAxisDirectionVectors(Quaternion newChartRotation)
        {
            //Don't call this function manually. Use "chartDrawing.Rotation" and "chartDrawing.Width_inWorldSpace" and "chartDrawing.Height_inWorldSpace" instead.
            axisVector_normalized_inWorldSpace = newChartRotation * unrotated_axisVector_normalized_inWorldSpace;
            axisVector_inWorldSpace = axisVector_normalized_inWorldSpace * length_inWorldSpace;
        }

        public void ChartUpdatesAxisLength(float newLength)
        {
            //Don't call this function manually. Use "chartDrawing.Width_inWorldSpace" and "chartDrawing.Height_inWorldSpace" instead.
            axisVector_inWorldSpace = axisVector_normalized_inWorldSpace * newLength;
            length_inWorldSpace = newLength;
        }

        public bool IsInsideDisplayedSpan(float valueToCheckIfItIsInsideTheDisplayedSpan)
        {
            //same as in "ChartDrawing.ChartSpace_to_WorldSpace"
            //if you want to check both dimensions at once you can use "chartDrawing.IsInsideDrawnChartArea"

            if (valueToCheckIfItIsInsideTheDisplayedSpan >= valueMarkingLowerEndOfTheAxis)
            {
                if (valueToCheckIfItIsInsideTheDisplayedSpan <= valueMarkingUpperEndOfTheAxis)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsLowerThanDisplayedSpan(float valueToCheckIfItIsLowerThanTheDisplayedSpan)
        {
            //same as in "ChartDrawing.ChartSpace_to_WorldSpace"
            return (valueToCheckIfItIsLowerThanTheDisplayedSpan < valueMarkingLowerEndOfTheAxis);
        }

        public bool IsHigherThanDisplayedSpan(float valueToCheckIfItIsHigherThanTheDisplayedSpan)
        {
            //same as in "ChartDrawing.ChartSpace_to_WorldSpace"
            return (valueToCheckIfItIsHigherThanTheDisplayedSpan > valueMarkingUpperEndOfTheAxis);
        }

        static DrawBasics.LineStyle lineStyle_forZeroPositionMarker = DrawBasics.LineStyle.dashed;
        void CreatePointsOfInterest_thatDisplaysTheZeroPositionAsDottedLine(out PointOfInterest created_pointOfInterest_displayingTheZeroPositionLine, out PointOfInterest created_pointOfInterest_displayingTheZeroPositionLine_lowAlphaUnderlay)
        {
            created_pointOfInterest_displayingTheZeroPositionLine = CreatePointOfInterest_thatDisplaysTheZeroPositionAsDottedLine(lineStyle_forZeroPositionMarker, 0.40f);
            created_pointOfInterest_displayingTheZeroPositionLine_lowAlphaUnderlay = CreatePointOfInterest_thatDisplaysTheZeroPositionAsDottedLine(DrawBasics.LineStyle.solid, 0.15f);
        }

        PointOfInterest CreatePointOfInterest_thatDisplaysTheZeroPositionAsDottedLine(DrawBasics.LineStyle lineStyle, float alpha)
        {
            Color color = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(chart_thisAxisIsPartOf.color, alpha);
            PointOfInterest created_pointOfInterest = new PointOfInterest(0.0f, 0.0f, color, chart_thisAxisIsPartOf, null, null);
            created_pointOfInterest.drawTextBoxIfPointIsOutsideOfChartArea = false;
            created_pointOfInterest.isDeletedOnClear = false;
            created_pointOfInterest.forceColorOfParent = true;
            if (cartesianDimension == UtilitiesDXXL_Math.Dimension.x)
            {
                created_pointOfInterest.xValue.lineStyle = lineStyle;
                created_pointOfInterest.xValue.linestylePatternScaleFactor = 0.75f;
                created_pointOfInterest.xValue.drawCoordinateAsText = true;
                created_pointOfInterest.xValue.lineExtent = DimensionOf_PointOfInterest.LineExtent.throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart;

                created_pointOfInterest.yValue.lineStyle = DrawBasics.LineStyle.invisible;
            }
            else
            {
                created_pointOfInterest.yValue.lineStyle = lineStyle;
                created_pointOfInterest.yValue.linestylePatternScaleFactor = 0.75f;
                created_pointOfInterest.yValue.drawCoordinateAsText = true;
                created_pointOfInterest.yValue.lineExtent = DimensionOf_PointOfInterest.LineExtent.throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart;

                created_pointOfInterest.xValue.lineStyle = DrawBasics.LineStyle.invisible;
            }
            return created_pointOfInterest;
        }

        void SetIf_pointOfInterestDisplayingTheZeroPositionLine_isDisplayed()
        {
            if (cartesianDimension == UtilitiesDXXL_Math.Dimension.x)
            {
                if (drawZeroPositionAsDottedLine)
                {
                    pointOfInterest_displayingTheZeroPositionLine.xValue.lineStyle = lineStyle_forZeroPositionMarker;
                    pointOfInterest_displayingTheZeroPositionLine_lowAlphaUnderlay.xValue.lineStyle = DrawBasics.LineStyle.solid;
                }
                else
                {
                    pointOfInterest_displayingTheZeroPositionLine.xValue.lineStyle = DrawBasics.LineStyle.invisible;
                    pointOfInterest_displayingTheZeroPositionLine_lowAlphaUnderlay.xValue.lineStyle = DrawBasics.LineStyle.invisible;
                }
            }
            else
            {
                if (drawZeroPositionAsDottedLine)
                {
                    pointOfInterest_displayingTheZeroPositionLine.yValue.lineStyle = lineStyle_forZeroPositionMarker;
                    pointOfInterest_displayingTheZeroPositionLine_lowAlphaUnderlay.yValue.lineStyle = DrawBasics.LineStyle.solid;
                }
                else
                {
                    pointOfInterest_displayingTheZeroPositionLine.yValue.lineStyle = DrawBasics.LineStyle.invisible;
                    pointOfInterest_displayingTheZeroPositionLine_lowAlphaUnderlay.yValue.lineStyle = DrawBasics.LineStyle.invisible;
                }
            }
        }

        public void SetAxisScalingDuringInspectionComponentPhases(float fixedLowerEndValueOfScale, float fixedUpperEndValueOfScale)
        {
            if (UtilitiesDXXL_Math.FloatIsValid(fixedLowerEndValueOfScale))
            {
                fixedLowerEndValueOfScale_duringInspectionComponentPhases = fixedLowerEndValueOfScale;
            }
            else
            {
                fixedLowerEndValueOfScale_duringInspectionComponentPhases = -100.0f;
            }

            if (UtilitiesDXXL_Math.FloatIsValid(fixedUpperEndValueOfScale))
            {
                fixedUpperEndValueOfScale_duringInspectionComponentPhases = fixedUpperEndValueOfScale;
            }
            else
            {
                fixedUpperEndValueOfScale_duringInspectionComponentPhases = 100.0f;
            }

            RecalcScaling();
        }

        public void GetAxisEndsIfAllNonHiddenValuesAreEncapsulated(out float lowEnd, out float upperEnd)
        {
            float lowestValueOfAllLines = GetLowestValueOfAllLines();
            float highestValueOfAllLines = GetHighestValueOfAllLines();
            GetAxisEndsIfAllValuesAreEncapsulated_inclPadding(out lowEnd, out upperEnd, lowestValueOfAllLines, highestValueOfAllLines);
        }

        public void GetAxisEndsIfAllNonHiddenValuesInsideOtherAxisSpanAreEncapsulated(out float lowEnd, out float upperEnd)
        {
            float lowestValueOfAllLines_insideSpanOfOtherAxis = GetLowestValueOfAllLines_insideSpanOfOtherAxis();
            float highestValueOfAllLines_insideSpanOfOtherAxis = GetHighestValueOfAllLines_insideSpanOfOtherAxis();

            if (float.IsInfinity(lowestValueOfAllLines_insideSpanOfOtherAxis) || float.IsInfinity(highestValueOfAllLines_insideSpanOfOtherAxis))
            {
                //-> no value has been found inside the restricted axis span:
                lowestValueOfAllLines_insideSpanOfOtherAxis = 0.0f;
                highestValueOfAllLines_insideSpanOfOtherAxis = 0.0f;
            }

            GetAxisEndsIfAllValuesAreEncapsulated_inclPadding(out lowEnd, out upperEnd, lowestValueOfAllLines_insideSpanOfOtherAxis, highestValueOfAllLines_insideSpanOfOtherAxis);
        }

        public void GetAxisEndsIfAllValuesAreEncapsulated_inclPadding(out float lowEnd, out float upperEnd, float lowestDisplayedValue, float highestDisplayedValue)
        {
            if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(lowestDisplayedValue, highestDisplayedValue))
            {
                float paddingAtEachSide = 1.6f;
                lowEnd = lowestDisplayedValue - paddingAtEachSide;
                upperEnd = lowestDisplayedValue + paddingAtEachSide;
            }
            else
            {
                float span_fromLowestToHighest = highestDisplayedValue - lowestDisplayedValue;
                float additionalPadding_atEachside = span_fromLowestToHighest * 0.05f;
                lowEnd = lowestDisplayedValue - additionalPadding_atEachside;
                upperEnd = highestDisplayedValue + additionalPadding_atEachside;
            }
        }

    }

}
