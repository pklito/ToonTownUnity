namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class UtilitiesDXXL_Math
    {
        public enum SkewedDirection { upLeft, upRight, downRight, downLeft, center };
        public enum Dimension { x, y, z };
        public enum DimensionNullable { x, y, z, none };

        public static float sqrtOf2_precalced = 1.414214f;
        public static float sqrtOf2_precalced_minus1 = 0.414214f;
        public static float inverseSqrtOf2_precalced = 0.7071065f;
        public static float anglesRad_perCircle = 2.0f * Mathf.PI;
        public static Vector3 arbitrarySeldomDir_precalced = new Vector3(0.132443f, 0.23452f, 0.87365f);
        public static Vector3 arbitrarySeldomDir2_precalced = new Vector3(-0.381128f, -0.18123f, -0.76529f);
        public static Vector3 arbitrarySeldomDir_normalized_precalced = new Vector3(0.1448693f, 0.2565236f, 0.9556195f);

        public static bool Check_ifVectors_arePerp(Vector3 first_vector3, Vector3 second_vector3)
        {
            return (ApproximatelyZero(Vector3.Dot(first_vector3, second_vector3)));
        }

        public static bool Check_ifTwoNormalizedVectorsAreApproxPerp_DXXL(Vector3 firstNormalizedVector3, Vector3 secondNormalizedVector3)
        {
            float threshold = 0.0001f;
            return (Abs(Vector3.Dot(firstNormalizedVector3, secondNormalizedVector3)) < threshold);
        }

        public static bool Check_ifVectorsPointAwayFromEachOther_perpCountsAsPointingInSameDir(Vector3 first_vector3, Vector3 second_vector3)
        {
            return (Vector3.Dot(first_vector3, second_vector3) < 0.0f);
        }

        public static bool Check_ifVectorsPointAwayFromEachOther_perpCountsAsPointingAwayFromEachOther(Vector3 first_vector3, Vector3 second_vector3)
        {
            return (Vector3.Dot(first_vector3, second_vector3) <= 0.0f);
        }

        public static bool Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(Vector2 first_vector2, Vector2 second_vector2)
        {
            return ((Vector2.Dot(first_vector2, second_vector2) < 0.0f) == false);
        }

        public static bool Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(Vector3 first_vector3, Vector3 second_vector3)
        {
            return ((Vector3.Dot(first_vector3, second_vector3) < 0.0f) == false);
        }

        public static bool Check_ifTwoNormalizedVectorsAreApproxParallel_butCanHeadToDifferntDirs(Vector3 first_vector3_normalized, Vector3 second_vector3_normalized)
        {
            Vector3 crossProduct_ofVectors = Vector3.Cross(first_vector3_normalized, second_vector3_normalized);
            return CheckIfCrossProductResultMeans_approxParallel_butCanHeadToDifferntDirs(crossProduct_ofVectors);
        }

        public static bool CheckIfCrossProductResultMeans_approxParallel_butCanHeadToDifferntDirs(Vector3 crossProductResult)
        {
            return (Abs(crossProductResult.x) <= Mathf.Epsilon && Abs(crossProductResult.y) <= Mathf.Epsilon && Abs(crossProductResult.z) <= Mathf.Epsilon);
        }

        public static Vector3 GetAVector_perpToGivenVectors(Vector3 vector1, Vector3 vector2)
        {
            return Vector3.Cross(vector1, vector2);
        }

        public static bool Check_ifTwoVectorsAreApproxParallel_butCanHeadToDifferntDirs_expensiveButAccurate(Vector3 first_vector3, Vector3 second_vector3)
        {
            first_vector3 = GetNormalized_afterScalingIntoRegionOfFloatPrecicion(first_vector3);
            second_vector3 = GetNormalized_afterScalingIntoRegionOfFloatPrecicion(second_vector3);
            return Check_ifTwoNormalizedVectorsAreApproxParallel_butCanHeadToDifferntDirs(first_vector3, second_vector3);
        }

        public static bool Check_ifTwoVectorsAreApproxParallel_butCanHeadToDifferntDirs_DXXL(Vector3 firstVector3, Vector3 secondVector3)
        {
            firstVector3 = GetNormalized_afterScalingIntoRegionOfFloatPrecicion(firstVector3);
            secondVector3 = GetNormalized_afterScalingIntoRegionOfFloatPrecicion(secondVector3);
            return Check_ifTwoNormalizedVectorsAreApproxParallel_butCanHeadToDifferntDirs_padding(firstVector3, secondVector3);
        }

        public static bool Check_ifTwoNormalizedVectorsAreApproxParallel_butCanHeadToDifferntDirs_padding(Vector3 firstNormalizedVector3, Vector3 secondNormalizedVector3, float absPadding)
        {
            Vector3 crossProduct_ofVectors = Vector3.Cross(firstNormalizedVector3, secondNormalizedVector3);
            return (Abs(crossProduct_ofVectors.x) <= absPadding && Abs(crossProduct_ofVectors.y) <= absPadding && Abs(crossProduct_ofVectors.z) <= absPadding);
        }

        public static bool Check_ifTwoNormalizedVectorsAreApproxParallel_butCanHeadToDifferntDirs_padding(Vector3 firstNormalizedVector3, Vector3 secondNormalizedVector3)
        {
            return Check_ifTwoNormalizedVectorsAreApproxParallel_butCanHeadToDifferntDirs_padding(firstNormalizedVector3, secondNormalizedVector3, 0.0001f);
        }

        public static bool CheckIf_twoFloatsAreApproximatelyEqual(float a, float b)
        {
            return (a >= (b - Mathf.Epsilon)) && (a <= (b + Mathf.Epsilon));
        }

        public static bool ApproximatelyZero(float float_toCheckItIsApproximatelyZero)
        {
            return (float_toCheckItIsApproximatelyZero >= (-Mathf.Epsilon)) && (float_toCheckItIsApproximatelyZero <= (Mathf.Epsilon));
        }

        public static bool CheckIf_twoVectorsAreApproximatelyEqual(Vector3 a, Vector3 b)
        {
            return (CheckIf_twoFloatsAreApproximatelyEqual(a.x, b.x) && CheckIf_twoFloatsAreApproximatelyEqual(a.y, b.y) && CheckIf_twoFloatsAreApproximatelyEqual(a.z, b.z));
        }

        public static bool CheckIf_twoVectorsAreExactlyEqual(Vector3 a, Vector3 b)
        {
            return ((a.x == b.x) && (a.y == b.y) && (a.z == b.z));
        }

        public static bool CheckIf_twoVectorsAreExactlyEqual(Vector2 a, Vector2 b)
        {
            return ((a.x == b.x) && (a.y == b.y));
        }

        public static bool CheckIf_vectorsAreApproximatelyEqual(Vector3 a, Vector3 b, Vector3 c)
        {
            if (CheckIf_twoVectorsAreApproximatelyEqual(a, b))
            {
                if (CheckIf_twoVectorsAreApproximatelyEqual(a, c))
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

        public static bool CheckIf_vectorsAreApproximatelyEqual(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
        {
            if (CheckIf_twoVectorsAreApproximatelyEqual(a, b))
            {
                if (CheckIf_twoVectorsAreApproximatelyEqual(a, c))
                {
                    if (CheckIf_twoVectorsAreApproximatelyEqual(a, d))
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

        public static bool ApproximatelyZero(Vector3 vector3_toCheckIfItIsApproximatelyZero)
        {
            return (ApproximatelyZero(vector3_toCheckIfItIsApproximatelyZero.x) && ApproximatelyZero(vector3_toCheckIfItIsApproximatelyZero.y) && ApproximatelyZero(vector3_toCheckIfItIsApproximatelyZero.z));
        }

        public static bool IsDefaultVector(Vector3 vector3_toCheckIfItIsTheDefaultVector)
        {
            return ApproximatelyZero(vector3_toCheckIfItIsTheDefaultVector);
        }

        public static bool IsDefaultVector(Vector2 vector2_toCheckIfItIsTheDefaultVector)
        {
            return ApproximatelyZero(vector2_toCheckIfItIsTheDefaultVector);
        }

        public static bool IsDefaultInvalidQuaternion(Quaternion quaternion_toCheckIfItIsTheDefaultInvalid)
        {
            return (ApproximatelyZero(quaternion_toCheckIfItIsTheDefaultInvalid.x) && ApproximatelyZero(quaternion_toCheckIfItIsTheDefaultInvalid.y) && ApproximatelyZero(quaternion_toCheckIfItIsTheDefaultInvalid.z) && ApproximatelyZero(quaternion_toCheckIfItIsTheDefaultInvalid.w));
        }

        public static bool IsQuaternionIdentity(Quaternion quaternion_toCheckIfItIsIdentity)
        {
            return (ApproximatelyZero(quaternion_toCheckIfItIsIdentity.x) && ApproximatelyZero(quaternion_toCheckIfItIsIdentity.y) && ApproximatelyZero(quaternion_toCheckIfItIsIdentity.z) && CheckIf_twoFloatsAreApproximatelyEqual(quaternion_toCheckIfItIsIdentity.w, 1.0f));
        }

        public static bool QuaternionIsApproxNormalized(Quaternion quaternion_toCheck)
        {
            float quaternionMagnitude = GetQuaternionMagnitude(quaternion_toCheck);
            return CheckIfValueLiesInsideDistanceNearAnotherValue(quaternionMagnitude, 1.0f, 0.01f);
        }

        public static float GetQuaternionMagnitude(Quaternion quaternion_toCheck)
        {
            return Mathf.Sqrt(quaternion_toCheck.x * quaternion_toCheck.x + quaternion_toCheck.y * quaternion_toCheck.y + quaternion_toCheck.z * quaternion_toCheck.z + quaternion_toCheck.w * quaternion_toCheck.w);
        }

        public static bool CheckIf_twoQuaternionsAreApproximatelyEqual(Quaternion a, Quaternion b)
        {
            return (CheckIf_twoFloatsAreApproximatelyEqual(a.x, b.x) && CheckIf_twoFloatsAreApproximatelyEqual(a.y, b.y) && CheckIf_twoFloatsAreApproximatelyEqual(a.z, b.z) && CheckIf_twoFloatsAreApproximatelyEqual(a.w, b.w));
        }

        public static bool CheckIf_twoQuaternionsAreExactlyEqual(Quaternion a, Quaternion b)
        {
            return ((a.x == b.x) && (a.y == b.y) && (a.z == b.z) && (a.w == b.w));
        }

        public static bool CheckIf_twoVectorsAreApproximatelyEqual(Vector2 a, Vector2 b)
        {
            return (CheckIf_twoFloatsAreApproximatelyEqual(a.x, b.x) && CheckIf_twoFloatsAreApproximatelyEqual(a.y, b.y));
        }

        public static bool ApproximatelyZero(Vector2 vector2_toCheckIfItIsApproximatelyZero)
        {
            return (ApproximatelyZero(vector2_toCheckIfItIsApproximatelyZero.x) && ApproximatelyZero(vector2_toCheckIfItIsApproximatelyZero.y));
        }

        public static float Get_linearRise(float given_x)
        {
            return (given_x);
        }

        public static float Get_linearDecay(float given_x)
        {
            return (1.0f - given_x);
        }

        public static float Abs(float signedNumber)
        {
            if (signedNumber < 0.0f)
            {
                return (-signedNumber);
            }
            else
            {
                return signedNumber;
            }
        }

        public static Vector3 Abs(Vector3 vector)
        {
            return new Vector3(Abs(vector.x), Abs(vector.y), Abs(vector.z));
        }

        public static Vector2 Abs(Vector2 vector)
        {
            return new Vector2(Abs(vector.x), Abs(vector.y));
        }

        public static bool CheckIf_givenNumberIs_evenNotOdd(int int_toCheck)
        {
            return (int_toCheck % 2 == 0);
        }

        public static bool CheckIfValueLiesInsideDistanceNearAnotherValue(float referenceValue_fromWhichPaddingIsMeasured, float valueToCheck_ifItLiesNearReferenceValue, float paddingSpan_usedForEachSide_soThisIsHalfOfTheWholeToleranceSpan)
        {
            if (valueToCheck_ifItLiesNearReferenceValue < (referenceValue_fromWhichPaddingIsMeasured - paddingSpan_usedForEachSide_soThisIsHalfOfTheWholeToleranceSpan))
            {
                return false;
            }
            else
            {
                if (valueToCheck_ifItLiesNearReferenceValue > (referenceValue_fromWhichPaddingIsMeasured + paddingSpan_usedForEachSide_soThisIsHalfOfTheWholeToleranceSpan))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        public static float GetSign_trueGivesPlus1_falseGivesMinus1(bool signAsBool)
        {
            if (signAsBool == true)
            {
                return (1.0f);
            }
            else
            {
                return (-1.0f);
            }
        }

        public static bool CheckIfVectorIsParallelToXAxis(Vector3 vector)
        {
            return (ApproximatelyZero(vector.y) && ApproximatelyZero(vector.z));
        }

        public static bool CheckIfVectorIsParallelToYAxis(Vector3 vector)
        {
            return (ApproximatelyZero(vector.x) && ApproximatelyZero(vector.z));
        }

        public static bool CheckIfVectorIsParallelToZAxis(Vector3 vector)
        {
            return (ApproximatelyZero(vector.x) && ApproximatelyZero(vector.y));
        }

        public static float Max(float value1, float value2, float value3)
        {
            return (Mathf.Max(Mathf.Max(value1, value2), value3));
        }

        public static float Min(float value1, float value2, float value3)
        {
            return (Mathf.Min(Mathf.Min(value1, value2), value3));
        }

        public static float Max(float value1, float value2, float value3, float value4, float value5)
        {
            return (Mathf.Max((Mathf.Max((Mathf.Max(Mathf.Max(value1, value2), value3)), value4)), value5));
        }

        public static double Max(double value1, double value2)
        {
            if (value1 > value2)
            {
                return value1;
            }
            else
            {
                return value2;
            }
        }

        public static double Min(double value1, double value2)
        {
            if (value1 < value2)
            {
                return value1;
            }
            else
            {
                return value2;
            }
        }

        public static float AbsNonZeroValue(float valueToAbs_ifNonZero)
        {
            if (ApproximatelyZero(valueToAbs_ifNonZero) == false)
            {
                return Mathf.Abs(valueToAbs_ifNonZero);
            }
            else
            {
                return valueToAbs_ifNonZero;
            }
        }

        public static bool VectorIsInvalid(Vector3 vectorToCheckForValidity)
        {
            return (FloatIsInvalid(vectorToCheckForValidity.x) || FloatIsInvalid(vectorToCheckForValidity.y) || FloatIsInvalid(vectorToCheckForValidity.z));
        }

        public static bool VectorIsInvalid(Vector2 vectorToCheckForValidity)
        {
            return (FloatIsInvalid(vectorToCheckForValidity.x) || FloatIsInvalid(vectorToCheckForValidity.y));
        }

        public static bool FloatIsValid(float floatToCheckForValidity)
        {
            return ((float.IsNaN(floatToCheckForValidity) == false) && (float.IsInfinity(floatToCheckForValidity) == false));
        }

        public static bool DoubleIsValid(double doubleToCheckForValidity)
        {
            return ((double.IsNaN(doubleToCheckForValidity) == false) && (double.IsInfinity(doubleToCheckForValidity) == false));
        }

        public static bool FloatIsInvalid(float floatToCheckForValidity)
        {
            return (float.IsNaN(floatToCheckForValidity) || float.IsInfinity(floatToCheckForValidity));
        }

        public static string GetFloatInvalidTypeAsString(float floatToCheckForValidity)
        {
            if (float.IsNaN(floatToCheckForValidity))
            {
                return "NaN ('not a number')";
            }
            else
            {
                if (float.IsPositiveInfinity(floatToCheckForValidity))
                {
                    return "positive infinity";
                }
                else
                {
                    if (float.IsNegativeInfinity(floatToCheckForValidity))
                    {
                        return "negative infinity";
                    }
                    else
                    {
                        return "valid float: " + floatToCheckForValidity;
                    }
                }
            }
        }

        public static float GetBiggestAbsComponent(Vector2 vector)
        {
            float biggestAbsComponent = 0.0f;
            float absX = Mathf.Abs(vector.x);
            if (absX > biggestAbsComponent)
            {
                biggestAbsComponent = absX;
            }

            float absY = Mathf.Abs(vector.y);
            if (absY > biggestAbsComponent)
            {
                biggestAbsComponent = absY;
            }

            return biggestAbsComponent;
        }

        public static float GetBiggestAbsComponent(Vector3 vector)
        {
            float biggestAbsComponent = 0.0f;
            float absX = Mathf.Abs(vector.x);
            if (absX > biggestAbsComponent)
            {
                biggestAbsComponent = absX;
            }

            float absY = Mathf.Abs(vector.y);
            if (absY > biggestAbsComponent)
            {
                biggestAbsComponent = absY;
            }

            float absZ = Mathf.Abs(vector.z);
            if (absZ > biggestAbsComponent)
            {
                biggestAbsComponent = absZ;
            }
            return biggestAbsComponent;
        }

        public static float GetBiggestAbsComponent_ignoringZ(Vector3 vector)
        {
            float biggestAbsComponent = 0.0f;
            float absX = Mathf.Abs(vector.x);
            if (absX > biggestAbsComponent)
            {
                biggestAbsComponent = absX;
            }

            float absY = Mathf.Abs(vector.y);
            if (absY > biggestAbsComponent)
            {
                biggestAbsComponent = absY;
            }

            return biggestAbsComponent;
        }

        public static float GetBiggestAbsComponent_butReassignTheSign(Vector3 vector)
        {
            float currentSign = 1.0f;
            float biggestAbsComponent = 0.0f;
            float absX = Mathf.Abs(vector.x);
            if (absX > biggestAbsComponent)
            {
                biggestAbsComponent = absX;
                currentSign = Mathf.Sign(vector.x);
            }

            float absY = Mathf.Abs(vector.y);
            if (absY > biggestAbsComponent)
            {
                biggestAbsComponent = absY;
                currentSign = Mathf.Sign(vector.y);
            }

            float absZ = Mathf.Abs(vector.z);
            if (absZ > biggestAbsComponent)
            {
                biggestAbsComponent = absZ;
                currentSign = Mathf.Sign(vector.z);
            }
            return (currentSign * biggestAbsComponent);
        }

        public static float GetBiggestAbsComponent_butReassignTheSign(Vector3 vector, Dimension dimensionToExcludeFromCheck)
        {
            float currentSign = 1.0f;
            float biggestAbsComponent = 0.0f;

            float absX;
            float absY;
            float absZ;

            switch (dimensionToExcludeFromCheck)
            {
                case Dimension.x:

                    absY = Mathf.Abs(vector.y);
                    if (absY > biggestAbsComponent)
                    {
                        biggestAbsComponent = absY;
                        currentSign = Mathf.Sign(vector.y);
                    }

                    absZ = Mathf.Abs(vector.z);
                    if (absZ > biggestAbsComponent)
                    {
                        biggestAbsComponent = absZ;
                        currentSign = Mathf.Sign(vector.z);
                    }

                    break;
                case Dimension.y:

                    absX = Mathf.Abs(vector.x);
                    if (absX > biggestAbsComponent)
                    {
                        biggestAbsComponent = absX;
                        currentSign = Mathf.Sign(vector.x);
                    }

                    absZ = Mathf.Abs(vector.z);
                    if (absZ > biggestAbsComponent)
                    {
                        biggestAbsComponent = absZ;
                        currentSign = Mathf.Sign(vector.z);
                    }

                    break;
                case Dimension.z:

                    absX = Mathf.Abs(vector.x);
                    if (absX > biggestAbsComponent)
                    {
                        biggestAbsComponent = absX;
                        currentSign = Mathf.Sign(vector.x);
                    }

                    absY = Mathf.Abs(vector.y);
                    if (absY > biggestAbsComponent)
                    {
                        biggestAbsComponent = absY;
                        currentSign = Mathf.Sign(vector.y);
                    }

                    break;
                default:
                    break;
            }

            return (currentSign * biggestAbsComponent);
        }

        public static float GetComponentByDimension(Vector3 vector, Dimension dimensionMarkingTheComponentToObtain)
        {
            switch (dimensionMarkingTheComponentToObtain)
            {
                case Dimension.x:
                    return vector.x;
                case Dimension.y:
                    return vector.y;
                case Dimension.z:
                    return vector.z;
                default:
                    return 0.0f;
            }
        }

        public static Vector3 GetUnitVectorOfDimension(Dimension dimensionMarkingTheComponentToObtain)
        {
            switch (dimensionMarkingTheComponentToObtain)
            {
                case Dimension.x:
                    return Vector3.right;
                case Dimension.y:
                    return Vector3.up;
                case Dimension.z:
                    return Vector3.forward;
                default:
                    return Vector3.zero;
            }
        }

        public static float GetSmallestComponent(Vector3 vector)
        {
            return Min(vector.x, vector.z, vector.z);
        }

        static InternalDXXL_Plane plane_perpToLine = new InternalDXXL_Plane();
        public static Vector3 Get_aNormalizedVector_perpToGivenVector(Vector3 givenVector, InternalDXXL_Plane plane_inWhichPerpResultVectorPreferablyLies)
        {
            if (ApproximatelyZero(givenVector))
            {
                return Vector3.up;
            }

            Vector3 aNormalizedVector_perpToLine;
            if ((plane_inWhichPerpResultVectorPreferablyLies == null) || Check_ifTwoVectorsAreApproxParallel_butCanHeadToDifferntDirs_expensiveButAccurate(givenVector, plane_inWhichPerpResultVectorPreferablyLies.normalDir))
            {
                if (CheckIfVectorIsParallelToXAxis(givenVector))
                {
                    aNormalizedVector_perpToLine = Vector3.up;
                }
                else
                {
                    if (CheckIfVectorIsParallelToYAxis(givenVector))
                    {
                        aNormalizedVector_perpToLine = Vector3.right;
                    }
                    else
                    {
                        if (CheckIfVectorIsParallelToZAxis(givenVector))
                        {
                            aNormalizedVector_perpToLine = Vector3.up;
                        }
                        else
                        {
                            if (ApproximatelyZero(givenVector.y))
                            {
                                //line is perp to yAxis
                                aNormalizedVector_perpToLine = Vector3.up;
                            }
                            else
                            {
                                //line is not perp to yAxis
                                plane_perpToLine.Recreate(Vector3.zero, givenVector);
                                Vector3 aVector_perpToLine = plane_perpToLine.Get_projectionOfVectorOntoPlane(Vector3.up);
                                aNormalizedVector_perpToLine = GetNormalized_afterScalingIntoRegionOfFloatPrecicion(aVector_perpToLine);
                            }
                        }
                    }
                }
            }
            else
            {
                //"plane_inWhichPerpResultVectorPreferablyLies" is not null, and "lineDir" is not perp to "plane_inWhichPerpResultVectorPreferablyLies":
                Vector3 aVector_perpToLine_insidePlane = Vector3.Cross(givenVector, plane_inWhichPerpResultVectorPreferablyLies.normalDir);
                aNormalizedVector_perpToLine = GetNormalized_afterScalingIntoRegionOfFloatPrecicion(aVector_perpToLine_insidePlane);
            }

            if (ApproximatelyZero(aNormalizedVector_perpToLine))
            {
                plane_perpToLine.Recreate(Vector3.zero, givenVector);
                Vector3 aVector_perpToLine = plane_perpToLine.Get_projectionOfVectorOntoPlane(arbitrarySeldomDir_precalced);
                aNormalizedVector_perpToLine = GetNormalized_afterScalingIntoRegionOfFloatPrecicion(aVector_perpToLine);
            }
            return aNormalizedVector_perpToLine;
        }

        public static Vector3 Get_aNormalizedVector_perpToGivenVector(Vector3 givenVector)
        {
            if (ApproximatelyZero(givenVector.y))
            {
                //line is horizonal (=perp to yAxis) / or zero
                return Vector3.up;
            }
            else
            {
                //line is not horizontal
                if (ApproximatelyZero(givenVector.x) && ApproximatelyZero(givenVector.z))
                {
                    //line is vertical
                    return Vector3.forward;
                }
                else
                {
                    plane_perpToLine.Recreate(Vector3.zero, givenVector);
                    Vector3 projection_ofGlobalUpVector_alongLineDir_onto_perpToLinePlane = plane_perpToLine.Get_projectionOfVectorOntoPlane(Vector3.up);
                    return GetNormalized_afterScalingIntoRegionOfFloatPrecicion(projection_ofGlobalUpVector_alongLineDir_onto_perpToLinePlane);
                }
            }
        }

        public static Vector3 Get_vector_projectedAlongOtherVectorToPerpToOtherVector(Vector3 vectorToProject, Vector3 otherVectorThatIsProjectionDir, bool tryNormalize)
        {
            if (ApproximatelyZero(otherVectorThatIsProjectionDir))
            {
                return Vector3.forward;
            }
            else
            {
                plane_perpToLine.Recreate(Vector3.zero, otherVectorThatIsProjectionDir);
                //Vector3 theVector_perpToRefVector = plane_perpToLine.Get_projectionOfVectorOntoPlane(vectorToProject); //-> since the plane is guaranteed "through zero origin" it is not needed to project "the whole vector", but it is sufficient to treat the vector as a point, and only project the point. This saves around 50% cpu operations.
                Vector3 theVector_perpToRefVector = plane_perpToLine.Get_perpProjectionOfPointOnPlane(vectorToProject);
                if (tryNormalize)
                {
                    return GetNormalized_afterScalingIntoRegionOfFloatPrecicion(theVector_perpToRefVector);
                }
                else
                {
                    return theVector_perpToRefVector;
                }
            }
        }

        public static Vector3 GetApproxNormalized(Vector3 vectorToApproxNormalize)
        {
            //-> not tested yet if this is actually faster than "Vector3.Normalize()"
            //-> Note that if the "vectorToApproxNormalize" is already normalized, then this will "denormalize" it.
            float biggestAbsComponent = GetBiggestAbsComponent(vectorToApproxNormalize);
            if (ApproximatelyZero(biggestAbsComponent))
            {
                return Vector3.zero;
            }
            else
            {
                return (vectorToApproxNormalize / biggestAbsComponent);
            }
        }

        public static Vector3 GetApproxNormalized_afterScalingIntoRegionOfFloatPrecicion(Vector3 vectorToApproxNormalize)
        {
            vectorToApproxNormalize = ScaleNonZeroVectorIntoRegionOfFloatPrecision(vectorToApproxNormalize);
            return GetApproxNormalized(vectorToApproxNormalize);
        }


        public static Vector3 OverwriteDefaultVectors(Vector3 vectorToOverwrite_ifDefault, Vector3 overwritingVector)
        {
            if (IsDefaultVector(vectorToOverwrite_ifDefault))
            {
                return overwritingVector;
            }
            else
            {
                return vectorToOverwrite_ifDefault;
            }
        }

        public static Vector2 OverwriteDefaultVectors(Vector2 vectorToOverwrite_ifDefault, Vector2 overwritingVector)
        {
            if (IsDefaultVector(vectorToOverwrite_ifDefault))
            {
                return overwritingVector;
            }
            else
            {
                return vectorToOverwrite_ifDefault;
            }
        }

        public static Quaternion OverwriteDefaultQuaternionToIdentity(Quaternion quaternionToOverwrite_ifDefault)
        {
            if (IsDefaultInvalidQuaternion(quaternionToOverwrite_ifDefault))
            {
                return Quaternion.identity;
            }
            else
            {
                return quaternionToOverwrite_ifDefault;
            }
        }

        public static Vector2 ScaleNonZeroVectorIntoRegionOfFloatPrecision(Vector2 vectorToScale)
        {
            Vector2 scaledVector = vectorToScale;
            for (int i = 0; i < 10; i++)
            {
                float biggestAbsComponent_ofScaledVector = GetBiggestAbsComponent(scaledVector);
                if (biggestAbsComponent_ofScaledVector < 0.001f)
                {
                    scaledVector = scaledVector * 1000.0f;
                }
                else
                {
                    if (biggestAbsComponent_ofScaledVector > 100000.0f)
                    {
                        scaledVector = scaledVector * 0.001f;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return scaledVector;
        }

        public static Vector3 ScaleNonZeroVectorToApproxBiggerThanMinLength(Vector3 vectorToScale, float minLength)
        {
            Vector3 scaledVector = vectorToScale;
            for (int i = 0; i < 10; i++)
            {
                //-> "GetBiggestAbsComponent()" is smaller than vector.magnitude -> therefore "*Approx*" in the function name
                float biggestAbsComponent_ofScaledVector = GetBiggestAbsComponent(scaledVector);
                if (biggestAbsComponent_ofScaledVector < minLength)
                {
                    scaledVector = scaledVector * 1000.0f;
                }
                else
                {
                    break;
                }
            }
            return scaledVector;
        }

        public static Vector3 ScaleNonZeroVectorIntoRegionOfFloatPrecision(Vector3 vectorToScale)
        {
            Vector3 scaledVector = vectorToScale;
            for (int i = 0; i < 10; i++)
            {
                float biggestAbsComponent_ofScaledVector = GetBiggestAbsComponent(scaledVector);
                if (biggestAbsComponent_ofScaledVector < 0.001f)
                {
                    scaledVector = scaledVector * 1000.0f;
                }
                else
                {
                    if (biggestAbsComponent_ofScaledVector > 100000.0f)
                    {
                        scaledVector = scaledVector * 0.001f;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return scaledVector;
        }

        public static bool CheckIfScaleToFloatPrecisionRegionFailed_meaningLineStayedTooShort(Vector3 previouslyScaledVectorToCheck)
        {
            return (GetBiggestAbsComponent(previouslyScaledVectorToCheck) < 0.001f);
        }

        public static Vector3 ScaleNonZeroVectorIntoRegionOfFloatPrecision(Vector3 vectorToScale, out bool wasRescaled)
        {
            Vector3 scaledVector = vectorToScale;
            wasRescaled = false;
            for (int i = 0; i < 10; i++)
            {
                float biggestAbsComponent_ofScaledVector = GetBiggestAbsComponent(scaledVector);
                if (biggestAbsComponent_ofScaledVector < 0.001f)
                {
                    scaledVector = scaledVector * 1000.0f;
                    wasRescaled = true;
                }
                else
                {
                    if (biggestAbsComponent_ofScaledVector > 100000.0f)
                    {
                        scaledVector = scaledVector * 0.001f;
                        wasRescaled = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return scaledVector;
        }

        public static Vector3 ScaleNonZeroVectorIntoRegionOfFloatPrecision(Vector3 vectorToScale, out bool wasRescaled, out float rescaleFactor)
        {
            Vector3 scaledVector = vectorToScale;
            rescaleFactor = 1.0f;
            wasRescaled = false;
            for (int i = 0; i < 10; i++)
            {
                float biggestAbsComponent_ofScaledVector = GetBiggestAbsComponent(scaledVector);
                if (biggestAbsComponent_ofScaledVector < 0.001f)
                {
                    scaledVector = scaledVector * 1000.0f;
                    rescaleFactor = rescaleFactor * 1000.0f;
                    wasRescaled = true;
                }
                else
                {
                    if (biggestAbsComponent_ofScaledVector > 100000.0f)
                    {
                        scaledVector = scaledVector * 0.001f;
                        rescaleFactor = rescaleFactor * 0.001f;
                        wasRescaled = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return scaledVector;
        }

        public static Vector2 ScaleNonZeroVectorIntoRegionOfFloatPrecision(Vector2 vectorToScale, out bool wasRescaled, out float rescaleFactor)
        {
            Vector2 scaledVector = vectorToScale;
            rescaleFactor = 1.0f;
            wasRescaled = false;
            for (int i = 0; i < 10; i++)
            {
                float biggestAbsComponent_ofScaledVector = GetBiggestAbsComponent(scaledVector);
                if (biggestAbsComponent_ofScaledVector < 0.001f)
                {
                    scaledVector = scaledVector * 1000.0f;
                    rescaleFactor = rescaleFactor * 1000.0f;
                    wasRescaled = true;
                }
                else
                {
                    if (biggestAbsComponent_ofScaledVector > 100000.0f)
                    {
                        scaledVector = scaledVector * 0.001f;
                        rescaleFactor = rescaleFactor * 0.001f;
                        wasRescaled = true;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return scaledVector;
        }

        public static Vector2 GetNormalized_afterScalingIntoRegionOfFloatPrecicion(Vector2 vectorToNormalize)
        {
            //the build-in ".normalize" function has problems with very small or very big vectors (and returns zero-vectors for those). This can be fixed by scaling the vectorToNormalize before the actual normalizing.
            vectorToNormalize = ScaleNonZeroVectorIntoRegionOfFloatPrecision(vectorToNormalize);
            return vectorToNormalize.normalized;
        }

        public static Vector2 GetNormalized_afterScalingIntoRegionOfFloatPrecicion(Vector2 vectorToNormalize, out float magnitudeOfUnscaledOriginalVector)
        {
            //the build-in ".normalize" function has problems with very small or very big vectors (and returns zero-vectors for those). This can be fixed by scaling the vectorToNormalize before the actual normalizing.
            bool wasRescaled;
            float rescaleFactor;
            vectorToNormalize = ScaleNonZeroVectorIntoRegionOfFloatPrecision(vectorToNormalize, out wasRescaled, out rescaleFactor);
            float magnitude_ofScaledVector = vectorToNormalize.magnitude;
            magnitudeOfUnscaledOriginalVector = wasRescaled ? (magnitude_ofScaledVector / rescaleFactor) : magnitude_ofScaledVector;

            if (ApproximatelyZero(magnitude_ofScaledVector))
            {
                return Vector2.zero;
            }
            else
            {
                return vectorToNormalize / magnitude_ofScaledVector;
            }
        }

        public static Vector3 GetNormalized_afterScalingIntoRegionOfFloatPrecicion(Vector3 vectorToNormalize)
        {
            //the build-in ".normalize" function has problems with very small or very big vectors (and returns zero-vectors for those). This can be fixed by scaling the vectorToNormalize before the actual normalizing.
            vectorToNormalize = ScaleNonZeroVectorIntoRegionOfFloatPrecision(vectorToNormalize);
            return vectorToNormalize.normalized;
        }

        public static Vector3 GetNormalized_afterScalingIntoRegionOfFloatPrecicion(Vector3 vectorToNormalize, out float magnitudeOfUnscaledOriginalVector)
        {
            //the build-in ".normalize" function has problems with very small or very big vectors (and returns zero-vectors for those). This can be fixed by scaling the vectorToNormalize before the actual normalizing.
            bool wasRescaled;
            float rescaleFactor;
            vectorToNormalize = ScaleNonZeroVectorIntoRegionOfFloatPrecision(vectorToNormalize, out wasRescaled, out rescaleFactor);
            float magnitude_ofScaledVector = vectorToNormalize.magnitude;
            magnitudeOfUnscaledOriginalVector = wasRescaled ? (magnitude_ofScaledVector / rescaleFactor) : magnitude_ofScaledVector;

            if (ApproximatelyZero(magnitude_ofScaledVector))
            {
                return Vector3.zero;
            }
            else
            {
                return vectorToNormalize / magnitude_ofScaledVector;
            }
        }

        public static bool CheckIfNormalizationFailed_meaningLineStayedTooShort(Vector3 previouslyNormalizedVectorToCheck)
        {
            return (GetBiggestAbsComponent(previouslyNormalizedVectorToCheck) < 0.1f);
        }

        public static bool CheckIfNormalizationFailed_meaningLineStayedTooShort(Vector2 previouslyNormalizedVectorToCheck)
        {
            return (GetBiggestAbsComponent(previouslyNormalizedVectorToCheck) < 0.1f);
        }

        public static float GetHighestXComponent(Vector3[] vertices)
        {
            //collection has to have at least 1 item
            float highestX = vertices[0].x;
            for (int i = 1; i < vertices.Length; i++)
            {
                highestX = Mathf.Max(highestX, vertices[i].x);
            }
            return highestX;
        }

        public static float GetLowestXComponent(Vector3[] vertices)
        {
            //collection has to have at least 1 item
            float lowestX = vertices[0].x;
            for (int i = 1; i < vertices.Length; i++)
            {
                lowestX = Mathf.Min(lowestX, vertices[i].x);
            }
            return lowestX;
        }

        public static float GetHighestYComponent(Vector3[] vertices)
        {
            //collection has to have at least 1 item
            float highestY = vertices[0].y;
            for (int i = 1; i < vertices.Length; i++)
            {
                highestY = Mathf.Max(highestY, vertices[i].y);
            }
            return highestY;
        }

        public static float GetLowestYComponent(Vector3[] vertices)
        {
            //collection has to have at least 1 item
            float lowestY = vertices[0].y;
            for (int i = 1; i < vertices.Length; i++)
            {
                lowestY = Mathf.Min(lowestY, vertices[i].y);
            }
            return lowestY;
        }

        public static float GetHighestZComponent(Vector3[] vertices)
        {
            //collection has to have at least 1 item
            float highestZ = vertices[0].z;
            for (int i = 1; i < vertices.Length; i++)
            {
                highestZ = Mathf.Max(highestZ, vertices[i].z);
            }
            return highestZ;
        }

        public static float GetLowestZComponent(Vector3[] vertices)
        {
            //collection has to have at least 1 item
            float lowestZ = vertices[0].z;
            for (int i = 1; i < vertices.Length; i++)
            {
                lowestZ = Mathf.Min(lowestZ, vertices[i].z);
            }
            return lowestZ;
        }

        public static float GetHighestXComponent(List<Vector3> vertices)
        {
            //collection has to have at least 1 item
            float highestX = vertices[0].x;
            for (int i = 1; i < vertices.Count; i++)
            {
                highestX = Mathf.Max(highestX, vertices[i].x);
            }
            return highestX;
        }

        public static float GetLowestXComponent(List<Vector3> vertices)
        {
            //collection has to have at least 1 item
            float lowestX = vertices[0].x;
            for (int i = 1; i < vertices.Count; i++)
            {
                lowestX = Mathf.Min(lowestX, vertices[i].x);
            }
            return lowestX;
        }

        public static float GetHighestYComponent(List<Vector3> vertices)
        {
            //collection has to have at least 1 item
            float highestY = vertices[0].y;
            for (int i = 1; i < vertices.Count; i++)
            {
                highestY = Mathf.Max(highestY, vertices[i].y);
            }
            return highestY;
        }

        public static float GetLowestYComponent(List<Vector3> vertices)
        {
            //collection has to have at least 1 item
            float lowestY = vertices[0].y;
            for (int i = 1; i < vertices.Count; i++)
            {
                lowestY = Mathf.Min(lowestY, vertices[i].y);
            }
            return lowestY;
        }

        public static float GetHighestZComponent(List<Vector3> vertices)
        {
            //collection has to have at least 1 item
            float highestZ = vertices[0].z;
            for (int i = 1; i < vertices.Count; i++)
            {
                highestZ = Mathf.Max(highestZ, vertices[i].z);
            }
            return highestZ;
        }

        public static float GetLowestZComponent(List<Vector3> vertices)
        {
            //collection has to have at least 1 item
            float lowestZ = vertices[0].z;
            for (int i = 1; i < vertices.Count; i++)
            {
                lowestZ = Mathf.Min(lowestZ, vertices[i].z);
            }
            return lowestZ;
        }

        public static float GetHighestXComponent(List<Vector3> vertices, int usedSlotsInList)
        {
            //collection has to have at least 1 item
            float highestX = vertices[0].x;
            for (int i = 1; i < usedSlotsInList; i++)
            {
                highestX = Mathf.Max(highestX, vertices[i].x);
            }
            return highestX;
        }

        public static float GetLowestXComponent(List<Vector3> vertices, int usedSlotsInList)
        {
            //collection has to have at least 1 item
            float lowestX = vertices[0].x;
            for (int i = 1; i < usedSlotsInList; i++)
            {
                lowestX = Mathf.Min(lowestX, vertices[i].x);
            }
            return lowestX;
        }

        public static float GetHighestYComponent(List<Vector3> vertices, int usedSlotsInList)
        {
            //collection has to have at least 1 item
            float highestY = vertices[0].y;
            for (int i = 1; i < usedSlotsInList; i++)
            {
                highestY = Mathf.Max(highestY, vertices[i].y);
            }
            return highestY;
        }

        public static float GetLowestYComponent(List<Vector3> vertices, int usedSlotsInList)
        {
            //collection has to have at least 1 item
            float lowestY = vertices[0].y;
            for (int i = 1; i < usedSlotsInList; i++)
            {
                lowestY = Mathf.Min(lowestY, vertices[i].y);
            }
            return lowestY;
        }

        public static float GetHighestZComponent(List<Vector3> vertices, int usedSlotsInList)
        {
            //collection has to have at least 1 item
            float highestZ = vertices[0].z;
            for (int i = 1; i < usedSlotsInList; i++)
            {
                highestZ = Mathf.Max(highestZ, vertices[i].z);
            }
            return highestZ;
        }

        public static float GetLowestZComponent(List<Vector3> vertices, int usedSlotsInList)
        {
            //collection has to have at least 1 item
            float lowestZ = vertices[0].z;
            for (int i = 1; i < usedSlotsInList; i++)
            {
                lowestZ = Mathf.Min(lowestZ, vertices[i].z);
            }
            return lowestZ;
        }

        public static float GetHighestXComponent(Vector2[] vertices)
        {
            //collection has to have at least 1 item
            float highestX = vertices[0].x;
            for (int i = 1; i < vertices.Length; i++)
            {
                highestX = Mathf.Max(highestX, vertices[i].x);
            }
            return highestX;
        }

        public static float GetLowestXComponent(Vector2[] vertices)
        {
            //collection has to have at least 1 item
            float lowestX = vertices[0].x;
            for (int i = 1; i < vertices.Length; i++)
            {
                lowestX = Mathf.Min(lowestX, vertices[i].x);
            }
            return lowestX;
        }

        public static float GetHighestYComponent(Vector2[] vertices)
        {
            //collection has to have at least 1 item
            float highestY = vertices[0].y;
            for (int i = 1; i < vertices.Length; i++)
            {
                highestY = Mathf.Max(highestY, vertices[i].y);
            }
            return highestY;
        }

        public static float GetLowestYComponent(Vector2[] vertices)
        {
            //collection has to have at least 1 item
            float lowestY = vertices[0].y;
            for (int i = 1; i < vertices.Length; i++)
            {
                lowestY = Mathf.Min(lowestY, vertices[i].y);
            }
            return lowestY;
        }

        public static float GetHighestXComponent(List<Vector2> vertices)
        {
            //collection has to have at least 1 item
            float highestX = vertices[0].x;
            for (int i = 1; i < vertices.Count; i++)
            {
                highestX = Mathf.Max(highestX, vertices[i].x);
            }
            return highestX;
        }

        public static float GetLowestXComponent(List<Vector2> vertices)
        {
            //collection has to have at least 1 item
            float lowestX = vertices[0].x;
            for (int i = 1; i < vertices.Count; i++)
            {
                lowestX = Mathf.Min(lowestX, vertices[i].x);
            }
            return lowestX;
        }

        public static float GetHighestYComponent(List<Vector2> vertices)
        {
            //collection has to have at least 1 item
            float highestY = vertices[0].y;
            for (int i = 1; i < vertices.Count; i++)
            {
                highestY = Mathf.Max(highestY, vertices[i].y);
            }
            return highestY;
        }

        public static float GetLowestYComponent(List<Vector2> vertices)
        {
            //collection has to have at least 1 item
            float lowestY = vertices[0].y;
            for (int i = 1; i < vertices.Count; i++)
            {
                lowestY = Mathf.Min(lowestY, vertices[i].y);
            }
            return lowestY;
        }

        public static Vector3 GetNearestVertex(Vector3 refPos, List<Vector3> verticesToChooseFrom, int usedSlotsInList)
        {
            //collection has to have at least 1 item
            Vector3 nearestVertex = verticesToChooseFrom[0];
            float nearestDistanceSqr = (refPos - verticesToChooseFrom[0]).sqrMagnitude;
            for (int i = 1; i < usedSlotsInList; i++)
            {
                float currDistanceSqr = (refPos - verticesToChooseFrom[i]).sqrMagnitude;
                if (currDistanceSqr < nearestDistanceSqr)
                {
                    nearestVertex = verticesToChooseFrom[i];
                    nearestDistanceSqr = currDistanceSqr;
                }
            }
            return nearestVertex;
        }

        public static Vector2 GetNearestVertex(Vector2 refPos, List<Vector2> verticesToChooseFrom, int usedSlotsInList)
        {
            //collection has to have at least 1 item
            Vector2 nearestVertex = verticesToChooseFrom[0];
            float nearestDistanceSqr = (refPos - verticesToChooseFrom[0]).sqrMagnitude;
            for (int i = 1; i < usedSlotsInList; i++)
            {
                float currDistanceSqr = (refPos - verticesToChooseFrom[i]).sqrMagnitude;
                if (currDistanceSqr < nearestDistanceSqr)
                {
                    nearestVertex = verticesToChooseFrom[i];
                    nearestDistanceSqr = currDistanceSqr;
                }
            }
            return nearestVertex;
        }

        public static bool IsVectorApproxUniform(Vector3 vector)
        {
            if (CheckIfValueLiesInsideDistanceNearAnotherValue(vector.x, vector.y, 0.001f))
            {
                if (CheckIfValueLiesInsideDistanceNearAnotherValue(vector.x, vector.z, 0.001f))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool IsVectorApproxUniform(Vector2 vector)
        {
            return CheckIfValueLiesInsideDistanceNearAnotherValue(vector.x, vector.y, 0.001f);
        }

        public static bool ContainsNegativeComponents(Vector3 vector)
        {
            return (vector.x < 0.0f || vector.y < 0.0f || vector.z < 0.0f);
        }

        public static bool ContainsNegativeComponents(Vector2 vector)
        {
            return (vector.x < 0.0f || vector.y < 0.0f);
        }

        public static bool ContainsZeroComponents(Vector3 vector)
        {
            return (ApproximatelyZero(vector.x) || ApproximatelyZero(vector.y) || ApproximatelyZero(vector.z));
        }

        public static bool ContainsZeroComponentsInXorY(Vector3 vector)
        {
            return (ApproximatelyZero(vector.x) || ApproximatelyZero(vector.y));
        }

        public static float Loop_floatIntoSpanFrom_0_to_1(float floatValue_thatMayBeOutside_spanFrom0to1)
        {
            if (floatValue_thatMayBeOutside_spanFrom0to1 > 0.0f)
            {
                return (floatValue_thatMayBeOutside_spanFrom0to1 - (float)Mathf.FloorToInt(floatValue_thatMayBeOutside_spanFrom0to1));
            }
            else
            {
                return ((floatValue_thatMayBeOutside_spanFrom0to1 - (float)Mathf.CeilToInt(floatValue_thatMayBeOutside_spanFrom0to1)) + 1.0f);
            }
        }

        public static float Loop_floatIntoSpanFrom_m1_to_p1(float floatValue_thatMayBeOutside_spanFrom_m10_to_p1)
        {
            if (floatValue_thatMayBeOutside_spanFrom_m10_to_p1 > 0.0f)
            {
                return (floatValue_thatMayBeOutside_spanFrom_m10_to_p1 - (float)Mathf.FloorToInt(floatValue_thatMayBeOutside_spanFrom_m10_to_p1));
            }
            else
            {
                return (floatValue_thatMayBeOutside_spanFrom_m10_to_p1 - (float)Mathf.CeilToInt(floatValue_thatMayBeOutside_spanFrom_m10_to_p1));
            }
        }

        public static float Loop_floatIntoSpanFrom_0_to_x(float floatValue_thatMayBeOutside_spanFrom_0_to_x, float xThreshold)
        {
            float floatStartValue_normalized = floatValue_thatMayBeOutside_spanFrom_0_to_x / xThreshold;
            float loopedTo_0_to_1 = Loop_floatIntoSpanFrom_0_to_1(floatStartValue_normalized);
            return (loopedTo_0_to_1 * xThreshold);
        }

        public static float Loop_floatIntoSpanFrom_mX_to_pX(float floatValue_thatMayBeOutside_spanFrom_mX_to_pX, float xThreshold)
        {
            float floatStartValue_normalized = floatValue_thatMayBeOutside_spanFrom_mX_to_pX / xThreshold;
            float loopedTo_m1_to_p1 = Loop_floatIntoSpanFrom_m1_to_p1(floatStartValue_normalized);
            return (loopedTo_m1_to_p1 * xThreshold);
        }

        public static int LoopOvershootingIndexIntoCollectionSize(int intValueToLoop, int collectionCount)
        {
            //-> works only for values not farer than one loopspan away
            if (intValueToLoop < 0)
            {
                return (intValueToLoop + collectionCount);
            }
            else
            {
                if (intValueToLoop < collectionCount)
                {
                    return intValueToLoop;
                }
                else
                {
                    return (intValueToLoop - collectionCount);
                }
            }
        }

        public static float Get_jumpFlyCurve_withPlateau(float given_x, float startOfPlateau, float endOfPlateau)
        {
            if (given_x < startOfPlateau)
            {
                return Get_2degParabolicFlateningRise(given_x / startOfPlateau);
            }
            else
            {
                if (given_x > endOfPlateau)
                {
                    float lengthOfEndSegment = 1.0f - endOfPlateau;
                    return Get_2degParabolicSteepeningDecay((given_x - endOfPlateau) / lengthOfEndSegment);
                }
                else
                {
                    return 1.0f;
                }
            }
        }

        public static float Get_2degParabolicSteepeningRise(float given_x)
        {
            return (given_x * given_x);
        }

        public static float Get_3degParabolicSteepeningRise(float given_x)
        {
            return (given_x * given_x * given_x);
        }

        public static float Get_4degParabolicSteepeningRise(float given_x)
        {
            return (given_x * given_x * given_x * given_x);
        }

        public static float Get_2degParabolicFlateningRise(float given_x)
        {
            float x_minus1 = (given_x - 1.0f);
            return (-x_minus1 * x_minus1 + 1.0f);
        }

        public static float Get_2degParabolicFlateningRise_flatRightOfOne(float given_x)
        {
            if (given_x >= 1.0f)
            {
                return 1.0f;
            }
            else
            {
                return Get_2degParabolicFlateningRise(given_x);
            }
        }

        public static float Get_3degParabolicFlateningRise(float given_x)
        {
            float x_minus1 = (given_x - 1.0f);
            return (x_minus1 * x_minus1 * x_minus1 + 1.0f);
        }

        public static float Get_4degParabolicFlateningRise(float given_x)
        {
            float x_minus1 = (given_x - 1.0f);
            return (-x_minus1 * x_minus1 * x_minus1 * x_minus1 + 1.0f);
        }

        public static float Get_2degParabolicSteepeningDecay(float given_x)
        {
            return (-(given_x * given_x) + 1.0f);
        }

        public static float GetDecimalOrderOfMagnitudeAtLowerEnd(float value_forWhichToGetTheDecimalOrderOnTheLowerSide, out float inverseOfReturnValue, out bool calculationWasSuccesful)
        {
            calculationWasSuccesful = true;
            if (value_forWhichToGetTheDecimalOrderOnTheLowerSide >= 1.0f)
            {
                if (value_forWhichToGetTheDecimalOrderOnTheLowerSide < 10.0f)
                {
                    inverseOfReturnValue = 1.0f;
                    return 1.0f;
                }
                else
                {
                    if (value_forWhichToGetTheDecimalOrderOnTheLowerSide < 100.0f)
                    {
                        inverseOfReturnValue = 0.1f;
                        return 10.0f;
                    }
                    else
                    {
                        if (value_forWhichToGetTheDecimalOrderOnTheLowerSide < 1000.0f)
                        {
                            inverseOfReturnValue = 0.01f;
                            return 100.0f;
                        }
                        else
                        {
                            if (value_forWhichToGetTheDecimalOrderOnTheLowerSide < 10000.0f)
                            {
                                inverseOfReturnValue = 0.001f;
                                return 1000.0f;
                            }
                            else
                            {
                                if (value_forWhichToGetTheDecimalOrderOnTheLowerSide < 100000.0f)
                                {
                                    inverseOfReturnValue = 0.0001f;
                                    return 10000.0f;
                                }
                                else
                                {
                                    if (value_forWhichToGetTheDecimalOrderOnTheLowerSide < 1000000.0f)
                                    {
                                        inverseOfReturnValue = 0.00001f;
                                        return 100000.0f;
                                    }
                                    else
                                    {
                                        if (value_forWhichToGetTheDecimalOrderOnTheLowerSide < 10000000.0f)
                                        {
                                            inverseOfReturnValue = 0.000001f;
                                            return 1000000.0f;
                                        }
                                        else
                                        {
                                            if (value_forWhichToGetTheDecimalOrderOnTheLowerSide < 100000000.0f)
                                            {
                                                inverseOfReturnValue = 0.0000001f;
                                                return 10000000.0f;
                                            }
                                            else
                                            {
                                                if (value_forWhichToGetTheDecimalOrderOnTheLowerSide < 1000000000.0f)
                                                {
                                                    inverseOfReturnValue = 0.00000001f;
                                                    return 100000000.0f;
                                                }
                                                else
                                                {
                                                    if (value_forWhichToGetTheDecimalOrderOnTheLowerSide < 10000000000.0f)
                                                    {
                                                        inverseOfReturnValue = 0.000000001f;
                                                        return 1000000000.0f;
                                                    }
                                                    else
                                                    {
                                                        if (value_forWhichToGetTheDecimalOrderOnTheLowerSide < 100000000000.0f)
                                                        {
                                                            inverseOfReturnValue = 0.0000000001f;
                                                            return 10000000000.0f;
                                                        }
                                                        else
                                                        {
                                                            if (value_forWhichToGetTheDecimalOrderOnTheLowerSide < 1000000000000.0f)
                                                            {
                                                                inverseOfReturnValue = 0.00000000001f;
                                                                return 100000000000.0f;
                                                            }
                                                            else
                                                            {
                                                                float decimalOrderOfMagnitudeAtLowerEnd_viaLog10 = GetDecimalOrderOfMagnitudeAtLowerEnd_viaLog10(value_forWhichToGetTheDecimalOrderOnTheLowerSide);
                                                                inverseOfReturnValue = 1.0f / decimalOrderOfMagnitudeAtLowerEnd_viaLog10;
                                                                return decimalOrderOfMagnitudeAtLowerEnd_viaLog10;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                //-> value_forWhichToGetTheDecimalOrderOnTheLowerSide < 1.0f
                if (value_forWhichToGetTheDecimalOrderOnTheLowerSide >= 0.1f)
                {
                    inverseOfReturnValue = 10.0f;
                    return 0.1f;
                }
                else
                {
                    if (value_forWhichToGetTheDecimalOrderOnTheLowerSide >= 0.01f)
                    {
                        inverseOfReturnValue = 100.0f;
                        return 0.01f;
                    }
                    else
                    {
                        if (value_forWhichToGetTheDecimalOrderOnTheLowerSide >= 0.001f)
                        {
                            inverseOfReturnValue = 1000.0f;
                            return 0.001f;
                        }
                        else
                        {
                            if (value_forWhichToGetTheDecimalOrderOnTheLowerSide >= 0.0001f)
                            {
                                inverseOfReturnValue = 10000.0f;
                                return 0.0001f;
                            }
                            else
                            {
                                if (value_forWhichToGetTheDecimalOrderOnTheLowerSide >= 0.00001f)
                                {
                                    inverseOfReturnValue = 100000.0f;
                                    return 0.00001f;
                                }
                                else
                                {
                                    if (value_forWhichToGetTheDecimalOrderOnTheLowerSide >= 0.000001f)
                                    {
                                        inverseOfReturnValue = 1000000.0f;
                                        return 0.000001f;
                                    }
                                    else
                                    {
                                        if (value_forWhichToGetTheDecimalOrderOnTheLowerSide >= 0.0000001f)
                                        {
                                            inverseOfReturnValue = 10000000.0f;
                                            return 0.0000001f;
                                        }
                                        else
                                        {
                                            if (value_forWhichToGetTheDecimalOrderOnTheLowerSide >= 0.00000001f)
                                            {
                                                inverseOfReturnValue = 100000000.0f;
                                                return 0.00000001f;
                                            }
                                            else
                                            {
                                                if (value_forWhichToGetTheDecimalOrderOnTheLowerSide >= 0.000000001f)
                                                {
                                                    inverseOfReturnValue = 1000000000.0f;
                                                    return 0.000000001f;
                                                }
                                                else
                                                {
                                                    if (value_forWhichToGetTheDecimalOrderOnTheLowerSide >= 0.0000000001f)
                                                    {
                                                        inverseOfReturnValue = 10000000000.0f;
                                                        return 0.0000000001f;
                                                    }
                                                    else
                                                    {
                                                        if (value_forWhichToGetTheDecimalOrderOnTheLowerSide >= 0.00000000001f)
                                                        {
                                                            inverseOfReturnValue = 100000000000.0f;
                                                            return 0.00000000001f;
                                                        }
                                                        else
                                                        {
                                                            if (value_forWhichToGetTheDecimalOrderOnTheLowerSide >= 0.000000000001f)
                                                            {
                                                                inverseOfReturnValue = 1000000000000.0f;
                                                                return 0.000000000001f;
                                                            }
                                                            else
                                                            {
                                                                calculationWasSuccesful = false;
                                                                inverseOfReturnValue = 1.0f;
                                                                return 1.0f;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public static float GetDecimalOrderOfMagnitudeAtLowerEnd_viaLog10(float value_forWhichToGetTheDecimalOrderOnTheLowerSide)
        {
            float log10ofValue = Mathf.Log10(value_forWhichToGetTheDecimalOrderOnTheLowerSide);
            float log10ofValue_floorInt = Mathf.Floor(log10ofValue);
            return Mathf.Pow(10.0f, log10ofValue_floorInt);
        }

        public static float AcuteAngle_0to90(Vector3 from, Vector3 to)
        {
            float angleDeg_0to180 = Vector3.Angle(from, to);
            if (angleDeg_0to180 > 90.0f)
            {
                return (180.0f - angleDeg_0to180);
            }
            else
            {
                return angleDeg_0to180;
            }
        }

        public static float GetCenterBetweenTwoFloats(float float1, float float2)
        {
            return 0.5f * (float1 + float2);
        }

        public static Vector3 GetCenterBetweenTwoPoints(Vector3 point1, Vector3 point2)
        {
            return 0.5f * (point1 + point2);
        }

        public static float GetAverageBoxExtent(Vector3 boxDimensions)
        {
            return 0.3333f * (boxDimensions.x + boxDimensions.y + boxDimensions.z);
        }

        public static float GetAverageBoxExtent(Vector2 boxDimensions)
        {
            return 0.5f * (boxDimensions.x + boxDimensions.y);
        }

    }

}
