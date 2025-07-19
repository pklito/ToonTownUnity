namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class DrawBasics2D
    {
        private static float default_zPos_forDrawing = 0.0f;
        public static float Default_zPos_forDrawing
        {
            get { return default_zPos_forDrawing; }
            set
            {
                if (float.IsNaN(value) || float.IsInfinity(value))
                {
                    Debug.LogError("Cannot set 'default_zPos_forDrawing' to " + value);
                }
                else
                {
                    default_zPos_forDrawing = value;
                }
            }
        }

        public static void Line(Vector2 start, Vector2 end, Color color = default(Color), float width = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Line_fadeableAnimSpeed_2D.InternalDraw(start, end, color, width, text, style, custom_zPos, stylePatternScaleFactor, animationSpeed, null, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void Ray(Vector2 start, Vector2 direction, Color color = default(Color), float width = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Ray_fadeableAnimSpeed_2D.InternalDraw(start, direction, color, width, text, style, custom_zPos, stylePatternScaleFactor, animationSpeed, null, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void LineFrom(Vector2 start, Vector2 direction, Color color = default(Color), float width = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            LineFrom_fadeableAnimSpeed_2D.InternalDraw(start, direction, color, width, text, style, custom_zPos, stylePatternScaleFactor, animationSpeed, null, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void LineTo(Vector2 direction, Vector2 end, Color color = default(Color), float width = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            LineTo_fadeableAnimSpeed_2D.InternalDraw(direction, end, color, width, text, style, custom_zPos, stylePatternScaleFactor, animationSpeed, null, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void LineColorFade(Vector2 start, Vector2 end, Color startColor, Color endColor, float width = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Line_fadeableAnimSpeed_2D.InternalDrawColorFade(start, end, startColor, endColor, width, text, style, custom_zPos, stylePatternScaleFactor, animationSpeed, null, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void RayColorFade(Vector2 start, Vector2 direction, Color startColor, Color endColor, float width = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Ray_fadeableAnimSpeed_2D.InternalDrawColorFade(start, direction, startColor, endColor, width, text, style, custom_zPos, stylePatternScaleFactor, animationSpeed, null, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void LineFrom_withColorFade(Vector2 start, Vector2 direction, Color startColor, Color endColor, float width = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            LineFrom_fadeableAnimSpeed_2D.InternalDraw_withColorFade(start, direction, startColor, endColor, width, text, style, custom_zPos, stylePatternScaleFactor, animationSpeed, null, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void LineTo_withColorFade(Vector2 direction, Vector2 end, Color startColor, Color endColor, float width = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            LineTo_fadeableAnimSpeed_2D.InternalDraw_withColorFade(direction, end, startColor, endColor, width, text, style, custom_zPos, stylePatternScaleFactor, animationSpeed, null, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void LineCircled(Vector2 circleCenter, Vector2 circleCenter_to_start, Vector2 circleCenter_to_end, Color color = default(Color), float forceRadius = 0.0f, float width = 0.0f, string text = null, bool useReflexAngleOver180deg = false, float custom_zPos = float.PositiveInfinity, bool skipFallbackDisplayOfZeroAngles = false, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 circleCenterV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(circleCenter, zPos);
            Vector3 circleCenter_to_startV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(circleCenter_to_start);
            Vector3 circleCenter_to_endV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(circleCenter_to_end);
            UtilitiesDXXL_LineCircled.LineCircled(circleCenterV3, circleCenter_to_startV3, circleCenter_to_endV3, color, forceRadius, width, text, useReflexAngleOver180deg, skipFallbackDisplayOfZeroAngles, true, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, hiddenByNearerObjects);
        }

        public static void LineCircled(Vector2 startPos, Vector2 circleCenter, float turnAngleDegCC, Color color = default(Color), float width = 0.0f, string text = null, float custom_zPos = float.PositiveInfinity, bool skipFallbackDisplayOfZeroAngles = false, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 startPosV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(startPos, zPos);
            Vector3 turnAxis_originV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(circleCenter, zPos);
            UtilitiesDXXL_LineCircled.LineCircled(startPosV3, turnAxis_originV3, Vector3.forward, turnAngleDegCC, color, width, text, skipFallbackDisplayOfZeroAngles, true, durationInSec, hiddenByNearerObjects, false, minAngleDeg_withoutTextLineBreak, textAnchor);
        }

        public static void LineCircled(Vector2 circleCenter, float startAngleDegCC_relativeToUp, float endAngleDegCC_relativeToUp, float radius = 0.5f, Color color = default(Color), float width = 0.0f, string text = null, float custom_zPos = float.PositiveInfinity, bool skipFallbackDisplayOfZeroAngles = false, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(startAngleDegCC_relativeToUp, "startAngleDegCC_relativeToUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(endAngleDegCC_relativeToUp, "endAngleDegCC_relativeToUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 turnAxis_originV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(circleCenter, zPos);
            float turnedAngleDegCC_fromStartAngle = endAngleDegCC_relativeToUp - startAngleDegCC_relativeToUp;
            Quaternion rotation_fromUp_toLineStart = Quaternion.AngleAxis(startAngleDegCC_relativeToUp, Vector3.forward);
            Vector3 cirleCenter_towards_lineStart_normalized = rotation_fromUp_toLineStart * Vector3.up;
            Vector3 startPosV3 = turnAxis_originV3 + cirleCenter_towards_lineStart_normalized * radius;
            UtilitiesDXXL_LineCircled.LineCircled(startPosV3, turnAxis_originV3, Vector3.forward, turnedAngleDegCC_fromStartAngle, color, width, text, skipFallbackDisplayOfZeroAngles, true, durationInSec, hiddenByNearerObjects, false, minAngleDeg_withoutTextLineBreak, textAnchor);
        }

        public static void CircleSegment(Vector2 circleCenter, Vector2 circleCenter_to_startPosOnPerimeter, Vector2 circleCenter_to_endPosOnPerimeter, Color color = default(Color), float forceRadius = 0.0f, string text = null, bool useReflexAngleOver180deg = false, float custom_zPos = float.PositiveInfinity, float radiusPortionWhereDrawFillStarts = 0.0f, bool skipFallbackDisplayOfZeroAngles = false, float fillDensity = 1.0f, float minAngleDeg_withoutTextLineBreak = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 circleCenterV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(circleCenter, zPos);
            Vector3 circleCenter_to_startV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(circleCenter_to_startPosOnPerimeter);
            Vector3 circleCenter_to_endV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(circleCenter_to_endPosOnPerimeter);
            UtilitiesDXXL_LineCircled.CircleSegment(circleCenterV3, circleCenter_to_startV3, circleCenter_to_endV3, color, forceRadius, fillDensity, text, useReflexAngleOver180deg, radiusPortionWhereDrawFillStarts, skipFallbackDisplayOfZeroAngles, minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL.LowerLeftOfWholeTextBlock, durationInSec, hiddenByNearerObjects);
        }

        public static void CircleSegment(Vector2 startPosOnPerimeter, Vector2 circleCenter, float turnAngleDegCC, Color color = default(Color), string text = null, float custom_zPos = float.PositiveInfinity, float radiusPortionWhereDrawFillStarts = 0.0f, bool skipFallbackDisplayOfZeroAngles = false, float fillDensity = 1.0f, float minAngleDeg_withoutTextLineBreak = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 startPosV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(startPosOnPerimeter, zPos);
            Vector3 turnAxis_originV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(circleCenter, zPos);
            UtilitiesDXXL_LineCircled.CircleSegment(startPosV3, turnAxis_originV3, Vector3.forward, turnAngleDegCC, color, text, radiusPortionWhereDrawFillStarts, skipFallbackDisplayOfZeroAngles, fillDensity, durationInSec, hiddenByNearerObjects, false, minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL.LowerLeftOfWholeTextBlock);
        }

        public static void CircleSegment(Vector2 circleCenter, float startAngleDegCC_relativeToUp, float endAngleDegCC_relativeToUp, float radius = 0.5f, Color color = default(Color), string text = null, float custom_zPos = float.PositiveInfinity, float radiusPortionWhereDrawFillStarts = 0.0f, bool skipFallbackDisplayOfZeroAngles = false, float fillDensity = 1.0f, float minAngleDeg_withoutTextLineBreak = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(startAngleDegCC_relativeToUp, "startAngleDegCC_relativeToUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(endAngleDegCC_relativeToUp, "endAngleDegCC_relativeToUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 turnAxis_originV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(circleCenter, zPos);
            float turnedAngleDegCC_fromStartAngle = endAngleDegCC_relativeToUp - startAngleDegCC_relativeToUp;
            Quaternion rotation_fromUp_toLineStart = Quaternion.AngleAxis(startAngleDegCC_relativeToUp, Vector3.forward);
            Vector3 cirleCenter_towards_lineStart_normalized = rotation_fromUp_toLineStart * Vector3.up;
            Vector3 startPosV3 = turnAxis_originV3 + cirleCenter_towards_lineStart_normalized * radius;
            UtilitiesDXXL_LineCircled.CircleSegment(startPosV3, turnAxis_originV3, Vector3.forward, turnedAngleDegCC_fromStartAngle, color, text, radiusPortionWhereDrawFillStarts, skipFallbackDisplayOfZeroAngles, fillDensity, durationInSec, hiddenByNearerObjects, false, minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL.LowerLeftOfWholeTextBlock);
        }

        public static void LineString(Vector2[] points, Color color = default(Color), bool closeGapBetweenLastAndFirstPoint = false, float width = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width, "width")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }

            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            width = UtilitiesDXXL_Math.AbsNonZeroValue(width);

            if (points.Length == 0)
            {
                Debug.Log("'points' has 0 items -> no drawing");
                return;
            }

            for (int i = 0; i < (points.Length - 1); i++)
            {
                Line_fadeableAnimSpeed_2D.InternalDraw(points[i], points[i + 1], color, width, null, style, zPos, stylePatternScaleFactor, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            if (closeGapBetweenLastAndFirstPoint)
            {
                Line_fadeableAnimSpeed_2D.InternalDraw(points[points.Length - 1], points[0], color, width, null, style, zPos, stylePatternScaleFactor, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            if (text != null && text != "")
            {
                TagLineString(zPos, text, points, width, color, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void LineString(List<Vector2> points, Color color = default(Color), bool closeGapBetweenLastAndFirstPoint = false, float width = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width, "width")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }

            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            width = UtilitiesDXXL_Math.AbsNonZeroValue(width);

            if (points.Count == 0)
            {
                Debug.Log("'points' has 0 items -> no drawing");
                return;
            }

            for (int i = 0; i < (points.Count - 1); i++)
            {
                Line_fadeableAnimSpeed_2D.InternalDraw(points[i], points[i + 1], color, width, null, style, zPos, stylePatternScaleFactor, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            if (closeGapBetweenLastAndFirstPoint)
            {
                Line_fadeableAnimSpeed_2D.InternalDraw(points[points.Count - 1], points[0], color, width, null, style, zPos, stylePatternScaleFactor, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            if (text != null && text != "")
            {
                TagLineString(zPos, text, points, width, color, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void LineStringColorFade(Vector2[] points, Color startColor, Color endColor, bool closeGapBetweenLastAndFirstPoint = false, float width = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width, "width")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }

            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            width = UtilitiesDXXL_Math.AbsNonZeroValue(width);

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
                Line_fadeableAnimSpeed_2D.InternalDraw(points[i], points[i + 1], color, width, null, style, zPos, stylePatternScaleFactor, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            if (closeGapBetweenLastAndFirstPoint)
            {
                Line_fadeableAnimSpeed_2D.InternalDraw(points[points.Length - 1], points[0], endColor, width, null, style, zPos, stylePatternScaleFactor, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            if (text != null && text != "")
            {
                Color averageColor = Color.Lerp(startColor, endColor, 0.5f);
                TagLineString(zPos, text, points, width, averageColor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void LineStringColorFade(List<Vector2> points, Color startColor, Color endColor, bool closeGapBetweenLastAndFirstPoint = false, float width = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width, "width")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }

            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            width = UtilitiesDXXL_Math.AbsNonZeroValue(width);

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
                Line_fadeableAnimSpeed_2D.InternalDraw(points[i], points[i + 1], color, width, null, style, zPos, stylePatternScaleFactor, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            if (closeGapBetweenLastAndFirstPoint)
            {
                Line_fadeableAnimSpeed_2D.InternalDraw(points[points.Count - 1], points[0], endColor, width, null, style, zPos, stylePatternScaleFactor, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            if (text != null && text != "")
            {
                Color averageColor = Color.Lerp(startColor, endColor, 0.5f);
                TagLineString(zPos, text, points, width, averageColor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
        }

        static void TagLineString(float zPos, string text, Vector2[] lineStringVerticesGlobal, float linesWidth, Color colorOfLines, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects)
        {
            if (lineStringVerticesGlobal.Length <= 0)
            {
                Debug.LogError("lineStringVerticesGlobal has " + lineStringVerticesGlobal.Length + " items -> no 'drawTag' operation");
                return;
            }

            if (text != null && text != "")
            {
                float xMin = UtilitiesDXXL_Math.GetLowestXComponent(lineStringVerticesGlobal);
                float xMax = UtilitiesDXXL_Math.GetHighestXComponent(lineStringVerticesGlobal);
                float yMin = UtilitiesDXXL_Math.GetLowestYComponent(lineStringVerticesGlobal);
                float yMax = UtilitiesDXXL_Math.GetHighestYComponent(lineStringVerticesGlobal);
                Vector3 virtualScale = new Vector3(xMax - xMin, yMax - yMin, 0.0f);
                Vector3 centerPosition = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(lineStringVerticesGlobal[0], zPos);
                for (int i = 0; i < lineStringVerticesGlobal.Length; i++)
                {
                    UtilitiesDXXL_List.AddToAVectorList(ref UtilitiesDXXL_Shapes.verticesLocal, UtilitiesDXXL_DrawBasics2D.Position_V2toV3(lineStringVerticesGlobal[i], zPos) - centerPosition, i);
                }
                Color invertedColor = UtilitiesDXXL_Colors.Invert_andAlphaTo1(colorOfLines);
                UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, null, centerPosition, lineStringVerticesGlobal.Length, 0.1f * linesWidth, virtualScale, invertedColor, invertedColor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
        }

        static void TagLineString(float zPos, string text, List<Vector2> lineStringVerticesGlobal, float linesWidth, Color colorOfLines, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects)
        {
            if (lineStringVerticesGlobal.Count <= 0)
            {
                Debug.LogError("lineStringVerticesGlobal has " + lineStringVerticesGlobal.Count + " items -> no 'drawTag' operation");
                return;
            }

            if (text != null && text != "")
            {
                float xMin = UtilitiesDXXL_Math.GetLowestXComponent(lineStringVerticesGlobal);
                float xMax = UtilitiesDXXL_Math.GetHighestXComponent(lineStringVerticesGlobal);
                float yMin = UtilitiesDXXL_Math.GetLowestYComponent(lineStringVerticesGlobal);
                float yMax = UtilitiesDXXL_Math.GetHighestYComponent(lineStringVerticesGlobal);
                Vector3 virtualScale = new Vector3(xMax - xMin, yMax - yMin, 0.0f);
                Vector3 centerPosition = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(lineStringVerticesGlobal[0], zPos);
                for (int i = 0; i < lineStringVerticesGlobal.Count; i++)
                {
                    UtilitiesDXXL_List.AddToAVectorList(ref UtilitiesDXXL_Shapes.verticesLocal, UtilitiesDXXL_DrawBasics2D.Position_V2toV3(lineStringVerticesGlobal[i], zPos) - centerPosition, i);
                }
                Color invertedColor = UtilitiesDXXL_Colors.Invert_andAlphaTo1(colorOfLines);
                UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, null, centerPosition, lineStringVerticesGlobal.Count, 0.1f * linesWidth, virtualScale, invertedColor, invertedColor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void PointArray(Vector2[] points, Color color = default(Color), float sizeOfMarkingCross = 1.0f, float markingCrossLinesWidth = 0.0f, bool drawCoordsAsText = true, float custom_zPos = float.PositiveInfinity, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }

            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            for (int i = 0; i < points.Length; i++)
            {
                Point(points[i], color, sizeOfMarkingCross, 0.0f, markingCrossLinesWidth, null, color, zPos, false, drawCoordsAsText, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void PointList(List<Vector2> points, Color color = default(Color), float sizeOfMarkingCross = 1.0f, float markingCrossLinesWidth = 0.0f, bool drawCoordsAsText = true, float custom_zPos = float.PositiveInfinity, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }

            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            for (int i = 0; i < points.Count; i++)
            {
                Point(points[i], color, sizeOfMarkingCross, 0.0f, markingCrossLinesWidth, null, color, zPos, false, drawCoordsAsText, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void Point(Vector2 position, Color markingCrossColor, float sizeOfMarkingCross = 1.0f, float angleDegCC = 0.0f, float markingCrossLinesWidth = 0.0f, string text = null, Color textColor = default(Color), float custom_zPos = float.PositiveInfinity, bool pointer_as_textAttachStyle = false, bool drawCoordsAsText = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Point(position, text, textColor, sizeOfMarkingCross, markingCrossLinesWidth, zPos, markingCrossColor, angleDegCC, pointer_as_textAttachStyle, drawCoordsAsText, durationInSec, hiddenByNearerObjects);
        }

        public static void Point(Vector2 position, string text = null, Color textColor = default(Color), float sizeOfMarkingCross = 1.0f, float markingCrossLinesWidth = 0.0f, float custom_zPos = float.PositiveInfinity, Color overwrite_markingCrossColor = default(Color), float angleDegCC = 0.0f, bool pointer_as_textAttachStyle = true, bool drawCoordsAsText = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 positionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(position, zPos);
            Quaternion rotation = UtilitiesDXXL_DrawBasics2D.QuaternionFromAngle(angleDegCC);
            UtilitiesDXXL_DrawBasics.Point(true, positionV3, text, textColor, sizeOfMarkingCross, markingCrossLinesWidth, overwrite_markingCrossColor, rotation, pointer_as_textAttachStyle, drawCoordsAsText, false, true, Vector3.zero, Quaternion.identity, Vector3.one, true, durationInSec, hiddenByNearerObjects);
        }

        public static void PointTag(Vector2 position, string text = null, Color color = default(Color), float linesWidth = 0.0f, float size_asTextOffsetDistance = 1.0f, Vector2 textOffsetDirection = default(Vector2), float custom_zPos = float.PositiveInfinity, float textSizeScaleFactor = 1.0f, bool skipConeDrawing = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 positionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(position, zPos);
            Vector3 textOffsetDirV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(textOffsetDirection);
            UtilitiesDXXL_DrawBasics2D.PointTag(positionV3, text, color, linesWidth, size_asTextOffsetDistance, textOffsetDirV3, textSizeScaleFactor, skipConeDrawing, durationInSec, hiddenByNearerObjects);
        }

        public static void Vector(Vector2 vectorStartPos, Vector2 vectorEndPos, Color color = default(Color), float lineWidth = 0.0f, string text = null, float coneLength = 0.17f, bool pointerAtBothSides = false, float custom_zPos = float.PositiveInfinity, bool addNormalizedMarkingText = false, float enlargeSmallTextToThisMinTextSize = 0.0f, bool writeComponentValuesAsText = false, float endPlates_size = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 vectorStartPosV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(vectorStartPos, zPos);
            Vector3 vectorEndPosV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(vectorEndPos, zPos);
            UtilitiesDXXL_DrawBasics.Vector(vectorStartPosV3, vectorEndPosV3, color, lineWidth, text, coneLength, pointerAtBothSides, true, addNormalizedMarkingText, enlargeSmallTextToThisMinTextSize, writeComponentValuesAsText, durationInSec, hiddenByNearerObjects, UtilitiesDXXL_DrawBasics2D.xyPlane_throughZero, true, endPlates_size);
        }

        public static void VectorFrom(Vector2 vectorStartPos, Vector2 vector, Color color = default(Color), float lineWidth = 0.0f, string text = null, float coneLength = 0.17f, bool pointerAtBothSides = false, float custom_zPos = float.PositiveInfinity, bool addNormalizedMarkingText = false, float enlargeSmallTextToThisMinTextSize = 0.0f, bool writeComponentValuesAsText = false, float endPlates_size = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 vectorStartPosV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(vectorStartPos, zPos);
            Vector3 vectorV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(vector);
            UtilitiesDXXL_DrawBasics.VectorFrom(vectorStartPosV3, vectorV3, color, lineWidth, text, coneLength, pointerAtBothSides, true, addNormalizedMarkingText, enlargeSmallTextToThisMinTextSize, writeComponentValuesAsText, durationInSec, hiddenByNearerObjects, UtilitiesDXXL_DrawBasics2D.xyPlane_throughZero, true, endPlates_size);
        }

        public static void VectorTo(Vector2 vector, Vector2 vectorEndPos, Color color = default(Color), float lineWidth = 0.0f, string text = null, float coneLength = 0.17f, bool pointerAtBothSides = false, float custom_zPos = float.PositiveInfinity, bool addNormalizedMarkingText = false, float enlargeSmallTextToThisMinTextSize = 0.0f, bool writeComponentValuesAsText = false, float endPlates_size = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vectorEndPos, "vectorEndPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vector, "vector")) { return; }
            VectorFrom(vectorEndPos - vector, vector, color, lineWidth, text, coneLength, pointerAtBothSides, custom_zPos, addNormalizedMarkingText, enlargeSmallTextToThisMinTextSize, writeComponentValuesAsText, endPlates_size, durationInSec, hiddenByNearerObjects);
        }

        public static void VectorCircled(Vector2 circleCenter, Vector2 circleCenter_to_start, Vector2 circleCenter_to_end, Color color = default(Color), float forceRadius = 0.0f, float lineWidth = 0.0f, string text = null, bool useReflexAngleOver180deg = false, float custom_zPos = float.PositiveInfinity, float coneLength = 0.17f, bool skipFallbackDisplayOfZeroAngles = false, bool pointerAtBothSides = false, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 circleCenterV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(circleCenter, zPos);
            Vector3 circleCenter_to_startV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(circleCenter_to_start);
            Vector3 circleCenter_to_endV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(circleCenter_to_end);
            UtilitiesDXXL_LineCircled.VectorCircled(circleCenterV3, circleCenter_to_startV3, circleCenter_to_endV3, color, forceRadius, lineWidth, text, useReflexAngleOver180deg, coneLength, skipFallbackDisplayOfZeroAngles, pointerAtBothSides, true, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, hiddenByNearerObjects);
        }

        public static void VectorCircled(Vector2 startPos, Vector2 circleCenter, float turnAngleDegCC, Color color = default(Color), float lineWidth = 0.0f, string text = null, float custom_zPos = float.PositiveInfinity, float coneLength = 0.17f, bool skipFallbackDisplayOfZeroAngles = false, bool pointerAtBothSides = false, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 startPosV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(startPos, zPos);
            Vector3 circleCenterV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(circleCenter, zPos);
            UtilitiesDXXL_LineCircled.VectorCircled(startPosV3, circleCenterV3, Vector3.forward, turnAngleDegCC, color, lineWidth, text, coneLength, skipFallbackDisplayOfZeroAngles, pointerAtBothSides, true, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, hiddenByNearerObjects, 1.0f);
        }

        public static void VectorCircled(Vector2 circleCenter, float startAngleDegCC_relativeToUp, float endAngleDegCC_relativeToUp, float radius = 0.5f, Color color = default(Color), float lineWidth = 0.0f, string text = null, float custom_zPos = float.PositiveInfinity, float coneLength = 0.17f, bool skipFallbackDisplayOfZeroAngles = false, bool pointerAtBothSides = false, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(startAngleDegCC_relativeToUp, "startAngleDegCC_relativeToUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(endAngleDegCC_relativeToUp, "endAngleDegCC_relativeToUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 circleCenterV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(circleCenter, zPos);
            float turnedAngleDegCC_fromStartAngle = endAngleDegCC_relativeToUp - startAngleDegCC_relativeToUp;
            Quaternion rotation_fromUp_toLineStart = Quaternion.AngleAxis(startAngleDegCC_relativeToUp, Vector3.forward);
            Vector3 cirleCenter_towards_lineStart_normalized = rotation_fromUp_toLineStart * Vector3.up;
            Vector3 startPosV3 = circleCenterV3 + cirleCenter_towards_lineStart_normalized * radius;
            UtilitiesDXXL_LineCircled.VectorCircled(startPosV3, circleCenterV3, Vector3.forward, turnedAngleDegCC_fromStartAngle, color, lineWidth, text, coneLength, skipFallbackDisplayOfZeroAngles, pointerAtBothSides, true, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, hiddenByNearerObjects, 1.0f);
        }

        public static void MovingArrowsRay(Vector2 start, Vector2 direction, Color color = default(Color), float lineWidth = 0.05f, float distanceBetweenArrows = 0.5f, float lengthOfArrows = 0.15f, string text = null, float custom_zPos = float.PositiveInfinity, float animationSpeed = 0.5f, bool backwardAnimationFlipsArrowDirection = true, float endPlates_size = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            MovingArrowsRay_fadeableAnimSpeed_2D.InternalDraw(start, direction, color, lineWidth, distanceBetweenArrows, lengthOfArrows, text, custom_zPos, animationSpeed, null, backwardAnimationFlipsArrowDirection, endPlates_size, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects);
        }

        public static void MovingArrowsLine(Vector2 start, Vector2 end, Color color = default(Color), float lineWidth = 0.05f, float distanceBetweenArrows = 0.5f, float lengthOfArrows = 0.15f, string text = null, float custom_zPos = float.PositiveInfinity, float animationSpeed = 0.5f, bool backwardAnimationFlipsArrowDirection = true, float endPlates_size = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            MovingArrowsLine_fadeableAnimSpeed_2D.InternalDraw(start, end, color, lineWidth, distanceBetweenArrows, lengthOfArrows, text, custom_zPos, animationSpeed, null, backwardAnimationFlipsArrowDirection, endPlates_size, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects);
        }

        public static void RayWithAlternatingColors(Vector2 start, Vector2 direction, Color color1 = default(Color), Color color2 = default(Color), float width = 0.0f, float lengthOfStripes = 0.04f, string text = null, float custom_zPos = float.PositiveInfinity, float animationSpeed = 0.0f, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            RayWithAlternatingColors_fadeableAnimSpeed_2D.InternalDraw(start, direction, color1, color2, width, lengthOfStripes, text, custom_zPos, animationSpeed, null, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void LineWithAlternatingColors(Vector2 start, Vector2 end, Color color1 = default(Color), Color color2 = default(Color), float width = 0.0f, float lengthOfStripes = 0.04f, string text = null, float custom_zPos = float.PositiveInfinity, float animationSpeed = 0.0f, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            //Lines drawn with this function have a higher likelyhood of accidentially using up high numbers of drawnLinePerFrame, because "lengthOfStripes" can be set manually instead of beeing determined by the lineStyle-code 
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            LineWithAlternatingColors_fadeableAnimSpeed_2D.InternalDraw(start, end, color1, color2, width, lengthOfStripes, text, custom_zPos, animationSpeed, null, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void BlinkingRay(Vector2 start, Vector2 direction, Color primaryColor = default(Color), float blinkDurationInSec = 0.5f, float width = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, Color blinkColor = default(Color), float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(direction, "direction")) { return; }

            Vector2 end = start + direction;
            BlinkingLine(start, end, primaryColor, blinkDurationInSec, width, text, style, blinkColor, custom_zPos, stylePatternScaleFactor, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void BlinkingLine(Vector2 start, Vector2 end, Color primaryColor = default(Color), float blinkDurationInSec = 0.5f, float width = 0.0f, string text = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, Color blinkColor = default(Color), float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(blinkDurationInSec, "blinkDurationInSec")) { return; }

            blinkDurationInSec = Mathf.Max(blinkDurationInSec, UtilitiesDXXL_DrawBasics.min_blinkDurationInSec);
            float passedBlinkIntervallsSinceStartup = UtilitiesDXXL_LineStyles.GetTime() / blinkDurationInSec;
            primaryColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(primaryColor);
            if (UtilitiesDXXL_Math.CheckIf_givenNumberIs_evenNotOdd(Mathf.FloorToInt(passedBlinkIntervallsSinceStartup)))
            {
                Line_fadeableAnimSpeed_2D.InternalDraw(start, end, primaryColor, width, text, style, custom_zPos, stylePatternScaleFactor, 0.0f, null, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
            }
            else
            {
                if (UtilitiesDXXL_Colors.IsDefaultColor(blinkColor))
                {
                    Color alternatingBlinkColor = UtilitiesDXXL_Colors.Invert_andAlphaTo1(primaryColor);
                    alternatingBlinkColor = UtilitiesDXXL_Colors.OverwriteColorNearGreyWithBlack(alternatingBlinkColor);
                    Line_fadeableAnimSpeed_2D.InternalDraw(start, end, alternatingBlinkColor, width, text, style, custom_zPos, stylePatternScaleFactor, 0.0f, null, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
                }
                else
                {
                    Line_fadeableAnimSpeed_2D.InternalDraw(start, end, blinkColor, width, text, style, custom_zPos, stylePatternScaleFactor, 0.0f, null, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
                }
            }
        }

        public static void RayUnderTension(Vector2 start, Vector2 direction, float relaxedLength = 1.0f, Color relaxedColor = default(Color), DrawBasics.LineStyle style = DrawBasics.LineStyle.sine, float stretchFactor_forStretchedTensionColor = 2.0f, Color color_forStretchedTension = default(Color), float stretchFactor_forSqueezedTensionColor = 0.0f, Color color_forSqueezedTension = default(Color), float width = 0.0f, string text = null, float alphaOfReferenceLengthDisplay = 0.15f, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, float endPlates_size = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(direction, "direction")) { return; }
            LineUnderTension(start, start + direction, relaxedLength, relaxedColor, style, stretchFactor_forStretchedTensionColor, color_forStretchedTension, stretchFactor_forSqueezedTensionColor, color_forSqueezedTension, width, text, alphaOfReferenceLengthDisplay, custom_zPos, stylePatternScaleFactor, endPlates_size, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void LineUnderTension(Vector2 start, Vector2 end, float relaxedLength = 1.0f, Color relaxedColor = default(Color), DrawBasics.LineStyle style = DrawBasics.LineStyle.sine, float stretchFactor_forStretchedTensionColor = 2.0f, Color color_forStretchedTension = default(Color), float stretchFactor_forSqueezedTensionColor = 0.0f, Color color_forSqueezedTension = default(Color), float width = 0.0f, string text = null, float alphaOfReferenceLengthDisplay = 0.15f, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, float endPlates_size = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 startV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(start, zPos);
            Vector3 endV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(end, zPos);
            style = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(style);

            bool parametersAreInvalid = UtilitiesDXXL_DrawBasics.GetSpecsOfLineUnderTension(out float tensionFactor, out Color usedColor, out float lineLength, startV3, endV3, relaxedLength, relaxedColor, color_forStretchedTension, color_forSqueezedTension, stretchFactor_forStretchedTensionColor, stretchFactor_forSqueezedTensionColor);
            if (parametersAreInvalid) { return; }

            UtilitiesDXXL_DrawBasics.TryDrawReferenceLengthDisplay_ofLineUnderTension(startV3, endV3, alphaOfReferenceLengthDisplay, relaxedLength, relaxedColor, lineLength, UtilitiesDXXL_DrawBasics2D.xyPlane_throughZero, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_DrawBasics.Line(startV3, endV3, usedColor, width, text, style, stylePatternScaleFactor, 0.0f, null, UtilitiesDXXL_DrawBasics2D.xyPlane_throughZero, true, 0.0f, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines, null, true, endPlates_size, tensionFactor);
        }

        public static void Icon(Vector2 position, DrawBasics.IconType icon, Color color = default(Color), float size = 1.0f, string text = null, float turnAngleDegCC = 0.0f, int strokeWidth_asPPMofSize = 0, float custom_zPos = float.PositiveInfinity, bool mirrorHorizontally = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 positionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(position, zPos);
            Quaternion rotation = UtilitiesDXXL_DrawBasics2D.QuaternionFromAngle(turnAngleDegCC);
            UtilitiesDXXL_DrawBasics.Icon(positionV3, icon, color, size, text, rotation, strokeWidth_asPPMofSize, mirrorHorizontally, durationInSec, hiddenByNearerObjects, 0.1f, 0.004f, true);
        }

        public static void Shape(Rect boxRect, DrawShapes.Shape2DType shape = DrawShapes.Shape2DType.square, Color color = default(Color), float angleDegCC = 0.0f, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Box(boxRect, color, angleDegCC, shape, linesWidth, text, lineStyle, zPos, stylePatternScaleFactor, fillStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Shape(Vector2 centerPosition, DrawShapes.Shape2DType shape, Vector2 size, Color color = default(Color), float angleDegCC = 0.0f, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Box(centerPosition, size, color, angleDegCC, shape, linesWidth, text, lineStyle, zPos, stylePatternScaleFactor, fillStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Dot(Vector2 position, float radius = 0.5f, Color color = default(Color), string text = null, float custom_zPos = float.PositiveInfinity, float density = 1.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 centerPositionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(position, zPos);
            DrawBasics.Dot(centerPositionV3, radius, Vector3.forward, color, text, density, durationInSec, hiddenByNearerObjects);
        }

        public static void Box(Rect boxRect, Color color = default(Color), float angleDegCC = 0.0f, DrawShapes.Shape2DType shape = DrawShapes.Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            DrawShapes.Box2D(boxRect, color, zPos, angleDegCC, shape, linesWidth, text, lineStyle, stylePatternScaleFactor, fillStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Box(Vector2 centerPosition, Vector2 size, Color color = default(Color), float angleDegCC = 0.0f, DrawShapes.Shape2DType shape = DrawShapes.Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            DrawShapes.Box2D(centerPosition, size, color, zPos, angleDegCC, shape, linesWidth, text, lineStyle, stylePatternScaleFactor, fillStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Circle(Rect hullRect, Color color = default(Color), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            DrawShapes.Circle2D(hullRect, color, zPos, linesWidth, text, lineStyle, stylePatternScaleFactor, fillStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Circle(Vector2 centerPosition, float radius = 0.5f, Color color = default(Color), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            DrawShapes.Circle2D(centerPosition, radius, color, zPos, linesWidth, text, lineStyle, stylePatternScaleFactor, fillStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Capsule(Vector2 posOfCircle1, Vector2 posOfCircle2, float radius = 0.5f, Color color = default(Color), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            DrawShapes.Capsule2D(posOfCircle1, posOfCircle2, radius, color, zPos, linesWidth, text, lineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Capsule(Rect hullRect, Color color = default(Color), CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Vertical, float angleDegCC = 0.0f, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            DrawShapes.Capsule2D(hullRect, color, zPos, capsuleDirection, angleDegCC, linesWidth, text, lineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Capsule(Vector2 centerPosition, Vector2 size, Color color = default(Color), CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Vertical, float angleDegCC = 0.0f, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float custom_zPos = float.PositiveInfinity, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            DrawShapes.Capsule2D(centerPosition, size, color, zPos, capsuleDirection, angleDegCC, linesWidth, text, lineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentQuadratic(GameObject startPositionAndDirection, GameObject endPosition, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, float custom_zPos = float.PositiveInfinity, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(startPositionAndDirection, "startPositionAndDirection")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(endPosition, "endPosition")) { return; }
            BezierSegmentQuadratic(startPositionAndDirection.transform, endPosition.transform, color, text, width, straightSubDivisions, custom_zPos, textSize, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentQuadratic(Transform startPositionAndDirection, Transform endPosition, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, float custom_zPos = float.PositiveInfinity, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(startPositionAndDirection, "startPositionAndDirection")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(endPosition, "endPosition")) { return; }
            Vector3 controlPosInBetween = startPositionAndDirection.position + startPositionAndDirection.right * startPositionAndDirection.localScale.x;
            BezierSegmentQuadratic(startPositionAndDirection.position, endPosition.position, controlPosInBetween, color, text, width, straightSubDivisions, custom_zPos, textSize, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentQuadratic(GameObject startPosition, GameObject endPosition, GameObject controlPosInBetween, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, float custom_zPos = float.PositiveInfinity, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(startPosition, "startPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(endPosition, "endPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(controlPosInBetween, "controlPosInBetween")) { return; }
            BezierSegmentQuadratic(startPosition.transform.position, endPosition.transform.position, controlPosInBetween.transform.position, color, text, width, straightSubDivisions, custom_zPos, textSize, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentQuadratic(Transform startPosition, Transform endPosition, Transform controlPosInBetween, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, float custom_zPos = float.PositiveInfinity, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(startPosition, "startPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(endPosition, "endPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(controlPosInBetween, "controlPosInBetween")) { return; }
            BezierSegmentQuadratic(startPosition.position, endPosition.position, controlPosInBetween.position, color, text, width, straightSubDivisions, custom_zPos, textSize, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentQuadratic(Vector2 startPosition, Vector2 endPosition, Vector2 controlPosInBetween, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, float custom_zPos = float.PositiveInfinity, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(startPosition, "startPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(endPosition, "endPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(controlPosInBetween, "controlPosInBetween")) { return; }

            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 startPositionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(startPosition, zPos);
            Vector3 endPositionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(endPosition, zPos);
            Vector3 controlPosInBetweenV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(controlPosInBetween, zPos);
            UtilitiesDXXL_Bezier.BezierSegmentQuadratic(true, startPositionV3, endPositionV3, controlPosInBetweenV3, color, text, width, straightSubDivisions, textSize, true, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentCubic(GameObject startPositionAndDirection, GameObject endPositionAndDirection, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, float custom_zPos = float.PositiveInfinity, bool closeGapFromEndToStart = false, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(startPositionAndDirection, "startPositionAndDirection")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(endPositionAndDirection, "endPositionAndDirection")) { return; }
            BezierSegmentCubic(startPositionAndDirection.transform, endPositionAndDirection.transform, color, text, width, straightSubDivisions, custom_zPos, closeGapFromEndToStart, textSize, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentCubic(Transform startPositionAndDirection, Transform endPositionAndDirection, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, float custom_zPos = float.PositiveInfinity, bool closeGapFromEndToStart = false, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(startPositionAndDirection, "startPositionAndDirection")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(endPositionAndDirection, "endPositionAndDirection")) { return; }
            Vector3 controlPosOfStartDirection = startPositionAndDirection.position + startPositionAndDirection.right * startPositionAndDirection.localScale.x;
            Vector3 controlPosOfEndDirection = endPositionAndDirection.position - endPositionAndDirection.right * endPositionAndDirection.localScale.x;
            BezierSegmentCubic(startPositionAndDirection.position, endPositionAndDirection.position, controlPosOfStartDirection, controlPosOfEndDirection, color, text, width, straightSubDivisions, custom_zPos, closeGapFromEndToStart, textSize, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentCubic(GameObject startPosition, GameObject endPosition, GameObject controlPosOfStartDirection, GameObject controlPosOfEndDirection, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, float custom_zPos = float.PositiveInfinity, bool closeGapFromEndToStart = false, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(startPosition, "startPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(endPosition, "endPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(controlPosOfStartDirection, "controlPosOfStartDirection")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(controlPosOfEndDirection, "controlPosOfEndDirection")) { return; }
            BezierSegmentCubic(startPosition.transform.position, endPosition.transform.position, controlPosOfStartDirection.transform.position, controlPosOfEndDirection.transform.position, color, text, width, straightSubDivisions, custom_zPos, closeGapFromEndToStart, textSize, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentCubic(Transform startPosition, Transform endPosition, Transform controlPosOfStartDirection, Transform controlPosOfEndDirection, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, float custom_zPos = float.PositiveInfinity, bool closeGapFromEndToStart = false, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(startPosition, "startPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(endPosition, "endPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(controlPosOfStartDirection, "controlPosOfStartDirection")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(controlPosOfEndDirection, "controlPosOfEndDirection")) { return; }
            BezierSegmentCubic(startPosition.position, endPosition.position, controlPosOfStartDirection.position, controlPosOfEndDirection.position, color, text, width, straightSubDivisions, custom_zPos, closeGapFromEndToStart, textSize, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentCubic(Vector2 startPosition, Vector2 endPosition, Vector2 controlPosOfStartDirection, Vector2 controlPosOfEndDirection, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, float custom_zPos = float.PositiveInfinity, bool closeGapFromEndToStart = false, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(startPosition, "startPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(endPosition, "endPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(controlPosOfStartDirection, "controlPosOfStartDirection")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(controlPosOfEndDirection, "controlPosOfEndDirection")) { return; }

            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 startPositionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(startPosition, zPos);
            Vector3 endPositionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(endPosition, zPos);
            Vector3 controlPosOfStartDirectionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(controlPosOfStartDirection, zPos);
            Vector3 controlPosOfEndDirectionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(controlPosOfEndDirection, zPos);
            UtilitiesDXXL_Bezier.BezierSegmentCubic(true, startPositionV3, endPositionV3, controlPosOfStartDirectionV3, controlPosOfEndDirectionV3, color, text, width, straightSubDivisions, textSize, true, durationInSec, hiddenByNearerObjects);
            if (closeGapFromEndToStart)
            {
                Vector3 startPos_to_startControlPosV3 = controlPosOfStartDirectionV3 - startPositionV3;
                Vector3 endPos_to_endControlPosV3 = controlPosOfEndDirectionV3 - endPositionV3;
                Vector3 controlPosOfStartDirection_ofSecondCurveV3 = startPositionV3 - startPos_to_startControlPosV3;
                Vector3 controlPosOfEndDirection_ofSecondCurveV3 = endPositionV3 - endPos_to_endControlPosV3;
                UtilitiesDXXL_Bezier.BezierSegmentCubic(true, startPositionV3, endPositionV3, controlPosOfStartDirection_ofSecondCurveV3, controlPosOfEndDirection_ofSecondCurveV3, color, null, width, straightSubDivisions, textSize, true, durationInSec, hiddenByNearerObjects);
            }
        }


        static UtilitiesDXXL_Bezier.FlexibleGetPosAtIndex<GameObject[]> GetPositions3DFromGameObjectsArray_2D_preAllocated = UtilitiesDXXL_Bezier.GetPositions3DFromGameObjectsArray_2D;
        static UtilitiesDXXL_Bezier.FlexibleGetDirectionControlPosOfTransform<GameObject[]> GetForwardControlPos3DFromGameObjectsArray_2D_preAllocated = UtilitiesDXXL_Bezier.GetForwardControlPos3DFromGameObjectsArray_2D;
        static UtilitiesDXXL_Bezier.FlexibleGetDirectionControlPosOfTransform<GameObject[]> GetBackwardControlPos3DFromGameObjectsArray_2D_preAllocated = UtilitiesDXXL_Bezier.GetBackwardControlPos3DFromGameObjectsArray_2D;
        public static void BezierSpline(GameObject[] points, Color color = default(Color), DrawBasics.BezierPosInterpretation interpretationOfArray = DrawBasics.BezierPosInterpretation.onlySegmentStartPoints_backwardForwardIsAligned, string text = null, float width = 0.0f, bool closeGapFromEndToStart = false, int straightSubDivisionsPerSegment = 50, float custom_zPos = float.PositiveInfinity, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            UtilitiesDXXL_Bezier.BezierSpline<GameObject[]>(true, custom_zPos, points, GetPositions3DFromGameObjectsArray_2D_preAllocated, GetForwardControlPos3DFromGameObjectsArray_2D_preAllocated, GetBackwardControlPos3DFromGameObjectsArray_2D_preAllocated, points.Length, color, interpretationOfArray, text, width, closeGapFromEndToStart, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
        }


        static UtilitiesDXXL_Bezier.FlexibleGetPosAtIndex<List<GameObject>> GetPositions3DFromGameObjectsList_2D_preAllocated = UtilitiesDXXL_Bezier.GetPositions3DFromGameObjectsList_2D;
        static UtilitiesDXXL_Bezier.FlexibleGetDirectionControlPosOfTransform<List<GameObject>> GetForwardControlPos3DFromGameObjectsList_2D_preAllocated = UtilitiesDXXL_Bezier.GetForwardControlPos3DFromGameObjectsList_2D;
        static UtilitiesDXXL_Bezier.FlexibleGetDirectionControlPosOfTransform<List<GameObject>> GetBackwardControlPos3DFromGameObjectsList_2D_preAllocated = UtilitiesDXXL_Bezier.GetBackwardControlPos3DFromGameObjectsList_2D;
        public static void BezierSpline(List<GameObject> points, Color color = default(Color), DrawBasics.BezierPosInterpretation interpretationOfList = DrawBasics.BezierPosInterpretation.onlySegmentStartPoints_backwardForwardIsAligned, string text = null, float width = 0.0f, bool closeGapFromEndToStart = false, int straightSubDivisionsPerSegment = 50, float custom_zPos = float.PositiveInfinity, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            UtilitiesDXXL_Bezier.BezierSpline<List<GameObject>>(true, custom_zPos, points, GetPositions3DFromGameObjectsList_2D_preAllocated, GetForwardControlPos3DFromGameObjectsList_2D_preAllocated, GetBackwardControlPos3DFromGameObjectsList_2D_preAllocated, points.Count, color, interpretationOfList, text, width, closeGapFromEndToStart, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
        }


        static UtilitiesDXXL_Bezier.FlexibleGetPosAtIndex<Transform[]> GetPositions3DFromTransformsArray_2D_preAllocated = UtilitiesDXXL_Bezier.GetPositions3DFromTransformsArray_2D;
        static UtilitiesDXXL_Bezier.FlexibleGetDirectionControlPosOfTransform<Transform[]> GetForwardControlPos3DFromTransformsArray_2D_preAllocated = UtilitiesDXXL_Bezier.GetForwardControlPos3DFromTransformsArray_2D;
        static UtilitiesDXXL_Bezier.FlexibleGetDirectionControlPosOfTransform<Transform[]> GetBackwardControlPos3DFromTransformsArray_2D_preAllocated = UtilitiesDXXL_Bezier.GetBackwardControlPos3DFromTransformsArray_2D;
        public static void BezierSpline(Transform[] points, Color color = default(Color), DrawBasics.BezierPosInterpretation interpretationOfArray = DrawBasics.BezierPosInterpretation.onlySegmentStartPoints_backwardForwardIsAligned, string text = null, float width = 0.0f, bool closeGapFromEndToStart = false, int straightSubDivisionsPerSegment = 50, float custom_zPos = float.PositiveInfinity, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            UtilitiesDXXL_Bezier.BezierSpline<Transform[]>(true, custom_zPos, points, GetPositions3DFromTransformsArray_2D_preAllocated, GetForwardControlPos3DFromTransformsArray_2D_preAllocated, GetBackwardControlPos3DFromTransformsArray_2D_preAllocated, points.Length, color, interpretationOfArray, text, width, closeGapFromEndToStart, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
        }


        static UtilitiesDXXL_Bezier.FlexibleGetPosAtIndex<List<Transform>> GetPositions3DFromTransformsList_2D_preAllocated = UtilitiesDXXL_Bezier.GetPositions3DFromTransformsList_2D;
        static UtilitiesDXXL_Bezier.FlexibleGetDirectionControlPosOfTransform<List<Transform>> GetForwardControlPos3DFromTransformsList_2D_preAllocated = UtilitiesDXXL_Bezier.GetForwardControlPos3DFromTransformsList_2D;
        static UtilitiesDXXL_Bezier.FlexibleGetDirectionControlPosOfTransform<List<Transform>> GetBackwardControlPos3DFromTransformsList_2D_preAllocated = UtilitiesDXXL_Bezier.GetBackwardControlPos3DFromTransformsList_2D;
        public static void BezierSpline(List<Transform> points, Color color = default(Color), DrawBasics.BezierPosInterpretation interpretationOfList = DrawBasics.BezierPosInterpretation.onlySegmentStartPoints_backwardForwardIsAligned, string text = null, float width = 0.0f, bool closeGapFromEndToStart = false, int straightSubDivisionsPerSegment = 50, float custom_zPos = float.PositiveInfinity, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            UtilitiesDXXL_Bezier.BezierSpline<List<Transform>>(true, custom_zPos, points, GetPositions3DFromTransformsList_2D_preAllocated, GetForwardControlPos3DFromTransformsList_2D_preAllocated, GetBackwardControlPos3DFromTransformsList_2D_preAllocated, points.Count, color, interpretationOfList, text, width, closeGapFromEndToStart, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
        }


        static UtilitiesDXXL_Bezier.FlexibleGetPosAtIndex<Vector2[]> GetPositions3DFromVector2Array_2D_preAllocated = UtilitiesDXXL_Bezier.GetPositions3DFromVector2Array_2D;
        public static void BezierSpline(Vector2[] points, Color color = default(Color), DrawBasics.BezierPosInterpretation interpretationOfArray = DrawBasics.BezierPosInterpretation.start_control1_control2_endIsNextStart, string text = null, float width = 0.0f, bool closeGapFromEndToStart = false, int straightSubDivisionsPerSegment = 50, float custom_zPos = float.PositiveInfinity, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            if (interpretationOfArray == DrawBasics.BezierPosInterpretation.start_control1_control2_endIsNextStart || interpretationOfArray == DrawBasics.BezierPosInterpretation.start_control1_endIsNextStart)
            {
                UtilitiesDXXL_Bezier.BezierSpline<Vector2[]>(true, custom_zPos, points, GetPositions3DFromVector2Array_2D_preAllocated, null, null, points.Length, color, interpretationOfArray, text, width, closeGapFromEndToStart, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                Debug.LogError("The specified interpretationOfArray ('" + interpretationOfArray + "') is not supported for BezierSpline() function overloads that take 'Vector2'-collections. You may choose an overload that takes 'Transform'- or 'GameObject'-collections.");
            }
        }


        static UtilitiesDXXL_Bezier.FlexibleGetPosAtIndex<List<Vector2>> GetPositions3DFromVector2List_2D_preAllocated = UtilitiesDXXL_Bezier.GetPositions3DFromVector2List_2D;
        public static void BezierSpline(List<Vector2> points, Color color = default(Color), DrawBasics.BezierPosInterpretation interpretationOfList = DrawBasics.BezierPosInterpretation.start_control1_control2_endIsNextStart, string text = null, float width = 0.0f, bool closeGapFromEndToStart = false, int straightSubDivisionsPerSegment = 50, float custom_zPos = float.PositiveInfinity, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            if (interpretationOfList == DrawBasics.BezierPosInterpretation.start_control1_control2_endIsNextStart || interpretationOfList == DrawBasics.BezierPosInterpretation.start_control1_endIsNextStart)
            {
                UtilitiesDXXL_Bezier.BezierSpline<List<Vector2>>(true, custom_zPos, points, GetPositions3DFromVector2List_2D_preAllocated, null, null, points.Count, color, interpretationOfList, text, width, closeGapFromEndToStart, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                Debug.LogError("The specified interpretationOfList ('" + interpretationOfList + "') is not supported for BezierSpline() function overloads that take 'Vector2'-collections. You may choose an overload that takes 'Transform'- or 'GameObject'-collections.");
            }
        }

    }

}
