namespace DrawXXL
{
    using UnityEngine;

    public class UtilitiesDXXL_Grid
    {
        public static bool forceSkip_drawingLocalOrigin = false;
        public static bool forceSkip_drawAroundPosVisualizationLocal = false;
        static float gridDensityDefaultScaleFactor = 0.148f;
        static Color colorFor_skewedPosIndicatingCubes = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(Color.black, 0.5f);
        static float alphaCascadingFactorForNextSmallerOrder = 0.8f;
        static float alphaCascadingFactorForNextSmallerOrder_forPointsBlackOverdraw = 0.7f;
        public static float min_distanceBetweenRepeatingCoordsTexts_relToGridDistance = 5.0f;
        public static bool default_hide_distanceDisplay_forGrids = false;
        public static float default_offsetForDistanceDisplays_inGrids = 0.65f;
        public static float default_offsetForCoordinateTextDisplays_inGrids = -1.0f;
        public static float default_coveredGridUnits_rel_forGridPlanes = 2.5f;
        public static float default_sizeScalingForCoordinateTexts_inGrids = 0.25f;
        public const float min_sizeScalingForCoordinateTexts_inGrids = 0.01f;
        static float rel_spaceBetweenLineAndCoordsText = 0.035f;

        public static void GridPlanes(bool drawXDim, bool drawYDim, bool drawZDim, Vector3 positionAroundWhichToDraw, float extentOfEachGridPlane_rel, float drawDensity, bool draw1000grid, bool draw100grid, bool draw10grid, bool draw1grid, bool draw0p1grid, bool draw0p01grid, bool draw0p001grid, float distanceBetweenRepeatingCoordsTexts_relToGridDistance, Color overwriteColorForX, Color overwriteColorForY, Color overwriteColorForZ, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(extentOfEachGridPlane_rel, "extentOfEachGridPlane_rel")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(drawDensity, "drawDensity")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(distanceBetweenRepeatingCoordsTexts_relToGridDistance, "distanceBetweenRepeatingCoordsTexts_relToGridDistance")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(positionAroundWhichToDraw, "positionAroundWhichToDraw")) { return; }

            float distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder = Get_distanceBetweenVisualizedGridPointsInVisualizedCoordSystemUnits_ofBiggestOrder(out int numberOfDrawnOrders, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid);

            Color colorForMainX = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColorForX, (numberOfDrawnOrders == 1) ? UtilitiesDXXL_Colors.red_xAxis : UtilitiesDXXL_Colors.red_xAxisAlpha1);
            Color colorForMainY = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColorForY, (numberOfDrawnOrders == 1) ? UtilitiesDXXL_Colors.green_yAxis : UtilitiesDXXL_Colors.green_yAxisAlpha1);
            Color colorForMainZ = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColorForZ, (numberOfDrawnOrders == 1) ? UtilitiesDXXL_Colors.blue_zAxis : UtilitiesDXXL_Colors.blue_zAxisAlpha1);

            if (numberOfDrawnOrders == 0)
            {
                Color fallbackColor = GetFallbackColor(drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ);
                UtilitiesDXXL_DrawBasics.PointFallback(positionAroundWhichToDraw, "[<color=#adadadFF><icon=logMessage></color> GridPlanes with 0 numberOfDrawnMagnitudeOrders]", fallbackColor, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            extentOfEachGridPlane_rel = UtilitiesDXXL_Math.AbsNonZeroValue(extentOfEachGridPlane_rel);
            extentOfEachGridPlane_rel = Mathf.Max(extentOfEachGridPlane_rel, 0.1f);

            float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits = 0.5f * extentOfEachGridPlane_rel;
            float sizeOfDrawnGridArea = DrawEngineBasics.coveredGridUnits_rel_forGridPlanes * distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder;
            float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits = 0.5f * (sizeOfDrawnGridArea / distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder);
            float alphaForUpcomingOrderColors = 1.0f;
            float alphaForUpcomingOrdersBlackPosMarker = 0.4f;

            //biggest order:
            DrawGridPlanesAndDrawAroundPosVisualization_forAOrderOfMagnitude(distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance);

            //smaller orders:
            if (draw1000grid)
            {
                //->is already biggest/cannot be a smaller order 
            }
            if (draw100grid)
            {
                if (distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder > 100.0f)
                {
                    DrawGridPlanesAndDrawAroundPosVisualization_forAOrderOfMagnitude(100.0f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
                }
            }
            if (draw10grid)
            {
                if (distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder > 10.0f)
                {
                    DrawGridPlanesAndDrawAroundPosVisualization_forAOrderOfMagnitude(10.0f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
                }
            }
            if (draw1grid)
            {
                if (distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder > 1.0f)
                {
                    DrawGridPlanesAndDrawAroundPosVisualization_forAOrderOfMagnitude(1.0f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
                }
            }
            if (draw0p1grid)
            {
                if (distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder > 0.1f)
                {
                    DrawGridPlanesAndDrawAroundPosVisualization_forAOrderOfMagnitude(0.1f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
                }
            }
            if (draw0p01grid)
            {
                if (distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder > 0.01f)
                {
                    DrawGridPlanesAndDrawAroundPosVisualization_forAOrderOfMagnitude(0.01f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
                }
            }
            if (draw0p001grid)
            {
                if (distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder > 0.001f)
                {
                    DrawGridPlanesAndDrawAroundPosVisualization_forAOrderOfMagnitude(0.001f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
                }
            }

        }

        public static void GridPlanesLocal(Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, bool drawXDim, bool drawYDim, bool drawZDim, Vector3 localPositionAroundWhichToDraw, float extentOfEachGridPlane_rel_inLocalSpaceUnits, float drawDensity, bool draw1000grid, bool draw100grid, bool draw10grid, bool draw1grid, bool draw0p1grid, bool draw0p01grid, bool draw0p001grid, float distanceBetweenRepeatingCoordsTexts_relToGridDistance, Color overwriteColorForX, Color overwriteColorForY, Color overwriteColorForZ, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(extentOfEachGridPlane_rel_inLocalSpaceUnits, "extentOfEachGridPlane_rel_inLocalSpaceUnits")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(drawDensity, "drawDensity")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(distanceBetweenRepeatingCoordsTexts_relToGridDistance, "distanceBetweenRepeatingCoordsTexts_relToGridDistance")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(originOfLocalSpace, "originOfLocalSpace")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(scaleOfLocalSpace, "scaleOfLocalSpace")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(localPositionAroundWhichToDraw, "localPositionAroundWhichToDraw")) { return; }

            rotationOfLocalSpace = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotationOfLocalSpace);

            float distanceBetweenVisualizedGridPointsInLocalSpaceUnits_ofBiggestOrder = Get_distanceBetweenVisualizedGridPointsInVisualizedCoordSystemUnits_ofBiggestOrder(out int numberOfDrawnOrders, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid);

            Color colorForMainX = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColorForX, (numberOfDrawnOrders == 1) ? UtilitiesDXXL_Colors.red_xAxis : UtilitiesDXXL_Colors.red_xAxisAlpha1);
            Color colorForMainY = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColorForY, (numberOfDrawnOrders == 1) ? UtilitiesDXXL_Colors.green_yAxis : UtilitiesDXXL_Colors.green_yAxisAlpha1);
            Color colorForMainZ = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColorForZ, (numberOfDrawnOrders == 1) ? UtilitiesDXXL_Colors.blue_zAxis : UtilitiesDXXL_Colors.blue_zAxisAlpha1);

            if (numberOfDrawnOrders == 0)
            {
                Color fallbackColor = GetFallbackColor(drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ);
                UtilitiesDXXL_DrawBasics.PointFallback(originOfLocalSpace, "[<color=#adadadFF><icon=logMessage></color> GridPlanesLocal with 0 numberOfDrawnMagnitudeOrders]", fallbackColor, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            if (drawXDim)
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(scaleOfLocalSpace.x))
                {
                    drawXDim = false;
                    UtilitiesDXXL_DrawBasics.PointFallback(originOfLocalSpace, "[<color=#adadadFF><icon=logMessage></color> GridPlanesLocal x-dimension scale of 0]", colorForMainX, 0.0f, durationInSec, hiddenByNearerObjects);
                }
            }
            if (drawYDim)
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(scaleOfLocalSpace.y))
                {
                    drawYDim = false;
                    UtilitiesDXXL_DrawBasics.PointFallback(originOfLocalSpace, "[<color=#adadadFF><icon=logMessage></color> GridPlanesLocal y-dimension scale of 0]", colorForMainY, 0.0f, durationInSec, hiddenByNearerObjects);
                }
            }
            if (drawZDim)
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(scaleOfLocalSpace.z))
                {
                    drawZDim = false;
                    UtilitiesDXXL_DrawBasics.PointFallback(originOfLocalSpace, "[<color=#adadadFF><icon=logMessage></color> GridPlanesLocal z-dimension scale of 0]", colorForMainZ, 0.0f, durationInSec, hiddenByNearerObjects);
                }
            }
            if ((drawXDim == false) && (drawYDim == false) && (drawZDim == false)) { return; }

            extentOfEachGridPlane_rel_inLocalSpaceUnits = UtilitiesDXXL_Math.AbsNonZeroValue(extentOfEachGridPlane_rel_inLocalSpaceUnits);
            extentOfEachGridPlane_rel_inLocalSpaceUnits = Mathf.Max(extentOfEachGridPlane_rel_inLocalSpaceUnits, 0.1f);

            float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits = 0.5f * extentOfEachGridPlane_rel_inLocalSpaceUnits;
            float sizeOfDrawnGridArea_inLocalSpaceUnits = DrawEngineBasics.coveredGridUnits_rel_forGridPlanes * distanceBetweenVisualizedGridPointsInLocalSpaceUnits_ofBiggestOrder;
            float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits = 0.5f * (sizeOfDrawnGridArea_inLocalSpaceUnits / distanceBetweenVisualizedGridPointsInLocalSpaceUnits_ofBiggestOrder);
            float alphaForUpcomingOrderColors = 1.0f;
            float alphaForUpcomingOrdersBlackPosMarker = 0.4f;

            Vector3 localForward_normalizedInGlobalSpace = rotationOfLocalSpace * Vector3.forward;
            Vector3 localUp_normalizedInGlobalSpace = rotationOfLocalSpace * Vector3.up;
            Vector3 localRight_normalizedInGlobalSpace = rotationOfLocalSpace * Vector3.right;
            Vector3 positionAroundWhichToDraw_global = originOfLocalSpace + localRight_normalizedInGlobalSpace * scaleOfLocalSpace.x * localPositionAroundWhichToDraw.x + localUp_normalizedInGlobalSpace * scaleOfLocalSpace.y * localPositionAroundWhichToDraw.y + localForward_normalizedInGlobalSpace * scaleOfLocalSpace.z * localPositionAroundWhichToDraw.z;

            //biggest order:
            DrawGridPlanesAndDrawAroundPosVisualization_forAOrderOfMagnitude_local(distanceBetweenVisualizedGridPointsInLocalSpaceUnits_ofBiggestOrder, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance, true);

            //smaller orders:
            if (draw1000grid)
            {
                //->is already biggest/cannot be a smaller order 
            }
            if (draw100grid)
            {
                if (distanceBetweenVisualizedGridPointsInLocalSpaceUnits_ofBiggestOrder > 100.0f)
                {
                    DrawGridPlanesAndDrawAroundPosVisualization_forAOrderOfMagnitude_local(100.0f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance, false);
                }
            }
            if (draw10grid)
            {
                if (distanceBetweenVisualizedGridPointsInLocalSpaceUnits_ofBiggestOrder > 10.0f)
                {
                    DrawGridPlanesAndDrawAroundPosVisualization_forAOrderOfMagnitude_local(10.0f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance, false);
                }
            }
            if (draw1grid)
            {
                if (distanceBetweenVisualizedGridPointsInLocalSpaceUnits_ofBiggestOrder > 1.0f)
                {
                    DrawGridPlanesAndDrawAroundPosVisualization_forAOrderOfMagnitude_local(1.0f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance, false);
                }
            }
            if (draw0p1grid)
            {
                if (distanceBetweenVisualizedGridPointsInLocalSpaceUnits_ofBiggestOrder > 0.1f)
                {
                    DrawGridPlanesAndDrawAroundPosVisualization_forAOrderOfMagnitude_local(0.1f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance, false);
                }
            }
            if (draw0p01grid)
            {
                if (distanceBetweenVisualizedGridPointsInLocalSpaceUnits_ofBiggestOrder > 0.01f)
                {
                    DrawGridPlanesAndDrawAroundPosVisualization_forAOrderOfMagnitude_local(0.01f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance, false);
                }
            }
            if (draw0p001grid)
            {
                if (distanceBetweenVisualizedGridPointsInLocalSpaceUnits_ofBiggestOrder > 0.001f)
                {
                    DrawGridPlanesAndDrawAroundPosVisualization_forAOrderOfMagnitude_local(0.001f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance, false);
                }
            }

        }

        public static void GridLines(Vector3 positionAroundWhichToDraw, float coveredGridUnits_rel, float lengthOfEachGridLine_rel, float linesWidth_signFlipsPerp, bool drawXDim, bool drawYDim, bool drawZDim, bool draw1000grid, bool draw100grid, bool draw10grid, bool draw1grid, bool draw0p1grid, bool draw0p01grid, bool draw0p001grid, float distanceBetweenRepeatingCoordsTexts_relToGridDistance, Color overwriteColorForX, Color overwriteColorForY, Color overwriteColorForZ, float durationInSec, bool hiddenByNearerObjects, DrawEngineBasics.XGridLinesOrientation orientation_ofXLines, DrawEngineBasics.YGridLinesOrientation orientation_ofYLines, DrawEngineBasics.ZGridLinesOrientation orientation_ofZLines)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(coveredGridUnits_rel, "coveredGridUnits_rel")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lengthOfEachGridLine_rel, "lengthOfEachGridLine_rel")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth_signFlipsPerp, "linesWidth_signFlipsPerp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(distanceBetweenRepeatingCoordsTexts_relToGridDistance, "distanceBetweenRepeatingCoordsTexts_relToGridDistance")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(positionAroundWhichToDraw, "positionAroundWhichToDraw")) { return; }

            float distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder = Get_distanceBetweenVisualizedGridPointsInVisualizedCoordSystemUnits_ofBiggestOrder(out int numberOfDrawnOrders, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid);
            Color colorForMainX = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColorForX, (numberOfDrawnOrders == 1) ? UtilitiesDXXL_Colors.red_xAxis : UtilitiesDXXL_Colors.red_xAxisAlpha1);
            Color colorForMainY = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColorForY, (numberOfDrawnOrders == 1) ? UtilitiesDXXL_Colors.green_yAxis : UtilitiesDXXL_Colors.green_yAxisAlpha1);
            Color colorForMainZ = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColorForZ, (numberOfDrawnOrders == 1) ? UtilitiesDXXL_Colors.blue_zAxis : UtilitiesDXXL_Colors.blue_zAxisAlpha1);

            if (numberOfDrawnOrders == 0)
            {
                Color fallbackColor = GetFallbackColor(drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ);
                UtilitiesDXXL_DrawBasics.PointFallback(positionAroundWhichToDraw, "[<color=#adadadFF><icon=logMessage></color> GridLines with 0 numberOfDrawnMagnitudeOrders]", fallbackColor, linesWidth_signFlipsPerp, durationInSec, hiddenByNearerObjects);
                return;
            }

            coveredGridUnits_rel = UtilitiesDXXL_Math.AbsNonZeroValue(coveredGridUnits_rel);
            coveredGridUnits_rel = Mathf.Max(coveredGridUnits_rel, 2.5f);

            lengthOfEachGridLine_rel = UtilitiesDXXL_Math.AbsNonZeroValue(lengthOfEachGridLine_rel);
            lengthOfEachGridLine_rel = Mathf.Max(lengthOfEachGridLine_rel, 0.1f);

            float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits = 0.5f * lengthOfEachGridLine_rel;
            float alphaForUpcomingOrderColors = 1.0f;
            float alphaForUpcomingOrdersBlackPosMarker = 0.4f;

            //biggest order:
            float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits = 0.5f * coveredGridUnits_rel;
            DrawGridLinesAndDrawAroundPosVisualization_forAOrderOfMagnitude(distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, distanceBetweenRepeatingCoordsTexts_relToGridDistance, linesWidth_signFlipsPerp, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);

            //smaller orders:
            //extentOfWholeCascadeAlongAxis_inOrdersOwnUnits = 6.5f;
            if (draw1000grid)
            {
                //->is already biggest/cannot be a smaller order 
            }
            if (draw100grid)
            {
                if (distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder > 100.0f)
                {
                    DrawGridLinesAndDrawAroundPosVisualization_forAOrderOfMagnitude(100.0f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, distanceBetweenRepeatingCoordsTexts_relToGridDistance, linesWidth_signFlipsPerp, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);
                }
            }
            if (draw10grid)
            {
                if (distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder > 10.0f)
                {
                    DrawGridLinesAndDrawAroundPosVisualization_forAOrderOfMagnitude(10.0f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, distanceBetweenRepeatingCoordsTexts_relToGridDistance, linesWidth_signFlipsPerp, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);
                }
            }
            if (draw1grid)
            {
                if (distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder > 1.0f)
                {
                    DrawGridLinesAndDrawAroundPosVisualization_forAOrderOfMagnitude(1.0f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, distanceBetweenRepeatingCoordsTexts_relToGridDistance, linesWidth_signFlipsPerp, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);
                }
            }
            if (draw0p1grid)
            {
                if (distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder > 0.1f)
                {
                    DrawGridLinesAndDrawAroundPosVisualization_forAOrderOfMagnitude(0.1f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, distanceBetweenRepeatingCoordsTexts_relToGridDistance, linesWidth_signFlipsPerp, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);
                }
            }
            if (draw0p01grid)
            {
                if (distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder > 0.01f)
                {
                    DrawGridLinesAndDrawAroundPosVisualization_forAOrderOfMagnitude(0.01f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, distanceBetweenRepeatingCoordsTexts_relToGridDistance, linesWidth_signFlipsPerp, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);
                }
            }
            if (draw0p001grid)
            {
                if (distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder > 0.001f)
                {
                    DrawGridLinesAndDrawAroundPosVisualization_forAOrderOfMagnitude(0.001f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, distanceBetweenRepeatingCoordsTexts_relToGridDistance, linesWidth_signFlipsPerp, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);
                }
            }

        }

        public static void GridLinesLocal(Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Vector3 localPositionAroundWhichToDraw, float coveredGridUnits_rel_inLocalSpaceUnits, float lengthOfDrawnLines_inLocalSpaceUnits, float linesWidth_inLocalSpaceUnits_signFlipsPerp, bool drawXDim, bool drawYDim, bool drawZDim, bool draw1000grid, bool draw100grid, bool draw10grid, bool draw1grid, bool draw0p1grid, bool draw0p01grid, bool draw0p001grid, float distanceBetweenRepeatingCoordsTexts_relToGridDistance, Color overwriteColorForX, Color overwriteColorForY, Color overwriteColorForZ, float durationInSec, bool hiddenByNearerObjects, DrawEngineBasics.XGridLinesOrientation orientation_ofXLines, DrawEngineBasics.YGridLinesOrientation orientation_ofYLines, DrawEngineBasics.ZGridLinesOrientation orientation_ofZLines)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(coveredGridUnits_rel_inLocalSpaceUnits, "coveredGridUnits_rel_inLocalSpaceUnits")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lengthOfDrawnLines_inLocalSpaceUnits, "lengthOfDrawnLines_inLocalSpaceUnits")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth_inLocalSpaceUnits_signFlipsPerp, "linesWidth_inLocalSpaceUnits_signFlipsPerp")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(distanceBetweenRepeatingCoordsTexts_relToGridDistance, "distanceBetweenRepeatingCoordsTexts_relToGridDistance")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(localPositionAroundWhichToDraw, "positionAroundWhichToDraw")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(scaleOfLocalSpace, "scaleOfLocalSpace")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(originOfLocalSpace, "originOfLocalSpace")) { return; }

            rotationOfLocalSpace = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotationOfLocalSpace);

            float distanceBetweenVisualizedGridPointsInLocalSpaceUnits_ofBiggestOrder = Get_distanceBetweenVisualizedGridPointsInVisualizedCoordSystemUnits_ofBiggestOrder(out int numberOfDrawnOrders, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid);
            Color colorForMainX = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColorForX, (numberOfDrawnOrders == 1) ? UtilitiesDXXL_Colors.red_xAxis : UtilitiesDXXL_Colors.red_xAxisAlpha1);
            Color colorForMainY = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColorForY, (numberOfDrawnOrders == 1) ? UtilitiesDXXL_Colors.green_yAxis : UtilitiesDXXL_Colors.green_yAxisAlpha1);
            Color colorForMainZ = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColorForZ, (numberOfDrawnOrders == 1) ? UtilitiesDXXL_Colors.blue_zAxis : UtilitiesDXXL_Colors.blue_zAxisAlpha1);

            if (numberOfDrawnOrders == 0)
            {
                Color fallbackColor = GetFallbackColor(drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ);
                UtilitiesDXXL_DrawBasics.PointFallback(originOfLocalSpace, "[<color=#adadadFF><icon=logMessage></color> GridLinesLocal with 0 numberOfDrawnMagnitudeOrders]", fallbackColor, linesWidth_inLocalSpaceUnits_signFlipsPerp, durationInSec, hiddenByNearerObjects);
                return;
            }

            if (drawXDim)
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(scaleOfLocalSpace.x))
                {
                    drawXDim = false;
                    UtilitiesDXXL_DrawBasics.PointFallback(originOfLocalSpace, "[<color=#adadadFF><icon=logMessage></color> GridLinesLocal x-dimension scale of 0]", colorForMainX, linesWidth_inLocalSpaceUnits_signFlipsPerp, durationInSec, hiddenByNearerObjects);
                }
            }
            if (drawYDim)
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(scaleOfLocalSpace.y))
                {
                    drawYDim = false;
                    UtilitiesDXXL_DrawBasics.PointFallback(originOfLocalSpace, "[<color=#adadadFF><icon=logMessage></color> GridLinesLocal y-dimension scale of 0]", colorForMainY, linesWidth_inLocalSpaceUnits_signFlipsPerp, durationInSec, hiddenByNearerObjects);
                }
            }
            if (drawZDim)
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(scaleOfLocalSpace.z))
                {
                    drawZDim = false;
                    UtilitiesDXXL_DrawBasics.PointFallback(originOfLocalSpace, "[<color=#adadadFF><icon=logMessage></color> GridLinesLocal z-dimension scale of 0]", colorForMainZ, linesWidth_inLocalSpaceUnits_signFlipsPerp, durationInSec, hiddenByNearerObjects);
                }
            }
            if ((drawXDim == false) && (drawYDim == false) && (drawZDim == false)) { return; }

            coveredGridUnits_rel_inLocalSpaceUnits = UtilitiesDXXL_Math.AbsNonZeroValue(coveredGridUnits_rel_inLocalSpaceUnits);
            coveredGridUnits_rel_inLocalSpaceUnits = Mathf.Max(coveredGridUnits_rel_inLocalSpaceUnits, 2.5f);

            lengthOfDrawnLines_inLocalSpaceUnits = UtilitiesDXXL_Math.AbsNonZeroValue(lengthOfDrawnLines_inLocalSpaceUnits);
            lengthOfDrawnLines_inLocalSpaceUnits = Mathf.Max(lengthOfDrawnLines_inLocalSpaceUnits, 0.1f);

            float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits = 0.5f * lengthOfDrawnLines_inLocalSpaceUnits;
            float alphaForUpcomingOrderColors = 1.0f;
            float alphaForUpcomingOrdersBlackPosMarker = 0.4f;

            Vector3 localForward_normalizedInGlobalSpace = rotationOfLocalSpace * Vector3.forward;
            Vector3 localUp_normalizedInGlobalSpace = rotationOfLocalSpace * Vector3.up;
            Vector3 localRight_normalizedInGlobalSpace = rotationOfLocalSpace * Vector3.right;
            Vector3 positionAroundWhichToDraw_global = originOfLocalSpace + localRight_normalizedInGlobalSpace * scaleOfLocalSpace.x * localPositionAroundWhichToDraw.x + localUp_normalizedInGlobalSpace * scaleOfLocalSpace.y * localPositionAroundWhichToDraw.y + localForward_normalizedInGlobalSpace * scaleOfLocalSpace.z * localPositionAroundWhichToDraw.z;

            //biggest order:
            float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits = 0.5f * coveredGridUnits_rel_inLocalSpaceUnits;
            DrawGridLinesAndDrawAroundPosVisualization_forAOrderOfMagnitude_local(distanceBetweenVisualizedGridPointsInLocalSpaceUnits_ofBiggestOrder, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, distanceBetweenRepeatingCoordsTexts_relToGridDistance, linesWidth_inLocalSpaceUnits_signFlipsPerp, true, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);

            //smaller orders:
            //extentOfWholeCascadeAlongAxis_inOrdersOwnUnits = 6.5f;
            if (draw1000grid)
            {
                //->is already biggest/cannot be a smaller order 
            }
            if (draw100grid)
            {
                if (distanceBetweenVisualizedGridPointsInLocalSpaceUnits_ofBiggestOrder > 100.0f)
                {
                    DrawGridLinesAndDrawAroundPosVisualization_forAOrderOfMagnitude_local(100.0f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, distanceBetweenRepeatingCoordsTexts_relToGridDistance, linesWidth_inLocalSpaceUnits_signFlipsPerp, false, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);
                }
            }
            if (draw10grid)
            {
                if (distanceBetweenVisualizedGridPointsInLocalSpaceUnits_ofBiggestOrder > 10.0f)
                {
                    DrawGridLinesAndDrawAroundPosVisualization_forAOrderOfMagnitude_local(10.0f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, distanceBetweenRepeatingCoordsTexts_relToGridDistance, linesWidth_inLocalSpaceUnits_signFlipsPerp, false, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);
                }
            }
            if (draw1grid)
            {
                if (distanceBetweenVisualizedGridPointsInLocalSpaceUnits_ofBiggestOrder > 1.0f)
                {
                    DrawGridLinesAndDrawAroundPosVisualization_forAOrderOfMagnitude_local(1.0f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, distanceBetweenRepeatingCoordsTexts_relToGridDistance, linesWidth_inLocalSpaceUnits_signFlipsPerp, false, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);
                }
            }
            if (draw0p1grid)
            {
                if (distanceBetweenVisualizedGridPointsInLocalSpaceUnits_ofBiggestOrder > 0.1f)
                {
                    DrawGridLinesAndDrawAroundPosVisualization_forAOrderOfMagnitude_local(0.1f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, distanceBetweenRepeatingCoordsTexts_relToGridDistance, linesWidth_inLocalSpaceUnits_signFlipsPerp, false, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);
                }
            }
            if (draw0p01grid)
            {
                if (distanceBetweenVisualizedGridPointsInLocalSpaceUnits_ofBiggestOrder > 0.01f)
                {
                    DrawGridLinesAndDrawAroundPosVisualization_forAOrderOfMagnitude_local(0.01f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, distanceBetweenRepeatingCoordsTexts_relToGridDistance, linesWidth_inLocalSpaceUnits_signFlipsPerp, false, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);
                }
            }
            if (draw0p001grid)
            {
                if (distanceBetweenVisualizedGridPointsInLocalSpaceUnits_ofBiggestOrder > 0.001f)
                {
                    DrawGridLinesAndDrawAroundPosVisualization_forAOrderOfMagnitude_local(0.001f, ref alphaForUpcomingOrderColors, ref alphaForUpcomingOrdersBlackPosMarker, originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, colorForMainX, colorForMainY, colorForMainZ, durationInSec, hiddenByNearerObjects, distanceBetweenRepeatingCoordsTexts_relToGridDistance, linesWidth_inLocalSpaceUnits_signFlipsPerp, false, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);
                }
            }

        }


        static float Get_distanceBetweenVisualizedGridPointsInVisualizedCoordSystemUnits_ofBiggestOrder(out int numberOfDrawnOrders, bool draw1000grid, bool draw100grid, bool draw10grid, bool draw1grid, bool draw0p1grid, bool draw0p01grid, bool draw0p001grid)
        {
            numberOfDrawnOrders = 0;
            float distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder = 1.0f;
            if (draw0p001grid)
            {
                distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder = 0.001f;
                numberOfDrawnOrders++;
            }
            if (draw0p01grid)
            {
                distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder = 0.01f;
                numberOfDrawnOrders++;
            }
            if (draw0p1grid)
            {
                distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder = 0.1f;
                numberOfDrawnOrders++;
            }
            if (draw1grid)
            {
                distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder = 1.0f;
                numberOfDrawnOrders++;
            }
            if (draw10grid)
            {
                distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder = 10.0f;
                numberOfDrawnOrders++;
            }
            if (draw100grid)
            {
                distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder = 100.0f;
                numberOfDrawnOrders++;
            }
            if (draw1000grid)
            {
                distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder = 1000.0f;
                numberOfDrawnOrders++;
            }
            return distanceBetweenVisualizedGridPointsInWorldUnits_ofBiggestOrder;
        }

        static void DrawGridPlanesAndDrawAroundPosVisualization_forAOrderOfMagnitude(float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, ref float alphaForUpcomingOrderColors, ref float alphaForUpcomingOrdersBlackPosMarker, Vector3 positionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, bool drawXDim, bool drawYDim, bool drawZDim, Color colorForMainX, Color colorForMainY, Color colorForMainZ, float durationInSec, bool hiddenByNearerObjects, float drawDensity, float distanceBetweenRepeatingCoordsTexts_relToGridDistance)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Color colorWithLoweredAlpha_x = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorForMainX, alphaForUpcomingOrderColors);
            Color colorWithLoweredAlpha_y = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorForMainY, alphaForUpcomingOrderColors);
            Color colorWithLoweredAlpha_z = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorForMainZ, alphaForUpcomingOrderColors);
            DrawGridPlanes_forAOrderOfMagnitude(positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, durationInSec, hiddenByNearerObjects, colorWithLoweredAlpha_x, colorWithLoweredAlpha_y, colorWithLoweredAlpha_z, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            TryDrawGridOrdersDrawAroundPosVisualization(distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, ref alphaForUpcomingOrdersBlackPosMarker, positionAroundWhichToDraw, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, durationInSec, hiddenByNearerObjects);
            alphaForUpcomingOrderColors = alphaForUpcomingOrderColors * alphaCascadingFactorForNextSmallerOrder;
        }

        static void DrawGridPlanesAndDrawAroundPosVisualization_forAOrderOfMagnitude_local(float distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, ref float alphaForUpcomingOrderColors, ref float alphaForUpcomingOrdersBlackPosMarker, Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Vector3 positionAroundWhichToDraw_global, Vector3 localForward_normalizedInGlobalSpace, Vector3 localUp_normalizedInGlobalSpace, Vector3 localRight_normalizedInGlobalSpace, Vector3 localPositionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, bool drawXDim, bool drawYDim, bool drawZDim, Color colorForMainX, Color colorForMainY, Color colorForMainZ, float durationInSec, bool hiddenByNearerObjects, float drawDensity, float distanceBetweenRepeatingCoordsTexts_relToGridDistance, bool drawALineToLocalOrigin)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Color colorWithLoweredAlpha_x = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorForMainX, alphaForUpcomingOrderColors);
            Color colorWithLoweredAlpha_y = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorForMainY, alphaForUpcomingOrderColors);
            Color colorWithLoweredAlpha_z = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorForMainZ, alphaForUpcomingOrderColors);
            float biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder = DrawGridPlanes_forAOrderOfMagnitude_local(positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, durationInSec, hiddenByNearerObjects, colorWithLoweredAlpha_x, colorWithLoweredAlpha_y, colorWithLoweredAlpha_z, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            TryDrawGridOrdersDrawAroundPosVisualization_local(biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, ref alphaForUpcomingOrdersBlackPosMarker, originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, localPositionAroundWhichToDraw, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, durationInSec, hiddenByNearerObjects, drawALineToLocalOrigin, positionAroundWhichToDraw_global);
            alphaForUpcomingOrderColors = alphaForUpcomingOrderColors * alphaCascadingFactorForNextSmallerOrder;
        }

        static void DrawGridLinesAndDrawAroundPosVisualization_forAOrderOfMagnitude(float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, ref float alphaForUpcomingOrderColors, ref float alphaForUpcomingOrdersBlackPosMarker, Vector3 positionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, bool drawXDim, bool drawYDim, bool drawZDim, Color colorForMainX, Color colorForMainY, Color colorForMainZ, float durationInSec, bool hiddenByNearerObjects, float distanceBetweenRepeatingCoordsTexts_relToGridDistance, float linesWidth, DrawEngineBasics.XGridLinesOrientation orientation_ofXLines, DrawEngineBasics.YGridLinesOrientation orientation_ofYLines, DrawEngineBasics.ZGridLinesOrientation orientation_ofZLines)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Color colorWithLoweredAlpha_x = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorForMainX, alphaForUpcomingOrderColors);
            Color colorWithLoweredAlpha_y = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorForMainY, alphaForUpcomingOrderColors);
            Color colorWithLoweredAlpha_z = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorForMainZ, alphaForUpcomingOrderColors);
            DrawGridLines_forAOrderOfMagnitude(positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, durationInSec, hiddenByNearerObjects, colorWithLoweredAlpha_x, colorWithLoweredAlpha_y, colorWithLoweredAlpha_z, distanceBetweenRepeatingCoordsTexts_relToGridDistance, linesWidth, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);
            TryDrawGridOrdersDrawAroundPosVisualization(distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, ref alphaForUpcomingOrdersBlackPosMarker, positionAroundWhichToDraw, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, durationInSec, hiddenByNearerObjects);
            alphaForUpcomingOrderColors = alphaForUpcomingOrderColors * alphaCascadingFactorForNextSmallerOrder;
        }

        static void DrawGridLinesAndDrawAroundPosVisualization_forAOrderOfMagnitude_local(float distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, ref float alphaForUpcomingOrderColors, ref float alphaForUpcomingOrdersBlackPosMarker, Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Vector3 positionAroundWhichToDraw_global, Vector3 localForward_normalizedInGlobalSpace, Vector3 localUp_normalizedInGlobalSpace, Vector3 localRight_normalizedInGlobalSpace, Vector3 localPositionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, bool drawXDim, bool drawYDim, bool drawZDim, Color colorForMainX, Color colorForMainY, Color colorForMainZ, float durationInSec, bool hiddenByNearerObjects, float distanceBetweenRepeatingCoordsTexts_relToGridDistance, float linesWidth_inLocalSpaceUnits, bool drawALineToLocalOrigin, DrawEngineBasics.XGridLinesOrientation orientation_ofXLines, DrawEngineBasics.YGridLinesOrientation orientation_ofYLines, DrawEngineBasics.ZGridLinesOrientation orientation_ofZLines)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Color colorWithLoweredAlpha_x = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorForMainX, alphaForUpcomingOrderColors);
            Color colorWithLoweredAlpha_y = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorForMainY, alphaForUpcomingOrderColors);
            Color colorWithLoweredAlpha_z = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorForMainZ, alphaForUpcomingOrderColors);
            float biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder = DrawGridLines_forAOrderOfMagnitude_local(positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, durationInSec, hiddenByNearerObjects, colorWithLoweredAlpha_x, colorWithLoweredAlpha_y, colorWithLoweredAlpha_z, distanceBetweenRepeatingCoordsTexts_relToGridDistance, linesWidth_inLocalSpaceUnits, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);
            TryDrawGridOrdersDrawAroundPosVisualization_local(biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, ref alphaForUpcomingOrdersBlackPosMarker, originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, localPositionAroundWhichToDraw, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawXDim, drawYDim, drawZDim, durationInSec, hiddenByNearerObjects, drawALineToLocalOrigin, positionAroundWhichToDraw_global);
            alphaForUpcomingOrderColors = alphaForUpcomingOrderColors * alphaCascadingFactorForNextSmallerOrder;
        }

        static void DrawGridPlanes_forAOrderOfMagnitude(Vector3 positionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, bool drawXDim, bool drawYDim, bool drawZDim, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, float durationInSec, bool hiddenByNearerObjects, Color colorForX, Color colorForY, Color colorForZ, float drawDensity, float distanceBetweenRepeatingCoordsTexts_relToGridDistance)
        {
            if (drawXDim)
            {
                DrawXDimPlanesOfGrid(positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, 0.0f, durationInSec, hiddenByNearerObjects, colorForX, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            }

            if (drawYDim)
            {
                DrawYDimPlanesOfGrid(positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, 0.0f, durationInSec, hiddenByNearerObjects, colorForY, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            }

            if (drawZDim)
            {
                DrawZDimPlanesOfGrid(positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, 0.0f, durationInSec, hiddenByNearerObjects, colorForZ, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            }
        }

        static float DrawGridPlanes_forAOrderOfMagnitude_local(Vector3 positionAroundWhichToDraw_global, Vector3 localForward_normalizedInGlobalSpace, Vector3 localUp_normalizedInGlobalSpace, Vector3 localRight_normalizedInGlobalSpace, Vector3 scaleOfLocalSpace, Vector3 localPositionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, bool drawXDim, bool drawYDim, bool drawZDim, float distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, float durationInSec, bool hiddenByNearerObjects, Color colorForX, Color colorForY, Color colorForZ, float drawDensity, float distanceBetweenRepeatingCoordsTexts_relToGridDistance)
        {
            float biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder = 0.0f;
            if (drawXDim)
            {
                float distanceBetweenVisualizedXGridPointsInWorldUnits_forThisOrder = distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder * scaleOfLocalSpace.x;
                biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder = Mathf.Max(biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, Mathf.Abs(distanceBetweenVisualizedXGridPointsInWorldUnits_forThisOrder));
                DrawXDimPlanesOfGrid_local(positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, 0.0f, durationInSec, hiddenByNearerObjects, colorForX, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            }

            if (drawYDim)
            {
                float distanceBetweenVisualizedYGridPointsInWorldUnits_forThisOrder = distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder * scaleOfLocalSpace.y;
                biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder = Mathf.Max(biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, Mathf.Abs(distanceBetweenVisualizedYGridPointsInWorldUnits_forThisOrder));
                DrawYDimPlanesOfGrid_local(positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, 0.0f, durationInSec, hiddenByNearerObjects, colorForY, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            }

            if (drawZDim)
            {
                float distanceBetweenVisualizedZGridPointsInWorldUnits_forThisOrder = distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder * scaleOfLocalSpace.z;
                biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder = Mathf.Max(biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, Mathf.Abs(distanceBetweenVisualizedZGridPointsInWorldUnits_forThisOrder));
                DrawZDimPlanesOfGrid_local(positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, 0.0f, durationInSec, hiddenByNearerObjects, colorForZ, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, drawDensity, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            }
            return biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
        }

        static void DrawGridLines_forAOrderOfMagnitude(Vector3 positionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, bool drawXDim, bool drawYDim, bool drawZDim, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, float durationInSec, bool hiddenByNearerObjects, Color colorForX, Color colorForY, Color colorForZ, float distanceBetweenRepeatingCoordsTexts_relToGridDistance, float linesWidth, DrawEngineBasics.XGridLinesOrientation orientation_ofXLines, DrawEngineBasics.YGridLinesOrientation orientation_ofYLines, DrawEngineBasics.ZGridLinesOrientation orientation_ofZLines)
        {
            if (drawXDim)
            {
                bool turned90DegAroundHisAxis = (orientation_ofXLines == DrawEngineBasics.XGridLinesOrientation.alongZ);
                DrawXDimLinesOfGrid(true, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, linesWidth, durationInSec, hiddenByNearerObjects, colorForX, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, turned90DegAroundHisAxis, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            }

            if (drawYDim)
            {
                bool turned90DegAroundHisAxis = (orientation_ofYLines == DrawEngineBasics.YGridLinesOrientation.alongZ);
                DrawYDimLinesOfGrid(true, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, linesWidth, durationInSec, hiddenByNearerObjects, colorForY, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, turned90DegAroundHisAxis, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            }

            if (drawZDim)
            {
                bool turned90DegAroundHisAxis = (orientation_ofZLines == DrawEngineBasics.ZGridLinesOrientation.alongX);
                DrawZDimLinesOfGrid(true, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, linesWidth, durationInSec, hiddenByNearerObjects, colorForZ, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, turned90DegAroundHisAxis, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            }
        }

        static float DrawGridLines_forAOrderOfMagnitude_local(Vector3 positionAroundWhichToDraw_global, Vector3 localForward_normalizedInGlobalSpace, Vector3 localUp_normalizedInGlobalSpace, Vector3 localRight_normalizedInGlobalSpace, Vector3 scaleOfLocalSpace, Vector3 localPositionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, bool drawXDim, bool drawYDim, bool drawZDim, float distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, float durationInSec, bool hiddenByNearerObjects, Color colorForX, Color colorForY, Color colorForZ, float distanceBetweenRepeatingCoordsTexts_relToGridDistance, float linesWidth_inLocalSpaceUnits, DrawEngineBasics.XGridLinesOrientation orientation_ofXLines, DrawEngineBasics.YGridLinesOrientation orientation_ofYLines, DrawEngineBasics.ZGridLinesOrientation orientation_ofZLines)
        {
            float biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder = 0.0f;
            if (drawXDim)
            {
                float distanceBetweenVisualizedXGridPointsInWorldUnits_forThisOrder = distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder * scaleOfLocalSpace.x;
                biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder = Mathf.Max(biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, Mathf.Abs(distanceBetweenVisualizedXGridPointsInWorldUnits_forThisOrder));
                bool turned90DegAroundHisAxis = (orientation_ofXLines == DrawEngineBasics.XGridLinesOrientation.alongZ);
                DrawXDimLinesOfGridLocal(true, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, linesWidth_inLocalSpaceUnits, durationInSec, hiddenByNearerObjects, colorForX, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, turned90DegAroundHisAxis, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            }

            if (drawYDim)
            {
                float distanceBetweenVisualizedYGridPointsInWorldUnits_forThisOrder = distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder * scaleOfLocalSpace.y;
                biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder = Mathf.Max(biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, Mathf.Abs(distanceBetweenVisualizedYGridPointsInWorldUnits_forThisOrder));
                bool turned90DegAroundHisAxis = (orientation_ofYLines == DrawEngineBasics.YGridLinesOrientation.alongZ);
                DrawYDimLinesOfGridLocal(true, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, linesWidth_inLocalSpaceUnits, durationInSec, hiddenByNearerObjects, colorForY, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, turned90DegAroundHisAxis, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            }

            if (drawZDim)
            {
                float distanceBetweenVisualizedZGridPointsInWorldUnits_forThisOrder = distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder * scaleOfLocalSpace.z;
                biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder = Mathf.Max(biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, Mathf.Abs(distanceBetweenVisualizedZGridPointsInWorldUnits_forThisOrder));
                bool turned90DegAroundHisAxis = (orientation_ofZLines == DrawEngineBasics.ZGridLinesOrientation.alongX);
                DrawZDimLinesOfGridLocal(true, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, linesWidth_inLocalSpaceUnits, durationInSec, hiddenByNearerObjects, colorForZ, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, turned90DegAroundHisAxis, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            }
            return biggest_absDistanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
        }

        static float relStrokeWidth_forDrawAroundPosCoordianteText = 68000.0f;
        static void TryDrawGridOrdersDrawAroundPosVisualization(float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, ref float alphaForUpcomingOrdersBlackPosMarker, Vector3 positionAroundWhichToDraw, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, bool drawXDim, bool drawYDim, bool drawZDim, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DrawEngineBasics.hide_positionAroundWhichToDraw_forGrids == false)
            {
                float extentOfWholeCascadeAlongAxis_inWorldUnits = extentOfWholeCascadeAlongAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;

                UtilitiesDXXL_DrawBasics.Set_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM_reversible(Mathf.CeilToInt(relStrokeWidth_forDrawAroundPosCoordianteText));
                DrawBasics.Point(positionAroundWhichToDraw, null, default(Color), extentOfWholeCascadeAlongAxis_inWorldUnits, 0.0f, default(Color), default(Quaternion), false, true, false, durationInSec, hiddenByNearerObjects);
                //DrawBasics.Point(positionAroundWhichToDraw, null, default(Color), extentOfWholeCascadeAlongAxis_inWorldUnits, 0.0f, UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(Color.black, alphaForUpcomingOrdersBlackPosMarker), default(Quaternion), false, true, false, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.Reverse_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM();

                alphaForUpcomingOrdersBlackPosMarker = alphaForUpcomingOrdersBlackPosMarker / alphaCascadingFactorForNextSmallerOrder_forPointsBlackOverdraw;
                DrawSkewedPosIndicatingCubes(positionAroundWhichToDraw, colorFor_skewedPosIndicatingCubes, extentOfWholeCascadeAlongAxis_inWorldUnits, drawXDim, drawYDim, drawZDim, durationInSec, hiddenByNearerObjects);
            }
        }

        static void TryDrawGridOrdersDrawAroundPosVisualization_local(float biggest_distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, ref float alphaForUpcomingOrdersBlackPosMarker, Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Vector3 localPositionAroundWhichToDraw, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, bool drawXDim, bool drawYDim, bool drawZDim, float durationInSec, bool hiddenByNearerObjects, bool drawALineToLocalOrigin, Vector3 positionAroundWhichToDraw_global)
        {
            if (DrawEngineBasics.hide_positionAroundWhichToDraw_forGrids == false)
            {
                float extentOfWholeCascadeAlongAxis_inWorldUnits = extentOfWholeCascadeAlongAxis_inOrdersOwnUnits * biggest_distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
                if (forceSkip_drawAroundPosVisualizationLocal == false)
                {
                    bool used_drawALineToLocalOrigin = forceSkip_drawingLocalOrigin ? false : drawALineToLocalOrigin;

                    UtilitiesDXXL_DrawBasics.Set_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM_reversible(Mathf.CeilToInt(relStrokeWidth_forDrawAroundPosCoordianteText));
                    DrawBasics.PointLocal(localPositionAroundWhichToDraw, originOfLocalSpace, rotationOfLocalSpace, scaleOfLocalSpace, null, default(Color), extentOfWholeCascadeAlongAxis_inWorldUnits, 0.0f, default(Color), default, false, true, false, used_drawALineToLocalOrigin, false, durationInSec, hiddenByNearerObjects);
                    //DrawBasics.PointLocal(localPositionAroundWhichToDraw, originOfLocalSpace, rotationOfLocalSpace, scaleOfLocalSpace, null, default(Color), extentOfWholeCascadeAlongAxis_inWorldUnits, 0.0f, UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(Color.black, alphaForUpcomingOrdersBlackPosMarker), default, false, true, false, false, false, durationInSec, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM();

                    alphaForUpcomingOrdersBlackPosMarker = alphaForUpcomingOrdersBlackPosMarker / alphaCascadingFactorForNextSmallerOrder_forPointsBlackOverdraw;
                }
                DrawSkewedPosIndicatingCubes_local(rotationOfLocalSpace, positionAroundWhichToDraw_global, colorFor_skewedPosIndicatingCubes, extentOfWholeCascadeAlongAxis_inWorldUnits, drawXDim, drawYDim, drawZDim, durationInSec, hiddenByNearerObjects); //-> is outside of "forceSkip_drawAroundPosVisualzationLocal", so that the drawAroundPos of "GridVisualizer's" with strongly non-uniform dimension scales are still somehow visible, also if the coordinate texts have been scaled to unreadable small.
            }
        }

        static Vector3 GetDistanceTextPositionOffset_relToVectorLine_forDisanceDisplayABOVEtheVectorLine(float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, Vector3 vectorAlongGridLine_normalized)
        {
            //slightly confusing naming: The text display ABOVE the vector line is for the Vector to the grid pos BELOW the drawAroundPos
            return (0.07f * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * vectorAlongGridLine_normalized);
        }

        static Vector3 GetDistanceTextPositionOffset_relToVectorLine_forDisanceDisplayBELOWtheVectorLine(float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, Vector3 vectorAlongGridLine_normalized)
        {
            //slightly confusing naming: The text display BELOW the vector line is for the Vector to the grid pos ABOVE the drawAroundPos
            return (-0.05f) * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * vectorAlongGridLine_normalized;
        }

        static void DrawXDimPlanesOfGrid(Vector3 positionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, float linesWidth, float durationInSec, bool hiddenByNearerObjects, Color color, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, float drawDensity, float distanceBetweenRepeatingCoordsTexts_relToGridDistance)
        {
            DrawXDimLinesOfGrid(false, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, linesWidth, durationInSec, hiddenByNearerObjects, color, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, false, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            DrawXDimLinesOfGrid(false, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, linesWidth, durationInSec, hiddenByNearerObjects, color, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, true, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            DrawXDimPlanesDenseWithoutText(positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawDensity, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, durationInSec, hiddenByNearerObjects, color, false);
            DrawXDimPlanesDenseWithoutText(positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawDensity, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, durationInSec, hiddenByNearerObjects, color, true);
        }

        static void DrawYDimPlanesOfGrid(Vector3 positionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, float linesWidth, float durationInSec, bool hiddenByNearerObjects, Color color, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, float drawDensity, float distanceBetweenRepeatingCoordsTexts_relToGridDistance)
        {
            DrawYDimLinesOfGrid(false, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, linesWidth, durationInSec, hiddenByNearerObjects, color, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, false, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            DrawYDimLinesOfGrid(false, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, linesWidth, durationInSec, hiddenByNearerObjects, color, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, true, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            DrawYDimPlanesDenseWithoutText(positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawDensity, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, durationInSec, hiddenByNearerObjects, color, false);
            DrawYDimPlanesDenseWithoutText(positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawDensity, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, durationInSec, hiddenByNearerObjects, color, true);
        }

        static void DrawZDimPlanesOfGrid(Vector3 positionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, float linesWidth, float durationInSec, bool hiddenByNearerObjects, Color color, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, float drawDensity, float distanceBetweenRepeatingCoordsTexts_relToGridDistance)
        {
            DrawZDimLinesOfGrid(false, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, linesWidth, durationInSec, hiddenByNearerObjects, color, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, false, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            DrawZDimLinesOfGrid(false, positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, linesWidth, durationInSec, hiddenByNearerObjects, color, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, true, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            DrawZDimPlanesDenseWithoutText(positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawDensity, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, durationInSec, hiddenByNearerObjects, color, false);
            DrawZDimPlanesDenseWithoutText(positionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawDensity, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, durationInSec, hiddenByNearerObjects, color, true);
        }

        static void DrawXDimPlanesOfGrid_local(Vector3 positionAroundWhichToDraw_global, Vector3 localForward_normalizedInGlobalSpace, Vector3 localUp_normalizedInGlobalSpace, Vector3 localRight_normalizedInGlobalSpace, Vector3 scaleOfLocalSpace, Vector3 localPositionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, float linesWidth, float durationInSec, bool hiddenByNearerObjects, Color color, float distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, float drawDensity, float distanceBetweenRepeatingCoordsTexts_relToGridDistance)
        {
            DrawXDimLinesOfGridLocal(false, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, linesWidth, durationInSec, hiddenByNearerObjects, color, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, false, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            DrawXDimLinesOfGridLocal(false, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, linesWidth, durationInSec, hiddenByNearerObjects, color, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, true, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            DrawXDimPlanesDenseWithoutTextLocal(positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawDensity, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, durationInSec, hiddenByNearerObjects, color, false);
            DrawXDimPlanesDenseWithoutTextLocal(positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawDensity, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, durationInSec, hiddenByNearerObjects, color, true);
        }

        static void DrawYDimPlanesOfGrid_local(Vector3 positionAroundWhichToDraw_global, Vector3 localForward_normalizedInGlobalSpace, Vector3 localUp_normalizedInGlobalSpace, Vector3 localRight_normalizedInGlobalSpace, Vector3 scaleOfLocalSpace, Vector3 localPositionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, float linesWidth, float durationInSec, bool hiddenByNearerObjects, Color color, float distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, float drawDensity, float distanceBetweenRepeatingCoordsTexts_relToGridDistance)
        {
            DrawYDimLinesOfGridLocal(false, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, linesWidth, durationInSec, hiddenByNearerObjects, color, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, false, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            DrawYDimLinesOfGridLocal(false, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, linesWidth, durationInSec, hiddenByNearerObjects, color, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, true, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            DrawYDimPlanesDenseWithoutTextLocal(positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawDensity, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, durationInSec, hiddenByNearerObjects, color, false);
            DrawYDimPlanesDenseWithoutTextLocal(positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawDensity, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, durationInSec, hiddenByNearerObjects, color, true);
        }

        static void DrawZDimPlanesOfGrid_local(Vector3 positionAroundWhichToDraw_global, Vector3 localForward_normalizedInGlobalSpace, Vector3 localUp_normalizedInGlobalSpace, Vector3 localRight_normalizedInGlobalSpace, Vector3 scaleOfLocalSpace, Vector3 localPositionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, float linesWidth, float durationInSec, bool hiddenByNearerObjects, Color color, float distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, float drawDensity, float distanceBetweenRepeatingCoordsTexts_relToGridDistance)
        {
            DrawZDimLinesOfGridLocal(false, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, linesWidth, durationInSec, hiddenByNearerObjects, color, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, false, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            DrawZDimLinesOfGridLocal(false, positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, linesWidth, durationInSec, hiddenByNearerObjects, color, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, true, distanceBetweenRepeatingCoordsTexts_relToGridDistance);
            DrawZDimPlanesDenseWithoutTextLocal(positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawDensity, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, durationInSec, hiddenByNearerObjects, color, false);
            DrawZDimPlanesDenseWithoutTextLocal(positionAroundWhichToDraw_global, localForward_normalizedInGlobalSpace, localUp_normalizedInGlobalSpace, localRight_normalizedInGlobalSpace, scaleOfLocalSpace, localPositionAroundWhichToDraw, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, drawDensity, distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, durationInSec, hiddenByNearerObjects, color, true);
        }

        static void DrawXDimLinesOfGrid(bool forGridLINES_notForGridPLANES, Vector3 positionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, float linesWidth, float durationInSec, bool hiddenByNearerObjects, Color color, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, bool theXGridLines_areAlingedAlongZ_notAlongY, float distanceBetweenRepeatingCoordsTexts_relToGridDistance)
        {
            float extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits = extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float extentOfWholeCascadeAlongAxis_inWorldUnits = extentOfWholeCascadeAlongAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float textPosOffset = DrawEngineBasics.offsetForCoordinateTextDisplays_inGrids * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float textSize = distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * DrawEngineBasics.SizeScalingForCoordinateTexts_inGrids;
            float linesWidth_worldSpace = linesWidth * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float absHalfLinesWidth = Mathf.Abs(0.5f * linesWidth_worldSpace);
            float spaceBetweenLineAndCoordsText = absHalfLinesWidth + rel_spaceBetweenLineAndCoordsText * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;

            float positionAroundWhichToDraw_expressedInUnitsOfThisOrder_x = positionAroundWhichToDraw.x / distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float xPos_ofLowestGridPos = Mathf.Round(positionAroundWhichToDraw_expressedInUnitsOfThisOrder_x - extentOfWholeCascadeAlongAxis_inOrdersOwnUnits) * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;

            bool lineWidthAndText_growsInsideRepresentedGridAxis_notPerpToIt = (linesWidth >= 0.0f);
            Vector3 vectorAlongGridLine_normalized = theXGridLines_areAlingedAlongZ_notAlongY ? Vector3.forward : Vector3.up;
            Vector3 vectorAlongPerpGrowingLineWidth_normalized = theXGridLines_areAlingedAlongZ_notAlongY ? Vector3.up : Vector3.forward;
            Vector3 vectorAlongGrowingLineWidth_normalized = lineWidthAndText_growsInsideRepresentedGridAxis_notPerpToIt ? Vector3.left : vectorAlongPerpGrowingLineWidth_normalized;

            Vector3 lineStart_ofLowestGridPos;
            Vector3 lineEnd_ofLowestGridPos;
            if (theXGridLines_areAlingedAlongZ_notAlongY)
            {
                lineStart_ofLowestGridPos = new Vector3(xPos_ofLowestGridPos, positionAroundWhichToDraw.y, positionAroundWhichToDraw.z - extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits);
                lineEnd_ofLowestGridPos = new Vector3(xPos_ofLowestGridPos, positionAroundWhichToDraw.y, positionAroundWhichToDraw.z + extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits);
            }
            else
            {
                lineStart_ofLowestGridPos = new Vector3(xPos_ofLowestGridPos, positionAroundWhichToDraw.y - extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits, positionAroundWhichToDraw.z);
                lineEnd_ofLowestGridPos = new Vector3(xPos_ofLowestGridPos, positionAroundWhichToDraw.y + extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits, positionAroundWhichToDraw.z);
            }

            float lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits = 2.0f * extentOfWholeCascadeAlongAxis_inOrdersOwnUnits;
            int numberOfVisualizedGridPointsAlongAxis = Mathf.RoundToInt(lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits) + 1;
            numberOfVisualizedGridPointsAlongAxis = Mathf.Max(numberOfVisualizedGridPointsAlongAxis, 3);
            for (int i = 0; i < numberOfVisualizedGridPointsAlongAxis; i++)
            {
                if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
                Vector3 shiftVector = Vector3.right * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * i;
                Vector3 lineStartPos = lineStart_ofLowestGridPos + shiftVector;
                Vector3 lineEndPos = lineEnd_ofLowestGridPos + shiftVector;
                float distanceOfVisualizedGridPosToPosAroundWhichToDraw_0to1 = Mathf.Abs((lineStartPos.x - positionAroundWhichToDraw.x) / extentOfWholeCascadeAlongAxis_inWorldUnits);
                if (distanceOfVisualizedGridPosToPosAroundWhichToDraw_0to1 > 1.12f) { continue; } //-> saving some performance for planes that are anyway not visible (because their alpha reached zero)
                Color lineColor = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 1.0f - 0.9f * distanceOfVisualizedGridPosToPosAroundWhichToDraw_0to1);
                Line_fadeableAnimSpeed.InternalDraw(lineStartPos, lineEndPos, lineColor, linesWidth_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, vectorAlongGrowingLineWidth_normalized, true, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                bool drawCoordsTextAtLeastOnce = (distanceBetweenRepeatingCoordsTexts_relToGridDistance >= 0.0f);
                if (drawCoordsTextAtLeastOnce)
                {
                    string coordsAsText = GetCoordsAsText_forXDimLines(lineStartPos.x, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder);
                    Vector3 textPos_ifTextIsInsideRepresentedGridAxis = theXGridLines_areAlingedAlongZ_notAlongY ? new Vector3(lineStartPos.x - spaceBetweenLineAndCoordsText, positionAroundWhichToDraw.y, positionAroundWhichToDraw.z + textPosOffset) : new Vector3(lineStartPos.x - spaceBetweenLineAndCoordsText, positionAroundWhichToDraw.y + textPosOffset, positionAroundWhichToDraw.z);
                    Vector3 textPos_ifTextIsPerpToRepresentedGridAxis = theXGridLines_areAlingedAlongZ_notAlongY ? new Vector3(lineStartPos.x, positionAroundWhichToDraw.y + spaceBetweenLineAndCoordsText, positionAroundWhichToDraw.z + textPosOffset) : new Vector3(lineStartPos.x, positionAroundWhichToDraw.y + textPosOffset, positionAroundWhichToDraw.z + spaceBetweenLineAndCoordsText);
                    Vector3 textPos = lineWidthAndText_growsInsideRepresentedGridAxis_notPerpToIt ? textPos_ifTextIsInsideRepresentedGridAxis : textPos_ifTextIsPerpToRepresentedGridAxis;
                    UtilitiesDXXL_Text.Write(coordsAsText, textPos, lineColor, textSize, vectorAlongGridLine_normalized, vectorAlongGrowingLineWidth_normalized, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                    DrawAdditionalCoordsTexts(vectorAlongGridLine_normalized, vectorAlongGrowingLineWidth_normalized, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, distanceBetweenRepeatingCoordsTexts_relToGridDistance, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, lineColor, textPos, coordsAsText, textSize, durationInSec, hiddenByNearerObjects);
                }
            }

            TryDrawDistanceLines_fromDrawAroundPos_toNeighboringGridValue(forGridLINES_notForGridPLANES, 1.0f, Vector3.right, vectorAlongGridLine_normalized, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, positionAroundWhichToDraw, positionAroundWhichToDraw_expressedInUnitsOfThisOrder_x, color, durationInSec, hiddenByNearerObjects);
        }

        static string GetCoordsAsText_forXDimLines(float lineStartPos_x, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder)
        {
            if (DrawEngineBasics.skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes)
            {
                return ("" + (Mathf.Round(lineStartPos_x / distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder) * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder)); //-> fixing floatCalculationImprecisionErrors (like "0.099999999" -> "0.1")
            }
            else
            {
                return ("x = " + (Mathf.Round(lineStartPos_x / distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder) * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder)); //-> fixing floatCalculationImprecisionErrors (like "0.099999999" -> "0.1")
            }
        }

        static void DrawYDimLinesOfGrid(bool forGridLINES_notForGridPLANES, Vector3 positionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, float linesWidth, float durationInSec, bool hiddenByNearerObjects, Color color, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, bool theYGridLines_areAlingedAlongZ_notAlongX, float distanceBetweenRepeatingCoordsTexts_relToGridDistance)
        {
            float extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits = extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float extentOfWholeCascadeAlongAxis_inWorldUnits = extentOfWholeCascadeAlongAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float textPosOffset = DrawEngineBasics.offsetForCoordinateTextDisplays_inGrids * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float textSize = distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * DrawEngineBasics.SizeScalingForCoordinateTexts_inGrids;
            float linesWidth_worldSpace = linesWidth * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float absHalfLinesWidth = Mathf.Abs(0.5f * linesWidth_worldSpace);
            float spaceBetweenLineAndCoordsText = absHalfLinesWidth + rel_spaceBetweenLineAndCoordsText * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;

            float positionAroundWhichToDraw_expressedInUnitsOfThisOrder_y = positionAroundWhichToDraw.y / distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float yPos_ofLowestGridPos = Mathf.Round(positionAroundWhichToDraw_expressedInUnitsOfThisOrder_y - extentOfWholeCascadeAlongAxis_inOrdersOwnUnits) * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;

            bool lineWidthAndText_growsInsideRepresentedGridAxis_notPerpToIt = (linesWidth >= 0.0f);
            Vector3 vectorAlongGridLine_normalized = theYGridLines_areAlingedAlongZ_notAlongX ? Vector3.forward : Vector3.right;
            Vector3 vectorAlongPerpGrowingLineWidth_normalized = theYGridLines_areAlingedAlongZ_notAlongX ? Vector3.left : Vector3.forward;
            Vector3 vectorAlongGrowingLineWidth_normalized = lineWidthAndText_growsInsideRepresentedGridAxis_notPerpToIt ? Vector3.up : vectorAlongPerpGrowingLineWidth_normalized;

            Vector3 lineStart_ofLowestGridPos;
            Vector3 lineEnd_ofLowestGridPos;
            if (theYGridLines_areAlingedAlongZ_notAlongX)
            {
                lineStart_ofLowestGridPos = new Vector3(positionAroundWhichToDraw.x, yPos_ofLowestGridPos, positionAroundWhichToDraw.z - extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits);
                lineEnd_ofLowestGridPos = new Vector3(positionAroundWhichToDraw.x, yPos_ofLowestGridPos, positionAroundWhichToDraw.z + extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits);
            }
            else
            {
                lineStart_ofLowestGridPos = new Vector3(positionAroundWhichToDraw.x - extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits, yPos_ofLowestGridPos, positionAroundWhichToDraw.z);
                lineEnd_ofLowestGridPos = new Vector3(positionAroundWhichToDraw.x + extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits, yPos_ofLowestGridPos, positionAroundWhichToDraw.z);
            }

            float lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits = 2.0f * extentOfWholeCascadeAlongAxis_inOrdersOwnUnits;
            int numberOfVisualizedGridPointsAlongAxis = Mathf.RoundToInt(lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits) + 1;
            numberOfVisualizedGridPointsAlongAxis = Mathf.Max(numberOfVisualizedGridPointsAlongAxis, 3);
            for (int i = 0; i < numberOfVisualizedGridPointsAlongAxis; i++)
            {
                if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
                Vector3 shiftVector = Vector3.up * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * i;
                Vector3 lineStartPos = lineStart_ofLowestGridPos + shiftVector;
                Vector3 lineEndPos = lineEnd_ofLowestGridPos + shiftVector;
                float distanceOfVisualizedGridPosToPosAroundWhichToDraw_0to1 = Mathf.Abs((lineStartPos.y - positionAroundWhichToDraw.y) / extentOfWholeCascadeAlongAxis_inWorldUnits);
                if (distanceOfVisualizedGridPosToPosAroundWhichToDraw_0to1 > 1.12f) { continue; } //-> saving some performance for planes that are anyway not visible (because their alpha reached zero)
                Color lineColor = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 1.0f - 0.9f * distanceOfVisualizedGridPosToPosAroundWhichToDraw_0to1);
                Line_fadeableAnimSpeed.InternalDraw(lineStartPos, lineEndPos, lineColor, linesWidth_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, vectorAlongGrowingLineWidth_normalized, true, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                bool drawCoordsTextAtLeastOnce = (distanceBetweenRepeatingCoordsTexts_relToGridDistance >= 0.0f);
                if (drawCoordsTextAtLeastOnce)
                {
                    string coordsAsText = GetCoordsAsText_forYDimLines(lineStartPos.y, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder);
                    Vector3 textPos_ifTextIsInsideRepresentedGridAxis = theYGridLines_areAlingedAlongZ_notAlongX ? new Vector3(positionAroundWhichToDraw.x, lineStartPos.y + spaceBetweenLineAndCoordsText, positionAroundWhichToDraw.z + textPosOffset) : new Vector3(positionAroundWhichToDraw.x + textPosOffset, lineStartPos.y + spaceBetweenLineAndCoordsText, positionAroundWhichToDraw.z);
                    Vector3 textPos_ifTextIsPerpToRepresentedGridAxis = theYGridLines_areAlingedAlongZ_notAlongX ? new Vector3(positionAroundWhichToDraw.x - spaceBetweenLineAndCoordsText, lineStartPos.y, positionAroundWhichToDraw.z + textPosOffset) : new Vector3(positionAroundWhichToDraw.x + textPosOffset, lineStartPos.y, positionAroundWhichToDraw.z + spaceBetweenLineAndCoordsText);
                    Vector3 textPos = lineWidthAndText_growsInsideRepresentedGridAxis_notPerpToIt ? textPos_ifTextIsInsideRepresentedGridAxis : textPos_ifTextIsPerpToRepresentedGridAxis;
                    UtilitiesDXXL_Text.Write(coordsAsText, textPos, lineColor, textSize, vectorAlongGridLine_normalized, vectorAlongGrowingLineWidth_normalized, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                    DrawAdditionalCoordsTexts(vectorAlongGridLine_normalized, vectorAlongGrowingLineWidth_normalized, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, distanceBetweenRepeatingCoordsTexts_relToGridDistance, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, lineColor, textPos, coordsAsText, textSize, durationInSec, hiddenByNearerObjects);
                }
            }

            TryDrawDistanceLines_fromDrawAroundPos_toNeighboringGridValue(forGridLINES_notForGridPLANES, 1.0f, Vector3.up, vectorAlongGridLine_normalized, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, positionAroundWhichToDraw, positionAroundWhichToDraw_expressedInUnitsOfThisOrder_y, color, durationInSec, hiddenByNearerObjects);
        }

        static string GetCoordsAsText_forYDimLines(float lineStartPos_y, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder)
        {
            if (DrawEngineBasics.skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes)
            {
                return ("" + (Mathf.Round(lineStartPos_y / distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder) * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder)); //-> fixing floatCalculationImprecisionErrors (like "0.099999999" -> "0.1")
            }
            else
            {
                return ("y = " + (Mathf.Round(lineStartPos_y / distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder) * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder)); //-> fixing floatCalculationImprecisionErrors (like "0.099999999" -> "0.1")
            }
        }

        static void DrawZDimLinesOfGrid(bool forGridLINES_notForGridPLANES, Vector3 positionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, float linesWidth, float durationInSec, bool hiddenByNearerObjects, Color color, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, bool theZGridLines_areAlingedAlongX_notAlongY, float distanceBetweenRepeatingCoordsTexts_relToGridDistance)
        {
            float extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits = extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float extentOfWholeCascadeAlongAxis_inWorldUnits = extentOfWholeCascadeAlongAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float textPosOffset = DrawEngineBasics.offsetForCoordinateTextDisplays_inGrids * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float textSize = distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * DrawEngineBasics.SizeScalingForCoordinateTexts_inGrids;
            float linesWidth_worldSpace = linesWidth * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float absHalfLinesWidth = Mathf.Abs(0.5f * linesWidth_worldSpace);
            float spaceBetweenLineAndCoordsText = absHalfLinesWidth + rel_spaceBetweenLineAndCoordsText * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;

            float positionAroundWhichToDraw_expressedInUnitsOfThisOrder_z = positionAroundWhichToDraw.z / distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float zPos_ofLowestGridPos = Mathf.Round(positionAroundWhichToDraw_expressedInUnitsOfThisOrder_z - extentOfWholeCascadeAlongAxis_inOrdersOwnUnits) * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;

            bool lineWidthAndText_growsInsideRepresentedGridAxis_notPerpToIt = (linesWidth >= 0.0f);
            Vector3 vectorAlongGridLine_normalized = theZGridLines_areAlingedAlongX_notAlongY ? Vector3.right : Vector3.up;
            Vector3 vectorAlongPerpGrowingLineWidth_normalized = theZGridLines_areAlingedAlongX_notAlongY ? Vector3.up : Vector3.left;
            Vector3 textUp_normalized_ifTextIsInsideRepresentedGridAxis = theZGridLines_areAlingedAlongX_notAlongY ? Vector3.forward : Vector3.back;
            Vector3 vectorAlongGrowingLineWidth_normalized = lineWidthAndText_growsInsideRepresentedGridAxis_notPerpToIt ? textUp_normalized_ifTextIsInsideRepresentedGridAxis : vectorAlongPerpGrowingLineWidth_normalized;

            Vector3 lineStart_ofLowestGridPos;
            Vector3 lineEnd_ofLowestGridPos;
            if (theZGridLines_areAlingedAlongX_notAlongY)
            {
                lineStart_ofLowestGridPos = new Vector3(positionAroundWhichToDraw.x - extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits, positionAroundWhichToDraw.y, zPos_ofLowestGridPos);
                lineEnd_ofLowestGridPos = new Vector3(positionAroundWhichToDraw.x + extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits, positionAroundWhichToDraw.y, zPos_ofLowestGridPos);
            }
            else
            {
                lineStart_ofLowestGridPos = new Vector3(positionAroundWhichToDraw.x, positionAroundWhichToDraw.y - extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits, zPos_ofLowestGridPos);
                lineEnd_ofLowestGridPos = new Vector3(positionAroundWhichToDraw.x, positionAroundWhichToDraw.y + extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits, zPos_ofLowestGridPos);
            }

            float lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits = 2.0f * extentOfWholeCascadeAlongAxis_inOrdersOwnUnits;
            int numberOfVisualizedGridPointsAlongAxis = Mathf.RoundToInt(lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits) + 1;
            numberOfVisualizedGridPointsAlongAxis = Mathf.Max(numberOfVisualizedGridPointsAlongAxis, 3);
            for (int i = 0; i < numberOfVisualizedGridPointsAlongAxis; i++)
            {
                if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
                Vector3 shiftVector = Vector3.forward * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * i;
                Vector3 lineStartPos = lineStart_ofLowestGridPos + shiftVector;
                Vector3 lineEndPos = lineEnd_ofLowestGridPos + shiftVector;
                float distanceOfVisualizedGridPosToPosAroundWhichToDraw_0to1 = Mathf.Abs((lineStartPos.z - positionAroundWhichToDraw.z) / extentOfWholeCascadeAlongAxis_inWorldUnits);
                if (distanceOfVisualizedGridPosToPosAroundWhichToDraw_0to1 > 1.12f) { continue; } //-> saving some performance for planes that are anyway not visible (because their alpha reached zero)
                Color lineColor = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 1.0f - 0.9f * distanceOfVisualizedGridPosToPosAroundWhichToDraw_0to1);
                Line_fadeableAnimSpeed.InternalDraw(lineStartPos, lineEndPos, lineColor, linesWidth_worldSpace, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, vectorAlongGrowingLineWidth_normalized, true, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                bool drawCoordsTextAtLeastOnce = (distanceBetweenRepeatingCoordsTexts_relToGridDistance >= 0.0f);
                if (drawCoordsTextAtLeastOnce)
                {
                    string coordsAsText = GetCoordsAsText_forZDimLines(lineStartPos.z, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder);
                    Vector3 textPos_ifTextIsInsideRepresentedGridAxis = theZGridLines_areAlingedAlongX_notAlongY ? new Vector3(positionAroundWhichToDraw.x + textPosOffset, positionAroundWhichToDraw.y, lineStartPos.z + spaceBetweenLineAndCoordsText) : new Vector3(positionAroundWhichToDraw.x, positionAroundWhichToDraw.y + textPosOffset, lineStartPos.z - spaceBetweenLineAndCoordsText);
                    Vector3 textPos_ifTextIsPerpToRepresentedGridAxis = theZGridLines_areAlingedAlongX_notAlongY ? new Vector3(positionAroundWhichToDraw.x + textPosOffset, positionAroundWhichToDraw.y + spaceBetweenLineAndCoordsText, lineStartPos.z) : new Vector3(positionAroundWhichToDraw.x - spaceBetweenLineAndCoordsText, positionAroundWhichToDraw.y + textPosOffset, lineStartPos.z);
                    Vector3 textPos = lineWidthAndText_growsInsideRepresentedGridAxis_notPerpToIt ? textPos_ifTextIsInsideRepresentedGridAxis : textPos_ifTextIsPerpToRepresentedGridAxis;
                    UtilitiesDXXL_Text.Write(coordsAsText, textPos, lineColor, textSize, vectorAlongGridLine_normalized, vectorAlongGrowingLineWidth_normalized, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                    DrawAdditionalCoordsTexts(vectorAlongGridLine_normalized, vectorAlongGrowingLineWidth_normalized, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, distanceBetweenRepeatingCoordsTexts_relToGridDistance, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, lineColor, textPos, coordsAsText, textSize, durationInSec, hiddenByNearerObjects);
                }
            }

            TryDrawDistanceLines_fromDrawAroundPos_toNeighboringGridValue(forGridLINES_notForGridPLANES, 1.0f, Vector3.forward, vectorAlongGridLine_normalized, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, positionAroundWhichToDraw, positionAroundWhichToDraw_expressedInUnitsOfThisOrder_z, color, durationInSec, hiddenByNearerObjects);
        }

        static string GetCoordsAsText_forZDimLines(float lineStartPos_z, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder)
        {
            if (DrawEngineBasics.skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes)
            {
                return ("" + (Mathf.Round(lineStartPos_z / distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder) * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder)); //-> fixing floatCalculationImprecisionErrors (like "0.099999999" -> "0.1")
            }
            else
            {
                return ("z = " + (Mathf.Round(lineStartPos_z / distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder) * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder)); //-> fixing floatCalculationImprecisionErrors (like "0.099999999" -> "0.1")
            }
        }

        static void DrawXDimLinesOfGridLocal(bool forGridLINES_notForGridPLANES, Vector3 positionAroundWhichToDraw_global, Vector3 localForward_normalizedInGlobalSpace, Vector3 localUp_normalizedInGlobalSpace, Vector3 localRight_normalizedInGlobalSpace, Vector3 scaleOfLocalSpace, Vector3 localPositionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, float linesWidth_inLocalSpaceUnits, float durationInSec, bool hiddenByNearerObjects, Color color, float distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, bool theXGridLines_areAlingedAlongZ_notAlongY, float distanceBetweenRepeatingCoordsTexts_relToGridDistance)
        {
            float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder = distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder * scaleOfLocalSpace.x;
            float extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits = extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float extentOfWholeCascadeAlongAxis_inWorldUnits = extentOfWholeCascadeAlongAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float textPosOffset = DrawEngineBasics.offsetForCoordinateTextDisplays_inGrids * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float textSize = distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * DrawEngineBasics.SizeScalingForCoordinateTexts_inGrids;
            float linesWidth_inGlobalUnits = linesWidth_inLocalSpaceUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float absHalfLinesWidth_inGlobalUnits = Mathf.Abs(0.5f * linesWidth_inGlobalUnits);
            float spaceBetweenLineAndCoordsText_inGlobalUnits = absHalfLinesWidth_inGlobalUnits + rel_spaceBetweenLineAndCoordsText * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;

            float localPositionAroundWhichToDraw_expressedInUnitsOfThisOrder_x = localPositionAroundWhichToDraw.x / distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder;
            float localXPos_ofLowestGridPos_inLocalSpaceUnits_ifUnrotated = Mathf.Round(localPositionAroundWhichToDraw_expressedInUnitsOfThisOrder_x - extentOfWholeCascadeAlongAxis_inOrdersOwnUnits) * distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder;

            bool lineWidthAndText_growsInsideRepresentedGridAxis_notPerpToIt = (linesWidth_inLocalSpaceUnits >= 0.0f);
            float distance_fromDrawAroundPos_to_smallestXGridPos_inGlobalWorldUnits = Mathf.Abs(localXPos_ofLowestGridPos_inLocalSpaceUnits_ifUnrotated - localPositionAroundWhichToDraw.x) * scaleOfLocalSpace.x;
            Vector3 drawAroundPosGlobal_to_smallestXGlobal = (-localRight_normalizedInGlobalSpace) * distance_fromDrawAroundPos_to_smallestXGridPos_inGlobalWorldUnits;
            Vector3 lineCenterGlobal_to_lineEndGlobal = theXGridLines_areAlingedAlongZ_notAlongY ? (localForward_normalizedInGlobalSpace * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits) : (localUp_normalizedInGlobalSpace * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits);
            Vector3 lineCenterGlobal_ofLowestGridPos = positionAroundWhichToDraw_global + drawAroundPosGlobal_to_smallestXGlobal;
            Vector3 lineStartGlobal_ofLowestGridPos = lineCenterGlobal_ofLowestGridPos - lineCenterGlobal_to_lineEndGlobal;
            Vector3 lineEndGlobal_ofLowestGridPos = lineCenterGlobal_ofLowestGridPos + lineCenterGlobal_to_lineEndGlobal;
            Vector3 lineStartPos_ifALineWouldBeDrawnThroughPositionAroundWhichToDraw_global = positionAroundWhichToDraw_global - lineCenterGlobal_to_lineEndGlobal;
            Vector3 vectorAlongGridLine_normalizedInGlobalSpace = theXGridLines_areAlingedAlongZ_notAlongY ? localForward_normalizedInGlobalSpace : localUp_normalizedInGlobalSpace;
            Vector3 vectorAlongPerpGrowingLineWidth_normalizedInGlobalSpace = theXGridLines_areAlingedAlongZ_notAlongY ? localUp_normalizedInGlobalSpace : localForward_normalizedInGlobalSpace;
            Vector3 vectorAlongGrowingLineWidth_normalizedInGlobalSpace = lineWidthAndText_growsInsideRepresentedGridAxis_notPerpToIt ? (-localRight_normalizedInGlobalSpace) : vectorAlongPerpGrowingLineWidth_normalizedInGlobalSpace;

            float lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits = 2.0f * extentOfWholeCascadeAlongAxis_inOrdersOwnUnits;
            int numberOfVisualizedGridPointsAlongAxis = Mathf.RoundToInt(lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits) + 1;
            numberOfVisualizedGridPointsAlongAxis = Mathf.Max(numberOfVisualizedGridPointsAlongAxis, 3);
            for (int i = 0; i < numberOfVisualizedGridPointsAlongAxis; i++)
            {
                if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
                Vector3 shiftVector = localRight_normalizedInGlobalSpace * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * i;
                Vector3 lineCenterPos = lineCenterGlobal_ofLowestGridPos + shiftVector;
                Vector3 lineStartPos = lineStartGlobal_ofLowestGridPos + shiftVector;
                Vector3 lineEndPos = lineEndGlobal_ofLowestGridPos + shiftVector;
                float distanceOfVisualizedGridPosToPosAroundWhichToDraw_0to1 = Mathf.Abs((lineStartPos - lineStartPos_ifALineWouldBeDrawnThroughPositionAroundWhichToDraw_global).magnitude / extentOfWholeCascadeAlongAxis_inWorldUnits);
                if (distanceOfVisualizedGridPosToPosAroundWhichToDraw_0to1 > 1.12f) { continue; } //-> saving some performance for planes that are anyway not visible (because their alpha reached zero)
                Color lineColor = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 1.0f - 0.9f * distanceOfVisualizedGridPosToPosAroundWhichToDraw_0to1);
                Line_fadeableAnimSpeed.InternalDraw(lineStartPos, lineEndPos, lineColor, linesWidth_inGlobalUnits, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, vectorAlongGrowingLineWidth_normalizedInGlobalSpace, true, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                bool drawCoordsTextAtLeastOnce = (distanceBetweenRepeatingCoordsTexts_relToGridDistance >= 0.0f);
                if (drawCoordsTextAtLeastOnce)
                {
                    float localXPos_ofCurrGridPos_inLocalSpaceUnits = localXPos_ofLowestGridPos_inLocalSpaceUnits_ifUnrotated + distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder * i;
                    float coordsWithFixedRoundingError = (Mathf.Round(localXPos_ofCurrGridPos_inLocalSpaceUnits / distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder) * distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder); //-> fixing floatCalculationImprecisionErrors (like "0.099999999" -> "0.1")
                    string coordsAsText = GetCoordsAsText_forLocalXDimLines(coordsWithFixedRoundingError, lineColor);
                    Vector3 textPos_ifTextIsInsideRepresentedGridAxis = theXGridLines_areAlingedAlongZ_notAlongY ? (lineCenterPos - localRight_normalizedInGlobalSpace * spaceBetweenLineAndCoordsText_inGlobalUnits + localForward_normalizedInGlobalSpace * textPosOffset) : (lineCenterPos - localRight_normalizedInGlobalSpace * spaceBetweenLineAndCoordsText_inGlobalUnits + localUp_normalizedInGlobalSpace * textPosOffset);
                    Vector3 textPos_ifTextIsPerpToRepresentedGridAxis = theXGridLines_areAlingedAlongZ_notAlongY ? (lineCenterPos + localUp_normalizedInGlobalSpace * spaceBetweenLineAndCoordsText_inGlobalUnits + localForward_normalizedInGlobalSpace * textPosOffset) : (lineCenterPos + localForward_normalizedInGlobalSpace * spaceBetweenLineAndCoordsText_inGlobalUnits + localUp_normalizedInGlobalSpace * textPosOffset);
                    Vector3 textPos = lineWidthAndText_growsInsideRepresentedGridAxis_notPerpToIt ? textPos_ifTextIsInsideRepresentedGridAxis : textPos_ifTextIsPerpToRepresentedGridAxis;
                    UtilitiesDXXL_Text.Write(coordsAsText, textPos, lineColor, textSize, vectorAlongGridLine_normalizedInGlobalSpace, vectorAlongGrowingLineWidth_normalizedInGlobalSpace, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                    DrawAdditionalCoordsTexts(vectorAlongGridLine_normalizedInGlobalSpace, vectorAlongGrowingLineWidth_normalizedInGlobalSpace, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, distanceBetweenRepeatingCoordsTexts_relToGridDistance, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, lineColor, textPos, coordsAsText, textSize, durationInSec, hiddenByNearerObjects);
                }
            }

            TryDrawDistanceLines_fromDrawAroundPos_toNeighboringGridValue(forGridLINES_notForGridPLANES, scaleOfLocalSpace.x, localRight_normalizedInGlobalSpace, vectorAlongGridLine_normalizedInGlobalSpace, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, positionAroundWhichToDraw_global, localPositionAroundWhichToDraw_expressedInUnitsOfThisOrder_x, color, durationInSec, hiddenByNearerObjects);
        }

        static string GetCoordsAsText_forLocalXDimLines(float coordsWithFixedRoundingError, Color lineColor)
        {

            if (DrawEngineBasics.skipLocalPrefix_inCoordinateTextsOnGridAxes)
            {
                if (DrawEngineBasics.skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes)
                {
                    return ("" + coordsWithFixedRoundingError);
                }
                else
                {
                    return ("x = " + coordsWithFixedRoundingError);
                }
            }
            else
            {
                if (DrawEngineBasics.skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes)
                {
                    return ("<color=#" + ColorUtility.ToHtmlStringRGBA(UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(lineColor, 0.4f)) + "><size=2>local</size></color>" + coordsWithFixedRoundingError);
                }
                else
                {
                    return ("<color=#" + ColorUtility.ToHtmlStringRGBA(UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(lineColor, 0.4f)) + "><size=2>local</size></color>x = " + coordsWithFixedRoundingError);
                }
            }
        }

        static void DrawYDimLinesOfGridLocal(bool forGridLINES_notForGridPLANES, Vector3 positionAroundWhichToDraw_global, Vector3 localForward_normalizedInGlobalSpace, Vector3 localUp_normalizedInGlobalSpace, Vector3 localRight_normalizedInGlobalSpace, Vector3 scaleOfLocalSpace, Vector3 localPositionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, float linesWidth_inLocalSpaceUnits, float durationInSec, bool hiddenByNearerObjects, Color color, float distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, bool theYGridLines_areAlingedAlongZ_notAlongX, float distanceBetweenRepeatingCoordsTexts_relToGridDistance)
        {
            float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder = distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder * scaleOfLocalSpace.y;
            float extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits = extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float extentOfWholeCascadeAlongAxis_inWorldUnits = extentOfWholeCascadeAlongAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float textPosOffset = DrawEngineBasics.offsetForCoordinateTextDisplays_inGrids * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float textSize = distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * DrawEngineBasics.SizeScalingForCoordinateTexts_inGrids;
            float linesWidth_inGlobalUnits = linesWidth_inLocalSpaceUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float absHalfLinesWidth_inGlobalUnits = Mathf.Abs(0.5f * linesWidth_inGlobalUnits);
            float spaceBetweenLineAndCoordsText_inGlobalUnits = absHalfLinesWidth_inGlobalUnits + rel_spaceBetweenLineAndCoordsText * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;

            float localPositionAroundWhichToDraw_expressedInUnitsOfThisOrder_y = localPositionAroundWhichToDraw.y / distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder;
            float localYPos_ofLowestGridPos_inLocalSpaceUnits_ifUnrotated = Mathf.Round(localPositionAroundWhichToDraw_expressedInUnitsOfThisOrder_y - extentOfWholeCascadeAlongAxis_inOrdersOwnUnits) * distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder;

            bool lineWidthAndText_growsInsideRepresentedGridAxis_notPerpToIt = (linesWidth_inLocalSpaceUnits >= 0.0f);
            float distance_fromDrawAroundPos_to_smallestYGridPos_inGlobalWorldUnits = Mathf.Abs(localYPos_ofLowestGridPos_inLocalSpaceUnits_ifUnrotated - localPositionAroundWhichToDraw.y) * scaleOfLocalSpace.y;
            Vector3 drawAroundPosGlobal_to_smallestYGlobal = (-localUp_normalizedInGlobalSpace) * distance_fromDrawAroundPos_to_smallestYGridPos_inGlobalWorldUnits;
            Vector3 lineCenterGlobal_to_lineEndGlobal = theYGridLines_areAlingedAlongZ_notAlongX ? (localForward_normalizedInGlobalSpace * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits) : (localRight_normalizedInGlobalSpace * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits);
            Vector3 lineCenterGlobal_ofLowestGridPos = positionAroundWhichToDraw_global + drawAroundPosGlobal_to_smallestYGlobal;
            Vector3 lineStartGlobal_ofLowestGridPos = lineCenterGlobal_ofLowestGridPos - lineCenterGlobal_to_lineEndGlobal;
            Vector3 lineEndGlobal_ofLowestGridPos = lineCenterGlobal_ofLowestGridPos + lineCenterGlobal_to_lineEndGlobal;
            Vector3 lineStartPos_ifALineWouldBeDrawnThroughPositionAroundWhichToDraw_global = positionAroundWhichToDraw_global - lineCenterGlobal_to_lineEndGlobal;
            Vector3 vectorAlongGridLine_normalizedInGlobalSpace = theYGridLines_areAlingedAlongZ_notAlongX ? localForward_normalizedInGlobalSpace : localRight_normalizedInGlobalSpace;
            Vector3 vectorAlongPerpGrowingLineWidth_normalizedInGlobalSpace = theYGridLines_areAlingedAlongZ_notAlongX ? (-localRight_normalizedInGlobalSpace) : localForward_normalizedInGlobalSpace;
            Vector3 vectorAlongGrowingLineWidth_normalizedInGlobalSpace = lineWidthAndText_growsInsideRepresentedGridAxis_notPerpToIt ? localUp_normalizedInGlobalSpace : vectorAlongPerpGrowingLineWidth_normalizedInGlobalSpace;

            float lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits = 2.0f * extentOfWholeCascadeAlongAxis_inOrdersOwnUnits;
            int numberOfVisualizedGridPointsAlongAxis = Mathf.RoundToInt(lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits) + 1;
            numberOfVisualizedGridPointsAlongAxis = Mathf.Max(numberOfVisualizedGridPointsAlongAxis, 3);
            for (int i = 0; i < numberOfVisualizedGridPointsAlongAxis; i++)
            {
                if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
                Vector3 shiftVector = localUp_normalizedInGlobalSpace * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * i;
                Vector3 lineCenterPos = lineCenterGlobal_ofLowestGridPos + shiftVector;
                Vector3 lineStartPos = lineStartGlobal_ofLowestGridPos + shiftVector;
                Vector3 lineEndPos = lineEndGlobal_ofLowestGridPos + shiftVector;
                float distanceOfVisualizedGridPosToPosAroundWhichToDraw_0to1 = Mathf.Abs((lineStartPos - lineStartPos_ifALineWouldBeDrawnThroughPositionAroundWhichToDraw_global).magnitude / extentOfWholeCascadeAlongAxis_inWorldUnits);
                if (distanceOfVisualizedGridPosToPosAroundWhichToDraw_0to1 > 1.12f) { continue; } //-> saving some performance for planes that are anyway not visible (because their alpha reached zero)
                Color lineColor = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 1.0f - 0.9f * distanceOfVisualizedGridPosToPosAroundWhichToDraw_0to1);
                Line_fadeableAnimSpeed.InternalDraw(lineStartPos, lineEndPos, lineColor, linesWidth_inGlobalUnits, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, vectorAlongGrowingLineWidth_normalizedInGlobalSpace, true, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                bool drawCoordsTextAtLeastOnce = (distanceBetweenRepeatingCoordsTexts_relToGridDistance >= 0.0f);
                if (drawCoordsTextAtLeastOnce)
                {
                    float localYPos_ofCurrGridPos_inLocalSpaceUnits = localYPos_ofLowestGridPos_inLocalSpaceUnits_ifUnrotated + distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder * i;
                    float coordsWithFixedRoundingError = (Mathf.Round(localYPos_ofCurrGridPos_inLocalSpaceUnits / distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder) * distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder); //-> fixing floatCalculationImprecisionErrors (like "0.099999999" -> "0.1")
                    string coordsAsText = GetCoordsAsText_forLocalYDimLines(coordsWithFixedRoundingError, lineColor);
                    Vector3 textPos_ifTextIsInsideRepresentedGridAxis = theYGridLines_areAlingedAlongZ_notAlongX ? (lineCenterPos + localUp_normalizedInGlobalSpace * spaceBetweenLineAndCoordsText_inGlobalUnits + localForward_normalizedInGlobalSpace * textPosOffset) : (lineCenterPos + localUp_normalizedInGlobalSpace * spaceBetweenLineAndCoordsText_inGlobalUnits + localRight_normalizedInGlobalSpace * textPosOffset);
                    Vector3 textPos_ifTextIsPerpToRepresentedGridAxis = theYGridLines_areAlingedAlongZ_notAlongX ? (lineCenterPos - localRight_normalizedInGlobalSpace * spaceBetweenLineAndCoordsText_inGlobalUnits + localForward_normalizedInGlobalSpace * textPosOffset) : (lineCenterPos + localForward_normalizedInGlobalSpace * spaceBetweenLineAndCoordsText_inGlobalUnits + localRight_normalizedInGlobalSpace * textPosOffset);
                    Vector3 textPos = lineWidthAndText_growsInsideRepresentedGridAxis_notPerpToIt ? textPos_ifTextIsInsideRepresentedGridAxis : textPos_ifTextIsPerpToRepresentedGridAxis;
                    UtilitiesDXXL_Text.Write(coordsAsText, textPos, lineColor, textSize, vectorAlongGridLine_normalizedInGlobalSpace, vectorAlongGrowingLineWidth_normalizedInGlobalSpace, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                    DrawAdditionalCoordsTexts(vectorAlongGridLine_normalizedInGlobalSpace, vectorAlongGrowingLineWidth_normalizedInGlobalSpace, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, distanceBetweenRepeatingCoordsTexts_relToGridDistance, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, lineColor, textPos, coordsAsText, textSize, durationInSec, hiddenByNearerObjects);
                }
            }

            TryDrawDistanceLines_fromDrawAroundPos_toNeighboringGridValue(forGridLINES_notForGridPLANES, scaleOfLocalSpace.y, localUp_normalizedInGlobalSpace, vectorAlongGridLine_normalizedInGlobalSpace, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, positionAroundWhichToDraw_global, localPositionAroundWhichToDraw_expressedInUnitsOfThisOrder_y, color, durationInSec, hiddenByNearerObjects);
        }

        static string GetCoordsAsText_forLocalYDimLines(float coordsWithFixedRoundingError, Color lineColor)
        {
            if (DrawEngineBasics.skipLocalPrefix_inCoordinateTextsOnGridAxes)
            {
                if (DrawEngineBasics.skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes)
                {
                    return ("" + coordsWithFixedRoundingError);
                }
                else
                {
                    return ("y = " + coordsWithFixedRoundingError);
                }
            }
            else
            {
                if (DrawEngineBasics.skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes)
                {
                    return ("<color=#" + ColorUtility.ToHtmlStringRGBA(UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(lineColor, 0.4f)) + "><size=2>local</size></color>" + coordsWithFixedRoundingError);
                }
                else
                {
                    return ("<color=#" + ColorUtility.ToHtmlStringRGBA(UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(lineColor, 0.4f)) + "><size=2>local</size></color>y = " + coordsWithFixedRoundingError);
                }
            }
        }

        static void DrawZDimLinesOfGridLocal(bool forGridLINES_notForGridPLANES, Vector3 positionAroundWhichToDraw_global, Vector3 localForward_normalizedInGlobalSpace, Vector3 localUp_normalizedInGlobalSpace, Vector3 localRight_normalizedInGlobalSpace, Vector3 scaleOfLocalSpace, Vector3 localPositionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, float linesWidth_inLocalSpaceUnits, float durationInSec, bool hiddenByNearerObjects, Color color, float distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, bool theZGridLines_areAlingedAlongX_notAlongY, float distanceBetweenRepeatingCoordsTexts_relToGridDistance)
        {
            float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder = distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder * scaleOfLocalSpace.z;
            float extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits = extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float extentOfWholeCascadeAlongAxis_inWorldUnits = extentOfWholeCascadeAlongAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float textPosOffset = DrawEngineBasics.offsetForCoordinateTextDisplays_inGrids * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float textSize = distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * DrawEngineBasics.SizeScalingForCoordinateTexts_inGrids;
            float linesWidth_inGlobalUnits = linesWidth_inLocalSpaceUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float absHalfLinesWidth_inGlobalUnits = Mathf.Abs(0.5f * linesWidth_inGlobalUnits);
            float spaceBetweenLineAndCoordsText_inGlobalUnits = absHalfLinesWidth_inGlobalUnits + rel_spaceBetweenLineAndCoordsText * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;

            float localPositionAroundWhichToDraw_expressedInUnitsOfThisOrder_z = localPositionAroundWhichToDraw.z / distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder;
            float localZPos_ofLowestGridPos_inLocalSpaceUnits_ifUnrotated = Mathf.Round(localPositionAroundWhichToDraw_expressedInUnitsOfThisOrder_z - extentOfWholeCascadeAlongAxis_inOrdersOwnUnits) * distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder;

            bool lineWidthAndText_growsInsideRepresentedGridAxis_notPerpToIt = (linesWidth_inLocalSpaceUnits >= 0.0f);
            float distance_fromDrawAroundPos_to_smallestZGridPos_inGlobalWorldUnits = Mathf.Abs(localZPos_ofLowestGridPos_inLocalSpaceUnits_ifUnrotated - localPositionAroundWhichToDraw.z) * scaleOfLocalSpace.z;
            Vector3 drawAroundPosGlobal_to_smallestZGlobal = (-localForward_normalizedInGlobalSpace) * distance_fromDrawAroundPos_to_smallestZGridPos_inGlobalWorldUnits;
            Vector3 lineCenterGlobal_to_lineEndGlobal = theZGridLines_areAlingedAlongX_notAlongY ? (localRight_normalizedInGlobalSpace * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits) : (localUp_normalizedInGlobalSpace * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits);
            Vector3 lineCenterGlobal_ofLowestGridPos = positionAroundWhichToDraw_global + drawAroundPosGlobal_to_smallestZGlobal;
            Vector3 lineStartGlobal_ofLowestGridPos = lineCenterGlobal_ofLowestGridPos - lineCenterGlobal_to_lineEndGlobal;
            Vector3 lineEndGlobal_ofLowestGridPos = lineCenterGlobal_ofLowestGridPos + lineCenterGlobal_to_lineEndGlobal;
            Vector3 lineStartPos_ifALineWouldBeDrawnThroughPositionAroundWhichToDraw_global = positionAroundWhichToDraw_global - lineCenterGlobal_to_lineEndGlobal;
            Vector3 vectorAlongGridLine_normalizedInGlobalSpace = theZGridLines_areAlingedAlongX_notAlongY ? localRight_normalizedInGlobalSpace : localUp_normalizedInGlobalSpace;
            Vector3 vectorAlongPerpGrowingLineWidth_normalizedInGlobalSpace = theZGridLines_areAlingedAlongX_notAlongY ? localUp_normalizedInGlobalSpace : (-localRight_normalizedInGlobalSpace);
            Vector3 textUp_normalizedInGlobalSpace_ifTextIsInsideRepresentedGridAxis = theZGridLines_areAlingedAlongX_notAlongY ? localForward_normalizedInGlobalSpace : (-localForward_normalizedInGlobalSpace);
            Vector3 vectorAlongGrowingLineWidth_normalizedInGlobalSpace = lineWidthAndText_growsInsideRepresentedGridAxis_notPerpToIt ? textUp_normalizedInGlobalSpace_ifTextIsInsideRepresentedGridAxis : vectorAlongPerpGrowingLineWidth_normalizedInGlobalSpace;

            float lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits = 2.0f * extentOfWholeCascadeAlongAxis_inOrdersOwnUnits;
            int numberOfVisualizedGridPointsAlongAxis = Mathf.RoundToInt(lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits) + 1;
            numberOfVisualizedGridPointsAlongAxis = Mathf.Max(numberOfVisualizedGridPointsAlongAxis, 3);
            for (int i = 0; i < numberOfVisualizedGridPointsAlongAxis; i++)
            {
                if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
                Vector3 shiftVector = localForward_normalizedInGlobalSpace * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * i;
                Vector3 lineCenterPos = lineCenterGlobal_ofLowestGridPos + shiftVector;
                Vector3 lineStartPos = lineStartGlobal_ofLowestGridPos + shiftVector;
                Vector3 lineEndPos = lineEndGlobal_ofLowestGridPos + shiftVector;
                float distanceOfVisualizedGridPosToPosAroundWhichToDraw_0to1 = Mathf.Abs((lineStartPos - lineStartPos_ifALineWouldBeDrawnThroughPositionAroundWhichToDraw_global).magnitude / extentOfWholeCascadeAlongAxis_inWorldUnits);
                if (distanceOfVisualizedGridPosToPosAroundWhichToDraw_0to1 > 1.12f) { continue; } //-> saving some performance for planes that are anyway not visible (because their alpha reached zero)
                Color lineColor = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 1.0f - 0.9f * distanceOfVisualizedGridPosToPosAroundWhichToDraw_0to1);
                Line_fadeableAnimSpeed.InternalDraw(lineStartPos, lineEndPos, lineColor, linesWidth_inGlobalUnits, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, vectorAlongGrowingLineWidth_normalizedInGlobalSpace, true, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                bool drawCoordsTextAtLeastOnce = (distanceBetweenRepeatingCoordsTexts_relToGridDistance >= 0.0f);
                if (drawCoordsTextAtLeastOnce)
                {
                    float localZPos_ofCurrGridPos_inLocalSpaceUnits = localZPos_ofLowestGridPos_inLocalSpaceUnits_ifUnrotated + distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder * i;
                    float coordsWithFixedRoundingError = (Mathf.Round(localZPos_ofCurrGridPos_inLocalSpaceUnits / distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder) * distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder); //-> fixing floatCalculationImprecisionErrors (like "0.099999999" -> "0.1")
                    string coordsAsText = GetCoordsAsText_forLocalZDimLines(coordsWithFixedRoundingError,  lineColor);
                    Vector3 textPos_ifTextIsInsideRepresentedGridAxis = theZGridLines_areAlingedAlongX_notAlongY ? (lineCenterPos + localForward_normalizedInGlobalSpace * spaceBetweenLineAndCoordsText_inGlobalUnits + localRight_normalizedInGlobalSpace * textPosOffset) : (lineCenterPos - localForward_normalizedInGlobalSpace * spaceBetweenLineAndCoordsText_inGlobalUnits + localUp_normalizedInGlobalSpace * textPosOffset);
                    Vector3 textPos_ifTextIsPerpToRepresentedGridAxis = theZGridLines_areAlingedAlongX_notAlongY ? (lineCenterPos + localUp_normalizedInGlobalSpace * spaceBetweenLineAndCoordsText_inGlobalUnits + localRight_normalizedInGlobalSpace * textPosOffset) : (lineCenterPos - localRight_normalizedInGlobalSpace * spaceBetweenLineAndCoordsText_inGlobalUnits + localUp_normalizedInGlobalSpace * textPosOffset);
                    Vector3 textPos = lineWidthAndText_growsInsideRepresentedGridAxis_notPerpToIt ? textPos_ifTextIsInsideRepresentedGridAxis : textPos_ifTextIsPerpToRepresentedGridAxis;
                    UtilitiesDXXL_Text.Write(coordsAsText, textPos, lineColor, textSize, vectorAlongGridLine_normalizedInGlobalSpace, vectorAlongGrowingLineWidth_normalizedInGlobalSpace, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                    DrawAdditionalCoordsTexts(vectorAlongGridLine_normalizedInGlobalSpace, vectorAlongGrowingLineWidth_normalizedInGlobalSpace, extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, distanceBetweenRepeatingCoordsTexts_relToGridDistance, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, lineColor, textPos, coordsAsText, textSize, durationInSec, hiddenByNearerObjects);
                }
            }

            TryDrawDistanceLines_fromDrawAroundPos_toNeighboringGridValue(forGridLINES_notForGridPLANES, scaleOfLocalSpace.z, localForward_normalizedInGlobalSpace, vectorAlongGridLine_normalizedInGlobalSpace, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, positionAroundWhichToDraw_global, localPositionAroundWhichToDraw_expressedInUnitsOfThisOrder_z, color, durationInSec, hiddenByNearerObjects);
        }

        static string GetCoordsAsText_forLocalZDimLines(float coordsWithFixedRoundingError, Color lineColor)
        {
            if (DrawEngineBasics.skipLocalPrefix_inCoordinateTextsOnGridAxes)
            {
                if (DrawEngineBasics.skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes)
                {
                    return ("" + coordsWithFixedRoundingError);
                }
                else
                {
                    return ("z = " + coordsWithFixedRoundingError);
                }
            }
            else
            {
                if (DrawEngineBasics.skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes)
                {
                    return ("<color=#" + ColorUtility.ToHtmlStringRGBA(UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(lineColor, 0.4f)) + "><size=2>local</size></color>" + coordsWithFixedRoundingError);
                }
                else
                {
                    return ("<color=#" + ColorUtility.ToHtmlStringRGBA(UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(lineColor, 0.4f)) + "><size=2>local</size></color>z = " + coordsWithFixedRoundingError);
                }
            }
        }

        static void DrawAdditionalCoordsTexts(Vector3 vectorAlongGridLine_normalized, Vector3 textUp, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float distanceBetweenRepeatingCoordsTexts_relToGridDistance, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, Color textColor, Vector3 unshiftedTextPos, string coordsAsText, float textSize, float durationInSec, bool hiddenByNearerObjects)
        {
            bool drawAdditionalCoordsTexts = (UtilitiesDXXL_Math.ApproximatelyZero(distanceBetweenRepeatingCoordsTexts_relToGridDistance) == false); //-> negative values of "distanceBetweenRepeatingCoordsTexts_relToGridDistance" don't even arrive here
            if (drawAdditionalCoordsTexts)
            {
                distanceBetweenRepeatingCoordsTexts_relToGridDistance = Mathf.Max(distanceBetweenRepeatingCoordsTexts_relToGridDistance, min_distanceBetweenRepeatingCoordsTexts_relToGridDistance); //-> setting "distanceBetweenRepeatingCoordsTexts_relToGridDistance" to lower values can cause massive performance hit/editor freeze
                if (extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits > distanceBetweenRepeatingCoordsTexts_relToGridDistance)
                {
                    int numberOfAdditinalCoordsTexts = Mathf.FloorToInt(extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits / distanceBetweenRepeatingCoordsTexts_relToGridDistance);
                    numberOfAdditinalCoordsTexts = Mathf.Max(numberOfAdditinalCoordsTexts, 1);
                    for (int i_additionalCoordsText = 0; i_additionalCoordsText < numberOfAdditinalCoordsTexts; i_additionalCoordsText++)
                    {
                        Vector3 shiftVectorForward = vectorAlongGridLine_normalized * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * distanceBetweenRepeatingCoordsTexts_relToGridDistance * (1 + i_additionalCoordsText);
                        Vector3 posOfLowerText = unshiftedTextPos + shiftVectorForward;
                        Vector3 posOfHigherText = unshiftedTextPos - shiftVectorForward;
                        UtilitiesDXXL_Text.Write(coordsAsText, posOfLowerText, textColor, textSize, vectorAlongGridLine_normalized, textUp, DrawText.TextAnchorDXXL.LowerCenterOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                        UtilitiesDXXL_Text.Write(coordsAsText, posOfHigherText, textColor, textSize, vectorAlongGridLine_normalized, textUp, DrawText.TextAnchorDXXL.LowerCenterOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                    }
                }
            }
        }

        static void DrawXDimPlanesDenseWithoutText(Vector3 positionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, float drawDensity, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, float durationInSec, bool hiddenByNearerObjects, Color color, bool turned90DegAroundHisAxis)
        {
            float extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits = extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float extentOfWholeCascadeAlongAxis_inWorldUnits = extentOfWholeCascadeAlongAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float lengthOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits = 2.0f * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits;
            float distanceBeweenLinesInsideGridSlice_inWorldUnits = gridDensityDefaultScaleFactor * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder / drawDensity;
            int numberOfLinesPerGridSlice = Mathf.RoundToInt(lengthOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits / distanceBeweenLinesInsideGridSlice_inWorldUnits);
            numberOfLinesPerGridSlice = Mathf.Max(numberOfLinesPerGridSlice, 3);
            float halfNumberOfLinesPerGridSlice = 0.5f * numberOfLinesPerGridSlice;

            float positionAroundWhichToDraw_expressedInUnitsOfThisOrder_x = positionAroundWhichToDraw.x / distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float xPos_ofLowestGridSlice = Mathf.Round(positionAroundWhichToDraw_expressedInUnitsOfThisOrder_x - extentOfWholeCascadeAlongAxis_inOrdersOwnUnits) * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            Vector3 startPosGlobal_ofMainLineOfLowestGridSlice;
            Vector3 endPosGlobal_ofMainLineOfLowestGridSlice;
            Vector3 vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized;
            if (turned90DegAroundHisAxis)
            {
                startPosGlobal_ofMainLineOfLowestGridSlice = new Vector3(xPos_ofLowestGridSlice, positionAroundWhichToDraw.y, positionAroundWhichToDraw.z - extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits);
                endPosGlobal_ofMainLineOfLowestGridSlice = new Vector3(xPos_ofLowestGridSlice, positionAroundWhichToDraw.y, positionAroundWhichToDraw.z + extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits);
                vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized = Vector3.down;
            }
            else
            {
                startPosGlobal_ofMainLineOfLowestGridSlice = new Vector3(xPos_ofLowestGridSlice, positionAroundWhichToDraw.y - extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits, positionAroundWhichToDraw.z);
                endPosGlobal_ofMainLineOfLowestGridSlice = new Vector3(xPos_ofLowestGridSlice, positionAroundWhichToDraw.y + extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits, positionAroundWhichToDraw.z);
                vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized = Vector3.back;
            }
            Vector3 vectorFromGridSlicesMainLineToLowestLineInsideSlice = vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits;

            float lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits = 2.0f * extentOfWholeCascadeAlongAxis_inOrdersOwnUnits;
            int numberOfVisualizedGridSlicesAlongAxis = Mathf.RoundToInt(lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits) + 2; //-> "+ 2" instead of "+ 1", because otherwise planes pop to invisible before reaching nearZeroAlpha. The "distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1"-earlyContinue inside the for-loop ensures that the line count doesn't rise through this measure
            numberOfVisualizedGridSlicesAlongAxis = Mathf.Max(numberOfVisualizedGridSlicesAlongAxis, 2);
            for (int i_gridSlice = 0; i_gridSlice < numberOfVisualizedGridSlicesAlongAxis; i_gridSlice++)
            {
                Vector3 lowestSlice_to_currSlice = Vector3.right * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * i_gridSlice;
                float x_ofCurrSlice = startPosGlobal_ofMainLineOfLowestGridSlice.x + lowestSlice_to_currSlice.x;
                float distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1 = Mathf.Abs((x_ofCurrSlice - positionAroundWhichToDraw.x) / extentOfWholeCascadeAlongAxis_inWorldUnits);
                if (distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1 > 1.12f) { continue; } //-> saving some performance for planes that are anyway not visible (because their alpha reached zero)
                float alphaOfCurrGridSlicesMainLine = 1.0f - 0.9f * distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1;
                Vector3 lineStartPos_ofLowestLineInsideSlice = startPosGlobal_ofMainLineOfLowestGridSlice + lowestSlice_to_currSlice + vectorFromGridSlicesMainLineToLowestLineInsideSlice;
                Vector3 lineEndPos_ofLowestLineInsideSlice = endPosGlobal_ofMainLineOfLowestGridSlice + lowestSlice_to_currSlice + vectorFromGridSlicesMainLineToLowestLineInsideSlice;

                for (int i_lineInsideSlice = 0; i_lineInsideSlice < numberOfLinesPerGridSlice; i_lineInsideSlice++)
                {
                    if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
                    Vector3 lowestLineInsideSlice_to_currLineInsideSlice = (-vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized) * distanceBeweenLinesInsideGridSlice_inWorldUnits * i_lineInsideSlice;
                    Vector3 startPos_ofLineInsideSlice = lineStartPos_ofLowestLineInsideSlice + lowestLineInsideSlice_to_currLineInsideSlice;
                    Vector3 endPos_ofLineInsideSlice = lineEndPos_ofLowestLineInsideSlice + lowestLineInsideSlice_to_currLineInsideSlice;
                    float distanceToCurrSlicesMainLine_0to1 = Mathf.Abs(((float)i_lineInsideSlice - halfNumberOfLinesPerGridSlice) / halfNumberOfLinesPerGridSlice);
                    float alphaOfLineInsideSlice = alphaOfCurrGridSlicesMainLine * (1.0f - 0.9f * distanceToCurrSlicesMainLine_0to1);
                    Color lineColor_ofLineInsideSlice = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, alphaOfLineInsideSlice);
                    Line_fadeableAnimSpeed.InternalDraw(startPos_ofLineInsideSlice, endPos_ofLineInsideSlice, lineColor_ofLineInsideSlice, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                }
            }
        }

        static void DrawYDimPlanesDenseWithoutText(Vector3 positionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, float drawDensity, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, float durationInSec, bool hiddenByNearerObjects, Color color, bool turned90DegAroundHisAxis)
        {
            float extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits = extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float extentOfWholeCascadeAlongAxis_inWorldUnits = extentOfWholeCascadeAlongAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float lengthOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits = 2.0f * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits;
            float distanceBeweenLinesInsideGridSlice_inWorldUnits = gridDensityDefaultScaleFactor * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder / drawDensity;
            int numberOfLinesPerGridSlice = Mathf.RoundToInt(lengthOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits / distanceBeweenLinesInsideGridSlice_inWorldUnits);
            numberOfLinesPerGridSlice = Mathf.Max(numberOfLinesPerGridSlice, 3);
            float halfNumberOfLinesPerGridSlice = 0.5f * numberOfLinesPerGridSlice;

            float positionAroundWhichToDraw_expressedInUnitsOfThisOrder_y = positionAroundWhichToDraw.y / distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float yPos_ofLowestGridSlice = Mathf.Round(positionAroundWhichToDraw_expressedInUnitsOfThisOrder_y - extentOfWholeCascadeAlongAxis_inOrdersOwnUnits) * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            Vector3 startPosGlobal_ofMainLineOfLowestGridSlice;
            Vector3 endPosGlobal_ofMainLineOfLowestGridSlice;
            Vector3 vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized;
            if (turned90DegAroundHisAxis)
            {
                startPosGlobal_ofMainLineOfLowestGridSlice = new Vector3(positionAroundWhichToDraw.x, yPos_ofLowestGridSlice, positionAroundWhichToDraw.z - extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits);
                endPosGlobal_ofMainLineOfLowestGridSlice = new Vector3(positionAroundWhichToDraw.x, yPos_ofLowestGridSlice, positionAroundWhichToDraw.z + extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits);
                vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized = Vector3.left;
            }
            else
            {
                startPosGlobal_ofMainLineOfLowestGridSlice = new Vector3(positionAroundWhichToDraw.x - extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits, yPos_ofLowestGridSlice, positionAroundWhichToDraw.z);
                endPosGlobal_ofMainLineOfLowestGridSlice = new Vector3(positionAroundWhichToDraw.x + extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits, yPos_ofLowestGridSlice, positionAroundWhichToDraw.z);
                vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized = Vector3.back;
            }
            Vector3 vectorFromGridSlicesMainLineToLowestLineInsideSlice = vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits;

            float lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits = 2.0f * extentOfWholeCascadeAlongAxis_inOrdersOwnUnits;
            int numberOfVisualizedGridSlicesAlongAxis = Mathf.RoundToInt(lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits) + 2; //-> "+ 2" instead of "+ 1", because otherwise planes pop to invisible before reaching nearZeroAlpha. The "distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1"-earlyContinue inside the for-loop ensures that the line count doesn't rise through this measure
            numberOfVisualizedGridSlicesAlongAxis = Mathf.Max(numberOfVisualizedGridSlicesAlongAxis, 2);
            for (int i_gridSlice = 0; i_gridSlice < numberOfVisualizedGridSlicesAlongAxis; i_gridSlice++)
            {
                Vector3 lowestSlice_to_currSlice = Vector3.up * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * i_gridSlice;
                float y_ofCurrSlice = startPosGlobal_ofMainLineOfLowestGridSlice.y + lowestSlice_to_currSlice.y;
                float distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1 = Mathf.Abs((y_ofCurrSlice - positionAroundWhichToDraw.y) / extentOfWholeCascadeAlongAxis_inWorldUnits);
                if (distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1 > 1.12f) { continue; } //-> saving some performance for planes that are anyway not visible (because their alpha reached zero)
                float alphaOfCurrGridSlicesMainLine = 1.0f - 0.9f * distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1;
                Vector3 lineStartPos_ofLowestLineInsideSlice = startPosGlobal_ofMainLineOfLowestGridSlice + lowestSlice_to_currSlice + vectorFromGridSlicesMainLineToLowestLineInsideSlice;
                Vector3 lineEndPos_ofLowestLineInsideSlice = endPosGlobal_ofMainLineOfLowestGridSlice + lowestSlice_to_currSlice + vectorFromGridSlicesMainLineToLowestLineInsideSlice;

                for (int i_lineInsideSlice = 0; i_lineInsideSlice < numberOfLinesPerGridSlice; i_lineInsideSlice++)
                {
                    if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
                    Vector3 lowestLineInsideSlice_to_currLineInsideSlice = (-vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized) * distanceBeweenLinesInsideGridSlice_inWorldUnits * i_lineInsideSlice;
                    Vector3 startPos_ofLineInsideSlice = lineStartPos_ofLowestLineInsideSlice + lowestLineInsideSlice_to_currLineInsideSlice;
                    Vector3 endPos_ofLineInsideSlice = lineEndPos_ofLowestLineInsideSlice + lowestLineInsideSlice_to_currLineInsideSlice;
                    float distanceToCurrSlicesMainLine_0to1 = Mathf.Abs(((float)i_lineInsideSlice - halfNumberOfLinesPerGridSlice) / halfNumberOfLinesPerGridSlice);
                    float alphaOfLineInsideSlice = alphaOfCurrGridSlicesMainLine * (1.0f - 0.9f * distanceToCurrSlicesMainLine_0to1);
                    Color lineColor_ofLineInsideSlice = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, alphaOfLineInsideSlice);
                    Line_fadeableAnimSpeed.InternalDraw(startPos_ofLineInsideSlice, endPos_ofLineInsideSlice, lineColor_ofLineInsideSlice, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                }
            }
        }


        static void DrawZDimPlanesDenseWithoutText(Vector3 positionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, float drawDensity, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, float durationInSec, bool hiddenByNearerObjects, Color color, bool turned90DegAroundHisAxis)
        {
            float extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits = extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float extentOfWholeCascadeAlongAxis_inWorldUnits = extentOfWholeCascadeAlongAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float lengthOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits = 2.0f * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits;
            float distanceBeweenLinesInsideGridSlice_inWorldUnits = gridDensityDefaultScaleFactor * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder / drawDensity;
            int numberOfLinesPerGridSlice = Mathf.RoundToInt(lengthOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits / distanceBeweenLinesInsideGridSlice_inWorldUnits);
            numberOfLinesPerGridSlice = Mathf.Max(numberOfLinesPerGridSlice, 3);
            float halfNumberOfLinesPerGridSlice = 0.5f * numberOfLinesPerGridSlice;

            float positionAroundWhichToDraw_expressedInUnitsOfThisOrder_z = positionAroundWhichToDraw.z / distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float zPos_ofLowestGridSlice = Mathf.Round(positionAroundWhichToDraw_expressedInUnitsOfThisOrder_z - extentOfWholeCascadeAlongAxis_inOrdersOwnUnits) * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            Vector3 startPosGlobal_ofMainLineOfLowestGridSlice;
            Vector3 endPosGlobal_ofMainLineOfLowestGridSlice;
            Vector3 vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized;
            if (turned90DegAroundHisAxis)
            {
                startPosGlobal_ofMainLineOfLowestGridSlice = new Vector3(positionAroundWhichToDraw.x - extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits, positionAroundWhichToDraw.y, zPos_ofLowestGridSlice);
                endPosGlobal_ofMainLineOfLowestGridSlice = new Vector3(positionAroundWhichToDraw.x + extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits, positionAroundWhichToDraw.y, zPos_ofLowestGridSlice);
                vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized = Vector3.down;
            }
            else
            {
                startPosGlobal_ofMainLineOfLowestGridSlice = new Vector3(positionAroundWhichToDraw.x, positionAroundWhichToDraw.y - extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits, zPos_ofLowestGridSlice);
                endPosGlobal_ofMainLineOfLowestGridSlice = new Vector3(positionAroundWhichToDraw.x, positionAroundWhichToDraw.y + extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits, zPos_ofLowestGridSlice);
                vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized = Vector3.left;
            }
            Vector3 vectorFromGridSlicesMainLineToLowestLineInsideSlice = vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits;

            float lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits = 2.0f * extentOfWholeCascadeAlongAxis_inOrdersOwnUnits;
            int numberOfVisualizedGridSlicesAlongAxis = Mathf.RoundToInt(lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits) + 2; //-> "+ 2" instead of "+ 1", because otherwise planes pop to invisible before reaching nearZeroAlpha. The "distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1"-earlyContinue inside the for-loop ensures that the line count doesn't rise through this measure
            numberOfVisualizedGridSlicesAlongAxis = Mathf.Max(numberOfVisualizedGridSlicesAlongAxis, 2);
            for (int i_gridSlice = 0; i_gridSlice < numberOfVisualizedGridSlicesAlongAxis; i_gridSlice++)
            {
                Vector3 lowestSlice_to_currSlice = Vector3.forward * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * i_gridSlice;
                float z_ofCurrSlice = startPosGlobal_ofMainLineOfLowestGridSlice.z + lowestSlice_to_currSlice.z;
                float distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1 = Mathf.Abs((z_ofCurrSlice - positionAroundWhichToDraw.z) / extentOfWholeCascadeAlongAxis_inWorldUnits);
                if (distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1 > 1.12f) { continue; } //-> saving some performance for planes that are anyway not visible (because their alpha reached zero)
                float alphaOfCurrGridSlicesMainLine = 1.0f - 0.9f * distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1;
                Vector3 lineStartPos_ofLowestLineInsideSlice = startPosGlobal_ofMainLineOfLowestGridSlice + lowestSlice_to_currSlice + vectorFromGridSlicesMainLineToLowestLineInsideSlice;
                Vector3 lineEndPos_ofLowestLineInsideSlice = endPosGlobal_ofMainLineOfLowestGridSlice + lowestSlice_to_currSlice + vectorFromGridSlicesMainLineToLowestLineInsideSlice;

                for (int i_lineInsideSlice = 0; i_lineInsideSlice < numberOfLinesPerGridSlice; i_lineInsideSlice++)
                {
                    if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
                    Vector3 lowestLineInsideSlice_to_currLineInsideSlice = (-vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized) * distanceBeweenLinesInsideGridSlice_inWorldUnits * i_lineInsideSlice;
                    Vector3 startPos_ofLineInsideSlice = lineStartPos_ofLowestLineInsideSlice + lowestLineInsideSlice_to_currLineInsideSlice;
                    Vector3 endPos_ofLineInsideSlice = lineEndPos_ofLowestLineInsideSlice + lowestLineInsideSlice_to_currLineInsideSlice;
                    float distanceToCurrSlicesMainLine_0to1 = Mathf.Abs(((float)i_lineInsideSlice - halfNumberOfLinesPerGridSlice) / halfNumberOfLinesPerGridSlice);
                    float alphaOfLineInsideSlice = alphaOfCurrGridSlicesMainLine * (1.0f - 0.9f * distanceToCurrSlicesMainLine_0to1);
                    Color lineColor_ofLineInsideSlice = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, alphaOfLineInsideSlice);
                    Line_fadeableAnimSpeed.InternalDraw(startPos_ofLineInsideSlice, endPos_ofLineInsideSlice, lineColor_ofLineInsideSlice, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                }
            }
        }

        static void DrawXDimPlanesDenseWithoutTextLocal(Vector3 positionAroundWhichToDraw_global, Vector3 localForward_normalizedInGlobalSpace, Vector3 localUp_normalizedInGlobalSpace, Vector3 localRight_normalizedInGlobalSpace, Vector3 scaleOfLocalSpace, Vector3 localPositionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, float drawDensity, float distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, float durationInSec, bool hiddenByNearerObjects, Color color, bool turned90DegAroundHisAxis)
        {
            float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder = distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder * scaleOfLocalSpace.x;
            float extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits = extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float extentOfWholeCascadeAlongAxis_inWorldUnits = extentOfWholeCascadeAlongAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float lengthOfDrawnLinesAtEachGridSlicePos_inWorldUnits = 2.0f * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits;
            float distanceBeweenLinesInsideGridSlice_inWorldUnits = gridDensityDefaultScaleFactor * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder / drawDensity;
            int numberOfLinesPerGridSlice = Mathf.RoundToInt(lengthOfDrawnLinesAtEachGridSlicePos_inWorldUnits / distanceBeweenLinesInsideGridSlice_inWorldUnits);
            numberOfLinesPerGridSlice = Mathf.Max(numberOfLinesPerGridSlice, 3);
            float halfNumberOfLinesPerGridSlice = 0.5f * numberOfLinesPerGridSlice;

            float localPositionAroundWhichToDraw_expressedInUnitsOfThisOrder_x = localPositionAroundWhichToDraw.x / distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder;
            float localXPos_ofLowestGridPos_inLocalSpaceUnits_ifUnrotated = Mathf.Round(localPositionAroundWhichToDraw_expressedInUnitsOfThisOrder_x - extentOfWholeCascadeAlongAxis_inOrdersOwnUnits) * distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder;

            float distance_fromDrawAroundPos_to_smallestXGridPos_inGlobalWorldUnits = Mathf.Abs(localXPos_ofLowestGridPos_inLocalSpaceUnits_ifUnrotated - localPositionAroundWhichToDraw.x) * scaleOfLocalSpace.x;
            Vector3 drawAroundPosGlobal_to_smallestXGlobal = (-localRight_normalizedInGlobalSpace) * distance_fromDrawAroundPos_to_smallestXGridPos_inGlobalWorldUnits;
            Vector3 lineCenterGlobal_to_lineEndGlobal = turned90DegAroundHisAxis ? (localForward_normalizedInGlobalSpace * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits) : (localUp_normalizedInGlobalSpace * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits);
            Vector3 lineCenterGlobal_ofLowestGridSlice = positionAroundWhichToDraw_global + drawAroundPosGlobal_to_smallestXGlobal;
            Vector3 startPosGlobal_ofMainLineOfLowestGridSlice = lineCenterGlobal_ofLowestGridSlice - lineCenterGlobal_to_lineEndGlobal;
            Vector3 endPosGlobal_ofMainLineOfLowestGridSlice = lineCenterGlobal_ofLowestGridSlice + lineCenterGlobal_to_lineEndGlobal;
            Vector3 lineStartPos_ifALineWouldBeDrawnThroughPositionAroundWhichToDraw_global = positionAroundWhichToDraw_global - lineCenterGlobal_to_lineEndGlobal;
            Vector3 vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized = turned90DegAroundHisAxis ? (-localUp_normalizedInGlobalSpace) : (-localForward_normalizedInGlobalSpace);
            Vector3 vectorFromGridSlicesMainLineToLowestLineInsideSlice = vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits;

            float lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits = 2.0f * extentOfWholeCascadeAlongAxis_inOrdersOwnUnits;
            int numberOfVisualizedGridSlicesAlongAxis = Mathf.RoundToInt(lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits) + 2; //-> "+ 2" instead of "+ 1", because otherwise planes pop to invisible before reaching nearZeroAlpha. The "distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1"-earlyContinue inside the for-loop ensures that the line count doesn't rise through this measure
            numberOfVisualizedGridSlicesAlongAxis = Mathf.Max(numberOfVisualizedGridSlicesAlongAxis, 2);
            for (int i_gridSlice = 0; i_gridSlice < numberOfVisualizedGridSlicesAlongAxis; i_gridSlice++)
            {
                Vector3 lowestSlice_to_currSlice = localRight_normalizedInGlobalSpace * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * i_gridSlice;
                Vector3 lineStartPos_ofMainLineInsideSlice = startPosGlobal_ofMainLineOfLowestGridSlice + lowestSlice_to_currSlice;
                Vector3 lineEndPos_ofMainLineInsideSlice = endPosGlobal_ofMainLineOfLowestGridSlice + lowestSlice_to_currSlice;
                float distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1 = Mathf.Abs((lineStartPos_ofMainLineInsideSlice - lineStartPos_ifALineWouldBeDrawnThroughPositionAroundWhichToDraw_global).magnitude / extentOfWholeCascadeAlongAxis_inWorldUnits);
                if (distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1 > 1.12f) { continue; } //-> saving some performance for planes that are anyway not visible (because their alpha reached zero)
                float alphaOfCurrGridSlicesMainLine = 1.0f - 0.9f * distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1;
                Vector3 lineStartPos_ofLowestLineInsideSlice = lineStartPos_ofMainLineInsideSlice + vectorFromGridSlicesMainLineToLowestLineInsideSlice;
                Vector3 lineEndPos_ofLowestLineInsideSlice = lineEndPos_ofMainLineInsideSlice + vectorFromGridSlicesMainLineToLowestLineInsideSlice;

                for (int i_lineInsideSlice = 0; i_lineInsideSlice < numberOfLinesPerGridSlice; i_lineInsideSlice++)
                {
                    if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
                    Vector3 lowestLineInsideSlice_to_currLineInsideSlice = (-vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized) * distanceBeweenLinesInsideGridSlice_inWorldUnits * i_lineInsideSlice;
                    Vector3 startPos_ofLineInsideSlice = lineStartPos_ofLowestLineInsideSlice + lowestLineInsideSlice_to_currLineInsideSlice;
                    Vector3 endPos_ofLineInsideSlice = lineEndPos_ofLowestLineInsideSlice + lowestLineInsideSlice_to_currLineInsideSlice;
                    float distanceToCurrSlicesMainLine_0to1 = Mathf.Abs(((float)i_lineInsideSlice - halfNumberOfLinesPerGridSlice) / halfNumberOfLinesPerGridSlice);
                    float alphaOfLineInsideSlice = alphaOfCurrGridSlicesMainLine * (1.0f - 0.9f * distanceToCurrSlicesMainLine_0to1);
                    Color lineColor_ofLineInsideSlice = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, alphaOfLineInsideSlice);
                    Line_fadeableAnimSpeed.InternalDraw(startPos_ofLineInsideSlice, endPos_ofLineInsideSlice, lineColor_ofLineInsideSlice, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                }
            }
        }

        static void DrawYDimPlanesDenseWithoutTextLocal(Vector3 positionAroundWhichToDraw_global, Vector3 localForward_normalizedInGlobalSpace, Vector3 localUp_normalizedInGlobalSpace, Vector3 localRight_normalizedInGlobalSpace, Vector3 scaleOfLocalSpace, Vector3 localPositionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, float drawDensity, float distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, float durationInSec, bool hiddenByNearerObjects, Color color, bool turned90DegAroundHisAxis)
        {
            float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder = distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder * scaleOfLocalSpace.y;
            float extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits = extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float extentOfWholeCascadeAlongAxis_inWorldUnits = extentOfWholeCascadeAlongAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float lengthOfDrawnLinesAtEachGridSlicePos_inWorldUnits = 2.0f * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits;
            float distanceBeweenLinesInsideGridSlice_inWorldUnits = gridDensityDefaultScaleFactor * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder / drawDensity;
            int numberOfLinesPerGridSlice = Mathf.RoundToInt(lengthOfDrawnLinesAtEachGridSlicePos_inWorldUnits / distanceBeweenLinesInsideGridSlice_inWorldUnits);
            numberOfLinesPerGridSlice = Mathf.Max(numberOfLinesPerGridSlice, 3);
            float halfNumberOfLinesPerGridSlice = 0.5f * numberOfLinesPerGridSlice;

            float localPositionAroundWhichToDraw_expressedInUnitsOfThisOrder_y = localPositionAroundWhichToDraw.y / distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder;
            float localYPos_ofLowestGridPos_inLocalSpaceUnits_ifUnrotated = Mathf.Round(localPositionAroundWhichToDraw_expressedInUnitsOfThisOrder_y - extentOfWholeCascadeAlongAxis_inOrdersOwnUnits) * distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder;

            float distance_fromDrawAroundPos_to_smallestYGridPos_inGlobalWorldUnits = Mathf.Abs(localYPos_ofLowestGridPos_inLocalSpaceUnits_ifUnrotated - localPositionAroundWhichToDraw.y) * scaleOfLocalSpace.y;
            Vector3 drawAroundPosGlobal_to_smallestYGlobal = (-localUp_normalizedInGlobalSpace) * distance_fromDrawAroundPos_to_smallestYGridPos_inGlobalWorldUnits;
            Vector3 lineCenterGlobal_to_lineEndGlobal = turned90DegAroundHisAxis ? (localForward_normalizedInGlobalSpace * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits) : (localRight_normalizedInGlobalSpace * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits);
            Vector3 lineCenterGlobal_ofLowestGridSlice = positionAroundWhichToDraw_global + drawAroundPosGlobal_to_smallestYGlobal;
            Vector3 startPosGlobal_ofMainLineOfLowestGridSlice = lineCenterGlobal_ofLowestGridSlice - lineCenterGlobal_to_lineEndGlobal;
            Vector3 endPosGlobal_ofMainLineOfLowestGridSlice = lineCenterGlobal_ofLowestGridSlice + lineCenterGlobal_to_lineEndGlobal;
            Vector3 lineStartPos_ifALineWouldBeDrawnThroughPositionAroundWhichToDraw_global = positionAroundWhichToDraw_global - lineCenterGlobal_to_lineEndGlobal;
            Vector3 vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized = turned90DegAroundHisAxis ? (-localRight_normalizedInGlobalSpace) : (-localForward_normalizedInGlobalSpace);
            Vector3 vectorFromGridSlicesMainLineToLowestLineInsideSlice = vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits;

            float lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits = 2.0f * extentOfWholeCascadeAlongAxis_inOrdersOwnUnits;
            int numberOfVisualizedGridSlicesAlongAxis = Mathf.RoundToInt(lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits) + 2; //-> "+ 2" instead of "+ 1", because otherwise planes pop to invisible before reaching nearZeroAlpha. The "distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1"-earlyContinue inside the for-loop ensures that the line count doesn't rise through this measure
            numberOfVisualizedGridSlicesAlongAxis = Mathf.Max(numberOfVisualizedGridSlicesAlongAxis, 2);
            for (int i_gridSlice = 0; i_gridSlice < numberOfVisualizedGridSlicesAlongAxis; i_gridSlice++)
            {
                Vector3 lowestSlice_to_currSlice = localUp_normalizedInGlobalSpace * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * i_gridSlice;
                Vector3 lineStartPos_ofMainLineInsideSlice = startPosGlobal_ofMainLineOfLowestGridSlice + lowestSlice_to_currSlice;
                Vector3 lineEndPos_ofMainLineInsideSlice = endPosGlobal_ofMainLineOfLowestGridSlice + lowestSlice_to_currSlice;
                float distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1 = Mathf.Abs((lineStartPos_ofMainLineInsideSlice - lineStartPos_ifALineWouldBeDrawnThroughPositionAroundWhichToDraw_global).magnitude / extentOfWholeCascadeAlongAxis_inWorldUnits);
                if (distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1 > 1.12f) { continue; } //-> saving some performance for planes that are anyway not visible (because their alpha reached zero)
                float alphaOfCurrGridSlicesMainLine = 1.0f - 0.9f * distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1;
                Vector3 lineStartPos_ofLowestLineInsideSlice = lineStartPos_ofMainLineInsideSlice + vectorFromGridSlicesMainLineToLowestLineInsideSlice;
                Vector3 lineEndPos_ofLowestLineInsideSlice = lineEndPos_ofMainLineInsideSlice + vectorFromGridSlicesMainLineToLowestLineInsideSlice;

                for (int i_lineInsideSlice = 0; i_lineInsideSlice < numberOfLinesPerGridSlice; i_lineInsideSlice++)
                {
                    if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
                    Vector3 lowestLineInsideSlice_to_currLineInsideSlice = (-vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized) * distanceBeweenLinesInsideGridSlice_inWorldUnits * i_lineInsideSlice;
                    Vector3 startPos_ofLineInsideSlice = lineStartPos_ofLowestLineInsideSlice + lowestLineInsideSlice_to_currLineInsideSlice;
                    Vector3 endPos_ofLineInsideSlice = lineEndPos_ofLowestLineInsideSlice + lowestLineInsideSlice_to_currLineInsideSlice;
                    float distanceToCurrSlicesMainLine_0to1 = Mathf.Abs(((float)i_lineInsideSlice - halfNumberOfLinesPerGridSlice) / halfNumberOfLinesPerGridSlice);
                    float alphaOfLineInsideSlice = alphaOfCurrGridSlicesMainLine * (1.0f - 0.9f * distanceToCurrSlicesMainLine_0to1);
                    Color lineColor_ofLineInsideSlice = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, alphaOfLineInsideSlice);
                    Line_fadeableAnimSpeed.InternalDraw(startPos_ofLineInsideSlice, endPos_ofLineInsideSlice, lineColor_ofLineInsideSlice, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                }
            }
        }

        static void DrawZDimPlanesDenseWithoutTextLocal(Vector3 positionAroundWhichToDraw_global, Vector3 localForward_normalizedInGlobalSpace, Vector3 localUp_normalizedInGlobalSpace, Vector3 localRight_normalizedInGlobalSpace, Vector3 scaleOfLocalSpace, Vector3 localPositionAroundWhichToDraw, float extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits, float extentOfWholeCascadeAlongAxis_inOrdersOwnUnits, float drawDensity, float distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder, float durationInSec, bool hiddenByNearerObjects, Color color, bool turned90DegAroundHisAxis)
        {
            float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder = distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder * scaleOfLocalSpace.z;
            float extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits = extentOfDrawnLinesAtEachFixedPosOnAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float extentOfWholeCascadeAlongAxis_inWorldUnits = extentOfWholeCascadeAlongAxis_inOrdersOwnUnits * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float lengthOfDrawnLinesAtEachGridSlicePos_inWorldUnits = 2.0f * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits;
            float distanceBeweenLinesInsideGridSlice_inWorldUnits = gridDensityDefaultScaleFactor * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder / drawDensity;
            int numberOfLinesPerGridSlice = Mathf.RoundToInt(lengthOfDrawnLinesAtEachGridSlicePos_inWorldUnits / distanceBeweenLinesInsideGridSlice_inWorldUnits);
            numberOfLinesPerGridSlice = Mathf.Max(numberOfLinesPerGridSlice, 3);
            float halfNumberOfLinesPerGridSlice = 0.5f * numberOfLinesPerGridSlice;

            float localPositionAroundWhichToDraw_expressedInUnitsOfThisOrder_z = localPositionAroundWhichToDraw.z / distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder;
            float localZPos_ofLowestGridPos_inLocalSpaceUnits_ifUnrotated = Mathf.Round(localPositionAroundWhichToDraw_expressedInUnitsOfThisOrder_z - extentOfWholeCascadeAlongAxis_inOrdersOwnUnits) * distanceBetweenVisualizedGridPointsInLocalSpaceUnits_forThisOrder;

            float distance_fromDrawAroundPos_to_smallestZGridPos_inGlobalWorldUnits = Mathf.Abs(localZPos_ofLowestGridPos_inLocalSpaceUnits_ifUnrotated - localPositionAroundWhichToDraw.z) * scaleOfLocalSpace.z;
            Vector3 drawAroundPosGlobal_to_smallestZGlobal = (-localForward_normalizedInGlobalSpace) * distance_fromDrawAroundPos_to_smallestZGridPos_inGlobalWorldUnits;
            Vector3 lineCenterGlobal_to_lineEndGlobal = turned90DegAroundHisAxis ? (localRight_normalizedInGlobalSpace * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits) : (localUp_normalizedInGlobalSpace * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits);
            Vector3 lineCenterGlobal_ofLowestGridSlice = positionAroundWhichToDraw_global + drawAroundPosGlobal_to_smallestZGlobal;
            Vector3 startPosGlobal_ofMainLineOfLowestGridSlice = lineCenterGlobal_ofLowestGridSlice - lineCenterGlobal_to_lineEndGlobal;
            Vector3 endPosGlobal_ofMainLineOfLowestGridSlice = lineCenterGlobal_ofLowestGridSlice + lineCenterGlobal_to_lineEndGlobal;
            Vector3 lineStartPos_ifALineWouldBeDrawnThroughPositionAroundWhichToDraw_global = positionAroundWhichToDraw_global - lineCenterGlobal_to_lineEndGlobal;
            Vector3 vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized = turned90DegAroundHisAxis ? (-localUp_normalizedInGlobalSpace) : (-localRight_normalizedInGlobalSpace);
            Vector3 vectorFromGridSlicesMainLineToLowestLineInsideSlice = vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized * extentOfDrawnLinesAtEachFixedPosOnAxis_inWorldUnits;

            float lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits = 2.0f * extentOfWholeCascadeAlongAxis_inOrdersOwnUnits;
            int numberOfVisualizedGridSlicesAlongAxis = Mathf.RoundToInt(lengthOfWholeCascadeAlongAxis_inOrdersOwnUnits) + 2; //-> "+ 2" instead of "+ 1", because otherwise planes pop to invisible before reaching nearZeroAlpha. The "distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1"-earlyContinue inside the for-loop ensures that the line count doesn't rise through this measure
            numberOfVisualizedGridSlicesAlongAxis = Mathf.Max(numberOfVisualizedGridSlicesAlongAxis, 2);
            for (int i_gridSlice = 0; i_gridSlice < numberOfVisualizedGridSlicesAlongAxis; i_gridSlice++)
            {
                Vector3 lowestSlice_to_currSlice = localForward_normalizedInGlobalSpace * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder * i_gridSlice;
                Vector3 lineStartPos_ofMainLineInsideSlice = startPosGlobal_ofMainLineOfLowestGridSlice + lowestSlice_to_currSlice;
                Vector3 lineEndPos_ofMainLineInsideSlice = endPosGlobal_ofMainLineOfLowestGridSlice + lowestSlice_to_currSlice;
                float distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1 = Mathf.Abs((lineStartPos_ofMainLineInsideSlice - lineStartPos_ifALineWouldBeDrawnThroughPositionAroundWhichToDraw_global).magnitude / extentOfWholeCascadeAlongAxis_inWorldUnits);
                if (distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1 > 1.12f) { continue; } //-> saving some performance for planes that are anyway not visible (because their alpha reached zero)
                float alphaOfCurrGridSlicesMainLine = 1.0f - 0.9f * distanceOfCurrGridSliceToPosAroundWhichToDraw_0to1;
                Vector3 lineStartPos_ofLowestLineInsideSlice = lineStartPos_ofMainLineInsideSlice + vectorFromGridSlicesMainLineToLowestLineInsideSlice;
                Vector3 lineEndPos_ofLowestLineInsideSlice = lineEndPos_ofMainLineInsideSlice + vectorFromGridSlicesMainLineToLowestLineInsideSlice;

                for (int i_lineInsideSlice = 0; i_lineInsideSlice < numberOfLinesPerGridSlice; i_lineInsideSlice++)
                {
                    if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
                    Vector3 lowestLineInsideSlice_to_currLineInsideSlice = (-vectorFromGridSlicesMainLineToLowestLineInsideSlice_normalized) * distanceBeweenLinesInsideGridSlice_inWorldUnits * i_lineInsideSlice;
                    Vector3 startPos_ofLineInsideSlice = lineStartPos_ofLowestLineInsideSlice + lowestLineInsideSlice_to_currLineInsideSlice;
                    Vector3 endPos_ofLineInsideSlice = lineEndPos_ofLowestLineInsideSlice + lowestLineInsideSlice_to_currLineInsideSlice;
                    float distanceToCurrSlicesMainLine_0to1 = Mathf.Abs(((float)i_lineInsideSlice - halfNumberOfLinesPerGridSlice) / halfNumberOfLinesPerGridSlice);
                    float alphaOfLineInsideSlice = alphaOfCurrGridSlicesMainLine * (1.0f - 0.9f * distanceToCurrSlicesMainLine_0to1);
                    Color lineColor_ofLineInsideSlice = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, alphaOfLineInsideSlice);
                    Line_fadeableAnimSpeed.InternalDraw(startPos_ofLineInsideSlice, endPos_ofLineInsideSlice, lineColor_ofLineInsideSlice, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                }
            }
        }

        static void TryDrawDistanceLines_fromDrawAroundPos_toNeighboringGridValue(bool forGridLINES_notForGridPLANES, float scaleOfLocalSpace, Vector3 forwardNormalized_ofRepresentedGridAxis, Vector3 vectorAlongGridLine_normalized, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, Vector3 positionAroundWhichToDraw_global, float positionAroundWhichToDraw_expressedInUnitsOfThisOrder, Color colorOfGridLines, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DrawEngineBasics.hide_distanceDisplay_forGrids == false)
            {
                float floor_of_positionAroundWhichToDraw_expressedInUnitsOfThisOrder = Mathf.Floor(positionAroundWhichToDraw_expressedInUnitsOfThisOrder);
                float floorToDrawAroundPos_expressedInUnitsOfThisOrder = positionAroundWhichToDraw_expressedInUnitsOfThisOrder - floor_of_positionAroundWhichToDraw_expressedInUnitsOfThisOrder;
                float drawAroundPosToCeil_expressedInUnitsOfThisOrder = 1.0f - floorToDrawAroundPos_expressedInUnitsOfThisOrder;
                float portion0to1_ofDrawAroundPos_fromNextLowerToNextHigherGridLine = floorToDrawAroundPos_expressedInUnitsOfThisOrder;

                float distanceToNextGridLineBELOWdrawAroundPos_forCurrentOrderOfMagnitude_inWorldSpace = floorToDrawAroundPos_expressedInUnitsOfThisOrder * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
                float distanceToNextGridLineABOVEdrawAroundPos_forCurrentOrderOfMagnitude_inWorldSpace = drawAroundPosToCeil_expressedInUnitsOfThisOrder * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
                float distanceToNextGridLineBELOWdrawAroundPos_forCurrentOrderOfMagnitude_inLocalSpace = Mathf.Approximately(1.0f, scaleOfLocalSpace) ? distanceToNextGridLineBELOWdrawAroundPos_forCurrentOrderOfMagnitude_inWorldSpace : (distanceToNextGridLineBELOWdrawAroundPos_forCurrentOrderOfMagnitude_inWorldSpace / scaleOfLocalSpace); //-> slightly complicated calculation pattern (instead of plainly diving by "scaleOfLocalSpace", which shouldn't make a difference if it is anyway "1"), because float calculations should be avoided here if possible, since they may introduce errors that will get displayed as text
                float distanceToNextGridLineABOVEdrawAroundPos_forCurrentOrderOfMagnitude_inLocalSpace = Mathf.Approximately(1.0f, scaleOfLocalSpace) ? distanceToNextGridLineABOVEdrawAroundPos_forCurrentOrderOfMagnitude_inWorldSpace : (distanceToNextGridLineABOVEdrawAroundPos_forCurrentOrderOfMagnitude_inWorldSpace / scaleOfLocalSpace); //-> slightly complicated calculation pattern (instead of plainly diving by "scaleOfLocalSpace", which shouldn't make a difference if it is anyway "1"), because float calculations should be avoided here if possible, since they may introduce errors that will get displayed as text

                float shiftOffsetDistance_fromDrawAroundPos_toDistanceDisplay = DrawEngineBasics.offsetForDistanceDisplays_inGrids * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
                Vector3 shiftOffsetVector_fromDrawAroundPos_toDistanceDisplay = shiftOffsetDistance_fromDrawAroundPos_toDistanceDisplay * vectorAlongGridLine_normalized;
                Vector3 positionAroundWhichToDraw_shiftedParallelToGrid_toDistanceDisplay = positionAroundWhichToDraw_global + shiftOffsetVector_fromDrawAroundPos_toDistanceDisplay;
                Vector3 endOfDistanceVector_onNextLOWERgridPosition = positionAroundWhichToDraw_shiftedParallelToGrid_toDistanceDisplay - forwardNormalized_ofRepresentedGridAxis * distanceToNextGridLineBELOWdrawAroundPos_forCurrentOrderOfMagnitude_inWorldSpace;
                Vector3 endOfDistanceVector_onNextHIGHERgridPosition = positionAroundWhichToDraw_shiftedParallelToGrid_toDistanceDisplay + forwardNormalized_ofRepresentedGridAxis * distanceToNextGridLineABOVEdrawAroundPos_forCurrentOrderOfMagnitude_inWorldSpace;

                float portion0to1_beside0p5_whereSecondaryVectorIsCompletelyFadedOut = 0.125f;
                float Op5_minus_portion0to1_beside0p5_whereSecondaryVectorIsCompletelyFadedOut = 0.5f - portion0to1_beside0p5_whereSecondaryVectorIsCompletelyFadedOut;
                float alpha_ofColorToBelow_insideFadeOutSpan = 1.0f - UtilitiesDXXL_Math.Get_2degParabolicFlateningRise((portion0to1_ofDrawAroundPos_fromNextLowerToNextHigherGridLine - 0.5f) / portion0to1_beside0p5_whereSecondaryVectorIsCompletelyFadedOut);
                float alpha_ofColorToAbove_insideFadeOutSpan = UtilitiesDXXL_Math.Get_2degParabolicSteepeningRise((portion0to1_ofDrawAroundPos_fromNextLowerToNextHigherGridLine - Op5_minus_portion0to1_beside0p5_whereSecondaryVectorIsCompletelyFadedOut) / portion0to1_beside0p5_whereSecondaryVectorIsCompletelyFadedOut);
                float alpha_ofColorToBelow_ifOnSideOfLongerVector = (portion0to1_ofDrawAroundPos_fromNextLowerToNextHigherGridLine > (0.5f + portion0to1_beside0p5_whereSecondaryVectorIsCompletelyFadedOut)) ? 0.0f : alpha_ofColorToBelow_insideFadeOutSpan;
                float alpha_ofColorToAbove_ifOnSideOfLongerVector = (portion0to1_ofDrawAroundPos_fromNextLowerToNextHigherGridLine < (0.5f - portion0to1_beside0p5_whereSecondaryVectorIsCompletelyFadedOut)) ? 0.0f : alpha_ofColorToAbove_insideFadeOutSpan;
                float alpha_ofColorToBelow = (portion0to1_ofDrawAroundPos_fromNextLowerToNextHigherGridLine < 0.5f) ? 1.0f : alpha_ofColorToBelow_ifOnSideOfLongerVector;
                float alpha_ofColorToAbove = (portion0to1_ofDrawAroundPos_fromNextLowerToNextHigherGridLine > 0.5f) ? 1.0f : alpha_ofColorToAbove_ifOnSideOfLongerVector;

                bool toBelow_isVisible = (UtilitiesDXXL_Math.ApproximatelyZero(alpha_ofColorToBelow) == false);
                bool toAbove_isVisible = (UtilitiesDXXL_Math.ApproximatelyZero(alpha_ofColorToAbove) == false);

                Color colorOfVectorToBelow = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorOfGridLines, alpha_ofColorToBelow);
                Color colorOfVectorToAbove = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorOfGridLines, alpha_ofColorToAbove);

                float lineWidth_ofDistanceLine = 0.04f * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;

                DrawDistanceVectors(toBelow_isVisible, toAbove_isVisible, distanceToNextGridLineBELOWdrawAroundPos_forCurrentOrderOfMagnitude_inWorldSpace, distanceToNextGridLineABOVEdrawAroundPos_forCurrentOrderOfMagnitude_inWorldSpace, positionAroundWhichToDraw_shiftedParallelToGrid_toDistanceDisplay, endOfDistanceVector_onNextLOWERgridPosition, endOfDistanceVector_onNextHIGHERgridPosition, lineWidth_ofDistanceLine, vectorAlongGridLine_normalized, colorOfVectorToBelow, colorOfVectorToAbove, durationInSec, hiddenByNearerObjects);
                DrawDistanceTexts(toBelow_isVisible, toAbove_isVisible, distanceToNextGridLineBELOWdrawAroundPos_forCurrentOrderOfMagnitude_inLocalSpace, distanceToNextGridLineABOVEdrawAroundPos_forCurrentOrderOfMagnitude_inLocalSpace, endOfDistanceVector_onNextLOWERgridPosition, endOfDistanceVector_onNextHIGHERgridPosition, forwardNormalized_ofRepresentedGridAxis, vectorAlongGridLine_normalized, lineWidth_ofDistanceLine, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, colorOfVectorToBelow, colorOfVectorToAbove, durationInSec, hiddenByNearerObjects);
                DrawDistanceEndPlates(forGridLINES_notForGridPLANES, toBelow_isVisible, toAbove_isVisible, endOfDistanceVector_onNextLOWERgridPosition, endOfDistanceVector_onNextHIGHERgridPosition, forwardNormalized_ofRepresentedGridAxis, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, colorOfVectorToBelow, colorOfVectorToAbove, durationInSec, hiddenByNearerObjects);
                DrawDashedLineToDistanceVector(positionAroundWhichToDraw_global, positionAroundWhichToDraw_shiftedParallelToGrid_toDistanceDisplay, forwardNormalized_ofRepresentedGridAxis, vectorAlongGridLine_normalized, distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, colorOfGridLines, durationInSec, hiddenByNearerObjects);
            }
        }

        static void DrawDistanceVectors(bool toBelow_isVisible, bool toAbove_isVisible, float distanceToNextGridLineBELOWdrawAroundPos_forCurrentOrderOfMagnitude_inWorldSpace, float distanceToNextGridLineABOVEdrawAroundPos_forCurrentOrderOfMagnitude_inWorldSpace, Vector3 positionAroundWhichToDraw_shiftedParallelToGrid_toDistanceDisplay, Vector3 endOfDistanceVector_onNextLOWERgridPosition, Vector3 endOfDistanceVector_onNextHIGHERgridPosition, float lineWidth_ofDistanceLine, Vector3 vectorAlongGridLine_normalized, Color colorOfVectorToBelow, Color colorOfVectorToAbove, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 customAmplitudeAndTextDir = vectorAlongGridLine_normalized;
            float endPlates_size = 0.0f; //-> the end plates are drawn separately, so not here together with the vector
            float coneLength = 0.17f;
            bool flattenThickRoundLineIntoAmplitudePlane = true;
            bool pointerAtBothSides = true;

            UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);

            if (toBelow_isVisible)
            {
                if (distanceToNextGridLineBELOWdrawAroundPos_forCurrentOrderOfMagnitude_inWorldSpace > UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.shortestLineWithDefinedAmplitudeDir_withTolerancePadding)
                {
                    DrawBasics.Vector(positionAroundWhichToDraw_shiftedParallelToGrid_toDistanceDisplay, endOfDistanceVector_onNextLOWERgridPosition, colorOfVectorToBelow, lineWidth_ofDistanceLine, null, coneLength, pointerAtBothSides, flattenThickRoundLineIntoAmplitudePlane, customAmplitudeAndTextDir, false, 0.0f, false, endPlates_size, durationInSec, hiddenByNearerObjects);
                }
            }

            if (toAbove_isVisible)
            {
                if (distanceToNextGridLineABOVEdrawAroundPos_forCurrentOrderOfMagnitude_inWorldSpace > UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.shortestLineWithDefinedAmplitudeDir_withTolerancePadding)
                {
                    DrawBasics.Vector(positionAroundWhichToDraw_shiftedParallelToGrid_toDistanceDisplay, endOfDistanceVector_onNextHIGHERgridPosition, colorOfVectorToAbove, lineWidth_ofDistanceLine, null, coneLength, pointerAtBothSides, flattenThickRoundLineIntoAmplitudePlane, customAmplitudeAndTextDir, false, 0.0f, false, endPlates_size, durationInSec, hiddenByNearerObjects);
                }
            }

            UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
        }

        static void DrawDistanceTexts(bool toBelow_isVisible, bool toAbove_isVisible, float distanceToNextGridLineBELOWdrawAroundPos_forCurrentOrderOfMagnitude_inLocalSpace, float distanceToNextGridLineABOVEdrawAroundPos_forCurrentOrderOfMagnitude_inLocalSpace, Vector3 endOfDistanceVector_onNextLOWERgridPosition, Vector3 endOfDistanceVector_onNextHIGHERgridPosition, Vector3 forwardNormalized_ofRepresentedGridAxis, Vector3 vectorAlongGridLine_normalized, float lineWidth_ofDistanceLine, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, Color colorOfVectorToBelow, Color colorOfVectorToAbove, float durationInSec, bool hiddenByNearerObjects)
        {
            float size = 0.1f;//-> will anyway get overwritten by "forceTextBlockEnlargementToThisMinWidth"/"forceRestrictTextBlockSizeToThisMaxTextWidth"
            Vector3 textDirection = forwardNormalized_ofRepresentedGridAxis;
            Vector3 textUp = vectorAlongGridLine_normalized;
            Vector3 textShiftOffset_thatCompensatesLineWidth = 0.5f * lineWidth_ofDistanceLine * vectorAlongGridLine_normalized;
            float forceTextBlockEnlargementToThisMinWidth = 0.7f * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            float forceRestrictTextBlockSizeToThisMaxTextWidth = forceTextBlockEnlargementToThisMinWidth;
            int strokeWidth = 60000;

            string text;
            Vector3 textPosition;
            DrawText.TextAnchorDXXL textAnchor;

            if (toBelow_isVisible)
            {
                if (Mathf.Approximately(0.0f, distanceToNextGridLineBELOWdrawAroundPos_forCurrentOrderOfMagnitude_inLocalSpace))
                {
                    //-> mitigate non-stable flickering (due to float calculation imprecision) between different grid segments (if the drawAroundPos is right on the border) by drawing the text for both side in these cases:
                    text = DrawText.MarkupStrokeWidth("-" + distanceToNextGridLineBELOWdrawAroundPos_forCurrentOrderOfMagnitude_inLocalSpace, strokeWidth);
                    textPosition = endOfDistanceVector_onNextLOWERgridPosition + GetDistanceTextPositionOffset_relToVectorLine_forDisanceDisplayBELOWtheVectorLine(distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, vectorAlongGridLine_normalized);
                    textAnchor = DrawText.TextAnchorDXXL.UpperRight;
                    UtilitiesDXXL_Text.WriteFramed(text, textPosition, colorOfVectorToBelow, size, textDirection, textUp, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, forceTextBlockEnlargementToThisMinWidth, forceRestrictTextBlockSizeToThisMaxTextWidth, 0.0f, true, durationInSec, hiddenByNearerObjects);

                    //Adding a "+" prefix, so that the text size difference doesn't get to irritatingly big:
                    text = DrawText.MarkupStrokeWidth("+" + distanceToNextGridLineBELOWdrawAroundPos_forCurrentOrderOfMagnitude_inLocalSpace, strokeWidth);
                }
                else
                {
                    text = DrawText.MarkupStrokeWidth("" + distanceToNextGridLineBELOWdrawAroundPos_forCurrentOrderOfMagnitude_inLocalSpace, strokeWidth);
                }

                textPosition = endOfDistanceVector_onNextLOWERgridPosition + textShiftOffset_thatCompensatesLineWidth + GetDistanceTextPositionOffset_relToVectorLine_forDisanceDisplayABOVEtheVectorLine(distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, vectorAlongGridLine_normalized);
                textAnchor = DrawText.TextAnchorDXXL.LowerLeftOfFirstLine;
                UtilitiesDXXL_Text.WriteFramed(text, textPosition, colorOfVectorToBelow, size, textDirection, textUp, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, forceTextBlockEnlargementToThisMinWidth, forceRestrictTextBlockSizeToThisMaxTextWidth, 0.0f, true, durationInSec, hiddenByNearerObjects);
            }

            if (toAbove_isVisible)
            {
                if (Mathf.Approximately(0.0f, distanceToNextGridLineABOVEdrawAroundPos_forCurrentOrderOfMagnitude_inLocalSpace))
                {
                    //-> mitigate non-stable flickering (due to float calculation imprecision) between different grid segments (if the drawAroundPos is right on the border) by drawing the text for both side in these cases:
                    text = DrawText.MarkupStrokeWidth("+" + distanceToNextGridLineABOVEdrawAroundPos_forCurrentOrderOfMagnitude_inLocalSpace, strokeWidth);
                    textPosition = endOfDistanceVector_onNextHIGHERgridPosition + textShiftOffset_thatCompensatesLineWidth + GetDistanceTextPositionOffset_relToVectorLine_forDisanceDisplayABOVEtheVectorLine(distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, vectorAlongGridLine_normalized);
                    textAnchor = DrawText.TextAnchorDXXL.LowerLeftOfFirstLine;
                    UtilitiesDXXL_Text.WriteFramed(text, textPosition, colorOfVectorToAbove, size, textDirection, textUp, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, forceTextBlockEnlargementToThisMinWidth, forceRestrictTextBlockSizeToThisMaxTextWidth, 0.0f, true, durationInSec, hiddenByNearerObjects);

                    //Adding a "-" prefix, so that the text size difference doesn't get to irritatingly big:
                    text = DrawText.MarkupStrokeWidth("-" + distanceToNextGridLineABOVEdrawAroundPos_forCurrentOrderOfMagnitude_inLocalSpace, strokeWidth);
                }
                else
                {
                    text = DrawText.MarkupStrokeWidth("" + distanceToNextGridLineABOVEdrawAroundPos_forCurrentOrderOfMagnitude_inLocalSpace, strokeWidth);
                }

                textPosition = endOfDistanceVector_onNextHIGHERgridPosition + GetDistanceTextPositionOffset_relToVectorLine_forDisanceDisplayBELOWtheVectorLine(distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, vectorAlongGridLine_normalized);
                textAnchor = DrawText.TextAnchorDXXL.UpperRight;
                UtilitiesDXXL_Text.WriteFramed(text, textPosition, colorOfVectorToAbove, size, textDirection, textUp, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, forceTextBlockEnlargementToThisMinWidth, forceRestrictTextBlockSizeToThisMaxTextWidth, 0.0f, true, durationInSec, hiddenByNearerObjects);
            }
        }

        static void DrawDistanceEndPlates(bool forGridLINES_notForGridPLANES, bool toBelow_isVisible, bool toAbove_isVisible, Vector3 endOfDistanceVector_onNextLOWERgridPosition, Vector3 endOfDistanceVector_onNextHIGHERgridPosition, Vector3 forwardNormalized_ofRepresentedGridAxis, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, Color colorOfVectorToBelow, Color colorOfVectorToAbove, float durationInSec, bool hiddenByNearerObjects)
        {
            if (forGridLINES_notForGridPLANES == false)
            {
                float radius = 0.08f * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
                Vector3 normal = forwardNormalized_ofRepresentedGridAxis;
                bool filledWithSpokes = true;
                float alphaFactor_forDecagons = 0.7f;
                Color colorOfDecagon_forVectorToBelow = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorOfVectorToBelow, alphaFactor_forDecagons);
                Color colorOfDecagon_forVectorToAbove = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorOfVectorToAbove, alphaFactor_forDecagons);

                if (toBelow_isVisible)
                {
                    DrawShapes.Decagon(endOfDistanceVector_onNextLOWERgridPosition, radius, colorOfDecagon_forVectorToBelow, normal, default(Vector3), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, filledWithSpokes, false, durationInSec, hiddenByNearerObjects);
                }

                if (toAbove_isVisible)
                {
                    DrawShapes.Decagon(endOfDistanceVector_onNextHIGHERgridPosition, radius, colorOfDecagon_forVectorToAbove, normal, default(Vector3), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, filledWithSpokes, false, durationInSec, hiddenByNearerObjects);
                }
            }
        }

        static void DrawDashedLineToDistanceVector(Vector3 positionAroundWhichToDraw_global, Vector3 positionAroundWhichToDraw_shiftedParallelToGrid_toDistanceDisplay, Vector3 forwardNormalized_ofRepresentedGridAxis, Vector3 vectorAlongGridLine_normalized, float distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder, Color colorOfGridLines, float durationInSec, bool hiddenByNearerObjects)
        {
            float lineWidth = 0.01f * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            DrawBasics.LineStyle style = DrawBasics.LineStyle.dashedLong;
            float stylePatternScaleFactor = 1.8f * distanceBetweenVisualizedGridPointsInWorldUnits_forThisOrder;
            Vector3 customAmplitudeAndTextDir = Vector3.Cross(forwardNormalized_ofRepresentedGridAxis, vectorAlongGridLine_normalized);
            bool flattenThickRoundLineIntoAmplitudePlane = true;
            bool skipPatternEnlargementForLongLines = true;
            bool skipPatternEnlargementForShortLines = true;
            DrawBasics.Line(positionAroundWhichToDraw_global, positionAroundWhichToDraw_shiftedParallelToGrid_toDistanceDisplay, colorOfGridLines, lineWidth, null, style, stylePatternScaleFactor, 0.0f, customAmplitudeAndTextDir, flattenThickRoundLineIntoAmplitudePlane, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, skipPatternEnlargementForLongLines, skipPatternEnlargementForShortLines);
        }

        static Color GetFallbackColor(bool drawXDim, bool drawYDim, bool drawZDim, Color colorForMainX, Color colorForMainY, Color colorForMainZ)
        {
            Color fallbackColor = default;
            if (drawZDim)
            {
                fallbackColor = colorForMainZ;
            }
            if (drawYDim)
            {
                fallbackColor = colorForMainY;
            }
            if (drawXDim)
            {
                fallbackColor = colorForMainX;
            }
            return fallbackColor;
        }

        static void DrawSkewedPosIndicatingCubes(Vector3 positionAroundWhichToDraw, Color color, float extentOfOrder_inWorldUnits, bool drawXDim, bool drawYDim, bool drawZDim, float durationInSec, bool hiddenByNearerObjects)
        {
            float witdth_ofSquare = 0.015f * extentOfOrder_inWorldUnits;
            float diagonal_ofSquare = witdth_ofSquare * UtilitiesDXXL_Math.sqrtOf2_precalced;
            float half_diagonal_ofSquare = 0.5f * diagonal_ofSquare;

            if (drawXDim)
            {
                DrawShapes.FlatShape(positionAroundWhichToDraw, DrawShapes.Shape2DType.square, witdth_ofSquare, witdth_ofSquare, color, Quaternion.LookRotation(Vector3.right, new Vector3(0.0f, 1.0f, 1.0f)), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, DrawBasics.LineStyle.invisible, false, durationInSec, hiddenByNearerObjects);
                Vector3 halfLength_ofLineAlong_xDim = Vector3.right * half_diagonal_ofSquare;
                Line_fadeableAnimSpeed.InternalDraw(positionAroundWhichToDraw + halfLength_ofLineAlong_xDim, positionAroundWhichToDraw - halfLength_ofLineAlong_xDim, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }
            if (drawYDim)
            {
                DrawShapes.FlatShape(positionAroundWhichToDraw, DrawShapes.Shape2DType.square, witdth_ofSquare, witdth_ofSquare, color, Quaternion.LookRotation(Vector3.up, new Vector3(1.0f, 0.0f, 1.0f)), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, DrawBasics.LineStyle.invisible, false, durationInSec, hiddenByNearerObjects);
                Vector3 halfLength_ofLineAlong_yDim = Vector3.up * half_diagonal_ofSquare;
                Line_fadeableAnimSpeed.InternalDraw(positionAroundWhichToDraw + halfLength_ofLineAlong_yDim, positionAroundWhichToDraw - halfLength_ofLineAlong_yDim, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }
            if (drawZDim)
            {
                DrawShapes.FlatShape(positionAroundWhichToDraw, DrawShapes.Shape2DType.square, witdth_ofSquare, witdth_ofSquare, color, Quaternion.LookRotation(Vector3.forward, new Vector3(1.0f, 1.0f, 0.0f)), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, DrawBasics.LineStyle.invisible, false, durationInSec, hiddenByNearerObjects);
                Vector3 halfLength_ofLineAlong_zDim = Vector3.forward * half_diagonal_ofSquare;
                Line_fadeableAnimSpeed.InternalDraw(positionAroundWhichToDraw + halfLength_ofLineAlong_zDim, positionAroundWhichToDraw - halfLength_ofLineAlong_zDim, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }
        }

        static void DrawSkewedPosIndicatingCubes_local(Quaternion rotationOfLocalSpace, Vector3 positionAroundWhichToDraw_global, Color color, float extentOfOrder_inWorldUnits, bool drawXDim, bool drawYDim, bool drawZDim, float durationInSec, bool hiddenByNearerObjects)
        {
            float witdth_ofSquare = 0.015f * extentOfOrder_inWorldUnits;
            float diagonal_ofSquare = witdth_ofSquare * UtilitiesDXXL_Math.sqrtOf2_precalced;
            float half_diagonal_ofSquare = 0.5f * diagonal_ofSquare;

            if (drawXDim)
            {
                DrawShapes.FlatShape(positionAroundWhichToDraw_global, DrawShapes.Shape2DType.square, witdth_ofSquare, witdth_ofSquare, color, rotationOfLocalSpace * Quaternion.LookRotation(Vector3.right, new Vector3(0.0f, 1.0f, 1.0f)), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, DrawBasics.LineStyle.invisible, false, durationInSec, hiddenByNearerObjects);
                Vector3 halfLength_ofLineAlong_xDim = rotationOfLocalSpace * (Vector3.right) * half_diagonal_ofSquare;
                Line_fadeableAnimSpeed.InternalDraw(positionAroundWhichToDraw_global + halfLength_ofLineAlong_xDim, positionAroundWhichToDraw_global - halfLength_ofLineAlong_xDim, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }
            if (drawYDim)
            {
                DrawShapes.FlatShape(positionAroundWhichToDraw_global, DrawShapes.Shape2DType.square, witdth_ofSquare, witdth_ofSquare, color, rotationOfLocalSpace * Quaternion.LookRotation(Vector3.up, new Vector3(1.0f, 0.0f, 1.0f)), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, DrawBasics.LineStyle.invisible, false, durationInSec, hiddenByNearerObjects);
                Vector3 halfLength_ofLineAlong_yDim = rotationOfLocalSpace * (Vector3.up) * half_diagonal_ofSquare;
                Line_fadeableAnimSpeed.InternalDraw(positionAroundWhichToDraw_global + halfLength_ofLineAlong_yDim, positionAroundWhichToDraw_global - halfLength_ofLineAlong_yDim, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }
            if (drawZDim)
            {
                DrawShapes.FlatShape(positionAroundWhichToDraw_global, DrawShapes.Shape2DType.square, witdth_ofSquare, witdth_ofSquare, color, rotationOfLocalSpace * Quaternion.LookRotation(Vector3.forward, new Vector3(1.0f, 1.0f, 0.0f)), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, DrawBasics.LineStyle.invisible, false, durationInSec, hiddenByNearerObjects);
                Vector3 halfLength_ofLineAlong_zDim = rotationOfLocalSpace * (Vector3.forward) * half_diagonal_ofSquare;
                Line_fadeableAnimSpeed.InternalDraw(positionAroundWhichToDraw_global + halfLength_ofLineAlong_zDim, positionAroundWhichToDraw_global - halfLength_ofLineAlong_zDim, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }
        }

        static bool hide_positionAroundWhichToDraw_forGrids_before;
        public static void Set_hide_positionAroundWhichToDraw_forGrids_reversible(bool new_hide_positionAroundWhichToDraw_forGrids)
        {
            hide_positionAroundWhichToDraw_forGrids_before = DrawEngineBasics.hide_positionAroundWhichToDraw_forGrids;
            DrawEngineBasics.hide_positionAroundWhichToDraw_forGrids = new_hide_positionAroundWhichToDraw_forGrids;
        }
        public static void Reverse_hide_positionAroundWhichToDraw_forGrids()
        {
            DrawEngineBasics.hide_positionAroundWhichToDraw_forGrids = hide_positionAroundWhichToDraw_forGrids_before;
        }

        static bool hide_distanceDisplay_forGrids_before;
        public static void Set_hide_distanceDisplay_forGrids_reversible(bool new_hide_distanceDisplay_forGrids)
        {
            hide_distanceDisplay_forGrids_before = DrawEngineBasics.hide_distanceDisplay_forGrids;
            DrawEngineBasics.hide_distanceDisplay_forGrids = new_hide_distanceDisplay_forGrids;
        }
        public static void Reverse_hide_distanceDisplay_forGrids()
        {
            DrawEngineBasics.hide_distanceDisplay_forGrids = hide_distanceDisplay_forGrids_before;
        }

        static float offsetForDistanceDisplays_inGrids_before;
        public static void Set_offsetForDistanceDisplays_inGrids_reversible(float new_offsetForDistanceDisplays_inGrids)
        {
            offsetForDistanceDisplays_inGrids_before = DrawEngineBasics.offsetForDistanceDisplays_inGrids;
            DrawEngineBasics.offsetForDistanceDisplays_inGrids = new_offsetForDistanceDisplays_inGrids;
        }
        public static void Reverse_offsetForDistanceDisplays_inGrids()
        {
            DrawEngineBasics.offsetForDistanceDisplays_inGrids = offsetForDistanceDisplays_inGrids_before;
        }

        static float offsetForCoordinateTextDisplays_inGrids_before;
        public static void Set_offsetForCoordinateTextDisplays_inGrids_reversible(float new_offsetForCoordinateTextDisplays_inGrids)
        {
            offsetForCoordinateTextDisplays_inGrids_before = DrawEngineBasics.offsetForCoordinateTextDisplays_inGrids;
            DrawEngineBasics.offsetForCoordinateTextDisplays_inGrids = new_offsetForCoordinateTextDisplays_inGrids;
        }
        public static void Reverse_offsetForCoordinateTextDisplays_inGrids()
        {
            DrawEngineBasics.offsetForCoordinateTextDisplays_inGrids = offsetForCoordinateTextDisplays_inGrids_before;
        }

        static float coveredGridUnits_rel_forGridPlanes_before;
        public static void Set_coveredGridUnits_rel_forGridPlanes_reversible(float new_coveredGridUnits_rel_forGridPlanes)
        {
            coveredGridUnits_rel_forGridPlanes_before = DrawEngineBasics.coveredGridUnits_rel_forGridPlanes;
            DrawEngineBasics.coveredGridUnits_rel_forGridPlanes = new_coveredGridUnits_rel_forGridPlanes;
        }
        public static void Reverse_coveredGridUnits_rel_forGridPlanes()
        {
            DrawEngineBasics.coveredGridUnits_rel_forGridPlanes = coveredGridUnits_rel_forGridPlanes_before;
        }

        static float sizeScalingForCoordinateTexts_inGrids_before;
        public static void Set_sizeScalingForCoordinateTexts_inGrids_reversible(float new_sizeScalingForCoordinateTexts_inGrids)
        {
            sizeScalingForCoordinateTexts_inGrids_before = DrawEngineBasics.SizeScalingForCoordinateTexts_inGrids;
            DrawEngineBasics.SizeScalingForCoordinateTexts_inGrids = new_sizeScalingForCoordinateTexts_inGrids;
        }
        public static void Reverse_sizeScalingForCoordinateTexts_inGrids()
        {
            DrawEngineBasics.SizeScalingForCoordinateTexts_inGrids = sizeScalingForCoordinateTexts_inGrids_before;
        }

        static bool skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes_before;
        public static void Set_skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes_reversible(bool new_skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes)
        {
            skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes_before = DrawEngineBasics.skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes;
            DrawEngineBasics.skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes = new_skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes;
        }
        public static void Reverse_skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes()
        {
            DrawEngineBasics.skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes = skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes_before;
        }

        static bool skipLocalPrefix_inCoordinateTextsOnGridAxes_before;
        public static void Set_skipLocalPrefix_inCoordinateTextsOnGridAxes_reversible(bool new_skipLocalPrefix_inCoordinateTextsOnGridAxes)
        {
            skipLocalPrefix_inCoordinateTextsOnGridAxes_before = DrawEngineBasics.skipLocalPrefix_inCoordinateTextsOnGridAxes;
            DrawEngineBasics.skipLocalPrefix_inCoordinateTextsOnGridAxes = new_skipLocalPrefix_inCoordinateTextsOnGridAxes;
        }
        public static void Reverse_skipLocalPrefix_inCoordinateTextsOnGridAxes()
        {
            DrawEngineBasics.skipLocalPrefix_inCoordinateTextsOnGridAxes = skipLocalPrefix_inCoordinateTextsOnGridAxes_before;
        }

    }

}
