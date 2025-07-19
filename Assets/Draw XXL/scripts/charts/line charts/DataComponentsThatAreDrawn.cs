namespace DrawXXL
{
    public class DataComponentsThatAreDrawn
    {
        //Vector2:
        public bool vector2_x = true;
        public bool vector2_y = true;

        //Vector3:
        public bool vector3_x = true;
        public bool vector3_y = true;
        public bool vector3_z = true;

        //Vector4:
        public bool vector4_x = true;
        public bool vector4_y = true;
        public bool vector4_z = true;
        public bool vector4_w = true;

        //Color:
        public bool color_r = true;
        public bool color_g = true;
        public bool color_b = true;
        public bool color_a = true;

        //Rotation:
        public bool rotation_eulerX = true;
        public bool rotation_eulerY = true;
        public bool rotation_eulerZ = true;

        //Transform:
        public bool localPosition_x = true;
        public bool localPosition_y = true;
        public bool localPosition_z = true;
        public bool localEulerAngle_x = false;
        public bool localEulerAngle_y = false;
        public bool localEulerAngle_z = false;
        public bool localScale_x = false;
        public bool localScale_y = false;
        public bool localScale_z = false;
        public bool globalPosition_x = false;
        public bool globalPosition_y = false;
        public bool globalPosition_z = false;
        public bool globalEulerAngle_x = false;
        public bool globalEulerAngle_y = false;
        public bool globalEulerAngle_z = false;
        public bool lossyScale_x = false;
        public bool lossyScale_y = false;
        public bool lossyScale_z = false;

        public void CopyValueFromOtherConfig(DataComponentsThatAreDrawn newConfig)
        {
            vector2_x = newConfig.vector2_x;
            vector2_y = newConfig.vector2_y;

            vector3_x = newConfig.vector3_x;
            vector3_y = newConfig.vector3_y;
            vector3_z = newConfig.vector3_z;

            vector4_x = newConfig.vector4_x;
            vector4_y = newConfig.vector4_y;
            vector4_z = newConfig.vector4_z;
            vector4_w = newConfig.vector4_w;

            color_r = newConfig.color_r;
            color_g = newConfig.color_g;
            color_b = newConfig.color_b;
            color_a = newConfig.color_a;

            rotation_eulerX = newConfig.rotation_eulerX;
            rotation_eulerY = newConfig.rotation_eulerY;
            rotation_eulerZ = newConfig.rotation_eulerZ;

            localPosition_x = newConfig.localPosition_x;
            localPosition_y = newConfig.localPosition_y;
            localPosition_z = newConfig.localPosition_z;
            localEulerAngle_x = newConfig.localEulerAngle_x;
            localEulerAngle_y = newConfig.localEulerAngle_y;
            localEulerAngle_z = newConfig.localEulerAngle_z;
            localScale_x = newConfig.localScale_x;
            localScale_y = newConfig.localScale_y;
            localScale_z = newConfig.localScale_z;
            globalPosition_x = newConfig.globalPosition_x;
            globalPosition_y = newConfig.globalPosition_y;
            globalPosition_z = newConfig.globalPosition_z;
            globalEulerAngle_x = newConfig.globalEulerAngle_x;
            globalEulerAngle_y = newConfig.globalEulerAngle_y;
            globalEulerAngle_z = newConfig.globalEulerAngle_z;
            lossyScale_x = newConfig.lossyScale_x;
            lossyScale_y = newConfig.lossyScale_y;
            lossyScale_z = newConfig.lossyScale_z;
        }

    }
}
