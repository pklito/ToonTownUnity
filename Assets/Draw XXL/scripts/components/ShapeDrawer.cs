namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Shape Drawer")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class ShapeDrawer : VisualizerParent
    {
        public enum ShapeCategory { _3D, flat };
        [SerializeField] ShapeCategory shapeCategory = ShapeCategory._3D;

        public enum ShapeType_3D { cube, sphere, capsule, cylinder, extrusion, ellipsoid, pyramid, bipyramid, cone, frustum };
        [SerializeField] ShapeType_3D shapeType_3D = ShapeType_3D.cube;

        public enum ShapeType_flat { circle, ellipse, star, capsule, icon, triangle, square, pentagon, hexagon, septagon, octagon, decagon, regularPolygon, plane, rhombus, dot };
        [SerializeField] ShapeType_flat shapeType_flat = ShapeType_flat.circle;

        public enum ShapeSizeDefinition { relativeToTheGlobalScaleOfTheTransformRespectivelyItsBiggestAbsoluteComponent, absoluteUnits, relativeToTheSceneViewWindowSize, relativeToTheGameViewWindowSize };
        [SerializeField] ShapeSizeDefinition sizeDefinition = ShapeSizeDefinition.relativeToTheGlobalScaleOfTheTransformRespectivelyItsBiggestAbsoluteComponent;
        float lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
        float biggestAbsGlobalSizeComponentOfTransform = 1.0f;
        [SerializeField] bool cameraForSizeDefinitionIsAvailable = false;
        Camera gameviewCameraForDrawing;
        public enum ShapeAttachedTextsizeReferenceContext { sizeOfShape, globalSpace, sceneViewWindowSize, gameViewWindowSize };
        [SerializeField] ShapeAttachedTextsizeReferenceContext shapeAttachedTextsizeReferenceContext = ShapeAttachedTextsizeReferenceContext.sceneViewWindowSize;
        [SerializeField] float textSize_value = 0.1f;
        [SerializeField] [Range(0.001f, 0.2f)] float textSize_value_relToScreen = 0.02f;

        public enum PyramidDefinitionVariant { fromCenterOfBasePlane, fromApex, fromCenterOfHullVolume };
        [SerializeField] PyramidDefinitionVariant pyramidDefinitionVariant = PyramidDefinitionVariant.fromCenterOfBasePlane;

        public enum FrustumDefinitionVariant { centerOfBigClipPlanePlusDistanceAndScaleFactorOfSmallPlane, centerOfBigClipPlanePlusDistancesToSmallPlaneAndApex, centersOfBigAndSmallClipPlanes, fromApex, fromCenterOfHullVolume };
        [SerializeField] FrustumDefinitionVariant frustumDefinitionVariant = FrustumDefinitionVariant.centerOfBigClipPlanePlusDistanceAndScaleFactorOfSmallPlane;

        public enum CornerOptionsForIrregularStar { _3, _4, _5, _6, _8, _10, _16, _32, _64 };
        [SerializeField] CornerOptionsForIrregularStar cornerOptionsForIrregularStar = CornerOptionsForIrregularStar._5;

        [SerializeField] public string labelOfPosition = ""; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public string labelOfForwardVector = ""; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public string labelOfUpVector = ""; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool forwardVector_hasHigherPrioThan_upVector; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.

        //general settings:
        [SerializeField] Color color = DrawBasics.defaultColor;
        [SerializeField] DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid;
        [SerializeField] DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible;
        [SerializeField] float shapeFillDensity = 1.0f;
        [SerializeField] bool textBlockAboveLine = false;

        //shape specific settings:
        [SerializeField] bool force2DShapeTo_facingToSceneViewCam = false;
        [SerializeField] bool force2DShapeTo_facingToGameViewCam = false;
        [SerializeField] bool coneIsFilled = true;
        [SerializeField] bool rhombusPositionDescribesCenterNotCorner = false;
        [SerializeField] bool cubeIsFilled = false;
        [SerializeField] int segmentsPerSide = 6;
        [SerializeField] Color colorOfSidePlanes = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawBasics.defaultColor, 0.5f);
        [SerializeField] bool useEdgesColorAsTextColor_ifAvailable = true;
        [SerializeField] bool ellipsoidIsNonUniform = false;
        [SerializeField] bool flatShapeIsNonUniform = false;
        [SerializeField] int struts = 2;
        [SerializeField] bool onlyUpperHalf = false;
        [SerializeField] bool drawEquator = true;
        [SerializeField] DrawShapes.Shape2DType baseShape_withInitialValueOf_circle4struts = DrawShapes.Shape2DType.circle4struts;
        [SerializeField] DrawShapes.Shape2DType baseShape_withInitialValueOf_square = DrawShapes.Shape2DType.square;
        [SerializeField] [Range(0.0f, 179.99f)] float angleDegVert_initialValueOf90 = 90.0f;
        [SerializeField] [Range(0.0f, 179.99f)] float angleDegHoriz_initialValueOf90 = 90.0f;
        [SerializeField] [Range(0.0f, 179.99f)] float angleDeg_initialValueOf60 = 60.0f;
        [SerializeField] float aspectRatio = (16.0f / 9.0f);
        [SerializeField] float scalingFactor_forSmallClipPlane = 0.5f;
        [SerializeField] bool flattenRoundLines_intoShapePlane = true;
        [SerializeField] bool filledWithSpokes = false;
        [SerializeField] float innerRadiusFactor = 0.5f;
        [SerializeField] int corners = 5;
        [SerializeField] CapsuleDirection2D capusleDirection2D = CapsuleDirection2D.Vertical;
        [SerializeField] DrawBasics.IconType iconType = DrawBasics.IconType.car;
        [SerializeField] bool iconIsMirroredHorizontally = false;
        [SerializeField] bool showAtlasOfAllAvailableIcons = false;
        [SerializeField] int subSegments = 10;
        [SerializeField] float fixedPlaneStrutDistance = 1.0f;
        [SerializeField] float planeAnchorVisualizationSize = 0.0f;
        [SerializeField] bool pointer_as_textAttachStyle_forPlanes = false;
        [SerializeField] float dotDensity = 1.0f;
        public enum PlaneStrutDefinitionType { fixedNumber, fixedWorldSpaceDistance };
        [SerializeField] PlaneStrutDefinitionType planeStrutDefinitionType = PlaneStrutDefinitionType.fixedNumber;
        [SerializeField] GameObject extendPlaneToOtherGO;
        [SerializeField] bool drawPlumbLine_fromExtentionPosition = true;
        public enum SphereQuality { _64_linesPerSphereCircle, _32_linesPerSphereCircle, _16_linesPerSphereCircle, _8_linesPerSphereCircle };
        [SerializeField] SphereQuality sphereQuality = Map_linesPerSphereCircle_to_sphereQuality(DrawShapes.LinesPerSphereCircle);

        //general - scale type dependent:
        [SerializeField] float linesWidth = 0.0f;
        [SerializeField] [Range(0.0f, 0.02f)] float linesWidth_relToScreen = 0.0f;
        [SerializeField] float stylePatternScaleFactor = 1.0f;
        [SerializeField] float stylePatternScaleFactor_relToScreen = 0.1f;

        //shape specific - scale type dependent:
        [SerializeField] Vector3 scaleFactors_ofHullVolume = Vector3.one;
        [SerializeField] Vector3 scaleFactors_ofHullVolume_relToScreen = 0.1f * Vector3.one;
        [SerializeField] float linesWidthOfCubeFillLines = 0.0f;
        [SerializeField] [Range(0.0f, 0.02f)] float linesWidthOfCubeFillLines_relToScreen = 0.0f;
        [SerializeField] float radiusScaleFactor = 0.5f;
        [SerializeField] [Range(0.0f, 0.5f)] float radiusScaleFactor_relToScreen = 0.05f;
        [SerializeField] float radiusUpScaleFactor_ellipsoid = 1.0f; //-> only difference to "radiusScaleFactor" is the higher initial value, so that the deformation of the sphere shape is visible at first sight.
        [SerializeField] [Range(0.0f, 0.5f)] float radiusUpScaleFactor_ellipsoid_relToScreen = 0.1f;
        [SerializeField] float radiusDownScaleFactor_ellipsoid = 1.0f; //-> is inconsistent to "heightToDown_scaleFactor" because the inverted direction doesn't need the "minus" here.
        [SerializeField] [Range(0.0f, 0.5f)] float radiusDownScaleFactor_ellipsoid_relToScreen = 0.1f;
        [SerializeField] float heightScaleFactor = 1.0f;
        [SerializeField] [Range(0.0f, 1.0f)] float heightScaleFactor_relToScreen = 0.1f;
        [SerializeField] float widthScaleFactor = 1.0f;
        [SerializeField] [Range(0.0f, 1.0f)] float widthScaleFactor_relToScreen = 0.1f;
        [SerializeField] float uniformSizeScaleFactor = 1.0f;
        [SerializeField] [Range(0.0f, 1.0f)] float uniformSizeScaleFactor_relToScreen = 0.1f;
        [SerializeField] float width_ofBase_scaleFactor = 1.0f;
        [SerializeField] float width_ofBase_scaleFactor_relToScreen = 0.1f;
        [SerializeField] float length_ofBase_scaleFactor = 1.0f;
        [SerializeField] float length_ofBase_scaleFactor_relToScreen = 0.1f;
        [SerializeField] float heightToUp_scaleFactor = 1.0f;
        [SerializeField] float heightToUp_scaleFactor_relToScreen = 0.1f;
        [SerializeField] float heightToDown_scaleFactor = -1.0f;
        [SerializeField] float heightToDown_scaleFactor_relToScreen = -0.1f;
        [SerializeField] float heightOfCapsule3D_scaleFactor = 2.0f;
        [SerializeField] float heightOfCapsule3D_scaleFactor_relToScreen = 0.2f;
        [SerializeField] float widthOfCapsule2D_scaleFactor = 1.0f;
        [SerializeField] [Range(0.0f, 1.0f)] float widthOfCapsule2D_scaleFactor_relToScreen = 0.1f;
        [SerializeField] float heightOfCapsule2D_scaleFactor = 2.0f;
        [SerializeField] [Range(0.0f, 1.0f)] float heightOfCapsule2D_scaleFactor_relToScreen = 0.2f;
        [SerializeField] float width_ofBigClipPlane_scaleFactor = 1.0f;
        [SerializeField] float width_ofBigClipPlane_scaleFactor_relToScreen = 0.1f;
        [SerializeField] float height_ofBigClipPlane_scaleFactor = 1.0f;
        [SerializeField] float height_ofBigClipPlane_scaleFactor_relToScreen = 0.1f;
        [SerializeField] float width_ofSmallClipPlane_scaleFactor = 0.5f;
        [SerializeField] float width_ofSmallClipPlane_scaleFactor_relToScreen = 0.05f;
        [SerializeField] float height_ofSmallClipPlane_scaleFactor = 0.5f;
        [SerializeField] float height_ofSmallClipPlane_scaleFactor_relToScreen = 0.05f;
        [SerializeField] float radiusSideward_ofEllipse_scaleFactor = 0.25f;
        [SerializeField] [Range(0.0f, 0.5f)] float radiusSideward_ofEllipse_scaleFactor_relToScreen = 0.025f;
        [SerializeField] float radiusUpward_ofEllipse_scaleFactor = 0.5f;
        [SerializeField] [Range(0.0f, 0.5f)] float radiusUpward_ofEllipse_scaleFactor_relToScreen = 0.05f;
        [SerializeField] float outerRadiusOfStars_scaleFactor = 0.5f;
        [SerializeField] [Range(0.0f, 0.5f)] float outerRadiusOfStars_scaleFactor_relToScreen = 0.05f;
        [SerializeField] float widthOfPlane_scaleFactor = 10.0f;
        [SerializeField] float widthOfPlane_scaleFactor_relToScreen = 0.25f;
        [SerializeField] float lengthOfPlane_scaleFactor = 10.0f;
        [SerializeField] float lengthOfPlane_scaleFactor_relToScreen = 0.25f;
        [SerializeField] float distanceBetweenClipPlanes_scaleFactor = 0.5f;
        [SerializeField] float distanceBetweenClipPlanes_scaleFactor_relToScreen = 0.05f;
        [SerializeField] float distance_bigClipPlaneToApex_scaleFactor = 1.0f;
        [SerializeField] float distance_bigClipPlaneToApex_scaleFactor_relToScreen = 0.1f;
        [SerializeField] float distanceApexToNearPlane_scaleFactor = 0.5f;
        [SerializeField] float distanceApexToNearPlane_scaleFactor_relToScreen = 0.05f;

        public override void InitializeValues_onceInComponentLifetime()
        {
            TrySetTextToEmptyString();

            customVector3_1_picker_isOutfolded = false;
            source_ofCustomVector3_1 = CustomVector3Source.transformsForward;
            customVector3_1_clipboardForManualInput = Vector3.forward;
            vectorInterpretation_ofCustomVector3_1 = VectorInterpretation.globalSpace;

            customVector3_2_picker_isOutfolded = false;
            source_ofCustomVector3_2 = CustomVector3Source.transformsUp;
            customVector3_2_clipboardForManualInput = Vector3.up;
            vectorInterpretation_ofCustomVector3_2 = VectorInterpretation.globalSpace;

            customVector3_3_picker_isOutfolded = false;
            source_ofCustomVector3_3 = CustomVector3Source.transformsForward;
            customVector3_3_clipboardForManualInput = Vector3.forward;
            vectorInterpretation_ofCustomVector3_3 = VectorInterpretation.globalSpace;

            customVector3_4_picker_isOutfolded = false;
            source_ofCustomVector3_4 = CustomVector3Source.transformsUp;
            customVector3_4_clipboardForManualInput = Vector3.up;
            vectorInterpretation_ofCustomVector3_4 = VectorInterpretation.globalSpace;
        }

        public override void DrawVisualizedObject()
        {
            ForceNonRhombusOrientationVectorsToLenghtOf1();
            corners = Mathf.Max(3, corners);
            struts = Mathf.Max(1, struts);
            CacheSizeScaleFactors();

            switch (shapeCategory)
            {
                case ShapeCategory._3D:
                    DrawAThreeDimensionalShape();
                    break;
                case ShapeCategory.flat:
                    DrawAFlatShape();
                    break;
                default:
                    break;
            }
        }

        void ForceNonRhombusOrientationVectorsToLenghtOf1()
        {
            bool isRhombus = ((shapeCategory == ShapeCategory.flat) && (shapeType_flat == ShapeType_flat.rhombus));
            if (isRhombus == false)
            {
                customVector3_1_hasForcedAbsLength = false;
                lengthRelScaleFactor_ofCustomVector3_1 = 1.0f;
                customVector3_2_hasForcedAbsLength = false;
                lengthRelScaleFactor_ofCustomVector3_2 = 1.0f;
            }
        }

        void CacheSizeScaleFactors()
        {
            biggestAbsGlobalSizeComponentOfTransform = UtilitiesDXXL_Math.GetBiggestAbsComponent(transform.lossyScale);
            cameraForSizeDefinitionIsAvailable = false;
            switch (sizeDefinition)
            {
                case ShapeSizeDefinition.relativeToTheGlobalScaleOfTheTransformRespectivelyItsBiggestAbsoluteComponent:
                    lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
                    break;
                case ShapeSizeDefinition.absoluteUnits:
                    lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
                    break;
                case ShapeSizeDefinition.relativeToTheSceneViewWindowSize:
#if UNITY_EDITOR
                    if (UnityEditor.SceneView.lastActiveSceneView != null)
                    {
                        cameraForSizeDefinitionIsAvailable = true;
                        float distance_ofDrawnObject_toCamera = (GetDrawPos3D_global() - UnityEditor.SceneView.lastActiveSceneView.camera.transform.position).magnitude;
                        lengthOfScreenDiagonal_atDrawnObjectsPosition = UtilitiesDXXL_Screenspace.Get_diagonalExtentOfViewport_at_distanceFromCam(UnityEditor.SceneView.lastActiveSceneView.camera, distance_ofDrawnObject_toCamera);
                    }
                    else
                    {
                        lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
                    }
#else
                    lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
#endif
                    break;
                case ShapeSizeDefinition.relativeToTheGameViewWindowSize:
                    cameraForSizeDefinitionIsAvailable = UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out gameviewCameraForDrawing, "Shape Drawer Component", false);
                    if (cameraForSizeDefinitionIsAvailable)
                    {
                        float distance_ofDrawnObject_toCamera = (GetDrawPos3D_global() - gameviewCameraForDrawing.transform.position).magnitude;
                        lengthOfScreenDiagonal_atDrawnObjectsPosition = UtilitiesDXXL_Screenspace.Get_diagonalExtentOfViewport_at_distanceFromCam(gameviewCameraForDrawing, distance_ofDrawnObject_toCamera);
                    }
                    else
                    {
                        lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
                    }
                    break;
                default:
                    lengthOfScreenDiagonal_atDrawnObjectsPosition = 1.0f;
                    break;
            }
        }

        void DrawAThreeDimensionalShape()
        {
            Set_globalTextSizeSpecs_reversible();
            switch (shapeType_3D)
            {
                case ShapeType_3D.cube:
                    labelOfPosition = "Position of cube center";
                    labelOfForwardVector = "<b>Forward orientation of cube</b>";
                    labelOfUpVector = "<b>Upward orientation of cube</b>";
                    forwardVector_hasHigherPrioThan_upVector = true;
                    if (cubeIsFilled)
                    {
                        DrawShapes.CubeFilled(GetDrawPos3D_global(), GetScaledHullSize(), colorOfSidePlanes, GetUpwardVector(), GetForwardVector(), Get_linesWidthOfCubeFillLines(), segmentsPerSide, text_inclGlobalMarkupTags, lineStyle, color, Get_linesWidth(), Get_stylePatternScaleFactor(), useEdgesColorAsTextColor_ifAvailable, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    else
                    {
                        DrawShapes.Cube(GetDrawPos3D_global(), GetScaledHullSize(), color, GetUpwardVector(), GetForwardVector(), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    break;
                case ShapeType_3D.sphere:
                    labelOfPosition = "Position of sphere center";
                    labelOfForwardVector = "<b>Forward orientation of sphere</b>";
                    labelOfUpVector = "<b>Upward orientation of sphere</b>";
                    forwardVector_hasHigherPrioThan_upVector = false;
                    UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(Map_sphereQuality_to_linesPerSphereCircle(sphereQuality));
                    DrawShapes.Sphere(GetDrawPos3D_global(), GetRadius(), color, GetUpwardVector(), GetForwardVector(), Get_linesWidth(), text_inclGlobalMarkupTags, struts, onlyUpperHalf, lineStyle, Get_stylePatternScaleFactor(), !drawEquator, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                    break;
                case ShapeType_3D.capsule:
                    labelOfPosition = "Position of capsule center";
                    labelOfForwardVector = "<b>Forward orientation of capsule</b>";
                    labelOfUpVector = "<b>Upward orientation of capsule</b>";
                    forwardVector_hasHigherPrioThan_upVector = false;
                    //-> radius cannot be restricted to be based on only 2 components (e.g. x and z for capsules along z) (as Collider.Capsule does), because the capsuleUpwardVector can be freely chosen and is not restricted to the transform directions
                    //-> same for "height"
                    UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(Map_sphereQuality_to_linesPerSphereCircle(sphereQuality));
                    DrawShapes.Capsule(GetDrawPos3D_global(), GetRadius(), Get_heightOfCapsule3D(), color, GetUpwardVector(), GetForwardVector(), Get_linesWidth(), text_inclGlobalMarkupTags, struts, onlyUpperHalf, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                    break;
                case ShapeType_3D.cylinder:
                    labelOfPosition = "Position of cylinder center";
                    labelOfForwardVector = "<b>Up orientation inside cross section plane</b>";
                    labelOfUpVector = "<b>Extrusion direction</b>";
                    forwardVector_hasHigherPrioThan_upVector = false;
                    DrawShapes.Cylinder(GetDrawPos3D_global(), Get_height(), Get_width_ofBase(), Get_length_ofBase(), color, GetUpwardVector(), GetForwardVector(), baseShape_withInitialValueOf_circle4struts, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    break;
                case ShapeType_3D.extrusion:
                    labelOfPosition = "Position of extrusion base";
                    labelOfForwardVector = "<b>Up orientation inside cross section plane</b>";
                    labelOfUpVector = "<b>Extrusion direction</b>";
                    forwardVector_hasHigherPrioThan_upVector = false;
                    DrawShapes.Extrusion(GetDrawPos3D_global(), Get_heightToUp(), Get_heightToDown(), Get_width_ofBase(), Get_length_ofBase(), color, GetUpwardVector(), GetForwardVector(), baseShape_withInitialValueOf_square, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    break;
                case ShapeType_3D.ellipsoid:
                    labelOfPosition = "Position of ellipsoid center";
                    labelOfForwardVector = "<b>Forward orientation of ellipsoid</b>";
                    labelOfUpVector = "<b>Upward orientation of ellipsoid</b>";
                    forwardVector_hasHigherPrioThan_upVector = false;

                    UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(Map_sphereQuality_to_linesPerSphereCircle(sphereQuality));
                    if (ellipsoidIsNonUniform)
                    {
                        DrawShapes.EllipsoidNonUniform(GetDrawPos3D_global(), 0.5f * Get_width_ofBase(), GetRadiusUp_ellipsoid(), GetRadiusDown_ellipsoid(), 0.5f * Get_length_ofBase(), color, GetUpwardVector(), GetForwardVector(), Get_linesWidth(), text_inclGlobalMarkupTags, struts, lineStyle, Get_stylePatternScaleFactor(), !drawEquator, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    else
                    {
                        Vector3 radius_forEachDim = new Vector3(0.5f * Get_width_ofBase(), GetRadiusUp_ellipsoid(), 0.5f * Get_length_ofBase());
                        DrawShapes.Ellipsoid(GetDrawPos3D_global(), radius_forEachDim, color, GetUpwardVector(), GetForwardVector(), Get_linesWidth(), text_inclGlobalMarkupTags, struts, onlyUpperHalf, lineStyle, Get_stylePatternScaleFactor(), !drawEquator, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                    break;
                case ShapeType_3D.pyramid:
                    switch (pyramidDefinitionVariant)
                    {
                        case PyramidDefinitionVariant.fromCenterOfBasePlane:
                            labelOfPosition = "Position of center of base plane";
                            labelOfForwardVector = "<b>Up orientation inside base plane</b>";
                            labelOfUpVector = "<b>Normal of base towards apex</b>";
                            forwardVector_hasHigherPrioThan_upVector = false;
                            DrawShapes.Pyramid(GetDrawPos3D_global(), Get_height(), Get_width_ofBase(), Get_length_ofBase(), color, GetUpwardVector(), GetForwardVector(), baseShape_withInitialValueOf_square, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                            break;
                        case PyramidDefinitionVariant.fromApex:
                            labelOfPosition = "Position of apex";
                            labelOfForwardVector = "<b>Forward direction from apex towards base plane</b>";
                            labelOfUpVector = "<b>Up orientation inside base plane</b>";
                            forwardVector_hasHigherPrioThan_upVector = true;
                            DrawShapes.Pyramid(GetDrawPos3D_global(), Get_height(), GetForwardVector(), GetUpwardVector(), angleDegVert_initialValueOf90, angleDegHoriz_initialValueOf90, color, baseShape_withInitialValueOf_square, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                            break;
                        case PyramidDefinitionVariant.fromCenterOfHullVolume:
                            labelOfPosition = "Position of center of pyramid hull volume";
                            labelOfForwardVector = "<b>Up orientation inside base plane</b>";
                            labelOfUpVector = "<b>Normal of base towards apex</b>";
                            forwardVector_hasHigherPrioThan_upVector = false;
                            Quaternion rotation = Quaternion.LookRotation(GetForwardVector(), GetUpwardVector());
                            DrawShapes.Pyramid(GetDrawPos3D_global(), GetScaledHullSize(), rotation, color, baseShape_withInitialValueOf_square, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                            break;
                        default:
                            break;
                    }
                    break;
                case ShapeType_3D.bipyramid:
                    labelOfPosition = "Position of center of base plane";
                    labelOfForwardVector = "<b>Up orientation inside base plane</b>";
                    labelOfUpVector = "<b>Normal of base towards upper apex</b>";
                    forwardVector_hasHigherPrioThan_upVector = false;
                    DrawShapes.Bipyramid(GetDrawPos3D_global(), Get_heightToUp(), Get_heightToDown(), Get_width_ofBase(), Get_length_ofBase(), color, GetUpwardVector(), GetForwardVector(), baseShape_withInitialValueOf_square, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    break;
                case ShapeType_3D.cone:
                    if (coneIsFilled)
                    {
                        switch (pyramidDefinitionVariant)
                        {
                            case PyramidDefinitionVariant.fromCenterOfBasePlane:
                                labelOfPosition = "Position of center of base circle";
                                labelOfForwardVector = "<b>Up orientation inside base circle</b>";
                                labelOfUpVector = "<b>Normal of base circle towards apex</b>";
                                forwardVector_hasHigherPrioThan_upVector = false;
                                DrawShapes.ConeFilled(GetDrawPos3D_global(), Get_height(), Get_width_ofBase(), Get_length_ofBase(), color, GetUpwardVector(), GetForwardVector(), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                                break;
                            case PyramidDefinitionVariant.fromApex:
                                labelOfPosition = "Position of apex";
                                labelOfForwardVector = "<b>Forward direction from apex towards base circle</b>";
                                labelOfUpVector = "<b>Up orientation inside base circle</b>";
                                forwardVector_hasHigherPrioThan_upVector = true;
                                DrawShapes.ConeFilled(GetDrawPos3D_global(), Get_height(), GetForwardVector(), GetUpwardVector(), angleDegVert_initialValueOf90, angleDegHoriz_initialValueOf90, color, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                                break;
                            case PyramidDefinitionVariant.fromCenterOfHullVolume:
                                labelOfPosition = "Position of center of cone hull volume";
                                labelOfForwardVector = "<b>Up orientation inside base circle</b>";
                                labelOfUpVector = "<b>Normal of base circle towards apex</b>";
                                forwardVector_hasHigherPrioThan_upVector = false;
                                Quaternion rotation = Quaternion.LookRotation(GetForwardVector(), GetUpwardVector());
                                DrawShapes.ConeFilled(GetDrawPos3D_global(), GetScaledHullSize(), rotation, color, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                                break;
                            default:
                                break;
                        }
                    }
                    else
                    {
                        switch (pyramidDefinitionVariant)
                        {
                            case PyramidDefinitionVariant.fromCenterOfBasePlane:
                                labelOfPosition = "Position of center of base circle";
                                labelOfForwardVector = "<b>Up orientation inside base circle</b>";
                                labelOfUpVector = "<b>Normal of base circle towards apex</b>";
                                forwardVector_hasHigherPrioThan_upVector = false;
                                DrawShapes.Cone(GetDrawPos3D_global(), Get_height(), Get_width_ofBase(), Get_length_ofBase(), color, GetUpwardVector(), GetForwardVector(), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                                break;
                            case PyramidDefinitionVariant.fromApex:
                                labelOfPosition = "Position of apex";
                                labelOfForwardVector = "<b>Forward direction from apex towards base circle</b>";
                                labelOfUpVector = "<b>Up orientation inside base circle</b>";
                                forwardVector_hasHigherPrioThan_upVector = true;
                                DrawShapes.Cone(GetDrawPos3D_global(), Get_height(), GetForwardVector(), GetUpwardVector(), angleDegVert_initialValueOf90, angleDegHoriz_initialValueOf90, color, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                                break;
                            case PyramidDefinitionVariant.fromCenterOfHullVolume:
                                labelOfPosition = "Position of center of cone hull volume";
                                labelOfForwardVector = "<b>Up orientation inside base circle</b>";
                                labelOfUpVector = "<b>Normal of base circle towards apex</b>";
                                forwardVector_hasHigherPrioThan_upVector = false;
                                Quaternion rotation = Quaternion.LookRotation(GetForwardVector(), GetUpwardVector());
                                DrawShapes.Cone(GetDrawPos3D_global(), GetScaledHullSize(), rotation, color, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                case ShapeType_3D.frustum:
                    switch (frustumDefinitionVariant)
                    {
                        case FrustumDefinitionVariant.centerOfBigClipPlanePlusDistanceAndScaleFactorOfSmallPlane:
                            labelOfPosition = "Position of center of big clip plane";
                            labelOfForwardVector = "<b>Up orientation inside clip plane</b>";
                            labelOfUpVector = "<b>Normal of clip plane towards apex</b>";
                            forwardVector_hasHigherPrioThan_upVector = false;
                            DrawShapes.Frustum(GetDrawPos3D_global(), Get_distanceBetweenClipPlanes(), scalingFactor_forSmallClipPlane, GetUpwardVector(), GetForwardVector(), Get_width_ofBigClipPlane(), Get_height_ofBigClipPlane(), color, baseShape_withInitialValueOf_square, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                            break;
                        case FrustumDefinitionVariant.centerOfBigClipPlanePlusDistancesToSmallPlaneAndApex:
                            labelOfPosition = "Position of center of big clip plane";
                            labelOfForwardVector = "<b>Up orientation inside clip plane</b>";
                            labelOfUpVector = "<b>Normal of clip plane towards apex</b>";
                            forwardVector_hasHigherPrioThan_upVector = false;
                            DrawShapes.Frustum(Get_distance_bigClipPlaneToApex(), Get_distanceBetweenClipPlanes(), GetDrawPos3D_global(), GetUpwardVector(), GetForwardVector(), Get_width_ofBigClipPlane(), Get_height_ofBigClipPlane(), color, baseShape_withInitialValueOf_square, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                            break;
                        case FrustumDefinitionVariant.centersOfBigAndSmallClipPlanes:
                            labelOfPosition = "Position of center of big clip plane";
                            labelOfForwardVector = "<b>Up orientation inside clip planes</b>";
                            labelOfUpVector = "<b>Fallback for normal of clip planes towards apex</b>";
                            forwardVector_hasHigherPrioThan_upVector = true;
                            DrawShapes.Frustum(GetDrawPos3D_global(), GetDrawPos3D_ofPartnerGameobject_global(), Get_width_ofBigClipPlane(), Get_height_ofBigClipPlane(), Get_width_ofSmallClipPlane(), Get_height_ofSmallClipPlane(), color, GetForwardVector(), GetUpwardVector(), baseShape_withInitialValueOf_square, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                            break;
                        case FrustumDefinitionVariant.fromApex:
                            labelOfPosition = "Position of apex";
                            labelOfForwardVector = "<b>Forward direction from apex towards clip planes</b>";
                            labelOfUpVector = "<b>Up orientation inside clip planes</b>";
                            forwardVector_hasHigherPrioThan_upVector = true;
                            DrawShapes.Frustum(GetDrawPos3D_global(), GetForwardVector(), GetUpwardVector(), angleDeg_initialValueOf60, aspectRatio, Get_distanceApexToNearPlane(), Get_distance_bigClipPlaneToApex(), color, baseShape_withInitialValueOf_square, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                            break;
                        case FrustumDefinitionVariant.fromCenterOfHullVolume:
                            labelOfPosition = "Position of center of frustums hull volume";
                            labelOfForwardVector = "<b>Up orientation inside clip planes</b>";
                            labelOfUpVector = "<b>Normal of clip planes towards apex</b>";
                            forwardVector_hasHigherPrioThan_upVector = false;
                            Quaternion rotation = Quaternion.LookRotation(GetForwardVector(), GetUpwardVector());
                            DrawShapes.Frustum(GetDrawPos3D_global(), GetScaledHullSize(), rotation, scalingFactor_forSmallClipPlane, color, baseShape_withInitialValueOf_square, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            Reverse_globalTextSizeSpecs();
        }

        void DrawAFlatShape()
        {
            switch (shapeType_flat)
            {
                case ShapeType_flat.circle:
                    labelOfPosition = "Position of circle center";
                    labelOfForwardVector = "<b>Normal of circle plane</b>";
                    labelOfUpVector = "<b>Upward orientation inside circle plane</b>";
                    forwardVector_hasHigherPrioThan_upVector = true;

                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    if (flatShapeIsNonUniform)
                    {
                        DrawShapes.FlatShape(GetDrawPos3D_global(), DrawShapes.Shape2DType.circle, Get_width(), Get_height(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), flattenRoundLines_intoShapePlane, fillStyle, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    else
                    {
                        DrawShapes.Circle(GetDrawPos3D_global(), GetRadius(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), fillStyle, filledWithSpokes, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType_flat.ellipse:
                    labelOfPosition = "Position of ellipse center";
                    labelOfForwardVector = "<b>Normal of ellipse plane</b>";
                    labelOfUpVector = "<b>Upward orientation inside ellipse plane</b>";
                    forwardVector_hasHigherPrioThan_upVector = true;

                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    DrawShapes.Ellipse(GetDrawPos3D_global(), Get_radiusSideward_ofEllipse(), Get_radiusUpward_ofEllipse(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), fillStyle, filledWithSpokes, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType_flat.star:
                    labelOfPosition = "Position of star center";
                    labelOfForwardVector = "<b>Normal of star plane</b>";
                    labelOfUpVector = "<b>Upward orientation inside star plane</b>";
                    forwardVector_hasHigherPrioThan_upVector = true;
                    Set_globalTextSizeSpecs_reversible();
                    if (flatShapeIsNonUniform)
                    {
                        DrawShapes.FlatShape(GetDrawPos3D_global(), Get_shape2DType_forIrregularStar(cornerOptionsForIrregularStar), Get_width(), Get_height(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), flattenRoundLines_intoShapePlane, DrawBasics.LineStyle.invisible, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    else
                    {
                        DrawShapes.Star(GetDrawPos3D_global(), Get_outerRadiusOfStars(), color, corners, innerRadiusFactor, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), filledWithSpokes, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType_flat.capsule:
                    labelOfPosition = "Position of capsule center";
                    labelOfForwardVector = "<b>Normal of capsule plane</b>";
                    labelOfUpVector = "<b>Upward orientation inside capsule plane</b>";
                    forwardVector_hasHigherPrioThan_upVector = true;

                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    DrawShapes.FlatCapsule(GetDrawPos3D_global(), Get_widthOfCapsule2D(), Get_heightOfCapsule2D(), color, GetForwardVector(true), GetUpwardVector(true), capusleDirection2D, Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), fillStyle, filledWithSpokes, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType_flat.icon:
                    labelOfPosition = "Position of icon center";
                    labelOfForwardVector = "<b>Normal of icon plane</b>";
                    labelOfUpVector = "<b>Upward orientation inside icon plane</b>";
                    forwardVector_hasHigherPrioThan_upVector = true;
                    float uniformSize = Get_uniformSize();
                    int strokeWidth_asPPMofSize = 0;
                    if ((UtilitiesDXXL_Math.ApproximatelyZero(uniformSize) == false) && (UtilitiesDXXL_Math.ApproximatelyZero(linesWidth) == false))
                    {
                        strokeWidth_asPPMofSize = (int)(1000000.0f * (Get_linesWidth() / uniformSize));
                    }
                    DrawBasics.Icon(GetDrawPos3D_global(), iconType, color, uniformSize, text_inclGlobalMarkupTags, GetForwardVector(true), GetUpwardVector(true), strokeWidth_asPPMofSize, iconIsMirroredHorizontally, 0.0f, hiddenByNearerObjects);

                    if (showAtlasOfAllAvailableIcons)
                    {
                        DrawBasics.DrawAtlasOfAllIconsWithTheirNames(GetDrawPos3D_global(), default(Color), default(Color), true, biggestAbsGlobalSizeComponentOfTransform);
                    }
                    break;
                case ShapeType_flat.triangle:
                    labelOfPosition = "Position of triangle center";
                    labelOfForwardVector = "<b>Normal of triangle plane</b>";
                    labelOfUpVector = "<b>Upward orientation inside triangle plane</b>";
                    forwardVector_hasHigherPrioThan_upVector = true;

                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    if (flatShapeIsNonUniform)
                    {
                        DrawShapes.FlatShape(GetDrawPos3D_global(), DrawShapes.Shape2DType.triangle, Get_width(), Get_height(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), flattenRoundLines_intoShapePlane, fillStyle, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    else
                    {
                        DrawShapes.Triangle(GetDrawPos3D_global(), GetRadius(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), fillStyle, filledWithSpokes, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType_flat.square:
                    labelOfPosition = "Position of square center";
                    labelOfForwardVector = "<b>Normal of square plane</b>";
                    labelOfUpVector = "<b>Upward orientation inside square plane</b>";
                    forwardVector_hasHigherPrioThan_upVector = true;

                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    if (flatShapeIsNonUniform)
                    {
                        DrawShapes.FlatShape(GetDrawPos3D_global(), DrawShapes.Shape2DType.square, Get_width(), Get_height(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), flattenRoundLines_intoShapePlane, fillStyle, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    else
                    {
                        DrawShapes.Square(GetDrawPos3D_global(), Get_uniformSize(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), fillStyle, filledWithSpokes, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType_flat.pentagon:
                    labelOfPosition = "Position of pentagon center";
                    labelOfForwardVector = "<b>Normal of pentagon plane</b>";
                    labelOfUpVector = "<b>Upward orientation inside pentagon plane</b>";
                    forwardVector_hasHigherPrioThan_upVector = true;

                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    if (flatShapeIsNonUniform)
                    {
                        DrawShapes.FlatShape(GetDrawPos3D_global(), DrawShapes.Shape2DType.pentagon, Get_width(), Get_height(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), flattenRoundLines_intoShapePlane, fillStyle, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    else
                    {
                        DrawShapes.Pentagon(GetDrawPos3D_global(), GetRadius(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), fillStyle, filledWithSpokes, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType_flat.hexagon:
                    labelOfPosition = "Position of hexagon center";
                    labelOfForwardVector = "<b>Normal of hexagon plane</b>";
                    labelOfUpVector = "<b>Upward orientation inside hexagon plane</b>";
                    forwardVector_hasHigherPrioThan_upVector = true;

                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    if (flatShapeIsNonUniform)
                    {
                        DrawShapes.FlatShape(GetDrawPos3D_global(), DrawShapes.Shape2DType.hexagon, Get_width(), Get_height(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), flattenRoundLines_intoShapePlane, fillStyle, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    else
                    {
                        DrawShapes.Hexagon(GetDrawPos3D_global(), GetRadius(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), fillStyle, filledWithSpokes, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType_flat.septagon:
                    labelOfPosition = "Position of septagon center";
                    labelOfForwardVector = "<b>Normal of septagon plane</b>";
                    labelOfUpVector = "<b>Upward orientation inside septagon plane</b>";
                    forwardVector_hasHigherPrioThan_upVector = true;

                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    if (flatShapeIsNonUniform)
                    {
                        DrawShapes.FlatShape(GetDrawPos3D_global(), DrawShapes.Shape2DType.septagon, Get_width(), Get_height(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), flattenRoundLines_intoShapePlane, fillStyle, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    else
                    {
                        DrawShapes.Septagon(GetDrawPos3D_global(), GetRadius(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), fillStyle, filledWithSpokes, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType_flat.octagon:
                    labelOfPosition = "Position of octagon center";
                    labelOfForwardVector = "<b>Normal of octagon plane</b>";
                    labelOfUpVector = "<b>Upward orientation inside octagon plane</b>";
                    forwardVector_hasHigherPrioThan_upVector = true;

                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    if (flatShapeIsNonUniform)
                    {
                        DrawShapes.FlatShape(GetDrawPos3D_global(), DrawShapes.Shape2DType.octagon, Get_width(), Get_height(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), flattenRoundLines_intoShapePlane, fillStyle, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    else
                    {
                        DrawShapes.Octagon(GetDrawPos3D_global(), GetRadius(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), fillStyle, filledWithSpokes, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType_flat.decagon:
                    labelOfPosition = "Position of decagon center";
                    labelOfForwardVector = "<b>Normal of decagon plane</b>";
                    labelOfUpVector = "<b>Upward orientation inside decagon plane</b>";
                    forwardVector_hasHigherPrioThan_upVector = true;

                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    if (flatShapeIsNonUniform)
                    {
                        DrawShapes.FlatShape(GetDrawPos3D_global(), DrawShapes.Shape2DType.decagon, Get_width(), Get_height(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), flattenRoundLines_intoShapePlane, fillStyle, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    else
                    {
                        DrawShapes.Decagon(GetDrawPos3D_global(), GetRadius(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), fillStyle, filledWithSpokes, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    }
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType_flat.regularPolygon:
                    labelOfPosition = "Position of polygon center";
                    labelOfForwardVector = "<b>Normal of polygon plane</b>";
                    labelOfUpVector = "<b>Upward orientation inside polygon plane</b>";
                    forwardVector_hasHigherPrioThan_upVector = true;

                    Set_globalTextSizeSpecs_reversible();
                    UtilitiesDXXL_Shapes.Set_shapeFillDensity_reversible(shapeFillDensity);
                    DrawShapes.RegularPolygon(corners, GetDrawPos3D_global(), GetRadius(), color, GetForwardVector(true), GetUpwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, lineStyle, Get_stylePatternScaleFactor(), fillStyle, filledWithSpokes, textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_Shapes.Reverse_shapeFillDensity();
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType_flat.plane:
                    //-> meaning of "up" and "forward" is inconsistent to the polygon shapes above to fit the default orientation of a plane mesh on transforms
                    labelOfPosition = "Position of plane center";
                    labelOfForwardVector = "<b>Forward orientation inside plane</b>";
                    labelOfUpVector = "<b>Normal of plane</b>";
                    forwardVector_hasHigherPrioThan_upVector = false;

                    float subSegments_signFlipsInterpretation = (planeStrutDefinitionType == PlaneStrutDefinitionType.fixedWorldSpaceDistance) ? (-fixedPlaneStrutDistance) : subSegments;
                    Vector3 planeAreaExtentionPosition = (extendPlaneToOtherGO == null) ? default(Vector3) : extendPlaneToOtherGO.transform.position;

                    Set_globalTextSizeSpecs_reversible(); //is only used by planes if "pointer_as_textAttachStyle_forPlanes == true", but does not harm in other cases
                    DrawShapes.Plane(GetDrawPos3D_global(), GetUpwardVector(true), planeAreaExtentionPosition, color, Get_widthOfPlane(), Get_lengthOfPlane(), GetForwardVector(true), Get_linesWidth(), text_inclGlobalMarkupTags, subSegments_signFlipsInterpretation, pointer_as_textAttachStyle_forPlanes, planeAnchorVisualizationSize, drawPlumbLine_fromExtentionPosition, lineStyle, Get_stylePatternScaleFactor(), textBlockAboveLine, 0.0f, hiddenByNearerObjects);
                    Reverse_globalTextSizeSpecs();
                    break;
                case ShapeType_flat.rhombus:
                    if (rhombusPositionDescribesCenterNotCorner)
                    {
                        labelOfPosition = "Position of rhombus center";
                        labelOfForwardVector = "<b>Rhombus edge 1</b>";
                        labelOfUpVector = "<b>Rhombus edge 2</b>";
                        forwardVector_hasHigherPrioThan_upVector = true;
                        DrawShapes.RhombusAroundCenter(GetDrawPos3D_global(), Get_customVector3_1_inGlobalSpaceUnits(), Get_customVector3_2_inGlobalSpaceUnits(), color, Get_linesWidth(), text_inclGlobalMarkupTags, subSegments, lineStyle, Get_stylePatternScaleFactor(), 0.0f, hiddenByNearerObjects);
                    }
                    else
                    {
                        labelOfPosition = "Position of start corner";
                        labelOfForwardVector = "<b>Rhombus edge 1</b>";
                        labelOfUpVector = "<b>Rhombus edge 2</b>";
                        forwardVector_hasHigherPrioThan_upVector = true;
                        DrawShapes.Rhombus(GetDrawPos3D_global(), Get_customVector3_1_inGlobalSpaceUnits(), Get_customVector3_2_inGlobalSpaceUnits(), color, Get_linesWidth(), text_inclGlobalMarkupTags, subSegments, lineStyle, Get_stylePatternScaleFactor(), 0.0f, hiddenByNearerObjects);
                    }
                    break;
                case ShapeType_flat.dot:
                    labelOfPosition = "Position of dot center";
                    labelOfForwardVector = "<b>Normal of Dot</b>";
                    labelOfUpVector = "<b>Upward orientation inside dot plane</b>"; //not used
                    forwardVector_hasHigherPrioThan_upVector = true;
                    DrawBasics.Dot(GetDrawPos3D_global(), 0.5f * Get_uniformSize(), GetForwardVector(true), color, text_inclGlobalMarkupTags, dotDensity, 0.0f, hiddenByNearerObjects);
                    break;
                default:
                    break;
            }
        }

        Vector3 GetForwardVector(bool tryUseFallbacksForForcingToObserverCam = false)
        {
            if (tryUseFallbacksForForcingToObserverCam)
            {
                if (force2DShapeTo_facingToSceneViewCam || force2DShapeTo_facingToGameViewCam)
                {
                    return Get_customVector3_3_inGlobalSpaceUnits();
                }
                else
                {
                    return Get_customVector3_1_inGlobalSpaceUnits();
                }
            }
            else
            {
                return Get_customVector3_1_inGlobalSpaceUnits();
            }
        }

        Vector3 GetUpwardVector(bool tryUseFallbacksForForcingToObserverCam = false)
        {
            if (tryUseFallbacksForForcingToObserverCam)
            {
                if (force2DShapeTo_facingToSceneViewCam || force2DShapeTo_facingToGameViewCam)
                {
                    return Get_customVector3_4_inGlobalSpaceUnits();
                }
                else
                {
                    return Get_customVector3_2_inGlobalSpaceUnits();
                }
            }
            else
            {
                return Get_customVector3_2_inGlobalSpaceUnits();
            }
        }

        Vector3 GetScaledHullSize()
        {
            if (sizeDefinition == ShapeSizeDefinition.relativeToTheGlobalScaleOfTheTransformRespectivelyItsBiggestAbsoluteComponent)
            {
                return Vector3.Scale(transform.lossyScale, scaleFactors_ofHullVolume);
            }
            else
            {
                float scaled_x = ScaleInputFloat_accordingToSizeDefinition(scaleFactors_ofHullVolume_relToScreen.x, scaleFactors_ofHullVolume.x);
                float scaled_y = ScaleInputFloat_accordingToSizeDefinition(scaleFactors_ofHullVolume_relToScreen.y, scaleFactors_ofHullVolume.y);
                float scaled_z = ScaleInputFloat_accordingToSizeDefinition(scaleFactors_ofHullVolume_relToScreen.z, scaleFactors_ofHullVolume.z);
                return new Vector3(scaled_x, scaled_y, scaled_z);
            }
        }

        float GetRadius()
        {
            return ScaleInputFloat_accordingToSizeDefinition(radiusScaleFactor_relToScreen, radiusScaleFactor);
        }

        float GetRadiusUp_ellipsoid()
        {
            return ScaleInputFloat_accordingToSizeDefinition(radiusUpScaleFactor_ellipsoid_relToScreen, radiusUpScaleFactor_ellipsoid);
        }

        float GetRadiusDown_ellipsoid()
        {
            return ScaleInputFloat_accordingToSizeDefinition(radiusDownScaleFactor_ellipsoid_relToScreen, radiusDownScaleFactor_ellipsoid);
        }

        float Get_height()
        {
            return ScaleInputFloat_accordingToSizeDefinition(heightScaleFactor_relToScreen, heightScaleFactor);
        }

        float Get_width()
        {
            return ScaleInputFloat_accordingToSizeDefinition(widthScaleFactor_relToScreen, widthScaleFactor);
        }

        float Get_uniformSize()
        {
            return ScaleInputFloat_accordingToSizeDefinition(uniformSizeScaleFactor_relToScreen, uniformSizeScaleFactor);
        }

        float Get_width_ofBase()
        {
            return ScaleInputFloat_accordingToSizeDefinition(width_ofBase_scaleFactor_relToScreen, width_ofBase_scaleFactor);
        }

        float Get_length_ofBase()
        {
            return ScaleInputFloat_accordingToSizeDefinition(length_ofBase_scaleFactor_relToScreen, length_ofBase_scaleFactor);
        }

        float Get_heightToUp()
        {
            return ScaleInputFloat_accordingToSizeDefinition(heightToUp_scaleFactor_relToScreen, heightToUp_scaleFactor);
        }

        float Get_heightToDown()
        {
            return ScaleInputFloat_accordingToSizeDefinition(heightToDown_scaleFactor_relToScreen, heightToDown_scaleFactor);
        }

        float Get_width_ofBigClipPlane()
        {
            return ScaleInputFloat_accordingToSizeDefinition(width_ofBigClipPlane_scaleFactor_relToScreen, width_ofBigClipPlane_scaleFactor);
        }

        float Get_height_ofBigClipPlane()
        {
            return ScaleInputFloat_accordingToSizeDefinition(height_ofBigClipPlane_scaleFactor_relToScreen, height_ofBigClipPlane_scaleFactor);
        }

        float Get_width_ofSmallClipPlane()
        {
            return ScaleInputFloat_accordingToSizeDefinition(width_ofSmallClipPlane_scaleFactor_relToScreen, width_ofSmallClipPlane_scaleFactor);
        }

        float Get_height_ofSmallClipPlane()
        {
            return ScaleInputFloat_accordingToSizeDefinition(height_ofSmallClipPlane_scaleFactor_relToScreen, height_ofSmallClipPlane_scaleFactor);
        }

        float Get_distanceBetweenClipPlanes()
        {
            return ScaleInputFloat_accordingToSizeDefinition(distanceBetweenClipPlanes_scaleFactor_relToScreen, distanceBetweenClipPlanes_scaleFactor);
        }

        float Get_distance_bigClipPlaneToApex()
        {
            return ScaleInputFloat_accordingToSizeDefinition(distance_bigClipPlaneToApex_scaleFactor_relToScreen, distance_bigClipPlaneToApex_scaleFactor);
        }

        float Get_distanceApexToNearPlane()
        {
            return ScaleInputFloat_accordingToSizeDefinition(distanceApexToNearPlane_scaleFactor_relToScreen, distanceApexToNearPlane_scaleFactor);
        }

        float Get_radiusSideward_ofEllipse()
        {
            return ScaleInputFloat_accordingToSizeDefinition(radiusSideward_ofEllipse_scaleFactor_relToScreen, radiusSideward_ofEllipse_scaleFactor);
        }

        float Get_radiusUpward_ofEllipse()
        {
            return ScaleInputFloat_accordingToSizeDefinition(radiusUpward_ofEllipse_scaleFactor_relToScreen, radiusUpward_ofEllipse_scaleFactor);
        }

        float Get_outerRadiusOfStars()
        {
            return ScaleInputFloat_accordingToSizeDefinition(outerRadiusOfStars_scaleFactor_relToScreen, outerRadiusOfStars_scaleFactor);
        }

        float Get_widthOfPlane()
        {
            return ScaleInputFloat_accordingToSizeDefinition(widthOfPlane_scaleFactor_relToScreen, widthOfPlane_scaleFactor);
        }

        float Get_lengthOfPlane()
        {
            return ScaleInputFloat_accordingToSizeDefinition(lengthOfPlane_scaleFactor_relToScreen, lengthOfPlane_scaleFactor);
        }

        float Get_linesWidth()
        {
            return ScaleInputFloat_accordingToSizeDefinition(linesWidth_relToScreen, linesWidth);
        }

        float Get_linesWidthOfCubeFillLines()
        {
            return ScaleInputFloat_accordingToSizeDefinition(linesWidthOfCubeFillLines_relToScreen, linesWidthOfCubeFillLines);
        }

        float Get_stylePatternScaleFactor()
        {
            float stylePatternScaleFactor_unclamped = ScaleInputFloat_accordingToSizeDefinition(stylePatternScaleFactor_relToScreen, stylePatternScaleFactor);
            return Mathf.Max(stylePatternScaleFactor_unclamped, UtilitiesDXXL_LineStyles.minStylePatternScaleFactor);
        }

        float Get_heightOfCapsule3D()
        {
            return ScaleInputFloat_accordingToSizeDefinition(heightOfCapsule3D_scaleFactor_relToScreen, heightOfCapsule3D_scaleFactor);
        }

        float Get_widthOfCapsule2D()
        {
            return ScaleInputFloat_accordingToSizeDefinition(widthOfCapsule2D_scaleFactor_relToScreen, widthOfCapsule2D_scaleFactor);
        }

        float Get_heightOfCapsule2D()
        {
            return ScaleInputFloat_accordingToSizeDefinition(heightOfCapsule2D_scaleFactor_relToScreen, heightOfCapsule2D_scaleFactor);
        }

        float ScaleInputFloat_accordingToSizeDefinition(float inputFloatToScale_versionThatIsRelToScreen, float inputFloatToScale)
        {
            switch (sizeDefinition)
            {
                case ShapeSizeDefinition.relativeToTheGlobalScaleOfTheTransformRespectivelyItsBiggestAbsoluteComponent:
                    return biggestAbsGlobalSizeComponentOfTransform * inputFloatToScale;
                case ShapeSizeDefinition.absoluteUnits:
                    return inputFloatToScale;
                case ShapeSizeDefinition.relativeToTheSceneViewWindowSize:
                    if (cameraForSizeDefinitionIsAvailable)
                    {
                        return lengthOfScreenDiagonal_atDrawnObjectsPosition * inputFloatToScale_versionThatIsRelToScreen;
                    }
                    else
                    {
                        return inputFloatToScale;
                    }
                case ShapeSizeDefinition.relativeToTheGameViewWindowSize:
                    if (cameraForSizeDefinitionIsAvailable)
                    {
                        return lengthOfScreenDiagonal_atDrawnObjectsPosition * inputFloatToScale_versionThatIsRelToScreen;
                    }
                    else
                    {
                        return inputFloatToScale;
                    }
                default:
                    return inputFloatToScale;
            }
        }

        float forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_before;
        float forcedConstantWorldspaceTextSize_forTextAtShapes_before;
        DrawBasics.CameraForAutomaticOrientation cameraForAutomaticOrientation_before;
        DrawText.AutomaticTextOrientation automaticTextOrientation_before;
        void Set_globalTextSizeSpecs_reversible()
        {
            forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_before = DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes;
            forcedConstantWorldspaceTextSize_forTextAtShapes_before = DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes;
            cameraForAutomaticOrientation_before = DrawBasics.cameraForAutomaticOrientation;
            automaticTextOrientation_before = DrawText.automaticTextOrientation;

            if (sizeDefinition == ShapeSizeDefinition.relativeToTheSceneViewWindowSize)
            {
                DrawBasics.cameraForAutomaticOrientation = DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;
                DrawText.automaticTextOrientation = DrawText.AutomaticTextOrientation.screen;
            }
            else
            {
                if (sizeDefinition == ShapeSizeDefinition.relativeToTheGameViewWindowSize)
                {
                    DrawBasics.cameraForAutomaticOrientation = DrawBasics.CameraForAutomaticOrientation.gameViewCamera;
                    DrawText.automaticTextOrientation = DrawText.AutomaticTextOrientation.screen;
                }
                else
                {
                    switch (shapeAttachedTextsizeReferenceContext)
                    {
                        case ShapeAttachedTextsizeReferenceContext.sizeOfShape:
                            DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = 0.0f;
                            DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = 0.0f;
                            break;
                        case ShapeAttachedTextsizeReferenceContext.globalSpace:
                            DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = 0.0f;
                            DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = textSize_value;
                            break;
                        case ShapeAttachedTextsizeReferenceContext.sceneViewWindowSize:
                            DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = textSize_value_relToScreen;
                            DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = 0.0f;
                            DrawBasics.cameraForAutomaticOrientation = DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;
                            DrawText.automaticTextOrientation = DrawText.AutomaticTextOrientation.screen;
                            break;
                        case ShapeAttachedTextsizeReferenceContext.gameViewWindowSize:
                            DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = textSize_value_relToScreen;
                            DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = 0.0f;
                            DrawBasics.cameraForAutomaticOrientation = DrawBasics.CameraForAutomaticOrientation.gameViewCamera;
                            DrawText.automaticTextOrientation = DrawText.AutomaticTextOrientation.screen;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        void Reverse_globalTextSizeSpecs()
        {
            DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_before;
            DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = forcedConstantWorldspaceTextSize_forTextAtShapes_before;
            DrawBasics.cameraForAutomaticOrientation = cameraForAutomaticOrientation_before;
            DrawText.automaticTextOrientation = automaticTextOrientation_before;
        }

        public static DrawShapes.Shape2DType Get_shape2DType_forIrregularStar(CornerOptionsForIrregularStar cornerOptionsForIrregularStar)
        {
            switch (cornerOptionsForIrregularStar)
            {
                case CornerOptionsForIrregularStar._3:
                    return DrawShapes.Shape2DType.star3;
                case CornerOptionsForIrregularStar._4:
                    return DrawShapes.Shape2DType.star4;
                case CornerOptionsForIrregularStar._5:
                    return DrawShapes.Shape2DType.star5;
                case CornerOptionsForIrregularStar._6:
                    return DrawShapes.Shape2DType.star6;
                case CornerOptionsForIrregularStar._8:
                    return DrawShapes.Shape2DType.star8;
                case CornerOptionsForIrregularStar._10:
                    return DrawShapes.Shape2DType.star10;
                case CornerOptionsForIrregularStar._16:
                    return DrawShapes.Shape2DType.star16;
                case CornerOptionsForIrregularStar._32:
                    return DrawShapes.Shape2DType.star32;
                case CornerOptionsForIrregularStar._64:
                    return DrawShapes.Shape2DType.star64;
                default:
                    return DrawShapes.Shape2DType.star5;
            }
        }

        public static int Map_sphereQuality_to_linesPerSphereCircle(SphereQuality sphereQuality_toMap)
        {
            switch (sphereQuality_toMap)
            {
                case SphereQuality._64_linesPerSphereCircle:
                    return 64;
                case SphereQuality._32_linesPerSphereCircle:
                    return 32;
                case SphereQuality._16_linesPerSphereCircle:
                    return 16;
                case SphereQuality._8_linesPerSphereCircle:
                    return 8;
                default:
                    return 64;
            }
        }

        public static SphereQuality Map_linesPerSphereCircle_to_sphereQuality(int linesPerSphereCircle_toMap)
        {
            switch (linesPerSphereCircle_toMap)
            {
                case 64:
                    return SphereQuality._64_linesPerSphereCircle;
                case 32:
                    return SphereQuality._32_linesPerSphereCircle;
                case 16:
                    return SphereQuality._16_linesPerSphereCircle;
                case 8:
                    return SphereQuality._8_linesPerSphereCircle;
                default:
                    Debug.LogError("'linesPerSphereCircle' has to be 64, 32, 16, or 8.");
                    return SphereQuality._64_linesPerSphereCircle;
            }
        }

    }

}
