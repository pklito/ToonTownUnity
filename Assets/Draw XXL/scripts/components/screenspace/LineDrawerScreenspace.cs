namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Screenspace/Line Drawer Screenspace")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class LineDrawerScreenspace : VisualizerScreenspaceParent
    {
        [SerializeField] LineDrawer.LineType lineType = LineDrawer.LineType.standardLine;
        [SerializeField] LineDrawer.LineDefinitionMode lineDefinitionMode = LineDrawer.LineDefinitionMode.startPositionAndEndPosition;
        [SerializeField] public bool lineDefinitionSection1_isOutfolded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool lineDefinitionSection2_isOutfolded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.

        //Shared:
        [SerializeField] bool interpretDirectionAsUnwarped = false;
        [SerializeField] Color startColor = DrawBasics.defaultColor;
        [SerializeField] bool useDifferentEndColor = false;
        [SerializeField] Color endColor = UtilitiesDXXL_Colors.red_boolFalse;
        [SerializeField] [Range(0.0f, 0.15f)] float lineWidth_relToViewportHeight = 0.0f;
        [SerializeField] DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid;
        [SerializeField] float stylePatternScaleFactor = 1.0f;
        [SerializeField] bool animationDuringEditMode = true;
        [SerializeField] float animationSpeed = 0.0f;
        LineAnimationProgress precedingLineAnimationProgress = new LineAnimationProgress();
        [SerializeField] bool enlargeSmallTextToThisMinTextSize = true;
        [SerializeField] [Range(0.0f, 0.3f)] float enlargeSmallTextToThisMinRelTextSize_value = DrawScreenspace.minTextSize_relToViewportHeight;
        [SerializeField] LineDrawer.EndPlatesConfig endPlatesConfig = LineDrawer.EndPlatesConfig.disabled;
        [SerializeField] [Range(0.0f, 0.5f)] float endPlatesSize_relToViewportHeight = 0.1f; //is initially disabled due to "endPlatesConfig"
        [SerializeField] [Range(0.0f, 0.5f)] float alphaFadeOutLength_0to1 = 0.0f;
        [SerializeField] bool shiftTextPosOnLines_toNonIntersecting = false;
        [SerializeField] [Range(0.05f, 10.0f)] float relSizeOfTextOnLines = 0.45f;

        //Vectors:
        [SerializeField] LineDrawer.ConesConfig conesConfig = LineDrawer.ConesConfig.onlyAtEnd;
        [SerializeField] [Range(0.0f, 0.3f)] float coneLength_relToViewportHeight = 0.05f;
        [SerializeField] bool writeComponentValuesAsText = false;

        //Vectors With Extention:
        [SerializeField] bool displayDistanceOutsideScreenBorder = true;

        //Blinking Line:
        [SerializeField] Color blinkColor = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawBasics.defaultColor, 0.2f);
        [SerializeField] float blinkDurationInSec = 0.5f;

        //Line under tension:
        [SerializeField] [Range(0.001f, 2.0f)] public float relaxedLength_relToViewportHeight = 0.4f;
        [SerializeField] public float stretchFactor_forStretchedTensionColor = 2.0f;
        [SerializeField] public float stretchFactor_forSqueezedTensionColor = 0.0f;
        [SerializeField] public DrawBasics.LineStyle lineStyle_underTension = DrawBasics.LineStyle.sine;
        [SerializeField] public Color relaxedColor = UtilitiesDXXL_Colors.green_boolTrue;
        [SerializeField] public Color color_forStretchedTension = UtilitiesDXXL_Colors.red_boolFalse;
        [SerializeField] public Color color_forSqueezedTension = UtilitiesDXXL_Colors.red_boolFalse;
        [SerializeField] [Range(0.0f, 1.0f)] public float alphaOfReferenceLengthDisplay = 0.1f;

        //Moving arrows line:
        [SerializeField] [Range(0.0f, 0.15f)] float lineWidth_ofMovingArrowsLine_relToViewportHeight = 0.016f;
        [SerializeField] float animationSpeed_ofMovingArrowsLine = 0.5f;
        [SerializeField] [Range(0.02f, 0.5f)] float distanceBetweenArrows_relToViewportHeight = 0.11f;
        [SerializeField] [Range(0.01f, 0.3f)] float lengthOfArrows_relToViewportHeight = 0.05f;
        [SerializeField] bool backwardAnimationFlipsArrowDirection = true;

        //Line with alternating colors:
        [SerializeField] Color alternatingColor = DrawBasics.defaultColor2_ofAlternatingColorLines;
        [SerializeField] [Range(0.001f, 0.2f)] float lengthOfStripes_relToViewportHeight = 0.03f;

        public override void InitializeValues_onceInComponentLifetime()
        {
            TrySetTextToEmptyString();

            customVector2_1_picker_isOutfolded = true;
            source_ofCustomVector2_1 = CustomVector2Source.manualInput;
            customVector2_1_clipboardForManualInput = 0.4f * Vector2.one;
            vectorInterpretation_ofCustomVector2_1 = VectorInterpretation.globalSpace;

            positionInsideViewport0to1 = new Vector2(0.3f, 0.3f);//-> initial line start position
            positionInsideViewport0to1_v2 = new Vector2(0.7f, 0.7f);//-> initial line end position
        }

        public override void InitializeValues_alsoOnPlaymodeEnter_andOnComponentCreatedAsCopy()
        {
            TryFetchCamOnThisGO_andDecideScreenspaceDefiningCamera();
        }

        public override void DrawVisualizedObject()
        {
            Camera usedCamera = Get_usedCamera("Shape Drawer Screenspace Component");
            if (usedCamera != null)
            {
                float used_enlargeSmallTextToThisMinRelTextSize_value = enlargeSmallTextToThisMinTextSize ? enlargeSmallTextToThisMinRelTextSize_value : 0.0f;
                float used_endPlatesSize_relToViewportHeight = Set_endPlatesConfig_reversible();
                UtilitiesDXXL_DrawBasics.Set_shiftTextPosOnLines_toNonIntersecting_reversible(shiftTextPosOnLines_toNonIntersecting);
                UtilitiesDXXL_DrawBasics.Set_relSizeOfTextOnLines_reversible(relSizeOfTextOnLines);
                GetLineStartPosAndDirection(out Vector2 lineStartPosition, out Vector2 vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace, usedCamera);
                bool used_interpretDirectionAsUnwarped = false; //-> "GetLineStartPosAndDirection()" already cares for "interpretDirectionAsUnwarped"

                switch (lineType)
                {
                    case LineDrawer.LineType.standardLine:
                        if (useDifferentEndColor)
                        {
                            precedingLineAnimationProgress = LineFrom_fadeableAnimSpeed_screenspace.InternalDraw_withColorFade(usedCamera, lineStartPosition, vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace, startColor, endColor, lineWidth_relToViewportHeight, text_inclGlobalMarkupTags, used_interpretDirectionAsUnwarped, lineStyle, stylePatternScaleFactor, animationSpeed, precedingLineAnimationProgress, used_endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, used_enlargeSmallTextToThisMinRelTextSize_value, 0.0f);
                        }
                        else
                        {
                            precedingLineAnimationProgress = LineFrom_fadeableAnimSpeed_screenspace.InternalDraw(usedCamera, lineStartPosition, vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace, startColor, lineWidth_relToViewportHeight, text_inclGlobalMarkupTags, used_interpretDirectionAsUnwarped, lineStyle, stylePatternScaleFactor, animationSpeed, precedingLineAnimationProgress, used_endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, used_enlargeSmallTextToThisMinRelTextSize_value, 0.0f);
                        }
                        break;
                    case LineDrawer.LineType.vector:
                        switch (conesConfig)
                        {
                            case LineDrawer.ConesConfig.bothSides:
                                DrawScreenspace.VectorFrom(usedCamera, lineStartPosition, vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace, startColor, lineWidth_relToViewportHeight, text_inclGlobalMarkupTags, used_interpretDirectionAsUnwarped, coneLength_relToViewportHeight, true, writeComponentValuesAsText, used_endPlatesSize_relToViewportHeight, 0.0f);
                                break;
                            case LineDrawer.ConesConfig.onlyAtStart:
                                DrawScreenspace.VectorTo(usedCamera, -vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace, lineStartPosition, startColor, lineWidth_relToViewportHeight, text_inclGlobalMarkupTags, used_interpretDirectionAsUnwarped, coneLength_relToViewportHeight, false, writeComponentValuesAsText, used_endPlatesSize_relToViewportHeight, 0.0f);
                                break;
                            case LineDrawer.ConesConfig.onlyAtEnd:
                                DrawScreenspace.VectorFrom(usedCamera, lineStartPosition, vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace, startColor, lineWidth_relToViewportHeight, text_inclGlobalMarkupTags, used_interpretDirectionAsUnwarped, coneLength_relToViewportHeight, false, writeComponentValuesAsText, used_endPlatesSize_relToViewportHeight, 0.0f);
                                break;
                            default:
                                break;
                        }
                        break;
                    case LineDrawer.LineType.vectorWithExtention:
                        DrawEngineBasics.RayLineExtendedScreenspace(usedCamera, lineStartPosition, vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace, startColor, lineWidth_relToViewportHeight, text_inclGlobalMarkupTags, used_interpretDirectionAsUnwarped, coneLength_relToViewportHeight, displayDistanceOutsideScreenBorder, 0.0f);
                        break;
                    case LineDrawer.LineType.blinkingLine:
                        float used_blinkDurationInSec = ((Application.isPlaying == false) && (animationDuringEditMode == false)) ? float.MaxValue : blinkDurationInSec; //-> this prevents a problem in the situation where "animationDuringEditMode" has been disabled in a blink phase where the line is not possible. Otherwise in such cases the line would permanently invisible.
                        DrawScreenspace.BlinkingRay(usedCamera, lineStartPosition, vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace, startColor, used_blinkDurationInSec, lineWidth_relToViewportHeight, text_inclGlobalMarkupTags, used_interpretDirectionAsUnwarped, lineStyle, blinkColor, stylePatternScaleFactor, used_endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, used_enlargeSmallTextToThisMinRelTextSize_value, 0.0f);
                        break;
                    case LineDrawer.LineType.lineUnderTension:
                        DrawScreenspace.RayUnderTension(usedCamera, lineStartPosition, vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace, relaxedLength_relToViewportHeight, relaxedColor, lineStyle_underTension, stretchFactor_forStretchedTensionColor, color_forStretchedTension, stretchFactor_forSqueezedTensionColor, color_forSqueezedTension, lineWidth_relToViewportHeight, text_inclGlobalMarkupTags, alphaOfReferenceLengthDisplay, used_interpretDirectionAsUnwarped, stylePatternScaleFactor, used_endPlatesSize_relToViewportHeight, used_enlargeSmallTextToThisMinRelTextSize_value, 0.0f);
                        break;
                    case LineDrawer.LineType.movingArrowsLine:
                        precedingLineAnimationProgress = MovingArrowsRay_fadeableAnimSpeed_screenspace.InternalDraw(usedCamera, lineStartPosition, vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace, startColor, lineWidth_ofMovingArrowsLine_relToViewportHeight, distanceBetweenArrows_relToViewportHeight, lengthOfArrows_relToViewportHeight, text_inclGlobalMarkupTags, animationSpeed_ofMovingArrowsLine, precedingLineAnimationProgress, backwardAnimationFlipsArrowDirection, used_interpretDirectionAsUnwarped, used_endPlatesSize_relToViewportHeight, 0.0f);
                        break;
                    case LineDrawer.LineType.lineWithAlternatingColors:
                        precedingLineAnimationProgress = RayWithAlternatingColors_fadeableAnimSpeed_screenspace.InternalDraw(usedCamera, lineStartPosition, vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace, startColor, alternatingColor, lineWidth_relToViewportHeight, lengthOfStripes_relToViewportHeight, text_inclGlobalMarkupTags, used_interpretDirectionAsUnwarped, animationSpeed, precedingLineAnimationProgress, used_endPlatesSize_relToViewportHeight, alphaFadeOutLength_0to1, 0.0f);
                        break;
                    default:
                        break;
                }

                UtilitiesDXXL_DrawBasics.Reverse_shiftTextPosOnLines_toNonIntersecting();
                UtilitiesDXXL_DrawBasics.Reverse_relSizeOfTextOnLines();
                Reverse_endPlatesConfig();

                TrySheduleRepaintSceneViewForAnimationOutsidePlaymode();
            }
        }

        void GetLineStartPosAndDirection(out Vector2 lineStartPosition, out Vector2 vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace, Camera usedCamera)
        {
            Vector2 lineEndPosition;
            switch (lineDefinitionMode)
            {
                case LineDrawer.LineDefinitionMode.startPositionAndEndPosition:
                    lineStartPosition = positionInsideViewport0to1;
                    lineEndPosition = positionInsideViewport0to1_v2;
                    vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace = lineEndPosition - lineStartPosition;
                    break;
                case LineDrawer.LineDefinitionMode.startPositionAndDirectionVectorToEndPosition:
                    lineStartPosition = positionInsideViewport0to1;
                    vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace = Get_vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace(usedCamera);
                    break;
                case LineDrawer.LineDefinitionMode.endPositionAndDirectionVectorToIt:
                    lineEndPosition = positionInsideViewport0to1_v2;
                    vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace = Get_vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace(usedCamera);
                    lineStartPosition = lineEndPosition - vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace;
                    break;
                default:
                    lineStartPosition = Vector2.zero;
                    vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace = Vector2.one;
                    break;
            }
        }

        Vector2 Get_vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace(Camera usedCamera)
        {
            if (interpretDirectionAsUnwarped || (source_ofCustomVector2_1 == CustomVector2Source.rotationAroundZStartingFromRight))
            {
                return DrawScreenspace.DirectionInUnitsOfUnwarpedSpace_to_sameLookingDirectionInUnitsOfWarpedSpace(Get_customVector2_1_inGlobalSpaceUnits(), usedCamera);
            }
            else
            {
                return Get_customVector2_1_inGlobalSpaceUnits();
            }
        }

        float Set_endPlatesConfig_reversible()
        {
            if (endPlatesConfig == LineDrawer.EndPlatesConfig.disabled)
            {
                return 0.0f;
            }
            else
            {
                switch (endPlatesConfig)
                {
                    case LineDrawer.EndPlatesConfig.bothSides:
                        UtilitiesDXXL_DrawBasics.Set_disableEndPlates_atLineStart_reversible(false);
                        UtilitiesDXXL_DrawBasics.Set_disableEndPlates_atLineEnd_reversible(false);
                        break;
                    case LineDrawer.EndPlatesConfig.onlyAtStart:
                        UtilitiesDXXL_DrawBasics.Set_disableEndPlates_atLineStart_reversible(false);
                        UtilitiesDXXL_DrawBasics.Set_disableEndPlates_atLineEnd_reversible(true);
                        break;
                    case LineDrawer.EndPlatesConfig.onlyAtEnd:
                        UtilitiesDXXL_DrawBasics.Set_disableEndPlates_atLineStart_reversible(true);
                        UtilitiesDXXL_DrawBasics.Set_disableEndPlates_atLineEnd_reversible(false);
                        break;
                    default:
                        break;
                }
                return endPlatesSize_relToViewportHeight;
            }
        }

        void Reverse_endPlatesConfig()
        {
            if (endPlatesConfig != LineDrawer.EndPlatesConfig.disabled)
            {
                UtilitiesDXXL_DrawBasics.Reverse_disableEndPlates_atLineStart();
                UtilitiesDXXL_DrawBasics.Reverse_disableEndPlates_atLineEnd();
            }
        }

        void TrySheduleRepaintSceneViewForAnimationOutsidePlaymode()
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
            if (lineType == LineDrawer.LineType.standardLine)
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

            if (lineType == LineDrawer.LineType.lineWithAlternatingColors)
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

            if (lineType == LineDrawer.LineType.movingArrowsLine)
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

            if (lineType == LineDrawer.LineType.blinkingLine)
            {
                return true;
            }

            return false;
        }

        public float GetLineLength_relToViewportHeight()
        {
            Camera usedCamera = Get_usedCamera("Shape Drawer Screenspace Component");
            if (usedCamera != null)
            {
                GetLineStartPosAndDirection(out Vector2 lineStartPosition, out Vector2 vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace, usedCamera);
                Vector2 vector_fromLineStart_toLineEnd_inUnwarpedScreenspace = DrawScreenspace.DirectionInUnitsOfWarpedSpace_to_sameLookingDirectionInUnitsOfUnwarpedSpace(vector_fromLineStart_toLineEnd_inAspectWarpedScreenspace, usedCamera);
                return vector_fromLineStart_toLineEnd_inUnwarpedScreenspace.magnitude;
            }
            else
            {
                return 1.0f;
            }
        }

    }

}
