namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/2D/Measurement Visualizer 2D")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class MeasurementVisualizer2D : VisualizerParent
    {
        public enum MeasurementType { distanceBetweenPoints, distanceThresholdBetweenPoints, distanceFromPointToLine, angleBetweenVectors, angleFromLineToLine };
        [SerializeField] MeasurementType measurementType = MeasurementType.distanceBetweenPoints;

        [SerializeField] Color color1 = DrawMeasurements.defaultColor1;
        [SerializeField] Color color2 = DrawMeasurements.defaultColor2;
        [SerializeField] public bool appearanceBlock_isOutfolded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public float measuredResultValue; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] Color color = DrawBasics.defaultColor;
        [SerializeField] float linesWidth = 0.0f;
        [SerializeField] float enlargeSmallTextToThisMinTextSize = 0.005f;
        [SerializeField] bool addTextForAlternativeAngleUnit = true;
        [SerializeField] MeasurementVisualizer.AngleUnit angleUnitToDisplay = MeasurementVisualizer.AngleUnit.degree;
        [SerializeField] float forceRadius_value = 1.0f;
        [SerializeField] bool useReflexAngleOver180deg = false;
        [SerializeField] bool drawBoundaryLines = true;
        [SerializeField] bool returnObtuseAngleOver90deg = false;
        [SerializeField] string name_ofGeoObject1 = null;
        [SerializeField] string name_ofGeoObject2 = null;
        [SerializeField] MeasurementVisualizer.DistanceThresholdType distanceThresholdType = MeasurementVisualizer.DistanceThresholdType.one;
        [SerializeField] MeasurementVisualizer.PointerConfigOfAngleBetweenVectors pointerConfigOfAngleBetweenVectors = MeasurementVisualizer.PointerConfigOfAngleBetweenVectors.atBothEnds;
        [SerializeField] float minimumLineLength_forDistancePointToLine = DrawMeasurements2D.minimumLineLength_forDistancePointToLine;
        [SerializeField] float minimumLineLength_forAngleLineToLine = DrawMeasurements2D.minimumLineLength_forAngleLineToLine;

        //only for distanceThreshold:
        [SerializeField] float smallerThresholdDistance = 1.0f;
        [SerializeField] float biggerThresholdDistance = 2.0f;
        [SerializeField] bool displayDistanceAlsoAsText = false;
        [SerializeField] MeasurementVisualizer.ExactlyOnThresholdBehaviour exactlyOnThresholdBehaviour = MeasurementVisualizer.ExactlyOnThresholdBehaviour.countAsShorterThanThreshold;

        [SerializeField] DrawBasics.LineStyle overwriteStyle_forNear = DrawBasics.LineStyle.electricNoise;
        [SerializeField] DrawBasics.LineStyle overwriteStyle_forMiddle = DrawBasics.LineStyle.electricImpulses;
        [SerializeField] DrawBasics.LineStyle overwriteStyle_forFar = DrawBasics.LineStyle.solid;

        [SerializeField] Color overwriteColor_forNear_oneThresholdVersion = UtilitiesDXXL_Colors.red_boolFalse;
        [SerializeField] Color overwriteColor_forFar_oneThresholdVersion = UtilitiesDXXL_Colors.green_boolTrue;

        [SerializeField] Color overwriteColor_forNear_twoThresholdsVersion = UtilitiesDXXL_Colors.red_lineThresholdFarDistance;
        [SerializeField] Color overwriteColor_forMiddle_twoThresholdsVersion = UtilitiesDXXL_Colors.orange_lineThresholdMiddleDistance;
        [SerializeField] Color overwriteColor_forFar_twoThresholdsVersion = UtilitiesDXXL_Colors.green_lineThresholdNearDistance;

        public override void InitializeValues_onceInComponentLifetime()
        {
            TrySetTextToEmptyString();
            endPlates_size = 0.0f;
            coneLength_forStraightVectors = 0.10f;
            coneLength_forCircledVectors = 0.13f;
            drawPosOffset2DSection_isOutfolded = true;
            drawPosOffset2DSection_ofPartnerGameobject_isOutfolded = true;

            customVector2_1_picker_isOutfolded = true;
            source_ofCustomVector2_1 = CustomVector2Source.transformsRight;
            customVector2_1_clipboardForManualInput = Vector2.right;
            vectorInterpretation_ofCustomVector2_1 = VectorInterpretation.globalSpace;

            customVector2_2_picker_isOutfolded = true;
            source_ofCustomVector2_2 = CustomVector2Source.manualInput;
            customVector2_2_clipboardForManualInput = Vector2.one;
            vectorInterpretation_ofCustomVector2_2 = VectorInterpretation.globalSpace;

            customVector2_3_picker_isOutfolded = true;
            source_ofCustomVector2_3 = CustomVector2Source.transformsRight;
            customVector2_3_clipboardForManualInput = Vector2.one;
            vectorInterpretation_ofCustomVector2_3 = VectorInterpretation.globalSpace;

            customVector2ofPartnerGameobject_picker_isOutfolded = true;
            source_ofCustomVector2ofPartnerGameobject = CustomVector2Source.transformsRight;
            customVector2ofPartnerGameobject_clipboardForManualInput = Vector2.one;
            vectorInterpretation_ofCustomVector2ofPartnerGameobject = VectorInterpretation.globalSpace;
        }

        public override void DrawVisualizedObject()
        {
            UtilitiesDXXL_Measurements.Set_defaultColor1_reversible(color1);
            UtilitiesDXXL_Measurements.Set_defaultColor2_reversible(color2);
            switch (measurementType)
            {
                case MeasurementType.distanceBetweenPoints:
                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(coneLength_interpretation_forStraightVectors);
                    measuredResultValue = DrawMeasurements2D.Distance(GetDrawPos2D_global(), GetDrawPos2D_ofPartnerGameobject_global(), color, linesWidth, text_inclGlobalMarkupTags, GetZPos_global_for2D(), coneLength_forStraightVectors, enlargeSmallTextToThisMinTextSize, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                    break;
                case MeasurementType.distanceThresholdBetweenPoints:
                    switch (distanceThresholdType)
                    {
                        case MeasurementVisualizer.DistanceThresholdType.one:
                            UtilitiesDXXL_DrawBasics.Set_endPlates_sizeInterpretation_reversible(endPlates_sizeInterpretation);
                            DrawMeasurements2D.DistanceThreshold(GetDrawPos2D_global(), GetDrawPos2D_ofPartnerGameobject_global(), smallerThresholdDistance, text_inclGlobalMarkupTags, displayDistanceAlsoAsText, linesWidth, GetZPos_global_for2D(), ExactlyThresholdLength_countsAsShorter(), endPlates_size, overwriteStyle_forNear, overwriteStyle_forFar, overwriteColor_forNear_oneThresholdVersion, overwriteColor_forFar_oneThresholdVersion, enlargeSmallTextToThisMinTextSize, 0.0f, hiddenByNearerObjects);
                            UtilitiesDXXL_DrawBasics.Reverse_endPlates_sizeInterpretation();
                            break;
                        case MeasurementVisualizer.DistanceThresholdType.two:
                            UtilitiesDXXL_DrawBasics.Set_endPlates_sizeInterpretation_reversible(endPlates_sizeInterpretation);
                            DrawMeasurements2D.DistanceThresholds(GetDrawPos2D_global(), GetDrawPos2D_ofPartnerGameobject_global(), smallerThresholdDistance, biggerThresholdDistance, text_inclGlobalMarkupTags, displayDistanceAlsoAsText, linesWidth, GetZPos_global_for2D(), ExactlyThresholdLength_countsAsShorter(), endPlates_size, overwriteStyle_forNear, overwriteStyle_forMiddle, overwriteStyle_forFar, overwriteColor_forNear_twoThresholdsVersion, overwriteColor_forMiddle_twoThresholdsVersion, overwriteColor_forFar_twoThresholdsVersion, enlargeSmallTextToThisMinTextSize, 0.0f, hiddenByNearerObjects);
                            UtilitiesDXXL_DrawBasics.Reverse_endPlates_sizeInterpretation();
                            break;
                        default:
                            break;
                    }
                    break;
                case MeasurementType.distanceFromPointToLine:
                    UtilitiesDXXL_Measurements2D.Set_minimumLineLength_forDistancePointToLine_reversible(minimumLineLength_forDistancePointToLine);
                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(coneLength_interpretation_forStraightVectors);
                    measuredResultValue = DrawMeasurements2D.DistancePointToLine(GetDrawPos2D_global(), GetDrawPos2D_ofPartnerGameobject_global(), Get_customVector2ofPartnerGameobject_inGlobalSpaceUnits(), color, linesWidth, text_inclGlobalMarkupTags, name_ofGeoObject2, GetZPos_global_for2D(), coneLength_forStraightVectors, enlargeSmallTextToThisMinTextSize, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                    UtilitiesDXXL_Measurements2D.Reverse_minimumLineLength_forDistancePointToLine();
                    break;
                case MeasurementType.angleBetweenVectors:
                    switch (pointerConfigOfAngleBetweenVectors)
                    {
                        case MeasurementVisualizer.PointerConfigOfAngleBetweenVectors.atBothEnds:
                            UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forCircledVectors_reversible(coneLength_interpretation_forCircledVectors);
                            measuredResultValue = DrawMeasurements2D.AngleSpan(Get_customVector2_1_inGlobalSpaceUnits(), Get_customVector2_2_inGlobalSpaceUnits(), GetDrawPos2D_global_independentAlternativeValue(), color, forceRadius_value, linesWidth, text_inclGlobalMarkupTags, GetZPos_global_for2D(), useReflexAngleOver180deg, DisplayAndReturn_radInsteadOfDeg(), coneLength_forCircledVectors, drawBoundaryLines, addTextForAlternativeAngleUnit, 0.0f, hiddenByNearerObjects);
                            UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forCircledVectors();
                            break;
                        case MeasurementVisualizer.PointerConfigOfAngleBetweenVectors.onlyAtStart:
                            UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forCircledVectors_reversible(coneLength_interpretation_forCircledVectors);
                            measuredResultValue = DrawMeasurements2D.Angle(Get_customVector2_2_inGlobalSpaceUnits(), Get_customVector2_1_inGlobalSpaceUnits(), GetDrawPos2D_global_independentAlternativeValue(), color, forceRadius_value, linesWidth, text_inclGlobalMarkupTags, GetZPos_global_for2D(), useReflexAngleOver180deg, DisplayAndReturn_radInsteadOfDeg(), coneLength_forCircledVectors, drawBoundaryLines, addTextForAlternativeAngleUnit, 0.0f, hiddenByNearerObjects);
                            UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forCircledVectors();
                            break;
                        case MeasurementVisualizer.PointerConfigOfAngleBetweenVectors.onlyAtEnd:
                            UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forCircledVectors_reversible(coneLength_interpretation_forCircledVectors);
                            measuredResultValue = DrawMeasurements2D.Angle(Get_customVector2_1_inGlobalSpaceUnits(), Get_customVector2_2_inGlobalSpaceUnits(), GetDrawPos2D_global_independentAlternativeValue(), color, forceRadius_value, linesWidth, text_inclGlobalMarkupTags, GetZPos_global_for2D(), useReflexAngleOver180deg, DisplayAndReturn_radInsteadOfDeg(), coneLength_forCircledVectors, drawBoundaryLines, addTextForAlternativeAngleUnit, 0.0f, hiddenByNearerObjects);
                            UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forCircledVectors();
                            break;
                        default:
                            break;
                    }
                    break;
                case MeasurementType.angleFromLineToLine:
                    UtilitiesDXXL_Measurements2D.Set_minimumLineLength_forAngleLineToLine_reversible(minimumLineLength_forAngleLineToLine);
                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forCircledVectors_reversible(coneLength_interpretation_forCircledVectors);
                    measuredResultValue = DrawMeasurements2D.AngleLineToLine(GetDrawPos2D_global(), Get_customVector2_3_inGlobalSpaceUnits(), GetDrawPos2D_ofPartnerGameobject_global(), Get_customVector2ofPartnerGameobject_inGlobalSpaceUnits(), color, linesWidth, text_inclGlobalMarkupTags, name_ofGeoObject1, name_ofGeoObject2, GetZPos_global_for2D(), returnObtuseAngleOver90deg, DisplayAndReturn_radInsteadOfDeg(), coneLength_forCircledVectors, addTextForAlternativeAngleUnit, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forCircledVectors();
                    UtilitiesDXXL_Measurements2D.Reverse_minimumLineLength_forAngleLineToLine();
                    break;
                default:
                    break;
            }

            UtilitiesDXXL_Measurements.Reverse_defaultColor1();
            UtilitiesDXXL_Measurements.Reverse_defaultColor2();
        }

        bool DisplayAndReturn_radInsteadOfDeg()
        {
            return (angleUnitToDisplay == MeasurementVisualizer.AngleUnit.radians);
        }

        bool ExactlyThresholdLength_countsAsShorter()
        {
            return (exactlyOnThresholdBehaviour == MeasurementVisualizer.ExactlyOnThresholdBehaviour.countAsShorterThanThreshold);
        }

    }

}
