namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/2D/Line Drawer 2D")]
    public class LineDrawer2D : LineDrawer
    {
        public override void InitializeValues_onceInComponentLifetime()
        {
            TrySetTextToEmptyString();

            customVector2_1_picker_isOutfolded = true;
            source_ofCustomVector2_1 = CustomVector2Source.transformsRight;
            customVector2_1_clipboardForManualInput = Vector2.right;
            vectorInterpretation_ofCustomVector2_1 = VectorInterpretation.globalSpace;

            source_ofCustomVector2_3 = CustomVector2Source.manualInput;
            customVector2_3_clipboardForManualInput = Vector2.zero;
            vectorInterpretation_ofCustomVector2_3 = VectorInterpretation.globalSpace;

            source_ofCustomVector2_4 = CustomVector2Source.manualInput;
            customVector2_4_clipboardForManualInput = Vector2.right;
            vectorInterpretation_ofCustomVector2_4 = VectorInterpretation.globalSpace;

            endPlates_size = 0.1f; //is initially disabled due to "endPlatesConfig"
            lineStyle_underTension = DrawBasics.LineStyle.sine;
        }

        public override void DrawVisualizedObject()
        {
            float used_enlargeSmallTextToThisMinTextSize_value = enlargeSmallTextToThisMinTextSize ? enlargeSmallTextToThisMinTextSize_value : 0.0f;
            float used_endPlates_size = Set_endPlatesConfig_reversible();
            UtilitiesDXXL_DrawBasics.Set_shiftTextPosOnLines_toNonIntersecting_reversible(shiftTextPosOnLines_toNonIntersecting);
            UtilitiesDXXL_DrawBasics.Set_relSizeOfTextOnLines_reversible(relSizeOfTextOnLines);
            GetLineStartPosAndDirection(out Vector2 lineStartPosition, out Vector2 vector_fromLineStart_toLineEnd);

            switch (lineType)
            {
                case LineType.standardLine:
                    if (useDifferentEndColor)
                    {
                        precedingLineAnimationProgress = LineFrom_fadeableAnimSpeed_2D.InternalDraw_withColorFade(lineStartPosition, vector_fromLineStart_toLineEnd, startColor, endColor, lineWidth, text_inclGlobalMarkupTags, lineStyle, GetZPos_global_for2D(), stylePatternScaleFactor, animationSpeed, precedingLineAnimationProgress, used_endPlates_size, alphaFadeOutLength_0to1, used_enlargeSmallTextToThisMinTextSize_value, 0.0f, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
                    }
                    else
                    {
                        precedingLineAnimationProgress = LineFrom_fadeableAnimSpeed_2D.InternalDraw(lineStartPosition, vector_fromLineStart_toLineEnd, startColor, lineWidth, text_inclGlobalMarkupTags, lineStyle, GetZPos_global_for2D(), stylePatternScaleFactor, animationSpeed, precedingLineAnimationProgress, used_endPlates_size, alphaFadeOutLength_0to1, used_enlargeSmallTextToThisMinTextSize_value, 0.0f, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
                    }
                    break;
                case LineType.vector:
                    switch (conesConfig)
                    {
                        case ConesConfig.bothSides:
                            DrawBasics2D.VectorFrom(lineStartPosition, vector_fromLineStart_toLineEnd, startColor, lineWidth, text_inclGlobalMarkupTags, coneLength_forStraightVectors, true, GetZPos_global_for2D(), addNormalizedMarkingText, used_enlargeSmallTextToThisMinTextSize_value, writeComponentValuesAsText, used_endPlates_size, 0.0f, hiddenByNearerObjects);
                            break;
                        case ConesConfig.onlyAtStart:
                            DrawBasics2D.VectorTo(-vector_fromLineStart_toLineEnd, lineStartPosition, startColor, lineWidth, text_inclGlobalMarkupTags, coneLength_forStraightVectors, false, GetZPos_global_for2D(), addNormalizedMarkingText, used_enlargeSmallTextToThisMinTextSize_value, writeComponentValuesAsText, used_endPlates_size, 0.0f, hiddenByNearerObjects);
                            break;
                        case ConesConfig.onlyAtEnd:
                            DrawBasics2D.VectorFrom(lineStartPosition, vector_fromLineStart_toLineEnd, startColor, lineWidth, text_inclGlobalMarkupTags, coneLength_forStraightVectors, false, GetZPos_global_for2D(), addNormalizedMarkingText, used_enlargeSmallTextToThisMinTextSize_value, writeComponentValuesAsText, used_endPlates_size, 0.0f, hiddenByNearerObjects);
                            break;
                        default:
                            break;
                    }
                    break;
                case LineType.vectorWithExtention:
                    float used_forceFixedConeLength_value = forceFixedConeLength ? forceFixedConeLength_value : 0.0f;
                    DrawEngineBasics.RayLineExtended2D(lineStartPosition, vector_fromLineStart_toLineEnd, startColor, lineWidth, text_inclGlobalMarkupTags, GetZPos_global_for2D(), used_forceFixedConeLength_value, addNormalizedMarkingText, used_enlargeSmallTextToThisMinTextSize_value, extentionLength, 0.0f, hiddenByNearerObjects);
                    break;
                case LineType.blinkingLine:
                    float used_blinkDurationInSec = ((Application.isPlaying == false) && (animationDuringEditMode == false)) ? float.MaxValue : blinkDurationInSec; //-> this prevents a problem in the situation where "animationDuringEditMode" has been disabled in a blink phase where the line is not possible. Otherwise in such cases the line would permanently invisible.
                    DrawBasics2D.BlinkingRay(lineStartPosition, vector_fromLineStart_toLineEnd, startColor, used_blinkDurationInSec, lineWidth, text_inclGlobalMarkupTags, lineStyle, blinkColor, GetZPos_global_for2D(), stylePatternScaleFactor, used_endPlates_size, alphaFadeOutLength_0to1, used_enlargeSmallTextToThisMinTextSize_value, 0.0f, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
                    break;
                case LineType.lineUnderTension:
                    DrawBasics2D.RayUnderTension(lineStartPosition, vector_fromLineStart_toLineEnd, relaxedLength, relaxedColor, lineStyle_underTension, stretchFactor_forStretchedTensionColor, color_forStretchedTension, stretchFactor_forSqueezedTensionColor, color_forSqueezedTension, lineWidth, text_inclGlobalMarkupTags, alphaOfReferenceLengthDisplay, GetZPos_global_for2D(), stylePatternScaleFactor, used_endPlates_size, used_enlargeSmallTextToThisMinTextSize_value, 0.0f, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
                    break;
                case LineType.movingArrowsLine:
                    precedingLineAnimationProgress = MovingArrowsRay_fadeableAnimSpeed_2D.InternalDraw(lineStartPosition, vector_fromLineStart_toLineEnd, startColor, lineWidth_ofMovingArrowsLine, distanceBetweenArrows, lengthOfArrows, text_inclGlobalMarkupTags, GetZPos_global_for2D(), animationSpeed_ofMovingArrowsLine, precedingLineAnimationProgress, backwardAnimationFlipsArrowDirection, used_endPlates_size, used_enlargeSmallTextToThisMinTextSize_value, 0.0f, hiddenByNearerObjects);
                    break;
                case LineType.lineWithAlternatingColors:
                    precedingLineAnimationProgress = RayWithAlternatingColors_fadeableAnimSpeed_2D.InternalDraw(lineStartPosition, vector_fromLineStart_toLineEnd, startColor, alternatingColor, lineWidth, lengthOfStripes, text_inclGlobalMarkupTags, GetZPos_global_for2D(), animationSpeed, precedingLineAnimationProgress, used_endPlates_size, alphaFadeOutLength_0to1, used_enlargeSmallTextToThisMinTextSize_value, 0.0f, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
                    break;
                default:
                    break;
            }

            UtilitiesDXXL_DrawBasics.Reverse_shiftTextPosOnLines_toNonIntersecting();
            UtilitiesDXXL_DrawBasics.Reverse_relSizeOfTextOnLines();
            Reverse_endPlatesConfig();

            TrySheduleRepaintSceneViewForAnimationOutsidePlaymode();
        }

        void GetLineStartPosAndDirection(out Vector2 lineStartPosition, out Vector2 vector_fromLineStart_toLineEnd)
        {
            Vector2 lineEndPosition;
            switch (lineDefinitionMode)
            {
                case LineDefinitionMode.startPositionAndEndPosition:
                    lineStartPosition = GetLineStartPosition_fromDefineStartPosSection();
                    lineEndPosition = GetLineEndPosition_fromDefineEndPosSection();
                    vector_fromLineStart_toLineEnd = lineEndPosition - lineStartPosition;
                    break;
                case LineDefinitionMode.startPositionAndDirectionVectorToEndPosition:
                    lineStartPosition = GetLineStartPosition_fromDefineStartPosSection();
                    vector_fromLineStart_toLineEnd = Get_customVector2_1_inGlobalSpaceUnits();
                    break;
                case LineDefinitionMode.endPositionAndDirectionVectorToIt:
                    lineEndPosition = GetLineEndPosition_fromDefineEndPosSection();
                    vector_fromLineStart_toLineEnd = Get_customVector2_1_inGlobalSpaceUnits();
                    lineStartPosition = lineEndPosition - vector_fromLineStart_toLineEnd;
                    break;
                default:
                    lineStartPosition = Vector2.zero;
                    vector_fromLineStart_toLineEnd = Vector2.right;
                    break;
            }
        }

        Vector2 GetLineStartPosition_fromDefineStartPosSection()
        {
            switch (positionDefinitionOption_ofStartPos)
            {
                case PositionDefinitionOption.positionOfThisGameobjectPlusOffset:
                    return GetDrawPos2D_global();
                case PositionDefinitionOption.positionOfOtherGameobjectPlusOffset:
                    bool theSpaceForTransformingThePartnerLocalOffsetValue_isTakenFromThePartnerGameobject_notFromThisGameobject = (coordinateSpaceForLocalOffsetOnOtherGameobject_forStartPos == CoordinateSpaceForLocalOffset.useLocalSpaceDefinedByTransformOnOtherGameobject);
                    return GetDrawPos2D_ofPartnerGameobject_global(theSpaceForTransformingThePartnerLocalOffsetValue_isTakenFromThePartnerGameobject_notFromThisGameobject);
                case PositionDefinitionOption.chooseFree:
                    return Get_customVector2_3_inGlobalSpaceUnits();
                default:
                    return Vector2.zero;
            }
        }

        Vector2 GetLineEndPosition_fromDefineEndPosSection()
        {
            switch (positionDefinitionOption_ofEndPos)
            {
                case PositionDefinitionOption.positionOfThisGameobjectPlusOffset:
                    return GetDrawPos2D_global_independentAlternativeValue();
                case PositionDefinitionOption.positionOfOtherGameobjectPlusOffset:
                    bool theSpaceForTransformingThePartnerLocalOffsetValue_isTakenFromThePartnerGameobject_notFromThisGameobject = (coordinateSpaceForLocalOffsetOnOtherGameobject_forEndPos == CoordinateSpaceForLocalOffset.useLocalSpaceDefinedByTransformOnOtherGameobject);
                    return GetDrawPos2D_ofPartnerGameobject_global_independentAlternativeValue(theSpaceForTransformingThePartnerLocalOffsetValue_isTakenFromThePartnerGameobject_notFromThisGameobject);
                case PositionDefinitionOption.chooseFree:
                    return Get_customVector2_4_inGlobalSpaceUnits();
                default:
                    return Vector2.one;
            }
        }

        public override float GetLineLength()
        {
            GetLineStartPosAndDirection(out Vector2 lineStartPosition, out Vector2 vector_fromLineStart_toLineEnd);
            return vector_fromLineStart_toLineEnd.magnitude;
        }

    }

}
