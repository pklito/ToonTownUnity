namespace DrawXXL
{
    using System;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomPropertyDrawer(typeof(InternalDXXL_LineSpecsForChartInspector))]
    public class LineSpecsForInspectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            float currYOffset = 0.0f;

            SerializedProperty serializedProperty_currentHideLineState = property.FindPropertyRelative("currentHideLineState");
            Rect space_ofMainLineHideCheckboxToggle = new Rect(position.x + 0.4f * EditorGUIUtility.singleLineHeight, position.y + currYOffset, position.width, EditorGUIUtility.singleLineHeight);
            serializedProperty_currentHideLineState.boolValue = !EditorGUI.Toggle(space_ofMainLineHideCheckboxToggle, !serializedProperty_currentHideLineState.boolValue);

            SerializedProperty serializedProperty_lineSection_isExpanded = property.FindPropertyRelative("lineSection_isExpanded");
            Rect space_ofMainFoldout = new Rect(position.x, position.y + currYOffset, position.width, EditorGUIUtility.singleLineHeight);
            serializedProperty_lineSection_isExpanded.boolValue = EditorGUI.Foldout(space_ofMainFoldout, serializedProperty_lineSection_isExpanded.boolValue, GUIContent.none, true, EditorStyles.foldout);

            SerializedProperty serializedProperty_lineColor = property.FindPropertyRelative("lineColor");
            SerializedProperty serializedProperty_linesCompoundName = property.FindPropertyRelative("linesCompoundName");

            GUIStyle richtextEnabled_fontStyle = new GUIStyle();
            richtextEnabled_fontStyle.richText = true;
            float indent_ofColoredLineName = 50;
            Rect space_ofColoredLineName = new Rect(position.x + 1.6f * EditorGUIUtility.singleLineHeight, position.y + currYOffset, position.width - indent_ofColoredLineName, position.height);
            EditorGUI.LabelField(space_ofColoredLineName, "Line:  <b><color=#" + ColorUtility.ToHtmlStringRGBA(serializedProperty_lineColor.colorValue) + ">" + serializedProperty_linesCompoundName.stringValue + "</color></b>", richtextEnabled_fontStyle);

            if (serializedProperty_lineSection_isExpanded.boolValue)
            {
                int previousIndent = EditorGUI.indentLevel;

                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                currYOffset = currYOffset + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
                Rect space_ofColorPicker = new Rect(position.x, position.y + currYOffset, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(space_ofColorPicker, serializedProperty_lineColor, new GUIContent("Color"));

                currYOffset = currYOffset + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
                SerializedProperty serializedProperty_currentHideCursorXState_duringComponentInspectionPhase = property.FindPropertyRelative("currentHideCursorXState_duringComponentInspectionPhase");
                Rect space_ofHideCursorXToggle = new Rect(position.x, position.y + currYOffset, position.width, EditorGUIUtility.singleLineHeight);
                serializedProperty_currentHideCursorXState_duringComponentInspectionPhase.boolValue = !EditorGUI.Toggle(space_ofHideCursorXToggle, "Draw X value", !serializedProperty_currentHideCursorXState_duringComponentInspectionPhase.boolValue);

                currYOffset = currYOffset + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
                SerializedProperty serializedProperty_currentHideCursorYState_duringComponentInspectionPhase = property.FindPropertyRelative("currentHideCursorYState_duringComponentInspectionPhase");
                Rect space_ofHideCursorYToggle = new Rect(position.x, position.y + currYOffset, position.width, EditorGUIUtility.singleLineHeight);
                serializedProperty_currentHideCursorYState_duringComponentInspectionPhase.boolValue = !EditorGUI.Toggle(space_ofHideCursorYToggle, "Draw Y value", !serializedProperty_currentHideCursorYState_duringComponentInspectionPhase.boolValue);

                DrawXXLChartInspector theDrawXXLChartInspector = (DrawXXLChartInspector)property.serializedObject.targetObject;
                bool thisLineIsTheOnlyOneOfTheChart = theDrawXXLChartInspector.ContainsOnly1Line_hiddenOrUnhidden();

                currYOffset = currYOffset + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
                EditorGUI.BeginDisabledGroup(thisLineIsTheOnlyOneOfTheChart);
                SerializedProperty serializedProperty_i_ofThisLineSpec_insideChartInspectorsLinesSpecsArray = property.FindPropertyRelative("i_ofThisLineSpec_insideChartInspectorsLinesSpecsArray");
                Rect space_ofButton_hideAllOtherLines = new Rect(position.x + 15.0f, position.y + currYOffset, 150.0f, EditorGUIUtility.singleLineHeight);
                if (theDrawXXLChartInspector.AllOtherLinesAreHidden(serializedProperty_i_ofThisLineSpec_insideChartInspectorsLinesSpecsArray.intValue))
                {
                    if (GUI.Button(space_ofButton_hideAllOtherLines, "Unhide all lines    "))
                    {
                        theDrawXXLChartInspector.UnhideAllLines();
                    }
                }
                else
                {
                    if (GUI.Button(space_ofButton_hideAllOtherLines, "Hide all other lines    "))
                    {
                        theDrawXXLChartInspector.HideAllOtherLines(serializedProperty_i_ofThisLineSpec_insideChartInspectorsLinesSpecsArray.intValue);
                    }
                }

                currYOffset = currYOffset + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
                Rect space_ofButton_hideAllOtherCursors = new Rect(position.x + 15.0f, position.y + currYOffset, 150.0f, EditorGUIUtility.singleLineHeight);
                if (theDrawXXLChartInspector.AllOtherCursorsAreHidden(serializedProperty_i_ofThisLineSpec_insideChartInspectorsLinesSpecsArray.intValue))
                {
                    if (GUI.Button(space_ofButton_hideAllOtherCursors, "Unhide all cursors"))
                    {
                        theDrawXXLChartInspector.UnhideAllCursors(serializedProperty_i_ofThisLineSpec_insideChartInspectorsLinesSpecsArray.intValue);
                    }
                }
                else
                {
                    if (GUI.Button(space_ofButton_hideAllOtherCursors, "Hide all other cursors"))
                    {
                        theDrawXXLChartInspector.HideAllOtherCursors(serializedProperty_i_ofThisLineSpec_insideChartInspectorsLinesSpecsArray.intValue);
                    }
                }
                EditorGUI.EndDisabledGroup();

                currYOffset = currYOffset + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
                SerializedProperty serializedProperty_alpha_ofVertFillLines = property.FindPropertyRelative("alpha_ofVertFillLines");
                GUIContent guiContent_for_alpha_ofVertFillLines = new GUIContent("Fill area (alpha)");
                Rect space_of_alpha_ofVertFillLines = new Rect(position.x, position.y + currYOffset, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(space_of_alpha_ofVertFillLines, serializedProperty_alpha_ofVertFillLines, guiContent_for_alpha_ofVertFillLines);
                if (serializedProperty_alpha_ofVertFillLines.floatValue < 0.001f) { serializedProperty_alpha_ofVertFillLines.floatValue = 0.0f; } //-> prenting float calculation uncertainty errors to accidentaly activate the many drawn lines for fillVerticalSpace

                currYOffset = currYOffset + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
                SerializedProperty serializedProperty_lineWidth = property.FindPropertyRelative("lineWidth");
                GUIContent guiContent_forWidthSlider = new GUIContent("Width", "This is relative to the chart height." + Environment.NewLine + Environment.NewLine + "Performance warning: Setting this to values other than 0 can significantly increase the number of drawn lines.");
                Rect space_ofWidthSlider = new Rect(position.x, position.y + currYOffset, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(space_ofWidthSlider, serializedProperty_lineWidth, guiContent_forWidthSlider);
                if (serializedProperty_lineWidth.floatValue < 0.00005f) { serializedProperty_lineWidth.floatValue = 0.0f; } //-> prenting float calculation uncertainty errors to accidentaly activate the many drawn lines for non-zeroWidth-lines 

                currYOffset = currYOffset + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
                Rect space_of_connectionsType = new Rect(position.x, position.y + currYOffset, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(space_of_connectionsType, property.FindPropertyRelative("connectionsType"), new GUIContent("Connection style"));

                currYOffset = currYOffset + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
                Rect space_of_dataPointVisualization = new Rect(position.x, position.y + currYOffset, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(space_of_dataPointVisualization, property.FindPropertyRelative("dataPointVisualization"), new GUIContent("Points style"));

                bool datapointVisualizerOfLineAreInvisible = theDrawXXLChartInspector.DatapointVisualizerOfLineAreInvisible(serializedProperty_i_ofThisLineSpec_insideChartInspectorsLinesSpecsArray.intValue);
                EditorGUI.BeginDisabledGroup(datapointVisualizerOfLineAreInvisible);
                currYOffset = currYOffset + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
                Rect space_of_dataPointVisualization_size = new Rect(position.x, position.y + currYOffset, position.width, EditorGUIUtility.singleLineHeight);
                EditorGUI.PropertyField(space_of_dataPointVisualization_size, property.FindPropertyRelative("dataPointVisualization_size"), new GUIContent("Points size"));
                EditorGUI.EndDisabledGroup();

                currYOffset = currYOffset + (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
                GUIStyle style_forDatapointValuesHeadline = new GUIStyle(EditorStyles.foldout);
                style_forDatapointValuesHeadline.richText = true;
                SerializedProperty serializedProperty_datapointValuesSection_isExpanded = property.FindPropertyRelative("datapointValuesSection_isExpanded");
                Rect space_ofDatapointArrayFoldout = new Rect(position.x, position.y + currYOffset, position.width, EditorGUIUtility.singleLineHeight);
                serializedProperty_datapointValuesSection_isExpanded.boolValue = EditorGUI.Foldout(space_ofDatapointArrayFoldout, serializedProperty_datapointValuesSection_isExpanded.boolValue, "<color=#" + ColorUtility.ToHtmlStringRGBA(serializedProperty_lineColor.colorValue) + ">Datapoint Values</color>  (read only)", true, style_forDatapointValuesHeadline);

                if (serializedProperty_datapointValuesSection_isExpanded.boolValue)
                {
                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                    SerializedProperty arrayOfNeighboringDatapoints = property.FindPropertyRelative("neighboringDatapointValues");
                    int arraySize = arrayOfNeighboringDatapoints.arraySize;

                    currYOffset = currYOffset + 1.5f * EditorGUIUtility.singleLineHeight;
                    Rect space_ofArrayLengthChooser = new Rect(position.x, position.y + currYOffset, position.width, EditorGUIUtility.singleLineHeight);
                    EditorGUI.PropertyField(space_ofArrayLengthChooser, property.FindPropertyRelative("numberOfDisplayedDatapointsInArray"), new GUIContent("Listed values"));

                    currYOffset = currYOffset + 1.5f * EditorGUIUtility.singleLineHeight;
                    int i_insideShortenedForInspectorArray_markingTheValueAtCursor = property.FindPropertyRelative("i_insideShortenedForInspectorArray_markingTheValueAtCursor").intValue;
                    for (int i = 0; i < arraySize; i++)
                    {
                        SerializedProperty datapointValuesOfCurrentSlot = arrayOfNeighboringDatapoints.GetArrayElementAtIndex(i);
                        string slotName = GetArraySlotName(i, i_insideShortenedForInspectorArray_markingTheValueAtCursor);
                        Rect space_ofCurrDataPoint = new Rect(position.x, position.y + (currYOffset + (i * (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing))), position.width, EditorGUIUtility.singleLineHeight);
                        EditorGUI.PropertyField(space_ofCurrDataPoint, datapointValuesOfCurrentSlot, new GUIContent(slotName));
                    }
                }

                EditorGUI.indentLevel = previousIndent;
            }

        }

        string GetArraySlotName(int i, int i_insideShortenedForInspectorArray_markingTheValueAtCursor)
        {
            int indexesDistance_toCursor = i - i_insideShortenedForInspectorArray_markingTheValueAtCursor;
            if (indexesDistance_toCursor == 0)
            {
                return ("<b>At Cursor</b>");
            }
            else
            {
                if (indexesDistance_toCursor < 0)
                {
                    return ("Cursor -" + Mathf.Abs(indexesDistance_toCursor));
                }
                else
                {
                    return ("Cursor +" + Mathf.Abs(indexesDistance_toCursor));
                }
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.FindPropertyRelative("lineSection_isExpanded").boolValue)
            {
                if (property.FindPropertyRelative("datapointValuesSection_isExpanded").boolValue)
                {
                    int displayedDatapointValues = property.FindPropertyRelative("neighboringDatapointValues").arraySize;
                    return (EditorGUIUtility.singleLineHeight) * (15.0f + displayedDatapointValues) + (EditorGUIUtility.standardVerticalSpacing) * (13.0f + displayedDatapointValues);
                }
                else
                {
                    return (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * 13.0f;
                }
            }
            else
            {
                return EditorGUIUtility.singleLineHeight;
            }
        }
    }
#endif
}
