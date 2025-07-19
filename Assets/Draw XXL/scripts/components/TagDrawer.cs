namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Tag Drawer")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class TagDrawer : VisualizerParent
    {
        public enum TagStyle { pointer, boxed };
        [SerializeField] TagStyle tagStyle = TagStyle.pointer;

        public enum PointerSizeInterpretation { absoluteUnits, relativeToGameobjectSize, relativeToTheSceneViewWindowSize, relativeToTheGameViewWindowSize };
        [SerializeField] PointerSizeInterpretation pointerSizeInterpretation = PointerSizeInterpretation.absoluteUnits;
        float lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
        float biggestAbsGlobalSizeComponentOfTransform = 1.0f;
        [SerializeField] bool cameraForSizeDefinitionIsAvailable = false;
        Camera gameviewCameraForDrawing;

        public enum AttachedTextsizeReferenceContext { extentOfTag, globalSpace, sceneViewWindowSize, gameViewWindowSize };
        [SerializeField] AttachedTextsizeReferenceContext attachedTextsizeReferenceContext = AttachedTextsizeReferenceContext.sceneViewWindowSize;

        //both styles:
        [SerializeField] float linesWidth = 0.0f;
        [SerializeField] [Range(0.0f, 0.02f)] float linesWidth_relToScreen = 0.0f;
        [SerializeField] Color colorForText = DrawBasics.defaultColor;

        //pointer style:
        [SerializeField] public bool textOffsetDistance_isOutfolded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] float textOffsetDistance = 1.0f;
        [SerializeField] [Range(0.0025f, 0.3f)] float textOffsetDistance_relToScreen = 0.1f;
        [SerializeField] float textSize_value = 0.1f;
        [SerializeField] [Range(0.001f, 0.2f)] float textSize_value_relToScreen = 0.01f;
        [SerializeField] bool forcePointerDirection = false;
        [SerializeField] bool skipConeDrawing = false;

        //pointer style (coordinates):
        [SerializeField] bool drawGlobalCoordinates = false;
        [SerializeField] bool drawLocalCoordinates = false;
        [SerializeField] float sizeOfMarkingCross = 1.0f;
        [SerializeField] [Range(0.0f, 1.0f)] float strokeWidth_forCoordinateTexts_onPointVisualiation_in0to1 = 0.0f;
        [SerializeField] [Range(0.005f, 1.0f)] float sizeOfMarkingCross_relToScreen = 0.15f;

        //boxed style
        [SerializeField] bool encapsulateChildren = true;
        [SerializeField] bool textBlockAboveLine = false;
        [SerializeField] bool differentBoxColor = false;
        [SerializeField] Color differentBoxColor_value = UtilitiesDXXL_Colors.violet;

        public override void InitializeValues_onceInComponentLifetime()
        {
            if (text_exclGlobalMarkupTags == null || text_exclGlobalMarkupTags == "")
            {
                text_exclGlobalMarkupTags = "tag text of " + this.gameObject.name;
                text_inclGlobalMarkupTags = "tag text of " + this.gameObject.name;
            }
            textSection_isOutfolded = true;

            source_ofCustomVector3_1 = CustomVector3Source.manualInput;
            customVector3_1_clipboardForManualInput = (-DrawBasics.Default_textOffsetDirection_forPointTags);
            vectorInterpretation_ofCustomVector3_1 = VectorInterpretation.globalSpace;
        }

        public override void DrawVisualizedObject()
        {
            UtilitiesDXXL_Text.Set_automaticTextOrientation_reversible(DrawText.AutomaticTextOrientation.screen);
            UtilitiesDXXL_DrawBasics.Set_cameraForAutomaticOrientation_reversible(Get_cameraForAutomaticOrientation());
            try
            {
                DrawTag();
            }
            catch { }
            UtilitiesDXXL_Text.Reverse_automaticTextOrientation();
            UtilitiesDXXL_DrawBasics.Reverse_cameraForAutomaticOrientation();
        }

        void DrawTag()
        {
            switch (tagStyle)
            {
                case TagStyle.pointer:
                    CacheSizeScaleFactors();
                    Vector3 drawPos3D_global = GetDrawPos3D_global();
                    TryDrawCoordinates(drawPos3D_global);
                    Vector3 used_textOffsetDir = forcePointerDirection ? (-Get_customVector3_1_inGlobalSpaceUnits()) : Vector3.zero;
                    float used_linesWidth = ScaleInputFloat_accordingToSizeDefinition(linesWidth_relToScreen, linesWidth);
                    float used_textOffsetDistance_unclamped = ScaleInputFloat_accordingToSizeDefinition(textOffsetDistance_relToScreen, textOffsetDistance);
                    float used_textOffsetDistance = UtilitiesDXXL_DrawBasics.GetClamped_pointTagSize_asTextOffsetDistance(used_textOffsetDistance_unclamped, used_linesWidth);
                    float used_relTextSizeScaling = Get_used_relTextSizeScaling(used_textOffsetDistance);
                    DrawBasics.PointTag(drawPos3D_global, text_inclGlobalMarkupTags, colorForText, used_linesWidth, used_textOffsetDistance, used_textOffsetDir, used_relTextSizeScaling, skipConeDrawing, 0.0f, hiddenByNearerObjects);
                    break;
                case TagStyle.boxed:
                    Color used_colorForBox = differentBoxColor ? differentBoxColor_value : colorForText;
                    float used_textSize = Set_globalTextSizeSpecs_reversible();
                    DrawEngineBasics.TagGameObject(this.gameObject, text_inclGlobalMarkupTags, colorForText, used_colorForBox, used_textSize, linesWidth, encapsulateChildren, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    Reverse_globalTextSizeSpecs();
                    break;
                default:
                    break;
            }
        }

        void CacheSizeScaleFactors()
        {
            biggestAbsGlobalSizeComponentOfTransform = UtilitiesDXXL_Math.GetBiggestAbsComponent(transform.lossyScale);
            cameraForSizeDefinitionIsAvailable = false;
            switch (pointerSizeInterpretation)
            {
                case PointerSizeInterpretation.absoluteUnits:
                    lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
                    break;
                case PointerSizeInterpretation.relativeToGameobjectSize:
                    lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
                    break;
                case PointerSizeInterpretation.relativeToTheSceneViewWindowSize:
#if UNITY_EDITOR
                    if (UnityEditor.SceneView.lastActiveSceneView != null)
                    {
                        cameraForSizeDefinitionIsAvailable = true;
                        float distance_ofDrawnObject_toCamera = (GetDrawPos3D_global() - UnityEditor.SceneView.lastActiveSceneView.camera.transform.position).magnitude;
                        lengthOfScreenDiagonal_atDrawnObjectsPosition = UtilitiesDXXL_Screenspace.Get_diagonalExtentOfViewport_at_distanceFromCam(UnityEditor.SceneView.lastActiveSceneView.camera, distance_ofDrawnObject_toCamera);
                    }
                    else
                    {
                        lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
                    }
#else
                    lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
#endif
                    break;
                case PointerSizeInterpretation.relativeToTheGameViewWindowSize:
                    cameraForSizeDefinitionIsAvailable = UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out gameviewCameraForDrawing, "Tag Drawer Component", false);
                    if (cameraForSizeDefinitionIsAvailable)
                    {
                        float distance_ofDrawnObject_toCamera = (GetDrawPos3D_global() - gameviewCameraForDrawing.transform.position).magnitude;
                        lengthOfScreenDiagonal_atDrawnObjectsPosition = UtilitiesDXXL_Screenspace.Get_diagonalExtentOfViewport_at_distanceFromCam(gameviewCameraForDrawing, distance_ofDrawnObject_toCamera);
                    }
                    else
                    {
                        lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
                    }
                    break;
                default:
                    lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
                    break;
            }
        }

        void TryDrawCoordinates(Vector3 drawPos3D_global)
        {
            UtilitiesDXXL_DrawBasics.Set_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM_reversible(Convert_strokeWidthForCoordinateTexts_toPPM());
            if (this.transform.parent == null)
            {
                if (drawGlobalCoordinates || drawLocalCoordinates)
                {
                    DrawBasics.Point(drawPos3D_global, null, default(Color), ScaleInputFloat_accordingToSizeDefinition(sizeOfMarkingCross_relToScreen, sizeOfMarkingCross), 0.0f, default(Color), default(Quaternion), false, true, false, 0.0f, hiddenByNearerObjects);
                }
            }
            else
            {
                if (drawLocalCoordinates)
                {
                    //-> "PointLocal()" already includes a potentially drawn "Point(global)()"
                    DrawBasics.PointLocal(GetDrawPos3D_inLocalSpaceAsDefinedByParent(), this.transform.parent, null, default(Color), ScaleInputFloat_accordingToSizeDefinition(sizeOfMarkingCross_relToScreen, sizeOfMarkingCross), 0.0f, default(Color), default(Quaternion), false, true, drawGlobalCoordinates, false, false, 0.0f, hiddenByNearerObjects);
                }
                else
                {
                    if (drawGlobalCoordinates)
                    {
                        DrawBasics.Point(drawPos3D_global, null, default(Color), ScaleInputFloat_accordingToSizeDefinition(sizeOfMarkingCross_relToScreen, sizeOfMarkingCross), 0.0f, default(Color), default(Quaternion), false, true, false, 0.0f, hiddenByNearerObjects);
                    }
                }
            }
            UtilitiesDXXL_DrawBasics.Reverse_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM();
        }

        int Convert_strokeWidthForCoordinateTexts_toPPM()
        {
            if (strokeWidth_forCoordinateTexts_onPointVisualiation_in0to1 <= 0.0f)
            {
                return 0;
            }
            else
            {
                return Mathf.CeilToInt(strokeWidth_forCoordinateTexts_onPointVisualiation_in0to1 * UtilitiesDXXL_Text.maxRelStrokeWidth_inPPMofSize);
            }
        }

        float Get_used_relTextSizeScaling(float used_textOffsetDistance)
        {
            if (CheckIf_pointerSizeInterpretaion_isDependentOn_screenspace() == false)
            {
                switch (attachedTextsizeReferenceContext)
                {
                    case AttachedTextsizeReferenceContext.extentOfTag:
                        return 1.0f;
                    case AttachedTextsizeReferenceContext.globalSpace:
                        return Get_used_relTextSizeScaling_toReachAFixedWorldSpaceTextSize(textSize_value, used_textOffsetDistance);
                    case AttachedTextsizeReferenceContext.sceneViewWindowSize:
                        //cannot reuse "cameraForSizeDefinitionIsAvailable" here, since "pointerAttachedTextsizeReferenceContext" is another setting than "pointerSizeInterpretation"
#if UNITY_EDITOR
                        if (UnityEditor.SceneView.lastActiveSceneView != null)
                        {
                            float distance_ofDrawnObject_toCamera = (GetDrawPos3D_global() - UnityEditor.SceneView.lastActiveSceneView.camera.transform.position).magnitude;
                            float lengthOfScreenDiagonal_atDrawnObjectsPosition = UtilitiesDXXL_Screenspace.Get_diagonalExtentOfViewport_at_distanceFromCam(UnityEditor.SceneView.lastActiveSceneView.camera, distance_ofDrawnObject_toCamera);
                            float worldSpaceTextSize_toReachWantedScreenspaceTextSize = lengthOfScreenDiagonal_atDrawnObjectsPosition * textSize_value_relToScreen;
                            return Get_used_relTextSizeScaling_toReachAFixedWorldSpaceTextSize(worldSpaceTextSize_toReachWantedScreenspaceTextSize, used_textOffsetDistance);
                        }
                        else
                        {
                            return 1.0f;
                        }
#else
                        return 1.0f;
#endif
                    case AttachedTextsizeReferenceContext.gameViewWindowSize:
                        //cannot reuse "cameraForSizeDefinitionIsAvailable" here, since "pointerAttachedTextsizeReferenceContext" is another setting than "pointerSizeInterpretation"
                        bool cameraForTextSizeDefinitionIsAvailable = UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out gameviewCameraForDrawing, "Tag Drawer Component", false);
                        if (cameraForTextSizeDefinitionIsAvailable)
                        {
                            float distance_ofDrawnObject_toCamera = (GetDrawPos3D_global() - gameviewCameraForDrawing.transform.position).magnitude;
                            float lengthOfScreenDiagonal_atDrawnObjectsPosition = UtilitiesDXXL_Screenspace.Get_diagonalExtentOfViewport_at_distanceFromCam(gameviewCameraForDrawing, distance_ofDrawnObject_toCamera);
                            float worldSpaceTextSize_toReachWantedScreenspaceTextSize = lengthOfScreenDiagonal_atDrawnObjectsPosition * textSize_value_relToScreen;
                            return Get_used_relTextSizeScaling_toReachAFixedWorldSpaceTextSize(worldSpaceTextSize_toReachWantedScreenspaceTextSize, used_textOffsetDistance);
                        }
                        else
                        {
                            return 1.0f;
                        }
                    default:
                        return 1.0f;
                }
            }
            else
            {
                return 1.0f;
            }
        }

        float Get_used_relTextSizeScaling_toReachAFixedWorldSpaceTextSize(float fixedWorldSpaceTextSize_toReach, float used_textOffsetDistance)
        {
            //"used_textOffsetDistance" is guaranteed bigger than 0 here -> no "division by 0" check necessary
            return (fixedWorldSpaceTextSize_toReach / (used_textOffsetDistance * UtilitiesDXXL_DrawBasics.pointTagsTextSize_relToOffset));
        }

        bool CheckIf_pointerSizeInterpretaion_isDependentOn_screenspace()
        {
            return ((pointerSizeInterpretation == PointerSizeInterpretation.relativeToTheSceneViewWindowSize) || (pointerSizeInterpretation == PointerSizeInterpretation.relativeToTheGameViewWindowSize));
        }

        DrawBasics.CameraForAutomaticOrientation Get_cameraForAutomaticOrientation()
        {
            if (tagStyle == TagStyle.pointer && CheckIf_pointerSizeInterpretaion_isDependentOn_screenspace())
            {
                if (pointerSizeInterpretation == PointerSizeInterpretation.relativeToTheSceneViewWindowSize)
                {
                    return DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;
                }
                if (pointerSizeInterpretation == PointerSizeInterpretation.relativeToTheGameViewWindowSize)
                {
                    return DrawBasics.CameraForAutomaticOrientation.gameViewCamera;
                }
                UtilitiesDXXL_Log.PrintErrorCode("81-" + pointerSizeInterpretation);
                return DrawBasics.cameraForAutomaticOrientation;
            }
            else
            {
                switch (attachedTextsizeReferenceContext)
                {
                    case AttachedTextsizeReferenceContext.extentOfTag:
                        return DrawBasics.cameraForAutomaticOrientation;
                    case AttachedTextsizeReferenceContext.globalSpace:
                        return DrawBasics.cameraForAutomaticOrientation;
                    case AttachedTextsizeReferenceContext.sceneViewWindowSize:
                        return DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;
                    case AttachedTextsizeReferenceContext.gameViewWindowSize:
                        return DrawBasics.CameraForAutomaticOrientation.gameViewCamera;
                    default:
                        return DrawBasics.cameraForAutomaticOrientation;
                }
            }
        }

        float forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_before;
        float forcedConstantWorldspaceTextSize_forTextAtShapes_before;
        DrawBasics.CameraForAutomaticOrientation cameraForAutomaticOrientation_before;
        float Set_globalTextSizeSpecs_reversible()
        {
            forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_before = DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes;
            forcedConstantWorldspaceTextSize_forTextAtShapes_before = DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes;
            cameraForAutomaticOrientation_before = DrawBasics.cameraForAutomaticOrientation;

            switch (attachedTextsizeReferenceContext)
            {
                case AttachedTextsizeReferenceContext.extentOfTag:
                    DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = 0.0f;
                    DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = 0.0f;
                    return 0.0f;
                case AttachedTextsizeReferenceContext.globalSpace:
                    return textSize_value;
                case AttachedTextsizeReferenceContext.sceneViewWindowSize:
                    DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = textSize_value_relToScreen;
                    DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = 0.0f;
                    DrawBasics.cameraForAutomaticOrientation = DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;
                    return 0.0f;
                case AttachedTextsizeReferenceContext.gameViewWindowSize:
                    DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = textSize_value_relToScreen;
                    DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = 0.0f;
                    DrawBasics.cameraForAutomaticOrientation = DrawBasics.CameraForAutomaticOrientation.gameViewCamera;
                    return 0.0f;
                default:
                    return textSize_value;
            }
        }

        void Reverse_globalTextSizeSpecs()
        {
            DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_before;
            DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = forcedConstantWorldspaceTextSize_forTextAtShapes_before;
            DrawBasics.cameraForAutomaticOrientation = cameraForAutomaticOrientation_before;
        }

        float ScaleInputFloat_accordingToSizeDefinition(float inputFloatToScale_versionThatIsRelToScreen, float inputFloatToScale)
        {
            switch (pointerSizeInterpretation)
            {
                case PointerSizeInterpretation.absoluteUnits:
                    return inputFloatToScale;
                case PointerSizeInterpretation.relativeToGameobjectSize:
                    return biggestAbsGlobalSizeComponentOfTransform * inputFloatToScale;
                case PointerSizeInterpretation.relativeToTheSceneViewWindowSize:
                    if (cameraForSizeDefinitionIsAvailable)
                    {
                        return lengthOfScreenDiagonal_atDrawnObjectsPosition * inputFloatToScale_versionThatIsRelToScreen;
                    }
                    else
                    {
                        return inputFloatToScale;
                    }
                case PointerSizeInterpretation.relativeToTheGameViewWindowSize:
                    if (cameraForSizeDefinitionIsAvailable)
                    {
                        return lengthOfScreenDiagonal_atDrawnObjectsPosition * inputFloatToScale_versionThatIsRelToScreen;
                    }
                    else
                    {
                        return inputFloatToScale;
                    }
                default:
                    return inputFloatToScale;
            }
        }
    }

}
