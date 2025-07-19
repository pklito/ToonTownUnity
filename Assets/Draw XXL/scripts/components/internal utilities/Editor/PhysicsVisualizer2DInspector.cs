namespace DrawXXL
{
    using UnityEngine;
    using System;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(PhysicsVisualizer2D))]
    [CanEditMultipleObjects]
    public class PhysicsVisualizer2DInspector : VisualizerParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("physics2D");
            SerializedProperty sP_saveDrawnLinesType = serializedObject.FindProperty("saveDrawnLinesType");
            EditorGUILayout.PropertyField(sP_saveDrawnLinesType, new GUIContent("Save drawn lines ?", "This simplifies the visualization. It may be helpful to save performance if you draw many casts at once." + Environment.NewLine + Environment.NewLine + "You can change the initial value of this via 'DrawXXL.DrawPhysics2D.visualizationQuality'."));
            GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

            bool typeIsCast_notOverlap = false;
            SerializedProperty sP_theGameobjectHasACompatibleAndEnabledCollider = serializedObject.FindProperty("theGameobjectHasACompatibleAndEnabledCollider");
            SerializedProperty sP_shape = serializedObject.FindProperty("shape");

            TryDrawCollisionTypeChooser(sP_shape, sP_theGameobjectHasACompatibleAndEnabledCollider, ref typeIsCast_notOverlap);

            if ((sP_shape.enumValueIndex == (int)PhysicsVisualizer2D.Shape.collidersOnThisGameobject) && (sP_theGameobjectHasACompatibleAndEnabledCollider.boolValue == false))
            {
                EditorGUILayout.HelpBox("There is no enabled collider on this gameobject that could be cast. Supported colliders are 'BoxCollider2D', 'CircleCollider2D' and 'CapsuleCollider2D'.", MessageType.None, true);
            }
            else
            {
                SerializedProperty sP_wantedHits = serializedObject.FindProperty("wantedHits");
                EditorGUILayout.PropertyField(sP_wantedHits, new GUIContent("Wanted hits"));
                DrawDetectedHitsCount();

                GUILayout.Space(EditorGUIUtility.singleLineHeight);

                DrawShapeSizeSpecification(sP_shape);
                DrawCastDirectionAndDistance(sP_shape, typeIsCast_notOverlap);
                DrawOtherSettings(sP_shape, sP_wantedHits, sP_saveDrawnLinesType, typeIsCast_notOverlap);
                DrawTextSpecs(sP_saveDrawnLinesType, typeIsCast_notOverlap);
                DrawCheckboxFor_drawOnlyIfSelected("physics2D");
                DrawCheckboxFor_hiddenByNearerObjects("physics2D");
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }

        void TryDrawCollisionTypeChooser(SerializedProperty sP_shape, SerializedProperty sP_theGameobjectHasACompatibleAndEnabledCollider, ref bool typeIsCast_notOverlap)
        {
            SerializedProperty sP_collisionType = serializedObject.FindProperty("collisionType");

            bool forceDisplayToCast = false;
            switch ((PhysicsVisualizer2D.Shape)sP_shape.enumValueIndex)
            {
                case PhysicsVisualizer2D.Shape.collidersOnThisGameobject:
                    if (sP_theGameobjectHasACompatibleAndEnabledCollider.boolValue)
                    {
                        forceDisplayToCast = false;
                        typeIsCast_notOverlap = Get_typeIsCast_notOverlap(sP_collisionType, forceDisplayToCast);
                    }
                    break;
                case PhysicsVisualizer2D.Shape.box:
                    forceDisplayToCast = false;
                    typeIsCast_notOverlap = Get_typeIsCast_notOverlap(sP_collisionType, forceDisplayToCast);
                    break;
                case PhysicsVisualizer2D.Shape.circle:
                    forceDisplayToCast = false;
                    typeIsCast_notOverlap = Get_typeIsCast_notOverlap(sP_collisionType, forceDisplayToCast);
                    break;
                case PhysicsVisualizer2D.Shape.capsule:
                    forceDisplayToCast = false;
                    typeIsCast_notOverlap = Get_typeIsCast_notOverlap(sP_collisionType, forceDisplayToCast);
                    break;
                case PhysicsVisualizer2D.Shape.rayOrPoint:
                    forceDisplayToCast = false;
                    typeIsCast_notOverlap = Get_typeIsCast_notOverlap(sP_collisionType, forceDisplayToCast);
                    break;
                case PhysicsVisualizer2D.Shape.ray3D:
                    forceDisplayToCast = true;
                    typeIsCast_notOverlap = Get_typeIsCast_notOverlap(sP_collisionType, forceDisplayToCast);
                    break;
                default:
                    break;
            }

            string label_ofShape = typeIsCast_notOverlap ? "Shape to cast" : "Shape to overlap";
            EditorGUILayout.PropertyField(sP_shape, new GUIContent(label_ofShape));

            switch ((PhysicsVisualizer2D.Shape)sP_shape.enumValueIndex)
            {
                case PhysicsVisualizer2D.Shape.collidersOnThisGameobject:
                    if (sP_theGameobjectHasACompatibleAndEnabledCollider.boolValue)
                    {
                        if (UtilitiesDXXL_EngineBasics.CheckIf_transformOrAParentHasNonUniformScale_2D(transform_onVisualizerObject.parent))
                        {
                            EditorGUILayout.HelpBox("A parent transform has a non-uniform scale -> The collider and cast positions may be wrong.", MessageType.Warning, true);
                        }

                        if (UtilitiesDXXL_EngineBasics.CheckIfThisOrAParentHasANonZRotation_2D(transform_onVisualizerObject))
                        {
                            EditorGUILayout.HelpBox("This transform or a parent has a non-z rotation. -> The collider and cast positions may be wrong.", MessageType.Warning, true);
                        }

                        if (serializedObject.FindProperty("aVisualizedBoxCollider2DOnThisComponent_hasANonZeroEdge").boolValue)
                        {
                            EditorGUILayout.HelpBox("A visualized BoxCollider2D component uses 'edge radius'. Casting rounded boxes is not supported." + Environment.NewLine + "-> Fallback to casting the box without the rounded edge.", MessageType.Warning, true);
                        }

                        if (serializedObject.FindProperty("aVisualizedBoxCollider2DOnThisComponent_hasAutoTiling").boolValue)
                        {
                            EditorGUILayout.HelpBox("A visualized BoxCollider2D component uses 'Auto Tiling'. The visualisation of this is not supported.", MessageType.Warning, true);
                        }

                        DrawCollisionTypeChooser(sP_collisionType, false);
                    }
                    break;
                case PhysicsVisualizer2D.Shape.box:
                    DrawCollisionTypeChooser(sP_collisionType, false);
                    break;
                case PhysicsVisualizer2D.Shape.circle:
                    DrawCollisionTypeChooser(sP_collisionType, false);
                    break;
                case PhysicsVisualizer2D.Shape.capsule:
                    DrawCollisionTypeChooser(sP_collisionType, false);
                    break;
                case PhysicsVisualizer2D.Shape.rayOrPoint:
                    DrawCollisionTypeChooser(sP_collisionType, false);
                    break;
                case PhysicsVisualizer2D.Shape.ray3D:
                    DrawCollisionTypeChooser(sP_collisionType, true);
                    break;
                default:
                    break;
            }
        }

        bool Get_typeIsCast_notOverlap(SerializedProperty sP_collisionType, bool forceDisplayToCast)
        {
            if (forceDisplayToCast)
            {
                return true;
            }
            else
            {
                return (sP_collisionType.enumValueIndex == (int)PhysicsVisualizer2D.CollisionType.cast);
            }
        }

        void DrawCollisionTypeChooser(SerializedProperty sP_collisionType, bool forceDisplayToCast)
        {
            string label = "Collision type";
            if (forceDisplayToCast)
            {
                PhysicsVisualizer2D.CollisionType castCollisionType = PhysicsVisualizer2D.CollisionType.cast;

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.EnumPopup(new GUIContent(label), castCollisionType);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUILayout.PropertyField(sP_collisionType, new GUIContent(label));
            }
        }

        void DrawDetectedHitsCount()
        {
            int numberOfDetectedHits = serializedObject.FindProperty("numberOfFoundHits").intValue;
            Color displayColor = (numberOfDetectedHits == 0) ? serializedObject.FindProperty("colorForNonHittingCasts").colorValue : serializedObject.FindProperty("colorForHittingCasts").colorValue;
            string labelText_name = "Detected hits:"; //-> I wanted to color this as well, but rich text seems to work only for the second label
            string labelText_value = ("<b><color=#" + ColorUtility.ToHtmlStringRGBA(displayColor) + ">" + numberOfDetectedHits + "</color></b>");
            GUIStyle style = new GUIStyle(EditorStyles.label);
            style.richText = true;
            EditorGUILayout.LabelField(labelText_name, labelText_value, style);
        }

        void DrawShapeSizeSpecification(SerializedProperty sP_shape)
        {
            if ((sP_shape.enumValueIndex == (int)PhysicsVisualizer2D.Shape.box) || (sP_shape.enumValueIndex == (int)PhysicsVisualizer2D.Shape.circle) || (sP_shape.enumValueIndex == (int)PhysicsVisualizer2D.Shape.capsule))
            {
                GUIStyle richtextEnabledStyle = new GUIStyle(EditorStyles.label);
                richtextEnabledStyle.richText = true;

                switch ((PhysicsVisualizer2D.Shape)sP_shape.enumValueIndex)
                {
                    case PhysicsVisualizer2D.Shape.collidersOnThisGameobject:
                        break;
                    case PhysicsVisualizer2D.Shape.box:
                        EditorGUILayout.LabelField("<b>Box:</b>    --- Size / Orientation / Position ---", richtextEnabledStyle);
                        break;
                    case PhysicsVisualizer2D.Shape.circle:
                        EditorGUILayout.LabelField("<b>Circle:</b>    --- Size / Position ---", richtextEnabledStyle);
                        break;
                    case PhysicsVisualizer2D.Shape.capsule:
                        EditorGUILayout.LabelField("<b>Capsule:</b>    --- Size / Orientation / Position ---", richtextEnabledStyle);
                        break;
                    case PhysicsVisualizer2D.Shape.rayOrPoint:
                        break;
                    case PhysicsVisualizer2D.Shape.ray3D:
                        break;
                    default:
                        break;
                }

                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                switch ((PhysicsVisualizer2D.Shape)sP_shape.enumValueIndex)
                {
                    case PhysicsVisualizer2D.Shape.collidersOnThisGameobject:
                        break;
                    case PhysicsVisualizer2D.Shape.box:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("sizeScaleFactors_ofCastRespCheckedBox"), new GUIContent("Size"));
                        DrawShapeRotation();
                        Draw_DrawPosition2DOffset(true, "Position Offset");
                        DrawZPosChooserFor2D(true);
                        break;
                    case PhysicsVisualizer2D.Shape.circle:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("radiusScaleFactor_ofCastRespCheckedCircle"), new GUIContent("Radius"));
                        Draw_DrawPosition2DOffset(true, "Position Offset");
                        DrawZPosChooserFor2D(true);
                        break;
                    case PhysicsVisualizer2D.Shape.capsule:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("sizeScaleFactors_ofCastRespCheckedCapsule"), new GUIContent("Size"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("capsuleDirection2D_ofManuallyConstructedCapsuleMeaningNotFromCollider"), new GUIContent("Capsule Expansion"));
                        DrawShapeRotation();
                        Draw_DrawPosition2DOffset(true, "Position Offset");
                        DrawZPosChooserFor2D(true);
                        break;
                    case PhysicsVisualizer2D.Shape.rayOrPoint:
                        break;
                    case PhysicsVisualizer2D.Shape.ray3D:
                        break;
                    default:
                        break;
                }
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }
            else
            {
                if (sP_shape.enumValueIndex == (int)PhysicsVisualizer2D.Shape.rayOrPoint)
                {
                    Draw_DrawPosition2DOffset(false, "Position Offset");
                    DrawZPosChooserFor2D();
                }

                if (sP_shape.enumValueIndex == (int)PhysicsVisualizer2D.Shape.ray3D)
                {
                    Draw_DrawPosition3DOffset(false, "Position Offset");
                    DrawZPosChooserFor2D();
                }
            }
        }

        void DrawShapeRotation()
        {
            SerializedProperty sP_shapeRotationType = serializedObject.FindProperty("shapeRotationType");
            EditorGUILayout.PropertyField(sP_shapeRotationType, new GUIContent("Rotation"));

            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            switch (sP_shapeRotationType.enumValueIndex)
            {
                case (int)PhysicsVisualizer2D.ShapeRotationType.transformsRotationPlusOptionalAdditionalRotation:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("rotationAngleOfShape_additionallyToTransformsAngle"), new GUIContent("Additional rotation", "The rotation is taken from the transforms z rotation. This value is added onto this."));
                    break;
                case (int)PhysicsVisualizer2D.ShapeRotationType.customRotationIndependentFromTransform:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("rotationAngleOfShape_additionallyToTransformsAngle"), new GUIContent("Custom rotation"));
                    break;
                default:
                    break;
            }

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }
        void DrawCastDirectionAndDistance(SerializedProperty sP_shape, bool typeIsCast_notOverlap)
        {
            if (typeIsCast_notOverlap)
            {
                if (sP_shape.enumValueIndex == (int)PhysicsVisualizer2D.Shape.ray3D)
                {
                    DrawSpecificationOf_customVector3_1("Cast direction", false, null, true, false, true, false);
                }
                else
                {
                    DrawSpecificationOf_customVector2_1("Cast direction", false, null, true, false, true, false);
                }

                EditorGUILayout.LabelField("Cast distance");
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                SerializedProperty sP_distanceIsInfinityRespToOtherGO = serializedObject.FindProperty("distanceIsInfinityRespToOtherGO");

                if (sP_shape.enumValueIndex == (int)PhysicsVisualizer2D.Shape.ray3D)
                {
                    if (visualizerParentMonoBehaviour_unserialized.source_ofCustomVector3_1 == VisualizerParent.CustomVector3Source.toOtherGameobject)
                    {
                        EditorGUILayout.PropertyField(sP_distanceIsInfinityRespToOtherGO, new GUIContent("Till other gameobject"));
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(sP_distanceIsInfinityRespToOtherGO, new GUIContent("Infinity"));
                    }
                }
                else
                {
                    if (visualizerParentMonoBehaviour_unserialized.source_ofCustomVector2_1 == VisualizerParent.CustomVector2Source.toOtherGameobject)
                    {
                        EditorGUILayout.PropertyField(sP_distanceIsInfinityRespToOtherGO, new GUIContent("Till other gameobject"));
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(sP_distanceIsInfinityRespToOtherGO, new GUIContent("Infinity"));
                    }
                }

                EditorGUI.BeginDisabledGroup(sP_distanceIsInfinityRespToOtherGO.boolValue);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("adjustedDistance"), new GUIContent("Flexible"));
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }
        }

        void DrawOtherSettings(SerializedProperty sP_shape, SerializedProperty sP_wantedHits, SerializedProperty sP_saveDrawnLinesType, bool typeIsCast_notOverlap)
        {
            SerializedProperty sP_otherSettings_isFoldedOut = serializedObject.FindProperty("otherSettings_isFoldedOut");
            sP_otherSettings_isFoldedOut.boolValue = EditorGUILayout.Foldout(sP_otherSettings_isFoldedOut.boolValue, "Other settings", true);
            if (sP_otherSettings_isFoldedOut.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                EditorGUILayout.PropertyField(serializedObject.FindProperty("excludeCollidersOnThisGO"), new GUIContent("Exclude colliders on this Gameobject from result", "Caution for disabling this:" + Environment.NewLine + "In many cases the colliders on this Gameobject will overlap with the starting position of the cast shapes. In these cases a collision will be detected, but the position and hit distance may be wrong."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("excludeCollidersOnParentGOs"), new GUIContent("Exclude colliders on parents from result", "Caution for disabling this:" + Environment.NewLine + "In many cases the colliders on the parents will overlap with the starting position of the cast shapes. In these cases a collision will be detected, but the position and hit distance may be wrong."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("excludeCollidersOnChildrenGOs"), new GUIContent("Exclude colliders on children from result", "Caution for disabling this:" + Environment.NewLine + "In many cases the colliders on the children will overlap with the starting position of the cast shapes. In these cases a collision will be detected, but the position and hit distance may be wrong."));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("layerMask"), new GUIContent("Colliding layers", "The collision check considers only colliders that are part of the herewith selected layers." + Environment.NewLine + Environment.NewLine + "For more details see Unitys documentation of the 'layerMask' parameter inside the 'UnityEngine.Physics2D.*' functions documentation."));

                bool castShape_isRay3D = (sP_shape.enumValueIndex == (int)PhysicsVisualizer2D.Shape.ray3D);
                bool wantedHits_isAll = (sP_wantedHits.enumValueIndex == (int)PhysicsVisualizer2D.WantedHits.all);

                //depth restriction:
                SerializedProperty sP_useDepth = serializedObject.FindProperty("useDepth");

                EditorGUI.BeginDisabledGroup(castShape_isRay3D);
                EditorGUILayout.PropertyField(sP_useDepth, new GUIContent("Restrict Z range", "When using this the collision results will only include Collider2D who have their Z coordinate inside the specified range." + Environment.NewLine + Environment.NewLine + "This is not available for 'Shape' of 'Ray 3D'." + Environment.NewLine + Environment.NewLine + "For more details see Unitys documentation of 'ContactFilter2D'."));
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                EditorGUI.BeginDisabledGroup(sP_useDepth.boolValue == false);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("minDepth"), new GUIContent("Minimum Z"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("maxDepth"), new GUIContent("Maximum Z"));

                EditorGUI.BeginDisabledGroup(wantedHits_isAll == false);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("useOutsideDepth"), new GUIContent("Restrict to OUTSIDE", "Restrict the collisions to the area outside of the range, instead of to area inside the range." + Environment.NewLine + Environment.NewLine + "This is only available if 'Wanted hits' is set to 'All'. And it is not available for 'Shape' of 'Ray 3D'."));
                EditorGUI.EndDisabledGroup();

                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                EditorGUI.EndDisabledGroup();

                //normal angle restriction:
                SerializedProperty sP_useNormalAngle = serializedObject.FindProperty("useNormalAngle");
                EditorGUI.BeginDisabledGroup(castShape_isRay3D || (typeIsCast_notOverlap == false) || (wantedHits_isAll == false));
                EditorGUILayout.PropertyField(sP_useNormalAngle, new GUIContent("Restrict by normal angle", "This restricts the result to collisons whose collision normal points into a direction that lies inside the specified angle segment. The angle segment is defined in global space and independent of the cast direction. An angle of 0 means 'towards global right' (positive x-axis). Higher angles are turned counter clockwise from there." + Environment.NewLine + Environment.NewLine + "This is only available for casts with 'Wanted hits' set to 'All'. And it is not available for 'Shape' of 'Ray 3D' and not for colliders that are configurated as triggers and not for overlap checks." + Environment.NewLine + Environment.NewLine + "For more details see Unitys documentation of 'ContactFilter2D'."));

                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUI.BeginDisabledGroup(sP_useNormalAngle.boolValue == false);

                SerializedProperty sP_minNormalAngle = serializedObject.FindProperty("minNormalAngle");
                SerializedProperty sP_maxNormalAngle = serializedObject.FindProperty("maxNormalAngle");

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(sP_minNormalAngle, new GUIContent("Minimum normal angle", "The start of the segment that defines the allowed normal angles (in degrees). It is measured in global space counter clockwise from 'towards right' (positive x-axis)"));
                bool minNormalAngle_changed = EditorGUI.EndChangeCheck();

                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(sP_maxNormalAngle, new GUIContent("Maximum normal angle", "The end of the segment that defines the allowed normal angles (in degrees). It is measured in global space counter clockwise from 'towards right' (positive x-axis)"));
                bool maxNormalAngle_changed = EditorGUI.EndChangeCheck();

                if (minNormalAngle_changed)
                {
                    sP_maxNormalAngle.floatValue = Mathf.Max(sP_minNormalAngle.floatValue, sP_maxNormalAngle.floatValue);
                }
                else
                {
                    if (maxNormalAngle_changed)
                    {
                        sP_minNormalAngle.floatValue = Mathf.Min(sP_minNormalAngle.floatValue, sP_maxNormalAngle.floatValue);
                    }
                }

                // The angle range via only single slider, that displays a span:
                // (disadvantage: it doesn't display the values as numbers)
                //  float minNormalAngle = sP_minNormalAngle.floatValue;
                //  float maxNormalAngle = sP_maxNormalAngle.floatValue;
                //  EditorGUILayout.MinMaxSlider("Angle Segment (0°-360°)",ref minNormalAngle, ref maxNormalAngle, 0.0f, 360.0f);
                //  sP_minNormalAngle.floatValue = minNormalAngle;
                //  sP_maxNormalAngle.floatValue = maxNormalAngle;

                EditorGUILayout.PropertyField(serializedObject.FindProperty("useOutsideNormalAngle"), new GUIContent("Restrict to OUTSIDE", "Restrict the collisions to the normal angles outside of the defined segment, instead of to the angles inside the segment."));
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                EditorGUI.EndDisabledGroup();

                //others:
                EditorGUI.BeginDisabledGroup(castShape_isRay3D || (wantedHits_isAll == false));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("useTriggers"), new GUIContent("Collide with triggers", "Specifies wheather the collision check also interacts with colliders that are configured as triggers." + Environment.NewLine + " Note that the restiction of 'normal angles' doesn't work on triggers." + Environment.NewLine + Environment.NewLine + "This is only available if 'Wanted hits' is set to 'All'. And it is not available for 'Shape' of 'Ray 3D'."));
                EditorGUI.EndDisabledGroup();

                string displayNameOfColorForNonHittingChecks;
                string displayNameOfColorForHittingChecks;
                if (typeIsCast_notOverlap)
                {
                    displayNameOfColorForNonHittingChecks = "Non hitting rays";
                    displayNameOfColorForHittingChecks = "Hitting rays";
                }
                else
                {
                    displayNameOfColorForNonHittingChecks = "Not Overlapping";
                    displayNameOfColorForHittingChecks = "Overlapping";
                }

                EditorGUILayout.PropertyField(serializedObject.FindProperty("colorForNonHittingCasts"), new GUIContent(displayNameOfColorForNonHittingChecks, "You can change the initial value of this via 'DrawXXL.DrawPhysics2D.colorForNonHittingCasts'."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("colorForHittingCasts"), new GUIContent(displayNameOfColorForHittingChecks, "You can change the initial value of this via 'DrawXXL.DrawPhysics2D.colorForHittingCasts'."));

                if (typeIsCast_notOverlap)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("colorForCastLineBeyondHit"), new GUIContent("After last hit", "You can change the initial value of this via 'DrawXXL.DrawPhysics2D.colorForCastLineBeyondHit'."));
                    DrawChooserLineFor_overwriteColorForCastsHitNormals();
                    Draw_castCorridorVisualizerDensity(sP_saveDrawnLinesType);
                }

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }
        }

        void DrawChooserLineFor_overwriteColorForCastsHitNormals()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("doOverwriteColorForCastsHitNormals"), new GUIContent("Custom Hit Normal Color", "The default color for normals at collision positions is that 'Color of hitting rays' is used, but with an adjusted brightness. Though you can overwrite the color of the normal here."));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("overwriteColorForCastsHitNormals"), GUIContent.none);
            EditorGUILayout.EndHorizontal();
        }

        void Draw_castCorridorVisualizerDensity(SerializedProperty sP_saveDrawnLinesType)
        {
            if (sP_saveDrawnLinesType.enumValueIndex == (int)PhysicsVisualizer.SaveDrawnLinesType.yes_displayWithLowDetails)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("castCorridorVisualizerDensity"), new GUIContent("Density (of cast corridor visualizers)", "Not available in 'save drawn lines' mode."));
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("castCorridorVisualizerDensity"), new GUIContent("Density (of cast corridor visualizers)", "You can change the initial value of this via 'DrawXXL.DrawPhysics2D.castCorridorVisualizerDensity'." + Environment.NewLine + Environment.NewLine + "If you reached a limit and no new corridor visualizers appear you can raise the maximum value via 'DrawXXL.DrawPhysics2D.maxCorridorVisualizersPerCastVisualization'."));
            }
        }

        public void DrawTextSpecs(SerializedProperty sP_saveDrawnLinesType, bool typeIsCast_notOverlap)
        {
            if (sP_saveDrawnLinesType.enumValueIndex == (int)PhysicsVisualizer.SaveDrawnLinesType.yes_displayWithLowDetails)
            {
                EditorGUI.BeginDisabledGroup(true);
                GUIStyle style_forFoldoutLine = new GUIStyle(EditorStyles.foldout);
                EditorGUILayout.Foldout(false, new GUIContent("Text", "Not available in 'save drawn lines' mode."), false, style_forFoldoutLine);
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                //-> this fakes the appearance of beeing inside the parents text specification foldout
                bool emptyLineAtEndIfOutfolded = false;
                DrawTextInputInclMarkupHelper(true, false, null, emptyLineAtEndIfOutfolded);

                if (serializedObject.FindProperty("textSection_isOutfolded").boolValue == true)
                {
                    EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                    if (typeIsCast_notOverlap)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("drawCastNameTag_atCastOrigin"), new GUIContent("Draw text tag at cast origin", "You can change the initial value of this via 'DrawXXL.DrawPhysics2D.drawCastNameTag_atCastOrigin'."));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("drawCastNameTag_atHitPositions"), new GUIContent("Draw text tag at hit positions", "You can change the initial value of this via 'DrawXXL.DrawPhysics2D.drawCastNameTag_atHitPositions'."));

                        GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

                        EditorGUILayout.PropertyField(serializedObject.FindProperty("colorForCastsHitText"), new GUIContent("Hit description color", "You can change the initial value of this via 'DrawXXL.DrawPhysics2D.colorForCastsHitText'."));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("scaleFactor_forCastHitTextSize"), new GUIContent("Text size of hit descriptions", "You can change the initial value of this via 'DrawXXL.DrawPhysics2D.scaleFactor_forCastHitTextSize'."));
                        DrawChooserFor_useCustomDirectionForHitResultText(sP_saveDrawnLinesType);
                    }
                    else
                    {
                        SerializedProperty sP_overlapResultTextSizeInterpretation = serializedObject.FindProperty("overlapResultTextSizeInterpretation");
                        EditorGUILayout.PropertyField(sP_overlapResultTextSizeInterpretation, new GUIContent("Text Size Reference Frame"));

                        switch (sP_overlapResultTextSizeInterpretation.enumValueIndex)
                        {
                            case (int)PhysicsVisualizer.OverlapResultTextSizeInterpretation.relativeToTheSizeOfTheOverlapingPhysicsShape:
                                break;
                            case (int)PhysicsVisualizer.OverlapResultTextSizeInterpretation.fixedWorldSpaceSize:
                                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("forcedConstantWorldspaceTextSize_forOverlapResultTexts"), new GUIContent("Text size"));
                                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                                break;
                            case (int)PhysicsVisualizer.OverlapResultTextSizeInterpretation.relativeToTheSceneViewWindowSize:
                                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts"), new GUIContent("Text size"));
                                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                                break;
                            case (int)PhysicsVisualizer.OverlapResultTextSizeInterpretation.relativeToTheGameViewWindowSize:
                                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts"), new GUIContent("Text size"));
                                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                                break;
                            default:
                                break;
                        }

                        SerializedProperty sP_maxListedColliders_inOverlapVolumesTextList = serializedObject.FindProperty("maxListedColliders_inOverlapVolumesTextList");
                        EditorGUILayout.PropertyField(sP_maxListedColliders_inOverlapVolumesTextList, new GUIContent("Maximum listed colliders in results text list", "The results list will be truncated if there are more results, along with a notification how many results are hidden." + Environment.NewLine + Environment.NewLine + "You can set the initial value of this via 'DrawPhysics2D.MaxListedColliders_inOverlapVolumesTextList'."));
                        sP_maxListedColliders_inOverlapVolumesTextList.intValue = Mathf.Max(1, sP_maxListedColliders_inOverlapVolumesTextList.intValue);

                        SerializedProperty sP_maxOverlapingCollidersWithUntruncatedText = serializedObject.FindProperty("maxOverlapingCollidersWithUntruncatedText");
                        if (sP_saveDrawnLinesType.enumValueIndex == (int)PhysicsVisualizer.SaveDrawnLinesType.no_useFullDetails)
                        {
                            EditorGUILayout.PropertyField(sP_maxOverlapingCollidersWithUntruncatedText, new GUIContent("Maximum collision markers with untruncated text", "If more collisions than this number are found then the description text at the collision position marker will be truncated to only a sequential number." + Environment.NewLine + Environment.NewLine + "You can set the initial value of this via 'DrawPhysics2D.maxOverlapingCollidersWithUntruncatedText'."));
                        }
                        else
                        {
                            EditorGUI.BeginDisabledGroup(true);
                            EditorGUILayout.PropertyField(sP_maxOverlapingCollidersWithUntruncatedText, new GUIContent("Maximum collision markers with untruncated text", "Only available if 'save drawn lines' is disabled."));
                            EditorGUI.EndDisabledGroup();
                        }
                        sP_maxOverlapingCollidersWithUntruncatedText.intValue = Mathf.Max(0, sP_maxOverlapingCollidersWithUntruncatedText.intValue);

                    }

                    GUILayout.Space(EditorGUIUtility.singleLineHeight);

                    EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                }
            }
        }

        void DrawChooserFor_useCustomDirectionForHitResultText(SerializedProperty sP_saveDrawnLinesType)
        {
            if (sP_saveDrawnLinesType.enumValueIndex == (int)PhysicsVisualizer.SaveDrawnLinesType.no_useFullDetails)
            {
                SerializedProperty sP_useCustomDirectionForHitResultText = serializedObject.FindProperty("useCustomDirectionForHitResultText");
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PropertyField(sP_useCustomDirectionForHitResultText, new GUIContent("Custom placement of hit description block", "You can change the initial value of this via 'DrawPhysics2D.directionOfHitResultText'."));

                EditorGUI.BeginDisabledGroup(!sP_useCustomDirectionForHitResultText.boolValue);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("customDirectionForHitResultText"), GUIContent.none);
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);

                SerializedProperty sP_useCustomDirectionForHitResultText = serializedObject.FindProperty("useCustomDirectionForHitResultText");
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.PropertyField(sP_useCustomDirectionForHitResultText, new GUIContent("Custom placement of hit description block", "Only available if 'save drawn lines' is disabled."));

                EditorGUI.BeginDisabledGroup(!sP_useCustomDirectionForHitResultText.boolValue);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("customDirectionForHitResultText"), GUIContent.none);
                EditorGUI.EndDisabledGroup();

                EditorGUILayout.EndHorizontal();

                EditorGUI.EndDisabledGroup();
            }
        }

    }
#endif
}
