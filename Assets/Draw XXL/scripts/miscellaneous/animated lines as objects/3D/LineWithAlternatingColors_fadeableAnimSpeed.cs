namespace DrawXXL
{
    using UnityEngine;

    public class LineWithAlternatingColors_fadeableAnimSpeed : ParentOf_Lines_fadeableAnimSpeed_3D
    {
        public Vector3 start = Vector3.zero;
        public Vector3 end = Vector3.one;
        public Color alternatingColor = DrawBasics.defaultColor2_ofAlternatingColorLines;
        public float lengthOfStripes = 0.04f;
        public float alphaFadeOutLength_0to1 = 0.0f;
        public bool skipPatternEnlargementForLongLines = false;
        public bool skipPatternEnlargementForShortLines = false;

        public LineWithAlternatingColors_fadeableAnimSpeed(Vector3 start, Vector3 end)
        {
            this.start = start;
            this.end = end;
        }

        public void Draw()
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            lineAnimationProgress = InternalDraw(start, end, color, alternatingColor, width, lengthOfStripes, text, animationSpeed, lineAnimationProgress, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static LineAnimationProgress InternalDraw(Vector3 start, Vector3 end, Color color1, Color color2, float width, float lengthOfStripes, string text, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, Vector3 customAmplitudeAndTextDir, bool flattenThickRoundLineIntoAmplitudePlane, float endPlates_size, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects, bool skipPatternEnlargementForLongLines, bool skipPatternEnlargementForShortLines)
        {
            //Lines drawn with this function have a higher likelyhood of accidentially using up high numbers of drawnLinePerFrame, because "lengthOfStripes" can be set manually instead of beeing determined by the lineStyle-code 
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lengthOfStripes, "lengthOfStripes")) { return null; }

            lengthOfStripes = Mathf.Max(lengthOfStripes, UtilitiesDXXL_DrawBasics.min_lengthOfStripes_ofAlternatingColorLine);
            UtilitiesDXXL_LineStyles.curr_dashLength_forAlternatingColorStripesLine = lengthOfStripes;
            Color prev_defaultAlternateColorOfStripedLines = DrawBasics.defaultColor2_ofAlternatingColorLines;
            if (UtilitiesDXXL_Colors.IsDefaultColor(color2) == false)
            {
                DrawBasics.defaultColor2_ofAlternatingColorLines = color2;
            }
            LineAnimationProgress lineAnimationProgress = Line_fadeableAnimSpeed.InternalDraw(start, end, color1, width, text, DrawBasics.LineStyle.alternatingColorStripes, 1.0f, animationSpeed, precedingLineAnimationProgress, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
            UtilitiesDXXL_LineStyles.curr_dashLength_forAlternatingColorStripesLine = UtilitiesDXXL_LineStyles.default_dashLength_forAlternatingColorStripesLine;
            DrawBasics.defaultColor2_ofAlternatingColorLines = prev_defaultAlternateColorOfStripedLines;

            return lineAnimationProgress;
        }

    }

}
