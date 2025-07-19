namespace DrawXXL
{
    using System;
    using UnityEngine;

    [Serializable]
    public class InternalDXXL_BezierControlAnchorSubPoint2D : InternalDXXL_BezierControlSubPoint2D
    {
        [SerializeField] public InternalDXXL_BezierControlAnchorSubPoint.JunctureType junctureType;
        public enum SourceOf_directionToHelper2D { independentFromGameobject, gameobjectsUp, gameobjectsRight, gameobjectsDown, gameobjectsLeft };
        public enum SourceOf_directionToHelper2D_wordedForInspectorDisplayAtHelpers { directionFromPointCenterToThisWeightIsIndependentFromTheRotationOfTheGameobjectThatIsBoundToTheCenterPosition, directionFromPointCenterToThisWeightIsTheUpDirectionOfTheGameobjectThatIsBoundToTheCenterPosition, directionFromPointCenterToThisWeightIsTheRightDirectionOfTheGameobjectThatIsBoundToTheCenterPosition, directionFromPointCenterToThisWeightIsTheDownDirectionOfTheGameobjectThatIsBoundToTheCenterPosition, directionFromPointCenterToThisWeightIsTheLeftDirectionOfTheGameobjectThatIsBoundToTheCenterPosition };
        [SerializeField] public SourceOf_directionToHelper2D sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked; //-> is only used when a "boundGameobject" is assigned

        public int controlID_ofCustomHandles_sphere;
        public int controlID_ofCustomHandles_forwardCone;
        public int controlID_ofCustomHandles_backwardCone;

        public int controlID_ofUnityStyleRotationHandle2D;

        public Quaternion rotation_ofRotationHandleDuringRotationDragPhases;
        public bool recalc_rotation_ofRotationHandleDuringRotationDragPhases_duringNextOnSceneGUI = true;
        public Vector3 directionForHandles_forwardCone_inUnitsOfGlobalSpace_normalized = Vector3.right;
        public Vector3 directionForHandles_backwardCone_inUnitsOfGlobalSpace_normalized = Vector3.left;
        public bool recalc_directionForHandles_forwardCone_duringNextOnSceneGUI = true;
        public bool recalc_directionForHandles_backwardCone_duringNextOnSceneGUI = true;

        public override void InitializeValuesThatAreIndependentFromOtherSubPoints(InternalDXXL_BezierControlPointTriplet2D controlPoint_thisSubPointIsPartOf)
        {
            base.InitializeValuesThatAreIndependentFromOtherSubPoints(controlPoint_thisSubPointIsPartOf);
            sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked = SourceOf_directionToHelper2D.independentFromGameobject;
        }

        public InternalDXXL_BezierControlHelperSubPoint2D GetForwardHelperPoint()
        {
            //Serialized lists containing other lists and containing custom classes with cross references don't work well in Unity, even when using the [SerializeReference] attribute. Functions like this try to keep the convenience of a real reference tree. The only "real" reference is the monobehavior-inherited spline component. Everything has to start from there.
            return bezierSplineDrawer_thisSubPointIsPartOf.listOfControlPointTriplets[i_ofContainingControlPoint_insideControlPointsList].forwardHelperPoint;
        }

        public InternalDXXL_BezierControlHelperSubPoint2D GetBackwardHelperPoint()
        {
            //Serialized lists containing other lists and containing custom classes with cross references don't work well in Unity, even when using the [SerializeReference] attribute. Functions like this try to keep the convenience of a real reference tree. The only "real" reference is the monobehavior-inherited spline component. Everything has to start from there.
            return bezierSplineDrawer_thisSubPointIsPartOf.listOfControlPointTriplets[i_ofContainingControlPoint_insideControlPointsList].backwardHelperPoint;
        }

        public override void ResetDirectionSourceToIndependent()
        {
            sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked = SourceOf_directionToHelper2D.independentFromGameobject;
        }

        public override void TryTransferBoundGameobjectsRotationToAnchorPointsDirection()
        {
            //-> during no-gameobject-assigned-phases the "sourceOf_direction*'s" are forced to "independentFromGameobject", so here only cases where the assignment changed from one gameobject to another gameobject cause an action inside "connectionComponent_onBoundGameobject.Transfer_aTransformDirection_fromBoundGameobject_toSpline()":
            connectionComponent_onBoundGameobject.Transfer_aTransformDirection_fromBoundGameobject_toSpline();
        }

        public void ProcessChanging_sourceOfDirectionToHelper(SourceOf_directionToHelper2D newSourceOfDirection)
        {
            if (newSourceOfDirection != sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked)
            {
                sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked = newSourceOfDirection;
                if (newSourceOfDirection != SourceOf_directionToHelper2D.independentFromGameobject)
                {
                    TryTransfer_newTransformDirection_fromBoundGameobject_toSpline_afterDirectionSourceChange();
                }
            }
        }

        public void TryTransfer_newTransformDirection_fromBoundGameobject_toSpline_afterDirectionSourceChange()
        {
            SetBoolOf_boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled();
            if (boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled)
            {
                connectionComponent_onBoundGameobject.Transfer_aTransformDirection_fromBoundGameobject_toSpline();
            }
        }

        public void SetJunctureType(InternalDXXL_BezierControlAnchorSubPoint.JunctureType newJunctureType)
        {
            if (newJunctureType != junctureType)
            {
                InternalDXXL_BezierControlAnchorSubPoint.JunctureType oldJunctureType = junctureType;
                junctureType = newJunctureType;

                if (oldJunctureType == InternalDXXL_BezierControlAnchorSubPoint.JunctureType.kinked)
                {
                    TrySwitchBothHelpersTo_isUsed();
                    //<- if at least one helperPoint was unused before, then the helperPoints are now already cleanly converted to the new juncture type (meaning: they are now "both used", "parallel", "same absDistance from anchor" and "on differnt sides of the anchor")
                    //-> if both helperPoints were already used before, then the above didn't have any effect, and the following "forcing to parallel/sameAbsDistance" (inside the "newJuncture == nonKinked"-threads) finishes the conversion. This "forcing to parallel/sameAbsDistance" doesn't harm if the two helpers are already "parallel/sameAbsDistance".
                }

                if (newJunctureType == InternalDXXL_BezierControlAnchorSubPoint.JunctureType.kinked)
                {
                    //this "set isUsed-states" is actually only needed for "change AWAY from kinked", and then during non-kinked-phases the isUsed-state will anyway not change so it will arrive at the next "change TO kinked" still with "all are used". But in order to not having to care what happens in non-kinked-phases and still being sure that kinked-phases always start with "all are used" it is explicitly called here. Besides that it may act as double bottom if the states still get confused somehow.
                    TrySwitchBothHelpersTo_isUsed();
                }

                if (newJunctureType == InternalDXXL_BezierControlAnchorSubPoint.JunctureType.aligned)
                {
                    MirrorDirection_from_toForward_to_toBackward();
                    Get_controlPointTriplet_thisSubPointIsPartOf().alignedHelperPoints_areOnTheSameSideOfTheAnchor = false; //-> this field is only used by "aligned" anchorPoints
                }

                if (newJunctureType == InternalDXXL_BezierControlAnchorSubPoint.JunctureType.mirrored)
                {
                    MirrorDirection_from_toForward_to_toBackward();
                    MirrorDistance_from_toForward_to_toBackward();
                }
            }
        }

        void TrySwitchBothHelpersTo_isUsed()
        {
            //the order of the helperPointActivation here only matters for cases where BOTH helperPoints have been unused during the kinked-phase and are now set to used, because a non-kinked juncture-phase follows. The first activated helperPoint then dictates the shape of the second activated helperPoint. Therefore "forward" comes first.
            if (false == GetForwardHelperPoint().IsUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid())
            {
                GetForwardHelperPoint().ChangeUsedState(true, false);
            }

            if (false == GetBackwardHelperPoint().IsUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid())
            {
                GetBackwardHelperPoint().ChangeUsedState(true, false);
            }
        }

        void MirrorDirection_from_toForward_to_toBackward()
        {
            GetBackwardHelperPoint().Set_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized(-GetForwardHelperPoint().Get_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized(), true, null);
        }

        void MirrorDistance_from_toForward_to_toBackward()
        {
            GetBackwardHelperPoint().Set_absDistanceToAnchorPoint_inUnitsOfGlobalSpace(GetForwardHelperPoint().Get_absDistanceToAnchorPoint_inUnitsOfGlobalSpace(), true, null);
        }

        public Vector2 Get_aDirection_inUnitsOfGlobalSpace_normalized(bool requestedDirection_isForward_notBackward)
        {
            if (requestedDirection_isForward_notBackward)
            {
                return Get_direction_toForward_inUnitsOfGlobalSpace_normalized();
            }
            else
            {
                return Get_direction_toBackward_inUnitsOfGlobalSpace_normalized();
            }
        }

        public Vector2 Get_aDirection_inUnitsOfActiveDrawSpace_normalized(bool requestedDirection_isForward_notBackward)
        {
            if (requestedDirection_isForward_notBackward)
            {
                return Get_direction_toForward_inUnitsOfActiveDrawSpace_normalized();
            }
            else
            {
                return Get_direction_toBackward_inUnitsOfActiveDrawSpace_normalized();
            }
        }

        public Vector2 Get_direction_toForward_inUnitsOfGlobalSpace_normalized()
        {
            return GetForwardHelperPoint().Get_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized();
        }

        public Vector2 Get_direction_toForward_inUnitsOfActiveDrawSpace_normalized()
        {
            return GetForwardHelperPoint().Get_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfActiveDrawSpace_normalized();
        }

        public Vector2 Get_direction_toBackward_inUnitsOfGlobalSpace_normalized()
        {
            return GetBackwardHelperPoint().Get_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized();
        }

        public Vector2 Get_direction_toBackward_inUnitsOfActiveDrawSpace_normalized()
        {
            return GetBackwardHelperPoint().Get_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfActiveDrawSpace_normalized();
        }

        public void Set_aDirection_inUnitsOfGlobalSpace_normalized(bool requestedDirection_isForward_notBackward, Vector2 newDirection_inUnitsOfGlobalSpace_normalized, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            if (requestedDirection_isForward_notBackward)
            {
                Set_direction_toForward_inUnitsOfGlobalSpace_normalized(newDirection_inUnitsOfGlobalSpace_normalized, updateDependentValuesOnControlPointTriplet, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
            }
            else
            {
                Set_direction_toBackward_inUnitsOfGlobalSpace_normalized(newDirection_inUnitsOfGlobalSpace_normalized, updateDependentValuesOnControlPointTriplet, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
            }
        }

        public void Set_aDirection_inUnitsOfActiveDrawSpace_normalized(bool requestedDirection_isForward_notBackward, Vector2 newDirection_inUnitsOfActiveDrawSpace_normalized, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            if (requestedDirection_isForward_notBackward)
            {
                Set_direction_toForward_inUnitsOfActiveDrawSpace_normalized(newDirection_inUnitsOfActiveDrawSpace_normalized, updateDependentValuesOnControlPointTriplet, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
            }
            else
            {
                Set_direction_toBackward_inUnitsOfActiveDrawSpace_normalized(newDirection_inUnitsOfActiveDrawSpace_normalized, updateDependentValuesOnControlPointTriplet, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
            }
        }

        public override InternalDXXL_BezierControlAnchorSubPoint.JunctureType GetJunctureType()
        {
            return junctureType;
        }

        public override InternalDXXL_BezierControlHelperSubPoint2D GetForwardHelper()
        {
            return GetForwardHelperPoint();
        }

        public override InternalDXXL_BezierControlHelperSubPoint2D GetBackwardHelper()
        {
            return GetBackwardHelperPoint();
        }

        public override SourceOf_directionToHelper2D Get_sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked()
        {
            return sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked;
        }

        public bool CheckIf_boundGameobjectInfluencesRotation()
        {
            SetBoolOf_boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled();
            if (boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled)
            {
                if (junctureType == InternalDXXL_BezierControlAnchorSubPoint.JunctureType.kinked)
                {
                    if (GetBackwardHelperPoint().isUsed == true)
                    {
                        if (GetBackwardHelperPoint().sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture != SourceOf_directionToHelper2D.independentFromGameobject)
                        {
                            return true;
                        }
                    }

                    if (GetForwardHelperPoint().isUsed == true)
                    {
                        if (GetForwardHelperPoint().sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture != SourceOf_directionToHelper2D.independentFromGameobject)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    return (sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked != SourceOf_directionToHelper2D.independentFromGameobject);
                }
            }
            return false;
        }

        public override void Set_direction_toForward_inUnitsOfGlobalSpace_normalized(Vector2 newDirection_toForward_inUnitsOfGlobalSpace_normalized, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            GetForwardHelperPoint().Set_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized(newDirection_toForward_inUnitsOfGlobalSpace_normalized, updateDependentValuesOnControlPointTriplet, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
        }

        public void Set_direction_toForward_inUnitsOfActiveDrawSpace_normalized(Vector2 newDirection_toForward_inUnitsOfActiveDrawSpace_normalized, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            GetForwardHelperPoint().Set_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfActiveDrawSpace_normalized(newDirection_toForward_inUnitsOfActiveDrawSpace_normalized, updateDependentValuesOnControlPointTriplet, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
        }

        public override void Set_direction_toBackward_inUnitsOfGlobalSpace_normalized(Vector2 newDirection_toBackward_inUnitsOfGlobalSpace_normalized, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            GetBackwardHelperPoint().Set_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized(newDirection_toBackward_inUnitsOfGlobalSpace_normalized, updateDependentValuesOnControlPointTriplet, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
        }

        public void Set_direction_toBackward_inUnitsOfActiveDrawSpace_normalized(Vector2 newDirection_toBackward_inUnitsOfActiveDrawSpace_normalized, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            GetBackwardHelperPoint().Set_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfActiveDrawSpace_normalized(newDirection_toBackward_inUnitsOfActiveDrawSpace_normalized, updateDependentValuesOnControlPointTriplet, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
        }

        public void TryPassOnNew_directionToHelper_unifiedTowardsForwardForCaseNonKinked_inUnitsOfGlobalSpace_normalized_toBoundGameobject(Vector2 newDirection_unifiedToForward_inUnitsOfGlobalSpace_normalized, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            if (sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked != SourceOf_directionToHelper2D.independentFromGameobject)
            {
                connectionComponent_onBoundGameobject.Transfer_newDirectionToAHelperPointInUnitsOfGlobalSpaceNormalized_fromSpline_toBoundGameobject(newDirection_unifiedToForward_inUnitsOfGlobalSpace_normalized, sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
            }
        }

        public Vector2 ConvertGivenDirectionToHelper_to_alingedDirectionToOtherHelper(Vector2 givenDirectionToHelper)
        {
            if ((junctureType == InternalDXXL_BezierControlAnchorSubPoint.JunctureType.aligned) && Get_controlPointTriplet_thisSubPointIsPartOf().alignedHelperPoints_areOnTheSameSideOfTheAnchor)
            {
                return givenDirectionToHelper;
            }
            else
            {
                return (-givenDirectionToHelper);
            }
        }

        public void AddRotation_toForwardDirection(Quaternion rotationIncrement, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            Vector3 new_direction_toForward_inUnitsOfGlobalSpace_normalized = rotationIncrement * Get_direction_toForward_inUnitsOfGlobalSpace_normalized();
            Set_direction_toForward_inUnitsOfGlobalSpace_normalized(new_direction_toForward_inUnitsOfGlobalSpace_normalized, updateDependentValuesOnControlPointTriplet, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
        }

        public void AddRotation_toBackwardDirection(Quaternion rotationIncrement, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            Vector3 new_direction_toBackward_inUnitsOfGlobalSpace_normalized = rotationIncrement * Get_direction_toBackward_inUnitsOfGlobalSpace_normalized();
            Set_direction_toBackward_inUnitsOfGlobalSpace_normalized(new_direction_toBackward_inUnitsOfGlobalSpace_normalized, updateDependentValuesOnControlPointTriplet, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
        }

        public override void SetPos_inUnitsOfGlobalSpace(Vector2 newPos_inUnitsOfGlobalSpace, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            Vector2 offset_fromPrevious_toNewPosition = SetPos_inUnitsOfGlobalSpace_butIgnoreDependentValues_nonRecursively(newPos_inUnitsOfGlobalSpace, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
            if (updateDependentValuesOnControlPointTriplet)
            {
                if ((junctureType == InternalDXXL_BezierControlAnchorSubPoint.JunctureType.kinked) && (Get_controlPointTriplet_thisSubPointIsPartOf().IsEndPointToVoid_atStartOrEndOfAnUnclosedSpline() == false))
                {
                    if (GetForwardHelperPoint().isUsed == true)
                    {
                        if (GetForwardHelperPoint().sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture == SourceOf_directionToHelper2D.independentFromGameobject)
                        {
                            Vector2 newDirection_fromAnchorPoint_toForwardHelper_inUnitsOfGlobalSpace_notNormalized = GetForwardHelperPoint().GetPos_inUnitsOfGlobalSpace() - newPos_inUnitsOfGlobalSpace;
                            float newAbsDistance_toForwardHelper_inUnitsOfGlobalSpace = newDirection_fromAnchorPoint_toForwardHelper_inUnitsOfGlobalSpace_notNormalized.magnitude;
                            GetForwardHelperPoint().Set_absDistanceToAnchorPoint_inUnitsOfGlobalSpace(newAbsDistance_toForwardHelper_inUnitsOfGlobalSpace, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);

                            if (UtilitiesDXXL_Math.ApproximatelyZero(newAbsDistance_toForwardHelper_inUnitsOfGlobalSpace) == false) //-> no change of the direction for zero-distances
                            {
                                Vector2 newDirection_fromAnchorPoint_toForwardHelper_inUnitsOfGlobalSpace_normalized = newDirection_fromAnchorPoint_toForwardHelper_inUnitsOfGlobalSpace_notNormalized / newAbsDistance_toForwardHelper_inUnitsOfGlobalSpace;
                                Set_direction_toForward_inUnitsOfGlobalSpace_normalized(newDirection_fromAnchorPoint_toForwardHelper_inUnitsOfGlobalSpace_normalized, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
                            }
                        }
                        else
                        {
                            GetForwardHelperPoint().AddPosOffset_inUnitsOfGlobalSpace(offset_fromPrevious_toNewPosition, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
                        }
                    }

                    if (GetBackwardHelperPoint().isUsed == true)
                    {
                        if (GetBackwardHelperPoint().sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture == SourceOf_directionToHelper2D.independentFromGameobject)
                        {
                            Vector2 newDirection_fromAnchorPoint_toBackwardHelper_inUnitsOfGlobalSpace_notNormalized = GetBackwardHelperPoint().GetPos_inUnitsOfGlobalSpace() - newPos_inUnitsOfGlobalSpace;
                            float newAbsDistance_toBackwardHelper_inUnitsOfGlobalSpace = newDirection_fromAnchorPoint_toBackwardHelper_inUnitsOfGlobalSpace_notNormalized.magnitude;
                            GetBackwardHelperPoint().Set_absDistanceToAnchorPoint_inUnitsOfGlobalSpace(newAbsDistance_toBackwardHelper_inUnitsOfGlobalSpace, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);

                            if (UtilitiesDXXL_Math.ApproximatelyZero(newAbsDistance_toBackwardHelper_inUnitsOfGlobalSpace) == false) //-> no change of the direction for zero-distances
                            {
                                Vector2 newDirection_fromAnchorPoint_toBackwardHelper_inUnitsOfGlobalSpace_normalized = newDirection_fromAnchorPoint_toBackwardHelper_inUnitsOfGlobalSpace_notNormalized / newAbsDistance_toBackwardHelper_inUnitsOfGlobalSpace;
                                Set_direction_toBackward_inUnitsOfGlobalSpace_normalized(newDirection_fromAnchorPoint_toBackwardHelper_inUnitsOfGlobalSpace_normalized, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
                            }
                        }
                        else
                        {
                            GetBackwardHelperPoint().AddPosOffset_inUnitsOfGlobalSpace(offset_fromPrevious_toNewPosition, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
                        }
                    }
                }
                else
                {
                    //-> the helperPoints have always "isUsed=true" here, since the junctureType is "not kinked". Reminder: endPoints of non-closed splines are always forced to "kinked" and therefore also don't arrive here
                    GetForwardHelperPoint().AddPosOffset_inUnitsOfGlobalSpace(offset_fromPrevious_toNewPosition, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
                    GetBackwardHelperPoint().AddPosOffset_inUnitsOfGlobalSpace(offset_fromPrevious_toNewPosition, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
                }
            }
        }

        public InternalDXXL_BezierControlHelperSubPoint2D GetAHelperPoint(bool requestedHelperPoint_isForward_notBackward)
        {
            if (requestedHelperPoint_isForward_notBackward)
            {
                return GetForwardHelperPoint();
            }
            else
            {
                return GetBackwardHelperPoint();
            }
        }

        public override InternalDXXL_BezierControlSubPoint2D GetNextSubPointAlongSplineDir(bool allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet)
        {
            return GetForwardHelperPoint();
        }

        public override InternalDXXL_BezierControlSubPoint2D GetPreviousSubPointAlongSplineDir(bool allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet)
        {
            return GetBackwardHelperPoint();
        }

        public override InternalDXXL_BezierControlSubPoint2D GetNextUsedSubPointAlongSplineDir(bool allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet)
        {
            if (GetForwardHelperPoint().isUsed)
            {
                return GetForwardHelperPoint();
            }
            else
            {
                InternalDXXL_BezierControlPointTriplet2D next_controlPoint = Get_controlPointTriplet_thisSubPointIsPartOf().GetNextControlPointTripletAlongSplineDir(allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet);
                if (next_controlPoint != null)
                {
                    if (next_controlPoint.backwardHelperPoint.isUsed)
                    {
                        return next_controlPoint.backwardHelperPoint;
                    }
                    else
                    {
                        return next_controlPoint.anchorPoint;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public override InternalDXXL_BezierControlSubPoint2D GetPreviousUsedSubPointAlongSplineDir(bool allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet)
        {
            if (GetBackwardHelperPoint().isUsed)
            {
                return GetBackwardHelperPoint();
            }
            else
            {
                InternalDXXL_BezierControlPointTriplet2D previous_controlPoint = Get_controlPointTriplet_thisSubPointIsPartOf().GetPreviousControlPointTripletAlongSplineDir(allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet);
                if (previous_controlPoint != null)
                {
                    if (previous_controlPoint.forwardHelperPoint.isUsed)
                    {
                        return previous_controlPoint.forwardHelperPoint;
                    }
                    else
                    {
                        return previous_controlPoint.anchorPoint;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public override Vector2 GetDirectionAlongSplineForwardBasedOnNeighborPoints_inUnitsOfGlobalSpace()
        {
            if (GetForwardHelperPoint().isUsed)
            {
                return Get_direction_toForward_inUnitsOfGlobalSpace_normalized();
            }
            else
            {
                if (GetBackwardHelperPoint().isUsed)
                {
                    return (-Get_direction_toBackward_inUnitsOfGlobalSpace_normalized());
                }
                else
                {
                    InternalDXXL_BezierControlSubPoint2D nextUsedNonSuperimposedSubPointAlongSplineDir = GetNextUsedNonSuperimposedSubPointAlongSplineDir(false);
                    if (nextUsedNonSuperimposedSubPointAlongSplineDir != null)
                    {
                        return (nextUsedNonSuperimposedSubPointAlongSplineDir.GetPos_inUnitsOfGlobalSpace() - GetPos_inUnitsOfGlobalSpace());
                    }
                    else
                    {
                        InternalDXXL_BezierControlSubPoint2D previousUsedNonSuperimposedSubPointAlongSplineDir = GetPreviousUsedNonSuperimposedSubPointAlongSplineDir(false);
                        if (previousUsedNonSuperimposedSubPointAlongSplineDir != null)
                        {
                            return (GetPos_inUnitsOfGlobalSpace() - previousUsedNonSuperimposedSubPointAlongSplineDir.GetPos_inUnitsOfGlobalSpace());
                        }
                        else
                        {
                            return bezierSplineDrawer_thisSubPointIsPartOf.Get_right_ofActiveDrawSpace_inUnitsOfGlobalSpace_normalized();
                        }
                    }
                }
            }
        }

        public override bool IsUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid()
        {
            return false;
        }

        public static SourceOf_directionToHelper2D ConvertDirectionSource_fromWordedVersion_toUsedVersion(SourceOf_directionToHelper2D_wordedForInspectorDisplayAtHelpers wordedVersion_toConvert)
        {
            switch (wordedVersion_toConvert)
            {
                case SourceOf_directionToHelper2D_wordedForInspectorDisplayAtHelpers.directionFromPointCenterToThisWeightIsIndependentFromTheRotationOfTheGameobjectThatIsBoundToTheCenterPosition:
                    return SourceOf_directionToHelper2D.independentFromGameobject;
                case SourceOf_directionToHelper2D_wordedForInspectorDisplayAtHelpers.directionFromPointCenterToThisWeightIsTheUpDirectionOfTheGameobjectThatIsBoundToTheCenterPosition:
                    return SourceOf_directionToHelper2D.gameobjectsUp;
                case SourceOf_directionToHelper2D_wordedForInspectorDisplayAtHelpers.directionFromPointCenterToThisWeightIsTheRightDirectionOfTheGameobjectThatIsBoundToTheCenterPosition:
                    return SourceOf_directionToHelper2D.gameobjectsRight;
                case SourceOf_directionToHelper2D_wordedForInspectorDisplayAtHelpers.directionFromPointCenterToThisWeightIsTheDownDirectionOfTheGameobjectThatIsBoundToTheCenterPosition:
                    return SourceOf_directionToHelper2D.gameobjectsDown;
                case SourceOf_directionToHelper2D_wordedForInspectorDisplayAtHelpers.directionFromPointCenterToThisWeightIsTheLeftDirectionOfTheGameobjectThatIsBoundToTheCenterPosition:
                    return SourceOf_directionToHelper2D.gameobjectsLeft;
                default:
                    return SourceOf_directionToHelper2D.independentFromGameobject;
            }
        }

        public static SourceOf_directionToHelper2D_wordedForInspectorDisplayAtHelpers ConvertDirectionSource_fromUsedVersion_toWordedVersion(SourceOf_directionToHelper2D usedVersion_toConvert)
        {
            switch (usedVersion_toConvert)
            {
                case SourceOf_directionToHelper2D.independentFromGameobject:
                    return SourceOf_directionToHelper2D_wordedForInspectorDisplayAtHelpers.directionFromPointCenterToThisWeightIsIndependentFromTheRotationOfTheGameobjectThatIsBoundToTheCenterPosition;
                case SourceOf_directionToHelper2D.gameobjectsUp:
                    return SourceOf_directionToHelper2D_wordedForInspectorDisplayAtHelpers.directionFromPointCenterToThisWeightIsTheUpDirectionOfTheGameobjectThatIsBoundToTheCenterPosition;
                case SourceOf_directionToHelper2D.gameobjectsRight:
                    return SourceOf_directionToHelper2D_wordedForInspectorDisplayAtHelpers.directionFromPointCenterToThisWeightIsTheRightDirectionOfTheGameobjectThatIsBoundToTheCenterPosition;
                case SourceOf_directionToHelper2D.gameobjectsDown:
                    return SourceOf_directionToHelper2D_wordedForInspectorDisplayAtHelpers.directionFromPointCenterToThisWeightIsTheDownDirectionOfTheGameobjectThatIsBoundToTheCenterPosition;
                case SourceOf_directionToHelper2D.gameobjectsLeft:
                    return SourceOf_directionToHelper2D_wordedForInspectorDisplayAtHelpers.directionFromPointCenterToThisWeightIsTheLeftDirectionOfTheGameobjectThatIsBoundToTheCenterPosition;
                default:
                    return SourceOf_directionToHelper2D_wordedForInspectorDisplayAtHelpers.directionFromPointCenterToThisWeightIsIndependentFromTheRotationOfTheGameobjectThatIsBoundToTheCenterPosition;
            }
        }

        SourceOf_directionToHelper2D sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked_afterInspectorInput;
        InternalDXXL_BezierControlAnchorSubPoint.JunctureType junctureType_afterInspectorInput;

        public void DrawValuesToInspector(Rect rectForOnlyTheContentLines_alreadyClearedFromAnyColorBoxPaddings)
        {
            FillingIsUsedFieldsWithDefaultValues_forInspector();

            float currentHeightOffset = 0.0f;
            DrawPositionLine_forInspector(RecalcCurrentRect_forInspector(rectForOnlyTheContentLines_alreadyClearedFromAnyColorBoxPaddings, currentHeightOffset), new GUIContent("Position", Get_tooltip_explainingWheatherUnitsAreInGlobalOrInLocalSpace_forInspector()));
            currentHeightOffset += UtilitiesDXXL_Components.Get_inspector_singleLineHeightInclSpacingBetweenLines();
            DrawBoundGameobjectLine_forInspector(RecalcCurrentRect_forInspector(rectForOnlyTheContentLines_alreadyClearedFromAnyColorBoxPaddings, currentHeightOffset), new GUIContent("Bind to gameobject"));
            TryDrawDirectionSourceLine_forInspector(ref currentHeightOffset, rectForOnlyTheContentLines_alreadyClearedFromAnyColorBoxPaddings);
            currentHeightOffset += UtilitiesDXXL_Components.Get_inspector_singleLineHeightInclSpacingBetweenLines();
            DrawJunctureTypeLine_forInspector(rectForOnlyTheContentLines_alreadyClearedFromAnyColorBoxPaddings, currentHeightOffset);
        }

        void FillingIsUsedFieldsWithDefaultValues_forInspector()
        {
            //This is for cases where the fields are not displayed or greyed out. They are used for an "hasChanged"-check afterwards:
            position_inUnitsOfActiveDrawSpace_afterInspectorInput = position_inUnitsOfActiveDrawSpace;
            boundGameobject_afterInspectorInput = boundGameobject;
            sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked_afterInspectorInput = sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked;
            junctureType_afterInspectorInput = junctureType;
        }

        void TryDrawDirectionSourceLine_forInspector(ref float currentHeightOffset, Rect rectForOnlyTheContentLines_alreadyClearedFromAnyColorBoxPaddings)
        {
#if UNITY_EDITOR
            if (boundGameobject != null) //-> could be improved: there is no check whether the "boundGameobject" is "inactive" or the connection component on it is "disabled". Also "GetPropertyHeightForInspectorList()" may be affected by a fix.
            {
                if (junctureType != InternalDXXL_BezierControlAnchorSubPoint.JunctureType.kinked)
                {
                    currentHeightOffset += UtilitiesDXXL_Components.Get_inspector_singleLineHeightInclSpacingBetweenLines();
                    sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked_afterInspectorInput = (SourceOf_directionToHelper2D)UnityEditor.EditorGUI.EnumPopup(RecalcCurrentRect_forInspector(rectForOnlyTheContentLines_alreadyClearedFromAnyColorBoxPaddings, currentHeightOffset), new GUIContent("Direction Source"), sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked);
                }
            }
#endif
        }

        void DrawJunctureTypeLine_forInspector(Rect rectForOnlyTheContentLines_alreadyClearedFromAnyColorBoxPaddings, float currentHeightOffset)
        {
#if UNITY_EDITOR
            bool isEndPointToVoid_atStartOrEndOfAnUnclosedSpline = Get_controlPointTriplet_thisSubPointIsPartOf().IsEndPointToVoid_atStartOrEndOfAnUnclosedSpline();

            string tooltip_forJunctureType;
            if (isEndPointToVoid_atStartOrEndOfAnUnclosedSpline)
            {
                tooltip_forJunctureType = "Not adjustable at end points of non-ring splines.";
            }
            else
            {
                tooltip_forJunctureType = "";
            }

            UnityEditor.EditorGUI.BeginDisabledGroup(isEndPointToVoid_atStartOrEndOfAnUnclosedSpline); //note: end points of non-closed splines are always forced to "kinked" so that their helperPoints can be deactivated/activated.
            junctureType_afterInspectorInput = (InternalDXXL_BezierControlAnchorSubPoint.JunctureType)UnityEditor.EditorGUI.EnumPopup(RecalcCurrentRect_forInspector(rectForOnlyTheContentLines_alreadyClearedFromAnyColorBoxPaddings, currentHeightOffset), new GUIContent("Juncture type", tooltip_forJunctureType), junctureType);
            UnityEditor.EditorGUI.EndDisabledGroup();
#endif
        }

        public bool TryApplyChangesAfterInspectorInput()
        {
            //The checks here are more reliable than "EditorGUI.BeginChangeCheck/EndChangeCheck()", which also reports "change" only due to mouse selection, even when the value didn't change yet.

            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreExactlyEqual(position_inUnitsOfActiveDrawSpace_afterInspectorInput, position_inUnitsOfActiveDrawSpace) == false)
            {
                bezierSplineDrawer_thisSubPointIsPartOf.RegisterStateForUndo("Change Spline Position", true, true);
                SetPos_inUnitsOfActiveDrawSpace(position_inUnitsOfActiveDrawSpace_afterInspectorInput, true, null);
                return true;
            }

            if (boundGameobject_afterInspectorInput != boundGameobject)
            {
                //bezierSplineDrawer_thisSubPointIsPartOf.RegisterStateForUndo("Change Spline Gameobject Ref", true, true); //not needed here, because it is cared for inside "ProcessNewGameobjectAssignment"
                ProcessNewGameobjectAssignment(boundGameobject_afterInspectorInput);
                return true;
            }

            if (sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked_afterInspectorInput != sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked)
            {
                bezierSplineDrawer_thisSubPointIsPartOf.RegisterStateForUndo("Change Spline Direction Source", true, true);
                ProcessChanging_sourceOfDirectionToHelper(sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked_afterInspectorInput);
                return true;
            }

            if (junctureType_afterInspectorInput != junctureType)
            {
                bezierSplineDrawer_thisSubPointIsPartOf.RegisterStateForUndo("Change Spline Juncture", true, true);
                SetJunctureType(junctureType_afterInspectorInput);
                return true;
            }

            return false;
        }

        public override float GetPropertyHeightForInspectorList()
        {
            int linesForDirectionSource = ((boundGameobject != null) && (junctureType != InternalDXXL_BezierControlAnchorSubPoint.JunctureType.kinked)) ? 1 : 0;
            float height_forAllContentLines = (3.0f + linesForDirectionSource) * UtilitiesDXXL_Components.Get_inspector_singleLineHeightInclSpacingBetweenLines();
            return height_forAllContentLines;
        }

    }

}
