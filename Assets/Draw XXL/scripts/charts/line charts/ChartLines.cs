namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class ChartLines
    {
        static float default_luminanceOfLineColors = 0.5f;
        private float luminanceOfLineColors = default_luminanceOfLineColors;
        public float LuminanceOfLineColors
        {
            get { return luminanceOfLineColors; }
            set
            {
                value = Mathf.Clamp01(value);
                ReassignLuminanceToLinecolors(luminanceOfLineColors, value);
                luminanceOfLineColors = value;
            }
        }

        ChartDrawing chart_theseLinesArePartOf;
        List<InternalDXXL_HorizontalThresholdLineInitializer> horizontalThresholdsInitializer = new List<InternalDXXL_HorizontalThresholdLineInitializer>();

        //default colors:
        static float default_colorHue_of_xDimension = 0.0f;
        static float default_colorHue_of_yDimension = 0.3333333f;
        static float default_colorHue_of_zDimension = 0.6666666f;
        static float default_colorHue_of_wDimension = 0.2f;
        static float default_colorHue_of_premadeLine_float = 0.14f;
        static float default_colorHue_of_premadeLine_int = 0.465f;
        // static float default_colorHue_of_premadeLine_bool = 0.88f;
        static float default_colorHue_of_premadeLine_bool = 0.05f;
        //premade lines:
        List<ChartLine> preMadeLines = new List<ChartLine>();  
        List<ChartLine> userMadeLines = new List<ChartLine>();  
        public ChartLine premadeLine_float;
        public ChartLine premadeLine_int;
        public ChartLine premadeLine_vector2_x;
        public ChartLine premadeLine_vector2_y;
        public ChartLine premadeLine_vector3_x;
        public ChartLine premadeLine_vector3_y;
        public ChartLine premadeLine_vector3_z;
        public ChartLine premadeLine_vector4_x;
        public ChartLine premadeLine_vector4_y;
        public ChartLine premadeLine_vector4_z;
        public ChartLine premadeLine_vector4_w;
        public ChartLine premadeLine_color_r;
        public ChartLine premadeLine_color_g;
        public ChartLine premadeLine_color_b;
        public ChartLine premadeLine_color_a;
        public ChartLine premadeLine_rotation_eulerX;
        public ChartLine premadeLine_rotation_eulerY;
        public ChartLine premadeLine_rotation_eulerZ;
        public ChartLine premadeLine_bool;
        //premade lines for transform:
        //int i_premadeLine_markingTheFirstOneOfTheTransformLines;
        //int i_premadeLine_markingTheLastOneOfTheTransformLines;
        public ChartLine premadeLine_transform_localPosition_x;
        public ChartLine premadeLine_transform_localPosition_y;
        public ChartLine premadeLine_transform_localPosition_z;
        public ChartLine premadeLine_transform_localEulerAngle_x;
        public ChartLine premadeLine_transform_localEulerAngle_y;
        public ChartLine premadeLine_transform_localEulerAngle_z;
        public ChartLine premadeLine_transform_localScale_x;
        public ChartLine premadeLine_transform_localScale_y;
        public ChartLine premadeLine_transform_localScale_z;
        public ChartLine premadeLine_transform_globalPosition_x;
        public ChartLine premadeLine_transform_globalPosition_y;
        public ChartLine premadeLine_transform_globalPosition_z;
        public ChartLine premadeLine_transform_globalEulerAngle_x;
        public ChartLine premadeLine_transform_globalEulerAngle_y;
        public ChartLine premadeLine_transform_globalEulerAngle_z;
        public ChartLine premadeLine_transform_lossyScale_x;
        public ChartLine premadeLine_transform_lossyScale_y;
        public ChartLine premadeLine_transform_lossyScale_z;
        //premade lines for lists:
        List<List<ChartLine>> listOfPremadeListsOfLines = new List<List<ChartLine>>();
        List<ChartLine> preMadeListOfLines_float;
        List<ChartLine> preMadeListOfLines_int;
        List<ChartLine> preMadeListOfLines_vector2_x;
        List<ChartLine> preMadeListOfLines_vector2_y;
        List<ChartLine> preMadeListOfLines_vector3_x;
        List<ChartLine> preMadeListOfLines_vector3_y;
        List<ChartLine> preMadeListOfLines_vector3_z;
        List<ChartLine> preMadeListOfLines_rotation_eulerX;
        List<ChartLine> preMadeListOfLines_rotation_eulerY;
        List<ChartLine> preMadeListOfLines_rotation_eulerZ;
        List<ChartLine> preMadeListOfLines_bool;
        List<ChartLine> preMadeListOfLines_transforms_localPosition_x;
        List<ChartLine> preMadeListOfLines_transforms_localPosition_y;
        List<ChartLine> preMadeListOfLines_transforms_localPosition_z;
        List<ChartLine> preMadeListOfLines_transforms_localEulerAngle_x;
        List<ChartLine> preMadeListOfLines_transforms_localEulerAngle_y;
        List<ChartLine> preMadeListOfLines_transforms_localEulerAngle_z;
        List<ChartLine> preMadeListOfLines_transforms_localScale_x;
        List<ChartLine> preMadeListOfLines_transforms_localScale_y;
        List<ChartLine> preMadeListOfLines_transforms_localScale_z;
        List<ChartLine> preMadeListOfLines_transforms_globalPosition_x;
        List<ChartLine> preMadeListOfLines_transforms_globalPosition_y;
        List<ChartLine> preMadeListOfLines_transforms_globalPosition_z;
        List<ChartLine> preMadeListOfLines_transforms_globalEulerAngle_x;
        List<ChartLine> preMadeListOfLines_transforms_globalEulerAngle_y;
        List<ChartLine> preMadeListOfLines_transforms_globalEulerAngle_z;
        List<ChartLine> preMadeListOfLines_transforms_lossyScale_x;
        List<ChartLine> preMadeListOfLines_transforms_lossyScale_y;
        List<ChartLine> preMadeListOfLines_transforms_lossyScale_z;

        public ChartLines(ChartDrawing chart_theseLinesArePartOf)
        {
            this.chart_theseLinesArePartOf = chart_theseLinesArePartOf;

            premadeLine_float = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_premadeLine_float, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_float.Name = "float";
            preMadeLines.Add(premadeLine_float);

            premadeLine_int = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_premadeLine_int, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_int.Name = "int";
            preMadeLines.Add(premadeLine_int);

            premadeLine_vector2_x = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_vector2_x.Name = "x";
            premadeLine_vector2_x.NameExtraInfo = "of Vector2";
            premadeLine_vector2_x.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_vector2_x_isEnabled);
            preMadeLines.Add(premadeLine_vector2_x);

            premadeLine_vector2_y = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_vector2_y.Name = "y";
            premadeLine_vector2_y.NameExtraInfo = "of Vector2";
            premadeLine_vector2_y.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_vector2_y_isEnabled);
            preMadeLines.Add(premadeLine_vector2_y);

            premadeLine_vector3_x = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_vector3_x.Name = "x";
            premadeLine_vector3_x.NameExtraInfo = "of Vector3";
            premadeLine_vector3_x.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_vector3_x_isEnabled);
            preMadeLines.Add(premadeLine_vector3_x);

            premadeLine_vector3_y = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_vector3_y.Name = "y";
            premadeLine_vector3_y.NameExtraInfo = "of Vector3";
            premadeLine_vector3_y.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_vector3_y_isEnabled);
            preMadeLines.Add(premadeLine_vector3_y);

            premadeLine_vector3_z = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_vector3_z.Name = "z";
            premadeLine_vector3_z.NameExtraInfo = "of Vector3";
            premadeLine_vector3_z.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_vector3_z_isEnabled);
            preMadeLines.Add(premadeLine_vector3_z);

            premadeLine_vector4_x = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_vector4_x.Name = "x";
            premadeLine_vector4_x.NameExtraInfo = "of Vector4";
            premadeLine_vector4_x.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_vector4_x_isEnabled);
            preMadeLines.Add(premadeLine_vector4_x);

            premadeLine_vector4_y = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_vector4_y.Name = "y";
            premadeLine_vector4_y.NameExtraInfo = "of Vector4";
            premadeLine_vector4_y.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_vector4_y_isEnabled);
            preMadeLines.Add(premadeLine_vector4_y);

            premadeLine_vector4_z = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_vector4_z.Name = "z";
            premadeLine_vector4_z.NameExtraInfo = "of Vector4";
            premadeLine_vector4_z.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_vector4_z_isEnabled);
            preMadeLines.Add(premadeLine_vector4_z);

            premadeLine_vector4_w = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_wDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_vector4_w.Name = "w";
            premadeLine_vector4_w.NameExtraInfo = "of Vector4";
            premadeLine_vector4_w.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_vector4_w_isEnabled);
            preMadeLines.Add(premadeLine_vector4_w);

            premadeLine_color_r = new ChartLine(Color.red, chart_theseLinesArePartOf);
            premadeLine_color_r.Name = "red";
            premadeLine_color_r.NameExtraInfo = "color component";
            premadeLine_color_r.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_color_r_isEnabled);
            preMadeLines.Add(premadeLine_color_r);

            premadeLine_color_g = new ChartLine(Color.green, chart_theseLinesArePartOf);
            premadeLine_color_g.Name = "green";
            premadeLine_color_g.NameExtraInfo = "color component";
            premadeLine_color_g.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_color_g_isEnabled);
            preMadeLines.Add(premadeLine_color_g);

            premadeLine_color_b = new ChartLine(Color.blue, chart_theseLinesArePartOf);
            premadeLine_color_b.Name = "blue";
            premadeLine_color_b.NameExtraInfo = "color component";
            premadeLine_color_b.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_color_b_isEnabled);
            preMadeLines.Add(premadeLine_color_b);

            premadeLine_color_a = new ChartLine(Color.white, chart_theseLinesArePartOf);
            premadeLine_color_a.Name = "alpha";
            premadeLine_color_a.NameExtraInfo = "color component";
            premadeLine_color_a.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_color_a_isEnabled);
            preMadeLines.Add(premadeLine_color_a);

            premadeLine_rotation_eulerX = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_rotation_eulerX.Name = "x";
            premadeLine_rotation_eulerX.NameExtraInfo = "rotation euler [degrees]";
            premadeLine_rotation_eulerX.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_rotation_eulerX_isEnabled);
            preMadeLines.Add(premadeLine_rotation_eulerX);

            premadeLine_rotation_eulerY = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_rotation_eulerY.Name = "y";
            premadeLine_rotation_eulerY.NameExtraInfo = "rotation euler [degrees]";
            premadeLine_rotation_eulerY.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_rotation_eulerY_isEnabled);
            preMadeLines.Add(premadeLine_rotation_eulerY);

            premadeLine_rotation_eulerZ = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_rotation_eulerZ.Name = "z";
            premadeLine_rotation_eulerZ.NameExtraInfo = "rotation euler [degrees]";
            premadeLine_rotation_eulerZ.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_rotation_eulerZ_isEnabled);
            preMadeLines.Add(premadeLine_rotation_eulerZ);

            premadeLine_bool = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_premadeLine_bool, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_bool.Name = "bool";
            premadeLine_bool.disableMinMaxYVisualizers_dueTo_lineRepresentsBoolValues = true;
            preMadeLines.Add(premadeLine_bool);

            //i_premadeLine_markingTheFirstOneOfTheTransformLines = preMadeLines.Count;

            premadeLine_transform_localPosition_x = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_transform_localPosition_x.Name = "x";
            premadeLine_transform_localPosition_x.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_localPosition_x_isEnabled);
            preMadeLines.Add(premadeLine_transform_localPosition_x);

            premadeLine_transform_localPosition_y = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_transform_localPosition_y.Name = "y";
            premadeLine_transform_localPosition_y.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_localPosition_y_isEnabled);
            preMadeLines.Add(premadeLine_transform_localPosition_y);

            premadeLine_transform_localPosition_z = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_transform_localPosition_z.Name = "z";
            premadeLine_transform_localPosition_z.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_localPosition_z_isEnabled);
            preMadeLines.Add(premadeLine_transform_localPosition_z);

            premadeLine_transform_localEulerAngle_x = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_transform_localEulerAngle_x.Name = "x";
            premadeLine_transform_localEulerAngle_x.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_localEulerAngle_x_isEnabled);
            preMadeLines.Add(premadeLine_transform_localEulerAngle_x);

            premadeLine_transform_localEulerAngle_y = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_transform_localEulerAngle_y.Name = "y";
            premadeLine_transform_localEulerAngle_y.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_localEulerAngle_y_isEnabled);
            preMadeLines.Add(premadeLine_transform_localEulerAngle_y);

            premadeLine_transform_localEulerAngle_z = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_transform_localEulerAngle_z.Name = "z";
            premadeLine_transform_localEulerAngle_z.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_localEulerAngle_z_isEnabled);
            preMadeLines.Add(premadeLine_transform_localEulerAngle_z);

            premadeLine_transform_localScale_x = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_transform_localScale_x.Name = "x";
            premadeLine_transform_localScale_x.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_localScale_x_isEnabled);
            preMadeLines.Add(premadeLine_transform_localScale_x);

            premadeLine_transform_localScale_y = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_transform_localScale_y.Name = "y";
            premadeLine_transform_localScale_y.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_localScale_y_isEnabled);
            preMadeLines.Add(premadeLine_transform_localScale_y);

            premadeLine_transform_localScale_z = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_transform_localScale_z.Name = "z";
            premadeLine_transform_localScale_z.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_localScale_z_isEnabled);
            preMadeLines.Add(premadeLine_transform_localScale_z);

            premadeLine_transform_globalPosition_x = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_transform_globalPosition_x.Name = "x";
            premadeLine_transform_globalPosition_x.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_globalPosition_x_isEnabled);
            preMadeLines.Add(premadeLine_transform_globalPosition_x);

            premadeLine_transform_globalPosition_y = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_transform_globalPosition_y.Name = "y";
            premadeLine_transform_globalPosition_y.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_globalPosition_y_isEnabled);
            preMadeLines.Add(premadeLine_transform_globalPosition_y);

            premadeLine_transform_globalPosition_z = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_transform_globalPosition_z.Name = "z";
            premadeLine_transform_globalPosition_z.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_globalPosition_z_isEnabled);
            preMadeLines.Add(premadeLine_transform_globalPosition_z);

            premadeLine_transform_globalEulerAngle_x = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_transform_globalEulerAngle_x.Name = "x";
            premadeLine_transform_globalEulerAngle_x.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_globalEulerAngle_x_isEnabled);
            preMadeLines.Add(premadeLine_transform_globalEulerAngle_x);

            premadeLine_transform_globalEulerAngle_y = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_transform_globalEulerAngle_y.Name = "y";
            premadeLine_transform_globalEulerAngle_y.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_globalEulerAngle_y_isEnabled);
            preMadeLines.Add(premadeLine_transform_globalEulerAngle_y);

            premadeLine_transform_globalEulerAngle_z = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_transform_globalEulerAngle_z.Name = "z";
            premadeLine_transform_globalEulerAngle_z.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_globalEulerAngle_z_isEnabled);
            preMadeLines.Add(premadeLine_transform_globalEulerAngle_z);

            premadeLine_transform_lossyScale_x = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_transform_lossyScale_x.Name = "x";
            premadeLine_transform_lossyScale_x.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_lossyScale_x_isEnabled);
            preMadeLines.Add(premadeLine_transform_lossyScale_x);

            premadeLine_transform_lossyScale_y = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_transform_lossyScale_y.Name = "y";
            premadeLine_transform_lossyScale_y.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_lossyScale_y_isEnabled);
            preMadeLines.Add(premadeLine_transform_lossyScale_y);

            premadeLine_transform_lossyScale_z = new ChartLine(SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, default_luminanceOfLineColors), chart_theseLinesArePartOf);
            premadeLine_transform_lossyScale_z.Name = "z";
            premadeLine_transform_lossyScale_z.Assign_singleComponentOfMulticomponentData_thisLineRepresents(UtilitiesDXXL_ChartLine.DrawIf_lossyScale_z_isEnabled);
            preMadeLines.Add(premadeLine_transform_lossyScale_z);

            //i_premadeLine_markingTheLastOneOfTheTransformLines = preMadeLines.Count - 1;
        }

        public void Clear()
        {
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                preMadeLines[i].Clear();
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                userMadeLines[i].Clear();
            }
            listOfPremadeListsOfLines = new List<List<ChartLine>>();
            preMadeListOfLines_float = null;
            preMadeListOfLines_int = null;
            preMadeListOfLines_vector2_x = null;
            preMadeListOfLines_vector2_y = null;
            preMadeListOfLines_vector3_x = null;
            preMadeListOfLines_vector3_y = null;
            preMadeListOfLines_vector3_z = null;
            preMadeListOfLines_rotation_eulerX = null;
            preMadeListOfLines_rotation_eulerY = null;
            preMadeListOfLines_rotation_eulerZ = null;
            preMadeListOfLines_bool = null;
            preMadeListOfLines_transforms_localPosition_x = null;
            preMadeListOfLines_transforms_localPosition_y = null;
            preMadeListOfLines_transforms_localPosition_z = null;
            preMadeListOfLines_transforms_localEulerAngle_x = null;
            preMadeListOfLines_transforms_localEulerAngle_y = null;
            preMadeListOfLines_transforms_localEulerAngle_z = null;
            preMadeListOfLines_transforms_localScale_x = null;
            preMadeListOfLines_transforms_localScale_y = null;
            preMadeListOfLines_transforms_localScale_z = null;
            preMadeListOfLines_transforms_globalPosition_x = null;
            preMadeListOfLines_transforms_globalPosition_y = null;
            preMadeListOfLines_transforms_globalPosition_z = null;
            preMadeListOfLines_transforms_globalEulerAngle_x = null;
            preMadeListOfLines_transforms_globalEulerAngle_y = null;
            preMadeListOfLines_transforms_globalEulerAngle_z = null;
            preMadeListOfLines_transforms_lossyScale_x = null;
            preMadeListOfLines_transforms_lossyScale_y = null;
            preMadeListOfLines_transforms_lossyScale_z = null;
        }

        public void Draw(InternalDXXL_Plane chartPlane, float durationInSec, bool hiddenByNearerObjects)
        {
            //Use "chartDrawing.Draw" instead
            DXXLWrapperForUntiysBuildInDrawLines.currentlyDrawingChart = chart_theseLinesArePartOf;
            int numberOfAlreadyDrawnLines = 0;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                bool lineWasDrawn = preMadeLines[i].Draw(chartPlane, numberOfAlreadyDrawnLines, chart_theseLinesArePartOf.xAxis.ValueMarkingLowerEndOfTheAxis, chart_theseLinesArePartOf.xAxis.ValueMarkingUpperEndOfTheAxis, chart_theseLinesArePartOf.yAxis.ValueMarkingLowerEndOfTheAxis, chart_theseLinesArePartOf.yAxis.ValueMarkingUpperEndOfTheAxis, durationInSec, hiddenByNearerObjects);
                DXXLWrapperForUntiysBuildInDrawLines.currentlyDrawingChart = chart_theseLinesArePartOf;
                if (lineWasDrawn) { numberOfAlreadyDrawnLines++; }
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                bool lineWasDrawn = userMadeLines[i].Draw(chartPlane, numberOfAlreadyDrawnLines, chart_theseLinesArePartOf.xAxis.ValueMarkingLowerEndOfTheAxis, chart_theseLinesArePartOf.xAxis.ValueMarkingUpperEndOfTheAxis, chart_theseLinesArePartOf.yAxis.ValueMarkingLowerEndOfTheAxis, chart_theseLinesArePartOf.yAxis.ValueMarkingUpperEndOfTheAxis, durationInSec, hiddenByNearerObjects);
                DXXLWrapperForUntiysBuildInDrawLines.currentlyDrawingChart = chart_theseLinesArePartOf;
                if (lineWasDrawn) { numberOfAlreadyDrawnLines++; }
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    bool lineWasDrawn = listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].Draw(chartPlane, numberOfAlreadyDrawnLines, chart_theseLinesArePartOf.xAxis.ValueMarkingLowerEndOfTheAxis, chart_theseLinesArePartOf.xAxis.ValueMarkingUpperEndOfTheAxis, chart_theseLinesArePartOf.yAxis.ValueMarkingLowerEndOfTheAxis, chart_theseLinesArePartOf.yAxis.ValueMarkingUpperEndOfTheAxis, durationInSec, hiddenByNearerObjects);
                    DXXLWrapperForUntiysBuildInDrawLines.currentlyDrawingChart = chart_theseLinesArePartOf;
                    if (lineWasDrawn) { numberOfAlreadyDrawnLines++; }
                }
            }
            DXXLWrapperForUntiysBuildInDrawLines.currentlyDrawingChart = null;
        }

        GameObject gameobject_thatDeliveredThePreviousValue;
        public void AddValue(Transform yValueOfNewDataPoint)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                if (yValueOfNewDataPoint == null)
                {
                    Debug.LogError("Chart: Cannot add value, because Transform is null.");
                    return;
                }

                bool isFirstValueOfThisGameobject = false;
                if (gameobject_thatDeliveredThePreviousValue == null)
                {
                    isFirstValueOfThisGameobject = true;
                    gameobject_thatDeliveredThePreviousValue = yValueOfNewDataPoint.gameObject;
                }
                else
                {
                    if (gameobject_thatDeliveredThePreviousValue != yValueOfNewDataPoint.gameObject)
                    {
                        AddPointOfInterestForChangingValueSourceGameobject(premadeLine_transform_localPosition_x.GetCurrentAutomaticXValue(), gameobject_thatDeliveredThePreviousValue, yValueOfNewDataPoint.gameObject);
                        isFirstValueOfThisGameobject = true;
                        gameobject_thatDeliveredThePreviousValue = yValueOfNewDataPoint.gameObject;
                    }
                }

                AddValuesToTheTransformLines(yValueOfNewDataPoint, isFirstValueOfThisGameobject);
            }
        }

        void AddValuesToTheTransformLines(Transform yValueOfNewDataPoint, bool isFirstValueOfThisGameobject)
        {
            //use chartDrawing.AddValue() instead
            AddValueToATransformLine(premadeLine_transform_localPosition_x, yValueOfNewDataPoint.localPosition.x, isFirstValueOfThisGameobject, "local position", yValueOfNewDataPoint.gameObject);
            AddValueToATransformLine(premadeLine_transform_localPosition_y, yValueOfNewDataPoint.localPosition.y, isFirstValueOfThisGameobject, "local position", yValueOfNewDataPoint.gameObject);
            AddValueToATransformLine(premadeLine_transform_localPosition_z, yValueOfNewDataPoint.localPosition.z, isFirstValueOfThisGameobject, "local position", yValueOfNewDataPoint.gameObject);

            AddValueToATransformLine(premadeLine_transform_localEulerAngle_x, yValueOfNewDataPoint.localEulerAngles.x, isFirstValueOfThisGameobject, "local euler angles", yValueOfNewDataPoint.gameObject);
            AddValueToATransformLine(premadeLine_transform_localEulerAngle_y, yValueOfNewDataPoint.localEulerAngles.y, isFirstValueOfThisGameobject, "local euler angles", yValueOfNewDataPoint.gameObject);
            AddValueToATransformLine(premadeLine_transform_localEulerAngle_z, yValueOfNewDataPoint.localEulerAngles.z, isFirstValueOfThisGameobject, "local euler angles", yValueOfNewDataPoint.gameObject);

            AddValueToATransformLine(premadeLine_transform_localScale_x, yValueOfNewDataPoint.localScale.x, isFirstValueOfThisGameobject, "local scale", yValueOfNewDataPoint.gameObject);
            AddValueToATransformLine(premadeLine_transform_localScale_y, yValueOfNewDataPoint.localScale.y, isFirstValueOfThisGameobject, "local scale", yValueOfNewDataPoint.gameObject);
            AddValueToATransformLine(premadeLine_transform_localScale_z, yValueOfNewDataPoint.localScale.z, isFirstValueOfThisGameobject, "local scale", yValueOfNewDataPoint.gameObject);

            AddValueToATransformLine(premadeLine_transform_globalPosition_x, yValueOfNewDataPoint.position.x, isFirstValueOfThisGameobject, "global position", yValueOfNewDataPoint.gameObject);
            AddValueToATransformLine(premadeLine_transform_globalPosition_y, yValueOfNewDataPoint.position.y, isFirstValueOfThisGameobject, "global position", yValueOfNewDataPoint.gameObject);
            AddValueToATransformLine(premadeLine_transform_globalPosition_z, yValueOfNewDataPoint.position.z, isFirstValueOfThisGameobject, "global position", yValueOfNewDataPoint.gameObject);

            AddValueToATransformLine(premadeLine_transform_globalEulerAngle_x, yValueOfNewDataPoint.eulerAngles.x, isFirstValueOfThisGameobject, "global euler angles", yValueOfNewDataPoint.gameObject);
            AddValueToATransformLine(premadeLine_transform_globalEulerAngle_y, yValueOfNewDataPoint.eulerAngles.y, isFirstValueOfThisGameobject, "global euler angles", yValueOfNewDataPoint.gameObject);
            AddValueToATransformLine(premadeLine_transform_globalEulerAngle_z, yValueOfNewDataPoint.eulerAngles.z, isFirstValueOfThisGameobject, "global euler angles", yValueOfNewDataPoint.gameObject);

            AddValueToATransformLine(premadeLine_transform_lossyScale_x, yValueOfNewDataPoint.lossyScale.x, isFirstValueOfThisGameobject, "lossy scale", yValueOfNewDataPoint.gameObject);
            AddValueToATransformLine(premadeLine_transform_lossyScale_y, yValueOfNewDataPoint.lossyScale.y, isFirstValueOfThisGameobject, "lossy scale", yValueOfNewDataPoint.gameObject);
            AddValueToATransformLine(premadeLine_transform_lossyScale_z, yValueOfNewDataPoint.lossyScale.z, isFirstValueOfThisGameobject, "lossy scale", yValueOfNewDataPoint.gameObject);
        }

        void AddValueToATransformLine(ChartLine thisLine, float addedYValue, bool isFirstValueOfThisGameobject, string lineNameExtraInfo, GameObject gameobject_thatDeliversTheValue)
        {
            thisLine.AddValue(addedYValue);
            if (isFirstValueOfThisGameobject)
            {
                thisLine.NameExtraInfo = lineNameExtraInfo + " (of " + gameobject_thatDeliversTheValue.name + ")";
                thisLine.InternalAssignRepresentedGameobject(gameobject_thatDeliversTheValue);
            }
        }

        public void AddValuesFromList_toListWithFloatLines(List<float> yValues)
        {
            //use chartDrawing.AddValues_eachIndexIsALine() instead

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                AddValueCollectionSlots_toLines<List<float>>(ref preMadeListOfLines_float, false, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfFloats_preAllocated, UtilitiesDXXL_ChartLine.DoDrawBecauseLineDoesntRepresentMultiComponentData, UtilitiesDXXL_Math.DimensionNullable.none, "(float)");
            }
        }

        public void AddValuesFromArray_toListWithFloatLines(float[] yValues)
        {
            //use chartDrawing.AddValues_eachIndexIsALine() instead

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                AddValueCollectionSlots_toLines<float[]>(ref preMadeListOfLines_float, false, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfFloats_preAllocated, UtilitiesDXXL_ChartLine.DoDrawBecauseLineDoesntRepresentMultiComponentData, UtilitiesDXXL_Math.DimensionNullable.none, "(float)");
            }
        }

        public void AddValuesFromList_toListWithIntLines(List<int> yValues)
        {
            //use chartDrawing.AddValues_eachIndexIsALine() instead

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                AddValueCollectionSlots_toLines<List<int>>(ref preMadeListOfLines_int, false, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfInts_preAllocated, UtilitiesDXXL_ChartLine.DoDrawBecauseLineDoesntRepresentMultiComponentData, UtilitiesDXXL_Math.DimensionNullable.none, "(int)");
            }
        }

        public void AddValuesFromArray_toListWithIntLines(int[] yValues)
        {
            //use chartDrawing.AddValues_eachIndexIsALine() instead

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                AddValueCollectionSlots_toLines<int[]>(ref preMadeListOfLines_int, false, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfInts_preAllocated, UtilitiesDXXL_ChartLine.DoDrawBecauseLineDoesntRepresentMultiComponentData, UtilitiesDXXL_Math.DimensionNullable.none, "(int)");
            }
        }

        public void AddValuesFromList_toListWithVector2Lines(List<Vector2> yValues)
        {
            //use chartDrawing.AddValues_eachIndexIsALine() instead

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                AddValueCollectionSlots_toLines<List<Vector2>>(ref preMadeListOfLines_vector2_x, false, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfVector2s_xComponent_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_vector2_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, "of Vector2");
                AddValueCollectionSlots_toLines<List<Vector2>>(ref preMadeListOfLines_vector2_y, false, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfVector2s_yComponent_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_vector2_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, "of Vector2");
            }
        }

        public void AddValuesFromArray_toListWithVector2Lines(Vector2[] yValues)
        {
            //use chartDrawing.AddValues_eachIndexIsALine() instead

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                AddValueCollectionSlots_toLines<Vector2[]>(ref preMadeListOfLines_vector2_x, false, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfVector2s_xComponent_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_vector2_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, "of Vector2");
                AddValueCollectionSlots_toLines<Vector2[]>(ref preMadeListOfLines_vector2_y, false, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfVector2s_yComponent_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_vector2_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, "of Vector2");
            }
        }

        public void AddValuesFromList_toListWithVector3Lines(List<Vector3> yValues)
        {
            //use chartDrawing.AddValues_eachIndexIsALine() instead

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                AddValueCollectionSlots_toLines<List<Vector3>>(ref preMadeListOfLines_vector3_x, false, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfVector3s_xComponent_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_vector3_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, "of Vector3");
                AddValueCollectionSlots_toLines<List<Vector3>>(ref preMadeListOfLines_vector3_y, false, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfVector3s_yComponent_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_vector3_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, "of Vector3");
                AddValueCollectionSlots_toLines<List<Vector3>>(ref preMadeListOfLines_vector3_z, false, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfVector3s_zComponent_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_vector3_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, "of Vector3");
            }
        }

        public void AddValuesFromArray_toListWithVector3Lines(Vector3[] yValues)
        {
            //use chartDrawing.AddValues_eachIndexIsALine() instead

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                AddValueCollectionSlots_toLines<Vector3[]>(ref preMadeListOfLines_vector3_x, false, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfVector3s_xComponent_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_vector3_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, "of Vector3");
                AddValueCollectionSlots_toLines<Vector3[]>(ref preMadeListOfLines_vector3_y, false, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfVector3s_yComponent_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_vector3_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, "of Vector3");
                AddValueCollectionSlots_toLines<Vector3[]>(ref preMadeListOfLines_vector3_z, false, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfVector3s_zComponent_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_vector3_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, "of Vector3");
            }
        }

        public void AddValuesFromList_toListWithQuaternionLines(List<Quaternion> yValues)
        {
            //use chartDrawing.AddValues_eachIndexIsALine() instead

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                AddValueCollectionSlots_toLines<List<Quaternion>>(ref preMadeListOfLines_rotation_eulerX, false, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfQuaternions_eulerXComponent_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_rotation_eulerX_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, "(euler angles)");
                AddValueCollectionSlots_toLines<List<Quaternion>>(ref preMadeListOfLines_rotation_eulerY, false, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfQuaternions_eulerYComponent_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_rotation_eulerY_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, "(euler angles)");
                AddValueCollectionSlots_toLines<List<Quaternion>>(ref preMadeListOfLines_rotation_eulerZ, false, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfQuaternions_eulerZComponent_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_rotation_eulerZ_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, "(euler angles)");
            }
        }

        public void AddValuesFromArray_toListWithQuaternionLines(Quaternion[] yValues)
        {
            //use chartDrawing.AddValues_eachIndexIsALine() instead

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                AddValueCollectionSlots_toLines<Quaternion[]>(ref preMadeListOfLines_rotation_eulerX, false, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfQuaternions_eulerXComponent_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_rotation_eulerX_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, "(euler angles)");
                AddValueCollectionSlots_toLines<Quaternion[]>(ref preMadeListOfLines_rotation_eulerY, false, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfQuaternions_eulerYComponent_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_rotation_eulerY_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, "(euler angles)");
                AddValueCollectionSlots_toLines<Quaternion[]>(ref preMadeListOfLines_rotation_eulerZ, false, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfQuaternions_eulerZComponent_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_rotation_eulerZ_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, "(euler angles)");
            }
        }

        public void AddValuesFromList_toListWithBoolLines(List<bool> yValues)
        {
            //use chartDrawing.AddValues_eachIndexIsALine() instead

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                AddValueCollectionSlots_toLines<List<bool>>(ref preMadeListOfLines_bool, false, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfBools_preAllocated, UtilitiesDXXL_ChartLine.DoDrawBecauseLineDoesntRepresentMultiComponentData, UtilitiesDXXL_Math.DimensionNullable.none, "(bool)");
            }
        }

        public void AddValuesFromArray_toListWithBoolLines(bool[] yValues)
        {
            //use chartDrawing.AddValues_eachIndexIsALine() instead

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                AddValueCollectionSlots_toLines<bool[]>(ref preMadeListOfLines_bool, false, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfBools_preAllocated, UtilitiesDXXL_ChartLine.DoDrawBecauseLineDoesntRepresentMultiComponentData, UtilitiesDXXL_Math.DimensionNullable.none, "(bool)");
            }
        }

        public void AddValuesFromList_toListWithGameobjectLines(List<GameObject> yValues)
        {
            //use chartDrawing.AddValues_eachIndexIsALine() instead

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                TrySetPointsOfInterest_forChangingGameobjectsInCollectionSlots(yValues);
                AddValueCollectionSlots_toLines<List<GameObject>>(ref preMadeListOfLines_transforms_localPosition_x, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfGameobjects_localPosition_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localPosition_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<List<GameObject>>(ref preMadeListOfLines_transforms_localPosition_y, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfGameobjects_localPosition_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localPosition_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<List<GameObject>>(ref preMadeListOfLines_transforms_localPosition_z, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfGameobjects_localPosition_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localPosition_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<List<GameObject>>(ref preMadeListOfLines_transforms_localEulerAngle_x, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfGameobjects_localEulerAngle_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localEulerAngle_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<List<GameObject>>(ref preMadeListOfLines_transforms_localEulerAngle_y, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfGameobjects_localEulerAngle_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localEulerAngle_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<List<GameObject>>(ref preMadeListOfLines_transforms_localEulerAngle_z, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfGameobjects_localEulerAngle_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localEulerAngle_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<List<GameObject>>(ref preMadeListOfLines_transforms_localScale_x, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfGameobjects_localScale_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localScale_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<List<GameObject>>(ref preMadeListOfLines_transforms_localScale_y, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfGameobjects_localScale_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localScale_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<List<GameObject>>(ref preMadeListOfLines_transforms_localScale_z, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfGameobjects_localScale_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localScale_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<List<GameObject>>(ref preMadeListOfLines_transforms_globalPosition_x, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfGameobjects_globalPosition_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalPosition_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<List<GameObject>>(ref preMadeListOfLines_transforms_globalPosition_y, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfGameobjects_globalPosition_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalPosition_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<List<GameObject>>(ref preMadeListOfLines_transforms_globalPosition_z, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfGameobjects_globalPosition_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalPosition_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<List<GameObject>>(ref preMadeListOfLines_transforms_globalEulerAngle_x, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfGameobjects_globalEulerAngle_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalEulerAngle_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<List<GameObject>>(ref preMadeListOfLines_transforms_globalEulerAngle_y, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfGameobjects_globalEulerAngle_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalEulerAngle_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<List<GameObject>>(ref preMadeListOfLines_transforms_globalEulerAngle_z, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfGameobjects_globalEulerAngle_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalEulerAngle_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<List<GameObject>>(ref preMadeListOfLines_transforms_lossyScale_x, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfGameobjects_lossyScale_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_lossyScale_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<List<GameObject>>(ref preMadeListOfLines_transforms_lossyScale_y, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfGameobjects_lossyScale_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_lossyScale_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<List<GameObject>>(ref preMadeListOfLines_transforms_lossyScale_z, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfGameobjects_lossyScale_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_lossyScale_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
            }
        }

        public void AddValuesFromArray_toListWithGameobjectLines(GameObject[] yValues)
        {
            //use chartDrawing.AddValues_eachIndexIsALine() instead

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                TrySetPointsOfInterest_forChangingGameobjectsInCollectionSlots(yValues);
                AddValueCollectionSlots_toLines<GameObject[]>(ref preMadeListOfLines_transforms_localPosition_x, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfGameobjects_localPosition_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localPosition_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<GameObject[]>(ref preMadeListOfLines_transforms_localPosition_y, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfGameobjects_localPosition_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localPosition_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<GameObject[]>(ref preMadeListOfLines_transforms_localPosition_z, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfGameobjects_localPosition_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localPosition_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<GameObject[]>(ref preMadeListOfLines_transforms_localEulerAngle_x, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfGameobjects_localEulerAngle_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localEulerAngle_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<GameObject[]>(ref preMadeListOfLines_transforms_localEulerAngle_y, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfGameobjects_localEulerAngle_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localEulerAngle_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<GameObject[]>(ref preMadeListOfLines_transforms_localEulerAngle_z, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfGameobjects_localEulerAngle_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localEulerAngle_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<GameObject[]>(ref preMadeListOfLines_transforms_localScale_x, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfGameobjects_localScale_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localScale_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<GameObject[]>(ref preMadeListOfLines_transforms_localScale_y, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfGameobjects_localScale_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localScale_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<GameObject[]>(ref preMadeListOfLines_transforms_localScale_z, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfGameobjects_localScale_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localScale_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<GameObject[]>(ref preMadeListOfLines_transforms_globalPosition_x, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfGameobjects_globalPosition_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalPosition_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<GameObject[]>(ref preMadeListOfLines_transforms_globalPosition_y, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfGameobjects_globalPosition_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalPosition_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<GameObject[]>(ref preMadeListOfLines_transforms_globalPosition_z, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfGameobjects_globalPosition_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalPosition_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<GameObject[]>(ref preMadeListOfLines_transforms_globalEulerAngle_x, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfGameobjects_globalEulerAngle_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalEulerAngle_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<GameObject[]>(ref preMadeListOfLines_transforms_globalEulerAngle_y, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfGameobjects_globalEulerAngle_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalEulerAngle_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<GameObject[]>(ref preMadeListOfLines_transforms_globalEulerAngle_z, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfGameobjects_globalEulerAngle_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalEulerAngle_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<GameObject[]>(ref preMadeListOfLines_transforms_lossyScale_x, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfGameobjects_lossyScale_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_lossyScale_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<GameObject[]>(ref preMadeListOfLines_transforms_lossyScale_y, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfGameobjects_lossyScale_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_lossyScale_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<GameObject[]>(ref preMadeListOfLines_transforms_lossyScale_z, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfGameobjects_lossyScale_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_lossyScale_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
            }
        }

        public void AddValuesFromList_toListWithTransformLines(List<Transform> yValues)
        {
            //use chartDrawing.AddValues_eachIndexIsALine() instead

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                TrySetPointsOfInterest_forChangingGameobjectsInCollectionSlots(yValues);
                AddValueCollectionSlots_toLines<List<Transform>>(ref preMadeListOfLines_transforms_localPosition_x, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfTransforms_localPosition_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localPosition_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<List<Transform>>(ref preMadeListOfLines_transforms_localPosition_y, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfTransforms_localPosition_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localPosition_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<List<Transform>>(ref preMadeListOfLines_transforms_localPosition_z, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfTransforms_localPosition_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localPosition_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<List<Transform>>(ref preMadeListOfLines_transforms_localEulerAngle_x, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfTransforms_localEulerAngle_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localEulerAngle_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<List<Transform>>(ref preMadeListOfLines_transforms_localEulerAngle_y, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfTransforms_localEulerAngle_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localEulerAngle_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<List<Transform>>(ref preMadeListOfLines_transforms_localEulerAngle_z, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfTransforms_localEulerAngle_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localEulerAngle_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<List<Transform>>(ref preMadeListOfLines_transforms_localScale_x, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfTransforms_localScale_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localScale_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<List<Transform>>(ref preMadeListOfLines_transforms_localScale_y, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfTransforms_localScale_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localScale_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<List<Transform>>(ref preMadeListOfLines_transforms_localScale_z, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfTransforms_localScale_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localScale_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<List<Transform>>(ref preMadeListOfLines_transforms_globalPosition_x, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfTransforms_globalPosition_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalPosition_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<List<Transform>>(ref preMadeListOfLines_transforms_globalPosition_y, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfTransforms_globalPosition_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalPosition_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<List<Transform>>(ref preMadeListOfLines_transforms_globalPosition_z, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfTransforms_globalPosition_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalPosition_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<List<Transform>>(ref preMadeListOfLines_transforms_globalEulerAngle_x, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfTransforms_globalEulerAngle_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalEulerAngle_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<List<Transform>>(ref preMadeListOfLines_transforms_globalEulerAngle_y, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfTransforms_globalEulerAngle_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalEulerAngle_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<List<Transform>>(ref preMadeListOfLines_transforms_globalEulerAngle_z, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfTransforms_globalEulerAngle_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalEulerAngle_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<List<Transform>>(ref preMadeListOfLines_transforms_lossyScale_x, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfTransforms_lossyScale_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_lossyScale_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<List<Transform>>(ref preMadeListOfLines_transforms_lossyScale_y, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfTransforms_lossyScale_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_lossyScale_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<List<Transform>>(ref preMadeListOfLines_transforms_lossyScale_z, true, yValues, yValues.Count, UtilitiesDXXL_ChartDrawing.GetYValueFrom_listOfTransforms_lossyScale_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_lossyScale_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
            }
        }

        public void AddValuesFromArray_toListWithTransformLines(Transform[] yValues)
        {
            //use chartDrawing.AddValues_eachIndexIsALine() instead

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                TrySetPointsOfInterest_forChangingGameobjectsInCollectionSlots(yValues);
                AddValueCollectionSlots_toLines<Transform[]>(ref preMadeListOfLines_transforms_localPosition_x, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfTransforms_localPosition_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localPosition_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<Transform[]>(ref preMadeListOfLines_transforms_localPosition_y, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfTransforms_localPosition_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localPosition_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<Transform[]>(ref preMadeListOfLines_transforms_localPosition_z, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfTransforms_localPosition_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localPosition_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<Transform[]>(ref preMadeListOfLines_transforms_localEulerAngle_x, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfTransforms_localEulerAngle_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localEulerAngle_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<Transform[]>(ref preMadeListOfLines_transforms_localEulerAngle_y, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfTransforms_localEulerAngle_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localEulerAngle_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<Transform[]>(ref preMadeListOfLines_transforms_localEulerAngle_z, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfTransforms_localEulerAngle_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localEulerAngle_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<Transform[]>(ref preMadeListOfLines_transforms_localScale_x, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfTransforms_localScale_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localScale_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<Transform[]>(ref preMadeListOfLines_transforms_localScale_y, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfTransforms_localScale_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localScale_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<Transform[]>(ref preMadeListOfLines_transforms_localScale_z, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfTransforms_localScale_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_localScale_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<Transform[]>(ref preMadeListOfLines_transforms_globalPosition_x, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfTransforms_globalPosition_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalPosition_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<Transform[]>(ref preMadeListOfLines_transforms_globalPosition_y, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfTransforms_globalPosition_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalPosition_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<Transform[]>(ref preMadeListOfLines_transforms_globalPosition_z, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfTransforms_globalPosition_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalPosition_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<Transform[]>(ref preMadeListOfLines_transforms_globalEulerAngle_x, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfTransforms_globalEulerAngle_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalEulerAngle_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<Transform[]>(ref preMadeListOfLines_transforms_globalEulerAngle_y, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfTransforms_globalEulerAngle_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalEulerAngle_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<Transform[]>(ref preMadeListOfLines_transforms_globalEulerAngle_z, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfTransforms_globalEulerAngle_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_globalEulerAngle_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
                AddValueCollectionSlots_toLines<Transform[]>(ref preMadeListOfLines_transforms_lossyScale_x, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfTransforms_lossyScale_x_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_lossyScale_x_isEnabled, UtilitiesDXXL_Math.DimensionNullable.x, null);
                AddValueCollectionSlots_toLines<Transform[]>(ref preMadeListOfLines_transforms_lossyScale_y, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfTransforms_lossyScale_y_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_lossyScale_y_isEnabled, UtilitiesDXXL_Math.DimensionNullable.y, null);
                AddValueCollectionSlots_toLines<Transform[]>(ref preMadeListOfLines_transforms_lossyScale_z, true, yValues, yValues.Length, UtilitiesDXXL_ChartDrawing.GetYValueFrom_arrayOfTransforms_lossyScale_z_preAllocated, UtilitiesDXXL_ChartLine.DrawIf_lossyScale_z_isEnabled, UtilitiesDXXL_Math.DimensionNullable.z, null);
            }
        }

        void AddValueCollectionSlots_toLines<CollectionWithYValues>(ref List<ChartLine> concernedPremadeListOfLines, bool dataSourceIsFromGameobjectRespTransform, CollectionWithYValues yValues, int count_ofCollectionWithYValues, UtilitiesDXXL_ChartDrawing.FlexibleGetYValueFromCollection<CollectionWithYValues> FunctionThatObtainsYValueOfGenericCollection, UtilitiesDXXL_ChartLine.IsDrawnBecause_theSingleComponentOfMulticomponentData_thisLineRepresents_isEnabledChecker FunctionThatAssignsTheMultiComponentComponentThisLineRepresents, UtilitiesDXXL_Math.DimensionNullable vectorDimensionThisListOfLinesRepresents, string lineNameExtraInfo_forNonGameObjectDataSources)
        {
            if (yValues == null)
            {
                Debug.LogError("Chart: Cannot add values, because list is null.");
                return;
            }

            if (concernedPremadeListOfLines == null)
            {
                concernedPremadeListOfLines = new List<ChartLine>();
                listOfPremadeListsOfLines.Add(concernedPremadeListOfLines);
            }

            ChartLine lineOfList_withMostDataPoints_beforeCurrentlyAddedOne = Get_lineOfList_withMostDataPoints_beforeCurrentlyAddedOne(concernedPremadeListOfLines);
            int numberOfDataPoints_ofLineOfThisListWithMostDataPoints_beforeCurrentlyAddedOne = (lineOfList_withMostDataPoints_beforeCurrentlyAddedOne == null) ? 0 : lineOfList_withMostDataPoints_beforeCurrentlyAddedOne.dataPoints.Count;
            for (int i_insideNewValuesListThatEachMarkASeparateLine = 0; i_insideNewValuesListThatEachMarkASeparateLine < count_ofCollectionWithYValues; i_insideNewValuesListThatEachMarkASeparateLine++)
            {
                float addedYValue_asFloat = FunctionThatObtainsYValueOfGenericCollection(yValues, i_insideNewValuesListThatEachMarkASeparateLine, out GameObject gameobjectInCurrSlotOfAddedNewValuesList, out string lineNameExtraInfo_forTransformsAndGameObjects);
                CreateNewLineForCurrListSlot(ref concernedPremadeListOfLines, i_insideNewValuesListThatEachMarkASeparateLine, dataSourceIsFromGameobjectRespTransform, gameobjectInCurrSlotOfAddedNewValuesList, lineNameExtraInfo_forTransformsAndGameObjects, lineNameExtraInfo_forNonGameObjectDataSources, FunctionThatAssignsTheMultiComponentComponentThisLineRepresents, vectorDimensionThisListOfLinesRepresents);
                TryAssignGameobject_toCurrSlotsExistingLineWhichUntilNowHadANullGameobject(concernedPremadeListOfLines, i_insideNewValuesListThatEachMarkASeparateLine, dataSourceIsFromGameobjectRespTransform, gameobjectInCurrSlotOfAddedNewValuesList, lineNameExtraInfo_forTransformsAndGameObjects);
                BackwardFillGapOfLine_forSlotsThatWereTemporarlyNotContainedInTheList(concernedPremadeListOfLines, i_insideNewValuesListThatEachMarkASeparateLine, numberOfDataPoints_ofLineOfThisListWithMostDataPoints_beforeCurrentlyAddedOne, lineOfList_withMostDataPoints_beforeCurrentlyAddedOne);
                concernedPremadeListOfLines[i_insideNewValuesListThatEachMarkASeparateLine].InternalAddFromList(addedYValue_asFloat);
                TryReassignNewGameobject_toCurrSlotsLineWhichUntilNowHadADifferentGameobject(concernedPremadeListOfLines, i_insideNewValuesListThatEachMarkASeparateLine, dataSourceIsFromGameobjectRespTransform, gameobjectInCurrSlotOfAddedNewValuesList, lineNameExtraInfo_forTransformsAndGameObjects);
            }
        }

        ChartLine Get_lineOfList_withMostDataPoints_beforeCurrentlyAddedOne(List<ChartLine> concernedListOfLines)
        {
            if (concernedListOfLines == null)
            {
                return null;
            }
            else
            {
                if (concernedListOfLines.Count == 0)
                {
                    return null;
                }
                else
                {
                    int currMaxDataPointsOfALine = 0;
                    ChartLine returnedLine = concernedListOfLines[0];
                    for (int i = 0; i < concernedListOfLines.Count; i++)
                    {
                        if (concernedListOfLines[i].dataPoints.Count > currMaxDataPointsOfALine)
                        {
                            currMaxDataPointsOfALine = concernedListOfLines[i].dataPoints.Count;
                            returnedLine = concernedListOfLines[i];
                        }
                    }
                    return returnedLine;
                }
            }
        }

        void CreateNewLineForCurrListSlot(ref List<ChartLine> concernedPremadeListOfLines, int i_insideNewValuesListThatEachMarkASeparateLine, bool dataSourceIsFromGameobjectRespTransform, GameObject gameobjectInCurrSlotOfAddedNewValuesList, string lineNameExtraInfo_forTransformsAndGameObjects, string lineNameExtraInfo_forNonGameObjectDataSources, UtilitiesDXXL_ChartLine.IsDrawnBecause_theSingleComponentOfMulticomponentData_thisLineRepresents_isEnabledChecker FunctionThatAssignsTheMultiComponentComponentThisLineRepresents, UtilitiesDXXL_Math.DimensionNullable vectorDimensionThisListOfLinesRepresents)
        {
            if (concernedPremadeListOfLines.Count <= i_insideNewValuesListThatEachMarkASeparateLine)
            {
                ChartLine newlyCreatedLine = new ChartLine(default(Color), chart_theseLinesArePartOf);
                newlyCreatedLine.representsValuesFromAddedLists = true;
                AddExistingHorizThresholdsToNewlyCreatedLine(ref newlyCreatedLine);
                concernedPremadeListOfLines.Add(newlyCreatedLine);

                switch (vectorDimensionThisListOfLinesRepresents)
                {
                    case UtilitiesDXXL_Math.DimensionNullable.x:
                        concernedPremadeListOfLines[i_insideNewValuesListThatEachMarkASeparateLine].Name = "i=" + i_insideNewValuesListThatEachMarkASeparateLine + " x";
                        break;
                    case UtilitiesDXXL_Math.DimensionNullable.y:
                        concernedPremadeListOfLines[i_insideNewValuesListThatEachMarkASeparateLine].Name = "i=" + i_insideNewValuesListThatEachMarkASeparateLine + " y";
                        break;
                    case UtilitiesDXXL_Math.DimensionNullable.z:
                        concernedPremadeListOfLines[i_insideNewValuesListThatEachMarkASeparateLine].Name = "i=" + i_insideNewValuesListThatEachMarkASeparateLine + " z";
                        break;
                    case UtilitiesDXXL_Math.DimensionNullable.none:
                        concernedPremadeListOfLines[i_insideNewValuesListThatEachMarkASeparateLine].Name = "i=" + i_insideNewValuesListThatEachMarkASeparateLine;
                        break;
                    default:
                        concernedPremadeListOfLines[i_insideNewValuesListThatEachMarkASeparateLine].Name = "i=" + i_insideNewValuesListThatEachMarkASeparateLine;
                        break;
                }

                concernedPremadeListOfLines[i_insideNewValuesListThatEachMarkASeparateLine].Assign_singleComponentOfMulticomponentData_thisLineRepresents(FunctionThatAssignsTheMultiComponentComponentThisLineRepresents);
                if (dataSourceIsFromGameobjectRespTransform)
                {
                    if (gameobjectInCurrSlotOfAddedNewValuesList == null)
                    {
                        concernedPremadeListOfLines[i_insideNewValuesListThatEachMarkASeparateLine].NameExtraInfo = lineNameExtraInfo_forTransformsAndGameObjects;
                    }
                    else
                    {
                        AssignGameobjectToCurrListSlotsLine(concernedPremadeListOfLines, i_insideNewValuesListThatEachMarkASeparateLine, gameobjectInCurrSlotOfAddedNewValuesList, lineNameExtraInfo_forTransformsAndGameObjects);
                    }
                }
                else
                {
                    concernedPremadeListOfLines[i_insideNewValuesListThatEachMarkASeparateLine].NameExtraInfo = lineNameExtraInfo_forNonGameObjectDataSources;
                }
                ReassignListsLineColors(concernedPremadeListOfLines, vectorDimensionThisListOfLinesRepresents, luminanceOfLineColors);
            }
        }

        void TryAssignGameobject_toCurrSlotsExistingLineWhichUntilNowHadANullGameobject(List<ChartLine> concernedPremadeListOfLines, int i_insideNewValuesListThatEachMarkASeparateLine, bool dataSourceIsFromGameobjectRespTransform, GameObject gameobjectInCurrSlotOfAddedNewValuesList, string lineNameExtraInfo_forTransformsAndGameObjects)
        {
            if (dataSourceIsFromGameobjectRespTransform)
            {
                if (concernedPremadeListOfLines[i_insideNewValuesListThatEachMarkASeparateLine].Gameobject_thatThisLineCurrentlyRepresents == null)
                {
                    if (gameobjectInCurrSlotOfAddedNewValuesList != null)
                    {
                        //-> the line has already been created (because a previously added valueList contained this slot), but until now the slot was always filled with 'null':
                        AssignGameobjectToCurrListSlotsLine(concernedPremadeListOfLines, i_insideNewValuesListThatEachMarkASeparateLine, gameobjectInCurrSlotOfAddedNewValuesList, lineNameExtraInfo_forTransformsAndGameObjects);
                    }
                }
            }
        }

        void BackwardFillGapOfLine_forSlotsThatWereTemporarlyNotContainedInTheList(List<ChartLine> concernedPremadeListOfLines, int i_insideNewValuesListThatEachMarkASeparateLine, int numberOfDataPoints_ofLineOfThisListWithMostDataPoints_beforeCurrentlyAddedOne, ChartLine lineOfList_withMostDataPoints_beforeCurrentlyAddedOne)
        {
            int dataPointCount_ofCurrLineInsidePremadeList_beforeCurrentlyAddedOne = concernedPremadeListOfLines[i_insideNewValuesListThatEachMarkASeparateLine].dataPoints.Count;
            if (dataPointCount_ofCurrLineInsidePremadeList_beforeCurrentlyAddedOne < numberOfDataPoints_ofLineOfThisListWithMostDataPoints_beforeCurrentlyAddedOne)
            {
                //-> Filling gaps if yValues.Length/Count changed from call to call (in other words: if currLine wasn't supplied during the preceding "AddValues_eachIndexIsALine()"-calls (because the collection-Length/Count was shorter) and it now has to "catch up"):
                for (int i = dataPointCount_ofCurrLineInsidePremadeList_beforeCurrentlyAddedOne; i < numberOfDataPoints_ofLineOfThisListWithMostDataPoints_beforeCurrentlyAddedOne; i++)
                {
                    concernedPremadeListOfLines[i_insideNewValuesListThatEachMarkASeparateLine].InternalAddPlaceholderDatapointForNonExistingListSlot(lineOfList_withMostDataPoints_beforeCurrentlyAddedOne.dataPoints[i].xValue);
                }
            }
        }

        void TryReassignNewGameobject_toCurrSlotsLineWhichUntilNowHadADifferentGameobject(List<ChartLine> concernedPremadeListOfLines, int i_insideNewValuesListThatEachMarkASeparateLine, bool dataSourceIsFromGameobjectRespTransform, GameObject gameobjectInCurrSlotOfAddedNewValuesList, string lineNameExtraInfo_forTransformsAndGameObjects)
        {
            if (dataSourceIsFromGameobjectRespTransform)
            {
                if (gameobjectInCurrSlotOfAddedNewValuesList != null)
                {
                    // "concernedPremadeListOfLines[i_insideNewValuesListThatEachMarkASeparateLine].gameobject_thatThisLineCurrentlyRepresents" cannot be "null" here
                    bool theGameobjectThatIsRepresentedByTheCurrList_changedSinceLastValueAdding = (gameobjectInCurrSlotOfAddedNewValuesList != concernedPremadeListOfLines[i_insideNewValuesListThatEachMarkASeparateLine].Gameobject_thatThisLineCurrentlyRepresents);
                    if (theGameobjectThatIsRepresentedByTheCurrList_changedSinceLastValueAdding)
                    {
                        AssignGameobjectToCurrListSlotsLine(concernedPremadeListOfLines, i_insideNewValuesListThatEachMarkASeparateLine, gameobjectInCurrSlotOfAddedNewValuesList, lineNameExtraInfo_forTransformsAndGameObjects);
                    }
                }
            }
        }

        void AssignGameobjectToCurrListSlotsLine(List<ChartLine> concernedPremadeListOfLines, int i_insideNewValuesListThatEachMarkASeparateLine, GameObject gameobjectInCurrSlotOfAddedNewValuesList, string lineNameExtraInfo_forTransformsAndGameObjects)
        {
            concernedPremadeListOfLines[i_insideNewValuesListThatEachMarkASeparateLine].NameExtraInfo = lineNameExtraInfo_forTransformsAndGameObjects;
            concernedPremadeListOfLines[i_insideNewValuesListThatEachMarkASeparateLine].InternalAssignRepresentedGameobject(gameobjectInCurrSlotOfAddedNewValuesList);
        }

        public ChartLine AddLine(string name, Color color = default(Color))
        {
            if (name == null || name == "")
            {
                Debug.LogError("'AddLine()' failed, because 'name' is null or empty.");
                return null;
            }

            ChartLine alreadyExistingLineWithSameName = GetUsermadeLine(name, false);
            if (alreadyExistingLineWithSameName == null)
            {
                ChartLine newLine = new ChartLine(UtilitiesDXXL_Colors.red_xAxisAlpha1, chart_theseLinesArePartOf);
                newLine.Name = name;
                userMadeLines.Add(newLine);
                if (UtilitiesDXXL_Colors.IsDefaultColor(color))
                {
                    TryReassignRainbowColorsToAllUsermadeLines();
                }
                else
                {
                    newLine.Color = color;
                }
                AddExistingHorizThresholdsToNewlyCreatedLine(ref newLine);
                return newLine;
            }
            else
            {
                Debug.LogError("'AddLine()' failed, because a line with the name '" + name + "' already exists.");
                return null;
            }
        }

        public ChartLine GetUsermadeLine(string lineName, bool createLineIfItDoesntExist)
        {
            if (lineName == null || lineName == "")
            {
                Debug.LogError("'GetUsermadeLine()' failed, because the requested 'lineName' is null or empty.");
                return null;
            }

            for (int i = 0; i < userMadeLines.Count; i++)
            {
                if (userMadeLines[i].Name != null)
                {
                    if (userMadeLines[i].Name == lineName)
                    {
                        return userMadeLines[i];
                    }
                }
            }

            //A line with this name doesn't exist yet:
            if (createLineIfItDoesntExist)
            {
                return chart_theseLinesArePartOf.AddLine(lineName, default(Color), false);
            }
            else
            {
                return null;
            }
        }

        public void SetLineNamesPositions(ChartLine.NamePosition newLineNamesPosition)
        {
            //overwrites the linePosition for all lines of the chart. If you want to specificy different linename positions for each line you can directly set chartLine.namePosition
            chart_theseLinesArePartOf.default_lineNamePosition = newLineNamesPosition;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                preMadeLines[i].namePosition = newLineNamesPosition;
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                userMadeLines[i].namePosition = newLineNamesPosition;
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].namePosition = newLineNamesPosition;
                }
            }
        }

        public void SetLineNamesSize(float newLineNamesSizeScaleFactor)
        {
            //overwrites the NameText_sizeScaleFactor for all lines of the chart. If you want to specificy different NameText_sizeScaleFactor for each line you can directly set chartLine.NameText_sizeScaleFactor
            chart_theseLinesArePartOf.default_lineNameText_sizeScaleFactor = newLineNamesSizeScaleFactor;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                preMadeLines[i].NameText_sizeScaleFactor = newLineNamesSizeScaleFactor;
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                userMadeLines[i].NameText_sizeScaleFactor = newLineNamesSizeScaleFactor;
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].NameText_sizeScaleFactor = newLineNamesSizeScaleFactor;
                }
            }
        }

        public void Set_lineConnectionsType(ChartLine.LineConnectionsType newLineConnectionsType)
        {
            //overwrites the lineConnectionsType for all lines of the chart. If you want to specificy different lineConnectionsTypes for each line you can directly set chartLine.lineConnectionsType
            chart_theseLinesArePartOf.default_lineConnectionsType = newLineConnectionsType;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                preMadeLines[i].lineConnectionsType = newLineConnectionsType;
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                userMadeLines[i].lineConnectionsType = newLineConnectionsType;
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].lineConnectionsType = newLineConnectionsType;
                }
            }
        }

        public void Set_dataPointVisualization(ChartLine.DataPointVisualization newDataPointVisualization)
        {
            //overwrites the dataPointVisualization for all lines of the chart. If you want to specificy different dataPointVisualization for each line you can directly set chartLine.dataPointVisualization
            chart_theseLinesArePartOf.default_dataPointVisualization = newDataPointVisualization;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                preMadeLines[i].dataPointVisualization = newDataPointVisualization;
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                userMadeLines[i].dataPointVisualization = newDataPointVisualization;
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].dataPointVisualization = newDataPointVisualization;
                }
            }
        }

        public void Set_alpha_ofVerticalAreaFillLines(float newAlpha)
        {
            //overwrites the alpha_ofVerticalFillAreaLines for all lines of the chart. If you want to specificy different alpha_ofVerticalFillAreaLines for each line you can directly set chartLine.alpha_ofVerticalFillAreaLines
            chart_theseLinesArePartOf.default_alpha_ofVerticalAreaFillLines = newAlpha;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                preMadeLines[i].Alpha_ofVerticalAreaFillLines = newAlpha;
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                userMadeLines[i].Alpha_ofVerticalAreaFillLines = newAlpha;
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].Alpha_ofVerticalAreaFillLines = newAlpha;
                }
            }
        }

        public void Set_alpha_ofHighlighterForMostCurrentValue_xDim(float newAlpha)
        {
            //overwrites the alpha_ofHighlighterForMostCurrentValue_xDim for all lines of the chart. If you want to specificy different alpha_ofHighlighterForMostCurrentValue_xDim for each line you can directly set chartLine.alpha_ofHighlighterForMostCurrentValue_xDim
            chart_theseLinesArePartOf.default_alpha_ofHighlighterForMostCurrentValue_xDim = newAlpha;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                preMadeLines[i].alpha_ofHighlighterForMostCurrentValue_xDim = newAlpha;
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                userMadeLines[i].alpha_ofHighlighterForMostCurrentValue_xDim = newAlpha;
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].alpha_ofHighlighterForMostCurrentValue_xDim = newAlpha;
                }
            }
        }

        public void Set_alpha_ofHighlighterForMostCurrentValue_yDim(float newAlpha)
        {
            //overwrites the alpha_ofHighlighterForMostCurrentValue_yDim for all lines of the chart. If you want to specificy different alpha_ofHighlighterForMostCurrentValue_yDim for each line you can directly set chartLine.alpha_ofHighlighterForMostCurrentValue_yDim
            chart_theseLinesArePartOf.default_alpha_ofHighlighterForMostCurrentValue_yDim = newAlpha;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                preMadeLines[i].alpha_ofHighlighterForMostCurrentValue_yDim = newAlpha;
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                userMadeLines[i].alpha_ofHighlighterForMostCurrentValue_yDim = newAlpha;
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].alpha_ofHighlighterForMostCurrentValue_yDim = newAlpha;
                }
            }
        }

        public void Set_displayDeltaAtHighlighterForMostCurrentValue(bool doDisplay)
        {
            //overwrites the displayDeltaAtHighlighterForMostCurrentValue for all lines of the chart. If you want to specificy different displayDeltaAtHighlighterForMostCurrentValue for each line you can directly set chartLine.displayDeltaAtHighlighterForMostCurrentValue
            chart_theseLinesArePartOf.default_displayDeltaAtHighlighterForMostCurrentValue = doDisplay;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                preMadeLines[i].displayDeltaAtHighlighterForMostCurrentValue = doDisplay;
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                userMadeLines[i].displayDeltaAtHighlighterForMostCurrentValue = doDisplay;
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].displayDeltaAtHighlighterForMostCurrentValue = doDisplay;
                }
            }
        }

        public void Set_alpha_ofMaxiumumYValueMarker(float newAlpha)
        {
            //overwrites the alpha_ofMaxiumumYValueMarker for all lines of the chart. If you want to specificy different alpha_ofMaxiumumYValueMarker for each line you can directly set chartLine.alpha_ofMaxiumumYValueMarker
            chart_theseLinesArePartOf.default_alpha_ofMaxiumumYValueMarker = newAlpha;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                preMadeLines[i].alpha_ofMaxiumumYValueMarker = newAlpha;
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                userMadeLines[i].alpha_ofMaxiumumYValueMarker = newAlpha;
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].alpha_ofMaxiumumYValueMarker = newAlpha;
                }
            }
        }

        public void Set_alpha_ofMinimumYValueMarker(float newAlpha)
        {
            //overwrites the alpha_ofMinimumYValueMarker for all lines of the chart. If you want to specificy different alpha_ofMinimumYValueMarker for each line you can directly set chartLine.alpha_ofMinimumYValueMarker
            chart_theseLinesArePartOf.default_alpha_ofMinimumYValueMarker = newAlpha;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                preMadeLines[i].alpha_ofMinimumYValueMarker = newAlpha;
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                userMadeLines[i].alpha_ofMinimumYValueMarker = newAlpha;
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].alpha_ofMinimumYValueMarker = newAlpha;
                }
            }
        }

        public void Set_markAllYMaximumTurningPoints(bool markEnabled)
        {
            //overwrites the markAllYMaximumTurningPoints for all lines of the chart. If you want to specificy different markAllYMaximumTurningPoints for each line you can directly set chartLine.markAllYMaximumTurningPoints
            chart_theseLinesArePartOf.default_markAllYMaximumTurningPoints = markEnabled;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                preMadeLines[i].markAllYMaximumTurningPoints = markEnabled;
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                userMadeLines[i].markAllYMaximumTurningPoints = markEnabled;
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].markAllYMaximumTurningPoints = markEnabled;
                }
            }
        }

        public void Set_markAllYMinimumTurningPoints(bool markEnabled)
        {
            //overwrites the markAllYMinimumTurningPoints for all lines of the chart. If you want to specificy different markAllYMinimumTurningPoints for each line you can directly set chartLine.markAllYMinimumTurningPoints
            chart_theseLinesArePartOf.default_markAllYMinimumTurningPoints = markEnabled;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                preMadeLines[i].markAllYMinimumTurningPoints = markEnabled;
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                userMadeLines[i].markAllYMinimumTurningPoints = markEnabled;
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].markAllYMinimumTurningPoints = markEnabled;
                }
            }
        }

        public void Set_SizeOfPoints_relToChartHeight(float newRelSize)
        {
            //overwrites the SizeOfPoints_relToChartHeight for all lines of the chart. If you want to specificy different SizeOfPoints_relToChartHeight for each line you can directly set chartLine.SizeOfPoints_relToChartHeight
            chart_theseLinesArePartOf.default_SizeOfPoints_relToChartHeight = newRelSize;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                preMadeLines[i].SizeOfPoints_relToChartHeight = newRelSize;
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                userMadeLines[i].SizeOfPoints_relToChartHeight = newRelSize;
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].SizeOfPoints_relToChartHeight = newRelSize;
                }
            }
        }

        public void Set_lineWidth_relToChartHeight(float newRelSize)
        {
            //overwrites the lineWidth_relToChartHeight for all lines of the chart. If you want to specificy different lineWidth_relToChartHeight for each line you can directly set chartLine.lineWidth_relToChartHeight
            chart_theseLinesArePartOf.default_lineWidth_relToChartHeight = newRelSize;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                preMadeLines[i].LineWidth_relToChartHeight = newRelSize;
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                userMadeLines[i].LineWidth_relToChartHeight = newRelSize;
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].LineWidth_relToChartHeight = newRelSize;
                }
            }
        }

        public void Set_pointVisualisationLineWidth_relToChartHeight(float newRelSize)
        {
            //overwrites the pointVisualisationLineWidth_relToChartHeight for all lines of the chart. If you want to specificy different pointVisualisationLineWidth_relToChartHeight for each line you can directly set chartLine.pointVisualisationLineWidth_relToChartHeight
            chart_theseLinesArePartOf.default_pointVisualisationLineWidth_relToChartHeight = newRelSize;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                preMadeLines[i].pointVisualisationLineWidth_relToChartHeight = newRelSize;
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                userMadeLines[i].pointVisualisationLineWidth_relToChartHeight = newRelSize;
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].pointVisualisationLineWidth_relToChartHeight = newRelSize;
                }
            }
        }

        int stillAvailableTextBoxes_inUpperRightCorner_duringPreviousDrawRun = 0;
        public void DrawPointsOfInterest(float yHeightFacor_forColumnStartPos, float durationInSec, bool hiddenByNearerObjects)
        {
            //Use "chartDrawing.Draw() instead.

            //first pass: check if they are drawn
            int stillAvailableTextBoxes_inUpperRightCorner = chart_theseLinesArePartOf.MaxDisplayedPointOfInterestTextBoxesPerSide;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                stillAvailableTextBoxes_inUpperRightCorner = preMadeLines[i].Internal_Set_isDrawnInNextPass_forAllPointsOfInterest(stillAvailableTextBoxes_inUpperRightCorner);
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                stillAvailableTextBoxes_inUpperRightCorner = userMadeLines[i].Internal_Set_isDrawnInNextPass_forAllPointsOfInterest(stillAvailableTextBoxes_inUpperRightCorner);
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    stillAvailableTextBoxes_inUpperRightCorner = listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].Internal_Set_isDrawnInNextPass_forAllPointsOfInterest(stillAvailableTextBoxes_inUpperRightCorner);
                }
            }

            //second pass: draw them
            Vector3 next_lowAnchorPositionOfText_inWorldspace = chart_theseLinesArePartOf.Position_worldspace + chart_theseLinesArePartOf.yAxis.AxisVector_inWorldSpace * yHeightFacor_forColumnStartPos + chart_theseLinesArePartOf.xAxis.AxisVector_inWorldSpace * 1.00f;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                next_lowAnchorPositionOfText_inWorldspace = preMadeLines[i].DrawPointsOfInterest(next_lowAnchorPositionOfText_inWorldspace, durationInSec, hiddenByNearerObjects);
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                next_lowAnchorPositionOfText_inWorldspace = userMadeLines[i].DrawPointsOfInterest(next_lowAnchorPositionOfText_inWorldspace, durationInSec, hiddenByNearerObjects);
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    next_lowAnchorPositionOfText_inWorldspace = listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].DrawPointsOfInterest(next_lowAnchorPositionOfText_inWorldspace, durationInSec, hiddenByNearerObjects);
                }
            }

            //draw "hiddenTextBoxes-communicating-textBox"
            //-> draw this "hiddenTextBoxes-communicating-textBox" AFTER(=ABOVE) the other textBoxes, so that it appears as the "oldest" text box: newer text boxes will be drawn lower than this
            if (stillAvailableTextBoxes_inUpperRightCorner < 0)
            {
                if (stillAvailableTextBoxes_inUpperRightCorner != stillAvailableTextBoxes_inUpperRightCorner_duringPreviousDrawRun) //-> saving GC.alloc by only recreating the text when something changes
                {
                    if (stillAvailableTextBoxes_inUpperRightCorner == (-1))
                    {
                        chart_theseLinesArePartOf.pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheRightSide.text = "<size=10>...and 1 more hidden older text box.<br><br></size><size=6>See also 'ChartDrawing.maxDisplayedPointOfInterestTextBoxesPerSide'.</size>";
                    }
                    else
                    {
                        chart_theseLinesArePartOf.pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheRightSide.text = "<size=10>...and " + (-stillAvailableTextBoxes_inUpperRightCorner) + " more hidden older text boxes.<br><br></size><size=6>See also 'ChartDrawing.maxDisplayedPointOfInterestTextBoxesPerSide'.</size>";
                    }
                }
                stillAvailableTextBoxes_inUpperRightCorner_duringPreviousDrawRun = stillAvailableTextBoxes_inUpperRightCorner;
                chart_theseLinesArePartOf.pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheRightSide.colorOfPointerTextBox = chart_theseLinesArePartOf.color;
                UtilitiesDXXL_Math.SkewedDirection cornerOfChartWhereTextBoxIsMounted = UtilitiesDXXL_Math.SkewedDirection.upRight;
                chart_theseLinesArePartOf.pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheRightSide.TryDraw(next_lowAnchorPositionOfText_inWorldspace, cornerOfChartWhereTextBoxIsMounted, durationInSec, hiddenByNearerObjects);
            }
        }

        public void AddHorizontalThresholdToEachLine(float yPosition, bool lineItselfCountsToLowerArea = false)
        {
            //use "chartDrawing.AddHorizontalThresholdLine" instead

            if (UtilitiesDXXL_Math.FloatIsInvalid(yPosition))
            {
                Debug.LogError("Cannot create threshold line at " + yPosition);
                return;
            }

            InternalDXXL_HorizontalThresholdLineInitializer thresholdLineInitializer = new InternalDXXL_HorizontalThresholdLineInitializer();
            thresholdLineInitializer.yPositionOfThresholdToCreate = yPosition;
            thresholdLineInitializer.lineItselfCountsToLowerArea = lineItselfCountsToLowerArea;
            horizontalThresholdsInitializer.Add(thresholdLineInitializer);

            bool hideThresholdLineAndShowOnlyTheIntersectionPointers = true;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                preMadeLines[i].AddHorizontalThresholdLine(yPosition, lineItselfCountsToLowerArea, default(Color), hideThresholdLineAndShowOnlyTheIntersectionPointers);
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                userMadeLines[i].AddHorizontalThresholdLine(yPosition, lineItselfCountsToLowerArea, default(Color), hideThresholdLineAndShowOnlyTheIntersectionPointers);
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].AddHorizontalThresholdLine(yPosition, lineItselfCountsToLowerArea, default(Color), hideThresholdLineAndShowOnlyTheIntersectionPointers);
                }
            }
        }

        void AddExistingHorizThresholdsToNewlyCreatedLine(ref ChartLine newlyCreatedChartLine)
        {
            bool hideThresholdLineAndShowOnlyTheIntersectionPointers = true;
            for (int i = 0; i < horizontalThresholdsInitializer.Count; i++)
            {
                newlyCreatedChartLine.AddHorizontalThresholdLine(horizontalThresholdsInitializer[i].yPositionOfThresholdToCreate, horizontalThresholdsInitializer[i].lineItselfCountsToLowerArea, default(Color), hideThresholdLineAndShowOnlyTheIntersectionPointers);
            }
        }

        int numberOfUserMadeLines_inMomentOfLastAutomaticRainbowColorReassignment = 0;
        void TryReassignRainbowColorsToAllUsermadeLines()
        {
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                bool isTheNewlyCreatedLine = (i == (userMadeLines.Count - 1));
                if (isTheNewlyCreatedLine)
                {
                    userMadeLines[i].Color = SeededColorGenerator.GetRainbowColor(i, 1.0f, userMadeLines.Count, luminanceOfLineColors);
                }
                else
                {
                    Color expectedColorOfLine_beforeCurrReassignment_ifColorWasAutogenerated = SeededColorGenerator.GetRainbowColor(i, 1.0f, numberOfUserMadeLines_inMomentOfLastAutomaticRainbowColorReassignment, luminanceOfLineColors);
                    bool colorWasManuallySetByUser = (UtilitiesDXXL_Colors.IsApproxSameColor(userMadeLines[i].Color, expectedColorOfLine_beforeCurrReassignment_ifColorWasAutogenerated) == false);
                    if (colorWasManuallySetByUser == false)
                    {
                        userMadeLines[i].Color = SeededColorGenerator.GetRainbowColor(i, 1.0f, userMadeLines.Count, luminanceOfLineColors);
                    }
                }
            }
            numberOfUserMadeLines_inMomentOfLastAutomaticRainbowColorReassignment = userMadeLines.Count;
        }

        void ReassignLuminanceToLinecolors(float oldLuminance, float newLuminance)
        {
            Color colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated;
            Color colorToAssign_ifAutogenerated;

            for (int i = 0; i < userMadeLines.Count; i++)
            {
                colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetRainbowColor(i, 1.0f, numberOfUserMadeLines_inMomentOfLastAutomaticRainbowColorReassignment, oldLuminance);
                colorToAssign_ifAutogenerated = SeededColorGenerator.GetRainbowColor(i, 1.0f, numberOfUserMadeLines_inMomentOfLastAutomaticRainbowColorReassignment, newLuminance);
                ReassignLuminanceToLineColor(userMadeLines[i], colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);
            }

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_premadeLine_float, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_premadeLine_float, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_float, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_premadeLine_int, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_premadeLine_int, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_int, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_vector2_x, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_vector2_y, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_vector3_x, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_vector3_y, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_vector3_z, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_vector4_x, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_vector4_y, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_vector4_z, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_wDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_wDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_vector4_w, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            //premadeLine_color_r -> doesn't change it's luminance
            //premadeLine_color_g -> doesn't change it's luminance
            //premadeLine_color_b -> doesn't change it's luminance
            //premadeLine_color_a -> doesn't change it's luminance

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_rotation_eulerX, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_rotation_eulerY, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_rotation_eulerZ, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_premadeLine_bool, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_premadeLine_bool, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_bool, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            //Non-list transform lines:
            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_transform_localPosition_x, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_transform_localPosition_y, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_transform_localPosition_z, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_transform_localEulerAngle_x, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_transform_localEulerAngle_y, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_transform_localEulerAngle_z, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_transform_localScale_x, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_transform_localScale_y, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_transform_localScale_z, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_transform_globalPosition_x, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_transform_globalPosition_y, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_transform_globalPosition_z, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_transform_globalEulerAngle_x, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_transform_globalEulerAngle_y, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_transform_globalEulerAngle_z, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_xDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_transform_lossyScale_x, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_yDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_transform_lossyScale_y, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, oldLuminance);
            colorToAssign_ifAutogenerated = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(default_colorHue_of_zDimension, newLuminance);
            ReassignLuminanceToLineColor(premadeLine_transform_lossyScale_z, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, colorToAssign_ifAutogenerated, newLuminance);

            //for listOfLines: no check if user has overwritten the line color. Always force to rainbowColorWithLuminance:
            ReassignListsLineColors(preMadeListOfLines_float, UtilitiesDXXL_Math.DimensionNullable.none, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_int, UtilitiesDXXL_Math.DimensionNullable.none, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_vector2_x, UtilitiesDXXL_Math.DimensionNullable.x, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_vector2_y, UtilitiesDXXL_Math.DimensionNullable.y, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_vector3_x, UtilitiesDXXL_Math.DimensionNullable.x, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_vector3_y, UtilitiesDXXL_Math.DimensionNullable.y, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_vector3_z, UtilitiesDXXL_Math.DimensionNullable.z, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_rotation_eulerX, UtilitiesDXXL_Math.DimensionNullable.x, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_rotation_eulerY, UtilitiesDXXL_Math.DimensionNullable.y, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_rotation_eulerZ, UtilitiesDXXL_Math.DimensionNullable.z, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_bool, UtilitiesDXXL_Math.DimensionNullable.none, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_transforms_localPosition_x, UtilitiesDXXL_Math.DimensionNullable.x, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_transforms_localPosition_y, UtilitiesDXXL_Math.DimensionNullable.y, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_transforms_localPosition_z, UtilitiesDXXL_Math.DimensionNullable.z, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_transforms_localEulerAngle_x, UtilitiesDXXL_Math.DimensionNullable.x, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_transforms_localEulerAngle_y, UtilitiesDXXL_Math.DimensionNullable.y, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_transforms_localEulerAngle_z, UtilitiesDXXL_Math.DimensionNullable.z, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_transforms_localScale_x, UtilitiesDXXL_Math.DimensionNullable.x, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_transforms_localScale_y, UtilitiesDXXL_Math.DimensionNullable.y, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_transforms_localScale_z, UtilitiesDXXL_Math.DimensionNullable.z, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_transforms_globalPosition_x, UtilitiesDXXL_Math.DimensionNullable.x, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_transforms_globalPosition_y, UtilitiesDXXL_Math.DimensionNullable.y, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_transforms_globalPosition_z, UtilitiesDXXL_Math.DimensionNullable.z, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_transforms_globalEulerAngle_x, UtilitiesDXXL_Math.DimensionNullable.x, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_transforms_globalEulerAngle_y, UtilitiesDXXL_Math.DimensionNullable.y, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_transforms_globalEulerAngle_z, UtilitiesDXXL_Math.DimensionNullable.z, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_transforms_lossyScale_x, UtilitiesDXXL_Math.DimensionNullable.x, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_transforms_lossyScale_y, UtilitiesDXXL_Math.DimensionNullable.y, newLuminance);
            ReassignListsLineColors(preMadeListOfLines_transforms_lossyScale_z, UtilitiesDXXL_Math.DimensionNullable.z, newLuminance);
        }

        void ReassignLuminanceToLineColor(ChartLine concernedLine, Color colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated, Color colorToAssign_ifAutogenerated, float newLuminance)
        {
            bool colorWasAutogenerated = UtilitiesDXXL_Colors.IsApproxSameColor(concernedLine.Color, colorThisLineShouldHaveAtPrevLuminance_beforeCurrentColorReassignment_ifAutogenerated);
            if (colorWasAutogenerated)
            {
                concernedLine.Color = colorToAssign_ifAutogenerated;
            }
            else
            {
                //color has been manually set by user:
                if (UtilitiesDXXL_Math.ApproximatelyZero(concernedLine.Color.r) && UtilitiesDXXL_Math.ApproximatelyZero(concernedLine.Color.g) && UtilitiesDXXL_Math.ApproximatelyZero(concernedLine.Color.b))
                {
                    //-> black colors cannot be forced with luminance, therefore: slight lift, to make it grey, which can be forced:
                    concernedLine.Color = new Color(0.01f, 0.01f, 0.01f, concernedLine.Color.a);
                }
                concernedLine.Color = SeededColorGenerator.ForceApproxLuminance(concernedLine.Color, newLuminance);
            }
        }

        void ReassignListsLineColors(List<ChartLine> concernedPremadeListOfLines, UtilitiesDXXL_Math.DimensionNullable vectorDimensionThisListOfLinesRepresents, float usedLuminance)
        {
            if (concernedPremadeListOfLines != null)
            {
                for (int i = 0; i < concernedPremadeListOfLines.Count; i++)
                {
                    switch (vectorDimensionThisListOfLinesRepresents)
                    {
                        case UtilitiesDXXL_Math.DimensionNullable.none:
                            concernedPremadeListOfLines[i].Color = SeededColorGenerator.GetRainbowColor(i, 1.0f, concernedPremadeListOfLines.Count, usedLuminance);
                            break;
                        case UtilitiesDXXL_Math.DimensionNullable.x:
                            concernedPremadeListOfLines[i].Color = SeededColorGenerator.GetRainbowColorAroundRed(i, 1.0f, concernedPremadeListOfLines.Count, false, usedLuminance);
                            break;
                        case UtilitiesDXXL_Math.DimensionNullable.y:
                            concernedPremadeListOfLines[i].Color = SeededColorGenerator.GetRainbowColorAroundGreen(i, 1.0f, concernedPremadeListOfLines.Count, false, usedLuminance);
                            break;
                        case UtilitiesDXXL_Math.DimensionNullable.z:
                            concernedPremadeListOfLines[i].Color = SeededColorGenerator.GetRainbowColorAroundBlue(i, 1.0f, concernedPremadeListOfLines.Count, false, usedLuminance);
                            break;
                        default:
                            UtilitiesDXXL_Log.PrintErrorCode("8");
                            concernedPremadeListOfLines[i].Color = SeededColorGenerator.GetRainbowColor(i, 1.0f, concernedPremadeListOfLines.Count, usedLuminance);
                            break;
                    }
                }
            }
        }

        public bool HasAtLeastOneDrawnLineWithAtLeastOneValidValue()
        {
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                if (preMadeLines[i].HasAtLeastOneValuePairOfValidData && preMadeLines[i].CheckIfLineIsDrawn())
                {
                    return true;
                }
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                if (userMadeLines[i].HasAtLeastOneValuePairOfValidData && userMadeLines[i].CheckIfLineIsDrawn())
                {
                    return true;
                }
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    if (listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].HasAtLeastOneValuePairOfValidData && listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].CheckIfLineIsDrawn())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public float GetMostCurrentXValueOfAllLines()
        {
            float mostCurrentXValue_ofAllLines = float.NegativeInfinity;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                if (preMadeLines[i].HasAtLeastOneValuePairOfValidData && preMadeLines[i].CheckIfLineIsDrawn())
                {
                    mostCurrentXValue_ofAllLines = Mathf.Max(mostCurrentXValue_ofAllLines, preMadeLines[i].GetMostCurrentValidXValue());
                }
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                if (userMadeLines[i].HasAtLeastOneValuePairOfValidData && userMadeLines[i].CheckIfLineIsDrawn())
                {
                    mostCurrentXValue_ofAllLines = Mathf.Max(mostCurrentXValue_ofAllLines, userMadeLines[i].GetMostCurrentValidXValue());
                }
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    if (listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].HasAtLeastOneValuePairOfValidData && listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].CheckIfLineIsDrawn())
                    {
                        mostCurrentXValue_ofAllLines = Mathf.Max(mostCurrentXValue_ofAllLines, listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].GetMostCurrentValidXValue());
                    }
                }
            }
            return mostCurrentXValue_ofAllLines;
        }

        public float GetMostCurrentYValueOfAllLines()
        {
            float mostCurrentYValue_ofAllLines = float.NegativeInfinity;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                if (preMadeLines[i].HasAtLeastOneValuePairOfValidData && preMadeLines[i].CheckIfLineIsDrawn())
                {
                    mostCurrentYValue_ofAllLines = Mathf.Max(mostCurrentYValue_ofAllLines, preMadeLines[i].GetMostCurrentValidYValue());
                }
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                if (userMadeLines[i].HasAtLeastOneValuePairOfValidData && userMadeLines[i].CheckIfLineIsDrawn())
                {
                    mostCurrentYValue_ofAllLines = Mathf.Max(mostCurrentYValue_ofAllLines, userMadeLines[i].GetMostCurrentValidYValue());
                }
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    if (listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].HasAtLeastOneValuePairOfValidData && listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].CheckIfLineIsDrawn())
                    {
                        mostCurrentYValue_ofAllLines = Mathf.Max(mostCurrentYValue_ofAllLines, listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].GetMostCurrentValidYValue());
                    }
                }
            }
            return mostCurrentYValue_ofAllLines;
        }

        public float GetLowestXValueOfAllLines()
        {
            //see also "chart_theseLinesArePartOf.overallMinXValue_includingHiddenLines", which is similar, but also includes hidden lines
            float lowestXValue_ofAllLines = float.PositiveInfinity;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                if (preMadeLines[i].HasAtLeastOneValuePairOfValidData && preMadeLines[i].CheckIfLineIsDrawn())
                {
                    lowestXValue_ofAllLines = Mathf.Min(lowestXValue_ofAllLines, preMadeLines[i].GetLowestXValue());
                }
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                if (userMadeLines[i].HasAtLeastOneValuePairOfValidData && userMadeLines[i].CheckIfLineIsDrawn())
                {
                    lowestXValue_ofAllLines = Mathf.Min(lowestXValue_ofAllLines, userMadeLines[i].GetLowestXValue());
                }
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    if (listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].HasAtLeastOneValuePairOfValidData && listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].CheckIfLineIsDrawn())
                    {
                        lowestXValue_ofAllLines = Mathf.Min(lowestXValue_ofAllLines, listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].GetLowestXValue());
                    }
                }
            }
            return lowestXValue_ofAllLines;
        }

        public float GetLowestYValueOfAllLines()
        {
            //see also "chart_theseLinesArePartOf.overallMinYValue_includingHiddenLines", which is similar, but also includes hidden lines
            float lowestYValue_ofAllLines = float.PositiveInfinity;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                if (preMadeLines[i].HasAtLeastOneValuePairOfValidData && preMadeLines[i].CheckIfLineIsDrawn())
                {
                    lowestYValue_ofAllLines = Mathf.Min(lowestYValue_ofAllLines, preMadeLines[i].GetLowestYValue());
                }
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                if (userMadeLines[i].HasAtLeastOneValuePairOfValidData && userMadeLines[i].CheckIfLineIsDrawn())
                {
                    lowestYValue_ofAllLines = Mathf.Min(lowestYValue_ofAllLines, userMadeLines[i].GetLowestYValue());
                }
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    if (listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].HasAtLeastOneValuePairOfValidData && listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].CheckIfLineIsDrawn())
                    {
                        lowestYValue_ofAllLines = Mathf.Min(lowestYValue_ofAllLines, listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].GetLowestYValue());
                    }
                }
            }
            return lowestYValue_ofAllLines;
        }

        public float GetHighestXValueOfAllLines()
        {
            //see also "chart_theseLinesArePartOf.overallMaxXValue_includingHiddenLines", which is similar, but also includes hidden lines
            float highestXValue_ofAllLines = float.NegativeInfinity;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                if (preMadeLines[i].HasAtLeastOneValuePairOfValidData && preMadeLines[i].CheckIfLineIsDrawn())
                {
                    highestXValue_ofAllLines = Mathf.Max(highestXValue_ofAllLines, preMadeLines[i].GetHighestXValue());
                }
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                if (userMadeLines[i].HasAtLeastOneValuePairOfValidData && userMadeLines[i].CheckIfLineIsDrawn())
                {
                    highestXValue_ofAllLines = Mathf.Max(highestXValue_ofAllLines, userMadeLines[i].GetHighestXValue());
                }
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    if (listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].HasAtLeastOneValuePairOfValidData && listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].CheckIfLineIsDrawn())
                    {
                        highestXValue_ofAllLines = Mathf.Max(highestXValue_ofAllLines, listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].GetHighestXValue());
                    }
                }
            }
            return highestXValue_ofAllLines;
        }

        public float GetHighestYValueOfAllLines()
        {
            //see also "chart_theseLinesArePartOf.overallMaxYValue_includingHiddenLines", which is similar, but also includes hidden lines
            float highestYValue_ofAllLines = float.NegativeInfinity;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                if (preMadeLines[i].HasAtLeastOneValuePairOfValidData && preMadeLines[i].CheckIfLineIsDrawn())
                {
                    highestYValue_ofAllLines = Mathf.Max(highestYValue_ofAllLines, preMadeLines[i].GetHighestYValue());
                }
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                if (userMadeLines[i].HasAtLeastOneValuePairOfValidData && userMadeLines[i].CheckIfLineIsDrawn())
                {
                    highestYValue_ofAllLines = Mathf.Max(highestYValue_ofAllLines, userMadeLines[i].GetHighestYValue());
                }
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    if (listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].HasAtLeastOneValuePairOfValidData && listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].CheckIfLineIsDrawn())
                    {
                        highestYValue_ofAllLines = Mathf.Max(highestYValue_ofAllLines, listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].GetHighestYValue());
                    }
                }
            }
            return highestYValue_ofAllLines;
        }

        public float GetLowestXValueOfAllLines_insideRestricedYSpan(float minAllowedY, float maxAllowedY)
        {
            float lowestXValue_ofAllLines = float.PositiveInfinity;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                if (preMadeLines[i].HasAtLeastOneValuePairOfValidData && preMadeLines[i].CheckIfLineIsDrawn())
                {
                    lowestXValue_ofAllLines = Mathf.Min(lowestXValue_ofAllLines, preMadeLines[i].GetLowestXValue_insideRestricedYSpan(minAllowedY,  maxAllowedY));
                }
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                if (userMadeLines[i].HasAtLeastOneValuePairOfValidData && userMadeLines[i].CheckIfLineIsDrawn())
                {
                    lowestXValue_ofAllLines = Mathf.Min(lowestXValue_ofAllLines, userMadeLines[i].GetLowestXValue_insideRestricedYSpan(minAllowedY, maxAllowedY));
                }
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    if (listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].HasAtLeastOneValuePairOfValidData && listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].CheckIfLineIsDrawn())
                    {
                        lowestXValue_ofAllLines = Mathf.Min(lowestXValue_ofAllLines, listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].GetLowestXValue_insideRestricedYSpan(minAllowedY, maxAllowedY));
                    }
                }
            }
            return lowestXValue_ofAllLines;
        }

        public float GetLowestYValueOfAllLines_insideRestricedXSpan(float minAllowedX, float maxAllowedX)
        {
            float lowestYValue_ofAllLines = float.PositiveInfinity;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                if (preMadeLines[i].HasAtLeastOneValuePairOfValidData && preMadeLines[i].CheckIfLineIsDrawn())
                {
                    lowestYValue_ofAllLines = Mathf.Min(lowestYValue_ofAllLines, preMadeLines[i].GetLowestYValue_insideRestricedXSpan(minAllowedX, maxAllowedX));
                }
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                if (userMadeLines[i].HasAtLeastOneValuePairOfValidData && userMadeLines[i].CheckIfLineIsDrawn())
                {
                    lowestYValue_ofAllLines = Mathf.Min(lowestYValue_ofAllLines, userMadeLines[i].GetLowestYValue_insideRestricedXSpan(minAllowedX, maxAllowedX));
                }
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    if (listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].HasAtLeastOneValuePairOfValidData && listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].CheckIfLineIsDrawn())
                    {
                        lowestYValue_ofAllLines = Mathf.Min(lowestYValue_ofAllLines, listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].GetLowestYValue_insideRestricedXSpan(minAllowedX, maxAllowedX));
                    }
                }
            }
            return lowestYValue_ofAllLines;
        }

        public float GetHighestXValueOfAllLines_insideRestricedYSpan(float minAllowedY, float maxAllowedY)
        {
            float highestXValue_ofAllLines = float.NegativeInfinity;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                if (preMadeLines[i].HasAtLeastOneValuePairOfValidData && preMadeLines[i].CheckIfLineIsDrawn())
                {
                    highestXValue_ofAllLines = Mathf.Max(highestXValue_ofAllLines, preMadeLines[i].GetHighestXValue_insideRestricedYSpan(minAllowedY, maxAllowedY));
                }
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                if (userMadeLines[i].HasAtLeastOneValuePairOfValidData && userMadeLines[i].CheckIfLineIsDrawn())
                {
                    highestXValue_ofAllLines = Mathf.Max(highestXValue_ofAllLines, userMadeLines[i].GetHighestXValue_insideRestricedYSpan(minAllowedY, maxAllowedY));
                }
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    if (listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].HasAtLeastOneValuePairOfValidData && listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].CheckIfLineIsDrawn())
                    {
                        highestXValue_ofAllLines = Mathf.Max(highestXValue_ofAllLines, listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].GetHighestXValue_insideRestricedYSpan(minAllowedY, maxAllowedY));
                    }
                }
            }
            return highestXValue_ofAllLines;
        }

        public float GetHighestYValueOfAllLines_insideRestricedXSpan(float minAllowedX, float maxAllowedX)
        {
            float highestYValue_ofAllLines = float.NegativeInfinity;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                if (preMadeLines[i].HasAtLeastOneValuePairOfValidData && preMadeLines[i].CheckIfLineIsDrawn())
                {
                    highestYValue_ofAllLines = Mathf.Max(highestYValue_ofAllLines, preMadeLines[i].GetHighestYValue_insideRestricedXSpan(minAllowedX, maxAllowedX));
                }
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                if (userMadeLines[i].HasAtLeastOneValuePairOfValidData && userMadeLines[i].CheckIfLineIsDrawn())
                {
                    highestYValue_ofAllLines = Mathf.Max(highestYValue_ofAllLines, userMadeLines[i].GetHighestYValue_insideRestricedXSpan(minAllowedX, maxAllowedX));
                }
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    if (listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].HasAtLeastOneValuePairOfValidData && listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].CheckIfLineIsDrawn())
                    {
                        highestYValue_ofAllLines = Mathf.Max(highestYValue_ofAllLines, listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].GetHighestYValue_insideRestricedXSpan(minAllowedX, maxAllowedX));
                    }
                }
            }
            return highestYValue_ofAllLines;
        }

        void TrySetPointsOfInterest_forChangingGameobjectsInCollectionSlots(List<GameObject> yValues)
        {
            for (int i_newlyAddedValueListSlot = 0; i_newlyAddedValueListSlot < yValues.Count; i_newlyAddedValueListSlot++)
            {
                TrySetPointOfInterest_forChangingGameobjectInCollectionSlot(yValues[i_newlyAddedValueListSlot], i_newlyAddedValueListSlot);
            }
        }

        void TrySetPointsOfInterest_forChangingGameobjectsInCollectionSlots(GameObject[] yValues)
        {
            for (int i_newlyAddedValueListSlot = 0; i_newlyAddedValueListSlot < yValues.Length; i_newlyAddedValueListSlot++)
            {
                TrySetPointOfInterest_forChangingGameobjectInCollectionSlot(yValues[i_newlyAddedValueListSlot], i_newlyAddedValueListSlot);
            }
        }

        void TrySetPointsOfInterest_forChangingGameobjectsInCollectionSlots(List<Transform> yValues)
        {
            for (int i_newlyAddedValueListSlot = 0; i_newlyAddedValueListSlot < yValues.Count; i_newlyAddedValueListSlot++)
            {
                TrySetPointOfInterest_forChangingGameobjectInCollectionSlot(yValues[i_newlyAddedValueListSlot].gameObject, i_newlyAddedValueListSlot);
            }
        }

        void TrySetPointsOfInterest_forChangingGameobjectsInCollectionSlots(Transform[] yValues)
        {
            for (int i_newlyAddedValueListSlot = 0; i_newlyAddedValueListSlot < yValues.Length; i_newlyAddedValueListSlot++)
            {
                TrySetPointOfInterest_forChangingGameobjectInCollectionSlot(yValues[i_newlyAddedValueListSlot].gameObject, i_newlyAddedValueListSlot);
            }
        }

        void TrySetPointOfInterest_forChangingGameobjectInCollectionSlot(GameObject gameobjectInCheckedSlotOf_newlyAddedValues, int i_newlyAddedValueListSlot)
        {
            if (gameobjectInCheckedSlotOf_newlyAddedValues != null)
            {
                //-> using "preMadeListOfLines_transforms_localPosition_x" as representative of all "preMadeListOfLines_transforms_*"
                if (preMadeListOfLines_transforms_localPosition_x != null)
                {
                    if (i_newlyAddedValueListSlot < preMadeListOfLines_transforms_localPosition_x.Count)
                    {
                        if (preMadeListOfLines_transforms_localPosition_x[i_newlyAddedValueListSlot] != null)
                        {
                            if (preMadeListOfLines_transforms_localPosition_x[i_newlyAddedValueListSlot].Gameobject_thatThisLineCurrentlyRepresents != null)
                            {
                                if (gameobjectInCheckedSlotOf_newlyAddedValues != preMadeListOfLines_transforms_localPosition_x[i_newlyAddedValueListSlot].Gameobject_thatThisLineCurrentlyRepresents)
                                {
                                    //-> Only setting the notification point. For the reassignment of '.Gameobject_thatThisLineCurrentlyRepresents' will 'AddValueCollectionSlots_toLines()' take care.
                                    SetPointOfInterest_forChangingGameobjectInCollectionSlot(gameobjectInCheckedSlotOf_newlyAddedValues, i_newlyAddedValueListSlot);
                                }
                            }
                        }
                    }
                }
            }
        }

        void SetPointOfInterest_forChangingGameobjectInCollectionSlot(GameObject gameobjectInCheckedSlotOf_newlyAddedValues, int i_newlyAddedValueListSlot)
        {
            Vector2 position = new Vector2(preMadeListOfLines_transforms_localPosition_x[i_newlyAddedValueListSlot].GetCurrentAutomaticXValue(), float.NaN);
            string textToDisplay = "<color=#adadadFF><icon=logMessage></color> 'i=" + i_newlyAddedValueListSlot + "':<br>A new Gameobject delivers the values:<br>" + gameobjectInCheckedSlotOf_newlyAddedValues.name + "<br>Up to now the values came from:<br>" + preMadeListOfLines_transforms_localPosition_x[i_newlyAddedValueListSlot].Gameobject_thatThisLineCurrentlyRepresents.name;
            DrawBasics.LineStyle horizLinestyle = DrawBasics.LineStyle.invisible;
            DrawBasics.LineStyle vertLinestyle = DrawBasics.LineStyle.solid;
            float alphaOfColor_relToParent = 1.0f;
            bool getsDeletedOnClear = true;
            PointOfInterest pointOfInterest_indicatingTheChangeOfTheGameobjectInsideACollectionSlot = chart_theseLinesArePartOf.AddPointOfInterest(position, textToDisplay, horizLinestyle, vertLinestyle, alphaOfColor_relToParent, getsDeletedOnClear);
            pointOfInterest_indicatingTheChangeOfTheGameobjectInsideACollectionSlot.drawTextBoxIfPointIsOutsideOfChartArea = true;
            pointOfInterest_indicatingTheChangeOfTheGameobjectInsideACollectionSlot.forceColorOfParent = true;
            pointOfInterest_indicatingTheChangeOfTheGameobjectInsideACollectionSlot.xValue.lineExtent = DimensionOf_PointOfInterest.LineExtent.throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart;

            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(preMadeListOfLines_transforms_localPosition_x[i_newlyAddedValueListSlot]);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(preMadeListOfLines_transforms_localPosition_y[i_newlyAddedValueListSlot]);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(preMadeListOfLines_transforms_localPosition_z[i_newlyAddedValueListSlot]);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(preMadeListOfLines_transforms_localEulerAngle_x[i_newlyAddedValueListSlot]);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(preMadeListOfLines_transforms_localEulerAngle_y[i_newlyAddedValueListSlot]);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(preMadeListOfLines_transforms_localEulerAngle_z[i_newlyAddedValueListSlot]);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(preMadeListOfLines_transforms_localScale_x[i_newlyAddedValueListSlot]);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(preMadeListOfLines_transforms_localScale_y[i_newlyAddedValueListSlot]);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(preMadeListOfLines_transforms_localScale_z[i_newlyAddedValueListSlot]);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(preMadeListOfLines_transforms_globalPosition_x[i_newlyAddedValueListSlot]);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(preMadeListOfLines_transforms_globalPosition_y[i_newlyAddedValueListSlot]);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(preMadeListOfLines_transforms_globalPosition_z[i_newlyAddedValueListSlot]);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(preMadeListOfLines_transforms_globalEulerAngle_x[i_newlyAddedValueListSlot]);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(preMadeListOfLines_transforms_globalEulerAngle_y[i_newlyAddedValueListSlot]);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(preMadeListOfLines_transforms_globalEulerAngle_z[i_newlyAddedValueListSlot]);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(preMadeListOfLines_transforms_lossyScale_x[i_newlyAddedValueListSlot]);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(preMadeListOfLines_transforms_lossyScale_y[i_newlyAddedValueListSlot]);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(preMadeListOfLines_transforms_lossyScale_z[i_newlyAddedValueListSlot]);
        }

        void AddPointOfInterestForChangingValueSourceGameobject(float xPos, GameObject theOldGameobject, GameObject theNewGameobject)
        {
            Color colorOfVertLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(chart_theseLinesArePartOf.color, 0.75f);
            PointOfInterest pointOfInterest_visualizingTheChangingGameobject = new PointOfInterest(xPos, 0.0f, colorOfVertLine, chart_theseLinesArePartOf, null, null);
            pointOfInterest_visualizingTheChangingGameobject.isDeletedOnClear = true;
            pointOfInterest_visualizingTheChangingGameobject.forceColorOfParent = true;
            pointOfInterest_visualizingTheChangingGameobject.xValue.lineStyle = DrawBasics.LineStyle.solid;
            pointOfInterest_visualizingTheChangingGameobject.xValue.labelText = "<size=5><color=#adadadFF><icon=logMessage></color> A new Gameobject delivers the values: '" + theNewGameobject.name + "'. Up to now the values came from '" + theOldGameobject.name + "'</size>";
            pointOfInterest_visualizingTheChangingGameobject.xValue.drawCoordinateAsText = true;
            pointOfInterest_visualizingTheChangingGameobject.xValue.lineExtent = DimensionOf_PointOfInterest.LineExtent.throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart;
            pointOfInterest_visualizingTheChangingGameobject.yValue.lineStyle = DrawBasics.LineStyle.invisible;
            chart_theseLinesArePartOf.AddPointOfInterest(pointOfInterest_visualizingTheChangingGameobject);

            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(premadeLine_transform_localPosition_x);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(premadeLine_transform_localPosition_y);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(premadeLine_transform_localPosition_z);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(premadeLine_transform_localEulerAngle_x);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(premadeLine_transform_localEulerAngle_y);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(premadeLine_transform_localEulerAngle_z);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(premadeLine_transform_localScale_x);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(premadeLine_transform_localScale_y);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(premadeLine_transform_localScale_z);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(premadeLine_transform_globalPosition_x);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(premadeLine_transform_globalPosition_y);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(premadeLine_transform_globalPosition_z);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(premadeLine_transform_globalEulerAngle_x);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(premadeLine_transform_globalEulerAngle_y);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(premadeLine_transform_globalEulerAngle_z);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(premadeLine_transform_lossyScale_x);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(premadeLine_transform_lossyScale_y);
            MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(premadeLine_transform_lossyScale_z);
        }

        void MarkTransitionAndDiscardTurningPointsDetection_forLineWhoseRepresentedGameobjectChanged(ChartLine concernedLine)
        {
            concernedLine.AddEmphasizingCircleToMostCurrentPoint(true, true);
            concernedLine.ForceUpcomingNextCreatedConnectionLine_toLowAlpha();
            concernedLine.turningPointDetector.DiscardPrecedingPointsFromComparison();
        }

        public List<ChartLine> Get_all_hiddenAndUnhiddenLines_withAtLeastOneValidOrInvalidDatapoint(bool includeLinesThatRepresentDisabledMultiComponentComponents = false)
        {
            return Get_all_hiddenAndUnhiddenLines_withAtLeastOneValidOrInvalidDatapoint(out int numberOfDatapoints_inLongestLine, includeLinesThatRepresentDisabledMultiComponentComponents);
        }

        public List<ChartLine> Get_all_hiddenAndUnhiddenLines_withAtLeastOneValidOrInvalidDatapoint(out int numberOfDatapoints_inLongestLine, bool includeLinesThatRepresentDisabledMultiComponentComponents = false)
        {
            List<ChartLine> list_with_allLinesWithAtLeastOneDataPoint_validOrInvalid = new List<ChartLine>();
            numberOfDatapoints_inLongestLine = 0;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                if (preMadeLines[i].IsHiddenOrUnhidden_butWithAtLeastOneValidOrInvalidDatapoint(includeLinesThatRepresentDisabledMultiComponentComponents))
                {
                    numberOfDatapoints_inLongestLine = Mathf.Max(numberOfDatapoints_inLongestLine, preMadeLines[i].dataPoints.Count);
                    list_with_allLinesWithAtLeastOneDataPoint_validOrInvalid.Add(preMadeLines[i]);
                }
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                if (userMadeLines[i].IsHiddenOrUnhidden_butWithAtLeastOneValidOrInvalidDatapoint(includeLinesThatRepresentDisabledMultiComponentComponents))
                {
                    numberOfDatapoints_inLongestLine = Mathf.Max(numberOfDatapoints_inLongestLine, userMadeLines[i].dataPoints.Count);
                    list_with_allLinesWithAtLeastOneDataPoint_validOrInvalid.Add(userMadeLines[i]);
                }
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    if (listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].IsHiddenOrUnhidden_butWithAtLeastOneValidOrInvalidDatapoint(includeLinesThatRepresentDisabledMultiComponentComponents))
                    {
                        numberOfDatapoints_inLongestLine = Mathf.Max(numberOfDatapoints_inLongestLine, listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].dataPoints.Count);
                        list_with_allLinesWithAtLeastOneDataPoint_validOrInvalid.Add(listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList]);
                    }
                }
            }
            return list_with_allLinesWithAtLeastOneDataPoint_validOrInvalid;
        }

        public int Get_numberOf_allHiddenAndUnhiddenLines_withAtLeastOneValidOrInvalidDatapoint(bool includeLinesThatRepresentDisabledMultiComponentComponents = false)
        {
            int numberOfAllLinesWithAtLeastOneDataPoint_validOrInvalid = 0;
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                if (preMadeLines[i].IsHiddenOrUnhidden_butWithAtLeastOneValidOrInvalidDatapoint(includeLinesThatRepresentDisabledMultiComponentComponents))
                {
                    numberOfAllLinesWithAtLeastOneDataPoint_validOrInvalid++;
                }
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                if (userMadeLines[i].IsHiddenOrUnhidden_butWithAtLeastOneValidOrInvalidDatapoint(includeLinesThatRepresentDisabledMultiComponentComponents))
                {
                    numberOfAllLinesWithAtLeastOneDataPoint_validOrInvalid++;
                }
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    if (listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].IsHiddenOrUnhidden_butWithAtLeastOneValidOrInvalidDatapoint(includeLinesThatRepresentDisabledMultiComponentComponents))
                    {
                        numberOfAllLinesWithAtLeastOneDataPoint_validOrInvalid++;
                    }
                }
            }
            return numberOfAllLinesWithAtLeastOneDataPoint_validOrInvalid;
        }

        public void InitInspectionViaComponent()
        {
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                preMadeLines[i].InitInspectionViaComponent();
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                userMadeLines[i].InitInspectionViaComponent();
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].InitInspectionViaComponent();
                }
            }
        }

        public ChartLine.NamePosition GetAUsedLineNamePosition(bool includeLinesThatRepresentDisabledMultiComponentComponents = false)
        {
            //First: only try unhidden lines:
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                if (preMadeLines[i].IsUnhidden_andWithAtLeastOneValidOrInvalidDatapoint(includeLinesThatRepresentDisabledMultiComponentComponents))
                {
                    return preMadeLines[i].namePosition;
                }
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                if (userMadeLines[i].IsUnhidden_andWithAtLeastOneValidOrInvalidDatapoint(includeLinesThatRepresentDisabledMultiComponentComponents))
                {
                    return userMadeLines[i].namePosition;
                }
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    if (listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].IsUnhidden_andWithAtLeastOneValidOrInvalidDatapoint(includeLinesThatRepresentDisabledMultiComponentComponents))
                    {
                        return listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].namePosition;
                    }
                }
            }

            //Second: Also try hidden lines:
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                if (preMadeLines[i].IsHiddenOrUnhidden_butWithAtLeastOneValidOrInvalidDatapoint(includeLinesThatRepresentDisabledMultiComponentComponents))
                {
                    return preMadeLines[i].namePosition;
                }
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                if (userMadeLines[i].IsHiddenOrUnhidden_butWithAtLeastOneValidOrInvalidDatapoint(includeLinesThatRepresentDisabledMultiComponentComponents))
                {
                    return userMadeLines[i].namePosition;
                }
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    if (listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].IsHiddenOrUnhidden_butWithAtLeastOneValidOrInvalidDatapoint(includeLinesThatRepresentDisabledMultiComponentComponents))
                    {
                        return listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].namePosition;
                    }
                }
            }

            //Third: Fallback
            return ChartLine.NamePosition.dynamicallyMoving_atLineEnd_towardsRight;
        }


        public float GetAUsedLineNameSizeSclaeFactor(bool includeLinesThatRepresentDisabledMultiComponentComponents = false)
        {
            //First: only try unhidden lines:
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                if (preMadeLines[i].IsUnhidden_andWithAtLeastOneValidOrInvalidDatapoint(includeLinesThatRepresentDisabledMultiComponentComponents))
                {
                    return preMadeLines[i].NameText_sizeScaleFactor;
                }
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                if (userMadeLines[i].IsUnhidden_andWithAtLeastOneValidOrInvalidDatapoint(includeLinesThatRepresentDisabledMultiComponentComponents))
                {
                    return userMadeLines[i].NameText_sizeScaleFactor;
                }
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    if (listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].IsUnhidden_andWithAtLeastOneValidOrInvalidDatapoint(includeLinesThatRepresentDisabledMultiComponentComponents))
                    {
                        return listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].NameText_sizeScaleFactor;
                    }
                }
            }

            //Second: Also try hidden lines:
            for (int i = 0; i < preMadeLines.Count; i++)
            {
                if (preMadeLines[i].IsHiddenOrUnhidden_butWithAtLeastOneValidOrInvalidDatapoint(includeLinesThatRepresentDisabledMultiComponentComponents))
                {
                    return preMadeLines[i].NameText_sizeScaleFactor;
                }
            }
            for (int i = 0; i < userMadeLines.Count; i++)
            {
                if (userMadeLines[i].IsHiddenOrUnhidden_butWithAtLeastOneValidOrInvalidDatapoint(includeLinesThatRepresentDisabledMultiComponentComponents))
                {
                    return userMadeLines[i].NameText_sizeScaleFactor;
                }
            }
            for (int i_listOfLines = 0; i_listOfLines < listOfPremadeListsOfLines.Count; i_listOfLines++)
            {
                for (int i_lineInsideCurrList = 0; i_lineInsideCurrList < listOfPremadeListsOfLines[i_listOfLines].Count; i_lineInsideCurrList++)
                {
                    if (listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].IsHiddenOrUnhidden_butWithAtLeastOneValidOrInvalidDatapoint(includeLinesThatRepresentDisabledMultiComponentComponents))
                    {
                        return listOfPremadeListsOfLines[i_listOfLines][i_lineInsideCurrList].NameText_sizeScaleFactor;
                    }
                }
            }

            //Third: Fallback
            return 1.0f;
        }

    }

}
