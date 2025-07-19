namespace DrawXXL
{
    using System;
    using UnityEngine;

    [Serializable]
    public class InternalDXXL_BezierControlPointTriplet2D
    {
        public bool isHighlighted;
        public int i_ofThisPoint_insideControlPointsList;
        public BezierSplineDrawer2D bezierSplineDrawer_thisPointIsPartOf;

        [SerializeField] public bool alignedHelperPoints_areOnTheSameSideOfTheAnchor;
        [SerializeField] public float progress0to1_ofPlusButtonPosition_inUpcomingBezierSegment;

        public InternalDXXL_BezierControlAnchorSubPoint2D anchorPoint;
        public InternalDXXL_BezierControlHelperSubPoint2D forwardHelperPoint;
        public InternalDXXL_BezierControlHelperSubPoint2D backwardHelperPoint;

        InternalDXXL_BezierPointShapeConfig2D pointShape_afterSpaceChange_inUnitsOfGlobalSpace;
        InternalDXXL_BezierPointShapeConfig2D pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace;

        public Rect inspectorRect_reservedForThisTriplet;
        public bool minusButtonAtThisListItem_hasBeenClickedInInspector = false;

        public void Initialize(BezierSplineDrawer2D containingSplineDrawer, Vector2 initialPos_inUnitsOfGlobalSpace, Vector2 initialForwardDir_inUnitsOfGlobalSpace_normalized, InternalDXXL_BezierControlAnchorSubPoint.JunctureType junctureType)
        {
            bezierSplineDrawer_thisPointIsPartOf = containingSplineDrawer;

            isHighlighted = false; //-> the caller has to decide whether and take care of highlighting newly created control points after this initialization

            alignedHelperPoints_areOnTheSameSideOfTheAnchor = false;
            progress0to1_ofPlusButtonPosition_inUpcomingBezierSegment = 0.5f;

            anchorPoint = new InternalDXXL_BezierControlAnchorSubPoint2D();
            anchorPoint.InitializeValuesThatAreIndependentFromOtherSubPoints(this);
            anchorPoint.subPointType = InternalDXXL_BezierControlSubPoint.SubPointType.anchor;
            anchorPoint.isUsed = true;
            anchorPoint.junctureType = junctureType;
            anchorPoint.SetPos_inUnitsOfGlobalSpace(initialPos_inUnitsOfGlobalSpace, false, null);

            forwardHelperPoint = new InternalDXXL_BezierControlHelperSubPoint2D();
            forwardHelperPoint.InitializeValuesThatAreIndependentFromOtherSubPoints(this);
            forwardHelperPoint.subPointType = InternalDXXL_BezierControlSubPoint.SubPointType.forwardHelper;
            forwardHelperPoint.isForward_notBackward = true;
            forwardHelperPoint.isUsed = true;
            forwardHelperPoint.Set_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized(initialForwardDir_inUnitsOfGlobalSpace_normalized, false, null);
            float initial_forwardWeightDistance_inUnitsOfGlobalSpace = bezierSplineDrawer_thisPointIsPartOf.TransformLength_fromUnitsOfActiveDrawSpace_toGlobalSpace(bezierSplineDrawer_thisPointIsPartOf.forwardWeightDistance_ofNewlyCreatedPoints_inUnitsOfActiveDrawSpace);
            Vector2 initialPos_ofForwardHelperPoint_inUnitsOfGlobalSpace = initialPos_inUnitsOfGlobalSpace + initialForwardDir_inUnitsOfGlobalSpace_normalized * initial_forwardWeightDistance_inUnitsOfGlobalSpace;
            forwardHelperPoint.SetPos_inUnitsOfGlobalSpace(initialPos_ofForwardHelperPoint_inUnitsOfGlobalSpace, false, null);
            forwardHelperPoint.Set_absDistanceToAnchorPoint_inUnitsOfGlobalSpace(initial_forwardWeightDistance_inUnitsOfGlobalSpace, false, null);

            backwardHelperPoint = new InternalDXXL_BezierControlHelperSubPoint2D();
            backwardHelperPoint.InitializeValuesThatAreIndependentFromOtherSubPoints(this);
            backwardHelperPoint.subPointType = InternalDXXL_BezierControlSubPoint.SubPointType.backwardHelper;
            backwardHelperPoint.isForward_notBackward = false;
            backwardHelperPoint.isUsed = true;
            backwardHelperPoint.Set_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized(-initialForwardDir_inUnitsOfGlobalSpace_normalized, false, null);
            float initial_backwardWeightDistance_inUnitsOfGlobalSpace = bezierSplineDrawer_thisPointIsPartOf.TransformLength_fromUnitsOfActiveDrawSpace_toGlobalSpace(bezierSplineDrawer_thisPointIsPartOf.backwardWeightDistance_ofNewlyCreatedPoints_inUnitsOfActiveDrawSpace);
            Vector2 initialPos_ofBackwardHelperPoint_inUnitsOfGlobalSpace = initialPos_inUnitsOfGlobalSpace - initialForwardDir_inUnitsOfGlobalSpace_normalized * initial_backwardWeightDistance_inUnitsOfGlobalSpace;
            backwardHelperPoint.SetPos_inUnitsOfGlobalSpace(initialPos_ofBackwardHelperPoint_inUnitsOfGlobalSpace, false, null);
            backwardHelperPoint.Set_absDistanceToAnchorPoint_inUnitsOfGlobalSpace(initial_backwardWeightDistance_inUnitsOfGlobalSpace, false, null);
        }

        public void ReassignIndexInsideControlPointsList(int new_i)
        {
            i_ofThisPoint_insideControlPointsList = new_i;

            if (anchorPoint != null) //-> skipping newly created points (whose sub points don't exist yet). They get the value in the "controlPointTriplet.Initialize()" function 
            {
                anchorPoint.ReassignIndexInsideControlPointsList(new_i);
            }

            if (forwardHelperPoint != null) //-> skipping newly created points (whose sub points don't exist yet). They get the value in the "controlPointTriplet.Initialize()" function 
            {
                forwardHelperPoint.ReassignIndexInsideControlPointsList(new_i);
            }

            if (backwardHelperPoint != null) //-> skipping newly created points (whose sub points don't exist yet). They get the value in the "controlPointTriplet.Initialize()" function 
            {
                backwardHelperPoint.ReassignIndexInsideControlPointsList(new_i);
            }
        }

        public InternalDXXL_BezierControlHelperSubPoint2D GetAHelperPoint(bool requestedHelperPoint_isForward_notBackward)
        {
            if (requestedHelperPoint_isForward_notBackward)
            {
                return forwardHelperPoint;
            }
            else
            {
                return backwardHelperPoint;
            }
        }

        public InternalDXXL_BezierControlSubPoint2D GetASubPoint(InternalDXXL_BezierControlSubPoint.SubPointType typeOfRequestedSubPoint)
        {
            switch (typeOfRequestedSubPoint)
            {
                case InternalDXXL_BezierControlSubPoint.SubPointType.backwardHelper:
                    return backwardHelperPoint;
                case InternalDXXL_BezierControlSubPoint.SubPointType.anchor:
                    return anchorPoint;
                case InternalDXXL_BezierControlSubPoint.SubPointType.forwardHelper:
                    return forwardHelperPoint;
                default:
                    return anchorPoint;
            }
        }

        public bool IsFirstControlPoint()
        {
            return bezierSplineDrawer_thisPointIsPartOf.IsFirstControlPoint(this);
        }

        public bool IsLastControlPoint()
        {
            return bezierSplineDrawer_thisPointIsPartOf.IsLastControlPoint(this);
        }

        public InternalDXXL_BezierControlPointTriplet2D GetNextControlPointTripletAlongSplineDir(bool allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet)
        {
            return bezierSplineDrawer_thisPointIsPartOf.GetNextControlPointTriplet(this, allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet);
        }

        public InternalDXXL_BezierControlPointTriplet2D GetPreviousControlPointTripletAlongSplineDir(bool allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet)
        {
            return bezierSplineDrawer_thisPointIsPartOf.GetPreviousControlPointTriplet(this, allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet);
        }

        public void Save_currentPosConfigInUnitsOfGlobalSpace_as_posConfigAfterSpaceChangeInUnitsOfGlobalSpace()
        {
            pointShape_afterSpaceChange_inUnitsOfGlobalSpace.anchorPos = anchorPoint.GetPos_inUnitsOfGlobalSpace();
            pointShape_afterSpaceChange_inUnitsOfGlobalSpace.direction_toForward_normalized = anchorPoint.Get_direction_toForward_inUnitsOfGlobalSpace_normalized();
            pointShape_afterSpaceChange_inUnitsOfGlobalSpace.direction_toBackward_normalized = anchorPoint.Get_direction_toBackward_inUnitsOfGlobalSpace_normalized();
            pointShape_afterSpaceChange_inUnitsOfGlobalSpace.forwardHelperPos = forwardHelperPoint.GetPos_inUnitsOfGlobalSpace();
            pointShape_afterSpaceChange_inUnitsOfGlobalSpace.absDistanceToForwardAnchorPoint = forwardHelperPoint.Get_absDistanceToAnchorPoint_inUnitsOfGlobalSpace();
            pointShape_afterSpaceChange_inUnitsOfGlobalSpace.backwardHelperPos = backwardHelperPoint.GetPos_inUnitsOfGlobalSpace();
            pointShape_afterSpaceChange_inUnitsOfGlobalSpace.absDistanceToBackwardAnchorPoint = backwardHelperPoint.Get_absDistanceToAnchorPoint_inUnitsOfGlobalSpace();
        }

        public void Save_currentPosConfigInUnitsOfActiveDrawSpace_as_posConfigAfterSpaceChangeInUnitsOfActiveDrawSpace()
        {
            pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.anchorPos = anchorPoint.GetPos_inUnitsOfActiveDrawSpace();
            pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.direction_toForward_normalized = anchorPoint.Get_direction_toForward_inUnitsOfActiveDrawSpace_normalized();
            pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.direction_toBackward_normalized = anchorPoint.Get_direction_toBackward_inUnitsOfActiveDrawSpace_normalized();
            pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.forwardHelperPos = forwardHelperPoint.GetPos_inUnitsOfActiveDrawSpace();
            pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.absDistanceToForwardAnchorPoint = forwardHelperPoint.Get_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace();
            pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.backwardHelperPos = backwardHelperPoint.GetPos_inUnitsOfActiveDrawSpace();
            pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.absDistanceToBackwardAnchorPoint = backwardHelperPoint.Get_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace();
        }

        public void Save_currentPosConfigInUnitsOfActiveDrawSpace_as_posConfigAfterSpaceChangeInUnitsOfGlobalSpace()
        {
            pointShape_afterSpaceChange_inUnitsOfGlobalSpace.anchorPos = anchorPoint.GetPos_inUnitsOfActiveDrawSpace();
            pointShape_afterSpaceChange_inUnitsOfGlobalSpace.direction_toForward_normalized = anchorPoint.Get_direction_toForward_inUnitsOfActiveDrawSpace_normalized();
            pointShape_afterSpaceChange_inUnitsOfGlobalSpace.direction_toBackward_normalized = anchorPoint.Get_direction_toBackward_inUnitsOfActiveDrawSpace_normalized();
            pointShape_afterSpaceChange_inUnitsOfGlobalSpace.forwardHelperPos = forwardHelperPoint.GetPos_inUnitsOfActiveDrawSpace();
            pointShape_afterSpaceChange_inUnitsOfGlobalSpace.absDistanceToForwardAnchorPoint = forwardHelperPoint.Get_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace();
            pointShape_afterSpaceChange_inUnitsOfGlobalSpace.backwardHelperPos = backwardHelperPoint.GetPos_inUnitsOfActiveDrawSpace();
            pointShape_afterSpaceChange_inUnitsOfGlobalSpace.absDistanceToBackwardAnchorPoint = backwardHelperPoint.Get_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace();
        }

        public void Save_currentPosConfigInUnitsOfGlobalSpace_as_posConfigAfterSpaceChangeInUnitsOfActiveDrawSpace()
        {
            pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.anchorPos = anchorPoint.GetPos_inUnitsOfGlobalSpace();
            pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.direction_toForward_normalized = anchorPoint.Get_direction_toForward_inUnitsOfGlobalSpace_normalized();
            pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.direction_toBackward_normalized = anchorPoint.Get_direction_toBackward_inUnitsOfGlobalSpace_normalized();
            pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.forwardHelperPos = forwardHelperPoint.GetPos_inUnitsOfGlobalSpace();
            pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.absDistanceToForwardAnchorPoint = forwardHelperPoint.Get_absDistanceToAnchorPoint_inUnitsOfGlobalSpace();
            pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.backwardHelperPos = backwardHelperPoint.GetPos_inUnitsOfGlobalSpace();
            pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.absDistanceToBackwardAnchorPoint = backwardHelperPoint.Get_absDistanceToAnchorPoint_inUnitsOfGlobalSpace();
        }

        public void ApplySaved_posConfigAfterSpaceChangeInUnitsOfGlobalSpace()
        {
            anchorPoint.SetPos_inUnitsOfGlobalSpace(pointShape_afterSpaceChange_inUnitsOfGlobalSpace.anchorPos, false, null);
            anchorPoint.Set_direction_toForward_inUnitsOfGlobalSpace_normalized(pointShape_afterSpaceChange_inUnitsOfGlobalSpace.direction_toForward_normalized, false, null);
            anchorPoint.Set_direction_toBackward_inUnitsOfGlobalSpace_normalized(pointShape_afterSpaceChange_inUnitsOfGlobalSpace.direction_toBackward_normalized, false, null);
            forwardHelperPoint.SetPos_inUnitsOfGlobalSpace(pointShape_afterSpaceChange_inUnitsOfGlobalSpace.forwardHelperPos, false, null);
            forwardHelperPoint.Set_absDistanceToAnchorPoint_inUnitsOfGlobalSpace(pointShape_afterSpaceChange_inUnitsOfGlobalSpace.absDistanceToForwardAnchorPoint, false, null);
            backwardHelperPoint.SetPos_inUnitsOfGlobalSpace(pointShape_afterSpaceChange_inUnitsOfGlobalSpace.backwardHelperPos, false, null);
            backwardHelperPoint.Set_absDistanceToAnchorPoint_inUnitsOfGlobalSpace(pointShape_afterSpaceChange_inUnitsOfGlobalSpace.absDistanceToBackwardAnchorPoint, false, null);
        }

        public void ApplySaved_posConfigAfterSpaceChangeInUnitsOfActiveDrawSpace()
        {
            anchorPoint.SetPos_inUnitsOfActiveDrawSpace(pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.anchorPos, false, null);
            anchorPoint.Set_direction_toForward_inUnitsOfActiveDrawSpace_normalized(pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.direction_toForward_normalized, false, null);
            anchorPoint.Set_direction_toBackward_inUnitsOfActiveDrawSpace_normalized(pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.direction_toBackward_normalized, false, null);
            forwardHelperPoint.SetPos_inUnitsOfActiveDrawSpace(pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.forwardHelperPos, false, null);
            forwardHelperPoint.Set_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace(pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.absDistanceToForwardAnchorPoint, false, null);
            backwardHelperPoint.SetPos_inUnitsOfActiveDrawSpace(pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.backwardHelperPos, false, null);
            backwardHelperPoint.Set_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace(pointShape_afterSpaceChange_inUnitsOfActiveDrawSpace.absDistanceToBackwardAnchorPoint, false, null);
        }

        public bool IsEndPointToVoid_atStartOrEndOfAnUnclosedSpline()
        {
            if (bezierSplineDrawer_thisPointIsPartOf.gapFromEndToStart_isClosed == false)
            {
                if (IsFirstControlPoint() || IsLastControlPoint())
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsPartOfThisTriplet(InternalDXXL_BezierControlSubPoint2D subPointToCheckIfItIsPartOfThisTriplet)
        {
            if (subPointToCheckIfItIsPartOfThisTriplet == backwardHelperPoint)
            {
                return true;
            }

            if (subPointToCheckIfItIsPartOfThisTriplet == anchorPoint)
            {
                return true;
            }

            if (subPointToCheckIfItIsPartOfThisTriplet == forwardHelperPoint)
            {
                return true;
            }

            return false;
        }

        public bool CheckIf_foldableHelperPoints_areUnfolded_inTheInspectorList()
        {
            if (backwardHelperPoint.IsUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid())
            {
                //-> not foldable
            }
            else
            {
                if (backwardHelperPoint.isOutfoldedInInspector == false)
                {
                    return false;
                }
            }

            if (forwardHelperPoint.IsUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid())
            {
                //-> not foldable
            }
            else
            {
                if (forwardHelperPoint.isOutfoldedInInspector == false)
                {
                    return false;
                }
            }
            return true;
        }

        public bool CheckIf_foldableHelperPoints_areCollapsed_inTheInspectorList()
        {
            if (backwardHelperPoint.IsUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid())
            {
                //-> not foldable
            }
            else
            {
                if (backwardHelperPoint.isOutfoldedInInspector)
                {
                    return false;
                }
            }

            if (forwardHelperPoint.IsUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid())
            {
                //-> not foldable
            }
            else
            {
                if (forwardHelperPoint.isOutfoldedInInspector)
                {
                    return false;
                }
            }
            return true;
        }

        public void UnfoldBothHelperPointInTheInspectorList()
        {
            backwardHelperPoint.isOutfoldedInInspector = true;
            forwardHelperPoint.isOutfoldedInInspector = true;
        }

        public void CollapseBothHelperPointInTheInspectorList()
        {
            backwardHelperPoint.isOutfoldedInInspector = false;
            forwardHelperPoint.isOutfoldedInInspector = false;
        }

        public Vector2 GetPosAtPlusButton_onUpcomingBezierSegment_inUnitsOfGlobalSpace()
        {
            return GetAPos_onUpcomingBezierSegment_inUnitsOfGlobalSpace(progress0to1_ofPlusButtonPosition_inUpcomingBezierSegment);
        }

        public Vector2 GetAPos_onUpcomingBezierSegment_inUnitsOfGlobalSpace(float progress0to1_inUpcomingBezierSegment)
        {
            InternalDXXL_BezierControlPointTriplet2D nextControlPointTriplet = GetNextControlPointTripletAlongSplineDir(true);
            if (nextControlPointTriplet == null)
            {
                return anchorPoint.GetPos_inUnitsOfGlobalSpace();
            }
            else
            {
                Vector2 segmentStartPos = anchorPoint.GetPos_inUnitsOfGlobalSpace();
                Vector2 segmentEndPos = nextControlPointTriplet.anchorPoint.GetPos_inUnitsOfGlobalSpace();
                if (forwardHelperPoint.isUsed == true)
                {
                    Vector2 firstControlPosInBetween = forwardHelperPoint.GetPos_inUnitsOfGlobalSpace();
                    if (nextControlPointTriplet.backwardHelperPoint.isUsed == true)
                    {
                        Vector2 secondControlPosInBetween = nextControlPointTriplet.backwardHelperPoint.GetPos_inUnitsOfGlobalSpace();
                        return GetPos_onCubicBezierSegment(progress0to1_inUpcomingBezierSegment, segmentStartPos, firstControlPosInBetween, secondControlPosInBetween, segmentEndPos);
                    }
                    else
                    {
                        return GetPos_onQuadraticBezierSegment(progress0to1_inUpcomingBezierSegment, segmentStartPos, firstControlPosInBetween, segmentEndPos);
                    }
                }
                else
                {
                    if (nextControlPointTriplet.backwardHelperPoint.isUsed == true)
                    {
                        Vector2 theSingleControlPosInBetween = nextControlPointTriplet.backwardHelperPoint.GetPos_inUnitsOfGlobalSpace();
                        return GetPos_onQuadraticBezierSegment(progress0to1_inUpcomingBezierSegment, segmentStartPos, theSingleControlPosInBetween, segmentEndPos);
                    }
                    else
                    {
                        //straight line:
                        Vector2 fromStart_toEnd = segmentEndPos - segmentStartPos;
                        return (segmentStartPos + fromStart_toEnd * progress0to1_inUpcomingBezierSegment);
                    }
                }
            }
        }

        Vector2 GetPos_onQuadraticBezierSegment(float progress_0to1, Vector2 segmentStartPos, Vector2 controlPosInBetween, Vector2 segmentEndPos)
        {
            float oneMinusProgress0to1 = 1.0f - progress_0to1;
            float factor1 = oneMinusProgress0to1 * oneMinusProgress0to1;
            float factor2 = 2.0f * oneMinusProgress0to1 * progress_0to1;
            float factor3 = progress_0to1 * progress_0to1;
            return (factor1 * segmentStartPos + factor2 * controlPosInBetween + factor3 * segmentEndPos);
        }

        Vector2 GetPos_onCubicBezierSegment(float progress_0to1, Vector2 segmentStartPos, Vector2 firstControlPosInBetween, Vector2 secondControlPosInBetween, Vector2 segmentEndPos)
        {
            float progress_0to1_sqr = progress_0to1 * progress_0to1;
            float oneMinusProgress0to1 = 1.0f - progress_0to1;
            float oneMinusProgress0to1_sqr = oneMinusProgress0to1 * oneMinusProgress0to1;
            float factor1 = oneMinusProgress0to1 * oneMinusProgress0to1_sqr;
            float factor2 = 3.0f * oneMinusProgress0to1_sqr * progress_0to1;
            float factor3 = 3.0f * oneMinusProgress0to1 * progress_0to1_sqr;
            float factor4 = progress_0to1 * progress_0to1_sqr;
            return (factor1 * segmentStartPos + factor2 * firstControlPosInBetween + factor3 * secondControlPosInBetween + factor4 * segmentEndPos);
        }

        public Vector2 GetTangentAtPlusButton_onUpcomingBezierSegment_inUnitsOfGlobalSpace(bool normalized)
        {
            return GetATangent_onUpcomingBezierSegment_inUnitsOfGlobalSpace(progress0to1_ofPlusButtonPosition_inUpcomingBezierSegment, normalized);
        }

        public Vector2 GetATangent_onUpcomingBezierSegment_inUnitsOfGlobalSpace(float progress0to1_inUpcomingBezierSegment, bool normalized)
        {
            InternalDXXL_BezierControlPointTriplet2D nextControlPointTriplet = GetNextControlPointTripletAlongSplineDir(true);
            if (nextControlPointTriplet == null)
            {
                return bezierSplineDrawer_thisPointIsPartOf.Get_forwardTangent_ofControlPointThatDoesntKnowOfANextOne_inUnitsOfGlobalSpace_normalized(this);
            }
            else
            {
                Vector2 tangent_notNormalized;

                Vector2 segmentStartPos = anchorPoint.GetPos_inUnitsOfGlobalSpace();
                Vector2 segmentEndPos = nextControlPointTriplet.anchorPoint.GetPos_inUnitsOfGlobalSpace();
                if (forwardHelperPoint.isUsed == true)
                {
                    Vector2 firstControlPosInBetween = forwardHelperPoint.GetPos_inUnitsOfGlobalSpace();
                    if (nextControlPointTriplet.backwardHelperPoint.isUsed == true)
                    {
                        Vector2 secondControlPosInBetween = nextControlPointTriplet.backwardHelperPoint.GetPos_inUnitsOfGlobalSpace();
                        tangent_notNormalized = GetTangent_onCubicBezierSegment(progress0to1_inUpcomingBezierSegment, segmentStartPos, firstControlPosInBetween, secondControlPosInBetween, segmentEndPos);
                    }
                    else
                    {
                        tangent_notNormalized = GetTangent_onQuadraticBezierSegment(progress0to1_inUpcomingBezierSegment, segmentStartPos, firstControlPosInBetween, segmentEndPos);
                    }
                }
                else
                {
                    if (nextControlPointTriplet.backwardHelperPoint.isUsed == true)
                    {
                        Vector2 theSingleControlPosInBetween = nextControlPointTriplet.backwardHelperPoint.GetPos_inUnitsOfGlobalSpace();
                        tangent_notNormalized = GetTangent_onQuadraticBezierSegment(progress0to1_inUpcomingBezierSegment, segmentStartPos, theSingleControlPosInBetween, segmentEndPos);
                    }
                    else
                    {
                        //straight line:
                        Vector2 fromStart_toEnd = segmentEndPos - segmentStartPos;
                        if (UtilitiesDXXL_Math.ApproximatelyZero(fromStart_toEnd))
                        {
                            return bezierSplineDrawer_thisPointIsPartOf.Get_forwardTangent_ofControlPointThatDoesntKnowOfANextOne_inUnitsOfGlobalSpace_normalized(this);
                        }
                        else
                        {
                            tangent_notNormalized = fromStart_toEnd;
                        }
                    }
                }

                if (normalized)
                {
                    Vector2 tangent_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(tangent_notNormalized);
                    if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(tangent_normalized))
                    {
                        return bezierSplineDrawer_thisPointIsPartOf.Get_right_ofActiveDrawSpace_inUnitsOfGlobalSpace_normalized();
                    }
                    else
                    {
                        return tangent_normalized;
                    }
                }
                else
                {
                    return tangent_notNormalized;
                }
            }
        }

        Vector2 GetTangent_onQuadraticBezierSegment(float progress_0to1, Vector2 segmentStartPos, Vector2 controlPosInBetween, Vector2 segmentEndPos)
        {
            return (2.0f * (1.0f - progress_0to1) * (controlPosInBetween - segmentStartPos) + 2.0f * progress_0to1 * (segmentEndPos - controlPosInBetween));
        }

        Vector2 GetTangent_onCubicBezierSegment(float progress_0to1, Vector2 segmentStartPos, Vector2 firstControlPosInBetween, Vector2 secondControlPosInBetween, Vector2 segmentEndPos)
        {
            float progress_0to1_sqr = progress_0to1 * progress_0to1;
            float oneMinusProgress0to1 = 1.0f - progress_0to1;
            return (3.0f * oneMinusProgress0to1 * oneMinusProgress0to1 * (firstControlPosInBetween - segmentStartPos) + (6.0f * oneMinusProgress0to1 * progress_0to1) * (secondControlPosInBetween - firstControlPosInBetween) + 3.0f * progress_0to1_sqr * (segmentEndPos - secondControlPosInBetween));
        }

        public void Invert_alignedHelperPoints_areOnTheSameSideOfTheAnchor()
        {
            alignedHelperPoints_areOnTheSameSideOfTheAnchor = !alignedHelperPoints_areOnTheSameSideOfTheAnchor;
        }

        public void DrawValuesToInspector()
        {
            TryHighlightThisControlPoint_dueToMouseClickOnItInInspector();
            DrawBackgroundArea_forInspector();
            DrawSubPoints_forInspector();
        }

        void TryHighlightThisControlPoint_dueToMouseClickOnItInInspector()
        {
            Event currentEvent = Event.current;
            if (currentEvent.type == EventType.MouseDown)
            {
                if (inspectorRect_reservedForThisTriplet.Contains(currentEvent.mousePosition))
                {
                    bezierSplineDrawer_thisPointIsPartOf.SetSelectedListSlot(i_ofThisPoint_insideControlPointsList);
                    bezierSplineDrawer_thisPointIsPartOf.SheduleSceneViewRepaint();
                }
            }
        }

        void DrawBackgroundArea_forInspector()
        {
            Rect space_ofBigBackgroundColorRect = new Rect(Get_xPos_ofBackgroundRect(inspectorRect_reservedForThisTriplet), Get_yPos_ofBackgroundRect(inspectorRect_reservedForThisTriplet), Get_width_ofBackgroundRect(inspectorRect_reservedForThisTriplet), inspectorRect_reservedForThisTriplet.height - 2.0f * Get_inspectorVertSpace_from_mainRect_to_backgroundColorBox() - Get_inspectorVertSpace_ofMinusButtonOnEachControlPoint());
            float alphaFactor_ofBackgroundColor = isHighlighted ? InternalDXXL_BezierControlPointTriplet.alpha_ofInspectorBackgroundColor_highlighted : InternalDXXL_BezierControlPointTriplet.alpha_ofInspectorBackgroundColor_nonHighlighted;
            Color backgroundColor = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(bezierSplineDrawer_thisPointIsPartOf.color_ofAnchorPoints, alphaFactor_ofBackgroundColor);

            DrawBackgroundColor_forInspector(space_ofBigBackgroundColorRect, backgroundColor);

            //See also "Get_xPos_ofPlusAndMinusButtonRect()":
            //float y_of_minusButton = position.y + position.height - inspectorVertSpace_from_mainRect_to_backgroundColorBox - inspectorVertSpace_ofMinusButtonOnEachControlPoint; //-> float calculation imprecision errors seems to introduce overlap offsets of 1 pixel when calculated like this
            //float y_of_minusButton = space_ofBigBackgroundColorRect.y + space_ofBigBackgroundColorRect.height; //-> this also is not solving the 1 pixel overlap offset
            //float y_of_minusButton = space_ofBigBackgroundColorRect.yMax; //-> this also is not solving the 1 pixel overlap offset
            float y_of_minusButton = space_ofBigBackgroundColorRect.yMax + 1.0f; //-> fixing it manually (with the risk of adding an empty pixel line if there are situations where the problem doesn't occur)

            DrawMinusButton_forInspector(backgroundColor, y_of_minusButton);
            DrawIndexNumber_forInspector(backgroundColor, y_of_minusButton);
        }

        void DrawBackgroundColor_forInspector(Rect space_ofBigBackgroundColorRect, Color backgroundColor)
        {
#if UNITY_EDITOR
            UnityEditor.EditorGUI.DrawRect(space_ofBigBackgroundColorRect, backgroundColor);
#endif
        }

        void DrawMinusButton_forInspector(Color backgroundColor, float y_of_minusButton)
        {
#if UNITY_EDITOR
            Rect space_ofMinusButtonBackground = new Rect(Get_xPos_ofPlusAndMinusButtonRect(inspectorRect_reservedForThisTriplet), y_of_minusButton, Get_width_ofPlusAndMinusButtons(), Get_inspectorVertSpace_ofMinusButtonOnEachControlPoint());
            UnityEditor.EditorGUI.DrawRect(space_ofMinusButtonBackground, backgroundColor);

            GUIContent minusSymbolIcon = UnityEditor.EditorGUIUtility.TrIconContent("Toolbar Minus", "Delete control point");
            GUIStyle style_ofMinusButton = "RL FooterButton";
            minusButtonAtThisListItem_hasBeenClickedInInspector = GUI.Button(space_ofMinusButtonBackground, minusSymbolIcon, style_ofMinusButton);
#endif
        }

        void DrawIndexNumber_forInspector(Color backgroundColor, float y_of_minusButton)
        {
#if UNITY_EDITOR
            float horizSpace_betweenSlotIndexNumber_and_minusButton = 0.5f * UnityEditor.EditorGUIUtility.singleLineHeight;
            float horizPadding_besideIndexNumberInsideNumberRect_perSide = 0.5f * UnityEditor.EditorGUIUtility.singleLineHeight;
            float horizPadding_besideIndexNumberInsideNumberRect_forBothSides = 2.0f * horizPadding_besideIndexNumberInsideNumberRect_perSide;
            float width_perNumberDigit = 0.6f * UnityEditor.EditorGUIUtility.singleLineHeight;
            float width_ofSlotIndexNumberRect = horizPadding_besideIndexNumberInsideNumberRect_forBothSides + width_perNumberDigit * GetNumberOfDigitsInIndexNumber();
            Rect space_ofSlotIndexNumberBackground = new Rect(inspectorRect_reservedForThisTriplet.x + inspectorRect_reservedForThisTriplet.width - Get_inspectorHorizSpace_from_mainRect_to_backgroundColorBox() - Get_width_ofPlusAndMinusButtons() - horizSpace_betweenSlotIndexNumber_and_minusButton - width_ofSlotIndexNumberRect, y_of_minusButton, width_ofSlotIndexNumberRect, Get_inspectorVertSpace_ofMinusButtonOnEachControlPoint());
            UnityEditor.EditorGUI.DrawRect(space_ofSlotIndexNumberBackground, backgroundColor);

            GUIStyle style_ofNumber = new GUIStyle();
            Color color_ofNumber;
            if (isHighlighted)
            {
                color_ofNumber = UtilitiesDXXL_Colors.GetSimilarColorWithOtherBrightnessValue(bezierSplineDrawer_thisPointIsPartOf.color_ofAnchorPoints, 0.375f);
            }
            else
            {
                color_ofNumber = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(bezierSplineDrawer_thisPointIsPartOf.color_ofAnchorPoints, 0.725f);
            }

            string textString_ofNumber = "<size=16><color=#" + ColorUtility.ToHtmlStringRGBA(color_ofNumber) + "><b>" + i_ofThisPoint_insideControlPointsList + "</b></color></size>";
            float y_offset_ofSlotIndexNumber = -0.1f * UnityEditor.EditorGUIUtility.singleLineHeight;
            Rect space_ofSlotIndexNumber = new Rect(space_ofSlotIndexNumberBackground.x + horizPadding_besideIndexNumberInsideNumberRect_perSide, space_ofSlotIndexNumberBackground.y + y_offset_ofSlotIndexNumber, space_ofSlotIndexNumberBackground.width - horizPadding_besideIndexNumberInsideNumberRect_forBothSides, space_ofSlotIndexNumberBackground.height);
            UnityEditor.EditorGUI.LabelField(space_ofSlotIndexNumber, textString_ofNumber, style_ofNumber);
#endif
        }

        int GetNumberOfDigitsInIndexNumber()
        {
            if (i_ofThisPoint_insideControlPointsList >= 10000)
            {
                return 5;
            }
            else
            {
                if (i_ofThisPoint_insideControlPointsList >= 1000)
                {
                    return 4;
                }
                else
                {
                    if (i_ofThisPoint_insideControlPointsList >= 100)
                    {
                        return 3;
                    }
                    else
                    {
                        if (i_ofThisPoint_insideControlPointsList >= 10)
                        {
                            return 2;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                }
            }
        }

        void DrawSubPoints_forInspector()
        {
            float height_ofBackwardHelper = backwardHelperPoint.GetPropertyHeightForInspectorList();
            float height_ofAnchor = anchorPoint.GetPropertyHeightForInspectorList();
            float height_ofForwardHelper = forwardHelperPoint.GetPropertyHeightForInspectorList();

            float x_ofHelperPointsColorBox = inspectorRect_reservedForThisTriplet.x + Get_inspectorHorizSpace_from_mainRect_to_backgroundColorBox() + Get_inspectorSpace_fromOutsideColorBoxEdge_toInsideColorBoxEdge();
            float width_ofHelperPointsColorBox = inspectorRect_reservedForThisTriplet.width - 2.0f * Get_inspectorHorizSpace_from_mainRect_to_backgroundColorBox() - 2.0f * Get_inspectorSpace_fromOutsideColorBoxEdge_toInsideColorBoxEdge();

            float currentHeightOffset = 0.0f;
            currentHeightOffset += Get_inspectorVertSpace_from_mainRect_to_backgroundColorBox();
            currentHeightOffset += Get_inspectorSpace_fromOutsideColorBoxEdge_toInsideColorBoxEdge();

            Rect rect_of_backwardHelperPoint = new Rect(x_ofHelperPointsColorBox, inspectorRect_reservedForThisTriplet.y + currentHeightOffset, width_ofHelperPointsColorBox, height_ofBackwardHelper);

            currentHeightOffset += height_ofBackwardHelper;
            currentHeightOffset += Get_inspectorVertSpace_betweenSubPoints();

            float x_ofAnchorPointsSubProperty = inspectorRect_reservedForThisTriplet.x + Get_inspectorHorizSpace_from_mainRect_to_backgroundColorBox() + Get_inspectorSpace_fromOutsideColorBoxEdge_toInsideColorBoxEdge() + InternalDXXL_BezierControlHelperSubPoint.Get_inspectorHorizSpace_between_subPointColorBoxEdge_and_actualContenFields();
            float width_ofAnchorPointsSubProperty = inspectorRect_reservedForThisTriplet.width - 2.0f * Get_inspectorHorizSpace_from_mainRect_to_backgroundColorBox() - 2.0f * Get_inspectorSpace_fromOutsideColorBoxEdge_toInsideColorBoxEdge() - 2.0f * InternalDXXL_BezierControlHelperSubPoint.Get_inspectorHorizSpace_between_subPointColorBoxEdge_and_actualContenFields();
            Rect rect_of_anchorPoint = new Rect(x_ofAnchorPointsSubProperty, inspectorRect_reservedForThisTriplet.y + currentHeightOffset, width_ofAnchorPointsSubProperty, height_ofAnchor);

            currentHeightOffset += height_ofAnchor;
            currentHeightOffset += Get_inspectorVertSpace_betweenSubPoints();

            Rect rect_of_forwardHelperPoint = new Rect(x_ofHelperPointsColorBox, inspectorRect_reservedForThisTriplet.y + currentHeightOffset, width_ofHelperPointsColorBox, height_ofForwardHelper);

            backwardHelperPoint.DrawValuesToInspector(rect_of_backwardHelperPoint);
            anchorPoint.DrawValuesToInspector(rect_of_anchorPoint);
            forwardHelperPoint.DrawValuesToInspector(rect_of_forwardHelperPoint);
        }

        public bool TryApplyChangesAfterInspectorInput()
        {
            bool didChangeSomething;

            didChangeSomething = backwardHelperPoint.TryApplyChangesAfterInspectorInput();
            if (didChangeSomething) { return true; }

            didChangeSomething = anchorPoint.TryApplyChangesAfterInspectorInput();
            if (didChangeSomething) { return true; }

            didChangeSomething = forwardHelperPoint.TryApplyChangesAfterInspectorInput();
            if (didChangeSomething) { return true; }

            return false;
        }

        public float GetPropertyHeightForInspectorList()
        {
            float height_ofBackwardHelper = backwardHelperPoint.GetPropertyHeightForInspectorList();
            float height_ofAnchor = anchorPoint.GetPropertyHeightForInspectorList();
            float height_ofForwardHelper = forwardHelperPoint.GetPropertyHeightForInspectorList();
            float additionalHeight = Get_inspectorVertSpace_from_mainRect_to_backgroundColorBox() + Get_inspectorSpace_fromOutsideColorBoxEdge_toInsideColorBoxEdge() + Get_inspectorVertSpace_betweenSubPoints() + Get_inspectorVertSpace_betweenSubPoints() + Get_inspectorSpace_fromOutsideColorBoxEdge_toInsideColorBoxEdge() + Get_inspectorVertSpace_ofMinusButtonOnEachControlPoint() + Get_inspectorVertSpace_from_mainRect_to_backgroundColorBox();

            return (height_ofBackwardHelper + height_ofAnchor + height_ofForwardHelper + additionalHeight);
        }

        public static void DrawEmptyControlPointHoldingOnlyAPlusButton_forInspector(out bool plusButtonBelowListOfControlPoints_hasBeenClicked, out bool unfoldAllWeightsBelowListOfControlPoints_hasBeenClicked, out bool collapseAllWeightsBelowListOfControlPoints_hasBeenClicked, Rect position, Color color_ofAnchorPoints, GUIContent plusSymbolIcon, bool greyOutUnfoldAllButton, bool greyOutCollapseAllButton)
        {
#if UNITY_EDITOR
            Color backgroundColor = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofAnchorPoints, InternalDXXL_BezierControlPointTriplet.alpha_ofInspectorBackgroundColor_nonHighlighted);

            Rect rect_ofEmptyLine = new Rect(Get_xPos_ofBackgroundRect(position), Get_yPos_ofBackgroundRect(position), Get_width_ofBackgroundRect(position), Get_inspectorVertSpace_ofEmptyLineOfEmptyControlPointHoldingOnlyAPlusButton());
            UnityEditor.EditorGUI.DrawRect(rect_ofEmptyLine, backgroundColor);

            Rect rect_ofPlusButtonBelowEmptyLine = new Rect(Get_xPos_ofPlusAndMinusButtonRect(position), position.y + Get_inspectorVertSpace_from_mainRect_to_backgroundColorBox() + Get_inspectorVertSpace_ofEmptyLineOfEmptyControlPointHoldingOnlyAPlusButton(), Get_width_ofPlusAndMinusButtons(), Get_inspectorVertSpace_ofMinusButtonOnEachControlPoint());
            UnityEditor.EditorGUI.DrawRect(rect_ofPlusButtonBelowEmptyLine, backgroundColor);

            GUIStyle style_ofMinusButton = "RL FooterButton";
            plusButtonBelowListOfControlPoints_hasBeenClicked = GUI.Button(rect_ofPlusButtonBelowEmptyLine, plusSymbolIcon, style_ofMinusButton);

            Rect rect_ofBothFoldAllButtons = new Rect(Get_xPos_ofBackgroundRect(position), position.y + Get_inspectorVertSpace_from_mainRect_to_backgroundColorBox() + Get_inspectorVertSpace_ofEmptyLineOfEmptyControlPointHoldingOnlyAPlusButton() + Get_inspectorVertSpace_ofMinusButtonOnEachControlPoint() + Get_additionalInspectorVertSpace_belowEmptyPlusLine_tillFoldAllButtons(), Get_width_ofBackgroundRect(position), Get_inspectorVertSpace_forFoldAllButtons());

            float halfHorizSpaceBetweenButtons = 0.5f * Get_inspectorHorizSpace_from_mainRect_to_backgroundColorBox();
            Rect rect_ofUnfoldAllButton = new Rect(rect_ofBothFoldAllButtons.x, rect_ofBothFoldAllButtons.y, rect_ofBothFoldAllButtons.width * 0.5f - halfHorizSpaceBetweenButtons, rect_ofBothFoldAllButtons.height);
            Rect rect_ofCollapseAllButton = new Rect(rect_ofBothFoldAllButtons.x + rect_ofBothFoldAllButtons.width * 0.5f + halfHorizSpaceBetweenButtons, rect_ofBothFoldAllButtons.y, rect_ofBothFoldAllButtons.width * 0.5f - halfHorizSpaceBetweenButtons, rect_ofBothFoldAllButtons.height);

            GUIStyle style_ofUnfoldAllButtons = new GUIStyle(style_ofMinusButton);
            style_ofUnfoldAllButtons.fontStyle = FontStyle.Normal;
            Color color_ofAGreyedOut_foldAllButton = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(backgroundColor, 0.5f);

            Color color_ofUnfoldAllButton = greyOutUnfoldAllButton ? color_ofAGreyedOut_foldAllButton : backgroundColor;
            UnityEditor.EditorGUI.DrawRect(rect_ofUnfoldAllButton, color_ofUnfoldAllButton);
            UnityEditor.EditorGUI.BeginDisabledGroup(greyOutUnfoldAllButton);
            unfoldAllWeightsBelowListOfControlPoints_hasBeenClicked = GUI.Button(rect_ofUnfoldAllButton, "Unfold all weights", style_ofUnfoldAllButtons);
            UnityEditor.EditorGUI.EndDisabledGroup();

            Color color_ofCollapseAllButton = greyOutCollapseAllButton ? color_ofAGreyedOut_foldAllButton : backgroundColor;
            UnityEditor.EditorGUI.DrawRect(rect_ofCollapseAllButton, color_ofCollapseAllButton);
            UnityEditor.EditorGUI.BeginDisabledGroup(greyOutCollapseAllButton);
            collapseAllWeightsBelowListOfControlPoints_hasBeenClicked = GUI.Button(rect_ofCollapseAllButton, "Collapse all weights", style_ofUnfoldAllButtons);
            UnityEditor.EditorGUI.EndDisabledGroup();
#else
            plusButtonBelowListOfControlPoints_hasBeenClicked = false;
            unfoldAllWeightsBelowListOfControlPoints_hasBeenClicked = false;
            collapseAllWeightsBelowListOfControlPoints_hasBeenClicked = false;
#endif
        }

        public static float GetPropertyHeightForEmptyControlPointHoldingOnlyAPlusButtonAndFoldAllButtons()
        {
            return (Get_inspectorVertSpace_from_mainRect_to_backgroundColorBox() + Get_inspectorVertSpace_ofEmptyLineOfEmptyControlPointHoldingOnlyAPlusButton() + Get_inspectorVertSpace_ofMinusButtonOnEachControlPoint() + Get_inspectorVertSpace_from_mainRect_to_backgroundColorBox() + Get_additionalInspectorVertSpace_belowEmptyPlusLine_tillFoldAllButtons() + Get_inspectorVertSpace_forFoldAllButtons() + Get_emptyInspectorVertSpace_belowFoldAllButtons());
        }

        static float Get_xPos_ofBackgroundRect(Rect inspectorRect_reservedForThisTriplet)
        {
            return (inspectorRect_reservedForThisTriplet.x + Get_inspectorHorizSpace_from_mainRect_to_backgroundColorBox());
        }

        static float Get_yPos_ofBackgroundRect(Rect inspectorRect_reservedForThisTriplet)
        {
            return (inspectorRect_reservedForThisTriplet.y + Get_inspectorVertSpace_from_mainRect_to_backgroundColorBox());
        }

        static float Get_width_ofBackgroundRect(Rect inspectorRect_reservedForThisTriplet)
        {
            return (inspectorRect_reservedForThisTriplet.width - 2.0f * Get_inspectorHorizSpace_from_mainRect_to_backgroundColorBox());
        }

        static float Get_xPos_ofPlusAndMinusButtonRect(Rect inspectorRect_reservedForThisTriplet)
        {
            //-> similar problem as described in "DrawBackgroundArea()", where the yPos is shifted by 1 pixel.
            //-> in this case the x-position is shifted by 1 pixel to the left.
            float offsetForFixingTheOnePixelOffset = 1.0f; //-> fixing it manually (with the risk of introducing a horizontal one pixel offset if there are situations where the problem doesn't occur)
            return (inspectorRect_reservedForThisTriplet.x + inspectorRect_reservedForThisTriplet.width - Get_inspectorHorizSpace_from_mainRect_to_backgroundColorBox() - Get_width_ofPlusAndMinusButtons() + offsetForFixingTheOnePixelOffset);
        }

        static float Get_inspectorSpace_fromOutsideColorBoxEdge_toInsideColorBoxEdge()
        {
#if UNITY_EDITOR
            return (0.2f * UnityEditor.EditorGUIUtility.singleLineHeight);
#else
            return 16.0f; //-> not used
#endif
        }

        static float Get_inspectorVertSpace_from_mainRect_to_backgroundColorBox()
        {
#if UNITY_EDITOR
            return (0.2f * UnityEditor.EditorGUIUtility.singleLineHeight);
#else
            return 16.0f; //-> not used
#endif
        }

        static float Get_inspectorVertSpace_betweenSubPoints()
        {
#if UNITY_EDITOR
            return (2.0f * Get_inspectorSpace_fromOutsideColorBoxEdge_toInsideColorBoxEdge());
#else
            return 16.0f; //-> not used
#endif
        }

        static float Get_inspectorVertSpace_ofMinusButtonOnEachControlPoint()
        {
#if UNITY_EDITOR
            return (UnityEditor.EditorGUIUtility.singleLineHeight);
#else
            return 16.0f; //-> not used
#endif
        }

        static float Get_inspectorVertSpace_ofEmptyLineOfEmptyControlPointHoldingOnlyAPlusButton()
        {
#if UNITY_EDITOR
            return (UnityEditor.EditorGUIUtility.singleLineHeight);
#else
            return 16.0f; //-> not used
#endif
        }

        static float Get_additionalInspectorVertSpace_belowEmptyPlusLine_tillFoldAllButtons()
        {
#if UNITY_EDITOR
            return (0.5f * UnityEditor.EditorGUIUtility.singleLineHeight);
#else
            return 16.0f; //-> not used
#endif
        }

        static float Get_inspectorVertSpace_forFoldAllButtons()
        {
#if UNITY_EDITOR
            return (UnityEditor.EditorGUIUtility.singleLineHeight);
#else
            return 16.0f; //-> not used
#endif
        }

        static float Get_emptyInspectorVertSpace_belowFoldAllButtons()
        {
#if UNITY_EDITOR
            return (0.0f * UnityEditor.EditorGUIUtility.singleLineHeight);
#else
            return 16.0f; //-> not used
#endif
        }

        static float Get_width_ofPlusAndMinusButtons()
        {
#if UNITY_EDITOR
            return (1.5f * UnityEditor.EditorGUIUtility.singleLineHeight);
#else
            return 16.0f; //-> not used
#endif
        }

        static float Get_inspectorHorizSpace_from_mainRect_to_backgroundColorBox()
        {
#if UNITY_EDITOR
            return (0.2f * UnityEditor.EditorGUIUtility.singleLineHeight);
#else
            return 16.0f; //-> not used
#endif
        }

    }

}
