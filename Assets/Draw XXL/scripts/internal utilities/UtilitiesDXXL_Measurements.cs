namespace DrawXXL
{
    using UnityEngine;

    public class UtilitiesDXXL_Measurements
    {
        public enum DistanceSpecifyingStringType { point_point, point_line, line_line, point_ll_point, point_l_I_l_point, point_plane, plane_plane };

        static float angleDeg_markingALineBreakAfterAFloatPlusTheDegreeSign = 45.5f; //-> is roughly optimized that a float with all digits plus a "°" are in one line, then the linebreak for the rad-display. Though not fully stable in this regard.

        public static float Distance(bool is2D, DistanceSpecifyingStringType distanceSpecifyingStringType, Vector3 from, Vector3 to, Color color, float lineWidth, string text, float coneLength, float enlargeSmallTextToThisMinTextSize, float durationInSec, bool hiddenByNearerObjects, bool skipDraw)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(from, "from")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(to, "to")) { return 0.0f; }

            //fallback for "distance=zero" -> The called "Draw.Vector()" function already displays a comprehensible fallback

            Vector3 startToEnd = to - from;
            float distance = startToEnd.magnitude;
            if (skipDraw) { return distance; }
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return distance; }

            lineWidth = UtilitiesDXXL_Math.AbsNonZeroValue(lineWidth);

            //assuring that text is always from left to right:
            Vector3 drawnVectorsStartPos;
            Vector3 drawnVectorsEndPos;
            if (from.x < to.x)
            {
                drawnVectorsStartPos = from;
                drawnVectorsEndPos = to;
            }
            else
            {
                drawnVectorsStartPos = to;
                drawnVectorsEndPos = from;
            }

            string finalTextAtLine = string.IsNullOrEmpty(text) ? (GetDistanceSpecifyingDistancePrefixString(distanceSpecifyingStringType) + distance) : (GetDistanceSpecifyingDistancePrefixString(distanceSpecifyingStringType) + distance + "<br><br>" + text);

            UtilitiesDXXL_DrawBasics.Set_relSizeOfTextOnLines_reversible(0.65f);
            UtilitiesDXXL_DrawBasics.Set_shiftTextPosOnLines_toNonIntersecting_reversible(true);
            DrawBasics.Vector(drawnVectorsStartPos, drawnVectorsEndPos, color, lineWidth, finalTextAtLine, coneLength, true, is2D, default(Vector3), false, enlargeSmallTextToThisMinTextSize, false, 0.0f, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_DrawBasics.Reverse_relSizeOfTextOnLines();
            UtilitiesDXXL_DrawBasics.Reverse_shiftTextPosOnLines_toNonIntersecting();

            Color anchorPoint1Color = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawMeasurements.defaultColor1, 0.3f);
            Color anchorPoint2Color = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawMeasurements.defaultColor2, 0.3f);
            DrawBasics.Point(from, null, anchorPoint1Color, 0.1f * distance, 0.0f, anchorPoint1Color, Quaternion.identity, false, true, is2D, durationInSec, hiddenByNearerObjects);
            DrawBasics.Point(to, null, anchorPoint2Color, 0.1f * distance, 0.0f, anchorPoint2Color, Quaternion.identity, false, true, is2D, durationInSec, hiddenByNearerObjects);

            return distance;
        }

        static string GetDistanceSpecifyingDistancePrefixString(DistanceSpecifyingStringType distanceSpecifyingStringType)
        {
            switch (distanceSpecifyingStringType)
            {
                case DistanceSpecifyingStringType.point_point:
                    return "<size=2>POINT-POINT</size>distance =<br>";

                case DistanceSpecifyingStringType.point_line:
                    return "<size=2>POINT-LINE</size>distance =<br>";

                case DistanceSpecifyingStringType.line_line:
                    return "<size=2>LINE-LINE</size>distance =<br>";

                case DistanceSpecifyingStringType.point_ll_point:
                    return "<size=2>POINT||POINT</size>distance =<br>";

                case DistanceSpecifyingStringType.point_l_I_l_point:
                    return "<size=2>POINT|=|POINT</size>distance =<br>";

                case DistanceSpecifyingStringType.point_plane:
                    return "<size=2>POINT-PLANE</size>distance =<br>";

                case DistanceSpecifyingStringType.plane_plane:
                    return "<size=2>PLANE-PLANE</size>distance =<br>";

                default:
                    return "distance =<br>";
            }
        }


        public static float Angle(bool boldTextDisplay, bool is2D, bool pointerAtBothSides, Vector3 from, Vector3 to, Vector3 turnCenter, Color color, float forceRadius, float lineWidth, string text, bool useReflexAngleOver180deg, bool displayRadInsteadOfDeg, float coneLength, bool drawBoundaryLines, bool addTextForAlternativeAngleUnit, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(from, "from")) { return 0.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(to, "to")) { return 0.0f; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(turnCenter, "turnCenter")) { return 0.0f; }

            lineWidth = UtilitiesDXXL_Math.AbsNonZeroValue(lineWidth);

            if (UtilitiesDXXL_Math.ApproximatelyZero(from))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(turnCenter, "[<color=#adadadFF><icon=logMessage></color> Angle with startVectorLength of 0]<br>" + text, color, lineWidth, durationInSec, hiddenByNearerObjects);
                return 0.0f;
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(to))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(turnCenter, "[<color=#adadadFF><icon=logMessage></color> Angle with endVectorLength of 0]<br>" + text, color, lineWidth, durationInSec, hiddenByNearerObjects);
                return 0.0f;
            }

            Vector3 from_scaledIntoRegionOfFloatPrecision = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(from);
            Vector3 to_scaledIntoRegionOfFloatPrecision = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(to);

            if (UtilitiesDXXL_Math.GetBiggestAbsComponent(from_scaledIntoRegionOfFloatPrecision) < 0.0001f)
            {
                UtilitiesDXXL_DrawBasics.PointFallback(turnCenter, "[<color=#adadadFF><icon=logMessage></color> Angle with startVectorLength near 0]<br>" + text, color, lineWidth, durationInSec, hiddenByNearerObjects);
                return 0.0f;
            }

            if (UtilitiesDXXL_Math.GetBiggestAbsComponent(to_scaledIntoRegionOfFloatPrecision) < 0.0001f)
            {
                UtilitiesDXXL_DrawBasics.PointFallback(turnCenter, "[<color=#adadadFF><icon=logMessage></color> Angle with endVectorLength near 0]<br>" + text, color, lineWidth, durationInSec, hiddenByNearerObjects);
                return 0.0f;
            }

            float angleDeg = Vector3.Angle(from_scaledIntoRegionOfFloatPrecision, to_scaledIntoRegionOfFloatPrecision);
            if (useReflexAngleOver180deg)
            {
                angleDeg = 360.0f - angleDeg;
            }
            float angleRad = angleDeg * Mathf.Deg2Rad;
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped())
            {
                return ReturnAngleInCorrectUnit(angleRad, angleDeg, displayRadInsteadOfDeg);
            }

            Color smallRepresenationColor = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.5f);
            string mainValueRepresentationText;
            string minorValueRepresentationText = null;
            if (displayRadInsteadOfDeg)
            {
                mainValueRepresentationText = "<size=7>" + angleRad + " rad</size>";
                if (addTextForAlternativeAngleUnit)
                {
                    minorValueRepresentationText = "<color=#" + ColorUtility.ToHtmlStringRGBA(smallRepresenationColor) + "><size=4>(" + angleDeg + "°)</size></color>";
                }
            }
            else
            {
                mainValueRepresentationText = "<size=10>" + angleDeg + "</size>°";
                if (addTextForAlternativeAngleUnit)
                {
                    minorValueRepresentationText = "<color=#" + ColorUtility.ToHtmlStringRGBA(smallRepresenationColor) + "><size=4>(" + angleRad + " rad)</size></color>";
                }
            }

            string angleText;
            if (UtilitiesDXXL_Math.ApproximatelyZero(angleDeg))
            {
                //-> fallback information:
                angleText = "Angle measurement with result: " + mainValueRepresentationText + minorValueRepresentationText + "<br>" + text;
            }
            else
            {
                if (boldTextDisplay)
                {
                    angleText = "<sw=40000>" + mainValueRepresentationText + minorValueRepresentationText + "</sw><br>" + text;
                }
                else
                {
                    angleText = "" + mainValueRepresentationText + minorValueRepresentationText + "<br>" + text;
                }
            }

            if (is2D)
            {
                DrawBasics2D.VectorCircled(turnCenter, from, to, color, forceRadius, lineWidth, angleText, useReflexAngleOver180deg, turnCenter.z, coneLength, false, pointerAtBothSides, angleDeg_markingALineBreakAfterAFloatPlusTheDegreeSign, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                DrawBasics.VectorCircled(turnCenter, from, to, color, forceRadius, lineWidth, angleText, useReflexAngleOver180deg, coneLength, pointerAtBothSides, false, true, angleDeg_markingALineBreakAfterAFloatPlusTheDegreeSign, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, durationInSec, hiddenByNearerObjects);
            }

            if (drawBoundaryLines)
            {
                if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(forceRadius, "forceRadius"))
                {
                    return ReturnAngleInCorrectUnit(angleRad, angleDeg, displayRadInsteadOfDeg);
                }

                Vector3 from_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(from, out float from_magnitude);
                Vector3 to_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(to);
                float radius = UtilitiesDXXL_Math.ApproximatelyZero(forceRadius) ? from_magnitude : forceRadius;
                radius = Mathf.Abs(radius);

                if (radius > 0.0f)
                {
                    Color colorOfFromLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawMeasurements.defaultColor1, 0.2f);
                    Color colorOfToLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawMeasurements.defaultColor2, 0.2f);
                    Line_fadeableAnimSpeed.InternalDraw(turnCenter, turnCenter + from_normalized * radius * 1.1f, colorOfFromLine, 0.0f, null, DrawBasics.LineStyle.dashedLong, radius * 1.022f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                    Line_fadeableAnimSpeed.InternalDraw(turnCenter, turnCenter + to_normalized * radius * 1.1f, colorOfToLine, 0.0f, null, DrawBasics.LineStyle.dashedLong, radius * 1.022f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

                    Vector3 turnAxis = Vector3.Cross(from_normalized, to_normalized);
                    if (UtilitiesDXXL_Math.ApproximatelyZero(turnAxis) == false)
                    {
                        Vector3 turnAxis_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(turnAxis);
                        float lenghtOfTurnAxisDisplayLine = radius * 0.1f;
                        float halfLenghtOfTurnAxisDisplayLine = 0.5f * lenghtOfTurnAxisDisplayLine;
                        Color colorOfTurnAxisDisplay = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.2f);
                        Line_fadeableAnimSpeed.InternalDraw(turnCenter - turnAxis_normalized * halfLenghtOfTurnAxisDisplayLine, turnCenter + turnAxis_normalized * halfLenghtOfTurnAxisDisplayLine, colorOfTurnAxisDisplay, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                    }
                }
            }

            return ReturnAngleInCorrectUnit(angleRad, angleDeg, displayRadInsteadOfDeg);
        }

        static float ReturnAngleInCorrectUnit(float angleRad, float angleDeg, bool displayRadInsteadOfDeg)
        {
            if (displayRadInsteadOfDeg)
            {
                return angleRad;
            }
            else
            {
                return angleDeg;
            }
        }

        public static void WriteLineNameAtProjectionPlumbPos(bool is2D, string lineName, string fallbackLineName, string stringPrefix, Vector3 projectionToLineOrigin, float distance_projectionToLineOrigin, Vector3 pointPreProjection_forTextUp, InternalDXXL_Line line, Vector3 pointsProjectionOntoLine, float textSize, Color color, bool alignedCenter, float durationInSec, bool hiddenByNearerObjects)
        {
            // float lineDirection_magnitude = line.direction.magnitude;
            if (distance_projectionToLineOrigin > line.length || UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(projectionToLineOrigin, line.direction_normalized))
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(distance_projectionToLineOrigin) == false)
                {
                    Vector3 projectionToLineOrigin_normalized = projectionToLineOrigin / distance_projectionToLineOrigin;
                    string lineIdentifyingText = ((lineName == null) || (lineName == "")) ? fallbackLineName : lineName;
                    lineIdentifyingText = stringPrefix + lineIdentifyingText;
                    textSize = Mathf.Max(textSize, 0.001f);
                    Vector3 textDir = line.origin - pointsProjectionOntoLine;
                    if (UtilitiesDXXL_Math.GetBiggestAbsComponent(textDir) < 0.0001f)
                    {
                        textDir = UtilitiesDXXL_Math.arbitrarySeldomDir_normalized_precalced;
                    }
                    Vector3 textUp = pointsProjectionOntoLine - pointPreProjection_forTextUp;
                    if (UtilitiesDXXL_Math.GetBiggestAbsComponent(textUp) < 0.0001f)
                    {
                        textUp = UtilitiesDXXL_Math.arbitrarySeldomDir2_precalced;
                    }
                    bool textDirHasBeenSwitched = false;
                    if (is2D)
                    {
                        if (textUp.y > 0.0f)
                        {
                            if (textDir.x < 0.0f)
                            {
                                textDir = -textDir;
                                textDirHasBeenSwitched = true;
                            }
                        }
                        else
                        {
                            if (textDir.x > 0.0f)
                            {
                                textDir = -textDir;
                                textDirHasBeenSwitched = true;
                            }
                        }
                    }
                    UtilitiesDXXL_Text.Write(lineIdentifyingText, pointsProjectionOntoLine, color, textSize, textDir, textUp, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, true, is2D, false);
                    float lengthOfLongestLine_ofLineIdentifyingText = DrawText.parsedTextSpecs.widthOfLongestLine;
                    if (alignedCenter == false)
                    {
                        lengthOfLongestLine_ofLineIdentifyingText = 0.0f;
                    }
                    Vector3 textPos = textDirHasBeenSwitched ? (pointsProjectionOntoLine + projectionToLineOrigin_normalized * (0.5f * lengthOfLongestLine_ofLineIdentifyingText)) : (pointsProjectionOntoLine - projectionToLineOrigin_normalized * (0.5f * lengthOfLongestLine_ofLineIdentifyingText));
                    UtilitiesDXXL_Text.WriteFramed(lineIdentifyingText, textPos, color, textSize, textDir, textUp, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
                }
            }
        }

        public static void Draw90degSymbol(float distance, Vector3 pointsProjectionOntoLine, Vector3 pointPreProjection, Vector3 lineDirPerpToMeasuredDistance_normalized, Color color, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(distance) == false)
            {
                float size_of90DegSymbol = 0.25f;
                size_of90DegSymbol = Mathf.Min(size_of90DegSymbol, 0.25f * distance);
                Vector3 projectionToPoint = pointPreProjection - pointsProjectionOntoLine;
                Vector3 projectionToPoint_normalized = projectionToPoint / distance;
                Vector3 symbolFor90deg_firstPoint = pointsProjectionOntoLine + lineDirPerpToMeasuredDistance_normalized * size_of90DegSymbol;
                Vector3 symbolFor90deg_middlePoint = pointsProjectionOntoLine + lineDirPerpToMeasuredDistance_normalized * size_of90DegSymbol + projectionToPoint_normalized * size_of90DegSymbol;
                Vector3 symbolFor90deg_thirdPoint = pointsProjectionOntoLine + projectionToPoint_normalized * size_of90DegSymbol;
                Line_fadeableAnimSpeed.InternalDraw(symbolFor90deg_firstPoint, symbolFor90deg_middlePoint, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                Line_fadeableAnimSpeed.InternalDraw(symbolFor90deg_thirdPoint, symbolFor90deg_middlePoint, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }
        }

        public static void ChooseColorAndStyleForDistanceThresholdLine(ref Color usedColor, ref DrawBasics.LineStyle usedLineStyle, float distance, float thresholdDistance, bool exactlyThresholdLength_countsAsShorter, DrawBasics.LineStyle overwriteStyle_forNear, DrawBasics.LineStyle overwriteStyle_forFar, Color overwriteColor_forNear, Color overwriteColor_forFar)
        {
            if (exactlyThresholdLength_countsAsShorter)
            {
                if (distance <= thresholdDistance)
                {
                    usedColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor_forNear, UtilitiesDXXL_Colors.red_boolFalse);
                    usedLineStyle = overwriteStyle_forNear;
                }
                else
                {
                    usedColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor_forFar, UtilitiesDXXL_Colors.green_boolTrue);
                    usedLineStyle = overwriteStyle_forFar;
                }
            }
            else
            {
                if (distance < thresholdDistance)
                {
                    usedColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor_forNear, UtilitiesDXXL_Colors.red_boolFalse);
                    usedLineStyle = overwriteStyle_forNear;
                }
                else
                {
                    usedColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor_forFar, UtilitiesDXXL_Colors.green_boolTrue);
                    usedLineStyle = overwriteStyle_forFar;
                }
            }
        }


        public static void ChooseColorAndStyleForDistanceThresholdsLine(ref Color usedColor, ref DrawBasics.LineStyle usedLineStyle, float distance, float smallerThresholdDistance, float biggerThresholdDistance, bool exactlyThresholdLength_countsAsShorter, DrawBasics.LineStyle overwriteStyle_forNear, DrawBasics.LineStyle overwriteStyle_forMiddle, DrawBasics.LineStyle overwriteStyle_forFar, Color overwriteColor_forNear, Color overwriteColor_forMiddle, Color overwriteColor_forFar)
        {
            if (exactlyThresholdLength_countsAsShorter)
            {
                if (distance <= smallerThresholdDistance)
                {
                    usedColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor_forNear, UtilitiesDXXL_Colors.red_lineThresholdFarDistance);
                    usedLineStyle = overwriteStyle_forNear;
                }
                else
                {
                    if (distance <= biggerThresholdDistance)
                    {
                        usedColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor_forMiddle, UtilitiesDXXL_Colors.orange_lineThresholdMiddleDistance);
                        usedLineStyle = overwriteStyle_forMiddle;
                    }
                    else
                    {
                        usedColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor_forFar, UtilitiesDXXL_Colors.green_lineThresholdNearDistance);
                        usedLineStyle = overwriteStyle_forFar;
                    }
                }
            }
            else
            {
                if (distance < smallerThresholdDistance)
                {
                    usedColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor_forNear, UtilitiesDXXL_Colors.red_lineThresholdFarDistance);
                    usedLineStyle = overwriteStyle_forNear;
                }
                else
                {
                    if (distance < biggerThresholdDistance)
                    {
                        usedColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor_forMiddle, UtilitiesDXXL_Colors.orange_lineThresholdMiddleDistance);
                        usedLineStyle = overwriteStyle_forMiddle;
                    }
                    else
                    {
                        usedColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor_forFar, UtilitiesDXXL_Colors.green_lineThresholdNearDistance);
                        usedLineStyle = overwriteStyle_forFar;
                    }
                }
            }
        }

        public static void WriteOrthoViewDirNameAtProjectionPlumbPos(string lineName, Vector3 pointPreProjection_forTextUp, Vector3 pointsProjectionOntoLine, Vector3 textDir, float textSize, Color color, float durationInSec, bool hiddenByNearerObjects)
        {
            textSize = Mathf.Max(textSize, 0.001f);
            Vector3 textUp = pointsProjectionOntoLine - pointPreProjection_forTextUp;
            if (UtilitiesDXXL_Math.GetBiggestAbsComponent(textUp) < 0.0001f)
            {
                textUp = UtilitiesDXXL_Math.arbitrarySeldomDir2_precalced;
            }
            UtilitiesDXXL_Text.WriteFramed(lineName, pointsProjectionOntoLine, color, textSize, textDir, textUp, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
        }

        static Color defaultColor1_before;
        public static void Set_defaultColor1_reversible(Color new_defaultColor1)
        {
            defaultColor1_before = DrawMeasurements.defaultColor1;
            DrawMeasurements.defaultColor1 = new_defaultColor1;
        }
        public static void Reverse_defaultColor1()
        {
            DrawMeasurements.defaultColor1 = defaultColor1_before;
        }

        static Color defaultColor2_before;
        public static void Set_defaultColor2_reversible(Color new_defaultColor2)
        {
            defaultColor2_before = DrawMeasurements.defaultColor2;
            DrawMeasurements.defaultColor2 = new_defaultColor2;
        }
        public static void Reverse_defaultColor2()
        {
            DrawMeasurements.defaultColor2 = defaultColor2_before;
        }

        public static void Set_defaultColors_reversible(Color new_defaultColors)
        {
            Set_defaultColor1_reversible(new_defaultColors);
            Set_defaultColor2_reversible(new_defaultColors);
        }
        public static void Reverse_defaultColors()
        {
            Reverse_defaultColor1();
            Reverse_defaultColor2();
        }

        static Vector3 preferredPlanePatternOrientation_forDistancePointToPlane_before;
        public static void Set_preferredPlanePatternOrientation_forDistancePointToPlane_reversible(Vector3 new_preferredPlanePatternOrientation_forDistancePointToPlane)
        {
            preferredPlanePatternOrientation_forDistancePointToPlane_before = DrawMeasurements.preferredPlanePatternOrientation_forDistancePointToPlane;
            DrawMeasurements.preferredPlanePatternOrientation_forDistancePointToPlane = new_preferredPlanePatternOrientation_forDistancePointToPlane;
        }
        public static void Reverse_preferredPlanePatternOrientation_forDistancePointToPlane()
        {
            DrawMeasurements.preferredPlanePatternOrientation_forDistancePointToPlane = preferredPlanePatternOrientation_forDistancePointToPlane_before;
        }

        static float minimumLineLength_forDistancePointToLine_before;
        public static void Set_minimumLineLength_forDistancePointToLine_reversible(float new_minimumLineLength_forDistancePointToLine)
        {
            minimumLineLength_forDistancePointToLine_before = DrawMeasurements.minimumLineLength_forDistancePointToLine;
            DrawMeasurements.minimumLineLength_forDistancePointToLine = new_minimumLineLength_forDistancePointToLine;
        }
        public static void Reverse_minimumLineLength_forDistancePointToLine()
        {
            DrawMeasurements.minimumLineLength_forDistancePointToLine = minimumLineLength_forDistancePointToLine_before;
        }

        static float minimumLineLength_forDistanceLineToLine_before;
        public static void Set_minimumLineLength_forDistanceLineToLine_reversible(float new_minimumLineLength_forDistanceLineToLine)
        {
            minimumLineLength_forDistanceLineToLine_before = DrawMeasurements.minimumLineLength_forDistanceLineToLine;
            DrawMeasurements.minimumLineLength_forDistanceLineToLine = new_minimumLineLength_forDistanceLineToLine;
        }
        public static void Reverse_minimumLineLength_forDistanceLineToLine()
        {
            DrawMeasurements.minimumLineLength_forDistanceLineToLine = minimumLineLength_forDistanceLineToLine_before;
        }

        static float minimumLineLength_forAngleLineToPlane_before;
        public static void Set_minimumLineLength_forAngleLineToPlane_reversible(float new_minimumLineLength_forAngleLineToPlane)
        {
            minimumLineLength_forAngleLineToPlane_before = DrawMeasurements.minimumLineLength_forAngleLineToPlane;
            DrawMeasurements.minimumLineLength_forAngleLineToPlane = new_minimumLineLength_forAngleLineToPlane;
        }
        public static void Reverse_minimumLineLength_forAngleLineToPlane()
        {
            DrawMeasurements.minimumLineLength_forAngleLineToPlane = minimumLineLength_forAngleLineToPlane_before;
        }

    }

}
