namespace DrawXXL
{
    using UnityEngine;
    using System;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(ShapeDrawer))]
    [CanEditMultipleObjects]
    public class ShapeDrawerInspector : VisualizerParentInspector
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();

            DrawConsumedLines("shape");

            SerializedProperty sP_shapeCategory = serializedObject.FindProperty("shapeCategory");
            SerializedProperty sP_shapeType_3D = serializedObject.FindProperty("shapeType_3D");
            SerializedProperty sP_shapeType_flat = serializedObject.FindProperty("shapeType_flat");
            SerializedProperty sP_pyramidDefinitionVariant = serializedObject.FindProperty("pyramidDefinitionVariant");
            SerializedProperty sP_frustumDefinitionVariant = serializedObject.FindProperty("frustumDefinitionVariant");

            EditorGUILayout.PropertyField(sP_shapeCategory, new GUIContent("Shape category"));
            DrawShapeTypeChoosers(sP_shapeCategory, sP_shapeType_3D, sP_shapeType_flat, sP_pyramidDefinitionVariant, sP_frustumDefinitionVariant);

            bool isIcon = ((sP_shapeCategory.enumValueIndex == (int)ShapeDrawer.ShapeCategory.flat) && (sP_shapeType_flat.enumValueIndex == (int)ShapeDrawer.ShapeType_flat.icon));
            bool isDot = ((sP_shapeCategory.enumValueIndex == (int)ShapeDrawer.ShapeCategory.flat) && (sP_shapeType_flat.enumValueIndex == (int)ShapeDrawer.ShapeType_flat.dot));
            bool isPlane = ((sP_shapeCategory.enumValueIndex == (int)ShapeDrawer.ShapeCategory.flat) && (sP_shapeType_flat.enumValueIndex == (int)ShapeDrawer.ShapeType_flat.plane));
            bool isRhombus = ((sP_shapeCategory.enumValueIndex == (int)ShapeDrawer.ShapeCategory.flat) && (sP_shapeType_flat.enumValueIndex == (int)ShapeDrawer.ShapeType_flat.rhombus));

            DrawPositionDefinition(sP_shapeCategory, sP_shapeType_3D, sP_frustumDefinitionVariant);
            DrawOrientationDefinition(sP_shapeCategory, isIcon, isDot, isPlane, isRhombus);

            SerializedProperty sP_sizeDefinition = serializedObject.FindProperty("sizeDefinition");
            Draw_sizeInterpretationChooser(sP_sizeDefinition, isRhombus);

            bool displayFillstyleOption = false;
            DrawShapeSpecificOptions(sP_shapeCategory, sP_shapeType_3D, sP_pyramidDefinitionVariant, sP_frustumDefinitionVariant, sP_shapeType_flat, ref displayFillstyleOption, sP_sizeDefinition);
            DrawGeneralOptions(displayFillstyleOption, isIcon, isDot, isPlane, isRhombus, sP_sizeDefinition);

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }

        void DrawShapeTypeChoosers(SerializedProperty sP_shapeCategory, SerializedProperty sP_shapeType_3D, SerializedProperty sP_shapeType_flat, SerializedProperty sP_pyramidDefinitionVariant, SerializedProperty sP_frustumDefinitionVariant)
        {
            switch (sP_shapeCategory.enumValueIndex)
            {
                case (int)ShapeDrawer.ShapeCategory._3D:
                    EditorGUILayout.PropertyField(sP_shapeType_3D, new GUIContent("Type of 3D shape"));
                    break;
                case (int)ShapeDrawer.ShapeCategory.flat:
                    EditorGUILayout.PropertyField(sP_shapeType_flat, new GUIContent("Type of flat shape"));
                    break;
                default:
                    break;
            }

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            if (sP_shapeCategory.enumValueIndex == (int)ShapeDrawer.ShapeCategory._3D)
            {
                if (sP_shapeType_3D.enumValueIndex == (int)ShapeDrawer.ShapeType_3D.pyramid)
                {
                    EditorGUILayout.PropertyField(sP_pyramidDefinitionVariant, new GUIContent("Pyramid definition variant"));
                    GUILayout.Space(EditorGUIUtility.singleLineHeight);
                }

                if (sP_shapeType_3D.enumValueIndex == (int)ShapeDrawer.ShapeType_3D.cone)
                {
                    EditorGUILayout.PropertyField(sP_pyramidDefinitionVariant, new GUIContent("Cone definition variant"));
                    GUILayout.Space(EditorGUIUtility.singleLineHeight);
                }

                if (sP_shapeType_3D.enumValueIndex == (int)ShapeDrawer.ShapeType_3D.frustum)
                {
                    EditorGUILayout.PropertyField(sP_frustumDefinitionVariant, new GUIContent("Frustum definition variant"));
                    GUILayout.Space(EditorGUIUtility.singleLineHeight);
                }
            }

            if (sP_shapeCategory.enumValueIndex == (int)ShapeDrawer.ShapeCategory.flat)
            {
                if (sP_shapeType_flat.enumValueIndex == (int)ShapeDrawer.ShapeType_flat.icon)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("iconType"), new GUIContent("Icon type"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("showAtlasOfAllAvailableIcons"), new GUIContent("Show atlas of all available icons"));
                    GUILayout.Space(EditorGUIUtility.singleLineHeight);
                }
            }

        }

        void DrawPositionDefinition(SerializedProperty sP_shapeCategory, SerializedProperty sP_shapeType_3D, SerializedProperty sP_frustumDefinitionVariant)
        {
            GUIStyle style_ofHeadline = new GUIStyle();
            style_ofHeadline.fontStyle = FontStyle.Bold;
            EditorGUILayout.LabelField(serializedObject.FindProperty("labelOfPosition").stringValue, style_ofHeadline);
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            Draw_DrawPosition3DOffset(true, "Offset from transform position");
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

            GUILayout.Space(EditorGUIUtility.singleLineHeight);

            if ((sP_shapeCategory.enumValueIndex == (int)ShapeDrawer.ShapeCategory._3D) && (sP_shapeType_3D.enumValueIndex == (int)ShapeDrawer.ShapeType_3D.frustum) && (sP_frustumDefinitionVariant.enumValueIndex == (int)ShapeDrawer.FrustumDefinitionVariant.centersOfBigAndSmallClipPlanes))
            {
                //special case: one frustum variant uses a second position:
                EditorGUILayout.LabelField("Position of center of small clip plane", style_ofHeadline);
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
                EditorGUILayout.PropertyField(serializedObject.FindProperty("partnerGameobject"), new GUIContent("Position"));
                EditorGUI.BeginDisabledGroup(visualizerParentMonoBehaviour_unserialized.partnerGameobject == null);
                Draw_DrawPosition3DOffset_ofPartnerGameobject(true, "Offset from other transform position");
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }
        }

        void DrawOrientationDefinition(SerializedProperty sP_shapeCategory, bool isIcon, bool isDot, bool isPlane, bool isRhombus)
        {
            bool customVectorPickers_useGreyedOutVectors_3and4 = false;
            if ((sP_shapeCategory.enumValueIndex == (int)ShapeDrawer.ShapeCategory.flat) && (isRhombus == false))
            {
                SerializedProperty sP_force2DShapeTo_facingToSceneViewCam = serializedObject.FindProperty("force2DShapeTo_facingToSceneViewCam");
                SerializedProperty sP_force2DShapeTo_facingToGameViewCam = serializedObject.FindProperty("force2DShapeTo_facingToGameViewCam");

                bool forceToSceneViewCam_isGreyedOut = false;
                bool forceToGameViewCam_isGreyedOut = false;

                if (sP_force2DShapeTo_facingToGameViewCam.boolValue == true)
                {
                    sP_force2DShapeTo_facingToSceneViewCam.boolValue = false;
                    forceToSceneViewCam_isGreyedOut = true;
                }

                if (sP_force2DShapeTo_facingToSceneViewCam.boolValue == true)
                {
                    sP_force2DShapeTo_facingToGameViewCam.boolValue = false;
                    forceToGameViewCam_isGreyedOut = true;
                }

                EditorGUI.BeginDisabledGroup(forceToSceneViewCam_isGreyedOut);
                EditorGUILayout.PropertyField(sP_force2DShapeTo_facingToSceneViewCam, new GUIContent("Force facing to Scene view camera"));
                EditorGUI.EndDisabledGroup();

                EditorGUI.BeginDisabledGroup(forceToGameViewCam_isGreyedOut);
                EditorGUILayout.PropertyField(sP_force2DShapeTo_facingToGameViewCam, new GUIContent("Force facing to Game view camera"));
                EditorGUI.EndDisabledGroup();

                customVectorPickers_useGreyedOutVectors_3and4 = ((sP_force2DShapeTo_facingToGameViewCam.boolValue == true) || (sP_force2DShapeTo_facingToSceneViewCam.boolValue == true));

                if (sP_force2DShapeTo_facingToGameViewCam.boolValue == true)
                {
                    visualizerParentMonoBehaviour_unserialized.observerCamera_ofCustomVector3_3 = DrawBasics.CameraForAutomaticOrientation.gameViewCamera;
                    visualizerParentMonoBehaviour_unserialized.observerCamera_ofCustomVector3_4 = DrawBasics.CameraForAutomaticOrientation.gameViewCamera;
                }

                if (sP_force2DShapeTo_facingToSceneViewCam.boolValue == true)
                {
                    visualizerParentMonoBehaviour_unserialized.observerCamera_ofCustomVector3_3 = DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;
                    visualizerParentMonoBehaviour_unserialized.observerCamera_ofCustomVector3_4 = DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;
                }

                if (isPlane)
                {
                    visualizerParentMonoBehaviour_unserialized.source_ofCustomVector3_3 = VisualizerParent.CustomVector3Source.observerCameraUp;
                    visualizerParentMonoBehaviour_unserialized.source_ofCustomVector3_4 = VisualizerParent.CustomVector3Source.observerCameraForward;
                }
                else
                {
                    if (isIcon || isDot)
                    {
                        visualizerParentMonoBehaviour_unserialized.source_ofCustomVector3_3 = VisualizerParent.CustomVector3Source.observerCameraBack;
                        visualizerParentMonoBehaviour_unserialized.source_ofCustomVector3_4 = VisualizerParent.CustomVector3Source.observerCameraUp;
                    }
                    else
                    {
                        visualizerParentMonoBehaviour_unserialized.source_ofCustomVector3_3 = VisualizerParent.CustomVector3Source.observerCameraForward;
                        visualizerParentMonoBehaviour_unserialized.source_ofCustomVector3_4 = VisualizerParent.CustomVector3Source.observerCameraUp;
                    }
                }

                GUILayout.Space(EditorGUIUtility.singleLineHeight);
            }

            if (customVectorPickers_useGreyedOutVectors_3and4)
            {
                string explantionText_forGreyedOut = " (forced to face camera)";

                EditorGUI.BeginDisabledGroup(true);
                if (serializedObject.FindProperty("forwardVector_hasHigherPrioThan_upVector").boolValue)
                {
                    DrawSpecificationOf_customVector3_3(serializedObject.FindProperty("labelOfForwardVector").stringValue + explantionText_forGreyedOut, false, null, true, false, true, false);
                    if (isDot == false)
                    {
                        DrawSpecificationOf_customVector3_4(serializedObject.FindProperty("labelOfUpVector").stringValue + explantionText_forGreyedOut, false, null, true, false, true, true);
                    }
                }
                else
                {
                    DrawSpecificationOf_customVector3_4(serializedObject.FindProperty("labelOfUpVector").stringValue + explantionText_forGreyedOut, false, null, true, false, true, false);
                    DrawSpecificationOf_customVector3_3(serializedObject.FindProperty("labelOfForwardVector").stringValue + explantionText_forGreyedOut, false, null, true, false, true, true);
                }
                EditorGUI.EndDisabledGroup();
            }
            else
            {
                bool hideEverythingThatConcernsLength_atCustomVectorPicker = isRhombus ? false : true;
                if (serializedObject.FindProperty("forwardVector_hasHigherPrioThan_upVector").boolValue)
                {
                    DrawSpecificationOf_customVector3_1(serializedObject.FindProperty("labelOfForwardVector").stringValue, false, null, hideEverythingThatConcernsLength_atCustomVectorPicker, false, true, false);
                    if (isDot == false)
                    {
                        DrawSpecificationOf_customVector3_2(serializedObject.FindProperty("labelOfUpVector").stringValue, false, null, hideEverythingThatConcernsLength_atCustomVectorPicker, false, true, true);
                    }
                }
                else
                {
                    DrawSpecificationOf_customVector3_2(serializedObject.FindProperty("labelOfUpVector").stringValue, false, null, hideEverythingThatConcernsLength_atCustomVectorPicker, false, true, false);
                    DrawSpecificationOf_customVector3_1(serializedObject.FindProperty("labelOfForwardVector").stringValue, false, null, hideEverythingThatConcernsLength_atCustomVectorPicker, false, true, true);
                }
            }
        }

        void Draw_sizeInterpretationChooser(SerializedProperty sP_sizeDefinition, bool isRhombus)
        {
            EditorGUILayout.PropertyField(sP_sizeDefinition, new GUIContent("Size definition", "Some of the following parameters that define the size of the shape can be defined relative to a context of interest." + Environment.NewLine + "The values you specify in fields below like 'Size' or 'Lines width' will be interpreted according to the setting here."));

            SerializedProperty sP_cameraForSizeDefinitionIsAvailable = serializedObject.FindProperty("cameraForSizeDefinitionIsAvailable");
            switch ((ShapeDrawer.ShapeSizeDefinition)sP_sizeDefinition.enumValueIndex)
            {
                case ShapeDrawer.ShapeSizeDefinition.relativeToTheGlobalScaleOfTheTransformRespectivelyItsBiggestAbsoluteComponent:
                    break;
                case ShapeDrawer.ShapeSizeDefinition.absoluteUnits:
                    break;
                case ShapeDrawer.ShapeSizeDefinition.relativeToTheSceneViewWindowSize:
                    if (sP_cameraForSizeDefinitionIsAvailable.boolValue == false)
                    {
                        EditorGUILayout.HelpBox("Scene View Camera Window is not available", MessageType.Warning, true);
                    }
                    if (isRhombus) { DrawInfoBoxForRhombusInScreenspace("Scene View"); }
                    break;
                case ShapeDrawer.ShapeSizeDefinition.relativeToTheGameViewWindowSize:
                    if (sP_cameraForSizeDefinitionIsAvailable.boolValue == false)
                    {
                        EditorGUILayout.HelpBox("No Game View Camera found.", MessageType.Warning, true);
                    }
                    if (isRhombus) { DrawInfoBoxForRhombusInScreenspace("Game View"); }
                    break;
                default:
                    break;
            }
        }

        void DrawInfoBoxForRhombusInScreenspace(string typeOfScreen)
        {
            EditorGUILayout.HelpBox("The size of 'Rhombus' is defined by the two edge vectors and thus is not dependent on the screen window size. Though at least 'Lines width' and 'Line style scaling' are now tied to the " + typeOfScreen + " window size.", MessageType.Info, false);
        }

        void DrawShapeSpecificOptions(SerializedProperty sP_shapeCategory, SerializedProperty sP_shapeType_3D, SerializedProperty sP_pyramidDefinitionVariant, SerializedProperty sP_frustumDefinitionVariant, SerializedProperty sP_shapeType_flat, ref bool displayFillstyleOption, SerializedProperty sP_sizeDefinition)
        {

            switch (sP_shapeCategory.enumValueIndex)
            {
                case (int)ShapeDrawer.ShapeCategory._3D:
                    DrawShapeSpecificOptions_for3DShapes(sP_shapeType_3D, sP_pyramidDefinitionVariant, sP_frustumDefinitionVariant, sP_sizeDefinition);
                    break;
                case (int)ShapeDrawer.ShapeCategory.flat:
                    DrawShapeSpecificOptions_forFlatShapes(sP_shapeType_flat, ref displayFillstyleOption, sP_sizeDefinition);
                    break;
                default:
                    break;
            }
        }

        void DrawShapeSpecificOptions_for3DShapes(SerializedProperty sP_shapeType_3D, SerializedProperty sP_pyramidDefinitionVariant, SerializedProperty sP_frustumDefinitionVariant, SerializedProperty sP_sizeDefinition)
        {
            SerializedProperty sP_coneIsFilled = serializedObject.FindProperty("coneIsFilled");

            switch (sP_shapeType_3D.enumValueIndex)
            {
                case (int)ShapeDrawer.ShapeType_3D.cube:
                    Draw_sizeInterpretationDependentLine("scaleFactors_ofHullVolume", "Size", null, sP_sizeDefinition);

                    SerializedProperty sP_cubeIsFilled = serializedObject.FindProperty("cubeIsFilled");
                    EditorGUILayout.PropertyField(sP_cubeIsFilled, new GUIContent("Fill sides"));
                    if (serializedObject.FindProperty("cubeIsFilled").boolValue)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("colorOfSidePlanes"), new GUIContent("Color of cube sides"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("segmentsPerSide"), new GUIContent("Segments per cube side"));
                        Draw_sizeInterpretationDependentLine("linesWidthOfCubeFillLines", "Line width of side filling", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("useEdgesColorAsTextColor_ifAvailable"), new GUIContent("Text color from edges", "The text color can be chosen to be the same as the edge color or the same as side planes color."));
                    }
                    break;
                case (int)ShapeDrawer.ShapeType_3D.sphere:
                    Draw_sizeInterpretationDependentLine("radiusScaleFactor", "Radius", null, sP_sizeDefinition);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("struts"), new GUIContent("Struts"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("sphereQuality"), new GUIContent("Roundness Quality", "You can change the initial value of this for newly created components via 'DrawShapes.LinesPerSphereCircle'."));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("onlyUpperHalf"), new GUIContent("Only upper half shell"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("drawEquator"), new GUIContent("Draw equator"));
                    break;
                case (int)ShapeDrawer.ShapeType_3D.capsule:
                    Draw_sizeInterpretationDependentLine("radiusScaleFactor", "Radius", "In constrast to the behaviour of the size definition of CapsuleCollider all three dimensions are taken into account here, since the upward orientation can be freely chosen and doesn't have to fit a transform axis.", sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("heightOfCapsule3D_scaleFactor", "Height", "In constrast to the behaviour of the size definition of CapsuleCollider all three dimensions are taken into account here, since the upward orientation can be freely chosen and doesn't have to fit a transform axis.", sP_sizeDefinition);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("struts"), new GUIContent("Struts"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("sphereQuality"), new GUIContent("Roundness Quality", "You can change the initial value of this for newly created components via 'DrawShapes.LinesPerSphereCircle'."));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("onlyUpperHalf"), new GUIContent("Only upper half shell"));
                    break;
                case (int)ShapeDrawer.ShapeType_3D.cylinder:
                    Draw_sizeInterpretationDependentLine("heightScaleFactor", "Height", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("width_ofBase_scaleFactor", "Width of base", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("length_ofBase_scaleFactor", "Length of base", null, sP_sizeDefinition);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("baseShape_withInitialValueOf_circle4struts"), new GUIContent("Extruded shape"));
                    break;
                case (int)ShapeDrawer.ShapeType_3D.extrusion:
                    Draw_sizeInterpretationDependentLine("heightToUp_scaleFactor", "Upward extrusion height", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("heightToDown_scaleFactor", "Downward extrusion height", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("width_ofBase_scaleFactor", "Width of extruded cross section", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("length_ofBase_scaleFactor", "Length of extruded cross section", null, sP_sizeDefinition);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("baseShape_withInitialValueOf_square"), new GUIContent("Extruded shape"));
                    break;
                case (int)ShapeDrawer.ShapeType_3D.ellipsoid:
                    SerializedProperty sP_ellipsoidIsNonUniform = serializedObject.FindProperty("ellipsoidIsNonUniform");
                    EditorGUILayout.PropertyField(sP_ellipsoidIsNonUniform, new GUIContent("Non uniform half shells"));
                    if (sP_ellipsoidIsNonUniform.boolValue)
                    {
                        Draw_sizeInterpretationDependentLine("radiusUpScaleFactor_ellipsoid", "Upward radius", null, sP_sizeDefinition);
                        Draw_sizeInterpretationDependentLine("radiusDownScaleFactor_ellipsoid", "Downward radius", null, sP_sizeDefinition);
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("onlyUpperHalf"), new GUIContent("Only upper half shell"));
                        Draw_sizeInterpretationDependentLine("radiusUpScaleFactor_ellipsoid", "Upward radius", null, sP_sizeDefinition);
                    }
                    Draw_sizeInterpretationDependentLine("width_ofBase_scaleFactor", "X - equator radius", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("length_ofBase_scaleFactor", "Z - equator radius", null, sP_sizeDefinition);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("struts"), new GUIContent("Struts"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("sphereQuality"), new GUIContent("Roundness Quality", "You can change the initial value of this for newly created components via 'DrawShapes.LinesPerSphereCircle'."));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("drawEquator"), new GUIContent("Draw equator"));
                    break;
                case (int)ShapeDrawer.ShapeType_3D.pyramid:
                    switch (sP_pyramidDefinitionVariant.enumValueIndex)
                    {
                        case (int)ShapeDrawer.PyramidDefinitionVariant.fromCenterOfBasePlane:
                            Draw_sizeInterpretationDependentLine("heightScaleFactor", "Height", null, sP_sizeDefinition);
                            Draw_sizeInterpretationDependentLine("width_ofBase_scaleFactor", "Width of base", null, sP_sizeDefinition);
                            Draw_sizeInterpretationDependentLine("length_ofBase_scaleFactor", "Length of base", null, sP_sizeDefinition);
                            break;
                        case (int)ShapeDrawer.PyramidDefinitionVariant.fromApex:
                            Draw_sizeInterpretationDependentLine("heightScaleFactor", "Height", null, sP_sizeDefinition);
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("angleDegVert_initialValueOf90"), new GUIContent("Angle (vertical)"));
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("angleDegHoriz_initialValueOf90"), new GUIContent("Angle (horizontal)"));
                            break;
                        case (int)ShapeDrawer.PyramidDefinitionVariant.fromCenterOfHullVolume:
                            Draw_sizeInterpretationDependentLine("scaleFactors_ofHullVolume", "Size of hull", null, sP_sizeDefinition);
                            break;
                        default:
                            break;
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("baseShape_withInitialValueOf_square"), new GUIContent("Base shape"));
                    break;
                case (int)ShapeDrawer.ShapeType_3D.bipyramid:
                    Draw_sizeInterpretationDependentLine("heightToUp_scaleFactor", "Height of upper pyramid", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("heightToDown_scaleFactor", "Height of lower pyramid", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("width_ofBase_scaleFactor", "Width of base", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("length_ofBase_scaleFactor", "Length of base", null, sP_sizeDefinition);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("baseShape_withInitialValueOf_square"), new GUIContent("Base shape"));
                    break;
                case (int)ShapeDrawer.ShapeType_3D.cone:
                    EditorGUILayout.PropertyField(sP_coneIsFilled, new GUIContent("Dense filling"));
                    if (sP_coneIsFilled.boolValue)
                    {
                        switch (sP_pyramidDefinitionVariant.enumValueIndex)
                        {
                            case (int)ShapeDrawer.PyramidDefinitionVariant.fromCenterOfBasePlane:
                                Draw_sizeInterpretationDependentLine("heightScaleFactor", "Height", null, sP_sizeDefinition);
                                Draw_sizeInterpretationDependentLine("width_ofBase_scaleFactor", "Width of base circle", null, sP_sizeDefinition);
                                Draw_sizeInterpretationDependentLine("length_ofBase_scaleFactor", "Length of base circle", null, sP_sizeDefinition);
                                break;
                            case (int)ShapeDrawer.PyramidDefinitionVariant.fromApex:
                                Draw_sizeInterpretationDependentLine("heightScaleFactor", "Height", null, sP_sizeDefinition);
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("angleDegVert_initialValueOf90"), new GUIContent("Angle (vertical)"));
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("angleDegHoriz_initialValueOf90"), new GUIContent("Angle (horizontal)"));
                                break;
                            case (int)ShapeDrawer.PyramidDefinitionVariant.fromCenterOfHullVolume:
                                Draw_sizeInterpretationDependentLine("scaleFactors_ofHullVolume", "Size of hull", null, sP_sizeDefinition);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (sP_pyramidDefinitionVariant.enumValueIndex)
                        {
                            case (int)ShapeDrawer.PyramidDefinitionVariant.fromCenterOfBasePlane:
                                Draw_sizeInterpretationDependentLine("heightScaleFactor", "Height", null, sP_sizeDefinition);
                                Draw_sizeInterpretationDependentLine("width_ofBase_scaleFactor", "Width of base circle", null, sP_sizeDefinition);
                                Draw_sizeInterpretationDependentLine("length_ofBase_scaleFactor", "Length of base circle", null, sP_sizeDefinition);
                                break;
                            case (int)ShapeDrawer.PyramidDefinitionVariant.fromApex:
                                Draw_sizeInterpretationDependentLine("heightScaleFactor", "Height", null, sP_sizeDefinition);
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("angleDegVert_initialValueOf90"), new GUIContent("Angle (vertical)"));
                                EditorGUILayout.PropertyField(serializedObject.FindProperty("angleDegHoriz_initialValueOf90"), new GUIContent("Angle (horizontal)"));
                                break;
                            case (int)ShapeDrawer.PyramidDefinitionVariant.fromCenterOfHullVolume:
                                Draw_sizeInterpretationDependentLine("scaleFactors_ofHullVolume", "Size of hull", null, sP_sizeDefinition);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case (int)ShapeDrawer.ShapeType_3D.frustum:
                    switch (sP_frustumDefinitionVariant.enumValueIndex)
                    {
                        case (int)ShapeDrawer.FrustumDefinitionVariant.centerOfBigClipPlanePlusDistanceAndScaleFactorOfSmallPlane:
                            Draw_sizeInterpretationDependentLine("distanceBetweenClipPlanes_scaleFactor", "Distance between clip planes", null, sP_sizeDefinition);
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("scalingFactor_forSmallClipPlane"), new GUIContent("Small clip plane size (relative to big plane)"));
                            Draw_sizeInterpretationDependentLine("width_ofBigClipPlane_scaleFactor", "Big clip plane width", null, sP_sizeDefinition);
                            Draw_sizeInterpretationDependentLine("height_ofBigClipPlane_scaleFactor", "Big clip plane height", null, sP_sizeDefinition);
                            break;
                        case (int)ShapeDrawer.FrustumDefinitionVariant.centerOfBigClipPlanePlusDistancesToSmallPlaneAndApex:
                            Draw_sizeInterpretationDependentLine("distance_bigClipPlaneToApex_scaleFactor", "Distance from big clip plane to apex", null, sP_sizeDefinition);
                            Draw_sizeInterpretationDependentLine("distanceBetweenClipPlanes_scaleFactor", "Distance between clip planes", null, sP_sizeDefinition);
                            Draw_sizeInterpretationDependentLine("width_ofBigClipPlane_scaleFactor", "Big clip plane width", null, sP_sizeDefinition);
                            Draw_sizeInterpretationDependentLine("height_ofBigClipPlane_scaleFactor", "Big clip plane height", null, sP_sizeDefinition);
                            break;
                        case (int)ShapeDrawer.FrustumDefinitionVariant.centersOfBigAndSmallClipPlanes:
                            Draw_sizeInterpretationDependentLine("width_ofBigClipPlane_scaleFactor", "Big clip plane width", null, sP_sizeDefinition);
                            Draw_sizeInterpretationDependentLine("height_ofBigClipPlane_scaleFactor", "Big clip plane height", null, sP_sizeDefinition);
                            Draw_sizeInterpretationDependentLine("width_ofSmallClipPlane_scaleFactor", "Small clip plane width", null, sP_sizeDefinition);
                            Draw_sizeInterpretationDependentLine("height_ofSmallClipPlane_scaleFactor", "Small clip plane height", null, sP_sizeDefinition);
                            break;
                        case (int)ShapeDrawer.FrustumDefinitionVariant.fromApex:
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("angleDeg_initialValueOf60"), new GUIContent("Field of view (vertical angle)"));
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("aspectRatio"), new GUIContent("Aspect ratio"));
                            Draw_sizeInterpretationDependentLine("distanceApexToNearPlane_scaleFactor", "Near clip planes distance from apex", null, sP_sizeDefinition);
                            Draw_sizeInterpretationDependentLine("distance_bigClipPlaneToApex_scaleFactor", "Far clip planes distance from apex", null, sP_sizeDefinition);
                            break;
                        case (int)ShapeDrawer.FrustumDefinitionVariant.fromCenterOfHullVolume:
                            Draw_sizeInterpretationDependentLine("scaleFactors_ofHullVolume", "Size of hull", null, sP_sizeDefinition);
                            EditorGUILayout.PropertyField(serializedObject.FindProperty("scalingFactor_forSmallClipPlane"), new GUIContent("Small clip plane size (relative to big plane)"));
                            break;
                        default:
                            break;
                    }
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("baseShape_withInitialValueOf_square"), new GUIContent("Clip planes shape"));
                    break;
                default:
                    break;
            }
        }

        void DrawShapeSpecificOptions_forFlatShapes(SerializedProperty sP_shapeType_flat, ref bool displayFillstyleOption, SerializedProperty sP_sizeDefinition)
        {
            SerializedProperty sP_flatShapeIsNonUniform = serializedObject.FindProperty("flatShapeIsNonUniform");
            string flatShapeIsNonUniform_description = "Uneven aspect ratio";

            switch (sP_shapeType_flat.enumValueIndex)
            {
                case (int)ShapeDrawer.ShapeType_flat.circle:
                    displayFillstyleOption = true;
                    EditorGUILayout.PropertyField(sP_flatShapeIsNonUniform, new GUIContent(flatShapeIsNonUniform_description));
                    if (sP_flatShapeIsNonUniform.boolValue)
                    {
                        Draw_sizeInterpretationDependentLine("widthScaleFactor", "Width", null, sP_sizeDefinition);
                        Draw_sizeInterpretationDependentLine("heightScaleFactor", "Height", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("flattenRoundLines_intoShapePlane"), new GUIContent("Force round lines to flat", "This only applies if 'Lines width' is bigger than zero."));
                    }
                    else
                    {
                        Draw_sizeInterpretationDependentLine("radiusScaleFactor", "Radius", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("filledWithSpokes"), new GUIContent("Filled with spokes"));
                    }
                    break;
                case (int)ShapeDrawer.ShapeType_flat.ellipse:
                    displayFillstyleOption = true;
                    Draw_sizeInterpretationDependentLine("radiusSideward_ofEllipse_scaleFactor", "Radius towards side", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("radiusUpward_ofEllipse_scaleFactor", "Radius towards up", null, sP_sizeDefinition);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("filledWithSpokes"), new GUIContent("Filled with spokes"));
                    break;
                case (int)ShapeDrawer.ShapeType_flat.star:
                    EditorGUILayout.PropertyField(sP_flatShapeIsNonUniform, new GUIContent(flatShapeIsNonUniform_description));
                    if (sP_flatShapeIsNonUniform.boolValue)
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("cornerOptionsForIrregularStar"), new GUIContent("Corners"));
                        Draw_sizeInterpretationDependentLine("widthScaleFactor", "Width", null, sP_sizeDefinition);
                        Draw_sizeInterpretationDependentLine("heightScaleFactor", "Height", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("flattenRoundLines_intoShapePlane"), new GUIContent("Force round lines to flat", "This only applies if 'Lines width' is bigger than zero."));
                    }
                    else
                    {
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("corners"), new GUIContent("Corners"));
                        Draw_sizeInterpretationDependentLine("outerRadiusOfStars_scaleFactor", "Outer radius", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("innerRadiusFactor"), new GUIContent("Inner radius (relative to outer radius)"));
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("filledWithSpokes"), new GUIContent("Filled with spokes"));
                    }
                    break;
                case (int)ShapeDrawer.ShapeType_flat.capsule:
                    displayFillstyleOption = true;
                    Draw_sizeInterpretationDependentLine("widthOfCapsule2D_scaleFactor", "Width", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("heightOfCapsule2D_scaleFactor", "Height", null, sP_sizeDefinition);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("capusleDirection2D"), new GUIContent("Direction"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("filledWithSpokes"), new GUIContent("Filled with spokes"));
                    break;
                case (int)ShapeDrawer.ShapeType_flat.icon:
                    Draw_sizeInterpretationDependentLine("uniformSizeScaleFactor", "Size", null, sP_sizeDefinition);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("iconIsMirroredHorizontally"), new GUIContent("Mirror horizontally"));
                    break;
                case (int)ShapeDrawer.ShapeType_flat.triangle:
                    displayFillstyleOption = true;
                    EditorGUILayout.PropertyField(sP_flatShapeIsNonUniform, new GUIContent(flatShapeIsNonUniform_description));
                    if (sP_flatShapeIsNonUniform.boolValue)
                    {
                        Draw_sizeInterpretationDependentLine("widthScaleFactor", "Width", null, sP_sizeDefinition);
                        Draw_sizeInterpretationDependentLine("heightScaleFactor", "Height", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("flattenRoundLines_intoShapePlane"), new GUIContent("Force round lines to flat", "This only applies if 'Lines width' is bigger than zero."));
                    }
                    else
                    {
                        Draw_sizeInterpretationDependentLine("radiusScaleFactor", "Radius (of hull circle)", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("filledWithSpokes"), new GUIContent("Filled with spokes"));
                    }
                    break;
                case (int)ShapeDrawer.ShapeType_flat.square:
                    displayFillstyleOption = true;
                    EditorGUILayout.PropertyField(sP_flatShapeIsNonUniform, new GUIContent(flatShapeIsNonUniform_description));
                    if (sP_flatShapeIsNonUniform.boolValue)
                    {
                        Draw_sizeInterpretationDependentLine("widthScaleFactor", "Width", null, sP_sizeDefinition);
                        Draw_sizeInterpretationDependentLine("heightScaleFactor", "Height", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("flattenRoundLines_intoShapePlane"), new GUIContent("Force round lines to flat", "This only applies if 'Lines width' is bigger than zero."));
                    }
                    else
                    {
                        Draw_sizeInterpretationDependentLine("uniformSizeScaleFactor", "Size", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("filledWithSpokes"), new GUIContent("Filled with spokes"));
                    }
                    break;
                case (int)ShapeDrawer.ShapeType_flat.pentagon:
                    displayFillstyleOption = true;
                    EditorGUILayout.PropertyField(sP_flatShapeIsNonUniform, new GUIContent(flatShapeIsNonUniform_description));
                    if (sP_flatShapeIsNonUniform.boolValue)
                    {
                        Draw_sizeInterpretationDependentLine("widthScaleFactor", "Width", null, sP_sizeDefinition);
                        Draw_sizeInterpretationDependentLine("heightScaleFactor", "Height", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("flattenRoundLines_intoShapePlane"), new GUIContent("Force round lines to flat", "This only applies if 'Lines width' is bigger than zero."));
                    }
                    else
                    {
                        Draw_sizeInterpretationDependentLine("radiusScaleFactor", "Radius (of hull circle)", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("filledWithSpokes"), new GUIContent("Filled with spokes"));
                    }
                    break;
                case (int)ShapeDrawer.ShapeType_flat.hexagon:
                    displayFillstyleOption = true;
                    EditorGUILayout.PropertyField(sP_flatShapeIsNonUniform, new GUIContent(flatShapeIsNonUniform_description));
                    if (sP_flatShapeIsNonUniform.boolValue)
                    {
                        Draw_sizeInterpretationDependentLine("widthScaleFactor", "Width", null, sP_sizeDefinition);
                        Draw_sizeInterpretationDependentLine("heightScaleFactor", "Height", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("flattenRoundLines_intoShapePlane"), new GUIContent("Force round lines to flat", "This only applies if 'Lines width' is bigger than zero."));
                    }
                    else
                    {
                        Draw_sizeInterpretationDependentLine("radiusScaleFactor", "Radius (of hull circle)", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("filledWithSpokes"), new GUIContent("Filled with spokes"));
                    }
                    break;
                case (int)ShapeDrawer.ShapeType_flat.septagon:
                    displayFillstyleOption = true;
                    EditorGUILayout.PropertyField(sP_flatShapeIsNonUniform, new GUIContent(flatShapeIsNonUniform_description));
                    if (sP_flatShapeIsNonUniform.boolValue)
                    {
                        Draw_sizeInterpretationDependentLine("widthScaleFactor", "Width", null, sP_sizeDefinition);
                        Draw_sizeInterpretationDependentLine("heightScaleFactor", "Height", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("flattenRoundLines_intoShapePlane"), new GUIContent("Force round lines to flat", "This only applies if 'Lines width' is bigger than zero."));
                    }
                    else
                    {
                        Draw_sizeInterpretationDependentLine("radiusScaleFactor", "Radius (of hull circle)", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("filledWithSpokes"), new GUIContent("Filled with spokes"));
                    }
                    break;
                case (int)ShapeDrawer.ShapeType_flat.octagon:
                    displayFillstyleOption = true;
                    EditorGUILayout.PropertyField(sP_flatShapeIsNonUniform, new GUIContent(flatShapeIsNonUniform_description));
                    if (sP_flatShapeIsNonUniform.boolValue)
                    {
                        Draw_sizeInterpretationDependentLine("widthScaleFactor", "Width", null, sP_sizeDefinition);
                        Draw_sizeInterpretationDependentLine("heightScaleFactor", "Height", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("flattenRoundLines_intoShapePlane"), new GUIContent("Force round lines to flat", "This only applies if 'Lines width' is bigger than zero."));
                    }
                    else
                    {
                        Draw_sizeInterpretationDependentLine("radiusScaleFactor", "Radius (of hull circle)", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("filledWithSpokes"), new GUIContent("Filled with spokes"));
                    }
                    break;
                case (int)ShapeDrawer.ShapeType_flat.decagon:
                    displayFillstyleOption = true;
                    EditorGUILayout.PropertyField(sP_flatShapeIsNonUniform, new GUIContent(flatShapeIsNonUniform_description));
                    if (sP_flatShapeIsNonUniform.boolValue)
                    {
                        Draw_sizeInterpretationDependentLine("widthScaleFactor", "Width", null, sP_sizeDefinition);
                        Draw_sizeInterpretationDependentLine("heightScaleFactor", "Height", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("flattenRoundLines_intoShapePlane"), new GUIContent("Force round lines to flat", "This only applies if 'Lines width' is bigger than zero."));
                    }
                    else
                    {
                        Draw_sizeInterpretationDependentLine("radiusScaleFactor", "Radius (of hull circle)", null, sP_sizeDefinition);
                        EditorGUILayout.PropertyField(serializedObject.FindProperty("filledWithSpokes"), new GUIContent("Filled with spokes"));
                    }
                    break;
                case (int)ShapeDrawer.ShapeType_flat.regularPolygon:
                    displayFillstyleOption = true;
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("corners"), new GUIContent("Corners"));
                    Draw_sizeInterpretationDependentLine("radiusScaleFactor", "Radius (of hull circle)", null, sP_sizeDefinition);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("filledWithSpokes"), new GUIContent("Filled with spokes"));
                    break;
                case (int)ShapeDrawer.ShapeType_flat.plane:
                    Draw_sizeInterpretationDependentLine("widthOfPlane_scaleFactor", "Width", null, sP_sizeDefinition);
                    Draw_sizeInterpretationDependentLine("lengthOfPlane_scaleFactor", "Length", null, sP_sizeDefinition);
                    Draw_extendPlaneToOtherGO();
                    Draw_planeStrutConfig();
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("planeAnchorVisualizationSize"), new GUIContent("Size of Anchor Point Visualization"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("pointer_as_textAttachStyle_forPlanes"), new GUIContent("Text position beside plane", "This only applies if 'Drawn text tag' is filled"));
                    break;
                case (int)ShapeDrawer.ShapeType_flat.rhombus:
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("rhombusPositionDescribesCenterNotCorner"), new GUIContent("Position defines center, not corner"));
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("subSegments"), new GUIContent("Sub segments"));
                    break;
                case (int)ShapeDrawer.ShapeType_flat.dot:
                    Draw_sizeInterpretationDependentLine("uniformSizeScaleFactor", "Size", null, sP_sizeDefinition);
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("dotDensity"), new GUIContent("Fill density", "Raise this if you want the dot to be more opaque."));
                    break;
                default:
                    break;
            }
        }

        void DrawGeneralOptions(bool displayFillstyleOption, bool isIcon, bool isDot, bool isPlane, bool isRhombus, SerializedProperty sP_sizeDefinition)
        {
            if (isDot == false)
            {
                Draw_sizeInterpretationDependentLine("linesWidth", "Lines width", null, sP_sizeDefinition);
            }

            if ((isIcon == false) && (isDot == false))
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("lineStyle"), new GUIContent("Line style"));
                Draw_sizeInterpretationDependentLine("stylePatternScaleFactor", "Line style scaling", null, sP_sizeDefinition);
            }

            if (displayFillstyleOption)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("fillStyle"), new GUIContent("Fill style"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("shapeFillDensity"), new GUIContent("Fill density"));
            }

            EditorGUILayout.PropertyField(serializedObject.FindProperty("color"), new GUIContent("Color"));

            bool shapeDisplaysText_viaPointCollection = ((isIcon == false) && (isDot == false) && (isRhombus == false) && ((isPlane == false) || (serializedObject.FindProperty("pointer_as_textAttachStyle_forPlanes").boolValue)));
            DrawTextSpecs(sP_sizeDefinition, shapeDisplaysText_viaPointCollection);
            DrawCheckboxFor_drawOnlyIfSelected("shape");
            DrawCheckboxFor_hiddenByNearerObjects("shape");
        }

        void DrawTextSpecs(SerializedProperty sP_sizeDefinition, bool shapeDisplaysText_viaPointCollection)
        {
            //-> this fakes the appearance of beeing inside the parents text specification foldout
            bool emptyLineAtEndIfOutfolded = false;
            DrawTextInputInclMarkupHelper(true, false, null, emptyLineAtEndIfOutfolded);

            if (serializedObject.FindProperty("textSection_isOutfolded").boolValue == true)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                if (CheckIf_shapeAttachedTextsizeReferenceContext_isUsed(sP_sizeDefinition, shapeDisplaysText_viaPointCollection))
                {
                    DrawTextSizeChooser();
                }

                if (shapeDisplaysText_viaPointCollection)
                {
                    EditorGUILayout.PropertyField(serializedObject.FindProperty("textBlockAboveLine"), new GUIContent("Text block above line"));
                }

                GUILayout.Space(EditorGUIUtility.singleLineHeight);
                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        bool CheckIf_shapeAttachedTextsizeReferenceContext_isUsed(SerializedProperty sP_sizeDefinition, bool shapeDisplaysText_viaPointCollection)
        {
            if (CheckIf_shapeSizeDefinition_isDependentOn_screenspace(sP_sizeDefinition))
            {
                return false;
            }
            else
            {
                return shapeDisplaysText_viaPointCollection;
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
            return ((sP_sizeDefinition.enumValueIndex == (int)ShapeDrawer.ShapeSizeDefinition.relativeToTheSceneViewWindowSize) || (sP_sizeDefinition.enumValueIndex == (int)ShapeDrawer.ShapeSizeDefinition.relativeToTheGameViewWindowSize));
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

        void Draw_extendPlaneToOtherGO()
        {
            SerializedProperty sP_extendPlaneToOtherGO = serializedObject.FindProperty("extendPlaneToOtherGO");
            EditorGUILayout.PropertyField(sP_extendPlaneToOtherGO, new GUIContent("Extend Plane to other Gameobject", "This will extend the drawn plane so it incorporates the position of an other Gameobject, respectively the perpendicular plump position on the plane of this other Gameobject."));
            bool gameobjectIsAssigned = (sP_extendPlaneToOtherGO.objectReferenceValue != null);

            EditorGUI.BeginDisabledGroup(gameobjectIsAssigned == false);
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            string tooltip = gameobjectIsAssigned ? null : "This is only available if the Gameobject in the preceding line is assigned.";
            EditorGUILayout.PropertyField(serializedObject.FindProperty("drawPlumbLine_fromExtentionPosition"), new GUIContent("Draw perpendicular line to other Gameobject", tooltip));
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            EditorGUI.EndDisabledGroup();
        }

        void Draw_planeStrutConfig()
        {
            SerializedProperty sP_planeStrutDefinitionType = serializedObject.FindProperty("planeStrutDefinitionType");
            EditorGUILayout.PropertyField(sP_planeStrutDefinitionType, new GUIContent("Sub segments definition"));

            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            if (sP_planeStrutDefinitionType.enumValueIndex == (int)ShapeDrawer.PlaneStrutDefinitionType.fixedNumber)
            {
                SerializedProperty sP_subSegments = serializedObject.FindProperty("subSegments");
                EditorGUILayout.PropertyField(sP_subSegments, new GUIContent("Number of Sub Segments"));
                sP_subSegments.intValue = Mathf.Max(sP_subSegments.intValue, 1);
            }
            else
            {
                SerializedProperty sP_fixedPlaneStrutDistance = serializedObject.FindProperty("fixedPlaneStrutDistance");
                EditorGUILayout.PropertyField(sP_fixedPlaneStrutDistance, new GUIContent("Distance of Sub Segments", "This is always in world space, so it is independent from the 'Size definition' setting above."));
                sP_fixedPlaneStrutDistance.floatValue = Mathf.Max(sP_fixedPlaneStrutDistance.floatValue, UtilitiesDXXL_Shapes.min_fixedWorldSpaceDistanceOfStrutSegments);
            }

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

    }
#endif
}
