namespace DrawXXL
{
    using UnityEngine;

    public class DrawMeasurements2D
    {
        public static float minimumLineLength_forDistancePointToLine = 1000.0f;
        public static float minimumLineLength_forAngleLineToLine = 1000.0f;

        public static float Distance(Vector2 from, Vector2 to, Color color = default(Color), float lineWidth = 0.0f, string text = null, float custom_zPos = float.PositiveInfinity, float coneLength = 0.10f, float enlargeSmallTextToThisMinTextSize = 0.005f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 fromV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(from, zPos);
            Vector3 toV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(to, zPos);
            return UtilitiesDXXL_Measurements.Distance(true, UtilitiesDXXL_Measurements.DistanceSpecifyingStringType.point_point, fromV3, toV3, color, lineWidth, text, coneLength, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, false);
        }

        public static float Angle(Vector2 from, Vector2 to, Vector2 turnCenter, Color color = default(Color), float forceRadius = 0.0f, float lineWidth = 0.0f, string text = null, float custom_zPos = float.PositiveInfinity, bool useReflexAngleOver180deg = false, bool displayAndReturn_radInsteadOfDeg = false, float coneLength = 0.13f, bool drawBoundaryLines = true, bool addTextForAlternativeAngleUnit = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 turnCenterV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(turnCenter, zPos);
            Vector3 fromV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(from);
            Vector3 toV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(to);
            return UtilitiesDXXL_Measurements.Angle(false, true, false, fromV3, toV3, turnCenterV3, color, forceRadius, lineWidth, text, useReflexAngleOver180deg, displayAndReturn_radInsteadOfDeg, coneLength, drawBoundaryLines, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
        }

        public static float AngleSpan(Vector2 from, Vector2 to, Vector2 turnCenter, Color color = default(Color), float forceRadius = 0.0f, float lineWidth = 0.0f, string text = null, float custom_zPos = float.PositiveInfinity, bool useReflexAngleOver180deg = false, bool displayAndReturn_radInsteadOfDeg = false, float coneLength = 0.13f, bool drawBoundaryLines = true, bool addTextForAlternativeAngleUnit = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 turnCenterV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(turnCenter, zPos);
            Vector3 fromV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(from);
            Vector3 toV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(to);
            return UtilitiesDXXL_Measurements.Angle(false, true, true, fromV3, toV3, turnCenterV3, color, forceRadius, lineWidth, text, useReflexAngleOver180deg, displayAndReturn_radInsteadOfDeg, coneLength, drawBoundaryLines, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
        }

        public static float DistancePointToLine(Vector2 point, Ray2D line, Color color = default(Color), float linesWidth = 0.0f, string text = null, string lineName = null, float custom_zPos = float.PositiveInfinity, float coneLength = 0.10f, float enlargeSmallTextToThisMinTextSize = 0.005f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            return DistancePointToLine(point, line.origin, line.direction, color, linesWidth, text, lineName, custom_zPos, coneLength, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects);
        }

        static InternalDXXL_Line line_3D = new InternalDXXL_Line();
        public static float DistancePointToLine(Vector2 point, Vector2 lineOrigin, Vector2 lineDirection, Color color = default(Color), float linesWidth = 0.0f, string text = null, string lineName = null, float custom_zPos = float.PositiveInfinity, float coneLength = 0.10f, float enlargeSmallTextToThisMinTextSize = 0.005f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return 0.0f; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(point, "point")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(lineOrigin, "lineOrigin")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(lineDirection, "lineDirection")) { return 0.0f; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            if (UtilitiesDXXL_Math.ApproximatelyZero(lineDirection))
            {
                UtilitiesDXXL_DrawBasics2D.PointFallback(point, zPos, "[<color=#adadadFF><icon=logMessage></color> 'lineDirection' is zero. DistancePointToLine2D measure operation not executed.]<br>[point]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics2D.PointFallback(lineOrigin, zPos, "[<color=#adadadFF><icon=logMessage></color> 'lineDirection' is zero. DistancePointToLine2D measure operation not executed.]<br>[lineOrigin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return 0.0f;
            }

            Vector3 pointV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(point, zPos);
            Vector3 lineOriginV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(lineOrigin, zPos);
            Vector3 lineDirectionV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(lineDirection);

            line_3D.Recreate(lineOriginV3, lineDirectionV3, false);
            Vector3 pointsProjectionOntoLine = line_3D.Get_perpProjectionOfPoint_ontoThisLine(pointV3);

            //Draw distance:
            bool skipDrawing = DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped();
            float distance = UtilitiesDXXL_Measurements.Distance(true, UtilitiesDXXL_Measurements.DistanceSpecifyingStringType.point_line, pointV3, pointsProjectionOntoLine, color, linesWidth, text, coneLength, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipDrawing);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return distance; }

            //Draw line:
            string lineIdentifyingText = ((lineName == null) || (lineName == "")) ? "line direction" : lineName;
            float widthOfDirVector = UtilitiesDXXL_Math.ApproximatelyZero(linesWidth) ? 0.01f : (0.6f * linesWidth);
            Color colorOfProlongedLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawMeasurements.defaultColor2, 0.55f);
            Color colorOfAttachments = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawMeasurements.defaultColor2, 0.35f);
            DrawBasics2D.VectorFrom(lineOriginV3, lineDirectionV3, DrawMeasurements.defaultColor2, widthOfDirVector, DrawText.MarkupColor(lineIdentifyingText, colorOfAttachments), coneLength * 1.7f, false, zPos, true, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);

            Vector3 projectionToLineOrigin = line_3D.origin - pointsProjectionOntoLine;
            float distance_projectionToLineOrigin = projectionToLineOrigin.magnitude;
            float lineExtentionPerSide = Mathf.Max(minimumLineLength_forDistancePointToLine, 1.1f * distance_projectionToLineOrigin);
            Line_fadeableAnimSpeed.InternalDraw(lineOriginV3 - line_3D.direction_normalized * lineExtentionPerSide, lineOriginV3 + line_3D.direction_normalized * lineExtentionPerSide, colorOfProlongedLine, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            UtilitiesDXXL_Measurements.WriteLineNameAtProjectionPlumbPos(true, lineName, "line", "", projectionToLineOrigin, distance_projectionToLineOrigin, pointV3, line_3D, pointsProjectionOntoLine, 0.02f * distance, colorOfProlongedLine, true, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_Measurements.Draw90degSymbol(distance, pointsProjectionOntoLine, pointV3, line_3D.direction_normalized, colorOfAttachments, durationInSec, hiddenByNearerObjects);

            return distance;
        }

        public static float AngleLineToLine(Ray2D line1, Ray2D line2, Color color = default(Color), float linesWidth = 0.0f, string text = null, string line1Name = null, string line2Name = null, float custom_zPos = float.PositiveInfinity, bool returnObtuseAngleOver90deg = false, bool displayAndReturn_radInsteadOfDeg = false, float coneLength = 0.13f, bool addTextForAlternativeAngleUnit = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            return AngleLineToLine(line1.origin, line1.direction, line2.origin, line2.direction, color, linesWidth, text, line1Name, line2Name, custom_zPos, returnObtuseAngleOver90deg, displayAndReturn_radInsteadOfDeg, coneLength, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
        }

        static InternalDXXL_Line line1_3D = new InternalDXXL_Line();
        static InternalDXXL_Line line2_3D = new InternalDXXL_Line();
        static InternalDXXL_Line2D line1_2D = new InternalDXXL_Line2D();
        static InternalDXXL_Line2D line2_2D = new InternalDXXL_Line2D();
        public static float AngleLineToLine(Vector2 line1Origin, Vector2 line1Direction, Vector2 line2Origin, Vector2 line2Direction, Color color = default(Color), float linesWidth = 0.0f, string text = null, string line1Name = null, string line2Name = null, float custom_zPos = float.PositiveInfinity, bool returnObtuseAngleOver90deg = false, bool displayAndReturn_radInsteadOfDeg = false, float coneLength = 0.13f, bool addTextForAlternativeAngleUnit = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return 0.0f; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(line1Origin, "line1Origin")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(line1Direction, "line1Direction")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(line2Origin, "line2Origin")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(line2Direction, "line2Direction")) { return 0.0f; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            if (UtilitiesDXXL_Math.ApproximatelyZero(line1Direction))
            {
                UtilitiesDXXL_DrawBasics2D.PointFallback(line1Origin, zPos, "[<color=#adadadFF><icon=logMessage></color> 'line1Direction' is zero. AngleLineToLine measure operation not executed.]<br>[line1Origin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics2D.PointFallback(line2Origin, zPos, "[<color=#adadadFF><icon=logMessage></color> 'line1Direction' is zero. AngleLineToLine measure operation not executed.]<br>[line2Origin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return 0.0f;
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(line2Direction))
            {
                UtilitiesDXXL_DrawBasics2D.PointFallback(line1Origin, zPos, "[<color=#adadadFF><icon=logMessage></color> 'line2Direction' is zero. AngleLineToLine measure operation not executed.]<br>[line1Origin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics2D.PointFallback(line2Origin, zPos, "[<color=#adadadFF><icon=logMessage></color> 'line2Direction' is zero. AngleLineToLine measure operation not executed.]<br>[line2Origin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return 0.0f;
            }

            float length_ofLine1Direction;
            float length_ofLine2Direction;
            Vector2 line1_direction_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(line1Direction, out length_ofLine1Direction);
            Vector2 line2_direction_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(line2Direction, out length_ofLine2Direction);

            float alphaFactor_ofProlongedLines = 0.55f;
            float alphaFactor_ofLineAttachments = 0.35f;
            Color colorOfProlongedLine1 = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawMeasurements.defaultColor1, alphaFactor_ofProlongedLines);
            Color colorOfProlongedLine2 = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawMeasurements.defaultColor2, alphaFactor_ofProlongedLines);
            Color colorOfLine1Attachments = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawMeasurements.defaultColor1, alphaFactor_ofLineAttachments);
            Color colorOfLine2Attachments = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawMeasurements.defaultColor2, alphaFactor_ofLineAttachments);

            string line1IdentifyingText = ((line1Name == null) || (line1Name == "")) ? "line1 direction" : line1Name;
            string line2IdentifyingText = ((line2Name == null) || (line2Name == "")) ? "line2 direction" : line2Name;

            float widthOfDirVector = UtilitiesDXXL_Math.ApproximatelyZero(linesWidth) ? 0.01f : (0.6f * linesWidth);

            //->calculation falls back to line-3D, because it is more precise
            Vector3 line1OriginV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(line1Origin, zPos);
            Vector3 line2OriginV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(line2Origin, zPos);
            Vector3 line1DirectionV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(line1Direction);
            Vector3 line2DirectionV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(line2Direction);
            line1_3D.Recreate(line1OriginV3, line1DirectionV3, false);
            line2_3D.Recreate(line2OriginV3, line2DirectionV3, false);

            bool linesAreApproxParallel = CheckIfLinesAreApproxParallel(out float displayedAndReturnedAngle_forParallelLines, returnObtuseAngleOver90deg, displayAndReturn_radInsteadOfDeg, line1Direction, line2Direction);
            if (linesAreApproxParallel)
            {
                if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return displayedAndReturnedAngle_forParallelLines; }
                float distanceBetweenLines = line1_3D.Get_perpDistance_ofGivenPoint_toThisLine(line2OriginV3);
                if (distanceBetweenLines < 0.001f)
                {
                    //parallel and coinciding lines:
                    line1IdentifyingText = line1IdentifyingText + "<br><br>[<color=#adadadFF><icon=logMessage></color> AngleLineToLine:<br>The two lines<br>lie approximately<br>parallel on top<br>of each other]";
                    float distanceBetweenLineOrigins = (line1Origin - line2Origin).magnitude;
                    if (distanceBetweenLineOrigins > Mathf.Max(length_ofLine1Direction, length_ofLine2Direction))
                    {
                        line2IdentifyingText = line2IdentifyingText + "<br><br>[<color=#adadadFF><icon=logMessage></color> AngleLineToLine:<br>The two lines<br>lie approximately<br>parallel on top<br>of each other]";
                    }
                }
                else
                {
                    //parallel lines with distance (not coinciding):
                    Vector3 line2origin_projectedOntoLine1 = line1_3D.Get_perpProjectionOfPoint_ontoThisLine(line2OriginV3);
                    Vector3 line1Origin_to_line2OriginOnLine1 = line2origin_projectedOntoLine1 - line1OriginV3;
                    float distanceBetweenLineOrigins_alongLineDir = line1Origin_to_line2OriginOnLine1.magnitude;

                    string textForDistanceFallback = "[<color=#adadadFF><icon=logMessage></color> AngleLineToLine:<br>The two lines are<br>approximately parallel<br>-> Fallback to 'Distance()']<br><br>" + text;
                    if (distanceBetweenLineOrigins_alongLineDir < 5.0f)
                    {
                        Vector3 centerBetweenLineOrigins_onLine1 = line1OriginV3 + 0.5f * line1Origin_to_line2OriginOnLine1;
                        Distance(centerBetweenLineOrigins_onLine1, line2_3D.Get_perpProjectionOfPoint_ontoThisLine(centerBetweenLineOrigins_onLine1), color, linesWidth, textForDistanceFallback, zPos, coneLength, 0.005f, durationInSec, hiddenByNearerObjects);
                    }
                    else
                    {
                        Distance(line1OriginV3, line2_3D.Get_perpProjectionOfPoint_ontoThisLine(line1OriginV3), color, linesWidth, textForDistanceFallback, zPos, coneLength, 0.005f, durationInSec, hiddenByNearerObjects);
                        Distance(line1_3D.Get_perpProjectionOfPoint_ontoThisLine(line2OriginV3), line2OriginV3, color, linesWidth, textForDistanceFallback, zPos, coneLength, 0.005f, durationInSec, hiddenByNearerObjects);
                    }
                }

                //Draw lineVectors:
                DrawBasics2D.VectorFrom(line1Origin, line1Direction, colorOfProlongedLine1, widthOfDirVector, line1IdentifyingText, coneLength, false, zPos, true, 0.005f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                DrawBasics2D.VectorFrom(line2Origin, line2Direction, colorOfProlongedLine2, widthOfDirVector, line2IdentifyingText, coneLength, false, zPos, true, 0.005f, false, 0.0f, durationInSec, hiddenByNearerObjects);

                //Draw lineExtentions:
                float lineExtentionPerSide = UtilitiesDXXL_Math.Max(minimumLineLength_forAngleLineToLine, line1Origin.magnitude, line2Origin.magnitude);
                Line_fadeableAnimSpeed_2D.InternalDraw(line1Origin, line1Origin - line1_direction_normalized * lineExtentionPerSide, colorOfProlongedLine1, 0.0f, null, DrawBasics.LineStyle.solid, zPos, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                Line_fadeableAnimSpeed_2D.InternalDraw(line1Origin, line1Origin + line1_direction_normalized * lineExtentionPerSide, colorOfProlongedLine1, 0.0f, null, DrawBasics.LineStyle.solid, zPos, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                Line_fadeableAnimSpeed_2D.InternalDraw(line2Origin, line2Origin - line2_direction_normalized * lineExtentionPerSide, colorOfProlongedLine2, 0.0f, null, DrawBasics.LineStyle.solid, zPos, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                Line_fadeableAnimSpeed_2D.InternalDraw(line2Origin, line2Origin + line2_direction_normalized * lineExtentionPerSide, colorOfProlongedLine2, 0.0f, null, DrawBasics.LineStyle.solid, zPos, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

                return displayedAndReturnedAngle_forParallelLines;
            }
            else
            {
                //-> lines are not parallel

                Vector2 intersectionPos = line1_3D.Get_posOnLine_thatIsNearestTo_passingOtherLine(line2_3D);
                Vector2 intersectionToLine1Origin = line1Origin - intersectionPos;
                Vector2 intersectionToLine2Origin = line2Origin - intersectionPos;
                float distance_intersection_to_line1origin = intersectionToLine1Origin.magnitude;
                float distance_intersection_to_line2origin = intersectionToLine2Origin.magnitude;
                Vector2 intersectionTowardsLine1 = UtilitiesDXXL_Math.ApproximatelyZero(intersectionToLine1Origin) ? line1Direction : intersectionToLine1Origin;
                Vector2 intersectionTowardsLine2 = UtilitiesDXXL_Math.ApproximatelyZero(intersectionToLine2Origin) ? line2Direction : intersectionToLine2Origin;

                //Draw lineVectors:
                DrawBasics2D.VectorFrom(line1Origin, line1Direction, colorOfProlongedLine1, widthOfDirVector, DrawText.MarkupColor(line1IdentifyingText, colorOfProlongedLine1), coneLength, false, zPos, true, 0.005f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                DrawBasics2D.VectorFrom(line2Origin, line2Direction, colorOfProlongedLine2, widthOfDirVector, DrawText.MarkupColor(line2IdentifyingText, colorOfProlongedLine2), coneLength, false, zPos, true, 0.005f, false, 0.0f, durationInSec, hiddenByNearerObjects);

                //Draw lineExtentions:
                float lineExtentionPerSide = UtilitiesDXXL_Math.Max(minimumLineLength_forAngleLineToLine, line1Origin.magnitude, line2Origin.magnitude, 1.1f * distance_intersection_to_line1origin, 1.1f * distance_intersection_to_line2origin);
                Line_fadeableAnimSpeed_2D.InternalDraw(line1Origin, line1Origin - line1_direction_normalized * lineExtentionPerSide, colorOfProlongedLine1, 0.0f, null, DrawBasics.LineStyle.solid, zPos, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                Line_fadeableAnimSpeed_2D.InternalDraw(line1Origin, line1Origin + line1_direction_normalized * lineExtentionPerSide, colorOfProlongedLine1, 0.0f, null, DrawBasics.LineStyle.solid, zPos, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                Line_fadeableAnimSpeed_2D.InternalDraw(line2Origin, line2Origin - line2_direction_normalized * lineExtentionPerSide, colorOfProlongedLine2, 0.0f, null, DrawBasics.LineStyle.solid, zPos, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                Line_fadeableAnimSpeed_2D.InternalDraw(line2Origin, line2Origin + line2_direction_normalized * lineExtentionPerSide, colorOfProlongedLine2, 0.0f, null, DrawBasics.LineStyle.solid, zPos, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

                //Draw Angle Display:
                float angleDeg_towards1toTowards2 = Vector2.Angle(intersectionTowardsLine1, intersectionTowardsLine2);
                bool towards1_to_towards2_isAcuteBelow90deg = angleDeg_towards1toTowards2 <= 90.0f;
                float radius = Mathf.Min(0.5f * distance_intersection_to_line1origin, 0.5f * distance_intersection_to_line2origin);
                radius = Mathf.Max(radius, 0.3f);
                // if (text != null) { text = DrawText.MarkupSize(text, 4); }
                if (text != null) { text = "<size=4> <br>" + text + "</size>"; }

                float returnedAngle;
                if (returnObtuseAngleOver90deg)
                {
                    if (towards1_to_towards2_isAcuteBelow90deg)
                    {
                        returnedAngle = Angle(intersectionTowardsLine1, -intersectionTowardsLine2, intersectionPos, color, radius, linesWidth, text, zPos, false, displayAndReturn_radInsteadOfDeg, coneLength, false, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
                    }
                    else
                    {
                        returnedAngle = Angle(intersectionTowardsLine1, intersectionTowardsLine2, intersectionPos, color, radius, linesWidth, text, zPos, false, displayAndReturn_radInsteadOfDeg, coneLength, false, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
                    }
                }
                else
                {
                    if (towards1_to_towards2_isAcuteBelow90deg)
                    {
                        returnedAngle = Angle(intersectionTowardsLine1, intersectionTowardsLine2, intersectionPos, color, radius, linesWidth, text, zPos, false, displayAndReturn_radInsteadOfDeg, coneLength, false, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
                    }
                    else
                    {
                        returnedAngle = Angle(intersectionTowardsLine1, -intersectionTowardsLine2, intersectionPos, color, radius, linesWidth, text, zPos, false, displayAndReturn_radInsteadOfDeg, coneLength, false, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
                    }
                }
                if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return returnedAngle; }

                //Draw intersectionPos accentuating lines:
                lineExtentionPerSide = radius * 0.1f;
                float widthOfLineAccentuation = 0.015f * lineExtentionPerSide;
                Vector2 halfLine1ExtentionVector = line1_direction_normalized * lineExtentionPerSide;
                Vector2 halfLine2ExtentionVector = line2_direction_normalized * lineExtentionPerSide;
                Line_fadeableAnimSpeed_2D.InternalDraw(intersectionPos - halfLine1ExtentionVector, intersectionPos + halfLine1ExtentionVector, colorOfProlongedLine1, widthOfLineAccentuation, null, DrawBasics.LineStyle.solid, zPos, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                Line_fadeableAnimSpeed_2D.InternalDraw(intersectionPos - halfLine2ExtentionVector, intersectionPos + halfLine2ExtentionVector, colorOfProlongedLine2, widthOfLineAccentuation, null, DrawBasics.LineStyle.solid, zPos, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                DrawBasics2D.Circle(intersectionPos, radius * 0.01f, color, 0.0f, null, DrawBasics.LineStyle.solid, zPos, 1.0f, DrawBasics.LineStyle.invisible, false, durationInSec, hiddenByNearerObjects);

                UtilitiesDXXL_DrawBasics.Set_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM_reversible(0);
                DrawBasics2D.Point(intersectionPos, color, radius * 0.2f, 0.0f, 0.0f, null, color, zPos, false, true, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.Reverse_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM();

                //Draw additional lineNameText at intersection pos:
                line1_2D.Recalc_line_throughTwoPoints_returnSteepForVertLines(line1Origin, line1Origin + line1Direction);
                line2_2D.Recalc_line_throughTwoPoints_returnSteepForVertLines(line2Origin, line2Origin + line2Direction);
                bool line1_isSteeperUpwardsWhenWalkingAlongPositiveX = line1_2D.m >= line2_2D.m;
                float minNecessaryDistanceToLineOrigin_forSeparateLineNameTextToBeDrawn = 2.5f;
                float textSize = 0.03f * radius;

                line1IdentifyingText = ((line1Name == null) || (line1Name == "")) ? "  line1" : "  " + line1Name;
                line2IdentifyingText = ((line2Name == null) || (line2Name == "")) ? "  line2" : "  " + line2Name;

                if (distance_intersection_to_line1origin > Mathf.Max(2.0f * length_ofLine1Direction, minNecessaryDistanceToLineOrigin_forSeparateLineNameTextToBeDrawn))
                {
                    Vector2 textDir = (line1Direction.x > 0.0f) ? line1Direction : (-line1Direction);
                    DrawText.TextAnchorDXXL textAnchor = line1_isSteeperUpwardsWhenWalkingAlongPositiveX ? DrawText.TextAnchorDXXL.LowerLeft : DrawText.TextAnchorDXXL.UpperLeft;
                    UtilitiesDXXL_Text.Write2DFramed(line1IdentifyingText, intersectionPos, colorOfLine1Attachments, textSize, textDir, textAnchor, zPos, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.005f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
                }

                if (distance_intersection_to_line2origin > Mathf.Max(2.0f * length_ofLine2Direction, minNecessaryDistanceToLineOrigin_forSeparateLineNameTextToBeDrawn))
                {
                    Vector2 textDir = (line2Direction.x > 0.0f) ? line2Direction : (-line2Direction);
                    DrawText.TextAnchorDXXL textAnchor = line1_isSteeperUpwardsWhenWalkingAlongPositiveX ? DrawText.TextAnchorDXXL.UpperLeft : DrawText.TextAnchorDXXL.LowerLeft;
                    UtilitiesDXXL_Text.Write2DFramed(line2IdentifyingText, intersectionPos, colorOfLine2Attachments, textSize, textDir, textAnchor, zPos, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.005f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
                }

                return returnedAngle;
            }
        }

        static bool CheckIfLinesAreApproxParallel(out float displayedAndReturnedAngle_forParallelLines, bool returnObtuseAngleOver90deg, bool displayAndReturn_radInsteadOfDeg, Vector2 line1Direction, Vector2 line2Direction)
        {
            float angleDeg = Vector2.Angle(line1Direction, line2Direction);
            float angleDeg_acute = (angleDeg > 90.0f) ? (180.0f - angleDeg) : angleDeg;
            float angleDeg_obtuse = (angleDeg < 90.0f) ? (180.0f - angleDeg) : angleDeg;
            float angleRad_acute = Mathf.Deg2Rad * angleDeg_acute;
            float angleRad_obtuse = Mathf.Deg2Rad * angleDeg_obtuse;

            if (returnObtuseAngleOver90deg)
            {
                if (displayAndReturn_radInsteadOfDeg)
                {
                    displayedAndReturnedAngle_forParallelLines = angleRad_obtuse;
                }
                else
                {
                    displayedAndReturnedAngle_forParallelLines = angleDeg_obtuse;
                }
            }
            else
            {
                if (displayAndReturn_radInsteadOfDeg)
                {
                    displayedAndReturnedAngle_forParallelLines = angleRad_acute;
                }
                else
                {
                    displayedAndReturnedAngle_forParallelLines = angleDeg_acute;
                }
            }

            bool linesAreApproxParallel = (angleDeg_acute < 0.02f); //the minimum angle that "UnityEngine.Vector2.Angle()" can return seems to be between "0.01" and "0.02". Below that it always returns 0.
            return linesAreApproxParallel;
        }

        public static void DistanceThreshold(Vector2 startPos, Vector2 endPos, float thresholdDistance, string text = null, bool displayDistanceAlsoAsText = false, float lineWidth = 0.0f, float custom_zPos = float.PositiveInfinity, bool exactlyThresholdLength_countsAsShorter = true, float endPlates_size = 0.0f, DrawBasics.LineStyle overwriteStyle_forNear = DrawBasics.LineStyle.electricNoise, DrawBasics.LineStyle overwriteStyle_forFar = DrawBasics.LineStyle.solid, Color overwriteColor_forNear = default(Color), Color overwriteColor_forFar = default(Color), float enlargeSmallTextToThisMinTextSize = 0.005f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(thresholdDistance, "thresholdDistance")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(startPos, "startPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(endPos, "endPos")) { return; }

            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(startPos, endPos))
            {
                UtilitiesDXXL_DrawBasics2D.PointFallback(startPos, "[<color=#adadadFF><icon=logMessage></color> DistanceThreshold2D with distance of 0]<br>" + text, UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor_forNear, UtilitiesDXXL_Colors.red_boolFalse), lineWidth, durationInSec, hiddenByNearerObjects);
                return;
            }

            Color usedColor = new Color();
            DrawBasics.LineStyle usedLineStyle = DrawBasics.LineStyle.solid;
            float distance = (endPos - startPos).magnitude;
            float stylePatternScaleFactor = distance;

            if (displayDistanceAlsoAsText)
            {
                text = string.IsNullOrEmpty(text) ? ("distance =<br>" + distance) : ("distance =<br>" + distance + "<br>" + text);
            }

            UtilitiesDXXL_Measurements.ChooseColorAndStyleForDistanceThresholdLine(ref usedColor, ref usedLineStyle, distance, thresholdDistance, exactlyThresholdLength_countsAsShorter, overwriteStyle_forNear, overwriteStyle_forFar, overwriteColor_forNear, overwriteColor_forFar);

            UtilitiesDXXL_DrawBasics.Set_relSizeOfTextOnLines_reversible(0.65f);
            UtilitiesDXXL_DrawBasics.Set_shiftTextPosOnLines_toNonIntersecting_reversible(true);
            Line_fadeableAnimSpeed_2D.InternalDraw(startPos, endPos, usedColor, lineWidth, text, usedLineStyle, custom_zPos, stylePatternScaleFactor, 0.0f, null, endPlates_size, 0.0f, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, true, true);
            UtilitiesDXXL_DrawBasics.Reverse_relSizeOfTextOnLines();
            UtilitiesDXXL_DrawBasics.Reverse_shiftTextPosOnLines_toNonIntersecting();
        }

        public static void DistanceThresholds(Vector2 startPos, Vector2 endPos, float smallerThresholdDistance, float biggerThresholdDistance, string text = null, bool displayDistanceAlsoAsText = false, float lineWidth = 0.0f, float custom_zPos = float.PositiveInfinity, bool exactlyThresholdLength_countsAsShorter = true, float endPlates_size = 0.0f, DrawBasics.LineStyle overwriteStyle_forNear = DrawBasics.LineStyle.electricNoise, DrawBasics.LineStyle overwriteStyle_forMiddle = DrawBasics.LineStyle.electricImpulses, DrawBasics.LineStyle overwriteStyle_forFar = DrawBasics.LineStyle.solid, Color overwriteColor_forNear = default(Color), Color overwriteColor_forMiddle = default(Color), Color overwriteColor_forFar = default(Color), float enlargeSmallTextToThisMinTextSize = 0.005f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(smallerThresholdDistance, "smallerThresholdDistance")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(biggerThresholdDistance, "biggerThresholdDistance")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(startPos, "startPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(endPos, "endPos")) { return; }

            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(startPos, endPos))
            {
                UtilitiesDXXL_DrawBasics2D.PointFallback(startPos, "[<color=#adadadFF><icon=logMessage></color> DistanceThresholds2D with distance of 0]<br>" + text, UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor_forNear, UtilitiesDXXL_Colors.red_boolFalse), lineWidth, durationInSec, hiddenByNearerObjects);
                return;
            }

            if (smallerThresholdDistance > biggerThresholdDistance)
            {
                text = "[<color=#adadadFF><icon=logMessage></color> threshold distances are automatically flipped: smaller (" + smallerThresholdDistance + " -> " + biggerThresholdDistance + ") / bigger (" + biggerThresholdDistance + " -> " + smallerThresholdDistance + ")]<br>" + text;
                float smallerClipboard = smallerThresholdDistance;
                smallerThresholdDistance = biggerThresholdDistance;
                biggerThresholdDistance = smallerClipboard;
            }

            Color usedColor = new Color();
            DrawBasics.LineStyle usedLineStyle = DrawBasics.LineStyle.solid;
            float distance = (endPos - startPos).magnitude;
            float stylePatternScaleFactor = distance;

            if (displayDistanceAlsoAsText)
            {
                text = string.IsNullOrEmpty(text) ? ("distance =<br>" + distance) : ("distance =<br>" + distance + "<br>" + text);
            }

            UtilitiesDXXL_Measurements.ChooseColorAndStyleForDistanceThresholdsLine(ref usedColor, ref usedLineStyle, distance, smallerThresholdDistance, biggerThresholdDistance, exactlyThresholdLength_countsAsShorter, overwriteStyle_forNear, overwriteStyle_forMiddle, overwriteStyle_forFar, overwriteColor_forNear, overwriteColor_forMiddle, overwriteColor_forFar);

            UtilitiesDXXL_DrawBasics.Set_relSizeOfTextOnLines_reversible(0.65f);
            UtilitiesDXXL_DrawBasics.Set_shiftTextPosOnLines_toNonIntersecting_reversible(true);
            Line_fadeableAnimSpeed_2D.InternalDraw(startPos, endPos, usedColor, lineWidth, text, usedLineStyle, custom_zPos, stylePatternScaleFactor, 0.0f, null, endPlates_size, 0.0f, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, true, true);
            UtilitiesDXXL_DrawBasics.Reverse_relSizeOfTextOnLines();
            UtilitiesDXXL_DrawBasics.Reverse_shiftTextPosOnLines_toNonIntersecting();
        }

    }

}
