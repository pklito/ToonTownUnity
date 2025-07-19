namespace DrawXXL
{
    using UnityEngine;

    public class UtilitiesDXXL_FlatShapesNormaAndUpCalculation
    {
        static InternalDXXL_Plane s_planeInWhichTextUpShouldLie = new InternalDXXL_Plane(); //doesn't have to contain the text, but can be parallel shifted

        public static void GetNormalAndUpInsidePlane(out Vector3 normal_final_notGuaranteedNormalized, out Vector3 upInsideFlatPlane_normalized, Vector3 normal_fromCaller, Vector3 upInsideFlatPlane_fromCaller, Vector3 shapePos)
        {
            //most callers don't care whether the normal points towardsCam or awayFromIt.
            //Exception: 'UtilitiesDXXL_DrawBasics.Icon()':
            //The normal of an icon is actually pointing towards the observer
            //Therefore the userDelivered normal gets flipped so it is a "forward (e.g.ofTheDefaulXYPlane)"
            //And this is what 'UtilitiesDXXL_DrawBasics.Icon()' expects: a "forward (e.g.ofTheDefaulXYPlane)"
            //This function here cares for this expectation and delivers this "forward (e.g.ofTheDefaulXYPlane)" (except for the "GetNormalAndUp_whileUserHas_notSpecifiedNormal_but_specifiedUp"-thread, where the flipInversion is undefined)

            bool normal_isUnspecified = UtilitiesDXXL_Math.IsDefaultVector(normal_fromCaller);
            bool upInsideFlatPlane_isUnspecified = UtilitiesDXXL_Math.IsDefaultVector(upInsideFlatPlane_fromCaller);

            if (normal_isUnspecified && upInsideFlatPlane_isUnspecified)
            {
                //both "normal" and "up" are unspecified:
                GetNormalAndUp_withoutAnyUserSpecification(out normal_final_notGuaranteedNormalized, out upInsideFlatPlane_normalized, shapePos);
            }
            else
            {
                if ((normal_isUnspecified == false) && (upInsideFlatPlane_isUnspecified == false))
                {
                    //both "normal" and "up" are specified:
                    NormalizeAndForcePerp_userSpecifiedNonDefaultNormalAndUp(out normal_final_notGuaranteedNormalized, out upInsideFlatPlane_normalized, normal_fromCaller, upInsideFlatPlane_fromCaller);
                }
                else
                {
                    if (upInsideFlatPlane_isUnspecified)
                    {
                        //"normal" is specified:
                        //"up" is unspecified:
                        GetNormalAndUp_whileUserHas_specifiedNormal_but_notSpecifiedUp(out normal_final_notGuaranteedNormalized, out upInsideFlatPlane_normalized, normal_fromCaller);
                    }
                    else
                    {
                        // "normal" is unspecified:
                        // "up" is specified:
                        GetNormalAndUp_whileUserHas_notSpecifiedNormal_but_specifiedUp(out normal_final_notGuaranteedNormalized, out upInsideFlatPlane_normalized, upInsideFlatPlane_fromCaller, shapePos);
                    }
                }
            }
        }

        static void GetNormalAndUp_withoutAnyUserSpecification(out Vector3 normal_final_notGuaranteedNormalized, out Vector3 upInsideFlatPlane_normalized, Vector3 shapePos)
        {
            Vector3 observerCamForward_normalized;
            Vector3 observerCamUp_normalized;
            Vector3 observerCamRight_normalized;
            Vector3 cam_to_lineCenter;

            switch (DrawShapes.automaticOrientationOfFlatShapes)
            {
                case DrawShapes.AutomaticOrientationOfFlatShapes.screen:
                    UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter, shapePos, Vector3.zero, null);
                    normal_final_notGuaranteedNormalized = observerCamForward_normalized;
                    //normal_final_notGuaranteedNormalized = cam_to_lineCenter; //-> seems to make no difference
                    upInsideFlatPlane_normalized = observerCamUp_normalized;
                    return;
                case DrawShapes.AutomaticOrientationOfFlatShapes.screen_butVerticalInWorldSpace:
                    UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter, shapePos, Vector3.zero, null);
                    UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.GetUpAndTextDir_withoutCallerSpecifiedPreference_independentFromTooShortLineDir_alignedVertical(out Vector3 textUp_normalized, out Vector3 textDir_normalized, observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                    normal_final_notGuaranteedNormalized = Vector3.Cross(textDir_normalized, textUp_normalized);
                    upInsideFlatPlane_normalized = textUp_normalized;
                    return;
                case DrawShapes.AutomaticOrientationOfFlatShapes.xyPlane:
                    normal_final_notGuaranteedNormalized = Vector3.forward;
                    upInsideFlatPlane_normalized = Vector3.up;
                    return;
                case DrawShapes.AutomaticOrientationOfFlatShapes.xzPlane:
                    normal_final_notGuaranteedNormalized = Vector3.down;
                    upInsideFlatPlane_normalized = Vector3.forward;
                    return;
                case DrawShapes.AutomaticOrientationOfFlatShapes.zyPlane:
                    normal_final_notGuaranteedNormalized = Vector3.right;
                    upInsideFlatPlane_normalized = Vector3.up;
                    return;
                default:
                    Debug.LogError("DrawShapes.automaticOrientationOfFlatShapes of " + DrawShapes.automaticOrientationOfFlatShapes + " not implemented.");
                    normal_final_notGuaranteedNormalized = Vector3.forward;
                    upInsideFlatPlane_normalized = Vector3.up;
                    return;
            }
        }

        static void GetNormalAndUp_whileUserHas_specifiedNormal_but_notSpecifiedUp(out Vector3 normal_final_notGuaranteedNormalized, out Vector3 upInsideFlatPlane_normalized, Vector3 normal_fromCaller)
        {
            NormalizeAndForcePerp_userSpecifiedNonDefaultNormalAndUp(out normal_final_notGuaranteedNormalized, out upInsideFlatPlane_normalized, normal_fromCaller, Vector3.up);
        }

        static InternalDXXL_Plane s_planeInWhichNormalShouldLie = new InternalDXXL_Plane();
        static void GetNormalAndUp_whileUserHas_notSpecifiedNormal_but_specifiedUp(out Vector3 normal_final_notGuaranteedNormalized, out Vector3 upInsideFlatPlane_normalized, Vector3 upInsideFlatPlane_fromCaller, Vector3 shapePos)
        {
            upInsideFlatPlane_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(upInsideFlatPlane_fromCaller);
            s_planeInWhichNormalShouldLie.Recreate(Vector3.zero, upInsideFlatPlane_normalized);

            UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out Vector3 observerCamForward_normalized, out Vector3 observerCamUp_normalized, out Vector3 observerCamRight_normalized, out Vector3 cam_to_lineCenter, shapePos, Vector3.zero, null);
            InternalDXXL_Plane flatPlane_theWouldContainTheShape_accordingTo_DrawShapesAutomaticOrientationOfFlatShapesSetting = GetFlatPlane_thatContainsTheShape_accordingTo_DrawShapesAutomaticOrientationOfFlatShapesSetting(observerCamForward_normalized, false);
            Vector3 normalFromAutomaticOrientation_projectedOnto_planeInWhichNormalShouldLie = s_planeInWhichNormalShouldLie.Get_projectionOfVectorOntoPlane(flatPlane_theWouldContainTheShape_accordingTo_DrawShapesAutomaticOrientationOfFlatShapesSetting.normalDir);
            normalFromAutomaticOrientation_projectedOnto_planeInWhichNormalShouldLie = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(normalFromAutomaticOrientation_projectedOnto_planeInWhichNormalShouldLie);

            if (UtilitiesDXXL_Math.CheckIfScaleToFloatPrecisionRegionFailed_meaningLineStayedTooShort(normalFromAutomaticOrientation_projectedOnto_planeInWhichNormalShouldLie))
            {
                //-> "upInsideFlatPlane_fromCaller" is perp to the wanted planeNormal (e.g. alongCamViewDir):
                normal_final_notGuaranteedNormalized = UtilitiesDXXL_Math.Get_aNormalizedVector_perpToGivenVector(upInsideFlatPlane_normalized);
            }
            else
            {
                normal_final_notGuaranteedNormalized = normalFromAutomaticOrientation_projectedOnto_planeInWhichNormalShouldLie;
            }
        }

        static InternalDXXL_Plane GetFlatPlane_thatContainsTheShape_accordingTo_DrawShapesAutomaticOrientationOfFlatShapesSetting(Vector3 observerCamForward_normalized, bool normalizePlaneNormal)
        {
            switch (DrawShapes.automaticOrientationOfFlatShapes)
            {
                case DrawShapes.AutomaticOrientationOfFlatShapes.screen:
                    s_planeInWhichTextUpShouldLie.Recreate(Vector3.zero, observerCamForward_normalized); //the plane could also be perp to "cam_to_lineCenter", but some of the already unintuitive cases get worse then
                    return s_planeInWhichTextUpShouldLie;
                case DrawShapes.AutomaticOrientationOfFlatShapes.screen_butVerticalInWorldSpace:
                    return GetFlatPlane_ifAutomaticTextOrientationSettingIs_screen_butVerticalInWorldSpace(observerCamForward_normalized, normalizePlaneNormal);
                case DrawShapes.AutomaticOrientationOfFlatShapes.xyPlane:
                    return InternalDXXL_Plane.xyPlane_throughZeroOrigin;
                case DrawShapes.AutomaticOrientationOfFlatShapes.xzPlane:
                    return InternalDXXL_Plane.horizPlane_throughZeroOrigin;
                case DrawShapes.AutomaticOrientationOfFlatShapes.zyPlane:
                    return InternalDXXL_Plane.zyPlane_throughZeroOrigin;
                default:
                    Debug.LogError("DrawShapes.automaticOrientationOfFlatShapes of " + DrawShapes.automaticOrientationOfFlatShapes + " not implemented.");
                    return InternalDXXL_Plane.xyPlane_throughZeroOrigin;
            }
        }

        static InternalDXXL_Plane GetFlatPlane_ifAutomaticTextOrientationSettingIs_screen_butVerticalInWorldSpace(Vector3 observerCamForward_normalized, bool normalizePlaneNormal)
        {
            Vector3 normal_ofVertPlane_thatIsAlignedToObserverCam_potentiallyZero = InternalDXXL_Plane.horizPlane_throughZeroOrigin.Get_projectionOfVectorOntoPlane(observerCamForward_normalized); //the projection could also be made from "cam_to_lineCenter", but some of the already unintuitive cases get worse then
            Vector3 normal_ofVertPlane_thatIsAlignedToObserverCam_lengthIsMinApprox1_butCanBeVeryLong = UtilitiesDXXL_Math.ScaleNonZeroVectorToApproxBiggerThanMinLength(normal_ofVertPlane_thatIsAlignedToObserverCam_potentiallyZero, 1.0f);
            if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(normal_ofVertPlane_thatIsAlignedToObserverCam_lengthIsMinApprox1_butCanBeVeryLong))
            {
                //camera is looking along vertical y-axis
                return InternalDXXL_Plane.horizPlane_throughZeroOrigin;
            }
            else
            {
                if (normalizePlaneNormal)
                {
                    normal_ofVertPlane_thatIsAlignedToObserverCam_lengthIsMinApprox1_butCanBeVeryLong = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(normal_ofVertPlane_thatIsAlignedToObserverCam_lengthIsMinApprox1_butCanBeVeryLong);
                }
                s_planeInWhichTextUpShouldLie.Recreate(Vector3.zero, normal_ofVertPlane_thatIsAlignedToObserverCam_lengthIsMinApprox1_butCanBeVeryLong);
                return s_planeInWhichTextUpShouldLie;
            }
        }

        static InternalDXXL_Plane aPlane_parallelToPolygonPlane = new InternalDXXL_Plane();
        static void NormalizeAndForcePerp_userSpecifiedNonDefaultNormalAndUp(out Vector3 normal_final_notGuaranteedNormalized, out Vector3 upInsideFlatPlane_normalized, Vector3 normal_fromCaller, Vector3 upInsideFlatPlane_fromCaller)
        {
            aPlane_parallelToPolygonPlane.Recreate(Vector3.zero, normal_fromCaller);
            normal_final_notGuaranteedNormalized = aPlane_parallelToPolygonPlane.normalDir; //-> is now treated with "ScaleNonZeroVectorIntoRegionOfFloatPrecision" (whhich happens inside the plane.Recreate())
            upInsideFlatPlane_fromCaller = UtilitiesDXXL_Shapes.ForceVectorPerpToOtherVector(upInsideFlatPlane_fromCaller, aPlane_parallelToPolygonPlane);
            if (UtilitiesDXXL_Math.Check_ifTwoVectorsAreApproxParallel_butCanHeadToDifferntDirs_DXXL(upInsideFlatPlane_fromCaller, normal_final_notGuaranteedNormalized))
            {
                upInsideFlatPlane_normalized = UtilitiesDXXL_Math.Get_aNormalizedVector_perpToGivenVector(normal_final_notGuaranteedNormalized, aPlane_parallelToPolygonPlane);
            }
            else
            {
                upInsideFlatPlane_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(upInsideFlatPlane_fromCaller);
            }
        }

        public static Quaternion GetQuaternion(Quaternion quaternion_fromCaller, Vector3 position_ofDrawnShape)
        {
            if (UtilitiesDXXL_Math.IsDefaultInvalidQuaternion(quaternion_fromCaller))
            {
                GetNormalAndUpInsidePlane(out Vector3 forward_final_notGuaranteedNormalized, out Vector3 upInsideIconPlane_normalized, default(Vector3), default(Vector3), position_ofDrawnShape);
                return Quaternion.LookRotation(forward_final_notGuaranteedNormalized, upInsideIconPlane_normalized);
            }
            else
            {
                return quaternion_fromCaller;
            }
        }

    }

}
