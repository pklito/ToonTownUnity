namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Cross Product Visualizer")]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class CrossProductVisualizer : VisualizerParent
    {
        [SerializeField] [Range(0.0f, 0.1f)] float linesWidth = 0.0025f;
        [SerializeField] public bool colorSection_isOutfolded = false;
        [SerializeField] Color colorOfVector1_forCrossProduct = DrawEngineBasics.colorOfVector1_forCrossProduct;
        [SerializeField] Color colorOfVector2_forCrossProduct = DrawEngineBasics.colorOfVector2_forCrossProduct;
        [SerializeField] Color colorOfAngle_forCrossProduct = DrawEngineBasics.colorOfAngle_forCrossProduct;
        [SerializeField] Color colorOfResultVector_forCrossProduct = DrawEngineBasics.colorOfResultVector_forCrossProduct;
        [SerializeField] Color colorOfResultText_forCrossProduct = DrawEngineBasics.colorOfResultText_forCrossProduct;
        [SerializeField] Color colorOfResultPlane_forCrossProduct = DrawEngineBasics.colorOfResultPlane_forCrossProduct;

        public override void InitializeValues_onceInComponentLifetime()
        {
            TrySetTextToGameobjectName();

            customVector3_1_picker_isOutfolded = true;
            source_ofCustomVector3_1 = CustomVector3Source.transformsForward;
            customVector3_1_clipboardForManualInput = Vector3.forward;
            vectorInterpretation_ofCustomVector3_1 = VectorInterpretation.globalSpace;

            customVector3_2_picker_isOutfolded = true;
            source_ofCustomVector3_2 = CustomVector3Source.manualInput;
            customVector3_2_clipboardForManualInput = Vector3.right;
            vectorInterpretation_ofCustomVector3_2 = VectorInterpretation.globalSpace;
        }

        public override void DrawVisualizedObject()
        {
            Vector3 vector1_lhs_leftThumb = Get_customVector3_1_inGlobalSpaceUnits();
            Vector3 vector2_rhs_leftIndexFinger = Get_customVector3_2_inGlobalSpaceUnits();

            UtilitiesDXXL_EngineBasics.Set_colorOfVector1_forCrossProduct_reversible(colorOfVector1_forCrossProduct);
            UtilitiesDXXL_EngineBasics.Set_colorOfVector2_forCrossProduct_reversible(colorOfVector2_forCrossProduct);
            UtilitiesDXXL_EngineBasics.Set_colorOfAngle_forCrossProduct_reversible(colorOfAngle_forCrossProduct);
            UtilitiesDXXL_EngineBasics.Set_colorOfResultVector_forCrossProduct_reversible(colorOfResultVector_forCrossProduct);
            UtilitiesDXXL_EngineBasics.Set_colorOfResultText_forCrossProduct_reversible(colorOfResultText_forCrossProduct);
            UtilitiesDXXL_EngineBasics.Set_colorOfResultPlane_forCrossProduct_reversible(colorOfResultPlane_forCrossProduct);

            DrawEngineBasics.CrossProduct(vector1_lhs_leftThumb, vector2_rhs_leftIndexFinger, GetDrawPos3D_global(), linesWidth, 0.0f, hiddenByNearerObjects);

            UtilitiesDXXL_EngineBasics.Reverse_colorOfVector1_forCrossProduct();
            UtilitiesDXXL_EngineBasics.Reverse_colorOfVector2_forCrossProduct();
            UtilitiesDXXL_EngineBasics.Reverse_colorOfAngle_forCrossProduct();
            UtilitiesDXXL_EngineBasics.Reverse_colorOfResultVector_forCrossProduct();
            UtilitiesDXXL_EngineBasics.Reverse_colorOfResultText_forCrossProduct();
            UtilitiesDXXL_EngineBasics.Reverse_colorOfResultPlane_forCrossProduct();
        }

    }

}
