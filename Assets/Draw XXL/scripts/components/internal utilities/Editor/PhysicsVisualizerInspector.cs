namespace DrawXXL
{
    using UnityEngine;
    using System;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(PhysicsVisualizer))]
    [CanEditMultipleObjects]
    public class PhysicsVisualizerInspector : VisualizerParentInspector
    {
        PhysicsVisualizer physicsVisualizer_unserializedMonoB;

        void OnEnable()
        {
            OnEnable_base();
            physicsVisualizer_unserializedMonoB = (PhysicsVisualizer)target;
        }

        public void OnSceneGUI()
        {
            if (physicsVisualizer_unserializedMonoB.showTransformIndependentRotationHandle)
            {
                Vector3 positionOfPhysicsShape = physicsVisualizer_unserializedMonoB.GetDrawPos3D_global();
                float sizeOfRotationHandle = HandleUtility.GetHandleSize(positionOfPhysicsShape);
                Vector3 shiftOffsetDirectionFromTransformHandle_normalized = Vector3.up;
                if (SceneView.lastActiveSceneView != null)
                {
                    shiftOffsetDirectionFromTransformHandle_normalized = SceneView.lastActiveSceneView.camera.transform.up;
                }
                Vector3 postionOfAdditionalRotationHandle = positionOfPhysicsShape + shiftOffsetDirectionFromTransformHandle_normalized * 2.5f * sizeOfRotationHandle;
                Quaternion returnedRotationFromHandle;

                switch (physicsVisualizer_unserializedMonoB.shapeOrientationType)
                {
                    case PhysicsVisualizer.ShapeOrientationType.transformsRotationPlusOptionalAdditionalLocalRotation:
                        Quaternion optionalAdditionalRotation_local = physicsVisualizer_unserializedMonoB.transform.rotation * Quaternion.Euler(physicsVisualizer_unserializedMonoB.optionalAdditionalRotation_asEulersInV3);
                        Quaternion returnedRotationFromHandle_local = Handles.RotationHandle(optionalAdditionalRotation_local, postionOfAdditionalRotationHandle);
                        Quaternion returnedRotationFromHandle_global = Quaternion.Inverse(physicsVisualizer_unserializedMonoB.transform.rotation) * returnedRotationFromHandle_local;
                        physicsVisualizer_unserializedMonoB.optionalAdditionalRotation_asEulersInV3 = returnedRotationFromHandle_global.eulerAngles;
                        break;
                    case PhysicsVisualizer.ShapeOrientationType.transformsRotationPlusOptionalAdditionalGlobalRotation:
                        returnedRotationFromHandle = Handles.RotationHandle(Quaternion.Euler(physicsVisualizer_unserializedMonoB.optionalAdditionalRotation_asEulersInV3), postionOfAdditionalRotationHandle);
                        physicsVisualizer_unserializedMonoB.optionalAdditionalRotation_asEulersInV3 = returnedRotationFromHandle.eulerAngles;
                        break;
                    case PhysicsVisualizer.ShapeOrientationType.customRotationIndependentFromTransform:
                        returnedRotationFromHandle = Handles.RotationHandle(Quaternion.Euler(physicsVisualizer_unserializedMonoB.customRotation_asEulersInV3), postionOfAdditionalRotationHandle);
                        physicsVisualizer_unserializedMonoB.customRotation_asEulersInV3 = returnedRotationFromHandle.eulerAngles;
                        break;
                    default:
                        break;
                }

                Handles.Label(postionOfAdditionalRotationHandle, "<color=white>Handle for Physics Shape</color>", new GUIStyle());
            }
        }

        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("physics");
            SerializedProperty sP_saveDrawnLinesType = serializedObject.FindProperty("saveDrawnLinesType");
            EditorGUILayout.PropertyField(sP_saveDrawnLinesType, new GUIContent("Save drawn lines ?", "This simplifies the visualization. It may be helpful to save performance if you draw many casts at once." + Environment.NewLine + Environment.NewLine + "You can change the initial value of this via 'DrawXXL.DrawPhysics.visualizationQuality'."));
            GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

            bool typeIsCast_notCheck = false;
            SerializedProperty sP_theGameobjectHasACompatibleAndEnabledCollider = serializedObject.FindProperty("theGameobjectHasACompatibleAndEnabledCollider");
            SerializedProperty sP_shape = serializedObject.FindProperty("shape");

            TryDrawCollisionTypeChooser(sP_shape, sP_theGameobjectHasACompatibleAndEnabledCollider, ref typeIsCast_notCheck);

            if ((sP_shape.enumValueIndex == (int)PhysicsVisualizer.Shape.collidersOnThisGameobject) && (sP_theGameobjectHasACompatibleAndEnabledCollider.boolValue == false))
            {
                EditorGUILayout.HelpBox("There is no enabled collider on this gameobject that could be cast. Supported colliders are 'BoxCollider', 'SphereCollider' and 'CapsuleCollider'.", MessageType.None, true);
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("wantedHits"), new GUIContent("Wanted hits"));
                DrawDetectedHitsCount();

                GUILayout.Space(EditorGUIUtility.singleLineHeight);

                DrawShapeSizeSpecification(sP_shape);
                DrawCastDirectionAndDistance(typeIsCast_notCheck);
                DrawOtherSettings(sP_saveDrawnLinesType, typeIsCast_notCheck);
                DrawTextSpecs(sP_saveDrawnLinesType, typeIsCast_notCheck);
                DrawCheckboxFor_drawOnlyIfSelected("physics");
                DrawCheckboxFor_hiddenByNearerObjects("physics");
            }

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }

        void TryDrawCollisionTypeChooser(SerializedProperty sP_shape, SerializedProperty sP_theGameobjectHasACompatibleAndEnabledCollider, ref bool typeIsCast_notCheck)
        {
            SerializedProperty sP_collisionType = serializedObject.FindProperty("collisionType");

            bool forceDisplayToCast = false;
            switch ((PhysicsVisualizer.Shape)sP_shape.enumValueIndex)
            {
                case PhysicsVisualizer.Shape.collidersOnThisGameobject:
                    if (sP_theGameobjectHasACompatibleAndEnabledCollider.boolValue)
                    {
                        forceDisplayToCast = false;
                        typeIsCast_notCheck = Get_typeIsCast_notCheck(sP_collisionType, forceDisplayToCast);
                    }
                    break;
                case PhysicsVisualizer.Shape.box:
                    forceDisplayToCast = false;
                    typeIsCast_notCheck = Get_typeIsCast_notCheck(sP_collisionType, forceDisplayToCast);
                    break;
                case PhysicsVisualizer.Shape.sphere:
                    forceDisplayToCast = false;
                    typeIsCast_notCheck = Get_typeIsCast_notCheck(sP_collisionType, forceDisplayToCast);
                    break;
                case PhysicsVisualizer.Shape.capsule:
                    forceDisplayToCast = false;
                    typeIsCast_notCheck = Get_typeIsCast_notCheck(sP_collisionType, forceDisplayToCast);
                    break;
                case PhysicsVisualizer.Shape.ray:
                    forceDisplayToCast = true;
                    typeIsCast_notCheck = Get_typeIsCast_notCheck(sP_collisionType, forceDisplayToCast);
                    break;
                default:
                    break;
            }

            string label_ofShape = typeIsCast_notCheck ? "Shape to cast" : "Shape to overlap";
            EditorGUILayout.PropertyField(sP_shape, new GUIContent(label_ofShape));

            switch ((PhysicsVisualizer.Shape)sP_shape.enumValueIndex)
            {
                case PhysicsVisualizer.Shape.collidersOnThisGameobject:
                    if (sP_theGameobjectHasACompatibleAndEnabledCollider.boolValue)
                    {
                        if (UtilitiesDXXL_EngineBasics.CheckIf_transformOrAParentHasNonUniformScale(transform_onVisualizerObject.parent))
                        {
                            EditorGUILayout.HelpBox("A parent transform has a non-uniform scale -> The collider and cast positions may be wrong.", MessageType.Warning, true);
                        }
                        DrawCollisionTypeChooser(sP_collisionType, forceDisplayToCast);
                    }
                    break;
                case PhysicsVisualizer.Shape.box:
                    DrawCollisionTypeChooser(sP_collisionType, forceDisplayToCast);
                    break;
                case PhysicsVisualizer.Shape.sphere:
                    DrawCollisionTypeChooser(sP_collisionType, forceDisplayToCast);
                    break;
                case PhysicsVisualizer.Shape.capsule:
                    DrawCollisionTypeChooser(sP_collisionType, forceDisplayToCast);
                    break;
                case PhysicsVisualizer.Shape.ray:
                    DrawCollisionTypeChooser(sP_collisionType, forceDisplayToCast);
                    break;
                default:
                    break;
            }
        }

        bool Get_typeIsCast_notCheck(SerializedProperty sP_collisionType, bool forceDisplayToCast)
        {
            if (forceDisplayToCast)
            {
                return true;
            }
            else
            {
                return (sP_collisionType.enumValueIndex == (int)PhysicsVisualizer.CollisionType.cast);
            }
        }

        void DrawCollisionTypeChooser(SerializedProperty sP_collisionType, bool forceDisplayToCast)
        {
            string label = "Collision type";
            if (forceDisplayToCast)
            {
                PhysicsVisualizer.CollisionType castCollisionType = PhysicsVisualizer.CollisionType.cast;

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
            if ((sP_shape.enumValueIndex == (int)PhysicsVisualizer.Shape.box) || (sP_shape.enumValueIndex == (int)PhysicsVisualizer.Shape.sphere) || (sP_shape.enumValueIndex == (int)PhysicsVisualizer.Shape.capsule))
            {
                GUIStyle richtextEnabledStyle = new GUIStyle(EditorStyles.label);
                richtextEnabledStyle.richText = true;

                switch ((PhysicsVisualizer.Shape)sP_shape.enumValueIndex)
                {
                    case PhysicsVisualizer.Shape.collidersOnThisGameobject:
                        break;
                    case PhysicsVisualizer.Shape.box:
                        EditorGUILayout.LabelField("<b>Box:</b>    --- Size / Orientation / Position ---", richtextEnabledStyle);
                        break;
                    case PhysicsVisualizer.Shape.sphere:
                        EditorGUILayout.LabelField("<b>Sphere:</b>    --- Size / Position ---", richtextEnabledStyle);
                        break;
                    case PhysicsVisualizer.Shape.capsule:
                        EditorGUILayout.LabelField("<b>Capsule:</b>    --- Size / Orientation / Position ---", richtextEnabledStyle);
                        break;
                    case PhysicsVisualizer.Shape.ray:
                        break;
                    default:
                        break;
                }

                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                switch ((PhysicsVisualizer.Shape)sP_shape.enumValueIndex)
                {
                    case PhysicsVisualizer.Shape.collidersOnThisGameobject:
                        break;
                    case PhysicsVisualizer.Shape.box:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("sizeScaleFactors_ofCastRespCheckedBox"), new GUIContent("Size"));
                        DrawShapeOrientation();
                        Draw_DrawPosition3DOffset(true, "Position Offset");
                        break;
                    case PhysicsVisualizer.Shape.sphere:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("radiusScaleFactor_ofCastRespCheckedShape"), new GUIContent("Radius"));
                        Draw_DrawPosition3DOffset(true, "Position Offset");
                        break;
                    case PhysicsVisualizer.Shape.capsule:
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("radiusScaleFactor_ofCastRespCheckedShape"), new GUIContent("Radius"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("heightScaleFactor_ofCastRespCheckedCapsule"), new GUIContent("Height"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("capsuleAlignment"), new GUIContent("Height alignment"));
                        DrawShapeOrientation();
                        Draw_DrawPosition3DOffset(true, "Position Offset");
                        break;
                    case PhysicsVisualizer.Shape.ray:
                        break;
                    default:
                        break;
                }
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }
            else
            {
                if (sP_shape.enumValueIndex == (int)PhysicsVisualizer.Shape.ray)
                {
                    Draw_DrawPosition3DOffset(false, "Position Offset");
                }
            }
        }

        void DrawShapeOrientation()
        {
            SerializedProperty sP_shapeOrientationType = serializedObject.FindProperty("shapeOrientationType");
            EditorGUILayout.PropertyField(sP_shapeOrientationType, new GUIContent("Orientation"));

            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            switch (sP_shapeOrientationType.enumValueIndex)
            {
                case (int)PhysicsVisualizer.ShapeOrientationType.transformsRotationPlusOptionalAdditionalLocalRotation:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("optionalAdditionalRotation_asEulersInV3"), new GUIContent("Additional rotation (local)", "This is added to the rotation from the transform component. The angle values here rotate around the local axes of the transform, which may already be skewed and define a local space."));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("showTransformIndependentRotationHandle"), new GUIContent("Show additional rotation handle", "This activates an additional rotation handle which is independent from the transforms rotation handle and only affects the additional rotation defined in the above line."));
                    break;
                case (int)PhysicsVisualizer.ShapeOrientationType.transformsRotationPlusOptionalAdditionalGlobalRotation:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("optionalAdditionalRotation_asEulersInV3"), new GUIContent("Additional rotation (global)", "This is added to the rotation from the transform component. The angle values here rotate around the global axes, independent of how the transform is already skewed in space."));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("showTransformIndependentRotationHandle"), new GUIContent("Show additional rotation handle", "This activates an additional rotation handle which is independent from the transforms rotation handle and only affects the additional rotation defined in the above line."));
                    break;
                case (int)PhysicsVisualizer.ShapeOrientationType.customRotationIndependentFromTransform:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("customRotation_asEulersInV3"), new GUIContent("Custom rotation"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("showTransformIndependentRotationHandle"), new GUIContent("Show additional rotation handle", "This activates an additional rotation handle which is independent from the transforms rotation handle and only affects the custom rotation defined in the above line."));
                    break;
                default:
                    break;
            }

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

        void DrawCastDirectionAndDistance(bool typeIsCast_notCheck)
        {
            if (typeIsCast_notCheck)
            {
                DrawSpecificationOf_customVector3_1("Cast direction", false, null, true, false, true, false);

                EditorGUILayout.LabelField("Cast distance");
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                SerializedProperty sP_distanceIsInfinityRespToOtherGO = serializedObject.FindProperty("distanceIsInfinityRespToOtherGO");

                if (visualizerParentMonoBehaviour_unserialized.source_ofCustomVector3_1 == VisualizerParent.CustomVector3Source.toOtherGameobject)
                {
                    EditorGUILayout.PropertyField(sP_distanceIsInfinityRespToOtherGO, new GUIContent("Till other gameobject"));
                }
                else
                {
                    EditorGUILayout.PropertyField(sP_distanceIsInfinityRespToOtherGO, new GUIContent("Infinity"));
                }

                EditorGUI.BeginDisabledGroup(sP_distanceIsInfinityRespToOtherGO.boolValue);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("adjustedDistance"), new GUIContent("Flexible"));
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }
        }

        void DrawOtherSettings(SerializedProperty sP_saveDrawnLinesType, bool typeIsCast_notCheck)
        {
            SerializedProperty sP_otherSettings_isFoldedOut = serializedObject.FindProperty("otherSettings_isFoldedOut");
            sP_otherSettings_isFoldedOut.boolValue = EditorGUILayout.Foldout(sP_otherSettings_isFoldedOut.boolValue, "Other settings", true);
            if (sP_otherSettings_isFoldedOut.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                EditorGUILayout.PropertyField(serializedObject.FindProperty("excludeCollidersOnThisGO"), new GUIContent("Exclude colliders on this Gameobject from result", "Caution for disabling this:" + Environment.NewLine + "In many cases the colliders on this Gameobject will overlap with the starting position of the cast shapes. In these cases a collision will be detected, but the position and hit distance may be wrong."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("excludeCollidersOnParentGOs"), new GUIContent("Exclude colliders on parents from result", "Caution for disabling this:" + Environment.NewLine + "In many cases the colliders on the parents will overlap with the starting position of the cast shapes. In these cases a collision will be detected, but the position and hit distance may be wrong."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("excludeCollidersOnChildrenGOs"), new GUIContent("Exclude colliders on children from result", "Caution for disabling this:" + Environment.NewLine + "In many cases the colliders on the children will overlap with the starting position of the cast shapes. In these cases a collision will be detected, but the position and hit distance may be wrong."));

                EditorGUILayout.PropertyField(serializedObject.FindProperty("layerMask"), new GUIContent("Colliding layers", "The collision check considers only colliders that are part of the herewith selected layers." + Environment.NewLine + Environment.NewLine + "For more details see Unitys documentation of the 'layerMask' parameter inside the 'UnityEngine.Physics.*' functions documentation."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("queryTriggerInteraction"), new GUIContent("Trigger Interaction Config", "This defines whether the collision checks interact with colliders that are configurated as triggers." + Environment.NewLine + "'Use global' means that the global setting is used, which is defined by 'UnityEngine.Physics.queriesHitTriggers'." + Environment.NewLine + Environment.NewLine + "For more details see Unitys documentation of 'QueryTriggerInteraction'."));

                string displayNameOfColorForNonHittingChecks;
                string displayNameOfColorForHittingChecks;
                if (typeIsCast_notCheck)
                {
                    displayNameOfColorForNonHittingChecks = "Non hitting rays";
                    displayNameOfColorForHittingChecks = "Hitting rays";
                }
                else
                {
                    displayNameOfColorForNonHittingChecks = "Not Overlapping";
                    displayNameOfColorForHittingChecks = "Overlapping";
                }

                EditorGUILayout.PropertyField(serializedObject.FindProperty("colorForNonHittingCasts"), new GUIContent(displayNameOfColorForNonHittingChecks, "You can change the initial value of this via 'DrawPhysics.colorForNonHittingCasts'."));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("colorForHittingCasts"), new GUIContent(displayNameOfColorForHittingChecks, "You can change the initial value of this via 'DrawPhysics.colorForHittingCasts'."));

                if (typeIsCast_notCheck)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("colorForCastLineBeyondHit"), new GUIContent("After last hit", "You can change the initial value of this via 'DrawPhysics.colorForCastLineBeyondHit'."));
                    DrawChooserLineFor_overwriteColorForCastsHitNormals();
                    Draw_castSilhouetteVisualizerDensity(sP_saveDrawnLinesType);
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

        void Draw_castSilhouetteVisualizerDensity(SerializedProperty sP_saveDrawnLinesType)
        {
            if (sP_saveDrawnLinesType.enumValueIndex == (int)PhysicsVisualizer.SaveDrawnLinesType.yes_displayWithLowDetails)
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("castSilhouetteVisualizerDensity"), new GUIContent("Density (of cast silhouette visualizers)", "Not available in 'save drawn lines' mode."));
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("castSilhouetteVisualizerDensity"), new GUIContent("Density (of cast silhouette visualizers)", "You can change the initial value of this via 'DrawXXL.DrawPhysics.castSilhouetteVisualizerDensity'." + Environment.NewLine + Environment.NewLine + "If you reached a limit and no new silhouettes appear you can raise the maximum value via 'DrawXXL.DrawPhysics.maxSilhouettesPerCastVisualization'."));
            }
        }

        public void DrawTextSpecs(SerializedProperty sP_saveDrawnLinesType, bool typeIsCast_notCheck)
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

                    if (typeIsCast_notCheck)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("drawCastNameTag_atCastOrigin"), new GUIContent("Draw text tag at cast origin", "You can change the initial value of this via 'DrawPhysics.drawCastNameTag_atCastOrigin'."));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("drawCastNameTag_atHitPositions"), new GUIContent("Draw text tag at hit positions", "You can change the initial value of this via 'DrawPhysics.drawCastNameTag_atHitPositions'."));

                        GUILayout.Space(0.5f * EditorGUIUtility.singleLineHeight);

                        EditorGUILayout.PropertyField(serializedObject.FindProperty("colorForCastsHitText"), new GUIContent("Hit description color", "You can change the initial value of this via 'DrawPhysics.colorForCastsHitText'."));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("scaleFactor_forCastHitTextSize"), new GUIContent("Text size of hit descriptions", "You can change the initial value of this via 'DrawPhysics.scaleFactor_forCastHitTextSize'."));
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
                        EditorGUILayout.PropertyField(sP_maxListedColliders_inOverlapVolumesTextList, new GUIContent("Maximum listed colliders in results text list", "The results list will be truncated if there are more results, along with a notification how many results are hidden." + Environment.NewLine + Environment.NewLine + "You can set the initial value of this via 'DrawPhysics.MaxListedColliders_inOverlapVolumesTextList'."));
                        sP_maxListedColliders_inOverlapVolumesTextList.intValue = Mathf.Max(1, sP_maxListedColliders_inOverlapVolumesTextList.intValue);

                        SerializedProperty sP_maxOverlapingCollidersWithUntruncatedText = serializedObject.FindProperty("maxOverlapingCollidersWithUntruncatedText");
                        if (sP_saveDrawnLinesType.enumValueIndex == (int)PhysicsVisualizer.SaveDrawnLinesType.no_useFullDetails)
                        {
                            EditorGUILayout.PropertyField(sP_maxOverlapingCollidersWithUntruncatedText, new GUIContent("Maximum collision markers with untruncated text", "If more collisions than this number are found then the description text at the collision position marker will be truncated to only a sequential number." + Environment.NewLine + Environment.NewLine + "You can set the initial value of this via 'DrawPhysics.maxOverlapingCollidersWithUntruncatedText'."));
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

                EditorGUILayout.PropertyField(sP_useCustomDirectionForHitResultText, new GUIContent("Custom placement of hit description block", "You can change the initial value of this via 'DrawPhysics.directionOfHitResultText'."));

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
