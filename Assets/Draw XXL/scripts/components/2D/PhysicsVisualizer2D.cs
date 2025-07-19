namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/2D/Physics Visualizer 2D")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class PhysicsVisualizer2D : VisualizerParent
    {
        public enum Shape { collidersOnThisGameobject, box, circle, capsule, rayOrPoint, ray3D };
        [SerializeField] Shape shape = Shape.collidersOnThisGameobject;

        public enum CollisionType { cast, overlap };
        [SerializeField] CollisionType collisionType = CollisionType.cast;

        public enum WantedHits { onlyFirst, all };
        [SerializeField] WantedHits wantedHits = WantedHits.all;

        [SerializeField] bool distanceIsInfinityRespToOtherGO = true;
        [SerializeField] float adjustedDistance = 10.0f;
        float used_distance;

        [SerializeField] Vector2 sizeScaleFactors_ofCastRespCheckedBox = Vector2.one;
        [SerializeField] float radiusScaleFactor_ofCastRespCheckedCircle = 0.5f;
        [SerializeField] Vector2 sizeScaleFactors_ofCastRespCheckedCapsule = new Vector2(0.5f, 1.0f);
        [SerializeField] CapsuleDirection2D capsuleDirection2D_ofManuallyConstructedCapsuleMeaningNotFromCollider = CapsuleDirection2D.Vertical;
        public enum ShapeRotationType { transformsRotationPlusOptionalAdditionalRotation, customRotationIndependentFromTransform };
        [SerializeField] public ShapeRotationType shapeRotationType = ShapeRotationType.transformsRotationPlusOptionalAdditionalRotation;

        [SerializeField] [Range(0.0f, 360.0f)] float rotationAngleOfShape_additionallyToTransformsAngle = 0.0f;

        BoxCollider2D[] boxColliders2D_onThisGameobject;
        CircleCollider2D[] circleColliders2D_onThisGameobject;
        CapsuleCollider2D[] capsuleColliders2D_onThisGameobject;
        [SerializeField] public bool theGameobjectHasACompatibleAndEnabledCollider; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        List<RaycastHit2D> unusedRaycastHitResults = new List<RaycastHit2D>();
        List<Collider2D> unusedResultColliders = new List<Collider2D>();

        [SerializeField] public bool otherSettings_isFoldedOut = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.

        [SerializeField] bool excludeCollidersOnThisGO = true;
        [SerializeField] bool excludeCollidersOnParentGOs = true;
        [SerializeField] bool excludeCollidersOnChildrenGOs = true;

        Collider2D[] allColliders2DOnThisGameobject;
        List<bool> enabledState_ofAllColliders2DOnThisGO_beforeCurrentDrawOperation = new List<bool>();
        [SerializeField] public bool aVisualizedBoxCollider2DOnThisComponent_hasANonZeroEdge; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool aVisualizedBoxCollider2DOnThisComponent_hasAutoTiling; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.

        Collider2D[] allColliders2DOnThisGameobjectAndOnParents;
        List<bool> enabledState_ofAllColliders2DOnThisGOPlusParents_beforeCurrentDrawOperation = new List<bool>();
        List<bool> isOnThisGO_forAllColliders2DOnThisGOPlusParents_beforeCurrentDrawOperation = new List<bool>();

        Collider2D[] allColliders2DOnThisGameobjectAndOnChildren;
        List<bool> enabledState_ofAllColliders2DOnThisGOPlusChildren_beforeCurrentDrawOperation = new List<bool>();
        List<bool> isOnThisGO_forAllColliders2DOnThisGOPlusChildren_beforeCurrentDrawOperation = new List<bool>();

        [SerializeField] LayerMask layerMask = Physics2D.DefaultRaycastLayers; //The "Physics2D.DefaultRaycastLayers" already inludes the layer slots that are not yet defined by the users. That means this doesn't have to updated in cases where a user adds a custom defined layer AFTER the creation of this component

        [SerializeField] bool useDepth = false;
        [SerializeField] float minDepth = Mathf.NegativeInfinity;
        [SerializeField] float maxDepth = Mathf.Infinity;
        [SerializeField] bool useOutsideDepth = false;

        [SerializeField] bool useNormalAngle = false;
        [SerializeField] [Range(0.0f, 360.0f)] float minNormalAngle = 0.0f;
        [SerializeField] [Range(0.0f, 360.0f)] float maxNormalAngle = 360.0f;
        [SerializeField] bool useOutsideNormalAngle = false;

        [SerializeField] bool useTriggers = true;
        [SerializeField] int numberOfFoundHits = 0;

        //DrawPhysics2D' class global settings:
        [SerializeField] Color colorForNonHittingCasts = DrawPhysics2D.colorForNonHittingCasts;
        [SerializeField] Color colorForHittingCasts = DrawPhysics2D.colorForHittingCasts;
        [SerializeField] Color colorForCastLineBeyondHit = DrawPhysics2D.colorForCastLineBeyondHit;
        [SerializeField] Color colorForCastsHitText = DrawPhysics2D.colorForCastsHitText;
        [SerializeField] bool doOverwriteColorForCastsHitNormals = false;
        [SerializeField] Color overwriteColorForCastsHitNormals = UtilitiesDXXL_Physics2D.Get_defaultColor_ofNormal(); //-> not using "DrawPhysics2D.overwriteColorForCastsHitNormals", since this would be the default color that doesn't represent what the user sees as normal color in the Scene
        [SerializeField] float scaleFactor_forCastHitTextSize = DrawPhysics2D.scaleFactor_forCastHitTextSize;
        [SerializeField] float castCorridorVisualizerDensity = DrawPhysics2D.castCorridorVisualizerDensity;
        [SerializeField] bool drawCastNameTag_atCastOrigin = DrawPhysics2D.drawCastNameTag_atCastOrigin;
        [SerializeField] bool drawCastNameTag_atHitPositions = DrawPhysics2D.drawCastNameTag_atHitPositions;
        [SerializeField] int maxListedColliders_inOverlapVolumesTextList = DrawPhysics2D.MaxListedColliders_inOverlapVolumesTextList;
        [SerializeField] int maxOverlapingCollidersWithUntruncatedText = DrawPhysics2D.maxOverlapingCollidersWithUntruncatedText;
        [SerializeField] [Range(0.001f, 0.2f)] float forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts = 0.01f;
        [SerializeField] float forcedConstantWorldspaceTextSize_forOverlapResultTexts = 0.1f;
        [SerializeField] PhysicsVisualizer.SaveDrawnLinesType saveDrawnLinesType = PhysicsVisualizer.Map_visualizationQuality_to_saveDrawnLinesType(DrawPhysics2D.visualizationQuality);
        [SerializeField] PhysicsVisualizer.OverlapResultTextSizeInterpretation overlapResultTextSizeInterpretation = PhysicsVisualizer.OverlapResultTextSizeInterpretation.relativeToTheSizeOfTheOverlapingPhysicsShape;
        [SerializeField] bool useCustomDirectionForHitResultText = false;
        [SerializeField] Vector2 customDirectionForHitResultText = GetInitialValueFor_customDirectionForHitResultText();

        Color colorForNonHittingCasts_before;
        Color colorForHittingCasts_before;
        Color colorForCastLineBeyondHit_before;
        Color colorForCastsHitText_before;
        Color overwriteColorForCastsHitNormals_before;
        float scaleFactor_forCastHitTextSize_before;
        float castCorridorVisualizerDensity_before;
        DrawPhysics.VisualizationQuality visualizationQuality_before;
        bool drawCastNameTag_atCastOrigin_before;
        bool drawCastNameTag_atHitPositions_before;
        int maxListedColliders_inOverlapVolumesTextList_before;
        int maxOverlapingCollidersWithUntruncatedText_before;
        float forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts_before;
        float forcedConstantWorldspaceTextSize_forOverlapResultTexts_before;
        DrawBasics.CameraForAutomaticOrientation cameraForAutomaticOrientation_before;
        float custom_zPos_forCastVisualisation_before;
        Vector2 directionOfHitResultText_before;

        public override void InitializeValues_onceInComponentLifetime()
        {
            if (text_exclGlobalMarkupTags == null || text_exclGlobalMarkupTags == "")
            {
                text_exclGlobalMarkupTags = "Collisions2D from " + this.gameObject.name;
                text_inclGlobalMarkupTags = "Collisions2D from " + this.gameObject.name;
            }

            customVector3_1_picker_isOutfolded = true;
            source_ofCustomVector3_1 = CustomVector3Source.manualInput;
            customVector3_1_clipboardForManualInput = Vector3.one;
            vectorInterpretation_ofCustomVector3_1 = VectorInterpretation.globalSpace;

            customVector2_1_picker_isOutfolded = true;
            source_ofCustomVector2_1 = CustomVector2Source.rotationAroundZStartingFromRight;
            customVector2_1_clipboardForManualInput = Vector2.one;
            vectorInterpretation_ofCustomVector2_1 = VectorInterpretation.globalSpace;
        }

        public override void InitializeValues_alsoOnPlaymodeEnter_andOnComponentCreatedAsCopy()
        {
            if (shape == Shape.collidersOnThisGameobject)
            {
                if (CheckIfGameobjectHasASupportedCollider() == false)
                {
                    shape = Shape.box;
                }
            }
        }

        public override void DrawVisualizedObject()
        {
#if UNITY_EDITOR
            TryFetchCurrentEnabledStateOfColliders2DOnThisGameobjectsHierarchy();
            Save_globalDrawPhysics2DOptions_beforeThisDrawOperation();
            try
            {
                Set_globalDrawPhysics2DOptions_toValuesFromInpsector();
                TryDisableEnabledStateOfColliders2DOnThisGameobjectsHierarchy();
                CastRespCheckTheColliders2D_andDrawThem();
            }
            catch { }
            TryRestoreEnabledStateOfColliders2DOnThisGameobjectsHierarchy();
            Restore_globalDrawPhysics2DOptions_toValuesFromBefore();
#endif
        }

        bool CheckIfGameobjectHasASupportedCollider()
        {
            if ((this.gameObject.GetComponent<BoxCollider2D>() == null) && (this.gameObject.GetComponent<CircleCollider2D>() == null) && (this.gameObject.GetComponent<CapsuleCollider2D>() == null))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        void TryFetchCurrentEnabledStateOfColliders2DOnThisGameobjectsHierarchy()
        {
            if (excludeCollidersOnThisGO || excludeCollidersOnParentGOs || excludeCollidersOnChildrenGOs) //The other flag's threads need "allCollidersOnThisGameobject"
            {
                allColliders2DOnThisGameobject = this.gameObject.GetComponents<Collider2D>();
                if (allColliders2DOnThisGameobject != null)
                {
                    for (int i = 0; i < allColliders2DOnThisGameobject.Length; i++)
                    {
                        if (allColliders2DOnThisGameobject[i] != null)
                        {
                            UtilitiesDXXL_List.AddToABoolList(ref enabledState_ofAllColliders2DOnThisGO_beforeCurrentDrawOperation, allColliders2DOnThisGameobject[i].enabled, i);
                        }
                    }
                }
            }

            if (excludeCollidersOnParentGOs)
            {
                allColliders2DOnThisGameobjectAndOnParents = this.gameObject.GetComponentsInParent<Collider2D>();
                if (allColliders2DOnThisGameobjectAndOnParents != null)
                {
                    for (int i = 0; i < allColliders2DOnThisGameobjectAndOnParents.Length; i++)
                    {
                        if (allColliders2DOnThisGameobjectAndOnParents[i] != null)
                        {
                            UtilitiesDXXL_List.AddToABoolList(ref enabledState_ofAllColliders2DOnThisGOPlusParents_beforeCurrentDrawOperation, allColliders2DOnThisGameobjectAndOnParents[i].enabled, i);
                            UtilitiesDXXL_List.AddToABoolList(ref isOnThisGO_forAllColliders2DOnThisGOPlusParents_beforeCurrentDrawOperation, CheckIfCollider2DIsOnThisGameobject(allColliders2DOnThisGameobjectAndOnParents[i]), i);
                        }
                    }
                }
            }

            if (excludeCollidersOnChildrenGOs)
            {
                allColliders2DOnThisGameobjectAndOnChildren = this.gameObject.GetComponentsInChildren<Collider2D>();

                if (allColliders2DOnThisGameobjectAndOnChildren != null)
                {
                    for (int i = 0; i < allColliders2DOnThisGameobjectAndOnChildren.Length; i++)
                    {
                        if (allColliders2DOnThisGameobjectAndOnChildren[i] != null)
                        {
                            UtilitiesDXXL_List.AddToABoolList(ref enabledState_ofAllColliders2DOnThisGOPlusChildren_beforeCurrentDrawOperation, allColliders2DOnThisGameobjectAndOnChildren[i].enabled, i);
                            UtilitiesDXXL_List.AddToABoolList(ref isOnThisGO_forAllColliders2DOnThisGOPlusChildren_beforeCurrentDrawOperation, CheckIfCollider2DIsOnThisGameobject(allColliders2DOnThisGameobjectAndOnChildren[i]), i);
                        }
                    }
                }
            }
        }

        bool CheckIfCollider2DIsOnThisGameobject(Collider2D collider2D_toCheckIfItIsOnThisGameobject)
        {
            if (collider2D_toCheckIfItIsOnThisGameobject != null)
            {
                if (allColliders2DOnThisGameobject != null)
                {
                    for (int i = 0; i < allColliders2DOnThisGameobject.Length; i++)
                    {
                        if (allColliders2DOnThisGameobject[i] != null)
                        {
                            if (allColliders2DOnThisGameobject[i] == collider2D_toCheckIfItIsOnThisGameobject)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        bool GetEnabledStateBeforeCurrentDrawOperation_ofCollider2DComponentOnThisGameobject(Collider2D collider2DD_toRetrieveEnabledStateFor)
        {
            if (collider2DD_toRetrieveEnabledStateFor != null)
            {
                if (allColliders2DOnThisGameobject != null)
                {
                    for (int i = 0; i < allColliders2DOnThisGameobject.Length; i++)
                    {
                        if (allColliders2DOnThisGameobject[i] != null)
                        {
                            if (allColliders2DOnThisGameobject[i] == collider2DD_toRetrieveEnabledStateFor)
                            {
                                return enabledState_ofAllColliders2DOnThisGO_beforeCurrentDrawOperation[i];
                            }
                        }
                    }
                }
            }
            return false;
        }

        void TryDisableEnabledStateOfColliders2DOnThisGameobjectsHierarchy()
        {
            if (excludeCollidersOnThisGO)
            {
                if (allColliders2DOnThisGameobject != null)
                {
                    for (int i = 0; i < allColliders2DOnThisGameobject.Length; i++)
                    {
                        if (allColliders2DOnThisGameobject[i] != null)
                        {
                            allColliders2DOnThisGameobject[i].enabled = false;
                        }
                    }
                }
            }

            if (excludeCollidersOnParentGOs)
            {
                if (allColliders2DOnThisGameobjectAndOnParents != null)
                {
                    for (int i = 0; i < allColliders2DOnThisGameobjectAndOnParents.Length; i++)
                    {
                        if (allColliders2DOnThisGameobjectAndOnParents[i] != null)
                        {
                            if (isOnThisGO_forAllColliders2DOnThisGOPlusParents_beforeCurrentDrawOperation[i] == false)
                            {
                                allColliders2DOnThisGameobjectAndOnParents[i].enabled = false;
                            }
                        }
                    }
                }
            }

            if (excludeCollidersOnChildrenGOs)
            {
                if (allColliders2DOnThisGameobjectAndOnChildren != null)
                {
                    for (int i = 0; i < allColliders2DOnThisGameobjectAndOnChildren.Length; i++)
                    {
                        if (allColliders2DOnThisGameobjectAndOnChildren[i] != null)
                        {
                            if (isOnThisGO_forAllColliders2DOnThisGOPlusChildren_beforeCurrentDrawOperation[i] == false)
                            {
                                allColliders2DOnThisGameobjectAndOnChildren[i].enabled = false;
                            }
                        }
                    }
                }
            }
        }

        void TryRestoreEnabledStateOfColliders2DOnThisGameobjectsHierarchy()
        {
            if (excludeCollidersOnThisGO)
            {
                if (allColliders2DOnThisGameobject != null)
                {
                    for (int i = 0; i < allColliders2DOnThisGameobject.Length; i++)
                    {
                        if (allColliders2DOnThisGameobject[i] != null)
                        {
                            allColliders2DOnThisGameobject[i].enabled = enabledState_ofAllColliders2DOnThisGO_beforeCurrentDrawOperation[i];
                        }
                    }
                }
            }

            if (excludeCollidersOnParentGOs)
            {
                if (allColliders2DOnThisGameobjectAndOnParents != null)
                {
                    for (int i = 0; i < allColliders2DOnThisGameobjectAndOnParents.Length; i++)
                    {
                        if (allColliders2DOnThisGameobjectAndOnParents[i] != null)
                        {
                            if (isOnThisGO_forAllColliders2DOnThisGOPlusParents_beforeCurrentDrawOperation[i] == false)
                            {
                                allColliders2DOnThisGameobjectAndOnParents[i].enabled = enabledState_ofAllColliders2DOnThisGOPlusParents_beforeCurrentDrawOperation[i];
                            }
                        }
                    }
                }
            }

            if (excludeCollidersOnChildrenGOs)
            {
                if (allColliders2DOnThisGameobjectAndOnChildren != null)
                {
                    for (int i = 0; i < allColliders2DOnThisGameobjectAndOnChildren.Length; i++)
                    {
                        if (allColliders2DOnThisGameobjectAndOnChildren[i] != null)
                        {
                            if (isOnThisGO_forAllColliders2DOnThisGOPlusChildren_beforeCurrentDrawOperation[i] == false)
                            {
                                allColliders2DOnThisGameobjectAndOnChildren[i].enabled = enabledState_ofAllColliders2DOnThisGOPlusChildren_beforeCurrentDrawOperation[i];
                            }
                        }
                    }
                }
            }
        }

        void Save_globalDrawPhysics2DOptions_beforeThisDrawOperation()
        {
            colorForNonHittingCasts_before = DrawPhysics2D.colorForNonHittingCasts;
            colorForHittingCasts_before = DrawPhysics2D.colorForHittingCasts;
            colorForCastLineBeyondHit_before = DrawPhysics2D.colorForCastLineBeyondHit;
            colorForCastsHitText_before = DrawPhysics2D.colorForCastsHitText;
            scaleFactor_forCastHitTextSize_before = DrawPhysics2D.scaleFactor_forCastHitTextSize;
            castCorridorVisualizerDensity_before = DrawPhysics2D.castCorridorVisualizerDensity;
            visualizationQuality_before = DrawPhysics2D.visualizationQuality;
            drawCastNameTag_atCastOrigin_before = DrawPhysics2D.drawCastNameTag_atCastOrigin;
            drawCastNameTag_atHitPositions_before = DrawPhysics2D.drawCastNameTag_atHitPositions;
            custom_zPos_forCastVisualisation_before = DrawPhysics2D.custom_zPos_forCastVisualisation;
            maxListedColliders_inOverlapVolumesTextList_before = DrawPhysics2D.MaxListedColliders_inOverlapVolumesTextList;
            maxOverlapingCollidersWithUntruncatedText_before = DrawPhysics2D.maxOverlapingCollidersWithUntruncatedText;
            forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts_before = DrawPhysics2D.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts;
            forcedConstantWorldspaceTextSize_forOverlapResultTexts_before = DrawPhysics2D.forcedConstantWorldspaceTextSize_forOverlapResultTexts;
            cameraForAutomaticOrientation_before = DrawBasics.cameraForAutomaticOrientation;

            if (doOverwriteColorForCastsHitNormals)
            {
                overwriteColorForCastsHitNormals_before = DrawPhysics2D.overwriteColorForCastsHitNormals;
            }

            if (useCustomDirectionForHitResultText)
            {
                directionOfHitResultText_before = DrawPhysics2D.directionOfHitResultText;
            }
        }

        void Set_globalDrawPhysics2DOptions_toValuesFromInpsector()
        {
            DrawPhysics2D.colorForNonHittingCasts = colorForNonHittingCasts;
            DrawPhysics2D.colorForHittingCasts = colorForHittingCasts;
            DrawPhysics2D.colorForCastLineBeyondHit = colorForCastLineBeyondHit;
            DrawPhysics2D.colorForCastsHitText = colorForCastsHitText;
            DrawPhysics2D.scaleFactor_forCastHitTextSize = scaleFactor_forCastHitTextSize;
            DrawPhysics2D.castCorridorVisualizerDensity = castCorridorVisualizerDensity;
            DrawPhysics2D.visualizationQuality = PhysicsVisualizer.Map_saveDrawnLinesType_to_visualizationQuality(saveDrawnLinesType);
            DrawPhysics2D.drawCastNameTag_atCastOrigin = drawCastNameTag_atCastOrigin;
            DrawPhysics2D.drawCastNameTag_atHitPositions = drawCastNameTag_atHitPositions;
            DrawPhysics2D.custom_zPos_forCastVisualisation = GetZPos_global_for2D();
            DrawPhysics2D.MaxListedColliders_inOverlapVolumesTextList = maxListedColliders_inOverlapVolumesTextList;
            DrawPhysics2D.maxOverlapingCollidersWithUntruncatedText = maxOverlapingCollidersWithUntruncatedText;

            switch (overlapResultTextSizeInterpretation)
            {
                case PhysicsVisualizer.OverlapResultTextSizeInterpretation.relativeToTheSizeOfTheOverlapingPhysicsShape:
                    DrawPhysics2D.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts = 0.0f;
                    DrawPhysics2D.forcedConstantWorldspaceTextSize_forOverlapResultTexts = 0.0f;
                    break;
                case PhysicsVisualizer.OverlapResultTextSizeInterpretation.fixedWorldSpaceSize:
                    DrawPhysics2D.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts = 0.0f;
                    DrawPhysics2D.forcedConstantWorldspaceTextSize_forOverlapResultTexts = forcedConstantWorldspaceTextSize_forOverlapResultTexts;
                    break;
                case PhysicsVisualizer.OverlapResultTextSizeInterpretation.relativeToTheSceneViewWindowSize:
                    DrawPhysics2D.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts = forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts;
                    DrawPhysics2D.forcedConstantWorldspaceTextSize_forOverlapResultTexts = 0.0f;
                    DrawBasics.cameraForAutomaticOrientation = DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;
                    break;
                case PhysicsVisualizer.OverlapResultTextSizeInterpretation.relativeToTheGameViewWindowSize:
                    DrawPhysics2D.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts = forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts;
                    DrawPhysics2D.forcedConstantWorldspaceTextSize_forOverlapResultTexts = 0.0f;
                    DrawBasics.cameraForAutomaticOrientation = DrawBasics.CameraForAutomaticOrientation.gameViewCamera;
                    break;
                default:
                    break;
            }

            if (doOverwriteColorForCastsHitNormals)
            {
                DrawPhysics2D.overwriteColorForCastsHitNormals = overwriteColorForCastsHitNormals;
            }

            if (useCustomDirectionForHitResultText)
            {
                DrawPhysics2D.directionOfHitResultText = customDirectionForHitResultText;
            }
        }

        void Restore_globalDrawPhysics2DOptions_toValuesFromBefore()
        {
            DrawPhysics2D.colorForNonHittingCasts = colorForNonHittingCasts_before;
            DrawPhysics2D.colorForHittingCasts = colorForHittingCasts_before;
            DrawPhysics2D.colorForCastLineBeyondHit = colorForCastLineBeyondHit_before;
            DrawPhysics2D.colorForCastsHitText = colorForCastsHitText_before;
            DrawPhysics2D.scaleFactor_forCastHitTextSize = scaleFactor_forCastHitTextSize_before;
            DrawPhysics2D.castCorridorVisualizerDensity = castCorridorVisualizerDensity_before;
            DrawPhysics2D.visualizationQuality = visualizationQuality_before;
            DrawPhysics2D.drawCastNameTag_atCastOrigin = drawCastNameTag_atCastOrigin_before;
            DrawPhysics2D.drawCastNameTag_atHitPositions = drawCastNameTag_atHitPositions_before;
            DrawPhysics2D.custom_zPos_forCastVisualisation = custom_zPos_forCastVisualisation_before;
            DrawPhysics2D.MaxListedColliders_inOverlapVolumesTextList = maxListedColliders_inOverlapVolumesTextList_before;
            DrawPhysics2D.maxOverlapingCollidersWithUntruncatedText = maxOverlapingCollidersWithUntruncatedText_before;
            DrawPhysics2D.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts = forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts_before;
            DrawPhysics2D.forcedConstantWorldspaceTextSize_forOverlapResultTexts = forcedConstantWorldspaceTextSize_forOverlapResultTexts_before;
            DrawPhysics2D.forcedConstantWorldspaceTextSize_forOverlapResultTexts = forcedConstantWorldspaceTextSize_forOverlapResultTexts_before;
            DrawBasics.cameraForAutomaticOrientation = cameraForAutomaticOrientation_before;

            if (doOverwriteColorForCastsHitNormals)
            {
                DrawPhysics2D.overwriteColorForCastsHitNormals = overwriteColorForCastsHitNormals_before;
            }

            if (useCustomDirectionForHitResultText)
            {
                DrawPhysics2D.directionOfHitResultText = directionOfHitResultText_before;
            }
        }

        void CastRespCheckTheColliders2D_andDrawThem()
        {
            used_distance = Get_used_distance();
            switch (shape)
            {
                case Shape.collidersOnThisGameobject:
                    theGameobjectHasACompatibleAndEnabledCollider = false;
                    aVisualizedBoxCollider2DOnThisComponent_hasANonZeroEdge = false;
                    aVisualizedBoxCollider2DOnThisComponent_hasAutoTiling = false;
                    CastRespOverlap_boxColliders2DOnThisGameobject();
                    CastRespOverlap_circleColliders2DOnThisGameobject();
                    CastRespOverlap_capsuleColliders2DOnThisGameobject();
                    break;
                case Shape.box:
                    switch (collisionType)
                    {
                        case CollisionType.cast:
                            BoxCast2D(GetDrawPos2D_global(), Get_sizeOfFinalBoxShape(sizeScaleFactors_ofCastRespCheckedBox), GetUsedRotation());
                            break;
                        case CollisionType.overlap:
                            Box2D_checkRespOverlap(GetDrawPos2D_global(), Get_sizeOfFinalBoxShape(sizeScaleFactors_ofCastRespCheckedBox), GetUsedRotation());
                            break;
                        default:
                            break;
                    }
                    break;
                case Shape.circle:
                    switch (collisionType)
                    {
                        case CollisionType.cast:
                            CircleCast2D(GetDrawPos2D_global(), GetRadiusOfFinalCircleShape(radiusScaleFactor_ofCastRespCheckedCircle));
                            break;
                        case CollisionType.overlap:
                            Circle2D_checkRespOverlap(GetDrawPos2D_global(), GetRadiusOfFinalCircleShape(radiusScaleFactor_ofCastRespCheckedCircle));
                            break;
                        default:
                            break;
                    }
                    break;
                case Shape.capsule:
                    switch (collisionType)
                    {
                        case CollisionType.cast:
                            CapsuleCast2D(GetDrawPos2D_global(), Get_sizeOfFinalBoxShape(sizeScaleFactors_ofCastRespCheckedCapsule), capsuleDirection2D_ofManuallyConstructedCapsuleMeaningNotFromCollider, GetUsedRotation());
                            break;
                        case CollisionType.overlap:
                            Capsule2D_checkRespOverlap(GetDrawPos2D_global(), Get_sizeOfFinalBoxShape(sizeScaleFactors_ofCastRespCheckedCapsule), capsuleDirection2D_ofManuallyConstructedCapsuleMeaningNotFromCollider, GetUsedRotation());
                            break;
                        default:
                            break;
                    }
                    break;
                case Shape.rayOrPoint:
                    switch (collisionType)
                    {
                        case CollisionType.cast:
                            RayCast2D();
                            break;
                        case CollisionType.overlap:
                            Point2D_checkRespOverlap();
                            break;
                        default:
                            break;
                    }
                    break;
                case Shape.ray3D:
                    Ray3DCastIn2D();
                    break;
                default:
                    break;
            }
        }

        float Get_used_distance()
        {
            if (distanceIsInfinityRespToOtherGO)
            {
                if (shape == Shape.ray3D)
                {
                    if (source_ofCustomVector3_1 == VisualizerParent.CustomVector3Source.toOtherGameobject)
                    {
                        if (customVector3_1_targetGameObject != null)
                        {
                            return (transform.position - customVector3_1_targetGameObject.transform.position).magnitude;
                        }
                        else
                        {
                            return Mathf.Infinity;
                        }
                    }
                    else
                    {
                        return Mathf.Infinity;
                    }
                }
                else
                {
                    if (source_ofCustomVector2_1 == VisualizerParent.CustomVector2Source.toOtherGameobject)
                    {
                        if (customVector2_1_targetGameObject != null)
                        {
                            Vector2 thisTransformPos_asV2 = new Vector2(transform.position.x, transform.position.y);
                            Vector2 otherGameobjectPos_asV2 = new Vector2(customVector2_1_targetGameObject.transform.position.x, customVector2_1_targetGameObject.transform.position.y);
                            return (thisTransformPos_asV2 - otherGameobjectPos_asV2).magnitude;
                        }
                        else
                        {
                            return Mathf.Infinity;
                        }
                    }
                    else
                    {
                        return Mathf.Infinity;
                    }
                }
            }
            else
            {
                return adjustedDistance;
            }
        }

        float GetUsedRotation()
        {
            switch (shapeRotationType)
            {
                case ShapeRotationType.transformsRotationPlusOptionalAdditionalRotation:
                    return (transform.rotation.eulerAngles.z + rotationAngleOfShape_additionallyToTransformsAngle);
                case ShapeRotationType.customRotationIndependentFromTransform:
                    return rotationAngleOfShape_additionallyToTransformsAngle;
                default:
                    return 0.0f;
            }
        }

        void CastRespOverlap_boxColliders2DOnThisGameobject()
        {
            boxColliders2D_onThisGameobject = this.gameObject.GetComponents<BoxCollider2D>();
            if (boxColliders2D_onThisGameobject != null)
            {
                for (int i = 0; i < boxColliders2D_onThisGameobject.Length; i++)
                {
                    if (boxColliders2D_onThisGameobject[i] != null)
                    {
                        if (boxColliders2D_onThisGameobject[i].enabled || GetEnabledStateBeforeCurrentDrawOperation_ofCollider2DComponentOnThisGameobject(boxColliders2D_onThisGameobject[i]))
                        {
                            theGameobjectHasACompatibleAndEnabledCollider = true; //this is intentionally outside the "autoTiling"-check
                            if (UtilitiesDXXL_Math.ApproximatelyZero(boxColliders2D_onThisGameobject[i].edgeRadius) == false) { aVisualizedBoxCollider2DOnThisComponent_hasANonZeroEdge = true; }

                            if (boxColliders2D_onThisGameobject[i].autoTiling == true)
                            {
                                aVisualizedBoxCollider2DOnThisComponent_hasAutoTiling = true;
                            }
                            else
                            {
                                Vector2 center = GetCenterPosGlobalOfCollider(boxColliders2D_onThisGameobject[i].offset);
                                Vector2 size = Get_sizeOfFinalBoxShape(boxColliders2D_onThisGameobject[i].size);
                                float angle = transform.rotation.eulerAngles.z;
                                switch (collisionType)
                                {
                                    case CollisionType.cast:
                                        BoxCast2D(center, size, angle);
                                        break;
                                    case CollisionType.overlap:
                                        Box2D_checkRespOverlap(center, size, angle);
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
            }
        }

        void CastRespOverlap_circleColliders2DOnThisGameobject()
        {
            circleColliders2D_onThisGameobject = this.gameObject.GetComponents<CircleCollider2D>();
            if (circleColliders2D_onThisGameobject != null)
            {
                for (int i = 0; i < circleColliders2D_onThisGameobject.Length; i++)
                {
                    if (circleColliders2D_onThisGameobject[i] != null)
                    {
                        if (circleColliders2D_onThisGameobject[i].enabled || GetEnabledStateBeforeCurrentDrawOperation_ofCollider2DComponentOnThisGameobject(circleColliders2D_onThisGameobject[i]))
                        {
                            theGameobjectHasACompatibleAndEnabledCollider = true;
                            Vector2 origin = GetCenterPosGlobalOfCollider(circleColliders2D_onThisGameobject[i].offset);
                            float radius = GetRadiusOfFinalCircleShape(circleColliders2D_onThisGameobject[i].radius);
                            switch (collisionType)
                            {
                                case CollisionType.cast:
                                    CircleCast2D(origin, radius);
                                    break;
                                case CollisionType.overlap:
                                    Circle2D_checkRespOverlap(origin, radius);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        void CastRespOverlap_capsuleColliders2DOnThisGameobject()
        {
            capsuleColliders2D_onThisGameobject = this.gameObject.GetComponents<CapsuleCollider2D>();
            if (capsuleColliders2D_onThisGameobject != null)
            {
                for (int i = 0; i < capsuleColliders2D_onThisGameobject.Length; i++)
                {
                    if (capsuleColliders2D_onThisGameobject[i] != null)
                    {
                        if (capsuleColliders2D_onThisGameobject[i].enabled || GetEnabledStateBeforeCurrentDrawOperation_ofCollider2DComponentOnThisGameobject(capsuleColliders2D_onThisGameobject[i]))
                        {
                            theGameobjectHasACompatibleAndEnabledCollider = true;
                            Vector2 colliderCenter_global = GetCenterPosGlobalOfCollider(capsuleColliders2D_onThisGameobject[i].offset);
                            Vector2 size = Get_sizeOfFinalBoxShape(capsuleColliders2D_onThisGameobject[i].size);
                            float angle = transform.rotation.eulerAngles.z;
                            switch (collisionType)
                            {
                                case CollisionType.cast:
                                    CapsuleCast2D(colliderCenter_global, size, capsuleColliders2D_onThisGameobject[i].direction, angle);
                                    break;
                                case CollisionType.overlap:
                                    Capsule2D_checkRespOverlap(colliderCenter_global, size, capsuleColliders2D_onThisGameobject[i].direction, angle);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        float GetRadiusOfFinalCircleShape(float radiusScaleFactor_fromComponentProperty)
        {
            return (radiusScaleFactor_fromComponentProperty * UtilitiesDXXL_Math.GetBiggestAbsComponent_butReassignTheSign(transform.lossyScale, UtilitiesDXXL_Math.Dimension.z));
        }

        Vector2 Get_sizeOfFinalBoxShape(Vector2 sizeScaleFactors_fromComponentProperty)
        {
            Vector2 transformsLossyScale_asV2 = new Vector2(transform.lossyScale.x, transform.lossyScale.y);
            return Vector2.Scale(transformsLossyScale_asV2, sizeScaleFactors_fromComponentProperty);
        }

        Vector2 GetCenterPosGlobalOfCollider(Vector2 collidersLocalCenter)
        {
            Vector3 xOffset = transform.right * transform.lossyScale.x * collidersLocalCenter.x;
            Vector3 yOffset = transform.up * transform.lossyScale.y * collidersLocalCenter.y;
            return (transform.position + xOffset + yOffset);
        }

        void BoxCast2D(Vector2 origin, Vector2 size, float angle)
        {
            if (wantedHits == WantedHits.all)
            {
                numberOfFoundHits = DrawPhysics2D.BoxCast(origin, size, angle, Get_customVector2_1_inGlobalSpaceUnits(), GetContactFilter2D_fromInspectorSpecifiedOptions(), unusedRaycastHitResults, used_distance, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
            }
            else
            {
                Get_minAndMaxDepth(out float used_minDepth, out float used_maxDepth);
                RaycastHit2D result = DrawPhysics2D.BoxCast(origin, size, angle, Get_customVector2_1_inGlobalSpaceUnits(), used_distance, layerMask, used_minDepth, used_maxDepth, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = (result.collider != null) ? 1 : 0;
            }
        }

        void CircleCast2D(Vector2 origin, float radius)
        {
            if (wantedHits == WantedHits.all)
            {
                numberOfFoundHits = DrawPhysics2D.CircleCast(origin, radius, Get_customVector2_1_inGlobalSpaceUnits(), GetContactFilter2D_fromInspectorSpecifiedOptions(), unusedRaycastHitResults, used_distance, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
            }
            else
            {
                Get_minAndMaxDepth(out float used_minDepth, out float used_maxDepth);
                RaycastHit2D result = DrawPhysics2D.CircleCast(origin, radius, Get_customVector2_1_inGlobalSpaceUnits(), used_distance, layerMask, used_minDepth, used_maxDepth, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = (result.collider != null) ? 1 : 0;
            }
        }

        void CapsuleCast2D(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle)
        {
            if (wantedHits == WantedHits.all)
            {
                numberOfFoundHits = DrawPhysics2D.CapsuleCast(origin, size, capsuleDirection, angle, Get_customVector2_1_inGlobalSpaceUnits(), GetContactFilter2D_fromInspectorSpecifiedOptions(), unusedRaycastHitResults, used_distance, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
            }
            else
            {
                Get_minAndMaxDepth(out float used_minDepth, out float used_maxDepth);
                RaycastHit2D result = DrawPhysics2D.CapsuleCast(origin, size, capsuleDirection, angle, Get_customVector2_1_inGlobalSpaceUnits(), used_distance, layerMask, used_minDepth, used_maxDepth, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = (result.collider != null) ? 1 : 0;
            }
        }

        void RayCast2D()
        {
            if (wantedHits == WantedHits.all)
            {
                numberOfFoundHits = DrawPhysics2D.Raycast(GetDrawPos2D_global(), Get_customVector2_1_inGlobalSpaceUnits(), GetContactFilter2D_fromInspectorSpecifiedOptions(), unusedRaycastHitResults, used_distance, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
            }
            else
            {
                Get_minAndMaxDepth(out float used_minDepth, out float used_maxDepth);
                RaycastHit2D result = DrawPhysics2D.Raycast(GetDrawPos2D_global(), Get_customVector2_1_inGlobalSpaceUnits(), used_distance, layerMask, used_minDepth, used_maxDepth, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = (result.collider != null) ? 1 : 0;
            }
        }

        void Ray3DCastIn2D()
        {
            Ray ray = new Ray(GetDrawPos3D_global(), Get_customVector3_1_inGlobalSpaceUnits());
            if (wantedHits == WantedHits.all)
            {
                RaycastHit2D[] results = DrawPhysics2D.GetRayIntersectionAll(ray, used_distance, layerMask, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = (results != null) ? results.Length : 0;
            }
            else
            {
                RaycastHit2D result = DrawPhysics2D.GetRayIntersection(ray, used_distance, layerMask, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = (result.collider != null) ? 1 : 0;
            }
        }

        void Box2D_checkRespOverlap(Vector2 point, Vector2 size, float angle)
        {
            if (wantedHits == WantedHits.all)
            {
                numberOfFoundHits = DrawPhysics2D.OverlapBox(point, size, angle, GetContactFilter2D_fromInspectorSpecifiedOptions(), unusedResultColliders, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
            }
            else
            {
                Get_minAndMaxDepth(out float used_minDepth, out float used_maxDepth);
                Collider2D overlappingCollider = DrawPhysics2D.OverlapBox(point, size, angle, layerMask, used_minDepth, used_maxDepth, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = (overlappingCollider != null) ? 1 : 0;
            }
        }

        void Circle2D_checkRespOverlap(Vector2 point, float radius)
        {
            if (wantedHits == WantedHits.all)
            {
                numberOfFoundHits = DrawPhysics2D.OverlapCircle(point, radius, GetContactFilter2D_fromInspectorSpecifiedOptions(), unusedResultColliders, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
            }
            else
            {
                Get_minAndMaxDepth(out float used_minDepth, out float used_maxDepth);
                Collider2D overlappingCollider = DrawPhysics2D.OverlapCircle(point, radius, layerMask, used_minDepth, used_maxDepth, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = (overlappingCollider != null) ? 1 : 0;
            }
        }

        void Capsule2D_checkRespOverlap(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle)
        {
            if (wantedHits == WantedHits.all)
            {
                numberOfFoundHits = DrawPhysics2D.OverlapCapsule(point, size, direction, angle, GetContactFilter2D_fromInspectorSpecifiedOptions(), unusedResultColliders, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
            }
            else
            {
                Get_minAndMaxDepth(out float used_minDepth, out float used_maxDepth);
                Collider2D overlappingCollider = DrawPhysics2D.OverlapCapsule(point, size, direction, angle, layerMask, used_minDepth, used_maxDepth, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = (overlappingCollider != null) ? 1 : 0;
            }
        }

        void Point2D_checkRespOverlap()
        {
            if (wantedHits == WantedHits.all)
            {
                numberOfFoundHits = DrawPhysics2D.OverlapPoint(GetDrawPos2D_global(), GetContactFilter2D_fromInspectorSpecifiedOptions(), unusedResultColliders, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
            }
            else
            {
                Get_minAndMaxDepth(out float used_minDepth, out float used_maxDepth);
                Collider2D overlappingCollider = DrawPhysics2D.OverlapPoint(GetDrawPos2D_global(), layerMask, used_minDepth, used_maxDepth, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = (overlappingCollider != null) ? 1 : 0;
            }
        }

        ContactFilter2D GetContactFilter2D_fromInspectorSpecifiedOptions()
        {
            ContactFilter2D created_contactFilter2D = new ContactFilter2D();

            created_contactFilter2D.useLayerMask = true;
            created_contactFilter2D.layerMask = layerMask;

            created_contactFilter2D.useDepth = useDepth;
            created_contactFilter2D.minDepth = minDepth;
            created_contactFilter2D.maxDepth = maxDepth;
            created_contactFilter2D.useOutsideDepth = useOutsideDepth;

            created_contactFilter2D.useNormalAngle = useNormalAngle;
            created_contactFilter2D.minNormalAngle = minNormalAngle;
            created_contactFilter2D.maxNormalAngle = maxNormalAngle;
            created_contactFilter2D.useOutsideNormalAngle = useOutsideNormalAngle;

            created_contactFilter2D.useTriggers = useTriggers;

            return created_contactFilter2D;
        }

        void Get_minAndMaxDepth(out float used_minDepth, out float used_maxDepth)
        {
            if (useDepth)
            {
                used_minDepth = minDepth;
                used_maxDepth = maxDepth;
            }
            else
            {
                used_minDepth = Mathf.NegativeInfinity;
                used_maxDepth = Mathf.Infinity;
            }
        }

        static Vector2 GetInitialValueFor_customDirectionForHitResultText()
        {
            if (UtilitiesDXXL_Math.IsDefaultVector(DrawPhysics2D.directionOfHitResultText))
            {
                return DrawBasics.Default_textOffsetDirection_forPointTags;
            }
            else
            {
                return DrawPhysics2D.directionOfHitResultText;
            }
        }

    }

}
