namespace DrawXXL
{
    using System.Collections.Generic;
    using UnityEngine;

    public class DrawBasics
    {
        public enum AutomaticAmplitudeAndTextAlignment
        {
            vertical,
            perpendicularToCamera
        };
        public static AutomaticAmplitudeAndTextAlignment automaticAmplitudeAndTextAlignment = AutomaticAmplitudeAndTextAlignment.vertical;

        public enum AutomaticTextDirectionOfLines
        {
            towardsLineEnd,
            leftToRightInScreen
        };
        public static AutomaticTextDirectionOfLines automaticTextDirectionOfLines = AutomaticTextDirectionOfLines.leftToRightInScreen;

        public enum CameraForAutomaticOrientation
        {
            sceneViewCamera,
            gameViewCamera
        };
        public static CameraForAutomaticOrientation cameraForAutomaticOrientation = CameraForAutomaticOrientation.sceneViewCamera;

        public enum LineStyle { solid, invisible, dotted, dottedDense, dottedWide, dashed, dashedLong, dotDash, dotDashLong, twoDash, disconnectedAnchors, spiral, sine, zigzag, rhombus, doubleRhombus, electricNoise, electricImpulses, freeHand2D, freeHand3D, arrows, alternatingColorStripes };

        public enum BezierPosInterpretation
        {
            start_control1_control2_endIsNextStart, 
            start_control1_endIsNextStart,  
            onlySegmentStartPoints_backwardForwardIsAligned, 
            onlySegmentStartPoints_backwardForwardIsMirrored,  
            onlySegmentStartPoints_backwardForwardIsKinked  
        };

        private static int maxAllowedDrawnLinesPerFrame = 60000;
        public static int MaxAllowedDrawnLinesPerFrame
        {
            get { return maxAllowedDrawnLinesPerFrame; }
            set
            {
                if (value < UtilitiesDXXL_DrawBasics.maxMaxAllowedDrawnLinesPerFrame)
                {
                    maxAllowedDrawnLinesPerFrame = value;
                }
                else
                {
                    Debug.LogError("The upper threshold of 'MaxAllowedDrawnLinesPerFrame' is currently " + UtilitiesDXXL_DrawBasics.maxMaxAllowedDrawnLinesPerFrame + ". You tried to set it to " + value + ". Was that intentional?");
                }
            }
        }

        public enum UsedUnityLineDrawingMethod
        {
            debugLinesInPlayMode_gizmoLinesInEditModeAndPlaymodePauses,
            debugLines,
            gizmoLines,
            handlesLines,
            wireMesh,
            disabled
        };
        public static UsedUnityLineDrawingMethod usedUnityLineDrawingMethod = UsedUnityLineDrawingMethod.debugLines;

        public enum LengthInterpretation { relativeToLineLength, absoluteUnits };
        public static LengthInterpretation endPlates_sizeInterpretation = LengthInterpretation.relativeToLineLength;
        public static LengthInterpretation coneLength_interpretation_forStraightVectors = LengthInterpretation.relativeToLineLength; 
        public static LengthInterpretation coneLength_interpretation_forCircledVectors = LengthInterpretation.relativeToLineLength;
        public static bool disableEndPlates_atLineStart = false;  
        public static bool disableEndPlates_atLineEnd = false; 

        public enum MaxLinesExceededNotificationOnScreenType { ExplanationText, WarningSymbol, None };
        public static MaxLinesExceededNotificationOnScreenType maxLinesExceededNotificationOnScreenType = MaxLinesExceededNotificationOnScreenType.ExplanationText; //if you have problems with too many lines per frame and want to raise the "maxDrawnLinesPerFrame_preventingProgramFreeze"-value, then you can set this to "true". This replaces the warning text on screen which informs you of the exceeded limit with a simple warning-symbol. This saves around 7500 lines that would be used to draw the warning text which you then can use for your own draw operations.
        public enum MaxLinesExceededNotificationInLogConsoleType { Error, Warning, Log, None };
        public static MaxLinesExceededNotificationInLogConsoleType maxLinesExceededNotificationInLogConsoleType = MaxLinesExceededNotificationInLogConsoleType.Error; //if you want to keep your console clean from warnings that state that you have exceeded the maxLinesPerFrameL-Limit. see also "logType_forExceededMaxLinesConsoleMessage"


        private static float lineLength_aboveWhichToAutoEnlargeThePattern = 20.0f; //this prevents high computational effort for long lines that have a very fine grained pattern, which would result in drawing many single lines to compose the long patterned line. 
        //static float lineLengthSqr_aboveWhichToAutoEnlargeThePattern = lineLength_aboveWhichToAutoEnlargeThePattern * lineLength_aboveWhichToAutoEnlargeThePattern; //not precalculated, so that "lineLength_aboveWhichToAutoEnlargeThePattern" can be changed during runtime
        public static float LineLength_aboveWhichToAutoEnlargeThePattern
        {
            get { return lineLength_aboveWhichToAutoEnlargeThePattern; }
            set
            {
                lineLength_aboveWhichToAutoEnlargeThePattern = Mathf.Max(value, 0.1f);
            }
        }

        public static bool autoEnlargeBigPatternsLater_whichDistortsPatternSizeRatios = false;  
        public static Color defaultColor = Color.white;  
        public static Color defaultColor2_ofAlternatingColorLines = UtilitiesDXXL_Colors.red_boolFalse;
        public static float thinestPossibleNonZeroWidthLine = 0.00005f;

        private static float density_ofThickLines = 100.0f;
        public static float Density_ofThickLines
        {
            get { return density_ofThickLines; }
            set
            {
                density_ofThickLines = value;
                UtilitiesDXXL_DrawBasics.lowThreshold_ofLineWidth_forNumberOfThinLinesThatComposeTheThickLine = 0.6f / value;
            }
        }

        private static float stylePatternScaleFactor_alongLineDir_ignoringAmplitude = 1.0f;
        public static float StylePatternScaleFactor_alongLineDir_ignoringAmplitude //Many line drawing function have a parameter called "stylePatternScaleFactor" (e.g. DrawBasics.Line), which can be used to scale line styles (link zu lineStyle-global enum)), so that they remain well recognizable even for far view distances. It keeps the general shape of the pattern, but only scales its size. Additionally to these "stylePatternScaleFactor" parameters there is the global setting "StylePatternScaleFactor_alongLineDir_ignoringAmplitude". This also scales the line styles, but only along the line direction, while the amplitude remains the same. It can be used e.g. to change the winding density of a spiral line. Both patternScaleFactors can also be used in conjunction.
        {
            get { return stylePatternScaleFactor_alongLineDir_ignoringAmplitude; }
            set
            {
                if (UtilitiesDXXL_Math.FloatIsValid(value))
                {
                    stylePatternScaleFactor_alongLineDir_ignoringAmplitude = Mathf.Max(value, 0.01f);
                }
                else
                {
                    Debug.LogError("Cannot set 'StylePatternScaleFactor_alongLineDir_ignoringAmplitude' to the invalid value of " + value);
                }
            }
        }


        private static Vector3 default_textOffsetDirection_forPointTags = UtilitiesDXXL_DrawBasics.default_default_textOffsetDirection_forPointTags;
        public static Vector3 Default_textOffsetDirection_forPointTags //This is used by "DrawBasics.PointTag()" and "DrawBasics2D.PointTag()", if the "textOffsetDirection" parameter is not specified. It works in conjunction with "DrawText.automaticTextOrientation". That means: The z component should be 0 in most cases, which results in effectively a 2D vector inside the xy-plane. This 2D vector is then automatically rotated to fit the desired workflow as specified by "DrawText.automaticTextOrientation", e.g. rotated to screenspace if "DrawText.automaticTextOrientation" is at its default setting of of "screen". A z value would change the vector direction perpendicular to the 2D plane which "DrawText.automaticTextOrientation" specifies. "DrawBasics2D.PointTag()" ignores the z component in any case. The length of the vector is ignored. Only the direction has effect.
        {
            get { return default_textOffsetDirection_forPointTags; }
            set
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(value))
                {
                    Debug.LogError("Cannot set 'default_textOffsetDirection_forPointTags' to zero.");
                }
                else
                {
                    default_textOffsetDirection_forPointTags = value;
                }
            }
        }

        public static bool drawerComponentsAutomaticallyProceedOneFrameStepOnPauseStarts_toPreventFrozenOverlayDraw = false; //With this setting you can change the behaviour of drawing components, that seem to draw twice when pausing a game. For example if you use a Text Drawer Component and write "some test text", then pause the game and change the text to "another test text", then both is displayed ("some test text" and "another test text") seemingly as overlay. The reason for this is that Draw XXL components use Debug.DrawLine() for drawing when the game runs and Gizmo.DrawLine() when the game pauses. Otherwise the drawing could not be changed during pauses. Though lines from Debug.DrawLine() don't get cleared during pauses, instead they remain fixed until the pause ends. With this setting it is possible to prevent that overlay, because the drawer components(link) automatically proceed one frame when the game is paused, in which they skip the drawing with Debug.DrawLine(), so that it is not displayed during the game pause. <br><br> // If this is enabled and you have a Draw XXL drawer component in your Scene, then everytime the game pauses it will automatically perform an additional "proceed one frame step", as it can also be done by the right one of the three play/pause-buttons on the upper end inside the Unity Editor. For cases where you want to pause the game at a specific defined frame (via Debug.Break() somewhere in your code) and this is enabled, then your game pause actually ends up one frame later. If you want to end up in the frame where you called your Debug.Break() then you should leave this setting at its default disabled state. // see also "DrawCharts.chartInspectorComponentsAutomaticallyProceedOneFrameStepOnPauseStarts_toPreventFrozenOverlayDraw" which does the same for Chart Inspector components.
        public static int strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM = 0; //This can be set before drawing point visualizations via "Point","PointLocal","PointArray" or "PointList" (etc.), but not the equivalents in "DrawScreenspace"). The coordiante values that are drawn as text alongside these point visualization will use this value for their stroke width, given that the "drawCoordsAsText" parameter is active. It can be useful in busy environments with lots of lines that intersect and protrude each other so that the text stays distinct and readable. For example "DrawEngineBasics.Grid*()" uses it. "inPPM" means that the stroke width is measured in parts per million of the text size, the same way as "DrawText.MarkupStrokeWidth()" and "richTextMarkup-strokeWidth" expect it.
        public static bool initial_drawOnlyIfSelected_forComponents = false; //Most of the drawer and visualizer components have a checkbox saying "Draw only if selected", which, by is intially disabled. Depending on your use case it may be better if it is initially enabled, for example if you add GridVisualizers to many Gameobjects, but want to see the grid only around the currently selected Gameobject. You can change the initial value here with this setting.

        private static float relSizeOfTextOnLines = 0.45f;
        public static float RelSizeOfTextOnLines
        {
            get { return relSizeOfTextOnLines; }
            set { relSizeOfTextOnLines = Mathf.Clamp(value, 0.01f, 1000.0f); }
        }

        public static bool shiftTextPosOnLines_toNonIntersecting = false;

        public static float GlobalAlphaFactor
        {
            get { return DXXLWrapperForUntiysBuildInDrawLines.globalAlphaFactor; }
            set
            {
                DXXLWrapperForUntiysBuildInDrawLines.globalAlphaFactor_is0 = (value <= 0.0f);
                DXXLWrapperForUntiysBuildInDrawLines.globalAlphaFactor_is1 = Mathf.Approximately(value, 1.0f);
                DXXLWrapperForUntiysBuildInDrawLines.globalAlphaFactor = value;
            }
        }

        public enum IconType
        {
            //system/operate:
            dataDisc,
            saveData,
            loadData,
            folder,
            saveToFolder,
            loadFromFolder,
            share,
            trashcan,
            optionsSettingsGear,
            adjustOptionsSettings,
            homeHouse,
            profileFoto,
            imageLandscape,
            cursorPointer,
            timeHourglassCursor,
            cursorHand,
            magnifier,
            magnifierPlus,
            magnifierMinus,
            switchOnOff,
            playButton,
            pauseButton,
            stopButton,
            playPauseButton,
            camera,
            videoCamera,
            music,
            microphone,
            audioSpeaker,
            audioSpeakerMute,
            megaphone,
            wlan_wifi,
            telephone,
            battery,
            cloud,
            timeClock,
            locationPin,
            stars5Rate,

            //human:
            humanMale,
            humanFemale,
            thumbUp,
            thumbDown,
            speechBubble,
            speechBubbleEmpty,
            fist,
            boxingGlove,

            //nature/weather:
            sun,
            moonHalf,
            moonFullPlanet,
            stars3,
            shootingStar,
            rain,
            wind,
            snow,
            iceIcicle,
            lightning,
            fire,
            tree,
            palm,
            leaf,
            animal,
            bird,
            fish,
            mushroom,

            //games:
            heart,
            trophy,
            crown,
            awardMedal,
            star,
            bomb,
            bombExplosion,
            health,
            healthBox,
            pill,
            potion,
            death,
            gemDiamond,
            gold,
            coin,
            coins,
            moneyBills,
            moneyBag,
            presentGift,
            chestTreasureBox_closed,
            chestTreasureBox_open,
            lootbox,
            shoppingCart,
            map,
            compass,
            car,
            fuelStation,
            fuelCan,
            foodPlate,
            foodMeat,
            flag,
            flagChequered,
            crosshair,
            ball,
            dice,
            tower,
            jigsawPuzzle,
            rocket,
            magnet,
            doorClosed,
            doorOpen,
            doorEnter,
            doorLeave,
            key,
            lockLocked,
            lockUnlocked,
            gamepad,
            joystick,
            lightBulbOn,
            lightBulbOff,

            //tools/weapons:
            pen,
            gun,
            bullet,
            sword,
            shield,
            hammer,
            shovel,
            axe,
            pickAxe,
            arrow,
            arrowBow,

            //signs/warning:
            warning,
            fireWarning,
            nukeNuclearWarning,
            biohazardWarning,
            emergencyExit,
            logMessage,
            logMessageError,
            logMessageException,
            logMessageAssertion,

            //basics:
            questionMark,
            exclamationMark,
            checkmarkChecked,
            checkmarkUnchecked,
            arrowLeft,
            arrowRight,
            arrowUp,
            arrowDown,
            up_oneStroke,
            up_twoStroke,
            up_threeStroke,
            down_oneStroke,
            down_twoStroke,
            down_threeStroke,
            left_oneStroke,
            left_twoStroke,
            left_threeStroke,
            right_oneStroke,
            right_twoStroke,
            right_threeStroke,
            circleDotFilled,
            circleDotUnfilled,
            unitCircle,
            unitSquareCrossed,
            unitSquare,
            unitSquareIncl1Right,
            unitSquareIncl2Right,
            unitSquareIncl3Right,
            unitSquareIncl4Right,
            unitSquareIncl5Right,
            unitSquareIncl6Right,

            //miscellaneous:
            leftHandRule,
            rightHandRule
        };

        public static void Line(Vector3 start, Vector3 end, Color color = default(Color), float width = 0.0f, string text = null, LineStyle style = LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, Vector3 customAmplitudeAndTextDir = default(Vector3), bool flattenThickRoundLineIntoAmplitudePlane = false, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Line_fadeableAnimSpeed.InternalDraw(start, end, color, width, text, style, stylePatternScaleFactor, animationSpeed, null, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void Ray(Vector3 start, Vector3 direction, Color color = default(Color), float width = 0.0f, string text = null, LineStyle style = LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, Vector3 customAmplitudeAndTextDir = default(Vector3), bool flattenThickRoundLineIntoAmplitudePlane = false, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {

            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Ray_fadeableAnimSpeed.InternalDraw(start, direction, color, width, text, style, stylePatternScaleFactor, animationSpeed, null, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void LineFrom(Vector3 start, Vector3 direction, Color color = default(Color), float width = 0.0f, string text = null, LineStyle style = LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, Vector3 customAmplitudeAndTextDir = default(Vector3), bool flattenThickRoundLineIntoAmplitudePlane = false, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            LineFrom_fadeableAnimSpeed.InternalDraw(start, direction, color, width, text, style, stylePatternScaleFactor, animationSpeed, null, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void LineTo(Vector3 direction, Vector3 end, Color color = default(Color), float width = 0.0f, string text = null, LineStyle style = LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, Vector3 customAmplitudeAndTextDir = default(Vector3), bool flattenThickRoundLineIntoAmplitudePlane = false, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            LineTo_fadeableAnimSpeed.InternalDraw(direction, end, color, width, text, style, stylePatternScaleFactor, animationSpeed, null, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void LineColorFade(Vector3 start, Vector3 end, Color startColor, Color endColor, float width = 0.0f, string text = null, LineStyle style = LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, Vector3 customAmplitudeAndTextDir = default(Vector3), bool flattenThickRoundLineIntoAmplitudePlane = false, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Line_fadeableAnimSpeed.InternalDrawColorFade(start, end, startColor, endColor, width, text, style, stylePatternScaleFactor, animationSpeed, null, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void RayColorFade(Vector3 start, Vector3 direction, Color startColor, Color endColor, float width = 0.0f, string text = null, LineStyle style = LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, Vector3 customAmplitudeAndTextDir = default(Vector3), bool flattenThickRoundLineIntoAmplitudePlane = false, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Ray_fadeableAnimSpeed.InternalDrawColorFade(start, direction, startColor, endColor, width, text, style, stylePatternScaleFactor, animationSpeed, null, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void LineFrom_withColorFade(Vector3 start, Vector3 direction, Color startColor, Color endColor, float width = 0.0f, string text = null, LineStyle style = LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, Vector3 customAmplitudeAndTextDir = default(Vector3), bool flattenThickRoundLineIntoAmplitudePlane = false, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            LineFrom_fadeableAnimSpeed.InternalDraw_withColorFade(start, direction, startColor, endColor, width, text, style, stylePatternScaleFactor, animationSpeed, null, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void LineTo_withColorFade(Vector3 direction, Vector3 end, Color startColor, Color endColor, float width = 0.0f, string text = null, LineStyle style = LineStyle.solid, float stylePatternScaleFactor = 1.0f, float animationSpeed = 0.0f, Vector3 customAmplitudeAndTextDir = default(Vector3), bool flattenThickRoundLineIntoAmplitudePlane = false, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            LineTo_fadeableAnimSpeed.InternalDraw_withColorFade(direction, end, startColor, endColor, width, text, style, stylePatternScaleFactor, animationSpeed, null, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void LineCircled(Vector3 circleCenter, Vector3 circleCenter_to_start, Vector3 circleCenter_to_end, Color color = default(Color), float forceRadius = 0.0f, float width = 0.0f, string text = null, bool useReflexAngleOver180deg = false, bool skipFallbackDisplayOfZeroAngles = false, bool flattenThickRoundLineIntoCirclePlane = true, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_LineCircled.LineCircled(circleCenter, circleCenter_to_start, circleCenter_to_end, color, forceRadius, width, text, useReflexAngleOver180deg, skipFallbackDisplayOfZeroAngles, flattenThickRoundLineIntoCirclePlane, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, hiddenByNearerObjects);
        }

        public static void LineCircled(Vector3 circleCenterPos, Quaternion orientation, float turnAngleDegCC_startingFromUp, float radius, Color color, float width = 0.0f, string text = null, bool skipFallbackDisplayOfZeroAngles = false, bool flattenThickRoundLineIntoCirclePlane = true, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenterPos, "circleCenterPos")) { return; }

            orientation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(orientation);

            Vector3 startPos = circleCenterPos + orientation * Vector3.up * radius;
            Vector3 turnAxis_origin = circleCenterPos;
            Vector3 turnAxis_direction = orientation * Vector3.forward;
            LineCircled(startPos, turnAxis_origin, turnAxis_direction, turnAngleDegCC_startingFromUp, color, width, text, skipFallbackDisplayOfZeroAngles, flattenThickRoundLineIntoCirclePlane, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, hiddenByNearerObjects);
        }

        public static void LineCircled(Vector3 circleCenterPos, Quaternion orientation, float startAngleDegCC_relativeToUp, float endAngleDegCC_relativeToUp, float radius = 1.0f, Color color = default(Color), float width = 0.0f, string text = null, bool skipFallbackDisplayOfZeroAngles = false, bool flattenThickRoundLineIntoCirclePlane = true, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(startAngleDegCC_relativeToUp, "startAngleDegCC_relativeToUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(endAngleDegCC_relativeToUp, "endAngleDegCC_relativeToUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenterPos, "circleCenterPos")) { return; }

            orientation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(orientation);
            Vector3 turnAxis_direction = orientation * Vector3.forward;
            Quaternion fromOrientation_toOrientationSoThatTheStartAngleMarksTheRotationsUpwardDir = Quaternion.AngleAxis(startAngleDegCC_relativeToUp, turnAxis_direction);
            float turnedAngleDegCC_fromStartAngle = endAngleDegCC_relativeToUp - startAngleDegCC_relativeToUp;
            Vector3 startPos = circleCenterPos + fromOrientation_toOrientationSoThatTheStartAngleMarksTheRotationsUpwardDir * orientation * Vector3.up * radius;
            Vector3 turnAxis_origin = circleCenterPos;
            LineCircled(startPos, turnAxis_origin, turnAxis_direction, turnedAngleDegCC_fromStartAngle, color, width, text, skipFallbackDisplayOfZeroAngles, flattenThickRoundLineIntoCirclePlane, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, hiddenByNearerObjects);
        }

        public static void LineCircled(Vector3 startPos, Ray turnAxis, float turnAngleDegCC, Color color = default(Color), float width = 0.0f, string text = null, bool skipFallbackDisplayOfZeroAngles = false, bool flattenThickRoundLineIntoCirclePlane = true, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            LineCircled(startPos, turnAxis.origin, turnAxis.direction, turnAngleDegCC, color, width, text, skipFallbackDisplayOfZeroAngles, flattenThickRoundLineIntoCirclePlane, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, hiddenByNearerObjects);
        }

        public static void LineCircled(Vector3 startPos, Vector3 turnAxis_origin, Vector3 turnAxis_direction, float turnAngleDegCC, Color color = default(Color), float width = 0.0f, string text = null, bool skipFallbackDisplayOfZeroAngles = false, bool flattenThickRoundLineIntoCirclePlane = true, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_LineCircled.LineCircled(startPos, turnAxis_origin, turnAxis_direction, turnAngleDegCC, color, width, text, skipFallbackDisplayOfZeroAngles, flattenThickRoundLineIntoCirclePlane, durationInSec, hiddenByNearerObjects, false, minAngleDeg_withoutTextLineBreak, textAnchor);
        }

        public static void CircleSegment(Vector3 centerOfCircle, Vector3 circleCenter_to_startPosOnPerimeter, Vector3 circleCenter_to_endPosOnPerimeter, Color color = default(Color), float forceRadius = 0.0f, string text = null, bool useReflexAngleOver180deg = false, float radiusPortionWhereDrawFillStarts = 0.0f, bool skipFallbackDisplayOfZeroAngles = false, float fillDensity = 1.0f, float minAngleDeg_withoutTextLineBreak = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_LineCircled.CircleSegment(centerOfCircle, circleCenter_to_startPosOnPerimeter, circleCenter_to_endPosOnPerimeter, color, forceRadius, fillDensity, text, useReflexAngleOver180deg, radiusPortionWhereDrawFillStarts, skipFallbackDisplayOfZeroAngles, minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL.LowerLeftOfWholeTextBlock, durationInSec, hiddenByNearerObjects);
        }

        public static void CircleSegment(Vector3 centerOfCircle, Quaternion orientation, float turnAngleDegCC_startingFromUp, float radius, Color color, string text = null, float radiusPortionWhereDrawFillStarts = 0.0f, bool skipFallbackDisplayOfZeroAngles = false, float fillDensity = 1.0f, float minAngleDeg_withoutTextLineBreak = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(centerOfCircle, "centerOfCircle")) { return; }

            orientation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(orientation);

            Vector3 startPos = centerOfCircle + orientation * Vector3.up * radius;
            Vector3 turnAxis_origin = centerOfCircle;
            Vector3 turnAxis_direction = orientation * Vector3.forward;
            CircleSegment(startPos, turnAxis_origin, turnAxis_direction, turnAngleDegCC_startingFromUp, color, text, radiusPortionWhereDrawFillStarts, skipFallbackDisplayOfZeroAngles, fillDensity, minAngleDeg_withoutTextLineBreak, durationInSec, hiddenByNearerObjects);
        }

        public static void CircleSegment(Vector3 centerOfCircle, Quaternion orientation, float startAngleDegCC_relativeToUp, float endAngleDegCC_relativeToUp, float radius = 1.0f, Color color = default(Color), string text = null, float radiusPortionWhereDrawFillStarts = 0.0f, bool skipFallbackDisplayOfZeroAngles = false, float fillDensity = 1.0f, float minAngleDeg_withoutTextLineBreak = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(startAngleDegCC_relativeToUp, "startAngleDegCC_relativeToUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(endAngleDegCC_relativeToUp, "endAngleDegCC_relativeToUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(centerOfCircle, "centerOfCircle")) { return; }

            orientation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(orientation);
            Vector3 turnAxis_direction = orientation * Vector3.forward;
            Quaternion fromOrientation_toOrientationSoThatTheStartAngleMarksTheRotationsUpwardDir = Quaternion.AngleAxis(startAngleDegCC_relativeToUp, turnAxis_direction);
            float turnedAngleDegCC_fromStartAngle = endAngleDegCC_relativeToUp - startAngleDegCC_relativeToUp;
            Vector3 startPos = centerOfCircle + fromOrientation_toOrientationSoThatTheStartAngleMarksTheRotationsUpwardDir * orientation * Vector3.up * radius;
            Vector3 turnAxis_origin = centerOfCircle;
            CircleSegment(startPos, turnAxis_origin, turnAxis_direction, turnedAngleDegCC_fromStartAngle, color, text, radiusPortionWhereDrawFillStarts, skipFallbackDisplayOfZeroAngles, fillDensity, minAngleDeg_withoutTextLineBreak, durationInSec, hiddenByNearerObjects);
        }

        public static void CircleSegment(Vector3 startPos, Ray turnAxis, float turnAngleDegCC, Color color = default(Color), string text = null, float radiusPortionWhereDrawFillStarts = 0.0f, bool skipFallbackDisplayOfZeroAngles = false, float fillDensity = 1.0f, float minAngleDeg_withoutTextLineBreak = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            CircleSegment(startPos, turnAxis.origin, turnAxis.direction, turnAngleDegCC, color, text, radiusPortionWhereDrawFillStarts, skipFallbackDisplayOfZeroAngles, fillDensity, minAngleDeg_withoutTextLineBreak, durationInSec, hiddenByNearerObjects);
        }

        public static void CircleSegment(Vector3 startPosOnPerimeter, Vector3 centerOfCircle, Vector3 normalOfCircle, float turnAngleDegCC, Color color = default(Color), string text = null, float radiusPortionWhereDrawFillStarts = 0.0f, bool skipFallbackDisplayOfZeroAngles = false, float fillDensity = 1.0f, float minAngleDeg_withoutTextLineBreak = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_LineCircled.CircleSegment(startPosOnPerimeter, centerOfCircle, normalOfCircle, turnAngleDegCC, color, text, radiusPortionWhereDrawFillStarts, skipFallbackDisplayOfZeroAngles, fillDensity, durationInSec, hiddenByNearerObjects, false, minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL.LowerLeftOfWholeTextBlock);
        }

        public static void LineString(Vector3[] points, Color color = default(Color), bool closeGapBetweenLastAndFirstPoint = false, float width = 0.0f, string text = null, LineStyle style = LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width, "width")) { return; }
            width = UtilitiesDXXL_Math.AbsNonZeroValue(width);

            if (points == null)
            {
                Debug.LogError("'points' is 'null'");
                return;
            }

            if (points.Length == 0)
            {
                Debug.Log("'points' has 0 items -> no drawing");
                return;
            }

            for (int i = 0; i < (points.Length - 1); i++)
            {
                Line_fadeableAnimSpeed.InternalDraw(points[i], points[i + 1], color, width, null, style, stylePatternScaleFactor, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            if (closeGapBetweenLastAndFirstPoint)
            {
                Line_fadeableAnimSpeed.InternalDraw(points[points.Length - 1], points[0], color, width, null, style, stylePatternScaleFactor, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            if (text != null && text != "")
            {
                TagLineString(text, points, width, color, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void LineString(List<Vector3> points, Color color = default(Color), bool closeGapBetweenLastAndFirstPoint = false, float width = 0.0f, string text = null, LineStyle style = LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width, "width")) { return; }
            width = UtilitiesDXXL_Math.AbsNonZeroValue(width);
            if (points == null)
            {
                Debug.LogError("'points' is 'null'");
                return;
            }

            if (points.Count == 0)
            {
                Debug.Log("'points' has 0 items -> no drawing");
                return;
            }

            for (int i = 0; i < (points.Count - 1); i++)
            {
                Line_fadeableAnimSpeed.InternalDraw(points[i], points[i + 1], color, width, null, style, stylePatternScaleFactor, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            if (closeGapBetweenLastAndFirstPoint)
            {
                Line_fadeableAnimSpeed.InternalDraw(points[points.Count - 1], points[0], color, width, null, style, stylePatternScaleFactor, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            if (text != null && text != "")
            {
                TagLineString(text, points, width, color, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void LineStringColorFade(Vector3[] points, Color startColor, Color endColor, bool closeGapBetweenLastAndFirstPoint = false, float width = 0.0f, string text = null, LineStyle style = LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width, "width")) { return; }
            width = UtilitiesDXXL_Math.AbsNonZeroValue(width);
            if (points == null)
            {
                Debug.LogError("'points' is 'null'");
                return;
            }

            if (points.Length == 0)
            {
                Debug.Log("'points' has 0 items -> no drawing");
                return;
            }

            int iOffset_forColorFade = -1;
            if (closeGapBetweenLastAndFirstPoint)
            {
                iOffset_forColorFade = 0;
            }

            for (int i = 0; i < (points.Length - 1); i++)
            {
                Color color = UtilitiesDXXL_DrawBasics.GetFadedColorFromSegments(startColor, endColor, i, points.Length + iOffset_forColorFade);
                Line_fadeableAnimSpeed.InternalDraw(points[i], points[i + 1], color, width, null, style, stylePatternScaleFactor, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            if (closeGapBetweenLastAndFirstPoint)
            {
                Line_fadeableAnimSpeed.InternalDraw(points[points.Length - 1], points[0], endColor, width, null, style, stylePatternScaleFactor, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            if (text != null && text != "")
            {
                Color averageColor = Color.Lerp(startColor, endColor, 0.5f);
                TagLineString(text, points, width, averageColor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void LineStringColorFade(List<Vector3> points, Color startColor, Color endColor, bool closeGapBetweenLastAndFirstPoint = false, float width = 0.0f, string text = null, LineStyle style = LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width, "width")) { return; }
            width = UtilitiesDXXL_Math.AbsNonZeroValue(width);
            if (points == null)
            {
                Debug.LogError("'points' is 'null'");
                return;
            }

            if (points.Count == 0)
            {
                Debug.Log("'points' has 0 items -> no drawing");
                return;
            }

            int iOffset_forColorFade = -1;
            if (closeGapBetweenLastAndFirstPoint)
            {
                iOffset_forColorFade = 0;
            }

            for (int i = 0; i < (points.Count - 1); i++)
            {
                Color color = UtilitiesDXXL_DrawBasics.GetFadedColorFromSegments(startColor, endColor, i, points.Count + iOffset_forColorFade);
                Line_fadeableAnimSpeed.InternalDraw(points[i], points[i + 1], color, width, null, style, stylePatternScaleFactor, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            if (closeGapBetweenLastAndFirstPoint)
            {
                Line_fadeableAnimSpeed.InternalDraw(points[points.Count - 1], points[0], endColor, width, null, style, stylePatternScaleFactor, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            if (text != null && text != "")
            {
                Color averageColor = Color.Lerp(startColor, endColor, 0.5f);
                TagLineString(text, points, width, averageColor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
        }

        static void TagLineString(string text, Vector3[] lineStringVerticesGlobal, float linesWidth, Color colorOfLines, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects)
        {
            if (text != null && text != "")
            {
                float xMin = UtilitiesDXXL_Math.GetLowestXComponent(lineStringVerticesGlobal);
                float xMax = UtilitiesDXXL_Math.GetHighestXComponent(lineStringVerticesGlobal);
                float yMin = UtilitiesDXXL_Math.GetLowestYComponent(lineStringVerticesGlobal);
                float yMax = UtilitiesDXXL_Math.GetHighestYComponent(lineStringVerticesGlobal);
                float zMin = UtilitiesDXXL_Math.GetLowestZComponent(lineStringVerticesGlobal);
                float zMax = UtilitiesDXXL_Math.GetHighestZComponent(lineStringVerticesGlobal);
                Vector3 virtualScale = new Vector3(xMax - xMin, yMax - yMin, zMax - zMin);
                Vector3 centerPosition = lineStringVerticesGlobal[0];
                for (int i = 0; i < lineStringVerticesGlobal.Length; i++)
                {
                    UtilitiesDXXL_List.AddToAVectorList(ref UtilitiesDXXL_Shapes.verticesLocal, lineStringVerticesGlobal[i] - centerPosition, i);
                }
                Color invertedColor = UtilitiesDXXL_Colors.Invert_andAlphaTo1(colorOfLines);
                UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, null, centerPosition, lineStringVerticesGlobal.Length, 0.1f * linesWidth, virtualScale, invertedColor, invertedColor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
        }

        static void TagLineString(string text, List<Vector3> lineStringVerticesGlobal, float linesWidth, Color colorOfLines, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects)
        {
            if (text != null && text != "")
            {
                float xMin = UtilitiesDXXL_Math.GetLowestXComponent(lineStringVerticesGlobal);
                float xMax = UtilitiesDXXL_Math.GetHighestXComponent(lineStringVerticesGlobal);
                float yMin = UtilitiesDXXL_Math.GetLowestYComponent(lineStringVerticesGlobal);
                float yMax = UtilitiesDXXL_Math.GetHighestYComponent(lineStringVerticesGlobal);
                float zMin = UtilitiesDXXL_Math.GetLowestZComponent(lineStringVerticesGlobal);
                float zMax = UtilitiesDXXL_Math.GetHighestZComponent(lineStringVerticesGlobal);
                Vector3 virtualScale = new Vector3(xMax - xMin, yMax - yMin, zMax - zMin);
                Vector3 centerPosition = lineStringVerticesGlobal[0];
                for (int i = 0; i < lineStringVerticesGlobal.Count; i++)
                {
                    UtilitiesDXXL_List.AddToAVectorList(ref UtilitiesDXXL_Shapes.verticesLocal, lineStringVerticesGlobal[i] - centerPosition, i);
                }
                Color invertedColor = UtilitiesDXXL_Colors.Invert_andAlphaTo1(colorOfLines);
                UtilitiesDXXL_TextTagForPointCollection.TagPointCollection(text, null, centerPosition, lineStringVerticesGlobal.Count, 0.1f * linesWidth, virtualScale, invertedColor, invertedColor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void PointArray(Vector3[] points, Color color = default(Color), float sizeOfMarkingCross = 1.0f, float markingCrossLinesWidth = 0.0f, bool drawCoordsAsText = true, bool hideZDir = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }

            for (int i = 0; i < points.Length; i++)
            {
                Point(points[i], color, sizeOfMarkingCross, Quaternion.identity, markingCrossLinesWidth, null, color, false, drawCoordsAsText, hideZDir, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void PointList(List<Vector3> points, Color color = default(Color), float sizeOfMarkingCross = 1.0f, float markingCrossLinesWidth = 0.0f, bool drawCoordsAsText = true, bool hideZDir = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }

            for (int i = 0; i < points.Count; i++)
            {
                Point(points[i], color, sizeOfMarkingCross, Quaternion.identity, markingCrossLinesWidth, null, color, false, drawCoordsAsText, hideZDir, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void Point(Vector3 position, Color markingCrossColor, float sizeOfMarkingCross = 1.0f, Quaternion rotation = default(Quaternion), float markingCrossLinesWidth = 0.0f, string text = null, Color textColor = default(Color), bool pointer_as_textAttachStyle = false, bool drawCoordsAsText = true, bool hideZDir = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Point(position, text, textColor, sizeOfMarkingCross, markingCrossLinesWidth, markingCrossColor, rotation, pointer_as_textAttachStyle, drawCoordsAsText, hideZDir, durationInSec, hiddenByNearerObjects);
        }

        public static void Point(Vector3 position, string text = null, Color textColor = default(Color), float sizeOfMarkingCross = 1.0f, float markingCrossLinesWidth = 0.0f, Color overwrite_markingCrossColor = default(Color), Quaternion rotation = default(Quaternion), bool pointer_as_textAttachStyle = true, bool drawCoordsAsText = true, bool hideZDir = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_DrawBasics.Point(false, position, text, textColor, sizeOfMarkingCross, markingCrossLinesWidth, overwrite_markingCrossColor, rotation, pointer_as_textAttachStyle, drawCoordsAsText, false, true, Vector3.zero, Quaternion.identity, Vector3.one, hideZDir, durationInSec, hiddenByNearerObjects);
        }

        public static void PointLocalArray(Vector3[] localPoints, Transform parentTransform, Color color = default(Color), float sizeOfMarkingCross_global = 1.0f, float markingCrossLinesWidth = 0.0f, bool drawCoordsAsText = true, bool additionallyDrawGlobalCoords = false, bool drawLocalOrigin = true, bool hideZDir = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(localPoints, "points")) { return; }
            if (parentTransform == null)
            {
                PointArray(localPoints, color, sizeOfMarkingCross_global, markingCrossLinesWidth, drawCoordsAsText, hideZDir, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                for (int i = 0; i < localPoints.Length; i++)
                {
                    PointLocal(localPoints[i], parentTransform.position, parentTransform.rotation, parentTransform.lossyScale, null, color, sizeOfMarkingCross_global, markingCrossLinesWidth, color, Quaternion.identity, false, drawCoordsAsText, additionallyDrawGlobalCoords, drawLocalOrigin, hideZDir, durationInSec, hiddenByNearerObjects);
                }
            }
        }

        public static void PointLocalList(List<Vector3> localPoints, Transform parentTransform, Color color = default(Color), float sizeOfMarkingCross_global = 1.0f, float markingCrossLinesWidth = 0.0f, bool drawCoordsAsText = true, bool additionallyDrawGlobalCoords = false, bool drawLocalOrigin = true, bool hideZDir = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(localPoints, "points")) { return; }
            if (parentTransform == null)
            {
                PointList(localPoints, color, sizeOfMarkingCross_global, markingCrossLinesWidth, drawCoordsAsText, hideZDir, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                for (int i = 0; i < localPoints.Count; i++)
                {
                    PointLocal(localPoints[i], parentTransform.position, parentTransform.rotation, parentTransform.lossyScale, null, color, sizeOfMarkingCross_global, markingCrossLinesWidth, color, Quaternion.identity, false, drawCoordsAsText, additionallyDrawGlobalCoords, drawLocalOrigin, hideZDir, durationInSec, hiddenByNearerObjects);
                }
            }
        }

        public static void PointLocalArray(Vector3[] localPoints, Vector3 parentPositionGlobal, Quaternion parentRotationGlobal, Vector3 parentScaleGlobal, Color color = default(Color), float sizeOfMarkingCross_global = 1.0f, float markingCrossLinesWidth = 0.0f, bool drawCoordsAsText = true, bool additionallyDrawGlobalCoords = false, bool drawLocalOrigin = true, bool hideZDir = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(localPoints, "points")) { return; }

            for (int i = 0; i < localPoints.Length; i++)
            {
                PointLocal(localPoints[i], parentPositionGlobal, parentRotationGlobal, parentScaleGlobal, null, color, sizeOfMarkingCross_global, markingCrossLinesWidth, color, Quaternion.identity, false, drawCoordsAsText, additionallyDrawGlobalCoords, drawLocalOrigin, hideZDir, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void PointLocalList(List<Vector3> localPoints, Vector3 parentPositionGlobal, Quaternion parentRotationGlobal, Vector3 parentScaleGlobal, Color color = default(Color), float sizeOfMarkingCross_global = 1.0f, float markingCrossLinesWidth = 0.0f, bool drawCoordsAsText = true, bool additionallyDrawGlobalCoords = false, bool drawLocalOrigin = true, bool hideZDir = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(localPoints, "points")) { return; }

            for (int i = 0; i < localPoints.Count; i++)
            {
                PointLocal(localPoints[i], parentPositionGlobal, parentRotationGlobal, parentScaleGlobal, null, color, sizeOfMarkingCross_global, markingCrossLinesWidth, color, Quaternion.identity, false, drawCoordsAsText, additionallyDrawGlobalCoords, drawLocalOrigin, hideZDir, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void PointLocal(Vector3 localPosition, Transform parentTransform, Color markingCrossColor, float sizeOfMarkingCross_global = 1.0f, Quaternion localRotation = default(Quaternion), float markingCrossLinesWidth = 0.0f, string text = null, Color textColor = default(Color), bool pointer_as_textAttachStyle = false, bool drawCoordsAsText = true, bool additionallyDrawGlobalCoords = false, bool drawLocalOrigin = true, bool hideZDir = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (parentTransform == null)
            {
                text = "[<color=#adadadFF><icon=logMessage></color> parent transform that defines<br>   the local space is 'null'<br>   -> fallback to global coordinates]<br>" + text;
                Point(localPosition, markingCrossColor, sizeOfMarkingCross_global, localRotation, markingCrossLinesWidth, text, textColor, pointer_as_textAttachStyle, drawCoordsAsText, hideZDir, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                PointLocal(localPosition, parentTransform.position, parentTransform.rotation, parentTransform.lossyScale, markingCrossColor, sizeOfMarkingCross_global, localRotation, markingCrossLinesWidth, text, textColor, pointer_as_textAttachStyle, drawCoordsAsText, additionallyDrawGlobalCoords, drawLocalOrigin, hideZDir, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void PointLocal(Vector3 localPosition, Transform parentTransform, string text = null, Color textColor = default(Color), float sizeOfMarkingCross_global = 1.0f, float markingCrossLinesWidth = 0.0f, Color overwrite_markingCrossColor = default(Color), Quaternion localRotation = default(Quaternion), bool pointer_as_textAttachStyle = true, bool drawCoordsAsText = true, bool additionallyDrawGlobalCoords = true, bool drawLocalOrigin = true, bool hideZDir = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (parentTransform == null)
            {
                text = "[<color=#adadadFF><icon=logMessage></color> parent transform that defines<br>   the local space is 'null'<br>   -> fallback to global coordinates]<br>" + text;
                Point(localPosition, text, textColor, sizeOfMarkingCross_global, markingCrossLinesWidth, overwrite_markingCrossColor, localRotation, pointer_as_textAttachStyle, drawCoordsAsText, hideZDir, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                PointLocal(localPosition, parentTransform.position, parentTransform.rotation, parentTransform.lossyScale, text, textColor, sizeOfMarkingCross_global, markingCrossLinesWidth, overwrite_markingCrossColor, localRotation, pointer_as_textAttachStyle, drawCoordsAsText, additionallyDrawGlobalCoords, drawLocalOrigin, hideZDir, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void PointLocal(Vector3 localPosition, Vector3 parentPositionGlobal, Quaternion parentRotationGlobal, Vector3 parentScaleGlobal, Color markingCrossColor, float sizeOfMarkingCross_global = 1.0f, Quaternion localRotation = default(Quaternion), float markingCrossLinesWidth = 0.0f, string text = null, Color textColor = default(Color), bool pointer_as_textAttachStyle = false, bool drawCoordsAsText = true, bool additionallyDrawGlobalCoords = false, bool drawLocalOrigin = true, bool hideZDir = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            PointLocal(localPosition, parentPositionGlobal, parentRotationGlobal, parentScaleGlobal, text, textColor, sizeOfMarkingCross_global, markingCrossLinesWidth, markingCrossColor, localRotation, pointer_as_textAttachStyle, drawCoordsAsText, additionallyDrawGlobalCoords, drawLocalOrigin, hideZDir, durationInSec, hiddenByNearerObjects);
        }

        public static void PointLocal(Vector3 localPosition, Vector3 parentPositionGlobal, Quaternion parentRotationGlobal, Vector3 parentScaleGlobal, string text = null, Color textColor = default(Color), float sizeOfMarkingCross_global = 1.0f, float markingCrossLinesWidth = 0.0f, Color overwrite_markingCrossColor = default(Color), Quaternion localRotation = default(Quaternion), bool pointer_as_textAttachStyle = true, bool drawCoordsAsText = true, bool additionallyDrawGlobalCoords = true, bool drawLocalOrigin = true, bool hideZDir = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_DrawBasics.Point(false, localPosition, text, textColor, sizeOfMarkingCross_global, markingCrossLinesWidth, overwrite_markingCrossColor, localRotation, pointer_as_textAttachStyle, drawCoordsAsText, additionallyDrawGlobalCoords, false, parentPositionGlobal, parentRotationGlobal, parentScaleGlobal, hideZDir, durationInSec, hiddenByNearerObjects);
            if (drawLocalOrigin)
            {
                if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(sizeOfMarkingCross_global, "sizeOfMarkingCross_global")) { return; }

                if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(localPosition, "localPosition")) { return; }
                if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(parentPositionGlobal, "parentPositionGlobal")) { return; }
                if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(parentScaleGlobal, "parentScaleGlobal")) { return; }

                parentRotationGlobal = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(parentRotationGlobal);

                Vector3 parentForward = parentRotationGlobal * Vector3.forward;
                Vector3 parentUp = parentRotationGlobal * Vector3.up;
                Vector3 parentRight = parentRotationGlobal * Vector3.right;
                Vector3 worldPosition = parentPositionGlobal + parentRight * parentScaleGlobal.x * localPosition.x + parentUp * parentScaleGlobal.y * localPosition.y + parentForward * parentScaleGlobal.z * localPosition.z;
                DrawConnection_fromLocalPoint_toLocalOrigin(parentPositionGlobal, worldPosition, 0.3f * sizeOfMarkingCross_global, parentRotationGlobal, durationInSec, hiddenByNearerObjects);
            }
        }

        static void DrawConnection_fromLocalPoint_toLocalOrigin(Vector3 originWorldPosition, Vector3 markedPointWorldPosition, float sizeOfMarkingCross, Quaternion parentRotationGlobal, float durationInSec, bool hiddenByNearerObjects)
        {
            Color originColor = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(Color.white, 0.2f);
            UtilitiesDXXL_DrawBasics.Point(false, originWorldPosition, "<size=5>local<br>origin</size>", originColor, sizeOfMarkingCross, 0.0f, originColor, parentRotationGlobal, false, false, false, true, Vector3.zero, Quaternion.identity, Vector3.one, false, durationInSec, hiddenByNearerObjects);
            Vector3 localOriginToMarkedPoint = markedPointWorldPosition - originWorldPosition;
            float distanceToLocalOrigin = localOriginToMarkedPoint.magnitude;
            Line_fadeableAnimSpeed.InternalDraw(originWorldPosition, markedPointWorldPosition, originColor, 0.0f, null, LineStyle.dashedLong, distanceToLocalOrigin, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
        }

        public static void PointTag(Vector3 position, string text = null, Color color = default(Color), float linesWidth = 0.0f, float size_asTextOffsetDistance = 1.0f, Vector3 textOffsetDirection = default(Vector3), float textSizeScaleFactor = 1.0f, bool skipConeDrawing = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_DrawBasics.PointTag(position, text, color, linesWidth, size_asTextOffsetDistance, textOffsetDirection, textSizeScaleFactor, skipConeDrawing, durationInSec, hiddenByNearerObjects);
        }

        public static void Vector(Vector3 vectorStartPos, Vector3 vectorEndPos, Color color = default(Color), float lineWidth = 0.0f, string text = null, float coneLength = 0.17f, bool pointerAtBothSides = false, bool flattenThickRoundLineIntoAmplitudePlane = false, Vector3 customAmplitudeAndTextDir = default(Vector3), bool addNormalizedMarkingText = false, float enlargeSmallTextToThisMinTextSize = 0.0f, bool writeComponentValuesAsText = false, float endPlates_size = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_DrawBasics.Vector(vectorStartPos, vectorEndPos, color, lineWidth, text, coneLength, pointerAtBothSides, flattenThickRoundLineIntoAmplitudePlane, addNormalizedMarkingText, enlargeSmallTextToThisMinTextSize, writeComponentValuesAsText, durationInSec, hiddenByNearerObjects, customAmplitudeAndTextDir, false, endPlates_size);
        }

        public static void VectorFrom(Vector3 vectorStartPos, Vector3 vector, Color color = default(Color), float lineWidth = 0.0f, string text = null, float coneLength = 0.17f, bool pointerAtBothSides = false, bool flattenThickRoundLineIntoAmplitudePlane = false, Vector3 customAmplitudeAndTextDir = default(Vector3), bool addNormalizedMarkingText = false, float enlargeSmallTextToThisMinTextSize = 0.0f, bool writeComponentValuesAsText = false, float endPlates_size = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_DrawBasics.VectorFrom(vectorStartPos, vector, color, lineWidth, text, coneLength, pointerAtBothSides, flattenThickRoundLineIntoAmplitudePlane, addNormalizedMarkingText, enlargeSmallTextToThisMinTextSize, writeComponentValuesAsText, durationInSec, hiddenByNearerObjects, customAmplitudeAndTextDir, false, endPlates_size);
        }

        public static void VectorTo(Vector3 vector, Vector3 vectorEndPos, Color color = default(Color), float lineWidth = 0.0f, string text = null, float coneLength = 0.17f, bool pointerAtBothSides = false, bool flattenThickRoundLineIntoAmplitudePlane = false, Vector3 customAmplitudeAndTextDir = default(Vector3), bool addNormalizedMarkingText = false, float enlargeSmallTextToThisMinTextSize = 0.0f, bool writeComponentValuesAsText = false, float endPlates_size = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vectorEndPos, "vectorEndPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vector, "vector")) { return; }
            VectorFrom(vectorEndPos - vector, vector, color, lineWidth, text, coneLength, pointerAtBothSides, flattenThickRoundLineIntoAmplitudePlane, customAmplitudeAndTextDir, addNormalizedMarkingText, enlargeSmallTextToThisMinTextSize, writeComponentValuesAsText, endPlates_size, durationInSec, hiddenByNearerObjects);
        }

        public static void VectorCircled(Vector3 circleCenter, Vector3 circleCenter_to_start, Vector3 circleCenter_to_end, Color color = default(Color), float forceRadius = 0.0f, float lineWidth = 0.0f, string text = null, bool useReflexAngleOver180deg = false, float coneLength = 0.17f, bool pointerAtBothSides = false, bool skipFallbackDisplayOfZeroAngles = false, bool flattenThickRoundLineIntoCirclePlane = true, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_LineCircled.VectorCircled(circleCenter, circleCenter_to_start, circleCenter_to_end, color, forceRadius, lineWidth, text, useReflexAngleOver180deg, coneLength, skipFallbackDisplayOfZeroAngles, pointerAtBothSides, flattenThickRoundLineIntoCirclePlane, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, hiddenByNearerObjects);
        }

        public static void VectorCircled(Vector3 circleCenterPos, Quaternion orientation, float turnAngleDegCC_startingFromUp, float radius, Color color, float lineWidth = 0.0f, string text = null, float coneLength = 0.17f, bool pointerAtBothSides = false, bool skipFallbackDisplayOfZeroAngles = false, bool flattenThickRoundLineIntoCirclePlane = true, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenterPos, "circleCenterPos")) { return; }

            orientation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(orientation);

            Vector3 startPos = circleCenterPos + orientation * Vector3.up * radius;
            Vector3 turnAxis_origin = circleCenterPos;
            Vector3 turnAxis_direction = orientation * Vector3.forward;
            VectorCircled(startPos, turnAxis_origin, turnAxis_direction, turnAngleDegCC_startingFromUp, color, lineWidth, text, coneLength, pointerAtBothSides, skipFallbackDisplayOfZeroAngles, flattenThickRoundLineIntoCirclePlane, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, hiddenByNearerObjects);
        }

        public static void VectorCircled(Vector3 circleCenterPos, Quaternion orientation, float startAngleDegCC_relativeToUp, float endAngleDegCC_relativeToUp, float radius = 1.0f, Color color = default(Color), float lineWidth = 0.0f, string text = null, float coneLength = 0.17f, bool pointerAtBothSides = false, bool skipFallbackDisplayOfZeroAngles = false, bool flattenThickRoundLineIntoCirclePlane = true, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(startAngleDegCC_relativeToUp, "startAngleDegCC_relativeToUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(endAngleDegCC_relativeToUp, "endAngleDegCC_relativeToUp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenterPos, "circleCenterPos")) { return; }

            orientation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(orientation);
            Vector3 turnAxis_direction = orientation * Vector3.forward;
            Quaternion fromOrientation_toOrientationSoThatTheStartAngleMarksTheRotationsUpwardDir = Quaternion.AngleAxis(startAngleDegCC_relativeToUp, turnAxis_direction);
            float turnedAngleDegCC_fromStartAngle = endAngleDegCC_relativeToUp - startAngleDegCC_relativeToUp;
            Vector3 startPos = circleCenterPos + fromOrientation_toOrientationSoThatTheStartAngleMarksTheRotationsUpwardDir * orientation * Vector3.up * radius;
            Vector3 turnAxis_origin = circleCenterPos;
            VectorCircled(startPos, turnAxis_origin, turnAxis_direction, turnedAngleDegCC_fromStartAngle, color, lineWidth, text, coneLength, pointerAtBothSides, skipFallbackDisplayOfZeroAngles, flattenThickRoundLineIntoCirclePlane, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, hiddenByNearerObjects);
        }

        public static void VectorCircled(Vector3 startPos, Ray turnAxis, float turnAngleDegCC, Color color = default(Color), float lineWidth = 0.0f, string text = null, float coneLength = 0.17f, bool pointerAtBothSides = false, bool skipFallbackDisplayOfZeroAngles = false, bool flattenThickRoundLineIntoCirclePlane = true, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            VectorCircled(startPos, turnAxis.origin, turnAxis.direction, turnAngleDegCC, color, lineWidth, text, coneLength, pointerAtBothSides, skipFallbackDisplayOfZeroAngles, flattenThickRoundLineIntoCirclePlane, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, hiddenByNearerObjects);
        }

        public static void VectorCircled(Vector3 startPos, Vector3 turnAxis_origin, Vector3 turnAxis_direction, float turnAngleDegCC, Color color = default(Color), float lineWidth = 0.0f, string text = null, float coneLength = 0.17f, bool pointerAtBothSides = false, bool skipFallbackDisplayOfZeroAngles = false, bool flattenThickRoundLineIntoCirclePlane = true, float minAngleDeg_withoutTextLineBreak = 45.0f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_LineCircled.VectorCircled(startPos, turnAxis_origin, turnAxis_direction, turnAngleDegCC, color, lineWidth, text, coneLength, skipFallbackDisplayOfZeroAngles, pointerAtBothSides, flattenThickRoundLineIntoCirclePlane, minAngleDeg_withoutTextLineBreak, textAnchor, durationInSec, hiddenByNearerObjects, 1.0f);
        }
     
        public static void Icon(Vector3 position, IconType icon, Color color, float size, string text, Vector3 normal, Vector3 up_insideIconPlane = default(Vector3), int strokeWidth_asPPMofSize = 0, bool mirrorHorizontally = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(normal, "normal")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up_insideIconPlane, "up_insideIconPlane")) { return; }

            Quaternion rotation = UtilitiesDXXL_DrawBasics.GetRotationOfIcon(position, normal, up_insideIconPlane);
            Icon(position, icon, color, size, text, rotation, strokeWidth_asPPMofSize, mirrorHorizontally, durationInSec, hiddenByNearerObjects);
        }

        public static void Icon(Vector3 position, IconType icon, Color color = default(Color), float size = 1.0f, string text = null, Quaternion rotation = default(Quaternion), int strokeWidth_asPPMofSize = 0, bool mirrorHorizontally = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_DrawBasics.Icon(position, icon, color, size, text, rotation, strokeWidth_asPPMofSize, mirrorHorizontally, durationInSec, hiddenByNearerObjects, 0.1f, 0.004f, true);
        }

        public static void DrawAtlasOfAllIconsWithTheirNames(Vector3 position = default(Vector3), Color iconsColor = default(Color), Color textColor = default(Color), bool displayNameTexts = true, float sizeOfIconWall = 10.0f)
        {
            UtilitiesDXXL_CharsAndIcons.DrawAllIconsWithTheirNames(position, iconsColor, textColor, displayNameTexts, sizeOfIconWall);
        }

        public static void Dot(Vector3 position, float radius = 0.5f, Vector3 normal = default(Vector3), Color color = default(Color), string text = null, float density = 1.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            UtilitiesDXXL_DrawBasics.Dot(position, radius, normal, color, text, density, durationInSec, hiddenByNearerObjects, true);
        }

        public static void MovingArrowsRay(Vector3 start, Vector3 direction, Color color = default(Color), float lineWidth = 0.05f, float distanceBetweenArrows = 0.5f, float lengthOfArrows = 0.15f, string text = null, float animationSpeed = 0.5f, bool backwardAnimationFlipsArrowDirection = true, bool flattenThickRoundLineIntoAmplitudePlane = true, Vector3 customAmplitudeAndTextDir = default(Vector3), float endPlates_size = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            MovingArrowsRay_fadeableAnimSpeed.InternalDraw(start, direction, color, lineWidth, distanceBetweenArrows, lengthOfArrows, text, animationSpeed, null, backwardAnimationFlipsArrowDirection, flattenThickRoundLineIntoAmplitudePlane, customAmplitudeAndTextDir, endPlates_size, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects);
        }

        public static void MovingArrowsLine(Vector3 start, Vector3 end, Color color = default(Color), float lineWidth = 0.05f, float distanceBetweenArrows = 0.5f, float lengthOfArrows = 0.15f, string text = null, float animationSpeed = 0.5f, bool backwardAnimationFlipsArrowDirection = true, bool flattenThickRoundLineIntoAmplitudePlane = true, Vector3 customAmplitudeAndTextDir = default(Vector3), float endPlates_size = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            MovingArrowsLine_fadeableAnimSpeed.InternalDraw(start, end, color, lineWidth, distanceBetweenArrows, lengthOfArrows, text, animationSpeed, null, backwardAnimationFlipsArrowDirection, flattenThickRoundLineIntoAmplitudePlane, customAmplitudeAndTextDir, endPlates_size, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects);
        }

        public static void RayWithAlternatingColors(Vector3 start, Vector3 direction, Color color1 = default(Color), Color color2 = default(Color), float width = 0.0f, float lengthOfStripes = 0.04f, string text = null, float animationSpeed = 0.0f, Vector3 customAmplitudeAndTextDir = default(Vector3), bool flattenThickRoundLineIntoAmplitudePlane = false, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            RayWithAlternatingColors_fadeableAnimSpeed.InternalDraw(start, direction, color1, color2, width, lengthOfStripes, text, animationSpeed, null, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void LineWithAlternatingColors(Vector3 start, Vector3 end, Color color1 = default(Color), Color color2 = default(Color), float width = 0.0f, float lengthOfStripes = 0.04f, string text = null, float animationSpeed = 0.0f, Vector3 customAmplitudeAndTextDir = default(Vector3), bool flattenThickRoundLineIntoAmplitudePlane = false, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            LineWithAlternatingColors_fadeableAnimSpeed.InternalDraw(start, end, color1, color2, width, lengthOfStripes, text, animationSpeed, null, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void BlinkingRay(Vector3 start, Vector3 direction, Color primaryColor = default(Color), float blinkDurationInSec = 0.5f, float width = 0.0f, string text = null, LineStyle style = LineStyle.solid, Color blinkColor = default(Color), float stylePatternScaleFactor = 1.0f, Vector3 customAmplitudeAndTextDir = default(Vector3), bool flattenThickRoundLineIntoAmplitudePlane = false, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(direction, "direction")) { return; }

            Vector3 end = start + direction;
            BlinkingLine(start, end, primaryColor, blinkDurationInSec, width, text, style, blinkColor, stylePatternScaleFactor, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void BlinkingLine(Vector3 start, Vector3 end, Color primaryColor = default(Color), float blinkDurationInSec = 0.5f, float width = 0.0f, string text = null, LineStyle style = LineStyle.solid, Color blinkColor = default(Color), float stylePatternScaleFactor = 1.0f, Vector3 customAmplitudeAndTextDir = default(Vector3), bool flattenThickRoundLineIntoAmplitudePlane = false, float endPlates_size = 0.0f, float alphaFadeOutLength_0to1 = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(blinkDurationInSec, "blinkDurationInSec")) { return; }

            blinkDurationInSec = Mathf.Max(blinkDurationInSec, UtilitiesDXXL_DrawBasics.min_blinkDurationInSec);
            float passedBlinkIntervallsSinceStartup = UtilitiesDXXL_LineStyles.GetTime() / blinkDurationInSec;
            primaryColor = UtilitiesDXXL_Colors.OverwriteDefaultColor(primaryColor);
            if (UtilitiesDXXL_Math.CheckIf_givenNumberIs_evenNotOdd(Mathf.FloorToInt(passedBlinkIntervallsSinceStartup)))
            {
                Line_fadeableAnimSpeed.InternalDraw(start, end, primaryColor, width, text, style, stylePatternScaleFactor, 0.0f, null, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
            }
            else
            {
                if (UtilitiesDXXL_Colors.IsDefaultColor(blinkColor))
                {
                    Color alternatingBlinkColor = UtilitiesDXXL_Colors.Invert_andAlphaTo1(primaryColor);
                    alternatingBlinkColor = UtilitiesDXXL_Colors.OverwriteColorNearGreyWithBlack(alternatingBlinkColor);
                    Line_fadeableAnimSpeed.InternalDraw(start, end, alternatingBlinkColor, width, text, style, stylePatternScaleFactor, 0.0f, null, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
                }
                else
                {
                    Line_fadeableAnimSpeed.InternalDraw(start, end, blinkColor, width, text, style, stylePatternScaleFactor, 0.0f, null, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, alphaFadeOutLength_0to1, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
                }
            }
        }

        public static void RayUnderTension(Vector3 start, Vector3 direction, float relaxedLength = 1.0f, Color relaxedColor = default(Color), LineStyle style = LineStyle.spiral, float stretchFactor_forStretchedTensionColor = 2.0f, Color color_forStretchedTension = default(Color), float stretchFactor_forSqueezedTensionColor = 0.0f, Color color_forSqueezedTension = default(Color), float width = 0.0f, string text = null, float alphaOfReferenceLengthDisplay = 0.15f, float stylePatternScaleFactor = 1.0f, Vector3 customAmplitudeAndTextDir = default(Vector3), bool flattenThickRoundLineIntoAmplitudePlane = false, float endPlates_size = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(direction, "direction")) { return; }
            LineUnderTension(start, start + direction, relaxedLength, relaxedColor, style, stretchFactor_forStretchedTensionColor, color_forStretchedTension, stretchFactor_forSqueezedTensionColor, color_forSqueezedTension, width, text, alphaOfReferenceLengthDisplay, stylePatternScaleFactor, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, endPlates_size, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        public static void LineUnderTension(Vector3 start, Vector3 end, float relaxedLength = 1.0f, Color relaxedColor = default(Color), LineStyle style = LineStyle.spiral, float stretchFactor_forStretchedTensionColor = 2.0f, Color color_forStretchedTension = default(Color), float stretchFactor_forSqueezedTensionColor = 0.0f, Color color_forSqueezedTension = default(Color), float width = 0.0f, string text = null, float alphaOfReferenceLengthDisplay = 0.15f, float stylePatternScaleFactor = 1.0f, Vector3 customAmplitudeAndTextDir = default(Vector3), bool flattenThickRoundLineIntoAmplitudePlane = false, float endPlates_size = 0.0f, float enlargeSmallTextToThisMinTextSize = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true, bool skipPatternEnlargementForLongLines = false, bool skipPatternEnlargementForShortLines = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

            bool parametersAreInvalid = UtilitiesDXXL_DrawBasics.GetSpecsOfLineUnderTension(out float tensionFactor, out Color usedColor, out float lineLength, start, end, relaxedLength, relaxedColor, color_forStretchedTension, color_forSqueezedTension, stretchFactor_forStretchedTensionColor, stretchFactor_forSqueezedTensionColor);
            if (parametersAreInvalid) { return; }
            UtilitiesDXXL_DrawBasics.TryDrawReferenceLengthDisplay_ofLineUnderTension(start, end, alphaOfReferenceLengthDisplay, relaxedLength, relaxedColor, lineLength, null, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_DrawBasics.Line(start, end, usedColor, width, text, style, stylePatternScaleFactor, 0.0f, null, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, 0.0f, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines, null, false, endPlates_size, tensionFactor);
        }

        public static void BezierSegmentQuadratic(GameObject startPositionAndDirection, GameObject endPosition, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(startPositionAndDirection, "startPositionAndDirection")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(endPosition, "endPosition")) { return; }
            BezierSegmentQuadratic(startPositionAndDirection.transform, endPosition.transform, color, text, width, straightSubDivisions, textSize, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentQuadratic(Transform startPositionAndDirection, Transform endPosition, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(startPositionAndDirection, "startPositionAndDirection")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(endPosition, "endPosition")) { return; }
            Vector3 controlPosInBetween = startPositionAndDirection.position + startPositionAndDirection.forward * startPositionAndDirection.localScale.z;
            BezierSegmentQuadratic(startPositionAndDirection.position, endPosition.position, controlPosInBetween, color, text, width, straightSubDivisions, textSize, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentQuadratic(GameObject startPosition, GameObject endPosition, GameObject controlPosInBetween, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(startPosition, "startPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(endPosition, "endPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(controlPosInBetween, "controlPosInBetween")) { return; }
            BezierSegmentQuadratic(startPosition.transform.position, endPosition.transform.position, controlPosInBetween.transform.position, color, text, width, straightSubDivisions, textSize, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentQuadratic(Transform startPosition, Transform endPosition, Transform controlPosInBetween, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(startPosition, "startPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(endPosition, "endPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(controlPosInBetween, "controlPosInBetween")) { return; }
            BezierSegmentQuadratic(startPosition.position, endPosition.position, controlPosInBetween.position, color, text, width, straightSubDivisions, textSize, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentQuadratic(Vector3 startPosition, Vector3 endPosition, Vector3 controlPosInBetween, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(startPosition, "startPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(endPosition, "endPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(controlPosInBetween, "controlPosInBetween")) { return; }

            UtilitiesDXXL_Bezier.BezierSegmentQuadratic(false, startPosition, endPosition, controlPosInBetween, color, text, width, straightSubDivisions, textSize, true, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentCubic(GameObject startPositionAndDirection, GameObject endPositionAndDirection, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, bool closeGapFromEndToStart = false, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(startPositionAndDirection, "startPositionAndDirection")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(endPositionAndDirection, "endPositionAndDirection")) { return; }
            BezierSegmentCubic(startPositionAndDirection.transform, endPositionAndDirection.transform, color, text, width, straightSubDivisions, closeGapFromEndToStart, textSize, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentCubic(Transform startPositionAndDirection, Transform endPositionAndDirection, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, bool closeGapFromEndToStart = false, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(startPositionAndDirection, "startPositionAndDirection")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(endPositionAndDirection, "endPositionAndDirection")) { return; }
            Vector3 controlPosOfStartDirection = startPositionAndDirection.position + startPositionAndDirection.forward * startPositionAndDirection.localScale.z;
            Vector3 controlPosOfEndDirection = endPositionAndDirection.position - endPositionAndDirection.forward * endPositionAndDirection.localScale.z;
            BezierSegmentCubic(startPositionAndDirection.position, endPositionAndDirection.position, controlPosOfStartDirection, controlPosOfEndDirection, color, text, width, straightSubDivisions, closeGapFromEndToStart, textSize, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentCubic(GameObject startPosition, GameObject endPosition, GameObject controlPosOfStartDirection, GameObject controlPosOfEndDirection, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, bool closeGapFromEndToStart = false, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(startPosition, "startPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(endPosition, "endPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(controlPosOfStartDirection, "controlPosOfStartDirection")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(controlPosOfEndDirection, "controlPosOfEndDirection")) { return; }
            BezierSegmentCubic(startPosition.transform.position, endPosition.transform.position, controlPosOfStartDirection.transform.position, controlPosOfEndDirection.transform.position, color, text, width, straightSubDivisions, closeGapFromEndToStart, textSize, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentCubic(Transform startPosition, Transform endPosition, Transform controlPosOfStartDirection, Transform controlPosOfEndDirection, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, bool closeGapFromEndToStart = false, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(startPosition, "startPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(endPosition, "endPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(controlPosOfStartDirection, "controlPosOfStartDirection")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(controlPosOfEndDirection, "controlPosOfEndDirection")) { return; }
            BezierSegmentCubic(startPosition.position, endPosition.position, controlPosOfStartDirection.position, controlPosOfEndDirection.position, color, text, width, straightSubDivisions, closeGapFromEndToStart, textSize, durationInSec, hiddenByNearerObjects);
        }

        public static void BezierSegmentCubic(Vector3 startPosition, Vector3 endPosition, Vector3 controlPosOfStartDirection, Vector3 controlPosOfEndDirection, Color color = default(Color), string text = null, float width = 0.0f, int straightSubDivisions = 50, bool closeGapFromEndToStart = false, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(startPosition, "startPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(endPosition, "endPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(controlPosOfStartDirection, "controlPosOfStartDirection")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(controlPosOfEndDirection, "controlPosOfEndDirection")) { return; }

            UtilitiesDXXL_Bezier.BezierSegmentCubic(false, startPosition, endPosition, controlPosOfStartDirection, controlPosOfEndDirection, color, text, width, straightSubDivisions, textSize, true, durationInSec, hiddenByNearerObjects);
            if (closeGapFromEndToStart)
            {
                Vector3 startPos_to_startControlPos = controlPosOfStartDirection - startPosition;
                Vector3 endPos_to_endControlPos = controlPosOfEndDirection - endPosition;
                Vector3 controlPosOfStartDirection_ofSecondCurve = startPosition - startPos_to_startControlPos;
                Vector3 controlPosOfEndDirection_ofSecondCurve = endPosition - endPos_to_endControlPos;
                UtilitiesDXXL_Bezier.BezierSegmentCubic(false, startPosition, endPosition, controlPosOfStartDirection_ofSecondCurve, controlPosOfEndDirection_ofSecondCurve, color, null, width, straightSubDivisions, textSize, true, durationInSec, hiddenByNearerObjects);
            }
        }

        static UtilitiesDXXL_Bezier.FlexibleGetPosAtIndex<GameObject[]> GetPositionsFromGameObjectsArray_preAllocated = UtilitiesDXXL_Bezier.GetPositionsFromGameObjectsArray;
        static UtilitiesDXXL_Bezier.FlexibleGetDirectionControlPosOfTransform<GameObject[]> GetForwardControlPosFromGameObjectsArray_preAllocated = UtilitiesDXXL_Bezier.GetForwardControlPosFromGameObjectsArray;
        static UtilitiesDXXL_Bezier.FlexibleGetDirectionControlPosOfTransform<GameObject[]> GetBackwardControlPosFromGameObjectsArray_preAllocated = UtilitiesDXXL_Bezier.GetBackwardControlPosFromGameObjectsArray;
        public static void BezierSpline(GameObject[] points, Color color = default(Color), BezierPosInterpretation interpretationOfArray = BezierPosInterpretation.onlySegmentStartPoints_backwardForwardIsAligned, string text = null, float width = 0.0f, bool closeGapFromEndToStart = false, int straightSubDivisionsPerSegment = 50, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            UtilitiesDXXL_Bezier.BezierSpline<GameObject[]>(false, 0.0f, points, GetPositionsFromGameObjectsArray_preAllocated, GetForwardControlPosFromGameObjectsArray_preAllocated, GetBackwardControlPosFromGameObjectsArray_preAllocated, points.Length, color, interpretationOfArray, text, width, closeGapFromEndToStart, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
        }


        static UtilitiesDXXL_Bezier.FlexibleGetPosAtIndex<List<GameObject>> GetPositionsFromGameObjectsList_preAllocated = UtilitiesDXXL_Bezier.GetPositionsFromGameObjectsList;
        static UtilitiesDXXL_Bezier.FlexibleGetDirectionControlPosOfTransform<List<GameObject>> GetForwardControlPosFromGameObjectsList_preAllocated = UtilitiesDXXL_Bezier.GetForwardControlPosFromGameObjectsList;
        static UtilitiesDXXL_Bezier.FlexibleGetDirectionControlPosOfTransform<List<GameObject>> GetBackwardControlPosFromGameObjectsList_preAllocated = UtilitiesDXXL_Bezier.GetBackwardControlPosFromGameObjectsList;
        public static void BezierSpline(List<GameObject> points, Color color = default(Color), BezierPosInterpretation interpretationOfList = BezierPosInterpretation.onlySegmentStartPoints_backwardForwardIsAligned, string text = null, float width = 0.0f, bool closeGapFromEndToStart = false, int straightSubDivisionsPerSegment = 50, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            UtilitiesDXXL_Bezier.BezierSpline<List<GameObject>>(false, 0.0f, points, GetPositionsFromGameObjectsList_preAllocated, GetForwardControlPosFromGameObjectsList_preAllocated, GetBackwardControlPosFromGameObjectsList_preAllocated, points.Count, color, interpretationOfList, text, width, closeGapFromEndToStart, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
        }


        static UtilitiesDXXL_Bezier.FlexibleGetPosAtIndex<Transform[]> GetPositionsFromTransformsArray_preAllocated = UtilitiesDXXL_Bezier.GetPositionsFromTransformsArray;
        static UtilitiesDXXL_Bezier.FlexibleGetDirectionControlPosOfTransform<Transform[]> GetForwardControlPosFromTransformsArray_preAllocated = UtilitiesDXXL_Bezier.GetForwardControlPosFromTransformsArray;
        static UtilitiesDXXL_Bezier.FlexibleGetDirectionControlPosOfTransform<Transform[]> GetBackwardControlPosFromTransformsArray_preAllocated = UtilitiesDXXL_Bezier.GetBackwardControlPosFromTransformsArray;
        public static void BezierSpline(Transform[] points, Color color = default(Color), BezierPosInterpretation interpretationOfArray = BezierPosInterpretation.onlySegmentStartPoints_backwardForwardIsAligned, string text = null, float width = 0.0f, bool closeGapFromEndToStart = false, int straightSubDivisionsPerSegment = 50, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            UtilitiesDXXL_Bezier.BezierSpline<Transform[]>(false, 0.0f, points, GetPositionsFromTransformsArray_preAllocated, GetForwardControlPosFromTransformsArray_preAllocated, GetBackwardControlPosFromTransformsArray_preAllocated, points.Length, color, interpretationOfArray, text, width, closeGapFromEndToStart, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
        }


        static UtilitiesDXXL_Bezier.FlexibleGetPosAtIndex<List<Transform>> GetPositionsFromTransformsList_preAllocated = UtilitiesDXXL_Bezier.GetPositionsFromTransformsList;
        static UtilitiesDXXL_Bezier.FlexibleGetDirectionControlPosOfTransform<List<Transform>> GetForwardControlPosFromTransformsList_preAllocated = UtilitiesDXXL_Bezier.GetForwardControlPosFromTransformsList;
        static UtilitiesDXXL_Bezier.FlexibleGetDirectionControlPosOfTransform<List<Transform>> GetBackwardControlPosFromTransformsList_preAllocated = UtilitiesDXXL_Bezier.GetBackwardControlPosFromTransformsList;
        public static void BezierSpline(List<Transform> points, Color color = default(Color), BezierPosInterpretation interpretationOfList = BezierPosInterpretation.onlySegmentStartPoints_backwardForwardIsAligned, string text = null, float width = 0.0f, bool closeGapFromEndToStart = false, int straightSubDivisionsPerSegment = 50, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            UtilitiesDXXL_Bezier.BezierSpline<List<Transform>>(false, 0.0f, points, GetPositionsFromTransformsList_preAllocated, GetForwardControlPosFromTransformsList_preAllocated, GetBackwardControlPosFromTransformsList_preAllocated, points.Count, color, interpretationOfList, text, width, closeGapFromEndToStart, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
        }


        static UtilitiesDXXL_Bezier.FlexibleGetPosAtIndex<Vector3[]> GetPositionsFromVector3Array_preAllocated = UtilitiesDXXL_Bezier.GetPositionsFromVector3Array;
        public static void BezierSpline(Vector3[] points, Color color = default(Color), BezierPosInterpretation interpretationOfArray = BezierPosInterpretation.start_control1_control2_endIsNextStart, string text = null, float width = 0.0f, bool closeGapFromEndToStart = false, int straightSubDivisionsPerSegment = 50, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            //"interpretationOfArray": only "start_control1_control2_endIsNextStart" and "start_control1_endIsNextStart" are supported for the "BezierSpline" overloads that supply the spline positions as "Vector3". For the other "BezierPosInterpretation": Use a function overload that takes "Transform" or "GameObject"
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            if (interpretationOfArray == BezierPosInterpretation.start_control1_control2_endIsNextStart || interpretationOfArray == BezierPosInterpretation.start_control1_endIsNextStart)
            {
                UtilitiesDXXL_Bezier.BezierSpline<Vector3[]>(false, 0.0f, points, GetPositionsFromVector3Array_preAllocated, null, null, points.Length, color, interpretationOfArray, text, width, closeGapFromEndToStart, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                Debug.LogError("The specified interpretationOfArray ('" + interpretationOfArray + "') is not supported for BezierSpline() function overloads that take 'Vector3'-collections. You may choose an overload that takes 'Transform'- or 'GameObject'-collections.");
            }
        }

        static UtilitiesDXXL_Bezier.FlexibleGetPosAtIndex<List<Vector3>> GetPositionsFromVector3List_preAllocated = UtilitiesDXXL_Bezier.GetPositionsFromVector3List;
        public static void BezierSpline(List<Vector3> points, Color color = default(Color), BezierPosInterpretation interpretationOfList = BezierPosInterpretation.start_control1_control2_endIsNextStart, string text = null, float width = 0.0f, bool closeGapFromEndToStart = false, int straightSubDivisionsPerSegment = 50, float textSize = 0.1f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            //"interpretationOfArray": only "start_control1_control2_endIsNextStart" and "start_control1_endIsNextStart" are supported for the "BezierSpline" overloads that supply the spline positions as "Vector3". For the other "BezierPosInterpretation": Use a function overload that takes "Transform" or "GameObject"
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(points, "points")) { return; }
            if (interpretationOfList == BezierPosInterpretation.start_control1_control2_endIsNextStart || interpretationOfList == BezierPosInterpretation.start_control1_endIsNextStart)
            {
                UtilitiesDXXL_Bezier.BezierSpline<List<Vector3>>(false, 0.0f, points, GetPositionsFromVector3List_preAllocated, null, null, points.Count, color, interpretationOfList, text, width, closeGapFromEndToStart, straightSubDivisionsPerSegment, textSize, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                Debug.LogError("The specified interpretationOfList ('" + interpretationOfList + "') is not supported for BezierSpline() function overloads that take 'Vector3'-collections. You may choose an overload that takes 'Transform'- or 'GameObject'-collections.");
            }
        }

        public static int GetNumberOfDrawnLinesSinceCycleStart()
        {
            return DXXLWrapperForUntiysBuildInDrawLines.DrawnLinesSinceFrameStart;
        }

        public static void ToggleGlobalOverwriteFor_durationInSec(bool globalOverwriteIsEnabled, float valueOf_durationInSec_thatShouldAlwaysBeEnforced = 0.0f)
        {
            DXXLWrapperForUntiysBuildInDrawLines.ToggleGlobalOverwriteFor_durationInSec(globalOverwriteIsEnabled, valueOf_durationInSec_thatShouldAlwaysBeEnforced);
        }

        public static void ToggleGlobalOverwriteFor_hiddenByNearerObjects(bool globalOverwriteIsEnabled, bool valueOf_hiddenByNearerObjects_thatShouldAlwaysBeEnforced = true)
        {
            DXXLWrapperForUntiysBuildInDrawLines.ToggleGlobalOverwriteFor_hiddenByNearerObjects(globalOverwriteIsEnabled, valueOf_hiddenByNearerObjects_thatShouldAlwaysBeEnforced);
        }

        public static float StylePatternScaleFactor_screenspaceToWorldspace(Vector3 positionOfLineStart, Vector3 positionOfLineEnd, float stylePatternScaleFactor_inScreenspace = 1.0f, Camera screenspaceDefiningCamera = null)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(positionOfLineStart, "positionOfLineStart")) { return 1.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(positionOfLineEnd, "positionOfLineEnd")) { return 1.0f; }

            Vector3 centerOfLine = 0.5f * (positionOfLineStart + positionOfLineEnd);
            return StylePatternScaleFactor_screenspaceToWorldspace(centerOfLine, stylePatternScaleFactor_inScreenspace, screenspaceDefiningCamera);
        }

        public static float StylePatternScaleFactor_screenspaceToWorldspace(Vector3 positionOfStyledObject, float stylePatternScaleFactor_inScreenspace = 1.0f, Camera screenspaceDefiningCamera = null)
        {
            //Many draw functions (e.g. like "DrawBasics.Line()"(link) have a "stylePatternScaleFactor" paramter, which scales the line style pattern, so different camera distances can be served with a recognizable pattern. This function can be used to generate "stylePatternScaleFactors" with which the line styles always appear with a constant screenspace pattern size. Just fill the return value of this function into the "stylePatternScaleFactor" parameter of the other drawing functions.
            //The downside of fixed screenspace size patterns is, that if you change the camera position so that a line comes closer or gets farer, then the pattern is not "mounted/fixed" at the line start and end position, since the line changes its length on the screen, but the pattern remains with constant size in the screen. This can compromise the viewers intuitive perception of the line as an object in 3D space.
            //The function is only a rough approximation, and the returned values may have a notable error span when "positionOfStyledObject" is more and more in a screen corner or for certain distances from the camera.
            //"stylePatternScaleFactor_inScreenspace": scales the pattern inside the screenspace
            //"screenspaceDefiningCamera": if it is not specified then "DrawScreenspace.defaultCameraForDrawing" or "DrawScreenspace.defaultScreenspaceWindowForDrawing" is used as fallback.

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(positionOfStyledObject, "positionOfStyledObject")) { return 1.0f; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(stylePatternScaleFactor_inScreenspace, "stylePatternScaleFactor_inScreenspace")) { return 1.0f; }

            stylePatternScaleFactor_inScreenspace = Mathf.Abs(stylePatternScaleFactor_inScreenspace);
            stylePatternScaleFactor_inScreenspace = Mathf.Max(stylePatternScaleFactor_inScreenspace, 0.01f);

            Camera usedCamera;
            if (screenspaceDefiningCamera == null)
            {
                if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out usedCamera, "DrawBasics.ScreenspaceStylePatternScaleFactor_to_WorldspaceStylePatternScaleFactor") == false) { return 1.0f; }
            }
            else
            {
                usedCamera = screenspaceDefiningCamera;
            }

            float distanceToCamera = (positionOfStyledObject - usedCamera.transform.position).magnitude;
            //float diagonalExtentOfViewport_at_distanceFromCam = UtilitiesDXXL_Screenspace.Get_diagonalExtentOfViewport_at_distanceFromCam(usedCamera, distanceToCamera);
            float diagonalExtentOfViewport_at_distanceFromCam = UtilitiesDXXL_Screenspace.Get_vertExtentOfViewport_at_distanceFromCam(usedCamera, distanceToCamera);
            float stylePatternScaleFactor_inWorldspace = diagonalExtentOfViewport_at_distanceFromCam * stylePatternScaleFactor_inScreenspace;
            stylePatternScaleFactor_inWorldspace = Mathf.Max(stylePatternScaleFactor_inWorldspace, UtilitiesDXXL_LineStyles.minStylePatternScaleFactor);
            return stylePatternScaleFactor_inWorldspace;
        }

    }

}





