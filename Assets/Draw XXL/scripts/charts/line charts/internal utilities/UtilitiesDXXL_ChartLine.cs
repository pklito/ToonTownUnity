namespace DrawXXL
{
    public class UtilitiesDXXL_ChartLine
    {
        public delegate bool IsDrawnBecause_theSingleComponentOfMulticomponentData_thisLineRepresents_isEnabledChecker(DataComponentsThatAreDrawn dataComponentsThatAreDrawn);

        //Non-multiComponentData:
        public static bool DoDrawBecauseLineDoesntRepresentMultiComponentData(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return true;
        }

        //Vector2:
        public static bool DrawIf_vector2_x_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.vector2_x;
        }
        public static bool DrawIf_vector2_y_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.vector2_y;
        }

        //Vector3:
        public static bool DrawIf_vector3_x_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.vector3_x;
        }
        public static bool DrawIf_vector3_y_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.vector3_y;
        }
        public static bool DrawIf_vector3_z_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.vector3_z;
        }

        //Vector4:
        public static bool DrawIf_vector4_x_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.vector4_x;
        }
        public static bool DrawIf_vector4_y_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.vector4_y;
        }
        public static bool DrawIf_vector4_z_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.vector4_z;
        }
        public static bool DrawIf_vector4_w_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.vector4_w;
        }

        //Color:
        public static bool DrawIf_color_r_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.color_r;
        }
        public static bool DrawIf_color_g_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.color_g;
        }
        public static bool DrawIf_color_b_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.color_b;
        }
        public static bool DrawIf_color_a_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.color_a;
        }

        //Rotation:
        public static bool DrawIf_rotation_eulerX_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.rotation_eulerX;
        }
        public static bool DrawIf_rotation_eulerY_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.rotation_eulerY;
        }
        public static bool DrawIf_rotation_eulerZ_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.rotation_eulerZ;
        }

        //Transform:
        public static bool DrawIf_localPosition_x_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.localPosition_x;
        }
        public static bool DrawIf_localPosition_y_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.localPosition_y;
        }
        public static bool DrawIf_localPosition_z_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.localPosition_z;
        }
        public static bool DrawIf_localEulerAngle_x_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.localEulerAngle_x;
        }
        public static bool DrawIf_localEulerAngle_y_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.localEulerAngle_y;
        }
        public static bool DrawIf_localEulerAngle_z_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.localEulerAngle_z;
        }
        public static bool DrawIf_localScale_x_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.localScale_x;
        }
        public static bool DrawIf_localScale_y_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.localScale_y;
        }
        public static bool DrawIf_localScale_z_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.localScale_z;
        }
        public static bool DrawIf_globalPosition_x_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.globalPosition_x;
        }
        public static bool DrawIf_globalPosition_y_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.globalPosition_y;
        }
        public static bool DrawIf_globalPosition_z_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.globalPosition_z;
        }
        public static bool DrawIf_globalEulerAngle_x_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.globalEulerAngle_x;
        }
        public static bool DrawIf_globalEulerAngle_y_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.globalEulerAngle_y;
        }
        public static bool DrawIf_globalEulerAngle_z_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.globalEulerAngle_z;
        }
        public static bool DrawIf_lossyScale_x_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.lossyScale_x;
        }
        public static bool DrawIf_lossyScale_y_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.lossyScale_y;
        }
        public static bool DrawIf_lossyScale_z_isEnabled(DataComponentsThatAreDrawn dataComponentsThatAreDrawn)
        {
            return dataComponentsThatAreDrawn.lossyScale_z;
        }

    }

}
