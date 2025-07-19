namespace DrawXXL
{
    using UnityEngine;

    public class UtilitiesDXXL_Colors
    {
        public static Color violet = new Color(0.9372549f, 0.4196078f, 0.6156863f, 1f);
        public static Color darkBlue = new Color(0.1372549f, 0.1058824f, 0.7176471f, 1f);
        public static Color red_xAxis = new Color(0.9607843f, 0.282353f, 0.1607843f, 0.9333333f);
        public static Color green_yAxis = new Color(0.6784314f, 0.9568627f, 0.3058824f, 0.9333333f);
        public static Color blue_zAxis = new Color(0.2470588f, 0.5176471f, 0.9882353f, 1f);
        public static Color red_xAxisAlpha1 = new Color(0.9607843f, 0.282353f, 0.1607843f, 1f);
        public static Color green_yAxisAlpha1 = new Color(0.6784314f, 0.9568627f, 0.3058824f, 1f);
        public static Color blue_zAxisAlpha1 = new Color(0.2470588f, 0.5176471f, 0.9882353f, 1f);
        public static Color green_lineThresholdNearDistance = new Color(0.1215686f, 0.8745098f, 0.2078431f, 1f);
        public static Color orange_lineThresholdMiddleDistance = new Color(0.9960784f, 0.7490196f, 0f, 1f);
        public static Color red_lineThresholdFarDistance = new Color(0.9647059f, 0f, 0.1607843f, 1f);
        public static Color grey_logMessage = new Color(0.6784314f, 0.6784314f, 0.6784314f, 1f);
        public static Color yellow_logWarning = new Color(0.8862745f, 0.6666667f, 0f, 1f);
        public static Color red_logError = new Color(0.8078431f, 0.05490196f, 0.05490196f, 1f);
        public static Color purple_raycastHitTextDefault = new Color(0.2941177f, 0f, 0.3882353f, 1f);
        public static Color green_boolTrue = new Color(0.5137255f, 0.8862745f, 0.2901961f, 1f);
        public static Color red_boolFalse = new Color(0.9294118f, 0.2784314f, 0.1921569f, 1f);

        public enum ColorComponent { r, g, b, a };

        public static Color Get_color_butWithAdjustedAlpha(Color baseColor_forWhichToAdjustTheAlpha, float alphaFactor)
        {
            Color color_toReturn = new Color();

            color_toReturn.r = baseColor_forWhichToAdjustTheAlpha.r;
            color_toReturn.g = baseColor_forWhichToAdjustTheAlpha.g;
            color_toReturn.b = baseColor_forWhichToAdjustTheAlpha.b;
            color_toReturn.a = baseColor_forWhichToAdjustTheAlpha.a * alphaFactor;

            return color_toReturn;
        }

        public static Color Get_color_butWithFixedAlpha(Color baseColor_forWhichToSetTheAlpha, float fixedAlpha)
        {
            Color color_toReturn = new Color();

            color_toReturn.r = baseColor_forWhichToSetTheAlpha.r;
            color_toReturn.g = baseColor_forWhichToSetTheAlpha.g;
            color_toReturn.b = baseColor_forWhichToSetTheAlpha.b;
            color_toReturn.a = fixedAlpha;

            return color_toReturn;
        }

        public static Color Get_color_darkenedFromGivenColor(Color baseColor_toAdjust, float darkeningIntensity)
        {
            Color color_toReturn = new Color();

            color_toReturn.r = baseColor_toAdjust.r / darkeningIntensity;
            color_toReturn.g = baseColor_toAdjust.g / darkeningIntensity;
            color_toReturn.b = baseColor_toAdjust.b / darkeningIntensity;
            color_toReturn.a = baseColor_toAdjust.a;

            return color_toReturn;
        }

        public static Color Get_randomColorSeeded(int seed)
        {
            return Get_randomColorSeeded(seed, 1.0f, 0.0f);
        }

        public static Color Get_randomColorSeeded(int seed, float alphaToUse_0to1, float forceLuminance)
        {
            Color color_toReturn = new Color();

            float r_x = 1235.235f + 2.014f * (float)seed;
            float r_y = 2395.0911f + 3.599f * (float)seed;
            color_toReturn.r = Mathf.PerlinNoise(UtilitiesDXXL_Math.Loop_floatIntoSpanFrom_m1_to_p1(r_x) * 100.0f, UtilitiesDXXL_Math.Loop_floatIntoSpanFrom_m1_to_p1(r_y) * 100.0f);
            float g_x = 213.732f + 3.521f * (float)seed;
            float g_y = -35.9806f + 2.511f * (float)seed;
            color_toReturn.g = Mathf.PerlinNoise(UtilitiesDXXL_Math.Loop_floatIntoSpanFrom_m1_to_p1(g_x) * 100.0f, UtilitiesDXXL_Math.Loop_floatIntoSpanFrom_m1_to_p1(g_y) * 100.0f);
            float b_x = 355.5312f - 2.761f * (float)seed;
            float b_y = -299.1816f + 1.811f * (float)seed;
            color_toReturn.b = Mathf.PerlinNoise(UtilitiesDXXL_Math.Loop_floatIntoSpanFrom_m1_to_p1(b_x) * 100.0f, UtilitiesDXXL_Math.Loop_floatIntoSpanFrom_m1_to_p1(b_y) * 100.0f);

            ColorComponent biggestComponent = GetBiggestComponentRGB(color_toReturn);
            ColorComponent smallestComponent = GetSmallestComponentRGB(color_toReturn);

            color_toReturn = color_toReturn * color_toReturn * 1.0f;
            switch (biggestComponent)
            {
                case ColorComponent.r:
                    color_toReturn.r = 0.5f * (color_toReturn.r + 1.0f);
                    break;

                case ColorComponent.g:
                    color_toReturn.g = 0.5f * (color_toReturn.g + 1.0f);
                    break;

                case ColorComponent.b:
                    color_toReturn.b = 0.5f * (color_toReturn.b + 1.0f);
                    break;

                default:
                    break;
            }

            switch (smallestComponent)
            {
                case ColorComponent.r:
                    color_toReturn.r = 1.0f;
                    break;

                case ColorComponent.g:
                    color_toReturn.g = 1.0f;
                    break;

                case ColorComponent.b:
                    color_toReturn.b = 1.0f;
                    break;

                default:
                    break;
            }

            if (UtilitiesDXXL_Math.CheckIf_givenNumberIs_evenNotOdd(seed))
            {
                color_toReturn = Invert_andAlphaTo1(color_toReturn);
                color_toReturn = color_toReturn * 2.4f;
            }

            color_toReturn = SeededColorGenerator.ForceApproxLuminance(color_toReturn, forceLuminance);
            color_toReturn.a = alphaToUse_0to1;
            return color_toReturn;
        }

        static ColorComponent GetBiggestComponentRGB(Color color)
        {
            float componentValue;
            return GetBiggestComponentRGB(color, out componentValue);
        }

        static ColorComponent GetBiggestComponentRGB(Color color, out float biggestValue)
        {
            ColorComponent biggestComponent = ColorComponent.r;
            biggestValue = color.r;

            if (color.g > biggestValue)
            {
                biggestComponent = ColorComponent.g;
                biggestValue = color.g;
            }

            if (color.b > biggestValue)
            {
                biggestComponent = ColorComponent.b;
                biggestValue = color.b;
            }

            return biggestComponent;
        }

        static ColorComponent GetSmallestComponentRGB(Color color)
        {
            float componentValue;
            return GetSmallestComponentRGB(color, out componentValue);
        }

        static ColorComponent GetSmallestComponentRGB(Color color, out float smallestValue)
        {
            ColorComponent smallestComponent = ColorComponent.r;
            smallestValue = color.r;

            if (color.g < smallestValue)
            {
                smallestComponent = ColorComponent.g;
                smallestValue = color.g;
            }

            if (color.b < smallestValue)
            {
                smallestComponent = ColorComponent.b;
                smallestValue = color.b;
            }

            return smallestComponent;
        }

        public static Color OverwriteColorNearGreyWithBlack(Color colorToOverwrite)
        {
            if (UtilitiesDXXL_Math.CheckIfValueLiesInsideDistanceNearAnotherValue(0.5f, colorToOverwrite.r, 0.12f) && UtilitiesDXXL_Math.CheckIfValueLiesInsideDistanceNearAnotherValue(0.5f, colorToOverwrite.g, 0.12f) && UtilitiesDXXL_Math.CheckIfValueLiesInsideDistanceNearAnotherValue(0.5f, colorToOverwrite.b, 0.12f))
            {
                return Color.black;
            }
            else
            {
                return colorToOverwrite;
            }
        }

        public static Color Invert_andAlphaTo1(Color color)
        {
            return new Color(1.0f - color.r, 1.0f - color.g, 1.0f - color.b);
        }

        public static Color OverwriteDefaultColor(Color colorToOverwriteIfDefault)
        {
            return OverwriteDefaultColor(colorToOverwriteIfDefault, DrawBasics.defaultColor);
        }

        public static Color OverwriteDefaultColor(Color colorToOverwriteIfDefault, Color overwritingColor)
        {
            if (UtilitiesDXXL_Math.FloatIsInvalid(colorToOverwriteIfDefault.r) || UtilitiesDXXL_Math.FloatIsInvalid(colorToOverwriteIfDefault.g) || UtilitiesDXXL_Math.FloatIsInvalid(colorToOverwriteIfDefault.b) || UtilitiesDXXL_Math.FloatIsInvalid(colorToOverwriteIfDefault.a))
            {
                Debug.LogError("color contains invalid float components: ( <b>r</b> is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(colorToOverwriteIfDefault.r) + ", <b>g</b> is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(colorToOverwriteIfDefault.g) + ", <b>b</b> is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(colorToOverwriteIfDefault.b) + ", <b>a</b> is " + UtilitiesDXXL_Math.GetFloatInvalidTypeAsString(colorToOverwriteIfDefault.a) + ") -> fallback to white color.");
                return overwritingColor;
            }

            if (IsDefaultColor(colorToOverwriteIfDefault))
            {
                return overwritingColor;
            }
            else
            {
                return colorToOverwriteIfDefault;
            }
        }

        public static bool IsDefaultColor(Color color)
        {
            return (UtilitiesDXXL_Math.ApproximatelyZero(color.r) && UtilitiesDXXL_Math.ApproximatelyZero(color.g) && UtilitiesDXXL_Math.ApproximatelyZero(color.b) && UtilitiesDXXL_Math.ApproximatelyZero(color.a));
        }

        public static bool IsApproxSameColor(Color color1, Color color2, bool ignoreAlpha = true)
        {
            if (UtilitiesDXXL_Math.CheckIfValueLiesInsideDistanceNearAnotherValue(color1.r, color2.r, 0.005f))
            {
                if (UtilitiesDXXL_Math.CheckIfValueLiesInsideDistanceNearAnotherValue(color1.g, color2.g, 0.005f))
                {
                    if (UtilitiesDXXL_Math.CheckIfValueLiesInsideDistanceNearAnotherValue(color1.b, color2.b, 0.005f))
                    {
                        if (ignoreAlpha)
                        {
                            return true;
                        }
                        else
                        {
                            if (UtilitiesDXXL_Math.CheckIfValueLiesInsideDistanceNearAnotherValue(color1.a, color2.a, 0.005f))
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static Color GetSimilarColorWithOtherBrightnessValue(Color initialColor)
        {
            return ((initialColor.grayscale < 0.175f) ? Color.Lerp(initialColor, Color.white, 0.7f) : Color.Lerp(initialColor, Color.black, 0.7f));
        }

        public static Color GetSimilarColorWithOtherBrightnessValue(Color initialColor, float changeIntensity)
        {
            return ((initialColor.grayscale < 0.175f) ? Color.Lerp(initialColor, Color.white, changeIntensity) : Color.Lerp(initialColor, Color.black, changeIntensity));
        }

        public static Color GetSimilarColorWithSlightlyOtherBrightnessValue(Color initialColor)
        {
            float luminanceOfInitialColor = SeededColorGenerator.GetLuminance(initialColor);
            return ((luminanceOfInitialColor < 0.5f) ? Color.Lerp(initialColor, Color.white, 0.2f) : Color.Lerp(initialColor, Color.black, 0.2f));
        }

        public static Color GetSimilarColorWithAdjustableOtherBrightnessValue(Color initialColor, float changeIntensity)
        {
            float luminanceOfInitialColor = SeededColorGenerator.GetLuminance(initialColor);
            return ((luminanceOfInitialColor < 0.5f) ? Color.Lerp(initialColor, Color.white, changeIntensity) : Color.Lerp(initialColor, Color.black, changeIntensity));
        }

        static Keyframe redCurveOfRainbow_atLuminanceNotForced_keyframe0 = new Keyframe(0f, 1f, 0f, 0f, 0f, 0.0267929f);
        static Keyframe redCurveOfRainbow_atLuminanceNotForced_keyframe1 = new Keyframe(0.1666667f, 1f, 0f, -2.628986f, 0.03435588f, 0.3283422f);
        static Keyframe redCurveOfRainbow_atLuminanceNotForced_keyframe2 = new Keyframe(0.3333333f, 0f, -19.39972f, 0f, 0.1582886f, 0.01367907f);
        static Keyframe redCurveOfRainbow_atLuminanceNotForced_keyframe3 = new Keyframe(0.6666667f, 0f, 0f, 45.27549f, 0.0103105f, 0.06844892f);
        static Keyframe redCurveOfRainbow_atLuminanceNotForced_keyframe4 = new Keyframe(0.8333333f, 1f, 2.024274f, 0f, 0.4823529f, 0.01595771f);
        static Keyframe redCurveOfRainbow_atLuminanceNotForced_keyframe5 = new Keyframe(1f, 1f, 0f, 0f, 0.02324617f, 0f);
        static AnimationCurve redCurveOfRainbow_atLuminanceNotForced = new AnimationCurve(redCurveOfRainbow_atLuminanceNotForced_keyframe0, redCurveOfRainbow_atLuminanceNotForced_keyframe1, redCurveOfRainbow_atLuminanceNotForced_keyframe2, redCurveOfRainbow_atLuminanceNotForced_keyframe3, redCurveOfRainbow_atLuminanceNotForced_keyframe4, redCurveOfRainbow_atLuminanceNotForced_keyframe5);

        static Keyframe greenCurveOfRainbow_atLuminanceNotForced_keyframe0 = new Keyframe(0f, 0f, 0f, 0f, 0f, 0.00778269f);
        static Keyframe greenCurveOfRainbow_atLuminanceNotForced_keyframe1 = new Keyframe(0.333333f, 0f, 0f, 17.67738f, 0.003058169f, 0.1337735f);
        static Keyframe greenCurveOfRainbow_atLuminanceNotForced_keyframe2 = new Keyframe(0.4080812f, 0.5186183f, 5.17586f, 5.17586f, 0.4374433f, 0.4164693f);
        static Keyframe greenCurveOfRainbow_atLuminanceNotForced_keyframe3 = new Keyframe(0.5f, 1f, 2.790231f, 0f, 0.2850692f, 0.007641078f);
        static Keyframe greenCurveOfRainbow_atLuminanceNotForced_keyframe4 = new Keyframe(0.8333333f, 1f, 0f, -1.997148f, 0.006811738f, 0.3967916f);
        static Keyframe greenCurveOfRainbow_atLuminanceNotForced_keyframe5 = new Keyframe(1f, 0f, -14.63444f, -5.54757f, 0.1411764f, 0f);
        static AnimationCurve greenCurveOfRainbow_atLuminanceNotForced = new AnimationCurve(greenCurveOfRainbow_atLuminanceNotForced_keyframe0, greenCurveOfRainbow_atLuminanceNotForced_keyframe1, greenCurveOfRainbow_atLuminanceNotForced_keyframe2, greenCurveOfRainbow_atLuminanceNotForced_keyframe3, greenCurveOfRainbow_atLuminanceNotForced_keyframe4, greenCurveOfRainbow_atLuminanceNotForced_keyframe5);

        static Keyframe blueCurveOfRainbow_atLuminanceNotForced_keyframe0 = new Keyframe(0f, -0.002349854f, 5.103677f, 9.843518f, 0f, 0.4812853f);
        static Keyframe blueCurveOfRainbow_atLuminanceNotForced_keyframe1 = new Keyframe(0.166666f, 1f, 3.602612f, 0f, 0.3582862f, 0.005642463f);
        static Keyframe blueCurveOfRainbow_atLuminanceNotForced_keyframe2 = new Keyframe(0.5f, 1f, 0f, -3.017625f, 0.01999663f, 0.3048127f);
        static Keyframe blueCurveOfRainbow_atLuminanceNotForced_keyframe3 = new Keyframe(0.6666666f, 0f, -22.01747f, 0f, 0.1368982f, 0.01547652f);
        static Keyframe blueCurveOfRainbow_atLuminanceNotForced_keyframe4 = new Keyframe(1f, 0f, 0f, 0f, 0f, 0f);
        static AnimationCurve blueCurveOfRainbow_atLuminanceNotForced = new AnimationCurve(blueCurveOfRainbow_atLuminanceNotForced_keyframe0, blueCurveOfRainbow_atLuminanceNotForced_keyframe1, blueCurveOfRainbow_atLuminanceNotForced_keyframe2, blueCurveOfRainbow_atLuminanceNotForced_keyframe3, blueCurveOfRainbow_atLuminanceNotForced_keyframe4);

        static Keyframe luminanceTargetCurveOfRainbow_atLuminanceNotForced_keyframe0 = new Keyframe(0f, 1f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminanceNotForced_keyframe1 = new Keyframe(1f, 1f, 0f, 0f, 0f, 0f);
        static AnimationCurve luminanceTargetCurveOfRainbow_atLuminanceNotForced = new AnimationCurve(luminanceTargetCurveOfRainbow_atLuminanceNotForced_keyframe0, luminanceTargetCurveOfRainbow_atLuminanceNotForced_keyframe1);

        static Keyframe redCurveOfRainbow_atLuminance0825_keyframe0 = new Keyframe(0f, 1f, 0f, 0f, 0f, 0.0267929f);
        static Keyframe redCurveOfRainbow_atLuminance0825_keyframe1 = new Keyframe(0.1666667f, 1f, 0f, -34.14504f, 0.03435588f, 0.0845046f);
        static Keyframe redCurveOfRainbow_atLuminance0825_keyframe2 = new Keyframe(0.2320574f, 0.4720321f, -5.176624f, -5.176624f, 0.4951762f, 0.423487f);
        static Keyframe redCurveOfRainbow_atLuminance0825_keyframe3 = new Keyframe(0.33333f, 0.05f, -2.198078f, 0f, 0.1232304f, 0.01367907f);
        static Keyframe redCurveOfRainbow_atLuminance0825_keyframe4 = new Keyframe(0.41f, 0f, -0.2348027f, 0f, 0.7860962f, 0.01367907f);
        static Keyframe redCurveOfRainbow_atLuminance0825_keyframe5 = new Keyframe(0.6666667f, 0f, 0f, 8.216114f, 0.0103105f, 0.2128025f);
        static Keyframe redCurveOfRainbow_atLuminance0825_keyframe6 = new Keyframe(0.7662566f, 0.5346156f, 2.536475f, 2.750102f, 0.4451402f, 0.6306142f);
        static Keyframe redCurveOfRainbow_atLuminance0825_keyframe7 = new Keyframe(0.8333333f, 1f, 14.31863f, 0f, 0.2072095f, 0.01595771f);
        static Keyframe redCurveOfRainbow_atLuminance0825_keyframe8 = new Keyframe(1f, 1f, 0f, 0f, 0.02324617f, 0f);
        static AnimationCurve redCurveOfRainbow_atLuminance0825 = new AnimationCurve(redCurveOfRainbow_atLuminance0825_keyframe0, redCurveOfRainbow_atLuminance0825_keyframe1, redCurveOfRainbow_atLuminance0825_keyframe2, redCurveOfRainbow_atLuminance0825_keyframe3, redCurveOfRainbow_atLuminance0825_keyframe4, redCurveOfRainbow_atLuminance0825_keyframe5, redCurveOfRainbow_atLuminance0825_keyframe6, redCurveOfRainbow_atLuminance0825_keyframe7, redCurveOfRainbow_atLuminance0825_keyframe8);

        static Keyframe greenCurveOfRainbow_atLuminance0825_keyframe0 = new Keyframe(0f, 0f, 0f, 0f, 0f, 0.00778269f);
        static Keyframe greenCurveOfRainbow_atLuminance0825_keyframe1 = new Keyframe(0.257754f, 0f, 0f, 0f, 0f, 0.00778269f);
        static Keyframe greenCurveOfRainbow_atLuminance0825_keyframe2 = new Keyframe(0.333333f, 0.05f, 0f, 2.176718f, 0.003058169f, 0.4737473f);
        static Keyframe greenCurveOfRainbow_atLuminance0825_keyframe3 = new Keyframe(0.4266467f, 0.4177842f, 4.696632f, 4.900658f, 0.4689451f, 0.4386555f);
        static Keyframe greenCurveOfRainbow_atLuminance0825_keyframe4 = new Keyframe(0.5f, 1f, 10.00402f, 0f, 0.2608117f, 0.007641078f);
        static Keyframe greenCurveOfRainbow_atLuminance0825_keyframe5 = new Keyframe(0.8333333f, 1f, 0f, -24.77313f, 0.006811738f, 0.1773775f);
        static Keyframe greenCurveOfRainbow_atLuminance0825_keyframe6 = new Keyframe(0.8805654f, 0.531985f, -9.559245f, -7.222806f, 0.2444473f, 0.294646f);
        static Keyframe greenCurveOfRainbow_atLuminance0825_keyframe7 = new Keyframe(1f, 0f, -4.811322f, -5.54757f, 0.2375333f, 0f);
        static AnimationCurve greenCurveOfRainbow_atLuminance0825 = new AnimationCurve(greenCurveOfRainbow_atLuminance0825_keyframe0, greenCurveOfRainbow_atLuminance0825_keyframe1, greenCurveOfRainbow_atLuminance0825_keyframe2, greenCurveOfRainbow_atLuminance0825_keyframe3, greenCurveOfRainbow_atLuminance0825_keyframe4, greenCurveOfRainbow_atLuminance0825_keyframe5, greenCurveOfRainbow_atLuminance0825_keyframe6, greenCurveOfRainbow_atLuminance0825_keyframe7);

        static Keyframe blueCurveOfRainbow_atLuminance0825_keyframe0 = new Keyframe(0f, 0f, 5.103677f, 2.205189f, 0f, 0.4330214f);
        static Keyframe blueCurveOfRainbow_atLuminance0825_keyframe1 = new Keyframe(0.1062056f, 0.3805489f, 4.460747f, 4.460747f, 0.410798f, 0.3914252f);
        static Keyframe blueCurveOfRainbow_atLuminance0825_keyframe2 = new Keyframe(0.166666f, 1f, 17.27078f, 0f, 0.2181011f, 0.005642463f);
        static Keyframe blueCurveOfRainbow_atLuminance0825_keyframe3 = new Keyframe(0.5f, 1f, 0f, -16.78093f, 0.01999663f, 0.2947439f);
        static Keyframe blueCurveOfRainbow_atLuminance0825_keyframe4 = new Keyframe(0.5647435f, 0.4470914f, -4.936384f, -3.610273f, 0.3874579f, 0.2659675f);
        static Keyframe blueCurveOfRainbow_atLuminance0825_keyframe5 = new Keyframe(0.6666666f, 0f, -9.622663f, 0f, 0.3078054f, 0.01547652f);
        static Keyframe blueCurveOfRainbow_atLuminance0825_keyframe6 = new Keyframe(1f, 0f, 0f, 0f, 0f, 0f);
        static AnimationCurve blueCurveOfRainbow_atLuminance0825 = new AnimationCurve(blueCurveOfRainbow_atLuminance0825_keyframe0, blueCurveOfRainbow_atLuminance0825_keyframe1, blueCurveOfRainbow_atLuminance0825_keyframe2, blueCurveOfRainbow_atLuminance0825_keyframe3, blueCurveOfRainbow_atLuminance0825_keyframe4, blueCurveOfRainbow_atLuminance0825_keyframe5, blueCurveOfRainbow_atLuminance0825_keyframe6);

        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0825_keyframe0 = new Keyframe(0f, 1f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0825_keyframe1 = new Keyframe(1f, 1f, 0f, 0f, 0f, 0f);
        static AnimationCurve luminanceTargetCurveOfRainbow_atLuminance0825 = new AnimationCurve(luminanceTargetCurveOfRainbow_atLuminance0825_keyframe0, luminanceTargetCurveOfRainbow_atLuminance0825_keyframe1);

        static Keyframe redCurveOfRainbow_atLuminance0675_keyframe0 = new Keyframe(0f, 1f, 0f, 0f, 0f, 0.0267929f);
        static Keyframe redCurveOfRainbow_atLuminance0675_keyframe1 = new Keyframe(0.1666667f, 1f, 0f, -27.8928f, 0.03435588f, 0.04299471f);
        static Keyframe redCurveOfRainbow_atLuminance0675_keyframe2 = new Keyframe(0.2331255f, 0.519197f, -5.176624f, -5.176624f, 0.4951762f, 0.423487f);
        static Keyframe redCurveOfRainbow_atLuminance0675_keyframe3 = new Keyframe(0.3333333f, 0.1f, -2.198078f, 0f, 0.1232304f, 0.01367907f);
        static Keyframe redCurveOfRainbow_atLuminance0675_keyframe4 = new Keyframe(0.41f, 0f, -0.2348027f, 0f, 0.7860962f, 0.01367907f);
        static Keyframe redCurveOfRainbow_atLuminance0675_keyframe5 = new Keyframe(0.6666667f, 0f, 0f, 8.878784f, 0.0103105f, 0.1732416f);
        static Keyframe redCurveOfRainbow_atLuminance0675_keyframe6 = new Keyframe(0.7705347f, 0.5629174f, 3.463916f, 4.153102f, 0.5805737f, 0.599038f);
        static Keyframe redCurveOfRainbow_atLuminance0675_keyframe7 = new Keyframe(0.8333333f, 1f, 8.022022f, 0f, 0.2558905f, 0.01595771f);
        static Keyframe redCurveOfRainbow_atLuminance0675_keyframe8 = new Keyframe(1f, 1f, 0f, 0f, 0.02324617f, 0f);
        static AnimationCurve redCurveOfRainbow_atLuminance0675 = new AnimationCurve(redCurveOfRainbow_atLuminance0675_keyframe0, redCurveOfRainbow_atLuminance0675_keyframe1, redCurveOfRainbow_atLuminance0675_keyframe2, redCurveOfRainbow_atLuminance0675_keyframe3, redCurveOfRainbow_atLuminance0675_keyframe4, redCurveOfRainbow_atLuminance0675_keyframe5, redCurveOfRainbow_atLuminance0675_keyframe6, redCurveOfRainbow_atLuminance0675_keyframe7, redCurveOfRainbow_atLuminance0675_keyframe8);

        static Keyframe greenCurveOfRainbow_atLuminance0675_keyframe0 = new Keyframe(0f, 0f, 0f, 0f, 0f, 0.00778269f);
        static Keyframe greenCurveOfRainbow_atLuminance0675_keyframe1 = new Keyframe(0.257754f, 0f, 0f, 0f, 0f, 0.00778269f);
        static Keyframe greenCurveOfRainbow_atLuminance0675_keyframe2 = new Keyframe(0.333333f, 0.1023499f, 0f, 4.399855f, 0.003058169f, 0.1528241f);
        static Keyframe greenCurveOfRainbow_atLuminance0675_keyframe3 = new Keyframe(0.4266467f, 0.4177842f, 4.516745f, 4.023407f, 0.5491757f, 0.4652202f);
        static Keyframe greenCurveOfRainbow_atLuminance0675_keyframe4 = new Keyframe(0.5f, 1f, 10.00402f, 0f, 0.2608117f, 0.007641078f);
        static Keyframe greenCurveOfRainbow_atLuminance0675_keyframe5 = new Keyframe(0.8333333f, 1f, 0f, -14.14011f, 0.006811738f, 0.259255f);
        static Keyframe greenCurveOfRainbow_atLuminance0675_keyframe6 = new Keyframe(0.9009427f, 0.4423702f, -4.054608f, -5.455518f, 0.3919348f, 0.3876472f);
        static Keyframe greenCurveOfRainbow_atLuminance0675_keyframe7 = new Keyframe(1f, 0f, -7.105606f, -5.54757f, 0.1943456f, 0f);
        static AnimationCurve greenCurveOfRainbow_atLuminance0675 = new AnimationCurve(greenCurveOfRainbow_atLuminance0675_keyframe0, greenCurveOfRainbow_atLuminance0675_keyframe1, greenCurveOfRainbow_atLuminance0675_keyframe2, greenCurveOfRainbow_atLuminance0675_keyframe3, greenCurveOfRainbow_atLuminance0675_keyframe4, greenCurveOfRainbow_atLuminance0675_keyframe5, greenCurveOfRainbow_atLuminance0675_keyframe6, greenCurveOfRainbow_atLuminance0675_keyframe7);

        static Keyframe blueCurveOfRainbow_atLuminance0675_keyframe0 = new Keyframe(0f, 0f, 5.103677f, 79.3868f, 0f, 0.01229945f);
        static Keyframe blueCurveOfRainbow_atLuminance0675_keyframe1 = new Keyframe(0.08695666f, 0.371119f, 4.460747f, 4.460747f, 0.410798f, 0.3914252f);
        static Keyframe blueCurveOfRainbow_atLuminance0675_keyframe2 = new Keyframe(0.166666f, 1f, 17.27078f, 0f, 0.2181011f, 0.005642463f);
        static Keyframe blueCurveOfRainbow_atLuminance0675_keyframe3 = new Keyframe(0.5f, 1f, 0f, -16.78093f, 0.01999663f, 0.2947439f);
        static Keyframe blueCurveOfRainbow_atLuminance0675_keyframe4 = new Keyframe(0.5647435f, 0.4470914f, -4.936384f, -3.610273f, 0.3874579f, 0.2659675f);
        static Keyframe blueCurveOfRainbow_atLuminance0675_keyframe5 = new Keyframe(0.6666666f, 0f, -9.622663f, 0f, 0.3078054f, 0.01547652f);
        static Keyframe blueCurveOfRainbow_atLuminance0675_keyframe6 = new Keyframe(1f, 0f, 0f, 0f, 0f, 0f);
        static AnimationCurve blueCurveOfRainbow_atLuminance0675 = new AnimationCurve(blueCurveOfRainbow_atLuminance0675_keyframe0, blueCurveOfRainbow_atLuminance0675_keyframe1, blueCurveOfRainbow_atLuminance0675_keyframe2, blueCurveOfRainbow_atLuminance0675_keyframe3, blueCurveOfRainbow_atLuminance0675_keyframe4, blueCurveOfRainbow_atLuminance0675_keyframe5, blueCurveOfRainbow_atLuminance0675_keyframe6);

        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0675_keyframe0 = new Keyframe(0f, 1.1f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0675_keyframe1 = new Keyframe(0.1666666f, 1f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0675_keyframe2 = new Keyframe(0.3333333f, 1.1f, 0.002788117f, 0f, 0.8747915f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0675_keyframe3 = new Keyframe(0.5f, 1f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0675_keyframe4 = new Keyframe(0.8333333f, 1f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0675_keyframe5 = new Keyframe(1f, 1.1f, 0f, 0f, 0f, 0f);
        static AnimationCurve luminanceTargetCurveOfRainbow_atLuminance0675 = new AnimationCurve(luminanceTargetCurveOfRainbow_atLuminance0675_keyframe0, luminanceTargetCurveOfRainbow_atLuminance0675_keyframe1, luminanceTargetCurveOfRainbow_atLuminance0675_keyframe2, luminanceTargetCurveOfRainbow_atLuminance0675_keyframe3, luminanceTargetCurveOfRainbow_atLuminance0675_keyframe4, luminanceTargetCurveOfRainbow_atLuminance0675_keyframe5);

        static Keyframe redCurveOfRainbow_atLuminance05_keyframe0 = new Keyframe(0f, 1f, 0f, 0f, 0f, 0.0267929f);
        static Keyframe redCurveOfRainbow_atLuminance05_keyframe1 = new Keyframe(0.1666667f, 1f, 0f, -27.8928f, 0.03435588f, 0.04299471f);
        static Keyframe redCurveOfRainbow_atLuminance05_keyframe2 = new Keyframe(0.2331255f, 0.519197f, -5.176624f, -5.176624f, 0.4951762f, 0.423487f);
        static Keyframe redCurveOfRainbow_atLuminance05_keyframe3 = new Keyframe(0.3333333f, 0.1f, -2.198078f, 0f, 0.1232304f, 0.01367907f);
        static Keyframe redCurveOfRainbow_atLuminance05_keyframe4 = new Keyframe(0.41f, 0f, -0.2348027f, 0f, 0.7860962f, 0.01367907f);
        static Keyframe redCurveOfRainbow_atLuminance05_keyframe5 = new Keyframe(0.6666667f, 0f, 0f, 28.39604f, 0.0103105f, 0.1390375f);
        static Keyframe redCurveOfRainbow_atLuminance05_keyframe6 = new Keyframe(0.8333333f, 1f, 26.88915f, 0f, 0.03315497f, 0.01595771f);
        static Keyframe redCurveOfRainbow_atLuminance05_keyframe7 = new Keyframe(1f, 1f, 0f, 0f, 0.02324617f, 0f);
        static AnimationCurve redCurveOfRainbow_atLuminance05 = new AnimationCurve(redCurveOfRainbow_atLuminance05_keyframe0, redCurveOfRainbow_atLuminance05_keyframe1, redCurveOfRainbow_atLuminance05_keyframe2, redCurveOfRainbow_atLuminance05_keyframe3, redCurveOfRainbow_atLuminance05_keyframe4, redCurveOfRainbow_atLuminance05_keyframe5, redCurveOfRainbow_atLuminance05_keyframe6, redCurveOfRainbow_atLuminance05_keyframe7);

        static Keyframe greenCurveOfRainbow_atLuminance05_keyframe0 = new Keyframe(0f, 0f, 0f, 0f, 0f, 0.00778269f);
        static Keyframe greenCurveOfRainbow_atLuminance05_keyframe1 = new Keyframe(0.257754f, 0f, 0f, 0f, 0f, 0.00778269f);
        static Keyframe greenCurveOfRainbow_atLuminance05_keyframe2 = new Keyframe(0.333333f, 0.1023499f, 0f, 4.399855f, 0.003058169f, 0.1528241f);
        static Keyframe greenCurveOfRainbow_atLuminance05_keyframe3 = new Keyframe(0.4266467f, 0.4177842f, 4.516745f, 4.023407f, 0.5491757f, 0.4652202f);
        static Keyframe greenCurveOfRainbow_atLuminance05_keyframe4 = new Keyframe(0.5f, 1f, 10.00402f, 0f, 0.2608117f, 0.007641078f);
        static Keyframe greenCurveOfRainbow_atLuminance05_keyframe5 = new Keyframe(0.8333333f, 1f, 0f, -11.52388f, 0.006811738f, 0.3067519f);
        static Keyframe greenCurveOfRainbow_atLuminance05_keyframe6 = new Keyframe(0.9234037f, 0.4471004f, -4.054608f, -5.455518f, 0.3919348f, 0.3876472f);
        static Keyframe greenCurveOfRainbow_atLuminance05_keyframe7 = new Keyframe(1f, 0f, -17.81115f, -5.54757f, 0.1841612f, 0f);
        static AnimationCurve greenCurveOfRainbow_atLuminance05 = new AnimationCurve(greenCurveOfRainbow_atLuminance05_keyframe0, greenCurveOfRainbow_atLuminance05_keyframe1, greenCurveOfRainbow_atLuminance05_keyframe2, greenCurveOfRainbow_atLuminance05_keyframe3, greenCurveOfRainbow_atLuminance05_keyframe4, greenCurveOfRainbow_atLuminance05_keyframe5, greenCurveOfRainbow_atLuminance05_keyframe6, greenCurveOfRainbow_atLuminance05_keyframe7);

        static Keyframe blueCurveOfRainbow_atLuminance05_keyframe0 = new Keyframe(0f, 0f, 5.103677f, 79.3868f, 0f, 0.01229945f);
        static Keyframe blueCurveOfRainbow_atLuminance05_keyframe1 = new Keyframe(0.08695666f, 0.371119f, 4.460747f, 4.460747f, 0.410798f, 0.3914252f);
        static Keyframe blueCurveOfRainbow_atLuminance05_keyframe2 = new Keyframe(0.166666f, 1f, 17.27078f, 0f, 0.2181011f, 0.005642463f);
        static Keyframe blueCurveOfRainbow_atLuminance05_keyframe3 = new Keyframe(0.5f, 1f, 0f, -16.78093f, 0.01999663f, 0.2947439f);
        static Keyframe blueCurveOfRainbow_atLuminance05_keyframe4 = new Keyframe(0.5743871f, 0.5367215f, -4.936384f, -3.610273f, 0.3874579f, 0.2659675f);
        static Keyframe blueCurveOfRainbow_atLuminance05_keyframe5 = new Keyframe(0.6666666f, 0f, -17.39986f, 0f, 0.282023f, 0.01547652f);
        static Keyframe blueCurveOfRainbow_atLuminance05_keyframe6 = new Keyframe(1f, 0f, 0f, 0f, 0f, 0f);
        static AnimationCurve blueCurveOfRainbow_atLuminance05 = new AnimationCurve(blueCurveOfRainbow_atLuminance05_keyframe0, blueCurveOfRainbow_atLuminance05_keyframe1, blueCurveOfRainbow_atLuminance05_keyframe2, blueCurveOfRainbow_atLuminance05_keyframe3, blueCurveOfRainbow_atLuminance05_keyframe4, blueCurveOfRainbow_atLuminance05_keyframe5, blueCurveOfRainbow_atLuminance05_keyframe6);

        static Keyframe luminanceTargetCurveOfRainbow_atLuminance05_keyframe0 = new Keyframe(0f, 1.1f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance05_keyframe1 = new Keyframe(0.1666666f, 1f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance05_keyframe2 = new Keyframe(0.3333333f, 1.22f, 0.002788117f, 0f, 0.8747915f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance05_keyframe3 = new Keyframe(0.5f, 1f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance05_keyframe4 = new Keyframe(0.8333333f, 1f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance05_keyframe5 = new Keyframe(1f, 1.1f, 0f, 0f, 0f, 0f);
        static AnimationCurve luminanceTargetCurveOfRainbow_atLuminance05 = new AnimationCurve(luminanceTargetCurveOfRainbow_atLuminance05_keyframe0, luminanceTargetCurveOfRainbow_atLuminance05_keyframe1, luminanceTargetCurveOfRainbow_atLuminance05_keyframe2, luminanceTargetCurveOfRainbow_atLuminance05_keyframe3, luminanceTargetCurveOfRainbow_atLuminance05_keyframe4, luminanceTargetCurveOfRainbow_atLuminance05_keyframe5);

        static Keyframe redCurveOfRainbow_atLuminance044_keyframe0 = new Keyframe(0f, 1f, 0f, 0f, 0f, 0.0267929f);
        static Keyframe redCurveOfRainbow_atLuminance044_keyframe1 = new Keyframe(0.1666667f, 1f, 0f, -21.46155f, 0.03435588f, 0.1358287f);
        static Keyframe redCurveOfRainbow_atLuminance044_keyframe2 = new Keyframe(0.3333333f, 0.1f, -4.729165f, 0f, 0.2133534f, 0.01367907f);
        static Keyframe redCurveOfRainbow_atLuminance044_keyframe3 = new Keyframe(0.41f, 0f, -0.2348027f, 0f, 0.7860962f, 0.01367907f);
        static Keyframe redCurveOfRainbow_atLuminance044_keyframe4 = new Keyframe(0.6666667f, 0f, 0f, 28.39604f, 0.0103105f, 0.1390375f);
        static Keyframe redCurveOfRainbow_atLuminance044_keyframe5 = new Keyframe(0.8333333f, 1f, 6.455942f, 0f, 0.3989304f, 0.01595771f);
        static Keyframe redCurveOfRainbow_atLuminance044_keyframe6 = new Keyframe(1f, 1f, 0f, 0f, 0.02324617f, 0f);
        static AnimationCurve redCurveOfRainbow_atLuminance044 = new AnimationCurve(redCurveOfRainbow_atLuminance044_keyframe0, redCurveOfRainbow_atLuminance044_keyframe1, redCurveOfRainbow_atLuminance044_keyframe2, redCurveOfRainbow_atLuminance044_keyframe3, redCurveOfRainbow_atLuminance044_keyframe4, redCurveOfRainbow_atLuminance044_keyframe5, redCurveOfRainbow_atLuminance044_keyframe6);

        static Keyframe greenCurveOfRainbow_atLuminance044_keyframe0 = new Keyframe(0f, 0f, 0f, 0f, 0f, 0.00778269f);
        static Keyframe greenCurveOfRainbow_atLuminance044_keyframe1 = new Keyframe(0.257754f, 0f, 0f, 0f, 0f, 0.00778269f);
        static Keyframe greenCurveOfRainbow_atLuminance044_keyframe2 = new Keyframe(0.333333f, 0.1f, 0f, 3.354114f, 0.003058169f, 0.599824f);
        static Keyframe greenCurveOfRainbow_atLuminance044_keyframe3 = new Keyframe(0.4266467f, 0.4177842f, 4.944809f, 4.023407f, 0.2511761f, 0.4652202f);
        static Keyframe greenCurveOfRainbow_atLuminance044_keyframe4 = new Keyframe(0.5f, 1f, 10.00402f, 0f, 0.2608117f, 0.007641078f);
        static Keyframe greenCurveOfRainbow_atLuminance044_keyframe5 = new Keyframe(0.8333333f, 1f, 0f, -14.82952f, 0.006811738f, 0.2913237f);
        static Keyframe greenCurveOfRainbow_atLuminance044_keyframe6 = new Keyframe(0.9234037f, 0.4471004f, -4.054608f, -5.455518f, 0.3919348f, 0.3876472f);
        static Keyframe greenCurveOfRainbow_atLuminance044_keyframe7 = new Keyframe(1f, 0f, -17.81115f, -5.54757f, 0.1841612f, 0f);
        static AnimationCurve greenCurveOfRainbow_atLuminance044 = new AnimationCurve(greenCurveOfRainbow_atLuminance044_keyframe0, greenCurveOfRainbow_atLuminance044_keyframe1, greenCurveOfRainbow_atLuminance044_keyframe2, greenCurveOfRainbow_atLuminance044_keyframe3, greenCurveOfRainbow_atLuminance044_keyframe4, greenCurveOfRainbow_atLuminance044_keyframe5, greenCurveOfRainbow_atLuminance044_keyframe6, greenCurveOfRainbow_atLuminance044_keyframe7);

        static Keyframe blueCurveOfRainbow_atLuminance044_keyframe0 = new Keyframe(0f, 0f, 5.103677f, 10.5359f, 0f, 0.2193234f);
        static Keyframe blueCurveOfRainbow_atLuminance044_keyframe1 = new Keyframe(0.09098038f, 0.3947893f, 4.52334f, 3.623064f, 0.518025f, 0.5643008f);
        static Keyframe blueCurveOfRainbow_atLuminance044_keyframe2 = new Keyframe(0.166666f, 1f, 17.27078f, 0f, 0.2237331f, 0.005642463f);
        static Keyframe blueCurveOfRainbow_atLuminance044_keyframe3 = new Keyframe(0.5f, 1f, 0f, -16.78093f, 0.01999663f, 0.2947439f);
        static Keyframe blueCurveOfRainbow_atLuminance044_keyframe4 = new Keyframe(0.5743871f, 0.5367215f, -4.936384f, -3.610273f, 0.3874579f, 0.2659675f);
        static Keyframe blueCurveOfRainbow_atLuminance044_keyframe5 = new Keyframe(0.6666666f, 0f, -17.39986f, 0f, 0.282023f, 0.01547652f);
        static Keyframe blueCurveOfRainbow_atLuminance044_keyframe6 = new Keyframe(1f, 0f, 0f, 0f, 0f, 0f);
        static AnimationCurve blueCurveOfRainbow_atLuminance044 = new AnimationCurve(blueCurveOfRainbow_atLuminance044_keyframe0, blueCurveOfRainbow_atLuminance044_keyframe1, blueCurveOfRainbow_atLuminance044_keyframe2, blueCurveOfRainbow_atLuminance044_keyframe3, blueCurveOfRainbow_atLuminance044_keyframe4, blueCurveOfRainbow_atLuminance044_keyframe5, blueCurveOfRainbow_atLuminance044_keyframe6);

        static Keyframe luminanceTargetCurveOfRainbow_atLuminance044_keyframe0 = new Keyframe(0f, 1.3f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance044_keyframe1 = new Keyframe(0.1666666f, 1f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance044_keyframe2 = new Keyframe(0.3333333f, 1.3f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance044_keyframe3 = new Keyframe(0.5f, 1f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance044_keyframe4 = new Keyframe(0.8333333f, 1f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance044_keyframe5 = new Keyframe(1f, 1.3f, 0f, 0f, 0f, 0f);
        static AnimationCurve luminanceTargetCurveOfRainbow_atLuminance044 = new AnimationCurve(luminanceTargetCurveOfRainbow_atLuminance044_keyframe0, luminanceTargetCurveOfRainbow_atLuminance044_keyframe1, luminanceTargetCurveOfRainbow_atLuminance044_keyframe2, luminanceTargetCurveOfRainbow_atLuminance044_keyframe3, luminanceTargetCurveOfRainbow_atLuminance044_keyframe4, luminanceTargetCurveOfRainbow_atLuminance044_keyframe5);

        static Keyframe redCurveOfRainbow_atLuminance040_keyframe0 = new Keyframe(0f, 1f, 0f, 0f, 0f, 0.0267929f);
        static Keyframe redCurveOfRainbow_atLuminance040_keyframe1 = new Keyframe(0.1666667f, 1f, 0f, -21.46155f, 0.03435588f, 0.1358287f);
        static Keyframe redCurveOfRainbow_atLuminance040_keyframe2 = new Keyframe(0.3333333f, 0.1f, -4.729165f, 0f, 0.2133534f, 0.01367907f);
        static Keyframe redCurveOfRainbow_atLuminance040_keyframe3 = new Keyframe(0.5f, 0.1f, -0.02962309f, -13.02118f, 0.2406417f, 0.04171121f);
        static Keyframe redCurveOfRainbow_atLuminance040_keyframe4 = new Keyframe(0.6666667f, 0f, 0.01826039f, 14.58432f, 0.7786098f, 0.2614246f);
        static Keyframe redCurveOfRainbow_atLuminance040_keyframe5 = new Keyframe(0.7071924f, 0.485076f, 6.289471f, 3.807689f, 0.7160985f, 0.5661704f);
        static Keyframe redCurveOfRainbow_atLuminance040_keyframe6 = new Keyframe(0.8333333f, 0.7555728f, 4.005681f, 1.052538f, 0.3659997f, 0.3454545f);
        static Keyframe redCurveOfRainbow_atLuminance040_keyframe7 = new Keyframe(1f, 1f, 2.362824f, 0f, 0.2631014f, 0f);
        static AnimationCurve redCurveOfRainbow_atLuminance040 = new AnimationCurve(redCurveOfRainbow_atLuminance040_keyframe0, redCurveOfRainbow_atLuminance040_keyframe1, redCurveOfRainbow_atLuminance040_keyframe2, redCurveOfRainbow_atLuminance040_keyframe3, redCurveOfRainbow_atLuminance040_keyframe4, redCurveOfRainbow_atLuminance040_keyframe5, redCurveOfRainbow_atLuminance040_keyframe6, redCurveOfRainbow_atLuminance040_keyframe7);

        static Keyframe greenCurveOfRainbow_atLuminance040_keyframe0 = new Keyframe(0f, 0f, 0f, 0f, 0f, 0.00778269f);
        static Keyframe greenCurveOfRainbow_atLuminance040_keyframe1 = new Keyframe(0.257754f, 0f, 0f, 0f, 0f, 0.00778269f);
        static Keyframe greenCurveOfRainbow_atLuminance040_keyframe2 = new Keyframe(0.333333f, 0.1f, 0f, 28.33642f, 0.003058169f, 0.02803065f);
        static Keyframe greenCurveOfRainbow_atLuminance040_keyframe3 = new Keyframe(0.4223742f, 0.4059434f, 4.689831f, 4.023407f, 0.455475f, 0.4652202f);
        static Keyframe greenCurveOfRainbow_atLuminance040_keyframe4 = new Keyframe(0.4989319f, 1f, 10.67839f, 0f, 0.2724356f, 0.007641078f);
        static Keyframe greenCurveOfRainbow_atLuminance040_keyframe5 = new Keyframe(0.666666f, 1f, 0f, -3.810817f, 0.006811738f, 0.2820041f);
        static Keyframe greenCurveOfRainbow_atLuminance040_keyframe6 = new Keyframe(0.8266703f, 0.6480225f, -1.449353f, -2.16764f, 0.4432741f, 0.402506f);
        static Keyframe greenCurveOfRainbow_atLuminance040_keyframe7 = new Keyframe(0.9405828f, 0.3847077f, -3.07261f, -3.760881f, 0.5782549f, 0.6939976f);
        static Keyframe greenCurveOfRainbow_atLuminance040_keyframe8 = new Keyframe(1f, -0.004608155f, -6.25816f, -5.54757f, 0.3911783f, 0f);
        static AnimationCurve greenCurveOfRainbow_atLuminance040 = new AnimationCurve(greenCurveOfRainbow_atLuminance040_keyframe0, greenCurveOfRainbow_atLuminance040_keyframe1, greenCurveOfRainbow_atLuminance040_keyframe2, greenCurveOfRainbow_atLuminance040_keyframe3, greenCurveOfRainbow_atLuminance040_keyframe4, greenCurveOfRainbow_atLuminance040_keyframe5, greenCurveOfRainbow_atLuminance040_keyframe6, greenCurveOfRainbow_atLuminance040_keyframe7, greenCurveOfRainbow_atLuminance040_keyframe8);

        static Keyframe blueCurveOfRainbow_atLuminance040_keyframe0 = new Keyframe(0f, 0f, 5.103677f, 13.03066f, 0f, 0.2586207f);
        static Keyframe blueCurveOfRainbow_atLuminance040_keyframe1 = new Keyframe(0.09098038f, 0.3947893f, 4.52334f, 3.623064f, 0.518025f, 0.5643008f);
        static Keyframe blueCurveOfRainbow_atLuminance040_keyframe2 = new Keyframe(0.166666f, 1f, 17.27078f, 0f, 0.2237331f, 0.005642463f);
        static Keyframe blueCurveOfRainbow_atLuminance040_keyframe3 = new Keyframe(0.333333f, 1f, -0.06550641f, 0f, 0.2160403f, 0.005642463f);
        static Keyframe blueCurveOfRainbow_atLuminance040_keyframe4 = new Keyframe(0.5f, 1f, 1.894388f, -10.71091f, 0.4780738f, 0.352255f);
        static Keyframe blueCurveOfRainbow_atLuminance040_keyframe5 = new Keyframe(0.5743871f, 0.5367215f, -4.936384f, -3.610273f, 0.3874579f, 0.2659675f);
        static Keyframe blueCurveOfRainbow_atLuminance040_keyframe6 = new Keyframe(0.6666666f, 0f, -17.39986f, 0f, 0.282023f, 0.01547652f);
        static Keyframe blueCurveOfRainbow_atLuminance040_keyframe7 = new Keyframe(1f, 0f, 0f, 0f, 0f, 0f);
        static AnimationCurve blueCurveOfRainbow_atLuminance040 = new AnimationCurve(blueCurveOfRainbow_atLuminance040_keyframe0, blueCurveOfRainbow_atLuminance040_keyframe1, blueCurveOfRainbow_atLuminance040_keyframe2, blueCurveOfRainbow_atLuminance040_keyframe3, blueCurveOfRainbow_atLuminance040_keyframe4, blueCurveOfRainbow_atLuminance040_keyframe5, blueCurveOfRainbow_atLuminance040_keyframe6, blueCurveOfRainbow_atLuminance040_keyframe7);

        static Keyframe luminanceTargetCurveOfRainbow_atLuminance040_keyframe0 = new Keyframe(0f, 1f, 0f, 0.006816681f, 0f, 0.2125848f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance040_keyframe1 = new Keyframe(0.33333f, 1.3f, 4.02498f, -4.666481f, 0.215308f, 0.3330204f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance040_keyframe2 = new Keyframe(0.5f, 0.8f, 1.246914f, 1.343871f, 0.4983332f, 0.3648783f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance040_keyframe3 = new Keyframe(0.66666f, 0.8f, 0.1161369f, 8.206551f, 0.08776538f, 0.174981f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance040_keyframe4 = new Keyframe(0.7430212f, 1.022459f, 1.236433f, 1.362377f, 0.3987748f, 0.5715872f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance040_keyframe5 = new Keyframe(0.833333f, 1.168779f, 5.622433f, -0.4000894f, 0.2310172f, 0.3592924f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance040_keyframe6 = new Keyframe(0.9692383f, 1f, -10.4096f, 0f, 0.1525469f, 0f);
        static AnimationCurve luminanceTargetCurveOfRainbow_atLuminance040 = new AnimationCurve(luminanceTargetCurveOfRainbow_atLuminance040_keyframe0, luminanceTargetCurveOfRainbow_atLuminance040_keyframe1, luminanceTargetCurveOfRainbow_atLuminance040_keyframe2, luminanceTargetCurveOfRainbow_atLuminance040_keyframe3, luminanceTargetCurveOfRainbow_atLuminance040_keyframe4, luminanceTargetCurveOfRainbow_atLuminance040_keyframe5, luminanceTargetCurveOfRainbow_atLuminance040_keyframe6);

        static Keyframe redCurveOfRainbow_atLuminance0325_keyframe0 = new Keyframe(0f, 1f, 0f, 0f, 0f, 0.0267929f);
        static Keyframe redCurveOfRainbow_atLuminance0325_keyframe1 = new Keyframe(0.1666666f, 1f, 0f, -62.98078f, 0.03435588f, 0.05243218f);
        static Keyframe redCurveOfRainbow_atLuminance0325_keyframe2 = new Keyframe(0.2312603f, 0.4656304f, -5.133174f, -5.133174f, 0.3522511f, 0.3878298f);
        static Keyframe redCurveOfRainbow_atLuminance0325_keyframe3 = new Keyframe(0.3333333f, 0.1f, -4.912827f, 0f, 0.436582f, 0.01367907f);
        static Keyframe redCurveOfRainbow_atLuminance0325_keyframe4 = new Keyframe(0.41f, 0f, -0.2348027f, 0.07194741f, 0.7860962f, 0.3642307f);
        static Keyframe redCurveOfRainbow_atLuminance0325_keyframe5 = new Keyframe(0.5f, 0.15f, 12.79003f, -32.79271f, 0.124777f, 0.05083349f);
        static Keyframe redCurveOfRainbow_atLuminance0325_keyframe6 = new Keyframe(0.5946785f, 0.001490435f, -0.1213733f, -0.03375284f, 0.4860174f, 0.2922802f);
        static Keyframe redCurveOfRainbow_atLuminance0325_keyframe7 = new Keyframe(0.6666666f, 0.1f, 8.413258f, 5.895631f, 0.1535204f, 0.445222f);
        static Keyframe redCurveOfRainbow_atLuminance0325_keyframe8 = new Keyframe(0.704013f, 0.3859394f, 6.607747f, 4.422125f, 0.2726335f, 0.5933781f);
        static Keyframe redCurveOfRainbow_atLuminance0325_keyframe9 = new Keyframe(0.833333f, 1f, 3.091813f, -0.03322281f, 0.772702f, 0.4352952f);
        static Keyframe redCurveOfRainbow_atLuminance0325_keyframe10 = new Keyframe(1f, 1f, -0.1315563f, 0f, 0.2181816f, 0f);
        static AnimationCurve redCurveOfRainbow_atLuminance0325 = new AnimationCurve(redCurveOfRainbow_atLuminance0325_keyframe0, redCurveOfRainbow_atLuminance0325_keyframe1, redCurveOfRainbow_atLuminance0325_keyframe2, redCurveOfRainbow_atLuminance0325_keyframe3, redCurveOfRainbow_atLuminance0325_keyframe4, redCurveOfRainbow_atLuminance0325_keyframe5, redCurveOfRainbow_atLuminance0325_keyframe6, redCurveOfRainbow_atLuminance0325_keyframe7, redCurveOfRainbow_atLuminance0325_keyframe8, redCurveOfRainbow_atLuminance0325_keyframe9, redCurveOfRainbow_atLuminance0325_keyframe10);

        static Keyframe greenCurveOfRainbow_atLuminance0325_keyframe0 = new Keyframe(0f, 0f, 0f, 0f, 0f, 0.00778269f);
        static Keyframe greenCurveOfRainbow_atLuminance0325_keyframe1 = new Keyframe(0.257754f, 0f, 0f, 0f, 0f, 0.00778269f);
        static Keyframe greenCurveOfRainbow_atLuminance0325_keyframe2 = new Keyframe(0.333333f, 0.1f, 0f, 27.62473f, 0.003058169f, 0.01461566f);
        static Keyframe greenCurveOfRainbow_atLuminance0325_keyframe3 = new Keyframe(0.4309248f, 0.4177842f, 4.390889f, 4.307955f, 0.42647f, 0.4129865f);
        static Keyframe greenCurveOfRainbow_atLuminance0325_keyframe4 = new Keyframe(0.5010681f, 0.8421326f, 10.00402f, 0f, 0.2608117f, 0.007641078f);
        static Keyframe greenCurveOfRainbow_atLuminance0325_keyframe5 = new Keyframe(0.66666f, 0.8377342f, 0f, 31.03253f, 0.006811738f, 0.02356853f);
        static Keyframe greenCurveOfRainbow_atLuminance0325_keyframe6 = new Keyframe(0.8333333f, 1f, -1.449353f, -3.562928f, 0.4432741f, 0.2856619f);
        static Keyframe greenCurveOfRainbow_atLuminance0325_keyframe7 = new Keyframe(1f, 0.1f, -10.88458f, -5.54757f, 0.1603991f, 0f);
        static AnimationCurve greenCurveOfRainbow_atLuminance0325 = new AnimationCurve(greenCurveOfRainbow_atLuminance0325_keyframe0, greenCurveOfRainbow_atLuminance0325_keyframe1, greenCurveOfRainbow_atLuminance0325_keyframe2, greenCurveOfRainbow_atLuminance0325_keyframe3, greenCurveOfRainbow_atLuminance0325_keyframe4, greenCurveOfRainbow_atLuminance0325_keyframe5, greenCurveOfRainbow_atLuminance0325_keyframe6, greenCurveOfRainbow_atLuminance0325_keyframe7);

        static Keyframe blueCurveOfRainbow_atLuminance0325_keyframe0 = new Keyframe(0f, 0.1f, 5.103677f, 8.752334f, 0f, 0.1758645f);
        static Keyframe blueCurveOfRainbow_atLuminance0325_keyframe1 = new Keyframe(0.09132262f, 0.5363793f, 4.945463f, 4.945463f, 0.387399f, 0.3577237f);
        static Keyframe blueCurveOfRainbow_atLuminance0325_keyframe2 = new Keyframe(0.166666f, 1f, 5.200615f, 0f, 0.2529558f, 0.005642463f);
        static Keyframe blueCurveOfRainbow_atLuminance0325_keyframe3 = new Keyframe(0.5f, 1f, 0f, -5.748352f, 0.01999663f, 0.6307411f);
        static Keyframe blueCurveOfRainbow_atLuminance0325_keyframe4 = new Keyframe(0.5914994f, 0.4423819f, -4.936384f, -5.070686f, 0.3874579f, 0.2775002f);
        static Keyframe blueCurveOfRainbow_atLuminance0325_keyframe5 = new Keyframe(0.6666666f, 0.1f, -8.185369f, -4.944241f, 0.4558724f, 0.2835832f);
        static Keyframe blueCurveOfRainbow_atLuminance0325_keyframe6 = new Keyframe(0.7144385f, 0f, -0.6431804f, 0f, 0.5373129f, 0.01948809f);
        static Keyframe blueCurveOfRainbow_atLuminance0325_keyframe7 = new Keyframe(1.001099f, 0.1f, 0.419374f, 0f, 0.3433503f, 0f);
        static AnimationCurve blueCurveOfRainbow_atLuminance0325 = new AnimationCurve(blueCurveOfRainbow_atLuminance0325_keyframe0, blueCurveOfRainbow_atLuminance0325_keyframe1, blueCurveOfRainbow_atLuminance0325_keyframe2, blueCurveOfRainbow_atLuminance0325_keyframe3, blueCurveOfRainbow_atLuminance0325_keyframe4, blueCurveOfRainbow_atLuminance0325_keyframe5, blueCurveOfRainbow_atLuminance0325_keyframe6, blueCurveOfRainbow_atLuminance0325_keyframe7);

        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0325_keyframe0 = new Keyframe(0f, 0.7f, 0f, -1.716568f, 0f, 0.5813318f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0325_keyframe1 = new Keyframe(0.333333f, 1f, 2.374693f, 0f, 0.3904677f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0325_keyframe2 = new Keyframe(0.66666f, 1f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0325_keyframe3 = new Keyframe(0.833333f, 1.25f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0325_keyframe4 = new Keyframe(0.9692383f, 0.7f, -15.0162f, 0f, 0.1678049f, 0f);
        static AnimationCurve luminanceTargetCurveOfRainbow_atLuminance0325 = new AnimationCurve(luminanceTargetCurveOfRainbow_atLuminance0325_keyframe0, luminanceTargetCurveOfRainbow_atLuminance0325_keyframe1, luminanceTargetCurveOfRainbow_atLuminance0325_keyframe2, luminanceTargetCurveOfRainbow_atLuminance0325_keyframe3, luminanceTargetCurveOfRainbow_atLuminance0325_keyframe4);

        static Keyframe redCurveOfRainbow_atLuminance0275_keyframe0 = new Keyframe(0.00320816f, 0.7815781f, 0f, 0.3580622f, 0f, 0.1108752f);
        static Keyframe redCurveOfRainbow_atLuminance0275_keyframe1 = new Keyframe(0.1666666f, 1f, 0f, -62.98078f, 0.03435588f, 0.05243218f);
        static Keyframe redCurveOfRainbow_atLuminance0275_keyframe2 = new Keyframe(0.2312603f, 0.4656304f, -5.133174f, -5.133174f, 0.3522511f, 0.3878298f);
        static Keyframe redCurveOfRainbow_atLuminance0275_keyframe3 = new Keyframe(0.3333333f, 0.1f, -4.912827f, 0f, 0.436582f, 0.01367907f);
        static Keyframe redCurveOfRainbow_atLuminance0275_keyframe4 = new Keyframe(0.41f, 0f, -0.2348027f, 1.364813f, 0.7860962f, 0.3482143f);
        static Keyframe redCurveOfRainbow_atLuminance0275_keyframe5 = new Keyframe(0.5010681f, 0.15f, -0.003825905f, -2.337732f, 0.5930647f, 0.3713345f);
        static Keyframe redCurveOfRainbow_atLuminance0275_keyframe6 = new Keyframe(0.5946785f, 0.001490435f, -0.1213733f, -0.03375284f, 0.4860174f, 0.2922802f);
        static Keyframe redCurveOfRainbow_atLuminance0275_keyframe7 = new Keyframe(0.6666666f, 0.1f, 8.413258f, 5.895631f, 0.1535204f, 0.445222f);
        static Keyframe redCurveOfRainbow_atLuminance0275_keyframe8 = new Keyframe(0.704013f, 0.3859394f, 4.497206f, 5.18395f, 0.4940645f, 0.4610528f);
        static Keyframe redCurveOfRainbow_atLuminance0275_keyframe9 = new Keyframe(0.833333f, 1f, 3.091813f, -2.025448f, 0.772702f, 0.289987f);
        static Keyframe redCurveOfRainbow_atLuminance0275_keyframe10 = new Keyframe(1.001038f, 0.7013932f, 0.06829919f, 0f, 0.2863593f, 0f);
        static AnimationCurve redCurveOfRainbow_atLuminance0275 = new AnimationCurve(redCurveOfRainbow_atLuminance0275_keyframe0, redCurveOfRainbow_atLuminance0275_keyframe1, redCurveOfRainbow_atLuminance0275_keyframe2, redCurveOfRainbow_atLuminance0275_keyframe3, redCurveOfRainbow_atLuminance0275_keyframe4, redCurveOfRainbow_atLuminance0275_keyframe5, redCurveOfRainbow_atLuminance0275_keyframe6, redCurveOfRainbow_atLuminance0275_keyframe7, redCurveOfRainbow_atLuminance0275_keyframe8, redCurveOfRainbow_atLuminance0275_keyframe9, redCurveOfRainbow_atLuminance0275_keyframe10);

        static Keyframe greenCurveOfRainbow_atLuminance0275_keyframe0 = new Keyframe(0f, 0.25f, 0f, -5.423751f, 0f, 0.2033195f);
        static Keyframe greenCurveOfRainbow_atLuminance0275_keyframe1 = new Keyframe(0.257754f, 0f, 0f, 0f, 0f, 0.00778269f);
        static Keyframe greenCurveOfRainbow_atLuminance0275_keyframe2 = new Keyframe(0.333333f, 0.1f, 0f, 16.91939f, 0.003058169f, 0.03653381f);
        static Keyframe greenCurveOfRainbow_atLuminance0275_keyframe3 = new Keyframe(0.4309248f, 0.4177842f, 4.390889f, 4.307955f, 0.42647f, 0.4129865f);
        static Keyframe greenCurveOfRainbow_atLuminance0275_keyframe4 = new Keyframe(0.5010681f, 0.8421326f, 10.00402f, 0f, 0.2608117f, 0.007641078f);
        static Keyframe greenCurveOfRainbow_atLuminance0275_keyframe5 = new Keyframe(0.66666f, 0.8377342f, 0f, 31.03253f, 0.006811738f, 0.02356853f);
        static Keyframe greenCurveOfRainbow_atLuminance0275_keyframe6 = new Keyframe(0.8333333f, 1f, -1.449353f, -3.562928f, 0.4432741f, 0.2856619f);
        static Keyframe greenCurveOfRainbow_atLuminance0275_keyframe7 = new Keyframe(1f, 0.25f, -0.5542867f, -5.54757f, 0.3016044f, 0f);
        static AnimationCurve greenCurveOfRainbow_atLuminance0275 = new AnimationCurve(greenCurveOfRainbow_atLuminance0275_keyframe0, greenCurveOfRainbow_atLuminance0275_keyframe1, greenCurveOfRainbow_atLuminance0275_keyframe2, greenCurveOfRainbow_atLuminance0275_keyframe3, greenCurveOfRainbow_atLuminance0275_keyframe4, greenCurveOfRainbow_atLuminance0275_keyframe5, greenCurveOfRainbow_atLuminance0275_keyframe6, greenCurveOfRainbow_atLuminance0275_keyframe7);

        static Keyframe blueCurveOfRainbow_atLuminance0275_keyframe0 = new Keyframe(0f, 0.25f, 5.103677f, 11.81485f, 0f, 0.1369982f);
        static Keyframe blueCurveOfRainbow_atLuminance0275_keyframe1 = new Keyframe(0.08596926f, 0.6425111f, 4.945463f, 3.843233f, 0.387399f, 0.4074239f);
        static Keyframe blueCurveOfRainbow_atLuminance0275_keyframe2 = new Keyframe(0.166666f, 1f, 3.406605f, 0f, 0.8813255f, 0.005642463f);
        static Keyframe blueCurveOfRainbow_atLuminance0275_keyframe3 = new Keyframe(0.5f, 1f, 0f, -4.492854f, 0.01999663f, 0.7253633f);
        static Keyframe blueCurveOfRainbow_atLuminance0275_keyframe4 = new Keyframe(0.6107255f, 0.489562f, -4.936384f, -5.070686f, 0.3874579f, 0.2775002f);
        static Keyframe blueCurveOfRainbow_atLuminance0275_keyframe5 = new Keyframe(0.6666666f, 0.1f, -8.185369f, -4.944241f, 0.4558724f, 0.2835832f);
        static Keyframe blueCurveOfRainbow_atLuminance0275_keyframe6 = new Keyframe(0.7144385f, 0f, -0.6431804f, 1.59352f, 0.5373129f, 0.4285358f);
        static Keyframe blueCurveOfRainbow_atLuminance0275_keyframe7 = new Keyframe(1.001099f, 0.25f, 3.545919f, 0f, 0.306275f, 0f);
        static AnimationCurve blueCurveOfRainbow_atLuminance0275 = new AnimationCurve(blueCurveOfRainbow_atLuminance0275_keyframe0, blueCurveOfRainbow_atLuminance0275_keyframe1, blueCurveOfRainbow_atLuminance0275_keyframe2, blueCurveOfRainbow_atLuminance0275_keyframe3, blueCurveOfRainbow_atLuminance0275_keyframe4, blueCurveOfRainbow_atLuminance0275_keyframe5, blueCurveOfRainbow_atLuminance0275_keyframe6, blueCurveOfRainbow_atLuminance0275_keyframe7);

        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0275_keyframe0 = new Keyframe(-0.001037598f, 0.75f, 0f, -14.63627f, 0f, 0.03838513f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0275_keyframe1 = new Keyframe(0.1882049f, 0.497113f, -0.6622246f, -0.2468882f, 0.6106648f, 0.6339823f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0275_keyframe2 = new Keyframe(0.333333f, 1f, 18.33286f, 0f, 0.06071621f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0275_keyframe3 = new Keyframe(0.5f, 0.96f, -2.330969f, 7.099766f, 0.1140539f, 0.04144035f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0275_keyframe4 = new Keyframe(0.66666f, 1f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0275_keyframe5 = new Keyframe(0.833333f, 1.212746f, 0f, -1.323475f, 0f, 0.4565187f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0275_keyframe6 = new Keyframe(0.9129034f, 0.9447585f, -3.983558f, -3.688873f, 0.3326225f, 0.3312222f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0275_keyframe7 = new Keyframe(0.9692383f, 0.75f, -1.734095f, 0f, 0.422055f, 0f);
        static AnimationCurve luminanceTargetCurveOfRainbow_atLuminance0275 = new AnimationCurve(luminanceTargetCurveOfRainbow_atLuminance0275_keyframe0, luminanceTargetCurveOfRainbow_atLuminance0275_keyframe1, luminanceTargetCurveOfRainbow_atLuminance0275_keyframe2, luminanceTargetCurveOfRainbow_atLuminance0275_keyframe3, luminanceTargetCurveOfRainbow_atLuminance0275_keyframe4, luminanceTargetCurveOfRainbow_atLuminance0275_keyframe5, luminanceTargetCurveOfRainbow_atLuminance0275_keyframe6, luminanceTargetCurveOfRainbow_atLuminance0275_keyframe7);

        static Keyframe redCurveOfRainbow_atLuminance0175_keyframe0 = new Keyframe(0.00320816f, 0.7815781f, 0f, 0.3580622f, 0f, 0.1108752f);
        static Keyframe redCurveOfRainbow_atLuminance0175_keyframe1 = new Keyframe(0.1666666f, 1f, 0f, -8.082616f, 0.03435588f, 0.3589919f);
        static Keyframe redCurveOfRainbow_atLuminance0175_keyframe2 = new Keyframe(0.2643566f, 0.4491204f, -3.816614f, -5.133174f, 0.3135815f, 0.3878298f);
        static Keyframe redCurveOfRainbow_atLuminance0175_keyframe3 = new Keyframe(0.3333333f, 0.151f, -4.912827f, 0f, 0.436582f, 0.01367907f);
        static Keyframe redCurveOfRainbow_atLuminance0175_keyframe4 = new Keyframe(0.41f, 0f, -0.2348027f, 1.364813f, 0.7860962f, 0.3482143f);
        static Keyframe redCurveOfRainbow_atLuminance0175_keyframe5 = new Keyframe(0.5010681f, 0.15f, -0.003825905f, -2.337732f, 0.5930647f, 0.3713345f);
        static Keyframe redCurveOfRainbow_atLuminance0175_keyframe6 = new Keyframe(0.5946785f, 0.001490435f, -0.1213733f, -0.03375284f, 0.4860174f, 0.2922802f);
        static Keyframe redCurveOfRainbow_atLuminance0175_keyframe7 = new Keyframe(0.6666666f, 0.1f, 8.413258f, 5.895631f, 0.1535204f, 0.445222f);
        static Keyframe redCurveOfRainbow_atLuminance0175_keyframe8 = new Keyframe(0.704013f, 0.3859394f, 4.497206f, 5.18395f, 0.4940645f, 0.4610528f);
        static Keyframe redCurveOfRainbow_atLuminance0175_keyframe9 = new Keyframe(0.833333f, 1f, 3.091813f, -2.025448f, 0.772702f, 0.289987f);
        static Keyframe redCurveOfRainbow_atLuminance0175_keyframe10 = new Keyframe(1.001038f, 0.7013932f, 0.06829919f, 0f, 0.2863593f, 0f);
        static AnimationCurve redCurveOfRainbow_atLuminance0175 = new AnimationCurve(redCurveOfRainbow_atLuminance0175_keyframe0, redCurveOfRainbow_atLuminance0175_keyframe1, redCurveOfRainbow_atLuminance0175_keyframe2, redCurveOfRainbow_atLuminance0175_keyframe3, redCurveOfRainbow_atLuminance0175_keyframe4, redCurveOfRainbow_atLuminance0175_keyframe5, redCurveOfRainbow_atLuminance0175_keyframe6, redCurveOfRainbow_atLuminance0175_keyframe7, redCurveOfRainbow_atLuminance0175_keyframe8, redCurveOfRainbow_atLuminance0175_keyframe9, redCurveOfRainbow_atLuminance0175_keyframe10);

        static Keyframe greenCurveOfRainbow_atLuminance0175_keyframe0 = new Keyframe(0f, 0.25f, 0f, -1.662345f, 0f, 0.6721992f);
        static Keyframe greenCurveOfRainbow_atLuminance0175_keyframe1 = new Keyframe(0.257754f, 0f, 0f, 0.2219158f, 0f, 0.3396244f);
        static Keyframe greenCurveOfRainbow_atLuminance0175_keyframe2 = new Keyframe(0.333333f, 0.1514221f, 2.628582f, 16.91939f, 0.4622616f, 0.03653381f);
        static Keyframe greenCurveOfRainbow_atLuminance0175_keyframe3 = new Keyframe(0.4309248f, 0.4177842f, 4.390889f, 4.307955f, 0.42647f, 0.4129865f);
        static Keyframe greenCurveOfRainbow_atLuminance0175_keyframe4 = new Keyframe(0.5010681f, 0.8421326f, 10.00402f, 0f, 0.2608117f, 0.007641078f);
        static Keyframe greenCurveOfRainbow_atLuminance0175_keyframe5 = new Keyframe(0.66666f, 0.8377342f, 0f, 31.03253f, 0.006811738f, 0.02356853f);
        static Keyframe greenCurveOfRainbow_atLuminance0175_keyframe6 = new Keyframe(0.8333333f, 1f, -1.449353f, -3.562928f, 0.4432741f, 0.2856619f);
        static Keyframe greenCurveOfRainbow_atLuminance0175_keyframe7 = new Keyframe(1f, 0.25f, -0.5542867f, -5.54757f, 0.3016044f, 0f);
        static AnimationCurve greenCurveOfRainbow_atLuminance0175 = new AnimationCurve(greenCurveOfRainbow_atLuminance0175_keyframe0, greenCurveOfRainbow_atLuminance0175_keyframe1, greenCurveOfRainbow_atLuminance0175_keyframe2, greenCurveOfRainbow_atLuminance0175_keyframe3, greenCurveOfRainbow_atLuminance0175_keyframe4, greenCurveOfRainbow_atLuminance0175_keyframe5, greenCurveOfRainbow_atLuminance0175_keyframe6, greenCurveOfRainbow_atLuminance0175_keyframe7);

        static Keyframe blueCurveOfRainbow_atLuminance0175_keyframe0 = new Keyframe(0f, 0.25f, 5.103677f, 11.81485f, 0f, 0.1369982f);
        static Keyframe blueCurveOfRainbow_atLuminance0175_keyframe1 = new Keyframe(0.08489856f, 0.6495866f, 2.646874f, 3.249499f, 0.5640966f, 0.680275f);
        static Keyframe blueCurveOfRainbow_atLuminance0175_keyframe2 = new Keyframe(0.1473939f, 0.9339607f, 4.998507f, 19.57937f, 0.4651634f, 0.06425192f);
        static Keyframe blueCurveOfRainbow_atLuminance0175_keyframe3 = new Keyframe(0.3030017f, 0.837265f, 0.1810475f, 0f, 0.531534f, 0.005642463f);
        static Keyframe blueCurveOfRainbow_atLuminance0175_keyframe4 = new Keyframe(0.5f, 1f, 0f, -4.492854f, 0.01999663f, 0.7253633f);
        static Keyframe blueCurveOfRainbow_atLuminance0175_keyframe5 = new Keyframe(0.6107255f, 0.489562f, -4.936384f, -5.070686f, 0.3874579f, 0.2775002f);
        static Keyframe blueCurveOfRainbow_atLuminance0175_keyframe6 = new Keyframe(0.6666666f, 0.1f, -8.185369f, -4.944241f, 0.4558724f, 0.2835832f);
        static Keyframe blueCurveOfRainbow_atLuminance0175_keyframe7 = new Keyframe(0.7144385f, 0f, -0.6431804f, 1.59352f, 0.5373129f, 0.4285358f);
        static Keyframe blueCurveOfRainbow_atLuminance0175_keyframe8 = new Keyframe(1.001099f, 0.25f, 3.545919f, 0f, 0.306275f, 0f);
        static AnimationCurve blueCurveOfRainbow_atLuminance0175 = new AnimationCurve(blueCurveOfRainbow_atLuminance0175_keyframe0, blueCurveOfRainbow_atLuminance0175_keyframe1, blueCurveOfRainbow_atLuminance0175_keyframe2, blueCurveOfRainbow_atLuminance0175_keyframe3, blueCurveOfRainbow_atLuminance0175_keyframe4, blueCurveOfRainbow_atLuminance0175_keyframe5, blueCurveOfRainbow_atLuminance0175_keyframe6, blueCurveOfRainbow_atLuminance0175_keyframe7, blueCurveOfRainbow_atLuminance0175_keyframe8);

        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0175_keyframe0 = new Keyframe(0f, 0.7969055f, 0f, -1.238152f, 0f, 0.2999468f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0175_keyframe1 = new Keyframe(0.1353185f, 0.5177611f, -3.722492f, -2.240829f, 0.1552123f, 0.2174204f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0175_keyframe2 = new Keyframe(0.289876f, 0.3924408f, 1.735897f, 1.634704f, 0.3291568f, 0.3739646f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0175_keyframe3 = new Keyframe(0.5f, 0.96f, -2.330969f, 7.099766f, 0.1140539f, 0.04144035f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0175_keyframe4 = new Keyframe(0.66666f, 1f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0175_keyframe5 = new Keyframe(0.833333f, 1.212746f, 0f, -1.323475f, 0f, 0.4565187f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0175_keyframe6 = new Keyframe(0.9129034f, 0.9447585f, -3.983558f, -3.688873f, 0.3326225f, 0.3312222f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance0175_keyframe7 = new Keyframe(0.9692383f, 0.8300476f, -1.734095f, 0f, 0.422055f, 0f);
        static AnimationCurve luminanceTargetCurveOfRainbow_atLuminance0175 = new AnimationCurve(luminanceTargetCurveOfRainbow_atLuminance0175_keyframe0, luminanceTargetCurveOfRainbow_atLuminance0175_keyframe1, luminanceTargetCurveOfRainbow_atLuminance0175_keyframe2, luminanceTargetCurveOfRainbow_atLuminance0175_keyframe3, luminanceTargetCurveOfRainbow_atLuminance0175_keyframe4, luminanceTargetCurveOfRainbow_atLuminance0175_keyframe5, luminanceTargetCurveOfRainbow_atLuminance0175_keyframe6, luminanceTargetCurveOfRainbow_atLuminance0175_keyframe7);

        static Keyframe redCurveOfRainbow_atLuminance005_keyframe0 = new Keyframe(0.00320816f, 0.7815781f, 0f, 0.3580622f, 0f, 0.1108752f);
        static Keyframe redCurveOfRainbow_atLuminance005_keyframe1 = new Keyframe(0.1666666f, 1f, 0f, -8.082616f, 0.03435588f, 0.3589919f);
        static Keyframe redCurveOfRainbow_atLuminance005_keyframe2 = new Keyframe(0.2643566f, 0.4491204f, -3.816614f, -5.133174f, 0.3135815f, 0.3878298f);
        static Keyframe redCurveOfRainbow_atLuminance005_keyframe3 = new Keyframe(0.3333333f, 0.151f, -4.912827f, 0f, 0.436582f, 0.01367907f);
        static Keyframe redCurveOfRainbow_atLuminance005_keyframe4 = new Keyframe(0.41f, 0f, -0.2348027f, 1.364813f, 0.7860962f, 0.3482143f);
        static Keyframe redCurveOfRainbow_atLuminance005_keyframe5 = new Keyframe(0.5010681f, 0.15f, -0.003825905f, -2.337732f, 0.5930647f, 0.3713345f);
        static Keyframe redCurveOfRainbow_atLuminance005_keyframe6 = new Keyframe(0.5946785f, 0.001490435f, -0.1213733f, -0.03375284f, 0.4860174f, 0.2922802f);
        static Keyframe redCurveOfRainbow_atLuminance005_keyframe7 = new Keyframe(0.6666666f, 0.1f, 8.413258f, 5.895631f, 0.1535204f, 0.445222f);
        static Keyframe redCurveOfRainbow_atLuminance005_keyframe8 = new Keyframe(0.704013f, 0.3859394f, 4.497206f, 5.18395f, 0.4940645f, 0.4610528f);
        static Keyframe redCurveOfRainbow_atLuminance005_keyframe9 = new Keyframe(0.833333f, 1f, 3.091813f, -2.025448f, 0.772702f, 0.289987f);
        static Keyframe redCurveOfRainbow_atLuminance005_keyframe10 = new Keyframe(1.001038f, 0.7013932f, 0.06829919f, 0f, 0.2863593f, 0f);
        static AnimationCurve redCurveOfRainbow_atLuminance005 = new AnimationCurve(redCurveOfRainbow_atLuminance005_keyframe0, redCurveOfRainbow_atLuminance005_keyframe1, redCurveOfRainbow_atLuminance005_keyframe2, redCurveOfRainbow_atLuminance005_keyframe3, redCurveOfRainbow_atLuminance005_keyframe4, redCurveOfRainbow_atLuminance005_keyframe5, redCurveOfRainbow_atLuminance005_keyframe6, redCurveOfRainbow_atLuminance005_keyframe7, redCurveOfRainbow_atLuminance005_keyframe8, redCurveOfRainbow_atLuminance005_keyframe9, redCurveOfRainbow_atLuminance005_keyframe10);

        static Keyframe greenCurveOfRainbow_atLuminance005_keyframe0 = new Keyframe(0f, 0.25f, 0f, -1.662345f, 0f, 0.6721992f);
        static Keyframe greenCurveOfRainbow_atLuminance005_keyframe1 = new Keyframe(0.257754f, 0f, 0f, 0.2219158f, 0f, 0.3396244f);
        static Keyframe greenCurveOfRainbow_atLuminance005_keyframe2 = new Keyframe(0.333333f, 0.1514221f, 2.628582f, 16.91939f, 0.4622616f, 0.03653381f);
        static Keyframe greenCurveOfRainbow_atLuminance005_keyframe3 = new Keyframe(0.4309248f, 0.4177842f, 4.390889f, 4.307955f, 0.42647f, 0.4129865f);
        static Keyframe greenCurveOfRainbow_atLuminance005_keyframe4 = new Keyframe(0.5010681f, 0.8421326f, 10.00402f, 0f, 0.2608117f, 0.007641078f);
        static Keyframe greenCurveOfRainbow_atLuminance005_keyframe5 = new Keyframe(0.66666f, 0.8377342f, 0f, 31.03253f, 0.006811738f, 0.02356853f);
        static Keyframe greenCurveOfRainbow_atLuminance005_keyframe6 = new Keyframe(0.8333333f, 1f, -1.449353f, -3.562928f, 0.4432741f, 0.2856619f);
        static Keyframe greenCurveOfRainbow_atLuminance005_keyframe7 = new Keyframe(1f, 0.25f, -0.5542867f, -5.54757f, 0.3016044f, 0f);
        static AnimationCurve greenCurveOfRainbow_atLuminance005 = new AnimationCurve(greenCurveOfRainbow_atLuminance005_keyframe0, greenCurveOfRainbow_atLuminance005_keyframe1, greenCurveOfRainbow_atLuminance005_keyframe2, greenCurveOfRainbow_atLuminance005_keyframe3, greenCurveOfRainbow_atLuminance005_keyframe4, greenCurveOfRainbow_atLuminance005_keyframe5, greenCurveOfRainbow_atLuminance005_keyframe6, greenCurveOfRainbow_atLuminance005_keyframe7);

        static Keyframe blueCurveOfRainbow_atLuminance005_keyframe0 = new Keyframe(0f, 0.25f, 5.103677f, 11.81485f, 0f, 0.1369982f);
        static Keyframe blueCurveOfRainbow_atLuminance005_keyframe1 = new Keyframe(0.08489856f, 0.6495866f, 2.646874f, 3.249499f, 0.5640966f, 0.680275f);
        static Keyframe blueCurveOfRainbow_atLuminance005_keyframe2 = new Keyframe(0.1473939f, 0.9339607f, 4.998507f, 3.770786f, 0.4651634f, 0.1555204f);
        static Keyframe blueCurveOfRainbow_atLuminance005_keyframe3 = new Keyframe(0.3392435f, 1.17904f, 3.387797f, -3.471945f, 0.6513343f, 0.5786006f);
        static Keyframe blueCurveOfRainbow_atLuminance005_keyframe4 = new Keyframe(0.5f, 1f, 0f, -4.492854f, 0.01999663f, 0.7253633f);
        static Keyframe blueCurveOfRainbow_atLuminance005_keyframe5 = new Keyframe(0.6107255f, 0.489562f, -4.936384f, -5.070686f, 0.3874579f, 0.2775002f);
        static Keyframe blueCurveOfRainbow_atLuminance005_keyframe6 = new Keyframe(0.6666666f, 0.1f, -8.185369f, -4.944241f, 0.4558724f, 0.2835832f);
        static Keyframe blueCurveOfRainbow_atLuminance005_keyframe7 = new Keyframe(0.7144385f, 0f, -0.6431804f, 1.59352f, 0.5373129f, 0.4285358f);
        static Keyframe blueCurveOfRainbow_atLuminance005_keyframe8 = new Keyframe(1.001099f, 0.25f, 3.545919f, 0f, 0.306275f, 0f);
        static AnimationCurve blueCurveOfRainbow_atLuminance005 = new AnimationCurve(blueCurveOfRainbow_atLuminance005_keyframe0, blueCurveOfRainbow_atLuminance005_keyframe1, blueCurveOfRainbow_atLuminance005_keyframe2, blueCurveOfRainbow_atLuminance005_keyframe3, blueCurveOfRainbow_atLuminance005_keyframe4, blueCurveOfRainbow_atLuminance005_keyframe5, blueCurveOfRainbow_atLuminance005_keyframe6, blueCurveOfRainbow_atLuminance005_keyframe7, blueCurveOfRainbow_atLuminance005_keyframe8);

        static Keyframe luminanceTargetCurveOfRainbow_atLuminance005_keyframe0 = new Keyframe(0f, 0.7969055f, 0f, -0.4345227f, 0f, 0.4554668f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance005_keyframe1 = new Keyframe(0.1477589f, 0.5718302f, -0.3365634f, -0.877695f, 0.3685954f, 0.3002268f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance005_keyframe2 = new Keyframe(0.3189052f, 0.4952482f, 1.555321f, 1.448426f, 0.3855759f, 0.396134f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance005_keyframe3 = new Keyframe(0.500001f, 0.9960338f, 1.386169f, 7.099766f, 0.3321387f, 0.04144035f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance005_keyframe4 = new Keyframe(0.6656224f, 0.9299316f, 0f, 0f, 0f, 0f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance005_keyframe5 = new Keyframe(0.822957f, 1.046589f, -0.107637f, -1.172717f, 0.2380097f, 0.2087598f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance005_keyframe6 = new Keyframe(0.9129034f, 0.9447585f, -3.983558f, -3.688873f, 0.3326225f, 0.3312222f);
        static Keyframe luminanceTargetCurveOfRainbow_atLuminance005_keyframe7 = new Keyframe(0.9692383f, 0.8300476f, -1.403513f, 0f, 0.460025f, 0f);
        static AnimationCurve luminanceTargetCurveOfRainbow_atLuminance005 = new AnimationCurve(luminanceTargetCurveOfRainbow_atLuminance005_keyframe0, luminanceTargetCurveOfRainbow_atLuminance005_keyframe1, luminanceTargetCurveOfRainbow_atLuminance005_keyframe2, luminanceTargetCurveOfRainbow_atLuminance005_keyframe3, luminanceTargetCurveOfRainbow_atLuminance005_keyframe4, luminanceTargetCurveOfRainbow_atLuminance005_keyframe5, luminanceTargetCurveOfRainbow_atLuminance005_keyframe6, luminanceTargetCurveOfRainbow_atLuminance005_keyframe7);

        public static Color GetIteratingRainbowColor(int iterationStep, float alphaOfGeneratedColor, int numberOfColorsPerSpectrumPass, float lowerBorderOfColorLoop, float higherBorderOfColorLoop, bool sawToothTransition, float forceLuminance)
        {
            numberOfColorsPerSpectrumPass = Mathf.Max(numberOfColorsPerSpectrumPass, 2);
            float progressThroughRainbow_exceedingThe0to1bounds = ((float)iterationStep) / (float)numberOfColorsPerSpectrumPass;
            float progressThroughRainbow_as0to1_ifFullSpectrumIsUsed;
            if (sawToothTransition)
            {
                progressThroughRainbow_as0to1_ifFullSpectrumIsUsed = UtilitiesDXXL_Math.Loop_floatIntoSpanFrom_0_to_1(progressThroughRainbow_exceedingThe0to1bounds);
                if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(progressThroughRainbow_as0to1_ifFullSpectrumIsUsed, 1.0f))
                {
                    progressThroughRainbow_as0to1_ifFullSpectrumIsUsed = 0.0f; //-> prevents slight inconsistency for the case where the "iterationStep" transitions from negative to positive
                }
            }
            else
            {
                float progressThroughRainbow_as0to1_ifFullSpectrumIsUsed_cappedTo2 = UtilitiesDXXL_Math.Loop_floatIntoSpanFrom_0_to_x(progressThroughRainbow_exceedingThe0to1bounds, 2.0f);
                if (progressThroughRainbow_as0to1_ifFullSpectrumIsUsed_cappedTo2 > 1.0f)
                {
                    progressThroughRainbow_as0to1_ifFullSpectrumIsUsed = 2.0f - progressThroughRainbow_as0to1_ifFullSpectrumIsUsed_cappedTo2;
                }
                else
                {
                    progressThroughRainbow_as0to1_ifFullSpectrumIsUsed = progressThroughRainbow_as0to1_ifFullSpectrumIsUsed_cappedTo2;
                }
            }

            float spanOfColorLoop = higherBorderOfColorLoop - lowerBorderOfColorLoop;
            float progressThroughRainbow_as0to1InRGBSpace = lowerBorderOfColorLoop + spanOfColorLoop * progressThroughRainbow_as0to1_ifFullSpectrumIsUsed;
            if (progressThroughRainbow_as0to1InRGBSpace < 0.0f)
            {
                progressThroughRainbow_as0to1InRGBSpace = progressThroughRainbow_as0to1InRGBSpace + 1.0f;
            }

            float progressThroughRainbow_as0to1InRBGSpace = 1.0f - progressThroughRainbow_as0to1InRGBSpace; //The AnimationCurves were generated in RBG-space (meaning R at 0, B at 0.3333 and G at 0.6666), while the industry convention is RGB (R at 0, G at 0.3333 and B at 0.6666)
            Color generatedRainbowColor = default;
            if (UtilitiesDXXL_Math.ApproximatelyZero(forceLuminance))
            {
                generatedRainbowColor.r = redCurveOfRainbow_atLuminanceNotForced.Evaluate(progressThroughRainbow_as0to1InRBGSpace);
                generatedRainbowColor.g = greenCurveOfRainbow_atLuminanceNotForced.Evaluate(progressThroughRainbow_as0to1InRBGSpace);
                generatedRainbowColor.b = blueCurveOfRainbow_atLuminanceNotForced.Evaluate(progressThroughRainbow_as0to1InRBGSpace);
                generatedRainbowColor = SeededColorGenerator.ForceApproxLuminance(generatedRainbowColor, forceLuminance);
            }
            else
            {
                if (forceLuminance > 0.825f)
                {
                    generatedRainbowColor.r = redCurveOfRainbow_atLuminance0825.Evaluate(progressThroughRainbow_as0to1InRBGSpace);
                    generatedRainbowColor.g = greenCurveOfRainbow_atLuminance0825.Evaluate(progressThroughRainbow_as0to1InRBGSpace);
                    generatedRainbowColor.b = blueCurveOfRainbow_atLuminance0825.Evaluate(progressThroughRainbow_as0to1InRBGSpace);
                    generatedRainbowColor = SeededColorGenerator.ForceApproxLuminance(generatedRainbowColor, forceLuminance * luminanceTargetCurveOfRainbow_atLuminance0825.Evaluate(progressThroughRainbow_as0to1InRBGSpace));
                }
                else
                {
                    if (forceLuminance > 0.675f)
                    {
                        generatedRainbowColor = LerpColorBetweenLuminanceAnchorPoints(0.675f, 0.825f, redCurveOfRainbow_atLuminance0675, greenCurveOfRainbow_atLuminance0675, blueCurveOfRainbow_atLuminance0675, luminanceTargetCurveOfRainbow_atLuminance0675, redCurveOfRainbow_atLuminance0825, greenCurveOfRainbow_atLuminance0825, blueCurveOfRainbow_atLuminance0825, luminanceTargetCurveOfRainbow_atLuminance0825, progressThroughRainbow_as0to1InRBGSpace, forceLuminance);
                    }
                    else
                    {
                        if (forceLuminance > 0.5f)
                        {
                            generatedRainbowColor = LerpColorBetweenLuminanceAnchorPoints(0.5f, 0.675f, redCurveOfRainbow_atLuminance05, greenCurveOfRainbow_atLuminance05, blueCurveOfRainbow_atLuminance05, luminanceTargetCurveOfRainbow_atLuminance05, redCurveOfRainbow_atLuminance0675, greenCurveOfRainbow_atLuminance0675, blueCurveOfRainbow_atLuminance0675, luminanceTargetCurveOfRainbow_atLuminance0675, progressThroughRainbow_as0to1InRBGSpace, forceLuminance);
                        }
                        else
                        {
                            if (forceLuminance > 0.44f)
                            {
                                generatedRainbowColor = LerpColorBetweenLuminanceAnchorPoints(0.44f, 0.5f, redCurveOfRainbow_atLuminance044, greenCurveOfRainbow_atLuminance044, blueCurveOfRainbow_atLuminance044, luminanceTargetCurveOfRainbow_atLuminance044, redCurveOfRainbow_atLuminance05, greenCurveOfRainbow_atLuminance05, blueCurveOfRainbow_atLuminance05, luminanceTargetCurveOfRainbow_atLuminance05, progressThroughRainbow_as0to1InRBGSpace, forceLuminance);
                            }
                            else
                            {
                                if (forceLuminance > 0.40f)
                                {
                                    generatedRainbowColor = LerpColorBetweenLuminanceAnchorPoints(0.40f, 0.44f, redCurveOfRainbow_atLuminance040, greenCurveOfRainbow_atLuminance040, blueCurveOfRainbow_atLuminance040, luminanceTargetCurveOfRainbow_atLuminance040, redCurveOfRainbow_atLuminance044, greenCurveOfRainbow_atLuminance044, blueCurveOfRainbow_atLuminance044, luminanceTargetCurveOfRainbow_atLuminance044, progressThroughRainbow_as0to1InRBGSpace, forceLuminance);
                                }
                                else
                                {
                                    if (forceLuminance > 0.325f)
                                    {
                                        generatedRainbowColor = LerpColorBetweenLuminanceAnchorPoints(0.325f, 0.40f, redCurveOfRainbow_atLuminance0325, greenCurveOfRainbow_atLuminance0325, blueCurveOfRainbow_atLuminance0325, luminanceTargetCurveOfRainbow_atLuminance0325, redCurveOfRainbow_atLuminance040, greenCurveOfRainbow_atLuminance040, blueCurveOfRainbow_atLuminance040, luminanceTargetCurveOfRainbow_atLuminance040, progressThroughRainbow_as0to1InRBGSpace, forceLuminance);
                                    }
                                    else
                                    {
                                        if (forceLuminance > 0.275f)
                                        {
                                            generatedRainbowColor = LerpColorBetweenLuminanceAnchorPoints(0.275f, 0.325f, redCurveOfRainbow_atLuminance0275, greenCurveOfRainbow_atLuminance0275, blueCurveOfRainbow_atLuminance0275, luminanceTargetCurveOfRainbow_atLuminance0275, redCurveOfRainbow_atLuminance0325, greenCurveOfRainbow_atLuminance0325, blueCurveOfRainbow_atLuminance0325, luminanceTargetCurveOfRainbow_atLuminance0325, progressThroughRainbow_as0to1InRBGSpace, forceLuminance);
                                        }
                                        else
                                        {
                                            if (forceLuminance > 0.175f)
                                            {
                                                generatedRainbowColor = LerpColorBetweenLuminanceAnchorPoints(0.175f, 0.275f, redCurveOfRainbow_atLuminance0175, greenCurveOfRainbow_atLuminance0175, blueCurveOfRainbow_atLuminance0175, luminanceTargetCurveOfRainbow_atLuminance0175, redCurveOfRainbow_atLuminance0275, greenCurveOfRainbow_atLuminance0275, blueCurveOfRainbow_atLuminance0275, luminanceTargetCurveOfRainbow_atLuminance0275, progressThroughRainbow_as0to1InRBGSpace, forceLuminance);
                                            }
                                            else
                                            {
                                                if (forceLuminance > 0.05f)
                                                {
                                                    generatedRainbowColor = LerpColorBetweenLuminanceAnchorPoints(0.05f, 0.175f, redCurveOfRainbow_atLuminance005, greenCurveOfRainbow_atLuminance005, blueCurveOfRainbow_atLuminance005, luminanceTargetCurveOfRainbow_atLuminance005, redCurveOfRainbow_atLuminance0175, greenCurveOfRainbow_atLuminance0175, blueCurveOfRainbow_atLuminance0175, luminanceTargetCurveOfRainbow_atLuminance0175, progressThroughRainbow_as0to1InRBGSpace, forceLuminance);
                                                }
                                                else
                                                {
                                                    generatedRainbowColor.r = redCurveOfRainbow_atLuminance005.Evaluate(progressThroughRainbow_as0to1InRBGSpace);
                                                    generatedRainbowColor.g = greenCurveOfRainbow_atLuminance005.Evaluate(progressThroughRainbow_as0to1InRBGSpace);
                                                    generatedRainbowColor.b = blueCurveOfRainbow_atLuminance005.Evaluate(progressThroughRainbow_as0to1InRBGSpace);
                                                    generatedRainbowColor = SeededColorGenerator.ForceApproxLuminance(generatedRainbowColor, forceLuminance * luminanceTargetCurveOfRainbow_atLuminance005.Evaluate(progressThroughRainbow_as0to1InRBGSpace));
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

            generatedRainbowColor.a = alphaOfGeneratedColor;
            return generatedRainbowColor;
        }

        static Color LerpColorBetweenLuminanceAnchorPoints(float luminanceMarkingLowerAnchor, float luminanceMarkingHigherAnchor, AnimationCurve animCurve_lowerAnchor_red, AnimationCurve animCurve_lowerAnchor_green, AnimationCurve animCurve_lowerAnchor_blue, AnimationCurve animCurve_lowerAnchor_luminanceTargetValueModifier, AnimationCurve animCurve_higherAnchor_red, AnimationCurve animCurve_higherAnchor_green, AnimationCurve animCurve_higherAnchor_blue, AnimationCurve animCurve_higherAnchor_luminanceTargetValueModifier, float progressThroughRainbow_as0to1, float forceLuminance)
        {
            Color generatedRainbowColor_atLowerLuminanceAnchor = default;
            generatedRainbowColor_atLowerLuminanceAnchor.r = animCurve_lowerAnchor_red.Evaluate(progressThroughRainbow_as0to1);
            generatedRainbowColor_atLowerLuminanceAnchor.g = animCurve_lowerAnchor_green.Evaluate(progressThroughRainbow_as0to1);
            generatedRainbowColor_atLowerLuminanceAnchor.b = animCurve_lowerAnchor_blue.Evaluate(progressThroughRainbow_as0to1);
            generatedRainbowColor_atLowerLuminanceAnchor = SeededColorGenerator.ForceApproxLuminance(generatedRainbowColor_atLowerLuminanceAnchor, forceLuminance * animCurve_lowerAnchor_luminanceTargetValueModifier.Evaluate(progressThroughRainbow_as0to1));

            Color generatedRainbowColor_atHigherLuminanceAnchor = default;
            generatedRainbowColor_atHigherLuminanceAnchor.r = animCurve_higherAnchor_red.Evaluate(progressThroughRainbow_as0to1);
            generatedRainbowColor_atHigherLuminanceAnchor.g = animCurve_higherAnchor_green.Evaluate(progressThroughRainbow_as0to1);
            generatedRainbowColor_atHigherLuminanceAnchor.b = animCurve_higherAnchor_blue.Evaluate(progressThroughRainbow_as0to1);
            generatedRainbowColor_atHigherLuminanceAnchor = SeededColorGenerator.ForceApproxLuminance(generatedRainbowColor_atHigherLuminanceAnchor, forceLuminance * animCurve_higherAnchor_luminanceTargetValueModifier.Evaluate(progressThroughRainbow_as0to1));

            float luminanceSpan_fromLowerToHigherAnchor = luminanceMarkingHigherAnchor - luminanceMarkingLowerAnchor;
            float forceLuminance_portionOverLowerAnchor = forceLuminance - luminanceMarkingLowerAnchor;
            float progress0to1_fromLowerLuminanceAnchor_toHigherLuminanceAnchor = forceLuminance_portionOverLowerAnchor / luminanceSpan_fromLowerToHigherAnchor;
            return Color.Lerp(generatedRainbowColor_atLowerLuminanceAnchor, generatedRainbowColor_atHigherLuminanceAnchor, progress0to1_fromLowerLuminanceAnchor_toHigherLuminanceAnchor);
        }

    }

}
