namespace DrawXXL
{
    using UnityEngine;

    public class InternalDXXL_Line
    {
        public Vector3 origin;
        public Vector3 direction;

        public float length;
        public Vector3 direction_normalized;
        public bool originHasBeenRelocated = false;
        Vector3 directionProlongedIntoStableFloatRegion; //not always filled

        public InternalDXXL_Line()
        {
            //x-axis through world origin:
            origin = Vector3.zero;
            direction = new Vector3(1.0f, 0.0f, 0.0f);
            length = 1.0f;
            direction_normalized = direction;
        }

        public void RecreateLineFromTwoPoints(Vector3 point1, Vector3 point2)
        {
            origin = point1;

            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(point1, point2))
            {
                Debug.LogError("Cannot create a line3D with a zero-vector as direction. Provided points were " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(point1) + " and " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(point2) + " Now creating standard-line3D instead, which is x-Axis.");
                origin = Vector3.zero;
                direction = new Vector3(1.0f, 0.0f, 0.0f);
            }
            else
            {
                direction = point2 - point1;
                if (CheckIfDirVectorIsZero(direction))
                {
                    Debug.LogError("Cannot create a line3D with a zero-vector as direction. Provided points were " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(point1) + " and " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(point2) + " Now creating standard-line3D instead, which is x-Axis.");
                    origin = Vector3.zero;
                    direction = new Vector3(1.0f, 0.0f, 0.0f);
                }
            }

            CalcLengthAndNormalizedDir(false);
            RelocateFarAwayOrigins();
        }

        public void Recreate(Vector3 lineOrigin, Vector3 lineDirection, bool lineDir_isAlreadyGuaranteedNormalized)
        {
            origin = lineOrigin;
            if (UtilitiesDXXL_Math.ApproximatelyZero(lineDirection))
            {
                //Debug.LogError("'lineDirection' is zero -> Fallback to x-axis-line.");
                UtilitiesDXXL_Log.PrintErrorCode("22");

                direction = new Vector3(1.0f, 0.0f, 0.0f);
                length = 1.0f;
                direction_normalized = direction;
            }
            else
            {
                direction = lineDirection;
                CalcLengthAndNormalizedDir(lineDir_isAlreadyGuaranteedNormalized);
                RelocateFarAwayOrigins();
            }
        }

        void CalcLengthAndNormalizedDir(bool direction_isAlreadyGuaranteedNormalized)
        {
            if (direction_isAlreadyGuaranteedNormalized)
            {
                length = 1.0f;
                directionProlongedIntoStableFloatRegion = direction;
                direction_normalized = direction;
            }
            else
            {
                length = direction.magnitude;
                directionProlongedIntoStableFloatRegion = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(direction, out bool directionHasBeenRescaledIntoStableFloatRegion);

                if (directionHasBeenRescaledIntoStableFloatRegion)
                {
                    float lengthOfProlongedDirection = directionProlongedIntoStableFloatRegion.magnitude;
                    direction_normalized = directionProlongedIntoStableFloatRegion / lengthOfProlongedDirection;
                }
                else
                {
                    direction_normalized = direction / length;
                }
            }
        }

        public Vector3 Get_intersectionPoint_withPlane_withoutParallelCheck(InternalDXXL_Plane plane)
        {
            float lenghtOfDirVector = ((plane.d - plane.a * origin.x - plane.b * origin.y - plane.c * origin.z) / (plane.a * direction_normalized.x + plane.b * direction_normalized.y + plane.c * direction_normalized.z));
            if (float.IsNaN(lenghtOfDirVector) || float.IsInfinity(lenghtOfDirVector))
            {
                return new Vector3(float.NaN, float.NaN, float.NaN);
            }
            else
            {
                return (origin + lenghtOfDirVector * direction_normalized);
            }
        }


        static bool CheckIfDirVectorIsZero(Vector3 dirVectorToCheck)
        {
            if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(dirVectorToCheck.x, 0.0f))
            {
                if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(dirVectorToCheck.y, 0.0f))
                {
                    if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(dirVectorToCheck.z, 0.0f))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


        InternalDXXL_Plane universalUsablePlane = new InternalDXXL_Plane();
        public Vector3 Get_perpProjectionOfPoint_ontoThisLine(Vector3 pointToProject)
        {
            //plane-meaning: "plane_perpToLine_throughPointToProject"
            universalUsablePlane.Recreate(pointToProject, direction_normalized);
            return Get_intersectionPoint_withPlane_withoutParallelCheck(universalUsablePlane);
        }

        public Vector3 Get_vectorFromPoint_perpOntoThisLine(Vector3 pointToGetVectorFor)
        {
            Vector3 givenPoints_perpProjection_ontoThisLine = Get_perpProjectionOfPoint_ontoThisLine(pointToGetVectorFor);
            return givenPoints_perpProjection_ontoThisLine - pointToGetVectorFor;
        }

        public Vector3 Get_vectorFromLine_perpToGivenPoint(Vector3 pointToGetVectorFor)
        {
            Vector3 givenPoints_perpProjection_ontoThisLine = Get_perpProjectionOfPoint_ontoThisLine(pointToGetVectorFor);
            return pointToGetVectorFor - givenPoints_perpProjection_ontoThisLine;
        }

        public Vector3 Get_randomPerpVectorAwayFromLine_notNormalized()
        {
            return Get_vectorFromLine_perpToGivenPoint(new Vector3(0.10293f, 1.315532f, 2.10928f)); //using a "randomSeldomPoint"
        }

        public float Get_perpDistance_ofGivenPoint_toThisLine(Vector3 pointToGetDistanceFor)
        {
            return Get_vectorFromPoint_perpOntoThisLine(pointToGetDistanceFor).magnitude;
        }


        static InternalDXXL_Plane perpPlane_throughWorldOrigin = new InternalDXXL_Plane();
        static float maxAllowedDistance_aboveWhichOriginGetsRelocated = 100000.0f;
        public void RelocateFarAwayOrigins()
        {
            float biggestAbsComponent_ofOrigin = UtilitiesDXXL_Math.GetBiggestAbsComponent(origin);
            if (biggestAbsComponent_ofOrigin > maxAllowedDistance_aboveWhichOriginGetsRelocated)
            {
                //'origin' gets relocated because float positions become rough and uncertain over 100000.0f, which leads to undefined behaviour.
                perpPlane_throughWorldOrigin.Recreate(Vector3.zero, direction_normalized);
                origin = Get_intersectionPoint_withPlane_withoutParallelCheck(perpPlane_throughWorldOrigin);
                originHasBeenRelocated = true;
            }

        }

        public Vector3 Get_posOnLine_thatIsNearestTo_passingOtherLine(InternalDXXL_Line passingOtherLine)
        {
            if (UtilitiesDXXL_Math.Check_ifTwoVectorsAreApproxParallel_butCanHeadToDifferntDirs_expensiveButAccurate(direction_normalized, passingOtherLine.direction_normalized))
            {
                return new Vector3(float.NaN, float.NaN, float.NaN);
            }

            //plane-meaning: "plane_throughGivenLine_spannedFromThisLinesDirAndGivenLinesDir"
            universalUsablePlane.Recreate(passingOtherLine.origin, passingOtherLine.origin + direction_normalized, passingOtherLine.origin + passingOtherLine.direction_normalized);
            Vector3 normalOfOtherLine_insidePlaneThatIsSpannedByTheTwoLines = universalUsablePlane.Get_projectionOfVectorOntoPlane(passingOtherLine.Get_randomPerpVectorAwayFromLine_notNormalized());
            //plane-meaning: "plane_throughOtherLine_butMostPerpTo_passingDirOfThisLine"
            universalUsablePlane.Recreate(passingOtherLine.origin, normalOfOtherLine_insidePlaneThatIsSpannedByTheTwoLines);
            return this.Get_intersectionPoint_withPlane_withoutParallelCheck(universalUsablePlane);
        }

        public bool ErrorLogForInvalidLineParameters()
        {
            if (UtilitiesDXXL_Math.FloatIsInvalid(origin.x) || UtilitiesDXXL_Math.FloatIsInvalid(origin.y) || UtilitiesDXXL_Math.FloatIsInvalid(origin.z))
            {
                Debug.LogError("The Vector3 'origin' contains invalid float components: ( <b>x</b> is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(origin.x) + ", <b>y</b> is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(origin.y) + ", <b>z</b> is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(origin.z) + ").");
                return false;
            }
            else
            {
                if (UtilitiesDXXL_Math.FloatIsInvalid(direction.x) || UtilitiesDXXL_Math.FloatIsInvalid(direction.y) || UtilitiesDXXL_Math.FloatIsInvalid(direction.z))
                {
                    Debug.LogError("The Vector3 'direction' contains invalid float components: ( <b>x</b> is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(direction.x) + ", <b>y</b> is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(direction.y) + ", <b>z</b> is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(direction.z) + ").");
                    return false;
                }
                else
                {
                    if (UtilitiesDXXL_Math.FloatIsInvalid(direction_normalized.x) || UtilitiesDXXL_Math.FloatIsInvalid(direction_normalized.y) || UtilitiesDXXL_Math.FloatIsInvalid(direction_normalized.z))
                    {
                        Debug.LogError("The Vector3 'direction_normalized' contains invalid float components: ( <b>x</b> is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(direction_normalized.x) + ", <b>y</b> is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(direction_normalized.y) + ", <b>z</b> is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(direction_normalized.z) + ").");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
    }

}
