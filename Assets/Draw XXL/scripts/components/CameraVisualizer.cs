namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Camera Visualizer")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class CameraVisualizer : VisualizerScreenspaceParent
    {
        static Color initialCameraColor = new Color(0.96f, 0.9f, 0.24f, 1.0f);

        //symmetric fields for both:
        [SerializeField] bool drawCamera = true;
        [SerializeField] bool drawFrustum = true;
        [SerializeField] Color color_ofCamera_enabledCam = initialCameraColor;
        [SerializeField] Color color_ofCamera_disabledCam = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(initialCameraColor, 0.2f);
        [SerializeField] Color color_ofFrustum_enabledCam = DrawBasics.defaultColor;
        [SerializeField] Color color_ofFrustum_disabledCam = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawBasics.defaultColor, 0.1f);
        [SerializeField] float linesWidth_camera = 0.0f;
        [SerializeField] float linesWidth_frustum = 0.0f;

        //only for frustum:
        [SerializeField] [Range(0.0f, 1.0f)] float alphaFactor_forBoundarySurfaceLines = 0.18f;
        [SerializeField] int linesPerBoundarySurface = 60;
        [SerializeField] bool forceTextOnNearPlaneUnmirroredTowardsCam = true;
        [SerializeField] float distanceOfHighlightedPlane = 0.0f;
        [SerializeField] float distanceOfHighlightedPlane_offsetFromPosition = 0.0f;
        [SerializeField] bool drawFrustumsHighlightedPlaneAlsoIfFarerThanFarClipPlane = true;
        [SerializeField] bool doOverwriteColorForFrustumsHighlightedPlane = false;
        [SerializeField] Color overwriteColorForFrustumsHighlightedPlane; //-> not using "DrawPhysics.overwriteColorForCastsHitNormals", since this would be the default color that doesn't represent what the user sees as normal color in the Scene

        public enum HighlightedPlaneDefintionType { disabled, definedByDistanceFromCamera, definedByAPosition };
        [SerializeField] HighlightedPlaneDefintionType highlightedPlaneDefintionType = HighlightedPlaneDefintionType.disabled;

        public enum HighlightedPlaneViaPosDefintionType { fixedPosition, gameobject };
        [SerializeField] HighlightedPlaneViaPosDefintionType highlightedPlaneViaPosDefintionType = HighlightedPlaneViaPosDefintionType.fixedPosition;

        [SerializeField] Vector3 vector3_thatSpecifiesThePosOfTheAdditionalFrustumPlane;
        [SerializeField] GameObject gameobject_thatSpecifiesThePosOfTheAdditionalFrustumPlane;


        public override void InitializeValues_onceInComponentLifetime()
        {
            TrySetTextToEmptyString();
            overwriteColorForFrustumsHighlightedPlane = UtilitiesDXXL_EngineBasics.Get_defaultColor_ofFrustumsHighlightedPlane(color_ofFrustum_enabledCam);
        }

        public override void InitializeValues_alsoOnPlaymodeEnter_andOnComponentCreatedAsCopy()
        {
            TryFetchCamOnThisGO_andDecideScreenspaceDefiningCamera();
        }

        public override void DrawVisualizedObject()
        {
            Camera usedCamera = Get_usedCamera("Camera Visualizer Component");
            if (usedCamera != null)
            {
                if (drawCamera)
                {
                    Color used_color = CheckIf_usedCameraIsActiveAndEnabled() ? color_ofCamera_enabledCam : color_ofCamera_disabledCam;
                    DrawEngineBasics.Camera(usedCamera, used_color, drawFrustum ? null : text_inclGlobalMarkupTags, linesWidth_camera, 0.0f, hiddenByNearerObjects);
                }

                if (drawFrustum) { DrawFrustum(usedCamera); }
            }
        }

        void DrawFrustum(Camera usedCamera)
        {
            Color used_color = CheckIf_usedCameraIsActiveAndEnabled() ? color_ofFrustum_enabledCam : color_ofFrustum_disabledCam;

            UtilitiesDXXL_EngineBasics.Set_drawFrustumsHighlightedPlaneAlsoIfFarerThanFarClipPlane_reversible(drawFrustumsHighlightedPlaneAlsoIfFarerThanFarClipPlane);
            if (doOverwriteColorForFrustumsHighlightedPlane)
            {
                UtilitiesDXXL_EngineBasics.Set_overwriteColorForFrustumsHighlightedPlane_reversible(overwriteColorForFrustumsHighlightedPlane);
            }

            switch (highlightedPlaneDefintionType)
            {
                case HighlightedPlaneDefintionType.disabled:
                    UtilitiesDXXL_EngineBasics.Set_distanceOfFrustumsHighlightedPlane_reversible(0.0f);
                    DrawEngineBasics.CameraFrustum(usedCamera, used_color, alphaFactor_forBoundarySurfaceLines, linesWidth_frustum, linesPerBoundarySurface, text_inclGlobalMarkupTags, forceTextOnNearPlaneUnmirroredTowardsCam, default(Vector3), 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_EngineBasics.Reverse_distanceOfFrustumsHighlightedPlane();
                    break;
                case HighlightedPlaneDefintionType.definedByDistanceFromCamera:
                    UtilitiesDXXL_EngineBasics.Set_distanceOfFrustumsHighlightedPlane_reversible(distanceOfHighlightedPlane);
                    DrawEngineBasics.CameraFrustum(usedCamera, used_color, alphaFactor_forBoundarySurfaceLines, linesWidth_frustum, linesPerBoundarySurface, text_inclGlobalMarkupTags, forceTextOnNearPlaneUnmirroredTowardsCam, default(Vector3), 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_EngineBasics.Reverse_distanceOfFrustumsHighlightedPlane();
                    break;
                case HighlightedPlaneDefintionType.definedByAPosition:
                    Vector3 positionOnHighlightedPlane;
                    bool skipDraw = false;
                    switch (highlightedPlaneViaPosDefintionType)
                    {
                        case HighlightedPlaneViaPosDefintionType.fixedPosition:
                            positionOnHighlightedPlane = vector3_thatSpecifiesThePosOfTheAdditionalFrustumPlane;
                            break;
                        case HighlightedPlaneViaPosDefintionType.gameobject:
                            if (gameobject_thatSpecifiesThePosOfTheAdditionalFrustumPlane != null)
                            {
                                positionOnHighlightedPlane = gameobject_thatSpecifiesThePosOfTheAdditionalFrustumPlane.transform.position;
                            }
                            else
                            {
                                positionOnHighlightedPlane = Vector3.zero;
                                skipDraw = true;
                            }
                            break;
                        default:
                            positionOnHighlightedPlane = Vector3.zero;
                            break;
                    }

                    positionOnHighlightedPlane = positionOnHighlightedPlane + usedCamera.transform.forward * distanceOfHighlightedPlane_offsetFromPosition;
                    if (UtilitiesDXXL_Math.IsDefaultVector(positionOnHighlightedPlane)) { positionOnHighlightedPlane = new Vector3(0.0f, 0.0f, 0.0001f); } //-> "DrawEngineBasics.CameraFrustum" would skip drawing the additional plane if the position remains at the default value of (0/0/0)
                    if (skipDraw) { positionOnHighlightedPlane = (-usedCamera.transform.forward) * 100000.0f; }

                    UtilitiesDXXL_EngineBasics.Set_distanceOfFrustumsHighlightedPlane_reversible(0.0f);
                    DrawEngineBasics.CameraFrustum(usedCamera, used_color, alphaFactor_forBoundarySurfaceLines, linesWidth_frustum, linesPerBoundarySurface, text_inclGlobalMarkupTags, forceTextOnNearPlaneUnmirroredTowardsCam, positionOnHighlightedPlane, 0.0f, hiddenByNearerObjects);
                    UtilitiesDXXL_EngineBasics.Reverse_distanceOfFrustumsHighlightedPlane();
                    break;
                default:
                    break;
            }

            UtilitiesDXXL_EngineBasics.Reverse_drawFrustumsHighlightedPlaneAlsoIfFarerThanFarClipPlane();
            if (doOverwriteColorForFrustumsHighlightedPlane)
            {
                UtilitiesDXXL_EngineBasics.Reverse_overwriteColorForFrustumsHighlightedPlane();
            }

        }

    }

}
