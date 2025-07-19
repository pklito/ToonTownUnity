namespace DrawXXL
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(ShapeDrawerScreenspace))]
    [CanEditMultipleObjects]
    public class ShapeDrawerScreenspaceInspector : VisualizerScreenspaceParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("shape");
            if (DrawCameraChooser(true))
            {
                GUILayout.Space(EditorGUIUtility.singleLineHeight);

                SerializedProperty sP_shapeType = serializedObject.FindProperty("shapeType");
                EditorGUILayout.PropertyField(sP_shapeType, new GUIContent("Shape type"));

                GUILayout.Space(0.75f * EditorGUIUtility.singleLineHeight);

                EditorGUILayout.PropertyField(serializedObject.FindProperty("positionInsideViewport0to1"), new GUIContent("Position (inside viewport)"));
                DrawShapeSpecificOptions(sP_shapeType);
                DrawGeneralOptions(sP_shapeType);
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }

        void DrawShapeSpecificOptions(SerializedProperty sP_shapeType)
        {

            switch (sP_shapeType.enumValueIndex)
            {
                case (int)ShapeDrawer2D.ShapeType.circle:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("radius_relToViewportHeight"), new GUIContent("Radius", tooltip_explaining_relativeToViewPortHeight));
                    break;
                case (int)ShapeDrawer2D.ShapeType.ellipse:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("width_relToViewportHeight_initialValue01"), new GUIContent("Width", tooltip_explaining_relativeToViewPortHeight));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("height_relToViewportHeight_initialValue02"), new GUIContent("Height", tooltip_explaining_relativeToViewPortHeight));
                    break;
                case (int)ShapeDrawer2D.ShapeType.star:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("cornerOptionsForIrregularStar"), new GUIContent("Corners"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("width_relToViewportHeight_initialValue01"), new GUIContent("Width", tooltip_explaining_relativeToViewPortHeight));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("height_relToViewportHeight_initialValue01"), new GUIContent("Height", tooltip_explaining_relativeToViewPortHeight));
                    break;
                case (int)ShapeDrawer2D.ShapeType.capsule:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("width_relToViewportHeight_initialValue01"), new GUIContent("Width", tooltip_explaining_relativeToViewPortHeight));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("height_relToViewportHeight_initialValue02"), new GUIContent("Height", tooltip_explaining_relativeToViewPortHeight));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("capusleDirection2D"), new GUIContent("Direction"));
                    break;
                case (int)ShapeDrawer2D.ShapeType.icon:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("iconType"), new GUIContent("Icon type"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("sizeOfIcon_relToViewportHeight"), new GUIContent("Size", tooltip_explaining_relativeToViewPortHeight));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("iconIsMirroredHorizontally"), new GUIContent("Mirror horizontally"));
                    break;
                case (int)ShapeDrawer2D.ShapeType.triangle:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("width_relToViewportHeight_initialValue01"), new GUIContent("Width", tooltip_explaining_relativeToViewPortHeight));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("height_relToViewportHeight_initialValue01"), new GUIContent("Height", tooltip_explaining_relativeToViewPortHeight));
                    break;
                case (int)ShapeDrawer2D.ShapeType.square:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("width_relToViewportHeight_initialValue01"), new GUIContent("Width", tooltip_explaining_relativeToViewPortHeight));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("height_relToViewportHeight_initialValue01"), new GUIContent("Height", tooltip_explaining_relativeToViewPortHeight));
                    break;
                case (int)ShapeDrawer2D.ShapeType.pentagon:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("width_relToViewportHeight_initialValue01"), new GUIContent("Width", tooltip_explaining_relativeToViewPortHeight));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("height_relToViewportHeight_initialValue01"), new GUIContent("Height", tooltip_explaining_relativeToViewPortHeight));
                    break;
                case (int)ShapeDrawer2D.ShapeType.hexagon:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("width_relToViewportHeight_initialValue01"), new GUIContent("Width", tooltip_explaining_relativeToViewPortHeight));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("height_relToViewportHeight_initialValue01"), new GUIContent("Height", tooltip_explaining_relativeToViewPortHeight));
                    break;
                case (int)ShapeDrawer2D.ShapeType.septagon:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("width_relToViewportHeight_initialValue01"), new GUIContent("Width", tooltip_explaining_relativeToViewPortHeight));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("height_relToViewportHeight_initialValue01"), new GUIContent("Height", tooltip_explaining_relativeToViewPortHeight));
                    break;
                case (int)ShapeDrawer2D.ShapeType.octagon:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("width_relToViewportHeight_initialValue01"), new GUIContent("Width", tooltip_explaining_relativeToViewPortHeight));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("height_relToViewportHeight_initialValue01"), new GUIContent("Height", tooltip_explaining_relativeToViewPortHeight));
                    break;
                case (int)ShapeDrawer2D.ShapeType.decagon:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("width_relToViewportHeight_initialValue01"), new GUIContent("Width", tooltip_explaining_relativeToViewPortHeight));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("height_relToViewportHeight_initialValue01"), new GUIContent("Height", tooltip_explaining_relativeToViewPortHeight));
                    break;
                case (int)ShapeDrawer2D.ShapeType.dot:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("radius_relToViewportHeight"), new GUIContent("Radius", tooltip_explaining_relativeToViewPortHeight));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("dotDensity"), new GUIContent("Fill density", "Raise this if you want the dot to be more opaque."));
                    break;
                default:
                    break;
            }
        }

        void DrawGeneralOptions(SerializedProperty sP_shapeType)
        {
            bool isIcon = (sP_shapeType.enumValueIndex == (int)ShapeDrawer2D.ShapeType.icon);
            bool isDot = (sP_shapeType.enumValueIndex == (int)ShapeDrawer2D.ShapeType.dot);
            bool isStar = (sP_shapeType.enumValueIndex == (int)ShapeDrawer2D.ShapeType.star);

            if (isDot == false)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("zRotationDegCC"), new GUIContent("Rotation"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("linesWidth_relToViewportHeight"), new GUIContent("Lines width", tooltip_explaining_relativeToViewPortHeight));
            }

            if ((isIcon == false) && (isDot == false))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("lineStyle"), new GUIContent("Line style"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("stylePatternScaleFactor"), new GUIContent("Line style scaling"));
            }

            bool displayFillstyleOption = ((isIcon == false) && (isDot == false) && (isStar == false));
            if (displayFillstyleOption)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("fillStyle"), new GUIContent("Fill style"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("shapeFillDensity"), new GUIContent("Fill density"));
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("color"), new GUIContent("Color"));

            EditorGUILayout.PropertyField(serializedObject.FindProperty("drawPointerIfOffscreen"), new GUIContent("Draw pointer if off screen"));
            if ((isIcon == false) && (isDot == false))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("addTextForOutsideDistance_toOffscreenPointer"), new GUIContent("Add text for outside screen distance", "This only applies if the shape position is outside of the viewport."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("drawHullEdgeLines_forScreenEncasingShapes"), new GUIContent("Draw indicator for screen encasing shapes", "This helps to identify and locate shapes that are (partly) bigger than the screen."));
            }

            DrawTextInputInclMarkupHelper();
            DrawCheckboxFor_drawOnlyIfSelected("shape");
        }
    }
#endif
}
