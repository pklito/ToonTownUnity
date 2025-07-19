namespace DrawXXL
{
    using UnityEngine;

    public class UtilitiesDXXL_Euler
    {
        static float eulerAxes_relLineWidth = 0.03f;
        static float eulerAxesTurnAngleVisualizer_relLineWidth = 0.006f;
        static float relRadius_ofSpheres = 0.07f;
        static float stripeSizeFactor_forGimbelLocksAlternatingColorLine = 0.08f;
        static float relRadius_ofAxisTurnAngleVisualizer = 0.15f;
        static InternalDXXL_Line rotatedXAxis_line_inGlobalSpaceUnits = new InternalDXXL_Line();
        static InternalDXXL_Line rotatedYAxis_line_inGlobalSpaceUnits = new InternalDXXL_Line();
        static InternalDXXL_Line rotatedZAxis_line_inGlobalSpaceUnits = new InternalDXXL_Line();
        static float angleDeg_betweenTurnedCircleSlice_visualizerLines = 2.912347f;
        static float approxLength_ofShortestTurnedVectorThatIsDrawn = 0.01f;

        public static Vector3 GetEulerAnglesFromNonNullTransform(Transform transform, bool useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay, bool useLocalRotation_notGlobal)
        {
            if (useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay)
            {
                return GetEulerAnglesFromNonNullTransform_asReturnedByAPIcalls_notAsShownInInspector(transform, useLocalRotation_notGlobal);
            }
            else
            {
#if UNITY_EDITOR
                //"useLocalRotation_notGlobal": The caller has to handle the case where global angles are requested, because this always returns local angles.
                return UnityEditor.TransformUtils.GetInspectorRotation(transform); //this returns the local eulerAngles. The angles that are displayed in the transform inspector are local angels.
#else
                return GetEulerAnglesFromNonNullTransform_asReturnedByAPIcalls_notAsShownInInspector(transform, useLocalRotation_notGlobal);
#endif
            }
        }

        static Vector3 GetEulerAnglesFromNonNullTransform_asReturnedByAPIcalls_notAsShownInInspector(Transform transform, bool useLocalRotation_notGlobal)
        {
            if (useLocalRotation_notGlobal)
            {
                return transform.localEulerAngles;
            }
            else
            {
                return transform.eulerAngles;
            }
        }

        public static void EulerRotation_local(Vector3 eulerAnglesToDraw, Vector3 posWhereToDraw, Vector3 customVectorToRotate_local, float length_ofUpAndForwardVectors_local, float alpha_ofSquareSpannedByForwardAndUp, float alpha_ofUnrotatedGimbalAxes, float gimbalSize, string text, bool isLocal, float durationInSec, bool hiddenByNearerObjects, Transform parentTransform)
        {
            //-> The order of drawing of the individual elements inside this function has effect on the readability of the gimbal
            //-> This is because the spacial state of the gimbal is comprehended by the question "which axis is in front?"
            //-> The z-ordering of "Debug.DrawLine()" works like this: Later calls are in front
            //-> So which axes are drawn "last(=on top of all others)" is dependent on the observer direction. That's why this function is dependent on "DrawBasics.cameraForAutomaticOrientation"

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(length_ofUpAndForwardVectors_local, "length_ofUpAndForwardVectors")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(alpha_ofSquareSpannedByForwardAndUp, "alpha_ofSquareSpannedByForwardAndUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(gimbalSize, "gimbalSize")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(alpha_ofUnrotatedGimbalAxes, "alpha_ofUnrotatedGimbalAxes")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(eulerAnglesToDraw, "eulerAnglesToDraw")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(posWhereToDraw, "posWhereToDraw")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(customVectorToRotate_local, "customVectorToRotate")) { return; }

            //"gimbalSize" of 1 means: gimbalAxes are 1 unit long to each side, so 2 units in total.
            gimbalSize = Mathf.Max(gimbalSize, 0.01f);


            Quaternion rotation_ofLocalSpace = (parentTransform == null) ? Quaternion.identity : parentTransform.rotation;



            //The following declaration blocks are sometimes ordered "x -> y -> z" (default) and sometimes "y -> x -> z" (main axis to most dependent axis):

            //Note that "normalized in local space" is the same as "normalized in global space", because scale_ofSpaces is not used here:
            Vector3 yAxis_unrotated_inLocalSpace_normalized = Vector3.up;
            Vector3 xAxis_unrotated_inLocalSpace_normalized = Vector3.right;
            Vector3 zAxis_unrotated_inLocalSpace_normalized = Vector3.forward;

            Quaternion eulerRotationAroundYAxis = Quaternion.AngleAxis(eulerAnglesToDraw.y, yAxis_unrotated_inLocalSpace_normalized);
            Quaternion eulerRotationAroundXAxis = Quaternion.AngleAxis(eulerAnglesToDraw.x, xAxis_unrotated_inLocalSpace_normalized);
            Quaternion eulerRotationAroundZAxis = Quaternion.AngleAxis(eulerAnglesToDraw.z, zAxis_unrotated_inLocalSpace_normalized);

            Vector3 yAxis_rotated_inLocalSpace_normalized = yAxis_unrotated_inLocalSpace_normalized;
            Vector3 xAxis_rotated_inLocalSpace_normalized = eulerRotationAroundYAxis * xAxis_unrotated_inLocalSpace_normalized;
            Vector3 zAxis_rotated_inLocalSpace_normalized = eulerRotationAroundYAxis * eulerRotationAroundXAxis * zAxis_unrotated_inLocalSpace_normalized;

            Vector3 zAxis_rotatedOnlyAroundY_inLocalSpace_normalized = eulerRotationAroundYAxis * zAxis_unrotated_inLocalSpace_normalized;

            Vector3 yAxis_rotated_inGlobalSpace_normalized = rotation_ofLocalSpace * yAxis_rotated_inLocalSpace_normalized;
            Vector3 xAxis_rotated_inGlobalSpace_normalized = rotation_ofLocalSpace * xAxis_rotated_inLocalSpace_normalized;
            Vector3 zAxis_rotated_inGlobalSpace_normalized = rotation_ofLocalSpace * zAxis_rotated_inLocalSpace_normalized;

            Vector3 xAxis_unrotated_inGlobalSpace_normalized = rotation_ofLocalSpace * xAxis_unrotated_inLocalSpace_normalized;
            Vector3 zAxis_unrotated_inGlobalSpace_normalized = rotation_ofLocalSpace * zAxis_unrotated_inLocalSpace_normalized;

            Vector3 zAxis_rotatedOnlyAroundY_inGlobalSpace_normalized = rotation_ofLocalSpace * zAxis_rotatedOnlyAroundY_inLocalSpace_normalized;

            Vector3 fromDrawCenterPos_toYAxisPeak_inGlobalSpace = gimbalSize * yAxis_rotated_inGlobalSpace_normalized;
            Vector3 fromDrawCenterPos_toXAxisPeak_inGlobalSpace = gimbalSize * xAxis_rotated_inGlobalSpace_normalized;
            Vector3 fromDrawCenterPos_toZAxisPeak_inGlobalSpace = gimbalSize * zAxis_rotated_inGlobalSpace_normalized;

            Vector3 fromDrawCenterPos_toXAxisUnrotatedPeak_inGlobalSpace = gimbalSize * xAxis_unrotated_inGlobalSpace_normalized;
            Vector3 fromDrawCenterPos_toZAxisUnrotatedPeak_inGlobalSpace = gimbalSize * zAxis_unrotated_inGlobalSpace_normalized;

            Vector3 fromDrawCenterPos_toZAxisRotatedOnlyAroundYPeak_inGlobalSpace = gimbalSize * zAxis_rotatedOnlyAroundY_inGlobalSpace_normalized;

            Vector3 xAxis_startPos_inGlobalSpace = posWhereToDraw - fromDrawCenterPos_toXAxisPeak_inGlobalSpace;
            Vector3 xAxis_endPos_inGlobalSpace = posWhereToDraw + fromDrawCenterPos_toXAxisPeak_inGlobalSpace;

            Vector3 yAxis_startPos_inGlobalSpace = posWhereToDraw - fromDrawCenterPos_toYAxisPeak_inGlobalSpace;
            Vector3 yAxis_endPos_inGlobalSpace = posWhereToDraw + fromDrawCenterPos_toYAxisPeak_inGlobalSpace;

            Vector3 zAxis_startPos_inGlobalSpace = posWhereToDraw - fromDrawCenterPos_toZAxisPeak_inGlobalSpace;
            Vector3 zAxis_endPos_inGlobalSpace = posWhereToDraw + fromDrawCenterPos_toZAxisPeak_inGlobalSpace;

            Vector3 xAxisUnrotated_startPos_inGlobalSpace = posWhereToDraw - fromDrawCenterPos_toXAxisUnrotatedPeak_inGlobalSpace;
            Vector3 xAxisUnrotated_endPos_inGlobalSpace = posWhereToDraw + fromDrawCenterPos_toXAxisUnrotatedPeak_inGlobalSpace;
            Vector3 xAxisUnrotatedShortened_endPos_inGlobalSpace = posWhereToDraw + fromDrawCenterPos_toXAxisUnrotatedPeak_inGlobalSpace * 0.328f;

            Vector3 zAxisUnrotated_startPos_inGlobalSpace = posWhereToDraw - fromDrawCenterPos_toZAxisUnrotatedPeak_inGlobalSpace;
            Vector3 zAxisUnrotated_endPos_inGlobalSpace = posWhereToDraw + fromDrawCenterPos_toZAxisUnrotatedPeak_inGlobalSpace;
            Vector3 zAxisUnrotatedShortened_endPos_inGlobalSpace = posWhereToDraw + fromDrawCenterPos_toZAxisUnrotatedPeak_inGlobalSpace * 0.355f;

            Vector3 zAxisRotatedOnlyAroundYShortened_endPos_inGlobalSpace = posWhereToDraw + fromDrawCenterPos_toZAxisRotatedOnlyAroundYPeak_inGlobalSpace * 0.3575f;

            Vector3 perpToYAxis_markingYAxisZeroTurnAngle_unrotated_inLocalSpace_normalized = Vector3.forward;
            Vector3 perpToXAxis_markingXAxisZeroTurnAngle_unrotated_inLocalSpace_normalized = Vector3.up;
            Vector3 perpToZAxis_markingZAxisZeroTurnAngle_unrotated_inLocalSpace_normalized = Vector3.up;

            Vector3 perpToYAxis_markingYAxisEndTurnAngle_unrotated_inLocalSpace_normalized = eulerRotationAroundYAxis * perpToYAxis_markingYAxisZeroTurnAngle_unrotated_inLocalSpace_normalized;
            Vector3 perpToXAxis_markingXAxisEndTurnAngle_unrotated_inLocalSpace_normalized = eulerRotationAroundXAxis * perpToXAxis_markingXAxisZeroTurnAngle_unrotated_inLocalSpace_normalized;
            Vector3 perpToZAxis_markingZAxisEndTurnAngle_unrotated_inLocalSpace_normalized = eulerRotationAroundZAxis * perpToZAxis_markingZAxisZeroTurnAngle_unrotated_inLocalSpace_normalized;

            Vector3 perpToYAxis_markingYAxisZeroTurnAngle_rotated_inLocalSpace_normalized = perpToYAxis_markingYAxisZeroTurnAngle_unrotated_inLocalSpace_normalized;
            Vector3 perpToXAxis_markingXAxisZeroTurnAngle_rotated_inLocalSpace_normalized = eulerRotationAroundYAxis * perpToXAxis_markingXAxisZeroTurnAngle_unrotated_inLocalSpace_normalized;
            Vector3 perpToZAxis_markingZAxisZeroTurnAngle_rotated_inLocalSpace_normalized = eulerRotationAroundYAxis * eulerRotationAroundXAxis * perpToZAxis_markingZAxisZeroTurnAngle_unrotated_inLocalSpace_normalized;

            Vector3 perpToYAxis_markingYAxisEndTurnAngle_rotated_inLocalSpace_normalized = perpToYAxis_markingYAxisEndTurnAngle_unrotated_inLocalSpace_normalized;
            Vector3 perpToXAxis_markingXAxisEndTurnAngle_rotated_inLocalSpace_normalized = eulerRotationAroundYAxis * perpToXAxis_markingXAxisEndTurnAngle_unrotated_inLocalSpace_normalized;
            Vector3 perpToZAxis_markingZAxisEndTurnAngle_rotated_inLocalSpace_normalized = eulerRotationAroundYAxis * eulerRotationAroundXAxis * perpToZAxis_markingZAxisEndTurnAngle_unrotated_inLocalSpace_normalized;

            Vector3 perpToYAxis_markingYAxisZeroTurnAngle_rotated_inGlobalSpace_normalized = rotation_ofLocalSpace * perpToYAxis_markingYAxisZeroTurnAngle_rotated_inLocalSpace_normalized;
            Vector3 perpToXAxis_markingXAxisZeroTurnAngle_rotated_inGlobalSpace_normalized = rotation_ofLocalSpace * perpToXAxis_markingXAxisZeroTurnAngle_rotated_inLocalSpace_normalized;
            Vector3 perpToZAxis_markingZAxisZeroTurnAngle_rotated_inGlobalSpace_normalized = rotation_ofLocalSpace * perpToZAxis_markingZAxisZeroTurnAngle_rotated_inLocalSpace_normalized;

            Vector3 perpToYAxis_markingYAxisEndTurnAngle_rotated_inGlobalSpace_normalized = rotation_ofLocalSpace * perpToYAxis_markingYAxisEndTurnAngle_rotated_inLocalSpace_normalized;
            Vector3 perpToXAxis_markingXAxisEndTurnAngle_rotated_inGlobalSpace_normalized = rotation_ofLocalSpace * perpToXAxis_markingXAxisEndTurnAngle_rotated_inLocalSpace_normalized;
            Vector3 perpToZAxis_markingZAxisEndTurnAngle_rotated_inGlobalSpace_normalized = rotation_ofLocalSpace * perpToZAxis_markingZAxisEndTurnAngle_rotated_inLocalSpace_normalized;

            float lineWidth_ofAxes = eulerAxes_relLineWidth * gimbalSize;
            float radius_ofSpheres = relRadius_ofSpheres * gimbalSize;
            float diameter_ofSpheres = 2.0f * radius_ofSpheres;
            float radius_ofYAxisHolderRing = diameter_ofSpheres;
            float absConeLength = 0.20f * gimbalSize;
            Vector3 normalOfYAxisHolderRing = Vector3.Cross(yAxis_rotated_inGlobalSpace_normalized, xAxis_rotated_inGlobalSpace_normalized);
            float angleDeg_ofHalfYAxisHolderRingSegment = 64.0f;

            UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out Vector3 observerCamForward_normalized, out Vector3 observerCamUp_normalized, out Vector3 observerCamRight_normalized, out Vector3 cam_to_posWhereToDraw, posWhereToDraw, Vector3.zero, null);

            bool isInGimbalLock = UtilitiesDXXL_Math.Check_ifTwoNormalizedVectorsAreApproxParallel_butCanHeadToDifferntDirs_padding(yAxis_rotated_inGlobalSpace_normalized, zAxis_rotated_inGlobalSpace_normalized);

            bool xAxis_pointsAwayFromObserverCam = UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(observerCamForward_normalized, xAxis_rotated_inGlobalSpace_normalized);
            bool yAxis_pointsAwayFromObserverCam = UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(observerCamForward_normalized, yAxis_rotated_inGlobalSpace_normalized);
            bool zAxis_pointsAwayFromObserverCam = UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(observerCamForward_normalized, zAxis_rotated_inGlobalSpace_normalized);

            Vector3 vector_parallelToTurnedYAxis_dirIsTowardsObserverCam_normalized = yAxis_pointsAwayFromObserverCam ? (-yAxis_rotated_inGlobalSpace_normalized) : yAxis_rotated_inGlobalSpace_normalized;
            Vector3 vector_parallelToTurnedZAxis_dirIsTowardsObserverCam_normalized = zAxis_pointsAwayFromObserverCam ? (-zAxis_rotated_inGlobalSpace_normalized) : zAxis_rotated_inGlobalSpace_normalized;

            Vector3 aRefVector_perpToXAxis_perpToObserverViewDir = Vector3.Cross(xAxis_rotated_inGlobalSpace_normalized, cam_to_posWhereToDraw);
            aRefVector_perpToXAxis_perpToObserverViewDir = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(aRefVector_perpToXAxis_perpToObserverViewDir);

            float dotProduct_ofYAxisFlippedTowardsCam_and_refVectorPerpToXAxisAndCamViewDir = Vector3.Dot(vector_parallelToTurnedYAxis_dirIsTowardsObserverCam_normalized, aRefVector_perpToXAxis_perpToObserverViewDir);
            float dotProduct_ofZAxisFlippedTowardsCam_and_refVectorPerpToXAxisAndCamViewDir = Vector3.Dot(vector_parallelToTurnedZAxis_dirIsTowardsObserverCam_normalized, aRefVector_perpToXAxis_perpToObserverViewDir);

            bool theTowardsCamPointingPartOf_YAndZAxis_appearOnTheSameSideOfTheXAxis = Mathf.Sign(dotProduct_ofYAxisFlippedTowardsCam_and_refVectorPerpToXAxisAndCamViewDir) == Mathf.Sign(dotProduct_ofZAxisFlippedTowardsCam_and_refVectorPerpToXAxisAndCamViewDir);
            bool theTowardsCamPointingPartOf_yAxis_isNearerToObserverCam_thanTheTowardsCamPointingPartOf_zAxis;
            bool theAwayFromCamPointingPartOf_yAxis_isNearerToObserverCam_thanTheAwayFromCamPointingPartOf_zAxis;

            if (theTowardsCamPointingPartOf_YAndZAxis_appearOnTheSameSideOfTheXAxis)
            {
                float dotProduct_ofYAxisFlippedTowardsCam_and_inverseCamViewDir = Vector3.Dot(vector_parallelToTurnedYAxis_dirIsTowardsObserverCam_normalized, (-cam_to_posWhereToDraw));
                float dotProduct_ofZAxisFlippedTowardsCam_and_inverseCamViewDir = Vector3.Dot(vector_parallelToTurnedZAxis_dirIsTowardsObserverCam_normalized, (-cam_to_posWhereToDraw));
                if (dotProduct_ofYAxisFlippedTowardsCam_and_inverseCamViewDir > dotProduct_ofZAxisFlippedTowardsCam_and_inverseCamViewDir)
                {
                    theTowardsCamPointingPartOf_yAxis_isNearerToObserverCam_thanTheTowardsCamPointingPartOf_zAxis = true;
                    theAwayFromCamPointingPartOf_yAxis_isNearerToObserverCam_thanTheAwayFromCamPointingPartOf_zAxis = false;
                }
                else
                {
                    theTowardsCamPointingPartOf_yAxis_isNearerToObserverCam_thanTheTowardsCamPointingPartOf_zAxis = false;
                    theAwayFromCamPointingPartOf_yAxis_isNearerToObserverCam_thanTheAwayFromCamPointingPartOf_zAxis = true;
                }
            }
            else
            {
                //theTowardsCamPointingPartOf_YAndZAxis_appearOn-DIFFERENT-sidesOfTheXAxis:
                //-> the "towardsCamPointing"-parts and also the "awayPointing"-parts of y and z both cannot overlap each other
                //-> no z-fighting
                //-> no correction neccessary

                //will not get used:
                theTowardsCamPointingPartOf_yAxis_isNearerToObserverCam_thanTheTowardsCamPointingPartOf_zAxis = true;
                theAwayFromCamPointingPartOf_yAxis_isNearerToObserverCam_thanTheAwayFromCamPointingPartOf_zAxis = false;
            }

            TryDrawUnrotatedXAndZAxis(alpha_ofUnrotatedGimbalAxes, posWhereToDraw, xAxisUnrotated_startPos_inGlobalSpace, xAxisUnrotated_endPos_inGlobalSpace, zAxisUnrotated_startPos_inGlobalSpace, zAxisUnrotated_endPos_inGlobalSpace, xAxis_rotated_inGlobalSpace_normalized, yAxis_rotated_inGlobalSpace_normalized, xAxisUnrotatedShortened_endPos_inGlobalSpace, zAxisUnrotatedShortened_endPos_inGlobalSpace, zAxisRotatedOnlyAroundYShortened_endPos_inGlobalSpace, eulerAnglesToDraw.x, eulerAnglesToDraw.y, lineWidth_ofAxes, absConeLength, gimbalSize, durationInSec, hiddenByNearerObjects);
            if (theAwayFromCamPointingPartOf_yAxis_isNearerToObserverCam_thanTheAwayFromCamPointingPartOf_zAxis)
            {
                //drawing y AFTER z and x:
                DrawFirstPassOfZAxis(zAxis_pointsAwayFromObserverCam, zAxis_rotated_inGlobalSpace_normalized, zAxis_startPos_inGlobalSpace, zAxis_endPos_inGlobalSpace, perpToZAxis_markingZAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, perpToZAxis_markingZAxisEndTurnAngle_rotated_inGlobalSpace_normalized, eulerAnglesToDraw.z, lineWidth_ofAxes, absConeLength, gimbalSize, isLocal, durationInSec, hiddenByNearerObjects);
                DrawFirstPassOfXAxis(xAxis_pointsAwayFromObserverCam, xAxis_rotated_inGlobalSpace_normalized, xAxis_startPos_inGlobalSpace, xAxis_endPos_inGlobalSpace, perpToXAxis_markingXAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, perpToXAxis_markingXAxisEndTurnAngle_rotated_inGlobalSpace_normalized, eulerAnglesToDraw.x, lineWidth_ofAxes, absConeLength, gimbalSize, isLocal, durationInSec, hiddenByNearerObjects);
                DrawAwayFacingPartofYAxis(yAxis_pointsAwayFromObserverCam, posWhereToDraw, yAxis_rotated_inGlobalSpace_normalized, yAxis_startPos_inGlobalSpace, yAxis_endPos_inGlobalSpace, normalOfYAxisHolderRing, perpToYAxis_markingYAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, perpToYAxis_markingYAxisEndTurnAngle_rotated_inGlobalSpace_normalized, angleDeg_ofHalfYAxisHolderRingSegment, radius_ofYAxisHolderRing, eulerAnglesToDraw.y, lineWidth_ofAxes, absConeLength, gimbalSize, isLocal, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                //drawing y BEFORE z and x:
                DrawAwayFacingPartofYAxis(yAxis_pointsAwayFromObserverCam, posWhereToDraw, yAxis_rotated_inGlobalSpace_normalized, yAxis_startPos_inGlobalSpace, yAxis_endPos_inGlobalSpace, normalOfYAxisHolderRing, perpToYAxis_markingYAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, perpToYAxis_markingYAxisEndTurnAngle_rotated_inGlobalSpace_normalized, angleDeg_ofHalfYAxisHolderRingSegment, radius_ofYAxisHolderRing, eulerAnglesToDraw.y, lineWidth_ofAxes, absConeLength, gimbalSize, isLocal, durationInSec, hiddenByNearerObjects);
                DrawFirstPassOfZAxis(zAxis_pointsAwayFromObserverCam, zAxis_rotated_inGlobalSpace_normalized, zAxis_startPos_inGlobalSpace, zAxis_endPos_inGlobalSpace, perpToZAxis_markingZAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, perpToZAxis_markingZAxisEndTurnAngle_rotated_inGlobalSpace_normalized, eulerAnglesToDraw.z, lineWidth_ofAxes, absConeLength, gimbalSize, isLocal, durationInSec, hiddenByNearerObjects);
                DrawFirstPassOfXAxis(xAxis_pointsAwayFromObserverCam, xAxis_rotated_inGlobalSpace_normalized, xAxis_startPos_inGlobalSpace, xAxis_endPos_inGlobalSpace, perpToXAxis_markingXAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, perpToXAxis_markingXAxisEndTurnAngle_rotated_inGlobalSpace_normalized, eulerAnglesToDraw.x, lineWidth_ofAxes, absConeLength, gimbalSize, isLocal, durationInSec, hiddenByNearerObjects);
            }

            if (isInGimbalLock)
            {
                DrawAwayPointingPartOfYAxis_asGimbalLockHybrid(yAxis_pointsAwayFromObserverCam, posWhereToDraw, yAxis_startPos_inGlobalSpace, yAxis_rotated_inGlobalSpace_normalized, fromDrawCenterPos_toYAxisPeak_inGlobalSpace, radius_ofYAxisHolderRing, lineWidth_ofAxes, gimbalSize, durationInSec, hiddenByNearerObjects);
            }

            int struts_ofSpheres = 24;
            Vector3 offset_ofGreenSpheres_alongXAxis = diameter_ofSpheres * xAxis_rotated_inGlobalSpace_normalized;
            Vector3 offset_ofFarerBetweenSpheresFlange_alongXAxis = 0.6f * diameter_ofSpheres * xAxis_rotated_inGlobalSpace_normalized;
            Vector3 offset_ofNearerBetweenSpheresFlange_alongXAxis = 0.4f * diameter_ofSpheres * xAxis_rotated_inGlobalSpace_normalized;
            Color color_ofRedXFlange = Color.Lerp(UtilitiesDXXL_Colors.red_xAxisAlpha1, Color.black, 0.35f);
            Color color_ofGreenYFlange = Color.Lerp(UtilitiesDXXL_Colors.green_yAxisAlpha1, Color.black, 0.4f);
            Color color_ofBlueZFlange = Color.Lerp(UtilitiesDXXL_Colors.blue_zAxisAlpha1, Color.black, 0.4f);
            float distance_centerToFarFlanges = 1.4f * diameter_ofSpheres;

            if (xAxis_pointsAwayFromObserverCam)
            {
                DrawShapes.Sphere(posWhereToDraw + offset_ofGreenSpheres_alongXAxis, radius_ofSpheres, UtilitiesDXXL_Colors.green_yAxisAlpha1, fromDrawCenterPos_toXAxisPeak_inGlobalSpace, default(Vector3), 0.0f, null, struts_ofSpheres, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);
                DrawDoubleFlange_closedHole(posWhereToDraw + offset_ofFarerBetweenSpheresFlange_alongXAxis, xAxis_rotated_inGlobalSpace_normalized, radius_ofSpheres, color_ofRedXFlange, color_ofGreenYFlange, durationInSec, hiddenByNearerObjects);
                DrawShapes.Sphere(posWhereToDraw, radius_ofSpheres, UtilitiesDXXL_Colors.red_xAxisAlpha1, fromDrawCenterPos_toZAxisPeak_inGlobalSpace, default(Vector3), 0.0f, null, struts_ofSpheres, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);
                DrawSingleFlange(posWhereToDraw - offset_ofNearerBetweenSpheresFlange_alongXAxis, xAxis_rotated_inGlobalSpace_normalized, radius_ofSpheres, color_ofRedXFlange, durationInSec, hiddenByNearerObjects);
                DrawShapes.Sphere(posWhereToDraw - offset_ofGreenSpheres_alongXAxis, radius_ofSpheres, UtilitiesDXXL_Colors.green_yAxisAlpha1, fromDrawCenterPos_toXAxisPeak_inGlobalSpace, default(Vector3), 0.0f, null, struts_ofSpheres, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);

                DrawCamPointingSecondPassOfXAxis(xAxis_pointsAwayFromObserverCam, posWhereToDraw, xAxis_rotated_inGlobalSpace_normalized, xAxis_startPos_inGlobalSpace, xAxis_endPos_inGlobalSpace, perpToXAxis_markingXAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, perpToXAxis_markingXAxisEndTurnAngle_rotated_inGlobalSpace_normalized, color_ofRedXFlange, color_ofGreenYFlange, eulerAnglesToDraw.x, lineWidth_ofAxes, radius_ofSpheres, distance_centerToFarFlanges, absConeLength, gimbalSize, isLocal, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                DrawShapes.Sphere(posWhereToDraw - offset_ofGreenSpheres_alongXAxis, radius_ofSpheres, UtilitiesDXXL_Colors.green_yAxisAlpha1, fromDrawCenterPos_toXAxisPeak_inGlobalSpace, default(Vector3), 0.0f, null, struts_ofSpheres, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);
                DrawDoubleFlange_closedHole(posWhereToDraw - offset_ofFarerBetweenSpheresFlange_alongXAxis, xAxis_rotated_inGlobalSpace_normalized, radius_ofSpheres, color_ofRedXFlange, color_ofGreenYFlange, durationInSec, hiddenByNearerObjects);
                DrawShapes.Sphere(posWhereToDraw, radius_ofSpheres, UtilitiesDXXL_Colors.red_xAxisAlpha1, fromDrawCenterPos_toZAxisPeak_inGlobalSpace, default(Vector3), 0.0f, null, struts_ofSpheres, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);
                DrawSingleFlange(posWhereToDraw + offset_ofNearerBetweenSpheresFlange_alongXAxis, xAxis_rotated_inGlobalSpace_normalized, radius_ofSpheres, color_ofRedXFlange, durationInSec, hiddenByNearerObjects);
                DrawShapes.Sphere(posWhereToDraw + offset_ofGreenSpheres_alongXAxis, radius_ofSpheres, UtilitiesDXXL_Colors.green_yAxisAlpha1, fromDrawCenterPos_toXAxisPeak_inGlobalSpace, default(Vector3), 0.0f, null, struts_ofSpheres, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);

                DrawCamPointingSecondPassOfXAxis(xAxis_pointsAwayFromObserverCam, posWhereToDraw, xAxis_rotated_inGlobalSpace_normalized, xAxis_startPos_inGlobalSpace, xAxis_endPos_inGlobalSpace, perpToXAxis_markingXAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, perpToXAxis_markingXAxisEndTurnAngle_rotated_inGlobalSpace_normalized, color_ofRedXFlange, color_ofGreenYFlange, eulerAnglesToDraw.x, lineWidth_ofAxes, radius_ofSpheres, distance_centerToFarFlanges, absConeLength, gimbalSize, isLocal, durationInSec, hiddenByNearerObjects);
            }

            if (theTowardsCamPointingPartOf_yAxis_isNearerToObserverCam_thanTheTowardsCamPointingPartOf_zAxis)
            {
                DrawCamPointingSecondPassOfZAxis(zAxis_pointsAwayFromObserverCam, posWhereToDraw, zAxis_rotated_inGlobalSpace_normalized, zAxis_startPos_inGlobalSpace, zAxis_endPos_inGlobalSpace, perpToZAxis_markingZAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, perpToZAxis_markingZAxisEndTurnAngle_rotated_inGlobalSpace_normalized, radius_ofSpheres, diameter_ofSpheres, eulerAnglesToDraw.z, lineWidth_ofAxes, absConeLength, gimbalSize, color_ofRedXFlange, color_ofBlueZFlange, isLocal, durationInSec, hiddenByNearerObjects);
                DrawCamPointingSecondPassOfYAxis(yAxis_pointsAwayFromObserverCam, posWhereToDraw, yAxis_rotated_inGlobalSpace_normalized, yAxis_startPos_inGlobalSpace, yAxis_endPos_inGlobalSpace, normalOfYAxisHolderRing, perpToYAxis_markingYAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, perpToYAxis_markingYAxisEndTurnAngle_rotated_inGlobalSpace_normalized, angleDeg_ofHalfYAxisHolderRingSegment, radius_ofYAxisHolderRing, eulerAnglesToDraw.y, lineWidth_ofAxes, absConeLength, gimbalSize, isLocal, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                DrawCamPointingSecondPassOfYAxis(yAxis_pointsAwayFromObserverCam, posWhereToDraw, yAxis_rotated_inGlobalSpace_normalized, yAxis_startPos_inGlobalSpace, yAxis_endPos_inGlobalSpace, normalOfYAxisHolderRing, perpToYAxis_markingYAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, perpToYAxis_markingYAxisEndTurnAngle_rotated_inGlobalSpace_normalized, angleDeg_ofHalfYAxisHolderRingSegment, radius_ofYAxisHolderRing, eulerAnglesToDraw.y, lineWidth_ofAxes, absConeLength, gimbalSize, isLocal, durationInSec, hiddenByNearerObjects);
                DrawCamPointingSecondPassOfZAxis(zAxis_pointsAwayFromObserverCam, posWhereToDraw, zAxis_rotated_inGlobalSpace_normalized, zAxis_startPos_inGlobalSpace, zAxis_endPos_inGlobalSpace, perpToZAxis_markingZAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, perpToZAxis_markingZAxisEndTurnAngle_rotated_inGlobalSpace_normalized, radius_ofSpheres, diameter_ofSpheres, eulerAnglesToDraw.z, lineWidth_ofAxes, absConeLength, gimbalSize, color_ofRedXFlange, color_ofBlueZFlange, isLocal, durationInSec, hiddenByNearerObjects);
            }

            float size_ofGimbalLockText = 0.10f * gimbalSize;
            if (isInGimbalLock)
            {
                DrawCamPointingPartOfYAxis_asGimbalLockHybrid(yAxis_pointsAwayFromObserverCam, posWhereToDraw, yAxis_startPos_inGlobalSpace, yAxis_rotated_inGlobalSpace_normalized, fromDrawCenterPos_toYAxisPeak_inGlobalSpace, radius_ofYAxisHolderRing, lineWidth_ofAxes, gimbalSize, durationInSec, hiddenByNearerObjects);
                Color color_ofGimbalLockText = new Color(1.0f, 0.6473441f, 0.5707547f);
                UtilitiesDXXL_Text.WriteFramed("<sw=80000>GIMBAL    LOCK  </sw><br><size=1> </size><br>", posWhereToDraw, color_ofGimbalLockText, size_ofGimbalLockText, yAxis_rotated_inGlobalSpace_normalized, default(Vector3), DrawText.TextAnchorDXXL.LowerCenter, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                bool isNearGimbalLock = UtilitiesDXXL_Math.Check_ifTwoNormalizedVectorsAreApproxParallel_butCanHeadToDifferntDirs_padding(yAxis_rotated_inGlobalSpace_normalized, zAxis_rotated_inGlobalSpace_normalized, 0.02f);
                if (isNearGimbalLock)
                {
                    Color color_ofNearGimbalLockText = new Color(1.0f, 0.6473441f, 0.5707547f);
                    UtilitiesDXXL_Text.WriteFramed("<sw=10000>NEAR GIMBAL LOCK</sw><br><size=1> </size><br>", posWhereToDraw, color_ofNearGimbalLockText, size_ofGimbalLockText, yAxis_rotated_inGlobalSpace_normalized, default(Vector3), DrawText.TextAnchorDXXL.LowerCenter, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
                }
            }

            float maxAbsDistance_ofAnyPlumbPos_toTurnCenter = DrawTurnedVectors(customVectorToRotate_local, length_ofUpAndForwardVectors_local, parentTransform, alpha_ofSquareSpannedByForwardAndUp, posWhereToDraw, eulerAnglesToDraw, rotation_ofLocalSpace, yAxis_unrotated_inLocalSpace_normalized, xAxis_rotated_inLocalSpace_normalized, zAxis_rotated_inLocalSpace_normalized, xAxis_rotated_inGlobalSpace_normalized, yAxis_rotated_inGlobalSpace_normalized, zAxis_rotated_inGlobalSpace_normalized, gimbalSize, isLocal, durationInSec, hiddenByNearerObjects);

            if (maxAbsDistance_ofAnyPlumbPos_toTurnCenter > (gimbalSize * 1.001f)) //-> factor prevents on/off-flicker due to float calculation imprecision
            {
                DrawThinDashedAxesProlongations(xAxis_startPos_inGlobalSpace, xAxis_endPos_inGlobalSpace, yAxis_startPos_inGlobalSpace, yAxis_endPos_inGlobalSpace, zAxis_startPos_inGlobalSpace, zAxis_endPos_inGlobalSpace, xAxis_rotated_inGlobalSpace_normalized, yAxis_rotated_inGlobalSpace_normalized, zAxis_rotated_inGlobalSpace_normalized, gimbalSize, maxAbsDistance_ofAnyPlumbPos_toTurnCenter, durationInSec, hiddenByNearerObjects);
            }

            if (text != null && text != "")
            {
                Vector3 position_ofText = posWhereToDraw + yAxis_rotated_inGlobalSpace_normalized * 1.13f * gimbalSize;
                float size_ofText = 0.05f * gimbalSize;
                UtilitiesDXXL_Text.WriteFramed(text, position_ofText, Color.white, size_ofText, default(Vector3), Vector3.up, DrawText.TextAnchorDXXL.LowerCenter, DrawBasics.LineStyle.solid, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
            }
        }

        static void TryDrawUnrotatedXAndZAxis(float alpha_ofUnrotatedGimbalAxes, Vector3 posWhereToDraw, Vector3 xAxisUnrotated_startPos_inGlobalSpace, Vector3 xAxisUnrotated_endPos_inGlobalSpace, Vector3 zAxisUnrotated_startPos_inGlobalSpace, Vector3 zAxisUnrotated_endPos_inGlobalSpace, Vector3 xAxis_rotated_inGlobalSpace_normalized, Vector3 yAxis_rotated_inGlobalSpace_normalized, Vector3 xAxisUnrotatedShortened_endPos_inGlobalSpace, Vector3 zAxisUnrotatedShortened_endPos_inGlobalSpace, Vector3 zAxisRotatedOnlyAroundYShortened_endPos_inGlobalSpace, float eulerAnglesToDraw_x, float eulerAnglesToDraw_y, float lineWidth_ofAxes, float absConeLength, float gimbalSize, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(alpha_ofUnrotatedGimbalAxes) == false)
            {
                float angleDeg_forDrawnCircledArrowOfXAxis = Loop360degProtrudingAngle_toDrawableNon360_isIntoSpan_fromExclM360_toExclP360(eulerAnglesToDraw_x);
                float angleDeg_forDrawnCircledArrowOfYAxis = Loop360degProtrudingAngle_toDrawableNon360_isIntoSpan_fromExclM360_toExclP360(eulerAnglesToDraw_y);
                bool loopedXTurnAngleIsApproxZero = CheckIfLoopedTurnAngleIsApproxZero(angleDeg_forDrawnCircledArrowOfXAxis);
                bool loopedYTurnAngleIsApproxZero = CheckIfLoopedTurnAngleIsApproxZero(angleDeg_forDrawnCircledArrowOfYAxis);
                bool xAxisIsUnturned = loopedYTurnAngleIsApproxZero;
                bool zAxisIsUnturned = loopedXTurnAngleIsApproxZero && loopedYTurnAngleIsApproxZero;

                if (zAxisIsUnturned == false)
                {
                    Color color_ofUnturnedZAxis = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(UtilitiesDXXL_Colors.blue_zAxisAlpha1, alpha_ofUnrotatedGimbalAxes);
                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                    DrawBasics.Vector(zAxisUnrotated_startPos_inGlobalSpace, zAxisUnrotated_endPos_inGlobalSpace, color_ofUnturnedZAxis, lineWidth_ofAxes, null, absConeLength, false, false, default(Vector3), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                }

                if (xAxisIsUnturned == false)
                {
                    Color color_ofUnturnedXAxis = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(UtilitiesDXXL_Colors.red_xAxisAlpha1, alpha_ofUnrotatedGimbalAxes);
                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                    DrawBasics.Vector(xAxisUnrotated_startPos_inGlobalSpace, xAxisUnrotated_endPos_inGlobalSpace, color_ofUnturnedXAxis, lineWidth_ofAxes, null, absConeLength, false, false, default(Vector3), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                }

                float coneLength_relToCircledLineRadius = 0.25f;
                float lineWidth_ofCircledLine = 0.005f * gimbalSize;
                float customAlphaFactor_forCircledLinePointers = 2.5f;

                if (zAxisIsUnturned == false)
                {
                    Color color_ofCircledVectorForZAxis = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(UtilitiesDXXL_Colors.blue_zAxisAlpha1, 0.5f * alpha_ofUnrotatedGimbalAxes);
                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forCircledVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                    UtilitiesDXXL_LineCircled.VectorCircled(zAxisUnrotatedShortened_endPos_inGlobalSpace, posWhereToDraw, yAxis_rotated_inGlobalSpace_normalized, angleDeg_forDrawnCircledArrowOfYAxis, color_ofCircledVectorForZAxis, lineWidth_ofCircledLine, null, coneLength_relToCircledLineRadius, false, false, false, 45.0f, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, durationInSec, hiddenByNearerObjects, customAlphaFactor_forCircledLinePointers);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forCircledVectors();

                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forCircledVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                    UtilitiesDXXL_LineCircled.VectorCircled(zAxisRotatedOnlyAroundYShortened_endPos_inGlobalSpace, posWhereToDraw, xAxis_rotated_inGlobalSpace_normalized, angleDeg_forDrawnCircledArrowOfXAxis, color_ofCircledVectorForZAxis, lineWidth_ofCircledLine, null, coneLength_relToCircledLineRadius, false, false, false, 45.0f, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, durationInSec, hiddenByNearerObjects, customAlphaFactor_forCircledLinePointers);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forCircledVectors();
                }

                if (xAxisIsUnturned == false)
                {
                    Color color_ofCircledVectorForXAxis = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(UtilitiesDXXL_Colors.red_xAxisAlpha1, 0.5f * alpha_ofUnrotatedGimbalAxes);
                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forCircledVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                    UtilitiesDXXL_LineCircled.VectorCircled(xAxisUnrotatedShortened_endPos_inGlobalSpace, posWhereToDraw, yAxis_rotated_inGlobalSpace_normalized, angleDeg_forDrawnCircledArrowOfYAxis, color_ofCircledVectorForXAxis, lineWidth_ofCircledLine, null, coneLength_relToCircledLineRadius, false, false, false, 45.0f, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, durationInSec, hiddenByNearerObjects, customAlphaFactor_forCircledLinePointers);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forCircledVectors();
                }
            }
        }

        static void DrawFirstPassOfZAxis(bool zAxis_pointsAwayFromObserverCam, Vector3 zAxis_rotated_inGlobalSpace_normalized, Vector3 zAxis_startPos_inGlobalSpace, Vector3 zAxis_endPos_inGlobalSpace, Vector3 perpToZAxis_markingZAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, Vector3 perpToZAxis_markingZAxisEndTurnAngle_rotated_inGlobalSpace_normalized, float eulerAnglesToDraw_z, float lineWidth_ofAxes, float absConeLength, float gimbalSize, bool isLocal, float durationInSec, bool hiddenByNearerObjects)
        {
            UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
            DrawBasics.Vector(zAxis_startPos_inGlobalSpace, zAxis_endPos_inGlobalSpace, UtilitiesDXXL_Colors.blue_zAxisAlpha1, lineWidth_ofAxes, null, absConeLength, false, false, default(Vector3), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();

            if (zAxis_pointsAwayFromObserverCam == false)
            {
                DrawAngleVisualizerAtAxis("Z", zAxis_startPos_inGlobalSpace, zAxis_rotated_inGlobalSpace_normalized, perpToZAxis_markingZAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, perpToZAxis_markingZAxisEndTurnAngle_rotated_inGlobalSpace_normalized, UtilitiesDXXL_Colors.blue_zAxisAlpha1, 0.56f, Color.Lerp(UtilitiesDXXL_Colors.blue_zAxisAlpha1, Color.white, 0.2f), eulerAnglesToDraw_z, gimbalSize, isLocal, durationInSec, hiddenByNearerObjects);
            }
        }

        static void DrawFirstPassOfXAxis(bool xAxis_pointsAwayFromObserverCam, Vector3 xAxis_rotated_inGlobalSpace_normalized, Vector3 xAxis_startPos_inGlobalSpace, Vector3 xAxis_endPos_inGlobalSpace, Vector3 perpToXAxis_markingXAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, Vector3 perpToXAxis_markingXAxisEndTurnAngle_rotated_inGlobalSpace_normalized, float eulerAnglesToDraw_x, float lineWidth_ofAxes, float absConeLength, float gimbalSize, bool isLocal, float durationInSec, bool hiddenByNearerObjects)
        {
            UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
            DrawBasics.Vector(xAxis_startPos_inGlobalSpace, xAxis_endPos_inGlobalSpace, UtilitiesDXXL_Colors.red_xAxisAlpha1, lineWidth_ofAxes, null, absConeLength, false, false, default(Vector3), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();

            if (xAxis_pointsAwayFromObserverCam == false)
            {
                DrawAngleVisualizerAtAxis("X", xAxis_startPos_inGlobalSpace, xAxis_rotated_inGlobalSpace_normalized, perpToXAxis_markingXAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, perpToXAxis_markingXAxisEndTurnAngle_rotated_inGlobalSpace_normalized, UtilitiesDXXL_Colors.red_xAxisAlpha1, 0.47f, UtilitiesDXXL_Colors.GetSimilarColorWithAdjustableOtherBrightnessValue(UtilitiesDXXL_Colors.red_xAxisAlpha1, 0.2f), eulerAnglesToDraw_x, gimbalSize, isLocal, durationInSec, hiddenByNearerObjects);
            }
        }

        static void DrawAwayFacingPartofYAxis(bool yAxis_pointsAwayFromObserverCam, Vector3 posWhereToDraw, Vector3 yAxis_rotated_inGlobalSpace_normalized, Vector3 yAxis_startPos_inGlobalSpace, Vector3 yAxis_endPos_inGlobalSpace, Vector3 normalOfYAxisHolderRing, Vector3 perpToYAxis_markingYAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, Vector3 perpToYAxis_markingYAxisEndTurnAngle_rotated_inGlobalSpace_normalized, float angleDeg_ofHalfYAxisHolderRingSegment, float radius_ofYAxisHolderRing, float eulerAnglesToDraw_y, float lineWidth_ofAxes, float absConeLength, float gimbalSize, bool isLocal, float durationInSec, bool hiddenByNearerObjects)
        {
            if (yAxis_pointsAwayFromObserverCam)
            {
                DrawYAxis_halfSegmentThatContainsTheArrowPeak(false, posWhereToDraw, yAxis_rotated_inGlobalSpace_normalized, yAxis_endPos_inGlobalSpace, normalOfYAxisHolderRing, angleDeg_ofHalfYAxisHolderRingSegment, radius_ofYAxisHolderRing, lineWidth_ofAxes, absConeLength, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                DrawYAxis_halfSegmentThatDoesNotContainTheArrowPeak(false, posWhereToDraw, yAxis_rotated_inGlobalSpace_normalized, yAxis_startPos_inGlobalSpace, normalOfYAxisHolderRing, angleDeg_ofHalfYAxisHolderRingSegment, radius_ofYAxisHolderRing, lineWidth_ofAxes, durationInSec, hiddenByNearerObjects);
                DrawAngleVisualizerAtAxis("Y", yAxis_startPos_inGlobalSpace, yAxis_rotated_inGlobalSpace_normalized, perpToYAxis_markingYAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, perpToYAxis_markingYAxisEndTurnAngle_rotated_inGlobalSpace_normalized, UtilitiesDXXL_Colors.green_yAxisAlpha1, 0.4f, UtilitiesDXXL_Colors.GetSimilarColorWithAdjustableOtherBrightnessValue(UtilitiesDXXL_Colors.green_yAxisAlpha1, 0.2f), eulerAnglesToDraw_y, gimbalSize, isLocal, durationInSec, hiddenByNearerObjects);
            }
        }

        static void DrawAwayPointingPartOfYAxis_asGimbalLockHybrid(bool yAxis_pointsAwayFromObserverCam, Vector3 posWhereToDraw, Vector3 yAxis_startPos_inGlobalSpace, Vector3 yAxis_rotated_inGlobalSpace_normalized, Vector3 fromDrawCenterPos_toYAxisPeak_inGlobalSpace, float radius_ofYAxisHolderRing, float lineWidth_ofAxes, float gimbalSize, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 yAxisStartPos_onHolderRing;
            Vector3 endPos;

            if (yAxis_pointsAwayFromObserverCam)
            {
                yAxisStartPos_onHolderRing = posWhereToDraw + yAxis_rotated_inGlobalSpace_normalized * radius_ofYAxisHolderRing;
                endPos = posWhereToDraw + fromDrawCenterPos_toYAxisPeak_inGlobalSpace * 0.9f;
            }
            else
            {
                yAxisStartPos_onHolderRing = posWhereToDraw - yAxis_rotated_inGlobalSpace_normalized * radius_ofYAxisHolderRing;
                endPos = yAxis_startPos_inGlobalSpace;
            }

            float lengthOfStripes = stripeSizeFactor_forGimbelLocksAlternatingColorLine * gimbalSize;
            LineWithAlternatingColors_fadeableAnimSpeed.InternalDraw(yAxisStartPos_onHolderRing, endPos, UtilitiesDXXL_Colors.green_yAxisAlpha1, UtilitiesDXXL_Colors.blue_zAxisAlpha1, lineWidth_ofAxes, lengthOfStripes, null, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, true, true);
        }

        static void DrawCamPointingSecondPassOfXAxis(bool xAxis_pointsAwayFromObserverCam, Vector3 posWhereToDraw, Vector3 xAxis_rotated_inGlobalSpace_normalized, Vector3 xAxis_startPos_inGlobalSpace, Vector3 xAxis_endPos_inGlobalSpace, Vector3 perpToXAxis_markingXAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, Vector3 perpToXAxis_markingXAxisEndTurnAngle_rotated_inGlobalSpace_normalized, Color color_ofRedXFlange, Color color_ofGreenYFlange, float eulerAnglesToDraw_x, float lineWidth_ofAxes, float radius_ofSpheres, float distance_centerToFarFlanges, float absConeLength, float gimbalSize, bool isLocal, float durationInSec, bool hiddenByNearerObjects)
        {
            if (xAxis_pointsAwayFromObserverCam)
            {
                Vector3 xAxisEntryPointIntoRedSphere = posWhereToDraw - xAxis_rotated_inGlobalSpace_normalized * distance_centerToFarFlanges;
                DrawDoubleFlange_openHole(xAxisEntryPointIntoRedSphere, xAxis_rotated_inGlobalSpace_normalized, radius_ofSpheres, color_ofRedXFlange, color_ofGreenYFlange, durationInSec, hiddenByNearerObjects);
                Line_fadeableAnimSpeed.InternalDraw(xAxisEntryPointIntoRedSphere, xAxis_startPos_inGlobalSpace, UtilitiesDXXL_Colors.red_xAxisAlpha1, lineWidth_ofAxes, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

                DrawAngleVisualizerAtAxis("X", xAxis_startPos_inGlobalSpace, xAxis_rotated_inGlobalSpace_normalized, perpToXAxis_markingXAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, perpToXAxis_markingXAxisEndTurnAngle_rotated_inGlobalSpace_normalized, UtilitiesDXXL_Colors.red_xAxisAlpha1, 0.47f, UtilitiesDXXL_Colors.GetSimilarColorWithAdjustableOtherBrightnessValue(UtilitiesDXXL_Colors.red_xAxisAlpha1, 0.2f), eulerAnglesToDraw_x, gimbalSize, isLocal, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                Vector3 xAxisEntryPointIntoRedSphere = posWhereToDraw + xAxis_rotated_inGlobalSpace_normalized * distance_centerToFarFlanges;
                DrawDoubleFlange_openHole(xAxisEntryPointIntoRedSphere, xAxis_rotated_inGlobalSpace_normalized, radius_ofSpheres, color_ofRedXFlange, color_ofGreenYFlange, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                DrawBasics.Vector(xAxisEntryPointIntoRedSphere, xAxis_endPos_inGlobalSpace, UtilitiesDXXL_Colors.red_xAxisAlpha1, lineWidth_ofAxes, null, absConeLength, false, false, default(Vector3), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();

            }
        }

        static void DrawCamPointingSecondPassOfZAxis(bool zAxis_pointsAwayFromObserverCam, Vector3 posWhereToDraw, Vector3 zAxis_rotated_inGlobalSpace_normalized, Vector3 zAxis_startPos_inGlobalSpace, Vector3 zAxis_endPos_inGlobalSpace, Vector3 perpToZAxis_markingZAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, Vector3 perpToZAxis_markingZAxisEndTurnAngle_rotated_inGlobalSpace_normalized, float radius_ofSpheres, float diameter_ofSpheres, float eulerAnglesToDraw_z, float lineWidth_ofAxes, float absConeLength, float gimbalSize, Color color_ofRedXFlange, Color color_ofBlueZFlange, bool isLocal, float durationInSec, bool hiddenByNearerObjects)
        {
            float distance_centerToNearFlanges = 0.4f * diameter_ofSpheres;
            if (zAxis_pointsAwayFromObserverCam)
            {
                Vector3 zAxisEntryPointIntoRedSphere = posWhereToDraw - zAxis_rotated_inGlobalSpace_normalized * distance_centerToNearFlanges;
                DrawDoubleFlange_openHole(zAxisEntryPointIntoRedSphere, zAxis_rotated_inGlobalSpace_normalized, radius_ofSpheres, color_ofBlueZFlange, color_ofRedXFlange, durationInSec, hiddenByNearerObjects);
                Line_fadeableAnimSpeed.InternalDraw(zAxisEntryPointIntoRedSphere, zAxis_startPos_inGlobalSpace, UtilitiesDXXL_Colors.blue_zAxisAlpha1, lineWidth_ofAxes, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

                DrawAngleVisualizerAtAxis("Z", zAxis_startPos_inGlobalSpace, zAxis_rotated_inGlobalSpace_normalized, perpToZAxis_markingZAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, perpToZAxis_markingZAxisEndTurnAngle_rotated_inGlobalSpace_normalized, UtilitiesDXXL_Colors.blue_zAxisAlpha1, 0.56f, Color.Lerp(UtilitiesDXXL_Colors.blue_zAxisAlpha1, Color.white, 0.2f), eulerAnglesToDraw_z, gimbalSize, isLocal, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                Vector3 zAxisEntryPointIntoRedSphere = posWhereToDraw + zAxis_rotated_inGlobalSpace_normalized * distance_centerToNearFlanges;
                DrawDoubleFlange_openHole(zAxisEntryPointIntoRedSphere, zAxis_rotated_inGlobalSpace_normalized, radius_ofSpheres, color_ofBlueZFlange, color_ofRedXFlange, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                DrawBasics.Vector(zAxisEntryPointIntoRedSphere, zAxis_endPos_inGlobalSpace, UtilitiesDXXL_Colors.blue_zAxisAlpha1, lineWidth_ofAxes, null, absConeLength, false, false, default(Vector3), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
            }
        }

        static void DrawCamPointingSecondPassOfYAxis(bool yAxis_pointsAwayFromObserverCam, Vector3 posWhereToDraw, Vector3 yAxis_rotated_inGlobalSpace_normalized, Vector3 yAxis_startPos_inGlobalSpace, Vector3 yAxis_endPos_inGlobalSpace, Vector3 normalOfYAxisHolderRing, Vector3 perpToYAxis_markingYAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, Vector3 perpToYAxis_markingYAxisEndTurnAngle_rotated_inGlobalSpace_normalized, float angleDeg_ofHalfYAxisHolderRingSegment, float radius_ofYAxisHolderRing, float eulerAnglesToDraw_y, float lineWidth_ofAxes, float absConeLength, float gimbalSize, bool isLocal, float durationInSec, bool hiddenByNearerObjects)
        {
            if (yAxis_pointsAwayFromObserverCam)
            {
                DrawYAxis_halfSegmentThatDoesNotContainTheArrowPeak(true, posWhereToDraw, yAxis_rotated_inGlobalSpace_normalized, yAxis_startPos_inGlobalSpace, normalOfYAxisHolderRing, angleDeg_ofHalfYAxisHolderRingSegment, radius_ofYAxisHolderRing, lineWidth_ofAxes, durationInSec, hiddenByNearerObjects);
                DrawAngleVisualizerAtAxis("Y", yAxis_startPos_inGlobalSpace, yAxis_rotated_inGlobalSpace_normalized, perpToYAxis_markingYAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, perpToYAxis_markingYAxisEndTurnAngle_rotated_inGlobalSpace_normalized, UtilitiesDXXL_Colors.green_yAxisAlpha1, 0.4f, UtilitiesDXXL_Colors.GetSimilarColorWithAdjustableOtherBrightnessValue(UtilitiesDXXL_Colors.green_yAxisAlpha1, 0.2f), eulerAnglesToDraw_y, gimbalSize, isLocal, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                DrawYAxis_halfSegmentThatContainsTheArrowPeak(true, posWhereToDraw, yAxis_rotated_inGlobalSpace_normalized, yAxis_endPos_inGlobalSpace, normalOfYAxisHolderRing, angleDeg_ofHalfYAxisHolderRingSegment, radius_ofYAxisHolderRing, lineWidth_ofAxes, absConeLength, durationInSec, hiddenByNearerObjects);
            }
        }

        static void DrawCamPointingPartOfYAxis_asGimbalLockHybrid(bool yAxis_pointsAwayFromObserverCam, Vector3 posWhereToDraw, Vector3 yAxis_startPos_inGlobalSpace, Vector3 yAxis_rotated_inGlobalSpace_normalized, Vector3 fromDrawCenterPos_toYAxisPeak_inGlobalSpace, float radius_ofYAxisHolderRing, float lineWidth_ofAxes, float gimbalSize, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 yAxisStartPos_onHolderRing;
            Vector3 endPos;

            if (yAxis_pointsAwayFromObserverCam)
            {
                yAxisStartPos_onHolderRing = posWhereToDraw - yAxis_rotated_inGlobalSpace_normalized * radius_ofYAxisHolderRing;
                endPos = yAxis_startPos_inGlobalSpace;
            }
            else
            {
                yAxisStartPos_onHolderRing = posWhereToDraw + yAxis_rotated_inGlobalSpace_normalized * radius_ofYAxisHolderRing;
                endPos = posWhereToDraw + fromDrawCenterPos_toYAxisPeak_inGlobalSpace * 0.9f;
            }

            float lengthOfStripes = stripeSizeFactor_forGimbelLocksAlternatingColorLine * gimbalSize;
            LineWithAlternatingColors_fadeableAnimSpeed.InternalDraw(yAxisStartPos_onHolderRing, endPos, UtilitiesDXXL_Colors.green_yAxisAlpha1, UtilitiesDXXL_Colors.blue_zAxisAlpha1, lineWidth_ofAxes, lengthOfStripes, null, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, true, true);
        }

        static void DrawYAxis_halfSegmentThatContainsTheArrowPeak(bool containsFlanges, Vector3 posWhereToDraw, Vector3 yAxis_rotated_inGlobalSpace_normalized, Vector3 yAxis_endPos_inGlobalSpace, Vector3 normalOfYAxisHolderRing, float angleDeg_ofHalfYAxisHolderRingSegment, float radius_ofYAxisHolderRing, float lineWidth_ofAxes, float absConeLength, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 yAxisStartPos_onHolderRing = posWhereToDraw + yAxis_rotated_inGlobalSpace_normalized * radius_ofYAxisHolderRing;
            if (containsFlanges)
            {
                DrawFlanges_atYAxisHolderCircle(posWhereToDraw, yAxisStartPos_onHolderRing, normalOfYAxisHolderRing, lineWidth_ofAxes, angleDeg_ofHalfYAxisHolderRingSegment, durationInSec, hiddenByNearerObjects);
            }
            UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
            DrawBasics.Vector(yAxisStartPos_onHolderRing, yAxis_endPos_inGlobalSpace, UtilitiesDXXL_Colors.green_yAxisAlpha1, lineWidth_ofAxes, null, absConeLength, false, false, default(Vector3), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();

            DrawBasics.LineCircled(yAxisStartPos_onHolderRing, posWhereToDraw, normalOfYAxisHolderRing, angleDeg_ofHalfYAxisHolderRingSegment, UtilitiesDXXL_Colors.green_yAxisAlpha1, lineWidth_ofAxes, null, false, false, 45.0f, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, durationInSec, hiddenByNearerObjects);
            DrawBasics.LineCircled(yAxisStartPos_onHolderRing, posWhereToDraw, normalOfYAxisHolderRing, -angleDeg_ofHalfYAxisHolderRingSegment, UtilitiesDXXL_Colors.green_yAxisAlpha1, lineWidth_ofAxes, null, false, false, 45.0f, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, durationInSec, hiddenByNearerObjects);
        }

        static void DrawYAxis_halfSegmentThatDoesNotContainTheArrowPeak(bool containsFlanges, Vector3 posWhereToDraw, Vector3 yAxis_rotated_inGlobalSpace_normalized, Vector3 yAxis_startPos_inGlobalSpace, Vector3 normalOfYAxisHolderRing, float angleDeg_ofHalfYAxisHolderRingSegment, float radius_ofYAxisHolderRing, float lineWidth_ofAxes, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 yAxisStartPos_onHolderRing = posWhereToDraw - yAxis_rotated_inGlobalSpace_normalized * radius_ofYAxisHolderRing;
            if (containsFlanges)
            {
                DrawFlanges_atYAxisHolderCircle(posWhereToDraw, yAxisStartPos_onHolderRing, normalOfYAxisHolderRing, lineWidth_ofAxes, angleDeg_ofHalfYAxisHolderRingSegment, durationInSec, hiddenByNearerObjects);
            }
            Line_fadeableAnimSpeed.InternalDraw(yAxisStartPos_onHolderRing, yAxis_startPos_inGlobalSpace, UtilitiesDXXL_Colors.green_yAxisAlpha1, lineWidth_ofAxes, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            DrawBasics.LineCircled(yAxisStartPos_onHolderRing, posWhereToDraw, normalOfYAxisHolderRing, angleDeg_ofHalfYAxisHolderRingSegment, UtilitiesDXXL_Colors.green_yAxisAlpha1, lineWidth_ofAxes, null, false, false, 45.0f, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, durationInSec, hiddenByNearerObjects);
            DrawBasics.LineCircled(yAxisStartPos_onHolderRing, posWhereToDraw, normalOfYAxisHolderRing, -angleDeg_ofHalfYAxisHolderRingSegment, UtilitiesDXXL_Colors.green_yAxisAlpha1, lineWidth_ofAxes, null, false, false, 45.0f, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, durationInSec, hiddenByNearerObjects);
        }

        static void DrawFlanges_atYAxisHolderCircle(Vector3 posWhereToDraw, Vector3 yAxisStartPos_onHolderRing, Vector3 normalOfYAxisHolderRing, float lineWidth_ofAxes, float angleDeg_ofHalfYAxisHolderRingSegment, float durationInSec, bool hiddenByNearerObjects)
        {
            Quaternion rotation_fromTriMountPoint_toFlange1 = Quaternion.AngleAxis(angleDeg_ofHalfYAxisHolderRingSegment, normalOfYAxisHolderRing);
            Quaternion rotation_fromTriMountPoint_toFlange2 = Quaternion.Inverse(rotation_fromTriMountPoint_toFlange1);

            Vector3 fromCenterPos_toTriMountPoint = yAxisStartPos_onHolderRing - posWhereToDraw;
            Vector3 fromCenterPos_toFlange1 = rotation_fromTriMountPoint_toFlange1 * fromCenterPos_toTriMountPoint;
            Vector3 fromCenterPos_toFlange2 = rotation_fromTriMountPoint_toFlange2 * fromCenterPos_toTriMountPoint;

            Vector3 centerPos_ofFlange1 = posWhereToDraw + fromCenterPos_toFlange1;
            Vector3 centerPos_ofFlange2 = posWhereToDraw + fromCenterPos_toFlange2;

            Quaternion rotation_aroundHolderCircle_by90deg = Quaternion.AngleAxis(90.0f, normalOfYAxisHolderRing);
            Vector3 normal_ofFlange1 = rotation_aroundHolderCircle_by90deg * fromCenterPos_toFlange1;
            Vector3 normal_ofFlange2 = rotation_aroundHolderCircle_by90deg * fromCenterPos_toFlange2;

            float radius = (0.5f * lineWidth_ofAxes) * 1.2f;
            Color color = Color.Lerp(UtilitiesDXXL_Colors.green_yAxisAlpha1, Color.black, 0.6f);

            DrawShapes.Decagon(centerPos_ofFlange1, radius, color, normal_ofFlange1, default(Vector3), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
            DrawShapes.Decagon(centerPos_ofFlange2, radius, color, normal_ofFlange2, default(Vector3), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
        }

        static void DrawSingleFlange(Vector3 position, Vector3 innerAxis_rotated_inGlobalSpace_normalized, float radius_ofSpheres, Color color, float durationInSec, bool hiddenByNearerObjects)
        {
            float radius_ofOuterFlange = radius_ofSpheres * 0.3f;
            float lineWidth_ofOuterFlange = radius_ofSpheres * 0.55f;
            DrawShapes.Decagon(position, radius_ofOuterFlange, color, innerAxis_rotated_inGlobalSpace_normalized, default(Vector3), lineWidth_ofOuterFlange, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
        }

        static void DrawDoubleFlange_openHole(Vector3 position, Vector3 innerAxis_rotated_inGlobalSpace_normalized, float radius_ofSpheres, Color color_ofInnerFlange, Color color_ofOuterFlange, float durationInSec, bool hiddenByNearerObjects)
        {
            float radius_ofOuterFlange = radius_ofSpheres * 0.55f;
            float lineWidth_ofOuterFlange = radius_ofSpheres * 0.2f;
            DrawShapes.Decagon(position, radius_ofOuterFlange, color_ofOuterFlange, innerAxis_rotated_inGlobalSpace_normalized, default(Vector3), lineWidth_ofOuterFlange, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);

            float radius_ofInnerFlange = radius_ofSpheres * 0.35f;
            float lineWidth_ofInnerFlange = radius_ofSpheres * 0.2f;
            DrawShapes.Decagon(position, radius_ofInnerFlange, color_ofInnerFlange, innerAxis_rotated_inGlobalSpace_normalized, default(Vector3), lineWidth_ofInnerFlange, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
        }

        static void DrawDoubleFlange_closedHole(Vector3 position, Vector3 innerAxis_rotated_inGlobalSpace_normalized, float radius_ofSpheres, Color color_ofInnerFlange, Color color_ofOuterFlange, float durationInSec, bool hiddenByNearerObjects)
        {
            float radius_ofOuterFlange = radius_ofSpheres * 0.55f;
            float lineWidth_ofOuterFlange = radius_ofSpheres * 0.2f;
            DrawShapes.Decagon(position, radius_ofOuterFlange, color_ofOuterFlange, innerAxis_rotated_inGlobalSpace_normalized, default(Vector3), lineWidth_ofOuterFlange, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);

            float radius_ofInnerFlange = radius_ofSpheres * 0.25f;
            float lineWidth_ofInnerFlange = radius_ofSpheres * 0.4f;
            DrawShapes.Decagon(position, radius_ofInnerFlange, color_ofInnerFlange, innerAxis_rotated_inGlobalSpace_normalized, default(Vector3), lineWidth_ofInnerFlange, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
        }

        static void DrawAngleVisualizerAtAxis(string axisName, Vector3 axis_startPos_inGlobalSpace, Vector3 axis_rotated_inGlobalSpace_normalized, Vector3 perpToAxis_markingAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, Vector3 perpToAxis_markingAxisEndTurnAngle_rotated_inGlobalSpace_normalized, Color color_ofAxis, float alphaFactor_forColorOfThinFullCircle, Color color_ofAngleText, float eulerAngles_withWhichAxisIsTurned, float gimbalSize, bool isLocal, float durationInSec, bool hiddenByNearerObjects)
        {
            float radius_ofTurnAngleAtAxes_visualizer = relRadius_ofAxisTurnAngleVisualizer * gimbalSize;
            float radiusOfText_ofTurnAngleAtAxes_visualizer = radius_ofTurnAngleAtAxes_visualizer * 1.1f;
            float size_ofTurnAngleVisualizerText = 0.28f * radius_ofTurnAngleAtAxes_visualizer;
            float lineWidth_ofTurnAngleCircledArrow = eulerAxesTurnAngleVisualizer_relLineWidth * gimbalSize;

            Vector3 startPosOnTurnAngleVisualizerCircle = axis_startPos_inGlobalSpace + perpToAxis_markingAxisZeroTurnAngle_rotated_inGlobalSpace_normalized * radius_ofTurnAngleAtAxes_visualizer;
            Vector3 endPosOnTurnAngleVisualizerCircle = axis_startPos_inGlobalSpace + perpToAxis_markingAxisEndTurnAngle_rotated_inGlobalSpace_normalized * radius_ofTurnAngleAtAxes_visualizer;

            Line_fadeableAnimSpeed.InternalDraw(axis_startPos_inGlobalSpace, startPosOnTurnAngleVisualizerCircle, color_ofAxis, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(axis_startPos_inGlobalSpace, endPosOnTurnAngleVisualizerCircle, color_ofAxis, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            Color color_ofThinFullCircle = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofAxis, alphaFactor_forColorOfThinFullCircle);
            DrawShapes.Circle(axis_startPos_inGlobalSpace, radius_ofTurnAngleAtAxes_visualizer, color_ofThinFullCircle, axis_rotated_inGlobalSpace_normalized, default(Vector3), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);

            bool absAngle_isBiggerOrEqualThan360deg = Mathf.Abs(eulerAngles_withWhichAxisIsTurned) >= 360.0f;
            if (absAngle_isBiggerOrEqualThan360deg)
            {
                DrawShapes.Circle(axis_startPos_inGlobalSpace, radius_ofTurnAngleAtAxes_visualizer, color_ofAxis, axis_rotated_inGlobalSpace_normalized, default(Vector3), lineWidth_ofTurnAngleCircledArrow, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
            }

            float angleDeg_forDrawnCircledArrow = Loop360degProtrudingAngle_toDrawableNon360_isIntoSpan_fromExclM360_toExclP360(eulerAngles_withWhichAxisIsTurned);
            if (UtilitiesDXXL_Math.ApproximatelyZero(angleDeg_forDrawnCircledArrow))
            {
                //-> skip drawing circledArrow
            }
            else
            {
                DrawCircledArrow_atAxisTurnVisualizer(angleDeg_forDrawnCircledArrow, axis_startPos_inGlobalSpace, axis_rotated_inGlobalSpace_normalized, startPosOnTurnAngleVisualizerCircle, color_ofAxis, lineWidth_ofTurnAngleCircledArrow, durationInSec, hiddenByNearerObjects);
            }

            Vector3 startPosOfTextOnTurnAngleVisualizerCircle = axis_startPos_inGlobalSpace + perpToAxis_markingAxisZeroTurnAngle_rotated_inGlobalSpace_normalized * radiusOfText_ofTurnAngleAtAxes_visualizer;
            Vector3 turnAxis_ofText = (eulerAngles_withWhichAxisIsTurned < 0.0f) ? axis_rotated_inGlobalSpace_normalized : (-axis_rotated_inGlobalSpace_normalized); //-> prevent text from starting to away from circledArrow
            UtilitiesDXXL_Text.WriteOnCircle("" + eulerAngles_withWhichAxisIsTurned + "°", startPosOfTextOnTurnAngleVisualizerCircle, axis_startPos_inGlobalSpace, turnAxis_ofText, color_ofAngleText, size_ofTurnAngleVisualizerText, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, 0.0f, true, durationInSec, hiddenByNearerObjects);

            Vector3 textDir_ofAxisIdentifierName_normalized = Vector3.Cross(perpToAxis_markingAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, axis_rotated_inGlobalSpace_normalized);
            Vector3 pos_ofAxisIdentifierName = axis_startPos_inGlobalSpace + perpToAxis_markingAxisZeroTurnAngle_rotated_inGlobalSpace_normalized * radius_ofTurnAngleAtAxes_visualizer * 0.075f; //-> slightly shifted, so it ends up with homogenuous padding to upper and lower side.

            UtilitiesDXXL_Text.Write("<sw=80000>" + axisName + "</sw><br><size=2> axis</size>", pos_ofAxisIdentifierName, color_ofAngleText, 1.9f * size_ofTurnAngleVisualizerText, textDir_ofAxisIdentifierName_normalized, perpToAxis_markingAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, DrawText.TextAnchorDXXL.UpperCenter, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
            if (isLocal)
            {
                Vector3 pos_ofLocalPrefix = axis_startPos_inGlobalSpace - perpToAxis_markingAxisZeroTurnAngle_rotated_inGlobalSpace_normalized * radius_ofTurnAngleAtAxes_visualizer * 0.22f;
                UtilitiesDXXL_Text.Write("   local   ", pos_ofLocalPrefix, color_ofAngleText, 0.35f * size_ofTurnAngleVisualizerText, textDir_ofAxisIdentifierName_normalized, perpToAxis_markingAxisZeroTurnAngle_rotated_inGlobalSpace_normalized, DrawText.TextAnchorDXXL.UpperRight, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
            }
        }

        static void DrawCircledArrow_atAxisTurnVisualizer(float eulerAnglesSpanOfDrawnCircledArrow, Vector3 axis_startPos_inGlobalSpace, Vector3 axis_rotated_inGlobalSpace_normalized, Vector3 startPosOnTurnAngleVisualizerCircle, Color color_ofAxis, float lineWidth_ofTurnAngleCircledArrow, float durationInSec, bool hiddenByNearerObjects)
        {
            float coneLength_relToRadius = 0.4f;

            UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forCircledVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
            DrawBasics.VectorCircled(startPosOnTurnAngleVisualizerCircle, axis_startPos_inGlobalSpace, axis_rotated_inGlobalSpace_normalized, eulerAnglesSpanOfDrawnCircledArrow, color_ofAxis, lineWidth_ofTurnAngleCircledArrow, null, coneLength_relToRadius, false, false, true, 45.0f, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forCircledVectors();
        }

        static float Loop360degProtrudingAngle_toDrawableNon360_isIntoSpan_fromExclM360_toExclP360(float angleDeg_toLoop)
        {
            if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(angleDeg_toLoop, 360.0f))
            {
                return 359.9f;
            }
            else
            {
                if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(angleDeg_toLoop, -360.0f))
                {
                    return (-359.9f);
                }
                else
                {
                    if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(angleDeg_toLoop, 720.0f))
                    {
                        return 359.9f;
                    }
                    else
                    {
                        if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(angleDeg_toLoop, -720.0f))
                        {
                            return (-359.9f);
                        }
                        else
                        {
                            if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(angleDeg_toLoop, 1080.0f))
                            {
                                return 359.9f;
                            }
                            else
                            {
                                if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(angleDeg_toLoop, -1080.0f))
                                {
                                    return (-359.9f);
                                }
                                else
                                {
                                    float angleDeg_loopedToBetween_m360_and_p360 = UtilitiesDXXL_Math.Loop_floatIntoSpanFrom_mX_to_pX(angleDeg_toLoop, 360.0f);
                                    float abs_angleDeg_loopedToBetween_m360_and_p360 = Mathf.Abs(angleDeg_loopedToBetween_m360_and_p360);
                                    if (abs_angleDeg_loopedToBetween_m360_and_p360 > 0.1f)
                                    {
                                        return angleDeg_toLoop;
                                    }
                                    else
                                    {
                                        return 0.0f;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        static bool CheckIfLoopedTurnAngleIsApproxZero(float loopedTurnAngleDeg)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(loopedTurnAngleDeg))
            {
                return true;
            }
            else
            {
                if (loopedTurnAngleDeg >= 359.9f)
                {
                    return true;
                }
                else
                {
                    if (loopedTurnAngleDeg <= (-359.9f))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static float DrawTurnedVectors(Vector3 customVectorToRotate_local, float length_ofUpAndForwardVectors_local, Transform parentTransform, float alpha_ofSquareSpannedByForwardAndUp, Vector3 posWhereToDraw, Vector3 eulerAnglesToDraw, Quaternion rotation_ofLocalSpace, Vector3 yAxis_unrotated_inLocalSpace_normalized, Vector3 xAxis_rotated_inLocalSpace_normalized, Vector3 zAxis_rotated_inLocalSpace_normalized, Vector3 xAxis_rotated_inGlobalSpace_normalized, Vector3 yAxis_rotated_inGlobalSpace_normalized, Vector3 zAxis_rotated_inGlobalSpace_normalized, float gimbalSize, bool isLocal, float durationInSec, bool hiddenByNearerObjects)
        {
            float maxAbsDistance_ofAnyPlumbPos_toTurnCenter = 0.0f;

            Vector3 customVectorToTurn_unturned_inLocalSpaceUnits_butAlreadyScaledSoLengthFitsGlobalUnits = (parentTransform == null) ? customVectorToRotate_local : Vector3.Scale(parentTransform.lossyScale, customVectorToRotate_local);
            float length_ofUpAndForwardVectors_global = (parentTransform == null) ? length_ofUpAndForwardVectors_local : (parentTransform.lossyScale.x * length_ofUpAndForwardVectors_local);

            bool drawCustomVector = CustomVectorIsDrawn(customVectorToTurn_unturned_inLocalSpaceUnits_butAlreadyScaledSoLengthFitsGlobalUnits);
            bool drawForwardAndUpVectors = ForwardAndUpVectorsAreDrawn(length_ofUpAndForwardVectors_global);
            if (drawCustomVector || drawForwardAndUpVectors)
            {
                Quaternion eulerAnglesToDraw_asQuaternion = Quaternion.Euler(eulerAnglesToDraw.x, eulerAnglesToDraw.y, eulerAnglesToDraw.z);
                eulerAnglesToDraw_asQuaternion.ToAngleAxis(out float shortestTurnAngleDeg, out Vector3 quaternionTurnAxis);
                if (shortestTurnAngleDeg > 180.0f)
                {
                    //-> "ToAngleAxis()" return an angle between 0 and 360 (probably because it internally uses 'acos'), though the common communication on quaternions is that they can represent a span "from -180 to +180".
                    shortestTurnAngleDeg = shortestTurnAngleDeg - 360.0f;
                }
                float abs_shortestTurnAngleDeg = Mathf.Abs(shortestTurnAngleDeg);
                bool rotationIsApproxIdentity = (abs_shortestTurnAngleDeg < 0.001f);

                Quaternion localEulerRotationAroundYAxis = Quaternion.AngleAxis(eulerAnglesToDraw.y, yAxis_unrotated_inLocalSpace_normalized);
                Quaternion localEulerRotationAroundRotatedXAxis = Quaternion.AngleAxis(eulerAnglesToDraw.x, xAxis_rotated_inLocalSpace_normalized);
                Quaternion localEulerRotationAroundRotatedZAxis = Quaternion.AngleAxis(eulerAnglesToDraw.z, zAxis_rotated_inLocalSpace_normalized);

                rotatedXAxis_line_inGlobalSpaceUnits.Recreate(posWhereToDraw, xAxis_rotated_inGlobalSpace_normalized, true);
                rotatedYAxis_line_inGlobalSpaceUnits.Recreate(posWhereToDraw, yAxis_rotated_inGlobalSpace_normalized, true);
                rotatedZAxis_line_inGlobalSpaceUnits.Recreate(posWhereToDraw, zAxis_rotated_inGlobalSpace_normalized, true);

                float maxAbsDistance_ofAnyPlumbPos_toTurnCenter_ofCurrTurnedVector;
                if (drawCustomVector)
                {
                    Color color_ofUnturnedCustomVector = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(0.105f, 0.1165f, 1.0f);
                    Color color_ofTurnedCustomVector = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(0.105f, 0.5f, 1.0f);
                    maxAbsDistance_ofAnyPlumbPos_toTurnCenter_ofCurrTurnedVector = DrawRotationOfCustomVector(customVectorToRotate_local, customVectorToTurn_unturned_inLocalSpaceUnits_butAlreadyScaledSoLengthFitsGlobalUnits, color_ofUnturnedCustomVector, color_ofTurnedCustomVector, "customVector", posWhereToDraw, eulerAnglesToDraw, rotation_ofLocalSpace, xAxis_rotated_inGlobalSpace_normalized, yAxis_rotated_inGlobalSpace_normalized, zAxis_rotated_inGlobalSpace_normalized, localEulerRotationAroundRotatedXAxis, localEulerRotationAroundYAxis, localEulerRotationAroundRotatedZAxis, gimbalSize, rotationIsApproxIdentity, isLocal, durationInSec, hiddenByNearerObjects);
                    maxAbsDistance_ofAnyPlumbPos_toTurnCenter = Mathf.Max(maxAbsDistance_ofAnyPlumbPos_toTurnCenter_ofCurrTurnedVector, maxAbsDistance_ofAnyPlumbPos_toTurnCenter);
                }

                if (drawForwardAndUpVectors)
                {
                    Color color_ofUnturnedForwardVector = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(0.6666f, 0.1625f, 1.0f);
                    Color color_ofTurnedForwardVector = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(0.6666f, 0.875f, 1.0f);

                    Color color_ofUnturnedUpVector = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(0.28f, 0.125f, 1.0f);
                    Color color_ofTurnedUpVector = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(0.27f, 0.85f, 1.0f);

                    Vector3 localForwardVector_unturned_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits = Vector3.forward * length_ofUpAndForwardVectors_local;
                    Vector3 localUpVector_unturned_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits = Vector3.up * length_ofUpAndForwardVectors_local;
                    Vector3 localForwardVector_unturned_inLocalSpaceUnits_scaledSoItFitsGlobalUnits = Vector3.forward * length_ofUpAndForwardVectors_global;
                    Vector3 localUpVector_unturned_inLocalSpaceUnits_scaledSoItFitsGlobalUnits = Vector3.up * length_ofUpAndForwardVectors_global;

                    if (UtilitiesDXXL_Math.ApproximatelyZero(alpha_ofSquareSpannedByForwardAndUp) == false)
                    {
                        DrawSquareSpannedByForwardAndUp(alpha_ofSquareSpannedByForwardAndUp, localForwardVector_unturned_inLocalSpaceUnits_scaledSoItFitsGlobalUnits, localUpVector_unturned_inLocalSpaceUnits_scaledSoItFitsGlobalUnits, posWhereToDraw, eulerAnglesToDraw_asQuaternion, rotation_ofLocalSpace, color_ofUnturnedForwardVector, color_ofTurnedForwardVector, color_ofUnturnedUpVector, color_ofTurnedUpVector, rotationIsApproxIdentity, durationInSec, hiddenByNearerObjects);
                    }
                    maxAbsDistance_ofAnyPlumbPos_toTurnCenter_ofCurrTurnedVector = DrawRotationOfCustomVector(localForwardVector_unturned_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits, localForwardVector_unturned_inLocalSpaceUnits_scaledSoItFitsGlobalUnits, color_ofUnturnedForwardVector, color_ofTurnedForwardVector, "Vector3.forward", posWhereToDraw, eulerAnglesToDraw, rotation_ofLocalSpace, xAxis_rotated_inGlobalSpace_normalized, yAxis_rotated_inGlobalSpace_normalized, zAxis_rotated_inGlobalSpace_normalized, localEulerRotationAroundRotatedXAxis, localEulerRotationAroundYAxis, localEulerRotationAroundRotatedZAxis, gimbalSize, rotationIsApproxIdentity, isLocal, durationInSec, hiddenByNearerObjects);
                    maxAbsDistance_ofAnyPlumbPos_toTurnCenter = Mathf.Max(maxAbsDistance_ofAnyPlumbPos_toTurnCenter_ofCurrTurnedVector, maxAbsDistance_ofAnyPlumbPos_toTurnCenter);
                    maxAbsDistance_ofAnyPlumbPos_toTurnCenter_ofCurrTurnedVector = DrawRotationOfCustomVector(localUpVector_unturned_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits, localUpVector_unturned_inLocalSpaceUnits_scaledSoItFitsGlobalUnits, color_ofUnturnedUpVector, color_ofTurnedUpVector, "Vector3.up", posWhereToDraw, eulerAnglesToDraw, rotation_ofLocalSpace, xAxis_rotated_inGlobalSpace_normalized, yAxis_rotated_inGlobalSpace_normalized, zAxis_rotated_inGlobalSpace_normalized, localEulerRotationAroundRotatedXAxis, localEulerRotationAroundYAxis, localEulerRotationAroundRotatedZAxis, gimbalSize, rotationIsApproxIdentity, isLocal, durationInSec, hiddenByNearerObjects);
                    maxAbsDistance_ofAnyPlumbPos_toTurnCenter = Mathf.Max(maxAbsDistance_ofAnyPlumbPos_toTurnCenter_ofCurrTurnedVector, maxAbsDistance_ofAnyPlumbPos_toTurnCenter);
                }
            }
            return maxAbsDistance_ofAnyPlumbPos_toTurnCenter;
        }

        static bool CustomVectorIsDrawn(Vector3 customVectorToRotate)
        {
            float biggestAbsComponent = UtilitiesDXXL_Math.GetBiggestAbsComponent(customVectorToRotate);
            return (biggestAbsComponent > approxLength_ofShortestTurnedVectorThatIsDrawn);
        }

        static bool ForwardAndUpVectorsAreDrawn(float length_ofUpAndForwardVectors)
        {
            float abs_length_ofUpAndForwardVectors = Mathf.Abs(length_ofUpAndForwardVectors);
            return (abs_length_ofUpAndForwardVectors > approxLength_ofShortestTurnedVectorThatIsDrawn);
        }

        static void DrawSquareSpannedByForwardAndUp(float alpha_ofSquareSpannedByForwardAndUp, Vector3 localForwardVector_unturned_inLocalSpaceUnits_scaledSoItFitsGlobalUnits, Vector3 localUpVector_unturned_inLocalSpaceUnits_scaledSoItFitsGlobalUnits, Vector3 posWhereToDraw, Quaternion eulerAnglesToDraw_asQuaternion, Quaternion rotation_ofLocalSpace, Color color_ofUnturnedForwardVector, Color color_ofTurnedForwardVector, Color color_ofUnturnedUpVector, Color color_ofTurnedUpVector, bool rotationIsApproxIdentity, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 localForwardVector_turned_inLocalSpaceUnits_scaledSoItFitsGlobalUnits = eulerAnglesToDraw_asQuaternion * localForwardVector_unturned_inLocalSpaceUnits_scaledSoItFitsGlobalUnits;
            Vector3 localUpVector_turned_inLocalSpaceUnits_scaledSoItFitsGlobalUnits = eulerAnglesToDraw_asQuaternion * localUpVector_unturned_inLocalSpaceUnits_scaledSoItFitsGlobalUnits;

            Vector3 localForwardVectorScaled_unturned_inGlobalSpaceUnits = rotation_ofLocalSpace * localForwardVector_unturned_inLocalSpaceUnits_scaledSoItFitsGlobalUnits;
            Vector3 localUpVectorScaled_unturned_inGlobalSpaceUnits = rotation_ofLocalSpace * localUpVector_unturned_inLocalSpaceUnits_scaledSoItFitsGlobalUnits;

            Vector3 localForwardVectorScaled_turned_inGlobalSpaceUnits = rotation_ofLocalSpace * localForwardVector_turned_inLocalSpaceUnits_scaledSoItFitsGlobalUnits;
            Vector3 localUpVectorScaled_turned_inGlobalSpaceUnits = rotation_ofLocalSpace * localUpVector_turned_inLocalSpaceUnits_scaledSoItFitsGlobalUnits;

            Color color_of90DegSymbolBeforeRotation = Color.Lerp(color_ofUnturnedForwardVector, color_ofUnturnedUpVector, 0.5f);
            UtilitiesDXXL_Quaternion.Draw90DegSymbolToQuaternionVectorPair(color_of90DegSymbolBeforeRotation, posWhereToDraw, localForwardVectorScaled_unturned_inGlobalSpaceUnits, localUpVectorScaled_unturned_inGlobalSpaceUnits, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_Quaternion.DrawSquareArea_spannedByUpAndForward(false, posWhereToDraw, localForwardVectorScaled_unturned_inGlobalSpaceUnits, localUpVectorScaled_unturned_inGlobalSpaceUnits, color_ofUnturnedForwardVector, color_ofUnturnedUpVector, 1.3f * alpha_ofSquareSpannedByForwardAndUp, 0.1f, durationInSec, hiddenByNearerObjects);

            if (rotationIsApproxIdentity == false)
            {
                Color color_of90DegSymbolAfterRotation = Color.Lerp(color_ofTurnedForwardVector, color_ofTurnedUpVector, 0.5f);
                UtilitiesDXXL_Quaternion.Draw90DegSymbolToQuaternionVectorPair(color_of90DegSymbolAfterRotation, posWhereToDraw, localForwardVectorScaled_turned_inGlobalSpaceUnits, localUpVectorScaled_turned_inGlobalSpaceUnits, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_Quaternion.DrawSquareArea_spannedByUpAndForward(false, posWhereToDraw, localForwardVectorScaled_turned_inGlobalSpaceUnits, localUpVectorScaled_turned_inGlobalSpaceUnits, color_ofTurnedForwardVector, color_ofTurnedUpVector, alpha_ofSquareSpannedByForwardAndUp, 0.1f, durationInSec, hiddenByNearerObjects);
            }
        }

        static float DrawRotationOfCustomVector(Vector3 customVectorToTurn_unturned_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits, Vector3 customVectorToTurn_unturned_inLocalSpaceUnits_butAlreadyScaledSoLengthFitsGlobalUnits, Color color_darkVariant, Color color_brightVariant, string vectorName, Vector3 posWhereToDraw, Vector3 eulerAnglesToDraw, Quaternion rotation_ofLocalSpace, Vector3 xAxis_rotated_inGlobalSpace_normalized, Vector3 yAxis_rotated_inGlobalSpace_normalized, Vector3 zAxis_rotated_inGlobalSpace_normalized, Quaternion localEulerRotationAroundRotatedXAxis, Quaternion localEulerRotationAroundYAxis, Quaternion localEulerRotationAroundRotatedZAxis, float gimbalSize, bool rotationIsApproxIdentity, bool isLocal, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 vectorToTurn_rotatedOnlyAroundYAxis_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits = localEulerRotationAroundYAxis * customVectorToTurn_unturned_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits;
            Vector3 vectorToTurn_rotatedAroundYAxisThenXAxis_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits = localEulerRotationAroundRotatedXAxis * vectorToTurn_rotatedOnlyAroundYAxis_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits;
            Vector3 vectorToTurn_rotatedAroundYAxisThenXAxisThenZAxis_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits = localEulerRotationAroundRotatedZAxis * vectorToTurn_rotatedAroundYAxisThenXAxis_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits;

            Vector3 vectorToTurn_rotatedOnlyAroundYAxis_inLocalSpaceUnits = localEulerRotationAroundYAxis * customVectorToTurn_unturned_inLocalSpaceUnits_butAlreadyScaledSoLengthFitsGlobalUnits;
            Vector3 vectorToTurn_rotatedAroundYAxisThenXAxis_inLocalSpaceUnits = localEulerRotationAroundRotatedXAxis * vectorToTurn_rotatedOnlyAroundYAxis_inLocalSpaceUnits;
            Vector3 vectorToTurn_rotatedAroundYAxisThenXAxisThenZAxis_inLocalSpaceUnits = localEulerRotationAroundRotatedZAxis * vectorToTurn_rotatedAroundYAxisThenXAxis_inLocalSpaceUnits;

            Vector3 aVector_unturned_inGlobalSpaceUnits = rotation_ofLocalSpace * customVectorToTurn_unturned_inLocalSpaceUnits_butAlreadyScaledSoLengthFitsGlobalUnits;
            Vector3 vectorToTurn_rotatedOnlyAroundYAxis_inGlobalSpaceUnits = rotation_ofLocalSpace * vectorToTurn_rotatedOnlyAroundYAxis_inLocalSpaceUnits;
            Vector3 vectorToTurn_rotatedAroundYAxisThenXAxis_inGlobalSpaceUnits = rotation_ofLocalSpace * vectorToTurn_rotatedAroundYAxisThenXAxis_inLocalSpaceUnits;
            Vector3 vectorToTurn_rotatedAroundYAxisThenXAxisThenZAxis_inGlobalSpaceUnits = rotation_ofLocalSpace * vectorToTurn_rotatedAroundYAxisThenXAxisThenZAxis_inLocalSpaceUnits;

            Vector3 endPos_ofAVectorUnturned_inGlobalSpaceUnits = posWhereToDraw + aVector_unturned_inGlobalSpaceUnits;
            Vector3 endPos_of_vectorToTurn_rotatedOnlyAroundYAxis_inGlobalSpaceUnits = posWhereToDraw + vectorToTurn_rotatedOnlyAroundYAxis_inGlobalSpaceUnits;
            Vector3 endPos_of_vectorToTurn_rotatedAroundYAxisThenXAxis_inGlobalSpaceUnits = posWhereToDraw + vectorToTurn_rotatedAroundYAxisThenXAxis_inGlobalSpaceUnits;

            Color color_ofThinFullCircle_x = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(UtilitiesDXXL_Colors.red_xAxisAlpha1, 0.33f);
            Color color_ofThinFullCircle_y = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(UtilitiesDXXL_Colors.green_yAxisAlpha1, 0.28f);
            Color color_ofThinFullCircle_z = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(UtilitiesDXXL_Colors.blue_zAxisAlpha1, 0.4f);

            Color color_forAccentuatedVector_x = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(UtilitiesDXXL_Colors.red_xAxisAlpha1, 0.8f);
            Color color_forAccentuatedVector_y = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(UtilitiesDXXL_Colors.green_yAxisAlpha1, 0.68f);
            Color color_forAccentuatedVector_z = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(UtilitiesDXXL_Colors.blue_zAxisAlpha1, 0.9f);

            float maxAbsDistance_ofAnyPlumbPos_toTurnCenter = 0.0f;
            float absDistance_ofPlumbPos_toTurnCenter;
            absDistance_ofPlumbPos_toTurnCenter = DrawAVectorsTurningAroundOneAxis(posWhereToDraw, endPos_ofAVectorUnturned_inGlobalSpaceUnits, yAxis_rotated_inGlobalSpace_normalized, eulerAnglesToDraw.y, rotatedYAxis_line_inGlobalSpaceUnits, color_forAccentuatedVector_y, color_ofThinFullCircle_y, gimbalSize, durationInSec, hiddenByNearerObjects);
            maxAbsDistance_ofAnyPlumbPos_toTurnCenter = Mathf.Max(absDistance_ofPlumbPos_toTurnCenter, maxAbsDistance_ofAnyPlumbPos_toTurnCenter);
            absDistance_ofPlumbPos_toTurnCenter = DrawAVectorsTurningAroundOneAxis(posWhereToDraw, endPos_of_vectorToTurn_rotatedOnlyAroundYAxis_inGlobalSpaceUnits, xAxis_rotated_inGlobalSpace_normalized, eulerAnglesToDraw.x, rotatedXAxis_line_inGlobalSpaceUnits, color_forAccentuatedVector_x, color_ofThinFullCircle_x, gimbalSize, durationInSec, hiddenByNearerObjects);
            maxAbsDistance_ofAnyPlumbPos_toTurnCenter = Mathf.Max(absDistance_ofPlumbPos_toTurnCenter, maxAbsDistance_ofAnyPlumbPos_toTurnCenter);
            absDistance_ofPlumbPos_toTurnCenter = DrawAVectorsTurningAroundOneAxis(posWhereToDraw, endPos_of_vectorToTurn_rotatedAroundYAxisThenXAxis_inGlobalSpaceUnits, zAxis_rotated_inGlobalSpace_normalized, eulerAnglesToDraw.z, rotatedZAxis_line_inGlobalSpaceUnits, color_forAccentuatedVector_z, color_ofThinFullCircle_z, gimbalSize, durationInSec, hiddenByNearerObjects);
            maxAbsDistance_ofAnyPlumbPos_toTurnCenter = Mathf.Max(absDistance_ofPlumbPos_toTurnCenter, maxAbsDistance_ofAnyPlumbPos_toTurnCenter);

            string text_atUnturnedCustomVector;
            if (rotationIsApproxIdentity)
            {
                text_atUnturnedCustomVector = isLocal ? (vectorName + "<br><size=4> </size><br>unrotated<br>=after rotation<br><size=8><size=2>local</size>x = " + customVectorToTurn_unturned_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits.x + "<br><size=2>local</size>y = " + customVectorToTurn_unturned_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits.y + "<br><size=2>local</size>z = " + customVectorToTurn_unturned_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits.z + "</size>") : (vectorName + "<br><size=4> </size><br>unrotated<br>=after rotation<br><size=8>x = " + customVectorToTurn_unturned_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits.x + "<br>y = " + customVectorToTurn_unturned_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits.y + "<br>z = " + customVectorToTurn_unturned_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits.z + "</size>");
            }
            else
            {
                text_atUnturnedCustomVector = isLocal ? (vectorName + "<br><size=4> </size><br>unrotated<br><size=8><size=2>local</size>x = " + customVectorToTurn_unturned_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits.x + "<br><size=2>local</size>y = " + customVectorToTurn_unturned_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits.y + "<br><size=2>local</size>z = " + customVectorToTurn_unturned_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits.z + "</size>") : (vectorName + "<br><size=4> </size><br>unrotated<br><size=8>x = " + customVectorToTurn_unturned_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits.x + "<br>y = " + customVectorToTurn_unturned_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits.y + "<br>z = " + customVectorToTurn_unturned_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits.z + "</size>");
            }
            DrawBasics.VectorFrom(posWhereToDraw, aVector_unturned_inGlobalSpaceUnits, color_darkVariant, 0.0f, text_atUnturnedCustomVector, 0.17f, false, false, default(Vector3), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);

            if (rotationIsApproxIdentity == false)
            {
                string text_atTurnedCustomVector = isLocal ? (vectorName + "<br><size=4> </size><br>after rotation<br><size=8><size=2>local</size>x = " + vectorToTurn_rotatedAroundYAxisThenXAxisThenZAxis_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits.x + "<br><size=2>local</size>y = " + vectorToTurn_rotatedAroundYAxisThenXAxisThenZAxis_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits.y + "<br><size=2>local</size>z = " + vectorToTurn_rotatedAroundYAxisThenXAxisThenZAxis_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits.z + "</size>") : (vectorName + "<br><size=4> </size><br>after rotation<br><size=8>x = " + vectorToTurn_rotatedAroundYAxisThenXAxisThenZAxis_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits.x + "<br>y = " + vectorToTurn_rotatedAroundYAxisThenXAxisThenZAxis_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits.y + "<br>z = " + vectorToTurn_rotatedAroundYAxisThenXAxisThenZAxis_inLocalSpaceUnits_lengthStillFitsLocalSpaceUnits.z + "</size>");
                DrawBasics.VectorFrom(posWhereToDraw, vectorToTurn_rotatedAroundYAxisThenXAxisThenZAxis_inGlobalSpaceUnits, color_brightVariant, 0.0f, text_atTurnedCustomVector, 0.17f, false, false, default(Vector3), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
            }

            return maxAbsDistance_ofAnyPlumbPos_toTurnCenter;
        }

        static float DrawAVectorsTurningAroundOneAxis(Vector3 posWhereToDraw, Vector3 endPos_ofAVectorToTurn_preTurn_inGlobalSpaceUnits, Vector3 axisToTurnAround_rotated_inGlobalSpace_normalized, float eulerAnglesToDraw_ofConcernedAxis, InternalDXXL_Line line_describingTheTurnedGizmoAxis, Color color_forAccentuatedVector, Color color_ofThinFullCircle, float gimbalSize, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 plumbPos_ofUnturnedVectorOnTurnAxis = line_describingTheTurnedGizmoAxis.Get_perpProjectionOfPoint_ontoThisLine(endPos_ofAVectorToTurn_preTurn_inGlobalSpaceUnits);
            Vector3 fromPlumbPos_toUnturnedVectorsPeak = endPos_ofAVectorToTurn_preTurn_inGlobalSpaceUnits - plumbPos_ofUnturnedVectorOnTurnAxis;
            float absDistance_ofPlumbPos_toTurnCenter = (plumbPos_ofUnturnedVectorOnTurnAxis - posWhereToDraw).magnitude;
            float radius = fromPlumbPos_toUnturnedVectorsPeak.magnitude;
            if (radius > 0.01f)
            {
                DrawShapes.Circle(plumbPos_ofUnturnedVectorOnTurnAxis, radius, color_ofThinFullCircle, axisToTurnAround_rotated_inGlobalSpace_normalized, fromPlumbPos_toUnturnedVectorsPeak, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);

                float absTurnAngleDeg = Mathf.Abs(eulerAnglesToDraw_ofConcernedAxis);
                float drawnTurnAngleDeg = Loop360degProtrudingAngle_toDrawableNon360_isIntoSpan_fromExclM360_toExclP360(eulerAnglesToDraw_ofConcernedAxis);
                float absDrawnTurnAngleDeg = Mathf.Abs(drawnTurnAngleDeg);
                float lineWidth_ofCircledArrow = gimbalSize * 0.0096f;

                bool absAngle_isBiggerOrEqualThan360deg = Mathf.Abs(absTurnAngleDeg) >= 360.0f;
                if (absAngle_isBiggerOrEqualThan360deg)
                {
                    //using "FlatShape()" instead of "Circle()" because it has a "flattenRoundLines_intoShapePlane"-parameter
                    float diameter = 2.0f * radius;
                    DrawShapes.FlatShape(plumbPos_ofUnturnedVectorOnTurnAxis, DrawShapes.Shape2DType.circle, diameter, diameter, color_forAccentuatedVector, axisToTurnAround_rotated_inGlobalSpace_normalized, default(Vector3), lineWidth_ofCircledArrow, null, DrawBasics.LineStyle.solid, 1.0f, false, DrawBasics.LineStyle.invisible, false, durationInSec, hiddenByNearerObjects);
                }

                if (absDrawnTurnAngleDeg >= 0.5f)
                {
                    float absConeLength_ofCircledVectors_thatDisplaysThePerAxisCustomVectorTurning = gimbalSize * 0.08f;

                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forCircledVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                    DrawBasics.VectorCircled(endPos_ofAVectorToTurn_preTurn_inGlobalSpaceUnits, plumbPos_ofUnturnedVectorOnTurnAxis, axisToTurnAround_rotated_inGlobalSpace_normalized, drawnTurnAngleDeg, color_forAccentuatedVector, lineWidth_ofCircledArrow, null, absConeLength_ofCircledVectors_thatDisplaysThePerAxisCustomVectorTurning, false, false, false, 0.0f, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, durationInSec, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forCircledVectors();
                }

                if (absTurnAngleDeg >= 0.5f)
                {
                    if (absTurnAngleDeg < (2.0f * angleDeg_betweenTurnedCircleSlice_visualizerLines))
                    {
                        Quaternion rotation_from_toStartDir_to_middleOfStartAndEndDir = Quaternion.AngleAxis(0.5f * absTurnAngleDeg, axisToTurnAround_rotated_inGlobalSpace_normalized);
                        Vector3 plumbPos_to_endPosOfSingleSliceVisualizerStrut = rotation_from_toStartDir_to_middleOfStartAndEndDir * fromPlumbPos_toUnturnedVectorsPeak;
                        Line_fadeableAnimSpeed.InternalDraw(plumbPos_ofUnturnedVectorOnTurnAxis, plumbPos_ofUnturnedVectorOnTurnAxis + plumbPos_to_endPosOfSingleSliceVisualizerStrut, color_ofThinFullCircle, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                    }
                    else
                    {
                        float turnAngleDeg_ofCurrSliceVisualizerStrut = 0.0f;
                        int maxStruts_minus1 = 10000;
                        for (int i_strut = 0; i_strut <= maxStruts_minus1; i_strut++)
                        {
                            if (i_strut == maxStruts_minus1)
                            {
                                UtilitiesDXXL_Log.PrintErrorCode("28-" + eulerAnglesToDraw_ofConcernedAxis + "-" + angleDeg_betweenTurnedCircleSlice_visualizerLines);
                            }

                            if (eulerAnglesToDraw_ofConcernedAxis > 0.0f)
                            {
                                turnAngleDeg_ofCurrSliceVisualizerStrut = turnAngleDeg_ofCurrSliceVisualizerStrut + angleDeg_betweenTurnedCircleSlice_visualizerLines;
                                if (turnAngleDeg_ofCurrSliceVisualizerStrut > eulerAnglesToDraw_ofConcernedAxis)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                turnAngleDeg_ofCurrSliceVisualizerStrut = turnAngleDeg_ofCurrSliceVisualizerStrut - angleDeg_betweenTurnedCircleSlice_visualizerLines;
                                if (turnAngleDeg_ofCurrSliceVisualizerStrut < eulerAnglesToDraw_ofConcernedAxis)
                                {
                                    break;
                                }
                            }
                            Quaternion rotation_from_toStartDir_to_currSliceVisualizerStrut = Quaternion.AngleAxis(turnAngleDeg_ofCurrSliceVisualizerStrut, axisToTurnAround_rotated_inGlobalSpace_normalized);
                            Vector3 plumbPos_to_endPosOfCurrSliceVisualizerStrut = rotation_from_toStartDir_to_currSliceVisualizerStrut * fromPlumbPos_toUnturnedVectorsPeak;
                            Line_fadeableAnimSpeed.InternalDraw(plumbPos_ofUnturnedVectorOnTurnAxis, plumbPos_ofUnturnedVectorOnTurnAxis + plumbPos_to_endPosOfCurrSliceVisualizerStrut, color_ofThinFullCircle, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                        }
                    }

                    //accentuated straight border lines of pieSlices:
                    Quaternion rotation_from_toStartDir_to_toEndDir = Quaternion.AngleAxis(eulerAnglesToDraw_ofConcernedAxis, axisToTurnAround_rotated_inGlobalSpace_normalized);
                    Vector3 plumbPos_to_endPosOfWholeRotation = rotation_from_toStartDir_to_toEndDir * fromPlumbPos_toUnturnedVectorsPeak;

                    Line_fadeableAnimSpeed.InternalDraw(plumbPos_ofUnturnedVectorOnTurnAxis, plumbPos_ofUnturnedVectorOnTurnAxis + fromPlumbPos_toUnturnedVectorsPeak, color_forAccentuatedVector, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                    Line_fadeableAnimSpeed.InternalDraw(plumbPos_ofUnturnedVectorOnTurnAxis, plumbPos_ofUnturnedVectorOnTurnAxis + plumbPos_to_endPosOfWholeRotation, color_forAccentuatedVector, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                }
            }
            return absDistance_ofPlumbPos_toTurnCenter;
        }

        static void DrawThinDashedAxesProlongations(Vector3 xAxis_startPos_inGlobalSpace, Vector3 xAxis_endPos_inGlobalSpace, Vector3 yAxis_startPos_inGlobalSpace, Vector3 yAxis_endPos_inGlobalSpace, Vector3 zAxis_startPos_inGlobalSpace, Vector3 zAxis_endPos_inGlobalSpace, Vector3 xAxis_rotated_inGlobalSpace_normalized, Vector3 yAxis_rotated_inGlobalSpace_normalized, Vector3 zAxis_rotated_inGlobalSpace_normalized, float axesLength_toEachSideFromDrawCenter, float maxAbsDistance_ofAnyPlumbPos_toTurnCenter, float durationInSec, bool hiddenByNearerObjects)
        {
            float length_ofAxesInclProlongation_perSideFromDrawCenter = maxAbsDistance_ofAnyPlumbPos_toTurnCenter * 1.6f;
            float length_ofAxesProlongations = length_ofAxesInclProlongation_perSideFromDrawCenter - axesLength_toEachSideFromDrawCenter;
            float stylePatternScaleFactor = axesLength_toEachSideFromDrawCenter * 7.0f;
            DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.dashed;

            Color color_ofXAxisProlongation = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(UtilitiesDXXL_Colors.red_xAxisAlpha1, 0.55f);
            Line_fadeableAnimSpeed.InternalDraw(xAxis_startPos_inGlobalSpace, xAxis_startPos_inGlobalSpace - xAxis_rotated_inGlobalSpace_normalized * length_ofAxesProlongations, color_ofXAxisProlongation, 0.0f, null, lineStyle, stylePatternScaleFactor, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, true, true);
            Line_fadeableAnimSpeed.InternalDraw(xAxis_endPos_inGlobalSpace, xAxis_endPos_inGlobalSpace + xAxis_rotated_inGlobalSpace_normalized * length_ofAxesProlongations, color_ofXAxisProlongation, 0.0f, null, lineStyle, stylePatternScaleFactor, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, true, true);

            Color color_ofYAxisProlongation = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(UtilitiesDXXL_Colors.green_yAxisAlpha1, 0.3f);
            Line_fadeableAnimSpeed.InternalDraw(yAxis_startPos_inGlobalSpace, yAxis_startPos_inGlobalSpace - yAxis_rotated_inGlobalSpace_normalized * length_ofAxesProlongations, color_ofYAxisProlongation, 0.0f, null, lineStyle, stylePatternScaleFactor, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, true, true);
            Line_fadeableAnimSpeed.InternalDraw(yAxis_endPos_inGlobalSpace, yAxis_endPos_inGlobalSpace + yAxis_rotated_inGlobalSpace_normalized * length_ofAxesProlongations, color_ofYAxisProlongation, 0.0f, null, lineStyle, stylePatternScaleFactor, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, true, true);

            Color color_ofZAxisProlongation = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(UtilitiesDXXL_Colors.blue_zAxisAlpha1, 0.72f);
            Line_fadeableAnimSpeed.InternalDraw(zAxis_startPos_inGlobalSpace, zAxis_startPos_inGlobalSpace - zAxis_rotated_inGlobalSpace_normalized * length_ofAxesProlongations, color_ofZAxisProlongation, 0.0f, null, lineStyle, stylePatternScaleFactor, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, true, true);
            Line_fadeableAnimSpeed.InternalDraw(zAxis_endPos_inGlobalSpace, zAxis_endPos_inGlobalSpace + zAxis_rotated_inGlobalSpace_normalized * length_ofAxesProlongations, color_ofZAxisProlongation, 0.0f, null, lineStyle, stylePatternScaleFactor, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, true, true);
        }

    }

}
