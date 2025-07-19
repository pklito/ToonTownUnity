namespace DrawXXL
{
    using System.Collections.Generic;
    using UnityEngine;

    public class ChartDrawing
    {
        //taking chart data point values from edit mode to play mode #1:
        //-> Theoretically it is possible by making the chart and all dependent classes serializable
        //-> Though a test resulted in Unity to endlessly compile and finally crash
        //-> The reason might be that the serialization system reaches it's limits. There are many lists, and lists of custom classes that again hold lists, and even lists of lists to be serialized, and the contained classes hold references back to the containing classes. Unitys own documentation on the serialization system says: "avoid nested structures and cross references"
        //-> The mistake during trying this the last time could have been, that the contained sub-classes like "ChartLine" (and many others) hold a reference back to "chart_thisLineIsPartOf". This field has been tried to be serialized with "[SerializeField]". Does this acutually create endless loops, because the lines are (with intermediate steps) also serialized as members inside "ChartDrawing"? Instead "[SerializeReference]" could have been tried, or probalbly even better an approach as "BezierSplineDrawer" uses it (see notes at "BezierSplineDrawer.cs/listOfControlPointTriplets"), though it may be easier there, because a MonoBehaviour is available there.

        //"Taking values from Edit Mode to Play Mode" #2:
        //-> The static charts in the "DrawCharts" class by default reset when you enter playmode, so the charts start empty without any values. In some cases it may be desired to skip this deletion and instead take data values from Edit Mode to Play Mode. You can do so by disabling Domain Reload (see Unity documentation). Though this works only for static charts (like the premade ones in the "DrawCharts" class). It doesn't work for Transform Charts. And it is not possible the other way round: You cannot take values from Playmode back to Edit Mode.
        //-> Moreover there is the other case where you want to disable Domain Reload for some other reason but still want a cleared empty chart when you enter Playmode. In such cases you have to manually clear the charts (e.g. in the "Start()" function of a MonoBehaviour) by using the "Clear()" functions of the charts.

        public enum RotationSource
        {
            screen,
            screen_butVerticalInWorldSpace,
            userDefinedFixedRotation
        }

        public RotationSource rotationSource = RotationSource.screen;

        public Quaternion fixedRotation = Quaternion.identity;
        private Quaternion internalRotation = Quaternion.identity; //not documented. Users should use "fixedRotation"
        public Quaternion InternalRotation
        {
            //This is the internally used rotation. "fixedRotation" does nothing but getting filled into this before drawing if "rotationSource=userDefinedFixedRotation"
            get { return internalRotation; }
            set
            {
                internalRotation = value;
                chartPlane = new InternalDXXL_Plane(Position_worldspace, internalRotation * Vector3.forward);
                xAxis.ChartUpdatesAxisDirectionVectors(internalRotation);
                yAxis.ChartUpdatesAxisDirectionVectors(internalRotation);
            }
        }

        private Vector3 position_worldspace = Vector3.zero;
        public Vector3 Position_worldspace
        {
            get
            {
                if (internal_indexNumberOfPremadeChart == (-1))
                {
                    return position_worldspace;
                }
                else
                {
                    return DrawCharts.GetAutoLayoutedPositionOfPremadeLineChart(internal_indexNumberOfPremadeChart);
                }
            }
            set
            {
                position_worldspace = value;
                internal_indexNumberOfPremadeChart = -1; //-> if a user sets the position, then the chart is not affected anymore by the automatic layouting of the premade charts.
            }
        }

        static float minChartSize = 0.01f;
        public float Width_inWorldSpace
        {
            get { return xAxis.Length_inWorldSpace; }
            set { xAxis.ChartUpdatesAxisLength(Mathf.Max(minChartSize, value)); }
        }

        public float Height_inWorldSpace
        {
            get { return yAxis.Length_inWorldSpace; }
            set { yAxis.ChartUpdatesAxisLength(Mathf.Max(minChartSize, value)); }
        }

        static Vector2 default_position_inCamViewportspace = new Vector2(0.15f, 0.2f);
        public Vector2 position_inCamViewportspace = default_position_inCamViewportspace;
        private float width_relToCamViewport = 0.7f;
        public float Width_relToCamViewport
        {
            get { return width_relToCamViewport; }
            set { width_relToCamViewport = Mathf.Clamp(value, 0.01f, 10.0f); }
        }

        private float height_relToCamViewportHeight = 0.6f;
        public float Height_relToCamViewportHeight
        {
            get { return height_relToCamViewportHeight; }
            set { height_relToCamViewportHeight = Mathf.Clamp(value, 0.01f, 10.0f); }
        }

        public ChartAxis xAxis;
        public ChartAxis yAxis;
        public ChartLines lines;
        public string name = null;
        public Color color = DrawBasics.defaultColor;
        public bool drawValuesOutsideOfChartArea = false;

        public float LuminanceOfLineColors
        {
            get { return lines.LuminanceOfLineColors; }
            set { lines.LuminanceOfLineColors = value; }
        }

        public ChartLine.LineConnectionsType default_lineConnectionsType = ChartLine.LineConnectionsType.straightFromPointToPoint; //is also set by "Set_lineConnectionsType"(link), but there it gets changed also for existing lines.
        public ChartLine.DataPointVisualization default_dataPointVisualization = ChartLine.DataPointVisualization.invisible;//is also set by "Set_dataPointVisualization"(link), but there it gets changed also for existing lines.
        public ChartLine.NamePosition default_lineNamePosition = ChartLine.NamePosition.dynamicallyMoving_atLineEnd_towardsRight;//is also set by "SetLineNamesPositions"(link), but there it gets changed also for existing lines.
        public float default_lineNameText_sizeScaleFactor = 1.0f;//is also set by "SetLineNamesSize"(link), but there it gets changed also for existing lines.
        public float default_alpha_ofHighlighterForMostCurrentValue_xDim = 0.3f;//is also set by "Set_alpha_ofHighlighterForMostCurrentValue_xDim"(link), but there it gets changed also for existing lines.
        public float default_alpha_ofHighlighterForMostCurrentValue_yDim = 0.3f;//is also set by "Set_alpha_ofHighlighterForMostCurrentValue_yDim"(link), but there it gets changed also for existing lines.
        public bool default_displayDeltaAtHighlighterForMostCurrentValue = true;//is also set by "Set_displayDeltaAtHighlighterForMostCurrentValue"(link), but there it gets changed also for existing lines.
        public float default_alpha_ofMaxiumumYValueMarker = 0.3f;//is also set by "Set_alpha_ofMaxiumumYValueMarker"(link), but there it gets changed also for existing lines.
        public float default_alpha_ofMinimumYValueMarker = 0.3f;//is also set by "Set_alpha_ofMinimumYValueMarker"(link), but there it gets changed also for existing lines.
        public bool default_markAllYMaximumTurningPoints = false;//is also set by "Set_markAllYMaximumTurningPoints"(link), but there it gets changed also for existing lines.
        public bool default_markAllYMinimumTurningPoints = false;//is also set by "Set_markAllYMinimumTurningPoints"(link), but there it gets changed also for existing lines.
        public float default_alpha_ofVerticalAreaFillLines = 0.0f;//is also set by "Set_alpha_ofVerticalAreaFillLines"(link), but there it gets changed also for existing lines.
        public float default_SizeOfPoints_relToChartHeight = 0.02f;//is also set by "Set_SizeOfPoints_relToChartHeight"(link), but there it gets changed also for existing lines.
        public float default_lineWidth_relToChartHeight = 0.0f;//is also set by "Set_lineWidth_relToChartHeight"(link), but there it gets changed also for existing lines.
        public float default_pointVisualisationLineWidth_relToChartHeight = 0.0f;//is also set by "Set_pointVisualisationLineWidth_relToChartHeight"(link), but there it gets changed also for existing lines.
        public bool autoFlipAllText_toFitObsererCamera = true;

        public int internal_indexNumberOfPremadeChart = -1; //-> this is used internally for the automatic layout of the premade chart, whose position hasn't been explicitly set by the user.

#if UNITY_EDITOR
        bool theMostCurrentChartDrawing_hasBeenMadeInScreenspace = false;
#endif
        Camera cameraUsedByMostCurrentScreenspaceDrawing;
        bool theMostCurrentScreenspaceDrawing_definedChartWidth_relToCamWidth = true;
        bool theMostCurrentChartDrawing_wasDrawnWithConfigOf_hiddenByNearerObjects = true;

        public DataComponentsThatAreDrawn dataComponentsThatAreDrawn = new DataComponentsThatAreDrawn();
        public bool displayHighlightingOfMostCurrentValues_forLinesFromLists = false;
        private bool isEmptyWithNoLinesToDraw;
        public bool IsEmptyWithNoLinesToDraw
        {
            get { return isEmptyWithNoLinesToDraw; }
            set { Debug.LogError("Setting 'IsEmptyWithNoLinesToDraw' manually is not supported."); }
        }

        private float scaleFactor_forChartNameTextSize = 1.0f;
        public float ScaleFactor_forChartNameTextSize
        {
            get { return scaleFactor_forChartNameTextSize; }
            set { scaleFactor_forChartNameTextSize = Mathf.Max(value, 0.02f); }
        }

        /// other:
        public InternalDXXL_Plane chartPlane = new InternalDXXL_Plane(Vector3.zero, Vector3.forward);
        Vector3 posOfOriginOfChartSpace_inWorldSpace;
        int xPos_currentManualIncrementValue = 0;
        List<PointOfInterest> pointsOfInterest = new List<PointOfInterest>();
        PointOfInterest pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheLeftSide;
        public PointOfInterest pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheRightSide;

        private int maxDisplayedPointOfInterestTextBoxesPerSide = 6;
        public int MaxDisplayedPointOfInterestTextBoxesPerSide // Some points of interest display a text box with explantion text in the top corners of the chart. In some unforeseen situation, for example if many invalid float values are added as data points, the number of text boxes that notify you of these invalid float values can rapidly grow and as a result slow down the Editor performance. Therefore "MaxDisplayedPointOfInterestTextBoxesPerSide" limits the number of displayed text boxes. If there are more text boxes then one additional text box is displayed which communicates how many text boxes are hidden.
        {
            get { return maxDisplayedPointOfInterestTextBoxesPerSide; }
            set { maxDisplayedPointOfInterestTextBoxesPerSide = Mathf.Max(0, value); }
        }
        InternalDXXL_ChartToCSVfileWriter chartToCSVfileWriter;
        public float overallMinXValue_includingHiddenLines; //not contained in docs //see also "lines.GetLowestXValueOfAllLines()", which is similar, but ignores hidden lines
        public float overallMaxXValue_includingHiddenLines; //not contained in docs //see also "lines.GetHighestXValueOfAllLines()", which is similar, but ignores hidden lines
        public float overallMinYValue_includingHiddenLines; //not contained in docs //see also "lines.GetLowestYValueOfAllLines()", which is similar, but ignores hidden lines
        public float overallMaxYValue_includingHiddenLines; //not contained in docs //see also "lines.GetHighestYValueOfAllLines()", which is similar, but ignores hidden lines

        public bool drawRGBColorUnderlayForColorGraphs = true; //The alpha value may be displayed to high because adjacent color lines overlay each other.


        public ChartDrawing(string chartName = null)
        {
            name = chartName;

            xAxis = new ChartAxis(Vector3.right, this, UtilitiesDXXL_Math.Dimension.x);
            yAxis = new ChartAxis(Vector3.up, this, UtilitiesDXXL_Math.Dimension.y);
            xAxis.theOtherAxis = yAxis;
            yAxis.theOtherAxis = xAxis;
            ResetOverallMinMaxValues();
            lines = new ChartLines(this);
            Create_pointsOfInterest_thatCommunicateTheHiddenPointsOfInterest();
        }

        public void Clear()
        {
            lines.Clear();
            xPos_currentManualIncrementValue = 0;
            xAxis.Clear();
            yAxis.Clear();
            ResetOverallMinMaxValues();
            DeletePointsOfInterestOnClear();
        }

        public void Draw(float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            //is ignored as long as a chart inspection gameobject (created via "CreateChartInspectionGameobject()") exists, because the chart inspection component will then take care of the chart drawing by itself.
            if (chartInspector_component == null)
            {
                Internal_Draw(durationInSec, hiddenByNearerObjects);
            }
        }

        public void Internal_Draw(float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            DXXLWrapperForUntiysBuildInDrawLines.currentlyDrawingChart = this;

            ApplyInternalRotation(); //-> "DXXLWrapperForUntiyDebugDraw.CheckIfDrawingIsCurrentlySkipped" uses the here applied rotation already in it's fallback

            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            DrawChartName(durationInSec, hiddenByNearerObjects);
            isEmptyWithNoLinesToDraw = !lines.HasAtLeastOneDrawnLineWithAtLeastOneValidValue();
            TryDrawFallbackForEmptyChart(durationInSec, hiddenByNearerObjects);
            RecalcAxisScaling();
            Vector3 chartAnchorPositionInWorldSpace_shiftedAlongChartsXAxis_toOriginOfChartSpace = Position_worldspace - xAxis.AxisVector_normalized_inWorldSpace * xAxis.ValueMarkingTheLowerAxisEnd_convertedToUnitsOfTheUnwarpedUnscaledWorldSpace;
            posOfOriginOfChartSpace_inWorldSpace = chartAnchorPositionInWorldSpace_shiftedAlongChartsXAxis_toOriginOfChartSpace - yAxis.AxisVector_normalized_inWorldSpace * yAxis.ValueMarkingTheLowerAxisEnd_convertedToUnitsOfTheUnwarpedUnscaledWorldSpace;
            xAxis.Draw(chartPlane, durationInSec, hiddenByNearerObjects);
            DXXLWrapperForUntiysBuildInDrawLines.currentlyDrawingChart = this;
            yAxis.Draw(chartPlane, durationInSec, hiddenByNearerObjects);
            DXXLWrapperForUntiysBuildInDrawLines.currentlyDrawingChart = this;
            TryDrawRGBColorUnderlay(xAxis.ValueMarkingLowerEndOfTheAxis, xAxis.ValueMarkingUpperEndOfTheAxis, yAxis.ValueMarkingLowerEndOfTheAxis, yAxis.ValueMarkingUpperEndOfTheAxis, durationInSec, hiddenByNearerObjects);
            lines.Draw(chartPlane, durationInSec, hiddenByNearerObjects);
            DXXLWrapperForUntiysBuildInDrawLines.currentlyDrawingChart = this;
            DrawPointsOfInterest(durationInSec, hiddenByNearerObjects);
            DXXLWrapperForUntiysBuildInDrawLines.currentlyDrawingChart = null;
#if UNITY_EDITOR
            theMostCurrentChartDrawing_hasBeenMadeInScreenspace = false;
#endif
            theMostCurrentChartDrawing_wasDrawnWithConfigOf_hiddenByNearerObjects = hiddenByNearerObjects;
        }

        public void DrawScreenspace(bool chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight = true, float durationInSec = 0.0f)
        {
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "ChartDrawing.DrawScreenspace") == false) { return; }
            DrawScreenspace(automaticallyFoundCamera, chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight, durationInSec);
        }

        public void DrawScreenspace(Camera targetCamera, bool chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

            bool skipScreenspaceSheduling_dueTo_isFirstDrawOfScreenspaceDrawing = Get_skipScreenspaceSheduling_dueTo_isFirstDrawOfScreenspaceDrawing();
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem && (skipScreenspaceSheduling_dueTo_isFirstDrawOfScreenspaceDrawing == false))
            {
                DrawXXL_LinesManager.instance.listOfSheduled_DrawScreenspaceChart.Add(new DrawScreenspaceChart(targetCamera, chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight, durationInSec, this));
                return;
            }

            if (chartInspector_component == null)
            {
                Internal_DrawScreenspace(targetCamera, chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight, durationInSec);
            }
        }

        bool Get_skipScreenspaceSheduling_dueTo_isFirstDrawOfScreenspaceDrawing()
        {
#if UNITY_EDITOR
            //This is for this case:
            //-> After a call to "DrawScreenspace()" "CreateChartInspectionGameobject()" is called, which expects a correctly set "theMostCurrentChartDrawing_hasBeenMadeInScreenspace".
            //-> The delay from "noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem" prevents "theMostCurrentChartDrawing_hasBeenMadeInScreenspace" (and other fields) from beeing correctly set for the chartInspectorComponent.
            //This hack could lead to problems in this case:
            //-> Multiple charts are drawn to the scene - some in screenspace and some in worldspace.
            //-> The "theMostCurrentChartDrawing_hasBeenMadeInScreenspace" would toggle to and from all the time.
            //-> Expected negative side effect: Screenspace charts could ongoingly be drawn with a spacial delay to moving cameras..
            return (theMostCurrentChartDrawing_hasBeenMadeInScreenspace == false);
#else
            return false;
#endif
        }

        public void Internal_DrawScreenspace(Camera targetCamera, bool chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(targetCamera, "targetCamera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(targetCamera)) { return; }

            cameraUsedByMostCurrentScreenspaceDrawing = targetCamera;
            theMostCurrentScreenspaceDrawing_definedChartWidth_relToCamWidth = chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight;

            Vector3 chartPosition_beforeScreenspaceDrawing = Position_worldspace;
            Quaternion fixedChartRotation_beforeScreenspaceDrawing = fixedRotation;
            float chartWidthWorldspace_beforeScreenspaceDrawing = Width_inWorldSpace;
            float chartHeigthWorldspace_beforeScreenspaceDrawing = Height_inWorldSpace;
            RotationSource rotationSource_beforeScreenspaceDrawing = rotationSource;
            autoFlipAllText_toFitObsererCamera = false;

            try
            {
                UtilitiesDXXL_ChartDrawing.SetPosRotScaleOfChart_toScreenspace(this, targetCamera, chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight, false);
                Internal_Draw(durationInSec, false);
            }
            catch { }

            Position_worldspace = chartPosition_beforeScreenspaceDrawing;
            fixedRotation = fixedChartRotation_beforeScreenspaceDrawing;
            Width_inWorldSpace = chartWidthWorldspace_beforeScreenspaceDrawing;
            Height_inWorldSpace = chartHeigthWorldspace_beforeScreenspaceDrawing;
            rotationSource = rotationSource_beforeScreenspaceDrawing;
            autoFlipAllText_toFitObsererCamera = true;
#if UNITY_EDITOR
            theMostCurrentChartDrawing_hasBeenMadeInScreenspace = true;
#endif
        }

        public void ApplyInternalRotation()
        {
            Vector3 observerCamForward_normalized;
            Vector3 observerCamUp_normalized;
            Vector3 observerCamRight_normalized;
            Vector3 cam_to_lineCenter;

            switch (rotationSource)
            {
                case RotationSource.screen:
                    UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter, Position_worldspace, Vector3.zero, null);
                    InternalRotation = Quaternion.LookRotation(observerCamForward_normalized, observerCamUp_normalized);
                    break;
                case RotationSource.screen_butVerticalInWorldSpace:
                    UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_lineCenter, Position_worldspace, Vector3.zero, null);
                    UtilitiesDXXL_LineAmplitudeAndTextDirCalculation.GetUpAndTextDir_withoutCallerSpecifiedPreference_independentFromTooShortLineDir_alignedVertical(out Vector3 chartUp_normalized, out Vector3 chartRight_normalized, observerCamForward_normalized, observerCamUp_normalized, observerCamRight_normalized, cam_to_lineCenter);
                    Vector3 chartForward_normalized = Vector3.Cross(chartRight_normalized, chartUp_normalized);
                    InternalRotation = Quaternion.LookRotation(chartForward_normalized, chartUp_normalized);
                    break;
                case RotationSource.userDefinedFixedRotation:
                    InternalRotation = fixedRotation;
                    break;
                default:
                    InternalRotation = fixedRotation;
                    Debug.LogError("rotationSource of '" + rotationSource + "' not implemented.");
                    break;
            }
        }

        void RecalcAxisScaling()
        {
            //-> this order flipping should only concern cases where one axis has "dynamic_encapsulateAllValues_butOnlyThoseInsideOfOtherAxisDisplay" scaling and the other axis has a fixed scaling type, because "dynamic_encapsulateAllValues_butOnlyThoseInsideOfOtherAxisDisplay" needs the axis ends of the fixed axis and therefore depends on being called afterwards.
            //-> for other cases it shouldn't make any difference

            if (xAxis.HasAFixedScalingType())
            {
                xAxis.RecalcScaling();
                yAxis.RecalcScaling();
            }
            else
            {
                yAxis.RecalcScaling();
                xAxis.RecalcScaling();
            }
        }

        public void AddValue(float yValueOfNewDataPoint, string nameOfReceivingLine)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.GetUsermadeLine(nameOfReceivingLine, true).AddValue(yValueOfNewDataPoint);
            }
        }

        public void AddValue(float yValueOfNewDataPoint)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.premadeLine_float.AddValue(yValueOfNewDataPoint);
            }
        }

        public void AddValue(int yValueOfNewDataPoint)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.premadeLine_int.AddValue((float)yValueOfNewDataPoint);
            }
        }

        public void AddValue(Vector2 yValueOfNewDataPoint)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.premadeLine_vector2_x.AddValue(yValueOfNewDataPoint.x);
                lines.premadeLine_vector2_y.AddValue(yValueOfNewDataPoint.y);
            }
        }

        public void AddValue(Vector3 yValueOfNewDataPoint)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.premadeLine_vector3_x.AddValue(yValueOfNewDataPoint.x);
                lines.premadeLine_vector3_y.AddValue(yValueOfNewDataPoint.y);
                lines.premadeLine_vector3_z.AddValue(yValueOfNewDataPoint.z);
            }
        }

        public void AddValue(Vector4 yValueOfNewDataPoint)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.premadeLine_vector4_x.AddValue(yValueOfNewDataPoint.x);
                lines.premadeLine_vector4_y.AddValue(yValueOfNewDataPoint.y);
                lines.premadeLine_vector4_z.AddValue(yValueOfNewDataPoint.z);
                lines.premadeLine_vector4_w.AddValue(yValueOfNewDataPoint.w);
            }
        }

        public void AddValue(Color yValueOfNewDataPoint)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.premadeLine_color_r.AddValue(yValueOfNewDataPoint.r);
                lines.premadeLine_color_g.AddValue(yValueOfNewDataPoint.g);
                lines.premadeLine_color_b.AddValue(yValueOfNewDataPoint.b);
                lines.premadeLine_color_a.AddValue(yValueOfNewDataPoint.a);
            }
        }

        public void AddValue(Quaternion yValueOfNewDataPoint)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.premadeLine_rotation_eulerX.AddValue(yValueOfNewDataPoint.eulerAngles.x);
                lines.premadeLine_rotation_eulerY.AddValue(yValueOfNewDataPoint.eulerAngles.y);
                lines.premadeLine_rotation_eulerZ.AddValue(yValueOfNewDataPoint.eulerAngles.z);
            }
        }

        public void AddValue(bool yValueOfNewDataPoint)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.premadeLine_bool.AddValue(yValueOfNewDataPoint);
            }
        }

        public void AddValue(GameObject yValueOfNewDataPoint)
        {
            if (yValueOfNewDataPoint == null)
            {
                Debug.LogError("Chart: Cannot add value, because GameObject is null.");
                return;
            }
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                AddValue(yValueOfNewDataPoint.transform);
            }
        }

        public void AddValue(Transform yValueOfNewDataPoint)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.AddValue(yValueOfNewDataPoint);
            }
        }

        public void AddXYValue(Vector2 xyValueOfNewDataPoint, string nameOfReceivingLine)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.GetUsermadeLine(nameOfReceivingLine, true).AddXYValue(xyValueOfNewDataPoint);
            }
        }

        public void AddXYValue(Vector2 xyValueOfNewDataPoint)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.premadeLine_float.AddXYValue(xyValueOfNewDataPoint);
            }
        }

        public void AddXYValue(float xValue, float yValue, string nameOfReceivingLine)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.GetUsermadeLine(nameOfReceivingLine, true).AddXYValue(xValue, yValue);
            }
        }

        public void AddXYValue(float xValue, float yValue)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.premadeLine_float.AddXYValue(xValue, yValue);
            }
        }

        public void AddXYValue(float xValue, int yValue)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.premadeLine_int.AddXYValue(xValue, (float)yValue);
            }
        }

        public void AddXYValue(float xValue, bool yValue)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.premadeLine_bool.AddXYValue(xValue, yValue);
            }
        }

        public void AddValues_eachIndexIsALine(List<float> yValues)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.AddValuesFromList_toListWithFloatLines(yValues);
            }
        }

        public void AddValues_eachIndexIsALine(float[] yValues)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.AddValuesFromArray_toListWithFloatLines(yValues);
            }
        }

        public void AddValues_eachIndexIsALine(List<int> yValues)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.AddValuesFromList_toListWithIntLines(yValues);
            }
        }

        public void AddValues_eachIndexIsALine(int[] yValues)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.AddValuesFromArray_toListWithIntLines(yValues);
            }
        }

        public void AddValues_eachIndexIsALine(List<Vector2> yValues)
        {
            if (yValues == null)
            {
                Debug.LogError("Chart: Cannot add values, because list is null.");
                return;
            }

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.AddValuesFromList_toListWithVector2Lines(yValues);
            }
        }

        public void AddValues_eachIndexIsALine(Vector2[] yValues)
        {
            if (yValues == null)
            {
                Debug.LogError("Chart: Cannot add values, because array is null.");
                return;
            }

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.AddValuesFromArray_toListWithVector2Lines(yValues);
            }
        }

        public void AddValues_eachIndexIsALine(List<Vector3> yValues)
        {
            if (yValues == null)
            {
                Debug.LogError("Chart: Cannot add values, because list is null.");
                return;
            }

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.AddValuesFromList_toListWithVector3Lines(yValues);
            }
        }

        public void AddValues_eachIndexIsALine(Vector3[] yValues)
        {
            if (yValues == null)
            {
                Debug.LogError("Chart: Cannot add values, because array is null.");
                return;
            }

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.AddValuesFromArray_toListWithVector3Lines(yValues);
            }
        }

        public void AddValues_eachIndexIsALine(List<Quaternion> yValues)
        {
            if (yValues == null)
            {
                Debug.LogError("Chart: Cannot add values, because list is null.");
                return;
            }

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.AddValuesFromList_toListWithQuaternionLines(yValues);
            }
        }

        public void AddValues_eachIndexIsALine(Quaternion[] yValues)
        {
            if (yValues == null)
            {
                Debug.LogError("Chart: Cannot add values, because array is null.");
                return;
            }

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.AddValuesFromArray_toListWithQuaternionLines(yValues);
            }
        }

        public void AddValues_eachIndexIsALine(List<bool> yValues)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.AddValuesFromList_toListWithBoolLines(yValues);
            }
        }

        public void AddValues_eachIndexIsALine(bool[] yValues)
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.AddValuesFromArray_toListWithBoolLines(yValues);
            }
        }

        public void AddValues_eachIndexIsALine(List<GameObject> yValues)
        {
            if (yValues == null)
            {
                Debug.LogError("Chart: Cannot add values, because list is null.");
                return;
            }

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.AddValuesFromList_toListWithGameobjectLines(yValues);
            }
        }

        public void AddValues_eachIndexIsALine(GameObject[] yValues)
        {
            if (yValues == null)
            {
                Debug.LogError("Chart: Cannot add values, because array is null.");
                return;
            }

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.AddValuesFromArray_toListWithGameobjectLines(yValues);
            }
        }

        public void AddValues_eachIndexIsALine(List<Transform> yValues)
        {
            if (yValues == null)
            {
                Debug.LogError("Chart: Cannot add values, because list is null.");
                return;
            }

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.AddValuesFromList_toListWithTransformLines(yValues);
            }
        }

        public void AddValues_eachIndexIsALine(Transform[] yValues)
        {
            if (yValues == null)
            {
                Debug.LogError("Chart: Cannot add values, because array is null.");
                return;
            }

            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled) //-> preventing memory leaks in builds if someone forgot to remove is "AddValue()" calls
            {
                lines.AddValuesFromArray_toListWithTransformLines(yValues);
            }
        }

        public ChartLine AddLine(string name, Color color = default(Color), bool newLineRepresentsBoolValues = false)
        {
            ChartLine newlyCreatedLine = lines.AddLine(name, color);
            if (newlyCreatedLine != null) { newlyCreatedLine.disableMinMaxYVisualizers_dueTo_lineRepresentsBoolValues = newLineRepresentsBoolValues; }
            return newlyCreatedLine;
        }

        public ChartLine GetUsermadeLine(string lineName, bool createLineIfItDoesntExist = false)
        {
            return lines.GetUsermadeLine(lineName, createLineIfItDoesntExist);
        }

        public void IncrementXPos(int steps = 1)
        {
            for (int i = 0; i < steps; i++)
            {
                xPos_currentManualIncrementValue++;
            }
        }

        public int GetManuallyIncrementedXPos()
        {
            return xPos_currentManualIncrementValue;
        }

        void TryDrawRGBColorUnderlay(float valueMarkingLowerEndOf_XAxis, float valueMarkingUpperEndOf_XAxis, float valueMarkingLowerEndOf_YAxis, float valueMarkingUpperEndOf_YAxis, float durationInSec, bool hiddenByNearerObjects)
        {
            if (drawRGBColorUnderlayForColorGraphs)
            {
                if (lines.premadeLine_color_r.dataPoints.Count != lines.premadeLine_color_g.dataPoints.Count) { ErrorLogForDifferentCountOfRGBAchannels(); return; }
                if (lines.premadeLine_color_r.dataPoints.Count != lines.premadeLine_color_b.dataPoints.Count) { ErrorLogForDifferentCountOfRGBAchannels(); return; }
                if (lines.premadeLine_color_r.dataPoints.Count != lines.premadeLine_color_a.dataPoints.Count) { ErrorLogForDifferentCountOfRGBAchannels(); return; }

                for (int i = 0; i < lines.premadeLine_color_r.dataPoints.Count; i++)
                {
                    bool allFourColorChannelsAreValid = (lines.premadeLine_color_r.dataPoints[i].validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid) && (lines.premadeLine_color_g.dataPoints[i].validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid) && (lines.premadeLine_color_b.dataPoints[i].validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid) && (lines.premadeLine_color_a.dataPoints[i].validState == InternalDXXL_DataPointOfChartLine.ValidState.isValid);
                    if (allFourColorChannelsAreValid)
                    {
                        lines.premadeLine_color_r.dataPoints[i].DetermineIfAndHowPointIsDrawn_forRGBColorUnderlay(valueMarkingLowerEndOf_XAxis, valueMarkingUpperEndOf_XAxis, valueMarkingLowerEndOf_YAxis, valueMarkingUpperEndOf_YAxis);
                        if (lines.premadeLine_color_r.dataPoints[i].isInsideChartsXSpan || drawValuesOutsideOfChartArea)
                        {
                            Color rgbColor = new Color(lines.premadeLine_color_r.dataPoints[i].yValue, lines.premadeLine_color_g.dataPoints[i].yValue, lines.premadeLine_color_b.dataPoints[i].yValue, lines.premadeLine_color_a.dataPoints[i].yValue);
                            Line_fadeableAnimSpeed.InternalDraw(lines.premadeLine_color_r.dataPoints[i].positionInWorldSpace_atYHeightOfLowerEndOf_rgbColorUnderlay, lines.premadeLine_color_r.dataPoints[i].positionInWorldSpace_atYHeightOfUpperEndOf_rgbColorUnderlay, rgbColor, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                        }
                    }
                }
            }
        }

        void ErrorLogForDifferentCountOfRGBAchannels()
        {
            Debug.LogError("Chart: Cannot draw rgb color underlay, because the 4 channels have differnet numbers of values. r has " + lines.premadeLine_color_r.dataPoints.Count + ", g has " + lines.premadeLine_color_g.dataPoints.Count + ", b has" + lines.premadeLine_color_b.dataPoints.Count + ", a has " + lines.premadeLine_color_a.dataPoints.Count);
        }

        public void SetLineNamesPositions(ChartLine.NamePosition newLineNamesPosition)
        {
            //overwrites the linePosition for all lines of the chart. If you want to specificy different linename positions for each line you can directly set chartLine.namePosition
            lines.SetLineNamesPositions(newLineNamesPosition);
        }

        public void SetLineNamesSize(float newLineNamesSizeScaleFactor)
        {
            //overwrites the NameText_sizeScaleFactor for all lines of the chart. If you want to specificy different NameText_sizeScaleFactor for each line you can directly set chartLine.NameText_sizeScaleFactor
            lines.SetLineNamesSize(newLineNamesSizeScaleFactor);
        }

        public void Set_lineConnectionsType(ChartLine.LineConnectionsType newLineConnectionsType)
        {
            //overwrites the lineConnectionsType for all lines of the chart. If you want to specificy different lineConnectionsTypes for each line you can directly set chartLine.lineConnectionsType
            lines.Set_lineConnectionsType(newLineConnectionsType);
        }

        public void Set_dataPointVisualization(ChartLine.DataPointVisualization newDataPointVisualization)
        {
            //overwrites the dataPointVisualization for all lines of the chart. If you want to specificy different dataPointVisualization for each line you can directly set chartLine.dataPointVisualization
            lines.Set_dataPointVisualization(newDataPointVisualization);
        }

        public void Set_alpha_ofVerticalAreaFillLines(float newAlpha)
        {
            //overwrites the alpha_ofVerticalFillAreaLines for all lines of the chart. If you want to specificy different alpha_ofVerticalFillAreaLines for each line you can directly set chartLine.alpha_ofVerticalFillAreaLines
            lines.Set_alpha_ofVerticalAreaFillLines(newAlpha);
        }

        public void Set_alpha_ofHighlighterForMostCurrentValue_xDim(float newAlpha)
        {
            //overwrites the alpha_ofHighlighterForMostCurrentValue_xDim for all lines of the chart. If you want to specificy different alpha_ofHighlighterForMostCurrentValue_xDim for each line you can directly set chartLine.alpha_ofHighlighterForMostCurrentValue_xDim
            lines.Set_alpha_ofHighlighterForMostCurrentValue_xDim(newAlpha);
        }

        public void Set_alpha_ofHighlighterForMostCurrentValue_yDim(float newAlpha)
        {
            //overwrites the alpha_ofHighlighterForMostCurrentValue_yDim for all lines of the chart. If you want to specificy different alpha_ofHighlighterForMostCurrentValue_yDim for each line you can directly set chartLine.alpha_ofHighlighterForMostCurrentValue_yDim
            lines.Set_alpha_ofHighlighterForMostCurrentValue_yDim(newAlpha);
        }

        public void Set_displayDeltaAtHighlighterForMostCurrentValue(bool doDisplay)
        {
            //overwrites the displayDeltaAtHighlighterForMostCurrentValue for all lines of the chart. If you want to specificy different displayDeltaAtHighlighterForMostCurrentValue for each line you can directly set chartLine.displayDeltaAtHighlighterForMostCurrentValue
            lines.Set_displayDeltaAtHighlighterForMostCurrentValue(doDisplay);
        }

        public void Set_alpha_ofMaxiumumYValueMarker(float newAlpha)
        {
            //overwrites the alpha_ofMaxiumumYValueMarker for all lines of the chart. If you want to specificy different alpha_ofMaxiumumYValueMarker for each line you can directly set chartLine.alpha_ofMaxiumumYValueMarker
            lines.Set_alpha_ofMaxiumumYValueMarker(newAlpha);
        }

        public void Set_alpha_ofMinimumYValueMarker(float newAlpha)
        {
            //overwrites the alpha_ofMinimumYValueMarker for all lines of the chart. If you want to specificy different alpha_ofMinimumYValueMarker for each line you can directly set chartLine.alpha_ofMinimumYValueMarker
            lines.Set_alpha_ofMinimumYValueMarker(newAlpha);
        }

        public void Set_markAllYMaximumTurningPoints(bool markEnabled)
        {
            //overwrites the markAllYMaximumTurningPoints for all lines of the chart. If you want to specificy different markAllYMaximumTurningPoints for each line you can directly set chartLine.markAllYMaximumTurningPoints
            lines.Set_markAllYMaximumTurningPoints(markEnabled);
        }

        public void Set_markAllYMinimumTurningPoints(bool markEnabled)
        {
            //overwrites the markAllYMinimumTurningPoints for all lines of the chart. If you want to specificy different markAllYMinimumTurningPoints for each line you can directly set chartLine.markAllYMinimumTurningPoints
            lines.Set_markAllYMinimumTurningPoints(markEnabled);
        }

        public void Set_SizeOfPoints_relToChartHeight(float newRelSize)
        {
            //overwrites the SizeOfPoints_relToChartHeight for all lines of the chart. If you want to specificy different SizeOfPoints_relToChartHeight for each line you can directly set chartLine.SizeOfPoints_relToChartHeight
            lines.Set_SizeOfPoints_relToChartHeight(newRelSize);
        }

        public void Set_lineWidth_relToChartHeight(float newRelSize)
        {
            //overwrites the lineWidth_relToChartHeight for all lines of the chart. If you want to specificy different lineWidth_relToChartHeight for each line you can directly set chartLine.lineWidth_relToChartHeight
            lines.Set_lineWidth_relToChartHeight(newRelSize);
        }

        public void Set_pointVisualisationLineWidth_relToChartHeight(float newRelSize)
        {
            //overwrites the pointVisualisationLineWidth_relToChartHeight for all lines of the chart. If you want to specificy different pointVisualisationLineWidth_relToChartHeight for each line you can directly set chartLine.pointVisualisationLineWidth_relToChartHeight
            lines.Set_pointVisualisationLineWidth_relToChartHeight(newRelSize);
        }

        public void SetAll_dataComponentsThatAreDrawn(DataComponentsThatAreDrawn newConfig)
        {
            //easy way of overwriting "dataComponentsThatAreDrawn" from another chart, without fiddling with single bool members.
            dataComponentsThatAreDrawn.CopyValueFromOtherConfig(newConfig);
        }

        public Vector3 ChartSpace_to_WorldSpace(Vector2 positionInChartSpace)
        {
            //This is dependent on the axis scaling which can change dynamically. Therefore the returned value fits the axis scaling of the last "chartDrawing.Draw()"(<-link)-Call. If ""ChartDrawing.Draw()"(<-link)" hasn't been called at least once this will return faulty values.
            float xPos_inUnwarpedUnscaledChartSpace = positionInChartSpace.x * xAxis.LengthConversionFactor_fromChartScaling_toWorldScaling;
            float yPos_inUnwarpedUnscaledChartSpace = positionInChartSpace.y * yAxis.LengthConversionFactor_fromChartScaling_toWorldScaling;
            return (posOfOriginOfChartSpace_inWorldSpace + xAxis.AxisVector_normalized_inWorldSpace * xPos_inUnwarpedUnscaledChartSpace + yAxis.AxisVector_normalized_inWorldSpace * yPos_inUnwarpedUnscaledChartSpace);
        }

        public bool IsInsideDrawnChartArea(Vector2 posToCheck_inChartUnits)
        {
            //if you want to check only one dimension you can use xAxis/yAxis.IsInsideDisplayedSpan
            return IsInsideDrawnChartArea(posToCheck_inChartUnits.x, posToCheck_inChartUnits.y);
        }

        public bool IsInsideDrawnChartArea(float posToCheck_x_inChartUnits, float posToCheck_y_inChartUnits)
        {
            //if you want to check only one dimension you can use xAxis/yAxis.IsInsideDisplayedSpan
            if (xAxis.IsInsideDisplayedSpan(posToCheck_x_inChartUnits))
            {
                if (yAxis.IsInsideDisplayedSpan(posToCheck_y_inChartUnits))
                {
                    return true;
                }
            }
            return false;
        }

        void Create_pointsOfInterest_thatCommunicateTheHiddenPointsOfInterest()
        {
            float position_x = 0.0f; //-> is unused
            float position_y = 0.0f; //-> is unused
            Color color = default; //-> gets continuously overwritten
            string textToDisplay = null; //-> gets continuously overwritten

            pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheLeftSide = new PointOfInterest(position_x, position_y, color, this, null, textToDisplay);
            pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheLeftSide.internal_isPOIthatCommunicatesTheHiddenPOIs = true;
            pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheLeftSide.isDeletedOnClear = false;
            pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheLeftSide.xValue.lineStyle = DrawBasics.LineStyle.invisible;
            pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheLeftSide.yValue.lineStyle = DrawBasics.LineStyle.invisible;
            pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheLeftSide.Internal_Set_isDrawnInNextPass(1); //-> this means: it will ALWAYS be drawn

            pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheRightSide = new PointOfInterest(position_x, position_y, color, this, null, textToDisplay);
            pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheRightSide.internal_isPOIthatCommunicatesTheHiddenPOIs = true;
            pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheRightSide.isDeletedOnClear = false;
            pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheRightSide.xValue.lineStyle = DrawBasics.LineStyle.invisible;
            pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheRightSide.yValue.lineStyle = DrawBasics.LineStyle.invisible;
            pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheRightSide.Internal_Set_isDrawnInNextPass(1); //-> this means: it will ALWAYS be drawn
        }

        public void AddPointOfInterest(PointOfInterest newPointOfInterest)
        {
            newPointOfInterest.chart_thisPointIsPartOf = this;
            pointsOfInterest.Add(newPointOfInterest);
        }

        public PointOfInterest AddPointOfInterest(Vector2 position, string textToDisplay = null, DrawBasics.LineStyle horizLinestyle = DrawBasics.LineStyle.invisible, DrawBasics.LineStyle vertLinestyle = DrawBasics.LineStyle.invisible, float alphaOfColor_relToChartColor = 1.0f, bool getsDeletedOnClear = true)
        {
            return AddPointOfInterest(position.x, position.y, textToDisplay, horizLinestyle, vertLinestyle, alphaOfColor_relToChartColor, getsDeletedOnClear);
        }

        public PointOfInterest AddPointOfInterest(float position_x, float position_y, string textToDisplay = null, DrawBasics.LineStyle horizLinestyle = DrawBasics.LineStyle.invisible, DrawBasics.LineStyle vertLinestyle = DrawBasics.LineStyle.invisible, float alphaOfColor_relToChartColor = 1.0f, bool getsDeletedOnClear = true)
        {
            //the function returns the new point of interest, so it can then accessed and be further modified.
            //if no text is appointed then invisible horizontal und vertical lines will automatically be forced to solid line (otherwise the point would not be visible.)

            if (textToDisplay == null || textToDisplay == "")
            {
                if (horizLinestyle == DrawBasics.LineStyle.invisible) { horizLinestyle = DrawBasics.LineStyle.solid; }
                if (vertLinestyle == DrawBasics.LineStyle.invisible) { vertLinestyle = DrawBasics.LineStyle.solid; }
            }

            Color colorOfPoint = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, alphaOfColor_relToChartColor);
            PointOfInterest newPointOfInterest = new PointOfInterest(position_x, position_y, colorOfPoint, this, null, textToDisplay);
            newPointOfInterest.isDeletedOnClear = getsDeletedOnClear;
            newPointOfInterest.xValue.lineStyle = vertLinestyle;
            newPointOfInterest.yValue.lineStyle = horizLinestyle;
            pointsOfInterest.Add(newPointOfInterest);
            return newPointOfInterest;
        }

        void DeletePointsOfInterestOnClear()
        {
            for (int i = pointsOfInterest.Count - 1; i >= 0; i--)
            {
                if (pointsOfInterest[i].isDeletedOnClear)
                {
                    pointsOfInterest.RemoveAt(i);
                }
            }
        }

        int stillAvailableTextBoxes_inUpperLeftCorner_duringPreviousDrawRun = 0;
        void DrawPointsOfInterest(float durationInSec, bool hiddenByNearerObjects)
        {
            if (isEmptyWithNoLinesToDraw == false)
            {
                float yHeightFacor_forColumnStartPos = 1.05f;

                //Points that are attached to the chart:
                UtilitiesDXXL_Math.SkewedDirection cornerOfChartWhereTextBoxIsMounted = UtilitiesDXXL_Math.SkewedDirection.upLeft;
                Vector3 next_lowAnchorPositionOfText_inWorldspace = Position_worldspace + yAxis.AxisVector_inWorldSpace * yHeightFacor_forColumnStartPos + xAxis.AxisVector_inWorldSpace * 0.0f;

                int stillAvailableTextBoxes_inUpperLeftCorner = MaxDisplayedPointOfInterestTextBoxesPerSide;
                for (int i = pointsOfInterest.Count - 1; i >= 0; i--) //-> counting DOWNWARD means: only the NEWEST textBoxes get drawn
                {
                    stillAvailableTextBoxes_inUpperLeftCorner = pointsOfInterest[i].Internal_Set_isDrawnInNextPass(stillAvailableTextBoxes_inUpperLeftCorner);
                }

                if (stillAvailableTextBoxes_inUpperLeftCorner < 0)
                {
                    //-> draw this "hiddenTextBoxes-communicating-textBox" BEFORE(=BELOW) the other textBoxes, so that it appears as the "oldest" text box: newer text boxes will be drawn higher than this
                    if (stillAvailableTextBoxes_inUpperLeftCorner != stillAvailableTextBoxes_inUpperLeftCorner_duringPreviousDrawRun) //-> saving GC.alloc by only recreating the text when something changes
                    {
                        if (stillAvailableTextBoxes_inUpperLeftCorner == (-1))
                        {
                            pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheLeftSide.text = "<size=10>...and 1 more hidden older text box.<br><br></size><size=6>See also 'ChartDrawing.maxDisplayedPointOfInterestTextBoxesPerSide'.</size>";
                        }
                        else
                        {
                            pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheLeftSide.text = "<size=10>...and " + (-stillAvailableTextBoxes_inUpperLeftCorner) + " more hidden older text boxes.<br><br></size><size=6>See also 'ChartDrawing.maxDisplayedPointOfInterestTextBoxesPerSide'.</size>";
                        }
                    }
                    stillAvailableTextBoxes_inUpperLeftCorner_duringPreviousDrawRun = stillAvailableTextBoxes_inUpperLeftCorner;
                    pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheLeftSide.colorOfPointerTextBox = color;
                    next_lowAnchorPositionOfText_inWorldspace = pointOfInterest_thatCommunicatesTheHiddenPointsOfInterest_onTheLeftSide.TryDraw(next_lowAnchorPositionOfText_inWorldspace, cornerOfChartWhereTextBoxIsMounted, durationInSec, hiddenByNearerObjects);
                }

                for (int i = 0; i < pointsOfInterest.Count; i++) //-> counting UPWARD for PointOfInterestBoxes on the upperLeftSide produced less pointer crossings in most cases. Though this has the disadvantage, that the "newest/oldest"-ordering is the other way round than the PointOfInterestBoxes on the upperLeftSide.
                {
                    next_lowAnchorPositionOfText_inWorldspace = pointsOfInterest[i].TryDraw(next_lowAnchorPositionOfText_inWorldspace, cornerOfChartWhereTextBoxIsMounted, durationInSec, hiddenByNearerObjects);
                }

                //Points that are attached to single lines:
                lines.DrawPointsOfInterest(yHeightFacor_forColumnStartPos, durationInSec, hiddenByNearerObjects);
            }
        }

        public PointOfInterest AddFixedHorizLine(float yPosition, string textLabel = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.dashed, float alphaOfColor_relToParent = 0.75f)
        {
            //the function returns the horizontal line as pointOfInterest, so it can then accessed and be further modified.
            //if you additionally want to display the intersectionPoints of the line with this horizonal line you can use "__" instead.

            Color colorOfLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, alphaOfColor_relToParent);
            PointOfInterest newPointOfInterest = new PointOfInterest(0.0f, yPosition, colorOfLine, this, null, null);
            newPointOfInterest.xValue.lineStyle = DrawBasics.LineStyle.invisible;
            newPointOfInterest.yValue.lineStyle = style;
            newPointOfInterest.yValue.labelText = textLabel;
            newPointOfInterest.yValue.lineExtent = DimensionOf_PointOfInterest.LineExtent.throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart;
            newPointOfInterest.isDeletedOnClear = false;
            pointsOfInterest.Add(newPointOfInterest);
            return newPointOfInterest;
        }

        public PointOfInterest AddFixedHorizLine_withPointer(float yPosition, string textInPointerBox, string textAtLine = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float alphaOfLineColor_relToParent = 0.75f)
        {
            //the function returns the horizontal line as pointOfInterest , so it can then accessed and be further modified.
            //if you additionally want to display the intersectionPoints of the line with this horizonal line you can use "AddHorizontalThresholdLine" instead.

            Color colorOfLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, alphaOfLineColor_relToParent);
            PointOfInterest newPointOfInterest = new PointOfInterest(float.NaN, yPosition, colorOfLine, this, null, textInPointerBox);
            newPointOfInterest.colorOfPointerTextBox = color;
            newPointOfInterest.drawTextBoxIfPointIsOutsideOfChartArea = true;
            newPointOfInterest.xValue.lineStyle = DrawBasics.LineStyle.invisible;
            newPointOfInterest.yValue.lineStyle = style;
            newPointOfInterest.yValue.labelText = textAtLine;
            newPointOfInterest.yValue.lineExtent = DimensionOf_PointOfInterest.LineExtent.throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart;
            newPointOfInterest.isDeletedOnClear = false;
            newPointOfInterest.forceColorOfParent = false;
            pointsOfInterest.Add(newPointOfInterest);
            return newPointOfInterest;
        }


        public PointOfInterest AddFixedVertLine(float xPosition, string textLabel = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.dashed, float alphaOfColor_relToParent = 0.75f)
        {
            //the function returns the vertical line as pointOfInterest, so it can then accessed and be further modified.
            Color colorOfLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, alphaOfColor_relToParent);
            PointOfInterest newPointOfInterest = new PointOfInterest(xPosition, 0.0f, colorOfLine, this, null, null);
            newPointOfInterest.xValue.lineStyle = style;
            newPointOfInterest.xValue.labelText = textLabel;
            newPointOfInterest.xValue.lineExtent = DimensionOf_PointOfInterest.LineExtent.throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart;
            newPointOfInterest.yValue.lineStyle = DrawBasics.LineStyle.invisible;
            newPointOfInterest.isDeletedOnClear = false;
            pointsOfInterest.Add(newPointOfInterest);
            return newPointOfInterest;
        }

        public PointOfInterest AddFixedVertLine_withPointer(float xPosition, string textInPointerBox, string textAtLine = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float alphaOfLineColor_relToParent = 0.75f)
        {
            //the function returns the vertical line as pointOfInterest, so it can then accessed and be further modified.
            Color colorOfLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, alphaOfLineColor_relToParent);
            PointOfInterest newPointOfInterest = new PointOfInterest(xPosition, float.NaN, colorOfLine, this, null, textInPointerBox);
            newPointOfInterest.colorOfPointerTextBox = color;
            newPointOfInterest.drawTextBoxIfPointIsOutsideOfChartArea = true;
            newPointOfInterest.xValue.lineStyle = style;
            newPointOfInterest.xValue.labelText = textAtLine;
            newPointOfInterest.xValue.lineExtent = DimensionOf_PointOfInterest.LineExtent.throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart;
            newPointOfInterest.yValue.lineStyle = DrawBasics.LineStyle.invisible;
            newPointOfInterest.isDeletedOnClear = false;
            newPointOfInterest.forceColorOfParent = false;
            pointsOfInterest.Add(newPointOfInterest);
            return newPointOfInterest;
        }

        public PointOfInterest AddFixedCrossOFHorizAndVertLine(Vector2 position, string textLabel_onHorizLine = null, string textLabel_onVertLine = null, string textLabel_forPointerBox = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float alphaOfColor_relToParent = 0.75f)
        {
            //the function returns the line cross as pointOfInterest, so it can then accessed and be further modified.
            return AddFixedCrossOFHorizAndVertLine(position.x, position.y, textLabel_onHorizLine, textLabel_onVertLine, textLabel_forPointerBox, style, alphaOfColor_relToParent);
        }

        public PointOfInterest AddFixedCrossOFHorizAndVertLine(float xPosition, float yPosition, string textLabel_onHorizLine = null, string textLabel_onVertLine = null, string textLabel_forPointerBox = null, DrawBasics.LineStyle style = DrawBasics.LineStyle.solid, float alphaOfColor_relToParent = 0.75f)
        {
            //the function returns the line cross as pointOfInterest, so it can then accessed and be further modified.
            Color colorOfLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, alphaOfColor_relToParent);
            PointOfInterest newPointOfInterest = new PointOfInterest(xPosition, yPosition, colorOfLine, this, null, textLabel_forPointerBox);
            newPointOfInterest.xValue.lineStyle = style;
            newPointOfInterest.yValue.lineStyle = style;
            newPointOfInterest.xValue.labelText = textLabel_onVertLine;
            newPointOfInterest.yValue.labelText = textLabel_onHorizLine;
            newPointOfInterest.xValue.lineExtent = DimensionOf_PointOfInterest.LineExtent.throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart;
            newPointOfInterest.yValue.lineExtent = DimensionOf_PointOfInterest.LineExtent.throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart;
            newPointOfInterest.isDeletedOnClear = false;
            pointsOfInterest.Add(newPointOfInterest);
            return newPointOfInterest;
        }

        public void AddHorizontalThresholdLine(float yPosition, bool lineItselfCountsToLowerArea = false, Color colorOfHorizLine = default(Color))
        {
            //if you want to add the threshold to only a single line you can use the "AddHorizontalThresholdLine"-version in "chartline"
            //If you don't want to see the intersection points and only want to draw a horizontal line that doesn't do anything you can use 'AddFixedHorizLine' instead.
            //"colorOfHorizLine": supply this if the color should deviate from the charts color
            //"lineItselfCountsToLowerArea" determines if it counts as an intersection if the line doesn't cross the threshold but runs exactly onto it.

            if (UtilitiesDXXL_Math.FloatIsInvalid(yPosition))
            {
                Debug.LogError("Cannot create threshold line at " + yPosition);
                return;
            }

            if (UtilitiesDXXL_Colors.IsDefaultColor(colorOfHorizLine))
            {
                colorOfHorizLine = UtilitiesDXXL_Colors.GetSimilarColorWithSlightlyOtherBrightnessValue(color);
            }

            PointOfInterest pointOfInterst_visualizingTheHorizThresholdLine = new PointOfInterest(0.0f, yPosition, colorOfHorizLine, this, null, null);
            pointOfInterst_visualizingTheHorizThresholdLine.isDeletedOnClear = false;
            pointOfInterst_visualizingTheHorizThresholdLine.forceColorOfParent = false;
            pointOfInterst_visualizingTheHorizThresholdLine.xValue.lineStyle = DrawBasics.LineStyle.invisible;
            pointOfInterst_visualizingTheHorizThresholdLine.yValue.lineStyle = DrawBasics.LineStyle.solid;
            pointOfInterst_visualizingTheHorizThresholdLine.yValue.labelText = "Threshold";
            pointOfInterst_visualizingTheHorizThresholdLine.yValue.drawCoordinateAsText = true;
            pointOfInterst_visualizingTheHorizThresholdLine.yValue.lineExtent = DimensionOf_PointOfInterest.LineExtent.throughWholeChart_alsoIfOtherDimensionsValueIsOutsideChart;
            pointsOfInterest.Add(pointOfInterst_visualizingTheHorizThresholdLine);

            lines.AddHorizontalThresholdToEachLine(yPosition, lineItselfCountsToLowerArea);
        }

        void DrawChartName(float durationInSec, bool hiddenByNearerObjects)
        {
            if (name != null && name != "")
            {
                float textSize = xAxis.Length_inWorldSpace * 0.04f * scaleFactor_forChartNameTextSize;
                Vector3 position = Position_worldspace + 0.5f * xAxis.AxisVector_inWorldSpace + yAxis.AxisVector_normalized_inWorldSpace * (yAxis.Length_inWorldSpace * 1.035f + textSize);
                float enclosingBox_paddingSize_relToTextSize = 0.5f;
                float autoLineBreakWidth = 0.65f * xAxis.Length_inWorldSpace;
                UtilitiesDXXL_Text.WriteFramed(name, position, color, textSize, internalRotation, DrawText.TextAnchorDXXL.LowerCenter, DrawBasics.LineStyle.solid, 0.0f, enclosingBox_paddingSize_relToTextSize, 0.0f, 0.0f, autoLineBreakWidth, autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects);
            }
        }

        void TryDrawFallbackForEmptyChart(float durationInSec, bool hiddenByNearerObjects)
        {
            if (isEmptyWithNoLinesToDraw)
            {
                string text = "<color=#adadadFF><icon=logMessage></color> This chart doesn't contain<br>  any data to draw."; //-> the two spaces are intentional
                float textSize = 1.0f; //->not relevant, because textsize gets forced by "forceTextEnlargementToThisMinWidth" respectively "forceRestrictTextSizeToThisMaxTextWidth"
                DrawText.TextAnchorDXXL textAnchor = DrawText.TextAnchorDXXL.LowerLeft;
                float forceTextEnlargementToThisMinWidth = Width_inWorldSpace;
                float forceRestrictTextSizeToThisMaxTextWidth = forceTextEnlargementToThisMinWidth;
                UtilitiesDXXL_Text.WriteFramed(text, Position_worldspace, color, textSize, internalRotation, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, forceTextEnlargementToThisMinWidth, forceRestrictTextSizeToThisMaxTextWidth, 0.0f, autoFlipAllText_toFitObsererCamera, durationInSec, hiddenByNearerObjects);
            }
        }

        public void DrawWarningForMaxLinesPerFrame()
        {
            string warningText = "<color=#e2aa00FF><size=33><icon=warning></size></color><br><color=red>Max lines exceeded<br>(see log)</color>";
            float textSize = 1.0f; //gets forced to fit chart width 
            float forceTextEnlargementToThisMinWidth = Width_inWorldSpace;
            float forceRestrictTextSizeToThisMaxTextWidth = Width_inWorldSpace;
            UtilitiesDXXL_Text.WriteFramed(warningText, Position_worldspace, color, textSize, internalRotation, DrawText.TextAnchorDXXL.LowerLeft, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, forceTextEnlargementToThisMinWidth, forceRestrictTextSizeToThisMaxTextWidth, 0.0f, autoFlipAllText_toFitObsererCamera, 0.0f, false);
        }

        void ResetOverallMinMaxValues()
        {
            overallMinXValue_includingHiddenLines = float.NaN;
            overallMaxXValue_includingHiddenLines = float.NaN;
            overallMinYValue_includingHiddenLines = float.NaN;
            overallMaxYValue_includingHiddenLines = float.NaN;
        }
#if UNITY_EDITOR
        GameObject chartInspector_gameObject = null;
#endif
        public DrawXXLChartInspector chartInspector_component = null;
        public void CreateChartInspectionGameobject(bool freezeAxesInMomentOfInspectorCreation = false, bool setSceneViewCamToChart = false, bool setChartGameobjectToTheTopOfTheHierarchy = false, bool autoSelectCreatedChartInInspector = false)
        {
#if UNITY_EDITOR
            if (chartInspector_gameObject == null)
            {
                Internal_CreateChartInspectionGameobject(freezeAxesInMomentOfInspectorCreation, setSceneViewCamToChart, autoSelectCreatedChartInInspector, setChartGameobjectToTheTopOfTheHierarchy);
            }
            else
            {
                if (chartInspector_component == null)
                {
                    DrawXXLChartInspector[] allChartInspectorComponents_thatAreAttachedToTheGameObject = chartInspector_gameObject.GetComponents<DrawXXLChartInspector>();
                    for (int i = 0; i < allChartInspectorComponents_thatAreAttachedToTheGameObject.Length; i++)
                    {
                        if (Application.isPlaying)
                        {
                            UnityEngine.Object.Destroy(allChartInspectorComponents_thatAreAttachedToTheGameObject[i]);
                        }
                        else
                        {
                            UnityEngine.Object.DestroyImmediate(allChartInspectorComponents_thatAreAttachedToTheGameObject[i]);
                        }
                    }
                    Internal_CreateChartInspectionGameobject(freezeAxesInMomentOfInspectorCreation, setSceneViewCamToChart, autoSelectCreatedChartInInspector, setChartGameobjectToTheTopOfTheHierarchy);
                }
                else
                {
                    //Debug.Log("Calling 'CreateChartInspectionGameobject()' has no effect because there is already a chart inspection component.");
                }
            }
#endif
        }

        void Internal_CreateChartInspectionGameobject(bool freezeAxes, bool setSceneViewCamToChart, bool autoSelectCreatedChartInInspector, bool setChartGameobjectToTheTopOfTheHierarchy)
        {
#if UNITY_EDITOR
            xAxis.SetAxisScalingDuringInspectionComponentPhases(xAxis.ValueMarkingLowerEndOfTheAxis, xAxis.ValueMarkingUpperEndOfTheAxis);
            yAxis.SetAxisScalingDuringInspectionComponentPhases(yAxis.ValueMarkingLowerEndOfTheAxis, yAxis.ValueMarkingUpperEndOfTheAxis);

            string gameobjectName;
            if (name == null || name == "")
            {
                gameobjectName = "Draw XXL Chart Inspector";
            }
            else
            {
                gameobjectName = "Chart: " + name + "";
            }

            if (chartInspector_gameObject == null)
            {
                chartInspector_gameObject = new GameObject(gameobjectName);
            }

            chartInspector_component = chartInspector_gameObject.AddComponent<DrawXXLChartInspector>();
            lines.InitInspectionViaComponent();

            chartInspector_component.hasBeenManuallyCreated = false;
            chartInspector_component.hasBeenCreatedOutsidePlaymode = !Application.isPlaying;
            chartInspector_component.theChartIsDrawnInScreenspace = theMostCurrentChartDrawing_hasBeenMadeInScreenspace;
            chartInspector_component.screenSpaceTargetCamera = cameraUsedByMostCurrentScreenspaceDrawing;
            chartInspector_component.chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight = theMostCurrentScreenspaceDrawing_definedChartWidth_relToCamWidth;
            chartInspector_component.nonScreenspaceDrawing_happensWith_drawConfigOf_hiddenByNearerObjects = theMostCurrentChartDrawing_wasDrawnWithConfigOf_hiddenByNearerObjects;
            chartInspector_component.AssignChart(this);
            chartInspector_component.curr_luminanceOfLineColors_accordingToSlider = LuminanceOfLineColors;
            chartInspector_component.prev_luminanceOfLineColors_accordingToSlider = LuminanceOfLineColors;
            chartInspector_component.prev_luminanceOfLineColors_accordingToChartSetting = LuminanceOfLineColors;
            chartInspector_component.lineNamePositions = lines.GetAUsedLineNamePosition(false);
            chartInspector_component.lineNames_sizeScaleFactor = lines.GetAUsedLineNameSizeSclaeFactor(false);

            if (setSceneViewCamToChart && (chartInspector_component.theChartIsDrawnInScreenspace == false)) { chartInspector_component.TrySetSceneViewCamToChart_dueToButtonClick(); }
            if (autoSelectCreatedChartInInspector) { UnityEditor.Selection.SetActiveObjectWithContext(chartInspector_gameObject, UnityEditor.Selection.activeContext); }
            if (setChartGameobjectToTheTopOfTheHierarchy) { chartInspector_gameObject.transform.SetAsFirstSibling(); }
            if (freezeAxes == false) { chartInspector_component.alwaysEncapsulateAllValues = true; }
#endif
        }

        public void ExportToCSVfile(string fileName = null)
        {
            if (chartToCSVfileWriter == null) { chartToCSVfileWriter = new InternalDXXL_ChartToCSVfileWriter(); }
            chartToCSVfileWriter.ExportToCSVfile(lines, fileName);
        }

        public Vector3 GetCenterPos()
        {
            //returns the center position of the chart (in worldspace) as opposed to the low left origin which "Position_worldspace" returns.
            return (Position_worldspace + 0.5f * (xAxis.AxisVector_inWorldSpace + yAxis.AxisVector_inWorldSpace));
        }

        public float GetDiagonalSize()
        {
            return (xAxis.AxisVector_inWorldSpace + yAxis.AxisVector_inWorldSpace).magnitude;
        }

        public Vector3 Get_unified45degAxis_forHandleSliders_normalized()
        {
            return (-(xAxis.AxisVector_normalized_inWorldSpace + yAxis.AxisVector_normalized_inWorldSpace) * UtilitiesDXXL_Math.inverseSqrtOf2_precalced);
        }

        public float Get_unified45degAxis_length()
        {
            return 3.6f * xAxis.Get_fixedConeLength_forBothAxisVectors();
        }

    }

}
