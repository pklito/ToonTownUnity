namespace DrawXXL
{
    using UnityEngine;

    public class UtilitiesDXXL_DrawBasics2D
    {

        public static InternalDXXL_Plane xyPlane_throughZero = new InternalDXXL_Plane(Vector3.zero, Vector3.forward);

        public static float TryFallbackToDefaultZ(float z_fromFunctionCallParameters)
        {
            if (float.IsNaN(z_fromFunctionCallParameters))
            {
                //Debug.LogWarning("'custom_zPos' is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(z_fromFunctionCallParameters) + " -> fallback to Draw2D.Default_zPos_forDrawing (" + DrawBasics2D.Default_zPos_forDrawing + ")");
                return DrawBasics2D.Default_zPos_forDrawing;
            }
            else
            {
                if (float.IsInfinity(z_fromFunctionCallParameters))
                {
                    return DrawBasics2D.Default_zPos_forDrawing;
                }
                else
                {
                    return z_fromFunctionCallParameters;
                }
            }
        }

        public static Quaternion QuaternionFromAngle(float angleDegCC)
        {
            if (float.IsNaN(angleDegCC) || float.IsInfinity(angleDegCC))
            {
                Debug.LogWarning("'angleDegCC' is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(angleDegCC) + " -> fallback to 'angleDegCC = 0'");
                return Quaternion.identity;
            }
            else
            {
                return ((UtilitiesDXXL_Math.ApproximatelyZero(angleDegCC)) ? Quaternion.identity : Quaternion.AngleAxis(angleDegCC, Vector3.forward));
            }
        }

        public static Vector3 Position_V2toV3(Vector2 vector2Position, float zPos)
        {
            return new Vector3(vector2Position.x, vector2Position.y, zPos);
        }

        public static Vector3 Direction_V2toV3(Vector2 vector2Direction)
        {
            return new Vector3(vector2Direction.x, vector2Direction.y, 0.0f);
        }

        public static void PointFallback(Vector3 position, string text, Color color, float markingCrossLinesWidth, float durationInSec, bool hiddenByNearerObjects)
        {
            UtilitiesDXXL_DrawBasics.Point(true, position, text, color, 0.5f, markingCrossLinesWidth, color, Quaternion.identity, true, true, false, true, Vector3.zero, Quaternion.identity, Vector3.one, true, durationInSec, hiddenByNearerObjects);
        }

        public static void PointFallback(Vector2 position, float zPos, string text, Color color, float markingCrossLinesWidth, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 positionV3 = new Vector3(position.x, position.y, zPos);
            UtilitiesDXXL_DrawBasics.Point(true, positionV3, text, color, 0.5f, markingCrossLinesWidth, color, Quaternion.identity, true, true, false, true, Vector3.zero, Quaternion.identity, Vector3.one, true, durationInSec, hiddenByNearerObjects);
        }

        public static void PointTag(Vector3 position, string text = null, Color color = default(Color), float linesWidth = 0.0f, float size_asTextOffsetDistance = 1.0f, Vector3 textOffsetDirection = default(Vector3), float textSizeScaleFactor = 1.0f, bool skipConeDrawing = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(size_asTextOffsetDistance, "size_asTextOffsetDistance")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(textSizeScaleFactor, "textSizeScaleFactor")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(textOffsetDirection, "textOffsetDirection")) { return; }

            //DO NOT fallback to "Point()" here, because "Point()" calls "PointTag()" again, which can create an endless loop.

            if (UtilitiesDXXL_Math.IsDefaultVector(textOffsetDirection)) { textOffsetDirection = new Vector3(DrawBasics.Default_textOffsetDirection_forPointTags.x, DrawBasics.Default_textOffsetDirection_forPointTags.y, 0.0f); }
            if (UtilitiesDXXL_Math.IsDefaultVector(textOffsetDirection)) { textOffsetDirection = UtilitiesDXXL_DrawBasics.default_default_textOffsetDirection_forPointTags; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);
            size_asTextOffsetDistance = UtilitiesDXXL_DrawBasics.GetClamped_pointTagSize_asTextOffsetDistance(size_asTextOffsetDistance, linesWidth);
            textSizeScaleFactor = Mathf.Abs(textSizeScaleFactor);
            textSizeScaleFactor = Mathf.Max(textSizeScaleFactor, 0.01f);

            Vector3 textOffsetDir_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(textOffsetDirection);
            Vector3 pos_to_startOfUnderLine = textOffsetDir_normalized * size_asTextOffsetDistance;
            float coneHeight = 0.2f * size_asTextOffsetDistance;
            coneHeight = Mathf.Max(coneHeight, 2.4f * linesWidth);
            Vector3 startOfTextUnderline = position + pos_to_startOfUnderLine;

            if (skipConeDrawing)
            {
                Line_fadeableAnimSpeed.InternalDraw(position, startOfTextUnderline, color, linesWidth, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, true, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }
            else
            {
                float offsetDistance_forStartAnchorOfLineToText = (3.0f * linesWidth);
                offsetDistance_forStartAnchorOfLineToText = Mathf.Min(offsetDistance_forStartAnchorOfLineToText, coneHeight);
                Vector3 offsettedStartAnchor_ofLineToText = position + offsetDistance_forStartAnchorOfLineToText * textOffsetDir_normalized;
                Line_fadeableAnimSpeed.InternalDraw(offsettedStartAnchor_ofLineToText, startOfTextUnderline, color, linesWidth, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, true, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            float dotProductResult_of_globalRight_and_textOffsetDir = Vector3.Dot(Vector3.right, textOffsetDir_normalized);
            float absDotProductResult_of_globalRight_and_textOffsetDir = Mathf.Abs(dotProductResult_of_globalRight_and_textOffsetDir);
            bool pointerIsApproxVert = (absDotProductResult_of_globalRight_and_textOffsetDir < UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.absDotProductResult_ofTwoApproxNormalizedVectors_belowWhichVectorsAreConsideredPerp);
            bool textIsOnLeftSideOfPoint = dotProductResult_of_globalRight_and_textOffsetDir < 0.0f;
            if (pointerIsApproxVert) { textIsOnLeftSideOfPoint = false; } //-> this prevents jitter in vert case (which is otherwise bistable due to float calculation imprecision)

            float textLength = 0.2f * size_asTextOffsetDistance;
            if (text != null && text != "")
            {
                float textSize = UtilitiesDXXL_DrawBasics.pointTagsTextSize_relToOffset * textSizeScaleFactor * size_asTextOffsetDistance;
                DrawText.TextAnchorDXXL textAnchor = textIsOnLeftSideOfPoint ? DrawText.TextAnchorDXXL.LowerRightOfFirstLine : DrawText.TextAnchorDXXL.LowerLeftOfFirstLine;
                UtilitiesDXXL_Text.Write(text, startOfTextUnderline + Vector3.up * (0.3f * textSize + 0.5f * linesWidth), color, textSize, Vector3.right, Vector3.up, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                float lengthOfLongestLine_inText = DrawText.parsedTextSpecs.widthOfLongestLine;
                textLength = Mathf.Max(textLength, lengthOfLongestLine_inText);
            }

            Vector3 lineEnd = textIsOnLeftSideOfPoint ? (startOfTextUnderline + Vector3.left * textLength) : (startOfTextUnderline + Vector3.right * textLength);
            Line_fadeableAnimSpeed.InternalDraw(startOfTextUnderline, lineEnd, color, linesWidth, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, true, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            if (skipConeDrawing == false)
            {
                float coneAngleDeg = 25.0f;
                Vector3 upVector_ofConeBaseRect = Vector3.forward;
                DrawShapes.ConeFilled(position, coneHeight, pos_to_startOfUnderLine, upVector_ofConeBaseRect, 0.0f, coneAngleDeg, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
            }
        }

    }

}
