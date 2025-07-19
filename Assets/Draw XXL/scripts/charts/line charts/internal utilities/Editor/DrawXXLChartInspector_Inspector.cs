namespace DrawXXL
{
    using UnityEngine;
    using System;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(DrawXXLChartInspector))]
    public class DrawXXLChartInspector_Inspector : Editor
    {
        DrawXXLChartInspector theDrawXXLChartInspector_unserializedMonoB;

        GUIContent checkmarkSymbol;
        GUIContent crossSymbol;
        GUIContent magnifierGlassSymbol_atXAxis;
        GUIContent magnifierGlassSymbol_atYAxis;
        GUIContent magnifierGlassSymbol_forBothAxes;
        GUIContent handSymbol_atXAxis;
        GUIContent handSymbol_atYAxis;
        GUIContent handSymbol_forBothAxes;

        float travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_zoomXAxisHandle = 0.0f;
        float travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_zoomYAxisHandle = 0.0f;
        float travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_forZoomBothAxesHandle = 0.0f;
        float travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_dragXAxisHandle = 0.0f;
        float travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_dragYAxisHandle = 0.0f;
        float travelledWorldSpaceDistanceAlongChartsXDirection_sinceMouseDown_for_unifiedDragHandle = 0.0f;
        float travelledWorldSpaceDistanceAlongChartsYDirection_sinceMouseDown_for_unifiedDragHandle = 0.0f;

        float sizeFactor_forBacksnappingConeCaps = 1.0f;
        float sizeFactor_forBacksnappingCylinderCaps = 0.8f;
        float sizeFactor_forBacksnappingSphereCaps = 1.0f;

        float distanceFromAnchorPosFactor_forBacksnappingConeCaps = -0.5f;
        float distanceFromAnchorPosFactor_forBacksnappingCylinderCaps = -1.7f;

        float sizeFactor_forCheckmarkIcon = 0.5f;
        float sizeFactor_forCrossIcon = 0.5f;
        float sizeFactor_forHandIcon_onConesAndSpheres = 0.42f;
        float sizeFactor_forMagnifierGlassIcons_onCylinders = 0.5f;

        void OnEnable()
        {
            theDrawXXLChartInspector_unserializedMonoB = (DrawXXLChartInspector)target;

            //credits to https://github.com/Zxynine/UnityEditorIcons
            string tooltip_forCheckbox = "If this is selected then the axis scaling will be automatically updated, so that all active lines are always fully displayed, also if new values are added." + Environment.NewLine + Environment.NewLine + "This setting gets auto-deactivated as soon as you drag one of the manual scale sliders.";
            checkmarkSymbol = EditorGUIUtility.IconContent("FilterSelectedOnly@2x", tooltip_forCheckbox);
            crossSymbol = EditorGUIUtility.IconContent("winbtn_win_close@2x", tooltip_forCheckbox);
            magnifierGlassSymbol_atXAxis = EditorGUIUtility.IconContent("ViewToolZoom@2x", "Drag this to zoom only the X axis.");
            magnifierGlassSymbol_atYAxis = EditorGUIUtility.IconContent("ViewToolZoom@2x", "Drag this to zoom only the Y axis.");
            magnifierGlassSymbol_forBothAxes = EditorGUIUtility.IconContent("ViewToolZoom@2x", "Drag this to zoom both axes.");
            handSymbol_atXAxis = EditorGUIUtility.IconContent("ViewToolMove@2x", "Drag this to scroll only the X axis.");
            handSymbol_atYAxis = EditorGUIUtility.IconContent("ViewToolMove@2x", "Drag this to scroll only the Y axis.");
            handSymbol_forBothAxes = EditorGUIUtility.IconContent("ViewToolMove@2x", "Drag this to scroll both axes.");
        }

        public void OnSceneGUI()
        {
            if (theDrawXXLChartInspector_unserializedMonoB.hideHandles_andInsteadUseInspectorSlidersForZoomAndScroll == false)
            {
                theDrawXXLChartInspector_unserializedMonoB.TryNote_chartWorldSpacePosRotScale_beforeChangingItToScreenspace();
                try
                {
                    if (theDrawXXLChartInspector_unserializedMonoB.theChartIsDrawnInScreenspace) { UtilitiesDXXL_ChartDrawing.SetPosRotScaleOfChart_toScreenspace(theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo, theDrawXXLChartInspector_unserializedMonoB.screenSpaceTargetCamera, theDrawXXLChartInspector_unserializedMonoB.chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight, true); }

                    DrawCursorSliders();
                    float sizeOfHandleCaps = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.xAxis.Get_fixedConeLength_forBothAxisVectors();
                    DrawScrollSliderAsConeAtXAxis(sizeOfHandleCaps);
                    DrawScrollSliderAsConeAtYAxis(sizeOfHandleCaps);
                    DrawScrollSliderAsSphereAtUnifiedAxis(sizeOfHandleCaps);
                    DrawZoomSliderAsCylinderAtXAxis(sizeOfHandleCaps);
                    DrawZoomSliderAsCylinderAtYAxis(sizeOfHandleCaps);
                    DrawZoomSliderAsCylinderAtUnifiedAxis(sizeOfHandleCaps);
                    DrawCheckboxFor_showAllValues(sizeOfHandleCaps);
                }
                catch { }
                theDrawXXLChartInspector_unserializedMonoB.TrySetBack_chartWorldSpacePosRotScale_afterUsingItInScreenspace();
            }
        }

        void DrawCursorSliders()
        {
            DrawACursorSlider(true, true);
        }

        void DrawACursorSlider(bool isAtLowerEndOfCursorsVertLine_notAtUpperEnd, bool tryOtherCursorSlider_ifThisCursorSliderDoesntReportAnyChange)
        {
            float worldSpaceDistanceAlongXAxis_measuredFromYAxis_beforeDrag = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.Width_inWorldSpace * theDrawXXLChartInspector_unserializedMonoB.curr_cursorPosition_as0to1OfChartWidth;

            EditorGUI.BeginChangeCheck();
            float worldSpaceDistanceAlongXAxis_measuredFromYAxis_afterDrag = InternalDXXL_ChartHandles.CursorSlider(isAtLowerEndOfCursorsVertLine_notAtUpperEnd, worldSpaceDistanceAlongXAxis_measuredFromYAxis_beforeDrag, theDrawXXLChartInspector_unserializedMonoB);
            bool hasChanged = EditorGUI.EndChangeCheck();

            if (hasChanged)
            {
                UpdateCursorPos0to1(worldSpaceDistanceAlongXAxis_measuredFromYAxis_afterDrag);
            }
            else
            {
                if (tryOtherCursorSlider_ifThisCursorSliderDoesntReportAnyChange)
                {
                    DrawACursorSlider(!isAtLowerEndOfCursorsVertLine_notAtUpperEnd, false);
                }
            }
        }

        void UpdateCursorPos0to1(float worldSpaceDistanceAlongXAxis_measuredFromYAxis_afterDrag)
        {
            float new_cursorPosition_as0to1OfChartWidth = worldSpaceDistanceAlongXAxis_measuredFromYAxis_afterDrag / theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.Width_inWorldSpace;
            theDrawXXLChartInspector_unserializedMonoB.ForceCursorSliders_toFitGiven0to1XPos(new_cursorPosition_as0to1OfChartWidth);
        }

        void DrawScrollSliderAsConeAtXAxis(float sizeOfHandleCaps)
        {
            float distanceFromAnchorPos = distanceFromAnchorPosFactor_forBacksnappingConeCaps * sizeOfHandleCaps;
            Vector3 sliderDirection_worldSpace_normalized = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.xAxis.AxisVector_normalized_inWorldSpace;
            Vector3 restingPosition = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.Position_worldspace + theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.xAxis.AxisVector_inWorldSpace + sliderDirection_worldSpace_normalized * distanceFromAnchorPos;
            Handles.CapFunction capFunction = Handles.ConeHandleCap;
            float capSizeScaleFactor = sizeFactor_forBacksnappingConeCaps;
            GUIContent icon = handSymbol_atXAxis;
            float iconSizeFactor = sizeFactor_forHandIcon_onConesAndSpheres;
            Vector2 iconPositionOffset_inScreenspace_relToHandleCapSize = new Vector2(-0.45f, 0.2f); //-> manually shifting the icon, so it appears centered over the handle cap.

            EditorGUI.BeginChangeCheck();
            travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_dragXAxisHandle = InternalDXXL_ChartHandles.OneDirectionalBacksnapSlider(travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_dragXAxisHandle, restingPosition, theDrawXXLChartInspector_unserializedMonoB, sliderDirection_worldSpace_normalized, capFunction, capSizeScaleFactor, icon, iconSizeFactor, iconPositionOffset_inScreenspace_relToHandleCapSize, false, false);
            bool hasChanged = EditorGUI.EndChangeCheck();

            if (hasChanged)
            {
                theDrawXXLChartInspector_unserializedMonoB.alwaysEncapsulateAllValues = false;
                theDrawXXLChartInspector_unserializedMonoB.scrollSinceMouseDown_fromOnlyXDimHandleSlider_asTravelledWorldSpaceDistanceAlongSliderDirection = travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_dragXAxisHandle;
            }
            else
            {
                if (GUIUtility.hotControl == 0)
                {
                    travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_dragXAxisHandle = 0.0f;
                    theDrawXXLChartInspector_unserializedMonoB.scrollSinceMouseDown_fromOnlyXDimHandleSlider_asTravelledWorldSpaceDistanceAlongSliderDirection = 0.0f;
                }
            }

        }

        void DrawScrollSliderAsConeAtYAxis(float sizeOfHandleCaps)
        {
            float distanceFromAnchorPos = distanceFromAnchorPosFactor_forBacksnappingConeCaps * sizeOfHandleCaps;
            Vector3 sliderDirection_worldSpace_normalized = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.yAxis.AxisVector_normalized_inWorldSpace;
            Vector3 restingPosition = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.Position_worldspace + theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.yAxis.AxisVector_inWorldSpace + sliderDirection_worldSpace_normalized * distanceFromAnchorPos;
            Handles.CapFunction capFunction = Handles.ConeHandleCap;
            float capSizeScaleFactor = sizeFactor_forBacksnappingConeCaps;
            GUIContent icon = handSymbol_atYAxis;
            float iconSizeFactor = sizeFactor_forHandIcon_onConesAndSpheres;
            Vector2 iconPositionOffset_inScreenspace_relToHandleCapSize = new Vector2(-0.23f, 0.0f); //-> manually shifting the icon, so it appears centered over the handle cap.

            EditorGUI.BeginChangeCheck();
            travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_dragYAxisHandle = InternalDXXL_ChartHandles.OneDirectionalBacksnapSlider(travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_dragYAxisHandle, restingPosition, theDrawXXLChartInspector_unserializedMonoB, sliderDirection_worldSpace_normalized, capFunction, capSizeScaleFactor, icon, iconSizeFactor, iconPositionOffset_inScreenspace_relToHandleCapSize, false, false);
            bool hasChanged = EditorGUI.EndChangeCheck();

            if (hasChanged)
            {
                theDrawXXLChartInspector_unserializedMonoB.alwaysEncapsulateAllValues = false;
                theDrawXXLChartInspector_unserializedMonoB.scrollSinceMouseDown_fromOnlyYDimHandleSlider_asTravelledWorldSpaceDistanceAlongSliderDirection = travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_dragYAxisHandle;
            }
            else
            {
                if (GUIUtility.hotControl == 0)
                {
                    travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_dragYAxisHandle = 0.0f;
                    theDrawXXLChartInspector_unserializedMonoB.scrollSinceMouseDown_fromOnlyYDimHandleSlider_asTravelledWorldSpaceDistanceAlongSliderDirection = 0.0f;
                }
            }
        }

        void DrawScrollSliderAsSphereAtUnifiedAxis(float sizeOfHandleCaps)
        {
            float distanceFromAnchorPos = 2.3f * sizeOfHandleCaps;
            Vector3 unified45degAxis_forHandleSliders_normalized = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.Get_unified45degAxis_forHandleSliders_normalized();
            Vector3 restingPosition = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.Position_worldspace + unified45degAxis_forHandleSliders_normalized * distanceFromAnchorPos;
            float capSizeScaleFactor = sizeFactor_forBacksnappingSphereCaps;
            GUIContent icon = handSymbol_forBothAxes;
            float iconSizeFactor = sizeFactor_forMagnifierGlassIcons_onCylinders;
            Vector2 iconPositionOffset_inScreenspace_relToHandleCapSize = new Vector2(-0.25f, 0.25f); //-> manually shifting the icon, so it appears centered over the handle cap.

            EditorGUI.BeginChangeCheck();
            InternalDXXL_ChartHandles.TwoDirectionalBacksnapSlider(ref travelledWorldSpaceDistanceAlongChartsXDirection_sinceMouseDown_for_unifiedDragHandle, ref travelledWorldSpaceDistanceAlongChartsYDirection_sinceMouseDown_for_unifiedDragHandle, restingPosition, theDrawXXLChartInspector_unserializedMonoB, capSizeScaleFactor, icon, iconSizeFactor, iconPositionOffset_inScreenspace_relToHandleCapSize);
            bool hasChanged = EditorGUI.EndChangeCheck();

            if (hasChanged)
            {
                theDrawXXLChartInspector_unserializedMonoB.alwaysEncapsulateAllValues = false;
                theDrawXXLChartInspector_unserializedMonoB.scrollSinceMouseDown_fromUnifiedHandleSlider_asTravelledWorldSpaceDistanceAlongChartsXDirection = travelledWorldSpaceDistanceAlongChartsXDirection_sinceMouseDown_for_unifiedDragHandle;
                theDrawXXLChartInspector_unserializedMonoB.scrollSinceMouseDown_fromUnifiedHandleSlider_asTravelledWorldSpaceDistanceAlongChartsYDirection = travelledWorldSpaceDistanceAlongChartsYDirection_sinceMouseDown_for_unifiedDragHandle;
            }
            else
            {
                if (GUIUtility.hotControl == 0)
                {
                    travelledWorldSpaceDistanceAlongChartsXDirection_sinceMouseDown_for_unifiedDragHandle = 0.0f;
                    travelledWorldSpaceDistanceAlongChartsYDirection_sinceMouseDown_for_unifiedDragHandle = 0.0f;
                    theDrawXXLChartInspector_unserializedMonoB.scrollSinceMouseDown_fromUnifiedHandleSlider_asTravelledWorldSpaceDistanceAlongChartsXDirection = 0.0f;
                    theDrawXXLChartInspector_unserializedMonoB.scrollSinceMouseDown_fromUnifiedHandleSlider_asTravelledWorldSpaceDistanceAlongChartsYDirection = 0.0f;
                }
            }
        }

        void DrawZoomSliderAsCylinderAtXAxis(float sizeOfHandleCaps)
        {
            float distanceFromAnchorPos = distanceFromAnchorPosFactor_forBacksnappingCylinderCaps * sizeOfHandleCaps;
            Vector3 sliderDirection_worldSpace_normalized = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.xAxis.AxisVector_normalized_inWorldSpace;
            Vector3 restingPosition = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.Position_worldspace + theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.xAxis.AxisVector_inWorldSpace + sliderDirection_worldSpace_normalized * distanceFromAnchorPos;
            Handles.CapFunction capFunction = Handles.CylinderHandleCap;
            float capSizeScaleFactor = sizeFactor_forBacksnappingCylinderCaps;
            GUIContent icon = magnifierGlassSymbol_atXAxis;
            float iconSizeFactor = sizeFactor_forHandIcon_onConesAndSpheres;
            Vector2 iconPositionOffset_inScreenspace_relToHandleCapSize = new Vector2(-0.22f, 0.2f); //-> manually shifting the icon, so it appears centered over the handle cap.

            EditorGUI.BeginChangeCheck();
            travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_zoomXAxisHandle = InternalDXXL_ChartHandles.OneDirectionalBacksnapSlider(travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_zoomXAxisHandle, restingPosition, theDrawXXLChartInspector_unserializedMonoB, sliderDirection_worldSpace_normalized, capFunction, capSizeScaleFactor, icon, iconSizeFactor, iconPositionOffset_inScreenspace_relToHandleCapSize, false, true);
            bool hasChanged = EditorGUI.EndChangeCheck();

            if (hasChanged)
            {
                theDrawXXLChartInspector_unserializedMonoB.alwaysEncapsulateAllValues = false;
                float zoomWeight_m1_to_p1 = travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_zoomXAxisHandle / theDrawXXLChartInspector_unserializedMonoB.GetBacksnapSliderReferenceLength_inWorldspaceUnits();
                theDrawXXLChartInspector_unserializedMonoB.xAxisZoomSinceMouseDown_fromHandleSlider_m1_to_p1 = zoomWeight_m1_to_p1;
            }
            else
            {
                if (GUIUtility.hotControl == 0)
                {
                    travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_zoomXAxisHandle = 0.0f;
                    theDrawXXLChartInspector_unserializedMonoB.xAxisZoomSinceMouseDown_fromHandleSlider_m1_to_p1 = 0.0f;
                }
            }
        }

        void DrawZoomSliderAsCylinderAtYAxis(float sizeOfHandleCaps)
        {
            float distanceFromAnchorPos = distanceFromAnchorPosFactor_forBacksnappingCylinderCaps * sizeOfHandleCaps;
            Vector3 sliderDirection_worldSpace_normalized = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.yAxis.AxisVector_normalized_inWorldSpace;
            Vector3 restingPosition = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.Position_worldspace + theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.yAxis.AxisVector_inWorldSpace + sliderDirection_worldSpace_normalized * distanceFromAnchorPos;
            Handles.CapFunction capFunction = Handles.CylinderHandleCap;
            float capSizeScaleFactor = sizeFactor_forBacksnappingCylinderCaps;
            GUIContent icon = magnifierGlassSymbol_atYAxis;
            float iconSizeFactor = sizeFactor_forMagnifierGlassIcons_onCylinders;
            Vector2 iconPositionOffset_inScreenspace_relToHandleCapSize = new Vector2(-0.23f, 0.24f); //-> manually shifting the icon, so it appears centered over the handle cap.

            EditorGUI.BeginChangeCheck();
            travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_zoomYAxisHandle = InternalDXXL_ChartHandles.OneDirectionalBacksnapSlider(travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_zoomYAxisHandle, restingPosition, theDrawXXLChartInspector_unserializedMonoB, sliderDirection_worldSpace_normalized, capFunction, capSizeScaleFactor, icon, iconSizeFactor, iconPositionOffset_inScreenspace_relToHandleCapSize, false, true);
            bool hasChanged = EditorGUI.EndChangeCheck();

            if (hasChanged)
            {
                theDrawXXLChartInspector_unserializedMonoB.alwaysEncapsulateAllValues = false;
                float zoomWeight_m1_to_p1 = travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_zoomYAxisHandle / theDrawXXLChartInspector_unserializedMonoB.GetBacksnapSliderReferenceLength_inWorldspaceUnits();
                theDrawXXLChartInspector_unserializedMonoB.yAxisZoomSinceMouseDown_fromHandleSlider_m1_to_p1 = zoomWeight_m1_to_p1;
            }
            else
            {
                if (GUIUtility.hotControl == 0)
                {
                    travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_zoomYAxisHandle = 0.0f;
                    theDrawXXLChartInspector_unserializedMonoB.yAxisZoomSinceMouseDown_fromHandleSlider_m1_to_p1 = 0.0f;
                }
            }
        }

        void DrawZoomSliderAsCylinderAtUnifiedAxis(float sizeOfHandleCaps)
        {
            float distanceFromAnchorPos = 1.0f * sizeOfHandleCaps;
            Vector3 sliderDirection_worldSpace_normalized = -theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.Get_unified45degAxis_forHandleSliders_normalized();
            Vector3 restingPosition = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.Position_worldspace - sliderDirection_worldSpace_normalized * distanceFromAnchorPos;
            Handles.CapFunction capFunction = Handles.CylinderHandleCap;
            float capSizeScaleFactor = sizeFactor_forBacksnappingCylinderCaps;
            GUIContent icon = magnifierGlassSymbol_forBothAxes;
            float iconSizeFactor = sizeFactor_forMagnifierGlassIcons_onCylinders;
            Vector2 iconPositionOffset_inScreenspace_relToHandleCapSize = new Vector2(-0.25f, 0.25f); //-> manually shifting the icon, so it appears centered over the handle cap.

            EditorGUI.BeginChangeCheck();
            travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_forZoomBothAxesHandle = InternalDXXL_ChartHandles.OneDirectionalBacksnapSlider(travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_forZoomBothAxesHandle, restingPosition, theDrawXXLChartInspector_unserializedMonoB, sliderDirection_worldSpace_normalized, capFunction, capSizeScaleFactor, icon, iconSizeFactor, iconPositionOffset_inScreenspace_relToHandleCapSize, false, true);
            bool hasChanged = EditorGUI.EndChangeCheck();

            if (hasChanged)
            {
                theDrawXXLChartInspector_unserializedMonoB.alwaysEncapsulateAllValues = false;
                float zoomWeight_m1_to_p1 = travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_forZoomBothAxesHandle / theDrawXXLChartInspector_unserializedMonoB.GetBacksnapSliderReferenceLength_inWorldspaceUnits();
                theDrawXXLChartInspector_unserializedMonoB.bothAxesZoomSinceMouseDown_fromHandleSlider_m1_to_p1 = zoomWeight_m1_to_p1;
            }
            else
            {
                if (GUIUtility.hotControl == 0)
                {
                    travelledWorldSpaceDistanceAlongSliderDirection_sinceMouseDown_for_forZoomBothAxesHandle = 0.0f;
                    theDrawXXLChartInspector_unserializedMonoB.bothAxesZoomSinceMouseDown_fromHandleSlider_m1_to_p1 = 0.0f;
                }
            }
        }

        void DrawCheckboxFor_showAllValues(float sizeOfHandleCaps)
        {
            float distanceFromAnchorPos = 3.6f * sizeOfHandleCaps;
            Vector3 unified45degAxis_forHandleSliders_normalized = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.Get_unified45degAxis_forHandleSliders_normalized();
            Vector3 position = theDrawXXLChartInspector_unserializedMonoB.chart_thisInspectorIsAttachedTo.Position_worldspace + unified45degAxis_forHandleSliders_normalized * distanceFromAnchorPos;
            Vector2 checkmarkIconPositionOffset_inScreenspace_relToHandleCapSize = new Vector2(-0.25f, 0.25f); //-> manually shifting the icon, so it appears centered over the handle cap.
            Vector2 crossIconPositionOffset_inScreenspace_relToHandleCapSize = new Vector2(-0.25f, 0.25f); //-> manually shifting the icon, so it appears centered over the handle cap.

            bool checkmarkState_before = theDrawXXLChartInspector_unserializedMonoB.alwaysEncapsulateAllValues;
            bool checkmarkState_after = InternalDXXL_ChartHandles.ShowAllButton(checkmarkState_before, position, theDrawXXLChartInspector_unserializedMonoB, sizeFactor_forBacksnappingSphereCaps, checkmarkSymbol, crossSymbol, sizeFactor_forCheckmarkIcon, sizeFactor_forCrossIcon, checkmarkIconPositionOffset_inScreenspace_relToHandleCapSize, crossIconPositionOffset_inScreenspace_relToHandleCapSize);
            theDrawXXLChartInspector_unserializedMonoB.alwaysEncapsulateAllValues = checkmarkState_after;
        }

        public override void OnInspectorGUI()
        {
            if (theDrawXXLChartInspector_unserializedMonoB.CheckIfReferencedChartGotLost())
            {
                EditorGUILayout.HelpBox("The ChartDrawing that should be displayed with this component got lost for some reason, which normally leads to the automatic deletion of the whole gameobject." + Environment.NewLine + Environment.NewLine + "This gameobject has not been automatically deleted though, because it seems to host other components or has childs or parents attached." + Environment.NewLine + Environment.NewLine + "You can delete this component if you don't need it anymore. If you want to continue inspecting the chart then create a new chart inspector component via 'chartThatYouWantToInspect.CreateChartInspectionGameobject()'.", MessageType.Info, true);
            }
            else
            {
                int indentLevel_before = EditorGUI.indentLevel;
                serializedObject.Update();

                GUIStyle style_forFoldoutWithBoldText = new GUIStyle(EditorStyles.foldout);
                style_forFoldoutWithBoldText.fontStyle = FontStyle.Bold;

                DrawConsumedLines();
                DrawHandles_andOptionallyZoomAndAxisScaling(style_forFoldoutWithBoldText);
                DrawSection_sceneViewCamera(style_forFoldoutWithBoldText);
                DrawSection_other(style_forFoldoutWithBoldText);
                DrawSection_lines();

                serializedObject.ApplyModifiedProperties();
                EditorGUI.indentLevel = indentLevel_before;
            }
        }

        void DrawConsumedLines()
        {
            SerializedProperty sP_drawnLinesPerPass = serializedObject.FindProperty("drawnSmallStraightLines_duringLastDrawRun");
            GUIStyle style_ofConsumedLinesLabel = new GUIStyle();
            style_ofConsumedLinesLabel.richText = true;
            float allowedConsumedLines_0to1 = (float)sP_drawnLinesPerPass.intValue / (float)DrawBasics.MaxAllowedDrawnLinesPerFrame;
            float hueValueOfColor = 0.33333f * (1.0f - allowedConsumedLines_0to1);
            Color color_visualizingWarningForTooManyLines = SeededColorGenerator.GetColorFromHueAndLuminance_tunedTransitionsSpectrum(hueValueOfColor, 0.5f, 1.0f);
            string coloredLineNumber = "<color=#" + ColorUtility.ToHtmlStringRGBA(color_visualizingWarningForTooManyLines) + ">" + sP_drawnLinesPerPass.intValue + "</color>";
            string tooltipForBoth = "This chart is drawn with single small straight lines (like from 'Debug.DrawLine()'). If too many of these straight lines are drawn it may hit the Editor execution performance." + Environment.NewLine + Environment.NewLine + "The color becomes more red as the number reaches a critical area." + Environment.NewLine + Environment.NewLine + "(see also 'DrawXXL.DrawBasics.MaxAllowedDrawnLinesPerFrame')";
            GUIContent drawnLinesWord = new GUIContent("Drawn small straight lines:", tooltipForBoth);
            GUIContent drawnLinesNumber = new GUIContent(coloredLineNumber, tooltipForBoth);
            EditorGUILayout.LabelField(drawnLinesWord, drawnLinesNumber, style_ofConsumedLinesLabel);
        }

        void DrawSection_sceneViewCamera(GUIStyle style_forFoldoutWithBoldText)
        {
            SerializedProperty serializedProperty_sceneViewCamSection_isExpanded = serializedObject.FindProperty("sceneViewCamSection_isExpanded");
            serializedProperty_sceneViewCamSection_isExpanded.boolValue = EditorGUILayout.Foldout(serializedProperty_sceneViewCamSection_isExpanded.boolValue, "Scene view camera", true, style_forFoldoutWithBoldText);
            if (serializedProperty_sceneViewCamSection_isExpanded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                SerializedProperty serializedProperty_forceSceneViewCamToFollowChart = serializedObject.FindProperty("forceSceneViewCamToFollowChart");

                GUILayout.BeginHorizontal();
                GUILayout.Space(15.0f);
                GUIContent guiContent_forbutton_setToChart = new GUIContent("Set to chart", "Set the Scene view camera position so that it looks at the chart.");
                EditorGUI.BeginDisabledGroup(serializedProperty_forceSceneViewCamToFollowChart.boolValue);
                if (GUILayout.Button(guiContent_forbutton_setToChart))
                {
                    theDrawXXLChartInspector_unserializedMonoB.TrySetSceneViewCamToChart_dueToButtonClick();
                }
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();

                //The feature "Checkbox: Stay at chart" is disabled. Reason:
                //-> The risk is too high, that a user activates "Scene View Camera - Stay at Chart"
                //-> and then changes the GameObject
                //-> and then looses track why his SceneView Camera is fixed and he cannot navigate anymore with his camera through the Scene.

                GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        void DrawHandles_andOptionallyZoomAndAxisScaling(GUIStyle style_forFoldoutWithBoldText)
        {
            SerializedProperty sP_hideHandles_andInsteadUseInspectorSlidersForZoomAndScroll = serializedObject.FindProperty("hideHandles_andInsteadUseInspectorSlidersForZoomAndScroll");
            sP_hideHandles_andInsteadUseInspectorSlidersForZoomAndScroll.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Hide handles and use inspector sliders instead", "This may be helpful if the handles cover important information." + Environment.NewLine + Environment.NewLine + "When hiding handles you can still zoom and scroll the chart via sliders here in the inspector."), sP_hideHandles_andInsteadUseInspectorSlidersForZoomAndScroll.boolValue);

            if (sP_hideHandles_andInsteadUseInspectorSlidersForZoomAndScroll.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                DrawSection_cursor(style_forFoldoutWithBoldText);
                DrawSection_axesScaling(style_forFoldoutWithBoldText);
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        void DrawSection_cursor(GUIStyle style_forFoldoutWithBoldText)
        {
            SerializedProperty serializedProperty_cursorSection_isExpanded = serializedObject.FindProperty("cursorSection_isExpanded");
            serializedProperty_cursorSection_isExpanded.boolValue = EditorGUILayout.Foldout(serializedProperty_cursorSection_isExpanded.boolValue, "Cursor", true, style_forFoldoutWithBoldText);

            if (serializedProperty_cursorSection_isExpanded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                SerializedProperty serializedProperty_curr_cursorPosition_inChartspaceUnits = serializedObject.FindProperty("curr_cursorPosition_inChartspaceUnits");
                EditorGUILayout.PropertyField(serializedProperty_curr_cursorPosition_inChartspaceUnits, new GUIContent("Position"));

                GUILayout.Space(0.3f * EditorGUIUtility.singleLineHeight);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("cursorPos0to1_raw"), new GUIContent("Slide 0-1", "Move the cursor between the left end of the chart (0) and the right end of the chart (1)."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cursorPos0to1_finetune"), new GUIContent("Finetune", "Add fine offset to the cursor slide position."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("cursorPos0to1_superFinetune"), new GUIContent("Super Finetune", "Add very fine offset to the cursor slide position."));

                GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        void DrawSection_axesScaling(GUIStyle style_forFoldoutWithBoldText)
        {
            SerializedProperty sP_zoomForBothAxesToApply_fromInspectorSlider_m1_to_p1 = serializedObject.FindProperty("zoomForBothAxesToApply_fromInspectorSlider_m1_to_p1");
            SerializedProperty sP_xAxisZoomToApply_fromInspectorSlider_m1_to_p1 = serializedObject.FindProperty("xAxisZoomToApply_fromInspectorSlider_m1_to_p1");
            SerializedProperty sP_xAxisScrollToApply_fromInspectorSlider_m1_to_p1 = serializedObject.FindProperty("xAxisScrollToApply_fromInspectorSlider_m1_to_p1");
            SerializedProperty sP_yAxisZoomToApply_fromInspectorSlider_m1_to_p1 = serializedObject.FindProperty("yAxisZoomToApply_fromInspectorSlider_m1_to_p1");
            SerializedProperty sP_yAxisScrollToApply_fromInspectorSlider_m1_to_p1 = serializedObject.FindProperty("yAxisScrollToApply_fromInspectorSlider_m1_to_p1");

            SerializedProperty serializedProperty_axesSection_isExpanded = serializedObject.FindProperty("axesSection_isExpanded");
            serializedProperty_axesSection_isExpanded.boolValue = EditorGUILayout.Foldout(serializedProperty_axesSection_isExpanded.boolValue, "Axes Scaling", true, style_forFoldoutWithBoldText);

            if (serializedProperty_axesSection_isExpanded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                GUILayout.Space(0.3f * EditorGUIUtility.singleLineHeight);

                GUILayout.BeginHorizontal();
                GUILayout.Space(15.0f);
                GUIContent guiContent_forbutton_showEverything = new GUIContent("Show everything", "Set the axis scaling so that all datapoints of all non-hidden lines are visible.");
                if (GUILayout.Button(guiContent_forbutton_showEverything, GUILayout.Width(150)))
                {
                    theDrawXXLChartInspector_unserializedMonoB.ResetAxesScalingTo_encapsulateAllValues();
                }

                SerializedProperty serializedProperty_alwaysEncapsulateAllValues = serializedObject.FindProperty("alwaysEncapsulateAllValues");
                GUIContent guiContent_alwaysEncapsulateAllValues = new GUIContent("Always", "This continuously overwrites the axis scaling so that always all datapoints are visible.");
                serializedProperty_alwaysEncapsulateAllValues.boolValue = GUILayout.Toggle(serializedProperty_alwaysEncapsulateAllValues.boolValue, guiContent_alwaysEncapsulateAllValues);
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.Space(15.0f);
                GUIContent guiContent_forbutton_revertChanges = new GUIContent("Revert changes", "Set the axis scaling to as it was in the moment where the chart was frozen for inspection.");
                if (GUILayout.Button(guiContent_forbutton_revertChanges, GUILayout.Width(150)))
                {
                    theDrawXXLChartInspector_unserializedMonoB.ResetAxesScalingTo_stateInMomentOfComponentCreation();
                }
                GUILayout.EndHorizontal();

                EditorGUI.BeginDisabledGroup(serializedProperty_alwaysEncapsulateAllValues.boolValue);

                GUILayout.Space(0.3f * EditorGUIUtility.singleLineHeight);
                EditorGUILayout.PropertyField(sP_zoomForBothAxesToApply_fromInspectorSlider_m1_to_p1, new GUIContent("Zoom"));

                GUILayout.Space(0.7f * EditorGUIUtility.singleLineHeight);
                EditorGUILayout.PropertyField(sP_xAxisZoomToApply_fromInspectorSlider_m1_to_p1, new GUIContent("Zoom X"));
                EditorGUILayout.PropertyField(sP_xAxisScrollToApply_fromInspectorSlider_m1_to_p1, new GUIContent("Scroll X"));

                GUILayout.Space(0.7f * EditorGUIUtility.singleLineHeight);
                EditorGUILayout.PropertyField(sP_yAxisZoomToApply_fromInspectorSlider_m1_to_p1, new GUIContent("Zoom Y"));
                EditorGUILayout.PropertyField(sP_yAxisScrollToApply_fromInspectorSlider_m1_to_p1, new GUIContent("Scroll Y"));
                GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);

                EditorGUI.EndDisabledGroup();

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }

            if (EditorGUIUtility.hotControl == 0)
            {
                //This resets the sliders to their zero position in the middle if they are not clicked or dragged
                //-> It fakes an "Event.MouseUp" event
                sP_zoomForBothAxesToApply_fromInspectorSlider_m1_to_p1.floatValue = 0.0f;
                sP_xAxisZoomToApply_fromInspectorSlider_m1_to_p1.floatValue = 0.0f;
                sP_xAxisScrollToApply_fromInspectorSlider_m1_to_p1.floatValue = 0.0f;
                sP_yAxisZoomToApply_fromInspectorSlider_m1_to_p1.floatValue = 0.0f;
                sP_yAxisScrollToApply_fromInspectorSlider_m1_to_p1.floatValue = 0.0f;
            }
        }

        void DrawSection_other(GUIStyle style_forFoldoutWithBoldText)
        {
            SerializedProperty serializedProperty_otherSection_isExpanded = serializedObject.FindProperty("otherSection_isExpanded");
            serializedProperty_otherSection_isExpanded.boolValue = EditorGUILayout.Foldout(serializedProperty_otherSection_isExpanded.boolValue, "Other", true, style_forFoldoutWithBoldText);
            if (serializedProperty_otherSection_isExpanded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                DrawSection_csvExport();
                DrawSection_clearLineValues();

                EditorGUILayout.PropertyField(serializedObject.FindProperty("sizeOfDatapointVisualization"), new GUIContent("Size (cursor values)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("curr_luminanceOfLineColors_accordingToSlider"), new GUIContent("Luminance (line colors)", "Changing this can improve the readability of the lines in front of different background brightness."));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("lineNamePositions"), new GUIContent("Line names position"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("lineNames_sizeScaleFactor"), new GUIContent("Size (line names)"));

                int new_maxDisplayedPointOfInterestTextBoxesPerSide = EditorGUILayout.IntField(new GUIContent("Maximum number of Text Boxes (for Points of Interest)", "Some Points of Interest display a text box with explantion text in the top corners of the chart. In some unforeseen situation, for example if many invalid float values are added as data points, the number of text boxes that notify you of these invalid float values can rapidly grow and as a result slow down the Editor performance."), theDrawXXLChartInspector_unserializedMonoB.Get_maxDisplayedPointOfInterestTextBoxesPerSide());
                if (new_maxDisplayedPointOfInterestTextBoxesPerSide != theDrawXXLChartInspector_unserializedMonoB.Get_maxDisplayedPointOfInterestTextBoxesPerSide())
                {
                    theDrawXXLChartInspector_unserializedMonoB.Set_maxDisplayedPointOfInterestTextBoxesPerSide(new_maxDisplayedPointOfInterestTextBoxesPerSide);
                }

                GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
            else
            {
                GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);
            }
        }

        void DrawSection_csvExport()
        {
            SerializedProperty serializedProperty_csvExportSection_isExpanded = serializedObject.FindProperty("csvExportSection_isExpanded");
            serializedProperty_csvExportSection_isExpanded.boolValue = EditorGUILayout.Foldout(serializedProperty_csvExportSection_isExpanded.boolValue, "CSV file export", true);
            if (serializedProperty_csvExportSection_isExpanded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.HelpBox("The file is written into the Assets folder of your project. Existing files with the same name will not be overwritten but a sequential number will be added to the new file name.", MessageType.None, true);

                SerializedProperty serializedProperty_csvFileName = serializedObject.FindProperty("csvFileName");
                serializedProperty_csvFileName.stringValue = EditorGUILayout.TextField("File name", serializedProperty_csvFileName.stringValue);

                GUILayout.BeginHorizontal();
                GUILayout.Space(30.0f);
                if (GUILayout.Button("Export"))
                {
                    theDrawXXLChartInspector_unserializedMonoB.ExportCSVFile(serializedProperty_csvFileName.stringValue);
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        void DrawSection_clearLineValues()
        {
            SerializedProperty serializedProperty_clearButtonSection_isExpanded = serializedObject.FindProperty("clearButtonSection_isExpanded");
            serializedProperty_clearButtonSection_isExpanded.boolValue = EditorGUILayout.Foldout(serializedProperty_clearButtonSection_isExpanded.boolValue, "Clear Line Data", true);
            if (serializedProperty_clearButtonSection_isExpanded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.HelpBox("No 'Undo' possible.", MessageType.Warning, true);

                GUILayout.BeginHorizontal();
                GUILayout.Space(30.0f);
                if (GUILayout.Button(new GUIContent("Clear all Line Data Values", "This deletes all data points of all lines of this chart, so you can start with a fresh unfilled chart." + Environment.NewLine + Environment.NewLine + "Be cautious, because this cannot be undone.")))
                {
                    theDrawXXLChartInspector_unserializedMonoB.ClearLineData();
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        void DrawSection_lines()
        {
            GUIStyle boldFontStyle = new GUIStyle();
            boldFontStyle.fontStyle = FontStyle.Bold;

            GUILayout.Label("Lines:", boldFontStyle);
            SerializedProperty serializedProperty_specsForInspectorForEachDrawnLine = serializedObject.FindProperty("specsForInspector_forEachDrawnLine");
            for (int i = 0; i < serializedProperty_specsForInspectorForEachDrawnLine.arraySize; i++)
            {
                EditorGUILayout.PropertyField(serializedProperty_specsForInspectorForEachDrawnLine.GetArrayElementAtIndex(i));
            }
        }

    }
#endif
}
