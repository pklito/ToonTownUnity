namespace DrawXXL
{
    using System;
    using UnityEngine;

    [Serializable]
    public class InternalDXXL_BezierControlHelperSubPoint2D : InternalDXXL_BezierControlSubPoint2D
    {
        [SerializeField] public bool isOutfoldedInInspector = false;
        [SerializeField] Vector2 direction_toThisHelper_fromAnchor_inUnitsOfGlobalSpace_normalized;
        [SerializeField] Vector2 direction_toThisHelper_fromAnchor_inUnitsOfActiveDrawSpace_normalized;
        [SerializeField] public InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture; //-> is only used when a "boundGameobject" is assigned at the mountingAnchor
        [SerializeField] public float absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace;
        [SerializeField] float absDistanceToAnchorPoint_inUnitsOfGlobalSpace;

        public bool isForward_notBackward;

        public int controlID_ofCustomHandles_sphere;
        public int controlID_ofCustomHandles_coneAlongLineWithAnchor;
        public int controlID_ofCustomHandles_coneAlongLineWithNeighborsHelper;
        public int controlID_ofCustomHandles_cylinderAlongLineWithAnchor;
        public int controlID_ofCustomHandles_cylinderAlongLineWithNeighborsHelper;

        public Vector2 directionForHandles_alongLineWithMountingAnchor_toForwardDirOfWholeSpline_inUnitsOfGlobalSpace_normalized = Vector2.right;
        public Vector2 directionForHandles_alongLineWithNeighboringHelperOfNeighboringControlPoint_toForwardDirOfWholeSpline_inUnitsOfGlobalSpace_normalized = Vector2.left;
        public bool recalc_directionForHandles_alongLineWithMountingAnchor_duringNextOnSceneGUI = true;
        public bool recalc_directionForHandles_alongLineWithNeighboringHelperOfNeighboringControlPoint_duringNextOnSceneGUI = true;
        [SerializeField] Vector2 positionOfAnchor_inMomentOfDeactivationOfThisHelper_inUnitsOfGlobalSpace;

        public override void InitializeValuesThatAreIndependentFromOtherSubPoints(InternalDXXL_BezierControlPointTriplet2D controlPoint_thisSubPointIsPartOf)
        {
            base.InitializeValuesThatAreIndependentFromOtherSubPoints(controlPoint_thisSubPointIsPartOf);
            sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture = InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.independentFromGameobject;
        }

        public InternalDXXL_BezierControlAnchorSubPoint2D GetMountingAnchorPoint()
        {
            //Serialized lists containing other lists and containing custom classes with cross references don't work well in Unity, even when using the [SerializeReference] attribute. Functions like this try to keep the convenience of a real reference tree. The only "real" reference is the monobehavior-inherited spline component. Everything has to start from there.
            return bezierSplineDrawer_thisSubPointIsPartOf.listOfControlPointTriplets[i_ofContainingControlPoint_insideControlPointsList].anchorPoint;
        }

        public InternalDXXL_BezierControlHelperSubPoint2D GetOppositeHelperPoint()
        {
            //Serialized lists containing other lists and containing custom classes with cross references don't work well in Unity, even when using the [SerializeReference] attribute. Functions like this try to keep the convenience of a real reference tree. The only "real" reference is the monobehavior-inherited spline component. Everything has to start from there.
            return bezierSplineDrawer_thisSubPointIsPartOf.listOfControlPointTriplets[i_ofContainingControlPoint_insideControlPointsList].GetAHelperPoint(!isForward_notBackward);
        }

        public void ChangeUsedState(bool newStateOf_isUsed, bool stateChangeComesFromUserInputViaInspectorsIsUsedBoolCheckboxOnKinkedTriplet)
        {
            if (newStateOf_isUsed != isUsed)
            {
                isUsed = newStateOf_isUsed;
                if (newStateOf_isUsed == true)
                {
                    ProcessActivation(stateChangeComesFromUserInputViaInspectorsIsUsedBoolCheckboxOnKinkedTriplet);
                }
                else
                {
                    ProcessDeactivation();
                }
            }
        }

        void ProcessActivation(bool stateChangeComesFromUserInputViaInspectorsIsUsedBoolCheckboxOnKinkedTriplet)
        {
            if (stateChangeComesFromUserInputViaInspectorsIsUsedBoolCheckboxOnKinkedTriplet)
            {
                //-> "junctureType == kinked" is guaranteed here
                if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(positionOfAnchor_inMomentOfDeactivationOfThisHelper_inUnitsOfGlobalSpace, GetMountingAnchorPoint().GetPos_inUnitsOfGlobalSpace()))
                {
                    SetInitialPos_viaRestoringValuesFromPreDeactivation();
                }
                else
                {
                    SetInitialPos_withoutKnowledgeOfPreDeactivationPos();
                }
            }
            else
            {
                SetInitialPos_withoutKnowledgeOfPreDeactivationPos();
            }
        }

        void ProcessDeactivation()
        {
            positionOfAnchor_inMomentOfDeactivationOfThisHelper_inUnitsOfGlobalSpace = GetMountingAnchorPoint().GetPos_inUnitsOfGlobalSpace();
        }

        void SetInitialPos_viaRestoringValuesFromPreDeactivation()
        {
            //This restoration allows:
            //-> ticking the used state in the inspector testwise off an on without losing the values
            //-> special moves like temporarily decactivating a helperPoint, so that only the other helperPoint is affected by the rotationHandle of the anchorPoint.

            //The only case where this happens is: "junctureType=kinked", and the helper has been manually deactivated during this "kinked"-phase
            //The condition that the mountingAnchor shouldn't have changed it's position since the deactivation has this effect:
            //-> If the spline shape changed a lot during the inactive-phase then the reactivation could cause irritation because the spline shape can then immediately be unintendedly warped and stretched across the whole scene. 
            //-> As a side effect the re-activated helper forgets his former state also if the helper only changed slightly. It would actually be a welcomed functionality that in such cases the helper would restore its former position, but it is sacrified for for the prevention of the aforementioned irration.

            this.SetBoolOf_boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled();
            GetMountingAnchorPoint().SetBoolOf_boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled();
            if (GetMountingAnchorPoint().boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled && (sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture != InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.independentFromGameobject))
            {
                //restoring the position based on the distance and direction:
                float newAbsDistance_inUnitsOfGlobalSpace;
                if (this.boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled)
                {
                    //the "thisHelper.boundGameobject" may have changed it's position during the unused-phase. But since "mountingAnchor.boundGameobject" dictates a "direction" only the DISTANCE from "thisHelper.boundGameobject" is used, not the POSITION:
                    newAbsDistance_inUnitsOfGlobalSpace = (GetMountingAnchorPoint().GetPos_inUnitsOfGlobalSpace() - this.GetPos_inUnitsOfGlobalSpace()).magnitude;
                }
                else
                {
                    newAbsDistance_inUnitsOfGlobalSpace = absDistanceToAnchorPoint_inUnitsOfGlobalSpace;
                }

                Set_absDistanceToAnchorPoint_inUnitsOfGlobalSpace(newAbsDistance_inUnitsOfGlobalSpace, true, this.boundGameobject); //-> "this.boundGameobject" can also be "null" here, which does not harm
                GetMountingAnchorPoint().connectionComponent_onBoundGameobject.Transfer_aTransformDirection_fromBoundGameobject_toSpline();
            }
            else
            {
                //restoring the direction and distance based on the position:
                if (this.boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled)
                {
                    connectionComponent_onBoundGameobject.Transfer_position_fromBoundGameobject_toSpline();
                }
                else
                {
                    //-> this doesn't change the position, but refreshes the dependent values ("direction" and "distance"):
                    SetPos_inUnitsOfGlobalSpace(GetPos_inUnitsOfGlobalSpace(), true, null); //-> this is actually not be necessary here, because the direction and distance cannot be different from what they were onSetUnused, since the anchorPos stayed the same. It acts here as double bottom if somehow the states got confused.
                }
            }
        }

        void SetInitialPos_withoutKnowledgeOfPreDeactivationPos()
        {
            Vector2 initialDirection_inUnitsOfGlobalSpace_normalized;
            float initialAbsDistance_inUnitsOfGlobalSpace;
            if (GetOppositeHelperPoint().isUsed == true)
            {
                initialDirection_inUnitsOfGlobalSpace_normalized = (-GetOppositeHelperPoint().Get_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized());
                if ((UtilitiesDXXL_Math.ApproximatelyZero(GetOppositeHelperPoint().Get_absDistanceToAnchorPoint_inUnitsOfGlobalSpace()) == false) || (GetMountingAnchorPoint().junctureType == InternalDXXL_BezierControlAnchorSubPoint.JunctureType.mirrored))
                {
                    initialAbsDistance_inUnitsOfGlobalSpace = GetOppositeHelperPoint().Get_absDistanceToAnchorPoint_inUnitsOfGlobalSpace();
                }
                else
                {
                    initialAbsDistance_inUnitsOfGlobalSpace = Get_initialAbsDistance_inUnitsOfGlobalSpace_fromGlobalSettingsForNewlyCreatedPoints();
                }
            }
            else
            {
                initialDirection_inUnitsOfGlobalSpace_normalized = Get_initialDirection_inUnitsOfGlobalSpace_normalized_caseNoInformationFromOppositeHelper();
                initialAbsDistance_inUnitsOfGlobalSpace = Get_initialAbsDistance_inUnitsOfGlobalSpace_fromGlobalSettingsForNewlyCreatedPoints();
            }
            SetInitialPos(initialDirection_inUnitsOfGlobalSpace_normalized, initialAbsDistance_inUnitsOfGlobalSpace);
        }

        void SetInitialPos(Vector2 initialDirection_inUnitsOfGlobalSpace_normalized, float initialAbsDistance_inUnitsOfGlobalSpace)
        {
            Vector2 initialPosition_inUnitsOfGlobalSpace = GetMountingAnchorPoint().GetPos_inUnitsOfGlobalSpace() + initialDirection_inUnitsOfGlobalSpace_normalized * initialAbsDistance_inUnitsOfGlobalSpace;
            SetPos_inUnitsOfGlobalSpace(initialPosition_inUnitsOfGlobalSpace, true, null);
        }

        float Get_initialAbsDistance_inUnitsOfGlobalSpace_fromGlobalSettingsForNewlyCreatedPoints()
        {
            if (isForward_notBackward)
            {
                return bezierSplineDrawer_thisSubPointIsPartOf.TransformLength_fromUnitsOfActiveDrawSpace_toGlobalSpace(bezierSplineDrawer_thisSubPointIsPartOf.forwardWeightDistance_ofNewlyCreatedPoints_inUnitsOfActiveDrawSpace);
            }
            else
            {
                return bezierSplineDrawer_thisSubPointIsPartOf.TransformLength_fromUnitsOfActiveDrawSpace_toGlobalSpace(bezierSplineDrawer_thisSubPointIsPartOf.backwardWeightDistance_ofNewlyCreatedPoints_inUnitsOfActiveDrawSpace);
            }
        }

        Vector2 Get_initialDirection_inUnitsOfGlobalSpace_normalized_caseNoInformationFromOppositeHelper()
        {
            InternalDXXL_BezierControlSubPoint2D nextUsedNonSuperimposed_subPoint;
            InternalDXXL_BezierControlSubPoint2D previousUsedNonSuperimposed_subPoint;
            if (isForward_notBackward)
            {
                nextUsedNonSuperimposed_subPoint = GetNextUsedNonSuperimposedSubPointAlongSplineDir(false);
                previousUsedNonSuperimposed_subPoint = GetOppositeHelperPoint().GetPreviousUsedNonSuperimposedSubPointAlongSplineDir(false);
            }
            else
            {
                nextUsedNonSuperimposed_subPoint = GetOppositeHelperPoint().GetNextUsedNonSuperimposedSubPointAlongSplineDir(false);
                previousUsedNonSuperimposed_subPoint = GetPreviousUsedNonSuperimposedSubPointAlongSplineDir(false);
            }

            if (nextUsedNonSuperimposed_subPoint == null)
            {
                return GetRightRespLeft_ofActiveDrawSpace_normalized();
            }
            else
            {
                if (previousUsedNonSuperimposed_subPoint == null)
                {
                    return GetRightRespLeft_ofActiveDrawSpace_normalized();
                }
                else
                {
                    if (Get_controlPointTriplet_thisSubPointIsPartOf().IsPartOfThisTriplet(nextUsedNonSuperimposed_subPoint))
                    {
                        return GetRightRespLeft_ofActiveDrawSpace_normalized();
                    }
                    else
                    {
                        if (Get_controlPointTriplet_thisSubPointIsPartOf().IsPartOfThisTriplet(previousUsedNonSuperimposed_subPoint))
                        {
                            return GetRightRespLeft_ofActiveDrawSpace_normalized();
                        }
                        else
                        {
                            Vector2 previousUsedSubpoint_to_nextUsedSubpoint_inUnitsOfGlobalSpace = nextUsedNonSuperimposed_subPoint.GetPos_inUnitsOfGlobalSpace() - previousUsedNonSuperimposed_subPoint.GetPos_inUnitsOfGlobalSpace();
                            if (UtilitiesDXXL_Math.GetBiggestAbsComponent(previousUsedSubpoint_to_nextUsedSubpoint_inUnitsOfGlobalSpace) < 0.0001f)
                            {
                                return GetRightRespLeft_ofActiveDrawSpace_normalized();
                            }
                            else
                            {
                                Vector2 previousUsedSubpoint_to_nextUsedSubpoint_inUnitsOfGlobalSpace_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(previousUsedSubpoint_to_nextUsedSubpoint_inUnitsOfGlobalSpace);
                                if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(previousUsedSubpoint_to_nextUsedSubpoint_inUnitsOfGlobalSpace_normalized))
                                {
                                    return GetRightRespLeft_ofActiveDrawSpace_normalized();
                                }
                                else
                                {
                                    return TryFlipVectorForBackwardHelpers(previousUsedSubpoint_to_nextUsedSubpoint_inUnitsOfGlobalSpace_normalized);
                                }
                            }
                        }
                    }
                }
            }
        }

        Vector2 GetRightRespLeft_ofActiveDrawSpace_normalized()
        {
            return TryFlipVectorForBackwardHelpers(bezierSplineDrawer_thisSubPointIsPartOf.Get_right_ofActiveDrawSpace_inUnitsOfGlobalSpace_normalized());
        }

        Vector2 TryFlipVectorForBackwardHelpers(Vector2 vectorToTryFlip)
        {
            if (isForward_notBackward)
            {
                return vectorToTryFlip;
            }
            else
            {
                return (-vectorToTryFlip);
            }
        }

        public override void ResetDirectionSourceToIndependent()
        {
            sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture = InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.independentFromGameobject;
        }

        public void ProcessChanging_sourceOfDirectionFromAnchor(InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D newSourceOfDirection)
        {
            if (newSourceOfDirection != sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture)
            {
                sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture = newSourceOfDirection;
                if (newSourceOfDirection != InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.independentFromGameobject)
                {
                    GetMountingAnchorPoint().TryTransfer_newTransformDirection_fromBoundGameobject_toSpline_afterDirectionSourceChange();
                }
            }
        }

        public float Get_absDistanceToAnchorPoint_inUnitsOfGlobalSpace()
        {
            return absDistanceToAnchorPoint_inUnitsOfGlobalSpace;
        }

        public float Get_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace()
        {
            return absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace;
        }

        public void Set_absDistanceToAnchorPoint_inUnitsOfGlobalSpace(float new_absDistanceToAnchorPoint_inUnitsOfGlobalSpace, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            if (UtilitiesDXXL_Math.FloatIsValid(new_absDistanceToAnchorPoint_inUnitsOfGlobalSpace))
            {
                new_absDistanceToAnchorPoint_inUnitsOfGlobalSpace = Mathf.Max(0.0f, new_absDistanceToAnchorPoint_inUnitsOfGlobalSpace);
                absDistanceToAnchorPoint_inUnitsOfGlobalSpace = new_absDistanceToAnchorPoint_inUnitsOfGlobalSpace;
                absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace = bezierSplineDrawer_thisSubPointIsPartOf.TransformLength_fromGlobalSpace_toUnitsOfActiveDrawSpace(new_absDistanceToAnchorPoint_inUnitsOfGlobalSpace);
                TryUpdateDependentValues_after_onSetAbsDistanceToAnchorPointInUnitsOfGlobalSpace(new_absDistanceToAnchorPoint_inUnitsOfGlobalSpace, updateDependentValuesOnControlPointTriplet, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
            }
        }

        void TryUpdateDependentValues_after_onSetAbsDistanceToAnchorPointInUnitsOfGlobalSpace(float new_absDistanceToAnchorPoint_inUnitsOfGlobalSpace, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            if (updateDependentValuesOnControlPointTriplet)
            {
                Vector2 vector_fromMountingAnchorPoint_toThisHelperPoint_inUnitsOfGlobalSpace = Get_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized() * new_absDistanceToAnchorPoint_inUnitsOfGlobalSpace;
                Vector2 newPosition_inUnitsOfGlobalSpace = GetMountingAnchorPoint().GetPos_inUnitsOfGlobalSpace() + vector_fromMountingAnchorPoint_toThisHelperPoint_inUnitsOfGlobalSpace;
                SetPos_inUnitsOfGlobalSpace(newPosition_inUnitsOfGlobalSpace, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);

                if (GetOppositeHelperPoint().isUsed)
                {
                    if (GetMountingAnchorPoint().junctureType == InternalDXXL_BezierControlAnchorSubPoint.JunctureType.mirrored)
                    {
                        GetOppositeHelperPoint().Set_absDistanceToAnchorPoint_inUnitsOfGlobalSpace(new_absDistanceToAnchorPoint_inUnitsOfGlobalSpace, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);

                        Vector2 newPosition_ofOppositeHelperPoint_inUnitsOfGlobalSpace = GetMountingAnchorPoint().GetPos_inUnitsOfGlobalSpace() - vector_fromMountingAnchorPoint_toThisHelperPoint_inUnitsOfGlobalSpace;
                        GetOppositeHelperPoint().SetPos_inUnitsOfGlobalSpace(newPosition_ofOppositeHelperPoint_inUnitsOfGlobalSpace, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
                    }
                }
            }
        }

        public void Set_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace(float new_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            if (UtilitiesDXXL_Math.FloatIsValid(new_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace))
            {
                new_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace = Mathf.Max(0.0f, new_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace);
                absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace = new_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace;
                absDistanceToAnchorPoint_inUnitsOfGlobalSpace = bezierSplineDrawer_thisSubPointIsPartOf.TransformLength_fromUnitsOfActiveDrawSpace_toGlobalSpace(new_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace);
                TryUpdateDependentValues_after_onSetAbsDistanceToAnchorPointInUnitsOfActiveDrawSpace(new_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace, updateDependentValuesOnControlPointTriplet, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
            }
        }

        void TryUpdateDependentValues_after_onSetAbsDistanceToAnchorPointInUnitsOfActiveDrawSpace(float new_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            if (updateDependentValuesOnControlPointTriplet)
            {
                Vector2 vector_fromMountingAnchorPoint_toThisHelperPoint_inUnitsOfActiveDrawSpace = Get_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfActiveDrawSpace_normalized() * new_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace;
                Vector2 newPosition_inUnitsOfActiveDrawSpace = GetMountingAnchorPoint().GetPos_inUnitsOfActiveDrawSpace() + vector_fromMountingAnchorPoint_toThisHelperPoint_inUnitsOfActiveDrawSpace;
                SetPos_inUnitsOfActiveDrawSpace(newPosition_inUnitsOfActiveDrawSpace, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);

                if (GetOppositeHelperPoint().isUsed)
                {
                    if (GetMountingAnchorPoint().junctureType == InternalDXXL_BezierControlAnchorSubPoint.JunctureType.mirrored)
                    {
                        GetOppositeHelperPoint().Set_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace(new_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);

                        Vector2 newPosition_ofOppositeHelperPoint_inUnitsOfActiveDrawSpace = GetMountingAnchorPoint().GetPos_inUnitsOfActiveDrawSpace() - vector_fromMountingAnchorPoint_toThisHelperPoint_inUnitsOfActiveDrawSpace;
                        GetOppositeHelperPoint().SetPos_inUnitsOfActiveDrawSpace(newPosition_ofOppositeHelperPoint_inUnitsOfActiveDrawSpace, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
                    }
                }
            }
        }

        public override void SetPos_inUnitsOfGlobalSpace(Vector2 newPos_inUnitsOfGlobalSpace, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            SetPos_inUnitsOfGlobalSpace_butIgnoreDependentValues_nonRecursively(newPos_inUnitsOfGlobalSpace, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);

            if (updateDependentValuesOnControlPointTriplet)
            {
                Vector2 newDirection_fromAnchorPoint_toThisHelperPoint_inUnitsOfGlobalSpace_notNormalized = newPos_inUnitsOfGlobalSpace - GetMountingAnchorPoint().GetPos_inUnitsOfGlobalSpace();
                float newAbsDistance_toAnchorPoint_inUnitsOfGlobalSpace = newDirection_fromAnchorPoint_toThisHelperPoint_inUnitsOfGlobalSpace_notNormalized.magnitude;
                Set_absDistanceToAnchorPoint_inUnitsOfGlobalSpace(newAbsDistance_toAnchorPoint_inUnitsOfGlobalSpace, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);

                Vector2 newDirection_fromAnchorPoint_toThisHelperPoint_inUnitsOfGlobalSpace_normalized;
                if (UtilitiesDXXL_Math.ApproximatelyZero(newAbsDistance_toAnchorPoint_inUnitsOfGlobalSpace))
                {
                    //-> no change of the direction:
                    newDirection_fromAnchorPoint_toThisHelperPoint_inUnitsOfGlobalSpace_normalized = GetMountingAnchorPoint().Get_aDirection_inUnitsOfGlobalSpace_normalized(isForward_notBackward);
                }
                else
                {
                    newDirection_fromAnchorPoint_toThisHelperPoint_inUnitsOfGlobalSpace_normalized = newDirection_fromAnchorPoint_toThisHelperPoint_inUnitsOfGlobalSpace_notNormalized / newAbsDistance_toAnchorPoint_inUnitsOfGlobalSpace;
                }
                Set_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized(newDirection_fromAnchorPoint_toThisHelperPoint_inUnitsOfGlobalSpace_normalized, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);

                if (GetOppositeHelperPoint().isUsed)
                {
                    if (GetMountingAnchorPoint().junctureType != InternalDXXL_BezierControlAnchorSubPoint.JunctureType.kinked)
                    {
                        Vector2 newDirection_fromAnchorPoint_toOtherHelperPoint_inUnitsOfGlobalSpace_normalized = GetMountingAnchorPoint().ConvertGivenDirectionToHelper_to_alingedDirectionToOtherHelper(newDirection_fromAnchorPoint_toThisHelperPoint_inUnitsOfGlobalSpace_normalized);
                        GetOppositeHelperPoint().Set_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized(newDirection_fromAnchorPoint_toOtherHelperPoint_inUnitsOfGlobalSpace_normalized, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);

                        if (GetMountingAnchorPoint().junctureType == InternalDXXL_BezierControlAnchorSubPoint.JunctureType.mirrored)
                        {
                            GetOppositeHelperPoint().Set_absDistanceToAnchorPoint_inUnitsOfGlobalSpace(newAbsDistance_toAnchorPoint_inUnitsOfGlobalSpace, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
                        }

                        Vector2 newPos_ofOppositeHelperPoint_inUnitsOfGlobalSpace = GetMountingAnchorPoint().GetPos_inUnitsOfGlobalSpace() + GetOppositeHelperPoint().Get_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized() * GetOppositeHelperPoint().Get_absDistanceToAnchorPoint_inUnitsOfGlobalSpace();
                        GetOppositeHelperPoint().SetPos_inUnitsOfGlobalSpace(newPos_ofOppositeHelperPoint_inUnitsOfGlobalSpace, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
                    }
                }
            }
        }

        public Vector2 Get_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized()
        {
            return direction_toThisHelper_fromAnchor_inUnitsOfGlobalSpace_normalized;
        }

        public Vector2 Get_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfActiveDrawSpace_normalized()
        {
            return direction_toThisHelper_fromAnchor_inUnitsOfActiveDrawSpace_normalized;
        }

        public void Set_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized(Vector2 newDirection_inUnitsOfGlobalSpace_normalized, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            Set_direction_toThisHelper_fromAnchor_inBothSpaces_butDefinedViaGlobalSpace_normalized(newDirection_inUnitsOfGlobalSpace_normalized);
            TryPassOnNew_direction_toThisHelper_inUnitsOfGlobalSpace_normalized_toBoundGameobject(newDirection_inUnitsOfGlobalSpace_normalized, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
            TryUpdateDependentValues_after_onSetDirectionToThisHelperInUnitsOfGlobalSpace(newDirection_inUnitsOfGlobalSpace_normalized, updateDependentValuesOnControlPointTriplet, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
        }

        void Set_direction_toThisHelper_fromAnchor_inBothSpaces_butDefinedViaGlobalSpace_normalized(Vector2 newDirection_inUnitsOfGlobalSpace_normalized)
        {
            direction_toThisHelper_fromAnchor_inUnitsOfGlobalSpace_normalized = newDirection_inUnitsOfGlobalSpace_normalized;
            direction_toThisHelper_fromAnchor_inUnitsOfActiveDrawSpace_normalized = bezierSplineDrawer_thisSubPointIsPartOf.TransformDirection_fromGlobalSpace_toUnitsOfActiveDrawSpace(newDirection_inUnitsOfGlobalSpace_normalized);

            //normalization because:
            //-> if the spline component carrying gameobject or a parent has a non-z-rotation, then the normalized return value of "TransformDirection_fromGlobalSpace_toUnitsOfActiveDrawSpace" has non-0 in the z-component. After the z-component is discarded the normalization is lost
            direction_toThisHelper_fromAnchor_inUnitsOfActiveDrawSpace_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(direction_toThisHelper_fromAnchor_inUnitsOfActiveDrawSpace_normalized);
            if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(direction_toThisHelper_fromAnchor_inUnitsOfActiveDrawSpace_normalized))
            {
                direction_toThisHelper_fromAnchor_inUnitsOfActiveDrawSpace_normalized = GetRightRespLeft_ofActiveDrawSpace_normalized();
            }

        }

        void TryUpdateDependentValues_after_onSetDirectionToThisHelperInUnitsOfGlobalSpace(Vector2 newDirection_inUnitsOfGlobalSpace_normalized, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            if (updateDependentValuesOnControlPointTriplet)
            {
                Vector2 newPos_inUnitsOfGlobalSpace = GetMountingAnchorPoint().GetPos_inUnitsOfGlobalSpace() + newDirection_inUnitsOfGlobalSpace_normalized * Get_absDistanceToAnchorPoint_inUnitsOfGlobalSpace();
                SetPos_inUnitsOfGlobalSpace(newPos_inUnitsOfGlobalSpace, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);

                if (GetOppositeHelperPoint().isUsed)
                {
                    if (GetMountingAnchorPoint().junctureType != InternalDXXL_BezierControlAnchorSubPoint.JunctureType.kinked)
                    {
                        Vector2 newDirection_toOppositeHelper_inUnitsOfGlobalSpace_normalized = ConvertGivenDirectionToHelper_to_alingedDirectionToOtherHelper(newDirection_inUnitsOfGlobalSpace_normalized);
                        GetOppositeHelperPoint().Set_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized(newDirection_toOppositeHelper_inUnitsOfGlobalSpace_normalized, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);

                        Vector2 newPosOfOppositeHelper_inUnitsOfGlobalSpace = GetMountingAnchorPoint().GetPos_inUnitsOfGlobalSpace() + newDirection_toOppositeHelper_inUnitsOfGlobalSpace_normalized * GetOppositeHelperPoint().Get_absDistanceToAnchorPoint_inUnitsOfGlobalSpace();
                        GetOppositeHelperPoint().SetPos_inUnitsOfGlobalSpace(newPosOfOppositeHelper_inUnitsOfGlobalSpace, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
                    }
                }
            }
        }

        public void Set_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfActiveDrawSpace_normalized(Vector2 newDirection_inUnitsOfActiveDrawSpace_normalized, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            Set_direction_toThisHelper_fromAnchor_inBothSpaces_butDefinedViaActiveDrawSpace_normalized(newDirection_inUnitsOfActiveDrawSpace_normalized);
            //"direction_toThisHelper_fromAnchor_inUnitsOfGlobalSpace_normalized" has been set right before inside "Set_direction_toThisHelper_fromAnchor_inBothSpaces_butDefinedViaActiveDrawSpace_normalized()" and therefore is guaranteed available here
            TryPassOnNew_direction_toThisHelper_inUnitsOfGlobalSpace_normalized_toBoundGameobject(direction_toThisHelper_fromAnchor_inUnitsOfGlobalSpace_normalized, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
            TryUpdateDependentValues_after_onSetDirectionToThisHelperInUnitsOfActiveDrawSpace(newDirection_inUnitsOfActiveDrawSpace_normalized, updateDependentValuesOnControlPointTriplet, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
        }

        void Set_direction_toThisHelper_fromAnchor_inBothSpaces_butDefinedViaActiveDrawSpace_normalized(Vector2 newDirection_inUnitsOfActiveDrawSpace_normalized)
        {
            direction_toThisHelper_fromAnchor_inUnitsOfActiveDrawSpace_normalized = newDirection_inUnitsOfActiveDrawSpace_normalized;
            direction_toThisHelper_fromAnchor_inUnitsOfGlobalSpace_normalized = bezierSplineDrawer_thisSubPointIsPartOf.TransformDirection_fromUnitsOfActiveDrawSpace_toGlobalSpace(newDirection_inUnitsOfActiveDrawSpace_normalized);

            //normalization because:
            //-> if the spline component carrying gameobject or a parent has a non-z-rotation, then the normalized return value of "TransformDirection_fromUnitsOfActiveDrawSpace_toGlobalSpace" has non-0 in the z-component. After the z-component is discarded the normalization is lost
            direction_toThisHelper_fromAnchor_inUnitsOfGlobalSpace_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(direction_toThisHelper_fromAnchor_inUnitsOfGlobalSpace_normalized);
            if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(direction_toThisHelper_fromAnchor_inUnitsOfGlobalSpace_normalized))
            {
                direction_toThisHelper_fromAnchor_inUnitsOfGlobalSpace_normalized = GetRightRespLeft_ofActiveDrawSpace_normalized();
            }
        }

        public void TryUpdateDependentValues_after_onSetDirectionToThisHelperInUnitsOfActiveDrawSpace(Vector2 newDirection_inUnitsOfActiveDrawSpace_normalized, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            if (updateDependentValuesOnControlPointTriplet)
            {
                Vector2 newPos_inUnitsOfActiveDrawSpace = GetMountingAnchorPoint().GetPos_inUnitsOfActiveDrawSpace() + newDirection_inUnitsOfActiveDrawSpace_normalized * Get_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace();
                SetPos_inUnitsOfActiveDrawSpace(newPos_inUnitsOfActiveDrawSpace, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);

                if (GetOppositeHelperPoint().isUsed)
                {
                    if (GetMountingAnchorPoint().junctureType != InternalDXXL_BezierControlAnchorSubPoint.JunctureType.kinked)
                    {
                        Vector2 newDirection_toOppositeHelper_inUnitsOfActiveDrawSpace_normalized = ConvertGivenDirectionToHelper_to_alingedDirectionToOtherHelper(newDirection_inUnitsOfActiveDrawSpace_normalized);
                        GetOppositeHelperPoint().Set_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfActiveDrawSpace_normalized(newDirection_toOppositeHelper_inUnitsOfActiveDrawSpace_normalized, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);

                        Vector2 newPosOfOppositeHelper_inUnitsOfActiveDrawSpace = GetMountingAnchorPoint().GetPos_inUnitsOfActiveDrawSpace() + newDirection_toOppositeHelper_inUnitsOfActiveDrawSpace_normalized * GetOppositeHelperPoint().Get_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace();
                        GetOppositeHelperPoint().SetPos_inUnitsOfActiveDrawSpace(newPosOfOppositeHelper_inUnitsOfActiveDrawSpace, false, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
                    }
                }
            }
        }

        public void TryPassOnNew_direction_toThisHelper_inUnitsOfGlobalSpace_normalized_toBoundGameobject(Vector2 newDirection_inUnitsOfGlobalSpace_normalized, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            GetMountingAnchorPoint().SetBoolOf_boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled();
            if (GetMountingAnchorPoint().boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled)
            {
                if (GetMountingAnchorPoint().junctureType == InternalDXXL_BezierControlAnchorSubPoint.JunctureType.kinked)
                {
                    if (sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture != InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.independentFromGameobject)
                    {
                        GetMountingAnchorPoint().connectionComponent_onBoundGameobject.Transfer_newDirectionToAHelperPointInUnitsOfGlobalSpaceNormalized_fromSpline_toBoundGameobject(newDirection_inUnitsOfGlobalSpace_normalized, sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
                    }
                }
                else
                {
                    Vector2 newDirection_unifiedToForward_inUnitsOfGlobalSpace_normalized = newDirection_inUnitsOfGlobalSpace_normalized;
                    if (isForward_notBackward == false)
                    {
                        newDirection_unifiedToForward_inUnitsOfGlobalSpace_normalized = ConvertGivenDirectionToHelper_to_alingedDirectionToOtherHelper(newDirection_unifiedToForward_inUnitsOfGlobalSpace_normalized);
                    }
                    GetMountingAnchorPoint().TryPassOnNew_directionToHelper_unifiedTowardsForwardForCaseNonKinked_inUnitsOfGlobalSpace_normalized_toBoundGameobject(newDirection_unifiedToForward_inUnitsOfGlobalSpace_normalized, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
                }
            }
        }

        public Vector2 ConvertGivenDirectionToHelper_to_alingedDirectionToOtherHelper(Vector2 givenDirectionToHelper)
        {
            return GetMountingAnchorPoint().ConvertGivenDirectionToHelper_to_alingedDirectionToOtherHelper(givenDirectionToHelper);
        }

        public override InternalDXXL_BezierControlSubPoint2D GetNextSubPointAlongSplineDir(bool allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet)
        {
            if (isForward_notBackward)
            {
                InternalDXXL_BezierControlPointTriplet2D next_controlPoint = Get_controlPointTriplet_thisSubPointIsPartOf().GetNextControlPointTripletAlongSplineDir(allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet);
                if (next_controlPoint != null)
                {
                    return next_controlPoint.backwardHelperPoint;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return GetMountingAnchorPoint();
            }
        }

        public override InternalDXXL_BezierControlSubPoint2D GetPreviousSubPointAlongSplineDir(bool allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet)
        {
            if (isForward_notBackward)
            {
                return GetMountingAnchorPoint();
            }
            else
            {
                InternalDXXL_BezierControlPointTriplet2D previous_controlPoint = Get_controlPointTriplet_thisSubPointIsPartOf().GetPreviousControlPointTripletAlongSplineDir(allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet);
                if (previous_controlPoint != null)
                {
                    return previous_controlPoint.forwardHelperPoint;
                }
                else
                {
                    return null;
                }
            }
        }

        public override InternalDXXL_BezierControlSubPoint2D GetNextUsedSubPointAlongSplineDir(bool allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet)
        {
            if (isForward_notBackward)
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
            else
            {
                return GetMountingAnchorPoint();
            }
        }

        public override InternalDXXL_BezierControlSubPoint2D GetPreviousUsedSubPointAlongSplineDir(bool allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet)
        {
            if (isForward_notBackward)
            {
                return GetMountingAnchorPoint();
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
            if (isForward_notBackward)
            {
                return Get_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized();
            }
            else
            {
                return (-Get_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized());
            }
        }

        public InternalDXXL_BezierControlPointTriplet2D Get_neighboringControlPoint(bool allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet)
        {
            return (isForward_notBackward ? Get_controlPointTriplet_thisSubPointIsPartOf().GetNextControlPointTripletAlongSplineDir(allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet) : Get_controlPointTriplet_thisSubPointIsPartOf().GetPreviousControlPointTripletAlongSplineDir(allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet));
        }

        public InternalDXXL_BezierControlHelperSubPoint2D Get_neighboringHelperPoint_ofNeighboringControlPoint(bool allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet)
        {
            InternalDXXL_BezierControlPointTriplet2D neighboringControlPoint = Get_neighboringControlPoint(allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet);
            if (neighboringControlPoint == null)
            {
                return null;
            }
            else
            {
                InternalDXXL_BezierControlHelperSubPoint2D neighboringHelperPoint_ofNeighboringControlPoint = neighboringControlPoint.GetAHelperPoint(!isForward_notBackward);
                if (neighboringHelperPoint_ofNeighboringControlPoint.isUsed)
                {
                    return neighboringHelperPoint_ofNeighboringControlPoint;
                }
                else
                {
                    return null;
                }
            }
        }

        public override bool IsUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid()
        {
            bool isUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid;
            if (isForward_notBackward)
            {
                isUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid = ((bezierSplineDrawer_thisSubPointIsPartOf.gapFromEndToStart_isClosed == false) && Get_controlPointTriplet_thisSubPointIsPartOf().IsLastControlPoint());
            }
            else
            {
                isUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid = ((bezierSplineDrawer_thisSubPointIsPartOf.gapFromEndToStart_isClosed == false) && Get_controlPointTriplet_thisSubPointIsPartOf().IsFirstControlPoint());
            }

            if (isUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid) { isUsed = false; } //-> this setting of "isUsed" is actually not be necessary here. It acts here as double bottom if somehow the states got confused.
            return isUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid;
        }

        bool isUsed_afterInspectorInput;
        Vector2 direction_toThisHelper_fromAnchor_inUnitsOfActiveDrawSpace_normalized_afterInspectorInput;
        InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture_afterInspectorInput;
        float absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace_afterInspectorInput;
        bool closeGapState_afterInspectorInput;

        public void DrawValuesToInspector(Rect rect_ofEnclosingColorSubBox)
        {
#if UNITY_EDITOR
            FillingIsUsedFieldsWithDefaultValues_forInspector();

            bool isUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid = IsUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid();
            bool anchorHasKinkedJunctureType = (GetMountingAnchorPoint().junctureType == InternalDXXL_BezierControlAnchorSubPoint.JunctureType.kinked);

            DrawBackgroundColor_forInspector(rect_ofEnclosingColorSubBox);

            Rect reducedRectForOnlyContentLines = new Rect(rect_ofEnclosingColorSubBox.x + Get_inspectorHorizSpace_between_subPointColorBoxEdge_and_actualContenFields(), rect_ofEnclosingColorSubBox.y + Get_inspectorHorizSpace_between_subPointColorBoxEdge_and_actualContenFields(), rect_ofEnclosingColorSubBox.width - 2.0f * Get_inspectorHorizSpace_between_subPointColorBoxEdge_and_actualContenFields(), rect_ofEnclosingColorSubBox.height - 2.0f * Get_inspectorHorizSpace_between_subPointColorBoxEdge_and_actualContenFields());
            float currentHeightOffset = 0.0f;
            float horizShiftOffset_ofIsUsedCheckbox = 7.0f * UnityEditor.EditorGUIUtility.singleLineHeight;
            float width_ofCheckBox = 0.75f * UnityEditor.EditorGUIUtility.singleLineHeight;

            DrawHeadline_forInspector(reducedRectForOnlyContentLines, currentHeightOffset, horizShiftOffset_ofIsUsedCheckbox, width_ofCheckBox, isUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid, anchorHasKinkedJunctureType);
            DrawCollapsableArea_forInspector(reducedRectForOnlyContentLines, currentHeightOffset, isUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid, anchorHasKinkedJunctureType);
#endif
        }

        void FillingIsUsedFieldsWithDefaultValues_forInspector()
        {
            //This is for cases where the fields are not displayed or greyed out. They are used for an "hasChanged"-check afterwards:
            isUsed_afterInspectorInput = isUsed;
            position_inUnitsOfActiveDrawSpace_afterInspectorInput = position_inUnitsOfActiveDrawSpace;
            boundGameobject_afterInspectorInput = boundGameobject;
            direction_toThisHelper_fromAnchor_inUnitsOfActiveDrawSpace_normalized_afterInspectorInput = direction_toThisHelper_fromAnchor_inUnitsOfActiveDrawSpace_normalized;
            sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture_afterInspectorInput = sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture;
            absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace_afterInspectorInput = absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace;
            closeGapState_afterInspectorInput = bezierSplineDrawer_thisSubPointIsPartOf.gapFromEndToStart_isClosed;
        }

        void DrawBackgroundColor_forInspector(Rect rect_ofEnclosingColorSubBox)
        {
#if UNITY_EDITOR
            float alphaFactor_ofBackgroundColor = Get_controlPointTriplet_thisSubPointIsPartOf().isHighlighted ? InternalDXXL_BezierControlHelperSubPoint.alpha_ofInspectorBackgroundColor_highlighted : InternalDXXL_BezierControlHelperSubPoint.alpha_ofInspectorBackgroundColor_nonHighlighted;
            Color backgroundColor = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(bezierSplineDrawer_thisSubPointIsPartOf.color_ofHelperPoints, alphaFactor_ofBackgroundColor);
            UnityEditor.EditorGUI.DrawRect(rect_ofEnclosingColorSubBox, backgroundColor);
#endif
        }

        void DrawHeadline_forInspector(Rect reducedRectForOnlyContentLines, float currentHeightOffset, float horizShiftOffset_ofIsUsedCheckbox, float width_ofCheckBox, bool isUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid, bool anchorHasKinkedJunctureType)
        {
            DrawIsUsedCheckbox_forInspector(reducedRectForOnlyContentLines, currentHeightOffset, horizShiftOffset_ofIsUsedCheckbox, width_ofCheckBox, isUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid, anchorHasKinkedJunctureType);
            TryDrawCloseGapCheckbox_forInspector(reducedRectForOnlyContentLines, currentHeightOffset, width_ofCheckBox);
            DrawHeadlineTextWithFoldout_forInspector(reducedRectForOnlyContentLines, currentHeightOffset, horizShiftOffset_ofIsUsedCheckbox, isUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid, anchorHasKinkedJunctureType);
        }

        void DrawIsUsedCheckbox_forInspector(Rect reducedRectForOnlyContentLines, float currentHeightOffset, float horizShiftOffset_ofIsUsedCheckbox, float width_ofCheckBox, bool isUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid, bool anchorHasKinkedJunctureType)
        {
#if UNITY_EDITOR
            //no tooltip can be used here because this would require also a mainTextField, which this checkbox doesn't have. Instead the tooltip of the foldout (which also contains the headline text) beside this checkbox is intentionally misused to explain the checkbox meaning.
            Rect rect_of_isUsedCheckbox = new Rect(reducedRectForOnlyContentLines.x + horizShiftOffset_ofIsUsedCheckbox, reducedRectForOnlyContentLines.y + currentHeightOffset, width_ofCheckBox, UnityEditor.EditorGUIUtility.singleLineHeight);
            if (isUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid)
            {
                //always "off":
                UnityEditor.EditorGUI.BeginDisabledGroup(true);
                UnityEditor.EditorGUI.Toggle(rect_of_isUsedCheckbox, false);
                UnityEditor.EditorGUI.EndDisabledGroup();
            }
            else
            {
                if (anchorHasKinkedJunctureType)
                {
                    isUsed_afterInspectorInput = UnityEditor.EditorGUI.Toggle(rect_of_isUsedCheckbox, GUIContent.none, isUsed);
                }
                else
                {
                    //always "on":
                    UnityEditor.EditorGUI.BeginDisabledGroup(true);
                    UnityEditor.EditorGUI.Toggle(rect_of_isUsedCheckbox, true);
                    UnityEditor.EditorGUI.EndDisabledGroup();
                }
            }
#endif
        }

        void TryDrawCloseGapCheckbox_forInspector(Rect reducedRectForOnlyContentLines, float currentHeightOffset, float width_ofCheckBox)
        {
            if (isForward_notBackward)
            {
                if (Get_controlPointTriplet_thisSubPointIsPartOf().IsLastControlPoint())
                {
                    DrawCloseGapCheckbox_forInspector(reducedRectForOnlyContentLines, currentHeightOffset, width_ofCheckBox, "Close the gap from this last forward control point of the spline and connect it to the start of the spline to get a closed ring curve.");
                }
            }
            else
            {
                if (Get_controlPointTriplet_thisSubPointIsPartOf().IsFirstControlPoint())
                {
                    DrawCloseGapCheckbox_forInspector(reducedRectForOnlyContentLines, currentHeightOffset, width_ofCheckBox, "Close the gap from this first backward control point of the spline and connect it to the end of the spline to get a closed ring curve.");
                }
            }
        }

        void DrawCloseGapCheckbox_forInspector(Rect reducedRectForOnlyContentLines, float currentHeightOffset, float width_ofCheckBox, string tooltip)
        {
#if UNITY_EDITOR
            float width_ofCloseGapLabel = 4.2f * UnityEditor.EditorGUIUtility.singleLineHeight;

            Rect rect_of_closeGapCheckbox = new Rect(reducedRectForOnlyContentLines.x + reducedRectForOnlyContentLines.width - width_ofCheckBox, reducedRectForOnlyContentLines.y + currentHeightOffset, width_ofCheckBox, UnityEditor.EditorGUIUtility.singleLineHeight);
            closeGapState_afterInspectorInput = UnityEditor.EditorGUI.Toggle(rect_of_closeGapCheckbox, bezierSplineDrawer_thisSubPointIsPartOf.gapFromEndToStart_isClosed);

            Rect rect_of_closeGapLabel = new Rect(reducedRectForOnlyContentLines.x + reducedRectForOnlyContentLines.width - width_ofCloseGapLabel, reducedRectForOnlyContentLines.y + currentHeightOffset, width_ofCloseGapLabel, UnityEditor.EditorGUIUtility.singleLineHeight);
            UnityEditor.EditorGUI.LabelField(rect_of_closeGapLabel, new GUIContent("Close ring", tooltip));
#endif
        }

        void DrawHeadlineTextWithFoldout_forInspector(Rect reducedRectForOnlyContentLines, float currentHeightOffset, float horizShiftOffset_ofIsUsedCheckbox, bool isUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid, bool anchorHasKinkedJunctureType)
        {
#if UNITY_EDITOR
            GUIStyle style_ofHeadline = new GUIStyle(UnityEditor.EditorStyles.foldout);
            style_ofHeadline.richText = true;
            Color color_ofHeadline_nonAccentuated = bezierSplineDrawer_thisSubPointIsPartOf.color_ofHelperPoints;
            Color color_ofHeadline_accentuated = ((color_ofHeadline_nonAccentuated.grayscale < 0.175f) ? Color.Lerp(color_ofHeadline_nonAccentuated, Color.white, 0.9f) : Color.Lerp(color_ofHeadline_nonAccentuated, Color.black, 0.7f));
            if (isUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid)
            {
                color_ofHeadline_accentuated = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofHeadline_accentuated, 0.5f);
            }

            string headlineText;
            if (isForward_notBackward)
            {
                headlineText = "<color=#" + ColorUtility.ToHtmlStringRGBA(color_ofHeadline_accentuated) + "><b> Forward Weight</b></color>";
            }
            else
            {
                headlineText = "<color=#" + ColorUtility.ToHtmlStringRGBA(color_ofHeadline_accentuated) + "><b> Backward Weight</b></color>";
            }

            float horizShiftOffset_ofFoldoutHeadline = 0.65f * UnityEditor.EditorGUIUtility.singleLineHeight;
            Rect rect_of_foldoutHeadline = new Rect(reducedRectForOnlyContentLines.x + horizShiftOffset_ofFoldoutHeadline, reducedRectForOnlyContentLines.y + currentHeightOffset, horizShiftOffset_ofIsUsedCheckbox - horizShiftOffset_ofFoldoutHeadline, UnityEditor.EditorGUIUtility.singleLineHeight);

            //the tooltip for the foldoutWithText is intentionally misused here and doesn't explain the foldout itself, but the "isUsed"-checkbox right beside the text
            if (isUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid)
            {
                UnityEditor.EditorGUI.BeginDisabledGroup(true);
                UnityEditor.EditorGUI.Foldout(rect_of_foldoutHeadline, false, new GUIContent(headlineText, "Only available for closed ring splines."), true, style_ofHeadline);
                UnityEditor.EditorGUI.EndDisabledGroup();
            }
            else
            {
                GUIContent guiContent_ofFoldoutHeadline;
                if (anchorHasKinkedJunctureType)
                {
                    guiContent_ofFoldoutHeadline = new GUIContent(headlineText);
                }
                else
                {
                    guiContent_ofFoldoutHeadline = new GUIContent(headlineText, "Can only be deactivated for kinked juncture type.");
                }
                isOutfoldedInInspector = UnityEditor.EditorGUI.Foldout(rect_of_foldoutHeadline, isOutfoldedInInspector, guiContent_ofFoldoutHeadline, true, style_ofHeadline);
            }
#endif
        }

        void DrawCollapsableArea_forInspector(Rect reducedRectForOnlyContentLines, float currentHeightOffset, bool isUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid, bool anchorHasKinkedJunctureType)
        {
#if UNITY_EDITOR
            if (isOutfoldedInInspector && (isUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid == false))
            {
                bool greyOutValues = (isUsed == false);
                if (greyOutValues) { UnityEditor.EditorGUI.BeginDisabledGroup(true); }

                currentHeightOffset += UtilitiesDXXL_Components.Get_inspector_singleLineHeightInclSpacingBetweenLines();
                DrawPositionLine_forInspector(reducedRectForOnlyContentLines, currentHeightOffset);
                currentHeightOffset += UtilitiesDXXL_Components.Get_inspector_singleLineHeightInclSpacingBetweenLines();
                DrawBoundGameobjectLine_forInspector(reducedRectForOnlyContentLines, currentHeightOffset);
                currentHeightOffset += UtilitiesDXXL_Components.Get_inspector_singleLineHeightInclSpacingBetweenLines();
                DrawDirectionLine_forInspector(reducedRectForOnlyContentLines, currentHeightOffset, anchorHasKinkedJunctureType);
                currentHeightOffset += UtilitiesDXXL_Components.Get_inspector_singleLineHeightInclSpacingBetweenLines();
                DrawDistanceLine_forInspector(reducedRectForOnlyContentLines, currentHeightOffset);

                if (greyOutValues) { UnityEditor.EditorGUI.EndDisabledGroup(); }
            }
#endif
        }

        void DrawPositionLine_forInspector(Rect reducedRectForOnlyContentLines, float currentHeightOffset)
        {
#if UNITY_EDITOR
            string label_of_position = "Position";
            if (isUsed)
            {
                DrawPositionLine_forInspector(RecalcCurrentRect_forInspector(reducedRectForOnlyContentLines, currentHeightOffset), new GUIContent(label_of_position, Get_tooltip_explainingWheatherUnitsAreInGlobalOrInLocalSpace_forInspector()));
            }
            else
            {
                UnityEditor.EditorGUI.Vector2Field(RecalcCurrentRect_forInspector(reducedRectForOnlyContentLines, currentHeightOffset), new GUIContent(label_of_position), new Vector2(float.NaN, float.NaN));
            }
#endif
        }

        void DrawBoundGameobjectLine_forInspector(Rect reducedRectForOnlyContentLines, float currentHeightOffset)
        {
#if UNITY_EDITOR
            string label_of_bindToGameobject = "Bind to gameobject";
            if (isUsed)
            {
                DrawBoundGameobjectLine_forInspector(RecalcCurrentRect_forInspector(reducedRectForOnlyContentLines, currentHeightOffset), new GUIContent(label_of_bindToGameobject));
            }
            else
            {
                UnityEditor.EditorGUI.ObjectField(RecalcCurrentRect_forInspector(reducedRectForOnlyContentLines, currentHeightOffset), new GUIContent(label_of_bindToGameobject), null, typeof(UnityEngine.GameObject), true);
            }
#endif
        }

        void DrawDirectionLine_forInspector(Rect reducedRectForOnlyContentLines, float currentHeightOffset, bool anchorHasKinkedJunctureType)
        {
#if UNITY_EDITOR
            string label_ofDirection;
            if (Get_controlPointTriplet_thisSubPointIsPartOf().isHighlighted)
            {
                label_ofDirection = "Direction from <color=#" + ColorUtility.ToHtmlStringRGBA(bezierSplineDrawer_thisSubPointIsPartOf.color_ofAnchorPoints) + ">point center</color> to this weight";
            }
            else
            {
                label_ofDirection = "Direction from point center to this weight"; //-> Could adapt the color to the semitransparent triplet main color, but in most cases then the readability is bad.
            }

            if (isUsed)
            {
                string tooltip_ofDirection;
                bool anchorHasBoundGameobject = (GetMountingAnchorPoint().boundGameobject != null); //-> could be improved: there is no check whether the "boundGameobject" is "inactive" or the connection component on it is "disabled".

                if (anchorHasBoundGameobject && anchorHasKinkedJunctureType)
                {
                    tooltip_ofDirection = "This line displays the normalized direction from the center point to this weight point." + Environment.NewLine + Environment.NewLine + "For kinked juncture types this direction can be bound individually per weight side to the rotation of the gameobject at the center position." + Environment.NewLine + Environment.NewLine + "Note that the gameobject mentioned in the direction source picker dropdown menu is the gameobject that is bound to the CENTER position, not the one bound to this WEIGHT position." + Environment.NewLine + Environment.NewLine + Get_tooltip_explainingWheatherUnitsAreInGlobalOrInLocalSpace_forInspector();
                }
                else
                {
                    tooltip_ofDirection = "This is normalized. " + Environment.NewLine + "Value input will be changed so that the overall vector stays normalized." + Environment.NewLine + Environment.NewLine + Get_tooltip_explainingWheatherUnitsAreInGlobalOrInLocalSpace_forInspector();
                }

                direction_toThisHelper_fromAnchor_inUnitsOfActiveDrawSpace_normalized_afterInspectorInput = Draw_Vector2Field_withoutLineBreak_forInspector(RecalcCurrentRect_forInspector(reducedRectForOnlyContentLines, currentHeightOffset), out Rect rectForOnlyThePrefixLabel, new GUIContent(label_ofDirection, tooltip_ofDirection), direction_toThisHelper_fromAnchor_inUnitsOfActiveDrawSpace_normalized, true);

                if (anchorHasBoundGameobject && anchorHasKinkedJunctureType)
                {
                    float portionOPrefixLabelSpace_thatIsFilledByEnumPopup = 1.0f;
                    Rect rect_forDirectionSourceEnumPopup = new Rect(rectForOnlyThePrefixLabel.x + (1.0f - portionOPrefixLabelSpace_thatIsFilledByEnumPopup) * rectForOnlyThePrefixLabel.width, rectForOnlyThePrefixLabel.y, rectForOnlyThePrefixLabel.width * portionOPrefixLabelSpace_thatIsFilledByEnumPopup, UnityEditor.EditorGUIUtility.singleLineHeight);

                    InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D_wordedForInspectorDisplayAtHelpers wordedDirectionSource_beforeInput = InternalDXXL_BezierControlAnchorSubPoint2D.ConvertDirectionSource_fromUsedVersion_toWordedVersion(sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture);
                    InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D_wordedForInspectorDisplayAtHelpers wordedDirectionSource_afterInput = (InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D_wordedForInspectorDisplayAtHelpers)UnityEditor.EditorGUI.EnumPopup(rect_forDirectionSourceEnumPopup, GUIContent.none, wordedDirectionSource_beforeInput);
                    sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture_afterInspectorInput = InternalDXXL_BezierControlAnchorSubPoint2D.ConvertDirectionSource_fromWordedVersion_toUsedVersion(wordedDirectionSource_afterInput);
                }
            }
            else
            {
                UnityEditor.EditorGUI.Vector2Field(RecalcCurrentRect_forInspector(reducedRectForOnlyContentLines, currentHeightOffset), label_ofDirection, new Vector2(float.NaN, float.NaN));
            }
#endif
        }

        void DrawDistanceLine_forInspector(Rect reducedRectForOnlyContentLines, float currentHeightOffset)
        {
#if UNITY_EDITOR
            string label_of_distance = "Distance";
            if (isUsed)
            {
                absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace_afterInspectorInput = UnityEditor.EditorGUI.FloatField(RecalcCurrentRect_forInspector(reducedRectForOnlyContentLines, currentHeightOffset), new GUIContent(label_of_distance, Get_tooltip_explainingWheatherUnitsAreInGlobalOrInLocalSpace_forInspector()), absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace);
            }
            else
            {
                UnityEditor.EditorGUI.FloatField(RecalcCurrentRect_forInspector(reducedRectForOnlyContentLines, currentHeightOffset), new GUIContent(label_of_distance), float.NaN);
            }
#endif
        }

        public bool TryApplyChangesAfterInspectorInput()
        {
            //The checks here are more reliable than "EditorGUI.BeginChangeCheck/EndChangeCheck()", which also reports "change" only due to mouse selection, even when the value didn't change yet.

            if (isUsed_afterInspectorInput != isUsed)
            {
                bezierSplineDrawer_thisSubPointIsPartOf.RegisterStateForUndo("Toggle Spline Weight", false, false);
                ChangeUsedState(isUsed_afterInspectorInput, true);
                return true;
            }

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

            if (direction_toThisHelper_fromAnchor_inUnitsOfActiveDrawSpace_normalized_afterInspectorInput != direction_toThisHelper_fromAnchor_inUnitsOfActiveDrawSpace_normalized)
            {
                bezierSplineDrawer_thisSubPointIsPartOf.RegisterStateForUndo("Change Spline Direction", true, true);
                direction_toThisHelper_fromAnchor_inUnitsOfActiveDrawSpace_normalized_afterInspectorInput.Normalize();
                Set_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfActiveDrawSpace_normalized(direction_toThisHelper_fromAnchor_inUnitsOfActiveDrawSpace_normalized_afterInspectorInput, true, null);
                return true;
            }

            if (sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture_afterInspectorInput != sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture)
            {
                bezierSplineDrawer_thisSubPointIsPartOf.RegisterStateForUndo("Change Spline Direction Source", true, true);
                ProcessChanging_sourceOfDirectionFromAnchor(sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture_afterInspectorInput);
                return true;
            }

            if (absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace_afterInspectorInput != absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace)
            {
                absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace_afterInspectorInput = Mathf.Max(absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace_afterInspectorInput, 0.0f);
                bezierSplineDrawer_thisSubPointIsPartOf.RegisterStateForUndo("Change Spline Distance", true, true);
                Set_absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace(absDistanceToAnchorPoint_inUnitsOfActiveDrawSpace_afterInspectorInput, true, null);
                return true;
            }

            if (closeGapState_afterInspectorInput != bezierSplineDrawer_thisSubPointIsPartOf.gapFromEndToStart_isClosed)
            {
                bezierSplineDrawer_thisSubPointIsPartOf.ChangeCloseGapState(closeGapState_afterInspectorInput);
                return true;
            }

            return false;
        }

        public override float GetPropertyHeightForInspectorList()
        {
#if UNITY_EDITOR
            if (isOutfoldedInInspector && (IsUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid() == false))
            {
                float height_forAllContentLines = 5.0f * UtilitiesDXXL_Components.Get_inspector_singleLineHeightInclSpacingBetweenLines();
                return (Get_inspectorHorizSpace_between_subPointColorBoxEdge_and_actualContenFields() + height_forAllContentLines + Get_inspectorHorizSpace_between_subPointColorBoxEdge_and_actualContenFields());
            }
            else
            {
                return (Get_inspectorHorizSpace_between_subPointColorBoxEdge_and_actualContenFields() + UnityEditor.EditorGUIUtility.singleLineHeight + Get_inspectorHorizSpace_between_subPointColorBoxEdge_and_actualContenFields());
            }
#else
            return 16.0f; //-> not used
#endif
        }

        public static float Get_inspectorHorizSpace_between_subPointColorBoxEdge_and_actualContenFields()
        {
#if UNITY_EDITOR
            return (0.2f * UnityEditor.EditorGUIUtility.singleLineHeight);
#else
            return 16.0f; //-> not used
#endif
        }

    }

}
