namespace DrawXXL
{
    using UnityEngine;

    public class UtilitiesDXXL_LineCircled
    {
        static InternalDXXL_Plane plane_inWhichArbitraryTurnAxis_preferrablyLies = new InternalDXXL_Plane(Vector3.zero, new Vector3(1.129872f, 0.7129881f, 0.0f)); //=seldom plane which contains z-axis
        public static void LineCircled(Vector3 circleCenter, Vector3 circleCenter_to_start, Vector3 circleCenter_to_end, Color color, float forceRadius, float width, string text, bool useReflexAngleOver180deg, bool skipFallbackDisplayOfZeroAngles, bool flattenThickRoundLineIntoCirclePlane, float minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL textAnchor, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(forceRadius, "forceRadius")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenter, "circleCenter")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenter_to_start, "circleCenter_to_start")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenter_to_end, "circleCenter_to_end")) { return; }

            if (UtilitiesDXXL_Math.ApproximatelyZero(circleCenter_to_start))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(circleCenter, "[<color=#adadadFF><icon=logMessage></color> LineCircled with startVectorLength of 0]<br>" + text, color, width, durationInSec, hiddenByNearerObjects);
                return;
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(circleCenter_to_end))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(circleCenter, "[<color=#adadadFF><icon=logMessage></color> LineCircled with endVectorLength of 0]<br>" + text, color, width, durationInSec, hiddenByNearerObjects);
                return;
            }

            Vector3 circleCenter_to_start_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(circleCenter_to_start);
            Vector3 circleCenter_towards_end_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(circleCenter_to_end);
            float turnAngleDeg = Vector3.Angle(circleCenter_to_start_normalized, circleCenter_towards_end_normalized);
            Vector3 turnAxis = Vector3.Cross(circleCenter_to_start_normalized, circleCenter_towards_end_normalized);
            if (UtilitiesDXXL_Math.ApproximatelyZero(forceRadius) == false)
            {
                circleCenter_to_start = circleCenter_to_start_normalized * Mathf.Abs(forceRadius);
            }
            Vector3 startPos = circleCenter + circleCenter_to_start;

            if (UtilitiesDXXL_Math.GetBiggestAbsComponent(turnAxis) < 0.0001f)
            {
                // -> turnAngleDeg is "0°" or "180°"
                Vector3 arbitraryTurnAxis = UtilitiesDXXL_Math.Get_aNormalizedVector_perpToGivenVector(circleCenter_to_start, plane_inWhichArbitraryTurnAxis_preferrablyLies);
                if (arbitraryTurnAxis.z < 0.0f) { arbitraryTurnAxis = -arbitraryTurnAxis; } //-> 2D circledLines need turnAxis along positiveZ

                if (turnAngleDeg < 90.0f)
                {
                    LineCircled(startPos, circleCenter, arbitraryTurnAxis, 0.0f, color, width, "[<color=#adadadFF><icon=logMessage></color> LineCircled with 'toStart' and 'toEnd' vectors roughly along same direction (segment angle is 0°) => arbitrary turn axis]<br>" + text, skipFallbackDisplayOfZeroAngles, flattenThickRoundLineIntoCirclePlane, durationInSec, hiddenByNearerObjects, false, minAngleDeg_withoutTextLineBreak, textAnchor);
                }
                else
                {
                    LineCircled(startPos, circleCenter, arbitraryTurnAxis, 180.0f, color, width, "<size=3>[<color=#adadadFF><icon=logMessage></color> LineCircled with 'toStart' and 'toEnd' vectors roughly along opposite directions (segment angle is 180°) => arbitrary turn axis]</size><br>" + text, skipFallbackDisplayOfZeroAngles, flattenThickRoundLineIntoCirclePlane, durationInSec, hiddenByNearerObjects, false, minAngleDeg_withoutTextLineBreak, textAnchor);
                }
            }
            else
            {
                if (useReflexAngleOver180deg)
                {
                    Vector3 usedTurnAxis = -turnAxis;
                    float usedTurnAngleDeg = 360.0f - turnAngleDeg;
                    LineCircled(startPos, circleCenter, usedTurnAxis, usedTurnAngleDeg, color, width, text, skipFallbackDisplayOfZeroAngles, flattenThickRoundLineIntoCirclePlane, durationInSec, hiddenByNearerObjects, false, minAngleDeg_withoutTextLineBreak, textAnchor);
                }
                else
                {
                    LineCircled(startPos, circleCenter, turnAxis, turnAngleDeg, color, width, text, skipFallbackDisplayOfZeroAngles, flattenThickRoundLineIntoCirclePlane, durationInSec, hiddenByNearerObjects, false, minAngleDeg_withoutTextLineBreak, textAnchor);
                }
            }
        }

        static InternalDXXL_Line turnAxis_ofLineCircled = new InternalDXXL_Line();
        public static void LineCircled(Vector3 startPos, Vector3 turnAxis_origin, Vector3 turnAxis_direction, float turnAngleDegCC, Color color, float width, string text, bool skipFallbackDisplayOfZeroAngles, bool flattenThickRoundLineIntoCirclePlane, float durationInSec, bool hiddenByNearerObjects, bool skipTextMirrorInvertedFlipCheck, float minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL textAnchor, bool drawSeparateFullCircle_forAnglesBiggerThan360 = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(turnAngleDegCC, "turnAngleDegCC")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width, "width")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(minAngleDeg_withoutTextLineBreak, "minAngleDeg_withoutTextLineBreak")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(startPos, "startPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(turnAxis_origin, "turnAxis_origin")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(turnAxis_direction, "turnAxis_direction")) { return; }

            width = UtilitiesDXXL_Math.AbsNonZeroValue(width);

            if (UtilitiesDXXL_Math.ApproximatelyZero(turnAxis_direction))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(turnAxis_origin, "[<color=#adadadFF><icon=logMessage></color> LineCircled with turnAxis_direction_length of 0]<br>" + text, color, width, durationInSec, hiddenByNearerObjects);
                return;
            }

            turnAxis_ofLineCircled.Recreate(turnAxis_origin, turnAxis_direction, false);
            Vector3 circleCenter = turnAxis_ofLineCircled.Get_perpProjectionOfPoint_ontoThisLine(startPos);

            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(startPos, circleCenter))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(circleCenter, "[<color=#adadadFF><icon=logMessage></color> LineCircled with startPos on turnAxis]<br>" + text, color, width, durationInSec, hiddenByNearerObjects);
                return;
            }

            float unlooped_turnAngleDegCC = turnAngleDegCC;
            bool unloopedAngleIsOutside_m360_to_p360 = CheckIfAngleIsOutside_m360_to_p360(unlooped_turnAngleDegCC);
            if (drawSeparateFullCircle_forAnglesBiggerThan360) //-> endless regression loops are actually already prevented by "turnAngleDegCC_ofAdditional360degRing = 360.0f", but it's better to additionally protect against float calculation/comparison imprecsion herewith
            {
                if (unloopedAngleIsOutside_m360_to_p360)
                {
                    float turnAngleDegCC_ofAdditional360degRing = 360.0f;
                    string text_ofAdditional360degRing = null;
                    bool drawSeparateFullCircle_forAnglesBiggerThan360_onceMore = false;
                    LineCircled(startPos, turnAxis_origin, turnAxis_direction, turnAngleDegCC_ofAdditional360degRing, color, width, text_ofAdditional360degRing, true, flattenThickRoundLineIntoCirclePlane, durationInSec, hiddenByNearerObjects, skipTextMirrorInvertedFlipCheck, minAngleDeg_withoutTextLineBreak, textAnchor, drawSeparateFullCircle_forAnglesBiggerThan360_onceMore);
                }
            }

            turnAngleDegCC = LoopAngleIntoSpanFrom_m360_to_p360(unlooped_turnAngleDegCC);
            Vector3 circleCenter_to_start = startPos - circleCenter;
            if (UtilitiesDXXL_Math.ApproximatelyZero(turnAngleDegCC))
            {
                if (unloopedAngleIsOutside_m360_to_p360)
                {
                    //-> multiples of 360° arrive here, like +/-720° or +/-1080°
                    turnAngleDegCC = 359.99f * Mathf.Sign(unlooped_turnAngleDegCC);
                }
                else
                {
                    if (skipFallbackDisplayOfZeroAngles == false)
                    {
                        UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                        DrawBasics.VectorFrom(circleCenter, circleCenter_to_start, color, width, "[<color=#adadadFF><icon=logMessage></color> LineCircled with angle of 0°]<br>" + text, 0.17f, false, false, default(Vector3), false, 0.02f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                        DrawBasics.VectorFrom(circleCenter, turnAxis_ofLineCircled.direction_normalized, UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.6f), 0.0f, "[turnAxis]", 0.17f, false, false, default(Vector3), false, 0.02f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                        UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                    }
                    return;
                }
            }

            Vector3 circleCenter_towards_end_normalized = circleCenter_to_start; //-> silencing the compiler who seems to not realize that it will always get filled via the out-parameters before it is used
            float usedRadius;
            if (turnAngleDegCC >= 180.0f)
            {
                usedRadius = LineCircledBelow180Deg(circleCenter, circleCenter_to_start, out circleCenter_towards_end_normalized, 90.0f, turnAxis_ofLineCircled.direction_normalized, color, 0.0f, width, flattenThickRoundLineIntoCirclePlane, durationInSec, hiddenByNearerObjects);
                LineCircledBelow180Deg(circleCenter, -circleCenter_to_start, out circleCenter_towards_end_normalized, -90.0f, turnAxis_ofLineCircled.direction_normalized, color, 0.0f, width, flattenThickRoundLineIntoCirclePlane, durationInSec, hiddenByNearerObjects);
                LineCircledBelow180Deg(circleCenter, -circleCenter_to_start, out circleCenter_towards_end_normalized, turnAngleDegCC - 180.0f, turnAxis_ofLineCircled.direction_normalized, color, 0.0f, width, flattenThickRoundLineIntoCirclePlane, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                if (turnAngleDegCC <= (-180.0f))
                {
                    usedRadius = LineCircledBelow180Deg(circleCenter, circleCenter_to_start, out circleCenter_towards_end_normalized, -90.0f, turnAxis_ofLineCircled.direction_normalized, color, 0.0f, width, flattenThickRoundLineIntoCirclePlane, durationInSec, hiddenByNearerObjects);
                    LineCircledBelow180Deg(circleCenter, -circleCenter_to_start, out circleCenter_towards_end_normalized, 90.0f, turnAxis_ofLineCircled.direction_normalized, color, 0.0f, width, flattenThickRoundLineIntoCirclePlane, durationInSec, hiddenByNearerObjects);
                    LineCircledBelow180Deg(circleCenter, -circleCenter_to_start, out circleCenter_towards_end_normalized, turnAngleDegCC + 180.0f, turnAxis_ofLineCircled.direction_normalized, color, 0.0f, width, flattenThickRoundLineIntoCirclePlane, durationInSec, hiddenByNearerObjects);
                }
                else
                {
                    usedRadius = LineCircledBelow180Deg(circleCenter, circleCenter_to_start, out circleCenter_towards_end_normalized, turnAngleDegCC, turnAxis_ofLineCircled.direction_normalized, color, 0.0f, width, flattenThickRoundLineIntoCirclePlane, durationInSec, hiddenByNearerObjects);
                }
            }

            TryDraw_textAtLineCircled(color, width, text, usedRadius, circleCenter, turnAxis_direction, circleCenter_to_start, circleCenter_towards_end_normalized, turnAngleDegCC, minAngleDeg_withoutTextLineBreak, textAnchor, skipTextMirrorInvertedFlipCheck, durationInSec, hiddenByNearerObjects);
        }

        public static void CircleSegment(Vector3 circleCenter, Vector3 circleCenter_to_startPosOnPerimeter, Vector3 circleCenter_to_endPosOnPerimeter, Color color, float forceRadius, float fillDensity, string text, bool useReflexAngleOver180deg, float radiusPortionWhereDrawFillStarts, bool skipFallbackDisplayOfZeroAngles, float minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL textAnchor, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(forceRadius, "forceRadius")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenter, "circleCenter")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenter_to_startPosOnPerimeter, "circleCenter_to_startPosOnPerimeter")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenter_to_endPosOnPerimeter, "circleCenter_to_endPosOnPerimeter")) { return; }

            if (UtilitiesDXXL_Math.ApproximatelyZero(circleCenter_to_startPosOnPerimeter))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(circleCenter, "[<color=#adadadFF><icon=logMessage></color> CircleSegment with circleCenter_to_startPosOnPerimeter-length of 0]<br>" + text, color, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(circleCenter_to_endPosOnPerimeter))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(circleCenter, "[<color=#adadadFF><icon=logMessage></color> CircleSegment with circleCenter_to_endPosOnPerimeter-length of 0]<br>" + text, color, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            Vector3 circleCenter_towards_startPosOnPerimeter_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(circleCenter_to_startPosOnPerimeter);
            Vector3 circleCenter_towards_endPosOnPerimeter_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(circleCenter_to_endPosOnPerimeter);
            float turnAngleDeg = Vector3.Angle(circleCenter_towards_startPosOnPerimeter_normalized, circleCenter_towards_endPosOnPerimeter_normalized);
            Vector3 turnAxis = Vector3.Cross(circleCenter_towards_startPosOnPerimeter_normalized, circleCenter_towards_endPosOnPerimeter_normalized);
            if (UtilitiesDXXL_Math.ApproximatelyZero(forceRadius) == false)
            {
                circleCenter_to_startPosOnPerimeter = circleCenter_towards_startPosOnPerimeter_normalized * Mathf.Abs(forceRadius);
            }
            Vector3 startPosOnPerimeter = circleCenter + circleCenter_to_startPosOnPerimeter;

            if (UtilitiesDXXL_Math.GetBiggestAbsComponent(turnAxis) < 0.0001f)
            {
                // -> turnAngleDeg is "0°" or "180°"
                Vector3 arbitraryTurnAxis = UtilitiesDXXL_Math.Get_aNormalizedVector_perpToGivenVector(circleCenter_to_startPosOnPerimeter, plane_inWhichArbitraryTurnAxis_preferrablyLies);
                if (arbitraryTurnAxis.z < 0.0f) { arbitraryTurnAxis = -arbitraryTurnAxis; } //-> 2D circledLines need turnAxis along positiveZ

                if (turnAngleDeg < 90.0f)
                {
                    CircleSegment(startPosOnPerimeter, circleCenter, arbitraryTurnAxis, 0.0f, color, "[<color=#adadadFF><icon=logMessage></color> CircleSegment with 'toStart' and 'toEnd' vectors roughly along same direction (segment angle is 0°) => arbitrary turn axis]<br>" + text, radiusPortionWhereDrawFillStarts, skipFallbackDisplayOfZeroAngles, fillDensity, durationInSec, hiddenByNearerObjects, false, minAngleDeg_withoutTextLineBreak, textAnchor);
                }
                else
                {
                    CircleSegment(startPosOnPerimeter, circleCenter, arbitraryTurnAxis, 180.0f, color, "<size=3>[<color=#adadadFF><icon=logMessage></color> CircleSegment with 'toStart' and 'toEnd' vectors roughly along opposite directions (segment angle is 180°) => arbitrary turn axis]</size><br>" + text, radiusPortionWhereDrawFillStarts, skipFallbackDisplayOfZeroAngles, fillDensity, durationInSec, hiddenByNearerObjects, false, minAngleDeg_withoutTextLineBreak, textAnchor);
                }
            }
            else
            {
                if (useReflexAngleOver180deg)
                {
                    Vector3 usedTurnAxis = -turnAxis;
                    float usedTurnAngleDeg = 360.0f - turnAngleDeg;
                    CircleSegment(startPosOnPerimeter, circleCenter, usedTurnAxis, usedTurnAngleDeg, color, text, radiusPortionWhereDrawFillStarts, skipFallbackDisplayOfZeroAngles, fillDensity, durationInSec, hiddenByNearerObjects, false, minAngleDeg_withoutTextLineBreak, textAnchor);
                }
                else
                {
                    CircleSegment(startPosOnPerimeter, circleCenter, turnAxis, turnAngleDeg, color, text, radiusPortionWhereDrawFillStarts, skipFallbackDisplayOfZeroAngles, fillDensity, durationInSec, hiddenByNearerObjects, false, minAngleDeg_withoutTextLineBreak, textAnchor);
                }
            }
        }

        public static void CircleSegment(Vector3 startPosOnPerimeter, Vector3 centerOfCircle, Vector3 normalOfCircle, float turnAngleDegCC, Color color, string text, float radiusPortionWhereDrawFillStarts, bool skipFallbackDisplayOfZeroAngles, float fillDensity, float durationInSec, bool hiddenByNearerObjects, bool skipTextMirrorInvertedFlipCheck, float minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL textAnchor, bool drawSeparateFullCircle_forAnglesBiggerThan360 = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(turnAngleDegCC, "turnAngleDegCC")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radiusPortionWhereDrawFillStarts, "radiusPortionWhereDrawFillStarts")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(fillDensity, "fillDensity")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(minAngleDeg_withoutTextLineBreak, "minAngleDeg_withoutTextLineBreak")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(startPosOnPerimeter, "startPosOnPerimeter")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(centerOfCircle, "centerOfCircle")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(normalOfCircle, "normalOfCircle")) { return; }

            if (radiusPortionWhereDrawFillStarts >= 1.0f)
            {
                LineCircled(startPosOnPerimeter, centerOfCircle, normalOfCircle, turnAngleDegCC, color, 0.0f, text, skipFallbackDisplayOfZeroAngles, false, durationInSec, hiddenByNearerObjects, skipTextMirrorInvertedFlipCheck, minAngleDeg_withoutTextLineBreak, textAnchor, drawSeparateFullCircle_forAnglesBiggerThan360);
                return;
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(normalOfCircle))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(centerOfCircle, "[<color=#adadadFF><icon=logMessage></color> CircleSegment with normalOfCircle-length of 0]<br>" + text, color, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            turnAxis_ofLineCircled.Recreate(centerOfCircle, normalOfCircle, false);
            Vector3 circleCenter = turnAxis_ofLineCircled.Get_perpProjectionOfPoint_ontoThisLine(startPosOnPerimeter);

            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(startPosOnPerimeter, circleCenter))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(circleCenter, "[<color=#adadadFF><icon=logMessage></color> CircleSegment with perimeter start position on circle center]<br>" + text, color, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            float unlooped_turnAngleDegCC = turnAngleDegCC;
            bool unloopedAngleIsOutside_m360_to_p360 = CheckIfAngleIsOutside_m360_to_p360(unlooped_turnAngleDegCC);
            bool applyOffsetForRadialLinesToIndicateAnglesOver360deg = false;
            if (drawSeparateFullCircle_forAnglesBiggerThan360) //-> endless regression loops are actually already prevented by "turnAngleDegCC_ofAdditional360degRing = 360.0f", but it's better to additionally protect against float calculation/comparison imprecsion herewith
            {
                if (unloopedAngleIsOutside_m360_to_p360)
                {
                    float turnAngleDegCC_ofAdditional360degRing = 360.0f;
                    string text_ofAdditional360degRing = null;
                    bool drawSeparateFullCircle_forAnglesBiggerThan360_onceMore = false;
                    CircleSegment(startPosOnPerimeter, centerOfCircle, normalOfCircle, turnAngleDegCC_ofAdditional360degRing, color, text_ofAdditional360degRing, radiusPortionWhereDrawFillStarts, true, fillDensity, durationInSec, hiddenByNearerObjects, skipTextMirrorInvertedFlipCheck, minAngleDeg_withoutTextLineBreak, textAnchor, drawSeparateFullCircle_forAnglesBiggerThan360_onceMore);
                    applyOffsetForRadialLinesToIndicateAnglesOver360deg = true;
                }
            }

            turnAngleDegCC = LoopAngleIntoSpanFrom_m360_to_p360(unlooped_turnAngleDegCC);
            Vector3 circleCenter_to_startPosOnPerimeter = startPosOnPerimeter - circleCenter;
            if (UtilitiesDXXL_Math.ApproximatelyZero(turnAngleDegCC))
            {
                if (unloopedAngleIsOutside_m360_to_p360)
                {
                    //-> multiples of 360° arrive here, like +/-720° or +/-1080°
                    turnAngleDegCC = 359.99f * Mathf.Sign(unlooped_turnAngleDegCC);
                }
                else
                {
                    if (skipFallbackDisplayOfZeroAngles == false)
                    {
                        UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                        DrawBasics.VectorFrom(circleCenter, circleCenter_to_startPosOnPerimeter, color, 0.0f, "[<color=#adadadFF><icon=logMessage></color> CircleSegment with angle of 0°]<br>" + text, 0.17f, false, false, default(Vector3), false, 0.02f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                        DrawBasics.VectorFrom(circleCenter, turnAxis_ofLineCircled.direction_normalized, UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.6f), 0.0f, "[normal of circle]", 0.17f, false, false, default(Vector3), false, 0.02f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                        UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                    }
                    return;
                }
            }

            Vector3 circleCenter_towards_endPosOnPerimeter_normalized = circleCenter_to_startPosOnPerimeter; //-> silencing the compiler who seems to not realize that it will always get filled via the out-parameters before it is used
            float radiusOfRoundOuterBoundaryLine;
            if (turnAngleDegCC >= 180.0f)
            {
                radiusOfRoundOuterBoundaryLine = CircleSegmentBelow180Deg(applyOffsetForRadialLinesToIndicateAnglesOver360deg, true, true, circleCenter, circleCenter_to_startPosOnPerimeter, out circleCenter_towards_endPosOnPerimeter_normalized, 90.0f, turnAxis_ofLineCircled.direction_normalized, radiusPortionWhereDrawFillStarts, color, 0.0f, fillDensity, durationInSec, hiddenByNearerObjects);
                CircleSegmentBelow180Deg(applyOffsetForRadialLinesToIndicateAnglesOver360deg, true, false, circleCenter, -circleCenter_to_startPosOnPerimeter, out circleCenter_towards_endPosOnPerimeter_normalized, -90.0f, turnAxis_ofLineCircled.direction_normalized, radiusPortionWhereDrawFillStarts, color, 0.0f, fillDensity, durationInSec, hiddenByNearerObjects);
                CircleSegmentBelow180Deg(applyOffsetForRadialLinesToIndicateAnglesOver360deg, false, true, circleCenter, -circleCenter_to_startPosOnPerimeter, out circleCenter_towards_endPosOnPerimeter_normalized, turnAngleDegCC - 180.0f, turnAxis_ofLineCircled.direction_normalized, radiusPortionWhereDrawFillStarts, color, 0.0f, fillDensity, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                if (turnAngleDegCC <= (-180.0f))
                {
                    radiusOfRoundOuterBoundaryLine = CircleSegmentBelow180Deg(applyOffsetForRadialLinesToIndicateAnglesOver360deg, true, true, circleCenter, circleCenter_to_startPosOnPerimeter, out circleCenter_towards_endPosOnPerimeter_normalized, -90.0f, turnAxis_ofLineCircled.direction_normalized, radiusPortionWhereDrawFillStarts, color, 0.0f, fillDensity, durationInSec, hiddenByNearerObjects);
                    CircleSegmentBelow180Deg(applyOffsetForRadialLinesToIndicateAnglesOver360deg, true, false, circleCenter, -circleCenter_to_startPosOnPerimeter, out circleCenter_towards_endPosOnPerimeter_normalized, 90.0f, turnAxis_ofLineCircled.direction_normalized, radiusPortionWhereDrawFillStarts, color, 0.0f, fillDensity, durationInSec, hiddenByNearerObjects);
                    CircleSegmentBelow180Deg(applyOffsetForRadialLinesToIndicateAnglesOver360deg, false, true, circleCenter, -circleCenter_to_startPosOnPerimeter, out circleCenter_towards_endPosOnPerimeter_normalized, turnAngleDegCC + 180.0f, turnAxis_ofLineCircled.direction_normalized, radiusPortionWhereDrawFillStarts, color, 0.0f, fillDensity, durationInSec, hiddenByNearerObjects);
                }
                else
                {
                    radiusOfRoundOuterBoundaryLine = CircleSegmentBelow180Deg(applyOffsetForRadialLinesToIndicateAnglesOver360deg, true, true, circleCenter, circleCenter_to_startPosOnPerimeter, out circleCenter_towards_endPosOnPerimeter_normalized, turnAngleDegCC, turnAxis_ofLineCircled.direction_normalized, radiusPortionWhereDrawFillStarts, color, 0.0f, fillDensity, durationInSec, hiddenByNearerObjects);
                }
            }

            TryDraw_textAtLineCircled(color, 0.0f, text, radiusOfRoundOuterBoundaryLine, circleCenter, normalOfCircle, circleCenter_to_startPosOnPerimeter, circleCenter_towards_endPosOnPerimeter_normalized, turnAngleDegCC, minAngleDeg_withoutTextLineBreak, textAnchor, skipTextMirrorInvertedFlipCheck, durationInSec, hiddenByNearerObjects);
        }

        static void TryDraw_textAtLineCircled(Color color, float widthOfOuterBoundaryLine, string text, float radiusOfRoundOuterBoundaryLine, Vector3 circleCenter, Vector3 turnAxis_direction, Vector3 circleCenter_to_start, Vector3 circleCenter_towards_endPosOnPerimeter_normalized, float turnAngleDegCC, float minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL textAnchor, bool skipTextMirrorInvertedFlipCheck, float durationInSec, bool hiddenByNearerObjects)
        {
            if (text != null && text != "")
            {
                float textRadius = 1.05f * radiusOfRoundOuterBoundaryLine + 0.5f * widthOfOuterBoundaryLine;
                float textSize = 0.1f * radiusOfRoundOuterBoundaryLine;
                GetTextsInitialDirAndUpVectors(out Vector3 textsInitialDir, out Vector3 textsInitialUp, turnAxis_direction, turnAngleDegCC, circleCenter, circleCenter_to_start, circleCenter_towards_endPosOnPerimeter_normalized, skipTextMirrorInvertedFlipCheck);
                float autoLineBreakAngleDeg = Mathf.Abs(turnAngleDegCC);
                if (minAngleDeg_withoutTextLineBreak > 0.0f)
                {
                    autoLineBreakAngleDeg = Mathf.Max(autoLineBreakAngleDeg, minAngleDeg_withoutTextLineBreak);
                }

                //"autoFlipToPreventMirrorInverted" is disabled here, because:
                //-> The problem is already solved by "GetTextsInitialDirAndUpVectors()". The solutions may be exchangeable. The difference is: The solution in "GetTextsInitialDirAndUpVectors" mounts the flippedText at the lineEnd, while the solution via "autoFlipToPreventMirrorInverted" mounts the flippedText text at lineStart.
                //-> Advantage of the "GetTextsInitialDirAndUpVectors()"-solution: For circledLines that span only a small angle: The text "starts" at the line and the textEnd prodrudes over the line end, instead of the text starting somewhere away from the line and the "ends" at the line.
                //-> Disadvantage of the "GetTextsInitialDirAndUpVectors()"-solution: For circledLines that have a bigger angle span: If the text gets autoFlipped then it is mounted at the lineEnd and therefore may confuse the human observer, wheather there is the lineStart or lineEnd.
                bool autoFlipToPreventMirrorInverted = false;
                UtilitiesDXXL_Text.WriteOnCircle(text, circleCenter, textRadius, color, textSize, textsInitialDir, textsInitialUp, textAnchor, autoLineBreakAngleDeg, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, false, false, false);
            }
        }

        static void GetTextsInitialDirAndUpVectors(out Vector3 textsInitialDir, out Vector3 textsInitialUp, Vector3 turnAxis_direction, float turnAngleDegCC, Vector3 circleCenter, Vector3 circleCenter_to_start, Vector3 circleCenter_towards_end_normalized, bool skipTextMirrorInvertedFlipCheck)
        {
            if (skipTextMirrorInvertedFlipCheck)
            {
                textsInitialUp = circleCenter_to_start;
                textsInitialDir = Vector3.Cross(textsInitialUp, turnAxis_direction);
            }
            else
            {
                Vector3 turnAxis_flippedSoThatLookingAlongThisAlwaysResultsInClockwiseRotation;
                if (turnAngleDegCC > 0.0f)
                {
                    //counter clockwise:
                    turnAxis_flippedSoThatLookingAlongThisAlwaysResultsInClockwiseRotation = (-turnAxis_direction);
                }
                else
                {
                    //clockwise:
                    turnAxis_flippedSoThatLookingAlongThisAlwaysResultsInClockwiseRotation = turnAxis_direction;
                }

                UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out Vector3 observerCamForward_normalized, out Vector3 observerCamUp_normalized, out Vector3 observerCamRight_normalized, out Vector3 cam_to_lineCenter, circleCenter, Vector3.zero, null);
                bool textFromCircleStart_wouldResultInMirrorInvertedText = UtilitiesDXXL_Math.Check_ifVectorsPointAwayFromEachOther_perpCountsAsPointingInSameDir(turnAxis_flippedSoThatLookingAlongThisAlwaysResultsInClockwiseRotation, cam_to_lineCenter);
                if (textFromCircleStart_wouldResultInMirrorInvertedText)
                {
                    textsInitialUp = circleCenter_towards_end_normalized;
                    textsInitialDir = Vector3.Cross(textsInitialUp, (-turnAxis_flippedSoThatLookingAlongThisAlwaysResultsInClockwiseRotation));
                }
                else
                {
                    textsInitialUp = circleCenter_to_start;
                    textsInitialDir = Vector3.Cross(textsInitialUp, turnAxis_flippedSoThatLookingAlongThisAlwaysResultsInClockwiseRotation);
                }
            }
        }

        public static void VectorCircled(Vector3 circleCenter, Vector3 circleCenter_to_start, Vector3 circleCenter_to_end, Color color, float forceRadius, float lineWidth, string text, bool useReflexAngleOver180deg, float coneLength, bool skipFallbackDisplayOfZeroAngles, bool pointerAtBothSides, bool flattenThickRoundLineIntoCirclePlane, float minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL textAnchor, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(forceRadius, "forceRadius")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenter, "circleCenter")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenter_to_start, "circleCenter_to_start")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenter_to_end, "circleCenter_to_end")) { return; }

            if (UtilitiesDXXL_Math.ApproximatelyZero(circleCenter_to_start))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(circleCenter, "[<color=#adadadFF><icon=logMessage></color> VectorCircled with startVectorLength of 0]<br>" + text, color, lineWidth, durationInSec, hiddenByNearerObjects);
                return;
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(circleCenter_to_end))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(circleCenter, "[<color=#adadadFF><icon=logMessage></color> VectorCircled with endVectorLength of 0]<br>" + text, color, lineWidth, durationInSec, hiddenByNearerObjects);
                return;
            }

            Vector3 circleCenter_to_start_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(circleCenter_to_start);
            Vector3 circleCenter_towards_end_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(circleCenter_to_end);
            float turnAngleDeg = Vector3.Angle(circleCenter_to_start_normalized, circleCenter_towards_end_normalized);
            Vector3 turnAxis = Vector3.Cross(circleCenter_to_start_normalized, circleCenter_towards_end_normalized);
            if (UtilitiesDXXL_Math.ApproximatelyZero(forceRadius) == false)
            {
                circleCenter_to_start = circleCenter_to_start_normalized * Mathf.Abs(forceRadius);
            }

            if (UtilitiesDXXL_Math.GetBiggestAbsComponent(turnAxis) < 0.0001f)
            {
                // -> turnAngleDeg is "0°" or "180°"
                Vector3 arbitraryTurnAxis = UtilitiesDXXL_Math.Get_aNormalizedVector_perpToGivenVector(circleCenter_to_start, plane_inWhichArbitraryTurnAxis_preferrablyLies);
                if (arbitraryTurnAxis.z < 0.0f) { arbitraryTurnAxis = -arbitraryTurnAxis; } //-> 2D circledLines need turnAxis along positiveZ

                if (turnAngleDeg < 90.0f)
                {
                    VectorCircled(circleCenter + circleCenter_to_start, circleCenter, arbitraryTurnAxis, 0.0f, color, lineWidth, "[<color=#adadadFF><icon=logMessage></color> VectorCircled with vectors roughly along same direction (covered angle is 0°) => arbitrary turn axis]<br>" + text, coneLength, skipFallbackDisplayOfZeroAngles, pointerAtBothSides, flattenThickRoundLineIntoCirclePlane, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, hiddenByNearerObjects, 1.0f);
                }
                else
                {
                    VectorCircled(circleCenter + circleCenter_to_start, circleCenter, arbitraryTurnAxis, 180.0f, color, lineWidth, "<size=3>[<color=#adadadFF><icon=logMessage></color> VectorCircled with vectors roughly along opposite directions (covered angle is 180°) => arbitrary turn axis]</size><br>" + text, coneLength, skipFallbackDisplayOfZeroAngles, pointerAtBothSides, flattenThickRoundLineIntoCirclePlane, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, hiddenByNearerObjects, 1.0f);
                }
            }
            else
            {
                if (useReflexAngleOver180deg)
                {
                    Vector3 usedTurnAxis = -turnAxis;
                    float usedTurnAngleDeg = 360.0f - turnAngleDeg;
                    VectorCircled(circleCenter + circleCenter_to_start, circleCenter, usedTurnAxis, usedTurnAngleDeg, color, lineWidth, text, coneLength, skipFallbackDisplayOfZeroAngles, pointerAtBothSides, flattenThickRoundLineIntoCirclePlane, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, hiddenByNearerObjects, 1.0f);
                }
                else
                {
                    VectorCircled(circleCenter + circleCenter_to_start, circleCenter, turnAxis, turnAngleDeg, color, lineWidth, text, coneLength, skipFallbackDisplayOfZeroAngles, pointerAtBothSides, flattenThickRoundLineIntoCirclePlane, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, hiddenByNearerObjects, 1.0f);
                }
            }
        }

        static InternalDXXL_Line turnAxis_ofVectorCircled = new InternalDXXL_Line();
        public static void VectorCircled(Vector3 startPos, Vector3 turnAxis_origin, Vector3 turnAxis_direction, float turnAngleDegCC, Color color, float lineWidth, string text, float coneLength, bool skipFallbackDisplayOfZeroAngles, bool pointerAtBothSides, bool flattenThickRoundLineIntoCirclePlane, float minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL textAnchor, float durationInSec, bool hiddenByNearerObjects, float alphaFactorForPointers)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(turnAngleDegCC, "turnAngleDegCC")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lineWidth, "lineWidth")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(coneLength, "coneLength")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(startPos, "startPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(turnAxis_origin, "turnAxis_origin")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(turnAxis_direction, "turnAxis_direction")) { return; }

            if (UtilitiesDXXL_Math.ApproximatelyZero(turnAxis_direction))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(turnAxis_origin, "[<color=#adadadFF><icon=logMessage></color> VectorCircled with turnAxis_direction_length of 0]<br>" + text, color, lineWidth, durationInSec, hiddenByNearerObjects);
                return;
            }

            turnAxis_ofVectorCircled.Recreate(turnAxis_origin, turnAxis_direction, false);
            Vector3 circleCenter = turnAxis_ofVectorCircled.Get_perpProjectionOfPoint_ontoThisLine(startPos);

            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(startPos, circleCenter))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(circleCenter, "[<color=#adadadFF><icon=logMessage></color> VectorCircled with startPos on turnAxis]<br>" + text, color, lineWidth, durationInSec, hiddenByNearerObjects);
                return;
            }

            float unlooped_turnAngleDegCC = turnAngleDegCC;
            bool unloopedAngleIsOutside_m360_to_p360 = CheckIfAngleIsOutside_m360_to_p360(unlooped_turnAngleDegCC);
            if (unloopedAngleIsOutside_m360_to_p360)
            {
                float turnAngleDegCC_ofAdditional360degRing = 360.0f;
                string text_ofAdditional360degRing = null;
                bool drawSeparateFullCircle_forAnglesBiggerThan360_onceMore = false;
                LineCircled(startPos, circleCenter, turnAxis_ofVectorCircled.direction_normalized, turnAngleDegCC_ofAdditional360degRing, color, lineWidth, text_ofAdditional360degRing, true, flattenThickRoundLineIntoCirclePlane, durationInSec, hiddenByNearerObjects, true, minAngleDeg_withoutTextLineBreak, textAnchor, drawSeparateFullCircle_forAnglesBiggerThan360_onceMore);
            }

            Vector3 circleCenter_to_start = startPos - circleCenter;
            turnAngleDegCC = LoopAngleIntoSpanFrom_m360_to_p360(turnAngleDegCC);
            if (UtilitiesDXXL_Math.ApproximatelyZero(turnAngleDegCC))
            {
                if (unloopedAngleIsOutside_m360_to_p360)
                {
                    //-> multiples of 360° arrive here, like +/-720° or +/-1080°
                    turnAngleDegCC = 359.99f * Mathf.Sign(unlooped_turnAngleDegCC);
                }
                else
                {
                    if (skipFallbackDisplayOfZeroAngles == false)
                    {
                        UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                        DrawBasics.VectorFrom(circleCenter, circleCenter_to_start, color, lineWidth, "[<color=#adadadFF><icon=logMessage></color> VectorCircled with angle of 0°]<br>" + text, 0.17f, false, false, default(Vector3), false, 0.02f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                        DrawBasics.VectorFrom(circleCenter, turnAxis_ofVectorCircled.direction_normalized, UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.6f), 0.0f, "[turnAxis]", 0.17f, false, false, default(Vector3), false, 0.02f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                        UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                    }
                    return;
                }
            }

            lineWidth = UtilitiesDXXL_Math.AbsNonZeroValue(lineWidth);
            if (lineWidth < DrawBasics.thinestPossibleNonZeroWidthLine) { lineWidth = 0.0f; }
            bool isThinLine = UtilitiesDXXL_Math.ApproximatelyZero(lineWidth);
            float absTurnAngleDeg = Mathf.Abs(turnAngleDegCC);
            float turnAngleSign = Mathf.Sign(turnAngleDegCC);
            float radius = circleCenter_to_start.magnitude;
            float perimeterOf360deg = 2.0f * radius * Mathf.PI;

            if (DrawBasics.coneLength_interpretation_forCircledVectors == DrawBasics.LengthInterpretation.relativeToLineLength) { coneLength = radius * coneLength; }
            coneLength = Mathf.Abs(coneLength);
            float minConeLength = (0.01f * absTurnAngleDeg / 360.0f) * perimeterOf360deg;
            float maxConeLength = (0.45f * absTurnAngleDeg / 360.0f) * perimeterOf360deg;
            float maxConeLength_asAngleDeg = 45.0f;
            float maxConeLength_fromAngleLimit = (maxConeLength_asAngleDeg / 360.0f) * perimeterOf360deg;
            maxConeLength = Mathf.Min(maxConeLength, maxConeLength_fromAngleLimit);
            coneLength = Mathf.Clamp(coneLength, minConeLength, maxConeLength);
            float coneLength_asAngleDeg = 360.0f * (coneLength / perimeterOf360deg);

            float coneAngleDeg = 25.0f;
            if (isThinLine == false)
            {
                float coneSize_to_lineWidth_scaler = 1.0f;
                float minConeAngleDeg = 2.0f * Mathf.Rad2Deg * Mathf.Atan(coneSize_to_lineWidth_scaler * lineWidth / coneLength);
                coneAngleDeg = Mathf.Max(coneAngleDeg, minConeAngleDeg);
            }

            float shorteningOf_straightLineInsideCone = 0.0f;
            if (isThinLine == false)
            {
                shorteningOf_straightLineInsideCone = (0.5f * lineWidth) / Mathf.Tan(Mathf.Deg2Rad * 0.5f * coneAngleDeg);
                shorteningOf_straightLineInsideCone = 2.0f * shorteningOf_straightLineInsideCone; //solves: curvedLine is not parallel/aligned to coneDir and therefore intersects the coneSurface
                shorteningOf_straightLineInsideCone = Mathf.Min(shorteningOf_straightLineInsideCone, 0.85f * coneLength);
            }
            float shorteningOf_straightLineInsideCone_asAngleDeg = 360.0f * (shorteningOf_straightLineInsideCone / perimeterOf360deg);

            float turnAngleDeg_ofUnconedPart = turnAngleDegCC - (turnAngleSign * coneLength_asAngleDeg);
            Vector3 circleCenter_to_startOfUnconedPart = circleCenter_to_start;
            if (pointerAtBothSides)
            {
                turnAngleDeg_ofUnconedPart = turnAngleDegCC - 2.0f * (turnAngleSign * coneLength_asAngleDeg);
                Quaternion rotationToStartOfUnconedPart = Quaternion.AngleAxis(turnAngleSign * coneLength_asAngleDeg, turnAxis_ofVectorCircled.direction_normalized);
                circleCenter_to_startOfUnconedPart = rotationToStartOfUnconedPart * circleCenter_to_start;
            }

            Vector3 startOfUnconedPart = circleCenter + circleCenter_to_startOfUnconedPart;
            LineCircled(startOfUnconedPart, circleCenter, turnAxis_ofVectorCircled.direction_normalized, turnAngleDeg_ofUnconedPart, color, lineWidth, text, skipFallbackDisplayOfZeroAngles, flattenThickRoundLineIntoCirclePlane, durationInSec, hiddenByNearerObjects, false, minAngleDeg_withoutTextLineBreak, textAnchor);

            Quaternion rotationToEndOfUnconedPart = Quaternion.AngleAxis(turnAngleDegCC - (turnAngleSign * coneLength_asAngleDeg), turnAxis_ofVectorCircled.direction_normalized);
            Vector3 circleCenter_to_endOfUnconedPart = rotationToEndOfUnconedPart * circleCenter_to_start;
            Quaternion rotationToEndOfDrawnCircleLineInsideEndCone = Quaternion.AngleAxis(turnAngleDegCC - (turnAngleSign * shorteningOf_straightLineInsideCone_asAngleDeg), turnAxis_ofVectorCircled.direction_normalized);
            Vector3 circleCenter_to_endOfDrawnCircleLineInsideEndCone = rotationToEndOfDrawnCircleLineInsideEndCone * circleCenter_to_start;
            Quaternion rotationToStartOfDrawnCircleLineInsideStartCone = Quaternion.AngleAxis(turnAngleSign * shorteningOf_straightLineInsideCone_asAngleDeg, turnAxis_ofVectorCircled.direction_normalized);
            Vector3 circleCenter_to_startOfDrawnCircleLineInsideStartCone = rotationToStartOfDrawnCircleLineInsideStartCone * circleCenter_to_start;
            float angleDeg_ofLinePartThatIsInsideCone = Vector3.Angle(circleCenter_to_endOfUnconedPart, circleCenter_to_endOfDrawnCircleLineInsideEndCone);
            if (angleDeg_ofLinePartThatIsInsideCone > 0.5f)
            {
                DrawBasics.LineCircled(circleCenter, circleCenter_to_endOfUnconedPart, circleCenter_to_endOfDrawnCircleLineInsideEndCone, color, radius, lineWidth, null, false, skipFallbackDisplayOfZeroAngles, flattenThickRoundLineIntoCirclePlane, 0.0f, textAnchor, durationInSec, hiddenByNearerObjects);
                if (pointerAtBothSides)
                {
                    DrawBasics.LineCircled(circleCenter, circleCenter_to_startOfDrawnCircleLineInsideStartCone, circleCenter_to_startOfUnconedPart, color, radius, lineWidth, null, false, skipFallbackDisplayOfZeroAngles, flattenThickRoundLineIntoCirclePlane, 0.0f, textAnchor, durationInSec, hiddenByNearerObjects);
                }
            }

            Vector3 circleCenter_to_end = Get_circleCenter_to_end(turnAngleDegCC, circleCenter_to_start, turnAxis_ofVectorCircled.direction_normalized);
            Vector3 directionOfEndCone = circleCenter_to_end - circleCenter_to_endOfUnconedPart;
            Vector3 upOfConeBaseRect = circleCenter_to_end + circleCenter_to_endOfUnconedPart;
            float coneAngleDeg_perpToCirclePlane = flattenThickRoundLineIntoCirclePlane ? 0.0f : coneAngleDeg;

            Color color_ofPointers = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, alphaFactorForPointers);
            DrawShapes.ConeFilled(circleCenter + circleCenter_to_end, coneLength, -directionOfEndCone, upOfConeBaseRect, coneAngleDeg, coneAngleDeg_perpToCirclePlane, color_ofPointers, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
            float angleDeg_ofLineWidthCone = 0.0f;
            float angleDeg_ofLineWidthCone_perpToCirclePlane = 0.0f;
            if (isThinLine == false)
            {
                angleDeg_ofLineWidthCone = Mathf.Rad2Deg * 2.0f * Mathf.Atan(0.5f * lineWidth / Mathf.Max(shorteningOf_straightLineInsideCone, 0.00001f));
                angleDeg_ofLineWidthCone = Mathf.Min(angleDeg_ofLineWidthCone, coneAngleDeg);
                angleDeg_ofLineWidthCone_perpToCirclePlane = flattenThickRoundLineIntoCirclePlane ? 0.0f : angleDeg_ofLineWidthCone;
                DrawShapes.ConeFilled(circleCenter + circleCenter_to_end, shorteningOf_straightLineInsideCone, circleCenter_to_endOfDrawnCircleLineInsideEndCone - circleCenter_to_end, circleCenter_to_endOfDrawnCircleLineInsideEndCone, angleDeg_ofLineWidthCone, angleDeg_ofLineWidthCone_perpToCirclePlane, color_ofPointers, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
            }

            if (pointerAtBothSides)
            {
                Vector3 directionOfStartCone = circleCenter_to_start - circleCenter_to_startOfUnconedPart;
                upOfConeBaseRect = circleCenter_to_start + circleCenter_to_startOfUnconedPart;
                DrawShapes.ConeFilled(circleCenter + circleCenter_to_start, coneLength, -directionOfStartCone, upOfConeBaseRect, coneAngleDeg, coneAngleDeg_perpToCirclePlane, color_ofPointers, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
                if (isThinLine == false)
                {
                    DrawShapes.ConeFilled(circleCenter + circleCenter_to_start, shorteningOf_straightLineInsideCone, circleCenter_to_startOfDrawnCircleLineInsideStartCone - circleCenter_to_start, circleCenter_to_startOfDrawnCircleLineInsideStartCone, angleDeg_ofLineWidthCone, angleDeg_ofLineWidthCone_perpToCirclePlane, color_ofPointers, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
                }
            }

        }

        public static Vector3 Get_circleCenter_to_end(float turnAngleDeg, Vector3 circleCenter_to_start, Vector3 turnAxis)
        {
            Vector3 circleCenter_to_end;
            if (turnAngleDeg >= 180.0f)
            {
                Quaternion rotation90DegIntoCorrectDir = Quaternion.AngleAxis(90.0f, turnAxis);
                circleCenter_to_end = rotation90DegIntoCorrectDir * circleCenter_to_start;
                circleCenter_to_end = rotation90DegIntoCorrectDir * circleCenter_to_end;
                Quaternion rotationOfHighterThan180Part = Quaternion.AngleAxis(turnAngleDeg - 180.0f, turnAxis);
                circleCenter_to_end = rotationOfHighterThan180Part * circleCenter_to_end;
            }
            else
            {
                if (turnAngleDeg <= (-180.0f))
                {
                    Quaternion rotation90DegIntoCorrectDir = Quaternion.AngleAxis(-90.0f, turnAxis);
                    circleCenter_to_end = rotation90DegIntoCorrectDir * circleCenter_to_start;
                    circleCenter_to_end = rotation90DegIntoCorrectDir * circleCenter_to_end;
                    Quaternion rotationOfHighterThan180Part = Quaternion.AngleAxis(turnAngleDeg + 180.0f, turnAxis);
                    circleCenter_to_end = rotationOfHighterThan180Part * circleCenter_to_end;
                }
                else
                {
                    Quaternion rotation_fromStartToEnd = Quaternion.AngleAxis(turnAngleDeg, turnAxis);
                    circleCenter_to_end = rotation_fromStartToEnd * circleCenter_to_start;
                }
            }
            return circleCenter_to_end;
        }

        static InternalDXXL_Plane circlePlane = new InternalDXXL_Plane();
        static float LineCircledBelow180Deg(Vector3 circleCenter, Vector3 circleCenter_to_start, out Vector3 circleCenter_towards_end_normalized, float turnAngleDeg, Vector3 turnAxis, Color color, float forceRadius, float width, bool flattenThickRoundLineIntoCirclePlane, float durationInSec, bool hiddenByNearerObjects)
        {
            //returns "usedRadius"
            circleCenter_towards_end_normalized = circleCenter_to_start;
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0.0f; }

            if ((turnAngleDeg > (-0.001f)) && (turnAngleDeg < 0.001f)) //-> it prooved not enough here to only check for "ApproxZero", even a threshold of "0.0001f" was too tight. It triggered errorcode-21.
            {
                return circleCenter_to_start.magnitude;
            }

            if (turnAngleDeg >= 180.0f)
            {
                return circleCenter_to_start.magnitude;
            }

            if (turnAngleDeg <= -180.0f)
            {
                return circleCenter_to_start.magnitude;
            }

            Vector3 circleCenter_to_start_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(circleCenter_to_start, out float circleCenter_to_start_magnitude);
            circleCenter_towards_end_normalized = circleCenter_to_start_normalized; //-> initial value that will get updated below for each subSegment

            float radius;
            Vector3 circleLineStartPos;
            if (UtilitiesDXXL_Math.ApproximatelyZero(forceRadius))
            {
                radius = circleCenter_to_start_magnitude;
                circleLineStartPos = circleCenter + circleCenter_to_start;
            }
            else
            {
                radius = forceRadius;
                circleLineStartPos = circleCenter + circleCenter_to_start_normalized * radius;
            }

            Quaternion rotationFromStartToEnd = Quaternion.AngleAxis(turnAngleDeg, turnAxis);
            float segmentsPerCircleDegree = 16.0f / 90.0f;
            int subSegments = Mathf.CeilToInt(5.0f + segmentsPerCircleDegree * Mathf.Abs(turnAngleDeg));
            float subSegmentsFloat = (float)subSegments;

            width = UtilitiesDXXL_Math.AbsNonZeroValue(width);
            if (width < DrawBasics.thinestPossibleNonZeroWidthLine) { width = 0.0f; }
            bool isThinLine = UtilitiesDXXL_Math.ApproximatelyZero(width);

            float progress0to1 = 1.0f / subSegmentsFloat;
            Quaternion rotation_fromCirceStart_toEndOfCurrSegment = Quaternion.Lerp(Quaternion.identity, rotationFromStartToEnd, progress0to1);
            Vector3 circleCenter_to_endOfCurrSegment_normalized = rotation_fromCirceStart_toEndOfCurrSegment * circleCenter_to_start_normalized;
            Vector3 endPosOfSegment = circleCenter + circleCenter_to_endOfCurrSegment_normalized * radius;

            if (flattenThickRoundLineIntoCirclePlane)
            {
                //-> This is the only case that needs an amplitude plane.
                //-> Precalced here so it has not be done repeatedly for every segment.
                circlePlane.Recreate(circleCenter, turnAxis);
            }

            UtilitiesDXXL_DrawBasics.DrawCircleSegment(isThinLine, circleLineStartPos, endPosOfSegment, color, width, durationInSec, hiddenByNearerObjects, flattenThickRoundLineIntoCirclePlane, circlePlane);
            Vector3 endPosOfPrevSegment = endPosOfSegment;

            for (int i = 1; i < subSegments; i++)
            {
                progress0to1 = (1.0f + i) / subSegmentsFloat;
                rotation_fromCirceStart_toEndOfCurrSegment = Quaternion.Lerp(Quaternion.identity, rotationFromStartToEnd, progress0to1);
                circleCenter_to_endOfCurrSegment_normalized = rotation_fromCirceStart_toEndOfCurrSegment * circleCenter_to_start_normalized;
                circleCenter_towards_end_normalized = circleCenter_to_endOfCurrSegment_normalized;
                endPosOfSegment = circleCenter + circleCenter_to_endOfCurrSegment_normalized * radius;
                UtilitiesDXXL_DrawBasics.DrawCircleSegment(isThinLine, endPosOfPrevSegment, endPosOfSegment, color, width, durationInSec, hiddenByNearerObjects, flattenThickRoundLineIntoCirclePlane, circlePlane);
                endPosOfPrevSegment = endPosOfSegment;
            }

            return radius;
        }

        static float CircleSegmentBelow180Deg(bool applyOffsetForRadialLinesToIndicateAnglesOver360deg, bool drawStraighRadialLineAtSegmentStart, bool drawStraighRadialLineAtSegmentEnd, Vector3 circleCenter, Vector3 circleCenter_to_startOnPerimeter, out Vector3 circleCenter_towards_endPosOnPerimeter_normalized, float turnAngleDeg, Vector3 turnAxis, float radiusPortionWhereDrawFillStarts, Color color, float forceRadius, float fillDensity, float durationInSec, bool hiddenByNearerObjects)
        {
            //returns "radiusOfTheOuterEndOfTheWidenedCircledLine"
            circleCenter_towards_endPosOnPerimeter_normalized = circleCenter_to_startOnPerimeter;
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0.0f; }

            if ((turnAngleDeg > (-0.001f)) && (turnAngleDeg < 0.001f)) //-> it prooved not enough here to only check for "ApproxZero", even a threshold of "0.0001f" was too tight. It triggered errorcode-21.
            {
                return circleCenter_to_startOnPerimeter.magnitude;
            }

            if (turnAngleDeg >= 180.0f)
            {
                return circleCenter_to_startOnPerimeter.magnitude;
            }

            if (turnAngleDeg <= -180.0f)
            {
                return circleCenter_to_startOnPerimeter.magnitude;
            }

            Vector3 circleCenter_towards_startOnPerimeter_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(circleCenter_to_startOnPerimeter, out float circleCenter_to_start_magnitude);
            circleCenter_towards_endPosOnPerimeter_normalized = circleCenter_towards_startOnPerimeter_normalized; //-> initial value that will get updated below for each subSegment

            float min_radiusPortionWhereDrawFillStarts = 0.02f;
            bool drawFill_startsAtCircleCenter = (radiusPortionWhereDrawFillStarts < min_radiusPortionWhereDrawFillStarts);
            radiusPortionWhereDrawFillStarts = Mathf.Clamp(radiusPortionWhereDrawFillStarts, min_radiusPortionWhereDrawFillStarts, 0.99f); //-> the case where "radiusPortionWhereDrawFillStarts" is "1" doesn't arrive here, but has already been treated otherwise

            float radiusOfTheOuterEndOfTheWidenedCircledLine;
            Vector3 startPosOfSegment_onRoundOuterBoundaryLine;
            if (UtilitiesDXXL_Math.ApproximatelyZero(forceRadius))
            {
                radiusOfTheOuterEndOfTheWidenedCircledLine = circleCenter_to_start_magnitude;
                startPosOfSegment_onRoundOuterBoundaryLine = circleCenter + circleCenter_to_startOnPerimeter;
            }
            else
            {
                radiusOfTheOuterEndOfTheWidenedCircledLine = forceRadius;
                startPosOfSegment_onRoundOuterBoundaryLine = circleCenter + circleCenter_towards_startOnPerimeter_normalized * radiusOfTheOuterEndOfTheWidenedCircledLine;
            }

            Quaternion rotationFromStartToEnd = Quaternion.AngleAxis(turnAngleDeg, turnAxis);
            float segmentsPerCircleDegree = fillDensity * (16.0f / 90.0f);
            segmentsPerCircleDegree = Mathf.Max(0.0f, segmentsPerCircleDegree);
            int subSegments = Mathf.CeilToInt(5.0f + segmentsPerCircleDegree * Mathf.Abs(turnAngleDeg));
            float subSegmentsFloat = (float)subSegments;

            float offsetForRadialLinesIndicatingAnglesOver360deg = applyOffsetForRadialLinesToIndicateAnglesOver360deg ? (-0.5f) : 0.0f;
            float progress0to1 = (1.0f + offsetForRadialLinesIndicatingAnglesOver360deg) / subSegmentsFloat;
            Quaternion rotation_fromCirceStart_toEndOfCurrSegment = Quaternion.Lerp(Quaternion.identity, rotationFromStartToEnd, progress0to1);
            Vector3 circleCenter_to_endOfCurrSegment_normalized = rotation_fromCirceStart_toEndOfCurrSegment * circleCenter_towards_startOnPerimeter_normalized;
            Vector3 endPosOfSegment_forRoundOuterBoundaryLine = circleCenter + circleCenter_to_endOfCurrSegment_normalized * radiusOfTheOuterEndOfTheWidenedCircledLine;

            Vector3 startPosOfSegment_onRoundInnerBoundaryLine = Get_posOnRoundInnerBoundaryLine(drawFill_startsAtCircleCenter, circleCenter, startPosOfSegment_onRoundOuterBoundaryLine, radiusPortionWhereDrawFillStarts);
            Vector3 endPosOfSegment_forRoundInnerBoundaryLine = Get_posOnRoundInnerBoundaryLine(drawFill_startsAtCircleCenter, circleCenter, endPosOfSegment_forRoundOuterBoundaryLine, radiusPortionWhereDrawFillStarts);

            UtilitiesDXXL_DrawBasics.DrawCircleSegment(true, startPosOfSegment_onRoundOuterBoundaryLine, endPosOfSegment_forRoundOuterBoundaryLine, color, 0.0f, durationInSec, hiddenByNearerObjects, false, null);

            if (drawFill_startsAtCircleCenter == false)
            {
                UtilitiesDXXL_DrawBasics.DrawCircleSegment(true, startPosOfSegment_onRoundInnerBoundaryLine, endPosOfSegment_forRoundInnerBoundaryLine, color, 0.0f, durationInSec, hiddenByNearerObjects, false, null);
            }

            if (drawStraighRadialLineAtSegmentStart)
            {
                DrawBasics.Line(startPosOfSegment_onRoundInnerBoundaryLine, startPosOfSegment_onRoundOuterBoundaryLine, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            startPosOfSegment_onRoundOuterBoundaryLine = endPosOfSegment_forRoundOuterBoundaryLine;
            startPosOfSegment_onRoundInnerBoundaryLine = endPosOfSegment_forRoundInnerBoundaryLine;

            for (int i = 1; i < subSegments; i++)
            {
                bool isLastSegment = (i == (subSegments - 1));
                float used_offsetForRadialLinesIndicatingAnglesOver360deg = isLastSegment ? 0.0f : offsetForRadialLinesIndicatingAnglesOver360deg;
                progress0to1 = (1.0f + i + used_offsetForRadialLinesIndicatingAnglesOver360deg) / subSegmentsFloat;
                rotation_fromCirceStart_toEndOfCurrSegment = Quaternion.Lerp(Quaternion.identity, rotationFromStartToEnd, progress0to1);
                circleCenter_to_endOfCurrSegment_normalized = rotation_fromCirceStart_toEndOfCurrSegment * circleCenter_towards_startOnPerimeter_normalized;
                circleCenter_towards_endPosOnPerimeter_normalized = circleCenter_to_endOfCurrSegment_normalized;
                endPosOfSegment_forRoundOuterBoundaryLine = circleCenter + circleCenter_to_endOfCurrSegment_normalized * radiusOfTheOuterEndOfTheWidenedCircledLine;
                endPosOfSegment_forRoundInnerBoundaryLine = Get_posOnRoundInnerBoundaryLine(drawFill_startsAtCircleCenter, circleCenter, endPosOfSegment_forRoundOuterBoundaryLine, radiusPortionWhereDrawFillStarts);

                UtilitiesDXXL_DrawBasics.DrawCircleSegment(true, startPosOfSegment_onRoundOuterBoundaryLine, endPosOfSegment_forRoundOuterBoundaryLine, color, 0.0f, durationInSec, hiddenByNearerObjects, false, null);

                if (drawFill_startsAtCircleCenter == false)
                {
                    UtilitiesDXXL_DrawBasics.DrawCircleSegment(true, startPosOfSegment_onRoundInnerBoundaryLine, endPosOfSegment_forRoundInnerBoundaryLine, color, 0.0f, durationInSec, hiddenByNearerObjects, false, null);
                }

                DrawBasics.Line(startPosOfSegment_onRoundInnerBoundaryLine, startPosOfSegment_onRoundOuterBoundaryLine, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

                if (isLastSegment)
                {
                    if (drawStraighRadialLineAtSegmentEnd)
                    {
                        DrawBasics.Line(endPosOfSegment_forRoundInnerBoundaryLine, endPosOfSegment_forRoundOuterBoundaryLine, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                    }
                }

                startPosOfSegment_onRoundOuterBoundaryLine = endPosOfSegment_forRoundOuterBoundaryLine;
                startPosOfSegment_onRoundInnerBoundaryLine = endPosOfSegment_forRoundInnerBoundaryLine;
            }

            return radiusOfTheOuterEndOfTheWidenedCircledLine;
        }

        static Vector3 Get_posOnRoundInnerBoundaryLine(bool drawFill_startsAtCircleCenter, Vector3 circleCenter, Vector3 endPosOnPerimeter, float radiusPortionWhereDrawFillStarts)
        {
            if (drawFill_startsAtCircleCenter)
            {
                return circleCenter;
            }
            else
            {
                Vector3 circleCenter_to_endPosOnPerimeter = (endPosOnPerimeter - circleCenter);
                return (circleCenter + circleCenter_to_endPosOnPerimeter * radiusPortionWhereDrawFillStarts);
            }
        }

        public static void LineCircledScreenspace(Camera targetCamera, Vector2 startPos, Vector2 circleCenter, float turnAngleDegCC, Color color, float width_relToViewportHeight, string text, bool skipFallbackDisplayOfZeroAngles, float minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL textAnchor, float durationInSec, bool drawSeparateFullCircle_forAnglesBiggerThan360)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(turnAngleDegCC, "turnAngleDegCC")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width_relToViewportHeight, "width_relToViewportHeight")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenter, "circleCenter")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(startPos, "startPos")) { return; }

            width_relToViewportHeight = UtilitiesDXXL_Math.AbsNonZeroValue(width_relToViewportHeight);
            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(startPos, circleCenter))
            {
                UtilitiesDXXL_Screenspace.PointFallback(targetCamera, circleCenter, "[<color=#adadadFF><icon=logMessage></color> LineCircledScreenspace with radius of 0]<br>" + text, color, width_relToViewportHeight, durationInSec);
                return;
            }

            Vector3 circleCenter_worldSpace = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(targetCamera, circleCenter, false);
            Vector3 startPos_worldSpace = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(targetCamera, startPos, false);
            Vector3 circleCenter_to_start_worldSpace = startPos_worldSpace - circleCenter_worldSpace;
            float width_worldSpace = 0.0f;
            if (UtilitiesDXXL_Math.ApproximatelyZero(width_relToViewportHeight) == false)
            {
                width_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, startPos, true, width_relToViewportHeight);
            }

            float unlooped_turnAngleDegCC = turnAngleDegCC;
            bool unloopedAngleIsOutside_m360_to_p360 = CheckIfAngleIsOutside_m360_to_p360(unlooped_turnAngleDegCC);
            if (drawSeparateFullCircle_forAnglesBiggerThan360) //-> endless regression loops are actually already prevented by "turnAngleDegCC_ofAdditional360degRing = 360.0f", but it's better to additionally protect against float calculation/comparison imprecsion herewith
            {
                if (unloopedAngleIsOutside_m360_to_p360)
                {
                    float turnAngleDegCC_ofAdditional360degRing = 360.0f;
                    string text_ofAdditional360degRing = null;
                    bool drawSeparateFullCircle_forAnglesBiggerThan360_onceMore = false;
                    LineCircledScreenspace(targetCamera, startPos, circleCenter, turnAngleDegCC_ofAdditional360degRing, color, width_relToViewportHeight, text_ofAdditional360degRing, true, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, drawSeparateFullCircle_forAnglesBiggerThan360_onceMore);
                }
            }

            turnAngleDegCC = LoopAngleIntoSpanFrom_m360_to_p360(unlooped_turnAngleDegCC);
            if (UtilitiesDXXL_Math.ApproximatelyZero(turnAngleDegCC))
            {
                if (unloopedAngleIsOutside_m360_to_p360)
                {
                    //-> multiples of 360° arrive here, like +/-720° or +/-1080°
                    turnAngleDegCC = 359.99f * Mathf.Sign(unlooped_turnAngleDegCC);
                }
                else
                {
                    if (skipFallbackDisplayOfZeroAngles == false)
                    {
                        DrawScreenspace.Vector(targetCamera, circleCenter, startPos, color, width_relToViewportHeight, "[<color=#adadadFF><icon=logMessage></color> LineCircledScreenSpace with angle of 0°]<br>" + text, 0.05f, false, false, 0.0f, durationInSec);
                    }
                    return;
                }
            }

            Vector3 circleCenter_towards_end_normalized;
            if (turnAngleDegCC >= 180.0f)
            {
                LineCircledBelow180Deg(circleCenter_worldSpace, circleCenter_to_start_worldSpace, out circleCenter_towards_end_normalized, 90.0f, targetCamera.transform.forward, color, 0.0f, width_worldSpace, true, durationInSec, false);
                LineCircledBelow180Deg(circleCenter_worldSpace, -circleCenter_to_start_worldSpace, out circleCenter_towards_end_normalized, -90.0f, targetCamera.transform.forward, color, 0.0f, width_worldSpace, true, durationInSec, false);
                LineCircledBelow180Deg(circleCenter_worldSpace, -circleCenter_to_start_worldSpace, out circleCenter_towards_end_normalized, turnAngleDegCC - 180.0f, targetCamera.transform.forward, color, 0.0f, width_worldSpace, true, durationInSec, false);
            }
            else
            {
                if (turnAngleDegCC <= (-180.0f))
                {
                    LineCircledBelow180Deg(circleCenter_worldSpace, circleCenter_to_start_worldSpace, out circleCenter_towards_end_normalized, -90.0f, targetCamera.transform.forward, color, 0.0f, width_worldSpace, true, durationInSec, false);
                    LineCircledBelow180Deg(circleCenter_worldSpace, -circleCenter_to_start_worldSpace, out circleCenter_towards_end_normalized, 90.0f, targetCamera.transform.forward, color, 0.0f, width_worldSpace, true, durationInSec, false);
                    LineCircledBelow180Deg(circleCenter_worldSpace, -circleCenter_to_start_worldSpace, out circleCenter_towards_end_normalized, turnAngleDegCC + 180.0f, targetCamera.transform.forward, color, 0.0f, width_worldSpace, true, durationInSec, false);
                }
                else
                {
                    LineCircledBelow180Deg(circleCenter_worldSpace, circleCenter_to_start_worldSpace, out circleCenter_towards_end_normalized, turnAngleDegCC, targetCamera.transform.forward, color, 0.0f, width_worldSpace, true, durationInSec, false);
                }
            }

            TryDrawText_atLineCircledScreenspace(targetCamera, startPos, circleCenter, turnAngleDegCC, color, width_relToViewportHeight, text, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec);
        }

        public static void CircleSegmentScreenspace(Camera targetCamera, Vector2 startPosOnPerimeter, Vector2 circleCenter, float turnAngleDegCC, Color color, string text, float radiusPortionWhereDrawFillStarts, bool skipFallbackDisplayOfZeroAngles, float fillDensity, float minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL textAnchor, float durationInSec, bool drawSeparateFullCircle_forAnglesBiggerThan360)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(turnAngleDegCC, "turnAngleDegCC")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radiusPortionWhereDrawFillStarts, "radiusPortionWhereDrawFillStarts")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(fillDensity, "fillDensity")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenter, "circleCenter")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(startPosOnPerimeter, "startPosOnPerimeter")) { return; }

            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(startPosOnPerimeter, circleCenter))
            {
                UtilitiesDXXL_Screenspace.PointFallback(targetCamera, circleCenter, "[<color=#adadadFF><icon=logMessage></color> CircleSegmentScreenspace with radius of 0]<br>" + text, color, 0.0f, durationInSec);
                return;
            }

            if (radiusPortionWhereDrawFillStarts >= 1.0f)
            {
                LineCircledScreenspace(targetCamera, startPosOnPerimeter, circleCenter, turnAngleDegCC, color, 0.0f, text, skipFallbackDisplayOfZeroAngles, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, drawSeparateFullCircle_forAnglesBiggerThan360);
                return;
            }

            Vector3 circleCenter_worldSpace = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(targetCamera, circleCenter, false);
            Vector3 startPosOnPerimeter_worldSpace = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(targetCamera, startPosOnPerimeter, false);
            Vector3 circleCenter_to_startPosOnPerimeter_worldSpace = startPosOnPerimeter_worldSpace - circleCenter_worldSpace;

            float unlooped_turnAngleDegCC = turnAngleDegCC;
            bool unloopedAngleIsOutside_m360_to_p360 = CheckIfAngleIsOutside_m360_to_p360(unlooped_turnAngleDegCC);
            bool applyOffsetForRadialLinesToIndicateAnglesOver360deg = false;
            if (drawSeparateFullCircle_forAnglesBiggerThan360) //-> endless regression loops are actually already prevented by "turnAngleDegCC_ofAdditional360degRing = 360.0f", but it's better to additionally protect against float calculation/comparison imprecsion herewith
            {
                if (unloopedAngleIsOutside_m360_to_p360)
                {
                    float turnAngleDegCC_ofAdditional360degRing = 360.0f;
                    string text_ofAdditional360degRing = null;
                    bool drawSeparateFullCircle_forAnglesBiggerThan360_onceMore = false;
                    CircleSegmentScreenspace(targetCamera, startPosOnPerimeter, circleCenter, turnAngleDegCC_ofAdditional360degRing, color, text_ofAdditional360degRing, radiusPortionWhereDrawFillStarts, true, fillDensity, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, drawSeparateFullCircle_forAnglesBiggerThan360_onceMore);
                    applyOffsetForRadialLinesToIndicateAnglesOver360deg = true;
                }
            }

            turnAngleDegCC = LoopAngleIntoSpanFrom_m360_to_p360(unlooped_turnAngleDegCC);
            if (UtilitiesDXXL_Math.ApproximatelyZero(turnAngleDegCC))
            {
                if (unloopedAngleIsOutside_m360_to_p360)
                {
                    //-> multiples of 360° arrive here, like +/-720° or +/-1080°
                    turnAngleDegCC = 359.99f * Mathf.Sign(unlooped_turnAngleDegCC);
                }
                else
                {
                    if (skipFallbackDisplayOfZeroAngles == false)
                    {
                        DrawScreenspace.Vector(targetCamera, circleCenter, startPosOnPerimeter, color, 0.0f, "[<color=#adadadFF><icon=logMessage></color> CircleSegmentScreenspace with angle of 0°]<br>" + text, 0.05f, false, false, 0.0f, durationInSec);
                    }
                    return;
                }
            }

            Vector3 circleCenter_towards_endPosOnPerimeter_normalized;
            if (turnAngleDegCC >= 180.0f)
            {
                CircleSegmentBelow180Deg(applyOffsetForRadialLinesToIndicateAnglesOver360deg, true, true, circleCenter_worldSpace, circleCenter_to_startPosOnPerimeter_worldSpace, out circleCenter_towards_endPosOnPerimeter_normalized, 90.0f, targetCamera.transform.forward, radiusPortionWhereDrawFillStarts, color, 0.0f, fillDensity, durationInSec, false);
                CircleSegmentBelow180Deg(applyOffsetForRadialLinesToIndicateAnglesOver360deg, true, false, circleCenter_worldSpace, -circleCenter_to_startPosOnPerimeter_worldSpace, out circleCenter_towards_endPosOnPerimeter_normalized, -90.0f, targetCamera.transform.forward, radiusPortionWhereDrawFillStarts, color, 0.0f, fillDensity, durationInSec, false);
                CircleSegmentBelow180Deg(applyOffsetForRadialLinesToIndicateAnglesOver360deg, false, true, circleCenter_worldSpace, -circleCenter_to_startPosOnPerimeter_worldSpace, out circleCenter_towards_endPosOnPerimeter_normalized, turnAngleDegCC - 180.0f, targetCamera.transform.forward, radiusPortionWhereDrawFillStarts, color, 0.0f, fillDensity, durationInSec, false);
            }
            else
            {
                if (turnAngleDegCC <= (-180.0f))
                {
                    CircleSegmentBelow180Deg(applyOffsetForRadialLinesToIndicateAnglesOver360deg, true, true, circleCenter_worldSpace, circleCenter_to_startPosOnPerimeter_worldSpace, out circleCenter_towards_endPosOnPerimeter_normalized, -90.0f, targetCamera.transform.forward, radiusPortionWhereDrawFillStarts, color, 0.0f, fillDensity, durationInSec, false);
                    CircleSegmentBelow180Deg(applyOffsetForRadialLinesToIndicateAnglesOver360deg, true, false, circleCenter_worldSpace, -circleCenter_to_startPosOnPerimeter_worldSpace, out circleCenter_towards_endPosOnPerimeter_normalized, 90.0f, targetCamera.transform.forward, radiusPortionWhereDrawFillStarts, color, 0.0f, fillDensity, durationInSec, false);
                    CircleSegmentBelow180Deg(applyOffsetForRadialLinesToIndicateAnglesOver360deg, false, true, circleCenter_worldSpace, -circleCenter_to_startPosOnPerimeter_worldSpace, out circleCenter_towards_endPosOnPerimeter_normalized, turnAngleDegCC + 180.0f, targetCamera.transform.forward, radiusPortionWhereDrawFillStarts, color, 0.0f, fillDensity, durationInSec, false);
                }
                else
                {
                    CircleSegmentBelow180Deg(applyOffsetForRadialLinesToIndicateAnglesOver360deg, true, true, circleCenter_worldSpace, circleCenter_to_startPosOnPerimeter_worldSpace, out circleCenter_towards_endPosOnPerimeter_normalized, turnAngleDegCC, targetCamera.transform.forward, radiusPortionWhereDrawFillStarts, color, 0.0f, fillDensity, durationInSec, false);
                }
            }

            TryDrawText_atLineCircledScreenspace(targetCamera, startPosOnPerimeter, circleCenter, turnAngleDegCC, color, 0.0f, text, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec);
        }

        static void TryDrawText_atLineCircledScreenspace(Camera targetCamera, Vector2 startPos, Vector2 circleCenter, float turnAngleDegCC, Color color, float width_relToViewportHeight, string text, float minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL textAnchor, float durationInSec)
        {
            if (text != null && text != "")
            {
                Vector2 circleCenter_to_start_inWarpedViewportSpace = startPos - circleCenter;
                Vector2 circleCenter_to_start_lookingLikeTheVersionInWarpedSpace_butInUnitsOfTheUnwarpedViewportSpace = DrawScreenspace.DirectionInUnitsOfWarpedSpace_to_sameLookingDirectionInUnitsOfUnwarpedSpace(circleCenter_to_start_inWarpedViewportSpace, targetCamera);
                float lineRadius_relToViewportHeight = circleCenter_to_start_lookingLikeTheVersionInWarpedSpace_butInUnitsOfTheUnwarpedViewportSpace.magnitude;
                float textRadius_relToViewportHeight = 1.05f * lineRadius_relToViewportHeight + 0.5f * width_relToViewportHeight;
                float textSize_relToViewportHeight = 0.1f * textRadius_relToViewportHeight;
                textSize_relToViewportHeight = Mathf.Max(textSize_relToViewportHeight, DrawScreenspace.minTextSize_relToViewportHeight);

                Vector3 textsInitialUp;
                float autoLineBreakAngleDeg;
                if (turnAngleDegCC > 0.0f)
                {
                    textsInitialUp = Quaternion.AngleAxis(turnAngleDegCC, Vector3.forward) * circleCenter_to_start_lookingLikeTheVersionInWarpedSpace_butInUnitsOfTheUnwarpedViewportSpace;
                    autoLineBreakAngleDeg = turnAngleDegCC;
                }
                else
                {
                    textsInitialUp = circleCenter_to_start_lookingLikeTheVersionInWarpedSpace_butInUnitsOfTheUnwarpedViewportSpace;
                    autoLineBreakAngleDeg = -turnAngleDegCC;
                }

                if (minAngleDeg_withoutTextLineBreak > 0.0f)
                {
                    autoLineBreakAngleDeg = Mathf.Max(autoLineBreakAngleDeg, minAngleDeg_withoutTextLineBreak);
                }

                UtilitiesDXXL_Text.WriteOnCircleScreenspace(targetCamera, text, circleCenter, textRadius_relToViewportHeight, color, textSize_relToViewportHeight, textsInitialUp, textAnchor, autoLineBreakAngleDeg, durationInSec, false);
            }
        }

        public static void VectorCircledScreenspace(Camera targetCamera, Vector2 startPos, Vector2 circleCenter, float turnAngleDegCC, Color color, float lineWidth_relToViewportHeight, string text, float coneLength_relToViewportHeight, bool skipFallbackDisplayOfZeroAngles, bool pointerAtBothSides, float minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL textAnchor, float durationInSec)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(turnAngleDegCC, "turnAngleDegCC")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lineWidth_relToViewportHeight, "lineWidth_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(coneLength_relToViewportHeight, "coneLength_relToViewportHeight")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenter, "circleCenter")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(startPos, "startPos")) { return; }

            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(startPos, circleCenter))
            {
                UtilitiesDXXL_Screenspace.PointFallback(targetCamera, circleCenter, "[<color=#adadadFF><icon=logMessage></color> VectorCircledScreenspace with radius of 0]<br>" + text, color, lineWidth_relToViewportHeight, durationInSec);
                return;
            }

            Vector3 circleCenter_worldSpace = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(targetCamera, circleCenter, false);
            Vector3 startPos_worldSpace = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(targetCamera, startPos, false);

            lineWidth_relToViewportHeight = UtilitiesDXXL_Math.AbsNonZeroValue(lineWidth_relToViewportHeight);
            float lineWidth_worldSpace;
            bool isThinLine;
            if (UtilitiesDXXL_Math.ApproximatelyZero(lineWidth_relToViewportHeight))
            {
                isThinLine = true;
                lineWidth_worldSpace = 0.0f;
            }
            else
            {
                isThinLine = false;
                lineWidth_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, startPos, true, lineWidth_relToViewportHeight);
            }

            float unlooped_turnAngleDegCC = turnAngleDegCC;
            bool unloopedAngleIsOutside_m360_to_p360 = CheckIfAngleIsOutside_m360_to_p360(unlooped_turnAngleDegCC);
            if (unloopedAngleIsOutside_m360_to_p360)
            {
                float turnAngleDegCC_ofAdditional360degRing = 360.0f;
                string text_ofAdditional360degRing = null;
                bool drawSeparateFullCircle_forAnglesBiggerThan360_onceMore = false;
                LineCircled(startPos_worldSpace, circleCenter_worldSpace, targetCamera.transform.forward, turnAngleDegCC_ofAdditional360degRing, color, lineWidth_worldSpace, text_ofAdditional360degRing, skipFallbackDisplayOfZeroAngles, true, durationInSec, false, true, minAngleDeg_withoutTextLineBreak, textAnchor, drawSeparateFullCircle_forAnglesBiggerThan360_onceMore);
            }

            turnAngleDegCC = LoopAngleIntoSpanFrom_m360_to_p360(turnAngleDegCC);
            if (UtilitiesDXXL_Math.ApproximatelyZero(turnAngleDegCC))
            {
                if (unloopedAngleIsOutside_m360_to_p360)
                {
                    //-> multiples of 360° arrive here, like +/-720° or +/-1080°
                    turnAngleDegCC = 359.99f * Mathf.Sign(unlooped_turnAngleDegCC);
                }
                else
                {
                    if (skipFallbackDisplayOfZeroAngles == false)
                    {
                        DrawScreenspace.Vector(targetCamera, circleCenter, startPos, color, lineWidth_relToViewportHeight, "[<color=#adadadFF><icon=logMessage></color> radius line of VectorCircledScreenspace with angle of 0°]<br>" + text, 0.05f, false, false, 0.0f, durationInSec);
                    }
                    return;
                }
            }

            float absTurnAngleDeg = Mathf.Abs(turnAngleDegCC);
            float turnAngleSign = Mathf.Sign(turnAngleDegCC);
            Vector3 circleCenter_to_start_worldSpace = startPos_worldSpace - circleCenter_worldSpace;
            float circleCenter_to_start_worldSpace_magnitude = circleCenter_to_start_worldSpace.magnitude;
            float radius_worldSpace = circleCenter_to_start_worldSpace_magnitude;
            float perimeterOf360deg_worldSpace = 2.0f * radius_worldSpace * Mathf.PI;
            float coneLength_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, startPos, true, coneLength_relToViewportHeight);
            float minConeLength_worldSpace = (0.01f * absTurnAngleDeg / 360.0f) * perimeterOf360deg_worldSpace;
            float maxConeLength_worldSpace = (0.45f * absTurnAngleDeg / 360.0f) * perimeterOf360deg_worldSpace;
            float maxConeLength_asAngleDeg = 45.0f;
            float maxConeLength_fromAngleLimit_worldSpace = (maxConeLength_asAngleDeg / 360.0f) * perimeterOf360deg_worldSpace;
            maxConeLength_worldSpace = Mathf.Min(maxConeLength_worldSpace, maxConeLength_fromAngleLimit_worldSpace);
            coneLength_worldSpace = Mathf.Clamp(coneLength_worldSpace, minConeLength_worldSpace, maxConeLength_worldSpace);
            float coneLength_asAngleDeg = 360.0f * (coneLength_worldSpace / perimeterOf360deg_worldSpace);
            float coneAngleDeg = 25.0f;
            if (isThinLine == false)
            {
                float coneSize_to_lineWidth_scaler = 1.2f;
                float minConeAngleDeg = 2.0f * Mathf.Rad2Deg * Mathf.Atan(coneSize_to_lineWidth_scaler * lineWidth_worldSpace / coneLength_worldSpace);
                coneAngleDeg = Mathf.Max(coneAngleDeg, minConeAngleDeg);
            }

            float shorteningOf_straightLineInsideCone_worldSpace = 0.0f;
            if (isThinLine == false)
            {
                shorteningOf_straightLineInsideCone_worldSpace = (0.5f * lineWidth_worldSpace) / Mathf.Tan(Mathf.Deg2Rad * 0.5f * coneAngleDeg);
                shorteningOf_straightLineInsideCone_worldSpace = 2.0f * shorteningOf_straightLineInsideCone_worldSpace; //solves: curvedLine is not parallel/aligned to coneDir and therefore intersects the coneSurface
                shorteningOf_straightLineInsideCone_worldSpace = Mathf.Min(shorteningOf_straightLineInsideCone_worldSpace, 0.85f * coneLength_worldSpace);
            }
            float shorteningOf_straightLineInsideCone_asAngleDeg = 360.0f * (shorteningOf_straightLineInsideCone_worldSpace / perimeterOf360deg_worldSpace);
            float turnAngleDeg_ofUnconedPart = turnAngleDegCC - (turnAngleSign * coneLength_asAngleDeg);
            Vector3 circleCenter_to_startOfUnconedPart_worldSpace = circleCenter_to_start_worldSpace;
            if (pointerAtBothSides)
            {
                turnAngleDeg_ofUnconedPart = turnAngleDegCC - 2.0f * (turnAngleSign * coneLength_asAngleDeg);
                Quaternion rotationToStartOfUnconedPart = Quaternion.AngleAxis(turnAngleSign * coneLength_asAngleDeg, targetCamera.transform.forward);
                circleCenter_to_startOfUnconedPart_worldSpace = rotationToStartOfUnconedPart * circleCenter_to_start_worldSpace;
            }
            Quaternion rotationToEndOfUnconedPart = Quaternion.AngleAxis(turnAngleDegCC - (turnAngleSign * coneLength_asAngleDeg), targetCamera.transform.forward);
            Vector3 circleCenter_to_endOfUnconedPart_worldSpace = rotationToEndOfUnconedPart * circleCenter_to_start_worldSpace;

            Vector3 circleCenter_to_counterClockwiseEndOfUnconedPart_worldSpace;
            float turnAngleDeg_ofUnconedPart_clockwise;

            if (turnAngleDeg_ofUnconedPart > 0.0f)
            {
                //counterclockwise:
                turnAngleDeg_ofUnconedPart_clockwise = -turnAngleDeg_ofUnconedPart;
                circleCenter_to_counterClockwiseEndOfUnconedPart_worldSpace = circleCenter_to_endOfUnconedPart_worldSpace;
            }
            else
            {
                //clockwise:
                turnAngleDeg_ofUnconedPart_clockwise = turnAngleDeg_ofUnconedPart;
                circleCenter_to_counterClockwiseEndOfUnconedPart_worldSpace = circleCenter_to_startOfUnconedPart_worldSpace;
            }
            bool skipTextMirrorInvertedFlipCheck = true;
            LineCircled(circleCenter_worldSpace + circleCenter_to_counterClockwiseEndOfUnconedPart_worldSpace, circleCenter_worldSpace, targetCamera.transform.forward, turnAngleDeg_ofUnconedPart_clockwise, color, lineWidth_worldSpace, text, skipFallbackDisplayOfZeroAngles, true, durationInSec, false, skipTextMirrorInvertedFlipCheck, minAngleDeg_withoutTextLineBreak, textAnchor);

            Quaternion rotationToEndOfDrawnCircleLineInsideEndCone = Quaternion.AngleAxis(turnAngleDegCC - (turnAngleSign * shorteningOf_straightLineInsideCone_asAngleDeg), targetCamera.transform.forward);
            Vector3 circleCenter_to_endOfDrawnCircleLineInsideEndCone_worldSpace = rotationToEndOfDrawnCircleLineInsideEndCone * circleCenter_to_start_worldSpace;
            Quaternion rotationToStartOfDrawnCircleLineInsideStartCone = Quaternion.AngleAxis(turnAngleSign * shorteningOf_straightLineInsideCone_asAngleDeg, targetCamera.transform.forward);
            Vector3 circleCenter_to_startOfDrawnCircleLineInsideStartCone_worldSpace = rotationToStartOfDrawnCircleLineInsideStartCone * circleCenter_to_start_worldSpace;
            float angleDeg_ofLinePartThatIsInsideCone = Vector3.Angle(circleCenter_to_endOfUnconedPart_worldSpace, circleCenter_to_endOfDrawnCircleLineInsideEndCone_worldSpace);
            if (angleDeg_ofLinePartThatIsInsideCone > 0.5f)
            {
                DrawBasics.LineCircled(circleCenter_worldSpace, circleCenter_to_endOfUnconedPart_worldSpace, circleCenter_to_endOfDrawnCircleLineInsideEndCone_worldSpace, color, radius_worldSpace, lineWidth_worldSpace, null, false, skipFallbackDisplayOfZeroAngles, true, 0.0f, textAnchor, durationInSec, false);
                if (pointerAtBothSides)
                {
                    DrawBasics.LineCircled(circleCenter_worldSpace, circleCenter_to_startOfDrawnCircleLineInsideStartCone_worldSpace, circleCenter_to_startOfUnconedPart_worldSpace, color, radius_worldSpace, lineWidth_worldSpace, null, false, skipFallbackDisplayOfZeroAngles, true, 0.0f, textAnchor, durationInSec, false);
                }
            }

            Vector3 circleCenter_to_end_worldSpace = Get_circleCenter_to_end(turnAngleDegCC, circleCenter_to_start_worldSpace, targetCamera.transform.forward);
            Vector3 directionOfEndCone_worldSpace = circleCenter_to_end_worldSpace - circleCenter_to_endOfUnconedPart_worldSpace;
            Vector3 upOfConeBaseRect_worldSpace = circleCenter_to_end_worldSpace + circleCenter_to_endOfUnconedPart_worldSpace;
            DrawShapes.ConeFilled(circleCenter_worldSpace + circleCenter_to_end_worldSpace, coneLength_worldSpace, -directionOfEndCone_worldSpace, upOfConeBaseRect_worldSpace, coneAngleDeg, 0.0f, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, false);
            float angleDeg_ofLineWidthCone = 0.0f;
            if (isThinLine == false)
            {
                angleDeg_ofLineWidthCone = Mathf.Rad2Deg * 2.0f * Mathf.Atan(0.5f * lineWidth_worldSpace / Mathf.Max(shorteningOf_straightLineInsideCone_worldSpace, 0.00001f));
                angleDeg_ofLineWidthCone = Mathf.Min(angleDeg_ofLineWidthCone, coneAngleDeg);
                DrawShapes.ConeFilled(circleCenter_worldSpace + circleCenter_to_end_worldSpace, shorteningOf_straightLineInsideCone_worldSpace, circleCenter_to_endOfDrawnCircleLineInsideEndCone_worldSpace - circleCenter_to_end_worldSpace, circleCenter_to_endOfDrawnCircleLineInsideEndCone_worldSpace, angleDeg_ofLineWidthCone, 0.0f, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, false);
            }

            if (pointerAtBothSides)
            {
                Vector3 directionOfStartCone_worldSpace = circleCenter_to_start_worldSpace - circleCenter_to_startOfUnconedPart_worldSpace;
                upOfConeBaseRect_worldSpace = circleCenter_to_start_worldSpace + circleCenter_to_startOfUnconedPart_worldSpace;
                DrawShapes.ConeFilled(circleCenter_worldSpace + circleCenter_to_start_worldSpace, coneLength_worldSpace, -directionOfStartCone_worldSpace, upOfConeBaseRect_worldSpace, coneAngleDeg, 0.0f, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, false);
                if (isThinLine == false)
                {
                    DrawShapes.ConeFilled(circleCenter_worldSpace + circleCenter_to_start_worldSpace, shorteningOf_straightLineInsideCone_worldSpace, circleCenter_to_startOfDrawnCircleLineInsideStartCone_worldSpace - circleCenter_to_start_worldSpace, circleCenter_to_startOfDrawnCircleLineInsideStartCone_worldSpace, angleDeg_ofLineWidthCone, 0.0f, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, false);
                }
            }
        }

        static bool CheckIfAngleIsOutside_m360_to_p360(float angle_preLoop)
        {
            if (angle_preLoop < (-360.0f))
            {
                return true;
            }
            else
            {
                if (angle_preLoop > 360.0f)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        static float LoopAngleIntoSpanFrom_m360_to_p360(float angle_preLoop)
        {
            if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(angle_preLoop, 360.0f))
            {
                return 359.99f;
            }
            else
            {
                if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(angle_preLoop, -360.0f))
                {
                    return (-359.99f);
                }
                else
                {
                    return UtilitiesDXXL_Math.Loop_floatIntoSpanFrom_mX_to_pX(angle_preLoop, 360.0f);
                }
            }
        }

    }

}
