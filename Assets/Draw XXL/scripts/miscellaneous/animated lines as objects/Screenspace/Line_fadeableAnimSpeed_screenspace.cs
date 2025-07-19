namespace DrawXXL
{
    using UnityEngine;

    public class Line_fadeableAnimSpeed_screenspace : ParentOf_Lines_fadeableAnimSpeed_screenspace
    {
        public Vector2 start = Vector2.zero;
        public Vector2 end = Vector2.one;
        public Color endColor = default(Color); //Can be ignored if the line should have only one color. If it is specified then the line will have a fading color transition, starting with "color"(link) at the start of the line and ending with "endColor" at the end of the line
        public DrawBasics.LineStyle style = DrawBasics.LineStyle.solid;
        public float stylePatternScaleFactor = 1.0f;
        public float alphaFadeOutLength_0to1 = 0.0f;
        public float enlargeSmallTextToThisMinRelTextSize = DrawScreenspace.minTextSize_relToViewportHeight;

        public Line_fadeableAnimSpeed_screenspace(Vector2 start, Vector2 end)
        {
            this.start = start;
            this.end = end;
        }

        public override void Draw()
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (TryFetchCamera("Line_fadeableAnimSpeed_screenspace.Draw") == false) { return; }

            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ParentOf_Lines_fadeableAnimSpeed_screenspace.Add(this);
                return;
            }

            if (UtilitiesDXXL_Colors.IsDefaultColor(endColor))
            {
                lineAnimationProgress = InternalDraw(targetCamera, start, end, color, width_relToViewportHeight, text, style, stylePatternScaleFactor, animationSpeed, lineAnimationProgress, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
            }
            else
            {
                lineAnimationProgress = InternalDrawColorFade(targetCamera, start, end, color, endColor, width_relToViewportHeight, text, style, stylePatternScaleFactor, animationSpeed, lineAnimationProgress, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
            }
        }

        public static LineAnimationProgress InternalDraw(Camera targetCamera, Vector2 start, Vector2 end, Color color, float width_relToViewportHeight, string text, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, float endPlatesSize_relToViewportHeight, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinRelTextSize, float durationInSec)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return null; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return null; }

            InternalDXXL_LineParamsFromCamViewportSpace lineParams = UtilitiesDXXL_Screenspace.GetLineParamsFromCamViewportSpace(targetCamera, start, end, width_relToViewportHeight, style, stylePatternScaleFactor, enlargeSmallTextToThisMinRelTextSize, animationSpeed, endPlatesSize_relToViewportHeight);
            if (lineParams == null)
            {
                return null;
            }
            else
            {
                UtilitiesDXXL_DrawBasics.Set_endPlates_sizeInterpretation_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                LineAnimationProgress returned_lineAnimationProgress = UtilitiesDXXL_DrawBasics.Line(lineParams.startAnchor_worldSpace, lineParams.endAnchor_worldSpace, color, lineParams.width_worldSpace, text, lineParams.lineStyleForcedTo2D, lineParams.patternScaleFactor_worldSpace, lineParams.animationSpeed_worldSpace, precedingLineAnimationProgress, lineParams.camPlane, true, alphaFadeOutLength_0to1, lineParams.enlargeSmallTextToThisMinTextSize_worldSpace, durationInSec, false, false, false, targetCamera, false, lineParams.endPlatesSize_inAbsoluteWorldSpaceUnits);
                UtilitiesDXXL_DrawBasics.Reverse_endPlates_sizeInterpretation();
                return returned_lineAnimationProgress;
            }
        }

        public static LineAnimationProgress InternalDrawColorFade(Camera targetCamera, Vector2 start, Vector2 end, Color startColor, Color endColor, float width_relToViewportHeight, string text, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, LineAnimationProgress precedingLineAnimationProgress, float endPlatesSize_relToViewportHeight, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinRelTextSize, float durationInSec)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return null; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return null; }
            InternalDXXL_LineParamsFromCamViewportSpace lineParams = UtilitiesDXXL_Screenspace.GetLineParamsFromCamViewportSpace(targetCamera, start, end, width_relToViewportHeight, style, stylePatternScaleFactor, enlargeSmallTextToThisMinRelTextSize, animationSpeed, endPlatesSize_relToViewportHeight);
            if (lineParams == null)
            {
                return null;
            }
            else
            {
                UtilitiesDXXL_DrawBasics.Set_endPlates_sizeInterpretation_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                LineAnimationProgress returned_lineAnimationProgress = UtilitiesDXXL_DrawBasics.LineColorFade(lineParams.startAnchor_worldSpace, lineParams.endAnchor_worldSpace, startColor, endColor, lineParams.width_worldSpace, text, lineParams.lineStyleForcedTo2D, lineParams.patternScaleFactor_worldSpace, lineParams.animationSpeed_worldSpace, precedingLineAnimationProgress, lineParams.camPlane, true, alphaFadeOutLength_0to1, lineParams.enlargeSmallTextToThisMinTextSize_worldSpace, durationInSec, false, false, false, targetCamera, false, lineParams.endPlatesSize_inAbsoluteWorldSpaceUnits, 1.0f);
                UtilitiesDXXL_DrawBasics.Reverse_endPlates_sizeInterpretation();

                return returned_lineAnimationProgress;
            }
        }

    }

}
