namespace DrawXXL
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(MeasurementVisualizer))]
    [CanEditMultipleObjects]
    public class MeasurementVisualizerInspector : VisualizerParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("measurement");

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            SerializedProperty sP_measurementType = serializedObject.FindProperty("measurementType");
            SerializedProperty sP_angleUnitToDisplay = serializedObject.FindProperty("angleUnitToDisplay");
            SerializedProperty sP_distanceThresholdType = serializedObject.FindProperty("distanceThresholdType");

            EditorGUILayout.PropertyField(sP_measurementType, new GUIContent("Measured quantity"));
            DrawResult(sP_measurementType, sP_angleUnitToDisplay);

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            Draw_geometrySpecification(sP_measurementType, sP_distanceThresholdType, sP_angleUnitToDisplay);

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            DrawAppearanceSection(sP_measurementType, sP_distanceThresholdType);
            DrawTextInputInclMarkupHelper();
            DrawCheckboxFor_drawOnlyIfSelected("measurement");
            DrawCheckboxFor_hiddenByNearerObjects("measurement");

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }

        void DrawResult(SerializedProperty sP_measurementType, SerializedProperty sP_angleUnitToDisplay)
        {
            SerializedProperty sP_measuredResultValue = serializedObject.FindProperty("measuredResultValue");
            string result_nameString = "";
            switch (sP_measurementType.enumValueIndex)
            {
                case (int)MeasurementVisualizer.MeasurementType.distanceBetweenPoints:
                    result_nameString = Get_resultNameString_forDistance();
                    break;
                case (int)MeasurementVisualizer.MeasurementType.distanceThresholdBetweenPoints:
                    break;
                case (int)MeasurementVisualizer.MeasurementType.distanceFromPointToLine:
                    result_nameString = Get_resultNameString_forDistance();
                    break;
                case (int)MeasurementVisualizer.MeasurementType.distanceFromLineToLine:
                    result_nameString = Get_resultNameString_forDistance();
                    break;
                case (int)MeasurementVisualizer.MeasurementType.distanceFromPointToPlane:
                    result_nameString = Get_resultNameString_forDistance();
                    break;
                case (int)MeasurementVisualizer.MeasurementType.distanceAlongOrthographicViewDir:
                    result_nameString = Get_resultNameString_forDistance();
                    break;
                case (int)MeasurementVisualizer.MeasurementType.distancePerpendicularToOrthographicViewDir:
                    result_nameString = Get_resultNameString_forDistance();
                    break;
                case (int)MeasurementVisualizer.MeasurementType.angleBetweenVectors:
                    result_nameString = Get_resultNameString_forAngle(sP_angleUnitToDisplay);
                    break;
                case (int)MeasurementVisualizer.MeasurementType.angleFromLineToPlane:
                    result_nameString = Get_resultNameString_forAngle(sP_angleUnitToDisplay);
                    break;
                case (int)MeasurementVisualizer.MeasurementType.angleFromPlaneToPlane:
                    result_nameString = Get_resultNameString_forAngle(sP_angleUnitToDisplay);
                    break;
                default:
                    break;
            }

            if (sP_measurementType.enumValueIndex != (int)MeasurementVisualizer.MeasurementType.distanceThresholdBetweenPoints)
            {
                EditorGUILayout.FloatField(new GUIContent(result_nameString, "read only"), sP_measuredResultValue.floatValue);
            }
        }

        public static string Get_resultNameString_forDistance()
        {
            return "Result (distance)";
        }

        public static string Get_resultNameString_forAngle(SerializedProperty sP_angleUnitToDisplay)
        {
            if (sP_angleUnitToDisplay.enumValueIndex == (int)MeasurementVisualizer.AngleUnit.degree)
            {
                return "Result (angle [deg])";
            }
            else
            {
                return "Result (angle [rad])";
            }
        }

        void Draw_geometrySpecification(SerializedProperty sP_measurementType, SerializedProperty sP_distanceThresholdType, SerializedProperty sP_angleUnitToDisplay)
        {
            switch (sP_measurementType.enumValueIndex)
            {
                case (int)MeasurementVisualizer.MeasurementType.distanceBetweenPoints:
                    Draw_specificationForTwoPoints("Start", "End", true, true);
                    break;
                case (int)MeasurementVisualizer.MeasurementType.distanceThresholdBetweenPoints:
                    Draw_specificationForTwoPoints("Start", "End", false, false);
                    EditorGUILayout.PropertyField(sP_distanceThresholdType, new GUIContent("Number of threshold distances"));
                    switch (sP_distanceThresholdType.enumValueIndex)
                    {
                        case (int)MeasurementVisualizer.DistanceThresholdType.one:
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("smallerThresholdDistance"), new GUIContent("Threshold distance"));
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("exactlyOnThresholdBehaviour"), new GUIContent("Distances exactly on the threshold"));
                            break;
                        case (int)MeasurementVisualizer.DistanceThresholdType.two:
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("smallerThresholdDistance"), new GUIContent("Small threshold distance"));
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("biggerThresholdDistance"), new GUIContent("Big threshold distance"));
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("exactlyOnThresholdBehaviour"), new GUIContent("Distances exactly on the threshold"));
                            break;
                        default:
                            break;
                    }

                    break;
                case (int)MeasurementVisualizer.MeasurementType.distanceFromPointToLine:
                    Draw_positionSpecificationForPointAtThisGameobject("Point", true);
                    Draw_lineAsGeoObject2_definedByPartnerGameobject("Line", true, serializedObject.FindProperty("minimumLineLength_forDistancePointToLine"));
                    break;
                case (int)MeasurementVisualizer.MeasurementType.distanceFromLineToLine:
                    Draw_lineAsGeoObject1_definedByThisGameobject("Line 1", serializedObject.FindProperty("minimumLineLength_forDistanceLineToLine"));
                    Draw_lineAsGeoObject2_definedByPartnerGameobject("Line 2", true, serializedObject.FindProperty("minimumLineLength_forDistanceLineToLine"));
                    break;
                case (int)MeasurementVisualizer.MeasurementType.distanceFromPointToPlane:
                    Draw_positionSpecificationForPointAtThisGameobject("Point", true);
                    Draw_planeAsGeoObject2_definedByPartnerGameobject("Plane", true);
                    break;
                case (int)MeasurementVisualizer.MeasurementType.distanceAlongOrthographicViewDir:
                    DrawSpecificationOf_customVector3_1_forDistanceInOrthogrphicViewDir();
                    Draw_specificationForTwoPoints("Point 1", "Point 2", true, true);
                    break;
                case (int)MeasurementVisualizer.MeasurementType.distancePerpendicularToOrthographicViewDir:
                    DrawSpecificationOf_customVector3_1_forDistanceInOrthogrphicViewDir();
                    Draw_specificationForTwoPoints("Point 1", "Point 2", true, true);
                    break;
                case (int)MeasurementVisualizer.MeasurementType.angleBetweenVectors:
                    DrawAngleVectorsWithColor();
                    EditorGUILayout.PropertyField(sP_angleUnitToDisplay, new GUIContent("Unit"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("addTextForAlternativeAngleUnit"), new GUIContent("Show also other angle unit (rad/deg)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("useReflexAngleOver180deg"), new GUIContent("Draw reflex angle over 180°"));
                    Draw_DrawPosition3DOffset_independentAlternativeValue(true, null, null, null, false);
                    break;
                case (int)MeasurementVisualizer.MeasurementType.angleFromLineToPlane:
                    Draw_lineAsGeoObject1_definedByThisGameobject("Line", serializedObject.FindProperty("minimumLineLength_forAngleLineToPlane"));
                    Draw_planeAsGeoObject2_definedByPartnerGameobject("Plane", false);
                    EditorGUILayout.PropertyField(sP_angleUnitToDisplay, new GUIContent("Unit"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("addTextForAlternativeAngleUnit"), new GUIContent("Show also other angle unit (rad/deg)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("returnObtuseAngleOver90deg"), new GUIContent("Draw obtuse angle over 90°"));
                    break;
                case (int)MeasurementVisualizer.MeasurementType.angleFromPlaneToPlane:
                    Draw_planeAsGeoObject1_definedByThisGameobject("Plane 1");
                    Draw_planeAsGeoObject2_definedByPartnerGameobject("Plane 2", false);
                    EditorGUILayout.PropertyField(sP_angleUnitToDisplay, new GUIContent("Unit"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("addTextForAlternativeAngleUnit"), new GUIContent("Show also other angle unit (rad/deg)"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("returnObtuseAngleOver90deg"), new GUIContent("Draw obtuse angle over 90°"));
                    break;
                default:
                    break;
            }
        }

        void GeoSpecsBlock_start(string name)
        {
            GUIStyle style_ofHeadline = new GUIStyle();
            style_ofHeadline.fontStyle = FontStyle.Bold;
            EditorGUILayout.LabelField(name, style_ofHeadline);
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
        }

        void GeoSpecsBlock_end(bool skipEmptyLineAtEnd)
        {
            if (skipEmptyLineAtEnd == false)
            {
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

        void Draw_specificationForTwoPoints(string nameOfFirstPoint, string nameOfSecondPoint, bool skipEmptyLineAtEnd, bool drawColorOfPoints)
        {
            Draw_positionSpecificationForPointAtThisGameobject(nameOfFirstPoint, drawColorOfPoints);
            Draw_positionSpecificationForEndPointDefinedByPartnerGameobject(nameOfSecondPoint, skipEmptyLineAtEnd, drawColorOfPoints);
        }

        void Draw_positionSpecificationForPointAtThisGameobject(string blockHeadline, bool drawColorOfPoint)
        {
            GeoSpecsBlock_start(blockHeadline);
            if (drawColorOfPoint) { EditorGUILayout.PropertyField(serializedObject.FindProperty("color1"), new GUIContent("Color")); }
            EditorGUILayout.LabelField("Position: This gameobject plus offset");
            Draw_DrawPosition3DOffset(true, "Additional offset");
            GeoSpecsBlock_end(false);
        }

        void Draw_positionSpecificationForEndPointDefinedByPartnerGameobject(string blockHeadline, bool skipEmptyLineAtEnd, bool drawColorOfPoint)
        {
            GeoSpecsBlock_start(blockHeadline);
            if (drawColorOfPoint) { EditorGUILayout.PropertyField(serializedObject.FindProperty("color2"), new GUIContent("Color")); }
            EditorGUILayout.PropertyField(serializedObject.FindProperty("partnerGameobject"), new GUIContent("Position"));
            Draw_DrawPosition3DOffset_ofPartnerGameobject(true, "Additional offset");
            GeoSpecsBlock_end(skipEmptyLineAtEnd);
        }

        void Draw_lineAsGeoObject1_definedByThisGameobject(string blockHeadline, SerializedProperty optionalLineLengthField)
        {
            Draw_lineOrPlaneAsGeoObject1_definedByThisGameobject(blockHeadline, "Direction", optionalLineLengthField);
        }

        void Draw_planeAsGeoObject1_definedByThisGameobject(string blockHeadline)
        {
            Draw_lineOrPlaneAsGeoObject1_definedByThisGameobject(blockHeadline, "Normal", null);
        }

        void Draw_lineOrPlaneAsGeoObject1_definedByThisGameobject(string blockHeadline, string meaningOfVector, SerializedProperty optionalLineLengthField)
        {
            GeoSpecsBlock_start(blockHeadline);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("color1"), new GUIContent("Color"));
            EditorGUILayout.LabelField("Origin: This gameobject plus offset");
            Draw_DrawPosition3DOffset(true, "Additional offset for origin");
            DrawSpecificationOf_customVector3_4(meaningOfVector, false, null, true, false, false, false);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("name_ofGeoObject1"), new GUIContent("Drawn name tag"));
            TryDrawLineLength(optionalLineLengthField);
            GeoSpecsBlock_end(false);
        }

        void Draw_lineAsGeoObject2_definedByPartnerGameobject(string blockHeadline, bool skipEmptyLineAtEnd, SerializedProperty optionalLineLengthField)
        {
            Draw_lineOrPlaneAsGeoObject2_definedByPartnerGameobject(blockHeadline, "Direction", skipEmptyLineAtEnd, optionalLineLengthField);
        }

        void Draw_planeAsGeoObject2_definedByPartnerGameobject(string blockHeadline, bool skipEmptyLineAtEnd)
        {
            Draw_lineOrPlaneAsGeoObject2_definedByPartnerGameobject(blockHeadline, "Normal", skipEmptyLineAtEnd, null);
        }

        void Draw_lineOrPlaneAsGeoObject2_definedByPartnerGameobject(string blockHeadline, string meaningOfVector, bool skipEmptyLineAtEnd, SerializedProperty optionalLineLengthField)
        {
            GeoSpecsBlock_start(blockHeadline);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("color2"), new GUIContent("Color"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("partnerGameobject"), new GUIContent("Origin position"));
            EditorGUI.BeginDisabledGroup(visualizerParentMonoBehaviour_unserialized.partnerGameobject == null);
            Draw_DrawPosition3DOffset_ofPartnerGameobject(true, "Additional offset for origin");
            DrawSpecificationOf_customVector3ofPartnerGameobject(meaningOfVector, false, null, true, false, false, false);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("name_ofGeoObject2"), new GUIContent("Drawn name tag"));
            TryDrawLineLength(optionalLineLengthField);
            EditorGUI.EndDisabledGroup();
            GeoSpecsBlock_end(skipEmptyLineAtEnd);
        }

        void TryDrawLineLength(SerializedProperty optionalLineLengthField)
        {
            if (optionalLineLengthField != null)
            {
                EditorGUILayout.PropertyField(optionalLineLengthField, new GUIContent("Line Length", "The length of the line is at least so big that it spans to all measurement participants. It can be further prolonged via this field here."));
            }
        }

        void DrawSpecificationOf_customVector3_1_forDistanceInOrthogrphicViewDir()
        {
            DrawSpecificationOf_customVector3_1("<b>View direction</b>", false, null, true, false, true, false);
        }

        void DrawAngleVectorsWithColor()
        {
            DrawSpecificationOf_customVector3_2("<b>Vector 1</b>", false, null, true, false, false, false);
            if (serializedObject.FindProperty("customVector3_2_picker_isOutfolded").boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("color1"), new GUIContent("Color"));
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }

            DrawSpecificationOf_customVector3_3("<b>Vector 2</b>", false, null, true, false, false, true);
            if (serializedObject.FindProperty("customVector3_3_picker_isOutfolded").boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("color2"), new GUIContent("Color"));
                GUILayout.Space(EditorGUIUtility.singleLineHeight);
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        void DrawAppearanceSection(SerializedProperty sP_measurementType, SerializedProperty sP_distanceThresholdType)
        {
            SerializedProperty sP_appearanceBlock_isOutfolded = serializedObject.FindProperty("appearanceBlock_isOutfolded");
            sP_appearanceBlock_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_appearanceBlock_isOutfolded.boolValue, "Appearance", true);
            if (sP_appearanceBlock_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                string descriptionFor_enlargeSmallTextToThisMinTextSize = "Enlarge small text to this minimum textsize";
                switch (sP_measurementType.enumValueIndex)
                {
                    case (int)MeasurementVisualizer.MeasurementType.distanceBetweenPoints:
                        Draw_coneConfig_insideFoldout_forStraightVectors();
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("enlargeSmallTextToThisMinTextSize"), new GUIContent(descriptionFor_enlargeSmallTextToThisMinTextSize));
                        break;
                    case (int)MeasurementVisualizer.MeasurementType.distanceThresholdBetweenPoints:
                        switch (sP_distanceThresholdType.enumValueIndex)
                        {
                            case (int)MeasurementVisualizer.DistanceThresholdType.one:

                                GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

                                EditorGUILayout.LabelField("Short lines");
                                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("overwriteColor_forNear_oneThresholdVersion"), new GUIContent("Color"));
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("overwriteStyle_forNear"), new GUIContent("Style"));
                                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

                                GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

                                EditorGUILayout.LabelField("Long lines");
                                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("overwriteColor_forFar_oneThresholdVersion"), new GUIContent("Color"));
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("overwriteStyle_forFar"), new GUIContent("Style"));
                                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

                                GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

                                break;
                            case (int)MeasurementVisualizer.DistanceThresholdType.two:

                                GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

                                EditorGUILayout.LabelField("Short lines");
                                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("overwriteColor_forNear_twoThresholdsVersion"), new GUIContent("Color"));
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("overwriteStyle_forNear"), new GUIContent("Style"));
                                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

                                GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

                                EditorGUILayout.LabelField("Middle lines");
                                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("overwriteColor_forMiddle_twoThresholdsVersion"), new GUIContent("Color"));
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("overwriteStyle_forMiddle"), new GUIContent("Style"));
                                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

                                GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

                                EditorGUILayout.LabelField("Long lines");
                                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("overwriteColor_forFar_twoThresholdsVersion"), new GUIContent("Color"));
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("overwriteStyle_forFar"), new GUIContent("Style"));
                                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

                                GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

                                break;
                            default:
                                break;
                        }

                        EditorGUILayout.PropertyField(serializedObject.FindProperty("displayDistanceAlsoAsText"), new GUIContent("Draw distance value"));
                        Draw_endPlatesConfig_insideFoldout();
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("enlargeSmallTextToThisMinTextSize"), new GUIContent(descriptionFor_enlargeSmallTextToThisMinTextSize));
                        break;
                    case (int)MeasurementVisualizer.MeasurementType.distanceFromPointToLine:
                        Draw_coneConfig_insideFoldout_forStraightVectors();
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("enlargeSmallTextToThisMinTextSize"), new GUIContent(descriptionFor_enlargeSmallTextToThisMinTextSize));
                        break;
                    case (int)MeasurementVisualizer.MeasurementType.distanceFromLineToLine:
                        Draw_coneConfig_insideFoldout_forStraightVectors();
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("enlargeSmallTextToThisMinTextSize"), new GUIContent(descriptionFor_enlargeSmallTextToThisMinTextSize));
                        break;
                    case (int)MeasurementVisualizer.MeasurementType.distanceFromPointToPlane:
                        Draw_coneConfig_insideFoldout_forStraightVectors();
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("enlargeSmallTextToThisMinTextSize"), new GUIContent(descriptionFor_enlargeSmallTextToThisMinTextSize));
                        break;
                    case (int)MeasurementVisualizer.MeasurementType.distanceAlongOrthographicViewDir:
                        Draw_coneConfig_insideFoldout_forStraightVectors();
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("enlargeSmallTextToThisMinTextSize"), new GUIContent(descriptionFor_enlargeSmallTextToThisMinTextSize));
                        break;
                    case (int)MeasurementVisualizer.MeasurementType.distancePerpendicularToOrthographicViewDir:
                        Draw_coneConfig_insideFoldout_forStraightVectors();
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("enlargeSmallTextToThisMinTextSize"), new GUIContent(descriptionFor_enlargeSmallTextToThisMinTextSize));
                        break;
                    case (int)MeasurementVisualizer.MeasurementType.angleBetweenVectors:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("drawBoundaryLines"), new GUIContent("Draw boundary lines"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("forceRadius_value"), new GUIContent("Radius"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("pointerConfigOfAngleBetweenVectors"), new GUIContent("Pointers"));
                        Draw_coneLength_forCircledVectors();
                        break;
                    case (int)MeasurementVisualizer.MeasurementType.angleFromLineToPlane:
                        Draw_coneLength_forCircledVectors();
                        break;
                    case (int)MeasurementVisualizer.MeasurementType.angleFromPlaneToPlane:
                        Draw_coneLength_forCircledVectors();
                        break;
                    default:
                        break;
                }

                EditorGUILayout.PropertyField(serializedObject.FindProperty("linesWidth"), new GUIContent("Lines width"));
                if (sP_measurementType.enumValueIndex != (int)MeasurementVisualizer.MeasurementType.distanceThresholdBetweenPoints)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("color"), new GUIContent("Color"));
                }

                GUILayout.Space(EditorGUIUtility.singleLineHeight);
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

    }
#endif
}
