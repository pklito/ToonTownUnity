namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class DrawText
    {
        public enum TextAnchorDXXL { UpperLeft, UpperCenter, UpperRight, MiddleLeft, MiddleCenter, MiddleRight, LowerLeft, LowerCenter, LowerRight, LowerLeftOfFirstLine, LowerCenterOfFirstLine, LowerRightOfFirstLine }
        public enum TextAnchorCircledDXXL
        {
            LowerLeftOfFirstLine,
            LowerLeftOfWholeTextBlock
        }

        public enum AutomaticTextOrientation
        {
            screen,
            screen_butVerticalInWorldSpace,
            xyPlane,
            xzPlane,
            zyPlane
        }
        public static AutomaticTextOrientation automaticTextOrientation = AutomaticTextOrientation.screen;

        public static ParsedTextSpecs parsedTextSpecs = new ParsedTextSpecs(); //is filled with the values of the most recently called WriteText-function.
        public static ParsedTextOnCircleSpecs parsedTextOnCircleSpecs = new ParsedTextOnCircleSpecs(); //is filled with the values of the most recently called WriteTextCircled-function.

        public static void WriteScreenspace(string text, Vector3 position_in3DWorldspace, Color color, float size_relToViewportHeight, Vector2 textDirection, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = 0.0f, bool autoLineBreakAtViewportBorder = true, float autoLineBreakWidth_relToViewportWidth = 0.0f, bool autoFlipTextToPreventUpsideDown = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteScreenspace") == false) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextScreenspace_3Dpos_dirViaVec_cam.Add(new TextScreenspace_3Dpos_dirViaVec_cam(automaticallyFoundCamera, text, position_in3DWorldspace, color, size_relToViewportHeight, textDirection, textAnchor, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            WriteScreenspace(text, position_in2DViewportSpace, color, size_relToViewportHeight, textDirection, textAnchor, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec);
        }

        public static void WriteScreenspace(Camera screenCamera, string text, Vector3 position_in3DWorldspace, Color color, float size_relToViewportHeight, Vector2 textDirection, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = 0.0f, bool autoLineBreakAtViewportBorder = true, float autoLineBreakWidth_relToViewportWidth = 0.0f, bool autoFlipTextToPreventUpsideDown = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextScreenspace_3Dpos_dirViaVec_cam.Add(new TextScreenspace_3Dpos_dirViaVec_cam(screenCamera, text, position_in3DWorldspace, color, size_relToViewportHeight, textDirection, textAnchor, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            WriteScreenspace(screenCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, textDirection, textAnchor, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec);
        }

        public static void WriteScreenspace(string text, Vector2 position_in2DViewportSpace, Color color, float size_relToViewportHeight, Vector2 textDirection, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = 0.0f, bool autoLineBreakAtViewportBorder = true, float autoLineBreakWidth_relToViewportWidth = 0.0f, bool autoFlipTextToPreventUpsideDown = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteScreenspace") == false) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextScreenspace_2Dpos_dirViaVec_cam.Add(new TextScreenspace_2Dpos_dirViaVec_cam(automaticallyFoundCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, textDirection, textAnchor, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec));
                return;
            }

            WriteScreenspace(automaticallyFoundCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, textDirection, textAnchor, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec);
        }

        public static void WriteScreenspace(Camera screenCamera, string text, Vector2 position_in2DViewportSpace, Color color, float size_relToViewportHeight, Vector2 textDirection, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = 0.0f, bool autoLineBreakAtViewportBorder = true, float autoLineBreakWidth_relToViewportWidth = 0.0f, bool autoFlipTextToPreventUpsideDown = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextScreenspace_2Dpos_dirViaVec_cam.Add(new TextScreenspace_2Dpos_dirViaVec_cam(screenCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, textDirection, textAnchor, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec));
                return;
            }

            WriteScreenspaceFramed(screenCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, textDirection, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec);
        }

        public static void WriteScreenspaceFramed(string text, Vector3 position_in3DWorldspace, Color color, float size_relToViewportHeight, Vector2 textDirection, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle enclosingBoxLineStyle = DrawBasics.LineStyle.solid, float enclosingBox_lineWidth_relToTextSize = 0.0f, float enclosingBox_paddingSize_relToTextSize = 0.0f, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = 0.0f, bool autoLineBreakAtViewportBorder = true, float autoLineBreakWidth_relToViewportWidth = 0.0f, bool autoFlipTextToPreventUpsideDown = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteScreenspaceFramed") == false) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam.Add(new TextScreenspaceFramed_3Dpos_dirViaVec_cam(automaticallyFoundCamera, text, position_in3DWorldspace, color, size_relToViewportHeight, textDirection, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            WriteScreenspaceFramed(text, position_in2DViewportSpace, color, size_relToViewportHeight, textDirection, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec);
        }
        public static void WriteScreenspaceFramed(Camera screenCamera, string text, Vector3 position_in3DWorldspace, Color color, float size_relToViewportHeight, Vector2 textDirection, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle enclosingBoxLineStyle = DrawBasics.LineStyle.solid, float enclosingBox_lineWidth_relToTextSize = 0.0f, float enclosingBox_paddingSize_relToTextSize = 0.0f, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = 0.0f, bool autoLineBreakAtViewportBorder = true, float autoLineBreakWidth_relToViewportWidth = 0.0f, bool autoFlipTextToPreventUpsideDown = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam.Add(new TextScreenspaceFramed_3Dpos_dirViaVec_cam(screenCamera, text, position_in3DWorldspace, color, size_relToViewportHeight, textDirection, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            WriteScreenspaceFramed(screenCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, textDirection, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec);
        }

        public static void WriteScreenspaceFramed(string text, Vector2 position_in2DViewportSpace, Color color, float size_relToViewportHeight, Vector2 textDirection, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle enclosingBoxLineStyle = DrawBasics.LineStyle.solid, float enclosingBox_lineWidth_relToTextSize = 0.0f, float enclosingBox_paddingSize_relToTextSize = 0.0f, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = 0.0f, bool autoLineBreakAtViewportBorder = true, float autoLineBreakWidth_relToViewportWidth = 0.0f, bool autoFlipTextToPreventUpsideDown = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteScreenspaceFramed") == false) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam.Add(new TextScreenspaceFramed_2Dpos_dirViaVec_cam(automaticallyFoundCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, textDirection, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec));
                return;
            }

            WriteScreenspaceFramed(automaticallyFoundCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, textDirection, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec);
        }
        public static void WriteScreenspaceFramed(Camera screenCamera, string text, Vector2 position_in2DViewportSpace, Color color, float size_relToViewportHeight, Vector2 textDirection, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle enclosingBoxLineStyle = DrawBasics.LineStyle.solid, float enclosingBox_lineWidth_relToTextSize = 0.0f, float enclosingBox_paddingSize_relToTextSize = 0.0f, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = 0.0f, bool autoLineBreakAtViewportBorder = true, float autoLineBreakWidth_relToViewportWidth = 0.0f, bool autoFlipTextToPreventUpsideDown = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(screenCamera, "screenCamera")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam.Add(new TextScreenspaceFramed_2Dpos_dirViaVec_cam(screenCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, textDirection, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec));
                return;
            }

            bool skipDraw = false; //-> The corresponding method in "TextUtilitiesDXXL" has the same parameters and can be used interchangeably, except that it has the additional parameter "skipDraw". This can be set to 'true' if only the 'parsedTextSpecs' should be filled (e.g. as decision base for the final placement of the text via an additinal DrawText-call).
            UtilitiesDXXL_Text.WriteScreenSpace(screenCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, textDirection, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec, skipDraw);
        }

        public static void WriteScreenspace(string text, Vector3 position_in3DWorldspace, Color color = default(Color), float size_relToViewportHeight = 0.025f, float zRotationDegCC = 0.0f, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = 0.0f, bool autoLineBreakAtViewportBorder = true, float autoLineBreakWidth_relToViewportWidth = 0.0f, bool autoFlipTextToPreventUpsideDown = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteScreenspace") == false) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextScreenspace_3Dpos_dirViaAngle_cam.Add(new TextScreenspace_3Dpos_dirViaAngle_cam(automaticallyFoundCamera, text, position_in3DWorldspace, color, size_relToViewportHeight, zRotationDegCC, textAnchor, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            WriteScreenspace(text, position_in2DViewportSpace, color, size_relToViewportHeight, zRotationDegCC, textAnchor, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec);
        }
        public static void WriteScreenspace(Camera screenCamera, string text, Vector3 position_in3DWorldspace, Color color = default(Color), float size_relToViewportHeight = 0.025f, float zRotationDegCC = 0.0f, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = 0.0f, bool autoLineBreakAtViewportBorder = true, float autoLineBreakWidth_relToViewportWidth = 0.0f, bool autoFlipTextToPreventUpsideDown = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextScreenspace_3Dpos_dirViaAngle_cam.Add(new TextScreenspace_3Dpos_dirViaAngle_cam(screenCamera, text, position_in3DWorldspace, color, size_relToViewportHeight, zRotationDegCC, textAnchor, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            WriteScreenspace(screenCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, zRotationDegCC, textAnchor, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec);
        }

        public static void WriteScreenspace(string text, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), float size_relToViewportHeight = 0.025f, float zRotationDegCC = 0.0f, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = 0.0f, bool autoLineBreakAtViewportBorder = true, float autoLineBreakWidth_relToViewportWidth = 0.0f, bool autoFlipTextToPreventUpsideDown = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteScreenspace") == false) { return; }

            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextScreenspace_2Dpos_dirViaAngle_cam.Add(new TextScreenspace_2Dpos_dirViaAngle_cam(automaticallyFoundCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, zRotationDegCC, textAnchor, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec));
                return;
            }

            WriteScreenspace(automaticallyFoundCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, zRotationDegCC, textAnchor, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec);
        }
        public static void WriteScreenspace(Camera screenCamera, string text, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), float size_relToViewportHeight = 0.025f, float zRotationDegCC = 0.0f, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = 0.0f, bool autoLineBreakAtViewportBorder = true, float autoLineBreakWidth_relToViewportWidth = 0.0f, bool autoFlipTextToPreventUpsideDown = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextScreenspace_2Dpos_dirViaAngle_cam.Add(new TextScreenspace_2Dpos_dirViaAngle_cam(screenCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, zRotationDegCC, textAnchor, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec));
                return;
            }

            WriteScreenspaceFramed(screenCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, zRotationDegCC, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec);
        }

        public static void WriteScreenspaceFramed(string text, Vector3 position_in3DWorldspace, Color color = default(Color), float size_relToViewportHeight = 0.025f, float zRotationDegCC = 0.0f, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle enclosingBoxLineStyle = DrawBasics.LineStyle.solid, float enclosingBox_lineWidth_relToTextSize = 0.0f, float enclosingBox_paddingSize_relToTextSize = 0.0f, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = 0.0f, bool autoLineBreakAtViewportBorder = true, float autoLineBreakWidth_relToViewportWidth = 0.0f, bool autoFlipTextToPreventUpsideDown = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteScreenspaceFramed") == false) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam.Add(new TextScreenspaceFramed_3Dpos_dirViaAngle_cam(automaticallyFoundCamera, text, position_in3DWorldspace, color, size_relToViewportHeight, zRotationDegCC, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            WriteScreenspaceFramed(text, position_in2DViewportSpace, color, size_relToViewportHeight, zRotationDegCC, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec);
        }
        public static void WriteScreenspaceFramed(Camera screenCamera, string text, Vector3 position_in3DWorldspace, Color color = default(Color), float size_relToViewportHeight = 0.025f, float zRotationDegCC = 0.0f, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle enclosingBoxLineStyle = DrawBasics.LineStyle.solid, float enclosingBox_lineWidth_relToTextSize = 0.0f, float enclosingBox_paddingSize_relToTextSize = 0.0f, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = 0.0f, bool autoLineBreakAtViewportBorder = true, float autoLineBreakWidth_relToViewportWidth = 0.0f, bool autoFlipTextToPreventUpsideDown = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam.Add(new TextScreenspaceFramed_3Dpos_dirViaAngle_cam(screenCamera, text, position_in3DWorldspace, color, size_relToViewportHeight, zRotationDegCC, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            WriteScreenspaceFramed(screenCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, zRotationDegCC, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec);
        }

        public static void WriteScreenspaceFramed(string text, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), float size_relToViewportHeight = 0.025f, float zRotationDegCC = 0.0f, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle enclosingBoxLineStyle = DrawBasics.LineStyle.solid, float enclosingBox_lineWidth_relToTextSize = 0.0f, float enclosingBox_paddingSize_relToTextSize = 0.0f, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = 0.0f, bool autoLineBreakAtViewportBorder = true, float autoLineBreakWidth_relToViewportWidth = 0.0f, bool autoFlipTextToPreventUpsideDown = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteScreenspaceFramed") == false) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam.Add(new TextScreenspaceFramed_2Dpos_dirViaAngle_cam(automaticallyFoundCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, zRotationDegCC, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec));
                return;
            }

            WriteScreenspaceFramed(automaticallyFoundCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, zRotationDegCC, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec);
        }
        public static void WriteScreenspaceFramed(Camera screenCamera, string text, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), float size_relToViewportHeight = 0.025f, float zRotationDegCC = 0.0f, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle enclosingBoxLineStyle = DrawBasics.LineStyle.solid, float enclosingBox_lineWidth_relToTextSize = 0.0f, float enclosingBox_paddingSize_relToTextSize = 0.0f, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = 0.0f, bool autoLineBreakAtViewportBorder = true, float autoLineBreakWidth_relToViewportWidth = 0.0f, bool autoFlipTextToPreventUpsideDown = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(screenCamera, "screenCamera")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam.Add(new TextScreenspaceFramed_2Dpos_dirViaAngle_cam(screenCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, zRotationDegCC, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec));
                return;
            }

            bool skipDraw = false; //-> The corresponding method in "TextUtilitiesDXXL" has the same parameters and can be used interchangeably, except that it has the additional parameter "skipDraw". This can be set to 'true' if only the 'parsedTextSpecs' should be filled (e.g. as decision base for the final placement of the text via an additinal DrawText-call).
            UtilitiesDXXL_Text.WriteScreenspace(screenCamera, text, position_in2DViewportSpace, color, size_relToViewportHeight, zRotationDegCC, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, autoLineBreakAtViewportBorder, autoLineBreakWidth_relToViewportWidth, autoFlipTextToPreventUpsideDown, durationInSec, skipDraw);
        }

        public static void Write2D(string text, Vector2 position, Color color = default(Color), float size = 0.1f, Vector2 textDirection = default(Vector2), TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, float custom_zPos = float.PositiveInfinity, float forceTextBlockEnlargementToThisMinWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth = 0.0f, float autoLineBreakWidth = 0.0f, bool autoFlipToPreventMirrorInverted = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Write2DFramed(text, position, color, size, textDirection, textAnchor, custom_zPos, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, forceTextBlockEnlargementToThisMinWidth, forceRestrictTextBlockSizeToThisMaxTextWidth, autoLineBreakWidth, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects);
        }

        public static void Write2DFramed(string text, Vector2 position, Color color = default(Color), float size = 0.1f, Vector2 textDirection = default(Vector2), TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, float custom_zPos = float.PositiveInfinity, DrawBasics.LineStyle enclosingBoxLineStyle = DrawBasics.LineStyle.solid, float enclosingBox_lineWidth_relToTextSize = 0.0f, float enclosingBox_paddingSize_relToTextSize = 0.0f, float forceTextBlockEnlargementToThisMinWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth = 0.0f, float autoLineBreakWidth = 0.0f, bool autoFlipToPreventMirrorInverted = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Text.Write2DFramed(text, position, color, size, textDirection, textAnchor, custom_zPos, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth, forceRestrictTextBlockSizeToThisMaxTextWidth, autoLineBreakWidth, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects);
        }

        public static void Write2D(string text, Vector2 position, Color color, float size, float zRotationDegCC, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, float custom_zPos = float.PositiveInfinity, float forceTextBlockEnlargementToThisMinWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth = 0.0f, float autoLineBreakWidth = 0.0f, bool autoFlipToPreventMirrorInverted = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Write2DFramed(text, position, color, size, zRotationDegCC, textAnchor, custom_zPos, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, forceTextBlockEnlargementToThisMinWidth, forceRestrictTextBlockSizeToThisMaxTextWidth, autoLineBreakWidth, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects);
        }

        public static void Write2DFramed(string text, Vector2 position, Color color, float size, float zRotationDegCC, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, float custom_zPos = float.PositiveInfinity, DrawBasics.LineStyle enclosingBoxLineStyle = DrawBasics.LineStyle.solid, float enclosingBox_lineWidth_relToTextSize = 0.0f, float enclosingBox_paddingSize_relToTextSize = 0.0f, float forceTextBlockEnlargementToThisMinWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth = 0.0f, float autoLineBreakWidth = 0.0f, bool autoFlipToPreventMirrorInverted = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Text.Write2DFramed(text, position, color, size, zRotationDegCC, textAnchor, custom_zPos, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth, forceRestrictTextBlockSizeToThisMaxTextWidth, autoLineBreakWidth, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects);
        }

        public static void Write(string text, Vector3 position, Color color, float size, Quaternion rotation, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, float forceTextBlockEnlargementToThisMinWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth = 0.0f, float autoLineBreakWidth = 0.0f, bool autoFlipToPreventMirrorInverted = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            WriteFramed(text, position, color, size, rotation, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, forceTextBlockEnlargementToThisMinWidth, forceRestrictTextBlockSizeToThisMaxTextWidth, autoLineBreakWidth, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteFramed(string text, Vector3 position, Color color, float size, Quaternion rotation, TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle enclosingBoxLineStyle = DrawBasics.LineStyle.solid, float enclosingBox_lineWidth_relToTextSize = 0.0f, float enclosingBox_paddingSize_relToTextSize = 0.0f, float forceTextBlockEnlargementToThisMinWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth = 0.0f, float autoLineBreakWidth = 0.0f, bool autoFlipToPreventMirrorInverted = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Text.WriteFramed(text, position, color, size, rotation, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth, forceRestrictTextBlockSizeToThisMaxTextWidth, autoLineBreakWidth, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects);
        }

        public static void Write(string text, Vector3 position, Color color = default(Color), float size = 0.1f, Vector3 textDirection = default(Vector3), Vector3 textUp = default(Vector3), TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, float forceTextBlockEnlargementToThisMinWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth = 0.0f, float autoLineBreakWidth = 0.0f, bool autoFlipToPreventMirrorInverted = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            WriteFramed(text, position, color, size, textDirection, textUp, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, forceTextBlockEnlargementToThisMinWidth, forceRestrictTextBlockSizeToThisMaxTextWidth, autoLineBreakWidth, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects);
        }
        public static void WriteFramed(string text, Vector3 position, Color color = default(Color), float size = 0.1f, Vector3 textDirection = default(Vector3), Vector3 textUp = default(Vector3), TextAnchorDXXL textAnchor = TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle enclosingBoxLineStyle = DrawBasics.LineStyle.solid, float enclosingBox_lineWidth_relToTextSize = 0.0f, float enclosingBox_paddingSize_relToTextSize = 0.0f, float forceTextBlockEnlargementToThisMinWidth = 0.0f, float forceRestrictTextBlockSizeToThisMaxTextWidth = 0.0f, float autoLineBreakWidth = 0.0f, bool autoFlipToPreventMirrorInverted = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Text.WriteFramed(text, position, color, size, textDirection, textUp, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, forceTextBlockEnlargementToThisMinWidth, forceRestrictTextBlockSizeToThisMaxTextWidth, autoLineBreakWidth, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteOnCircleScreenspace(string text, Vector2 textStartPos, Vector2 circleCenterPosition, Color color = default(Color), float size_relToViewportHeight = 0.025f, TextAnchorCircledDXXL textAnchor = TextAnchorCircledDXXL.LowerLeftOfFirstLine, float autoLineBreakAngleDeg = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteOnCircleScreenspace") == false) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextOnCircleScreenspace_viaStartPos_cam.Add(new TextOnCircleScreenspace_viaStartPos_cam(automaticallyFoundCamera, text, textStartPos, circleCenterPosition, color, size_relToViewportHeight, textAnchor, autoLineBreakAngleDeg, durationInSec));
                return;
            }

            WriteOnCircleScreenspace(automaticallyFoundCamera, text, textStartPos, circleCenterPosition, color, size_relToViewportHeight, textAnchor, autoLineBreakAngleDeg, durationInSec);
        }

        public static void WriteOnCircleScreenspace(Camera screenCamera, string text, Vector2 textStartPos, Vector2 circleCenterPosition, Color color = default(Color), float size_relToViewportHeight = 0.025f, TextAnchorCircledDXXL textAnchor = TextAnchorCircledDXXL.LowerLeftOfFirstLine, float autoLineBreakAngleDeg = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(textStartPos, "textStartPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenterPosition, "circleCenterPosition")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextOnCircleScreenspace_viaStartPos_cam.Add(new TextOnCircleScreenspace_viaStartPos_cam(screenCamera, text, textStartPos, circleCenterPosition, color, size_relToViewportHeight, textAnchor, autoLineBreakAngleDeg, durationInSec));
                return;
            }

            Vector2 textStartPos_inWarpedViewportSpace = textStartPos;
            Vector2 circleCenterPosition_inWarpedViewportSpace = circleCenterPosition;
            Vector2 textsInitialUp_inWarpedViewportSpace = textStartPos_inWarpedViewportSpace - circleCenterPosition_inWarpedViewportSpace;
            Vector2 textsInitialUp_inUniformSpace = DrawScreenspace.DirectionInUnitsOfWarpedSpace_to_sameLookingDirectionInUnitsOfUnwarpedSpace(textsInitialUp_inWarpedViewportSpace, screenCamera);
            float radius_inUniformSpace = textsInitialUp_inUniformSpace.magnitude;
            float radius_relToViewportHeight = radius_inUniformSpace; //-> this is equal, because the convention of "DrawScreenspace.DirectionInUnitsOfWarpedSpace_to_sameLookingDirectionInUnitsOfUnwarpedSpace()" is, that the screenHeight is 1 unit in the uniform space.

            bool skipDraw = false; //-> The corresponding method in "TextUtilitiesDXXL" has the same parameters and can be used interchangeably, except that it has the additional parameter "skipDraw". This can be set to 'true' if only the 'parsedTextSpecs' should be filled (e.g. as decision base for the final placement of the text via an additinal DrawText-call).
            UtilitiesDXXL_Text.WriteOnCircleScreenspace(screenCamera, text, circleCenterPosition, radius_relToViewportHeight, color, size_relToViewportHeight, textsInitialUp_inUniformSpace, textAnchor, autoLineBreakAngleDeg, durationInSec, skipDraw);
        }

        public static void WriteOnCircleScreenspace(string text, Vector2 circleCenterPosition, float radius_relToViewportHeight, Color color, float size_relToViewportHeight, Vector2 textsInitialUp, TextAnchorCircledDXXL textAnchor = TextAnchorCircledDXXL.LowerLeftOfFirstLine, float autoLineBreakAngleDeg = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteOnCircleScreenspace") == false) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextOnCircleScreenspace_dirViaVecUp_cam.Add(new TextOnCircleScreenspace_dirViaVecUp_cam(automaticallyFoundCamera, text, circleCenterPosition, radius_relToViewportHeight, color, size_relToViewportHeight, textsInitialUp, textAnchor, autoLineBreakAngleDeg, durationInSec));
                return;
            }

            WriteOnCircleScreenspace(automaticallyFoundCamera, text, circleCenterPosition, radius_relToViewportHeight, color, size_relToViewportHeight, textsInitialUp, textAnchor, autoLineBreakAngleDeg, durationInSec);
        }
        public static void WriteOnCircleScreenspace(Camera screenCamera, string text, Vector2 circleCenterPosition, float radius_relToViewportHeight, Color color, float size_relToViewportHeight, Vector2 textsInitialUp, TextAnchorCircledDXXL textAnchor = TextAnchorCircledDXXL.LowerLeftOfFirstLine, float autoLineBreakAngleDeg = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(screenCamera, "screenCamera")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextOnCircleScreenspace_dirViaVecUp_cam.Add(new TextOnCircleScreenspace_dirViaVecUp_cam(screenCamera, text, circleCenterPosition, radius_relToViewportHeight, color, size_relToViewportHeight, textsInitialUp, textAnchor, autoLineBreakAngleDeg, durationInSec));
                return;
            }

            bool skipDraw = false; //-> The corresponding method in "TextUtilitiesDXXL" has the same parameters and can be used interchangeably, except that it has the additional parameter "skipDraw". This can be set to 'true' if only the 'parsedTextSpecs' should be filled (e.g. as decision base for the final placement of the text via an additinal DrawText-call).
            UtilitiesDXXL_Text.WriteOnCircleScreenspace(screenCamera, text, circleCenterPosition, radius_relToViewportHeight, color, size_relToViewportHeight, textsInitialUp, textAnchor, autoLineBreakAngleDeg, durationInSec, skipDraw);
        }

        public static void WriteOnCircleScreenspace(string text, Vector2 circleCenterPosition, float radius_relToViewportHeight, Color color = default(Color), float size_relToViewportHeight = 0.025f, float initialTextDirection_as_zRotationDegCCfromCamUp = 0.0f, TextAnchorCircledDXXL textAnchor = TextAnchorCircledDXXL.LowerLeftOfFirstLine, float autoLineBreakAngleDeg = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteOnCircleScreenspace") == false) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextOnCircleScreenspace_dirViaAngle_cam.Add(new TextOnCircleScreenspace_dirViaAngle_cam(automaticallyFoundCamera, text, circleCenterPosition, radius_relToViewportHeight, color, size_relToViewportHeight, initialTextDirection_as_zRotationDegCCfromCamUp, textAnchor, autoLineBreakAngleDeg, durationInSec));
                return;
            }

            WriteOnCircleScreenspace(automaticallyFoundCamera, text, circleCenterPosition, radius_relToViewportHeight, color, size_relToViewportHeight, initialTextDirection_as_zRotationDegCCfromCamUp, textAnchor, autoLineBreakAngleDeg, durationInSec);
        }
        public static void WriteOnCircleScreenspace(Camera screenCamera, string text, Vector2 circleCenterPosition, float radius_relToViewportHeight, Color color = default(Color), float size_relToViewportHeight = 0.025f, float initialTextDirection_as_zRotationDegCCfromCamUp = 0.0f, TextAnchorCircledDXXL textAnchor = TextAnchorCircledDXXL.LowerLeftOfFirstLine, float autoLineBreakAngleDeg = 0.0f, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(screenCamera, "screenCamera")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TextOnCircleScreenspace_dirViaAngle_cam.Add(new TextOnCircleScreenspace_dirViaAngle_cam(screenCamera, text, circleCenterPosition, radius_relToViewportHeight, color, size_relToViewportHeight, initialTextDirection_as_zRotationDegCCfromCamUp, textAnchor, autoLineBreakAngleDeg, durationInSec));
                return;
            }

            bool skipDraw = false; //-> The corresponding method in "TextUtilitiesDXXL" has the same parameters and can be used interchangeably, except that it has the additional parameter "skipDraw". This can be set to 'true' if only the 'parsedTextSpecs' should be filled (e.g. as decision base for the final placement of the text via an additinal DrawText-call).
            UtilitiesDXXL_Text.WriteOnCircleScreenspace(screenCamera, text, circleCenterPosition, radius_relToViewportHeight, color, size_relToViewportHeight, initialTextDirection_as_zRotationDegCCfromCamUp, textAnchor, autoLineBreakAngleDeg, durationInSec, skipDraw);
        }

        public static void WriteOnCircle2D(string text, Vector2 textStartPos, Vector2 circleCenterPosition, Color color = default(Color), float size = 0.1f, float custom_zPos = float.PositiveInfinity, TextAnchorCircledDXXL textAnchor = TextAnchorCircledDXXL.LowerLeftOfFirstLine, float autoLineBreakAngleDeg = 0.0f, bool autoFlipToPreventMirrorInverted = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(textStartPos, "textStartPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(circleCenterPosition, "circleCenterPosition")) { return; }

            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector2 textStartPosV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(textStartPos, zPos);
            Vector2 circleCenterPositionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(circleCenterPosition, zPos);

            Vector3 textsInitialUp = textStartPosV3 - circleCenterPositionV3;
            Vector3 textsInitialUp_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(textsInitialUp, out float radius); //-> normalizing is actually not necessary here, but is also no problem, since length/magnitude is needed for the radius anyway
            Vector3 textsInitialDir = Vector3.Cross(textsInitialUp_normalized, Vector3.forward);
            textsInitialDir = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(textsInitialDir);

            bool skipDraw = false; //-> The corresponding method in "TextUtilitiesDXXL" has the same parameters and can be used interchangeably, except that it has the additional parameter "skipDraw". This can be set to 'true' if only the 'parsedTextSpecs' should be filled (e.g. as decision base for the final placement of the text via an additinal DrawText-call).
            UtilitiesDXXL_Text.WriteOnCircle(text, circleCenterPosition, radius, color, size, textsInitialDir, textsInitialUp, textAnchor, autoLineBreakAngleDeg, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, skipDraw, true, false);
        }

        public static void WriteOnCircle2D(string text, Vector2 circleCenterPosition, float radius, Color color, float size, Vector2 textsInitialUp, float custom_zPos = float.PositiveInfinity, TextAnchorCircledDXXL textAnchor = TextAnchorCircledDXXL.LowerLeftOfFirstLine, float autoLineBreakAngleDeg = 0.0f, bool autoFlipToPreventMirrorInverted = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 circleCenterPositionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(circleCenterPosition, zPos);
            Vector3 textsInitialUpV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(textsInitialUp);
            textsInitialUpV3 = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(textsInitialUpV3);
            Vector3 textsInitialDirV3 = Vector3.Cross(textsInitialUpV3, Vector3.forward);

            bool skipDraw = false; //-> The method in "TextUtilitiesDXXL" has the additional parameter "skipDraw". This can be set to 'true' if only the 'parsedTextSpecs' should be filled (e.g. as decision base for the final placement of the text via an additinal DrawText-call).
            UtilitiesDXXL_Text.WriteOnCircle(text, circleCenterPositionV3, radius, color, size, textsInitialDirV3, textsInitialUpV3, textAnchor, autoLineBreakAngleDeg, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, skipDraw, true, false);
        }

        public static void WriteOnCircle2D(string text, Vector2 circleCenterPosition, float radius, Color color = default(Color), float size = 0.1f, float initialTextDirection_as_zRotationDegCCfromV3Right = 0.0f, float custom_zPos = float.PositiveInfinity, TextAnchorCircledDXXL textAnchor = TextAnchorCircledDXXL.LowerLeftOfFirstLine, float autoLineBreakAngleDeg = 0.0f, bool autoFlipToPreventMirrorInverted = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 circleCenterPositionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(circleCenterPosition, zPos);
            Quaternion rotation = UtilitiesDXXL_DrawBasics2D.QuaternionFromAngle(initialTextDirection_as_zRotationDegCCfromV3Right);
            Vector3 textsInitialDirV3 = rotation * Vector3.right;
            Vector3 textsInitialUpV3 = Vector3.Cross(Vector3.forward, textsInitialDirV3);

            bool skipDraw = false; //-> The method in "TextUtilitiesDXXL" has the additional parameter "skipDraw". This can be set to 'true' if only the 'parsedTextSpecs' should be filled (e.g. as decision base for the final placement of the text via an additinal DrawText-call).
            UtilitiesDXXL_Text.WriteOnCircle(text, circleCenterPositionV3, radius, color, size, textsInitialDirV3, textsInitialUpV3, textAnchor, autoLineBreakAngleDeg, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, skipDraw, true, true);
        }

        public static void WriteOnCircle(string text, Vector3 textStartPos, Vector3 circleCenterPosition, Vector3 turnAxis_direction = default(Vector3), Color color = default(Color), float size = 0.1f, TextAnchorCircledDXXL textAnchor = TextAnchorCircledDXXL.LowerLeftOfFirstLine, float autoLineBreakAngleDeg = 0.0f, bool autoFlipToPreventMirrorInverted = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            UtilitiesDXXL_Text.WriteOnCircle(text, textStartPos, circleCenterPosition, turnAxis_direction, color, size, textAnchor, autoLineBreakAngleDeg, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteOnCircle(string text, Vector3 circleCenterPosition, float radius, Color color = default(Color), float size = 0.1f, Quaternion orientation = default(Quaternion), TextAnchorCircledDXXL textAnchor = TextAnchorCircledDXXL.LowerLeftOfFirstLine, float autoLineBreakAngleDeg = 0.0f, bool autoFlipToPreventMirrorInverted = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            bool rotationIsValid = UtilitiesDXXL_TextDirAndUpCalculation.ConvertQuaternionToTextDirAndUpVectors(out Vector3 textsInitialDir, out Vector3 textsInitialUp, orientation);
            bool skipDraw = false; //-> The corresponding method in "TextUtilitiesDXXL" has the same parameters and can be used interchangeably, except that it has the additional parameter "skipDraw". This can be set to 'true' if only the 'parsedTextSpecs' should be filled (e.g. as decision base for the final placement of the text via an additinal DrawText-call).
            UtilitiesDXXL_Text.WriteOnCircle(text, circleCenterPosition, radius, color, size, textsInitialDir, textsInitialUp, textAnchor, autoLineBreakAngleDeg, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, skipDraw, false, rotationIsValid);
        }

        public static void WriteOnCircle(string text, Vector3 circleCenterPosition, float radius, Color color = default(Color), float size = 0.1f, Vector3 textsInitialDir = default(Vector3), Vector3 textsInitialUp = default(Vector3), TextAnchorCircledDXXL textAnchor = TextAnchorCircledDXXL.LowerLeftOfFirstLine, float autoLineBreakAngleDeg = 0.0f, bool autoFlipToPreventMirrorInverted = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            bool skipDraw = false; //-> The corresponding method in "TextUtilitiesDXXL" has the same parameters and can be used interchangeably, except that it has the additional parameter "skipDraw". This can be set to 'true' if only the 'parsedTextSpecs' should be filled (e.g. as decision base for the final placement of the text via an additinal DrawText-call).
            UtilitiesDXXL_Text.WriteOnCircle(text, circleCenterPosition, radius, color, size, textsInitialDir, textsInitialUp, textAnchor, autoLineBreakAngleDeg, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, skipDraw, false, false);
        }

        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<bool[]> GetContentFromBoolArray_preAllocated = UtilitiesDXXL_DrawCollections.GetBoolAsStringFromArray;
        public static void WriteArray(bool[] boolArray, Vector3 position = default(Vector3), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, Quaternion rotation = default(Quaternion), bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(boolArray, "boolArray")) { return; }

            string titleFallback = "Array";
            bool collectionRepresentsBools = true;
            UtilitiesDXXL_DrawCollections.WriteCollection_in3D(boolArray, boolArray.Length, GetContentFromBoolArray_preAllocated, null, null, null, "bool", null, null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, rotation, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteArray2D(bool[] boolArray, Vector2 position = default(Vector2), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, float custom_zPos = float.PositiveInfinity, bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(boolArray, "boolArray")) { return; }

            string titleFallback = "Array";
            bool collectionRepresentsBools = true;
            UtilitiesDXXL_DrawCollections.WriteCollection_in2D(boolArray, boolArray.Length, GetContentFromBoolArray_preAllocated, null, null, null, "bool", null, null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, custom_zPos, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteArrayScreenspace(bool[] boolArray, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfBool_screenspace_3Dpos.Add(new ArrayOfBool_screenspace_3Dpos(boolArray, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteArrayScreenspace") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            WriteArrayScreenspace(boolArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(Camera screenCamera, bool[] boolArray, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfBool_screenspace_3Dpos_cam.Add(new ArrayOfBool_screenspace_3Dpos_cam(screenCamera, boolArray, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            WriteArrayScreenspace(screenCamera, boolArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(bool[] boolArray, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfBool_screenspace_2Dpos.Add(new ArrayOfBool_screenspace_2Dpos(boolArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteArrayScreenspace") == false) { return; }
            WriteArrayScreenspace(automaticallyFoundCamera, boolArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(Camera screenCamera, bool[] boolArray, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(boolArray, "boolArray")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfBool_screenspace_2Dpos_cam.Add(new ArrayOfBool_screenspace_2Dpos_cam(screenCamera, boolArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            string titleFallback = "Array";
            bool collectionRepresentsBools = true;
            UtilitiesDXXL_DrawCollections.WriteCollection_inScreenspace(screenCamera, boolArray, boolArray.Length, GetContentFromBoolArray_preAllocated, null, null, null, "bool", null, null, null, position_in2DViewportSpace, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox_relToViewportHeight, textSize_relToViewportHeight, color, title, titleFallback, collectionRepresentsBools, durationInSec);
        }

        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<List<bool>> GetContentFromBoolList_preAllocated = UtilitiesDXXL_DrawCollections.GetBoolAsStringFromList;
        public static void WriteList(List<bool> boolList, Vector3 position = default(Vector3), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, Quaternion rotation = default(Quaternion), bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(boolList, "boolList")) { return; }

            string titleFallback = "List";
            bool collectionRepresentsBools = true;
            UtilitiesDXXL_DrawCollections.WriteCollection_in3D(boolList, boolList.Count, GetContentFromBoolList_preAllocated, null, null, null, "bool", null, null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, rotation, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteList2D(List<bool> boolList, Vector2 position = default(Vector2), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, float custom_zPos = float.PositiveInfinity, bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(boolList, "boolList")) { return; }

            string titleFallback = "List";
            bool collectionRepresentsBools = true;
            UtilitiesDXXL_DrawCollections.WriteCollection_in2D(boolList, boolList.Count, GetContentFromBoolList_preAllocated, null, null, null, "bool", null, null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, custom_zPos, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteListScreenspace(List<bool> boolList, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfBool_screenspace_3Dpos.Add(new ListOfBool_screenspace_3Dpos(boolList, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteListScreenspace") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            WriteListScreenspace(boolList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(Camera screenCamera, List<bool> boolList, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfBool_screenspace_3Dpos_cam.Add(new ListOfBool_screenspace_3Dpos_cam(screenCamera, boolList, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            WriteListScreenspace(screenCamera, boolList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(List<bool> boolList, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfBool_screenspace_2Dpos.Add(new ListOfBool_screenspace_2Dpos(boolList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteListScreenspace") == false) { return; }
            WriteListScreenspace(automaticallyFoundCamera, boolList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(Camera screenCamera, List<bool> boolList, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(boolList, "boolList")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfBool_screenspace_2Dpos_cam.Add(new ListOfBool_screenspace_2Dpos_cam(screenCamera, boolList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            string titleFallback = "List";
            bool collectionRepresentsBools = true;
            UtilitiesDXXL_DrawCollections.WriteCollection_inScreenspace(screenCamera, boolList, boolList.Count, GetContentFromBoolList_preAllocated, null, null, null, "bool", null, null, null, position_in2DViewportSpace, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox_relToViewportHeight, textSize_relToViewportHeight, color, title, titleFallback, collectionRepresentsBools, durationInSec);
        }

        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<int[]> GetContentFromIntArray_preAllocated = UtilitiesDXXL_DrawCollections.GetIntAsStringFromArray;
        public static void WriteArray(int[] intArray, Vector3 position = default(Vector3), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, Quaternion rotation = default(Quaternion), bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(intArray, "intArray")) { return; }

            string titleFallback = "Array";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in3D(intArray, intArray.Length, GetContentFromIntArray_preAllocated, null, null, null, "int", null, null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, rotation, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteArray2D(int[] intArray, Vector2 position = default(Vector2), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, float custom_zPos = float.PositiveInfinity, bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(intArray, "intArray")) { return; }

            string titleFallback = "Array";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in2D(intArray, intArray.Length, GetContentFromIntArray_preAllocated, null, null, null, "int", null, null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, custom_zPos, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteArrayScreenspace(int[] intArray, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfInt_screenspace_3Dpos.Add(new ArrayOfInt_screenspace_3Dpos(intArray, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteArrayScreenspace") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            WriteArrayScreenspace(intArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(Camera screenCamera, int[] intArray, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfInt_screenspace_3Dpos_cam.Add(new ArrayOfInt_screenspace_3Dpos_cam(screenCamera, intArray, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            WriteArrayScreenspace(screenCamera, intArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(int[] intArray, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfInt_screenspace_2Dpos.Add(new ArrayOfInt_screenspace_2Dpos(intArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteArrayScreenspace") == false) { return; }
            WriteArrayScreenspace(automaticallyFoundCamera, intArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(Camera screenCamera, int[] intArray, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(intArray, "intArray")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfInt_screenspace_2Dpos_cam.Add(new ArrayOfInt_screenspace_2Dpos_cam(screenCamera, intArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            string titleFallback = "Array";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_inScreenspace(screenCamera, intArray, intArray.Length, GetContentFromIntArray_preAllocated, null, null, null, "int", null, null, null, position_in2DViewportSpace, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox_relToViewportHeight, textSize_relToViewportHeight, color, title, titleFallback, collectionRepresentsBools, durationInSec);
        }

        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<List<int>> GetContentFromIntList_preAllocated = UtilitiesDXXL_DrawCollections.GetIntAsStringFromList;
        public static void WriteList(List<int> intList, Vector3 position = default(Vector3), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, Quaternion rotation = default(Quaternion), bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(intList, "intList")) { return; }

            string titleFallback = "List";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in3D(intList, intList.Count, GetContentFromIntList_preAllocated, null, null, null, "int", null, null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, rotation, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteList2D(List<int> intList, Vector2 position = default(Vector2), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, float custom_zPos = float.PositiveInfinity, bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(intList, "intList")) { return; }

            string titleFallback = "List";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in2D(intList, intList.Count, GetContentFromIntList_preAllocated, null, null, null, "int", null, null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, custom_zPos, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteListScreenspace(List<int> intList, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfInt_screenspace_3Dpos.Add(new ListOfInt_screenspace_3Dpos(intList, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteListScreenspace") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            WriteListScreenspace(intList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(Camera screenCamera, List<int> intList, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfInt_screenspace_3Dpos_cam.Add(new ListOfInt_screenspace_3Dpos_cam(screenCamera, intList, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            WriteListScreenspace(screenCamera, intList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(List<int> intList, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfInt_screenspace_2Dpos.Add(new ListOfInt_screenspace_2Dpos(intList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteListScreenspace") == false) { return; }
            WriteListScreenspace(automaticallyFoundCamera, intList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(Camera screenCamera, List<int> intList, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(intList, "intList")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfInt_screenspace_2Dpos_cam.Add(new ListOfInt_screenspace_2Dpos_cam(screenCamera, intList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            string titleFallback = "List";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_inScreenspace(screenCamera, intList, intList.Count, GetContentFromIntList_preAllocated, null, null, null, "int", null, null, null, position_in2DViewportSpace, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox_relToViewportHeight, textSize_relToViewportHeight, color, title, titleFallback, collectionRepresentsBools, durationInSec);
        }

        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<float[]> GetContentFromFloatArray_preAllocated = UtilitiesDXXL_DrawCollections.GetFloatAsStringFromArray;
        public static void WriteArray(float[] floatArray, Vector3 position = default(Vector3), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, Quaternion rotation = default(Quaternion), bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(floatArray, "floatArray")) { return; }

            string titleFallback = "Array";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in3D(floatArray, floatArray.Length, GetContentFromFloatArray_preAllocated, null, null, null, "float", null, null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, rotation, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteArray2D(float[] floatArray, Vector2 position = default(Vector2), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, float custom_zPos = float.PositiveInfinity, bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(floatArray, "floatArray")) { return; }

            string titleFallback = "Array";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in2D(floatArray, floatArray.Length, GetContentFromFloatArray_preAllocated, null, null, null, "float", null, null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, custom_zPos, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteArrayScreenspace(float[] floatArray, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfFloat_screenspace_3Dpos.Add(new ArrayOfFloat_screenspace_3Dpos(floatArray, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteArrayScreenspace") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            WriteArrayScreenspace(floatArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(Camera screenCamera, float[] floatArray, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfFloat_screenspace_3Dpos_cam.Add(new ArrayOfFloat_screenspace_3Dpos_cam(screenCamera, floatArray, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            WriteArrayScreenspace(screenCamera, floatArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(float[] floatArray, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfFloat_screenspace_2Dpos.Add(new ArrayOfFloat_screenspace_2Dpos(floatArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteArrayScreenspace") == false) { return; }
            WriteArrayScreenspace(automaticallyFoundCamera, floatArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(Camera screenCamera, float[] floatArray, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(floatArray, "floatArray")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfFloat_screenspace_2Dpos_cam.Add(new ArrayOfFloat_screenspace_2Dpos_cam(screenCamera, floatArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            string titleFallback = "Array";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_inScreenspace(screenCamera, floatArray, floatArray.Length, GetContentFromFloatArray_preAllocated, null, null, null, "float", null, null, null, position_in2DViewportSpace, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox_relToViewportHeight, textSize_relToViewportHeight, color, title, titleFallback, collectionRepresentsBools, durationInSec);
        }

        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<List<float>> GetContentFromFloatList_preAllocated = UtilitiesDXXL_DrawCollections.GetFloatAsStringFromList;
        public static void WriteList(List<float> floatList, Vector3 position = default(Vector3), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, Quaternion rotation = default(Quaternion), bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(floatList, "floatList")) { return; }

            string titleFallback = "List";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in3D(floatList, floatList.Count, GetContentFromFloatList_preAllocated, null, null, null, "float", null, null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, rotation, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteList2D(List<float> floatList, Vector2 position = default(Vector2), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, float custom_zPos = float.PositiveInfinity, bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(floatList, "floatList")) { return; }

            string titleFallback = "List";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in2D(floatList, floatList.Count, GetContentFromFloatList_preAllocated, null, null, null, "float", null, null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, custom_zPos, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteListScreenspace(List<float> floatList, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfFloat_screenspace_3Dpos.Add(new ListOfFloat_screenspace_3Dpos(floatList, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteListScreenspace") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            WriteListScreenspace(floatList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(Camera screenCamera, List<float> floatList, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfFloat_screenspace_3Dpos_cam.Add(new ListOfFloat_screenspace_3Dpos_cam(screenCamera, floatList, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            WriteListScreenspace(screenCamera, floatList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(List<float> floatList, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfFloat_screenspace_2Dpos.Add(new ListOfFloat_screenspace_2Dpos(floatList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteListScreenspace") == false) { return; }
            WriteListScreenspace(automaticallyFoundCamera, floatList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(Camera screenCamera, List<float> floatList, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(floatList, "floatList")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfFloat_screenspace_2Dpos_cam.Add(new ListOfFloat_screenspace_2Dpos_cam(screenCamera, floatList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            string titleFallback = "List";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_inScreenspace(screenCamera, floatList, floatList.Count, GetContentFromFloatList_preAllocated, null, null, null, "float", null, null, null, position_in2DViewportSpace, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox_relToViewportHeight, textSize_relToViewportHeight, color, title, titleFallback, collectionRepresentsBools, durationInSec);
        }

        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<string[]> GetContentFromStringArray_preAllocated = UtilitiesDXXL_DrawCollections.GetStringFromArray;
        public static void WriteArray(string[] stringArray, Vector3 position = default(Vector3), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, Quaternion rotation = default(Quaternion), bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(stringArray, "stringArray")) { return; }

            string titleFallback = "Array";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in3D(stringArray, stringArray.Length, GetContentFromStringArray_preAllocated, null, null, null, "string", null, null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, rotation, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteArray2D(string[] stringArray, Vector2 position = default(Vector2), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, float custom_zPos = float.PositiveInfinity, bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(stringArray, "stringArray")) { return; }

            string titleFallback = "Array";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in2D(stringArray, stringArray.Length, GetContentFromStringArray_preAllocated, null, null, null, "string", null, null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, custom_zPos, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteArrayScreenspace(string[] stringArray, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfString_screenspace_3Dpos.Add(new ArrayOfString_screenspace_3Dpos(stringArray, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteArrayScreenspace") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            WriteArrayScreenspace(stringArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(Camera screenCamera, string[] stringArray, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfString_screenspace_3Dpos_cam.Add(new ArrayOfString_screenspace_3Dpos_cam(screenCamera, stringArray, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            WriteArrayScreenspace(screenCamera, stringArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(string[] stringArray, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfString_screenspace_2Dpos.Add(new ArrayOfString_screenspace_2Dpos(stringArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteArrayScreenspace") == false) { return; }
            WriteArrayScreenspace(automaticallyFoundCamera, stringArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(Camera screenCamera, string[] stringArray, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(stringArray, "stringArray")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfString_screenspace_2Dpos_cam.Add(new ArrayOfString_screenspace_2Dpos_cam(screenCamera, stringArray, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            string titleFallback = "Array";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_inScreenspace(screenCamera, stringArray, stringArray.Length, GetContentFromStringArray_preAllocated, null, null, null, "string", null, null, null, position_in2DViewportSpace, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox_relToViewportHeight, textSize_relToViewportHeight, color, title, titleFallback, collectionRepresentsBools, durationInSec);
        }

        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<List<string>> GetContentFromStringList_preAllocated = UtilitiesDXXL_DrawCollections.GetStringFromList;
        public static void WriteList(List<string> stringList, Vector3 position = default(Vector3), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, Quaternion rotation = default(Quaternion), bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(stringList, "stringList")) { return; }

            string titleFallback = "List";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in3D(stringList, stringList.Count, GetContentFromStringList_preAllocated, null, null, null, "string", null, null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, rotation, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteList2D(List<string> stringList, Vector2 position = default(Vector2), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, float custom_zPos = float.PositiveInfinity, bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(stringList, "stringList")) { return; }

            string titleFallback = "List";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in2D(stringList, stringList.Count, GetContentFromStringList_preAllocated, null, null, null, "string", null, null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, custom_zPos, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteListScreenspace(List<string> stringList, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfString_screenspace_3Dpos.Add(new ListOfString_screenspace_3Dpos(stringList, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteListScreenspace") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            WriteListScreenspace(stringList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(Camera screenCamera, List<string> stringList, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfString_screenspace_3Dpos_cam.Add(new ListOfString_screenspace_3Dpos_cam(screenCamera, stringList, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            WriteListScreenspace(screenCamera, stringList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(List<string> stringList, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfString_screenspace_2Dpos.Add(new ListOfString_screenspace_2Dpos(stringList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteListScreenspace") == false) { return; }
            WriteListScreenspace(automaticallyFoundCamera, stringList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(Camera screenCamera, List<string> stringList, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(stringList, "stringList")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfString_screenspace_2Dpos_cam.Add(new ListOfString_screenspace_2Dpos_cam(screenCamera, stringList, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            string titleFallback = "List";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_inScreenspace(screenCamera, stringList, stringList.Count, GetContentFromStringList_preAllocated, null, null, null, "string", null, null, null, position_in2DViewportSpace, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox_relToViewportHeight, textSize_relToViewportHeight, color, title, titleFallback, collectionRepresentsBools, durationInSec);
        }

        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<Vector2[]> GetContentXFromVector2Array_preAllocated = UtilitiesDXXL_DrawCollections.GetVector2XAsStringFromArray;
        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<Vector2[]> GetContentYFromVector2Array_preAllocated = UtilitiesDXXL_DrawCollections.GetVector2YAsStringFromArray;
        public static void WriteArray(Vector2[] vector2Array, Vector3 position = default(Vector3), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, Quaternion rotation = default(Quaternion), bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(vector2Array, "vector2Array")) { return; }

            string titleFallback = "Array of Vector2";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in3D(vector2Array, vector2Array.Length, GetContentXFromVector2Array_preAllocated, GetContentYFromVector2Array_preAllocated, null, null, "X", "Y", null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, rotation, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteArray2D(Vector2[] vector2Array, Vector2 position = default(Vector2), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, float custom_zPos = float.PositiveInfinity, bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(vector2Array, "vector2Array")) { return; }

            string titleFallback = "Array of Vector2";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in2D(vector2Array, vector2Array.Length, GetContentXFromVector2Array_preAllocated, GetContentYFromVector2Array_preAllocated, null, null, "X", "Y", null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, custom_zPos, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteArrayScreenspace(Vector2[] vector2Array, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfVector2_screenspace_3Dpos.Add(new ArrayOfVector2_screenspace_3Dpos(vector2Array, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteArrayScreenspace") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            WriteArrayScreenspace(vector2Array, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(Camera screenCamera, Vector2[] vector2Array, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfVector2_screenspace_3Dpos_cam.Add(new ArrayOfVector2_screenspace_3Dpos_cam(screenCamera, vector2Array, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            WriteArrayScreenspace(screenCamera, vector2Array, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(Vector2[] vector2Array, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfVector2_screenspace_2Dpos.Add(new ArrayOfVector2_screenspace_2Dpos(vector2Array, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteArrayScreenspace") == false) { return; }
            WriteArrayScreenspace(automaticallyFoundCamera, vector2Array, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(Camera screenCamera, Vector2[] vector2Array, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(vector2Array, "vector2Array")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfVector2_screenspace_2Dpos_cam.Add(new ArrayOfVector2_screenspace_2Dpos_cam(screenCamera, vector2Array, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            string titleFallback = "Array of Vector2";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_inScreenspace(screenCamera, vector2Array, vector2Array.Length, GetContentXFromVector2Array_preAllocated, GetContentYFromVector2Array_preAllocated, null, null, "X", "Y", null, null, position_in2DViewportSpace, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox_relToViewportHeight, textSize_relToViewportHeight, color, title, titleFallback, collectionRepresentsBools, durationInSec);
        }

        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<List<Vector2>> GetContentXFromVector2List_preAllocated = UtilitiesDXXL_DrawCollections.GetVector2XAsStringFromList;
        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<List<Vector2>> GetContentYFromVector2List_preAllocated = UtilitiesDXXL_DrawCollections.GetVector2YAsStringFromList;
        public static void WriteList(List<Vector2> vector2List, Vector3 position = default(Vector3), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, Quaternion rotation = default(Quaternion), bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(vector2List, "vector2List")) { return; }

            string titleFallback = "List of Vector2";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in3D(vector2List, vector2List.Count, GetContentXFromVector2List_preAllocated, GetContentYFromVector2List_preAllocated, null, null, "X", "Y", null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, rotation, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteList2D(List<Vector2> vector2List, Vector2 position = default(Vector2), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, float custom_zPos = float.PositiveInfinity, bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(vector2List, "vector2List")) { return; }

            string titleFallback = "List of Vector2";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in2D(vector2List, vector2List.Count, GetContentXFromVector2List_preAllocated, GetContentYFromVector2List_preAllocated, null, null, "X", "Y", null, null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, custom_zPos, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteListScreenspace(List<Vector2> vector2List, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfVector2_screenspace_3Dpos.Add(new ListOfVector2_screenspace_3Dpos(vector2List, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteListScreenspace") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            WriteListScreenspace(vector2List, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(Camera screenCamera, List<Vector2> vector2List, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfVector2_screenspace_3Dpos_cam.Add(new ListOfVector2_screenspace_3Dpos_cam(screenCamera, vector2List, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            WriteListScreenspace(screenCamera, vector2List, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(List<Vector2> vector2List, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfVector2_screenspace_2Dpos.Add(new ListOfVector2_screenspace_2Dpos(vector2List, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteListScreenspace") == false) { return; }
            WriteListScreenspace(automaticallyFoundCamera, vector2List, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(Camera screenCamera, List<Vector2> vector2List, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(vector2List, "vector2List")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfVector2_screenspace_2Dpos_cam.Add(new ListOfVector2_screenspace_2Dpos_cam(screenCamera, vector2List, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            string titleFallback = "List of Vector2";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_inScreenspace(screenCamera, vector2List, vector2List.Count, GetContentXFromVector2List_preAllocated, GetContentYFromVector2List_preAllocated, null, null, "X", "Y", null, null, position_in2DViewportSpace, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox_relToViewportHeight, textSize_relToViewportHeight, color, title, titleFallback, collectionRepresentsBools, durationInSec);
        }

        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<Vector3[]> GetContentXFromVector3Array_preAllocated = UtilitiesDXXL_DrawCollections.GetVector3XAsStringFromArray;
        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<Vector3[]> GetContentYFromVector3Array_preAllocated = UtilitiesDXXL_DrawCollections.GetVector3YAsStringFromArray;
        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<Vector3[]> GetContentZFromVector3Array_preAllocated = UtilitiesDXXL_DrawCollections.GetVector3ZAsStringFromArray;
        public static void WriteArray(Vector3[] vector3Array, Vector3 position = default(Vector3), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, Quaternion rotation = default(Quaternion), bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(vector3Array, "vector3Array")) { return; }

            string titleFallback = "Array of Vector3";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in3D(vector3Array, vector3Array.Length, GetContentXFromVector3Array_preAllocated, GetContentYFromVector3Array_preAllocated, GetContentZFromVector3Array_preAllocated, null, "X", "Y", "Z", null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, rotation, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteArray2D(Vector3[] vector3Array, Vector2 position = default(Vector2), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, float custom_zPos = float.PositiveInfinity, bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(vector3Array, "vector3Array")) { return; }

            string titleFallback = "Array of Vector3";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in2D(vector3Array, vector3Array.Length, GetContentXFromVector3Array_preAllocated, GetContentYFromVector3Array_preAllocated, GetContentZFromVector3Array_preAllocated, null, "X", "Y", "Z", null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, custom_zPos, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteArrayScreenspace(Vector3[] vector3Array, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfVector3_screenspace_3Dpos.Add(new ArrayOfVector3_screenspace_3Dpos(vector3Array, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteArrayScreenspace") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            WriteArrayScreenspace(vector3Array, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(Camera screenCamera, Vector3[] vector3Array, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfVector3_screenspace_3Dpos_cam.Add(new ArrayOfVector3_screenspace_3Dpos_cam(screenCamera, vector3Array, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            WriteArrayScreenspace(screenCamera, vector3Array, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(Vector3[] vector3Array, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfVector3_screenspace_2Dpos.Add(new ArrayOfVector3_screenspace_2Dpos(vector3Array, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteArrayScreenspace") == false) { return; }
            WriteArrayScreenspace(automaticallyFoundCamera, vector3Array, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(Camera screenCamera, Vector3[] vector3Array, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(vector3Array, "vector3Array")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfVector3_screenspace_2Dpos_cam.Add(new ArrayOfVector3_screenspace_2Dpos_cam(screenCamera, vector3Array, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            string titleFallback = "Array of Vector3";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_inScreenspace(screenCamera, vector3Array, vector3Array.Length, GetContentXFromVector3Array_preAllocated, GetContentYFromVector3Array_preAllocated, GetContentZFromVector3Array_preAllocated, null, "X", "Y", "Z", null, position_in2DViewportSpace, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox_relToViewportHeight, textSize_relToViewportHeight, color, title, titleFallback, collectionRepresentsBools, durationInSec);
        }

        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<List<Vector3>> GetContentXFromVector3List_preAllocated = UtilitiesDXXL_DrawCollections.GetVector3XAsStringFromList;
        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<List<Vector3>> GetContentYFromVector3List_preAllocated = UtilitiesDXXL_DrawCollections.GetVector3YAsStringFromList;
        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<List<Vector3>> GetContentZFromVector3List_preAllocated = UtilitiesDXXL_DrawCollections.GetVector3ZAsStringFromList;
        public static void WriteList(List<Vector3> vector3List, Vector3 position = default(Vector3), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, Quaternion rotation = default(Quaternion), bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(vector3List, "vector3List")) { return; }

            string titleFallback = "List of Vector3";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in3D(vector3List, vector3List.Count, GetContentXFromVector3List_preAllocated, GetContentYFromVector3List_preAllocated, GetContentZFromVector3List_preAllocated, null, "X", "Y", "Z", null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, rotation, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteList2D(List<Vector3> vector3List, Vector2 position = default(Vector2), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, float custom_zPos = float.PositiveInfinity, bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(vector3List, "vector3List")) { return; }

            string titleFallback = "List of Vector3";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in2D(vector3List, vector3List.Count, GetContentXFromVector3List_preAllocated, GetContentYFromVector3List_preAllocated, GetContentZFromVector3List_preAllocated, null, "X", "Y", "Z", null, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, custom_zPos, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteListScreenspace(List<Vector3> vector3List, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfVector3_screenspace_3Dpos.Add(new ListOfVector3_screenspace_3Dpos(vector3List, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteListScreenspace") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            WriteListScreenspace(vector3List, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(Camera screenCamera, List<Vector3> vector3List, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfVector3_screenspace_3Dpos_cam.Add(new ListOfVector3_screenspace_3Dpos_cam(screenCamera, vector3List, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            WriteListScreenspace(screenCamera, vector3List, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(List<Vector3> vector3List, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfVector3_screenspace_2Dpos.Add(new ListOfVector3_screenspace_2Dpos(vector3List, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteListScreenspace") == false) { return; }
            WriteListScreenspace(automaticallyFoundCamera, vector3List, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(Camera screenCamera, List<Vector3> vector3List, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(vector3List, "vector3List")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfVector3_screenspace_2Dpos_cam.Add(new ListOfVector3_screenspace_2Dpos_cam(screenCamera, vector3List, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            string titleFallback = "List of Vector3";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_inScreenspace(screenCamera, vector3List, vector3List.Count, GetContentXFromVector3List_preAllocated, GetContentYFromVector3List_preAllocated, GetContentZFromVector3List_preAllocated, null, "X", "Y", "Z", null, position_in2DViewportSpace, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox_relToViewportHeight, textSize_relToViewportHeight, color, title, titleFallback, collectionRepresentsBools, durationInSec);
        }

        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<Vector4[]> GetContentXFromVector4Array_preAllocated = UtilitiesDXXL_DrawCollections.GetVector4XAsStringFromArray;
        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<Vector4[]> GetContentYFromVector4Array_preAllocated = UtilitiesDXXL_DrawCollections.GetVector4YAsStringFromArray;
        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<Vector4[]> GetContentZFromVector4Array_preAllocated = UtilitiesDXXL_DrawCollections.GetVector4ZAsStringFromArray;
        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<Vector4[]> GetContentWFromVector4Array_preAllocated = UtilitiesDXXL_DrawCollections.GetVector4WAsStringFromArray;
        public static void WriteArray(Vector4[] vector4Array, Vector3 position = default(Vector3), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, Quaternion rotation = default(Quaternion), bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(vector4Array, "vector4Array")) { return; }

            string titleFallback = "Array of Vector4";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in3D(vector4Array, vector4Array.Length, GetContentXFromVector4Array_preAllocated, GetContentYFromVector4Array_preAllocated, GetContentZFromVector4Array_preAllocated, GetContentWFromVector4Array_preAllocated, "X", "Y", "Z", "W", position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, rotation, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteArray2D(Vector4[] vector4Array, Vector2 position = default(Vector2), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, float custom_zPos = float.PositiveInfinity, bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(vector4Array, "vector4Array")) { return; }

            string titleFallback = "Array of Vector4";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in2D(vector4Array, vector4Array.Length, GetContentXFromVector4Array_preAllocated, GetContentYFromVector4Array_preAllocated, GetContentZFromVector4Array_preAllocated, GetContentWFromVector4Array_preAllocated, "X", "Y", "Z", "W", position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, custom_zPos, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteArrayScreenspace(Vector4[] vector4Array, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfVector4_screenspace_3Dpos.Add(new ArrayOfVector4_screenspace_3Dpos(vector4Array, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteArrayScreenspace") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            WriteArrayScreenspace(vector4Array, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(Camera screenCamera, Vector4[] vector4Array, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfVector4_screenspace_3Dpos_cam.Add(new ArrayOfVector4_screenspace_3Dpos_cam(screenCamera, vector4Array, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            WriteArrayScreenspace(screenCamera, vector4Array, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(Vector4[] vector4Array, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfVector4_screenspace_2Dpos.Add(new ArrayOfVector4_screenspace_2Dpos(vector4Array, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteArrayScreenspace") == false) { return; }
            WriteArrayScreenspace(automaticallyFoundCamera, vector4Array, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteArrayScreenspace(Camera screenCamera, Vector4[] vector4Array, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(vector4Array, "vector4Array")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ArrayOfVector4_screenspace_2Dpos_cam.Add(new ArrayOfVector4_screenspace_2Dpos_cam(screenCamera, vector4Array, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            string titleFallback = "Array of Vector4";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_inScreenspace(screenCamera, vector4Array, vector4Array.Length, GetContentXFromVector4Array_preAllocated, GetContentYFromVector4Array_preAllocated, GetContentZFromVector4Array_preAllocated, GetContentWFromVector4Array_preAllocated, "X", "Y", "Z", "W", position_in2DViewportSpace, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox_relToViewportHeight, textSize_relToViewportHeight, color, title, titleFallback, collectionRepresentsBools, durationInSec);
        }

        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<List<Vector4>> GetContentXFromVector4List_preAllocated = UtilitiesDXXL_DrawCollections.GetVector4XAsStringFromList;
        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<List<Vector4>> GetContentYFromVector4List_preAllocated = UtilitiesDXXL_DrawCollections.GetVector4YAsStringFromList;
        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<List<Vector4>> GetContentZFromVector4List_preAllocated = UtilitiesDXXL_DrawCollections.GetVector4ZAsStringFromList;
        static UtilitiesDXXL_DrawCollections.FlexibleGetColumnContentAtIndexAsString<List<Vector4>> GetContentWFromVector4List_preAllocated = UtilitiesDXXL_DrawCollections.GetVector4WAsStringFromList;
        public static void WriteList(List<Vector4> vector4List, Vector3 position = default(Vector3), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, Quaternion rotation = default(Quaternion), bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(vector4List, "vector4List")) { return; }

            string titleFallback = "List of Vector4";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in3D(vector4List, vector4List.Count, GetContentXFromVector4List_preAllocated, GetContentYFromVector4List_preAllocated, GetContentZFromVector4List_preAllocated, GetContentWFromVector4List_preAllocated, "X", "Y", "Z", "W", position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, rotation, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteList2D(List<Vector4> vector4List, Vector2 position = default(Vector2), Color color = default(Color), string title = null, float textSize = 0.05f, float forceHeightOfWholeTableBox = 0.0f, float custom_zPos = float.PositiveInfinity, bool position_isTopLeft_notLowLeft = true, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(vector4List, "vector4List")) { return; }

            string titleFallback = "List of Vector4";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_in2D(vector4List, vector4List.Count, GetContentXFromVector4List_preAllocated, GetContentYFromVector4List_preAllocated, GetContentZFromVector4List_preAllocated, GetContentWFromVector4List_preAllocated, "X", "Y", "Z", "W", position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, color, custom_zPos, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteListScreenspace(List<Vector4> vector4List, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfVector4_screenspace_3Dpos.Add(new ListOfVector4_screenspace_3Dpos(vector4List, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteListScreenspace") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            WriteListScreenspace(vector4List, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(Camera screenCamera, List<Vector4> vector4List, Vector3 position_in3DWorldspace, Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfVector4_screenspace_3Dpos_cam.Add(new ListOfVector4_screenspace_3Dpos_cam(screenCamera, vector4List, position_in3DWorldspace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            WriteListScreenspace(screenCamera, vector4List, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(List<Vector4> vector4List, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfVector4_screenspace_2Dpos.Add(new ListOfVector4_screenspace_2Dpos(vector4List, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawText.WriteListScreenspace") == false) { return; }
            WriteListScreenspace(automaticallyFoundCamera, vector4List, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec);
        }

        public static void WriteListScreenspace(Camera screenCamera, List<Vector4> vector4List, Vector2 position_in2DViewportSpace = default(Vector2), Color color = default(Color), string title = null, float textSize_relToViewportHeight = 0.025f, float forceHeightOfWholeTableBox_relToViewportHeight = 0.0f, bool position_isTopLeft_notLowLeft = false, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullSystemObjects(vector4List, "vector4List")) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_ListOfVector4_screenspace_2Dpos_cam.Add(new ListOfVector4_screenspace_2Dpos_cam(screenCamera, vector4List, position_in2DViewportSpace, color, title, textSize_relToViewportHeight, forceHeightOfWholeTableBox_relToViewportHeight, position_isTopLeft_notLowLeft, durationInSec));
                return;
            }

            string titleFallback = "List of Vector4";
            bool collectionRepresentsBools = false;
            UtilitiesDXXL_DrawCollections.WriteCollection_inScreenspace(screenCamera, vector4List, vector4List.Count, GetContentXFromVector4List_preAllocated, GetContentYFromVector4List_preAllocated, GetContentZFromVector4List_preAllocated, GetContentWFromVector4List_preAllocated, "X", "Y", "Z", "W", position_in2DViewportSpace, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox_relToViewportHeight, textSize_relToViewportHeight, color, title, titleFallback, collectionRepresentsBools, durationInSec);
        }

        public static string MarkupBold(string stringToMark)
        {
            return UtilitiesDXXL_Text.boldStartMarkupString + stringToMark + UtilitiesDXXL_Text.boldEndMarkupString;
        }

        public static string MarkupBold(int intToMark)
        {
            return UtilitiesDXXL_Text.boldStartMarkupString + intToMark + UtilitiesDXXL_Text.boldEndMarkupString;
        }
        public static string MarkupBoldEscape(string stringToMark)
        {
            return UtilitiesDXXL_Text.boldEndMarkupString + stringToMark + UtilitiesDXXL_Text.boldStartMarkupString;
        }

        public static string MarkupBoldEscape(int intToMark)
        {
            return UtilitiesDXXL_Text.boldEndMarkupString + intToMark + UtilitiesDXXL_Text.boldStartMarkupString;
        }

        public static string MarkupItalic(string stringToMark)
        {
            return UtilitiesDXXL_Text.italicStartMarkupString + stringToMark + UtilitiesDXXL_Text.italicEndMarkupString;
        }

        public static string MarkupItalic(int intToMark)
        {
            return UtilitiesDXXL_Text.italicStartMarkupString + intToMark + UtilitiesDXXL_Text.italicEndMarkupString;
        }

        public static string MarkupItalicEscape(string stringToMark)
        {
            return UtilitiesDXXL_Text.italicEndMarkupString + stringToMark + UtilitiesDXXL_Text.italicStartMarkupString;
        }

        public static string MarkupItalicEscape(int intToMark)
        {
            return UtilitiesDXXL_Text.italicEndMarkupString + intToMark + UtilitiesDXXL_Text.italicStartMarkupString;
        }

        public static string MarkupDeleted(string stringToMark)
        {
            return UtilitiesDXXL_Text.deletedStartMarkupString + stringToMark + UtilitiesDXXL_Text.deletedEndMarkupString;
        }

        public static string MarkupDeleted(int intToMark)
        {
            return UtilitiesDXXL_Text.deletedStartMarkupString + intToMark + UtilitiesDXXL_Text.deletedEndMarkupString;
        }

        public static string MarkupDeletedEscape(string stringToMark)
        {
            return UtilitiesDXXL_Text.deletedEndMarkupString + stringToMark + UtilitiesDXXL_Text.deletedStartMarkupString;
        }

        public static string MarkupDeletedEscape(int intToMark)
        {
            return UtilitiesDXXL_Text.deletedEndMarkupString + intToMark + UtilitiesDXXL_Text.deletedStartMarkupString;
        }

        public static string MarkupUnderlined(string stringToMark)
        {
            return UtilitiesDXXL_Text.underlinedStartMarkupString + stringToMark + UtilitiesDXXL_Text.underlinedEndMarkupString;
        }

        public static string MarkupUnderlined(int intToMark)
        {
            return UtilitiesDXXL_Text.underlinedStartMarkupString + intToMark + UtilitiesDXXL_Text.underlinedEndMarkupString;
        }

        public static string MarkupUnderlinedEscape(string stringToMark)
        {
            return UtilitiesDXXL_Text.underlinedEndMarkupString + stringToMark + UtilitiesDXXL_Text.underlinedStartMarkupString;
        }

        public static string MarkupUnderlinedEscape(int intToMark)
        {
            return UtilitiesDXXL_Text.underlinedEndMarkupString + intToMark + UtilitiesDXXL_Text.underlinedStartMarkupString;
        }

        public static string MarkupStrokeWidth(string stringToMark, int strokeWidth_asPPMofSize)
        {
            return UtilitiesDXXL_Text.strokeWidthStartMarkupString_preValue + strokeWidth_asPPMofSize + UtilitiesDXXL_Text.valueMarkupString_postValue + stringToMark + UtilitiesDXXL_Text.strokeWidthEndMarkupString;
        }

        public static string MarkupSize(string stringToMark, float sizeScaleFactor)
        {
            int size_asInt = Mathf.RoundToInt(11.0f * sizeScaleFactor);
            size_asInt = Mathf.Max(1, size_asInt);
            return MarkupSize(stringToMark, size_asInt);
        }

        public static string MarkupSize(string stringToMark, int size_relTo_11)
        {
            return UtilitiesDXXL_Text.sizeStartMarkupString_preValue + size_relTo_11 + UtilitiesDXXL_Text.valueMarkupString_postValue + stringToMark + UtilitiesDXXL_Text.sizeEndMarkupString;
        }

        public static string MarkupColor(string stringToMark, Color color)
        {
            return "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + UtilitiesDXXL_Text.valueMarkupString_postValue + stringToMark + UtilitiesDXXL_Text.colorEndMarkupString;
        }

        public static string MarkupColor(string stringToMark, string colorAsHexHTMLString)
        {
            if (colorAsHexHTMLString != null && colorAsHexHTMLString.Length > 0)
            {
                if (colorAsHexHTMLString[0] != '#')
                {
                    colorAsHexHTMLString = "#" + colorAsHexHTMLString;
                }
            }
            return UtilitiesDXXL_Text.colorStartMarkupString_preValue + colorAsHexHTMLString + UtilitiesDXXL_Text.valueMarkupString_postValue + stringToMark + UtilitiesDXXL_Text.colorEndMarkupString;
        }

        public static string MarkupColor(string stringToMark, bool truthValueThatColorShouldIndicate)
        {
            if (truthValueThatColorShouldIndicate)
            {
                return "<color=#83E24AFF>" + stringToMark + UtilitiesDXXL_Text.colorEndMarkupString;
            }
            else
            {
                return "<color=#ED4731FF>" + stringToMark + UtilitiesDXXL_Text.colorEndMarkupString;
            }
        }

        public static string MarkupColorFromGameobjectID(string stringToMark, GameObject colorDefiningGameobject, float forceLuminance = 0.0f)
        {
            Color color = SeededColorGenerator.ColorOfGameobjectID(colorDefiningGameobject, forceLuminance);
            return MarkupColor(stringToMark, color);
        }

        public static string MarkupColorSeededRandom(string stringToMark, int seed, float alphaOfGeneratedColor = 1.0f, float forceLuminance = 0.0f)
        {
            Color color = SeededColorGenerator.GetRandomColorSeeded(seed, alphaOfGeneratedColor, forceLuminance);
            return MarkupColor(stringToMark, color);
        }

        public static string MarkupColorRainbow(string stringToMark, int seed, float alphaOfGeneratedColor = 1.0f, int colorsPerSpectrumPass = 8, float forceLuminance = 0.0f)
        {
            Color color = SeededColorGenerator.GetRainbowColor(seed, alphaOfGeneratedColor, colorsPerSpectrumPass, forceLuminance);
            return MarkupColor(stringToMark, color);
        }

        public static string MarkupColorRainbowAroundRed(string stringToMark, int seed, float alphaOfGeneratedColor = 1.0f, int colorsPerSpectrumPass = 5, bool sawToothTransition = false, float forceLuminance = 0.0f)
        {
            Color color = SeededColorGenerator.GetRainbowColorAroundRed(seed, alphaOfGeneratedColor, colorsPerSpectrumPass, sawToothTransition, forceLuminance);
            return MarkupColor(stringToMark, color);
        }

        public static string MarkupColorRainbowAroundGreen(string stringToMark, int seed, float alphaOfGeneratedColor = 1.0f, int colorsPerSpectrumPass = 4, bool sawToothTransition = false, float forceLuminance = 0.0f)
        {
            Color color = SeededColorGenerator.GetRainbowColorAroundGreen(seed, alphaOfGeneratedColor, colorsPerSpectrumPass, sawToothTransition, forceLuminance);
            return MarkupColor(stringToMark, color);
        }

        public static string MarkupColorRainbowAroundBlue(string stringToMark, int seed, float alphaOfGeneratedColor = 1.0f, int colorsPerSpectrumPass = 5, bool sawToothTransition = false, float forceLuminance = 0.0f)
        {
            Color color = SeededColorGenerator.GetRainbowColorAroundBlue(seed, alphaOfGeneratedColor, colorsPerSpectrumPass, sawToothTransition, forceLuminance);
            return MarkupColor(stringToMark, color);
        }

        public static string MarkupIcon(DrawBasics.IconType icon)
        {
            return UtilitiesDXXL_CharsAndIcons.GetIconAsMarkupString(icon);
        }

        public static string MarkupCustomHeightEmptyLine(float vertSize_relToTextSize)
        {
            int sizeScaler = Mathf.RoundToInt(11.0f * vertSize_relToTextSize);
            return MarkupCustomHeightEmptyLine(sizeScaler);
        }

        public static string MarkupCustomHeightEmptyLine(int sizeMarkupValue)
        {
            return "<br><size=" + sizeMarkupValue + "> </size><br>";
        }

        public static string MarkupBoolDisplayer(string boolName, bool boolValueToDisplay, bool saveDrawnLines = false)
        {
            if (saveDrawnLines)
            {
                if (boolValueToDisplay)
                {
                    return boolName + ": <color=#ed47314C><icon=circleDotUnfilled></color><color=#83e24aFF><icon=circleDotFilled></color>";
                }
                else
                {
                    return boolName + ": <color=#ed4731FF><icon=circleDotFilled></color><color=#83e24a4C><icon=circleDotUnfilled></color>";
                }
            }
            else
            {
                if (boolValueToDisplay)
                {
                    return boolName + ": <sw=70000><color=#ed47314C><icon=circleDotUnfilled></color><color=#83e24aFF><icon=circleDotFilled></color></sw>";
                }
                else
                {
                    return boolName + ": <sw=70000><color=#ed4731FF><icon=circleDotFilled></color><color=#83e24a4C><icon=circleDotUnfilled></color></sw>";
                }
            }
        }

        public static string MarkupBoolDisplayer(bool boolValueToDisplay, bool saveDrawnLines = false)
        {
            if (saveDrawnLines)
            {
                if (boolValueToDisplay)
                {
                    return "<color=#ed47314C><icon=circleDotUnfilled></color><color=#83e24aFF><icon=circleDotFilled></color>";
                }
                else
                {
                    return "<color=#ed4731FF><icon=circleDotFilled></color><color=#83e24a4C><icon=circleDotUnfilled></color>";
                }
            }
            else
            {
                if (boolValueToDisplay)
                {
                    return "<sw=70000><color=#ed47314C><icon=circleDotUnfilled></color><color=#83e24aFF><icon=circleDotFilled></color></sw>";
                }
                else
                {
                    return "<sw=70000><color=#ed4731FF><icon=circleDotFilled></color><color=#83e24a4C><icon=circleDotUnfilled></color></sw>";
                }
            }
        }

        public static string MarkupBoolArrow(bool boolValueToDisplay)
        {
            if (boolValueToDisplay)
            {
                return "<color=#83E24AFF><icon=arrowUp></color>"; //the used color here is the same as "UtilitiesDXXL_Colors.green_boolTrue"
            }
            else
            {
                return "<color=#ED4731FF><icon=arrowDown></color>"; //the used color here is the same as "UtilitiesDXXL_Colors.red_boolFalse"
            }
        }

        public static string MarkupLogSymbol(LogType logType)
        {
            switch (logType)
            {
                case LogType.Log:
                    return "<color=#adadadFF><icon=logMessage></color>";

                case LogType.Warning:
                    return "<color=#e2aa00FF><icon=warning></color>";

                case LogType.Error:
                    return "<color=#ce0e0eFF><icon=logMessageError></color>";

                case LogType.Exception:
                    return "<color=#ce0e0eFF><icon=logMessageException></color>";

                case LogType.Assert:
                    return "<color=#ce0e0eFF><icon=logMessageAssertion></color>";

                default:
                    Debug.LogError("logType '" + logType + "' not implemented yet");
                    return "";
            }
        }

        public static bool ContainsLineBreak(string textToCheckForLineBreaks)
        {
            int i_startOfLineBreakMarkupString = textToCheckForLineBreaks.IndexOf(UtilitiesDXXL_Text.lineBreakMarkupString, 0);
            if (i_startOfLineBreakMarkupString >= 0)
            {
                return true;
            }

            for (int i_char = 0; i_char < textToCheckForLineBreaks.Length; i_char++)
            {
                //unicode 10 = linefeed
                //unicode 13 = carriagereturn
                if (10 == (int)textToCheckForLineBreaks[i_char] || 13 == (int)textToCheckForLineBreaks[i_char])
                {
                    return true;
                }
            }
            return false;
        }

    }

}
