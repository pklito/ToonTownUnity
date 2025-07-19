namespace DrawXXL
{
    using UnityEngine;

    public class MovingArrowsRay_fadeableAnimSpeed : ParentOf_Lines_fadeableAnimSpeed_3D
    {
        public Vector3 start = Vector3.zero;
        public Vector3 direction = Vector3.one;
        public float distanceBetweenArrows = 0.5f;
        public float lengthOfArrows = 0.15f;
        public bool backwardAnimationFlipsArrowDirection = true;

        public MovingArrowsRay_fadeableAnimSpeed(Vector3 start, Vector3 direction)
        {
            this.start = start;
            this.direction = direction;
            width = 0.05f;
            animationSpeed = 0.5f;
            flattenThickRoundLineIntoAmplitudePlane = true;
        }

        public void Draw()
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            lineAnimationProgress = InternalDraw(start, direction, color, width, distanceBetweenArrows, lengthOfArrows, text, animationSpeed, lineAnimationProgress, backwardAnimationFlipsArrowDirection, flattenThickRoundLineIntoAmplitudePlane, customAmplitudeAndTextDir, endPlates_size, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects);
        }

        public static LineAnimationProgress InternalDraw(Vector3 start, Vector3 direction, Color color, float lineWidth, float distanceBetweenArrows, float lengthOfArrows, string text, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, bool backwardAnimationFlipsArrowDirection, bool flattenThickRoundLineIntoAmplitudePlane, Vector3 customAmplitudeAndTextDir, float endPlates_size, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(direction, "direction")) { return null; }

            Vector3 end = start + direction;
            return MovingArrowsLine_fadeableAnimSpeed.InternalDraw(start, end, color, lineWidth, distanceBetweenArrows, lengthOfArrows, text, animationSpeed, precedingLineAnimationProgress, backwardAnimationFlipsArrowDirection, flattenThickRoundLineIntoAmplitudePlane, customAmplitudeAndTextDir, endPlates_size, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects);
        }
    }

}
