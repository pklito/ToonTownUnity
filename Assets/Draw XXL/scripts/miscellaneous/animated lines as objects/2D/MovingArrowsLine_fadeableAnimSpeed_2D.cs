namespace DrawXXL
{
    using UnityEngine;

    public class MovingArrowsLine_fadeableAnimSpeed_2D : ParentOf_Lines_fadeableAnimSpeed_2D
    {
        public Vector2 start = Vector2.zero;
        public Vector2 end = Vector2.one;
        public float distanceBetweenArrows = 0.5f;
        public float lengthOfArrows = 0.15f;
        public bool backwardAnimationFlipsArrowDirection = true;

        public MovingArrowsLine_fadeableAnimSpeed_2D(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
            width = 0.05f;
            animationSpeed = 0.5f;
        }

        public void Draw()
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            lineAnimationProgress = InternalDraw(start, end, color, width, distanceBetweenArrows, lengthOfArrows, text, custom_zPos, animationSpeed, lineAnimationProgress, backwardAnimationFlipsArrowDirection, endPlates_size, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects);
        }

        public static LineAnimationProgress InternalDraw(Vector2 start, Vector2 end, Color color, float lineWidth, float distanceBetweenArrows, float lengthOfArrows, string text, float custom_zPos, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, bool backwardAnimationFlipsArrowDirection, float endPlates_size, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 startV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(start, zPos);
            Vector3 endV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(end, zPos);
            return UtilitiesDXXL_DrawBasics.MovingArrowsLine(startV3, endV3, color, lineWidth, distanceBetweenArrows, lengthOfArrows, text, animationSpeed, precedingLineAnimationProgress, backwardAnimationFlipsArrowDirection, true, UtilitiesDXXL_DrawBasics2D.xyPlane_throughZero, endPlates_size, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, true);
        }

    }

}
