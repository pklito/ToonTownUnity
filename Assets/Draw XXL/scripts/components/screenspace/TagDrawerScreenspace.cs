namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Screenspace/Tag Drawer Screenspace")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class TagDrawerScreenspace : VisualizerScreenspaceParent
    {
        public enum TaggedPositionType { positionOnViewport, aGameobject, multipleGameobjects };
        [SerializeField] TaggedPositionType taggedPositionType = TaggedPositionType.positionOnViewport;
        public enum PointerDirectionSpecificationType { fixedAngle, vanishingPointPosition };
        [SerializeField] PointerDirectionSpecificationType pointerDirectionSpecificationType = PointerDirectionSpecificationType.vanishingPointPosition;

        //both types:
        [SerializeField] [Range(0.0f, 0.2f)] float linesWidth_relToViewportHeight = 0.0f;
        [SerializeField] Color colorForText = DrawBasics.defaultColor;
        [SerializeField] bool drawPointerIfOffscreen = true;

        //only for "positionOnViewport":
        [SerializeField] bool forceTextSize = false;
        [SerializeField] [Range(0.01f, 0.5f)] float forceTextSize_value = 0.1f;
        [SerializeField] bool skipConeDrawing = false;
        [SerializeField] bool addTextForOutsideDistance_toOffscreenPointer = true;
        static float default_textOffsetDistance_relToViewportHeight = 0.2f;
        [SerializeField] [Range(0.035f, 1.0f)] float textOffsetDistance_relToViewportHeight = default_textOffsetDistance_relToViewportHeight;
        [SerializeField] [Range(-360f, 360.0f)] float fixedPointerDiretion_angledDegCC = -30.0f;

        //only for "aGameobject":
        [SerializeField] bool differentBoxColor = false;
        [SerializeField] Color differentBoxColor_value = UtilitiesDXXL_Colors.violet;
        [SerializeField] bool encapsulateChildren = true;

        //only for "multipleGameobjects":
        [SerializeField] InternalDXXL_TaggedScreenspaceObject[] taggedScreenspaceObjects;

        public override void InitializeValues_onceInComponentLifetime()
        {
            if (text_exclGlobalMarkupTags == null || text_exclGlobalMarkupTags == "")
            {
                text_exclGlobalMarkupTags = "tag text";
                text_inclGlobalMarkupTags = "tag text";
            }
            textSection_isOutfolded = true;
            positionInsideViewport0to1 = new Vector2(0.35f, 0.35f);
            positionInsideViewport0to1_v2 = new Vector2(0.5f, 0.5f);
        }

        public override void InitializeValues_alsoOnPlaymodeEnter_andOnComponentCreatedAsCopy()
        {
            TryFetchCamOnThisGO_andDecideScreenspaceDefiningCamera();
        }

        public override void DrawVisualizedObject()
        {
            Camera usedCamera = Get_usedCamera("Tag Drawer Screenspace Component");
            if (usedCamera != null)
            {
                float used_relTextSizeScaling;
                switch (taggedPositionType)
                {
                    case TaggedPositionType.positionOnViewport:
                        used_relTextSizeScaling = Get_used_relTextSizeScaling();
                        Get_vanishingPointSpecs(out Vector2 textOffsetDir, out Vector2 customTowardsPoint_ofDefaultTextOffsetDir, usedCamera);
                        DrawScreenspace.PointTag(usedCamera, positionInsideViewport0to1, text_inclGlobalMarkupTags, null, colorForText, drawPointerIfOffscreen, linesWidth_relToViewportHeight, textOffsetDistance_relToViewportHeight, textOffsetDir, used_relTextSizeScaling, skipConeDrawing, addTextForOutsideDistance_toOffscreenPointer, 0.0f, customTowardsPoint_ofDefaultTextOffsetDir);
                        break;
                    case TaggedPositionType.aGameobject:
                        Color used_colorForBox = differentBoxColor ? differentBoxColor_value : colorForText;
                        TagAGameobject(usedCamera, partnerGameobject, text_inclGlobalMarkupTags, used_colorForBox);
                        break;
                    case TaggedPositionType.multipleGameobjects:
                        if (taggedScreenspaceObjects != null)
                        {
                            for (int i = 0; i < taggedScreenspaceObjects.Length; i++)
                            {
                                taggedScreenspaceObjects[i].TryUseSeededColorFromGameobjectID();
                                TagAGameobject(usedCamera, taggedScreenspaceObjects[i].gameobject, taggedScreenspaceObjects[i].text, taggedScreenspaceObjects[i].color);
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        void TagAGameobject(Camera usedCamera, GameObject gameobjectToTag, string textAtGameobject, Color used_colorForBox)
        {
            if (gameobjectToTag != null)
            {
                //float used_relTextSizeScaling = forceTextSize_value; //"TagGameObjectScreenspace()" uses a fixed text size indepentent of the box size. So an "absolute" forceTextSize_value can be used here as "relative scaler", because "relative" already means "relative to viewport height". And that is an "absolute" size in the sense, that it is "absoulte in viewportSpaceUnits" and not "relative to boxSize/pointerLength".
                float used_relTextSizeScaling = 1.0f; //In the "aGameobject"-case the text size is not scaled via the "relTextSizeScaling" parameter but via "Text/Style/Size scaling"
                DrawEngineBasics.TagGameObjectScreenspace(usedCamera, gameobjectToTag, textAtGameobject, colorForText, used_colorForBox, linesWidth_relToViewportHeight, drawPointerIfOffscreen, used_relTextSizeScaling, encapsulateChildren, 0.0f);
            }
        }

        float Get_used_relTextSizeScaling()
        {
            //This relative scaler is used to actually produce an "absolute" text size ("absoulte in viewportSpaceUnits")
            if (forceTextSize)
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(textOffsetDistance_relToViewportHeight) == false)
                {
                    return ((forceTextSize_value * default_textOffsetDistance_relToViewportHeight) / (textOffsetDistance_relToViewportHeight * UtilitiesDXXL_DrawBasics.pointTagsTextSize_relToOffset));
                }
            }
            return 1.0f;
        }

        void Get_vanishingPointSpecs(out Vector2 textOffsetDir, out Vector2 customTowardsPoint_ofDefaultTextOffsetDir, Camera usedCamera)
        {
            switch (pointerDirectionSpecificationType)
            {
                case PointerDirectionSpecificationType.fixedAngle:
                    Quaternion rotation_fromUp = Quaternion.AngleAxis(fixedPointerDiretion_angledDegCC, Vector3.forward);
                    Vector3 textOffsetDir_asV3 = rotation_fromUp * Vector3.up;
                    Vector2 textOffsetDir_inUnwarpedSpace = (new Vector2(textOffsetDir_asV3.x, textOffsetDir_asV3.y));
                    Vector2 textOffsetDir_inWarpedSpace = DrawScreenspace.DirectionInUnitsOfUnwarpedSpace_to_sameLookingDirectionInUnitsOfWarpedSpace(textOffsetDir_inUnwarpedSpace, usedCamera);
                    textOffsetDir = textOffsetDir_inWarpedSpace;
                    customTowardsPoint_ofDefaultTextOffsetDir = default(Vector2);
                    break;
                case PointerDirectionSpecificationType.vanishingPointPosition:
                    textOffsetDir = default(Vector2);
                    customTowardsPoint_ofDefaultTextOffsetDir = positionInsideViewport0to1_v2;
                    break;
                default:
                    textOffsetDir = default(Vector2);
                    customTowardsPoint_ofDefaultTextOffsetDir = default(Vector2);
                    break;
            }
        }

    }

}
