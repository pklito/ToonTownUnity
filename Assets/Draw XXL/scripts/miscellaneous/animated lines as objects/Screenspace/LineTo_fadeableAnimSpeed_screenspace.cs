namespace DrawXXL
{
    using UnityEngine;

    public class LineTo_fadeableAnimSpeed_screenspace : ParentOf_Lines_fadeableAnimSpeed_screenspace
    {
        public Vector2 direction = Vector2.one;
        public Vector2 end = Vector2.one;
        public Color endColor = default(Color); //Can be ignored if the line should have only one color. If it is specified then the line will have a fading color transition, starting with "color"(link) at the start of the line and ending with "endColor" at the end of the line
        public DrawBasics.LineStyle style = DrawBasics.LineStyle.solid;
        public float stylePatternScaleFactor = 1.0f;
        public bool interpretDirectionAsUnwarped = false;
        public float alphaFadeOutLength_0to1 = 0.0f;
        public float enlargeSmallTextToThisMinRelTextSize = DrawScreenspace.minTextSize_relToViewportHeight;

        public LineTo_fadeableAnimSpeed_screenspace(Vector2 direction, Vector2 end)
        {
            this.direction = direction;
            this.end = end;
        }

        public override void Draw()
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (TryFetchCamera("LineTo_fadeableAnimSpeed_screenspace.Draw") == false) { return; }

            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ParentOf_Lines_fadeableAnimSpeed_screenspace.Add(this);
                return;
            }

            if (UtilitiesDXXL_Colors.IsDefaultColor(endColor))
            {
                lineAnimationProgress = InternalDraw(targetCamera, direction, end, color, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, lineAnimationProgress, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
            }
            else
            {
                lineAnimationProgress = InternalDraw_withColorFade(targetCamera, direction, end, color, endColor, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, lineAnimationProgress, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
            }
        }

        public static LineAnimationProgress InternalDraw(Camera targetCamera, Vector2 direction, Vector2 end, Color color, float width_relToViewportHeight, string text, bool interpretDirectionAsUnwarped, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, float endPlatesSize_relToViewportHeight, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinRelTextSize, float durationInSec)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(end, "end")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(direction, "direction")) { return null; }
            return Ray_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, end - direction, direction, color, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, precedingLineAnimationProgress, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

        public static LineAnimationProgress InternalDraw_withColorFade(Camera targetCamera, Vector2 direction, Vector2 end, Color startColor, Color endColor, float width_relToViewportHeight, string text, bool interpretDirectionAsUnwarped, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, float endPlatesSize_relToViewportHeight, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinRelTextSize, float durationInSec)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(end, "end")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(direction, "direction")) { return null; }
            return Ray_fadeableAnimSpeed_screenspace.InternalDrawColorFade(targetCamera, end - direction, direction, startColor, endColor, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, precedingLineAnimationProgress, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

    }

}
