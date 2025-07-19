namespace DrawXXL
{
    using UnityEngine;

    public class MovingArrowsLine_fadeableAnimSpeed : ParentOf_Lines_fadeableAnimSpeed_3D
    {
        public Vector3 start = Vector3.zero;
        public Vector3 end = Vector3.one;
        public float distanceBetweenArrows = 0.5f;
        public float lengthOfArrows = 0.15f;
        public bool backwardAnimationFlipsArrowDirection = true;

        public MovingArrowsLine_fadeableAnimSpeed(Vector3 start, Vector3 end)
        {
            this.start = start;
            this.end = end;
            width = 0.05f;
            animationSpeed = 0.5f;
            flattenThickRoundLineIntoAmplitudePlane = true;
        }

        public void Draw()
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            lineAnimationProgress = InternalDraw(start, end, color, width, distanceBetweenArrows, lengthOfArrows, text, animationSpeed, lineAnimationProgress, backwardAnimationFlipsArrowDirection, flattenThickRoundLineIntoAmplitudePlane, customAmplitudeAndTextDir, endPlates_size, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects);
        }

        public static LineAnimationProgress InternalDraw(Vector3 start, Vector3 end, Color color, float lineWidth, float distanceBetweenArrows, float lengthOfArrows, string text, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, bool backwardAnimationFlipsArrowDirection, bool flattenThickRoundLineIntoAmplitudePlane, Vector3 customAmplitudeAndTextDir, float endPlates_size, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            return UtilitiesDXXL_DrawBasics.MovingArrowsLine(start, end, color, lineWidth, distanceBetweenArrows, lengthOfArrows, text, animationSpeed, precedingLineAnimationProgress, backwardAnimationFlipsArrowDirection, flattenThickRoundLineIntoAmplitudePlane, customAmplitudeAndTextDir, endPlates_size, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, false);
        }
    }

}
