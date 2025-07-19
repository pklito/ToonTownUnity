namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Screenspace/Camera Grid Visualizer")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class CameraGridVisualizer : VisualizerScreenspaceParent
    {
        [SerializeField] Color color = DrawBasics.defaultColor;
        [SerializeField] [Range(0.0f, 0.1f)] float linesWidth_relToViewportHeight = 0.0f;
        [SerializeField] bool drawTenthLines = true;
        [SerializeField] bool drawHundredthLines = true;
        [SerializeField] DrawEngineBasics.GridScreenspaceMode gridScreenspaceMode = DrawEngineBasics.GridScreenspaceMode.warpWidthAndHeightIndividuallyToFitScreenInBothAxes;

        public override void InitializeValues_onceInComponentLifetime()
        {
            TrySetTextToEmptyString();
        }

        public override void InitializeValues_alsoOnPlaymodeEnter_andOnComponentCreatedAsCopy()
        {
            TryFetchCamOnThisGO_andDecideScreenspaceDefiningCamera();
        }

        public override void DrawVisualizedObject()
        {
            Camera usedCamera = Get_usedCamera("Camera Grid Visualizer Component");
            if (usedCamera != null)
            {
                DrawEngineBasics.GridScreenspace(usedCamera, color, linesWidth_relToViewportHeight, drawTenthLines, drawHundredthLines, gridScreenspaceMode, 0.0f);
            }
        }

    }

}
