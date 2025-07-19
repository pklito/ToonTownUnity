namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/2D/Shape Drawer 2D")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class ShapeDrawer2D : VisualizerParent
    {
        public enum ShapeType { circle, ellipse, star, capsule, icon, triangle, square, pentagon, hexagon, septagon, octagon, decagon, dot }
        [SerializeField] ShapeType shapeType = ShapeType.circle;
        public enum ShapeSizeDefinition { relativeToGlobalScaleOfTheTransformUsingTheBiggestAbsoluteComponentButIgnoringZ, absoluteUnits, relativeToTheSceneViewWindowSize, relativeToTheGameViewWindowSize };
        [SerializeField] ShapeSizeDefinition sizeDefinition = ShapeSizeDefinition.relativeToGlobalScaleOfTheTransformUsingTheBiggestAbsoluteComponentButIgnoringZ;
        [SerializeField] ShapeDrawer.ShapeAttachedTextsizeReferenceContext shapeAttachedTextsizeReferenceContext = ShapeDrawer.ShapeAttachedTextsizeReferenceContext.sceneViewWindowSize;
        [SerializeField] float textSize_value = 0.1f;
        [SerializeField] [Range(0.001f, 0.2f)] float textSize_value_relToScreen = 0.02f;

        float lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
        float biggestAbsGlobalSizeComponentOfTransform_ignoringZ = 1.0f;
        [SerializeField] bool cameraForSizeDefinitionIsAvailable = false;
        Camera gameviewCameraForDrawing;

        //general settings:
        [SerializeField] [Range(-360.0f, 360.0f)] float rotation_angleDegCC = 0.0f;
        [SerializeField] Color color = DrawBasics.defaultColor;
        [SerializeField] DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid;
        [SerializeField] DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible;
        [SerializeField] float shapeFillDensity = 1.0f;
        [SerializeField] bool textBlockAboveLine = false;

        //shape specific settings:
        [SerializeField] DrawBasics.IconType iconType = DrawBasics.IconType.car;
        [SerializeField] bool iconIsMirroredHorizontally = false;
        [SerializeField] bool showAtlasOfAllAvailableIcons = false;
        [SerializeField] ShapeDrawer.CornerOptionsForIrregularStar cornerOptionsForIrregularStar = ShapeDrawer.CornerOptionsForIrregularStar._5;
        [SerializeField] CapsuleDirection2D capusleDirection2D = CapsuleDirection2D.Vertical;
        [SerializeField] float dotDensity = 1.0f;

        //general - scale type dependent:
        [SerializeField] float linesWidth = 0.0f;
        [SerializeField] [Range(0.0f, 0.02f)] float linesWidth_relToScreen = 0.0f;
        [SerializeField] float stylePatternScaleFactor = 1.0f;
        [SerializeField] float stylePatternScaleFactor_relToScreen = 0.1f;

        //shape specific - scale type dependent:
        [SerializeField] float radiusScaleFactor = 0.5f;
        [SerializeField] [Range(0.0f, 0.5f)] float radiusScaleFactor_relToScreen = 0.05f;
        [SerializeField] float sizeOfIconScaleFactor = 1.0f;
        [SerializeField] [Range(0.0f, 1.0f)] float sizeOfIconScaleFactor_relToScreen = 0.1f;
        [SerializeField] float width_scaleFactor_initialValue1 = 1.0f;
        [SerializeField] [Range(0.0f, 1.0f)] float width_scaleFactor_initialValue1_relToScreen = 0.1f;
        [SerializeField] float height_scaleFactor_initialValue1 = 1.0f;
        [SerializeField] [Range(0.0f, 1.0f)] float height_scaleFactor_initialValue1_relToScreen = 0.1f;
        [SerializeField] float height_scaleFactor_initialValue2 = 2.0f;
        [SerializeField] [Range(0.0f, 1.0f)] float height_scaleFactor_initialValue2_relToScreen = 0.2f;

        public override void InitializeValues_onceInComponentLifetime()
        {
            TrySetTextToEmptyString();
        }

        public override void DrawVisualizedObject()
        {
            CacheSizeScaleFactors();
            switch (shapeType)
            {
                case ShapeType.circle:
                    float size_asFloat = 2.0f * GetRadius();
                    Vector2 size = new Vector2(size_asFloat, size_asFloat);

                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    DrawBasics2D.Shape(GetDrawPos2D_global(), DrawShapes.Shape2DType.circle, size, color, rotation_angleDegCC, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, GetZPos_global_for2D(), Get_stylePatternScaleFactor(), fillStyle, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType.ellipse:
                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    DrawBasics2D.Shape(GetDrawPos2D_global(), DrawShapes.Shape2DType.circle, Get_size_initialValueNonUniform(), color, rotation_angleDegCC, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, GetZPos_global_for2D(), Get_stylePatternScaleFactor(), fillStyle, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType.star:
                    Set_globalTextSizeSpecs_reversible();
                    DrawBasics2D.Shape(GetDrawPos2D_global(), ShapeDrawer.Get_shape2DType_forIrregularStar(cornerOptionsForIrregularStar), Get_size_initialValueUniform(), color, rotation_angleDegCC, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, GetZPos_global_for2D(), Get_stylePatternScaleFactor(), DrawBasics.LineStyle.invisible, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType.capsule:
                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    DrawBasics2D.Capsule(GetDrawPos2D_global(), Get_size_initialValueNonUniform(), color, capusleDirection2D, rotation_angleDegCC, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, GetZPos_global_for2D(), Get_stylePatternScaleFactor(), fillStyle, false, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType.icon:
                    float sizeOfIcon = GetSizeOfIcon();
                    int strokeWidth_asPPMofSize = 0;
                    if ((UtilitiesDXXL_Math.ApproximatelyZero(sizeOfIcon) == false) && (UtilitiesDXXL_Math.ApproximatelyZero(linesWidth) == false))
                    {
                        strokeWidth_asPPMofSize = (int)(1000000.0f * (Get_linesWidth() / sizeOfIcon));
                    }
                    DrawBasics2D.Icon(GetDrawPos2D_global(), iconType, color, sizeOfIcon, text_inclGlobalMarkupTags, rotation_angleDegCC, strokeWidth_asPPMofSize, GetZPos_global_for2D(), iconIsMirroredHorizontally, 0.0f, hiddenByNearerObjects);

                    if (showAtlasOfAllAvailableIcons)
                    {
                        DrawBasics.DrawAtlasOfAllIconsWithTheirNames(GetDrawPos3D_ofA2DModeTransform_global(), default(Color), default(Color), true, biggestAbsGlobalSizeComponentOfTransform_ignoringZ);
                    }
                    break;
                case ShapeType.triangle:
                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    DrawBasics2D.Shape(GetDrawPos2D_global(), DrawShapes.Shape2DType.triangle, Get_size_initialValueUniform(), color, rotation_angleDegCC, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, GetZPos_global_for2D(), Get_stylePatternScaleFactor(), fillStyle, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType.square:
                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    DrawBasics2D.Shape(GetDrawPos2D_global(), DrawShapes.Shape2DType.square, Get_size_initialValueUniform(), color, rotation_angleDegCC, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, GetZPos_global_for2D(), Get_stylePatternScaleFactor(), fillStyle, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType.pentagon:
                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    DrawBasics2D.Shape(GetDrawPos2D_global(), DrawShapes.Shape2DType.pentagon, Get_size_initialValueUniform(), color, rotation_angleDegCC, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, GetZPos_global_for2D(), Get_stylePatternScaleFactor(), fillStyle, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType.hexagon:
                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    DrawBasics2D.Shape(GetDrawPos2D_global(), DrawShapes.Shape2DType.hexagon, Get_size_initialValueUniform(), color, rotation_angleDegCC, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, GetZPos_global_for2D(), Get_stylePatternScaleFactor(), fillStyle, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType.septagon:
                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    DrawBasics2D.Shape(GetDrawPos2D_global(), DrawShapes.Shape2DType.septagon, Get_size_initialValueUniform(), color, rotation_angleDegCC, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, GetZPos_global_for2D(), Get_stylePatternScaleFactor(), fillStyle, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType.octagon:
                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    DrawBasics2D.Shape(GetDrawPos2D_global(), DrawShapes.Shape2DType.octagon, Get_size_initialValueUniform(), color, rotation_angleDegCC, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, GetZPos_global_for2D(), Get_stylePatternScaleFactor(), fillStyle, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType.decagon:
                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    DrawBasics2D.Shape(GetDrawPos2D_global(), DrawShapes.Shape2DType.decagon, Get_size_initialValueUniform(), color, rotation_angleDegCC, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, GetZPos_global_for2D(), Get_stylePatternScaleFactor(), fillStyle, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType.dot:
                    DrawBasics2D.Dot(GetDrawPos2D_global(), 0.5f * GetSizeOfIcon(), color, text_inclGlobalMarkupTags, GetZPos_global_for2D(), dotDensity, 0.0f, hiddenByNearerObjects);
                    break;
                default:
                    break;
            }
        }

        void CacheSizeScaleFactors()
        {
            biggestAbsGlobalSizeComponentOfTransform_ignoringZ = UtilitiesDXXL_Math.GetBiggestAbsComponent_ignoringZ(transform.lossyScale);
            cameraForSizeDefinitionIsAvailable = false;
            switch (sizeDefinition)
            {
                case ShapeSizeDefinition.relativeToGlobalScaleOfTheTransformUsingTheBiggestAbsoluteComponentButIgnoringZ:
                    lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
                    break;
                case ShapeSizeDefinition.absoluteUnits:
                    lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
                    break;
                case ShapeSizeDefinition.relativeToTheSceneViewWindowSize:
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
                case ShapeSizeDefinition.relativeToTheGameViewWindowSize:
                    cameraForSizeDefinitionIsAvailable = UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out gameviewCameraForDrawing, "Shape Drawer 2D Component", false);
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

        float GetRadius()
        {
            return ScaleInputFloat_accordingToSizeDefinition(radiusScaleFactor_relToScreen, radiusScaleFactor);
        }

        float GetSizeOfIcon()
        {
            return ScaleInputFloat_accordingToSizeDefinition(sizeOfIconScaleFactor_relToScreen, sizeOfIconScaleFactor);
        }

        float Get_width_initialValue1()
        {
            return ScaleInputFloat_accordingToSizeDefinition(width_scaleFactor_initialValue1_relToScreen, width_scaleFactor_initialValue1);
        }

        float Get_height_initialValue1()
        {
            return ScaleInputFloat_accordingToSizeDefinition(height_scaleFactor_initialValue1_relToScreen, height_scaleFactor_initialValue1);
        }

        Vector2 Get_size_initialValueUniform()
        {
            return new Vector2(Get_width_initialValue1(), Get_height_initialValue1());
        }

        float Get_height_initialValue2()
        {
            return ScaleInputFloat_accordingToSizeDefinition(height_scaleFactor_initialValue2_relToScreen, height_scaleFactor_initialValue2);
        }

        Vector2 Get_size_initialValueNonUniform()
        {
            return new Vector2(Get_width_initialValue1(), Get_height_initialValue2());
        }

        float Get_linesWidth()
        {
            return ScaleInputFloat_accordingToSizeDefinition(linesWidth_relToScreen, linesWidth);
        }

        float Get_stylePatternScaleFactor()
        {
            float stylePatternScaleFactor_unclamped = ScaleInputFloat_accordingToSizeDefinition(stylePatternScaleFactor_relToScreen, stylePatternScaleFactor);
            return Mathf.Max(stylePatternScaleFactor_unclamped, UtilitiesDXXL_LineStyles.minStylePatternScaleFactor);
        }

        float ScaleInputFloat_accordingToSizeDefinition(float inputFloatToScale_versionThatIsRelToScreen, float inputFloatToScale)
        {
            switch (sizeDefinition)
            {
                case ShapeSizeDefinition.relativeToGlobalScaleOfTheTransformUsingTheBiggestAbsoluteComponentButIgnoringZ:
                    return biggestAbsGlobalSizeComponentOfTransform_ignoringZ * inputFloatToScale;
                case ShapeSizeDefinition.absoluteUnits:
                    return inputFloatToScale;
                case ShapeSizeDefinition.relativeToTheSceneViewWindowSize:
                    if (cameraForSizeDefinitionIsAvailable)
                    {
                        return lengthOfScreenDiagonal_atDrawnObjectsPosition * inputFloatToScale_versionThatIsRelToScreen;
                    }
                    else
                    {
                        return inputFloatToScale;
                    }
                case ShapeSizeDefinition.relativeToTheGameViewWindowSize:
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

        float forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_before;
        float forcedConstantWorldspaceTextSize_forTextAtShapes_before;
        DrawBasics.CameraForAutomaticOrientation cameraForAutomaticOrientation_before;
        DrawText.AutomaticTextOrientation automaticTextOrientation_before;
        void Set_globalTextSizeSpecs_reversible()
        {
            forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_before = DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes;
            forcedConstantWorldspaceTextSize_forTextAtShapes_before = DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes;
            cameraForAutomaticOrientation_before = DrawBasics.cameraForAutomaticOrientation;
            automaticTextOrientation_before = DrawText.automaticTextOrientation;

            if (sizeDefinition == ShapeSizeDefinition.relativeToTheSceneViewWindowSize)
            {
                DrawBasics.cameraForAutomaticOrientation = DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;
                DrawText.automaticTextOrientation = DrawText.AutomaticTextOrientation.screen;
            }
            else
            {
                if (sizeDefinition == ShapeSizeDefinition.relativeToTheGameViewWindowSize)
                {
                    DrawBasics.cameraForAutomaticOrientation = DrawBasics.CameraForAutomaticOrientation.gameViewCamera;
                    DrawText.automaticTextOrientation = DrawText.AutomaticTextOrientation.screen;
                }
                else
                {
                    switch (shapeAttachedTextsizeReferenceContext)
                    {
                        case ShapeDrawer.ShapeAttachedTextsizeReferenceContext.sizeOfShape:
                            DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = 0.0f;
                            DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = 0.0f;
                            break;
                        case ShapeDrawer.ShapeAttachedTextsizeReferenceContext.globalSpace:
                            DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = 0.0f;
                            DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = textSize_value;
                            break;
                        case ShapeDrawer.ShapeAttachedTextsizeReferenceContext.sceneViewWindowSize:
                            DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = textSize_value_relToScreen;
                            DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = 0.0f;
                            DrawBasics.cameraForAutomaticOrientation = DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;
                            DrawText.automaticTextOrientation = DrawText.AutomaticTextOrientation.screen;
                            break;
                        case ShapeDrawer.ShapeAttachedTextsizeReferenceContext.gameViewWindowSize:
                            DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = textSize_value_relToScreen;
                            DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = 0.0f;
                            DrawBasics.cameraForAutomaticOrientation = DrawBasics.CameraForAutomaticOrientation.gameViewCamera;
                            DrawText.automaticTextOrientation = DrawText.AutomaticTextOrientation.screen;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        void Reverse_globalTextSizeSpecs()
        {
            DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_before;
            DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = forcedConstantWorldspaceTextSize_forTextAtShapes_before;
            DrawBasics.cameraForAutomaticOrientation = cameraForAutomaticOrientation_before;
            DrawText.automaticTextOrientation = automaticTextOrientation_before;
        }

    }

}
