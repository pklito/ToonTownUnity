namespace DrawXXL
{
    using UnityEngine;

    public class MovingArrowsRay_fadeableAnimSpeed_screenspace : ParentOf_Lines_fadeableAnimSpeed_screenspace
    {
        public Vector2 start = Vector2.zero;
        public Vector2 direction = Vector2.one;
        public bool interpretDirectionAsUnwarped = false;
        public bool backwardAnimationFlipsArrowDirection = true;
        public float distanceBetweenArrows_relToViewportHeight = 0.11f;
        public float lengthOfArrows_relToViewportHeight = 0.05f;

        public MovingArrowsRay_fadeableAnimSpeed_screenspace(Vector2 start, Vector2 direction)
        {
            this.start = start;
            this.direction = direction;
            width_relToViewportHeight = 0.016f;
            animationSpeed = 0.5f;
        }

        public override void Draw()
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (TryFetchCamera("MovingArrowsRay_fadeableAnimSpeed_screenspace.Draw") == false) { return; }

            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ParentOf_Lines_fadeableAnimSpeed_screenspace.Add(this);
                return;
            }

            lineAnimationProgress = InternalDraw(targetCamera, start, direction, color, width_relToViewportHeight, distanceBetweenArrows_relToViewportHeight, lengthOfArrows_relToViewportHeight, text, animationSpeed, lineAnimationProgress, backwardAnimationFlipsArrowDirection, interpretDirectionAsUnwarped, endPlatesSize_relToViewportHeight, durationInSec);
        }

        public static LineAnimationProgress InternalDraw(Camera targetCamera, Vector2 start, Vector2 direction, Color color, float lineWidth_relToViewportHeight, float distanceBetweenArrows_relToViewportHeight, float lengthOfArrows_relToViewportHeight, string text, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, bool backwardAnimationFlipsArrowDirection, bool interpretDirectionAsUnwarped, float endPlatesSize_relToViewportHeight, float durationInSec)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(direction, "direction")) { return null; }

            Vector2 direction_inNonSquareViewportSpace = interpretDirectionAsUnwarped ? DrawScreenspace.DirectionInUnitsOfUnwarpedSpace_to_sameLookingDirectionInUnitsOfWarpedSpace(direction, targetCamera) : direction;
            Vector2 end = start + direction_inNonSquareViewportSpace;
            return MovingArrowsLine_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, start, end, color, lineWidth_relToViewportHeight, distanceBetweenArrows_relToViewportHeight, lengthOfArrows_relToViewportHeight, text, animationSpeed, precedingLineAnimationProgress, backwardAnimationFlipsArrowDirection, endPlatesSize_relToViewportHeight, durationInSec);
        }

    }

}
