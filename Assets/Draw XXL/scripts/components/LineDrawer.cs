namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Line Drawer")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class LineDrawer : VisualizerParent
    {
        public enum LineType { standardLine, vector, vectorWithExtention, blinkingLine, lineUnderTension, movingArrowsLine, lineWithAlternatingColors };
        [SerializeField] public LineType lineType = LineType.standardLine;

        public enum LineDefinitionMode { startPositionAndEndPosition, startPositionAndDirectionVectorToEndPosition, endPositionAndDirectionVectorToIt };
        [SerializeField] public LineDefinitionMode lineDefinitionMode = LineDefinitionMode.startPositionAndDirectionVectorToEndPosition;

        public enum PositionDefinitionOption { positionOfThisGameobjectPlusOffset, positionOfOtherGameobjectPlusOffset, chooseFree };
        [SerializeField] public PositionDefinitionOption positionDefinitionOption_ofStartPos = PositionDefinitionOption.positionOfThisGameobjectPlusOffset;
        [SerializeField] public PositionDefinitionOption positionDefinitionOption_ofEndPos = PositionDefinitionOption.chooseFree;
        [SerializeField] public bool lineDefinitionSection1_isOutfolded = false;
        [SerializeField] public bool lineDefinitionSection2_isOutfolded = false;

        public enum CoordinateSpaceForLocalOffset { useLocalSpaceDefinedByTransformOnThisGameobject, useLocalSpaceDefinedByTransformOnOtherGameobject };
        [SerializeField] public CoordinateSpaceForLocalOffset coordinateSpaceForLocalOffsetOnOtherGameobject_forStartPos = CoordinateSpaceForLocalOffset.useLocalSpaceDefinedByTransformOnThisGameobject;
        [SerializeField] public CoordinateSpaceForLocalOffset coordinateSpaceForLocalOffsetOnOtherGameobject_forEndPos = CoordinateSpaceForLocalOffset.useLocalSpaceDefinedByTransformOnThisGameobject;

        //Shared:
        [SerializeField] public Color startColor = DrawBasics.defaultColor;
        [SerializeField] public bool useDifferentEndColor = false;
        [SerializeField] public Color endColor = UtilitiesDXXL_Colors.red_boolFalse;
        [SerializeField] public float lineWidth = 0.0f;
        [SerializeField] public DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid;
        [SerializeField] public float stylePatternScaleFactor = 1.0f;
        [SerializeField] public bool animationDuringEditMode = true;
        [SerializeField] public float animationSpeed = 0.0f;
        public LineAnimationProgress precedingLineAnimationProgress = new LineAnimationProgress();
        [SerializeField] public bool enlargeSmallTextToThisMinTextSize = true;
        [SerializeField] public float enlargeSmallTextToThisMinTextSize_value = 0.005f;
        [SerializeField] public bool skipPatternEnlargementForLongLines = false;
        [SerializeField] public bool skipPatternEnlargementForShortLines = false;
        public enum AmplitudeAndTextAlignment { verticalInGlobalSpace, perpendicularToSceneViewCamera, perpendicularToGameViewCamera, customAmplitudeDirection };
        [SerializeField] AmplitudeAndTextAlignment amplitudeAndTextAlignment = AmplitudeAndTextAlignment.verticalInGlobalSpace;
        public enum EndPlatesConfig { disabled, bothSides, onlyAtStart, onlyAtEnd };
        [SerializeField] public EndPlatesConfig endPlatesConfig = EndPlatesConfig.disabled;
        [SerializeField] [Range(0.0f, 0.5f)] public float alphaFadeOutLength_0to1 = 0.0f;
        [SerializeField] bool flattenThickRoundLineIntoAmplitudePlane = false;
        [SerializeField] public bool shiftTextPosOnLines_toNonIntersecting = false;
        [SerializeField] [Range(0.05f, 10.0f)] public float relSizeOfTextOnLines = 0.45f;

        //Vectors:
        public enum ConesConfig { bothSides, onlyAtStart, onlyAtEnd };
        [SerializeField] public ConesConfig conesConfig = ConesConfig.onlyAtEnd;
        [SerializeField] public bool addNormalizedMarkingText = false;
        [SerializeField] public bool writeComponentValuesAsText = false;

        //Vectors With Extention:
        [SerializeField] public float extentionLength = 1000.0f;
        [SerializeField] public bool forceFixedConeLength = false;
        [SerializeField] public float forceFixedConeLength_value = 0.17f;

        //Blinking Line:
        [SerializeField] public Color blinkColor = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawBasics.defaultColor, 0.2f);
        [SerializeField] public float blinkDurationInSec = 0.5f;

        //Line under tension:
        [SerializeField] public float relaxedLength = 1.0f;
        [SerializeField] public float stretchFactor_forStretchedTensionColor = 2.0f;
        [SerializeField] public float stretchFactor_forSqueezedTensionColor = 0.0f;
        [SerializeField] public DrawBasics.LineStyle lineStyle_underTension = DrawBasics.LineStyle.spiral;
        [SerializeField] public Color relaxedColor = UtilitiesDXXL_Colors.green_boolTrue;
        [SerializeField] public Color color_forStretchedTension = UtilitiesDXXL_Colors.red_boolFalse;
        [SerializeField] public Color color_forSqueezedTension = UtilitiesDXXL_Colors.red_boolFalse;
        [SerializeField] [Range(0.0f, 1.0f)] public float alphaOfReferenceLengthDisplay = 0.15f;

        //Moving arrows line:
        [SerializeField] public float lineWidth_ofMovingArrowsLine = 0.05f;
        [SerializeField] bool flattenThickRoundLineIntoAmplitudePlane_ofMovingArrowsLine = true;
        [SerializeField] public float animationSpeed_ofMovingArrowsLine = 0.5f;
        [SerializeField] public float distanceBetweenArrows = 0.5f;
        [SerializeField] public float lengthOfArrows = 0.15f;
        [SerializeField] public bool backwardAnimationFlipsArrowDirection = true;

        //Line with alternating colors:
        [SerializeField] public Color alternatingColor = DrawBasics.defaultColor2_ofAlternatingColorLines;
        [SerializeField] public float lengthOfStripes = 0.04f;

        public override void InitializeValues_onceInComponentLifetime()
        {
            TrySetTextToEmptyString();

            customVector3_1_picker_isOutfolded = true;
            source_ofCustomVector3_1 = CustomVector3Source.transformsForward;
            customVector3_1_clipboardForManualInput = Vector3.forward;
            vectorInterpretation_ofCustomVector3_1 = VectorInterpretation.globalSpace;

            source_ofCustomVector3_2 = CustomVector3Source.manualInput;
            customVector3_2_clipboardForManualInput = Vector3.up;
            vectorInterpretation_ofCustomVector3_2 = VectorInterpretation.globalSpace;

            source_ofCustomVector3_3 = CustomVector3Source.manualInput;
            customVector3_3_clipboardForManualInput = Vector3.zero;
            vectorInterpretation_ofCustomVector3_3 = VectorInterpretation.globalSpace;

            source_ofCustomVector3_4 = CustomVector3Source.manualInput;
            customVector3_4_clipboardForManualInput = Vector3.forward;
            vectorInterpretation_ofCustomVector3_4 = VectorInterpretation.globalSpace;

            endPlates_size = 0.1f; //Despite this value: End plates display is initially disabled due to "endPlatesConfig"
        }

        public override void DrawVisualizedObject()
        {
            float used_enlargeSmallTextToThisMinTextSize_value = enlargeSmallTextToThisMinTextSize ? enlargeSmallTextToThisMinTextSize_value : 0.0f;
            float used_endPlates_size = Set_endPlatesConfig_reversible();
            Vector3 used_customAmplitudeAndTextDir = Set_amplitudeAndTextDirConfig_reversible();
            UtilitiesDXXL_DrawBasics.Set_shiftTextPosOnLines_toNonIntersecting_reversible(shiftTextPosOnLines_toNonIntersecting);
            UtilitiesDXXL_DrawBasics.Set_relSizeOfTextOnLines_reversible(relSizeOfTextOnLines);
            GetLineStartPosAndDirection(out Vector3 lineStartPosition, out Vector3 vector_fromLineStart_toLineEnd);

            switch (lineType)
            {
                case LineType.standardLine:
                    if (useDifferentEndColor)
                    {
                        precedingLineAnimationProgress = LineFrom_fadeableAnimSpeed.InternalDraw_withColorFade(lineStartPosition, vector_fromLineStart_toLineEnd, startColor, endColor, lineWidth, text_inclGlobalMarkupTags, lineStyle, stylePatternScaleFactor, animationSpeed, precedingLineAnimationProgress, used_customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, used_endPlates_size, alphaFadeOutLength_0to1, used_enlargeSmallTextToThisMinTextSize_value, 0.0f, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
                    }
                    else
                    {
                        precedingLineAnimationProgress = LineFrom_fadeableAnimSpeed.InternalDraw(lineStartPosition, vector_fromLineStart_toLineEnd, startColor, lineWidth, text_inclGlobalMarkupTags, lineStyle, stylePatternScaleFactor, animationSpeed, precedingLineAnimationProgress, used_customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, used_endPlates_size, alphaFadeOutLength_0to1, used_enlargeSmallTextToThisMinTextSize_value, 0.0f, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
                    }
                    break;
                case LineType.vector:
                    switch (conesConfig)
                    {
                        case ConesConfig.bothSides:
                            DrawBasics.VectorFrom(lineStartPosition, vector_fromLineStart_toLineEnd, startColor, lineWidth, text_inclGlobalMarkupTags, coneLength_forStraightVectors, true, flattenThickRoundLineIntoAmplitudePlane, used_customAmplitudeAndTextDir, addNormalizedMarkingText, used_enlargeSmallTextToThisMinTextSize_value, writeComponentValuesAsText, used_endPlates_size, 0.0f, hiddenByNearerObjects);
                            break;
                        case ConesConfig.onlyAtStart:
                            DrawBasics.VectorTo(-vector_fromLineStart_toLineEnd, lineStartPosition, startColor, lineWidth, text_inclGlobalMarkupTags, coneLength_forStraightVectors, false, flattenThickRoundLineIntoAmplitudePlane, used_customAmplitudeAndTextDir, addNormalizedMarkingText, used_enlargeSmallTextToThisMinTextSize_value, writeComponentValuesAsText, used_endPlates_size, 0.0f, hiddenByNearerObjects);
                            break;
                        case ConesConfig.onlyAtEnd:
                            DrawBasics.VectorFrom(lineStartPosition, vector_fromLineStart_toLineEnd, startColor, lineWidth, text_inclGlobalMarkupTags, coneLength_forStraightVectors, false, flattenThickRoundLineIntoAmplitudePlane, used_customAmplitudeAndTextDir, addNormalizedMarkingText, used_enlargeSmallTextToThisMinTextSize_value, writeComponentValuesAsText, used_endPlates_size, 0.0f, hiddenByNearerObjects);
                            break;
                        default:
                            break;
                    }
                    break;
                case LineType.vectorWithExtention:
                    float used_forceFixedConeLength_value = forceFixedConeLength ? forceFixedConeLength_value : 0.0f;
                    DrawEngineBasics.RayLineExtended(lineStartPosition, vector_fromLineStart_toLineEnd, startColor, lineWidth, text_inclGlobalMarkupTags, used_forceFixedConeLength_value, addNormalizedMarkingText, used_enlargeSmallTextToThisMinTextSize_value, extentionLength, 0.0f, hiddenByNearerObjects);
                    break;
                case LineType.blinkingLine:
                    float used_blinkDurationInSec = ((Application.isPlaying == false) && (animationDuringEditMode == false)) ? float.MaxValue : blinkDurationInSec; //-> this prevents a problem in the situation where "animationDuringEditMode" has been disabled in a blink phase where the line is not possible. Otherwise in such cases the line would permanently invisible.
                    DrawBasics.BlinkingRay(lineStartPosition, vector_fromLineStart_toLineEnd, startColor, used_blinkDurationInSec, lineWidth, text_inclGlobalMarkupTags, lineStyle, blinkColor, stylePatternScaleFactor, used_customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, used_endPlates_size, alphaFadeOutLength_0to1, used_enlargeSmallTextToThisMinTextSize_value, 0.0f, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
                    break;
                case LineType.lineUnderTension:
                    DrawBasics.RayUnderTension(lineStartPosition, vector_fromLineStart_toLineEnd, relaxedLength, relaxedColor, lineStyle_underTension, stretchFactor_forStretchedTensionColor, color_forStretchedTension, stretchFactor_forSqueezedTensionColor, color_forSqueezedTension, lineWidth, text_inclGlobalMarkupTags, alphaOfReferenceLengthDisplay, stylePatternScaleFactor, used_customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, used_endPlates_size, used_enlargeSmallTextToThisMinTextSize_value, 0.0f, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
                    break;
                case LineType.movingArrowsLine:
                    precedingLineAnimationProgress = MovingArrowsRay_fadeableAnimSpeed.InternalDraw(lineStartPosition, vector_fromLineStart_toLineEnd, startColor, lineWidth_ofMovingArrowsLine, distanceBetweenArrows, lengthOfArrows, text_inclGlobalMarkupTags, animationSpeed_ofMovingArrowsLine, precedingLineAnimationProgress, backwardAnimationFlipsArrowDirection, flattenThickRoundLineIntoAmplitudePlane_ofMovingArrowsLine, used_customAmplitudeAndTextDir, used_endPlates_size, used_enlargeSmallTextToThisMinTextSize_value, 0.0f, hiddenByNearerObjects);
                    break;
                case LineType.lineWithAlternatingColors:
                    precedingLineAnimationProgress = RayWithAlternatingColors_fadeableAnimSpeed.InternalDraw(lineStartPosition, vector_fromLineStart_toLineEnd, startColor, alternatingColor, lineWidth, lengthOfStripes, text_inclGlobalMarkupTags, animationSpeed, precedingLineAnimationProgress, used_customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, used_endPlates_size, alphaFadeOutLength_0to1, used_enlargeSmallTextToThisMinTextSize_value, 0.0f, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
                    break;
                default:
                    break;
            }

            UtilitiesDXXL_DrawBasics.Reverse_shiftTextPosOnLines_toNonIntersecting();
            UtilitiesDXXL_DrawBasics.Reverse_relSizeOfTextOnLines();
            Reverse_amplitudeAndTextDirConfig();
            Reverse_endPlatesConfig();

            TrySheduleRepaintSceneViewForAnimationOutsidePlaymode();
        }

        void GetLineStartPosAndDirection(out Vector3 lineStartPosition, out Vector3 vector_fromLineStart_toLineEnd)
        {
            Vector3 lineEndPosition;
            switch (lineDefinitionMode)
            {
                case LineDefinitionMode.startPositionAndEndPosition:
                    lineStartPosition = GetLineStartPosition_fromDefineStartPosSection();
                    lineEndPosition = GetLineEndPosition_fromDefineEndPosSection();
                    vector_fromLineStart_toLineEnd = lineEndPosition - lineStartPosition;
                    break;
                case LineDefinitionMode.startPositionAndDirectionVectorToEndPosition:
                    lineStartPosition = GetLineStartPosition_fromDefineStartPosSection();
                    vector_fromLineStart_toLineEnd = Get_customVector3_1_inGlobalSpaceUnits();
                    break;
                case LineDefinitionMode.endPositionAndDirectionVectorToIt:
                    lineEndPosition = GetLineEndPosition_fromDefineEndPosSection();
                    vector_fromLineStart_toLineEnd = Get_customVector3_1_inGlobalSpaceUnits();
                    lineStartPosition = lineEndPosition - vector_fromLineStart_toLineEnd;
                    break;
                default:
                    lineStartPosition = Vector3.zero;
                    vector_fromLineStart_toLineEnd = Vector3.forward;
                    break;
            }
        }

        Vector3 GetLineStartPosition_fromDefineStartPosSection()
        {
            switch (positionDefinitionOption_ofStartPos)
            {
                case PositionDefinitionOption.positionOfThisGameobjectPlusOffset:
                    return GetDrawPos3D_global();
                case PositionDefinitionOption.positionOfOtherGameobjectPlusOffset:
                    bool theSpaceForTransformingThePartnerLocalOffsetValue_isTakenFromThePartnerGameobject_notFromThisGameobject = (coordinateSpaceForLocalOffsetOnOtherGameobject_forStartPos == CoordinateSpaceForLocalOffset.useLocalSpaceDefinedByTransformOnOtherGameobject);
                    return GetDrawPos3D_ofPartnerGameobject_global(theSpaceForTransformingThePartnerLocalOffsetValue_isTakenFromThePartnerGameobject_notFromThisGameobject);
                case PositionDefinitionOption.chooseFree:
                    return Get_customVector3_3_inGlobalSpaceUnits();
                default:
                    return Vector3.zero;
            }
        }

        Vector3 GetLineEndPosition_fromDefineEndPosSection()
        {
            switch (positionDefinitionOption_ofEndPos)
            {
                case PositionDefinitionOption.positionOfThisGameobjectPlusOffset:
                    return GetDrawPos3D_global_independentAlternativeValue();
                case PositionDefinitionOption.positionOfOtherGameobjectPlusOffset:
                    bool theSpaceForTransformingThePartnerLocalOffsetValue_isTakenFromThePartnerGameobject_notFromThisGameobject = (coordinateSpaceForLocalOffsetOnOtherGameobject_forEndPos == CoordinateSpaceForLocalOffset.useLocalSpaceDefinedByTransformOnOtherGameobject);
                    return GetDrawPos3D_ofPartnerGameobject_global_independentAlternativeValue(theSpaceForTransformingThePartnerLocalOffsetValue_isTakenFromThePartnerGameobject_notFromThisGameobject);
                case PositionDefinitionOption.chooseFree:
                    return Get_customVector3_4_inGlobalSpaceUnits();
                default:
                    return Vector3.forward;
            }
        }

        Vector3 Set_amplitudeAndTextDirConfig_reversible()
        {
            switch (amplitudeAndTextAlignment)
            {
                case AmplitudeAndTextAlignment.verticalInGlobalSpace:
                    UtilitiesDXXL_DrawBasics.Set_automaticAmplitudeAndTextAlignment_reversible(DrawBasics.AutomaticAmplitudeAndTextAlignment.vertical);
                    return Vector3.zero;
                case AmplitudeAndTextAlignment.perpendicularToSceneViewCamera:
                    UtilitiesDXXL_DrawBasics.Set_automaticAmplitudeAndTextAlignment_reversible(DrawBasics.AutomaticAmplitudeAndTextAlignment.perpendicularToCamera);
                    UtilitiesDXXL_DrawBasics.Set_cameraForAutomaticOrientation_reversible(DrawBasics.CameraForAutomaticOrientation.sceneViewCamera);
                    return Vector3.zero;
                case AmplitudeAndTextAlignment.perpendicularToGameViewCamera:
                    UtilitiesDXXL_DrawBasics.Set_automaticAmplitudeAndTextAlignment_reversible(DrawBasics.AutomaticAmplitudeAndTextAlignment.perpendicularToCamera);
                    UtilitiesDXXL_DrawBasics.Set_cameraForAutomaticOrientation_reversible(DrawBasics.CameraForAutomaticOrientation.gameViewCamera);
                    return Vector3.zero;
                case AmplitudeAndTextAlignment.customAmplitudeDirection:
                    return Get_customVector3_2_inGlobalSpaceUnits();
                default:
                    return Vector3.zero;
            }
        }

        void Reverse_amplitudeAndTextDirConfig()
        {
            switch (amplitudeAndTextAlignment)
            {
                case AmplitudeAndTextAlignment.verticalInGlobalSpace:
                    UtilitiesDXXL_DrawBasics.Reverse_automaticAmplitudeAndTextAlignment();
                    break;
                case AmplitudeAndTextAlignment.perpendicularToSceneViewCamera:
                    UtilitiesDXXL_DrawBasics.Reverse_automaticAmplitudeAndTextAlignment();
                    UtilitiesDXXL_DrawBasics.Reverse_cameraForAutomaticOrientation();
                    break;
                case AmplitudeAndTextAlignment.perpendicularToGameViewCamera:
                    UtilitiesDXXL_DrawBasics.Reverse_automaticAmplitudeAndTextAlignment();
                    UtilitiesDXXL_DrawBasics.Reverse_cameraForAutomaticOrientation();
                    break;
                case AmplitudeAndTextAlignment.customAmplitudeDirection:
                    break;
                default:
                    break;
            }
        }

        public float Set_endPlatesConfig_reversible()
        {
            if (endPlatesConfig == EndPlatesConfig.disabled)
            {
                return 0.0f;
            }
            else
            {
                UtilitiesDXXL_DrawBasics.Set_endPlates_sizeInterpretation_reversible(endPlates_sizeInterpretation);
                switch (endPlatesConfig)
                {
                    case EndPlatesConfig.bothSides:
                        UtilitiesDXXL_DrawBasics.Set_disableEndPlates_atLineStart_reversible(false);
                        UtilitiesDXXL_DrawBasics.Set_disableEndPlates_atLineEnd_reversible(false);
                        break;
                    case EndPlatesConfig.onlyAtStart:
                        UtilitiesDXXL_DrawBasics.Set_disableEndPlates_atLineStart_reversible(false);
                        UtilitiesDXXL_DrawBasics.Set_disableEndPlates_atLineEnd_reversible(true);
                        break;
                    case EndPlatesConfig.onlyAtEnd:
                        UtilitiesDXXL_DrawBasics.Set_disableEndPlates_atLineStart_reversible(true);
                        UtilitiesDXXL_DrawBasics.Set_disableEndPlates_atLineEnd_reversible(false);
                        break;
                    default:
                        break;
                }
                return endPlates_size;
            }
        }

        public void Reverse_endPlatesConfig()
        {
            if (endPlatesConfig != EndPlatesConfig.disabled)
            {
                UtilitiesDXXL_DrawBasics.Reverse_endPlates_sizeInterpretation();
                UtilitiesDXXL_DrawBasics.Reverse_disableEndPlates_atLineStart();
                UtilitiesDXXL_DrawBasics.Reverse_disableEndPlates_atLineEnd();
            }
        }

        public void TrySheduleRepaintSceneViewForAnimationOutsidePlaymode()
        {
            if (animationDuringEditMode)
            {
                if (Application.isPlaying == false)
                {
                    if (DrawnLineUsesAnimation(true))
                    {
                        UtilitiesDXXL_Components.currentVirtualOnDrawGizmoCycle_shouldRepaintAllViews = true;
                    }
                }
            }
        }

        public bool DrawnLineUsesAnimation(bool returnFalseForAnimationSpeedOfZero)
        {
            if (lineType == LineType.standardLine)
            {
                if (UtilitiesDXXL_LineStyles.CheckIfLineStyleIsAnimatable(lineStyle))
                {
                    if (UtilitiesDXXL_Math.ApproximatelyZero(animationSpeed))
                    {
                        return (!returnFalseForAnimationSpeedOfZero);
                    }
                    else
                    {
                        return true;
                    }
                }
            }

            if (lineType == LineType.lineWithAlternatingColors)
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(animationSpeed))
                {
                    return (!returnFalseForAnimationSpeedOfZero);
                }
                else
                {
                    return true;
                }
            }

            if (lineType == LineType.movingArrowsLine)
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(animationSpeed_ofMovingArrowsLine))
                {
                    return (!returnFalseForAnimationSpeedOfZero);
                }
                else
                {
                    return true;
                }
            }

            if (lineType == LineType.blinkingLine)
            {
                return true;
            }

            return false;
        }

        public bool CheckIf_lineCanBeAffectedByFlattenBool()
        {
            if (lineType == LineType.vectorWithExtention)
            {
                return false; //never affected: doesn't have the flatten-option
            }
            else
            {
                if (lineType == LineType.vector)
                {
                    return true; //always affected due to "cones".
                }
                else
                {
                    if ((lineStyle == DrawBasics.LineStyle.arrows) && ((lineType == LineType.standardLine) || (lineType == LineType.blinkingLine)))
                    {
                        return true; //always affected due to "cones".
                    }
                    else
                    {
                        if ((lineStyle_underTension == DrawBasics.LineStyle.arrows) && (lineType == LineType.lineUnderTension))
                        {
                            return true; //always affected due to "cones".
                        }
                        else
                        {
                            if (lineType == LineType.movingArrowsLine)
                            {
                                return ((UtilitiesDXXL_Math.ApproximatelyZero(lineWidth_ofMovingArrowsLine) == false) || EndPlatesAreActive());
                            }
                            else
                            {
                                return ((UtilitiesDXXL_Math.ApproximatelyZero(lineWidth) == false) || EndPlatesAreActive());
                            }
                        }
                    }
                }
            }
        }

        bool EndPlatesAreActive()
        {
            return ((endPlatesConfig != EndPlatesConfig.disabled) && (UtilitiesDXXL_Math.ApproximatelyZero(endPlates_size) == false));
        }

        public virtual float GetLineLength()
        {
            GetLineStartPosAndDirection(out Vector3 lineStartPosition, out Vector3 vector_fromLineStart_toLineEnd);
            return vector_fromLineStart_toLineEnd.magnitude;
        }

    }

}
