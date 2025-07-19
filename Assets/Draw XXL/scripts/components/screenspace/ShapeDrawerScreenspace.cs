namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Screenspace/Shape Drawer Screenspace")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class ShapeDrawerScreenspace : VisualizerScreenspaceParent
    {
        [SerializeField] ShapeDrawer2D.ShapeType shapeType = ShapeDrawer2D.ShapeType.circle;

        //size definitions:
        [SerializeField] [Range(0.0f, 1.5f)] float radius_relToViewportHeight = 0.05f;
        [SerializeField] [Range(0.0f, 1.5f)] float width_relToViewportHeight_initialValue01 = 0.1f;
        [SerializeField] [Range(0.0f, 1.5f)] float height_relToViewportHeight_initialValue01 = 0.1f;
        [SerializeField] [Range(0.0f, 1.5f)] float height_relToViewportHeight_initialValue02 = 0.2f;
        [SerializeField] [Range(0.0f, 1.5f)] float sizeOfIcon_relToViewportHeight = 0.1f;

        //other definitions:
        [SerializeField] [Range(-360.0f, 360.0f)] float zRotationDegCC = 0.0f;
        [SerializeField] Color color = DrawBasics.defaultColor;
        [SerializeField] [Range(0.0f, 0.2f)] float linesWidth_relToViewportHeight = 0.0f;
        [SerializeField] DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid;
        [SerializeField] float stylePatternScaleFactor = 1.0f;
        [SerializeField] DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible;
        [SerializeField] float shapeFillDensity = 1.0f;
        [SerializeField] bool drawPointerIfOffscreen = true;
        [SerializeField] bool addTextForOutsideDistance_toOffscreenPointer = true;
        [SerializeField] DrawBasics.IconType iconType = DrawBasics.IconType.car;
        [SerializeField] bool iconIsMirroredHorizontally = false;
        [SerializeField] CapsuleDirection2D capusleDirection2D = CapsuleDirection2D.Vertical;
        [SerializeField] ShapeDrawer.CornerOptionsForIrregularStar cornerOptionsForIrregularStar = ShapeDrawer.CornerOptionsForIrregularStar._5;
        [SerializeField] bool drawHullEdgeLines_forScreenEncasingShapes = false;
        [SerializeField] float dotDensity = 1.0f;

        public override void InitializeValues_onceInComponentLifetime()
        {
            TrySetTextToEmptyString();
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
                UtilitiesDXXL_LineStyles.logWarningToConsole_forTooSmallPatternScaleFactor = false;
                switch (shapeType)
                {
                    case ShapeDrawer2D.ShapeType.circle:
                        float size_ofCircleHull = 2.0f * radius_relToViewportHeight;

                        UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                        UtilitiesDXXL_Screenspace.DrawShape(usedCamera, positionInsideViewport0to1, DrawShapes.Shape2DType.circle, color, color, size_ofCircleHull, size_ofCircleHull, zRotationDegCC, linesWidth_relToViewportHeight, text_inclGlobalMarkupTags, lineStyle, stylePatternScaleFactor, fillStyle, drawPointerIfOffscreen, addTextForOutsideDistance_toOffscreenPointer, 0.0f, 1.0f, null, drawHullEdgeLines_forScreenEncasingShapes);
                        UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                        break;
                    case ShapeDrawer2D.ShapeType.ellipse:
                        UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                        UtilitiesDXXL_Screenspace.DrawShape(usedCamera, positionInsideViewport0to1, DrawShapes.Shape2DType.circle, color, color, width_relToViewportHeight_initialValue01, height_relToViewportHeight_initialValue02, zRotationDegCC, linesWidth_relToViewportHeight, text_inclGlobalMarkupTags, lineStyle, stylePatternScaleFactor, fillStyle, drawPointerIfOffscreen, addTextForOutsideDistance_toOffscreenPointer, 0.0f, 1.0f, null, drawHullEdgeLines_forScreenEncasingShapes);
                        UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                        break;
                    case ShapeDrawer2D.ShapeType.star:
                        UtilitiesDXXL_Screenspace.DrawShape(usedCamera, positionInsideViewport0to1, ShapeDrawer.Get_shape2DType_forIrregularStar(cornerOptionsForIrregularStar), color, color, width_relToViewportHeight_initialValue01, height_relToViewportHeight_initialValue01, zRotationDegCC, linesWidth_relToViewportHeight, text_inclGlobalMarkupTags, lineStyle, stylePatternScaleFactor, DrawBasics.LineStyle.invisible, drawPointerIfOffscreen, addTextForOutsideDistance_toOffscreenPointer, 0.0f, 1.0f, null, drawHullEdgeLines_forScreenEncasingShapes);
                        break;
                    case ShapeDrawer2D.ShapeType.capsule:
                        Vector2 sizeOfCapsule_relToViewportHeight = new Vector2(width_relToViewportHeight_initialValue01, height_relToViewportHeight_initialValue02);

                        UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                        UtilitiesDXXL_Screenspace.Capsule(usedCamera, positionInsideViewport0to1, sizeOfCapsule_relToViewportHeight, color, capusleDirection2D, zRotationDegCC, linesWidth_relToViewportHeight, text_inclGlobalMarkupTags, drawPointerIfOffscreen, lineStyle, stylePatternScaleFactor, fillStyle, addTextForOutsideDistance_toOffscreenPointer, 0.0f, drawHullEdgeLines_forScreenEncasingShapes);
                        UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                        break;
                    case ShapeDrawer2D.ShapeType.icon:
                        DrawScreenspace.Icon(usedCamera, positionInsideViewport0to1, iconType, color, sizeOfIcon_relToViewportHeight, text_inclGlobalMarkupTags, zRotationDegCC, linesWidth_relToViewportHeight, drawPointerIfOffscreen, iconIsMirroredHorizontally, 0.0f);
                        break;
                    case ShapeDrawer2D.ShapeType.triangle:
                        UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                        UtilitiesDXXL_Screenspace.DrawShape(usedCamera, positionInsideViewport0to1, DrawShapes.Shape2DType.triangle, color, color, width_relToViewportHeight_initialValue01, height_relToViewportHeight_initialValue01, zRotationDegCC, linesWidth_relToViewportHeight, text_inclGlobalMarkupTags, lineStyle, stylePatternScaleFactor, fillStyle, drawPointerIfOffscreen, addTextForOutsideDistance_toOffscreenPointer, 0.0f, 1.0f, null, drawHullEdgeLines_forScreenEncasingShapes);
                        UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                        break;
                    case ShapeDrawer2D.ShapeType.square:
                        UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                        UtilitiesDXXL_Screenspace.DrawShape(usedCamera, positionInsideViewport0to1, DrawShapes.Shape2DType.square, color, color, width_relToViewportHeight_initialValue01, height_relToViewportHeight_initialValue01, zRotationDegCC, linesWidth_relToViewportHeight, text_inclGlobalMarkupTags, lineStyle, stylePatternScaleFactor, fillStyle, drawPointerIfOffscreen, addTextForOutsideDistance_toOffscreenPointer, 0.0f, 1.0f, null, drawHullEdgeLines_forScreenEncasingShapes);
                        UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                        break;
                    case ShapeDrawer2D.ShapeType.pentagon:
                        UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                        UtilitiesDXXL_Screenspace.DrawShape(usedCamera, positionInsideViewport0to1, DrawShapes.Shape2DType.pentagon, color, color, width_relToViewportHeight_initialValue01, height_relToViewportHeight_initialValue01, zRotationDegCC, linesWidth_relToViewportHeight, text_inclGlobalMarkupTags, lineStyle, stylePatternScaleFactor, fillStyle, drawPointerIfOffscreen, addTextForOutsideDistance_toOffscreenPointer, 0.0f, 1.0f, null, drawHullEdgeLines_forScreenEncasingShapes);
                        UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                        break;
                    case ShapeDrawer2D.ShapeType.hexagon:
                        UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                        UtilitiesDXXL_Screenspace.DrawShape(usedCamera, positionInsideViewport0to1, DrawShapes.Shape2DType.hexagon, color, color, width_relToViewportHeight_initialValue01, height_relToViewportHeight_initialValue01, zRotationDegCC, linesWidth_relToViewportHeight, text_inclGlobalMarkupTags, lineStyle, stylePatternScaleFactor, fillStyle, drawPointerIfOffscreen, addTextForOutsideDistance_toOffscreenPointer, 0.0f, 1.0f, null, drawHullEdgeLines_forScreenEncasingShapes);
                        UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                        break;
                    case ShapeDrawer2D.ShapeType.septagon:
                        UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                        UtilitiesDXXL_Screenspace.DrawShape(usedCamera, positionInsideViewport0to1, DrawShapes.Shape2DType.septagon, color, color, width_relToViewportHeight_initialValue01, height_relToViewportHeight_initialValue01, zRotationDegCC, linesWidth_relToViewportHeight, text_inclGlobalMarkupTags, lineStyle, stylePatternScaleFactor, fillStyle, drawPointerIfOffscreen, addTextForOutsideDistance_toOffscreenPointer, 0.0f, 1.0f, null, drawHullEdgeLines_forScreenEncasingShapes);
                        UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                        break;
                    case ShapeDrawer2D.ShapeType.octagon:
                        UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                        UtilitiesDXXL_Screenspace.DrawShape(usedCamera, positionInsideViewport0to1, DrawShapes.Shape2DType.octagon, color, color, width_relToViewportHeight_initialValue01, height_relToViewportHeight_initialValue01, zRotationDegCC, linesWidth_relToViewportHeight, text_inclGlobalMarkupTags, lineStyle, stylePatternScaleFactor, fillStyle, drawPointerIfOffscreen, addTextForOutsideDistance_toOffscreenPointer, 0.0f, 1.0f, null, drawHullEdgeLines_forScreenEncasingShapes);
                        UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                        break;
                    case ShapeDrawer2D.ShapeType.decagon:
                        UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                        UtilitiesDXXL_Screenspace.DrawShape(usedCamera, positionInsideViewport0to1, DrawShapes.Shape2DType.decagon, color, color, width_relToViewportHeight_initialValue01, height_relToViewportHeight_initialValue01, zRotationDegCC, linesWidth_relToViewportHeight, text_inclGlobalMarkupTags, lineStyle, stylePatternScaleFactor, fillStyle, drawPointerIfOffscreen, addTextForOutsideDistance_toOffscreenPointer, 0.0f, 1.0f, null, drawHullEdgeLines_forScreenEncasingShapes);
                        UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                        break;
                    case ShapeDrawer2D.ShapeType.dot:
                        DrawScreenspace.Dot(usedCamera, positionInsideViewport0to1, radius_relToViewportHeight, color, text_inclGlobalMarkupTags, dotDensity, drawPointerIfOffscreen, 0.0f);
                        break;
                    default:
                        break;
                }
                UtilitiesDXXL_LineStyles.logWarningToConsole_forTooSmallPatternScaleFactor = true;
            }
        }

    }

}
