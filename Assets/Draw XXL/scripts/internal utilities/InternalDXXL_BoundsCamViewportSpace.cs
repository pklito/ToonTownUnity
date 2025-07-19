namespace DrawXXL
{

    using UnityEngine;


    public class InternalDXXL_BoundsCamViewportSpace
    {
        public static Vector2 viewportCenter = new Vector2(0.5f, 0.5f);
        public static InternalDXXL_BoundsCamViewportSpace wholeViewportAsBounds = new InternalDXXL_BoundsCamViewportSpace(viewportCenter, Vector2.one);

        public Vector2 center;
        public float xMin;
        public float xMax;
        public float yMin;
        public float yMax;

        public InternalDXXL_BoundsCamViewportSpace()
        {
        }

        public InternalDXXL_BoundsCamViewportSpace(Vector2 centerPos, Vector2 size)
        {
            center = centerPos;
            float halfXSize = 0.5f * size.x;
            float halfYSize = 0.5f * size.y;
            xMin = center.x - halfXSize;
            xMax = center.x + halfXSize;
            yMin = center.y - halfYSize;
            yMax = center.y + halfYSize;
        }

        public void Recreate(Vector2 centerPos, Vector2 size)
        {
            center = centerPos;
            float halfXSize = 0.5f * size.x;
            float halfYSize = 0.5f * size.y;
            xMin = center.x - halfXSize;
            xMax = center.x + halfXSize;
            yMin = center.y - halfYSize;
            yMax = center.y + halfYSize;
        }

        public InternalDXXL_BoundsCamViewportSpace GetCopy()
        {
            InternalDXXL_BoundsCamViewportSpace copiedBounds = new InternalDXXL_BoundsCamViewportSpace(center, new Vector2(xMax - xMin, yMax - yMin));
            return copiedBounds;
        }

        public void Encapsulate(Vector2 pos)
        {
            if (pos.x < xMin)
            {
                xMin = pos.x;
                center.x = 0.5f * (xMax + xMin);
            }
            else
            {
                if (pos.x > xMax)
                {
                    xMax = pos.x;
                    center.x = 0.5f * (xMax + xMin);
                }
            }

            if (pos.y < yMin)
            {
                yMin = pos.y;
                center.y = 0.5f * (yMax + yMin);
            }
            else
            {
                if (pos.y > yMax)
                {
                    yMax = pos.y;
                    center.y = 0.5f * (yMax + yMin);
                }
            }
        }

        public void EncapsulateButPreventGrowingIntoViewport(Vector2 pos)
        {
            if (pos.x < xMin)
            {
                if (xMin >= 1.0f)
                {
                    xMin = Mathf.Max(pos.x, Mathf.Min(xMin, 1.01f));
                }
                else
                {
                    xMin = pos.x;
                }
                center.x = 0.5f * (xMax + xMin);
            }
            else
            {
                if (pos.x > xMax)
                {
                    if (xMax <= 0.0f)
                    {
                        xMax = Mathf.Min(pos.x, Mathf.Max(xMax, -0.01f));
                    }
                    else
                    {
                        xMax = pos.x;
                    }
                    center.x = 0.5f * (xMax + xMin);
                }
            }

            if (pos.y < yMin)
            {
                if (yMin >= 1.0f)
                {
                    yMin = Mathf.Max(pos.y, Mathf.Min(yMin, 1.01f));
                }
                else
                {
                    yMin = pos.y;
                }
                center.y = 0.5f * (yMax + yMin);
            }
            else
            {
                if (pos.y > yMax)
                {
                    if (yMax <= 0.0f)
                    {
                        yMax = Mathf.Min(pos.y, Mathf.Max(yMax, -0.01f));
                    }
                    else
                    {
                        yMax = pos.y;
                    }
                    center.y = 0.5f * (yMax + yMin);
                }
            }
        }

        public void Encapsulate(InternalDXXL_BoundsCamViewportSpace boundsToEncapsulate)
        {
            if (boundsToEncapsulate != null)
            {
                if (boundsToEncapsulate.xMin < xMin)
                {
                    xMin = boundsToEncapsulate.xMin;
                    center.x = 0.5f * (xMax + xMin);
                }
                else
                {
                    if (boundsToEncapsulate.xMax > xMax)
                    {
                        xMax = boundsToEncapsulate.xMax;
                        center.x = 0.5f * (xMax + xMin);
                    }
                }

                if (boundsToEncapsulate.yMin < yMin)
                {
                    yMin = boundsToEncapsulate.yMin;
                    center.y = 0.5f * (yMax + yMin);
                }
                else
                {
                    if (boundsToEncapsulate.yMax > yMax)
                    {
                        yMax = boundsToEncapsulate.yMax;
                        center.y = 0.5f * (yMax + yMin);
                    }
                }
            }
        }

        public void EncapsulateButPreventGrowingIntoViewport(InternalDXXL_BoundsCamViewportSpace boundsToEncapsulate)
        {
            if (boundsToEncapsulate != null)
            {
                if (boundsToEncapsulate.xMin < xMin)
                {
                    if (xMin >= 1.0f)
                    {
                        xMin = Mathf.Max(boundsToEncapsulate.xMin, Mathf.Min(xMin, 1.01f));
                    }
                    else
                    {
                        xMin = boundsToEncapsulate.xMin;
                    }
                    center.x = 0.5f * (xMax + xMin);
                }
                else
                {
                    if (boundsToEncapsulate.xMax > xMax)
                    {
                        if (xMax <= 0.0f)
                        {
                            xMax = Mathf.Min(boundsToEncapsulate.xMax, Mathf.Max(xMax, -0.01f));
                        }
                        else
                        {
                            xMax = boundsToEncapsulate.xMax;
                        }
                        center.x = 0.5f * (xMax + xMin);
                    }
                }

                if (boundsToEncapsulate.yMin < yMin)
                {
                    if (yMin >= 1.0f)
                    {
                        yMin = Mathf.Max(boundsToEncapsulate.yMin, Mathf.Min(yMin, 1.01f));
                    }
                    else
                    {
                        yMin = boundsToEncapsulate.yMin;
                    }
                    center.y = 0.5f * (yMax + yMin);
                }
                else
                {
                    if (boundsToEncapsulate.yMax > yMax)
                    {
                        if (yMax <= 0.0f)
                        {
                            yMax = Mathf.Min(boundsToEncapsulate.yMax, Mathf.Max(yMax, -0.01f));
                        }
                        else
                        {
                            yMax = boundsToEncapsulate.yMax;
                        }
                        center.y = 0.5f * (yMax + yMin);
                    }
                }
            }
        }

        public Vector2 GetNearestCorner(Vector2 posToWhichCornerShouldBeNearest)
        {
            if (center.x < posToWhichCornerShouldBeNearest.x)
            {
                if (center.y < posToWhichCornerShouldBeNearest.y)
                {
                    return new Vector2(xMax, yMax);
                }
                else
                {
                    return new Vector2(xMax, yMin);
                }
            }
            else
            {
                if (center.y < posToWhichCornerShouldBeNearest.y)
                {
                    return new Vector2(xMin, yMax);
                }
                else
                {
                    return new Vector2(xMin, yMin);
                }
            }
        }

        public Vector2 GetPosOutsideNearestCorner(Vector2 posToWhichCornerShouldBeNearest)
        {
            Vector2 nearestCorner = GetNearestCorner(posToWhichCornerShouldBeNearest);
            Vector2 centerToNearestCorner = nearestCorner - center;
            return (center + centerToNearestCorner * 1.01f);
        }

        public Vector2 GetLowerLeftCorner()
        {
            return new Vector2(xMin, yMin);
        }

        public Vector2 GetLowerRightCorner()
        {
            return new Vector2(xMax, yMin);
        }

        public Vector2 GetUpperLeftCorner()
        {
            return new Vector2(xMin, yMax);
        }

        public Vector2 GetUpperRightCorner()
        {
            return new Vector2(xMax, yMax);
        }

        public bool IsCompletelyInsideViewport()
        {
            if (xMin >= 0.0f)
            {
                if (xMax <= 1.0f)
                {
                    if (yMin >= 0.0f)
                    {
                        if (yMax <= 1.0f)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool IsCompletelyOutsideViewport()
        {
            if (xMax < 0.0f || xMin > 1.0f || yMax < 0.0f || yMin > 1.0f)
            {
                return true;
            }
            return false;
        }

        public bool HasVertEdgePartInsideViewport()
        {
            if (yMax >= 0.0f && yMin <= 1.0f)
            {
                if (xMin >= 0.0f && xMin <= 1.0f)
                {
                    return true;
                }

                if (xMax >= 0.0f && xMax <= 1.0f)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasHorizEdgePartInsideViewport()
        {
            if (xMax >= 0.0f && xMin <= 1.0f) 
            {
                if (yMin >= 0.0f && yMin <= 1.0f)
                {
                    return true;
                }

                if (yMax >= 0.0f && yMax <= 1.0f)
                {
                    return true;
                }
            }
            return false;
        }

        public bool HasEdgePartInsideViewport()
        {
            if (HasVertEdgePartInsideViewport() || HasHorizEdgePartInsideViewport())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool HasCornerInsideViewport()
        {
            if (HasVertEdgePartInsideViewport() && HasHorizEdgePartInsideViewport())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool CompletelyEncapsulatesViewport()
        {
            if (xMin <= 0.0f)
            {
                if (xMax >= 1.0f)
                {
                    if (yMin <= 0.0f)
                    {
                        if (yMax >= 1.0f)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static void ConstructAndOrEncapsulate(ref InternalDXXL_BoundsCamViewportSpace boundsToConstructOrGrow, Vector2 pos, bool preventGrowingIntoViewport)
        {
            if (boundsToConstructOrGrow == null)
            {
                if (preventGrowingIntoViewport)
                {
                    if (IsInsideViewportInclBorder(pos))
                    {
                        pos = GetViewportCenterPlumbIntersectionWithViewportBorderShifted(pos, 0.01f);
                    }
                }
                boundsToConstructOrGrow = new InternalDXXL_BoundsCamViewportSpace(pos, Vector2.zero);
            }
            else
            {
                if (preventGrowingIntoViewport)
                {
                    boundsToConstructOrGrow.EncapsulateButPreventGrowingIntoViewport(pos);
                }
                else
                {
                    boundsToConstructOrGrow.Encapsulate(pos);
                }
            }
        }

        public static void ConstructAndOrEncapsulate(ref InternalDXXL_BoundsCamViewportSpace boundsToConstructOrGrow, InternalDXXL_BoundsCamViewportSpace boundsToEncapsulate, bool preventGrowingIntoViewport)
        {
            if (boundsToEncapsulate == null)
            {
                if (boundsToConstructOrGrow == null)
                {
                    //Debug.LogError("Both 'boundsToConstructOrGrow' and 'boundsToEncapsulate' are 'null'. 'boundsToConstructOrGrow' is not constructed and remains 'null'.");
                    UtilitiesDXXL_Log.PrintErrorCode("3");
                }
            }
            else
            {
                if (boundsToConstructOrGrow == null)
                {
                    if (boundsToEncapsulate.IsCompletelyOutsideViewport())
                    {
                        boundsToConstructOrGrow = boundsToEncapsulate.GetCopy();
                    }
                    else
                    {
                        Vector2 centerPosShiftedToOutsideViewport = GetViewportCenterPlumbIntersectionWithViewportBorderShifted(boundsToEncapsulate.center, 0.01f);
                        boundsToConstructOrGrow = new InternalDXXL_BoundsCamViewportSpace(centerPosShiftedToOutsideViewport, Vector2.zero);
                    }
                }
                else
                {
                    if (preventGrowingIntoViewport)
                    {
                        boundsToConstructOrGrow.EncapsulateButPreventGrowingIntoViewport(boundsToEncapsulate);
                    }
                    else
                    {
                        boundsToConstructOrGrow.Encapsulate(boundsToEncapsulate);
                    }
                }
            }
        }

        public static Vector2 GetViewportCenterPlumbIntersectionWithViewportBorder(Vector2 posToPlumbTowardsViewportCenter)
        {
            return GetViewportCenterPlumbIntersectionWithViewportBorderShifted(posToPlumbTowardsViewportCenter, 0.0f);
        }

        static InternalDXXL_Line2D plumbLine = new InternalDXXL_Line2D();
        public static Vector2 GetViewportCenterPlumbIntersectionWithViewportBorderShifted(Vector2 posToPlumbTowardsViewportCenter, float shiftDistanceToOutsideOfViewport)
        {
            float onePlusShiftDistanceToOutside = 1.0f + shiftDistanceToOutsideOfViewport;
            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(posToPlumbTowardsViewportCenter, viewportCenter))
            {
                return new Vector2(-shiftDistanceToOutsideOfViewport, 0.25f);
            }
            else
            {
                if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(posToPlumbTowardsViewportCenter.y, 0.5f))
                {
                    if (posToPlumbTowardsViewportCenter.x <= 0.5f)
                    {
                        return new Vector2(-shiftDistanceToOutsideOfViewport, 0.5f);
                    }
                    else
                    {
                        return new Vector2(onePlusShiftDistanceToOutside, 0.5f);
                    }
                }
                else
                {
                    if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(posToPlumbTowardsViewportCenter.x, 0.5f))
                    {
                        if (posToPlumbTowardsViewportCenter.y <= 0.5f)
                        {
                            return new Vector2(0.5f, -shiftDistanceToOutsideOfViewport);
                        }
                        else
                        {
                            return new Vector2(0.5f, onePlusShiftDistanceToOutside);
                        }
                    }
                    else
                    {
                        plumbLine.Recalc_line_throughTwoPoints_returnSteepForVertLines(posToPlumbTowardsViewportCenter, viewportCenter);
                        if (posToPlumbTowardsViewportCenter.x <= 0.5f)
                        {
                            Vector2 intersectionWithLeftViewportBorder = new Vector2(-shiftDistanceToOutsideOfViewport, plumbLine.GetYatX(-shiftDistanceToOutsideOfViewport));
                            if (intersectionWithLeftViewportBorder.y > -shiftDistanceToOutsideOfViewport && intersectionWithLeftViewportBorder.y < onePlusShiftDistanceToOutside)
                            {
                                return intersectionWithLeftViewportBorder;
                            }
                            else
                            {
                                if (posToPlumbTowardsViewportCenter.y > 0.5f)
                                {
                                    return new Vector2(plumbLine.GetXatY(onePlusShiftDistanceToOutside), onePlusShiftDistanceToOutside);
                                }
                                else
                                {
                                    return new Vector2(plumbLine.GetXatY(-shiftDistanceToOutsideOfViewport), -shiftDistanceToOutsideOfViewport);
                                }
                            }
                        }
                        else
                        {
                            Vector2 intersectionWithRightViewportBorder = new Vector2(onePlusShiftDistanceToOutside, plumbLine.GetYatX(onePlusShiftDistanceToOutside));
                            if (intersectionWithRightViewportBorder.y > -shiftDistanceToOutsideOfViewport && intersectionWithRightViewportBorder.y < onePlusShiftDistanceToOutside)
                            {
                                return intersectionWithRightViewportBorder;
                            }
                            else
                            {
                                if (posToPlumbTowardsViewportCenter.y > 0.5f)
                                {
                                    return new Vector2(plumbLine.GetXatY(onePlusShiftDistanceToOutside), onePlusShiftDistanceToOutside);
                                }
                                else
                                {
                                    return new Vector2(plumbLine.GetXatY(-shiftDistanceToOutsideOfViewport), -shiftDistanceToOutsideOfViewport);
                                }
                            }
                        }
                    }
                }
            }
        }

        public static Vector2 ClampIntoViewport(Vector2 posToClamp)
        {
            return new Vector2(Mathf.Clamp01(posToClamp.x), Mathf.Clamp01(posToClamp.y));
        }

        public static bool IsInsideViewportInclBorder(Vector2 pos)
        {
            if (pos.x >= 0.0f)
            {
                if (pos.x <= 1.0f)
                {
                    if (pos.y >= 0.0f)
                    {
                        if (pos.y <= 1.0f)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool IsInsideViewportExclBorder(Vector2 pos)
        {
            if (pos.x > 0.0f)
            {
                if (pos.x < 1.0f)
                {
                    if (pos.y > 0.0f)
                    {
                        if (pos.y < 1.0f)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool IsOutsideViewportInclBorder(Vector2 pos)
        {
            return !IsInsideViewportInclBorder(pos);
        }

        public static bool IsOutsideViewportExclBorder(Vector2 pos)
        {
            return !IsInsideViewportExclBorder(pos);
        }

        public static bool IsOutsideViewportWithPadding(Vector2 posToCheckIfOutside, float paddingHowMuchViewportGetsEnlargedForTheCheck)
        {
            if (posToCheckIfOutside.x < (-paddingHowMuchViewportGetsEnlargedForTheCheck))
            {
                return true;
            }
            if (posToCheckIfOutside.x > (1.0f + paddingHowMuchViewportGetsEnlargedForTheCheck))
            {
                return true;
            }
            if (posToCheckIfOutside.y < (-paddingHowMuchViewportGetsEnlargedForTheCheck))
            {
                return true;
            }
            if (posToCheckIfOutside.y > (1.0f + paddingHowMuchViewportGetsEnlargedForTheCheck))
            {
                return true;
            }
            return false;
        }

        public static bool IsOutsideViewportXWithPadding(Vector2 posToCheckIfOutside, float paddingHowMuchViewportGetsEnlargedForTheCheck)
        {
            if (posToCheckIfOutside.x < (-paddingHowMuchViewportGetsEnlargedForTheCheck))
            {
                return true;
            }
            if (posToCheckIfOutside.x > (1.0f + paddingHowMuchViewportGetsEnlargedForTheCheck))
            {
                return true;
            }
            return false;
        }

        public static bool IsOutsideViewportYWithPadding(Vector2 posToCheckIfOutside, float paddingHowMuchViewportGetsEnlargedForTheCheck)
        {
            if (posToCheckIfOutside.y < (-paddingHowMuchViewportGetsEnlargedForTheCheck))
            {
                return true;
            }
            if (posToCheckIfOutside.y > (1.0f + paddingHowMuchViewportGetsEnlargedForTheCheck))
            {
                return true;
            }
            return false;
        }

        public bool LeftBorderCrossesCompletelyInsideViewport()
        {
            if (xMin > 0.0f && xMin < 1.0f)
            {
                if (yMin < 0.0f && yMax > 1.0f)
                {
                    return true;
                }
            }
            return false;

        }

        public bool RightBorderCrossesCompletelyInsideViewport()
        {
            if (xMax > 0.0f && xMax < 1.0f)
            {
                if (yMin < 0.0f && yMax > 1.0f)
                {
                    return true;
                }
            }
            return false;
        }

        public bool LowerBorderCrossesCompletelyInsideViewport()
        {
            if (yMin > 0.0f && yMin < 1.0f)
            {
                if (xMin < 0.0f && xMax > 1.0f)
                {
                    return true;
                }
            }
            return false;
        }

        public bool UpperBorderCrossesCompletelyInsideViewport()
        {
            if (yMax > 0.0f && yMax < 1.0f)
            {
                if (xMin < 0.0f && xMax > 1.0f)
                {
                    return true;
                }
            }
            return false;
        }

        public Vector2 GetPosOnMostCenteredViewportCrossingEdge(float posOnEdge_as0to1OfViewport)
        {
            Vector2 mostCenteredPos = Vector2.zero;
            float smallestDistanceToCenter = 1.0f;

            // float viewportCenter_1D = 0.5f; //-> makes the textPos flicker in common cases where edges are symetrical around a viewport0.5-axis
            float viewportCenter_1D = 0.505f; //-> prevent textPos-flicker of common case where edges are symetrical around a viewport0.5-axis

            if (LeftBorderCrossesCompletelyInsideViewport())
            {
                float distanceToCenter = Mathf.Abs(xMin - viewportCenter_1D);
                if (distanceToCenter < smallestDistanceToCenter)
                {
                    smallestDistanceToCenter = distanceToCenter;
                    mostCenteredPos = new Vector2(xMin, posOnEdge_as0to1OfViewport);
                }
            }

            if (RightBorderCrossesCompletelyInsideViewport())
            {
                float distanceToCenter = Mathf.Abs(xMax - viewportCenter_1D);

                if (distanceToCenter < smallestDistanceToCenter)
                {
                    smallestDistanceToCenter = distanceToCenter;
                    mostCenteredPos = new Vector2(xMax, posOnEdge_as0to1OfViewport);
                }
            }

            if (LowerBorderCrossesCompletelyInsideViewport())
            {
                float distanceToCenter = Mathf.Abs(yMin - viewportCenter_1D);
                if (distanceToCenter < smallestDistanceToCenter)
                {
                    smallestDistanceToCenter = distanceToCenter;
                    mostCenteredPos = new Vector2(posOnEdge_as0to1OfViewport, yMin);
                }
            }

            if (UpperBorderCrossesCompletelyInsideViewport())
            {
                float distanceToCenter = Mathf.Abs(yMax - viewportCenter_1D);
                if (distanceToCenter < smallestDistanceToCenter)
                {
                    smallestDistanceToCenter = distanceToCenter;
                    mostCenteredPos = new Vector2(posOnEdge_as0to1OfViewport, yMax);
                }
            }

            return mostCenteredPos;
        }

        public void DrawViewportCrossingEdges(Camera camera, Color color, float lineWidth_relToViewportHeight, float durationInSec)
        {
            bool hasAlreadyDrawnHorizDottedLines = false;
            bool hasAlreadyDrawnVertDottedLines = false;

            if (LeftBorderCrossesCompletelyInsideViewport())
            {
                Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(xMin, 0.0f), new Vector2(xMin, 1.0f), color, lineWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
                if (hasAlreadyDrawnHorizDottedLines == false)
                {
                    float rightEndOfDashedLine = Mathf.Min(0.995f, xMax);
                    Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(xMin, 0.005f), new Vector2(rightEndOfDashedLine, 0.005f), color, 0.0f, null, DrawBasics.LineStyle.dashed, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
                    Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(xMin, 0.995f), new Vector2(rightEndOfDashedLine, 0.995f), color, 0.0f, null, DrawBasics.LineStyle.dashed, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
                    hasAlreadyDrawnHorizDottedLines = true;
                }
            }

            if (RightBorderCrossesCompletelyInsideViewport())
            {
                Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(xMax, 0.0f), new Vector2(xMax, 1.0f), color, lineWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
                if (hasAlreadyDrawnHorizDottedLines == false)
                {
                    float leftEndOfDashedLine = Mathf.Max(0.005f, xMin);
                    Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(xMax, 0.005f), new Vector2(leftEndOfDashedLine, 0.005f), color, 0.0f, null, DrawBasics.LineStyle.dashed, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
                    Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(xMax, 0.995f), new Vector2(leftEndOfDashedLine, 0.995f), color, 0.0f, null, DrawBasics.LineStyle.dashed, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
                    hasAlreadyDrawnHorizDottedLines = true;
                }
            }

            if (LowerBorderCrossesCompletelyInsideViewport())
            {
                Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(0.0f, yMin), new Vector2(1.0f, yMin), color, lineWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
                if (hasAlreadyDrawnVertDottedLines == false)
                {
                    float upperEndOfDashedLine = Mathf.Min(0.995f, yMax);
                    Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(0.005f, yMin), new Vector2(0.005f, upperEndOfDashedLine), color, 0.0f, null, DrawBasics.LineStyle.dashed, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
                    Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(0.995f, yMin), new Vector2(0.995f, upperEndOfDashedLine), color, 0.0f, null, DrawBasics.LineStyle.dashed, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
                    hasAlreadyDrawnVertDottedLines = true;
                }
            }

            if (UpperBorderCrossesCompletelyInsideViewport())
            {
                Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(0.0f, yMax), new Vector2(1.0f, yMax), color, lineWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
                if (hasAlreadyDrawnVertDottedLines == false)
                {
                    float lowerEndOfDashedLine = Mathf.Max(0.005f, yMin);
                    Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(0.005f, yMax), new Vector2(0.005f, lowerEndOfDashedLine), color, 0.0f, null, DrawBasics.LineStyle.dashed, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
                    Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(0.995f, yMax), new Vector2(0.995f, lowerEndOfDashedLine), color, 0.0f, null, DrawBasics.LineStyle.dashed, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
                    hasAlreadyDrawnVertDottedLines = true;
                }
            }
        }

        public void Draw(Camera camera, Color color, float lineWidth_relToViewportHeight, float durationInSec)
        {
            Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(xMin, yMin), new Vector2(xMin, yMax), color, lineWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
            Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(xMax, yMin), new Vector2(xMax, yMax), color, lineWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
            Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(xMin, yMin), new Vector2(xMax, yMin), color, lineWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
            Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(xMin, yMax), new Vector2(xMax, yMax), color, lineWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
        }

        public static void DrawViewportBorder(Camera camera, Color color, float lineWidth_relToViewportHeight, float offsetTowardsInsideOfViewport, float durationInSec)
        {
            float oneMinusOffset = 1.0f - offsetTowardsInsideOfViewport;
            lineWidth_relToViewportHeight = UtilitiesDXXL_Math.AbsNonZeroValue(lineWidth_relToViewportHeight);
            float halfLineWidth_relToViewportHeight = 0.5f * lineWidth_relToViewportHeight;
            float halfLineWidth_relToViewportWidth = halfLineWidth_relToViewportHeight / camera.aspect;

            //horiz Lines:
            Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(offsetTowardsInsideOfViewport - halfLineWidth_relToViewportWidth, offsetTowardsInsideOfViewport), new Vector2(oneMinusOffset + halfLineWidth_relToViewportWidth, offsetTowardsInsideOfViewport), color, lineWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
            Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(offsetTowardsInsideOfViewport - halfLineWidth_relToViewportWidth, oneMinusOffset), new Vector2(oneMinusOffset + halfLineWidth_relToViewportWidth, oneMinusOffset), color, lineWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
            //vert Lines:
            Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(offsetTowardsInsideOfViewport, offsetTowardsInsideOfViewport - halfLineWidth_relToViewportHeight), new Vector2(offsetTowardsInsideOfViewport, oneMinusOffset + halfLineWidth_relToViewportHeight), color, lineWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
            Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(oneMinusOffset, offsetTowardsInsideOfViewport - halfLineWidth_relToViewportHeight), new Vector2(oneMinusOffset, oneMinusOffset + halfLineWidth_relToViewportHeight), color, lineWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec);
        }

        public void PrintSpecsToLog()
        {
            Debug.Log("InternalDXXL_BoundsCamViewportSpace specs -> center: " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(center) + " xMin: " + xMin + " xMax: " + xMax + " yMin: " + yMin + " yMax: " + yMax);
        }

    }

}
