namespace DrawXXL
{
    using UnityEngine;

    public class RayWithAlternatingColors_fadeableAnimSpeed : ParentOf_Lines_fadeableAnimSpeed_3D
    {
        public Vector3 start = Vector3.zero;
        public Vector3 direction = Vector3.one;
        public Color alternatingColor = DrawBasics.defaultColor2_ofAlternatingColorLines;
        public float lengthOfStripes = 0.04f;
        public float alphaFadeOutLength_0to1 = 0.0f;
        public bool skipPatternEnlargementForLongLines = false;
        public bool skipPatternEnlargementForShortLines = false;

        public RayWithAlternatingColors_fadeableAnimSpeed(Vector3 start, Vector3 direction)
        {
            this.start = start;
            this.direction = direction;
        }

        public void Draw()
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            lineAnimationProgress = InternalDraw(start, direction, color, alternatingColor, width, lengthOfStripes, text, animationSpeed, lineAnimationProgress, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static LineAnimationProgress InternalDraw(Vector3 start, Vector3 direction, Color color1, Color color2, float width, float lengthOfStripes, string text, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, Vector3 customAmplitudeAndTextDir, bool flattenThickRoundLineIntoAmplitudePlane, float endPlates_size, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects, bool skipPatternEnlargementForLongLines, bool skipPatternEnlargementForShortLines)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(direction, "direction")) { return null; }

            Vector3 end = start + direction;
            return LineWithAlternatingColors_fadeableAnimSpeed.InternalDraw(start, end, color1, color2, width, lengthOfStripes, text, animationSpeed, precedingLineAnimationProgress, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }
    }

}
