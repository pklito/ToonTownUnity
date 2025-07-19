namespace DrawXXL
{
    using UnityEngine;

    public class InternalDXXL_Plane
    {
        ///plane equation: a*x + b*y + c*z = d

        ///meaning of the parameters:
        /// x-axisInterception = d / a
        /// y-axisInterception = d / b
        /// z-axisInterception = d / c
        /// a, b und c define the planes normal vector as (a,b,c)
        /// a = inverse of x-axisInterception * d
        /// b = inverse of y-axisInterception * d
        /// c = inverse of z-axisInterception * d
        /// d = perpendicular distance to origin?

        public float a;
        public float b;
        public float c;
        public float d;

        public Vector3 normalDir; //not necessarily normalized

        //not defined for every plane:
        Vector3 triangleMountingPoint;
        Vector3 triangleMountingPoint_toFirstTriangleCorner;
        Vector3 triangleMountingPoint_toSecondTriangleCorner;

        //saving GC.Alloc:
        public static InternalDXXL_Plane horizPlane_throughZeroOrigin = new InternalDXXL_Plane(Vector3.zero, Vector3.up);
        public static InternalDXXL_Plane xyPlane_throughZeroOrigin = new InternalDXXL_Plane(Vector3.zero, Vector3.forward);
        public static InternalDXXL_Plane zyPlane_throughZeroOrigin = new InternalDXXL_Plane(Vector3.zero, Vector3.right);

        public void Recreate(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            triangleMountingPoint = point1;
            triangleMountingPoint_toFirstTriangleCorner = point2 - point1;
            triangleMountingPoint_toSecondTriangleCorner = point3 - point1;
            normalDir = Vector3.Cross(triangleMountingPoint_toFirstTriangleCorner, triangleMountingPoint_toSecondTriangleCorner);
            normalDir = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(normalDir);

            a = normalDir.x;
            b = normalDir.y;
            c = normalDir.z;
            d = triangleMountingPoint.x * a + triangleMountingPoint.y * b + triangleMountingPoint.z * c;

            ErrorLogForInvalidPlanes();
        }

        public void Recreate(Vector3 point1, Vector3 point2, Vector3 point3, bool skipErrorOfNonValidPlane)
        {
            triangleMountingPoint = point1;
            triangleMountingPoint_toFirstTriangleCorner = point2 - point1;
            triangleMountingPoint_toSecondTriangleCorner = point3 - point1;
            normalDir = Vector3.Cross(triangleMountingPoint_toFirstTriangleCorner, triangleMountingPoint_toSecondTriangleCorner);
            normalDir = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(normalDir);

            a = normalDir.x;
            b = normalDir.y;
            c = normalDir.z;
            d = triangleMountingPoint.x * a + triangleMountingPoint.y * b + triangleMountingPoint.z * c;

            if (skipErrorOfNonValidPlane == false) { ErrorLogForInvalidPlanes(); }
        }

        void ErrorLogForInvalidPlanes()
        {
            if (CheckIfPlaneIsValid() == false)
            {
                Debug.LogError("All plane parameters are zero. Seems like you wanted to create a plane with three points that lie on a line and therefore don't describe a plane.");
            }
        }

        public bool CheckIfPlaneIsValid()
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(a))
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(b))
                {
                    if (UtilitiesDXXL_Math.ApproximatelyZero(c))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public bool CheckIfPlaneIsValid(float floatCalculationErrorTolerancePadding)
        {
            if (UtilitiesDXXL_Math.CheckIfValueLiesInsideDistanceNearAnotherValue(0.0f, a, floatCalculationErrorTolerancePadding))
            {
                if (UtilitiesDXXL_Math.CheckIfValueLiesInsideDistanceNearAnotherValue(0.0f, b, floatCalculationErrorTolerancePadding))
                {
                    if (UtilitiesDXXL_Math.CheckIfValueLiesInsideDistanceNearAnotherValue(0.0f, c, floatCalculationErrorTolerancePadding))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public InternalDXXL_Plane(Vector3 aPointOnThePlane, Vector3 normalOfThePlane_notNecessarilyNormalized)
        {
            normalDir = normalOfThePlane_notNecessarilyNormalized;
            normalDir = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(normalDir);

            if (UtilitiesDXXL_Math.ApproximatelyZero(normalDir))
            {
                Debug.LogError("Cannot create a plane with normal that has 0 lenght: " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(normalDir));
            }

            a = normalDir.x;
            b = normalDir.y;
            c = normalDir.z;
            d = a * aPointOnThePlane.x + b * aPointOnThePlane.y + c * aPointOnThePlane.z;
        }

        public void Recreate(Vector3 aPointOnThePlane, Vector3 normalOfThePlane_notNecessarilyNormalized)
        {
            normalDir = normalOfThePlane_notNecessarilyNormalized;
            normalDir = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(normalDir);

            if (UtilitiesDXXL_Math.ApproximatelyZero(normalDir))
            {
                Debug.LogError("Cannot create a plane with normal that has 0 lenght: " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(normalDir));
            }

            a = normalDir.x;
            b = normalDir.y;
            c = normalDir.z;
            d = a * aPointOnThePlane.x + b * aPointOnThePlane.y + c * aPointOnThePlane.z;
        }

        public InternalDXXL_Plane()
        {
            //default: xy-plane through origin
            normalDir = new Vector3(0.0f, 0.0f, 1.0f);
            a = normalDir.x;
            b = normalDir.y;
            c = normalDir.z;
            d = 0.0f;
        }

        public void TryRecreateAsCopyOfOther(InternalDXXL_Plane planeToCopyFieldsFrom)
        {
            if (planeToCopyFieldsFrom != null)
            {
                normalDir = planeToCopyFieldsFrom.normalDir;
                a = planeToCopyFieldsFrom.a;
                b = planeToCopyFieldsFrom.b;
                c = planeToCopyFieldsFrom.c;
                d = planeToCopyFieldsFrom.d;
            }
        }

        public bool CheckIfLineIsParallel(InternalDXXL_Line line3D)
        {
            return UtilitiesDXXL_Math.Check_ifVectors_arePerp(normalDir, line3D.direction_normalized);
        }

        public static void Calc_intersectionLine_ofTwoPlanes(ref InternalDXXL_Line alreadyConstructedLineToFill, InternalDXXL_Plane plane1, InternalDXXL_Plane plane2)
        {
            Vector3 directionOfLine3D = Vector3.Cross(plane1.normalDir, plane2.normalDir);
            directionOfLine3D = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(directionOfLine3D);
            Vector3 aValidPointOnTheLine3D = default;

            bool lineIsParallelTo_yzPlane = UtilitiesDXXL_Math.ApproximatelyZero(directionOfLine3D.x);
            if (lineIsParallelTo_yzPlane == false)
            {
                aValidPointOnTheLine3D.x = 0.0f;
                aValidPointOnTheLine3D.z = (((plane2.d * plane1.b) - (plane1.d * plane2.b)) / ((plane2.c * plane1.b) - (plane1.c * plane2.b)));

                if (plane1.b != 0.0f)
                {
                    aValidPointOnTheLine3D.y = ((plane1.d - plane1.c * aValidPointOnTheLine3D.z) / plane1.b);
                }
                else
                {
                    aValidPointOnTheLine3D.y = ((plane2.d - plane2.c * aValidPointOnTheLine3D.z) / plane2.b);
                }
            }
            else
            {
                bool lineIsParallelTo_xzPlane = UtilitiesDXXL_Math.ApproximatelyZero(directionOfLine3D.y);
                if (lineIsParallelTo_xzPlane == false)
                {
                    aValidPointOnTheLine3D.y = 0.0f;
                    aValidPointOnTheLine3D.x = (((plane2.c * plane1.d) - (plane1.c * plane2.d)) / ((plane2.c * plane1.a) - (plane1.c * plane2.a)));

                    if (plane1.c != 0.0f)
                    {
                        aValidPointOnTheLine3D.z = ((plane1.d - plane1.a * aValidPointOnTheLine3D.x) / plane1.c);
                    }
                    else
                    {
                        aValidPointOnTheLine3D.z = ((plane2.d - plane2.a * aValidPointOnTheLine3D.x) / plane2.c);
                    }
                }
                else
                {
                    bool lineIsParallelTo_xyPlane = UtilitiesDXXL_Math.ApproximatelyZero(directionOfLine3D.z);
                    if (lineIsParallelTo_xyPlane == false)
                    {
                        aValidPointOnTheLine3D.z = 0.0f;
                        aValidPointOnTheLine3D.y = (((plane2.a * plane1.d) - (plane1.a * plane2.d)) / ((plane2.a * plane1.b) - (plane1.a * plane2.b)));

                        if (plane1.a != 0.0f)
                        {
                            aValidPointOnTheLine3D.x = ((plane1.d - plane1.b * aValidPointOnTheLine3D.y) / plane1.a);
                        }
                        else
                        {
                            aValidPointOnTheLine3D.x = ((plane2.d - plane2.b * aValidPointOnTheLine3D.y) / plane2.a);
                        }
                    }
                    else
                    {
                        //-> "directionOfLine3D" is zero
                        //-> the two planes are parallel
                        //-> "aValidPointOnTheLine3D" is undefined (=left at default of 0/0/0)
                        //-> "alreadyConstructedLineToFill.Recreate" will notify this via ErrorCode-Log
                    }
                }
            }
            alreadyConstructedLineToFill.Recreate(aValidPointOnTheLine3D, directionOfLine3D, false);
        }

        public Vector3 GetIntersectionWithLine(InternalDXXL_Line intersectingLine)
        {
            return intersectingLine.Get_intersectionPoint_withPlane_withoutParallelCheck(this);
        }


        static InternalDXXL_Line perpLine = new InternalDXXL_Line();
        public Vector3 Get_perpProjectionOfPointOnPlane(Vector3 pointToProjectOntoPlane)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(normalDir))
            {
                Debug.LogError("not allowed: normal is zero: " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(normalDir));
                normalDir = Vector3.forward;
            }

            perpLine.Recreate(pointToProjectOntoPlane, normalDir, false);
            return perpLine.Get_intersectionPoint_withPlane_withoutParallelCheck(this);
        }

        public Vector3 Get_perpVector_fromPlaneToPoint(Vector3 point_toWhichResultingVectorWillPoint)
        {
            Vector3 perpProjection_ofPointOnPlane = Get_perpProjectionOfPointOnPlane(point_toWhichResultingVectorWillPoint);
            return (point_toWhichResultingVectorWillPoint - perpProjection_ofPointOnPlane);
        }

        static InternalDXXL_Line customProjectionDirLine = new InternalDXXL_Line();
        public Vector3 Get_projectionOfPointOnPlane_alongCustomDir(Vector3 pointToProjectOntoPlane, Vector3 dir_alongWhichToProject)
        {
            customProjectionDirLine.Recreate(pointToProjectOntoPlane, dir_alongWhichToProject, false);
            return customProjectionDirLine.Get_intersectionPoint_withPlane_withoutParallelCheck(this);
        }

        public bool CheckIf_twoPoints_lieOnDifferentSidesOfThePlane_returnsFalseIfAGivenPointIsONplane(Vector3 point1, Vector3 point2)
        {
            return UtilitiesDXXL_Math.Check_ifVectorsPointAwayFromEachOther_perpCountsAsPointingInSameDir(Get_perpVector_fromPlaneToPoint(point1), Get_perpVector_fromPlaneToPoint(point2));
        }

        public Vector3 Get_projectionOfVectorOntoPlane(Vector3 vector_toProject)
        {
            return (Get_perpProjectionOfPointOnPlane(vector_toProject) - Get_perpProjectionOfPointOnPlane(Vector3.zero));
        }

        public Vector3 Get_projectionOfVectorOntoPlane_alongCustomDir(Vector3 vector_toProject, Vector3 dir_alongWhichToProject)
        {
            //-> doesn't contain parallel-check. Caller has to ensure that "dir_alongWhichToProject" is not parallel to plane
            return (Get_projectionOfPointOnPlane_alongCustomDir(vector_toProject, dir_alongWhichToProject) - Get_projectionOfPointOnPlane_alongCustomDir(Vector3.zero, dir_alongWhichToProject));
        }

        public static bool IsHorizontal(InternalDXXL_Plane planeToCheckIfItIsHorizontal)
        {
            return (UtilitiesDXXL_Math.ApproximatelyZero(planeToCheckIfItIsHorizontal.normalDir.x) && UtilitiesDXXL_Math.ApproximatelyZero(planeToCheckIfItIsHorizontal.normalDir.z));
        }

    }

}
