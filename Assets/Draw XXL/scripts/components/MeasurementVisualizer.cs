namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Measurement Visualizer")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class MeasurementVisualizer : VisualizerParent
    {
        public enum MeasurementType { distanceBetweenPoints, distanceThresholdBetweenPoints, distanceFromPointToLine, distanceFromLineToLine, distanceFromPointToPlane, distanceAlongOrthographicViewDir, distancePerpendicularToOrthographicViewDir, angleBetweenVectors, angleFromLineToPlane, angleFromPlaneToPlane };
        [SerializeField] MeasurementType measurementType = MeasurementType.distanceBetweenPoints;

        public enum AngleUnit { degree, radians };
        [SerializeField] AngleUnit angleUnitToDisplay = AngleUnit.degree;

        [SerializeField] Color color1 = DrawMeasurements.defaultColor1;
        [SerializeField] Color color2 = DrawMeasurements.defaultColor2;
        [SerializeField] public bool appearanceBlock_isOutfolded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public float measuredResultValue; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] float linesWidth = 0.0f;
        [SerializeField] Color color = DrawBasics.defaultColor;
        [SerializeField] float enlargeSmallTextToThisMinTextSize = 0.005f;
        [SerializeField] bool addTextForAlternativeAngleUnit = true;
        [SerializeField] float forceRadius_value = 1.0f;
        [SerializeField] bool useReflexAngleOver180deg = false;
        [SerializeField] bool drawBoundaryLines = true;
        [SerializeField] bool returnObtuseAngleOver90deg = false;
        [SerializeField] string name_ofGeoObject1 = null;
        [SerializeField] string name_ofGeoObject2 = null;
        [SerializeField] float minimumLineLength_forDistancePointToLine = DrawMeasurements.minimumLineLength_forDistancePointToLine;
        [SerializeField] float minimumLineLength_forDistanceLineToLine = DrawMeasurements.minimumLineLength_forDistanceLineToLine;
        [SerializeField] float minimumLineLength_forAngleLineToPlane = DrawMeasurements.minimumLineLength_forAngleLineToPlane;

        public enum PointerConfigOfAngleBetweenVectors { atBothEnds, onlyAtStart, onlyAtEnd };
        [SerializeField] PointerConfigOfAngleBetweenVectors pointerConfigOfAngleBetweenVectors = PointerConfigOfAngleBetweenVectors.atBothEnds;

        //only for distanceThreshold:
        public enum DistanceThresholdType { one, two };
        [SerializeField] DistanceThresholdType distanceThresholdType = DistanceThresholdType.one;

        [SerializeField] float smallerThresholdDistance = 1.0f;
        [SerializeField] float biggerThresholdDistance = 2.0f;
        [SerializeField] bool displayDistanceAlsoAsText = false;

        public enum ExactlyOnThresholdBehaviour { countAsShorterThanThreshold, countAsLongerThanThreshold };
        [SerializeField] ExactlyOnThresholdBehaviour exactlyOnThresholdBehaviour = ExactlyOnThresholdBehaviour.countAsShorterThanThreshold;

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
            drawPosOffset3DSection_isOutfolded = true;
            drawPosOffset3DSection_ofPartnerGameobject_isOutfolded = true;

            customVector3_1_picker_isOutfolded = true;
            source_ofCustomVector3_1 = CustomVector3Source.transformsForward;
            customVector3_1_clipboardForManualInput = Vector3.one;
            vectorInterpretation_ofCustomVector3_1 = VectorInterpretation.globalSpace;

            customVector3_2_picker_isOutfolded = true;
            source_ofCustomVector3_2 = CustomVector3Source.transformsForward;
            customVector3_2_clipboardForManualInput = Vector3.forward;
            vectorInterpretation_ofCustomVector3_2 = VectorInterpretation.globalSpace;

            customVector3_3_picker_isOutfolded = true;
            source_ofCustomVector3_3 = CustomVector3Source.manualInput;
            customVector3_3_clipboardForManualInput = Vector3.one;
            vectorInterpretation_ofCustomVector3_3 = VectorInterpretation.globalSpace;

            customVector3_4_picker_isOutfolded = true;
            source_ofCustomVector3_4 = CustomVector3Source.transformsForward;
            customVector3_4_clipboardForManualInput = Vector3.one;
            vectorInterpretation_ofCustomVector3_4 = VectorInterpretation.globalSpace;

            customVector3ofPartnerGameobject_picker_isOutfolded = true;
            source_ofCustomVector3ofPartnerGameobject = CustomVector3Source.transformsForward;
            customVector3ofPartnerGameobject_clipboardForManualInput = Vector3.one;
            vectorInterpretation_ofCustomVector3ofPartnerGameobject = VectorInterpretation.globalSpace;
        }

        public override void DrawVisualizedObject()
        {
            UtilitiesDXXL_Measurements.Set_defaultColor1_reversible(color1);
            UtilitiesDXXL_Measurements.Set_defaultColor2_reversible(color2);
            switch (measurementType)
            {
                case MeasurementType.distanceBetweenPoints:
                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(coneLength_interpretation_forStraightVectors);
                    measuredResultValue = DrawMeasurements.Distance(GetDrawPos3D_global(), GetDrawPos3D_ofPartnerGameobject_global(), color, linesWidth, text_inclGlobalMarkupTags, coneLength_forStraightVectors, enlargeSmallTextToThisMinTextSize, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                    break;
                case MeasurementType.distanceThresholdBetweenPoints:
                    switch (distanceThresholdType)
                    {
                        case DistanceThresholdType.one:
                            UtilitiesDXXL_DrawBasics.Set_endPlates_sizeInterpretation_reversible(endPlates_sizeInterpretation);
                            DrawMeasurements.DistanceThreshold(GetDrawPos3D_global(), GetDrawPos3D_ofPartnerGameobject_global(), smallerThresholdDistance, text_inclGlobalMarkupTags, displayDistanceAlsoAsText, linesWidth, ExactlyThresholdLength_countsAsShorter(), endPlates_size, overwriteStyle_forNear, overwriteStyle_forFar, overwriteColor_forNear_oneThresholdVersion, overwriteColor_forFar_oneThresholdVersion, default(Vector3), enlargeSmallTextToThisMinTextSize, 0.0f, hiddenByNearerObjects);
                            UtilitiesDXXL_DrawBasics.Reverse_endPlates_sizeInterpretation();
                            break;
                        case DistanceThresholdType.two:
                            UtilitiesDXXL_DrawBasics.Set_endPlates_sizeInterpretation_reversible(endPlates_sizeInterpretation);
                            DrawMeasurements.DistanceThresholds(GetDrawPos3D_global(), GetDrawPos3D_ofPartnerGameobject_global(), smallerThresholdDistance, biggerThresholdDistance, text_inclGlobalMarkupTags, displayDistanceAlsoAsText, linesWidth, ExactlyThresholdLength_countsAsShorter(), endPlates_size, overwriteStyle_forNear, overwriteStyle_forMiddle, overwriteStyle_forFar, overwriteColor_forNear_twoThresholdsVersion, overwriteColor_forMiddle_twoThresholdsVersion, overwriteColor_forFar_twoThresholdsVersion, default(Vector3), enlargeSmallTextToThisMinTextSize, 0.0f, hiddenByNearerObjects);
                            UtilitiesDXXL_DrawBasics.Reverse_endPlates_sizeInterpretation();
                            break;
                        default:
                            break;
                    }
                    break;
                case MeasurementType.distanceFromPointToLine:
                    UtilitiesDXXL_Measurements.Set_minimumLineLength_forDistancePointToLine_reversible(minimumLineLength_forDistancePointToLine);
                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(coneLength_interpretation_forStraightVectors);
                    measuredResultValue = DrawMeasurements.DistancePointToLine(GetDrawPos3D_global(), GetDrawPos3D_ofPartnerGameobject_global(), Get_customVector3ofPartnerGameobject_inGlobalSpaceUnits(), color, linesWidth, text_inclGlobalMarkupTags, name_ofGeoObject2, coneLength_forStraightVectors, enlargeSmallTextToThisMinTextSize, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                    UtilitiesDXXL_Measurements.Reverse_minimumLineLength_forDistancePointToLine();
                    break;
                case MeasurementType.distanceFromLineToLine:
                    UtilitiesDXXL_Measurements.Set_minimumLineLength_forDistanceLineToLine_reversible(minimumLineLength_forDistanceLineToLine);
                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(coneLength_interpretation_forStraightVectors);
                    measuredResultValue = DrawMeasurements.DistanceLineToLine(GetDrawPos3D_global(), Get_customVector3_4_inGlobalSpaceUnits(), GetDrawPos3D_ofPartnerGameobject_global(), Get_customVector3ofPartnerGameobject_inGlobalSpaceUnits(), color, linesWidth, text_inclGlobalMarkupTags, name_ofGeoObject1, name_ofGeoObject2, coneLength_forStraightVectors, enlargeSmallTextToThisMinTextSize, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                    UtilitiesDXXL_Measurements.Reverse_minimumLineLength_forDistanceLineToLine();
                    break;
                case MeasurementType.distanceFromPointToPlane:
                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(coneLength_interpretation_forStraightVectors);
                    measuredResultValue = DrawMeasurements.DistancePointToPlane(GetDrawPos3D_global(), GetDrawPos3D_ofPartnerGameobject_global(), Get_customVector3ofPartnerGameobject_inGlobalSpaceUnits(), color, linesWidth, text_inclGlobalMarkupTags, name_ofGeoObject2, coneLength_forStraightVectors, enlargeSmallTextToThisMinTextSize, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                    break;
                case MeasurementType.distanceAlongOrthographicViewDir:
                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(coneLength_interpretation_forStraightVectors);
                    measuredResultValue = DrawMeasurements.DistanceAlongOrthoViewDir(GetDrawPos3D_global(), GetDrawPos3D_ofPartnerGameobject_global(), Get_customVector3_1_inGlobalSpaceUnits(), color, linesWidth, text_inclGlobalMarkupTags, coneLength_forStraightVectors, enlargeSmallTextToThisMinTextSize, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                    break;
                case MeasurementType.distancePerpendicularToOrthographicViewDir:
                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(coneLength_interpretation_forStraightVectors);
                    measuredResultValue = DrawMeasurements.DistancePerpToOrthoViewDir(GetDrawPos3D_global(), GetDrawPos3D_ofPartnerGameobject_global(), Get_customVector3_1_inGlobalSpaceUnits(), color, linesWidth, text_inclGlobalMarkupTags, coneLength_forStraightVectors, enlargeSmallTextToThisMinTextSize, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                    break;
                case MeasurementType.angleBetweenVectors:
                    switch (pointerConfigOfAngleBetweenVectors)
                    {
                        case PointerConfigOfAngleBetweenVectors.atBothEnds:
                            UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forCircledVectors_reversible(coneLength_interpretation_forCircledVectors);
                            measuredResultValue = DrawMeasurements.AngleSpan(Get_customVector3_2_inGlobalSpaceUnits(), Get_customVector3_3_inGlobalSpaceUnits(), GetDrawPos3D_global_independentAlternativeValue(), color, forceRadius_value, linesWidth, text_inclGlobalMarkupTags, useReflexAngleOver180deg, DisplayAndReturn_radInsteadOfDeg(), coneLength_forCircledVectors, drawBoundaryLines, addTextForAlternativeAngleUnit, 0.0f, hiddenByNearerObjects);
                            UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forCircledVectors();
                            break;
                        case PointerConfigOfAngleBetweenVectors.onlyAtStart:
                            UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forCircledVectors_reversible(coneLength_interpretation_forCircledVectors);
                            measuredResultValue = DrawMeasurements.Angle(Get_customVector3_3_inGlobalSpaceUnits(), Get_customVector3_2_inGlobalSpaceUnits(), GetDrawPos3D_global_independentAlternativeValue(), color, forceRadius_value, linesWidth, text_inclGlobalMarkupTags, useReflexAngleOver180deg, DisplayAndReturn_radInsteadOfDeg(), coneLength_forCircledVectors, drawBoundaryLines, addTextForAlternativeAngleUnit, 0.0f, hiddenByNearerObjects);
                            UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forCircledVectors();
                            break;
                        case PointerConfigOfAngleBetweenVectors.onlyAtEnd:
                            UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forCircledVectors_reversible(coneLength_interpretation_forCircledVectors);
                            measuredResultValue = DrawMeasurements.Angle(Get_customVector3_2_inGlobalSpaceUnits(), Get_customVector3_3_inGlobalSpaceUnits(), GetDrawPos3D_global_independentAlternativeValue(), color, forceRadius_value, linesWidth, text_inclGlobalMarkupTags, useReflexAngleOver180deg, DisplayAndReturn_radInsteadOfDeg(), coneLength_forCircledVectors, drawBoundaryLines, addTextForAlternativeAngleUnit, 0.0f, hiddenByNearerObjects);
                            UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forCircledVectors();
                            break;
                        default:
                            break;
                    }
                    break;
                case MeasurementType.angleFromLineToPlane:
                    UtilitiesDXXL_Measurements.Set_minimumLineLength_forAngleLineToPlane_reversible(minimumLineLength_forAngleLineToPlane);
                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forCircledVectors_reversible(coneLength_interpretation_forCircledVectors);
                    measuredResultValue = DrawMeasurements.AngleLineToPlane(GetDrawPos3D_global(), Get_customVector3_4_inGlobalSpaceUnits(), GetDrawPos3D_ofPartnerGameobject_global(), Get_customVector3ofPartnerGameobject_inGlobalSpaceUnits(), color, linesWidth, text_inclGlobalMarkupTags, name_ofGeoObject1, name_ofGeoObject2, returnObtuseAngleOver90deg, DisplayAndReturn_radInsteadOfDeg(), coneLength_forCircledVectors, addTextForAlternativeAngleUnit, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forCircledVectors();
                    UtilitiesDXXL_Measurements.Reverse_minimumLineLength_forAngleLineToPlane();
                    break;
                case MeasurementType.angleFromPlaneToPlane:
                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forCircledVectors_reversible(coneLength_interpretation_forCircledVectors);
                    measuredResultValue = DrawMeasurements.AnglePlaneToPlane(GetDrawPos3D_global(), Get_customVector3_4_inGlobalSpaceUnits(), GetDrawPos3D_ofPartnerGameobject_global(), Get_customVector3ofPartnerGameobject_inGlobalSpaceUnits(), color, linesWidth, text_inclGlobalMarkupTags, name_ofGeoObject1, name_ofGeoObject2, returnObtuseAngleOver90deg, DisplayAndReturn_radInsteadOfDeg(), coneLength_forCircledVectors, addTextForAlternativeAngleUnit, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forCircledVectors();
                    break;
                default:
                    break;
            }

            UtilitiesDXXL_Measurements.Reverse_defaultColor1();
            UtilitiesDXXL_Measurements.Reverse_defaultColor2();
        }

        bool DisplayAndReturn_radInsteadOfDeg()
        {
            return (angleUnitToDisplay == AngleUnit.radians);
        }

        bool ExactlyThresholdLength_countsAsShorter()
        {
            return (exactlyOnThresholdBehaviour == ExactlyOnThresholdBehaviour.countAsShorterThanThreshold);
        }

    }

}
