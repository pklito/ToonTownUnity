namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/2D/Tag Drawer 2D")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class TagDrawer2D : VisualizerParent
    {
        [SerializeField] float linesWidth = 0.0f;
        [SerializeField] [Range(0.0f, 0.02f)] float linesWidth_relToScreen = 0.0f;
        [SerializeField] Color color = DrawBasics.defaultColor;
        [SerializeField] bool drawGlobalCoordinates = false;
        [SerializeField] [Range(0.0f, 1.0f)] float strokeWidth_forCoordinateTexts_onPointVisualiation_in0to1 = 0.0f;
        [SerializeField] float sizeOfMarkingCross = 1.0f;
        [SerializeField] [Range(0.005f, 1.0f)] float sizeOfMarkingCross_relToScreen = 0.1f;
        [SerializeField] bool skipConeDrawing = false;
        [SerializeField] bool forcePointerDirection = false;
        [SerializeField] float pointerTextSize_value = 0.1f;
        [SerializeField] [Range(0.001f, 0.3f)] float pointerTextSize_value_relToScreen = 0.01f;
        [SerializeField] public bool textOffsetDistance_isOutfolded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] float textOffsetDistance = 1.0f;
        [SerializeField] [Range(0.0025f, 0.2f)] float textOffsetDistance_relToScreen = 0.1f;
        [SerializeField] TagDrawer.PointerSizeInterpretation pointerSizeInterpretation = TagDrawer.PointerSizeInterpretation.absoluteUnits;
        [SerializeField] TagDrawer.AttachedTextsizeReferenceContext attachedTextsizeReferenceContext = TagDrawer.AttachedTextsizeReferenceContext.sceneViewWindowSize;
        float lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
        float biggestAbsGlobalSizeComponentOfTransform_ignoringZ = 1.0f;
        [SerializeField] bool cameraForSizeDefinitionIsAvailable = false;
        Camera gameviewCameraForDrawing;

        public override void InitializeValues_onceInComponentLifetime()
        {
            if (text_exclGlobalMarkupTags == null || text_exclGlobalMarkupTags == "")
            {
                text_exclGlobalMarkupTags = "tag text of " + this.gameObject.name;
                text_inclGlobalMarkupTags = "tag text of " + this.gameObject.name;
            }
            textSection_isOutfolded = true;

            source_ofCustomVector2_1 = CustomVector2Source.manualInput;
            customVector2_1_clipboardForManualInput = (-DrawBasics.Default_textOffsetDirection_forPointTags);
            vectorInterpretation_ofCustomVector2_1 = VectorInterpretation.globalSpace;
        }

        public override void DrawVisualizedObject()
        {
            CacheSizeScaleFactors();
            Vector2 drawPos2D_global = GetDrawPos2D_global();
            TryDrawCoordinates(drawPos2D_global);
            Vector2 used_textOffsetDir = forcePointerDirection ? (-Get_customVector2_1_inGlobalSpaceUnits()) : Vector2.zero;
            float used_linesWidth = ScaleInputFloat_accordingToSizeDefinition(linesWidth_relToScreen, linesWidth);
            float used_textOffsetDistance_unclamped = ScaleInputFloat_accordingToSizeDefinition(textOffsetDistance_relToScreen, textOffsetDistance);
            float used_textOffsetDistance = UtilitiesDXXL_DrawBasics.GetClamped_pointTagSize_asTextOffsetDistance(used_textOffsetDistance_unclamped, used_linesWidth);
            float used_relTextSizeScaling = Get_used_relTextSizeScaling(used_textOffsetDistance);
            DrawBasics2D.PointTag(drawPos2D_global, text_inclGlobalMarkupTags, color, used_linesWidth, used_textOffsetDistance, used_textOffsetDir, GetZPos_global_for2D(), used_relTextSizeScaling, skipConeDrawing, 0.0f, hiddenByNearerObjects);
        }

        void CacheSizeScaleFactors()
        {
            biggestAbsGlobalSizeComponentOfTransform_ignoringZ = UtilitiesDXXL_Math.GetBiggestAbsComponent_ignoringZ(transform.lossyScale);
            cameraForSizeDefinitionIsAvailable = false;
            switch (pointerSizeInterpretation)
            {
                case TagDrawer.PointerSizeInterpretation.absoluteUnits:
                    lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
                    break;
                case TagDrawer.PointerSizeInterpretation.relativeToGameobjectSize:
                    lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
                    break;
                case TagDrawer.PointerSizeInterpretation.relativeToTheSceneViewWindowSize:
#if UNITY_EDITOR
                    if (UnityEditor.SceneView.lastActiveSceneView != null)
                    {
                        cameraForSizeDefinitionIsAvailable = true;
                        float distance_ofDrawnObject_toCamera = (GetDrawPos3D_ofA2DModeTransform_global() - UnityEditor.SceneView.lastActiveSceneView.camera.transform.position).magnitude;
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
                case TagDrawer.PointerSizeInterpretation.relativeToTheGameViewWindowSize:
                    cameraForSizeDefinitionIsAvailable = UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out gameviewCameraForDrawing, "Tag Drawer 2D Component", false);
                    if (cameraForSizeDefinitionIsAvailable)
                    {
                        float distance_ofDrawnObject_toCamera = (GetDrawPos3D_ofA2DModeTransform_global() - gameviewCameraForDrawing.transform.position).magnitude;
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

        void TryDrawCoordinates(Vector2 drawPos2D_global)
        {
            if (drawGlobalCoordinates)
            {
                UtilitiesDXXL_DrawBasics.Set_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM_reversible(Convert_strokeWidthForCoordinateTexts_toPPM());
                DrawBasics2D.Point(drawPos2D_global, null, default(Color), ScaleInputFloat_accordingToSizeDefinition(sizeOfMarkingCross_relToScreen, sizeOfMarkingCross), 0.0f, GetZPos_global_for2D(), default(Color), 0.0f, false, true, 0.0f, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.Reverse_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM();
            }
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
                    case TagDrawer.AttachedTextsizeReferenceContext.extentOfTag:
                        return 1.0f;
                    case TagDrawer.AttachedTextsizeReferenceContext.globalSpace:
                        return Get_used_relTextSizeScaling_toReachAFixedWorldSpaceTextSize(pointerTextSize_value, used_textOffsetDistance);
                    case TagDrawer.AttachedTextsizeReferenceContext.sceneViewWindowSize:
                        //cannot reuse "cameraForSizeDefinitionIsAvailable" here, since "pointerAttachedTextsizeReferenceContext" is another setting than "pointerSizeInterpretation"
#if UNITY_EDITOR
                        if (UnityEditor.SceneView.lastActiveSceneView != null)
                        {
                            float distance_ofDrawnObject_toCamera = (GetDrawPos3D_ofA2DModeTransform_global() - UnityEditor.SceneView.lastActiveSceneView.camera.transform.position).magnitude;
                            float lengthOfScreenDiagonal_atDrawnObjectsPosition = UtilitiesDXXL_Screenspace.Get_diagonalExtentOfViewport_at_distanceFromCam(UnityEditor.SceneView.lastActiveSceneView.camera, distance_ofDrawnObject_toCamera);
                            float worldSpaceTextSize_toReachWantedScreenspaceTextSize = lengthOfScreenDiagonal_atDrawnObjectsPosition * pointerTextSize_value_relToScreen;
                            return Get_used_relTextSizeScaling_toReachAFixedWorldSpaceTextSize(worldSpaceTextSize_toReachWantedScreenspaceTextSize, used_textOffsetDistance);
                        }
                        else
                        {
                            return 1.0f;
                        }
#else
                        return 1.0f;
#endif
                    case TagDrawer.AttachedTextsizeReferenceContext.gameViewWindowSize:
                        //cannot reuse "cameraForSizeDefinitionIsAvailable" here, since "pointerAttachedTextsizeReferenceContext" is another setting than "pointerSizeInterpretation"
                        bool cameraForTextSizeDefinitionIsAvailable = UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out gameviewCameraForDrawing, "Tag Drawer Component", false);
                        if (cameraForTextSizeDefinitionIsAvailable)
                        {
                            float distance_ofDrawnObject_toCamera = (GetDrawPos3D_ofA2DModeTransform_global() - gameviewCameraForDrawing.transform.position).magnitude;
                            float lengthOfScreenDiagonal_atDrawnObjectsPosition = UtilitiesDXXL_Screenspace.Get_diagonalExtentOfViewport_at_distanceFromCam(gameviewCameraForDrawing, distance_ofDrawnObject_toCamera);
                            float worldSpaceTextSize_toReachWantedScreenspaceTextSize = lengthOfScreenDiagonal_atDrawnObjectsPosition * pointerTextSize_value_relToScreen;
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
            return ((pointerSizeInterpretation == TagDrawer.PointerSizeInterpretation.relativeToTheSceneViewWindowSize) || (pointerSizeInterpretation == TagDrawer.PointerSizeInterpretation.relativeToTheGameViewWindowSize));
        }

        float ScaleInputFloat_accordingToSizeDefinition(float inputFloatToScale_versionThatIsRelToScreen, float inputFloatToScale)
        {
            switch (pointerSizeInterpretation)
            {
                case TagDrawer.PointerSizeInterpretation.absoluteUnits:
                    return inputFloatToScale;
                case TagDrawer.PointerSizeInterpretation.relativeToGameobjectSize:
                    return biggestAbsGlobalSizeComponentOfTransform_ignoringZ * inputFloatToScale;
                case TagDrawer.PointerSizeInterpretation.relativeToTheSceneViewWindowSize:
                    if (cameraForSizeDefinitionIsAvailable)
                    {
                        return lengthOfScreenDiagonal_atDrawnObjectsPosition * inputFloatToScale_versionThatIsRelToScreen;
                    }
                    else
                    {
                        return inputFloatToScale;
                    }
                case TagDrawer.PointerSizeInterpretation.relativeToTheGameViewWindowSize:
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
