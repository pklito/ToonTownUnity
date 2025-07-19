namespace DrawXXL
{
    using UnityEngine;
    using System;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(ShapeDrawer2D))]
    [CanEditMultipleObjects]
    public class ShapeDrawer2DInspector : VisualizerParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("shape2D");

            SerializedProperty sP_shapeType = serializedObject.FindProperty("shapeType");
            EditorGUILayout.PropertyField(sP_shapeType, new GUIContent("Shape type"));

            GUILayout.Space(0.75f * EditorGUIUtility.singleLineHeight);

            SerializedProperty sP_sizeDefinition = serializedObject.FindProperty("sizeDefinition");
            Draw_sizeInterpretationChooser(sP_sizeDefinition);
            DrawShapeSpecificOptions(sP_shapeType, sP_sizeDefinition);
            DrawGeneralOptions(sP_shapeType, sP_sizeDefinition);

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }

        void Draw_sizeInterpretationChooser(SerializedProperty sP_sizeDefinition)
        {
            EditorGUILayout.PropertyField(sP_sizeDefinition, new GUIContent("Size definition", "Some of the following parameters that define the size of the shape can be defined relative to a context of interest." + Environment.NewLine + "The values you specify in fields below like 'Radius' or 'Lines width' will be interpreted according to the setting here."));

            SerializedProperty sP_cameraForSizeDefinitionIsAvailable = serializedObject.FindProperty("cameraForSizeDefinitionIsAvailable");
            switch ((ShapeDrawer2D.ShapeSizeDefinition)sP_sizeDefinition.enumValueIndex)
            {
                case ShapeDrawer2D.ShapeSizeDefinition.relativeToGlobalScaleOfTheTransformUsingTheBiggestAbsoluteComponentButIgnoringZ:
                    break;
                case ShapeDrawer2D.ShapeSizeDefinition.absoluteUnits:
                    break;
                case ShapeDrawer2D.ShapeSizeDefinition.relativeToTheSceneViewWindowSize:
                    if (sP_cameraForSizeDefinitionIsAvailable.boolValue == false)
                    {
                        EditorGUILayout.HelpBox("Scene View Camera Window is not available", MessageType.Warning, true);
                    }
                    break;
                case ShapeDrawer2D.ShapeSizeDefinition.relativeToTheGameViewWindowSize:
                    if (sP_cameraForSizeDefinitionIsAvailable.boolValue == false)
                    {
                        EditorGUILayout.HelpBox("No Game View Camera found.", MessageType.Warning, true);
                    }
                    break;
                default:
                    break;
            }
        }

        void DrawShapeSpecificOptions(SerializedProperty sP_shapeType, SerializedProperty sP_sizeDefinition)
        {
            switch (sP_shapeType.enumValueIndex)
            {
                case (int)ShapeDrawer2D.ShapeType.circle:
                    Draw_sizeInterpretationDependentLine("radiusScaleFactor", "Radius", null, sP_sizeDefinition);
                    break;
                case (int)ShapeDrawer2D.ShapeType.ellipse:
                    Draw_sizeInterpretationDependentLine("width_scaleFactor_initialValue1", "Width", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("height_scaleFactor_initialValue2", "Height", null, sP_sizeDefinition);
                    break;
                case (int)ShapeDrawer2D.ShapeType.star:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("cornerOptionsForIrregularStar"), new GUIContent("Corners"));
                    Draw_sizeInterpretationDependentLine("width_scaleFactor_initialValue1", "Width", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("height_scaleFactor_initialValue1", "Height", null, sP_sizeDefinition);
                    break;
                case (int)ShapeDrawer2D.ShapeType.capsule:
                    Draw_sizeInterpretationDependentLine("width_scaleFactor_initialValue1", "Width", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("height_scaleFactor_initialValue2", "Height", null, sP_sizeDefinition);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("capusleDirection2D"), new GUIContent("Direction"));
                    break;
                case (int)ShapeDrawer2D.ShapeType.icon:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("iconType"), new GUIContent("Icon type"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("showAtlasOfAllAvailableIcons"), new GUIContent("Show atlas of all available icons"));
                    Draw_sizeInterpretationDependentLine("sizeOfIconScaleFactor", "Size", null, sP_sizeDefinition);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("iconIsMirroredHorizontally"), new GUIContent("Mirror horizontally"));
                    break;
                case (int)ShapeDrawer2D.ShapeType.triangle:
                    Draw_sizeInterpretationDependentLine("width_scaleFactor_initialValue1", "Width", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("height_scaleFactor_initialValue1", "Height", null, sP_sizeDefinition);
                    break;
                case (int)ShapeDrawer2D.ShapeType.square:
                    Draw_sizeInterpretationDependentLine("width_scaleFactor_initialValue1", "Width", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("height_scaleFactor_initialValue1", "Height", null, sP_sizeDefinition);
                    break;
                case (int)ShapeDrawer2D.ShapeType.pentagon:
                    Draw_sizeInterpretationDependentLine("width_scaleFactor_initialValue1", "Width", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("height_scaleFactor_initialValue1", "Height", null, sP_sizeDefinition);
                    break;
                case (int)ShapeDrawer2D.ShapeType.hexagon:
                    Draw_sizeInterpretationDependentLine("width_scaleFactor_initialValue1", "Width", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("height_scaleFactor_initialValue1", "Height", null, sP_sizeDefinition);
                    break;
                case (int)ShapeDrawer2D.ShapeType.septagon:
                    Draw_sizeInterpretationDependentLine("width_scaleFactor_initialValue1", "Width", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("height_scaleFactor_initialValue1", "Height", null, sP_sizeDefinition);
                    break;
                case (int)ShapeDrawer2D.ShapeType.octagon:
                    Draw_sizeInterpretationDependentLine("width_scaleFactor_initialValue1", "Width", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("height_scaleFactor_initialValue1", "Height", null, sP_sizeDefinition);
                    break;
                case (int)ShapeDrawer2D.ShapeType.decagon:
                    Draw_sizeInterpretationDependentLine("width_scaleFactor_initialValue1", "Width", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("height_scaleFactor_initialValue1", "Height", null, sP_sizeDefinition);
                    break;
                case (int)ShapeDrawer2D.ShapeType.dot:
                    Draw_sizeInterpretationDependentLine("sizeOfIconScaleFactor", "Size", null, sP_sizeDefinition);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("dotDensity"), new GUIContent("Fill density", "Raise this if you want the dot to be more opaque."));
                    break;
                default:
                    break;
            }
        }

        void DrawGeneralOptions(SerializedProperty sP_shapeType, SerializedProperty sP_sizeDefinition)
        {
            bool isIcon = (sP_shapeType.enumValueIndex == (int)ShapeDrawer2D.ShapeType.icon);
            bool isStar = (sP_shapeType.enumValueIndex == (int)ShapeDrawer2D.ShapeType.star);
            bool isDot = (sP_shapeType.enumValueIndex == (int)ShapeDrawer2D.ShapeType.dot);

            if (isDot == false)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("rotation_angleDegCC"), new GUIContent("Rotation"));
                Draw_sizeInterpretationDependentLine("linesWidth", "Lines width", null, sP_sizeDefinition);
            }

            if ((isIcon == false) && (isDot == false))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("lineStyle"), new GUIContent("Line style"));
                Draw_sizeInterpretationDependentLine("stylePatternScaleFactor", "Line style scaling", null, sP_sizeDefinition);
            }

            bool displayFillstyleOption = ((isIcon == false) && (isStar == false) && (isDot == false));
            if (displayFillstyleOption)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("fillStyle"), new GUIContent("Fill style"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("shapeFillDensity"), new GUIContent("Fill density"));
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("color"), new GUIContent("Color"));

            Draw_DrawPosition2DOffset();
            DrawZPosChooserFor2D();
            DrawTextSpecs(sP_sizeDefinition, isIcon, isDot);
            DrawCheckboxFor_drawOnlyIfSelected("shape");
            DrawCheckboxFor_hiddenByNearerObjects("shape2D");
        }

        void DrawTextSpecs(SerializedProperty sP_sizeDefinition, bool isIcon, bool isDot)
        {
            //-> this fakes the appearance of beeing inside the parents text specification foldout
            bool emptyLineAtEndIfOutfolded = false;
            DrawTextInputInclMarkupHelper(true, false, null, emptyLineAtEndIfOutfolded);

            if (serializedObject.FindProperty("textSection_isOutfolded").boolValue == true)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                if (CheckIf_shapeAttachedTextsizeReferenceContext_isUsed(sP_sizeDefinition, isIcon, isDot))
                {
                    DrawTextSizeChooser();
                }

                if ((isIcon == false) && (isDot == false))
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("textBlockAboveLine"), new GUIContent("Text block above line"));
                }

                GUILayout.Space(EditorGUIUtility.singleLineHeight);
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        bool CheckIf_shapeAttachedTextsizeReferenceContext_isUsed(SerializedProperty sP_sizeDefinition, bool isIcon, bool isDot)
        {
            if (CheckIf_shapeSizeDefinition_isDependentOn_screenspace(sP_sizeDefinition))
            {
                return false;
            }
            else
            {
                if (isIcon || isDot)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        void DrawTextSizeChooser()
        {
            EditorGUILayout.LabelField("Text Size");

            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            SerializedProperty sP_shapeAttachedTextsizeReferenceContext = serializedObject.FindProperty("shapeAttachedTextsizeReferenceContext");
            EditorGUILayout.PropertyField(sP_shapeAttachedTextsizeReferenceContext, new GUIContent("Relative to"));

            switch (sP_shapeAttachedTextsizeReferenceContext.enumValueIndex)
            {
                case (int)ShapeDrawer.ShapeAttachedTextsizeReferenceContext.sizeOfShape:
                    break;
                case (int)ShapeDrawer.ShapeAttachedTextsizeReferenceContext.globalSpace:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("textSize_value"), new GUIContent("Size per letter", "Text size in world units"));
                    break;
                case (int)ShapeDrawer.ShapeAttachedTextsizeReferenceContext.sceneViewWindowSize:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("textSize_value_relToScreen"), new GUIContent("Size per letter", "Text size relative to Scene View window size."));
                    break;
                case (int)ShapeDrawer.ShapeAttachedTextsizeReferenceContext.gameViewWindowSize:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("textSize_value_relToScreen"), new GUIContent("Size per letter", "Text size relative to Game View window size."));
                    break;
                default:
                    break;
            }

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

        bool CheckIf_shapeSizeDefinition_isDependentOn_screenspace(SerializedProperty sP_sizeDefinition)
        {
            return ((sP_sizeDefinition.enumValueIndex == (int)ShapeDrawer2D.ShapeSizeDefinition.relativeToTheSceneViewWindowSize) || (sP_sizeDefinition.enumValueIndex == (int)ShapeDrawer2D.ShapeSizeDefinition.relativeToTheGameViewWindowSize));
        }

        void Draw_sizeInterpretationDependentLine(string fieldName_withoutRelToScreenSuffix, string displayName, string tooltip, SerializedProperty sP_sizeDefinition)
        {
            GUIContent guiContent;
            string toolTipSuffix = "This is relative to the reference frame defined by 'Size definition'";
            if (tooltip == null)
            {
                guiContent = new GUIContent(displayName, toolTipSuffix);
            }
            else
            {
                guiContent = new GUIContent(displayName, tooltip + Environment.NewLine + Environment.NewLine + toolTipSuffix);
            }

            switch ((ShapeDrawer.ShapeSizeDefinition)sP_sizeDefinition.enumValueIndex)
            {
                case ShapeDrawer.ShapeSizeDefinition.relativeToTheGlobalScaleOfTheTransformRespectivelyItsBiggestAbsoluteComponent:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(fieldName_withoutRelToScreenSuffix), guiContent);
                    break;
                case ShapeDrawer.ShapeSizeDefinition.absoluteUnits:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(fieldName_withoutRelToScreenSuffix), guiContent);
                    break;
                case ShapeDrawer.ShapeSizeDefinition.relativeToTheSceneViewWindowSize:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(fieldName_withoutRelToScreenSuffix + "_relToScreen"), guiContent);
                    break;
                case ShapeDrawer.ShapeSizeDefinition.relativeToTheGameViewWindowSize:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty(fieldName_withoutRelToScreenSuffix + "_relToScreen"), guiContent);
                    break;
                default:
                    break;
            }
        }

    }
#endif
}
