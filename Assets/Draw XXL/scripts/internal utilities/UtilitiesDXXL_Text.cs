namespace DrawXXL
{
    using System;
    using UnityEngine;
    using System.Collections.Generic;

    public class UtilitiesDXXL_Text
    {
        public static float relLineDistance = 1.6f;
        public static float minTextSize = 0.00001f;
        public static int maxRelStrokeWidth_inPPMofSize = 240000;
        static float minRelBoldStrokeWidth = 0.16f;
        static float maxRelBoldStrokeWidth = 0.36f;
        static float italicIntensity = 0.6f;
        static float minRadius = 0.0001f;
        static Vector3 duplicateTriangle_LeftLeftUp_normalized = new Vector3(-0.8660254f, 0.5f, 0.0f);
        static Vector3 duplicateTriangle_RightRightUp_normalized = new Vector3(0.8660254f, 0.5f, 0.0f);
        static Vector3 duplicateTriangle_LeftLeftDown_normalized = new Vector3(-0.8660254f, -0.5f, 0.0f);
        static Vector3 duplicateTriangle_RightRightDown_normalized = new Vector3(0.8660254f, -0.5f, 0.0f);
        static Vector3 duplicateTriangle_LeftUpUp_normalized = new Vector3(-0.5f, 0.8660254f, 0.0f);
        static Vector3 duplicateTriangle_RightUpUp_normalized = new Vector3(0.5f, 0.8660254f, 0.0f);
        static Vector3 duplicateTriangle_LeftDownDown_normalized = new Vector3(-0.5f, -0.8660254f, 0.0f);
        static Vector3 duplicateTriangle_RightDownDown_normalized = new Vector3(0.5f, -0.8660254f, 0.0f);

        //0/1 markups:
        public static string lineBreakMarkupString = "<br>";
        public static string boldStartMarkupString = "<b>";
        public static string boldEndMarkupString = "</b>";
        public static string italicStartMarkupString = "<i>";
        public static string italicEndMarkupString = "</i>";
        public static string deletedStartMarkupString = "<d>";
        public static string deletedEndMarkupString = "</d>";
        public static string underlinedStartMarkupString = "<u>";
        public static string underlinedEndMarkupString = "</u>";
        //value markups:
        public static string strokeWidthStartMarkupString_preValue = "<sw="; //"sw" = "(rel) Stroke Width" "in ppm (parts-per-million) der char-size"
        public static string strokeWidthEndMarkupString = "</sw>";
        public static string sizeStartMarkupString_preValue = "<size=";  //"size=11" seems to be the size that the unity console logs use. This is probably measured in pixels.
        public static string sizeEndMarkupString = "</size>";
        public static string colorStartMarkupString_preValue = "<color=";
        public static string colorEndMarkupString = "</color>";
        public static string iconMarkupString_preValue = "<icon=";
        public static string valueMarkupString_postValue = ">";

        //saving GC.Alloc:
        static List<InternalDXXL_CharConfig> chars = new List<InternalDXXL_CharConfig>();
        static InternalDXXL_CharConfig autoLineBreakChar = new InternalDXXL_CharConfig();
        static int usedCharConfigListSlots;
        static List<InternalDXXL_MarkupPhase> relStrokeWidthMarkupPhases = new List<InternalDXXL_MarkupPhase>();
        static List<InternalDXXL_MarkupPhase> sizeMarkupPhases = new List<InternalDXXL_MarkupPhase>();
        static List<InternalDXXL_MarkupPhase> colorMarkupPhases = new List<InternalDXXL_MarkupPhase>();
        static InternalDXXL_CharConfig.SetCharStyleProperty MarkAsBold_preAllocated = InternalDXXL_CharConfig.MarkAsBold;
        static InternalDXXL_CharConfig.SetCharStyleProperty MarkAsItalic_preAllocated = InternalDXXL_CharConfig.MarkAsItalic;
        static InternalDXXL_CharConfig.SetCharStyleProperty MarkAsDeleted_preAllocated = InternalDXXL_CharConfig.MarkAsDeleted;
        static InternalDXXL_CharConfig.SetCharStyleProperty MarkAsUnderlined_preAllocated = InternalDXXL_CharConfig.MarkAsUnderlined;


        public static void WriteFramed(string text, Vector3 position, Color color, float size, Quaternion rotation, DrawText.TextAnchorDXXL textAnchor, DrawBasics.LineStyle enclosingBoxLineStyle, float enclosingBox_lineWidth_relToTextSize, float enclosingBox_paddingSize_relToTextSize, float forceTextBlockEnlargementToThisMinWidth, float forceRestrictTextBlockSizeToThisMaxTextWidth, float autoLineBreakWidth, bool autoFlipToPreventMirrorInverted, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            bool rotationIsValid = UtilitiesDXXL_TextDirAndUpCalculation.ConvertQuaternionToTextDirAndUpVectors(out Vector3 textDir, out Vector3 textUp, rotation);
            bool skipDraw = false; //-> The called method has (almost) the same parameters and can be used interchangeably, except that it has the additional parameter "skipDraw". This can be set to 'true' if only the 'parsedTextSpecs' should be filled (e.g. as decision base for the final placement of the text via an additinal DrawText-call).
            Write(text, position, color, size, textDir, textUp, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth, forceRestrictTextBlockSizeToThisMaxTextWidth, autoLineBreakWidth, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, skipDraw, false, rotationIsValid);
        }

        public static void WriteFramed(string text, Vector3 position, Color color, float size, Vector3 textDirection, Vector3 textUp, DrawText.TextAnchorDXXL textAnchor, DrawBasics.LineStyle enclosingBoxLineStyle, float enclosingBox_lineWidth_relToTextSize, float enclosingBox_paddingSize_relToTextSize, float forceTextBlockEnlargementToThisMinWidth, float forceRestrictTextBlockSizeToThisMaxTextWidth, float autoLineBreakWidth, bool autoFlipToPreventMirrorInverted, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            bool skipDraw = false; //-> The called method has the same parameters and can be used interchangeably, except that it has the additional parameter "skipDraw". This can be set to 'true' if only the 'parsedTextSpecs' should be filled (e.g. as decision base for the final placement of the text via an additinal DrawText-call).
            Write(text, position, color, size, textDirection, textUp, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth, forceRestrictTextBlockSizeToThisMaxTextWidth, autoLineBreakWidth, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, skipDraw, false, false);
        }

        public static void Write2DFramed(string text, Vector2 position, Color color, float size, Vector2 textDirection, DrawText.TextAnchorDXXL textAnchor, float custom_zPos, DrawBasics.LineStyle enclosingBoxLineStyle, float enclosingBox_lineWidth_relToTextSize, float enclosingBox_paddingSize_relToTextSize, float forceTextBlockEnlargementToThisMinWidth, float forceRestrictTextBlockSizeToThisMaxTextWidth, float autoLineBreakWidth, bool autoFlipToPreventMirrorInverted, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 positionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(position, zPos);
            Vector3 textDirV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(textDirection);
            Vector3 textUpV3 = Vector3.Cross(Vector3.forward, textDirV3);

            bool skipDraw = false; //-> "skipDraw" can be set to 'true' if only the 'parsedTextSpecs' should be filled (e.g. as decision base for the final placement of the text via an additinal DrawText-call).
            Write(text, positionV3, color, size, textDirV3, textUpV3, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth, forceRestrictTextBlockSizeToThisMaxTextWidth, autoLineBreakWidth, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, skipDraw, true, false);
        }

        public static void Write2DFramed(string text, Vector2 position, Color color, float size, float zRotationDegCC, DrawText.TextAnchorDXXL textAnchor, float custom_zPos, DrawBasics.LineStyle enclosingBoxLineStyle, float enclosingBox_lineWidth_relToTextSize, float enclosingBox_paddingSize_relToTextSize, float forceTextBlockEnlargementToThisMinWidth, float forceRestrictTextBlockSizeToThisMaxTextWidth, float autoLineBreakWidth, bool autoFlipToPreventMirrorInverted, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 positionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(position, zPos);
            Quaternion rotation = UtilitiesDXXL_DrawBasics2D.QuaternionFromAngle(zRotationDegCC);
            Vector3 textDirV3 = rotation * Vector3.right;
            Vector3 textUpV3 = Vector3.Cross(Vector3.forward, textDirV3);

            bool skipDraw = false; //-> "skipDraw" can be set to 'true' if only the 'parsedTextSpecs' should be filled (e.g. as decision base for the final placement of the text via an additinal DrawText-call).
            Write(text, positionV3, color, size, textDirV3, textUpV3, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth, forceRestrictTextBlockSizeToThisMaxTextWidth, autoLineBreakWidth, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, skipDraw, true, true);
        }

        public static void WriteScreenSpace(Camera camera, string text, Vector2 position, Color color, float size_relToViewportHeight, Vector2 textDirection, DrawText.TextAnchorDXXL textAnchor, DrawBasics.LineStyle enclosingBoxLineStyle, float enclosingBox_lineWidth_relToTextSize, float enclosingBox_paddingSize_relToTextSize, float forceTextEnlargementToThisMinWidth_relToViewportWidth, float forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth, bool autoLineBreakAtScreenBorder, float autoLineBreakWidth_relToViewportWidth, bool autoFlipTextToPreventUpsideDown, float durationInSec, bool skipDraw)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(textDirection, "textDirection")) { return; }

            if (InternalDXXL_BoundsCamViewportSpace.IsOutsideViewportWithPadding(position, 6.0f)) { return; }
            if (InternalDXXL_BoundsCamViewportSpace.IsOutsideViewportXWithPadding(position, 2.0f) && InternalDXXL_BoundsCamViewportSpace.IsOutsideViewportYWithPadding(position, 2.0f)) { return; }

            if (UtilitiesDXXL_Math.ApproximatelyZero(textDirection))
            {
                textDirection = Vector3.right;
            }

            float zRotationDegCC = Vector2.Angle(Vector2.right, textDirection);
            if (textDirection.y < 0.0f)
            {
                zRotationDegCC = -zRotationDegCC;
            }
            WriteScreenspace(camera, text, position, color, size_relToViewportHeight, zRotationDegCC, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtScreenBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec, skipDraw);
        }

        public static void WriteScreenspace(Camera camera, string text, Vector2 position, Color color, float size_relToViewportHeight, float zRotationDegCC, DrawText.TextAnchorDXXL textAnchor, DrawBasics.LineStyle enclosingBoxLineStyle, float enclosingBox_lineWidth_relToTextSize, float enclosingBox_paddingSize_relToTextSize, float forceTextEnlargementToThisMinWidth_relToViewportWidth, float forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth, bool autoLineBreakAtScreenBorder, float autoLineBreakWidth_relToViewportWidth, bool autoFlipTextToPreventUpsideDown, float durationInSec, bool skipDraw)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(camera, "camera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(camera)) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(size_relToViewportHeight, "size_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(zRotationDegCC, "zRotationDegCC")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(forceTextEnlargementToThisMinWidth_relToViewportWidth, "forceTextEnlargementToThisMinWidth_relToViewportWidth")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth, "forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(autoLineBreakWidth_relToViewportWidth, "autoLineBreakWidth_relToViewportWidth")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }

            if (InternalDXXL_BoundsCamViewportSpace.IsOutsideViewportWithPadding(position, 6.0f)) { return; }
            if (InternalDXXL_BoundsCamViewportSpace.IsOutsideViewportXWithPadding(position, 2.0f) && InternalDXXL_BoundsCamViewportSpace.IsOutsideViewportYWithPadding(position, 2.0f)) { return; }

            Vector3 pos_worldSpace = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(camera, position, false);
            float size_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, position, false, size_relToViewportHeight);

            float forceTextEnlargementToThisMinWidth_worldSpace = 0.0f;
            if (UtilitiesDXXL_Math.ApproximatelyZero(forceTextEnlargementToThisMinWidth_relToViewportWidth) == false)
            {
                forceTextEnlargementToThisMinWidth_worldSpace = UtilitiesDXXL_Screenspace.HorizExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, position, false, forceTextEnlargementToThisMinWidth_relToViewportWidth);
            }

            float forceRestrictTextSizeToThisMaxTextWidth_worldSpace = 0.0f;
            if (UtilitiesDXXL_Math.ApproximatelyZero(forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth) == false)
            {
                forceRestrictTextSizeToThisMaxTextWidth_worldSpace = UtilitiesDXXL_Screenspace.HorizExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, position, false, forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth);
            }

            autoLineBreakWidth_relToViewportWidth = GetCapped_autoLineBreakWidth_relToViewportWidth(camera, autoLineBreakAtScreenBorder, autoLineBreakWidth_relToViewportWidth, position, textAnchor, zRotationDegCC);
            float autoLineBreakWidth_worldSpace = 0.0f;
            if (UtilitiesDXXL_Math.ApproximatelyZero(autoLineBreakWidth_relToViewportWidth) == false)
            {
                autoLineBreakWidth_worldSpace = UtilitiesDXXL_Screenspace.HorizExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, position, false, autoLineBreakWidth_relToViewportWidth);
            }

            Quaternion rotation_aroundCamForward = Quaternion.AngleAxis(zRotationDegCC, camera.transform.forward);
            Vector3 textDir_worldSpace_normalized = rotation_aroundCamForward * camera.transform.right;
            Vector3 textUp_worldSpace_normalized = Vector3.Cross(camera.transform.forward, textDir_worldSpace_normalized);

            enclosingBoxLineStyle = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(enclosingBoxLineStyle);
            UtilitiesDXXL_TextDirAndUpCalculation.TryAutoFlipScreenspaceTextToPreventUpsideDown(out DrawText.TextAnchorDXXL textAnchor_postFlip, out Vector3 textDir_worldSpace_normalized_postFlip, out Vector3 textUp_worldSpace_normalized_postFlip, textAnchor, textDir_worldSpace_normalized, textUp_worldSpace_normalized, autoFlipTextToPreventUpsideDown, camera);

            Write(text, pos_worldSpace, color, size_worldSpace, textDir_worldSpace_normalized_postFlip, textUp_worldSpace_normalized_postFlip, textAnchor_postFlip, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextEnlargementToThisMinWidth_worldSpace, forceRestrictTextSizeToThisMaxTextWidth_worldSpace, autoLineBreakWidth_worldSpace, false, durationInSec, false, skipDraw, false, true);
            ConvertParsedSpecs_toViewportSpace(camera, position, pos_worldSpace, textDir_worldSpace_normalized, textUp_worldSpace_normalized);
        }

        public static void Write(string text, Vector3 position, Color color, float size, Vector3 textDirection, Vector3 textUp, DrawText.TextAnchorDXXL textAnchor, DrawBasics.LineStyle enclosingBoxLineStyle, float enclosingBox_lineWidth_relToTextSize, float enclosingBox_paddingSize_relToTextSize, float forceTextEnlargementToThisMinWidth, float forceRestrictTextSizeToThisMaxTextWidth, float autoLineBreakWidth, bool autoFlipToPreventMirrorInverted, float durationInSec, bool hiddenByNearerObjects, bool skipDraw, bool isFrom_Write2D, bool dirAndUp_areAlreadyGuaranteed_perpAndNormalized)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

            DrawText.parsedTextSpecs.widthOfLongestLine = 0.0f;
            DrawText.parsedTextSpecs.numberOfChars_inLongestLine = 0;
            DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine = 0.0f;
            DrawText.parsedTextSpecs.height_wholeTextBlock = 0.0f;
            DrawText.parsedTextSpecs.height_lowFirstLine_toLowLastLine = 0.0f;
            DrawText.parsedTextSpecs.lowLeftPos_ofFirstLine = default;
            DrawText.parsedTextSpecs.used_textDirection_normalized = default;
            DrawText.parsedTextSpecs.used_textUp_normalized = default;
            DrawText.parsedTextSpecs.usedTextAnchor = textAnchor;
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(size, "size")) { return; }
            DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine = size;

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(enclosingBox_lineWidth_relToTextSize, "enclosingBox_lineWidth_relToTextSize")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(enclosingBox_paddingSize_relToTextSize, "enclosingBox_paddingSize_relToTextSize")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(forceTextEnlargementToThisMinWidth, "forceTextEnlargementToThisMinWidth")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(forceRestrictTextSizeToThisMaxTextWidth, "forceRestrictTextSizeToThisMaxTextWidth")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(autoLineBreakWidth, "autoLineBreakWidth")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }
            DrawText.parsedTextSpecs.lowLeftPos_ofFirstLine = position;
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(textDirection, "textDirection")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(textUp, "textUp")) { return; }

            if (text == null)
            {
                Debug.Log("Draw XXL: 'Write' (and parsing) is skipped, because 'text' is 'null'.");
                return;
            }

            if (text.Length == 0)
            {
                Debug.Log("Draw XXL: 'Write' (and parsing) is skipped, because 'text' has zero characters.");
                return;
            }

            size = UtilitiesDXXL_Math.AbsNonZeroValue(size);
            if (size < minTextSize)
            {
                //preventing undefined behaviour in the region of calculation errors of very small float values.
                //DO NOT fallback to "Point()" here, because "Point()" calls "Write()" again, which can create an endless loop.
                Debug.Log("Draw XXL: 'Write' (and parsing) is skipped, because 'size' (" + size + ") is too small. The minimum text size is " + minTextSize + ".");
                return;
            }

            color = UtilitiesDXXL_Colors.OverwriteDefaultColor(color);
            forceTextEnlargementToThisMinWidth = UtilitiesDXXL_Math.AbsNonZeroValue(forceTextEnlargementToThisMinWidth);
            forceRestrictTextSizeToThisMaxTextWidth = UtilitiesDXXL_Math.AbsNonZeroValue(forceRestrictTextSizeToThisMaxTextWidth);

            CreateCharConfigs(text, color, size);
            InsertLineBreaks_fromRichTextMarkups(text);
            SwitchTextStyle(text, boldStartMarkupString, boldEndMarkupString, MarkAsBold_preAllocated);
            SwitchTextStyle(text, italicStartMarkupString, italicEndMarkupString, MarkAsItalic_preAllocated);
            SwitchTextStyle(text, deletedStartMarkupString, deletedEndMarkupString, MarkAsDeleted_preAllocated);
            SwitchTextStyle(text, underlinedStartMarkupString, underlinedEndMarkupString, MarkAsUnderlined_preAllocated);

            UtilitiesDXXL_TextDirAndUpCalculation.GetTextDirAndUpNormalized(out Vector3 textDirNormalized_preFlip, out Vector3 textUpNormalized_preFlip, textDirection, textUp, position, isFrom_Write2D, dirAndUp_areAlreadyGuaranteed_perpAndNormalized);
            Vector3 forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip = Vector3.Cross(textDirNormalized_preFlip, textUpNormalized_preFlip);
            Vector3 forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip;
            UtilitiesDXXL_TextDirAndUpCalculation.TryAutoFlipStraightTextToPreventMirrorInverted(out textAnchor, out Vector3 textDirNormalized_postFlip, out Vector3 textUpNormalized_postFlip, out forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip, textAnchor, textDirNormalized_preFlip, textUpNormalized_preFlip, forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip, position, autoFlipToPreventMirrorInverted);
            DrawText.parsedTextSpecs.used_textDirection_normalized = textDirNormalized_postFlip;
            DrawText.parsedTextSpecs.used_textUp_normalized = textUpNormalized_postFlip;

            int usedSlotsInListOf_relStrokeWidthMarkupPhases = GetMarkupPhases(ref relStrokeWidthMarkupPhases, text, strokeWidthStartMarkupString_preValue, strokeWidthEndMarkupString);
            Quaternion rotationOfChars = Quaternion.LookRotation(forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip, textUpNormalized_postFlip);
            AssignStrokeWidthOffsets(usedSlotsInListOf_relStrokeWidthMarkupPhases, size, rotationOfChars, skipDraw);
            int usedSlotsInListOf_sizeMarkupPhases = GetMarkupPhases(ref sizeMarkupPhases, text, sizeStartMarkupString_preValue, sizeEndMarkupString);
            ScaleSize_perChar(usedSlotsInListOf_sizeMarkupPhases, size);
            int usedSlotsInListOf_colorMarkupPhases = GetMarkupPhases(ref colorMarkupPhases, text, colorStartMarkupString_preValue, colorEndMarkupString);
            ApplyColor(usedSlotsInListOf_colorMarkupPhases, skipDraw);
            InsertIcons(text);
            InsertLineBreaks_fromEscapedUnicodeChars();

            if (GetNumberOfNonStrippedChars() <= 0)
            {
                Debug.Log("Draw XXL: 'Write' is skipped, because there are no chars left after parsing. Unparsed text: " + text);
                return;
            }

            InsertLineBreaks_fromMaxTextBlockWidthParameter(autoLineBreakWidth, size);
            FillParsedTextSpecs();

            float scaleFactorFromForceWholeTextBlockWidth = ForceTextWidth(forceTextEnlargementToThisMinWidth, forceRestrictTextSizeToThisMaxTextWidth);
            DrawText.parsedTextSpecs.lowLeftPos_ofFirstLine = GetLowLeftPosOfFirstLine(position, textAnchor, textDirNormalized_postFlip, textUpNormalized_postFlip);
            DrawEncapsulatingBox(skipDraw, size, scaleFactorFromForceWholeTextBlockWidth, color, textDirNormalized_postFlip, textUpNormalized_postFlip, enclosingBoxLineStyle, enclosingBox_paddingSize_relToTextSize, enclosingBox_lineWidth_relToTextSize, durationInSec, hiddenByNearerObjects);
            if (skipDraw) { return; }
            PrintChars(DrawText.parsedTextSpecs.lowLeftPos_ofFirstLine, textDirNormalized_postFlip, textUpNormalized_postFlip, rotationOfChars, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteOnCircleScreenspace(Camera camera, string text, Vector2 circleCenterPosition, float radius_relToViewportHeight, Color color, float size_relToViewportHeight, Vector2 textsInitialUp, DrawText.TextAnchorCircledDXXL textAnchor, float autoLineBreakAngleDeg, float durationInSec, bool skipDraw)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(textsInitialUp, "textsInitialUp")) { return; }

            if (UtilitiesDXXL_Math.ApproximatelyZero(textsInitialUp))
            {
                textsInitialUp = Vector3.up;
            }

            float initial_zRotationDeg_fromCamUp = Vector2.Angle(Vector2.up, textsInitialUp);
            if (textsInitialUp.x > 0.0f)
            {
                initial_zRotationDeg_fromCamUp = -initial_zRotationDeg_fromCamUp;
            }

            WriteOnCircleScreenspace(camera, text, circleCenterPosition, radius_relToViewportHeight, color, size_relToViewportHeight, initial_zRotationDeg_fromCamUp, textAnchor, autoLineBreakAngleDeg, durationInSec, skipDraw);
        }

        public static void WriteOnCircleScreenspace(Camera camera, string text, Vector2 circleCenterPosition, float radius_relToViewportHeight, Color color, float size_relToViewportHeight, float initialTextDir_as_zRotationDegCCfromCamUp, DrawText.TextAnchorCircledDXXL textAnchor, float autoLineBreakAngleDeg, float durationInSec, bool skipDraw)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(camera, "camera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(camera)) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius_relToViewportHeight, "radius_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(size_relToViewportHeight, "size_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(initialTextDir_as_zRotationDegCCfromCamUp, "initialTextDir_as_zRotationDegCCfromCamUp")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenterPosition, "circleCenterPosition")) { return; }

            Quaternion rotation_toInitialUp_inAspectCorrected1by1SquareViewportSpace = Quaternion.AngleAxis(initialTextDir_as_zRotationDegCCfromCamUp, Vector3.forward);
            Vector2 initialUp_inAspectCorrected1by1SquareViewportSpace_normalized = rotation_toInitialUp_inAspectCorrected1by1SquareViewportSpace * Vector2.up;
            //"approx", because radius is always used as "_relToViewportHeight" instead of correcting for non-uniform (non-1by1) viewport rects:
            Vector2 approxStartPos_inAspectCorrected1by1SquareViewportSpace = circleCenterPosition + initialUp_inAspectCorrected1by1SquareViewportSpace_normalized * radius_relToViewportHeight;
            Quaternion initialUpsRotation_aroundCamForward_worldSpace = Quaternion.AngleAxis(initialTextDir_as_zRotationDegCCfromCamUp, camera.transform.forward);
            Vector3 textsInitialUp_worldSpace_normalized = initialUpsRotation_aroundCamForward_worldSpace * camera.transform.up;
            Vector3 textsInitialDir_worldSpace_normalized = Vector3.Cross(textsInitialUp_worldSpace_normalized, camera.transform.forward);
            float size_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, approxStartPos_inAspectCorrected1by1SquareViewportSpace, false, size_relToViewportHeight);
            float radius_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(camera, approxStartPos_inAspectCorrected1by1SquareViewportSpace, false, radius_relToViewportHeight);
            Vector3 circleCenterPosition_worldSpace = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(camera, circleCenterPosition, false);
            WriteOnCircle(text, circleCenterPosition_worldSpace, radius_worldSpace, color, size_worldSpace, textsInitialDir_worldSpace_normalized, textsInitialUp_worldSpace_normalized, textAnchor, autoLineBreakAngleDeg, false, durationInSec, false, skipDraw, false, true);
            ConvertParsedSpecsOnCircle_toViewportSpace(camera, circleCenterPosition_worldSpace, textsInitialUp_worldSpace_normalized, radius_worldSpace, approxStartPos_inAspectCorrected1by1SquareViewportSpace);
        }

        public static void WriteOnCircle(string text, Vector3 textStartPos, Vector3 circleCenterPosition, Vector3 turnAxis_direction = default(Vector3), Color color = default(Color), float size = 0.1f, DrawText.TextAnchorCircledDXXL textAnchor = DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, float autoLineBreakAngleDeg = 0.0f, bool autoFlipToPreventMirrorInverted = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(textStartPos, "textStartPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenterPosition, "circleCenterPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(turnAxis_direction, "turnAxis_direction")) { return; }

            Vector3 textsInitialUp = textStartPos - circleCenterPosition;
            Vector3 textsInitialUp_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(textsInitialUp, out float radius); //-> normalizing is actually not necessary here, but is also no problem, since length/magnitude is needed for the radius anyway
            Vector3 textsInitialDir = Vector3.Cross(textsInitialUp_normalized, turnAxis_direction);
            textsInitialDir = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(textsInitialDir);

            bool skipDraw = false; //-> The called method has (almost) the same parameters and can be used interchangeably, except that it has the additional parameter "skipDraw". This can be set to 'true' if only the 'parsedTextSpecs' should be filled (e.g. as decision base for the final placement of the text via an additinal DrawText-call).
            WriteOnCircle(text, circleCenterPosition, radius, color, size, textsInitialDir, textsInitialUp_normalized, textAnchor, autoLineBreakAngleDeg, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, skipDraw, false, false);
        }

        public static void WriteOnCircle(string text, Vector3 circleCenterPosition, float radius, Color color, float size, Vector3 textsInitialDir, Vector3 textsInitialUp, DrawText.TextAnchorCircledDXXL textAnchor, float autoLineBreakAngleDeg, bool autoFlipToPreventMirrorInverted, float durationInSec, bool hiddenByNearerObjects, bool skipDraw, bool isFrom_Write2D, bool dirAndUp_areAlreadyGuaranteed_perpAndNormalized)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

            DrawText.parsedTextOnCircleSpecs.angleDegOfLongestLine = 0.0f;
            DrawText.parsedTextOnCircleSpecs.numberOfChars_inLongestLine = 0;
            DrawText.parsedTextOnCircleSpecs.numberOfChars_afterParsingOutTheMarkupTags = 0;
            DrawText.parsedTextOnCircleSpecs.sizeOfBiggestCharInFirstLine = 0.0f;
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(size, "size")) { return; }
            DrawText.parsedTextOnCircleSpecs.sizeOfBiggestCharInFirstLine = size;

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(autoLineBreakAngleDeg, "autoLineBreakAngleDeg")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenterPosition, "circleCenterPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(textsInitialDir, "textsInitialDir")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(textsInitialUp, "textsInitialUp")) { return; }

            if (text == null)
            {
                Debug.Log("Draw XXL: 'WriteOnCircle' (and parsing) is skipped, because 'text' is 'null'.");
                return;
            }

            if (text.Length == 0)
            {
                Debug.Log("Draw XXL: 'WriteOnCircle' (and parsing) is skipped, because 'text' has zero characters.");
                return;
            }

            size = UtilitiesDXXL_Math.AbsNonZeroValue(size);
            if (size < 0.00001f)
            {
                //preventing undefined behaviour in the region of calculation errors of very small float values.
                Debug.LogWarning("Draw XXL: 'WriteOnCircle' is skipped, because 'size' (" + size + ") is too small.");
                return;
            }

            color = UtilitiesDXXL_Colors.OverwriteDefaultColor(color);
            radius = Mathf.Abs(radius);
            radius = Mathf.Max(radius, minRadius);

            CreateCharConfigs(text, color, size);
            InsertLineBreaks_fromRichTextMarkups(text);
            SwitchTextStyle(text, boldStartMarkupString, boldEndMarkupString, MarkAsBold_preAllocated);
            SwitchTextStyle(text, italicStartMarkupString, italicEndMarkupString, MarkAsItalic_preAllocated);
            SwitchTextStyle(text, deletedStartMarkupString, deletedEndMarkupString, MarkAsDeleted_preAllocated);
            SwitchTextStyle(text, underlinedStartMarkupString, underlinedEndMarkupString, MarkAsUnderlined_preAllocated);

            int usedSlotsInListOf_relStrokeWidthMarkupPhases = GetMarkupPhases(ref relStrokeWidthMarkupPhases, text, strokeWidthStartMarkupString_preValue, strokeWidthEndMarkupString);
            int usedSlotsInListOf_sizeMarkupPhases = GetMarkupPhases(ref sizeMarkupPhases, text, sizeStartMarkupString_preValue, sizeEndMarkupString);
            ScaleSize_perChar(usedSlotsInListOf_sizeMarkupPhases, size);
            int usedSlotsInListOf_colorMarkupPhases = GetMarkupPhases(ref colorMarkupPhases, text, colorStartMarkupString_preValue, colorEndMarkupString);
            ApplyColor(usedSlotsInListOf_colorMarkupPhases, skipDraw);
            InsertIcons(text);
            InsertLineBreaks_fromEscapedUnicodeChars();

            if (GetNumberOfNonStrippedChars() <= 0)
            {
                Debug.Log("Draw XXL: 'Write' is skipped, because there are no chars left after parsing. Unparsed text: " + text);
                return;
            }

            Assign_coveredAnglePerChar_onTheLineAtTheReferenceRadius(radius, size);
            radius = TryShiftTheTextToOutwardsOfRadius_soItEndsOnTheCircleInsteadOfStartingThere(textAnchor, autoLineBreakAngleDeg, size, radius);
            InsertLineBreaksOnCircle_fromMaxTextBlockAngleParameter(autoLineBreakAngleDeg, size, radius);
            Assign_coveredAnglePerChar_onTheCharsOwnLine(radius, size);
            FillParsedTextOnCircleSpecs();
            if (skipDraw) { return; }

            UtilitiesDXXL_TextDirAndUpCalculation.GetTextDirAndUpNormalized(out Vector3 initialTextDirNormalized_preFlip, out Vector3 initialTextUpNormalized_preFlip, textsInitialDir, textsInitialUp, circleCenterPosition, isFrom_Write2D, dirAndUp_areAlreadyGuaranteed_perpAndNormalized);
            Vector3 forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip = Vector3.Cross(initialTextDirNormalized_preFlip, initialTextUpNormalized_preFlip);
            Vector3 forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip;
            UtilitiesDXXL_TextDirAndUpCalculation.TryAutoFlipCircledTextToPreventMirrorInverted(out Vector3 initialTextDirNormalized_postFlip, out Vector3 initialTextUpNormalized_postFlip, out forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip, initialTextDirNormalized_preFlip, initialTextUpNormalized_preFlip, forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_preFlip, circleCenterPosition, autoFlipToPreventMirrorInverted, DrawText.parsedTextOnCircleSpecs.angleDegOfLongestLine);

            AssignIndividualRotation(initialTextUpNormalized_postFlip, forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip);
            Quaternion initialRotationOfChars = Quaternion.LookRotation(forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip, initialTextUpNormalized_postFlip);
            AssignStrokeWidthOffsets(usedSlotsInListOf_relStrokeWidthMarkupPhases, size, initialRotationOfChars, skipDraw);
            TurnStrokeWidthOffsetsOnCircle(); //-> could probably be skipped without big visual disadvantage
            PrintCharsOnCircle(circleCenterPosition, forward_isPerpReaderViewDirThatSeesTextUnmirrored_normalized_postFlip, initialTextUpNormalized_postFlip, radius, durationInSec, hiddenByNearerObjects);
        }

        static void PrintChars(Vector3 position, Vector3 textDirNormalized, Vector3 textUpNormalized, Quaternion rotationOfChars, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 startPosOfCurrLine = position;
            Vector3 lineStart_to_charStart = Vector3.zero;

            for (int i_char = 0; i_char < usedCharConfigListSlots; i_char++)
            {
                if (chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself > 0)
                {
                    float sizeOfBiggestCharInUpcomingLine = GetBiggestCharSizeInsideLine(i_char + chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself);
                    startPosOfCurrLine = startPosOfCurrLine - textUpNormalized * sizeOfBiggestCharInUpcomingLine * relLineDistance;
                    lineStart_to_charStart = Vector3.zero;
                    for (int i_charOfLineBreakString = 1; i_charOfLineBreakString < chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself; i_charOfLineBreakString++)
                    {
                        i_char++;
                    }
                }
                else
                {
                    if (chars[i_char].strippedDueToParsing == false)
                    {
                        chars[i_char].pos = startPosOfCurrLine + lineStart_to_charStart;
                        PrintChar(chars[i_char], rotationOfChars, durationInSec, hiddenByNearerObjects);
                        lineStart_to_charStart = lineStart_to_charStart + chars[i_char].size * textDirNormalized;
                    }
                }
            }
        }

        static void PrintCharsOnCircle(Vector3 circleCenterPosition, Vector3 forward, Vector3 initialTextUpNormalized, float radius, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 circleCenter_to_startPosOfMostOuterLine = initialTextUpNormalized * radius;
            Vector3 circleCenter_to_startPosOfCurrLine = circleCenter_to_startPosOfMostOuterLine;
            float angleDeg_lineStartToCurrChar = 0.0f;

            for (int i_char = 0; i_char < usedCharConfigListSlots; i_char++)
            {
                if (chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself > 0)
                {
                    float sizeOfBiggestCharInUpcomingLine = GetBiggestCharSizeInsideLine(i_char + chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself);
                    Vector3 circleCenter_to_startPosOfPrevLine = circleCenter_to_startPosOfCurrLine;
                    circleCenter_to_startPosOfCurrLine = circleCenter_to_startPosOfCurrLine - initialTextUpNormalized * sizeOfBiggestCharInUpcomingLine * relLineDistance;
                    if (UtilitiesDXXL_Math.Check_ifVectorsPointAwayFromEachOther_perpCountsAsPointingAwayFromEachOther(circleCenter_to_startPosOfPrevLine, circleCenter_to_startPosOfCurrLine))
                    {
                        circleCenter_to_startPosOfCurrLine = circleCenter_to_startPosOfMostOuterLine.normalized * minRadius;
                    }
                    angleDeg_lineStartToCurrChar = 0.0f;
                    for (int i_charOfLineBreakString = 1; i_charOfLineBreakString < chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself; i_charOfLineBreakString++)
                    {
                        i_char++;
                    }
                }
                else
                {
                    if (chars[i_char].strippedDueToParsing == false)
                    {
                        Quaternion rotation = Quaternion.AngleAxis(angleDeg_lineStartToCurrChar, -forward);
                        chars[i_char].pos = circleCenterPosition + rotation * circleCenter_to_startPosOfCurrLine;
                        PrintCharOnCircle(chars[i_char], forward, chars[i_char].charUp, durationInSec, hiddenByNearerObjects);
                        angleDeg_lineStartToCurrChar = angleDeg_lineStartToCurrChar + chars[i_char].coveredAngleDegOnOwnLine;
                    }
                }
            }
        }

        static void PrintChar(InternalDXXL_CharConfig printedChar, Quaternion rotationOfChar, float durationInSec, bool hiddenByNearerObjects)
        {
            UtilitiesDXXL_CharsAndIcons.RefillCurrPrintedCharDef(printedChar, out printedChar.hasMissingSymbolDefinition);
            ShearTowardsItalic(printedChar);

            for (int i = 0; i < DrawXXL_LinesManager.instance.numberOfStrokes_forCurrUsedChar; i++)
            {
                TurnCharDef(ref DrawXXL_LinesManager.instance.currPrinted_charDef[i], rotationOfChar);
            }
            DrawTurnedDistortedCurrCharDef(printedChar, durationInSec, hiddenByNearerObjects);
        }

        static void PrintCharOnCircle(InternalDXXL_CharConfig printedChar, Vector3 forward, Vector3 charUp, float durationInSec, bool hiddenByNearerObjects)
        {
            UtilitiesDXXL_CharsAndIcons.RefillCurrPrintedCharDef(printedChar, out printedChar.hasMissingSymbolDefinition);
            ShearTowardsItalic(printedChar);

            for (int i = 0; i < DrawXXL_LinesManager.instance.numberOfStrokes_forCurrUsedChar; i++)
            {
                TurnCharDef(ref DrawXXL_LinesManager.instance.currPrinted_charDef[i], forward, charUp);
            }
            DrawTurnedDistortedCurrCharDef(printedChar, durationInSec, hiddenByNearerObjects);
        }

        static void DrawTurnedDistortedCurrCharDef(InternalDXXL_CharConfig printedChar, float durationInSec, bool hiddenByNearerObjects)
        {
            for (int i_stroke = 0; i_stroke < DrawXXL_LinesManager.instance.numberOfStrokes_forCurrUsedChar; i_stroke++)
            {
                int linesInsideCurrStroke = DrawXXL_LinesManager.instance.numberOfPointsForEachStroke_forCurrUsedChar[i_stroke] - 1;
                for (int i_point = 0; i_point < linesInsideCurrStroke; i_point++)
                {
                    for (int i_duplicatePrint = 0; i_duplicatePrint < printedChar.usedSlots_inDuplicatesPrintOffsetList; i_duplicatePrint++)
                    {
                        Vector3 lineStartPos = printedChar.pos + printedChar.duplicatesPrintOffsets[i_duplicatePrint] * printedChar.sizeScalingFactor + DrawXXL_LinesManager.instance.currPrinted_charDef[i_stroke][i_point] * printedChar.size;
                        Vector3 lineEndPos = printedChar.pos + printedChar.duplicatesPrintOffsets[i_duplicatePrint] * printedChar.sizeScalingFactor + DrawXXL_LinesManager.instance.currPrinted_charDef[i_stroke][i_point + 1] * printedChar.size;
                        Line_fadeableAnimSpeed.InternalDraw(lineStartPos, lineEndPos, printedChar.color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                    }
                }
            }
        }

        static void ShearTowardsItalic(InternalDXXL_CharConfig printedChar)
        {
            if (printedChar.italic)
            {
                for (int i_stroke = 0; i_stroke < DrawXXL_LinesManager.instance.numberOfStrokes_forCurrUsedChar; i_stroke++)
                {
                    for (int i_lineSegment = 0; i_lineSegment < DrawXXL_LinesManager.instance.numberOfPointsForEachStroke_forCurrUsedChar[i_stroke]; i_lineSegment++)
                    {
                        DrawXXL_LinesManager.instance.currPrinted_charDef[i_stroke][i_lineSegment].x = DrawXXL_LinesManager.instance.currPrinted_charDef[i_stroke][i_lineSegment].x + DrawXXL_LinesManager.instance.currPrinted_charDef[i_stroke][i_lineSegment].y * italicIntensity;
                    }
                }
            }
        }

        static void TurnCharDef(ref Vector3[] charDef, Vector3 forward, Vector3 textUpNormalized)
        {
            Quaternion rotation = Quaternion.LookRotation(forward, textUpNormalized);
            for (int i = 0; i < charDef.Length; i++)
            {
                charDef[i] = rotation * charDef[i];
            }
        }

        public static void TurnCharDef(ref Vector3[] charDef, Quaternion rotation)
        {
            //tested via profiler: no performance gain if default rotations are skipped from this multiplication
            for (int i = 0; i < charDef.Length; i++)
            {
                charDef[i] = rotation * charDef[i];
            }
        }

        public static void TurnCharDef(ref List<Vector3> charDef, int usedSlotsInList, Quaternion rotation)
        {
            //tested via profiler: no performance gain if default rotations are skipped from this multiplication
            for (int i = 0; i < usedSlotsInList; i++)
            {
                charDef[i] = rotation * charDef[i];
            }
        }

        static void CreateCharConfigs(string text, Color color, float size)
        {
            // int i_char_highestPossibleSlotOfAutoLineBreakChars = Mathf.Min(text.Length - 1, chars.Count - 1); //<-Cannot save iteration cycles like this, because if only "text.Length - 1" chars are checked for autoLineBreakChar-removal, then autoLineBreakChar's at higher positions can be moved into the relevant range, which again should be removed.
            int i_char_highestPossibleSlotOfAutoLineBreakChars = chars.Count - 1;
            for (int i_char = i_char_highestPossibleSlotOfAutoLineBreakChars; i_char >= 0; i_char--)
            {
                if (chars[i_char] == autoLineBreakChar)
                {
                    chars.RemoveAt(i_char);
                }
            }

            for (int i_char = 0; i_char < text.Length; i_char++)
            {
                if (i_char < chars.Count)
                {
                    InternalDXXL_CharConfig currChar = chars[i_char];
                    currChar.character = text[i_char];
                    currChar.size = size;
                    currChar.color = color;
                    currChar.hasMissingSymbolDefinition = false;
                    currChar.bold = false;
                    currChar.italic = false;
                    currChar.deleted = false;
                    currChar.underlined = false;
                    currChar.sizeScalingFactor = 1.0f;
                    currChar.sizeHasBeenScaledViaRichtextMarkup = false;
                    currChar.strippedDueToParsing = false;
                    currChar.numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself = 0;
                    currChar.isIcon = false;
                }
                else
                {
                    InternalDXXL_CharConfig currChar = new InternalDXXL_CharConfig();
                    currChar.character = text[i_char];
                    currChar.size = size;
                    currChar.color = color;
                    chars.Add(currChar);
                }
            }
            usedCharConfigListSlots = text.Length;
        }

        static void InsertLineBreaks_fromRichTextMarkups(string text)
        {
            //lineBreaks from embedded richtext "<br>" strings:
            int maxNumberOfLineBreaks = 5000; //-> preventing endless loops
            int i_endOfCurrLineBreak = 0;
            int i_startOfCurrLineBreak;
            for (int i_lineBreak = 0; i_lineBreak < maxNumberOfLineBreaks; i_lineBreak++)
            {
                i_startOfCurrLineBreak = text.IndexOf(lineBreakMarkupString, i_endOfCurrLineBreak);
                if (i_startOfCurrLineBreak >= i_endOfCurrLineBreak)
                {
                    chars[i_startOfCurrLineBreak].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself = lineBreakMarkupString.Length;
                    i_endOfCurrLineBreak = i_startOfCurrLineBreak + (lineBreakMarkupString.Length - 1);
                    for (int i_char = i_startOfCurrLineBreak; i_char <= i_endOfCurrLineBreak; i_char++)
                    {
                        chars[i_char].strippedDueToParsing = true;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        static bool SwitchTextStyle(string text, string startMarkingString, string endMarkingString, InternalDXXL_CharConfig.SetCharStyleProperty StyleAssigningFunction)
        {
            bool foundAtLeastOneMarkup = false;
            int maxNumberOfMarkups = 10000;
            int i_endOfCurrMarkupPhase = 0;
            int i_startOfCurrMarkupPhase;
            for (int i_markup = 0; i_markup < maxNumberOfMarkups; i_markup++)
            {
                i_startOfCurrMarkupPhase = text.IndexOf(startMarkingString, i_endOfCurrMarkupPhase);
                if (i_startOfCurrMarkupPhase >= i_endOfCurrMarkupPhase)
                {
                    i_endOfCurrMarkupPhase = text.IndexOf(endMarkingString, i_startOfCurrMarkupPhase);
                    if (i_endOfCurrMarkupPhase > i_startOfCurrMarkupPhase)
                    {
                        foundAtLeastOneMarkup = true;
                        for (int i_char = i_startOfCurrMarkupPhase; i_char < i_startOfCurrMarkupPhase + startMarkingString.Length; i_char++)
                        {
                            chars[i_char].strippedDueToParsing = true;
                        }

                        for (int i_char = i_endOfCurrMarkupPhase; i_char < i_endOfCurrMarkupPhase + endMarkingString.Length; i_char++)
                        {
                            chars[i_char].strippedDueToParsing = true;
                        }

                        for (int i_char = i_startOfCurrMarkupPhase; i_char <= i_endOfCurrMarkupPhase; i_char++)
                        {
                            InternalDXXL_CharConfig currChar = chars[i_char];
                            StyleAssigningFunction(ref currChar);
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            return foundAtLeastOneMarkup;
        }

        static int GetMarkupPhases(ref List<InternalDXXL_MarkupPhase> markupPhasesList, string text, string startMarkingString_preValue, string endMarkingString)
        {
            //returns "usedSlotsInList"

            int i_nextFreeSlotInMarkupPhaseList = 0;
            int maxNumberOfMarkupPhases = 10000;
            int maxNumberOfEnclosingMarkupPhases = 1000;
            int i_startOfCurrMarkupPhase = text.IndexOf(startMarkingString_preValue, 0);

            for (int i_markup = 0; i_markup < maxNumberOfMarkupPhases; i_markup++)
            {
                if (i_startOfCurrMarkupPhase >= 0)
                {
                    int i_startOfValue = i_startOfCurrMarkupPhase + startMarkingString_preValue.Length;
                    int i_closingBracketAfterValue = text.IndexOf(valueMarkupString_postValue, i_startOfValue);
                    if (i_closingBracketAfterValue > i_startOfValue)
                    {
                        int valueLength = i_closingBracketAfterValue - i_startOfValue;
                        string value = text.Substring(i_startOfValue, valueLength);

                        int i_endOfCurrMarkupPhase = -1;
                        int i_startPosForSearchingNext_markupEnd = i_closingBracketAfterValue;
                        int i_startPosForSearchingNext_markupStart = i_closingBracketAfterValue;
                        for (int i = 0; i < maxNumberOfEnclosingMarkupPhases; i++)
                        {
                            int i_nextCurrMarkupPhaseEnd = text.IndexOf(endMarkingString, i_startPosForSearchingNext_markupEnd);
                            if (i_nextCurrMarkupPhaseEnd >= i_startPosForSearchingNext_markupEnd)
                            {
                                i_endOfCurrMarkupPhase = i_nextCurrMarkupPhaseEnd;
                                int i_nextMarkupPhaseStart = text.IndexOf(startMarkingString_preValue, i_startPosForSearchingNext_markupStart);
                                if (i_nextMarkupPhaseStart >= i_startPosForSearchingNext_markupStart)
                                {
                                    if (i_nextMarkupPhaseStart < i_nextCurrMarkupPhaseEnd)
                                    {
                                        i_startPosForSearchingNext_markupEnd = i_nextCurrMarkupPhaseEnd + 1;
                                        i_startPosForSearchingNext_markupStart = i_nextMarkupPhaseStart + 1;
                                        continue;
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                                else
                                {
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (i_endOfCurrMarkupPhase > i_startOfCurrMarkupPhase)
                        {
                            for (int i_char = i_startOfCurrMarkupPhase; i_char < i_startOfCurrMarkupPhase + startMarkingString_preValue.Length + valueLength + valueMarkupString_postValue.Length; i_char++)
                            {
                                chars[i_char].strippedDueToParsing = true;
                            }

                            for (int i_char = i_endOfCurrMarkupPhase; i_char < i_endOfCurrMarkupPhase + endMarkingString.Length; i_char++)
                            {
                                chars[i_char].strippedDueToParsing = true;
                            }

                            InternalDXXL_MarkupPhase markupPhase = new InternalDXXL_MarkupPhase();
                            markupPhase.unparsedValue = value;
                            markupPhase.i_firstChar = i_closingBracketAfterValue + 1;
                            markupPhase.i_lastChar = i_endOfCurrMarkupPhase - 1;
                            i_nextFreeSlotInMarkupPhaseList = AddToMarkupPhasesList(ref markupPhasesList, markupPhase, i_nextFreeSlotInMarkupPhaseList);
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }

                i_startOfCurrMarkupPhase = text.IndexOf(startMarkingString_preValue, i_startOfCurrMarkupPhase + 1);
            }
            return i_nextFreeSlotInMarkupPhaseList;
        }

        static int AddToMarkupPhasesList(ref List<InternalDXXL_MarkupPhase> markupPhasesList, InternalDXXL_MarkupPhase markupPhaseToAdd, int i_ofSlotWhereToAdd)
        {
            //function returns "i_nextFreeSlot"
            //function is not ensuring yet if addSlot is the next higher nonExisting-slot
            if (i_ofSlotWhereToAdd < markupPhasesList.Count)
            {
                markupPhasesList[i_ofSlotWhereToAdd] = markupPhaseToAdd;
            }
            else
            {
                markupPhasesList.Add(markupPhaseToAdd);
            }
            i_ofSlotWhereToAdd++;
            return i_ofSlotWhereToAdd;
        }

        static void AssignStrokeWidthOffsets(int usedSlotsInListOf_relStrokeWidthMarkupPhases, float unmodifiedCharSize, Quaternion rotationOfChars, bool skipDraw)
        {
            if (skipDraw == false)
            {
                //Add standard duplicate vector for zero-width-strokes:
                int i_ofFirstBoldChar = -1;
                for (int i = 0; i < usedCharConfigListSlots; i++)
                {
                    if (chars[i].bold)
                    {
                        if (i_ofFirstBoldChar == (-1))
                        {
                            chars[i].usedSlots_inDuplicatesPrintOffsetList = GetDuplicatesPrintOffsetsForBoldUnrotated(ref chars[i].duplicatesPrintOffsets, unmodifiedCharSize, 0.0f);
                            TurnCharDef(ref chars[i].duplicatesPrintOffsets, chars[i].usedSlots_inDuplicatesPrintOffsetList, rotationOfChars);
                            i_ofFirstBoldChar = i;
                        }
                        else
                        {
                            UtilitiesDXXL_List.CopyContentOfVectorLists(ref chars[i].duplicatesPrintOffsets, ref chars[i_ofFirstBoldChar].duplicatesPrintOffsets, chars[i_ofFirstBoldChar].usedSlots_inDuplicatesPrintOffsetList);
                            chars[i].usedSlots_inDuplicatesPrintOffsetList = chars[i_ofFirstBoldChar].usedSlots_inDuplicatesPrintOffsetList;
                        }
                    }
                    else
                    {
                        UtilitiesDXXL_List.AddToAVectorList(ref chars[i].duplicatesPrintOffsets, Vector3.zero, 0);
                        chars[i].usedSlots_inDuplicatesPrintOffsetList = 1;
                    }
                }

                //Add additional duplicate vectors for nonZero-width-strokes (overwriting upper standard-thinStroke-block):
                for (int i_markupPhase = 0; i_markupPhase < usedSlotsInListOf_relStrokeWidthMarkupPhases; i_markupPhase++)
                {
                    int relCharLinesWidth_asPPMofSize = 0;
                    try
                    {
                        relCharLinesWidth_asPPMofSize = Convert.ToInt32(relStrokeWidthMarkupPhases[i_markupPhase].unparsedValue);
                    }
                    catch (OverflowException)
                    {
                        Debug.LogError("Overflow exception in rich text strokeWidth markup. Couldn't parse '" + relStrokeWidthMarkupPhases[i_markupPhase].unparsedValue + "'");
                        continue;
                    }
                    catch (FormatException)
                    {
                        Debug.LogError("Wrong format in rich text strokeWidth markup. Couldn't parse '" + relStrokeWidthMarkupPhases[i_markupPhase].unparsedValue + "'");
                        continue;
                    }

                    relCharLinesWidth_asPPMofSize = Mathf.Max(relCharLinesWidth_asPPMofSize, 0);
                    if (relCharLinesWidth_asPPMofSize > maxRelStrokeWidth_inPPMofSize)
                    {
                        Debug.Log("relCharLinesWidth_asPPMofSize (" + relCharLinesWidth_asPPMofSize + ") has been reduced to maxRelStrokeWidth (" + maxRelStrokeWidth_inPPMofSize + "). (i_swMarkUpPhase = " + i_markupPhase + ")");
                        relCharLinesWidth_asPPMofSize = maxRelStrokeWidth_inPPMofSize;
                    }
                    float relCharLinesWidth = 0.000001f * relCharLinesWidth_asPPMofSize;

                    int i_ofFirstBoldCharOfPhase = -1;
                    int i_ofFirstNonBoldCharOfPhase = -1;
                    for (int i_char = relStrokeWidthMarkupPhases[i_markupPhase].i_firstChar; i_char <= relStrokeWidthMarkupPhases[i_markupPhase].i_lastChar; i_char++)
                    {
                        if (chars[i_char].bold)
                        {
                            if (i_ofFirstBoldCharOfPhase == (-1))
                            {
                                chars[i_char].usedSlots_inDuplicatesPrintOffsetList = GetDuplicatesPrintOffsetsForBoldUnrotated(ref chars[i_char].duplicatesPrintOffsets, unmodifiedCharSize, relCharLinesWidth);
                                TurnCharDef(ref chars[i_char].duplicatesPrintOffsets, chars[i_char].usedSlots_inDuplicatesPrintOffsetList, rotationOfChars);
                                i_ofFirstBoldCharOfPhase = i_char;
                            }
                            else
                            {
                                UtilitiesDXXL_List.CopyContentOfVectorLists(ref chars[i_char].duplicatesPrintOffsets, ref chars[i_ofFirstBoldCharOfPhase].duplicatesPrintOffsets, chars[i_ofFirstBoldCharOfPhase].usedSlots_inDuplicatesPrintOffsetList);
                                chars[i_char].usedSlots_inDuplicatesPrintOffsetList = chars[i_ofFirstBoldCharOfPhase].usedSlots_inDuplicatesPrintOffsetList;
                            }
                        }
                        else
                        {
                            if (i_ofFirstNonBoldCharOfPhase == (-1))
                            {
                                chars[i_char].usedSlots_inDuplicatesPrintOffsetList = GetDuplicatesPrintOffsetsUnrotated(ref chars[i_char].duplicatesPrintOffsets, unmodifiedCharSize, relCharLinesWidth);
                                TurnCharDef(ref chars[i_char].duplicatesPrintOffsets, chars[i_char].usedSlots_inDuplicatesPrintOffsetList, rotationOfChars);
                                i_ofFirstNonBoldCharOfPhase = i_char;
                            }
                            else
                            {
                                UtilitiesDXXL_List.CopyContentOfVectorLists(ref chars[i_char].duplicatesPrintOffsets, ref chars[i_ofFirstNonBoldCharOfPhase].duplicatesPrintOffsets, chars[i_ofFirstNonBoldCharOfPhase].usedSlots_inDuplicatesPrintOffsetList);
                                chars[i_char].usedSlots_inDuplicatesPrintOffsetList = chars[i_ofFirstNonBoldCharOfPhase].usedSlots_inDuplicatesPrintOffsetList;
                            }
                        }
                    }
                }
            }
        }

        static void TurnStrokeWidthOffsetsOnCircle()
        {
            for (int i_char = 0; i_char < usedCharConfigListSlots; i_char++)
            {
                if (chars[i_char].usedSlots_inDuplicatesPrintOffsetList > 0)
                {
                    for (int i_duplicateOffset = 0; i_duplicateOffset < chars[i_char].usedSlots_inDuplicatesPrintOffsetList; i_duplicateOffset++)
                    {
                        chars[i_char].duplicatesPrintOffsets[i_duplicateOffset] = chars[i_char].rotationFromCircleStart * chars[i_char].duplicatesPrintOffsets[i_duplicateOffset];
                    }
                }
            }
        }

        static void ScaleSize_perChar(int usedSlotsInListOf_sizeMarkupPhases, float size)
        {
            for (int i_markupPhase = 0; i_markupPhase < usedSlotsInListOf_sizeMarkupPhases; i_markupPhase++)
            {
                int parsedSizeModifierValue = 100;
                try
                {
                    parsedSizeModifierValue = Convert.ToInt32(sizeMarkupPhases[i_markupPhase].unparsedValue);
                }
                catch (OverflowException)
                {
                    Debug.LogError("Overflow exception in rich text size markup. Couldn't parse '" + sizeMarkupPhases[i_markupPhase].unparsedValue + "'");
                    continue;
                }
                catch (FormatException)
                {
                    Debug.LogError("Wrong format in rich text size markup. Couldn't parse '" + sizeMarkupPhases[i_markupPhase].unparsedValue + "'");
                    continue;
                }

                if (parsedSizeModifierValue <= 0)
                {
                    parsedSizeModifierValue = 1;
                }

                for (int i_char = sizeMarkupPhases[i_markupPhase].i_firstChar; i_char <= sizeMarkupPhases[i_markupPhase].i_lastChar; i_char++)
                {
                    //chars[i_char].sizeScalingFactor = (0.01f * (float)sizeModifier_inPercent); //-> if size would be in percent
                    chars[i_char].sizeScalingFactor = (0.090909090f * (float)parsedSizeModifierValue); //-> factor comes from: "size=11" seems to be the size that the unity console logs use. This is probably measured in pixels.

                    //chars[i_char].size = chars[i_char].size * chars[i_char].sizeScalingFactor; //-> this causes nested size markup phases to scale RELATIVE to the enclosing size markup phase.
                    chars[i_char].size = size * chars[i_char].sizeScalingFactor; //-> this causes nested size markup phases to scale ABSOLUTE depending only on the size value of the own markup phase.
                    chars[i_char].sizeHasBeenScaledViaRichtextMarkup = true; //-> used only for performance optimization
                }
            }
        }

        static void ApplyColor(int usedSlotsInListOf_colorMarkupPhases, bool skipDraw)
        {
            if (skipDraw == false)
            {
                for (int i_markupPhase = 0; i_markupPhase < usedSlotsInListOf_colorMarkupPhases; i_markupPhase++)
                {
                    Color parsedColor;
                    bool colorSuccesfullyParsed = ColorUtility.TryParseHtmlString(colorMarkupPhases[i_markupPhase].unparsedValue, out parsedColor); //"TryParseHtmlString" takes hex-string or alternatively the color name strings supported by Unity rich text
                    if (colorSuccesfullyParsed)
                    {
                        for (int i_char = colorMarkupPhases[i_markupPhase].i_firstChar; i_char <= colorMarkupPhases[i_markupPhase].i_lastChar; i_char++)
                        {
                            chars[i_char].color = parsedColor;
                        }
                    }
                    else
                    {
                        if (colorMarkupPhases[i_markupPhase].unparsedValue.Contains("#"))
                        {
                            Debug.LogError("Color parse failure in rich text size markup. Couldn't parse '" + colorMarkupPhases[i_markupPhase].unparsedValue + "'");
                        }
                        else
                        {
                            Debug.LogError("Color parse failure in rich text size markup. Couldn't parse '" + colorMarkupPhases[i_markupPhase].unparsedValue + "'. The reason may be a missing hashtag as start of the color definition.");
                        }
                    }
                }
            }
        }

        static void InsertIcons(string text)
        {
            int maxNumberOfMarkupPhases = 10000;
            int i_startOfCurrMarkupPhase;
            int i_closingBracketAfterValue = 0;
            for (int i_markup = 0; i_markup < maxNumberOfMarkupPhases; i_markup++)
            {
                i_startOfCurrMarkupPhase = text.IndexOf(iconMarkupString_preValue, i_closingBracketAfterValue);
                if (i_startOfCurrMarkupPhase >= i_closingBracketAfterValue)
                {
                    int i_startOfValue = i_startOfCurrMarkupPhase + iconMarkupString_preValue.Length;
                    i_closingBracketAfterValue = text.IndexOf(valueMarkupString_postValue, i_startOfValue);
                    if (i_closingBracketAfterValue > i_startOfValue)
                    {
                        int valueLength = i_closingBracketAfterValue - i_startOfValue;
                        string value = text.Substring(i_startOfValue, valueLength);
                        //The FIRST char of the icon-markupPhase is used as non-stripped icon-char (because 'InsertAutoLineBreaks()' inserts his linebreaks BEFORE non-stripped chars):
                        chars[i_startOfCurrMarkupPhase].isIcon = true;
                        chars[i_startOfCurrMarkupPhase].iconString = value;
                        for (int i_char = i_startOfCurrMarkupPhase + 1; i_char <= i_closingBracketAfterValue; i_char++)
                        {
                            chars[i_char].strippedDueToParsing = true;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
        }

        static int GetNumberOfNonStrippedChars()
        {
            int count = 0;
            for (int i = 0; i < usedCharConfigListSlots; i++)
            {
                if (chars[i].strippedDueToParsing == false)
                {
                    count++;
                }
            }
            return count;
        }

        static void InsertLineBreaks_fromEscapedUnicodeChars()
        {
            int maxNumberOfLineBreaks = 5000; //-> preventing endless loops
            int insertedLineBreaks = 0;
            for (int i_char = 0; i_char < usedCharConfigListSlots; i_char++)
            {
                if (chars[i_char].strippedDueToParsing == false)
                {
                    if (insertedLineBreaks > maxNumberOfLineBreaks)
                    {
                        Debug.LogWarning("Stopped parsing lineFeeds and carriageReturns, because maxNumberOfLineBreaks (" + maxNumberOfLineBreaks + ") was reached.");
                        break;
                    }

                    bool isALineBreakChar = false;
                    bool lineBreakConsistsOfTwoChars = false;

                    //unicode 10 = linefeed
                    //unicode 13 = carriagereturn
                    if (13 == (int)chars[i_char].character)
                    {
                        if (((i_char + 1) < usedCharConfigListSlots) && (10 == (int)chars[i_char + 1].character))
                        {
                            //-> sequence of "\r\n" was parsed into two unicode-chars:
                            isALineBreakChar = true;
                            lineBreakConsistsOfTwoChars = true;
                            chars[i_char + 1].hasMissingSymbolDefinition = true;
                            chars[i_char + 1].strippedDueToParsing = true;
                        }
                        else
                        {
                            //-> "\r" without following "\n" was parsed into one unicode-char:
                            isALineBreakChar = true;
                        }
                    }
                    else
                    {
                        if (10 == (int)chars[i_char].character)
                        {
                            //-> "\n" was parsed into one unicode-char:
                            isALineBreakChar = true;
                        }
                    }

                    if (isALineBreakChar)
                    {
                        chars[i_char].hasMissingSymbolDefinition = true;
                        chars[i_char].strippedDueToParsing = true;
                        chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself = lineBreakConsistsOfTwoChars ? 2 : 1;
                        for (int i = 1; i < chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself; i++)
                        {
                            i_char++;
                        }
                        insertedLineBreaks++;
                    }
                }
            }
        }

        static void InsertLineBreaks_fromMaxTextBlockWidthParameter(float autoLineBreakWidth, float size)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(autoLineBreakWidth) == false)
            {
                autoLineBreakWidth = Mathf.Max(autoLineBreakWidth, 0.0001f);
                float lengthOfCurrLine = 0.0f;
                int numberOfNonIgnoredChars_inCurrLine = 0;
                int numberOfInsertedAutoLineBreaks = 0;
                for (int i_char = 0; i_char < usedCharConfigListSlots; i_char++)
                {
                    if (numberOfInsertedAutoLineBreaks > 10000)
                    {
                        Debug.LogWarning("Inserting autoLineBreaks stopped, coz numberOfInsertedAutoLineBreaks (" + numberOfInsertedAutoLineBreaks + ") is too high.");
                        break;
                    }

                    if (chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself > 0)
                    {
                        lengthOfCurrLine = 0.0f;
                        numberOfNonIgnoredChars_inCurrLine = 0;
                        for (int i_charOfLineBreakString = 1; i_charOfLineBreakString < chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself; i_charOfLineBreakString++)
                        {
                            i_char++;
                        }
                    }
                    else
                    {
                        if (chars[i_char].strippedDueToParsing == false)
                        {
                            numberOfNonIgnoredChars_inCurrLine++;
                            lengthOfCurrLine = lengthOfCurrLine + chars[i_char].size;
                            if (numberOfNonIgnoredChars_inCurrLine > 1) //-> auto-lineBreaks can only be inserted if the line has at least one char (otherwise: Danger of endless loop)
                            {
                                if (lengthOfCurrLine > autoLineBreakWidth)
                                {
                                    numberOfInsertedAutoLineBreaks++;
                                    InsertLineBreakChar(i_char);
                                    lengthOfCurrLine = 0.0f;
                                    numberOfNonIgnoredChars_inCurrLine = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        static void Assign_coveredAnglePerChar_onTheLineAtTheReferenceRadius(float referenceRadius, float size)
        {
            //"referenceRadius" is the one which has been specified by the user via function parameter(s) (and if no lineBreaks would exist). This "referenceRadius" can later be shifted outwards by "textAnchor == TextAnchorCircledDXXL.LowerLeftOfWholeTextBlock", which will also update the "coveredAngleDeg_onTheLineAtTheReferenceRadius"'s.
            float coveredAngleDeg_onTheLineAtTheReferenceRadius_forADefaultSizedChar = Mathf.Rad2Deg * Mathf.Atan(size / referenceRadius);
            for (int i = 0; i < usedCharConfigListSlots; i++)
            {
                if (chars[i].sizeHasBeenScaledViaRichtextMarkup)
                {
                    chars[i].coveredAngleDeg_onTheLineAtTheReferenceRadius = Mathf.Rad2Deg * Mathf.Atan(chars[i].size / referenceRadius);
                }
                else
                {
                    chars[i].coveredAngleDeg_onTheLineAtTheReferenceRadius = coveredAngleDeg_onTheLineAtTheReferenceRadius_forADefaultSizedChar;
                }
            }
        }

        static float TryShiftTheTextToOutwardsOfRadius_soItEndsOnTheCircleInsteadOfStartingThere(DrawText.TextAnchorCircledDXXL textAnchor, float autoLineBreakAngleDeg, float size, float radius_beforeShifting)
        {
            if (textAnchor == DrawText.TextAnchorCircledDXXL.LowerLeftOfWholeTextBlock)
            {
                autoLineBreakAngleDeg = ForceDefault_autoLineBreakAngle(autoLineBreakAngleDeg);
                float radiusShiftOffset_awayFromCircleCenter_perDefaultSizedLineBreak = size * relLineDistance;
                float angleDegThatIsAlreadyCoveredByChars_forCurrLine = 0.0f;
                float angleDeg_ofStandardSizedChar_inTheLineAtTheReferenceRadius = Mathf.Rad2Deg * Mathf.Atan(size / radius_beforeShifting);
                float correctionFactor_forCoveredAnglePerChar_forCurrLine = 1.0f;
                float currRadius = radius_beforeShifting;
                float sizeOfBiggestChar_inCurrFinishedLine = 0.0f;
                int numberOfNonIgnoredChars_inCurrLine = 0;
                int numberOfShiftedLineBreaks = 0;

                for (int i_char = usedCharConfigListSlots - 1; i_char >= 0; i_char--)
                {
                    if (numberOfShiftedLineBreaks > 10000)
                    {
                        //-> "InsertLineBreaksOnCircle_fromMaxTextBlockAngleParameter()" will print the log message for cases like this later
                        return radius_beforeShifting;
                    }

                    if (chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself > 0)
                    {
                        //current char is already a lineBreak (from <br>-markup, or from unicode-chars, since autoLineBreakAngle is not done yet):
                        UpdateAndReset_charsPerLineCountingParameters_dueToLineShift(ref numberOfShiftedLineBreaks, out angleDegThatIsAlreadyCoveredByChars_forCurrLine, out numberOfNonIgnoredChars_inCurrLine, out currRadius, out correctionFactor_forCoveredAnglePerChar_forCurrLine, out sizeOfBiggestChar_inCurrFinishedLine, currRadius, radiusShiftOffset_awayFromCircleCenter_perDefaultSizedLineBreak, size, angleDeg_ofStandardSizedChar_inTheLineAtTheReferenceRadius);
                    }
                    else
                    {
                        if (chars[i_char].strippedDueToParsing == false)
                        {
                            numberOfNonIgnoredChars_inCurrLine++;
                            sizeOfBiggestChar_inCurrFinishedLine = Mathf.Max(sizeOfBiggestChar_inCurrFinishedLine, chars[i_char].size);
                            angleDegThatIsAlreadyCoveredByChars_forCurrLine = angleDegThatIsAlreadyCoveredByChars_forCurrLine + chars[i_char].coveredAngleDeg_onTheLineAtTheReferenceRadius * correctionFactor_forCoveredAnglePerChar_forCurrLine;
                            if (angleDegThatIsAlreadyCoveredByChars_forCurrLine > autoLineBreakAngleDeg)
                            {
                                if (numberOfNonIgnoredChars_inCurrLine > 1)
                                {
                                    //-> lines with only 1 char, which already protrudes the "autoLineBreakAngleDeg" the char will stay as "protruding" and will stay in its line. His covered angle span is therefore already "used up" and we can proceed to the next char. These cases don't arrive here.
                                    //-> in lines, where the first prodruding char is the 2nd char or a higher char: These cases arrive here. The protruding char will not be printed in this line, but will start the line after the line break. His covered angle span is not used up yet so when starting with the next line his angle span should be considered once again. Therefore "i_char" gets manually changed here.
                                    i_char++; //-> ensuring that the angle span of the current charcter will be considered once again after the current line shift.
                                }

                                float radiusShiftOffset_awayFromCircleCenter_forThisLineBreak = Get_radiusShiftOffset_awayFromCircleCenter_forThisLineBreak(numberOfNonIgnoredChars_inCurrLine, i_char, size, sizeOfBiggestChar_inCurrFinishedLine, radiusShiftOffset_awayFromCircleCenter_perDefaultSizedLineBreak);
                                UpdateAndReset_charsPerLineCountingParameters_dueToLineShift(ref numberOfShiftedLineBreaks, out angleDegThatIsAlreadyCoveredByChars_forCurrLine, out numberOfNonIgnoredChars_inCurrLine, out currRadius, out correctionFactor_forCoveredAnglePerChar_forCurrLine, out sizeOfBiggestChar_inCurrFinishedLine, currRadius, radiusShiftOffset_awayFromCircleCenter_forThisLineBreak, size, angleDeg_ofStandardSizedChar_inTheLineAtTheReferenceRadius);
                            }
                        }
                    }
                }

                if (numberOfShiftedLineBreaks > 0)
                {
                    ScaleAllAnglesOnTheLineAtTheReferenceRadius(correctionFactor_forCoveredAnglePerChar_forCurrLine);
                    return currRadius;
                }
                else
                {
                    return radius_beforeShifting;
                }
            }
            else
            {
                return radius_beforeShifting;
            }
        }

        static float Get_radiusShiftOffset_awayFromCircleCenter_forThisLineBreak(int numberOfNonIgnoredChars_inCurrLine, int i_char, float size, float sizeOfBiggestChar_inCurrFinishedLine, float radiusShiftOffset_awayFromCircleCenter_perDefaultSizedLineBreak)
        {
            //-> This tries to compensate the influence of size richtext markups
            //-> This is not a fully precise substitue for "GetBiggestCharSizeInsideLine()" (which is not available yet) (and the detected lineShifts here are at different chars than the later determined lineBreaks, since we iterate the char-list backward here, but later we iterate forward), but it is better than nothing:
            float approxRelSize_ofBiggestCharInTheLine;
            if (numberOfNonIgnoredChars_inCurrLine == 1)
            {
                approxRelSize_ofBiggestCharInTheLine = chars[i_char].size / size;
            }
            else
            {
                float relSize_ofBiggestCharInTheLine = sizeOfBiggestChar_inCurrFinishedLine / size;
                float weightOfBiggestFoundChar = 0.5f;
                approxRelSize_ofBiggestCharInTheLine = Mathf.Lerp(1.0f, relSize_ofBiggestCharInTheLine, weightOfBiggestFoundChar);
            }

            return (radiusShiftOffset_awayFromCircleCenter_perDefaultSizedLineBreak * approxRelSize_ofBiggestCharInTheLine);
        }

        static void UpdateAndReset_charsPerLineCountingParameters_dueToLineShift(ref int numberOfShiftedLineBreaks, out float angleDegThatIsAlreadyCoveredByChars_forCurrLine, out int numberOfNonIgnoredChars_inCurrLine, out float radius_postShift, out float correctionFactor_forCoveredAnglePerChar_forCurrLine, out float sizeOfBiggestChar_startValueForUpcomingLine, float radius_preShift, float radiusShiftOffset_awayFromCircleCenter_forThisLineBreak, float sizePerChar_withoutRichtextSizeMarkupModification, float angleDeg_ofStandardSizedChar_inTheLineAtTheReferenceRadius)
        {
            numberOfShiftedLineBreaks++;
            angleDegThatIsAlreadyCoveredByChars_forCurrLine = 0.0f;
            numberOfNonIgnoredChars_inCurrLine = 0;
            sizeOfBiggestChar_startValueForUpcomingLine = 0.0f;
            radius_postShift = radius_preShift + radiusShiftOffset_awayFromCircleCenter_forThisLineBreak; //-> more correct would be to use "GetBiggestCharSizeInsideLine()" instead of "size", but it is not available yet, because the automaticLineBreaksAtMaxAngle are not inserted yet. Using the lineShifts that get detected here is not a sufficient substitute, because the lineBreaks here can be at differnt positions than those who get inserted afterwards in "InsertLineBreaksOnCircle_fromMaxTextBlockAngleParameter()". 
            correctionFactor_forCoveredAnglePerChar_forCurrLine = Calc_correctionFactor_forCoveredAnglePerChar_perLine(radius_postShift, sizePerChar_withoutRichtextSizeMarkupModification, angleDeg_ofStandardSizedChar_inTheLineAtTheReferenceRadius); //-> this "angleCorrectionFactorForCurrLine" is only provisional here, so not neccessarily fully precise, due to using "radius_postShift" (see notes there)
        }

        static void ScaleAllAnglesOnTheLineAtTheReferenceRadius(float scaleFactor)
        {
            for (int i = 0; i < usedCharConfigListSlots; i++)
            {
                chars[i].coveredAngleDeg_onTheLineAtTheReferenceRadius = chars[i].coveredAngleDeg_onTheLineAtTheReferenceRadius * scaleFactor;
            }
        }

        static void InsertLineBreaksOnCircle_fromMaxTextBlockAngleParameter(float autoLineBreakAngleDeg, float size, float radius)
        {
            autoLineBreakAngleDeg = ForceDefault_autoLineBreakAngle(autoLineBreakAngleDeg);

            float angleDegThatIsAlreadyCoveredByChars_forCurrLine = 0.0f;
            float angleDeg_ofStandardSizedChar_inTheLineAtTheReferenceRadius = Mathf.Rad2Deg * Mathf.Atan(size / radius);
            float correctionFactor_forCoveredAnglePerChar_forCurrLine = 1.0f;
            float currRadius = radius;
            int numberOfNonIgnoredChars_inCurrLine = 0;
            int numberOfInsertedAutoLineBreaks = 0;
            for (int i_char = 0; i_char < usedCharConfigListSlots; i_char++)
            {
                if (numberOfInsertedAutoLineBreaks > 10000)
                {
                    Debug.LogWarning("Inserting autoLineBreaks stopped, coz numberOfInsertedAutoLineBreaks (" + numberOfInsertedAutoLineBreaks + ") is too high.");
                    break;
                }

                if (chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself > 0)
                {
                    //current char is already a lineBreak (from <br>-markup, or from unicode-chars, since autoLineBreakAngle is not done yet):
                    UpdateAndReset_charsPerLineCountingParameters_dueToLineBreak(out angleDegThatIsAlreadyCoveredByChars_forCurrLine, out numberOfNonIgnoredChars_inCurrLine, out currRadius, out correctionFactor_forCoveredAnglePerChar_forCurrLine, currRadius, size, angleDeg_ofStandardSizedChar_inTheLineAtTheReferenceRadius);
                    for (int i_charOfLineBreakString = 1; i_charOfLineBreakString < chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself; i_charOfLineBreakString++)
                    {
                        i_char++;
                    }
                }
                else
                {
                    if (chars[i_char].strippedDueToParsing == false)
                    {
                        numberOfNonIgnoredChars_inCurrLine++;
                        angleDegThatIsAlreadyCoveredByChars_forCurrLine = angleDegThatIsAlreadyCoveredByChars_forCurrLine + chars[i_char].coveredAngleDeg_onTheLineAtTheReferenceRadius * correctionFactor_forCoveredAnglePerChar_forCurrLine;
                        if (numberOfNonIgnoredChars_inCurrLine > 1) //-> auto-lineBreaks can only be inserted if the line has at least one char (otherwise: danger of endless loop)
                        {
                            if (angleDegThatIsAlreadyCoveredByChars_forCurrLine > autoLineBreakAngleDeg)
                            {
                                numberOfInsertedAutoLineBreaks++;
                                InsertLineBreakChar(i_char);
                                UpdateAndReset_charsPerLineCountingParameters_dueToLineBreak(out angleDegThatIsAlreadyCoveredByChars_forCurrLine, out numberOfNonIgnoredChars_inCurrLine, out currRadius, out correctionFactor_forCoveredAnglePerChar_forCurrLine, currRadius, size, angleDeg_ofStandardSizedChar_inTheLineAtTheReferenceRadius);
                            }
                        }
                    }
                }
            }
        }

        static float ForceDefault_autoLineBreakAngle(float autoLineBreakAngleDeg)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(autoLineBreakAngleDeg)) { autoLineBreakAngleDeg = 360.0f; }
            autoLineBreakAngleDeg = Mathf.Max(autoLineBreakAngleDeg, 0.01f);
            autoLineBreakAngleDeg = Mathf.Min(autoLineBreakAngleDeg, 360.0f);
            return autoLineBreakAngleDeg;
        }

        static void UpdateAndReset_charsPerLineCountingParameters_dueToLineBreak(out float angleDegThatIsAlreadyCoveredByChars_forCurrLine, out int numberOfNonIgnoredChars_inCurrLine, out float radiusAfterLineBreak, out float correctionFactor_forCoveredAnglePerChar_forUpcomingLine, float radius_beforeLineBreak, float sizePerChar_withoutRichtextSizeMarkupModification, float angleDeg_ofStandardSizedChar_inTheLineAtTheReferenceRadius)
        {
            angleDegThatIsAlreadyCoveredByChars_forCurrLine = 0.0f;
            numberOfNonIgnoredChars_inCurrLine = 0;
            radiusAfterLineBreak = GetRadiusOfCirclesNextInnerLine(radius_beforeLineBreak, sizePerChar_withoutRichtextSizeMarkupModification); //-> more correct would be to use "GetBiggestCharSizeInsideLine()" instead of "size", but it is not available yet, since we would need all lineBreaks, but we are currently still in the process of inserting lineBreaks
            correctionFactor_forCoveredAnglePerChar_forUpcomingLine = Calc_correctionFactor_forCoveredAnglePerChar_perLine(radiusAfterLineBreak, sizePerChar_withoutRichtextSizeMarkupModification, angleDeg_ofStandardSizedChar_inTheLineAtTheReferenceRadius); //-> this "angleCorrectionFactorForUpcomingLine" is only provisional here, so not neccessarily fully precise, since "GetBiggestCharSizeInsideLine()" hasn't been taken into account yet while calculating "radiusAfterLineBreak".
        }

        static void InsertLineBreakChar(int i_whereToInsert)
        {
            autoLineBreakChar.hasMissingSymbolDefinition = true;
            autoLineBreakChar.strippedDueToParsing = true;
            autoLineBreakChar.numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself = 1;
            chars.Insert(i_whereToInsert, autoLineBreakChar);
            usedCharConfigListSlots++;
        }

        static float Calc_correctionFactor_forCoveredAnglePerChar_perLine(float radiusOfConcernedLine, float sizeOfStandardSizedChar, float angleDeg_ofAStandardSizedChar_onTheLineAtTheReferenceRadius)
        {
            //"standard sized" means "the size specified by the textSize-paramter, but not scaled by a richtext size markup"
            float angleDeg_ofAStandardSizedChar_inCurrLine = Mathf.Rad2Deg * Mathf.Atan(sizeOfStandardSizedChar / radiusOfConcernedLine);
            return (angleDeg_ofAStandardSizedChar_inCurrLine / angleDeg_ofAStandardSizedChar_onTheLineAtTheReferenceRadius);
        }

        static void Assign_coveredAnglePerChar_onTheCharsOwnLine(float radius, float size)
        {
            float coveredAngleDeg_ofAStandardSizedChar_onTheLineAtTheReferenceRadius = Mathf.Rad2Deg * Mathf.Atan(size / radius);
            float correctionFactor_forCoveredAnglePerChar_forCurrLine = 1.0f;
            float currRadius = radius;
            for (int i_char = 0; i_char < usedCharConfigListSlots; i_char++)
            {
                if (chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself > 0)
                {
                    //switching the lines:
                    float sizeOfBiggestCharInUpcomingLine = GetBiggestCharSizeInsideLine(i_char + chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself);
                    currRadius = GetRadiusOfCirclesNextInnerLine(currRadius, sizeOfBiggestCharInUpcomingLine);
                    correctionFactor_forCoveredAnglePerChar_forCurrLine = Calc_correctionFactor_forCoveredAnglePerChar_perLine(currRadius, size, coveredAngleDeg_ofAStandardSizedChar_onTheLineAtTheReferenceRadius);

                    for (int i_charOfLineBreakString = 1; i_charOfLineBreakString < chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself; i_charOfLineBreakString++)
                    {
                        i_char++;
                    }
                }
                else
                {
                    //correcting the sizes inside the current line:
                    chars[i_char].coveredAngleDegOnOwnLine = chars[i_char].coveredAngleDeg_onTheLineAtTheReferenceRadius * correctionFactor_forCoveredAnglePerChar_forCurrLine;
                }
            }
        }

        static float GetRadiusOfCirclesNextInnerLine(float radiusOfLine_beforeJumpingDownToNextInnerLine, float sizeOfBiggestCharInUpcomingLine)
        {
            float reducedRadius = radiusOfLine_beforeJumpingDownToNextInnerLine - sizeOfBiggestCharInUpcomingLine * relLineDistance;
            return Mathf.Max(reducedRadius, minRadius);
        }

        static void FillParsedTextSpecs()
        {
            DrawText.parsedTextSpecs.widthOfLongestLine = 0.0f;
            DrawText.parsedTextSpecs.numberOfChars_inLongestLine = 0;
            DrawText.parsedTextSpecs.numberOfChars_afterParsingOutTheMarkupTags = 0;
            DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine = GetBiggestCharSizeInsideLine(0);
            DrawText.parsedTextSpecs.height_lowFirstLine_toLowLastLine = 0.0f;

            float lengthOfCurrLine = 0.0f;
            int numberOfChars_inCurrLine = 0;
            for (int i_char = 0; i_char < usedCharConfigListSlots; i_char++)
            {
                if (chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself > 0)
                {
                    DrawText.parsedTextSpecs.widthOfLongestLine = Math.Max(DrawText.parsedTextSpecs.widthOfLongestLine, lengthOfCurrLine);
                    DrawText.parsedTextSpecs.numberOfChars_inLongestLine = Math.Max(DrawText.parsedTextSpecs.numberOfChars_inLongestLine, numberOfChars_inCurrLine);
                    lengthOfCurrLine = 0.0f;
                    numberOfChars_inCurrLine = 0;
                    for (int i_charOfLineBreakString = 1; i_charOfLineBreakString < chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself; i_charOfLineBreakString++)
                    {
                        i_char++;
                    }
                    DrawText.parsedTextSpecs.height_lowFirstLine_toLowLastLine = DrawText.parsedTextSpecs.height_lowFirstLine_toLowLastLine + GetBiggestCharSizeInsideLine(i_char + 1) * relLineDistance;
                }
                else
                {
                    if (chars[i_char].strippedDueToParsing == false)
                    {
                        lengthOfCurrLine = lengthOfCurrLine + chars[i_char].size;
                        numberOfChars_inCurrLine++;
                        DrawText.parsedTextSpecs.numberOfChars_afterParsingOutTheMarkupTags++;
                    }
                }
            }

            //last line (after last lineBreak):
            DrawText.parsedTextSpecs.widthOfLongestLine = Math.Max(DrawText.parsedTextSpecs.widthOfLongestLine, lengthOfCurrLine);
            DrawText.parsedTextSpecs.numberOfChars_inLongestLine = Math.Max(DrawText.parsedTextSpecs.numberOfChars_inLongestLine, numberOfChars_inCurrLine);

            DrawText.parsedTextSpecs.height_wholeTextBlock = DrawText.parsedTextSpecs.height_lowFirstLine_toLowLastLine + DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine * relLineDistance;
        }

        static void FillParsedTextOnCircleSpecs()
        {
            DrawText.parsedTextOnCircleSpecs.angleDegOfLongestLine = 0.0f;
            DrawText.parsedTextOnCircleSpecs.numberOfChars_inLongestLine = 0;
            DrawText.parsedTextOnCircleSpecs.numberOfChars_afterParsingOutTheMarkupTags = 0;
            DrawText.parsedTextOnCircleSpecs.sizeOfBiggestCharInFirstLine = 0.0f;

            float angleDegOfCurrLine = 0.0f;
            int numberOfChars_inCurrLine = 0;
            bool isFirstLine = true;
            for (int i_char = 0; i_char < usedCharConfigListSlots; i_char++)
            {
                if (chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself > 0)
                {
                    isFirstLine = false;
                    DrawText.parsedTextOnCircleSpecs.angleDegOfLongestLine = Math.Max(DrawText.parsedTextOnCircleSpecs.angleDegOfLongestLine, angleDegOfCurrLine);
                    DrawText.parsedTextOnCircleSpecs.numberOfChars_inLongestLine = Math.Max(DrawText.parsedTextOnCircleSpecs.numberOfChars_inLongestLine, numberOfChars_inCurrLine);
                    angleDegOfCurrLine = 0.0f;
                    numberOfChars_inCurrLine = 0;
                    for (int i_charOfLineBreakString = 1; i_charOfLineBreakString < chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself; i_charOfLineBreakString++)
                    {
                        i_char++;
                    }
                }
                else
                {
                    if (chars[i_char].strippedDueToParsing == false)
                    {
                        angleDegOfCurrLine = angleDegOfCurrLine + chars[i_char].coveredAngleDegOnOwnLine;
                        numberOfChars_inCurrLine++;
                        DrawText.parsedTextOnCircleSpecs.numberOfChars_afterParsingOutTheMarkupTags++;
                        if (isFirstLine)
                        {
                            DrawText.parsedTextOnCircleSpecs.sizeOfBiggestCharInFirstLine = Math.Max(DrawText.parsedTextOnCircleSpecs.sizeOfBiggestCharInFirstLine, chars[i_char].size);
                        }
                    }
                }
            }

            //last line (without lineBreak):
            DrawText.parsedTextOnCircleSpecs.angleDegOfLongestLine = Math.Max(DrawText.parsedTextOnCircleSpecs.angleDegOfLongestLine, angleDegOfCurrLine);
            DrawText.parsedTextOnCircleSpecs.numberOfChars_inLongestLine = Math.Max(DrawText.parsedTextOnCircleSpecs.numberOfChars_inLongestLine, numberOfChars_inCurrLine);
        }

        static void AssignIndividualRotation(Vector3 initialTextUpNormalized, Vector3 forward)
        {
            float angleDegOfCurrLine = 0.0f;
            for (int i_char = 0; i_char < usedCharConfigListSlots; i_char++)
            {
                if (chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself > 0)
                {
                    angleDegOfCurrLine = 0.0f;
                    for (int i_charOfLineBreakString = 1; i_charOfLineBreakString < chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself; i_charOfLineBreakString++)
                    {
                        i_char++;
                    }
                }
                else
                {
                    if (chars[i_char].strippedDueToParsing == false)
                    {
                        chars[i_char].rotationFromCircleStart = Quaternion.AngleAxis(angleDegOfCurrLine, -forward);
                        chars[i_char].charUp = chars[i_char].rotationFromCircleStart * initialTextUpNormalized;
                        //chars[i_char].charDirection = chars[i_char].rotationFromCircleStart * initialTextDirNormalized;
                        angleDegOfCurrLine = angleDegOfCurrLine + chars[i_char].coveredAngleDegOnOwnLine;
                    }
                }
            }
        }

        static float ForceTextWidth(float forceTextEnlargementToThisMinWidth, float forceRestrictTextSizeToThisMaxTextWidth)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(forceTextEnlargementToThisMinWidth) == false || UtilitiesDXXL_Math.ApproximatelyZero(forceRestrictTextSizeToThisMaxTextWidth) == false)
            {
                float scaleFactor = 1.0f;
                bool ignore_forceMin = false;
                if (forceRestrictTextSizeToThisMaxTextWidth > 0.0f)
                {
                    if (DrawText.parsedTextSpecs.widthOfLongestLine > forceRestrictTextSizeToThisMaxTextWidth)
                    {
                        scaleFactor = forceRestrictTextSizeToThisMaxTextWidth / DrawText.parsedTextSpecs.widthOfLongestLine;
                    }

                    if (forceTextEnlargementToThisMinWidth > forceRestrictTextSizeToThisMaxTextWidth)
                    {
                        ignore_forceMin = true;
                        Debug.LogWarning("Contradiction: forceTextEnlargementToThisMinWidth (" + forceTextEnlargementToThisMinWidth + ") is bigger than forceRestrictTextSizeToThisMaxTextWidth (" + forceRestrictTextSizeToThisMaxTextWidth + ") -> forceTextEnlargementToThisMinWidth gets ignored.");
                    }
                }

                if (ignore_forceMin == false)
                {
                    if (forceTextEnlargementToThisMinWidth > 0.0f)
                    {
                        if (DrawText.parsedTextSpecs.widthOfLongestLine < forceTextEnlargementToThisMinWidth)
                        {
                            scaleFactor = forceTextEnlargementToThisMinWidth / DrawText.parsedTextSpecs.widthOfLongestLine;
                        }
                    }
                }

                if (Mathf.Approximately(1.0f, scaleFactor) == false)
                {
                    ScaleSizeOfAllChars(scaleFactor);
                    DrawText.parsedTextSpecs.widthOfLongestLine = DrawText.parsedTextSpecs.widthOfLongestLine * scaleFactor;
                    DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine = DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine * scaleFactor;
                    DrawText.parsedTextSpecs.height_wholeTextBlock = DrawText.parsedTextSpecs.height_wholeTextBlock * scaleFactor;
                    DrawText.parsedTextSpecs.height_lowFirstLine_toLowLastLine = DrawText.parsedTextSpecs.height_lowFirstLine_toLowLastLine * scaleFactor;
                    return scaleFactor;
                }
            }
            return 1.0f;
        }

        static void ScaleSizeOfAllChars(float scaleFactor)
        {
            for (int i_char = 0; i_char < usedCharConfigListSlots; i_char++)
            {
                chars[i_char].sizeScalingFactor = chars[i_char].sizeScalingFactor * scaleFactor;
                chars[i_char].size = chars[i_char].size * scaleFactor;
            }
        }

        static int GetDuplicatesPrintOffsetsUnrotated(ref List<Vector3> concernedDuplicatesPrintOffsetsList, float size, float relativeStrokeWidth)
        {
            //function returns "usedSlots_inConcernedDuplicatesPrintOffsetsList"
            if (UtilitiesDXXL_Math.ApproximatelyZero(relativeStrokeWidth) == false)
            {
                int usedSlots_inConcernedDuplicatesPrintOffsetsList = 0;
                usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, Vector3.zero, usedSlots_inConcernedDuplicatesPrintOffsetsList);

                float halfAbsStrokeWidth = 0.5f * size * relativeStrokeWidth;

                usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * Vector3.up, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * duplicateTriangle_LeftLeftDown_normalized, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * duplicateTriangle_RightRightDown_normalized, usedSlots_inConcernedDuplicatesPrintOffsetsList);

                if (relativeStrokeWidth > 0.05f)
                {
                    usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * Vector3.down, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                    usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * duplicateTriangle_LeftLeftUp_normalized, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                    usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * duplicateTriangle_RightRightUp_normalized, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                }

                if (relativeStrokeWidth > 0.1f)
                {
                    usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * Vector3.left, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                    usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * duplicateTriangle_RightUpUp_normalized, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                    usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * duplicateTriangle_RightDownDown_normalized, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                }

                if (relativeStrokeWidth > 0.16f)
                {
                    usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * Vector3.right, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                    usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * duplicateTriangle_LeftUpUp_normalized, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                    usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * duplicateTriangle_LeftDownDown_normalized, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                }

                return usedSlots_inConcernedDuplicatesPrintOffsetsList;
            }
            else
            {
                UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, Vector3.zero, 0);
                return 1;
            }
        }

        public static int GetDuplicatesPrintOffsetsUnrotated_ofTextIndependentIconOfSize1(ref List<Vector3> concernedDuplicatesPrintOffsetsList, float size, float relativeStrokeWidth)
        {
            //function returns "usedSlots_inConcernedDuplicatesPrintOffsetsList"
            if (UtilitiesDXXL_Math.ApproximatelyZero(relativeStrokeWidth) == false)
            {
                int usedSlots_inConcernedDuplicatesPrintOffsetsList = 0;
                usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, Vector3.zero, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                float halfAbsStrokeWidth = 0.5f * size * relativeStrokeWidth;

                usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * Vector3.up, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * duplicateTriangle_LeftLeftDown_normalized, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * duplicateTriangle_RightRightDown_normalized, usedSlots_inConcernedDuplicatesPrintOffsetsList);

                if (relativeStrokeWidth > 0.012f)
                {
                    usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * Vector3.down, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                    usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * duplicateTriangle_LeftLeftUp_normalized, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                    usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * duplicateTriangle_RightRightUp_normalized, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                }

                if (relativeStrokeWidth > 0.025f)
                {
                    usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * Vector3.left, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                    usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * duplicateTriangle_RightUpUp_normalized, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                    usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * duplicateTriangle_RightDownDown_normalized, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                    usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * Vector3.right, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                    usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * duplicateTriangle_LeftUpUp_normalized, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                    usedSlots_inConcernedDuplicatesPrintOffsetsList = UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, halfAbsStrokeWidth * duplicateTriangle_LeftDownDown_normalized, usedSlots_inConcernedDuplicatesPrintOffsetsList);
                }
                return usedSlots_inConcernedDuplicatesPrintOffsetsList;
            }
            else
            {
                UtilitiesDXXL_List.AddToAVectorList(ref concernedDuplicatesPrintOffsetsList, Vector3.zero, 0);
                return 1;
            }
        }

        static int GetDuplicatesPrintOffsetsForBoldUnrotated(ref List<Vector3> concernedDuplicatesPrintOffsetsList, float size, float relativeStrokeWidth)
        {
            //function returns "usedSlots_inConcernedDuplicatesPrintOffsetsList"
            float relBoldStrokeWidth = 3.0f * relativeStrokeWidth;
            relBoldStrokeWidth = Mathf.Max(relBoldStrokeWidth, minRelBoldStrokeWidth);
            relBoldStrokeWidth = Mathf.Min(relBoldStrokeWidth, maxRelBoldStrokeWidth);
            return GetDuplicatesPrintOffsetsUnrotated(ref concernedDuplicatesPrintOffsetsList, size, relBoldStrokeWidth);
        }

        static float GetBiggestCharSizeInsideLine(int i_startOfLine)
        {
            if (i_startOfLine < usedCharConfigListSlots)
            {
                float biggestSize = 0.0f;
                for (int i_char = i_startOfLine; i_char < usedCharConfigListSlots; i_char++)
                {
                    if (chars[i_char].numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself > 0)
                    {
                        break;
                    }

                    if (chars[i_char].strippedDueToParsing == false)
                    {
                        biggestSize = Mathf.Max(biggestSize, chars[i_char].size);
                    }
                }

                if (UtilitiesDXXL_Math.ApproximatelyZero(biggestSize))
                {
                    return chars[i_startOfLine].size;
                }
                else
                {
                    return biggestSize;
                }
            }
            else
            {
                return 0.0f;
            }
        }

        static Vector3 GetLowLeftPosOfFirstLine(Vector3 position, DrawText.TextAnchorDXXL textAnchor, Vector3 textDirNormalized, Vector3 textUpNormalized)
        {
            Vector3 fromMiddleLeft_toLowLeftOfFirstLine;
            switch (textAnchor)
            {
                case DrawText.TextAnchorDXXL.UpperLeft:
                    return position - textUpNormalized * DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine * relLineDistance;

                case DrawText.TextAnchorDXXL.UpperCenter:
                    return position - 0.5f * textDirNormalized * DrawText.parsedTextSpecs.widthOfLongestLine - textUpNormalized * DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine * relLineDistance;

                case DrawText.TextAnchorDXXL.UpperRight:
                    return position - textDirNormalized * DrawText.parsedTextSpecs.widthOfLongestLine - textUpNormalized * DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine * relLineDistance;

                case DrawText.TextAnchorDXXL.MiddleLeft:
                    fromMiddleLeft_toLowLeftOfFirstLine = textUpNormalized * (0.5f * DrawText.parsedTextSpecs.height_wholeTextBlock - 0.775f * DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine * relLineDistance);
                    return position + fromMiddleLeft_toLowLeftOfFirstLine;

                case DrawText.TextAnchorDXXL.MiddleCenter:
                    fromMiddleLeft_toLowLeftOfFirstLine = textUpNormalized * (0.5f * DrawText.parsedTextSpecs.height_wholeTextBlock - 0.775f * DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine * relLineDistance);
                    return position - 0.5f * textDirNormalized * DrawText.parsedTextSpecs.widthOfLongestLine + fromMiddleLeft_toLowLeftOfFirstLine;

                case DrawText.TextAnchorDXXL.MiddleRight:
                    fromMiddleLeft_toLowLeftOfFirstLine = textUpNormalized * (0.5f * DrawText.parsedTextSpecs.height_wholeTextBlock - 0.775f * DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine * relLineDistance);
                    return position - textDirNormalized * DrawText.parsedTextSpecs.widthOfLongestLine + fromMiddleLeft_toLowLeftOfFirstLine;

                case DrawText.TextAnchorDXXL.LowerLeft:
                    return position + textUpNormalized * DrawText.parsedTextSpecs.height_lowFirstLine_toLowLastLine;

                case DrawText.TextAnchorDXXL.LowerCenter:
                    return position - 0.5f * textDirNormalized * DrawText.parsedTextSpecs.widthOfLongestLine + textUpNormalized * DrawText.parsedTextSpecs.height_lowFirstLine_toLowLastLine;

                case DrawText.TextAnchorDXXL.LowerRight:
                    return position - textDirNormalized * DrawText.parsedTextSpecs.widthOfLongestLine + textUpNormalized * DrawText.parsedTextSpecs.height_lowFirstLine_toLowLastLine;

                case DrawText.TextAnchorDXXL.LowerLeftOfFirstLine:
                    return position;

                case DrawText.TextAnchorDXXL.LowerCenterOfFirstLine:
                    return position - 0.5f * textDirNormalized * DrawText.parsedTextSpecs.widthOfLongestLine;

                case DrawText.TextAnchorDXXL.LowerRightOfFirstLine:
                    return position - textDirNormalized * DrawText.parsedTextSpecs.widthOfLongestLine;

                default:
                    Debug.LogError("TextAnchorExt of '" + textAnchor + "' not found. Fallback to 'LowerLeftOfFirstLine'");
                    return position;
            }
        }

        static float GetCapped_autoLineBreakWidth_relToViewportWidth(Camera camera, bool autoLineBreakAtViewportBorder, float autoLineBreakWidth_relToViewportWidth, Vector2 position, DrawText.TextAnchorDXXL textAnchor, float zRotationDeg)
        {
            if (autoLineBreakAtViewportBorder)
            {
                Quaternion rotation_aroundForward = Quaternion.AngleAxis(zRotationDeg, Vector3.forward);
                Vector2 textDir_inAspectCorrected1by1SquareViewportSpace_normalized = rotation_aroundForward * Vector2.right;

                if (textAnchor == DrawText.TextAnchorDXXL.LowerLeft || textAnchor == DrawText.TextAnchorDXXL.LowerLeftOfFirstLine || textAnchor == DrawText.TextAnchorDXXL.MiddleLeft || textAnchor == DrawText.TextAnchorDXXL.UpperLeft)
                {
                    bool distanceToForwardViewportBorder_isValid;
                    float distanceToForwardViewportBorder_relToViewportWidth = GetDistanceToViewportBorder_relToViewportWidth(out distanceToForwardViewportBorder_isValid, camera, position, textDir_inAspectCorrected1by1SquareViewportSpace_normalized);
                    if (distanceToForwardViewportBorder_isValid)
                    {
                        if (UtilitiesDXXL_Math.ApproximatelyZero(autoLineBreakWidth_relToViewportWidth))
                        {
                            return distanceToForwardViewportBorder_relToViewportWidth;
                        }
                        else
                        {
                            return Mathf.Min(autoLineBreakWidth_relToViewportWidth, distanceToForwardViewportBorder_relToViewportWidth);
                        }
                    }
                    else
                    {
                        return autoLineBreakWidth_relToViewportWidth;
                    }
                }
                else
                {
                    if (textAnchor == DrawText.TextAnchorDXXL.LowerRight || textAnchor == DrawText.TextAnchorDXXL.LowerRightOfFirstLine || textAnchor == DrawText.TextAnchorDXXL.MiddleRight || textAnchor == DrawText.TextAnchorDXXL.UpperRight)
                    {
                        bool distanceToBackwardViewportBorder_isValid;
                        float distanceToBackwardViewportBorder_relToViewportWidth = GetDistanceToViewportBorder_relToViewportWidth(out distanceToBackwardViewportBorder_isValid, camera, position, -textDir_inAspectCorrected1by1SquareViewportSpace_normalized);
                        if (distanceToBackwardViewportBorder_isValid)
                        {
                            if (UtilitiesDXXL_Math.ApproximatelyZero(autoLineBreakWidth_relToViewportWidth))
                            {
                                return distanceToBackwardViewportBorder_relToViewportWidth;
                            }
                            else
                            {
                                return Mathf.Min(autoLineBreakWidth_relToViewportWidth, distanceToBackwardViewportBorder_relToViewportWidth);
                            }
                        }
                        else
                        {
                            return autoLineBreakWidth_relToViewportWidth;
                        }
                    }
                    else
                    {
                        //TextAnchorExt: Middle
                        bool distanceToForwardViewportBorder_isValid;
                        float distanceToForwardViewportBorder_relToViewportWidth = GetDistanceToViewportBorder_relToViewportWidth(out distanceToForwardViewportBorder_isValid, camera, position, textDir_inAspectCorrected1by1SquareViewportSpace_normalized);
                        bool distanceToBackwardViewportBorder_isValid;
                        float distanceToBackwardViewportBorder_relToViewportWidth = GetDistanceToViewportBorder_relToViewportWidth(out distanceToBackwardViewportBorder_isValid, camera, position, -textDir_inAspectCorrected1by1SquareViewportSpace_normalized);
                        if (distanceToForwardViewportBorder_isValid && distanceToBackwardViewportBorder_isValid)
                        {
                            if (UtilitiesDXXL_Math.ApproximatelyZero(autoLineBreakWidth_relToViewportWidth))
                            {
                                return Mathf.Min(2.0f * distanceToForwardViewportBorder_relToViewportWidth, 2.0f * distanceToBackwardViewportBorder_relToViewportWidth);
                            }
                            else
                            {
                                return UtilitiesDXXL_Math.Min(autoLineBreakWidth_relToViewportWidth, 2.0f * distanceToForwardViewportBorder_relToViewportWidth, 2.0f * distanceToBackwardViewportBorder_relToViewportWidth);
                            }
                        }
                        else
                        {
                            if (distanceToForwardViewportBorder_isValid)
                            {
                                return Mathf.Min(2.0f * distanceToForwardViewportBorder_relToViewportWidth, autoLineBreakWidth_relToViewportWidth);
                            }
                            else
                            {
                                if (distanceToBackwardViewportBorder_isValid)
                                {
                                    return Mathf.Min(autoLineBreakWidth_relToViewportWidth, 2.0f * distanceToBackwardViewportBorder_relToViewportWidth);
                                }
                                else
                                {
                                    return autoLineBreakWidth_relToViewportWidth;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                return autoLineBreakWidth_relToViewportWidth;
            }
        }

        static InternalDXXL_Line2D line2D_inNonSquareViewportSpace_forViewportBorderDistanceCalculation = new InternalDXXL_Line2D();
        static InternalDXXL_Line2D line2D_inAspectCorrected1by1SquareViewportSpace_forViewportBorderDistanceCalculation = new InternalDXXL_Line2D();
        static float GetDistanceToViewportBorder_relToViewportWidth(out bool distanceIsValid, Camera camera, Vector2 fromPosition, Vector2 textDir_inAspectCorrected1by1SquareViewportSpace_normalized)
        {
            if (Mathf.Abs(textDir_inAspectCorrected1by1SquareViewportSpace_normalized.y) < 0.0001f)
            {
                //dir is horizontal:
                if (textDir_inAspectCorrected1by1SquareViewportSpace_normalized.x > 0.0f)
                {
                    distanceIsValid = (fromPosition.x < 1.0f);
                    return (1.0f - fromPosition.x);
                }
                else
                {
                    distanceIsValid = (fromPosition.x > 0.0f);
                    return fromPosition.x;
                }
            }
            else
            {
                if (Mathf.Abs(textDir_inAspectCorrected1by1SquareViewportSpace_normalized.x) < 0.0001f)
                {
                    //dir is vertical:
                    if (textDir_inAspectCorrected1by1SquareViewportSpace_normalized.y > 0.0f)
                    {
                        distanceIsValid = (fromPosition.y < 1.0f);
                        return (1.0f - fromPosition.y) / camera.aspect;
                    }
                    else
                    {
                        distanceIsValid = (fromPosition.y > 0.0f);
                        return fromPosition.y / camera.aspect;
                    }
                }
                else
                {
                    //dir is NOT horizonal/vertical:
                    Vector2 textDir_inNonSquareViewportSpace = new Vector2(textDir_inAspectCorrected1by1SquareViewportSpace_normalized.x, textDir_inAspectCorrected1by1SquareViewportSpace_normalized.y * camera.aspect);
                    line2D_inNonSquareViewportSpace_forViewportBorderDistanceCalculation.Recalc_line_throughTwoPoints_notVertLineProof(fromPosition, fromPosition + textDir_inNonSquareViewportSpace);
                    if (textDir_inAspectCorrected1by1SquareViewportSpace_normalized.y > 0.0f)
                    {
                        //dir is skewed upward:
                        Vector2 intersectionWithUpperViewportBorder = new Vector2(line2D_inNonSquareViewportSpace_forViewportBorderDistanceCalculation.GetXatY(1.0f), 1.0f);
                        if (0.0f <= intersectionWithUpperViewportBorder.x && intersectionWithUpperViewportBorder.x <= 1.0f)
                        {
                            //line intersects with upper viewport border:
                            distanceIsValid = (fromPosition.y < 1.0f);
                            Vector3 toNearestBoarder = intersectionWithUpperViewportBorder - fromPosition;
                            toNearestBoarder.y = toNearestBoarder.y / camera.aspect;
                            return toNearestBoarder.magnitude;
                        }
                        else
                        {
                            //line intersects with a side border:
                            line2D_inAspectCorrected1by1SquareViewportSpace_forViewportBorderDistanceCalculation.Recalc_line_throughTwoPoints_notVertLineProof(fromPosition, fromPosition + textDir_inAspectCorrected1by1SquareViewportSpace_normalized);
                            if (textDir_inAspectCorrected1by1SquareViewportSpace_normalized.x > 0.0f)
                            {
                                //dir to upward-right:
                                Vector2 intersectionWithRightViewportBorder = new Vector2(1.0f, line2D_inAspectCorrected1by1SquareViewportSpace_forViewportBorderDistanceCalculation.GetYatX(1.0f));
                                distanceIsValid = ((fromPosition.x < 1.0f) && (fromPosition.y < 1.0f));
                                Vector3 toNearestBoarder = intersectionWithRightViewportBorder - fromPosition;
                                return toNearestBoarder.magnitude;
                            }
                            else
                            {
                                //dir to upward-left:
                                Vector2 intersectionWithLeftViewportBorder = new Vector2(0.0f, line2D_inAspectCorrected1by1SquareViewportSpace_forViewportBorderDistanceCalculation.GetYatX(0.0f));
                                distanceIsValid = ((fromPosition.x > 0.0f) && (fromPosition.y < 1.0f));
                                Vector3 toNearestBoarder = intersectionWithLeftViewportBorder - fromPosition;
                                return toNearestBoarder.magnitude;
                            }
                        }
                    }
                    else
                    {
                        //dir is skewed downward:
                        Vector2 intersectionWithLowerViewportBorder = new Vector2(line2D_inNonSquareViewportSpace_forViewportBorderDistanceCalculation.GetXatY(0.0f), 0.0f);
                        if (0.0f <= intersectionWithLowerViewportBorder.x && intersectionWithLowerViewportBorder.x <= 1.0f)
                        {
                            //line intersects with lower viewport border:
                            distanceIsValid = (fromPosition.y > 0.0f);
                            Vector3 toNearestBoarder = intersectionWithLowerViewportBorder - fromPosition;
                            toNearestBoarder.y = toNearestBoarder.y / camera.aspect;
                            return toNearestBoarder.magnitude;
                        }
                        else
                        {
                            //line intersects with a side border:
                            line2D_inAspectCorrected1by1SquareViewportSpace_forViewportBorderDistanceCalculation.Recalc_line_throughTwoPoints_notVertLineProof(fromPosition, fromPosition + textDir_inAspectCorrected1by1SquareViewportSpace_normalized);
                            if (textDir_inAspectCorrected1by1SquareViewportSpace_normalized.x > 0.0f)
                            {
                                //dir to downward-right:
                                Vector2 intersectionWithRightViewportBorder = new Vector2(1.0f, line2D_inAspectCorrected1by1SquareViewportSpace_forViewportBorderDistanceCalculation.GetYatX(1.0f));
                                distanceIsValid = ((fromPosition.x < 1.0f) && (fromPosition.y > 0.0f));
                                Vector3 toNearestBoarder = intersectionWithRightViewportBorder - fromPosition;
                                return toNearestBoarder.magnitude;
                            }
                            else
                            {
                                //dir to downward-left:
                                Vector2 intersectionWithLeftViewportBorder = new Vector2(0.0f, line2D_inAspectCorrected1by1SquareViewportSpace_forViewportBorderDistanceCalculation.GetYatX(0.0f));
                                distanceIsValid = ((fromPosition.x > 0.0f) && (fromPosition.y > 0.0f));
                                Vector3 toNearestBoarder = intersectionWithLeftViewportBorder - fromPosition;
                                return toNearestBoarder.magnitude;
                            }
                        }
                    }
                }
            }
        }

        static InternalDXXL_Plane planeInWhichTextLies = new InternalDXXL_Plane();
        static LineAnimationProgress unusedLineAnimProgress = new LineAnimationProgress();
        static void DrawEncapsulatingBox(bool skipDraw, float size, float scaleFactorFromForceWholeTextBlockWidth, Color color, Vector3 textDirNormalized, Vector3 textUpNormalized, DrawBasics.LineStyle enclosingBoxLineStyle, float enclosingBox_paddingOffset_relToTextSize, float enclosingBox_lineWidth_relToTextSize, float durationInSec, bool hiddenByNearerObjects)
        {
            if (enclosingBoxLineStyle != DrawBasics.LineStyle.invisible)
            {
                float used_size = size * scaleFactorFromForceWholeTextBlockWidth;
                planeInWhichTextLies.Recreate(DrawText.parsedTextSpecs.lowLeftPos_ofFirstLine, DrawText.parsedTextSpecs.lowLeftPos_ofFirstLine + textDirNormalized, DrawText.parsedTextSpecs.lowLeftPos_ofFirstLine + textUpNormalized);
                float patternScaleFactor = used_size * 10.0f;
                float enclosingBox_paddingOffset_worldSpace = enclosingBox_paddingOffset_relToTextSize * used_size;
                float enclosingBox_lineWidth_worldSpace;
                if (UtilitiesDXXL_Math.ApproximatelyZero(enclosingBox_lineWidth_relToTextSize))
                {
                    enclosingBox_lineWidth_worldSpace = 0.0f;
                }
                else
                {
                    enclosingBox_lineWidth_worldSpace = enclosingBox_lineWidth_relToTextSize * used_size;
                }
                enclosingBox_lineWidth_worldSpace = UtilitiesDXXL_Math.AbsNonZeroValue(enclosingBox_lineWidth_worldSpace);
                float halfLineWidth_worldSpace = 0.5f * enclosingBox_lineWidth_worldSpace;
                float approximateLengthOfLongestBoxEdge = Math.Max(Mathf.Abs(DrawText.parsedTextSpecs.widthOfLongestLine + 2.0f * enclosingBox_paddingOffset_worldSpace + enclosingBox_lineWidth_worldSpace), Mathf.Abs(DrawText.parsedTextSpecs.height_wholeTextBlock + 2.0f * enclosingBox_paddingOffset_worldSpace + enclosingBox_lineWidth_worldSpace));
                approximateLengthOfLongestBoxEdge = Mathf.Max(approximateLengthOfLongestBoxEdge, used_size);
                float amplitude;
                if (enclosingBoxLineStyle == DrawBasics.LineStyle.solid) { patternScaleFactor = 1.0f; } //-> omits a warning for small lines, that doesn't apply for solid lines
                UtilitiesDXXL_LineStyles.RefillListOfSubLines(Vector3.zero, new Vector3(approximateLengthOfLongestBoxEdge, 0.0f, 0.0f), enclosingBoxLineStyle, patternScaleFactor, enclosingBox_lineWidth_worldSpace, out amplitude, Vector3.up, 0.0f, ref unusedLineAnimProgress, false, false, 1.0f); //<- Is only for obtaining the "amplitude", but not for drawing
                amplitude = UtilitiesDXXL_Math.AbsNonZeroValue(amplitude);
                float halfAmplitude = 0.5f * amplitude;

                Vector3 offsetForMiteredCornerStyle_towardsRight = textDirNormalized * halfLineWidth_worldSpace;
                Vector3 offsetForMiteredCornerStyle_towardsUp = textUpNormalized * halfLineWidth_worldSpace;
                float additionalShiftThatExceedsTheTightParsedSpecsBoundingBox = enclosingBox_paddingOffset_worldSpace + halfLineWidth_worldSpace + halfAmplitude;
                Vector3 lowLeftPosOfFirstLine_shiftedToUpperLine = DrawText.parsedTextSpecs.lowLeftPos_ofFirstLine + textUpNormalized * ((0.8f * relLineDistance * DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine) + additionalShiftThatExceedsTheTightParsedSpecsBoundingBox);
                Vector3 lowLeftPosOfFirstLine_shiftedToLowerLine = DrawText.parsedTextSpecs.lowLeftPos_ofFirstLine - textUpNormalized * (DrawText.parsedTextSpecs.height_lowFirstLine_toLowLastLine + additionalShiftThatExceedsTheTightParsedSpecsBoundingBox + 0.2f * relLineDistance * used_size);

                Vector3 upperLeftCorner_exclOffsetForMiteredCornerStyle = lowLeftPosOfFirstLine_shiftedToUpperLine - textDirNormalized * (additionalShiftThatExceedsTheTightParsedSpecsBoundingBox + 0.1f * relLineDistance * used_size);
                Vector3 upperRightCorner_exclOffsetForMiteredCornerStyle = lowLeftPosOfFirstLine_shiftedToUpperLine + textDirNormalized * (DrawText.parsedTextSpecs.widthOfLongestLine + additionalShiftThatExceedsTheTightParsedSpecsBoundingBox + 0.1f * relLineDistance * used_size);
                Vector3 lowerLeftCorner_exclOffsetForMiteredCornerStyle = lowLeftPosOfFirstLine_shiftedToLowerLine - textDirNormalized * (additionalShiftThatExceedsTheTightParsedSpecsBoundingBox + 0.1f * relLineDistance * used_size);
                Vector3 lowerRightCorner_exclOffsetForMiteredCornerStyle = lowLeftPosOfFirstLine_shiftedToLowerLine + textDirNormalized * (DrawText.parsedTextSpecs.widthOfLongestLine + additionalShiftThatExceedsTheTightParsedSpecsBoundingBox + 0.1f * relLineDistance * used_size);

                if (skipDraw == false)
                {
                    UtilitiesDXXL_DrawBasics.Line(upperLeftCorner_exclOffsetForMiteredCornerStyle - offsetForMiteredCornerStyle_towardsRight, upperRightCorner_exclOffsetForMiteredCornerStyle + offsetForMiteredCornerStyle_towardsRight, color, enclosingBox_lineWidth_worldSpace, null, enclosingBoxLineStyle, patternScaleFactor, 0.0f, null, planeInWhichTextLies, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    UtilitiesDXXL_DrawBasics.Line(lowerLeftCorner_exclOffsetForMiteredCornerStyle - offsetForMiteredCornerStyle_towardsRight, lowerRightCorner_exclOffsetForMiteredCornerStyle + offsetForMiteredCornerStyle_towardsRight, color, enclosingBox_lineWidth_worldSpace, null, enclosingBoxLineStyle, patternScaleFactor, 0.0f, null, planeInWhichTextLies, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    UtilitiesDXXL_DrawBasics.Line(upperLeftCorner_exclOffsetForMiteredCornerStyle + offsetForMiteredCornerStyle_towardsUp, lowerLeftCorner_exclOffsetForMiteredCornerStyle - offsetForMiteredCornerStyle_towardsUp, color, enclosingBox_lineWidth_worldSpace, null, enclosingBoxLineStyle, patternScaleFactor, 0.0f, null, planeInWhichTextLies, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                    UtilitiesDXXL_DrawBasics.Line(upperRightCorner_exclOffsetForMiteredCornerStyle + offsetForMiteredCornerStyle_towardsUp, lowerRightCorner_exclOffsetForMiteredCornerStyle - offsetForMiteredCornerStyle_towardsUp, color, enclosingBox_lineWidth_worldSpace, null, enclosingBoxLineStyle, patternScaleFactor, 0.0f, null, planeInWhichTextLies, true, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f);
                }

                DrawText.parsedTextSpecs.lowLeftPos_ofEnclosingBox = lowerLeftCorner_exclOffsetForMiteredCornerStyle;
                DrawText.parsedTextSpecs.lowRightPos_ofEnclosingBox = lowerRightCorner_exclOffsetForMiteredCornerStyle;
                DrawText.parsedTextSpecs.upperLeftPos_ofEnclosingBox = upperLeftCorner_exclOffsetForMiteredCornerStyle;
                DrawText.parsedTextSpecs.upperRightPos_ofEnclosingBox = upperRightCorner_exclOffsetForMiteredCornerStyle;
            }
        }

        static void ConvertParsedSpecs_toViewportSpace(Camera camera, Vector2 position, Vector3 pos_worldSpace, Vector3 textDir_worldSpace_normalized, Vector3 textUp_worldSpace_normalized)
        {
            Vector3 endPos_ofLongestLine_worldSpace = pos_worldSpace + textDir_worldSpace_normalized * DrawText.parsedTextSpecs.widthOfLongestLine;
            Vector2 endPos_ofLongestLine_nonSquareViewportSpace0to1 = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(camera, endPos_ofLongestLine_worldSpace, false);
            DrawText.parsedTextSpecs.widthOfLongestLine = (endPos_ofLongestLine_nonSquareViewportSpace0to1 - position).magnitude; //=magnitude in (warped) nonSquareViewportSpace

            Vector3 endPos_ofBiggestCharAfterStartPosIfWroteUpwards_worldSpace = pos_worldSpace + camera.transform.up * DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine;
            Vector2 endPos_ofBiggestCharAfterStartPosIfWroteUpwards_nonSquareViewportSpace0to1 = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(camera, endPos_ofBiggestCharAfterStartPosIfWroteUpwards_worldSpace, false);
            DrawText.parsedTextSpecs.sizeOfBiggestCharInFirstLine = (endPos_ofBiggestCharAfterStartPosIfWroteUpwards_nonSquareViewportSpace0to1 - position).magnitude; //=magnitude in (warped) nonSquareViewportSpace

            //slightly imprecise due to the transformation happening at posible shifted positions (could be improved by considering 'textAnchorPos' instead of 'pos_worldSpace'):
            Vector3 highestPos_worldSpace = pos_worldSpace + textUp_worldSpace_normalized * DrawText.parsedTextSpecs.height_wholeTextBlock;
            Vector2 highestPos_nonSquareViewportSpace0to1 = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(camera, highestPos_worldSpace, false);
            DrawText.parsedTextSpecs.height_wholeTextBlock = (highestPos_nonSquareViewportSpace0to1 - position).magnitude; //=magnitude in (warped) nonSquareViewportSpace

            //slightly imprecise due to the transformation happening at posible shifted positions (could be improved by considering 'textAnchorPos' instead of 'pos_worldSpace'):
            Vector3 highestPosLines_worldSpace = pos_worldSpace + textUp_worldSpace_normalized * DrawText.parsedTextSpecs.height_lowFirstLine_toLowLastLine;
            Vector2 highestPosLines_nonSquareViewportSpace0to1 = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(camera, highestPosLines_worldSpace, false);
            DrawText.parsedTextSpecs.height_lowFirstLine_toLowLastLine = (highestPosLines_nonSquareViewportSpace0to1 - position).magnitude; //=magnitude in (warped) nonSquareViewportSpace

            DrawText.parsedTextSpecs.lowLeftPos_ofFirstLine = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(camera, DrawText.parsedTextSpecs.lowLeftPos_ofFirstLine, false);
        }

        static void ConvertParsedSpecsOnCircle_toViewportSpace(Camera camera, Vector3 circleCenterPosition_worldSpace, Vector3 textsInitialUp_worldSpace_normalized, float radius_worldSpace, Vector2 approxStartPos_inAspectCorrected1by1SquareViewportSpace)
        {
            Vector3 startPos_worldSpace = circleCenterPosition_worldSpace + textsInitialUp_worldSpace_normalized * radius_worldSpace;
            //using "camera.transform.up" here means: result is relative to viewport height:
            Vector3 endPos_ofBiggestCharAfterStartPosIfWroteUpwards_worldSpace = startPos_worldSpace + camera.transform.up * DrawText.parsedTextOnCircleSpecs.sizeOfBiggestCharInFirstLine;
            Vector2 endPos_ofBiggestCharAfterStartPosIfWroteUpwards_viewportSpace0to1 = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(camera, endPos_ofBiggestCharAfterStartPosIfWroteUpwards_worldSpace, false);
            DrawText.parsedTextOnCircleSpecs.sizeOfBiggestCharInFirstLine = (endPos_ofBiggestCharAfterStartPosIfWroteUpwards_viewportSpace0to1 - approxStartPos_inAspectCorrected1by1SquareViewportSpace).magnitude;
        }

        static DrawText.AutomaticTextOrientation automaticTextOrientation_before;
        public static void Set_automaticTextOrientation_reversible(DrawText.AutomaticTextOrientation new_automaticTextOrientation)
        {
            automaticTextOrientation_before = DrawText.automaticTextOrientation;
            DrawText.automaticTextOrientation = new_automaticTextOrientation;
        }
        public static void Reverse_automaticTextOrientation()
        {
            DrawText.automaticTextOrientation = automaticTextOrientation_before;
        }

    }

}
