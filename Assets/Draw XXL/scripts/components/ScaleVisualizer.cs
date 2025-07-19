namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Scale Visualizer")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class ScaleVisualizer : VisualizerParent
    {
        public enum ScaleType { local, global };
        [SerializeField] ScaleType scaleType;
        [SerializeField] [Range(0.0f, 0.5f)] float lineWidth = 0.0035f;
        [SerializeField] bool drawXDim = true;
        [SerializeField] bool drawYDim = true;
        [SerializeField] bool drawZDim = true;
        [SerializeField] [Range(0.0f, 1.0f)] float relSizeOfPlanes = 0.5f;
        [SerializeField] bool force_overwriteColor = false;
        [SerializeField] Color overwriteColor = DrawBasics.defaultColor;

        public override void InitializeValues_onceInComponentLifetime()
        {
            TrySetTextToGameobjectName();
            hiddenByNearerObjects = false;
        }

        public override void DrawVisualizedObject()
        {
            SetScaleType();
            Color used_color = force_overwriteColor ? overwriteColor : default(Color);
            switch (scaleType)
            {
                case ScaleType.local:
                    DrawEngineBasics.LocalScale(GetDrawPos3D_inLocalSpaceAsDefinedByParent(), transform.localScale, transform.parent, transform.localRotation, lineWidth, text_inclGlobalMarkupTags, drawXDim, drawYDim, drawZDim, relSizeOfPlanes, used_color, 0.0f, hiddenByNearerObjects);
                    break;
                case ScaleType.global:
                    DrawEngineBasics.Scale(GetDrawPos3D_global(), transform.lossyScale, lineWidth, text_inclGlobalMarkupTags, transform.rotation, drawXDim, drawYDim, drawZDim, relSizeOfPlanes, used_color, 0.0f, hiddenByNearerObjects);
                    break;
                default:
                    break;
            }
        }

        void SetScaleType()
        {
            if (transform.parent == null)
            {
                scaleType = ScaleType.global;
            }
            else
            {
                scaleType = ScaleType.local;
            }
        }

    }

}
