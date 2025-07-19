namespace DrawXXL
{
    using UnityEngine;

    public class UtilitiesDXXL_LineAmplitudeAndTextDirCalculation
    {
        public static float shortestLineWithDefinedAmplitudeDir = 0.0001f; //below that threshold float calculation imprecision may lead to undefined behaviour
        public static float shortestLineWithDefinedAmplitudeDir_withTolerancePadding = 2.0f * shortestLineWithDefinedAmplitudeDir;
        public static float absDotProductResult_ofTwoApproxNormalizedVectors_belowWhichVectorsAreConsideredPerp = 0.01f; // "0.01" is equivalent to around +/-0.6° angle deviation from 90° //This is mainly for preventing jitter in situations that are bistable due to float calculation imprecision

        static Vector3 amplitudeUp_normalized_forArrowsOfCurrArrowLine; //defined at start of arrows line
        static Vector3 textDir_normalized_forArrowsOfCurrArrowLine = default(Vector3); //always disabled
        public static bool theCurrentlyDrawnLines_areAllFrom_vectorsThatBuildUpAnArrowsLine = false;


        static InternalDXXL_Plane plane_perpToLine = new InternalDXXL_Plane();
        public static void Get_normalized_amplitudeAndTextDirVectors(out Vector3 amplitudeUp_normalized, out Vector3 textDir_normalized, out float lengthOfDrawnLine, out bool lengthOfDrawnLine_isFilled, Vector3 lineStartPos, Vector3 line_startToEnd, bool isThinLine, bool lineIsSoShortThatItDoesntHaveADefinedAmplitude, string text, bool textDrawingIsSkipped_dueToLineIsTooShort, DrawBasics.LineStyle style, bool flattenThickRoundLineIntoAmplitudePlane, bool uses_endPlates, InternalDXXL_Plane preferredAmplitudePlane, Vector3 customAmplitudeAndTextDir, Camera cameraFrom_DrawScreenspaceCall, bool drawnLineIsFrom_DrawBasics2D)
        {
            //"preferredAmplitudePlane" doesn't have to contain the drawnLine (that means it can be parallel shifted to somewhere else), but it only indicates the orientation of amplitudeUp

            if (theCurrentlyDrawnLines_areAllFrom_vectorsThatBuildUpAnArrowsLine)
            {
                amplitudeUp_normalized = amplitudeUp_normalized_forArrowsOfCurrArrowLine;
                textDir_normalized = textDir_normalized_forArrowsOfCurrArrowLine;
                lengthOfDrawnLine_isFilled = false;
                lengthOfDrawnLine = 0.0f;
            }
            else
            {
                bool lineNeedsA_perpNormalizedVector_intoADefinedDirection;
                bool lineNeedsA_perpNormalizedVector_intoAnArbitraryDirection;
                bool lineNeedsA_textDirection;
                bool theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection;
                CheckWhichDirectionsAreNeeded(out lineNeedsA_perpNormalizedVector_intoADefinedDirection, out lineNeedsA_perpNormalizedVector_intoAnArbitraryDirection, out lineNeedsA_textDirection, out theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection, lineIsSoShortThatItDoesntHaveADefinedAmplitude, isThinLine, style, text, flattenThickRoundLineIntoAmplitudePlane, uses_endPlates, textDrawingIsSkipped_dueToLineIsTooShort);
                GetUpAndTextDir_accordingToSubCaseSpecification(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled, lineStartPos, line_startToEnd, preferredAmplitudePlane, customAmplitudeAndTextDir, lineNeedsA_perpNormalizedVector_intoADefinedDirection, lineNeedsA_perpNormalizedVector_intoAnArbitraryDirection, lineNeedsA_textDirection, theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection, lineIsSoShortThatItDoesntHaveADefinedAmplitude, cameraFrom_DrawScreenspaceCall, drawnLineIsFrom_DrawBasics2D);
                if (style == DrawBasics.LineStyle.arrows) { amplitudeUp_normalized_forArrowsOfCurrArrowLine = amplitudeUp_normalized; }
            }
        }

        static void CheckWhichDirectionsAreNeeded(out bool lineNeedsA_perpNormalizedVector_intoADefinedDirection, out bool lineNeedsA_perpNormalizedVector_intoAnArbitraryDirection, out bool lineNeedsA_textDirection, out bool theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection, bool lineIsSoShortThatItDoesntHaveADefinedAmplitude, bool isThinLine, DrawBasics.LineStyle style, string text, bool flattenThickRoundLineIntoAmplitudePlane, bool uses_endPlates, bool textDrawingIsSkipped_dueToLineIsTooShort)
        {
            lineNeedsA_textDirection = false;
            lineNeedsA_perpNormalizedVector_intoADefinedDirection = false;
            lineNeedsA_perpNormalizedVector_intoAnArbitraryDirection = false;
            theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection = false; //<-not used in case of "GetUpAndTextDir_complyingWithCallerSpecifiedUpVector"
            if (text != null && text != "" && (textDrawingIsSkipped_dueToLineIsTooShort == false))
            {
                lineNeedsA_perpNormalizedVector_intoADefinedDirection = true;
                lineNeedsA_textDirection = true;
                if (lineIsSoShortThatItDoesntHaveADefinedAmplitude)
                {
                    theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection = true;
                }
            }
            else
            {
                //-> line doesn't have text:
                if (UtilitiesDXXL_LineStyles.CheckIfLineStyleNeedsDefinedAmplitudeForSubLineCreation(style))
                {
                    lineNeedsA_perpNormalizedVector_intoADefinedDirection = true;
                }
                else
                {
                    if (isThinLine)
                    {
                        //thin lines:
                        if (flattenThickRoundLineIntoAmplitudePlane && uses_endPlates)
                        {
                            //-> only endPlates use this:
                            lineNeedsA_perpNormalizedVector_intoADefinedDirection = true;
                        }
                    }
                    else
                    {
                        //thick lines:
                        if (flattenThickRoundLineIntoAmplitudePlane)
                        {
                            lineNeedsA_perpNormalizedVector_intoADefinedDirection = true;
                        }
                        else
                        {
                            lineNeedsA_perpNormalizedVector_intoAnArbitraryDirection = true;
                            //-> used to get the cylindrical hull line anchors
                        }
                    }
                }
            }
        }

        static void GetUpAndTextDir_accordingToSubCaseSpecification(out Vector3 amplitudeUp_normalized, out Vector3 textDir_normalized, out float lengthOfDrawnLine, out bool lengthOfDrawnLine_isFilled, Vector3 lineStartPos, Vector3 line_startToEnd, InternalDXXL_Plane preferredAmplitudePlane, Vector3 customAmplitudeAndTextDir, bool lineNeedsA_perpNormalizedVector_intoADefinedDirection, bool lineNeedsA_perpNormalizedVector_intoAnArbitraryDirection, bool lineNeedsA_textDirection, bool theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection, bool lineIsSoShortThatItDoesntHaveADefinedAmplitude, Camera cameraFrom_DrawScreenspaceCall, bool drawnLineIsFrom_DrawBasics2D)
        {
            if (lineNeedsA_perpNormalizedVector_intoADefinedDirection)
            {
                Vector3 observerCamForward_normalized;
                Vector3 observerCamUp_normalized;
                Vector3 observerCamRight_normalized;
                Vector3 cam_to_lineCenter;
                UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter, lineStartPos, line_startToEnd, cameraFrom_DrawScreenspaceCall);

                if (preferredAmplitudePlane != null)
                {
                    //"preferredAmplitudePlane" has been defined
                    //"preferredAmplitudePlane" is stronger than "customAmplitudeAndTextDir":
                    //->if a "preferredAmplitudePlane" is defined then "customAmplitudeAndTextDir" gets ignored.
                    GetUpAndTextDir_insideAmplitudePlane(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled, lineStartPos, line_startToEnd, preferredAmplitudePlane, lineNeedsA_textDirection, theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection, cameraFrom_DrawScreenspaceCall, drawnLineIsFrom_DrawBasics2D, observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);

                    //bool userHasSpecifiedAn_amplitudeUpVector = (UtilitiesDXXL_Math.IsDefaultVector(customAmplitudeAndTextDir) == false);
                    //if (userHasSpecifiedAn_amplitudeUpVector)
                    //{
                    //    //-> user has specified an amplitudePlane AND an amplitudeUpVector
                    //    //-> this case is not expected here, but a partial implementation (that at least handles the "*_independentFromTooShortLineDir_*"-cases) already exists in "UtilitiesDXXL_TextDirAndUpCalculation.GetTextDirAndUpNormalized_whileUserHas_notSpecifiedDir_but_specifiedUp"
                    //}
                }
                else
                {
                    //-> no amplitudePlane is specified
                    bool userHasSpecifiedAn_amplitudeUpVector = (UtilitiesDXXL_Math.IsDefaultVector(customAmplitudeAndTextDir) == false);
                    if (userHasSpecifiedAn_amplitudeUpVector)
                    {
                        GetUpAndTextDir_complyingWithCallerSpecifiedUpVector(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled, lineIsSoShortThatItDoesntHaveADefinedAmplitude, lineStartPos, line_startToEnd, customAmplitudeAndTextDir, lineNeedsA_textDirection, theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection, observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                    }
                    else
                    {
                        //-> no "amplitudePlane" and no "customAmplitudeAndTextDir-vector" dictate the amplitude direction.
                        //-> drawnLine.up and textDir can be freely chosen on the base of the "DrawBasics.cameraForAutomaticOrientation"-viewDirection (as long as they are aligned to the drawnLine)
                        GetUpAndTextDir_withoutCallerSpecifiedPreference_accordingToAutomaticAmplitudeAndTextAlignmentSettings(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled, lineStartPos, line_startToEnd, lineNeedsA_textDirection, theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection, observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                    }
                }
            }
            else
            {
                //-> line doesn't need a perpNormalizedVector_intoADefinedDirection
                //-> line doesn't need a textDir
                textDir_normalized = default(Vector3);
                lengthOfDrawnLine_isFilled = false;
                lengthOfDrawnLine = 0.0f;
                GetArbitraryUpDir(out amplitudeUp_normalized, line_startToEnd, lineNeedsA_perpNormalizedVector_intoAnArbitraryDirection);
            }
        }

        static void GetUpAndTextDir_insideAmplitudePlane(out Vector3 amplitudeUp_normalized, out Vector3 textDir_normalized, out float lengthOfDrawnLine, out bool lengthOfDrawnLine_isFilled, Vector3 lineStartPos, Vector3 line_startToEnd, InternalDXXL_Plane preferredAmplitudePlane, bool lineNeedsA_textDirection, bool theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection, Camera cameraFrom_DrawScreenspaceCall, bool drawnLineIsFrom_DrawBasics2D, Vector3 observerCamForward_normalized, Vector3 observerCamUp_normalized, Vector3 observerCamRight_normalized, Vector3 cam_to_lineCenter)
        {
            if (theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection)
            {
                //-> line is too short to have any direction preferences: That means that any directions can be chosen, as long as they lie inside the plane.
                //-> "DrawBasics.automaticTextDirectionOfLines == towardsLineEnd" has no effect, since there is no lineDirection that could be used -> The only remaining option here is "DrawBasics.automaticTextDirectionOfLines == leftToRightInScreen"

                if (cameraFrom_DrawScreenspaceCall != null)
                {
                    //-> The call came from "DrawScreenspace.*()"
                    amplitudeUp_normalized = observerCamUp_normalized;
                    textDir_normalized = observerCamRight_normalized;
                    lengthOfDrawnLine_isFilled = false;
                    lengthOfDrawnLine = 0.0f;
                }
                else
                {
                    if (drawnLineIsFrom_DrawBasics2D)
                    {
                        //-> The call came from "DrawBasics2D.*()"
                        amplitudeUp_normalized = Vector3.up;
                        textDir_normalized = Vector3.right;
                        lengthOfDrawnLine_isFilled = false;
                        lengthOfDrawnLine = 0.0f;
                    }
                    else
                    {
                        if (DrawBasics.automaticAmplitudeAndTextAlignment == DrawBasics.AutomaticAmplitudeAndTextAlignment.vertical)
                        {
                            GetUpAndTextDir_insideAmplitudePlane_independentFromTooShortLineDir_alignedToVertical(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled, preferredAmplitudePlane, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                        }
                        else
                        {
                            GetUpAndTextDir_insideAmplitudePlane_independentFromTooShortLineDir_alignedToObserverCam(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled, preferredAmplitudePlane, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                        }
                    }
                }
            }
            else
            {
                //-> drawnLine is guaranteed longer than zero/longer than minLengthThreshold here
                //-> textDir may or may not be required
                GetUpAndTextDir_insideAmplitudePlane_forNonShortLine(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled, lineStartPos, line_startToEnd, preferredAmplitudePlane, lineNeedsA_textDirection, theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection, cameraFrom_DrawScreenspaceCall, observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter, false);
            }
        }

        static void GetUpAndTextDir_insideAmplitudePlane_independentFromTooShortLineDir_alignedToVertical(out Vector3 amplitudeUp_normalized, out Vector3 textDir_normalized, out float lengthOfDrawnLine, out bool lengthOfDrawnLine_isFilled, InternalDXXL_Plane preferredAmplitudePlane, Vector3 observerCamUp_normalized, Vector3 observerCamRight_normalized, Vector3 cam_to_lineCenter)
        {
            bool thePlaneIsHorizontal = InternalDXXL_Plane.IsHorizontal(preferredAmplitudePlane);
            if (thePlaneIsHorizontal)
            {
                bool viewDirCamToLine_isHorizontal = UtilitiesDXXL_Math.ApproximatelyZero(cam_to_lineCenter.y);
                if (viewDirCamToLine_isHorizontal)
                {
                    //-> the camera is looking "inside/along" the (horiz)plane, so it cannot see any vector inside the plane
                    //-> up and textDir can be freely chosen, as long as they lie in the plane
                    GetUpAndTextDir_insideHorizPlane_independentFromTooShortLineDir_independentFromObserverCamDir(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled);
                }
                else
                {
                    if (UtilitiesDXXL_Math.GetBiggestAbsComponent(cam_to_lineCenter) < 0.001f)
                    {
                        GetUpAndTextDir_insideHorizPlane_independentFromTooShortLineDir_independentFromObserverCamDir(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled);
                    }
                    else
                    {
                        Vector3 observerCamUp_projectedAlongViewDir_ontoAmplitudePlane_notNormalized = preferredAmplitudePlane.Get_projectionOfVectorOntoPlane_alongCustomDir(observerCamUp_normalized, cam_to_lineCenter);
                        Vector3 observerCamUp_projectedAlongViewDir_ontoAmplitudePlane_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(observerCamUp_projectedAlongViewDir_ontoAmplitudePlane_notNormalized);

                        if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(observerCamUp_projectedAlongViewDir_ontoAmplitudePlane_normalized))
                        {
                            UtilitiesDXXL_Log.PrintErrorCode("16-" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(observerCamUp_normalized) + "-" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(cam_to_lineCenter) + "-" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(preferredAmplitudePlane.normalDir));
                            GetUpAndTextDir_insideHorizPlane_independentFromTooShortLineDir_independentFromObserverCamDir(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled);
                        }
                        else
                        {
                            //->"amplitudeUp_normalized" is flat inside horizPlane -> it's y is zero
                            Vector3 aNormalizedVector_definingTheTextAlignment = new Vector3(observerCamUp_projectedAlongViewDir_ontoAmplitudePlane_normalized.z, 0.0f, -observerCamUp_projectedAlongViewDir_ontoAmplitudePlane_normalized.x); //-> turns "amplitudeUp_normalized" by 90deg around the y-axis

                            //line is so short that alignment "automaticTextDirectionOfLines == towardsLineEnd" is not possible, therefore using "FlipNormalizedUpAndTextDir_toFit_textDirFromLeftToRightInsideScreen" instead of "FlipNormalizedUpAndTextDir_toFit_automaticTextDirection"
                            FlipNormalizedUpAndTextDir_toFit_textDirFromLeftToRightInsideScreen_viaUpDirDictatesVertInsideScreenApproximation(out amplitudeUp_normalized, out textDir_normalized, observerCamUp_projectedAlongViewDir_ontoAmplitudePlane_normalized, aNormalizedVector_definingTheTextAlignment, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                            lengthOfDrawnLine_isFilled = false;
                            lengthOfDrawnLine = 0.0f;
                        }
                    }
                }
            }
            else
            {
                GetUpAndTextDir_insideNonHorizontalAmplitudePlane_viaProjectionOfVerticalGlobalUpOntoPlane(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled, preferredAmplitudePlane, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
            }
        }

        static void GetUpAndTextDir_insideAmplitudePlane_independentFromTooShortLineDir_alignedToObserverCam(out Vector3 amplitudeUp_normalized, out Vector3 textDir_normalized, out float lengthOfDrawnLine, out bool lengthOfDrawnLine_isFilled, InternalDXXL_Plane preferredAmplitudePlane, Vector3 observerCamUp_normalized, Vector3 observerCamRight_normalized, Vector3 cam_to_lineCenter)
        {
            //-> "cam_to_lineCenter" is allowed to be zero here. It will lead to "camViewDir_isApproxInsideAmplitudePlane" and is then not further used.
            float absDotProductResult_of_camToLine_and_amplitudePlaneNormal = Mathf.Abs(Vector3.Dot(cam_to_lineCenter, preferredAmplitudePlane.normalDir));
            bool camViewDir_isApproxInsideAmplitudePlane = (absDotProductResult_of_camToLine_and_amplitudePlaneNormal < absDotProductResult_ofTwoApproxNormalizedVectors_belowWhichVectorsAreConsideredPerp);
            if (camViewDir_isApproxInsideAmplitudePlane)
            {
                //-> the camera is looking "inside/along" the plane, so it cannot see any vector inside the plane
                //-> up and textDir can be freely chosen, as long as they lie in the plane
                //-> it doesn't matter which vectors inside the plane are used because the camera cannot see any of them
                //-> fallback to "automaticAmplitudeAndTextAlignment == vertical"

                bool thePlaneIsHorizontal = InternalDXXL_Plane.IsHorizontal(preferredAmplitudePlane);
                if (thePlaneIsHorizontal)
                {
                    GetUpAndTextDir_insideHorizPlane_independentFromTooShortLineDir_independentFromObserverCamDir(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled);
                }
                else
                {
                    //fallback to alignVertical:
                    GetUpAndTextDir_insideNonHorizontalAmplitudePlane_viaProjectionOfVerticalGlobalUpOntoPlane(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled, preferredAmplitudePlane, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                }
            }
            else
            {
                //-> camToLineCenter is NOT inside the amplitudePlane
                //-> no bistable "projection is maybe zero" is expected here, because the "camViewDir_isApproxInsideAmplitudePlane"-calulation dotProduct has an anglePadding.

                //->"cam_to_lineCenter" is safely longer than 0 here and can therefore be used as projectionDir without problems
                Vector3 camUp_projectedAlongCamToLineCenter_ontoAmplitudePlane_notNormalized = preferredAmplitudePlane.Get_projectionOfVectorOntoPlane_alongCustomDir(observerCamUp_normalized, cam_to_lineCenter);
                Vector3 aNormalizedVector_definingTheUpAlignment = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(camUp_projectedAlongCamToLineCenter_ontoAmplitudePlane_notNormalized);

                Vector3 aVector_definingTheTextAlignment = Vector3.Cross(aNormalizedVector_definingTheUpAlignment, preferredAmplitudePlane.normalDir);
                Vector3 aNormalizedVector_definingTheTextAlignment = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(aVector_definingTheTextAlignment);

                if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(aNormalizedVector_definingTheTextAlignment)) //<- this implicitly also checks if "aNormalizedVector_definingTheUpAlignment" has been successfully normalized
                {
                    UtilitiesDXXL_Log.PrintErrorCode("18-" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(preferredAmplitudePlane.normalDir) + "-" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(observerCamUp_normalized) + "-" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(cam_to_lineCenter));
                    GetUpAndTextDir_insideAmplitudePlane_independentFromTooShortLineDir_independentFromObserverCamDir(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled, preferredAmplitudePlane);
                }
                else
                {
                    //line is so short that alignment "automaticTextDirectionOfLines == towardsLineEnd" is not possible, therefore using "FlipNormalizedUpAndTextDir_toFit_textDirFromLeftToRightInsideScreen" instead of "FlipNormalizedUpAndTextDir_toFit_automaticTextDirection"
                    FlipNormalizedUpAndTextDir_toFit_textDirFromLeftToRightInsideScreen_viaUpDirDictatesVertInsideScreenApproximation(out amplitudeUp_normalized, out textDir_normalized, aNormalizedVector_definingTheUpAlignment, aNormalizedVector_definingTheTextAlignment, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                    lengthOfDrawnLine_isFilled = false;
                    lengthOfDrawnLine = 0.0f;
                }
            }
        }

        static void GetUpAndTextDir_insideHorizPlane_independentFromTooShortLineDir_independentFromObserverCamDir(out Vector3 amplitudeUp_normalized, out Vector3 textDir_normalized, out float lengthOfDrawnLine, out bool lengthOfDrawnLine_isFilled)
        {
            amplitudeUp_normalized = Vector3.forward;
            textDir_normalized = Vector3.right;
            lengthOfDrawnLine_isFilled = false;
            lengthOfDrawnLine = 0.0f;
        }

        static void GetUpAndTextDir_insideAmplitudePlane_independentFromTooShortLineDir_independentFromObserverCamDir(out Vector3 amplitudeUp_normalized, out Vector3 textDir_normalized, out float lengthOfDrawnLine, out bool lengthOfDrawnLine_isFilled, InternalDXXL_Plane preferredAmplitudePlane)
        {
            Vector3 arbitraryVector_insideThePlane_notNormalized = preferredAmplitudePlane.Get_projectionOfVectorOntoPlane(UtilitiesDXXL_Math.arbitrarySeldomDir_normalized_precalced);
            amplitudeUp_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(arbitraryVector_insideThePlane_notNormalized);
            textDir_normalized = Vector3.Cross(amplitudeUp_normalized, preferredAmplitudePlane.normalDir).normalized;
            lengthOfDrawnLine_isFilled = false;
            lengthOfDrawnLine = 0.0f;
        }

        static InternalDXXL_Line intersectionLine_ofTwoPlanes = new InternalDXXL_Line();
        static void GetUpAndTextDir_insideNonHorizontalAmplitudePlane_viaProjectionOfVerticalGlobalUpOntoPlane(out Vector3 amplitudeUp_normalized, out Vector3 textDir_normalized, out float lengthOfDrawnLine, out bool lengthOfDrawnLine_isFilled, InternalDXXL_Plane preferredAmplitudePlane, Vector3 observerCamUp_normalized, Vector3 observerCamRight_normalized, Vector3 cam_to_lineCenter)
        {
            //-> this function is only called from places that need textDir

            Vector3 projection_ofGlobalUpVector_perpOntoAmplitudePlane_notNormalized = preferredAmplitudePlane.Get_projectionOfVectorOntoPlane(Vector3.up);
            Vector3 aNormalizedVector_definingTheUpAlignment = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(projection_ofGlobalUpVector_perpOntoAmplitudePlane_notNormalized);

            if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(aNormalizedVector_definingTheUpAlignment))
            {
                UtilitiesDXXL_Log.PrintErrorCode("17-" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(preferredAmplitudePlane.normalDir));
                GetUpAndTextDir_insideHorizPlane_independentFromTooShortLineDir_independentFromObserverCamDir(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled);
            }
            else
            {
                InternalDXXL_Plane.Calc_intersectionLine_ofTwoPlanes(ref intersectionLine_ofTwoPlanes, preferredAmplitudePlane, InternalDXXL_Plane.horizPlane_throughZeroOrigin);

                //-> this function is only called from places where the line is so short that alignment "automaticTextDirectionOfLines == towardsLineEnd" is not possible, therefore using "FlipNormalizedUpAndTextDir_toFit_textDirFromLeftToRightInsideScreen" instead of "FlipNormalizedUpAndTextDir_toFit_automaticTextDirection"
                FlipNormalizedUpAndTextDir_toFit_textDirFromLeftToRightInsideScreen_viaUpDirDictatesVertInsideScreenApproximation(out amplitudeUp_normalized, out textDir_normalized, aNormalizedVector_definingTheUpAlignment, intersectionLine_ofTwoPlanes.direction_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                lengthOfDrawnLine_isFilled = false;
                lengthOfDrawnLine = 0.0f;
            }
        }

        public static void GetUpAndTextDir_insideAmplitudePlane_forNonShortLine(out Vector3 amplitudeUp_normalized, out Vector3 textDir_normalized, out float lengthOfDrawnLine, out bool lengthOfDrawnLine_isFilled, Vector3 lineStartPos, Vector3 line_startToEnd, InternalDXXL_Plane preferredAmplitudePlane, bool lineNeedsA_textDirection, bool theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection, Camera cameraFrom_DrawScreenspaceCall, Vector3 observerCamForward_normalized, Vector3 observerCamUp_normalized, Vector3 observerCamRight_normalized, Vector3 cam_to_lineCenter, bool isFallbackFrom_GetUpAndTextDir_withoutCallerSpecifiedPreference)
        {
            Vector3 aLine_perpToDrawnLine_insideAmplitudePlane_notNormalized = Vector3.Cross(line_startToEnd, preferredAmplitudePlane.normalDir);
            Vector3 aLine_perpToDrawnLine_insideAmplitudePlane_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(aLine_perpToDrawnLine_insideAmplitudePlane_notNormalized);

            if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(aLine_perpToDrawnLine_insideAmplitudePlane_normalized))
            {
                //-> cross product result could not be normalized
                //-> the line is perp to plane
                //-> the condition that the upVector should lie in the specified plane is already met.
                //-> EVERY perpToLine-vector lies inside the specified plane
                //-> any fallback upVector can be chosen, as long as it lies inside the plane.
                //-> textDir vector: impossible to put into plane, since it should be perp to plane

                if (isFallbackFrom_GetUpAndTextDir_withoutCallerSpecifiedPreference)
                {
                    //the fallback came with a vert line and the xy-plane: How can this be perpToEachOther?
                    //UtilitiesDXXL_Log.PrintErrorCode("21-" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(lineStartPos) + "-" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(line_startToEnd) + "-" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(preferredAmplitudePlane.normalDir));
                    //<- "UtilitiesDXXL_LineCircled.LineCircledBelow180Deg(turnAngleDeg=approxZero)" once triggered this before widening the span that counts as zero.
                    //<- also after that "UtilitiesDXXL_LineCircled.LineCircledBelow180Deg(turnAngleDeg=approxZero)" triggered it by supplying "line_startToEnd = 0,0,0". Therefore decision: Omit error code and accept using the fallback sometimes.

                    //this fallback doesn't align to observerCam and can appear mirrorInverted:
                    amplitudeUp_normalized = Vector3.left;
                    textDir_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(line_startToEnd);
                    if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(textDir_normalized))
                    {
                        textDir_normalized = Vector3.up;
                    }

                    if (textDir_normalized.y < 0.0f) { textDir_normalized = (-textDir_normalized); }
                    lengthOfDrawnLine_isFilled = false;
                    lengthOfDrawnLine = 0.0f;
                }
                else
                {
                    GetUpAndTextDir_withoutCallerSpecifiedPreference_accordingToAutomaticAmplitudeAndTextAlignmentSettings(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled, lineStartPos, line_startToEnd, lineNeedsA_textDirection, theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection, observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                    //<- this fallback will meet the required criteria:
                    //-> amplitudeUp will be inside the "preferredAmplitudePlane", since perpToLine-vector does
                    //-> textDir will be along the line. Since it is impossible to put it into the plane the fallback at least makes sure that it is nonMirrored readable in the observerCam
                }
            }
            else
            {
                //line is not short and not perp to plane:
                if (lineNeedsA_textDirection)
                {
                    lengthOfDrawnLine_isFilled = true;
                    Vector3 lineDir_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(line_startToEnd, out lengthOfDrawnLine);
                    FlipNormalizedUpAndTextDir_toFit_automaticTextDirection(out amplitudeUp_normalized, out textDir_normalized, aLine_perpToDrawnLine_insideAmplitudePlane_normalized, lineDir_normalized, false, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                }
                else
                {
                    amplitudeUp_normalized = aLine_perpToDrawnLine_insideAmplitudePlane_normalized;
                    textDir_normalized = default(Vector3);
                    lengthOfDrawnLine_isFilled = false;
                    lengthOfDrawnLine = 0.0f;
                }
            }
        }

        static void GetUpAndTextDir_withoutCallerSpecifiedPreference_accordingToAutomaticAmplitudeAndTextAlignmentSettings(out Vector3 amplitudeUp_normalized, out Vector3 textDir_normalized, out float lengthOfDrawnLine, out bool lengthOfDrawnLine_isFilled, Vector3 lineStartPos, Vector3 line_startToEnd, bool lineNeedsA_textDirection, bool theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection, Vector3 observerCamForward_normalized, Vector3 observerCamUp_normalized, Vector3 observerCamRight_normalized, Vector3 cam_to_lineCenter)
        {
            if (theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection)
            {
                //-> only autoEnlargedText may use this:
                //-> even less restrictions for choosing the up/textDir: Because here the directions don't even have to be aligned along the drawnLine (-> now "DrawBasics.cameraForAutomaticOrientation" can really choose freely)

                lengthOfDrawnLine_isFilled = false;
                lengthOfDrawnLine = 0.0f;

                if (DrawBasics.automaticAmplitudeAndTextAlignment == DrawBasics.AutomaticAmplitudeAndTextAlignment.vertical)
                {
                    GetUpAndTextDir_withoutCallerSpecifiedPreference_independentFromTooShortLineDir_alignedVertical(out amplitudeUp_normalized, out textDir_normalized, observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                }
                else
                {
                    //align to camera:
                    amplitudeUp_normalized = observerCamUp_normalized;
                    textDir_normalized = observerCamRight_normalized;
                }
            }
            else
            {
                //-> drawnLine is guaranteed longer than zero/longer than minLengthThreshold here
                //-> textDir may or may not be required
                if (DrawBasics.automaticAmplitudeAndTextAlignment == DrawBasics.AutomaticAmplitudeAndTextAlignment.vertical)
                {
                    GetUpAndTextDir_withoutCallerSpecifiedPreference_forNonShortLine_alignedVertical(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled, lineStartPos, line_startToEnd, lineNeedsA_textDirection, observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                }
                else
                {
                    GetUpAndTextDir_withoutCallerSpecifiedPreference_forNonShortLine_alignedToObserverCam(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled, lineStartPos, line_startToEnd, lineNeedsA_textDirection, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                }
            }
        }

        public static void GetUpAndTextDir_withoutCallerSpecifiedPreference_independentFromTooShortLineDir_alignedVertical(out Vector3 amplitudeUp_normalized, out Vector3 textDir_normalized, Vector3 observerCamForward_normalized, Vector3 observerCamUp_normalized, Vector3 observerCamRight_normalized, Vector3 cam_to_lineCenter)
        {
            //align vertical in global space:
            amplitudeUp_normalized = Vector3.up;

            //-> textDir: has to lie inside horizPlane
            //-> textDir.y is always 0

            if (Mathf.Abs(observerCamRight_normalized.y) < 0.0001f)
            {
                //most common case:
                //-> observerCam is not tilted sidewards
                textDir_normalized = observerCamRight_normalized;
            }
            else
            {
                //-> observerCam is tilted sidewards
                Vector3 camRight_projectedInto_horizPlane_notNormalized = InternalDXXL_Plane.horizPlane_throughZeroOrigin.Get_projectionOfVectorOntoPlane(observerCamRight_normalized);
                Vector3 camRight_projectedInto_horizPlane_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(camRight_projectedInto_horizPlane_notNormalized);
                if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(camRight_projectedInto_horizPlane_normalized))
                {
                    //very rare case
                    //-> the observerCam is tilted to the side by 90 degree (and has no other tilts that prevents it from looking inside the horizPlane(=no y-rotation))
                    //-> known issue: This projection to "camRight_projectedInto_horizPlane_notNormalized" may not be reproducably zero, due to float calculation imprecsion. This leads to this thread not beeing reached.
                    Quaternion rotation_fromDrawnLineUp_to_textDir = Quaternion.AngleAxis(-90.0f, observerCamForward_normalized);
                    textDir_normalized = rotation_fromDrawnLineUp_to_textDir * amplitudeUp_normalized;
                }
                else
                {
                    //line is too short to have a direction, so "FlipNormalizedUpAndTextDir_toFit_textDirFromLeftToRightInsideScreen" can be called instead of "FlipNormalizedUpAndTextDir_toFit_automaticTextDirection":
                    FlipNormalizedUpAndTextDir_toFit_textDirFromLeftToRightInsideScreen_viaUpDirDictatesVertInsideScreenApproximation(out amplitudeUp_normalized, out textDir_normalized, amplitudeUp_normalized, camRight_projectedInto_horizPlane_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                }
            }
        }

        static void GetUpAndTextDir_withoutCallerSpecifiedPreference_forNonShortLine_alignedVertical(out Vector3 amplitudeUp_normalized, out Vector3 textDir_normalized, out float lengthOfDrawnLine, out bool lengthOfDrawnLine_isFilled, Vector3 lineStartPos, Vector3 line_startToEnd, bool lineNeedsA_textDirection, Vector3 observerCamForward_normalized, Vector3 observerCamUp_normalized, Vector3 observerCamRight_normalized, Vector3 cam_to_lineCenter)
        {
            Vector3 line_startToEnd_approxNormalized = UtilitiesDXXL_Math.GetApproxNormalized_afterScalingIntoRegionOfFloatPrecicion(line_startToEnd);
            float absDotProductResult_of_lineDir_and_globalRight = Mathf.Abs(Vector3.Dot(line_startToEnd_approxNormalized, Vector3.right));
            float absDotProductResult_of_lineDir_and_globalForward = Mathf.Abs(Vector3.Dot(line_startToEnd_approxNormalized, Vector3.forward));
            bool lineItselfIsVerticalInGlobalSpace = ((absDotProductResult_of_lineDir_and_globalRight < absDotProductResult_ofTwoApproxNormalizedVectors_belowWhichVectorsAreConsideredPerp) && (absDotProductResult_of_lineDir_and_globalForward < absDotProductResult_ofTwoApproxNormalizedVectors_belowWhichVectorsAreConsideredPerp));
            //<- The dot product returns a stable result, because both initialVectors are already approx normalized. 
            //<- If there is still an edge case that produces falsePositive-parallelDetection, then it is no big problem, because the worst thing that can happen, is that the text gets aligned to xy-plane instead

            if (lineItselfIsVerticalInGlobalSpace)
            {
                //-> fallback to alignToCamera:
                //GetUpAndTextDir_withoutCallerSpecifiedPreference_forNonShortLine_alignedToObserverCam(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled, lineStartPos, line_startToEnd, lineNeedsA_textDirection, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);

                //-> fallback to inside-xyPlane:
                InternalDXXL_Plane preferredAmplitudePlane = UtilitiesDXXL_DrawBasics2D.xyPlane_throughZero;
                GetUpAndTextDir_insideAmplitudePlane_forNonShortLine(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled, lineStartPos, line_startToEnd, preferredAmplitudePlane, lineNeedsA_textDirection, false, null, observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter, true);
            }
            else
            {
                plane_perpToLine.Recreate(lineStartPos, line_startToEnd);
                Vector3 amplitudeUp_notNormalized = plane_perpToLine.Get_projectionOfVectorOntoPlane(Vector3.up);
                amplitudeUp_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(amplitudeUp_notNormalized);

                if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(amplitudeUp_normalized))
                {
                    UtilitiesDXXL_Log.PrintErrorCode("14-" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(line_startToEnd));
                }

                if (lineNeedsA_textDirection)
                {
                    lengthOfDrawnLine_isFilled = true;
                    Vector3 lineDir_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(line_startToEnd, out lengthOfDrawnLine);
                    if (DrawBasics.automaticTextDirectionOfLines == DrawBasics.AutomaticTextDirectionOfLines.towardsLineEnd)
                    {
                        textDir_normalized = lineDir_normalized;
                        Vector3 aVector_perpToDrawnLine_intoTheDirectionOf_amplitudeUpIfTextShouldBeReadable_notNormalized = Vector3.Cross(cam_to_lineCenter, textDir_normalized);
                        //"aVector_perpToDrawnLine_intoTheDirectionOf_amplitudeUpIfTextShouldBeReadable_notNormalized" can become zero, but it is not a big problem, because this means that the drawnLine is parallel to camViewDir, so the text will anyway not be readable and can therefore go into the wrong opposite direction.
                        if (UtilitiesDXXL_Math.Check_ifVectorsPointAwayFromEachOther_perpCountsAsPointingAwayFromEachOther(amplitudeUp_normalized, aVector_perpToDrawnLine_intoTheDirectionOf_amplitudeUpIfTextShouldBeReadable_notNormalized))
                        {
                            amplitudeUp_normalized = (-amplitudeUp_normalized);
                        }
                    }
                    else
                    {
                        //Text dir: leftToRightInScreen
                        //-> amplitudeUp(plane) is aligned to the "vertical"-setting, but the upDir and textDir now gets flipped to comply with "leftToRightInScreen"
                        FlipNormalizedUpAndTextDir_toFit_textDirFromLeftToRightInsideScreen_viaUpDirDictatesVertInsideScreenApproximation(out amplitudeUp_normalized, out textDir_normalized, amplitudeUp_normalized, lineDir_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                    }
                }
                else
                {
                    textDir_normalized = default(Vector3);
                    lengthOfDrawnLine_isFilled = false;
                    lengthOfDrawnLine = 0.0f;
                }
            }
        }

        static void GetUpAndTextDir_withoutCallerSpecifiedPreference_forNonShortLine_alignedToObserverCam(out Vector3 amplitudeUp_normalized, out Vector3 textDir_normalized, out float lengthOfDrawnLine, out bool lengthOfDrawnLine_isFilled, Vector3 lineStartPos, Vector3 line_startToEnd, bool lineNeedsA_textDirection, Vector3 observerCamUp_normalized, Vector3 observerCamRight_normalized, Vector3 cam_to_lineCenter)
        {
            //-> drawnLine is guaranteed longer than zero/longer than minLengthThreshold here

            Vector3 aVector_perpToCamViewDir_perpToLine_toTheSideWhereUpShouldPointForNonMirroredCamReadabilityInCaseTextTowardsLineEnd_notNormalized = Vector3.Cross(cam_to_lineCenter, line_startToEnd);
            Vector3 aVector_perpToCamViewDir_perpToLine_toTheSideWhereUpShouldPointForNonMirroredCamReadabilityInCaseTextTowardsLineEnd_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(aVector_perpToCamViewDir_perpToLine_toTheSideWhereUpShouldPointForNonMirroredCamReadabilityInCaseTextTowardsLineEnd_notNormalized);

            if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(aVector_perpToCamViewDir_perpToLine_toTheSideWhereUpShouldPointForNonMirroredCamReadabilityInCaseTextTowardsLineEnd_normalized))
            {
                //-> The drawnLine is approx parallel to cameras view direction:
                plane_perpToLine.Recreate(lineStartPos, line_startToEnd);
                Vector3 amplitudeUp_notNormalized = plane_perpToLine.Get_projectionOfVectorOntoPlane(observerCamUp_normalized); //-> Also for perspective cams with their warped viewField: A "cam.transform.up"-vector always appears as vertical inside screen, independent where in the scene it is placed or if it appears in the warped carner of the perspective screen.
                amplitudeUp_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(amplitudeUp_notNormalized);
                if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(amplitudeUp_normalized))
                {
                    UtilitiesDXXL_Log.PrintErrorCode("15-" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(line_startToEnd) + "-" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(observerCamUp_normalized));
                }

                if (lineNeedsA_textDirection)
                {
                    //-> no flipping of the direction because the text is anyway not readable in the screen (since the line is parallel to camViewDir)
                    lengthOfDrawnLine_isFilled = true;
                    textDir_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(line_startToEnd, out lengthOfDrawnLine);
                }
                else
                {
                    textDir_normalized = default(Vector3);
                    lengthOfDrawnLine_isFilled = false;
                    lengthOfDrawnLine = 0.0f;
                }
            }
            else
            {
                //-> Note for perspective cameras: "aVector_perpToCamViewDir_perpToLine_toTheSideWhereUpShouldPointForNonMirroredCamReadabilityInCaseTextTowardsLineEnd_normalized" is not "perp to the screenPlane", but it is "perp to the view dir".
                if (lineNeedsA_textDirection)
                {
                    lengthOfDrawnLine_isFilled = true;
                    Vector3 lineDir_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(line_startToEnd, out lengthOfDrawnLine);
                    FlipNormalizedUpAndTextDir_toFit_automaticTextDirection(out amplitudeUp_normalized, out textDir_normalized, aVector_perpToCamViewDir_perpToLine_toTheSideWhereUpShouldPointForNonMirroredCamReadabilityInCaseTextTowardsLineEnd_normalized, lineDir_normalized, true, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                }
                else
                {
                    amplitudeUp_normalized = aVector_perpToCamViewDir_perpToLine_toTheSideWhereUpShouldPointForNonMirroredCamReadabilityInCaseTextTowardsLineEnd_normalized;
                    textDir_normalized = default(Vector3);
                    lengthOfDrawnLine_isFilled = false;
                    lengthOfDrawnLine = 0.0f;
                }
            }
        }

        static void GetUpAndTextDir_complyingWithCallerSpecifiedUpVector(out Vector3 amplitudeUp_normalized, out Vector3 textDir_normalized, out float lengthOfDrawnLine, out bool lengthOfDrawnLine_isFilled, bool lineIsSoShortThatItDoesntHaveADefinedAmplitude, Vector3 lineStartPos, Vector3 line_startToEnd, Vector3 customAmplitudeAndTextDir, bool lineNeedsA_textDirection, bool theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection, Vector3 observerCamForward_normalized, Vector3 observerCamUp_normalized, Vector3 observerCamRight_normalized, Vector3 cam_to_lineCenter)
        {
            //-> The user has specifed a custom amplitude dir via vector parameter
            //-> The user-specifed "customAmplitudeAndTextDir"-Vector dictates the drawnLine.up-direction
            //-> The user-specifed "customAmplitudeAndTextDir"-Vector is not guaranteed to be normalized
            //-> "theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection" does not apply here 

            if (lineIsSoShortThatItDoesntHaveADefinedAmplitude)
            {
                lengthOfDrawnLine_isFilled = false;
                lengthOfDrawnLine = 0.0f;
                GetUpAndTextDir_complyingWithCallerSpecifiedUpVector_independentFromTooShortLineDir(out amplitudeUp_normalized, out textDir_normalized, customAmplitudeAndTextDir, lineNeedsA_textDirection, cam_to_lineCenter);
            }
            else
            {
                GetUpAndTextDir_complyingWithCallerSpecifiedUpVector_forNonShortLine(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled, lineStartPos, line_startToEnd, customAmplitudeAndTextDir, lineNeedsA_textDirection, theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection, observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
            }
        }

        static void GetUpAndTextDir_complyingWithCallerSpecifiedUpVector_independentFromTooShortLineDir(out Vector3 amplitudeUp_normalized, out Vector3 textDir_normalized, Vector3 customAmplitudeAndTextDir, bool lineNeedsA_textDirection, Vector3 cam_to_lineCenter)
        {
            amplitudeUp_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(customAmplitudeAndTextDir);
            if (lineNeedsA_textDirection)
            {
                //-> only enlarged text may use this...
                //-> textDir doesn't have any requirements except for beeing perp to "amplitudeUp"

                Vector3 textDir_ifTextShouldBeNonMirroredReadableInCamScreen_notNormalized = Vector3.Cross(amplitudeUp_normalized, cam_to_lineCenter);
                Vector3 textDir_ifTextShouldBeNonMirroredReadableInCamScreen_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(textDir_ifTextShouldBeNonMirroredReadableInCamScreen_notNormalized);
                if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(textDir_ifTextShouldBeNonMirroredReadableInCamScreen_normalized))
                {
                    //-> amplitudeUp is parallel to cam view dir
                    //-> text anyway not seen in camera
                    textDir_normalized = UtilitiesDXXL_Math.Get_aNormalizedVector_perpToGivenVector(amplitudeUp_normalized);
                }
                else
                {
                    textDir_normalized = textDir_ifTextShouldBeNonMirroredReadableInCamScreen_normalized;
                }
            }
            else
            {
                textDir_normalized = default(Vector3);
            }
        }

        static void GetUpAndTextDir_complyingWithCallerSpecifiedUpVector_forNonShortLine(out Vector3 amplitudeUp_normalized, out Vector3 textDir_normalized, out float lengthOfDrawnLine, out bool lengthOfDrawnLine_isFilled, Vector3 lineStartPos, Vector3 line_startToEnd, Vector3 customAmplitudeAndTextDir, bool lineNeedsA_textDirection, bool theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection, Vector3 observerCamForward_normalized, Vector3 observerCamUp_normalized, Vector3 observerCamRight_normalized, Vector3 cam_to_lineCenter)
        {
            //-> drawnLine is guaranteed longer than zero/longer than minLengthThreshold here
            plane_perpToLine.Recreate(lineStartPos, line_startToEnd);
            Vector3 projection_ofCustomUpVector_alongLineDir_onto_perpToLinePlane_notNormalized = plane_perpToLine.Get_projectionOfVectorOntoPlane(customAmplitudeAndTextDir);
            Vector3 projection_ofCustomUpVector_alongLineDir_onto_perpToLinePlane_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(projection_ofCustomUpVector_alongLineDir_onto_perpToLinePlane_notNormalized);
            if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(projection_ofCustomUpVector_alongLineDir_onto_perpToLinePlane_normalized))
            {
                //user-specified upVector is approx parallel to line:
                GetUpAndTextDir_withoutCallerSpecifiedPreference_accordingToAutomaticAmplitudeAndTextAlignmentSettings(out amplitudeUp_normalized, out textDir_normalized, out lengthOfDrawnLine, out lengthOfDrawnLine_isFilled, lineStartPos, line_startToEnd, lineNeedsA_textDirection, theOnlyThingThatTheLineNeeds_is_upAndTextDirForAnEnlargedText_andThoseDirsAreNotAlignedAlongTheLineBecauseTheLineIsTooShortToHaveADefinedDirection, observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
            }
            else
            {
                //user-specified upVector is not parallel to line:
                amplitudeUp_normalized = projection_ofCustomUpVector_alongLineDir_onto_perpToLinePlane_normalized;
                //<- does not get flipInverted_toFit_automaticTextDirection, also if it leads to upSideDown-texts, because user has specified the direction

                if (lineNeedsA_textDirection)
                {
                    lengthOfDrawnLine_isFilled = true;
                    Vector3 lineDir_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(line_startToEnd, out lengthOfDrawnLine);
                    FlipNormalizedTextDir_toBeNonMirroredReadable_forFixedAmplitudeUpDir(out textDir_normalized, lineDir_normalized, amplitudeUp_normalized, cam_to_lineCenter);
                }
                else
                {
                    textDir_normalized = default(Vector3);
                    lengthOfDrawnLine_isFilled = false;
                    lengthOfDrawnLine = 0.0f;
                }
            }
        }

        static void GetArbitraryUpDir(out Vector3 amplitudeUp_normalized, Vector3 line_startToEnd, bool lineNeedsA_perpNormalizedVector_intoAnArbitraryDirection)
        {
            if (lineNeedsA_perpNormalizedVector_intoAnArbitraryDirection)
            {
                //-> line is thick and needs the arbitraryUpVector for obtaining the non-flattened thickening hullLines
                //-> In other words: used for thick-nonFlat-Lines to get the cylindrical hull line anchors
                //-> drawnLine is guaranteed longer than zero/longer than minLengthThreshold here
                amplitudeUp_normalized = UtilitiesDXXL_Math.Get_aNormalizedVector_perpToGivenVector(line_startToEnd);
            }
            else
            {
                amplitudeUp_normalized = default(Vector3);
            }
        }

        static void FlipNormalizedUpAndTextDir_toFit_automaticTextDirection(out Vector3 amplitudeUp_normalized_postFlip, out Vector3 textDir_normalized_postFlip, Vector3 amplitudeUp_normalized_preFlip, Vector3 textDir_normalized_preFlip, bool amplitudeUp_preFlip_pointsAlreadyToTheSideWhereTextIsUnmirroredReadableInCaseTextTowardsLineEnd, Vector3 cameraUp_normalized, Vector3 cameraRight_normalized, Vector3 cam_to_lineCenter)
        {
            if (DrawBasics.automaticTextDirectionOfLines == DrawBasics.AutomaticTextDirectionOfLines.towardsLineEnd)
            {
                //-> The case "line is vert inside screen" has no special treatment here, because the problem of bistable jittering text direction doesn't exist for "textDirection == towardsLineEnd", because in this case the text direction doesn't flip when passing the verticalDirection.
                textDir_normalized_postFlip = textDir_normalized_preFlip; //-> lineDir is used without any flipInverting
                if (amplitudeUp_preFlip_pointsAlreadyToTheSideWhereTextIsUnmirroredReadableInCaseTextTowardsLineEnd)
                {
                    amplitudeUp_normalized_postFlip = amplitudeUp_normalized_preFlip;
                }
                else
                {
                    FlipNormalizedUp_toBeNonMirroredReadable_withA_fixedTextDir(out amplitudeUp_normalized_postFlip, amplitudeUp_normalized_preFlip, textDir_normalized_postFlip, cam_to_lineCenter);
                }
            }
            else
            {
                //textDir fromLeftToRight_inScreen:
                FlipNormalizedUpAndTextDir_toFit_textDirFromLeftToRightInsideScreen_viaUpDirDictatesVertInsideScreenApproximation(out amplitudeUp_normalized_postFlip, out textDir_normalized_postFlip, amplitudeUp_normalized_preFlip, textDir_normalized_preFlip, cameraUp_normalized, cameraRight_normalized, cam_to_lineCenter);
            }
        }

        static void FlipNormalizedUpAndTextDir_toFit_textDirFromLeftToRightInsideScreen_viaTextDirDictatesVertInsideScreenApproximation(out Vector3 amplitudeUp_normalized_postFlip, out Vector3 textDir_normalized_postFlip, Vector3 amplitudeUp_normalized_preFlip, Vector3 textDir_normalized_preFlip, Vector3 cameraUp_normalized, Vector3 cameraRight_normalized, Vector3 cam_to_lineCenter)
        {
            //This "_viaTextDirDictates"-variant has:
            //-> Advantage compared to "_viaUpDirDictates"-variant: lines that are verticalInGlobalSpace are better detected as "drawnLine_isApproxVertical_insideScreen"
            //-> Disadvantage compared to "_viaUpDirDictates"-variant: The resulting texts may be upSideDown(though at least non-mirrored) even for lines that are quite horizontalInScreenSpace

            //-> camRight always appears as horizLineInScreen, no matter where in the scene it is placed or in which corner of a warping perspective camera it appears
            float dotProductResult_of_textDirPreFlip_and_camRight = Vector3.Dot(textDir_normalized_preFlip, cameraRight_normalized);
            //<- The dot product returns a stable result, because both initialVectors are already normalized. 
            float absDotProductResult_of_textDirPreFlip_and_camRight = Mathf.Abs(dotProductResult_of_textDirPreFlip_and_camRight);
            bool drawnLine_isApproxVertical_insideScreen = absDotProductResult_of_textDirPreFlip_and_camRight < absDotProductResult_ofTwoApproxNormalizedVectors_belowWhichVectorsAreConsideredPerp;
            if (drawnLine_isApproxVertical_insideScreen)
            {
                if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(textDir_normalized_preFlip, cameraUp_normalized))
                {
                    textDir_normalized_postFlip = textDir_normalized_preFlip;
                }
                else
                {
                    textDir_normalized_postFlip = (-textDir_normalized_preFlip);
                }
            }
            else
            {
                if (dotProductResult_of_textDirPreFlip_and_camRight > 0.0f)
                {
                    textDir_normalized_postFlip = textDir_normalized_preFlip;
                }
                else
                {
                    textDir_normalized_postFlip = (-textDir_normalized_preFlip);
                }
            }
            FlipNormalizedUp_toBeNonMirroredReadable_withA_fixedTextDir(out amplitudeUp_normalized_postFlip, amplitudeUp_normalized_preFlip, textDir_normalized_postFlip, cam_to_lineCenter);
        }

        static void FlipNormalizedUpAndTextDir_toFit_textDirFromLeftToRightInsideScreen_viaUpDirDictatesVertInsideScreenApproximation(out Vector3 amplitudeUp_normalized_postFlip, out Vector3 textDir_normalized_postFlip, Vector3 amplitudeUp_normalized_preFlip, Vector3 textDir_normalized_preFlip, Vector3 cameraUp_normalized, Vector3 cameraRight_normalized, Vector3 cam_to_lineCenter)
        {
            //This "_viaUpDirDictates"-variant has:
            //-> Disadvantage compared to "_viaTextDirDictates"-variant: Cannot detect lines that are vertInsideScreen very well, especially not for perspective cameras
            //-> Advantage compared to "_viaTextDirDictates"-variant: But in most cases a resulting upSideDown-text is restricted to a smaller deviation of the lineDir from screenSpaceVert
            //-> The solution that would have fully proper detection would be to first project the drawnLines onto the observerCameraPlane and execute the flipping after that. Though this seems too expensive, at least as long as the current implementation is acceptable.

            //A zero-result-dotProduct here does not mean that upDirPreFlip und camUp are perpInsideScreen:
            //-> camUp always appears as vertLineInScreen, no matter where in the scene it is placed or in which corner of a warping perspective camera it appears
            //-> perpendicularityInGlobalSpace doesn't translate to perpendicularityInScreenSpace, as long as lineDir and lineUp are both parallel to the screenPlane
            //-> even if the zero-result-dotProduct indicates (in some cases) a perpendicularityInScreenspace, then a perpendicularityInScreenspace of lineUp and lineDir is still not guaranteed
            //-> vertLines_inGlobalSpace appear as vertLines_inScreenSpace only for (non-z-turned)orthographic-cameras, but not for perspective cameras
            float dotProductResult_of_upDirPreFlip_and_camUp = Vector3.Dot(amplitudeUp_normalized_preFlip, cameraUp_normalized);
            //<- The dot product returns a stable result, because both initialVectors are already normalized. 
            float absDotProductResult_of_upDirPreFlip_and_camUp = Mathf.Abs(dotProductResult_of_upDirPreFlip_and_camUp);
            bool drawnLine_isApproxVertical_insideScreen = absDotProductResult_of_upDirPreFlip_and_camUp < absDotProductResult_ofTwoApproxNormalizedVectors_belowWhichVectorsAreConsideredPerp;

            if (drawnLine_isApproxVertical_insideScreen)
            {
                //-> This cares for the "bistable case" of vertical lines, where the text direction can "flicker" because (due to float calculation imprecision) it cannot decide wheater to go upward or downward.
                //-> In other words: The fallback here does this: Lines that are "almost" vertical count also as vertical, to prevent the flickering of text (who ongoingly changes it's direction because it cannot decide between "toUp" or "toDown" due to float calculation imprecision) in the common case of vertical(inWorldSpace) lines and vertical cameras.
                //-> The fallback here is, that the text always goes upward in the screen (meaning: same behaviour as "automaticTextDirectionOfLines == towardsLineEnd"), while the "swap" to going downward is not at "exactly vertical line" but at "line angle has to differ from vertical by at least the angle that 'dotProductResult_ofTwoNormalizedVectors_belowWhichVectorsAreConsideredPerp' represents"
                //-> In many cases this thread will not be reached, because as described above the zero-result-dotProduct doesn't detect the common vertLines_inGlobalSpace-case well.
                //-> The lines that come from DrawScreenspace use this regularly, since in this case the zero-result-dotProduct works precisely.
                //-> vertLines_inGlobalSpace for orthographic cameras (that have no z- and no x-rotation) arrive here.

                //-> an undefined bistable result of the dotProduct is not expected, since already before it was checked that "lineDir_normalized" is almost parallel to "cam.up". But if it happens it is no big problem, because the worst case is that the text is going downward(still non-mirrored and readable) instead of the wanted upward.
                if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(textDir_normalized_preFlip, cameraUp_normalized))
                {
                    textDir_normalized_postFlip = textDir_normalized_preFlip;
                }
                else
                {
                    textDir_normalized_postFlip = (-textDir_normalized_preFlip);
                }
                FlipNormalizedUp_toBeNonMirroredReadable_withA_fixedTextDir(out amplitudeUp_normalized_postFlip, amplitudeUp_normalized_preFlip, textDir_normalized_postFlip, cam_to_lineCenter);
            }
            else
            {
                //textDir fromLeftToRight_inScreen - and line is not vertical inside screen

                //this ensures that "amplitudeUp_normalized" (approx)always goes upward inside the screen:
                if (dotProductResult_of_upDirPreFlip_and_camUp > 0.0f)
                {
                    amplitudeUp_normalized_postFlip = amplitudeUp_normalized_preFlip;
                }
                else
                {
                    amplitudeUp_normalized_postFlip = (-amplitudeUp_normalized_preFlip);
                }
                FlipNormalizedTextDir_toBeNonMirroredReadable_forFixedAmplitudeUpDir(out textDir_normalized_postFlip, textDir_normalized_preFlip, amplitudeUp_normalized_postFlip, cam_to_lineCenter);
            }
        }

        static void FlipNormalizedUp_toBeNonMirroredReadable_withA_fixedTextDir(out Vector3 amplitudeUp_normalized_postFlip, Vector3 amplitudeUp_normalized_preFlip, Vector3 fixed_textDir_normalized, Vector3 cam_to_lineCenter)
        {
            Vector3 amplitudeUp_ifTextShouldBeNonMirroredReadableInCamScreen_notNormalized = Vector3.Cross(cam_to_lineCenter, fixed_textDir_normalized);
            //-> "amplitudeUp_ifTextShouldBeNonMirroredReadableInCamScreen_notNormalized" may become zero in seldom edge cases
            //-> but this is not a problem, because in such cases the text would anyway be almost parallel to camViewDir so it is not a big problem if the text will be displayed as mirrored.

            //-> concerning the following dotProduct inside "Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir": It is very unlikely or even impossible that the two vectors become perp. And even if: The worst problem it could produce is that the text is displayed mirrored, so no big problem.
            if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(amplitudeUp_normalized_preFlip, amplitudeUp_ifTextShouldBeNonMirroredReadableInCamScreen_notNormalized))
            {
                amplitudeUp_normalized_postFlip = amplitudeUp_normalized_preFlip;
            }
            else
            {
                amplitudeUp_normalized_postFlip = (-amplitudeUp_normalized_preFlip);
            }
        }

        public static void FlipNormalizedTextDir_toBeNonMirroredReadable_forFixedAmplitudeUpDir(out Vector3 textDir_normalized_postFlip, Vector3 textDir_normalized_preFlip, Vector3 fixed_amplitudeUp_normalized, Vector3 cam_to_lineCenter)
        {
            if (DrawBasics.automaticTextDirectionOfLines == DrawBasics.AutomaticTextDirectionOfLines.towardsLineEnd)
            {
                textDir_normalized_postFlip = textDir_normalized_preFlip;
            }
            else
            {
                //textDir fromLeftToRight in screen:
                Vector3 textDir_ifTextShouldBeNonMirroredReadableInCamScreen_notNormalized_notParallelToLine = Vector3.Cross(fixed_amplitudeUp_normalized, cam_to_lineCenter);
                //crossProduct or dotProduct result of 0 is not a big problem here, because that means that either the upVector or the lineDir are along camera view dir: So the text will anyway not be seen an the correct "direction flipping" of the textDir doesn't matter in such cases.
                if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(textDir_ifTextShouldBeNonMirroredReadableInCamScreen_notNormalized_notParallelToLine, textDir_normalized_preFlip))
                {
                    textDir_normalized_postFlip = textDir_normalized_preFlip;
                }
                else
                {
                    textDir_normalized_postFlip = (-textDir_normalized_preFlip);
                }
            }
        }

    }

}
