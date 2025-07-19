namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;
    
    public class UtilitiesDXXL_ChartDrawing
    {
        public delegate float FlexibleGetYValueFromCollection<T>(T yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject);


        /// Lists:
        /// -------
        /// -------
        /// -------
        public static FlexibleGetYValueFromCollection<List<float>> GetYValueFrom_listOfFloats_preAllocated = GetYValueFrom_listOfFloats;
        static float GetYValueFrom_listOfFloats(List<float> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return yValues[i_slotWhereToObtainTheValue];
        }

        public static FlexibleGetYValueFromCollection<List<int>> GetYValueFrom_listOfInts_preAllocated = GetYValueFrom_listOfInts;
        static float GetYValueFrom_listOfInts(List<int> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return (float)yValues[i_slotWhereToObtainTheValue];
        }

        public static FlexibleGetYValueFromCollection<List<Vector2>> GetYValueFrom_listOfVector2s_xComponent_preAllocated = GetYValueFrom_listOfVector2s_xComponent;
        static float GetYValueFrom_listOfVector2s_xComponent(List<Vector2> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return yValues[i_slotWhereToObtainTheValue].x;
        }
        public static FlexibleGetYValueFromCollection<List<Vector2>> GetYValueFrom_listOfVector2s_yComponent_preAllocated = GetYValueFrom_listOfVector2s_yComponent;
        static float GetYValueFrom_listOfVector2s_yComponent(List<Vector2> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return yValues[i_slotWhereToObtainTheValue].y;
        }

        public static FlexibleGetYValueFromCollection<List<Vector3>> GetYValueFrom_listOfVector3s_xComponent_preAllocated = GetYValueFrom_listOfVector3s_xComponent;
        static float GetYValueFrom_listOfVector3s_xComponent(List<Vector3> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return yValues[i_slotWhereToObtainTheValue].x;
        }
        public static FlexibleGetYValueFromCollection<List<Vector3>> GetYValueFrom_listOfVector3s_yComponent_preAllocated = GetYValueFrom_listOfVector3s_yComponent;
        static float GetYValueFrom_listOfVector3s_yComponent(List<Vector3> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return yValues[i_slotWhereToObtainTheValue].y;
        }
        public static FlexibleGetYValueFromCollection<List<Vector3>> GetYValueFrom_listOfVector3s_zComponent_preAllocated = GetYValueFrom_listOfVector3s_zComponent;
        static float GetYValueFrom_listOfVector3s_zComponent(List<Vector3> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return yValues[i_slotWhereToObtainTheValue].z;
        }

        public static FlexibleGetYValueFromCollection<List<Quaternion>> GetYValueFrom_listOfQuaternions_eulerXComponent_preAllocated = GetYValueFrom_listOfQuaternions_eulerXComponent;
        static float GetYValueFrom_listOfQuaternions_eulerXComponent(List<Quaternion> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return yValues[i_slotWhereToObtainTheValue].eulerAngles.x;
        }
        public static FlexibleGetYValueFromCollection<List<Quaternion>> GetYValueFrom_listOfQuaternions_eulerYComponent_preAllocated = GetYValueFrom_listOfQuaternions_eulerYComponent;
        static float GetYValueFrom_listOfQuaternions_eulerYComponent(List<Quaternion> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return yValues[i_slotWhereToObtainTheValue].eulerAngles.y;
        }
        public static FlexibleGetYValueFromCollection<List<Quaternion>> GetYValueFrom_listOfQuaternions_eulerZComponent_preAllocated = GetYValueFrom_listOfQuaternions_eulerZComponent;
        static float GetYValueFrom_listOfQuaternions_eulerZComponent(List<Quaternion> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return yValues[i_slotWhereToObtainTheValue].eulerAngles.z;
        }

        public static FlexibleGetYValueFromCollection<List<bool>> GetYValueFrom_listOfBools_preAllocated = GetYValueFrom_listOfBools;
        static float GetYValueFrom_listOfBools(List<bool> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            if (yValues[i_slotWhereToObtainTheValue] == true)
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }


        public static FlexibleGetYValueFromCollection<List<GameObject>> GetYValueFrom_listOfGameobjects_localPosition_x_preAllocated = GetYValueFrom_listOfGameobjects_localPosition_x;
        static float GetYValueFrom_listOfGameobjects_localPosition_x(List<GameObject> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.localPosition.x;
            }
        }
        public static FlexibleGetYValueFromCollection<List<GameObject>> GetYValueFrom_listOfGameobjects_localPosition_y_preAllocated = GetYValueFrom_listOfGameobjects_localPosition_y;
        static float GetYValueFrom_listOfGameobjects_localPosition_y(List<GameObject> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.localPosition.y;
            }
        }
        public static FlexibleGetYValueFromCollection<List<GameObject>> GetYValueFrom_listOfGameobjects_localPosition_z_preAllocated = GetYValueFrom_listOfGameobjects_localPosition_z;
        static float GetYValueFrom_listOfGameobjects_localPosition_z(List<GameObject> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.localPosition.z;
            }
        }
        public static FlexibleGetYValueFromCollection<List<GameObject>> GetYValueFrom_listOfGameobjects_localEulerAngle_x_preAllocated = GetYValueFrom_listOfGameobjects_localEulerAngle_x;
        static float GetYValueFrom_listOfGameobjects_localEulerAngle_x(List<GameObject> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.localEulerAngles.x;
            }
        }
        public static FlexibleGetYValueFromCollection<List<GameObject>> GetYValueFrom_listOfGameobjects_localEulerAngle_y_preAllocated = GetYValueFrom_listOfGameobjects_localEulerAngle_y;
        static float GetYValueFrom_listOfGameobjects_localEulerAngle_y(List<GameObject> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.localEulerAngles.y;
            }
        }
        public static FlexibleGetYValueFromCollection<List<GameObject>> GetYValueFrom_listOfGameobjects_localEulerAngle_z_preAllocated = GetYValueFrom_listOfGameobjects_localEulerAngle_z;
        static float GetYValueFrom_listOfGameobjects_localEulerAngle_z(List<GameObject> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.localEulerAngles.z;
            }
        }
        public static FlexibleGetYValueFromCollection<List<GameObject>> GetYValueFrom_listOfGameobjects_localScale_x_preAllocated = GetYValueFrom_listOfGameobjects_localScale_x;
        static float GetYValueFrom_listOfGameobjects_localScale_x(List<GameObject> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.localScale.x;
            }
        }
        public static FlexibleGetYValueFromCollection<List<GameObject>> GetYValueFrom_listOfGameobjects_localScale_y_preAllocated = GetYValueFrom_listOfGameobjects_localScale_y;
        static float GetYValueFrom_listOfGameobjects_localScale_y(List<GameObject> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.localScale.y;
            }
        }
        public static FlexibleGetYValueFromCollection<List<GameObject>> GetYValueFrom_listOfGameobjects_localScale_z_preAllocated = GetYValueFrom_listOfGameobjects_localScale_z;
        static float GetYValueFrom_listOfGameobjects_localScale_z(List<GameObject> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.localScale.z;
            }
        }
        public static FlexibleGetYValueFromCollection<List<GameObject>> GetYValueFrom_listOfGameobjects_globalPosition_x_preAllocated = GetYValueFrom_listOfGameobjects_globalPosition_x;
        static float GetYValueFrom_listOfGameobjects_globalPosition_x(List<GameObject> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.position.x;
            }
        }
        public static FlexibleGetYValueFromCollection<List<GameObject>> GetYValueFrom_listOfGameobjects_globalPosition_y_preAllocated = GetYValueFrom_listOfGameobjects_globalPosition_y;
        static float GetYValueFrom_listOfGameobjects_globalPosition_y(List<GameObject> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.position.y;
            }
        }
        public static FlexibleGetYValueFromCollection<List<GameObject>> GetYValueFrom_listOfGameobjects_globalPosition_z_preAllocated = GetYValueFrom_listOfGameobjects_globalPosition_z;
        static float GetYValueFrom_listOfGameobjects_globalPosition_z(List<GameObject> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.position.z;
            }
        }
        public static FlexibleGetYValueFromCollection<List<GameObject>> GetYValueFrom_listOfGameobjects_globalEulerAngle_x_preAllocated = GetYValueFrom_listOfGameobjects_globalEulerAngle_x;
        static float GetYValueFrom_listOfGameobjects_globalEulerAngle_x(List<GameObject> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.eulerAngles.x;
            }
        }
        public static FlexibleGetYValueFromCollection<List<GameObject>> GetYValueFrom_listOfGameobjects_globalEulerAngle_y_preAllocated = GetYValueFrom_listOfGameobjects_globalEulerAngle_y;
        static float GetYValueFrom_listOfGameobjects_globalEulerAngle_y(List<GameObject> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.eulerAngles.y;
            }
        }
        public static FlexibleGetYValueFromCollection<List<GameObject>> GetYValueFrom_listOfGameobjects_globalEulerAngle_z_preAllocated = GetYValueFrom_listOfGameobjects_globalEulerAngle_z;
        static float GetYValueFrom_listOfGameobjects_globalEulerAngle_z(List<GameObject> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.eulerAngles.z;
            }
        }
        public static FlexibleGetYValueFromCollection<List<GameObject>> GetYValueFrom_listOfGameobjects_lossyScale_x_preAllocated = GetYValueFrom_listOfGameobjects_lossyScale_x;
        static float GetYValueFrom_listOfGameobjects_lossyScale_x(List<GameObject> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.lossyScale.x;
            }
        }
        public static FlexibleGetYValueFromCollection<List<GameObject>> GetYValueFrom_listOfGameobjects_lossyScale_y_preAllocated = GetYValueFrom_listOfGameobjects_lossyScale_y;
        static float GetYValueFrom_listOfGameobjects_lossyScale_y(List<GameObject> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.lossyScale.y;
            }
        }
        public static FlexibleGetYValueFromCollection<List<GameObject>> GetYValueFrom_listOfGameobjects_lossyScale_z_preAllocated = GetYValueFrom_listOfGameobjects_lossyScale_z;
        static float GetYValueFrom_listOfGameobjects_lossyScale_z(List<GameObject> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.lossyScale.z;
            }
        }








        public static FlexibleGetYValueFromCollection<List<Transform>> GetYValueFrom_listOfTransforms_localPosition_x_preAllocated = GetYValueFrom_listOfTransforms_localPosition_x;
        static float GetYValueFrom_listOfTransforms_localPosition_x(List<Transform> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].localPosition.x;
            }
        }
        public static FlexibleGetYValueFromCollection<List<Transform>> GetYValueFrom_listOfTransforms_localPosition_y_preAllocated = GetYValueFrom_listOfTransforms_localPosition_y;
        static float GetYValueFrom_listOfTransforms_localPosition_y(List<Transform> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].localPosition.y;
            }
        }
        public static FlexibleGetYValueFromCollection<List<Transform>> GetYValueFrom_listOfTransforms_localPosition_z_preAllocated = GetYValueFrom_listOfTransforms_localPosition_z;
        static float GetYValueFrom_listOfTransforms_localPosition_z(List<Transform> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].localPosition.z;
            }
        }
        public static FlexibleGetYValueFromCollection<List<Transform>> GetYValueFrom_listOfTransforms_localEulerAngle_x_preAllocated = GetYValueFrom_listOfTransforms_localEulerAngle_x;
        static float GetYValueFrom_listOfTransforms_localEulerAngle_x(List<Transform> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].localEulerAngles.x;
            }
        }
        public static FlexibleGetYValueFromCollection<List<Transform>> GetYValueFrom_listOfTransforms_localEulerAngle_y_preAllocated = GetYValueFrom_listOfTransforms_localEulerAngle_y;
        static float GetYValueFrom_listOfTransforms_localEulerAngle_y(List<Transform> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].localEulerAngles.y;
            }
        }
        public static FlexibleGetYValueFromCollection<List<Transform>> GetYValueFrom_listOfTransforms_localEulerAngle_z_preAllocated = GetYValueFrom_listOfTransforms_localEulerAngle_z;
        static float GetYValueFrom_listOfTransforms_localEulerAngle_z(List<Transform> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].localEulerAngles.z;
            }
        }
        public static FlexibleGetYValueFromCollection<List<Transform>> GetYValueFrom_listOfTransforms_localScale_x_preAllocated = GetYValueFrom_listOfTransforms_localScale_x;
        static float GetYValueFrom_listOfTransforms_localScale_x(List<Transform> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].localScale.x;
            }
        }
        public static FlexibleGetYValueFromCollection<List<Transform>> GetYValueFrom_listOfTransforms_localScale_y_preAllocated = GetYValueFrom_listOfTransforms_localScale_y;
        static float GetYValueFrom_listOfTransforms_localScale_y(List<Transform> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].localScale.y;
            }
        }
        public static FlexibleGetYValueFromCollection<List<Transform>> GetYValueFrom_listOfTransforms_localScale_z_preAllocated = GetYValueFrom_listOfTransforms_localScale_z;
        static float GetYValueFrom_listOfTransforms_localScale_z(List<Transform> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].localScale.z;
            }
        }
        public static FlexibleGetYValueFromCollection<List<Transform>> GetYValueFrom_listOfTransforms_globalPosition_x_preAllocated = GetYValueFrom_listOfTransforms_globalPosition_x;
        static float GetYValueFrom_listOfTransforms_globalPosition_x(List<Transform> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].position.x;
            }
        }
        public static FlexibleGetYValueFromCollection<List<Transform>> GetYValueFrom_listOfTransforms_globalPosition_y_preAllocated = GetYValueFrom_listOfTransforms_globalPosition_y;
        static float GetYValueFrom_listOfTransforms_globalPosition_y(List<Transform> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].position.y;
            }
        }
        public static FlexibleGetYValueFromCollection<List<Transform>> GetYValueFrom_listOfTransforms_globalPosition_z_preAllocated = GetYValueFrom_listOfTransforms_globalPosition_z;
        static float GetYValueFrom_listOfTransforms_globalPosition_z(List<Transform> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].position.z;
            }
        }
        public static FlexibleGetYValueFromCollection<List<Transform>> GetYValueFrom_listOfTransforms_globalEulerAngle_x_preAllocated = GetYValueFrom_listOfTransforms_globalEulerAngle_x;
        static float GetYValueFrom_listOfTransforms_globalEulerAngle_x(List<Transform> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].eulerAngles.x;
            }
        }
        public static FlexibleGetYValueFromCollection<List<Transform>> GetYValueFrom_listOfTransforms_globalEulerAngle_y_preAllocated = GetYValueFrom_listOfTransforms_globalEulerAngle_y;
        static float GetYValueFrom_listOfTransforms_globalEulerAngle_y(List<Transform> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].eulerAngles.y;
            }
        }
        public static FlexibleGetYValueFromCollection<List<Transform>> GetYValueFrom_listOfTransforms_globalEulerAngle_z_preAllocated = GetYValueFrom_listOfTransforms_globalEulerAngle_z;
        static float GetYValueFrom_listOfTransforms_globalEulerAngle_z(List<Transform> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].eulerAngles.z;
            }
        }

        public static FlexibleGetYValueFromCollection<List<Transform>> GetYValueFrom_listOfTransforms_lossyScale_x_preAllocated = GetYValueFrom_listOfTransforms_lossyScale_x;
        static float GetYValueFrom_listOfTransforms_lossyScale_x(List<Transform> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].lossyScale.x;
            }
        }
        public static FlexibleGetYValueFromCollection<List<Transform>> GetYValueFrom_listOfTransforms_lossyScale_y_preAllocated = GetYValueFrom_listOfTransforms_lossyScale_y;
        static float GetYValueFrom_listOfTransforms_lossyScale_y(List<Transform> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].lossyScale.y;
            }
        }
        public static FlexibleGetYValueFromCollection<List<Transform>> GetYValueFrom_listOfTransforms_lossyScale_z_preAllocated = GetYValueFrom_listOfTransforms_lossyScale_z;
        static float GetYValueFrom_listOfTransforms_lossyScale_z(List<Transform> yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].lossyScale.z;
            }
        }


        /// Arrays:
        /// -------
        /// -------
        /// -------
        public static FlexibleGetYValueFromCollection<float[]> GetYValueFrom_arrayOfFloats_preAllocated = GetYValueFrom_arrayOfFloats;
        static float GetYValueFrom_arrayOfFloats(float[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return yValues[i_slotWhereToObtainTheValue];
        }

        public static FlexibleGetYValueFromCollection<int[]> GetYValueFrom_arrayOfInts_preAllocated = GetYValueFrom_arrayOfInts;
        static float GetYValueFrom_arrayOfInts(int[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return (float)yValues[i_slotWhereToObtainTheValue];
        }

        public static FlexibleGetYValueFromCollection<Vector2[]> GetYValueFrom_arrayOfVector2s_xComponent_preAllocated = GetYValueFrom_arrayOfVector2s_xComponent;
        static float GetYValueFrom_arrayOfVector2s_xComponent(Vector2[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return yValues[i_slotWhereToObtainTheValue].x;
        }
        public static FlexibleGetYValueFromCollection<Vector2[]> GetYValueFrom_arrayOfVector2s_yComponent_preAllocated = GetYValueFrom_arrayOfVector2s_yComponent;
        static float GetYValueFrom_arrayOfVector2s_yComponent(Vector2[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return yValues[i_slotWhereToObtainTheValue].y;
        }

        public static FlexibleGetYValueFromCollection<Vector3[]> GetYValueFrom_arrayOfVector3s_xComponent_preAllocated = GetYValueFrom_arrayOfVector3s_xComponent;
        static float GetYValueFrom_arrayOfVector3s_xComponent(Vector3[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return yValues[i_slotWhereToObtainTheValue].x;
        }
        public static FlexibleGetYValueFromCollection<Vector3[]> GetYValueFrom_arrayOfVector3s_yComponent_preAllocated = GetYValueFrom_arrayOfVector3s_yComponent;
        static float GetYValueFrom_arrayOfVector3s_yComponent(Vector3[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return yValues[i_slotWhereToObtainTheValue].y;
        }
        public static FlexibleGetYValueFromCollection<Vector3[]> GetYValueFrom_arrayOfVector3s_zComponent_preAllocated = GetYValueFrom_arrayOfVector3s_zComponent;
        static float GetYValueFrom_arrayOfVector3s_zComponent(Vector3[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return yValues[i_slotWhereToObtainTheValue].z;
        }

        public static FlexibleGetYValueFromCollection<Quaternion[]> GetYValueFrom_arrayOfQuaternions_eulerXComponent_preAllocated = GetYValueFrom_arrayOfQuaternions_eulerXComponent;
        static float GetYValueFrom_arrayOfQuaternions_eulerXComponent(Quaternion[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return yValues[i_slotWhereToObtainTheValue].eulerAngles.x;
        }
        public static FlexibleGetYValueFromCollection<Quaternion[]> GetYValueFrom_arrayOfQuaternions_eulerYComponent_preAllocated = GetYValueFrom_arrayOfQuaternions_eulerYComponent;
        static float GetYValueFrom_arrayOfQuaternions_eulerYComponent(Quaternion[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return yValues[i_slotWhereToObtainTheValue].eulerAngles.y;
        }
        public static FlexibleGetYValueFromCollection<Quaternion[]> GetYValueFrom_arrayOfQuaternions_eulerZComponent_preAllocated = GetYValueFrom_arrayOfQuaternions_eulerZComponent;
        static float GetYValueFrom_arrayOfQuaternions_eulerZComponent(Quaternion[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            return yValues[i_slotWhereToObtainTheValue].eulerAngles.z;
        }

        public static FlexibleGetYValueFromCollection<bool[]> GetYValueFrom_arrayOfBools_preAllocated = GetYValueFrom_arrayOfBools;
        static float GetYValueFrom_arrayOfBools(bool[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            gameobjectThatIsTheSourceOfTheValues = null;
            lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = null;
            if (yValues[i_slotWhereToObtainTheValue] == true)
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }


        public static FlexibleGetYValueFromCollection<GameObject[]> GetYValueFrom_arrayOfGameobjects_localPosition_x_preAllocated = GetYValueFrom_arrayOfGameobjects_localPosition_x;
        static float GetYValueFrom_arrayOfGameobjects_localPosition_x(GameObject[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.localPosition.x;
            }
        }
        public static FlexibleGetYValueFromCollection<GameObject[]> GetYValueFrom_arrayOfGameobjects_localPosition_y_preAllocated = GetYValueFrom_arrayOfGameobjects_localPosition_y;
        static float GetYValueFrom_arrayOfGameobjects_localPosition_y(GameObject[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.localPosition.y;
            }
        }
        public static FlexibleGetYValueFromCollection<GameObject[]> GetYValueFrom_arrayOfGameobjects_localPosition_z_preAllocated = GetYValueFrom_arrayOfGameobjects_localPosition_z;
        static float GetYValueFrom_arrayOfGameobjects_localPosition_z(GameObject[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.localPosition.z;
            }
        }
        public static FlexibleGetYValueFromCollection<GameObject[]> GetYValueFrom_arrayOfGameobjects_localEulerAngle_x_preAllocated = GetYValueFrom_arrayOfGameobjects_localEulerAngle_x;
        static float GetYValueFrom_arrayOfGameobjects_localEulerAngle_x(GameObject[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.localEulerAngles.x;
            }
        }
        public static FlexibleGetYValueFromCollection<GameObject[]> GetYValueFrom_arrayOfGameobjects_localEulerAngle_y_preAllocated = GetYValueFrom_arrayOfGameobjects_localEulerAngle_y;
        static float GetYValueFrom_arrayOfGameobjects_localEulerAngle_y(GameObject[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.localEulerAngles.y;
            }
        }
        public static FlexibleGetYValueFromCollection<GameObject[]> GetYValueFrom_arrayOfGameobjects_localEulerAngle_z_preAllocated = GetYValueFrom_arrayOfGameobjects_localEulerAngle_z;
        static float GetYValueFrom_arrayOfGameobjects_localEulerAngle_z(GameObject[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.localEulerAngles.z;
            }
        }
        public static FlexibleGetYValueFromCollection<GameObject[]> GetYValueFrom_arrayOfGameobjects_localScale_x_preAllocated = GetYValueFrom_arrayOfGameobjects_localScale_x;
        static float GetYValueFrom_arrayOfGameobjects_localScale_x(GameObject[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.localScale.x;
            }
        }
        public static FlexibleGetYValueFromCollection<GameObject[]> GetYValueFrom_arrayOfGameobjects_localScale_y_preAllocated = GetYValueFrom_arrayOfGameobjects_localScale_y;
        static float GetYValueFrom_arrayOfGameobjects_localScale_y(GameObject[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.localScale.y;
            }
        }
        public static FlexibleGetYValueFromCollection<GameObject[]> GetYValueFrom_arrayOfGameobjects_localScale_z_preAllocated = GetYValueFrom_arrayOfGameobjects_localScale_z;
        static float GetYValueFrom_arrayOfGameobjects_localScale_z(GameObject[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.localScale.z;
            }
        }
        public static FlexibleGetYValueFromCollection<GameObject[]> GetYValueFrom_arrayOfGameobjects_globalPosition_x_preAllocated = GetYValueFrom_arrayOfGameobjects_globalPosition_x;
        static float GetYValueFrom_arrayOfGameobjects_globalPosition_x(GameObject[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.position.x;
            }
        }
        public static FlexibleGetYValueFromCollection<GameObject[]> GetYValueFrom_arrayOfGameobjects_globalPosition_y_preAllocated = GetYValueFrom_arrayOfGameobjects_globalPosition_y;
        static float GetYValueFrom_arrayOfGameobjects_globalPosition_y(GameObject[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.position.y;
            }
        }
        public static FlexibleGetYValueFromCollection<GameObject[]> GetYValueFrom_arrayOfGameobjects_globalPosition_z_preAllocated = GetYValueFrom_arrayOfGameobjects_globalPosition_z;
        static float GetYValueFrom_arrayOfGameobjects_globalPosition_z(GameObject[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.position.z;
            }
        }
        public static FlexibleGetYValueFromCollection<GameObject[]> GetYValueFrom_arrayOfGameobjects_globalEulerAngle_x_preAllocated = GetYValueFrom_arrayOfGameobjects_globalEulerAngle_x;
        static float GetYValueFrom_arrayOfGameobjects_globalEulerAngle_x(GameObject[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.eulerAngles.x;
            }
        }
        public static FlexibleGetYValueFromCollection<GameObject[]> GetYValueFrom_arrayOfGameobjects_globalEulerAngle_y_preAllocated = GetYValueFrom_arrayOfGameobjects_globalEulerAngle_y;
        static float GetYValueFrom_arrayOfGameobjects_globalEulerAngle_y(GameObject[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.eulerAngles.y;
            }
        }
        public static FlexibleGetYValueFromCollection<GameObject[]> GetYValueFrom_arrayOfGameobjects_globalEulerAngle_z_preAllocated = GetYValueFrom_arrayOfGameobjects_globalEulerAngle_z;
        static float GetYValueFrom_arrayOfGameobjects_globalEulerAngle_z(GameObject[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.eulerAngles.z;
            }
        }
        public static FlexibleGetYValueFromCollection<GameObject[]> GetYValueFrom_arrayOfGameobjects_lossyScale_x_preAllocated = GetYValueFrom_arrayOfGameobjects_lossyScale_x;
        static float GetYValueFrom_arrayOfGameobjects_lossyScale_x(GameObject[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.lossyScale.x;
            }
        }
        public static FlexibleGetYValueFromCollection<GameObject[]> GetYValueFrom_arrayOfGameobjects_lossyScale_y_preAllocated = GetYValueFrom_arrayOfGameobjects_lossyScale_y;
        static float GetYValueFrom_arrayOfGameobjects_lossyScale_y(GameObject[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.lossyScale.y;
            }
        }
        public static FlexibleGetYValueFromCollection<GameObject[]> GetYValueFrom_arrayOfGameobjects_lossyScale_z_preAllocated = GetYValueFrom_arrayOfGameobjects_lossyScale_z;
        static float GetYValueFrom_arrayOfGameobjects_lossyScale_z(GameObject[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of: GameObject is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue];
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].transform.lossyScale.z;
            }
        }








        public static FlexibleGetYValueFromCollection<Transform[]> GetYValueFrom_arrayOfTransforms_localPosition_x_preAllocated = GetYValueFrom_arrayOfTransforms_localPosition_x;
        static float GetYValueFrom_arrayOfTransforms_localPosition_x(Transform[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].localPosition.x;
            }
        }
        public static FlexibleGetYValueFromCollection<Transform[]> GetYValueFrom_arrayOfTransforms_localPosition_y_preAllocated = GetYValueFrom_arrayOfTransforms_localPosition_y;
        static float GetYValueFrom_arrayOfTransforms_localPosition_y(Transform[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].localPosition.y;
            }
        }
        public static FlexibleGetYValueFromCollection<Transform[]> GetYValueFrom_arrayOfTransforms_localPosition_z_preAllocated = GetYValueFrom_arrayOfTransforms_localPosition_z;
        static float GetYValueFrom_arrayOfTransforms_localPosition_z(Transform[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localPosition (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].localPosition.z;
            }
        }
        public static FlexibleGetYValueFromCollection<Transform[]> GetYValueFrom_arrayOfTransforms_localEulerAngle_x_preAllocated = GetYValueFrom_arrayOfTransforms_localEulerAngle_x;
        static float GetYValueFrom_arrayOfTransforms_localEulerAngle_x(Transform[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].localEulerAngles.x;
            }
        }
        public static FlexibleGetYValueFromCollection<Transform[]> GetYValueFrom_arrayOfTransforms_localEulerAngle_y_preAllocated = GetYValueFrom_arrayOfTransforms_localEulerAngle_y;
        static float GetYValueFrom_arrayOfTransforms_localEulerAngle_y(Transform[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].localEulerAngles.y;
            }
        }
        public static FlexibleGetYValueFromCollection<Transform[]> GetYValueFrom_arrayOfTransforms_localEulerAngle_z_preAllocated = GetYValueFrom_arrayOfTransforms_localEulerAngle_z;
        static float GetYValueFrom_arrayOfTransforms_localEulerAngle_z(Transform[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localEulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].localEulerAngles.z;
            }
        }
        public static FlexibleGetYValueFromCollection<Transform[]> GetYValueFrom_arrayOfTransforms_localScale_x_preAllocated = GetYValueFrom_arrayOfTransforms_localScale_x;
        static float GetYValueFrom_arrayOfTransforms_localScale_x(Transform[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].localScale.x;
            }
        }
        public static FlexibleGetYValueFromCollection<Transform[]> GetYValueFrom_arrayOfTransforms_localScale_y_preAllocated = GetYValueFrom_arrayOfTransforms_localScale_y;
        static float GetYValueFrom_arrayOfTransforms_localScale_y(Transform[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].localScale.y;
            }
        }
        public static FlexibleGetYValueFromCollection<Transform[]> GetYValueFrom_arrayOfTransforms_localScale_z_preAllocated = GetYValueFrom_arrayOfTransforms_localScale_z;
        static float GetYValueFrom_arrayOfTransforms_localScale_z(Transform[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "localScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].localScale.z;
            }
        }
        public static FlexibleGetYValueFromCollection<Transform[]> GetYValueFrom_arrayOfTransforms_globalPosition_x_preAllocated = GetYValueFrom_arrayOfTransforms_globalPosition_x;
        static float GetYValueFrom_arrayOfTransforms_globalPosition_x(Transform[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].position.x;
            }
        }
        public static FlexibleGetYValueFromCollection<Transform[]> GetYValueFrom_arrayOfTransforms_globalPosition_y_preAllocated = GetYValueFrom_arrayOfTransforms_globalPosition_y;
        static float GetYValueFrom_arrayOfTransforms_globalPosition_y(Transform[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].position.y;
            }
        }
        public static FlexibleGetYValueFromCollection<Transform[]> GetYValueFrom_arrayOfTransforms_globalPosition_z_preAllocated = GetYValueFrom_arrayOfTransforms_globalPosition_z;
        static float GetYValueFrom_arrayOfTransforms_globalPosition_z(Transform[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)position (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].position.z;
            }
        }
        public static FlexibleGetYValueFromCollection<Transform[]> GetYValueFrom_arrayOfTransforms_globalEulerAngle_x_preAllocated = GetYValueFrom_arrayOfTransforms_globalEulerAngle_x;
        static float GetYValueFrom_arrayOfTransforms_globalEulerAngle_x(Transform[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].eulerAngles.x;
            }
        }
        public static FlexibleGetYValueFromCollection<Transform[]> GetYValueFrom_arrayOfTransforms_globalEulerAngle_y_preAllocated = GetYValueFrom_arrayOfTransforms_globalEulerAngle_y;
        static float GetYValueFrom_arrayOfTransforms_globalEulerAngle_y(Transform[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].eulerAngles.y;
            }
        }
        public static FlexibleGetYValueFromCollection<Transform[]> GetYValueFrom_arrayOfTransforms_globalEulerAngle_z_preAllocated = GetYValueFrom_arrayOfTransforms_globalEulerAngle_z;
        static float GetYValueFrom_arrayOfTransforms_globalEulerAngle_z(Transform[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "(global)eulerAngles (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].eulerAngles.z;
            }
        }

        public static FlexibleGetYValueFromCollection<Transform[]> GetYValueFrom_arrayOfTransforms_lossyScale_x_preAllocated = GetYValueFrom_arrayOfTransforms_lossyScale_x;
        static float GetYValueFrom_arrayOfTransforms_lossyScale_x(Transform[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].lossyScale.x;
            }
        }
        public static FlexibleGetYValueFromCollection<Transform[]> GetYValueFrom_arrayOfTransforms_lossyScale_y_preAllocated = GetYValueFrom_arrayOfTransforms_lossyScale_y;
        static float GetYValueFrom_arrayOfTransforms_lossyScale_y(Transform[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].lossyScale.y;
            }
        }
        public static FlexibleGetYValueFromCollection<Transform[]> GetYValueFrom_arrayOfTransforms_lossyScale_z_preAllocated = GetYValueFrom_arrayOfTransforms_lossyScale_z;
        static float GetYValueFrom_arrayOfTransforms_lossyScale_z(Transform[] yValues, int i_slotWhereToObtainTheValue, out GameObject gameobjectThatIsTheSourceOfTheValues, out string lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject)
        {
            if (yValues[i_slotWhereToObtainTheValue] == null)
            {
                gameobjectThatIsTheSourceOfTheValues = null;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of: Transform is null)";
                return float.NaN;
            }
            else
            {
                gameobjectThatIsTheSourceOfTheValues = yValues[i_slotWhereToObtainTheValue].gameObject;
                lineNameExtraInfoOfConcernedLine_ifValueSourceIsGameobject = "lossyScale (of " + gameobjectThatIsTheSourceOfTheValues.name + ")";
                return yValues[i_slotWhereToObtainTheValue].lossyScale.z;
            }
        }


        ///Other:
        public static void SetPosRotScaleOfChart_toScreenspace(ChartDrawing concernedChart, Camera targetCamera, bool chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight, bool andApplyFixedRotationToInternalRotation)
        {
            concernedChart.Position_worldspace = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(targetCamera, concernedChart.position_inCamViewportspace, false);
            concernedChart.fixedRotation = targetCamera.transform.rotation;
            concernedChart.rotationSource = ChartDrawing.RotationSource.userDefinedFixedRotation;
            concernedChart.Height_inWorldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, InternalDXXL_BoundsCamViewportSpace.viewportCenter, true, concernedChart.Height_relToCamViewportHeight);
            if (chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight)
            {
                concernedChart.Width_inWorldSpace = UtilitiesDXXL_Screenspace.HorizExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, InternalDXXL_BoundsCamViewportSpace.viewportCenter, true, concernedChart.Width_relToCamViewport);
            }
            else
            {
                concernedChart.Width_inWorldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(targetCamera, InternalDXXL_BoundsCamViewportSpace.viewportCenter, true, concernedChart.Width_relToCamViewport);
            }

            if (andApplyFixedRotationToInternalRotation)
            {
                concernedChart.ApplyInternalRotation();
            }
        }

    }

}
