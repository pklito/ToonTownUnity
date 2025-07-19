namespace DrawXXL
{
    using UnityEngine;

    public class RayWithAlternatingColors_fadeableAnimSpeed_screenspace : ParentOf_Lines_fadeableAnimSpeed_screenspace
    {
        public Vector2 start = Vector2.zero;
        public Vector2 direction = Vector2.one;
        public Color alternatingColor = DrawBasics.defaultColor2_ofAlternatingColorLines;
        public float lengthOfStripes_relToViewportHeight = 0.03f;
        public bool interpretDirectionAsUnwarped = false;
        public float alphaFadeOutLength_0to1 = 0.0f;

        public RayWithAlternatingColors_fadeableAnimSpeed_screenspace(Vector2 start, Vector2 direction)
        {
            this.start = start;
            this.direction = direction;
        }

        public override void Draw()
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (TryFetchCamera("RayWithAlternatingColors_fadeableAnimSpeed_screenspace.Draw") == false) { return; }

            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ParentOf_Lines_fadeableAnimSpeed_screenspace.Add(this);
                return;
            }

            lineAnimationProgress = InternalDraw(targetCamera, start, direction, color, alternatingColor, width_relToViewportHeight, lengthOfStripes_relToViewportHeight, text, interpretDirectionAsUnwarped, animationSpeed, lineAnimationProgress, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, durationInSec);
        }

        public static LineAnimationProgress InternalDraw(Camera targetCamera, Vector2 start, Vector2 direction, Color color1, Color color2, float lineWidth_relToViewportHeight, float lengthOfStripes_relToViewportHeight, string text, bool interpretDirectionAsUnwarped, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, float endPlatesSize_relToViewportHeight, float alphaFadeOutLength_0to1, float durationInSec)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(direction, "direction")) { return null; }

            Vector2 direction_inNonSquareViewportSpace = interpretDirectionAsUnwarped ? DrawScreenspace.DirectionInUnitsOfUnwarpedSpace_to_sameLookingDirectionInUnitsOfWarpedSpace(direction, targetCamera) : direction;
            Vector2 end = start + direction_inNonSquareViewportSpace;
            return LineWithAlternatingColors_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, start, end, color1, color2, lineWidth_relToViewportHeight, lengthOfStripes_relToViewportHeight, text, animationSpeed, precedingLineAnimationProgress, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, durationInSec);
        }
    }

}
