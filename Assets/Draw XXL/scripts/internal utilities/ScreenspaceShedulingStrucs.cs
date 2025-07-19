namespace DrawXXL
{
    using DrawXXL;
    using UnityEngine;
    using System.Collections.Generic;

    //-> The purpose of these struct is to delay the drawing for screenspace shapes+lines
    //-> When calling a DrawXXL.DrawSomethingToScreenspace-function it is unknown when inside the Update() cycle the user called it.
    //-> The camera position and stance may get changed after the drawXXLline-call, but the transformation has already been done with the old camera postion and stance
    //-> To prevent that all ScreenspaceDrawings get sheduled and finally executed as late as possible in the Update-cylce, so that any camera position changes have already been done.

    public struct TextScreenspace_3Dpos_dirViaVec_cam
    {
        public Camera screenCamera;
        public string text;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public float size_relToViewportHeight;
        public Vector2 textDirection;
        public DrawText.TextAnchorDXXL textAnchor;
        public float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth;
        public float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth;
        public bool autoLineBreakAtViewportBorder;
        public float autoLineBreakWidth_relToViewportWidth;
        public bool autoFlipTextToPreventUpsideDown;
        public float durationInSec;

        public TextScreenspace_3Dpos_dirViaVec_cam(Camera screenCamera, string text, Vector3 position_in3DWorldspace, Color color, float size_relToViewportHeight, Vector2 textDirection, DrawText.TextAnchorDXXL textAnchor, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, bool autoLineBreakAtViewportBorder, float autoLineBreakWidth_relToViewportWidth, bool autoFlipTextToPreventUpsideDown, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.text = text;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.textDirection = textDirection;
            this.textAnchor = textAnchor;
            this.forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = forceTextBlockEnlargementToThisMinWidth_relToViewportWidth;
            this.forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth;
            this.autoLineBreakAtViewportBorder = autoLineBreakAtViewportBorder;
            this.autoLineBreakWidth_relToViewportWidth = autoLineBreakWidth_relToViewportWidth;
            this.autoFlipTextToPreventUpsideDown = autoFlipTextToPreventUpsideDown;
            this.durationInSec = durationInSec;
        }
    }

    public struct TextScreenspace_2Dpos_dirViaVec_cam
    {
        public Camera screenCamera;
        public string text;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public float size_relToViewportHeight;
        public Vector2 textDirection;
        public DrawText.TextAnchorDXXL textAnchor;
        public float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth;
        public float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth;
        public bool autoLineBreakAtViewportBorder;
        public float autoLineBreakWidth_relToViewportWidth;
        public bool autoFlipTextToPreventUpsideDown;
        public float durationInSec;

        public TextScreenspace_2Dpos_dirViaVec_cam(Camera screenCamera, string text, Vector2 position_in2DViewportSpace, Color color, float size_relToViewportHeight, Vector2 textDirection, DrawText.TextAnchorDXXL textAnchor, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, bool autoLineBreakAtViewportBorder, float autoLineBreakWidth_relToViewportWidth, bool autoFlipTextToPreventUpsideDown, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.text = text;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.textDirection = textDirection;
            this.textAnchor = textAnchor;
            this.forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = forceTextBlockEnlargementToThisMinWidth_relToViewportWidth;
            this.forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth;
            this.autoLineBreakAtViewportBorder = autoLineBreakAtViewportBorder;
            this.autoLineBreakWidth_relToViewportWidth = autoLineBreakWidth_relToViewportWidth;
            this.autoFlipTextToPreventUpsideDown = autoFlipTextToPreventUpsideDown;
            this.durationInSec = durationInSec;
        }
    }

    public struct TextScreenspaceFramed_3Dpos_dirViaVec_cam
    {
        public Camera screenCamera;
        public string text;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public float size_relToViewportHeight;
        public Vector2 textDirection;
        public DrawText.TextAnchorDXXL textAnchor;
        public DrawBasics.LineStyle enclosingBoxLineStyle;
        public float enclosingBox_lineWidth_relToTextSize;
        public float enclosingBox_paddingSize_relToTextSize;
        public float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth;
        public float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth;
        public bool autoLineBreakAtViewportBorder;
        public float autoLineBreakWidth_relToViewportWidth;
        public bool autoFlipTextToPreventUpsideDown;
        public float durationInSec;
        public TextScreenspaceFramed_3Dpos_dirViaVec_cam(Camera screenCamera, string text, Vector3 position_in3DWorldspace, Color color, float size_relToViewportHeight, Vector2 textDirection, DrawText.TextAnchorDXXL textAnchor, DrawBasics.LineStyle enclosingBoxLineStyle, float enclosingBox_lineWidth_relToTextSize, float enclosingBox_paddingSize_relToTextSize, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, bool autoLineBreakAtViewportBorder, float autoLineBreakWidth_relToViewportWidth, bool autoFlipTextToPreventUpsideDown, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.text = text;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.textDirection = textDirection;
            this.textAnchor = textAnchor;
            this.enclosingBoxLineStyle = enclosingBoxLineStyle;
            this.enclosingBox_lineWidth_relToTextSize = enclosingBox_lineWidth_relToTextSize;
            this.enclosingBox_paddingSize_relToTextSize = enclosingBox_paddingSize_relToTextSize;
            this.forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = forceTextBlockEnlargementToThisMinWidth_relToViewportWidth;
            this.forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth;
            this.autoLineBreakAtViewportBorder = autoLineBreakAtViewportBorder;
            this.autoLineBreakWidth_relToViewportWidth = autoLineBreakWidth_relToViewportWidth;
            this.autoFlipTextToPreventUpsideDown = autoFlipTextToPreventUpsideDown;
            this.durationInSec = durationInSec;
        }
    }

    public struct TextScreenspaceFramed_2Dpos_dirViaVec_cam
    {
        public Camera screenCamera;
        public string text;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public float size_relToViewportHeight;
        public Vector2 textDirection;
        public DrawText.TextAnchorDXXL textAnchor;
        public DrawBasics.LineStyle enclosingBoxLineStyle;
        public float enclosingBox_lineWidth_relToTextSize;
        public float enclosingBox_paddingSize_relToTextSize;
        public float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth;
        public float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth;
        public bool autoLineBreakAtViewportBorder;
        public float autoLineBreakWidth_relToViewportWidth;
        public bool autoFlipTextToPreventUpsideDown;
        public float durationInSec;
        public TextScreenspaceFramed_2Dpos_dirViaVec_cam(Camera screenCamera, string text, Vector2 position_in2DViewportSpace, Color color, float size_relToViewportHeight, Vector2 textDirection, DrawText.TextAnchorDXXL textAnchor, DrawBasics.LineStyle enclosingBoxLineStyle, float enclosingBox_lineWidth_relToTextSize, float enclosingBox_paddingSize_relToTextSize, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, bool autoLineBreakAtViewportBorder, float autoLineBreakWidth_relToViewportWidth, bool autoFlipTextToPreventUpsideDown, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.text = text;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.textDirection = textDirection;
            this.textAnchor = textAnchor;
            this.enclosingBoxLineStyle = enclosingBoxLineStyle;
            this.enclosingBox_lineWidth_relToTextSize = enclosingBox_lineWidth_relToTextSize;
            this.enclosingBox_paddingSize_relToTextSize = enclosingBox_paddingSize_relToTextSize;
            this.forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = forceTextBlockEnlargementToThisMinWidth_relToViewportWidth;
            this.forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth;
            this.autoLineBreakAtViewportBorder = autoLineBreakAtViewportBorder;
            this.autoLineBreakWidth_relToViewportWidth = autoLineBreakWidth_relToViewportWidth;
            this.autoFlipTextToPreventUpsideDown = autoFlipTextToPreventUpsideDown;
            this.durationInSec = durationInSec;
        }
    }

    public struct TextScreenspace_3Dpos_dirViaAngle_cam
    {
        public Camera screenCamera;
        public string text;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public float size_relToViewportHeight;
        public float zRotationDegCC;
        public DrawText.TextAnchorDXXL textAnchor;
        public float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth;
        public float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth;
        public bool autoLineBreakAtViewportBorder;
        public float autoLineBreakWidth_relToViewportWidth;
        public bool autoFlipTextToPreventUpsideDown;
        public float durationInSec;
        public TextScreenspace_3Dpos_dirViaAngle_cam(Camera screenCamera, string text, Vector3 position_in3DWorldspace, Color color, float size_relToViewportHeight, float zRotationDegCC, DrawText.TextAnchorDXXL textAnchor, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, bool autoLineBreakAtViewportBorder, float autoLineBreakWidth_relToViewportWidth, bool autoFlipTextToPreventUpsideDown, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.text = text;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.zRotationDegCC = zRotationDegCC;
            this.textAnchor = textAnchor;
            this.forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = forceTextBlockEnlargementToThisMinWidth_relToViewportWidth;
            this.forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth;
            this.autoLineBreakAtViewportBorder = autoLineBreakAtViewportBorder;
            this.autoLineBreakWidth_relToViewportWidth = autoLineBreakWidth_relToViewportWidth;
            this.autoFlipTextToPreventUpsideDown = autoFlipTextToPreventUpsideDown;
            this.durationInSec = durationInSec;
        }
    }

    public struct TextScreenspace_2Dpos_dirViaAngle_cam
    {
        public Camera screenCamera;
        public string text;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public float size_relToViewportHeight;
        public float zRotationDegCC;
        public DrawText.TextAnchorDXXL textAnchor;
        public float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth;
        public float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth;
        public bool autoLineBreakAtViewportBorder;
        public float autoLineBreakWidth_relToViewportWidth;
        public bool autoFlipTextToPreventUpsideDown;
        public float durationInSec;

        public TextScreenspace_2Dpos_dirViaAngle_cam(Camera screenCamera, string text, Vector2 position_in2DViewportSpace, Color color, float size_relToViewportHeight, float zRotationDegCC, DrawText.TextAnchorDXXL textAnchor, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, bool autoLineBreakAtViewportBorder, float autoLineBreakWidth_relToViewportWidth, bool autoFlipTextToPreventUpsideDown, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.text = text;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.zRotationDegCC = zRotationDegCC;
            this.textAnchor = textAnchor;
            this.forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = forceTextBlockEnlargementToThisMinWidth_relToViewportWidth;
            this.forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth;
            this.autoLineBreakAtViewportBorder = autoLineBreakAtViewportBorder;
            this.autoLineBreakWidth_relToViewportWidth = autoLineBreakWidth_relToViewportWidth;
            this.autoFlipTextToPreventUpsideDown = autoFlipTextToPreventUpsideDown;
            this.durationInSec = durationInSec;
        }
    }

    public struct TextScreenspaceFramed_3Dpos_dirViaAngle_cam
    {
        public Camera screenCamera;
        public string text;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public float size_relToViewportHeight;
        public float zRotationDegCC;
        public DrawText.TextAnchorDXXL textAnchor;
        public DrawBasics.LineStyle enclosingBoxLineStyle;
        public float enclosingBox_lineWidth_relToTextSize;
        public float enclosingBox_paddingSize_relToTextSize;
        public float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth;
        public float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth;
        public bool autoLineBreakAtViewportBorder;
        public float autoLineBreakWidth_relToViewportWidth;
        public bool autoFlipTextToPreventUpsideDown;
        public float durationInSec;

        public TextScreenspaceFramed_3Dpos_dirViaAngle_cam(Camera screenCamera, string text, Vector3 position_in3DWorldspace, Color color, float size_relToViewportHeight, float zRotationDegCC, DrawText.TextAnchorDXXL textAnchor, DrawBasics.LineStyle enclosingBoxLineStyle, float enclosingBox_lineWidth_relToTextSize, float enclosingBox_paddingSize_relToTextSize, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, bool autoLineBreakAtViewportBorder, float autoLineBreakWidth_relToViewportWidth, bool autoFlipTextToPreventUpsideDown, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.text = text;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.zRotationDegCC = zRotationDegCC;
            this.textAnchor = textAnchor;
            this.enclosingBoxLineStyle = enclosingBoxLineStyle;
            this.enclosingBox_lineWidth_relToTextSize = enclosingBox_lineWidth_relToTextSize;
            this.enclosingBox_paddingSize_relToTextSize = enclosingBox_paddingSize_relToTextSize;
            this.forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = forceTextBlockEnlargementToThisMinWidth_relToViewportWidth;
            this.forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth;
            this.autoLineBreakAtViewportBorder = autoLineBreakAtViewportBorder;
            this.autoLineBreakWidth_relToViewportWidth = autoLineBreakWidth_relToViewportWidth;
            this.autoFlipTextToPreventUpsideDown = autoFlipTextToPreventUpsideDown;
            this.durationInSec = durationInSec;
        }
    }

    public struct TextScreenspaceFramed_2Dpos_dirViaAngle_cam
    {
        public Camera screenCamera;
        public string text;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public float size_relToViewportHeight;
        public float zRotationDegCC;
        public DrawText.TextAnchorDXXL textAnchor;
        public DrawBasics.LineStyle enclosingBoxLineStyle;
        public float enclosingBox_lineWidth_relToTextSize;
        public float enclosingBox_paddingSize_relToTextSize;
        public float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth;
        public float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth;
        public bool autoLineBreakAtViewportBorder;
        public float autoLineBreakWidth_relToViewportWidth;
        public bool autoFlipTextToPreventUpsideDown;
        public float durationInSec;

        public TextScreenspaceFramed_2Dpos_dirViaAngle_cam(Camera screenCamera, string text, Vector2 position_in2DViewportSpace, Color color, float size_relToViewportHeight, float zRotationDegCC, DrawText.TextAnchorDXXL textAnchor, DrawBasics.LineStyle enclosingBoxLineStyle, float enclosingBox_lineWidth_relToTextSize, float enclosingBox_paddingSize_relToTextSize, float forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, float forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, bool autoLineBreakAtViewportBorder, float autoLineBreakWidth_relToViewportWidth, bool autoFlipTextToPreventUpsideDown, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.text = text;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.zRotationDegCC = zRotationDegCC;
            this.textAnchor = textAnchor;
            this.enclosingBoxLineStyle = enclosingBoxLineStyle;
            this.enclosingBox_lineWidth_relToTextSize = enclosingBox_lineWidth_relToTextSize;
            this.enclosingBox_paddingSize_relToTextSize = enclosingBox_paddingSize_relToTextSize;
            this.forceTextBlockEnlargementToThisMinWidth_relToViewportWidth = forceTextBlockEnlargementToThisMinWidth_relToViewportWidth;
            this.forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth = forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth;
            this.autoLineBreakAtViewportBorder = autoLineBreakAtViewportBorder;
            this.autoLineBreakWidth_relToViewportWidth = autoLineBreakWidth_relToViewportWidth;
            this.autoFlipTextToPreventUpsideDown = autoFlipTextToPreventUpsideDown;
            this.durationInSec = durationInSec;
        }
    }

    public struct TextOnCircleScreenspace_viaStartPos_cam
    {
        public Camera screenCamera;
        public string text;
        public Vector2 textStartPos;
        public Vector2 circleCenterPosition;
        public Color color;
        public float size_relToViewportHeight;
        public DrawText.TextAnchorCircledDXXL textAnchor;
        public float autoLineBreakAngleDeg;
        public float durationInSec;

        public TextOnCircleScreenspace_viaStartPos_cam(Camera screenCamera, string text, Vector2 textStartPos, Vector2 circleCenterPosition, Color color, float size_relToViewportHeight, DrawText.TextAnchorCircledDXXL textAnchor, float autoLineBreakAngleDeg, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.text = text;
            this.textStartPos = textStartPos;
            this.circleCenterPosition = circleCenterPosition;
            this.color = color;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.textAnchor = textAnchor;
            this.autoLineBreakAngleDeg = autoLineBreakAngleDeg;
            this.durationInSec = durationInSec;
        }
    }

    public struct TextOnCircleScreenspace_dirViaVecUp_cam
    {
        public Camera screenCamera;
        public string text;
        public Vector2 circleCenterPosition;
        public float radius_relToViewportHeight;
        public Color color;
        public float size_relToViewportHeight;
        public Vector2 textsInitialUp;
        public DrawText.TextAnchorCircledDXXL textAnchor;
        public float autoLineBreakAngleDeg;
        public float durationInSec;

        public TextOnCircleScreenspace_dirViaVecUp_cam(Camera screenCamera, string text, Vector2 circleCenterPosition, float radius_relToViewportHeight, Color color, float size_relToViewportHeight, Vector2 textsInitialUp, DrawText.TextAnchorCircledDXXL textAnchor, float autoLineBreakAngleDeg, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.text = text;
            this.circleCenterPosition = circleCenterPosition;
            this.radius_relToViewportHeight = radius_relToViewportHeight;
            this.color = color;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.textsInitialUp = textsInitialUp;
            this.textAnchor = textAnchor;
            this.autoLineBreakAngleDeg = autoLineBreakAngleDeg;
            this.durationInSec = durationInSec;
        }
    }

    public struct TextOnCircleScreenspace_dirViaAngle_cam
    {
        public Camera screenCamera;
        public string text;
        public Vector2 circleCenterPosition;
        public float radius_relToViewportHeight;
        public Color color;
        public float size_relToViewportHeight;
        public float initialTextDirection_as_zRotationDegCCfromCamUp;
        public DrawText.TextAnchorCircledDXXL textAnchor;
        public float autoLineBreakAngleDeg;
        public float durationInSec;

        public TextOnCircleScreenspace_dirViaAngle_cam(Camera screenCamera, string text, Vector2 circleCenterPosition, float radius_relToViewportHeight, Color color, float size_relToViewportHeight, float initialTextDirection_as_zRotationDegCCfromCamUp, DrawText.TextAnchorCircledDXXL textAnchor, float autoLineBreakAngleDeg, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.text = text;
            this.circleCenterPosition = circleCenterPosition;
            this.radius_relToViewportHeight = radius_relToViewportHeight;
            this.color = color;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.initialTextDirection_as_zRotationDegCCfromCamUp = initialTextDirection_as_zRotationDegCCfromCamUp;
            this.textAnchor = textAnchor;
            this.autoLineBreakAngleDeg = autoLineBreakAngleDeg;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfBool_screenspace_3Dpos
    {
        public bool[] boolArray;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfBool_screenspace_3Dpos(bool[] boolArray, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.boolArray = boolArray;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfBool_screenspace_3Dpos_cam
    {
        public Camera screenCamera;
        public bool[] boolArray;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfBool_screenspace_3Dpos_cam(Camera screenCamera, bool[] boolArray, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.boolArray = boolArray;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfBool_screenspace_2Dpos
    {
        public bool[] boolArray;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfBool_screenspace_2Dpos(bool[] boolArray, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.boolArray = boolArray;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfBool_screenspace_2Dpos_cam
    {
        public Camera screenCamera;
        public bool[] boolArray;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfBool_screenspace_2Dpos_cam(Camera screenCamera, bool[] boolArray, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.boolArray = boolArray;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfBool_screenspace_3Dpos
    {
        public List<bool> boolList;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfBool_screenspace_3Dpos(List<bool> boolList, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.boolList = boolList;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfBool_screenspace_3Dpos_cam
    {
        public Camera screenCamera;
        public List<bool> boolList;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfBool_screenspace_3Dpos_cam(Camera screenCamera, List<bool> boolList, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.boolList = boolList;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfBool_screenspace_2Dpos
    {
        public List<bool> boolList;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfBool_screenspace_2Dpos(List<bool> boolList, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.boolList = boolList;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfBool_screenspace_2Dpos_cam
    {
        public Camera screenCamera;
        public List<bool> boolList;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfBool_screenspace_2Dpos_cam(Camera screenCamera, List<bool> boolList, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.boolList = boolList;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfInt_screenspace_3Dpos
    {
        public int[] intArray;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfInt_screenspace_3Dpos(int[] intArray, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.intArray = intArray;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfInt_screenspace_3Dpos_cam
    {
        public Camera screenCamera;
        public int[] intArray;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfInt_screenspace_3Dpos_cam(Camera screenCamera, int[] intArray, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.intArray = intArray;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfInt_screenspace_2Dpos
    {
        public int[] intArray;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfInt_screenspace_2Dpos(int[] intArray, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.intArray = intArray;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfInt_screenspace_2Dpos_cam
    {
        public Camera screenCamera;
        public int[] intArray;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfInt_screenspace_2Dpos_cam(Camera screenCamera, int[] intArray, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.intArray = intArray;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfInt_screenspace_3Dpos
    {
        public List<int> intList;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfInt_screenspace_3Dpos(List<int> intList, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.intList = intList;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfInt_screenspace_3Dpos_cam
    {
        public Camera screenCamera;
        public List<int> intList;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfInt_screenspace_3Dpos_cam(Camera screenCamera, List<int> intList, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.intList = intList;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfInt_screenspace_2Dpos
    {
        public List<int> intList;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfInt_screenspace_2Dpos(List<int> intList, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.intList = intList;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfInt_screenspace_2Dpos_cam
    {
        public Camera screenCamera;
        public List<int> intList;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfInt_screenspace_2Dpos_cam(Camera screenCamera, List<int> intList, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.intList = intList;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfFloat_screenspace_3Dpos
    {
        public float[] floatArray;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfFloat_screenspace_3Dpos(float[] floatArray, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.floatArray = floatArray;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfFloat_screenspace_3Dpos_cam
    {
        public Camera screenCamera;
        public float[] floatArray;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfFloat_screenspace_3Dpos_cam(Camera screenCamera, float[] floatArray, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.floatArray = floatArray;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfFloat_screenspace_2Dpos
    {
        public float[] floatArray;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfFloat_screenspace_2Dpos(float[] floatArray, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.floatArray = floatArray;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfFloat_screenspace_2Dpos_cam
    {
        public Camera screenCamera;
        public float[] floatArray;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfFloat_screenspace_2Dpos_cam(Camera screenCamera, float[] floatArray, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.floatArray = floatArray;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfFloat_screenspace_3Dpos
    {
        public List<float> floatList;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfFloat_screenspace_3Dpos(List<float> floatList, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.floatList = floatList;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfFloat_screenspace_3Dpos_cam
    {
        public Camera screenCamera;
        public List<float> floatList;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfFloat_screenspace_3Dpos_cam(Camera screenCamera, List<float> floatList, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.floatList = floatList;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfFloat_screenspace_2Dpos
    {
        public List<float> floatList;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfFloat_screenspace_2Dpos(List<float> floatList, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.floatList = floatList;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfFloat_screenspace_2Dpos_cam
    {
        public Camera screenCamera;
        public List<float> floatList;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfFloat_screenspace_2Dpos_cam(Camera screenCamera, List<float> floatList, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.floatList = floatList;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfString_screenspace_3Dpos
    {
        public string[] stringArray;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfString_screenspace_3Dpos(string[] stringArray, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.stringArray = stringArray;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfString_screenspace_3Dpos_cam
    {
        public Camera screenCamera;
        public string[] stringArray;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfString_screenspace_3Dpos_cam(Camera screenCamera, string[] stringArray, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.stringArray = stringArray;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfString_screenspace_2Dpos
    {
        public string[] stringArray;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfString_screenspace_2Dpos(string[] stringArray, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.stringArray = stringArray;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfString_screenspace_2Dpos_cam
    {
        public Camera screenCamera;
        public string[] stringArray;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfString_screenspace_2Dpos_cam(Camera screenCamera, string[] stringArray, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.stringArray = stringArray;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfString_screenspace_3Dpos
    {
        public List<string> stringList;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfString_screenspace_3Dpos(List<string> stringList, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.stringList = stringList;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfString_screenspace_3Dpos_cam
    {
        public Camera screenCamera;
        public List<string> stringList;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfString_screenspace_3Dpos_cam(Camera screenCamera, List<string> stringList, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.stringList = stringList;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfString_screenspace_2Dpos
    {
        public List<string> stringList;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfString_screenspace_2Dpos(List<string> stringList, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.stringList = stringList;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfString_screenspace_2Dpos_cam
    {
        public Camera screenCamera;
        public List<string> stringList;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfString_screenspace_2Dpos_cam(Camera screenCamera, List<string> stringList, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.stringList = stringList;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfVector2_screenspace_3Dpos
    {
        public Vector2[] vector2Array;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfVector2_screenspace_3Dpos(Vector2[] vector2Array, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.vector2Array = vector2Array;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfVector2_screenspace_3Dpos_cam
    {
        public Camera screenCamera;
        public Vector2[] vector2Array;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfVector2_screenspace_3Dpos_cam(Camera screenCamera, Vector2[] vector2Array, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.vector2Array = vector2Array;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfVector2_screenspace_2Dpos
    {
        public Vector2[] vector2Array;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfVector2_screenspace_2Dpos(Vector2[] vector2Array, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.vector2Array = vector2Array;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfVector2_screenspace_2Dpos_cam
    {
        public Camera screenCamera;
        public Vector2[] vector2Array;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfVector2_screenspace_2Dpos_cam(Camera screenCamera, Vector2[] vector2Array, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.vector2Array = vector2Array;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfVector2_screenspace_3Dpos
    {
        public List<Vector2> vector2List;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfVector2_screenspace_3Dpos(List<Vector2> vector2List, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.vector2List = vector2List;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfVector2_screenspace_3Dpos_cam
    {
        public Camera screenCamera;
        public List<Vector2> vector2List;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfVector2_screenspace_3Dpos_cam(Camera screenCamera, List<Vector2> vector2List, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.vector2List = vector2List;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfVector2_screenspace_2Dpos
    {
        public List<Vector2> vector2List;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfVector2_screenspace_2Dpos(List<Vector2> vector2List, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.vector2List = vector2List;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfVector2_screenspace_2Dpos_cam
    {
        public Camera screenCamera;
        public List<Vector2> vector2List;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfVector2_screenspace_2Dpos_cam(Camera screenCamera, List<Vector2> vector2List, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.vector2List = vector2List;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfVector3_screenspace_3Dpos
    {
        public Vector3[] vector3Array;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfVector3_screenspace_3Dpos(Vector3[] vector3Array, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.vector3Array = vector3Array;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfVector3_screenspace_3Dpos_cam
    {
        public Camera screenCamera;
        public Vector3[] vector3Array;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfVector3_screenspace_3Dpos_cam(Camera screenCamera, Vector3[] vector3Array, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.vector3Array = vector3Array;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfVector3_screenspace_2Dpos
    {
        public Vector3[] vector3Array;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfVector3_screenspace_2Dpos(Vector3[] vector3Array, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.vector3Array = vector3Array;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfVector3_screenspace_2Dpos_cam
    {
        public Camera screenCamera;
        public Vector3[] vector3Array;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfVector3_screenspace_2Dpos_cam(Camera screenCamera, Vector3[] vector3Array, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.vector3Array = vector3Array;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfVector3_screenspace_3Dpos
    {
        public List<Vector3> vector3List;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfVector3_screenspace_3Dpos(List<Vector3> vector3List, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.vector3List = vector3List;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfVector3_screenspace_3Dpos_cam
    {
        public Camera screenCamera;
        public List<Vector3> vector3List;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfVector3_screenspace_3Dpos_cam(Camera screenCamera, List<Vector3> vector3List, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.vector3List = vector3List;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfVector3_screenspace_2Dpos
    {
        public List<Vector3> vector3List;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfVector3_screenspace_2Dpos(List<Vector3> vector3List, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.vector3List = vector3List;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfVector3_screenspace_2Dpos_cam
    {
        public Camera screenCamera;
        public List<Vector3> vector3List;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfVector3_screenspace_2Dpos_cam(Camera screenCamera, List<Vector3> vector3List, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.vector3List = vector3List;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfVector4_screenspace_3Dpos
    {
        public Vector4[] vector4Array;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfVector4_screenspace_3Dpos(Vector4[] vector4Array, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.vector4Array = vector4Array;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfVector4_screenspace_3Dpos_cam
    {
        public Camera screenCamera;
        public Vector4[] vector4Array;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfVector4_screenspace_3Dpos_cam(Camera screenCamera, Vector4[] vector4Array, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.vector4Array = vector4Array;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfVector4_screenspace_2Dpos
    {
        public Vector4[] vector4Array;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfVector4_screenspace_2Dpos(Vector4[] vector4Array, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.vector4Array = vector4Array;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ArrayOfVector4_screenspace_2Dpos_cam
    {
        public Camera screenCamera;
        public Vector4[] vector4Array;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ArrayOfVector4_screenspace_2Dpos_cam(Camera screenCamera, Vector4[] vector4Array, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.vector4Array = vector4Array;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfVector4_screenspace_3Dpos
    {
        public List<Vector4> vector4List;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfVector4_screenspace_3Dpos(List<Vector4> vector4List, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.vector4List = vector4List;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfVector4_screenspace_3Dpos_cam
    {
        public Camera screenCamera;
        public List<Vector4> vector4List;
        public Vector3 position_in3DWorldspace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfVector4_screenspace_3Dpos_cam(Camera screenCamera, List<Vector4> vector4List, Vector3 position_in3DWorldspace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.vector4List = vector4List;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfVector4_screenspace_2Dpos
    {
        public List<Vector4> vector4List;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfVector4_screenspace_2Dpos(List<Vector4> vector4List, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.vector4List = vector4List;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct ListOfVector4_screenspace_2Dpos_cam
    {
        public Camera screenCamera;
        public List<Vector4> vector4List;
        public Vector2 position_in2DViewportSpace;
        public Color color;
        public string title;
        public float textSize_relToViewportHeight;
        public float forceHeightOfWholeTableBox_relToViewportHeight;
        public bool position_isTopLeft_notLowLeft;
        public float durationInSec;

        public ListOfVector4_screenspace_2Dpos_cam(Camera screenCamera, List<Vector4> vector4List, Vector2 position_in2DViewportSpace, Color color, string title, float textSize_relToViewportHeight, float forceHeightOfWholeTableBox_relToViewportHeight, bool position_isTopLeft_notLowLeft, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.vector4List = vector4List;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.color = color;
            this.title = title;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.forceHeightOfWholeTableBox_relToViewportHeight = forceHeightOfWholeTableBox_relToViewportHeight;
            this.position_isTopLeft_notLowLeft = position_isTopLeft_notLowLeft;
            this.durationInSec = durationInSec;
        }
    }

    public struct TagGameObjectScreenspace
    {
        public Camera screenCamera;
        public GameObject gameObject;
        public string text;
        public Color colorForText;
        public Color colorForTagBox;
        public float linesWidth_relToViewportHeight;
        public bool drawPointerIfOffscreen;
        public float relTextSizeScaling;
        public bool encapsulateChildren;
        public float durationInSec;

        public TagGameObjectScreenspace(Camera screenCamera, GameObject gameObject, string text, Color colorForText, Color colorForTagBox, float linesWidth_relToViewportHeight, bool drawPointerIfOffscreen, float relTextSizeScaling, bool encapsulateChildren, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.gameObject = gameObject;
            this.text = text;
            this.colorForText = colorForText;
            this.colorForTagBox = colorForTagBox;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.relTextSizeScaling = relTextSizeScaling;
            this.encapsulateChildren = encapsulateChildren;
            this.durationInSec = durationInSec;
        }
    }

    public struct GridScreenspace
    {
        public Camera camera;
        public Color color;
        public float linesWidth_relToViewportHeight;
        public bool drawTenthLines;
        public bool drawHundredthLines;
        public DrawEngineBasics.GridScreenspaceMode gridScreenspaceMode;
        public float durationInSec;

        public GridScreenspace(Camera camera, Color color, float linesWidth_relToViewportHeight, bool drawTenthLines, bool drawHundredthLines, DrawEngineBasics.GridScreenspaceMode gridScreenspaceMode, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.camera = camera;
            this.color = color;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.drawTenthLines = drawTenthLines;
            this.drawHundredthLines = drawHundredthLines;
            this.gridScreenspaceMode = gridScreenspaceMode;
            this.durationInSec = durationInSec;
        }
    }

    public struct BoolDisplayerScreenspace_3Dpos
    {
        public bool boolValueToDisplay;
        public string boolName;
        public Vector3 position_in3DWorldspace;
        public float size_relToViewportHeight;
        public Color color_forTextAndFrame;
        public Color overwriteColor_forTrue;
        public Color overwriteColor_forFalse;
        public float durationInSec;

        public BoolDisplayerScreenspace_3Dpos(bool boolValueToDisplay, string boolName, Vector3 position_in3DWorldspace, float size_relToViewportHeight, Color color_forTextAndFrame, Color overwriteColor_forTrue, Color overwriteColor_forFalse, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.boolValueToDisplay = boolValueToDisplay;
            this.boolName = boolName;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.color_forTextAndFrame = color_forTextAndFrame;
            this.overwriteColor_forTrue = overwriteColor_forTrue;
            this.overwriteColor_forFalse = overwriteColor_forFalse;
            this.durationInSec = durationInSec;
        }
    }

    public struct BoolDisplayerScreenspace_3Dpos_cam
    {
        public Camera screenCamera;
        public bool boolValueToDisplay;
        public string boolName;
        public Vector3 position_in3DWorldspace;
        public float size_relToViewportHeight;
        public Color color_forTextAndFrame;
        public Color overwriteColor_forTrue;
        public Color overwriteColor_forFalse;
        public float durationInSec;

        public BoolDisplayerScreenspace_3Dpos_cam(Camera screenCamera, bool boolValueToDisplay, string boolName, Vector3 position_in3DWorldspace, float size_relToViewportHeight, Color color_forTextAndFrame, Color overwriteColor_forTrue, Color overwriteColor_forFalse, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.boolValueToDisplay = boolValueToDisplay;
            this.boolName = boolName;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.color_forTextAndFrame = color_forTextAndFrame;
            this.overwriteColor_forTrue = overwriteColor_forTrue;
            this.overwriteColor_forFalse = overwriteColor_forFalse;
            this.durationInSec = durationInSec;
        }
    }

    public struct BoolDisplayerScreenspace_2Dpos_cam
    {
        public Camera screenCamera;
        public bool boolValueToDisplay;
        public string boolName;
        public Vector2 position_in2DViewportSpace;
        public float size_relToViewportHeight;
        public Color color_forTextAndFrame;
        public Color overwriteColor_forTrue;
        public Color overwriteColor_forFalse;
        public float durationInSec;

        public BoolDisplayerScreenspace_2Dpos_cam(Camera screenCamera, bool boolValueToDisplay, string boolName, Vector2 position_in2DViewportSpace, float size_relToViewportHeight, Color color_forTextAndFrame, Color overwriteColor_forTrue, Color overwriteColor_forFalse, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.screenCamera = screenCamera;
            this.boolValueToDisplay = boolValueToDisplay;
            this.boolName = boolName;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.color_forTextAndFrame = color_forTextAndFrame;
            this.overwriteColor_forTrue = overwriteColor_forTrue;
            this.overwriteColor_forFalse = overwriteColor_forFalse;
            this.durationInSec = durationInSec;
        }
    }

    public struct LogsOnScreen
    {
        public Camera cameraWhereToDraw;
        public bool drawNormalPrio;
        public bool drawWarningPrio;
        public bool drawErrorPrio;
        public int maxNumberOfDisplayedLogMessages;
        public float textSize_relToViewportHeight;
        public Color textColor;
        public bool stackTraceForNormalPrio;
        public bool stackTraceForWarningPrio;
        public bool stackTraceForErrorPrio;
        public float durationInSec;
        public bool logListenerWasActive; //-> additional member that deviates from the normal pattern inside this script file. This is for keeping the functionality that the log listener can be activated and deactivated for different code blocks.

        public LogsOnScreen(Camera cameraWhereToDraw, bool drawNormalPrio, bool drawWarningPrio, bool drawErrorPrio, int maxNumberOfDisplayedLogMessages, float textSize_relToViewportHeight, Color textColor, bool stackTraceForNormalPrio, bool stackTraceForWarningPrio, bool stackTraceForErrorPrio, float durationInSec, bool logListenerWasActive)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.cameraWhereToDraw = cameraWhereToDraw;
            this.drawNormalPrio = drawNormalPrio;
            this.drawWarningPrio = drawWarningPrio;
            this.drawErrorPrio = drawErrorPrio;
            this.maxNumberOfDisplayedLogMessages = maxNumberOfDisplayedLogMessages;
            this.textSize_relToViewportHeight = textSize_relToViewportHeight;
            this.textColor = textColor;
            this.stackTraceForNormalPrio = stackTraceForNormalPrio;
            this.stackTraceForWarningPrio = stackTraceForWarningPrio;
            this.stackTraceForErrorPrio = stackTraceForErrorPrio;
            this.durationInSec = durationInSec;
            this.logListenerWasActive = logListenerWasActive;
        }
    }

    public struct ScreenspaceLine
    {
        public Camera targetCamera;
        public Vector2 start;
        public Vector2 end;
        public Color color;
        public float width_relToViewportHeight;
        public string text;
        public DrawBasics.LineStyle style;
        public float stylePatternScaleFactor;
        public float animationSpeed;
        public float endPlatesSize_relToViewportHeight;
        public float alphaFadeOutLength_0to1;
        public float enlargeSmallTextToThisMinRelTextSize;
        public float durationInSec;
        public ScreenspaceLine(Camera targetCamera, Vector2 start, Vector2 end, Color color, float width_relToViewportHeight, string text, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, float endPlatesSize_relToViewportHeight, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinRelTextSize, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.start = start;
            this.end = end;
            this.color = color;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.text = text;
            this.style = style;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.animationSpeed = animationSpeed;
            this.endPlatesSize_relToViewportHeight = endPlatesSize_relToViewportHeight;
            this.alphaFadeOutLength_0to1 = alphaFadeOutLength_0to1;
            this.enlargeSmallTextToThisMinRelTextSize = enlargeSmallTextToThisMinRelTextSize;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceRay
    {
        public Camera targetCamera;
        public Vector2 start;
        public Vector2 direction;
        public Color color;
        public float width_relToViewportHeight;
        public string text;
        public bool interpretDirectionAsUnwarped;
        public DrawBasics.LineStyle style;
        public float stylePatternScaleFactor;
        public float animationSpeed;
        public float endPlatesSize_relToViewportHeight;
        public float alphaFadeOutLength_0to1;
        public float enlargeSmallTextToThisMinRelTextSize;
        public float durationInSec;
        public ScreenspaceRay(Camera targetCamera, Vector2 start, Vector2 direction, Color color, float width_relToViewportHeight, string text, bool interpretDirectionAsUnwarped, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, float endPlatesSize_relToViewportHeight, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinRelTextSize, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.start = start;
            this.direction = direction;
            this.color = color;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.text = text;
            this.interpretDirectionAsUnwarped = interpretDirectionAsUnwarped;
            this.style = style;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.animationSpeed = animationSpeed;
            this.endPlatesSize_relToViewportHeight = endPlatesSize_relToViewportHeight;
            this.alphaFadeOutLength_0to1 = alphaFadeOutLength_0to1;
            this.enlargeSmallTextToThisMinRelTextSize = enlargeSmallTextToThisMinRelTextSize;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceLineFrom
    {
        public Camera targetCamera;
        public Vector2 start;
        public Vector2 direction;
        public Color color;
        public float width_relToViewportHeight;
        public string text;
        public bool interpretDirectionAsUnwarped;
        public DrawBasics.LineStyle style;
        public float stylePatternScaleFactor;
        public float animationSpeed;
        public float endPlatesSize_relToViewportHeight;
        public float alphaFadeOutLength_0to1;
        public float enlargeSmallTextToThisMinRelTextSize;
        public float durationInSec;
        public ScreenspaceLineFrom(Camera targetCamera, Vector2 start, Vector2 direction, Color color, float width_relToViewportHeight, string text, bool interpretDirectionAsUnwarped, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, float endPlatesSize_relToViewportHeight, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinRelTextSize, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.start = start;
            this.direction = direction;
            this.color = color;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.text = text;
            this.interpretDirectionAsUnwarped = interpretDirectionAsUnwarped;
            this.style = style;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.animationSpeed = animationSpeed;
            this.endPlatesSize_relToViewportHeight = endPlatesSize_relToViewportHeight;
            this.alphaFadeOutLength_0to1 = alphaFadeOutLength_0to1;
            this.enlargeSmallTextToThisMinRelTextSize = enlargeSmallTextToThisMinRelTextSize;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceLineTo
    {
        public Camera targetCamera;
        public Vector2 direction;
        public Vector2 end;
        public Color color;
        public float width_relToViewportHeight;
        public string text;
        public bool interpretDirectionAsUnwarped;
        public DrawBasics.LineStyle style;
        public float stylePatternScaleFactor;
        public float animationSpeed;
        public float endPlatesSize_relToViewportHeight;
        public float alphaFadeOutLength_0to1;
        public float enlargeSmallTextToThisMinRelTextSize;
        public float durationInSec;
        public ScreenspaceLineTo(Camera targetCamera, Vector2 direction, Vector2 end, Color color, float width_relToViewportHeight, string text, bool interpretDirectionAsUnwarped, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, float endPlatesSize_relToViewportHeight, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinRelTextSize, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.direction = direction;
            this.end = end;
            this.color = color;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.text = text;
            this.interpretDirectionAsUnwarped = interpretDirectionAsUnwarped;
            this.style = style;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.animationSpeed = animationSpeed;
            this.endPlatesSize_relToViewportHeight = endPlatesSize_relToViewportHeight;
            this.alphaFadeOutLength_0to1 = alphaFadeOutLength_0to1;
            this.enlargeSmallTextToThisMinRelTextSize = enlargeSmallTextToThisMinRelTextSize;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceLineColorFade
    {
        public Camera targetCamera;
        public Vector2 start;
        public Vector2 end;
        public Color startColor;
        public Color endColor;
        public float width_relToViewportHeight;
        public string text;
        public DrawBasics.LineStyle style;
        public float stylePatternScaleFactor;
        public float animationSpeed;
        public float endPlatesSize_relToViewportHeight;
        public float alphaFadeOutLength_0to1;
        public float enlargeSmallTextToThisMinRelTextSize;
        public float durationInSec;
        public ScreenspaceLineColorFade(Camera targetCamera, Vector2 start, Vector2 end, Color startColor, Color endColor, float width_relToViewportHeight, string text, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, float endPlatesSize_relToViewportHeight, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinRelTextSize, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.start = start;
            this.end = end;
            this.startColor = startColor;
            this.endColor = endColor;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.text = text;
            this.style = style;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.animationSpeed = animationSpeed;
            this.endPlatesSize_relToViewportHeight = endPlatesSize_relToViewportHeight;
            this.alphaFadeOutLength_0to1 = alphaFadeOutLength_0to1;
            this.enlargeSmallTextToThisMinRelTextSize = enlargeSmallTextToThisMinRelTextSize;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceRayColorFade
    {
        public Camera targetCamera;
        public Vector2 start;
        public Vector2 direction;
        public Color startColor;
        public Color endColor;
        public float width_relToViewportHeight;
        public string text;
        public bool interpretDirectionAsUnwarped;
        public DrawBasics.LineStyle style;
        public float stylePatternScaleFactor;
        public float animationSpeed;
        public float endPlatesSize_relToViewportHeight;
        public float alphaFadeOutLength_0to1;
        public float enlargeSmallTextToThisMinRelTextSize;
        public float durationInSec;
        public ScreenspaceRayColorFade(Camera targetCamera, Vector2 start, Vector2 direction, Color startColor, Color endColor, float width_relToViewportHeight, string text, bool interpretDirectionAsUnwarped, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, float endPlatesSize_relToViewportHeight, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinRelTextSize, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.start = start;
            this.direction = direction;
            this.startColor = startColor;
            this.endColor = endColor;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.text = text;
            this.interpretDirectionAsUnwarped = interpretDirectionAsUnwarped;
            this.style = style;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.animationSpeed = animationSpeed;
            this.endPlatesSize_relToViewportHeight = endPlatesSize_relToViewportHeight;
            this.alphaFadeOutLength_0to1 = alphaFadeOutLength_0to1;
            this.enlargeSmallTextToThisMinRelTextSize = enlargeSmallTextToThisMinRelTextSize;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceLineFrom_withColorFade
    {
        public Camera targetCamera;
        public Vector2 start;
        public Vector2 direction;
        public Color startColor;
        public Color endColor;
        public float width_relToViewportHeight;
        public string text;
        public bool interpretDirectionAsUnwarped;
        public DrawBasics.LineStyle style;
        public float stylePatternScaleFactor;
        public float animationSpeed;
        public float endPlatesSize_relToViewportHeight;
        public float alphaFadeOutLength_0to1;
        public float enlargeSmallTextToThisMinRelTextSize;
        public float durationInSec;
        public ScreenspaceLineFrom_withColorFade(Camera targetCamera, Vector2 start, Vector2 direction, Color startColor, Color endColor, float width_relToViewportHeight, string text, bool interpretDirectionAsUnwarped, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, float endPlatesSize_relToViewportHeight, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinRelTextSize, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.start = start;
            this.direction = direction;
            this.startColor = startColor;
            this.endColor = endColor;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.text = text;
            this.interpretDirectionAsUnwarped = interpretDirectionAsUnwarped;
            this.style = style;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.animationSpeed = animationSpeed;
            this.endPlatesSize_relToViewportHeight = endPlatesSize_relToViewportHeight;
            this.alphaFadeOutLength_0to1 = alphaFadeOutLength_0to1;
            this.enlargeSmallTextToThisMinRelTextSize = enlargeSmallTextToThisMinRelTextSize;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceLineTo_withColorFade
    {
        public Camera targetCamera;
        public Vector2 direction;
        public Vector2 end;
        public Color startColor;
        public Color endColor;
        public float width_relToViewportHeight;
        public string text;
        public bool interpretDirectionAsUnwarped;
        public DrawBasics.LineStyle style;
        public float stylePatternScaleFactor;
        public float animationSpeed;
        public float endPlatesSize_relToViewportHeight;
        public float alphaFadeOutLength_0to1;
        public float enlargeSmallTextToThisMinRelTextSize;
        public float durationInSec;
        public ScreenspaceLineTo_withColorFade(Camera targetCamera, Vector2 direction, Vector2 end, Color startColor, Color endColor, float width_relToViewportHeight, string text, bool interpretDirectionAsUnwarped, DrawBasics.LineStyle style, float stylePatternScaleFactor, float animationSpeed, float endPlatesSize_relToViewportHeight, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinRelTextSize, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.direction = direction;
            this.end = end;
            this.startColor = startColor;
            this.endColor = endColor;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.text = text;
            this.interpretDirectionAsUnwarped = interpretDirectionAsUnwarped;
            this.style = style;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.animationSpeed = animationSpeed;
            this.endPlatesSize_relToViewportHeight = endPlatesSize_relToViewportHeight;
            this.alphaFadeOutLength_0to1 = alphaFadeOutLength_0to1;
            this.enlargeSmallTextToThisMinRelTextSize = enlargeSmallTextToThisMinRelTextSize;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceLineCircled_angleToAngle_cam
    {
        public Camera targetCamera;
        public Vector2 circleCenter;
        public float startAngleDegCC_relativeToUp;
        public float endAngleDegCC_relativeToUp;
        public float radius_relToViewportHeight;
        public Color color;
        public float width_relToViewportHeight;
        public string text;
        public bool skipFallbackDisplayOfZeroAngles;
        public float minAngleDeg_withoutTextLineBreak;
        public DrawText.TextAnchorCircledDXXL textAnchor;
        public float durationInSec;
        public ScreenspaceLineCircled_angleToAngle_cam(Camera targetCamera, Vector2 circleCenter, float startAngleDegCC_relativeToUp, float endAngleDegCC_relativeToUp, float radius_relToViewportHeight, Color color, float width_relToViewportHeight, string text, bool skipFallbackDisplayOfZeroAngles, float minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL textAnchor, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.circleCenter = circleCenter;
            this.startAngleDegCC_relativeToUp = startAngleDegCC_relativeToUp;
            this.endAngleDegCC_relativeToUp = endAngleDegCC_relativeToUp;
            this.radius_relToViewportHeight = radius_relToViewportHeight;
            this.color = color;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.text = text;
            this.skipFallbackDisplayOfZeroAngles = skipFallbackDisplayOfZeroAngles;
            this.minAngleDeg_withoutTextLineBreak = minAngleDeg_withoutTextLineBreak;
            this.textAnchor = textAnchor;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceLineCircled_angleFromStartPos_cam
    {
        public Camera targetCamera;
        public Vector2 startPos;
        public Vector2 circleCenter;
        public float turnAngleDegCC;
        public Color color;
        public float width_relToViewportHeight;
        public string text;
        public bool skipFallbackDisplayOfZeroAngles;
        public float minAngleDeg_withoutTextLineBreak;
        public DrawText.TextAnchorCircledDXXL textAnchor;
        public float durationInSec;
        public ScreenspaceLineCircled_angleFromStartPos_cam(Camera targetCamera, Vector2 startPos, Vector2 circleCenter, float turnAngleDegCC, Color color, float width_relToViewportHeight, string text, bool skipFallbackDisplayOfZeroAngles, float minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL textAnchor, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.startPos = startPos;
            this.circleCenter = circleCenter;
            this.turnAngleDegCC = turnAngleDegCC;
            this.color = color;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.text = text;
            this.skipFallbackDisplayOfZeroAngles = skipFallbackDisplayOfZeroAngles;
            this.minAngleDeg_withoutTextLineBreak = minAngleDeg_withoutTextLineBreak;
            this.textAnchor = textAnchor;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceCircleSegment_angleToAngle_cam
    {
        public Camera targetCamera;
        public Vector2 circleCenter;
        public float startAngleDegCC_relativeToUp;
        public float endAngleDegCC_relativeToUp;
        public float radius_relToViewportHeight;
        public Color color;
        public string text;
        public float radiusPortionWhereDrawFillStarts;
        public bool skipFallbackDisplayOfZeroAngles;
        public float fillDensity;
        public float minAngleDeg_withoutTextLineBreak;
        public float durationInSec;
        public ScreenspaceCircleSegment_angleToAngle_cam(Camera targetCamera, Vector2 circleCenter, float startAngleDegCC_relativeToUp, float endAngleDegCC_relativeToUp, float radius_relToViewportHeight, Color color, string text, float radiusPortionWhereDrawFillStarts, bool skipFallbackDisplayOfZeroAngles, float fillDensity, float minAngleDeg_withoutTextLineBreak, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.circleCenter = circleCenter;
            this.startAngleDegCC_relativeToUp = startAngleDegCC_relativeToUp;
            this.endAngleDegCC_relativeToUp = endAngleDegCC_relativeToUp;
            this.radius_relToViewportHeight = radius_relToViewportHeight;
            this.color = color;
            this.text = text;
            this.radiusPortionWhereDrawFillStarts = radiusPortionWhereDrawFillStarts;
            this.skipFallbackDisplayOfZeroAngles = skipFallbackDisplayOfZeroAngles;
            this.fillDensity = fillDensity;
            this.minAngleDeg_withoutTextLineBreak = minAngleDeg_withoutTextLineBreak;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceCircleSegment_angleFromStartPos_cam
    {
        public Camera targetCamera;
        public Vector2 startPosOnPerimeter;
        public Vector2 circleCenter;
        public float turnAngleDegCC;
        public Color color;
        public string text;
        public float radiusPortionWhereDrawFillStarts;
        public bool skipFallbackDisplayOfZeroAngles;
        public float fillDensity;
        public float minAngleDeg_withoutTextLineBreak;
        public float durationInSec;
        public ScreenspaceCircleSegment_angleFromStartPos_cam(Camera targetCamera, Vector2 startPosOnPerimeter, Vector2 circleCenter, float turnAngleDegCC, Color color, string text, float radiusPortionWhereDrawFillStarts, bool skipFallbackDisplayOfZeroAngles, float fillDensity, float minAngleDeg_withoutTextLineBreak, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.startPosOnPerimeter = startPosOnPerimeter;
            this.circleCenter = circleCenter;
            this.turnAngleDegCC = turnAngleDegCC;
            this.color = color;
            this.text = text;
            this.radiusPortionWhereDrawFillStarts = radiusPortionWhereDrawFillStarts;
            this.skipFallbackDisplayOfZeroAngles = skipFallbackDisplayOfZeroAngles;
            this.fillDensity = fillDensity;
            this.minAngleDeg_withoutTextLineBreak = minAngleDeg_withoutTextLineBreak;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceLineString_array_cam
    {
        public Camera targetCamera;
        public Vector2[] points;
        public Color color;
        public bool closeGapBetweenLastAndFirstPoint;
        public float width_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle style;
        public float stylePatternScaleFactor;
        public float durationInSec;
        public ScreenspaceLineString_array_cam(Camera targetCamera, Vector2[] points, Color color, bool closeGapBetweenLastAndFirstPoint, float width_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle style, float stylePatternScaleFactor, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.points = points;
            this.color = color;
            this.closeGapBetweenLastAndFirstPoint = closeGapBetweenLastAndFirstPoint;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.style = style;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceLineString_list_cam
    {
        public Camera targetCamera;
        public List<Vector2> points;
        public Color color;
        public bool closeGapBetweenLastAndFirstPoint;
        public float width_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle style;
        public float stylePatternScaleFactor;
        public float durationInSec;
        public ScreenspaceLineString_list_cam(Camera targetCamera, List<Vector2> points, Color color, bool closeGapBetweenLastAndFirstPoint, float width_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle style, float stylePatternScaleFactor, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.points = points;
            this.color = color;
            this.closeGapBetweenLastAndFirstPoint = closeGapBetweenLastAndFirstPoint;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.style = style;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceLineStringColorFade_array_cam
    {
        public Camera targetCamera;
        public Vector2[] points;
        public Color startColor;
        public Color endColor;
        public bool closeGapBetweenLastAndFirstPoint;
        public float width_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle style;
        public float stylePatternScaleFactor;
        public float durationInSec;
        public ScreenspaceLineStringColorFade_array_cam(Camera targetCamera, Vector2[] points, Color startColor, Color endColor, bool closeGapBetweenLastAndFirstPoint, float width_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle style, float stylePatternScaleFactor, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.points = points;
            this.startColor = startColor;
            this.endColor = endColor;
            this.closeGapBetweenLastAndFirstPoint = closeGapBetweenLastAndFirstPoint;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.style = style;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceLineStringColorFade_list_cam
    {
        public Camera targetCamera;
        public List<Vector2> points;
        public Color startColor;
        public Color endColor;
        public bool closeGapBetweenLastAndFirstPoint;
        public float width_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle style;
        public float stylePatternScaleFactor;
        public float durationInSec;
        public ScreenspaceLineStringColorFade_list_cam(Camera targetCamera, List<Vector2> points, Color startColor, Color endColor, bool closeGapBetweenLastAndFirstPoint, float width_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle style, float stylePatternScaleFactor, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.points = points;
            this.startColor = startColor;
            this.endColor = endColor;
            this.closeGapBetweenLastAndFirstPoint = closeGapBetweenLastAndFirstPoint;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.style = style;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceShape_3Dpos
    {
        public Vector3 centerPosition_in3DWorldspace;
        public DrawShapes.Shape2DType shape;
        public Color color;
        public float width_relToViewportHeight;
        public float height_relToViewportHeight;
        public float zRotationDegCC;
        public float linesWidth_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspaceShape_3Dpos(Vector3 centerPosition_in3DWorldspace, DrawShapes.Shape2DType shape, Color color, float width_relToViewportHeight, float height_relToViewportHeight, float zRotationDegCC, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.centerPosition_in3DWorldspace = centerPosition_in3DWorldspace;
            this.shape = shape;
            this.color = color;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.height_relToViewportHeight = height_relToViewportHeight;
            this.zRotationDegCC = zRotationDegCC;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceShape_3Dpos_cam
    {
        public Camera targetCamera;
        public Vector3 centerPosition_in3DWorldspace;
        public DrawShapes.Shape2DType shape;
        public Color color;
        public float width_relToViewportHeight;
        public float height_relToViewportHeight;
        public float zRotationDegCC;
        public float linesWidth_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspaceShape_3Dpos_cam(Camera targetCamera, Vector3 centerPosition_in3DWorldspace, DrawShapes.Shape2DType shape, Color color, float width_relToViewportHeight, float height_relToViewportHeight, float zRotationDegCC, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.centerPosition_in3DWorldspace = centerPosition_in3DWorldspace;
            this.shape = shape;
            this.color = color;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.height_relToViewportHeight = height_relToViewportHeight;
            this.zRotationDegCC = zRotationDegCC;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceShape_2Dpos_cam
    {
        public Camera targetCamera;
        public Vector2 centerPosition_in2DViewportSpace;
        public DrawShapes.Shape2DType shape;
        public Color color;
        public float width_relToViewportHeight;
        public float height_relToViewportHeight;
        public float zRotationDegCC;
        public float linesWidth_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspaceShape_2Dpos_cam(Camera targetCamera, Vector2 centerPosition_in2DViewportSpace, DrawShapes.Shape2DType shape, Color color, float width_relToViewportHeight, float height_relToViewportHeight, float zRotationDegCC, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.centerPosition_in2DViewportSpace = centerPosition_in2DViewportSpace;
            this.shape = shape;
            this.color = color;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.height_relToViewportHeight = height_relToViewportHeight;
            this.zRotationDegCC = zRotationDegCC;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceRectangle
    {
        public Camera targetCamera;
        public Vector2 lowLeftCorner;
        public float width_relToScreenWidth;
        public float height_relToScreenHeight;
        public Color color;
        public DrawShapes.Shape2DType shape;
        public float linesWidth_relToScreenHeight;
        public string text;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public float durationInSec;
        public ScreenspaceRectangle(Camera targetCamera, Vector2 lowLeftCorner, float width_relToScreenWidth, float height_relToScreenHeight, Color color, DrawShapes.Shape2DType shape, float linesWidth_relToScreenHeight, string text, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.lowLeftCorner = lowLeftCorner;
            this.width_relToScreenWidth = width_relToScreenWidth;
            this.height_relToScreenHeight = height_relToScreenHeight;
            this.color = color;
            this.shape = shape;
            this.linesWidth_relToScreenHeight = linesWidth_relToScreenHeight;
            this.text = text;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceBox_rect_cam
    {
        public Camera targetCamera;
        public Rect rect;
        public Color color;
        public float zRotationDegCC;
        public DrawShapes.Shape2DType shape;
        public float linesWidth_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspaceBox_rect_cam(Camera targetCamera, Rect rect, Color color, float zRotationDegCC, DrawShapes.Shape2DType shape, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.rect = rect;
            this.color = color;
            this.zRotationDegCC = zRotationDegCC;
            this.shape = shape;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceBox_3Dpos_vec
    {
        public Vector3 centerPosition_in3DWorldspace;
        public Vector2 size_relToViewportHeight;
        public Color color;
        public float zRotationDegCC;
        public DrawShapes.Shape2DType shape;
        public float linesWidth_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public bool forceSizeInterpretationToWarpedViewportSpace;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspaceBox_3Dpos_vec(Vector3 centerPosition_in3DWorldspace, Vector2 size_relToViewportHeight, Color color, float zRotationDegCC, DrawShapes.Shape2DType shape, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool forceSizeInterpretationToWarpedViewportSpace, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.centerPosition_in3DWorldspace = centerPosition_in3DWorldspace;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.color = color;
            this.zRotationDegCC = zRotationDegCC;
            this.shape = shape;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.forceSizeInterpretationToWarpedViewportSpace = forceSizeInterpretationToWarpedViewportSpace;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceBox_3Dpos_vec_cam
    {
        public Camera targetCamera;
        public Vector3 centerPosition_in3DWorldspace;
        public Vector2 size_relToViewportHeight;
        public Color color;
        public float zRotationDegCC;
        public DrawShapes.Shape2DType shape;
        public float linesWidth_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public bool forceSizeInterpretationToWarpedViewportSpace;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspaceBox_3Dpos_vec_cam(Camera targetCamera, Vector3 centerPosition_in3DWorldspace, Vector2 size_relToViewportHeight, Color color, float zRotationDegCC, DrawShapes.Shape2DType shape, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool forceSizeInterpretationToWarpedViewportSpace, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.centerPosition_in3DWorldspace = centerPosition_in3DWorldspace;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.color = color;
            this.zRotationDegCC = zRotationDegCC;
            this.shape = shape;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.forceSizeInterpretationToWarpedViewportSpace = forceSizeInterpretationToWarpedViewportSpace;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceBox_2Dpos_vec_cam
    {
        public Camera targetCamera;
        public Vector2 centerPosition_in2DViewportSpace;
        public Vector2 size_relToViewportHeight;
        public Color color;
        public float zRotationDegCC;
        public DrawShapes.Shape2DType shape;
        public float linesWidth_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public bool forceSizeInterpretationToWarpedViewportSpace;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspaceBox_2Dpos_vec_cam(Camera targetCamera, Vector2 centerPosition_in2DViewportSpace, Vector2 size_relToViewportHeight, Color color, float zRotationDegCC, DrawShapes.Shape2DType shape, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool forceSizeInterpretationToWarpedViewportSpace, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.centerPosition_in2DViewportSpace = centerPosition_in2DViewportSpace;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.color = color;
            this.zRotationDegCC = zRotationDegCC;
            this.shape = shape;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.forceSizeInterpretationToWarpedViewportSpace = forceSizeInterpretationToWarpedViewportSpace;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceCircle_rect_cam
    {
        public Camera targetCamera;
        public Rect rect;
        public Color color;
        public float linesWidth_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspaceCircle_rect_cam(Camera targetCamera, Rect rect, Color color, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.rect = rect;
            this.color = color;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceCircle_3Dpos_vecRad
    {
        public Vector3 centerPosition_in3DWorldspace;
        public float radius_relToViewportHeight;
        public Color color;
        public float linesWidth_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspaceCircle_3Dpos_vecRad(Vector3 centerPosition_in3DWorldspace, float radius_relToViewportHeight, Color color, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.centerPosition_in3DWorldspace = centerPosition_in3DWorldspace;
            this.radius_relToViewportHeight = radius_relToViewportHeight;
            this.color = color;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceCircle_3Dpos_vecRad_cam
    {
        public Camera targetCamera;
        public Vector3 centerPosition_in3DWorldspace;
        public float radius_relToViewportHeight;
        public Color color;
        public float linesWidth_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspaceCircle_3Dpos_vecRad_cam(Camera targetCamera, Vector3 centerPosition_in3DWorldspace, float radius_relToViewportHeight, Color color, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.centerPosition_in3DWorldspace = centerPosition_in3DWorldspace;
            this.radius_relToViewportHeight = radius_relToViewportHeight;
            this.color = color;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceCircle_2Dpos_vecRad_cam
    {
        public Camera targetCamera;
        public Vector2 centerPosition_in2DViewportSpace;
        public float radius_relToViewportHeight;
        public Color color;
        public float linesWidth_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspaceCircle_2Dpos_vecRad_cam(Camera targetCamera, Vector2 centerPosition_in2DViewportSpace, float radius_relToViewportHeight, Color color, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.centerPosition_in2DViewportSpace = centerPosition_in2DViewportSpace;
            this.radius_relToViewportHeight = radius_relToViewportHeight;
            this.color = color;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceCapsule_3Dpos_vecC1C2Pos
    {
        public Vector3 posOfCircle1_in3DWorldspace;
        public Vector3 posOfCircle2_in3DWorldspace;
        public float radius_relToViewportHeight;
        public Color color;
        public float linesWidth_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspaceCapsule_3Dpos_vecC1C2Pos(Vector3 posOfCircle1_in3DWorldspace, Vector3 posOfCircle2_in3DWorldspace, float radius_relToViewportHeight, Color color, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.posOfCircle1_in3DWorldspace = posOfCircle1_in3DWorldspace;
            this.posOfCircle2_in3DWorldspace = posOfCircle2_in3DWorldspace;
            this.radius_relToViewportHeight = radius_relToViewportHeight;
            this.color = color;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam
    {
        public Camera targetCamera;
        public Vector3 posOfCircle1_in3DWorldspace;
        public Vector3 posOfCircle2_in3DWorldspace;
        public float radius_relToViewportHeight;
        public Color color;
        public float linesWidth_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam(Camera targetCamera, Vector3 posOfCircle1_in3DWorldspace, Vector3 posOfCircle2_in3DWorldspace, float radius_relToViewportHeight, Color color, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.posOfCircle1_in3DWorldspace = posOfCircle1_in3DWorldspace;
            this.posOfCircle2_in3DWorldspace = posOfCircle2_in3DWorldspace;
            this.radius_relToViewportHeight = radius_relToViewportHeight;
            this.color = color;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam
    {
        public Camera targetCamera;
        public Vector2 posOfCircle1_in2DViewportSpace;
        public Vector2 posOfCircle2_in2DViewportSpace;
        public float radius_relToViewportHeight;
        public Color color;
        public float linesWidth_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam(Camera targetCamera, Vector2 posOfCircle1_in2DViewportSpace, Vector2 posOfCircle2_in2DViewportSpace, float radius_relToViewportHeight, Color color, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.posOfCircle1_in2DViewportSpace = posOfCircle1_in2DViewportSpace;
            this.posOfCircle2_in2DViewportSpace = posOfCircle2_in2DViewportSpace;
            this.radius_relToViewportHeight = radius_relToViewportHeight;
            this.color = color;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceCapsule_rect_cam
    {
        public Camera targetCamera;
        public Rect rect;
        public Color color;
        public CapsuleDirection2D capsuleDirection;
        public float zRotationDegCC;
        public float linesWidth_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspaceCapsule_rect_cam(Camera targetCamera, Rect rect, Color color, CapsuleDirection2D capsuleDirection, float zRotationDegCC, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.rect = rect;
            this.color = color;
            this.capsuleDirection = capsuleDirection;
            this.zRotationDegCC = zRotationDegCC;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceCapsule_3Dpos_vecPosSize
    {
        public Vector3 centerPosition_in3DWorldspace;
        public Vector2 size_relToViewportHeight;
        public Color color;
        public CapsuleDirection2D capsuleDirection;
        public float zRotationDegCC;
        public float linesWidth_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public bool forceSizeInterpretationToWarpedViewportSpace;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspaceCapsule_3Dpos_vecPosSize(Vector3 centerPosition_in3DWorldspace, Vector2 size_relToViewportHeight, Color color, CapsuleDirection2D capsuleDirection, float zRotationDegCC, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool forceSizeInterpretationToWarpedViewportSpace, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.centerPosition_in3DWorldspace = centerPosition_in3DWorldspace;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.color = color;
            this.capsuleDirection = capsuleDirection;
            this.zRotationDegCC = zRotationDegCC;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.forceSizeInterpretationToWarpedViewportSpace = forceSizeInterpretationToWarpedViewportSpace;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceCapsule_3Dpos_vecPosSize_cam
    {
        public Camera targetCamera;
        public Vector3 centerPosition_in3DWorldspace;
        public Vector2 size_relToViewportHeight;
        public Color color;
        public CapsuleDirection2D capsuleDirection;
        public float zRotationDegCC;
        public float linesWidth_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public bool forceSizeInterpretationToWarpedViewportSpace;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspaceCapsule_3Dpos_vecPosSize_cam(Camera targetCamera, Vector3 centerPosition_in3DWorldspace, Vector2 size_relToViewportHeight, Color color, CapsuleDirection2D capsuleDirection, float zRotationDegCC, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool forceSizeInterpretationToWarpedViewportSpace, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.centerPosition_in3DWorldspace = centerPosition_in3DWorldspace;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.color = color;
            this.capsuleDirection = capsuleDirection;
            this.zRotationDegCC = zRotationDegCC;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.forceSizeInterpretationToWarpedViewportSpace = forceSizeInterpretationToWarpedViewportSpace;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceCapsule_2Dpos_vecPosSize_cam
    {
        public Camera targetCamera;
        public Vector2 centerPosition_in2DViewportSpace;
        public Vector2 size_relToViewportHeight;
        public Color color;
        public CapsuleDirection2D capsuleDirection;
        public float zRotationDegCC;
        public float linesWidth_relToViewportHeight;
        public string text;
        public bool drawPointerIfOffscreen;
        public DrawBasics.LineStyle lineStyle;
        public float stylePatternScaleFactor;
        public DrawBasics.LineStyle fillStyle;
        public bool forceSizeInterpretationToWarpedViewportSpace;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspaceCapsule_2Dpos_vecPosSize_cam(Camera targetCamera, Vector2 centerPosition_in2DViewportSpace, Vector2 size_relToViewportHeight, Color color, CapsuleDirection2D capsuleDirection, float zRotationDegCC, float linesWidth_relToViewportHeight, string text, bool drawPointerIfOffscreen, DrawBasics.LineStyle lineStyle, float stylePatternScaleFactor, DrawBasics.LineStyle fillStyle, bool forceSizeInterpretationToWarpedViewportSpace, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.centerPosition_in2DViewportSpace = centerPosition_in2DViewportSpace;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.color = color;
            this.capsuleDirection = capsuleDirection;
            this.zRotationDegCC = zRotationDegCC;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.text = text;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.lineStyle = lineStyle;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.fillStyle = fillStyle;
            this.forceSizeInterpretationToWarpedViewportSpace = forceSizeInterpretationToWarpedViewportSpace;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspacePointArray
    {
        public Camera targetCamera;
        public Vector2[] points;
        public Color color;
        public float sizeOfMarkingCross_relToViewportHeight;
        public float markingCrossLinesWidth_relToViewportHeight;
        public bool drawCoordsAsText;
        public float durationInSec;
        public ScreenspacePointArray(Camera targetCamera, Vector2[] points, Color color, float sizeOfMarkingCross_relToViewportHeight, float markingCrossLinesWidth_relToViewportHeight, bool drawCoordsAsText, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.points = points;
            this.color = color;
            this.sizeOfMarkingCross_relToViewportHeight = sizeOfMarkingCross_relToViewportHeight;
            this.markingCrossLinesWidth_relToViewportHeight = markingCrossLinesWidth_relToViewportHeight;
            this.drawCoordsAsText = drawCoordsAsText;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspacePointList
    {
        public Camera targetCamera;
        public List<Vector2> points;
        public Color color;
        public float sizeOfMarkingCross_relToViewportHeight;
        public float markingCrossLinesWidth_relToViewportHeight;
        public bool drawCoordsAsText;
        public float durationInSec;
        public ScreenspacePointList(Camera targetCamera, List<Vector2> points, Color color, float sizeOfMarkingCross_relToViewportHeight, float markingCrossLinesWidth_relToViewportHeight, bool drawCoordsAsText, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.points = points;
            this.color = color;
            this.sizeOfMarkingCross_relToViewportHeight = sizeOfMarkingCross_relToViewportHeight;
            this.markingCrossLinesWidth_relToViewportHeight = markingCrossLinesWidth_relToViewportHeight;
            this.drawCoordsAsText = drawCoordsAsText;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspacePoint
    {
        public Vector2 position;
        public Color color;
        public float sizeOfMarkingCross_relToViewportHeight;
        public float zRotationDegCC;
        public float markingCrossLinesWidth_relToViewportHeight;
        public bool drawPointerIfOffscreen;
        public string text;
        public bool pointer_as_textAttachStyle;
        public bool drawCoordsAsText;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspacePoint(Vector2 position, Color color, float sizeOfMarkingCross_relToViewportHeight, float zRotationDegCC, float markingCrossLinesWidth_relToViewportHeight, bool drawPointerIfOffscreen, string text, bool pointer_as_textAttachStyle, bool drawCoordsAsText, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.position = position;
            this.color = color;
            this.sizeOfMarkingCross_relToViewportHeight = sizeOfMarkingCross_relToViewportHeight;
            this.zRotationDegCC = zRotationDegCC;
            this.markingCrossLinesWidth_relToViewportHeight = markingCrossLinesWidth_relToViewportHeight;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.text = text;
            this.pointer_as_textAttachStyle = pointer_as_textAttachStyle;
            this.drawCoordsAsText = drawCoordsAsText;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspacePoint_prioText_cam
    {
        public Camera targetCamera;
        public Vector2 position;
        public string text;
        public Color color;
        public float sizeOfMarkingCross_relToViewportHeight;
        public float markingCrossLinesWidth_relToViewportHeight;
        public float zRotationDegCC;
        public bool drawPointerIfOffscreen;
        public bool pointer_as_textAttachStyle;
        public bool drawCoordsAsText;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public ScreenspacePoint_prioText_cam(Camera targetCamera, Vector2 position, string text, Color color, float sizeOfMarkingCross_relToViewportHeight, float markingCrossLinesWidth_relToViewportHeight, float zRotationDegCC, bool drawPointerIfOffscreen, bool pointer_as_textAttachStyle, bool drawCoordsAsText, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.position = position;
            this.text = text;
            this.color = color;
            this.sizeOfMarkingCross_relToViewportHeight = sizeOfMarkingCross_relToViewportHeight;
            this.markingCrossLinesWidth_relToViewportHeight = markingCrossLinesWidth_relToViewportHeight;
            this.zRotationDegCC = zRotationDegCC;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.pointer_as_textAttachStyle = pointer_as_textAttachStyle;
            this.drawCoordsAsText = drawCoordsAsText;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspacePointTag_3Dpos
    {
        public Vector3 position_in3DWorldspace;
        public string text;
        public string titleText;
        public Color color;
        public bool drawPointerIfOffscreen;
        public float linesWidth_relToViewportHeight;
        public float size_asTextOffsetDistance_relToViewportHeight;
        public Vector2 textOffsetDirection;
        public float textSizeScaleFactor;
        public bool skipConeDrawing;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public Vector2 customTowardsPoint_ofDefaultTextOffsetDirection;
        public ScreenspacePointTag_3Dpos(Vector3 position_in3DWorldspace, string text, string titleText, Color color, bool drawPointerIfOffscreen, float linesWidth_relToViewportHeight, float size_asTextOffsetDistance_relToViewportHeight, Vector2 textOffsetDirection, float textSizeScaleFactor, bool skipConeDrawing, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec, Vector2 customTowardsPoint_ofDefaultTextOffsetDirection)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.position_in3DWorldspace = position_in3DWorldspace;
            this.text = text;
            this.titleText = titleText;
            this.color = color;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.size_asTextOffsetDistance_relToViewportHeight = size_asTextOffsetDistance_relToViewportHeight;
            this.textOffsetDirection = textOffsetDirection;
            this.textSizeScaleFactor = textSizeScaleFactor;
            this.skipConeDrawing = skipConeDrawing;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
            this.customTowardsPoint_ofDefaultTextOffsetDirection = customTowardsPoint_ofDefaultTextOffsetDirection;
        }
    }

    public struct ScreenspacePointTag_3Dpos_cam
    {
        public Camera targetCamera;
        public Vector3 position_in3DWorldspace;
        public string text;
        public string titleText;
        public Color color;
        public bool drawPointerIfOffscreen;
        public float linesWidth_relToViewportHeight;
        public float size_asTextOffsetDistance_relToViewportHeight;
        public Vector2 textOffsetDirection;
        public float textSizeScaleFactor;
        public bool skipConeDrawing;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public Vector2 customTowardsPoint_ofDefaultTextOffsetDirection;
        public ScreenspacePointTag_3Dpos_cam(Camera targetCamera, Vector3 position_in3DWorldspace, string text, string titleText, Color color, bool drawPointerIfOffscreen, float linesWidth_relToViewportHeight, float size_asTextOffsetDistance_relToViewportHeight, Vector2 textOffsetDirection, float textSizeScaleFactor, bool skipConeDrawing, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec, Vector2 customTowardsPoint_ofDefaultTextOffsetDirection)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.text = text;
            this.titleText = titleText;
            this.color = color;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.size_asTextOffsetDistance_relToViewportHeight = size_asTextOffsetDistance_relToViewportHeight;
            this.textOffsetDirection = textOffsetDirection;
            this.textSizeScaleFactor = textSizeScaleFactor;
            this.skipConeDrawing = skipConeDrawing;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
            this.customTowardsPoint_ofDefaultTextOffsetDirection = customTowardsPoint_ofDefaultTextOffsetDirection;
        }
    }

    public struct ScreenspacePointTag_2Dpos_cam
    {
        public Camera targetCamera;
        public Vector2 position_in2DViewportSpace;
        public string text;
        public string titleText;
        public Color color;
        public bool drawPointerIfOffscreen;
        public float linesWidth_relToViewportHeight;
        public float size_asTextOffsetDistance_relToViewportHeight;
        public Vector2 textOffsetDirection;
        public float textSizeScaleFactor;
        public bool skipConeDrawing;
        public bool addTextForOutsideDistance_toOffscreenPointer;
        public float durationInSec;
        public Vector2 customTowardsPoint_ofDefaultTextOffsetDirection;
        public ScreenspacePointTag_2Dpos_cam(Camera targetCamera, Vector2 position_in2DViewportSpace, string text, string titleText, Color color, bool drawPointerIfOffscreen, float linesWidth_relToViewportHeight, float size_asTextOffsetDistance_relToViewportHeight, Vector2 textOffsetDirection, float textSizeScaleFactor, bool skipConeDrawing, bool addTextForOutsideDistance_toOffscreenPointer, float durationInSec, Vector2 customTowardsPoint_ofDefaultTextOffsetDirection)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.text = text;
            this.titleText = titleText;
            this.color = color;
            this.drawPointerIfOffscreen = drawPointerIfOffscreen;
            this.linesWidth_relToViewportHeight = linesWidth_relToViewportHeight;
            this.size_asTextOffsetDistance_relToViewportHeight = size_asTextOffsetDistance_relToViewportHeight;
            this.textOffsetDirection = textOffsetDirection;
            this.textSizeScaleFactor = textSizeScaleFactor;
            this.skipConeDrawing = skipConeDrawing;
            this.addTextForOutsideDistance_toOffscreenPointer = addTextForOutsideDistance_toOffscreenPointer;
            this.durationInSec = durationInSec;
            this.customTowardsPoint_ofDefaultTextOffsetDirection = customTowardsPoint_ofDefaultTextOffsetDirection;
        }
    }

    public struct ScreenspaceVectorFrom
    {
        public Camera targetCamera;
        public Vector2 vectorStartPos;
        public Vector2 vector;
        public Color color;
        public float lineWidth_relToViewportHeight;
        public string text;
        public bool interpretVectorAsUnwarped;
        public float coneLength_relToViewportHeight;
        public bool pointerAtBothSides;
        public bool writeComponentValuesAsText;
        public float endPlatesSize_relToViewportHeight;
        public float durationInSec;
        public ScreenspaceVectorFrom(Camera targetCamera, Vector2 vectorStartPos, Vector2 vector, Color color, float lineWidth_relToViewportHeight, string text, bool interpretVectorAsUnwarped, float coneLength_relToViewportHeight, bool pointerAtBothSides, bool writeComponentValuesAsText, float endPlatesSize_relToViewportHeight, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.vectorStartPos = vectorStartPos;
            this.vector = vector;
            this.color = color;
            this.lineWidth_relToViewportHeight = lineWidth_relToViewportHeight;
            this.text = text;
            this.interpretVectorAsUnwarped = interpretVectorAsUnwarped;
            this.coneLength_relToViewportHeight = coneLength_relToViewportHeight;
            this.pointerAtBothSides = pointerAtBothSides;
            this.writeComponentValuesAsText = writeComponentValuesAsText;
            this.endPlatesSize_relToViewportHeight = endPlatesSize_relToViewportHeight;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceVectorTo
    {
        public Camera targetCamera;
        public Vector2 vector;
        public Vector2 vectorEndPos;
        public Color color;
        public float lineWidth_relToViewportHeight;
        public string text;
        public bool interpretVectorAsUnwarped;
        public float coneLength_relToViewportHeight;
        public bool pointerAtBothSides;
        public bool writeComponentValuesAsText;
        public float endPlatesSize_relToViewportHeight;
        public float durationInSec;
        public ScreenspaceVectorTo(Camera targetCamera, Vector2 vector, Vector2 vectorEndPos, Color color, float lineWidth_relToViewportHeight, string text, bool interpretVectorAsUnwarped, float coneLength_relToViewportHeight, bool pointerAtBothSides, bool writeComponentValuesAsText, float endPlatesSize_relToViewportHeight, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.vector = vector;
            this.vectorEndPos = vectorEndPos;
            this.color = color;
            this.lineWidth_relToViewportHeight = lineWidth_relToViewportHeight;
            this.text = text;
            this.interpretVectorAsUnwarped = interpretVectorAsUnwarped;
            this.coneLength_relToViewportHeight = coneLength_relToViewportHeight;
            this.pointerAtBothSides = pointerAtBothSides;
            this.writeComponentValuesAsText = writeComponentValuesAsText;
            this.endPlatesSize_relToViewportHeight = endPlatesSize_relToViewportHeight;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceVectorCircled_angleToAngle_cam
    {
        public Camera targetCamera;
        public Vector2 circleCenter;
        public float startAngleDegCC_relativeToUp;
        public float endAngleDegCC_relativeToUp;
        public float radius_relToViewportHeight;
        public Color color;
        public float lineWidth_relToViewportHeight;
        public string text;
        public float coneLength_relToViewportHeight;
        public bool skipFallbackDisplayOfZeroAngles;
        public bool pointerAtBothSides;
        public float minAngleDeg_withoutTextLineBreak;
        public DrawText.TextAnchorCircledDXXL textAnchor;
        public float durationInSec;
        public ScreenspaceVectorCircled_angleToAngle_cam(Camera targetCamera, Vector2 circleCenter, float startAngleDegCC_relativeToUp, float endAngleDegCC_relativeToUp, float radius_relToViewportHeight, Color color, float lineWidth_relToViewportHeight, string text, float coneLength_relToViewportHeight, bool skipFallbackDisplayOfZeroAngles, bool pointerAtBothSides, float minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL textAnchor, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.circleCenter = circleCenter;
            this.startAngleDegCC_relativeToUp = startAngleDegCC_relativeToUp;
            this.endAngleDegCC_relativeToUp = endAngleDegCC_relativeToUp;
            this.radius_relToViewportHeight = radius_relToViewportHeight;
            this.color = color;
            this.lineWidth_relToViewportHeight = lineWidth_relToViewportHeight;
            this.text = text;
            this.coneLength_relToViewportHeight = coneLength_relToViewportHeight;
            this.skipFallbackDisplayOfZeroAngles = skipFallbackDisplayOfZeroAngles;
            this.pointerAtBothSides = pointerAtBothSides;
            this.minAngleDeg_withoutTextLineBreak = minAngleDeg_withoutTextLineBreak;
            this.textAnchor = textAnchor;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceVectorCircled_angleFromStartPos_cam
    {
        public Camera targetCamera;
        public Vector2 startPos;
        public Vector2 circleCenter;
        public float turnAngleDegCC;
        public Color color;
        public float lineWidth_relToViewportHeight;
        public string text;
        public float coneLength_relToViewportHeight;
        public bool skipFallbackDisplayOfZeroAngles;
        public bool pointerAtBothSides;
        public float minAngleDeg_withoutTextLineBreak;
        public DrawText.TextAnchorCircledDXXL textAnchor;
        public float durationInSec;
        public ScreenspaceVectorCircled_angleFromStartPos_cam(Camera targetCamera, Vector2 startPos, Vector2 circleCenter, float turnAngleDegCC, Color color, float lineWidth_relToViewportHeight, string text, float coneLength_relToViewportHeight, bool skipFallbackDisplayOfZeroAngles, bool pointerAtBothSides, float minAngleDeg_withoutTextLineBreak, DrawText.TextAnchorCircledDXXL textAnchor, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.startPos = startPos;
            this.circleCenter = circleCenter;
            this.turnAngleDegCC = turnAngleDegCC;
            this.color = color;
            this.lineWidth_relToViewportHeight = lineWidth_relToViewportHeight;
            this.text = text;
            this.coneLength_relToViewportHeight = coneLength_relToViewportHeight;
            this.skipFallbackDisplayOfZeroAngles = skipFallbackDisplayOfZeroAngles;
            this.pointerAtBothSides = pointerAtBothSides;
            this.minAngleDeg_withoutTextLineBreak = minAngleDeg_withoutTextLineBreak;
            this.textAnchor = textAnchor;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceIcon_3Dpos
    {
        public Vector3 position_in3DWorldspace;
        public DrawBasics.IconType icon;
        public Color color;
        public float size_relToViewportHeight;
        public string text;
        public float zRotationDegCC;
        public float strokeWidth_relToViewportHeight;
        public bool displayPointerIfOffscreen;
        public bool mirrorHorizontally;
        public float durationInSec;
        public ScreenspaceIcon_3Dpos(Vector3 position_in3DWorldspace, DrawBasics.IconType icon, Color color, float size_relToViewportHeight, string text, float zRotationDegCC, float strokeWidth_relToViewportHeight, bool displayPointerIfOffscreen, bool mirrorHorizontally, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.position_in3DWorldspace = position_in3DWorldspace;
            this.icon = icon;
            this.color = color;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.text = text;
            this.zRotationDegCC = zRotationDegCC;
            this.strokeWidth_relToViewportHeight = strokeWidth_relToViewportHeight;
            this.displayPointerIfOffscreen = displayPointerIfOffscreen;
            this.mirrorHorizontally = mirrorHorizontally;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceIcon_3Dpos_cam
    {
        public Camera targetCamera;
        public Vector3 position_in3DWorldspace;
        public DrawBasics.IconType icon;
        public Color color;
        public float size_relToViewportHeight;
        public string text;
        public float zRotationDegCC;
        public float strokeWidth_relToViewportHeight;
        public bool displayPointerIfOffscreen;
        public bool mirrorHorizontally;
        public float durationInSec;
        public ScreenspaceIcon_3Dpos_cam(Camera targetCamera, Vector3 position_in3DWorldspace, DrawBasics.IconType icon, Color color, float size_relToViewportHeight, string text, float zRotationDegCC, float strokeWidth_relToViewportHeight, bool displayPointerIfOffscreen, bool mirrorHorizontally, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.icon = icon;
            this.color = color;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.text = text;
            this.zRotationDegCC = zRotationDegCC;
            this.strokeWidth_relToViewportHeight = strokeWidth_relToViewportHeight;
            this.displayPointerIfOffscreen = displayPointerIfOffscreen;
            this.mirrorHorizontally = mirrorHorizontally;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceIcon_2Dpos_cam
    {
        public Camera targetCamera;
        public Vector2 position_in2DViewportSpace;
        public DrawBasics.IconType icon;
        public Color color;
        public float size_relToViewportHeight;
        public string text;
        public float zRotationDegCC;
        public float strokeWidth_relToViewportHeight;
        public bool displayPointerIfOffscreen;
        public bool mirrorHorizontally;
        public float durationInSec;
        public ScreenspaceIcon_2Dpos_cam(Camera targetCamera, Vector2 position_in2DViewportSpace, DrawBasics.IconType icon, Color color, float size_relToViewportHeight, string text, float zRotationDegCC, float strokeWidth_relToViewportHeight, bool displayPointerIfOffscreen, bool mirrorHorizontally, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.icon = icon;
            this.color = color;
            this.size_relToViewportHeight = size_relToViewportHeight;
            this.text = text;
            this.zRotationDegCC = zRotationDegCC;
            this.strokeWidth_relToViewportHeight = strokeWidth_relToViewportHeight;
            this.displayPointerIfOffscreen = displayPointerIfOffscreen;
            this.mirrorHorizontally = mirrorHorizontally;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceDot_3Dpos
    {
        public Vector3 position_in3DWorldspace;
        public float radius_relToViewportHeight;
        public Color color;
        public string text;
        public float density;
        public bool displayPointerIfOffscreen;
        public float durationInSec;
        public ScreenspaceDot_3Dpos(Vector3 position_in3DWorldspace, float radius_relToViewportHeight, Color color, string text, float density, bool displayPointerIfOffscreen, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.position_in3DWorldspace = position_in3DWorldspace;
            this.radius_relToViewportHeight = radius_relToViewportHeight;
            this.color = color;
            this.text = text;
            this.density = density;
            this.displayPointerIfOffscreen = displayPointerIfOffscreen;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceDot_3Dpos_cam
    {
        public Camera targetCamera;
        public Vector3 position_in3DWorldspace;
        public float radius_relToViewportHeight;
        public Color color;
        public string text;
        public float density;
        public bool displayPointerIfOffscreen;
        public float durationInSec;
        public ScreenspaceDot_3Dpos_cam(Camera targetCamera, Vector3 position_in3DWorldspace, float radius_relToViewportHeight, Color color, string text, float density, bool displayPointerIfOffscreen, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.position_in3DWorldspace = position_in3DWorldspace;
            this.radius_relToViewportHeight = radius_relToViewportHeight;
            this.color = color;
            this.text = text;
            this.density = density;
            this.displayPointerIfOffscreen = displayPointerIfOffscreen;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceDot_2Dpos_cam
    {
        public Camera targetCamera;
        public Vector2 position_in2DViewportSpace;
        public float radius_relToViewportHeight;
        public Color color;
        public string text;
        public float density;
        public bool displayPointerIfOffscreen;
        public float durationInSec;
        public ScreenspaceDot_2Dpos_cam(Camera targetCamera, Vector2 position_in2DViewportSpace, float radius_relToViewportHeight, Color color, string text, float density, bool displayPointerIfOffscreen, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.position_in2DViewportSpace = position_in2DViewportSpace;
            this.radius_relToViewportHeight = radius_relToViewportHeight;
            this.color = color;
            this.text = text;
            this.density = density;
            this.displayPointerIfOffscreen = displayPointerIfOffscreen;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceMovingArrowsRay
    {
        public Camera targetCamera;
        public Vector2 start;
        public Vector2 direction;
        public Color color;
        public float lineWidth_relToViewportHeight;
        public float distanceBetweenArrows_relToViewportHeight;
        public float lengthOfArrows_relToViewportHeight;
        public string text;
        public float animationSpeed;
        public bool backwardAnimationFlipsArrowDirection;
        public bool interpretDirectionAsUnwarped;
        public float endPlatesSize_relToViewportHeight;
        public float durationInSec;
        public ScreenspaceMovingArrowsRay(Camera targetCamera, Vector2 start, Vector2 direction, Color color, float lineWidth_relToViewportHeight, float distanceBetweenArrows_relToViewportHeight, float lengthOfArrows_relToViewportHeight, string text, float animationSpeed, bool backwardAnimationFlipsArrowDirection, bool interpretDirectionAsUnwarped, float endPlatesSize_relToViewportHeight, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.start = start;
            this.direction = direction;
            this.color = color;
            this.lineWidth_relToViewportHeight = lineWidth_relToViewportHeight;
            this.distanceBetweenArrows_relToViewportHeight = distanceBetweenArrows_relToViewportHeight;
            this.lengthOfArrows_relToViewportHeight = lengthOfArrows_relToViewportHeight;
            this.text = text;
            this.animationSpeed = animationSpeed;
            this.backwardAnimationFlipsArrowDirection = backwardAnimationFlipsArrowDirection;
            this.interpretDirectionAsUnwarped = interpretDirectionAsUnwarped;
            this.endPlatesSize_relToViewportHeight = endPlatesSize_relToViewportHeight;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceMovingArrowsLine
    {
        public Camera targetCamera;
        public Vector2 start;
        public Vector2 end;
        public Color color;
        public float lineWidth_relToViewportHeight;
        public float distanceBetweenArrows_relToViewportHeight;
        public float lengthOfArrows_relToViewportHeight;
        public string text;
        public float animationSpeed;
        public bool backwardAnimationFlipsArrowDirection;
        public float endPlatesSize_relToViewportHeight;
        public float durationInSec;
        public ScreenspaceMovingArrowsLine(Camera targetCamera, Vector2 start, Vector2 end, Color color, float lineWidth_relToViewportHeight, float distanceBetweenArrows_relToViewportHeight, float lengthOfArrows_relToViewportHeight, string text, float animationSpeed, bool backwardAnimationFlipsArrowDirection, float endPlatesSize_relToViewportHeight, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.start = start;
            this.end = end;
            this.color = color;
            this.lineWidth_relToViewportHeight = lineWidth_relToViewportHeight;
            this.distanceBetweenArrows_relToViewportHeight = distanceBetweenArrows_relToViewportHeight;
            this.lengthOfArrows_relToViewportHeight = lengthOfArrows_relToViewportHeight;
            this.text = text;
            this.animationSpeed = animationSpeed;
            this.backwardAnimationFlipsArrowDirection = backwardAnimationFlipsArrowDirection;
            this.endPlatesSize_relToViewportHeight = endPlatesSize_relToViewportHeight;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceRayWithAlternatingColors
    {
        public Camera targetCamera;
        public Vector2 start;
        public Vector2 direction;
        public Color color1;
        public Color color2;
        public float lineWidth_relToViewportHeight;
        public float lengthOfStripes_relToViewportHeight;
        public string text;
        public bool interpretDirectionAsUnwarped;
        public float animationSpeed;
        public float endPlatesSize_relToViewportHeight;
        public float alphaFadeOutLength_0to1;
        public float durationInSec;
        public ScreenspaceRayWithAlternatingColors(Camera targetCamera, Vector2 start, Vector2 direction, Color color1, Color color2, float lineWidth_relToViewportHeight, float lengthOfStripes_relToViewportHeight, string text, bool interpretDirectionAsUnwarped, float animationSpeed, float endPlatesSize_relToViewportHeight, float alphaFadeOutLength_0to1, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.start = start;
            this.direction = direction;
            this.color1 = color1;
            this.color2 = color2;
            this.lineWidth_relToViewportHeight = lineWidth_relToViewportHeight;
            this.lengthOfStripes_relToViewportHeight = lengthOfStripes_relToViewportHeight;
            this.text = text;
            this.interpretDirectionAsUnwarped = interpretDirectionAsUnwarped;
            this.animationSpeed = animationSpeed;
            this.endPlatesSize_relToViewportHeight = endPlatesSize_relToViewportHeight;
            this.alphaFadeOutLength_0to1 = alphaFadeOutLength_0to1;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceLineWithAlternatingColors
    {
        public Camera targetCamera;
        public Vector2 start;
        public Vector2 end;
        public Color color1;
        public Color color2;
        public float lineWidth_relToViewportHeight;
        public float lengthOfStripes_relToViewportHeight;
        public string text;
        public float animationSpeed;
        public float endPlatesSize_relToViewportHeight;
        public float alphaFadeOutLength_0to1;
        public float durationInSec;
        public ScreenspaceLineWithAlternatingColors(Camera targetCamera, Vector2 start, Vector2 end, Color color1, Color color2, float lineWidth_relToViewportHeight, float lengthOfStripes_relToViewportHeight, string text, float animationSpeed, float endPlatesSize_relToViewportHeight, float alphaFadeOutLength_0to1, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.start = start;
            this.end = end;
            this.color1 = color1;
            this.color2 = color2;
            this.lineWidth_relToViewportHeight = lineWidth_relToViewportHeight;
            this.lengthOfStripes_relToViewportHeight = lengthOfStripes_relToViewportHeight;
            this.text = text;
            this.animationSpeed = animationSpeed;
            this.endPlatesSize_relToViewportHeight = endPlatesSize_relToViewportHeight;
            this.alphaFadeOutLength_0to1 = alphaFadeOutLength_0to1;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceBlinkingRay
    {
        public Camera targetCamera;
        public Vector2 start;
        public Vector2 direction;
        public Color primaryColor;
        public float blinkDurationInSec;
        public float width_relToViewportHeight;
        public string text;
        public bool interpretDirectionAsUnwarped;
        public DrawBasics.LineStyle style;
        public Color blinkColor;
        public float stylePatternScaleFactor;
        public float endPlatesSize_relToViewportHeight;
        public float alphaFadeOutLength_0to1;
        public float enlargeSmallTextToThisMinRelTextSize;
        public float durationInSec;
        public ScreenspaceBlinkingRay(Camera targetCamera, Vector2 start, Vector2 direction, Color primaryColor, float blinkDurationInSec, float width_relToViewportHeight, string text, bool interpretDirectionAsUnwarped, DrawBasics.LineStyle style, Color blinkColor, float stylePatternScaleFactor, float endPlatesSize_relToViewportHeight, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinRelTextSize, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.start = start;
            this.direction = direction;
            this.primaryColor = primaryColor;
            this.blinkDurationInSec = blinkDurationInSec;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.text = text;
            this.interpretDirectionAsUnwarped = interpretDirectionAsUnwarped;
            this.style = style;
            this.blinkColor = blinkColor;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.endPlatesSize_relToViewportHeight = endPlatesSize_relToViewportHeight;
            this.alphaFadeOutLength_0to1 = alphaFadeOutLength_0to1;
            this.enlargeSmallTextToThisMinRelTextSize = enlargeSmallTextToThisMinRelTextSize;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceBlinkingLine
    {
        public Camera targetCamera;
        public Vector2 start;
        public Vector2 end;
        public Color primaryColor;
        public float blinkDurationInSec;
        public float width_relToViewportHeight;
        public string text;
        public DrawBasics.LineStyle style;
        public Color blinkColor;
        public float stylePatternScaleFactor;
        public float endPlatesSize_relToViewportHeight;
        public float alphaFadeOutLength_0to1;
        public float enlargeSmallTextToThisMinRelTextSize;
        public float durationInSec;
        public ScreenspaceBlinkingLine(Camera targetCamera, Vector2 start, Vector2 end, Color primaryColor, float blinkDurationInSec, float width_relToViewportHeight, string text, DrawBasics.LineStyle style, Color blinkColor, float stylePatternScaleFactor, float endPlatesSize_relToViewportHeight, float alphaFadeOutLength_0to1, float enlargeSmallTextToThisMinRelTextSize, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.start = start;
            this.end = end;
            this.primaryColor = primaryColor;
            this.blinkDurationInSec = blinkDurationInSec;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.text = text;
            this.style = style;
            this.blinkColor = blinkColor;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.endPlatesSize_relToViewportHeight = endPlatesSize_relToViewportHeight;
            this.alphaFadeOutLength_0to1 = alphaFadeOutLength_0to1;
            this.enlargeSmallTextToThisMinRelTextSize = enlargeSmallTextToThisMinRelTextSize;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceRayUnderTension
    {
        public Camera targetCamera;
        public Vector2 start;
        public Vector2 direction;
        public float relaxedLength_relToViewportHeight;
        public Color relaxedColor;
        public DrawBasics.LineStyle style;
        public float stretchFactor_forStretchedTensionColor;
        public Color color_forStretchedTension;
        public float stretchFactor_forSqueezedTensionColor;
        public Color color_forSqueezedTension;
        public float width_relToViewportHeight;
        public string text;
        public float alphaOfReferenceLengthDisplay;
        public bool interpretDirectionAsUnwarped;
        public float stylePatternScaleFactor;
        public float endPlatesSize_relToViewportHeight;
        public float enlargeSmallTextToThisMinRelTextSize;
        public float durationInSec;
        public ScreenspaceRayUnderTension(Camera targetCamera, Vector2 start, Vector2 direction, float relaxedLength_relToViewportHeight, Color relaxedColor, DrawBasics.LineStyle style, float stretchFactor_forStretchedTensionColor, Color color_forStretchedTension, float stretchFactor_forSqueezedTensionColor, Color color_forSqueezedTension, float width_relToViewportHeight, string text, float alphaOfReferenceLengthDisplay, bool interpretDirectionAsUnwarped, float stylePatternScaleFactor, float endPlatesSize_relToViewportHeight, float enlargeSmallTextToThisMinRelTextSize, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.start = start;
            this.direction = direction;
            this.relaxedLength_relToViewportHeight = relaxedLength_relToViewportHeight;
            this.relaxedColor = relaxedColor;
            this.style = style;
            this.stretchFactor_forStretchedTensionColor = stretchFactor_forStretchedTensionColor;
            this.color_forStretchedTension = color_forStretchedTension;
            this.stretchFactor_forSqueezedTensionColor = stretchFactor_forSqueezedTensionColor;
            this.color_forSqueezedTension = color_forSqueezedTension;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.text = text;
            this.alphaOfReferenceLengthDisplay = alphaOfReferenceLengthDisplay;
            this.interpretDirectionAsUnwarped = interpretDirectionAsUnwarped;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.endPlatesSize_relToViewportHeight = endPlatesSize_relToViewportHeight;
            this.enlargeSmallTextToThisMinRelTextSize = enlargeSmallTextToThisMinRelTextSize;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceLineUnderTension
    {
        public Camera targetCamera;
        public Vector2 start;
        public Vector2 end;
        public float relaxedLength_relToViewportHeight;
        public Color relaxedColor;
        public DrawBasics.LineStyle style;
        public float stretchFactor_forStretchedTensionColor;
        public Color color_forStretchedTension;
        public float stretchFactor_forSqueezedTensionColor;
        public Color color_forSqueezedTension;
        public float width_relToViewportHeight;
        public string text;
        public float alphaOfReferenceLengthDisplay;
        public float stylePatternScaleFactor;
        public float endPlatesSize_relToViewportHeight;
        public float enlargeSmallTextToThisMinRelTextSize;
        public float durationInSec;
        public ScreenspaceLineUnderTension(Camera targetCamera, Vector2 start, Vector2 end, float relaxedLength_relToViewportHeight, Color relaxedColor, DrawBasics.LineStyle style, float stretchFactor_forStretchedTensionColor, Color color_forStretchedTension, float stretchFactor_forSqueezedTensionColor, Color color_forSqueezedTension, float width_relToViewportHeight, string text, float alphaOfReferenceLengthDisplay, float stylePatternScaleFactor, float endPlatesSize_relToViewportHeight, float enlargeSmallTextToThisMinRelTextSize, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.start = start;
            this.end = end;
            this.relaxedLength_relToViewportHeight = relaxedLength_relToViewportHeight;
            this.relaxedColor = relaxedColor;
            this.style = style;
            this.stretchFactor_forStretchedTensionColor = stretchFactor_forStretchedTensionColor;
            this.color_forStretchedTension = color_forStretchedTension;
            this.stretchFactor_forSqueezedTensionColor = stretchFactor_forSqueezedTensionColor;
            this.color_forSqueezedTension = color_forSqueezedTension;
            this.width_relToViewportHeight = width_relToViewportHeight;
            this.text = text;
            this.alphaOfReferenceLengthDisplay = alphaOfReferenceLengthDisplay;
            this.stylePatternScaleFactor = stylePatternScaleFactor;
            this.endPlatesSize_relToViewportHeight = endPlatesSize_relToViewportHeight;
            this.enlargeSmallTextToThisMinRelTextSize = enlargeSmallTextToThisMinRelTextSize;
            this.durationInSec = durationInSec;
        }
    }

    public struct ScreenspaceVisualizeAutomaticCameraForDrawing
    {
        public bool visualizeFrustum;
        public bool logPositionToConsole;
        public Color color;
        public float durationInSec;
        public ScreenspaceVisualizeAutomaticCameraForDrawing(bool visualizeFrustum, bool logPositionToConsole, Color color, float durationInSec)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.visualizeFrustum = visualizeFrustum;
            this.logPositionToConsole = logPositionToConsole;
            this.color = color;
            this.durationInSec = durationInSec;
        }
    }

    public struct DrawScreenspaceChart
    {
        public Camera targetCamera;
        public bool chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight;
        public float durationInSec;
        public ChartDrawing concernedChartDrawing; //-> additional member that deviates from the normal pattern inside this script file, because charts are instances, not static fields
        public DrawScreenspaceChart(Camera targetCamera, bool chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight, float durationInSec, ChartDrawing concernedChartDrawing)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight = chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight;
            this.durationInSec = durationInSec;
            this.concernedChartDrawing = concernedChartDrawing;
        }
    }

    public struct DrawScreenspacePieChart
    {
        public Camera targetCamera;
        public bool chartSize_isDefinedRelTo_cameraWidth_notCameraHeight;
        public float durationInSec;
        public PieChartDrawing concernedPieChartDrawing; //-> additional member that deviates from the normal pattern inside this script file, because charts are instances, not static fields

        public DrawScreenspacePieChart(Camera targetCamera, bool chartSize_isDefinedRelTo_cameraWidth_notCameraHeight, float durationInSec, PieChartDrawing concernedPieChartDrawing)
        {
            DrawXXL_LinesManager.instance.atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = true;

            this.targetCamera = targetCamera;
            this.chartSize_isDefinedRelTo_cameraWidth_notCameraHeight = chartSize_isDefinedRelTo_cameraWidth_notCameraHeight;
            this.durationInSec = durationInSec;
            this.concernedPieChartDrawing = concernedPieChartDrawing;
        }
    }

}
