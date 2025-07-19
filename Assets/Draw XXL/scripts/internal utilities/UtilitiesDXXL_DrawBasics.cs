namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class UtilitiesDXXL_DrawBasics
    {
        public const int maxMaxAllowedDrawnLinesPerFrame = 2000000; //Raising the value alone may not work in some cases, because "DrawXXL_LinesManager.TryUpdateMesh_fromCachedLines.maxSubMeshesOf65535IndexesEach" also limits the maximum value.
        public static float lowThreshold_ofLineWidth_forNumberOfThinLinesThatComposeTheThickLine = 0.6f / DrawBasics.Density_ofThickLines;
        public static bool useMoreStrutsForFlatPyramidArrow = false;
        static float default_vectorConeAngleDeg = 25.0f; //-> this default value is used for default drawing of vectors. 
        public static float curr_vectorConeAngleDeg = default_vectorConeAngleDeg; //-> Gets temporarily raised when arrows-style lines are drawn.
        public static float min_relConeLengthForVectors = 0.005f;
        public static float max_relConeLengthForVectors = 0.45f;
        public static float min_blinkDurationInSec = 0.02f;
        public static float min_lengthOfStripes_ofAlternatingColorLine = 0.001f;
        public static Vector3 default_default_textOffsetDirection_forPointTags = new Vector3(0.65f, 1.25f, 0.0f);

        public static LineAnimationProgress Line(Vector3 start, Vector3 end, Color color, float width, string text, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, Vector3 customAmplitudeAndTextDir, bool flattenThickRoundLineIntoAmplitudePlane, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects, bool skipPatternEnlargementForLongLines, bool skipPatternEnlargementForShortLines, Camera cameraFrom_DrawScreenspaceCall, bool drawnLineIsFrom_DrawBasics2D, float endPlates_size, float tensionFactor = 1.0f)
        {
            //-> amplitude specified via vector
            return Line(start, end, color, width, text, style, stylePatternScaleFactor, animationSpeed, precedingLineAnimationProgress, null, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines, cameraFrom_DrawScreenspaceCall, drawnLineIsFrom_DrawBasics2D, endPlates_size, tensionFactor);
        }

        public static LineAnimationProgress Line(Vector3 start, Vector3 end, Color color, float width, string text, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, InternalDXXL_Plane preferredAmplitudePlane, bool flattenThickRoundLineIntoAmplitudePlane, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects, bool skipPatternEnlargementForLongLines, bool skipPatternEnlargementForShortLines, Camera cameraFrom_DrawScreenspaceCall, bool drawnLineIsFrom_DrawBasics2D, float endPlates_size, float tensionFactor = 1.0f)
        {
            //-> amplitude specified via plane
            return Line(start, end, color, width, text, style, stylePatternScaleFactor, animationSpeed, precedingLineAnimationProgress, preferredAmplitudePlane, default(Vector3), flattenThickRoundLineIntoAmplitudePlane, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines, cameraFrom_DrawScreenspaceCall, drawnLineIsFrom_DrawBasics2D, endPlates_size, tensionFactor);
        }

        static LineAnimationProgress Line(Vector3 start, Vector3 end, Color color, float width, string text, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, InternalDXXL_Plane preferredAmplitudePlane, Vector3 customAmplitudeAndTextDir, bool flattenThickRoundLineIntoAmplitudePlane, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects, bool skipPatternEnlargementForLongLines, bool skipPatternEnlargementForShortLines, Camera cameraFrom_DrawScreenspaceCall, bool drawnLineIsFrom_DrawBasics2D, float endPlates_size, float tensionFactor)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(alphaFadeOutLength_0to1, "alphaFadeOutLength_0to1")) { return null; } //<- could be refactored to: still draw the line but skip alphaFadeOut
            if (style == DrawBasics.LineStyle.solid && (UtilitiesDXXL_Math.ApproximatelyZero(alphaFadeOutLength_0to1) == false))
            {
                //-> has no colorFade, but alphaFade uses the same approach: coloring the subLines with different alpha
                return DrawLineColorFade_forUsuallyNotSubdividedSolidLines(start, end, color, color, width, text, preferredAmplitudePlane, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, cameraFrom_DrawScreenspaceCall, drawnLineIsFrom_DrawBasics2D, endPlates_size);
            }
            else
            {
                return DrawLine_uniOrMultiColor(start, end, color, default, width, text, style, stylePatternScaleFactor, animationSpeed, precedingLineAnimationProgress, preferredAmplitudePlane, customAmplitudeAndTextDir, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, flattenThickRoundLineIntoAmplitudePlane, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines, cameraFrom_DrawScreenspaceCall, drawnLineIsFrom_DrawBasics2D, endPlates_size, tensionFactor);
            }
        }

        public static LineAnimationProgress LineColorFade(Vector3 start, Vector3 end, Color startColor, Color endColor, float width, string text, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, Vector3 customAmplitudeAndTextDir, bool flattenThickRoundLineIntoAmplitudePlane, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects, bool skipPatternEnlargementForLongLines, bool skipPatternEnlargementForShortLines, Camera cameraFrom_DrawScreenspaceCall, bool drawnLineIsFrom_DrawBasics2D, float endPlates_size, float tensionFactor)
        {
            //-> amplitude specified via vector
            return LineColorFade(start, end, startColor, endColor, width, text, style, stylePatternScaleFactor, animationSpeed, precedingLineAnimationProgress, null, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines, cameraFrom_DrawScreenspaceCall, drawnLineIsFrom_DrawBasics2D, endPlates_size, tensionFactor);
        }

        public static LineAnimationProgress LineColorFade(Vector3 start, Vector3 end, Color startColor, Color endColor, float width, string text, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, InternalDXXL_Plane preferredAmplitudePlane, bool flattenThickRoundLineIntoAmplitudePlane, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects, bool skipPatternEnlargementForLongLines, bool skipPatternEnlargementForShortLines, Camera cameraFrom_DrawScreenspaceCall, bool drawnLineIsFrom_DrawBasics2D, float endPlates_size, float tensionFactor)
        {
            //-> amplitude specified via plane
            return LineColorFade(start, end, startColor, endColor, width, text, style, stylePatternScaleFactor, animationSpeed, precedingLineAnimationProgress, preferredAmplitudePlane, default(Vector3), flattenThickRoundLineIntoAmplitudePlane, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines, cameraFrom_DrawScreenspaceCall, drawnLineIsFrom_DrawBasics2D, endPlates_size, tensionFactor);
        }

        static LineAnimationProgress LineColorFade(Vector3 start, Vector3 end, Color startColor, Color endColor, float width, string text, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, InternalDXXL_Plane preferredAmplitudePlane, Vector3 customAmplitudeAndTextDir, bool flattenThickRoundLineIntoAmplitudePlane, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects, bool skipPatternEnlargementForLongLines, bool skipPatternEnlargementForShortLines, Camera cameraFrom_DrawScreenspaceCall, bool drawnLineIsFrom_DrawBasics2D, float endPlates_size, float tensionFactor)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }

            if (style == DrawBasics.LineStyle.solid)
            {
                return DrawLineColorFade_forUsuallyNotSubdividedSolidLines(start, end, startColor, endColor, width, text, preferredAmplitudePlane, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, cameraFrom_DrawScreenspaceCall, drawnLineIsFrom_DrawBasics2D, endPlates_size);
            }
            else
            {
                return DrawLine_uniOrMultiColor(start, end, startColor, endColor, width, text, style, stylePatternScaleFactor, animationSpeed, precedingLineAnimationProgress, preferredAmplitudePlane, customAmplitudeAndTextDir, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, flattenThickRoundLineIntoAmplitudePlane, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines, cameraFrom_DrawScreenspaceCall, drawnLineIsFrom_DrawBasics2D, endPlates_size, tensionFactor);
            }
        }

        static LineAnimationProgress DrawLine_uniOrMultiColor(Vector3 start, Vector3 end, Color startColor, Color endColor, float width, string text, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, InternalDXXL_Plane preferredAmplitudePlane, Vector3 customAmplitudeAndTextDir, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects, bool flattenThickRoundLineIntoAmplitudePlane, bool skipPatternEnlargementForLongLines, bool skipPatternEnlargementForShortLines, Camera cameraFrom_DrawScreenspaceCall, bool drawnLineIsFrom_DrawBasics2D, float endPlates_size, float tensionFactor)
        {
            //"uni or multicolor" is decided by "ThinLine()"/"ThickLine()/TryDrawPerpEndPlates()", depending on if "endColor" is default or specified.
            //color fade is realized by coloring the sub lines (that each line style anyway produces) individually. 

            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width, "width")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(stylePatternScaleFactor, "stylePatternScaleFactor")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(animationSpeed, "animationSpeed")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(alphaFadeOutLength_0to1, "alphaFadeOutLength_0to1")) { return null; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(end, "end")) { return null; }

            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(start, end))
            {
                //DO NOT fallback to "Point()" here, because "Point()" calls "Line()" again, which can create an endless loop.
                //PointFallback();
                //Debug.Log("'Line' is not drawn, because start and end are at the same position (" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(start) + ").");
                return precedingLineAnimationProgress;
            }

            LineAnimationProgress returned_lineAnimationProgress;
            InternalDXXL_AmplitudeDependentLineDetails amplitudeDependentLineDetails = Get_amplitudeDependentLineDetails(start, end, text, flattenThickRoundLineIntoAmplitudePlane, preferredAmplitudePlane, customAmplitudeAndTextDir, width, enlargeSmallTextToThisMinTextSize, style, endPlates_size, cameraFrom_DrawScreenspaceCall, drawnLineIsFrom_DrawBasics2D);

            if (amplitudeDependentLineDetails.isThinLine)
            {
                returned_lineAnimationProgress = ThinLine(start, end, startColor, endColor, text, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, stylePatternScaleFactor, animationSpeed, precedingLineAnimationProgress, hiddenByNearerObjects, flattenThickRoundLineIntoAmplitudePlane, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines, amplitudeDependentLineDetails, tensionFactor);
            }
            else
            {
                returned_lineAnimationProgress = ThickLine(start, end, startColor, endColor, text, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, stylePatternScaleFactor, animationSpeed, precedingLineAnimationProgress, hiddenByNearerObjects, flattenThickRoundLineIntoAmplitudePlane, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines, amplitudeDependentLineDetails, tensionFactor);
            }

            TryDrawPerpEndPlates(amplitudeDependentLineDetails.uses_endPlates, amplitudeDependentLineDetails.endPlates_size, amplitudeDependentLineDetails.amplitudeUp_normalized, amplitudeDependentLineDetails.lengthOfDrawnLine, amplitudeDependentLineDetails.lengthOfDrawnLine_isFilled, flattenThickRoundLineIntoAmplitudePlane, start, end, startColor, endColor, durationInSec, hiddenByNearerObjects);
            return returned_lineAnimationProgress;
        }

        static InternalDXXL_AmplitudeDependentLineDetails Get_amplitudeDependentLineDetails(Vector3 lineStartPos, Vector3 lineEndPos, string text, bool flattenThickRoundLineIntoAmplitudePlane, InternalDXXL_Plane preferredAmplitudePlane, Vector3 customAmplitudeAndTextDir, float width, float enlargeSmallTextToThisMinTextSize, DrawBasics.LineStyle style, float endPlates_size, Camera cameraFrom_DrawScreenspaceCall, bool drawnLineIsFrom_DrawBasics2D)
        {
            InternalDXXL_AmplitudeDependentLineDetails amplitudeDependentLineDetails = new InternalDXXL_AmplitudeDependentLineDetails();
            width = UtilitiesDXXL_Math.AbsNonZeroValue(width);
            if (width < DrawBasics.thinestPossibleNonZeroWidthLine) { width = 0.0f; }
            amplitudeDependentLineDetails.lineWidth = width;
            amplitudeDependentLineDetails.isThinLine = UtilitiesDXXL_Math.ApproximatelyZero(width);
            amplitudeDependentLineDetails.enlargeSmallText = CheckIfSmallTextGetsEnlarged(enlargeSmallTextToThisMinTextSize);
            amplitudeDependentLineDetails.style = style;
            amplitudeDependentLineDetails.textDrawingIsSkipped_dueToLineIsTooShort = false;

            if (UtilitiesDXXL_Math.FloatIsValid(endPlates_size))
            {
                amplitudeDependentLineDetails.uses_endPlates = (UtilitiesDXXL_Math.ApproximatelyZero(endPlates_size) == false);
                amplitudeDependentLineDetails.endPlates_size = endPlates_size;
            }
            else
            {
                Debug.LogError("The float value 'endPlates_size' is not a valid float, but " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(endPlates_size) + ". End plates drawing is skipped.");
                amplitudeDependentLineDetails.uses_endPlates = false;
                amplitudeDependentLineDetails.endPlates_size = 0.0f;
            }

            Vector3 line_startToEnd = lineEndPos - lineStartPos;
            bool lineIsSoShortThatItDoesntHaveADefinedAmplitude = (UtilitiesDXXL_Math.GetBiggestAbsComponent(line_startToEnd) < UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.shortestLineWithDefinedAmplitudeDir);
            if (lineIsSoShortThatItDoesntHaveADefinedAmplitude)
            {
                //This line parameter forcing has simplifying effect on the succeeding "UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.Get_normalized_amplitudeAndTextDirVectors"-call:
                //-> "to solid" means: no amplitude is required for the line style anymore (side note: line styles that don't have amplitude (like "dashed") could be preserved (instead of beeing forced to "solid"), but when is it needed in this small order of magnitude?)
                //-> "to thin" means: no amplitude is needed for "flattenThickRoundLineIntoAmplitudePlane" or for the "cylidrical hull lines" anymore
                //-> the only occasion that still uses the amplitude vectors is "text with enlargeSmallText" -> this will use the cheap fallback directions, that don't care about perpendicularity with the line.

                amplitudeDependentLineDetails.isThinLine = true;
                amplitudeDependentLineDetails.lineWidth = 0.0f;
                amplitudeDependentLineDetails.style = DrawBasics.LineStyle.solid;
                amplitudeDependentLineDetails.uses_endPlates = false;
                amplitudeDependentLineDetails.endPlates_size = 0.0f;
                if (amplitudeDependentLineDetails.enlargeSmallText == false) { amplitudeDependentLineDetails.textDrawingIsSkipped_dueToLineIsTooShort = true; }
            }
            UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.Get_normalized_amplitudeAndTextDirVectors(out amplitudeDependentLineDetails.amplitudeUp_normalized, out amplitudeDependentLineDetails.textDir_normalized, out amplitudeDependentLineDetails.lengthOfDrawnLine, out amplitudeDependentLineDetails.lengthOfDrawnLine_isFilled, lineStartPos, line_startToEnd, amplitudeDependentLineDetails.isThinLine, lineIsSoShortThatItDoesntHaveADefinedAmplitude, text, amplitudeDependentLineDetails.textDrawingIsSkipped_dueToLineIsTooShort, amplitudeDependentLineDetails.style, flattenThickRoundLineIntoAmplitudePlane, amplitudeDependentLineDetails.uses_endPlates, preferredAmplitudePlane, customAmplitudeAndTextDir, cameraFrom_DrawScreenspaceCall, drawnLineIsFrom_DrawBasics2D);
            return amplitudeDependentLineDetails;
        }

        static bool CheckIfSmallTextGetsEnlarged(float enlargeSmallTextToThisMinTextSize)
        {
            if (UtilitiesDXXL_Math.FloatIsValid(enlargeSmallTextToThisMinTextSize))
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(enlargeSmallTextToThisMinTextSize) == false)
                {
                    return true;
                }
            }
            return false;
        }

        public static void DrawCircleSegment(bool isThinLine, Vector3 segmentStartPos, Vector3 segmentEndPos, Color color, float width, float durationInSec, bool hiddenByNearerObjects, bool flattenThickRoundLineIntoCirclePlane, InternalDXXL_Plane circlePlane)
        {
            Vector3 line_startToEnd = segmentEndPos - segmentStartPos;
            bool lineIsSoShortThatItDoesntHaveADefinedAmplitude = false; //<- "UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.Get_normalized_amplitudeAndTextDirVectors" does anyway not use this in the here applying "GetUpAndTextDir_insideAmplitudePlane"-case and the other cases (which require only "lineNeedsA_perpNormalizedVector_intoAnArbitraryDirection" at most)

            InternalDXXL_AmplitudeDependentLineDetails amplitudeDependentLineDetails = new InternalDXXL_AmplitudeDependentLineDetails();
            amplitudeDependentLineDetails.lineWidth = width;
            amplitudeDependentLineDetails.isThinLine = isThinLine;
            amplitudeDependentLineDetails.enlargeSmallText = false;
            amplitudeDependentLineDetails.textDrawingIsSkipped_dueToLineIsTooShort = false;
            amplitudeDependentLineDetails.style = DrawBasics.LineStyle.solid;
            amplitudeDependentLineDetails.uses_endPlates = false;
            amplitudeDependentLineDetails.endPlates_size = 0.0f;

            if (isThinLine)
            {
                UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.Get_normalized_amplitudeAndTextDirVectors(out amplitudeDependentLineDetails.amplitudeUp_normalized, out amplitudeDependentLineDetails.textDir_normalized, out amplitudeDependentLineDetails.lengthOfDrawnLine, out amplitudeDependentLineDetails.lengthOfDrawnLine_isFilled, segmentStartPos, line_startToEnd, isThinLine, lineIsSoShortThatItDoesntHaveADefinedAmplitude, null, false, DrawBasics.LineStyle.solid, flattenThickRoundLineIntoCirclePlane, false, null, default(Vector3), null, false);
                ThinLine(segmentStartPos, segmentEndPos, color, default, null, 0.0f, 0.0f, durationInSec, 1.0f, 0.0f, null, hiddenByNearerObjects, flattenThickRoundLineIntoCirclePlane, false, false, amplitudeDependentLineDetails, 1.0f);
            }
            else
            {
                if (flattenThickRoundLineIntoCirclePlane)
                {
                    UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.Get_normalized_amplitudeAndTextDirVectors(out amplitudeDependentLineDetails.amplitudeUp_normalized, out amplitudeDependentLineDetails.textDir_normalized, out amplitudeDependentLineDetails.lengthOfDrawnLine, out amplitudeDependentLineDetails.lengthOfDrawnLine_isFilled, segmentStartPos, line_startToEnd, isThinLine, lineIsSoShortThatItDoesntHaveADefinedAmplitude, null, false, DrawBasics.LineStyle.solid, flattenThickRoundLineIntoCirclePlane, false, circlePlane, default(Vector3), null, false);
                    ThickLine(segmentStartPos, segmentEndPos, color, default, null, 0.0f, 0.0f, durationInSec, 1.0f, 0.0f, null, hiddenByNearerObjects, flattenThickRoundLineIntoCirclePlane, false, false, amplitudeDependentLineDetails, 1.0f);
                }
                else
                {
                    UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.Get_normalized_amplitudeAndTextDirVectors(out amplitudeDependentLineDetails.amplitudeUp_normalized, out amplitudeDependentLineDetails.textDir_normalized, out amplitudeDependentLineDetails.lengthOfDrawnLine, out amplitudeDependentLineDetails.lengthOfDrawnLine_isFilled, segmentStartPos, line_startToEnd, isThinLine, lineIsSoShortThatItDoesntHaveADefinedAmplitude, null, false, DrawBasics.LineStyle.solid, flattenThickRoundLineIntoCirclePlane, false, null, default(Vector3), null, false);
                    ThickLine(segmentStartPos, segmentEndPos, color, default, null, 0.0f, 0.0f, durationInSec, 1.0f, 0.0f, null, hiddenByNearerObjects, flattenThickRoundLineIntoCirclePlane, false, false, amplitudeDependentLineDetails, 1.0f);
                }
            }
        }

        static LineAnimationProgress ThinLine(Vector3 start, Vector3 end, Color startColor, Color endColor, string text, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinTextSize, float durationInSec, float stylePatternScaleFactor, float animationSpeed, LineAnimationProgress lineAnimationProgressToUpdate, bool hiddenByNearerObjects, bool flattenThickRoundLineIntoAmplitudePlane, bool skipPatternEnlargementForLongLines, bool skipPatternEnlargementForShortLines, InternalDXXL_AmplitudeDependentLineDetails amplitudeDependentLineDetails, float tensionFactor)
        {
            startColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(startColor);
            Color textColor = startColor; //-> is a separate field so it is not affected by "FlipColors()"
            bool hasColorFade = (UtilitiesDXXL_Colors.IsDefaultColor(endColor) == false);
            if (hasColorFade && (animationSpeed < 0.0f)) { FlipColors(out startColor, out endColor, startColor, endColor); }
            bool hasAlphaFade = (UtilitiesDXXL_Math.ApproximatelyZero(alphaFadeOutLength_0to1) == false);
            if (amplitudeDependentLineDetails.style == DrawBasics.LineStyle.disconnectedAnchors) { hasAlphaFade = false; }
            if (hasAlphaFade) { alphaFadeOutLength_0to1 = Mathf.Clamp(alphaFadeOutLength_0to1, 0.0001f, 0.4999f); }

            float lineStyleAmplitude;
            int usedSlotsInListOfSubLines = UtilitiesDXXL_LineStyles.RefillListOfSubLines(start, end, amplitudeDependentLineDetails.style, stylePatternScaleFactor, 0.0f, out lineStyleAmplitude, amplitudeDependentLineDetails.amplitudeUp_normalized, animationSpeed, ref lineAnimationProgressToUpdate, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines, tensionFactor);

            for (int i_subLineAnchor = 0; i_subLineAnchor < usedSlotsInListOfSubLines; i_subLineAnchor++)
            {
                Color colorOfCurrSubLine = GetColorOfCurrSubLine(startColor, endColor, hasColorFade, hasAlphaFade, alphaFadeOutLength_0to1, usedSlotsInListOfSubLines, i_subLineAnchor);
                if (amplitudeDependentLineDetails.style == DrawBasics.LineStyle.arrows)
                {
                    DrawThinSubLineAsVector(colorOfCurrSubLine, i_subLineAnchor, animationSpeed, flattenThickRoundLineIntoAmplitudePlane, durationInSec, hiddenByNearerObjects);
                }
                else
                {
                    bool lineStyle_is_alternatingColors = (amplitudeDependentLineDetails.style == DrawBasics.LineStyle.alternatingColorStripes);
                    if (lineStyle_is_alternatingColors && CheckIf_alternatingColorStripeIsDrawnForCurrentSubline(i_subLineAnchor, usedSlotsInListOfSubLines))
                    {
                        //AlternatingColorLines fill the gap between the actual subLines:
                        //called BEFORE the main color, because: see explanatin inside "CheckIf_alternatingColorStripeIsDrawnForCurrentSubline"
                        Color usedAlternateColorOfStripedLines = GetColorWithAppliedAlpha_fromSubLines(DrawBasics.defaultColor2_ofAlternatingColorLines, hasAlphaFade, alphaFadeOutLength_0to1, i_subLineAnchor, usedSlotsInListOfSubLines);
                        DXXLWrapperForUntiysBuildInDrawLines.TryDrawLine(UtilitiesDXXL_LineStyles.s_listOfSubLines[i_subLineAnchor], UtilitiesDXXL_LineStyles.s_listOfSubLines[i_subLineAnchor - 1], usedAlternateColorOfStripedLines, durationInSec, hiddenByNearerObjects);
                        if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
                    }

                    DXXLWrapperForUntiysBuildInDrawLines.TryDrawLine(UtilitiesDXXL_LineStyles.s_listOfSubLines[i_subLineAnchor], UtilitiesDXXL_LineStyles.s_listOfSubLines[i_subLineAnchor + 1], colorOfCurrSubLine, durationInSec, hiddenByNearerObjects);
                    if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
                }

                i_subLineAnchor++;
            }

            TagLine(start, end, text, textColor, lineStyleAmplitude, enlargeSmallTextToThisMinTextSize, amplitudeDependentLineDetails, durationInSec, hiddenByNearerObjects);
            return lineAnimationProgressToUpdate;
        }

        static int maxNumberOfSmallLines_thatBuildTheThickLine = 100;
        static Vector3[] startPositions_ofHullLines = new Vector3[maxNumberOfSmallLines_thatBuildTheThickLine];
        static LineAnimationProgress ThickLine(Vector3 start, Vector3 end, Color startColor, Color endColor, string text, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinTextSize, float durationInSec, float stylePatternScaleFactor, float animationSpeed, LineAnimationProgress lineAnimationProgressToUpdate, bool hiddenByNearerObjects, bool flattenThickRoundLineIntoAmplitudePlane, bool skipPatternEnlargementForLongLines, bool skipPatternEnlargementForShortLines, InternalDXXL_AmplitudeDependentLineDetails amplitudeDependentLineDetails, float tensionFactor)
        {
            Vector3 startToEnd = end - start;
            startColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(startColor);
            Color textColor = startColor; //-> is a separate field so it is not affected by "FlipColors()"
            bool hasColorFade = (UtilitiesDXXL_Colors.IsDefaultColor(endColor) == false);
            if (hasColorFade && (animationSpeed < 0.0f)) { FlipColors(out startColor, out endColor, startColor, endColor); }
            bool hasAlphaFade = (UtilitiesDXXL_Math.ApproximatelyZero(alphaFadeOutLength_0to1) == false);
            if (amplitudeDependentLineDetails.style == DrawBasics.LineStyle.disconnectedAnchors) { hasAlphaFade = false; }
            if (hasAlphaFade) { alphaFadeOutLength_0to1 = Mathf.Clamp(alphaFadeOutLength_0to1, 0.0001f, 0.4999f); }

            float lineStyleAmplitude;
            int usedSlotsInListOfSubLines = UtilitiesDXXL_LineStyles.RefillListOfSubLines(start, end, amplitudeDependentLineDetails.style, stylePatternScaleFactor, amplitudeDependentLineDetails.lineWidth, out lineStyleAmplitude, amplitudeDependentLineDetails.amplitudeUp_normalized, animationSpeed, ref lineAnimationProgressToUpdate, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines, tensionFactor);

            bool subLinesAreAssumedToBe45DegToMainLine = ((amplitudeDependentLineDetails.style == DrawBasics.LineStyle.zigzag) || (amplitudeDependentLineDetails.style == DrawBasics.LineStyle.rhombus) || (amplitudeDependentLineDetails.style == DrawBasics.LineStyle.doubleRhombus));
            bool subLinesAreAssumedToBe45DegToMainLineInclinedAlongAmplitudeDir = ((amplitudeDependentLineDetails.style == DrawBasics.LineStyle.zigzag) || (amplitudeDependentLineDetails.style == DrawBasics.LineStyle.rhombus));

            // int numberOfSmallLines_thatBuildTheThickLine = 16 + Mathf.RoundToInt(density_ofThickLines * amplitudeDependentLineDetails.lineWidth);
            int numberOfSmallLines_thatBuildTheThickLine = 4 + Mathf.RoundToInt(12.0f * Mathf.Min(amplitudeDependentLineDetails.lineWidth / lowThreshold_ofLineWidth_forNumberOfThinLinesThatComposeTheThickLine, 1.0f)) + Mathf.RoundToInt(DrawBasics.Density_ofThickLines * amplitudeDependentLineDetails.lineWidth);
            numberOfSmallLines_thatBuildTheThickLine = Mathf.Min(numberOfSmallLines_thatBuildTheThickLine, maxNumberOfSmallLines_thatBuildTheThickLine);

            Vector3 end_ofPrevSubLine = (usedSlotsInListOfSubLines > 0) ? UtilitiesDXXL_LineStyles.s_listOfSubLines[0] : Vector3.zero;
            for (int i_subLineAnchor = 0; i_subLineAnchor < usedSlotsInListOfSubLines; i_subLineAnchor++)
            {
                Vector3 start_ofCurrSubLine = UtilitiesDXXL_LineStyles.s_listOfSubLines[i_subLineAnchor];
                Vector3 end_ofCurrSubLine = UtilitiesDXXL_LineStyles.s_listOfSubLines[i_subLineAnchor + 1];

                if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(start_ofCurrSubLine, end_ofCurrSubLine))
                {
                    i_subLineAnchor++;
                    continue;
                }

                Vector3 startToEnd_ofCurrSubLine = end_ofCurrSubLine - start_ofCurrSubLine;
                if (UtilitiesDXXL_Math.ApproximatelyZero(startToEnd_ofCurrSubLine))
                {
                    i_subLineAnchor++;
                    continue;
                }

                Color colorOfCurrSubLine = GetColorOfCurrSubLine(startColor, endColor, hasColorFade, hasAlphaFade, alphaFadeOutLength_0to1, usedSlotsInListOfSubLines, i_subLineAnchor);

                if (amplitudeDependentLineDetails.style == DrawBasics.LineStyle.arrows)
                {
                    DrawThickSubLineAsVector(colorOfCurrSubLine, start_ofCurrSubLine, end_ofCurrSubLine, animationSpeed, amplitudeDependentLineDetails.lineWidth, flattenThickRoundLineIntoAmplitudePlane, durationInSec, hiddenByNearerObjects);
                }
                else
                {
                    Fill_startPositions_ofHullLines(startToEnd, subLinesAreAssumedToBe45DegToMainLine, subLinesAreAssumedToBe45DegToMainLineInclinedAlongAmplitudeDir, start_ofCurrSubLine, numberOfSmallLines_thatBuildTheThickLine, flattenThickRoundLineIntoAmplitudePlane, amplitudeDependentLineDetails.amplitudeUp_normalized, amplitudeDependentLineDetails.lineWidth);
                    DrawThickSubLine(start_ofCurrSubLine, end_ofCurrSubLine, end_ofPrevSubLine, startToEnd_ofCurrSubLine, colorOfCurrSubLine, i_subLineAnchor, usedSlotsInListOfSubLines, numberOfSmallLines_thatBuildTheThickLine, hasAlphaFade, alphaFadeOutLength_0to1, amplitudeDependentLineDetails.style, durationInSec, hiddenByNearerObjects);
                    if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
                }

                end_ofPrevSubLine = end_ofCurrSubLine;
                i_subLineAnchor++;
            }
            TagLine(start, end, text, textColor, lineStyleAmplitude, enlargeSmallTextToThisMinTextSize, amplitudeDependentLineDetails, durationInSec, hiddenByNearerObjects);
            return lineAnimationProgressToUpdate;
        }

        static void FlipColors(out Color startColor_flipped, out Color endColor_flipped, Color startColor_preFlip, Color endColor_preFlip)
        {
            //-> This corresponds to the behaviour of "UtilitiesDXXL_LineStyles.RefillListOfSubLines()", where the line startPos and endPos get flipped for negative animation speed. 
            //-> This would also lead to flipped colors in lines with color fade
            //-> In such cases the colors are flipped here to re-compensate the start/end-flipping inside "UtilitiesDXXL_LineStyles.RefillListOfSubLines()", so that as a result for negative animation speeds only the anmiation direciton is flipped, but the color fade direction stays.

            startColor_flipped = endColor_preFlip;
            endColor_flipped = startColor_preFlip;
        }

        static void DrawThinSubLineAsVector(Color colorOfCurrSubLine, int i_subLineAnchor, float animationSpeed, bool flattenThickRoundLineIntoAmplitudePlane, float durationInSec, bool hiddenByNearerObjects)
        {
            //note: danger of overwriting the classwide "verticesGlobal" or "verticesLocal" list inside the following "Vector()"-call (which may disturb the function that called this "Line()") has been prevented via "verticesLocal_ofPyramid". For details: See declaration of "verticesLocal_ofPyramid"
            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(UtilitiesDXXL_LineStyles.s_listOfSubLines[i_subLineAnchor], UtilitiesDXXL_LineStyles.s_listOfSubLines[i_subLineAnchor + 1]) == false)
            {
                Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                try
                {
                    UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.theCurrentlyDrawnLines_areAllFrom_vectorsThatBuildUpAnArrowsLine = true;
                    curr_vectorConeAngleDeg = 45.0f;
                    //"back to heading towards lineEnd", because "s_listOfSubLines" is already filled backwards due to negative animation speed:
                    bool flipArrowDirsBackToHeadingTowardsLineEnd = ((animationSpeed < 0.0f) && (UtilitiesDXXL_LineStyles.curr_pointersDirAlongAnimationDir == false));
                    Vector3 vectorStartPos = flipArrowDirsBackToHeadingTowardsLineEnd ? UtilitiesDXXL_LineStyles.s_listOfSubLines[i_subLineAnchor + 1] : UtilitiesDXXL_LineStyles.s_listOfSubLines[i_subLineAnchor];
                    Vector3 vectorEndPos = flipArrowDirsBackToHeadingTowardsLineEnd ? UtilitiesDXXL_LineStyles.s_listOfSubLines[i_subLineAnchor] : UtilitiesDXXL_LineStyles.s_listOfSubLines[i_subLineAnchor + 1];
                    //no amplitudeDir needed, because it gets auto-forced via "UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.theCurrentlyDrawnLines_areAllFrom_vectorsThatBuildUpAnArrowsLine":
                    Vector(vectorStartPos, vectorEndPos, colorOfCurrSubLine, 0.0f, null, 0.45f, false, flattenThickRoundLineIntoAmplitudePlane, false, 0.0f, false, durationInSec, hiddenByNearerObjects, null, default(Vector3), false, 0.0f);
                }
                catch { }
                UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.theCurrentlyDrawnLines_areAllFrom_vectorsThatBuildUpAnArrowsLine = false;
                Reverse_coneLength_interpretation_forStraightVectors();
                curr_vectorConeAngleDeg = default_vectorConeAngleDeg;
            }
        }

        static void DrawThickSubLineAsVector(Color colorOfCurrSubLine, Vector3 start_ofCurrSubLine, Vector3 end_ofCurrSubLine, float animationSpeed, float width, bool flattenThickRoundLineIntoAmplitudePlane, float durationInSec, bool hiddenByNearerObjects)
        {
            Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
            try
            {
                UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.theCurrentlyDrawnLines_areAllFrom_vectorsThatBuildUpAnArrowsLine = true;
                curr_vectorConeAngleDeg = 45.0f;
                //"back to heading towards lineEnd", because "s_listOfSubLines" is already filled backwards due to negative animation speed:
                bool flipArrowDirsBackToHeadingTowardsLineEnd = ((animationSpeed < 0.0f) && (UtilitiesDXXL_LineStyles.curr_pointersDirAlongAnimationDir == false));
                Vector3 vectorStartPos = flipArrowDirsBackToHeadingTowardsLineEnd ? end_ofCurrSubLine : start_ofCurrSubLine;
                Vector3 vectorEndPos = flipArrowDirsBackToHeadingTowardsLineEnd ? start_ofCurrSubLine : end_ofCurrSubLine;
                //note: danger of overwriting the classwide "verticesGlobal" or "verticesLocal" list inside the following "Vector()"-call (which may disturb the function that called this "Line()") has been prevented via "verticesLocal_ofPyramid". For details: See declaration of "verticesLocal_ofPyramid"
                //no amplitudeDir needed, because it gets auto-forced via "UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.theCurrentlyDrawnLines_areAllFrom_vectorsThatBuildUpAnArrowsLine":
                Vector(vectorStartPos, vectorEndPos, colorOfCurrSubLine, width, null, 0.45f, false, flattenThickRoundLineIntoAmplitudePlane, false, 0.0f, false, durationInSec, hiddenByNearerObjects, null, default(Vector3), false, 0.0f);
            }
            catch { }
            UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.theCurrentlyDrawnLines_areAllFrom_vectorsThatBuildUpAnArrowsLine = false;
            Reverse_coneLength_interpretation_forStraightVectors();
            curr_vectorConeAngleDeg = default_vectorConeAngleDeg;
        }

        static void Fill_startPositions_ofHullLines(Vector3 startToEnd_ofMainLine, bool subLineIsAssumedToBe45DegToMainLine, bool subLineIsAssumedToBe45DegToMainLineInclinedAlongAmplitudeDir, Vector3 start_ofCurrSubLine, int numberOfSmallLines_thatBuildTheThickLine, bool flattenThickRoundLineIntoAmplitudePlane, Vector3 amplitudeUp_normalized, float lineWidth)
        {
            //"subLineIsAssumedToBe45DegToMainLine" and "subLineIsAssumedToBe45DegToMainLineInclinedAlongAmplitudeDir" are there for this problem:
            //-> the lineWidth gets expanded along "amplitudeUp_normalized" (which is the same for the whole mainLine). 
            //-> so if a subLine is inclined to the main line it gets a parallax shortening
            //-> this results in an unconsistent lineWidth e.g. for "sine" lineStyle
            //-> it is compenstated quiet easy/cheap at least for zigZag/rhomubs lineStyles because it can be assumed that the inclination angle is 45° there (which though is not always the case in edge cases)
            //-> it is tolerated for other lineStyles
            //-> the "correct" way would be to calculate "amplitudeUp_normalized" per subLine, which would be much more expensive
            //---> this "correct way" also has the disadvantage, that non-zeroWidth zigZag-lineStyles have a "dent" at the 90°-corners joins
            //---> these "dents" could also be prevented by extending the subLines inside "UtilitiesDXXL_LineStyles.GetListOfSubLines_forZigZagLine()", but the solution here leads to a much nicer appearance

            if (flattenThickRoundLineIntoAmplitudePlane)
            {
                Vector3 vector_fromLineLowerEnd_toUpperEnd = amplitudeUp_normalized * lineWidth;
                if (subLineIsAssumedToBe45DegToMainLine)
                {
                    vector_fromLineLowerEnd_toUpperEnd = vector_fromLineLowerEnd_toUpperEnd * UtilitiesDXXL_Math.sqrtOf2_precalced;
                }
                Vector3 lineLowerEnd = start_ofCurrSubLine - (0.5f * vector_fromLineLowerEnd_toUpperEnd);
                Vector3 vector_fromFlatenedLineToNeighborFlatenedLine = vector_fromLineLowerEnd_toUpperEnd / (numberOfSmallLines_thatBuildTheThickLine - 1);
                for (int i_hullLine = 0; i_hullLine < numberOfSmallLines_thatBuildTheThickLine; i_hullLine++)
                {
                    startPositions_ofHullLines[i_hullLine] = lineLowerEnd + vector_fromFlatenedLineToNeighborFlatenedLine * i_hullLine;
                }
            }
            else
            {
                float thickLineRadius = 0.5f * lineWidth;
                Vector3 vector_fromStartPos_toStartPosOfFirstHullLine = amplitudeUp_normalized * thickLineRadius;
                float angleDeg_betweenHullLines = 360.0f / numberOfSmallLines_thatBuildTheThickLine;

                if (subLineIsAssumedToBe45DegToMainLine)
                {
                    for (int i_hullLine = 0; i_hullLine < numberOfSmallLines_thatBuildTheThickLine; i_hullLine++)
                    {
                        Quaternion rotation_fromFirstHullLineStartPos_toCurrentHullLineStartPos = Quaternion.AngleAxis(angleDeg_betweenHullLines * i_hullLine, startToEnd_ofMainLine);
                        float offsetScaleFactor;
                        if (subLineIsAssumedToBe45DegToMainLineInclinedAlongAmplitudeDir)
                        {
                            float progress0to1_throughWholeHullCircle = ((float)i_hullLine / (float)numberOfSmallLines_thatBuildTheThickLine);
                            offsetScaleFactor = 1.0f + UtilitiesDXXL_Math.sqrtOf2_precalced_minus1 * GetHullLinesModulationFactor_whichComponesatesTheDeformationOf45DegSublines(progress0to1_throughWholeHullCircle);
                        }
                        else
                        {
                            // doubleRhombus doesn't get that correction and therefore remains with uneven line intersection extents and not fully fitting the specified "lineWidth"
                            offsetScaleFactor = 1.0f;
                        }
                        startPositions_ofHullLines[i_hullLine] = start_ofCurrSubLine + rotation_fromFirstHullLineStartPos_toCurrentHullLineStartPos * (vector_fromStartPos_toStartPosOfFirstHullLine * offsetScaleFactor);
                    }
                }
                else
                {
                    for (int i_hullLine = 0; i_hullLine < numberOfSmallLines_thatBuildTheThickLine; i_hullLine++)
                    {
                        //  Quaternion rotation_fromFirstHullLineStartPos_toCurrentHullLineStartPos = Quaternion.AngleAxis(angleDeg_betweenHullLines * i_hullLine, startToEnd_ofCurrSubLine);
                        Quaternion rotation_fromFirstHullLineStartPos_toCurrentHullLineStartPos = Quaternion.AngleAxis(angleDeg_betweenHullLines * i_hullLine, startToEnd_ofMainLine);
                        startPositions_ofHullLines[i_hullLine] = start_ofCurrSubLine + rotation_fromFirstHullLineStartPos_toCurrentHullLineStartPos * vector_fromStartPos_toStartPosOfFirstHullLine;
                    }
                }
            }
        }

        static float GetHullLinesModulationFactor_whichComponesatesTheDeformationOf45DegSublines(float progress0to1_throughWholeHullCircle)
        {
            //this is only an approximation:
            float progress0to4_throughWholeHullCircle = 4.0f * progress0to1_throughWholeHullCircle;
            if (progress0to4_throughWholeHullCircle < 1.0f)
            {
                return (1.0f - progress0to4_throughWholeHullCircle);
            }
            else
            {
                if (progress0to4_throughWholeHullCircle < 2.0f)
                {
                    return (progress0to4_throughWholeHullCircle - 1.0f);
                }
                else
                {
                    if (progress0to4_throughWholeHullCircle < 3.0f)
                    {
                        return (3.0f - progress0to4_throughWholeHullCircle);
                    }
                    else
                    {
                        return (progress0to4_throughWholeHullCircle - 3.0f);
                    }
                }
            }
        }

        static void DrawThickSubLine(Vector3 start_ofCurrSubLine, Vector3 end_ofCurrSubLine, Vector3 end_ofPrevSubLine, Vector3 startToEnd_ofCurrSubLine, Color colorOfCurrSubLine, int i_subLineAnchor, int usedSlotsInListOfSubLines, int numberOfSmallLines_thatBuildTheThickLine, bool hasAlphaFade, float alphaFadeOutLength_0to1, DrawBasics.LineStyle lineStyle, float durationInSec, bool hiddenByNearerObjects)
        {
            bool lineStyle_is_alternatingColors = (lineStyle == DrawBasics.LineStyle.alternatingColorStripes);
            Color usedAlternateColorOfStripedLines = default(Color);
            bool drawAlternatingColorStripe = false;
            if (lineStyle_is_alternatingColors)
            {
                usedAlternateColorOfStripedLines = GetColorWithAppliedAlpha_fromSubLines(DrawBasics.defaultColor2_ofAlternatingColorLines, hasAlphaFade, alphaFadeOutLength_0to1, i_subLineAnchor, usedSlotsInListOfSubLines);
                drawAlternatingColorStripe = CheckIf_alternatingColorStripeIsDrawnForCurrentSubline(i_subLineAnchor, usedSlotsInListOfSubLines);
            }

            //hull lines:
            //(cylindrical around and parallel to the central line to simulate a "width" of the line)
            for (int i_hullLine = 0; i_hullLine < numberOfSmallLines_thatBuildTheThickLine; i_hullLine++)
            {
                if (drawAlternatingColorStripe)
                {
                    //AlternatingColorLines fill the gap between the actual subLines:
                    //called BEFORE the main color, because: see explanatin inside "DrawAlternatingColorStripe"
                    Vector3 startOfCurr_to_endOfPrevSubLine = end_ofPrevSubLine - start_ofCurrSubLine;
                    DXXLWrapperForUntiysBuildInDrawLines.TryDrawLine(startPositions_ofHullLines[i_hullLine], startPositions_ofHullLines[i_hullLine] + startOfCurr_to_endOfPrevSubLine, usedAlternateColorOfStripedLines, durationInSec, hiddenByNearerObjects);
                }
                DXXLWrapperForUntiysBuildInDrawLines.TryDrawLine(startPositions_ofHullLines[i_hullLine], startPositions_ofHullLines[i_hullLine] + startToEnd_ofCurrSubLine, colorOfCurrSubLine, durationInSec, hiddenByNearerObjects);
            }

            //central line:
            if (drawAlternatingColorStripe)
            {
                //AlternatingColorLines fill the gap between the actual subLines:
                //called BEFORE the main color, because: see explanatin inside "CheckIf_alternatingColorStripeIsDrawnForCurrentSubline"
                DXXLWrapperForUntiysBuildInDrawLines.TryDrawLine(start_ofCurrSubLine, end_ofPrevSubLine, usedAlternateColorOfStripedLines, durationInSec, hiddenByNearerObjects);
            }
            DXXLWrapperForUntiysBuildInDrawLines.TryDrawLine(start_ofCurrSubLine, end_ofCurrSubLine, colorOfCurrSubLine, durationInSec, hiddenByNearerObjects);
        }

        static Color GetColorOfCurrSubLine(Color startColor, Color endColor, bool hasColorFade, bool hasAlphaFade, float alphaFadeOutLength_0to1, int usedSlotsInListOfSubLines, int i_subLineAnchor)
        {
            Color color;
            if (hasColorFade)
            {
                color = GetFadedColorFromSubLines(startColor, endColor, usedSlotsInListOfSubLines, i_subLineAnchor);
            }
            else
            {
                color = startColor;
            }
            return GetColorWithAppliedAlpha_fromSubLines(color, hasAlphaFade, alphaFadeOutLength_0to1, i_subLineAnchor, usedSlotsInListOfSubLines);
        }

        static LineAnimationProgress DrawLineColorFade_forUsuallyNotSubdividedSolidLines(Vector3 start, Vector3 end, Color startColor, Color endColor, float width, string text, InternalDXXL_Plane preferredAmplitudePlane, Vector3 customAmplitudeAndTextDir, bool flattenThickRoundLineIntoAmplitudePlane, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects, Camera cameraFrom_DrawScreenspaceCall, bool drawnLineIsFrom_DrawBasics2D, float endPlates_size)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width, "width")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(alphaFadeOutLength_0to1, "alphaFadeOutLength_0to1")) { return null; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(end, "end")) { return null; }

            width = UtilitiesDXXL_Math.AbsNonZeroValue(width);
            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(start, end))
            {
                //-> no "PointFallback()" in case of lines
                //-> at least there would not be the danger of endless loops here as it is in "DrawLine_uniOrMultiColor()"
                //Debug.Log("'LineColorFade' is not drawn, because start and end are at the same position (" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(start) + ").");
                return null;
            }

            int numberOfSubLineSegments = 24;
            bool hasAlphaFade = (UtilitiesDXXL_Math.ApproximatelyZero(alphaFadeOutLength_0to1) == false);
            if (hasAlphaFade)
            {
                alphaFadeOutLength_0to1 = Mathf.Clamp(alphaFadeOutLength_0to1, 0.0001f, 0.4999f);
                numberOfSubLineSegments = 48;
            }

            Vector3 startToEnd = end - start;
            Vector3 lineNormalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(startToEnd, out float lineLength);
            float segmentLenght = lineLength / (float)numberOfSubLineSegments;
            Vector3 subLineSegment = lineNormalized * segmentLenght;
            InternalDXXL_AmplitudeDependentLineDetails amplitudeDependentLineDetails = Get_amplitudeDependentLineDetails(start, end, text, flattenThickRoundLineIntoAmplitudePlane, preferredAmplitudePlane, customAmplitudeAndTextDir, width, enlargeSmallTextToThisMinTextSize, DrawBasics.LineStyle.solid, endPlates_size, cameraFrom_DrawScreenspaceCall, drawnLineIsFrom_DrawBasics2D);
            DrawSubLines_of_subdividedSolidLine(start, end, startColor, endColor, numberOfSubLineSegments, subLineSegment, hasAlphaFade, alphaFadeOutLength_0to1, durationInSec, hiddenByNearerObjects, flattenThickRoundLineIntoAmplitudePlane, amplitudeDependentLineDetails);
            TryDrawPerpEndPlates(amplitudeDependentLineDetails.uses_endPlates, amplitudeDependentLineDetails.endPlates_size, amplitudeDependentLineDetails.amplitudeUp_normalized, amplitudeDependentLineDetails.lengthOfDrawnLine, amplitudeDependentLineDetails.lengthOfDrawnLine_isFilled, flattenThickRoundLineIntoAmplitudePlane, start, end, startColor, endColor, durationInSec, hiddenByNearerObjects);
            TagLine(start, end, text, startColor, 0.0f, enlargeSmallTextToThisMinTextSize, amplitudeDependentLineDetails, durationInSec, hiddenByNearerObjects);
            return null;
        }

        static void DrawSubLines_of_subdividedSolidLine(Vector3 start, Vector3 end, Color startColor, Color endColor, int numberOfSubLineSegments, Vector3 subLineSegment, bool hasAlphaFade, float alphaFadeOutLength_0to1, float durationInSec, bool hiddenByNearerObjects, bool flattenThickRoundLineIntoAmplitudePlane, InternalDXXL_AmplitudeDependentLineDetails amplitudeDependentLineDetails)
        {
            for (int i_segment = 0; i_segment < numberOfSubLineSegments; i_segment++)
            {
                Vector3 segmentStartPos = start + i_segment * subLineSegment;
                Vector3 segmentEndPos = start + (i_segment + 1) * subLineSegment;
                Color segmentColor;
                if (i_segment == (numberOfSubLineSegments - 1))
                {
                    segmentEndPos = end;
                    segmentColor = endColor;
                }
                else
                {
                    segmentColor = GetFadedColorFromSegments(startColor, endColor, i_segment, numberOfSubLineSegments);
                }
                segmentColor = GetColorWithAppliedAlpha_fromSegments(segmentColor, hasAlphaFade, alphaFadeOutLength_0to1, i_segment, numberOfSubLineSegments);

                if (amplitudeDependentLineDetails.isThinLine)
                {
                    ThinLine(segmentStartPos, segmentEndPos, segmentColor, default, null, 0.0f, 0.0f, durationInSec, 1.0f, 0.0f, null, hiddenByNearerObjects, flattenThickRoundLineIntoAmplitudePlane, false, false, amplitudeDependentLineDetails, 1.0f);
                }
                else
                {
                    ThickLine(segmentStartPos, segmentEndPos, segmentColor, default, null, 0.0f, 0.0f, durationInSec, 1.0f, 0.0f, null, hiddenByNearerObjects, flattenThickRoundLineIntoAmplitudePlane, false, false, amplitudeDependentLineDetails, 1.0f);
                }
            }
        }

        static void TagLine(Vector3 start, Vector3 end, string text, Color color, float lineStyleAmplitude, float enlargeSmallTextToThisMinTextSize, InternalDXXL_AmplitudeDependentLineDetails amplitudeDependentLineDetails, float durationInSec, bool hiddenByNearerObjects)
        {
            if (amplitudeDependentLineDetails.textDrawingIsSkipped_dueToLineIsTooShort == false)
            {
                if (text != null && text != "")
                {
                    float halfLineWidth = 0.5f * amplitudeDependentLineDetails.lineWidth;
                    Vector3 startToEnd = end - start;
                    //"middleOfLine" could also be reused from "UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.GetLineCenter", but is most likely not relevant for performance/not worth the required dependency.
                    Vector3 middleOfLine = 0.5f * (start + end);
                    //text is not drawn here, but the "Write"-call is only for filling "parsedTextSpecs":
                    UtilitiesDXXL_Text.Write(text, Vector3.zero, default, 1.0f, default, default, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, false, durationInSec, hiddenByNearerObjects, true, true, false); //"isFrom_Write2D" enabled, because has no effect, but is cheaper 
                    int numberOfChars_inLongestLine_ifSizeIs1 = DrawText.parsedTextSpecs.numberOfChars_inLongestLine;
                    float lengthOfLongestLine_ifSizeIs1 = DrawText.parsedTextSpecs.widthOfLongestLine;
                    if (numberOfChars_inLongestLine_ifSizeIs1 <= 0)
                    {
                        //UtilitiesDXXL_Log.PrintErrorCode("13-" + numberOfChars_inLongestLine_ifSizeIs1); //-> it may validly happen, e.g. when a string without content characters, but with markup tags (like "<color=red></color>") is supplied
                        return;
                    }
                    float maxTextBlockWidth_0to1 = DrawBasics.RelSizeOfTextOnLines;
                    float textSizeShrinkingFactor_dueToLowCharNumber_0to1 = 1.0f;
                    int charNumberThreshold_belowWhichShrinkingHappens = 6;
                    if (numberOfChars_inLongestLine_ifSizeIs1 < charNumberThreshold_belowWhichShrinkingHappens)
                    {
                        textSizeShrinkingFactor_dueToLowCharNumber_0to1 = (float)numberOfChars_inLongestLine_ifSizeIs1 / (float)charNumberThreshold_belowWhichShrinkingHappens;
                    }

                    float lengthOfDrawnLine = GetLengthOfDrawnLine(amplitudeDependentLineDetails.lengthOfDrawnLine, amplitudeDependentLineDetails.lengthOfDrawnLine_isFilled, startToEnd);
                    float finalWidthOfTextBlock = lengthOfDrawnLine * maxTextBlockWidth_0to1 * textSizeShrinkingFactor_dueToLowCharNumber_0to1;
                    float sizeOfScaledText = finalWidthOfTextBlock / lengthOfLongestLine_ifSizeIs1;
                    Vector3 textStartPos_onLine = middleOfLine - 0.5f * finalWidthOfTextBlock * amplitudeDependentLineDetails.textDir_normalized;
                    Vector3 textStartPos = textStartPos_onLine + amplitudeDependentLineDetails.amplitudeUp_normalized * (halfLineWidth + lineStyleAmplitude + 0.7f * sizeOfScaledText);

                    if (amplitudeDependentLineDetails.enlargeSmallText)
                    {
                        // if (UtilitiesDXXL_Log.ErroLogForInvalidFloats(enlargeSmallTextToThisMinTextSize, "enlargeSmallTextToThisMinTextSize")) { return; } <- enlargeSmallTextToThisMinTextSize has already been checked for validity during the "enlargeSmallText"-derivation (inside "CheckIfSmallTextGetsEnlarged()")
                        enlargeSmallTextToThisMinTextSize = UtilitiesDXXL_Math.AbsNonZeroValue(enlargeSmallTextToThisMinTextSize);
                        sizeOfScaledText = Mathf.Max(sizeOfScaledText, enlargeSmallTextToThisMinTextSize);
                        maxTextBlockWidth_0to1 = 100000.0f;
                    }

                    DrawText.TextAnchorDXXL textAnchor = DrawBasics.shiftTextPosOnLines_toNonIntersecting ? DrawText.TextAnchorDXXL.LowerLeft : DrawText.TextAnchorDXXL.LowerLeftOfFirstLine;
                    UtilitiesDXXL_Text.Write(text, textStartPos, color, sizeOfScaledText, amplitudeDependentLineDetails.textDir_normalized, amplitudeDependentLineDetails.amplitudeUp_normalized, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, lengthOfDrawnLine * maxTextBlockWidth_0to1, 0.0f, false, durationInSec, hiddenByNearerObjects, false, false, true);
                }
            }
        }

        static float GetLengthOfDrawnLine(float lengthOfDrawnLine_fromAmplitudeDependentLineDetails, bool lengthOfDrawnLine_isFilledInsideAmplitudeDependentLineDetails, Vector3 startToEnd)
        {
            if (lengthOfDrawnLine_isFilledInsideAmplitudeDependentLineDetails)
            {
                return lengthOfDrawnLine_fromAmplitudeDependentLineDetails;
            }
            else
            {
                return startToEnd.magnitude;
            }
        }

        static bool CheckIf_alternatingColorStripeIsDrawnForCurrentSubline(int i_subLineAnchor, int usedSlotsInListOfSubLines)
        {
            //  if (i_subLineAnchor > 3) //skip first two segments (one is the additional unanimated segment at linestart, the other is the real first segment which doesn't have a gap to the non-existing preceding segment) <- This variant leaves a gap sometimes->See commment in next line
            if (i_subLineAnchor > 1) //skip the first segment (which is the additional unanimated segment at lineStart). The second segment (which is the first animated one) is not skipped despite it often (in all unanimated cases) starts at lineStart, which causes a stripeSegment to be drawn from lineStart. It cannot be skipped, becuase this second (first animated) segment sometimes starts after the first fixed segment, which would lead a gap without the stripe color. This stripeSegment_fromLineStart is drawn before the mainColor and therefore gets overdrawn by the mainColor, so it doesn't disturb the apperance
            {
                if (i_subLineAnchor < (usedSlotsInListOfSubLines - 2)) //skip last segment which is the additional unanimated segment at lineEnd.
                {
                    return true;
                }
            }
            return false;
        }

        public static void PointFallback(Vector3 position, string text, Color color, float markingCrossLinesWidth, float durationInSec, bool hiddenByNearerObjects)
        {
            Point(false, position, text, color, 0.5f, markingCrossLinesWidth, color, Quaternion.identity, true, true, false, true, Vector3.zero, Quaternion.identity, Vector3.one, false, durationInSec, hiddenByNearerObjects);
        }

        public static void Point(bool is2D, Vector3 localPosition, string text, Color textColor, float sizeOfMarkingCross, float markingCrossLinesWidth, Color markingCrossColor, Quaternion localRotation, bool pointer_as_textAttachStyle, bool drawCoordsAsText, bool additionallyDrawGlobalCoords, bool coordSystemIsGlobalNotLocal, Vector3 parentPositionGlobal, Quaternion parentRotationGlobal, Vector3 parentScaleGlobal, bool hideZDir, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(sizeOfMarkingCross, "sizeOfMarkingCross")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(markingCrossLinesWidth, "markingCrossLinesWidth")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(localPosition, "localPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(parentPositionGlobal, "parentPositionGlobal")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(parentScaleGlobal, "parentScaleGlobal")) { return; }

            localRotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(localRotation);
            bool isZeroParentRotation = (parentRotationGlobal == Quaternion.identity);
            bool isZeroLocalRotation = (localRotation == Quaternion.identity);
            markingCrossLinesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(markingCrossLinesWidth);
            float halfAbsMarkingCrossLinesWidth = 0.5f * markingCrossLinesWidth;
            sizeOfMarkingCross = Mathf.Abs(sizeOfMarkingCross);
            sizeOfMarkingCross = UtilitiesDXXL_Math.Max(sizeOfMarkingCross, 4.0f * markingCrossLinesWidth, 0.01f);
            float halfMarkingCrossExtent = 0.5f * sizeOfMarkingCross;

            Color color_x = UtilitiesDXXL_Colors.red_xAxis;
            Color color_y = UtilitiesDXXL_Colors.green_yAxis;
            Color color_z = UtilitiesDXXL_Colors.blue_zAxis;
            if (UtilitiesDXXL_Colors.IsDefaultColor(markingCrossColor) == false)
            {
                color_x = markingCrossColor;
                color_y = markingCrossColor;
                color_z = markingCrossColor;
            }

            Vector3 parentForward_normalized = Vector3.forward;
            Vector3 parentUp_normalized = Vector3.up;
            Vector3 parentRight_normalized = Vector3.right;

            if (isZeroParentRotation == false)
            {
                parentForward_normalized = parentRotationGlobal * Vector3.forward;
                parentUp_normalized = parentRotationGlobal * Vector3.up;
                parentRight_normalized = parentRotationGlobal * Vector3.right;
            }

            Vector3 locallyRotatedForward_normalized = parentForward_normalized;
            Vector3 locallyRotatedUp_normalized = parentUp_normalized;
            Vector3 locallyRotatedRight_normalized = parentRight_normalized;

            if (isZeroLocalRotation == false)
            {
                locallyRotatedForward_normalized = localRotation * parentForward_normalized;
                locallyRotatedUp_normalized = localRotation * parentUp_normalized;
                locallyRotatedRight_normalized = localRotation * parentRight_normalized;
            }

            Vector3 worldPosition = localPosition;
            if (UtilitiesDXXL_Math.ApproximatelyZero(parentPositionGlobal) == false)
            {
                worldPosition = parentPositionGlobal + parentRight_normalized * parentScaleGlobal.x * localPosition.x + parentUp_normalized * parentScaleGlobal.y * localPosition.y + parentForward_normalized * parentScaleGlobal.z * localPosition.z;
            }

            if (hideZDir == false)
            {
                Line_fadeableAnimSpeed.InternalDraw(worldPosition - locallyRotatedForward_normalized * halfMarkingCrossExtent, worldPosition + locallyRotatedForward_normalized * halfMarkingCrossExtent, color_z, markingCrossLinesWidth, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }
            Line_fadeableAnimSpeed.InternalDraw(worldPosition - locallyRotatedUp_normalized * halfMarkingCrossExtent, worldPosition + locallyRotatedUp_normalized * halfMarkingCrossExtent, color_y, markingCrossLinesWidth, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, is2D, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(worldPosition - locallyRotatedRight_normalized * halfMarkingCrossExtent, worldPosition + locallyRotatedRight_normalized * halfMarkingCrossExtent, color_x, markingCrossLinesWidth, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, is2D, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            if (markingCrossLinesWidth > 0.0f)
            {
                if (is2D)
                {
                    //"DrawShapes.Circle" is allowed to fallback to point (for edge cases) because this fallback will be called with "is2D == false", therefore no danger of endless loop.
                    DrawShapes.Circle(worldPosition, halfAbsMarkingCrossLinesWidth, Color.white, Vector3.forward, default(Vector3), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, true, false, durationInSec, hiddenByNearerObjects);
                }
                else
                {
                    int numberOfSphereStruts = 8;
                    DrawShapes.Sphere(worldPosition, halfAbsMarkingCrossLinesWidth, Color.white, locallyRotatedUp_normalized, locallyRotatedForward_normalized, 0.0f, null, numberOfSphereStruts, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);
                }
            }

            DrawCoordsAsText(drawCoordsAsText, coordSystemIsGlobalNotLocal, worldPosition, localPosition, sizeOfMarkingCross, halfAbsMarkingCrossLinesWidth, color_x, color_y, color_z, parentRotationGlobal, isZeroLocalRotation, markingCrossLinesWidth, hideZDir, additionallyDrawGlobalCoords, durationInSec, hiddenByNearerObjects);

            if (text != null && text != "")
            {
                if (pointer_as_textAttachStyle)
                {
                    float widthOfLinesTowardsText = 0.3f * markingCrossLinesWidth;
                    if (is2D)
                    {
                        Vector2 positionV2 = new Vector2(localPosition.x, localPosition.y);
                        DrawBasics2D.PointTag(positionV2, text, textColor, widthOfLinesTowardsText, 1.5f * sizeOfMarkingCross, default(Vector2), localPosition.z, 1.0f, false, durationInSec, hiddenByNearerObjects);
                    }
                    else
                    {
                        DrawBasics.PointTag(worldPosition, text, textColor, widthOfLinesTowardsText, 1.5f * sizeOfMarkingCross, default(Vector3), 1.0f, false, durationInSec, hiddenByNearerObjects);
                    }
                }
                else
                {
                    float textSize = 0.25f * sizeOfMarkingCross;
                    float spacingFromMarkerCross = 0.1f * textSize;
                    Vector3 textPos = worldPosition - locallyRotatedUp_normalized * (1.1f * textSize + halfAbsMarkingCrossLinesWidth) + locallyRotatedRight_normalized * (spacingFromMarkerCross + halfAbsMarkingCrossLinesWidth);
                    UtilitiesDXXL_Text.Write(text, textPos, textColor, textSize, locallyRotatedRight_normalized, locallyRotatedUp_normalized, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                }
            }
        }

        static void DrawCoordsAsText(bool drawCoordsAsText, bool coordSystemIsGlobalNotLocal, Vector3 worldPosition, Vector3 localPosition, float markingCrossExtent, float halfAbsMarkerLinesWidth, Color color_x, Color color_y, Color color_z, Quaternion parentRotationGlobal, bool isZeroLocalRotation, float markingCrossLinesWidth, bool hideZDir, bool additionallyDrawGlobalCoords, float durationInSec, bool hiddenByNearerObjects)
        {
            if (drawCoordsAsText)
            {
                if (coordSystemIsGlobalNotLocal)
                {
                    DrawCoordAxesAtPoint(worldPosition, worldPosition, markingCrossExtent, false, halfAbsMarkerLinesWidth, color_x, color_y, color_z, !isZeroLocalRotation, false, false, Quaternion.identity, false, markingCrossLinesWidth, hideZDir, durationInSec, hiddenByNearerObjects);
                }
                else
                {
                    if (additionallyDrawGlobalCoords)
                    {
                        float doubleShrinkFactor_ofLocalPointsGlobalCoordsText = 1.0f;
                        if (isZeroLocalRotation == false)
                        {
                            doubleShrinkFactor_ofLocalPointsGlobalCoordsText = 0.5f;
                        }
                        DrawCoordAxesAtPoint(worldPosition, worldPosition, markingCrossExtent * doubleShrinkFactor_ofLocalPointsGlobalCoordsText, true, halfAbsMarkerLinesWidth, color_x, color_y, color_z, true, true, false, Quaternion.identity, true, markingCrossLinesWidth, hideZDir, durationInSec, hiddenByNearerObjects);
                    }
                    DrawCoordAxesAtPoint(worldPosition, localPosition, markingCrossExtent, false, halfAbsMarkerLinesWidth, color_x, color_y, color_z, !isZeroLocalRotation, true, true, parentRotationGlobal, false, markingCrossLinesWidth, hideZDir, durationInSec, hiddenByNearerObjects);
                }
            }
        }

        static string globalCoordSystemSpecifyingText = "<size=2>global</size>";
        static string localCoordSystemSpecifyingText = "<size=2>local</size>";
        static void DrawCoordAxesAtPoint(Vector3 drawPosition, Vector3 positionCoordsToWrite, float markingCrossExtent, bool shrinkToSmaller, float halfAbsMarkerLinesWidth, Color color_x, Color color_y, Color color_z, bool drawThinButWithOwnAxisLines, bool drawCoordSystemSpecifyingString, bool coordSystemSpecifyingStringIsLocal, Quaternion rotation, bool coordTextBelowAxes, float markingCrossLinesWidth, bool hideZDir, float durationInSec, bool hiddenByNearerObjects)
        {
            float halfMarkingCrossExtent = 0.5f * markingCrossExtent;
            float coordsTextSize = 0.05f * markingCrossExtent;
            float spacingFromMarkerCross = 1.75f * coordsTextSize;
            if (shrinkToSmaller)
            {
                coordsTextSize = coordsTextSize * 0.4f;
            }
            float spacingAboveLine = 0.5f * coordsTextSize;
            float maxTextLength = halfMarkingCrossExtent - (spacingFromMarkerCross + halfAbsMarkerLinesWidth);
            float coordSystemSpecifier_alpha = 0.4f;
            bool isZeroRotation = (rotation == Quaternion.identity);

            Vector3 forward_normalized = Vector3.forward;
            Vector3 up_normalized = Vector3.up;
            Vector3 right_normalized = Vector3.right;
            if (isZeroRotation == false)
            {
                forward_normalized = rotation * Vector3.forward;
                up_normalized = rotation * Vector3.up;
                right_normalized = rotation * Vector3.right;
            }

            if (drawThinButWithOwnAxisLines)
            {
                coordSystemSpecifier_alpha = 0.8f;
                float coordsTextAndThinLinesAlpha = 0.5f;
                if (markingCrossLinesWidth > 0.0f)
                {
                    coordsTextAndThinLinesAlpha = 0.6f;
                }
                color_x = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_x, coordsTextAndThinLinesAlpha);
                color_y = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_y, coordsTextAndThinLinesAlpha);
                color_z = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_z, coordsTextAndThinLinesAlpha);

                float lenghtOfThinHalfLine = halfMarkingCrossExtent;
                if (shrinkToSmaller)
                {
                    lenghtOfThinHalfLine = lenghtOfThinHalfLine * 0.73f;
                }

                if (hideZDir == false)
                {
                    Line_fadeableAnimSpeed.InternalDraw(drawPosition, drawPosition + forward_normalized * lenghtOfThinHalfLine, color_z, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                }
                Line_fadeableAnimSpeed.InternalDraw(drawPosition, drawPosition + up_normalized * lenghtOfThinHalfLine, color_y, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                Line_fadeableAnimSpeed.InternalDraw(drawPosition, drawPosition + right_normalized * lenghtOfThinHalfLine, color_x, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            ///z:
            if (hideZDir == false)
            {
                Vector3 zTextPos;
                if (coordTextBelowAxes)
                {
                    zTextPos = drawPosition - up_normalized * (spacingAboveLine + halfAbsMarkerLinesWidth + coordsTextSize) + forward_normalized * (spacingFromMarkerCross + halfAbsMarkerLinesWidth);
                }
                else
                {
                    zTextPos = drawPosition + up_normalized * (spacingAboveLine + halfAbsMarkerLinesWidth) + forward_normalized * (spacingFromMarkerCross + halfAbsMarkerLinesWidth);
                }
                string coordSystemSpecifyingText_z = drawCoordSystemSpecifyingString ? (DrawText.MarkupColor(coordSystemSpecifyingStringIsLocal ? localCoordSystemSpecifyingText : globalCoordSystemSpecifyingText, UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_z, coordSystemSpecifier_alpha))) : null;
                string drawnCoordsText_z = (DrawBasics.strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM == 0) ? (coordSystemSpecifyingText_z + "z = " + positionCoordsToWrite.z) : (coordSystemSpecifyingText_z + "<sw=" + DrawBasics.strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM + ">z = " + positionCoordsToWrite.z + "</sw>");
                UtilitiesDXXL_Text.Write(drawnCoordsText_z, zTextPos, color_z, coordsTextSize, forward_normalized, up_normalized, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, maxTextLength, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
            }

            ///y:
            Vector3 yTextPos;
            if (coordTextBelowAxes)
            {
                yTextPos = drawPosition - Vector3.left * (spacingAboveLine + halfAbsMarkerLinesWidth + coordsTextSize) + up_normalized * (spacingFromMarkerCross + halfAbsMarkerLinesWidth);
            }
            else
            {
                yTextPos = drawPosition + Vector3.left * (spacingAboveLine + halfAbsMarkerLinesWidth) + up_normalized * (spacingFromMarkerCross + halfAbsMarkerLinesWidth);
            }
            string coordSystemSpecifyingText_y = drawCoordSystemSpecifyingString ? (DrawText.MarkupColor(coordSystemSpecifyingStringIsLocal ? localCoordSystemSpecifyingText : globalCoordSystemSpecifyingText, UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_y, coordSystemSpecifier_alpha))) : null;
            string drawnCoordsText_y = (DrawBasics.strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM == 0) ? (coordSystemSpecifyingText_y + "y = " + positionCoordsToWrite.y) : (coordSystemSpecifyingText_y + "<sw=" + DrawBasics.strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM + ">y = " + positionCoordsToWrite.y + "</sw>");
            UtilitiesDXXL_Text.Write(drawnCoordsText_y, yTextPos, color_y, coordsTextSize, up_normalized, (-right_normalized), DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, maxTextLength, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);

            ///x:
            Vector3 xTextPos;
            if (coordTextBelowAxes)
            {
                xTextPos = drawPosition - up_normalized * (spacingAboveLine + halfAbsMarkerLinesWidth + coordsTextSize) + right_normalized * (spacingFromMarkerCross + halfAbsMarkerLinesWidth);
            }
            else
            {
                xTextPos = drawPosition + up_normalized * (spacingAboveLine + halfAbsMarkerLinesWidth) + right_normalized * (spacingFromMarkerCross + halfAbsMarkerLinesWidth);
            }
            string coordSystemSpecifyingText_x = drawCoordSystemSpecifyingString ? (DrawText.MarkupColor(coordSystemSpecifyingStringIsLocal ? localCoordSystemSpecifyingText : globalCoordSystemSpecifyingText, UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_x, coordSystemSpecifier_alpha))) : null;
            string drawnCoordsText_x = (DrawBasics.strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM == 0) ? (coordSystemSpecifyingText_x + "x = " + positionCoordsToWrite.x) : (coordSystemSpecifyingText_x + "<sw=" + DrawBasics.strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM + ">x = " + positionCoordsToWrite.x + "</sw>");
            UtilitiesDXXL_Text.Write(drawnCoordsText_x, xTextPos, color_x, coordsTextSize, right_normalized, up_normalized, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, maxTextLength, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
        }

        public static float pointTagsTextSize_relToOffset = 0.1f;
        public static void PointTag(Vector3 position, string text = null, Color color = default(Color), float linesWidth = 0.0f, float size_asTextOffsetDistance = 1.0f, Vector3 textOffsetDirection = default(Vector3), float textSizeScaleFactor = 1.0f, bool skipConeDrawing = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(size_asTextOffsetDistance, "size_asTextOffsetDistance")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(textSizeScaleFactor, "textSizeScaleFactor")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(textOffsetDirection, "textOffsetDirection")) { return; }

            //DO NOT fallback to "Point()" here, because "Point()" calls "PointTag()" again, which can create an endless loop.

            UtilitiesDXXL_TextDirAndUpCalculation.GetTextDirAndUpNormalized(out Vector3 textDir_normalized, out Vector3 textUp_normalized, default(Vector3), default(Vector3), position, false, false);
            Vector3 textForward_normalized = Vector3.Cross(textDir_normalized, textUp_normalized);
            if (UtilitiesDXXL_Math.IsDefaultVector(textOffsetDirection))
            {
                Quaternion rotation = Quaternion.LookRotation(textForward_normalized, textUp_normalized);
                textOffsetDirection = rotation * DrawBasics.Default_textOffsetDirection_forPointTags;
            }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);
            size_asTextOffsetDistance = GetClamped_pointTagSize_asTextOffsetDistance(size_asTextOffsetDistance, linesWidth);
            textSizeScaleFactor = Mathf.Abs(textSizeScaleFactor);
            textSizeScaleFactor = Mathf.Max(textSizeScaleFactor, 0.01f);

            Vector3 textOffsetDir_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(textOffsetDirection);
            Vector3 pos_to_startOfUnderLine = textOffsetDir_normalized * size_asTextOffsetDistance;
            float coneHeight = 0.2f * size_asTextOffsetDistance;
            coneHeight = Mathf.Max(coneHeight, 2.4f * linesWidth);
            Vector3 startOfTextUnderline = position + pos_to_startOfUnderLine;

            if (skipConeDrawing)
            {
                Line_fadeableAnimSpeed.InternalDraw(position, startOfTextUnderline, color, linesWidth, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }
            else
            {
                float offsetDistance_forStartAnchorOfLineToText = (3.0f * linesWidth);
                offsetDistance_forStartAnchorOfLineToText = Mathf.Min(offsetDistance_forStartAnchorOfLineToText, coneHeight);
                Vector3 offsettedStartAnchor_ofLineToText = position + offsetDistance_forStartAnchorOfLineToText * textOffsetDir_normalized;
                Line_fadeableAnimSpeed.InternalDraw(offsettedStartAnchor_ofLineToText, startOfTextUnderline, color, linesWidth, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            float textLength = 0.2f * size_asTextOffsetDistance;
            if (text != null && text != "")
            {
                float textSize = pointTagsTextSize_relToOffset * textSizeScaleFactor * size_asTextOffsetDistance;
                UtilitiesDXXL_Text.Write(text, startOfTextUnderline + textUp_normalized * (0.3f * textSize + 0.5f * linesWidth), color, textSize, textDir_normalized, textUp_normalized, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                float lengthOfLongestLine_inText = DrawText.parsedTextSpecs.widthOfLongestLine;
                textLength = Mathf.Max(textLength, lengthOfLongestLine_inText);
            }
            Line_fadeableAnimSpeed.InternalDraw(startOfTextUnderline, startOfTextUnderline + textDir_normalized * textLength, color, linesWidth, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            if (skipConeDrawing == false)
            {
                float coneAngleDeg = 25.0f;
                Vector3 upVector_ofConeBaseRect = textForward_normalized;
                DrawShapes.ConeFilled(position, coneHeight, pos_to_startOfUnderLine, upVector_ofConeBaseRect, 0.0f, coneAngleDeg, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
            }
        }

        public static float GetClamped_pointTagSize_asTextOffsetDistance(float size_asTextOffsetDistance_unclamped, float linesWidth)
        {
            size_asTextOffsetDistance_unclamped = Mathf.Abs(size_asTextOffsetDistance_unclamped);
            return UtilitiesDXXL_Math.Max(size_asTextOffsetDistance_unclamped, 3.0f * linesWidth, 0.01f);
        }

        static DrawBasics.LengthInterpretation endPlates_sizeInterpretation_before;
        public static void Set_endPlates_sizeInterpretation_reversible(DrawBasics.LengthInterpretation newSizeInterpretation)
        {
            endPlates_sizeInterpretation_before = DrawBasics.endPlates_sizeInterpretation;
            DrawBasics.endPlates_sizeInterpretation = newSizeInterpretation;
        }
        public static void Reverse_endPlates_sizeInterpretation()
        {
            DrawBasics.endPlates_sizeInterpretation = endPlates_sizeInterpretation_before;
        }

        static bool disableEndPlates_atLineStart_before;
        public static void Set_disableEndPlates_atLineStart_reversible(bool new_disableEndPlates_atLineStart)
        {
            disableEndPlates_atLineStart_before = DrawBasics.disableEndPlates_atLineStart;
            DrawBasics.disableEndPlates_atLineStart = new_disableEndPlates_atLineStart;
        }
        public static void Reverse_disableEndPlates_atLineStart()
        {
            DrawBasics.disableEndPlates_atLineStart = disableEndPlates_atLineStart_before;
        }

        static bool disableEndPlates_atLineEnd_before;
        public static void Set_disableEndPlates_atLineEnd_reversible(bool new_disableEndPlates_atLineEnd)
        {
            disableEndPlates_atLineEnd_before = DrawBasics.disableEndPlates_atLineEnd;
            DrawBasics.disableEndPlates_atLineEnd = new_disableEndPlates_atLineEnd;
        }
        public static void Reverse_disableEndPlates_atLineEnd()
        {
            DrawBasics.disableEndPlates_atLineEnd = disableEndPlates_atLineEnd_before;
        }

        public static float Set_coneLength_interpretation_forStraightVectors_reversible(bool setConeLengthToRelative_notToAbsolute, float coneLength_ifSetToRelative, float coneLength_ifSetToAbsolute)
        {
            if (setConeLengthToRelative_notToAbsolute)
            {
                Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                return coneLength_ifSetToRelative;
            }
            else
            {
                Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                return coneLength_ifSetToAbsolute;
            }
        }

        static DrawBasics.LengthInterpretation coneLength_interpretationn_forStraightVectors_before;
        public static void Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation newLengthInterpretation)
        {
            coneLength_interpretationn_forStraightVectors_before = DrawBasics.coneLength_interpretation_forStraightVectors;
            DrawBasics.coneLength_interpretation_forStraightVectors = newLengthInterpretation;
        }
        public static void Reverse_coneLength_interpretation_forStraightVectors()
        {
            DrawBasics.coneLength_interpretation_forStraightVectors = coneLength_interpretationn_forStraightVectors_before;
        }

        public static float Set_coneLength_interpretation_forCircledVectors_reversible(bool setConeLengthToRelative_notToAbsolute, float coneLength_ifSetToRelative, float coneLength_ifSetToAbsolute)
        {
            if (setConeLengthToRelative_notToAbsolute)
            {
                Set_coneLength_interpretation_forCircledVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                return coneLength_ifSetToRelative;
            }
            else
            {
                Set_coneLength_interpretation_forCircledVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                return coneLength_ifSetToAbsolute;
            }
        }

        static DrawBasics.LengthInterpretation coneLength_interpretationn_forCircledVectors_before;
        public static void Set_coneLength_interpretation_forCircledVectors_reversible(DrawBasics.LengthInterpretation newLengthInterpretation)
        {
            coneLength_interpretationn_forCircledVectors_before = DrawBasics.coneLength_interpretation_forCircledVectors;
            DrawBasics.coneLength_interpretation_forCircledVectors = newLengthInterpretation;
        }
        public static void Reverse_coneLength_interpretation_forCircledVectors()
        {
            DrawBasics.coneLength_interpretation_forCircledVectors = coneLength_interpretationn_forCircledVectors_before;
        }

        static DrawBasics.AutomaticAmplitudeAndTextAlignment automaticAmplitudeAndTextAlignment_before;
        public static void Set_automaticAmplitudeAndTextAlignment_reversible(DrawBasics.AutomaticAmplitudeAndTextAlignment new_automaticAmplitudeAndTextAlignment)
        {
            automaticAmplitudeAndTextAlignment_before = DrawBasics.automaticAmplitudeAndTextAlignment;
            DrawBasics.automaticAmplitudeAndTextAlignment = new_automaticAmplitudeAndTextAlignment;
        }
        public static void Reverse_automaticAmplitudeAndTextAlignment()
        {
            DrawBasics.automaticAmplitudeAndTextAlignment = automaticAmplitudeAndTextAlignment_before;
        }

        static DrawBasics.CameraForAutomaticOrientation cameraForAutomaticOrientation_before;
        public static void Set_cameraForAutomaticOrientation_reversible(DrawBasics.CameraForAutomaticOrientation new_cameraForAutomaticOrientation)
        {
            cameraForAutomaticOrientation_before = DrawBasics.cameraForAutomaticOrientation;
            DrawBasics.cameraForAutomaticOrientation = new_cameraForAutomaticOrientation;
        }
        public static void Reverse_cameraForAutomaticOrientation()
        {
            DrawBasics.cameraForAutomaticOrientation = cameraForAutomaticOrientation_before;
        }

        static DrawBasics.UsedUnityLineDrawingMethod usedLineDrawingMethod_before;
        public static void Set_usedLineDrawingMethod_reversible(DrawBasics.UsedUnityLineDrawingMethod new_usedLineDrawingMethod)
        {
#if UNITY_EDITOR
            usedLineDrawingMethod_before = DrawBasics.usedUnityLineDrawingMethod;
            DrawBasics.usedUnityLineDrawingMethod = new_usedLineDrawingMethod;
#else
            DrawBasics.usedUnityLineDrawingMethod =  DrawBasics.UsedUnityLineDrawingMethod.wireMesh;
#endif
        }
        public static void Reverse_usedLineDrawingMethod()
        {
#if UNITY_EDITOR
            DrawBasics.usedUnityLineDrawingMethod = usedLineDrawingMethod_before;
#endif
        }

        static Matrix4x4 gizmoMatrix_before;
        public static void Set_gizmoMatrix_reversible(Matrix4x4 new_gizmoMatrix)
        {
            gizmoMatrix_before = Gizmos.matrix;
            Gizmos.matrix = new_gizmoMatrix;
        }
        public static void Reverse_gizmoMatrix()
        {
            Gizmos.matrix = gizmoMatrix_before;
        }

        static int strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM_before;
        public static void Set_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM_reversible(int new_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM)
        {
            strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM_before = DrawBasics.strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM;
            DrawBasics.strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM = new_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM;
        }
        public static void Reverse_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM()
        {
            DrawBasics.strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM = strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM_before;
        }

        static float relSizeOfTextOnLines_before;
        public static void Set_relSizeOfTextOnLines_reversible(float new_relSizeOfTextOnLines)
        {
            relSizeOfTextOnLines_before = DrawBasics.RelSizeOfTextOnLines;
            DrawBasics.RelSizeOfTextOnLines = new_relSizeOfTextOnLines;
        }
        public static void Reverse_relSizeOfTextOnLines()
        {
            DrawBasics.RelSizeOfTextOnLines = relSizeOfTextOnLines_before;
        }

        static bool shiftTextPosOnLines_toNonIntersecting_before;
        public static void Set_shiftTextPosOnLines_toNonIntersecting_reversible(bool new_shiftTextPosOnLines_toNonIntersecting)
        {
            shiftTextPosOnLines_toNonIntersecting_before = DrawBasics.shiftTextPosOnLines_toNonIntersecting;
            DrawBasics.shiftTextPosOnLines_toNonIntersecting = new_shiftTextPosOnLines_toNonIntersecting;
        }
        public static void Reverse_shiftTextPosOnLines_toNonIntersecting()
        {
            DrawBasics.shiftTextPosOnLines_toNonIntersecting = shiftTextPosOnLines_toNonIntersecting_before;
        }

        static float globalAlphaFactor_before;
        public static void Set_globalAlphaFactor_reversible(float new_globalAlphaFactor)
        {
            globalAlphaFactor_before = DrawBasics.GlobalAlphaFactor;
            DrawBasics.GlobalAlphaFactor = new_globalAlphaFactor;
        }
        public static void Reverse_globalAlphaFactor()
        {
            DrawBasics.GlobalAlphaFactor = globalAlphaFactor_before;
        }

        public static void Vector(Vector3 vectorStartPos, Vector3 vectorEndPos, Color color, float lineWidth, string text, float coneLength, bool pointerAtBothSides, bool flattenThickRoundLineIntoAmplitudePlane, bool addNormalizedMarkingText, float enlargeSmallTextToThisMinTextSize, bool writeComponentValuesAsText, float durationInSec, bool hiddenByNearerObjects, Vector3 customAmplitudeAndTextDir, bool drawnLineIsFrom_DrawBasics2D, float endPlates_size)
        {
            //-> amplitude specified via vector
            Vector(vectorStartPos, vectorEndPos, color, lineWidth, text, coneLength, pointerAtBothSides, flattenThickRoundLineIntoAmplitudePlane, addNormalizedMarkingText, enlargeSmallTextToThisMinTextSize, writeComponentValuesAsText, durationInSec, hiddenByNearerObjects, null, customAmplitudeAndTextDir, drawnLineIsFrom_DrawBasics2D, endPlates_size);
        }

        public static void Vector(Vector3 vectorStartPos, Vector3 vectorEndPos, Color color, float lineWidth, string text, float coneLength, bool pointerAtBothSides, bool flattenThickRoundLineIntoAmplitudePlane, bool addNormalizedMarkingText, float enlargeSmallTextToThisMinTextSize, bool writeComponentValuesAsText, float durationInSec, bool hiddenByNearerObjects, InternalDXXL_Plane preferredAmplitudePlane, bool drawnLineIsFrom_DrawBasics2D, float endPlates_size)
        {
            //-> amplitude specified via plane
            Vector(vectorStartPos, vectorEndPos, color, lineWidth, text, coneLength, pointerAtBothSides, flattenThickRoundLineIntoAmplitudePlane, addNormalizedMarkingText, enlargeSmallTextToThisMinTextSize, writeComponentValuesAsText, durationInSec, hiddenByNearerObjects, preferredAmplitudePlane, default(Vector3), drawnLineIsFrom_DrawBasics2D, endPlates_size);
        }

        public static void Vector(Vector3 vectorStartPos, Vector3 vectorEndPos, Color color, float lineWidth, string text, float coneLength, bool pointerAtBothSides, bool flattenThickRoundLineIntoAmplitudePlane, bool addNormalizedMarkingText, float enlargeSmallTextToThisMinTextSize, bool writeComponentValuesAsText, float durationInSec, bool hiddenByNearerObjects, InternalDXXL_Plane preferredAmplitudePlane, Vector3 customAmplitudeAndTextDir, bool drawnLineIsFrom_DrawBasics2D, float endPlates_size)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vectorStartPos, "vectorStartPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vectorEndPos, "vectorEndPos")) { return; }
            VectorFrom(vectorStartPos, vectorEndPos - vectorStartPos, color, lineWidth, text, coneLength, pointerAtBothSides, flattenThickRoundLineIntoAmplitudePlane, addNormalizedMarkingText, enlargeSmallTextToThisMinTextSize, writeComponentValuesAsText, durationInSec, hiddenByNearerObjects, preferredAmplitudePlane, customAmplitudeAndTextDir, drawnLineIsFrom_DrawBasics2D, endPlates_size);
        }

        public static void VectorFrom(Vector3 vectorStartPos, Vector3 vector, Color color, float lineWidth, string text, float coneLength, bool pointerAtBothSides, bool flattenThickRoundLineIntoAmplitudePlane, bool addNormalizedMarkingText, float enlargeSmallTextToThisMinTextSize, bool writeComponentValuesAsText, float durationInSec, bool hiddenByNearerObjects, Vector3 customAmplitudeAndTextDir, bool drawnLineIsFrom_DrawBasics2D, float endPlates_size)
        {
            VectorFrom(vectorStartPos, vector, color, lineWidth, text, coneLength, pointerAtBothSides, flattenThickRoundLineIntoAmplitudePlane, addNormalizedMarkingText, enlargeSmallTextToThisMinTextSize, writeComponentValuesAsText, durationInSec, hiddenByNearerObjects, null, customAmplitudeAndTextDir, drawnLineIsFrom_DrawBasics2D, endPlates_size);
        }

        public static void VectorFrom(Vector3 vectorStartPos, Vector3 vector, Color color, float lineWidth, string text, float coneLength, bool pointerAtBothSides, bool flattenThickRoundLineIntoAmplitudePlane, bool addNormalizedMarkingText, float enlargeSmallTextToThisMinTextSize, bool writeComponentValuesAsText, float durationInSec, bool hiddenByNearerObjects, InternalDXXL_Plane preferredAmplitudePlane, bool drawnLineIsFrom_DrawBasics2D, float endPlates_size)
        {
            VectorFrom(vectorStartPos, vector, color, lineWidth, text, coneLength, pointerAtBothSides, flattenThickRoundLineIntoAmplitudePlane, addNormalizedMarkingText, enlargeSmallTextToThisMinTextSize, writeComponentValuesAsText, durationInSec, hiddenByNearerObjects, preferredAmplitudePlane, default(Vector3), drawnLineIsFrom_DrawBasics2D, endPlates_size);
        }

        public static void VectorFrom(Vector3 vectorStartPos, Vector3 vector, Color color, float lineWidth, string text, float coneLength, bool pointerAtBothSides, bool flattenThickRoundLineIntoAmplitudePlane, bool addNormalizedMarkingText, float enlargeSmallTextToThisMinTextSize, bool writeComponentValuesAsText, float durationInSec, bool hiddenByNearerObjects, InternalDXXL_Plane preferredAmplitudePlane, Vector3 customAmplitudeAndTextDir, bool drawnLineIsFrom_DrawBasics2D, float endPlates_size)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lineWidth, "lineWidth")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(coneLength, "coneLength")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vectorStartPos, "vectorStartPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vector, "vector")) { return; }

            lineWidth = UtilitiesDXXL_Math.AbsNonZeroValue(lineWidth);

            if (UtilitiesDXXL_Math.ApproximatelyZero(vector))
            {
                if (UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.theCurrentlyDrawnLines_areAllFrom_vectorsThatBuildUpAnArrowsLine == false)
                {
                    DrawFallbackForApproxZeroLengthVectors(drawnLineIsFrom_DrawBasics2D, vectorStartPos, color, lineWidth, text, durationInSec, hiddenByNearerObjects);
                }
                return;
            }

            Vector3 vectorEndPos = vectorStartPos + vector;
            bool isThinLine = UtilitiesDXXL_Math.ApproximatelyZero(lineWidth);
            Vector3 vectorNormalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(vector, out float vectorLength);

            if (vectorLength < UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.shortestLineWithDefinedAmplitudeDir)
            {
                if (UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.theCurrentlyDrawnLines_areAllFrom_vectorsThatBuildUpAnArrowsLine == false)
                {
                    DrawFallbackForApproxZeroLengthVectors(drawnLineIsFrom_DrawBasics2D, vectorStartPos, color, lineWidth, text, durationInSec, hiddenByNearerObjects);
                }
                return;
            }

            if (DrawBasics.coneLength_interpretation_forStraightVectors == DrawBasics.LengthInterpretation.relativeToLineLength) { coneLength = coneLength * vectorLength; }
            coneLength = Mathf.Clamp(coneLength, min_relConeLengthForVectors * vectorLength, max_relConeLengthForVectors * vectorLength);

            float coneAngleDeg = curr_vectorConeAngleDeg;
            if (isThinLine == false)
            {
                float coneSize_to_lineWidth_scaler = 1.0f;
                float minConeAngleDeg = 2.0f * Mathf.Rad2Deg * Mathf.Atan(coneSize_to_lineWidth_scaler * lineWidth / coneLength);
                coneAngleDeg = Mathf.Max(coneAngleDeg, minConeAngleDeg);
            }

            float lengthTillConeStart = vectorLength - coneLength;
            Vector3 endConeBaseCenter = vectorStartPos + vectorNormalized * lengthTillConeStart;
            Vector3 startConeBaseCenter = vectorStartPos;
            if (pointerAtBothSides)
            {
                startConeBaseCenter = vectorStartPos + vectorNormalized * coneLength;
            }

            if (writeComponentValuesAsText)
            {
                if (drawnLineIsFrom_DrawBasics2D)
                {
                    text = "<size=6>( " + vector.x + " , " + vector.y + " )</size><br>" + text;
                }
                else
                {
                    text = "<size=6>( " + vector.x + " , " + vector.y + " , " + vector.z + " )</size><br>" + text;
                }
            }
            Line(startConeBaseCenter, endConeBaseCenter, color, lineWidth, text, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, preferredAmplitudePlane, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, 0.0f, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, false, false, null, drawnLineIsFrom_DrawBasics2D, 0.0f, 1.0f);

            float shorteningOf_straightLineInsideCone = 0.0f;
            if (isThinLine == false)
            {
                shorteningOf_straightLineInsideCone = (0.5f * lineWidth) / Mathf.Tan(Mathf.Deg2Rad * 0.5f * coneAngleDeg);
                shorteningOf_straightLineInsideCone = Mathf.Min(shorteningOf_straightLineInsideCone, 0.99f * coneLength);
            }

            Vector3 lineEndInsideEndCone = vectorEndPos - vectorNormalized * shorteningOf_straightLineInsideCone;
            Line(endConeBaseCenter, lineEndInsideEndCone, color, lineWidth, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, preferredAmplitudePlane, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f, 1.0f);
            if (pointerAtBothSides)
            {
                Vector3 lineEndInsideStartCone = vectorStartPos + vectorNormalized * shorteningOf_straightLineInsideCone;
                Line(startConeBaseCenter, lineEndInsideStartCone, color, lineWidth, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, preferredAmplitudePlane, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f, 1.0f);
            }

            DrawBasics.LineStyle dummyStyle_forForcingCalcDefinedAmplitudeDir = DrawBasics.LineStyle.zigzag; //-> the logic in which cases a definedUp_amplitude is needed here differs from the default implementation in "UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.CheckWhichDirectionsAreNeeded()", therefore this dummyStyle is used to ensure that such a definedUp_amplitude is always calced (also if it is not always required). It may also get overwritten by "UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.theCurrentlyDrawnLines_areAllFrom_vectorsThatBuildUpAnArrowsLine"
            UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.Get_normalized_amplitudeAndTextDirVectors(out Vector3 upVector_insideConeBaseCircle, out Vector3 textDir_normalized, out float lengthOfDrawnLine, out bool lengthOfDrawnLine_isFilled, vectorStartPos, vector, false, false, null, false, dummyStyle_forForcingCalcDefinedAmplitudeDir, true, false, preferredAmplitudePlane, customAmplitudeAndTextDir, null, false);

            useMoreStrutsForFlatPyramidArrow = ((isThinLine == false) && color.a < 0.8f); //thick arrows with low alpha (like in "DrawPhysics.VolumeCast()") look irregular with lower values of "DrawShapes.cornersOnFlatPyramidBase"
            float coneAngleDeg_inHorizDir = flattenThickRoundLineIntoAmplitudePlane ? 0.0f : coneAngleDeg;
            DrawShapes.ConeFilled(vectorEndPos, coneLength, -vector, upVector_insideConeBaseCircle, coneAngleDeg, coneAngleDeg_inHorizDir, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
            if (pointerAtBothSides)
            {
                DrawShapes.ConeFilled(vectorStartPos, coneLength, vector, upVector_insideConeBaseCircle, coneAngleDeg, coneAngleDeg_inHorizDir, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
            }
            useMoreStrutsForFlatPyramidArrow = false;

            if (addNormalizedMarkingText)
            {
                Vector3 aVectorPerpToDrawnVector_normalized = UtilitiesDXXL_Math.Get_aNormalizedVector_perpToGivenVector(vectorNormalized);
                Vector3 textsInitialDir_normalized = Vector3.Cross(aVectorPerpToDrawnVector_normalized, vectorNormalized);
                float half_lineWidth = 0.5f * lineWidth;
                float textRadius = 0.05f + half_lineWidth;
                float textSize = 0.4f * textRadius;
                UtilitiesDXXL_Text.WriteOnCircle("normalized", vectorStartPos + vectorNormalized, textRadius, color, textSize, textsInitialDir_normalized, aVectorPerpToDrawnVector_normalized, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                Line_fadeableAnimSpeed.InternalDraw(vectorStartPos, vectorStartPos + vectorNormalized, UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.2f), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(endPlates_size) == false)
            {
                DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.invisible;
                Line(vectorStartPos, vectorEndPos, color, 0.0f, null, lineStyle, 1.0f, 0.0f, null, preferredAmplitudePlane, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, endPlates_size, 1.0f);
            }
        }

        static void DrawFallbackForApproxZeroLengthVectors(bool drawnLineIsFrom_DrawBasics2D, Vector3 vectorStartPos, Color color, float lineWidth, string text, float durationInSec, bool hiddenByNearerObjects)
        {
            if (drawnLineIsFrom_DrawBasics2D)
            {
                UtilitiesDXXL_DrawBasics2D.PointFallback(vectorStartPos, "[<color=#adadadFF><icon=logMessage></color> Vector with length of approximately 0]<br>" + text, color, lineWidth, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                PointFallback(vectorStartPos, "[<color=#adadadFF><icon=logMessage></color> Vector with length of approximately 0]<br>" + text, color, lineWidth, durationInSec, hiddenByNearerObjects);
            }
        }

        public static LineAnimationProgress MovingArrowsLine(Vector3 start, Vector3 end, Color color, float lineWidth, float distanceBetweenArrows, float lengthOfArrows, string text, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, bool pointersDirAlongAnimationDir, bool flattenThickRoundLineIntoAmplitudePlane, Vector3 customAmplitudeAndTextDir, float endPlates_size, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects, bool drawnLineIsFrom_DrawBasics2D)
        {
            //-> amplitude specified via vector
            return MovingArrowsLine(start, end, color, lineWidth, distanceBetweenArrows, lengthOfArrows, text, animationSpeed, precedingLineAnimationProgress, pointersDirAlongAnimationDir, flattenThickRoundLineIntoAmplitudePlane, null, customAmplitudeAndTextDir, endPlates_size, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, drawnLineIsFrom_DrawBasics2D);
        }

        public static LineAnimationProgress MovingArrowsLine(Vector3 start, Vector3 end, Color color, float lineWidth, float distanceBetweenArrows, float lengthOfArrows, string text, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, bool pointersDirAlongAnimationDir, bool flattenThickRoundLineIntoAmplitudePlane, InternalDXXL_Plane preferredAmplitudePlane, float endPlates_size, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects, bool drawnLineIsFrom_DrawBasics2D)
        {
            //-> amplitude specified via plane
            return MovingArrowsLine(start, end, color, lineWidth, distanceBetweenArrows, lengthOfArrows, text, animationSpeed, precedingLineAnimationProgress, pointersDirAlongAnimationDir, flattenThickRoundLineIntoAmplitudePlane, preferredAmplitudePlane, default(Vector3), endPlates_size, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, drawnLineIsFrom_DrawBasics2D);
        }

        public static LineAnimationProgress MovingArrowsLine(Vector3 start, Vector3 end, Color color, float lineWidth, float distanceBetweenArrows, float lengthOfArrows, string text, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, bool pointersDirAlongAnimationDir, bool flattenThickRoundLineIntoAmplitudePlane, InternalDXXL_Plane preferredAmplitudePlane, Vector3 customAmplitudeAndTextDir, float endPlates_size, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects, bool drawnLineIsFrom_DrawBasics2D)
        {
            //Lines drawn with this function have a higher likelyhood of accidentially using up high numbers of drawnLinePerFrame, because "distanceBetweenArrows" and "lengthOfArrows" can be set manually instead of beeing determined by the lineStyle-code 
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lineWidth, "lineWidth")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(distanceBetweenArrows, "distanceBetweenArrows")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lengthOfArrows, "lengthOfArrows")) { return null; }

            lineWidth = UtilitiesDXXL_Math.AbsNonZeroValue(lineWidth);
            //small or big values of "distanceBetweenArrows" and "lengthOfArrows" may additionally get changed in "UtilitiesDXXL_LineStyles" inside the "skipPatternEnlargementFor*Lines == false" mechanic.
            distanceBetweenArrows = Mathf.Max(distanceBetweenArrows, 0.0002f);
            lengthOfArrows = Mathf.Min(lengthOfArrows, 0.9f * distanceBetweenArrows);
            lengthOfArrows = Mathf.Max(lengthOfArrows, 0.0001f);
            float lengthOfEmptySpaces = distanceBetweenArrows - lengthOfArrows;
            LineAnimationProgress lineAnimProgressAfterDrawing = null;

            UtilitiesDXXL_LineStyles.curr_pointersDirAlongAnimationDir = pointersDirAlongAnimationDir;
            try
            {
                UtilitiesDXXL_LineStyles.curr_dashLength_forArrowLine = lengthOfArrows;
                if (UtilitiesDXXL_Math.ApproximatelyZero(lineWidth) == false)
                {
                    UtilitiesDXXL_LineStyles.curr_minRatio_for_dashLengthToLineWidth_forArrowLine = lengthOfArrows / lineWidth;
                }
                UtilitiesDXXL_LineStyles.curr_spaceToDash_ratio_forArrowLine = lengthOfEmptySpaces / lengthOfArrows;
                UtilitiesDXXL_LineStyles.curr_minEmptySpacesLength_forArrowLine = lengthOfEmptySpaces;
                lineAnimProgressAfterDrawing = Line(start, end, color, lineWidth, text, DrawBasics.LineStyle.arrows, 1.0f, animationSpeed, precedingLineAnimationProgress, preferredAmplitudePlane, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, 0.0f, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, false, false, null, drawnLineIsFrom_DrawBasics2D, endPlates_size, 1.0f);
            }
            catch { }

            UtilitiesDXXL_LineStyles.curr_pointersDirAlongAnimationDir = UtilitiesDXXL_LineStyles.default_pointersDirAlongAnimationDir;
            UtilitiesDXXL_LineStyles.curr_dashLength_forArrowLine = UtilitiesDXXL_LineStyles.default_dashLength_forArrowLine;
            UtilitiesDXXL_LineStyles.curr_minRatio_for_dashLengthToLineWidth_forArrowLine = UtilitiesDXXL_LineStyles.default_minRatio_for_dashLengthToLineWidth_forArrowLine;
            UtilitiesDXXL_LineStyles.curr_spaceToDash_ratio_forArrowLine = UtilitiesDXXL_LineStyles.default_spaceToDash_ratio_forArrowLine;
            UtilitiesDXXL_LineStyles.curr_minEmptySpacesLength_forArrowLine = UtilitiesDXXL_LineStyles.default_minEmptySpacesLength_forArrowLine;

            return lineAnimProgressAfterDrawing;
        }

        public static bool GetSpecsOfLineUnderTension(out float tensionFactor, out Color usedColor, out float lineLength, Vector3 start, Vector3 end, float relaxedLength, Color relaxedColor, Color color_forStretchedTension, Color color_forSqueezedTension, float stretchFactor_forStretchedTensionColor, float stretchFactor_forSqueezedTensionColor)
        {
            //returns "parametersAreInvalid"

            tensionFactor = 1.0f; //-> for invalid early returns
            usedColor = default; //-> for invalid early returns
            lineLength = 1.0f; //-> for invalid early returns
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return true; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(end, "end")) { return true; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(relaxedLength, "relaxedLength")) { return true; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(stretchFactor_forStretchedTensionColor, "stretchFactor_forStretchedTensionColor")) { return true; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(stretchFactor_forSqueezedTensionColor, "stretchFactor_forSqueezedTensionColor")) { return true; }

            lineLength = (end - start).magnitude;
            relaxedLength = Mathf.Abs(relaxedLength);
            relaxedLength = Mathf.Max(relaxedLength, 0.001f);
            tensionFactor = lineLength / relaxedLength;
            tensionFactor = Mathf.Max(tensionFactor, 0.001f);
            usedColor = GetColorOfLineUnderTension(relaxedColor, color_forStretchedTension, color_forSqueezedTension, lineLength, relaxedLength, stretchFactor_forStretchedTensionColor, stretchFactor_forSqueezedTensionColor);
            return false;
        }

        static Color GetColorOfLineUnderTension(Color relaxedColor, Color color_forStretchedTension, Color color_forSqueezedTension, float lineLength, float relaxedLength, float stretchFactor_forStretchedTensionColor, float stretchFactor_forSqueezedTensionColor)
        {
            stretchFactor_forStretchedTensionColor = Mathf.Abs(stretchFactor_forStretchedTensionColor);
            stretchFactor_forStretchedTensionColor = Mathf.Max(1.001f, stretchFactor_forStretchedTensionColor);

            stretchFactor_forSqueezedTensionColor = Mathf.Abs(stretchFactor_forSqueezedTensionColor);
            stretchFactor_forSqueezedTensionColor = Mathf.Min(0.999f, stretchFactor_forSqueezedTensionColor);

            float lineLength_markingStretchedColor = relaxedLength * stretchFactor_forStretchedTensionColor;
            float lineLength_markingSqueezedColor = relaxedLength * stretchFactor_forSqueezedTensionColor;

            relaxedColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(relaxedColor, UtilitiesDXXL_Colors.green_boolTrue);
            color_forStretchedTension = UtilitiesDXXL_Colors.OverwriteDefaultColor(color_forStretchedTension, UtilitiesDXXL_Colors.red_boolFalse);
            color_forSqueezedTension = UtilitiesDXXL_Colors.OverwriteDefaultColor(color_forSqueezedTension, UtilitiesDXXL_Colors.red_boolFalse);

            if (lineLength < lineLength_markingSqueezedColor)
            {
                return color_forSqueezedTension;
            }
            else
            {
                if (lineLength > lineLength_markingStretchedColor)
                {
                    return color_forStretchedTension;
                }
                else
                {
                    float absLengthDifference_fromRelaxed_toActualLineLength = Mathf.Abs(relaxedLength - lineLength);
                    if (lineLength < relaxedLength)
                    {
                        float absLengthDifference_fromRelaxed_toSqueezed = Mathf.Abs(relaxedLength - lineLength_markingSqueezedColor);
                        float progress0to1_fromRelaxedToSqueezed = absLengthDifference_fromRelaxed_toActualLineLength / absLengthDifference_fromRelaxed_toSqueezed;
                        return Color.Lerp(relaxedColor, color_forSqueezedTension, progress0to1_fromRelaxedToSqueezed);
                    }
                    else
                    {
                        float absLengthDifference_fromRelaxed_toStretched = Mathf.Abs(relaxedLength - lineLength_markingStretchedColor);
                        float progress0to1_fromRelaxedToStreched = absLengthDifference_fromRelaxed_toActualLineLength / absLengthDifference_fromRelaxed_toStretched;
                        return Color.Lerp(relaxedColor, color_forStretchedTension, progress0to1_fromRelaxedToStreched);
                    }
                }
            }
        }

        public static void TryDrawReferenceLengthDisplay_ofLineUnderTension(Vector3 start, Vector3 end, float alphaOfReferenceLengthDisplay, float relaxedLength, Color relaxedColor, float lineLength, InternalDXXL_Plane preferredAmplitudePlane, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(alphaOfReferenceLengthDisplay) == false)
            {
                if (lineLength > 0.0001f) //-> prevent wrong results and divisionByZero in the region of float calculation imprecision
                {
                    Vector3 startToEnd = end - start;
                    Vector3 startToEnd_normalized = startToEnd / lineLength;
                    Vector3 startToRelaxedLength = startToEnd_normalized * relaxedLength;
                    Color usedColor = UtilitiesDXXL_Colors.Get_color_butWithFixedAlpha(relaxedColor, alphaOfReferenceLengthDisplay);
                    bool flattenThickRoundLineIntoAmplitudePlane = true;
                    float endPlates_size = 0.06f;
                    Set_endPlates_sizeInterpretation_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                    Line(start, start + startToRelaxedLength, usedColor, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, preferredAmplitudePlane, flattenThickRoundLineIntoAmplitudePlane, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, endPlates_size, 1.0f);
                    Reverse_endPlates_sizeInterpretation();
                }
            }
        }

        static void TryDrawPerpEndPlates(bool uses_endPlates, float endPlates_size, Vector3 amplitudeUp_normalized_ofMountingLine, float length_ofMountingLine, bool length_ofMountingLine_isFilled, bool flattenThickRoundLineIntoAmplitudePlane, Vector3 start_ofMountingLine, Vector3 end_ofMountingLine, Color startColor, Color endColor, float durationInSec, bool hiddenByNearerObjects)
        {
            if (uses_endPlates)
            {
                if (UtilitiesDXXL_Colors.IsDefaultColor(endColor)) { endColor = startColor; }
                Vector3 startToEnd = end_ofMountingLine - start_ofMountingLine;
                float half_lengthOfPlate = GetHalfLengthOfEndPlate(startToEnd, endPlates_size, length_ofMountingLine, length_ofMountingLine_isFilled);

                if (flattenThickRoundLineIntoAmplitudePlane)
                {
                    Vector3 mountingPosTo_plateStart = amplitudeUp_normalized_ofMountingLine * half_lengthOfPlate;
                    Vector3 mountingPosTo_plateEnd = (-mountingPosTo_plateStart);
                    InternalDXXL_AmplitudeDependentLineDetails amplitudeDependentLineDetails = Get_amplitudeDependentLineDetails_forThinEndPlateLine();

                    if (DrawBasics.disableEndPlates_atLineStart == false)
                    {
                        ThinLine(start_ofMountingLine + mountingPosTo_plateStart, start_ofMountingLine + mountingPosTo_plateEnd, startColor, startColor, null, 0.0f, 0.0f, durationInSec, 1.0f, 0.0f, null, hiddenByNearerObjects, false, false, false, amplitudeDependentLineDetails, 1.0f);
                    }

                    if (DrawBasics.disableEndPlates_atLineEnd == false)
                    {
                        ThinLine(end_ofMountingLine + mountingPosTo_plateStart, end_ofMountingLine + mountingPosTo_plateEnd, endColor, endColor, null, 0.0f, 0.0f, durationInSec, 1.0f, 0.0f, null, hiddenByNearerObjects, false, false, false, amplitudeDependentLineDetails, 1.0f);
                    }
                }
                else
                {
                    Vector3 up_insideDecagonPlane = default(Vector3);
                    Vector3 normal = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(startToEnd);

                    if (DrawBasics.disableEndPlates_atLineStart == false)
                    {
                        UtilitiesDXXL_Shapes.Decagon(start_ofMountingLine, half_lengthOfPlate, startColor, normal, up_insideDecagonPlane, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, true, false, durationInSec, hiddenByNearerObjects, false);
                    }

                    if (DrawBasics.disableEndPlates_atLineEnd == false)
                    {
                        UtilitiesDXXL_Shapes.Decagon(end_ofMountingLine, half_lengthOfPlate, endColor, normal, up_insideDecagonPlane, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, true, false, durationInSec, hiddenByNearerObjects, false);
                    }
                }
            }
        }

        static float GetHalfLengthOfEndPlate(Vector3 startToEnd, float endPlates_size, float length_ofMountingLine, bool length_ofMountingLine_isFilled)
        {
            if (DrawBasics.endPlates_sizeInterpretation == DrawBasics.LengthInterpretation.relativeToLineLength)
            {
                float startToEnd_length;
                if (length_ofMountingLine_isFilled)
                {
                    startToEnd_length = length_ofMountingLine;
                }
                else
                {
                    startToEnd_length = startToEnd.magnitude;
                }
                float lengthOfPlate = startToEnd_length * endPlates_size;
                return (0.5f * lengthOfPlate);
            }
            else
            {
                return (0.5f * endPlates_size);
            }
        }

        static InternalDXXL_AmplitudeDependentLineDetails Get_amplitudeDependentLineDetails_forThinEndPlateLine()
        {
            InternalDXXL_AmplitudeDependentLineDetails amplitudeDependentLineDetails = new InternalDXXL_AmplitudeDependentLineDetails();
            amplitudeDependentLineDetails.lineWidth = 0.0f;
            amplitudeDependentLineDetails.isThinLine = true;
            amplitudeDependentLineDetails.enlargeSmallText = false;
            amplitudeDependentLineDetails.textDrawingIsSkipped_dueToLineIsTooShort = false;
            amplitudeDependentLineDetails.style = DrawBasics.LineStyle.solid;
            amplitudeDependentLineDetails.uses_endPlates = false; //-> the now drawnLine is itself the endPlate -> this endPlate doesn't need further endPlates at itself.
            amplitudeDependentLineDetails.endPlates_size = 0.0f;
            amplitudeDependentLineDetails.amplitudeUp_normalized = default(Vector3); //-> the endPlates are thinLines of style=solid without text so they don't need an amplitudeDir
            amplitudeDependentLineDetails.textDir_normalized = default(Vector3);
            amplitudeDependentLineDetails.lengthOfDrawnLine_isFilled = false;
            amplitudeDependentLineDetails.lengthOfDrawnLine = 0.0f;
            return amplitudeDependentLineDetails;
        }

        static List<Vector3> duplicatesPrintOffsetsForThickLineIcons = new List<Vector3>();
        static int usedSlots_inIconDuplicatesPrintOffsetList;
        public static void Icon(Vector3 position, DrawBasics.IconType icon, Color color, float size, string text, Quaternion rotation, int strokeWidth_asPPMofSize, bool mirrorHorizontally, float durationInSec, bool hiddenByNearerObjects, float textScaleFactor, float minTextSize, bool autoFlipMirroredText_toFitObserverCam)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(size, "size")) { return; }

            if (UtilitiesDXXL_Math.ApproximatelyZero(size))
            {
                PointFallback(position, "[Icon " + DrawText.MarkupIcon(icon) + " with size of 0]<br>" + text, color, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            //"UtilitiesDXXL_FlatShapesNormaAndUpCalculation" delivers "forward", not "normal" (see notes inside "UtilitiesDXXL_FlatShapesNormaAndUpCalculation.GetNormalAndUpInsidePlane()")
            Quaternion unmirroredRotation = UtilitiesDXXL_FlatShapesNormaAndUpCalculation.GetQuaternion(rotation, position);
            Quaternion mirroredRotation = default;
            if (mirrorHorizontally)
            {
                Vector3 unmirroredUp = unmirroredRotation * Vector3.up;
                Vector3 unmirroredForward = unmirroredRotation * Vector3.forward;
                mirroredRotation = Quaternion.LookRotation((-unmirroredForward), unmirroredUp);
            }
            Quaternion usedRotation = mirrorHorizontally ? mirroredRotation : unmirroredRotation;

            strokeWidth_asPPMofSize = Mathf.Max(strokeWidth_asPPMofSize, 0);
            strokeWidth_asPPMofSize = Mathf.Min(strokeWidth_asPPMofSize, 100000);

            UtilitiesDXXL_CharsAndIcons.RefillCurrPrintedCharDefWithZeroCenteredIcon(icon);
            for (int i = 0; i < DrawXXL_LinesManager.instance.numberOfStrokes_forCurrUsedChar; i++)
            {
                UtilitiesDXXL_Text.TurnCharDef(ref DrawXXL_LinesManager.instance.currPrinted_charDef[i], usedRotation);
            }

            float relativeStrokeWidth = (strokeWidth_asPPMofSize == 0) ? 0.0f : (0.000001f * strokeWidth_asPPMofSize);
            usedSlots_inIconDuplicatesPrintOffsetList = UtilitiesDXXL_Text.GetDuplicatesPrintOffsetsUnrotated_ofTextIndependentIconOfSize1(ref duplicatesPrintOffsetsForThickLineIcons, size, relativeStrokeWidth);
            UtilitiesDXXL_Text.TurnCharDef(ref duplicatesPrintOffsetsForThickLineIcons, usedSlots_inIconDuplicatesPrintOffsetList, usedRotation);

            for (int i_stroke = 0; i_stroke < DrawXXL_LinesManager.instance.numberOfStrokes_forCurrUsedChar; i_stroke++)
            {
                int linesInsideCurrStroke = DrawXXL_LinesManager.instance.numberOfPointsForEachStroke_forCurrUsedChar[i_stroke] - 1;
                for (int i_point = 0; i_point < linesInsideCurrStroke; i_point++)
                {
                    for (int i_duplicatePrint = 0; i_duplicatePrint < usedSlots_inIconDuplicatesPrintOffsetList; i_duplicatePrint++)
                    {
                        Vector3 lineStartPos = position + duplicatesPrintOffsetsForThickLineIcons[i_duplicatePrint] + DrawXXL_LinesManager.instance.currPrinted_charDef[i_stroke][i_point] * size;
                        Vector3 lineEndPos = position + duplicatesPrintOffsetsForThickLineIcons[i_duplicatePrint] + DrawXXL_LinesManager.instance.currPrinted_charDef[i_stroke][i_point + 1] * size;
                        Line_fadeableAnimSpeed.InternalDraw(lineStartPos, lineEndPos, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                    }
                }
            }

            if (text != null && text != "")
            {
                Vector3 up_insideIconPlane_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(unmirroredRotation * Vector3.up);
                float halfSize = 0.5f * size;
                Vector3 textPosition = position - up_insideIconPlane_normalized * halfSize;
                float textSize = textScaleFactor * size;
                textSize = Mathf.Max(textSize, minTextSize);
                float autoLineBreakWidth = size;
                UtilitiesDXXL_Text.WriteFramed(text, textPosition, color, textSize, unmirroredRotation, DrawText.TextAnchorDXXL.UpperCenter, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0025f, 0.0f, autoLineBreakWidth, autoFlipMirroredText_toFitObserverCam, durationInSec, hiddenByNearerObjects);
            }
        }

        public static Quaternion GetRotationOfIcon(Vector3 position, Vector3 normal, Vector3 up_insideIconPlane)
        {
            if (UtilitiesDXXL_Math.IsDefaultVector(normal) && UtilitiesDXXL_Math.IsDefaultVector(up_insideIconPlane))
            {
                return default(Quaternion); //-> will use "DrawShapes.automaticOrientationOfFlatShapes"
            }
            else
            {
                Vector3 forward = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(-normal);
                up_insideIconPlane = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(up_insideIconPlane);
                UtilitiesDXXL_FlatShapesNormaAndUpCalculation.GetNormalAndUpInsidePlane(out Vector3 forward_final_notGuaranteedNormalized, out Vector3 up_insideIconPlane_normalized, forward, up_insideIconPlane, position);
                return Quaternion.LookRotation(forward_final_notGuaranteedNormalized, up_insideIconPlane_normalized);
            }
        }

        public static void Dot(Vector3 position, float radius, Vector3 normal, Color color, string text, float density, float durationInSec, bool hiddenByNearerObjects, bool autoFlipMirroredText_toFitObserverCam)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(normal, "normal")) { return; }

            Quaternion rotation = GetRotationOfIcon(position, normal, default(Vector3));
            Dot(position, radius, rotation, color, text, density, durationInSec, hiddenByNearerObjects, 0.1f, 0.004f, autoFlipMirroredText_toFitObserverCam);
        }

        public static void Dot(Vector3 position, float radius, Quaternion rotation, Color color, string text, float density, float durationInSec, bool hiddenByNearerObjects, float textScaleFactor, float minTextSize, bool autoFlipMirroredText_toFitObserverCam)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(density, "density")) { return; }

            if (UtilitiesDXXL_Math.ApproximatelyZero(radius))
            {
                PointFallback(position, "[<color=#adadadFF><icon=logMessage></color> Dot with radius of 0]<br>" + text, color, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            int numberOfDrawnRings = 4 + Mathf.RoundToInt(Mathf.Abs(density) * 14);
            float diameter = 2.0f * radius;
            float diameterReduction_perDrawnCircle = diameter / (float)numberOfDrawnRings;
            for (int i = 0; i < numberOfDrawnRings; i++)
            {
                string textAtCurrIcon = (i == 0) ? text : null;
                float sizeOfCurrIcon = diameter - diameterReduction_perDrawnCircle * i;
                Icon(position, DrawBasics.IconType.unitCircle, color, sizeOfCurrIcon, textAtCurrIcon, rotation, 0, false, durationInSec, hiddenByNearerObjects, textScaleFactor, minTextSize, autoFlipMirroredText_toFitObserverCam);
            }
        }

        static Color GetFadedColorFromSubLines(Color startColor, Color endColor, int usedSlotsInListOfSubLines, int i_startAnchorOfSubLine)
        {
            int numberOfSubLines = Mathf.RoundToInt(0.5f * usedSlotsInListOfSubLines);
            int numberOfSubLinesMinus1 = numberOfSubLines - 1;
            if (numberOfSubLinesMinus1 == 0)
            {
                return startColor;
            }

            int i_subLine = Mathf.RoundToInt(0.5f * i_startAnchorOfSubLine);

            if (i_subLine == 0)
            {
                return startColor;
            }
            else
            {
                if (i_subLine == (numberOfSubLines - 1))
                {
                    return endColor;
                }
                else
                {
                    float progress0to1 = (float)i_subLine / (float)(numberOfSubLinesMinus1);
                    return Color.Lerp(startColor, endColor, progress0to1);
                }
            }
        }

        public static Color GetFadedColorFromSegments(Color startColor, Color endColor, int i_segment, int segments)
        {
            if (segments == 0)
            {
                return startColor;
            }

            if (i_segment == 0)
            {
                return startColor;
            }
            else
            {
                if (i_segment == (segments - 1))
                {
                    return endColor;
                }
                else
                {
                    return Color.Lerp(startColor, endColor, (float)i_segment / (float)segments);
                }
            }
        }

        static Color GetColorWithAppliedAlpha_fromSubLines(Color colorToModify, bool hasAlphaFade, float alphaFadeOutLength_0to1, int i_subLineAnchor, int usedSlotsInListOfSubLines)
        {
            if (hasAlphaFade)
            {
                if (usedSlotsInListOfSubLines == 0)
                {
                    return colorToModify;
                }

                if (UtilitiesDXXL_Math.ApproximatelyZero(alphaFadeOutLength_0to1))
                {
                    return colorToModify;
                }

                float maxAllowedAlpha = 1.0f;
                if (i_subLineAnchor == 0 || i_subLineAnchor >= (usedSlotsInListOfSubLines - 2))
                {
                    maxAllowedAlpha = 0.1f;
                }
                float progress0to1 = (float)(i_subLineAnchor + 1) / (float)usedSlotsInListOfSubLines;
                return ModifyColorWithAlpha(colorToModify, progress0to1, alphaFadeOutLength_0to1, maxAllowedAlpha);
            }
            else
            {
                return colorToModify;
            }
        }

        static Color GetColorWithAppliedAlpha_fromSegments(Color colorToModify, bool hasAlphaFade, float alphaFadeOutLength_0to1, int i_segment, int segments)
        {
            if (hasAlphaFade)
            {
                int segmentsPlus1 = segments + 1;
                if (segmentsPlus1 == 0)
                {
                    return colorToModify;
                }

                if (UtilitiesDXXL_Math.ApproximatelyZero(alphaFadeOutLength_0to1))
                {
                    return colorToModify;
                }

                float maxAllowedAlpha = 1.0f;
                if (i_segment == 0 || i_segment >= (segments - 1))
                {
                    maxAllowedAlpha = 0.1f;
                }
                float progress0to1 = (float)(i_segment + 1) / (float)segmentsPlus1;
                return ModifyColorWithAlpha(colorToModify, progress0to1, alphaFadeOutLength_0to1, maxAllowedAlpha);
            }
            else
            {
                return colorToModify;
            }
        }

        static Color ModifyColorWithAlpha(Color colorToModify, float progress0to1, float alphaFadeOutLength_0to1, float maxAllowedAlpha)
        {
            if (progress0to1 < 0.5f)
            {
                if (progress0to1 < alphaFadeOutLength_0to1)
                {
                    float currAlpha = UtilitiesDXXL_Math.Get_2degParabolicSteepeningRise(progress0to1 / alphaFadeOutLength_0to1);
                    currAlpha = Mathf.Min(currAlpha, maxAllowedAlpha);
                    return UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorToModify, currAlpha);
                }
                else
                {
                    return colorToModify;
                }
            }
            else
            {
                float progressTillEnd_as0to1 = 1.0f - progress0to1;
                if (progressTillEnd_as0to1 < alphaFadeOutLength_0to1)
                {
                    float currAlpha = UtilitiesDXXL_Math.Get_2degParabolicSteepeningRise(progressTillEnd_as0to1 / alphaFadeOutLength_0to1);
                    currAlpha = Mathf.Min(currAlpha, maxAllowedAlpha);
                    return UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorToModify, currAlpha);
                }
                else
                {
                    return colorToModify;
                }
            }
        }

        public static void OverwriteDefaultVectorsWithStandardIdentity(ref Vector3 up, ref Vector3 forward, bool up_persistsDuringParallelCheck)
        {
            up = UtilitiesDXXL_Math.OverwriteDefaultVectors(up, Vector3.up);
            forward = UtilitiesDXXL_Math.OverwriteDefaultVectors(forward, Vector3.forward);
            if (up_persistsDuringParallelCheck)
            {
                forward = OverwriteParallelVectorsWithPerpVector(forward, up);
            }
            else
            {
                up = OverwriteParallelVectorsWithPerpVector(up, forward);
            }
        }

        static Vector3 OverwriteParallelVectorsWithPerpVector(Vector3 vectorToOverwrite_ifParallel, Vector3 referenceVector)
        {
            if (UtilitiesDXXL_Math.Check_ifTwoVectorsAreApproxParallel_butCanHeadToDifferntDirs_DXXL(vectorToOverwrite_ifParallel, referenceVector))
            {
                return UtilitiesDXXL_Math.Get_aNormalizedVector_perpToGivenVector(referenceVector);
            }
            else
            {
                return vectorToOverwrite_ifParallel;
            }
        }

    }

}
