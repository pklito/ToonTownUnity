namespace DrawXXL
{
    using UnityEngine;

    public class Line_fadeableAnimSpeed_2D : ParentOf_Lines_fadeableAnimSpeed_2D
    {
        public Vector2 start = Vector2.zero;
        public Vector2 end = Vector2.one;
        public Color endColor = default(Color); //Can be ignored if the line should have only one color. If it is specified then the line will have a fading color transition, starting with "color"(link) at the start of the line and ending with "endColor" at the end of the line
        public DrawBasics.LineStyle style = DrawBasics.LineStyle.solid;
        public float stylePatternScaleFactor = 1.0f;
        public float alphaFadeOutLength_0to1 = 0.0f;
        public bool skipPatternEnlargementForLongLines = false;
        public bool skipPatternEnlargementForShortLines = false;

        public Line_fadeableAnimSpeed_2D(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
        }

        public void Draw()
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Colors.IsDefaultColor(endColor))
            {
                lineAnimationProgress = InternalDraw(start, end, color, width, text, style, custom_zPos, stylePatternScaleFactor, animationSpeed, lineAnimationProgress, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
            }
            else
            {
                lineAnimationProgress = InternalDrawColorFade(start, end, color, endColor, width, text, style, custom_zPos, stylePatternScaleFactor, animationSpeed, lineAnimationProgress, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
            }
        }

        public static LineAnimationProgress InternalDraw(Vector2 start, Vector2 end, Color color, float width, string text, DrawBasics.LineStyle style, float custom_zPos, float stylePatternScaleFactor, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, float endPlates_size, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects, bool skipPatternEnlargementForLongLines, bool skipPatternEnlargementForShortLines)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 startV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(start, zPos);
            Vector3 endV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(end, zPos);
            style = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(style);
            return UtilitiesDXXL_DrawBasics.Line(startV3, endV3, color, width, text, style, stylePatternScaleFactor, animationSpeed, precedingLineAnimationProgress, UtilitiesDXXL_DrawBasics2D.xyPlane_throughZero, true, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines, null, true, endPlates_size);
        }

        public static LineAnimationProgress InternalDrawColorFade(Vector2 start, Vector2 end, Color startColor, Color endColor, float width, string text, DrawBasics.LineStyle style, float custom_zPos, float stylePatternScaleFactor, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, float endPlates_size, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects, bool skipPatternEnlargementForLongLines, bool skipPatternEnlargementForShortLines)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 startV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(start, zPos);
            Vector3 endV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(end, zPos);
            style = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(style);
            return UtilitiesDXXL_DrawBasics.LineColorFade(startV3, endV3, startColor, endColor, width, text, style, stylePatternScaleFactor, animationSpeed, precedingLineAnimationProgress, UtilitiesDXXL_DrawBasics2D.xyPlane_throughZero, true, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines, null, true, endPlates_size, 1.0f);
        }
    }

}
