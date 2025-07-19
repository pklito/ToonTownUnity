namespace DrawXXL
{
    using UnityEngine;

    public class UtilitiesDXXL_TextDirAndUpCalculation
    {
        static InternalDXXL_Plane s_planeInWhichTextUpShouldLie = new InternalDXXL_Plane(); //doesn't have to contain the text, but can be parallel shifted
        static InternalDXXL_Line intersectionLine_ofTwoPlanes = new InternalDXXL_Line();
        static InternalDXXL_Plane aPlanePerpToTextDir = new InternalDXXL_Plane();
        static InternalDXXL_Plane aPlanePerpToUpDir = new InternalDXXL_Plane();

        public static void GetTextDirAndUpNormalized(out Vector3 textDir_normalized, out Vector3 textUp_normalized, Vector3 textDir_fromCaller, Vector3 textUp_fromCaller, Vector3 textPos, bool isFrom_Write2D, bool dirAndUp_areAlreadyGuaranteed_perpAndNormalized)
        {
            if (dirAndUp_areAlreadyGuaranteed_perpAndNormalized)
            {
                //-> calls from WriteScreenspace always arrive here
                textDir_normalized = textDir_fromCaller;
                textUp_normalized = textUp_fromCaller;
            }
            else
            {
                bool textDir_isUnspecified = UtilitiesDXXL_Math.IsDefaultVector(textDir_fromCaller);
                bool textUp_isUnspecified = UtilitiesDXXL_Math.IsDefaultVector(textUp_fromCaller);

                if (textDir_isUnspecified && textUp_isUnspecified)
                {
                    //both "dir" and "up" are unspecified:
                    GetTextDirAndUpNormalized_withoutAnyUserSpecification(out textDir_normalized, out textUp_normalized, textPos, isFrom_Write2D);
                }
                else
                {
                    if ((textDir_isUnspecified == false) && (textUp_isUnspecified == false))
                    {
                        //both "dir" and "up" are specified:
                        //-> "isFrom_Write2D" has no effect here, except for the fallback_caseTextAndUpAreParallel
                        //-> "textPos" has no effect here, except for the fallback_caseTextAndUpAreParallel
                        NormalizeAndForcePerp_userSpecifiedNonDefaultDirAndUp(out textDir_normalized, out textUp_normalized, textDir_fromCaller, textUp_fromCaller, textPos, isFrom_Write2D);
                    }
                    else
                    {
                        if (textUp_isUnspecified)
                        {
                            //"dir" is specified:
                            //"up" is unspecified:
                            GetTextDirAndUpNormalized_whileUserHas_specifiedDir_but_notSpecifiedUp(out textDir_normalized, out textUp_normalized, textDir_fromCaller, textPos, isFrom_Write2D);
                        }
                        else
                        {
                            // "dir" is unspecified:
                            // "up" is specified:
                            GetTextDirAndUpNormalized_whileUserHas_notSpecifiedDir_but_specifiedUp(out textDir_normalized, out textUp_normalized, textUp_fromCaller, textPos, isFrom_Write2D);
                        }
                    }
                }
            }
        }

        static void GetTextDirAndUpNormalized_withoutAnyUserSpecification(out Vector3 textDir_normalized, out Vector3 textUp_normalized, Vector3 textPos, bool isFrom_Write2D)
        {
            if (isFrom_Write2D)
            {
                textDir_normalized = Vector3.right;
                textUp_normalized = Vector3.up;
            }
            else
            {
                Vector3 observerCamForward_normalized;
                Vector3 observerCamUp_normalized;
                Vector3 observerCamRight_normalized;
                Vector3 cam_to_lineCenter;

                switch (DrawText.automaticTextOrientation)
                {
                    case DrawText.AutomaticTextOrientation.screen:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter, textPos, Vector3.zero, null);
                        textDir_normalized = observerCamRight_normalized;
                        textUp_normalized = observerCamUp_normalized;
                        return;
                    case DrawText.AutomaticTextOrientation.screen_butVerticalInWorldSpace:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter, textPos, Vector3.zero, null);
                        UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.GetUpAndTextDir_withoutCallerSpecifiedPreference_independentFromTooShortLineDir_alignedVertical(out textUp_normalized, out textDir_normalized, observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                        return;
                    case DrawText.AutomaticTextOrientation.xyPlane:
                        textDir_normalized = Vector3.right;
                        textUp_normalized = Vector3.up;
                        return;
                    case DrawText.AutomaticTextOrientation.xzPlane:
                        textDir_normalized = Vector3.right;
                        textUp_normalized = Vector3.forward;
                        return;
                    case DrawText.AutomaticTextOrientation.zyPlane:
                        textDir_normalized = Vector3.forward;
                        textUp_normalized = Vector3.up;
                        return;
                    default:
                        Debug.LogError("DrawText.AutomaticTextOrientation of " + DrawText.automaticTextOrientation + " not implemented.");
                        textDir_normalized = Vector3.right;
                        textUp_normalized = Vector3.up;
                        return;
                }
            }
        }

        static void GetTextDirAndUpNormalized_whileUserHas_specifiedDir_but_notSpecifiedUp(out Vector3 textDir_normalized, out Vector3 textUp_normalized, Vector3 textDir_fromCaller, Vector3 textPos, bool isFrom_Write2D)
        {
            //-> may look unintuitive for perspective cameras
            //-> the current implementation (explained with the example of "automaticTextOrientation==screen"):
            //---> tries to put the upVector into the screenParallelPlane
            //---> this is probably what the name "(automaticTextOrientation==)screen" implies: The dir cannot be forced into this plane, because it is user-specified, so let at least force everything else (=the upVector) into this plane, so that (since you cannot force EVERYTHING into this plane) at least as much as possible results inside this plane.
            //---> But: this doesn't result in the maximum readabilty for the screen-viewpoint (and that may "actually" be the meaning of "(automaticTextOrientation==)screen": clearest, most-unwarped readabilty from screen-viewpoint)
            //---> Because: The upVector_afterForcingIntoScreenPlane may come out es very parallel to the dirVector (when seen from the screen-viewpoint), so the text can be strongly sheared and therfore unreadable.
            //-> A refactoring could try this: 
            //---> Use the upVector, that is not necessarily in the screenParallelPlane, but is perp_toDirVector "from the screen viewpoint perspective".
            //---> The refactoring should keep in mind that for perspective cameras this "perp_toDirVector_fromScreenViewPoint"-direction is different depending wheather the textStartPosition or the textEndPosition is chosen as the point where the perpendicularity should be. (also: one pos could be in front of the camera, the other pos behind the camera)

            textDir_fromCaller = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(textDir_fromCaller);
            UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out Vector3 observerCamForward_normalized, out Vector3 observerCamUp_normalized, out Vector3 observerCamRight_normalized, out Vector3 cam_to_lineCenter, textPos, textDir_fromCaller, null);
            InternalDXXL_Plane planeInWhichTextShouldLie_ifNoTextAndNoUpAreSpecified_normalIsNormalized = GetPlane_thatContainsTheText_accordingTo_DrawTextAutomaticTextOrientationSetting_ifNoTextAndNoUpAreSpecified(observerCamForward_normalized, false, isFrom_Write2D);

            DrawBasics.AutomaticTextDirectionOfLines automaticTextDirectionOfLines_before = DrawBasics.automaticTextDirectionOfLines;
            try
            {
                DrawBasics.automaticTextDirectionOfLines = DrawBasics.AutomaticTextDirectionOfLines.towardsLineEnd;
                UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.GetUpAndTextDir_insideAmplitudePlane_forNonShortLine(out textUp_normalized, out textDir_normalized, out float lengthOfDrawnLine, out bool lengthOfDrawnLine_isFilled, textPos, textDir_fromCaller, planeInWhichTextShouldLie_ifNoTextAndNoUpAreSpecified_normalIsNormalized, true, false, null, observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter, false);
            }
            catch
            {
                UtilitiesDXXL_Log.PrintErrorCode("23-" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(textDir_fromCaller) + "-" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(textPos) + "-" + DrawText.automaticTextOrientation);
                textDir_normalized = Vector3.right;
                textUp_normalized = Vector3.up;
            }
            DrawBasics.automaticTextDirectionOfLines = automaticTextDirectionOfLines_before;
        }

        static void GetTextDirAndUpNormalized_whileUserHas_notSpecifiedDir_but_specifiedUp(out Vector3 textDir_normalized, out Vector3 textUp_normalized, Vector3 textUp_fromCaller, Vector3 textPos, bool isFrom_Write2D)
        {
            //-> amplitudeUpDir is specified
            //-> AND
            //-> amplitudePlane is specified
            //This case is not yet implemented in "UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.GetUpAndTextDir_accordingToSubCaseSpecification()": 
            //Note that amplitudeUpDir may or may not be inside the amplitudePlane
            //The implementation here only cares for the case "textDir=undefinedShort"

            //-> may look unintuitive for perspective cameras (see explanation in "GetTextDirAndUpNormalized_whileUserHas_specifiedDir_but_notSpecifiedUp")

            textUp_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(textUp_fromCaller);
            aPlanePerpToUpDir.Recreate(Vector3.zero, textUp_normalized);
            UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out Vector3 observerCamForward_normalized, out Vector3 observerCamUp_normalized, out Vector3 observerCamRight_normalized, out Vector3 cam_to_lineCenter, textPos, Vector3.zero, null);
            InternalDXXL_Plane planeInWhichTextShouldLie_ifNoTextAndNoUpAreSpecified_normalIsNormalized = GetPlane_thatContainsTheText_accordingTo_DrawTextAutomaticTextOrientationSetting_ifNoTextAndNoUpAreSpecified(observerCamForward_normalized, true, isFrom_Write2D); //-> parameter forces normalizing: it's a seldom case, so no further optimizing, though also approxNormalized would be sufficient for the following parallelCheck

            if (UtilitiesDXXL_Math.Check_ifTwoNormalizedVectorsAreApproxParallel_butCanHeadToDifferntDirs_padding(planeInWhichTextShouldLie_ifNoTextAndNoUpAreSpecified_normalIsNormalized.normalDir, aPlanePerpToUpDir.normalDir))
            {
                //user-specified upDir is perp to plane that "DrawText.automaticTextOrientation" specifies
                //-> textDir cannot be obtained via intersection of the two planes
                //-> EVERY textDir is valid, as long as it lies inside the (parallel)plane(s)
                //-> EVERY dir inside the (parallel)plane(s) fulfils both criteria:
                //---> is perp to upDir
                //---> is inside the plane that is wanted by "DrawText.automaticTextOrientation"
                //-> so, perpToUpPlane can be ignored, and the dirVector can be restricted by only the "DrawText.automaticTextOrientation"-wanted plane (the planes are parallel, meaning exchangable)
                //-> fallback to "GetTextDirAndUpNormalized_withoutAnyUserSpecification()", but only for textDir. UpDir stays as user-specified
                GetTextDirAndUpNormalized_withoutAnyUserSpecification(out textDir_normalized, out Vector3 unused_textUp_normalized, textPos, isFrom_Write2D);
            }
            else
            {
                InternalDXXL_Plane.Calc_intersectionLine_ofTwoPlanes(ref intersectionLine_ofTwoPlanes, planeInWhichTextShouldLie_ifNoTextAndNoUpAreSpecified_normalIsNormalized, aPlanePerpToUpDir);

                DrawBasics.AutomaticTextDirectionOfLines automaticTextDirectionOfLines_before = DrawBasics.automaticTextDirectionOfLines;
                try
                {
                    DrawBasics.automaticTextDirectionOfLines = DrawBasics.AutomaticTextDirectionOfLines.leftToRightInScreen;
                    UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.FlipNormalizedTextDir_toBeNonMirroredReadable_forFixedAmplitudeUpDir(out textDir_normalized, intersectionLine_ofTwoPlanes.direction_normalized, textUp_normalized, cam_to_lineCenter);
                }
                catch
                {
                    UtilitiesDXXL_Log.PrintErrorCode("24-" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(intersectionLine_ofTwoPlanes.direction_normalized) + "-" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(textUp_normalized) + "-" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(cam_to_lineCenter) + DrawText.automaticTextOrientation);
                    textDir_normalized = Vector3.right; //may be parallel to 'textUp'
                }
                DrawBasics.automaticTextDirectionOfLines = automaticTextDirectionOfLines_before;
            }
        }

        static InternalDXXL_Plane GetPlane_thatContainsTheText_accordingTo_DrawTextAutomaticTextOrientationSetting_ifNoTextAndNoUpAreSpecified(Vector3 observerCamForward_normalized, bool normalizePlaneNormal, bool isFrom_Write2D)
        {
            if (isFrom_Write2D)
            {
                return InternalDXXL_Plane.xyPlane_throughZeroOrigin;
            }
            else
            {
                switch (DrawText.automaticTextOrientation)
                {
                    case DrawText.AutomaticTextOrientation.screen:
                        s_planeInWhichTextUpShouldLie.Recreate(Vector3.zero, observerCamForward_normalized); //the plane could also be perp to "cam_to_lineCenter", but some of the already unintuitive cases get worse then
                        return s_planeInWhichTextUpShouldLie;
                    case DrawText.AutomaticTextOrientation.screen_butVerticalInWorldSpace:
                        return GetPlane_thatContainsTheText_ifAutomaticTextOrientationSettingIs_screen_butVerticalInWorldSpace(observerCamForward_normalized, normalizePlaneNormal);
                    case DrawText.AutomaticTextOrientation.xyPlane:
                        return InternalDXXL_Plane.xyPlane_throughZeroOrigin;
                    case DrawText.AutomaticTextOrientation.xzPlane:
                        return InternalDXXL_Plane.horizPlane_throughZeroOrigin;
                    case DrawText.AutomaticTextOrientation.zyPlane:
                        return InternalDXXL_Plane.zyPlane_throughZeroOrigin;
                    default:
                        Debug.LogError("DrawText.AutomaticTextOrientation of " + DrawText.automaticTextOrientation + " not implemented.");
                        return InternalDXXL_Plane.xyPlane_throughZeroOrigin;
                }
            }
        }

        static InternalDXXL_Plane GetPlane_thatContainsTheText_ifAutomaticTextOrientationSettingIs_screen_butVerticalInWorldSpace(Vector3 observerCamForward_normalized, bool normalizePlaneNormal)
        {
            Vector3 normal_ofVertPlane_thatIsAlignedToObserverCam_potentiallyZero = InternalDXXL_Plane.horizPlane_throughZeroOrigin.Get_projectionOfVectorOntoPlane(observerCamForward_normalized); //the projection could also be made from "cam_to_lineCenter", but some of the already unintuitive cases get worse then
            Vector3 normal_ofVertPlane_thatIsAlignedToObserverCam_lengthIsMinApprox1_butCanBeVeryLong = UtilitiesDXXL_Math.ScaleNonZeroVectorToApproxBiggerThanMinLength(normal_ofVertPlane_thatIsAlignedToObserverCam_potentiallyZero, 1.0f);
            if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(normal_ofVertPlane_thatIsAlignedToObserverCam_lengthIsMinApprox1_butCanBeVeryLong))
            {
                //camera is looking along vertical y-axis
                //-> fallback to draw inside horizPlane
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

        static void NormalizeAndForcePerp_userSpecifiedNonDefaultDirAndUp(out Vector3 textDir_normalized, out Vector3 textUp_normalized, Vector3 textDir_fromCaller, Vector3 textUp_fromCaller, Vector3 textPos, bool isFrom_Write2D)
        {
            textDir_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(textDir_fromCaller);
            Vector3 textUp_fromCaller_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(textUp_fromCaller);
            float absDotProduct_ofDirNormalized_andUpFromCallerNormalized = Mathf.Abs(Vector3.Dot(textDir_normalized, textUp_fromCaller_normalized));
            if (absDotProduct_ofDirNormalized_andUpFromCallerNormalized < UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.absDotProductResult_ofTwoApproxNormalizedVectors_belowWhichVectorsAreConsideredPerp)
            {
                //user-specified dir and up have already been perp:
                textUp_normalized = textUp_fromCaller_normalized;
            }
            else
            {
                aPlanePerpToTextDir.Recreate(Vector3.zero, textDir_normalized);
                Vector3 up_norNormalized = aPlanePerpToTextDir.Get_projectionOfVectorOntoPlane(textUp_fromCaller_normalized);
                Vector3 up_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(up_norNormalized);
                if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(up_normalized))
                {
                    //Debug.Log("Ambiguous: 'textDir' (" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(textDir_fromCaller) + ") and 'textUp' (" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(textUp_fromCaller) + ") are approximately parallel -> now using auto-obtained 'textUp' as fallback.");
                    GetTextDirAndUpNormalized_whileUserHas_specifiedDir_but_notSpecifiedUp(out textDir_normalized, out textUp_normalized, textDir_normalized, textPos, isFrom_Write2D);
                }
                else
                {
                    textUp_normalized = up_normalized;
                }
            }
        }

        public static void TryAutoFlipScreenspaceTextToPreventUpsideDown(out DrawText.TextAnchorDXXL textAnchor_postFlip, out Vector3 textDir_worldSpace_normalized_postFlip, out Vector3 textUp_worldSpace_normalized_postFlip, DrawText.TextAnchorDXXL textAnchor_preFlip, Vector3 textDir_worldSpace_normalized_preFlip, Vector3 textUp_worldSpace_normalized_preFlip, bool autoFlipTextToPreventUpsideDown, Camera screenspaceCamera)
        {
            //"autoFlipTextToPreventUpsideDown" has a separate implementation here (compared to "UtilitiesDXXL_LineAmplitudeAndTextDirCalculation"), because it seems easier for the user to learn only the one bool instead of the whole "DrawBasics.automaticAlignmentEnums"
            //Moreover: this does not flip "mirrorInversion" but "upsideDown"

            if (autoFlipTextToPreventUpsideDown)
            {
                float dotProduct_ofTextUp_and_camUp = Vector3.Dot(textUp_worldSpace_normalized_preFlip, screenspaceCamera.transform.up);
                float absDotProduct_ofTextUp_and_camUp = Mathf.Abs(dotProduct_ofTextUp_and_camUp);
                bool textIsApproxVertInsideScreen = (absDotProduct_ofTextUp_and_camUp < UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.absDotProductResult_ofTwoApproxNormalizedVectors_belowWhichVectorsAreConsideredPerp);
                if (textIsApproxVertInsideScreen)
                {
                    textAnchor_postFlip = textAnchor_preFlip;
                    float dotProduct_ofTextDir_and_camUp = Vector3.Dot(textDir_worldSpace_normalized_preFlip, screenspaceCamera.transform.up);
                    if (dotProduct_ofTextDir_and_camUp > 0.0f)
                    {
                        textDir_worldSpace_normalized_postFlip = screenspaceCamera.transform.up;
                        textUp_worldSpace_normalized_postFlip = (-screenspaceCamera.transform.right);
                    }
                    else
                    {
                        textDir_worldSpace_normalized_postFlip = (-screenspaceCamera.transform.up);
                        textUp_worldSpace_normalized_postFlip = screenspaceCamera.transform.right;
                    }
                }
                else
                {
                    if (dotProduct_ofTextUp_and_camUp < 0.0f)
                    {
                        textAnchor_postFlip = GetHorizMirroredTextAnchor(textAnchor_preFlip);
                        textDir_worldSpace_normalized_postFlip = (-textDir_worldSpace_normalized_preFlip);
                        textUp_worldSpace_normalized_postFlip = (-textUp_worldSpace_normalized_preFlip);
                    }
                    else
                    {
                        ReturnUnchangedScreenspaceTextSpecs(out textAnchor_postFlip, out textDir_worldSpace_normalized_postFlip, out textUp_worldSpace_normalized_postFlip, textAnchor_preFlip, textDir_worldSpace_normalized_preFlip, textUp_worldSpace_normalized_preFlip);
                    }
                }
            }
            else
            {
                ReturnUnchangedScreenspaceTextSpecs(out textAnchor_postFlip, out textDir_worldSpace_normalized_postFlip, out textUp_worldSpace_normalized_postFlip, textAnchor_preFlip, textDir_worldSpace_normalized_preFlip, textUp_worldSpace_normalized_preFlip);
            }
        }

        static void ReturnUnchangedScreenspaceTextSpecs(out DrawText.TextAnchorDXXL textAnchor_postFlip, out Vector3 textDir_worldSpace_normalized_postFlip, out Vector3 textUp_worldSpace_normalized_postFlip, DrawText.TextAnchorDXXL textAnchor_preFlip, Vector3 textDir_worldSpace_normalized_preFlip, Vector3 textUp_worldSpace_normalized_preFlip)
        {
            textAnchor_postFlip = textAnchor_preFlip;
            textDir_worldSpace_normalized_postFlip = textDir_worldSpace_normalized_preFlip;
            textUp_worldSpace_normalized_postFlip = textUp_worldSpace_normalized_preFlip;
        }

        public static void TryAutoFlipStraightTextToPreventMirrorInverted(out DrawText.TextAnchorDXXL textAnchor_postFlip, out Vector3 textDir_normalized_postFlip, out Vector3 textUp_normalized_postFlip, out Vector3 forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip, DrawText.TextAnchorDXXL textAnchor_preFlip, Vector3 textDir_normalized_preFlip, Vector3 textUp_normalized_preFlip, Vector3 forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip, Vector3 textPos, bool autoFlipToPreventMirrorInverted)
        {
            if (autoFlipToPreventMirrorInverted)
            {
                bool textAppearsMirrorInvertedToObserverCam = CheckIf_textAppearsMirrorInvertedToObserverCam(textPos, forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip);
                if (textAppearsMirrorInvertedToObserverCam)
                {
                    textAnchor_postFlip = GetHorizMirroredTextAnchor(textAnchor_preFlip);
                    textDir_normalized_postFlip = (-textDir_normalized_preFlip);
                    textUp_normalized_postFlip = textUp_normalized_preFlip;
                    forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip = (-forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip);
                }
                else
                {
                    ReturnUnchangedTextSpecs_forStraightText(out textAnchor_postFlip, out textDir_normalized_postFlip, out textUp_normalized_postFlip, out forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip, textAnchor_preFlip, textDir_normalized_preFlip, textUp_normalized_preFlip, forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip);
                }
            }
            else
            {
                ReturnUnchangedTextSpecs_forStraightText(out textAnchor_postFlip, out textDir_normalized_postFlip, out textUp_normalized_postFlip, out forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip, textAnchor_preFlip, textDir_normalized_preFlip, textUp_normalized_preFlip, forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip);
            }
        }

        static void ReturnUnchangedTextSpecs_forStraightText(out DrawText.TextAnchorDXXL textAnchor_postFlip, out Vector3 textDir_normalized_postFlip, out Vector3 textUp_normalized_postFlip, out Vector3 forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip, DrawText.TextAnchorDXXL textAnchor_preFlip, Vector3 textDir_normalized_preFlip, Vector3 textUp_normalized_preFlip, Vector3 forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip)
        {
            textAnchor_postFlip = textAnchor_preFlip;
            textDir_normalized_postFlip = textDir_normalized_preFlip;
            textUp_normalized_postFlip = textUp_normalized_preFlip;
            forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip = forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip;
        }

        public static void TryAutoFlipCircledTextToPreventMirrorInverted(out Vector3 initialTextDirNormalized_postFlip, out Vector3 initialTextUpNormalized_postFlip, out Vector3 forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip, Vector3 initialTextDirNormalized_preFlip, Vector3 initialTextUpNormalized_preFlip, Vector3 forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip, Vector3 textPos, bool autoFlipToPreventMirrorInverted, float angleDegOfLongestLine)
        {
            if (autoFlipToPreventMirrorInverted)
            {
                bool textAppearsMirrorInvertedToObserverCam = CheckIf_textAppearsMirrorInvertedToObserverCam(textPos, forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip);
                if (textAppearsMirrorInvertedToObserverCam)
                {
                    Quaternion rotation_fromInitialUp_aroundForwardPreFlip_toEndOfTextSegment = Quaternion.AngleAxis(-angleDegOfLongestLine, forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip);
                    initialTextUpNormalized_postFlip = rotation_fromInitialUp_aroundForwardPreFlip_toEndOfTextSegment * initialTextUpNormalized_preFlip;
                    forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip = (-forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip);
                    initialTextDirNormalized_postFlip = Vector3.Cross(initialTextUpNormalized_postFlip, forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip);
                }
                else
                {
                    ReturnUnchangedTextSpecs_forCircledText(out initialTextDirNormalized_postFlip, out initialTextUpNormalized_postFlip, out forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip, initialTextDirNormalized_preFlip, initialTextUpNormalized_preFlip, forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip);
                }
            }
            else
            {
                ReturnUnchangedTextSpecs_forCircledText(out initialTextDirNormalized_postFlip, out initialTextUpNormalized_postFlip, out forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip, initialTextDirNormalized_preFlip, initialTextUpNormalized_preFlip, forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip);
            }
        }

        static void ReturnUnchangedTextSpecs_forCircledText(out Vector3 initialTextDirNormalized_postFlip, out Vector3 initialTextUpNormalized_postFlip, out Vector3 forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip, Vector3 initialTextDirNormalized_preFlip, Vector3 initialTextUpNormalized_preFlip, Vector3 forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip)
        {
            initialTextDirNormalized_postFlip = initialTextDirNormalized_preFlip;
            initialTextUpNormalized_postFlip = initialTextUpNormalized_preFlip;
            forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip = forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip;
        }

        static bool CheckIf_textAppearsMirrorInvertedToObserverCam(Vector3 textPos, Vector3 forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip)
        {
            UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out Vector3 observerCamForward_normalized, out Vector3 observerCamUp_normalized, out Vector3 observerCamRight_normalized, out Vector3 cam_to_lineCenter, textPos, Vector3.zero, null);
            return UtilitiesDXXL_Math.Check_ifVectorsPointAwayFromEachOther_perpCountsAsPointingAwayFromEachOther(forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip, cam_to_lineCenter);
        }

        public static bool ConvertQuaternionToTextDirAndUpVectors(out Vector3 textDir, out Vector3 textUp, Quaternion quaternion)
        {
            //returns "rotationIsValid"
            if (UtilitiesDXXL_Math.IsDefaultInvalidQuaternion(quaternion))
            {
                //-> will use "automaticTextOrientation"
                textDir = default(Vector3);
                textUp = default(Vector3);
                return false;
            }
            else
            {
                textDir = quaternion * Vector3.right;
                textUp = quaternion * Vector3.up;
                return true;
            }
        }

        static DrawText.TextAnchorDXXL GetHorizMirroredTextAnchor(DrawText.TextAnchorDXXL anchorToMirror)
        {
            switch (anchorToMirror)
            {
                case DrawText.TextAnchorDXXL.UpperLeft:
                    return DrawText.TextAnchorDXXL.UpperRight;

                case DrawText.TextAnchorDXXL.UpperCenter:
                    return anchorToMirror;

                case DrawText.TextAnchorDXXL.UpperRight:
                    return DrawText.TextAnchorDXXL.UpperLeft;

                case DrawText.TextAnchorDXXL.MiddleLeft:
                    return DrawText.TextAnchorDXXL.MiddleRight;

                case DrawText.TextAnchorDXXL.MiddleCenter:
                    return anchorToMirror;

                case DrawText.TextAnchorDXXL.MiddleRight:
                    return DrawText.TextAnchorDXXL.MiddleLeft;

                case DrawText.TextAnchorDXXL.LowerLeft:
                    return DrawText.TextAnchorDXXL.LowerRight;

                case DrawText.TextAnchorDXXL.LowerCenter:
                    return anchorToMirror;

                case DrawText.TextAnchorDXXL.LowerRight:
                    return DrawText.TextAnchorDXXL.LowerLeft;

                case DrawText.TextAnchorDXXL.LowerLeftOfFirstLine:
                    return DrawText.TextAnchorDXXL.LowerRightOfFirstLine;

                case DrawText.TextAnchorDXXL.LowerCenterOfFirstLine:
                    return anchorToMirror;

                case DrawText.TextAnchorDXXL.LowerRightOfFirstLine:
                    return DrawText.TextAnchorDXXL.LowerLeftOfFirstLine;

                default:
                    Debug.LogError("TextAnchorExt of '" + anchorToMirror + "' not found. TextAnchorHorizMirroring not supported.");
                    return anchorToMirror;
            }
        }

    }

}
