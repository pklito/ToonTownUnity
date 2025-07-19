namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class UtilitiesDXXL_Bezier
    {
        public static void BezierSegmentQuadratic(bool is2D, Vector3 startPosition, Vector3 endPosition, Vector3 controlPosInBetween, Color color, string text, float width, int straightSubDivisions, float textSize, bool drawIngameWarningTextForZeroExtent, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (drawIngameWarningTextForZeroExtent)
            {
                if (UtilitiesDXXL_Math.CheckIf_vectorsAreApproximatelyEqual(startPosition, endPosition, controlPosInBetween))
                {
                    if (is2D)
                    {
                        UtilitiesDXXL_DrawBasics2D.PointFallback(startPosition, "[<color=#adadadFF><icon=logMessage></color> BezierQuadratic2D with extent of 0]<br>" + text, color, width, durationInSec, hiddenByNearerObjects);
                    }
                    else
                    {
                        UtilitiesDXXL_DrawBasics.PointFallback(startPosition, "[<color=#adadadFF><icon=logMessage></color> BezierQuadratic with extent of 0]<br>" + text, color, width, durationInSec, hiddenByNearerObjects);
                    }
                    return;
                }
            }

            straightSubDivisions = Mathf.Clamp(straightSubDivisions, 4, 1000);
            InternalDXXL_Plane preferredAmplitudeDir = is2D ? UtilitiesDXXL_DrawBasics2D.xyPlane_throughZero : null;
            Vector3 prevPointOnBezierCurve = startPosition;
            float progress0to1_perStraightSubDivision = 1.0f / (float)straightSubDivisions;
            for (int i = 1; i < straightSubDivisions; i++)
            {
                float progress_0to1 = progress0to1_perStraightSubDivision * i;
                float oneMinusProgress0to1 = 1.0f - progress_0to1;
                float factor1 = oneMinusProgress0to1 * oneMinusProgress0to1;
                float factor2 = 2.0f * oneMinusProgress0to1 * progress_0to1;
                float factor3 = progress_0to1 * progress_0to1;
                Vector3 currPointOnBezierCurve = factor1 * startPosition + factor2 * controlPosInBetween + factor3 * endPosition;
                UtilitiesDXXL_DrawBasics.Line(prevPointOnBezierCurve, currPointOnBezierCurve, color, width, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, preferredAmplitudeDir, is2D, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                prevPointOnBezierCurve = currPointOnBezierCurve;
            }
            UtilitiesDXXL_DrawBasics.Line(prevPointOnBezierCurve, endPosition, color, width, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, preferredAmplitudeDir, is2D, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);

            if (text != null && text != "")
            {
                if (is2D)
                {
                    UtilitiesDXXL_Text.Write2DFramed(text, startPosition, color, textSize, default(Vector2), DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, startPosition.z, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
                }
                else
                {
                    UtilitiesDXXL_Text.WriteFramed(text, startPosition, color, textSize, default(Vector3), default(Vector3), DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
                }
            }
        }

        public static void BezierSegmentCubic(bool is2D, Vector3 startPosition, Vector3 endPosition, Vector3 controlPosOfStartDirection, Vector3 controlPosOfEndDirection, Color color, string text, float width, int straightSubDivisions, float textSize, bool drawIngameWarningTextForZeroExtent, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (drawIngameWarningTextForZeroExtent)
            {
                if (UtilitiesDXXL_Math.CheckIf_vectorsAreApproximatelyEqual(startPosition, endPosition, controlPosOfStartDirection, controlPosOfEndDirection))
                {
                    if (is2D)
                    {
                        UtilitiesDXXL_DrawBasics2D.PointFallback(startPosition, "[<color=#adadadFF><icon=logMessage></color> BezierCubic2D with extent of 0]<br>" + text, color, width, durationInSec, hiddenByNearerObjects);
                    }
                    else
                    {
                        UtilitiesDXXL_DrawBasics.PointFallback(startPosition, "[<color=#adadadFF><icon=logMessage></color> BezierCubic with extent of 0]<br>" + text, color, width, durationInSec, hiddenByNearerObjects);
                    }
                    return;
                }
            }

            straightSubDivisions = Mathf.Clamp(straightSubDivisions, 4, 1000);
            InternalDXXL_Plane preferredAmplitudeDir = is2D ? UtilitiesDXXL_DrawBasics2D.xyPlane_throughZero : null;
            Vector3 prevPointOnBezierCurve = startPosition;
            float progress0to1_perStraightSubDivision = 1.0f / (float)straightSubDivisions;
            for (int i = 1; i < straightSubDivisions; i++)
            {
                float progress_0to1 = progress0to1_perStraightSubDivision * i;
                float progress_0to1_sqr = progress_0to1 * progress_0to1;
                float oneMinusProgress0to1 = 1.0f - progress_0to1;
                float oneMinusProgress0to1_sqr = oneMinusProgress0to1 * oneMinusProgress0to1;
                float factor1 = oneMinusProgress0to1 * oneMinusProgress0to1_sqr;
                float factor2 = 3.0f * oneMinusProgress0to1_sqr * progress_0to1;
                float factor3 = 3.0f * oneMinusProgress0to1 * progress_0to1_sqr;
                float factor4 = progress_0to1 * progress_0to1_sqr;
                Vector3 currPointOnBezierCurve = factor1 * startPosition + factor2 * controlPosOfStartDirection + factor3 * controlPosOfEndDirection + factor4 * endPosition;
                UtilitiesDXXL_DrawBasics.Line(prevPointOnBezierCurve, currPointOnBezierCurve, color, width, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, preferredAmplitudeDir, is2D, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                prevPointOnBezierCurve = currPointOnBezierCurve;
            }
            UtilitiesDXXL_DrawBasics.Line(prevPointOnBezierCurve, endPosition, color, width, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, preferredAmplitudeDir, is2D, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);

            if (text != null && text != "")
            {
                if (is2D)
                {
                    UtilitiesDXXL_Text.Write2DFramed(text, startPosition, color, textSize, default(Vector2), DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, startPosition.z, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
                }
                else
                {
                    UtilitiesDXXL_Text.WriteFramed(text, startPosition, color, textSize, default(Vector3), default(Vector3), DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
                }
            }
        }

        public delegate Vector3 FlexibleGetPosAtIndex<T>(T collection, int i_whereToObtain, out bool itemIsNull);
        public delegate Vector3 FlexibleGetDirectionControlPosOfTransform<T>(T collection, int i_transformWhereToObtain);
        public static Vector3 GetPositionsFromGameObjectsArray(GameObject[] points, int i_whereToObtain, out bool itemIsNull)
        {
            if (points[i_whereToObtain] == null)
            {
                itemIsNull = true;
                return Vector3.zero;
            }
            else
            {
                itemIsNull = false;
                return points[i_whereToObtain].transform.position;
            }
        }
        public static Vector3 GetPositionsFromGameObjectsList(List<GameObject> points, int i_whereToObtain, out bool itemIsNull)
        {
            if (points[i_whereToObtain] == null)
            {
                itemIsNull = true;
                return Vector3.zero;
            }
            else
            {
                itemIsNull = false;
                return points[i_whereToObtain].transform.position;
            }
        }

        public static Vector3 GetPositionsFromTransformsArray(Transform[] points, int i_whereToObtain, out bool itemIsNull)
        {
            if (points[i_whereToObtain] == null)
            {
                itemIsNull = true;
                return Vector3.zero;
            }
            else
            {
                itemIsNull = false;
                return points[i_whereToObtain].position;
            }
        }
        public static Vector3 GetPositionsFromTransformsList(List<Transform> points, int i_whereToObtain, out bool itemIsNull)
        {
            if (points[i_whereToObtain] == null)
            {
                itemIsNull = true;
                return Vector3.zero;
            }
            else
            {
                itemIsNull = false;
                return points[i_whereToObtain].position;
            }
        }

        public static Vector3 GetPositionsFromVector3Array(Vector3[] points, int i_whereToObtain, out bool itemIsNull)
        {
            Vector3 vectorAtRequestedPos = points[i_whereToObtain];
            if (UtilitiesDXXL_Math.FloatIsInvalid(vectorAtRequestedPos.x) || UtilitiesDXXL_Math.FloatIsInvalid(vectorAtRequestedPos.y) || UtilitiesDXXL_Math.FloatIsInvalid(vectorAtRequestedPos.z))
            {
                itemIsNull = true;
                return Vector3.zero;
            }
            else
            {
                itemIsNull = false;
                return points[i_whereToObtain];
            }
        }
        public static Vector3 GetPositionsFromVector3List(List<Vector3> points, int i_whereToObtain, out bool itemIsNull)
        {
            Vector3 vectorAtRequestedPos = points[i_whereToObtain];
            if (UtilitiesDXXL_Math.FloatIsInvalid(vectorAtRequestedPos.x) || UtilitiesDXXL_Math.FloatIsInvalid(vectorAtRequestedPos.y) || UtilitiesDXXL_Math.FloatIsInvalid(vectorAtRequestedPos.z))
            {
                itemIsNull = true;
                return Vector3.zero;
            }
            else
            {
                itemIsNull = false;
                return points[i_whereToObtain];
            }
        }

        public static Vector3 GetForwardControlPosFromGameObjectsArray(GameObject[] points, int i_whereToObtain)
        {
            return (points[i_whereToObtain].transform.position + points[i_whereToObtain].transform.forward * points[i_whereToObtain].transform.localScale.z);
        }
        public static Vector3 GetForwardControlPosFromGameObjectsList(List<GameObject> points, int i_whereToObtain)
        {
            return (points[i_whereToObtain].transform.position + points[i_whereToObtain].transform.forward * points[i_whereToObtain].transform.localScale.z);
        }

        public static Vector3 GetForwardControlPosFromTransformsArray(Transform[] points, int i_whereToObtain)
        {
            return (points[i_whereToObtain].position + points[i_whereToObtain].forward * points[i_whereToObtain].localScale.z);
        }
        public static Vector3 GetForwardControlPosFromTransformsList(List<Transform> points, int i_whereToObtain)
        {
            return (points[i_whereToObtain].position + points[i_whereToObtain].forward * points[i_whereToObtain].localScale.z);
        }


        public static Vector3 GetBackwardControlPosFromGameObjectsArray(GameObject[] points, int i_whereToObtain)
        {
            return (points[i_whereToObtain].transform.position - points[i_whereToObtain].transform.forward * points[i_whereToObtain].transform.localScale.y);
        }
        public static Vector3 GetBackwardControlPosFromGameObjectsList(List<GameObject> points, int i_whereToObtain)
        {
            return (points[i_whereToObtain].transform.position - points[i_whereToObtain].transform.forward * points[i_whereToObtain].transform.localScale.y);
        }

        public static Vector3 GetBackwardControlPosFromTransformsArray(Transform[] points, int i_whereToObtain)
        {
            return (points[i_whereToObtain].position - points[i_whereToObtain].forward * points[i_whereToObtain].localScale.y);
        }
        public static Vector3 GetBackwardControlPosFromTransformsList(List<Transform> points, int i_whereToObtain)
        {
            return (points[i_whereToObtain].position - points[i_whereToObtain].forward * points[i_whereToObtain].localScale.y);
        }

        public static Vector3 GetPositions3DFromGameObjectsArray_2D(GameObject[] points, int i_whereToObtain, out bool itemIsNull)
        {
            if (points[i_whereToObtain] == null)
            {
                itemIsNull = true;
                return new Vector3(0.0f, 0.0f, currentZPos);
            }
            else
            {
                itemIsNull = false;
                return new Vector3(points[i_whereToObtain].transform.position.x, points[i_whereToObtain].transform.position.y, currentZPos);
            }
        }
        public static Vector3 GetPositions3DFromGameObjectsList_2D(List<GameObject> points, int i_whereToObtain, out bool itemIsNull)
        {
            if (points[i_whereToObtain] == null)
            {
                itemIsNull = true;
                return new Vector3(0.0f, 0.0f, currentZPos);
            }
            else
            {
                itemIsNull = false;
                return new Vector3(points[i_whereToObtain].transform.position.x, points[i_whereToObtain].transform.position.y, currentZPos);
            }
        }

        public static Vector3 GetPositions3DFromTransformsArray_2D(Transform[] points, int i_whereToObtain, out bool itemIsNull)
        {
            if (points[i_whereToObtain] == null)
            {
                itemIsNull = true;
                return new Vector3(0.0f, 0.0f, currentZPos);
            }
            else
            {
                itemIsNull = false;
                return new Vector3(points[i_whereToObtain].position.x, points[i_whereToObtain].position.y, currentZPos);
            }
        }
        public static Vector3 GetPositions3DFromTransformsList_2D(List<Transform> points, int i_whereToObtain, out bool itemIsNull)
        {
            if (points[i_whereToObtain] == null)
            {
                itemIsNull = true;
                return new Vector3(0.0f, 0.0f, currentZPos);
            }
            else
            {
                itemIsNull = false;
                return new Vector3(points[i_whereToObtain].position.x, points[i_whereToObtain].position.y, currentZPos);
            }
        }

        public static Vector3 GetPositions3DFromVector2Array_2D(Vector2[] points, int i_whereToObtain, out bool itemIsNull)
        {
            Vector2 vectorAtRequestedPos = points[i_whereToObtain];
            if (UtilitiesDXXL_Math.FloatIsInvalid(vectorAtRequestedPos.x) || UtilitiesDXXL_Math.FloatIsInvalid(vectorAtRequestedPos.y))
            {
                itemIsNull = true;
                return Vector2.zero;
            }
            else
            {
                itemIsNull = false;
                return new Vector3(points[i_whereToObtain].x, points[i_whereToObtain].y, currentZPos);
            }
        }
        public static Vector3 GetPositions3DFromVector2List_2D(List<Vector2> points, int i_whereToObtain, out bool itemIsNull)
        {
            Vector2 vectorAtRequestedPos = points[i_whereToObtain];
            if (UtilitiesDXXL_Math.FloatIsInvalid(vectorAtRequestedPos.x) || UtilitiesDXXL_Math.FloatIsInvalid(vectorAtRequestedPos.y))
            {
                itemIsNull = true;
                return Vector2.zero;
            }
            else
            {
                itemIsNull = false;
                return new Vector3(points[i_whereToObtain].x, points[i_whereToObtain].y, currentZPos);
            }
        }

        public static Vector3 GetForwardControlPos3DFromGameObjectsArray_2D(GameObject[] points, int i_whereToObtain)
        {
            Vector3 pos3D_notYetForcedToDrawZPos = (points[i_whereToObtain].transform.position + points[i_whereToObtain].transform.right * points[i_whereToObtain].transform.localScale.x);
            return new Vector3(pos3D_notYetForcedToDrawZPos.x, pos3D_notYetForcedToDrawZPos.y, currentZPos);

        }
        public static Vector3 GetForwardControlPos3DFromGameObjectsList_2D(List<GameObject> points, int i_whereToObtain)
        {
            Vector3 pos3D_notYetForcedToDrawZPos = (points[i_whereToObtain].transform.position + points[i_whereToObtain].transform.right * points[i_whereToObtain].transform.localScale.x);
            return new Vector3(pos3D_notYetForcedToDrawZPos.x, pos3D_notYetForcedToDrawZPos.y, currentZPos);
        }

        public static Vector3 GetForwardControlPos3DFromTransformsArray_2D(Transform[] points, int i_whereToObtain)
        {
            Vector3 pos3D_notYetForcedToDrawZPos = (points[i_whereToObtain].position + points[i_whereToObtain].right * points[i_whereToObtain].localScale.x);
            return new Vector3(pos3D_notYetForcedToDrawZPos.x, pos3D_notYetForcedToDrawZPos.y, currentZPos);
        }
        public static Vector3 GetForwardControlPos3DFromTransformsList_2D(List<Transform> points, int i_whereToObtain)
        {
            Vector3 pos3D_notYetForcedToDrawZPos = (points[i_whereToObtain].position + points[i_whereToObtain].right * points[i_whereToObtain].localScale.x);
            return new Vector3(pos3D_notYetForcedToDrawZPos.x, pos3D_notYetForcedToDrawZPos.y, currentZPos);
        }

        public static Vector3 GetBackwardControlPos3DFromGameObjectsArray_2D(GameObject[] points, int i_whereToObtain)
        {
            Vector3 pos3D_notYetForcedToDrawZPos = (points[i_whereToObtain].transform.position - points[i_whereToObtain].transform.right * points[i_whereToObtain].transform.localScale.y);
            return new Vector3(pos3D_notYetForcedToDrawZPos.x, pos3D_notYetForcedToDrawZPos.y, currentZPos);
        }
        public static Vector3 GetBackwardControlPos3DFromGameObjectsList_2D(List<GameObject> points, int i_whereToObtain)
        {
            Vector3 pos3D_notYetForcedToDrawZPos = (points[i_whereToObtain].transform.position - points[i_whereToObtain].transform.right * points[i_whereToObtain].transform.localScale.y);
            return new Vector3(pos3D_notYetForcedToDrawZPos.x, pos3D_notYetForcedToDrawZPos.y, currentZPos);
        }

        public static Vector3 GetBackwardControlPos3DFromTransformsArray_2D(Transform[] points, int i_whereToObtain)
        {
            Vector3 pos3D_notYetForcedToDrawZPos = (points[i_whereToObtain].position - points[i_whereToObtain].right * points[i_whereToObtain].localScale.y);
            return new Vector3(pos3D_notYetForcedToDrawZPos.x, pos3D_notYetForcedToDrawZPos.y, currentZPos);
        }
        public static Vector3 GetBackwardControlPos3DFromTransformsList_2D(List<Transform> points, int i_whereToObtain)
        {
            Vector3 pos3D_notYetForcedToDrawZPos = (points[i_whereToObtain].position - points[i_whereToObtain].right * points[i_whereToObtain].localScale.y);
            return new Vector3(pos3D_notYetForcedToDrawZPos.x, pos3D_notYetForcedToDrawZPos.y, currentZPos);
        }

        static float currentZPos;
        public static void BezierSpline<BezierPointCollection>(bool is2D, float customZPos_for2D, BezierPointCollection bezierPointCollection, FlexibleGetPosAtIndex<BezierPointCollection> GetPos, FlexibleGetDirectionControlPosOfTransform<BezierPointCollection> GetForwardControlPos, FlexibleGetDirectionControlPosOfTransform<BezierPointCollection> GetBackwardControlPos, int lengthOfCollection, Color color, DrawBasics.BezierPosInterpretation interpretationOfPointsCollection, string text, float width, bool closeGapFromEndToStart, int straightSubDivisionsPerSegment, float textSize, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Vector3 textPosition;
            bool textPosition_hasBeenAssigned;
            currentZPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(customZPos_for2D);
            switch (interpretationOfPointsCollection)
            {
                case DrawBasics.BezierPosInterpretation.start_control1_control2_endIsNextStart:
                    textPosition = DrawSpline_case_start_control1_control2_endIsNextStart(is2D, out textPosition_hasBeenAssigned, ref text, bezierPointCollection, GetPos, lengthOfCollection, color, interpretationOfPointsCollection, width, closeGapFromEndToStart, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
                    break;

                case DrawBasics.BezierPosInterpretation.start_control1_endIsNextStart:
                    textPosition = DrawSpline_case_start_control1_endIsNextStart(is2D, out textPosition_hasBeenAssigned, ref text, bezierPointCollection, GetPos, lengthOfCollection, color, interpretationOfPointsCollection, width, closeGapFromEndToStart, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
                    break;

                case DrawBasics.BezierPosInterpretation.onlySegmentStartPoints_backwardForwardIsAligned:
                    textPosition = DrawSpline_case_onlySegmentStartPoints_backwardForwardIsAligned(is2D, out textPosition_hasBeenAssigned, bezierPointCollection, GetPos, GetForwardControlPos, GetBackwardControlPos, lengthOfCollection, color, interpretationOfPointsCollection, width, closeGapFromEndToStart, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
                    break;

                case DrawBasics.BezierPosInterpretation.onlySegmentStartPoints_backwardForwardIsMirrored:
                    textPosition = DrawSpline_case_onlySegmentStartPoints_backwardForwardIsMirrored(is2D, out textPosition_hasBeenAssigned, bezierPointCollection, GetPos, GetForwardControlPos, lengthOfCollection, color, interpretationOfPointsCollection, width, closeGapFromEndToStart, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
                    break;

                case DrawBasics.BezierPosInterpretation.onlySegmentStartPoints_backwardForwardIsKinked:
                    textPosition = DrawSpline_case_onlySegmentStartPoints_backwardForwardIsKinked(is2D, out textPosition_hasBeenAssigned, bezierPointCollection, GetPos, GetForwardControlPos, lengthOfCollection, color, interpretationOfPointsCollection, width, closeGapFromEndToStart, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
                    break;

                default:
                    textPosition = Vector3.zero;
                    textPosition_hasBeenAssigned = false;
                    Debug.LogError("BezierPosInterpretation of '" + interpretationOfPointsCollection + "' not implemented -> DrawBezierSpline not executed.");
                    break;
            }

            if (text != null && text != "")
            {
                if (textPosition_hasBeenAssigned)
                {
                    UtilitiesDXXL_Text.WriteFramed(text, textPosition, color, textSize, default(Vector3), default(Vector3), DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
                }
                else
                {
                    Debug.Log("No ingame position for the text of the BezierSpline could be appointed -> Fallback to log console. The text is: " + text);
                }
            }
        }

        static Vector3 DrawSpline_case_start_control1_control2_endIsNextStart<BezierPointCollection>(bool is2D, out bool textPosition_hasAlreadyBeenAssigned, ref string text, BezierPointCollection bezierPointCollection, FlexibleGetPosAtIndex<BezierPointCollection> GetPos, int lengthOfCollection, Color color, DrawBasics.BezierPosInterpretation interpretationOfPointsCollection, float width, bool closeGapFromEndToStart, int straightSubDivisionsPerSegment, float textSize, float durationInSec, bool hiddenByNearerObjects)
        {
            textPosition_hasAlreadyBeenAssigned = false;
            Vector3 textPosition = Vector3.zero;

            if (lengthOfCollection < 3)
            {
                Debug.LogError("Bezier spline with BezierPosInterpretation of '" + interpretationOfPointsCollection + "' needs at least 3 control points, but the specified collection has only " + lengthOfCollection + " -> drawing skipped.");
            }
            else
            {
                bool splineHasAlreadyCommunicatedANullItem = false; //preventing log spam and many drawn lines (for multiple fallback texts)
                int i_startOfCurrSegment = 0;
                int i_endOfCurrSegment = i_startOfCurrSegment + 3;
                int maxNumberOfWhileLoops = 10000; //prevent freeze/endless loops
                int i_whileLoop = 0;
                while (i_endOfCurrSegment < lengthOfCollection)
                {
                    TryDrawCubicSegment_ofSplineCase_start_control1_control2_endIsNextStart<BezierPointCollection>(is2D, i_startOfCurrSegment, i_endOfCurrSegment, bezierPointCollection, GetPos, i_whileLoop, ref textPosition_hasAlreadyBeenAssigned, ref textPosition, ref splineHasAlreadyCommunicatedANullItem, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
                    i_startOfCurrSegment = i_endOfCurrSegment;
                    i_endOfCurrSegment = i_startOfCurrSegment + 3;

                    i_whileLoop++;
                    if (i_whileLoop > maxNumberOfWhileLoops)
                    {
                        Debug.LogError("Too many Bezier Segments (more than " + maxNumberOfWhileLoops + "). Drawing aborted.");
                        break;
                    }
                }

                if ((i_startOfCurrSegment + 2) < lengthOfCollection)
                {
                    //two splineDefiningCollectionPoints more than '3 points per segment (+1 final)' scheme:
                    //->one to little for another segment
                    if (closeGapFromEndToStart)
                    {
                        TryDrawCubicSegment_ofSplineCase_start_control1_control2_endIsNextStart<BezierPointCollection>(is2D, i_startOfCurrSegment, 0, bezierPointCollection, GetPos, i_whileLoop, ref textPosition_hasAlreadyBeenAssigned, ref textPosition, ref splineHasAlreadyCommunicatedANullItem, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
                    }
                    else
                    {
                        //Fallback to quadratic bezier at spline end:
                        TryDrawQuadraticSegment_ofSplineCase_start_control1_end<BezierPointCollection>(is2D, i_startOfCurrSegment, i_startOfCurrSegment + 2, bezierPointCollection, GetPos, i_whileLoop, ref textPosition_hasAlreadyBeenAssigned, ref textPosition, ref splineHasAlreadyCommunicatedANullItem, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
                        if (lengthOfCollection > 3)
                        {
                            text = "[<color=#e2aa00FF><icon=warning></color> points collection length is 2 too long for the '3 points per segment (+1 final)' scheme<br>-> last segment falls back from cubic to quadratic bezier]<br>" + text;
                        }
                    }

                    if (lengthOfCollection == 3)
                    {
                        text = "[<color=#e2aa00FF><icon=warning></color> points collection length is 3 but the first segment needs 4<br>-> the single segment falls back from cubic to quadratic bezier]<br>" + text;
                    }
                }
                else
                {
                    if ((i_startOfCurrSegment + 1) < lengthOfCollection)
                    {
                        //one splineDefiningCollectionPoint more than '3 points per segment (+1 final)' scheme:
                        //->two to little for another segment
                        if (closeGapFromEndToStart)
                        {
                            TryDrawQuadraticSegment_ofSplineCase_start_control1_end<BezierPointCollection>(is2D, i_startOfCurrSegment, 0, bezierPointCollection, GetPos, i_whileLoop, ref textPosition_hasAlreadyBeenAssigned, ref textPosition, ref splineHasAlreadyCommunicatedANullItem, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
                        }
                        else
                        {
                            text = "[<color=#e2aa00FF><icon=warning></color> points collection length is 1 too long for the '3 points per segment (+1 final)' scheme<br>-> last point discarded]<br>" + text;
                        }
                    }
                    else
                    {
                        if (i_startOfCurrSegment < lengthOfCollection)
                        {
                            //bezierPointCollection.length perfectly fits the '3 points per segment (+1 final)' scheme
                            //-> no surplus points available
                            if (closeGapFromEndToStart)
                            {
                                TryDrawCubicSegment_asGapCloserBetweenSplineEndAndStart<BezierPointCollection>(is2D, i_startOfCurrSegment, bezierPointCollection, GetPos, i_whileLoop, ref textPosition_hasAlreadyBeenAssigned, ref textPosition, ref splineHasAlreadyCommunicatedANullItem, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
                            }
                        }
                    }
                }
            }
            return textPosition;
        }

        static void TryDrawCubicSegment_ofSplineCase_start_control1_control2_endIsNextStart<BezierPointCollection>(bool is2D, int i_ofSegmentStartPos_insideBezierPointsCollection, int i_ofSegmentEndPos_insideBezierPointsCollection, BezierPointCollection bezierPointCollection, FlexibleGetPosAtIndex<BezierPointCollection> GetPos, int i_whileLoop, ref bool textPosition_hasAlreadyBeenAssigned, ref Vector3 textPosition, ref bool splineHasAlreadyCommunicatedANullItem, Color color, float width, int straightSubDivisionsPerSegment, float textSize, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 fallbackPositionOfSegment = Vector3.zero;
            bool fallbackPositionOfSegment_hasAlreadyBeenAssigned = false;
            bool oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid = false;

            Vector3 startPosition = TryGetPosFromBezierPointCollection(out bool startPosItemIsNull, bezierPointCollection, GetPos, i_ofSegmentStartPos_insideBezierPointsCollection, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
            TryAssignFallbackAndTextPositions(startPosItemIsNull, startPosition, ref fallbackPositionOfSegment, ref fallbackPositionOfSegment_hasAlreadyBeenAssigned, ref textPosition, ref textPosition_hasAlreadyBeenAssigned);

            Vector3 controlPosOfStartDirection = TryGetPosFromBezierPointCollection(bezierPointCollection, GetPos, i_ofSegmentStartPos_insideBezierPointsCollection + 1, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
            Vector3 controlPosOfEndDirection = TryGetPosFromBezierPointCollection(bezierPointCollection, GetPos, i_ofSegmentStartPos_insideBezierPointsCollection + 2, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);

            Vector3 endPosition = TryGetPosFromBezierPointCollection(out bool endPosItemIsNull, bezierPointCollection, GetPos, i_ofSegmentEndPos_insideBezierPointsCollection, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
            TryAssignFallbackAndTextPositions(endPosItemIsNull, endPosition, ref fallbackPositionOfSegment, ref fallbackPositionOfSegment_hasAlreadyBeenAssigned, ref textPosition, ref textPosition_hasAlreadyBeenAssigned);

            TryDrawCubicBezierSegment(is2D, startPosition, endPosition, controlPosOfStartDirection, controlPosOfEndDirection, oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid, ref splineHasAlreadyCommunicatedANullItem, i_whileLoop, fallbackPositionOfSegment_hasAlreadyBeenAssigned, fallbackPositionOfSegment, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
        }

        static void TryDrawQuadraticSegment_ofSplineCase_start_control1_end<BezierPointCollection>(bool is2D, int i_ofSegmentStartPos_insideBezierPointsCollection, int i_ofSegmentEndPos_insideBezierPointsCollection, BezierPointCollection bezierPointCollection, FlexibleGetPosAtIndex<BezierPointCollection> GetPos, int i_whileLoop, ref bool textPosition_hasAlreadyBeenAssigned, ref Vector3 textPosition, ref bool splineHasAlreadyCommunicatedANullItem, Color color, float width, int straightSubDivisionsPerSegment, float textSize, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 fallbackPositionOfSegment = Vector3.zero;
            bool fallbackPositionOfSegment_hasAlreadyBeenAssigned = false;
            bool oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid = false;

            Vector3 startPosition = TryGetPosFromBezierPointCollection<BezierPointCollection>(out bool startPosItemIsNullOrInvalid, bezierPointCollection, GetPos, i_ofSegmentStartPos_insideBezierPointsCollection, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
            TryAssignFallbackAndTextPositions(startPosItemIsNullOrInvalid, startPosition, ref fallbackPositionOfSegment, ref fallbackPositionOfSegment_hasAlreadyBeenAssigned, ref textPosition, ref textPosition_hasAlreadyBeenAssigned);

            Vector3 controlPosInBetween = TryGetPosFromBezierPointCollection<BezierPointCollection>(bezierPointCollection, GetPos, i_ofSegmentStartPos_insideBezierPointsCollection + 1, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);

            Vector3 endPosition = TryGetPosFromBezierPointCollection<BezierPointCollection>(out bool endPosItemIsNullOrInvalid, bezierPointCollection, GetPos, i_ofSegmentEndPos_insideBezierPointsCollection, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
            TryAssignFallbackAndTextPositions(endPosItemIsNullOrInvalid, endPosition, ref fallbackPositionOfSegment, ref fallbackPositionOfSegment_hasAlreadyBeenAssigned, ref textPosition, ref textPosition_hasAlreadyBeenAssigned);

            TryDrawQuadraticBezierSegment(is2D, startPosition, endPosition, controlPosInBetween, oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid, ref splineHasAlreadyCommunicatedANullItem, i_whileLoop, fallbackPositionOfSegment_hasAlreadyBeenAssigned, fallbackPositionOfSegment, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
        }

        static void TryDrawCubicSegment_asGapCloserBetweenSplineEndAndStart<BezierPointCollection>(bool is2D, int i_lastSlotOfBezierPointsCollection, BezierPointCollection bezierPointCollection, FlexibleGetPosAtIndex<BezierPointCollection> GetPos, int i_whileLoop, ref bool textPosition_hasAlreadyBeenAssigned, ref Vector3 textPosition, ref bool splineHasAlreadyCommunicatedANullItem, Color color, float width, int straightSubDivisionsPerSegment, float textSize, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 fallbackPositionOfSegment = Vector3.zero;
            bool fallbackPositionOfSegment_hasAlreadyBeenAssigned = false;
            bool oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid = false;

            Vector3 gapClosingSegmentsStartPosition_isSplineEndPos = TryGetPosFromBezierPointCollection(out bool segmentsStartPosItemIsNull, bezierPointCollection, GetPos, i_lastSlotOfBezierPointsCollection, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
            TryAssignFallbackAndTextPositions(segmentsStartPosItemIsNull, gapClosingSegmentsStartPosition_isSplineEndPos, ref fallbackPositionOfSegment, ref fallbackPositionOfSegment_hasAlreadyBeenAssigned, ref textPosition, ref textPosition_hasAlreadyBeenAssigned);

            Vector3 backwardControlPosOfSplinesEndPos = TryGetPosFromBezierPointCollection(out bool backwardControlPosOfSplinesEndPos_isNull, bezierPointCollection, GetPos, i_lastSlotOfBezierPointsCollection - 1, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
            Vector3 controlPosOfSegmentStartDirection = Vector3.zero;
            if (backwardControlPosOfSplinesEndPos_isNull == false)
            {
                Vector3 splinesEndPos_toHisBackwardControlPos = backwardControlPosOfSplinesEndPos - gapClosingSegmentsStartPosition_isSplineEndPos;
                controlPosOfSegmentStartDirection = gapClosingSegmentsStartPosition_isSplineEndPos - splinesEndPos_toHisBackwardControlPos;
            }

            Vector3 gapClosingSegmentsEndPosition_isSplineStartPos = TryGetPosFromBezierPointCollection(out bool segmentsEndPosItemIsNull, bezierPointCollection, GetPos, 0, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
            TryAssignFallbackAndTextPositions(segmentsEndPosItemIsNull, gapClosingSegmentsEndPosition_isSplineStartPos, ref fallbackPositionOfSegment, ref fallbackPositionOfSegment_hasAlreadyBeenAssigned, ref textPosition, ref textPosition_hasAlreadyBeenAssigned);

            Vector3 forwardControlPosOfSplinesStartPos = TryGetPosFromBezierPointCollection(out bool forwardControlPosOfSplinesStartPos_isNull, bezierPointCollection, GetPos, 1, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
            Vector3 controlPosOfSegmentEndDirection = Vector3.zero;
            if (forwardControlPosOfSplinesStartPos_isNull == false)
            {
                Vector3 splinesStartPos_toHisForwardControlPos = forwardControlPosOfSplinesStartPos - gapClosingSegmentsEndPosition_isSplineStartPos;
                controlPosOfSegmentEndDirection = gapClosingSegmentsEndPosition_isSplineStartPos - splinesStartPos_toHisForwardControlPos;
            }

            TryDrawCubicBezierSegment(is2D, gapClosingSegmentsStartPosition_isSplineEndPos, gapClosingSegmentsEndPosition_isSplineStartPos, controlPosOfSegmentStartDirection, controlPosOfSegmentEndDirection, oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid, ref splineHasAlreadyCommunicatedANullItem, i_whileLoop, fallbackPositionOfSegment_hasAlreadyBeenAssigned, fallbackPositionOfSegment, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
        }

        static Vector3 DrawSpline_case_start_control1_endIsNextStart<BezierPointCollection>(bool is2D, out bool textPosition_hasAlreadyBeenAssigned, ref string text, BezierPointCollection bezierPointCollection, FlexibleGetPosAtIndex<BezierPointCollection> GetPos, int lengthOfCollection, Color color, DrawBasics.BezierPosInterpretation interpretationOfPointsCollection, float width, bool closeGapFromEndToStart, int straightSubDivisionsPerSegment, float textSize, float durationInSec, bool hiddenByNearerObjects)
        {
            textPosition_hasAlreadyBeenAssigned = false;
            Vector3 textPosition = Vector3.zero;

            if (lengthOfCollection < 3)
            {
                Debug.LogError("Bezier spline with BezierPosInterpretation of '" + interpretationOfPointsCollection + "' needs at least 3 control points, but the specified collection has only " + lengthOfCollection + " -> drawing skipped.");
            }
            else
            {
                bool splineHasAlreadyCommunicatedANullItem = false; //preventing log spam and many drawn lines (for multiple fallback texts)
                int i_startOfCurrSegment = 0;
                int i_endOfCurrSegment = i_startOfCurrSegment + 2;
                int maxNumberOfWhileLoops = 10000; //prevent freeze/endless loops
                int i_whileLoop = 0;
                while (i_endOfCurrSegment < lengthOfCollection)
                {
                    TryDrawQuadraticSegment_ofSplineCase_start_control1_end<BezierPointCollection>(is2D, i_startOfCurrSegment, i_endOfCurrSegment, bezierPointCollection, GetPos, i_whileLoop, ref textPosition_hasAlreadyBeenAssigned, ref textPosition, ref splineHasAlreadyCommunicatedANullItem, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);

                    i_startOfCurrSegment = i_endOfCurrSegment;
                    i_endOfCurrSegment = i_startOfCurrSegment + 2;

                    i_whileLoop++;
                    if (i_whileLoop > maxNumberOfWhileLoops)
                    {
                        Debug.LogError("Too many Bezier Segments (more than " + maxNumberOfWhileLoops + "). Drawing aborted.");
                        break;
                    }
                }

                if ((i_startOfCurrSegment + 1) < lengthOfCollection)
                {
                    //one splineDefiningCollectionPoint more than '2 points per segment (+1 final)' scheme:
                    //->one to little for another segment
                    if (closeGapFromEndToStart)
                    {
                        TryDrawQuadraticSegment_ofSplineCase_start_control1_end<BezierPointCollection>(is2D, i_startOfCurrSegment, 0, bezierPointCollection, GetPos, i_whileLoop, ref textPosition_hasAlreadyBeenAssigned, ref textPosition, ref splineHasAlreadyCommunicatedANullItem, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
                    }
                    else
                    {
                        text = "[<color=#e2aa00FF><icon=warning></color> points collection length is 1 too long for the '2 points per segment (+1 final)' scheme<br>-> last point discarded]<br>" + text;
                    }
                }
                else
                {
                    if (i_startOfCurrSegment < lengthOfCollection)
                    {
                        //bezierPointCollection.length perfectly fits the '3 points per segment (+1 final)' scheme
                        //-> no surplus points available
                        if (closeGapFromEndToStart)
                        {
                            TryDrawQuadraticSegment_asGapCloserBetweenSplineEndAndStart<BezierPointCollection>(is2D, i_startOfCurrSegment, bezierPointCollection, GetPos, i_whileLoop, ref textPosition_hasAlreadyBeenAssigned, ref textPosition, ref splineHasAlreadyCommunicatedANullItem, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
                        }
                    }
                }
            }
            return textPosition;
        }

        static void TryDrawQuadraticSegment_asGapCloserBetweenSplineEndAndStart<BezierPointCollection>(bool is2D, int i_lastSlotOfBezierPointsCollection, BezierPointCollection bezierPointCollection, FlexibleGetPosAtIndex<BezierPointCollection> GetPos, int i_whileLoop, ref bool textPosition_hasAlreadyBeenAssigned, ref Vector3 textPosition, ref bool splineHasAlreadyCommunicatedANullItem, Color color, float width, int straightSubDivisionsPerSegment, float textSize, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 fallbackPositionOfSegment = Vector3.zero;
            bool fallbackPositionOfSegment_hasAlreadyBeenAssigned = false;
            bool oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid = false;

            Vector3 gapClosingSegmentsStartPosition_isSplineEndPos = TryGetPosFromBezierPointCollection(out bool segmentsStartPosItemIsNull, bezierPointCollection, GetPos, i_lastSlotOfBezierPointsCollection, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
            TryAssignFallbackAndTextPositions(segmentsStartPosItemIsNull, gapClosingSegmentsStartPosition_isSplineEndPos, ref fallbackPositionOfSegment, ref fallbackPositionOfSegment_hasAlreadyBeenAssigned, ref textPosition, ref textPosition_hasAlreadyBeenAssigned);

            Vector3 backwardControlPosOfSplinesEndPos = TryGetPosFromBezierPointCollection(out bool backwardControlPosOfSplinesEndPos_isNull, bezierPointCollection, GetPos, i_lastSlotOfBezierPointsCollection - 1, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
            Vector3 controlPosOfSegmentStartDirection = Vector3.zero;
            if (backwardControlPosOfSplinesEndPos_isNull == false)
            {
                Vector3 splinesEndPos_toHisBackwardControlPos = backwardControlPosOfSplinesEndPos - gapClosingSegmentsStartPosition_isSplineEndPos;
                controlPosOfSegmentStartDirection = gapClosingSegmentsStartPosition_isSplineEndPos - splinesEndPos_toHisBackwardControlPos;
            }

            Vector3 gapClosingSegmentsEndPosition_isSplineStartPos = TryGetPosFromBezierPointCollection(out bool segmentsEndPosItemIsNull, bezierPointCollection, GetPos, 0, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
            TryAssignFallbackAndTextPositions(segmentsEndPosItemIsNull, gapClosingSegmentsEndPosition_isSplineStartPos, ref fallbackPositionOfSegment, ref fallbackPositionOfSegment_hasAlreadyBeenAssigned, ref textPosition, ref textPosition_hasAlreadyBeenAssigned);

            Vector3 forwardControlPosOfSplinesStartPos = TryGetPosFromBezierPointCollection(bezierPointCollection, GetPos, 1, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);

            TryDrawQuadraticBezierSegment(is2D, gapClosingSegmentsStartPosition_isSplineEndPos, gapClosingSegmentsEndPosition_isSplineStartPos, controlPosOfSegmentStartDirection, oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid, ref splineHasAlreadyCommunicatedANullItem, i_whileLoop, fallbackPositionOfSegment_hasAlreadyBeenAssigned, fallbackPositionOfSegment, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
        }

        static Vector3 DrawSpline_case_onlySegmentStartPoints_backwardForwardIsAligned<BezierPointCollection>(bool is2D, out bool textPosition_hasAlreadyBeenAssigned, BezierPointCollection bezierPointCollection, FlexibleGetPosAtIndex<BezierPointCollection> GetPos, FlexibleGetDirectionControlPosOfTransform<BezierPointCollection> GetForwardControlPos, FlexibleGetDirectionControlPosOfTransform<BezierPointCollection> GetBackwardControlPos, int lengthOfCollection, Color color, DrawBasics.BezierPosInterpretation interpretationOfPointsCollection, float width, bool closeGapFromEndToStart, int straightSubDivisionsPerSegment, float textSize, float durationInSec, bool hiddenByNearerObjects)
        {
            textPosition_hasAlreadyBeenAssigned = false;
            Vector3 textPosition = Vector3.zero;

            if (lengthOfCollection < 2)
            {
                Debug.LogError("Bezier spline with BezierPosInterpretation of '" + interpretationOfPointsCollection + "' needs at least 2 control points, but the specified collection has only " + lengthOfCollection + " -> drawing skipped.");
            }
            else
            {
                bool splineHasAlreadyCommunicatedANullItem = false; //preventing log spam and many drawn lines (for multiple fallback texts)
                for (int i_segmentEndPos = 1; i_segmentEndPos < lengthOfCollection; i_segmentEndPos++)
                {
                    int i_segmentStartPos = i_segmentEndPos - 1;
                    TryDrawSegment_ofSplineCase_onlySegmentStartPoints_backwardForwardIsAligned<BezierPointCollection>(is2D, i_segmentStartPos, i_segmentEndPos, bezierPointCollection, GetPos, GetForwardControlPos, GetBackwardControlPos, ref textPosition_hasAlreadyBeenAssigned, ref textPosition, ref splineHasAlreadyCommunicatedANullItem, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
                }

                if (closeGapFromEndToStart)
                {
                    TryDrawSegment_ofSplineCase_onlySegmentStartPoints_backwardForwardIsAligned<BezierPointCollection>(is2D, lengthOfCollection - 1, 0, bezierPointCollection, GetPos, GetForwardControlPos, GetBackwardControlPos, ref textPosition_hasAlreadyBeenAssigned, ref textPosition, ref splineHasAlreadyCommunicatedANullItem, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
                }
            }
            return textPosition;
        }

        static void TryDrawSegment_ofSplineCase_onlySegmentStartPoints_backwardForwardIsAligned<BezierPointCollection>(bool is2D, int i_segmentStartPos, int i_segmentEndPos, BezierPointCollection bezierPointCollection, FlexibleGetPosAtIndex<BezierPointCollection> GetPos, FlexibleGetDirectionControlPosOfTransform<BezierPointCollection> GetForwardControlPos, FlexibleGetDirectionControlPosOfTransform<BezierPointCollection> GetBackwardControlPos, ref bool textPosition_hasAlreadyBeenAssigned, ref Vector3 textPosition, ref bool splineHasAlreadyCommunicatedANullItem, Color color, float width, int straightSubDivisionsPerSegment, float textSize, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 fallbackPositionOfSegment = Vector3.zero;
            Vector3 controlPosOfEndDirection = Vector3.zero;
            Vector3 controlPosOfStartDirection = Vector3.zero;
            bool fallbackPositionOfSegment_hasAlreadyBeenAssigned = false;
            bool oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid = false;

            Vector3 startPosition = TryGetPosFromBezierPointCollection<BezierPointCollection>(out bool startPosItemIsNullOrInvalid, bezierPointCollection, GetPos, i_segmentStartPos, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
            TryAssignFallbackAndTextPositions(startPosItemIsNullOrInvalid, startPosition, ref fallbackPositionOfSegment, ref fallbackPositionOfSegment_hasAlreadyBeenAssigned, ref textPosition, ref textPosition_hasAlreadyBeenAssigned);
            if (startPosItemIsNullOrInvalid == false) { controlPosOfStartDirection = GetForwardControlPos(bezierPointCollection, i_segmentStartPos); }

            Vector3 endPosition = TryGetPosFromBezierPointCollection<BezierPointCollection>(out bool endPosItemIsNullOrInvalid, bezierPointCollection, GetPos, i_segmentEndPos, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
            TryAssignFallbackAndTextPositions(endPosItemIsNullOrInvalid, endPosition, ref fallbackPositionOfSegment, ref fallbackPositionOfSegment_hasAlreadyBeenAssigned, ref textPosition, ref textPosition_hasAlreadyBeenAssigned);
            if (endPosItemIsNullOrInvalid == false) { controlPosOfEndDirection = GetBackwardControlPos(bezierPointCollection, i_segmentEndPos); }

            TryDrawCubicBezierSegment(is2D, startPosition, endPosition, controlPosOfStartDirection, controlPosOfEndDirection, oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid, ref splineHasAlreadyCommunicatedANullItem, i_segmentStartPos, fallbackPositionOfSegment_hasAlreadyBeenAssigned, fallbackPositionOfSegment, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
        }

        static Vector3 DrawSpline_case_onlySegmentStartPoints_backwardForwardIsMirrored<BezierPointCollection>(bool is2D, out bool textPosition_hasAlreadyBeenAssigned, BezierPointCollection bezierPointCollection, FlexibleGetPosAtIndex<BezierPointCollection> GetPos, FlexibleGetDirectionControlPosOfTransform<BezierPointCollection> GetForwardControlPos, int lengthOfCollection, Color color, DrawBasics.BezierPosInterpretation interpretationOfPointsCollection, float width, bool closeGapFromEndToStart, int straightSubDivisionsPerSegment, float textSize, float durationInSec, bool hiddenByNearerObjects)
        {
            textPosition_hasAlreadyBeenAssigned = false;
            Vector3 textPosition = Vector3.zero;

            if (lengthOfCollection < 2)
            {
                Debug.LogError("Bezier spline with BezierPosInterpretation of '" + interpretationOfPointsCollection + "' needs at least 2 control points, but the specified collection has only " + lengthOfCollection + " -> drawing skipped.");
            }
            else
            {
                bool splineHasAlreadyCommunicatedANullItem = false; //preventing log spam and many drawn lines (for multiple fallback texts)
                for (int i_segmentEndPos = 1; i_segmentEndPos < lengthOfCollection; i_segmentEndPos++)
                {
                    int i_segmentStartPos = i_segmentEndPos - 1;
                    TryDrawSegment_ofSplineCase_onlySegmentStartPoints_backwardForwardIsMirrored<BezierPointCollection>(is2D, i_segmentStartPos, i_segmentEndPos, bezierPointCollection, GetPos, GetForwardControlPos, ref textPosition_hasAlreadyBeenAssigned, ref textPosition, ref splineHasAlreadyCommunicatedANullItem, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
                }

                if (closeGapFromEndToStart)
                {
                    TryDrawSegment_ofSplineCase_onlySegmentStartPoints_backwardForwardIsMirrored<BezierPointCollection>(is2D, lengthOfCollection - 1, 0, bezierPointCollection, GetPos, GetForwardControlPos, ref textPosition_hasAlreadyBeenAssigned, ref textPosition, ref splineHasAlreadyCommunicatedANullItem, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
                }
            }
            return textPosition;
        }

        static void TryDrawSegment_ofSplineCase_onlySegmentStartPoints_backwardForwardIsMirrored<BezierPointCollection>(bool is2D, int i_segmentStartPos, int i_segmentEndPos, BezierPointCollection bezierPointCollection, FlexibleGetPosAtIndex<BezierPointCollection> GetPos, FlexibleGetDirectionControlPosOfTransform<BezierPointCollection> GetForwardControlPos, ref bool textPosition_hasAlreadyBeenAssigned, ref Vector3 textPosition, ref bool splineHasAlreadyCommunicatedANullItem, Color color, float width, int straightSubDivisionsPerSegment, float textSize, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 fallbackPositionOfSegment = Vector3.zero;
            Vector3 controlPosOfEndDirection = Vector3.zero;
            Vector3 controlPosOfStartDirection = Vector3.zero;
            bool fallbackPositionOfSegment_hasAlreadyBeenAssigned = false;
            bool oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid = false;

            Vector3 startPosition = TryGetPosFromBezierPointCollection<BezierPointCollection>(out bool startPosItemIsNullOrInvalid, bezierPointCollection, GetPos, i_segmentStartPos, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
            TryAssignFallbackAndTextPositions(startPosItemIsNullOrInvalid, startPosition, ref fallbackPositionOfSegment, ref fallbackPositionOfSegment_hasAlreadyBeenAssigned, ref textPosition, ref textPosition_hasAlreadyBeenAssigned);
            if (startPosItemIsNullOrInvalid == false) { controlPosOfStartDirection = GetForwardControlPos(bezierPointCollection, i_segmentStartPos); }

            Vector3 endPosition = TryGetPosFromBezierPointCollection<BezierPointCollection>(out bool endPosItemIsNullOrInvalid, bezierPointCollection, GetPos, i_segmentEndPos, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
            TryAssignFallbackAndTextPositions(endPosItemIsNullOrInvalid, endPosition, ref fallbackPositionOfSegment, ref fallbackPositionOfSegment_hasAlreadyBeenAssigned, ref textPosition, ref textPosition_hasAlreadyBeenAssigned);
            if (endPosItemIsNullOrInvalid == false)
            {
                Vector3 controlPosOfEndDirection_forward = GetForwardControlPos(bezierPointCollection, i_segmentEndPos);
                Vector3 endPos_to_endPosForwardControlPos = controlPosOfEndDirection_forward - endPosition;
                controlPosOfEndDirection = endPosition - endPos_to_endPosForwardControlPos;
            }

            TryDrawCubicBezierSegment(is2D, startPosition, endPosition, controlPosOfStartDirection, controlPosOfEndDirection, oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid, ref splineHasAlreadyCommunicatedANullItem, i_segmentStartPos, fallbackPositionOfSegment_hasAlreadyBeenAssigned, fallbackPositionOfSegment, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
        }

        static Vector3 DrawSpline_case_onlySegmentStartPoints_backwardForwardIsKinked<BezierPointCollection>(bool is2D, out bool textPosition_hasAlreadyBeenAssigned, BezierPointCollection bezierPointCollection, FlexibleGetPosAtIndex<BezierPointCollection> GetPos, FlexibleGetDirectionControlPosOfTransform<BezierPointCollection> GetForwardControlPos, int lengthOfCollection, Color color, DrawBasics.BezierPosInterpretation interpretationOfPointsCollection, float width, bool closeGapFromEndToStart, int straightSubDivisionsPerSegment, float textSize, float durationInSec, bool hiddenByNearerObjects)
        {
            textPosition_hasAlreadyBeenAssigned = false;
            Vector3 textPosition = Vector3.zero;

            if (lengthOfCollection < 2)
            {
                Debug.LogError("Bezier spline with BezierPosInterpretation of '" + interpretationOfPointsCollection + "' needs at least 2 control points, but the specified collection has only " + lengthOfCollection + " -> drawing skipped.");
            }
            else
            {
                bool splineHasAlreadyCommunicatedANullItem = false; //preventing log spam and many drawn lines (for multiple fallback texts)
                for (int i_segmentEndPos = 1; i_segmentEndPos < lengthOfCollection; i_segmentEndPos++)
                {
                    int i_segmentStartPos = i_segmentEndPos - 1;
                    TryDrawSegment_ofSplineCase_onlySegmentStartPoints_backwardForwardIsKinked<BezierPointCollection>(is2D, i_segmentStartPos, i_segmentEndPos, bezierPointCollection, GetPos, GetForwardControlPos, ref textPosition_hasAlreadyBeenAssigned, ref textPosition, ref splineHasAlreadyCommunicatedANullItem, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
                }

                if (closeGapFromEndToStart)
                {
                    TryDrawSegment_ofSplineCase_onlySegmentStartPoints_backwardForwardIsKinked<BezierPointCollection>(is2D, lengthOfCollection - 1, 0, bezierPointCollection, GetPos, GetForwardControlPos, ref textPosition_hasAlreadyBeenAssigned, ref textPosition, ref splineHasAlreadyCommunicatedANullItem, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
                }
            }
            return textPosition;
        }

        static void TryDrawSegment_ofSplineCase_onlySegmentStartPoints_backwardForwardIsKinked<BezierPointCollection>(bool is2D, int i_segmentStartPos, int i_segmentEndPos, BezierPointCollection bezierPointCollection, FlexibleGetPosAtIndex<BezierPointCollection> GetPos, FlexibleGetDirectionControlPosOfTransform<BezierPointCollection> GetForwardControlPos, ref bool textPosition_hasAlreadyBeenAssigned, ref Vector3 textPosition, ref bool splineHasAlreadyCommunicatedANullItem, Color color, float width, int straightSubDivisionsPerSegment, float textSize, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 fallbackPositionOfSegment = Vector3.zero;
            Vector3 controlPosInBetween = Vector3.zero;
            bool fallbackPositionOfSegment_hasAlreadyBeenAssigned = false;
            bool oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid = false;

            Vector3 startPosition = TryGetPosFromBezierPointCollection<BezierPointCollection>(out bool startPosItemIsNullOrInvalid, bezierPointCollection, GetPos, i_segmentStartPos, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
            TryAssignFallbackAndTextPositions(startPosItemIsNullOrInvalid, startPosition, ref fallbackPositionOfSegment, ref fallbackPositionOfSegment_hasAlreadyBeenAssigned, ref textPosition, ref textPosition_hasAlreadyBeenAssigned);
            if (startPosItemIsNullOrInvalid == false) { controlPosInBetween = GetForwardControlPos(bezierPointCollection, i_segmentStartPos); }

            Vector3 endPosition = TryGetPosFromBezierPointCollection<BezierPointCollection>(out bool endPosItemIsNullOrInvalid, bezierPointCollection, GetPos, i_segmentEndPos, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
            TryAssignFallbackAndTextPositions(endPosItemIsNullOrInvalid, endPosition, ref fallbackPositionOfSegment, ref fallbackPositionOfSegment_hasAlreadyBeenAssigned, ref textPosition, ref textPosition_hasAlreadyBeenAssigned);

            TryDrawQuadraticBezierSegment(is2D, startPosition, endPosition, controlPosInBetween, oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid, ref splineHasAlreadyCommunicatedANullItem, i_segmentStartPos, fallbackPositionOfSegment_hasAlreadyBeenAssigned, fallbackPositionOfSegment, color, width, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
        }

        static Vector3 TryGetPosFromBezierPointCollection<BezierPointCollection>(BezierPointCollection bezierPointCollection, FlexibleGetPosAtIndex<BezierPointCollection> GetPos, int i_insideBezierPointCollection_whereToTryObtain, ref bool oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid)
        {
            return TryGetPosFromBezierPointCollection<BezierPointCollection>(out bool unused, bezierPointCollection, GetPos, i_insideBezierPointCollection_whereToTryObtain, ref oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid);
        }

        static Vector3 TryGetPosFromBezierPointCollection<BezierPointCollection>(out bool collectionItemIsNullOrInvalid, BezierPointCollection bezierPointCollection, FlexibleGetPosAtIndex<BezierPointCollection> GetPos, int i_insideBezierPointCollection_whereToTryObtain, ref bool oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid)
        {
            Vector3 obtainedPos = GetPos(bezierPointCollection, i_insideBezierPointCollection_whereToTryObtain, out collectionItemIsNullOrInvalid);
            if (collectionItemIsNullOrInvalid) { oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid = true; }
            return obtainedPos;
        }

        static void TryAssignFallbackAndTextPositions(bool collectionItemIsNullOrInvalid, Vector3 position, ref Vector3 fallbackPositionOfSegment, ref bool fallbackPositionOfSegment_hasAlreadyBeenAssigned, ref Vector3 textPosition, ref bool textPosition_hasAlreadyBeenAssigned)
        {
            if (collectionItemIsNullOrInvalid == false)
            {
                AssignFallbackPositionOfSegment(ref fallbackPositionOfSegment, ref fallbackPositionOfSegment_hasAlreadyBeenAssigned, position);
                AssignTextPosition(ref textPosition, ref textPosition_hasAlreadyBeenAssigned, position);
            }
        }

        static void AssignFallbackPositionOfSegment(ref Vector3 fallbackPositionOfSegment, ref bool fallbackPositionOfSegment_hasAlreadyBeenAssigned, Vector3 fallbackPositionOfSegmentCandidate)
        {
            if (fallbackPositionOfSegment_hasAlreadyBeenAssigned == false)
            {
                fallbackPositionOfSegment = fallbackPositionOfSegmentCandidate;
                fallbackPositionOfSegment_hasAlreadyBeenAssigned = true;
            }
        }

        static void AssignTextPosition(ref Vector3 textPosition, ref bool textPosition_hasAlreadyBeenAssigned, Vector3 textPositionCandidate)
        {
            if (textPosition_hasAlreadyBeenAssigned == false)
            {
                textPosition = textPositionCandidate;
                textPosition_hasAlreadyBeenAssigned = true;
            }
        }

        static void TryDrawCubicBezierSegment(bool is2D, Vector3 startPosition, Vector3 endPosition, Vector3 controlPosOfStartDirection, Vector3 controlPosOfEndDirection, bool oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid, ref bool splineHasAlreadyCommunicatedANullItem, int i_segment_usedForErrorText, bool fallbackPositionOfSegment_hasAlreadyBeenAssigned, Vector3 fallbackPositionOfSegment, Color color, float width, int straightSubDivisionsPerSegment, float textSize, float durationInSec, bool hiddenByNearerObjects)
        {
            if (oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid)
            {
                CommunicateMissingArrayItem(ref splineHasAlreadyCommunicatedANullItem, i_segment_usedForErrorText, fallbackPositionOfSegment_hasAlreadyBeenAssigned, fallbackPositionOfSegment, color, textSize, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                BezierSegmentCubic(is2D, startPosition, endPosition, controlPosOfStartDirection, controlPosOfEndDirection, color, null, width, straightSubDivisionsPerSegment, 0.1f, false, durationInSec, hiddenByNearerObjects);
            }
        }

        static void TryDrawQuadraticBezierSegment(bool is2D, Vector3 startPosition, Vector3 endPosition, Vector3 controlPosInBetween, bool oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid, ref bool splineHasAlreadyCommunicatedANullItem, int i_segment_usedForErrorText, bool fallbackPositionOfSegment_hasAlreadyBeenAssigned, Vector3 fallbackPositionOfSegment, Color color, float width, int straightSubDivisionsPerSegment, float textSize, float durationInSec, bool hiddenByNearerObjects)
        {
            if (oneOfTheSegmentPointsIsUndefined_becauseCollectionItemIsNullOrInvalid)
            {
                CommunicateMissingArrayItem(ref splineHasAlreadyCommunicatedANullItem, i_segment_usedForErrorText, fallbackPositionOfSegment_hasAlreadyBeenAssigned, fallbackPositionOfSegment, color, textSize, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                BezierSegmentQuadratic(is2D, startPosition, endPosition, controlPosInBetween, color, null, width, straightSubDivisionsPerSegment, 0.1f, false, durationInSec, hiddenByNearerObjects);
            }
        }

        static void CommunicateMissingArrayItem(ref bool splineHasAlreadyCommunicatedANullItem, int i_segment_usedForErrorText, bool hasfallbackPositionOfSegment, Vector3 fallbackPositionOfSegment, Color color, float textSize, float durationInSec, bool hiddenByNearerObjects)
        {
            if (splineHasAlreadyCommunicatedANullItem == false)
            {
                if (hasfallbackPositionOfSegment)
                {
                    UtilitiesDXXL_Text.WriteFramed("[<color=#ce0e0eFF><icon=logMessageError></color> An item in points collection is null/invalid<br>-> skip drawing of bezier segment (i=" + i_segment_usedForErrorText + ")]", fallbackPositionOfSegment, color, textSize, default(Vector3), default(Vector3), DrawText.TextAnchorDXXL.UpperRight, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.02f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
                }
                else
                {
                    Debug.LogError("An item in points collection is null/invalid -> skip drawing of bezier spline segment (i=" + i_segment_usedForErrorText + ")");
                }
            }
            splineHasAlreadyCommunicatedANullItem = true;
        }

    }

}
