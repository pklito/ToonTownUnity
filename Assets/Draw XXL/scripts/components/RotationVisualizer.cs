namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Rotation Visualizer")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class RotationVisualizer : VisualizerParent
    {
        public enum RotationType { quaternionGlobal, quaternionLocal, eulerAnglesGlobal, eulerAnglesLocal };
        [SerializeField] RotationType rotationType = RotationType.quaternionGlobal;
        RotationType rotationType_before = RotationType.quaternionGlobal;

        [SerializeField] Color color_ofTurnAxis = DrawBasics.defaultColor;
        [SerializeField] [Range(0.008f, 0.3f)] float lineWidth = 0.008f;

        [SerializeField] float length_ofUpAndForwardVectors_caseQuaternion = 1.0f;
        [SerializeField] float length_ofUpAndForwardVectors_caseEuler = 0.0f;

        //custom rotated vector
        [SerializeField] bool drawCustomRotatedVector_caseQuaternion = false;
        [SerializeField] bool drawCustomRotatedVector_caseEuler = true;

        [SerializeField] [Range(0.0f, 1.0f)] float alpha_ofSquareSpannedByForwardAndUp = 0.45f;
        [SerializeField] public bool useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay = false;
        [SerializeField] [Range(0.0f, 1.0f)] float alpha_ofUnrotatedGimbalAxes = 0.06f;
        [SerializeField] public float gimbalSize = 1.0f;

        public override void InitializeValues_onceInComponentLifetime()
        {
            TrySetTextToGameobjectName();
            hiddenByNearerObjects = false;

            source_ofCustomVector3_1 = CustomVector3Source.manualInput;
            customVector3_1_clipboardForManualInput = Vector3.one;
            vectorInterpretation_ofCustomVector3_1 = VectorInterpretation.globalSpace;
        }

        public override void DrawVisualizedObject()
        {
            TryChangeCustomVectorInterpretation();
            switch (rotationType)
            {
                case RotationType.quaternionGlobal:
                    DrawQuaternion();
                    break;
                case RotationType.quaternionLocal:
                    DrawQuaternionLocal();
                    break;
                case RotationType.eulerAnglesGlobal:
                    DrawEuler();
                    break;
                case RotationType.eulerAnglesLocal:
                    DrawEulerLocal();
                    break;
                default:
                    break;
            }
        }

        void DrawQuaternion()
        {
            if (drawCustomRotatedVector_caseQuaternion)
            {
                DrawEngineBasics.QuaternionRotation(this.gameObject.transform.rotation, GetDrawPos3D_global(), Get_customVector3_1_inGlobalSpaceUnits(), length_ofUpAndForwardVectors_caseQuaternion, color_ofTurnAxis, lineWidth, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
            }
            else
            {
                DrawEngineBasics.QuaternionRotation(this.gameObject.transform.rotation, GetDrawPos3D_global(), default(Vector3), length_ofUpAndForwardVectors_caseQuaternion, color_ofTurnAxis, lineWidth, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
            }
        }

        void DrawQuaternionLocal()
        {
            if (drawCustomRotatedVector_caseQuaternion)
            {
                DrawEngineBasics.QuaternionRotation_local(this.transform.parent, this.gameObject.transform.localRotation, GetDrawPos3D_global(), Get_customVector3_1_inLocalSpaceDefinedByParentUnits(), length_ofUpAndForwardVectors_caseQuaternion, color_ofTurnAxis, lineWidth, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
            }
            else
            {
                DrawEngineBasics.QuaternionRotation_local(this.transform.parent, this.gameObject.transform.localRotation, GetDrawPos3D_global(), default(Vector3), length_ofUpAndForwardVectors_caseQuaternion, color_ofTurnAxis, lineWidth, text_inclGlobalMarkupTags, 0.0f, hiddenByNearerObjects);
            }
        }

        void DrawEuler()
        {
            string text_inclGlobalMarkupTags_extended;
            bool used_useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay;
            if (useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay == false)
            {
                if (transform.parent != null)
                {
                    text_inclGlobalMarkupTags_extended = "[<color=#adadadFF><icon=logMessage></color> The transform has a parent, but the angle values from the inspector show the local rotation<br>   -> fallback to values from transform.eulerAngles<br>   -> for further infos see documentation of 'useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay']<br>" + text_inclGlobalMarkupTags;
                    used_useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay = true;
                }
                else
                {
                    text_inclGlobalMarkupTags_extended = text_inclGlobalMarkupTags;
                    used_useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay = useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay;
                }
            }
            else
            {
                text_inclGlobalMarkupTags_extended = text_inclGlobalMarkupTags;
                used_useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay = useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay;
            }

            Vector3 eulerAnglesToDraw = UtilitiesDXXL_Euler.GetEulerAnglesFromNonNullTransform(this.transform, used_useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay, false);
            if (drawCustomRotatedVector_caseEuler)
            {
                DrawEngineBasics.EulerRotation(eulerAnglesToDraw, GetDrawPos3D_global(), Get_customVector3_1_inGlobalSpaceUnits(), length_ofUpAndForwardVectors_caseEuler, alpha_ofSquareSpannedByForwardAndUp, text_inclGlobalMarkupTags_extended, alpha_ofUnrotatedGimbalAxes, gimbalSize, 0.0f, hiddenByNearerObjects);
            }
            else
            {
                DrawEngineBasics.EulerRotation(eulerAnglesToDraw, GetDrawPos3D_global(), default(Vector3), length_ofUpAndForwardVectors_caseEuler, alpha_ofSquareSpannedByForwardAndUp, text_inclGlobalMarkupTags_extended, alpha_ofUnrotatedGimbalAxes, gimbalSize, 0.0f, hiddenByNearerObjects);
            }
        }

        void DrawEulerLocal()
        {
            Vector3 eulerAnglesToDraw_local = UtilitiesDXXL_Euler.GetEulerAnglesFromNonNullTransform(this.transform, useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay, true);
            if (drawCustomRotatedVector_caseEuler)
            {
                DrawEngineBasics.EulerRotation_local(this.transform.parent, eulerAnglesToDraw_local, GetDrawPos3D_global(), Get_customVector3_1_inLocalSpaceDefinedByParentUnits(), length_ofUpAndForwardVectors_caseEuler, alpha_ofSquareSpannedByForwardAndUp, text_inclGlobalMarkupTags, alpha_ofUnrotatedGimbalAxes, gimbalSize, 0.0f, hiddenByNearerObjects);
            }
            else
            {
                DrawEngineBasics.EulerRotation_local(this.transform.parent, eulerAnglesToDraw_local, GetDrawPos3D_global(), default(Vector3), length_ofUpAndForwardVectors_caseEuler, alpha_ofSquareSpannedByForwardAndUp, text_inclGlobalMarkupTags, alpha_ofUnrotatedGimbalAxes, gimbalSize, 0.0f, hiddenByNearerObjects);
            }
        }

        void TryChangeCustomVectorInterpretation()
        {
            //-> this implementation is not fit for Unitys build-in undo-functionality
            if (rotationType_before != rotationType)
            {
                switch (rotationType)
                {
                    case RotationType.quaternionGlobal:
                        vectorInterpretation_ofCustomVector3_1 = VectorInterpretation.globalSpace;
                        break;
                    case RotationType.quaternionLocal:
                        vectorInterpretation_ofCustomVector3_1 = VectorInterpretation.localSpaceDefinedByParent;
                        break;
                    case RotationType.eulerAnglesGlobal:
                        vectorInterpretation_ofCustomVector3_1 = VectorInterpretation.globalSpace;
                        break;
                    case RotationType.eulerAnglesLocal:
                        vectorInterpretation_ofCustomVector3_1 = VectorInterpretation.localSpaceDefinedByParent;
                        break;
                    default:
                        break;
                }
            }
            rotationType_before = rotationType;
        }

    }

}
