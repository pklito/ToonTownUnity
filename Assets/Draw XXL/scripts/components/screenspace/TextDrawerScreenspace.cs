namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Screenspace/Text Drawer Screenspace")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class TextDrawerScreenspace : VisualizerScreenspaceParent
    {
        [SerializeField] Color color = DrawBasics.defaultColor;
        [SerializeField] [Range(0.001f, 1.0f)] float size_relToViewportHeight = 0.025f;
        [SerializeField] DrawText.TextAnchorDXXL textAnchor = DrawText.TextAnchorDXXL.LowerLeft;
        [SerializeField] public bool enclosingBox_isOutfolded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] DrawBasics.LineStyle enclosingBoxLineStyle = DrawBasics.LineStyle.invisible;
        [SerializeField] float enclosingBox_lineWidth_relToTextSize = 0.0f;
        [SerializeField] float enclosingBox_paddingSize_relToTextSize = 0.0f;
        [SerializeField] bool autoLineBreakAtScreenBorder = true;
        [SerializeField] bool autoFlipTextToPreventUpsideDown = true;

        [SerializeField] public bool forceTextEnlargementToThisMinWidth_relToViewportWidth_isOutfolded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] bool forceTextEnlargementToThisMinWidth_relToViewportWidth = false;
        [SerializeField] [Range(0.003f, 2.0f)] float forceTextEnlargementToThisMinWidth_relToViewportWidth_value = 0.1f;

        [SerializeField] public bool forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth_isOutfolded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] bool forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth = false;
        [SerializeField] [Range(0.003f, 2.0f)] float forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth_value = 0.5f;

        [SerializeField] public bool autoLineBreakWidth_relToViewportWidth_isOutfolded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] bool autoLineBreakWidth_relToViewportWidth = false;
        [SerializeField] [Range(0.003f, 2.0f)] float autoLineBreakWidth_relToViewportWidth_value = 0.5f;

        public override void InitializeValues_onceInComponentLifetime()
        {
            if (text_exclGlobalMarkupTags == null || text_exclGlobalMarkupTags == "")
            {
                text_exclGlobalMarkupTags = "text to draw";
                text_inclGlobalMarkupTags = "text to draw";
            }
            textSection_isOutfolded = true;

            customVector2_1_picker_isOutfolded = false;
            source_ofCustomVector2_1 = CustomVector2Source.rotationAroundZStartingFromRight;
            customVector2_1_clipboardForManualInput = Vector2.right;
            vectorInterpretation_ofCustomVector2_1 = VectorInterpretation.globalSpace;
        }

        public override void InitializeValues_alsoOnPlaymodeEnter_andOnComponentCreatedAsCopy()
        {
            TryFetchCamOnThisGO_andDecideScreenspaceDefiningCamera();
        }

        public override void DrawVisualizedObject()
        {
            Camera usedCamera = Get_usedCamera("Text Drawer Screenspace Component");
            if (usedCamera != null)
            {
                if (text_inclGlobalMarkupTags != null && text_inclGlobalMarkupTags != "")
                {
                    if (UtilitiesDXXL_Math.ApproximatelyZero(size_relToViewportHeight) == false)
                    {
                        float used_forceTextEnlargementToThisMinWidth_relToViewportWidth_value = Get_used_forceTextEnlargementToThisMinWidth_relToViewportWidth_value();
                        float used_forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth_value = Get_used_forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth_value();
                        float used_autoLineBreakWidth_relToViewportWidth_value = Get_used_autoLineBreakWidth_relToViewportWidth_value();
                        Vector2 textDir = Get_customVector2_1_inGlobalSpaceUnits();
                        UtilitiesDXXL_Text.WriteScreenSpace(usedCamera, text_inclGlobalMarkupTags, positionInsideViewport0to1, color, size_relToViewportHeight, textDir, textAnchor, enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, used_forceTextEnlargementToThisMinWidth_relToViewportWidth_value, used_forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth_value, autoLineBreakAtScreenBorder, used_autoLineBreakWidth_relToViewportWidth_value, autoFlipTextToPreventUpsideDown, 0.0f, false);
                    }
                }
            }
        }

        public float Get_used_forceTextEnlargementToThisMinWidth_relToViewportWidth_value()
        {
            if (forceTextEnlargementToThisMinWidth_relToViewportWidth)
            {
                return forceTextEnlargementToThisMinWidth_relToViewportWidth_value;
            }
            else
            {
                return 0.0f;
            }
        }

        public float Get_used_forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth_value()
        {
            if (forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth)
            {
                return forceRestrictTextSizeToThisMaxTextWidth_relToViewportWidth_value;
            }
            else
            {
                return 0.0f;
            }
        }

        public float Get_used_autoLineBreakWidth_relToViewportWidth_value()
        {
            if (autoLineBreakWidth_relToViewportWidth)
            {
                return autoLineBreakWidth_relToViewportWidth_value;
            }
            else
            {
                return 0.0f;
            }
        }

    }

}
