namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;
    public class UtilitiesDXXL_Shapes
    {
        public static List<Vector3> verticesGlobal = new List<Vector3>(); 
        public static List<Vector3> verticesLocal = new List<Vector3>();
        static List<Vector3> precalcedUnitCirclePoints_forCurrentlyRequestedFlexNumberedFlatPolygon_aroundOrigin_insideYPlane_startingWithFirstVertexAtZForward_clockwiseWhenLookingDownward = new List<Vector3>();
        static float distanceOfUnscaledFillLines_asFractionOfShapeSize = 0.075f;
        static InternalDXXL_Plane polygonPlane = new InternalDXXL_Plane();
      
        public static int DrawFlatPolygon(float angleDeg_ofFirstCorner_from12oClock, int corners, Vector3 centerPosition, float hullRadius, Vector3 normal, Vector3 up_insidePolyPlane, Color color, float lineWidth, string text, float durationInSec, DrawBasics.LineStyle outlineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool filledWithSpokes, bool hiddenByNearerObjects, bool textBlockAboveLine, bool skipDraw)
        {
            //function returns "usedSlotsIn_verticesGlobal";

            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(angleDeg_ofFirstCorner_from12oClock, "angleDeg_ofFirstCorner_from12oClock")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(hullRadius, "hullRadius")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lineWidth, "lineWidth")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(stylePatternScaleFactor, "stylePatternScaleFactor")) { return 0; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(centerPosition, "centerPosition")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(normal, "normal")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up_insidePolyPlane, "up_insidePolyPlane")) { return 0; }

            lineWidth = UtilitiesDXXL_Math.AbsNonZeroValue(lineWidth);

            if (corners < 3)
            {
                Debug.LogError("Cannot draw a polygon with only " + corners + " corners.");
                return 0;
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(hullRadius))
            {
                //DO NOT fallback via "PointFallback-2D-()" here, because the 2D-version may draw a "Circle()", which forwards to here, which can create an endless loop.
                UtilitiesDXXL_DrawBasics.PointFallback(centerPosition, "[<color=#adadadFF><icon=logMessage></color> Polygon with radius of 0]<br>" + text, color, lineWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, centerPosition, 0);
                return 1;
            }

            UtilitiesDXXL_FlatShapesNormaAndUpCalculation.GetNormalAndUpInsidePlane(out Vector3 normal_final_notGuaranteedNormalized, out Vector3 up_insidePolyPlane_normalized, normal, up_insidePolyPlane, centerPosition);
            polygonPlane.Recreate(centerPosition, normal_final_notGuaranteedNormalized);

            Vector3 vector_fromPolyCenter_toPointOnCircleThatMarksUpInsidePolyPlane = up_insidePolyPlane_normalized * hullRadius;
            float angleDeg_betweenPolyCorners = 360.0f / corners;

            Vector3 posOfFirstCorner = Vector3.zero;
            Vector3 posOfCurrCorner = Vector3.zero;
            Vector3 posOfPrevCorner;

            bool requestedAmountOfCorners_hasPrecalcedPoints = TryFillPrecalcedUnitCirclePointsList(corners);
            Quaternion rotationOfPrecalcedPoints = default(Quaternion); //-> will get filled afterwards in each case where it is used
            if (requestedAmountOfCorners_hasPrecalcedPoints)
            {
                Vector3 forwardOfRotationOfPrecalcedPoints;
                if (UtilitiesDXXL_Math.ApproximatelyZero(angleDeg_ofFirstCorner_from12oClock) == false)
                {
                    Quaternion rotation_fromUpInsidePolyPlane_toUpTowardsFirstVertex = Quaternion.AngleAxis(angleDeg_ofFirstCorner_from12oClock, normal_final_notGuaranteedNormalized);
                    forwardOfRotationOfPrecalcedPoints = rotation_fromUpInsidePolyPlane_toUpTowardsFirstVertex * up_insidePolyPlane_normalized;
                }
                else
                {
                    forwardOfRotationOfPrecalcedPoints = up_insidePolyPlane_normalized;
                }

                rotationOfPrecalcedPoints = Quaternion.LookRotation(forwardOfRotationOfPrecalcedPoints, normal_final_notGuaranteedNormalized);
            }

            int usedSlotsIn_verticesGlobal = 0;
            for (int i = 0; i < corners; i++)
            {
                Vector3 vector_fromPolyCenter_toCurrCorner;
                if (requestedAmountOfCorners_hasPrecalcedPoints)
                {
                    vector_fromPolyCenter_toCurrCorner = rotationOfPrecalcedPoints * precalcedUnitCirclePoints_forCurrentlyRequestedFlexNumberedFlatPolygon_aroundOrigin_insideYPlane_startingWithFirstVertexAtZForward_clockwiseWhenLookingDownward[i];
                    vector_fromPolyCenter_toCurrCorner = hullRadius * vector_fromPolyCenter_toCurrCorner;
                }
                else
                {
                    Quaternion rotation_fromFirstCorner_toCurrCorner = Quaternion.AngleAxis(angleDeg_betweenPolyCorners * i + angleDeg_ofFirstCorner_from12oClock, normal_final_notGuaranteedNormalized);
                    vector_fromPolyCenter_toCurrCorner = rotation_fromFirstCorner_toCurrCorner * vector_fromPolyCenter_toPointOnCircleThatMarksUpInsidePolyPlane;
                }

                if (i == 0)
                {
                    posOfCurrCorner = centerPosition + vector_fromPolyCenter_toCurrCorner;
                    posOfFirstCorner = posOfCurrCorner;
                    usedSlotsIn_verticesGlobal = UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, posOfFirstCorner, usedSlotsIn_verticesGlobal);
                }
                else
                {
                    posOfPrevCorner = posOfCurrCorner;
                    posOfCurrCorner = centerPosition + vector_fromPolyCenter_toCurrCorner;
                    usedSlotsIn_verticesGlobal = UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, posOfCurrCorner, usedSlotsIn_verticesGlobal);

                    if (skipDraw == false)
                    {
                        UtilitiesDXXL_DrawBasics.Line(posOfPrevCorner, posOfCurrCorner, color, lineWidth, null, outlineStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                        if (filledWithSpokes)
                        {
                            UtilitiesDXXL_DrawBasics.Line(centerPosition, posOfCurrCorner, color, lineWidth, null, outlineStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                        }
                    }

                }
            }

            if (skipDraw == false)
            {
                //close gap from last to first corner:
                UtilitiesDXXL_DrawBasics.Line(posOfCurrCorner, posOfFirstCorner, color, lineWidth, null, outlineStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                if (filledWithSpokes)
                {
                    UtilitiesDXXL_DrawBasics.Line(centerPosition, posOfFirstCorner, color, lineWidth, null, outlineStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                }

                if (fillStyle != DrawBasics.LineStyle.invisible)
                {
                    float absHullRadius = Mathf.Abs(hullRadius);
                    // float distanceBetweenLines = 0.04f / DrawShapes.ShapeFillDensity;
                    float absHullDiameter = 2.0f * absHullRadius;
                    float distanceBetweenLines = (distanceOfUnscaledFillLines_asFractionOfShapeSize * absHullDiameter) / DrawShapes.ShapeFillDensity;
                    distanceBetweenLines = Mathf.Min(distanceBetweenLines, 0.15f * absHullRadius);
                    distanceBetweenLines = Mathf.Max(distanceBetweenLines, 0.005f * absHullRadius);
                    int usedSlotsInFillEdgesList = RecalcFillingOfPolygon(usedSlotsIn_verticesGlobal, up_insidePolyPlane_normalized, distanceBetweenLines);
                    for (int i = 0; i < usedSlotsInFillEdgesList; i++)
                    {
                        UtilitiesDXXL_DrawBasics.Line(fillEdges[i].start, fillEdges[i].end, color, 0.0f, null, fillStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    }
                }

                if (text != null && text != "")
                {
                    float virtualScalePerDim = 0.75f * hullRadius;
                    Copy_globalVertices_to_localVertices(centerPosition, usedSlotsIn_verticesGlobal);
                    UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, null, centerPosition, usedSlotsIn_verticesGlobal, lineWidth, virtualScalePerDim, color, color, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
                }
            }

            return usedSlotsIn_verticesGlobal;
        }

        public static int Triangle(Vector3 centerPosition, float hullRadius, Color color, Vector3 normal, Vector3 up_insideTrianglePlane, float lineWidth, string text, DrawBasics.LineStyle outlineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool filledWithSpokes, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects, bool skipDraw)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(hullRadius, "hullRadius")) { return 0; }
            return DrawFlatPolygon(0.0f, 3, centerPosition, Mathf.Abs(hullRadius), normal, up_insideTrianglePlane, color, lineWidth, text, durationInSec, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, hiddenByNearerObjects, textBlockAboveLine, skipDraw);
        }

        public static int Square(Vector3 centerPosition, float sideLength, Color color, Vector3 normal, Vector3 up_insideSquarePlane, float lineWidth, string text, DrawBasics.LineStyle outlineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool filledWithSpokes, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects, bool skipDraw)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(sideLength, "sideLength")) { return 0; }
            return DrawFlatPolygon(45.0f, 4, centerPosition, Mathf.Abs(0.5f * sideLength * UtilitiesDXXL_Math.sqrtOf2_precalced), normal, up_insideSquarePlane, color, lineWidth, text, durationInSec, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, hiddenByNearerObjects, textBlockAboveLine, skipDraw);
        }

        public static int Pentagon(Vector3 centerPosition, float hullRadius, Color color, Vector3 normal, Vector3 up_insidePentagonPlane, float lineWidth, string text, DrawBasics.LineStyle outlineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool filledWithSpokes, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects, bool skipDraw)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(hullRadius, "hullRadius")) { return 0; }
            return DrawFlatPolygon(0.0f, 5, centerPosition, Mathf.Abs(hullRadius), normal, up_insidePentagonPlane, color, lineWidth, text, durationInSec, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, hiddenByNearerObjects, textBlockAboveLine, skipDraw);
        }

        public static int Hexagon(Vector3 centerPosition, float hullRadius, Color color, Vector3 normal, Vector3 up_insideHexagonPlane, float lineWidth, string text, DrawBasics.LineStyle outlineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool filledWithSpokes, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects, bool skipDraw)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(hullRadius, "hullRadius")) { return 0; }
            return DrawFlatPolygon(30.0f, 6, centerPosition, Mathf.Abs(hullRadius), normal, up_insideHexagonPlane, color, lineWidth, text, durationInSec, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, hiddenByNearerObjects, textBlockAboveLine, skipDraw);
        }

        public static int Septagon(Vector3 centerPosition, float hullRadius, Color color, Vector3 normal, Vector3 up_insideSeptagonPlane, float lineWidth, string text, DrawBasics.LineStyle outlineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool filledWithSpokes, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects, bool skipDraw)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(hullRadius, "hullRadius")) { return 0; }
            return DrawFlatPolygon(0.0f, 7, centerPosition, Mathf.Abs(hullRadius), normal, up_insideSeptagonPlane, color, lineWidth, text, durationInSec, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, hiddenByNearerObjects, textBlockAboveLine, skipDraw);
        }

        public static int Octagon(Vector3 centerPosition, float hullRadius, Color color, Vector3 normal, Vector3 up_insideOctagonPlane, float lineWidth, string text, DrawBasics.LineStyle outlineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool filledWithSpokes, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects, bool skipDraw)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(hullRadius, "hullRadius")) { return 0; }
            return DrawFlatPolygon(67.5f, 8, centerPosition, Mathf.Abs(hullRadius), normal, up_insideOctagonPlane, color, lineWidth, text, durationInSec, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, hiddenByNearerObjects, textBlockAboveLine, skipDraw);
        }

        public static int Decagon(Vector3 centerPosition, float hullRadius, Color color, Vector3 normal, Vector3 up_insideDecagonPlane, float lineWidth, string text, DrawBasics.LineStyle outlineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool filledWithSpokes, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects, bool skipDraw)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(hullRadius, "hullRadius")) { return 0; }
            return DrawFlatPolygon(0.0f, 10, centerPosition, Mathf.Abs(hullRadius), normal, up_insideDecagonPlane, color, lineWidth, text, durationInSec, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, hiddenByNearerObjects, textBlockAboveLine, skipDraw);
        }

        public static int Circle(Vector3 centerPosition, float radius, Color color, Vector3 normal, Vector3 up_insideCirclePlane, float lineWidth, string text, DrawBasics.LineStyle outlineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool filledWithSpokes, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects, bool skipDraw)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return 0; }
            return DrawFlatPolygon(0.0f, 32, centerPosition, Mathf.Abs(radius), normal, up_insideCirclePlane, color, lineWidth, text, durationInSec, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, hiddenByNearerObjects, textBlockAboveLine, skipDraw);
        }

        public static int Ellipse(Vector3 centerPosition, float radiusSideward, float radiusUpward, Color color, Vector3 normal, Vector3 up_insideEllipsePlane, float lineWidth, string text, DrawBasics.LineStyle outlineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool filledWithSpokes, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects, bool skipDraw)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radiusUpward, "radiusUpward")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radiusSideward, "radiusSideward")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lineWidth, "lineWidth")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(stylePatternScaleFactor, "stylePatternScaleFactor")) { return 0; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(centerPosition, "centerPosition")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(normal, "normal")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up_insideEllipsePlane, "up_insideEllipsePlane")) { return 0; }

            lineWidth = UtilitiesDXXL_Math.AbsNonZeroValue(lineWidth);
            UtilitiesDXXL_FlatShapesNormaAndUpCalculation.GetNormalAndUpInsidePlane(out Vector3 normal_final_notGuaranteedNormalized, out Vector3 up_insideEllipsePlane_normalized, normal, up_insideEllipsePlane, centerPosition);
            float absRadiusForward = Mathf.Abs(radiusUpward);
            float absRadiusSideward = Mathf.Abs(radiusSideward);
            if (UtilitiesDXXL_Math.ApproximatelyZero(absRadiusForward) && UtilitiesDXXL_Math.ApproximatelyZero(absRadiusSideward))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(centerPosition, "[<color=#adadadFF><icon=logMessage></color> Ellipse with radius of 0]<br>" + text, color, lineWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, centerPosition, 0);
                return 1;
            }

            int usedSlotsIn_verticesGlobal = DrawFlatPolygon(0.0f, 32, Vector3.zero, 0.5f, Vector3.forward, Vector3.up, color, lineWidth, null, durationInSec, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, hiddenByNearerObjects, textBlockAboveLine, true);
            UtilitiesDXXL_List.CopyContentOfVectorLists(ref verticesLocal, ref verticesGlobal, usedSlotsIn_verticesGlobal); //-> fixing mixup from global and local, after "DrawFlatPolygon" was (virtually) drawn at origin, but filled the "verticesGlobal"-list

            ScaleY(ref verticesLocal, 2.0f * absRadiusForward, usedSlotsIn_verticesGlobal);
            ScaleX(ref verticesLocal, 2.0f * absRadiusSideward, usedSlotsIn_verticesGlobal);
            Quaternion rotation = Quaternion.LookRotation(normal_final_notGuaranteedNormalized, up_insideEllipsePlane_normalized);
            RotateVertices(ref verticesLocal, rotation, usedSlotsIn_verticesGlobal);
            Copy_localVertices_to_globalVertices(centerPosition, usedSlotsIn_verticesGlobal);

            if (skipDraw == false)
            {
                polygonPlane.Recreate(centerPosition, normal_final_notGuaranteedNormalized);
                for (int i = 0; i < usedSlotsIn_verticesGlobal; i++)
                {
                    UtilitiesDXXL_DrawBasics.Line(verticesGlobal[i], verticesGlobal[UtilitiesDXXL_Math.LoopOvershootingIndexIntoCollectionSize(i + 1, usedSlotsIn_verticesGlobal)], color, lineWidth, null, outlineStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    if (filledWithSpokes)
                    {
                        UtilitiesDXXL_DrawBasics.Line(centerPosition, verticesGlobal[i], color, lineWidth, null, outlineStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    }
                }

                if (fillStyle != DrawBasics.LineStyle.invisible)
                {
                    //float distanceBetweenLines = 0.04f / DrawShapes.ShapeFillDensity;
                    float absDiameterForward = 2.0f * absRadiusForward;
                    float distanceBetweenLines = (distanceOfUnscaledFillLines_asFractionOfShapeSize * absDiameterForward) / DrawShapes.ShapeFillDensity;
                    distanceBetweenLines = Mathf.Min(distanceBetweenLines, 0.15f * absRadiusForward);
                    distanceBetweenLines = Mathf.Max(distanceBetweenLines, 0.005f * absRadiusForward);
                    int usedSlotsInFillEdgesList = RecalcFillingOfPolygon(usedSlotsIn_verticesGlobal, up_insideEllipsePlane_normalized, distanceBetweenLines);
                    for (int i = 0; i < usedSlotsInFillEdgesList; i++)
                    {
                        UtilitiesDXXL_DrawBasics.Line(fillEdges[i].start, fillEdges[i].end, color, 0.0f, null, fillStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    }
                }

                if (text != null && text != "")
                {
                    float virtualScalePerDim = 0.75f * Mathf.Max(absRadiusForward, absRadiusSideward);
                    UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, null, centerPosition, usedSlotsIn_verticesGlobal, lineWidth, virtualScalePerDim, color, color, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
                }
            }

            return usedSlotsIn_verticesGlobal;
        }

        public static int Star(Vector3 centerPosition, float outerRadius, Color color, int corners, float innerRadiusFactor, Vector3 normal, Vector3 up_insideStarPlane, float lineWidth, string text, DrawBasics.LineStyle outlineStyle, float stylePatternScaleFactor, bool filledWithSpokes, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects, bool skipDraw)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(outerRadius, "outerRadius")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(innerRadiusFactor, "innerRadiusFactor")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lineWidth, "lineWidth")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(stylePatternScaleFactor, "stylePatternScaleFactor")) { return 0; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(centerPosition, "centerPosition")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(normal, "normal")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up_insideStarPlane, "up_insideStarPlane")) { return 0; }

            lineWidth = UtilitiesDXXL_Math.AbsNonZeroValue(lineWidth);

            if (corners < 3)
            {
                Debug.LogError("Cannot draw a star with only " + corners + " corners.");
                UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, centerPosition, 0);
                return 1;
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(outerRadius))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(centerPosition, "[<color=#adadadFF><icon=logMessage></color> Star with radius of 0]<br>" + text, color, lineWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, centerPosition, 0);
                return 1;
            }

            UtilitiesDXXL_FlatShapesNormaAndUpCalculation.GetNormalAndUpInsidePlane(out Vector3 normal_final_notGuaranteedNormalized, out Vector3 up_insideStarPlane_normalized, normal, up_insideStarPlane, centerPosition);
            if (skipDraw == false) { polygonPlane.Recreate(centerPosition, normal_final_notGuaranteedNormalized); }

            float angleDeg_betweenOuterCorners = 360.0f / corners;
            float angleDeg_betweenOuterAndInnerCorners = 0.5f * angleDeg_betweenOuterCorners;
            float scaleFactor_fromOuterRadius_toOuterPolyHullAtInnerCornersPos = Mathf.Cos(Mathf.Deg2Rad * angleDeg_betweenOuterAndInnerCorners);
            Vector3 vector_fromPolyCenter_toFirstOuterCorner = up_insideStarPlane_normalized * outerRadius;
            float scaleFactor_fromOuterRadius_toInnerRadius = scaleFactor_fromOuterRadius_toOuterPolyHullAtInnerCornersPos * innerRadiusFactor;
            float innerRadius = scaleFactor_fromOuterRadius_toInnerRadius * outerRadius;
            Vector3 vector_fromPolyCenter_toFirstOuterCorner_shortenedToInnerRadiusLength = vector_fromPolyCenter_toFirstOuterCorner * scaleFactor_fromOuterRadius_toInnerRadius;

            bool requestedAmountOfCorners_hasPrecalcedPoints = TryFillPrecalcedUnitCirclePointsList(corners);
            Quaternion rotationOfPrecalcedOuterPoints = default(Quaternion); //-> will get filled afterwards in each case where it is used
            Quaternion rotationOfPrecalcedInnerPoints = default(Quaternion); //-> will get filled afterwards in each case where it is used
            if (requestedAmountOfCorners_hasPrecalcedPoints)
            {
                Quaternion rotation_fromOuterPoints_toInnerPoints = Quaternion.AngleAxis(angleDeg_betweenOuterAndInnerCorners, normal_final_notGuaranteedNormalized);
                rotationOfPrecalcedOuterPoints = Quaternion.LookRotation(up_insideStarPlane, normal_final_notGuaranteedNormalized);
                rotationOfPrecalcedInnerPoints = Quaternion.LookRotation(rotation_fromOuterPoints_toInnerPoints * up_insideStarPlane, normal_final_notGuaranteedNormalized);
            }

            Vector3 posOfFirstOuterCorner = Vector3.zero;
            Vector3 posOfCurrOuterCorner = Vector3.zero;
            Vector3 posOfCurrInnerCorner = Vector3.zero;
            Vector3 posOfPrevInnerCorner;

            int usedSlotsIn_verticesGlobal = 0;
            for (int i = 0; i < corners; i++)
            {
                Vector3 vector_fromPolyCenter_toCurrOuterCorner;
                Vector3 vector_fromPolyCenter_toCurrInnerCorner;
                if (requestedAmountOfCorners_hasPrecalcedPoints)
                {
                    vector_fromPolyCenter_toCurrOuterCorner = rotationOfPrecalcedOuterPoints * precalcedUnitCirclePoints_forCurrentlyRequestedFlexNumberedFlatPolygon_aroundOrigin_insideYPlane_startingWithFirstVertexAtZForward_clockwiseWhenLookingDownward[i];
                    vector_fromPolyCenter_toCurrOuterCorner = outerRadius * vector_fromPolyCenter_toCurrOuterCorner;
                    vector_fromPolyCenter_toCurrInnerCorner = rotationOfPrecalcedInnerPoints * precalcedUnitCirclePoints_forCurrentlyRequestedFlexNumberedFlatPolygon_aroundOrigin_insideYPlane_startingWithFirstVertexAtZForward_clockwiseWhenLookingDownward[i];
                    vector_fromPolyCenter_toCurrInnerCorner = innerRadius * vector_fromPolyCenter_toCurrInnerCorner;
                }
                else
                {
                    Quaternion rotation_fromFirstOuterCorner_toCurrOuterCorner = Quaternion.AngleAxis(angleDeg_betweenOuterCorners * i, normal_final_notGuaranteedNormalized);
                    Quaternion rotation_fromFirstOuterCorner_toCurrInnerCorner = Quaternion.AngleAxis(angleDeg_betweenOuterCorners * i + 0.5f * angleDeg_betweenOuterCorners, normal_final_notGuaranteedNormalized);
                    vector_fromPolyCenter_toCurrOuterCorner = rotation_fromFirstOuterCorner_toCurrOuterCorner * vector_fromPolyCenter_toFirstOuterCorner;
                    vector_fromPolyCenter_toCurrInnerCorner = rotation_fromFirstOuterCorner_toCurrInnerCorner * vector_fromPolyCenter_toFirstOuterCorner_shortenedToInnerRadiusLength;
                }

                if (i == 0)
                {
                    posOfCurrOuterCorner = centerPosition + vector_fromPolyCenter_toCurrOuterCorner;
                    posOfFirstOuterCorner = posOfCurrOuterCorner;

                    posOfCurrInnerCorner = centerPosition + vector_fromPolyCenter_toCurrInnerCorner;
                    usedSlotsIn_verticesGlobal = UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, posOfCurrInnerCorner, usedSlotsIn_verticesGlobal);

                    if (skipDraw == false)
                    {
                        UtilitiesDXXL_DrawBasics.Line(posOfCurrOuterCorner, posOfCurrInnerCorner, color, lineWidth, null, outlineStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                        if (filledWithSpokes)
                        {
                            UtilitiesDXXL_DrawBasics.Line(centerPosition, posOfCurrInnerCorner, color, lineWidth, null, outlineStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                        }
                    }
                }
                else
                {
                    posOfCurrOuterCorner = centerPosition + vector_fromPolyCenter_toCurrOuterCorner;
                    posOfPrevInnerCorner = posOfCurrInnerCorner;
                    posOfCurrInnerCorner = centerPosition + vector_fromPolyCenter_toCurrInnerCorner;

                    usedSlotsIn_verticesGlobal = UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, posOfCurrOuterCorner, usedSlotsIn_verticesGlobal);
                    usedSlotsIn_verticesGlobal = UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, posOfCurrInnerCorner, usedSlotsIn_verticesGlobal);

                    if (skipDraw == false)
                    {
                        UtilitiesDXXL_DrawBasics.Line(posOfPrevInnerCorner, posOfCurrOuterCorner, color, lineWidth, null, outlineStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                        UtilitiesDXXL_DrawBasics.Line(posOfCurrOuterCorner, posOfCurrInnerCorner, color, lineWidth, null, outlineStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);

                        if (filledWithSpokes)
                        {
                            UtilitiesDXXL_DrawBasics.Line(centerPosition, posOfCurrOuterCorner, color, lineWidth, null, outlineStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                            UtilitiesDXXL_DrawBasics.Line(centerPosition, posOfCurrInnerCorner, color, lineWidth, null, outlineStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                        }
                    }
                }
            }

            usedSlotsIn_verticesGlobal = UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, posOfFirstOuterCorner, usedSlotsIn_verticesGlobal);

            if (skipDraw == false)
            {
                //close gap from last to first corner:
                UtilitiesDXXL_DrawBasics.Line(posOfCurrInnerCorner, posOfFirstOuterCorner, color, lineWidth, null, outlineStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                if (filledWithSpokes)
                {
                    UtilitiesDXXL_DrawBasics.Line(centerPosition, posOfFirstOuterCorner, color, lineWidth, null, outlineStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                }

                //"fillStyle" not supported: GetFillingOfPolygon is not fit for concave shapes like stars

                if (text != null && text != "")
                {
                    float virtualScalePerDim = 0.75f * outerRadius;
                    Copy_globalVertices_to_localVertices(centerPosition, usedSlotsIn_verticesGlobal);
                    UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, null, centerPosition, usedSlotsIn_verticesGlobal, lineWidth, virtualScalePerDim, color, color, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
                }
            }

            return usedSlotsIn_verticesGlobal;
        }

        static InternalDXXL_Plane plane_perpToCapsuleDir = new InternalDXXL_Plane();
        public static int FlatCapsule(Vector3 posOfCircle1, Vector3 posOfCircle2, float radius, Color color, Vector3 normal, float lineWidth, string text, DrawBasics.LineStyle outlineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool filledWithSpokes, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects, float unscaled_distanceBetweenFillLines = 0.0f, Vector3 upInsidePlane_fallbackForCircleCase = default(Vector3))
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return 0; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(posOfCircle1, "posOfCircle1")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(posOfCircle2, "posOfCircle2")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(normal, "normal")) { return 0; }

            Vector3 cirle1_to_circle2 = posOfCircle2 - posOfCircle1;
            Vector3 centerPosition = 0.5f * (posOfCircle1 + posOfCircle2);
            float distance_betweenCircleCenters = cirle1_to_circle2.magnitude;
            if (distance_betweenCircleCenters > 0.0001f)
            {
                //"normal" has to be defined here (as not-default-vector + perpToUp) because otherwise the later "UtilitiesDXXL_FlatShapesNormaAndUpCalculation.GetNormalAndUpInsidePlane" would force "upInsideCapsulePlane=cirle1_to_circle2" so it is perp to "normal", but in this case here has the peculiarity that "upInsideCapsulePlane" is the relevant dir, and "normal" should be forced to align perp to it.
                if (UtilitiesDXXL_Math.IsDefaultVector(normal))
                {
                    UtilitiesDXXL_FlatShapesNormaAndUpCalculation.GetNormalAndUpInsidePlane(out normal, out Vector3 upAlongVert_insideCapsulePlane_normalized, default(Vector3), cirle1_to_circle2, centerPosition);
                }
                else
                {
                    plane_perpToCapsuleDir.Recreate(posOfCircle1, cirle1_to_circle2);
                    normal = ForceVectorPerpToOtherVector(normal, plane_perpToCapsuleDir);
                }
            }

            float width = 2.0f * radius;
            float height = 2.0f * radius + distance_betweenCircleCenters;
            return FlatCapsule(centerPosition, width, height, color, normal, cirle1_to_circle2, CapsuleDirection2D.Vertical, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects, unscaled_distanceBetweenFillLines, upInsidePlane_fallbackForCircleCase);
        }

        public static int FlatCapsule(Vector3 centerPosition, float width, float height, Color color, Vector3 normal, Vector3 upAlongVert_insideCapsulePlane, CapsuleDirection2D direction, float lineWidth, string text, DrawBasics.LineStyle outlineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool filledWithSpokes, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects, float unscaled_distanceBetweenFillLines = 0.0f, Vector3 upInsidePlane_fallbackForCircleCase = default(Vector3))
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width, "width")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(height, "height")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lineWidth, "lineWidth")) { return 0; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(centerPosition, "centerPosition")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(normal, "normal")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(upAlongVert_insideCapsulePlane, "upAlongVert_insideCapsulePlane")) { return 0; }

            lineWidth = UtilitiesDXXL_Math.AbsNonZeroValue(lineWidth);

            if (UtilitiesDXXL_Math.ApproximatelyZero(width) && UtilitiesDXXL_Math.ApproximatelyZero(height))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(centerPosition, "[<color=#adadadFF><icon=logMessage></color> FlatCapsule with size of 0]<br>" + text, color, lineWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, centerPosition, 0);
                return 1;
            }

            float absWidth = UtilitiesDXXL_Math.AbsNonZeroValue(width);
            float absHeight = UtilitiesDXXL_Math.AbsNonZeroValue(height);

            UtilitiesDXXL_FlatShapesNormaAndUpCalculation.GetNormalAndUpInsidePlane(out Vector3 normal_final_notGuaranteedNormalized, out Vector3 upAlongVert_insideCapsulePlane_normalized, normal, upAlongVert_insideCapsulePlane, centerPosition);
            polygonPlane.Recreate(centerPosition, normal_final_notGuaranteedNormalized);

            float absBiggerSizeComponent;
            float absSmallerSizeComponent;
            float absRadius;
            bool isCircle = false;
            if (direction == CapsuleDirection2D.Vertical)
            {
                if (absHeight <= absWidth)
                {
                    isCircle = true;
                    absHeight = Mathf.Max(absHeight, absWidth);
                }
                absRadius = 0.5f * absWidth;
                absBiggerSizeComponent = absHeight;
                absSmallerSizeComponent = absWidth;
            }
            else
            {
                if (absWidth <= absHeight)
                {
                    isCircle = true;
                    absWidth = Mathf.Max(absHeight, absWidth);
                }
                absRadius = 0.5f * absHeight;
                absBiggerSizeComponent = absWidth;
                absSmallerSizeComponent = absHeight;
            }

            if (isCircle)
            {
                Vector3 up_insideCirclePlane;
                if (UtilitiesDXXL_Math.IsDefaultVector(upInsidePlane_fallbackForCircleCase))
                {
                    up_insideCirclePlane = (direction == CapsuleDirection2D.Vertical) ? upAlongVert_insideCapsulePlane_normalized : Vector3.Cross(upAlongVert_insideCapsulePlane_normalized, normal_final_notGuaranteedNormalized);
                }
                else
                {
                    up_insideCirclePlane = upInsidePlane_fallbackForCircleCase; //<-used by screenspace-version where the camera might be skewed
                }
                return DrawShapes.Circle(centerPosition, absRadius, color, normal_final_notGuaranteedNormalized, up_insideCirclePlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                int cornersPerHalfCircle = 17;
                int usedSlotsIn_verticesLocal = 2 * cornersPerHalfCircle;
                float halfAbsHeight = 0.5f * absHeight;
                float halfAbsWidth = 0.5f * absWidth;
                Vector3 centerOfSphere1_local_unrotated;
                Vector3 centerOfSphere2_local_unrotated;

                if (direction == CapsuleDirection2D.Vertical)
                {
                    Vector3 upperSphereCenter_local_unrotated = Vector3.up * (halfAbsHeight - absRadius);
                    Vector3 lowerSphereCenter_local_unrotated = Vector3.down * (halfAbsHeight - absRadius);
                    centerOfSphere1_local_unrotated = lowerSphereCenter_local_unrotated;
                    centerOfSphere2_local_unrotated = upperSphereCenter_local_unrotated;
                    for (int i = 0; i < cornersPerHalfCircle; i++)
                    {
                        int curr_i_inPrecalcedPointsArray = UtilitiesDXXL_Math.LoopOvershootingIndexIntoCollectionSize(8 - i, 32);
                        Vector3 vector_fromUpperSphereCenter_toCurrCorner = halfAbsWidth * _32precalcedUnitCirclePoints_aroundOrigin_insideZPlane_startingAtUpward_clockwiseWhenLookingAlongZForward[curr_i_inPrecalcedPointsArray];
                        UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, upperSphereCenter_local_unrotated + vector_fromUpperSphereCenter_toCurrCorner, i);
                        UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, lowerSphereCenter_local_unrotated - vector_fromUpperSphereCenter_toCurrCorner, i + cornersPerHalfCircle);
                    }
                }
                else
                {
                    Vector3 rightSphereCenter_local_unrotated = Vector3.right * (halfAbsWidth - absRadius);
                    Vector3 leftSphereCenter_local_unrotated = Vector3.left * (halfAbsWidth - absRadius);
                    centerOfSphere1_local_unrotated = leftSphereCenter_local_unrotated;
                    centerOfSphere2_local_unrotated = rightSphereCenter_local_unrotated;
                    for (int i = 0; i < cornersPerHalfCircle; i++)
                    {
                        int curr_i_inPrecalcedPointsArray = 16 - i;
                        Vector3 vector_fromRightSphereCenter_toCurrCorner = halfAbsHeight * _32precalcedUnitCirclePoints_aroundOrigin_insideZPlane_startingAtUpward_clockwiseWhenLookingAlongZForward[curr_i_inPrecalcedPointsArray];
                        UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, rightSphereCenter_local_unrotated + vector_fromRightSphereCenter_toCurrCorner, i);
                        UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, leftSphereCenter_local_unrotated - vector_fromRightSphereCenter_toCurrCorner, i + cornersPerHalfCircle);
                    }
                }

                Quaternion rotation = Quaternion.LookRotation(normal_final_notGuaranteedNormalized, upAlongVert_insideCapsulePlane_normalized);
                RotateVertices(ref verticesLocal, rotation, usedSlotsIn_verticesLocal);
                Copy_localVertices_to_globalVertices(centerPosition, usedSlotsIn_verticesLocal);
                int usedSlotsIn_verticesGlobal = usedSlotsIn_verticesLocal;

                for (int i = 0; i < usedSlotsIn_verticesGlobal; i++)
                {
                    UtilitiesDXXL_DrawBasics.Line(verticesGlobal[i], verticesGlobal[UtilitiesDXXL_Math.LoopOvershootingIndexIntoCollectionSize(i + 1, usedSlotsIn_verticesGlobal)], color, lineWidth, null, outlineStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    if (filledWithSpokes)
                    {
                        UtilitiesDXXL_DrawBasics.Line(centerPosition, verticesGlobal[i], color, lineWidth, null, outlineStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    }
                }

                if (filledWithSpokes)
                {
                    //additional struts at straight part:
                    Vector3 strutStartAnchors_at0 = verticesGlobal[0];
                    Vector3 strutStartAnchors_0to1 = verticesGlobal[usedSlotsIn_verticesGlobal - 1] - verticesGlobal[0];
                    Vector3 strutEndAnchors_at0 = verticesGlobal[cornersPerHalfCircle];
                    Vector3 strutEndAnchors_0to1 = verticesGlobal[cornersPerHalfCircle - 1] - verticesGlobal[cornersPerHalfCircle];

                    float ratio_biggerToSmallerDim = (Mathf.Max(0.02f, absBiggerSizeComponent) / Mathf.Max(0.01f, absSmallerSizeComponent));
                    int numberOfStrutsAtStraightPart = Mathf.FloorToInt(1.3f * ratio_biggerToSmallerDim * ratio_biggerToSmallerDim);
                    numberOfStrutsAtStraightPart = Mathf.Min(numberOfStrutsAtStraightPart, 30);
                    for (int i = 0; i < numberOfStrutsAtStraightPart; i++)
                    {
                        float progress_0to1 = (float)(i + 1) / (float)(numberOfStrutsAtStraightPart + 1);
                        UtilitiesDXXL_DrawBasics.Line(strutStartAnchors_at0 + progress_0to1 * strutStartAnchors_0to1, strutEndAnchors_at0 + progress_0to1 * strutEndAnchors_0to1, color, lineWidth, null, outlineStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    }
                }

                float absAverageCapsuleSize = 0.5f * (absSmallerSizeComponent + absBiggerSizeComponent);
                DrawCapsuleFilling(unscaled_distanceBetweenFillLines, fillStyle, usedSlotsIn_verticesGlobal, centerOfSphere1_local_unrotated, centerOfSphere2_local_unrotated, color, rotation, stylePatternScaleFactor, absAverageCapsuleSize, durationInSec, hiddenByNearerObjects);

                if (text != null && text != "")
                {
                    float virtualScalePerDim = 0.75f * absBiggerSizeComponent;
                    UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, null, centerPosition, usedSlotsIn_verticesLocal, lineWidth, virtualScalePerDim, color, color, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
                }
                return usedSlotsIn_verticesGlobal;
            }
        }

        static void DrawCapsuleFilling(float unscaled_distanceBetweenFillLines, DrawBasics.LineStyle fillStyle, int usedSlotsIn_verticesGlobal, Vector3 centerOfSphere1_local_unrotated, Vector3 centerOfSphere2_local_unrotated, Color color, Quaternion rotation, float stylePatternScaleFactor, float absAverageCapsuleSize, float durationInSec, bool hiddenByNearerObjects)
        {
            if (fillStyle != DrawBasics.LineStyle.invisible)
            {
                float distanceBetweenLines;
                if (UtilitiesDXXL_Math.ApproximatelyZero(unscaled_distanceBetweenFillLines))
                {
                    distanceBetweenLines = (distanceOfUnscaledFillLines_asFractionOfShapeSize * absAverageCapsuleSize) / DrawShapes.ShapeFillDensity;
                }
                else
                {
                    //used by Screenspace-Version, where fillLineDistances is dependent on screenHeight rather than capsuleSize:
                    distanceBetweenLines = unscaled_distanceBetweenFillLines / DrawShapes.ShapeFillDensity;
                }

                distanceBetweenLines = Mathf.Min(distanceBetweenLines, 0.15f * absAverageCapsuleSize);
                distanceBetweenLines = Mathf.Max(distanceBetweenLines, 0.005f * absAverageCapsuleSize);
                Vector3 centerOfSphere1_local_rotated = rotation * centerOfSphere1_local_unrotated;
                Vector3 centerOfSphere2_local_rotated = rotation * centerOfSphere2_local_unrotated;
                Vector3 up_insideCapsulePlane = centerOfSphere2_local_rotated - centerOfSphere1_local_rotated;
                Vector3 up_insideCapsulePlane_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(up_insideCapsulePlane);
                int usedSlotsInFillEdgesList = RecalcFillingOfPolygon(usedSlotsIn_verticesGlobal, up_insideCapsulePlane_normalized, distanceBetweenLines);
                for (int i = 0; i < usedSlotsInFillEdgesList; i++)
                {
                    UtilitiesDXXL_DrawBasics.Line(fillEdges[i].start, fillEdges[i].end, color, 0.0f, null, fillStyle, stylePatternScaleFactor, 0.0f, null, polygonPlane, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                }
            }
        }

        static InternalDXXL_Plane plane = new InternalDXXL_Plane();
        public static float finalWidth_ofLastDrawnPlane;
        public static float finalLength_ofLastDrawnPlane;
        public static void Plane(Vector3 planeMountingPoint, Vector3 positionOnPlaneToIncorporate, Vector3 normal, Color color, float width, float length, Vector3 forward_insidePlane, float linesWidth, string text, float subSegments_signFlipsInterpretation, bool pointer_as_textAttachStyle, float anchorVisualizationSize, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width, "width")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(length, "length")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(subSegments_signFlipsInterpretation, "subSegments_signFlipsInterpretation")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(anchorVisualizationSize, "anchorVisualizationSize")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(planeMountingPoint, "planeMountingPoint")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(positionOnPlaneToIncorporate, "positionOnPlaneToIncorporate")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(normal, "normal")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(forward_insidePlane, "forward")) { return; }

            if (subSegments_signFlipsInterpretation < 0.0f)
            {
                //-> drawing the plane a second time, but only the outline square, since the inner struts don't necessarily coincide with the outline
                string text_ofBoundarySquarePlane = null;
                float subSegments_signFlipsInterpretation_ofBoundarySquarePlane = 1.0f;//-> this is also important to prevent endless recursive drawing
                float anchorVisualizationSize_ofBoundarySquarePlane = 0.0f;
                Plane(planeMountingPoint, positionOnPlaneToIncorporate, normal, color, width, length, forward_insidePlane, linesWidth, text_ofBoundarySquarePlane, subSegments_signFlipsInterpretation_ofBoundarySquarePlane, pointer_as_textAttachStyle, anchorVisualizationSize_ofBoundarySquarePlane, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }

            width = UtilitiesDXXL_Math.AbsNonZeroValue(width);
            length = UtilitiesDXXL_Math.AbsNonZeroValue(length);

            Vector3 centralPositionOfTheDrawnPlaneArea = planeMountingPoint;
            if (UtilitiesDXXL_Math.IsDefaultVector(positionOnPlaneToIncorporate) == false)
            {
                if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(planeMountingPoint, positionOnPlaneToIncorporate) == false)
                {
                    Vector3 fromMountingPointToIncorporatedPoint = positionOnPlaneToIncorporate - planeMountingPoint;
                    centralPositionOfTheDrawnPlaneArea = centralPositionOfTheDrawnPlaneArea + 0.5f * fromMountingPointToIncorporatedPoint;
                    float distance_fromMountingPointToIncorporatedPoint_alongForwardInsidePlaneVector;
                    float distance_fromMountingPointToIncorporatedPoint_alongToSideInsidePlaneVector;
                    if (UtilitiesDXXL_Math.IsDefaultVector(forward_insidePlane))
                    {
                        forward_insidePlane = fromMountingPointToIncorporatedPoint;
                        distance_fromMountingPointToIncorporatedPoint_alongForwardInsidePlaneVector = fromMountingPointToIncorporatedPoint.magnitude;
                        distance_fromMountingPointToIncorporatedPoint_alongToSideInsidePlaneVector = 0.0f;
                    }
                    else
                    {
                        Vector3 fromMountingPointToIncorporatedPoint_alongForwardInsidePlaneVector = Vector3.Project(fromMountingPointToIncorporatedPoint, forward_insidePlane);
                        Vector3 fromMountingPointToIncorporatedPoint_perpToForwardInsidePlaneVector = fromMountingPointToIncorporatedPoint - fromMountingPointToIncorporatedPoint_alongForwardInsidePlaneVector;
                        distance_fromMountingPointToIncorporatedPoint_alongForwardInsidePlaneVector = fromMountingPointToIncorporatedPoint_alongForwardInsidePlaneVector.magnitude;
                        distance_fromMountingPointToIncorporatedPoint_alongToSideInsidePlaneVector = fromMountingPointToIncorporatedPoint_perpToForwardInsidePlaneVector.magnitude;
                    }

                    width = width + distance_fromMountingPointToIncorporatedPoint_alongToSideInsidePlaneVector;
                    length = length + distance_fromMountingPointToIncorporatedPoint_alongForwardInsidePlaneVector;
                }
            }

            finalWidth_ofLastDrawnPlane = width;
            finalLength_ofLastDrawnPlane = length;

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);
            if (UtilitiesDXXL_Math.ApproximatelyZero(width) && UtilitiesDXXL_Math.ApproximatelyZero(length))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(planeMountingPoint, "[<color=#adadadFF><icon=logMessage></color> Plane with extent of 0]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return;
            }

            UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref normal, ref forward_insidePlane, true);
            normal = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(normal);
            plane.Recreate(planeMountingPoint, normal);
            forward_insidePlane = ForceVectorPerpToOtherVector(forward_insidePlane, plane);

            int usedSlotsIn_verticesLoal;
            if (subSegments_signFlipsInterpretation < 0.0f)
            {
                usedSlotsIn_verticesLoal = DrawStruts_caseFixedWorldSpaceSegmentSize(centralPositionOfTheDrawnPlaneArea, planeMountingPoint, normal, color, width, length, forward_insidePlane, ref linesWidth, subSegments_signFlipsInterpretation, lineStyle, stylePatternScaleFactor, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                usedSlotsIn_verticesLoal = DrawStruts_caseFixedNumberOfSegments(centralPositionOfTheDrawnPlaneArea, normal, color, width, length, forward_insidePlane, ref linesWidth, subSegments_signFlipsInterpretation, lineStyle, stylePatternScaleFactor, durationInSec, hiddenByNearerObjects);
            }

            TryDrawPlaneAnchorVisualization(planeMountingPoint, normal, color, anchorVisualizationSize, durationInSec, hiddenByNearerObjects);
            TryDrawPlanesText(usedSlotsIn_verticesLoal, centralPositionOfTheDrawnPlaneArea, normal, color, width, length, forward_insidePlane, linesWidth, text, pointer_as_textAttachStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public const float min_fixedWorldSpaceDistanceOfStrutSegments = 0.001f;
        static int DrawStruts_caseFixedWorldSpaceSegmentSize(Vector3 centralPositionOfTheDrawnPlaneArea, Vector3 planeMountingPoint, Vector3 normal, Color color, float width, float length, Vector3 forward_insidePlane, ref float linesWidth, float subSegments_signFlipsInterpretation, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, float durationInSec, bool hiddenByNearerObjects)
        {
            //-> the outer boundary square has already been drawn
            //-> unrotated local space: the unrotated plane lies horizontal in the y plane, while looking form top down onto it
            //-> "forward_insidePlane" means "forwardAlongPositiveZAxis"
            //-> "centralPositionOfTheDrawnPlaneArea" is the zero origin of this pre-rotation space

            Quaternion rotation = Quaternion.LookRotation(forward_insidePlane, normal);
            Vector3 centralDrawPos_to_planeMountingPoint_inGlobalSpace = planeMountingPoint - centralPositionOfTheDrawnPlaneArea;
            Vector3 planeMountingPoint_inUnrotatedLocalSpace = Quaternion.Inverse(rotation) * centralDrawPos_to_planeMountingPoint_inGlobalSpace;

            float halfWidth = 0.5f * width;
            float halfLength = 0.5f * length;

            int strutsForBothDims = 0;
            int i_nextVertexToFillIn = 0;
            float absDistanceBetweenStruts = Mathf.Max(min_fixedWorldSpaceDistanceOfStrutSegments, (-subSegments_signFlipsInterpretation));
            int maxStrutsFromAnchorToEdge_perSide = 10000;

            //Further struts when walking along positive zDir:
            for (int i = 0; i < maxStrutsFromAnchorToEdge_perSide; i++) //-> starting with "i = 0" -> this positive direction cares for the line through the mountingPoint
            {
                float currentDistanceFromMountingPoint = absDistanceBetweenStruts * i;
                float currentZPos_inUnrotatedLocalSpace = planeMountingPoint_inUnrotatedLocalSpace.z + currentDistanceFromMountingPoint;

                if (currentZPos_inUnrotatedLocalSpace > halfLength)
                {
                    break;
                }
                else
                {
                    i_nextVertexToFillIn = UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(-halfWidth, 0.0f, currentZPos_inUnrotatedLocalSpace), i_nextVertexToFillIn);
                    i_nextVertexToFillIn = UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(+halfWidth, 0.0f, currentZPos_inUnrotatedLocalSpace), i_nextVertexToFillIn);
                    strutsForBothDims++;
                }
            }

            //Further struts when walking along negative zDir:
            for (int i = 1; i < maxStrutsFromAnchorToEdge_perSide; i++) //-> starting with "i = 1" -> the positive direction above already cared for the line through the mountingPoint
            {
                float currentDistanceFromMountingPoint = (-absDistanceBetweenStruts) * i;
                float currentZPos_inUnrotatedLocalSpace = planeMountingPoint_inUnrotatedLocalSpace.z + currentDistanceFromMountingPoint;

                if (currentZPos_inUnrotatedLocalSpace < (-halfLength))
                {
                    break;
                }
                else
                {
                    i_nextVertexToFillIn = UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(-halfWidth, 0.0f, currentZPos_inUnrotatedLocalSpace), i_nextVertexToFillIn);
                    i_nextVertexToFillIn = UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(+halfWidth, 0.0f, currentZPos_inUnrotatedLocalSpace), i_nextVertexToFillIn);
                    strutsForBothDims++;
                }
            }

            //Further struts when walking along positive xDir:
            for (int i = 0; i < maxStrutsFromAnchorToEdge_perSide; i++) //-> starting with "i = 0" -> this positive direction cares for the line through the mountingPoint
            {
                float currentDistanceFromMountingPoint = absDistanceBetweenStruts * i;
                float currentXPos_inUnrotatedLocalSpace = planeMountingPoint_inUnrotatedLocalSpace.x + currentDistanceFromMountingPoint;

                if (currentXPos_inUnrotatedLocalSpace > halfWidth)
                {
                    break;
                }
                else
                {
                    i_nextVertexToFillIn = UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(currentXPos_inUnrotatedLocalSpace, 0.0f, -halfLength), i_nextVertexToFillIn);
                    i_nextVertexToFillIn = UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(currentXPos_inUnrotatedLocalSpace, 0.0f, +halfLength), i_nextVertexToFillIn);
                    strutsForBothDims++;
                }
            }

            //Further struts when walking along negative xDir:
            for (int i = 1; i < maxStrutsFromAnchorToEdge_perSide; i++) //-> starting with "i = 1" -> the positive direction above already cared for the line through the mountingPoint
            {
                float currentDistanceFromMountingPoint = (-absDistanceBetweenStruts) * i;
                float currentXPos_inUnrotatedLocalSpace = planeMountingPoint_inUnrotatedLocalSpace.x + currentDistanceFromMountingPoint;

                if (currentXPos_inUnrotatedLocalSpace < (-halfWidth))
                {
                    break;
                }
                else
                {
                    i_nextVertexToFillIn = UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(currentXPos_inUnrotatedLocalSpace, 0.0f, -halfLength), i_nextVertexToFillIn);
                    i_nextVertexToFillIn = UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(currentXPos_inUnrotatedLocalSpace, 0.0f, +halfLength), i_nextVertexToFillIn);
                    strutsForBothDims++;
                }
            }

            RotateVertices(ref verticesLocal, rotation, i_nextVertexToFillIn);
            linesWidth = Mathf.Min(linesWidth, 0.9f * absDistanceBetweenStruts);

            for (int i_vertex = 0; i_vertex < i_nextVertexToFillIn; i_vertex++)
            {
                UtilitiesDXXL_DrawBasics.Line(centralPositionOfTheDrawnPlaneArea + verticesLocal[i_vertex], centralPositionOfTheDrawnPlaneArea + verticesLocal[i_vertex + 1], color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, plane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                i_vertex++;
            }

            return i_nextVertexToFillIn;
        }

        static int DrawStruts_caseFixedNumberOfSegments(Vector3 centralPositionOfTheDrawnPlaneArea, Vector3 normal, Color color, float width, float length, Vector3 forward_insidePlane, ref float linesWidth, float subSegments_signFlipsInterpretation, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, float durationInSec, bool hiddenByNearerObjects)
        {
            int subSegments = Mathf.Max((int)(subSegments_signFlipsInterpretation), 1);
            int strutsPerDim = subSegments + 1;
            int usedSlotsIn_verticesLoal = strutsPerDim * 4;

            float subSegmentLength_alongWidth = width / subSegments;
            float subSegmentLength_alongLength = length / subSegments;

            float halfWidth = 0.5f * width;
            float halfLength = 0.5f * length;

            //-> the unrotated plane lies horizontal in the y plane.
            //-> the vector names fit a view direction that looks from top down onto this horizontal plane, while "up" means "forwardAlongPositiveZAxis"
            //-> "centralPositionOfTheDrawnPlaneArea" is the zero origin of the pre-rotation space
            Vector3 lowLeftCorner_local_unrotated = Vector3.left * halfWidth + Vector3.back * halfLength;
            Vector3 topLeftCorner_local_unrotated = Vector3.left * halfWidth + Vector3.forward * halfLength;
            Vector3 lowRightCorner_local_unrotated = Vector3.right * halfWidth + Vector3.back * halfLength;

            //4 sides: starting with lowest, then counterclockwise, each ascending along axisPositive
            for (int i_strut = 0; i_strut < strutsPerDim; i_strut++)
            {
                UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, lowLeftCorner_local_unrotated + Vector3.right * subSegmentLength_alongWidth * i_strut, i_strut);
            }

            for (int i_strut = 0; i_strut < strutsPerDim; i_strut++)
            {
                UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, lowRightCorner_local_unrotated + Vector3.forward * subSegmentLength_alongLength * i_strut, strutsPerDim + i_strut);
            }

            for (int i_strut = 0; i_strut < strutsPerDim; i_strut++)
            {
                UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, topLeftCorner_local_unrotated + Vector3.right * subSegmentLength_alongWidth * i_strut, 2 * strutsPerDim + i_strut);
            }

            for (int i_strut = 0; i_strut < strutsPerDim; i_strut++)
            {
                UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, lowLeftCorner_local_unrotated + Vector3.forward * subSegmentLength_alongLength * i_strut, 3 * strutsPerDim + i_strut);
            }

            Quaternion rotation = Quaternion.LookRotation(forward_insidePlane, normal);
            RotateVertices(ref verticesLocal, rotation, usedSlotsIn_verticesLoal);
            linesWidth = UtilitiesDXXL_Math.Min(linesWidth, 0.9f * subSegmentLength_alongWidth, 0.9f * subSegmentLength_alongLength);

            for (int i_vertex = 0; i_vertex < strutsPerDim; i_vertex++)
            {
                UtilitiesDXXL_DrawBasics.Line(centralPositionOfTheDrawnPlaneArea + verticesLocal[i_vertex], centralPositionOfTheDrawnPlaneArea + verticesLocal[i_vertex + 2 * strutsPerDim], color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, plane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                UtilitiesDXXL_DrawBasics.Line(centralPositionOfTheDrawnPlaneArea + verticesLocal[i_vertex + 1 * strutsPerDim], centralPositionOfTheDrawnPlaneArea + verticesLocal[i_vertex + 3 * strutsPerDim], color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, plane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
            }

            return usedSlotsIn_verticesLoal;
        }

        static void TryDrawPlaneAnchorVisualization(Vector3 planeMountingPoint, Vector3 normal, Color color, float anchorVisualizationSize, float durationInSec, bool hiddenByNearerObjects)
        {
            if (anchorVisualizationSize > 0.0f)
            {
                Color colorForNormal = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.33f);
                Vector3 normal_normalized = normal.normalized;
                float lengthOfDrawnNormal = anchorVisualizationSize;
                Vector3 drawnNormal = normal_normalized * lengthOfDrawnNormal;
                Line_fadeableAnimSpeed.InternalDraw(planeMountingPoint, planeMountingPoint + drawnNormal, colorForNormal, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

                float radius = 0.5f * anchorVisualizationSize;
                DrawShapes.Circle(planeMountingPoint, radius, color, normal_normalized, default(Vector3), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
            }
        }

        static void TryDrawPlanesText(int usedSlotsIn_verticesLoal, Vector3 centralPositionOfTheDrawnPlaneArea, Vector3 normal, Color color, float width, float length, Vector3 forward_insidePlane, float linesWidth, string text, bool pointer_as_textAttachStyle, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects)
        {
            if (text != null && text != "")
            {
                if (pointer_as_textAttachStyle)
                {
                    UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, null, centralPositionOfTheDrawnPlaneArea, usedSlotsIn_verticesLoal, 0.4f * linesWidth, Mathf.Max(width, length), color, color, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
                }
                else
                {
                    Vector3 textUp = Vector3.Cross(forward_insidePlane, normal);
                    float textSize = 0.05f * width;
                    float halfLength = 0.5f * length;
                    float autoLineBreakWidth = 1.001f * halfLength; //-> the "1.001f"-factor prevents lineBreak-flicker
                    UtilitiesDXXL_Text.WriteFramed(text, centralPositionOfTheDrawnPlaneArea, color, textSize, forward_insidePlane, textUp, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, autoLineBreakWidth, true, durationInSec, hiddenByNearerObjects);
                }
            }
        }

        public static Vector3 GetPlaneNormalFromTransformEnum(Transform planeTransform, DrawShapes.PlaneNormalFromTransform normalAsEnum_toConvert)
        {
            switch (normalAsEnum_toConvert)
            {
                case DrawShapes.PlaneNormalFromTransform.right:
                    return planeTransform.right;
                case DrawShapes.PlaneNormalFromTransform.up:
                    return planeTransform.up;
                case DrawShapes.PlaneNormalFromTransform.forward:
                    return planeTransform.forward;
                case DrawShapes.PlaneNormalFromTransform.left:
                    return (-planeTransform.right);
                case DrawShapes.PlaneNormalFromTransform.down:
                    return (-planeTransform.up);
                case DrawShapes.PlaneNormalFromTransform.back:
                    return (-planeTransform.forward);
                default:
                    return planeTransform.up;
            }
        }

        public static Vector3 Get_forwardInsidePlane_FromPlameTransformEnum(Transform planeTransform, DrawShapes.PlaneNormalFromTransform normalAsEnum_toConvert)
        {
            switch (normalAsEnum_toConvert)
            {
                case DrawShapes.PlaneNormalFromTransform.right:
                    return planeTransform.up;
                case DrawShapes.PlaneNormalFromTransform.up:
                    return planeTransform.forward;
                case DrawShapes.PlaneNormalFromTransform.forward:
                    return planeTransform.up;
                case DrawShapes.PlaneNormalFromTransform.left:
                    return planeTransform.up;
                case DrawShapes.PlaneNormalFromTransform.down:
                    return planeTransform.forward;
                case DrawShapes.PlaneNormalFromTransform.back:
                    return planeTransform.up;
                default:
                    return planeTransform.up;
            }
        }

        public static float Get_width_FromPlameTransformEnum(Transform planeTransform, DrawShapes.PlaneNormalFromTransform normalAsEnum_toConvert)
        {
            switch (normalAsEnum_toConvert)
            {
                case DrawShapes.PlaneNormalFromTransform.right:
                    return planeTransform.lossyScale.z;
                case DrawShapes.PlaneNormalFromTransform.up:
                    return planeTransform.lossyScale.x;
                case DrawShapes.PlaneNormalFromTransform.forward:
                    return planeTransform.lossyScale.x;
                case DrawShapes.PlaneNormalFromTransform.left:
                    return planeTransform.lossyScale.z;
                case DrawShapes.PlaneNormalFromTransform.down:
                    return planeTransform.lossyScale.x;
                case DrawShapes.PlaneNormalFromTransform.back:
                    return planeTransform.lossyScale.x;
                default:
                    return planeTransform.lossyScale.x;
            }
        }

        public static float Get_length_FromPlameTransformEnum(Transform planeTransform, DrawShapes.PlaneNormalFromTransform normalAsEnum_toConvert)
        {
            switch (normalAsEnum_toConvert)
            {
                case DrawShapes.PlaneNormalFromTransform.right:
                    return planeTransform.lossyScale.y;
                case DrawShapes.PlaneNormalFromTransform.up:
                    return planeTransform.lossyScale.z;
                case DrawShapes.PlaneNormalFromTransform.forward:
                    return planeTransform.lossyScale.y;
                case DrawShapes.PlaneNormalFromTransform.left:
                    return planeTransform.lossyScale.y;
                case DrawShapes.PlaneNormalFromTransform.down:
                    return planeTransform.lossyScale.z;
                case DrawShapes.PlaneNormalFromTransform.back:
                    return planeTransform.lossyScale.y;
                default:
                    return planeTransform.lossyScale.z;
            }
        }

        public static void Rhombus(Vector3 startCornerPosition, Vector3 firstEdge, Vector3 secondEdge, Color color, float linesWidth, string text, int subSegments, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(startCornerPosition, "startCornerPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(firstEdge, "firstEdge")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(secondEdge, "secondEdge")) { return; }

            if (UtilitiesDXXL_Math.ApproximatelyZero(firstEdge) && UtilitiesDXXL_Math.ApproximatelyZero(secondEdge))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(startCornerPosition, "[<color=#adadadFF><icon=logMessage></color> Rhombus with extent of 0]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return;
            }

            Vector3 normal = Vector3.Cross(firstEdge, secondEdge);
            Vector3 normal_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(normal);
            if (UtilitiesDXXL_Math.GetBiggestAbsComponent(normal_normalized) < 0.0001f)
            {
                plane.Recreate(startCornerPosition, Vector3.forward);
            }
            else
            {
                plane.Recreate(startCornerPosition, normal_normalized);
            }

            Vector3 tenthOfVector1 = firstEdge / (float)subSegments;
            Vector3 tenthOfVector2 = secondEdge / (float)subSegments;
            for (int i = 0; i <= subSegments; i++)
            {
                Vector3 areaLine_startPos = startCornerPosition + tenthOfVector1 * (float)i;
                Vector3 areaLine_endPos = areaLine_startPos + secondEdge;
                UtilitiesDXXL_DrawBasics.Line(areaLine_startPos, areaLine_endPos, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, plane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);

                areaLine_startPos = startCornerPosition + tenthOfVector2 * (float)i;
                areaLine_endPos = areaLine_startPos + firstEdge;
                UtilitiesDXXL_DrawBasics.Line(areaLine_startPos, areaLine_endPos, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, plane, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
            }

            if (text != null && text != "")
            {
                float textSize;
                Vector3 textDir;
                Vector3 textUp;
                if (UtilitiesDXXL_Math.ApproximatelyZero(firstEdge))
                {
                    textDir = secondEdge;
                    textUp = default;
                    textSize = 0.05f * secondEdge.magnitude;
                }
                else
                {
                    textDir = firstEdge;
                    if (UtilitiesDXXL_Math.Check_ifTwoVectorsAreApproxParallel_butCanHeadToDifferntDirs_DXXL(firstEdge, secondEdge))
                    {
                        textUp = default;
                    }
                    else
                    {
                        textUp = -secondEdge;
                    }
                    textSize = 0.05f * firstEdge.magnitude;
                }
                textSize = Mathf.Max(textSize, 0.01f);
                UtilitiesDXXL_Text.WriteFramed(text, startCornerPosition, color, textSize, textDir, textUp, DrawText.TextAnchorDXXL.LowerLeft, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
            }

        }

        static InternalDXXL_Edge[] cubesEdgesLocal = new InternalDXXL_Edge[12] { new InternalDXXL_Edge(), new InternalDXXL_Edge(), new InternalDXXL_Edge(), new InternalDXXL_Edge(), new InternalDXXL_Edge(), new InternalDXXL_Edge(), new InternalDXXL_Edge(), new InternalDXXL_Edge(), new InternalDXXL_Edge(), new InternalDXXL_Edge(), new InternalDXXL_Edge(), new InternalDXXL_Edge() };
        public static int Cube(Vector3 position, Vector3 scale, Color colorForCube, Color colorForText, Vector3 up, Vector3 forward, float linesWidth, string text, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects, bool skipDraw, string headerText)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return 0; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(scale, "scale")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up, "up")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(forward, "forward")) { return 0; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);

            if (UtilitiesDXXL_Math.ApproximatelyZero(scale))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(position, "[<color=#adadadFF><icon=logMessage></color> Cube with scale of 0]<br>" + text, colorForCube, linesWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, position, 0);
                return 1;
            }

            UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref up, ref forward, true);
            basePlane.Recreate(position, up);
            forward = ForceVectorPerpToOtherVector(forward, basePlane);

            int usedSlotsIn_verticesLocal = 8;
            UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(-0.5f * scale.x, -0.5f * scale.y, -0.5f * scale.z), 0);
            UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(-0.5f * scale.x, 0.5f * scale.y, -0.5f * scale.z), 1);
            UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(0.5f * scale.x, -0.5f * scale.y, -0.5f * scale.z), 2);
            UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(0.5f * scale.x, 0.5f * scale.y, -0.5f * scale.z), 3);
            UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(-0.5f * scale.x, -0.5f * scale.y, 0.5f * scale.z), 4);
            UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(-0.5f * scale.x, 0.5f * scale.y, 0.5f * scale.z), 5);
            UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(0.5f * scale.x, -0.5f * scale.y, 0.5f * scale.z), 6);
            UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(0.5f * scale.x, 0.5f * scale.y, 0.5f * scale.z), 7);

            Quaternion rotation = Quaternion.LookRotation(forward, up);
            RotateVertices(ref verticesLocal, rotation, usedSlotsIn_verticesLocal);
            Copy_localVertices_to_globalVertices(position, usedSlotsIn_verticesLocal);

            if (skipDraw) { return usedSlotsIn_verticesLocal; }

            cubesEdgesLocal[0].Recreate(verticesLocal[0], verticesLocal[1]);
            cubesEdgesLocal[1].Recreate(verticesLocal[0], verticesLocal[2]);
            cubesEdgesLocal[2].Recreate(verticesLocal[1], verticesLocal[3]);
            cubesEdgesLocal[3].Recreate(verticesLocal[2], verticesLocal[3]);
            cubesEdgesLocal[4].Recreate(verticesLocal[4], verticesLocal[5]);
            cubesEdgesLocal[5].Recreate(verticesLocal[4], verticesLocal[6]);
            cubesEdgesLocal[6].Recreate(verticesLocal[5], verticesLocal[7]);
            cubesEdgesLocal[7].Recreate(verticesLocal[6], verticesLocal[7]);
            cubesEdgesLocal[8].Recreate(verticesLocal[0], verticesLocal[4]);
            cubesEdgesLocal[9].Recreate(verticesLocal[1], verticesLocal[5]);
            cubesEdgesLocal[10].Recreate(verticesLocal[2], verticesLocal[6]);
            cubesEdgesLocal[11].Recreate(verticesLocal[3], verticesLocal[7]);

            if (UtilitiesDXXL_Math.ApproximatelyZero(linesWidth) == false)
            {
                float biggestDim = UtilitiesDXXL_Math.GetBiggestAbsComponent(scale);
                float maxLinesWidth = biggestDim * 0.1f;
                linesWidth = Mathf.Min(linesWidth, maxLinesWidth);
            }

            for (int i = 0; i < cubesEdgesLocal.Length; i++)
            {
                Line_fadeableAnimSpeed.InternalDraw(position + cubesEdgesLocal[i].start, position + cubesEdgesLocal[i].end, colorForCube, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }
            UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, headerText, position, usedSlotsIn_verticesLocal, 0.4f * linesWidth, scale, colorForCube, colorForText, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            return usedSlotsIn_verticesLocal;
        }

        public static void CubeFilled(Vector3 position, Vector3 scale, Color color, Vector3 up, Vector3 forward, float linesWidth, int segmentsPerSide, string text, DrawBasics.LineStyle lineStyle, Color colorOfEdges, float linesWidthOfEdgesCube, float stylePatternScaleFactor, bool useEdgesColorAsTextColor_ifAvailable, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects)
        {
            //The color of the edges can still be different from the color of the fill lines, even if "colorOfEdges" is "default(Color)" (=don't draw edges separately). 
            //-> This happens for "color" with alpha lower than 1, because the fillLines are drawn as planes, and the edges are drawn twice, because they are part of to planes.

            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidthOfEdgesCube, "linesWidthOfEdgesCube")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(scale, "scale")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up, "up")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(forward, "forward")) { return; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);
            bool drawFrameCube = !UtilitiesDXXL_Colors.IsDefaultColor(colorOfEdges);

            if (UtilitiesDXXL_Math.ApproximatelyZero(scale))
            {
                Color textColor = color;
                if (drawFrameCube && useEdgesColorAsTextColor_ifAvailable) { textColor = colorOfEdges; }
                UtilitiesDXXL_DrawBasics.PointFallback(position, "[<color=#adadadFF><icon=logMessage></color> CubeFilled with scale of 0]<br>" + text, textColor, linesWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, position, 0);
                return;
            }

            segmentsPerSide = Mathf.Max(segmentsPerSide, 1);
            UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref up, ref forward, true);
            Vector3 up_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(up);
            basePlane.Recreate(position, up_normalized);
            forward = ForceVectorPerpToOtherVector(forward, basePlane);
            Vector3 forward_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(forward);
            Vector3 right = Vector3.Cross(up_normalized, forward_normalized);
            Vector3 right_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(right);

            Vector3 absScale = UtilitiesDXXL_Math.Abs(scale);
            Vector3 halfAbsScale = 0.5f * absScale;
            if (absScale.y > 0.001 || absScale.z > 0.001)
            {
                if (UtilitiesDXXL_Math.GetBiggestAbsComponent(right_normalized) > 0.1f)
                {
                    Vector3 x_negativeEnd_worldSpace = position - right_normalized * halfAbsScale.x;
                    Vector3 x_positiveEnd_worldSpace = position + right_normalized * halfAbsScale.x;
                    Plane(x_negativeEnd_worldSpace, default(Vector3), right_normalized, color, scale.z, scale.y, up_normalized, linesWidth, null, segmentsPerSide, false, 0.0f, lineStyle, stylePatternScaleFactor, false, durationInSec, hiddenByNearerObjects);
                    Plane(x_positiveEnd_worldSpace, default(Vector3), right_normalized, color, scale.z, scale.y, up_normalized, linesWidth, null, segmentsPerSide, false, 0.0f, lineStyle, stylePatternScaleFactor, false, durationInSec, hiddenByNearerObjects);
                }
            }

            if (absScale.x > 0.001 || absScale.z > 0.001)
            {
                if (UtilitiesDXXL_Math.GetBiggestAbsComponent(up_normalized) > 0.1f)
                {
                    Vector3 y_negativeEnd_worldSpace = position - up_normalized * halfAbsScale.y;
                    Vector3 y_positiveEnd_worldSpace = position + up_normalized * halfAbsScale.y;
                    Plane(y_negativeEnd_worldSpace, default(Vector3), up_normalized, color, scale.x, scale.z, forward_normalized, linesWidth, null, segmentsPerSide, false, 0.0f, lineStyle, stylePatternScaleFactor, false, durationInSec, hiddenByNearerObjects);
                    Plane(y_positiveEnd_worldSpace, default(Vector3), up_normalized, color, scale.x, scale.z, forward_normalized, linesWidth, null, segmentsPerSide, false, 0.0f, lineStyle, stylePatternScaleFactor, false, durationInSec, hiddenByNearerObjects);
                }
            }

            if (absScale.x > 0.001 || absScale.y > 0.001)
            {
                if (UtilitiesDXXL_Math.GetBiggestAbsComponent(forward_normalized) > 0.1f)
                {
                    Vector3 z_negativeEnd_worldSpace = position - forward_normalized * halfAbsScale.z;
                    Vector3 z_positiveEnd_worldSpace = position + forward_normalized * halfAbsScale.z;
                    Plane(z_negativeEnd_worldSpace, default(Vector3), forward_normalized, color, scale.x, scale.y, up_normalized, linesWidth, null, segmentsPerSide, false, 0.0f, lineStyle, stylePatternScaleFactor, false, durationInSec, hiddenByNearerObjects);
                    Plane(z_positiveEnd_worldSpace, default(Vector3), forward_normalized, color, scale.x, scale.y, up_normalized, linesWidth, null, segmentsPerSide, false, 0.0f, lineStyle, stylePatternScaleFactor, false, durationInSec, hiddenByNearerObjects);
                }
            }

            if (drawFrameCube)
            {
                DrawShapes.Cube(position, scale, colorOfEdges, up_normalized, forward_normalized, linesWidthOfEdgesCube, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
            }

            if (text != null && text != "")
            {
                int usedSlotsIn_verticesLocal = 8;
                UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(-0.5f * scale.x, -0.5f * scale.y, -0.5f * scale.z), 0);
                UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(-0.5f * scale.x, 0.5f * scale.y, -0.5f * scale.z), 1);
                UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(0.5f * scale.x, -0.5f * scale.y, -0.5f * scale.z), 2);
                UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(0.5f * scale.x, 0.5f * scale.y, -0.5f * scale.z), 3);
                UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(-0.5f * scale.x, -0.5f * scale.y, 0.5f * scale.z), 4);
                UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(-0.5f * scale.x, 0.5f * scale.y, 0.5f * scale.z), 5);
                UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(0.5f * scale.x, -0.5f * scale.y, 0.5f * scale.z), 6);
                UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, new Vector3(0.5f * scale.x, 0.5f * scale.y, 0.5f * scale.z), 7);

                Quaternion rotation = Quaternion.LookRotation(forward_normalized, up_normalized);
                RotateVertices(ref verticesLocal, rotation, usedSlotsIn_verticesLocal);
                Copy_localVertices_to_globalVertices(position, usedSlotsIn_verticesLocal);

                Color textColor = color;
                if (drawFrameCube && useEdgesColorAsTextColor_ifAvailable) { textColor = colorOfEdges; }
                UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, null, position, usedSlotsIn_verticesLocal, 0.4f * linesWidth, scale, textColor, textColor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
        }

        static InternalDXXL_Plane spheresPlaneForLineOrienation = new InternalDXXL_Plane();
        static int pointsPerSphereCircle = 64;
        static int half_pointsPerSphereCircle = 32;
        static float angleDeg_betweenSpheresCirclePoints = 360.0f / (float)pointsPerSphereCircle;
        static Vector3[] pointsOnSpheresMainCircle = new Vector3[pointsPerSphereCircle];
        static Vector3[] pointsOn_currStrutCircle = new Vector3[pointsPerSphereCircle];
        static List<Vector3> vectors_fromCenter_toStrutAnchorsUnrotated_normalized = new List<Vector3>();
        static List<Vector3> strutAnchors = new List<Vector3>(); //-> this is filled by "Sphere()" and "Ellipsoid()", but not used by them. A caller can use it afterwards in conjunction with the returned "usedSlotsIn_strutAnchorList". E.g. "Capsule()" does it this way.

        public static int Sphere(Vector3 position, float radius, Color color, Vector3 up, Vector3 forward, float linesWidth, string text, int struts, bool onlyUpperHalf, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects, bool skipMainRing)
        {
            //function returns "usedSlotsIn_strutAnchorList";

            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return 0; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up, "up")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(forward, "forward")) { return 0; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);

            if (UtilitiesDXXL_Math.ApproximatelyZero(radius))
            {
                //DO NOT fallback to "Point()" here, because "Point()" may draw a "Sphere()" again, which can create an endless loop.
                //PointFallback();
                //Debug.Log("'Sphere' (at " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(position) + ") is not drawn, because radius is 0.");
                UtilitiesDXXL_List.AddToAVectorList(ref strutAnchors, position, 0);
                return 1;
            }

            if (struts <= 0)
            {
                Debug.Log("'struts' (" + struts + ") must be bigger than 0 and is now automatically set to 2.");
                struts = 2;
            }

            UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref up, ref forward, true);
            spheresPlaneForLineOrienation.Recreate(position, up);
            forward = ForceVectorPerpToOtherVector(forward, spheresPlaneForLineOrienation);
            Quaternion sphereOrientation = Quaternion.LookRotation(forward, up);
            int usedSlotsIn_verticesGlobal = 0;
            int indexIncrements_forCurrentQualitiesSphereCircle = GetIndexIncrements_forCurrentQualitiesSphereCircle();

            if (skipMainRing == false)
            {
                int i_endOfPrecedingSubLine = 0;
                for (int i = 0; i < pointsPerSphereCircle;)
                {
                    pointsOnSpheresMainCircle[i] = radius * _64precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward[i];
                    pointsOnSpheresMainCircle[i] = sphereOrientation * pointsOnSpheresMainCircle[i];
                    pointsOnSpheresMainCircle[i] = position + pointsOnSpheresMainCircle[i];
                    usedSlotsIn_verticesGlobal = UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, pointsOnSpheresMainCircle[i], usedSlotsIn_verticesGlobal);

                    //skip first point (the gap will be closed with the last point):
                    if (i > 0)
                    {
                        UtilitiesDXXL_DrawBasics.Line(pointsOnSpheresMainCircle[i], pointsOnSpheresMainCircle[i_endOfPrecedingSubLine], color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, spheresPlaneForLineOrienation, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    }
                    i_endOfPrecedingSubLine = i;

                    //close gap that was skipped by the first point:
                    if (i == (pointsPerSphereCircle - indexIncrements_forCurrentQualitiesSphereCircle))
                    {
                        UtilitiesDXXL_DrawBasics.Line(pointsOnSpheresMainCircle[i], pointsOnSpheresMainCircle[0], color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, spheresPlaneForLineOrienation, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    }

                    i += indexIncrements_forCurrentQualitiesSphereCircle;
                }
            }

            float angleDeg_betweenStrutAnchorsPoints = 180.0f / (float)struts;
            for (int i = 0; i < struts; i++)
            {
                Quaternion currRotation_aroundUp = Quaternion.AngleAxis(angleDeg_betweenStrutAnchorsPoints * i, Vector3.up);
                UtilitiesDXXL_List.AddToAVectorList(ref vectors_fromCenter_toStrutAnchorsUnrotated_normalized, currRotation_aroundUp * Vector3.forward, i);
            }

            int usedSlotsIn_strutAnchorList = 0;
            int pointsPerStrutCircle = onlyUpperHalf ? half_pointsPerSphereCircle : pointsPerSphereCircle;
            for (int i_strut = 0; i_strut < struts; i_strut++)
            {
                Vector3 perpToUnrotatedStrutPlane = -Vector3.Cross(Vector3.up, vectors_fromCenter_toStrutAnchorsUnrotated_normalized[i_strut]);

                if (UtilitiesDXXL_Math.ApproximatelyZero(perpToUnrotatedStrutPlane)) { break; }

                Quaternion rotationOfStrutRelToEquatorCircle = Quaternion.LookRotation(vectors_fromCenter_toStrutAnchorsUnrotated_normalized[i_strut], perpToUnrotatedStrutPlane);
                Quaternion overallRotationOfStrutCircle = sphereOrientation * rotationOfStrutRelToEquatorCircle;

                Vector3 perpToRotatedStrutPlane = sphereOrientation * perpToUnrotatedStrutPlane;
                spheresPlaneForLineOrienation.Recreate(position, perpToRotatedStrutPlane);

                int i_endOfPrecedingSubLine = 0;
                for (int i_posOnStrut = 0; i_posOnStrut < pointsPerStrutCircle;)
                {
                    pointsOn_currStrutCircle[i_posOnStrut] = overallRotationOfStrutCircle * _64precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward[i_posOnStrut];
                    pointsOn_currStrutCircle[i_posOnStrut] = radius * pointsOn_currStrutCircle[i_posOnStrut];
                    pointsOn_currStrutCircle[i_posOnStrut] = position + pointsOn_currStrutCircle[i_posOnStrut];
                    usedSlotsIn_verticesGlobal = UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, pointsOn_currStrutCircle[i_posOnStrut], usedSlotsIn_verticesGlobal);

                    if (i_posOnStrut == 0)
                    {
                        usedSlotsIn_strutAnchorList = UtilitiesDXXL_List.AddToAVectorList(ref strutAnchors, pointsOn_currStrutCircle[i_posOnStrut], usedSlotsIn_strutAnchorList);
                        Vector3 oppositeStrutAnchor = position - (pointsOn_currStrutCircle[i_posOnStrut] - position);
                        usedSlotsIn_strutAnchorList = UtilitiesDXXL_List.AddToAVectorList(ref strutAnchors, oppositeStrutAnchor, usedSlotsIn_strutAnchorList);
                    }

                    //skip first point (the gap will be closed with the last point):
                    if (i_posOnStrut > 0)
                    {
                        UtilitiesDXXL_DrawBasics.Line(pointsOn_currStrutCircle[i_posOnStrut], pointsOn_currStrutCircle[i_endOfPrecedingSubLine], color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, spheresPlaneForLineOrienation, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    }
                    i_endOfPrecedingSubLine = i_posOnStrut;

                    //close gap that was skipped by the first point:
                    if (i_posOnStrut == (pointsPerStrutCircle - indexIncrements_forCurrentQualitiesSphereCircle))
                    {
                        Vector3 endPoint_local_unrotated = onlyUpperHalf ? (-vectors_fromCenter_toStrutAnchorsUnrotated_normalized[i_strut]) : vectors_fromCenter_toStrutAnchorsUnrotated_normalized[i_strut]; //-> it would also work by accessing "i=32" or "i=0" of "_64precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward"
                        endPoint_local_unrotated = radius * endPoint_local_unrotated;
                        Vector3 endPoint_local_rotated = sphereOrientation * endPoint_local_unrotated;
                        Vector3 endPoint_global = position + endPoint_local_rotated;
                        UtilitiesDXXL_DrawBasics.Line(pointsOn_currStrutCircle[i_posOnStrut], endPoint_global, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, spheresPlaneForLineOrienation, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    }

                    i_posOnStrut += indexIncrements_forCurrentQualitiesSphereCircle;
                }
            }

            if (text != null && text != "")
            {
                float virtualScalePerDim = 2.0f * radius;
                Copy_globalVertices_to_localVertices(position, usedSlotsIn_verticesGlobal);
                UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, null, position, usedSlotsIn_verticesGlobal, linesWidth, virtualScalePerDim, color, color, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }

            return usedSlotsIn_strutAnchorList;
        }

        public static int Ellipsoid(Vector3 position, Vector3 radius, Color color, Vector3 up, Vector3 forward, float linesWidth, string text, int struts, bool onlyUpperHalf, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects, bool skipMainRing)
        {
            //function returns "usedSlotsIn_strutAnchorList";

            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return 0; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(radius, "radius")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up, "up")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(forward, "forward")) { return 0; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);

            if (UtilitiesDXXL_Math.ApproximatelyZero(radius))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(position, "[<color=#adadadFF><icon=logMessage></color> Ellipsoid with extent of 0]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_List.AddToAVectorList(ref strutAnchors, position, 0);
                return 1;
            }

            if (struts <= 0)
            {
                Debug.Log("'struts' (" + struts + ") must be bigger than 0 and is now automatically set to 2.");
                struts = 2;
            }

            UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref up, ref forward, true);
            spheresPlaneForLineOrienation.Recreate(position, up);
            forward = ForceVectorPerpToOtherVector(forward, spheresPlaneForLineOrienation);
            Quaternion sphereOrientation = Quaternion.LookRotation(forward, up);
            bool x_and_z_areZero_but_y_isNot = UtilitiesDXXL_Math.ApproximatelyZero(radius.x) && UtilitiesDXXL_Math.ApproximatelyZero(radius.z);
            int usedSlotsIn_verticesGlobal = 0;
            int indexIncrements_forCurrentQualitiesSphereCircle = GetIndexIncrements_forCurrentQualitiesSphereCircle();

            if (x_and_z_areZero_but_y_isNot)
            {
                usedSlotsIn_verticesGlobal = UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, position, usedSlotsIn_verticesGlobal);
            }
            else
            {
                if (skipMainRing == false)
                {
                    int i_endOfPrecedingSubLine = 0;
                    for (int i = 0; i < pointsPerSphereCircle;)
                    {
                        Vector3 currPointLocal_onMainCircle_withoutSphereRotationApplied_onUnitCircle = _64precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward[i];
                        Vector3 currPointLocal_onMainCircle_withoutSphereRotationApplied_onWarpedCircle = new Vector3(currPointLocal_onMainCircle_withoutSphereRotationApplied_onUnitCircle.x * radius.x, currPointLocal_onMainCircle_withoutSphereRotationApplied_onUnitCircle.y, currPointLocal_onMainCircle_withoutSphereRotationApplied_onUnitCircle.z * radius.z);
                        Vector3 currPointLocal_onMainCircle_withSphereRotationApplied_onWarpedCircle = sphereOrientation * currPointLocal_onMainCircle_withoutSphereRotationApplied_onWarpedCircle;
                        pointsOnSpheresMainCircle[i] = position + currPointLocal_onMainCircle_withSphereRotationApplied_onWarpedCircle;
                        usedSlotsIn_verticesGlobal = UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, pointsOnSpheresMainCircle[i], usedSlotsIn_verticesGlobal);

                        //skip first point (the gap will be closed with the last point):
                        if (i > 0)
                        {
                            UtilitiesDXXL_DrawBasics.Line(pointsOnSpheresMainCircle[i], pointsOnSpheresMainCircle[i_endOfPrecedingSubLine], color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, spheresPlaneForLineOrienation, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                        }
                        i_endOfPrecedingSubLine = i;

                        //close gap that was skipped by the first point:
                        if (i == (pointsPerSphereCircle - indexIncrements_forCurrentQualitiesSphereCircle))
                        {
                            UtilitiesDXXL_DrawBasics.Line(pointsOnSpheresMainCircle[i], pointsOnSpheresMainCircle[0], color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, spheresPlaneForLineOrienation, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                        }

                        i += indexIncrements_forCurrentQualitiesSphereCircle;
                    }
                }
            }

            float angleDeg_betweenStrutAnchorsPoints = 180.0f / (float)struts;
            for (int i = 0; i < struts; i++)
            {
                Quaternion currRotation_aroundUp = Quaternion.AngleAxis(angleDeg_betweenStrutAnchorsPoints * i, Vector3.up);
                UtilitiesDXXL_List.AddToAVectorList(ref vectors_fromCenter_toStrutAnchorsUnrotated_normalized, currRotation_aroundUp * Vector3.forward, i);
            }

            int usedSlotsIn_strutAnchorList = 0;
            int pointsPerStrutCircle = onlyUpperHalf ? half_pointsPerSphereCircle : pointsPerSphereCircle;
            for (int i_strut = 0; i_strut < struts; i_strut++)
            {
                Vector3 perpToUnrotatedUnwarpedStrutPlane = -Vector3.Cross(Vector3.up, vectors_fromCenter_toStrutAnchorsUnrotated_normalized[i_strut]);
                Vector3 perpToUnrotatedWarpedStrutPlane;
                if (UtilitiesDXXL_Math.ApproximatelyZero(radius.x) || UtilitiesDXXL_Math.ApproximatelyZero(radius.z))
                {
                    perpToUnrotatedWarpedStrutPlane = perpToUnrotatedUnwarpedStrutPlane;
                }
                else
                {
                    perpToUnrotatedWarpedStrutPlane = new Vector3(perpToUnrotatedUnwarpedStrutPlane.x * radius.z, 0.0f, perpToUnrotatedUnwarpedStrutPlane.z * radius.x);
                }
                Vector3 perpToRotatedWarpedStrut = sphereOrientation * perpToUnrotatedWarpedStrutPlane;
                spheresPlaneForLineOrienation.Recreate(position, perpToRotatedWarpedStrut);

                int i_endOfPrecedingSubLine = 0;
                for (int i_posOnStrut = 0; i_posOnStrut < pointsPerStrutCircle;)
                {
                    //-> the repeated use of "Quaternion.AngleAxis" could probably be refactored to using something like "_64precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward" (see "Sphere()") to save performance
                    Quaternion rotation_forCurrPoint = Quaternion.AngleAxis(angleDeg_betweenSpheresCirclePoints * i_posOnStrut, perpToUnrotatedUnwarpedStrutPlane);
                    Vector3 currPointLocal_ofCurrStrut_withoutSphereRotationApplied_onUnitCircle = rotation_forCurrPoint * vectors_fromCenter_toStrutAnchorsUnrotated_normalized[i_strut];
                    Vector3 currPointLocal_ofCurrStrut_withoutSphereRotationApplied_onWarpedCircle = new Vector3(currPointLocal_ofCurrStrut_withoutSphereRotationApplied_onUnitCircle.x * radius.x, currPointLocal_ofCurrStrut_withoutSphereRotationApplied_onUnitCircle.y * radius.y, currPointLocal_ofCurrStrut_withoutSphereRotationApplied_onUnitCircle.z * radius.z);
                    Vector3 currPointLocal_ofCurrStrut_withSphereRotationApplied_onWarpedCircle = sphereOrientation * currPointLocal_ofCurrStrut_withoutSphereRotationApplied_onWarpedCircle;
                    pointsOn_currStrutCircle[i_posOnStrut] = position + currPointLocal_ofCurrStrut_withSphereRotationApplied_onWarpedCircle;
                    usedSlotsIn_verticesGlobal = UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, pointsOn_currStrutCircle[i_posOnStrut], usedSlotsIn_verticesGlobal);

                    //currently not used by 'Ellipsoid', but keeps consistency with 'Sphere':
                    if (i_posOnStrut == 0)
                    {
                        usedSlotsIn_strutAnchorList = UtilitiesDXXL_List.AddToAVectorList(ref strutAnchors, pointsOn_currStrutCircle[i_posOnStrut], usedSlotsIn_strutAnchorList);
                        Vector3 oppositeStrutAnchor = position - (pointsOn_currStrutCircle[i_posOnStrut] - position);
                        usedSlotsIn_strutAnchorList = UtilitiesDXXL_List.AddToAVectorList(ref strutAnchors, oppositeStrutAnchor, usedSlotsIn_strutAnchorList);
                    }

                    //skip first point (the gap will be closed with the last point):
                    if (i_posOnStrut > 0)
                    {
                        UtilitiesDXXL_DrawBasics.Line(pointsOn_currStrutCircle[i_posOnStrut], pointsOn_currStrutCircle[i_endOfPrecedingSubLine], color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, spheresPlaneForLineOrienation, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    }
                    i_endOfPrecedingSubLine = i_posOnStrut;

                    //close gap that was skipped by the first point:
                    if (i_posOnStrut == (pointsPerStrutCircle - indexIncrements_forCurrentQualitiesSphereCircle))
                    {
                        //-> "Quaternion.AngleAxis" is used here only for the case of "onlyUpperHalf". In the other case the rotation is always 360° and therefore redundant. It could probably refactored to eliminate the "Quaternion.AngleAxis" all over to save performance.
                        Quaternion rotation_forEndPoint = Quaternion.AngleAxis(angleDeg_betweenSpheresCirclePoints * (i_posOnStrut + indexIncrements_forCurrentQualitiesSphereCircle), perpToUnrotatedUnwarpedStrutPlane);
                        Vector3 endPointLocal_ofCurrStrut_withoutSphereRotationApplied_onUnitCircle = rotation_forEndPoint * vectors_fromCenter_toStrutAnchorsUnrotated_normalized[i_strut];
                        Vector3 endPointLocal_ofCurrStrut_withoutSphereRotationApplied_onWarpedCircle = new Vector3(endPointLocal_ofCurrStrut_withoutSphereRotationApplied_onUnitCircle.x * radius.x, endPointLocal_ofCurrStrut_withoutSphereRotationApplied_onUnitCircle.y * radius.y, endPointLocal_ofCurrStrut_withoutSphereRotationApplied_onUnitCircle.z * radius.z);
                        Vector3 endPointLocal_ofCurrStrut_withSphereRotationApplied_onWarpedCircle = sphereOrientation * endPointLocal_ofCurrStrut_withoutSphereRotationApplied_onWarpedCircle;
                        Vector3 endPointGlobal_ofCurrStrut_withSphereRotationApplied_onWarpedCircle = position + endPointLocal_ofCurrStrut_withSphereRotationApplied_onWarpedCircle;
                        UtilitiesDXXL_DrawBasics.Line(pointsOn_currStrutCircle[i_posOnStrut], endPointGlobal_ofCurrStrut_withSphereRotationApplied_onWarpedCircle, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, spheresPlaneForLineOrienation, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    }

                    i_posOnStrut += indexIncrements_forCurrentQualitiesSphereCircle;
                }
            }

            if (text != null && text != "")
            {
                float virtualScalePerDim = 2.0f * UtilitiesDXXL_Math.GetBiggestAbsComponent(radius);
                Copy_globalVertices_to_localVertices(position, usedSlotsIn_verticesGlobal);
                UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, null, position, usedSlotsIn_verticesGlobal, linesWidth, virtualScalePerDim, color, color, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }

            return usedSlotsIn_strutAnchorList;
        }

        static int GetIndexIncrements_forCurrentQualitiesSphereCircle()
        {
            if (DrawShapes.LinesPerSphereCircle == 64)
            {
                return 1;
            }
            else
            {
                if (DrawShapes.LinesPerSphereCircle == 32)
                {
                    return 2;
                }
                else
                {
                    if (DrawShapes.LinesPerSphereCircle == 16)
                    {
                        return 4;
                    }
                    else
                    {
                        //this is (DrawShapes.LinesPerSphereCircle == 8)
                        return 8;
                    }
                }
            }
        }

        static InternalDXXL_Plane strutPlane = new InternalDXXL_Plane();
        static List<Vector3> verticesGlobal_ofCapsule = new List<Vector3>();
        public static void Capsule(Vector3 position, Color color, float radius, float heightInclBothCaps, Vector3 up, Vector3 forward_insideCrosssectionPlane, float linesWidth, string text, int struts, bool onlyUpperHalfSphere, DrawBasics.LineStyle style, float stylePatternScaleFactor, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(heightInclBothCaps, "heightInclBothCaps")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up, "up")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(forward_insideCrosssectionPlane, "forward_insideCrosssectionPlane")) { return; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);
            UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref up, ref forward_insideCrosssectionPlane, true);
            Vector3 upNormalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(up);
            basePlane.Recreate(position, up);
            forward_insideCrosssectionPlane = ForceVectorPerpToOtherVector(forward_insideCrosssectionPlane, basePlane);

            if (UtilitiesDXXL_Math.ApproximatelyZero(heightInclBothCaps))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(position, "[<color=#adadadFF><icon=logMessage></color> Capsule with extent of 0]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return;
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(radius))
            {
                Vector3 upperSpherePos = position + 0.5f * heightInclBothCaps * upNormalized;
                Vector3 lowerSpherePos = position - 0.5f * heightInclBothCaps * upNormalized;
                Line_fadeableAnimSpeed.InternalDraw(lowerSpherePos, upperSpherePos, color, linesWidth, text, style, stylePatternScaleFactor, 0.0f, null, default, false, 0.0f, 0.0f, 0.005f, durationInSec, hiddenByNearerObjects, false, false);
                return;
            }

            int usedSlotsIn_verticesGlobalOfCapsule = 0;
            float heightOfCyl = heightInclBothCaps - Mathf.Sign(heightInclBothCaps) * 2.0f * radius;
            if (onlyUpperHalfSphere == false)
            {
                int numberOfStrutAnchors_lowerSphere = Sphere(position - 0.5f * heightOfCyl * upNormalized, radius, color, (Mathf.Sign(heightInclBothCaps)) * (-upNormalized), forward_insideCrosssectionPlane, linesWidth, null, struts, true, style, stylePatternScaleFactor, false, durationInSec, hiddenByNearerObjects, false);
                usedSlotsIn_verticesGlobalOfCapsule = UtilitiesDXXL_List.AddRangeToAVectorList(ref verticesGlobal_ofCapsule, strutAnchors, usedSlotsIn_verticesGlobalOfCapsule, numberOfStrutAnchors_lowerSphere);
            }
            int numberOfStrutAnchors_upperSphere = Sphere(position + 0.5f * heightOfCyl * upNormalized, radius, color, (Mathf.Sign(heightInclBothCaps)) * upNormalized, forward_insideCrosssectionPlane, linesWidth, null, struts, true, style, stylePatternScaleFactor, false, durationInSec, hiddenByNearerObjects, false);
            usedSlotsIn_verticesGlobalOfCapsule = UtilitiesDXXL_List.AddRangeToAVectorList(ref verticesGlobal_ofCapsule, strutAnchors, usedSlotsIn_verticesGlobalOfCapsule, numberOfStrutAnchors_upperSphere);

            for (int i = 0; i < numberOfStrutAnchors_upperSphere; i++)
            {
                Vector3 strutAnchor_onLowerSide = strutAnchors[i] - heightOfCyl * upNormalized;
                Vector3 strutPlaneNormal = Vector3.Cross(upNormalized, strutAnchors[i] - (position + 0.5f * heightOfCyl * upNormalized));

                if (UtilitiesDXXL_Math.ApproximatelyZero(strutPlaneNormal)) { break; }

                strutPlane.Recreate(position, strutPlaneNormal);
                UtilitiesDXXL_DrawBasics.Line(strutAnchors[i], strutAnchor_onLowerSide, color, linesWidth, null, style, stylePatternScaleFactor, 0.0f, null, strutPlane, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                if (onlyUpperHalfSphere)
                {
                    int numberOfCircleVertices = DrawShapes.Circle(position - 0.5f * heightOfCyl * upNormalized, radius, color, upNormalized, forward_insideCrosssectionPlane, linesWidth, null, style, stylePatternScaleFactor, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
                    usedSlotsIn_verticesGlobalOfCapsule = UtilitiesDXXL_List.AddRangeToAVectorList(ref verticesGlobal_ofCapsule, verticesGlobal, usedSlotsIn_verticesGlobalOfCapsule, numberOfCircleVertices);
                }
            }

            if (text != null && text != "")
            {
                float virtualScalePerDim = Mathf.Abs(heightInclBothCaps);
                usedSlotsIn_verticesGlobalOfCapsule = UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal_ofCapsule, position + (0.5f * heightOfCyl * upNormalized) + upNormalized * radius, usedSlotsIn_verticesGlobalOfCapsule);
                if (onlyUpperHalfSphere == false)
                {
                    usedSlotsIn_verticesGlobalOfCapsule = UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal_ofCapsule, position - (0.5f * heightOfCyl * upNormalized) - upNormalized * radius, usedSlotsIn_verticesGlobalOfCapsule);
                }
                Copy_customGlobalVertices_to_localVertices(ref verticesGlobal_ofCapsule, position, usedSlotsIn_verticesGlobalOfCapsule);
                UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, null, position, usedSlotsIn_verticesGlobalOfCapsule, linesWidth, virtualScalePerDim, color, color, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
        }

        static int default_cornersOnFlatPyramidBaseCircle = 16;
        static int cornersOnFlatPyramidBaseCircle = default_cornersOnFlatPyramidBaseCircle;
        static InternalDXXL_Plane basePlane = new InternalDXXL_Plane();
        static InternalDXXL_Plane localVertPlane = new InternalDXXL_Plane();
        static List<Vector3> verticesLocal_ofPyramid = new List<Vector3>(); //"Pyramid" cannot use the classwide "verticesLocal"-list, because it gets called from "Vector", which again gets called from "Line(lineStyle = arrows)". So any draw function that executes multiple successive "Line(lineStyle = arrows)"-calls (for example "Plane()") would get their used "verticesLocal"-List overwritten by the PyramidCone of the Arrows after this first "Line()"-call, resulting in undefined behaviour for the succeeding "Line()"-calls.
        public static void Pyramid(Vector3 center_ofBasePlane, float height, float width_ofBase, float length_ofBase, Color color, Vector3 normal_ofBaseTowardsApex, Vector3 up_insideBasePlane, DrawShapes.Shape2DType baseShape, float linesWidth, string text, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(height, "height")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width_ofBase, "width_ofBase")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(length_ofBase, "length_ofBase")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(center_ofBasePlane, "center_ofBasePlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(normal_ofBaseTowardsApex, "normal_ofBaseTowardsApex")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up_insideBasePlane, "up_insideBasePlane")) { return; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);

            if (UtilitiesDXXL_Math.ApproximatelyZero(height) && UtilitiesDXXL_Math.ApproximatelyZero(length_ofBase) && UtilitiesDXXL_Math.ApproximatelyZero(width_ofBase))
            {
                //DO NOT fallback to "Point()" here, because "Point()" calls "Pyramid()" again, which can create an endless loop.
                // PointFallback();
                //Debug.Log("'Pyramid' (at " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(center_ofBasePlane) + ") is not drawn, because height, length_ofBase and width_ofBase is 0.");
                return;
            }

            UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref normal_ofBaseTowardsApex, ref up_insideBasePlane, true);
            basePlane.Recreate(center_ofBasePlane, normal_ofBaseTowardsApex);
            up_insideBasePlane = ForceVectorPerpToOtherVector(up_insideBasePlane, basePlane);


            int usedSlotsIn_verticesLocal_ofPyramid;
            if (baseShape == DrawShapes.Shape2DType.circle)
            {
                //special case: preventing "GetUnitSquared2DShapeAnchorsInsideYPlane" from overwriting "verticesGlobal" because: See declaration of "verticesLocal_ofPyramid"
                usedSlotsIn_verticesLocal_ofPyramid = FillVerticesLocalOfPyramid_withBaseHalfUnitCircle_unrotated(length_ofBase, width_ofBase);
            }
            else
            {
                int usedSlotsIn_verticesGlobal = GetUnitSquared2DShapeAnchorsInsideYPlane(baseShape);
                UtilitiesDXXL_List.CopyContentOfVectorLists(ref verticesLocal_ofPyramid, ref verticesGlobal, usedSlotsIn_verticesGlobal); //-> fixing mixup from global and local, after "GetUnitSquared2DShapeAnchorsInsideYPlane" was (virtually) drawn at origin, but filled the "verticesGlobal"-list
                usedSlotsIn_verticesLocal_ofPyramid = usedSlotsIn_verticesGlobal;
            }

            ScaleZ(ref verticesLocal_ofPyramid, length_ofBase, usedSlotsIn_verticesLocal_ofPyramid);
            ScaleX(ref verticesLocal_ofPyramid, width_ofBase, usedSlotsIn_verticesLocal_ofPyramid);
            Quaternion rotation = Quaternion.LookRotation(up_insideBasePlane, normal_ofBaseTowardsApex);
            RotateVertices(ref verticesLocal_ofPyramid, rotation, usedSlotsIn_verticesLocal_ofPyramid);

            //line on pyramid-base:
            if (baseShape == DrawShapes.Shape2DType.circle && (UtilitiesDXXL_Math.ApproximatelyZero(length_ofBase) || UtilitiesDXXL_Math.ApproximatelyZero(width_ofBase)))
            {
                UtilitiesDXXL_DrawBasics.Line(center_ofBasePlane + verticesLocal_ofPyramid[0], center_ofBasePlane + verticesLocal_ofPyramid[cornersOnFlatPyramidBaseCircle - 1], color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, basePlane, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
            }
            else
            {
                for (int i = 0; i < usedSlotsIn_verticesLocal_ofPyramid; i++)
                {
                    UtilitiesDXXL_DrawBasics.Line(center_ofBasePlane + verticesLocal_ofPyramid[i], center_ofBasePlane + verticesLocal_ofPyramid[UtilitiesDXXL_Math.LoopOvershootingIndexIntoCollectionSize(i + 1, usedSlotsIn_verticesLocal_ofPyramid)], color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, basePlane, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                }
            }

            //line to pyramid-peak:
            Vector3 pyramidPeakLocal = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(normal_ofBaseTowardsApex) * height;
            for (int i = 0; i < usedSlotsIn_verticesLocal_ofPyramid; i++)
            {
                if (baseShape != DrawShapes.Shape2DType.circle4struts || CircleI_marksQuarter(i))
                {
                    localVertPlane.Recreate(center_ofBasePlane, center_ofBasePlane + verticesLocal_ofPyramid[i], center_ofBasePlane + normal_ofBaseTowardsApex, true);
                    if (UtilitiesDXXL_Math.ApproximatelyZero(localVertPlane.normalDir)) { continue; }
                    UtilitiesDXXL_DrawBasics.Line(center_ofBasePlane + verticesLocal_ofPyramid[i], center_ofBasePlane + pyramidPeakLocal, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, localVertPlane, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                }
            }

            usedSlotsIn_verticesLocal_ofPyramid = UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal_ofPyramid, pyramidPeakLocal, usedSlotsIn_verticesLocal_ofPyramid);

            if (text != null && text != "")
            {
                UtilitiesDXXL_List.CopyContentOfVectorLists(ref verticesLocal, ref verticesLocal_ofPyramid, usedSlotsIn_verticesLocal_ofPyramid); //since "Vector()->Cone()->Pyramid()" doesn't specify "text", this line doesn't cause problems as described at the "verticesLocal_ofPyramid"-declaration
                float virtualScalePerDim = 0.35f * UtilitiesDXXL_Math.Max(Mathf.Abs(height), Mathf.Abs(length_ofBase), Mathf.Abs(width_ofBase));
                UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, null, center_ofBasePlane, usedSlotsIn_verticesLocal_ofPyramid, linesWidth, virtualScalePerDim, color, color, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
        }

        static int FillVerticesLocalOfPyramid_withBaseHalfUnitCircle_unrotated(float baseLength, float baseWidth)
        {
            float hullDiameter = 1.0f;
            float hullRadius = 0.5f; //-> a real "unit circle" would have a radius of 1. Therefor the returend circle is called "HalfUnitCircle" in the function name

            if (UtilitiesDXXL_Math.ApproximatelyZero(baseLength))
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(baseWidth))
                {
                    UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal_ofPyramid, Vector3.zero, 0);
                    return 1;
                }
                else
                {
                    cornersOnFlatPyramidBaseCircle = UtilitiesDXXL_DrawBasics.useMoreStrutsForFlatPyramidArrow ? 32 : default_cornersOnFlatPyramidBaseCircle;
                    float distancePerCorner = hullDiameter / (cornersOnFlatPyramidBaseCircle - 1);
                    int usedSlotsIn_verticesLocal_ofPyramid = 0;
                    for (int i = 0; i < cornersOnFlatPyramidBaseCircle; i++)
                    {
                        Vector3 posOfCurrCorner = new Vector3((-hullRadius) + distancePerCorner * i, 0.0f, 0.0f);
                        usedSlotsIn_verticesLocal_ofPyramid = UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal_ofPyramid, posOfCurrCorner, usedSlotsIn_verticesLocal_ofPyramid);
                    }
                    return usedSlotsIn_verticesLocal_ofPyramid;
                }
            }
            else
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(baseWidth))
                {
                    cornersOnFlatPyramidBaseCircle = UtilitiesDXXL_DrawBasics.useMoreStrutsForFlatPyramidArrow ? 32 : default_cornersOnFlatPyramidBaseCircle;
                    float distancePerCorner = hullDiameter / (cornersOnFlatPyramidBaseCircle - 1);
                    int usedSlotsIn_verticesLocal_ofPyramid = 0;
                    for (int i = 0; i < cornersOnFlatPyramidBaseCircle; i++)
                    {
                        Vector3 posOfCurrCorner = new Vector3(0.0f, 0.0f, (-hullRadius) + distancePerCorner * i);
                        usedSlotsIn_verticesLocal_ofPyramid = UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal_ofPyramid, posOfCurrCorner, usedSlotsIn_verticesLocal_ofPyramid);
                    }
                    return usedSlotsIn_verticesLocal_ofPyramid;
                }
                else
                {
                    int corners = 32;
                    int usedSlotsIn_verticesLocal_ofPyramid = 0;
                    for (int i = 0; i < corners; i++)
                    {
                        usedSlotsIn_verticesLocal_ofPyramid = UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal_ofPyramid, hullRadius * _32precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward[i], usedSlotsIn_verticesLocal_ofPyramid);
                    }
                    return usedSlotsIn_verticesLocal_ofPyramid;
                }
            }
        }

        public static void Bipyramid(Vector3 center_ofBasePlane, float heightUp, float heightDown, float width_ofBase, float length_ofBase, Color color, Vector3 normal_ofBaseTowardsUpperApex, Vector3 up_insideBasePlane, DrawShapes.Shape2DType baseShape, float linesWidth, string text, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(heightUp, "heightUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(heightDown, "heightDown")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(length_ofBase, "length_ofBase")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width_ofBase, "width_ofBase")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(center_ofBasePlane, "center_ofBasePlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(normal_ofBaseTowardsUpperApex, "normal_ofBaseTowardsUpperApex")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up_insideBasePlane, "up_insideBasePlane")) { return; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);

            if (UtilitiesDXXL_Math.ApproximatelyZero(heightUp) && UtilitiesDXXL_Math.ApproximatelyZero(heightDown) && UtilitiesDXXL_Math.ApproximatelyZero(length_ofBase) && UtilitiesDXXL_Math.ApproximatelyZero(width_ofBase))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(center_ofBasePlane, "[<color=#adadadFF><icon=logMessage></color> Bipyramid with extent of 0]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return;
            }

            if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(heightUp, heightDown))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(center_ofBasePlane, "[<color=#adadadFF><icon=logMessage></color> Bipyramid with coinciding pyramids. Both heights = " + heightUp + "]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
            }

            UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref normal_ofBaseTowardsUpperApex, ref up_insideBasePlane, true);
            Vector3 upNormalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(normal_ofBaseTowardsUpperApex);
            basePlane.Recreate(center_ofBasePlane, upNormalized);
            up_insideBasePlane = ForceVectorPerpToOtherVector(up_insideBasePlane, basePlane);

            int usedSlotsIn_verticesGlobal = GetUnitSquared2DShapeAnchorsInsideYPlane(baseShape);
            UtilitiesDXXL_List.CopyContentOfVectorLists(ref verticesLocal, ref verticesGlobal, usedSlotsIn_verticesGlobal); //-> fixing mixup from global and local, after "GetUnitSquared2DShapeAnchorsInsideYPlane" was (virtually) drawn at origin, but filled the "verticesGlobal"-list
            int usedSlotsIn_verticesLocal = usedSlotsIn_verticesGlobal;

            ScaleZ(ref verticesLocal, length_ofBase, usedSlotsIn_verticesLocal);
            ScaleX(ref verticesLocal, width_ofBase, usedSlotsIn_verticesLocal);
            Quaternion rotation = Quaternion.LookRotation(up_insideBasePlane, normal_ofBaseTowardsUpperApex);
            RotateVertices(ref verticesLocal, rotation, usedSlotsIn_verticesLocal);

            Vector3 pyramidPeakUpsideLocal = upNormalized * heightUp;
            Vector3 pyramidPeakDownsideLocal = upNormalized * heightDown;
            for (int i = 0; i < usedSlotsIn_verticesLocal; i++)
            {
                UtilitiesDXXL_DrawBasics.Line(center_ofBasePlane + verticesLocal[i], center_ofBasePlane + verticesLocal[UtilitiesDXXL_Math.LoopOvershootingIndexIntoCollectionSize(i + 1, usedSlotsIn_verticesLocal)], color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, basePlane, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                if (baseShape != DrawShapes.Shape2DType.circle4struts || CircleI_marksQuarter(i))
                {
                    localVertPlane.Recreate(center_ofBasePlane, center_ofBasePlane + verticesLocal[i], center_ofBasePlane + normal_ofBaseTowardsUpperApex, true);
                    if (UtilitiesDXXL_Math.ApproximatelyZero(localVertPlane.normalDir)) { continue; }
                    UtilitiesDXXL_DrawBasics.Line(center_ofBasePlane + verticesLocal[i], center_ofBasePlane + pyramidPeakUpsideLocal, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, localVertPlane, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    UtilitiesDXXL_DrawBasics.Line(center_ofBasePlane + verticesLocal[i], center_ofBasePlane + pyramidPeakDownsideLocal, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, localVertPlane, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                }
            }
            usedSlotsIn_verticesLocal = UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, pyramidPeakUpsideLocal, usedSlotsIn_verticesLocal);
            usedSlotsIn_verticesLocal = UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, pyramidPeakDownsideLocal, usedSlotsIn_verticesLocal);

            if (text != null && text != "")
            {
                float virtualScalePerDim = 0.35f * UtilitiesDXXL_Math.Max(Mathf.Abs(heightUp) + Mathf.Abs(heightDown), Mathf.Abs(length_ofBase), Mathf.Abs(width_ofBase));
                UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, null, center_ofBasePlane, usedSlotsIn_verticesLocal, linesWidth, virtualScalePerDim, color, color, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }

        }

        static List<Vector3> verticesLocal_aroundNonExtrudedCenter = new List<Vector3>();
        public static void Cylinder(Vector3 centerPos, float height, float width_ofBase, float length_ofBase, Color color, Vector3 extrusionDirection, Vector3 up_insideCrossSectionPlane, DrawShapes.Shape2DType baseShape, float linesWidth, string text, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(height, "height")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width_ofBase, "width_ofBase")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(length_ofBase, "length_ofBase")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(centerPos, "centerPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(extrusionDirection, "extrusionDirection")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up_insideCrossSectionPlane, "up_insideCrossSectionPlane")) { return; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);

            if (UtilitiesDXXL_Math.ApproximatelyZero(height) && UtilitiesDXXL_Math.ApproximatelyZero(length_ofBase) && UtilitiesDXXL_Math.ApproximatelyZero(width_ofBase))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(centerPos, "[<color=#adadadFF><icon=logMessage></color> Cylinder with extent of 0]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return;
            }

            UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref extrusionDirection, ref up_insideCrossSectionPlane, true);
            basePlane.Recreate(centerPos, extrusionDirection);
            up_insideCrossSectionPlane = ForceVectorPerpToOtherVector(up_insideCrossSectionPlane, basePlane);

            int usedSlotsIn_verticesGlobal = GetUnitSquared2DShapeAnchorsInsideYPlane(baseShape);
            UtilitiesDXXL_List.CopyContentOfVectorLists(ref verticesLocal_aroundNonExtrudedCenter, ref verticesGlobal, usedSlotsIn_verticesGlobal); //-> fixing mixup from global and local, after "GetUnitSquared2DShapeAnchorsInsideYPlane" was (virtually) drawn at origin, but filled the "verticesGlobal"-list
            int usedSlotsIn_verticesLocalAroundNonExtrudedCenter = usedSlotsIn_verticesGlobal;

            ScaleZ(ref verticesLocal_aroundNonExtrudedCenter, length_ofBase, usedSlotsIn_verticesLocalAroundNonExtrudedCenter);
            ScaleX(ref verticesLocal_aroundNonExtrudedCenter, width_ofBase, usedSlotsIn_verticesLocalAroundNonExtrudedCenter);
            Quaternion rotation = Quaternion.LookRotation(up_insideCrossSectionPlane, extrusionDirection);
            RotateVertices(ref verticesLocal_aroundNonExtrudedCenter, rotation, usedSlotsIn_verticesLocalAroundNonExtrudedCenter);

            int usedSlotsIn_verticesLocal = 0;
            Vector3 up_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(extrusionDirection);
            Vector3 centerToUpperPlaneCenter = up_normalized * 0.5f * height;

            for (int i = 0; i < usedSlotsIn_verticesLocalAroundNonExtrudedCenter; i++)
            {
                Vector3 currVertex_onUpperPlane_local = centerToUpperPlaneCenter + verticesLocal_aroundNonExtrudedCenter[i];
                Vector3 currVertex_onLowerPlane_local = (-centerToUpperPlaneCenter) + verticesLocal_aroundNonExtrudedCenter[i];
                Vector3 prevVertex_onUpperPlane_local = centerToUpperPlaneCenter + verticesLocal_aroundNonExtrudedCenter[UtilitiesDXXL_Math.LoopOvershootingIndexIntoCollectionSize(i - 1, usedSlotsIn_verticesLocalAroundNonExtrudedCenter)];
                Vector3 prevVertex_onLowerPlane_local = (-centerToUpperPlaneCenter) + verticesLocal_aroundNonExtrudedCenter[UtilitiesDXXL_Math.LoopOvershootingIndexIntoCollectionSize(i - 1, usedSlotsIn_verticesLocalAroundNonExtrudedCenter)];

                UtilitiesDXXL_DrawBasics.Line(centerPos + currVertex_onUpperPlane_local, centerPos + prevVertex_onUpperPlane_local, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, basePlane, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                UtilitiesDXXL_DrawBasics.Line(centerPos + currVertex_onLowerPlane_local, centerPos + prevVertex_onLowerPlane_local, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, basePlane, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);

                if (baseShape != DrawShapes.Shape2DType.circle4struts || CircleI_marksQuarter(i))
                {
                    localVertPlane.Recreate(centerPos, centerPos + verticesLocal_aroundNonExtrudedCenter[i], centerPos + up_normalized, true);
                    if (UtilitiesDXXL_Math.ApproximatelyZero(localVertPlane.normalDir)) { continue; }
                    UtilitiesDXXL_DrawBasics.Line(centerPos + currVertex_onUpperPlane_local, centerPos + currVertex_onLowerPlane_local, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, localVertPlane, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                }

                usedSlotsIn_verticesLocal = UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, currVertex_onUpperPlane_local, usedSlotsIn_verticesLocal);
                usedSlotsIn_verticesLocal = UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, currVertex_onLowerPlane_local, usedSlotsIn_verticesLocal);
            }

            if (text != null && text != "")
            {
                float virtualScalePerDim = 0.35f * UtilitiesDXXL_Math.Max(Mathf.Abs(height), Mathf.Abs(length_ofBase), Mathf.Abs(width_ofBase));
                UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, null, centerPos, usedSlotsIn_verticesLocal, linesWidth, virtualScalePerDim, color, color, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }

        }

        static List<Vector3> frustumsBigPlaneAnchorPoints_local = new List<Vector3>();
        static List<Vector3> frustumsSmallPlaneAnchorPoints_local = new List<Vector3>();
        public static void Frustum(Vector3 center_ofBigClipPlane, Vector3 center_ofSmallClipPlane, float width_ofBigClipPlane, float height_ofBigClipPlane, float width_ofSmallClipPlane, float height_ofSmallClipPlane, Color color, Vector3 up_insideClippedPlanes, Vector3 fallback_for_normalOfClipPlanesTowardsApex, DrawShapes.Shape2DType clipPlanesShape, float linesWidth, string text, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width_ofBigClipPlane, "width_ofBigClipPlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(height_ofBigClipPlane, "height_ofBigClipPlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width_ofSmallClipPlane, "width_ofSmallClipPlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(height_ofSmallClipPlane, "height_ofSmallClipPlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(center_ofBigClipPlane, "center_ofBigClipPlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(center_ofSmallClipPlane, "center_ofSmallClipPlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up_insideClippedPlanes, "up_insideClippedPlanes")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(fallback_for_normalOfClipPlanesTowardsApex, "fallback_for_normalOfClipPlanesTowardsApex")) { return; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);
            up_insideClippedPlanes = UtilitiesDXXL_Math.OverwriteDefaultVectors(up_insideClippedPlanes, Vector3.forward);
            Vector3 bigPlaneCenter_to_smallPlaneCenter = center_ofSmallClipPlane - center_ofBigClipPlane;

            if (UtilitiesDXXL_Math.ApproximatelyZero(bigPlaneCenter_to_smallPlaneCenter) && UtilitiesDXXL_Math.ApproximatelyZero(height_ofBigClipPlane) && UtilitiesDXXL_Math.ApproximatelyZero(width_ofBigClipPlane) && UtilitiesDXXL_Math.ApproximatelyZero(height_ofSmallClipPlane) && UtilitiesDXXL_Math.ApproximatelyZero(width_ofSmallClipPlane))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(center_ofBigClipPlane, "[<color=#adadadFF><icon=logMessage></color> Frustum with extent of 0]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return;
            }

            Vector3 up = bigPlaneCenter_to_smallPlaneCenter;
            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(center_ofBigClipPlane, center_ofSmallClipPlane))
            {
                fallback_for_normalOfClipPlanesTowardsApex = UtilitiesDXXL_Math.OverwriteDefaultVectors(fallback_for_normalOfClipPlanesTowardsApex, Vector3.up);
                up = fallback_for_normalOfClipPlanesTowardsApex;
            }
            //forward = OverwriteParallelVectorsWithPerpVector(forward, up);
            basePlane.Recreate(center_ofBigClipPlane, up);
            up_insideClippedPlanes = ForceVectorPerpToOtherVector(up_insideClippedPlanes, basePlane);

            int usedSlotsIn_verticesGlobal = GetUnitSquared2DShapeAnchorsInsideYPlane(clipPlanesShape);
            UtilitiesDXXL_List.CopyContentOfVectorLists(ref frustumsBigPlaneAnchorPoints_local, ref verticesGlobal, usedSlotsIn_verticesGlobal); //-> fixing mixup from global and local, after "GetUnitSquared2DShapeAnchorsInsideYPlane" was (virtually) drawn at origin, but filled the "verticesGlobal"-list
            UtilitiesDXXL_List.CopyContentOfVectorLists(ref frustumsSmallPlaneAnchorPoints_local, ref verticesGlobal, usedSlotsIn_verticesGlobal); //-> fixing mixup from global and local, after "GetUnitSquared2DShapeAnchorsInsideYPlane" was (virtually) drawn at origin, but filled the "verticesGlobal"-list
            int usedSlotsIn_frustumsPlaneAnchorLists = usedSlotsIn_verticesGlobal;

            Quaternion rotation = Quaternion.LookRotation(up_insideClippedPlanes, up);
            ScaleZ(ref frustumsBigPlaneAnchorPoints_local, height_ofBigClipPlane, usedSlotsIn_frustumsPlaneAnchorLists);
            ScaleX(ref frustumsBigPlaneAnchorPoints_local, width_ofBigClipPlane, usedSlotsIn_frustumsPlaneAnchorLists);
            RotateVertices(ref frustumsBigPlaneAnchorPoints_local, rotation, usedSlotsIn_frustumsPlaneAnchorLists);
            ScaleZ(ref frustumsSmallPlaneAnchorPoints_local, height_ofSmallClipPlane, usedSlotsIn_frustumsPlaneAnchorLists);
            ScaleX(ref frustumsSmallPlaneAnchorPoints_local, width_ofSmallClipPlane, usedSlotsIn_frustumsPlaneAnchorLists);
            RotateVertices(ref frustumsSmallPlaneAnchorPoints_local, rotation, usedSlotsIn_frustumsPlaneAnchorLists);

            int usedSlotsIn_verticesLocal = 0;
            for (int i = 0; i < usedSlotsIn_frustumsPlaneAnchorLists; i++)
            {
                Vector3 currVertex_onBigPlane_localToBigPlaneCenter = frustumsBigPlaneAnchorPoints_local[i];
                Vector3 prevVertex_onBigPlane_localToBigPlaneCenter = frustumsBigPlaneAnchorPoints_local[UtilitiesDXXL_Math.LoopOvershootingIndexIntoCollectionSize(i - 1, usedSlotsIn_frustumsPlaneAnchorLists)];
                Vector3 currVertex_onSmallPlane_localToBigPlaneCenter = bigPlaneCenter_to_smallPlaneCenter + frustumsSmallPlaneAnchorPoints_local[i];
                Vector3 prevVertex_onSmallPlane_localToBigPlaneCenter = bigPlaneCenter_to_smallPlaneCenter + frustumsSmallPlaneAnchorPoints_local[UtilitiesDXXL_Math.LoopOvershootingIndexIntoCollectionSize(i - 1, usedSlotsIn_frustumsPlaneAnchorLists)];

                //Drawing clip plane outlines:
                UtilitiesDXXL_DrawBasics.Line(center_ofBigClipPlane + currVertex_onBigPlane_localToBigPlaneCenter, center_ofBigClipPlane + prevVertex_onBigPlane_localToBigPlaneCenter, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, basePlane, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                UtilitiesDXXL_DrawBasics.Line(center_ofBigClipPlane + currVertex_onSmallPlane_localToBigPlaneCenter, center_ofBigClipPlane + prevVertex_onSmallPlane_localToBigPlaneCenter, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, basePlane, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);

                //Drawing connection struts between clip planes:
                if (clipPlanesShape != DrawShapes.Shape2DType.circle4struts || CircleI_marksQuarter(i))
                {
                    if (UtilitiesDXXL_Math.ApproximatelyZero(currVertex_onBigPlane_localToBigPlaneCenter) || UtilitiesDXXL_Math.ApproximatelyZero(up))
                    {
                        UtilitiesDXXL_DrawBasics.Line(center_ofBigClipPlane + currVertex_onBigPlane_localToBigPlaneCenter, center_ofBigClipPlane + currVertex_onSmallPlane_localToBigPlaneCenter, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, null, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    }
                    else
                    {
                        localVertPlane.Recreate(center_ofBigClipPlane, center_ofBigClipPlane + currVertex_onBigPlane_localToBigPlaneCenter, center_ofBigClipPlane + up, true);
                        if (UtilitiesDXXL_Math.ApproximatelyZero(localVertPlane.normalDir) == false)
                        {
                            UtilitiesDXXL_DrawBasics.Line(center_ofBigClipPlane + currVertex_onBigPlane_localToBigPlaneCenter, center_ofBigClipPlane + currVertex_onSmallPlane_localToBigPlaneCenter, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, localVertPlane, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                        }
                        else
                        {
                            UtilitiesDXXL_DrawBasics.Line(center_ofBigClipPlane + currVertex_onBigPlane_localToBigPlaneCenter, center_ofBigClipPlane + currVertex_onSmallPlane_localToBigPlaneCenter, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, null, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                        }
                    }
                }

                usedSlotsIn_verticesLocal = UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, currVertex_onBigPlane_localToBigPlaneCenter, usedSlotsIn_verticesLocal);
                usedSlotsIn_verticesLocal = UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, currVertex_onSmallPlane_localToBigPlaneCenter, usedSlotsIn_verticesLocal);
            }

            if (text != null && text != "")
            {
                float height = bigPlaneCenter_to_smallPlaneCenter.magnitude;
                float virtualScalePerDim = 0.35f * UtilitiesDXXL_Math.Max(Mathf.Abs(height), Mathf.Abs(height_ofBigClipPlane), Mathf.Abs(width_ofBigClipPlane), Mathf.Abs(height_ofSmallClipPlane), Mathf.Abs(width_ofSmallClipPlane));
                UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, null, center_ofBigClipPlane, usedSlotsIn_verticesLocal, linesWidth, virtualScalePerDim, color, color, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
        }

        static InternalDXXL_Plane shapePlane = new InternalDXXL_Plane();
        public static int FlatShape(Vector3 centerPosition, float width, float height, Color color, Vector3 normal, Vector3 up_insideShapePlane, DrawShapes.Shape2DType outlineShape, float linesWidth, string text, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, bool flattenRoundLines_intoShapePlane, DrawBasics.LineStyle fillStyle, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects)
        {
            //function returns "usedSlotsIn_verticesGlobal";

            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(height, "height")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width, "width")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return 0; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(centerPosition, "centerPosition")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up_insideShapePlane, "up_insideShapePlane")) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(normal, "normal")) { return 0; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);

            if (UtilitiesDXXL_Math.ApproximatelyZero(height) && UtilitiesDXXL_Math.ApproximatelyZero(width))
            {
                //DO NOT fallback via "PointFallback-2D-()" here, because the 2D-version may draw a "Decagon()", which forwards to here, which can create an endless loop.
                UtilitiesDXXL_DrawBasics.PointFallback(centerPosition, "[<color=#adadadFF><icon=logMessage></color> FlatShape with extent of 0]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, centerPosition, 0);
                return 1;
            }

            UtilitiesDXXL_FlatShapesNormaAndUpCalculation.GetNormalAndUpInsidePlane(out Vector3 normal_final_notGuaranteedNormalized, out Vector3 up_insideShapePlane_normalized, normal, up_insideShapePlane, centerPosition);
            shapePlane.Recreate(centerPosition, normal_final_notGuaranteedNormalized);

            int usedSlotsIn_verticesGlobal = GetUnitSquared2DShapeAnchorsInsideYPlane(outlineShape);
            UtilitiesDXXL_List.CopyContentOfVectorLists(ref verticesLocal, ref verticesGlobal, usedSlotsIn_verticesGlobal); //-> fixing mixup from global and local, after "GetUnitSquared2DShapeAnchorsInsideYPlane" was (virtually) drawn at origin, but filled the "verticesGlobal"-list
            int usedSlotsIn_verticesLocal = usedSlotsIn_verticesGlobal;

            ScaleZ(ref verticesLocal, height, usedSlotsIn_verticesLocal);
            ScaleX(ref verticesLocal, width, usedSlotsIn_verticesLocal);
            Quaternion rotation = Quaternion.LookRotation(up_insideShapePlane_normalized, normal_final_notGuaranteedNormalized);
            RotateVertices(ref verticesLocal, rotation, usedSlotsIn_verticesLocal);
            Copy_localVertices_to_globalVertices(centerPosition, usedSlotsIn_verticesLocal);

            if (outlineShape == DrawShapes.Shape2DType.square && (UtilitiesDXXL_Math.ApproximatelyZero(linesWidth) == false))
            {
                //correcting the dents on square corners:
                Vector3 right_insideShapePlane = Vector3.Cross(normal_final_notGuaranteedNormalized, up_insideShapePlane_normalized);
                Vector3 right_insideShapePlane_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(right_insideShapePlane);
                float halfLineWidth = 0.5f * linesWidth;
                Vector3 cornerDent_correctionVector_inUpDir = up_insideShapePlane_normalized * halfLineWidth;
                Vector3 cornerDent_correctionVector_inRightDir = right_insideShapePlane_normalized * halfLineWidth;
                UtilitiesDXXL_DrawBasics.Line(verticesGlobal[0] - cornerDent_correctionVector_inRightDir, centerPosition + verticesLocal[1] + cornerDent_correctionVector_inRightDir, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, shapePlane, flattenRoundLines_intoShapePlane, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                UtilitiesDXXL_DrawBasics.Line(verticesGlobal[1] - cornerDent_correctionVector_inUpDir, centerPosition + verticesLocal[2] + cornerDent_correctionVector_inUpDir, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, shapePlane, flattenRoundLines_intoShapePlane, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                UtilitiesDXXL_DrawBasics.Line(verticesGlobal[2] + cornerDent_correctionVector_inRightDir, centerPosition + verticesLocal[3] - cornerDent_correctionVector_inRightDir, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, shapePlane, flattenRoundLines_intoShapePlane, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                UtilitiesDXXL_DrawBasics.Line(verticesGlobal[3] + cornerDent_correctionVector_inUpDir, centerPosition + verticesLocal[0] - cornerDent_correctionVector_inUpDir, color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, shapePlane, flattenRoundLines_intoShapePlane, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
            }
            else
            {
                for (int i = 0; i < usedSlotsIn_verticesLocal; i++)
                {
                    UtilitiesDXXL_DrawBasics.Line(verticesGlobal[i], centerPosition + verticesLocal[UtilitiesDXXL_Math.LoopOvershootingIndexIntoCollectionSize(i + 1, usedSlotsIn_verticesLocal)], color, linesWidth, null, lineStyle, stylePatternScaleFactor, 0.0f, null, shapePlane, flattenRoundLines_intoShapePlane, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                }
            }

            float absHeight = Mathf.Abs(height);
            DrawShapeFilling(outlineShape, fillStyle, usedSlotsIn_verticesGlobal, distanceOfUnscaledFillLines_asFractionOfShapeSize * absHeight, color, up_insideShapePlane_normalized, stylePatternScaleFactor, shapePlane, durationInSec, hiddenByNearerObjects);

            if (text != null && text != "")
            {
                float virtualScalePerDim = 0.35f * Mathf.Max(Mathf.Abs(height), Mathf.Abs(width));
                UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, null, centerPosition, usedSlotsIn_verticesLocal, linesWidth, virtualScalePerDim, color, color, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }

            return usedSlotsIn_verticesGlobal;
        }

        static int GetUnitSquared2DShapeAnchorsInsideYPlane(DrawShapes.Shape2DType baseShape)
        {
            //function returns "usedSlotsIn_verticesGlobal" and has filled "verticesGlobal" around origin with the requested shape

            switch (baseShape)
            {
                case DrawShapes.Shape2DType.triangle:
                    return Triangle(Vector3.zero, 0.5f, default, Vector3.up, Vector3.forward, default, null, default, 1.0f, default, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.square:
                    //return Square(Vector3.zero, 1.0f, default, Vector3.up, Vector3.forward, default, null, default, 1.0f, default, false, false, 0.0f, true, true);
                    UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, new Vector3(-0.5f, 0.0f, -0.5f), 0);
                    UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, new Vector3(0.5f, 0.0f, -0.5f), 1);
                    UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, new Vector3(0.5f, 0.0f, 0.5f), 2);
                    UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, new Vector3(-0.5f, 0.0f, 0.5f), 3);
                    return 4;

                case DrawShapes.Shape2DType.pentagon:
                    return Pentagon(Vector3.zero, 0.5f, default, Vector3.up, Vector3.forward, default, null, default, 1.0f, default, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.hexagon:
                    return Hexagon(Vector3.zero, 0.5f, default, Vector3.up, Vector3.forward, default, null, default, 1.0f, default, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.septagon:
                    return Septagon(Vector3.zero, 0.5f, default, Vector3.up, Vector3.forward, default, null, default, 1.0f, default, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.octagon:
                    return Octagon(Vector3.zero, 0.5f, default, Vector3.up, Vector3.forward, default, null, default, 1.0f, default, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.decagon:
                    return Decagon(Vector3.zero, 0.5f, default, Vector3.up, Vector3.forward, default, null, default, 1.0f, default, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.circle:
                    return Circle(Vector3.zero, 0.5f, default, Vector3.up, Vector3.forward, default, null, default, 1.0f, default, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.circle4struts:
                    return Circle(Vector3.zero, 0.5f, default, Vector3.up, Vector3.forward, default, null, default, 1.0f, default, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.star3:
                    return Star(Vector3.zero, 0.5f, default, 3, 0.5f, Vector3.up, Vector3.forward, default, null, default, 1.0f, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.star4:
                    return Star(Vector3.zero, 0.5f, default, 4, 0.5f, Vector3.up, Vector3.forward, default, null, default, 1.0f, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.star5:
                    return Star(Vector3.zero, 0.5f, default, 5, 0.5f, Vector3.up, Vector3.forward, default, null, default, 1.0f, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.star6:
                    return Star(Vector3.zero, 0.5f, default, 6, 0.5f, Vector3.up, Vector3.forward, default, null, default, 1.0f, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.star8:
                    return Star(Vector3.zero, 0.5f, default, 8, 0.5f, Vector3.up, Vector3.forward, default, null, default, 1.0f, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.star10:
                    return Star(Vector3.zero, 0.5f, default, 10, 0.5f, Vector3.up, Vector3.forward, default, null, default, 1.0f, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.star16:
                    return Star(Vector3.zero, 0.5f, default, 16, 0.5f, Vector3.up, Vector3.forward, default, null, default, 1.0f, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.star32:
                    return Star(Vector3.zero, 0.5f, default, 32, 0.5f, Vector3.up, Vector3.forward, default, null, default, 1.0f, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.star64:
                    return Star(Vector3.zero, 0.5f, default, 64, 0.5f, Vector3.up, Vector3.forward, default, null, default, 1.0f, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.ellipse05:
                    return Ellipse(Vector3.zero, 0.5f * 0.5f, 0.5f, default, Vector3.up, Vector3.forward, default, null, default, 1.0f, default, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.ellipse025:
                    return Ellipse(Vector3.zero, 0.5f * 0.25f, 0.5f, default, Vector3.up, Vector3.forward, default, null, default, 1.0f, default, false, false, 0.0f, true, true);

                case DrawShapes.Shape2DType.ellipse0125:
                    return Ellipse(Vector3.zero, 0.5f * 0.125f, 0.5f, default, Vector3.up, Vector3.forward, default, null, default, 1.0f, default, false, false, 0.0f, true, true);

                default:
                    Debug.LogError("BaseShape " + baseShape + " not yet implemented.");
                    return 0;
            }
        }

        static bool CircleI_marksQuarter(int i)
        {
            if (i == 0 || i == 8 || i == 16 || i == 24)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static List<InternalDXXL_Edge> fillEdges = new List<InternalDXXL_Edge>();
        static List<InternalDXXL_Edge> edges = new List<InternalDXXL_Edge>();
        static List<InternalDXXL_PolyFillLine> parallelFillLines = new List<InternalDXXL_PolyFillLine>();
        static int RecalcFillingOfPolygon(int usedSlotsInVerticesGlobalList, Vector3 up_insidePolyPlane_normalized, float distanceBetweenLines)
        {
            //returns "usedSlotsInFillEdgesList"

            //this function can only handle convex polygons. Concave polygons not implemented yet.
            //verticesGlobal have to lie in the same plane.

            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }

            int usedSlotsInFillEdgesList = 0;
            if (usedSlotsInVerticesGlobalList == 0) { return 0; }

            if (UtilitiesDXXL_Math.ApproximatelyZero(up_insidePolyPlane_normalized))
            {
                Debug.LogError("'up_insidePolyPlane_normalized' is not allowed to be zero.");
                return 0;
            }

            int usedSlots_inEdgesList = 0;
            Vector3 lowestVertex = verticesGlobal[0];
            Vector3 highestVertex = verticesGlobal[0];
            for (int i = 0; i < usedSlotsInVerticesGlobalList; i++)
            {
                Vector3 vertexToLowest = lowestVertex - verticesGlobal[i];
                if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(vertexToLowest, up_insidePolyPlane_normalized))
                {
                    lowestVertex = verticesGlobal[i];
                }

                Vector3 vertexToHighest = highestVertex - verticesGlobal[i];
                if (UtilitiesDXXL_Math.Check_ifVectorsPointAwayFromEachOther_perpCountsAsPointingAwayFromEachOther(vertexToHighest, up_insidePolyPlane_normalized))
                {
                    highestVertex = verticesGlobal[i];
                }

                Vector3 edgeStart = verticesGlobal[i];
                Vector3 edgeEnd = verticesGlobal[UtilitiesDXXL_Math.LoopOvershootingIndexIntoCollectionSize(i + 1, usedSlotsInVerticesGlobalList)];
                if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(edgeStart, edgeEnd) == false)
                {
                    usedSlots_inEdgesList = AddToAEdgeList(ref edges, edgeStart, edgeEnd, true, usedSlots_inEdgesList);
                }
            }

            Vector3 lowestToHighestVertex = highestVertex - lowestVertex;
            Vector3 lowestToHighestVertex_parallelToUp = Vector3.Project(lowestToHighestVertex, up_insidePolyPlane_normalized);
            float distance_fromlowestToHighestVertex_alongUp = lowestToHighestVertex_parallelToUp.magnitude;
            distanceBetweenLines = UtilitiesDXXL_Math.Max(distanceBetweenLines, 0.001f, 0.0005f * distance_fromlowestToHighestVertex_alongUp);
            float currDistanceFromLowestVertex = 0.499f * distanceBetweenLines;

            int usedSlotsIn_polyFillLinesList = 0;
            int loopIterationCounter = 0;
            while (currDistanceFromLowestVertex < distance_fromlowestToHighestVertex_alongUp)
            {
                Vector3 planeAnchorPos = lowestVertex + up_insidePolyPlane_normalized * currDistanceFromLowestVertex;
                usedSlotsIn_polyFillLinesList = AddToAPolyFillLinesList(ref parallelFillLines, planeAnchorPos, up_insidePolyPlane_normalized, usedSlotsIn_polyFillLinesList);
                currDistanceFromLowestVertex = currDistanceFromLowestVertex + distanceBetweenLines;

                loopIterationCounter++;
                if (loopIterationCounter > 100000)
                {
                    Debug.LogError("Too many while loop iterations. Forced quit to prevent freeze.   distanceBetweenLines: " + distanceBetweenLines + "    distance_fromlowestToHighestVertex_alongUp: " + distance_fromlowestToHighestVertex_alongUp);
                    break;
                }
            }

            for (int i_fillLine = 0; i_fillLine < usedSlotsIn_polyFillLinesList; i_fillLine++)
            {
                for (int i_edge = 0; i_edge < usedSlots_inEdgesList; i_edge++)
                {
                    parallelFillLines[i_fillLine].IntersectWithEdge(edges[i_edge]);
                }
                parallelFillLines[i_fillLine].RemoveDuplicateIntersections();
            }

            for (int i_fillLine = 0; i_fillLine < usedSlotsIn_polyFillLinesList; i_fillLine++)
            {
                if (parallelFillLines[i_fillLine].usedSlotsInFillLineAnchorsList >= 2)
                {
                    usedSlotsInFillEdgesList = AddToAEdgeList(ref fillEdges, parallelFillLines[i_fillLine].fillLineAnchors[0], parallelFillLines[i_fillLine].fillLineAnchors[1], false, usedSlotsInFillEdgesList);
                }
            }

            return usedSlotsInFillEdgesList;
        }

        static int AddToAPolyFillLinesList(ref List<InternalDXXL_PolyFillLine> targetList, Vector3 posOfPerpPlane, Vector3 normalOfPerpPlane, int i_ofSlotWhereToAdd)
        {
            //function returns "i_nextFreeSlot"
            if (i_ofSlotWhereToAdd < targetList.Count)
            {
                targetList[i_ofSlotWhereToAdd].plane_perpToPolygon.Recreate(posOfPerpPlane, normalOfPerpPlane);
                targetList[i_ofSlotWhereToAdd].usedSlotsInFillLineAnchorsList = 0;
            }
            else
            {
                InternalDXXL_PolyFillLine newPolyFillLine = new InternalDXXL_PolyFillLine(posOfPerpPlane, normalOfPerpPlane);
                targetList.Add(newPolyFillLine);
            }
            i_ofSlotWhereToAdd++;
            return i_ofSlotWhereToAdd;
        }

        static int AddToAEdgeList(ref List<InternalDXXL_Edge> targetList, Vector3 start, Vector3 end, bool calcLine, int i_ofSlotWhereToAdd)
        {
            //function returns "i_nextFreeSlot"
            if (i_ofSlotWhereToAdd < targetList.Count)
            {
                targetList[i_ofSlotWhereToAdd].start = start;
                targetList[i_ofSlotWhereToAdd].end = end;
                if (calcLine)
                {
                    targetList[i_ofSlotWhereToAdd].CalcLine();
                }
            }
            else
            {
                InternalDXXL_Edge newEdge = new InternalDXXL_Edge();
                newEdge.start = start;
                newEdge.end = end;
                if (calcLine)
                {
                    newEdge.CalcLine();
                }
                targetList.Add(newEdge);
            }
            i_ofSlotWhereToAdd++;
            return i_ofSlotWhereToAdd;
        }

        public static void DrawShapeFilling(DrawShapes.Shape2DType baseShape, DrawBasics.LineStyle fillStyle, int usedSlotsIn_verticesGlobal, float distanceBetweenLines, Color color, Vector3 up_normalized, float stylePatternScaleFactor, InternalDXXL_Plane shapePlane, float durationInSec, bool hiddenByNearerObjects)
        {
            if (fillStyle != DrawBasics.LineStyle.invisible)
            {
                if (Shape2DisConvex(baseShape))
                {
                    float scaledDistanceBetweenLines = distanceBetweenLines / DrawShapes.ShapeFillDensity;
                    int usedSlotsInFillEdgesList = RecalcFillingOfPolygon(usedSlotsIn_verticesGlobal, up_normalized, scaledDistanceBetweenLines);
                    for (int i = 0; i < usedSlotsInFillEdgesList; i++)
                    {
                        UtilitiesDXXL_DrawBasics.Line(fillEdges[i].start, fillEdges[i].end, color, 0.0f, null, fillStyle, stylePatternScaleFactor, 0.0f, null, shapePlane, false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    }
                }
                else
                {
                    Debug.Log("Fillstyle '" + fillStyle + "' is only supported for convex shapes, so not for '" + baseShape + "'.");
                }
            }
        }

        static Vector3[] _64precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward = new Vector3[] { new Vector3(0f, 0f, 1f), new Vector3(0.09801714f, 0f, 0.9951847f), new Vector3(0.1950903f, 0f, 0.9807853f), new Vector3(0.2902847f, 0f, 0.9569404f), new Vector3(0.3826834f, 0f, 0.9238795f), new Vector3(0.4713968f, 0f, 0.8819212f), new Vector3(0.5555702f, 0f, 0.8314697f), new Vector3(0.6343933f, 0f, 0.7730104f), new Vector3(0.7071068f, 0f, 0.7071067f), new Vector3(0.7730104f, 0f, 0.6343933f), new Vector3(0.8314696f, 0f, 0.5555702f), new Vector3(0.8819213f, 0f, 0.4713967f), new Vector3(0.9238795f, 0f, 0.3826834f), new Vector3(0.9569404f, 0f, 0.2902846f), new Vector3(0.9807853f, 0f, 0.1950902f), new Vector3(0.9951848f, 0f, 0.0980171f), new Vector3(1.0f, 0f, 0f), new Vector3(0.9951847f, 0f, -0.09801733f), new Vector3(0.9807853f, 0f, -0.1950903f), new Vector3(0.9569404f, 0f, -0.2902846f), new Vector3(0.9238795f, 0f, -0.3826835f), new Vector3(0.8819212f, 0f, -0.4713969f), new Vector3(0.8314695f, 0f, -0.5555704f), new Vector3(0.7730105f, 0f, -0.6343933f), new Vector3(0.7071068f, 0f, -0.7071067f), new Vector3(0.6343932f, 0f, -0.7730104f), new Vector3(0.5555702f, 0f, -0.8314697f), new Vector3(0.4713966f, 0f, -0.8819213f), new Vector3(0.3826833f, 0f, -0.9238796f), new Vector3(0.2902847f, 0f, -0.9569403f), new Vector3(0.1950903f, 0f, -0.9807853f), new Vector3(0.09801709f, 0f, -0.9951847f), new Vector3(0f, 0f, -1f), new Vector3(-0.09801727f, 0f, -0.9951847f), new Vector3(-0.1950905f, 0f, -0.9807853f), new Vector3(-0.2902849f, 0f, -0.9569403f), new Vector3(-0.3826834f, 0f, -0.9238794f), new Vector3(-0.4713968f, 0f, -0.8819213f), new Vector3(-0.5555703f, 0f, -0.8314694f), new Vector3(-0.6343934f, 0f, -0.7730104f), new Vector3(-0.7071069f, 0f, -0.7071067f), new Vector3(-0.7730104f, 0f, -0.6343933f), new Vector3(-0.8314698f, 0f, -0.5555701f), new Vector3(-0.8819213f, 0f, -0.4713967f), new Vector3(-0.9238797f, 0f, -0.3826832f), new Vector3(-0.9569404f, 0f, -0.2902846f), new Vector3(-0.9807853f, 0f, -0.1950904f), new Vector3(-0.9951848f, 0f, -0.09801698f), new Vector3(-1.0f, 0f, 0f), new Vector3(-0.9951847f, 0f, 0.09801739f), new Vector3(-0.9807853f, 0f, 0.1950904f), new Vector3(-0.9569402f, 0f, 0.2902851f), new Vector3(-0.9238795f, 0f, 0.3826835f), new Vector3(-0.8819213f, 0f, 0.4713967f), new Vector3(-0.8314695f, 0f, 0.5555705f), new Vector3(-0.7730104f, 0f, 0.6343933f), new Vector3(-0.7071066f, 0f, 0.707107f), new Vector3(-0.6343932f, 0f, 0.7730105f), new Vector3(-0.5555703f, 0f, 0.8314695f), new Vector3(-0.4713965f, 0f, 0.8819214f), new Vector3(-0.3826834f, 0f, 0.9238796f), new Vector3(-0.2902844f, 0f, 0.9569404f), new Vector3(-0.1950902f, 0f, 0.9807853f), new Vector3(-0.09801676f, 0f, 0.9951848f) };
        static Vector3[] _32precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward = new Vector3[] { new Vector3(0f, 0f, 1f), new Vector3(0.1950903f, 0f, 0.9807853f), new Vector3(0.3826834f, 0f, 0.9238795f), new Vector3(0.5555702f, 0f, 0.8314697f), new Vector3(0.7071068f, 0f, 0.7071067f), new Vector3(0.8314696f, 0f, 0.5555702f), new Vector3(0.9238795f, 0f, 0.3826834f), new Vector3(0.9807853f, 0f, 0.1950902f), new Vector3(1.0f, 0f, 0f), new Vector3(0.9807853f, 0f, -0.1950903f), new Vector3(0.9238795f, 0f, -0.3826835f), new Vector3(0.8314695f, 0f, -0.5555704f), new Vector3(0.7071068f, 0f, -0.7071067f), new Vector3(0.5555702f, 0f, -0.8314697f), new Vector3(0.3826833f, 0f, -0.9238796f), new Vector3(0.1950903f, 0f, -0.9807853f), new Vector3(0f, 0f, -1f), new Vector3(-0.1950905f, 0f, -0.9807853f), new Vector3(-0.3826834f, 0f, -0.9238794f), new Vector3(-0.5555703f, 0f, -0.8314694f), new Vector3(-0.7071069f, 0f, -0.7071067f), new Vector3(-0.8314698f, 0f, -0.5555701f), new Vector3(-0.9238797f, 0f, -0.3826832f), new Vector3(-0.9807853f, 0f, -0.1950904f), new Vector3(-1.0f, 0f, 0f), new Vector3(-0.9807853f, 0f, 0.1950904f), new Vector3(-0.9238795f, 0f, 0.3826835f), new Vector3(-0.8314695f, 0f, 0.5555705f), new Vector3(-0.7071066f, 0f, 0.707107f), new Vector3(-0.5555703f, 0f, 0.8314695f), new Vector3(-0.3826834f, 0f, 0.9238796f), new Vector3(-0.1950902f, 0f, 0.9807853f) };
        static Vector3[] _32precalcedUnitCirclePoints_aroundOrigin_insideZPlane_startingAtUpward_clockwiseWhenLookingAlongZForward = new Vector3[] { new Vector3(0f, 1f, 0f), new Vector3(0.1950903f, 0.9807853f, 0f), new Vector3(0.3826834f, 0.9238795f, 0f), new Vector3(0.5555702f, 0.8314697f, 0f), new Vector3(0.7071068f, 0.7071067f, 0f), new Vector3(0.8314696f, 0.5555702f, 0f), new Vector3(0.9238795f, 0.3826834f, 0f), new Vector3(0.9807853f, 0.1950902f, 0f), new Vector3(1.0f, 0f, 0f), new Vector3(0.9807853f, -0.1950903f, 0f), new Vector3(0.9238795f, -0.3826835f, 0f), new Vector3(0.8314695f, -0.5555704f, 0f), new Vector3(0.7071068f, -0.7071067f, 0f), new Vector3(0.5555702f, -0.8314697f, 0f), new Vector3(0.3826833f, -0.9238796f, 0f), new Vector3(0.1950903f, -0.9807853f, 0f), new Vector3(0f, -1f, 0f), new Vector3(-0.1950905f, -0.9807853f, 0f), new Vector3(-0.3826834f, -0.9238794f, 0f), new Vector3(-0.5555703f, -0.8314694f, 0f), new Vector3(-0.7071069f, -0.7071067f, 0f), new Vector3(-0.8314698f, -0.5555701f, 0f), new Vector3(-0.9238797f, -0.3826832f, 0f), new Vector3(-0.9807853f, -0.1950904f, 0f), new Vector3(-1.0f, 0f, 0f), new Vector3(-0.9807853f, 0.1950904f, 0f), new Vector3(-0.9238795f, 0.3826835f, 0f), new Vector3(-0.8314695f, 0.5555705f, 0f), new Vector3(-0.7071066f, 0.707107f, 0f), new Vector3(-0.5555703f, 0.8314695f, 0f), new Vector3(-0.3826834f, 0.9238796f, 0f), new Vector3(-0.1950902f, 0.9807853f, 0f) };
        static Vector3[] _10precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward = new Vector3[] { new Vector3(0f, 0f, 1f), new Vector3(0.5877853f, 0f, 0.809017f), new Vector3(0.9510565f, 0f, 0.309017f), new Vector3(0.9510565f, 0f, -0.3090172f), new Vector3(0.5877852f, 0f, -0.8090171f), new Vector3(0f, 0f, -1f), new Vector3(-0.5877855f, 0f, -0.8090168f), new Vector3(-0.9510564f, 0f, -0.3090171f), new Vector3(-0.9510565f, 0f, 0.3090172f), new Vector3(-0.5877853f, 0f, 0.8090169f) };
        static Vector3[] _8precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward = new Vector3[] { new Vector3(0f, 0f, 1f), new Vector3(0.7071068f, 0f, 0.7071067f), new Vector3(1f, 0f, 0f), new Vector3(0.7071068f, 0f, -0.7071067f), new Vector3(0f, 0f, -1f), new Vector3(-0.7071069f, 0f, -0.7071067f), new Vector3(-1f, 0f, 0f), new Vector3(-0.7071066f, 0f, 0.707107f) };
        static Vector3[] _7precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward = new Vector3[] { new Vector3(0f, 0f, 1f), new Vector3(0.7818314f, 0f, 0.6234899f), new Vector3(0.974928f, 0f, -0.2225208f), new Vector3(0.4338838f, 0f, -0.9009688f), new Vector3(-0.4338835f, 0f, -0.900969f), new Vector3(-0.9749281f, 0f, -0.2225206f), new Vector3(-0.7818316f, 0f, 0.6234897f) };
        static Vector3[] _6precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward = new Vector3[] { new Vector3(0f, 0f, 1f), new Vector3(0.8660254f, 0f, 0.5f), new Vector3(0.8660254f, 0f, -0.5f), new Vector3(0f, 0f, -1f), new Vector3(-0.8660255f, 0f, -0.5f), new Vector3(-0.8660255f, 0f, 0.5f) };
        static Vector3[] _5precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward = new Vector3[] { new Vector3(0f, 0f, 1f), new Vector3(0.9510565f, 0f, 0.309017f), new Vector3(0.5877852f, 0f, -0.8090171f), new Vector3(-0.5877855f, 0f, -0.8090168f), new Vector3(-0.9510565f, 0f, 0.3090172f) };
        static Vector3[] _4precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward = new Vector3[] { new Vector3(0f, 0f, 1f), new Vector3(1f, 0f, 0f), new Vector3(0f, 0f, -1f), new Vector3(-1f, 0f, 0f) };
        static Vector3[] _3precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward = new Vector3[] { new Vector3(0f, 0f, 1f), new Vector3(0.8660254f, 0f, -0.5f), new Vector3(-0.8660255f, 0f, -0.5f) };

        static bool TryFillPrecalcedUnitCirclePointsList(int requestedNumberOfPolygonCorners)
        {
            //-> this function returns "requestedAmountOfCorners_hasPrecalcedPoints"

            switch (requestedNumberOfPolygonCorners)
            {
                case 3:
                    for (int i = 0; i < 3; i++)
                    {
                        UtilitiesDXXL_List.AddToAVectorList(ref precalcedUnitCirclePoints_forCurrentlyRequestedFlexNumberedFlatPolygon_aroundOrigin_insideYPlane_startingWithFirstVertexAtZForward_clockwiseWhenLookingDownward, _3precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward[i], i);
                    }
                    return true;
                case 4:
                    for (int i = 0; i < 4; i++)
                    {
                        UtilitiesDXXL_List.AddToAVectorList(ref precalcedUnitCirclePoints_forCurrentlyRequestedFlexNumberedFlatPolygon_aroundOrigin_insideYPlane_startingWithFirstVertexAtZForward_clockwiseWhenLookingDownward, _4precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward[i], i);
                    }
                    return true;
                case 5:
                    for (int i = 0; i < 5; i++)
                    {
                        UtilitiesDXXL_List.AddToAVectorList(ref precalcedUnitCirclePoints_forCurrentlyRequestedFlexNumberedFlatPolygon_aroundOrigin_insideYPlane_startingWithFirstVertexAtZForward_clockwiseWhenLookingDownward, _5precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward[i], i);
                    }
                    return true;
                case 6:
                    for (int i = 0; i < 6; i++)
                    {
                        UtilitiesDXXL_List.AddToAVectorList(ref precalcedUnitCirclePoints_forCurrentlyRequestedFlexNumberedFlatPolygon_aroundOrigin_insideYPlane_startingWithFirstVertexAtZForward_clockwiseWhenLookingDownward, _6precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward[i], i);
                    }
                    return true;
                case 7:
                    for (int i = 0; i < 7; i++)
                    {
                        UtilitiesDXXL_List.AddToAVectorList(ref precalcedUnitCirclePoints_forCurrentlyRequestedFlexNumberedFlatPolygon_aroundOrigin_insideYPlane_startingWithFirstVertexAtZForward_clockwiseWhenLookingDownward, _7precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward[i], i);
                    }
                    return true;
                case 8:
                    for (int i = 0; i < 8; i++)
                    {
                        UtilitiesDXXL_List.AddToAVectorList(ref precalcedUnitCirclePoints_forCurrentlyRequestedFlexNumberedFlatPolygon_aroundOrigin_insideYPlane_startingWithFirstVertexAtZForward_clockwiseWhenLookingDownward, _8precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward[i], i);
                    }
                    return true;
                case 10:
                    for (int i = 0; i < 10; i++)
                    {
                        UtilitiesDXXL_List.AddToAVectorList(ref precalcedUnitCirclePoints_forCurrentlyRequestedFlexNumberedFlatPolygon_aroundOrigin_insideYPlane_startingWithFirstVertexAtZForward_clockwiseWhenLookingDownward, _10precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward[i], i);
                    }
                    return true;
                case 32:
                    for (int i = 0; i < 32; i++)
                    {
                        UtilitiesDXXL_List.AddToAVectorList(ref precalcedUnitCirclePoints_forCurrentlyRequestedFlexNumberedFlatPolygon_aroundOrigin_insideYPlane_startingWithFirstVertexAtZForward_clockwiseWhenLookingDownward, _32precalcedUnitCirclePoints_aroundOrigin_insideYPlane_startingAtZForward_clockwiseWhenLookingDownward[i], i);
                    }
                    return true;
                default:
                    return false;
            }
        }

        static bool Shape2DisConvex(DrawShapes.Shape2DType shapeToCheck)
        {
            switch (shapeToCheck)
            {
                case DrawShapes.Shape2DType.star3:
                    return false;

                case DrawShapes.Shape2DType.star4:
                    return false;

                case DrawShapes.Shape2DType.star5:
                    return false;

                case DrawShapes.Shape2DType.star6:
                    return false;

                case DrawShapes.Shape2DType.star8:
                    return false;

                case DrawShapes.Shape2DType.star10:
                    return false;

                case DrawShapes.Shape2DType.star16:
                    return false;

                case DrawShapes.Shape2DType.star32:
                    return false;

                case DrawShapes.Shape2DType.star64:
                    return false;

                default:
                    return true;
            }
        }

        static void ScaleX(ref List<Vector3> verticesToScale, float scaleFactor, int usedSlotsInList)
        {
            for (int i = 0; i < usedSlotsInList; i++)
            {
                Vector3 projectionOntoXPlane = new Vector3(0.0f, verticesToScale[i].y, verticesToScale[i].z);
                Vector3 from_projectionOntoXPlane_to_unscaledPos = verticesToScale[i] - projectionOntoXPlane;
                verticesToScale[i] = projectionOntoXPlane + from_projectionOntoXPlane_to_unscaledPos * scaleFactor;
            }
        }

        static void ScaleY(ref List<Vector3> verticesToScale, float scaleFactor, int usedSlotsInList)
        {
            for (int i = 0; i < usedSlotsInList; i++)
            {
                Vector3 projectionOntoYPlane = new Vector3(verticesToScale[i].x, 0.0f, verticesToScale[i].z);
                Vector3 from_projectionOntoYPlane_to_unscaledPos = verticesToScale[i] - projectionOntoYPlane;
                verticesToScale[i] = projectionOntoYPlane + from_projectionOntoYPlane_to_unscaledPos * scaleFactor;
            }
        }

        static void ScaleZ(ref List<Vector3> verticesToScale, float scaleFactor, int usedSlotsInList)
        {
            for (int i = 0; i < usedSlotsInList; i++)
            {
                Vector3 projectionOntoZPlane = new Vector3(verticesToScale[i].x, verticesToScale[i].y, 0.0f);
                Vector3 from_projectionOntoZPlane_to_unscaledPos = verticesToScale[i] - projectionOntoZPlane;
                verticesToScale[i] = projectionOntoZPlane + from_projectionOntoZPlane_to_unscaledPos * scaleFactor;
            }
        }

        static void RotateVertices(ref List<Vector3> verticesToRotate, Quaternion rotation, int usedSlotsInList)
        {
            for (int i = 0; i < usedSlotsInList; i++)
            {
                verticesToRotate[i] = rotation * verticesToRotate[i];
            }
        }

        public static Vector3 ForceVectorPerpToOtherVector(Vector3 vectorToForcePerp, InternalDXXL_Plane planePerpToOtherVector)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(vectorToForcePerp))
            {
                return UtilitiesDXXL_Math.Get_aNormalizedVector_perpToGivenVector(planePerpToOtherVector.normalDir);
            }
            else
            {
                Vector3 vectorProjectedIntoPerpToOtherVectorPlane = planePerpToOtherVector.Get_projectionOfVectorOntoPlane(vectorToForcePerp);
                vectorProjectedIntoPerpToOtherVectorPlane = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(vectorProjectedIntoPerpToOtherVectorPlane);
                if (UtilitiesDXXL_Math.ApproximatelyZero(vectorProjectedIntoPerpToOtherVectorPlane))
                {
                    return UtilitiesDXXL_Math.Get_aNormalizedVector_perpToGivenVector(planePerpToOtherVector.normalDir);
                }
                else
                {
                    return vectorProjectedIntoPerpToOtherVectorPlane;
                }
            }
        }

        public static void ConvertQuaternionToFlatShapesNormalAndUpInsideFlatPlane(out Vector3 normal, out Vector3 up_insideFlatPlane, Quaternion quaternion)
        {
            if (UtilitiesDXXL_Math.IsDefaultInvalidQuaternion(quaternion))
            {
                //-> will use "DrawShapes.automaticOrientationOfFlatShapes"
                normal = default(Vector3);
                up_insideFlatPlane = default(Vector3);
            }
            else
            {
                normal = quaternion * Vector3.forward;
                up_insideFlatPlane = quaternion * Vector3.up;
            }
        }

        static float shapeFillDensity_before;
        public static void Set_shapeFillDensity_reversible(float new_shapeFillDensity)
        {
            shapeFillDensity_before = DrawShapes.ShapeFillDensity;
            DrawShapes.ShapeFillDensity = new_shapeFillDensity;
        }

        public static void Reverse_shapeFillDensity()
        {
            DrawShapes.ShapeFillDensity = shapeFillDensity_before;
        }

        static int linesPerSphereCircle_before;
        public static void Set_linesPerSphereCircle_reversible(int new_linesPerSphereCircle)
        {
            linesPerSphereCircle_before = DrawShapes.LinesPerSphereCircle;
            DrawShapes.LinesPerSphereCircle = new_linesPerSphereCircle;
        }

        public static void Reverse_linesPerSphereCircle()
        {
            DrawShapes.LinesPerSphereCircle = linesPerSphereCircle_before;
        }

        static float forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_before;
        public static void Set_forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_reversible(float new_forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes)
        {
            forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_before = DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes;
            DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = new_forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes;
        }

        public static void Reverse_forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes()
        {
            DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_before;
        }

        static float forcedConstantWorldspaceTextSize_forTextAtShapes_before;
        public static void Set_forcedConstantWorldspaceTextSize_forTextAtShapes_reversible(float new_forcedConstantWorldspaceTextSize_forTextAtShapes)
        {
            forcedConstantWorldspaceTextSize_forTextAtShapes_before = DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes;
            DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = new_forcedConstantWorldspaceTextSize_forTextAtShapes;
        }

        public static void Reverse_forcedConstantWorldspaceTextSize_forTextAtShapes()
        {
            DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = forcedConstantWorldspaceTextSize_forTextAtShapes_before;
        }

        public static void Set_bothForcedConstantTextSizes_forTextAtShapes_reversible(float forScreenSpace, float forWorldspace)
        {
            Set_forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_reversible(forScreenSpace);
            Set_forcedConstantWorldspaceTextSize_forTextAtShapes_reversible(forWorldspace);
        }

        public static void Disable_bothForcedConstantTextSizes_forTextAtShapes_reversible()
        {
            Set_forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_reversible(0.0f);
            Set_forcedConstantWorldspaceTextSize_forTextAtShapes_reversible(0.0f);
        }

        public static void Reverse_disable_bothForcedConstantTextSizes_forTextAtShapes()
        {
            Reverse_forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes();
            Reverse_forcedConstantWorldspaceTextSize_forTextAtShapes();
        }

        static void Copy_globalVertices_to_localVertices(Vector3 globalCenter, int numberOfSlotsToCopy)
        {
            for (int i = 0; i < numberOfSlotsToCopy; i++)
            {
                UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, verticesGlobal[i] - globalCenter, i);
            }
        }

        static void Copy_customGlobalVertices_to_localVertices(ref List<Vector3> customGlobalVerticesListFromWhereToCopy, Vector3 globalCenter, int numberOfSlotsToCopy)
        {
            for (int i = 0; i < numberOfSlotsToCopy; i++)
            {
                UtilitiesDXXL_List.AddToAVectorList(ref verticesLocal, customGlobalVerticesListFromWhereToCopy[i] - globalCenter, i);
            }
        }

        static void Copy_localVertices_to_globalVertices(Vector3 globalCenter, int numberOfSlotsToCopy)
        {
            for (int i = 0; i < numberOfSlotsToCopy; i++)
            {
                UtilitiesDXXL_List.AddToAVectorList(ref verticesGlobal, globalCenter + verticesLocal[i], i);
            }
        }

    }

}
