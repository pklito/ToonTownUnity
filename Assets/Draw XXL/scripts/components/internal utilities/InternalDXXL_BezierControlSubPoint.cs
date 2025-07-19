namespace DrawXXL
{
    using System;
    using UnityEngine;

    [Serializable]
    public class InternalDXXL_BezierControlSubPoint
    {
        public enum SubPointType { backwardHelper, anchor, forwardHelper };

        [SerializeField] public bool isUsed; //can be disabled by the user in the control points list inspector (only for kinked junctures), or is automatically disabled for endPoints of non-closed splines
        [SerializeField] public Vector3 position_inUnitsOfActiveDrawSpace;
        [SerializeField] Vector3 position_inUnitsOfGlobalSpace;

        public BezierSplineDrawer bezierSplineDrawer_thisSubPointIsPartOf;
        [SerializeField] public int i_ofContainingControlPoint_insideControlPointsList;
        public SubPointType subPointType;

        public GameObject boundGameobject;
        public DrawXXLSplineConnection connectionComponent_onBoundGameobject;
        public bool boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled;
        public bool boundGameobjectInclConnectionComponent_isAssignedButMayBeInactiveOrDisabled;

        public Quaternion globalRotation_ofPositionHandle = Quaternion.identity;
        public bool recalc_globalRotation_ofPositionHandle_duringNextOnSceneGUI = true;

        public virtual void InitializeValuesThatAreIndependentFromOtherSubPoints(InternalDXXL_BezierControlPointTriplet controlPoint_thisSubPointIsPartOf)
        {
            boundGameobject = null;
            connectionComponent_onBoundGameobject = null;
            boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled = false;
            i_ofContainingControlPoint_insideControlPointsList = controlPoint_thisSubPointIsPartOf.i_ofThisPoint_insideControlPointsList;
            bezierSplineDrawer_thisSubPointIsPartOf = controlPoint_thisSubPointIsPartOf.bezierSplineDrawer_thisPointIsPartOf;
        }

        public void ReassignIndexInsideControlPointsList(int new_i)
        {
            SetBoolOf_boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled(); //-> function is dependent on the "old" i.
            if (boundGameobjectInclConnectionComponent_isAssignedButMayBeInactiveOrDisabled)
            {
                connectionComponent_onBoundGameobject.i_ofControlPointTriplet_thisGameobjectIsBoundTo = new_i;
            }
            i_ofContainingControlPoint_insideControlPointsList = new_i;
        }

        public InternalDXXL_BezierControlPointTriplet Get_controlPointTriplet_thisSubPointIsPartOf()
        {
            //Serialized lists containing other lists and containing custom classes with cross references don't work well in Unity, even when using the [SerializeReference] attribute. Functions like this try to keep the convenience of a real reference tree. The only "real" reference is the monobehavior-inherited spline component. Everything has to start from there.
            return bezierSplineDrawer_thisSubPointIsPartOf.listOfControlPointTriplets[i_ofContainingControlPoint_insideControlPointsList];
        }

        public void SetBoolOf_boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled()
        {
            ///De-assigning connectionComponents is not trivial as soon as it has to comply with the build-in Undo-System.
            //Problems:
            //The spline-component and the connection-component should only exist together, but they are on independent gameobjects, which the user can delete or copy independently.
            //When the spline-component is deleted then also all corresponding connection-componenets should be deleted.
            //-> It could be done in "spline.OnDestroy()", but this is not fired always when a component gets destroyed. The Unity docu says the it is not fired when the carrying gameobject is inactive. Though testing it in the Unity Editor showed: It is often called, even when the gameobject is inactive, but it is sometimes omited, even when the gameobject is active. So overall: Not fully reliable. Orphaned connection-components may still remain in the scene, which is not desired.
            //-> More reliable than "spline.OnDestroy()" is "spline.OnDisable()", but we don't want to destroy the connection reference just because some disabled a participating component (maybe with the intention to enabled it sometime later)
            //-> So the connection components should handle their deletion self contained, as soon as the referncing spline is not there anymore.
            //-> This is working fine as long as "Undo" doesn't come into play.
            //-> If a boundGameobject gets deleted and then the deletion is reverted via "Editor/Undo", then the two partners (spline-component and connection-component) don't recognize each other anymore as "referenced partners". The reference is lost. This seems to be a Unity bug, see here: https://forum.unity.com/threads/monobehaviour-references-are-lost-on-undo.587011/ and here: https://issuetracker.unity3d.com/issues/gameobject-isnt-set-as-public-variable-after-undo-operation It's an unconvenience but since Unity itself accepts this bug it's hopefully indeed "acceptable".
            //-> It can be fixed in the case of spline-deletion (because in "spline.OnDestroy()" the connectionComponents can be deleted by the spline itself (so not following the mentioned "self contained deletion" way)), and along with that the spline can register "Undo.DestroyImmediate()" for the connection-components: In this case the references are correctly reverted after Undo. A similar construction inside "connectionComponent.OnDestroy()" doesn't have the same fixing effect though (see note there).
            //-> Another approach would be to not immediately destroy the connection components after spline-deletion, but delay the self contained destruction with a timer, so that the undo-process doesn't have to "recreate them as new instance from serialized data" in the hope that they are then still the "correct reference". But tests showed: It is not the case. It makes no difference and the reference is still lost.
            //-> Another approach would be to somehow search the formerly reference component and recreate the reference "OnUndoExecuted". The problem with that is that it is quiete non-intended from Unities paradigm of doing things. Such "hacky interventions" sometimes lead to Editor crashes when shuffling around "Undo/Redo" to and fro. It seems unwieldly and to risky. 
            //-> Another case where the reference gets lost: "Assign boundGameobject in the inspector" -> "Undo assign" -> "Redo assign" -> reference is lost.

            if (boundGameobject == null)
            {
                //cases that arrive here:
                //-> "boundGameobject" hasnn't been assigned yet
                //-> "boundGameobject" was regularly deassgined
                //-> "boundGameobject" has been independently deleted (but may come back via "Undo")
                boundGameobjectInclConnectionComponent_isAssignedButMayBeInactiveOrDisabled = false;
                boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled = false;
            }
            else
            {
                if (connectionComponent_onBoundGameobject == null)
                {
                    //cases that arrive here:
                    //-> the "connectionComponent_onBoundGameobject"-component has been independently deleted from the (still existing) "boundGameobject" (but it may come back via "Undo")
                    //-> also after "Undo" it sometimes doesn't come back due to the reference lost error described above in this function
                    //-> assign boundGameobject to a controlSubPoint -> Undo assign -> Redo assign -> connection is not retrieved
                    boundGameobject = null;
                    boundGameobjectInclConnectionComponent_isAssignedButMayBeInactiveOrDisabled = false;
                    boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled = false;
                }
                else
                {
                    if (connectionComponent_onBoundGameobject.bezierSplineDrawer_thatHasReferencedThisGameobject != bezierSplineDrawer_thisSubPointIsPartOf)
                    {
                        //-> The spline component (e.g. along with it's carrying gameobject) has been copied. Both the old spline component and the new spline component here reference the single connection-component on the boundGameobject
                        //-> The new spline here creates his own additonal connection component on the boundGameobject now:

                        CreateConnectionComponentOnBoundGameobject("Auto-create connection after spline copy");
                        boundGameobjectInclConnectionComponent_isAssignedButMayBeInactiveOrDisabled = true;
                        boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled = connectionComponent_onBoundGameobject.isActiveAndEnabled;
                    }
                    else
                    {
                        // if (connectionComponent_onBoundGameobject.bezierSubPoint_thatHasReferencedThisGameobject != this) //this is not suitable as reference, because in the serialized context "InternalDXXL_BezierControlSubPoint" acts as value type, not as reference type.
                        if (connectionComponent_onBoundGameobject.i_ofControlPointTriplet_thisGameobjectIsBoundTo != i_ofContainingControlPoint_insideControlPointsList)
                        {
                            bezierSplineDrawer_thisSubPointIsPartOf.DeleteConnectionComponentOfBoundGameobject_onControlSubPoint(i_ofContainingControlPoint_insideControlPointsList, subPointType);
                            boundGameobjectInclConnectionComponent_isAssignedButMayBeInactiveOrDisabled = false;
                            boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled = false;
                            UtilitiesDXXL_Log.PrintErrorCode("50-" + connectionComponent_onBoundGameobject.i_ofControlPointTriplet_thisGameobjectIsBoundTo + "-" + i_ofContainingControlPoint_insideControlPointsList + "-" + bezierSplineDrawer_thisSubPointIsPartOf.listOfControlPointTriplets.Count);
                        }
                        else
                        {
                            if (connectionComponent_onBoundGameobject.subPointType_whereThisGameobjectIsBoundTo != subPointType)
                            {
                                bezierSplineDrawer_thisSubPointIsPartOf.DeleteConnectionComponentOfBoundGameobject_onControlSubPoint(i_ofContainingControlPoint_insideControlPointsList, subPointType);
                                boundGameobjectInclConnectionComponent_isAssignedButMayBeInactiveOrDisabled = false;
                                boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled = false;
                                UtilitiesDXXL_Log.PrintErrorCode("51-" + connectionComponent_onBoundGameobject.subPointType_whereThisGameobjectIsBoundTo + "-" + subPointType + "-" + bezierSplineDrawer_thisSubPointIsPartOf.listOfControlPointTriplets.Count);
                            }
                            else
                            {
                                boundGameobjectInclConnectionComponent_isAssignedButMayBeInactiveOrDisabled = true;
                                boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled = connectionComponent_onBoundGameobject.isActiveAndEnabled;
                            }
                        }
                    }
                }
            }
        }

        public void ProcessNewGameobjectAssignment(GameObject newlyAssignedGameobject)
        {
            if (newlyAssignedGameobject != boundGameobject)
            {
                if (bezierSplineDrawer_thisSubPointIsPartOf.CheckIf_gameobjectToAssign_isAlreadyAssignedAtAnotherSubPointOfTheSpline(newlyAssignedGameobject, out int i_ofTripletThatContainsTheSubPointThatAlreadyHasTheGameobjectAssigned, out SubPointType subPointThatAlreadyHasTheGameobjectAssigned))
                {
                    //This restriction is done to prevent tangled unwieldy cross-dependencies between subPoints. Otherwise numerous cases would need special consideration, e.g.: A gameobject could be bound not only to the backward helper, but also to the forward helper of the same controlPoint triplet.
                    //Already without this restriction there are overdefined situations: E.g. when different gameobjects are bound to all three subPoints of a triplet, junctureType=kinked, and both helperPoint bind their direction to the same transform direction of the boundGameobject at the center point.
                    Debug.LogError("Assignment of gameobject (" + newlyAssignedGameobject.name + ") denied, because a gameobject can only be assinged once per spline. It is already assinged at the " + GetSubPointTypeAsString(subPointThatAlreadyHasTheGameobjectAssigned) + " of control point " + i_ofTripletThatContainsTheSubPointThatAlreadyHasTheGameobjectAssigned + ".");
                }
                else
                {
                    string nameOfUndoEntry = "Change Spline Gameobject Ref";
                    bezierSplineDrawer_thisSubPointIsPartOf.RegisterStateForUndo(nameOfUndoEntry, true, true);

                    if (boundGameobject != null)
                    {
                        //Deassign old gameobject:
                        if (connectionComponent_onBoundGameobject != null)
                        {
#if UNITY_EDITOR
                            UnityEditor.Undo.DestroyObjectImmediate(connectionComponent_onBoundGameobject);
#endif
                        }
                        boundGameobject = null;
                        connectionComponent_onBoundGameobject = null;
                    }

                    boundGameobject = newlyAssignedGameobject;

                    if (newlyAssignedGameobject != null)
                    {
                        //Assign new gameobject:
                        CreateConnectionComponentOnBoundGameobject(nameOfUndoEntry);
                    }
                    else
                    {
                        ResetDirectionSourceToIndependent();
                    }
                }
            }
        }

        void CreateConnectionComponentOnBoundGameobject(string nameOfUndoEntry_forNewlyCreatedComponent)
        {
#if UNITY_EDITOR
            connectionComponent_onBoundGameobject = UnityEditor.Undo.AddComponent<DrawXXLSplineConnection>(boundGameobject);
            UnityEditor.Undo.RegisterCompleteObjectUndo(connectionComponent_onBoundGameobject, nameOfUndoEntry_forNewlyCreatedComponent);
#else
            connectionComponent_onBoundGameobject = boundGameobject.AddComponent<DrawXXLSplineConnection>();
#endif

            connectionComponent_onBoundGameobject.componentHasBeenManuallyCreated = false;
            connectionComponent_onBoundGameobject.bezierSplineDrawer_thatHasReferencedThisGameobject = bezierSplineDrawer_thisSubPointIsPartOf;
            connectionComponent_onBoundGameobject.i_ofControlPointTriplet_thisGameobjectIsBoundTo = i_ofContainingControlPoint_insideControlPointsList;
            connectionComponent_onBoundGameobject.subPointType_whereThisGameobjectIsBoundTo = subPointType;

            SetPos_inUnitsOfGlobalSpace(boundGameobject.transform.position, true, boundGameobject);
            TryTransferBoundGameobjectsRotationToAnchorPointsDirection();
        }

        public virtual void ResetDirectionSourceToIndependent()
        {
        }

        public virtual void TryTransferBoundGameobjectsRotationToAnchorPointsDirection()
        {
        }

        public Vector3 GetPos_inUnitsOfGlobalSpace()
        {
            return position_inUnitsOfGlobalSpace;
        }

        public Vector3 GetPos_inUnitsOfActiveDrawSpace()
        {
            return position_inUnitsOfActiveDrawSpace;
        }

        public virtual void SetPos_inUnitsOfGlobalSpace(Vector3 newPos_inUnitsOfGlobalSpace, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            //all children override this
        }

        public Vector3 SetPos_inUnitsOfGlobalSpace_butIgnoreDependentValues_nonRecursively(Vector3 newPos_inUnitsOfGlobalSpace, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            //"_nonRecursively" indicates: This function is reserved to be called inside the overrides of "SetPos_inUnitsOfGlobalSpace". In all other cases use "SetPos_inUnitsOfGlobalSpace(..., false)"

            Vector3 offset_fromPrevious_toNewPosition_inUnitsOfGlobalSpace = newPos_inUnitsOfGlobalSpace - position_inUnitsOfGlobalSpace;
            position_inUnitsOfGlobalSpace = newPos_inUnitsOfGlobalSpace;
            position_inUnitsOfActiveDrawSpace = bezierSplineDrawer_thisSubPointIsPartOf.TransformPos_fromGlobalSpace_toUnitsOfActiveDrawSpace(newPos_inUnitsOfGlobalSpace);

            SetBoolOf_boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled();
            if (boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled)
            {
                if (boundGameobject != boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
                {
                    boundGameobject.transform.position = newPos_inUnitsOfGlobalSpace;
                }
            }

            return offset_fromPrevious_toNewPosition_inUnitsOfGlobalSpace;
        }

        public void SetPos_inUnitsOfActiveDrawSpace(Vector3 newPos_inUnitsOfActiveDrawSpace, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            Vector3 newPos_inUnitsOfGlobalSpace = bezierSplineDrawer_thisSubPointIsPartOf.TransformPos_fromUnitsOfActiveDrawSpace_toGlobalSpace(newPos_inUnitsOfActiveDrawSpace);
            SetPos_inUnitsOfGlobalSpace(newPos_inUnitsOfGlobalSpace, updateDependentValuesOnControlPointTriplet, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
        }

        public void AddPosOffset_inUnitsOfGlobalSpace(Vector3 posOffset_inUnitsOfGlobalSpace, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            Vector3 newPos_inUnitsOfGlobalSpace = GetPos_inUnitsOfGlobalSpace() + posOffset_inUnitsOfGlobalSpace;
            SetPos_inUnitsOfGlobalSpace(newPos_inUnitsOfGlobalSpace, updateDependentValuesOnControlPointTriplet, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
        }

        public void AddPosOffset_inUnitsOfActiveDrawSpace(Vector3 posOffset_inUnitsOfActiveDrawSpace, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            Vector3 newPos_inUnitsOfActiveDrawSpace = GetPos_inUnitsOfActiveDrawSpace() + posOffset_inUnitsOfActiveDrawSpace;
            SetPos_inUnitsOfActiveDrawSpace(newPos_inUnitsOfActiveDrawSpace, updateDependentValuesOnControlPointTriplet, boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls);
        }

        public virtual void Set_direction_toForward_inUnitsOfGlobalSpace_normalized(Vector3 newDirection_toForward_inUnitsOfGlobalSpace_normalized, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            //-> is not intended to be called for non-overriding helperPoints. Use "helperPoint.Set_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized()" instead 
            UtilitiesDXXL_Log.PrintErrorCode("37");
        }

        public virtual void Set_direction_toBackward_inUnitsOfGlobalSpace_normalized(Vector3 newDirection_toBackward_inUnitsOfGlobalSpace_normalized, bool updateDependentValuesOnControlPointTriplet, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            //-> is not intended to be called for non-overriding helperPoints. Use "helperPoint.Set_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized()" instead 
            UtilitiesDXXL_Log.PrintErrorCode("38");
        }

        public virtual InternalDXXL_BezierControlAnchorSubPoint.JunctureType GetJunctureType()
        {
            //-> is not intended to be called for non-overriding helperPoints
            UtilitiesDXXL_Log.PrintErrorCode("39");
            return InternalDXXL_BezierControlAnchorSubPoint.JunctureType.aligned;
        }

        public virtual InternalDXXL_BezierControlHelperSubPoint GetForwardHelper()
        {
            //-> is not intended to be called for non-overriding helperPoints
            UtilitiesDXXL_Log.PrintErrorCode("40");
            return null;
        }

        public virtual InternalDXXL_BezierControlHelperSubPoint GetBackwardHelper()
        {
            //-> is not intended to be called for non-overriding helperPoints
            UtilitiesDXXL_Log.PrintErrorCode("41");
            return null;
        }

        public virtual InternalDXXL_BezierControlAnchorSubPoint.SourceOf_directionToHelper Get_sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked()
        {
            //-> is not intended to be called for non-overriding helperPoints
            UtilitiesDXXL_Log.PrintErrorCode("42");
            return InternalDXXL_BezierControlAnchorSubPoint.SourceOf_directionToHelper.independentFromGameobject;
        }

        public virtual InternalDXXL_BezierControlSubPoint GetNextSubPointAlongSplineDir(bool allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet)
        {
            //-> all children override this
            return null;
        }

        public virtual InternalDXXL_BezierControlSubPoint GetPreviousSubPointAlongSplineDir(bool allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet)
        {
            //-> all children override this
            return null;
        }

        public virtual InternalDXXL_BezierControlSubPoint GetNextUsedSubPointAlongSplineDir(bool allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet)
        {
            //-> all children override this
            return null;
        }

        public virtual InternalDXXL_BezierControlSubPoint GetPreviousUsedSubPointAlongSplineDir(bool allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet)
        {
            //-> all children override this
            return null;
        }

        public InternalDXXL_BezierControlSubPoint GetNextUsedNonSuperimposedSubPointAlongSplineDir(bool allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet)
        {
            InternalDXXL_BezierControlSubPoint currentCandidateFor_nextUsedNonSuperimposedSubPointAlongSplineDir = GetNextSubPointAlongSplineDir(allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet); //-> cannot use "GetNext-USED-SubPointAlongSplineDir()" here in this function, because then an endless-loop-prevention-check is not possible for cases, where this function is called on subPoints which are iself "isUsed=false".
            int maxAttempts = 100;
            for (int i = 0; i < maxAttempts; i++)
            {
                if (currentCandidateFor_nextUsedNonSuperimposedSubPointAlongSplineDir != null)
                {
                    if (currentCandidateFor_nextUsedNonSuperimposedSubPointAlongSplineDir != this) //-> prevent endless loops around closed splines
                    {
                        if (currentCandidateFor_nextUsedNonSuperimposedSubPointAlongSplineDir.isUsed == true)
                        {
                            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(GetPos_inUnitsOfGlobalSpace(), currentCandidateFor_nextUsedNonSuperimposedSubPointAlongSplineDir.GetPos_inUnitsOfGlobalSpace()))
                            {
                                currentCandidateFor_nextUsedNonSuperimposedSubPointAlongSplineDir = currentCandidateFor_nextUsedNonSuperimposedSubPointAlongSplineDir.GetNextSubPointAlongSplineDir(allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet);
                            }
                            else
                            {
                                return currentCandidateFor_nextUsedNonSuperimposedSubPointAlongSplineDir;
                            }
                        }
                        else
                        {
                            currentCandidateFor_nextUsedNonSuperimposedSubPointAlongSplineDir = currentCandidateFor_nextUsedNonSuperimposedSubPointAlongSplineDir.GetNextSubPointAlongSplineDir(allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public InternalDXXL_BezierControlSubPoint GetPreviousUsedNonSuperimposedSubPointAlongSplineDir(bool allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet)
        {
            InternalDXXL_BezierControlSubPoint currentCandidateFor_previousUsedNonSuperimposedSubPointAlongSplineDir = GetPreviousSubPointAlongSplineDir(allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet); //-> cannot use "GetPrevious-USED-SubPointAlongSplineDir()" here in this function, because then an endless-loop-prevention-check is not possible for cases, where this function is called on subPoints which are iself "isUsed=false".
            int maxAttempts = 100;
            for (int i = 0; i < maxAttempts; i++)
            {
                if (currentCandidateFor_previousUsedNonSuperimposedSubPointAlongSplineDir != null)
                {
                    if (currentCandidateFor_previousUsedNonSuperimposedSubPointAlongSplineDir != this) //-> prevent endless loops around closed splines
                    {
                        if (currentCandidateFor_previousUsedNonSuperimposedSubPointAlongSplineDir.isUsed == true)
                        {
                            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(GetPos_inUnitsOfGlobalSpace(), currentCandidateFor_previousUsedNonSuperimposedSubPointAlongSplineDir.GetPos_inUnitsOfGlobalSpace()))
                            {
                                currentCandidateFor_previousUsedNonSuperimposedSubPointAlongSplineDir = currentCandidateFor_previousUsedNonSuperimposedSubPointAlongSplineDir.GetPreviousSubPointAlongSplineDir(allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet);
                            }
                            else
                            {
                                return currentCandidateFor_previousUsedNonSuperimposedSubPointAlongSplineDir;
                            }
                        }
                        else
                        {
                            currentCandidateFor_previousUsedNonSuperimposedSubPointAlongSplineDir = currentCandidateFor_previousUsedNonSuperimposedSubPointAlongSplineDir.GetPreviousSubPointAlongSplineDir(allowRequestingControlPointAsResult_inCaseOfRingSplineOfOnlyOneControlPointTriplet);
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            return null;
        }

        public virtual Vector3 GetDirectionAlongSplineForwardBasedOnNeighborPoints_inUnitsOfGlobalSpace()
        {
            //-> all children override this
            //-> not guaranteed normalized
            //-> not to be consued with "anchor.Get_direction_to*Helper*()"
            return Vector3.forward;
        }

        public virtual bool IsUnusedHelperPointAtStartOrEndOfUnclosedSplines_onTheControlPointSideTowardsVoid()
        {
            //-> all children override this
            return false;
        }

        public static string GetSubPointTypeAsString(SubPointType subPointType_toGetAsString)
        {
            switch (subPointType_toGetAsString)
            {
                case SubPointType.backwardHelper:
                    return "backward weight point";
                case SubPointType.anchor:
                    return "center point";
                case SubPointType.forwardHelper:
                    return "forward weight point";
                default:
                    return "unknown sub point";
            }
        }

        public Rect RecalcCurrentRect_forInspector(Rect reducedRectForOnlyContentLines, float currentHeightOffset)
        {
#if UNITY_EDITOR
            return new Rect(reducedRectForOnlyContentLines.x, reducedRectForOnlyContentLines.y + currentHeightOffset, reducedRectForOnlyContentLines.width, UnityEditor.EditorGUIUtility.singleLineHeight);
#else
            return default;
#endif
        }

        public Vector3 Draw_Vector3Field_withoutLineBreak_forInspector(Rect position, GUIContent label, Vector3 value, bool allowRichText = false)
        {
            return Draw_Vector3Field_withoutLineBreak_forInspector(position, out Rect rectForOnlyThePrefixLabel, label, value, allowRichText);
        }

        public Vector3 Draw_Vector3Field_withoutLineBreak_forInspector(Rect position, out Rect rectForOnlyThePrefixLabel, GUIContent label, Vector3 value, bool allowRichText = false)
        {
#if UNITY_EDITOR
            //"EditorGUI.Vector3Field" makes a line break and expands to two lines if the inspector window gets narrow. Therefore this modified version without the line break.
            Rect position_forOnlyTheContentValues;
            if (allowRichText)
            {
                GUIStyle styleWithRichText = new GUIStyle(UnityEditor.EditorStyles.label);
                styleWithRichText.richText = true;
                position_forOnlyTheContentValues = UnityEditor.EditorGUI.PrefixLabel(position, label, styleWithRichText);
            }
            else
            {
                position_forOnlyTheContentValues = UnityEditor.EditorGUI.PrefixLabel(position, label);
            }
            rectForOnlyThePrefixLabel = new Rect(position.x, position.y, position.width - position_forOnlyTheContentValues.width, position.height);

            Vector3 value_after = UnityEditor.EditorGUI.Vector3Field(position_forOnlyTheContentValues, GUIContent.none, value);
            return value_after;
#else
            rectForOnlyThePrefixLabel = default;
            return default;
#endif
        }

        public Vector3 position_inUnitsOfActiveDrawSpace_afterInspectorInput;
        public void DrawPositionLine_forInspector(Rect rect, GUIContent guiContent)
        {
            position_inUnitsOfActiveDrawSpace_afterInspectorInput = Draw_Vector3Field_withoutLineBreak_forInspector(rect, guiContent, position_inUnitsOfActiveDrawSpace);
        }

        public GameObject boundGameobject_afterInspectorInput;
        public void DrawBoundGameobjectLine_forInspector(Rect rect, GUIContent guiContent)
        {
#if UNITY_EDITOR
            SetBoolOf_boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled();
            bool boundGameobjectConnetionExists_butIsNotActiveRespEnabled = ((boundGameobject != null) && (connectionComponent_onBoundGameobject != null) && ((boundGameobject.activeInHierarchy == false) || (connectionComponent_onBoundGameobject.enabled == false)));
            if (boundGameobjectConnetionExists_butIsNotActiveRespEnabled)
            {
                Rect rect_forGameobjectPickerAndInactiveNotifier = UnityEditor.EditorGUI.PrefixLabel(rect, guiContent);
                float posOfSeparation_0to1 = 0.5f;
                Rect rect_gameobjectPickerWithoutLabel = new Rect(rect_forGameobjectPickerAndInactiveNotifier.x, rect_forGameobjectPickerAndInactiveNotifier.y, posOfSeparation_0to1 * rect_forGameobjectPickerAndInactiveNotifier.width, rect_forGameobjectPickerAndInactiveNotifier.height);
                Rect rect_forInactiveRespDisabledNotifier = new Rect(rect_forGameobjectPickerAndInactiveNotifier.x + posOfSeparation_0to1 * rect_forGameobjectPickerAndInactiveNotifier.width, rect_forGameobjectPickerAndInactiveNotifier.y, (1.0f - posOfSeparation_0to1) * rect_forGameobjectPickerAndInactiveNotifier.width, rect_forGameobjectPickerAndInactiveNotifier.height);

                boundGameobject_afterInspectorInput = (UnityEngine.GameObject)UnityEditor.EditorGUI.ObjectField(rect_gameobjectPickerWithoutLabel, GUIContent.none, boundGameobject, typeof(UnityEngine.GameObject), true);
                if (boundGameobject.activeInHierarchy == false)
                {
                    UnityEditor.EditorGUI.LabelField(rect_forInactiveRespDisabledNotifier, new GUIContent(" is inactive", "The bound gameobject or any of it's parents is set to inactive. As long as this is the case the tieing will not be updated."));
                }
                else
                {
                    UnityEditor.EditorGUI.LabelField(rect_forInactiveRespDisabledNotifier, new GUIContent(" is disabled", "The connection component on the bound gameobject is disabled. As long as this is the case the tieing will not be updated."));
                }
            }
            else
            {
                boundGameobject_afterInspectorInput = (UnityEngine.GameObject)UnityEditor.EditorGUI.ObjectField(rect, guiContent, boundGameobject, typeof(UnityEngine.GameObject), true);
            }
#endif
        }

        public string Get_tooltip_explainingWheatherUnitsAreInGlobalOrInLocalSpace_forInspector()
        {
            switch (bezierSplineDrawer_thisSubPointIsPartOf.drawSpace)
            {
                case BezierSplineDrawer.DrawSpace.global:
                    return "This value display is in units of the active draw space, which is currently the global space.";
                case BezierSplineDrawer.DrawSpace.localDefinedByThisGameobject:
                    return "This value display is in units of the active draw space, which is currently the local space defined by the gameobject that holds this spline component.";
                default:
                    return "";
            }
        }

        public virtual float GetPropertyHeightForInspectorList()
        {
            //-> all children override this
            return 16.0f;
        }

    }

}
