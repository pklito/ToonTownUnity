namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/2D/Text Drawer 2D")]
    public class TextDrawer2D : TextDrawer
    {
        public override void InitializeValues_onceInComponentLifetime()
        {
            if (text_exclGlobalMarkupTags == null || text_exclGlobalMarkupTags == "")
            {
                text_exclGlobalMarkupTags = "text to draw";
                text_inclGlobalMarkupTags = "text to draw";
            }
            textSection_isOutfolded = true;

            customVector2_1_picker_isOutfolded = false;
            source_ofCustomVector2_1 = CustomVector2Source.transformsRight;
            customVector2_1_clipboardForManualInput = Vector2.right;
            vectorInterpretation_ofCustomVector2_1 = VectorInterpretation.globalSpace;
        }

        public override void DrawVisualizedObject()
        {
            CacheSizeScaleFactors("Text Drawer 2D Component");
            float used_size = Get_used_size();
            if (text_inclGlobalMarkupTags != null && text_inclGlobalMarkupTags != "")
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(used_size) == false)
                {
                    GetScaledTextBlockConstraintValues(out float used_forceTextEnlargementToThisMinWidth_value, out float used_forceRestrictTextSizeToThisMaxTextWidth_value, out float used_autoLineBreakWidth_value);
                    Vector2 textDir = Get_customVector2_1_inGlobalSpaceUnits();
                    UtilitiesDXXL_Text.Write2DFramed(text_inclGlobalMarkupTags, GetDrawPos2D_global(), color, used_size, textDir, textAnchor, GetZPos_global_for2D(), enclosingBoxLineStyle, enclosingBox_lineWidth_relToTextSize, enclosingBox_paddingSize_relToTextSize, used_forceTextEnlargementToThisMinWidth_value, used_forceRestrictTextSizeToThisMaxTextWidth_value, used_autoLineBreakWidth_value, autoFlipToPreventMirrorInverted, 0.0f, hiddenByNearerObjects);
                }
            }
        }

        public override float Get_biggestAbsGlobalSizeComponentOfTransform()
        {
            return UtilitiesDXXL_Math.GetBiggestAbsComponent_ignoringZ(transform.lossyScale);
        }

        public override Vector3 Get_used_drawPos3D_global()
        {
            return GetDrawPos3D_ofA2DModeTransform_global();
        }

    }

}
