namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Grid Visualizer")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class GridVisualizer : VisualizerParent
    {
        public enum SpaceType { global, localDefinedByParent, localDefinedByThisGameobject };
        [SerializeField] SpaceType spaceType = SpaceType.global;

        public enum XGridType { linesAlongY, linesAlongZ, planes, invisible };
        public enum YGridType { linesAlongX, linesAlongZ, planes, invisible };
        public enum ZGridType { linesAlongX, linesAlongY, planes, invisible };

        [SerializeField] XGridType xGridType = XGridType.linesAlongY;
        [SerializeField] YGridType yGridType = YGridType.linesAlongX;
        [SerializeField] ZGridType zGridType = ZGridType.linesAlongX;

        [SerializeField] public bool magnitudeOrderSection_isOutfolded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] bool draw1000grid = false;
        [SerializeField] bool draw100grid = false;
        [SerializeField] bool draw10grid = false;
        [SerializeField] bool draw1grid = true;
        [SerializeField] bool draw0p1grid = false;
        [SerializeField] bool draw0p01grid = false;
        [SerializeField] bool draw0p001grid = false;

        public enum LineWidthMode { growAlongVisualizedAxis, growPerpendicularToVisualizedAxis };
        [SerializeField] LineWidthMode lineWidthMode = LineWidthMode.growAlongVisualizedAxis;
        public const float max_linesWidth_alongVisualizedAxis = 0.5f;
        [SerializeField] [Range(0.0f, max_linesWidth_alongVisualizedAxis)] float linesWidth_alongVisualizedAxis = 0.0f;
        [SerializeField] float linesWidth_perpendicularToVisualizedAxis = 0.0f;

        [SerializeField] float coveredGridUnits_rel = 10.0f;
        [SerializeField] [Range(0.1f, 10.0f)] float drawDensity = 1.0f;
        [SerializeField] float lengthOfEachGridLine_rel = 10.0f;
        [SerializeField] float extentOfEachGridPlane_rel = 10.0f;
        [SerializeField] Color colorForX = UtilitiesDXXL_Colors.red_xAxisAlpha1;
        [SerializeField] Color colorForY = UtilitiesDXXL_Colors.green_yAxisAlpha1;
        [SerializeField] Color colorForZ = UtilitiesDXXL_Colors.blue_zAxisAlpha1;
        [SerializeField] bool show_positionAroundWhichToDraw_forGrids = true;
        [SerializeField] bool show_distanceDisplay_forGrids = !UtilitiesDXXL_Grid.default_hide_distanceDisplay_forGrids;
        [SerializeField] float offsetForDistanceDisplays_inGrids = UtilitiesDXXL_Grid.default_offsetForDistanceDisplays_inGrids;
        [SerializeField] float offsetForCoordinateTextDisplays_inGrids = UtilitiesDXXL_Grid.default_offsetForCoordinateTextDisplays_inGrids;
        [SerializeField] float coveredGridUnits_rel_forGridPlanes = UtilitiesDXXL_Grid.default_coveredGridUnits_rel_forGridPlanes;
        [SerializeField] [Range(UtilitiesDXXL_Grid.min_sizeScalingForCoordinateTexts_inGrids, 1.0f)] float sizeScalingForCoordinateTexts_inGrids = UtilitiesDXXL_Grid.default_sizeScalingForCoordinateTexts_inGrids;

        bool skip_drawAroundPosVisualization_forXDim;
        bool skip_drawAroundPosVisualization_forYDim;
        bool skip_drawAroundPosVisualization_forZDim;

        public enum RepeatingCoordsTextVariant { repeatAfterDistance, displayOnlyOnce, noDisplay };
        [SerializeField] RepeatingCoordsTextVariant repeatingCoordsTextVariant = RepeatingCoordsTextVariant.repeatAfterDistance;
        [SerializeField] float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f;
        [SerializeField] bool skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes = DrawEngineBasics.skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes;
        [SerializeField] bool skipLocalPrefix_inCoordinateTextsOnGridAxes = DrawEngineBasics. skipLocalPrefix_inCoordinateTextsOnGridAxes;

        public override void DrawVisualizedObject()
        {
            float used_distanceBetweenRepeatingCoordsTexts_relToGridDistance = Get_used_distanceBetweenRepeatingCoordsTexts_relToGridDistance();
            float used_linesWidth = (lineWidthMode == LineWidthMode.growAlongVisualizedAxis) ? linesWidth_alongVisualizedAxis : (-linesWidth_perpendicularToVisualizedAxis);

            UtilitiesDXXL_Grid.forceSkip_drawAroundPosVisualizationLocal = false;
            UtilitiesDXXL_Grid.Set_hide_positionAroundWhichToDraw_forGrids_reversible(!show_positionAroundWhichToDraw_forGrids);
            UtilitiesDXXL_Grid.Set_hide_distanceDisplay_forGrids_reversible(!show_distanceDisplay_forGrids);
            UtilitiesDXXL_Grid.Set_offsetForDistanceDisplays_inGrids_reversible(offsetForDistanceDisplays_inGrids);
            UtilitiesDXXL_Grid.Set_offsetForCoordinateTextDisplays_inGrids_reversible(offsetForCoordinateTextDisplays_inGrids);
            UtilitiesDXXL_Grid.Set_coveredGridUnits_rel_forGridPlanes_reversible(coveredGridUnits_rel_forGridPlanes);
            UtilitiesDXXL_Grid.Set_sizeScalingForCoordinateTexts_inGrids_reversible(0.75f * sizeScalingForCoordinateTexts_inGrids);
            UtilitiesDXXL_Grid.Set_skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes_reversible(skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes);
            UtilitiesDXXL_Grid.Set_skipLocalPrefix_inCoordinateTextsOnGridAxes_reversible(skipLocalPrefix_inCoordinateTextsOnGridAxes);

            switch (spaceType)
            {
                case SpaceType.global:
                    switch (xGridType)
                    {
                        case XGridType.linesAlongY:
                            DrawEngineBasics.XGridLines(GetDrawPos3D_global(), coveredGridUnits_rel, lengthOfEachGridLine_rel, used_linesWidth, DrawEngineBasics.XGridLinesOrientation.alongY, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForX, 0.0f, hiddenByNearerObjects);
                            break;
                        case XGridType.linesAlongZ:
                            DrawEngineBasics.XGridLines(GetDrawPos3D_global(), coveredGridUnits_rel, lengthOfEachGridLine_rel, used_linesWidth, DrawEngineBasics.XGridLinesOrientation.alongZ, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForX, 0.0f, hiddenByNearerObjects);
                            break;
                        case XGridType.planes:
                            DrawEngineBasics.XGridPlanes(GetDrawPos3D_global(), extentOfEachGridPlane_rel, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForX, 0.0f, hiddenByNearerObjects);
                            break;
                        case XGridType.invisible:
                            break;
                        default:
                            break;
                    }

                    switch (yGridType)
                    {
                        case YGridType.linesAlongX:
                            DrawEngineBasics.YGridLines(GetDrawPos3D_global(), coveredGridUnits_rel, lengthOfEachGridLine_rel, used_linesWidth, DrawEngineBasics.YGridLinesOrientation.alongX, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForY, 0.0f, hiddenByNearerObjects);
                            break;
                        case YGridType.linesAlongZ:
                            DrawEngineBasics.YGridLines(GetDrawPos3D_global(), coveredGridUnits_rel, lengthOfEachGridLine_rel, used_linesWidth, DrawEngineBasics.YGridLinesOrientation.alongZ, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForY, 0.0f, hiddenByNearerObjects);
                            break;
                        case YGridType.planes:
                            DrawEngineBasics.YGridPlanes(GetDrawPos3D_global(), extentOfEachGridPlane_rel, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForY, 0.0f, hiddenByNearerObjects);
                            break;
                        case YGridType.invisible:
                            break;
                        default:
                            break;
                    }

                    switch (zGridType)
                    {
                        case ZGridType.linesAlongX:
                            DrawEngineBasics.ZGridLines(GetDrawPos3D_global(), coveredGridUnits_rel, lengthOfEachGridLine_rel, used_linesWidth, DrawEngineBasics.ZGridLinesOrientation.alongX, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForZ, 0.0f, hiddenByNearerObjects);
                            break;
                        case ZGridType.linesAlongY:
                            DrawEngineBasics.ZGridLines(GetDrawPos3D_global(), coveredGridUnits_rel, lengthOfEachGridLine_rel, used_linesWidth, DrawEngineBasics.ZGridLinesOrientation.alongY, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForZ, 0.0f, hiddenByNearerObjects);
                            break;
                        case ZGridType.planes:
                            DrawEngineBasics.ZGridPlanes(GetDrawPos3D_global(), extentOfEachGridPlane_rel, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForZ, 0.0f, hiddenByNearerObjects);
                            break;
                        case ZGridType.invisible:
                            break;
                        default:
                            break;
                    }
                    break;
                case SpaceType.localDefinedByParent:
                    SetConfigFor_forceSkip_drawAroundPosVisualzationLocal();

                    UtilitiesDXXL_Grid.forceSkip_drawAroundPosVisualizationLocal = skip_drawAroundPosVisualization_forXDim;
                    switch (xGridType)
                    {
                        case XGridType.linesAlongY:
                            DrawEngineBasics.XGridLinesLocal(transform.parent, GetDrawPos3D_inLocalSpaceAsDefinedByParent(), coveredGridUnits_rel, lengthOfEachGridLine_rel, used_linesWidth, DrawEngineBasics.XGridLinesOrientation.alongY, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForX, 0.0f, hiddenByNearerObjects);
                            break;
                        case XGridType.linesAlongZ:
                            DrawEngineBasics.XGridLinesLocal(transform.parent, GetDrawPos3D_inLocalSpaceAsDefinedByParent(), coveredGridUnits_rel, lengthOfEachGridLine_rel, used_linesWidth, DrawEngineBasics.XGridLinesOrientation.alongZ, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForX, 0.0f, hiddenByNearerObjects);
                            break;
                        case XGridType.planes:
                            DrawEngineBasics.XGridPlanesLocal(transform.parent, GetDrawPos3D_inLocalSpaceAsDefinedByParent(), extentOfEachGridPlane_rel, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForX, 0.0f, hiddenByNearerObjects);
                            break;
                        case XGridType.invisible:
                            break;
                        default:
                            break;
                    }

                    UtilitiesDXXL_Grid.forceSkip_drawAroundPosVisualizationLocal = skip_drawAroundPosVisualization_forYDim;
                    switch (yGridType)
                    {
                        case YGridType.linesAlongX:
                            DrawEngineBasics.YGridLinesLocal(transform.parent, GetDrawPos3D_inLocalSpaceAsDefinedByParent(), coveredGridUnits_rel, lengthOfEachGridLine_rel, used_linesWidth, DrawEngineBasics.YGridLinesOrientation.alongX, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForY, 0.0f, hiddenByNearerObjects);
                            break;
                        case YGridType.linesAlongZ:
                            DrawEngineBasics.YGridLinesLocal(transform.parent, GetDrawPos3D_inLocalSpaceAsDefinedByParent(), coveredGridUnits_rel, lengthOfEachGridLine_rel, used_linesWidth, DrawEngineBasics.YGridLinesOrientation.alongZ, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForY, 0.0f, hiddenByNearerObjects);
                            break;
                        case YGridType.planes:
                            DrawEngineBasics.YGridPlanesLocal(transform.parent, GetDrawPos3D_inLocalSpaceAsDefinedByParent(), extentOfEachGridPlane_rel, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForY, 0.0f, hiddenByNearerObjects);
                            break;
                        case YGridType.invisible:
                            break;
                        default:
                            break;
                    }

                    UtilitiesDXXL_Grid.forceSkip_drawAroundPosVisualizationLocal = skip_drawAroundPosVisualization_forZDim;
                    switch (zGridType)
                    {
                        case ZGridType.linesAlongX:
                            DrawEngineBasics.ZGridLinesLocal(transform.parent, GetDrawPos3D_inLocalSpaceAsDefinedByParent(), coveredGridUnits_rel, lengthOfEachGridLine_rel, used_linesWidth, DrawEngineBasics.ZGridLinesOrientation.alongX, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForZ, 0.0f, hiddenByNearerObjects);
                            break;
                        case ZGridType.linesAlongY:
                            DrawEngineBasics.ZGridLinesLocal(transform.parent, GetDrawPos3D_inLocalSpaceAsDefinedByParent(), coveredGridUnits_rel, lengthOfEachGridLine_rel, used_linesWidth, DrawEngineBasics.ZGridLinesOrientation.alongY, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForZ, 0.0f, hiddenByNearerObjects);
                            break;
                        case ZGridType.planes:
                            DrawEngineBasics.ZGridPlanesLocal(transform.parent, GetDrawPos3D_inLocalSpaceAsDefinedByParent(), extentOfEachGridPlane_rel, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForZ, 0.0f, hiddenByNearerObjects);
                            break;
                        case ZGridType.invisible:
                            break;
                        default:
                            break;
                    }
                    break;
                case SpaceType.localDefinedByThisGameobject:
                    SetConfigFor_forceSkip_drawAroundPosVisualzationLocal();

                    UtilitiesDXXL_Grid.forceSkip_drawAroundPosVisualizationLocal = skip_drawAroundPosVisualization_forXDim;
                    switch (xGridType)
                    {
                        case XGridType.linesAlongY:
                            DrawEngineBasics.XGridLinesLocal(transform, GetDrawPos3D_inLocalSpaceAsDefinedByThisGameobject(), coveredGridUnits_rel, lengthOfEachGridLine_rel, used_linesWidth, DrawEngineBasics.XGridLinesOrientation.alongY, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForX, 0.0f, hiddenByNearerObjects);
                            break;
                        case XGridType.linesAlongZ:
                            DrawEngineBasics.XGridLinesLocal(transform, GetDrawPos3D_inLocalSpaceAsDefinedByThisGameobject(), coveredGridUnits_rel, lengthOfEachGridLine_rel, used_linesWidth, DrawEngineBasics.XGridLinesOrientation.alongZ, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForX, 0.0f, hiddenByNearerObjects);
                            break;
                        case XGridType.planes:
                            DrawEngineBasics.XGridPlanesLocal(transform, GetDrawPos3D_inLocalSpaceAsDefinedByThisGameobject(), extentOfEachGridPlane_rel, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForX, 0.0f, hiddenByNearerObjects);
                            break;
                        case XGridType.invisible:
                            break;
                        default:
                            break;
                    }

                    UtilitiesDXXL_Grid.forceSkip_drawAroundPosVisualizationLocal = skip_drawAroundPosVisualization_forYDim;
                    switch (yGridType)
                    {
                        case YGridType.linesAlongX:
                            DrawEngineBasics.YGridLinesLocal(transform, GetDrawPos3D_inLocalSpaceAsDefinedByThisGameobject(), coveredGridUnits_rel, lengthOfEachGridLine_rel, used_linesWidth, DrawEngineBasics.YGridLinesOrientation.alongX, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForY, 0.0f, hiddenByNearerObjects);
                            break;
                        case YGridType.linesAlongZ:
                            DrawEngineBasics.YGridLinesLocal(transform, GetDrawPos3D_inLocalSpaceAsDefinedByThisGameobject(), coveredGridUnits_rel, lengthOfEachGridLine_rel, used_linesWidth, DrawEngineBasics.YGridLinesOrientation.alongZ, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForY, 0.0f, hiddenByNearerObjects);
                            break;
                        case YGridType.planes:
                            DrawEngineBasics.YGridPlanesLocal(transform, GetDrawPos3D_inLocalSpaceAsDefinedByThisGameobject(), extentOfEachGridPlane_rel, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForY, 0.0f, hiddenByNearerObjects);
                            break;
                        case YGridType.invisible:
                            break;
                        default:
                            break;
                    }

                    UtilitiesDXXL_Grid.forceSkip_drawAroundPosVisualizationLocal = skip_drawAroundPosVisualization_forZDim;
                    switch (zGridType)
                    {
                        case ZGridType.linesAlongX:
                            DrawEngineBasics.ZGridLinesLocal(transform, GetDrawPos3D_inLocalSpaceAsDefinedByThisGameobject(), coveredGridUnits_rel, lengthOfEachGridLine_rel, used_linesWidth, DrawEngineBasics.ZGridLinesOrientation.alongX, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForZ, 0.0f, hiddenByNearerObjects);
                            break;
                        case ZGridType.linesAlongY:
                            DrawEngineBasics.ZGridLinesLocal(transform, GetDrawPos3D_inLocalSpaceAsDefinedByThisGameobject(), coveredGridUnits_rel, lengthOfEachGridLine_rel, used_linesWidth, DrawEngineBasics.ZGridLinesOrientation.alongY, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForZ, 0.0f, hiddenByNearerObjects);
                            break;
                        case ZGridType.planes:
                            DrawEngineBasics.ZGridPlanesLocal(transform, GetDrawPos3D_inLocalSpaceAsDefinedByThisGameobject(), extentOfEachGridPlane_rel, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, used_distanceBetweenRepeatingCoordsTexts_relToGridDistance, colorForZ, 0.0f, hiddenByNearerObjects);
                            break;
                        case ZGridType.invisible:
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            UtilitiesDXXL_Grid.Reverse_hide_positionAroundWhichToDraw_forGrids();
            UtilitiesDXXL_Grid.Reverse_hide_distanceDisplay_forGrids();
            UtilitiesDXXL_Grid.Reverse_offsetForDistanceDisplays_inGrids();
            UtilitiesDXXL_Grid.Reverse_offsetForCoordinateTextDisplays_inGrids();
            UtilitiesDXXL_Grid.Reverse_coveredGridUnits_rel_forGridPlanes();
            UtilitiesDXXL_Grid.Reverse_sizeScalingForCoordinateTexts_inGrids();
            UtilitiesDXXL_Grid.Reverse_skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes();
            UtilitiesDXXL_Grid.Reverse_skipLocalPrefix_inCoordinateTextsOnGridAxes();
            UtilitiesDXXL_Grid.forceSkip_drawAroundPosVisualizationLocal = false;
        }

        float Get_used_distanceBetweenRepeatingCoordsTexts_relToGridDistance()
        {
            switch (repeatingCoordsTextVariant)
            {
                case RepeatingCoordsTextVariant.repeatAfterDistance:
                    return distanceBetweenRepeatingCoordsTexts_relToGridDistance;
                case RepeatingCoordsTextVariant.displayOnlyOnce:
                    return 0.0f;
                case RepeatingCoordsTextVariant.noDisplay:
                    return -1.0f;
                default:
                    return 0.0f;
            }
        }

        void SetConfigFor_forceSkip_drawAroundPosVisualzationLocal()
        {
            UtilitiesDXXL_Math.Dimension theSingleDimensionThatGetsADrawAroundPosVisualization = UtilitiesDXXL_Math.Dimension.x;

            if (xGridType != XGridType.invisible)
            {
                theSingleDimensionThatGetsADrawAroundPosVisualization = UtilitiesDXXL_Math.Dimension.x;
            }
            else
            {
                if (yGridType != YGridType.invisible)
                {
                    theSingleDimensionThatGetsADrawAroundPosVisualization = UtilitiesDXXL_Math.Dimension.y;
                }
                else
                {
                    if (zGridType != ZGridType.invisible)
                    {
                        theSingleDimensionThatGetsADrawAroundPosVisualization = UtilitiesDXXL_Math.Dimension.z;
                    }
                }
            }

            switch (theSingleDimensionThatGetsADrawAroundPosVisualization)
            {
                case UtilitiesDXXL_Math.Dimension.x:
                    skip_drawAroundPosVisualization_forXDim = false;
                    skip_drawAroundPosVisualization_forYDim = true;
                    skip_drawAroundPosVisualization_forZDim = true;
                    break;
                case UtilitiesDXXL_Math.Dimension.y:
                    skip_drawAroundPosVisualization_forXDim = true;
                    skip_drawAroundPosVisualization_forYDim = false;
                    skip_drawAroundPosVisualization_forZDim = true;
                    break;
                case UtilitiesDXXL_Math.Dimension.z:
                    skip_drawAroundPosVisualization_forXDim = true;
                    skip_drawAroundPosVisualization_forYDim = true;
                    skip_drawAroundPosVisualization_forZDim = false;
                    break;
                default:
                    break;
            }
        }

    }

}
