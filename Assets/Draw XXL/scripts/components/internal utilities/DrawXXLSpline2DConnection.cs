namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Internal Not For Manual Creation/Draw XXL Spline 2D Connection")]
    [ExecuteInEditMode]
    public class DrawXXLSpline2DConnection : MonoBehaviour
    {
        Quaternion lastGlobalRotationOfThisGameobject_thatTheSplineKnowsOf; //-> other than the position the spline control points don't save a "rotation" but only a "direction", so there is an extra member to care for the synchronisation here
        public BezierSplineDrawer2D bezierSplineDrawer_thatHasReferencedThisGameobject;
        //public InternalDXXL_BezierControlSubPoint2D bezierSubPoint_thatHasReferencedThisGameobject; //this is not suitable as reference, because in the serialized context "InternalDXXL_BezierControlSubPoint" acts as value type, not as reference type.
        public bool componentHasBeenManuallyCreated = true;
        public int i_ofControlPointTriplet_thisGameobjectIsBoundTo;
        public InternalDXXL_BezierControlSubPoint.SubPointType subPointType_whereThisGameobjectIsBoundTo;

        void OnDestroy()
        {
            //-> calling "Undo.RegisterCompleteObjectUndo(bezierSplineDrawer_thatHasReferencedThisGameobject, "undoName")" also doesn't solve the bug, that the connectionReference cannot be restored via "Editor/Undo" after this connection component has been deleted. But it at least prevents the bool-comparison-error described in "TryEarlyReturnAndSelfDeleteBecauseReferenceGotLost()". Though this error can also be prevented with an additional bool-check.
            //-> Moreover calling "Undo.RegisterCompleteObjectUndo(bezierSplineDrawer_thatHasReferencedThisGameobject, "undoName")" may lead to confusting undo states, because this connection component gets destroyed (and therewith this "OnDestroy()" here gets called) not only when the user deletes it (or the carrying boundGameobject), but also when the spline component gets destroyed (specifically this connection component is destroyed inside "OnDestroy()" of the spline component). So the spline most likely already has an "Undo.DestroyImmediate()" registered for itself through the spline deletion the was triggerd by manual control in the Editor. Then it would be saved into the undo state once more here. That seems to risky since the undo system can have obscure errors when used in such hacky ways, which can even crash the Unity Editor.
        }

        void Update()
        {
            if (TryDestroyThisComponentIfItWasManuallyCreated()) { return; }
            if (TryEarlyReturnAndSelfDeleteBecauseReferenceGotLost()) { return; }

            //No "register undo for spline" is done here before changed transform values get written to the spline:
            //-> It is unclear how this gameobject changed it's transform. Has the "changer" already filed an "Undo" or should it be filed here? If it has already been filed is it harmful when it is filed twice into the undo state?
            //-> It is not necessary: since the transform of this boundGameobject is authoritative for the spline shape, the spline shape will follow the undo as soon as the transform is reverted via an Undo.
            //-> So the changer of this transform is responsible for caring for the undo entry registration. In most cases the transform will be changed via the Scene view handles or the Transform inspector, in which cases Unity already automatically registers the Undo entry.

            if (Get_bezierSubPoint_thatHasReferencedThisGameobject().isUsed)
            {
                if (false == UtilitiesDXXL_Math.CheckIf_twoVectorsAreExactlyEqual((Vector2)transform.position, Get_bezierSubPoint_thatHasReferencedThisGameobject().GetPos_inUnitsOfGlobalSpace()))
                {
                    //Note: A position change of this bound gameobject has never effect on the direction of the control sub point, so this will not "come back via forwarding to dependent sub points" and set this gameobjects rotation.
                    Transfer_position_fromBoundGameobject_toSpline();
                }
            }

            if (subPointType_whereThisGameobjectIsBoundTo == InternalDXXL_BezierControlSubPoint.SubPointType.anchor)
            {
                if (Get_bezierSubPoint_thatHasReferencedThisGameobject().Get_sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked() != InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.independentFromGameobject)
                {
                    //-> only anchor points can arrive here, but no helper points
                    //-> anchor points cannot be unused (meaning "isUsed == true" is guarateed here)
                    if (false == UtilitiesDXXL_Math.CheckIf_twoQuaternionsAreExactlyEqual(transform.rotation, lastGlobalRotationOfThisGameobject_thatTheSplineKnowsOf))
                    {
                        //Note: A rotation change of this bound gameobject has never effect on the position of the control sub point, so this will not "come back via forwarding to dependent sub points" and set this gameobjects position.
                        Transfer_aTransformDirection_fromBoundGameobject_toSpline();
                    }
                }
            }
        }

        InternalDXXL_BezierControlSubPoint2D Get_bezierSubPoint_thatHasReferencedThisGameobject()
        {
            //Serialized lists containing other lists and containing custom classes with cross references don't work well in Unity, even when using the [SerializeReference] attribute. Functions like this try to keep the convenience of a real reference tree. The only "real" reference is the monobehavior-inherited spline component. Everything has to start from there.
            if (bezierSplineDrawer_thatHasReferencedThisGameobject != null)
            {
                if (i_ofControlPointTriplet_thisGameobjectIsBoundTo < bezierSplineDrawer_thatHasReferencedThisGameobject.listOfControlPointTriplets.Count)
                {
                    return bezierSplineDrawer_thatHasReferencedThisGameobject.listOfControlPointTriplets[i_ofControlPointTriplet_thisGameobjectIsBoundTo].GetASubPoint(subPointType_whereThisGameobjectIsBoundTo);
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

        public void Transfer_position_fromBoundGameobject_toSpline()
        {
            if (CheckIf_thisConnectionComponentIsActive())
            {
                Get_bezierSubPoint_thatHasReferencedThisGameobject().SetPos_inUnitsOfGlobalSpace(transform.position, true, this.gameObject);
            }
        }

        public void Transfer_aTransformDirection_fromBoundGameobject_toSpline()
        {
            if (CheckIf_thisConnectionComponentIsActive())
            {
                if (subPointType_whereThisGameobjectIsBoundTo != InternalDXXL_BezierControlSubPoint.SubPointType.anchor)
                {
                    UtilitiesDXXL_Log.PrintErrorCode("69");
                }

                InternalDXXL_BezierControlSubPoint2D anchorPoint = Get_bezierSubPoint_thatHasReferencedThisGameobject(); //-> only for code readability
                if (anchorPoint.GetJunctureType() == InternalDXXL_BezierControlAnchorSubPoint.JunctureType.kinked)
                {
                    if (anchorPoint.GetBackwardHelper().isUsed == true)
                    {
                        if (anchorPoint.GetBackwardHelper().sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture != InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.independentFromGameobject)
                        {
                            Vector2 newDirection_toBackwardHelper_inUnitsOfGlobalSpace_normalized = GetDirectionNormalizedFromRotation(anchorPoint.GetBackwardHelper().sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture);
                            anchorPoint.Set_direction_toBackward_inUnitsOfGlobalSpace_normalized(newDirection_toBackwardHelper_inUnitsOfGlobalSpace_normalized, true, this.gameObject);
                            lastGlobalRotationOfThisGameobject_thatTheSplineKnowsOf = transform.rotation;
                        }
                    }

                    if (anchorPoint.GetForwardHelper().isUsed == true)
                    {
                        if (anchorPoint.GetForwardHelper().sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture != InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.independentFromGameobject)
                        {
                            Vector2 newDirection_toForwardHelper_inUnitsOfGlobalSpace_normalized = GetDirectionNormalizedFromRotation(anchorPoint.GetForwardHelper().sourceOf_directionFromAnchorToThisHelper_caseKinkedJuncture);
                            anchorPoint.Set_direction_toForward_inUnitsOfGlobalSpace_normalized(newDirection_toForwardHelper_inUnitsOfGlobalSpace_normalized, true, this.gameObject);
                            lastGlobalRotationOfThisGameobject_thatTheSplineKnowsOf = transform.rotation;
                        }
                    }
                }
                else
                {
                    if (anchorPoint.Get_sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked() != InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.independentFromGameobject)
                    {
                        Vector2 newDirection_toForwardHelper_inUnitsOfGlobalSpace_normalized = GetDirectionNormalizedFromRotation(anchorPoint.Get_sourceOf_directionToHelper_unifiedTowardsForwardForCaseNonKinked());
                        anchorPoint.Set_direction_toForward_inUnitsOfGlobalSpace_normalized(newDirection_toForwardHelper_inUnitsOfGlobalSpace_normalized, true, this.gameObject);
                        lastGlobalRotationOfThisGameobject_thatTheSplineKnowsOf = transform.rotation;
                    }
                }
            }
        }

        Vector2 GetDirectionNormalizedFromRotation(InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D directionSource)
        {
            switch (directionSource)
            {
                case InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.independentFromGameobject:
                    UtilitiesDXXL_Log.PrintErrorCode("58");
                    return Vector2.right;
                case InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.gameobjectsUp:
                    return ConvertDirectionV3_toDirectionV2Normalized(transform.up);
                case InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.gameobjectsRight:
                    return ConvertDirectionV3_toDirectionV2Normalized(transform.right);
                case InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.gameobjectsDown:
                    return ConvertDirectionV3_toDirectionV2Normalized(-transform.up);
                case InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.gameobjectsLeft:
                    return ConvertDirectionV3_toDirectionV2Normalized(-transform.right);
                default:
                    UtilitiesDXXL_Log.PrintErrorCode("59");
                    return Vector2.right;
            }
        }

        Vector2 ConvertDirectionV3_toDirectionV2Normalized(Vector3 directionV3_toConvert)
        {
            Vector2 direction_projectedPerpOntoXYPlane = UtilitiesDXXL_DrawBasics2D.xyPlane_throughZero.Get_projectionOfVectorOntoPlane(directionV3_toConvert);
            Vector2 direction_projectedPerpOntoXYPlane_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(direction_projectedPerpOntoXYPlane);

            if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(direction_projectedPerpOntoXYPlane_normalized))
            {
                return Vector2.right;
            }
            else
            {
                return direction_projectedPerpOntoXYPlane_normalized;
            }
        }

        public void Transfer_newDirectionToAHelperPointInUnitsOfGlobalSpaceNormalized_fromSpline_toBoundGameobject(Vector2 newDirection_toAHelperPoint_inUnitsOfGlobalSpace_normalized, InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D directionSource_thatTheNewDirectionDescribes, GameObject boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls)
        {
            if (CheckIf_thisConnectionComponentIsActive())
            {
                if (subPointType_whereThisGameobjectIsBoundTo != InternalDXXL_BezierControlSubPoint.SubPointType.anchor)
                {
                    UtilitiesDXXL_Log.PrintErrorCode("70");
                }

                if (this.gameObject != boundGameobjectThatTriggeredThisPosOrDirChange_independentlyFromSplineControls) //-> this should prevent "continuous slow drifting of the values". The values the get set here could otherwise be converted somehow in the sub points and then be written back to the transform of this gameobject. The therewith calculated value can be slightly different (due to float calculation imprecision).
                {
                    switch (directionSource_thatTheNewDirectionDescribes)
                    {
                        case InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.independentFromGameobject:
                            UtilitiesDXXL_Log.PrintErrorCode("60");
                            break;
                        case InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.gameobjectsUp:
                            AssignNewTransformUp(newDirection_toAHelperPoint_inUnitsOfGlobalSpace_normalized);
                            break;
                        case InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.gameobjectsRight:
                            AssignNewTransformRight(newDirection_toAHelperPoint_inUnitsOfGlobalSpace_normalized);
                            break;
                        case InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.gameobjectsDown:
                            AssignNewTransformUp(-newDirection_toAHelperPoint_inUnitsOfGlobalSpace_normalized);
                            break;
                        case InternalDXXL_BezierControlAnchorSubPoint2D.SourceOf_directionToHelper2D.gameobjectsLeft:
                            AssignNewTransformRight(-newDirection_toAHelperPoint_inUnitsOfGlobalSpace_normalized);
                            break;
                        default:
                            break;
                    }
                    lastGlobalRotationOfThisGameobject_thatTheSplineKnowsOf = transform.rotation;
                }
            }
        }

        void AssignNewTransformUp(Vector2 newUp_normalized_insideXYPlane)
        {
            Vector3 newUp_normalized_insideXYPlane_asV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(newUp_normalized_insideXYPlane);
            Vector3 oldUpOfTransform_projectedOntoXYPlane = UtilitiesDXXL_DrawBasics2D.xyPlane_throughZero.Get_projectionOfVectorOntoPlane(transform.up);
            Vector3 oldUpOfTransform_projectedOntoXYPlane_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(oldUpOfTransform_projectedOntoXYPlane);

            if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(oldUpOfTransform_projectedOntoXYPlane_normalized) == false)
            {
                Quaternion rotationIncrement = Quaternion.FromToRotation(oldUpOfTransform_projectedOntoXYPlane_normalized, newUp_normalized_insideXYPlane_asV3);
                transform.rotation = rotationIncrement * transform.rotation;
            }
        }

        void AssignNewTransformRight(Vector2 newRight_normalized_insideXYPlane)
        {
            Vector3 newRight_normalized_insideXYPlane_asV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(newRight_normalized_insideXYPlane);
            Vector3 oldRightOfTransform_projectedOntoXYPlane = UtilitiesDXXL_DrawBasics2D.xyPlane_throughZero.Get_projectionOfVectorOntoPlane(transform.right);
            Vector3 oldRightOfTransform_projectedOntoXYPlane_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(oldRightOfTransform_projectedOntoXYPlane);

            if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(oldRightOfTransform_projectedOntoXYPlane_normalized) == false)
            {
                Quaternion rotationIncrement = Quaternion.FromToRotation(oldRightOfTransform_projectedOntoXYPlane_normalized, newRight_normalized_insideXYPlane_asV3);
                transform.rotation = rotationIncrement * transform.rotation;
            }
        }

        bool TryDestroyThisComponentIfItWasManuallyCreated()
        {
            if (componentHasBeenManuallyCreated)
            {
                Debug.LogError("'DrawXXLSpline2DConnection' should not be created manually. It will be automatically created and destroyed by the 'Bezier Spline Drawer 2D' component.");
                DestroyThisComponent(false);
                return true;
            }
            else
            {
                return false;
            }
        }

        bool TryEarlyReturnAndSelfDeleteBecauseReferenceGotLost()
        {
            if (bezierSplineDrawer_thatHasReferencedThisGameobject == null)
            {
                //-> "bezierSplineDrawer_thatHasReferencedThisGameobject" has been deleted
                //-> actually this component could also be deleted now, but the problem is: If the deletion of "bezierSplineDrawer_thatHasReferencedThisGameobject" is reverted via the editors "Undo"-functionality, then the retrieved bezierSplineDrawer doesn't have this spline connection anymore and the "undo" is not complete in this regard.
                //-> see also more detialled explantation in "subPoint.SetBoolOf_boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled()"
                //-> The connection component here stays inactive as long as it doesn't have a "bezierSplineDrawer_thatHasReferencedThisGameobject"

                DestroyThisComponent(false);
                return true;
            }
            else
            {
                InternalDXXL_BezierControlSubPoint2D bezierSubPoint = Get_bezierSubPoint_thatHasReferencedThisGameobject();
                if (bezierSubPoint == null)
                {
                    UtilitiesDXXL_Log.PrintErrorCode("76-" + bezierSplineDrawer_thatHasReferencedThisGameobject.listOfControlPointTriplets.Count + "-" + i_ofControlPointTriplet_thisGameobjectIsBoundTo);
                    DestroyThisComponent(false);
                    return true;
                }
                else
                {
                    if (bezierSubPoint.bezierSplineDrawer_thisSubPointIsPartOf != bezierSplineDrawer_thatHasReferencedThisGameobject)
                    {
                        UtilitiesDXXL_Log.PrintErrorCode("77-" + bezierSubPoint.bezierSplineDrawer_thisSubPointIsPartOf + "-" + bezierSplineDrawer_thatHasReferencedThisGameobject);
                        DestroyThisComponent(false);
                        return true;
                    }
                    else
                    {
                        if (bezierSubPoint.boundGameobject != this.gameObject)
                        {
                            //-> case 1: the gameobject that carries this connection-component has been copied.
                            //-> case 2: connection-component has been manually copied to another gameobject
                            //-> the connection gets deleted here. It stays only at the pre-copy-gameobject
                            //-> case 3: some "copy spline component -> undo -> redo" to-and-fro arrives here, see also
                            //-> see also more detialled explantation in "subPoint.SetBoolOf_boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled()"

                            DestroyThisComponent(false);
                            return true;
                        }
                        else
                        {
                            //The following "if (bezierSubPoint.connectionComponent_onBoundGameobject != this)"-check gives false evaluations in some cases.
                            //Observed case:
                            //-> Add boundGamobject(this) to a spline control point.
                            //-> Then delete the bound gameobject.
                            //-> Then the deletion via the editors "undo"-functionality. 
                            //-> Then "bezierSubPoint.connectionComponent_onBoundGameobject" is "null" (probably due to the Unity bug, see explanation in "subPoint.SetBoolOf_boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled()"
                            //-> Despite "bezierSubPoint.connectionComponent_onBoundGameobject" beeing "null" and "this" not beeing "null" the check "if (bezierSubPoint.connectionComponent_onBoundGameobject != this)" results in "is the same".
                            //-> It has probably to do with Unitys way of serialization and undo

                            if (bezierSubPoint.connectionComponent_onBoundGameobject != this)
                            {
                                //-> this connection component has been manually copied as duplicate to the same gameobject
                                //-> some delete/undo/redo-to and fro may also arrive here
                                DestroyThisComponent(false);
                                return true;
                            }
                            else
                            {
                                if (bezierSubPoint.connectionComponent_onBoundGameobject == null) //-> additional check as double bottom that fixes the comparison error described above
                                {
                                    DestroyThisComponent(false);
                                    return true;
                                }
                                else
                                {
                                    if (bezierSubPoint.i_ofContainingControlPoint_insideControlPointsList != i_ofControlPointTriplet_thisGameobjectIsBoundTo)
                                    {
                                        UtilitiesDXXL_Log.PrintErrorCode("78-" + bezierSplineDrawer_thatHasReferencedThisGameobject.listOfControlPointTriplets.Count + "-" + i_ofControlPointTriplet_thisGameobjectIsBoundTo + "-" + bezierSubPoint.i_ofContainingControlPoint_insideControlPointsList);
                                        DestroyThisComponent(false);
                                        return true;
                                    }
                                    else
                                    {
                                        if (bezierSubPoint.subPointType != subPointType_whereThisGameobjectIsBoundTo)
                                        {
                                            UtilitiesDXXL_Log.PrintErrorCode("79-" + bezierSplineDrawer_thatHasReferencedThisGameobject.listOfControlPointTriplets.Count + "-" + i_ofControlPointTriplet_thisGameobjectIsBoundTo + "-" + bezierSubPoint.subPointType + "-" + subPointType_whereThisGameobjectIsBoundTo);
                                            DestroyThisComponent(false);
                                            return true;
                                        }
                                        else
                                        {
                                            return false;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        void DestroyThisComponent(bool withDestructionUndo)
        {
            //Destroying only the component, but keeping the gameObject and all other components:
            if (Application.isPlaying)
            {
                Destroy(this);
            }
            else
            {
                if (withDestructionUndo)
                {
#if UNITY_EDITOR
                    UnityEditor.Undo.DestroyObjectImmediate(this);
#else
                    //How can the code arrive here?
                    UtilitiesDXXL_Log.PrintErrorCode("84-"+ Application.isPlaying);
                    Destroy(this);
#endif
                }
                else
                {
                    DestroyImmediate(this);
                }
            }
        }

        bool CheckIf_thisConnectionComponentIsActive()
        {
            if (bezierSplineDrawer_thatHasReferencedThisGameobject != null) //this is only due to the undo-mechanic-selfDestruction-delay
            {
                return isActiveAndEnabled;
            }
            return false;
        }

    }

}