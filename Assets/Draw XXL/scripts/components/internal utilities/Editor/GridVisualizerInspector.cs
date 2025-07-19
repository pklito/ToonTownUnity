namespace DrawXXL
{
    using UnityEngine;
    using System;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(GridVisualizer))]
    [CanEditMultipleObjects]
    public class GridVisualizerInspector : VisualizerParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("grid");

            SerializedProperty sP_spaceType = serializedObject.FindProperty("spaceType");
            EditorGUILayout.PropertyField(sP_spaceType, new GUIContent("Visualized space"));

            GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

            SerializedProperty sP_xGridType = serializedObject.FindProperty("xGridType");
            SerializedProperty sP_yGridType = serializedObject.FindProperty("yGridType");
            SerializedProperty sP_zGridType = serializedObject.FindProperty("zGridType");

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sP_xGridType, new GUIContent("X"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("colorForX"), GUIContent.none);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sP_yGridType, new GUIContent("Y"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("colorForY"), GUIContent.none);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sP_zGridType, new GUIContent("Z"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("colorForZ"), GUIContent.none);
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

            GUIStyle style_forFoldoutLine = new GUIStyle(EditorStyles.foldout);
            style_forFoldoutLine.fontStyle = FontStyle.Bold;

            GUIStyle style_ofHeadlines = new GUIStyle();
            style_ofHeadlines.fontStyle = FontStyle.Bold;

            DrawOrdersOfMagnitudeSection(style_forFoldoutLine);

            GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

            bool noDimensionIsDisplayed_asPlane = ((sP_xGridType.enumValueIndex != (int)GridVisualizer.XGridType.planes) && (sP_yGridType.enumValueIndex != (int)GridVisualizer.YGridType.planes) && (sP_zGridType.enumValueIndex != (int)GridVisualizer.ZGridType.planes));
            bool planesSection_isDisabled = noDimensionIsDisplayed_asPlane;
            EditorGUI.BeginDisabledGroup(planesSection_isDisabled);
            EditorGUILayout.LabelField(planesSection_isDisabled ? "Planes details    (no planes are activated, see above at X, Y and Z)" : "Planes details", style_ofHeadlines);
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            SerializedProperty sP_coveredGridUnits_rel_forGridPlanes = serializedObject.FindProperty("coveredGridUnits_rel_forGridPlanes");
            sP_coveredGridUnits_rel_forGridPlanes.floatValue = EditorGUILayout.FloatField(new GUIContent("Covered grid units", "This is relative to the biggest drawn order of magnitude." + Environment.NewLine + Environment.NewLine + "The minimum value is 2.5"), sP_coveredGridUnits_rel_forGridPlanes.floatValue);
            sP_coveredGridUnits_rel_forGridPlanes.floatValue = Mathf.Max(sP_coveredGridUnits_rel_forGridPlanes.floatValue, 2.5f);

            SerializedProperty sP_extentOfEachGridPlane_rel = serializedObject.FindProperty("extentOfEachGridPlane_rel");
            EditorGUILayout.PropertyField(sP_extentOfEachGridPlane_rel, new GUIContent("Size", "This is relative to the smallest drawn order of magnitude." + Environment.NewLine + Environment.NewLine + "The minimum value is 0.1"));
            sP_extentOfEachGridPlane_rel.floatValue = Mathf.Max(sP_extentOfEachGridPlane_rel.floatValue, 0.1f);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("drawDensity"), new GUIContent("Density"));
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

            bool noDimensionIsDisplayed_asLine = ((sP_xGridType.enumValueIndex != (int)GridVisualizer.XGridType.linesAlongY) && (sP_xGridType.enumValueIndex != (int)GridVisualizer.XGridType.linesAlongZ) && (sP_yGridType.enumValueIndex != (int)GridVisualizer.YGridType.linesAlongX) && (sP_yGridType.enumValueIndex != (int)GridVisualizer.YGridType.linesAlongZ) && (sP_zGridType.enumValueIndex != (int)GridVisualizer.ZGridType.linesAlongX) && (sP_zGridType.enumValueIndex != (int)GridVisualizer.ZGridType.linesAlongY));
            EditorGUI.BeginDisabledGroup(noDimensionIsDisplayed_asLine);
            EditorGUILayout.LabelField(noDimensionIsDisplayed_asLine ? "Lines details    (no lines are activated, see above at X, Y and Z)" : "Lines details", style_ofHeadlines);
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            SerializedProperty sP_coveredGridUnits_rel = serializedObject.FindProperty("coveredGridUnits_rel");
            EditorGUILayout.PropertyField(sP_coveredGridUnits_rel, new GUIContent("Covered grid units", "This is relative to the biggest drawn order of magnitude." + Environment.NewLine + Environment.NewLine + "The minimum value is 2.5"));
            sP_coveredGridUnits_rel.floatValue = Mathf.Max(sP_coveredGridUnits_rel.floatValue, 2.5f);

            SerializedProperty sP_lengthOfEachGridLine_rel = serializedObject.FindProperty("lengthOfEachGridLine_rel");
            EditorGUILayout.PropertyField(sP_lengthOfEachGridLine_rel, new GUIContent("Length", "This is relative to the smallest drawn order of magnitude." + Environment.NewLine + Environment.NewLine + "The minimum value is 0.1"));
            sP_lengthOfEachGridLine_rel.floatValue = Mathf.Max(sP_lengthOfEachGridLine_rel.floatValue, 0.1f);

            DrawLineWidth();

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            EditorGUI.EndDisabledGroup();

            GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

            DrawCoordinateDisplaySection(sP_spaceType);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("show_positionAroundWhichToDraw_forGrids"), new GUIContent("Visualize Position"));

            SerializedProperty sP_show_distanceDisplay_forGrids = serializedObject.FindProperty("show_distanceDisplay_forGrids");
            EditorGUILayout.PropertyField(sP_show_distanceDisplay_forGrids, new GUIContent("Visualize Distance of Position to Grid"));

            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            EditorGUI.BeginDisabledGroup(!sP_show_distanceDisplay_forGrids.boolValue);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("offsetForDistanceDisplays_inGrids"), new GUIContent("Offset for Distance Display"));
            EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

            Draw_DrawPosition3DOffset();
            DrawCheckboxFor_drawOnlyIfSelected("grid");
            DrawCheckboxFor_hiddenByNearerObjects("grid");

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }

        void DrawOrdersOfMagnitudeSection(GUIStyle style_forFoldoutLine)
        {
            SerializedProperty sP_magnitudeOrderSection_isOutfolded = serializedObject.FindProperty("magnitudeOrderSection_isOutfolded");
            sP_magnitudeOrderSection_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_magnitudeOrderSection_isOutfolded.boolValue, "Drawn orders of magnitude", true, style_forFoldoutLine);
            if (sP_magnitudeOrderSection_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("draw1000grid"), new GUIContent("1000"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("draw100grid"), new GUIContent("100"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("draw10grid"), new GUIContent("10"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("draw1grid"), new GUIContent("1"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("draw0p1grid"), new GUIContent("0.1"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("draw0p01grid"), new GUIContent("0.01"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("draw0p001grid"), new GUIContent("0.001"));
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        void DrawLineWidth()
        {
            SerializedProperty sP_lineWidthMode = serializedObject.FindProperty("lineWidthMode");
            SerializedProperty sP_linesWidth_alongVisualizedAxis = serializedObject.FindProperty("linesWidth_alongVisualizedAxis");
            SerializedProperty sP_linesWidth_perpendicularToVisualizedAxis = serializedObject.FindProperty("linesWidth_perpendicularToVisualizedAxis");

            EditorGUILayout.PropertyField(sP_lineWidthMode, new GUIContent("Line width direction"));

            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            if (sP_lineWidthMode.enumValueIndex == (int)GridVisualizer.LineWidthMode.growAlongVisualizedAxis)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(sP_linesWidth_alongVisualizedAxis, new GUIContent("Line width", "This is relative to the concerned order of magnitude."));
                bool hasChanged = EditorGUI.EndChangeCheck();

                if (hasChanged)
                {
                    sP_linesWidth_perpendicularToVisualizedAxis.floatValue = sP_linesWidth_alongVisualizedAxis.floatValue;
                }
            }
            else
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(sP_linesWidth_perpendicularToVisualizedAxis, new GUIContent("Line width", "This is relative to the concerned order of magnitude."));
                sP_linesWidth_perpendicularToVisualizedAxis.floatValue = Mathf.Max(sP_linesWidth_perpendicularToVisualizedAxis.floatValue, 0.0f);
                bool hasChanged = EditorGUI.EndChangeCheck();

                if (hasChanged)
                {
                    sP_linesWidth_alongVisualizedAxis.floatValue = sP_linesWidth_perpendicularToVisualizedAxis.floatValue;
                    sP_linesWidth_alongVisualizedAxis.floatValue = Mathf.Min(sP_linesWidth_alongVisualizedAxis.floatValue, GridVisualizer.max_linesWidth_alongVisualizedAxis);
                }
            }

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

        void DrawCoordinateDisplaySection(SerializedProperty sP_spaceType)
        {
            SerializedProperty sP_repeatingCoordsTextVariant = serializedObject.FindProperty("repeatingCoordsTextVariant");
            EditorGUILayout.PropertyField(sP_repeatingCoordsTextVariant, new GUIContent("Coordiantes Display"));

            switch (sP_repeatingCoordsTextVariant.enumValueIndex)
            {
                case (int)GridVisualizer.RepeatingCoordsTextVariant.repeatAfterDistance:
                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                    SerializedProperty sP_distanceBetweenRepeatingCoordsTexts_relToGridDistance = serializedObject.FindProperty("distanceBetweenRepeatingCoordsTexts_relToGridDistance");
                    EditorGUILayout.PropertyField(sP_distanceBetweenRepeatingCoordsTexts_relToGridDistance, new GUIContent("Text distance between repeating coordinates", "This is relative to the grid distance." + Environment.NewLine + "The minimum value is 5." + Environment.NewLine + "You may see changes only if the length/extent of the lines/planes is long enough."));
                    sP_distanceBetweenRepeatingCoordsTexts_relToGridDistance.floatValue = Mathf.Max(sP_distanceBetweenRepeatingCoordsTexts_relToGridDistance.floatValue, UtilitiesDXXL_Grid.min_distanceBetweenRepeatingCoordsTexts_relToGridDistance);
                    DrawSharedFieldsOfNonDisabledCoordinateTextModes( sP_spaceType);

                    GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);
                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                    break;
                case (int)GridVisualizer.RepeatingCoordsTextVariant.displayOnlyOnce:

                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                    DrawSharedFieldsOfNonDisabledCoordinateTextModes( sP_spaceType);

                    GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);
                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

                    break;
                case (int)GridVisualizer.RepeatingCoordsTextVariant.noDisplay:
                    break;
                default:
                    break;
            }
        }

        void DrawSharedFieldsOfNonDisabledCoordinateTextModes(SerializedProperty sP_spaceType)
        {
            GUIContent coordinateTextOffset_GUIContent = new GUIContent("Text Position", "This is relative to the containing order of magnitude.");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("offsetForCoordinateTextDisplays_inGrids"), coordinateTextOffset_GUIContent);

            GUIContent textSize_GUIContent = new GUIContent("Text Size", "This is relative to the containing order of magnitude.");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("sizeScalingForCoordinateTexts_inGrids"), textSize_GUIContent);

            GUIContent skipXYZAxisIdentifier_GUIContent = new GUIContent("Skip 'X/Y/Z =' prefix", "This can save performance for large grid displays." + Environment.NewLine + Environment.NewLine + "The initial value of created components for this can be defined via 'DrawEngineBasics.skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes'.");
            EditorGUILayout.PropertyField(serializedObject.FindProperty("skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes"), skipXYZAxisIdentifier_GUIContent);

            if (GridDisplaysALocalSpace(sP_spaceType))
            {
                GUIContent skipLocalPrefix_GUIContent = new GUIContent("Skip 'local' prefix", "This can save performance for large grid displays." + Environment.NewLine + Environment.NewLine + "The initial value of created components for this can be defined via 'DrawEngineBasics.skipLocalPrefix_inCoordinateTextsOnGridAxes'.");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("skipLocalPrefix_inCoordinateTextsOnGridAxes"), skipLocalPrefix_GUIContent);
            }
        }

        bool GridDisplaysALocalSpace(SerializedProperty sP_spaceType)
        {
            switch (sP_spaceType.enumValueIndex)
            {
                case (int)GridVisualizer.SpaceType.global:
                    return false;
                case (int)GridVisualizer.SpaceType.localDefinedByParent:
                    return true;
                case (int)GridVisualizer.SpaceType.localDefinedByThisGameobject:
                    return true;
                default:
                    return false;
            }
        }

    }
#endif
}
