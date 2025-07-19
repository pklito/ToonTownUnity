namespace DrawXXL
{
    using UnityEngine;

    public class LineWithAlternatingColors_fadeableAnimSpeed_2D : ParentOf_Lines_fadeableAnimSpeed_2D
    {
        public Vector2 start = Vector2.zero;
        public Vector2 end = Vector2.one;
        public Color alternatingColor = DrawBasics.defaultColor2_ofAlternatingColorLines;
        public float lengthOfStripes = 0.04f;
        public float alphaFadeOutLength_0to1 = 0.0f;
        public bool skipPatternEnlargementForLongLines = false;
        public bool skipPatternEnlargementForShortLines = false;

        public LineWithAlternatingColors_fadeableAnimSpeed_2D(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
        }

        public void Draw()
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            lineAnimationProgress = InternalDraw(start, end, color, alternatingColor, width, lengthOfStripes, text, custom_zPos, animationSpeed, lineAnimationProgress, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static LineAnimationProgress InternalDraw(Vector2 start, Vector2 end, Color color1, Color color2, float width, float lengthOfStripes, string text, float custom_zPos, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, float endPlates_size, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects, bool skipPatternEnlargementForLongLines, bool skipPatternEnlargementForShortLines)
        {
            //Lines drawn with this function have a higher likelyhood of accidentially using up high numbers of drawnLinePerFrame, because "lengthOfStripes" can be set manually instead of beeing determined by the lineStyle-code 
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lengthOfStripes, "lengthOfStripes")) { return null; }

            lengthOfStripes = Mathf.Max(lengthOfStripes, 0.001f);
            UtilitiesDXXL_LineStyles.curr_dashLength_forAlternatingColorStripesLine = lengthOfStripes;
            Color prev_defaultAlternateColorOfStripedLines = DrawBasics.defaultColor2_ofAlternatingColorLines;
            if (UtilitiesDXXL_Colors.IsDefaultColor(color2) == false)
            {
                DrawBasics.defaultColor2_ofAlternatingColorLines = color2;
            }
            LineAnimationProgress lineAnimationProgress = Line_fadeableAnimSpeed_2D.InternalDraw(start, end, color1, width, text, DrawBasics.LineStyle.alternatingColorStripes, custom_zPos, 1.0f, animationSpeed, precedingLineAnimationProgress, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
            UtilitiesDXXL_LineStyles.curr_dashLength_forAlternatingColorStripesLine = UtilitiesDXXL_LineStyles.default_dashLength_forAlternatingColorStripesLine;
            DrawBasics.defaultColor2_ofAlternatingColorLines = prev_defaultAlternateColorOfStripedLines;
            return lineAnimationProgress;
        }
    }

}
