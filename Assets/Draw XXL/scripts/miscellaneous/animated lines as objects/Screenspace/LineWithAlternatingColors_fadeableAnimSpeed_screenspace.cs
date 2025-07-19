namespace DrawXXL
{
    using UnityEngine;

    public class LineWithAlternatingColors_fadeableAnimSpeed_screenspace : ParentOf_Lines_fadeableAnimSpeed_screenspace
    {
        public Vector2 start = Vector2.zero;
        public Vector2 end = Vector2.one;
        public Color alternatingColor = DrawBasics.defaultColor2_ofAlternatingColorLines;
        public float lengthOfStripes_relToViewportHeight = 0.03f;
        public float alphaFadeOutLength_0to1 = 0.0f;

        public LineWithAlternatingColors_fadeableAnimSpeed_screenspace(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
        }

        public override void Draw()
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (TryFetchCamera("LineWithAlternatingColors_fadeableAnimSpeed_screenspace.Draw") == false) { return; }

            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ParentOf_Lines_fadeableAnimSpeed_screenspace.Add(this);
                return;
            }

            lineAnimationProgress = InternalDraw(targetCamera, start, end, color, alternatingColor, width_relToViewportHeight, lengthOfStripes_relToViewportHeight, text, animationSpeed, lineAnimationProgress, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, durationInSec);
        }

        public static LineAnimationProgress InternalDraw(Camera targetCamera, Vector2 start, Vector2 end, Color color1, Color color2, float lineWidth_relToViewportHeight, float lengthOfStripes_relToViewportHeight, string text, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, float endPlatesSize_relToViewportHeight, float alphaFadeOutLength_0to1, float durationInSec)
        {
            //Lines drawn with this function have a higher likelyhood of accidentially using up high numbers of drawnLinePerFrame, because "lengthOfStripes" can be set manually instead of beeing determined by the lineStyle-code 
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return null; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return null; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lineWidth_relToViewportHeight, "lineWidth_relToViewportHeight")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lengthOfStripes_relToViewportHeight, "lengthOfStripes_relToViewportHeight")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(animationSpeed, "animationSpeed")) { return null; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(end, "end")) { return null; }

            Vector3 start_worldSpace = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(targetCamera, start, false);
            Vector3 end_worldSpace = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(targetCamera, end, false);
            Vector2 lineCenter = 0.5f * (start + end);
            UtilitiesDXXL_Screenspace.camPlane.Recreate(start_worldSpace, targetCamera.transform.forward);

            lineWidth_relToViewportHeight = UtilitiesDXXL_Math.AbsNonZeroValue(lineWidth_relToViewportHeight);
            float lineWidth_worldSpace = UtilitiesDXXL_Math.ApproximatelyZero(lineWidth_relToViewportHeight) ? 0.0f : UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, lineCenter, true, lineWidth_relToViewportHeight);
            float animationDirection = Mathf.Sign(animationSpeed);
            animationSpeed = UtilitiesDXXL_Screenspace.animationSpeedConversionFactor_viewportToWorldSpace * animationSpeed;
            float animationSpeed_worldSpace = UtilitiesDXXL_Math.ApproximatelyZero(animationSpeed) ? 0.0f : UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, lineCenter, true, animationSpeed);
            animationSpeed_worldSpace = animationSpeed_worldSpace * animationDirection;
            float minTextSize_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, lineCenter, true, DrawScreenspace.minTextSize_relToViewportHeight);
            lengthOfStripes_relToViewportHeight = Mathf.Max(lengthOfStripes_relToViewportHeight, UtilitiesDXXL_DrawBasics.min_lengthOfStripes_ofAlternatingColorLine);
            float lengthOfStripes_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, lineCenter, true, lengthOfStripes_relToViewportHeight);
            UtilitiesDXXL_LineStyles.curr_dashLength_forAlternatingColorStripesLine = lengthOfStripes_worldSpace;
            Color prev_defaultAlternateColorOfStripedLines = DrawBasics.defaultColor2_ofAlternatingColorLines;
            if (UtilitiesDXXL_Colors.IsDefaultColor(color2) == false)
            {
                DrawBasics.defaultColor2_ofAlternatingColorLines = color2;
            }
            float endPlatesSize_inAbsoluteWorldSpaceUnits = UtilitiesDXXL_Math.ApproximatelyZero(endPlatesSize_relToViewportHeight) ? 0.0f : UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, lineCenter, true, endPlatesSize_relToViewportHeight);

            UtilitiesDXXL_DrawBasics.Set_endPlates_sizeInterpretation_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
            LineAnimationProgress lineAnimProgressAfterDrawing = UtilitiesDXXL_DrawBasics.Line(start_worldSpace, end_worldSpace, color1, lineWidth_worldSpace, text, DrawBasics.LineStyle.alternatingColorStripes, 1.0f, animationSpeed_worldSpace, precedingLineAnimationProgress, UtilitiesDXXL_Screenspace.camPlane, true, alphaFadeOutLength_0to1, minTextSize_worldSpace, durationInSec, false, false, false, targetCamera, false, endPlatesSize_inAbsoluteWorldSpaceUnits);
            UtilitiesDXXL_DrawBasics.Reverse_endPlates_sizeInterpretation();

            UtilitiesDXXL_LineStyles.curr_dashLength_forAlternatingColorStripesLine = UtilitiesDXXL_LineStyles.default_dashLength_forAlternatingColorStripesLine;
            DrawBasics.defaultColor2_ofAlternatingColorLines = prev_defaultAlternateColorOfStripedLines;

            return lineAnimProgressAfterDrawing;
        }

    }

}
