namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Coordinate Axes Gizmo Visualizer")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class CoordinateAxesGizmoVisualizer : VisualizerParent
    {
        public enum VisualizedSpace { global, localDefinedByParent, localDefinedByThis };
        [SerializeField] VisualizedSpace visualizedSpace = VisualizedSpace.global;
        [SerializeField] bool drawXYZchars = true;
        [SerializeField] bool skipConeDrawing = false;
        [SerializeField] bool forceAllAxesLength = false;
        [SerializeField] float forceAllAxesLength_lengthValue = 1.0f;
        [SerializeField] [Range(0.0f, 1.0f)] float lineWidth = 0.025f;

        public override void InitializeValues_onceInComponentLifetime()
        {
            TrySetTextToEmptyString();
        }

        public override void DrawVisualizedObject()
        {
            float used_forceAxesLength_forLocal = forceAllAxesLength ? forceAllAxesLength_lengthValue : 0.0f;
            bool aParentHasANonUniformScale;
            switch (visualizedSpace)
            {
                case VisualizedSpace.global:
                    DrawGizmoForGlobalSpace();
                    break;
                case VisualizedSpace.localDefinedByParent:
                    if (transform.parent == null)
                    {
                        DrawGizmoForGlobalSpace();
                    }
                    else
                    {
                        aParentHasANonUniformScale = UtilitiesDXXL_EngineBasics.CheckIf_transformOrAParentHasNonUniformScale(transform.parent.parent);
                        UtilitiesDXXL_EngineBasics.CoordinateAxesGizmoLocal(GetDrawPos3D_global(), transform.parent.rotation, transform.parent.lossyScale, used_forceAxesLength_forLocal, lineWidth, text_inclGlobalMarkupTags, drawXYZchars, skipConeDrawing, 0.0f, hiddenByNearerObjects, aParentHasANonUniformScale);
                    }
                    break;
                case VisualizedSpace.localDefinedByThis:
                    aParentHasANonUniformScale = UtilitiesDXXL_EngineBasics.CheckIf_transformOrAParentHasNonUniformScale(transform.parent);
                    UtilitiesDXXL_EngineBasics.CoordinateAxesGizmoLocal(GetDrawPos3D_global(), transform.rotation, transform.lossyScale, used_forceAxesLength_forLocal, lineWidth, text_inclGlobalMarkupTags, drawXYZchars, skipConeDrawing, 0.0f, hiddenByNearerObjects, aParentHasANonUniformScale);
                    break;
                default:
                    break;
            }
        }

        void DrawGizmoForGlobalSpace()
        {
            UtilitiesDXXL_EngineBasics.CoordinateAxesGizmoLocal(GetDrawPos3D_global(), Quaternion.identity, default(Vector3), forceAllAxesLength_lengthValue, lineWidth, text_inclGlobalMarkupTags, drawXYZchars, skipConeDrawing, 0.0f, hiddenByNearerObjects, false);
        }
    }

}
