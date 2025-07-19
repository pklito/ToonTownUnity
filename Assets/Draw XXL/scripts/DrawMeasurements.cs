namespace DrawXXL
{
    using UnityEngine;

    public class DrawMeasurements
    {
        static InternalDXXL_Line line = new InternalDXXL_Line();
        static InternalDXXL_Line line1 = new InternalDXXL_Line();
        static InternalDXXL_Line line2 = new InternalDXXL_Line();

        public static Color defaultColor1 = new Color(0.26f, 1.0f, 1.0f, 1.0f);
        public static Color defaultColor2 = new Color(1.0f, 0.938f, 0.23f, 1.0f);

        public static Vector3 preferredPlanePatternOrientation_forDistancePointToPlane = Vector3.forward;
        public static float minimumLineLength_forDistancePointToLine = 1000.0f;
        public static float minimumLineLength_forDistanceLineToLine = 1000.0f;
        public static float minimumLineLength_forAngleLineToPlane = 1000.0f;

        public static float Distance(Vector3 from, Vector3 to, Color color = default(Color), float lineWidth = 0.0f, string text = null, float coneLength = 0.10f, float enlargeSmallTextToThisMinTextSize = 0.005f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            return UtilitiesDXXL_Measurements.Distance(false, UtilitiesDXXL_Measurements.DistanceSpecifyingStringType.point_point, from, to, color, lineWidth, text, coneLength, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, false);
        }

        public static float Angle(Vector3 from, Vector3 to, Vector3 turnCenter, Color color = default(Color), float forceRadius = 0.0f, float lineWidth = 0.0f, string text = null, bool useReflexAngleOver180deg = false, bool displayAndReturn_radInsteadOfDeg = false, float coneLength = 0.13f, bool drawBoundaryLines = true, bool addTextForAlternativeAngleUnit = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            return UtilitiesDXXL_Measurements.Angle(false, false, false, from, to, turnCenter, color, forceRadius, lineWidth, text, useReflexAngleOver180deg, displayAndReturn_radInsteadOfDeg, coneLength, drawBoundaryLines, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
        }

        public static float AngleSpan(Vector3 from, Vector3 to, Vector3 turnCenter, Color color = default(Color), float forceRadius = 0.0f, float lineWidth = 0.0f, string text = null, bool useReflexAngleOver180deg = false, bool displayAndReturn_radInsteadOfDeg = false, float coneLength = 0.13f, bool drawBoundaryLines = true, bool addTextForAlternativeAngleUnit = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            return UtilitiesDXXL_Measurements.Angle(false, false, true, from, to, turnCenter, color, forceRadius, lineWidth, text, useReflexAngleOver180deg, displayAndReturn_radInsteadOfDeg, coneLength, drawBoundaryLines, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
        }

        public static float DistancePointToLine(Vector3 point, Ray line, Color color = default(Color), float linesWidth = 0.0f, string text = null, string lineName = null, float coneLength = 0.10f, float enlargeSmallTextToThisMinTextSize = 0.005f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            return DistancePointToLine(point, line.origin, line.direction, color, linesWidth, text, lineName, coneLength, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects);
        }

        public static float DistancePointToLine(Vector3 point, Vector3 lineOrigin, Vector3 lineDirection, Color color = default(Color), float linesWidth = 0.0f, string text = null, string lineName = null, float coneLength = 0.10f, float enlargeSmallTextToThisMinTextSize = 0.005f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return 0.0f; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(point, "point")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(lineOrigin, "lineOrigin")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(lineDirection, "lineDirection")) { return 0.0f; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);
            if (UtilitiesDXXL_Math.ApproximatelyZero(lineDirection))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(point, "[<color=#adadadFF><icon=logMessage></color> 'lineDirection' is zero. DistancePointToLine measure operation not executed.]<br>[point]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.PointFallback(lineOrigin, "[<color=#adadadFF><icon=logMessage></color> 'lineDirection' is zero. DistancePointToLine measure operation not executed.]<br>[lineOrigin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return 0.0f;
            }

            line.Recreate(lineOrigin, lineDirection, false);
            Vector3 pointsProjectionOntoLine = line.Get_perpProjectionOfPoint_ontoThisLine(point);

            //Draw distance:
            bool skipDrawing = DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped();
            float distance = UtilitiesDXXL_Measurements.Distance(false, UtilitiesDXXL_Measurements.DistanceSpecifyingStringType.point_line, point, pointsProjectionOntoLine, color, linesWidth, text, coneLength, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipDrawing);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return distance; }

            //Draw line:
            string lineIdentifyingText = ((lineName == null) || (lineName == "")) ? "line direction" : lineName;
            float widthOfDirVector = UtilitiesDXXL_Math.ApproximatelyZero(linesWidth) ? 0.01f : (0.6f * linesWidth);
            Color colorOfProlongedLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(defaultColor2, 0.55f);
            Color colorOfProlongedLine_lowerAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(defaultColor2, 0.35f);
            DrawBasics.VectorFrom(lineOrigin, lineDirection, defaultColor2, widthOfDirVector, DrawText.MarkupColor(lineIdentifyingText, colorOfProlongedLine_lowerAlpha), coneLength * 1.7f, false, false, default(Vector3), true, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);

            Vector3 projectionToLineOrigin = line.origin - pointsProjectionOntoLine;
            float distance_projectionToLineOrigin = projectionToLineOrigin.magnitude;
            float lineExtentionPerSide = Mathf.Max(minimumLineLength_forDistancePointToLine, 1.1f * distance_projectionToLineOrigin);
            Line_fadeableAnimSpeed.InternalDraw(lineOrigin - line.direction_normalized * lineExtentionPerSide, lineOrigin + line.direction_normalized * lineExtentionPerSide, colorOfProlongedLine, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            UtilitiesDXXL_Measurements.WriteLineNameAtProjectionPlumbPos(false, lineName, "line", "", projectionToLineOrigin, distance_projectionToLineOrigin, point, line, pointsProjectionOntoLine, 0.02f * distance, colorOfProlongedLine, true, durationInSec, hiddenByNearerObjects);

            UtilitiesDXXL_Measurements.Draw90degSymbol(distance, pointsProjectionOntoLine, point, line.direction_normalized, colorOfProlongedLine_lowerAlpha, durationInSec, hiddenByNearerObjects);

            return distance;
        }

        public static float DistanceLineToLine(Ray line1, Ray line2, Color color = default(Color), float linesWidth = 0.0f, string text = null, string line1Name = null, string line2Name = null, float coneLength = 0.10f, float enlargeSmallTextToThisMinTextSize = 0.005f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            return DistanceLineToLine(line1.origin, line1.direction, line2.origin, line2.direction, color, linesWidth, text, line1Name, line2Name, coneLength, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects);
        }

        public static float DistanceLineToLine(Vector3 line1Origin, Vector3 line1Direction, Vector3 line2Origin, Vector3 line2Direction, Color color = default(Color), float linesWidth = 0.0f, string text = null, string line1Name = null, string line2Name = null, float coneLength = 0.10f, float enlargeSmallTextToThisMinTextSize = 0.005f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return 0.0f; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(line1Origin, "line1Origin")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(line1Direction, "line1Direction")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(line2Origin, "line2Origin")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(line2Direction, "line2Direction")) { return 0.0f; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);
            if (UtilitiesDXXL_Math.ApproximatelyZero(line1Direction))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(line1Origin, "[<color=#adadadFF><icon=logMessage></color> 'line1Direction' is zero. DistanceLineToLine measure operation not executed.]<br>[line1Origin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.PointFallback(line2Origin, "[<color=#adadadFF><icon=logMessage></color> 'line1Direction' is zero. DistanceLineToLine measure operation not executed.]<br>[line2Origin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return 0.0f;
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(line2Direction))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(line1Origin, "[<color=#adadadFF><icon=logMessage></color> 'line2Direction' is zero. DistanceLineToLine measure operation not executed.]<br>[line1Origin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.PointFallback(line2Origin, "[<color=#adadadFF><icon=logMessage></color> 'line2Direction' is zero. DistanceLineToLine measure operation not executed.]<br>[line2Origin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return 0.0f;
            }

            line1.Recreate(line1Origin, line1Direction, false);
            line2.Recreate(line2Origin, line2Direction, false);
            Vector3 posOnLine2_thatIsNearestToLine1 = line2.Get_posOnLine_thatIsNearestTo_passingOtherLine(line1);
            if (float.IsNaN(posOnLine2_thatIsNearestToLine1.x))
            {
                posOnLine2_thatIsNearestToLine1 = line2.origin;
            }
            Vector3 pointsProjectionOntoLine1 = line1.Get_perpProjectionOfPoint_ontoThisLine(posOnLine2_thatIsNearestToLine1);

            //Draw distance:
            bool skipDrawing = DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped();
            float distance = UtilitiesDXXL_Measurements.Distance(false, UtilitiesDXXL_Measurements.DistanceSpecifyingStringType.line_line, pointsProjectionOntoLine1, posOnLine2_thatIsNearestToLine1, color, linesWidth, text, coneLength, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipDrawing);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return distance; }

            //Draw lines:
            float widthOfDirVector = UtilitiesDXXL_Math.ApproximatelyZero(linesWidth) ? 0.01f : (0.6f * linesWidth);
            float alphaFactor_ofProlongedLines = 0.55f;
            float alphaFactor_ofLineAttachments = 0.35f;
            Color colorOfProlongedLine1 = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(defaultColor1, alphaFactor_ofProlongedLines);
            Color colorOfProlongedLine2 = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(defaultColor2, alphaFactor_ofProlongedLines);
            Color colorOfLine1Attachments = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(defaultColor1, alphaFactor_ofLineAttachments);
            Color colorOfLine2Attachments = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(defaultColor2, alphaFactor_ofLineAttachments);

            Vector3 projectionOnLine1ToLine1Origin = line1.origin - pointsProjectionOntoLine1;
            float distance_projectionToLine1Origin = projectionOnLine1ToLine1Origin.magnitude;
            float line1ExtentionPerSide = Mathf.Max(minimumLineLength_forDistanceLineToLine, 1.1f * distance_projectionToLine1Origin);
            string line1IdentifyingText = ((line1Name == null) || (line1Name == "")) ? "line1 direction" : line1Name;
            DrawBasics.VectorFrom(line1Origin, line1Direction, defaultColor1, widthOfDirVector, DrawText.MarkupColor(line1IdentifyingText, colorOfLine1Attachments), coneLength * 1.7f, false, false, default(Vector3), true, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
            Line_fadeableAnimSpeed.InternalDraw(line1Origin - line1.direction_normalized * line1ExtentionPerSide, line1Origin + line1.direction_normalized * line1ExtentionPerSide, colorOfProlongedLine1, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            Vector3 projectionOnLine2ToLine2Origin = line2.origin - posOnLine2_thatIsNearestToLine1;
            float distance_projectionToLine2Origin = projectionOnLine2ToLine2Origin.magnitude;
            float line2ExtentionPerSide = Mathf.Max(minimumLineLength_forDistanceLineToLine, 1.1f * distance_projectionToLine2Origin);
            string line2IdentifyingText = ((line2Name == null) || (line2Name == "")) ? "line2 direction" : line2Name;
            DrawBasics.VectorFrom(line2Origin, line2Direction, defaultColor2, widthOfDirVector, DrawText.MarkupColor(line2IdentifyingText, colorOfLine2Attachments), coneLength * 1.7f, false, false, default(Vector3), true, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
            Line_fadeableAnimSpeed.InternalDraw(line2Origin - line2.direction_normalized * line2ExtentionPerSide, line2Origin + line2.direction_normalized * line2ExtentionPerSide, colorOfProlongedLine2, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            UtilitiesDXXL_Measurements.WriteLineNameAtProjectionPlumbPos(false, line1Name, "line1", "", projectionOnLine1ToLine1Origin, distance_projectionToLine1Origin, posOnLine2_thatIsNearestToLine1, line1, pointsProjectionOntoLine1, 0.02f * distance, colorOfProlongedLine1, true, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_Measurements.WriteLineNameAtProjectionPlumbPos(false, line2Name, "line2", "", projectionOnLine2ToLine2Origin, distance_projectionToLine2Origin, pointsProjectionOntoLine1, line2, posOnLine2_thatIsNearestToLine1, 0.02f * distance, colorOfProlongedLine2, true, durationInSec, hiddenByNearerObjects);

            UtilitiesDXXL_Measurements.Draw90degSymbol(distance, pointsProjectionOntoLine1, posOnLine2_thatIsNearestToLine1, line1.direction_normalized, colorOfLine1Attachments, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_Measurements.Draw90degSymbol(distance, posOnLine2_thatIsNearestToLine1, pointsProjectionOntoLine1, line2.direction_normalized, colorOfLine2Attachments, durationInSec, hiddenByNearerObjects);

            return distance;
        }

        public static float DistancePerpToOrthoViewDir(Vector3 from, Vector3 to, Vector3 orthoViewDir, Color color = default(Color), float linesWidth = 0.0f, string text = null, float coneLength = 0.10f, float enlargeSmallTextToThisMinTextSize = 0.005f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return 0.0f; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(from, "from")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(to, "to")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(orthoViewDir, "orthoViewDir")) { return 0.0f; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);
            if (UtilitiesDXXL_Math.ApproximatelyZero(orthoViewDir))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(from, "[<color=#adadadFF><icon=logMessage></color> 'orthoViewDir' is zero. DistancePerpToOrthoViewDir measure operation not executed.]<br>[from]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.PointFallback(to, "[<color=#adadadFF><icon=logMessage></color> 'orthoViewDir' is zero. DistancePerpToOrthoViewDir measure operation not executed.]<br>[to]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return 0.0f;
            }

            orthoViewDir = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(orthoViewDir);
            line1.Recreate(from, orthoViewDir, false);
            line2.Recreate(to, orthoViewDir, false);
            Vector3 fromPoint_projectedOntoLine2 = line2.Get_perpProjectionOfPoint_ontoThisLine(from);
            if (float.IsNaN(fromPoint_projectedOntoLine2.x))
            {
                fromPoint_projectedOntoLine2 = line2.origin;
            }

            //Draw distance:
            bool skipDrawing = DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped();
            float distance = UtilitiesDXXL_Measurements.Distance(false, UtilitiesDXXL_Measurements.DistanceSpecifyingStringType.point_ll_point, from, fromPoint_projectedOntoLine2, color, linesWidth, text, coneLength, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipDrawing);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return distance; }

            //Draw lines:
            Vector3 point1_to_point2_alongViewDir = to - fromPoint_projectedOntoLine2;
            float distanceAlongViewDir = point1_to_point2_alongViewDir.magnitude;
            Vector3 point1_to_point2_alongViewDir_normalized;
            if (UtilitiesDXXL_Math.ApproximatelyZero(distanceAlongViewDir))
            {
                point1_to_point2_alongViewDir_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(orthoViewDir);
                distanceAlongViewDir = 1.0f;
            }
            else
            {
                point1_to_point2_alongViewDir_normalized = point1_to_point2_alongViewDir / distanceAlongViewDir;
            }

            Color colorOfAttachments = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(Color.white, 0.2f);
            float patternScaleFactor = distanceAlongViewDir;
            patternScaleFactor = Mathf.Max(patternScaleFactor, 0.02f);

            Vector3 line1_start = from - point1_to_point2_alongViewDir_normalized * (0.1f * distanceAlongViewDir);
            Line_fadeableAnimSpeed.InternalDraw(line1_start, line1_start + point1_to_point2_alongViewDir_normalized * (1.2f * distanceAlongViewDir), colorOfAttachments, 0.0f, null, DrawBasics.LineStyle.dashedLong, patternScaleFactor, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            Vector3 line2_start = to + point1_to_point2_alongViewDir_normalized * (0.1f * distanceAlongViewDir);
            Line_fadeableAnimSpeed.InternalDraw(line2_start, line2_start - point1_to_point2_alongViewDir_normalized * (1.2f * distanceAlongViewDir), colorOfAttachments, 0.0f, null, DrawBasics.LineStyle.dashedLong, patternScaleFactor, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            UtilitiesDXXL_Measurements.WriteOrthoViewDirNameAtProjectionPlumbPos("<size=5>       ortho view dir throught 'from'</size>", fromPoint_projectedOntoLine2, from, orthoViewDir, 0.02f * distance, colorOfAttachments, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_Measurements.WriteOrthoViewDirNameAtProjectionPlumbPos("<size=5>       ortho view dir throught 'to'</size>", from, fromPoint_projectedOntoLine2, -orthoViewDir, 0.02f * distance, colorOfAttachments, durationInSec, hiddenByNearerObjects);

            //Draw 'to'-Point:
            Vector3 upVector_forLookRotationOf_toPoint = (orthoViewDir.y >= 0.0f) ? orthoViewDir : (-orthoViewDir);
            Vector3 forwardVector_forLookRotationOf_toPoint = Vector3.Cross(orthoViewDir, from - fromPoint_projectedOntoLine2);
            forwardVector_forLookRotationOf_toPoint = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(forwardVector_forLookRotationOf_toPoint);
            if (UtilitiesDXXL_Math.ApproximatelyZero(forwardVector_forLookRotationOf_toPoint))
            {
                forwardVector_forLookRotationOf_toPoint = Vector3.forward;
            }
            Quaternion rotation_ofToPoint = Quaternion.LookRotation(forwardVector_forLookRotationOf_toPoint, upVector_forLookRotationOf_toPoint);

            UtilitiesDXXL_DrawBasics.Set_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM_reversible(0);
            DrawBasics.Point(to, "to<size=3>(distance perp to ortho)</size>", color, distance * 0.1f, linesWidth, color, rotation_ofToPoint, false, true, false, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_DrawBasics.Reverse_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM();

            UtilitiesDXXL_Measurements.Draw90degSymbol(distance, from, fromPoint_projectedOntoLine2, line1.direction_normalized, colorOfAttachments, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_Measurements.Draw90degSymbol(distance, fromPoint_projectedOntoLine2, from, line2.direction_normalized, colorOfAttachments, durationInSec, hiddenByNearerObjects);

            return distance;
        }

        public static float DistanceAlongOrthoViewDir(Vector3 from, Vector3 to, Vector3 orthoViewDir, Color color = default(Color), float linesWidth = 0.0f, string text = null, float coneLength = 0.10f, float enlargeSmallTextToThisMinTextSize = 0.005f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return 0.0f; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(from, "from")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(to, "to")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(orthoViewDir, "orthoViewDir")) { return 0.0f; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);
            if (UtilitiesDXXL_Math.ApproximatelyZero(orthoViewDir))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(from, "[<color=#adadadFF><icon=logMessage></color> 'orthoViewDir' is zero. DistanceAlongOrthoViewDir measure operation not executed.]<br>[from]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.PointFallback(to, "[<color=#adadadFF><icon=logMessage></color> 'orthoViewDir' is zero. DistanceAlongOrthoViewDir measure operation not executed.]<br>[to]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return 0.0f;
            }

            orthoViewDir = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(orthoViewDir);
            line1.Recreate(from, orthoViewDir, false);
            line2.Recreate(to, orthoViewDir, false);
            Vector3 fromPoint_projectedOntoLine2 = line2.Get_perpProjectionOfPoint_ontoThisLine(from);
            if (float.IsNaN(fromPoint_projectedOntoLine2.x))
            {
                fromPoint_projectedOntoLine2 = line2.origin;
            }
            Vector3 toPoint_projectedOntoLine1 = line1.Get_perpProjectionOfPoint_ontoThisLine(to);

            //Draw distance:
            bool skipDrawing = DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped();
            float distanceAlongViewDir = UtilitiesDXXL_Measurements.Distance(false, UtilitiesDXXL_Measurements.DistanceSpecifyingStringType.point_l_I_l_point, from, toPoint_projectedOntoLine1, color, linesWidth, text, coneLength, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipDrawing);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return distanceAlongViewDir; }

            //Draw viewDirLines:
            float perpDistance_betweenLines = (fromPoint_projectedOntoLine2 - from).magnitude;
            Vector3 point1_to_point2_alongViewDir = to - fromPoint_projectedOntoLine2;
            Vector3 point1_to_point2_alongViewDir_normalized;
            float extentionBase_forLinesAlongViewDir;
            if (UtilitiesDXXL_Math.ApproximatelyZero(distanceAlongViewDir))
            {
                point1_to_point2_alongViewDir_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(orthoViewDir);
                extentionBase_forLinesAlongViewDir = 1.0f;
            }
            else
            {
                point1_to_point2_alongViewDir_normalized = point1_to_point2_alongViewDir / distanceAlongViewDir;
                extentionBase_forLinesAlongViewDir = distanceAlongViewDir;
            }

            Color colorOfAttachments = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(Color.white, 0.2f);

            float patternScaleFactor = distanceAlongViewDir;
            patternScaleFactor = Mathf.Max(patternScaleFactor, 0.02f);
            Vector3 line1_start = from - point1_to_point2_alongViewDir_normalized * (0.6f * extentionBase_forLinesAlongViewDir);
            Line_fadeableAnimSpeed.InternalDraw(line1_start, line1_start + point1_to_point2_alongViewDir_normalized * (2.2f * extentionBase_forLinesAlongViewDir), colorOfAttachments, 0.0f, null, DrawBasics.LineStyle.dashedLong, patternScaleFactor, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Vector3 line2_start = to + point1_to_point2_alongViewDir_normalized * (0.6f * extentionBase_forLinesAlongViewDir);
            Line_fadeableAnimSpeed.InternalDraw(line2_start, line2_start - point1_to_point2_alongViewDir_normalized * (2.2f * extentionBase_forLinesAlongViewDir), colorOfAttachments, 0.0f, null, DrawBasics.LineStyle.dashedLong, patternScaleFactor, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            UtilitiesDXXL_Measurements.WriteOrthoViewDirNameAtProjectionPlumbPos("<size=5>          ortho view dir throught 'from'</size>", fromPoint_projectedOntoLine2, from, orthoViewDir, 0.02f * perpDistance_betweenLines, colorOfAttachments, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_Measurements.WriteOrthoViewDirNameAtProjectionPlumbPos("<size=5>          ortho view dir throught 'to'</size>", from, fromPoint_projectedOntoLine2, -orthoViewDir, 0.02f * perpDistance_betweenLines, colorOfAttachments, durationInSec, hiddenByNearerObjects);

            //Draw 'to'-Point:
            Vector3 upVector_forLookRotationOf_toPoint = (orthoViewDir.y >= 0.0f) ? orthoViewDir : (-orthoViewDir);
            Vector3 forwardVector_forLookRotationOf_toPoint = Vector3.Cross(orthoViewDir, from - fromPoint_projectedOntoLine2);
            forwardVector_forLookRotationOf_toPoint = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(forwardVector_forLookRotationOf_toPoint);
            if (UtilitiesDXXL_Math.ApproximatelyZero(forwardVector_forLookRotationOf_toPoint))
            {
                forwardVector_forLookRotationOf_toPoint = Vector3.forward;
            }
            Quaternion rotation_ofToPoint = Quaternion.LookRotation(forwardVector_forLookRotationOf_toPoint, upVector_forLookRotationOf_toPoint);

            UtilitiesDXXL_DrawBasics.Set_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM_reversible(0);
            DrawBasics.Point(to, "to<size=3>(distance along ortho)</size>", color, perpDistance_betweenLines * 0.1f, linesWidth, color, rotation_ofToPoint, false, true, false, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_DrawBasics.Reverse_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM();

            if (perpDistance_betweenLines > 0.0001f)
            {
                //Draw perp connection lines:
                Line_fadeableAnimSpeed.InternalDraw(from, fromPoint_projectedOntoLine2, colorOfAttachments, 0.0f, null, DrawBasics.LineStyle.disconnectedAnchors, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                Line_fadeableAnimSpeed.InternalDraw(to, toPoint_projectedOntoLine1, colorOfAttachments, 0.0f, null, DrawBasics.LineStyle.disconnectedAnchors, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

                UtilitiesDXXL_Measurements.Draw90degSymbol(perpDistance_betweenLines, from, fromPoint_projectedOntoLine2, line1.direction_normalized, colorOfAttachments, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_Measurements.Draw90degSymbol(perpDistance_betweenLines, fromPoint_projectedOntoLine2, from, line2.direction_normalized, colorOfAttachments, durationInSec, hiddenByNearerObjects);

                UtilitiesDXXL_Measurements.Draw90degSymbol(perpDistance_betweenLines, to, toPoint_projectedOntoLine1, line1.direction_normalized, colorOfAttachments, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_Measurements.Draw90degSymbol(perpDistance_betweenLines, toPoint_projectedOntoLine1, to, line2.direction_normalized, colorOfAttachments, durationInSec, hiddenByNearerObjects);
            }

            return distanceAlongViewDir;
        }

        public static float DistancePointToPlane(Vector3 point, Transform planeTransform, Color color = default(Color), float linesWidth = 0.0f, string text = null, string planeName = null, float coneLength = 0.10f, float enlargeSmallTextToThisMinTextSize = 0.005f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(planeTransform, "planeTransform")) { return 0.0f; }
            return DistancePointToPlane(point, planeTransform.position, planeTransform.up, color, linesWidth, text, planeName, coneLength, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects);
        }

        public static float DistancePointToPlane(Vector3 point, Plane plane, Color color = default(Color), float linesWidth = 0.0f, string text = null, string planeName = null, float coneLength = 0.10f, float enlargeSmallTextToThisMinTextSize = 0.005f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            return DistancePointToPlane(point, plane.ClosestPointOnPlane(Vector3.zero), plane.normal, color, linesWidth, text, planeName, coneLength, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects);
        }

        static InternalDXXL_Plane plane = new InternalDXXL_Plane();
        public static float DistancePointToPlane(Vector3 point, Vector3 planeOrigin, Vector3 planeNormal, Color color = default(Color), float linesWidth = 0.0f, string text = null, string planeName = null, float coneLength = 0.10f, float enlargeSmallTextToThisMinTextSize = 0.005f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return 0.0f; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(point, "point")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(planeOrigin, "planeOrigin")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(planeNormal, "planeNormal")) { return 0.0f; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);
            if (UtilitiesDXXL_Math.ApproximatelyZero(planeNormal))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(point, "[<color=#adadadFF><icon=logMessage></color> 'planeNormal' is zero. DistancePointToPlane measure operation not executed.]<br>[point]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.PointFallback(planeOrigin, "[<color=#adadadFF><icon=logMessage></color> 'planeNormal' is zero. DistancePointToPlane measure operation not executed.]<br>[planeOrigin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return 0.0f;
            }

            plane.Recreate(planeOrigin, planeNormal);
            Vector3 projectionOfPointOnPlane = plane.Get_perpProjectionOfPointOnPlane(point);
            if (float.IsNaN(projectionOfPointOnPlane.x))
            {
                projectionOfPointOnPlane = point;
            }

            bool skipDrawing = DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped();
            float distance = UtilitiesDXXL_Measurements.Distance(false, UtilitiesDXXL_Measurements.DistanceSpecifyingStringType.point_plane, point, projectionOfPointOnPlane, color, linesWidth, text, coneLength, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipDrawing);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return distance; }

            Color planeColor = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(defaultColor2, 0.5f);
            float planeSize_nonExtended = distance;
            planeSize_nonExtended = Mathf.Max(planeSize_nonExtended, 1.0f);
            float anchorVisualizationSize = 0.2f * planeSize_nonExtended;
            float subSegments_signFlipsInterpretation;
            Vector3 forward_insidePlane = preferredPlanePatternOrientation_forDistancePointToPlane; //-> The plane grid pattern orientation stance can rotate fast and unnaturally when "planeNormal" becomes similar to "preferredPlanePatternOrientation_forDistancePointToPlane". Though since there is no preferred plane stance expected to be used more often than others I don't see an easy fix for that.
            UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref planeNormal, ref forward_insidePlane, true);
            forward_insidePlane = plane.Get_projectionOfVectorOntoPlane(forward_insidePlane);

            //Main Plane:
            subSegments_signFlipsInterpretation = (-0.1f) * planeSize_nonExtended; //-> "negative sign" means "fixed world space size of segments"
            DrawShapes.Plane(planeOrigin, planeNormal, projectionOfPointOnPlane, planeColor, planeSize_nonExtended, planeSize_nonExtended, forward_insidePlane, 0.0f, null, subSegments_signFlipsInterpretation, false, anchorVisualizationSize, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);

            //Emphasizing plane at points projection on plane:
            subSegments_signFlipsInterpretation = 20; //-> "positive sign" means "fixed number of segments"
            DrawShapes.Plane(projectionOfPointOnPlane, planeNormal, default, planeColor, 0.2f * planeSize_nonExtended, 0.2f * planeSize_nonExtended, forward_insidePlane, 0.0f, null, subSegments_signFlipsInterpretation, false, 0.0f, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);

            UtilitiesDXXL_Measurements.Draw90degSymbol(distance, projectionOfPointOnPlane, point, forward_insidePlane, planeColor, durationInSec, hiddenByNearerObjects);

            if ((planeName == null) || (planeName == "")) { planeName = "plane"; }
            UtilitiesDXXL_Text.WriteFramed(planeName, projectionOfPointOnPlane + 0.11f * planeSize_nonExtended * forward_insidePlane, planeColor, 0.05f * planeSize_nonExtended, forward_insidePlane, Vector3.Cross(planeNormal, forward_insidePlane), DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
            return distance;
        }

        public static float AngleLineToPlane(Ray line, Transform planeTransform, Color color = default(Color), float linesWidth = 0.0f, string text = null, string lineName = null, string planeName = null, bool returnObtuseAngleOver90deg = false, bool displayAndReturn_radInsteadOfDeg = false, float coneLength = 0.13f, bool addTextForAlternativeAngleUnit = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(planeTransform, "planeTransform")) { return 0.0f; }
            return AngleLineToPlane(line.origin, line.direction, planeTransform.position, planeTransform.up, color, linesWidth, text, lineName, planeName, returnObtuseAngleOver90deg, displayAndReturn_radInsteadOfDeg, coneLength, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
        }

        public static float AngleLineToPlane(Ray line, Plane plane, Color color = default(Color), float linesWidth = 0.0f, string text = null, string lineName = null, string planeName = null, bool returnObtuseAngleOver90deg = false, bool displayAndReturn_radInsteadOfDeg = false, float coneLength = 0.13f, bool addTextForAlternativeAngleUnit = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            return AngleLineToPlane(line.origin, line.direction, plane.ClosestPointOnPlane(Vector3.zero), plane.normal, color, linesWidth, text, lineName, planeName, returnObtuseAngleOver90deg, displayAndReturn_radInsteadOfDeg, coneLength, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
        }

        public static float AngleLineToPlane(Ray line, Vector3 planeOrigin, Vector3 planeNormal, Color color = default(Color), float linesWidth = 0.0f, string text = null, string lineName = null, string planeName = null, bool returnObtuseAngleOver90deg = false, bool displayAndReturn_radInsteadOfDeg = false, float coneLength = 0.13f, bool addTextForAlternativeAngleUnit = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            return AngleLineToPlane(line.origin, line.direction, planeOrigin, planeNormal, color, linesWidth, text, lineName, planeName, returnObtuseAngleOver90deg, displayAndReturn_radInsteadOfDeg, coneLength, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
        }

        public static float AngleLineToPlane(Vector3 lineOrigin, Vector3 lineDirection, Transform planeTransform, Color color = default(Color), float linesWidth = 0.0f, string text = null, string lineName = null, string planeName = null, bool returnObtuseAngleOver90deg = false, bool displayAndReturn_radInsteadOfDeg = false, float coneLength = 0.13f, bool addTextForAlternativeAngleUnit = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(planeTransform, "planeTransform")) { return 0.0f; }
            return AngleLineToPlane(lineOrigin, lineDirection, planeTransform.position, planeTransform.up, color, linesWidth, text, lineName, planeName, returnObtuseAngleOver90deg, displayAndReturn_radInsteadOfDeg, coneLength, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
        }

        public static float AngleLineToPlane(Vector3 lineOrigin, Vector3 lineDirection, Plane plane, Color color = default(Color), float linesWidth = 0.0f, string text = null, string lineName = null, string planeName = null, bool returnObtuseAngleOver90deg = false, bool displayAndReturn_radInsteadOfDeg = false, float coneLength = 0.13f, bool addTextForAlternativeAngleUnit = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            return AngleLineToPlane(lineOrigin, lineDirection, plane.ClosestPointOnPlane(Vector3.zero), plane.normal, color, linesWidth, text, lineName, planeName, returnObtuseAngleOver90deg, displayAndReturn_radInsteadOfDeg, coneLength, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
        }

        public static float AngleLineToPlane(Vector3 lineOrigin, Vector3 lineDirection, Vector3 planeOrigin, Vector3 planeNormal, Color color = default(Color), float linesWidth = 0.0f, string text = null, string lineName = null, string planeName = null, bool returnObtuseAngleOver90deg = false, bool displayAndReturn_radInsteadOfDeg = false, float coneLength = 0.13f, bool addTextForAlternativeAngleUnit = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return 0.0f; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(lineOrigin, "lineOrigin")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(lineDirection, "lineDirection")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(planeOrigin, "planeOrigin")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(planeNormal, "planeNormal")) { return 0.0f; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);
            if (UtilitiesDXXL_Math.ApproximatelyZero(planeNormal))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(lineOrigin, "[<color=#adadadFF><icon=logMessage></color> 'planeNormal' is zero. AngleLineToPlane measure operation not executed.]<br>[lineOrigin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.PointFallback(planeOrigin, "[<color=#adadadFF><icon=logMessage></color> 'planeNormal' is zero. AngleLineToPlane measure operation not executed.]<br>[planeOrigin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return 0.0f;
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(lineDirection))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(lineOrigin, "[<color=#adadadFF><icon=logMessage></color> 'lineDirection' is zero. AngleLineToPlane measure operation not executed.]<br>[lineOrigin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.PointFallback(planeOrigin, "[<color=#adadadFF><icon=logMessage></color> 'lineDirection' is zero. AngleLineToPlane measure operation not executed.]<br>[planeOrigin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return 0.0f;
            }

            planeNormal = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(planeNormal);
            Vector3 planeNormal_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(planeNormal);
            line.Recreate(lineOrigin, lineDirection, false);
            plane.Recreate(planeOrigin, planeNormal_normalized);

            Color planeColor = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(defaultColor2, 0.5f);
            Color colorOfProlongedLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(defaultColor1, 0.55f);
            Color color_lowerAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.45f);
            Color colorOfAttachments = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(defaultColor1, 0.35f);

            string lineIdentifyingText = ((lineName == null) || (lineName == "")) ? "line direction" : lineName;
            float widthOfDirVector = UtilitiesDXXL_Math.ApproximatelyZero(linesWidth) ? 0.01f : (0.6f * linesWidth);
            Vector3 lineOrigins_projectionOnPlane = plane.Get_perpProjectionOfPointOnPlane(line.origin);

            if (Mathf.Abs(Vector3.Dot(plane.normalDir, line.direction_normalized)) < 0.0001f)
            {
                //line is parallel to plane:
                UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                DrawBasics.VectorFrom(lineOrigin, lineDirection, defaultColor1, widthOfDirVector, DrawText.MarkupColor(lineIdentifyingText, colorOfAttachments), 0.17f, false, false, default(Vector3), true, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();

                Vector3 intersectionToLineOrigin_fallback = Vector3.zero;
                float distance_intersectionToLineOrigin_fallback = 0.0f;
                Line_fadeableAnimSpeed.InternalDraw(lineOrigin - line.direction_normalized * minimumLineLength_forAngleLineToPlane, lineOrigin + line.direction_normalized * minimumLineLength_forAngleLineToPlane, colorOfProlongedLine, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                UtilitiesDXXL_Measurements.WriteLineNameAtProjectionPlumbPos(false, lineName, "line", "", intersectionToLineOrigin_fallback, distance_intersectionToLineOrigin_fallback, lineOrigins_projectionOnPlane, line, line.origin, 0.1f, colorOfProlongedLine, false, durationInSec, hiddenByNearerObjects);
                DistancePointToPlane(lineOrigin, planeOrigin, planeNormal_normalized, color, linesWidth, "[<color=#adadadFF><icon=logMessage></color> Line is approximately parallel to plane -> fallback from 'Angle()' to 'Distance()']<br>" + text, planeName, coneLength, 0.0f, durationInSec, hiddenByNearerObjects);
                return returnObtuseAngleOver90deg ? (displayAndReturn_radInsteadOfDeg ? (Mathf.Deg2Rad * 180.0f) : 180.0f) : 0.0f;
            }

            Vector3 intersectionPoint = line.Get_intersectionPoint_withPlane_withoutParallelCheck(plane);
            Vector3 intersection_to_lineOrigin = line.origin - intersectionPoint;
            float distance_intersection_to_lineOrigin = intersection_to_lineOrigin.magnitude;
            Vector3 intersection_towardsLine_normalized;
            if (distance_intersection_to_lineOrigin < 0.0001f)
            {
                intersection_towardsLine_normalized = planeNormal_normalized;
            }
            else
            {
                intersection_towardsLine_normalized = intersection_to_lineOrigin / distance_intersection_to_lineOrigin;
            }

            float radius = 0.5f * distance_intersection_to_lineOrigin;
            radius = Mathf.Max(radius, 0.2f);

            Vector3 lineOriginProjectionOnPlane_to_intersectionPos = intersectionPoint - lineOrigins_projectionOnPlane;
            float distance_from_lineOriginProjectionOnPlane_to_intersectionPos = lineOriginProjectionOnPlane_to_intersectionPos.magnitude;

            Vector3 forward_insidePlane_normalized = default;
            if (distance_from_lineOriginProjectionOnPlane_to_intersectionPos < 0.0001f)
            {
                //angle is approximately 90deg:
                UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref planeNormal_normalized, ref forward_insidePlane_normalized, true);
                forward_insidePlane_normalized = plane.Get_projectionOfVectorOntoPlane(forward_insidePlane_normalized);
                forward_insidePlane_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(forward_insidePlane_normalized);
            }
            else
            {
                forward_insidePlane_normalized = lineOriginProjectionOnPlane_to_intersectionPos / distance_from_lineOriginProjectionOnPlane_to_intersectionPos;
            }

            float planeSize_nonExtended;
            if (distance_from_lineOriginProjectionOnPlane_to_intersectionPos < 0.1f)
            {
                planeSize_nonExtended = 1.0f;
            }
            else
            {
                planeSize_nonExtended = 1.2f * distance_from_lineOriginProjectionOnPlane_to_intersectionPos;
            }
            planeSize_nonExtended = Mathf.Max(planeSize_nonExtended, 1.0f);

            float acuteAngle;
            float obtuseAngle;
            if (returnObtuseAngleOver90deg)
            {
                acuteAngle = UtilitiesDXXL_Measurements.Angle(false, false, false, intersection_towardsLine_normalized, -forward_insidePlane_normalized, intersectionPoint, color_lowerAlpha, radius, 0.0f, null, false, displayAndReturn_radInsteadOfDeg, coneLength, true, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
                obtuseAngle = UtilitiesDXXL_Measurements.Angle(true, false, false, intersection_towardsLine_normalized, forward_insidePlane_normalized, intersectionPoint, color, radius, linesWidth, text, false, displayAndReturn_radInsteadOfDeg, coneLength, true, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                acuteAngle = UtilitiesDXXL_Measurements.Angle(true, false, false, intersection_towardsLine_normalized, -forward_insidePlane_normalized, intersectionPoint, color, radius, linesWidth, text, false, displayAndReturn_radInsteadOfDeg, coneLength, true, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
                obtuseAngle = UtilitiesDXXL_Measurements.Angle(false, false, false, intersection_towardsLine_normalized, forward_insidePlane_normalized, intersectionPoint, color_lowerAlpha, radius, 0.0f, null, false, displayAndReturn_radInsteadOfDeg, coneLength, true, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
            }

            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped())
            {
                return ReturnAngleOfCorrectAcuteObtuseType(obtuseAngle, acuteAngle, returnObtuseAngleOver90deg);
            }

            //Draw plane:
            float subSegments_signFlipsInterpretation = (-0.1f) * planeSize_nonExtended; //-> "negative sign" means "fixed world space size of segments"
            float anchorVisualizationSize = 0.5f * planeSize_nonExtended;
            DrawShapes.Plane(planeOrigin, planeNormal_normalized, intersectionPoint, planeColor, planeSize_nonExtended, planeSize_nonExtended, forward_insidePlane_normalized, 0.0f, null, subSegments_signFlipsInterpretation, false, anchorVisualizationSize, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);

            Vector3 textUp_normalized = Vector3.Cross(planeNormal_normalized, forward_insidePlane_normalized);
            if ((planeName == null) || (planeName == "")) { planeName = "plane"; }
            UtilitiesDXXL_Text.Write(planeName, intersectionPoint - 0.05f * planeSize_nonExtended * forward_insidePlane_normalized, planeColor, 0.05f * planeSize_nonExtended, -forward_insidePlane_normalized, textUp_normalized, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);

            //Draw line:
            UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
            DrawBasics.VectorFrom(lineOrigin, lineDirection, defaultColor1, widthOfDirVector, DrawText.MarkupColor(lineIdentifyingText, colorOfAttachments), 0.17f, false, false, default(Vector3), true, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();

            float lineExtentionPerSide = Mathf.Max(minimumLineLength_forAngleLineToPlane, 1.1f * distance_intersection_to_lineOrigin);
            Line_fadeableAnimSpeed.InternalDraw(lineOrigin - line.direction_normalized * lineExtentionPerSide, lineOrigin + line.direction_normalized * lineExtentionPerSide, colorOfProlongedLine, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            UtilitiesDXXL_Measurements.WriteLineNameAtProjectionPlumbPos(false, lineName, "line", " ", intersection_to_lineOrigin, distance_intersection_to_lineOrigin, lineOrigins_projectionOnPlane, line, intersectionPoint, 0.08f * radius, colorOfProlongedLine, false, durationInSec, hiddenByNearerObjects);

            //Enforce line at radius of angleCone:
            Vector3 posOnLine_whereAngleConeTouches = intersectionPoint + intersection_towardsLine_normalized * radius;
            float halfLengthOfEnforcementLine = radius * 0.055f + 0.5f * linesWidth;
            float widthOfEnforcementLine = radius * 0.003f;
            Line_fadeableAnimSpeed.InternalDraw(posOnLine_whereAngleConeTouches - intersection_towardsLine_normalized * halfLengthOfEnforcementLine, posOnLine_whereAngleConeTouches + intersection_towardsLine_normalized * halfLengthOfEnforcementLine, color, widthOfEnforcementLine, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            //Draw intersection pos coordinate:
            UtilitiesDXXL_DrawBasics.Set_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM_reversible(0);
            DrawBasics.Point(intersectionPoint, "<size=5>intersection<br>position</size>", color, 0.025f * planeSize_nonExtended, 0.0f, color, default, false, true, false, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_DrawBasics.Reverse_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM();

            //Draw circles at intersection pos:
            DrawShapes.Decagon(intersectionPoint, radius * 0.02f, UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 1.0f), planeNormal, default(Vector3), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
            DrawShapes.Decagon(intersectionPoint, radius * 0.04f, UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.65f), planeNormal, default(Vector3), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
            DrawShapes.Circle(intersectionPoint, radius * 0.06f, UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.35f), planeNormal, default(Vector3), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
            DrawShapes.Circle(intersectionPoint, radius * 0.08f, UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.15f), planeNormal, default(Vector3), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);

            return ReturnAngleOfCorrectAcuteObtuseType(obtuseAngle, acuteAngle, returnObtuseAngleOver90deg);
        }

        static float ReturnAngleOfCorrectAcuteObtuseType(float obtuseAngle, float acuteAngle, bool returnObtuseAngleOver90deg)
        {
            if (returnObtuseAngleOver90deg)
            {
                return obtuseAngle;
            }
            else
            {
                return acuteAngle;
            }
        }

        public static float AnglePlaneToPlane(Transform plane1Transform, Transform plane2Transform, Color color = default(Color), float linesWidth = 0.0f, string text = null, string plane1Name = null, string plane2Name = null, bool returnObtuseAngleOver90deg = false, bool displayAndReturn_radInsteadOfDeg = false, float coneLength = 0.13f, bool addTextForAlternativeAngleUnit = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(plane1Transform, "plane1Transform")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(plane2Transform, "plane2Transform")) { return 0.0f; }
            return AnglePlaneToPlane(plane1Transform.position, plane1Transform.up, plane2Transform.position, plane2Transform.up, color, linesWidth, text, plane1Name, plane2Name, returnObtuseAngleOver90deg, displayAndReturn_radInsteadOfDeg, coneLength, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
        }

        public static float AnglePlaneToPlane(Plane plane1, Plane plane2, Vector3 drawPositioinAsPlumb = default(Vector3), Color color = default(Color), float linesWidth = 0.0f, string text = null, string plane1Name = null, string plane2Name = null, bool returnObtuseAngleOver90deg = false, bool displayAndReturn_radInsteadOfDeg = false, float coneLength = 0.13f, bool addTextForAlternativeAngleUnit = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(drawPositioinAsPlumb, "drawPositioinAsPlumb")) { return 0.0f; }
            drawPositioinAsPlumb = UtilitiesDXXL_Math.OverwriteDefaultVectors(drawPositioinAsPlumb, Vector3.zero);

            Vector3 closestPointOnPlane1 = plane1.ClosestPointOnPlane(drawPositioinAsPlumb);
            Vector3 closestPointOnPlane2 = plane2.ClosestPointOnPlane(drawPositioinAsPlumb);
            return AnglePlaneToPlane(closestPointOnPlane1, plane1.normal, closestPointOnPlane2, plane2.normal, color, linesWidth, text, plane1Name, plane2Name, returnObtuseAngleOver90deg, displayAndReturn_radInsteadOfDeg, coneLength, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
        }

        static InternalDXXL_Plane plane1 = new InternalDXXL_Plane();
        static InternalDXXL_Plane plane2 = new InternalDXXL_Plane();
        static InternalDXXL_Line intersectionLine = new InternalDXXL_Line();
        public static float AnglePlaneToPlane(Vector3 plane1Origin, Vector3 plane1Normal, Vector3 plane2Origin, Vector3 plane2Normal, Color color = default(Color), float linesWidth = 0.0f, string text = null, string plane1Name = null, string plane2Name = null, bool returnObtuseAngleOver90deg = false, bool displayAndReturn_radInsteadOfDeg = false, float coneLength = 0.13f, bool addTextForAlternativeAngleUnit = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return 0.0f; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(plane1Origin, "plane1Origin")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(plane1Normal, "plane1Normal")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(plane2Origin, "plane2Origin")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(plane2Normal, "plane2Normal")) { return 0.0f; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);
            if (UtilitiesDXXL_Math.ApproximatelyZero(plane1Normal))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(plane1Origin, "[<color=#adadadFF><icon=logMessage></color> 'plane1Normal' is zero. AnglePlaneToPlane measure operation not executed.]<br>[plane1Origin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.PointFallback(plane2Origin, "[<color=#adadadFF><icon=logMessage></color> 'plane1Normal' is zero. AnglePlaneToPlane measure operation not executed.]<br>[plane2Origin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return 0.0f;
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(plane2Normal))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(plane1Origin, "[<color=#adadadFF><icon=logMessage></color> 'plane2Normal' is zero. AnglePlaneToPlane measure operation not executed.]<br>[plane1Origin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.PointFallback(plane2Origin, "[<color=#adadadFF><icon=logMessage></color> 'plane2Normal' is zero. AnglePlaneToPlane measure operation not executed.]<br>[plane2Origin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return 0.0f;
            }

            Vector3 plane1Normal_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(plane1Normal);
            Vector3 plane2Normal_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(plane2Normal);

            plane1.Recreate(plane1Origin, plane1Normal_normalized);
            plane2.Recreate(plane2Origin, plane2Normal_normalized);

            Color plane1Color = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(defaultColor1, 0.5f);
            Color plane2Color = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(defaultColor2, 0.5f);
            Color colorOfAdditionalAngle = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.55f);

            if ((plane1Name == null) || (plane1Name == ""))
            {
                plane1Name = "plane1";
            }
            if ((plane2Name == null) || (plane2Name == ""))
            {
                plane2Name = "plane2";
            }

            if (Mathf.Abs(Vector3.Dot(plane1Normal_normalized, plane2Normal_normalized)) > 0.999999f)
            {
                //-> planes are approximately parallel

                //thresholds:
                //"0.9999f" -> minDetectableAngle: 0.8deg
                //"0.99999f" -> minDetectableAngle: 0.26deg
                //"0.999999f" -> minDetectableAngle: 0.09deg

                Vector3 plane1Origins_projectionOnPlane2 = plane2.Get_perpProjectionOfPointOnPlane(plane1Origin);
                float distance = UtilitiesDXXL_Measurements.Distance(false, UtilitiesDXXL_Measurements.DistanceSpecifyingStringType.plane_plane, plane1Origin, plane1Origins_projectionOnPlane2, color, linesWidth, "[<color=#adadadFF><icon=logMessage></color> Planes are approximately parallel -> fallback from 'angle' to 'distance']<br>" + text, coneLength, 0.0f, durationInSec, hiddenByNearerObjects, false);
                float bigPlaneSize = Mathf.Max(distance, 1.0f);
                float smallPlaneSize = 0.2f * bigPlaneSize;
                float textSize = 0.2f * smallPlaneSize;

                Vector3 forwardInsidePlane_normalized = UtilitiesDXXL_Math.Get_aNormalizedVector_perpToGivenVector(plane1Normal_normalized);
                Vector3 textDir_normalized = forwardInsidePlane_normalized;
                Vector3 textUp_normalized = Vector3.Cross(forwardInsidePlane_normalized, plane1Normal_normalized);

                DrawShapes.Plane(plane1Origin, plane1Normal_normalized, plane2Origin, plane1Color, bigPlaneSize, bigPlaneSize, forwardInsidePlane_normalized, 0.0f, null, 10, false, 0.0f, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
                DrawShapes.Plane(plane1Origin, plane1Normal_normalized, default, plane1Color, smallPlaneSize, smallPlaneSize, forwardInsidePlane_normalized, 0.0f, null, 10, false, 0.0f, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_Text.Write(plane1Name, plane1Origin + textDir_normalized * (0.6f * smallPlaneSize), plane1Color, textSize, textDir_normalized, textUp_normalized, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);

                DrawShapes.Plane(plane2Origin, plane2Normal_normalized, plane1Origins_projectionOnPlane2, plane2Color, bigPlaneSize, bigPlaneSize, forwardInsidePlane_normalized, 0.0f, null, 10, false, 0.0f, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
                DrawShapes.Plane(plane1Origins_projectionOnPlane2, plane2Normal_normalized, default, plane2Color, smallPlaneSize, smallPlaneSize, forwardInsidePlane_normalized, 0.0f, null, 10, false, 0.0f, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_Text.Write(plane2Name, plane1Origins_projectionOnPlane2 + textDir_normalized * (0.6f * smallPlaneSize), plane2Color, textSize, textDir_normalized, textUp_normalized, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);

                return returnObtuseAngleOver90deg ? (displayAndReturn_radInsteadOfDeg ? (Mathf.Deg2Rad * 180.0f) : 180.0f) : 0.0f;
            }

            InternalDXXL_Plane.Calc_intersectionLine_ofTwoPlanes(ref intersectionLine, plane1, plane2);
            if (intersectionLine.ErrorLogForInvalidLineParameters() == false)
            {
                UtilitiesDXXL_DrawBasics.PointFallback(plane1Origin, "[<color=#ce0e0eFF><icon=logMessageError></color> Couldn't calculate intersectionLine of the the planes. AnglePlaneToPlane measure operation not executed.]<br>[plane1Origin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.PointFallback(plane2Origin, "[<color=#ce0e0eFF><icon=logMessageError></color> Couldn't calculate intersectionLine of the the planes. AnglePlaneToPlane measure operation not executed.]<br>[plane2Origin]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return 0.0f;
            }

            bool angleIsApproximately90Deg = (Mathf.Abs(Vector3.Dot(plane1Normal_normalized, plane2Normal_normalized)) < 0.0001f);

            Vector3 plane1origin_projectedOnLine = intersectionLine.Get_perpProjectionOfPoint_ontoThisLine(plane1Origin);
            Vector3 intersectionPos1_to_plane1Origin = plane1Origin - plane1origin_projectedOnLine;
            float distance_intersection1_to_plane1Origin = intersectionPos1_to_plane1Origin.magnitude;

            Vector3 plane2origin_projectedOnLine = intersectionLine.Get_perpProjectionOfPoint_ontoThisLine(plane2Origin);
            Vector3 intersectionPos2_to_plane2Origin = plane2Origin - plane2origin_projectedOnLine;
            float distance_intersection2_to_plane2Origin = intersectionPos2_to_plane2Origin.magnitude;

            Vector3 angleCenterPos;
            Vector3 intersection_towardsPlane1_normalized;
            Vector3 intersection_towardsPlane2_normalized;
            float radius = 0.2f;

            if (distance_intersection1_to_plane1Origin < 0.0001f)
            {
                if (distance_intersection2_to_plane2Origin < 0.0001f)
                {
                    //both planeOrigins lie on intersectionLine:
                    angleCenterPos = plane1Origin;
                    Vector3 aNormalizedVector_perpToLine = UtilitiesDXXL_Math.Get_aNormalizedVector_perpToGivenVector(intersectionLine.direction_normalized);
                    Quaternion seldomRotation_aroundLine = Quaternion.AngleAxis(UtilitiesDXXL_Math.arbitrarySeldomDir_precalced.x, intersectionLine.direction_normalized);
                    Vector3 aSeldomNormalizedVector_perpToLine = seldomRotation_aroundLine * aNormalizedVector_perpToLine;
                    intersection_towardsPlane1_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(plane1.Get_projectionOfVectorOntoPlane(aSeldomNormalizedVector_perpToLine));
                    if (angleIsApproximately90Deg)
                    {
                        Quaternion rotation90deg = Quaternion.AngleAxis(90.0f, intersectionLine.direction_normalized);
                        intersection_towardsPlane2_normalized = rotation90deg * intersection_towardsPlane1_normalized;
                    }
                    else
                    {
                        intersection_towardsPlane2_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(plane2.Get_projectionOfVectorOntoPlane(intersection_towardsPlane1_normalized));
                    }
                }
                else
                {
                    //plane1Origin lies on intersectionLine, but plane2Origin NOT:
                    radius = 0.5f * distance_intersection2_to_plane2Origin;
                    angleCenterPos = plane2origin_projectedOnLine;
                    intersection_towardsPlane2_normalized = intersectionPos2_to_plane2Origin / distance_intersection2_to_plane2Origin;
                    if (angleIsApproximately90Deg)
                    {
                        Quaternion rotation90deg = Quaternion.AngleAxis(90.0f, intersectionLine.direction_normalized);
                        intersection_towardsPlane1_normalized = rotation90deg * intersection_towardsPlane2_normalized;
                    }
                    else
                    {
                        intersection_towardsPlane1_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(plane1.Get_projectionOfVectorOntoPlane(intersection_towardsPlane2_normalized));
                    }
                }
            }
            else
            {
                //plane1Origin does NOT lie on intersectionLine:
                if (distance_intersection2_to_plane2Origin < 0.0001f)
                {
                    //plane2Origin lies on intersectionLine, but plane1Origin NOT:
                    radius = 0.5f * distance_intersection1_to_plane1Origin;
                }
                else
                {
                    //both planeOrigins do NOT lie on intersectionLine:
                    float averageDistance_intersections_to_planeOrigins = 0.5f * (distance_intersection1_to_plane1Origin + distance_intersection2_to_plane2Origin);
                    radius = 0.5f * averageDistance_intersections_to_planeOrigins;
                }

                angleCenterPos = plane1origin_projectedOnLine;
                intersection_towardsPlane1_normalized = intersectionPos1_to_plane1Origin / distance_intersection1_to_plane1Origin;
                if (angleIsApproximately90Deg)
                {
                    Quaternion rotation90deg = Quaternion.AngleAxis(90.0f, intersectionLine.direction_normalized);
                    intersection_towardsPlane2_normalized = rotation90deg * intersection_towardsPlane1_normalized;
                }
                else
                {
                    intersection_towardsPlane2_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(plane2.Get_projectionOfVectorOntoPlane(intersection_towardsPlane1_normalized));
                }
            }

            radius = Mathf.Max(radius, 0.2f);
            float approxPlane1Length;
            if (distance_intersection1_to_plane1Origin < 0.1f)
            {
                approxPlane1Length = 1.0f + (2.0f * radius);
            }
            else
            {
                approxPlane1Length = 1.2f * distance_intersection1_to_plane1Origin + (2.0f * radius);
            }

            float approxPlane2Length;
            if (distance_intersection2_to_plane2Origin < 0.1f)
            {
                approxPlane2Length = 1.0f + (2.0f * radius);
            }
            else
            {
                approxPlane2Length = 1.2f * distance_intersection2_to_plane2Origin + (2.0f * radius);
            }

            float approxPlaneLength = Mathf.Max(approxPlane1Length, approxPlane2Length);
            approxPlaneLength = Mathf.Max(approxPlaneLength, 2.5f * radius);
            approxPlaneLength = Mathf.Max(approxPlaneLength, 1.0f);

            float distance_betweenTheTwoProjectionsOntoTheIntersectionLine = (plane1origin_projectedOnLine - plane2origin_projectedOnLine).magnitude;
            float planeWidth_thatProtrudesTheTwoProjectionsOnIntersectionLine = 0.25f * distance_betweenTheTwoProjectionsOntoTheIntersectionLine;
            planeWidth_thatProtrudesTheTwoProjectionsOnIntersectionLine = Mathf.Max(planeWidth_thatProtrudesTheTwoProjectionsOnIntersectionLine, 1.0f);

            float angleDeg_towards1_to_towards2 = Vector3.Angle(intersection_towardsPlane1_normalized, intersection_towardsPlane2_normalized);

            float acuteAngle;
            float obtuseAngle;
            if (angleIsApproximately90Deg || angleDeg_towards1_to_towards2 < 90.0f)
            {
                //towards1-to-towards2 is acute:
                if (returnObtuseAngleOver90deg)
                {
                    acuteAngle = UtilitiesDXXL_Measurements.Angle(false, false, false, intersection_towardsPlane1_normalized, intersection_towardsPlane2_normalized, angleCenterPos, colorOfAdditionalAngle, radius, 0.0f, null, false, displayAndReturn_radInsteadOfDeg, coneLength, true, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
                    obtuseAngle = UtilitiesDXXL_Measurements.Angle(true, false, false, intersection_towardsPlane1_normalized, -intersection_towardsPlane2_normalized, angleCenterPos, color, radius, linesWidth, text, false, displayAndReturn_radInsteadOfDeg, coneLength, true, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
                }
                else
                {
                    acuteAngle = UtilitiesDXXL_Measurements.Angle(true, false, false, intersection_towardsPlane1_normalized, intersection_towardsPlane2_normalized, angleCenterPos, color, radius, linesWidth, text, false, displayAndReturn_radInsteadOfDeg, coneLength, true, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
                    obtuseAngle = UtilitiesDXXL_Measurements.Angle(false, false, false, intersection_towardsPlane1_normalized, -intersection_towardsPlane2_normalized, angleCenterPos, colorOfAdditionalAngle, radius, 0.0f, null, false, displayAndReturn_radInsteadOfDeg, coneLength, true, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
                }
            }
            else
            {
                //towards1-to-towards2 is obtuse:
                if (returnObtuseAngleOver90deg)
                {
                    acuteAngle = UtilitiesDXXL_Measurements.Angle(false, false, false, intersection_towardsPlane1_normalized, -intersection_towardsPlane2_normalized, angleCenterPos, colorOfAdditionalAngle, radius, 0.0f, null, false, displayAndReturn_radInsteadOfDeg, coneLength, true, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
                    obtuseAngle = UtilitiesDXXL_Measurements.Angle(true, false, false, intersection_towardsPlane1_normalized, intersection_towardsPlane2_normalized, angleCenterPos, color, radius, linesWidth, text, false, displayAndReturn_radInsteadOfDeg, coneLength, true, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
                }
                else
                {
                    acuteAngle = UtilitiesDXXL_Measurements.Angle(true, false, false, intersection_towardsPlane1_normalized, -intersection_towardsPlane2_normalized, angleCenterPos, color, radius, linesWidth, text, false, displayAndReturn_radInsteadOfDeg, coneLength, true, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
                    obtuseAngle = UtilitiesDXXL_Measurements.Angle(false, false, false, intersection_towardsPlane1_normalized, intersection_towardsPlane2_normalized, angleCenterPos, colorOfAdditionalAngle, radius, 0.0f, null, false, displayAndReturn_radInsteadOfDeg, coneLength, true, addTextForAlternativeAngleUnit, durationInSec, hiddenByNearerObjects);
                }
            }

            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped())
            {
                return ReturnAngleOfCorrectAcuteObtuseType(obtuseAngle, acuteAngle, returnObtuseAngleOver90deg);
            }

            //Draw circles as angle display mounting point:
            Vector3 position_ofMountingPointDisplayCircle = angleCenterPos + intersection_towardsPlane1_normalized * radius;
            Vector3 normal_ofMountingPointDisplayCircle = plane1Normal_normalized;
            DrawShapes.Decagon(position_ofMountingPointDisplayCircle, radius * 0.02f, UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 1.0f), normal_ofMountingPointDisplayCircle, default(Vector3), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
            DrawShapes.Decagon(position_ofMountingPointDisplayCircle, radius * 0.04f, UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.65f), normal_ofMountingPointDisplayCircle, default(Vector3), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
            DrawShapes.Circle(position_ofMountingPointDisplayCircle, radius * 0.06f, UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.35f), normal_ofMountingPointDisplayCircle, default(Vector3), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
            DrawShapes.Circle(position_ofMountingPointDisplayCircle, radius * 0.08f, UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.15f), normal_ofMountingPointDisplayCircle, default(Vector3), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);

            //Draw planes:
            Vector3 pos_between_theTwoLineProjectedPlaneOrigins = 0.5f * (plane1origin_projectedOnLine + plane2origin_projectedOnLine);
            float anchorVisualizationSize = 0.5f * planeWidth_thatProtrudesTheTwoProjectionsOnIntersectionLine;
            float subSegments_signFlipsInterpretation = (-0.1f) * planeWidth_thatProtrudesTheTwoProjectionsOnIntersectionLine; //-> "negative sign" means "fixed world space size of segments"

            DrawShapes.Plane(plane1Origin, plane1Normal_normalized, plane2origin_projectedOnLine, plane1Color, planeWidth_thatProtrudesTheTwoProjectionsOnIntersectionLine, planeWidth_thatProtrudesTheTwoProjectionsOnIntersectionLine, intersection_towardsPlane1_normalized, 0.0f, null, subSegments_signFlipsInterpretation, false, anchorVisualizationSize, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_Text.Write(plane1Name, angleCenterPos - 0.05f * approxPlaneLength * intersection_towardsPlane1_normalized, plane1Color, 0.05f * approxPlaneLength, -intersection_towardsPlane1_normalized, intersectionLine.direction_normalized, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);

            DrawShapes.Plane(plane2Origin, plane2Normal_normalized, plane1origin_projectedOnLine, plane2Color, planeWidth_thatProtrudesTheTwoProjectionsOnIntersectionLine, planeWidth_thatProtrudesTheTwoProjectionsOnIntersectionLine, intersection_towardsPlane2_normalized, 0.0f, null, subSegments_signFlipsInterpretation, false, anchorVisualizationSize, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_Text.Write(plane2Name, angleCenterPos - 0.05f * approxPlaneLength * intersection_towardsPlane2_normalized, plane2Color, 0.05f * approxPlaneLength, -intersection_towardsPlane2_normalized, intersectionLine.direction_normalized, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);

            //Draw additional emphasizing planes around intersectionline:
            float length_ofAdditialPlanes = 0.3f * radius;
            DrawShapes.Plane(plane1origin_projectedOnLine, plane1Normal_normalized, plane2origin_projectedOnLine, plane1Color, planeWidth_thatProtrudesTheTwoProjectionsOnIntersectionLine, length_ofAdditialPlanes, intersection_towardsPlane1_normalized, 0.0f, null, 10, false, 0.0f, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
            DrawShapes.Plane(plane2origin_projectedOnLine, plane2Normal_normalized, plane1origin_projectedOnLine, plane2Color, planeWidth_thatProtrudesTheTwoProjectionsOnIntersectionLine, length_ofAdditialPlanes, intersection_towardsPlane2_normalized, 0.0f, null, 10, false, 0.0f, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);

            //Draw the emphasized intersection line:
            float lineWidth_ofIntersectionLineVisualization = 0.01f * radius;
            Vector3 intersectionLineVector_withLengthOfHalfWidthOfDisplayedPlanes = intersectionLine.direction_normalized * 0.5f * UtilitiesDXXL_Shapes.finalWidth_ofLastDrawnPlane;
            DrawBasics.Line(pos_between_theTwoLineProjectedPlaneOrigins - intersectionLineVector_withLengthOfHalfWidthOfDisplayedPlanes, pos_between_theTwoLineProjectedPlaneOrigins + intersectionLineVector_withLengthOfHalfWidthOfDisplayedPlanes, color, lineWidth_ofIntersectionLineVisualization, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            return ReturnAngleOfCorrectAcuteObtuseType(obtuseAngle, acuteAngle, returnObtuseAngleOver90deg);
        }

        public static void DistanceThreshold(Vector3 startPos, Vector3 endPos, float thresholdDistance, string text = null, bool displayDistanceAlsoAsText = false, float lineWidth = 0.0f, bool exactlyThresholdLength_countsAsShorter = true, float endPlates_size = 0.0f, DrawBasics.LineStyle overwriteStyle_forNear = DrawBasics.LineStyle.electricNoise, DrawBasics.LineStyle overwriteStyle_forFar = DrawBasics.LineStyle.solid, Color overwriteColor_forNear = default(Color), Color overwriteColor_forFar = default(Color), Vector3 customAmplitudeAndTextDir = default(Vector3), float enlargeSmallTextToThisMinTextSize = 0.005f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(thresholdDistance, "thresholdDistance")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(startPos, "startPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(endPos, "endPos")) { return; }

            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(startPos, endPos))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(startPos, "[<color=#adadadFF><icon=logMessage></color> DistanceThreshold with distance of 0]<br>" + text, UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor_forNear, UtilitiesDXXL_Colors.red_boolFalse), lineWidth, durationInSec, hiddenByNearerObjects);
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
            UtilitiesDXXL_DrawBasics.Line(startPos, endPos, usedColor, lineWidth, text, usedLineStyle, stylePatternScaleFactor, 0.0f, null, customAmplitudeAndTextDir, false, 0.0f, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, true, true, null, false, endPlates_size);
            UtilitiesDXXL_DrawBasics.Reverse_relSizeOfTextOnLines();
            UtilitiesDXXL_DrawBasics.Reverse_shiftTextPosOnLines_toNonIntersecting();
        }

        public static void DistanceThresholds(Vector3 startPos, Vector3 endPos, float smallerThresholdDistance, float biggerThresholdDistance, string text = null, bool displayDistanceAlsoAsText = false, float lineWidth = 0.0f, bool exactlyThresholdLength_countsAsShorter = true, float endPlates_size = 0.0f, DrawBasics.LineStyle overwriteStyle_forNear = DrawBasics.LineStyle.electricNoise, DrawBasics.LineStyle overwriteStyle_forMiddle = DrawBasics.LineStyle.electricImpulses, DrawBasics.LineStyle overwriteStyle_forFar = DrawBasics.LineStyle.solid, Color overwriteColor_forNear = default(Color), Color overwriteColor_forMiddle = default(Color), Color overwriteColor_forFar = default(Color), Vector3 customAmplitudeAndTextDir = default(Vector3), float enlargeSmallTextToThisMinTextSize = 0.005f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(smallerThresholdDistance, "smallerThresholdDistance")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(biggerThresholdDistance, "biggerThresholdDistance")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(startPos, "startPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(endPos, "endPos")) { return; }

            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(startPos, endPos))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(startPos, "[<color=#adadadFF><icon=logMessage></color> DistanceThresholds with distance of 0]<br>" + text, UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor_forNear, UtilitiesDXXL_Colors.red_boolFalse), lineWidth, durationInSec, hiddenByNearerObjects);
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
            UtilitiesDXXL_DrawBasics.Line(startPos, endPos, usedColor, lineWidth, text, usedLineStyle, stylePatternScaleFactor, 0.0f, null, customAmplitudeAndTextDir, false, 0.0f, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, true, true, null, false, endPlates_size);
            UtilitiesDXXL_DrawBasics.Reverse_relSizeOfTextOnLines();
            UtilitiesDXXL_DrawBasics.Reverse_shiftTextPosOnLines_toNonIntersecting();
        }

    }

}
