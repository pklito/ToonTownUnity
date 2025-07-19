namespace DrawXXL
{
    using UnityEngine;

    public class SeededColorGenerator
    {
        public static Color GetRandomColorSeeded(int seed, float alphaOfGeneratedColor = 1.0f, float forceLuminance = 0.0f)
        {
            //the color variation decreases when using very high seeds
            return UtilitiesDXXL_Colors.Get_randomColorSeeded(seed, alphaOfGeneratedColor, forceLuminance);
        }

        public static Color ColorOfGameobjectID(GameObject colorDefiningGameobject, float forceLuminance = 0.0f)
        {
            //the color variation decreases when using very high seeds
            return GetRandomColorSeeded(colorDefiningGameobject.GetInstanceID(), 1.0f, forceLuminance);
        }

        public static void DrawCatalogueOfRandomColors(int lowestDrawnSeed, int highestDrawnSeed, float forceLuminance = 0.0f, Vector3 position = default(Vector3), float durationInSec = 0.0f)
        {
            //draws a list of the seeded colors together with their corresponding seed number. You can use this to show a list of available seeded colors where you can choose from by filling in the corresponding number into "Get_randomColorSeeded"
            //note that the color variation decreases when using very high seeds
            //the number of drawn colors is restricted to 1000 for performance reasons.

            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

            int maxDrawnColors = 1000;
            int numberOfDrawnColors = highestDrawnSeed - lowestDrawnSeed;

            if (numberOfDrawnColors > maxDrawnColors)
            {
                highestDrawnSeed = lowestDrawnSeed + maxDrawnColors;
                Debug.Log("DrawCatalogueOfRandomSeededColors() is restricted to " + maxDrawnColors + ". Drawing of color catalogue stopped after color " + highestDrawnSeed);
            }

            float textSize = 0.1f;
            for (int i = lowestDrawnSeed; i < highestDrawnSeed; i++)
            {
                Vector3 currPos = position + new Vector3(0.0f, -textSize * (i - lowestDrawnSeed), 0.0f);
                Color currColor = GetRandomColorSeeded(i, 1.0f, forceLuminance);
                UtilitiesDXXL_Text.Write("" + i, currPos, currColor, textSize, Vector3.right, Vector3.up, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, false, false, false, true);
            }
        }

        public static Color GetRainbowColor(int seed, float alphaOfGeneratedColor = 1.0f, int colorsPerSpectrumPass = 8, float forceLuminance = 0.0f)
        {
            //returns a seeded color while the color moves along a rainbow spectrum for subsequent seeds.
            //"colorsPerSpectrumPass" can be used adjust the colorDiffernce between each step... higher numbers mean less differnce from color to color. Minimum is 2.
            //higher values of "forceLuminance" lead to less color diversity...

            return UtilitiesDXXL_Colors.GetIteratingRainbowColor(seed, alphaOfGeneratedColor, colorsPerSpectrumPass, 0.0f, 1.0f, true, forceLuminance);
        }

        public static Color GetRainbowColorAroundRed(int seed, float alphaOfGeneratedColor = 1.0f, int colorsPerSpectrumPass = 5, bool sawToothTransition = false, float forceLuminance = 0.0f)
        {
            return UtilitiesDXXL_Colors.GetIteratingRainbowColor(seed, alphaOfGeneratedColor, colorsPerSpectrumPass, -0.14f, 0.1f, sawToothTransition, forceLuminance);
        }

        public static Color GetRainbowColorAroundGreen(int seed, float alphaOfGeneratedColor = 1.0f, int colorsPerSpectrumPass = 4, bool sawToothTransition = false, float forceLuminance = 0.0f)
        {
            return UtilitiesDXXL_Colors.GetIteratingRainbowColor(seed, alphaOfGeneratedColor, colorsPerSpectrumPass, 0.235f, 0.44f, sawToothTransition, forceLuminance);
        }

        public static Color GetRainbowColorAroundBlue(int seed, float alphaOfGeneratedColor = 1.0f, int colorsPerSpectrumPass = 5, bool sawToothTransition = false, float forceLuminance = 0.0f)
        {
            return UtilitiesDXXL_Colors.GetIteratingRainbowColor(seed, alphaOfGeneratedColor, colorsPerSpectrumPass, 0.535f, 0.72f, sawToothTransition, forceLuminance);
        }

        public static Color GetColorFromHueAndLuminance_tunedTransitionsSpectrum(float hue, float luminance = 0.5f, float alphaOfGeneratedColor = 1.0f)
        {
            // is roughly HSL space, with saturation = 1

            int colorsPerSpectrumPass = 1000000;
            int colorInsideSpectrum = (int)(hue * (float)colorsPerSpectrumPass);
            return UtilitiesDXXL_Colors.GetIteratingRainbowColor(colorInsideSpectrum, alphaOfGeneratedColor, colorsPerSpectrumPass, 0.0f, 1.0f, true, luminance);
        }

        static ChartDrawing chartDisplaying_tunedHLColorSpace;
        public static void DrawWholeHueLuminaceSpectrumOfTunedTransitionColorSpace(Vector3 position = default(Vector3), float width_ofDrawnSpectrum = 1.0f, float height_ofDrawnSpectrum = 1.0f, int drawnHueValues = 400, int drawnLuminanceValuesPerHueValue = 60, float durationInSec = 0.0f)
        {
            if (chartDisplaying_tunedHLColorSpace == null)
            {
                chartDisplaying_tunedHLColorSpace = new ChartDrawing("<size=10>Color space with<br>tuned transitions</size>");
            }

            chartDisplaying_tunedHLColorSpace.Position_worldspace = position;


            width_ofDrawnSpectrum = Mathf.Max(width_ofDrawnSpectrum, 0.01f);
            height_ofDrawnSpectrum = Mathf.Max(height_ofDrawnSpectrum, 0.01f);
            chartDisplaying_tunedHLColorSpace.Width_inWorldSpace = width_ofDrawnSpectrum;
            chartDisplaying_tunedHLColorSpace.Height_inWorldSpace = height_ofDrawnSpectrum;
            drawnHueValues = Mathf.Clamp(drawnHueValues, 3, 10000);
            drawnLuminanceValuesPerHueValue = Mathf.Clamp(drawnLuminanceValuesPerHueValue, 3, 10000);

            chartDisplaying_tunedHLColorSpace.xAxis.scaling = ChartAxis.Scaling.fixed_absolute;
            chartDisplaying_tunedHLColorSpace.xAxis.fixedLowerEndValueOfScale = -0.05f;
            chartDisplaying_tunedHLColorSpace.xAxis.fixedUpperEndValueOfScale = 1.05f;
            chartDisplaying_tunedHLColorSpace.xAxis.Name = "Hue";
            chartDisplaying_tunedHLColorSpace.yAxis.scaling = ChartAxis.Scaling.fixed_absolute;
            chartDisplaying_tunedHLColorSpace.yAxis.fixedLowerEndValueOfScale = -0.05f;
            chartDisplaying_tunedHLColorSpace.yAxis.fixedUpperEndValueOfScale = 1.05f;
            chartDisplaying_tunedHLColorSpace.yAxis.Name = "Luminance";

            chartDisplaying_tunedHLColorSpace.Draw(durationInSec, false);

            int numberOfDrawnHues = drawnHueValues;
            int numberOfDrawnLuminancesPerHue = drawnLuminanceValuesPerHueValue;
            float luminanceChangePerLine = 1.0f / (float)numberOfDrawnLuminancesPerHue;
            float luminanceAtCenterOfLowestLine = 0.5f * luminanceChangePerLine;
            Vector2 eachLine_inChartspace = Vector2.up * luminanceChangePerLine;
            for (int i_hue = 0; i_hue <= numberOfDrawnHues; i_hue++)
            {
                float hue_0to1 = (float)i_hue / (float)numberOfDrawnHues;
                for (int i_luminanceShiftedColorInsideHue = 0; i_luminanceShiftedColorInsideHue < numberOfDrawnLuminancesPerHue; i_luminanceShiftedColorInsideHue++)
                {
                    float luminance_0to1_atLineStart = luminanceChangePerLine * i_luminanceShiftedColorInsideHue;
                    float luminance_0to1_atLineCenter = luminance_0to1_atLineStart + luminanceAtCenterOfLowestLine;
                    Color currColor = GetColorFromHueAndLuminance_tunedTransitionsSpectrum(hue_0to1, luminance_0to1_atLineCenter);
                    Vector2 startPos_inChartspace = new Vector2(hue_0to1, luminance_0to1_atLineStart);
                    Vector3 start = chartDisplaying_tunedHLColorSpace.ChartSpace_to_WorldSpace(startPos_inChartspace);
                    Vector3 end = chartDisplaying_tunedHLColorSpace.ChartSpace_to_WorldSpace(startPos_inChartspace + eachLine_inChartspace);
                    Line_fadeableAnimSpeed.InternalDraw(start, end, currColor, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, false, false, false);
                }
            }
        }

        public static Color ForceApproxLuminance(Color colorToForce, float targetLuminance)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(targetLuminance))
            {
                //forcing disabled:
                return colorToForce;
            }
            else
            {
                float targetLuminance_0to2 = targetLuminance * 2.0f;
                float prevLuminance = GetLuminance(colorToForce);
                prevLuminance = Mathf.Max(prevLuminance, 0.0001f);
                float changeFactor = targetLuminance_0to2 / prevLuminance;

                Color forcedColor;
                Vector3 forcedColor_overshooting1 = (new Vector3(colorToForce.r, colorToForce.g, colorToForce.b) * changeFactor);
                float maxForcedComponent_overshooting1 = UtilitiesDXXL_Math.Max(forcedColor_overshooting1.x, forcedColor_overshooting1.y, forcedColor_overshooting1.z);
                if (maxForcedComponent_overshooting1 <= 1.0f)
                {
                    forcedColor = new Color(forcedColor_overshooting1.x, forcedColor_overshooting1.y, forcedColor_overshooting1.z, colorToForce.a);
                }
                else
                {
                    Vector3 colorToForce_liftedZeros = new Vector3(Mathf.Max(colorToForce.r, 0.0001f), Mathf.Max(colorToForce.g, 0.0001f), Mathf.Max(colorToForce.b, 0.0001f));
                    float xFactorForReachingForcedLevelButMax1 = Mathf.Min(1.0f, forcedColor_overshooting1.x) / colorToForce_liftedZeros.x;
                    float yFactorForReachingForcedLevelButMax1 = Mathf.Min(1.0f, forcedColor_overshooting1.y) / colorToForce_liftedZeros.y;
                    float zFactorForReachingForcedLevelButMax1 = Mathf.Min(1.0f, forcedColor_overshooting1.z) / colorToForce_liftedZeros.z;
                    Vector3 factorsForReachingForcedLevelButMax1 = new Vector3(xFactorForReachingForcedLevelButMax1, yFactorForReachingForcedLevelButMax1, zFactorForReachingForcedLevelButMax1);
                    Vector3 portionOver1_of_factors = factorsForReachingForcedLevelButMax1 - Vector3.one;
                    float portionOver1ofFactorForX_shrinked = portionOver1_of_factors.x * (forcedColor_overshooting1.x / maxForcedComponent_overshooting1);
                    float portionOver1ofFactorForY_shrinked = portionOver1_of_factors.y * (forcedColor_overshooting1.y / maxForcedComponent_overshooting1);
                    float portionOver1ofFactorForZ_shrinked = portionOver1_of_factors.z * (forcedColor_overshooting1.z / maxForcedComponent_overshooting1);
                    Vector3 portionOver1ofFactors_shrinked = new Vector3(portionOver1ofFactorForX_shrinked, portionOver1ofFactorForY_shrinked, portionOver1ofFactorForZ_shrinked);
                    Vector3 factorsForOriginalColor_thatShrinkButPreserveDifferences = Vector3.one + portionOver1ofFactors_shrinked;
                    Vector3 forcedColor_cappedToLuminanceOf1 = new Vector3(colorToForce.r * factorsForOriginalColor_thatShrinkButPreserveDifferences.x, colorToForce.g * factorsForOriginalColor_thatShrinkButPreserveDifferences.y, colorToForce.b * factorsForOriginalColor_thatShrinkButPreserveDifferences.z);
                    if (targetLuminance_0to2 <= 1.0f)
                    {
                        forcedColor = new Color(forcedColor_cappedToLuminanceOf1.x, forcedColor_cappedToLuminanceOf1.y, forcedColor_cappedToLuminanceOf1.z, colorToForce.a);
                    }
                    else
                    {
                        Vector3 spanTill1 = new Vector3(1.0f - forcedColor_cappedToLuminanceOf1.x, 1.0f - forcedColor_cappedToLuminanceOf1.y, 1.0f - forcedColor_cappedToLuminanceOf1.z);
                        float spanReduceFactor = 2.0f - targetLuminance_0to2;
                        spanReduceFactor = Mathf.Max(spanReduceFactor, 0.0f);
                        Vector3 spanTill1_reduced = spanTill1 * spanReduceFactor;
                        Vector3 forcedColorCapppedToLum1_thenSqueezedFurtherTo1 = new Vector3(1.0f - spanTill1_reduced.x, 1.0f - spanTill1_reduced.y, 1.0f - spanTill1_reduced.z);
                        forcedColor = new Color(forcedColorCapppedToLum1_thenSqueezedFurtherTo1.x, forcedColorCapppedToLum1_thenSqueezedFurtherTo1.y, forcedColorCapppedToLum1_thenSqueezedFurtherTo1.z, colorToForce.a);
                    }
                }
                return forcedColor;
            }
        }

        public static float GetLuminance(Color colorForWhichToGetTheLuminance)
        {
            return (0.22f * Mathf.Clamp01(colorForWhichToGetTheLuminance.r) + 0.68f * Mathf.Clamp01(colorForWhichToGetTheLuminance.g) + 0.1f * Mathf.Clamp01(colorForWhichToGetTheLuminance.b));
        }

    }

}
