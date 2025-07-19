namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Position Visualizer")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class PositionVisualizer : VisualizerParent
    {

        [SerializeField] bool global = false;
        [SerializeField] bool local = true;
        [SerializeField] bool allParents = false;

        [SerializeField] Color color = DrawBasics.defaultColor;
        [SerializeField] [Range(0.0f, 2.0f)] float lineWidth = 0.0f;

        public override void InitializeValues_onceInComponentLifetime()
        {
            if (text_exclGlobalMarkupTags == null || text_exclGlobalMarkupTags == "")
            {
                text_exclGlobalMarkupTags = "to position of<br>" + this.gameObject.name;
                text_inclGlobalMarkupTags = "to position of<br>" + this.gameObject.name;
            }
            hiddenByNearerObjects = false;
        }

        public override void InitializeValues_alsoOnPlaymodeEnter_andOnComponentCreatedAsCopy()
        {
            if (transform.parent == null)
            {
                global = true;
                local = false;
            }
        }

        public override void DrawVisualizedObject()
        {
            if (global)
            {
                DrawEngineBasics.Position(transform.position, color, lineWidth, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
            }

            if (local)
            {
                if (transform.parent != null)
                {
                    DrawEngineBasics.Position_local(transform.parent, transform.localPosition, color, lineWidth, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
                }
            }

            if (allParents)
            {
                if (transform.parent != null)
                {
                    Transform[] transformsOfParents = transform.parent.GetComponentsInParent<Transform>(true);
                    if (transformsOfParents != null)
                    {
                        for (int i = 0; i < transformsOfParents.Length; i++)
                        {
                            if (transformsOfParents[i] != null)
                            {
                                if (transformsOfParents[i].parent == null)
                                {
                                    DrawEngineBasics.Position(transformsOfParents[i].position, color, lineWidth, null, 0.0f, hiddenByNearerObjects);
                                }
                                else
                                {
                                    DrawEngineBasics.Position_local(transformsOfParents[i].parent, transformsOfParents[i].localPosition, color, lineWidth, null, 0.0f, hiddenByNearerObjects);
                                }
                            }
                        }
                    }
                }
            }
        }

    }

}
