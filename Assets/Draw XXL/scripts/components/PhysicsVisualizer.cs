namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Physics Visualizer")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class PhysicsVisualizer : VisualizerParent
    {
        public enum Shape { collidersOnThisGameobject, box, sphere, capsule, ray };
        [SerializeField] Shape shape = Shape.collidersOnThisGameobject;

        public enum CollisionType { cast, overlap };
        [SerializeField] CollisionType collisionType = CollisionType.cast;

        public enum WantedHits { onlyFirst, all };
        [SerializeField] WantedHits wantedHits = WantedHits.all;

        [SerializeField] bool distanceIsInfinityRespToOtherGO = true;
        [SerializeField] float adjustedDistance = 20.0f;
        float used_distance;

        [SerializeField] float radiusScaleFactor_ofCastRespCheckedShape = 0.5f;
        [SerializeField] Vector3 sizeScaleFactors_ofCastRespCheckedBox = Vector3.one;
        [SerializeField] float heightScaleFactor_ofCastRespCheckedCapsule = 2.0f;
        public enum CapsuleAlignment { alongLocalX, alongLocalY, alongLocalZ };
        [SerializeField] CapsuleAlignment capsuleAlignment = CapsuleAlignment.alongLocalY;

        public enum ShapeOrientationType { transformsRotationPlusOptionalAdditionalLocalRotation, transformsRotationPlusOptionalAdditionalGlobalRotation, customRotationIndependentFromTransform };
        [SerializeField] public ShapeOrientationType shapeOrientationType = ShapeOrientationType.transformsRotationPlusOptionalAdditionalLocalRotation;
        [SerializeField] public Vector3 optionalAdditionalRotation_asEulersInV3 = Vector3.zero;
        [SerializeField] public Vector3 customRotation_asEulersInV3 = Vector3.zero;
        [SerializeField] public bool showTransformIndependentRotationHandle = false;

        BoxCollider[] boxColliders_onThisGameobject;
        SphereCollider[] sphereColliders_onThisGameobject;
        CapsuleCollider[] capsuleColliders_onThisGameobject;
        [SerializeField] public bool theGameobjectHasACompatibleAndEnabledCollider; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.

        [SerializeField] public bool otherSettings_isFoldedOut = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.

        [SerializeField] bool excludeCollidersOnThisGO = true;
        [SerializeField] bool excludeCollidersOnParentGOs = true;
        [SerializeField] bool excludeCollidersOnChildrenGOs = true;

        Collider[] allCollidersOnThisGameobject;
        List<bool> enabledState_ofAllCollidersOnThisGO_beforeCurrentDrawOperation = new List<bool>();

        Collider[] allCollidersOnThisGameobjectAndOnParents;
        List<bool> enabledState_ofAllCollidersOnThisGOPlusParents_beforeCurrentDrawOperation = new List<bool>();
        List<bool> isOnThisGO_forAllCollidersOnThisGOPlusParents_beforeCurrentDrawOperation = new List<bool>();

        Collider[] allCollidersOnThisGameobjectAndOnChildren;
        List<bool> enabledState_ofAllCollidersOnThisGOPlusChildren_beforeCurrentDrawOperation = new List<bool>();
        List<bool> isOnThisGO_forAllCollidersOnThisGOPlusChildren_beforeCurrentDrawOperation = new List<bool>();

        [SerializeField] LayerMask layerMask = Physics.DefaultRaycastLayers; //The "Physics.DefaultRaycastLayers" already inludes the layer slots that are not yet defined by the users. That means this doesn't have to updated in cases where a user adds a custom defined layer AFTER the creation of this component
        [SerializeField] QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
        [SerializeField] int numberOfFoundHits = 0;

        //DrawPhysics' class global settings:
        [SerializeField] Color colorForNonHittingCasts = DrawPhysics.colorForNonHittingCasts;
        [SerializeField] Color colorForHittingCasts = DrawPhysics.colorForHittingCasts;
        [SerializeField] Color colorForCastLineBeyondHit = DrawPhysics.colorForCastLineBeyondHit;
        [SerializeField] Color colorForCastsHitText = DrawPhysics.colorForCastsHitText;
        [SerializeField] bool doOverwriteColorForCastsHitNormals = false;
        [SerializeField] Color overwriteColorForCastsHitNormals = UtilitiesDXXL_Physics.Get_defaultColor_ofNormal(); //-> not using "DrawPhysics.overwriteColorForCastsHitNormals", since this would be the default color that doesn't represent what the user sees as normal color in the Scene
        [SerializeField] float scaleFactor_forCastHitTextSize = DrawPhysics.scaleFactor_forCastHitTextSize;
        [SerializeField] float castSilhouetteVisualizerDensity = DrawPhysics.castSilhouetteVisualizerDensity;
        [SerializeField] bool drawCastNameTag_atCastOrigin = DrawPhysics.drawCastNameTag_atCastOrigin;
        [SerializeField] bool drawCastNameTag_atHitPositions = DrawPhysics.drawCastNameTag_atHitPositions;
        [SerializeField] int maxListedColliders_inOverlapVolumesTextList = DrawPhysics.MaxListedColliders_inOverlapVolumesTextList;
        [SerializeField] int maxOverlapingCollidersWithUntruncatedText = DrawPhysics.maxOverlapingCollidersWithUntruncatedText;
        [SerializeField] [Range(0.001f, 0.2f)] float forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts = 0.01f;
        [SerializeField] float forcedConstantWorldspaceTextSize_forOverlapResultTexts = 0.1f;
        [SerializeField] bool useCustomDirectionForHitResultText = false;
        [SerializeField] Vector2 customDirectionForHitResultText = GetInitialValueFor_customDirectionForHitResultText();

        public enum SaveDrawnLinesType { no_useFullDetails, useAMidwayTradeoff, yes_displayWithLowDetails };
        [SerializeField] SaveDrawnLinesType saveDrawnLinesType = Map_visualizationQuality_to_saveDrawnLinesType(DrawPhysics.visualizationQuality);

        public enum OverlapResultTextSizeInterpretation { relativeToTheSizeOfTheOverlapingPhysicsShape, fixedWorldSpaceSize, relativeToTheSceneViewWindowSize, relativeToTheGameViewWindowSize };
        [SerializeField] OverlapResultTextSizeInterpretation overlapResultTextSizeInterpretation = OverlapResultTextSizeInterpretation.relativeToTheSizeOfTheOverlapingPhysicsShape;

        Color colorForNonHittingCasts_before;
        Color colorForHittingCasts_before;
        Color colorForCastLineBeyondHit_before;
        Color colorForCastsHitText_before;
        Color overwriteColorForCastsHitNormals_before;
        float scaleFactor_forCastHitTextSize_before;
        float castSilhouetteVisualizerDensity_before;
        DrawPhysics.VisualizationQuality visualizationQuality_before;
        bool drawCastNameTag_atCastOrigin_before;
        bool drawCastNameTag_atHitPositions_before;
        int maxListedColliders_inOverlapVolumesTextList_before;
        int maxOverlapingCollidersWithUntruncatedText_before;
        float forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts_before;
        float forcedConstantWorldspaceTextSize_forOverlapResultTexts_before;
        DrawBasics.CameraForAutomaticOrientation cameraForAutomaticOrientation_before;
        Vector2 directionOfHitResultText_before;

        public override void InitializeValues_onceInComponentLifetime()
        {
            if (text_exclGlobalMarkupTags == null || text_exclGlobalMarkupTags == "")
            {
                text_exclGlobalMarkupTags = "Collisions from " + this.gameObject.name;
                text_inclGlobalMarkupTags = "Collisions from " + this.gameObject.name;
            }

            customVector3_1_picker_isOutfolded = true;
            source_ofCustomVector3_1 = CustomVector3Source.transformsForward;
            customVector3_1_clipboardForManualInput = Vector3.one;
            vectorInterpretation_ofCustomVector3_1 = VectorInterpretation.globalSpace;
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
            TryFetchCurrentEnabledStateOfCollidersOnThisGameobjectsHierarchy();
            Save_globalDrawPhysicsOptions_beforeThisDrawOperation();
            try
            {
                Set_globalDrawPhysicsOptions_toValuesFromInpsector();
                TryDisableEnabledStateOfCollidersOnThisGameobjectsHierarchy();
                CastRespCheckTheColliders_andDrawThem();
            }
            catch { }
            TryRestoreEnabledStateOfCollidersOnThisGameobjectsHierarchy();
            Restore_globalDrawPhysicsOptions_toValuesFromBefore();
        }

        bool CheckIfGameobjectHasASupportedCollider()
        {
            if ((this.gameObject.GetComponent<BoxCollider>() == null) && (this.gameObject.GetComponent<SphereCollider>() == null) && (this.gameObject.GetComponent<CapsuleCollider>() == null))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        void TryFetchCurrentEnabledStateOfCollidersOnThisGameobjectsHierarchy()
        {
            if (excludeCollidersOnThisGO || excludeCollidersOnParentGOs || excludeCollidersOnChildrenGOs) //The other flag's threads need "allCollidersOnThisGameobject"
            {
                allCollidersOnThisGameobject = this.gameObject.GetComponents<Collider>();
                if (allCollidersOnThisGameobject != null)
                {
                    for (int i = 0; i < allCollidersOnThisGameobject.Length; i++)
                    {
                        if (allCollidersOnThisGameobject[i] != null)
                        {
                            UtilitiesDXXL_List.AddToABoolList(ref enabledState_ofAllCollidersOnThisGO_beforeCurrentDrawOperation, allCollidersOnThisGameobject[i].enabled, i);
                        }
                    }
                }
            }

            if (excludeCollidersOnParentGOs)
            {
                allCollidersOnThisGameobjectAndOnParents = this.gameObject.GetComponentsInParent<Collider>();
                if (allCollidersOnThisGameobjectAndOnParents != null)
                {
                    for (int i = 0; i < allCollidersOnThisGameobjectAndOnParents.Length; i++)
                    {
                        if (allCollidersOnThisGameobjectAndOnParents[i] != null)
                        {
                            UtilitiesDXXL_List.AddToABoolList(ref enabledState_ofAllCollidersOnThisGOPlusParents_beforeCurrentDrawOperation, allCollidersOnThisGameobjectAndOnParents[i].enabled, i);
                            UtilitiesDXXL_List.AddToABoolList(ref isOnThisGO_forAllCollidersOnThisGOPlusParents_beforeCurrentDrawOperation, CheckIfColliderIsOnThisGameobject(allCollidersOnThisGameobjectAndOnParents[i]), i);
                        }
                    }
                }
            }

            if (excludeCollidersOnChildrenGOs)
            {
                allCollidersOnThisGameobjectAndOnChildren = this.gameObject.GetComponentsInChildren<Collider>();

                if (allCollidersOnThisGameobjectAndOnChildren != null)
                {
                    for (int i = 0; i < allCollidersOnThisGameobjectAndOnChildren.Length; i++)
                    {
                        if (allCollidersOnThisGameobjectAndOnChildren[i] != null)
                        {
                            UtilitiesDXXL_List.AddToABoolList(ref enabledState_ofAllCollidersOnThisGOPlusChildren_beforeCurrentDrawOperation, allCollidersOnThisGameobjectAndOnChildren[i].enabled, i);
                            UtilitiesDXXL_List.AddToABoolList(ref isOnThisGO_forAllCollidersOnThisGOPlusChildren_beforeCurrentDrawOperation, CheckIfColliderIsOnThisGameobject(allCollidersOnThisGameobjectAndOnChildren[i]), i);
                        }
                    }
                }
            }
        }

        bool CheckIfColliderIsOnThisGameobject(Collider collider_toCheckIfItIsOnThisGameobject)
        {
            if (collider_toCheckIfItIsOnThisGameobject != null)
            {
                if (allCollidersOnThisGameobject != null)
                {
                    for (int i = 0; i < allCollidersOnThisGameobject.Length; i++)
                    {
                        if (allCollidersOnThisGameobject[i] != null)
                        {
                            if (allCollidersOnThisGameobject[i] == collider_toCheckIfItIsOnThisGameobject)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        bool GetEnabledStateBeforeCurrentDrawOperation_ofColliderComponentOnThisGameobject(Collider collider_toRetrieveEnabledStateFor)
        {
            if (collider_toRetrieveEnabledStateFor != null)
            {
                if (allCollidersOnThisGameobject != null)
                {
                    for (int i = 0; i < allCollidersOnThisGameobject.Length; i++)
                    {
                        if (allCollidersOnThisGameobject[i] != null)
                        {
                            if (allCollidersOnThisGameobject[i] == collider_toRetrieveEnabledStateFor)
                            {
                                return enabledState_ofAllCollidersOnThisGO_beforeCurrentDrawOperation[i];
                            }
                        }
                    }
                }
            }
            return false;
        }

        void TryDisableEnabledStateOfCollidersOnThisGameobjectsHierarchy()
        {
            if (excludeCollidersOnThisGO)
            {
                if (allCollidersOnThisGameobject != null)
                {
                    for (int i = 0; i < allCollidersOnThisGameobject.Length; i++)
                    {
                        if (allCollidersOnThisGameobject[i] != null)
                        {
                            allCollidersOnThisGameobject[i].enabled = false;
                        }
                    }
                }
            }

            if (excludeCollidersOnParentGOs)
            {
                if (allCollidersOnThisGameobjectAndOnParents != null)
                {
                    for (int i = 0; i < allCollidersOnThisGameobjectAndOnParents.Length; i++)
                    {
                        if (allCollidersOnThisGameobjectAndOnParents[i] != null)
                        {
                            if (isOnThisGO_forAllCollidersOnThisGOPlusParents_beforeCurrentDrawOperation[i] == false)
                            {
                                allCollidersOnThisGameobjectAndOnParents[i].enabled = false;
                            }
                        }
                    }
                }
            }

            if (excludeCollidersOnChildrenGOs)
            {
                if (allCollidersOnThisGameobjectAndOnChildren != null)
                {
                    for (int i = 0; i < allCollidersOnThisGameobjectAndOnChildren.Length; i++)
                    {
                        if (allCollidersOnThisGameobjectAndOnChildren[i] != null)
                        {
                            if (isOnThisGO_forAllCollidersOnThisGOPlusChildren_beforeCurrentDrawOperation[i] == false)
                            {
                                allCollidersOnThisGameobjectAndOnChildren[i].enabled = false;
                            }
                        }
                    }
                }
            }
        }

        void TryRestoreEnabledStateOfCollidersOnThisGameobjectsHierarchy()
        {
            if (excludeCollidersOnThisGO)
            {
                if (allCollidersOnThisGameobject != null)
                {
                    for (int i = 0; i < allCollidersOnThisGameobject.Length; i++)
                    {
                        if (allCollidersOnThisGameobject[i] != null)
                        {
                            allCollidersOnThisGameobject[i].enabled = enabledState_ofAllCollidersOnThisGO_beforeCurrentDrawOperation[i];
                        }
                    }
                }
            }

            if (excludeCollidersOnParentGOs)
            {
                if (allCollidersOnThisGameobjectAndOnParents != null)
                {
                    for (int i = 0; i < allCollidersOnThisGameobjectAndOnParents.Length; i++)
                    {
                        if (allCollidersOnThisGameobjectAndOnParents[i] != null)
                        {
                            if (isOnThisGO_forAllCollidersOnThisGOPlusParents_beforeCurrentDrawOperation[i] == false)
                            {
                                allCollidersOnThisGameobjectAndOnParents[i].enabled = enabledState_ofAllCollidersOnThisGOPlusParents_beforeCurrentDrawOperation[i];
                            }
                        }
                    }
                }
            }

            if (excludeCollidersOnChildrenGOs)
            {
                if (allCollidersOnThisGameobjectAndOnChildren != null)
                {
                    for (int i = 0; i < allCollidersOnThisGameobjectAndOnChildren.Length; i++)
                    {
                        if (allCollidersOnThisGameobjectAndOnChildren[i] != null)
                        {
                            if (isOnThisGO_forAllCollidersOnThisGOPlusChildren_beforeCurrentDrawOperation[i] == false)
                            {
                                allCollidersOnThisGameobjectAndOnChildren[i].enabled = enabledState_ofAllCollidersOnThisGOPlusChildren_beforeCurrentDrawOperation[i];
                            }
                        }
                    }
                }
            }
        }

        void Save_globalDrawPhysicsOptions_beforeThisDrawOperation()
        {
            colorForNonHittingCasts_before = DrawPhysics.colorForNonHittingCasts;
            colorForHittingCasts_before = DrawPhysics.colorForHittingCasts;
            colorForCastLineBeyondHit_before = DrawPhysics.colorForCastLineBeyondHit;
            colorForCastsHitText_before = DrawPhysics.colorForCastsHitText;
            scaleFactor_forCastHitTextSize_before = DrawPhysics.scaleFactor_forCastHitTextSize;
            castSilhouetteVisualizerDensity_before = DrawPhysics.castSilhouetteVisualizerDensity;
            visualizationQuality_before = DrawPhysics.visualizationQuality;
            drawCastNameTag_atCastOrigin_before = DrawPhysics.drawCastNameTag_atCastOrigin;
            drawCastNameTag_atHitPositions_before = DrawPhysics.drawCastNameTag_atHitPositions;
            maxListedColliders_inOverlapVolumesTextList_before = DrawPhysics.MaxListedColliders_inOverlapVolumesTextList;
            maxOverlapingCollidersWithUntruncatedText_before = DrawPhysics.maxOverlapingCollidersWithUntruncatedText;
            forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts_before = DrawPhysics.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts;
            forcedConstantWorldspaceTextSize_forOverlapResultTexts_before = DrawPhysics.forcedConstantWorldspaceTextSize_forOverlapResultTexts;
            cameraForAutomaticOrientation_before = DrawBasics.cameraForAutomaticOrientation;

            if (doOverwriteColorForCastsHitNormals)
            {
                overwriteColorForCastsHitNormals_before = DrawPhysics.overwriteColorForCastsHitNormals;
            }

            if (useCustomDirectionForHitResultText)
            {
                directionOfHitResultText_before = DrawPhysics.directionOfHitResultText;
            }
        }

        void Set_globalDrawPhysicsOptions_toValuesFromInpsector()
        {
            DrawPhysics.colorForNonHittingCasts = colorForNonHittingCasts;
            DrawPhysics.colorForHittingCasts = colorForHittingCasts;
            DrawPhysics.colorForCastLineBeyondHit = colorForCastLineBeyondHit;
            DrawPhysics.colorForCastsHitText = colorForCastsHitText;
            DrawPhysics.scaleFactor_forCastHitTextSize = scaleFactor_forCastHitTextSize;
            DrawPhysics.castSilhouetteVisualizerDensity = castSilhouetteVisualizerDensity;
            DrawPhysics.visualizationQuality = Map_saveDrawnLinesType_to_visualizationQuality(saveDrawnLinesType);
            DrawPhysics.drawCastNameTag_atCastOrigin = drawCastNameTag_atCastOrigin;
            DrawPhysics.drawCastNameTag_atHitPositions = drawCastNameTag_atHitPositions;
            DrawPhysics.MaxListedColliders_inOverlapVolumesTextList = maxListedColliders_inOverlapVolumesTextList;
            DrawPhysics.maxOverlapingCollidersWithUntruncatedText = maxOverlapingCollidersWithUntruncatedText;

            switch (overlapResultTextSizeInterpretation)
            {
                case OverlapResultTextSizeInterpretation.relativeToTheSizeOfTheOverlapingPhysicsShape:
                    DrawPhysics.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts = 0.0f;
                    DrawPhysics.forcedConstantWorldspaceTextSize_forOverlapResultTexts = 0.0f;
                    break;
                case OverlapResultTextSizeInterpretation.fixedWorldSpaceSize:
                    DrawPhysics.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts = 0.0f;
                    DrawPhysics.forcedConstantWorldspaceTextSize_forOverlapResultTexts = forcedConstantWorldspaceTextSize_forOverlapResultTexts;
                    break;
                case OverlapResultTextSizeInterpretation.relativeToTheSceneViewWindowSize:
                    DrawPhysics.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts = forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts;
                    DrawPhysics.forcedConstantWorldspaceTextSize_forOverlapResultTexts = 0.0f;
                    DrawBasics.cameraForAutomaticOrientation = DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;
                    break;
                case OverlapResultTextSizeInterpretation.relativeToTheGameViewWindowSize:
                    DrawPhysics.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts = forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts;
                    DrawPhysics.forcedConstantWorldspaceTextSize_forOverlapResultTexts = 0.0f;
                    DrawBasics.cameraForAutomaticOrientation = DrawBasics.CameraForAutomaticOrientation.gameViewCamera;
                    break;
                default:
                    break;
            }

            if (doOverwriteColorForCastsHitNormals)
            {
                DrawPhysics.overwriteColorForCastsHitNormals = overwriteColorForCastsHitNormals;
            }

            if (useCustomDirectionForHitResultText)
            {
                DrawPhysics.directionOfHitResultText = customDirectionForHitResultText;
            }
        }

        void Restore_globalDrawPhysicsOptions_toValuesFromBefore()
        {
            DrawPhysics.colorForNonHittingCasts = colorForNonHittingCasts_before;
            DrawPhysics.colorForHittingCasts = colorForHittingCasts_before;
            DrawPhysics.colorForCastLineBeyondHit = colorForCastLineBeyondHit_before;
            DrawPhysics.colorForCastsHitText = colorForCastsHitText_before;
            DrawPhysics.scaleFactor_forCastHitTextSize = scaleFactor_forCastHitTextSize_before;
            DrawPhysics.castSilhouetteVisualizerDensity = castSilhouetteVisualizerDensity_before;
            DrawPhysics.visualizationQuality = visualizationQuality_before;
            DrawPhysics.drawCastNameTag_atCastOrigin = drawCastNameTag_atCastOrigin_before;
            DrawPhysics.drawCastNameTag_atHitPositions = drawCastNameTag_atHitPositions_before;
            DrawPhysics.MaxListedColliders_inOverlapVolumesTextList = maxListedColliders_inOverlapVolumesTextList_before;
            DrawPhysics.maxOverlapingCollidersWithUntruncatedText = maxOverlapingCollidersWithUntruncatedText_before;
            DrawPhysics.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts = forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts_before;
            DrawPhysics.forcedConstantWorldspaceTextSize_forOverlapResultTexts = forcedConstantWorldspaceTextSize_forOverlapResultTexts_before;
            DrawBasics.cameraForAutomaticOrientation = cameraForAutomaticOrientation_before;

            if (doOverwriteColorForCastsHitNormals)
            {
                DrawPhysics.overwriteColorForCastsHitNormals = overwriteColorForCastsHitNormals_before;
            }

            if (useCustomDirectionForHitResultText)
            {
                DrawPhysics.directionOfHitResultText = directionOfHitResultText_before;
            }
        }

        void CastRespCheckTheColliders_andDrawThem()
        {
            used_distance = Get_used_distance();
            switch (shape)
            {
                case Shape.collidersOnThisGameobject:
                    theGameobjectHasACompatibleAndEnabledCollider = false;
                    CastRespOverlap_boxCollidersOnThisGameobject();
                    CastRespOverlap_sphereCollidersOnThisGameobject();
                    CastRespOverlap_capsuleCollidersOnThisGameobject();
                    break;
                case Shape.box:
                    switch (collisionType)
                    {
                        case CollisionType.cast:
                            BoxCast(GetDrawPos3D_global(), Get_halfExtentsOfFinalBoxShape(sizeScaleFactors_ofCastRespCheckedBox), GetUsedBoxRotation());
                            break;
                        case CollisionType.overlap:
                            Box_checkRespOverlap(GetDrawPos3D_global(), Get_halfExtentsOfFinalBoxShape(sizeScaleFactors_ofCastRespCheckedBox), GetUsedBoxRotation());
                            break;
                        default:
                            break;
                    }
                    break;
                case Shape.sphere:
                    switch (collisionType)
                    {
                        case CollisionType.cast:
                            SphereCast(GetDrawPos3D_global(), GetRadiusOfFinalSphereShape(radiusScaleFactor_ofCastRespCheckedShape));
                            break;
                        case CollisionType.overlap:
                            Sphere_checkRespOverlap(GetDrawPos3D_global(), GetRadiusOfFinalSphereShape(radiusScaleFactor_ofCastRespCheckedShape));
                            break;
                        default:
                            break;
                    }
                    break;
                case Shape.capsule:
                    Get_shapeParameters_ofFinalCapsuleShape(out Vector3 point1, out Vector3 point2, out float radius, capsuleAlignment, radiusScaleFactor_ofCastRespCheckedShape, heightScaleFactor_ofCastRespCheckedCapsule, GetDrawPos3D_global());
                    switch (collisionType)
                    {
                        case CollisionType.cast:
                            CapsuleCast(point1, point2, radius);
                            break;
                        case CollisionType.overlap:
                            Capsule_checkRespOverlap(point1, point2, radius);
                            break;
                        default:
                            break;
                    }
                    break;
                case Shape.ray:
                    RayCast();
                    break;
                default:
                    break;
            }
        }

        float Get_used_distance()
        {
            if (distanceIsInfinityRespToOtherGO)
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
                return adjustedDistance;
            }
        }

        void CastRespOverlap_boxCollidersOnThisGameobject()
        {
            boxColliders_onThisGameobject = this.gameObject.GetComponents<BoxCollider>();
            if (boxColliders_onThisGameobject != null)
            {
                for (int i = 0; i < boxColliders_onThisGameobject.Length; i++)
                {
                    if (boxColliders_onThisGameobject[i] != null)
                    {
                        if (boxColliders_onThisGameobject[i].enabled || GetEnabledStateBeforeCurrentDrawOperation_ofColliderComponentOnThisGameobject(boxColliders_onThisGameobject[i]))
                        {
                            theGameobjectHasACompatibleAndEnabledCollider = true;
                            Vector3 center = GetCenterPosGlobalOfCollider(boxColliders_onThisGameobject[i].center);
                            Vector3 halfExtents = Get_halfExtentsOfFinalBoxShape(boxColliders_onThisGameobject[i].size);
                            Quaternion orientation = transform.rotation;
                            switch (collisionType)
                            {
                                case CollisionType.cast:
                                    BoxCast(center, halfExtents, orientation);
                                    break;
                                case CollisionType.overlap:
                                    Box_checkRespOverlap(center, halfExtents, orientation);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        void CastRespOverlap_sphereCollidersOnThisGameobject()
        {
            sphereColliders_onThisGameobject = this.gameObject.GetComponents<SphereCollider>();
            if (sphereColliders_onThisGameobject != null)
            {
                for (int i = 0; i < sphereColliders_onThisGameobject.Length; i++)
                {
                    if (sphereColliders_onThisGameobject[i] != null)
                    {
                        if (sphereColliders_onThisGameobject[i].enabled || GetEnabledStateBeforeCurrentDrawOperation_ofColliderComponentOnThisGameobject(sphereColliders_onThisGameobject[i]))
                        {
                            theGameobjectHasACompatibleAndEnabledCollider = true;
                            Vector3 origin = GetCenterPosGlobalOfCollider(sphereColliders_onThisGameobject[i].center);
                            float radius = GetRadiusOfFinalSphereShape(sphereColliders_onThisGameobject[i].radius);
                            switch (collisionType)
                            {
                                case CollisionType.cast:
                                    SphereCast(origin, radius);
                                    break;
                                case CollisionType.overlap:
                                    Sphere_checkRespOverlap(origin, radius);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        void CastRespOverlap_capsuleCollidersOnThisGameobject()
        {
            capsuleColliders_onThisGameobject = this.gameObject.GetComponents<CapsuleCollider>();
            if (capsuleColliders_onThisGameobject != null)
            {
                for (int i = 0; i < capsuleColliders_onThisGameobject.Length; i++)
                {
                    if (capsuleColliders_onThisGameobject[i] != null)
                    {
                        if (capsuleColliders_onThisGameobject[i].enabled || GetEnabledStateBeforeCurrentDrawOperation_ofColliderComponentOnThisGameobject(capsuleColliders_onThisGameobject[i]))
                        {
                            theGameobjectHasACompatibleAndEnabledCollider = true;
                            Vector3 colliderCenter_global = GetCenterPosGlobalOfCollider(capsuleColliders_onThisGameobject[i].center);
                            Get_shapeParameters_ofFinalCapsuleShape(out Vector3 point1, out Vector3 point2, out float radius, GetCapsuleAlignmentFromCollider(capsuleColliders_onThisGameobject[i].direction), capsuleColliders_onThisGameobject[i].radius, capsuleColliders_onThisGameobject[i].height, colliderCenter_global);
                            switch (collisionType)
                            {
                                case CollisionType.cast:
                                    CapsuleCast(point1, point2, radius);
                                    break;
                                case CollisionType.overlap:
                                    Capsule_checkRespOverlap(point1, point2, radius);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        float GetRadiusOfFinalSphereShape(float radiusScaleFactor_fromComponentProperty)
        {
            return (radiusScaleFactor_fromComponentProperty * UtilitiesDXXL_Math.GetBiggestAbsComponent_butReassignTheSign(transform.lossyScale));
        }

        Vector3 Get_halfExtentsOfFinalBoxShape(Vector3 sizeScaleFactors_fromComponentProperty)
        {
            return (0.5f * Vector3.Scale(transform.lossyScale, sizeScaleFactors_fromComponentProperty));
        }

        void Get_shapeParameters_ofFinalCapsuleShape(out Vector3 point1, out Vector3 point2, out float radius, CapsuleAlignment capsuleAlignment, float radiusScaleFactor_fromComponentProperty, float heightScaleFactor_fromComponentProperty, Vector3 finalCapsuleCenterPos_global)
        {
            UtilitiesDXXL_Math.Dimension directionOfCapsule = Convert_capsuleAlignment_to_cartesianDimension(capsuleAlignment);
            radius = radiusScaleFactor_fromComponentProperty * UtilitiesDXXL_Math.GetBiggestAbsComponent_butReassignTheSign(transform.lossyScale, directionOfCapsule);
            float absRadius = Mathf.Abs(radius);
            float capsuleHeight = heightScaleFactor_fromComponentProperty * UtilitiesDXXL_Math.GetComponentByDimension(transform.lossyScale, directionOfCapsule); //"height" includes the whole capsule shape, and is not just from sphereCenter to sphereCenter
            float halfCapsuleHeight = 0.5f * capsuleHeight;
            float absHalfCapsuleHeight = Mathf.Abs(halfCapsuleHeight);
            Vector3 finalUpwardDirectionOfCapsuleToCastRespCheck_asNormalizedVector = Get_finalUpwardDirectionOfCapsuleToCastRespCheck_asNormalizedVector(directionOfCapsule);
            float point1s_offsetDistanceFromCenter = Mathf.Max(0.0f, absHalfCapsuleHeight - absRadius);
            Vector3 point1s_offsetFromCenter = finalUpwardDirectionOfCapsuleToCastRespCheck_asNormalizedVector * point1s_offsetDistanceFromCenter;
            point1 = finalCapsuleCenterPos_global + point1s_offsetFromCenter;
            point2 = finalCapsuleCenterPos_global - point1s_offsetFromCenter;
        }

        static UtilitiesDXXL_Math.Dimension Convert_capsuleAlignment_to_cartesianDimension(CapsuleAlignment capsuleAlignment_toConvert)
        {
            switch (capsuleAlignment_toConvert)
            {
                case CapsuleAlignment.alongLocalX:
                    return UtilitiesDXXL_Math.Dimension.x;
                case CapsuleAlignment.alongLocalY:
                    return UtilitiesDXXL_Math.Dimension.y;
                case CapsuleAlignment.alongLocalZ:
                    return UtilitiesDXXL_Math.Dimension.z;
                default:
                    return UtilitiesDXXL_Math.Dimension.x;
            }
        }

        Vector3 GetCenterPosGlobalOfCollider(Vector3 collidersLocalCenter)
        {
            Vector3 xOffset = transform.right * transform.lossyScale.x * collidersLocalCenter.x;
            Vector3 yOffset = transform.up * transform.lossyScale.y * collidersLocalCenter.y;
            Vector3 zOffset = transform.forward * transform.lossyScale.z * collidersLocalCenter.z;
            return (transform.position + xOffset + yOffset + zOffset);
        }

        CapsuleAlignment GetCapsuleAlignmentFromCollider(int capsulesDirection_asInt)
        {
            switch (capsulesDirection_asInt)
            {
                case 0:
                    return CapsuleAlignment.alongLocalX;
                case 1:
                    return CapsuleAlignment.alongLocalY;
                case 2:
                    return CapsuleAlignment.alongLocalZ;
                default:
                    return CapsuleAlignment.alongLocalX;
            }
        }

        Vector3 Get_finalUpwardDirectionOfCapsuleToCastRespCheck_asNormalizedVector(UtilitiesDXXL_Math.Dimension directionOfCapsule)
        {
            Quaternion additionalRotation;
            Quaternion customRotation;
            switch (shapeOrientationType)
            {
                case ShapeOrientationType.transformsRotationPlusOptionalAdditionalLocalRotation:
                    additionalRotation = UtilitiesDXXL_Math.ApproximatelyZero(optionalAdditionalRotation_asEulersInV3) ? Quaternion.identity : Quaternion.Euler(optionalAdditionalRotation_asEulersInV3);
                    switch (directionOfCapsule)
                    {
                        case UtilitiesDXXL_Math.Dimension.x:
                            return (transform.rotation * additionalRotation * Vector3.right);
                        case UtilitiesDXXL_Math.Dimension.y:
                            return (transform.rotation * additionalRotation * Vector3.up);
                        case UtilitiesDXXL_Math.Dimension.z:
                            return (transform.rotation * additionalRotation * Vector3.forward);
                        default:
                            return Vector3.forward;
                    }
                case ShapeOrientationType.transformsRotationPlusOptionalAdditionalGlobalRotation:
                    additionalRotation = UtilitiesDXXL_Math.ApproximatelyZero(optionalAdditionalRotation_asEulersInV3) ? Quaternion.identity : Quaternion.Euler(optionalAdditionalRotation_asEulersInV3);
                    switch (directionOfCapsule)
                    {
                        case UtilitiesDXXL_Math.Dimension.x:
                            return (additionalRotation * transform.right);
                        case UtilitiesDXXL_Math.Dimension.y:
                            return (additionalRotation * transform.up);
                        case UtilitiesDXXL_Math.Dimension.z:
                            return (additionalRotation * transform.forward);
                        default:
                            return Vector3.forward;
                    }
                case ShapeOrientationType.customRotationIndependentFromTransform:
                    customRotation = UtilitiesDXXL_Math.ApproximatelyZero(customRotation_asEulersInV3) ? Quaternion.identity : Quaternion.Euler(customRotation_asEulersInV3);
                    switch (directionOfCapsule)
                    {
                        case UtilitiesDXXL_Math.Dimension.x:
                            return (customRotation * Vector3.right);
                        case UtilitiesDXXL_Math.Dimension.y:
                            return (customRotation * Vector3.up);
                        case UtilitiesDXXL_Math.Dimension.z:
                            return (customRotation * Vector3.forward);
                        default:
                            return Vector3.forward;
                    }
                default:
                    return Vector3.forward;
            }
        }

        Quaternion GetUsedBoxRotation()
        {
            switch (shapeOrientationType)
            {
                case ShapeOrientationType.transformsRotationPlusOptionalAdditionalLocalRotation:
                    if (UtilitiesDXXL_Math.ApproximatelyZero(optionalAdditionalRotation_asEulersInV3))
                    {
                        return transform.rotation;
                    }
                    else
                    {
                        return (transform.rotation * Quaternion.Euler(optionalAdditionalRotation_asEulersInV3));
                    }
                case ShapeOrientationType.transformsRotationPlusOptionalAdditionalGlobalRotation:
                    if (UtilitiesDXXL_Math.ApproximatelyZero(optionalAdditionalRotation_asEulersInV3))
                    {
                        return transform.rotation;
                    }
                    else
                    {
                        return (Quaternion.Euler(optionalAdditionalRotation_asEulersInV3) * transform.rotation);
                    }
                case ShapeOrientationType.customRotationIndependentFromTransform:
                    if (UtilitiesDXXL_Math.ApproximatelyZero(customRotation_asEulersInV3))
                    {
                        return Quaternion.identity;
                    }
                    else
                    {
                        return Quaternion.Euler(customRotation_asEulersInV3);
                    }
                default:
                    return Quaternion.identity;
            }
        }

        void BoxCast(Vector3 center, Vector3 halfExtents, Quaternion orientation)
        {
            if (wantedHits == WantedHits.all)
            {
                RaycastHit[] hitInfos = DrawPhysics.BoxCastAll(center, halfExtents, Get_customVector3_1_inGlobalSpaceUnits(), orientation, used_distance, layerMask, queryTriggerInteraction, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = (hitInfos != null) ? hitInfos.Length : 0;
            }
            else
            {
                bool hasHit = DrawPhysics.BoxCast(center, halfExtents, Get_customVector3_1_inGlobalSpaceUnits(), orientation, used_distance, layerMask, queryTriggerInteraction, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = hasHit ? 1 : 0;
            }
        }

        void SphereCast(Vector3 origin, float radius)
        {
            if (wantedHits == WantedHits.all)
            {
                RaycastHit[] hitInfos = DrawPhysics.SphereCastAll(origin, radius, Get_customVector3_1_inGlobalSpaceUnits(), used_distance, layerMask, queryTriggerInteraction, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = (hitInfos != null) ? hitInfos.Length : 0;
            }
            else
            {
                bool hasHit = DrawPhysics.SphereCast(origin, radius, Get_customVector3_1_inGlobalSpaceUnits(), used_distance, layerMask, queryTriggerInteraction, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = hasHit ? 1 : 0;
            }
        }

        void CapsuleCast(Vector3 point1, Vector3 point2, float radius)
        {
            if (wantedHits == WantedHits.all)
            {
                RaycastHit[] hitInfos = DrawPhysics.CapsuleCastAll(point1, point2, radius, Get_customVector3_1_inGlobalSpaceUnits(), used_distance, layerMask, queryTriggerInteraction, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = (hitInfos != null) ? hitInfos.Length : 0;
            }
            else
            {
                bool hasHit = DrawPhysics.CapsuleCast(point1, point2, radius, Get_customVector3_1_inGlobalSpaceUnits(), used_distance, layerMask, queryTriggerInteraction, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = hasHit ? 1 : 0;
            }
        }

        void RayCast()
        {
            if (wantedHits == WantedHits.all)
            {
                RaycastHit[] hitInfos = DrawPhysics.RaycastAll(GetDrawPos3D_global(), Get_customVector3_1_inGlobalSpaceUnits(), used_distance, layerMask, queryTriggerInteraction, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = (hitInfos != null) ? hitInfos.Length : 0;
            }
            else
            {
                bool hasHit = DrawPhysics.Raycast(GetDrawPos3D_global(), Get_customVector3_1_inGlobalSpaceUnits(), used_distance, layerMask, queryTriggerInteraction, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = hasHit ? 1 : 0;
            }
        }

        void Box_checkRespOverlap(Vector3 center, Vector3 halfExtents, Quaternion orientation)
        {
            if (wantedHits == WantedHits.all)
            {
                Collider[] overlappingColliders = DrawPhysics.OverlapBox(center, halfExtents, orientation, layerMask, queryTriggerInteraction, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = (overlappingColliders != null) ? overlappingColliders.Length : 0;
            }
            else
            {
                bool doesOverlap = DrawPhysics.CheckBox(center, halfExtents, orientation, layerMask, queryTriggerInteraction, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = doesOverlap ? 1 : 0;
            }
        }

        void Sphere_checkRespOverlap(Vector3 position, float radius)
        {
            if (wantedHits == WantedHits.all)
            {
                Collider[] overlappingColliders = DrawPhysics.OverlapSphere(position, radius, layerMask, queryTriggerInteraction, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = (overlappingColliders != null) ? overlappingColliders.Length : 0;
            }
            else
            {
                bool doesOverlap = DrawPhysics.CheckSphere(position, radius, layerMask, queryTriggerInteraction, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = doesOverlap ? 1 : 0;
            }
        }

        void Capsule_checkRespOverlap(Vector3 point0, Vector3 point1, float radius)
        {
            if (wantedHits == WantedHits.all)
            {
                Collider[] overlappingColliders = DrawPhysics.OverlapCapsule(point0, point1, radius, layerMask, queryTriggerInteraction, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = (overlappingColliders != null) ? overlappingColliders.Length : 0;
            }
            else
            {
                bool doesOverlap = DrawPhysics.CheckCapsule(point0, point1, radius, layerMask, queryTriggerInteraction, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                numberOfFoundHits = doesOverlap ? 1 : 0;
            }
        }

        public static DrawPhysics.VisualizationQuality Map_saveDrawnLinesType_to_visualizationQuality(SaveDrawnLinesType saveDrawnLinesType_toMap)
        {
            switch (saveDrawnLinesType_toMap)
            {
                case SaveDrawnLinesType.no_useFullDetails:
                    return DrawPhysics.VisualizationQuality.high_withFullDetails;
                case SaveDrawnLinesType.useAMidwayTradeoff:
                    return DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes;
                case SaveDrawnLinesType.yes_displayWithLowDetails:
                    return DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes;
                default:
                    return DrawPhysics.VisualizationQuality.high_withFullDetails;
            }
        }

        public static SaveDrawnLinesType Map_visualizationQuality_to_saveDrawnLinesType(DrawPhysics.VisualizationQuality visualizationQuality_toMap)
        {
            switch (visualizationQuality_toMap)
            {
                case DrawPhysics.VisualizationQuality.high_withFullDetails:
                    return SaveDrawnLinesType.no_useFullDetails;
                case DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes:
                    return SaveDrawnLinesType.useAMidwayTradeoff;
                case DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes:
                    return SaveDrawnLinesType.yes_displayWithLowDetails;
                default:
                    return SaveDrawnLinesType.no_useFullDetails;
            }
        }

        static Vector2 GetInitialValueFor_customDirectionForHitResultText()
        {
            if (UtilitiesDXXL_Math.IsDefaultVector(DrawPhysics.directionOfHitResultText))
            {
                return DrawBasics.Default_textOffsetDirection_forPointTags;
            }
            else
            {
                return DrawPhysics.directionOfHitResultText;
            }
        }

    }

}
