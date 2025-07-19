namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class UtilitiesDXXL_LineStyles
    {
        public static bool logWarningToConsole_forTooSmallPatternScaleFactor = true;
        public static float minStylePatternScaleFactor = 0.01f;

        public static float sineLineAmplitude = 0.015f;
        public static float spiralLineAmplitude = 0.015f;
        public static float zigZagLineAmplitude = 0.02f;
        public static float rhombusLineAmplitude = 0.025f;
        public static float doubleRhombusLineAmplitude = 0.03f;
        public static float electricNoiseLineAmplitude = 0.1f;
        public static float electricImpulseLineAmplitude = 0.05f;
        public static float impulseDistance_ofElectricImpulseLines = 0.175f;
        public static float impulseSqueeze_ofElectricImpulseLines = 0.5f;
        public static float freeHandLineAmplitude = 0.05f;

        //lineStyle: arrows
        public static bool default_pointersDirAlongAnimationDir = true;
        public static bool curr_pointersDirAlongAnimationDir = default_pointersDirAlongAnimationDir;
        public static float default_dashLength_forArrowLine = 0.085f;
        public static float default_minRatio_for_dashLengthToLineWidth_forArrowLine = 24.0f;
        public static float default_spaceToDash_ratio_forArrowLine = 1.8f;
        public static float default_minEmptySpacesLength_forArrowLine = 0.105f;
        public static float curr_dashLength_forArrowLine = default_dashLength_forArrowLine;
        public static float curr_minRatio_for_dashLengthToLineWidth_forArrowLine = default_minRatio_for_dashLengthToLineWidth_forArrowLine;
        public static float curr_spaceToDash_ratio_forArrowLine = default_spaceToDash_ratio_forArrowLine;
        public static float curr_minEmptySpacesLength_forArrowLine = default_minEmptySpacesLength_forArrowLine;
        //lineStyle: alternatingColorStripes
        public static float default_dashLength_forAlternatingColorStripesLine = 0.04f;
        public static float curr_dashLength_forAlternatingColorStripesLine = default_dashLength_forAlternatingColorStripesLine;

        //saving GC.Alloc:
        public static List<Vector3> s_listOfSubLines = new List<Vector3>();
        static List<Vector3> s_addedRangeForListOfSubLines = new List<Vector3>();
        static List<float> s_subLineAnchor_distanceToStart = new List<float>();
        static List<Vector3> s_subLinePoints_projectionOntoStraightMainLine = new List<Vector3>();
        static List<Vector3> s_vectors_fromProjectionOntoMainLine_toSubLinePoints = new List<Vector3>();
        static List<Vector3> s_listOfSubLinesForZigZagsUpSegments = new List<Vector3>();
        static List<Vector3> s_listOfSubLinesForZigZagsDownSegments = new List<Vector3>();
        static List<Vector3> s_listOfSubLinesForZigZagsLeftSegments = new List<Vector3>();
        static List<Vector3> s_listOfSubLinesForZigZagsRightSegments = new List<Vector3>();
        static List<Vector3> s_listOfSubLinesForZigZagsUpAndDownSegments = new List<Vector3>();
        static List<Vector3> s_listOfSubLinesForZigZagsLeftAndRightSegments = new List<Vector3>();

        public static int RefillListOfSubLines(Vector3 start_unswapped, Vector3 end_unswapped, DrawBasics.LineStyle style, float stylePatternScaleFactor, float lineWidth, out float amplitude, Vector3 amplitudeUp_normalized, float animationSpeed, ref LineAnimationProgress lineAnimationProgressToUpdate, bool skipPatternEnlargementForLongLines, bool skipPatternEnlargementForShortLines, float tensionFactor)
        {
            //function returns "usedSlotsInListOfSubLines"
            //Vectors at i=0 and i=1 define the first subLine, at i=2 and i=3 the second subLine, and so on...

            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { amplitude = 0.0f; return 0; }
            Vector3 start;
            Vector3 end;
            if (animationSpeed < 0.0f)
            {
                //swap start/end, because animation is only fit for moving towards end
                //though this has the disadvantage of an uncontinuous animation jump in the moment when the animationSpeed changes it's sign
                start = end_unswapped;
                end = start_unswapped;
                animationSpeed = -animationSpeed;
            }
            else
            {
                start = start_unswapped;
                end = end_unswapped;
            }

            Vector3 startToEnd = end - start;
            tensionFactor = tensionFactor * DrawBasics.StylePatternScaleFactor_alongLineDir_ignoringAmplitude;
            stylePatternScaleFactor = AdjustPatternScaleFactor(stylePatternScaleFactor, startToEnd, skipPatternEnlargementForLongLines);
            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(start, end)) //-> callers already ensure this, too
            {
                SetLineAnimProgressToZero(ref lineAnimationProgressToUpdate);
                amplitude = 0.0f;
                return 0;
            }

            if (style == DrawBasics.LineStyle.solid)
            {
                UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, start, 0);
                UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, end, 1);
                SetLineAnimProgressToZero(ref lineAnimationProgressToUpdate);
                amplitude = 0.0f;
                return 2; //immediate return because the solid linetype acts as fallback -> This prevents the function from potentially calling itself recursively.
            }

            if (style == DrawBasics.LineStyle.invisible)
            {
                SetLineAnimProgressToZero(ref lineAnimationProgressToUpdate);
                amplitude = 0.0f;
                return 0;
            }

            if (style == DrawBasics.LineStyle.dotted)
            {
                amplitude = 0.0f;
                return GetListOfSubLines_forOneDashLine(0.0015f, 1.0f, 4.0f, 0.02f, stylePatternScaleFactor, skipPatternEnlargementForShortLines, start, end, lineWidth, animationSpeed, ref lineAnimationProgressToUpdate, tensionFactor);
            }

            if (style == DrawBasics.LineStyle.dottedDense)
            {
                amplitude = 0.0f;
                return GetListOfSubLines_forOneDashLine(0.0015f, 1.0f, 2.0f, 0.01f, stylePatternScaleFactor, skipPatternEnlargementForShortLines, start, end, lineWidth, animationSpeed, ref lineAnimationProgressToUpdate, tensionFactor);
            }

            if (style == DrawBasics.LineStyle.dottedWide)
            {
                amplitude = 0.0f;
                return GetListOfSubLines_forOneDashLine(0.0015f, 1.0f, 10.0f, 0.06f, stylePatternScaleFactor, skipPatternEnlargementForShortLines, start, end, lineWidth, animationSpeed, ref lineAnimationProgressToUpdate, tensionFactor);
            }

            if (style == DrawBasics.LineStyle.dashed)
            {
                amplitude = 0.0f;
                return GetListOfSubLines_forOneDashLine(0.01f, 7.0f, 1.0f, 0.02f, stylePatternScaleFactor, skipPatternEnlargementForShortLines, start, end, lineWidth, animationSpeed, ref lineAnimationProgressToUpdate, tensionFactor);
            }

            if (style == DrawBasics.LineStyle.dashedLong)
            {
                amplitude = 0.0f;
                return GetListOfSubLines_forOneDashLine(0.04f, 12.0f, 0.4f, 0.02f, stylePatternScaleFactor, skipPatternEnlargementForShortLines, start, end, lineWidth, animationSpeed, ref lineAnimationProgressToUpdate, tensionFactor);
            }

            if (style == DrawBasics.LineStyle.arrows)
            {
                amplitude = 0.0f;
                return GetListOfSubLines_forOneDashLine(curr_dashLength_forArrowLine, curr_minRatio_for_dashLengthToLineWidth_forArrowLine, curr_spaceToDash_ratio_forArrowLine, curr_minEmptySpacesLength_forArrowLine, stylePatternScaleFactor, skipPatternEnlargementForShortLines, start, end, lineWidth, animationSpeed, ref lineAnimationProgressToUpdate, tensionFactor);
            }

            if (style == DrawBasics.LineStyle.alternatingColorStripes)
            {
                amplitude = 0.0f;
                return GetListOfSubLines_forOneDashLine(curr_dashLength_forAlternatingColorStripesLine, 12.0f, 1.0f, curr_dashLength_forAlternatingColorStripesLine, stylePatternScaleFactor, skipPatternEnlargementForShortLines, start, end, lineWidth, animationSpeed, ref lineAnimationProgressToUpdate, tensionFactor);
            }

            if (style == DrawBasics.LineStyle.dotDash)
            {
                amplitude = 0.0f;
                return GetListOfSubLines_forTwoDashLine(0.0015f * stylePatternScaleFactor, true, 0.005f * stylePatternScaleFactor, 5.0f, 0.7f, 0.02f, skipPatternEnlargementForShortLines, start, end, lineWidth, animationSpeed, ref lineAnimationProgressToUpdate, tensionFactor);
            }

            if (style == DrawBasics.LineStyle.dotDashLong)
            {
                amplitude = 0.0f;
                return GetListOfSubLines_forTwoDashLine(0.0015f * stylePatternScaleFactor, true, 0.015f * stylePatternScaleFactor, 15.0f, 0.23f, 0.02f, skipPatternEnlargementForShortLines, start, end, lineWidth, animationSpeed, ref lineAnimationProgressToUpdate, tensionFactor);
            }

            if (style == DrawBasics.LineStyle.twoDash)
            {
                amplitude = 0.0f;
                return GetListOfSubLines_forTwoDashLine(0.008f * stylePatternScaleFactor, false, 0.018f * stylePatternScaleFactor, 18.0f, 0.23f, 0.02f, skipPatternEnlargementForShortLines, start, end, lineWidth, animationSpeed, ref lineAnimationProgressToUpdate, tensionFactor);
            }

            if (style == DrawBasics.LineStyle.disconnectedAnchors)
            {
                amplitude = 0.0f;
                return GetListOfSubLines_forDisconnectedAnchorLines(start, end, startToEnd, ref lineAnimationProgressToUpdate, animationSpeed, tensionFactor);
            }

            if (style == DrawBasics.LineStyle.spiral)
            {
                amplitude = spiralLineAmplitude * stylePatternScaleFactor;
                return GetListOfSubLines_forSpiral(amplitude, amplitude, start, end, amplitudeUp_normalized, animationSpeed, ref lineAnimationProgressToUpdate, tensionFactor);
            }

            if (style == DrawBasics.LineStyle.sine)
            {
                amplitude = sineLineAmplitude * stylePatternScaleFactor;
                return GetListOfSubLines_forSpiral(amplitude, 0.0f, start, end, amplitudeUp_normalized, animationSpeed, ref lineAnimationProgressToUpdate, tensionFactor);
            }

            if (style == DrawBasics.LineStyle.zigzag)
            {
                float animationProgress_inSegments;
                Vector3 normal_fromLinePerpToMainLinePoints;
                Vector3 normal_fromLinePerpToPerpLinePoints;
                amplitude = zigZagLineAmplitude * stylePatternScaleFactor;
                return GetListOfSubLines_forZigZagTypeLines(ref s_listOfSubLines, DrawBasics.LineStyle.zigzag, start, end, amplitude, amplitudeUp_normalized, tensionFactor, animationSpeed, out animationProgress_inSegments, out normal_fromLinePerpToMainLinePoints, out normal_fromLinePerpToPerpLinePoints, ref lineAnimationProgressToUpdate, false, out int i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine, out int i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
            }

            if (style == DrawBasics.LineStyle.rhombus)
            {
                float animationProgress_inSegments;
                Vector3 normal_fromLinePerpToMainLinePoints;
                Vector3 normal_fromLinePerpToPerpLinePoints;
                amplitude = rhombusLineAmplitude * stylePatternScaleFactor;
                return GetListOfSubLines_forZigZagTypeLines(ref s_listOfSubLines, DrawBasics.LineStyle.rhombus, start, end, amplitude, amplitudeUp_normalized, tensionFactor, animationSpeed, out animationProgress_inSegments, out normal_fromLinePerpToMainLinePoints, out normal_fromLinePerpToPerpLinePoints, ref lineAnimationProgressToUpdate, false, out int i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine, out int i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
            }

            if (style == DrawBasics.LineStyle.doubleRhombus)
            {
                float animationProgress_inSegments;
                Vector3 normal_fromLinePerpToMainLinePoints;
                Vector3 normal_fromLinePerpToPerpLinePoints;
                amplitude = doubleRhombusLineAmplitude * stylePatternScaleFactor;
                return GetListOfSubLines_forZigZagTypeLines(ref s_listOfSubLines, DrawBasics.LineStyle.doubleRhombus, start, end, amplitude, amplitudeUp_normalized, tensionFactor, animationSpeed, out animationProgress_inSegments, out normal_fromLinePerpToMainLinePoints, out normal_fromLinePerpToPerpLinePoints, ref lineAnimationProgressToUpdate, false, out int i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine, out int i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
            }

            if (style == DrawBasics.LineStyle.electricNoise)
            {
                return GetListOfSubLines_forElectricNoiseLines(start, end, stylePatternScaleFactor, lineWidth, out amplitude, ref lineAnimationProgressToUpdate, amplitudeUp_normalized, animationSpeed, tensionFactor);
            }

            if (style == DrawBasics.LineStyle.electricImpulses)
            {
                return GetListOfSubLines_forElectricImpulseLines(start, end, stylePatternScaleFactor, lineWidth, out amplitude, ref lineAnimationProgressToUpdate, amplitudeUp_normalized, animationSpeed, tensionFactor);
            }

            if (style == DrawBasics.LineStyle.freeHand2D)
            {
                amplitude = freeHandLineAmplitude * stylePatternScaleFactor;
                int usedSlotsInListOfSubLines = GetListOfSubLines_forFreehandTypeLines(DrawBasics.LineStyle.freeHand2D, start, end, amplitude, amplitudeUp_normalized, animationSpeed, ref lineAnimationProgressToUpdate, tensionFactor);
                amplitude = 0.6f * amplitude; //for text distance to line
                return usedSlotsInListOfSubLines;
            }

            if (style == DrawBasics.LineStyle.freeHand3D)
            {
                amplitude = freeHandLineAmplitude * stylePatternScaleFactor;
                int usedSlotsInListOfSubLines = GetListOfSubLines_forFreehandTypeLines(DrawBasics.LineStyle.freeHand3D, start, end, amplitude, amplitudeUp_normalized, animationSpeed, ref lineAnimationProgressToUpdate, tensionFactor);
                amplitude = 0.6f * amplitude; //for text distance to line
                return usedSlotsInListOfSubLines;
            }

            Debug.LogError("Line style " + style + " not implemented yet. Now returning emptyLine as fallback.");
            SetLineAnimProgressToZero(ref lineAnimationProgressToUpdate);
            amplitude = 0.0f;
            return 0;
        }

        static float AdjustPatternScaleFactor(float patternScaleFactor_toAdjust, Vector3 startToEnd, bool skipPatternEnlargementForLongLines)
        {
            if (patternScaleFactor_toAdjust < minStylePatternScaleFactor)
            {
                if (logWarningToConsole_forTooSmallPatternScaleFactor)
                {
                    Debug.LogWarning("stylePatternScaleFactor (" + patternScaleFactor_toAdjust + ") must be bigger than minStylePatternScaleFactor (" + minStylePatternScaleFactor + ") and has been uprounded.");
                }
                patternScaleFactor_toAdjust = minStylePatternScaleFactor;
            }

            //Restrict costly patternRecursion for long lines:
            if (skipPatternEnlargementForLongLines == false)
            {
                float lineLengthSqr = startToEnd.sqrMagnitude;
                float lineLengthSqr_aboveWhichToAutoEnlargeThePattern = DrawBasics.LineLength_aboveWhichToAutoEnlargeThePattern * DrawBasics.LineLength_aboveWhichToAutoEnlargeThePattern; //-> don't calculate on startUp, since "Draw.lineLength_aboveWhichToAutoEnlargeThePattern" can be changed during runtime
                if (lineLengthSqr > lineLengthSqr_aboveWhichToAutoEnlargeThePattern)
                {
                    float lineLength = Mathf.Sqrt(lineLengthSqr);
                    float patternAdjustFactor_ifPatternScaleFactorIs1 = lineLength * (1.0f / DrawBasics.LineLength_aboveWhichToAutoEnlargeThePattern); //-> patternAdjustFactor_ifPatternScaleFactorWas1 is always bigger than 1
                    if (DrawBasics.autoEnlargeBigPatternsLater_whichDistortsPatternSizeRatios)
                    {
                        if (patternAdjustFactor_ifPatternScaleFactorIs1 > patternScaleFactor_toAdjust)
                        {
                            float finalPatternAdjustFactor = patternAdjustFactor_ifPatternScaleFactorIs1 / Mathf.Max(patternScaleFactor_toAdjust, 1.0f);
                            patternScaleFactor_toAdjust = patternScaleFactor_toAdjust * finalPatternAdjustFactor;
                        }
                    }
                    else
                    {
                        patternScaleFactor_toAdjust = patternScaleFactor_toAdjust * patternAdjustFactor_ifPatternScaleFactorIs1;
                    }
                }
            }

            return patternScaleFactor_toAdjust;
        }

        static int GetListOfSubLines_forOneDashLine(float dashLength, float minRatio_for_dashLengthToLineWidth, float spaceToDash_ratio, float minEmptySpacesLength, float stylePatternScaleFactor, bool skipPatternEnlargementForShortLines, Vector3 start, Vector3 end, float lineWidth, float animationSpeed, ref LineAnimationProgress lineAnimationProgressToUpdate, float tensionFactor)
        {
            //function returns "usedSlotsInListOfSubLines"
            int i_nextFreeSlot = 0;

            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(start, end))
            {
                SetLineAnimProgressToZero(ref lineAnimationProgressToUpdate);
                return 0;
            }

            dashLength = EnlargeOneDashLengthForThickLines(dashLength, minRatio_for_dashLengthToLineWidth, lineWidth, skipPatternEnlargementForShortLines, stylePatternScaleFactor);
            
            float emptySpacesLength = dashLength * spaceToDash_ratio;
            if (skipPatternEnlargementForShortLines == false)
            {
                emptySpacesLength = Mathf.Max(emptySpacesLength, minEmptySpacesLength);
            }

            dashLength = dashLength * stylePatternScaleFactor;
            emptySpacesLength = emptySpacesLength * stylePatternScaleFactor;

            dashLength = dashLength * tensionFactor;
            emptySpacesLength = emptySpacesLength * tensionFactor;

            dashLength = Mathf.Max(dashLength, 0.0003f);
            emptySpacesLength = Mathf.Max(emptySpacesLength, 0.0003f);

            float animationLoopLength = dashLength + emptySpacesLength;
            Vector3 startToEnd = end - start;
            Vector3 lineNormalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(startToEnd, out float lineLength);

            //first dash with fixed position:
            i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, start, i_nextFreeSlot);
            if (dashLength > lineLength)
            {
                i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, end, i_nextFreeSlot);
                SetLineAnimProgressToZero(ref lineAnimationProgressToUpdate);
                return i_nextFreeSlot;
            }
            else
            {
                i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, start + lineNormalized * dashLength, i_nextFreeSlot);
            }

            //other dashes with animated positions:
            float animationProgress = GetAnimationProgess_forDashLine(animationSpeed, lineAnimationProgressToUpdate, animationLoopLength);
            GetCurrLineAnimationProgress(ref lineAnimationProgressToUpdate, animationProgress);

            float animationProgress_as0to1 = UtilitiesDXXL_Math.Loop_floatIntoSpanFrom_m1_to_p1(animationProgress);
            float steppedDistance_markingStartOfAnimatedLine = animationLoopLength * animationProgress_as0to1;
            Vector3 currPos = start + lineNormalized * steppedDistance_markingStartOfAnimatedLine;
            float alreadyStepped = steppedDistance_markingStartOfAnimatedLine;
            int loopIterationCounter = 0;  

            if (alreadyStepped > lineLength)
            {
                //lineEnd reached during first empty phase
                i_nextFreeSlot = Add_endingDash_toSubLineAnchorList(i_nextFreeSlot, end, lineNormalized, dashLength);
                return i_nextFreeSlot;
            }

            while (alreadyStepped < lineLength)
            {
                //Add dash startPos:
                i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, currPos, i_nextFreeSlot);
                currPos = currPos + lineNormalized * dashLength;
                alreadyStepped = alreadyStepped + dashLength;

                if (alreadyStepped > lineLength)
                {
                    //lineEnd reached during current dash phase
                    i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, end, i_nextFreeSlot);
                    i_nextFreeSlot = Add_endingDash_toSubLineAnchorList(i_nextFreeSlot, end, lineNormalized, dashLength);
                    break;
                }
                else
                {
                    //Add dash endPos:
                    i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, currPos, i_nextFreeSlot);
                    currPos = currPos + lineNormalized * emptySpacesLength;
                    alreadyStepped = alreadyStepped + emptySpacesLength;
                    if (alreadyStepped >= lineLength) //the "=" in ">=" is important to be continous with the "<" from the while-condition (otherwise (in edgeCases) the last dash will be lost (because: if the empty phase ends exactly at line end, then the upcoming loop will not start and then no "Add_endingDash_toSubLineAnchorList" is called))
                    {
                        //lineEnd reached during current empty phase
                        i_nextFreeSlot = Add_endingDash_toSubLineAnchorList(i_nextFreeSlot, end, lineNormalized, dashLength);
                        break;
                    }
                }
                loopIterationCounter++;
                if (loopIterationCounter > 100000)
                {
                    Debug.LogError("Too many while loop iterations. Forced quit to prevent freeze.   dashLength: " + dashLength + "   emptySpacesLength: " + emptySpacesLength);
                    break;
                }
            }

            return i_nextFreeSlot;
        }

        static int GetListOfSubLines_forTwoDashLine(float shortDashLength, bool treatShortDash_asDot, float longDashLength, float minRatio_for_longDashLengthToLineWidth, float spacesToLongDashs_ratio, float minEmptySpacesLength, bool skipPatternEnlargementForShortLines, Vector3 start, Vector3 end, float lineWidth, float animationSpeed, ref LineAnimationProgress lineAnimationProgressToUpdate, float tensionFactor)
        {
            //function returns "usedSlotsInListOfSubLines"
            int i_nextFreeSlot = 0;

            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(start, end))
            {
                SetLineAnimProgressToZero(ref lineAnimationProgressToUpdate);
                return 0;
            }

            bool dashLengthAdjustion_stoppedWithError = AdjustTwoDashLengthForThickLines(ref shortDashLength, ref longDashLength, treatShortDash_asDot, minRatio_for_longDashLengthToLineWidth, lineWidth, skipPatternEnlargementForShortLines);
            if (dashLengthAdjustion_stoppedWithError)
            {
                SetLineAnimProgressToZero(ref lineAnimationProgressToUpdate);
                return 0;
            }
            float emptySpacesLength = longDashLength * spacesToLongDashs_ratio;
            if (skipPatternEnlargementForShortLines == false)
            {
                emptySpacesLength = Mathf.Max(emptySpacesLength, minEmptySpacesLength);
            }

            shortDashLength = shortDashLength * tensionFactor;
            longDashLength = longDashLength * tensionFactor;
            emptySpacesLength = emptySpacesLength * tensionFactor;

            shortDashLength = Mathf.Max(shortDashLength, 0.0003f);
            longDashLength = Mathf.Max(longDashLength, 0.0003f);
            emptySpacesLength = Mathf.Max(emptySpacesLength, 0.0003f);

            Vector3 startToEnd = end - start;
            float animationLoopLength = longDashLength + emptySpacesLength + shortDashLength + emptySpacesLength;

            Vector3 lineNormalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(startToEnd, out float lineLength);
            float animationProgress = GetAnimationProgess_forDashLine(animationSpeed, lineAnimationProgressToUpdate, animationLoopLength);
            GetCurrLineAnimationProgress(ref lineAnimationProgressToUpdate, animationProgress);

            //first longDash with fixed position:
            i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, start, i_nextFreeSlot);
            if (longDashLength > lineLength)
            {
                i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, end, i_nextFreeSlot);
                return i_nextFreeSlot;
            }
            else
            {
                i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, start + lineNormalized * longDashLength, i_nextFreeSlot);
            }

            if (animationLoopLength > lineLength)
            {
                //lineEnd reached inside animationLoopSpan
                i_nextFreeSlot = Add_endingDash_toSubLineAnchorList(i_nextFreeSlot, end, lineNormalized, longDashLength);
                return i_nextFreeSlot;
            }

            float animationProgress_as0to1 = UtilitiesDXXL_Math.Loop_floatIntoSpanFrom_m1_to_p1(animationProgress);
            float steppedDistance_markingStartOfAnimatedLine = animationLoopLength * animationProgress_as0to1;

            //first shortDash that fills the animationGenerated hole:
            if (steppedDistance_markingStartOfAnimatedLine > (longDashLength + emptySpacesLength))
            {
                i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, start + lineNormalized * (steppedDistance_markingStartOfAnimatedLine - emptySpacesLength - shortDashLength), i_nextFreeSlot);
                i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, start + lineNormalized * (steppedDistance_markingStartOfAnimatedLine - emptySpacesLength), i_nextFreeSlot);
            }

            //other dashes with animated positions:
            Vector3 currPos = start + lineNormalized * steppedDistance_markingStartOfAnimatedLine;
            float alreadyStepped = steppedDistance_markingStartOfAnimatedLine;
            int loopIterationCounter = 0;
            while (alreadyStepped < lineLength)
            {
                //Add longDash startPos:
                i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, currPos, i_nextFreeSlot);
                currPos = currPos + lineNormalized * longDashLength;
                alreadyStepped = alreadyStepped + longDashLength;

                if (alreadyStepped > lineLength)
                {
                    //lineEnd reached during current longDash phase
                    i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, end, i_nextFreeSlot);//end current longDashLinePhase
                    i_nextFreeSlot = Add_endingDash_toSubLineAnchorList(i_nextFreeSlot, end, lineNormalized, longDashLength);
                    break;
                }
                else
                {
                    //Add longDash endPos:
                    i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, currPos, i_nextFreeSlot);
                    currPos = currPos + lineNormalized * emptySpacesLength;
                    alreadyStepped = alreadyStepped + emptySpacesLength;

                    if (alreadyStepped >= lineLength) //the "=" in ">=": Is probably not critical here, because it's not the last one of these "(alreadyStepped >= lineLength)"-checks inside the while loop
                    {
                        //lineEnd reached during current empty phase after longDash phase
                        i_nextFreeSlot = Add_endingDash_toSubLineAnchorList(i_nextFreeSlot, end, lineNormalized, longDashLength);
                        break;
                    }

                    //Add shortDash startPos:
                    i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, currPos, i_nextFreeSlot);
                    currPos = currPos + lineNormalized * shortDashLength;
                    alreadyStepped = alreadyStepped + shortDashLength;

                    if (alreadyStepped >= lineLength) //the "=" in ">=": Is probably not critical here, because it's not the last one of these "(alreadyStepped >= lineLength)"-checks inside the while loop
                    {
                        //lineEnd reached during current shortDash phase
                        i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, end, i_nextFreeSlot);//end current shortDashLinePhase
                        i_nextFreeSlot = Add_endingDash_toSubLineAnchorList(i_nextFreeSlot, end, lineNormalized, longDashLength);
                        break;
                    }

                    //Add shortDash endPos:
                    i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, currPos, i_nextFreeSlot);
                    currPos = currPos + lineNormalized * emptySpacesLength;
                    alreadyStepped = alreadyStepped + emptySpacesLength;

                    if (alreadyStepped >= lineLength) //the "=" in ">=" is important to be continous with the "<" from the while-condition (otherwise (in edgeCases) the last dash will be lost (because: if the empty phase ends exactly at line end, then the upcoming loop will not start and then no "Add_endingDash_toSubLineAnchorList" is called))
                    {
                        //lineEnd reached during current empty phase after shortDash phase
                        i_nextFreeSlot = Add_endingDash_toSubLineAnchorList(i_nextFreeSlot, end, lineNormalized, longDashLength);
                        break;
                    }
                }

                loopIterationCounter++;
                if (loopIterationCounter > 100000)
                {
                    Debug.LogError("Too many while loop iterations. Forced quit to prevent freeze.   longDashLength: " + longDashLength + "   shortDashLength: " + shortDashLength + "   emptySpacesLength: " + emptySpacesLength);
                    break;
                }
            }
            return i_nextFreeSlot;
        }

        static float EnlargeOneDashLengthForThickLines(float dashLength, float minRatio_for_dashLengthToLineWidth, float lineWidth, bool skipPatternEnlargementForShortLines, float stylePatternScaleFactor)
        {
            if (skipPatternEnlargementForShortLines == false)
            {
                if (lineWidth > 0.0f)
                {
                    //minRatio_for_dashLengthToLineWidth: Use "1.0f" for drawing dots, and higher values for drawing dashes.
                    minRatio_for_dashLengthToLineWidth = Mathf.Max(minRatio_for_dashLengthToLineWidth, 1.0f);
                    //float minDashLength_accordingToLineWidth = lineWidth * minRatio_for_dashLengthToLineWidth;
                    float minDashLength_accordingToLineWidth = lineWidth * minRatio_for_dashLengthToLineWidth / stylePatternScaleFactor; //"stylePatternScaleFactor" here recompensates an increasing "lineWidth". This fixes the problem that dashed lineStyles change their dashSize when changing the "nearClipPlane" of a camera on which a "DrawScreenspace" call is executed. In such cases the "lineWidth" has already been increased to fit the new screen dimension. This also causes the restriction of "stylePatternScaleFactor" as mentioned in the documentation, where it hasn't any effect below a dynamic limit.
                    dashLength = Mathf.Max(dashLength, minDashLength_accordingToLineWidth);
                }
            }
            return dashLength;
        }

        static bool AdjustTwoDashLengthForThickLines(ref float shortDashLength, ref float longDashLength, bool treatShortDash_asDot, float minRatio_for_longDashLengthToLineWidth, float lineWidth, bool skipPatternEnlargementForShortLines)
        {
            bool stoppedWithError = false;
            if (skipPatternEnlargementForShortLines == false)
            {
                if (lineWidth > 0.0f)
                {
                    //adjust dash length for thickLines:
                    float shortDashToLongDash_ratio = shortDashLength / longDashLength;
                    if (minRatio_for_longDashLengthToLineWidth > 1.0f)
                    {
                        float minLongDashLength_accordingToLineWidth = lineWidth * minRatio_for_longDashLengthToLineWidth;
                        longDashLength = Mathf.Max(longDashLength, minLongDashLength_accordingToLineWidth);
                    }
                    else
                    {
                        Debug.LogError("minRatio_for_longDashLengthToLineWidth (" + minRatio_for_longDashLengthToLineWidth + ") must be bigger than 1.0f, because the longDash is not supported to be treated as a dot.");
                        stoppedWithError = true;
                        return stoppedWithError;
                    }

                    shortDashLength = longDashLength * shortDashToLongDash_ratio;
                    if (shortDashLength < lineWidth || treatShortDash_asDot)
                    {
                        shortDashLength = lineWidth;
                    }
                }
            }

            return stoppedWithError;
        }

        static int Add_endingDash_toSubLineAnchorList(int i_nextFreeSlot, Vector3 end, Vector3 lineNormalized, float dashLength)
        {
            //function returns "i_nextFreeSlot"
            i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, end - lineNormalized * dashLength, i_nextFreeSlot);
            i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, end, i_nextFreeSlot);
            return i_nextFreeSlot;
        }

        static int GetListOfSubLines_forDisconnectedAnchorLines(Vector3 start, Vector3 end, Vector3 startToEnd, ref LineAnimationProgress lineAnimationProgressToUpdate, float animationSpeed, float tensionFactor)
        {
            float visibleSegment_lengthFactor_withoutTension = 0.3f;
            float visibleSegment_lengthFactor;

            if (tensionFactor > 1.0f)
            {
                visibleSegment_lengthFactor = visibleSegment_lengthFactor_withoutTension / tensionFactor; //-> caller has to ensure that "tensionFactor" is not "0"
            }
            else
            {
                float max_additionDueToTension = 0.5f - visibleSegment_lengthFactor_withoutTension;
                float additionDueToTension = max_additionDueToTension * (1.0f - tensionFactor);
                visibleSegment_lengthFactor = visibleSegment_lengthFactor_withoutTension + additionDueToTension;
            }

            float lengthFactor_oscillationAmplitude = 0.3333f * visibleSegment_lengthFactor;
            float max_relOscillationAmplitude = 0.48f;
            lengthFactor_oscillationAmplitude = Mathf.Min(lengthFactor_oscillationAmplitude, max_relOscillationAmplitude - visibleSegment_lengthFactor);

            float animationProgress = GetAnimationProgess_forDisconnectedAnchors(animationSpeed, lineAnimationProgressToUpdate);
            visibleSegment_lengthFactor = visibleSegment_lengthFactor + lengthFactor_oscillationAmplitude * Mathf.Sin(animationProgress);
            GetCurrLineAnimationProgress(ref lineAnimationProgressToUpdate, animationProgress);
            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(start, end))
            {
                return 0;
            }
            else
            {
                UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, start, 0);
                UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, start + startToEnd * visibleSegment_lengthFactor, 1);
                UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, end - startToEnd * visibleSegment_lengthFactor, 2);
                UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, end, 3);
                return 4;
            }
        }

        static int GetListOfSubLines_forSpiral(float sineDirAmplitude, float cosineDirAmplitude, Vector3 start, Vector3 end, Vector3 amplitudeUp_normalized, float animationSpeed, ref LineAnimationProgress lineAnimationProgressToUpdate, float tensionFactor)
        {
            //function returns "usedSlotsInListOfSubLines"
            int i_nextFreeSlot = 0;

            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(start, end))
            {
                SetLineAnimProgressToZero(ref lineAnimationProgressToUpdate);
                return 0;
            }

            float min_sineCycles_forWholeLine = 1.0f;
            int lineSegments_perSineCycle = 12;

            if (UtilitiesDXXL_Math.ApproximatelyZero(sineDirAmplitude))
            {
                SetLineAnimProgressToZero(ref lineAnimationProgressToUpdate);
                return FallbackToThinAndSolidLineStyle(start, end, out sineDirAmplitude, ref lineAnimationProgressToUpdate);
            }
            Vector3 startToEnd = end - start;
            Vector3 line_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(startToEnd, out float length);
            float sineCyles_perUnit = 1.0f / (sineDirAmplitude * UtilitiesDXXL_Math.anglesRad_perCircle);
            float sineCycles_forWholeLine = sineCyles_perUnit * length / tensionFactor; //-> caller has to ensure that "tensionFactor" is not "0"
            sineCycles_forWholeLine = Mathf.Max(sineCycles_forWholeLine, min_sineCycles_forWholeLine);
            int lineSegments_forWholeLine = Mathf.CeilToInt(lineSegments_perSineCycle * sineCycles_forWholeLine);
            float segmentLength = length / (float)lineSegments_forWholeLine;
            Vector3 segment = line_normalized * segmentLength;
            Vector3 maxAmplitude_inSineDir = amplitudeUp_normalized * sineDirAmplitude;
            Vector3 maxAmplitude_inCosineDir = Vector3.zero;
            bool isSpiral = true;
            if (UtilitiesDXXL_Math.ApproximatelyZero(cosineDirAmplitude))
            {
                isSpiral = false;
            }
            if (isSpiral)
            {
                Vector3 cosineDir_normalized = UtilitiesDXXL_Math.GetAVector_perpToGivenVectors(amplitudeUp_normalized, line_normalized); //is already normalized, because the two argument vectors are already normalized+perpendicular
                maxAmplitude_inCosineDir = cosineDir_normalized * cosineDirAmplitude;
            }

            float endOfFadeIn_as0to1ofWholeLine = (segmentLength * ((float)lineSegments_perSineCycle / 4.0f)) / length;
            float startOfFadeOut_as0to1ofWholeLine = 1.0f - endOfFadeIn_as0to1ofWholeLine;
            float animationProgress = GetAnimationProgess_forSpiral(animationSpeed, lineAnimationProgressToUpdate, sineDirAmplitude);
            GetCurrLineAnimationProgress(ref lineAnimationProgressToUpdate, animationProgress);

            i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, start, i_nextFreeSlot);//start of first subLineSegment
            for (int i = 1; i < lineSegments_forWholeLine; i++)
            {
                float progressThroughLine_atCurrPosOnLine_as0to1ofWholeLine = (float)i / (float)lineSegments_forWholeLine;
                float progressThroughLine_atCurrPosOnLine_asFinishedSineCylces = (float)i / (float)lineSegments_perSineCycle;
                float amplitudeDampingNearAnchors = UtilitiesDXXL_Math.Get_jumpFlyCurve_withPlateau(progressThroughLine_atCurrPosOnLine_as0to1ofWholeLine, endOfFadeIn_as0to1ofWholeLine, startOfFadeOut_as0to1ofWholeLine);
                Vector3 currPosOnLine = start + segment * i;
                float angleProgress = UtilitiesDXXL_Math.anglesRad_perCircle * progressThroughLine_atCurrPosOnLine_asFinishedSineCylces + animationProgress;
                Vector3 currSineOffset = amplitudeDampingNearAnchors * maxAmplitude_inSineDir * Mathf.Sin(angleProgress);
                Vector3 currCosineOffset = Vector3.zero;
                if (isSpiral)
                {
                    currCosineOffset = amplitudeDampingNearAnchors * maxAmplitude_inCosineDir * Mathf.Cos(angleProgress);
                }
                Vector3 currPos_onSpiralLine = currPosOnLine + currSineOffset + currCosineOffset;
                i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, currPos_onSpiralLine, i_nextFreeSlot); //end of previous subLineSegment
                i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, currPos_onSpiralLine, i_nextFreeSlot); //start of current subLineSegment
            }
            i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, end, i_nextFreeSlot);//end of last subLineSegment
            return i_nextFreeSlot;
        }

        static int GetListOfSubLines_forZigZagTypeLines(ref List<Vector3> usedListOfSubLines, DrawBasics.LineStyle zigZagType, Vector3 start, Vector3 end, float amplitude, Vector3 amplitudeUp_normalized, float tensionFactor, float animationSpeed, out float animationProgress_inSegments, out Vector3 normal_fromLinePerpToMainLinePoints, out Vector3 normal_fromLinePerpToPerpLinePoints, ref LineAnimationProgress lineAnimationProgressToUpdate, bool fillPosDetailLists, out int i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine, out int i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints)
        {
            //function returns "usedSlotsInListOfSubLines"

            int i_nextFreeSlot = 0;
            i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine = 0;
            i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints = 0;

            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(start, end))
            {
                animationProgress_inSegments = 0.0f;
                SetLineAnimProgressToZero(ref lineAnimationProgressToUpdate);
                normal_fromLinePerpToMainLinePoints = Vector3.zero;
                normal_fromLinePerpToPerpLinePoints = Vector3.zero;
                return 0;
            }

            float min_zigZagCycles_forWholeLine = 1.0f;
            int lineSegments_perZigZagCycle = 2;

            if (UtilitiesDXXL_Math.ApproximatelyZero(amplitude))
            {
                Debug.LogWarning("zigZag amplitude of 0 -> using straight solid line as fallback.");
                animationProgress_inSegments = 0.0f;
                normal_fromLinePerpToMainLinePoints = Vector3.zero;
                normal_fromLinePerpToPerpLinePoints = Vector3.zero;
                float lineStyleAmplitude;
                return FallbackToThinAndSolidLineStyle(start, end, out lineStyleAmplitude, ref lineAnimationProgressToUpdate);
            }

            Vector3 startToEnd = end - start;
            Vector3 line_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(startToEnd, out float length);
            float zigZagCyles_perUnit = 1.0f / (amplitude * 4.0f * tensionFactor); //-> caller has to ensure that "tensionFactor" is not 0 and not negative
            float zigZagCycles_forWholeLine = zigZagCyles_perUnit * length;
            zigZagCycles_forWholeLine = Mathf.Max(zigZagCycles_forWholeLine, min_zigZagCycles_forWholeLine);
            float subLineSegments_ofWholeLine = lineSegments_perZigZagCycle * zigZagCycles_forWholeLine;
            float segmentLength = length / subLineSegments_ofWholeLine;
            Vector3 segment = line_normalized * segmentLength;
            normal_fromLinePerpToMainLinePoints = amplitudeUp_normalized;
            Vector3 mainZigZagOffset = amplitude * normal_fromLinePerpToMainLinePoints;
            normal_fromLinePerpToPerpLinePoints = Vector3.zero;
            Vector3 additionalPerpZigZagOffset;

            if (zigZagType == DrawBasics.LineStyle.zigzag || zigZagType == DrawBasics.LineStyle.electricNoise || zigZagType == DrawBasics.LineStyle.electricImpulses || zigZagType == DrawBasics.LineStyle.freeHand2D || zigZagType == DrawBasics.LineStyle.freeHand3D)
            {
                i_nextFreeSlot = GetListOfSubLines_forZigZagLine(ref usedListOfSubLines, mainZigZagOffset, length, line_normalized, segment, segmentLength, start, end, animationSpeed, out animationProgress_inSegments, ref lineAnimationProgressToUpdate, fillPosDetailLists, out i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine, out i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
                if (zigZagType == DrawBasics.LineStyle.freeHand3D)
                {
                    normal_fromLinePerpToPerpLinePoints = UtilitiesDXXL_Math.GetAVector_perpToGivenVectors(normal_fromLinePerpToMainLinePoints, line_normalized); //result is normalized (because argument vectors are normalized+perp)
                }
            }
            else
            {
                if (zigZagType == DrawBasics.LineStyle.rhombus)
                {
                    int usedSlotsIn_upSegments = GetListOfSubLines_forZigZagLine(ref s_listOfSubLinesForZigZagsUpSegments, mainZigZagOffset, length, line_normalized, segment, segmentLength, start, end, animationSpeed, out animationProgress_inSegments, ref lineAnimationProgressToUpdate, false, out i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine, out i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
                    int usedSlotsIn_downSegments = GetListOfSubLines_forZigZagLine(ref s_listOfSubLinesForZigZagsDownSegments, -mainZigZagOffset, length, line_normalized, segment, segmentLength, start, end, animationSpeed, out animationProgress_inSegments, ref lineAnimationProgressToUpdate, fillPosDetailLists, out i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine, out i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
                    i_nextFreeSlot = GetIntegrateSegmentList(ref usedListOfSubLines, ref s_listOfSubLinesForZigZagsUpSegments, usedSlotsIn_upSegments, ref s_listOfSubLinesForZigZagsDownSegments, usedSlotsIn_downSegments);
                }
                else
                {
                    if (zigZagType == DrawBasics.LineStyle.doubleRhombus)
                    {
                        normal_fromLinePerpToPerpLinePoints = UtilitiesDXXL_Math.GetAVector_perpToGivenVectors(normal_fromLinePerpToMainLinePoints, line_normalized); //result is normalized (because argument vectors are normalized+perp)
                        additionalPerpZigZagOffset = amplitude * normal_fromLinePerpToPerpLinePoints;
                        int usedSlotsIn_upSegments = GetListOfSubLines_forZigZagLine(ref s_listOfSubLinesForZigZagsUpSegments, mainZigZagOffset, length, line_normalized, segment, segmentLength, start, end, animationSpeed, out animationProgress_inSegments, ref lineAnimationProgressToUpdate, false, out i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine, out i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
                        int usedSlotsIn_downSegments = GetListOfSubLines_forZigZagLine(ref s_listOfSubLinesForZigZagsDownSegments, -mainZigZagOffset, length, line_normalized, segment, segmentLength, start, end, animationSpeed, out animationProgress_inSegments, ref lineAnimationProgressToUpdate, false, out i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine, out i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
                        int usedSlotsIn_leftSegments = GetListOfSubLines_forZigZagLine(ref s_listOfSubLinesForZigZagsLeftSegments, additionalPerpZigZagOffset, length, line_normalized, segment, segmentLength, start, end, animationSpeed, out animationProgress_inSegments, ref lineAnimationProgressToUpdate, false, out i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine, out i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
                        int usedSlotsIn_rightSegments = GetListOfSubLines_forZigZagLine(ref s_listOfSubLinesForZigZagsRightSegments, -additionalPerpZigZagOffset, length, line_normalized, segment, segmentLength, start, end, animationSpeed, out animationProgress_inSegments, ref lineAnimationProgressToUpdate, fillPosDetailLists, out i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine, out i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
                        int usedSlotsIn_upAndDownSegments = GetIntegrateSegmentList(ref s_listOfSubLinesForZigZagsUpAndDownSegments, ref s_listOfSubLinesForZigZagsUpSegments, usedSlotsIn_upSegments, ref s_listOfSubLinesForZigZagsDownSegments, usedSlotsIn_downSegments);
                        int usedSlotsIn_leftAndRightSegments = GetIntegrateSegmentList(ref s_listOfSubLinesForZigZagsLeftAndRightSegments, ref s_listOfSubLinesForZigZagsLeftSegments, usedSlotsIn_leftSegments, ref s_listOfSubLinesForZigZagsRightSegments, usedSlotsIn_rightSegments);
                        i_nextFreeSlot = GetIntegrateSegmentList(ref usedListOfSubLines, ref s_listOfSubLinesForZigZagsUpAndDownSegments, usedSlotsIn_upAndDownSegments, ref s_listOfSubLinesForZigZagsLeftAndRightSegments, usedSlotsIn_leftAndRightSegments);
                    }
                    else
                    {
                        Debug.LogError("The line style " + zigZagType + " is not yet implemented as a zigZag-Line. -> Now using straight solid line as fallback.");
                        i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine = 0;
                        i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints = 0;
                        animationProgress_inSegments = 0.0f;
                        normal_fromLinePerpToMainLinePoints = Vector3.zero;
                        normal_fromLinePerpToPerpLinePoints = Vector3.zero;
                        float lineStyleAmplitude;
                        return FallbackToThinAndSolidLineStyle(start, end, out lineStyleAmplitude, ref lineAnimationProgressToUpdate);
                    }
                }
            }

            return i_nextFreeSlot;
        }

        static int GetListOfSubLines_forZigZagLine(ref List<Vector3> usedListOfSubLines, Vector3 zigZagOffset, float length, Vector3 line_normalized, Vector3 segment, float segmentLength, Vector3 start, Vector3 end, float animationSpeed, out float animationProgress_inSegments, ref LineAnimationProgress lineAnimationProgressToUpdate, bool fillPosDetailLists, out int i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine, out int i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints)
        {
            //function returns "usedSlotsIn_usedListOfSubLines"

            int i_nextFreeSlot_inUsedListOfSubLines = 0;
            int i_nextFreeSlot_inDistanceToStartList = 0;
            i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine = 0;
            i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints = 0;

            segmentLength = Mathf.Max(segmentLength, 0.0001f);
            float halfSegmentLength = 0.5f * segmentLength;
            float cycleLength = 2.0f * segmentLength; //segmentLength describes "1flank ("zig")", cylce describes "2flanks("zagZag")".
            float animationProgress = GetAnimationProgess_forSimpleCase(animationSpeed, lineAnimationProgressToUpdate);
            GetCurrLineAnimationProgress(ref lineAnimationProgressToUpdate, animationProgress);
            float animationProgress_inCycles = animationProgress / cycleLength;
            animationProgress_inSegments = 2.0f * animationProgress_inCycles;
            float animationProgress_as0to1 = UtilitiesDXXL_Math.Loop_floatIntoSpanFrom_m1_to_p1(animationProgress_inCycles);
            float alreadyStepped = -3.5f * segmentLength + animationProgress_as0to1 * cycleLength; //"-3.5" consists of: "0.5" -> so that line starts at crossing point, "1.0" -> is taken at start of first while loop, "2.0" -> the animation cycle

            bool sideOfCurrAmplitude = true;
            int loopIterationCounter = 0;
            while (alreadyStepped < length)
            {
                alreadyStepped = alreadyStepped + segmentLength; //incrementation at start of while loop ensures, that one pos after lineEnd gets added.

                Vector3 currUsed_zigZagOffset = sideOfCurrAmplitude ? (-zigZagOffset) : zigZagOffset;
                sideOfCurrAmplitude = !sideOfCurrAmplitude;

                Vector3 currPosOnLine = start + alreadyStepped * line_normalized;
                Vector3 currPos_onZigZagLine = currPosOnLine + currUsed_zigZagOffset;

                i_nextFreeSlot_inUsedListOfSubLines = UtilitiesDXXL_List.AddToAVectorList(ref usedListOfSubLines, currPos_onZigZagLine, i_nextFreeSlot_inUsedListOfSubLines);//end of previous subLineSegment
                i_nextFreeSlot_inUsedListOfSubLines = UtilitiesDXXL_List.AddToAVectorList(ref usedListOfSubLines, currPos_onZigZagLine, i_nextFreeSlot_inUsedListOfSubLines);//start of current subLineSegment
                i_nextFreeSlot_inDistanceToStartList = AddToDistanceToStartList(alreadyStepped, i_nextFreeSlot_inDistanceToStartList);
                i_nextFreeSlot_inDistanceToStartList = AddToDistanceToStartList(alreadyStepped, i_nextFreeSlot_inDistanceToStartList);
                if (fillPosDetailLists)
                {
                    i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine = UtilitiesDXXL_List.AddToAVectorList(ref s_subLinePoints_projectionOntoStraightMainLine, currPosOnLine, i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine);
                    i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine = UtilitiesDXXL_List.AddToAVectorList(ref s_subLinePoints_projectionOntoStraightMainLine, currPosOnLine, i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine);
                    i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints = UtilitiesDXXL_List.AddToAVectorList(ref s_vectors_fromProjectionOntoMainLine_toSubLinePoints, currUsed_zigZagOffset, i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
                    i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints = UtilitiesDXXL_List.AddToAVectorList(ref s_vectors_fromProjectionOntoMainLine_toSubLinePoints, currUsed_zigZagOffset, i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
                }

                loopIterationCounter++;
                if (loopIterationCounter > 100000)
                {
                    Debug.LogError("Too many while loop iterations. Forced quit to prevent freeze.   segmentLength: " + segmentLength);
                    break;
                }
            }

            //Beautify lineStart:
            for (int i = 0; i < i_nextFreeSlot_inUsedListOfSubLines; i++)
            {
                if (((i + 1) < i_nextFreeSlot_inUsedListOfSubLines) == false)
                {
                    UtilitiesDXXL_Log.PrintErrorCode("19-" + i + "-" + i_nextFreeSlot_inUsedListOfSubLines);
                    float lineStyleAmplitude;
                    return FallbackToThinAndSolidLineStyle(start, end, out lineStyleAmplitude, ref lineAnimationProgressToUpdate);
                }

                if (s_subLineAnchor_distanceToStart[i] > halfSegmentLength)
                {
                    break;
                }
                else
                {
                    if (s_subLineAnchor_distanceToStart[i] > (-halfSegmentLength))
                    {
                        float distance_thatMarks_nextAmplitudeZeroIntersection_afterCurrPos = s_subLineAnchor_distanceToStart[i] + halfSegmentLength;
                        Vector3 nextAmplitudeZeroIntersection_afterCurrPos = start + distance_thatMarks_nextAmplitudeZeroIntersection_afterCurrPos * line_normalized;
                        Vector3 pos_relToNextAmplitudeZeroIntersection = usedListOfSubLines[i] - nextAmplitudeZeroIntersection_afterCurrPos;
                        float triangleShorteningFactor = distance_thatMarks_nextAmplitudeZeroIntersection_afterCurrPos / segmentLength;

                        usedListOfSubLines[i] = nextAmplitudeZeroIntersection_afterCurrPos + pos_relToNextAmplitudeZeroIntersection * triangleShorteningFactor;
                        usedListOfSubLines[i + 1] = usedListOfSubLines[i];
                        if (fillPosDetailLists)
                        {
                            s_subLinePoints_projectionOntoStraightMainLine[i] = s_subLinePoints_projectionOntoStraightMainLine[i] + line_normalized * (halfSegmentLength * (1.0f - triangleShorteningFactor));
                            s_subLinePoints_projectionOntoStraightMainLine[i + 1] = s_subLinePoints_projectionOntoStraightMainLine[i];
                            s_vectors_fromProjectionOntoMainLine_toSubLinePoints[i] = s_vectors_fromProjectionOntoMainLine_toSubLinePoints[i] * triangleShorteningFactor;
                            s_vectors_fromProjectionOntoMainLine_toSubLinePoints[i + 1] = s_vectors_fromProjectionOntoMainLine_toSubLinePoints[i];
                        }
                    }
                }
                i++;
            }

            //Remove points lower than startPos:
            for (int i = i_nextFreeSlot_inUsedListOfSubLines - 1; i >= 0; i--)
            {
                if (s_subLineAnchor_distanceToStart[i] <= (-halfSegmentLength))
                {
                    i_nextFreeSlot_inUsedListOfSubLines = UtilitiesDXXL_List.RemoveAt_fromAVectorList(ref usedListOfSubLines, i, i_nextFreeSlot_inUsedListOfSubLines);
                    i_nextFreeSlot_inDistanceToStartList = RemoveAt_fromDistanceToStartList(i, i_nextFreeSlot_inDistanceToStartList);
                    if (fillPosDetailLists)
                    {
                        i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine = UtilitiesDXXL_List.RemoveAt_fromAVectorList(ref s_subLinePoints_projectionOntoStraightMainLine, i, i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine);
                        i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints = UtilitiesDXXL_List.RemoveAt_fromAVectorList(ref s_vectors_fromProjectionOntoMainLine_toSubLinePoints, i, i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
                    }
                }
            }

            //Beautify lineEnd:
            float distance_markingHalfSegmentAfterLineEnd = length + halfSegmentLength;
            float distance_markingHalfSegmentBeforeLineEnd = length - halfSegmentLength;
            for (int i = i_nextFreeSlot_inUsedListOfSubLines - 1; i >= 0; i--)
            {
                if (i <= 0)
                {
                    UtilitiesDXXL_Log.PrintErrorCode("20-" + i + "-" + i_nextFreeSlot_inUsedListOfSubLines);
                    float lineStyleAmplitude;
                    return FallbackToThinAndSolidLineStyle(start, end, out lineStyleAmplitude, ref lineAnimationProgressToUpdate);
                }

                if (s_subLineAnchor_distanceToStart[i] < distance_markingHalfSegmentBeforeLineEnd)
                {
                    break;
                }
                else
                {
                    if (s_subLineAnchor_distanceToStart[i] < distance_markingHalfSegmentAfterLineEnd)
                    {
                        float distance_thatMarks_lastAmplitudeZeroIntersection_beforeCurrPos = s_subLineAnchor_distanceToStart[i] - halfSegmentLength;
                        Vector3 lastAmplitudeZeroIntersection_beforeCurrPos = start + distance_thatMarks_lastAmplitudeZeroIntersection_beforeCurrPos * line_normalized;
                        Vector3 pos_relToPrevAmplitudeZeroIntersection = usedListOfSubLines[i] - lastAmplitudeZeroIntersection_beforeCurrPos;
                        float triangleShorteningFactor;
                        if (s_subLineAnchor_distanceToStart[i] < length)
                        {
                            float distance_thatMarks_firstAmplitudeZeroIntersection_afterCurrPos = s_subLineAnchor_distanceToStart[i] + halfSegmentLength;
                            triangleShorteningFactor = (segmentLength - (distance_thatMarks_firstAmplitudeZeroIntersection_afterCurrPos - length)) / segmentLength;
                        }
                        else
                        {
                            triangleShorteningFactor = (length - distance_thatMarks_lastAmplitudeZeroIntersection_beforeCurrPos) / segmentLength;
                        }

                        usedListOfSubLines[i] = lastAmplitudeZeroIntersection_beforeCurrPos + pos_relToPrevAmplitudeZeroIntersection * triangleShorteningFactor;
                        usedListOfSubLines[i - 1] = usedListOfSubLines[i];
                        if (fillPosDetailLists)
                        {
                            s_subLinePoints_projectionOntoStraightMainLine[i] = lastAmplitudeZeroIntersection_beforeCurrPos + 0.5f * segment * triangleShorteningFactor;
                            s_subLinePoints_projectionOntoStraightMainLine[i - 1] = s_subLinePoints_projectionOntoStraightMainLine[i];
                            s_vectors_fromProjectionOntoMainLine_toSubLinePoints[i] = s_vectors_fromProjectionOntoMainLine_toSubLinePoints[i] * triangleShorteningFactor;
                            s_vectors_fromProjectionOntoMainLine_toSubLinePoints[i - 1] = s_vectors_fromProjectionOntoMainLine_toSubLinePoints[i];
                        }
                    }
                    else
                    {
                        i_nextFreeSlot_inUsedListOfSubLines = UtilitiesDXXL_List.RemoveAt_fromAVectorList(ref usedListOfSubLines, i, i_nextFreeSlot_inUsedListOfSubLines);
                        i_nextFreeSlot_inUsedListOfSubLines = UtilitiesDXXL_List.RemoveAt_fromAVectorList(ref usedListOfSubLines, i - 1, i_nextFreeSlot_inUsedListOfSubLines);
                        i_nextFreeSlot_inDistanceToStartList = RemoveAt_fromDistanceToStartList(i, i_nextFreeSlot_inDistanceToStartList);
                        i_nextFreeSlot_inDistanceToStartList = RemoveAt_fromDistanceToStartList(i - 1, i_nextFreeSlot_inDistanceToStartList);

                        if (fillPosDetailLists)
                        {
                            i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine = UtilitiesDXXL_List.RemoveAt_fromAVectorList(ref s_subLinePoints_projectionOntoStraightMainLine, i, i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine);
                            i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine = UtilitiesDXXL_List.RemoveAt_fromAVectorList(ref s_subLinePoints_projectionOntoStraightMainLine, i - 1, i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine);
                            i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints = UtilitiesDXXL_List.RemoveAt_fromAVectorList(ref s_vectors_fromProjectionOntoMainLine_toSubLinePoints, i, i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
                            i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints = UtilitiesDXXL_List.RemoveAt_fromAVectorList(ref s_vectors_fromProjectionOntoMainLine_toSubLinePoints, i - 1, i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
                        }
                    }
                }
                i--;
            }

            //Fill detail info lists for line ends:
            i_nextFreeSlot_inUsedListOfSubLines = UtilitiesDXXL_List.InsertToAVectorList(ref usedListOfSubLines, 0, start, i_nextFreeSlot_inUsedListOfSubLines); //start of first subLineSegment
            if (fillPosDetailLists)
            {
                i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine = UtilitiesDXXL_List.InsertToAVectorList(ref s_subLinePoints_projectionOntoStraightMainLine, 0, start, i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine);
                i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints = UtilitiesDXXL_List.InsertToAVectorList(ref s_vectors_fromProjectionOntoMainLine_toSubLinePoints, 0, Vector3.zero, i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
            }

            i_nextFreeSlot_inUsedListOfSubLines = UtilitiesDXXL_List.AddToAVectorList(ref usedListOfSubLines, end, i_nextFreeSlot_inUsedListOfSubLines); //end of last subLineSegment
            if (fillPosDetailLists)
            {
                i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine = UtilitiesDXXL_List.AddToAVectorList(ref s_subLinePoints_projectionOntoStraightMainLine, end, i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine);
                i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints = UtilitiesDXXL_List.AddToAVectorList(ref s_vectors_fromProjectionOntoMainLine_toSubLinePoints, Vector3.zero, i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
            }

            return i_nextFreeSlot_inUsedListOfSubLines;
        }

        static int GetIntegrateSegmentList(ref List<Vector3> targetList, ref List<Vector3> list1_toIntegrate, int usedSlotsInList1, ref List<Vector3> list2_toIntegrate, int usedSlotsInList2)
        {
            //function returns "usedSlotsInTargetList"
            //This function integrates the different lineStrings of rhombus, so that colorFade correctly works
            int maxCycles = usedSlotsInList1 + usedSlotsInList2 + 100;
            int i_list1 = 0;
            int i_list2 = 0;
            bool list1or2 = true;
            int i_cycle = 0;
            int i_ofNextFreeSlot_inTargetList = 0;
            while (i_cycle < maxCycles)
            {
                if (list1or2)
                {
                    i_ofNextFreeSlot_inTargetList = IntegrateSegment(ref targetList, i_ofNextFreeSlot_inTargetList, list1_toIntegrate, ref i_list1, usedSlotsInList1);
                }
                else
                {
                    i_ofNextFreeSlot_inTargetList = IntegrateSegment(ref targetList, i_ofNextFreeSlot_inTargetList, list2_toIntegrate, ref i_list2, usedSlotsInList2);
                }
                list1or2 = !list1or2;
                i_cycle++;
            }
            return i_ofNextFreeSlot_inTargetList;
        }

        static int IntegrateSegment(ref List<Vector3> targetList, int i_ofNextFreeSlot_inTargetList, List<Vector3> listToIntegrate, ref int i_slotToIntegrateInListToIntegrate, int usedSlotsInListToIntegrate)
        {
            for (int i = 0; i < 2; i++)
            {
                if (i_slotToIntegrateInListToIntegrate < usedSlotsInListToIntegrate)
                {
                    i_ofNextFreeSlot_inTargetList = UtilitiesDXXL_List.AddToAVectorList(ref targetList, listToIntegrate[i_slotToIntegrateInListToIntegrate], i_ofNextFreeSlot_inTargetList);
                    i_slotToIntegrateInListToIntegrate++;
                }
            }
            return i_ofNextFreeSlot_inTargetList;
        }

        static int GetListOfSubLines_forFreehandTypeLines(DrawBasics.LineStyle freehandType, Vector3 start, Vector3 end, float patternScaled_freeHandLineAmplitude, Vector3 amplitudeUp_normalized, float animationSpeed, ref LineAnimationProgress lineAnimationProgressToUpdate, float tensionFactor)
        {
            //function returns "usedSlotsInListOfSubLines"

            if (freehandType != DrawBasics.LineStyle.freeHand2D && freehandType != DrawBasics.LineStyle.freeHand3D)
            {
                Debug.LogError("Line style " + freehandType + " is not yet implemented as a freehand style type. -> Now using straight solid line as fallback.");
                float lineStyleAmplitude;
                return FallbackToThinAndSolidLineStyle(start, end, out lineStyleAmplitude, ref lineAnimationProgressToUpdate);
            }

            float freehandsZigZagAmplitude = 0.05f * patternScaled_freeHandLineAmplitude;
            float animationProgress_inSegments;
            Vector3 normal_fromLinePerpToMainLinePoints;
            Vector3 normal_fromLinePerpToPerpLinePoints;
            int usedSlotsInListOfSubLines = GetListOfSubLines_forZigZagTypeLines(ref s_listOfSubLines, freehandType, start, end, freehandsZigZagAmplitude, amplitudeUp_normalized, tensionFactor, 2.0f * animationSpeed, out animationProgress_inSegments, out normal_fromLinePerpToMainLinePoints, out normal_fromLinePerpToPerpLinePoints, ref lineAnimationProgressToUpdate, true, out int i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine, out int i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);

            for (int i = 1; i < (usedSlotsInListOfSubLines - 1); i++)
            {
                float perlinResult_forZigZagDim = Mathf.PerlinNoise(((float)i + animationProgress_inSegments - 2.0f * Mathf.Floor(animationProgress_inSegments)) * 0.043f, 0.0f);
                float distance_fromProjectionOntoMainLine_toSubLinePointInZigZagPlane = patternScaled_freeHandLineAmplitude * (perlinResult_forZigZagDim - 0.5f);
                Vector3 currVector_fromProjectionOntoMainLine_toSubLinePointInZigZagPlane = normal_fromLinePerpToMainLinePoints * distance_fromProjectionOntoMainLine_toSubLinePointInZigZagPlane;

                float perlinResult_forPerpDim;
                float distance_fromProjectionOntoZigZagPlane_perpToSubLinePoint;
                Vector3 currVector_fromProjectionOntoZigZagPlane_perpToSubLinePoint = Vector3.zero;
                if (freehandType == DrawBasics.LineStyle.freeHand3D)
                {
                    perlinResult_forPerpDim = Mathf.PerlinNoise(((float)i + animationProgress_inSegments - 2.0f * Mathf.Floor(animationProgress_inSegments)) * 0.057f, 0.0f);
                    distance_fromProjectionOntoZigZagPlane_perpToSubLinePoint = patternScaled_freeHandLineAmplitude * (perlinResult_forPerpDim - 0.5f);
                    currVector_fromProjectionOntoZigZagPlane_perpToSubLinePoint = normal_fromLinePerpToPerpLinePoints * distance_fromProjectionOntoZigZagPlane_perpToSubLinePoint;
                }

                s_listOfSubLines[i] = s_subLinePoints_projectionOntoStraightMainLine[i] + currVector_fromProjectionOntoMainLine_toSubLinePointInZigZagPlane + currVector_fromProjectionOntoZigZagPlane_perpToSubLinePoint;
                s_listOfSubLines[i + 1] = s_listOfSubLines[i];
                i++;
            }

            return usedSlotsInListOfSubLines;
        }

        static int GetListOfSubLines_forElectricNoiseLines(Vector3 start, Vector3 end, float stylePatternScaleFactor, float lineWidth, out float amplitude, ref LineAnimationProgress lineAnimationProgressToUpdate, Vector3 amplitudeUp_normalized, float animationSpeed, float tensionFactor)
        {
            float minSqueezeRatio = 0.06f;
            float maxSqueezeRatio = 0.2f;
            float squeezeRatio = minSqueezeRatio;
            if (UtilitiesDXXL_Math.ApproximatelyZero(lineWidth) == false)
            {
                squeezeRatio = 5.0f * UtilitiesDXXL_Math.Get_2degParabolicFlateningRise_flatRightOfOne(lineWidth);
            }
            squeezeRatio = Mathf.Max(squeezeRatio, minSqueezeRatio);
            squeezeRatio = Mathf.Min(squeezeRatio, maxSqueezeRatio);
            squeezeRatio = squeezeRatio * tensionFactor;

            float minSqueezeRatio_afterTension = 0.001f;
            squeezeRatio = Mathf.Max(squeezeRatio, minSqueezeRatio_afterTension);

            float animationProgress_inSegments;
            Vector3 normal_fromLinePerpToMainLinePoints;
            Vector3 normal_fromLinePerpToPerpLinePoints;
            amplitude = electricNoiseLineAmplitude * stylePatternScaleFactor;
            int usedSlotsInListOfSubLines = GetListOfSubLines_forZigZagTypeLines(ref s_listOfSubLines, DrawBasics.LineStyle.electricNoise, start, end, amplitude, amplitudeUp_normalized, squeezeRatio, animationSpeed, out animationProgress_inSegments, out normal_fromLinePerpToMainLinePoints, out normal_fromLinePerpToPerpLinePoints, ref lineAnimationProgressToUpdate, true, out int i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine, out int i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
            amplitude = 0.7f * amplitude; //for text distance to line

            for (int i = 1; i < (usedSlotsInListOfSubLines - 1); i++)
            {
                float perlinResult = Mathf.PerlinNoise(((float)i + animationProgress_inSegments) * 13.23f, 0.0f);
                s_listOfSubLines[i] = s_subLinePoints_projectionOntoStraightMainLine[i] + s_vectors_fromProjectionOntoMainLine_toSubLinePoints[i] * perlinResult;
                s_listOfSubLines[i + 1] = s_listOfSubLines[i];
                i++;
            }
            return usedSlotsInListOfSubLines;
        }

        static int GetListOfSubLines_forElectricImpulseLines(Vector3 start, Vector3 end, float stylePatternScaleFactor, float lineWidth, out float amplitude, ref LineAnimationProgress lineAnimationProgressToUpdate, Vector3 amplitudeUp_normalized, float animationSpeed, float tensionFactor)
        {
            Vector3 startToEnd = end - start;
            float cycleLength = impulseDistance_ofElectricImpulseLines * stylePatternScaleFactor * tensionFactor; //-> caller has to ensure that "tensionFactor" is not "0"
            float patternScaled_electricImpulseLineAmplitude = electricImpulseLineAmplitude * stylePatternScaleFactor;
            amplitude = patternScaled_electricImpulseLineAmplitude;
            float impulseLength = patternScaled_electricImpulseLineAmplitude * impulseSqueeze_ofElectricImpulseLines + lineWidth;
            impulseLength = Mathf.Min(impulseLength, 0.3f * cycleLength);
            float solidLength = cycleLength - impulseLength;

            if (solidLength <= 0.0f)
            {
                //Debug.LogWarning("The configuration of the electricImpulses line style leads to no space between the impulses. Thus the result looks like the zigzag line style. Current configuration -> electricImpulseLineAmplitude: " + electricImpulseLineAmplitude + " patternScaled_electricImpulseLineAmplitude: " + patternScaled_electricImpulseLineAmplitude + " impulseDistance_ofElectricImpulseLines: " + impulseDistance_ofElectricImpulseLines + " impulseSqueeze_ofElectricImpulseLines: " + impulseSqueeze_ofElectricImpulseLines + " (potentially scaled)stylePatternScaleFactor: " + stylePatternScaleFactor);
                solidLength = 0.0001f;
            }

            impulseLength = Mathf.Max(impulseLength, 0.0001f);
            Vector3 lineNormalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(startToEnd, out float lineLength);

            if (UtilitiesDXXL_Math.ApproximatelyZero(animationSpeed) && CheckIfAnimationProgessIsZeroOrNull(lineAnimationProgressToUpdate))
            {
                //animation didn't start
                if (impulseLength > lineLength)
                {
                    //impulse fills whole line
                    float animProgress_ofZigZag;
                    Vector3 normal_fromLinePerpToMainLinePoints;
                    Vector3 normal_fromLinePerpToPerpLinePoints;
                    LineAnimationProgress lineAnimProgress_ofZigZag = null;
                    int usedSlotsInListOfSubLines = GetListOfSubLines_forZigZagTypeLines(ref s_listOfSubLines, DrawBasics.LineStyle.zigzag, start, end, patternScaled_electricImpulseLineAmplitude, amplitudeUp_normalized, impulseSqueeze_ofElectricImpulseLines, animationSpeed, out animProgress_ofZigZag, out normal_fromLinePerpToMainLinePoints, out normal_fromLinePerpToPerpLinePoints, ref lineAnimProgress_ofZigZag, false, out int i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine, out int i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
                    return usedSlotsInListOfSubLines;
                }
            }
            float animationProgress = GetAnimationProgess_forSimpleCase(animationSpeed, lineAnimationProgressToUpdate);
            GetCurrLineAnimationProgress(ref lineAnimationProgressToUpdate, animationProgress);
            float animationProgress_inCycles = animationProgress / cycleLength;
            float animationProgress_as0to1 = UtilitiesDXXL_Math.Loop_floatIntoSpanFrom_m1_to_p1(animationProgress_inCycles);
            float alreadyStepped = -cycleLength + cycleLength * animationProgress_as0to1 - 0.96f * solidLength;
            Vector3 currPos = start + alreadyStepped * lineNormalized;

            int i_nextFreeSlot = 0;
            int loopIterationCounter = 0;
            while (alreadyStepped < lineLength)
            {
                if (alreadyStepped <= 0.0f)
                {
                    i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, start, i_nextFreeSlot);//start of solid line
                }
                else
                {
                    i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, currPos, i_nextFreeSlot);//start of solid line
                }
                currPos = currPos + lineNormalized * solidLength;
                alreadyStepped = alreadyStepped + solidLength;
                if (alreadyStepped >= lineLength)
                {
                    //lineEnd reached during current solid phase
                    i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, end, i_nextFreeSlot); //end of solid line
                    break;
                }
                if (alreadyStepped <= 0.0f)
                {
                    i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, start, i_nextFreeSlot);//end of solid line
                }
                else
                {
                    i_nextFreeSlot = UtilitiesDXXL_List.AddToAVectorList(ref s_listOfSubLines, currPos, i_nextFreeSlot); //end of solid line
                }

                float alreadyStepped_afterUpcomingImpulse = alreadyStepped + impulseLength;
                Vector3 startPosOfImpulse;
                if (alreadyStepped <= 0.0f)
                {
                    startPosOfImpulse = start;
                }
                else
                {
                    startPosOfImpulse = currPos;
                }

                Vector3 endPosOfImpulse;
                if (alreadyStepped_afterUpcomingImpulse >= lineLength)
                {
                    endPosOfImpulse = end;
                }
                else
                {
                    if (alreadyStepped_afterUpcomingImpulse <= 0.0f)
                    {
                        endPosOfImpulse = start;
                    }
                    else
                    {
                        endPosOfImpulse = currPos + lineNormalized * impulseLength;
                    }
                }

                float animProgress_ofZigZag;
                Vector3 normal_fromLinePerpToMainLinePoints;
                Vector3 normal_fromLinePerpToPerpLinePoints;
                LineAnimationProgress lineAnimProgress_ofZigZag = null;
                int usedSlotsInRangeList = GetListOfSubLines_forZigZagTypeLines(ref s_addedRangeForListOfSubLines, DrawBasics.LineStyle.zigzag, startPosOfImpulse, endPosOfImpulse, patternScaled_electricImpulseLineAmplitude, amplitudeUp_normalized, impulseSqueeze_ofElectricImpulseLines, 0.0f, out animProgress_ofZigZag, out normal_fromLinePerpToMainLinePoints, out normal_fromLinePerpToPerpLinePoints, ref lineAnimProgress_ofZigZag, false, out int i_nextFreeSlot_in_subLinePoints_projectionOntoStraightMainLine, out int i_nextFreeSlot_in_vectors_fromProjectionOntoMainLine_toSubLinePoints);
                i_nextFreeSlot = UtilitiesDXXL_List.AddRangeToAVectorList(ref s_listOfSubLines, s_addedRangeForListOfSubLines, i_nextFreeSlot, usedSlotsInRangeList);

                alreadyStepped = alreadyStepped_afterUpcomingImpulse;
                currPos = start + alreadyStepped * lineNormalized;

                loopIterationCounter++;
                if (loopIterationCounter > 100000)
                {
                    Debug.LogError("Too many while loop iterations. Forced quit to prevent freeze.   solidLength: " + solidLength + "   impulseLength: " + impulseLength);
                    break;
                }
            }
            return i_nextFreeSlot;
        }

        static int FallbackToThinAndSolidLineStyle(Vector3 start, Vector3 end, out float unusedAmplitude, ref LineAnimationProgress lineAnimationProgressToUpdate)
        {
            return RefillListOfSubLines(start, end, DrawBasics.LineStyle.solid, 1.0f, 0.0f, out unusedAmplitude, default(Vector3), 0.0f, ref lineAnimationProgressToUpdate, false, false, 1.0f);
        }

        static void GetCurrLineAnimationProgress(ref LineAnimationProgress lineAnimationProgress, float currAnimProgress)
        {
            if (lineAnimationProgress != null)
            {
                lineAnimationProgress.animProgress = currAnimProgress;
                lineAnimationProgress.timeOfDraw = GetTime();
            }
        }

        static float GetAnimationProgess_forDashLine(float animationSpeed, LineAnimationProgress lineAnimationProgress, float animationLoopLength)
        {
            //Know issue: jittery animation in Screenspace:
            //-> The reason seems to be a float calculation precision limit error in "UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane()" when "UtilitiesDXXL_Screenspace.GetLineParamsFromCamViewportSpace()" requests the conversion from "width_relToViewportHeight" to "width_worldSpace".
            //-> In some situations it helped when using the "line_fadeableAnimSpeed_screenspace.Draw()" (which holds a persistent "LineAnimationProgress" member) instead of just calling the static function "DrawScreenspace.Line()", though I don't know yet why this helps.
          
            if (lineAnimationProgress == null)
            {
                return ((animationSpeed * GetTime()) / animationLoopLength);
            }
            else
            {
                float timeSinceLastCall = GetTime() - lineAnimationProgress.timeOfDraw;
                return lineAnimationProgress.animProgress + ((animationSpeed * timeSinceLastCall) / animationLoopLength);
            }
        }

        static float GetAnimationProgess_forSpiral(float animationSpeed, LineAnimationProgress lineAnimationProgress, float sineDirAmplitude)
        {
            //Know issue: jittery animation in Screenspace: See notes in "GetAnimationProgess_forDashLine()"
            if (lineAnimationProgress == null)
            {
                return (-(GetTime() * animationSpeed) / sineDirAmplitude);
            }
            else
            {
                float timeSinceLastCall = GetTime() - lineAnimationProgress.timeOfDraw;
                return lineAnimationProgress.animProgress + (-(timeSinceLastCall * animationSpeed) / sineDirAmplitude);
            }
        }

        static float GetAnimationProgess_forSimpleCase(float animationSpeed, LineAnimationProgress lineAnimationProgress)
        {
            //Know issue: jittery animation in Screenspace: See notes in "GetAnimationProgess_forDashLine()"
            if (lineAnimationProgress == null)
            {
                return (animationSpeed * GetTime());
            }
            else
            {
                float timeSinceLastCall = GetTime() - lineAnimationProgress.timeOfDraw;
                return lineAnimationProgress.animProgress + (animationSpeed * timeSinceLastCall);
            }
        }

        static float GetAnimationProgess_forDisconnectedAnchors(float animationSpeed, LineAnimationProgress lineAnimationProgress)
        {
            //Know issue: jittery animation in Screenspace: See notes in "GetAnimationProgess_forDashLine()"
            float speedScaling = 20.0f;
            if (lineAnimationProgress == null)
            {
                return (GetTime() * animationSpeed * speedScaling);
            }
            else
            {
                float timeSinceLastCall = GetTime() - lineAnimationProgress.timeOfDraw;
                return lineAnimationProgress.animProgress + (timeSinceLastCall * animationSpeed * speedScaling);
            }
        }

        public static float GetTime()
        {
#if UNITY_EDITOR
            if (Application.isPlaying)
            {
                return Time.time;
            }
            else
            {
                return (float)UnityEditor.EditorApplication.timeSinceStartup;
            }
#else
            return Time.time;
#endif
        }

        static bool CheckIfAnimationProgessIsZeroOrNull(LineAnimationProgress lineAnimationProgress)
        {
            if (lineAnimationProgress == null)
            {
                return true;
            }
            else
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(lineAnimationProgress.animProgress))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        static int AddToDistanceToStartList(float floatToAdd, int i_ofSlotWhereToAdd)
        {
            //function returns "i_nextFreeSlot"
            //function is not ensuring yet if addSlot is the next higher nonExisting-slot
            if (i_ofSlotWhereToAdd < s_subLineAnchor_distanceToStart.Count)
            {
                s_subLineAnchor_distanceToStart[i_ofSlotWhereToAdd] = floatToAdd;
            }
            else
            {
                s_subLineAnchor_distanceToStart.Add(floatToAdd);
            }
            i_ofSlotWhereToAdd++;
            return i_ofSlotWhereToAdd;
        }

        static int RemoveAt_fromDistanceToStartList(int i_toRemove, int i_nextFreeSlot)
        {
            //function returns "i_nextFreeSlot"
            //function is not checking yet if removeSlot already exists
            s_subLineAnchor_distanceToStart.RemoveAt(i_toRemove);
            i_nextFreeSlot--;
            return i_nextFreeSlot;
        }

        static void SetLineAnimProgressToZero(ref LineAnimationProgress lineAnimationProgress)
        {
            if (lineAnimationProgress != null)
            {
                lineAnimationProgress.animProgress = 0.0f;
                lineAnimationProgress.timeOfDraw = 0.0f;
            }
        }

        public static bool CheckIfLineStyleNeedsDefinedAmplitudeForSubLineCreation(DrawBasics.LineStyle styleToCheck)
        {
            switch (styleToCheck)
            {
                case DrawBasics.LineStyle.solid:
                    return false;
                case DrawBasics.LineStyle.invisible:
                    return false;
                case DrawBasics.LineStyle.dotted:
                    return false;
                case DrawBasics.LineStyle.dottedDense:
                    return false;
                case DrawBasics.LineStyle.dottedWide:
                    return false;
                case DrawBasics.LineStyle.dashed:
                    return false;
                case DrawBasics.LineStyle.dashedLong:
                    return false;
                case DrawBasics.LineStyle.dotDash:
                    return false;
                case DrawBasics.LineStyle.dotDashLong:
                    return false;
                case DrawBasics.LineStyle.twoDash:
                    return false;
                case DrawBasics.LineStyle.disconnectedAnchors:
                    return false;
                case DrawBasics.LineStyle.spiral:
                    return true;
                case DrawBasics.LineStyle.sine:
                    return true;
                case DrawBasics.LineStyle.zigzag:
                    return true;
                case DrawBasics.LineStyle.rhombus:
                    return true;
                case DrawBasics.LineStyle.doubleRhombus:
                    return true;
                case DrawBasics.LineStyle.electricNoise:
                    return true;
                case DrawBasics.LineStyle.electricImpulses:
                    return true;
                case DrawBasics.LineStyle.freeHand2D:
                    return true;
                case DrawBasics.LineStyle.freeHand3D:
                    return true;
                case DrawBasics.LineStyle.arrows:
                    return false; //arrows-lines do have a defined amplitude dir, but it is not used for sub line creation
                case DrawBasics.LineStyle.alternatingColorStripes:
                    return false;
                default:
                    return false;
            }
        }

        public static bool CheckIfLineStyleUsesPatternScaling(DrawBasics.LineStyle styleToCheck)
        {
            switch (styleToCheck)
            {
                case DrawBasics.LineStyle.solid:
                    return false;
                case DrawBasics.LineStyle.invisible:
                    return false;
                case DrawBasics.LineStyle.dotted:
                    return true;
                case DrawBasics.LineStyle.dottedDense:
                    return true;
                case DrawBasics.LineStyle.dottedWide:
                    return true;
                case DrawBasics.LineStyle.dashed:
                    return true;
                case DrawBasics.LineStyle.dashedLong:
                    return true;
                case DrawBasics.LineStyle.dotDash:
                    return true;
                case DrawBasics.LineStyle.dotDashLong:
                    return true;
                case DrawBasics.LineStyle.twoDash:
                    return true;
                case DrawBasics.LineStyle.disconnectedAnchors:
                    return false;
                case DrawBasics.LineStyle.spiral:
                    return true;
                case DrawBasics.LineStyle.sine:
                    return true;
                case DrawBasics.LineStyle.zigzag:
                    return true;
                case DrawBasics.LineStyle.rhombus:
                    return true;
                case DrawBasics.LineStyle.doubleRhombus:
                    return true;
                case DrawBasics.LineStyle.electricNoise:
                    return true;
                case DrawBasics.LineStyle.electricImpulses:
                    return true;
                case DrawBasics.LineStyle.freeHand2D:
                    return true;
                case DrawBasics.LineStyle.freeHand3D:
                    return true;
                case DrawBasics.LineStyle.arrows:
                    return true;
                case DrawBasics.LineStyle.alternatingColorStripes:
                    return true;
                default:
                    return false;
            }
        }

        public static bool CheckIfLineStyleIsAnimatable(DrawBasics.LineStyle styleToCheck)
        {
            switch (styleToCheck)
            {
                case DrawBasics.LineStyle.solid:
                    return false;
                case DrawBasics.LineStyle.invisible:
                    return false;
                case DrawBasics.LineStyle.dotted:
                    return true;
                case DrawBasics.LineStyle.dottedDense:
                    return true;
                case DrawBasics.LineStyle.dottedWide:
                    return true;
                case DrawBasics.LineStyle.dashed:
                    return true;
                case DrawBasics.LineStyle.dashedLong:
                    return true;
                case DrawBasics.LineStyle.dotDash:
                    return true;
                case DrawBasics.LineStyle.dotDashLong:
                    return true;
                case DrawBasics.LineStyle.twoDash:
                    return true;
                case DrawBasics.LineStyle.disconnectedAnchors:
                    return true;
                case DrawBasics.LineStyle.spiral:
                    return true;
                case DrawBasics.LineStyle.sine:
                    return true;
                case DrawBasics.LineStyle.zigzag:
                    return true;
                case DrawBasics.LineStyle.rhombus:
                    return true;
                case DrawBasics.LineStyle.doubleRhombus:
                    return true;
                case DrawBasics.LineStyle.electricNoise:
                    return true;
                case DrawBasics.LineStyle.electricImpulses:
                    return true;
                case DrawBasics.LineStyle.freeHand2D:
                    return true;
                case DrawBasics.LineStyle.freeHand3D:
                    return true;
                case DrawBasics.LineStyle.arrows:
                    return true;
                case DrawBasics.LineStyle.alternatingColorStripes:
                    return true;
                default:
                    return false;
            }
        }

        public static DrawBasics.LineStyle FallbackTo2DLineStyle(DrawBasics.LineStyle pot3DLineStyle)
        {
            if (pot3DLineStyle == DrawBasics.LineStyle.spiral)
            {
                return DrawBasics.LineStyle.sine;
            }

            if (pot3DLineStyle == DrawBasics.LineStyle.doubleRhombus)
            {
                return DrawBasics.LineStyle.rhombus;
            }

            if (pot3DLineStyle == DrawBasics.LineStyle.freeHand3D)
            {
                return DrawBasics.LineStyle.freeHand2D;
            }

            return pot3DLineStyle;
        }

    }

}
