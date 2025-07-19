namespace DrawXXL
{
    using UnityEngine;

    public class MovingArrowsRay_fadeableAnimSpeed_2D : ParentOf_Lines_fadeableAnimSpeed_2D
    {
        public Vector2 start = Vector2.zero;
        public Vector2 direction = Vector2.one;
        public float distanceBetweenArrows = 0.5f;
        public float lengthOfArrows = 0.15f;
        public bool backwardAnimationFlipsArrowDirection = true;

        public MovingArrowsRay_fadeableAnimSpeed_2D(Vector2 start, Vector2 direction)
        {
            this.start = start;
            this.direction = direction;
            width = 0.05f;
            animationSpeed = 0.5f;
        }

        public void Draw()
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            lineAnimationProgress = InternalDraw(start, direction, color, width, distanceBetweenArrows, lengthOfArrows, text, custom_zPos, animationSpeed, lineAnimationProgress, backwardAnimationFlipsArrowDirection, endPlates_size, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects);
        }

        public static LineAnimationProgress InternalDraw(Vector2 start, Vector2 direction, Color color, float lineWidth, float distanceBetweenArrows, float lengthOfArrows, string text, float custom_zPos, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, bool backwardAnimationFlipsArrowDirection, float endPlates_size, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(direction, "direction")) { return null; }

            Vector2 end = start + direction;
            return MovingArrowsLine_fadeableAnimSpeed_2D.InternalDraw(start, end, color, lineWidth, distanceBetweenArrows, lengthOfArrows, text, custom_zPos, animationSpeed, precedingLineAnimationProgress, backwardAnimationFlipsArrowDirection, endPlates_size, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects);
        }

    }

}
