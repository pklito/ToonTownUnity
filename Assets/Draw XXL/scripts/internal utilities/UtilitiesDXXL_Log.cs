namespace DrawXXL
{
    using UnityEngine;

    public class UtilitiesDXXL_Log
    {
        public static string Get_vectorComponentsAsString(Vector3 vector3_toGetComponentsFrom, bool andAppendVectorLength = false)
        {
            if (andAppendVectorLength)
            {
                return ("( " + vector3_toGetComponentsFrom.x + " , " + vector3_toGetComponentsFrom.y + " , " + vector3_toGetComponentsFrom.z + " ) (length = " + vector3_toGetComponentsFrom.magnitude + ")");
            }
            else
            {
                return ("( " + vector3_toGetComponentsFrom.x + " , " + vector3_toGetComponentsFrom.y + " , " + vector3_toGetComponentsFrom.z + " )");
            }
        }

        public static string Get_vectorComponentsAsString(Vector2 vector2_toGetComponentsFrom, bool andAppendVectorLength = false)
        {
            if (andAppendVectorLength)
            {
                return ("( " + vector2_toGetComponentsFrom.x + " , " + vector2_toGetComponentsFrom.y + " ) (length = " + vector2_toGetComponentsFrom.magnitude + ")");
            }
            else
            {
                return ("( " + vector2_toGetComponentsFrom.x + " , " + vector2_toGetComponentsFrom.y + " )");
            }
        }

        public static string Get_quaternionComponentsAsString(Quaternion quaternion_toGetComponentsFrom, bool andAppendMagnitude = false)
        {
            if (andAppendMagnitude)
            {
                return ("( " + quaternion_toGetComponentsFrom.x + " , " + quaternion_toGetComponentsFrom.y + " , " + quaternion_toGetComponentsFrom.z + " , " + quaternion_toGetComponentsFrom.w + " ) (magnitude = " + UtilitiesDXXL_Math.GetQuaternionMagnitude(quaternion_toGetComponentsFrom) + ")");
            }
            else
            {
                return ("( " + quaternion_toGetComponentsFrom.x + " , " + quaternion_toGetComponentsFrom.y + " , " + quaternion_toGetComponentsFrom.z + " , " + quaternion_toGetComponentsFrom.w + " )");
            }
        }

        public static bool ErrorLogForInvalidFloats(float floatToCheck, string floatName)
        {
            if (UtilitiesDXXL_Math.FloatIsInvalid(floatToCheck))
            {
                UnityEngine.Debug.LogError("The float value '" + floatName + "' is not a valid float, but " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(floatToCheck) + ". Draw operation is not executed.");
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ErrorLogForInvalidVectors(Vector3 vectorToCheck, string vectorName)
        {
            if (UtilitiesDXXL_Math.FloatIsInvalid(vectorToCheck.x) || UtilitiesDXXL_Math.FloatIsInvalid(vectorToCheck.y) || UtilitiesDXXL_Math.FloatIsInvalid(vectorToCheck.z))
            {
                UnityEngine.Debug.LogError("The Vector3 '" + vectorName + "' contains invalid float components: ( <b>x</b> is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(vectorToCheck.x) + ", <b>y</b> is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(vectorToCheck.y) + ", <b>z</b> is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(vectorToCheck.z) + "). Draw operation is not executed.");
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ErrorLogForInvalidVectors(Vector2 vectorToCheck, string vectorName)
        {
            if (UtilitiesDXXL_Math.FloatIsInvalid(vectorToCheck.x) || UtilitiesDXXL_Math.FloatIsInvalid(vectorToCheck.y))
            {
                UnityEngine.Debug.LogError("The Vector2 '" + vectorName + "' contains invalid float components: ( <b>x</b> is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(vectorToCheck.x) + ", <b>y</b> is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(vectorToCheck.y) + "). Draw operation is not executed.");
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ErrorLogForNullUnityObjects(UnityEngine.Object objectToCheck, string objectName)
        {
            if (objectToCheck == null)
            {
                UnityEngine.Debug.LogError("The Object '" + objectName + "' is 'null'. Draw operation is not executed.");
                return true;
            }
            else
            {
                return false;
            }
        }

        public static bool ErrorLogForNullSystemObjects(System.Object objectToCheck, string objectName)
        {
            if (objectToCheck == null)
            {
                UnityEngine.Debug.LogError("The Object '" + objectName + "' is 'null'. Draw operation is not executed.");
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void PrintErrorCode(string errorCodeNumber)
        {
            UnityEngine.Debug.LogError("Draw XXL error code " + errorCodeNumber + ". You can help improving the code by submitting this code line including the stack trace and the piece of code that triggered this error. e-mail: drawxxl@symphonygames.net");
        }

    }

}
