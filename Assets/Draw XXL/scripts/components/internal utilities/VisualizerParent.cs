namespace DrawXXL
{
    using UnityEngine;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Internal Not For Manual Creation/Visualizer Parent")]
    [ExecuteInEditMode]
    [DefaultExecutionOrder(31000)] //negative numers are early, positive numbers are late. Range is till 32000 to both negative and positive direction.
    public class VisualizerParent : MonoBehaviour //has "parent" in it's name despite beeing already apparent through the code structure: For better orientation in the Unity Editor Component Picker list.
    {
        [SerializeField] bool drawOnlyIfSelected = DrawBasics.initial_drawOnlyIfSelected_forComponents; //has no effect in builds
        [SerializeField] public bool hiddenByNearerObjects = true;
        [SerializeField] public int drawnLinesPerPass; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool drawPosOffset3DSection_isOutfolded = false;
        [SerializeField] Vector3 drawPosOffset3D_global = Vector3.zero;
        [SerializeField] Vector3 drawPosOffset3D_local = Vector3.zero;
        [SerializeField] public bool drawPosOffset3DSection_isOutfolded_independentAlternativeValue = false;
        [SerializeField] Vector3 drawPosOffset3D_global_independentAlternativeValue = Vector3.zero;
        [SerializeField] Vector3 drawPosOffset3D_local_independentAlternativeValue = Vector3.zero;
        [SerializeField] public bool drawPosOffset2DSection_isOutfolded = false;
        [SerializeField] Vector2 drawPosOffset2D_global = Vector2.zero;
        [SerializeField] Vector2 drawPosOffset2D_local = Vector2.zero;
        [SerializeField] public bool drawPosOffset2DSection_isOutfolded_independentAlternativeValue = false;
        [SerializeField] Vector2 drawPosOffset2D_global_independentAlternativeValue = Vector2.zero;
        [SerializeField] Vector2 drawPosOffset2D_local_independentAlternativeValue = Vector2.zero;

        //Foldouts:
        [SerializeField] public bool textSection_isOutfolded = false;
        [SerializeField] public bool textStyleSection_isOutfolded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool textMarkupHelperSection_isOutfolded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.

        //Text:
        public string text_exclGlobalMarkupTags;
        public string text_inclGlobalMarkupTags;

        [SerializeField] public bool globalText_isBold = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool globalText_isItalic = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool globalText_isUnderlined = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool globalText_isDeleted = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool globalText_isSizeModified = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool globalText_isColorModified = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool globalText_isStrokeWidthModified = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public float curr_sizeScaleFactor_forGlobalMarkup = 1.0f; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public Color curr_color_forGlobalMarkup = Color.red; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public float curr_strokeWidthSize0to1_forGlobalMarkup = 0.333333f; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.

        [SerializeField] public bool markupSnippetText_isBold = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool markupSnippetText_escapesBold = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool markupSnippetText_isItalic = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool markupSnippetText_escapesItalic = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool markupSnippetText_isUnderlined = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool markupSnippetText_escapesUnderlined = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool markupSnippetText_isDeleted = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool markupSnippetText_escapesDeleted = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool markupSnippetText_isSizeModified = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool markupSnippetText_isColorModified = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public bool markupSnippetText_isStrokeWidthModified = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public float curr_sizeScaleFactor_forMarkupSnippet = 1.0f; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public Color curr_color_forMarkupSnippet = Color.red; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public float curr_strokeWidthSize0to1_forMarkupSnippet = 0.333333f; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        public enum TextMarkupInputSnippetOptions { text, icon, logSymbol, customHeightEmptyLine, lineBreak };
        [SerializeField] public TextMarkupInputSnippetOptions curr_textMarkupInputSnippetOptions = TextMarkupInputSnippetOptions.text; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public string textSnippet_toPutInMarkupTags = "input text snippet"; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public string resultTextSnippet_insideMarkupTags_forTextInput = "input text snippet"; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public string resultTextSnippet_insideMarkupTags_forIconInput = DrawText.MarkupIcon(DrawBasics.IconType.heart); //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public string resultTextSnippet_insideMarkupTags_forCustomHeightEmptyLineInput = DrawText.MarkupCustomHeightEmptyLine(1.0f); //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public string resultTextSnippet_insideMarkupTags_forLogSymbolInput = DrawText.MarkupLogSymbol(LogType.Log); //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public DrawBasics.IconType curr_iconType_forMarkupCreator = DrawBasics.IconType.heart; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] [Range(0.1f, 10.0f)] public float curr_heightOfEmptyLine_forMarkupCreator = 1.0f; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public LogType curr_logType_forMarkupCreator = LogType.Log; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.

        [SerializeField] public float endPlates_size = 0.0f;
        [SerializeField] public DrawBasics.LengthInterpretation endPlates_sizeInterpretation = DrawBasics.endPlates_sizeInterpretation;
        [SerializeField] public bool endPlates_size_isOutfolded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public float coneLength_forStraightVectors = 0.17f;
        [SerializeField] public DrawBasics.LengthInterpretation coneLength_interpretation_forStraightVectors = DrawBasics.coneLength_interpretation_forStraightVectors;
        [SerializeField] public bool coneLength_forStraightVectors_isOutfolded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        [SerializeField] public float coneLength_forCircledVectors = 0.17f;
        [SerializeField] public DrawBasics.LengthInterpretation coneLength_interpretation_forCircledVectors = DrawBasics.coneLength_interpretation_forCircledVectors;
        [SerializeField] public bool coneLength_forCircledVectors_isOutfolded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.

        [SerializeField] public bool customZPos_for2D_isOutfolded = false; //is only "public" to silence the compiler warning saying that it is "never used". The compiler doesn't know that it is used via serialization.
        public enum ZPosSource { transformPositionPlusOffset, setAbsolute, defaultDrawZPosFromGlobalDrawXxlSettings };
        [SerializeField] ZPosSource zPosSource = ZPosSource.transformPositionPlusOffset;
        [SerializeField] float customZPos_offsetValue = 0.0f;
        [SerializeField] float customZPos_absValue = 0.0f;

        //custom vectors:
        public enum CustomVector3Source { manualInput, toOtherGameobject, fromOtherGameobject, transformsForward, transformsUp, transformsRight, transformsBack, transformsDown, transformsLeft, globalForward, globalUp, globalRight, globalBack, globalDown, globalLeft, observerCameraForward, observerCameraUp, observerCameraRight, observerCameraBack, observerCameraDown, observerCameraLeft, observerCameraToThisGameobject };
        public enum CustomVector2Source { rotationAroundZStartingFromRight, manualInput, toOtherGameobject, fromOtherGameobject, transformsUp, transformsRight, transformsDown, transformsLeft, globalUp, globalRight, globalDown, globalLeft };
        public enum VectorInterpretation { globalSpace, localSpaceDefinedByParent };

        //Custom Vector3's:
        [SerializeField] public CustomVector3Source source_ofCustomVector3_1 = (CustomVector3Source)(-1);
        [SerializeField] public Vector3 customVector3_1_clipboardForManualInput;
        [SerializeField] public GameObject customVector3_1_targetGameObject;
        [SerializeField] public bool customVector3_1_hasForcedAbsLength = false;
        [SerializeField] public bool customVector3_1_picker_isOutfolded = false;
        [SerializeField] float forcedAbsLength_ofCustomVector3_1 = 1.0f;
        [SerializeField] [Range(0.1f, 10.0f)] public float lengthRelScaleFactor_ofCustomVector3_1 = 1.0f;
        [SerializeField] public VectorInterpretation vectorInterpretation_ofCustomVector3_1;
        [SerializeField] public DrawBasics.CameraForAutomaticOrientation observerCamera_ofCustomVector3_1 = DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;

        [SerializeField] public CustomVector3Source source_ofCustomVector3_2 = (CustomVector3Source)(-1);
        [SerializeField] public Vector3 customVector3_2_clipboardForManualInput;
        [SerializeField] public GameObject customVector3_2_targetGameObject;
        [SerializeField] public bool customVector3_2_hasForcedAbsLength = false;
        [SerializeField] public bool customVector3_2_picker_isOutfolded = false;
        [SerializeField] float forcedAbsLength_ofCustomVector3_2 = 1.0f;
        [SerializeField] [Range(0.1f, 10.0f)] public float lengthRelScaleFactor_ofCustomVector3_2 = 1.0f;
        [SerializeField] public VectorInterpretation vectorInterpretation_ofCustomVector3_2;
        [SerializeField] public DrawBasics.CameraForAutomaticOrientation observerCamera_ofCustomVector3_2 = DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;

        [SerializeField] public CustomVector3Source source_ofCustomVector3_3 = (CustomVector3Source)(-1);
        [SerializeField] public Vector3 customVector3_3_clipboardForManualInput;
        [SerializeField] public GameObject customVector3_3_targetGameObject;
        [SerializeField] public bool customVector3_3_hasForcedAbsLength = false;
        [SerializeField] public bool customVector3_3_picker_isOutfolded = false;
        [SerializeField] float forcedAbsLength_ofCustomVector3_3 = 1.0f;
        [SerializeField] [Range(0.1f, 10.0f)] public float lengthRelScaleFactor_ofCustomVector3_3 = 1.0f;
        [SerializeField] public VectorInterpretation vectorInterpretation_ofCustomVector3_3;
        [SerializeField] public DrawBasics.CameraForAutomaticOrientation observerCamera_ofCustomVector3_3 = DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;

        [SerializeField] public CustomVector3Source source_ofCustomVector3_4 = (CustomVector3Source)(-1);
        [SerializeField] public Vector3 customVector3_4_clipboardForManualInput;
        [SerializeField] public GameObject customVector3_4_targetGameObject;
        [SerializeField] public bool customVector3_4_hasForcedAbsLength = false;
        [SerializeField] public bool customVector3_4_picker_isOutfolded = false;
        [SerializeField] float forcedAbsLength_ofCustomVector3_4 = 1.0f;
        [SerializeField] [Range(0.1f, 10.0f)] public float lengthRelScaleFactor_ofCustomVector3_4 = 1.0f;
        [SerializeField] public VectorInterpretation vectorInterpretation_ofCustomVector3_4;
        [SerializeField] public DrawBasics.CameraForAutomaticOrientation observerCamera_ofCustomVector3_4 = DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;

        //Custom Vector2's:
        [SerializeField] public CustomVector2Source source_ofCustomVector2_1 = (CustomVector2Source)(-1);
        [SerializeField] public Vector2 customVector2_1_clipboardForManualInput;
        [SerializeField] public GameObject customVector2_1_targetGameObject;
        [SerializeField] bool customVector2_1_hasForcedAbsLength = false;
        [SerializeField] public bool customVector2_1_picker_isOutfolded = false;
        [SerializeField] [Range(-360.0f, 360.0f)] float rotationFromRight_ofCustomVector2_1 = 0.0f;
        [SerializeField] float forcedAbsLength_ofCustomVector2_1 = 1.0f;
        [SerializeField] [Range(0.1f, 10.0f)] float lengthRelScaleFactor_ofCustomVector2_1 = 1.0f;
        [SerializeField] public VectorInterpretation vectorInterpretation_ofCustomVector2_1;

        [SerializeField] public CustomVector2Source source_ofCustomVector2_2 = (CustomVector2Source)(-1);
        [SerializeField] public Vector2 customVector2_2_clipboardForManualInput;
        [SerializeField] public GameObject customVector2_2_targetGameObject;
        [SerializeField] bool customVector2_2_hasForcedAbsLength = false;
        [SerializeField] public bool customVector2_2_picker_isOutfolded = false;
        [SerializeField] [Range(-360.0f, 360.0f)] float rotationFromRight_ofCustomVector2_2 = 0.0f;
        [SerializeField] float forcedAbsLength_ofCustomVector2_2 = 1.0f;
        [SerializeField] [Range(0.1f, 10.0f)] float lengthRelScaleFactor_ofCustomVector2_2 = 1.0f;
        [SerializeField] public VectorInterpretation vectorInterpretation_ofCustomVector2_2;

        [SerializeField] public CustomVector2Source source_ofCustomVector2_3 = (CustomVector2Source)(-1);
        [SerializeField] public Vector2 customVector2_3_clipboardForManualInput;
        [SerializeField] public GameObject customVector2_3_targetGameObject;
        [SerializeField] bool customVector2_3_hasForcedAbsLength = false;
        [SerializeField] public bool customVector2_3_picker_isOutfolded = false;
        [SerializeField] [Range(-360.0f, 360.0f)] float rotationFromRight_ofCustomVector2_3 = 0.0f;
        [SerializeField] float forcedAbsLength_ofCustomVector2_3 = 1.0f;
        [SerializeField] [Range(0.1f, 10.0f)] float lengthRelScaleFactor_ofCustomVector2_3 = 1.0f;
        [SerializeField] public VectorInterpretation vectorInterpretation_ofCustomVector2_3;

        [SerializeField] public CustomVector2Source source_ofCustomVector2_4 = (CustomVector2Source)(-1);
        [SerializeField] public Vector2 customVector2_4_clipboardForManualInput;
        [SerializeField] public GameObject customVector2_4_targetGameObject;
        [SerializeField] bool customVector2_4_hasForcedAbsLength = false;
        [SerializeField] public bool customVector2_4_picker_isOutfolded = false;
        [SerializeField] [Range(-360.0f, 360.0f)] float rotationFromRight_ofCustomVector2_4 = 0.0f;
        [SerializeField] float forcedAbsLength_ofCustomVector2_4 = 1.0f;
        [SerializeField] [Range(0.1f, 10.0f)] float lengthRelScaleFactor_ofCustomVector2_4 = 1.0f;
        [SerializeField] public VectorInterpretation vectorInterpretation_ofCustomVector2_4;

        public delegate Vector3 FlexibleGetCustomVector3();
        public delegate Vector2 FlexibleGetCustomVector2();

        //partner Gameobject:
        [SerializeField] public GameObject partnerGameobject;
        [SerializeField] public GameObject partnerGameobject_independentAlternativeValue;
        //parter gameobjects Vector3 offset:
        [SerializeField] public bool drawPosOffset3DSection_ofPartnerGameobject_isOutfolded = false;
        [SerializeField] Vector3 drawPosOffset3D_ofPartnerGameobject_global = Vector3.zero;
        [SerializeField] Vector3 drawPosOffset3D_ofPartnerGameobject_local = Vector3.zero;
        [SerializeField] public bool drawPosOffset3DSection_ofPartnerGameobject_isOutfolded_independentAlternativeValue = false;
        [SerializeField] Vector3 drawPosOffset3D_ofPartnerGameobject_global_independentAlternativeValue = Vector3.zero;
        [SerializeField] Vector3 drawPosOffset3D_ofPartnerGameobject_local_independentAlternativeValue = Vector3.zero;
        //parter gameobjects Vector2 offset:
        [SerializeField] public bool drawPosOffset2DSection_ofPartnerGameobject_isOutfolded = false;
        [SerializeField] Vector2 drawPosOffset2D_ofPartnerGameobject_global = Vector2.zero;
        [SerializeField] Vector2 drawPosOffset2D_ofPartnerGameobject_local = Vector2.zero;
        [SerializeField] public bool drawPosOffset2DSection_ofPartnerGameobject_isOutfolded_independentAlternativeValue = false;
        [SerializeField] Vector2 drawPosOffset2D_ofPartnerGameobject_global_independentAlternativeValue = Vector2.zero;
        [SerializeField] Vector2 drawPosOffset2D_ofPartnerGameobject_local_independentAlternativeValue = Vector2.zero;
        //parter gameobjects custom Vector3:
        [SerializeField] public CustomVector3Source source_ofCustomVector3ofPartnerGameobject = (CustomVector3Source)(-1);
        [SerializeField] public Vector3 customVector3ofPartnerGameobject_clipboardForManualInput;
        [SerializeField] public GameObject customVector3ofPartnerGameobject_targetGameObject;
        [SerializeField] bool customVector3ofPartnerGameobject_hasForcedAbsLength = false;
        [SerializeField] public bool customVector3ofPartnerGameobject_picker_isOutfolded = false;
        [SerializeField] float forcedAbsLength_ofCustomVector3ofPartnerGameobject = 1.0f;
        [SerializeField] [Range(0.1f, 10.0f)] float lengthRelScaleFactor_ofCustomVector3ofPartnerGameobject = 1.0f;
        [SerializeField] public VectorInterpretation vectorInterpretation_ofCustomVector3ofPartnerGameobject;
        [SerializeField] public DrawBasics.CameraForAutomaticOrientation observerCamera_ofCustomVector3ofPartnerGameobject = DrawBasics.CameraForAutomaticOrientation.sceneViewCamera;
        //parter gameobjects custom Vector2:
        [SerializeField] public CustomVector2Source source_ofCustomVector2ofPartnerGameobject = (CustomVector2Source)(-1);
        [SerializeField] public Vector2 customVector2ofPartnerGameobject_clipboardForManualInput;
        [SerializeField] public GameObject customVector2ofPartnerGameobject_targetGameObject;
        [SerializeField] bool customVector2ofPartnerGameobject_hasForcedAbsLength = false;
        [SerializeField] public bool customVector2ofPartnerGameobject_picker_isOutfolded = false;
        [SerializeField] [Range(-360.0f, 360.0f)] float rotationFromRight_ofCustomVector2ofPartnerGameobject = 0.0f;
        [SerializeField] float forcedAbsLength_ofCustomVector2ofPartnerGameobject = 1.0f;
        [SerializeField] [Range(0.1f, 10.0f)] float lengthRelScaleFactor_ofCustomVector2ofPartnerGameobject = 1.0f;
        [SerializeField] public VectorInterpretation vectorInterpretation_ofCustomVector2ofPartnerGameobject;

#if UNITY_EDITOR
        bool isFirstUpdateCycleAfterGamePause = false;
        int mostCurrentFrameCountDuringGamePause = -10;
        [SerializeField] bool isAlreadyInitialized = false;
        bool editorUpdateCallback_hasBeenRegistered = false;
#endif


        void Start()
        {
#if UNITY_EDITOR
            RegisterEditorUpdateCallback();
            Reset_mostCurrentFrameCountDuringGamePause();

            if (isAlreadyInitialized == false)
            {
                InitializeValues_onceInComponentLifetime();
                isAlreadyInitialized = true;
            }
            InitializeValues_alsoOnPlaymodeEnter_andOnComponentCreatedAsCopy();
#endif
        }

        public virtual void InitializeValues_onceInComponentLifetime()
        {
            //"BezierSplineDrawer.InitializeValues_onceInComponentLifetime()" contains the explanation of the difference between "InitializeValues_onceInComponentLifetime()" and "InitializeValues_alsoOnPlaymodeEnter_andOnComponentCreatedAsCopy()"
        }

        public virtual void InitializeValues_alsoOnPlaymodeEnter_andOnComponentCreatedAsCopy()
        {
            //"BezierSplineDrawer.InitializeValues_onceInComponentLifetime()" contains the explanation of the difference between "InitializeValues_onceInComponentLifetime()" and "InitializeValues_alsoOnPlaymodeEnter_andOnComponentCreatedAsCopy()"
        }

        public void TrySetTextToGameobjectName()
        {
            if (text_exclGlobalMarkupTags == null || text_exclGlobalMarkupTags == "")
            {
                text_exclGlobalMarkupTags = "of " + this.gameObject.name;
                text_inclGlobalMarkupTags = "of " + this.gameObject.name;
            }
        }

        public void TrySetTextToEmptyString()
        {
            if (text_exclGlobalMarkupTags == null || text_exclGlobalMarkupTags == "")
            {
                text_exclGlobalMarkupTags = "";
                text_inclGlobalMarkupTags = "";
            }
        }

        void OnDestroy()
        {
            UnregisterEditorUpdateCallback();
        }

#if UNITY_EDITOR
        void Reset_mostCurrentFrameCountDuringGamePause()
        {
            mostCurrentFrameCountDuringGamePause = -10;
        }
#endif

        public void RegisterEditorUpdateCallback()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update += EditorUpdateCallback;
            editorUpdateCallback_hasBeenRegistered = true;
#endif
        }

        public void UnregisterEditorUpdateCallback()
        {
#if UNITY_EDITOR
            if (editorUpdateCallback_hasBeenRegistered)
            {
                UnityEditor.EditorApplication.update -= EditorUpdateCallback;
                editorUpdateCallback_hasBeenRegistered = false;
            }
#endif
        }

        void EditorUpdateCallback()
        {
            UtilitiesDXXL_Components.TryProceedOneSheduledFrameStep();
        }

        void OnEnable()
        {
#if UNITY_EDITOR
            UtilitiesDXXL_Components.ExpandComponentInInspector(this);
#endif
        }

        void Update()
        {
#if UNITY_EDITOR
            UtilitiesDXXL_Components.ExpandComponentInInspector(this);
#endif
        }

        void LateUpdate()
        {
            if (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled)
            {
                UtilitiesDXXL_Components.ExpandComponentInInspector(this);
                if (this.enabled && PlaymodeIsActiveAndNotPaused() && CheckIf_drawObjectAccordingTo_selectedState())
                {
                    if (TrySkipDrawingBecauseItIsTheFirstFrameAfterAPausePhase()) { return; }
                    if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

                    UtilitiesDXXL_DrawBasics.Set_usedLineDrawingMethod_reversible(DrawBasics.UsedUnityLineDrawingMethod.debugLines);
                    long lineCounter_beforeDrawing = DXXLWrapperForUntiysBuildInDrawLines.DrawnLinesSinceStart; //using "DXXLWrapperForUntiyDebugDraw.DrawnLinesSinceStart" instead of "DXXLWrapperForUntiyDebugDraw.DrawnLinesSinceFrameStart" because if this is the only drawn thing inside the frame, then the following "DrawVisualizedObject()" resets the "DrawnLinesSinceFrameStart"-counter (because it's the first drawn thing since "Time.frameCount" was incremented), so the herewith obtained "lineCounter_beforeDrawing" is not accurate anymore.
                    DrawVisualizedObject();
                    drawnLinesPerPass = (int)(DXXLWrapperForUntiysBuildInDrawLines.DrawnLinesSinceStart - lineCounter_beforeDrawing);
                    UtilitiesDXXL_DrawBasics.Reverse_usedLineDrawingMethod();
                }
            }
        }

        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            UtilitiesDXXL_Components.ExpandComponentInInspector(this);
            if (this.enabled && IsEditModeOrPlaymodePause() && CheckIf_drawObjectAccordingTo_selectedState())
            {
                UtilitiesDXXL_Components.ReportOnDrawGizmosCycleOfAMonoBehaviour(this.GetInstanceID());
                if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

                UtilitiesDXXL_DrawBasics.Set_usedLineDrawingMethod_reversible(DrawBasics.UsedUnityLineDrawingMethod.gizmoLines); //-> "debugLinesInPlayMode_gizmoLinesInEditModeAndPlaymodePauses" is not an option here, because "DXXLWrapperForUntiysBuildInDrawLines.ChooseDebugOrGizmoLines_dependingOnPlayModeState()" forces to debug lines in some cases even if "pause == true"
                UtilitiesDXXL_DrawBasics.Set_gizmoMatrix_reversible(Matrix4x4.identity);
                long lineCounter_beforeDrawing = DXXLWrapperForUntiysBuildInDrawLines.DrawnLinesSinceStart; //using "DXXLWrapperForUntiyDebugDraw.DrawnLinesSinceStart" instead of "DXXLWrapperForUntiyDebugDraw.DrawnLinesSinceFrameStart": see comment in "LateUpdate()"
                DrawVisualizedObject();
                drawnLinesPerPass = (int)(DXXLWrapperForUntiysBuildInDrawLines.DrawnLinesSinceStart - lineCounter_beforeDrawing);
                UtilitiesDXXL_DrawBasics.Reverse_usedLineDrawingMethod();
                UtilitiesDXXL_DrawBasics.Reverse_gizmoMatrix();

                TrySheduleAutomaticFrameStepAtTheStartOfPausePhases();
            }
#endif
        }

        bool PlaymodeIsActiveAndNotPaused()
        {
#if UNITY_EDITOR
            return (UnityEditor.EditorApplication.isPlaying && (UnityEditor.EditorApplication.isPaused == false));
#else
            return (DrawBasics.usedUnityLineDrawingMethod != DrawBasics.UsedUnityLineDrawingMethod.disabled);
#endif
        }

        bool IsEditModeOrPlaymodePause()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying == false)
            {
                return true;
            }
            else
            {
                if (UnityEditor.EditorApplication.isPaused)
                {
                    return true;
                }
            }
            return false;
#else
            return false;
#endif
        }

        bool CheckIf_drawObjectAccordingTo_selectedState()
        {
#if UNITY_EDITOR
            if (drawOnlyIfSelected)
            {
                return UnityEditor.Selection.Contains(gameObject.GetInstanceID());
            }
            else
            {
                return true;
            }
#else
            return true;
#endif
        }

        public virtual void DrawVisualizedObject()
        {
        }

        bool TrySkipDrawingBecauseItIsTheFirstFrameAfterAPausePhase()
        {
            //returns "doSkipDrawing"

            //-> this prevents drawing in the first frame after pause phases, to prevent the additional frozen overdraw during pause phases, caused by using "Debug.DrawLine()", which doesn't get cleared during pause phases.
            //-> see also "TrySheduleAutomaticFrameStepAtTheStartOfPausePhases()"
            //-> for more details: See documentation of "DrawBasics.drawerComponentsAutomaticallyProceedOneFrameStepOnPauseStarts_toPreventFrozenOverlayDraw"
            //-> it also preserves the ability of the user to use the "Step"-functionality via the right one of the three play/pause buttons on top of the Unity window.

#if UNITY_EDITOR
            if (isFirstUpdateCycleAfterGamePause)
            {
                isFirstUpdateCycleAfterGamePause = false;
                return true;
            }
            else
            {
                isFirstUpdateCycleAfterGamePause = false;
                return false;
            }
#else
            return false;
#endif
        }

        void TrySheduleAutomaticFrameStepAtTheStartOfPausePhases()
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying && UnityEditor.EditorApplication.isPaused)
            {
                isFirstUpdateCycleAfterGamePause = true; //-> prepare for upcoming Update-cycles

                //-> the additional frame gives the component the chance to skip drawing with "Debug.DrawLine()" in the frame before pause phases, so there are no frozen uncleared debugLines present during the pause phase
                //-> see also "TrySkipDrawingBecauseItIsTheFirstFrameAfterAPausePhase()"
                //-> for more details: See documentation of "DrawBasics.drawerComponentsAutomaticallyProceedOneFrameStepOnPauseStarts_toPreventFrozenOverlayDraw"
                if (mostCurrentFrameCountDuringGamePause <= (Time.frameCount - 2))
                {
                    //-> is first arrival after pausing the game
                    //-> at least two Update cycles happened since the previous game pause (or alternatively the playmode has been startet with "pause" already activated)
                    //-> cannot arrive here after proceeding only a single frame step between two pause phases
                    //("Time.frameCount" doesn't increase during pause phases)
                    UtilitiesDXXL_Components.frameCount_forWhichAnEditorFrameStep_hasBeenSheduled_byADrawerComponent = Time.frameCount; //-> sheduling, because calling "UnityEditor.EditorApplication.Step()" from here causes Unity to print "recursive GUI rendering" errors
                }
                mostCurrentFrameCountDuringGamePause = Time.frameCount;
            }
#endif
        }

        public Vector3 GetDrawPos3D_global()
        {
            //-> local space is defined by this gameObject itself, not by the parent.
            return (transform.position + drawPosOffset3D_global + transform.rotation * Vector3.Scale(transform.lossyScale, drawPosOffset3D_local));
        }

        public Vector3 GetDrawPos3D_global_independentAlternativeValue()
        {
            //-> local space is defined by this gameObject itself, not by the parent.
            return (transform.position + drawPosOffset3D_global_independentAlternativeValue + transform.rotation * Vector3.Scale(transform.lossyScale, drawPosOffset3D_local_independentAlternativeValue));
        }

        public Vector3 GetDrawPos3D_ofPartnerGameobject_global(bool theSpaceForTransformingThePartnerLocalOffsetValue_isTakenFromThePartnerGameobject_notFromThisGameobject = true)
        {
            //-> local space is defined by the gameObject itself, not by the parent.
            if (partnerGameobject != null)
            {
                if (theSpaceForTransformingThePartnerLocalOffsetValue_isTakenFromThePartnerGameobject_notFromThisGameobject)
                {
                    return (partnerGameobject.transform.position + drawPosOffset3D_ofPartnerGameobject_global + partnerGameobject.transform.rotation * Vector3.Scale(partnerGameobject.transform.lossyScale, drawPosOffset3D_ofPartnerGameobject_local));
                }
                else
                {
                    return (partnerGameobject.transform.position + drawPosOffset3D_ofPartnerGameobject_global + transform.rotation * Vector3.Scale(transform.lossyScale, drawPosOffset3D_ofPartnerGameobject_local));
                }
            }
            else
            {
                return (transform.position + drawPosOffset3D_ofPartnerGameobject_global + transform.rotation * Vector3.Scale(transform.lossyScale, drawPosOffset3D_ofPartnerGameobject_local));
            }
        }

        public Vector3 GetDrawPos3D_ofPartnerGameobject_global_independentAlternativeValue(bool theSpaceForTransformingThePartnerLocalOffsetValue_isTakenFromThePartnerGameobject_notFromThisGameobject = true)
        {
            //-> local space is defined by the gameObject itself, not by the parent.
            if (partnerGameobject_independentAlternativeValue != null)
            {
                if (theSpaceForTransformingThePartnerLocalOffsetValue_isTakenFromThePartnerGameobject_notFromThisGameobject)
                {
                    return (partnerGameobject_independentAlternativeValue.transform.position + drawPosOffset3D_ofPartnerGameobject_global_independentAlternativeValue + partnerGameobject_independentAlternativeValue.transform.rotation * Vector3.Scale(partnerGameobject_independentAlternativeValue.transform.lossyScale, drawPosOffset3D_ofPartnerGameobject_local_independentAlternativeValue));
                }
                else
                {
                    return (partnerGameobject_independentAlternativeValue.transform.position + drawPosOffset3D_ofPartnerGameobject_global_independentAlternativeValue + transform.rotation * Vector3.Scale(transform.lossyScale, drawPosOffset3D_ofPartnerGameobject_local_independentAlternativeValue));
                }
            }
            else
            {
                return (transform.position + drawPosOffset3D_ofPartnerGameobject_global_independentAlternativeValue + transform.rotation * Vector3.Scale(transform.lossyScale, drawPosOffset3D_ofPartnerGameobject_local_independentAlternativeValue));
            }
        }

        public Vector3 GetDrawPos3D_inLocalSpaceAsDefinedByParent()
        {
            //-> "drawPosOffset3D_local" is in units of the space defined by this gameObject itself, not by the parent.
            //-> In constrast to "drawPosOffset3D_local" the return value is in units of the parent defined space

            if (transform.parent == null)
            {
                return GetDrawPos3D_global();
            }
            else
            {
                Vector3 localDrawPosOffset_localInsideParentSpace = transform.localRotation * Vector3.Scale(transform.localScale, drawPosOffset3D_local);
                Quaternion inverseRotationOfLocalSpaceDefinedByParent = Quaternion.Inverse(transform.parent.rotation);
                Vector3 globalDrawPosOffset_rotatedToLocalSpaceDefinedByParent = inverseRotationOfLocalSpaceDefinedByParent * drawPosOffset3D_global;
                if (UtilitiesDXXL_Math.ContainsZeroComponents(transform.parent.lossyScale))
                {
                    //-> see note in "CustomVector_globalToLocalSpaceDefinedByParent()"
                    return (transform.localPosition + localDrawPosOffset_localInsideParentSpace + globalDrawPosOffset_rotatedToLocalSpaceDefinedByParent);
                }
                else
                {
                    Vector3 globalDrawPosOffset_rotatedAndScaledToLocalSpaceDefinedByParent = new Vector3(globalDrawPosOffset_rotatedToLocalSpaceDefinedByParent.x / transform.parent.lossyScale.x, globalDrawPosOffset_rotatedToLocalSpaceDefinedByParent.y / transform.parent.lossyScale.y, globalDrawPosOffset_rotatedToLocalSpaceDefinedByParent.z / transform.parent.lossyScale.z);
                    return (transform.localPosition + localDrawPosOffset_localInsideParentSpace + globalDrawPosOffset_rotatedAndScaledToLocalSpaceDefinedByParent);
                }
            }
        }

        public Vector3 GetDrawPos3D_inLocalSpaceAsDefinedByThisGameobject()
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(drawPosOffset3D_global) && UtilitiesDXXL_Math.ApproximatelyZero(drawPosOffset3D_local))
            {
                return Vector3.zero;
            }
            else
            {
                Vector3 offsetPortion_fromGlobalOffset = transform.InverseTransformVector(drawPosOffset3D_global);
                Vector3 offsetPortion_fromLocalOffset = drawPosOffset3D_local;
                return (offsetPortion_fromGlobalOffset + offsetPortion_fromLocalOffset);
            }
        }

        public Vector2 GetDrawPos2D_global()
        {
            //-> local space is defined by this gameObject itself, not by the parent.
            Vector2 transformPos_asV2_global = new Vector2(transform.position.x, transform.position.y);

            Vector2 transformsRight_asV2_normalized = new Vector2(transform.right.x, transform.right.y);
            Vector2 localOffsetAlongTransformsRight_inGlobalSpaceUnits = transformsRight_asV2_normalized * transform.lossyScale.x * drawPosOffset2D_local.x;

            Vector2 transformsUp_asV2_normalized = new Vector2(transform.up.x, transform.up.y);
            Vector2 localOffsetAlongTransformsUp_inGlobalSpaceUnits = transformsUp_asV2_normalized * transform.lossyScale.y * drawPosOffset2D_local.y;

            return (transformPos_asV2_global + drawPosOffset2D_global + localOffsetAlongTransformsRight_inGlobalSpaceUnits + localOffsetAlongTransformsUp_inGlobalSpaceUnits);
        }

        public Vector2 GetDrawPos2D_global_independentAlternativeValue()
        {
            //-> local space is defined by this gameObject itself, not by the parent.
            Vector2 transformPos_asV2_global = new Vector2(transform.position.x, transform.position.y);

            Vector2 transformsRight_asV2_normalized = new Vector2(transform.right.x, transform.right.y);
            Vector2 localOffsetAlongTransformsRight_inGlobalSpaceUnits = transformsRight_asV2_normalized * transform.lossyScale.x * drawPosOffset2D_local_independentAlternativeValue.x;

            Vector2 transformsUp_asV2_normalized = new Vector2(transform.up.x, transform.up.y);
            Vector2 localOffsetAlongTransformsUp_inGlobalSpaceUnits = transformsUp_asV2_normalized * transform.lossyScale.y * drawPosOffset2D_local_independentAlternativeValue.y;

            return (transformPos_asV2_global + drawPosOffset2D_global_independentAlternativeValue + localOffsetAlongTransformsRight_inGlobalSpaceUnits + localOffsetAlongTransformsUp_inGlobalSpaceUnits);
        }

        public Vector2 GetDrawPos2D_ofPartnerGameobject_global(bool theSpaceForTransformingThePartnerLocalOffsetValue_isTakenFromThePartnerGameobject_notFromThisGameobject = true)
        {
            //-> local space is defined by this gameObject itself, not by the parent.
            Transform used_transform = (partnerGameobject != null) ? partnerGameobject.transform : transform;
            Vector2 transformPos_asV2_global = new Vector2(used_transform.position.x, used_transform.position.y);

            if (theSpaceForTransformingThePartnerLocalOffsetValue_isTakenFromThePartnerGameobject_notFromThisGameobject)
            {
                Vector2 transformsRight_asV2_normalized = new Vector2(used_transform.right.x, used_transform.right.y);
                Vector2 localOffsetAlongTransformsRight_inGlobalSpaceUnits = transformsRight_asV2_normalized * used_transform.lossyScale.x * drawPosOffset2D_ofPartnerGameobject_local.x;

                Vector2 transformsUp_asV2_normalized = new Vector2(used_transform.up.x, used_transform.up.y);
                Vector2 localOffsetAlongTransformsUp_inGlobalSpaceUnits = transformsUp_asV2_normalized * used_transform.lossyScale.y * drawPosOffset2D_ofPartnerGameobject_local.y;

                return (transformPos_asV2_global + drawPosOffset2D_ofPartnerGameobject_global + localOffsetAlongTransformsRight_inGlobalSpaceUnits + localOffsetAlongTransformsUp_inGlobalSpaceUnits);
            }
            else
            {
                Vector2 transformsRight_asV2_normalized = new Vector2(transform.right.x, transform.right.y);
                Vector2 localOffsetAlongTransformsRight_inGlobalSpaceUnits = transformsRight_asV2_normalized * transform.lossyScale.x * drawPosOffset2D_ofPartnerGameobject_local.x;

                Vector2 transformsUp_asV2_normalized = new Vector2(transform.up.x, transform.up.y);
                Vector2 localOffsetAlongTransformsUp_inGlobalSpaceUnits = transformsUp_asV2_normalized * transform.lossyScale.y * drawPosOffset2D_ofPartnerGameobject_local.y;

                return (transformPos_asV2_global + drawPosOffset2D_ofPartnerGameobject_global + localOffsetAlongTransformsRight_inGlobalSpaceUnits + localOffsetAlongTransformsUp_inGlobalSpaceUnits);
            }
        }

        public Vector2 GetDrawPos2D_ofPartnerGameobject_global_independentAlternativeValue(bool theSpaceForTransformingThePartnerLocalOffsetValue_isTakenFromThePartnerGameobject_notFromThisGameobject = true)
        {
            //-> local space is defined by this gameObject itself, not by the parent.
            Transform used_transform = (partnerGameobject_independentAlternativeValue != null) ? partnerGameobject_independentAlternativeValue.transform : transform;
            Vector2 transformPos_asV2_global = new Vector2(used_transform.position.x, used_transform.position.y);

            if (theSpaceForTransformingThePartnerLocalOffsetValue_isTakenFromThePartnerGameobject_notFromThisGameobject)
            {
                Vector2 transformsRight_asV2_normalized = new Vector2(used_transform.right.x, used_transform.right.y);
                Vector2 localOffsetAlongTransformsRight_inGlobalSpaceUnits = transformsRight_asV2_normalized * used_transform.lossyScale.x * drawPosOffset2D_ofPartnerGameobject_local_independentAlternativeValue.x;

                Vector2 transformsUp_asV2_normalized = new Vector2(used_transform.up.x, used_transform.up.y);
                Vector2 localOffsetAlongTransformsUp_inGlobalSpaceUnits = transformsUp_asV2_normalized * used_transform.lossyScale.y * drawPosOffset2D_ofPartnerGameobject_local_independentAlternativeValue.y;

                return (transformPos_asV2_global + drawPosOffset2D_ofPartnerGameobject_global_independentAlternativeValue + localOffsetAlongTransformsRight_inGlobalSpaceUnits + localOffsetAlongTransformsUp_inGlobalSpaceUnits);
            }
            else
            {
                Vector2 transformsRight_asV2_normalized = new Vector2(transform.right.x, transform.right.y);
                Vector2 localOffsetAlongTransformsRight_inGlobalSpaceUnits = transformsRight_asV2_normalized * transform.lossyScale.x * drawPosOffset2D_ofPartnerGameobject_local_independentAlternativeValue.x;

                Vector2 transformsUp_asV2_normalized = new Vector2(transform.up.x, transform.up.y);
                Vector2 localOffsetAlongTransformsUp_inGlobalSpaceUnits = transformsUp_asV2_normalized * transform.lossyScale.y * drawPosOffset2D_ofPartnerGameobject_local_independentAlternativeValue.y;

                return (transformPos_asV2_global + drawPosOffset2D_ofPartnerGameobject_global_independentAlternativeValue + localOffsetAlongTransformsRight_inGlobalSpaceUnits + localOffsetAlongTransformsUp_inGlobalSpaceUnits);
            }
        }

        public Vector2 GetDrawPos2D_inLocalSpaceAsDefinedByParent()
        {
            //-> "drawPosOffset2D_local" is in units of the space defined by this gameObject itself, not by the parent.
            //-> In constrast to "drawPosOffset2D_local" the return value is in units of the parent defined space

            if (transform.parent == null)
            {
                return GetDrawPos2D_global();
            }
            else
            {
                Vector2 transformsLocalPosition_asV2 = new Vector2(transform.localPosition.x, transform.localPosition.y);

                Vector3 drawPosOffset2D_local_asV3 = new Vector3(drawPosOffset2D_local.x, drawPosOffset2D_local.y, 0.0f);
                Vector3 localDrawPosOffset_localInsideParentSpace = transform.localRotation * Vector3.Scale(transform.localScale, drawPosOffset2D_local_asV3);
                Vector2 localDrawPosOffset_localInsideParentSpace_asV2 = new Vector2(localDrawPosOffset_localInsideParentSpace.x, localDrawPosOffset_localInsideParentSpace.y);

                Quaternion inverseRotationOfLocalSpaceDefinedByParent = Quaternion.Inverse(transform.parent.rotation);
                Vector3 drawPosOffset2D_global_asV3 = new Vector3(drawPosOffset2D_global.x, drawPosOffset2D_global.y, 0.0f);
                Vector3 globalDrawPosOffset_rotatedToLocalSpaceDefinedByParent = inverseRotationOfLocalSpaceDefinedByParent * drawPosOffset2D_global_asV3;
                Vector2 globalDrawPosOffset_rotatedToLocalSpaceDefinedByParent_asV2 = new Vector2(globalDrawPosOffset_rotatedToLocalSpaceDefinedByParent.x, globalDrawPosOffset_rotatedToLocalSpaceDefinedByParent.y);

                if (UtilitiesDXXL_Math.ContainsZeroComponentsInXorY(transform.parent.lossyScale))
                {
                    //-> see note in "CustomVector3_globalToLocalSpaceDefinedByParent()"
                    return (transformsLocalPosition_asV2 + localDrawPosOffset_localInsideParentSpace_asV2 + globalDrawPosOffset_rotatedToLocalSpaceDefinedByParent_asV2);
                }
                else
                {
                    Vector2 globalDrawPosOffset_rotatedAndScaledToLocalSpaceDefinedByParent_asV2 = new Vector2(globalDrawPosOffset_rotatedToLocalSpaceDefinedByParent_asV2.x / transform.parent.lossyScale.x, globalDrawPosOffset_rotatedToLocalSpaceDefinedByParent_asV2.y / transform.parent.lossyScale.y);
                    return (transformsLocalPosition_asV2 + localDrawPosOffset_localInsideParentSpace_asV2 + globalDrawPosOffset_rotatedAndScaledToLocalSpaceDefinedByParent_asV2);
                }
            }
        }

        public Vector3 GetDrawPos3D_ofA2DModeTransform_global()
        {
            Vector2 drawPos2D_global = GetDrawPos2D_global();
            Vector3 drawPos3D_ofA2DModeTransform_global = new Vector3(drawPos2D_global.x, drawPos2D_global.y, GetZPos_global_for2D());
            return drawPos3D_ofA2DModeTransform_global;
        }

        public float GetZPos_global_for2D()
        {
            switch (zPosSource)
            {
                case ZPosSource.transformPositionPlusOffset:
                    if (UtilitiesDXXL_Math.FloatIsValid(customZPos_offsetValue))
                    {
                        return transform.position.z + customZPos_offsetValue;
                    }
                    else
                    {
                        return transform.position.z;
                    }
                case ZPosSource.setAbsolute:
                    if (UtilitiesDXXL_Math.FloatIsValid(customZPos_absValue))
                    {
                        return customZPos_absValue;
                    }
                    else
                    {
                        return transform.position.z;
                    }
                case ZPosSource.defaultDrawZPosFromGlobalDrawXxlSettings:
                    return DrawBasics2D.Default_zPos_forDrawing;
                default:
                    return transform.position.z;
            }
        }

        public Vector3 Get_customVector3_1_inGlobalSpaceUnits()
        {
            return Get_aCustomVector3_inGlobalSpaceUnits(vectorInterpretation_ofCustomVector3_1, source_ofCustomVector3_1, customVector3_1_clipboardForManualInput, customVector3_1_targetGameObject, customVector3_1_hasForcedAbsLength, forcedAbsLength_ofCustomVector3_1, lengthRelScaleFactor_ofCustomVector3_1, observerCamera_ofCustomVector3_1, transform);
        }

        public Vector3 Get_customVector3_1_inLocalSpaceDefinedByParentUnits()
        {
            return Get_aCustomVector3_inLocalSpaceDefinedByParentUnits(vectorInterpretation_ofCustomVector3_1, source_ofCustomVector3_1, customVector3_1_clipboardForManualInput, customVector3_1_targetGameObject, customVector3_1_hasForcedAbsLength, forcedAbsLength_ofCustomVector3_1, lengthRelScaleFactor_ofCustomVector3_1, observerCamera_ofCustomVector3_1, transform);
        }

        public Vector3 Get_customVector3_2_inGlobalSpaceUnits()
        {
            return Get_aCustomVector3_inGlobalSpaceUnits(vectorInterpretation_ofCustomVector3_2, source_ofCustomVector3_2, customVector3_2_clipboardForManualInput, customVector3_2_targetGameObject, customVector3_2_hasForcedAbsLength, forcedAbsLength_ofCustomVector3_2, lengthRelScaleFactor_ofCustomVector3_2, observerCamera_ofCustomVector3_2, transform);
        }

        public Vector3 Get_customVector3_2_inLocalSpaceDefinedByParentUnits()
        {
            return Get_aCustomVector3_inLocalSpaceDefinedByParentUnits(vectorInterpretation_ofCustomVector3_2, source_ofCustomVector3_2, customVector3_2_clipboardForManualInput, customVector3_2_targetGameObject, customVector3_2_hasForcedAbsLength, forcedAbsLength_ofCustomVector3_2, lengthRelScaleFactor_ofCustomVector3_2, observerCamera_ofCustomVector3_2, transform);
        }

        public Vector3 Get_customVector3_3_inGlobalSpaceUnits()
        {
            return Get_aCustomVector3_inGlobalSpaceUnits(vectorInterpretation_ofCustomVector3_3, source_ofCustomVector3_3, customVector3_3_clipboardForManualInput, customVector3_3_targetGameObject, customVector3_3_hasForcedAbsLength, forcedAbsLength_ofCustomVector3_3, lengthRelScaleFactor_ofCustomVector3_3, observerCamera_ofCustomVector3_3, transform);
        }

        public Vector3 Get_customVector3_3_inLocalSpaceDefinedByParentUnits()
        {
            return Get_aCustomVector3_inLocalSpaceDefinedByParentUnits(vectorInterpretation_ofCustomVector3_3, source_ofCustomVector3_3, customVector3_3_clipboardForManualInput, customVector3_3_targetGameObject, customVector3_3_hasForcedAbsLength, forcedAbsLength_ofCustomVector3_3, lengthRelScaleFactor_ofCustomVector3_3, observerCamera_ofCustomVector3_3, transform);
        }

        public Vector3 Get_customVector3_4_inGlobalSpaceUnits()
        {
            return Get_aCustomVector3_inGlobalSpaceUnits(vectorInterpretation_ofCustomVector3_4, source_ofCustomVector3_4, customVector3_4_clipboardForManualInput, customVector3_4_targetGameObject, customVector3_4_hasForcedAbsLength, forcedAbsLength_ofCustomVector3_4, lengthRelScaleFactor_ofCustomVector3_4, observerCamera_ofCustomVector3_4, transform);
        }

        public Vector3 Get_customVector3_4_inLocalSpaceDefinedByParentUnits()
        {
            return Get_aCustomVector3_inLocalSpaceDefinedByParentUnits(vectorInterpretation_ofCustomVector3_4, source_ofCustomVector3_4, customVector3_4_clipboardForManualInput, customVector3_4_targetGameObject, customVector3_4_hasForcedAbsLength, forcedAbsLength_ofCustomVector3_4, lengthRelScaleFactor_ofCustomVector3_4, observerCamera_ofCustomVector3_4, transform);
        }

        public Vector3 Get_customVector3ofPartnerGameobject_inGlobalSpaceUnits()
        {
            if (partnerGameobject == null)
            {
                return Vector3.zero;
            }
            else
            {
                return Get_aCustomVector3_inGlobalSpaceUnits(vectorInterpretation_ofCustomVector3ofPartnerGameobject, source_ofCustomVector3ofPartnerGameobject, customVector3ofPartnerGameobject_clipboardForManualInput, customVector3ofPartnerGameobject_targetGameObject, customVector3ofPartnerGameobject_hasForcedAbsLength, forcedAbsLength_ofCustomVector3ofPartnerGameobject, lengthRelScaleFactor_ofCustomVector3ofPartnerGameobject, observerCamera_ofCustomVector3ofPartnerGameobject, partnerGameobject.transform);
            }
        }

        public Vector3 Get_customVector3ofPartnerGameobject_inLocalSpaceDefinedByParentUnits()
        {
            if (partnerGameobject == null)
            {
                return Vector3.zero;
            }
            else
            {
                return Get_aCustomVector3_inLocalSpaceDefinedByParentUnits(vectorInterpretation_ofCustomVector3ofPartnerGameobject, source_ofCustomVector3ofPartnerGameobject, customVector3ofPartnerGameobject_clipboardForManualInput, customVector3ofPartnerGameobject_targetGameObject, customVector3ofPartnerGameobject_hasForcedAbsLength, forcedAbsLength_ofCustomVector3ofPartnerGameobject, lengthRelScaleFactor_ofCustomVector3ofPartnerGameobject, observerCamera_ofCustomVector3ofPartnerGameobject, partnerGameobject.transform);
            }
        }

        Vector3 Get_aCustomVector3_inGlobalSpaceUnits(VectorInterpretation vectorInterpretation_ofCustomVector, CustomVector3Source source_ofCustomVector, Vector3 customVector_clipboardForManualInput, GameObject customVector_targetGameObject, bool customVector_hasForcedAbsLength, float forcedAbsLength_ofCustomVector, float lengthRelScaleFactor_ofCustomVector, DrawBasics.CameraForAutomaticOrientation observerCamera_ofCustomVector, Transform transformThatMountsTheCustomVector)
        {
            //The custom vector depends in some cases on other gameobjects than this, namely when "source_ofCustomVector == toOtherGameobject" or when "vectorInterpretation_ofCustomVector == local" (then depending on transform of parents)
            //-> Therefore the custom vector is calculated onDraw here in the MonoBehaviour instead of in the corresponding EditorClass.OnInspectorGUI(), since in this case it would only update there if this gameobject is selected.

            Vector3 vectorPreSpaceConversion_unscaled;
            Vector3 vectorPreSpaceConversion_scaled;

            Vector3 observerCamForward_normalized;
            Vector3 observerCamUp_normalized;
            Vector3 observerCamRight_normalized;
            Vector3 cam_to_observedPosition;

            if (vectorInterpretation_ofCustomVector == VectorInterpretation.globalSpace)
            {
                switch (source_ofCustomVector)
                {
                    case CustomVector3Source.manualInput:
                        vectorPreSpaceConversion_unscaled = customVector_clipboardForManualInput;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.toOtherGameobject:
                        vectorPreSpaceConversion_unscaled = Get_vector3ToOtherGameobject_preSpaceConversion_unscaled(customVector_targetGameObject, transformThatMountsTheCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.fromOtherGameobject:
                        vectorPreSpaceConversion_unscaled = -Get_vector3ToOtherGameobject_preSpaceConversion_unscaled(customVector_targetGameObject, transformThatMountsTheCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.transformsForward:
                        vectorPreSpaceConversion_unscaled = transformThatMountsTheCustomVector.forward;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.transformsUp:
                        vectorPreSpaceConversion_unscaled = transformThatMountsTheCustomVector.up;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.transformsRight:
                        vectorPreSpaceConversion_unscaled = transformThatMountsTheCustomVector.right;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.transformsBack:
                        vectorPreSpaceConversion_unscaled = (-transformThatMountsTheCustomVector.forward);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.transformsDown:
                        vectorPreSpaceConversion_unscaled = (-transformThatMountsTheCustomVector.up);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.transformsLeft:
                        vectorPreSpaceConversion_unscaled = (-transformThatMountsTheCustomVector.right);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.globalForward:
                        vectorPreSpaceConversion_unscaled = Vector3.forward;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.globalUp:
                        vectorPreSpaceConversion_unscaled = Vector3.up;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.globalRight:
                        vectorPreSpaceConversion_unscaled = Vector3.right;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.globalBack:
                        vectorPreSpaceConversion_unscaled = Vector3.back;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.globalDown:
                        vectorPreSpaceConversion_unscaled = Vector3.down;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.globalLeft:
                        vectorPreSpaceConversion_unscaled = Vector3.left;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.observerCameraForward:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = observerCamForward_normalized;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.observerCameraUp:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = observerCamUp_normalized;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.observerCameraRight:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = observerCamRight_normalized;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.observerCameraBack:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = (-observerCamForward_normalized);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.observerCameraDown:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = (-observerCamUp_normalized);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.observerCameraLeft:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = (-observerCamRight_normalized);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.observerCameraToThisGameobject:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = cam_to_observedPosition;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    default:
                        return default(Vector3);
                }
            }
            else
            {
                switch (source_ofCustomVector)
                {
                    case CustomVector3Source.manualInput:
                        vectorPreSpaceConversion_unscaled = customVector_clipboardForManualInput;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_localToGlobal(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.toOtherGameobject:
                        vectorPreSpaceConversion_unscaled = Get_vector3ToOtherGameobject_preSpaceConversion_unscaled(customVector_targetGameObject, transformThatMountsTheCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == toOtherGameobject" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.fromOtherGameobject:
                        vectorPreSpaceConversion_unscaled = -Get_vector3ToOtherGameobject_preSpaceConversion_unscaled(customVector_targetGameObject, transformThatMountsTheCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == toOtherGameobject" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.transformsForward:
                        vectorPreSpaceConversion_unscaled = transformThatMountsTheCustomVector.forward;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_localToGlobal(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector);
                    case CustomVector3Source.transformsUp:
                        vectorPreSpaceConversion_unscaled = transformThatMountsTheCustomVector.up;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_localToGlobal(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector);
                    case CustomVector3Source.transformsRight:
                        vectorPreSpaceConversion_unscaled = transformThatMountsTheCustomVector.right;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_localToGlobal(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector);
                    case CustomVector3Source.transformsBack:
                        vectorPreSpaceConversion_unscaled = (-transformThatMountsTheCustomVector.forward);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_localToGlobal(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector);
                    case CustomVector3Source.transformsDown:
                        vectorPreSpaceConversion_unscaled = (-transformThatMountsTheCustomVector.up);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_localToGlobal(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector);
                    case CustomVector3Source.transformsLeft:
                        vectorPreSpaceConversion_unscaled = (-transformThatMountsTheCustomVector.right);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_localToGlobal(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector);
                    case CustomVector3Source.globalForward:
                        vectorPreSpaceConversion_unscaled = Vector3.forward;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == globalForward" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.globalUp:
                        vectorPreSpaceConversion_unscaled = Vector3.up;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == globalUp" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.globalRight:
                        vectorPreSpaceConversion_unscaled = Vector3.right;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == globalRight" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.globalBack:
                        vectorPreSpaceConversion_unscaled = Vector3.back;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == globalBack" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.globalDown:
                        vectorPreSpaceConversion_unscaled = Vector3.down;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == globalDown" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.globalLeft:
                        vectorPreSpaceConversion_unscaled = Vector3.left;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == globalLeft" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.observerCameraForward:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = observerCamForward_normalized;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == observerCameraForward" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.observerCameraUp:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = observerCamUp_normalized;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == observerCameraUp" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.observerCameraRight:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = observerCamRight_normalized;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == observerCameraRight" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.observerCameraBack:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = (-observerCamForward_normalized);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == observerCameraBack" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.observerCameraDown:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = (-observerCamUp_normalized);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == observerCameraDown" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.observerCameraLeft:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = (-observerCamRight_normalized);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == observerCameraLeft" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.observerCameraToThisGameobject:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = cam_to_observedPosition;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == observerCameraToThisGameobject" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    default:
                        return default(Vector3);
                }
            }
        }

        Vector3 Get_aCustomVector3_inLocalSpaceDefinedByParentUnits(VectorInterpretation vectorInterpretation_ofCustomVector, CustomVector3Source source_ofCustomVector, Vector3 customVector_clipboardForManualInput, GameObject customVector_targetGameObject, bool customVector_hasForcedAbsLength, float forcedAbsLength_ofCustomVector, float lengthRelScaleFactor_ofCustomVector, DrawBasics.CameraForAutomaticOrientation observerCamera_ofCustomVector, Transform transformThatMountsTheCustomVector)
        {
            //-> see notes inside "Get_aCustomVector3_inGlobalSpaceUnits"

            Vector3 vectorPreSpaceConversion_unscaled;
            Vector3 vectorPreSpaceConversion_scaled;

            Vector3 observerCamForward_normalized;
            Vector3 observerCamUp_normalized;
            Vector3 observerCamRight_normalized;
            Vector3 cam_to_observedPosition;

            if (vectorInterpretation_ofCustomVector == VectorInterpretation.localSpaceDefinedByParent)
            {
                switch (source_ofCustomVector)
                {
                    case CustomVector3Source.manualInput:
                        vectorPreSpaceConversion_unscaled = customVector_clipboardForManualInput;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector3Source.toOtherGameobject:
                        vectorPreSpaceConversion_unscaled = Get_vector3ToOtherGameobject_preSpaceConversion_unscaled(customVector_targetGameObject, transformThatMountsTheCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == toOtherGameobject" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.fromOtherGameobject:
                        vectorPreSpaceConversion_unscaled = -Get_vector3ToOtherGameobject_preSpaceConversion_unscaled(customVector_targetGameObject, transformThatMountsTheCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == toOtherGameobject" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.transformsForward:
                        vectorPreSpaceConversion_unscaled = transformThatMountsTheCustomVector.forward;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector); //-> could alse be derived via "Vector3.forward" (acting as "transform.forward" in localSpace of this.transform), which gets converted one hierarchy-layer towards "more global", so it is inside localSpace defined by parent.
                    case CustomVector3Source.transformsUp:
                        vectorPreSpaceConversion_unscaled = transformThatMountsTheCustomVector.up;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector); //-> could alse be derived via "Vector3.up" (acting as "transform.up" in localSpace of this.transform), which gets converted one hierarchy-layer towards "more global", so it is inside localSpace defined by parent.
                    case CustomVector3Source.transformsRight:
                        vectorPreSpaceConversion_unscaled = transformThatMountsTheCustomVector.right;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector); //-> could alse be derived via "Vector3.right" (acting as "transform.right" in localSpace of this.transform), which gets converted one hierarchy-layer towards "more global", so it is inside localSpace defined by parent.
                    case CustomVector3Source.transformsBack:
                        vectorPreSpaceConversion_unscaled = (-transformThatMountsTheCustomVector.forward);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector); //-> could alse be derived via "Vector3.back" (acting as "transform.back" in localSpace of this.transform), which gets converted one hierarchy-layer towards "more global", so it is inside localSpace defined by parent.
                    case CustomVector3Source.transformsDown:
                        vectorPreSpaceConversion_unscaled = (-transformThatMountsTheCustomVector.up);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector); //-> could alse be derived via "Vector3.down" (acting as "transform.down" in localSpace of this.transform), which gets converted one hierarchy-layer towards "more global", so it is inside localSpace defined by parent.
                    case CustomVector3Source.transformsLeft:
                        vectorPreSpaceConversion_unscaled = (-transformThatMountsTheCustomVector.right);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector); //-> could alse be derived via "Vector3.left" (acting as "transform.left" in localSpace of this.transform), which gets converted one hierarchy-layer towards "more global", so it is inside localSpace defined by parent.
                    case CustomVector3Source.globalForward:
                        vectorPreSpaceConversion_unscaled = Vector3.forward;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == globalForward" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.globalUp:
                        vectorPreSpaceConversion_unscaled = Vector3.up;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == globalUp" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.globalRight:
                        vectorPreSpaceConversion_unscaled = Vector3.right;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == globalRight" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.globalBack:
                        vectorPreSpaceConversion_unscaled = Vector3.back;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == globalBack" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.globalDown:
                        vectorPreSpaceConversion_unscaled = Vector3.down;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == globalDown" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.globalLeft:
                        vectorPreSpaceConversion_unscaled = Vector3.left;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == globalLeft" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.observerCameraForward:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = observerCamForward_normalized;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == observerCameraForward" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.observerCameraUp:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = observerCamUp_normalized;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == observerCameraUp" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.observerCameraRight:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = observerCamRight_normalized;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == observerCameraRight" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.observerCameraBack:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = (-observerCamForward_normalized);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == observerCameraBack" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.observerCameraDown:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = (-observerCamUp_normalized);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == observerCameraDown" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.observerCameraLeft:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = (-observerCamRight_normalized);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == observerCameraLeft" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector3Source.observerCameraToThisGameobject:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = cam_to_observedPosition;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == observerCameraToThisGameobject" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    default:
                        return default(Vector3);
                }
            }
            else
            {
                switch (source_ofCustomVector)
                {
                    case CustomVector3Source.manualInput:
                        vectorPreSpaceConversion_unscaled = customVector_clipboardForManualInput;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.toOtherGameobject:
                        vectorPreSpaceConversion_unscaled = Get_vector3ToOtherGameobject_preSpaceConversion_unscaled(customVector_targetGameObject, transformThatMountsTheCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.fromOtherGameobject:
                        vectorPreSpaceConversion_unscaled = -Get_vector3ToOtherGameobject_preSpaceConversion_unscaled(customVector_targetGameObject, transformThatMountsTheCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.transformsForward:
                        vectorPreSpaceConversion_unscaled = transformThatMountsTheCustomVector.forward;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.transformsUp:
                        vectorPreSpaceConversion_unscaled = transformThatMountsTheCustomVector.up;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.transformsRight:
                        vectorPreSpaceConversion_unscaled = transformThatMountsTheCustomVector.right;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.transformsBack:
                        vectorPreSpaceConversion_unscaled = (-transformThatMountsTheCustomVector.forward);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.transformsDown:
                        vectorPreSpaceConversion_unscaled = (-transformThatMountsTheCustomVector.up);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.transformsLeft:
                        vectorPreSpaceConversion_unscaled = (-transformThatMountsTheCustomVector.right);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.globalForward:
                        vectorPreSpaceConversion_unscaled = Vector3.forward;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.globalUp:
                        vectorPreSpaceConversion_unscaled = Vector3.up;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.globalRight:
                        vectorPreSpaceConversion_unscaled = Vector3.right;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.globalBack:
                        vectorPreSpaceConversion_unscaled = Vector3.back;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.globalDown:
                        vectorPreSpaceConversion_unscaled = Vector3.down;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.globalLeft:
                        vectorPreSpaceConversion_unscaled = Vector3.left;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.observerCameraForward:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = observerCamForward_normalized;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.observerCameraUp:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = observerCamUp_normalized;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.observerCameraRight:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = observerCamRight_normalized;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.observerCameraBack:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = (-observerCamForward_normalized);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.observerCameraDown:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = (-observerCamUp_normalized);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.observerCameraLeft:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = (-observerCamRight_normalized);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector3Source.observerCameraToThisGameobject:
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out observerCamForward_normalized, out observerCamUp_normalized, out observerCamRight_normalized, out cam_to_observedPosition, transformThatMountsTheCustomVector.position, observerCamera_ofCustomVector);
                        vectorPreSpaceConversion_unscaled = cam_to_observedPosition;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector3(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector3_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    default:
                        return default(Vector3);
                }
            }
        }

        Vector3 ScaleCustomVector3(Vector3 unscaledVector, bool customVector_hasForcedAbsLength, float forcedAbsLength_ofCustomVector, float lengthRelScaleFactor_ofCustomVector)
        {
            if (customVector_hasForcedAbsLength)
            {
                return (unscaledVector.normalized * forcedAbsLength_ofCustomVector);
            }
            else
            {
                return (unscaledVector * lengthRelScaleFactor_ofCustomVector);
            }
        }

        Vector3 CustomVector3_globalToLocalSpaceDefinedByParent(Vector3 customVector_global, bool convertOnlyRotation_butNotScale, Transform transformThatMountsTheCustomVector)
        {
            if (transformThatMountsTheCustomVector.parent == null)
            {
                return customVector_global;
            }
            else
            {
                Vector3 vector_inverseRotated = Quaternion.Inverse(transformThatMountsTheCustomVector.parent.rotation) * customVector_global;
                if (convertOnlyRotation_butNotScale)
                {
                    return vector_inverseRotated;
                }
                else
                {
                    if (UtilitiesDXXL_Math.ContainsZeroComponents(transformThatMountsTheCustomVector.parent.lossyScale))
                    {
                        //-> at least one dimension of the local space is shrinked to the size of 0.
                        //-> scaling from global space to local space skipped, because
                        //---> the smaller the local space shrinks, the bigger the components of a vector (with fixed size in global space) will become in this local space.
                        //---> each vector in global space would become a vector of infinity-length in a local space of scale=0 
                        //---> or formulated the other way round: a vector in units of a "local space of scale=0" would have to have a length of infinity to be represented as a (non-zero) vector in global space.
                        return vector_inverseRotated;
                    }
                    else
                    {
                        //scale inverse:
                        return new Vector3(vector_inverseRotated.x / transformThatMountsTheCustomVector.parent.lossyScale.x, vector_inverseRotated.y / transformThatMountsTheCustomVector.parent.lossyScale.y, vector_inverseRotated.z / transformThatMountsTheCustomVector.parent.lossyScale.z);
                    }
                }
            }
        }

        Vector3 CustomVector3_localToGlobal(Vector3 customVector_local, bool convertOnlyScale_butNotRotation, Transform transformThatMountsTheCustomVector)
        {
            if (transformThatMountsTheCustomVector.parent == null)
            {
                return customVector_local;
            }
            else
            {
                Vector3 customVector_scaled = Vector3.Scale(transformThatMountsTheCustomVector.parent.lossyScale, customVector_local);
                if (convertOnlyScale_butNotRotation)
                {
                    return customVector_scaled;
                }
                else
                {
                    return (transformThatMountsTheCustomVector.parent.rotation * customVector_scaled);
                }
            }
        }

        Vector3 Get_vector3ToOtherGameobject_preSpaceConversion_unscaled(GameObject customVector_targetGameObject, Transform transformThatMountsTheCustomVector)
        {
            if (customVector_targetGameObject == null)
            {
                return Vector3.zero;
            }
            else
            {
                return (customVector_targetGameObject.transform.position - transformThatMountsTheCustomVector.position);
            }
        }

        public Vector2 Get_customVector2_1_inGlobalSpaceUnits()
        {
            return Get_aCustomVector2_inGlobalSpaceUnits(vectorInterpretation_ofCustomVector2_1, source_ofCustomVector2_1, rotationFromRight_ofCustomVector2_1, customVector2_1_clipboardForManualInput, customVector2_1_targetGameObject, customVector2_1_hasForcedAbsLength, forcedAbsLength_ofCustomVector2_1, lengthRelScaleFactor_ofCustomVector2_1, transform);
        }

        public Vector2 Get_customVector2_1_inLocalSpaceDefinedByParentUnits()
        {
            return Get_aCustomVector2_inLocalSpaceDefinedByParentUnits(vectorInterpretation_ofCustomVector2_1, source_ofCustomVector2_1, rotationFromRight_ofCustomVector2_1, customVector2_1_clipboardForManualInput, customVector2_1_targetGameObject, customVector2_1_hasForcedAbsLength, forcedAbsLength_ofCustomVector2_1, lengthRelScaleFactor_ofCustomVector2_1, transform);
        }

        public Vector2 Get_customVector2_2_inGlobalSpaceUnits()
        {
            return Get_aCustomVector2_inGlobalSpaceUnits(vectorInterpretation_ofCustomVector2_2, source_ofCustomVector2_2, rotationFromRight_ofCustomVector2_2, customVector2_2_clipboardForManualInput, customVector2_2_targetGameObject, customVector2_2_hasForcedAbsLength, forcedAbsLength_ofCustomVector2_2, lengthRelScaleFactor_ofCustomVector2_2, transform);
        }

        public Vector2 Get_customVector2_2_inLocalSpaceDefinedByParentUnits()
        {
            return Get_aCustomVector2_inLocalSpaceDefinedByParentUnits(vectorInterpretation_ofCustomVector2_2, source_ofCustomVector2_2, rotationFromRight_ofCustomVector2_2, customVector2_2_clipboardForManualInput, customVector2_2_targetGameObject, customVector2_2_hasForcedAbsLength, forcedAbsLength_ofCustomVector2_2, lengthRelScaleFactor_ofCustomVector2_2, transform);
        }

        public Vector2 Get_customVector2_3_inGlobalSpaceUnits()
        {
            return Get_aCustomVector2_inGlobalSpaceUnits(vectorInterpretation_ofCustomVector2_3, source_ofCustomVector2_3, rotationFromRight_ofCustomVector2_3, customVector2_3_clipboardForManualInput, customVector2_3_targetGameObject, customVector2_3_hasForcedAbsLength, forcedAbsLength_ofCustomVector2_3, lengthRelScaleFactor_ofCustomVector2_3, transform);
        }

        public Vector2 Get_customVector2_3_inLocalSpaceDefinedByParentUnits()
        {
            return Get_aCustomVector2_inLocalSpaceDefinedByParentUnits(vectorInterpretation_ofCustomVector2_3, source_ofCustomVector2_3, rotationFromRight_ofCustomVector2_3, customVector2_3_clipboardForManualInput, customVector2_3_targetGameObject, customVector2_3_hasForcedAbsLength, forcedAbsLength_ofCustomVector2_3, lengthRelScaleFactor_ofCustomVector2_3, transform);
        }

        public Vector2 Get_customVector2_4_inGlobalSpaceUnits()
        {
            return Get_aCustomVector2_inGlobalSpaceUnits(vectorInterpretation_ofCustomVector2_4, source_ofCustomVector2_4, rotationFromRight_ofCustomVector2_4, customVector2_4_clipboardForManualInput, customVector2_4_targetGameObject, customVector2_4_hasForcedAbsLength, forcedAbsLength_ofCustomVector2_4, lengthRelScaleFactor_ofCustomVector2_4, transform);
        }

        public Vector2 Get_customVector2_4_inLocalSpaceDefinedByParentUnits()
        {
            return Get_aCustomVector2_inLocalSpaceDefinedByParentUnits(vectorInterpretation_ofCustomVector2_4, source_ofCustomVector2_4, rotationFromRight_ofCustomVector2_4, customVector2_4_clipboardForManualInput, customVector2_4_targetGameObject, customVector2_4_hasForcedAbsLength, forcedAbsLength_ofCustomVector2_4, lengthRelScaleFactor_ofCustomVector2_4, transform);
        }

        public Vector2 Get_customVector2ofPartnerGameobject_inGlobalSpaceUnits()
        {
            if (partnerGameobject == null)
            {
                return Vector2.zero;
            }
            else
            {
                return Get_aCustomVector2_inGlobalSpaceUnits(vectorInterpretation_ofCustomVector2ofPartnerGameobject, source_ofCustomVector2ofPartnerGameobject, rotationFromRight_ofCustomVector2ofPartnerGameobject, customVector2ofPartnerGameobject_clipboardForManualInput, customVector2ofPartnerGameobject_targetGameObject, customVector2ofPartnerGameobject_hasForcedAbsLength, forcedAbsLength_ofCustomVector2ofPartnerGameobject, lengthRelScaleFactor_ofCustomVector2ofPartnerGameobject, partnerGameobject.transform);
            }
        }

        public Vector2 Get_customVector2ofPartnerGameobject_inLocalSpaceDefinedByParentUnits()
        {
            if (partnerGameobject == null)
            {
                return Vector2.zero;
            }
            else
            {
                return Get_aCustomVector2_inLocalSpaceDefinedByParentUnits(vectorInterpretation_ofCustomVector2ofPartnerGameobject, source_ofCustomVector2ofPartnerGameobject, rotationFromRight_ofCustomVector2ofPartnerGameobject, customVector2ofPartnerGameobject_clipboardForManualInput, customVector2ofPartnerGameobject_targetGameObject, customVector2ofPartnerGameobject_hasForcedAbsLength, forcedAbsLength_ofCustomVector2ofPartnerGameobject, lengthRelScaleFactor_ofCustomVector2ofPartnerGameobject, partnerGameobject.transform);
            }
        }

        Vector2 Get_aCustomVector2_inGlobalSpaceUnits(VectorInterpretation vectorInterpretation_ofCustomVector, CustomVector2Source source_ofCustomVector, float rotationFromRight_ofCustomVector, Vector2 customVector_clipboardForManualInput, GameObject customVector_targetGameObject, bool customVector_hasForcedAbsLength, float forcedAbsLength_ofCustomVector, float lengthRelScaleFactor_ofCustomVector, Transform transformThatMountsTheCustomVector)
        {
            //-> see notes inside "Get_aCustomVector3_inGlobalSpaceUnits"

            Vector2 vectorPreSpaceConversion_unscaled;
            Vector2 vectorPreSpaceConversion_scaled;
            if (vectorInterpretation_ofCustomVector == VectorInterpretation.globalSpace)
            {
                switch (source_ofCustomVector)
                {
                    case CustomVector2Source.rotationAroundZStartingFromRight:
                        vectorPreSpaceConversion_unscaled = GetToLeftVectorRotatedAroundZ_asV2(rotationFromRight_ofCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector2Source.manualInput:
                        vectorPreSpaceConversion_unscaled = customVector_clipboardForManualInput;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector2Source.toOtherGameobject:
                        vectorPreSpaceConversion_unscaled = Get_vector2ToOtherGameobject_preSpaceConversion_unscaled(customVector_targetGameObject, transformThatMountsTheCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector2Source.fromOtherGameobject:
                        vectorPreSpaceConversion_unscaled = -Get_vector2ToOtherGameobject_preSpaceConversion_unscaled(customVector_targetGameObject, transformThatMountsTheCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector2Source.transformsUp:
                        vectorPreSpaceConversion_unscaled = new Vector2(transformThatMountsTheCustomVector.up.x, transformThatMountsTheCustomVector.up.y);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector2Source.transformsRight:
                        vectorPreSpaceConversion_unscaled = new Vector2(transformThatMountsTheCustomVector.right.x, transformThatMountsTheCustomVector.right.y);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector2Source.transformsDown:
                        vectorPreSpaceConversion_unscaled = new Vector2((-transformThatMountsTheCustomVector.up).x, (-transformThatMountsTheCustomVector.up).y);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector2Source.transformsLeft:
                        vectorPreSpaceConversion_unscaled = new Vector2((-transformThatMountsTheCustomVector.right).x, (-transformThatMountsTheCustomVector.right).y);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector2Source.globalUp:
                        vectorPreSpaceConversion_unscaled = Vector2.up;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector2Source.globalRight:
                        vectorPreSpaceConversion_unscaled = Vector2.right;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector2Source.globalDown:
                        vectorPreSpaceConversion_unscaled = Vector2.down;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector2Source.globalLeft:
                        vectorPreSpaceConversion_unscaled = Vector2.left;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    default:
                        return default(Vector2);
                }
            }
            else
            {
                switch (source_ofCustomVector)
                {
                    case CustomVector2Source.rotationAroundZStartingFromRight:
                        vectorPreSpaceConversion_unscaled = GetToLeftVectorRotatedAroundZ_asV2(rotationFromRight_ofCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_localToGlobal(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector2Source.manualInput:
                        vectorPreSpaceConversion_unscaled = customVector_clipboardForManualInput;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_localToGlobal(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector2Source.toOtherGameobject:
                        vectorPreSpaceConversion_unscaled = Get_vector2ToOtherGameobject_preSpaceConversion_unscaled(customVector_targetGameObject, transformThatMountsTheCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == toOtherGameobject" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector2Source.fromOtherGameobject:
                        vectorPreSpaceConversion_unscaled = -Get_vector2ToOtherGameobject_preSpaceConversion_unscaled(customVector_targetGameObject, transformThatMountsTheCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == toOtherGameobject" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector2Source.transformsUp:
                        vectorPreSpaceConversion_unscaled = new Vector2(transformThatMountsTheCustomVector.up.x, transformThatMountsTheCustomVector.up.y);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_localToGlobal(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector);
                    case CustomVector2Source.transformsRight:
                        vectorPreSpaceConversion_unscaled = new Vector2(transformThatMountsTheCustomVector.right.x, transformThatMountsTheCustomVector.right.y);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_localToGlobal(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector);
                    case CustomVector2Source.transformsDown:
                        vectorPreSpaceConversion_unscaled = new Vector2((-transformThatMountsTheCustomVector.up).x, (-transformThatMountsTheCustomVector.up).y);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_localToGlobal(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector);
                    case CustomVector2Source.transformsLeft:
                        vectorPreSpaceConversion_unscaled = new Vector2((-transformThatMountsTheCustomVector.right).x, (-transformThatMountsTheCustomVector.right).y);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_localToGlobal(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector);
                    case CustomVector2Source.globalUp:
                        vectorPreSpaceConversion_unscaled = Vector2.up;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == globalUp" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector2Source.globalRight:
                        vectorPreSpaceConversion_unscaled = Vector2.right;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == globalRight" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector2Source.globalDown:
                        vectorPreSpaceConversion_unscaled = Vector2.down;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == globalDown" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector2Source.globalLeft:
                        vectorPreSpaceConversion_unscaled = Vector2.left;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled; //"vector source == globalLeft" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    default:
                        return default(Vector2);
                }
            }
        }

        Vector2 Get_aCustomVector2_inLocalSpaceDefinedByParentUnits(VectorInterpretation vectorInterpretation_ofCustomVector, CustomVector2Source source_ofCustomVector, float rotationFromRight_ofCustomVector, Vector2 customVector_clipboardForManualInput, GameObject customVector_targetGameObject, bool customVector_hasForcedAbsLength, float forcedAbsLength_ofCustomVector, float lengthRelScaleFactor_ofCustomVector, Transform transformThatMountsTheCustomVector)
        {
            //-> see notes inside "Get_aCustomVector3_inGlobalSpaceUnits"

            Vector2 vectorPreSpaceConversion_unscaled;
            Vector2 vectorPreSpaceConversion_scaled;
            if (vectorInterpretation_ofCustomVector == VectorInterpretation.localSpaceDefinedByParent)
            {
                switch (source_ofCustomVector)
                {
                    case CustomVector2Source.rotationAroundZStartingFromRight:
                        vectorPreSpaceConversion_unscaled = GetToLeftVectorRotatedAroundZ_asV2(rotationFromRight_ofCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector2Source.manualInput:
                        vectorPreSpaceConversion_unscaled = customVector_clipboardForManualInput;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return vectorPreSpaceConversion_scaled;
                    case CustomVector2Source.toOtherGameobject:
                        vectorPreSpaceConversion_unscaled = Get_vector2ToOtherGameobject_preSpaceConversion_unscaled(customVector_targetGameObject, transformThatMountsTheCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == toOtherGameobject" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector2Source.fromOtherGameobject:
                        vectorPreSpaceConversion_unscaled = -Get_vector2ToOtherGameobject_preSpaceConversion_unscaled(customVector_targetGameObject, transformThatMountsTheCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == toOtherGameobject" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector2Source.transformsUp:
                        vectorPreSpaceConversion_unscaled = new Vector2(transformThatMountsTheCustomVector.up.x, transformThatMountsTheCustomVector.up.y);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector); //-> could alse be derived via "Vector3.up" (acting as "transform.up" in localSpace of this.transform), which gets converted one hierarchy-layer towards "more global", so it is inside localSpace defined by parent.
                    case CustomVector2Source.transformsRight:
                        vectorPreSpaceConversion_unscaled = new Vector2(transformThatMountsTheCustomVector.right.x, transformThatMountsTheCustomVector.right.y);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector); //-> could alse be derived via "Vector3.right" (acting as "transform.right" in localSpace of this.transform), which gets converted one hierarchy-layer towards "more global", so it is inside localSpace defined by parent.
                    case CustomVector2Source.transformsDown:
                        vectorPreSpaceConversion_unscaled = new Vector2((-transformThatMountsTheCustomVector.up).x, (-transformThatMountsTheCustomVector.up).y);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector); //-> could alse be derived via "Vector3.down" (acting as "transform.down" in localSpace of this.transform), which gets converted one hierarchy-layer towards "more global", so it is inside localSpace defined by parent.
                    case CustomVector2Source.transformsLeft:
                        vectorPreSpaceConversion_unscaled = new Vector2((-transformThatMountsTheCustomVector.right).x, (-transformThatMountsTheCustomVector.right).y);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, true, transformThatMountsTheCustomVector); //-> could alse be derived via "Vector3.left" (acting as "transform.left" in localSpace of this.transform), which gets converted one hierarchy-layer towards "more global", so it is inside localSpace defined by parent.
                    case CustomVector2Source.globalUp:
                        vectorPreSpaceConversion_unscaled = Vector2.up;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == globalUp" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector2Source.globalRight:
                        vectorPreSpaceConversion_unscaled = Vector2.right;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == globalRight" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector2Source.globalDown:
                        vectorPreSpaceConversion_unscaled = Vector2.down;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == globalDown" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    case CustomVector2Source.globalLeft:
                        vectorPreSpaceConversion_unscaled = Vector2.left;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector); //"vector source == globalLeft" can not be selected in the inspector as "interpretation == localSpaceDefinedByParent", so this case cannot happen. The fallback here behaves equal to the "interpretation == globalSpace"-thread
                    default:
                        return default(Vector2);
                }
            }
            else
            {
                switch (source_ofCustomVector)
                {
                    case CustomVector2Source.rotationAroundZStartingFromRight:
                        vectorPreSpaceConversion_unscaled = GetToLeftVectorRotatedAroundZ_asV2(rotationFromRight_ofCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector2Source.manualInput:
                        vectorPreSpaceConversion_unscaled = customVector_clipboardForManualInput;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector2Source.toOtherGameobject:
                        vectorPreSpaceConversion_unscaled = Get_vector2ToOtherGameobject_preSpaceConversion_unscaled(customVector_targetGameObject, transformThatMountsTheCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector2Source.fromOtherGameobject:
                        vectorPreSpaceConversion_unscaled = -Get_vector2ToOtherGameobject_preSpaceConversion_unscaled(customVector_targetGameObject, transformThatMountsTheCustomVector);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector2Source.transformsUp:
                        vectorPreSpaceConversion_unscaled = new Vector2(transformThatMountsTheCustomVector.up.x, transformThatMountsTheCustomVector.up.y);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector2Source.transformsRight:
                        vectorPreSpaceConversion_unscaled = new Vector2(transformThatMountsTheCustomVector.right.x, transformThatMountsTheCustomVector.right.y);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector2Source.transformsDown:
                        vectorPreSpaceConversion_unscaled = new Vector2((-transformThatMountsTheCustomVector.up).x, (-transformThatMountsTheCustomVector.up).y);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector2Source.transformsLeft:
                        vectorPreSpaceConversion_unscaled = new Vector2((-transformThatMountsTheCustomVector.right).x, (-transformThatMountsTheCustomVector.right).y);
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector2Source.globalUp:
                        vectorPreSpaceConversion_unscaled = Vector2.up;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector2Source.globalRight:
                        vectorPreSpaceConversion_unscaled = Vector2.right;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector2Source.globalDown:
                        vectorPreSpaceConversion_unscaled = Vector2.down;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    case CustomVector2Source.globalLeft:
                        vectorPreSpaceConversion_unscaled = Vector2.left;
                        vectorPreSpaceConversion_scaled = ScaleCustomVector2(vectorPreSpaceConversion_unscaled, customVector_hasForcedAbsLength, forcedAbsLength_ofCustomVector, lengthRelScaleFactor_ofCustomVector);
                        return CustomVector2_globalToLocalSpaceDefinedByParent(vectorPreSpaceConversion_scaled, false, transformThatMountsTheCustomVector);
                    default:
                        return default(Vector2);
                }
            }
        }

        Vector2 ScaleCustomVector2(Vector2 unscaledVector, bool customVector_hasForcedAbsLength, float forcedAbsLength_ofCustomVector, float lengthRelScaleFactor_ofCustomVector)
        {
            if (customVector_hasForcedAbsLength)
            {
                return (unscaledVector.normalized * forcedAbsLength_ofCustomVector);
            }
            else
            {
                return (unscaledVector * lengthRelScaleFactor_ofCustomVector);
            }
        }

        Vector2 Get_vector2ToOtherGameobject_preSpaceConversion_unscaled(GameObject customVector_targetGameObject, Transform transformThatMountsTheCustomVector)
        {
            if (customVector_targetGameObject == null)
            {
                return Vector2.zero;
            }
            else
            {
                Vector2 thisTransformsPosition_asV2 = new Vector2(transformThatMountsTheCustomVector.position.x, transformThatMountsTheCustomVector.position.y);
                Vector2 otherGameobjectsPosition_asV2 = new Vector2(customVector_targetGameObject.transform.position.x, customVector_targetGameObject.transform.position.y);
                return (otherGameobjectsPosition_asV2 - thisTransformsPosition_asV2);
            }
        }

        Vector2 CustomVector2_localToGlobal(Vector2 customVector_local, bool convertOnlyScale_butNotRotation, Transform transformThatMountsTheCustomVector)
        {
            if (transformThatMountsTheCustomVector.parent == null)
            {
                return customVector_local;
            }
            else
            {
                Vector2 customVector_local_asV2 = new Vector2(customVector_local.x, customVector_local.y);
                Vector2 parentsLossyScale_asV2 = new Vector2(transformThatMountsTheCustomVector.parent.lossyScale.x, transformThatMountsTheCustomVector.parent.lossyScale.y);
                Vector2 customVector_scaled = Vector2.Scale(parentsLossyScale_asV2, customVector_local_asV2);
                if (convertOnlyScale_butNotRotation)
                {
                    return customVector_scaled;
                }
                else
                {
                    Vector3 customVector_scaled_asV3 = new Vector3(customVector_scaled.x, customVector_scaled.y, 0.0f);
                    Vector3 customVector_scaledAndRotated_asV3 = (transformThatMountsTheCustomVector.parent.rotation * customVector_scaled_asV3);
                    Vector2 customVector_scaledAndRotated_asV2 = new Vector2(customVector_scaledAndRotated_asV3.x, customVector_scaledAndRotated_asV3.y);
                    return customVector_scaledAndRotated_asV2;
                }
            }
        }

        Vector2 CustomVector2_globalToLocalSpaceDefinedByParent(Vector2 customVector_global, bool convertOnlyRotation_butNotScale, Transform transformThatMountsTheCustomVector)
        {
            if (transformThatMountsTheCustomVector.parent == null)
            {
                return customVector_global;
            }
            else
            {
                Quaternion inverseRotation_ofParent = Quaternion.Inverse(transformThatMountsTheCustomVector.parent.rotation);
                Vector3 customVector_global_asV3 = new Vector3(customVector_global.x, customVector_global.y, 0.0f);
                Vector3 customVector_global_inverseRotated_asV3 = inverseRotation_ofParent * customVector_global_asV3;
                Vector2 customVector_global_inverseRotated_asV2 = new Vector2(customVector_global_inverseRotated_asV3.x, customVector_global_inverseRotated_asV3.y);

                if (convertOnlyRotation_butNotScale)
                {
                    return customVector_global_inverseRotated_asV2;
                }
                else
                {
                    if (UtilitiesDXXL_Math.ContainsZeroComponentsInXorY(transformThatMountsTheCustomVector.parent.lossyScale))
                    {
                        //-> see note in "CustomVector3_globalToLocalSpaceDefinedByParent()"
                        return customVector_global_inverseRotated_asV2;
                    }
                    else
                    {
                        //scale inverse:
                        return new Vector2(customVector_global_inverseRotated_asV2.x / transformThatMountsTheCustomVector.parent.lossyScale.x, customVector_global_inverseRotated_asV2.y / transformThatMountsTheCustomVector.parent.lossyScale.y);
                    }
                }
            }
        }

        Vector2 GetToLeftVectorRotatedAroundZ_asV2(float rotationFromRight_ofCustomVector)
        {
            Quaternion rotation_fromRight = Quaternion.AngleAxis(rotationFromRight_ofCustomVector, Vector3.forward);
            Vector3 vectorPreSpaceConversion_unscaled_asV3 = rotation_fromRight * Vector3.right;
            return (new Vector2(vectorPreSpaceConversion_unscaled_asV3.x, vectorPreSpaceConversion_unscaled_asV3.y));
        }

    }

}
