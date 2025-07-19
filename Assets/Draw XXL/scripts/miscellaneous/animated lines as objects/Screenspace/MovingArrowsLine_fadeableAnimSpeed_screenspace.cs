namespace DrawXXL
{
    using UnityEngine;

    public class MovingArrowsLine_fadeableAnimSpeed_screenspace : ParentOf_Lines_fadeableAnimSpeed_screenspace
    {
        public Vector2 start = Vector2.zero;
        public Vector2 end = Vector2.one;
        public float distanceBetweenArrows_relToViewportHeight = 0.11f;
        public float lengthOfArrows_relToViewportHeight = 0.05f;
        public bool backwardAnimationFlipsArrowDirection = true;

        public MovingArrowsLine_fadeableAnimSpeed_screenspace(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
            width_relToViewportHeight = 0.016f;
            animationSpeed = 0.5f;
        }

        public override void Draw()
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (TryFetchCamera("MovingArrowsLine_fadeableAnimSpeed_screenspace.Draw") == false) { return; }

            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ParentOf_Lines_fadeableAnimSpeed_screenspace.Add(this);
                return;
            }

            lineAnimationProgress = InternalDraw(targetCamera, start, end, color, width_relToViewportHeight, distanceBetweenArrows_relToViewportHeight, lengthOfArrows_relToViewportHeight, text, animationSpeed, lineAnimationProgress, backwardAnimationFlipsArrowDirection, endPlatesSize_relToViewportHeight, durationInSec);
        }

        public static LineAnimationProgress InternalDraw(Camera targetCamera, Vector2 start, Vector2 end, Color color, float lineWidth_relToViewportHeight, float distanceBetweenArrows_relToViewportHeight, float lengthOfArrows_relToViewportHeight, string text, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, bool backwardAnimationFlipsArrowDirection, float endPlatesSize_relToViewportHeight, float durationInSec)
        {
            //"lengthOfArrows_relToViewportHeight" and "distanceBetweenArrows_relToViewportHeight": Very small (or very big) values may get rounded up (down) internally to prevent an explosive raise of drawn lines, see also "DrawBasics.MaxAllowedDrawnLinesPerFrame" (link).

            //Lines drawn with this function have a higher likelyhood of accidentially using up high numbers of drawnLinePerFrame, because "distanceBetweenArrows_relToViewportHeight" and "lengthOfArrows_relToViewportHeight" can be set manually instead of beeing determined by the lineStyle-code 
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return null; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return null; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lineWidth_relToViewportHeight, "lineWidth_relToViewportHeight")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(distanceBetweenArrows_relToViewportHeight, "distanceBetweenArrows_relToViewportHeight")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lengthOfArrows_relToViewportHeight, "lengthOfArrows_relToViewportHeight")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(animationSpeed, "animationSpeed")) { return null; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(end, "end")) { return null; }

            Vector3 start_worldSpace = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(targetCamera, start, false);
            Vector3 end_worldSpace = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(targetCamera, end, false);
            Vector2 lineCenter = 0.5f * (start + end);
            UtilitiesDXXL_Screenspace.camPlane.Recreate(start_worldSpace, targetCamera.transform.forward);

            lineWidth_relToViewportHeight = UtilitiesDXXL_Math.AbsNonZeroValue(lineWidth_relToViewportHeight);
            float lineWidth_worldSpace = UtilitiesDXXL_Math.ApproximatelyZero(lineWidth_relToViewportHeight) ? 0.0f : UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, lineCenter, true, lineWidth_relToViewportHeight);
            //small or big values of "distanceBetweenArrows" and "lengthOfArrows" may additionally get changed in "UtilitiesDXXL_LineStyles" inside the "skipPatternEnlargementFor*Lines == false" mechanic.
            distanceBetweenArrows_relToViewportHeight = Mathf.Max(distanceBetweenArrows_relToViewportHeight, 0.0002f);
            lengthOfArrows_relToViewportHeight = Mathf.Min(lengthOfArrows_relToViewportHeight, 0.9f * distanceBetweenArrows_relToViewportHeight);
            lengthOfArrows_relToViewportHeight = Mathf.Max(lengthOfArrows_relToViewportHeight, 0.0001f);
            float distanceBetweenArrows_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, lineCenter, true, distanceBetweenArrows_relToViewportHeight);
            float lengthOfArrows_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, lineCenter, true, lengthOfArrows_relToViewportHeight);
            float animationDirection = Mathf.Sign(animationSpeed);
            animationSpeed = UtilitiesDXXL_Screenspace.animationSpeedConversionFactor_viewportToWorldSpace * animationSpeed;
            float animationSpeed_worldSpace = UtilitiesDXXL_Math.ApproximatelyZero(animationSpeed) ? 0.0f : UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, lineCenter, true, animationSpeed);
            animationSpeed_worldSpace = animationSpeed_worldSpace * animationDirection;
            float minTextSize_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, lineCenter, true, DrawScreenspace.minTextSize_relToViewportHeight);
            float lengthOfEmptySpaces_worldSpace = distanceBetweenArrows_worldSpace - lengthOfArrows_worldSpace;
            float endPlatesSize_inAbsoluteWorldSpaceUnits = UtilitiesDXXL_Math.ApproximatelyZero(endPlatesSize_relToViewportHeight) ? 0.0f : UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, lineCenter, true, endPlatesSize_relToViewportHeight);
            LineAnimationProgress lineAnimProgressAfterDrawing = null;

            UtilitiesDXXL_LineStyles.curr_pointersDirAlongAnimationDir = backwardAnimationFlipsArrowDirection;
            UtilitiesDXXL_DrawBasics.Set_endPlates_sizeInterpretation_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
            try
            {
                UtilitiesDXXL_LineStyles.curr_dashLength_forArrowLine = lengthOfArrows_worldSpace;
                if (UtilitiesDXXL_Math.ApproximatelyZero(lineWidth_worldSpace) == false)
                {
                    UtilitiesDXXL_LineStyles.curr_minRatio_for_dashLengthToLineWidth_forArrowLine = lengthOfArrows_worldSpace / lineWidth_worldSpace;
                }
                UtilitiesDXXL_LineStyles.curr_spaceToDash_ratio_forArrowLine = lengthOfEmptySpaces_worldSpace / lengthOfArrows_worldSpace;
                UtilitiesDXXL_LineStyles.curr_minEmptySpacesLength_forArrowLine = lengthOfEmptySpaces_worldSpace;
                lineAnimProgressAfterDrawing = UtilitiesDXXL_DrawBasics.Line(start_worldSpace, end_worldSpace, color, lineWidth_worldSpace, text, DrawBasics.LineStyle.arrows, 1.0f, animationSpeed_worldSpace, precedingLineAnimationProgress, UtilitiesDXXL_Screenspace.camPlane, true, 0.0f, minTextSize_worldSpace, durationInSec, false, false, false, targetCamera, false, endPlatesSize_inAbsoluteWorldSpaceUnits);
            }
            catch { }

            UtilitiesDXXL_DrawBasics.Reverse_endPlates_sizeInterpretation();
            UtilitiesDXXL_LineStyles.curr_pointersDirAlongAnimationDir = UtilitiesDXXL_LineStyles.default_pointersDirAlongAnimationDir;
            UtilitiesDXXL_LineStyles.curr_dashLength_forArrowLine = UtilitiesDXXL_LineStyles.default_dashLength_forArrowLine;
            UtilitiesDXXL_LineStyles.curr_minRatio_for_dashLengthToLineWidth_forArrowLine = UtilitiesDXXL_LineStyles.default_minRatio_for_dashLengthToLineWidth_forArrowLine;
            UtilitiesDXXL_LineStyles.curr_spaceToDash_ratio_forArrowLine = UtilitiesDXXL_LineStyles.default_spaceToDash_ratio_forArrowLine;
            UtilitiesDXXL_LineStyles.curr_minEmptySpacesLength_forArrowLine = UtilitiesDXXL_LineStyles.default_minEmptySpacesLength_forArrowLine;

            return lineAnimProgressAfterDrawing;
        }
    }

}
