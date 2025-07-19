namespace DrawXXL
{
    using System.Collections.Generic;
    using UnityEngine;

    public class DrawScreenspace
    {
        public enum DefaultScreenspaceWindowForDrawing
        {
            sceneViewWindow,
            gameViewWindow
        };
        public static DefaultScreenspaceWindowForDrawing defaultScreenspaceWindowForDrawing = DefaultScreenspaceWindowForDrawing.gameViewWindow;

        public static Camera defaultCameraForDrawing = null;
        public const float minTextSize_relToViewportHeight = 0.01f;
        public static float drawOffsetBehindCamsNearPlane = 0.01f;

        public static void Line(Vector2 start, Vector2 end, Color color = default(Color), float width_relToViewportHeight = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Line") == false) { return; }
            Line(automaticallyFoundCamera, start, end, color, width_relToViewportHeight, text, style, stylePatternScaleFactor, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

        public static void Line(Camera targetCamera, Vector2 start, Vector2 end, Color color = default(Color), float width_relToViewportHeight = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceLine.Add(new ScreenspaceLine(targetCamera, start, end, color, width_relToViewportHeight, text, style, stylePatternScaleFactor, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec));
                return;
            }

            Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, start, end, color, width_relToViewportHeight, text, style, stylePatternScaleFactor, animationSpeed, null, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

        public static void Ray(Vector2 start, Vector2 direction, Color color = default(Color), float width_relToViewportHeight = 0.0f, string text = null, bool interpretDirectionAsUnwarped = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Ray") == false) { return; }
            Ray(automaticallyFoundCamera, start, direction, color, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }
        public static void Ray(Camera targetCamera, Vector2 start, Vector2 direction, Color color = default(Color), float width_relToViewportHeight = 0.0f, string text = null, bool interpretDirectionAsUnwarped = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceRay.Add(new ScreenspaceRay(targetCamera, start, direction, color, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec));
                return;
            }

            Ray_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, start, direction, color, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, null, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

        public static void LineFrom(Vector2 start, Vector2 direction, Color color = default(Color), float width_relToViewportHeight = 0.0f, string text = null, bool interpretDirectionAsUnwarped = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.LineFrom") == false) { return; }
            LineFrom(automaticallyFoundCamera, start, direction, color, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

        public static void LineFrom(Camera targetCamera, Vector2 start, Vector2 direction, Color color = default(Color), float width_relToViewportHeight = 0.0f, string text = null, bool interpretDirectionAsUnwarped = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceLineFrom.Add(new ScreenspaceLineFrom(targetCamera, start, direction, color, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec));
                return;
            }

            LineFrom_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, start, direction, color, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, null, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

        public static void LineTo(Vector2 direction, Vector2 end, Color color = default(Color), float width_relToViewportHeight = 0.0f, string text = null, bool interpretDirectionAsUnwarped = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.LineTo") == false) { return; }
            LineTo(automaticallyFoundCamera, direction, end, color, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

        public static void LineTo(Camera targetCamera, Vector2 direction, Vector2 end, Color color = default(Color), float width_relToViewportHeight = 0.0f, string text = null, bool interpretDirectionAsUnwarped = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceLineTo.Add(new ScreenspaceLineTo(targetCamera, direction, end, color, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec));
                return;
            }

            LineTo_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, direction, end, color, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, null, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

        public static void LineColorFade(Vector2 start, Vector2 end, Color startColor, Color endColor, float width_relToViewportHeight = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.LineColorFade") == false) { return; }
            LineColorFade(automaticallyFoundCamera, start, end, startColor, endColor, width_relToViewportHeight, text, style, stylePatternScaleFactor, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }
        public static void LineColorFade(Camera targetCamera, Vector2 start, Vector2 end, Color startColor, Color endColor, float width_relToViewportHeight = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceLineColorFade.Add(new ScreenspaceLineColorFade(targetCamera, start, end, startColor, endColor, width_relToViewportHeight, text, style, stylePatternScaleFactor, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec));
                return;
            }

            Line_fadeableAnimSpeed_screenspace.InternalDrawColorFade(targetCamera, start, end, startColor, endColor, width_relToViewportHeight, text, style, stylePatternScaleFactor, animationSpeed, null, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

        public static void RayColorFade(Vector2 start, Vector2 direction, Color startColor, Color endColor, float width_relToViewportHeight = 0.0f, string text = null, bool interpretDirectionAsUnwarped = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.RayColorFade") == false) { return; }
            RayColorFade(automaticallyFoundCamera, start, direction, startColor, endColor, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }
        public static void RayColorFade(Camera targetCamera, Vector2 start, Vector2 direction, Color startColor, Color endColor, float width_relToViewportHeight = 0.0f, string text = null, bool interpretDirectionAsUnwarped = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceRayColorFade.Add(new ScreenspaceRayColorFade(targetCamera, start, direction, startColor, endColor, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec));
                return;
            }

            Ray_fadeableAnimSpeed_screenspace.InternalDrawColorFade(targetCamera, start, direction, startColor, endColor, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, null, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

        public static void LineFrom_withColorFade(Vector2 start, Vector2 direction, Color startColor, Color endColor, float width_relToViewportHeight = 0.0f, string text = null, bool interpretDirectionAsUnwarped = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.LineFrom_withColorFade") == false) { return; }
            LineFrom_withColorFade(automaticallyFoundCamera, start, direction, startColor, endColor, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

        public static void LineFrom_withColorFade(Camera targetCamera, Vector2 start, Vector2 direction, Color startColor, Color endColor, float width_relToViewportHeight = 0.0f, string text = null, bool interpretDirectionAsUnwarped = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceLineFrom_withColorFade.Add(new ScreenspaceLineFrom_withColorFade(targetCamera, start, direction, startColor, endColor, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec));
                return;
            }

            LineFrom_fadeableAnimSpeed_screenspace.InternalDraw_withColorFade(targetCamera, start, direction, startColor, endColor, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, null, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

        public static void LineTo_withColorFade(Vector2 direction, Vector2 end, Color startColor, Color endColor, float width_relToViewportHeight = 0.0f, string text = null, bool interpretDirectionAsUnwarped = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.LineTo_withColorFade") == false) { return; }
            LineTo_withColorFade(automaticallyFoundCamera, direction, end, startColor, endColor, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

        public static void LineTo_withColorFade(Camera targetCamera, Vector2 direction, Vector2 end, Color startColor, Color endColor, float width_relToViewportHeight = 0.0f, string text = null, bool interpretDirectionAsUnwarped = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceLineTo_withColorFade.Add(new ScreenspaceLineTo_withColorFade(targetCamera, direction, end, startColor, endColor, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec));
                return;
            }

            LineTo_fadeableAnimSpeed_screenspace.InternalDraw_withColorFade(targetCamera, direction, end, startColor, endColor, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, stylePatternScaleFactor, animationSpeed, null, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

        public static void LineCircled(Vector2 circleCenter, float startAngleDegCC_relativeToUp, float endAngleDegCC_relativeToUp, float radius_relToViewportHeight = 0.05f, Color color = default(Color), float width_relToViewportHeight = 0.0f, string text = null, bool skipFallbackDisplayOfZeroAngles = false, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.LineCircled") == false) { return; }

            LineCircled(automaticallyFoundCamera, circleCenter, startAngleDegCC_relativeToUp, endAngleDegCC_relativeToUp, radius_relToViewportHeight, color, width_relToViewportHeight, text, skipFallbackDisplayOfZeroAngles, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec);
        }
        public static void LineCircled(Camera targetCamera, Vector2 circleCenter, float startAngleDegCC_relativeToUp, float endAngleDegCC_relativeToUp, float radius_relToViewportHeight = 0.05f, Color color = default(Color), float width_relToViewportHeight = 0.0f, string text = null, bool skipFallbackDisplayOfZeroAngles = false, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(startAngleDegCC_relativeToUp, "startAngleDegCC_relativeToUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(endAngleDegCC_relativeToUp, "endAngleDegCC_relativeToUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius_relToViewportHeight, "radius_relToViewportHeight")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceLineCircled_angleToAngle_cam.Add(new ScreenspaceLineCircled_angleToAngle_cam(targetCamera, circleCenter, startAngleDegCC_relativeToUp, endAngleDegCC_relativeToUp, radius_relToViewportHeight, color, width_relToViewportHeight, text, skipFallbackDisplayOfZeroAngles, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec));
                return;
            }

            Quaternion rotation_fromGlobalUp_toLineStartAngleInXYplane = Quaternion.AngleAxis(startAngleDegCC_relativeToUp, Vector3.forward);
            Vector3 circleCenter_to_startPos_inUnwarpedSpace_normalized = rotation_fromGlobalUp_toLineStartAngleInXYplane * Vector3.up;
            Vector2 circleCenter_to_startPos_inWarpedSpace_normalized = DirectionInUnitsOfUnwarpedSpace_to_sameLookingDirectionInUnitsOfWarpedSpace(circleCenter_to_startPos_inUnwarpedSpace_normalized, targetCamera);
            Vector2 circleCenter_to_startPos_inWarpedScreenspaceSpace = circleCenter_to_startPos_inWarpedSpace_normalized * radius_relToViewportHeight;
            Vector2 startPos = circleCenter + circleCenter_to_startPos_inWarpedScreenspaceSpace;
            float turnAngleDegCC = endAngleDegCC_relativeToUp - startAngleDegCC_relativeToUp;
            LineCircled(targetCamera, startPos, circleCenter, turnAngleDegCC, color, width_relToViewportHeight, text, skipFallbackDisplayOfZeroAngles, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec);
        }

        public static void LineCircled(Vector2 startPos, Vector2 circleCenter, float turnAngleDegCC, Color color = default(Color), float width_relToViewportHeight = 0.0f, string text = null, bool skipFallbackDisplayOfZeroAngles = false, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.LineCircled") == false) { return; }
            LineCircled(automaticallyFoundCamera, startPos, circleCenter, turnAngleDegCC, color, width_relToViewportHeight, text, skipFallbackDisplayOfZeroAngles, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec);
        }
        public static void LineCircled(Camera targetCamera, Vector2 startPos, Vector2 circleCenter, float turnAngleDegCC, Color color = default(Color), float width_relToViewportHeight = 0.0f, string text = null, bool skipFallbackDisplayOfZeroAngles = false, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceLineCircled_angleFromStartPos_cam.Add(new ScreenspaceLineCircled_angleFromStartPos_cam(targetCamera, startPos, circleCenter, turnAngleDegCC, color, width_relToViewportHeight, text, skipFallbackDisplayOfZeroAngles, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec));
                return;
            }

            UtilitiesDXXL_LineCircled.LineCircledScreenspace(targetCamera, startPos, circleCenter, turnAngleDegCC, color, width_relToViewportHeight, text, skipFallbackDisplayOfZeroAngles, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, true);
        }

        public static void CircleSegment(Vector2 circleCenter, float startAngleDegCC_relativeToUp, float endAngleDegCC_relativeToUp, float radius_relToViewportHeight = 0.05f, Color color = default(Color), string text = null, float radiusPortionWhereDrawFillStarts = 0.0f, bool skipFallbackDisplayOfZeroAngles = false, float fillDensity = 1.0f, float minAngleDeg_withoutTextLineBreak = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.CircleSegment") == false) { return; }
            CircleSegment(automaticallyFoundCamera, circleCenter, startAngleDegCC_relativeToUp, endAngleDegCC_relativeToUp, radius_relToViewportHeight, color, text, radiusPortionWhereDrawFillStarts, skipFallbackDisplayOfZeroAngles, fillDensity, minAngleDeg_withoutTextLineBreak, durationInSec);
        }
        public static void CircleSegment(Camera targetCamera, Vector2 circleCenter, float startAngleDegCC_relativeToUp, float endAngleDegCC_relativeToUp, float radius_relToViewportHeight = 0.05f, Color color = default(Color), string text = null, float radiusPortionWhereDrawFillStarts = 0.0f, bool skipFallbackDisplayOfZeroAngles = false, float fillDensity = 1.0f, float minAngleDeg_withoutTextLineBreak = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(startAngleDegCC_relativeToUp, "startAngleDegCC_relativeToUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(endAngleDegCC_relativeToUp, "endAngleDegCC_relativeToUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius_relToViewportHeight, "radius_relToViewportHeight")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceCircleSegment_angleToAngle_cam.Add(new ScreenspaceCircleSegment_angleToAngle_cam(targetCamera, circleCenter, startAngleDegCC_relativeToUp, endAngleDegCC_relativeToUp, radius_relToViewportHeight, color, text, radiusPortionWhereDrawFillStarts, skipFallbackDisplayOfZeroAngles, fillDensity, minAngleDeg_withoutTextLineBreak, durationInSec));
                return;
            }

            Quaternion rotation_fromGlobalUp_toLineStartAngleInXYplane = Quaternion.AngleAxis(startAngleDegCC_relativeToUp, Vector3.forward);
            Vector3 circleCenter_to_startPos_inUnwarpedSpace_normalized = rotation_fromGlobalUp_toLineStartAngleInXYplane * Vector3.up;
            Vector2 circleCenter_to_startPos_inWarpedSpace_normalized = DirectionInUnitsOfUnwarpedSpace_to_sameLookingDirectionInUnitsOfWarpedSpace(circleCenter_to_startPos_inUnwarpedSpace_normalized, targetCamera);
            Vector2 circleCenter_to_startPosOnPerimeter_inWarpedScreenspaceSpace = circleCenter_to_startPos_inWarpedSpace_normalized * radius_relToViewportHeight;
            Vector2 startPosOnPerimeter = circleCenter + circleCenter_to_startPosOnPerimeter_inWarpedScreenspaceSpace;
            float turnAngleDegCC = endAngleDegCC_relativeToUp - startAngleDegCC_relativeToUp;
            CircleSegment(targetCamera, startPosOnPerimeter, circleCenter, turnAngleDegCC, color, text, radiusPortionWhereDrawFillStarts, skipFallbackDisplayOfZeroAngles, fillDensity, minAngleDeg_withoutTextLineBreak, durationInSec);
        }

        public static void CircleSegment(Vector2 startPosOnPerimeter, Vector2 circleCenter, float turnAngleDegCC, Color color = default(Color), string text = null, float radiusPortionWhereDrawFillStarts = 0.0f, bool skipFallbackDisplayOfZeroAngles = false, float fillDensity = 1.0f, float minAngleDeg_withoutTextLineBreak = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.CircleSegment") == false) { return; }
            CircleSegment(automaticallyFoundCamera, startPosOnPerimeter, circleCenter, turnAngleDegCC, color, text, radiusPortionWhereDrawFillStarts, skipFallbackDisplayOfZeroAngles, fillDensity, minAngleDeg_withoutTextLineBreak, durationInSec);
        }
        public static void CircleSegment(Camera targetCamera, Vector2 startPosOnPerimeter, Vector2 circleCenter, float turnAngleDegCC, Color color = default(Color), string text = null, float radiusPortionWhereDrawFillStarts = 0.0f, bool skipFallbackDisplayOfZeroAngles = false, float fillDensity = 1.0f, float minAngleDeg_withoutTextLineBreak = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceCircleSegment_angleFromStartPos_cam.Add(new ScreenspaceCircleSegment_angleFromStartPos_cam(targetCamera, startPosOnPerimeter, circleCenter, turnAngleDegCC, color, text, radiusPortionWhereDrawFillStarts, skipFallbackDisplayOfZeroAngles, fillDensity, minAngleDeg_withoutTextLineBreak, durationInSec));
                return;
            }

            UtilitiesDXXL_LineCircled.CircleSegmentScreenspace(targetCamera, startPosOnPerimeter, circleCenter, turnAngleDegCC, color, text, radiusPortionWhereDrawFillStarts, skipFallbackDisplayOfZeroAngles, fillDensity, minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL.LowerLeftOfWholeTextBlock, durationInSec, true);
        }

        public static void LineString(Vector2[] points, Color color = default(Color), bool closeGapBetweenLastAndFirstPoint = false, float width_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.LineString") == false) { return; }
            LineString(automaticallyFoundCamera, points, color, closeGapBetweenLastAndFirstPoint, width_relToViewportHeight, text, drawPointerIfOffscreen, style, stylePatternScaleFactor, durationInSec);
        }

        public static void LineString(Camera targetCamera, Vector2[] points, Color color = default(Color), bool closeGapBetweenLastAndFirstPoint = false, float width_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceLineString_array_cam.Add(new ScreenspaceLineString_array_cam(targetCamera, points, color, closeGapBetweenLastAndFirstPoint, width_relToViewportHeight, text, drawPointerIfOffscreen, style, stylePatternScaleFactor, durationInSec));
                return;
            }

            if (points.Length == 0)
            {
                Debug.Log("'points' has 0 items -> no drawing");
                return;
            }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width_relToViewportHeight, "width_relToViewportHeight")) { return; }
            width_relToViewportHeight = UtilitiesDXXL_Math.AbsNonZeroValue(width_relToViewportHeight);

            for (int i = 0; i < (points.Length - 1); i++)
            {
                Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, points[i], points[i + 1], color, width_relToViewportHeight, null, style, stylePatternScaleFactor, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
            }

            if (closeGapBetweenLastAndFirstPoint)
            {
                Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, points[points.Length - 1], points[0], color, width_relToViewportHeight, null, style, stylePatternScaleFactor, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
            }

            if (drawPointerIfOffscreen || (text != null && text != ""))
            {
                UtilitiesDXXL_List.CopyContentOfVector2ArrayToList(ref UtilitiesDXXL_Screenspace.vertices_inViewportSpace0to1, ref points, points.Length);
                Color invertedColor = UtilitiesDXXL_Colors.Invert_andAlphaTo1(color);
                UtilitiesDXXL_Screenspace.TagPointCollection(targetCamera, text, null, points.Length, 0.3f * width_relToViewportHeight, invertedColor, invertedColor, drawPointerIfOffscreen, false, durationInSec, 1.0f, true);
            }
        }

        public static void LineString(List<Vector2> points, Color color = default(Color), bool closeGapBetweenLastAndFirstPoint = false, float width_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.LineString") == false) { return; }
            LineString(automaticallyFoundCamera, points, color, closeGapBetweenLastAndFirstPoint, width_relToViewportHeight, text, drawPointerIfOffscreen, style, stylePatternScaleFactor, durationInSec);
        }
        public static void LineString(Camera targetCamera, List<Vector2> points, Color color = default(Color), bool closeGapBetweenLastAndFirstPoint = false, float width_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceLineString_list_cam.Add(new ScreenspaceLineString_list_cam(targetCamera, points, color, closeGapBetweenLastAndFirstPoint, width_relToViewportHeight, text, drawPointerIfOffscreen, style, stylePatternScaleFactor, durationInSec));
                return;
            }

            if (points.Count == 0)
            {
                Debug.Log("'points' has 0 items -> no drawing");
                return;
            }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width_relToViewportHeight, "width_relToViewportHeight")) { return; }
            width_relToViewportHeight = UtilitiesDXXL_Math.AbsNonZeroValue(width_relToViewportHeight);

            for (int i = 0; i < (points.Count - 1); i++)
            {
                Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, points[i], points[i + 1], color, width_relToViewportHeight, null, style, stylePatternScaleFactor, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
            }

            if (closeGapBetweenLastAndFirstPoint)
            {
                Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, points[points.Count - 1], points[0], color, width_relToViewportHeight, null, style, stylePatternScaleFactor, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
            }

            if (drawPointerIfOffscreen || (text != null && text != ""))
            {
                UtilitiesDXXL_List.CopyContentOfVector2Lists(ref UtilitiesDXXL_Screenspace.vertices_inViewportSpace0to1, ref points, points.Count);
                Color invertedColor = UtilitiesDXXL_Colors.Invert_andAlphaTo1(color);
                UtilitiesDXXL_Screenspace.TagPointCollection(targetCamera, text, null, points.Count, 0.3f * width_relToViewportHeight, invertedColor, invertedColor, drawPointerIfOffscreen, false, durationInSec, 1.0f, true);
            }
        }

        public static void LineStringColorFade(Vector2[] points, Color startColor, Color endColor, bool closeGapBetweenLastAndFirstPoint = false, float width_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.LineStringColorFade") == false) { return; }
            LineStringColorFade(automaticallyFoundCamera, points, startColor, endColor, closeGapBetweenLastAndFirstPoint, width_relToViewportHeight, text, drawPointerIfOffscreen, style, stylePatternScaleFactor, durationInSec);
        }
        public static void LineStringColorFade(Camera targetCamera, Vector2[] points, Color startColor, Color endColor, bool closeGapBetweenLastAndFirstPoint = false, float width_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceLineStringColorFade_array_cam.Add(new ScreenspaceLineStringColorFade_array_cam(targetCamera, points, startColor, endColor, closeGapBetweenLastAndFirstPoint, width_relToViewportHeight, text, drawPointerIfOffscreen, style, stylePatternScaleFactor, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width_relToViewportHeight, "width_relToViewportHeight")) { return; }
            width_relToViewportHeight = UtilitiesDXXL_Math.AbsNonZeroValue(width_relToViewportHeight);

            if (points.Length == 0)
            {
                Debug.Log("'points' has 0 items -> no drawing");
                return;
            }

            int iOffset_forColorFade = -1;
            if (closeGapBetweenLastAndFirstPoint)
            {
                iOffset_forColorFade = 0;
            }

            for (int i = 0; i < (points.Length - 1); i++)
            {
                Color color = UtilitiesDXXL_DrawBasics.GetFadedColorFromSegments(startColor, endColor, i, points.Length + iOffset_forColorFade);
                Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, points[i], points[i + 1], color, width_relToViewportHeight, null, style, stylePatternScaleFactor, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
            }

            if (closeGapBetweenLastAndFirstPoint)
            {
                Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, points[points.Length - 1], points[0], endColor, width_relToViewportHeight, null, style, stylePatternScaleFactor, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
            }

            if (drawPointerIfOffscreen || (text != null && text != ""))
            {
                UtilitiesDXXL_List.CopyContentOfVector2ArrayToList(ref UtilitiesDXXL_Screenspace.vertices_inViewportSpace0to1, ref points, points.Length);
                Color invertedAverageColor = UtilitiesDXXL_Colors.Invert_andAlphaTo1(Color.Lerp(startColor, endColor, 0.5f));
                UtilitiesDXXL_Screenspace.TagPointCollection(targetCamera, text, null, points.Length, 0.3f * width_relToViewportHeight, invertedAverageColor, invertedAverageColor, drawPointerIfOffscreen, false, durationInSec, 1.0f, true);
            }

        }
        public static void LineStringColorFade(List<Vector2> points, Color startColor, Color endColor, bool closeGapBetweenLastAndFirstPoint = false, float width_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.LineStringColorFade") == false) { return; }
            LineStringColorFade(automaticallyFoundCamera, points, startColor, endColor, closeGapBetweenLastAndFirstPoint, width_relToViewportHeight, text, drawPointerIfOffscreen, style, stylePatternScaleFactor, durationInSec);
        }
        public static void LineStringColorFade(Camera targetCamera, List<Vector2> points, Color startColor, Color endColor, bool closeGapBetweenLastAndFirstPoint = false, float width_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceLineStringColorFade_list_cam.Add(new ScreenspaceLineStringColorFade_list_cam(targetCamera, points, startColor, endColor, closeGapBetweenLastAndFirstPoint, width_relToViewportHeight, text, drawPointerIfOffscreen, style, stylePatternScaleFactor, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width_relToViewportHeight, "width_relToViewportHeight")) { return; }
            width_relToViewportHeight = UtilitiesDXXL_Math.AbsNonZeroValue(width_relToViewportHeight);

            if (points.Count == 0)
            {
                Debug.Log("'points' has 0 items -> no drawing");
                return;
            }

            int iOffset_forColorFade = -1;
            if (closeGapBetweenLastAndFirstPoint)
            {
                iOffset_forColorFade = 0;
            }

            for (int i = 0; i < (points.Count - 1); i++)
            {
                Color color = UtilitiesDXXL_DrawBasics.GetFadedColorFromSegments(startColor, endColor, i, points.Count + iOffset_forColorFade);
                Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, points[i], points[i + 1], color, width_relToViewportHeight, null, style, stylePatternScaleFactor, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
            }

            if (closeGapBetweenLastAndFirstPoint)
            {
                Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, points[points.Count - 1], points[0], endColor, width_relToViewportHeight, null, style, stylePatternScaleFactor, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
            }

            if (drawPointerIfOffscreen || (text != null && text != ""))
            {
                UtilitiesDXXL_List.CopyContentOfVector2Lists(ref UtilitiesDXXL_Screenspace.vertices_inViewportSpace0to1, ref points, points.Count);
                Color invertedAverageColor = UtilitiesDXXL_Colors.Invert_andAlphaTo1(Color.Lerp(startColor, endColor, 0.5f));
                UtilitiesDXXL_Screenspace.TagPointCollection(targetCamera, text, null, points.Count, 0.3f * width_relToViewportHeight, invertedAverageColor, invertedAverageColor, drawPointerIfOffscreen, false, durationInSec, 1.0f, true);
            }
        }

        public static void Shape(Vector3 centerPosition_in3DWorldspace, DrawShapes.Shape2DType shape = DrawShapes.Shape2DType.square, Color color = default(Color), float width_relToViewportHeight = 0.1f, float height_relToViewportHeight = 0.1f, float zRotationDegCC = 0.0f, float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = true, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool addTextForOutsideDistance_toOffscreenPointer = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceShape_3Dpos.Add(new ScreenspaceShape_3Dpos(centerPosition_in3DWorldspace, shape, color, width_relToViewportHeight, height_relToViewportHeight, zRotationDegCC, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Shape") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, centerPosition_in3DWorldspace, false);
            Shape(position_in2DViewportSpace, shape, color, width_relToViewportHeight, height_relToViewportHeight, zRotationDegCC, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }
        public static void Shape(Camera targetCamera, Vector3 centerPosition_in3DWorldspace, DrawShapes.Shape2DType shape = DrawShapes.Shape2DType.square, Color color = default(Color), float width_relToViewportHeight = 0.1f, float height_relToViewportHeight = 0.1f, float zRotationDegCC = 0.0f, float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = true, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool addTextForOutsideDistance_toOffscreenPointer = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceShape_3Dpos_cam.Add(new ScreenspaceShape_3Dpos_cam(targetCamera, centerPosition_in3DWorldspace, shape, color, width_relToViewportHeight, height_relToViewportHeight, zRotationDegCC, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(targetCamera, centerPosition_in3DWorldspace, false);
            Shape(targetCamera, position_in2DViewportSpace, shape, color, width_relToViewportHeight, height_relToViewportHeight, zRotationDegCC, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }

        public static void Shape(Vector2 centerPosition_in2DViewportSpace, DrawShapes.Shape2DType shape = DrawShapes.Shape2DType.square, Color color = default(Color), float width_relToViewportHeight = 0.1f, float height_relToViewportHeight = 0.1f, float zRotationDegCC = 0.0f, float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = true, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool addTextForOutsideDistance_toOffscreenPointer = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Shape") == false) { return; }
            Shape(automaticallyFoundCamera, centerPosition_in2DViewportSpace, shape, color, width_relToViewportHeight, height_relToViewportHeight, zRotationDegCC, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }
        public static void Shape(Camera targetCamera, Vector2 centerPosition_in2DViewportSpace, DrawShapes.Shape2DType shape = DrawShapes.Shape2DType.square, Color color = default(Color), float width_relToViewportHeight = 0.1f, float height_relToViewportHeight = 0.1f, float zRotationDegCC = 0.0f, float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = true, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool addTextForOutsideDistance_toOffscreenPointer = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceShape_2Dpos_cam.Add(new ScreenspaceShape_2Dpos_cam(targetCamera, centerPosition_in2DViewportSpace, shape, color, width_relToViewportHeight, height_relToViewportHeight, zRotationDegCC, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            UtilitiesDXXL_Screenspace.DrawShape(targetCamera, centerPosition_in2DViewportSpace, shape, color, color, width_relToViewportHeight, height_relToViewportHeight, zRotationDegCC, linesWidth_relToViewportHeight, text, lineStyle, stylePatternScaleFactor, fillStyle, drawPointerIfOffscreen, addTextForOutsideDistance_toOffscreenPointer, durationInSec, 1.0f, null, true);
        }

        public static void Rectangle(Rect rect, Color color = default(Color), DrawShapes.Shape2DType shape = DrawShapes.Shape2DType.square, float linesWidth_relTScreenHeight = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Rectangle") == false) { return; }
            Rectangle(automaticallyFoundCamera, rect, color, shape, linesWidth_relTScreenHeight, text, lineStyle, stylePatternScaleFactor, fillStyle, durationInSec);
        }
        public static void Rectangle(Camera targetCamera, Rect rect, Color color = default(Color), DrawShapes.Shape2DType shape = DrawShapes.Shape2DType.square, float linesWidth_relToScreenHeight = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Rectangle(targetCamera, rect.position, rect.width, rect.height, color, shape, linesWidth_relToScreenHeight, text, lineStyle, stylePatternScaleFactor, fillStyle, durationInSec);
        }

        static InternalDXXL_Plane rectPlane = new InternalDXXL_Plane();
        public static void Rectangle(Vector2 lowLeftCorner, float width_relToScreenWidth, float height_relToScreenHeight, Color color = default(Color), DrawShapes.Shape2DType shape = DrawShapes.Shape2DType.square, float linesWidth_relToScreenHeight = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Rectangle") == false) { return; }
            Rectangle(automaticallyFoundCamera, lowLeftCorner, width_relToScreenWidth, height_relToScreenHeight, color, shape, linesWidth_relToScreenHeight, text, lineStyle, stylePatternScaleFactor, fillStyle, durationInSec);
        }
        public static void Rectangle(Camera targetCamera, Vector2 lowLeftCorner, float width_relToScreenWidth, float height_relToScreenHeight, Color color = default(Color), DrawShapes.Shape2DType shape = DrawShapes.Shape2DType.square, float linesWidth_relToScreenHeight = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth_relToScreenHeight, "linesWidth_relToScreenHeight")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceRectangle.Add(new ScreenspaceRectangle(targetCamera, lowLeftCorner, width_relToScreenWidth, height_relToScreenHeight, color, shape, linesWidth_relToScreenHeight, text, lineStyle, stylePatternScaleFactor, fillStyle, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width_relToScreenWidth, "width_relToScreenWidth")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(height_relToScreenHeight, "height_relToScreenHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth_relToScreenHeight, "linesWidth_relToScreenHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(stylePatternScaleFactor, "stylePatternScaleFactor")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(lowLeftCorner, "lowLeftCorner")) { return; }

            if (UtilitiesDXXL_Screenspace.HasDefaultViewPortRect(targetCamera) == false)
            {
                text = "[<color=#adadadFF><icon=logMessage></color> 'DrawScreenspace.Rectangle()' is not fit for non-default camera viewport rects<br>-> Fallback to 'Shape()']<br>" + text;
                Vector2 centerPosition = new Vector2(lowLeftCorner.x + 0.5f * width_relToScreenWidth, lowLeftCorner.y + 0.5f * height_relToScreenHeight);
                float width_relToScreenHeight = width_relToScreenWidth * targetCamera.aspect;
                Shape(targetCamera, centerPosition, shape, color, width_relToScreenHeight, height_relToScreenHeight, 0.0f, linesWidth_relToScreenHeight, text, false, lineStyle, stylePatternScaleFactor, fillStyle, false, durationInSec);
                return;
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(width_relToScreenWidth) && UtilitiesDXXL_Math.ApproximatelyZero(height_relToScreenHeight))
            {
                UtilitiesDXXL_Screenspace.PointFallback(targetCamera, lowLeftCorner, "[<color=#adadadFF><icon=logMessage></color> RectangleScreenspace with extent of 0]<br>" + text, color, linesWidth_relToScreenHeight, durationInSec);
                return;
            }

            Rect rect = new Rect(lowLeftCorner, new Vector2(width_relToScreenWidth, height_relToScreenHeight));
            lineStyle = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(lineStyle);
            float distanceToRectPlane = targetCamera.nearClipPlane + drawOffsetBehindCamsNearPlane;
            Vector3 screenSpaceCenter_insideRectPlane = targetCamera.transform.position + targetCamera.transform.forward * distanceToRectPlane;
            float heightOfScreenSpace_insideRectPlane;
            if (targetCamera.orthographic)
            {
                //orthographic cam:
                heightOfScreenSpace_insideRectPlane = 2.0f * targetCamera.orthographicSize;
                stylePatternScaleFactor = stylePatternScaleFactor * targetCamera.orthographicSize;
            }
            else
            {
                //perspective cam:
                float tanOfHalfFieldOfView = Mathf.Tan(0.5f * targetCamera.fieldOfView * Mathf.Deg2Rad);
                heightOfScreenSpace_insideRectPlane = 2.0f * distanceToRectPlane * tanOfHalfFieldOfView;
                stylePatternScaleFactor = stylePatternScaleFactor * heightOfScreenSpace_insideRectPlane;
            }
            float linesWidth_worldSpace = heightOfScreenSpace_insideRectPlane * linesWidth_relToScreenHeight;
            linesWidth_worldSpace = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth_worldSpace);
            float halfLinesWidth_worldSpace = 0.5f * linesWidth_worldSpace;
            float widthOfScreenspace_insideRectPlane = heightOfScreenSpace_insideRectPlane * targetCamera.aspect;
            Vector3 camLeftNormalized = Vector3.Cross(targetCamera.transform.forward, targetCamera.transform.up);
            Vector3 camRightNormalized = -camLeftNormalized;
            Vector3 screenSpaceLowLeft_insideRectPlane = screenSpaceCenter_insideRectPlane + camLeftNormalized * (0.5f * widthOfScreenspace_insideRectPlane) - targetCamera.transform.up * (0.5f * heightOfScreenSpace_insideRectPlane);
            Vector3 screenSpaceXspan0to1_insideRectPlane = camRightNormalized * widthOfScreenspace_insideRectPlane;
            Vector3 screenSpaceYspan0to1_insideRectPlane = targetCamera.transform.up * heightOfScreenSpace_insideRectPlane;
            Vector3 centerOfDrawnRect = screenSpaceLowLeft_insideRectPlane + screenSpaceXspan0to1_insideRectPlane * rect.center.x + screenSpaceYspan0to1_insideRectPlane * rect.center.y;
            float heightOfDrawnRect = heightOfScreenSpace_insideRectPlane * rect.height;
            float widthOfDrawnRect = widthOfScreenspace_insideRectPlane * rect.width;
            int usedSlotsIn_verticesGlobal;

            if (shape == DrawShapes.Shape2DType.square)
            {
                Vector3 bottomLeftCorner_ofDrawnRect_worldSpace = screenSpaceLowLeft_insideRectPlane + screenSpaceXspan0to1_insideRectPlane * rect.x + screenSpaceYspan0to1_insideRectPlane * rect.y;
                Vector3 topLeftCorner_ofDrawnRect_worldSpace = screenSpaceLowLeft_insideRectPlane + screenSpaceXspan0to1_insideRectPlane * rect.x + screenSpaceYspan0to1_insideRectPlane * (rect.y + rect.height);
                Vector3 bottomRightCorner_ofDrawnRect_worldSpace = screenSpaceLowLeft_insideRectPlane + screenSpaceXspan0to1_insideRectPlane * (rect.x + rect.width) + screenSpaceYspan0to1_insideRectPlane * rect.y;
                Vector3 topRightCorner_ofDrawnRect_worldSpace = screenSpaceLowLeft_insideRectPlane + screenSpaceXspan0to1_insideRectPlane * (rect.x + rect.width) + screenSpaceYspan0to1_insideRectPlane * (rect.y + rect.height);
                rectPlane.Recreate(centerOfDrawnRect, targetCamera.transform.forward);

                UtilitiesDXXL_DrawBasics.Line(bottomLeftCorner_ofDrawnRect_worldSpace - targetCamera.transform.up * halfLinesWidth_worldSpace, topLeftCorner_ofDrawnRect_worldSpace + targetCamera.transform.up * halfLinesWidth_worldSpace, color, linesWidth_worldSpace, null, lineStyle, stylePatternScaleFactor, 0.0f, null, rectPlane, true, 0.0f, 0.0f, durationInSec, false, false, false, null, false, 0.0f);
                UtilitiesDXXL_DrawBasics.Line(bottomRightCorner_ofDrawnRect_worldSpace - targetCamera.transform.up * halfLinesWidth_worldSpace, topRightCorner_ofDrawnRect_worldSpace + targetCamera.transform.up * halfLinesWidth_worldSpace, color, linesWidth_worldSpace, null, lineStyle, stylePatternScaleFactor, 0.0f, null, rectPlane, true, 0.0f, 0.0f, durationInSec, false, false, false, null, false, 0.0f);
                UtilitiesDXXL_DrawBasics.Line(bottomLeftCorner_ofDrawnRect_worldSpace + camLeftNormalized * halfLinesWidth_worldSpace, bottomRightCorner_ofDrawnRect_worldSpace + camRightNormalized * halfLinesWidth_worldSpace, color, linesWidth_worldSpace, null, lineStyle, stylePatternScaleFactor, 0.0f, null, rectPlane, true, 0.0f, 0.0f, durationInSec, false, false, false, null, false, 0.0f);
                UtilitiesDXXL_DrawBasics.Line(topLeftCorner_ofDrawnRect_worldSpace + camLeftNormalized * halfLinesWidth_worldSpace, topRightCorner_ofDrawnRect_worldSpace + camRightNormalized * halfLinesWidth_worldSpace, color, linesWidth_worldSpace, null, lineStyle, stylePatternScaleFactor, 0.0f, null, rectPlane, true, 0.0f, 0.0f, durationInSec, false, false, false, null, false, 0.0f);
                usedSlotsIn_verticesGlobal = 4;

                UtilitiesDXXL_List.AddToAVectorList(ref UtilitiesDXXL_Shapes.verticesGlobal, bottomLeftCorner_ofDrawnRect_worldSpace, 0);
                UtilitiesDXXL_List.AddToAVectorList(ref UtilitiesDXXL_Shapes.verticesGlobal, bottomRightCorner_ofDrawnRect_worldSpace, 1);
                UtilitiesDXXL_List.AddToAVectorList(ref UtilitiesDXXL_Shapes.verticesGlobal, topRightCorner_ofDrawnRect_worldSpace, 2);
                UtilitiesDXXL_List.AddToAVectorList(ref UtilitiesDXXL_Shapes.verticesGlobal, topLeftCorner_ofDrawnRect_worldSpace, 3);
            }
            else
            {
                usedSlotsIn_verticesGlobal = DrawShapes.FlatShape(centerOfDrawnRect, shape, widthOfDrawnRect, heightOfDrawnRect, color, targetCamera.transform.forward, targetCamera.transform.up, linesWidth_worldSpace, null, lineStyle, stylePatternScaleFactor, true, DrawBasics.LineStyle.invisible, false, durationInSec, false);
            }

            if (fillStyle != DrawBasics.LineStyle.invisible)
            {
                Color fillColor = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.3f);
                fillStyle = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(fillStyle);
                rectPlane.Recreate(centerOfDrawnRect, targetCamera.transform.forward);
                float distanceBetweenLines_screenSpace = 0.01f;
                float distanceBetweenLines_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, rect.center, true, distanceBetweenLines_screenSpace);
                UtilitiesDXXL_Shapes.DrawShapeFilling(shape, fillStyle, usedSlotsIn_verticesGlobal, distanceBetweenLines_worldSpace, fillColor, targetCamera.transform.up, stylePatternScaleFactor, rectPlane, durationInSec, false);
            }

            if (text != null && text != "")
            {
                float lineHeight = 0.02f * heightOfScreenSpace_insideRectPlane;
                lineHeight = Mathf.Min(lineHeight, 0.9f * heightOfDrawnRect);
                Vector3 topLeftCorner_ofDrawnRect = screenSpaceLowLeft_insideRectPlane + screenSpaceXspan0to1_insideRectPlane * rect.x + screenSpaceYspan0to1_insideRectPlane * (rect.y + rect.height);
                Vector3 textPosition = topLeftCorner_ofDrawnRect + (0.02f * widthOfDrawnRect + halfLinesWidth_worldSpace) * camRightNormalized - (1.7f * lineHeight + halfLinesWidth_worldSpace) * targetCamera.transform.up;
                UtilitiesDXXL_Text.Write(text, textPosition, color, lineHeight, camRightNormalized, targetCamera.transform.up, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.96f * widthOfDrawnRect - linesWidth_worldSpace, false, durationInSec, false, false, false, true);
            }
        }

        public static void Box(Rect rect, Color color = default(Color), float zRotationDegCC = 0.0f, DrawShapes.Shape2DType shape = DrawShapes.Shape2DType.square, float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Box") == false) { return; }
            Box(automaticallyFoundCamera, rect, color, zRotationDegCC, shape, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }
        public static void Box(Camera targetCamera, Rect rect, Color color = default(Color), float zRotationDegCC = 0.0f, DrawShapes.Shape2DType shape = DrawShapes.Shape2DType.square, float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceBox_rect_cam.Add(new ScreenspaceBox_rect_cam(targetCamera, rect, color, zRotationDegCC, shape, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            float width_relToViewportHeight = rect.size.x * targetCamera.aspect;
            Shape(targetCamera, rect.center, shape, color, width_relToViewportHeight, rect.size.y, zRotationDegCC, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }

        public static void Box(Vector3 centerPosition_in3DWorldspace, Vector2 size_relToViewportHeight, Color color = default(Color), float zRotationDegCC = 0.0f, DrawShapes.Shape2DType shape = DrawShapes.Shape2DType.square, float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool forceSizeInterpretationToWarpedViewportSpace = false, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceBox_3Dpos_vec.Add(new ScreenspaceBox_3Dpos_vec(centerPosition_in3DWorldspace, size_relToViewportHeight, color, zRotationDegCC, shape, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, forceSizeInterpretationToWarpedViewportSpace, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Box") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, centerPosition_in3DWorldspace, false);
            Box(position_in2DViewportSpace, size_relToViewportHeight, color, zRotationDegCC, shape, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, forceSizeInterpretationToWarpedViewportSpace, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }
        public static void Box(Camera targetCamera, Vector3 centerPosition_in3DWorldspace, Vector2 size_relToViewportHeight, Color color = default(Color), float zRotationDegCC = 0.0f, DrawShapes.Shape2DType shape = DrawShapes.Shape2DType.square, float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool forceSizeInterpretationToWarpedViewportSpace = false, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceBox_3Dpos_vec_cam.Add(new ScreenspaceBox_3Dpos_vec_cam(targetCamera, centerPosition_in3DWorldspace, size_relToViewportHeight, color, zRotationDegCC, shape, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, forceSizeInterpretationToWarpedViewportSpace, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(targetCamera, centerPosition_in3DWorldspace, false);
            Box(targetCamera, position_in2DViewportSpace, size_relToViewportHeight, color, zRotationDegCC, shape, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, forceSizeInterpretationToWarpedViewportSpace, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }

        public static void Box(Vector2 centerPosition_in2DViewportSpace, Vector2 size_relToViewportHeight, Color color = default(Color), float zRotationDegCC = 0.0f, DrawShapes.Shape2DType shape = DrawShapes.Shape2DType.square, float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool forceSizeInterpretationToWarpedViewportSpace = false, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Box") == false) { return; }
            Box(automaticallyFoundCamera, centerPosition_in2DViewportSpace, size_relToViewportHeight, color, zRotationDegCC, shape, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, forceSizeInterpretationToWarpedViewportSpace, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }
        public static void Box(Camera targetCamera, Vector2 centerPosition_in2DViewportSpace, Vector2 size_relToViewportHeight, Color color = default(Color), float zRotationDegCC = 0.0f, DrawShapes.Shape2DType shape = DrawShapes.Shape2DType.square, float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool forceSizeInterpretationToWarpedViewportSpace = false, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceBox_2Dpos_vec_cam.Add(new ScreenspaceBox_2Dpos_vec_cam(targetCamera, centerPosition_in2DViewportSpace, size_relToViewportHeight, color, zRotationDegCC, shape, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, forceSizeInterpretationToWarpedViewportSpace, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            if (forceSizeInterpretationToWarpedViewportSpace)
            {
                size_relToViewportHeight = DirectionInUnitsOfWarpedSpace_to_sameLookingDirectionInUnitsOfUnwarpedSpace(size_relToViewportHeight, targetCamera);
            }
            Shape(targetCamera, centerPosition_in2DViewportSpace, shape, color, size_relToViewportHeight.x, size_relToViewportHeight.y, zRotationDegCC, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }

        public static void Circle(Rect rect, Color color = default(Color), float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Circle") == false) { return; }
            Circle(automaticallyFoundCamera, rect, color, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }
        public static void Circle(Camera targetCamera, Rect rect, Color color = default(Color), float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceCircle_rect_cam.Add(new ScreenspaceCircle_rect_cam(targetCamera, rect, color, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            Box(targetCamera, rect, color, 0.0f, DrawShapes.Shape2DType.circle, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }

        public static void Circle(Vector3 centerPosition_in3DWorldspace, float radius_relToViewportHeight = 0.05f, Color color = default(Color), float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceCircle_3Dpos_vecRad.Add(new ScreenspaceCircle_3Dpos_vecRad(centerPosition_in3DWorldspace, radius_relToViewportHeight, color, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Circle") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, centerPosition_in3DWorldspace, false);
            Circle(position_in2DViewportSpace, radius_relToViewportHeight, color, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }
        public static void Circle(Camera targetCamera, Vector3 centerPosition_in3DWorldspace, float radius_relToViewportHeight = 0.05f, Color color = default(Color), float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceCircle_3Dpos_vecRad_cam.Add(new ScreenspaceCircle_3Dpos_vecRad_cam(targetCamera, centerPosition_in3DWorldspace, radius_relToViewportHeight, color, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(targetCamera, centerPosition_in3DWorldspace, false);
            Circle(targetCamera, position_in2DViewportSpace, radius_relToViewportHeight, color, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }

        public static void Circle(Vector2 centerPosition_in2DViewportSpace, float radius_relToViewportHeight = 0.05f, Color color = default(Color), float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Circle") == false) { return; }
            Circle(automaticallyFoundCamera, centerPosition_in2DViewportSpace, radius_relToViewportHeight, color, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }
        public static void Circle(Camera targetCamera, Vector2 centerPosition_in2DViewportSpace, float radius_relToViewportHeight = 0.05f, Color color = default(Color), float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius_relToViewportHeight, "radius_relToViewportHeight")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceCircle_2Dpos_vecRad_cam.Add(new ScreenspaceCircle_2Dpos_vecRad_cam(targetCamera, centerPosition_in2DViewportSpace, radius_relToViewportHeight, color, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            float diameter_relToViewportHeight = 2.0f * radius_relToViewportHeight;
            bool forceSizeInterpretationToWarpedViewportSpace = false;
            Box(targetCamera, centerPosition_in2DViewportSpace, new Vector2(diameter_relToViewportHeight, diameter_relToViewportHeight), color, 0.0f, DrawShapes.Shape2DType.circle, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, forceSizeInterpretationToWarpedViewportSpace, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }

        public static void Capsule(Vector3 posOfCircle1_in3DWorldspace, Vector3 posOfCircle2_in3DWorldspace, float radius_relToViewportHeight = 0.05f, Color color = default(Color), float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos.Add(new ScreenspaceCapsule_3Dpos_vecC1C2Pos(posOfCircle1_in3DWorldspace, posOfCircle2_in3DWorldspace, radius_relToViewportHeight, color, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Capsule") == false) { return; }
            Vector2 position1_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, posOfCircle1_in3DWorldspace, false);
            Vector2 position2_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, posOfCircle2_in3DWorldspace, false);
            Capsule(position1_in2DViewportSpace, position2_in2DViewportSpace, radius_relToViewportHeight, color, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }
        public static void Capsule(Camera targetCamera, Vector3 posOfCircle1_in3DWorldspace, Vector3 posOfCircle2_in3DWorldspace, float radius_relToViewportHeight = 0.05f, Color color = default(Color), float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam.Add(new ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam(targetCamera, posOfCircle1_in3DWorldspace, posOfCircle2_in3DWorldspace, radius_relToViewportHeight, color, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            Vector2 position1_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(targetCamera, posOfCircle1_in3DWorldspace, false);
            Vector2 position2_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(targetCamera, posOfCircle2_in3DWorldspace, false);
            Capsule(targetCamera, position1_in2DViewportSpace, position2_in2DViewportSpace, radius_relToViewportHeight, color, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }

        public static void Capsule(Vector2 posOfCircle1_in2DViewportSpace, Vector2 posOfCircle2_in2DViewportSpace, float radius_relToViewportHeight = 0.05f, Color color = default(Color), float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Capsule") == false) { return; }
            Capsule(automaticallyFoundCamera, posOfCircle1_in2DViewportSpace, posOfCircle2_in2DViewportSpace, radius_relToViewportHeight, color, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }
        public static void Capsule(Camera targetCamera, Vector2 posOfCircle1_in2DViewportSpace, Vector2 posOfCircle2_in2DViewportSpace, float radius_relToViewportHeight = 0.05f, Color color = default(Color), float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam.Add(new ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam(targetCamera, posOfCircle1_in2DViewportSpace, posOfCircle2_in2DViewportSpace, radius_relToViewportHeight, color, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            UtilitiesDXXL_Screenspace.Capsule(targetCamera, posOfCircle1_in2DViewportSpace, posOfCircle2_in2DViewportSpace, radius_relToViewportHeight, color, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }

        public static void Capsule(Rect rect, Color color = default(Color), CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Vertical, float zRotationDegCC = 0.0f, float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Capsule") == false) { return; }
            Capsule(automaticallyFoundCamera, rect, color, capsuleDirection, zRotationDegCC, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }
        public static void Capsule(Camera targetCamera, Rect rect, Color color = default(Color), CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Vertical, float zRotationDegCC = 0.0f, float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceCapsule_rect_cam.Add(new ScreenspaceCapsule_rect_cam(targetCamera, rect, color, capsuleDirection, zRotationDegCC, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            float width_relToViewportHeight = rect.size.x * targetCamera.aspect;
            Vector2 size_relToViewportHeight = new Vector2(width_relToViewportHeight, rect.size.y);
            bool forceSizeInterpretationToWarpedViewportSpace = false;
            Capsule(targetCamera, rect.center, size_relToViewportHeight, color, capsuleDirection, zRotationDegCC, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, forceSizeInterpretationToWarpedViewportSpace, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }

        public static void Capsule(Vector3 centerPosition_in3DWorldspace, Vector2 size_relToViewportHeight, Color color = default(Color), CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Vertical, float zRotationDegCC = 0.0f, float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool forceSizeInterpretationToWarpedViewportSpace = false, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize.Add(new ScreenspaceCapsule_3Dpos_vecPosSize(centerPosition_in3DWorldspace, size_relToViewportHeight, color, capsuleDirection, zRotationDegCC, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, forceSizeInterpretationToWarpedViewportSpace, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Capsule") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, centerPosition_in3DWorldspace, false);
            Capsule(position_in2DViewportSpace, size_relToViewportHeight, color, capsuleDirection, zRotationDegCC, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, forceSizeInterpretationToWarpedViewportSpace, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }
        public static void Capsule(Camera targetCamera, Vector3 centerPosition_in3DWorldspace, Vector2 size_relToViewportHeight, Color color = default(Color), CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Vertical, float zRotationDegCC = 0.0f, float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool forceSizeInterpretationToWarpedViewportSpace = false, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam.Add(new ScreenspaceCapsule_3Dpos_vecPosSize_cam(targetCamera, centerPosition_in3DWorldspace, size_relToViewportHeight, color, capsuleDirection, zRotationDegCC, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, forceSizeInterpretationToWarpedViewportSpace, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(targetCamera, centerPosition_in3DWorldspace, false);
            Capsule(targetCamera, position_in2DViewportSpace, size_relToViewportHeight, color, capsuleDirection, zRotationDegCC, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, forceSizeInterpretationToWarpedViewportSpace, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }

        public static void Capsule(Vector2 centerPosition_in2DViewportSpace, Vector2 size_relToViewportHeight, Color color = default(Color), CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Vertical, float zRotationDegCC = 0.0f, float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool forceSizeInterpretationToWarpedViewportSpace = false, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Capsule") == false) { return; }
            Capsule(automaticallyFoundCamera, centerPosition_in2DViewportSpace, size_relToViewportHeight, color, capsuleDirection, zRotationDegCC, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, forceSizeInterpretationToWarpedViewportSpace, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }
        public static void Capsule(Camera targetCamera, Vector2 centerPosition_in2DViewportSpace, Vector2 size_relToViewportHeight, Color color = default(Color), CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Vertical, float zRotationDegCC = 0.0f, float linesWidth_relToViewportHeight = 0.0f, string text = null, bool drawPointerIfOffscreen = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool forceSizeInterpretationToWarpedViewportSpace = false, bool addTextForOutsideDistance_toOffscreenPointer = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam.Add(new ScreenspaceCapsule_2Dpos_vecPosSize_cam(targetCamera, centerPosition_in2DViewportSpace, size_relToViewportHeight, color, capsuleDirection, zRotationDegCC, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, forceSizeInterpretationToWarpedViewportSpace, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            if (forceSizeInterpretationToWarpedViewportSpace)
            {
                size_relToViewportHeight = DirectionInUnitsOfWarpedSpace_to_sameLookingDirectionInUnitsOfUnwarpedSpace(size_relToViewportHeight, targetCamera);
            }
            UtilitiesDXXL_Screenspace.Capsule(targetCamera, centerPosition_in2DViewportSpace, size_relToViewportHeight, color, capsuleDirection, zRotationDegCC, linesWidth_relToViewportHeight, text, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, durationInSec, true);
        }

        public static void PointArray(Vector2[] points, Color color = default(Color), float sizeOfMarkingCross_relToViewportHeight = 0.1f, float markingCrossLinesWidth_relToViewportHeight = 0.0f, bool drawCoordsAsText = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.PointArray") == false) { return; }
            PointArray(automaticallyFoundCamera, points, color, sizeOfMarkingCross_relToViewportHeight, markingCrossLinesWidth_relToViewportHeight, drawCoordsAsText, durationInSec);
        }
        public static void PointArray(Camera targetCamera, Vector2[] points, Color color = default(Color), float sizeOfMarkingCross_relToViewportHeight = 0.1f, float markingCrossLinesWidth_relToViewportHeight = 0.0f, bool drawCoordsAsText = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspacePointArray.Add(new ScreenspacePointArray(targetCamera, points, color, sizeOfMarkingCross_relToViewportHeight, markingCrossLinesWidth_relToViewportHeight, drawCoordsAsText, durationInSec));
                return;
            }

            for (int i = 0; i < points.Length; i++)
            {
                Point(targetCamera, points[i], null, color, sizeOfMarkingCross_relToViewportHeight, markingCrossLinesWidth_relToViewportHeight, 0.0f, false, false, drawCoordsAsText, false, durationInSec);
            }
        }
        public static void PointList(List<Vector2> points, Color color = default(Color), float sizeOfMarkingCross_relToViewportHeight = 0.1f, float markingCrossLinesWidth_relToViewportHeight = 0.0f, bool drawCoordsAsText = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.PointList") == false) { return; }
            PointList(automaticallyFoundCamera, points, color, sizeOfMarkingCross_relToViewportHeight, markingCrossLinesWidth_relToViewportHeight, drawCoordsAsText, durationInSec);
        }
        public static void PointList(Camera targetCamera, List<Vector2> points, Color color = default(Color), float sizeOfMarkingCross_relToViewportHeight = 0.1f, float markingCrossLinesWidth_relToViewportHeight = 0.0f, bool drawCoordsAsText = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspacePointList.Add(new ScreenspacePointList(targetCamera, points, color, sizeOfMarkingCross_relToViewportHeight, markingCrossLinesWidth_relToViewportHeight, drawCoordsAsText, durationInSec));
                return;
            }

            for (int i = 0; i < points.Count; i++)
            {
                Point(targetCamera, points[i], null, color, sizeOfMarkingCross_relToViewportHeight, markingCrossLinesWidth_relToViewportHeight, 0.0f, false, false, drawCoordsAsText, false, durationInSec);
            }
        }

        public static void Point(Vector2 position, Color color, float sizeOfMarkingCross_relToViewportHeight = 0.1f, float zRotationDegCC = 0.0f, float markingCrossLinesWidth_relToViewportHeight = 0.0f, bool drawPointerIfOffscreen = false, string text = null, bool pointer_as_textAttachStyle = true, bool drawCoordsAsText = true, bool addTextForOutsideDistance_toOffscreenPointer = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspacePoint.Add(new ScreenspacePoint(position, color, sizeOfMarkingCross_relToViewportHeight, zRotationDegCC, markingCrossLinesWidth_relToViewportHeight, drawPointerIfOffscreen, text, pointer_as_textAttachStyle, drawCoordsAsText, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Point") == false) { return; }
            Point(automaticallyFoundCamera, position, color, sizeOfMarkingCross_relToViewportHeight, zRotationDegCC, markingCrossLinesWidth_relToViewportHeight, drawPointerIfOffscreen, text, pointer_as_textAttachStyle, drawCoordsAsText, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }
        public static void Point(Camera targetCamera, Vector2 position, Color color, float sizeOfMarkingCross_relToViewportHeight = 0.1f, float zRotationDegCC = 0.0f, float markingCrossLinesWidth_relToViewportHeight = 0.0f, bool drawPointerIfOffscreen = false, string text = null, bool pointer_as_textAttachStyle = true, bool drawCoordsAsText = true, bool addTextForOutsideDistance_toOffscreenPointer = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Point(targetCamera, position, text, color, sizeOfMarkingCross_relToViewportHeight, markingCrossLinesWidth_relToViewportHeight, zRotationDegCC, drawPointerIfOffscreen, pointer_as_textAttachStyle, drawCoordsAsText, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }

        public static void Point(Vector2 position, string text = null, Color color = default(Color), float sizeOfMarkingCross_relToViewportHeight = 0.1f, float markingCrossLinesWidth_relToViewportHeight = 0.0f, float zRotationDegCC = 0.0f, bool drawPointerIfOffscreen = false, bool pointer_as_textAttachStyle = true, bool drawCoordsAsText = true, bool addTextForOutsideDistance_toOffscreenPointer = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Point") == false) { return; }
            Point(automaticallyFoundCamera, position, text, color, sizeOfMarkingCross_relToViewportHeight, markingCrossLinesWidth_relToViewportHeight, zRotationDegCC, drawPointerIfOffscreen, pointer_as_textAttachStyle, drawCoordsAsText, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
        }
        public static void Point(Camera targetCamera, Vector2 position, string text = null, Color color = default(Color), float sizeOfMarkingCross_relToViewportHeight = 0.1f, float markingCrossLinesWidth_relToViewportHeight = 0.0f, float zRotationDegCC = 0.0f, bool drawPointerIfOffscreen = false, bool pointer_as_textAttachStyle = true, bool drawCoordsAsText = true, bool addTextForOutsideDistance_toOffscreenPointer = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(sizeOfMarkingCross_relToViewportHeight, "sizeOfMarkingCross_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(markingCrossLinesWidth_relToViewportHeight, "markingCrossLinesWidth_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(zRotationDegCC, "zRotationDegCC")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspacePoint_prioText_cam.Add(new ScreenspacePoint_prioText_cam(targetCamera, position, text, color, sizeOfMarkingCross_relToViewportHeight, markingCrossLinesWidth_relToViewportHeight, zRotationDegCC, drawPointerIfOffscreen, pointer_as_textAttachStyle, drawCoordsAsText, addTextForOutsideDistance_toOffscreenPointer, durationInSec));
                return;
            }

            bool isOffscreen = !InternalDXXL_BoundsCamViewportSpace.IsInsideViewportInclBorder(position);
            color = UtilitiesDXXL_Colors.OverwriteDefaultColor(color);
            markingCrossLinesWidth_relToViewportHeight = UtilitiesDXXL_Math.AbsNonZeroValue(markingCrossLinesWidth_relToViewportHeight);
            sizeOfMarkingCross_relToViewportHeight = Mathf.Abs(sizeOfMarkingCross_relToViewportHeight);
            sizeOfMarkingCross_relToViewportHeight = UtilitiesDXXL_Math.Max(sizeOfMarkingCross_relToViewportHeight, 4.0f * markingCrossLinesWidth_relToViewportHeight, 0.016f);

            if (isOffscreen == false)
            {
                bool isZeroRotation = UtilitiesDXXL_Math.ApproximatelyZero(zRotationDegCC);

                float halfAbsMarkingCrossLinesWidth_relToViewportHeight = 0.5f * markingCrossLinesWidth_relToViewportHeight;
                float halfAbsMarkingCrossLinesWidth_relToViewportWidth = halfAbsMarkingCrossLinesWidth_relToViewportHeight / targetCamera.aspect;

                float halfMarkingCrossSize_relToViewportHeight = 0.5f * sizeOfMarkingCross_relToViewportHeight;
                float halfMarkingCrossSize_relToViewportWidth = halfMarkingCrossSize_relToViewportHeight / targetCamera.aspect;

                Vector2 point_toUnrotatedLeftMiddle = new Vector2(-halfMarkingCrossSize_relToViewportWidth, 0.0f);
                Vector2 point_toUnrotatedRightMiddle = new Vector2(halfMarkingCrossSize_relToViewportWidth, 0.0f);
                Vector2 point_toUnrotatedLowMiddle = new Vector2(0.0f, -halfMarkingCrossSize_relToViewportHeight);
                Vector2 point_toUnrotatedHighMiddle = new Vector2(0.0f, halfMarkingCrossSize_relToViewportHeight);

                Vector2 leftMiddlePos_viewportSpace = position + point_toUnrotatedLeftMiddle;
                Vector2 rightMiddlePos_viewportSpace = position + point_toUnrotatedRightMiddle;
                Vector2 lowMiddlePos_viewportSpace = position + point_toUnrotatedLowMiddle;
                Vector2 highMiddlePos_viewportSpace = position + point_toUnrotatedHighMiddle;

                Color colorOf_nonRotLinesAndCoordsText = color;

                if (isZeroRotation)
                {
                    Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, leftMiddlePos_viewportSpace, rightMiddlePos_viewportSpace, color, markingCrossLinesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
                    Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, lowMiddlePos_viewportSpace, highMiddlePos_viewportSpace, color, markingCrossLinesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
                }
                else
                {
                    if (drawCoordsAsText)
                    {
                        colorOf_nonRotLinesAndCoordsText = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, (markingCrossLinesWidth_relToViewportHeight > 0.0f) ? 0.6f : 0.4f);
                        Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, leftMiddlePos_viewportSpace, rightMiddlePos_viewportSpace, colorOf_nonRotLinesAndCoordsText, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
                        Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, lowMiddlePos_viewportSpace, highMiddlePos_viewportSpace, colorOf_nonRotLinesAndCoordsText, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
                    }

                    point_toUnrotatedLeftMiddle = new Vector2(-halfMarkingCrossSize_relToViewportHeight, 0.0f);
                    point_toUnrotatedRightMiddle = new Vector2(halfMarkingCrossSize_relToViewportHeight, 0.0f);
                    point_toUnrotatedLowMiddle = new Vector2(0.0f, -halfMarkingCrossSize_relToViewportHeight);
                    point_toUnrotatedHighMiddle = new Vector2(0.0f, halfMarkingCrossSize_relToViewportHeight);

                    Quaternion rotation = Quaternion.AngleAxis(zRotationDegCC, Vector3.forward);

                    Vector2 point_toRotatedLeftMiddle = rotation * point_toUnrotatedLeftMiddle;
                    point_toRotatedLeftMiddle = new Vector2(point_toRotatedLeftMiddle.x / targetCamera.aspect, point_toRotatedLeftMiddle.y);
                    Vector2 point_toRotatedRightMiddle = rotation * point_toUnrotatedRightMiddle;
                    point_toRotatedRightMiddle = new Vector2(point_toRotatedRightMiddle.x / targetCamera.aspect, point_toRotatedRightMiddle.y);
                    Vector2 point_toRotatedLowMiddle = rotation * point_toUnrotatedLowMiddle;
                    point_toRotatedLowMiddle = new Vector2(point_toRotatedLowMiddle.x / targetCamera.aspect, point_toRotatedLowMiddle.y);
                    Vector2 point_toRotatedHighMiddle = rotation * point_toUnrotatedHighMiddle;
                    point_toRotatedHighMiddle = new Vector2(point_toRotatedHighMiddle.x / targetCamera.aspect, point_toRotatedHighMiddle.y);

                    leftMiddlePos_viewportSpace = position + point_toRotatedLeftMiddle;
                    rightMiddlePos_viewportSpace = position + point_toRotatedRightMiddle;
                    lowMiddlePos_viewportSpace = position + point_toRotatedLowMiddle;
                    highMiddlePos_viewportSpace = position + point_toRotatedHighMiddle;

                    Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, leftMiddlePos_viewportSpace, rightMiddlePos_viewportSpace, color, markingCrossLinesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
                    Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, lowMiddlePos_viewportSpace, highMiddlePos_viewportSpace, color, markingCrossLinesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
                }

                DrawCoordsAsText(drawCoordsAsText, targetCamera, position, colorOf_nonRotLinesAndCoordsText, halfMarkingCrossSize_relToViewportHeight, halfMarkingCrossSize_relToViewportWidth, halfAbsMarkingCrossLinesWidth_relToViewportWidth, halfAbsMarkingCrossLinesWidth_relToViewportHeight, minTextSize_relToViewportHeight, durationInSec);
            }

            bool forcePointerDueToIsOffscreen = (isOffscreen && drawPointerIfOffscreen);
            if (forcePointerDueToIsOffscreen || (text != null && text != ""))
            {
                if (pointer_as_textAttachStyle || isOffscreen)
                {
                    float widthOfLinesTowardsText = 0.3f * markingCrossLinesWidth_relToViewportHeight;
                    PointTag(targetCamera, position, text, null, color, drawPointerIfOffscreen, widthOfLinesTowardsText, sizeOfMarkingCross_relToViewportHeight, default, 1.6f, false, addTextForOutsideDistance_toOffscreenPointer, durationInSec);
                }
                else
                {
                    float textSize_relToViewportHeight = 0.25f * sizeOfMarkingCross_relToViewportHeight;
                    textSize_relToViewportHeight = Mathf.Max(textSize_relToViewportHeight, minTextSize_relToViewportHeight);
                    UtilitiesDXXL_Text.WriteScreenspace(targetCamera, "<size=2> </size>" + text, position, color, textSize_relToViewportHeight, zRotationDegCC, DrawText.TextAnchorDXXL.UpperLeft, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, true, 0.0f, false, durationInSec, false);
                }
            }

            return;
        }

        static void DrawCoordsAsText(bool drawCoordsAsText, Camera camera, Vector2 position, Color colorOf_nonRotLinesAndCoordsText, float halfMarkingCrossSize_relToViewportHeight, float halfMarkingCrossSize_relToViewportWidth, float halfAbsMarkingCrossLinesWidth_relToViewportWidth, float halfAbsMarkingCrossLinesWidth_relToViewportHeight, float minTextSizeInScreenSpace, float durationInSec)
        {
            if (drawCoordsAsText)
            {
                Color colorOf_xySigns = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorOf_nonRotLinesAndCoordsText, 0.35f);
                float coordsTextSize = Mathf.Max(0.1f * halfMarkingCrossSize_relToViewportHeight, minTextSizeInScreenSpace);

                float nonForced_yTextWidth = halfMarkingCrossSize_relToViewportWidth - halfAbsMarkingCrossLinesWidth_relToViewportWidth;
                Vector2 posOf_yText = new Vector2(position.x + halfMarkingCrossSize_relToViewportWidth - nonForced_yTextWidth, position.y + halfAbsMarkingCrossLinesWidth_relToViewportHeight + 0.3f * coordsTextSize * camera.aspect);
                UtilitiesDXXL_Text.WriteScreenspace(camera, " " + position.y, posOf_yText, colorOf_nonRotLinesAndCoordsText, coordsTextSize, 0.0f, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, nonForced_yTextWidth, 0.0f, false, 0.0f, false, durationInSec, false);
                float sizeOfBiggestCharInFirstLine_inYValueText = DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine;
                Vector2 posOf_ySign = new Vector2(position.x - halfMarkingCrossSize_relToViewportWidth, posOf_yText.y);
                UtilitiesDXXL_Text.WriteScreenspace(camera, "y=", posOf_ySign, colorOf_xySigns, sizeOfBiggestCharInFirstLine_inYValueText, 0.0f, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, false, 0.0f, false, durationInSec, false);

                float nonForced_xTextWidth = halfMarkingCrossSize_relToViewportHeight - halfAbsMarkingCrossLinesWidth_relToViewportHeight;
                Vector2 posOf_xText = new Vector2(position.x - halfAbsMarkingCrossLinesWidth_relToViewportWidth - 0.3f * coordsTextSize, position.y + halfMarkingCrossSize_relToViewportHeight - nonForced_xTextWidth);
                UtilitiesDXXL_Text.WriteScreenspace(camera, " " + position.x, posOf_xText, colorOf_nonRotLinesAndCoordsText, coordsTextSize, 90.0f, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, nonForced_xTextWidth / camera.aspect, 0.0f, false, 0.0f, false, durationInSec, false);
                float sizeOfBiggestCharInFirstLine_inXValueText = DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine;
                Vector2 posOf_xSign = new Vector2(posOf_xText.x, position.y - halfMarkingCrossSize_relToViewportHeight);
                UtilitiesDXXL_Text.WriteScreenspace(camera, "x=", posOf_xSign, colorOf_xySigns, sizeOfBiggestCharInFirstLine_inXValueText, 90.0f, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, false, 0.0f, false, durationInSec, false);
            }
        }

        public static void PointTag(Vector3 position_in3DWorldspace, string text = null, string titleText = null, Color color = default(Color), bool drawPointerIfOffscreen = true, float linesWidth_relToViewportHeight = 0.0f, float size_asTextOffsetDistance_relToViewportHeight = 0.2f, Vector2 textOffsetDirection = default(Vector2), float textSizeScaleFactor = 1.0f, bool skipConeDrawing = false, bool addTextForOutsideDistance_toOffscreenPointer = true, float durationInSec = 0.0f, Vector2 customTowardsPoint_ofDefaultTextOffsetDirection = default(Vector2))
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspacePointTag_3Dpos.Add(new ScreenspacePointTag_3Dpos(position_in3DWorldspace, text, titleText, color, drawPointerIfOffscreen, linesWidth_relToViewportHeight, size_asTextOffsetDistance_relToViewportHeight, textOffsetDirection, textSizeScaleFactor, skipConeDrawing, addTextForOutsideDistance_toOffscreenPointer, durationInSec, customTowardsPoint_ofDefaultTextOffsetDirection));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.PointTag") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            PointTag(position_in2DViewportSpace, text, titleText, color, drawPointerIfOffscreen, linesWidth_relToViewportHeight, size_asTextOffsetDistance_relToViewportHeight, textOffsetDirection, textSizeScaleFactor, skipConeDrawing, addTextForOutsideDistance_toOffscreenPointer, durationInSec, customTowardsPoint_ofDefaultTextOffsetDirection);
        }
        public static void PointTag(Camera targetCamera, Vector3 position_in3DWorldspace, string text = null, string titleText = null, Color color = default(Color), bool drawPointerIfOffscreen = true, float linesWidth_relToViewportHeight = 0.0f, float size_asTextOffsetDistance_relToViewportHeight = 0.2f, Vector2 textOffsetDirection = default(Vector2), float textSizeScaleFactor = 1.0f, bool skipConeDrawing = false, bool addTextForOutsideDistance_toOffscreenPointer = true, float durationInSec = 0.0f, Vector2 customTowardsPoint_ofDefaultTextOffsetDirection = default(Vector2))
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspacePointTag_3Dpos_cam.Add(new ScreenspacePointTag_3Dpos_cam(targetCamera, position_in3DWorldspace, text, titleText, color, drawPointerIfOffscreen, linesWidth_relToViewportHeight, size_asTextOffsetDistance_relToViewportHeight, textOffsetDirection, textSizeScaleFactor, skipConeDrawing, addTextForOutsideDistance_toOffscreenPointer, durationInSec, customTowardsPoint_ofDefaultTextOffsetDirection));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(targetCamera, position_in3DWorldspace, false);
            PointTag(targetCamera, position_in2DViewportSpace, text, titleText, color, drawPointerIfOffscreen, linesWidth_relToViewportHeight, size_asTextOffsetDistance_relToViewportHeight, textOffsetDirection, textSizeScaleFactor, skipConeDrawing, addTextForOutsideDistance_toOffscreenPointer, durationInSec, customTowardsPoint_ofDefaultTextOffsetDirection);
        }

        public static void PointTag(Vector2 position_in2DViewportSpace, string text = null, string titleText = null, Color color = default(Color), bool drawPointerIfOffscreen = true, float linesWidth_relToViewportHeight = 0.0f, float size_asTextOffsetDistance_relToViewportHeight = 0.2f, Vector2 textOffsetDirection = default(Vector2), float textSizeScaleFactor = 1.0f, bool skipConeDrawing = false, bool addTextForOutsideDistance_toOffscreenPointer = true, float durationInSec = 0.0f, Vector2 customTowardsPoint_ofDefaultTextOffsetDirection = default(Vector2))
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.PointTag") == false) { return; }
            PointTag(automaticallyFoundCamera, position_in2DViewportSpace, text, titleText, color, drawPointerIfOffscreen, linesWidth_relToViewportHeight, size_asTextOffsetDistance_relToViewportHeight, textOffsetDirection, textSizeScaleFactor, skipConeDrawing, addTextForOutsideDistance_toOffscreenPointer, durationInSec, customTowardsPoint_ofDefaultTextOffsetDirection);
        }
        public static void PointTag(Camera targetCamera, Vector2 position_in2DViewportSpace, string text = null, string titleText = null, Color color = default(Color), bool drawPointerIfOffscreen = true, float linesWidth_relToViewportHeight = 0.0f, float size_asTextOffsetDistance_relToViewportHeight = 0.2f, Vector2 textOffsetDirection = default(Vector2), float textSizeScaleFactor = 1.0f, bool skipConeDrawing = false, bool addTextForOutsideDistance_toOffscreenPointer = true, float durationInSec = 0.0f, Vector2 customTowardsPoint_ofDefaultTextOffsetDirection = default(Vector2))
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspacePointTag_2Dpos_cam.Add(new ScreenspacePointTag_2Dpos_cam(targetCamera, position_in2DViewportSpace, text, titleText, color, drawPointerIfOffscreen, linesWidth_relToViewportHeight, size_asTextOffsetDistance_relToViewportHeight, textOffsetDirection, textSizeScaleFactor, skipConeDrawing, addTextForOutsideDistance_toOffscreenPointer, durationInSec, customTowardsPoint_ofDefaultTextOffsetDirection));
                return;
            }

            UtilitiesDXXL_Screenspace.PointTag(targetCamera, position_in2DViewportSpace, text, titleText, color, color, drawPointerIfOffscreen, linesWidth_relToViewportHeight, size_asTextOffsetDistance_relToViewportHeight, textOffsetDirection, textSizeScaleFactor, skipConeDrawing, addTextForOutsideDistance_toOffscreenPointer, durationInSec, customTowardsPoint_ofDefaultTextOffsetDirection);
        }

        public static void Vector(Vector2 vectorStartPos, Vector2 vectorEndPos, Color color = default(Color), float lineWidth_relToViewportHeight = 0.0f, string text = null, float coneLength_relToViewportHeight = 0.05f, bool pointerAtBothSides = false, bool writeComponentValuesAsText = false, float endPlatesSize_relToViewportHeight = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Vector") == false) { return; }
            Vector(automaticallyFoundCamera, vectorStartPos, vectorEndPos, color, lineWidth_relToViewportHeight, text, coneLength_relToViewportHeight, pointerAtBothSides, writeComponentValuesAsText, endPlatesSize_relToViewportHeight, durationInSec);
        }
        public static void Vector(Camera targetCamera, Vector2 vectorStartPos, Vector2 vectorEndPos, Color color = default(Color), float lineWidth_relToViewportHeight = 0.0f, string text = null, float coneLength_relToViewportHeight = 0.05f, bool pointerAtBothSides = false, bool writeComponentValuesAsText = false, float endPlatesSize_relToViewportHeight = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vectorStartPos, "vectorStartPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vectorEndPos, "vectorEndPos")) { return; }
            VectorFrom(targetCamera, vectorStartPos, vectorEndPos - vectorStartPos, color, lineWidth_relToViewportHeight, text, false, coneLength_relToViewportHeight, pointerAtBothSides, writeComponentValuesAsText, endPlatesSize_relToViewportHeight, durationInSec);
        }

        public static void VectorFrom(Vector2 vectorStartPos, Vector2 vector, Color color = default(Color), float lineWidth_relToViewportHeight = 0.0f, string text = null, bool interpretVectorAsUnwarped = false, float coneLength_relToViewportHeight = 0.05f, bool pointerAtBothSides = false, bool writeComponentValuesAsText = false, float endPlatesSize_relToViewportHeight = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.VectorFrom") == false) { return; }
            VectorFrom(automaticallyFoundCamera, vectorStartPos, vector, color, lineWidth_relToViewportHeight, text, interpretVectorAsUnwarped, coneLength_relToViewportHeight, pointerAtBothSides, writeComponentValuesAsText, endPlatesSize_relToViewportHeight, durationInSec);
        }
        public static void VectorFrom(Camera targetCamera, Vector2 vectorStartPos, Vector2 vector, Color color = default(Color), float lineWidth_relToViewportHeight = 0.0f, string text = null, bool interpretVectorAsUnwarped = false, float coneLength_relToViewportHeight = 0.05f, bool pointerAtBothSides = false, bool writeComponentValuesAsText = false, float endPlatesSize_relToViewportHeight = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lineWidth_relToViewportHeight, "lineWidth_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(coneLength_relToViewportHeight, "coneLength_relToViewportHeight")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vectorStartPos, "vectorStartPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vector, "vector")) { return; }

            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceVectorFrom.Add(new ScreenspaceVectorFrom(targetCamera, vectorStartPos, vector, color, lineWidth_relToViewportHeight, text, interpretVectorAsUnwarped, coneLength_relToViewportHeight, pointerAtBothSides, writeComponentValuesAsText, endPlatesSize_relToViewportHeight, durationInSec));
                return;
            }

            lineWidth_relToViewportHeight = UtilitiesDXXL_Math.AbsNonZeroValue(lineWidth_relToViewportHeight);

            if (UtilitiesDXXL_Math.ApproximatelyZero(vector))
            {
                UtilitiesDXXL_Screenspace.PointFallback(targetCamera, vectorStartPos, "[<color=#adadadFF><icon=logMessage></color> VectorScreenspace with length of 0]<br>" + text, color, lineWidth_relToViewportHeight, durationInSec);
                return;
            }

            Vector2 vector_inNonSquareViewportSpace = interpretVectorAsUnwarped ? DirectionInUnitsOfUnwarpedSpace_to_sameLookingDirectionInUnitsOfWarpedSpace(vector, targetCamera) : vector;
            Vector2 vectorEndPos_inNonSquareViewportSpace = vectorStartPos + vector_inNonSquareViewportSpace;
            Vector2 middleOfVector_inNonSquareViewportSpace = 0.5f * (vectorStartPos + vectorEndPos_inNonSquareViewportSpace);

            bool isThinLine;
            float lineWidth_worldSpace = 0.0f;
            if (UtilitiesDXXL_Math.ApproximatelyZero(lineWidth_relToViewportHeight))
            {
                isThinLine = true;
            }
            else
            {
                lineWidth_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, middleOfVector_inNonSquareViewportSpace, true, lineWidth_relToViewportHeight);
                isThinLine = false;
            }

            Vector3 vectorStartPos_worldSpace = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(targetCamera, vectorStartPos, false);
            Vector3 vectorEndPos_worldSpace = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(targetCamera, vectorEndPos_inNonSquareViewportSpace, false);
            Vector3 vector_worldSpace = vectorEndPos_worldSpace - vectorStartPos_worldSpace;
            Vector3 vectorNormalized_worldSpace = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(vector_worldSpace, out float vectorLength_worldSpace);
            coneLength_relToViewportHeight = Mathf.Max(coneLength_relToViewportHeight, 0.0f);
            float coneLength_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, middleOfVector_inNonSquareViewportSpace, true, coneLength_relToViewportHeight);
            coneLength_worldSpace = Mathf.Clamp(coneLength_worldSpace, 0.01f * vectorLength_worldSpace, 0.45f * vectorLength_worldSpace);

            float coneAngleDeg = 25.0f;
            if (isThinLine == false)
            {
                float coneSize_to_lineWidth_scaler = 1.3f;
                float minConeAngleDeg = 2.0f * Mathf.Rad2Deg * Mathf.Atan(coneSize_to_lineWidth_scaler * lineWidth_worldSpace / coneLength_worldSpace);
                coneAngleDeg = Mathf.Max(coneAngleDeg, minConeAngleDeg);
            }

            float lengthTillConeStart_worldSpace = vectorLength_worldSpace - coneLength_worldSpace;
            Vector3 endConeBaseCenter_worldSpace = vectorStartPos_worldSpace + vectorNormalized_worldSpace * lengthTillConeStart_worldSpace;
            Vector3 startConeBaseCenter_worldSpace = vectorStartPos_worldSpace;
            if (pointerAtBothSides)
            {
                startConeBaseCenter_worldSpace = vectorStartPos_worldSpace + vectorNormalized_worldSpace * coneLength_worldSpace;
            }

            Vector3 startPos_ofNonConedLineSegment_worldSpace;
            Vector3 endPos_ofNonConedLineSegment_worldSpace;
            if (vectorStartPos.x < vectorEndPos_inNonSquareViewportSpace.x)
            {
                startPos_ofNonConedLineSegment_worldSpace = startConeBaseCenter_worldSpace;
                endPos_ofNonConedLineSegment_worldSpace = endConeBaseCenter_worldSpace;
            }
            else
            {
                startPos_ofNonConedLineSegment_worldSpace = endConeBaseCenter_worldSpace;
                endPos_ofNonConedLineSegment_worldSpace = startConeBaseCenter_worldSpace;
            }

            UtilitiesDXXL_Screenspace.camPlane.Recreate(targetCamera.transform.position, targetCamera.transform.forward);
            if (writeComponentValuesAsText)
            {
                text = "<size=6>( " + vector.x + " , " + vector.y + " )</size><br>" + text;
            }
            float minTextSize_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, middleOfVector_inNonSquareViewportSpace, true, minTextSize_relToViewportHeight);
            UtilitiesDXXL_DrawBasics.Line(startPos_ofNonConedLineSegment_worldSpace, endPos_ofNonConedLineSegment_worldSpace, color, lineWidth_worldSpace, text, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, UtilitiesDXXL_Screenspace.camPlane, true, 0.0f, minTextSize_worldSpace, durationInSec, false, false, false, targetCamera, false, 0.0f);

            float shorteningOf_straightLineInsideCone_worldSpace = 0.0f;
            if (isThinLine == false)
            {
                shorteningOf_straightLineInsideCone_worldSpace = (0.5f * lineWidth_worldSpace) / Mathf.Tan(Mathf.Deg2Rad * 0.5f * coneAngleDeg);
                shorteningOf_straightLineInsideCone_worldSpace = Mathf.Min(shorteningOf_straightLineInsideCone_worldSpace, 0.99f * coneLength_worldSpace);
            }

            Vector3 lineEndInsideEndCone_worldSpace = vectorEndPos_worldSpace - vectorNormalized_worldSpace * shorteningOf_straightLineInsideCone_worldSpace;
            UtilitiesDXXL_DrawBasics.Line(endConeBaseCenter_worldSpace, lineEndInsideEndCone_worldSpace, color, lineWidth_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, UtilitiesDXXL_Screenspace.camPlane, true, 0.0f, 0.0f, durationInSec, false, false, false, targetCamera, false, 0.0f);
            if (pointerAtBothSides)
            {
                Vector3 lineEndInsideStartCone_worldSpace = vectorStartPos_worldSpace + vectorNormalized_worldSpace * shorteningOf_straightLineInsideCone_worldSpace;
                UtilitiesDXXL_DrawBasics.Line(startConeBaseCenter_worldSpace, lineEndInsideStartCone_worldSpace, color, lineWidth_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, UtilitiesDXXL_Screenspace.camPlane, true, 0.0f, 0.0f, durationInSec, false, false, false, targetCamera, false, 0.0f);
            }

            Vector3 upVector_ofConeBaseRect_worldSpace = targetCamera.transform.forward;
            DrawShapes.ConeFilled(vectorEndPos_worldSpace, coneLength_worldSpace, -vector_worldSpace, upVector_ofConeBaseRect_worldSpace, 0.0f, coneAngleDeg, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, false);
            if (pointerAtBothSides)
            {
                DrawShapes.ConeFilled(vectorStartPos_worldSpace, coneLength_worldSpace, vector_worldSpace, upVector_ofConeBaseRect_worldSpace, 0.0f, coneAngleDeg, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, false);
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(endPlatesSize_relToViewportHeight) == false)
            {
                DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.invisible;
                Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, vectorStartPos, vectorEndPos_inNonSquareViewportSpace, color, 0.0f, null, lineStyle, 1.0f, 0.0f, null, endPlatesSize_relToViewportHeight, 0.0f, 0.0f, durationInSec);
            }
        }

        public static void VectorTo(Vector2 vector, Vector2 vectorEndPos, Color color = default(Color), float lineWidth_relToViewportHeight = 0.0f, string text = null, bool interpretVectorAsUnwarped = false, float coneLength_relToViewportHeight = 0.05f, bool pointerAtBothSides = false, bool writeComponentValuesAsText = false, float endPlatesSize_relToViewportHeight = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.VectorTo") == false) { return; }
            VectorTo(automaticallyFoundCamera, vector, vectorEndPos, color, lineWidth_relToViewportHeight, text, interpretVectorAsUnwarped, coneLength_relToViewportHeight, pointerAtBothSides, writeComponentValuesAsText, endPlatesSize_relToViewportHeight, durationInSec);
        }
        public static void VectorTo(Camera targetCamera, Vector2 vector, Vector2 vectorEndPos, Color color = default(Color), float lineWidth_relToViewportHeight = 0.0f, string text = null, bool interpretVectorAsUnwarped = false, float coneLength_relToViewportHeight = 0.05f, bool pointerAtBothSides = false, bool writeComponentValuesAsText = false, float endPlatesSize_relToViewportHeight = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vectorEndPos, "vectorEndPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vector, "vector")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceVectorTo.Add(new ScreenspaceVectorTo(targetCamera, vector, vectorEndPos, color, lineWidth_relToViewportHeight, text, interpretVectorAsUnwarped, coneLength_relToViewportHeight, pointerAtBothSides, writeComponentValuesAsText, endPlatesSize_relToViewportHeight, durationInSec));
                return;
            }

            Vector2 vectorEndPos_inNonSquareViewportSpace = vectorEndPos;
            Vector2 vectorStartPos_inNonSquareViewportSpace;
            if (interpretVectorAsUnwarped)
            {
                vectorStartPos_inNonSquareViewportSpace = vectorEndPos_inNonSquareViewportSpace - DirectionInUnitsOfUnwarpedSpace_to_sameLookingDirectionInUnitsOfWarpedSpace(vector, targetCamera);
            }
            else
            {
                vectorStartPos_inNonSquareViewportSpace = vectorEndPos_inNonSquareViewportSpace - vector;
            }
            VectorFrom(targetCamera, vectorStartPos_inNonSquareViewportSpace, vector, color, lineWidth_relToViewportHeight, text, interpretVectorAsUnwarped, coneLength_relToViewportHeight, pointerAtBothSides, writeComponentValuesAsText, endPlatesSize_relToViewportHeight, durationInSec);
        }

        public static void VectorCircled(Vector2 circleCenter, float startAngleDegCC_relativeToUp, float endAngleDegCC_relativeToUp, float radius_relToViewportHeight = 0.05f, Color color = default(Color), float lineWidth_relToViewportHeight = 0.0f, string text = null, float coneLength_relToViewportHeight = 0.05f, bool skipFallbackDisplayOfZeroAngles = false, bool pointerAtBothSides = false, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.VectorCircled") == false) { return; }
            VectorCircled(automaticallyFoundCamera, circleCenter, startAngleDegCC_relativeToUp, endAngleDegCC_relativeToUp, radius_relToViewportHeight, color, lineWidth_relToViewportHeight, text, coneLength_relToViewportHeight, skipFallbackDisplayOfZeroAngles, pointerAtBothSides, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec);
        }
        public static void VectorCircled(Camera targetCamera, Vector2 circleCenter, float startAngleDegCC_relativeToUp, float endAngleDegCC_relativeToUp, float radius_relToViewportHeight = 0.05f, Color color = default(Color), float lineWidth_relToViewportHeight = 0.0f, string text = null, float coneLength_relToViewportHeight = 0.05f, bool skipFallbackDisplayOfZeroAngles = false, bool pointerAtBothSides = false, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(startAngleDegCC_relativeToUp, "startAngleDegCC_relativeToUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(endAngleDegCC_relativeToUp, "endAngleDegCC_relativeToUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius_relToViewportHeight, "radius_relToViewportHeight")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceVectorCircled_angleToAngle_cam.Add(new ScreenspaceVectorCircled_angleToAngle_cam(targetCamera, circleCenter, startAngleDegCC_relativeToUp, endAngleDegCC_relativeToUp, radius_relToViewportHeight, color, lineWidth_relToViewportHeight, text, coneLength_relToViewportHeight, skipFallbackDisplayOfZeroAngles, pointerAtBothSides, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec));
                return;
            }

            Quaternion rotation_fromGlobalUp_toLineStartAngleInXYplane = Quaternion.AngleAxis(startAngleDegCC_relativeToUp, Vector3.forward);
            Vector3 circleCenter_to_startPos_inUnwarpedSpace_normalized = rotation_fromGlobalUp_toLineStartAngleInXYplane * Vector3.up;
            Vector2 circleCenter_to_startPos_inWarpedSpace_normalized = DirectionInUnitsOfUnwarpedSpace_to_sameLookingDirectionInUnitsOfWarpedSpace(circleCenter_to_startPos_inUnwarpedSpace_normalized, targetCamera);
            Vector2 circleCenter_to_startPos_inWarpedScreenspaceSpace = circleCenter_to_startPos_inWarpedSpace_normalized * radius_relToViewportHeight;
            Vector2 startPos = circleCenter + circleCenter_to_startPos_inWarpedScreenspaceSpace;
            float turnAngleDegCC = endAngleDegCC_relativeToUp - startAngleDegCC_relativeToUp;
            VectorCircled(targetCamera, startPos, circleCenter, turnAngleDegCC, color, lineWidth_relToViewportHeight, text, coneLength_relToViewportHeight, skipFallbackDisplayOfZeroAngles, pointerAtBothSides, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec);
        }

        public static void VectorCircled(Vector2 startPos, Vector2 circleCenter, float turnAngleDegCC, Color color = default(Color), float lineWidth_relToViewportHeight = 0.0f, string text = null, float coneLength_relToViewportHeight = 0.05f, bool skipFallbackDisplayOfZeroAngles = false, bool pointerAtBothSides = false, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.VectorCircled") == false) { return; }
            VectorCircled(automaticallyFoundCamera, startPos, circleCenter, turnAngleDegCC, color, lineWidth_relToViewportHeight, text, coneLength_relToViewportHeight, skipFallbackDisplayOfZeroAngles, pointerAtBothSides, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec);
        }
        public static void VectorCircled(Camera targetCamera, Vector2 startPos, Vector2 circleCenter, float turnAngleDegCC, Color color = default(Color), float lineWidth_relToViewportHeight = 0.0f, string text = null, float coneLength_relToViewportHeight = 0.05f, bool skipFallbackDisplayOfZeroAngles = false, bool pointerAtBothSides = false, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceVectorCircled_angleFromStartPos_cam.Add(new ScreenspaceVectorCircled_angleFromStartPos_cam(targetCamera, startPos, circleCenter, turnAngleDegCC, color, lineWidth_relToViewportHeight, text, coneLength_relToViewportHeight, skipFallbackDisplayOfZeroAngles, pointerAtBothSides, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec));
                return;
            }
            UtilitiesDXXL_LineCircled.VectorCircledScreenspace(targetCamera, startPos, circleCenter, turnAngleDegCC, color, lineWidth_relToViewportHeight, text, coneLength_relToViewportHeight, skipFallbackDisplayOfZeroAngles, pointerAtBothSides, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec);
        }

        public static void Icon(Vector3 position_in3DWorldspace, DrawBasics.IconType icon, Color color = default(Color), float size_relToViewportHeight = 0.1f, string text = null, float zRotationDegCC = 0.0f, float strokeWidth_relToViewportHeight = 0.0f, bool displayPointerIfOffscreen = false, bool mirrorHorizontally = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceIcon_3Dpos.Add(new ScreenspaceIcon_3Dpos(position_in3DWorldspace, icon, color, size_relToViewportHeight, text, zRotationDegCC, strokeWidth_relToViewportHeight, displayPointerIfOffscreen, mirrorHorizontally, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Icon") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            Icon(position_in2DViewportSpace, icon, color, size_relToViewportHeight, text, zRotationDegCC, strokeWidth_relToViewportHeight, displayPointerIfOffscreen, mirrorHorizontally, durationInSec);
        }
        public static void Icon(Camera targetCamera, Vector3 position_in3DWorldspace, DrawBasics.IconType icon, Color color = default(Color), float size_relToViewportHeight = 0.1f, string text = null, float zRotationDegCC = 0.0f, float strokeWidth_relToViewportHeight = 0.0f, bool displayPointerIfOffscreen = false, bool mirrorHorizontally = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceIcon_3Dpos_cam.Add(new ScreenspaceIcon_3Dpos_cam(targetCamera, position_in3DWorldspace, icon, color, size_relToViewportHeight, text, zRotationDegCC, strokeWidth_relToViewportHeight, displayPointerIfOffscreen, mirrorHorizontally, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(targetCamera, position_in3DWorldspace, false);
            Icon(targetCamera, position_in2DViewportSpace, icon, color, size_relToViewportHeight, text, zRotationDegCC, strokeWidth_relToViewportHeight, displayPointerIfOffscreen, mirrorHorizontally, durationInSec);
        }

        public static void Icon(Vector2 position_in2DViewportSpace, DrawBasics.IconType icon, Color color = default(Color), float size_relToViewportHeight = 0.1f, string text = null, float zRotationDegCC = 0.0f, float strokeWidth_relToViewportHeight = 0.0f, bool displayPointerIfOffscreen = false, bool mirrorHorizontally = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Icon") == false) { return; }
            Icon(automaticallyFoundCamera, position_in2DViewportSpace, icon, color, size_relToViewportHeight, text, zRotationDegCC, strokeWidth_relToViewportHeight, displayPointerIfOffscreen, mirrorHorizontally, durationInSec);
        }
        public static void Icon(Camera targetCamera, Vector2 position_in2DViewportSpace, DrawBasics.IconType icon, Color color = default(Color), float size_relToViewportHeight = 0.1f, string text = null, float zRotationDegCC = 0.0f, float strokeWidth_relToViewportHeight = 0.0f, bool displayPointerIfOffscreen = false, bool mirrorHorizontally = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(size_relToViewportHeight, "size_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(zRotationDegCC, "zRotationDegCC")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position_in2DViewportSpace, "position")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceIcon_2Dpos_cam.Add(new ScreenspaceIcon_2Dpos_cam(targetCamera, position_in2DViewportSpace, icon, color, size_relToViewportHeight, text, zRotationDegCC, strokeWidth_relToViewportHeight, displayPointerIfOffscreen, mirrorHorizontally, durationInSec));
                return;
            }

            float distanceThresholdOutsideScreen_relToViewportHeight = 0.4f * size_relToViewportHeight;
            if (InternalDXXL_BoundsCamViewportSpace.IsOutsideViewportYWithPadding(position_in2DViewportSpace, distanceThresholdOutsideScreen_relToViewportHeight))
            {
                if (displayPointerIfOffscreen)
                {
                    Point(targetCamera, position_in2DViewportSpace, "[Icon(ScreenSpace) " + DrawText.MarkupIcon(icon) + " offscreen at " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(position_in2DViewportSpace) + "]<br>" + text, color, 0.1f, 0.0f, 0.0f, true, true, false, true, durationInSec);
                }
                return;
            }
            float distanceThresholdOutsideScreen_relToViewportWidth = distanceThresholdOutsideScreen_relToViewportHeight / targetCamera.aspect;
            if (InternalDXXL_BoundsCamViewportSpace.IsOutsideViewportXWithPadding(position_in2DViewportSpace, distanceThresholdOutsideScreen_relToViewportWidth))
            {
                if (displayPointerIfOffscreen)
                {
                    Point(targetCamera, position_in2DViewportSpace, "[Icon(ScreenSpace) " + DrawText.MarkupIcon(icon) + " offscreen at " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(position_in2DViewportSpace) + "]<br>" + text, color, 0.1f, 0.0f, 0.0f, true, true, false, true, durationInSec);
                }
                return;
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(size_relToViewportHeight))
            {
                UtilitiesDXXL_Screenspace.PointFallback(targetCamera, position_in2DViewportSpace, "[Icon(ScreenSpace) " + DrawText.MarkupIcon(icon) + " with size of 0]<br>" + text, color, 0.0f, durationInSec);
                return;
            }

            Vector3 position_worldSpace = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(targetCamera, position_in2DViewportSpace, false);
            float size_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, position_in2DViewportSpace, true, size_relToViewportHeight);
            Quaternion rotation_worldSpace = UtilitiesDXXL_Math.ApproximatelyZero(zRotationDegCC) ? targetCamera.transform.rotation : (Quaternion.AngleAxis(zRotationDegCC, targetCamera.transform.forward) * targetCamera.transform.rotation);
            float minTextSize_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, position_in2DViewportSpace, true, minTextSize_relToViewportHeight);

            int strokeWidth_asPPMofSize = 0;
            if ((UtilitiesDXXL_Math.ApproximatelyZero(strokeWidth_relToViewportHeight) == false) && (UtilitiesDXXL_Math.ApproximatelyZero(size_relToViewportHeight) == false))
            {
                float strokeWidth_relToIconSize = strokeWidth_relToViewportHeight / size_relToViewportHeight;
                strokeWidth_asPPMofSize = (int)(1000000.0f * strokeWidth_relToIconSize);
            }

            bool autoFlipMirroredText_toFitObserverCam = false;
            UtilitiesDXXL_DrawBasics.Icon(position_worldSpace, icon, color, size_worldSpace, text, rotation_worldSpace, strokeWidth_asPPMofSize, mirrorHorizontally, durationInSec, false, 0.15f, minTextSize_worldSpace, autoFlipMirroredText_toFitObserverCam);
        }

        public static void Dot(Vector3 position_in3DWorldspace, float radius_relToViewportHeight = 0.05f, Color color = default(Color), string text = null, float density = 1.0f, bool displayPointerIfOffscreen = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceDot_3Dpos.Add(new ScreenspaceDot_3Dpos(position_in3DWorldspace, radius_relToViewportHeight, color, text, density, displayPointerIfOffscreen, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Dot") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            Dot(position_in2DViewportSpace, radius_relToViewportHeight, color, text, density, displayPointerIfOffscreen, durationInSec);
        }

        public static void Dot(Camera targetCamera, Vector3 position_in3DWorldspace, float radius_relToViewportHeight = 0.05f, Color color = default(Color), string text = null, float density = 1.0f, bool displayPointerIfOffscreen = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceDot_3Dpos_cam.Add(new ScreenspaceDot_3Dpos_cam(targetCamera, position_in3DWorldspace, radius_relToViewportHeight, color, text, density, displayPointerIfOffscreen, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(targetCamera, position_in3DWorldspace, false);
            Dot(targetCamera, position_in2DViewportSpace, radius_relToViewportHeight, color, text, density, displayPointerIfOffscreen, durationInSec);
        }

        public static void Dot(Vector2 position_in2DViewportSpace, float radius_relToViewportHeight = 0.05f, Color color = default(Color), string text = null, float density = 1.0f, bool displayPointerIfOffscreen = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.Dot") == false) { return; }
            Dot(automaticallyFoundCamera, position_in2DViewportSpace, radius_relToViewportHeight, color, text, density, displayPointerIfOffscreen, durationInSec);
        }

        public static void Dot(Camera targetCamera, Vector2 position_in2DViewportSpace, float radius_relToViewportHeight = 0.05f, Color color = default(Color), string text = null, float density = 1.0f, bool displayPointerIfOffscreen = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius_relToViewportHeight, "radius_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(density, "density")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position_in2DViewportSpace, "position")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceDot_2Dpos_cam.Add(new ScreenspaceDot_2Dpos_cam(targetCamera, position_in2DViewportSpace, radius_relToViewportHeight, color, text, density, displayPointerIfOffscreen, durationInSec));
                return;
            }

            float distanceThresholdOutsideScreen_relToViewportHeight = 0.975f * radius_relToViewportHeight;
            if (InternalDXXL_BoundsCamViewportSpace.IsOutsideViewportYWithPadding(position_in2DViewportSpace, distanceThresholdOutsideScreen_relToViewportHeight))
            {
                if (displayPointerIfOffscreen)
                {
                    Point(targetCamera, position_in2DViewportSpace, "[Dot(ScreenSpace) offscreen at " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(position_in2DViewportSpace) + "]<br>" + text, color, 0.1f, 0.0f, 0.0f, true, true, false, true, durationInSec);
                }
                return;
            }
            float distanceThresholdOutsideScreen_relToViewportWidth = distanceThresholdOutsideScreen_relToViewportHeight / targetCamera.aspect;
            if (InternalDXXL_BoundsCamViewportSpace.IsOutsideViewportXWithPadding(position_in2DViewportSpace, distanceThresholdOutsideScreen_relToViewportWidth))
            {
                if (displayPointerIfOffscreen)
                {
                    Point(targetCamera, position_in2DViewportSpace, "[Dot(ScreenSpace) offscreen at " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(position_in2DViewportSpace) + "]<br>" + text, color, 0.1f, 0.0f, 0.0f, true, true, false, true, durationInSec);
                }
                return;
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(radius_relToViewportHeight))
            {
                UtilitiesDXXL_Screenspace.PointFallback(targetCamera, position_in2DViewportSpace, "[Dot(ScreenSpace) with size of 0]<br>" + text, color, 0.0f, durationInSec);
                return;
            }

            Vector3 position_worldSpace = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(targetCamera, position_in2DViewportSpace, false);
            float radius_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, position_in2DViewportSpace, true, radius_relToViewportHeight);
            Quaternion rotation_worldSpace = targetCamera.transform.rotation;
            float minTextSize_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, position_in2DViewportSpace, true, minTextSize_relToViewportHeight);

            bool autoFlipMirroredText_toFitObserverCam = false;
            UtilitiesDXXL_DrawBasics.Dot(position_worldSpace, radius_worldSpace, rotation_worldSpace, color, text, density, durationInSec, false, 0.15f, minTextSize_worldSpace, autoFlipMirroredText_toFitObserverCam);
        }

        public static void MovingArrowsRay(Vector2 start, Vector2 direction, Color color = default(Color), float lineWidth_relToViewportHeight = 0.016f, float distanceBetweenArrows_relToViewportHeight = 0.11f, float lengthOfArrows_relToViewportHeight = 0.05f, string text = null, float animationSpeed = 0.5f, bool backwardAnimationFlipsArrowDirection = true, bool interpretDirectionAsUnwarped = false, float endPlatesSize_relToViewportHeight = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.MovingArrowsRay") == false) { return; }
            MovingArrowsRay(automaticallyFoundCamera, start, direction, color, lineWidth_relToViewportHeight, distanceBetweenArrows_relToViewportHeight, lengthOfArrows_relToViewportHeight, text, animationSpeed, backwardAnimationFlipsArrowDirection, interpretDirectionAsUnwarped, endPlatesSize_relToViewportHeight, durationInSec);
        }
        public static void MovingArrowsRay(Camera targetCamera, Vector2 start, Vector2 direction, Color color = default(Color), float lineWidth_relToViewportHeight = 0.016f, float distanceBetweenArrows_relToViewportHeight = 0.11f, float lengthOfArrows_relToViewportHeight = 0.05f, string text = null, float animationSpeed = 0.5f, bool backwardAnimationFlipsArrowDirection = true, bool interpretDirectionAsUnwarped = false, float endPlatesSize_relToViewportHeight = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceMovingArrowsRay.Add(new ScreenspaceMovingArrowsRay(targetCamera, start, direction, color, lineWidth_relToViewportHeight, distanceBetweenArrows_relToViewportHeight, lengthOfArrows_relToViewportHeight, text, animationSpeed, backwardAnimationFlipsArrowDirection, interpretDirectionAsUnwarped, endPlatesSize_relToViewportHeight, durationInSec));
                return;
            }

            MovingArrowsRay_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, start, direction, color, lineWidth_relToViewportHeight, distanceBetweenArrows_relToViewportHeight, lengthOfArrows_relToViewportHeight, text, animationSpeed, null, backwardAnimationFlipsArrowDirection, interpretDirectionAsUnwarped, endPlatesSize_relToViewportHeight, durationInSec);
        }

        public static void MovingArrowsLine(Vector2 start, Vector2 end, Color color = default(Color), float lineWidth_relToViewportHeight = 0.016f, float distanceBetweenArrows_relToViewportHeight = 0.11f, float lengthOfArrows_relToViewportHeight = 0.05f, string text = null, float animationSpeed = 0.5f, bool backwardAnimationFlipsArrowDirection = true, float endPlatesSize_relToViewportHeight = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.MovingArrowsLine") == false) { return; }
            MovingArrowsLine(automaticallyFoundCamera, start, end, color, lineWidth_relToViewportHeight, distanceBetweenArrows_relToViewportHeight, lengthOfArrows_relToViewportHeight, text, animationSpeed, backwardAnimationFlipsArrowDirection, endPlatesSize_relToViewportHeight, durationInSec);
        }

        public static void MovingArrowsLine(Camera targetCamera, Vector2 start, Vector2 end, Color color = default(Color), float lineWidth_relToViewportHeight = 0.016f, float distanceBetweenArrows_relToViewportHeight = 0.11f, float lengthOfArrows_relToViewportHeight = 0.05f, string text = null, float animationSpeed = 0.5f, bool backwardAnimationFlipsArrowDirection = true, float endPlatesSize_relToViewportHeight = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceMovingArrowsLine.Add(new ScreenspaceMovingArrowsLine(targetCamera, start, end, color, lineWidth_relToViewportHeight, distanceBetweenArrows_relToViewportHeight, lengthOfArrows_relToViewportHeight, text, animationSpeed, backwardAnimationFlipsArrowDirection, endPlatesSize_relToViewportHeight, durationInSec));
                return;
            }

            MovingArrowsLine_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, start, end, color, lineWidth_relToViewportHeight, distanceBetweenArrows_relToViewportHeight, lengthOfArrows_relToViewportHeight, text, animationSpeed, null, backwardAnimationFlipsArrowDirection, endPlatesSize_relToViewportHeight, durationInSec);
        }

        public static void RayWithAlternatingColors(Vector2 start, Vector2 direction, Color color1 = default(Color), Color color2 = default(Color), float lineWidth_relToViewportHeight = 0.0f, float lengthOfStripes_relToViewportHeight = 0.03f, string text = null, bool interpretDirectionAsUnwarped = false, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.RayWithAlternatingColors") == false) { return; }
            RayWithAlternatingColors(automaticallyFoundCamera, start, direction, color1, color2, lineWidth_relToViewportHeight, lengthOfStripes_relToViewportHeight, text, interpretDirectionAsUnwarped, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, durationInSec);
        }
        public static void RayWithAlternatingColors(Camera targetCamera, Vector2 start, Vector2 direction, Color color1 = default(Color), Color color2 = default(Color), float lineWidth_relToViewportHeight = 0.0f, float lengthOfStripes_relToViewportHeight = 0.03f, string text = null, bool interpretDirectionAsUnwarped = false, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceRayWithAlternatingColors.Add(new ScreenspaceRayWithAlternatingColors(targetCamera, start, direction, color1, color2, lineWidth_relToViewportHeight, lengthOfStripes_relToViewportHeight, text, interpretDirectionAsUnwarped, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, durationInSec));
                return;
            }

            RayWithAlternatingColors_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, start, direction, color1, color2, lineWidth_relToViewportHeight, lengthOfStripes_relToViewportHeight, text, interpretDirectionAsUnwarped, animationSpeed, null, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, durationInSec);
        }

        public static void LineWithAlternatingColors(Vector2 start, Vector2 end, Color color1 = default(Color), Color color2 = default(Color), float lineWidth_relToViewportHeight = 0.0f, float lengthOfStripes_relToViewportHeight = 0.03f, string text = null, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.LineWithAlternatingColors") == false) { return; }
            LineWithAlternatingColors(automaticallyFoundCamera, start, end, color1, color2, lineWidth_relToViewportHeight, lengthOfStripes_relToViewportHeight, text, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, durationInSec);
        }
        public static void LineWithAlternatingColors(Camera targetCamera, Vector2 start, Vector2 end, Color color1 = default(Color), Color color2 = default(Color), float lineWidth_relToViewportHeight = 0.0f, float lengthOfStripes_relToViewportHeight = 0.03f, string text = null, float animationSpeed = 0.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceLineWithAlternatingColors.Add(new ScreenspaceLineWithAlternatingColors(targetCamera, start, end, color1, color2, lineWidth_relToViewportHeight, lengthOfStripes_relToViewportHeight, text, animationSpeed, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, durationInSec));
                return;
            }
            LineWithAlternatingColors_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, start, end, color1, color2, lineWidth_relToViewportHeight, lengthOfStripes_relToViewportHeight, text, animationSpeed, null, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, durationInSec);
        }

        public static void BlinkingRay(Vector2 start, Vector2 direction, Color primaryColor = default(Color), float blinkDurationInSec = 0.5f, float width_relToViewportHeight = 0.0f, string text = null, bool interpretDirectionAsUnwarped = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, Color blinkColor = default(Color), float stylePatternScaleFactor = 1.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.BlinkingRay") == false) { return; }
            BlinkingRay(automaticallyFoundCamera, start, direction, primaryColor, blinkDurationInSec, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, blinkColor, stylePatternScaleFactor, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }
        public static void BlinkingRay(Camera targetCamera, Vector2 start, Vector2 direction, Color primaryColor = default(Color), float blinkDurationInSec = 0.5f, float width_relToViewportHeight = 0.0f, string text = null, bool interpretDirectionAsUnwarped = false, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, Color blinkColor = default(Color), float stylePatternScaleFactor = 1.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(direction, "direction")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceBlinkingRay.Add(new ScreenspaceBlinkingRay(targetCamera, start, direction, primaryColor, blinkDurationInSec, width_relToViewportHeight, text, interpretDirectionAsUnwarped, style, blinkColor, stylePatternScaleFactor, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec));
                return;
            }

            Vector2 direction_inNonSquareViewportSpace = interpretDirectionAsUnwarped ? DirectionInUnitsOfUnwarpedSpace_to_sameLookingDirectionInUnitsOfWarpedSpace(direction, targetCamera) : direction;
            Vector2 end = start + direction_inNonSquareViewportSpace;
            BlinkingLine(targetCamera, start, end, primaryColor, blinkDurationInSec, width_relToViewportHeight, text, style, blinkColor, stylePatternScaleFactor, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

        public static void BlinkingLine(Vector2 start, Vector2 end, Color primaryColor = default(Color), float blinkDurationInSec = 0.5f, float width_relToViewportHeight = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, Color blinkColor = default(Color), float stylePatternScaleFactor = 1.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.BlinkingLine") == false) { return; }
            BlinkingLine(automaticallyFoundCamera, start, end, primaryColor, blinkDurationInSec, width_relToViewportHeight, text, style, blinkColor, stylePatternScaleFactor, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }
        public static void BlinkingLine(Camera targetCamera, Vector2 start, Vector2 end, Color primaryColor = default(Color), float blinkDurationInSec = 0.5f, float width_relToViewportHeight = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, Color blinkColor = default(Color), float stylePatternScaleFactor = 1.0f, float endPlatesSize_relToViewportHeight = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(blinkDurationInSec, "blinkDurationInSec")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceBlinkingLine.Add(new ScreenspaceBlinkingLine(targetCamera, start, end, primaryColor, blinkDurationInSec, width_relToViewportHeight, text, style, blinkColor, stylePatternScaleFactor, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec));
                return;
            }

            blinkDurationInSec = Mathf.Max(blinkDurationInSec, UtilitiesDXXL_DrawBasics.min_blinkDurationInSec);
            float passedBlinkIntervallsSinceStartup = UtilitiesDXXL_LineStyles.GetTime() / blinkDurationInSec;
            primaryColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(primaryColor);
            if (UtilitiesDXXL_Math.CheckIf_givenNumberIs_evenNotOdd(Mathf.FloorToInt(passedBlinkIntervallsSinceStartup)))
            {
                Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, start, end, primaryColor, width_relToViewportHeight, text, style, stylePatternScaleFactor, 0.0f, null, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
            }
            else
            {
                if (UtilitiesDXXL_Colors.IsDefaultColor(blinkColor))
                {
                    Color alternatingBlinkColor = UtilitiesDXXL_Colors.Invert_andAlphaTo1(primaryColor);
                    alternatingBlinkColor = UtilitiesDXXL_Colors.OverwriteColorNearGreyWithBlack(alternatingBlinkColor);
                    Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, start, end, alternatingBlinkColor, width_relToViewportHeight, text, style, stylePatternScaleFactor, 0.0f, null, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
                }
                else
                {
                    Line_fadeableAnimSpeed_screenspace.InternalDraw(targetCamera, start, end, blinkColor, width_relToViewportHeight, text, style, stylePatternScaleFactor, 0.0f, null, endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinRelTextSize, durationInSec);
                }
            }
        }

        public static void RayUnderTension(Vector2 start, Vector2 direction, float relaxedLength_relToViewportHeight = 0.4f, Color relaxedColor = default(Color), DrawBasics.LineStyle style = DrawBasics.LineStyle.sine, float stretchFactor_forStretchedTensionColor = 2.0f, Color color_forStretchedTension = default(Color), float stretchFactor_forSqueezedTensionColor = 0.0f, Color color_forSqueezedTension = default(Color), float width_relToViewportHeight = 0.0f, string text = null, float alphaOfReferenceLengthDisplay = 0.1f, bool interpretDirectionAsUnwarped = false, float stylePatternScaleFactor = 1.0f, float endPlatesSize_relToViewportHeight = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.RayUnderTension") == false) { return; }
            RayUnderTension(automaticallyFoundCamera, start, direction, relaxedLength_relToViewportHeight, relaxedColor, style, stretchFactor_forStretchedTensionColor, color_forStretchedTension, stretchFactor_forSqueezedTensionColor, color_forSqueezedTension, width_relToViewportHeight, text, alphaOfReferenceLengthDisplay, interpretDirectionAsUnwarped, stylePatternScaleFactor, endPlatesSize_relToViewportHeight, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

        public static void RayUnderTension(Camera targetCamera, Vector2 start, Vector2 direction, float relaxedLength_relToViewportHeight = 0.4f, Color relaxedColor = default(Color), DrawBasics.LineStyle style = DrawBasics.LineStyle.sine, float stretchFactor_forStretchedTensionColor = 2.0f, Color color_forStretchedTension = default(Color), float stretchFactor_forSqueezedTensionColor = 0.0f, Color color_forSqueezedTension = default(Color), float width_relToViewportHeight = 0.0f, string text = null, float alphaOfReferenceLengthDisplay = 0.1f, bool interpretDirectionAsUnwarped = false, float stylePatternScaleFactor = 1.0f, float endPlatesSize_relToViewportHeight = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(direction, "direction")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceRayUnderTension.Add(new ScreenspaceRayUnderTension(targetCamera, start, direction, relaxedLength_relToViewportHeight, relaxedColor, style, stretchFactor_forStretchedTensionColor, color_forStretchedTension, stretchFactor_forSqueezedTensionColor, color_forSqueezedTension, width_relToViewportHeight, text, alphaOfReferenceLengthDisplay, interpretDirectionAsUnwarped, stylePatternScaleFactor, endPlatesSize_relToViewportHeight, enlargeSmallTextToThisMinRelTextSize, durationInSec));
                return;
            }

            Vector2 direction_inNonSquareViewportSpace = interpretDirectionAsUnwarped ? DirectionInUnitsOfUnwarpedSpace_to_sameLookingDirectionInUnitsOfWarpedSpace(direction, targetCamera) : direction;
            LineUnderTension(targetCamera, start, start + direction_inNonSquareViewportSpace, relaxedLength_relToViewportHeight, relaxedColor, style, stretchFactor_forStretchedTensionColor, color_forStretchedTension, stretchFactor_forSqueezedTensionColor, color_forSqueezedTension, width_relToViewportHeight, text, alphaOfReferenceLengthDisplay, stylePatternScaleFactor, endPlatesSize_relToViewportHeight, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

        public static void LineUnderTension(Vector2 start, Vector2 end, float relaxedLength_relToViewportHeight = 0.4f, Color relaxedColor = default(Color), DrawBasics.LineStyle style = DrawBasics.LineStyle.sine, float stretchFactor_forStretchedTensionColor = 2.0f, Color color_forStretchedTension = default(Color), float stretchFactor_forSqueezedTensionColor = 0.0f, Color color_forSqueezedTension = default(Color), float width_relToViewportHeight = 0.0f, string text = null, float alphaOfReferenceLengthDisplay = 0.1f, float stylePatternScaleFactor = 1.0f, float endPlatesSize_relToViewportHeight = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawScreenspace.LineUnderTension") == false) { return; }
            LineUnderTension(automaticallyFoundCamera, start, end, relaxedLength_relToViewportHeight, relaxedColor, style, stretchFactor_forStretchedTensionColor, color_forStretchedTension, stretchFactor_forSqueezedTensionColor, color_forSqueezedTension, width_relToViewportHeight, text, alphaOfReferenceLengthDisplay, stylePatternScaleFactor, endPlatesSize_relToViewportHeight, enlargeSmallTextToThisMinRelTextSize, durationInSec);
        }

        public static void LineUnderTension(Camera targetCamera, Vector2 start, Vector2 end, float relaxedLength_relToViewportHeight = 0.4f, Color relaxedColor = default(Color), DrawBasics.LineStyle style = DrawBasics.LineStyle.sine, float stretchFactor_forStretchedTensionColor = 2.0f, Color color_forStretchedTension = default(Color), float stretchFactor_forSqueezedTensionColor = 0.0f, Color color_forSqueezedTension = default(Color), float width_relToViewportHeight = 0.0f, string text = null, float alphaOfReferenceLengthDisplay = 0.1f, float stylePatternScaleFactor = 1.0f, float endPlatesSize_relToViewportHeight = 0.0f, float enlargeSmallTextToThisMinRelTextSize = minTextSize_relToViewportHeight, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceLineUnderTension.Add(new ScreenspaceLineUnderTension(targetCamera, start, end, relaxedLength_relToViewportHeight, relaxedColor, style, stretchFactor_forStretchedTensionColor, color_forStretchedTension, stretchFactor_forSqueezedTensionColor, color_forSqueezedTension, width_relToViewportHeight, text, alphaOfReferenceLengthDisplay, stylePatternScaleFactor, endPlatesSize_relToViewportHeight, enlargeSmallTextToThisMinRelTextSize, durationInSec));
                return;
            }

            InternalDXXL_LineParamsFromCamViewportSpace lineParams = UtilitiesDXXL_Screenspace.GetLineParamsFromCamViewportSpace(targetCamera, start, end, width_relToViewportHeight, style, stylePatternScaleFactor, enlargeSmallTextToThisMinRelTextSize, 0.0f, endPlatesSize_relToViewportHeight);
            if (lineParams == null) { return; }

            Vector2 middleV2 = 0.5f * (start + end);
            float relaxedLength_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, middleV2, true, relaxedLength_relToViewportHeight);
            bool parametersAreInvalid = UtilitiesDXXL_DrawBasics.GetSpecsOfLineUnderTension(out float tensionFactor, out Color usedColor, out float lineLength_worldSpace, lineParams.startAnchor_worldSpace, lineParams.endAnchor_worldSpace, relaxedLength_worldSpace, relaxedColor, color_forStretchedTension, color_forSqueezedTension, stretchFactor_forStretchedTensionColor, stretchFactor_forSqueezedTensionColor);
            if (parametersAreInvalid) { return; }

            UtilitiesDXXL_DrawBasics.TryDrawReferenceLengthDisplay_ofLineUnderTension(lineParams.startAnchor_worldSpace, lineParams.endAnchor_worldSpace, alphaOfReferenceLengthDisplay, relaxedLength_worldSpace, relaxedColor, lineLength_worldSpace, lineParams.camPlane, durationInSec, false);

            UtilitiesDXXL_DrawBasics.Set_endPlates_sizeInterpretation_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
            UtilitiesDXXL_DrawBasics.Line(lineParams.startAnchor_worldSpace, lineParams.endAnchor_worldSpace, usedColor, lineParams.width_worldSpace, text, lineParams.lineStyleForcedTo2D, lineParams.patternScaleFactor_worldSpace, 0.0f, null, lineParams.camPlane, true, 0.0f, lineParams.enlargeSmallTextToThisMinTextSize_worldSpace, durationInSec, false, false, false, targetCamera, false, lineParams.endPlatesSize_inAbsoluteWorldSpaceUnits, tensionFactor);
            UtilitiesDXXL_DrawBasics.Reverse_endPlates_sizeInterpretation();
        }

        public static Vector2 DirectionInUnitsOfWarpedSpace_to_sameLookingDirectionInUnitsOfUnwarpedSpace(Vector2 directionInsideWarpedNonUniformViewportSpace_toConvert, Camera camera)
        {
            //when drawing in screenspace there is the problem that the interpretation of direction vectors is ambiguous. This is because the mostly non-squared screens result in a screenSpace/viewportSpace that has differnt length units for the x axis and y axis. For example the Vector (1,1) in a normal unwarped coordinate system goes to the upper right side with an angle of 45 degrees. In viewportSpace with an aspect ratio of 16 (width) : 9 (height), the same vector is horizontally stretched (warped) resulting in a flatter direction (of around 30 degrees)
            //If you have a direction vector in units of the non-uniform viewportSpace and want the same looking vector (meaning appearing with the same angle and same length to the human viewer), but expressed in units of an unwarped uniform coordinate space, then you can use this function.
            //Convention of the conversion executed by this function: The height of the camera viewport counts as 1 unit in the unwarped uniform coordinate space.
            //see also the "DirectionInUnwarpedSpace_toSameLookingDirectionInWarpedSpace" function

            if (camera == null)
            {
                Debug.LogError("Cannot convert to unwarped space, because camera is 'null'.");
                return directionInsideWarpedNonUniformViewportSpace_toConvert;
            }

            return new Vector2(directionInsideWarpedNonUniformViewportSpace_toConvert.x * camera.aspect, directionInsideWarpedNonUniformViewportSpace_toConvert.y);
        }

        public static Vector2 DirectionInUnitsOfUnwarpedSpace_to_sameLookingDirectionInUnitsOfWarpedSpace(Vector2 directionInsideUniform1by1SquareSpace_toConvert, Camera camera)
        {
            if (camera == null)
            {
                Debug.LogError("Cannot convert to warped space, because camera is 'null'.");
                return directionInsideUniform1by1SquareSpace_toConvert;
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(camera.aspect))
            {
                Debug.LogError("Cannot convert to warped space, because camera.aspect is 0.");
                return directionInsideUniform1by1SquareSpace_toConvert;
            }

            return new Vector2(directionInsideUniform1by1SquareSpace_toConvert.x / camera.aspect, directionInsideUniform1by1SquareSpace_toConvert.y);
        }

        public static Camera VisualizeAutomaticCameraForDrawing(bool visualizeFrustum = true, bool logPositionToConsole = true, Color color = default(Color), float durationInSec = 0.0f)
        {
            //If you want find out to which camera the Screenspace functions without camera parameter are drawing you can use this function. It can be handy if you have multiple cameras in the Scene and don't know which one is used for drawing to screenspace. See also "defaultCameraForDrawing" and "defaultScreenspaceWindowForDrawing". Note that it can also be the Scene View camera, in which case you cannot find the camera in your hierarchy.
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return null; }
            UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, null, false);
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ScreenspaceVisualizeAutomaticCameraForDrawing.Add(new ScreenspaceVisualizeAutomaticCameraForDrawing(visualizeFrustum, logPositionToConsole, color, durationInSec));
                return automaticallyFoundCamera;
            }

            if (automaticallyFoundCamera != null)
            {
                if (logPositionToConsole)
                {
                    Debug.Log("[Draw XXL] Position of the 'Automatic Camera for Drawing': " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(automaticallyFoundCamera.transform.position));
                }

                if (visualizeFrustum)
                {
                    string displayedText = "This is the camera to which Draw XXL draws when using the<br><b>DrawScreenspace</b><br>class.";
                    bool hiddenByNearerObjects = false;
                    DrawEngineBasics.CameraFrustum(automaticallyFoundCamera, color, 0.18f, 0.0f, 60, displayedText, true, default(Vector3), durationInSec, hiddenByNearerObjects);
                }
            }
            return automaticallyFoundCamera;
        }

    }

}
