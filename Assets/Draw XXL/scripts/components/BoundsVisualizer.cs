namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Bounds Visualizer")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class BoundsVisualizer : VisualizerParent
    {
        public enum AttachedTextsizeReferenceContext { extentOfBounds, globalSpace, sceneViewWindowSize, gameViewWindowSize };
        [SerializeField] AttachedTextsizeReferenceContext attachedTextsizeReferenceContext = AttachedTextsizeReferenceContext.sceneViewWindowSize;
        [SerializeField] float textSize_value = 0.1f;
        [SerializeField] [Range(0.001f, 0.2f)] float textSize_value_relToScreen = 0.01f;

        [SerializeField] bool global = true;
        [SerializeField] bool local = true;

        [SerializeField] bool includeChildren = true;
        [SerializeField] [Range(0.0f, 0.5f)] float lineWidth = 0.01f;
        [SerializeField] Color color = new Color(1.0f, 0.7954724f, 0.3254902f, 1.0f);

        public override void InitializeValues_onceInComponentLifetime()
        {
            if (text_exclGlobalMarkupTags == null || text_exclGlobalMarkupTags == "")
            {
                text_exclGlobalMarkupTags = this.gameObject.name + " / children";
                text_inclGlobalMarkupTags = this.gameObject.name + " / children";
            }
        }

        public override void DrawVisualizedObject()
        {
            Set_globalTextSizeSpecs_reversible();
            if (global)
            {
                if (local)
                {
                    DrawEngineBasics.Bounds(this.gameObject, color, true, includeChildren, lineWidth, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                }
                else
                {
                    DrawEngineBasics.Bounds(this.gameObject, color, false, includeChildren, lineWidth, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                }
            }
            else
            {
                if (local)
                {
                    DrawEngineBasics.LocalBounds(this.gameObject, color, includeChildren, lineWidth, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                }
            }
            Reverse_globalTextSizeSpecs();
        }

        float forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_before;
        float forcedConstantWorldspaceTextSize_forTextAtShapes_before;
        DrawBasics.CameraForAutomaticOrientation cameraForAutomaticOrientation_before;
        void Set_globalTextSizeSpecs_reversible()
        {
            forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_before = DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes;
            forcedConstantWorldspaceTextSize_forTextAtShapes_before = DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes;
            cameraForAutomaticOrientation_before = DrawBasics.cameraForAutomaticOrientation;

            switch (attachedTextsizeReferenceContext)
            {
                case AttachedTextsizeReferenceContext.extentOfBounds:
                    DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = 0.0f;
                    DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = 0.0f;
                    break;
                case AttachedTextsizeReferenceContext.globalSpace:
                    DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = 0.0f;
                    DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = textSize_value;
                    break;
                case AttachedTextsizeReferenceContext.sceneViewWindowSize:
                    DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = textSize_value_relToScreen;
                    DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = 0.0f;
                    DrawBasics.cameraForAutomaticOrientation = DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;
                    break;
                case AttachedTextsizeReferenceContext.gameViewWindowSize:
                    DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = textSize_value_relToScreen;
                    DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = 0.0f;
                    DrawBasics.cameraForAutomaticOrientation = DrawBasics.CameraForAutomaticOrientation.gameViewCamera;
                    break;
                default:
                    break;
            }
        }

        void Reverse_globalTextSizeSpecs()
        {
            DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_before;
            DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes = forcedConstantWorldspaceTextSize_forTextAtShapes_before;
            DrawBasics.cameraForAutomaticOrientation = cameraForAutomaticOrientation_before;
        }

    }

}
