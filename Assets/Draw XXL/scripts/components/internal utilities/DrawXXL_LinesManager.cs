namespace DrawXXL
{
    using UnityEngine;
    using UnityEngine.Rendering;
    using System.Collections.Generic;

    [HelpURL("https://www.symphonygames.net/drawxxldocumentation/index.html")]
    [AddComponentMenu("Draw XXL/Internal Not For Manual Creation/Draw XXL Lines Manager")]
    [DefaultExecutionOrder(32190)] //negative numers are early, positive numbers are late. Range is till (incl) 32000 to both negative and positive direction.
    [ExecuteInEditMode]
    public class DrawXXL_LinesManager : MonoBehaviour
    {
        //This class contains (beside other things) a "virtual gizmo cylce mechanic". Details see here:
        //-> It is just communicating to the static line counter, when a new OnDrawGizmo-cylce starts (via "UtilitiesDXXL_Components.ReportOnDrawGizmosCycleOfAMonoBehaviour()"). With that information the static line counter knows when to reset the lineCount.
        //-> It has no effect on the functionality if this component exists multiple times in the scene, except that "gizmoLineCountManagerAutomaticallyRepaintsRendering" is "true", if ONE of the components has it as "true"
        //-> Also this component has two further functions beside acting as "count" manager:
        //---> Continuously repainting the Editor Windows depending on "gizmoLineCountManagerAutomaticallyRepaintsRendering" (see explanation in the inspector of the component)
        //---> The "DrawBasics.UsedUnityLineDrawingMethod.debugLinesInPlayMode_gizmoLinesInEditModeAndPlaymodePauses"-mechanic depends on the "UtilitiesDXXL_Components.virtualGizmoCycleCount"-cycle, which is triggered here with this component
        //-> Except for the "continuous repaint"-functionality any other drawer component already has the functionality implicitly inside, so if you can renounce of the continuous repaint and have already any drawer component in the scene, then you don't need this component here.

        /// -------

        //-> when a "DrawXXL_LinesManager" gameobject/component is created via "automatic()" during playmode, then it is not there anymore after exiting playmode. "DrawBasics.CreateGizmoLineCountManager()" has to be called again (outside playmode) in this case.
        //-> The whole "DrawXXL_LinesManager" mechanic is for edit mode, so this shouldn't be a problem: It doesn't make much sense to create it during playmode
        //-> The exception from this is "pause mode" (which is actually "during playmode", but in respect to line drawing it behaves like edit mode). A user can create a counter component during pause mode, and it will not be there anymore when he leaves playmode.
        //-> The irritation there is probably not big, because when the user knows how to draw in pause mode, then he knows about "OnDrawGizmos()" and therefore is easily able to create the counter component once more outside playmode.
        //-> As an alternative the settings file (which is persistent during playmode exit) could hold a flag to indicate that the counter component should automatically be created after exit playmode, if the counter component has been created by the user during playmode, but then probably more confusing setting options could be needed, because this approach would create the problem that deleting the counter component gets more complicated, because it would get autocreated all the time.

#if UNITY_EDITOR
        [SerializeField] bool gizmoLineCountManagerAutomaticallyRepaintsRendering = false; //this setting is declared in the component instead of as global static field in "DrawBasics", so that it is persistent on domain reloads (that means "on scipt reloads" and "on enter playmode")
#endif

        public List<Vector3> lineStartAndEndPoints_asMeshVertices;
        public List<Vector3> lineStartAndEndPoints_asMeshVertices_overlay;
        public List<Vector3> lineStartAndEndPoints_asMeshVertices_delayed_version1; //"_version1" and "_version2" exists to prevent high numbers of "list.Insert()" and "list.RemoveAt()", which are expensive for lists.
        public List<Vector3> lineStartAndEndPoints_asMeshVertices_delayed_version2;
        public List<Color> colors_perMeshVertex;
        public List<Color> colors_perMeshVertex_overlay;
        public List<Color> colors_perMeshVertex_delayed_version1;
        public List<Color> colors_perMeshVertex_delayed_version2;
        public List<float> time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_version1;
        public List<float> time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_version2;
        public int activeDurationCacheVersion;
        public List<int> indicesOfTheMeshVertices_thatBuildUpTheSingleLinesOfTheMesh;
        Mesh meshThatContainsTheDrawnLines;
        MeshRenderer meshRenderer;
        Material linesMaterial_hideable;
        Material linesMaterial_overlay;

        public static DrawXXL_LinesManager instance;
        public static void TryCreate()
        {
            if (instance == null)
            {
                TryFindExistingNonReferencedInstance();
                if (instance == null)
                {
                    GameObject hosting_gameObject = new GameObject("Draw XXL Lines Manager");
                    instance = hosting_gameObject.AddComponent<DrawXXL_LinesManager>();
                }

                if (Application.isPlaying)
                {
                    RecreateMesh();
                    RecreateCacheListsForMesh();
                }
            }
            else
            {
                if (Application.isPlaying)
                {
                    if (instance.meshThatContainsTheDrawnLines == null) { RecreateMesh(); }
                    if (instance.lineStartAndEndPoints_asMeshVertices == null) { RecreateCacheListsForMesh(); }
                }
            }
        }

        static void RecreateMesh()
        {
            //-> ".instance" is never null here
            //-> "isPlaymode" is always true here
            //-> "DrawBasics.usedUnityLineDrawingMethod == disabled" never arrives here

            instance.meshThatContainsTheDrawnLines = new Mesh();
            instance.meshThatContainsTheDrawnLines.name = "Draw XXL lines";
            instance.meshThatContainsTheDrawnLines.MarkDynamic();

            MeshFilter meshFilter = instance.gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = instance.meshThatContainsTheDrawnLines;

            instance.meshRenderer = instance.gameObject.AddComponent<MeshRenderer>();
            instance.linesMaterial_hideable = new Material(Resources.Load("DrawXXL_lines") as Shader);
            //instance.linesMaterial_overlay = new Material(Resources.Load("DrawXXL_lines_overlay") as Shader); //the current overlay shader for builds may work well in many situations, but there are setups (Render Pipeline? Image Effects?) where it produces unforseen effects.
            instance.linesMaterial_overlay = instance.linesMaterial_hideable; //<-to activate an overlay shader for meshes: comment this line out, and comment the preceding line in. But then still lines that have a "durationInSeconds" bigger than 0 may get assigned to the wrong hideable-vs-overlay-chunk.
            instance.meshRenderer.material = instance.linesMaterial_hideable;
            instance.meshRenderer.receiveShadows = false;
            instance.meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
            instance.meshRenderer.lightProbeUsage = LightProbeUsage.Off;
        }

        static void RecreateCacheListsForMesh()
        {
            //-> ".instance" is never null here
            //-> "isPlaymode" is always true here
            //-> "DrawBasics.usedUnityLineDrawingMethod == disabled" never arrives here

            instance.lineStartAndEndPoints_asMeshVertices = new List<Vector3>();
            instance.colors_perMeshVertex = new List<Color>();
            instance.indicesOfTheMeshVertices_thatBuildUpTheSingleLinesOfTheMesh = new List<int>();
            instance.CreateReusableIndexList();
            instance.lineStartAndEndPoints_asMeshVertices_overlay = new List<Vector3>();
            instance.colors_perMeshVertex_overlay = new List<Color>();
            instance.lineStartAndEndPoints_asMeshVertices_delayed_version1 = new List<Vector3>();
            instance.lineStartAndEndPoints_asMeshVertices_delayed_version2 = new List<Vector3>();
            instance.colors_perMeshVertex_delayed_version1 = new List<Color>();
            instance.colors_perMeshVertex_delayed_version2 = new List<Color>();
            instance.time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_version1 = new List<float>();
            instance.time_perMeshVertex_whenMeshLinesDelayedDisplayEnds_version2 = new List<float>();

            instance.activeDurationCacheVersion = 1;
        }

        void CreateReusableIndexList()
        {
            for (int i = 0; i < maxNumberOfIndices_perSubMesh; i++)
            {
                indicesOfTheMeshVertices_thatBuildUpTheSingleLinesOfTheMesh.Add(i);
            }
        }

        public static bool Exists()
        {
            if (instance == null)
            {
                //-> this is expensive, but is anyway only called in error-log-situations (or other seldom situations), so not during normal operation
                //-> it is necessary because the static "instance" reference is wiped often due to Unitys save/load serialization system
                TryFindExistingNonReferencedInstance();
                return (instance != null);
            }
            else
            {
                return true;
            }
        }

        static void TryFindExistingNonReferencedInstance()
        {
            instance = UnityEngine.Object.FindObjectOfType<DrawXXL_LinesManager>();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
        static void ScriptDomainHasBeenReloaded()
        {
            if (Exists())
            {
                //Why doing this? -> see notes in "Start()"
                UtilitiesDXXL_Components.ReportOnDrawGizmosCycleOfAMonoBehaviour(instance.GetInstanceID());
            }
        }
#endif

        void Awake()
        {
#if UNITY_EDITOR
            UtilitiesDXXL_Components.ExpandComponentInInspector(this);
#endif
        }

        void Start()
        {
#if UNITY_EDITOR
            UtilitiesDXXL_Components.ExpandComponentInInspector(this);

            DXXLWrapperForUntiysBuildInDrawLines.ResetLinesPerFrameCounter(); //-> start with a defined line counter

            //Calling "ReportOnDrawGizmosCycleOfAMonoBehaviour" also on "Start()", because:
            //-> Other components that draw lines could be fired earlier than this component inside the OnDrawGizmo-cylce.
            //-> In this case the first frame after StartPlaymode is not covered by the line counting mechanic (because the virtualGizmoCycle only increments when a component "comes back", so it has to come at least twice. So this manager component could produce its first gizmo cycle not before the end of the second frame of his existence).
            //-> This can result in false positive "Max lines exceeded" error logs in the first frame.
            //-> Calling it here prevents these false positives
            //-> The call in "ScriptDomainHasBeenReloaded()" may make this here obsolete though, but it doesn't harm to call it here again
            UtilitiesDXXL_Components.ReportOnDrawGizmosCycleOfAMonoBehaviour(this.GetInstanceID());
#endif
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
#else
            if (DrawBasics.usedUnityLineDrawingMethod == DrawBasics.UsedUnityLineDrawingMethod.wireMesh)
            {
                //without this the mesh in builds will not be cleared for cases where there is a drawing pause.
                DXXLWrapperForUntiysBuildInDrawLines.TryResetLinesPerFrameCounter();
            }
#endif
        }

        void LateUpdate()
        {
            DrawSheduledScreenspaceShapes(); //should be executed BEFORE "TryUpdateMesh_fromCachedLines()", otherwise sheduled screenspace lines will not be included in the displayed wire mesh.
            TryUpdateMesh_fromCachedLines();
        }

        void OnDrawGizmos()
        {
#if UNITY_EDITOR
            UtilitiesDXXL_Components.ExpandComponentInInspector(this);
            if (gizmoLineCountManagerAutomaticallyRepaintsRendering) { UtilitiesDXXL_Components.currentVirtualOnDrawGizmoCycle_shouldRepaintAllViews = true; }
            UtilitiesDXXL_Components.ReportOnDrawGizmosCycleOfAMonoBehaviour(this.GetInstanceID());
#endif
        }

        //Concerning "maxNumberOfIndices_perSubMesh":
        //-> This seems to be wrongly formulated in the Unity documenation
        //-> It self-contradictatoringly states that "to achieve meshes that are larger than 65535 vertices while using 16 bit index buffers", but also "maximum value supported in the index buffer is 65535".
        //-> So I guess they mean this: 
        //---> maximum vertices is "65536"
        //---> highest allowed vertex index is "65535"
        //---> to be on the safe side I use "65000"
        int maxNumberOfIndices_perSubMesh = 65000;
        int maxSubMeshesOf65000IndexesEach = 30; //lines have to vertices, so the limit of "65535" means only (approximately) "33000 lines". //this setting of "maxSubMeshesOf65535IndexesEach" overdefines together with "UtilitiesDXXL_DrawBasics.maxMaxAllowedDrawnLinesPerFrame".
        int numberOfVertices_combinedFromHiddenAndOverlayMeshes;
        int i_startOfUnhiddenOverlayLines;
        List<InternalDXXL_SubMeshIdentifier> subMeshs = new List<InternalDXXL_SubMeshIdentifier>();

        void TryUpdateMesh_fromCachedLines()
        {
            if (meshThatContainsTheDrawnLines != null)
            {
                if (lineStartAndEndPoints_asMeshVertices != null)
                {
                    Combine_hideableAndOverlayLines_intoTheSameLists();

                    meshThatContainsTheDrawnLines.SetVertices(lineStartAndEndPoints_asMeshVertices); //-> combined "hidden"+"overlay" into one list
                    meshThatContainsTheDrawnLines.SetColors(colors_perMeshVertex); //-> combined "hidden"+"overlay" into one list

                    ConfigureTheSubMeshs();
                    FillTheSubMeshsToTheMesh();
                    RecreateMaterialsArrayToFitTheSubMeshes();
                }
            }
        }

        void Combine_hideableAndOverlayLines_intoTheSameLists()
        {
            meshThatContainsTheDrawnLines.Clear();

            //Adding the unhiddenOverlay-lines:
            i_startOfUnhiddenOverlayLines = lineStartAndEndPoints_asMeshVertices.Count;
            for (int i_inOverlayList = 0; i_inOverlayList < lineStartAndEndPoints_asMeshVertices_overlay.Count; i_inOverlayList++)
            {
                lineStartAndEndPoints_asMeshVertices.Add(lineStartAndEndPoints_asMeshVertices_overlay[i_inOverlayList]);
                colors_perMeshVertex.Add(colors_perMeshVertex_overlay[i_inOverlayList]);
            }

            //Very obscure bug:
            //-> only affects drawing IN BUILDS
            //-> only affects drawing TO SCREENSPACE
            //-> drawing to screenspace in builds currently doesn't work due to the following line of code.
            //-> the code execution just stops with this line, despite this line not doing anything complicated
            //-> it is no exception
            numberOfVertices_combinedFromHiddenAndOverlayMeshes = lineStartAndEndPoints_asMeshVertices.Count;
        }

        void ConfigureTheSubMeshs()
        {
            subMeshs.Clear();
            int i_startOfCurrentlyTreatedHideableSubMesh = 0;
            int combinedLengthOfAllFull65000OverlayMeshesUpUntilNow = 0;

            for (int i_subMesh = 0; i_subMesh < maxSubMeshesOf65000IndexesEach; i_subMesh++)
            {
                InternalDXXL_SubMeshIdentifier currentlyAddedSubMesh = new InternalDXXL_SubMeshIdentifier();
                if (i_startOfCurrentlyTreatedHideableSubMesh < i_startOfUnhiddenOverlayLines)
                {
                    //hideable mesh:
                    currentlyAddedSubMesh.depthTestType = InternalDXXL_SubMeshIdentifier.DepthTestType.meshIsHidableBehindOtherGeometry;
                    currentlyAddedSubMesh.i_startOfSubMesh_insideTheFinalVertsList = maxNumberOfIndices_perSubMesh * i_subMesh;

                    if ((i_startOfCurrentlyTreatedHideableSubMesh + maxNumberOfIndices_perSubMesh) < i_startOfUnhiddenOverlayLines)
                    {
                        //hideable mesh (with max possible amount (65000) of verticesPerSubMesh):
                        currentlyAddedSubMesh.lengthOfSubMesh_inVertices = maxNumberOfIndices_perSubMesh;
                    }
                    else
                    {
                        //last hideable mesh (with arbitrarily decresed number of vertices):
                        currentlyAddedSubMesh.lengthOfSubMesh_inVertices = i_startOfUnhiddenOverlayLines - i_startOfCurrentlyTreatedHideableSubMesh;
                    }
                    i_startOfCurrentlyTreatedHideableSubMesh += maxNumberOfIndices_perSubMesh;
                }
                else
                {
                    currentlyAddedSubMesh.i_startOfSubMesh_insideTheFinalVertsList = i_startOfUnhiddenOverlayLines + combinedLengthOfAllFull65000OverlayMeshesUpUntilNow;
                    if (currentlyAddedSubMesh.i_startOfSubMesh_insideTheFinalVertsList < numberOfVertices_combinedFromHiddenAndOverlayMeshes)
                    {
                        //overlaying mesh:
                        currentlyAddedSubMesh.depthTestType = InternalDXXL_SubMeshIdentifier.DepthTestType.meshAlwaysOverlaysOtherGeometry;

                        if ((currentlyAddedSubMesh.i_startOfSubMesh_insideTheFinalVertsList + maxNumberOfIndices_perSubMesh) < numberOfVertices_combinedFromHiddenAndOverlayMeshes)
                        {
                            //overlaying mesh (with max possible amount (65000) of verticesPerSubMesh):
                            currentlyAddedSubMesh.lengthOfSubMesh_inVertices = maxNumberOfIndices_perSubMesh;
                        }
                        else
                        {
                            //overlaying mesh (with arbitrarily decresed number of vertices):
                            currentlyAddedSubMesh.lengthOfSubMesh_inVertices = numberOfVertices_combinedFromHiddenAndOverlayMeshes - currentlyAddedSubMesh.i_startOfSubMesh_insideTheFinalVertsList;
                        }
                        combinedLengthOfAllFull65000OverlayMeshesUpUntilNow += maxNumberOfIndices_perSubMesh;
                    }
                    else
                    {
                        break;
                    }
                }

                subMeshs.Add(currentlyAddedSubMesh);
            }
        }

        void FillTheSubMeshsToTheMesh()
        {
            MeshTopology topology = MeshTopology.Lines;
            bool calculateBounds = true;
            meshThatContainsTheDrawnLines.subMeshCount = subMeshs.Count;

            for (int i_subMesh = 0; i_subMesh < subMeshs.Count; i_subMesh++)
            {
                int indicesStart = 0;
                int indicesLength = subMeshs[i_subMesh].lengthOfSubMesh_inVertices;
                int baseVertex = subMeshs[i_subMesh].i_startOfSubMesh_insideTheFinalVertsList;

                meshThatContainsTheDrawnLines.SetIndices(indicesOfTheMeshVertices_thatBuildUpTheSingleLinesOfTheMesh, indicesStart, indicesLength, topology, i_subMesh, calculateBounds, baseVertex);
            }
        }

        void RecreateMaterialsArrayToFitTheSubMeshes()
        {
            if (meshRenderer == null)
            {
                meshRenderer = gameObject.AddComponent<MeshRenderer>();
                linesMaterial_hideable = new Material(Resources.Load("DrawXXL_lines") as Shader);
                linesMaterial_overlay = new Material(Resources.Load("DrawXXL_lines_overlay") as Shader);
            }

            Material[] materialsForAllSubMeshs = new Material[subMeshs.Count];
            for (int i_subMesh = 0; i_subMesh < subMeshs.Count; i_subMesh++)
            {
                switch (subMeshs[i_subMesh].depthTestType)
                {
                    case InternalDXXL_SubMeshIdentifier.DepthTestType.meshIsHidableBehindOtherGeometry:
                        materialsForAllSubMeshs[i_subMesh] = linesMaterial_hideable;
                        break;
                    case InternalDXXL_SubMeshIdentifier.DepthTestType.meshAlwaysOverlaysOtherGeometry:
                        materialsForAllSubMeshs[i_subMesh] = linesMaterial_overlay;
                        break;
                    default:
                        break;
                }
            }
            meshRenderer.materials = materialsForAllSubMeshs;
        }

        public bool noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem = true; //details: See notes inside the "ScreenspaceShedulingStrucs" script file
        public bool atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = false;

        public List<ParentOf_Lines_fadeableAnimSpeed_screenspace> listOfSheduled_ParentOf_Lines_fadeableAnimSpeed_screenspace = new List<ParentOf_Lines_fadeableAnimSpeed_screenspace>();

        public List<TextScreenspace_3Dpos_dirViaVec_cam> listOfSheduled_TextScreenspace_3Dpos_dirViaVec_cam = new List<TextScreenspace_3Dpos_dirViaVec_cam>();
        public List<TextScreenspace_2Dpos_dirViaVec_cam> listOfSheduled_TextScreenspace_2Dpos_dirViaVec_cam = new List<TextScreenspace_2Dpos_dirViaVec_cam>();
        public List<TextScreenspaceFramed_3Dpos_dirViaVec_cam> listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam = new List<TextScreenspaceFramed_3Dpos_dirViaVec_cam>();
        public List<TextScreenspaceFramed_2Dpos_dirViaVec_cam> listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam = new List<TextScreenspaceFramed_2Dpos_dirViaVec_cam>();
        public List<TextScreenspace_3Dpos_dirViaAngle_cam> listOfSheduled_TextScreenspace_3Dpos_dirViaAngle_cam = new List<TextScreenspace_3Dpos_dirViaAngle_cam>();
        public List<TextScreenspace_2Dpos_dirViaAngle_cam> listOfSheduled_TextScreenspace_2Dpos_dirViaAngle_cam = new List<TextScreenspace_2Dpos_dirViaAngle_cam>();
        public List<TextScreenspaceFramed_3Dpos_dirViaAngle_cam> listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam = new List<TextScreenspaceFramed_3Dpos_dirViaAngle_cam>();
        public List<TextScreenspaceFramed_2Dpos_dirViaAngle_cam> listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam = new List<TextScreenspaceFramed_2Dpos_dirViaAngle_cam>();

        public List<TextOnCircleScreenspace_viaStartPos_cam> listOfSheduled_TextOnCircleScreenspace_viaStartPos_cam = new List<TextOnCircleScreenspace_viaStartPos_cam>();
        public List<TextOnCircleScreenspace_dirViaVecUp_cam> listOfSheduled_TextOnCircleScreenspace_dirViaVecUp_cam = new List<TextOnCircleScreenspace_dirViaVecUp_cam>();
        public List<TextOnCircleScreenspace_dirViaAngle_cam> listOfSheduled_TextOnCircleScreenspace_dirViaAngle_cam = new List<TextOnCircleScreenspace_dirViaAngle_cam>();

        public List<ArrayOfBool_screenspace_3Dpos> listOfSheduled_ArrayOfBool_screenspace_3Dpos = new List<ArrayOfBool_screenspace_3Dpos>();
        public List<ArrayOfBool_screenspace_3Dpos_cam> listOfSheduled_ArrayOfBool_screenspace_3Dpos_cam = new List<ArrayOfBool_screenspace_3Dpos_cam>();
        public List<ArrayOfBool_screenspace_2Dpos> listOfSheduled_ArrayOfBool_screenspace_2Dpos = new List<ArrayOfBool_screenspace_2Dpos>();
        public List<ArrayOfBool_screenspace_2Dpos_cam> listOfSheduled_ArrayOfBool_screenspace_2Dpos_cam = new List<ArrayOfBool_screenspace_2Dpos_cam>();
        public List<ListOfBool_screenspace_3Dpos> listOfSheduled_ListOfBool_screenspace_3Dpos = new List<ListOfBool_screenspace_3Dpos>();
        public List<ListOfBool_screenspace_3Dpos_cam> listOfSheduled_ListOfBool_screenspace_3Dpos_cam = new List<ListOfBool_screenspace_3Dpos_cam>();
        public List<ListOfBool_screenspace_2Dpos> listOfSheduled_ListOfBool_screenspace_2Dpos = new List<ListOfBool_screenspace_2Dpos>();
        public List<ListOfBool_screenspace_2Dpos_cam> listOfSheduled_ListOfBool_screenspace_2Dpos_cam = new List<ListOfBool_screenspace_2Dpos_cam>();

        public List<ArrayOfInt_screenspace_3Dpos> listOfSheduled_ArrayOfInt_screenspace_3Dpos = new List<ArrayOfInt_screenspace_3Dpos>();
        public List<ArrayOfInt_screenspace_3Dpos_cam> listOfSheduled_ArrayOfInt_screenspace_3Dpos_cam = new List<ArrayOfInt_screenspace_3Dpos_cam>();
        public List<ArrayOfInt_screenspace_2Dpos> listOfSheduled_ArrayOfInt_screenspace_2Dpos = new List<ArrayOfInt_screenspace_2Dpos>();
        public List<ArrayOfInt_screenspace_2Dpos_cam> listOfSheduled_ArrayOfInt_screenspace_2Dpos_cam = new List<ArrayOfInt_screenspace_2Dpos_cam>();
        public List<ListOfInt_screenspace_3Dpos> listOfSheduled_ListOfInt_screenspace_3Dpos = new List<ListOfInt_screenspace_3Dpos>();
        public List<ListOfInt_screenspace_3Dpos_cam> listOfSheduled_ListOfInt_screenspace_3Dpos_cam = new List<ListOfInt_screenspace_3Dpos_cam>();
        public List<ListOfInt_screenspace_2Dpos> listOfSheduled_ListOfInt_screenspace_2Dpos = new List<ListOfInt_screenspace_2Dpos>();
        public List<ListOfInt_screenspace_2Dpos_cam> listOfSheduled_ListOfInt_screenspace_2Dpos_cam = new List<ListOfInt_screenspace_2Dpos_cam>();

        public List<ArrayOfFloat_screenspace_3Dpos> listOfSheduled_ArrayOfFloat_screenspace_3Dpos = new List<ArrayOfFloat_screenspace_3Dpos>();
        public List<ArrayOfFloat_screenspace_3Dpos_cam> listOfSheduled_ArrayOfFloat_screenspace_3Dpos_cam = new List<ArrayOfFloat_screenspace_3Dpos_cam>();
        public List<ArrayOfFloat_screenspace_2Dpos> listOfSheduled_ArrayOfFloat_screenspace_2Dpos = new List<ArrayOfFloat_screenspace_2Dpos>();
        public List<ArrayOfFloat_screenspace_2Dpos_cam> listOfSheduled_ArrayOfFloat_screenspace_2Dpos_cam = new List<ArrayOfFloat_screenspace_2Dpos_cam>();
        public List<ListOfFloat_screenspace_3Dpos> listOfSheduled_ListOfFloat_screenspace_3Dpos = new List<ListOfFloat_screenspace_3Dpos>();
        public List<ListOfFloat_screenspace_3Dpos_cam> listOfSheduled_ListOfFloat_screenspace_3Dpos_cam = new List<ListOfFloat_screenspace_3Dpos_cam>();
        public List<ListOfFloat_screenspace_2Dpos> listOfSheduled_ListOfFloat_screenspace_2Dpos = new List<ListOfFloat_screenspace_2Dpos>();
        public List<ListOfFloat_screenspace_2Dpos_cam> listOfSheduled_ListOfFloat_screenspace_2Dpos_cam = new List<ListOfFloat_screenspace_2Dpos_cam>();

        public List<ArrayOfString_screenspace_3Dpos> listOfSheduled_ArrayOfString_screenspace_3Dpos = new List<ArrayOfString_screenspace_3Dpos>();
        public List<ArrayOfString_screenspace_3Dpos_cam> listOfSheduled_ArrayOfString_screenspace_3Dpos_cam = new List<ArrayOfString_screenspace_3Dpos_cam>();
        public List<ArrayOfString_screenspace_2Dpos> listOfSheduled_ArrayOfString_screenspace_2Dpos = new List<ArrayOfString_screenspace_2Dpos>();
        public List<ArrayOfString_screenspace_2Dpos_cam> listOfSheduled_ArrayOfString_screenspace_2Dpos_cam = new List<ArrayOfString_screenspace_2Dpos_cam>();
        public List<ListOfString_screenspace_3Dpos> listOfSheduled_ListOfString_screenspace_3Dpos = new List<ListOfString_screenspace_3Dpos>();
        public List<ListOfString_screenspace_3Dpos_cam> listOfSheduled_ListOfString_screenspace_3Dpos_cam = new List<ListOfString_screenspace_3Dpos_cam>();
        public List<ListOfString_screenspace_2Dpos> listOfSheduled_ListOfString_screenspace_2Dpos = new List<ListOfString_screenspace_2Dpos>();
        public List<ListOfString_screenspace_2Dpos_cam> listOfSheduled_ListOfString_screenspace_2Dpos_cam = new List<ListOfString_screenspace_2Dpos_cam>();

        public List<ArrayOfVector2_screenspace_3Dpos> listOfSheduled_ArrayOfVector2_screenspace_3Dpos = new List<ArrayOfVector2_screenspace_3Dpos>();
        public List<ArrayOfVector2_screenspace_3Dpos_cam> listOfSheduled_ArrayOfVector2_screenspace_3Dpos_cam = new List<ArrayOfVector2_screenspace_3Dpos_cam>();
        public List<ArrayOfVector2_screenspace_2Dpos> listOfSheduled_ArrayOfVector2_screenspace_2Dpos = new List<ArrayOfVector2_screenspace_2Dpos>();
        public List<ArrayOfVector2_screenspace_2Dpos_cam> listOfSheduled_ArrayOfVector2_screenspace_2Dpos_cam = new List<ArrayOfVector2_screenspace_2Dpos_cam>();
        public List<ListOfVector2_screenspace_3Dpos> listOfSheduled_ListOfVector2_screenspace_3Dpos = new List<ListOfVector2_screenspace_3Dpos>();
        public List<ListOfVector2_screenspace_3Dpos_cam> listOfSheduled_ListOfVector2_screenspace_3Dpos_cam = new List<ListOfVector2_screenspace_3Dpos_cam>();
        public List<ListOfVector2_screenspace_2Dpos> listOfSheduled_ListOfVector2_screenspace_2Dpos = new List<ListOfVector2_screenspace_2Dpos>();
        public List<ListOfVector2_screenspace_2Dpos_cam> listOfSheduled_ListOfVector2_screenspace_2Dpos_cam = new List<ListOfVector2_screenspace_2Dpos_cam>();

        public List<ArrayOfVector3_screenspace_3Dpos> listOfSheduled_ArrayOfVector3_screenspace_3Dpos = new List<ArrayOfVector3_screenspace_3Dpos>();
        public List<ArrayOfVector3_screenspace_3Dpos_cam> listOfSheduled_ArrayOfVector3_screenspace_3Dpos_cam = new List<ArrayOfVector3_screenspace_3Dpos_cam>();
        public List<ArrayOfVector3_screenspace_2Dpos> listOfSheduled_ArrayOfVector3_screenspace_2Dpos = new List<ArrayOfVector3_screenspace_2Dpos>();
        public List<ArrayOfVector3_screenspace_2Dpos_cam> listOfSheduled_ArrayOfVector3_screenspace_2Dpos_cam = new List<ArrayOfVector3_screenspace_2Dpos_cam>();
        public List<ListOfVector3_screenspace_3Dpos> listOfSheduled_ListOfVector3_screenspace_3Dpos = new List<ListOfVector3_screenspace_3Dpos>();
        public List<ListOfVector3_screenspace_3Dpos_cam> listOfSheduled_ListOfVector3_screenspace_3Dpos_cam = new List<ListOfVector3_screenspace_3Dpos_cam>();
        public List<ListOfVector3_screenspace_2Dpos> listOfSheduled_ListOfVector3_screenspace_2Dpos = new List<ListOfVector3_screenspace_2Dpos>();
        public List<ListOfVector3_screenspace_2Dpos_cam> listOfSheduled_ListOfVector3_screenspace_2Dpos_cam = new List<ListOfVector3_screenspace_2Dpos_cam>();

        public List<ArrayOfVector4_screenspace_3Dpos> listOfSheduled_ArrayOfVector4_screenspace_3Dpos = new List<ArrayOfVector4_screenspace_3Dpos>();
        public List<ArrayOfVector4_screenspace_3Dpos_cam> listOfSheduled_ArrayOfVector4_screenspace_3Dpos_cam = new List<ArrayOfVector4_screenspace_3Dpos_cam>();
        public List<ArrayOfVector4_screenspace_2Dpos> listOfSheduled_ArrayOfVector4_screenspace_2Dpos = new List<ArrayOfVector4_screenspace_2Dpos>();
        public List<ArrayOfVector4_screenspace_2Dpos_cam> listOfSheduled_ArrayOfVector4_screenspace_2Dpos_cam = new List<ArrayOfVector4_screenspace_2Dpos_cam>();
        public List<ListOfVector4_screenspace_3Dpos> listOfSheduled_ListOfVector4_screenspace_3Dpos = new List<ListOfVector4_screenspace_3Dpos>();
        public List<ListOfVector4_screenspace_3Dpos_cam> listOfSheduled_ListOfVector4_screenspace_3Dpos_cam = new List<ListOfVector4_screenspace_3Dpos_cam>();
        public List<ListOfVector4_screenspace_2Dpos> listOfSheduled_ListOfVector4_screenspace_2Dpos = new List<ListOfVector4_screenspace_2Dpos>();
        public List<ListOfVector4_screenspace_2Dpos_cam> listOfSheduled_ListOfVector4_screenspace_2Dpos_cam = new List<ListOfVector4_screenspace_2Dpos_cam>();

        public List<TagGameObjectScreenspace> listOfSheduled_TagGameObjectScreenspace = new List<TagGameObjectScreenspace>();
        public List<GridScreenspace> listOfSheduled_GridScreenspace = new List<GridScreenspace>();
        public List<BoolDisplayerScreenspace_3Dpos> listOfSheduled_BoolDisplayerScreenspace_3Dpos = new List<BoolDisplayerScreenspace_3Dpos>();
        public List<BoolDisplayerScreenspace_3Dpos_cam> listOfSheduled_BoolDisplayerScreenspace_3Dpos_cam = new List<BoolDisplayerScreenspace_3Dpos_cam>();
        public List<BoolDisplayerScreenspace_2Dpos_cam> listOfSheduled_BoolDisplayerScreenspace_2Dpos_cam = new List<BoolDisplayerScreenspace_2Dpos_cam>();

        public List<LogsOnScreen> listOfSheduled_LogsOnScreen = new List<LogsOnScreen>();

        public List<ScreenspaceLine> listOfSheduled_ScreenspaceLine = new List<ScreenspaceLine>();
        public List<ScreenspaceRay> listOfSheduled_ScreenspaceRay = new List<ScreenspaceRay>();
        public List<ScreenspaceLineFrom> listOfSheduled_ScreenspaceLineFrom = new List<ScreenspaceLineFrom>();
        public List<ScreenspaceLineTo> listOfSheduled_ScreenspaceLineTo = new List<ScreenspaceLineTo>();
        public List<ScreenspaceLineColorFade> listOfSheduled_ScreenspaceLineColorFade = new List<ScreenspaceLineColorFade>();
        public List<ScreenspaceRayColorFade> listOfSheduled_ScreenspaceRayColorFade = new List<ScreenspaceRayColorFade>();
        public List<ScreenspaceLineFrom_withColorFade> listOfSheduled_ScreenspaceLineFrom_withColorFade = new List<ScreenspaceLineFrom_withColorFade>();
        public List<ScreenspaceLineTo_withColorFade> listOfSheduled_ScreenspaceLineTo_withColorFade = new List<ScreenspaceLineTo_withColorFade>();
        public List<ScreenspaceLineCircled_angleToAngle_cam> listOfSheduled_ScreenspaceLineCircled_angleToAngle_cam = new List<ScreenspaceLineCircled_angleToAngle_cam>();
        public List<ScreenspaceLineCircled_angleFromStartPos_cam> listOfSheduled_ScreenspaceLineCircled_angleFromStartPos_cam = new List<ScreenspaceLineCircled_angleFromStartPos_cam>();
        public List<ScreenspaceCircleSegment_angleToAngle_cam> listOfSheduled_ScreenspaceCircleSegment_angleToAngle_cam = new List<ScreenspaceCircleSegment_angleToAngle_cam>();
        public List<ScreenspaceCircleSegment_angleFromStartPos_cam> listOfSheduled_ScreenspaceCircleSegment_angleFromStartPos_cam = new List<ScreenspaceCircleSegment_angleFromStartPos_cam>();
        public List<ScreenspaceLineString_array_cam> listOfSheduled_ScreenspaceLineString_array_cam = new List<ScreenspaceLineString_array_cam>();
        public List<ScreenspaceLineString_list_cam> listOfSheduled_ScreenspaceLineString_list_cam = new List<ScreenspaceLineString_list_cam>();
        public List<ScreenspaceLineStringColorFade_array_cam> listOfSheduled_ScreenspaceLineStringColorFade_array_cam = new List<ScreenspaceLineStringColorFade_array_cam>();
        public List<ScreenspaceLineStringColorFade_list_cam> listOfSheduled_ScreenspaceLineStringColorFade_list_cam = new List<ScreenspaceLineStringColorFade_list_cam>();
        public List<ScreenspaceShape_3Dpos> listOfSheduled_ScreenspaceShape_3Dpos = new List<ScreenspaceShape_3Dpos>();
        public List<ScreenspaceShape_3Dpos_cam> listOfSheduled_ScreenspaceShape_3Dpos_cam = new List<ScreenspaceShape_3Dpos_cam>();
        public List<ScreenspaceShape_2Dpos_cam> listOfSheduled_ScreenspaceShape_2Dpos_cam = new List<ScreenspaceShape_2Dpos_cam>();
        public List<ScreenspaceRectangle> listOfSheduled_ScreenspaceRectangle = new List<ScreenspaceRectangle>();
        public List<ScreenspaceBox_rect_cam> listOfSheduled_ScreenspaceBox_rect_cam = new List<ScreenspaceBox_rect_cam>();
        public List<ScreenspaceBox_3Dpos_vec> listOfSheduled_ScreenspaceBox_3Dpos_vec = new List<ScreenspaceBox_3Dpos_vec>();
        public List<ScreenspaceBox_3Dpos_vec_cam> listOfSheduled_ScreenspaceBox_3Dpos_vec_cam = new List<ScreenspaceBox_3Dpos_vec_cam>();
        public List<ScreenspaceBox_2Dpos_vec_cam> listOfSheduled_ScreenspaceBox_2Dpos_vec_cam = new List<ScreenspaceBox_2Dpos_vec_cam>();
        public List<ScreenspaceCircle_rect_cam> listOfSheduled_ScreenspaceCircle_rect_cam = new List<ScreenspaceCircle_rect_cam>();
        public List<ScreenspaceCircle_3Dpos_vecRad> listOfSheduled_ScreenspaceCircle_3Dpos_vecRad = new List<ScreenspaceCircle_3Dpos_vecRad>();
        public List<ScreenspaceCircle_3Dpos_vecRad_cam> listOfSheduled_ScreenspaceCircle_3Dpos_vecRad_cam = new List<ScreenspaceCircle_3Dpos_vecRad_cam>();
        public List<ScreenspaceCircle_2Dpos_vecRad_cam> listOfSheduled_ScreenspaceCircle_2Dpos_vecRad_cam = new List<ScreenspaceCircle_2Dpos_vecRad_cam>();
        public List<ScreenspaceCapsule_3Dpos_vecC1C2Pos> listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos = new List<ScreenspaceCapsule_3Dpos_vecC1C2Pos>();
        public List<ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam> listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam = new List<ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam>();
        public List<ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam> listOfSheduled_ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam = new List<ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam>();
        public List<ScreenspaceCapsule_rect_cam> listOfSheduled_ScreenspaceCapsule_rect_cam = new List<ScreenspaceCapsule_rect_cam>();
        public List<ScreenspaceCapsule_3Dpos_vecPosSize> listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize = new List<ScreenspaceCapsule_3Dpos_vecPosSize>();
        public List<ScreenspaceCapsule_3Dpos_vecPosSize_cam> listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam = new List<ScreenspaceCapsule_3Dpos_vecPosSize_cam>();
        public List<ScreenspaceCapsule_2Dpos_vecPosSize_cam> listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam = new List<ScreenspaceCapsule_2Dpos_vecPosSize_cam>();
        public List<ScreenspacePointArray> listOfSheduled_ScreenspacePointArray = new List<ScreenspacePointArray>();
        public List<ScreenspacePointList> listOfSheduled_ScreenspacePointList = new List<ScreenspacePointList>();
        public List<ScreenspacePoint> listOfSheduled_ScreenspacePoint = new List<ScreenspacePoint>();
        public List<ScreenspacePoint_prioText_cam> listOfSheduled_ScreenspacePoint_prioText_cam = new List<ScreenspacePoint_prioText_cam>();
        public List<ScreenspacePointTag_3Dpos> listOfSheduled_ScreenspacePointTag_3Dpos = new List<ScreenspacePointTag_3Dpos>();
        public List<ScreenspacePointTag_3Dpos_cam> listOfSheduled_ScreenspacePointTag_3Dpos_cam = new List<ScreenspacePointTag_3Dpos_cam>();
        public List<ScreenspacePointTag_2Dpos_cam> listOfSheduled_ScreenspacePointTag_2Dpos_cam = new List<ScreenspacePointTag_2Dpos_cam>();
        public List<ScreenspaceVectorFrom> listOfSheduled_ScreenspaceVectorFrom = new List<ScreenspaceVectorFrom>();
        public List<ScreenspaceVectorTo> listOfSheduled_ScreenspaceVectorTo = new List<ScreenspaceVectorTo>();
        public List<ScreenspaceVectorCircled_angleToAngle_cam> listOfSheduled_ScreenspaceVectorCircled_angleToAngle_cam = new List<ScreenspaceVectorCircled_angleToAngle_cam>();
        public List<ScreenspaceVectorCircled_angleFromStartPos_cam> listOfSheduled_ScreenspaceVectorCircled_angleFromStartPos_cam = new List<ScreenspaceVectorCircled_angleFromStartPos_cam>();
        public List<ScreenspaceIcon_3Dpos> listOfSheduled_ScreenspaceIcon_3Dpos = new List<ScreenspaceIcon_3Dpos>();
        public List<ScreenspaceIcon_3Dpos_cam> listOfSheduled_ScreenspaceIcon_3Dpos_cam = new List<ScreenspaceIcon_3Dpos_cam>();
        public List<ScreenspaceIcon_2Dpos_cam> listOfSheduled_ScreenspaceIcon_2Dpos_cam = new List<ScreenspaceIcon_2Dpos_cam>();
        public List<ScreenspaceDot_3Dpos> listOfSheduled_ScreenspaceDot_3Dpos = new List<ScreenspaceDot_3Dpos>();
        public List<ScreenspaceDot_3Dpos_cam> listOfSheduled_ScreenspaceDot_3Dpos_cam = new List<ScreenspaceDot_3Dpos_cam>();
        public List<ScreenspaceDot_2Dpos_cam> listOfSheduled_ScreenspaceDot_2Dpos_cam = new List<ScreenspaceDot_2Dpos_cam>();
        public List<ScreenspaceMovingArrowsRay> listOfSheduled_ScreenspaceMovingArrowsRay = new List<ScreenspaceMovingArrowsRay>();
        public List<ScreenspaceMovingArrowsLine> listOfSheduled_ScreenspaceMovingArrowsLine = new List<ScreenspaceMovingArrowsLine>();
        public List<ScreenspaceRayWithAlternatingColors> listOfSheduled_ScreenspaceRayWithAlternatingColors = new List<ScreenspaceRayWithAlternatingColors>();
        public List<ScreenspaceLineWithAlternatingColors> listOfSheduled_ScreenspaceLineWithAlternatingColors = new List<ScreenspaceLineWithAlternatingColors>();
        public List<ScreenspaceBlinkingRay> listOfSheduled_ScreenspaceBlinkingRay = new List<ScreenspaceBlinkingRay>();
        public List<ScreenspaceBlinkingLine> listOfSheduled_ScreenspaceBlinkingLine = new List<ScreenspaceBlinkingLine>();
        public List<ScreenspaceRayUnderTension> listOfSheduled_ScreenspaceRayUnderTension = new List<ScreenspaceRayUnderTension>();
        public List<ScreenspaceLineUnderTension> listOfSheduled_ScreenspaceLineUnderTension = new List<ScreenspaceLineUnderTension>();
        public List<ScreenspaceVisualizeAutomaticCameraForDrawing> listOfSheduled_ScreenspaceVisualizeAutomaticCameraForDrawing = new List<ScreenspaceVisualizeAutomaticCameraForDrawing>();

        public List<DrawScreenspaceChart> listOfSheduled_DrawScreenspaceChart = new List<DrawScreenspaceChart>();
        public List<DrawScreenspacePieChart> listOfSheduled_DrawScreenspacePieChart = new List<DrawScreenspacePieChart>();

        void DrawSheduledScreenspaceShapes()
        {
            noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem = false;
            try
            {
                for (int i = 0; i < listOfSheduled_ParentOf_Lines_fadeableAnimSpeed_screenspace.Count; i++)
                {
                    listOfSheduled_ParentOf_Lines_fadeableAnimSpeed_screenspace[i].Draw();
                }
                listOfSheduled_ParentOf_Lines_fadeableAnimSpeed_screenspace.Clear();

                if (atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate)
                {
                    for (int i = 0; i < listOfSheduled_TextScreenspace_3Dpos_dirViaVec_cam.Count; i++)
                    {
                        DrawText.WriteScreenspace(listOfSheduled_TextScreenspace_3Dpos_dirViaVec_cam[i].screenCamera, listOfSheduled_TextScreenspace_3Dpos_dirViaVec_cam[i].text, listOfSheduled_TextScreenspace_3Dpos_dirViaVec_cam[i].position_in3DWorldspace, listOfSheduled_TextScreenspace_3Dpos_dirViaVec_cam[i].color, listOfSheduled_TextScreenspace_3Dpos_dirViaVec_cam[i].size_relToViewportHeight, listOfSheduled_TextScreenspace_3Dpos_dirViaVec_cam[i].textDirection, listOfSheduled_TextScreenspace_3Dpos_dirViaVec_cam[i].textAnchor, listOfSheduled_TextScreenspace_3Dpos_dirViaVec_cam[i].forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, listOfSheduled_TextScreenspace_3Dpos_dirViaVec_cam[i].forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, listOfSheduled_TextScreenspace_3Dpos_dirViaVec_cam[i].autoLineBreakAtViewportBorder, listOfSheduled_TextScreenspace_3Dpos_dirViaVec_cam[i].autoLineBreakWidth_relToViewportWidth, listOfSheduled_TextScreenspace_3Dpos_dirViaVec_cam[i].autoFlipTextToPreventUpsideDown, listOfSheduled_TextScreenspace_3Dpos_dirViaVec_cam[i].durationInSec);
                    }
                    listOfSheduled_TextScreenspace_3Dpos_dirViaVec_cam.Clear();

                    for (int i = 0; i < listOfSheduled_TextScreenspace_2Dpos_dirViaVec_cam.Count; i++)
                    {
                        DrawText.WriteScreenspace(listOfSheduled_TextScreenspace_2Dpos_dirViaVec_cam[i].screenCamera, listOfSheduled_TextScreenspace_2Dpos_dirViaVec_cam[i].text, listOfSheduled_TextScreenspace_2Dpos_dirViaVec_cam[i].position_in2DViewportSpace, listOfSheduled_TextScreenspace_2Dpos_dirViaVec_cam[i].color, listOfSheduled_TextScreenspace_2Dpos_dirViaVec_cam[i].size_relToViewportHeight, listOfSheduled_TextScreenspace_2Dpos_dirViaVec_cam[i].textDirection, listOfSheduled_TextScreenspace_2Dpos_dirViaVec_cam[i].textAnchor, listOfSheduled_TextScreenspace_2Dpos_dirViaVec_cam[i].forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, listOfSheduled_TextScreenspace_2Dpos_dirViaVec_cam[i].forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, listOfSheduled_TextScreenspace_2Dpos_dirViaVec_cam[i].autoLineBreakAtViewportBorder, listOfSheduled_TextScreenspace_2Dpos_dirViaVec_cam[i].autoLineBreakWidth_relToViewportWidth, listOfSheduled_TextScreenspace_2Dpos_dirViaVec_cam[i].autoFlipTextToPreventUpsideDown, listOfSheduled_TextScreenspace_2Dpos_dirViaVec_cam[i].durationInSec);
                    }
                    listOfSheduled_TextScreenspace_2Dpos_dirViaVec_cam.Clear();

                    for (int i = 0; i < listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam.Count; i++)
                    {
                        DrawText.WriteScreenspaceFramed(listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam[i].screenCamera, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam[i].text, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam[i].position_in3DWorldspace, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam[i].color, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam[i].size_relToViewportHeight, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam[i].textDirection, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam[i].textAnchor, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam[i].enclosingBoxLineStyle, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam[i].enclosingBox_lineWidth_relToTextSize, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam[i].enclosingBox_paddingSize_relToTextSize, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam[i].forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam[i].forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam[i].autoLineBreakAtViewportBorder, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam[i].autoLineBreakWidth_relToViewportWidth, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam[i].autoFlipTextToPreventUpsideDown, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam[i].durationInSec);
                    }
                    listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaVec_cam.Clear();

                    for (int i = 0; i < listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam.Count; i++)
                    {
                        DrawText.WriteScreenspaceFramed(listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam[i].screenCamera, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam[i].text, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam[i].position_in2DViewportSpace, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam[i].color, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam[i].size_relToViewportHeight, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam[i].textDirection, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam[i].textAnchor, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam[i].enclosingBoxLineStyle, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam[i].enclosingBox_lineWidth_relToTextSize, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam[i].enclosingBox_paddingSize_relToTextSize, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam[i].forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam[i].forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam[i].autoLineBreakAtViewportBorder, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam[i].autoLineBreakWidth_relToViewportWidth, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam[i].autoFlipTextToPreventUpsideDown, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam[i].durationInSec);
                    }
                    listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaVec_cam.Clear();

                    for (int i = 0; i < listOfSheduled_TextScreenspace_3Dpos_dirViaAngle_cam.Count; i++)
                    {
                        DrawText.WriteScreenspace(listOfSheduled_TextScreenspace_3Dpos_dirViaAngle_cam[i].screenCamera, listOfSheduled_TextScreenspace_3Dpos_dirViaAngle_cam[i].text, listOfSheduled_TextScreenspace_3Dpos_dirViaAngle_cam[i].position_in3DWorldspace, listOfSheduled_TextScreenspace_3Dpos_dirViaAngle_cam[i].color, listOfSheduled_TextScreenspace_3Dpos_dirViaAngle_cam[i].size_relToViewportHeight, listOfSheduled_TextScreenspace_3Dpos_dirViaAngle_cam[i].zRotationDegCC, listOfSheduled_TextScreenspace_3Dpos_dirViaAngle_cam[i].textAnchor, listOfSheduled_TextScreenspace_3Dpos_dirViaAngle_cam[i].forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, listOfSheduled_TextScreenspace_3Dpos_dirViaAngle_cam[i].forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, listOfSheduled_TextScreenspace_3Dpos_dirViaAngle_cam[i].autoLineBreakAtViewportBorder, listOfSheduled_TextScreenspace_3Dpos_dirViaAngle_cam[i].autoLineBreakWidth_relToViewportWidth, listOfSheduled_TextScreenspace_3Dpos_dirViaAngle_cam[i].autoFlipTextToPreventUpsideDown, listOfSheduled_TextScreenspace_3Dpos_dirViaAngle_cam[i].durationInSec);
                    }
                    listOfSheduled_TextScreenspace_3Dpos_dirViaAngle_cam.Clear();

                    for (int i = 0; i < listOfSheduled_TextScreenspace_2Dpos_dirViaAngle_cam.Count; i++)
                    {
                        DrawText.WriteScreenspace(listOfSheduled_TextScreenspace_2Dpos_dirViaAngle_cam[i].screenCamera, listOfSheduled_TextScreenspace_2Dpos_dirViaAngle_cam[i].text, listOfSheduled_TextScreenspace_2Dpos_dirViaAngle_cam[i].position_in2DViewportSpace, listOfSheduled_TextScreenspace_2Dpos_dirViaAngle_cam[i].color, listOfSheduled_TextScreenspace_2Dpos_dirViaAngle_cam[i].size_relToViewportHeight, listOfSheduled_TextScreenspace_2Dpos_dirViaAngle_cam[i].zRotationDegCC, listOfSheduled_TextScreenspace_2Dpos_dirViaAngle_cam[i].textAnchor, listOfSheduled_TextScreenspace_2Dpos_dirViaAngle_cam[i].forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, listOfSheduled_TextScreenspace_2Dpos_dirViaAngle_cam[i].forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, listOfSheduled_TextScreenspace_2Dpos_dirViaAngle_cam[i].autoLineBreakAtViewportBorder, listOfSheduled_TextScreenspace_2Dpos_dirViaAngle_cam[i].autoLineBreakWidth_relToViewportWidth, listOfSheduled_TextScreenspace_2Dpos_dirViaAngle_cam[i].autoFlipTextToPreventUpsideDown, listOfSheduled_TextScreenspace_2Dpos_dirViaAngle_cam[i].durationInSec);
                    }
                    listOfSheduled_TextScreenspace_2Dpos_dirViaAngle_cam.Clear();

                    for (int i = 0; i < listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam.Count; i++)
                    {
                        DrawText.WriteScreenspaceFramed(listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam[i].screenCamera, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam[i].text, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam[i].position_in3DWorldspace, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam[i].color, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam[i].size_relToViewportHeight, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam[i].zRotationDegCC, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam[i].textAnchor, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam[i].enclosingBoxLineStyle, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam[i].enclosingBox_lineWidth_relToTextSize, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam[i].enclosingBox_paddingSize_relToTextSize, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam[i].forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam[i].forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam[i].autoLineBreakAtViewportBorder, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam[i].autoLineBreakWidth_relToViewportWidth, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam[i].autoFlipTextToPreventUpsideDown, listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam[i].durationInSec);
                    }
                    listOfSheduled_TextScreenspaceFramed_3Dpos_dirViaAngle_cam.Clear();

                    for (int i = 0; i < listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam.Count; i++)
                    {
                        DrawText.WriteScreenspaceFramed(listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].screenCamera, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].text, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].position_in2DViewportSpace, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].color, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].size_relToViewportHeight, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].zRotationDegCC, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].textAnchor, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].enclosingBoxLineStyle, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].enclosingBox_lineWidth_relToTextSize, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].enclosingBox_paddingSize_relToTextSize, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].autoLineBreakAtViewportBorder, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].autoLineBreakWidth_relToViewportWidth, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].autoFlipTextToPreventUpsideDown, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].durationInSec);
                    }
                    listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam.Clear();

                    for (int i = 0; i < listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam.Count; i++)
                    {
                        DrawText.WriteScreenspaceFramed(listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].screenCamera, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].text, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].position_in2DViewportSpace, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].color, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].size_relToViewportHeight, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].zRotationDegCC, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].textAnchor, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].enclosingBoxLineStyle, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].enclosingBox_lineWidth_relToTextSize, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].enclosingBox_paddingSize_relToTextSize, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].forceTextBlockEnlargementToThisMinWidth_relToViewportWidth, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].forceRestrictTextBlockSizeToThisMaxTextWidth_relToViewportWidth, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].autoLineBreakAtViewportBorder, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].autoLineBreakWidth_relToViewportWidth, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].autoFlipTextToPreventUpsideDown, listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam[i].durationInSec);
                    }
                    listOfSheduled_TextScreenspaceFramed_2Dpos_dirViaAngle_cam.Clear();

                    for (int i = 0; i < listOfSheduled_TextOnCircleScreenspace_viaStartPos_cam.Count; i++)
                    {
                        DrawText.WriteOnCircleScreenspace(listOfSheduled_TextOnCircleScreenspace_viaStartPos_cam[i].screenCamera, listOfSheduled_TextOnCircleScreenspace_viaStartPos_cam[i].text, listOfSheduled_TextOnCircleScreenspace_viaStartPos_cam[i].textStartPos, listOfSheduled_TextOnCircleScreenspace_viaStartPos_cam[i].circleCenterPosition, listOfSheduled_TextOnCircleScreenspace_viaStartPos_cam[i].color, listOfSheduled_TextOnCircleScreenspace_viaStartPos_cam[i].size_relToViewportHeight, listOfSheduled_TextOnCircleScreenspace_viaStartPos_cam[i].textAnchor, listOfSheduled_TextOnCircleScreenspace_viaStartPos_cam[i].autoLineBreakAngleDeg, listOfSheduled_TextOnCircleScreenspace_viaStartPos_cam[i].durationInSec);
                    }
                    listOfSheduled_TextOnCircleScreenspace_viaStartPos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_TextOnCircleScreenspace_dirViaVecUp_cam.Count; i++)
                    {
                        DrawText.WriteOnCircleScreenspace(listOfSheduled_TextOnCircleScreenspace_dirViaVecUp_cam[i].screenCamera, listOfSheduled_TextOnCircleScreenspace_dirViaVecUp_cam[i].text, listOfSheduled_TextOnCircleScreenspace_dirViaVecUp_cam[i].circleCenterPosition, listOfSheduled_TextOnCircleScreenspace_dirViaVecUp_cam[i].radius_relToViewportHeight, listOfSheduled_TextOnCircleScreenspace_dirViaVecUp_cam[i].color, listOfSheduled_TextOnCircleScreenspace_dirViaVecUp_cam[i].size_relToViewportHeight, listOfSheduled_TextOnCircleScreenspace_dirViaVecUp_cam[i].textsInitialUp, listOfSheduled_TextOnCircleScreenspace_dirViaVecUp_cam[i].textAnchor, listOfSheduled_TextOnCircleScreenspace_dirViaVecUp_cam[i].autoLineBreakAngleDeg, listOfSheduled_TextOnCircleScreenspace_dirViaVecUp_cam[i].durationInSec);
                    }
                    listOfSheduled_TextOnCircleScreenspace_dirViaVecUp_cam.Clear();

                    for (int i = 0; i < listOfSheduled_TextOnCircleScreenspace_dirViaAngle_cam.Count; i++)
                    {
                        DrawText.WriteOnCircleScreenspace(listOfSheduled_TextOnCircleScreenspace_dirViaAngle_cam[i].screenCamera, listOfSheduled_TextOnCircleScreenspace_dirViaAngle_cam[i].text, listOfSheduled_TextOnCircleScreenspace_dirViaAngle_cam[i].circleCenterPosition, listOfSheduled_TextOnCircleScreenspace_dirViaAngle_cam[i].radius_relToViewportHeight, listOfSheduled_TextOnCircleScreenspace_dirViaAngle_cam[i].color, listOfSheduled_TextOnCircleScreenspace_dirViaAngle_cam[i].size_relToViewportHeight, listOfSheduled_TextOnCircleScreenspace_dirViaAngle_cam[i].initialTextDirection_as_zRotationDegCCfromCamUp, listOfSheduled_TextOnCircleScreenspace_dirViaAngle_cam[i].textAnchor, listOfSheduled_TextOnCircleScreenspace_dirViaAngle_cam[i].autoLineBreakAngleDeg, listOfSheduled_TextOnCircleScreenspace_dirViaAngle_cam[i].durationInSec);
                    }
                    listOfSheduled_TextOnCircleScreenspace_dirViaAngle_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfBool_screenspace_3Dpos.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfBool_screenspace_3Dpos[i].boolArray, listOfSheduled_ArrayOfBool_screenspace_3Dpos[i].position_in3DWorldspace, listOfSheduled_ArrayOfBool_screenspace_3Dpos[i].color, listOfSheduled_ArrayOfBool_screenspace_3Dpos[i].title, listOfSheduled_ArrayOfBool_screenspace_3Dpos[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfBool_screenspace_3Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfBool_screenspace_3Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfBool_screenspace_3Dpos[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfBool_screenspace_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfBool_screenspace_3Dpos_cam.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfBool_screenspace_3Dpos_cam[i].screenCamera, listOfSheduled_ArrayOfBool_screenspace_3Dpos_cam[i].boolArray, listOfSheduled_ArrayOfBool_screenspace_3Dpos_cam[i].position_in3DWorldspace, listOfSheduled_ArrayOfBool_screenspace_3Dpos_cam[i].color, listOfSheduled_ArrayOfBool_screenspace_3Dpos_cam[i].title, listOfSheduled_ArrayOfBool_screenspace_3Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfBool_screenspace_3Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfBool_screenspace_3Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfBool_screenspace_3Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfBool_screenspace_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfBool_screenspace_2Dpos.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfBool_screenspace_2Dpos[i].boolArray, listOfSheduled_ArrayOfBool_screenspace_2Dpos[i].position_in2DViewportSpace, listOfSheduled_ArrayOfBool_screenspace_2Dpos[i].color, listOfSheduled_ArrayOfBool_screenspace_2Dpos[i].title, listOfSheduled_ArrayOfBool_screenspace_2Dpos[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfBool_screenspace_2Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfBool_screenspace_2Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfBool_screenspace_2Dpos[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfBool_screenspace_2Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfBool_screenspace_2Dpos_cam.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfBool_screenspace_2Dpos_cam[i].screenCamera, listOfSheduled_ArrayOfBool_screenspace_2Dpos_cam[i].boolArray, listOfSheduled_ArrayOfBool_screenspace_2Dpos_cam[i].position_in2DViewportSpace, listOfSheduled_ArrayOfBool_screenspace_2Dpos_cam[i].color, listOfSheduled_ArrayOfBool_screenspace_2Dpos_cam[i].title, listOfSheduled_ArrayOfBool_screenspace_2Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfBool_screenspace_2Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfBool_screenspace_2Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfBool_screenspace_2Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfBool_screenspace_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfBool_screenspace_3Dpos.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfBool_screenspace_3Dpos[i].boolList, listOfSheduled_ListOfBool_screenspace_3Dpos[i].position_in3DWorldspace, listOfSheduled_ListOfBool_screenspace_3Dpos[i].color, listOfSheduled_ListOfBool_screenspace_3Dpos[i].title, listOfSheduled_ListOfBool_screenspace_3Dpos[i].textSize_relToViewportHeight, listOfSheduled_ListOfBool_screenspace_3Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfBool_screenspace_3Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfBool_screenspace_3Dpos[i].durationInSec);
                    }
                    listOfSheduled_ListOfBool_screenspace_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfBool_screenspace_3Dpos_cam.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfBool_screenspace_3Dpos_cam[i].screenCamera, listOfSheduled_ListOfBool_screenspace_3Dpos_cam[i].boolList, listOfSheduled_ListOfBool_screenspace_3Dpos_cam[i].position_in3DWorldspace, listOfSheduled_ListOfBool_screenspace_3Dpos_cam[i].color, listOfSheduled_ListOfBool_screenspace_3Dpos_cam[i].title, listOfSheduled_ListOfBool_screenspace_3Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ListOfBool_screenspace_3Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfBool_screenspace_3Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfBool_screenspace_3Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ListOfBool_screenspace_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfBool_screenspace_2Dpos.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfBool_screenspace_2Dpos[i].boolList, listOfSheduled_ListOfBool_screenspace_2Dpos[i].position_in2DViewportSpace, listOfSheduled_ListOfBool_screenspace_2Dpos[i].color, listOfSheduled_ListOfBool_screenspace_2Dpos[i].title, listOfSheduled_ListOfBool_screenspace_2Dpos[i].textSize_relToViewportHeight, listOfSheduled_ListOfBool_screenspace_2Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfBool_screenspace_2Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfBool_screenspace_2Dpos[i].durationInSec);
                    }
                    listOfSheduled_ListOfBool_screenspace_2Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfBool_screenspace_2Dpos_cam.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfBool_screenspace_2Dpos_cam[i].screenCamera, listOfSheduled_ListOfBool_screenspace_2Dpos_cam[i].boolList, listOfSheduled_ListOfBool_screenspace_2Dpos_cam[i].position_in2DViewportSpace, listOfSheduled_ListOfBool_screenspace_2Dpos_cam[i].color, listOfSheduled_ListOfBool_screenspace_2Dpos_cam[i].title, listOfSheduled_ListOfBool_screenspace_2Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ListOfBool_screenspace_2Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfBool_screenspace_2Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfBool_screenspace_2Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ListOfBool_screenspace_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfInt_screenspace_3Dpos.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfInt_screenspace_3Dpos[i].intArray, listOfSheduled_ArrayOfInt_screenspace_3Dpos[i].position_in3DWorldspace, listOfSheduled_ArrayOfInt_screenspace_3Dpos[i].color, listOfSheduled_ArrayOfInt_screenspace_3Dpos[i].title, listOfSheduled_ArrayOfInt_screenspace_3Dpos[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfInt_screenspace_3Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfInt_screenspace_3Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfInt_screenspace_3Dpos[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfInt_screenspace_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfInt_screenspace_3Dpos_cam.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfInt_screenspace_3Dpos_cam[i].screenCamera, listOfSheduled_ArrayOfInt_screenspace_3Dpos_cam[i].intArray, listOfSheduled_ArrayOfInt_screenspace_3Dpos_cam[i].position_in3DWorldspace, listOfSheduled_ArrayOfInt_screenspace_3Dpos_cam[i].color, listOfSheduled_ArrayOfInt_screenspace_3Dpos_cam[i].title, listOfSheduled_ArrayOfInt_screenspace_3Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfInt_screenspace_3Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfInt_screenspace_3Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfInt_screenspace_3Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfInt_screenspace_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfInt_screenspace_2Dpos.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfInt_screenspace_2Dpos[i].intArray, listOfSheduled_ArrayOfInt_screenspace_2Dpos[i].position_in2DViewportSpace, listOfSheduled_ArrayOfInt_screenspace_2Dpos[i].color, listOfSheduled_ArrayOfInt_screenspace_2Dpos[i].title, listOfSheduled_ArrayOfInt_screenspace_2Dpos[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfInt_screenspace_2Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfInt_screenspace_2Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfInt_screenspace_2Dpos[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfInt_screenspace_2Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfInt_screenspace_2Dpos_cam.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfInt_screenspace_2Dpos_cam[i].screenCamera, listOfSheduled_ArrayOfInt_screenspace_2Dpos_cam[i].intArray, listOfSheduled_ArrayOfInt_screenspace_2Dpos_cam[i].position_in2DViewportSpace, listOfSheduled_ArrayOfInt_screenspace_2Dpos_cam[i].color, listOfSheduled_ArrayOfInt_screenspace_2Dpos_cam[i].title, listOfSheduled_ArrayOfInt_screenspace_2Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfInt_screenspace_2Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfInt_screenspace_2Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfInt_screenspace_2Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfInt_screenspace_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfInt_screenspace_3Dpos.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfInt_screenspace_3Dpos[i].intList, listOfSheduled_ListOfInt_screenspace_3Dpos[i].position_in3DWorldspace, listOfSheduled_ListOfInt_screenspace_3Dpos[i].color, listOfSheduled_ListOfInt_screenspace_3Dpos[i].title, listOfSheduled_ListOfInt_screenspace_3Dpos[i].textSize_relToViewportHeight, listOfSheduled_ListOfInt_screenspace_3Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfInt_screenspace_3Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfInt_screenspace_3Dpos[i].durationInSec);
                    }
                    listOfSheduled_ListOfInt_screenspace_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfInt_screenspace_3Dpos_cam.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfInt_screenspace_3Dpos_cam[i].screenCamera, listOfSheduled_ListOfInt_screenspace_3Dpos_cam[i].intList, listOfSheduled_ListOfInt_screenspace_3Dpos_cam[i].position_in3DWorldspace, listOfSheduled_ListOfInt_screenspace_3Dpos_cam[i].color, listOfSheduled_ListOfInt_screenspace_3Dpos_cam[i].title, listOfSheduled_ListOfInt_screenspace_3Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ListOfInt_screenspace_3Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfInt_screenspace_3Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfInt_screenspace_3Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ListOfInt_screenspace_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfInt_screenspace_2Dpos.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfInt_screenspace_2Dpos[i].intList, listOfSheduled_ListOfInt_screenspace_2Dpos[i].position_in2DViewportSpace, listOfSheduled_ListOfInt_screenspace_2Dpos[i].color, listOfSheduled_ListOfInt_screenspace_2Dpos[i].title, listOfSheduled_ListOfInt_screenspace_2Dpos[i].textSize_relToViewportHeight, listOfSheduled_ListOfInt_screenspace_2Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfInt_screenspace_2Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfInt_screenspace_2Dpos[i].durationInSec);
                    }
                    listOfSheduled_ListOfInt_screenspace_2Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfInt_screenspace_2Dpos_cam.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfInt_screenspace_2Dpos_cam[i].screenCamera, listOfSheduled_ListOfInt_screenspace_2Dpos_cam[i].intList, listOfSheduled_ListOfInt_screenspace_2Dpos_cam[i].position_in2DViewportSpace, listOfSheduled_ListOfInt_screenspace_2Dpos_cam[i].color, listOfSheduled_ListOfInt_screenspace_2Dpos_cam[i].title, listOfSheduled_ListOfInt_screenspace_2Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ListOfInt_screenspace_2Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfInt_screenspace_2Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfInt_screenspace_2Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ListOfInt_screenspace_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfFloat_screenspace_3Dpos.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfFloat_screenspace_3Dpos[i].floatArray, listOfSheduled_ArrayOfFloat_screenspace_3Dpos[i].position_in3DWorldspace, listOfSheduled_ArrayOfFloat_screenspace_3Dpos[i].color, listOfSheduled_ArrayOfFloat_screenspace_3Dpos[i].title, listOfSheduled_ArrayOfFloat_screenspace_3Dpos[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfFloat_screenspace_3Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfFloat_screenspace_3Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfFloat_screenspace_3Dpos[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfFloat_screenspace_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfFloat_screenspace_3Dpos_cam.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfFloat_screenspace_3Dpos_cam[i].screenCamera, listOfSheduled_ArrayOfFloat_screenspace_3Dpos_cam[i].floatArray, listOfSheduled_ArrayOfFloat_screenspace_3Dpos_cam[i].position_in3DWorldspace, listOfSheduled_ArrayOfFloat_screenspace_3Dpos_cam[i].color, listOfSheduled_ArrayOfFloat_screenspace_3Dpos_cam[i].title, listOfSheduled_ArrayOfFloat_screenspace_3Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfFloat_screenspace_3Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfFloat_screenspace_3Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfFloat_screenspace_3Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfFloat_screenspace_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfFloat_screenspace_2Dpos.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfFloat_screenspace_2Dpos[i].floatArray, listOfSheduled_ArrayOfFloat_screenspace_2Dpos[i].position_in2DViewportSpace, listOfSheduled_ArrayOfFloat_screenspace_2Dpos[i].color, listOfSheduled_ArrayOfFloat_screenspace_2Dpos[i].title, listOfSheduled_ArrayOfFloat_screenspace_2Dpos[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfFloat_screenspace_2Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfFloat_screenspace_2Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfFloat_screenspace_2Dpos[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfFloat_screenspace_2Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfFloat_screenspace_2Dpos_cam.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfFloat_screenspace_2Dpos_cam[i].screenCamera, listOfSheduled_ArrayOfFloat_screenspace_2Dpos_cam[i].floatArray, listOfSheduled_ArrayOfFloat_screenspace_2Dpos_cam[i].position_in2DViewportSpace, listOfSheduled_ArrayOfFloat_screenspace_2Dpos_cam[i].color, listOfSheduled_ArrayOfFloat_screenspace_2Dpos_cam[i].title, listOfSheduled_ArrayOfFloat_screenspace_2Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfFloat_screenspace_2Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfFloat_screenspace_2Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfFloat_screenspace_2Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfFloat_screenspace_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfFloat_screenspace_3Dpos.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfFloat_screenspace_3Dpos[i].floatList, listOfSheduled_ListOfFloat_screenspace_3Dpos[i].position_in3DWorldspace, listOfSheduled_ListOfFloat_screenspace_3Dpos[i].color, listOfSheduled_ListOfFloat_screenspace_3Dpos[i].title, listOfSheduled_ListOfFloat_screenspace_3Dpos[i].textSize_relToViewportHeight, listOfSheduled_ListOfFloat_screenspace_3Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfFloat_screenspace_3Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfFloat_screenspace_3Dpos[i].durationInSec);
                    }
                    listOfSheduled_ListOfFloat_screenspace_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfFloat_screenspace_3Dpos_cam.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfFloat_screenspace_3Dpos_cam[i].screenCamera, listOfSheduled_ListOfFloat_screenspace_3Dpos_cam[i].floatList, listOfSheduled_ListOfFloat_screenspace_3Dpos_cam[i].position_in3DWorldspace, listOfSheduled_ListOfFloat_screenspace_3Dpos_cam[i].color, listOfSheduled_ListOfFloat_screenspace_3Dpos_cam[i].title, listOfSheduled_ListOfFloat_screenspace_3Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ListOfFloat_screenspace_3Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfFloat_screenspace_3Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfFloat_screenspace_3Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ListOfFloat_screenspace_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfFloat_screenspace_2Dpos.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfFloat_screenspace_2Dpos[i].floatList, listOfSheduled_ListOfFloat_screenspace_2Dpos[i].position_in2DViewportSpace, listOfSheduled_ListOfFloat_screenspace_2Dpos[i].color, listOfSheduled_ListOfFloat_screenspace_2Dpos[i].title, listOfSheduled_ListOfFloat_screenspace_2Dpos[i].textSize_relToViewportHeight, listOfSheduled_ListOfFloat_screenspace_2Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfFloat_screenspace_2Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfFloat_screenspace_2Dpos[i].durationInSec);
                    }
                    listOfSheduled_ListOfFloat_screenspace_2Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfFloat_screenspace_2Dpos_cam.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfFloat_screenspace_2Dpos_cam[i].screenCamera, listOfSheduled_ListOfFloat_screenspace_2Dpos_cam[i].floatList, listOfSheduled_ListOfFloat_screenspace_2Dpos_cam[i].position_in2DViewportSpace, listOfSheduled_ListOfFloat_screenspace_2Dpos_cam[i].color, listOfSheduled_ListOfFloat_screenspace_2Dpos_cam[i].title, listOfSheduled_ListOfFloat_screenspace_2Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ListOfFloat_screenspace_2Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfFloat_screenspace_2Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfFloat_screenspace_2Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ListOfFloat_screenspace_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfString_screenspace_3Dpos.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfString_screenspace_3Dpos[i].stringArray, listOfSheduled_ArrayOfString_screenspace_3Dpos[i].position_in3DWorldspace, listOfSheduled_ArrayOfString_screenspace_3Dpos[i].color, listOfSheduled_ArrayOfString_screenspace_3Dpos[i].title, listOfSheduled_ArrayOfString_screenspace_3Dpos[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfString_screenspace_3Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfString_screenspace_3Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfString_screenspace_3Dpos[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfString_screenspace_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfString_screenspace_3Dpos_cam.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfString_screenspace_3Dpos_cam[i].screenCamera, listOfSheduled_ArrayOfString_screenspace_3Dpos_cam[i].stringArray, listOfSheduled_ArrayOfString_screenspace_3Dpos_cam[i].position_in3DWorldspace, listOfSheduled_ArrayOfString_screenspace_3Dpos_cam[i].color, listOfSheduled_ArrayOfString_screenspace_3Dpos_cam[i].title, listOfSheduled_ArrayOfString_screenspace_3Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfString_screenspace_3Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfString_screenspace_3Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfString_screenspace_3Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfString_screenspace_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfString_screenspace_2Dpos.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfString_screenspace_2Dpos[i].stringArray, listOfSheduled_ArrayOfString_screenspace_2Dpos[i].position_in2DViewportSpace, listOfSheduled_ArrayOfString_screenspace_2Dpos[i].color, listOfSheduled_ArrayOfString_screenspace_2Dpos[i].title, listOfSheduled_ArrayOfString_screenspace_2Dpos[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfString_screenspace_2Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfString_screenspace_2Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfString_screenspace_2Dpos[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfString_screenspace_2Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfString_screenspace_2Dpos_cam.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfString_screenspace_2Dpos_cam[i].screenCamera, listOfSheduled_ArrayOfString_screenspace_2Dpos_cam[i].stringArray, listOfSheduled_ArrayOfString_screenspace_2Dpos_cam[i].position_in2DViewportSpace, listOfSheduled_ArrayOfString_screenspace_2Dpos_cam[i].color, listOfSheduled_ArrayOfString_screenspace_2Dpos_cam[i].title, listOfSheduled_ArrayOfString_screenspace_2Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfString_screenspace_2Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfString_screenspace_2Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfString_screenspace_2Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfString_screenspace_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfString_screenspace_3Dpos.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfString_screenspace_3Dpos[i].stringList, listOfSheduled_ListOfString_screenspace_3Dpos[i].position_in3DWorldspace, listOfSheduled_ListOfString_screenspace_3Dpos[i].color, listOfSheduled_ListOfString_screenspace_3Dpos[i].title, listOfSheduled_ListOfString_screenspace_3Dpos[i].textSize_relToViewportHeight, listOfSheduled_ListOfString_screenspace_3Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfString_screenspace_3Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfString_screenspace_3Dpos[i].durationInSec);
                    }
                    listOfSheduled_ListOfString_screenspace_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfString_screenspace_3Dpos_cam.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfString_screenspace_3Dpos_cam[i].screenCamera, listOfSheduled_ListOfString_screenspace_3Dpos_cam[i].stringList, listOfSheduled_ListOfString_screenspace_3Dpos_cam[i].position_in3DWorldspace, listOfSheduled_ListOfString_screenspace_3Dpos_cam[i].color, listOfSheduled_ListOfString_screenspace_3Dpos_cam[i].title, listOfSheduled_ListOfString_screenspace_3Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ListOfString_screenspace_3Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfString_screenspace_3Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfString_screenspace_3Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ListOfString_screenspace_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfString_screenspace_2Dpos.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfString_screenspace_2Dpos[i].stringList, listOfSheduled_ListOfString_screenspace_2Dpos[i].position_in2DViewportSpace, listOfSheduled_ListOfString_screenspace_2Dpos[i].color, listOfSheduled_ListOfString_screenspace_2Dpos[i].title, listOfSheduled_ListOfString_screenspace_2Dpos[i].textSize_relToViewportHeight, listOfSheduled_ListOfString_screenspace_2Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfString_screenspace_2Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfString_screenspace_2Dpos[i].durationInSec);
                    }
                    listOfSheduled_ListOfString_screenspace_2Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfString_screenspace_2Dpos_cam.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfString_screenspace_2Dpos_cam[i].screenCamera, listOfSheduled_ListOfString_screenspace_2Dpos_cam[i].stringList, listOfSheduled_ListOfString_screenspace_2Dpos_cam[i].position_in2DViewportSpace, listOfSheduled_ListOfString_screenspace_2Dpos_cam[i].color, listOfSheduled_ListOfString_screenspace_2Dpos_cam[i].title, listOfSheduled_ListOfString_screenspace_2Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ListOfString_screenspace_2Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfString_screenspace_2Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfString_screenspace_2Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ListOfString_screenspace_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfVector2_screenspace_3Dpos.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfVector2_screenspace_3Dpos[i].vector2Array, listOfSheduled_ArrayOfVector2_screenspace_3Dpos[i].position_in3DWorldspace, listOfSheduled_ArrayOfVector2_screenspace_3Dpos[i].color, listOfSheduled_ArrayOfVector2_screenspace_3Dpos[i].title, listOfSheduled_ArrayOfVector2_screenspace_3Dpos[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfVector2_screenspace_3Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfVector2_screenspace_3Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfVector2_screenspace_3Dpos[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfVector2_screenspace_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfVector2_screenspace_3Dpos_cam.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfVector2_screenspace_3Dpos_cam[i].screenCamera, listOfSheduled_ArrayOfVector2_screenspace_3Dpos_cam[i].vector2Array, listOfSheduled_ArrayOfVector2_screenspace_3Dpos_cam[i].position_in3DWorldspace, listOfSheduled_ArrayOfVector2_screenspace_3Dpos_cam[i].color, listOfSheduled_ArrayOfVector2_screenspace_3Dpos_cam[i].title, listOfSheduled_ArrayOfVector2_screenspace_3Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfVector2_screenspace_3Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfVector2_screenspace_3Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfVector2_screenspace_3Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfVector2_screenspace_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfVector2_screenspace_2Dpos.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfVector2_screenspace_2Dpos[i].vector2Array, listOfSheduled_ArrayOfVector2_screenspace_2Dpos[i].position_in2DViewportSpace, listOfSheduled_ArrayOfVector2_screenspace_2Dpos[i].color, listOfSheduled_ArrayOfVector2_screenspace_2Dpos[i].title, listOfSheduled_ArrayOfVector2_screenspace_2Dpos[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfVector2_screenspace_2Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfVector2_screenspace_2Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfVector2_screenspace_2Dpos[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfVector2_screenspace_2Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfVector2_screenspace_2Dpos_cam.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfVector2_screenspace_2Dpos_cam[i].screenCamera, listOfSheduled_ArrayOfVector2_screenspace_2Dpos_cam[i].vector2Array, listOfSheduled_ArrayOfVector2_screenspace_2Dpos_cam[i].position_in2DViewportSpace, listOfSheduled_ArrayOfVector2_screenspace_2Dpos_cam[i].color, listOfSheduled_ArrayOfVector2_screenspace_2Dpos_cam[i].title, listOfSheduled_ArrayOfVector2_screenspace_2Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfVector2_screenspace_2Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfVector2_screenspace_2Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfVector2_screenspace_2Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfVector2_screenspace_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfVector2_screenspace_3Dpos.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfVector2_screenspace_3Dpos[i].vector2List, listOfSheduled_ListOfVector2_screenspace_3Dpos[i].position_in3DWorldspace, listOfSheduled_ListOfVector2_screenspace_3Dpos[i].color, listOfSheduled_ListOfVector2_screenspace_3Dpos[i].title, listOfSheduled_ListOfVector2_screenspace_3Dpos[i].textSize_relToViewportHeight, listOfSheduled_ListOfVector2_screenspace_3Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfVector2_screenspace_3Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfVector2_screenspace_3Dpos[i].durationInSec);
                    }
                    listOfSheduled_ListOfVector2_screenspace_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfVector2_screenspace_3Dpos_cam.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfVector2_screenspace_3Dpos_cam[i].screenCamera, listOfSheduled_ListOfVector2_screenspace_3Dpos_cam[i].vector2List, listOfSheduled_ListOfVector2_screenspace_3Dpos_cam[i].position_in3DWorldspace, listOfSheduled_ListOfVector2_screenspace_3Dpos_cam[i].color, listOfSheduled_ListOfVector2_screenspace_3Dpos_cam[i].title, listOfSheduled_ListOfVector2_screenspace_3Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ListOfVector2_screenspace_3Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfVector2_screenspace_3Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfVector2_screenspace_3Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ListOfVector2_screenspace_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfVector2_screenspace_2Dpos.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfVector2_screenspace_2Dpos[i].vector2List, listOfSheduled_ListOfVector2_screenspace_2Dpos[i].position_in2DViewportSpace, listOfSheduled_ListOfVector2_screenspace_2Dpos[i].color, listOfSheduled_ListOfVector2_screenspace_2Dpos[i].title, listOfSheduled_ListOfVector2_screenspace_2Dpos[i].textSize_relToViewportHeight, listOfSheduled_ListOfVector2_screenspace_2Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfVector2_screenspace_2Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfVector2_screenspace_2Dpos[i].durationInSec);
                    }
                    listOfSheduled_ListOfVector2_screenspace_2Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfVector2_screenspace_2Dpos_cam.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfVector2_screenspace_2Dpos_cam[i].screenCamera, listOfSheduled_ListOfVector2_screenspace_2Dpos_cam[i].vector2List, listOfSheduled_ListOfVector2_screenspace_2Dpos_cam[i].position_in2DViewportSpace, listOfSheduled_ListOfVector2_screenspace_2Dpos_cam[i].color, listOfSheduled_ListOfVector2_screenspace_2Dpos_cam[i].title, listOfSheduled_ListOfVector2_screenspace_2Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ListOfVector2_screenspace_2Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfVector2_screenspace_2Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfVector2_screenspace_2Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ListOfVector2_screenspace_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfVector3_screenspace_3Dpos.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfVector3_screenspace_3Dpos[i].vector3Array, listOfSheduled_ArrayOfVector3_screenspace_3Dpos[i].position_in3DWorldspace, listOfSheduled_ArrayOfVector3_screenspace_3Dpos[i].color, listOfSheduled_ArrayOfVector3_screenspace_3Dpos[i].title, listOfSheduled_ArrayOfVector3_screenspace_3Dpos[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfVector3_screenspace_3Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfVector3_screenspace_3Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfVector3_screenspace_3Dpos[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfVector3_screenspace_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfVector3_screenspace_3Dpos_cam.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfVector3_screenspace_3Dpos_cam[i].screenCamera, listOfSheduled_ArrayOfVector3_screenspace_3Dpos_cam[i].vector3Array, listOfSheduled_ArrayOfVector3_screenspace_3Dpos_cam[i].position_in3DWorldspace, listOfSheduled_ArrayOfVector3_screenspace_3Dpos_cam[i].color, listOfSheduled_ArrayOfVector3_screenspace_3Dpos_cam[i].title, listOfSheduled_ArrayOfVector3_screenspace_3Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfVector3_screenspace_3Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfVector3_screenspace_3Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfVector3_screenspace_3Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfVector3_screenspace_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfVector3_screenspace_2Dpos.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfVector3_screenspace_2Dpos[i].vector3Array, listOfSheduled_ArrayOfVector3_screenspace_2Dpos[i].position_in2DViewportSpace, listOfSheduled_ArrayOfVector3_screenspace_2Dpos[i].color, listOfSheduled_ArrayOfVector3_screenspace_2Dpos[i].title, listOfSheduled_ArrayOfVector3_screenspace_2Dpos[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfVector3_screenspace_2Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfVector3_screenspace_2Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfVector3_screenspace_2Dpos[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfVector3_screenspace_2Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfVector3_screenspace_2Dpos_cam.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfVector3_screenspace_2Dpos_cam[i].screenCamera, listOfSheduled_ArrayOfVector3_screenspace_2Dpos_cam[i].vector3Array, listOfSheduled_ArrayOfVector3_screenspace_2Dpos_cam[i].position_in2DViewportSpace, listOfSheduled_ArrayOfVector3_screenspace_2Dpos_cam[i].color, listOfSheduled_ArrayOfVector3_screenspace_2Dpos_cam[i].title, listOfSheduled_ArrayOfVector3_screenspace_2Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfVector3_screenspace_2Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfVector3_screenspace_2Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfVector3_screenspace_2Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfVector3_screenspace_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfVector3_screenspace_3Dpos.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfVector3_screenspace_3Dpos[i].vector3List, listOfSheduled_ListOfVector3_screenspace_3Dpos[i].position_in3DWorldspace, listOfSheduled_ListOfVector3_screenspace_3Dpos[i].color, listOfSheduled_ListOfVector3_screenspace_3Dpos[i].title, listOfSheduled_ListOfVector3_screenspace_3Dpos[i].textSize_relToViewportHeight, listOfSheduled_ListOfVector3_screenspace_3Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfVector3_screenspace_3Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfVector3_screenspace_3Dpos[i].durationInSec);
                    }
                    listOfSheduled_ListOfVector3_screenspace_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfVector3_screenspace_3Dpos_cam.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfVector3_screenspace_3Dpos_cam[i].screenCamera, listOfSheduled_ListOfVector3_screenspace_3Dpos_cam[i].vector3List, listOfSheduled_ListOfVector3_screenspace_3Dpos_cam[i].position_in3DWorldspace, listOfSheduled_ListOfVector3_screenspace_3Dpos_cam[i].color, listOfSheduled_ListOfVector3_screenspace_3Dpos_cam[i].title, listOfSheduled_ListOfVector3_screenspace_3Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ListOfVector3_screenspace_3Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfVector3_screenspace_3Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfVector3_screenspace_3Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ListOfVector3_screenspace_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfVector3_screenspace_2Dpos.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfVector3_screenspace_2Dpos[i].vector3List, listOfSheduled_ListOfVector3_screenspace_2Dpos[i].position_in2DViewportSpace, listOfSheduled_ListOfVector3_screenspace_2Dpos[i].color, listOfSheduled_ListOfVector3_screenspace_2Dpos[i].title, listOfSheduled_ListOfVector3_screenspace_2Dpos[i].textSize_relToViewportHeight, listOfSheduled_ListOfVector3_screenspace_2Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfVector3_screenspace_2Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfVector3_screenspace_2Dpos[i].durationInSec);
                    }
                    listOfSheduled_ListOfVector3_screenspace_2Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfVector3_screenspace_2Dpos_cam.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfVector3_screenspace_2Dpos_cam[i].screenCamera, listOfSheduled_ListOfVector3_screenspace_2Dpos_cam[i].vector3List, listOfSheduled_ListOfVector3_screenspace_2Dpos_cam[i].position_in2DViewportSpace, listOfSheduled_ListOfVector3_screenspace_2Dpos_cam[i].color, listOfSheduled_ListOfVector3_screenspace_2Dpos_cam[i].title, listOfSheduled_ListOfVector3_screenspace_2Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ListOfVector3_screenspace_2Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfVector3_screenspace_2Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfVector3_screenspace_2Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ListOfVector3_screenspace_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfVector4_screenspace_3Dpos.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfVector4_screenspace_3Dpos[i].vector4Array, listOfSheduled_ArrayOfVector4_screenspace_3Dpos[i].position_in3DWorldspace, listOfSheduled_ArrayOfVector4_screenspace_3Dpos[i].color, listOfSheduled_ArrayOfVector4_screenspace_3Dpos[i].title, listOfSheduled_ArrayOfVector4_screenspace_3Dpos[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfVector4_screenspace_3Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfVector4_screenspace_3Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfVector4_screenspace_3Dpos[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfVector4_screenspace_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfVector4_screenspace_3Dpos_cam.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfVector4_screenspace_3Dpos_cam[i].screenCamera, listOfSheduled_ArrayOfVector4_screenspace_3Dpos_cam[i].vector4Array, listOfSheduled_ArrayOfVector4_screenspace_3Dpos_cam[i].position_in3DWorldspace, listOfSheduled_ArrayOfVector4_screenspace_3Dpos_cam[i].color, listOfSheduled_ArrayOfVector4_screenspace_3Dpos_cam[i].title, listOfSheduled_ArrayOfVector4_screenspace_3Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfVector4_screenspace_3Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfVector4_screenspace_3Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfVector4_screenspace_3Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfVector4_screenspace_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfVector4_screenspace_2Dpos.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfVector4_screenspace_2Dpos[i].vector4Array, listOfSheduled_ArrayOfVector4_screenspace_2Dpos[i].position_in2DViewportSpace, listOfSheduled_ArrayOfVector4_screenspace_2Dpos[i].color, listOfSheduled_ArrayOfVector4_screenspace_2Dpos[i].title, listOfSheduled_ArrayOfVector4_screenspace_2Dpos[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfVector4_screenspace_2Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfVector4_screenspace_2Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfVector4_screenspace_2Dpos[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfVector4_screenspace_2Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ArrayOfVector4_screenspace_2Dpos_cam.Count; i++)
                    {
                        DrawText.WriteArrayScreenspace(listOfSheduled_ArrayOfVector4_screenspace_2Dpos_cam[i].screenCamera, listOfSheduled_ArrayOfVector4_screenspace_2Dpos_cam[i].vector4Array, listOfSheduled_ArrayOfVector4_screenspace_2Dpos_cam[i].position_in2DViewportSpace, listOfSheduled_ArrayOfVector4_screenspace_2Dpos_cam[i].color, listOfSheduled_ArrayOfVector4_screenspace_2Dpos_cam[i].title, listOfSheduled_ArrayOfVector4_screenspace_2Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ArrayOfVector4_screenspace_2Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ArrayOfVector4_screenspace_2Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ArrayOfVector4_screenspace_2Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ArrayOfVector4_screenspace_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfVector4_screenspace_3Dpos.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfVector4_screenspace_3Dpos[i].vector4List, listOfSheduled_ListOfVector4_screenspace_3Dpos[i].position_in3DWorldspace, listOfSheduled_ListOfVector4_screenspace_3Dpos[i].color, listOfSheduled_ListOfVector4_screenspace_3Dpos[i].title, listOfSheduled_ListOfVector4_screenspace_3Dpos[i].textSize_relToViewportHeight, listOfSheduled_ListOfVector4_screenspace_3Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfVector4_screenspace_3Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfVector4_screenspace_3Dpos[i].durationInSec);
                    }
                    listOfSheduled_ListOfVector4_screenspace_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfVector4_screenspace_3Dpos_cam.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfVector4_screenspace_3Dpos_cam[i].screenCamera, listOfSheduled_ListOfVector4_screenspace_3Dpos_cam[i].vector4List, listOfSheduled_ListOfVector4_screenspace_3Dpos_cam[i].position_in3DWorldspace, listOfSheduled_ListOfVector4_screenspace_3Dpos_cam[i].color, listOfSheduled_ListOfVector4_screenspace_3Dpos_cam[i].title, listOfSheduled_ListOfVector4_screenspace_3Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ListOfVector4_screenspace_3Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfVector4_screenspace_3Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfVector4_screenspace_3Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ListOfVector4_screenspace_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfVector4_screenspace_2Dpos.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfVector4_screenspace_2Dpos[i].vector4List, listOfSheduled_ListOfVector4_screenspace_2Dpos[i].position_in2DViewportSpace, listOfSheduled_ListOfVector4_screenspace_2Dpos[i].color, listOfSheduled_ListOfVector4_screenspace_2Dpos[i].title, listOfSheduled_ListOfVector4_screenspace_2Dpos[i].textSize_relToViewportHeight, listOfSheduled_ListOfVector4_screenspace_2Dpos[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfVector4_screenspace_2Dpos[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfVector4_screenspace_2Dpos[i].durationInSec);
                    }
                    listOfSheduled_ListOfVector4_screenspace_2Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ListOfVector4_screenspace_2Dpos_cam.Count; i++)
                    {
                        DrawText.WriteListScreenspace(listOfSheduled_ListOfVector4_screenspace_2Dpos_cam[i].screenCamera, listOfSheduled_ListOfVector4_screenspace_2Dpos_cam[i].vector4List, listOfSheduled_ListOfVector4_screenspace_2Dpos_cam[i].position_in2DViewportSpace, listOfSheduled_ListOfVector4_screenspace_2Dpos_cam[i].color, listOfSheduled_ListOfVector4_screenspace_2Dpos_cam[i].title, listOfSheduled_ListOfVector4_screenspace_2Dpos_cam[i].textSize_relToViewportHeight, listOfSheduled_ListOfVector4_screenspace_2Dpos_cam[i].forceHeightOfWholeTableBox_relToViewportHeight, listOfSheduled_ListOfVector4_screenspace_2Dpos_cam[i].position_isTopLeft_notLowLeft, listOfSheduled_ListOfVector4_screenspace_2Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ListOfVector4_screenspace_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_TagGameObjectScreenspace.Count; i++)
                    {
                        DrawEngineBasics.TagGameObjectScreenspace(listOfSheduled_TagGameObjectScreenspace[i].screenCamera, listOfSheduled_TagGameObjectScreenspace[i].gameObject, listOfSheduled_TagGameObjectScreenspace[i].text, listOfSheduled_TagGameObjectScreenspace[i].colorForText, listOfSheduled_TagGameObjectScreenspace[i].colorForTagBox, listOfSheduled_TagGameObjectScreenspace[i].linesWidth_relToViewportHeight, listOfSheduled_TagGameObjectScreenspace[i].drawPointerIfOffscreen, listOfSheduled_TagGameObjectScreenspace[i].relTextSizeScaling, listOfSheduled_TagGameObjectScreenspace[i].encapsulateChildren, listOfSheduled_TagGameObjectScreenspace[i].durationInSec);
                    }
                    listOfSheduled_TagGameObjectScreenspace.Clear();

                    for (int i = 0; i < listOfSheduled_GridScreenspace.Count; i++)
                    {
                        DrawEngineBasics.GridScreenspace(listOfSheduled_GridScreenspace[i].camera, listOfSheduled_GridScreenspace[i].color, listOfSheduled_GridScreenspace[i].linesWidth_relToViewportHeight, listOfSheduled_GridScreenspace[i].drawTenthLines, listOfSheduled_GridScreenspace[i].drawHundredthLines, listOfSheduled_GridScreenspace[i].gridScreenspaceMode, listOfSheduled_GridScreenspace[i].durationInSec);
                    }
                    listOfSheduled_GridScreenspace.Clear();

                    for (int i = 0; i < listOfSheduled_BoolDisplayerScreenspace_3Dpos.Count; i++)
                    {
                        DrawEngineBasics.BoolDisplayerScreenspace(listOfSheduled_BoolDisplayerScreenspace_3Dpos[i].boolValueToDisplay, listOfSheduled_BoolDisplayerScreenspace_3Dpos[i].boolName, listOfSheduled_BoolDisplayerScreenspace_3Dpos[i].position_in3DWorldspace, listOfSheduled_BoolDisplayerScreenspace_3Dpos[i].size_relToViewportHeight, listOfSheduled_BoolDisplayerScreenspace_3Dpos[i].color_forTextAndFrame, listOfSheduled_BoolDisplayerScreenspace_3Dpos[i].overwriteColor_forTrue, listOfSheduled_BoolDisplayerScreenspace_3Dpos[i].overwriteColor_forFalse, listOfSheduled_BoolDisplayerScreenspace_3Dpos[i].durationInSec);
                    }
                    listOfSheduled_BoolDisplayerScreenspace_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_BoolDisplayerScreenspace_3Dpos_cam.Count; i++)
                    {
                        DrawEngineBasics.BoolDisplayerScreenspace(listOfSheduled_BoolDisplayerScreenspace_3Dpos_cam[i].screenCamera, listOfSheduled_BoolDisplayerScreenspace_3Dpos_cam[i].boolValueToDisplay, listOfSheduled_BoolDisplayerScreenspace_3Dpos_cam[i].boolName, listOfSheduled_BoolDisplayerScreenspace_3Dpos_cam[i].position_in3DWorldspace, listOfSheduled_BoolDisplayerScreenspace_3Dpos_cam[i].size_relToViewportHeight, listOfSheduled_BoolDisplayerScreenspace_3Dpos_cam[i].color_forTextAndFrame, listOfSheduled_BoolDisplayerScreenspace_3Dpos_cam[i].overwriteColor_forTrue, listOfSheduled_BoolDisplayerScreenspace_3Dpos_cam[i].overwriteColor_forFalse, listOfSheduled_BoolDisplayerScreenspace_3Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_BoolDisplayerScreenspace_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_BoolDisplayerScreenspace_2Dpos_cam.Count; i++)
                    {
                        DrawEngineBasics.BoolDisplayerScreenspace(listOfSheduled_BoolDisplayerScreenspace_2Dpos_cam[i].screenCamera, listOfSheduled_BoolDisplayerScreenspace_2Dpos_cam[i].boolValueToDisplay, listOfSheduled_BoolDisplayerScreenspace_2Dpos_cam[i].boolName, listOfSheduled_BoolDisplayerScreenspace_2Dpos_cam[i].position_in2DViewportSpace, listOfSheduled_BoolDisplayerScreenspace_2Dpos_cam[i].size_relToViewportHeight, listOfSheduled_BoolDisplayerScreenspace_2Dpos_cam[i].color_forTextAndFrame, listOfSheduled_BoolDisplayerScreenspace_2Dpos_cam[i].overwriteColor_forTrue, listOfSheduled_BoolDisplayerScreenspace_2Dpos_cam[i].overwriteColor_forFalse, listOfSheduled_BoolDisplayerScreenspace_2Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_BoolDisplayerScreenspace_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_LogsOnScreen.Count; i++)
                    {
                        bool logListenerBoolState_before = DrawLogs.LogMessageListenerForLogsOnScreen_isActivated;
                        if (listOfSheduled_LogsOnScreen[i].logListenerWasActive)
                        {
                            DrawLogs.ActivateLogMessageListenerForLogsOnScreen();
                        }
                        else
                        {
                            DrawLogs.DeactivateLogMessageListenerForLogsOnScreen();
                        }
                        DrawLogs.LogsOnScreen(listOfSheduled_LogsOnScreen[i].cameraWhereToDraw, listOfSheduled_LogsOnScreen[i].drawNormalPrio, listOfSheduled_LogsOnScreen[i].drawWarningPrio, listOfSheduled_LogsOnScreen[i].drawErrorPrio, listOfSheduled_LogsOnScreen[i].maxNumberOfDisplayedLogMessages, listOfSheduled_LogsOnScreen[i].textSize_relToViewportHeight, listOfSheduled_LogsOnScreen[i].textColor, listOfSheduled_LogsOnScreen[i].stackTraceForNormalPrio, listOfSheduled_LogsOnScreen[i].stackTraceForWarningPrio, listOfSheduled_LogsOnScreen[i].stackTraceForErrorPrio, listOfSheduled_LogsOnScreen[i].durationInSec);
                        if (logListenerBoolState_before)
                        {
                            DrawLogs.ActivateLogMessageListenerForLogsOnScreen();
                        }
                        else
                        {
                            DrawLogs.DeactivateLogMessageListenerForLogsOnScreen();
                        }
                    }
                    listOfSheduled_LogsOnScreen.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceLine.Count; i++)
                    {
                        DrawScreenspace.Line(listOfSheduled_ScreenspaceLine[i].targetCamera, listOfSheduled_ScreenspaceLine[i].start, listOfSheduled_ScreenspaceLine[i].end, listOfSheduled_ScreenspaceLine[i].color, listOfSheduled_ScreenspaceLine[i].width_relToViewportHeight, listOfSheduled_ScreenspaceLine[i].text, listOfSheduled_ScreenspaceLine[i].style, listOfSheduled_ScreenspaceLine[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceLine[i].animationSpeed, listOfSheduled_ScreenspaceLine[i].endPlatesSize_relToViewportHeight, listOfSheduled_ScreenspaceLine[i].alphaFadeOutLength_0to1, listOfSheduled_ScreenspaceLine[i].enlargeSmallTextToThisMinRelTextSize, listOfSheduled_ScreenspaceLine[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceLine.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceRay.Count; i++)
                    {
                        DrawScreenspace.Ray(listOfSheduled_ScreenspaceRay[i].targetCamera, listOfSheduled_ScreenspaceRay[i].start, listOfSheduled_ScreenspaceRay[i].direction, listOfSheduled_ScreenspaceRay[i].color, listOfSheduled_ScreenspaceRay[i].width_relToViewportHeight, listOfSheduled_ScreenspaceRay[i].text, listOfSheduled_ScreenspaceRay[i].interpretDirectionAsUnwarped, listOfSheduled_ScreenspaceRay[i].style, listOfSheduled_ScreenspaceRay[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceRay[i].animationSpeed, listOfSheduled_ScreenspaceRay[i].endPlatesSize_relToViewportHeight, listOfSheduled_ScreenspaceRay[i].alphaFadeOutLength_0to1, listOfSheduled_ScreenspaceRay[i].enlargeSmallTextToThisMinRelTextSize, listOfSheduled_ScreenspaceRay[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceRay.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceLineFrom.Count; i++)
                    {
                        DrawScreenspace.LineFrom(listOfSheduled_ScreenspaceLineFrom[i].targetCamera, listOfSheduled_ScreenspaceLineFrom[i].start, listOfSheduled_ScreenspaceLineFrom[i].direction, listOfSheduled_ScreenspaceLineFrom[i].color, listOfSheduled_ScreenspaceLineFrom[i].width_relToViewportHeight, listOfSheduled_ScreenspaceLineFrom[i].text, listOfSheduled_ScreenspaceLineFrom[i].interpretDirectionAsUnwarped, listOfSheduled_ScreenspaceLineFrom[i].style, listOfSheduled_ScreenspaceLineFrom[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceLineFrom[i].animationSpeed, listOfSheduled_ScreenspaceLineFrom[i].endPlatesSize_relToViewportHeight, listOfSheduled_ScreenspaceLineFrom[i].alphaFadeOutLength_0to1, listOfSheduled_ScreenspaceLineFrom[i].enlargeSmallTextToThisMinRelTextSize, listOfSheduled_ScreenspaceLineFrom[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceLineFrom.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceLineTo.Count; i++)
                    {
                        DrawScreenspace.LineTo(listOfSheduled_ScreenspaceLineTo[i].targetCamera, listOfSheduled_ScreenspaceLineTo[i].direction, listOfSheduled_ScreenspaceLineTo[i].end, listOfSheduled_ScreenspaceLineTo[i].color, listOfSheduled_ScreenspaceLineTo[i].width_relToViewportHeight, listOfSheduled_ScreenspaceLineTo[i].text, listOfSheduled_ScreenspaceLineTo[i].interpretDirectionAsUnwarped, listOfSheduled_ScreenspaceLineTo[i].style, listOfSheduled_ScreenspaceLineTo[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceLineTo[i].animationSpeed, listOfSheduled_ScreenspaceLineTo[i].endPlatesSize_relToViewportHeight, listOfSheduled_ScreenspaceLineTo[i].alphaFadeOutLength_0to1, listOfSheduled_ScreenspaceLineTo[i].enlargeSmallTextToThisMinRelTextSize, listOfSheduled_ScreenspaceLineTo[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceLineTo.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceLineColorFade.Count; i++)
                    {
                        DrawScreenspace.LineColorFade(listOfSheduled_ScreenspaceLineColorFade[i].targetCamera, listOfSheduled_ScreenspaceLineColorFade[i].start, listOfSheduled_ScreenspaceLineColorFade[i].end, listOfSheduled_ScreenspaceLineColorFade[i].startColor, listOfSheduled_ScreenspaceLineColorFade[i].endColor, listOfSheduled_ScreenspaceLineColorFade[i].width_relToViewportHeight, listOfSheduled_ScreenspaceLineColorFade[i].text, listOfSheduled_ScreenspaceLineColorFade[i].style, listOfSheduled_ScreenspaceLineColorFade[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceLineColorFade[i].animationSpeed, listOfSheduled_ScreenspaceLineColorFade[i].endPlatesSize_relToViewportHeight, listOfSheduled_ScreenspaceLineColorFade[i].alphaFadeOutLength_0to1, listOfSheduled_ScreenspaceLineColorFade[i].enlargeSmallTextToThisMinRelTextSize, listOfSheduled_ScreenspaceLineColorFade[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceLineColorFade.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceRayColorFade.Count; i++)
                    {
                        DrawScreenspace.RayColorFade(listOfSheduled_ScreenspaceRayColorFade[i].targetCamera, listOfSheduled_ScreenspaceRayColorFade[i].start, listOfSheduled_ScreenspaceRayColorFade[i].direction, listOfSheduled_ScreenspaceRayColorFade[i].startColor, listOfSheduled_ScreenspaceRayColorFade[i].endColor, listOfSheduled_ScreenspaceRayColorFade[i].width_relToViewportHeight, listOfSheduled_ScreenspaceRayColorFade[i].text, listOfSheduled_ScreenspaceRayColorFade[i].interpretDirectionAsUnwarped, listOfSheduled_ScreenspaceRayColorFade[i].style, listOfSheduled_ScreenspaceRayColorFade[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceRayColorFade[i].animationSpeed, listOfSheduled_ScreenspaceRayColorFade[i].endPlatesSize_relToViewportHeight, listOfSheduled_ScreenspaceRayColorFade[i].alphaFadeOutLength_0to1, listOfSheduled_ScreenspaceRayColorFade[i].enlargeSmallTextToThisMinRelTextSize, listOfSheduled_ScreenspaceRayColorFade[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceRayColorFade.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceLineFrom_withColorFade.Count; i++)
                    {
                        DrawScreenspace.LineFrom_withColorFade(listOfSheduled_ScreenspaceLineFrom_withColorFade[i].targetCamera, listOfSheduled_ScreenspaceLineFrom_withColorFade[i].start, listOfSheduled_ScreenspaceLineFrom_withColorFade[i].direction, listOfSheduled_ScreenspaceLineFrom_withColorFade[i].startColor, listOfSheduled_ScreenspaceLineFrom_withColorFade[i].endColor, listOfSheduled_ScreenspaceLineFrom_withColorFade[i].width_relToViewportHeight, listOfSheduled_ScreenspaceLineFrom_withColorFade[i].text, listOfSheduled_ScreenspaceLineFrom_withColorFade[i].interpretDirectionAsUnwarped, listOfSheduled_ScreenspaceLineFrom_withColorFade[i].style, listOfSheduled_ScreenspaceLineFrom_withColorFade[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceLineFrom_withColorFade[i].animationSpeed, listOfSheduled_ScreenspaceLineFrom_withColorFade[i].endPlatesSize_relToViewportHeight, listOfSheduled_ScreenspaceLineFrom_withColorFade[i].alphaFadeOutLength_0to1, listOfSheduled_ScreenspaceLineFrom_withColorFade[i].enlargeSmallTextToThisMinRelTextSize, listOfSheduled_ScreenspaceLineFrom_withColorFade[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceLineFrom_withColorFade.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceLineTo_withColorFade.Count; i++)
                    {
                        DrawScreenspace.LineTo_withColorFade(listOfSheduled_ScreenspaceLineTo_withColorFade[i].targetCamera, listOfSheduled_ScreenspaceLineTo_withColorFade[i].direction, listOfSheduled_ScreenspaceLineTo_withColorFade[i].end, listOfSheduled_ScreenspaceLineTo_withColorFade[i].startColor, listOfSheduled_ScreenspaceLineTo_withColorFade[i].endColor, listOfSheduled_ScreenspaceLineTo_withColorFade[i].width_relToViewportHeight, listOfSheduled_ScreenspaceLineTo_withColorFade[i].text, listOfSheduled_ScreenspaceLineTo_withColorFade[i].interpretDirectionAsUnwarped, listOfSheduled_ScreenspaceLineTo_withColorFade[i].style, listOfSheduled_ScreenspaceLineTo_withColorFade[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceLineTo_withColorFade[i].animationSpeed, listOfSheduled_ScreenspaceLineTo_withColorFade[i].endPlatesSize_relToViewportHeight, listOfSheduled_ScreenspaceLineTo_withColorFade[i].alphaFadeOutLength_0to1, listOfSheduled_ScreenspaceLineTo_withColorFade[i].enlargeSmallTextToThisMinRelTextSize, listOfSheduled_ScreenspaceLineTo_withColorFade[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceLineTo_withColorFade.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceLineCircled_angleToAngle_cam.Count; i++)
                    {
                        DrawScreenspace.LineCircled(listOfSheduled_ScreenspaceLineCircled_angleToAngle_cam[i].targetCamera, listOfSheduled_ScreenspaceLineCircled_angleToAngle_cam[i].circleCenter, listOfSheduled_ScreenspaceLineCircled_angleToAngle_cam[i].startAngleDegCC_relativeToUp, listOfSheduled_ScreenspaceLineCircled_angleToAngle_cam[i].endAngleDegCC_relativeToUp, listOfSheduled_ScreenspaceLineCircled_angleToAngle_cam[i].radius_relToViewportHeight, listOfSheduled_ScreenspaceLineCircled_angleToAngle_cam[i].color, listOfSheduled_ScreenspaceLineCircled_angleToAngle_cam[i].width_relToViewportHeight, listOfSheduled_ScreenspaceLineCircled_angleToAngle_cam[i].text, listOfSheduled_ScreenspaceLineCircled_angleToAngle_cam[i].skipFallbackDisplayOfZeroAngles, listOfSheduled_ScreenspaceLineCircled_angleToAngle_cam[i].minAngleDeg_withoutTextLineBreak, listOfSheduled_ScreenspaceLineCircled_angleToAngle_cam[i].textAnchor, listOfSheduled_ScreenspaceLineCircled_angleToAngle_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceLineCircled_angleToAngle_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceLineCircled_angleFromStartPos_cam.Count; i++)
                    {
                        DrawScreenspace.LineCircled(listOfSheduled_ScreenspaceLineCircled_angleFromStartPos_cam[i].targetCamera, listOfSheduled_ScreenspaceLineCircled_angleFromStartPos_cam[i].startPos, listOfSheduled_ScreenspaceLineCircled_angleFromStartPos_cam[i].circleCenter, listOfSheduled_ScreenspaceLineCircled_angleFromStartPos_cam[i].turnAngleDegCC, listOfSheduled_ScreenspaceLineCircled_angleFromStartPos_cam[i].color, listOfSheduled_ScreenspaceLineCircled_angleFromStartPos_cam[i].width_relToViewportHeight, listOfSheduled_ScreenspaceLineCircled_angleFromStartPos_cam[i].text, listOfSheduled_ScreenspaceLineCircled_angleFromStartPos_cam[i].skipFallbackDisplayOfZeroAngles, listOfSheduled_ScreenspaceLineCircled_angleFromStartPos_cam[i].minAngleDeg_withoutTextLineBreak, listOfSheduled_ScreenspaceLineCircled_angleFromStartPos_cam[i].textAnchor, listOfSheduled_ScreenspaceLineCircled_angleFromStartPos_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceLineCircled_angleFromStartPos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceCircleSegment_angleToAngle_cam.Count; i++)
                    {
                        DrawScreenspace.CircleSegment(listOfSheduled_ScreenspaceCircleSegment_angleToAngle_cam[i].targetCamera, listOfSheduled_ScreenspaceCircleSegment_angleToAngle_cam[i].circleCenter, listOfSheduled_ScreenspaceCircleSegment_angleToAngle_cam[i].startAngleDegCC_relativeToUp, listOfSheduled_ScreenspaceCircleSegment_angleToAngle_cam[i].endAngleDegCC_relativeToUp, listOfSheduled_ScreenspaceCircleSegment_angleToAngle_cam[i].radius_relToViewportHeight, listOfSheduled_ScreenspaceCircleSegment_angleToAngle_cam[i].color, listOfSheduled_ScreenspaceCircleSegment_angleToAngle_cam[i].text, listOfSheduled_ScreenspaceCircleSegment_angleToAngle_cam[i].radiusPortionWhereDrawFillStarts, listOfSheduled_ScreenspaceCircleSegment_angleToAngle_cam[i].skipFallbackDisplayOfZeroAngles, listOfSheduled_ScreenspaceCircleSegment_angleToAngle_cam[i].fillDensity, listOfSheduled_ScreenspaceCircleSegment_angleToAngle_cam[i].minAngleDeg_withoutTextLineBreak, listOfSheduled_ScreenspaceCircleSegment_angleToAngle_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceCircleSegment_angleToAngle_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceCircleSegment_angleFromStartPos_cam.Count; i++)
                    {
                        DrawScreenspace.CircleSegment(listOfSheduled_ScreenspaceCircleSegment_angleFromStartPos_cam[i].targetCamera, listOfSheduled_ScreenspaceCircleSegment_angleFromStartPos_cam[i].startPosOnPerimeter, listOfSheduled_ScreenspaceCircleSegment_angleFromStartPos_cam[i].circleCenter, listOfSheduled_ScreenspaceCircleSegment_angleFromStartPos_cam[i].turnAngleDegCC, listOfSheduled_ScreenspaceCircleSegment_angleFromStartPos_cam[i].color, listOfSheduled_ScreenspaceCircleSegment_angleFromStartPos_cam[i].text, listOfSheduled_ScreenspaceCircleSegment_angleFromStartPos_cam[i].radiusPortionWhereDrawFillStarts, listOfSheduled_ScreenspaceCircleSegment_angleFromStartPos_cam[i].skipFallbackDisplayOfZeroAngles, listOfSheduled_ScreenspaceCircleSegment_angleFromStartPos_cam[i].fillDensity, listOfSheduled_ScreenspaceCircleSegment_angleFromStartPos_cam[i].minAngleDeg_withoutTextLineBreak, listOfSheduled_ScreenspaceCircleSegment_angleFromStartPos_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceCircleSegment_angleFromStartPos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceLineString_array_cam.Count; i++)
                    {
                        DrawScreenspace.LineString(listOfSheduled_ScreenspaceLineString_array_cam[i].targetCamera, listOfSheduled_ScreenspaceLineString_array_cam[i].points, listOfSheduled_ScreenspaceLineString_array_cam[i].color, listOfSheduled_ScreenspaceLineString_array_cam[i].closeGapBetweenLastAndFirstPoint, listOfSheduled_ScreenspaceLineString_array_cam[i].width_relToViewportHeight, listOfSheduled_ScreenspaceLineString_array_cam[i].text, listOfSheduled_ScreenspaceLineString_array_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceLineString_array_cam[i].style, listOfSheduled_ScreenspaceLineString_array_cam[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceLineString_array_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceLineString_array_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceLineString_list_cam.Count; i++)
                    {
                        DrawScreenspace.LineString(listOfSheduled_ScreenspaceLineString_list_cam[i].targetCamera, listOfSheduled_ScreenspaceLineString_list_cam[i].points, listOfSheduled_ScreenspaceLineString_list_cam[i].color, listOfSheduled_ScreenspaceLineString_list_cam[i].closeGapBetweenLastAndFirstPoint, listOfSheduled_ScreenspaceLineString_list_cam[i].width_relToViewportHeight, listOfSheduled_ScreenspaceLineString_list_cam[i].text, listOfSheduled_ScreenspaceLineString_list_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceLineString_list_cam[i].style, listOfSheduled_ScreenspaceLineString_list_cam[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceLineString_list_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceLineString_list_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceLineStringColorFade_array_cam.Count; i++)
                    {
                        DrawScreenspace.LineStringColorFade(listOfSheduled_ScreenspaceLineStringColorFade_array_cam[i].targetCamera, listOfSheduled_ScreenspaceLineStringColorFade_array_cam[i].points, listOfSheduled_ScreenspaceLineStringColorFade_array_cam[i].startColor, listOfSheduled_ScreenspaceLineStringColorFade_array_cam[i].endColor, listOfSheduled_ScreenspaceLineStringColorFade_array_cam[i].closeGapBetweenLastAndFirstPoint, listOfSheduled_ScreenspaceLineStringColorFade_array_cam[i].width_relToViewportHeight, listOfSheduled_ScreenspaceLineStringColorFade_array_cam[i].text, listOfSheduled_ScreenspaceLineStringColorFade_array_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceLineStringColorFade_array_cam[i].style, listOfSheduled_ScreenspaceLineStringColorFade_array_cam[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceLineStringColorFade_array_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceLineStringColorFade_array_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceLineStringColorFade_list_cam.Count; i++)
                    {
                        DrawScreenspace.LineStringColorFade(listOfSheduled_ScreenspaceLineStringColorFade_list_cam[i].targetCamera, listOfSheduled_ScreenspaceLineStringColorFade_list_cam[i].points, listOfSheduled_ScreenspaceLineStringColorFade_list_cam[i].startColor, listOfSheduled_ScreenspaceLineStringColorFade_list_cam[i].endColor, listOfSheduled_ScreenspaceLineStringColorFade_list_cam[i].closeGapBetweenLastAndFirstPoint, listOfSheduled_ScreenspaceLineStringColorFade_list_cam[i].width_relToViewportHeight, listOfSheduled_ScreenspaceLineStringColorFade_list_cam[i].text, listOfSheduled_ScreenspaceLineStringColorFade_list_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceLineStringColorFade_list_cam[i].style, listOfSheduled_ScreenspaceLineStringColorFade_list_cam[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceLineStringColorFade_list_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceLineStringColorFade_list_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceShape_3Dpos.Count; i++)
                    {
                        DrawScreenspace.Shape(listOfSheduled_ScreenspaceShape_3Dpos[i].centerPosition_in3DWorldspace, listOfSheduled_ScreenspaceShape_3Dpos[i].shape, listOfSheduled_ScreenspaceShape_3Dpos[i].color, listOfSheduled_ScreenspaceShape_3Dpos[i].width_relToViewportHeight, listOfSheduled_ScreenspaceShape_3Dpos[i].height_relToViewportHeight, listOfSheduled_ScreenspaceShape_3Dpos[i].zRotationDegCC, listOfSheduled_ScreenspaceShape_3Dpos[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspaceShape_3Dpos[i].text, listOfSheduled_ScreenspaceShape_3Dpos[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceShape_3Dpos[i].lineStyle, listOfSheduled_ScreenspaceShape_3Dpos[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceShape_3Dpos[i].fillStyle, listOfSheduled_ScreenspaceShape_3Dpos[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspaceShape_3Dpos[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceShape_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceShape_3Dpos_cam.Count; i++)
                    {
                        DrawScreenspace.Shape(listOfSheduled_ScreenspaceShape_3Dpos_cam[i].targetCamera, listOfSheduled_ScreenspaceShape_3Dpos_cam[i].centerPosition_in3DWorldspace, listOfSheduled_ScreenspaceShape_3Dpos_cam[i].shape, listOfSheduled_ScreenspaceShape_3Dpos_cam[i].color, listOfSheduled_ScreenspaceShape_3Dpos_cam[i].width_relToViewportHeight, listOfSheduled_ScreenspaceShape_3Dpos_cam[i].height_relToViewportHeight, listOfSheduled_ScreenspaceShape_3Dpos_cam[i].zRotationDegCC, listOfSheduled_ScreenspaceShape_3Dpos_cam[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspaceShape_3Dpos_cam[i].text, listOfSheduled_ScreenspaceShape_3Dpos_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceShape_3Dpos_cam[i].lineStyle, listOfSheduled_ScreenspaceShape_3Dpos_cam[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceShape_3Dpos_cam[i].fillStyle, listOfSheduled_ScreenspaceShape_3Dpos_cam[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspaceShape_3Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceShape_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceShape_2Dpos_cam.Count; i++)
                    {
                        DrawScreenspace.Shape(listOfSheduled_ScreenspaceShape_2Dpos_cam[i].targetCamera, listOfSheduled_ScreenspaceShape_2Dpos_cam[i].centerPosition_in2DViewportSpace, listOfSheduled_ScreenspaceShape_2Dpos_cam[i].shape, listOfSheduled_ScreenspaceShape_2Dpos_cam[i].color, listOfSheduled_ScreenspaceShape_2Dpos_cam[i].width_relToViewportHeight, listOfSheduled_ScreenspaceShape_2Dpos_cam[i].height_relToViewportHeight, listOfSheduled_ScreenspaceShape_2Dpos_cam[i].zRotationDegCC, listOfSheduled_ScreenspaceShape_2Dpos_cam[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspaceShape_2Dpos_cam[i].text, listOfSheduled_ScreenspaceShape_2Dpos_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceShape_2Dpos_cam[i].lineStyle, listOfSheduled_ScreenspaceShape_2Dpos_cam[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceShape_2Dpos_cam[i].fillStyle, listOfSheduled_ScreenspaceShape_2Dpos_cam[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspaceShape_2Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceShape_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceRectangle.Count; i++)
                    {
                        DrawScreenspace.Rectangle(listOfSheduled_ScreenspaceRectangle[i].targetCamera, listOfSheduled_ScreenspaceRectangle[i].lowLeftCorner, listOfSheduled_ScreenspaceRectangle[i].width_relToScreenWidth, listOfSheduled_ScreenspaceRectangle[i].height_relToScreenHeight, listOfSheduled_ScreenspaceRectangle[i].color, listOfSheduled_ScreenspaceRectangle[i].shape, listOfSheduled_ScreenspaceRectangle[i].linesWidth_relToScreenHeight, listOfSheduled_ScreenspaceRectangle[i].text, listOfSheduled_ScreenspaceRectangle[i].lineStyle, listOfSheduled_ScreenspaceRectangle[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceRectangle[i].fillStyle, listOfSheduled_ScreenspaceRectangle[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceRectangle.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceBox_rect_cam.Count; i++)
                    {
                        DrawScreenspace.Box(listOfSheduled_ScreenspaceBox_rect_cam[i].targetCamera, listOfSheduled_ScreenspaceBox_rect_cam[i].rect, listOfSheduled_ScreenspaceBox_rect_cam[i].color, listOfSheduled_ScreenspaceBox_rect_cam[i].zRotationDegCC, listOfSheduled_ScreenspaceBox_rect_cam[i].shape, listOfSheduled_ScreenspaceBox_rect_cam[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspaceBox_rect_cam[i].text, listOfSheduled_ScreenspaceBox_rect_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceBox_rect_cam[i].lineStyle, listOfSheduled_ScreenspaceBox_rect_cam[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceBox_rect_cam[i].fillStyle, listOfSheduled_ScreenspaceBox_rect_cam[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspaceBox_rect_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceBox_rect_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceBox_3Dpos_vec.Count; i++)
                    {
                        DrawScreenspace.Box(listOfSheduled_ScreenspaceBox_3Dpos_vec[i].centerPosition_in3DWorldspace, listOfSheduled_ScreenspaceBox_3Dpos_vec[i].size_relToViewportHeight, listOfSheduled_ScreenspaceBox_3Dpos_vec[i].color, listOfSheduled_ScreenspaceBox_3Dpos_vec[i].zRotationDegCC, listOfSheduled_ScreenspaceBox_3Dpos_vec[i].shape, listOfSheduled_ScreenspaceBox_3Dpos_vec[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspaceBox_3Dpos_vec[i].text, listOfSheduled_ScreenspaceBox_3Dpos_vec[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceBox_3Dpos_vec[i].lineStyle, listOfSheduled_ScreenspaceBox_3Dpos_vec[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceBox_3Dpos_vec[i].fillStyle, listOfSheduled_ScreenspaceBox_3Dpos_vec[i].forceSizeInterpretationToWarpedViewportSpace, listOfSheduled_ScreenspaceBox_3Dpos_vec[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspaceBox_3Dpos_vec[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceBox_3Dpos_vec.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceBox_3Dpos_vec_cam.Count; i++)
                    {
                        DrawScreenspace.Box(listOfSheduled_ScreenspaceBox_3Dpos_vec_cam[i].targetCamera, listOfSheduled_ScreenspaceBox_3Dpos_vec_cam[i].centerPosition_in3DWorldspace, listOfSheduled_ScreenspaceBox_3Dpos_vec_cam[i].size_relToViewportHeight, listOfSheduled_ScreenspaceBox_3Dpos_vec_cam[i].color, listOfSheduled_ScreenspaceBox_3Dpos_vec_cam[i].zRotationDegCC, listOfSheduled_ScreenspaceBox_3Dpos_vec_cam[i].shape, listOfSheduled_ScreenspaceBox_3Dpos_vec_cam[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspaceBox_3Dpos_vec_cam[i].text, listOfSheduled_ScreenspaceBox_3Dpos_vec_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceBox_3Dpos_vec_cam[i].lineStyle, listOfSheduled_ScreenspaceBox_3Dpos_vec_cam[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceBox_3Dpos_vec_cam[i].fillStyle, listOfSheduled_ScreenspaceBox_3Dpos_vec_cam[i].forceSizeInterpretationToWarpedViewportSpace, listOfSheduled_ScreenspaceBox_3Dpos_vec_cam[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspaceBox_3Dpos_vec_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceBox_3Dpos_vec_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceBox_2Dpos_vec_cam.Count; i++)
                    {
                        DrawScreenspace.Box(listOfSheduled_ScreenspaceBox_2Dpos_vec_cam[i].targetCamera, listOfSheduled_ScreenspaceBox_2Dpos_vec_cam[i].centerPosition_in2DViewportSpace, listOfSheduled_ScreenspaceBox_2Dpos_vec_cam[i].size_relToViewportHeight, listOfSheduled_ScreenspaceBox_2Dpos_vec_cam[i].color, listOfSheduled_ScreenspaceBox_2Dpos_vec_cam[i].zRotationDegCC, listOfSheduled_ScreenspaceBox_2Dpos_vec_cam[i].shape, listOfSheduled_ScreenspaceBox_2Dpos_vec_cam[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspaceBox_2Dpos_vec_cam[i].text, listOfSheduled_ScreenspaceBox_2Dpos_vec_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceBox_2Dpos_vec_cam[i].lineStyle, listOfSheduled_ScreenspaceBox_2Dpos_vec_cam[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceBox_2Dpos_vec_cam[i].fillStyle, listOfSheduled_ScreenspaceBox_2Dpos_vec_cam[i].forceSizeInterpretationToWarpedViewportSpace, listOfSheduled_ScreenspaceBox_2Dpos_vec_cam[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspaceBox_2Dpos_vec_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceBox_2Dpos_vec_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceCircle_rect_cam.Count; i++)
                    {
                        DrawScreenspace.Circle(listOfSheduled_ScreenspaceCircle_rect_cam[i].targetCamera, listOfSheduled_ScreenspaceCircle_rect_cam[i].rect, listOfSheduled_ScreenspaceCircle_rect_cam[i].color, listOfSheduled_ScreenspaceCircle_rect_cam[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspaceCircle_rect_cam[i].text, listOfSheduled_ScreenspaceCircle_rect_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceCircle_rect_cam[i].lineStyle, listOfSheduled_ScreenspaceCircle_rect_cam[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceCircle_rect_cam[i].fillStyle, listOfSheduled_ScreenspaceCircle_rect_cam[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspaceCircle_rect_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceCircle_rect_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceCircle_3Dpos_vecRad.Count; i++)
                    {
                        DrawScreenspace.Circle(listOfSheduled_ScreenspaceCircle_3Dpos_vecRad[i].centerPosition_in3DWorldspace, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad[i].radius_relToViewportHeight, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad[i].color, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad[i].text, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad[i].lineStyle, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad[i].fillStyle, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceCircle_3Dpos_vecRad.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceCircle_3Dpos_vecRad_cam.Count; i++)
                    {
                        DrawScreenspace.Circle(listOfSheduled_ScreenspaceCircle_3Dpos_vecRad_cam[i].targetCamera, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad_cam[i].centerPosition_in3DWorldspace, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad_cam[i].radius_relToViewportHeight, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad_cam[i].color, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad_cam[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad_cam[i].text, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad_cam[i].lineStyle, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad_cam[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad_cam[i].fillStyle, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad_cam[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspaceCircle_3Dpos_vecRad_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceCircle_3Dpos_vecRad_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceCircle_2Dpos_vecRad_cam.Count; i++)
                    {
                        DrawScreenspace.Circle(listOfSheduled_ScreenspaceCircle_2Dpos_vecRad_cam[i].targetCamera, listOfSheduled_ScreenspaceCircle_2Dpos_vecRad_cam[i].centerPosition_in2DViewportSpace, listOfSheduled_ScreenspaceCircle_2Dpos_vecRad_cam[i].radius_relToViewportHeight, listOfSheduled_ScreenspaceCircle_2Dpos_vecRad_cam[i].color, listOfSheduled_ScreenspaceCircle_2Dpos_vecRad_cam[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspaceCircle_2Dpos_vecRad_cam[i].text, listOfSheduled_ScreenspaceCircle_2Dpos_vecRad_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceCircle_2Dpos_vecRad_cam[i].lineStyle, listOfSheduled_ScreenspaceCircle_2Dpos_vecRad_cam[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceCircle_2Dpos_vecRad_cam[i].fillStyle, listOfSheduled_ScreenspaceCircle_2Dpos_vecRad_cam[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspaceCircle_2Dpos_vecRad_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceCircle_2Dpos_vecRad_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos.Count; i++)
                    {
                        DrawScreenspace.Capsule(listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos[i].posOfCircle1_in3DWorldspace, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos[i].posOfCircle2_in3DWorldspace, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos[i].radius_relToViewportHeight, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos[i].color, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos[i].text, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos[i].lineStyle, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos[i].fillStyle, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam.Count; i++)
                    {
                        DrawScreenspace.Capsule(listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam[i].targetCamera, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam[i].posOfCircle1_in3DWorldspace, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam[i].posOfCircle2_in3DWorldspace, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam[i].radius_relToViewportHeight, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam[i].color, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam[i].text, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam[i].lineStyle, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam[i].fillStyle, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceCapsule_3Dpos_vecC1C2Pos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam.Count; i++)
                    {
                        DrawScreenspace.Capsule(listOfSheduled_ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam[i].targetCamera, listOfSheduled_ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam[i].posOfCircle1_in2DViewportSpace, listOfSheduled_ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam[i].posOfCircle2_in2DViewportSpace, listOfSheduled_ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam[i].radius_relToViewportHeight, listOfSheduled_ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam[i].color, listOfSheduled_ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam[i].text, listOfSheduled_ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam[i].lineStyle, listOfSheduled_ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam[i].fillStyle, listOfSheduled_ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceCapsule_2Dpos_vecC1C2Pos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceCapsule_rect_cam.Count; i++)
                    {
                        DrawScreenspace.Capsule(listOfSheduled_ScreenspaceCapsule_rect_cam[i].targetCamera, listOfSheduled_ScreenspaceCapsule_rect_cam[i].rect, listOfSheduled_ScreenspaceCapsule_rect_cam[i].color, listOfSheduled_ScreenspaceCapsule_rect_cam[i].capsuleDirection, listOfSheduled_ScreenspaceCapsule_rect_cam[i].zRotationDegCC, listOfSheduled_ScreenspaceCapsule_rect_cam[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspaceCapsule_rect_cam[i].text, listOfSheduled_ScreenspaceCapsule_rect_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceCapsule_rect_cam[i].lineStyle, listOfSheduled_ScreenspaceCapsule_rect_cam[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceCapsule_rect_cam[i].fillStyle, listOfSheduled_ScreenspaceCapsule_rect_cam[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspaceCapsule_rect_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceCapsule_rect_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize.Count; i++)
                    {
                        DrawScreenspace.Capsule(listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize[i].centerPosition_in3DWorldspace, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize[i].size_relToViewportHeight, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize[i].color, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize[i].capsuleDirection, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize[i].zRotationDegCC, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize[i].text, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize[i].lineStyle, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize[i].fillStyle, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize[i].forceSizeInterpretationToWarpedViewportSpace, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam.Count; i++)
                    {
                        DrawScreenspace.Capsule(listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam[i].targetCamera, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam[i].centerPosition_in3DWorldspace, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam[i].size_relToViewportHeight, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam[i].color, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam[i].capsuleDirection, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam[i].zRotationDegCC, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam[i].text, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam[i].lineStyle, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam[i].fillStyle, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam[i].forceSizeInterpretationToWarpedViewportSpace, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceCapsule_3Dpos_vecPosSize_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam.Count; i++)
                    {
                        DrawScreenspace.Capsule(listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam[i].targetCamera, listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam[i].centerPosition_in2DViewportSpace, listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam[i].size_relToViewportHeight, listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam[i].color, listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam[i].capsuleDirection, listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam[i].zRotationDegCC, listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam[i].text, listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam[i].lineStyle, listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam[i].fillStyle, listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam[i].forceSizeInterpretationToWarpedViewportSpace, listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceCapsule_2Dpos_vecPosSize_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspacePointArray.Count; i++)
                    {
                        DrawScreenspace.PointArray(listOfSheduled_ScreenspacePointArray[i].targetCamera, listOfSheduled_ScreenspacePointArray[i].points, listOfSheduled_ScreenspacePointArray[i].color, listOfSheduled_ScreenspacePointArray[i].sizeOfMarkingCross_relToViewportHeight, listOfSheduled_ScreenspacePointArray[i].markingCrossLinesWidth_relToViewportHeight, listOfSheduled_ScreenspacePointArray[i].drawCoordsAsText, listOfSheduled_ScreenspacePointArray[i].durationInSec);
                    }
                    listOfSheduled_ScreenspacePointArray.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspacePointList.Count; i++)
                    {
                        DrawScreenspace.PointList(listOfSheduled_ScreenspacePointList[i].targetCamera, listOfSheduled_ScreenspacePointList[i].points, listOfSheduled_ScreenspacePointList[i].color, listOfSheduled_ScreenspacePointList[i].sizeOfMarkingCross_relToViewportHeight, listOfSheduled_ScreenspacePointList[i].markingCrossLinesWidth_relToViewportHeight, listOfSheduled_ScreenspacePointList[i].drawCoordsAsText, listOfSheduled_ScreenspacePointList[i].durationInSec);
                    }
                    listOfSheduled_ScreenspacePointList.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspacePoint.Count; i++)
                    {
                        DrawScreenspace.Point(listOfSheduled_ScreenspacePoint[i].position, listOfSheduled_ScreenspacePoint[i].color, listOfSheduled_ScreenspacePoint[i].sizeOfMarkingCross_relToViewportHeight, listOfSheduled_ScreenspacePoint[i].zRotationDegCC, listOfSheduled_ScreenspacePoint[i].markingCrossLinesWidth_relToViewportHeight, listOfSheduled_ScreenspacePoint[i].drawPointerIfOffscreen, listOfSheduled_ScreenspacePoint[i].text, listOfSheduled_ScreenspacePoint[i].pointer_as_textAttachStyle, listOfSheduled_ScreenspacePoint[i].drawCoordsAsText, listOfSheduled_ScreenspacePoint[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspacePoint[i].durationInSec);
                    }
                    listOfSheduled_ScreenspacePoint.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspacePoint_prioText_cam.Count; i++)
                    {
                        DrawScreenspace.Point(listOfSheduled_ScreenspacePoint_prioText_cam[i].targetCamera, listOfSheduled_ScreenspacePoint_prioText_cam[i].position, listOfSheduled_ScreenspacePoint_prioText_cam[i].text, listOfSheduled_ScreenspacePoint_prioText_cam[i].color, listOfSheduled_ScreenspacePoint_prioText_cam[i].sizeOfMarkingCross_relToViewportHeight, listOfSheduled_ScreenspacePoint_prioText_cam[i].markingCrossLinesWidth_relToViewportHeight, listOfSheduled_ScreenspacePoint_prioText_cam[i].zRotationDegCC, listOfSheduled_ScreenspacePoint_prioText_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspacePoint_prioText_cam[i].pointer_as_textAttachStyle, listOfSheduled_ScreenspacePoint_prioText_cam[i].drawCoordsAsText, listOfSheduled_ScreenspacePoint_prioText_cam[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspacePoint_prioText_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspacePoint_prioText_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspacePointTag_3Dpos.Count; i++)
                    {
                        DrawScreenspace.PointTag(listOfSheduled_ScreenspacePointTag_3Dpos[i].position_in3DWorldspace, listOfSheduled_ScreenspacePointTag_3Dpos[i].text, listOfSheduled_ScreenspacePointTag_3Dpos[i].titleText, listOfSheduled_ScreenspacePointTag_3Dpos[i].color, listOfSheduled_ScreenspacePointTag_3Dpos[i].drawPointerIfOffscreen, listOfSheduled_ScreenspacePointTag_3Dpos[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspacePointTag_3Dpos[i].size_asTextOffsetDistance_relToViewportHeight, listOfSheduled_ScreenspacePointTag_3Dpos[i].textOffsetDirection, listOfSheduled_ScreenspacePointTag_3Dpos[i].textSizeScaleFactor, listOfSheduled_ScreenspacePointTag_3Dpos[i].skipConeDrawing, listOfSheduled_ScreenspacePointTag_3Dpos[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspacePointTag_3Dpos[i].durationInSec, listOfSheduled_ScreenspacePointTag_3Dpos[i].customTowardsPoint_ofDefaultTextOffsetDirection);
                    }
                    listOfSheduled_ScreenspacePointTag_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspacePointTag_3Dpos_cam.Count; i++)
                    {
                        DrawScreenspace.PointTag(listOfSheduled_ScreenspacePointTag_3Dpos_cam[i].targetCamera, listOfSheduled_ScreenspacePointTag_3Dpos_cam[i].position_in3DWorldspace, listOfSheduled_ScreenspacePointTag_3Dpos_cam[i].text, listOfSheduled_ScreenspacePointTag_3Dpos_cam[i].titleText, listOfSheduled_ScreenspacePointTag_3Dpos_cam[i].color, listOfSheduled_ScreenspacePointTag_3Dpos_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspacePointTag_3Dpos_cam[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspacePointTag_3Dpos_cam[i].size_asTextOffsetDistance_relToViewportHeight, listOfSheduled_ScreenspacePointTag_3Dpos_cam[i].textOffsetDirection, listOfSheduled_ScreenspacePointTag_3Dpos_cam[i].textSizeScaleFactor, listOfSheduled_ScreenspacePointTag_3Dpos_cam[i].skipConeDrawing, listOfSheduled_ScreenspacePointTag_3Dpos_cam[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspacePointTag_3Dpos_cam[i].durationInSec, listOfSheduled_ScreenspacePointTag_3Dpos_cam[i].customTowardsPoint_ofDefaultTextOffsetDirection);
                    }
                    listOfSheduled_ScreenspacePointTag_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspacePointTag_2Dpos_cam.Count; i++)
                    {
                        DrawScreenspace.PointTag(listOfSheduled_ScreenspacePointTag_2Dpos_cam[i].targetCamera, listOfSheduled_ScreenspacePointTag_2Dpos_cam[i].position_in2DViewportSpace, listOfSheduled_ScreenspacePointTag_2Dpos_cam[i].text, listOfSheduled_ScreenspacePointTag_2Dpos_cam[i].titleText, listOfSheduled_ScreenspacePointTag_2Dpos_cam[i].color, listOfSheduled_ScreenspacePointTag_2Dpos_cam[i].drawPointerIfOffscreen, listOfSheduled_ScreenspacePointTag_2Dpos_cam[i].linesWidth_relToViewportHeight, listOfSheduled_ScreenspacePointTag_2Dpos_cam[i].size_asTextOffsetDistance_relToViewportHeight, listOfSheduled_ScreenspacePointTag_2Dpos_cam[i].textOffsetDirection, listOfSheduled_ScreenspacePointTag_2Dpos_cam[i].textSizeScaleFactor, listOfSheduled_ScreenspacePointTag_2Dpos_cam[i].skipConeDrawing, listOfSheduled_ScreenspacePointTag_2Dpos_cam[i].addTextForOutsideDistance_toOffscreenPointer, listOfSheduled_ScreenspacePointTag_2Dpos_cam[i].durationInSec, listOfSheduled_ScreenspacePointTag_2Dpos_cam[i].customTowardsPoint_ofDefaultTextOffsetDirection);
                    }
                    listOfSheduled_ScreenspacePointTag_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceVectorFrom.Count; i++)
                    {
                        DrawScreenspace.VectorFrom(listOfSheduled_ScreenspaceVectorFrom[i].targetCamera, listOfSheduled_ScreenspaceVectorFrom[i].vectorStartPos, listOfSheduled_ScreenspaceVectorFrom[i].vector, listOfSheduled_ScreenspaceVectorFrom[i].color, listOfSheduled_ScreenspaceVectorFrom[i].lineWidth_relToViewportHeight, listOfSheduled_ScreenspaceVectorFrom[i].text, listOfSheduled_ScreenspaceVectorFrom[i].interpretVectorAsUnwarped, listOfSheduled_ScreenspaceVectorFrom[i].coneLength_relToViewportHeight, listOfSheduled_ScreenspaceVectorFrom[i].pointerAtBothSides, listOfSheduled_ScreenspaceVectorFrom[i].writeComponentValuesAsText, listOfSheduled_ScreenspaceVectorFrom[i].endPlatesSize_relToViewportHeight, listOfSheduled_ScreenspaceVectorFrom[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceVectorFrom.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceVectorTo.Count; i++)
                    {
                        DrawScreenspace.VectorTo(listOfSheduled_ScreenspaceVectorTo[i].targetCamera, listOfSheduled_ScreenspaceVectorTo[i].vector, listOfSheduled_ScreenspaceVectorTo[i].vectorEndPos, listOfSheduled_ScreenspaceVectorTo[i].color, listOfSheduled_ScreenspaceVectorTo[i].lineWidth_relToViewportHeight, listOfSheduled_ScreenspaceVectorTo[i].text, listOfSheduled_ScreenspaceVectorTo[i].interpretVectorAsUnwarped, listOfSheduled_ScreenspaceVectorTo[i].coneLength_relToViewportHeight, listOfSheduled_ScreenspaceVectorTo[i].pointerAtBothSides, listOfSheduled_ScreenspaceVectorTo[i].writeComponentValuesAsText, listOfSheduled_ScreenspaceVectorTo[i].endPlatesSize_relToViewportHeight, listOfSheduled_ScreenspaceVectorTo[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceVectorTo.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceVectorCircled_angleToAngle_cam.Count; i++)
                    {
                        DrawScreenspace.VectorCircled(listOfSheduled_ScreenspaceVectorCircled_angleToAngle_cam[i].targetCamera, listOfSheduled_ScreenspaceVectorCircled_angleToAngle_cam[i].circleCenter, listOfSheduled_ScreenspaceVectorCircled_angleToAngle_cam[i].startAngleDegCC_relativeToUp, listOfSheduled_ScreenspaceVectorCircled_angleToAngle_cam[i].endAngleDegCC_relativeToUp, listOfSheduled_ScreenspaceVectorCircled_angleToAngle_cam[i].radius_relToViewportHeight, listOfSheduled_ScreenspaceVectorCircled_angleToAngle_cam[i].color, listOfSheduled_ScreenspaceVectorCircled_angleToAngle_cam[i].lineWidth_relToViewportHeight, listOfSheduled_ScreenspaceVectorCircled_angleToAngle_cam[i].text, listOfSheduled_ScreenspaceVectorCircled_angleToAngle_cam[i].coneLength_relToViewportHeight, listOfSheduled_ScreenspaceVectorCircled_angleToAngle_cam[i].skipFallbackDisplayOfZeroAngles, listOfSheduled_ScreenspaceVectorCircled_angleToAngle_cam[i].pointerAtBothSides, listOfSheduled_ScreenspaceVectorCircled_angleToAngle_cam[i].minAngleDeg_withoutTextLineBreak, listOfSheduled_ScreenspaceVectorCircled_angleToAngle_cam[i].textAnchor, listOfSheduled_ScreenspaceVectorCircled_angleToAngle_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceVectorCircled_angleToAngle_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceVectorCircled_angleFromStartPos_cam.Count; i++)
                    {
                        DrawScreenspace.VectorCircled(listOfSheduled_ScreenspaceVectorCircled_angleFromStartPos_cam[i].targetCamera, listOfSheduled_ScreenspaceVectorCircled_angleFromStartPos_cam[i].startPos, listOfSheduled_ScreenspaceVectorCircled_angleFromStartPos_cam[i].circleCenter, listOfSheduled_ScreenspaceVectorCircled_angleFromStartPos_cam[i].turnAngleDegCC, listOfSheduled_ScreenspaceVectorCircled_angleFromStartPos_cam[i].color, listOfSheduled_ScreenspaceVectorCircled_angleFromStartPos_cam[i].lineWidth_relToViewportHeight, listOfSheduled_ScreenspaceVectorCircled_angleFromStartPos_cam[i].text, listOfSheduled_ScreenspaceVectorCircled_angleFromStartPos_cam[i].coneLength_relToViewportHeight, listOfSheduled_ScreenspaceVectorCircled_angleFromStartPos_cam[i].skipFallbackDisplayOfZeroAngles, listOfSheduled_ScreenspaceVectorCircled_angleFromStartPos_cam[i].pointerAtBothSides, listOfSheduled_ScreenspaceVectorCircled_angleFromStartPos_cam[i].minAngleDeg_withoutTextLineBreak, listOfSheduled_ScreenspaceVectorCircled_angleFromStartPos_cam[i].textAnchor, listOfSheduled_ScreenspaceVectorCircled_angleFromStartPos_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceVectorCircled_angleFromStartPos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceIcon_3Dpos.Count; i++)
                    {
                        DrawScreenspace.Icon(listOfSheduled_ScreenspaceIcon_3Dpos[i].position_in3DWorldspace, listOfSheduled_ScreenspaceIcon_3Dpos[i].icon, listOfSheduled_ScreenspaceIcon_3Dpos[i].color, listOfSheduled_ScreenspaceIcon_3Dpos[i].size_relToViewportHeight, listOfSheduled_ScreenspaceIcon_3Dpos[i].text, listOfSheduled_ScreenspaceIcon_3Dpos[i].zRotationDegCC, listOfSheduled_ScreenspaceIcon_3Dpos[i].strokeWidth_relToViewportHeight, listOfSheduled_ScreenspaceIcon_3Dpos[i].displayPointerIfOffscreen, listOfSheduled_ScreenspaceIcon_3Dpos[i].mirrorHorizontally, listOfSheduled_ScreenspaceIcon_3Dpos[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceIcon_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceIcon_3Dpos_cam.Count; i++)
                    {
                        DrawScreenspace.Icon(listOfSheduled_ScreenspaceIcon_3Dpos_cam[i].targetCamera, listOfSheduled_ScreenspaceIcon_3Dpos_cam[i].position_in3DWorldspace, listOfSheduled_ScreenspaceIcon_3Dpos_cam[i].icon, listOfSheduled_ScreenspaceIcon_3Dpos_cam[i].color, listOfSheduled_ScreenspaceIcon_3Dpos_cam[i].size_relToViewportHeight, listOfSheduled_ScreenspaceIcon_3Dpos_cam[i].text, listOfSheduled_ScreenspaceIcon_3Dpos_cam[i].zRotationDegCC, listOfSheduled_ScreenspaceIcon_3Dpos_cam[i].strokeWidth_relToViewportHeight, listOfSheduled_ScreenspaceIcon_3Dpos_cam[i].displayPointerIfOffscreen, listOfSheduled_ScreenspaceIcon_3Dpos_cam[i].mirrorHorizontally, listOfSheduled_ScreenspaceIcon_3Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceIcon_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceIcon_2Dpos_cam.Count; i++)
                    {
                        DrawScreenspace.Icon(listOfSheduled_ScreenspaceIcon_2Dpos_cam[i].targetCamera, listOfSheduled_ScreenspaceIcon_2Dpos_cam[i].position_in2DViewportSpace, listOfSheduled_ScreenspaceIcon_2Dpos_cam[i].icon, listOfSheduled_ScreenspaceIcon_2Dpos_cam[i].color, listOfSheduled_ScreenspaceIcon_2Dpos_cam[i].size_relToViewportHeight, listOfSheduled_ScreenspaceIcon_2Dpos_cam[i].text, listOfSheduled_ScreenspaceIcon_2Dpos_cam[i].zRotationDegCC, listOfSheduled_ScreenspaceIcon_2Dpos_cam[i].strokeWidth_relToViewportHeight, listOfSheduled_ScreenspaceIcon_2Dpos_cam[i].displayPointerIfOffscreen, listOfSheduled_ScreenspaceIcon_2Dpos_cam[i].mirrorHorizontally, listOfSheduled_ScreenspaceIcon_2Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceIcon_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceDot_3Dpos.Count; i++)
                    {
                        DrawScreenspace.Dot(listOfSheduled_ScreenspaceDot_3Dpos[i].position_in3DWorldspace, listOfSheduled_ScreenspaceDot_3Dpos[i].radius_relToViewportHeight, listOfSheduled_ScreenspaceDot_3Dpos[i].color, listOfSheduled_ScreenspaceDot_3Dpos[i].text, listOfSheduled_ScreenspaceDot_3Dpos[i].density, listOfSheduled_ScreenspaceDot_3Dpos[i].displayPointerIfOffscreen, listOfSheduled_ScreenspaceDot_3Dpos[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceDot_3Dpos.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceDot_3Dpos_cam.Count; i++)
                    {
                        DrawScreenspace.Dot(listOfSheduled_ScreenspaceDot_3Dpos_cam[i].targetCamera, listOfSheduled_ScreenspaceDot_3Dpos_cam[i].position_in3DWorldspace, listOfSheduled_ScreenspaceDot_3Dpos_cam[i].radius_relToViewportHeight, listOfSheduled_ScreenspaceDot_3Dpos_cam[i].color, listOfSheduled_ScreenspaceDot_3Dpos_cam[i].text, listOfSheduled_ScreenspaceDot_3Dpos_cam[i].density, listOfSheduled_ScreenspaceDot_3Dpos_cam[i].displayPointerIfOffscreen, listOfSheduled_ScreenspaceDot_3Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceDot_3Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceDot_2Dpos_cam.Count; i++)
                    {
                        DrawScreenspace.Dot(listOfSheduled_ScreenspaceDot_2Dpos_cam[i].targetCamera, listOfSheduled_ScreenspaceDot_2Dpos_cam[i].position_in2DViewportSpace, listOfSheduled_ScreenspaceDot_2Dpos_cam[i].radius_relToViewportHeight, listOfSheduled_ScreenspaceDot_2Dpos_cam[i].color, listOfSheduled_ScreenspaceDot_2Dpos_cam[i].text, listOfSheduled_ScreenspaceDot_2Dpos_cam[i].density, listOfSheduled_ScreenspaceDot_2Dpos_cam[i].displayPointerIfOffscreen, listOfSheduled_ScreenspaceDot_2Dpos_cam[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceDot_2Dpos_cam.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceMovingArrowsRay.Count; i++)
                    {
                        DrawScreenspace.MovingArrowsRay(listOfSheduled_ScreenspaceMovingArrowsRay[i].targetCamera, listOfSheduled_ScreenspaceMovingArrowsRay[i].start, listOfSheduled_ScreenspaceMovingArrowsRay[i].direction, listOfSheduled_ScreenspaceMovingArrowsRay[i].color, listOfSheduled_ScreenspaceMovingArrowsRay[i].lineWidth_relToViewportHeight, listOfSheduled_ScreenspaceMovingArrowsRay[i].distanceBetweenArrows_relToViewportHeight, listOfSheduled_ScreenspaceMovingArrowsRay[i].lengthOfArrows_relToViewportHeight, listOfSheduled_ScreenspaceMovingArrowsRay[i].text, listOfSheduled_ScreenspaceMovingArrowsRay[i].animationSpeed, listOfSheduled_ScreenspaceMovingArrowsRay[i].backwardAnimationFlipsArrowDirection, listOfSheduled_ScreenspaceMovingArrowsRay[i].interpretDirectionAsUnwarped, listOfSheduled_ScreenspaceMovingArrowsRay[i].endPlatesSize_relToViewportHeight, listOfSheduled_ScreenspaceMovingArrowsRay[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceMovingArrowsRay.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceMovingArrowsLine.Count; i++)
                    {
                        DrawScreenspace.MovingArrowsLine(listOfSheduled_ScreenspaceMovingArrowsLine[i].targetCamera, listOfSheduled_ScreenspaceMovingArrowsLine[i].start, listOfSheduled_ScreenspaceMovingArrowsLine[i].end, listOfSheduled_ScreenspaceMovingArrowsLine[i].color, listOfSheduled_ScreenspaceMovingArrowsLine[i].lineWidth_relToViewportHeight, listOfSheduled_ScreenspaceMovingArrowsLine[i].distanceBetweenArrows_relToViewportHeight, listOfSheduled_ScreenspaceMovingArrowsLine[i].lengthOfArrows_relToViewportHeight, listOfSheduled_ScreenspaceMovingArrowsLine[i].text, listOfSheduled_ScreenspaceMovingArrowsLine[i].animationSpeed, listOfSheduled_ScreenspaceMovingArrowsLine[i].backwardAnimationFlipsArrowDirection, listOfSheduled_ScreenspaceMovingArrowsLine[i].endPlatesSize_relToViewportHeight, listOfSheduled_ScreenspaceMovingArrowsLine[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceMovingArrowsLine.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceRayWithAlternatingColors.Count; i++)
                    {
                        DrawScreenspace.RayWithAlternatingColors(listOfSheduled_ScreenspaceRayWithAlternatingColors[i].targetCamera, listOfSheduled_ScreenspaceRayWithAlternatingColors[i].start, listOfSheduled_ScreenspaceRayWithAlternatingColors[i].direction, listOfSheduled_ScreenspaceRayWithAlternatingColors[i].color1, listOfSheduled_ScreenspaceRayWithAlternatingColors[i].color2, listOfSheduled_ScreenspaceRayWithAlternatingColors[i].lineWidth_relToViewportHeight, listOfSheduled_ScreenspaceRayWithAlternatingColors[i].lengthOfStripes_relToViewportHeight, listOfSheduled_ScreenspaceRayWithAlternatingColors[i].text, listOfSheduled_ScreenspaceRayWithAlternatingColors[i].interpretDirectionAsUnwarped, listOfSheduled_ScreenspaceRayWithAlternatingColors[i].animationSpeed, listOfSheduled_ScreenspaceRayWithAlternatingColors[i].endPlatesSize_relToViewportHeight, listOfSheduled_ScreenspaceRayWithAlternatingColors[i].alphaFadeOutLength_0to1, listOfSheduled_ScreenspaceRayWithAlternatingColors[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceRayWithAlternatingColors.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceLineWithAlternatingColors.Count; i++)
                    {
                        DrawScreenspace.LineWithAlternatingColors(listOfSheduled_ScreenspaceLineWithAlternatingColors[i].targetCamera, listOfSheduled_ScreenspaceLineWithAlternatingColors[i].start, listOfSheduled_ScreenspaceLineWithAlternatingColors[i].end, listOfSheduled_ScreenspaceLineWithAlternatingColors[i].color1, listOfSheduled_ScreenspaceLineWithAlternatingColors[i].color2, listOfSheduled_ScreenspaceLineWithAlternatingColors[i].lineWidth_relToViewportHeight, listOfSheduled_ScreenspaceLineWithAlternatingColors[i].lengthOfStripes_relToViewportHeight, listOfSheduled_ScreenspaceLineWithAlternatingColors[i].text, listOfSheduled_ScreenspaceLineWithAlternatingColors[i].animationSpeed, listOfSheduled_ScreenspaceLineWithAlternatingColors[i].endPlatesSize_relToViewportHeight, listOfSheduled_ScreenspaceLineWithAlternatingColors[i].alphaFadeOutLength_0to1, listOfSheduled_ScreenspaceLineWithAlternatingColors[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceLineWithAlternatingColors.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceBlinkingRay.Count; i++)
                    {
                        DrawScreenspace.BlinkingRay(listOfSheduled_ScreenspaceBlinkingRay[i].targetCamera, listOfSheduled_ScreenspaceBlinkingRay[i].start, listOfSheduled_ScreenspaceBlinkingRay[i].direction, listOfSheduled_ScreenspaceBlinkingRay[i].primaryColor, listOfSheduled_ScreenspaceBlinkingRay[i].blinkDurationInSec, listOfSheduled_ScreenspaceBlinkingRay[i].width_relToViewportHeight, listOfSheduled_ScreenspaceBlinkingRay[i].text, listOfSheduled_ScreenspaceBlinkingRay[i].interpretDirectionAsUnwarped, listOfSheduled_ScreenspaceBlinkingRay[i].style, listOfSheduled_ScreenspaceBlinkingRay[i].blinkColor, listOfSheduled_ScreenspaceBlinkingRay[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceBlinkingRay[i].endPlatesSize_relToViewportHeight, listOfSheduled_ScreenspaceBlinkingRay[i].alphaFadeOutLength_0to1, listOfSheduled_ScreenspaceBlinkingRay[i].enlargeSmallTextToThisMinRelTextSize, listOfSheduled_ScreenspaceBlinkingRay[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceBlinkingRay.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceBlinkingLine.Count; i++)
                    {
                        DrawScreenspace.BlinkingLine(listOfSheduled_ScreenspaceBlinkingLine[i].targetCamera, listOfSheduled_ScreenspaceBlinkingLine[i].start, listOfSheduled_ScreenspaceBlinkingLine[i].end, listOfSheduled_ScreenspaceBlinkingLine[i].primaryColor, listOfSheduled_ScreenspaceBlinkingLine[i].blinkDurationInSec, listOfSheduled_ScreenspaceBlinkingLine[i].width_relToViewportHeight, listOfSheduled_ScreenspaceBlinkingLine[i].text, listOfSheduled_ScreenspaceBlinkingLine[i].style, listOfSheduled_ScreenspaceBlinkingLine[i].blinkColor, listOfSheduled_ScreenspaceBlinkingLine[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceBlinkingLine[i].endPlatesSize_relToViewportHeight, listOfSheduled_ScreenspaceBlinkingLine[i].alphaFadeOutLength_0to1, listOfSheduled_ScreenspaceBlinkingLine[i].enlargeSmallTextToThisMinRelTextSize, listOfSheduled_ScreenspaceBlinkingLine[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceBlinkingLine.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceRayUnderTension.Count; i++)
                    {
                        DrawScreenspace.RayUnderTension(listOfSheduled_ScreenspaceRayUnderTension[i].targetCamera, listOfSheduled_ScreenspaceRayUnderTension[i].start, listOfSheduled_ScreenspaceRayUnderTension[i].direction, listOfSheduled_ScreenspaceRayUnderTension[i].relaxedLength_relToViewportHeight, listOfSheduled_ScreenspaceRayUnderTension[i].relaxedColor, listOfSheduled_ScreenspaceRayUnderTension[i].style, listOfSheduled_ScreenspaceRayUnderTension[i].stretchFactor_forStretchedTensionColor, listOfSheduled_ScreenspaceRayUnderTension[i].color_forStretchedTension, listOfSheduled_ScreenspaceRayUnderTension[i].stretchFactor_forSqueezedTensionColor, listOfSheduled_ScreenspaceRayUnderTension[i].color_forSqueezedTension, listOfSheduled_ScreenspaceRayUnderTension[i].width_relToViewportHeight, listOfSheduled_ScreenspaceRayUnderTension[i].text, listOfSheduled_ScreenspaceRayUnderTension[i].alphaOfReferenceLengthDisplay, listOfSheduled_ScreenspaceRayUnderTension[i].interpretDirectionAsUnwarped, listOfSheduled_ScreenspaceRayUnderTension[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceRayUnderTension[i].endPlatesSize_relToViewportHeight, listOfSheduled_ScreenspaceRayUnderTension[i].enlargeSmallTextToThisMinRelTextSize, listOfSheduled_ScreenspaceRayUnderTension[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceRayUnderTension.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceLineUnderTension.Count; i++)
                    {
                        DrawScreenspace.LineUnderTension(listOfSheduled_ScreenspaceLineUnderTension[i].targetCamera, listOfSheduled_ScreenspaceLineUnderTension[i].start, listOfSheduled_ScreenspaceLineUnderTension[i].end, listOfSheduled_ScreenspaceLineUnderTension[i].relaxedLength_relToViewportHeight, listOfSheduled_ScreenspaceLineUnderTension[i].relaxedColor, listOfSheduled_ScreenspaceLineUnderTension[i].style, listOfSheduled_ScreenspaceLineUnderTension[i].stretchFactor_forStretchedTensionColor, listOfSheduled_ScreenspaceLineUnderTension[i].color_forStretchedTension, listOfSheduled_ScreenspaceLineUnderTension[i].stretchFactor_forSqueezedTensionColor, listOfSheduled_ScreenspaceLineUnderTension[i].color_forSqueezedTension, listOfSheduled_ScreenspaceLineUnderTension[i].width_relToViewportHeight, listOfSheduled_ScreenspaceLineUnderTension[i].text, listOfSheduled_ScreenspaceLineUnderTension[i].alphaOfReferenceLengthDisplay, listOfSheduled_ScreenspaceLineUnderTension[i].stylePatternScaleFactor, listOfSheduled_ScreenspaceLineUnderTension[i].endPlatesSize_relToViewportHeight, listOfSheduled_ScreenspaceLineUnderTension[i].enlargeSmallTextToThisMinRelTextSize, listOfSheduled_ScreenspaceLineUnderTension[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceLineUnderTension.Clear();

                    for (int i = 0; i < listOfSheduled_ScreenspaceVisualizeAutomaticCameraForDrawing.Count; i++)
                    {
                        DrawScreenspace.VisualizeAutomaticCameraForDrawing(listOfSheduled_ScreenspaceVisualizeAutomaticCameraForDrawing[i].visualizeFrustum, listOfSheduled_ScreenspaceVisualizeAutomaticCameraForDrawing[i].logPositionToConsole, listOfSheduled_ScreenspaceVisualizeAutomaticCameraForDrawing[i].color, listOfSheduled_ScreenspaceVisualizeAutomaticCameraForDrawing[i].durationInSec);
                    }
                    listOfSheduled_ScreenspaceVisualizeAutomaticCameraForDrawing.Clear();

                    for (int i = 0; i < listOfSheduled_DrawScreenspaceChart.Count; i++)
                    {
                        listOfSheduled_DrawScreenspaceChart[i].concernedChartDrawing.DrawScreenspace(listOfSheduled_DrawScreenspaceChart[i].targetCamera, listOfSheduled_DrawScreenspaceChart[i].chartWidth_isDefinedRelTo_cameraWidth_notCameraHeight, listOfSheduled_DrawScreenspaceChart[i].durationInSec);
                    }
                    listOfSheduled_DrawScreenspaceChart.Clear();

                    for (int i = 0; i < listOfSheduled_DrawScreenspacePieChart.Count; i++)
                    {
                        listOfSheduled_DrawScreenspacePieChart[i].concernedPieChartDrawing.DrawScreenspace(listOfSheduled_DrawScreenspacePieChart[i].targetCamera, listOfSheduled_DrawScreenspacePieChart[i].chartSize_isDefinedRelTo_cameraWidth_notCameraHeight, listOfSheduled_DrawScreenspacePieChart[i].durationInSec);
                    }
                    listOfSheduled_DrawScreenspacePieChart.Clear();
                }
                atLeastOneScreenspaceLineHasBeenSheduledToLateInsideLateUpdate = false;
            }
            catch { }

            noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem = true;
        }

        //-> The char-specificationsViaVectorPositions should not be "static" (e.g. inside "UtilitiesDXXL_CharsAndIcons"), and are therefore declared here as part of the instanced Manager-Component
        //-> The problem with "static" here would be:
        //---> Static member declarations will automatically get moved to the static constructor of the containing class by the compiler.
        //---> Static constructors get initialized "lazy", that means the static constructor happens unpredictably delayed during the running game, in the moment when the member is used for the first time.
        //---> Moreover the delayed static constructor is not guaranteed to happen inside the main thread, but can also happen in a worker thread.
        //---> There is no controll over the worker threads in the background. This lead to "stack overflow" errors, and "nullReferenceExceptions" as aftereffect.
        //Addendum:
        //-> it was not enough to move the char and symbol definitions here to this class as non static fields.
        //-> obscure errors appeared (freeze on enter playmode, freeze on addDrawerComponent, stack overflow on enter playmode in the DrawXXL_LinesManager.ctor, ...)
        //-> for further details, see: https://forum.unity.com/threads/initializing-many-vector-arrays-in-constructor-causes-stack-overflow.1434982/
        //-> it somehow appears as if the field initializers of the chars are moved to the DrawXXL_LinesManager.ctor and produce some kind of overload there.
        //-> the fix for these errors was "lazy initialization" of the fields. They now get initialized when they are called the first time, but not all at once in the DrawXXL_LinesManager.ctor


        const int maxStrokesPerSymbol = 30;
        public int numberOfStrokes_forCurrUsedChar = 0;
        public int[] numberOfPointsForEachStroke_forCurrUsedChar = new int[maxStrokesPerSymbol];
        public Vector3[][] currPrinted_charDef = new Vector3[maxStrokesPerSymbol][]
        {
             new Vector3[71],
             new Vector3[44],
             new Vector3[44],
             new Vector3[20],
             new Vector3[16],
             new Vector3[16],
             new Vector3[16],
             new Vector3[16],
             new Vector3[16],
             new Vector3[14],
             new Vector3[8],
             new Vector3[8],
             new Vector3[8],
             new Vector3[8],
             new Vector3[8],
             new Vector3[8],
             new Vector3[8],
             new Vector3[8],
             new Vector3[8],
             new Vector3[7],
             new Vector3[7],
             new Vector3[7],
             new Vector3[5],
             new Vector3[5],
             new Vector3[3],
             new Vector3[3],
             new Vector3[3],
             new Vector3[3],
             new Vector3[3],
             new Vector3[3]
        };



        Vector3[][] Char_a() { if (char_a == null) { char_a = new Vector3[][] { new Vector3[] { new Vector2(0.723f, 0.165f), new Vector2(0.539f, 0.035f), new Vector2(0.263f, 0.037f), new Vector2(0.173f, 0.1421f), new Vector2(0.1761f, 0.2576f), new Vector2(0.3387f, 0.3954f), new Vector2(0.5582f, 0.4002f), new Vector2(0.7232f, 0.3654f) }, new Vector3[] { new Vector2(0.25f, 0.641f), new Vector2(0.397f, 0.698f), new Vector2(0.586f, 0.697f), new Vector2(0.724f, 0.579f), new Vector2(0.724f, 0.044f), new Vector2(0.84f, 0.044f) } }; } return char_a; }
        Vector3[][] Char_b() { if (char_b == null) { char_b = new Vector3[][] { new Vector3[] { new Vector2(0.225f, 0.256f), new Vector2(0.458f, 0.035f), new Vector2(0.644f, 0.037f), new Vector2(0.795f, 0.1421f), new Vector2(0.873f, 0.289f), new Vector2(0.875f, 0.438f), new Vector2(0.791f, 0.583f), new Vector2(0.651f, 0.692f), new Vector2(0.465f, 0.692f), new Vector2(0.226f, 0.481f) }, new Vector3[] { new Vector2(0.1f, 0.972f), new Vector2(0.23f, 0.966f), new Vector2(0.227f, 0.042f), new Vector2(0.122f, 0.043f) } }; } return char_b; }
        Vector3[][] Char_c() { if (char_c == null) { char_c = new Vector3[][] { new Vector3[] { new Vector2(0.877f, 0.158f), new Vector2(0.653f, 0.035f), new Vector2(0.425f, 0.037f), new Vector2(0.265f, 0.147f), new Vector2(0.1991f, 0.289f), new Vector2(0.2052f, 0.438f), new Vector2(0.279f, 0.595f), new Vector2(0.4362f, 0.692f), new Vector2(0.6296f, 0.692f), new Vector2(0.7598f, 0.6229f), new Vector2(0.8288f, 0.511f), new Vector2(0.826f, 0.6743f) } }; } return char_c; }
        Vector3[][] Char_d() { if (char_d == null) { char_d = new Vector3[][] { new Vector3[] { new Vector2(0.806f, 0.265f), new Vector2(0.579f, 0.035f), new Vector2(0.3861f, 0.037f), new Vector2(0.2268f, 0.147f), new Vector2(0.1556f, 0.289f), new Vector2(0.1541f, 0.438f), new Vector2(0.2408f, 0.595f), new Vector2(0.3874f, 0.692f), new Vector2(0.5582f, 0.692f), new Vector2(0.6832f, 0.6229f), new Vector2(0.8036f, 0.4701f) }, new Vector3[] { new Vector2(0.91f, 0.049f), new Vector2(0.807f, 0.05f), new Vector2(0.807f, 0.968f), new Vector2(0.677f, 0.97f) } }; } return char_d; }
        Vector3[][] Char_e() { if (char_e == null) { char_e = new Vector3[][] { new Vector3[] { new Vector2(0.837f, 0.128f), new Vector2(0.654f, 0.035f), new Vector2(0.419f, 0.037f), new Vector2(0.256f, 0.147f), new Vector2(0.174f, 0.303f), new Vector2(0.171f, 0.438f), new Vector2(0.259f, 0.595f), new Vector2(0.423f, 0.692f), new Vector2(0.584f, 0.692f), new Vector2(0.747f, 0.581f), new Vector2(0.824f, 0.376f), new Vector2(0.172f, 0.376f) } }; } return char_e; }
        Vector3[][] Char_f() { if (char_f == null) { char_f = new Vector3[][] { new Vector3[] { new Vector2(0.43f, 0.041f), new Vector2(0.431f, 0.827f), new Vector2(0.566f, 0.97f), new Vector2(0.863f, 0.956f) }, new Vector3[] { new Vector2(0.228f, 0.04f), new Vector2(0.764f, 0.048f) }, new Vector3[] { new Vector2(0.23f, 0.671f), new Vector2(0.762f, 0.677f) } }; } return char_f; }
        Vector3[][] Char_g() { if (char_g == null) { char_g = new Vector3[][] { new Vector3[] { new Vector2(0.758f, 0.246f), new Vector2(0.578f, 0.051f), new Vector2(0.374f, 0.049f), new Vector2(0.183f, 0.273f), new Vector2(0.184f, 0.484f), new Vector2(0.358f, 0.682f), new Vector2(0.56f, 0.682f), new Vector2(0.757f, 0.495f) }, new Vector3[] { new Vector2(0.334f, -0.252f), new Vector2(0.559f, -0.255f), new Vector2(0.757f, -0.082f), new Vector2(0.76f, 0.674f), new Vector2(0.861f, 0.676f) } }; } return char_g; }
        Vector3[][] Char_h() { if (char_h == null) { char_h = new Vector3[][] { new Vector3[] { new Vector2(0.242f, 0.532f), new Vector2(0.435f, 0.691f), new Vector2(0.63f, 0.69f), new Vector2(0.775f, 0.552f), new Vector2(0.776f, 0.046f) }, new Vector3[] { new Vector2(0.118f, 0.965f), new Vector2(0.242f, 0.963f), new Vector2(0.241f, 0.047f) }, new Vector3[] { new Vector2(0.13f, 0.048f), new Vector2(0.361f, 0.046f) }, new Vector3[] { new Vector2(0.662f, 0.045f), new Vector2(0.878f, 0.044f) } }; } return char_h; }
        Vector3[][] Char_i() { if (char_i == null) { char_i = new Vector3[][] { new Vector3[] { new Vector2(0.271f, 0.68f), new Vector2(0.502f, 0.676f), new Vector2(0.503f, 0.041f) }, new Vector3[] { new Vector2(0.206f, 0.045f), new Vector2(0.792f, 0.044f) }, new Vector3[] { new Vector2(0.485f, 1.006f), new Vector2(0.486f, 0.901f) } }; } return char_i; }
        Vector3[][] Char_j() { if (char_j == null) { char_j = new Vector3[][] { new Vector3[] { new Vector2(0.216f, 0.68f), new Vector2(0.637f, 0.676f), new Vector2(0.639f, -0.071f), new Vector2(0.499f, -0.249f), new Vector2(0.211f, -0.253f) }, new Vector3[] { new Vector2(0.561f, 0.999f), new Vector2(0.558f, 0.903f) } }; } return char_j; }
        Vector3[][] Char_k() { if (char_k == null) { char_k = new Vector3[][] { new Vector3[] { new Vector2(0.216f, 0.97f), new Vector2(0.335f, 0.969f), new Vector2(0.331f, 0.044f), new Vector2(0.23f, 0.046f) }, new Vector3[] { new Vector2(0.735f, 0.671f), new Vector2(0.345f, 0.327f) }, new Vector3[] { new Vector2(0.667f, 0.05f), new Vector2(0.888f, 0.05f) }, new Vector3[] { new Vector2(0.64f, 0.672f), new Vector2(0.826f, 0.668f) }, new Vector3[] { new Vector2(0.46f, 0.403f), new Vector2(0.811f, 0.05f) } }; } return char_k; }
        Vector3[][] Char_l() { if (char_l == null) { char_l = new Vector3[][] { new Vector3[] { new Vector2(0.266f, 0.97f), new Vector2(0.496f, 0.969f), new Vector2(0.497f, 0.044f) }, new Vector3[] { new Vector2(0.206f, 0.043f), new Vector2(0.785f, 0.045f) } }; } return char_l; }
        Vector3[][] Char_m() { if (char_m == null) { char_m = new Vector3[][] { new Vector3[] { new Vector2(0.178f, 0.55f), new Vector2(0.317f, 0.688f), new Vector2(0.426f, 0.686f), new Vector2(0.527f, 0.603f), new Vector2(0.523f, 0.05f), new Vector2(0.623f, 0.048f) }, new Vector3[] { new Vector2(0.523f, 0.549f), new Vector2(0.658f, 0.689f), new Vector2(0.787f, 0.691f), new Vector2(0.871f, 0.592f), new Vector2(0.872f, 0.046f), new Vector2(0.943f, 0.045f) }, new Vector3[] { new Vector2(0.074f, 0.681f), new Vector2(0.178f, 0.678f), new Vector2(0.175f, 0.047f) }, new Vector3[] { new Vector2(0.075f, 0.047f), new Vector2(0.275f, 0.046f) } }; } return char_m; }
        Vector3[][] Char_n() { if (char_n == null) { char_n = new Vector3[][] { new Vector3[] { new Vector2(0.277f, 0.514f), new Vector2(0.465f, 0.688f), new Vector2(0.65f, 0.686f), new Vector2(0.784f, 0.538f), new Vector2(0.784f, 0.05f) }, new Vector3[] { new Vector2(0.161f, 0.685f), new Vector2(0.272f, 0.681f), new Vector2(0.274f, 0.045f) }, new Vector3[] { new Vector2(0.163f, 0.045f), new Vector2(0.374f, 0.045f) }, new Vector3[] { new Vector2(0.684f, 0.047f), new Vector2(0.897f, 0.046f) } }; } return char_n; }
        Vector3[][] Char_o() { if (char_o == null) { char_o = new Vector3[][] { new Vector3[] { new Vector2(0.182f, 0.438f), new Vector2(0.254f, 0.587f), new Vector2(0.407f, 0.686f), new Vector2(0.595f, 0.686f), new Vector2(0.767f, 0.59f), new Vector2(0.847f, 0.439f), new Vector2(0.849f, 0.304f), new Vector2(0.776f, 0.152f), new Vector2(0.611f, 0.038f), new Vector2(0.414f, 0.039f), new Vector2(0.26f, 0.134f), new Vector2(0.184f, 0.267f), new Vector2(0.182f, 0.438f) } }; } return char_o; }
        Vector3[][] Char_p() { if (char_p == null) { char_p = new Vector3[][] { new Vector3[] { new Vector2(0.212f, 0.438f), new Vector2(0.284f, 0.587f), new Vector2(0.437f, 0.686f), new Vector2(0.625f, 0.686f), new Vector2(0.797f, 0.59f), new Vector2(0.877f, 0.439f), new Vector2(0.8789999f, 0.304f), new Vector2(0.806f, 0.152f), new Vector2(0.641f, 0.038f), new Vector2(0.444f, 0.039f), new Vector2(0.29f, 0.134f), new Vector2(0.214f, 0.267f), new Vector2(0.212f, 0.438f) }, new Vector3[] { new Vector2(0.107f, 0.676f), new Vector2(0.219f, 0.675f), new Vector2(0.218f, -0.249f) }, new Vector3[] { new Vector2(0.106f, -0.25f), new Vector2(0.414f, -0.252f) } }; } return char_p; }
        Vector3[][] Char_q() { if (char_q == null) { char_q = new Vector3[][] { new Vector3[] { new Vector2(0.139f, 0.438f), new Vector2(0.211f, 0.587f), new Vector2(0.364f, 0.686f), new Vector2(0.552f, 0.686f), new Vector2(0.724f, 0.59f), new Vector2(0.804f, 0.439f), new Vector2(0.806f, 0.304f), new Vector2(0.733f, 0.152f), new Vector2(0.568f, 0.038f), new Vector2(0.371f, 0.039f), new Vector2(0.217f, 0.134f), new Vector2(0.141f, 0.267f), new Vector2(0.139f, 0.438f) }, new Vector3[] { new Vector2(0.896f, 0.676f), new Vector2(0.797f, 0.675f), new Vector2(0.798f, -0.249f) }, new Vector3[] { new Vector2(0.605f, -0.25f), new Vector2(0.891f, -0.252f) } }; } return char_q; }
        Vector3[][] Char_r() { if (char_r == null) { char_r = new Vector3[][] { new Vector3[] { new Vector2(0.408f, 0.445f), new Vector2(0.602f, 0.644f), new Vector2(0.682f, 0.686f), new Vector2(0.8f, 0.686f), new Vector2(0.864f, 0.629f) }, new Vector3[] { new Vector2(0.245f, 0.676f), new Vector2(0.399f, 0.675f), new Vector2(0.403f, 0.041f) }, new Vector3[] { new Vector2(0.207f, 0.044f), new Vector2(0.72f, 0.048f) } }; } return char_r; }
        Vector3[][] Char_s() { if (char_s == null) { char_s = new Vector3[][] { new Vector3[] { new Vector2(0.222f, 0.137f), new Vector2(0.392f, 0.042f), new Vector2(0.643f, 0.041f), new Vector2(0.797f, 0.139f), new Vector2(0.796f, 0.251f), new Vector2(0.678f, 0.355f), new Vector2(0.371f, 0.414f), new Vector2(0.271f, 0.486f), new Vector2(0.271f, 0.594f), new Vector2(0.396f, 0.686f), new Vector2(0.6f, 0.696f), new Vector2(0.75f, 0.605f) }, new Vector3[] { new Vector2(0.748f, 0.676f), new Vector2(0.751f, 0.538f) }, new Vector3[] { new Vector2(0.218f, 0.044f), new Vector2(0.22f, 0.226f) } }; } return char_s; }
        Vector3[][] Char_t() { if (char_t == null) { char_t = new Vector3[][] { new Vector3[] { new Vector2(0.331f, 0.906f), new Vector2(0.334f, 0.155f), new Vector2(0.43f, 0.038f), new Vector2(0.666f, 0.034f), new Vector2(0.815f, 0.109f) }, new Vector3[] { new Vector2(0.748f, 0.676f), new Vector2(0.18f, 0.677f) } }; } return char_t; }
        Vector3[][] Char_u() { if (char_u == null) { char_u = new Vector3[][] { new Vector3[] { new Vector2(0.136f, 0.675f), new Vector2(0.24f, 0.676f), new Vector2(0.242f, 0.164f), new Vector2(0.347f, 0.04f), new Vector2(0.543f, 0.04f), new Vector2(0.779f, 0.185f) }, new Vector3[] { new Vector2(0.617f, 0.676f), new Vector2(0.776f, 0.677f), new Vector2(0.779f, 0.044f), new Vector2(0.87f, 0.045f) } }; } return char_u; }
        Vector3[][] Char_v() { if (char_v == null) { char_v = new Vector3[][] { new Vector3[] { new Vector2(0.241f, 0.675f), new Vector2(0.529f, 0.036f), new Vector2(0.819f, 0.678f) }, new Vector3[] { new Vector2(0.651f, 0.676f), new Vector2(0.906f, 0.677f) }, new Vector3[] { new Vector2(0.137f, 0.674f), new Vector2(0.395f, 0.675f) } }; } return char_v; }
        Vector3[][] Char_w() { if (char_w == null) { char_w = new Vector3[][] { new Vector3[] { new Vector2(0.169f, 0.675f), new Vector2(0.299f, 0.036f), new Vector2(0.52f, 0.495f), new Vector2(0.743f, 0.034f), new Vector2(0.877f, 0.675f) }, new Vector3[] { new Vector2(0.754f, 0.676f), new Vector2(0.923f, 0.677f) }, new Vector3[] { new Vector2(0.101f, 0.674f), new Vector2(0.282f, 0.675f) } }; } return char_w; }
        Vector3[][] Char_x() { if (char_x == null) { char_x = new Vector3[][] { new Vector3[] { new Vector2(0.232f, 0.675f), new Vector2(0.836f, 0.053f) }, new Vector3[] { new Vector2(0.637f, 0.676f), new Vector2(0.823f, 0.677f) }, new Vector3[] { new Vector2(0.179f, 0.674f), new Vector2(0.368f, 0.675f) }, new Vector3[] { new Vector2(0.136f, 0.048f), new Vector2(0.351f, 0.049f) }, new Vector3[] { new Vector2(0.653f, 0.051f), new Vector2(0.874f, 0.053f) }, new Vector3[] { new Vector2(0.174f, 0.049f), new Vector2(0.782f, 0.676f) } }; } return char_x; }
        Vector3[][] Char_y() { if (char_y == null) { char_y = new Vector3[][] { new Vector3[] { new Vector2(0.232f, 0.675f), new Vector2(0.553f, 0.031f) }, new Vector3[] { new Vector2(0.737f, 0.676f), new Vector2(0.914f, 0.677f) }, new Vector3[] { new Vector2(0.179f, 0.674f), new Vector2(0.357f, 0.675f) }, new Vector3[] { new Vector2(0.428f, -0.252f), new Vector2(0.865f, 0.674f) }, new Vector3[] { new Vector2(0.193f, -0.252f), new Vector2(0.531f, -0.25f) } }; } return char_y; }
        Vector3[][] Char_z() { if (char_z == null) { char_z = new Vector3[][] { new Vector3[] { new Vector2(0.268f, 0.567f), new Vector2(0.27f, 0.677f), new Vector2(0.8f, 0.677f), new Vector2(0.208f, 0.04f), new Vector2(0.8f, 0.04f), new Vector2(0.802f, 0.158f) } }; } return char_z; }
        Vector3[][] Char_ae() { if (char_ae == null) { char_ae = new Vector3[][] { new Vector3[] { new Vector2(0.723f, 0.165f), new Vector2(0.539f, 0.035f), new Vector2(0.263f, 0.037f), new Vector2(0.173f, 0.1421f), new Vector2(0.1761f, 0.2576f), new Vector2(0.3387f, 0.3954f), new Vector2(0.5582f, 0.4002f), new Vector2(0.7232f, 0.3654f) }, new Vector3[] { new Vector2(0.25f, 0.641f), new Vector2(0.397f, 0.698f), new Vector2(0.586f, 0.697f), new Vector2(0.724f, 0.579f), new Vector2(0.724f, 0.044f), new Vector2(0.84f, 0.044f) }, new Vector3[] { new Vector2(0.328f, 0.83f), new Vector2(0.277f, 0.855f), new Vector2(0.277f, 0.913f), new Vector2(0.329f, 0.95f), new Vector2(0.382f, 0.923f), new Vector2(0.383f, 0.855f), new Vector2(0.328f, 0.83f) }, new Vector3[] { new Vector2(0.655f, 0.83f), new Vector2(0.598f, 0.856f), new Vector2(0.598f, 0.921f), new Vector2(0.651f, 0.951f), new Vector2(0.71f, 0.9191f), new Vector2(0.709f, 0.8543f), new Vector2(0.655f, 0.83f) } }; } return char_ae; }
        Vector3[][] Char_oe() { if (char_oe == null) { char_oe = new Vector3[][] { new Vector3[] { new Vector2(0.182f, 0.438f), new Vector2(0.254f, 0.587f), new Vector2(0.407f, 0.686f), new Vector2(0.595f, 0.686f), new Vector2(0.767f, 0.59f), new Vector2(0.847f, 0.439f), new Vector2(0.849f, 0.304f), new Vector2(0.776f, 0.152f), new Vector2(0.611f, 0.038f), new Vector2(0.414f, 0.039f), new Vector2(0.26f, 0.134f), new Vector2(0.184f, 0.267f), new Vector2(0.182f, 0.438f) }, new Vector3[] { new Vector2(0.372f, 0.83f), new Vector2(0.321f, 0.855f), new Vector2(0.321f, 0.913f), new Vector2(0.373f, 0.95f), new Vector2(0.426f, 0.923f), new Vector2(0.427f, 0.855f), new Vector2(0.372f, 0.83f) }, new Vector3[] { new Vector2(0.699f, 0.83f), new Vector2(0.642f, 0.856f), new Vector2(0.642f, 0.921f), new Vector2(0.6950001f, 0.951f), new Vector2(0.754f, 0.9191f), new Vector2(0.753f, 0.8543f), new Vector2(0.699f, 0.83f) } }; } return char_oe; }
        Vector3[][] Char_ue() { if (char_ue == null) { char_ue = new Vector3[][] { new Vector3[] { new Vector2(0.136f, 0.675f), new Vector2(0.24f, 0.676f), new Vector2(0.242f, 0.164f), new Vector2(0.347f, 0.04f), new Vector2(0.543f, 0.04f), new Vector2(0.779f, 0.185f) }, new Vector3[] { new Vector2(0.617f, 0.676f), new Vector2(0.776f, 0.677f), new Vector2(0.779f, 0.044f), new Vector2(0.87f, 0.045f) }, new Vector3[] { new Vector2(0.372f, 0.83f), new Vector2(0.321f, 0.855f), new Vector2(0.321f, 0.913f), new Vector2(0.373f, 0.95f), new Vector2(0.426f, 0.923f), new Vector2(0.427f, 0.855f), new Vector2(0.372f, 0.83f) }, new Vector3[] { new Vector2(0.699f, 0.83f), new Vector2(0.642f, 0.856f), new Vector2(0.642f, 0.921f), new Vector2(0.6950001f, 0.951f), new Vector2(0.754f, 0.9191f), new Vector2(0.753f, 0.8543f), new Vector2(0.699f, 0.83f) } }; } return char_ue; }

        Vector3[][] Char_A() { if (char_A == null) { char_A = new Vector3[][] { new Vector3[] { new Vector2(0.227f, 0.882f), new Vector2(0.56f, 0.88f), new Vector2(0.868f, 0.035f) }, new Vector3[] { new Vector2(0.152f, 0.03f), new Vector2(0.461f, 0.877f) }, new Vector3[] { new Vector2(0.267f, 0.327f), new Vector2(0.751f, 0.328f) }, new Vector3[] { new Vector2(0.082f, 0.031f), new Vector2(0.296f, 0.033f) }, new Vector3[] { new Vector2(0.721f, 0.035f), new Vector2(0.93f, 0.037f) } }; } return char_A; }
        Vector3[][] Char_B() { if (char_B == null) { char_B = new Vector3[][] { new Vector3[] { new Vector2(0.133f, 0.882f), new Vector2(0.633f, 0.88f), new Vector2(0.8f, 0.755f), new Vector2(0.801f, 0.627f), new Vector2(0.631f, 0.469f) }, new Vector3[] { new Vector2(0.247f, 0.469f), new Vector2(0.631f, 0.469f), new Vector2(0.865f, 0.322f), new Vector2(0.867f, 0.161f), new Vector2(0.712f, 0.028f), new Vector2(0.144f, 0.028f) }, new Vector3[] { new Vector2(0.246f, 0.883f), new Vector2(0.247f, 0.029f) } }; } return char_B; }
        Vector3[][] Char_C() { if (char_C == null) { char_C = new Vector3[][] { new Vector3[] { new Vector2(0.821f, 0.732f), new Vector2(0.662f, 0.877f), new Vector2(0.514f, 0.91f), new Vector2(0.302f, 0.84f), new Vector2(0.146f, 0.594f), new Vector2(0.148f, 0.32f), new Vector2(0.252f, 0.136f), new Vector2(0.435f, 0.014f), new Vector2(0.647f, 0.009f), new Vector2(0.867f, 0.166f) }, new Vector3[] { new Vector2(0.824f, 0.666f), new Vector2(0.825f, 0.884f) } }; } return char_C; }
        Vector3[][] Char_D() { if (char_D == null) { char_D = new Vector3[][] { new Vector3[] { new Vector2(0.201f, 0.879f), new Vector2(0.628f, 0.877f), new Vector2(0.811f, 0.771f), new Vector2(0.897f, 0.577f), new Vector2(0.9f, 0.31f), new Vector2(0.81f, 0.142f), new Vector2(0.624f, 0.028f), new Vector2(0.205f, 0.025f) }, new Vector3[] { new Vector2(0.294f, 0.025f), new Vector2(0.295f, 0.879f) } }; } return char_D; }
        Vector3[][] Char_E() { if (char_E == null) { char_E = new Vector3[][] { new Vector3[] { new Vector2(0.14f, 0.879f), new Vector2(0.777f, 0.877f), new Vector2(0.777f, 0.701f) }, new Vector3[] { new Vector2(0.149f, 0.025f), new Vector2(0.825f, 0.024f), new Vector2(0.826f, 0.234f) }, new Vector3[] { new Vector2(0.248f, 0.025f), new Vector2(0.249f, 0.88f) }, new Vector3[] { new Vector2(0.249f, 0.466f), new Vector2(0.548f, 0.466f) }, new Vector3[] { new Vector2(0.551f, 0.567f), new Vector2(0.55f, 0.367f) } }; } return char_E; }
        Vector3[][] Char_F() { if (char_F == null) { char_F = new Vector3[][] { new Vector3[] { new Vector2(0.226f, 0.879f), new Vector2(0.908f, 0.877f), new Vector2(0.908f, 0.701f) }, new Vector3[] { new Vector2(0.233f, 0.025f), new Vector2(0.597f, 0.024f) }, new Vector3[] { new Vector2(0.338f, 0.025f), new Vector2(0.339f, 0.88f) }, new Vector3[] { new Vector2(0.341f, 0.466f), new Vector2(0.64f, 0.466f) }, new Vector3[] { new Vector2(0.643f, 0.567f), new Vector2(0.642f, 0.367f) } }; } return char_F; }
        Vector3[][] Char_G() { if (char_G == null) { char_G = new Vector3[][] { new Vector3[] { new Vector2(0.824f, 0.769f), new Vector2(0.614f, 0.9f), new Vector2(0.423f, 0.903f), new Vector2(0.261f, 0.798f), new Vector2(0.152f, 0.58f), new Vector2(0.152f, 0.309f), new Vector2(0.238f, 0.128f), new Vector2(0.433f, 0.009f), new Vector2(0.635f, 0.004f), new Vector2(0.824f, 0.078f), new Vector2(0.826f, 0.37f) }, new Vector3[] { new Vector2(0.582f, 0.371f), new Vector2(0.88f, 0.371f) }, new Vector3[] { new Vector2(0.824f, 0.713f), new Vector2(0.825f, 0.885f) } }; } return char_G; }
        Vector3[][] Char_H() { if (char_H == null) { char_H = new Vector3[][] { new Vector3[] { new Vector2(0.158f, 0.882f), new Vector2(0.354f, 0.882f) }, new Vector3[] { new Vector2(0.135f, 0.027f), new Vector2(0.343f, 0.027f) }, new Vector3[] { new Vector2(0.671f, 0.882f), new Vector2(0.856f, 0.882f) }, new Vector3[] { new Vector2(0.669f, 0.027f), new Vector2(0.871f, 0.027f) }, new Vector3[] { new Vector2(0.243f, 0.027f), new Vector2(0.243f, 0.882f) }, new Vector3[] { new Vector2(0.778f, 0.027f), new Vector2(0.779f, 0.882f) }, new Vector3[] { new Vector2(0.244f, 0.466f), new Vector2(0.777f, 0.467f) } }; } return char_H; }
        Vector3[][] Char_I() { if (char_I == null) { char_I = new Vector3[][] { new Vector3[] { new Vector2(0.239f, 0.882f), new Vector2(0.758f, 0.882f) }, new Vector3[] { new Vector2(0.24f, 0.027f), new Vector2(0.757f, 0.027f) }, new Vector3[] { new Vector2(0.501f, 0.882f), new Vector2(0.501f, 0.027f) } }; } return char_I; }
        Vector3[][] Char_J() { if (char_J == null) { char_J = new Vector3[][] { new Vector3[] { new Vector2(0.729f, 0.882f), new Vector2(0.729f, 0.203f), new Vector2(0.647f, 0.074f), new Vector2(0.459f, 0.001f), new Vector2(0.3f, 0.044f), new Vector2(0.194f, 0.134f), new Vector2(0.196f, 0.305f) }, new Vector3[] { new Vector2(0.471f, 0.882f), new Vector2(0.879f, 0.882f) } }; } return char_J; }
        Vector3[][] Char_K() { if (char_K == null) { char_K = new Vector3[][] { new Vector3[] { new Vector2(0.437f, 0.52f), new Vector2(0.661f, 0.358f), new Vector2(0.796f, 0.03f), new Vector2(0.89f, 0.028f) }, new Vector3[] { new Vector2(0.132f, 0.882f), new Vector2(0.375f, 0.882f) }, new Vector3[] { new Vector2(0.713f, 0.882f), new Vector2(0.897f, 0.885f) }, new Vector3[] { new Vector2(0.237f, 0.88f), new Vector2(0.237f, 0.025f) }, new Vector3[] { new Vector2(0.142f, 0.024f), new Vector2(0.375f, 0.025f) }, new Vector3[] { new Vector2(0.237f, 0.386f), new Vector2(0.857f, 0.882f) } }; } return char_K; }
        Vector3[][] Char_L() { if (char_L == null) { char_L = new Vector3[][] { new Vector3[] { new Vector2(0.177f, 0.03f), new Vector2(0.873f, 0.03f), new Vector2(0.875f, 0.25f) }, new Vector3[] { new Vector2(0.184f, 0.882f), new Vector2(0.535f, 0.882f) }, new Vector3[] { new Vector2(0.366f, 0.882f), new Vector2(0.365f, 0.027f) } }; } return char_L; }
        Vector3[][] Char_M() { if (char_M == null) { char_M = new Vector3[][] { new Vector3[] { new Vector2(0.103f, 0.882f), new Vector2(0.253f, 0.883f), new Vector2(0.528f, 0.286f), new Vector2(0.782f, 0.884f), new Vector2(0.913f, 0.886f) }, new Vector3[] { new Vector2(0.178f, 0.882f), new Vector2(0.178f, 0.026f) }, new Vector3[] { new Vector2(0.098f, 0.026f), new Vector2(0.302f, 0.027f) }, new Vector3[] { new Vector2(0.85f, 0.885f), new Vector2(0.847f, 0.028f) }, new Vector3[] { new Vector2(0.723f, 0.024f), new Vector2(0.918f, 0.025f) } }; } return char_M; }
        Vector3[][] Char_N() { if (char_N == null) { char_N = new Vector3[][] { new Vector3[] { new Vector2(0.103f, 0.882f), new Vector2(0.253f, 0.883f), new Vector2(0.799f, 0.023f), new Vector2(0.8f, 0.884f) }, new Vector3[] { new Vector2(0.217f, 0.882f), new Vector2(0.219f, 0.026f) }, new Vector3[] { new Vector2(0.135f, 0.026f), new Vector2(0.362f, 0.027f) }, new Vector3[] { new Vector2(0.66f, 0.885f), new Vector2(0.877f, 0.885f) } }; } return char_N; }
        Vector3[][] Char_O() { if (char_O == null) { char_O = new Vector3[][] { new Vector3[] { new Vector2(0.516f, 0.906f), new Vector2(0.713f, 0.846f), new Vector2(0.844f, 0.673f), new Vector2(0.896f, 0.471f), new Vector2(0.848f, 0.241f), new Vector2(0.708f, 0.061f), new Vector2(0.521f, 0.006f), new Vector2(0.337f, 0.06f), new Vector2(0.191f, 0.229f), new Vector2(0.148f, 0.441f), new Vector2(0.189f, 0.672f), new Vector2(0.312f, 0.833f), new Vector2(0.516f, 0.906f) } }; } return char_O; }
        Vector3[][] Char_P() { if (char_P == null) { char_P = new Vector3[][] { new Vector3[] { new Vector2(0.237f, 0.883f), new Vector2(0.663f, 0.883f), new Vector2(0.829f, 0.79f), new Vector2(0.875f, 0.644f), new Vector2(0.824f, 0.497f), new Vector2(0.651f, 0.395f), new Vector2(0.34f, 0.395f) }, new Vector3[] { new Vector2(0.345f, 0.882f), new Vector2(0.346f, 0.027f) }, new Vector3[] { new Vector2(0.236f, 0.026f), new Vector2(0.568f, 0.027f) } }; } return char_P; }
        Vector3[][] Char_Q() { if (char_Q == null) { char_Q = new Vector3[][] { new Vector3[] { new Vector2(0.516f, 0.906f), new Vector2(0.713f, 0.846f), new Vector2(0.844f, 0.673f), new Vector2(0.896f, 0.471f), new Vector2(0.848f, 0.241f), new Vector2(0.708f, 0.061f), new Vector2(0.521f, 0.006f), new Vector2(0.337f, 0.06f), new Vector2(0.191f, 0.229f), new Vector2(0.148f, 0.441f), new Vector2(0.189f, 0.672f), new Vector2(0.312f, 0.833f), new Vector2(0.516f, 0.906f) }, new Vector3[] { new Vector2(0.577f, 0.237f), new Vector2(0.871f, -0.077f) } }; } return char_Q; }
        Vector3[][] Char_R() { if (char_R == null) { char_R = new Vector3[][] { new Vector3[] { new Vector2(0.138f, 0.883f), new Vector2(0.564f, 0.883f), new Vector2(0.725f, 0.803f), new Vector2(0.782f, 0.664f), new Vector2(0.711f, 0.524f), new Vector2(0.517f, 0.437f), new Vector2(0.241f, 0.442f) }, new Vector3[] { new Vector2(0.519f, 0.436f), new Vector2(0.702f, 0.267f), new Vector2(0.821f, 0.03f), new Vector2(0.881f, 0.032f) }, new Vector3[] { new Vector2(0.142f, 0.026f), new Vector2(0.384f, 0.027f) }, new Vector3[] { new Vector2(0.246f, 0.026f), new Vector2(0.248f, 0.88f) } }; } return char_R; }
        Vector3[][] Char_S() { if (char_S == null) { char_S = new Vector3[][] { new Vector3[] { new Vector2(0.756f, 0.763f), new Vector2(0.641f, 0.876f), new Vector2(0.503f, 0.91f), new Vector2(0.328f, 0.854f), new Vector2(0.25f, 0.709f), new Vector2(0.28f, 0.568f), new Vector2(0.424f, 0.486f), new Vector2(0.654f, 0.435f), new Vector2(0.769f, 0.359f), new Vector2(0.809f, 0.226f), new Vector2(0.756f, 0.102f), new Vector2(0.652f, 0.026f), new Vector2(0.502f, 0.001f), new Vector2(0.341f, 0.04f), new Vector2(0.208f, 0.144f) }, new Vector3[] { new Vector2(0.208f, 0.224f), new Vector2(0.207f, 0.047f) }, new Vector3[] { new Vector2(0.757f, 0.701f), new Vector2(0.759f, 0.872f) } }; } return char_S; }
        Vector3[][] Char_T() { if (char_T == null) { char_T = new Vector3[][] { new Vector3[] { new Vector2(0.13f, 0.749f), new Vector2(0.132f, 0.879f), new Vector2(0.82f, 0.88f), new Vector2(0.822f, 0.747f) }, new Vector3[] { new Vector2(0.475f, 0.882f), new Vector2(0.474f, 0.027f) }, new Vector3[] { new Vector2(0.346f, 0.025f), new Vector2(0.603f, 0.026f) } }; } return char_T; }
        Vector3[][] Char_U() { if (char_U == null) { char_U = new Vector3[][] { new Vector3[] { new Vector2(0.227f, 0.879f), new Vector2(0.227f, 0.242f), new Vector2(0.3f, 0.103f), new Vector2(0.437f, 0.013f), new Vector2(0.602f, 0.01f), new Vector2(0.731f, 0.09f), new Vector2(0.808f, 0.251f), new Vector2(0.811f, 0.879f) }, new Vector3[] { new Vector2(0.693f, 0.883f), new Vector2(0.885f, 0.879f) }, new Vector3[] { new Vector2(0.14f, 0.879f), new Vector2(0.343f, 0.878f) } }; } return char_U; }
        Vector3[][] Char_V() { if (char_V == null) { char_V = new Vector3[][] { new Vector3[] { new Vector2(0.17f, 0.879f), new Vector2(0.514f, 0.003f), new Vector2(0.863f, 0.88f) }, new Vector3[] { new Vector2(0.729f, 0.883f), new Vector2(0.913f, 0.879f) }, new Vector3[] { new Vector2(0.107f, 0.879f), new Vector2(0.302f, 0.878f) } }; } return char_V; }
        Vector3[][] Char_W() { if (char_W == null) { char_W = new Vector3[][] { new Vector3[] { new Vector2(0.153f, 0.879f), new Vector2(0.269f, 0.009f), new Vector2(0.509f, 0.684f), new Vector2(0.739f, 0.009f), new Vector2(0.854f, 0.878f) }, new Vector3[] { new Vector2(0.694f, 0.878f), new Vector2(0.905f, 0.879f) }, new Vector3[] { new Vector2(0.091f, 0.879f), new Vector2(0.308f, 0.878f) } }; } return char_W; }
        Vector3[][] Char_X() { if (char_X == null) { char_X = new Vector3[][] { new Vector3[] { new Vector2(0.184f, 0.879f), new Vector2(0.862f, 0.028f) }, new Vector3[] { new Vector2(0.161f, 0.024f), new Vector2(0.792f, 0.879f) }, new Vector3[] { new Vector2(0.118f, 0.025f), new Vector2(0.302f, 0.026f) }, new Vector3[] { new Vector2(0.136f, 0.881f), new Vector2(0.298f, 0.881f) }, new Vector3[] { new Vector2(0.68f, 0.88f), new Vector2(0.84f, 0.879f) }, new Vector3[] { new Vector2(0.707f, 0.027f), new Vector2(0.902f, 0.027f) } }; } return char_X; }
        Vector3[][] Char_Y() { if (char_Y == null) { char_Y = new Vector3[][] { new Vector3[] { new Vector2(0.197f, 0.879f), new Vector2(0.523f, 0.435f), new Vector2(0.836f, 0.88f) }, new Vector3[] { new Vector2(0.522f, 0.436f), new Vector2(0.521f, 0.027f) }, new Vector3[] { new Vector2(0.396f, 0.025f), new Vector2(0.641f, 0.026f) }, new Vector3[] { new Vector2(0.136f, 0.881f), new Vector2(0.311f, 0.881f) }, new Vector3[] { new Vector2(0.732f, 0.88f), new Vector2(0.904f, 0.879f) } }; } return char_Y; }
        Vector3[][] Char_Z() { if (char_Z == null) { char_Z = new Vector3[][] { new Vector3[] { new Vector2(0.266f, 0.744f), new Vector2(0.266f, 0.88f), new Vector2(0.781f, 0.88f), new Vector2(0.208f, 0.024f), new Vector2(0.802f, 0.024f), new Vector2(0.802f, 0.198f) } }; } return char_Z; }
        Vector3[][] Char_AE() { if (char_AE == null) { char_AE = new Vector3[][] { new Vector3[] { new Vector2(0.227f, 0.882f), new Vector2(0.56f, 0.88f), new Vector2(0.868f, 0.035f) }, new Vector3[] { new Vector2(0.152f, 0.03f), new Vector2(0.461f, 0.877f) }, new Vector3[] { new Vector2(0.267f, 0.327f), new Vector2(0.751f, 0.328f) }, new Vector3[] { new Vector2(0.082f, 0.031f), new Vector2(0.296f, 0.033f) }, new Vector3[] { new Vector2(0.721f, 0.035f), new Vector2(0.93f, 0.037f) }, new Vector3[] { new Vector2(0.303f, 1.024f), new Vector2(0.252f, 1.049f), new Vector2(0.252f, 1.107f), new Vector2(0.304f, 1.144f), new Vector2(0.357f, 1.117f), new Vector2(0.358f, 1.049f), new Vector2(0.303f, 1.024f) }, new Vector3[] { new Vector2(0.63f, 1.024f), new Vector2(0.573f, 1.05f), new Vector2(0.573f, 1.115f), new Vector2(0.626f, 1.145f), new Vector2(0.685f, 1.1131f), new Vector2(0.684f, 1.0483f), new Vector2(0.63f, 1.024f) } }; } return char_AE; }
        Vector3[][] Char_OE() { if (char_OE == null) { char_OE = new Vector3[][] { new Vector3[] { new Vector2(0.516f, 0.906f), new Vector2(0.713f, 0.846f), new Vector2(0.844f, 0.673f), new Vector2(0.896f, 0.471f), new Vector2(0.848f, 0.241f), new Vector2(0.708f, 0.061f), new Vector2(0.521f, 0.006f), new Vector2(0.337f, 0.06f), new Vector2(0.191f, 0.229f), new Vector2(0.148f, 0.441f), new Vector2(0.189f, 0.672f), new Vector2(0.312f, 0.833f), new Vector2(0.516f, 0.906f) }, new Vector3[] { new Vector2(0.368f, 1.024f), new Vector2(0.317f, 1.049f), new Vector2(0.317f, 1.107f), new Vector2(0.369f, 1.144f), new Vector2(0.422f, 1.117f), new Vector2(0.423f, 1.049f), new Vector2(0.368f, 1.024f) }, new Vector3[] { new Vector2(0.695f, 1.024f), new Vector2(0.638f, 1.05f), new Vector2(0.638f, 1.115f), new Vector2(0.691f, 1.145f), new Vector2(0.75f, 1.1131f), new Vector2(0.749f, 1.0483f), new Vector2(0.695f, 1.024f) } }; } return char_OE; }
        Vector3[][] Char_UE() { if (char_UE == null) { char_UE = new Vector3[][] { new Vector3[] { new Vector2(0.227f, 0.879f), new Vector2(0.227f, 0.242f), new Vector2(0.3f, 0.103f), new Vector2(0.437f, 0.013f), new Vector2(0.602f, 0.01f), new Vector2(0.731f, 0.09f), new Vector2(0.808f, 0.251f), new Vector2(0.811f, 0.879f) }, new Vector3[] { new Vector2(0.693f, 0.883f), new Vector2(0.885f, 0.879f) }, new Vector3[] { new Vector2(0.14f, 0.879f), new Vector2(0.343f, 0.878f) }, new Vector3[] { new Vector2(0.368f, 1.024f), new Vector2(0.317f, 1.049f), new Vector2(0.317f, 1.107f), new Vector2(0.369f, 1.144f), new Vector2(0.422f, 1.117f), new Vector2(0.423f, 1.049f), new Vector2(0.368f, 1.024f) }, new Vector3[] { new Vector2(0.695f, 1.024f), new Vector2(0.638f, 1.05f), new Vector2(0.638f, 1.115f), new Vector2(0.691f, 1.145f), new Vector2(0.75f, 1.1131f), new Vector2(0.749f, 1.0483f), new Vector2(0.695f, 1.024f) } }; } return char_UE; }

        Vector3[][] Char_0() { if (char_0 == null) { char_0 = new Vector3[][] { new Vector3[] { new Vector2(0.451f, 0.004f), new Vector2(0.601f, 0.003f), new Vector2(0.732f, 0.127f), new Vector2(0.803f, 0.317f), new Vector2(0.802f, 0.672f), new Vector2(0.73f, 0.856f), new Vector2(0.603f, 0.971f), new Vector2(0.437f, 0.971f), new Vector2(0.313f, 0.858f), new Vector2(0.246f, 0.671f), new Vector2(0.246f, 0.333f), new Vector2(0.311f, 0.127f), new Vector2(0.451f, 0.004f) } }; } return char_0; }
        Vector3[][] Char_1() { if (char_1 == null) { char_1 = new Vector3[][] { new Vector3[] { new Vector2(0.241f, 0.874f), new Vector2(0.522f, 0.966f), new Vector2(0.524f, 0.028f) }, new Vector3[] { new Vector2(0.306f, 0.026f), new Vector2(0.732f, 0.027f) } }; } return char_1; }
        Vector3[][] Char_2() { if (char_2 == null) { char_2 = new Vector3[][] { new Vector3[] { new Vector2(0.764f, 0.105f), new Vector2(0.764f, 0.031f), new Vector2(0.183f, 0.031f), new Vector2(0.72f, 0.596f), new Vector2(0.763f, 0.708f), new Vector2(0.752f, 0.804f), new Vector2(0.687f, 0.903f), new Vector2(0.568f, 0.972f), new Vector2(0.42f, 0.967f), new Vector2(0.286f, 0.888f), new Vector2(0.227f, 0.779f) } }; } return char_2; }
        Vector3[][] Char_3() { if (char_3 == null) { char_3 = new Vector3[][] { new Vector3[] { new Vector2(0.228f, 0.094f), new Vector2(0.363f, 0.031f), new Vector2(0.519f, 0.001f), new Vector2(0.69f, 0.055f), new Vector2(0.783f, 0.157f), new Vector2(0.81f, 0.284f), new Vector2(0.755f, 0.414f), new Vector2(0.635f, 0.513f), new Vector2(0.481f, 0.535f), new Vector2(0.638f, 0.56f), new Vector2(0.75f, 0.635f), new Vector2(0.786f, 0.779f), new Vector2(0.713f, 0.908f), new Vector2(0.55f, 0.975f), new Vector2(0.375f, 0.951f), new Vector2(0.252f, 0.88f) } }; } return char_3; }
        Vector3[][] Char_4() { if (char_4 == null) { char_4 = new Vector3[][] { new Vector3[] { new Vector2(0.66f, 0.03f), new Vector2(0.662f, 0.987f), new Vector2(0.194f, 0.303f), new Vector2(0.797f, 0.304f) }, new Vector3[] { new Vector2(0.528f, 0.029f), new Vector2(0.743f, 0.027f) } }; } return char_4; }
        Vector3[][] Char_5() { if (char_5 == null) { char_5 = new Vector3[][] { new Vector3[] { new Vector2(0.228f, 0.124f), new Vector2(0.363f, 0.031f), new Vector2(0.519f, 0.001f), new Vector2(0.69f, 0.049f), new Vector2(0.796f, 0.157f), new Vector2(0.829f, 0.32f), new Vector2(0.791f, 0.491f), new Vector2(0.69f, 0.584f), new Vector2(0.564f, 0.609f), new Vector2(0.438f, 0.582f), new Vector2(0.319f, 0.53f), new Vector2(0.321f, 0.957f), new Vector2(0.722f, 0.958f) } }; } return char_5; }
        Vector3[][] Char_6() { if (char_6 == null) { char_6 = new Vector3[][] { new Vector3[] { new Vector2(0.83f, 0.947f), new Vector2(0.707f, 0.98f), new Vector2(0.534f, 0.932f), new Vector2(0.365f, 0.791f), new Vector2(0.277f, 0.567f), new Vector2(0.288f, 0.328f), new Vector2(0.355f, 0.134f), new Vector2(0.51f, 0.009f), new Vector2(0.665f, 0.007f), new Vector2(0.801f, 0.143f), new Vector2(0.829f, 0.319f), new Vector2(0.747f, 0.491f), new Vector2(0.596f, 0.564f), new Vector2(0.431f, 0.51f), new Vector2(0.298f, 0.333f) } }; } return char_6; }
        Vector3[][] Char_7() { if (char_7 == null) { char_7 = new Vector3[][] { new Vector3[] { new Vector2(0.222f, 0.863f), new Vector2(0.222f, 0.955f), new Vector2(0.759f, 0.955f), new Vector2(0.486f, 0.021f) } }; } return char_7; }
        Vector3[][] Char_8() { if (char_8 == null) { char_8 = new Vector3[][] { new Vector3[] { new Vector2(0.59f, 0.515f), new Vector2(0.416f, 0.518f), new Vector2(0.303f, 0.597f), new Vector2(0.242f, 0.73f), new Vector2(0.296f, 0.875f), new Vector2(0.425f, 0.966f), new Vector2(0.591f, 0.969f), new Vector2(0.702f, 0.891f), new Vector2(0.764f, 0.758f), new Vector2(0.732f, 0.624f), new Vector2(0.59f, 0.515f) }, new Vector3[] { new Vector2(0.591f, 0.515f), new Vector2(0.729f, 0.401f), new Vector2(0.766f, 0.253f), new Vector2(0.713f, 0.098f), new Vector2(0.512f, 0.004f), new Vector2(0.329f, 0.074f), new Vector2(0.253f, 0.247f), new Vector2(0.292f, 0.405f), new Vector2(0.416f, 0.517f) } }; } return char_8; }
        Vector3[][] Char_9() { if (char_9 == null) { char_9 = new Vector3[][] { new Vector3[] { new Vector2(0.267f, 0.026f), new Vector2(0.399f, -0.002f), new Vector2(0.572f, 0.047f), new Vector2(0.745f, 0.173f), new Vector2(0.821f, 0.361f), new Vector2(0.813f, 0.663f), new Vector2(0.753f, 0.84f), new Vector2(0.651f, 0.944f), new Vector2(0.52f, 0.978f), new Vector2(0.383f, 0.934f), new Vector2(0.28f, 0.776f), new Vector2(0.28f, 0.612f), new Vector2(0.362f, 0.46f), new Vector2(0.519f, 0.398f), new Vector2(0.696f, 0.468f), new Vector2(0.815f, 0.644f) } }; } return char_9; }

        Vector3[][] Char_space() { if (char_space == null) { char_space = new Vector3[][] { }; } return char_space; }
        Vector3[][] Char_unknown() { if (char_unknown == null) { char_unknown = new Vector3[][] { new Vector3[] { new Vector2(0.576f, 0.236f), new Vector2(0.576f, 0.11f), new Vector2(0.751f, 0.111f), new Vector2(0.751f, 0.332f), new Vector2(0.483f, 0.332f), new Vector2(0.576f, 0.332f), new Vector2(0.576f, 0.465f), new Vector2(0.751f, 0.465f), new Vector2(0.751f, 0.685f), new Vector2(0.653f, 0.685f), new Vector2(0.653f, 0.577f), new Vector2(0.751f, 0.577f), new Vector2(0.751f, 0.685f), new Vector2(0.576f, 0.685f), new Vector2(0.576f, 0.577f) }, new Vector3[] { new Vector2(0.33f, 0.069f), new Vector2(0.22f, 0.069f), new Vector2(0.221f, 0.219f), new Vector2(0.33f, 0.219f), new Vector2(0.33f, 0.673f), new Vector2(0.22f, 0.673f), new Vector2(0.22f, 0.755f), new Vector2(0.33f, 0.755f), new Vector2(0.33f, 0.884f) }, new Vector3[] { new Vector2(0.066f, -0.02f), new Vector2(0.066f, 1.024f), new Vector2(0.934f, 1.024f), new Vector2(0.934f, -0.02f), new Vector2(0.066f, -0.02f) }, new Vector3[] { new Vector2(0.751f, 0.828f), new Vector2(0.615f, 0.828f), new Vector2(0.576f, 0.901f) }, new Vector3[] { new Vector2(0.33f, 0.418f), new Vector2(0.22f, 0.336f), new Vector2(0.22f, 0.411f) }, new Vector3[] { new Vector2(0.33f, 0.556f), new Vector2(0.22f, 0.476f), new Vector2(0.22f, 0.544f) }, new Vector3[] { new Vector2(0.22f, 0.141f), new Vector2(0.33f, 0.141f) }, new Vector3[] { new Vector2(0.33f, 0.615f), new Vector2(0.22f, 0.614f) }, new Vector3[] { new Vector2(0.33f, 0.884f), new Vector2(0.33f, 0.813f), new Vector2(0.22f, 0.813f), new Vector2(0.22f, 0.884f), new Vector2(0.4f, 0.884f), new Vector2(0.4f, 0.813f) }, new Vector3[] { new Vector2(0.22f, 0.279f), new Vector2(0.33f, 0.279f) } }; } return char_unknown; }
        Vector3[][] Char_dollar() { if (char_dollar == null) { char_dollar = new Vector3[][] { new Vector3[] { new Vector2(0.758f, 0.833f), new Vector2(0.611f, 0.925f), new Vector2(0.419f, 0.918f), new Vector2(0.31f, 0.831f), new Vector2(0.26f, 0.714f), new Vector2(0.35f, 0.565f), new Vector2(0.69f, 0.473f), new Vector2(0.779f, 0.36f), new Vector2(0.779f, 0.253f), new Vector2(0.677f, 0.157f), new Vector2(0.522f, 0.109f), new Vector2(0.373f, 0.137f), new Vector2(0.251f, 0.202f) }, new Vector3[] { new Vector2(0.523f, 0.109f), new Vector2(0.527f, -0.146f) }, new Vector3[] { new Vector2(0.524f, 0.926f), new Vector2(0.523f, 1.063f) }, new Vector3[] { new Vector2(0.25f, 0.112f), new Vector2(0.25f, 0.258f) }, new Vector3[] { new Vector2(0.759f, 0.779f), new Vector2(0.758f, 0.913f) } }; } return char_dollar; }
        Vector3[][] Char_euro() { if (char_euro == null) { char_euro = new Vector3[][] { new Vector3[] { new Vector2(0.823f, 0.738f), new Vector2(0.703f, 0.852f), new Vector2(0.603f, 0.902f), new Vector2(0.416f, 0.896f), new Vector2(0.277f, 0.817f), new Vector2(0.189f, 0.69f), new Vector2(0.146f, 0.546f), new Vector2(0.151f, 0.362f), new Vector2(0.208f, 0.198f), new Vector2(0.318f, 0.076f), new Vector2(0.459f, 0.009f), new Vector2(0.623f, 0.006f), new Vector2(0.751f, 0.064f), new Vector2(0.857f, 0.169f) }, new Vector3[] { new Vector2(0.062f, 0.407f), new Vector2(0.563f, 0.408f) }, new Vector3[] { new Vector2(0.062f, 0.522f), new Vector2(0.597f, 0.522f) }, new Vector3[] { new Vector2(0.823f, 0.669f), new Vector2(0.823f, 0.888f) } }; } return char_euro; }
        Vector3[][] Char_hashtag() { if (char_hashtag == null) { char_hashtag = new Vector3[][] { new Vector3[] { new Vector2(0.23f, 0.604f), new Vector2(0.804f, 0.603f) }, new Vector3[] { new Vector2(0.232f, 0.348f), new Vector2(0.805f, 0.347f) }, new Vector3[] { new Vector2(0.315f, -0.069f), new Vector2(0.449f, 1.024f) }, new Vector3[] { new Vector2(0.59f, -0.066f), new Vector2(0.715f, 1.025f) } }; } return char_hashtag; }
        Vector3[][] Char_exclamationMark() { if (char_exclamationMark == null) { char_exclamationMark = new Vector3[][] { new Vector3[] { new Vector2(0.522f, -0.009f), new Vector2(0.564f, 0.006f), new Vector2(0.583f, 0.047f), new Vector2(0.566f, 0.091f), new Vector2(0.531f, 0.101f), new Vector2(0.489f, 0.086f), new Vector2(0.465f, 0.046f), new Vector2(0.482f, 0.006f), new Vector2(0.522f, -0.009f) }, new Vector3[] { new Vector2(0.522f, 0.353f), new Vector2(0.523f, 0.941f) } }; } return char_exclamationMark; }
        Vector3[][] Char_questionMark() { if (char_questionMark == null) { char_questionMark = new Vector3[][] { new Vector3[] { new Vector2(0.5399f, -0.009f), new Vector2(0.5924f, 0.0039f), new Vector2(0.6202f, 0.0459f), new Vector2(0.5862f, 0.0862f), new Vector2(0.54f, 0.101f), new Vector2(0.4869f, 0.0897f), new Vector2(0.4549f, 0.0508f), new Vector2(0.4862f, 0.0044f), new Vector2(0.5399f, -0.009f) }, new Vector3[] { new Vector2(0.525f, 0.328f), new Vector2(0.527f, 0.417f), new Vector2(0.672f, 0.495f), new Vector2(0.778f, 0.602f), new Vector2(0.779f, 0.745f), new Vector2(0.686f, 0.855f), new Vector2(0.527f, 0.908f), new Vector2(0.27f, 0.839f), new Vector2(0.271f, 0.754f) } }; } return char_questionMark; }
        Vector3[][] Char_quote() { if (char_quote == null) { char_quote = new Vector3[][] { new Vector3[] { new Vector2(0.497f, 0.969f), new Vector2(0.497f, 0.534f) } }; } return char_quote; }
        Vector3[][] Char_doublequote() { if (char_doublequote == null) { char_doublequote = new Vector3[][] { new Vector3[] { new Vector2(0.331f, 0.962f), new Vector2(0.331f, 0.576f) }, new Vector3[] { new Vector2(0.653f, 0.576f), new Vector2(0.653f, 0.958f) } }; } return char_doublequote; }
        Vector3[][] Char_plus() { if (char_plus == null) { char_plus = new Vector3[][] { new Vector3[] { new Vector2(0.524f, 0.837f), new Vector2(0.524f, 0.101f) }, new Vector3[] { new Vector2(0.175f, 0.465f), new Vector2(0.856f, 0.465f) } }; } return char_plus; }
        Vector3[][] Char_minus() { if (char_minus == null) { char_minus = new Vector3[][] { new Vector3[] { new Vector2(0.199f, 0.452f), new Vector2(0.821f, 0.452f) } }; } return char_minus; }
        Vector3[][] Char_comma() { if (char_comma == null) { char_comma = new Vector3[][] { new Vector3[] { new Vector2(0.465f, 0.213f), new Vector2(0.298f, -0.201f) } }; } return char_comma; }
        Vector3[][] Char_asterisk() { if (char_asterisk == null) { char_asterisk = new Vector3[][] { new Vector3[] { new Vector2(0.211f, 0.755f), new Vector2(0.499f, 0.675f), new Vector2(0.668f, 0.43f) }, new Vector3[] { new Vector2(0.323f, 0.434f), new Vector2(0.499f, 0.675f), new Vector2(0.776f, 0.75f) }, new Vector3[] { new Vector2(0.499f, 0.948f), new Vector2(0.499f, 0.675f) } }; } return char_asterisk; }
        Vector3[][] Char_underscore() { if (char_underscore == null) { char_underscore = new Vector3[][] { new Vector3[] { new Vector2(0.094f, 0.018f), new Vector2(0.932f, 0.018f) } }; } return char_underscore; }
        Vector3[][] Char_period() { if (char_period == null) { char_period = new Vector3[][] { new Vector3[] { new Vector2(0.493f, -0.009f), new Vector2(0.548f, 0.015f), new Vector2(0.5733f, 0.073f), new Vector2(0.546f, 0.116f), new Vector2(0.4931f, 0.143f), new Vector2(0.439f, 0.121f), new Vector2(0.408f, 0.074f), new Vector2(0.438f, 0.014f), new Vector2(0.493f, -0.009f) } }; } return char_period; }
        Vector3[][] Char_forwardslash() { if (char_forwardslash == null) { char_forwardslash = new Vector3[][] { new Vector3[] { new Vector2(0.812f, 1.052f), new Vector2(0.238f, -0.118f) } }; } return char_forwardslash; }
        Vector3[][] Char_backwardslash() { if (char_backwardslash == null) { char_backwardslash = new Vector3[][] { new Vector3[] { new Vector2(0.233f, 1.052f), new Vector2(0.806f, -0.118f) } }; } return char_backwardslash; }
        Vector3[][] Char_colon() { if (char_colon == null) { char_colon = new Vector3[][] { new Vector3[] { new Vector2(0.493f, -0.009f), new Vector2(0.548f, 0.015f), new Vector2(0.5733f, 0.073f), new Vector2(0.546f, 0.116f), new Vector2(0.4931f, 0.143f), new Vector2(0.439f, 0.121f), new Vector2(0.408f, 0.074f), new Vector2(0.438f, 0.014f), new Vector2(0.493f, -0.009f) }, new Vector3[] { new Vector2(0.483f, 0.499f), new Vector2(0.542f, 0.515f), new Vector2(0.574f, 0.573f), new Vector2(0.55f, 0.636f), new Vector2(0.491f, 0.658f), new Vector2(0.427f, 0.63f), new Vector2(0.404f, 0.579f), new Vector2(0.434f, 0.518f), new Vector2(0.483f, 0.499f) } }; } return char_colon; }
        Vector3[][] Char_semicolon() { if (char_semicolon == null) { char_semicolon = new Vector3[][] { new Vector3[] { new Vector2(0.517f, 0.5f), new Vector2(0.572f, 0.524f), new Vector2(0.5973001f, 0.582f), new Vector2(0.5700001f, 0.625f), new Vector2(0.5171f, 0.652f), new Vector2(0.463f, 0.63f), new Vector2(0.432f, 0.583f), new Vector2(0.462f, 0.523f), new Vector2(0.517f, 0.5f) }, new Vector3[] { new Vector2(0.483f, 0.207f), new Vector2(0.322f, -0.132f) } }; } return char_semicolon; }
        Vector3[][] Char_lessthan() { if (char_lessthan == null) { char_lessthan = new Vector3[][] { new Vector3[] { new Vector2(0.842f, 0.827f), new Vector2(0.161f, 0.444f), new Vector2(0.825f, 0.06f) } }; } return char_lessthan; }
        Vector3[][] Char_equals() { if (char_equals == null) { char_equals = new Vector3[][] { new Vector3[] { new Vector2(0.158f, 0.579f), new Vector2(0.849f, 0.579f) }, new Vector3[] { new Vector2(0.159f, 0.322f), new Vector2(0.848f, 0.322f) } }; } return char_equals; }
        Vector3[][] Char_greaterthan() { if (char_greaterthan == null) { char_greaterthan = new Vector3[][] { new Vector3[] { new Vector2(0.197f, 0.827f), new Vector2(0.866f, 0.444f), new Vector2(0.181f, 0.06f) } }; } return char_greaterthan; }
        Vector3[][] Char_percent() { if (char_percent == null) { char_percent = new Vector3[][] { new Vector3[] { new Vector2(0.357f, 0.594f), new Vector2(0.483f, 0.649f), new Vector2(0.54f, 0.777f), new Vector2(0.485f, 0.915f), new Vector2(0.35f, 0.9589999f), new Vector2(0.203f, 0.896f), new Vector2(0.1609999f, 0.786f), new Vector2(0.218f, 0.646f), new Vector2(0.357f, 0.594f) }, new Vector3[] { new Vector2(0.67f, -0.008f), new Vector2(0.794f, 0.05f), new Vector2(0.85f, 0.181f), new Vector2(0.786f, 0.307f), new Vector2(0.66f, 0.363f), new Vector2(0.525f, 0.301f), new Vector2(0.475f, 0.175f), new Vector2(0.531f, 0.046f), new Vector2(0.67f, -0.008f) }, new Vector3[] { new Vector2(0.171f, 0.238f), new Vector2(0.845f, 0.726f) } }; } return char_percent; }
        Vector3[][] Char_ampersand() { if (char_ampersand == null) { char_ampersand = new Vector3[][] { new Vector3[] { new Vector2(0.682f, 0.811f), new Vector2(0.632f, 0.78f), new Vector2(0.554f, 0.813f), new Vector2(0.47f, 0.811f), new Vector2(0.388f, 0.76f), new Vector2(0.344f, 0.684f), new Vector2(0.347f, 0.619f), new Vector2(0.724f, 0.031f), new Vector2(0.809f, 0.032f) }, new Vector3[] { new Vector2(0.439f, 0.44f), new Vector2(0.303f, 0.368f), new Vector2(0.247f, 0.222f), new Vector2(0.301f, 0.08f), new Vector2(0.454f, 0.005f), new Vector2(0.588f, 0.051f), new Vector2(0.681f, 0.189f), new Vector2(0.733f, 0.372f), new Vector2(0.791f, 0.372f) } }; } return char_ampersand; }
        Vector3[][] Char_openbracket() { if (char_openbracket == null) { char_openbracket = new Vector3[][] { new Vector3[] { new Vector2(0.695f, 0.954f), new Vector2(0.637f, 0.848f), new Vector2(0.588f, 0.736f), new Vector2(0.558f, 0.623f), new Vector2(0.538f, 0.527f), new Vector2(0.528f, 0.429f), new Vector2(0.525f, 0.318f), new Vector2(0.54f, 0.225f), new Vector2(0.568f, 0.124f), new Vector2(0.605f, 0.012f), new Vector2(0.646f, -0.094f), new Vector2(0.693f, -0.189f) } }; } return char_openbracket; }
        Vector3[][] Char_closebracket() { if (char_closebracket == null) { char_closebracket = new Vector3[][] { new Vector3[] { new Vector2(0.34f, 0.954f), new Vector2(0.392f, 0.848f), new Vector2(0.439f, 0.736f), new Vector2(0.479f, 0.623f), new Vector2(0.495f, 0.527f), new Vector2(0.501f, 0.429f), new Vector2(0.501f, 0.318f), new Vector2(0.489f, 0.225f), new Vector2(0.472f, 0.124f), new Vector2(0.431f, 0.012f), new Vector2(0.385f, -0.094f), new Vector2(0.334f, -0.189f) } }; } return char_closebracket; }
        Vector3[][] Char_opensquarebracket() { if (char_opensquarebracket == null) { char_opensquarebracket = new Vector3[][] { new Vector3[] { new Vector2(0.719f, 0.946f), new Vector2(0.506f, 0.946f), new Vector2(0.505f, -0.182f), new Vector2(0.704f, -0.183f) } }; } return char_opensquarebracket; }
        Vector3[][] Char_closesquarebracket() { if (char_closesquarebracket == null) { char_closesquarebracket = new Vector3[][] { new Vector3[] { new Vector2(0.303f, 0.946f), new Vector2(0.506f, 0.946f), new Vector2(0.505f, -0.182f), new Vector2(0.314f, -0.183f) } }; } return char_closesquarebracket; }
        Vector3[][] Char_leftbrace() { if (char_leftbrace == null) { char_leftbrace = new Vector3[][] { new Vector3[] { new Vector2(0.672f, 0.954f), new Vector2(0.61f, 0.948f), new Vector2(0.567f, 0.924f), new Vector2(0.536f, 0.881f), new Vector2(0.522f, 0.829f), new Vector2(0.523f, 0.505f), new Vector2(0.501f, 0.452f), new Vector2(0.455f, 0.413f), new Vector2(0.38f, 0.396f), new Vector2(0.455f, 0.379f), new Vector2(0.497f, 0.348f), new Vector2(0.522f, 0.295f), new Vector2(0.522f, -0.029f), new Vector2(0.546f, -0.101f), new Vector2(0.6f, -0.149f), new Vector2(0.666f, -0.164f) } }; } return char_leftbrace; }
        Vector3[][] Char_rightbrace() { if (char_rightbrace == null) { char_rightbrace = new Vector3[][] { new Vector3[] { new Vector2(0.356f, 0.954f), new Vector2(0.408f, 0.948f), new Vector2(0.447f, 0.924f), new Vector2(0.482f, 0.881f), new Vector2(0.5f, 0.829f), new Vector2(0.501f, 0.505f), new Vector2(0.531f, 0.443f), new Vector2(0.58f, 0.413f), new Vector2(0.641f, 0.396f), new Vector2(0.58f, 0.379f), new Vector2(0.529f, 0.348f), new Vector2(0.5f, 0.295f), new Vector2(0.5f, -0.029f), new Vector2(0.475f, -0.101f), new Vector2(0.423f, -0.149f), new Vector2(0.36f, -0.164f) } }; } return char_rightbrace; }
        Vector3[][] Char_verticalbar() { if (char_verticalbar == null) { char_verticalbar = new Vector3[][] { new Vector3[] { new Vector2(0.506f, 0.953f), new Vector2(0.505f, -0.196f) } }; } return char_verticalbar; }
        Vector3[][] Char_at() { if (char_at == null) { char_at = new Vector3[][] { new Vector3[] { new Vector2(0.652f, 0.622f), new Vector2(0.619f, 0.236f), new Vector2(0.653f, 0.153f), new Vector2(0.705f, 0.145f), new Vector2(0.776f, 0.278f), new Vector2(0.794f, 0.644f), new Vector2(0.743f, 0.895f), new Vector2(0.586f, 0.995f), new Vector2(0.44f, 0.996f), new Vector2(0.249f, 0.847f), new Vector2(0.166f, 0.603f), new Vector2(0.165f, 0.274f), new Vector2(0.256f, 0.037f), new Vector2(0.428f, -0.087f), new Vector2(0.594f, -0.09f), new Vector2(0.797f, -0.049f) }, new Vector3[] { new Vector2(0.64f, 0.55f), new Vector2(0.564f, 0.615f), new Vector2(0.504f, 0.634f), new Vector2(0.425f, 0.593f), new Vector2(0.387f, 0.518f), new Vector2(0.388f, 0.333f), new Vector2(0.46f, 0.242f), new Vector2(0.534f, 0.244f), new Vector2(0.626f, 0.321f) } }; } return char_at; }
        Vector3[][] Char_caret() { if (char_caret == null) { char_caret = new Vector3[][] { new Vector3[] { new Vector2(0.211f, 0.605f), new Vector2(0.501f, 0.942f), new Vector2(0.788f, 0.603f) } }; } return char_caret; }
        Vector3[][] Char_tilde() { if (char_tilde == null) { char_tilde = new Vector3[][] { new Vector3[] { new Vector2(0.186f, 0.407f), new Vector2(0.237f, 0.475f), new Vector2(0.296f, 0.523f), new Vector2(0.357f, 0.542f), new Vector2(0.421f, 0.524f), new Vector2(0.471f, 0.486f), new Vector2(0.547f, 0.421f), new Vector2(0.594f, 0.388f), new Vector2(0.649f, 0.372f), new Vector2(0.712f, 0.383f), new Vector2(0.768f, 0.426f), new Vector2(0.824f, 0.479f) } }; } return char_tilde; }
        Vector3[][] Char_degree() { if (char_degree == null) { char_degree = new Vector3[][] { new Vector3[] { new Vector2(0.531f, 0.767f), new Vector2(0.657f, 0.822f), new Vector2(0.714f, 0.95f), new Vector2(0.659f, 1.088f), new Vector2(0.524f, 1.132f), new Vector2(0.377f, 1.069f), new Vector2(0.3349999f, 0.959f), new Vector2(0.392f, 0.819f), new Vector2(0.531f, 0.767f) } }; } return char_degree; }
        Vector3[][] Char_section() { if (char_section == null) { char_section = new Vector3[][] { new Vector3[] { new Vector2(0.807f, 0.835f), new Vector2(0.707f, 0.95f), new Vector2(0.441f, 0.948f), new Vector2(0.357f, 0.89f), new Vector2(0.326f, 0.801f), new Vector2(0.366f, 0.704f), new Vector2(0.723f, 0.5f), new Vector2(0.749f, 0.327f), new Vector2(0.648f, 0.187f) }, new Vector3[] { new Vector2(0.256f, 0.057f), new Vector2(0.346f, -0.071f), new Vector2(0.58f, -0.072f), new Vector2(0.69f, -0.023f), new Vector2(0.733f, 0.076f), new Vector2(0.671f, 0.167f), new Vector2(0.322f, 0.356f), new Vector2(0.304f, 0.53f), new Vector2(0.398f, 0.677f) } }; } return char_section; }


        Vector3[][] Icon_profileFoto() { if (icon_profileFoto == null) { icon_profileFoto = new Vector3[][] { new Vector3[] { new Vector2(0.143f, 0.223f), new Vector2(0.213f, 0.313f), new Vector2(0.286f, 0.385f), new Vector2(0.405f, 0.447f) }, new Vector3[] { new Vector2(0.85f, 0.229f), new Vector2(0.796f, 0.314f), new Vector2(0.712f, 0.385f), new Vector2(0.586f, 0.448f) }, new Vector3[] { new Vector2(0.4985f, 0.05678758f), new Vector2(0.31975f, 0.09226254f), new Vector2(0.1865f, 0.177725f), new Vector2(0.09062499f, 0.3115626f), new Vector2(0.04674998f, 0.497f), new Vector2(0.07925001f, 0.6711501f), new Vector2(0.191375f, 0.8324f), new Vector2(0.32625f, 0.9178625f), new Vector2(0.505f, 0.9485f), new Vector2(0.6983749f, 0.9049625f), new Vector2(0.83325f, 0.8017625f), new Vector2(0.919375f, 0.6630876f), new Vector2(0.947f, 0.497f), new Vector2(0.9063749f, 0.3131751f), new Vector2(0.815375f, 0.1825626f), new Vector2(0.674f, 0.09387508f), new Vector2(0.4985f, 0.05678758f) }, new Vector3[] { new Vector2(0.492292f, 0.432179f), new Vector2(0.417822f, 0.447073f), new Vector2(0.362308f, 0.482954f), new Vector2(0.322365f, 0.539145f), new Vector2(0.304086f, 0.617f), new Vector2(0.317626f, 0.690116f), new Vector2(0.364339f, 0.757816f), new Vector2(0.42053f, 0.793697f), new Vector2(0.495f, 0.80656f), new Vector2(0.575563f, 0.788281f), new Vector2(0.631754f, 0.744953f), new Vector2(0.667635f, 0.686731f), new Vector2(0.679144f, 0.617f), new Vector2(0.662219f, 0.539822f), new Vector2(0.624307f, 0.484985f), new Vector2(0.565408f, 0.44775f), new Vector2(0.492292f, 0.432179f) } }; } return icon_profileFoto; }
        Vector3[][] Icon_imageLandscape() { if (icon_imageLandscape == null) { icon_imageLandscape = new Vector3[][] { new Vector3[] { new Vector2(0.0373444f, 0.813278f), new Vector2(0.06224066f, 0.8381743f), new Vector2(0.9294606f, 0.8381743f), new Vector2(0.9522821f, 0.8153527f), new Vector2(0.9522821f, 0.1659751f), new Vector2(0.9190871f, 0.1369295f), new Vector2(0.06016598f, 0.1369295f), new Vector2(0.03319502f, 0.1680498f), new Vector2(0.03319502f, 0.8153527f) }, new Vector3[] { new Vector2(0.2738589f, 0.1473029f), new Vector2(0.4979253f, 0.373444f), new Vector2(0.5788382f, 0.4253112f), new Vector2(0.6721992f, 0.4626556f), new Vector2(0.7697095f, 0.466805f), new Vector2(0.8651452f, 0.4460581f), new Vector2(0.9502075f, 0.3692946f) }, new Vector3[] { new Vector2(0.03319502f, 0.3443983f), new Vector2(0.1161826f, 0.4024896f), new Vector2(0.1742739f, 0.4087137f), new Vector2(0.2489627f, 0.3672199f), new Vector2(0.3319502f, 0.2904564f) }, new Vector3[] { new Vector2(0.246888f, 0.4232365f), new Vector2(0.3153527f, 0.4834025f), new Vector2(0.3817427f, 0.5145229f), new Vector2(0.4522822f, 0.5103735f), new Vector2(0.5020747f, 0.4771784f), new Vector2(0.5394191f, 0.43361f) }, new Vector3[] { new Vector2(0.2945f, 0.6286171f), new Vector2(0.2474835f, 0.6481118f), new Vector2(0.2289062f, 0.691f), new Vector2(0.2467954f, 0.7359524f), new Vector2(0.2933533f, 0.7559057f), new Vector2(0.3383056f, 0.7380165f), new Vector2(0.3564242f, 0.6907706f), new Vector2(0.3380763f, 0.6460476f), new Vector2(0.2945f, 0.6286171f) }, new Vector3[] { new Vector2(0.9375352f, 0.8609958f), new Vector2(0.973029f, 0.8236514f), new Vector2(0.973029f, 0.1576764f), new Vector2(0.9211618f, 0.1161079f), new Vector2(0.04979253f, 0.1161079f), new Vector2(0.01244813f, 0.1639004f), new Vector2(0.01244813f, 0.8195021f), new Vector2(0.06016598f, 0.8569958f), new Vector2(0.9356846f, 0.8569958f) } }; } return icon_imageLandscape; }
        Vector3[][] Icon_homeHouse() { if (icon_homeHouse == null) { icon_homeHouse = new Vector3[][] { new Vector3[] { new Vector2(0.5106209f, 0.9084967f), new Vector2(0.03349673f, 0.4493464f), new Vector2(0.1789216f, 0.4493464f), new Vector2(0.1789216f, 0.08333334f), new Vector2(0.4256536f, 0.08333334f), new Vector2(0.4256536f, 0.3039216f), new Vector2(0.5825163f, 0.3039216f), new Vector2(0.5825163f, 0.08333334f), new Vector2(0.8276144f, 0.08333334f), new Vector2(0.8276144f, 0.4509804f), new Vector2(0.9714052f, 0.4509804f), new Vector2(0.5106209f, 0.9084967f) } }; } return icon_homeHouse; }
        Vector3[][] Icon_dataDisc() { if (icon_dataDisc == null) { icon_dataDisc = new Vector3[][] { new Vector3[] { new Vector2(0.4256536f, 0.245098f), new Vector2(0.5923203f, 0.245098f) }, new Vector3[] { new Vector2(0.4256536f, 0.1748366f), new Vector2(0.5906863f, 0.1748366f) }, new Vector3[] { new Vector2(0.4256536f, 0.1013072f), new Vector2(0.5890523f, 0.1013072f) }, new Vector3[] { new Vector2(0.3472222f, 0.0506536f), new Vector2(0.3472222f, 0.2973856f), new Vector2(0.6674837f, 0.2973856f), new Vector2(0.6674837f, 0.0506536f) }, new Vector3[] { new Vector2(0.7933006f, 0.5375817f), new Vector2(0.7933006f, 0.05228758f), new Vector2(0.2197712f, 0.05228758f), new Vector2(0.2197712f, 0.6715686f), new Vector2(0.6740196f, 0.6715686f), new Vector2(0.7933006f, 0.5375817f) } }; } return icon_dataDisc; }
        Vector3[][] Icon_saveData() { if (icon_saveData == null) { icon_saveData = new Vector3[][] { new Vector3[] { new Vector2(0.4256536f, 0.245098f), new Vector2(0.5923203f, 0.245098f) }, new Vector3[] { new Vector2(0.4256536f, 0.1748366f), new Vector2(0.5906863f, 0.1748366f) }, new Vector3[] { new Vector2(0.4256536f, 0.1013072f), new Vector2(0.5890523f, 0.1013072f) }, new Vector3[] { new Vector2(0.3472222f, 0.0506536f), new Vector2(0.3472222f, 0.2973856f), new Vector2(0.6674837f, 0.2973856f), new Vector2(0.6674837f, 0.0506536f) }, new Vector3[] { new Vector2(0.7933006f, 0.5375817f), new Vector2(0.7933006f, 0.05228758f), new Vector2(0.2197712f, 0.05228758f), new Vector2(0.2197712f, 0.6715686f), new Vector2(0.4125817f, 0.6715686f) }, new Vector3[] { new Vector2(0.5972222f, 0.6699346f), new Vector2(0.6707516f, 0.6699346f), new Vector2(0.7933006f, 0.5359477f) }, new Vector3[] { new Vector2(0.499183f, 0.9787582f), new Vector2(0.499183f, 0.3676471f) }, new Vector3[] { new Vector2(0.3325163f, 0.5473856f), new Vector2(0.499183f, 0.3643791f), new Vector2(0.6642157f, 0.5424837f) } }; } return icon_saveData; }
        Vector3[][] Icon_loadData() { if (icon_loadData == null) { icon_loadData = new Vector3[][] { new Vector3[] { new Vector2(0.4256536f, 0.245098f), new Vector2(0.5923203f, 0.245098f) }, new Vector3[] { new Vector2(0.4256536f, 0.1748366f), new Vector2(0.5906863f, 0.1748366f) }, new Vector3[] { new Vector2(0.4256536f, 0.1013072f), new Vector2(0.5890523f, 0.1013072f) }, new Vector3[] { new Vector2(0.3472222f, 0.0506536f), new Vector2(0.3472222f, 0.2973856f), new Vector2(0.6674837f, 0.2973856f), new Vector2(0.6674837f, 0.0506536f) }, new Vector3[] { new Vector2(0.7933006f, 0.5375817f), new Vector2(0.7933006f, 0.05228758f), new Vector2(0.2197712f, 0.05228758f), new Vector2(0.2197712f, 0.6715686f), new Vector2(0.4027778f, 0.6715686f) }, new Vector3[] { new Vector2(0.6053922f, 0.6699346f), new Vector2(0.6740196f, 0.6699346f), new Vector2(0.7949346f, 0.5359477f) }, new Vector3[] { new Vector2(0.5057189f, 0.3692811f), new Vector2(0.5057189f, 0.9558824f) }, new Vector3[] { new Vector2(0.3357843f, 0.7826797f), new Vector2(0.5040849f, 0.9591503f), new Vector2(0.6674837f, 0.7810457f) } }; } return icon_loadData; }
        Vector3[][] Icon_speechBubble() { if (icon_speechBubble == null) { icon_speechBubble = new Vector3[][] { new Vector3[] { new Vector2(0.5f, 0.5f), new Vector2(0.7271242f, 0.7647059f), new Vector2(0.6748366f, 0.7745098f), new Vector2(0.6307189f, 0.8055556f), new Vector2(0.5964052f, 0.8431373f), new Vector2(0.5816994f, 0.8954248f), new Vector2(0.5816994f, 1.27451f), new Vector2(0.5947713f, 1.330065f), new Vector2(0.624183f, 1.370915f), new Vector2(0.6683006f, 1.397059f), new Vector2(0.7336601f, 1.408497f), new Vector2(1.285948f, 1.408497f), new Vector2(1.346405f, 1.392157f), new Vector2(1.388889f, 1.356209f), new Vector2(1.416667f, 1.315359f), new Vector2(1.418301f, 1.263072f), new Vector2(1.418301f, 0.8954248f), new Vector2(1.406863f, 0.8398693f), new Vector2(1.377451f, 0.8039216f), new Vector2(1.334967f, 0.7777778f), new Vector2(1.27451f, 0.7630719f), new Vector2(0.995098f, 0.7630719f), new Vector2(0.5f, 0.5f) }, new Vector3[] { new Vector2(0.6846405f, 1.253268f), new Vector2(1.313725f, 1.253268f) }, new Vector3[] { new Vector2(0.6879085f, 1.091503f), new Vector2(1.312092f, 1.091503f) }, new Vector3[] { new Vector2(0.6862745f, 0.9199346f), new Vector2(1.143791f, 0.9199346f) } }; } return icon_speechBubble; }
        Vector3[][] Icon_speechBubbleEmpty() { if (icon_speechBubbleEmpty == null) { icon_speechBubbleEmpty = new Vector3[][] { new Vector3[] { new Vector2(0.5f, 0.5f), new Vector2(0.7271242f, 0.7647059f), new Vector2(0.6748366f, 0.7745098f), new Vector2(0.6307189f, 0.8055556f), new Vector2(0.5964052f, 0.8431373f), new Vector2(0.5816994f, 0.8954248f), new Vector2(0.5816994f, 1.27451f), new Vector2(0.5947713f, 1.330065f), new Vector2(0.624183f, 1.370915f), new Vector2(0.6683006f, 1.397059f), new Vector2(0.7336601f, 1.408497f), new Vector2(1.285948f, 1.408497f), new Vector2(1.346405f, 1.392157f), new Vector2(1.388889f, 1.356209f), new Vector2(1.416667f, 1.315359f), new Vector2(1.418301f, 1.263072f), new Vector2(1.418301f, 0.8954248f), new Vector2(1.406863f, 0.8398693f), new Vector2(1.377451f, 0.8039216f), new Vector2(1.334967f, 0.7777778f), new Vector2(1.27451f, 0.7630719f), new Vector2(0.995098f, 0.7630719f), new Vector2(0.5f, 0.5f) } }; } return icon_speechBubbleEmpty; }
        Vector3[][] Icon_thumbUp() { if (icon_thumbUp == null) { icon_thumbUp = new Vector3[][] { new Vector3[] { new Vector2(0.2957516f, 0.1143791f), new Vector2(0.2957516f, 0.5130719f), new Vector2(0.3627451f, 0.5359477f), new Vector2(0.4313726f, 0.7107843f), new Vector2(0.4493464f, 0.9509804f), new Vector2(0.5228758f, 0.9460784f), new Vector2(0.5718954f, 0.9150327f), new Vector2(0.5980392f, 0.8594771f), new Vector2(0.6013072f, 0.7973856f), new Vector2(0.5915033f, 0.6486928f), new Vector2(0.8839869f, 0.6486928f), new Vector2(0.9428105f, 0.622549f), new Vector2(0.9346405f, 0.5245098f), new Vector2(0.8578432f, 0.4918301f), new Vector2(0.9232026f, 0.4542484f), new Vector2(0.9117647f, 0.3660131f), new Vector2(0.8137255f, 0.3300654f), new Vector2(0.875817f, 0.2990196f), new Vector2(0.8676471f, 0.1960784f), new Vector2(0.7712418f, 0.1764706f), new Vector2(0.8349673f, 0.1323529f), new Vector2(0.8251634f, 0.05882353f), new Vector2(0.7892157f, 0.03921569f), new Vector2(0.4918301f, 0.03921569f), new Vector2(0.3611111f, 0.1078431f), new Vector2(0.2957516f, 0.1143791f) }, new Vector3[] { new Vector2(0.03921569f, 0.5686275f), new Vector2(0.2647059f, 0.5686275f), new Vector2(0.2647059f, 0.04575163f), new Vector2(0.03921569f, 0.04575163f), new Vector2(0.03921569f, 0.5669935f) }, new Vector3[] { new Vector2(0.4444444f, 0.8382353f), new Vector2(0.4836601f, 0.8611111f), new Vector2(0.4918301f, 0.9444444f) } }; } return icon_thumbUp; }
        Vector3[][] Icon_thumbDown() { if (icon_thumbDown == null) { icon_thumbDown = new Vector3[][] { new Vector3[] { new Vector2(0.7042483f, 0.885621f), new Vector2(0.7042484f, 0.4869281f), new Vector2(0.6372549f, 0.4640523f), new Vector2(0.5686275f, 0.2892157f), new Vector2(0.5506536f, 0.04901963f), new Vector2(0.4771242f, 0.05392158f), new Vector2(0.4281046f, 0.08496732f), new Vector2(0.4019608f, 0.1405229f), new Vector2(0.3986928f, 0.2026144f), new Vector2(0.4084967f, 0.3513072f), new Vector2(0.1160131f, 0.3513072f), new Vector2(0.05718952f, 0.377451f), new Vector2(0.06535947f, 0.4754902f), new Vector2(0.1421568f, 0.5081699f), new Vector2(0.07679737f, 0.5457516f), new Vector2(0.08823532f, 0.6339869f), new Vector2(0.1862745f, 0.6699346f), new Vector2(0.124183f, 0.7009804f), new Vector2(0.1323529f, 0.8039216f), new Vector2(0.2287581f, 0.8235294f), new Vector2(0.1650327f, 0.8676471f), new Vector2(0.1748365f, 0.9411764f), new Vector2(0.2107843f, 0.9607843f), new Vector2(0.5081699f, 0.9607843f), new Vector2(0.6388888f, 0.8921568f), new Vector2(0.7042483f, 0.885621f) }, new Vector3[] { new Vector2(0.9607843f, 0.4313726f), new Vector2(0.7352941f, 0.4313726f), new Vector2(0.735294f, 0.9542484f), new Vector2(0.9607843f, 0.9542484f), new Vector2(0.9607843f, 0.4330066f) }, new Vector3[] { new Vector2(0.5555556f, 0.1617647f), new Vector2(0.5163399f, 0.1388889f), new Vector2(0.50817f, 0.05555558f) } }; } return icon_thumbDown; }
        Vector3[][] Icon_lightBulbOn() { if (icon_lightBulbOn == null) { icon_lightBulbOn = new Vector3[][] { new Vector3[] { new Vector2(0.3954248f, 0.2679739f), new Vector2(0.3905229f, 0.3251634f), new Vector2(0.3643791f, 0.3954248f), new Vector2(0.3251634f, 0.4689542f), new Vector2(0.3006536f, 0.5310457f), new Vector2(0.2957516f, 0.5931373f), new Vector2(0.3153595f, 0.6601307f), new Vector2(0.3529412f, 0.7189543f), new Vector2(0.4101307f, 0.7549019f), new Vector2(0.4738562f, 0.7745098f), new Vector2(0.5408497f, 0.7712418f), new Vector2(0.6045752f, 0.746732f), new Vector2(0.6503268f, 0.6993464f), new Vector2(0.6862745f, 0.6421568f), new Vector2(0.6977124f, 0.5767974f), new Vector2(0.6879085f, 0.5081699f), new Vector2(0.6617647f, 0.4558823f), new Vector2(0.6307189f, 0.3888889f), new Vector2(0.6062092f, 0.3284314f), new Vector2(0.5996732f, 0.2679739f) }, new Vector3[] { new Vector2(0.3823529f, 0.2009804f), new Vector2(0.3823529f, 0.245098f), new Vector2(0.624183f, 0.245098f), new Vector2(0.624183f, 0.1977124f), new Vector2(0.3807189f, 0.1977124f) }, new Vector3[] { new Vector2(0.3839869f, 0.1699346f), new Vector2(0.624183f, 0.1699346f), new Vector2(0.624183f, 0.124183f), new Vector2(0.3839869f, 0.124183f), new Vector2(0.3839869f, 0.1683007f) }, new Vector3[] { new Vector2(0.4297386f, 0.0882353f), new Vector2(0.5653595f, 0.0882353f), new Vector2(0.5359477f, 0.04084967f), new Vector2(0.4575163f, 0.04084967f), new Vector2(0.4297386f, 0.0882353f) }, new Vector3[] { new Vector2(0.4117647f, 0.5114379f), new Vector2(0.4362745f, 0.6356209f), new Vector2(0.4722222f, 0.503268f), new Vector2(0.5f, 0.6356209f), new Vector2(0.5310457f, 0.503268f), new Vector2(0.5620915f, 0.6339869f), new Vector2(0.5882353f, 0.5049019f) }, new Vector3[] { new Vector2(0.7761438f, 0.495098f), new Vector2(0.9150327f, 0.4509804f) }, new Vector3[] { new Vector2(0.7696078f, 0.6519608f), new Vector2(0.9084967f, 0.7009804f) }, new Vector3[] { new Vector2(0.6666667f, 0.7892157f), new Vector2(0.7614379f, 0.9052287f) }, new Vector3[] { new Vector2(0.501634f, 0.8480392f), new Vector2(0.501634f, 0.9869281f) }, new Vector3[] { new Vector2(0.3284314f, 0.7908497f), new Vector2(0.2369281f, 0.9003268f) }, new Vector3[] { new Vector2(0.2287582f, 0.6552287f), new Vector2(0.0882353f, 0.6960784f) }, new Vector3[] { new Vector2(0.2254902f, 0.4934641f), new Vector2(0.08006536f, 0.4493464f) } }; } return icon_lightBulbOn; }
        Vector3[][] Icon_lightBulbOff() { if (icon_lightBulbOff == null) { icon_lightBulbOff = new Vector3[][] { new Vector3[] { new Vector2(0.3954248f, 0.2679739f), new Vector2(0.3905229f, 0.3251634f), new Vector2(0.3643791f, 0.3954248f), new Vector2(0.3251634f, 0.4689542f), new Vector2(0.3006536f, 0.5310457f), new Vector2(0.2957516f, 0.5931373f), new Vector2(0.3153595f, 0.6601307f), new Vector2(0.3529412f, 0.7189543f), new Vector2(0.4101307f, 0.7549019f), new Vector2(0.4738562f, 0.7745098f), new Vector2(0.5408497f, 0.7712418f), new Vector2(0.6045752f, 0.746732f), new Vector2(0.6503268f, 0.6993464f), new Vector2(0.6862745f, 0.6421568f), new Vector2(0.6977124f, 0.5767974f), new Vector2(0.6879085f, 0.5081699f), new Vector2(0.6617647f, 0.4558823f), new Vector2(0.6307189f, 0.3888889f), new Vector2(0.6062092f, 0.3284314f), new Vector2(0.5996732f, 0.2679739f) }, new Vector3[] { new Vector2(0.3823529f, 0.2009804f), new Vector2(0.3823529f, 0.245098f), new Vector2(0.624183f, 0.245098f), new Vector2(0.624183f, 0.1977124f), new Vector2(0.3807189f, 0.1977124f) }, new Vector3[] { new Vector2(0.3839869f, 0.1699346f), new Vector2(0.624183f, 0.1699346f), new Vector2(0.624183f, 0.124183f), new Vector2(0.3839869f, 0.124183f), new Vector2(0.3839869f, 0.1683007f) }, new Vector3[] { new Vector2(0.4297386f, 0.0882353f), new Vector2(0.5653595f, 0.0882353f), new Vector2(0.5359477f, 0.04084967f), new Vector2(0.4575163f, 0.04084967f), new Vector2(0.4297386f, 0.0882353f) } }; } return icon_lightBulbOff; }
        Vector3[][] Icon_videoCamera() { if (icon_videoCamera == null) { icon_videoCamera = new Vector3[][] { new Vector3[] { new Vector2(0.1830065f, 0.5996732f), new Vector2(0.1339869f, 0.6470588f), new Vector2(0.1127451f, 0.7042484f), new Vector2(0.1176471f, 0.7777778f), new Vector2(0.1584967f, 0.8366013f), new Vector2(0.2140523f, 0.874183f), new Vector2(0.3055556f, 0.8709151f), new Vector2(0.3496732f, 0.8480392f), new Vector2(0.3807189f, 0.8088235f), new Vector2(0.4150327f, 0.8496732f), new Vector2(0.4820261f, 0.877451f), new Vector2(0.5571895f, 0.8643791f), new Vector2(0.6209151f, 0.8169935f), new Vector2(0.6519608f, 0.7418301f), new Vector2(0.6454248f, 0.6764706f), new Vector2(0.6176471f, 0.6339869f), new Vector2(0.5702614f, 0.5947713f) }, new Vector3[] { new Vector2(0.6748366f, 0.5964052f), new Vector2(0.08006536f, 0.5964052f), new Vector2(0.08006536f, 0.1666667f), new Vector2(0.6732026f, 0.1666667f), new Vector2(0.6748366f, 0.5964052f) }, new Vector3[] { new Vector2(0.6748366f, 0.4787582f), new Vector2(0.9281046f, 0.5931373f), new Vector2(0.9281046f, 0.1633987f), new Vector2(0.6732026f, 0.2663399f) } }; } return icon_videoCamera; }
        Vector3[][] Icon_camera() { if (icon_camera == null) { icon_camera = new Vector3[][] { new Vector3[] { new Vector2(0.5015236f, 0.2749854f), new Vector2(0.4334225f, 0.2886056f), new Vector2(0.3826562f, 0.321418f), new Vector2(0.3461292f, 0.3728034f), new Vector2(0.3294135f, 0.444f), new Vector2(0.3417955f, 0.5108629f), new Vector2(0.3845135f, 0.572773f), new Vector2(0.4358989f, 0.6055854f), new Vector2(0.504f, 0.6173483f), new Vector2(0.577673f, 0.6006326f), new Vector2(0.6290585f, 0.5610101f), new Vector2(0.6618708f, 0.5077674f), new Vector2(0.6723955f, 0.444f), new Vector2(0.656918f, 0.3734225f), new Vector2(0.6222483f, 0.3232753f), new Vector2(0.5683866f, 0.2892247f), new Vector2(0.5015236f, 0.2749854f) }, new Vector3[] { new Vector2(0.748366f, 0.7205882f), new Vector2(0.8905229f, 0.7205882f), new Vector2(0.9330065f, 0.6813725f), new Vector2(0.9330065f, 0.1715686f), new Vector2(0.8921568f, 0.130719f), new Vector2(0.1045752f, 0.130719f), new Vector2(0.06045752f, 0.1781046f), new Vector2(0.06045752f, 0.6797386f), new Vector2(0.1029412f, 0.7189543f), new Vector2(0.253268f, 0.7189543f), new Vector2(0.3366013f, 0.8464052f), new Vector2(0.6732026f, 0.8464052f), new Vector2(0.748366f, 0.7205882f) } }; } return icon_camera; }
        Vector3[][] Icon_music() { if (icon_music == null) { icon_music = new Vector3[][] { new Vector3[] { new Vector2(0.8464052f, 0.377451f), new Vector2(0.8006536f, 0.3839869f), new Vector2(0.7254902f, 0.3807189f), new Vector2(0.6470588f, 0.3480392f), new Vector2(0.6094771f, 0.2957516f), new Vector2(0.6045752f, 0.2205882f), new Vector2(0.6650327f, 0.1797386f), new Vector2(0.75f, 0.1666667f), new Vector2(0.8267974f, 0.1879085f), new Vector2(0.8839869f, 0.2173203f), new Vector2(0.9101307f, 0.2630719f), new Vector2(0.9150327f, 0.3284314f), new Vector2(0.9150327f, 0.9313725f), new Vector2(0.3104575f, 0.7859477f), new Vector2(0.3104575f, 0.2777778f), new Vector2(0.2303922f, 0.2761438f), new Vector2(0.1437909f, 0.245098f), new Vector2(0.08496732f, 0.1993464f), new Vector2(0.06535948f, 0.1339869f), new Vector2(0.09640523f, 0.0882353f), new Vector2(0.1781046f, 0.06045752f), new Vector2(0.2712418f, 0.06862745f), new Vector2(0.3513072f, 0.1143791f), new Vector2(0.3807189f, 0.1519608f), new Vector2(0.3888889f, 0.2156863f), new Vector2(0.3888889f, 0.6519608f), new Vector2(0.8447713f, 0.759804f), new Vector2(0.8447713f, 0.375817f) } }; } return icon_music; }
        Vector3[][] Icon_audioSpeaker() { if (icon_audioSpeaker == null) { icon_audioSpeaker = new Vector3[][] { new Vector3[] { new Vector2(0.3284314f, 0.6405229f), new Vector2(0.06045752f, 0.6405229f), new Vector2(0.06045752f, 0.3562092f), new Vector2(0.3284314f, 0.3562092f), new Vector2(0.509804f, 0.1846405f), new Vector2(0.509804f, 0.8202614f), new Vector2(0.3284314f, 0.6405229f) }, new Vector3[] { new Vector2(0.5996732f, 0.7173203f), new Vector2(0.6323529f, 0.6388889f), new Vector2(0.6503268f, 0.5506536f), new Vector2(0.6486928f, 0.4444444f), new Vector2(0.625817f, 0.3431373f), new Vector2(0.5898693f, 0.2614379f) }, new Vector3[] { new Vector2(0.7042484f, 0.7843137f), new Vector2(0.7450981f, 0.6846405f), new Vector2(0.7696078f, 0.5686275f), new Vector2(0.7712418f, 0.4493464f), new Vector2(0.7434641f, 0.3218954f), new Vector2(0.6862745f, 0.1977124f) }, new Vector3[] { new Vector2(0.8088235f, 0.8464052f), new Vector2(0.8513072f, 0.753268f), new Vector2(0.8856209f, 0.6519608f), new Vector2(0.8986928f, 0.5490196f), new Vector2(0.8986928f, 0.4330065f), new Vector2(0.8790849f, 0.3284314f), new Vector2(0.8431373f, 0.2254902f), new Vector2(0.7924837f, 0.130719f) } }; } return icon_audioSpeaker; }
        Vector3[][] Icon_microphone() { if (icon_microphone == null) { icon_microphone = new Vector3[][] { new Vector3[] { new Vector2(0.3349673f, 0.8169935f), new Vector2(0.3594771f, 0.872549f), new Vector2(0.3970588f, 0.9084967f), new Vector2(0.4477124f, 0.9379085f), new Vector2(0.501634f, 0.9411765f), new Vector2(0.5620915f, 0.9264706f), new Vector2(0.6029412f, 0.8970588f), new Vector2(0.6388889f, 0.8529412f), new Vector2(0.6552287f, 0.7957516f), new Vector2(0.6552287f, 0.4150327f), new Vector2(0.6454248f, 0.3676471f), new Vector2(0.622549f, 0.3300654f), new Vector2(0.5898693f, 0.2957516f), new Vector2(0.5457516f, 0.2712418f), new Vector2(0.495098f, 0.2679739f), new Vector2(0.4330065f, 0.2843137f), new Vector2(0.3807189f, 0.3186274f), new Vector2(0.3496732f, 0.3643791f), new Vector2(0.3333333f, 0.4330065f), new Vector2(0.3349673f, 0.8169935f) }, new Vector3[] { new Vector2(0.2352941f, 0.6029412f), new Vector2(0.2352941f, 0.3905229f), new Vector2(0.254902f, 0.3218954f), new Vector2(0.2990196f, 0.254902f), new Vector2(0.3660131f, 0.1960784f), new Vector2(0.4526144f, 0.1699346f), new Vector2(0.5653595f, 0.1699346f), new Vector2(0.6454248f, 0.2075163f), new Vector2(0.7058824f, 0.2647059f), new Vector2(0.7450981f, 0.3349673f), new Vector2(0.7679738f, 0.4183007f), new Vector2(0.7679738f, 0.6029412f) }, new Vector3[] { new Vector2(0.3316993f, 0.06045752f), new Vector2(0.6715686f, 0.06045752f) }, new Vector3[] { new Vector2(0.5f, 0.0620915f), new Vector2(0.5f, 0.1699346f) } }; } return icon_microphone; }
        Vector3[][] Icon_wlan_wifi() { if (icon_wlan_wifi == null) { icon_wlan_wifi = new Vector3[][] { new Vector3[] { new Vector2(0.504f, 0.07238549f), new Vector2(0.4326913f, 0.1019525f), new Vector2(0.4045156f, 0.167f), new Vector2(0.4316477f, 0.2351781f), new Vector2(0.5022607f, 0.2654408f), new Vector2(0.5704389f, 0.2383087f), new Vector2(0.5979188f, 0.1666522f), new Vector2(0.570091f, 0.0988219f), new Vector2(0.504f, 0.07238549f) }, new Vector3[] { new Vector2(0.2785124f, 0.3636364f), new Vector2(0.3512397f, 0.4165289f), new Vector2(0.438843f, 0.4479339f), new Vector2(0.5330579f, 0.4479339f), new Vector2(0.6157025f, 0.4247934f), new Vector2(0.6884298f, 0.3818182f), new Vector2(0.7148761f, 0.3586777f) }, new Vector3[] { new Vector2(0.1561984f, 0.4809917f), new Vector2(0.2355372f, 0.5438017f), new Vector2(0.3297521f, 0.5933884f), new Vector2(0.4289256f, 0.6181818f), new Vector2(0.5181818f, 0.6231405f), new Vector2(0.6157025f, 0.6066115f), new Vector2(0.7016529f, 0.5752066f), new Vector2(0.7859504f, 0.5206612f), new Vector2(0.8421488f, 0.477686f) }, new Vector3[] { new Vector2(0.04214876f, 0.6099173f), new Vector2(0.1264463f, 0.6793389f), new Vector2(0.2140496f, 0.7305785f), new Vector2(0.3231405f, 0.7702479f), new Vector2(0.4256198f, 0.7900826f), new Vector2(0.5561984f, 0.7917355f), new Vector2(0.6669421f, 0.7702479f), new Vector2(0.7776859f, 0.7256199f), new Vector2(0.8801653f, 0.6661157f), new Vector2(0.9595041f, 0.5933884f) } }; } return icon_wlan_wifi; }
        Vector3[][] Icon_share() { if (icon_share == null) { icon_share = new Vector3[][] { new Vector3[] { new Vector2(0.4428104f, 0.6176471f), new Vector2(0.3022876f, 0.6176471f), new Vector2(0.3022876f, 0.05392157f), new Vector2(0.6797386f, 0.05392157f), new Vector2(0.6797386f, 0.6192811f), new Vector2(0.5359477f, 0.6192811f) }, new Vector3[] { new Vector2(0.4918301f, 0.3366013f), new Vector2(0.4918301f, 0.7140523f), new Vector2(0.5081699f, 0.7777778f), new Vector2(0.5457516f, 0.8333333f), new Vector2(0.5849673f, 0.8643791f), new Vector2(0.6339869f, 0.877451f), new Vector2(0.7320262f, 0.877451f) }, new Vector3[] { new Vector2(0.6192811f, 0.9787582f), new Vector2(0.7303922f, 0.875817f), new Vector2(0.6160131f, 0.7761438f) } }; } return icon_share; }
        Vector3[][] Icon_timeClock() { if (icon_timeClock == null) { icon_timeClock = new Vector3[][] { new Vector3[] { new Vector2(0.500192f, 0.03435445f), new Vector2(0.3129722f, 0.07179838f), new Vector2(0.1734083f, 0.1620043f), new Vector2(0.07299039f, 0.3032703f), new Vector2(0.02703643f, 0.499f), new Vector2(0.0610764f, 0.6828159f), new Vector2(0.1785143f, 0.8530157f), new Vector2(0.3197802f, 0.9432217f), new Vector2(0.507f, 0.9755597f), new Vector2(0.7095378f, 0.9296057f), new Vector2(0.8508038f, 0.8206778f), new Vector2(0.9410096f, 0.6743059f), new Vector2(0.9699436f, 0.499f), new Vector2(0.9273937f, 0.3049722f), new Vector2(0.8320818f, 0.1671104f), new Vector2(0.6840079f, 0.07350042f), new Vector2(0.500192f, 0.03435445f) }, new Vector3[] { new Vector2(0.4999412f, 0.08449066f), new Vector2(0.3333257f, 0.1178137f), new Vector2(0.2091215f, 0.1980921f), new Vector2(0.119755f, 0.3238111f), new Vector2(0.07885844f, 0.498f), new Vector2(0.1091522f, 0.6615862f), new Vector2(0.2136655f, 0.8130548f), new Vector2(0.3393845f, 0.8933332f), new Vector2(0.506f, 0.9221122f), new Vector2(0.6862476f, 0.8812157f), new Vector2(0.8119667f, 0.7842758f), new Vector2(0.892245f, 0.6540127f), new Vector2(0.9179947f, 0.498f), new Vector2(0.8801275f, 0.3253258f), new Vector2(0.795305f, 0.2026362f), new Vector2(0.6635273f, 0.1193285f), new Vector2(0.4999412f, 0.08449066f) }, new Vector3[] { new Vector2(0.49f, 0.4291129f), new Vector2(0.4576771f, 0.4425152f), new Vector2(0.4449055f, 0.472f), new Vector2(0.457204f, 0.5029039f), new Vector2(0.4892116f, 0.5166215f), new Vector2(0.5201156f, 0.5043229f), new Vector2(0.5325717f, 0.4718423f), new Vector2(0.5199579f, 0.4410961f), new Vector2(0.49f, 0.4291129f) }, new Vector3[] { new Vector2(0.4851562f, 0.8375f), new Vector2(0.4851562f, 0.4734375f), new Vector2(0.7929688f, 0.3453125f) }, new Vector3[] { new Vector2(0.07966616f, 0.4992413f), new Vector2(0.146434f, 0.4992413f) }, new Vector3[] { new Vector2(0.1418816f, 0.7086495f), new Vector2(0.1949924f, 0.6737481f) }, new Vector3[] { new Vector2(0.2936267f, 0.8634294f), new Vector2(0.3194234f, 0.8072838f) }, new Vector3[] { new Vector2(0.4908953f, 0.9210926f), new Vector2(0.4908953f, 0.8694993f) }, new Vector3[] { new Vector2(0.7063733f, 0.8634294f), new Vector2(0.6760243f, 0.8088012f) }, new Vector3[] { new Vector2(0.8566009f, 0.7086495f), new Vector2(0.8034902f, 0.6752656f) }, new Vector3[] { new Vector2(0.9172989f, 0.5022762f), new Vector2(0.8505311f, 0.5022762f) }, new Vector3[] { new Vector2(0.8566009f, 0.2943854f), new Vector2(0.8141123f, 0.3216995f) }, new Vector3[] { new Vector2(0.698786f, 0.1456753f), new Vector2(0.6699545f, 0.1972686f) }, new Vector3[] { new Vector2(0.4984826f, 0.08497724f), new Vector2(0.4984826f, 0.1471927f) }, new Vector3[] { new Vector2(0.2921093f, 0.1441578f), new Vector2(0.3270106f, 0.1972686f) }, new Vector3[] { new Vector2(0.1418816f, 0.2943854f), new Vector2(0.2025797f, 0.3292868f) } }; } return icon_timeClock; }
        Vector3[][] Icon_telephone() { if (icon_telephone == null) { icon_telephone = new Vector3[][] { new Vector3[] { new Vector2(0.4526144f, 0.7369281f), new Vector2(0.3137255f, 0.6405229f), new Vector2(0.3251634f, 0.5784314f), new Vector2(0.5147059f, 0.3088235f), new Vector2(0.5735294f, 0.2843137f), new Vector2(0.7091503f, 0.377451f), new Vector2(0.8839869f, 0.2434641f), new Vector2(0.8333333f, 0.1437909f), new Vector2(0.7549019f, 0.08496732f), new Vector2(0.6486928f, 0.06372549f), new Vector2(0.5718954f, 0.07026144f), new Vector2(0.3954248f, 0.2124183f), new Vector2(0.2401961f, 0.4150327f), new Vector2(0.1830065f, 0.5212418f), new Vector2(0.122549f, 0.7418301f), new Vector2(0.1437909f, 0.8104575f), new Vector2(0.1879085f, 0.8709151f), new Vector2(0.2434641f, 0.9199346f), new Vector2(0.3071896f, 0.9460784f), new Vector2(0.3921569f, 0.9509804f), new Vector2(0.4526144f, 0.7369281f) }, new Vector3[] { new Vector2(0.2810458f, 0.9313725f), new Vector2(0.3660131f, 0.6830065f) }, new Vector3[] { new Vector2(0.624183f, 0.3186274f), new Vector2(0.8284314f, 0.1437909f) } }; } return icon_telephone; }
        Vector3[][] Icon_doorOpen() { if (icon_doorOpen == null) { icon_doorOpen = new Vector3[][] { new Vector3[] { new Vector2(0.875817f, 0.1813726f), new Vector2(0.7107843f, 0.1813726f), new Vector2(0.7107843f, 0.8464052f), new Vector2(0.6503268f, 0.8464052f) }, new Vector3[] { new Vector2(0.7091503f, 0.8104575f), new Vector2(0.4869281f, 0.9362745f), new Vector2(0.4869281f, 0.06862745f), new Vector2(0.7107843f, 0.1830065f) }, new Vector3[] { new Vector2(0.5114379f, 0.5196078f), new Vector2(0.5114379f, 0.4803922f), new Vector2(0.5555556f, 0.503268f), new Vector2(0.5114379f, 0.5196078f) }, new Vector3[] { new Vector2(0.1094771f, 0.1813726f), new Vector2(0.2777778f, 0.1813726f), new Vector2(0.2777778f, 0.8169935f), new Vector2(0.4852941f, 0.8169935f) }, new Vector3[] { new Vector2(0.246732f, 0.1830065f), new Vector2(0.246732f, 0.8464052f), new Vector2(0.4852941f, 0.8464052f) } }; } return icon_doorOpen; }
        Vector3[][] Icon_doorEnter() { if (icon_doorEnter == null) { icon_doorEnter = new Vector3[][] { new Vector3[] { new Vector2(0.9444444f, 0.1830065f), new Vector2(0.8594771f, 0.1830065f), new Vector2(0.8594771f, 0.8431373f), new Vector2(0.7973856f, 0.8431373f) }, new Vector3[] { new Vector2(0.8562092f, 0.8071895f), new Vector2(0.6307189f, 0.9346405f), new Vector2(0.6307189f, 0.06699347f), new Vector2(0.8594771f, 0.1846405f) }, new Vector3[] { new Vector2(0.6519608f, 0.5163399f), new Vector2(0.6519608f, 0.4820261f), new Vector2(0.6960784f, 0.5049019f), new Vector2(0.6519608f, 0.5163399f) }, new Vector3[] { new Vector2(0.6290849f, 0.8447713f), new Vector2(0.3888889f, 0.8447713f), new Vector2(0.3888889f, 0.6911765f) }, new Vector3[] { new Vector2(0.6290849f, 0.8169935f), new Vector2(0.4215686f, 0.8169935f), new Vector2(0.4215686f, 0.6911765f) }, new Vector3[] { new Vector2(0.253268f, 0.1813726f), new Vector2(0.4215686f, 0.1813726f), new Vector2(0.4215686f, 0.3088235f) }, new Vector3[] { new Vector2(0.3888889f, 0.1813726f), new Vector2(0.3888889f, 0.3071896f) }, new Vector3[] { new Vector2(0.08986928f, 0.503268f), new Vector2(0.4869281f, 0.503268f) }, new Vector3[] { new Vector2(0.3513072f, 0.6323529f), new Vector2(0.4869281f, 0.503268f), new Vector2(0.3447712f, 0.3611111f) } }; } return icon_doorEnter; }
        Vector3[][] Icon_doorLeave() { if (icon_doorLeave == null) { icon_doorLeave = new Vector3[][] { new Vector3[] { new Vector2(0.9444444f, 0.1830065f), new Vector2(0.8594771f, 0.1830065f), new Vector2(0.8594771f, 0.8431373f), new Vector2(0.7973856f, 0.8431373f) }, new Vector3[] { new Vector2(0.8562092f, 0.8071895f), new Vector2(0.6307189f, 0.9346405f), new Vector2(0.6307189f, 0.06699347f), new Vector2(0.8594771f, 0.1846405f) }, new Vector3[] { new Vector2(0.6519608f, 0.5163399f), new Vector2(0.6519608f, 0.4820261f), new Vector2(0.6960784f, 0.5049019f), new Vector2(0.6519608f, 0.5163399f) }, new Vector3[] { new Vector2(0.6290849f, 0.8447713f), new Vector2(0.3888889f, 0.8447713f), new Vector2(0.3888889f, 0.5947713f) }, new Vector3[] { new Vector2(0.6290849f, 0.8169935f), new Vector2(0.4199346f, 0.8169935f), new Vector2(0.4199346f, 0.5947713f) }, new Vector3[] { new Vector2(0.25f, 0.1813726f), new Vector2(0.4215686f, 0.1813726f), new Vector2(0.4215686f, 0.4117647f) }, new Vector3[] { new Vector2(0.3872549f, 0.1830065f), new Vector2(0.3872549f, 0.4101307f) }, new Vector3[] { new Vector2(0.498366f, 0.501634f), new Vector2(0.1013072f, 0.501634f) }, new Vector3[] { new Vector2(0.2418301f, 0.6339869f), new Vector2(0.1013072f, 0.501634f), new Vector2(0.248366f, 0.3611111f) } }; } return icon_doorLeave; }
        Vector3[][] Icon_locationPin() { if (icon_locationPin == null) { icon_locationPin = new Vector3[][] { new Vector3[] { new Vector2(0.497781f, 0.9146566f), new Vector2(0.4367599f, 0.9268609f), new Vector2(0.3912714f, 0.9562619f), new Vector2(0.3585419f, 1.002305f), new Vector2(0.343564f, 1.0661f), new Vector2(0.3546587f, 1.126012f), new Vector2(0.3929356f, 1.181485f), new Vector2(0.4389789f, 1.210886f), new Vector2(0.5f, 1.221427f), new Vector2(0.5660138f, 1.206449f), new Vector2(0.612057f, 1.170945f), new Vector2(0.6414581f, 1.123238f), new Vector2(0.6508887f, 1.0661f), new Vector2(0.6370202f, 1.00286f), new Vector2(0.6059549f, 0.9579262f), new Vector2(0.5576927f, 0.9274156f), new Vector2(0.497781f, 0.9146566f) }, new Vector3[] { new Vector2(0.2633884f, 0.9485698f), new Vector2(0.2338675f, 1.054099f), new Vector2(0.2437341f, 1.148963f), new Vector2(0.2899046f, 1.237074f), new Vector2(0.3781035f, 1.311479f), new Vector2(0.4810059f, 1.341857f), new Vector2(0.598626f, 1.326022f), new Vector2(0.6815119f, 1.277197f), new Vector2(0.7467375f, 1.189811f), new Vector2(0.7758869f, 1.074232f), new Vector2(0.7568836f, 0.9736289f), new Vector2(0.5f, 0.5f), new Vector2(0.2633884f, 0.9485698f) } }; } return icon_locationPin; }
        Vector3[][] Icon_folder() { if (icon_folder == null) { icon_folder = new Vector3[][] { new Vector3[] { new Vector2(0.75f, 0.08006536f), new Vector2(0.09803922f, 0.08006536f), new Vector2(0.2369281f, 0.4689542f), new Vector2(0.8937908f, 0.4689542f), new Vector2(0.75f, 0.08006536f) }, new Vector3[] { new Vector2(0.0996732f, 0.08169935f), new Vector2(0.0996732f, 0.6650327f), new Vector2(0.3055556f, 0.6650327f), new Vector2(0.3333333f, 0.6372549f), new Vector2(0.3333333f, 0.5964052f), new Vector2(0.3594771f, 0.5718954f), new Vector2(0.748366f, 0.5718954f), new Vector2(0.748366f, 0.4689542f) } }; } return icon_folder; }
        Vector3[][] Icon_saveToFolder() { if (icon_saveToFolder == null) { icon_saveToFolder = new Vector3[][] { new Vector3[] { new Vector2(0.75f, 0.08006536f), new Vector2(0.09803922f, 0.08006536f), new Vector2(0.2369281f, 0.4689542f), new Vector2(0.8937908f, 0.4689542f), new Vector2(0.75f, 0.08006536f) }, new Vector3[] { new Vector2(0.0996732f, 0.08169935f), new Vector2(0.0996732f, 0.6650327f), new Vector2(0.3055556f, 0.6650327f), new Vector2(0.3333333f, 0.6372549f), new Vector2(0.3333333f, 0.5964052f), new Vector2(0.3594771f, 0.5718954f), new Vector2(0.748366f, 0.5718954f), new Vector2(0.748366f, 0.4689542f) }, new Vector3[] { new Vector2(0.5441176f, 0.9689543f), new Vector2(0.5441176f, 0.6601307f) }, new Vector3[] { new Vector2(0.4297386f, 0.7794118f), new Vector2(0.5441176f, 0.6584967f), new Vector2(0.6650327f, 0.7826797f) } }; } return icon_saveToFolder; }
        Vector3[][] Icon_loadFromFolder() { if (icon_loadFromFolder == null) { icon_loadFromFolder = new Vector3[][] { new Vector3[] { new Vector2(0.75f, 0.08006536f), new Vector2(0.09803922f, 0.08006536f), new Vector2(0.2369281f, 0.4689542f), new Vector2(0.8937908f, 0.4689542f), new Vector2(0.75f, 0.08006536f) }, new Vector3[] { new Vector2(0.0996732f, 0.08169935f), new Vector2(0.0996732f, 0.6650327f), new Vector2(0.3055556f, 0.6650327f), new Vector2(0.3333333f, 0.6372549f), new Vector2(0.3333333f, 0.5964052f), new Vector2(0.3594771f, 0.5718954f), new Vector2(0.748366f, 0.5718954f), new Vector2(0.748366f, 0.4689542f) }, new Vector3[] { new Vector2(0.5441176f, 0.9689543f), new Vector2(0.5441176f, 0.6601307f) }, new Vector3[] { new Vector2(0.4297386f, 0.8496732f), new Vector2(0.5441176f, 0.9673203f), new Vector2(0.6650327f, 0.8480392f) } }; } return icon_loadFromFolder; }
        Vector3[][] Icon_optionsSettingsGear() { if (icon_optionsSettingsGear == null) { icon_optionsSettingsGear = new Vector3[][] { new Vector3[] { new Vector2(0.4970988f, 0.3151436f), new Vector2(0.422816f, 0.3300002f), new Vector2(0.3674416f, 0.365791f), new Vector2(0.327599f, 0.4218408f), new Vector2(0.3093659f, 0.4995f), new Vector2(0.3228719f, 0.5724322f), new Vector2(0.3694675f, 0.639962f), new Vector2(0.4255172f, 0.6757528f), new Vector2(0.4998f, 0.6885835f), new Vector2(0.5801604f, 0.6703504f), new Vector2(0.6362102f, 0.6271313f), new Vector2(0.672001f, 0.5690557f), new Vector2(0.6834811f, 0.4995f), new Vector2(0.6665986f, 0.422516f), new Vector2(0.6287819f, 0.3678169f), new Vector2(0.570031f, 0.3306755f), new Vector2(0.4970988f, 0.3151436f) }, new Vector3[] { new Vector2(0.5722689f, 0.8453782f), new Vector2(0.6798319f, 0.7882353f), new Vector2(0.7739496f, 0.8789916f), new Vector2(0.8764706f, 0.7697479f), new Vector2(0.7840336f, 0.6806723f), new Vector2(0.8277311f, 0.5731093f), new Vector2(0.9588235f, 0.5731093f), new Vector2(0.9588235f, 0.4285714f), new Vector2(0.8294117f, 0.4285714f), new Vector2(0.7840336f, 0.3176471f), new Vector2(0.8714285f, 0.2268908f), new Vector2(0.7672269f, 0.1260504f), new Vector2(0.6697479f, 0.2184874f), new Vector2(0.5705882f, 0.1663866f), new Vector2(0.5705882f, 0.03697479f), new Vector2(0.4226891f, 0.03697479f), new Vector2(0.4226891f, 0.1663866f), new Vector2(0.3151261f, 0.2151261f), new Vector2(0.2243697f, 0.1277311f), new Vector2(0.1235294f, 0.2285714f), new Vector2(0.202521f, 0.3126051f), new Vector2(0.1605042f, 0.4252101f), new Vector2(0.03445378f, 0.4252101f), new Vector2(0.03445378f, 0.5714286f), new Vector2(0.1588235f, 0.5714286f), new Vector2(0.207563f, 0.6890756f), new Vector2(0.1168067f, 0.7798319f), new Vector2(0.2226891f, 0.8840336f), new Vector2(0.3084034f, 0.7915967f), new Vector2(0.4210084f, 0.8403361f), new Vector2(0.4210084f, 0.9663866f), new Vector2(0.5705882f, 0.9663866f), new Vector2(0.5705882f, 0.8470588f) } }; } return icon_optionsSettingsGear; }
        Vector3[][] Icon_adjustOptionsSettings() { if (icon_adjustOptionsSettings == null) { icon_adjustOptionsSettings = new Vector3[][] { new Vector3[] { new Vector2(0.2853949f, 0.7272727f), new Vector2(0.2853949f, 0.8643815f), new Vector2(0.3137109f, 0.9076006f), new Vector2(0.3599106f, 0.9359165f), new Vector2(0.4180328f, 0.9359165f), new Vector2(0.45231f, 0.9150522f), new Vector2(0.476155f, 0.8792846f), new Vector2(0.4880775f, 0.8420268f), new Vector2(0.4880775f, 0.7272727f), new Vector2(0.4701937f, 0.6929955f), new Vector2(0.4433681f, 0.6631893f), new Vector2(0.3897168f, 0.6438152f), new Vector2(0.3315946f, 0.6616989f), new Vector2(0.2943368f, 0.6959761f), new Vector2(0.2853949f, 0.7272727f) }, new Vector3[] { new Vector2(0.5551416f, 0.5707899f), new Vector2(0.5774963f, 0.609538f), new Vector2(0.6177347f, 0.6393443f), new Vector2(0.6609538f, 0.6467958f), new Vector2(0.7086438f, 0.6274217f), new Vector2(0.7339791f, 0.6020864f), new Vector2(0.7548435f, 0.557377f), new Vector2(0.7548435f, 0.4336811f), new Vector2(0.7354695f, 0.3994039f), new Vector2(0.7041728f, 0.3710879f), new Vector2(0.6564829f, 0.3532042f), new Vector2(0.6102831f, 0.366617f), new Vector2(0.5745156f, 0.390462f), new Vector2(0.552161f, 0.4441133f), new Vector2(0.552161f, 0.5707899f) }, new Vector3[] { new Vector2(0.185544f, 0.266766f), new Vector2(0.204918f, 0.3129657f), new Vector2(0.2421759f, 0.3412817f), new Vector2(0.2943368f, 0.3502235f), new Vector2(0.3405365f, 0.3323398f), new Vector2(0.3703428f, 0.3010432f), new Vector2(0.3852459f, 0.2578242f), new Vector2(0.3852459f, 0.1296572f), new Vector2(0.3614009f, 0.0923994f), new Vector2(0.3301043f, 0.07004471f), new Vector2(0.2794337f, 0.05961252f), new Vector2(0.2302534f, 0.07302534f), new Vector2(0.2004471f, 0.1043219f), new Vector2(0.181073f, 0.1564829f), new Vector2(0.181073f, 0.2697467f) }, new Vector3[] { new Vector2(0.3822653f, 0.8494784f), new Vector2(0.3822653f, 0.7272727f) }, new Vector3[] { new Vector2(0.647541f, 0.5424739f), new Vector2(0.647541f, 0.4351714f) }, new Vector3[] { new Vector2(0.280924f, 0.2503726f), new Vector2(0.280924f, 0.1385991f) }, new Vector3[] { new Vector2(0.2853949f, 0.7883756f), new Vector2(0.0633383f, 0.7883756f) }, new Vector3[] { new Vector2(0.4895678f, 0.7868853f), new Vector2(0.9411327f, 0.7868853f) }, new Vector3[] { new Vector2(0.552161f, 0.4992549f), new Vector2(0.0633383f, 0.4992549f) }, new Vector3[] { new Vector2(0.7578241f, 0.4977645f), new Vector2(0.9411327f, 0.4977645f) }, new Vector3[] { new Vector2(0.3852459f, 0.2041729f), new Vector2(0.9411327f, 0.2041729f) }, new Vector3[] { new Vector2(0.1795827f, 0.2056632f), new Vector2(0.0633383f, 0.2056632f) } }; } return icon_adjustOptionsSettings; }
        Vector3[][] Icon_pen() { if (icon_pen == null) { icon_pen = new Vector3[][] { new Vector3[] { new Vector2(0.5f, 0.5f), new Vector2(0.6363636f, 0.7694215f), new Vector2(1.335537f, 1.468595f), new Vector2(1.385124f, 1.430578f), new Vector2(1.434711f, 1.379339f), new Vector2(1.456198f, 1.338017f), new Vector2(0.7619835f, 0.6388429f), new Vector2(0.5f, 0.5f) }, new Vector3[] { new Vector2(0.6347107f, 0.7694215f), new Vector2(0.6909091f, 0.7297521f), new Vector2(0.7338843f, 0.6818182f), new Vector2(0.7586777f, 0.6371901f) }, new Vector3[] { new Vector2(1.247934f, 1.379339f), new Vector2(1.295868f, 1.349587f), new Vector2(1.347107f, 1.298347f), new Vector2(1.370248f, 1.255372f) }, new Vector3[] { new Vector2(0.5586777f, 0.6190082f), new Vector2(0.6066115f, 0.5644628f) }, new Vector3[] { new Vector2(0.5471075f, 0.5975206f), new Vector2(0.5834711f, 0.5512397f) }, new Vector3[] { new Vector2(0.5355372f, 0.5743802f), new Vector2(0.5603306f, 0.5413223f) }, new Vector3[] { new Vector2(0.5190083f, 0.5479339f), new Vector2(0.5355372f, 0.5264463f) } }; } return icon_pen; }
        Vector3[][] Icon_questionMark() { if (icon_questionMark == null) { icon_questionMark = new Vector3[][] { new Vector3[] { new Vector2(0.5004716f, 0.05543387f), new Vector2(0.3209394f, 0.09134027f), new Vector2(0.1871064f, 0.1778421f), new Vector2(0.09081188f, 0.3133073f), new Vector2(0.04674485f, 0.501f), new Vector2(0.0793871f, 0.677268f), new Vector2(0.1920027f, 0.840479f), new Vector2(0.3274679f, 0.9269809f), new Vector2(0.507f, 0.957991f), new Vector2(0.7012211f, 0.913924f), new Vector2(0.8366864f, 0.8094689f), new Vector2(0.9231882f, 0.6691074f), new Vector2(0.9509341f, 0.501f), new Vector2(0.9101313f, 0.3149394f), new Vector2(0.8187331f, 0.1827385f), new Vector2(0.6767395f, 0.09297243f), new Vector2(0.5004716f, 0.05543387f) }, new Vector3[] { new Vector2(0.5f, 0.2213354f), new Vector2(0.4595543f, 0.2381056f), new Vector2(0.4435733f, 0.275f), new Vector2(0.4589624f, 0.3136701f), new Vector2(0.4990135f, 0.3308348f), new Vector2(0.5376835f, 0.3154457f), new Vector2(0.55327f, 0.2748027f), new Vector2(0.5374863f, 0.23633f), new Vector2(0.5f, 0.2213354f) }, new Vector3[] { new Vector2(0.3801214f, 0.6843703f), new Vector2(0.4104704f, 0.6919575f), new Vector2(0.4332322f, 0.7405159f), new Vector2(0.4711684f, 0.7617602f), new Vector2(0.5531108f, 0.7602428f), new Vector2(0.5849772f, 0.7298938f), new Vector2(0.5925645f, 0.6737481f), new Vector2(0.560698f, 0.6312595f), new Vector2(0.5f, 0.5902883f), new Vector2(0.4590288f, 0.5204856f), new Vector2(0.4499241f, 0.4597875f), new Vector2(0.4514416f, 0.4097117f), new Vector2(0.483308f, 0.3884674f), new Vector2(0.5212443f, 0.4127466f), new Vector2(0.5227618f, 0.4749621f), new Vector2(0.547041f, 0.5295903f), new Vector2(0.6062216f, 0.569044f), new Vector2(0.6532625f, 0.6267071f), new Vector2(0.6729894f, 0.6980273f), new Vector2(0.6456752f, 0.7678301f), new Vector2(0.5925645f, 0.8239757f), new Vector2(0.5091047f, 0.8421851f), new Vector2(0.4271624f, 0.8270106f), new Vector2(0.3664643f, 0.7860395f), new Vector2(0.3467375f, 0.7329287f), new Vector2(0.3558422f, 0.6995448f), new Vector2(0.3801214f, 0.6843703f) } }; } return icon_questionMark; }
        Vector3[][] Icon_exclamationMark() { if (icon_exclamationMark == null) { icon_exclamationMark = new Vector3[][] { new Vector3[] { new Vector2(0.5004716f, 0.05543387f), new Vector2(0.3209394f, 0.09134027f), new Vector2(0.1871064f, 0.1778421f), new Vector2(0.09081188f, 0.3133073f), new Vector2(0.04674485f, 0.501f), new Vector2(0.0793871f, 0.677268f), new Vector2(0.1920027f, 0.840479f), new Vector2(0.3274679f, 0.9269809f), new Vector2(0.507f, 0.957991f), new Vector2(0.7012211f, 0.913924f), new Vector2(0.8366864f, 0.8094689f), new Vector2(0.9231882f, 0.6691074f), new Vector2(0.9509341f, 0.501f), new Vector2(0.9101313f, 0.3149394f), new Vector2(0.8187331f, 0.1827385f), new Vector2(0.6767395f, 0.09297243f), new Vector2(0.5004716f, 0.05543387f) }, new Vector3[] { new Vector2(0.5f, 0.2213354f), new Vector2(0.4595543f, 0.2381056f), new Vector2(0.4435733f, 0.275f), new Vector2(0.4589624f, 0.3136701f), new Vector2(0.4990135f, 0.3308348f), new Vector2(0.5376835f, 0.3154457f), new Vector2(0.55327f, 0.2748027f), new Vector2(0.5374863f, 0.23633f), new Vector2(0.5f, 0.2213354f) }, new Vector3[] { new Vector2(0.547041f, 0.4339909f), new Vector2(0.5531108f, 0.8072838f), new Vector2(0.4377845f, 0.8072838f), new Vector2(0.4514416f, 0.4355083f), new Vector2(0.5455235f, 0.4355083f) } }; } return icon_exclamationMark; }
        Vector3[][] Icon_shoppingCart() { if (icon_shoppingCart == null) { icon_shoppingCart = new Vector3[][] { new Vector3[] { new Vector2(0.328f, 0.0289638f), new Vector2(0.2725775f, 0.05194387f), new Vector2(0.2506789f, 0.1025f), new Vector2(0.2717665f, 0.1554893f), new Vector2(0.3266482f, 0.1790101f), new Vector2(0.3796375f, 0.1579225f), new Vector2(0.4009955f, 0.1022296f), new Vector2(0.3793672f, 0.04951068f), new Vector2(0.328f, 0.0289638f) }, new Vector3[] { new Vector2(0.8052f, 0.02983347f), new Vector2(0.7506872f, 0.05238551f), new Vector2(0.729148f, 0.102f), new Vector2(0.7498895f, 0.1540024f), new Vector2(0.8038704f, 0.177085f), new Vector2(0.8559899f, 0.1563902f), new Vector2(0.8769972f, 0.1017347f), new Vector2(0.855724f, 0.04999765f), new Vector2(0.8052f, 0.02983347f) }, new Vector3[] { new Vector2(0.07310925f, 0.8470588f), new Vector2(0.1773109f, 0.8470588f), new Vector2(0.2210084f, 0.789916f), new Vector2(0.3252101f, 0.3226891f), new Vector2(0.2663866f, 0.2184874f), new Vector2(0.8445378f, 0.2184874f) }, new Vector3[] { new Vector2(0.3252101f, 0.3243698f), new Vector2(0.797479f, 0.3243698f), new Vector2(0.920168f, 0.7159664f), new Vector2(0.2378151f, 0.7159664f) }, new Vector3[] { new Vector2(0.2831933f, 0.5126051f), new Vector2(0.8546218f, 0.5126051f) }, new Vector3[] { new Vector2(0.6344538f, 0.3260504f), new Vector2(0.6865546f, 0.7142857f) }, new Vector3[] { new Vector2(0.4798319f, 0.3243698f), new Vector2(0.4310924f, 0.7176471f) } }; } return icon_shoppingCart; }
        Vector3[][] Icon_checkmarkChecked() { if (icon_checkmarkChecked == null) { icon_checkmarkChecked = new Vector3[][] { new Vector3[] { new Vector2(0.8383784f, 0.545056f), new Vector2(0.8328274f, 0.4063708f), new Vector2(0.7839646f, 0.2958893f), new Vector2(0.6939817f, 0.2076955f), new Vector2(0.5586894f, 0.1523994f), new Vector2(0.4228359f, 0.1558943f), new Vector2(0.2872558f, 0.2208181f), new Vector2(0.2063938f, 0.3119646f), new Vector2(0.1618434f, 0.4426985f), new Vector2(0.1717589f, 0.5933512f), new Vector2(0.2338696f, 0.7071881f), new Vector2(0.3286821f, 0.7886317f), new Vector2(0.45125f, 0.8293808f), new Vector2(0.5954049f, 0.8209394f), new Vector2(0.6697479f, 0.7915967f) }, new Vector3[] { new Vector2(0.505042f, 0.4252101f), new Vector2(0.5739496f, 0.5478992f), new Vector2(0.6563025f, 0.6571429f), new Vector2(0.7487395f, 0.7394958f), new Vector2(0.8193277f, 0.784874f), new Vector2(0.9067227f, 0.8319328f), new Vector2(0.9487395f, 0.8487395f), new Vector2(0.9638655f, 0.8252101f), new Vector2(0.8798319f, 0.7663866f), new Vector2(0.7605042f, 0.6605042f), new Vector2(0.6747899f, 0.5478992f), new Vector2(0.592437f, 0.4151261f), new Vector2(0.5386555f, 0.3058824f), new Vector2(0.5235294f, 0.2890756f), new Vector2(0.494958f, 0.282353f), new Vector2(0.4613445f, 0.3126051f), new Vector2(0.2932773f, 0.5882353f), new Vector2(0.3016807f, 0.6168067f), new Vector2(0.3201681f, 0.6420168f), new Vector2(0.3537815f, 0.6420168f), new Vector2(0.3789916f, 0.6184874f), new Vector2(0.505042f, 0.4252101f) } }; } return icon_checkmarkChecked; }
        Vector3[][] Icon_checkmarkUnchecked() { if (icon_checkmarkUnchecked == null) { icon_checkmarkUnchecked = new Vector3[][] { new Vector3[] { new Vector2(0.8383784f, 0.545056f), new Vector2(0.8328274f, 0.4063708f), new Vector2(0.7839646f, 0.2958893f), new Vector2(0.6939817f, 0.2076955f), new Vector2(0.5586894f, 0.1523994f), new Vector2(0.4228359f, 0.1558943f), new Vector2(0.2872558f, 0.2208181f), new Vector2(0.2063938f, 0.3119646f), new Vector2(0.1618434f, 0.4426985f), new Vector2(0.1717589f, 0.5933512f), new Vector2(0.2338696f, 0.7071881f), new Vector2(0.3286821f, 0.7886317f), new Vector2(0.45125f, 0.8293808f), new Vector2(0.5954049f, 0.8209394f), new Vector2(0.7052462f, 0.7682168f), new Vector2(0.7893279f, 0.6725702f), new Vector2(0.8383784f, 0.545056f) } }; } return icon_checkmarkUnchecked; }
        Vector3[][] Icon_battery() { if (icon_battery == null) { icon_battery = new Vector3[][] { new Vector3[] { new Vector2(0.8882353f, 0.6521009f), new Vector2(0.8882353f, 0.3512605f), new Vector2(0.8596638f, 0.3260504f), new Vector2(0.08487395f, 0.3260504f), new Vector2(0.05798319f, 0.3579832f), new Vector2(0.05798319f, 0.6487395f), new Vector2(0.08319328f, 0.6756303f), new Vector2(0.8680672f, 0.6756303f), new Vector2(0.8882353f, 0.6521009f) }, new Vector3[] { new Vector2(0.9084033f, 0.6605042f), new Vector2(0.9084033f, 0.3411765f), new Vector2(0.8680672f, 0.3042017f), new Vector2(0.07815126f, 0.3042017f), new Vector2(0.0394958f, 0.3529412f), new Vector2(0.0394958f, 0.6605042f), new Vector2(0.08151261f, 0.7008404f), new Vector2(0.8747899f, 0.7008404f), new Vector2(0.9084033f, 0.6605042f) }, new Vector3[] { new Vector2(0.4142857f, 0.6117647f), new Vector2(0.4142857f, 0.3714286f), new Vector2(0.3991597f, 0.3495798f), new Vector2(0.3621849f, 0.3495798f), new Vector2(0.3504202f, 0.3798319f), new Vector2(0.3504202f, 0.6100841f), new Vector2(0.3672269f, 0.6319328f), new Vector2(0.402521f, 0.6319328f), new Vector2(0.4142857f, 0.6117647f) }, new Vector3[] { new Vector2(0.3033614f, 0.6084034f), new Vector2(0.3033614f, 0.3731093f), new Vector2(0.2831933f, 0.3529412f), new Vector2(0.2529412f, 0.3529412f), new Vector2(0.2310924f, 0.3865546f), new Vector2(0.2310924f, 0.6067227f), new Vector2(0.2529412f, 0.6336135f), new Vector2(0.2865546f, 0.6336135f), new Vector2(0.3033614f, 0.6084034f) }, new Vector3[] { new Vector2(0.1890756f, 0.6033614f), new Vector2(0.1890756f, 0.3781513f), new Vector2(0.1722689f, 0.3579832f), new Vector2(0.1336135f, 0.3579832f), new Vector2(0.1168067f, 0.3798319f), new Vector2(0.1168067f, 0.6033614f), new Vector2(0.1352941f, 0.6302521f), new Vector2(0.1739496f, 0.6302521f), new Vector2(0.1890756f, 0.6033614f) }, new Vector3[] { new Vector2(0.9117647f, 0.5865546f), new Vector2(0.9638655f, 0.5546219f), new Vector2(0.9638655f, 0.4386555f), new Vector2(0.910084f, 0.4084034f) } }; } return icon_battery; }
        Vector3[][] Icon_cloud() { if (icon_cloud == null) { icon_cloud = new Vector3[][] { new Vector3[] { new Vector2(0.8260504f, 0.2285714f), new Vector2(0.1806723f, 0.2285714f), new Vector2(0.1134454f, 0.2521009f), new Vector2(0.0512605f, 0.3159664f), new Vector2(0.03277311f, 0.394958f), new Vector2(0.0512605f, 0.4789916f), new Vector2(0.1016807f, 0.5394958f), new Vector2(0.1605042f, 0.5680673f), new Vector2(0.2109244f, 0.5697479f), new Vector2(0.197479f, 0.6672269f), new Vector2(0.2277311f, 0.7546219f), new Vector2(0.2915967f, 0.8134454f), new Vector2(0.3655462f, 0.8470588f), new Vector2(0.4563025f, 0.8403361f), new Vector2(0.5319328f, 0.8016807f), new Vector2(0.5756302f, 0.7478992f), new Vector2(0.5991597f, 0.7109244f), new Vector2(0.6529412f, 0.7462185f), new Vector2(0.7235294f, 0.7546219f), new Vector2(0.792437f, 0.7210084f), new Vector2(0.8394958f, 0.6537815f), new Vector2(0.8428571f, 0.5815126f), new Vector2(0.8260504f, 0.5260504f), new Vector2(0.894958f, 0.5042017f), new Vector2(0.9436975f, 0.4453782f), new Vector2(0.9655462f, 0.3714286f), new Vector2(0.9436975f, 0.3008403f), new Vector2(0.894958f, 0.2521009f), new Vector2(0.8260504f, 0.2285714f) } }; } return icon_cloud; }
        Vector3[][] Icon_magnifier() { if (icon_magnifier == null) { icon_magnifier = new Vector3[][] { new Vector3[] { new Vector2(0.3296365f, 0.4222632f), new Vector2(0.228889f, 0.4424127f), new Vector2(0.1537864f, 0.4909546f), new Vector2(0.09974916f, 0.5669732f), new Vector2(0.07502025f, 0.6723f), new Vector2(0.09333797f, 0.7712157f), new Vector2(0.1565341f, 0.8628042f), new Vector2(0.2325526f, 0.9113462f), new Vector2(0.3333f, 0.928748f), new Vector2(0.4422903f, 0.9040191f), new Vector2(0.5183089f, 0.8454024f), new Vector2(0.5668508f, 0.7666363f), new Vector2(0.5824209f, 0.6723f), new Vector2(0.5595237f, 0.567889f), new Vector2(0.5082341f, 0.4937023f), new Vector2(0.4285521f, 0.4433286f), new Vector2(0.3296365f, 0.4222632f) }, new Vector3[] { new Vector2(0.9166667f, 0.1601307f), new Vector2(0.8937908f, 0.1062092f), new Vector2(0.8415033f, 0.07843138f), new Vector2(0.5441176f, 0.377451f), new Vector2(0.5620915f, 0.4395425f), new Vector2(0.6209151f, 0.4575163f), new Vector2(0.9166667f, 0.1601307f) }, new Vector3[] { new Vector2(0.5620915f, 0.4362745f), new Vector2(0.5049019f, 0.4934641f) }, new Vector3[] { new Vector2(0.2222222f, 0.5326797f), new Vector2(0.1797386f, 0.5800654f), new Vector2(0.1535948f, 0.6323529f), new Vector2(0.1454248f, 0.6879085f), new Vector2(0.1568628f, 0.740196f), new Vector2(0.1830065f, 0.7843137f), new Vector2(0.2156863f, 0.8202614f) } }; } return icon_magnifier; }
        Vector3[][] Icon_magnifierPlus() { if (icon_magnifierPlus == null) { icon_magnifierPlus = new Vector3[][] { new Vector3[] { new Vector2(0.3296365f, 0.4222632f), new Vector2(0.228889f, 0.4424127f), new Vector2(0.1537864f, 0.4909546f), new Vector2(0.09974916f, 0.5669732f), new Vector2(0.07502025f, 0.6723f), new Vector2(0.09333797f, 0.7712157f), new Vector2(0.1565341f, 0.8628042f), new Vector2(0.2325526f, 0.9113462f), new Vector2(0.3333f, 0.928748f), new Vector2(0.4422903f, 0.9040191f), new Vector2(0.5183089f, 0.8454024f), new Vector2(0.5668508f, 0.7666363f), new Vector2(0.5824209f, 0.6723f), new Vector2(0.5595237f, 0.567889f), new Vector2(0.5082341f, 0.4937023f), new Vector2(0.4285521f, 0.4433286f), new Vector2(0.3296365f, 0.4222632f) }, new Vector3[] { new Vector2(0.9166667f, 0.1601307f), new Vector2(0.8937908f, 0.1062092f), new Vector2(0.8415033f, 0.07843138f), new Vector2(0.5441176f, 0.377451f), new Vector2(0.5620915f, 0.4395425f), new Vector2(0.6209151f, 0.4575163f), new Vector2(0.9166667f, 0.1601307f) }, new Vector3[] { new Vector2(0.5620915f, 0.4362745f), new Vector2(0.5049019f, 0.4934641f) }, new Vector3[] { new Vector2(0.1846405f, 0.6764706f), new Vector2(0.4754902f, 0.6764706f) }, new Vector3[] { new Vector2(0.3316993f, 0.8218954f), new Vector2(0.3316993f, 0.5294118f) } }; } return icon_magnifierPlus; }
        Vector3[][] Icon_magnifierMinus() { if (icon_magnifierMinus == null) { icon_magnifierMinus = new Vector3[][] { new Vector3[] { new Vector2(0.3296365f, 0.4222632f), new Vector2(0.228889f, 0.4424127f), new Vector2(0.1537864f, 0.4909546f), new Vector2(0.09974916f, 0.5669732f), new Vector2(0.07502025f, 0.6723f), new Vector2(0.09333797f, 0.7712157f), new Vector2(0.1565341f, 0.8628042f), new Vector2(0.2325526f, 0.9113462f), new Vector2(0.3333f, 0.928748f), new Vector2(0.4422903f, 0.9040191f), new Vector2(0.5183089f, 0.8454024f), new Vector2(0.5668508f, 0.7666363f), new Vector2(0.5824209f, 0.6723f), new Vector2(0.5595237f, 0.567889f), new Vector2(0.5082341f, 0.4937023f), new Vector2(0.4285521f, 0.4433286f), new Vector2(0.3296365f, 0.4222632f) }, new Vector3[] { new Vector2(0.9166667f, 0.1601307f), new Vector2(0.8937908f, 0.1062092f), new Vector2(0.8415033f, 0.07843138f), new Vector2(0.5441176f, 0.377451f), new Vector2(0.5620915f, 0.4395425f), new Vector2(0.6209151f, 0.4575163f), new Vector2(0.9166667f, 0.1601307f) }, new Vector3[] { new Vector2(0.5620915f, 0.4362745f), new Vector2(0.5049019f, 0.4934641f) }, new Vector3[] { new Vector2(0.1846405f, 0.6764706f), new Vector2(0.4754902f, 0.6764706f) } }; } return icon_magnifierMinus; }
        Vector3[][] Icon_timeHourglassCursor() { if (icon_timeHourglassCursor == null) { icon_timeHourglassCursor = new Vector3[][] { new Vector3[] { new Vector2(0.2075163f, 0.9493464f), new Vector2(0.7908497f, 0.9493464f), new Vector2(0.7908497f, 0.8709151f), new Vector2(0.2091503f, 0.8709151f), new Vector2(0.2075163f, 0.9493464f) }, new Vector3[] { new Vector2(0.2124183f, 0.1437909f), new Vector2(0.7941176f, 0.1437909f), new Vector2(0.7941176f, 0.06372549f), new Vector2(0.2124183f, 0.06372549f), new Vector2(0.2124183f, 0.1437909f) }, new Vector3[] { new Vector2(0.2679739f, 0.8660131f), new Vector2(0.2712418f, 0.7777778f), new Vector2(0.2957516f, 0.7107843f), new Vector2(0.3447712f, 0.6421568f), new Vector2(0.3986928f, 0.5767974f), new Vector2(0.4411765f, 0.5343137f), new Vector2(0.4444444f, 0.4869281f), new Vector2(0.3954248f, 0.4395425f), new Vector2(0.3366013f, 0.374183f), new Vector2(0.2957516f, 0.3022876f), new Vector2(0.2745098f, 0.2140523f), new Vector2(0.2745098f, 0.1470588f) }, new Vector3[] { new Vector2(0.7222222f, 0.8692811f), new Vector2(0.7205882f, 0.7924837f), new Vector2(0.6911765f, 0.7107843f), new Vector2(0.6405229f, 0.6339869f), new Vector2(0.5882353f, 0.5784314f), new Vector2(0.5473856f, 0.5310457f), new Vector2(0.5473856f, 0.4820261f), new Vector2(0.6078432f, 0.4297386f), new Vector2(0.6617647f, 0.372549f), new Vector2(0.7026144f, 0.3071896f), new Vector2(0.7222222f, 0.2271242f), new Vector2(0.7254902f, 0.1454248f) }, new Vector3[] { new Vector2(0.3316993f, 0.2712418f), new Vector2(0.6699346f, 0.2712418f), new Vector2(0.6830065f, 0.1813726f), new Vector2(0.3186274f, 0.1813726f), new Vector2(0.3316993f, 0.2712418f) }, new Vector3[] { new Vector2(0.3823529f, 0.6797386f), new Vector2(0.496732f, 0.5653595f), new Vector2(0.6013072f, 0.6813725f), new Vector2(0.3823529f, 0.6813725f) } }; } return icon_timeHourglassCursor; }
        Vector3[][] Icon_cursorHand() { if (icon_cursorHand == null) { icon_cursorHand = new Vector3[][] { new Vector3[] { new Vector2(0.8135948f, -0.3888889f), new Vector2(0.413268f, -0.3888889f), new Vector2(0.3969281f, -0.2712418f), new Vector2(0.3364706f, -0.1699346f), new Vector2(0.2613072f, -0.0653595f), new Vector2(0.1926797f, -0.01307189f), new Vector2(0.2188235f, 0.04738557f), new Vector2(0.2694771f, 0.06699347f), new Vector2(0.3217647f, 0.05718952f), new Vector2(0.3805882f, 0.02287579f), new Vector2(0.4394118f, -0.02941179f), new Vector2(0.4394118f, 0.4754902f), new Vector2(0.4639216f, 0.5f), new Vector2(0.5015033f, 0.52f), new Vector2(0.535817f, 0.5f), new Vector2(0.5652288f, 0.4460784f), new Vector2(0.5652288f, 0.07516342f) }, new Vector3[] { new Vector2(0.8135948f, -0.3872549f), new Vector2(0.8201307f, -0.3088235f), new Vector2(0.8707843f, -0.1781046f), new Vector2(0.9279737f, -0.1176471f), new Vector2(0.9475816f, -0.04411769f), new Vector2(0.9475816f, 0.1519608f), new Vector2(0.913268f, 0.2075163f), new Vector2(0.8479085f, 0.2075163f), new Vector2(0.8103268f, 0.1519608f) }, new Vector3[] { new Vector2(0.5668627f, 0.2303922f), new Vector2(0.5979085f, 0.2777778f), new Vector2(0.6534641f, 0.2777778f), new Vector2(0.6910456f, 0.2156863f), new Vector2(0.6910456f, 0.03431368f) }, new Vector3[] { new Vector2(0.6926796f, 0.2156863f), new Vector2(0.7204576f, 0.2614379f), new Vector2(0.787451f, 0.259804f), new Vector2(0.8086928f, 0.2303922f), new Vector2(0.8086928f, -0.006535888f) } }; } return icon_cursorHand; }
        Vector3[][] Icon_cursorPointer() { if (icon_cursorPointer == null) { icon_cursorPointer = new Vector3[][] { new Vector3[] { new Vector2(0.5f, 0.4950981f), new Vector2(1.078431f, -0.0767974f), new Vector2(0.8169935f, -0.124183f), new Vector2(0.9330065f, -0.375817f), new Vector2(0.8186275f, -0.4297386f), new Vector2(0.7075163f, -0.1633987f), new Vector2(0.5f, -0.3022876f), new Vector2(0.5f, 0.4950981f) } }; } return icon_cursorPointer; }
        Vector3[][] Icon_trashcan() { if (icon_trashcan == null) { icon_trashcan = new Vector3[][] { new Vector3[] { new Vector2(0.1633987f, 0.7794118f), new Vector2(0.2647059f, 0.8186275f), new Vector2(0.3807189f, 0.8447713f), new Vector2(0.5343137f, 0.8562092f), new Vector2(0.6748366f, 0.8447713f), new Vector2(0.8071895f, 0.8186275f), new Vector2(0.9084967f, 0.7777778f), new Vector2(0.9084967f, 0.740196f), new Vector2(0.1633987f, 0.740196f), new Vector2(0.1633987f, 0.7794118f) }, new Vector3[] { new Vector2(0.2124183f, 0.7369281f), new Vector2(0.3104575f, 0.06699347f), new Vector2(0.759804f, 0.06699347f), new Vector2(0.8578432f, 0.7385621f) }, new Vector3[] { new Vector2(0.4215686f, 0.8480392f), new Vector2(0.4215686f, 0.9052287f), new Vector2(0.4575163f, 0.9379085f), new Vector2(0.6192811f, 0.9379085f), new Vector2(0.6519608f, 0.8970588f), new Vector2(0.6519608f, 0.8464052f) }, new Vector3[] { new Vector2(0.5359477f, 0.1699346f), new Vector2(0.5359477f, 0.6519608f) }, new Vector3[] { new Vector2(0.6552287f, 0.1699346f), new Vector2(0.6960784f, 0.6503268f) }, new Vector3[] { new Vector2(0.4150327f, 0.1715686f), new Vector2(0.374183f, 0.6470588f) } }; } return icon_trashcan; }
        Vector3[][] Icon_switchOnOff() { if (icon_switchOnOff == null) { icon_switchOnOff = new Vector3[][] { new Vector3[] { new Vector2(0.7349027f, 0.7637997f), new Vector2(0.8504868f, 0.647216f), new Vector2(0.9067466f, 0.5158696f), new Vector2(0.9105699f, 0.3668895f), new Vector2(0.8493953f, 0.2052008f), new Vector2(0.736874f, 0.09041098f), new Vector2(0.5713946f, 0.02536854f), new Vector2(0.4273166f, 0.02883014f), new Vector2(0.2782348f, 0.095633f), new Vector2(0.1558084f, 0.2256407f), new Vector2(0.1073216f, 0.3711601f), new Vector2(0.1132343f, 0.5188804f), new Vector2(0.1767464f, 0.657829f), new Vector2(0.2663399f, 0.7630719f), new Vector2(0.3088235f, 0.7761438f), new Vector2(0.3366013f, 0.759804f), new Vector2(0.3529412f, 0.7369281f), new Vector2(0.3513072f, 0.7075163f), new Vector2(0.3251634f, 0.6732026f), new Vector2(0.2581699f, 0.6094771f), new Vector2(0.2091503f, 0.5049019f), new Vector2(0.2107843f, 0.374183f), new Vector2(0.2565359f, 0.2696078f), new Vector2(0.3284314f, 0.1911765f), new Vector2(0.4379085f, 0.1437909f), new Vector2(0.5588235f, 0.1405229f), new Vector2(0.6650327f, 0.1846405f), new Vector2(0.740196f, 0.253268f), new Vector2(0.7990196f, 0.374183f), new Vector2(0.7957516f, 0.5049019f), new Vector2(0.751634f, 0.6045752f), new Vector2(0.6633987f, 0.6830065f), new Vector2(0.6503268f, 0.7091503f), new Vector2(0.6470588f, 0.7434641f), new Vector2(0.6715686f, 0.7712418f), new Vector2(0.7075163f, 0.7777778f), new Vector2(0.7349027f, 0.7637997f) }, new Vector3[] { new Vector2(0.4526144f, 0.9232026f), new Vector2(0.4526144f, 0.6111111f), new Vector2(0.4640523f, 0.5784314f), new Vector2(0.5f, 0.5620915f), new Vector2(0.5375817f, 0.5718954f), new Vector2(0.5522876f, 0.5980392f), new Vector2(0.5555556f, 0.6307189f), new Vector2(0.5555556f, 0.9248366f), new Vector2(0.5359477f, 0.9558824f), new Vector2(0.5065359f, 0.9640523f), new Vector2(0.4640523f, 0.9558824f), new Vector2(0.4526144f, 0.9232026f) } }; } return icon_switchOnOff; }
        Vector3[][] Icon_playButton() { if (icon_playButton == null) { icon_playButton = new Vector3[][] { new Vector3[] { new Vector2(0.4984218f, 0.05103594f), new Vector2(0.3175205f, 0.0872162f), new Vector2(0.1826668f, 0.1743777f), new Vector2(0.08563793f, 0.310876f), new Vector2(0.04123488f, 0.5f), new Vector2(0.07412603f, 0.6776122f), new Vector2(0.1876005f, 0.8420679f), new Vector2(0.3240987f, 0.9292295f), new Vector2(0.505f, 0.960476f), new Vector2(0.7007022f, 0.916073f), new Vector2(0.8372006f, 0.8108213f), new Vector2(0.9243621f, 0.6693895f), new Vector2(0.9523196f, 0.5f), new Vector2(0.9112056f, 0.3125206f), new Vector2(0.8191104f, 0.1793114f), new Vector2(0.676034f, 0.08886078f), new Vector2(0.4984218f, 0.05103594f) }, new Vector3[] { new Vector2(0.3594771f, 0.7434641f), new Vector2(0.3594771f, 0.2434641f), new Vector2(0.7565359f, 0.496732f), new Vector2(0.3594771f, 0.7434641f) } }; } return icon_playButton; }
        Vector3[][] Icon_pauseButton() { if (icon_pauseButton == null) { icon_pauseButton = new Vector3[][] { new Vector3[] { new Vector2(0.4984218f, 0.05103594f), new Vector2(0.3175205f, 0.0872162f), new Vector2(0.1826668f, 0.1743777f), new Vector2(0.08563793f, 0.310876f), new Vector2(0.04123488f, 0.5f), new Vector2(0.07412603f, 0.6776122f), new Vector2(0.1876005f, 0.8420679f), new Vector2(0.3240987f, 0.9292295f), new Vector2(0.505f, 0.960476f), new Vector2(0.7007022f, 0.916073f), new Vector2(0.8372006f, 0.8108213f), new Vector2(0.9243621f, 0.6693895f), new Vector2(0.9523196f, 0.5f), new Vector2(0.9112056f, 0.3125206f), new Vector2(0.8191104f, 0.1793114f), new Vector2(0.676034f, 0.08886078f), new Vector2(0.4984218f, 0.05103594f) }, new Vector3[] { new Vector2(0.2761438f, 0.7254902f), new Vector2(0.4444444f, 0.7254902f), new Vector2(0.4444444f, 0.2794118f), new Vector2(0.2761438f, 0.2794118f), new Vector2(0.2761438f, 0.7254902f) }, new Vector3[] { new Vector2(0.5588235f, 0.7238562f), new Vector2(0.7173203f, 0.7238562f), new Vector2(0.7173203f, 0.2777778f), new Vector2(0.5588235f, 0.2777778f), new Vector2(0.5588235f, 0.7238562f) } }; } return icon_pauseButton; }
        Vector3[][] Icon_stopButton() { if (icon_stopButton == null) { icon_stopButton = new Vector3[][] { new Vector3[] { new Vector2(0.4984218f, 0.05103594f), new Vector2(0.3175205f, 0.0872162f), new Vector2(0.1826668f, 0.1743777f), new Vector2(0.08563793f, 0.310876f), new Vector2(0.04123488f, 0.5f), new Vector2(0.07412603f, 0.6776122f), new Vector2(0.1876005f, 0.8420679f), new Vector2(0.3240987f, 0.9292295f), new Vector2(0.505f, 0.960476f), new Vector2(0.7007022f, 0.916073f), new Vector2(0.8372006f, 0.8108213f), new Vector2(0.9243621f, 0.6693895f), new Vector2(0.9523196f, 0.5f), new Vector2(0.9112056f, 0.3125206f), new Vector2(0.8191104f, 0.1793114f), new Vector2(0.676034f, 0.08886078f), new Vector2(0.4984218f, 0.05103594f) }, new Vector3[] { new Vector2(0.2761438f, 0.2777778f), new Vector2(0.2761438f, 0.7254902f), new Vector2(0.7156863f, 0.7254902f), new Vector2(0.7156863f, 0.2777778f), new Vector2(0.2761438f, 0.2777778f) } }; } return icon_stopButton; }
        Vector3[][] Icon_playPauseButton() { if (icon_playPauseButton == null) { icon_playPauseButton = new Vector3[][] { new Vector3[] { new Vector2(0.4984218f, 0.05103594f), new Vector2(0.3175205f, 0.0872162f), new Vector2(0.1826668f, 0.1743777f), new Vector2(0.08563793f, 0.310876f), new Vector2(0.04123488f, 0.5f), new Vector2(0.07412603f, 0.6776122f), new Vector2(0.1876005f, 0.8420679f), new Vector2(0.3240987f, 0.9292295f), new Vector2(0.505f, 0.960476f), new Vector2(0.7007022f, 0.916073f), new Vector2(0.8372006f, 0.8108213f), new Vector2(0.9243621f, 0.6693895f), new Vector2(0.9523196f, 0.5f), new Vector2(0.9112056f, 0.3125206f), new Vector2(0.8191104f, 0.1793114f), new Vector2(0.676034f, 0.08886078f), new Vector2(0.4984218f, 0.05103594f) }, new Vector3[] { new Vector2(0.2189543f, 0.6830065f), new Vector2(0.2189543f, 0.3218954f), new Vector2(0.4689542f, 0.5049019f), new Vector2(0.2189543f, 0.6830065f) }, new Vector3[] { new Vector2(0.5457516f, 0.6830065f), new Vector2(0.6323529f, 0.6830065f), new Vector2(0.6323529f, 0.3235294f), new Vector2(0.5457516f, 0.3235294f), new Vector2(0.5457516f, 0.6846405f) }, new Vector3[] { new Vector2(0.6781046f, 0.6813725f), new Vector2(0.7630719f, 0.6813725f), new Vector2(0.7630719f, 0.3235294f), new Vector2(0.6781046f, 0.3235294f), new Vector2(0.6781046f, 0.6830065f) } }; } return icon_playPauseButton; }
        Vector3[][] Icon_heart() { if (icon_heart == null) { icon_heart = new Vector3[][] { new Vector3[] { new Vector2(0.501634f, 0.129085f), new Vector2(0.1127451f, 0.5147059f), new Vector2(0.07026144f, 0.5898693f), new Vector2(0.05718954f, 0.6879085f), new Vector2(0.08333334f, 0.7728758f), new Vector2(0.1405229f, 0.8447713f), new Vector2(0.2107843f, 0.8872549f), new Vector2(0.3039216f, 0.9019608f), new Vector2(0.3823529f, 0.8839869f), new Vector2(0.4330065f, 0.8529412f), new Vector2(0.4738562f, 0.8120915f), new Vector2(0.501634f, 0.7679738f), new Vector2(0.5506536f, 0.8382353f), new Vector2(0.6029412f, 0.872549f), new Vector2(0.6764706f, 0.8986928f), new Vector2(0.7679738f, 0.8970588f), new Vector2(0.8431373f, 0.8611111f), new Vector2(0.9035948f, 0.8022876f), new Vector2(0.9428105f, 0.7271242f), new Vector2(0.9444444f, 0.6372549f), new Vector2(0.9215686f, 0.5588235f), new Vector2(0.8709151f, 0.4918301f), new Vector2(0.501634f, 0.129085f) } }; } return icon_heart; }
        Vector3[][] Icon_coin() { if (icon_coin == null) { icon_coin = new Vector3[][] { new Vector3[] { new Vector2(0.4977612f, 0.1414546f), new Vector2(0.3536953f, 0.1702678f), new Vector2(0.2463007f, 0.2396813f), new Vector2(0.169029f, 0.3483857f), new Vector2(0.1336673f, 0.499f), new Vector2(0.1598611f, 0.6404466f), new Vector2(0.2502298f, 0.7714156f), new Vector2(0.358934f, 0.8408293f), new Vector2(0.503f, 0.8657134f), new Vector2(0.6588531f, 0.8303517f), new Vector2(0.7675575f, 0.7465315f), new Vector2(0.836971f, 0.6338982f), new Vector2(0.8592358f, 0.499f), new Vector2(0.8264935f, 0.3496954f), new Vector2(0.7531509f, 0.2436104f), new Vector2(0.6392078f, 0.1715775f), new Vector2(0.4977612f, 0.1414546f) }, new Vector3[] { new Vector2(0.4974829f, 0.05820844f), new Vector2(0.3182628f, 0.0940524f), new Vector2(0.1846624f, 0.1804039f), new Vector2(0.08853531f, 0.3156337f), new Vector2(0.04454491f, 0.503f), new Vector2(0.07713038f, 0.6789616f), new Vector2(0.1895502f, 0.8418889f), new Vector2(0.3247799f, 0.9282404f), new Vector2(0.504f, 0.9591966f), new Vector2(0.6978835f, 0.9152062f), new Vector2(0.8331133f, 0.8109328f), new Vector2(0.9194647f, 0.6708152f), new Vector2(0.9471624f, 0.503f), new Vector2(0.9064305f, 0.3172629f), new Vector2(0.8151912f, 0.1852918f), new Vector2(0.6734444f, 0.09568173f), new Vector2(0.4974829f, 0.05820844f) }, new Vector3[] { new Vector2(0.6226891f, 0.5983194f), new Vector2(0.5689076f, 0.6453782f), new Vector2(0.5184874f, 0.6605042f), new Vector2(0.4596639f, 0.6621849f), new Vector2(0.4193277f, 0.6336135f), new Vector2(0.4092437f, 0.594958f), new Vector2(0.4260504f, 0.5462185f), new Vector2(0.4596639f, 0.5142857f), new Vector2(0.5487395f, 0.4605042f), new Vector2(0.5756302f, 0.4268908f), new Vector2(0.5773109f, 0.3781513f), new Vector2(0.5537815f, 0.3445378f), new Vector2(0.515126f, 0.3243698f), new Vector2(0.4630252f, 0.3294118f), new Vector2(0.412605f, 0.3529412f), new Vector2(0.3773109f, 0.3932773f) }, new Vector3[] { new Vector2(0.5f, 0.3243698f), new Vector2(0.5f, 0.2184874f) }, new Vector3[] { new Vector2(0.494958f, 0.6638656f), new Vector2(0.494958f, 0.7731093f) } }; } return icon_coin; }
        Vector3[][] Icon_coins() { if (icon_coins == null) { icon_coins = new Vector3[][] { new Vector3[] { new Vector2(0.0671406f, 0.3270142f), new Vector2(0.05608215f, 0.3649289f), new Vector2(0.0592417f, 0.4344392f), new Vector2(0.09873617f, 0.5371248f), new Vector2(0.1524487f, 0.6366509f), new Vector2(0.2093207f, 0.7172196f), new Vector2(0.2646129f, 0.7740916f), new Vector2(0.3167457f, 0.7977883f), new Vector2(0.3388626f, 0.8041074f), new Vector2(0.3546604f, 0.7883096f), new Vector2(0.3609795f, 0.7598736f), new Vector2(0.3562401f, 0.7109005f), new Vector2(0.3372828f, 0.6413902f), new Vector2(0.2835703f, 0.5229068f), new Vector2(0.2251185f, 0.4296998f), new Vector2(0.1619273f, 0.3649289f), new Vector2(0.1192733f, 0.3333333f), new Vector2(0.08135861f, 0.3206951f), new Vector2(0.0671406f, 0.3270142f) }, new Vector3[] { new Vector2(0.07503949f, 0.3238547f), new Vector2(0.157188f, 0.28594f), new Vector2(0.1777251f, 0.2890995f), new Vector2(0.2219589f, 0.3080569f), new Vector2(0.2740916f, 0.3522907f), new Vector2(0.3246445f, 0.4091627f), new Vector2(0.3736177f, 0.4818325f), new Vector2(0.4099526f, 0.5545024f), new Vector2(0.4320695f, 0.6240126f), new Vector2(0.4383886f, 0.685624f), new Vector2(0.4273302f, 0.7456556f), new Vector2(0.3404423f, 0.7993681f) }, new Vector3[] { new Vector2(0.4194313f, 0.586098f), new Vector2(0.471564f, 0.5703002f), new Vector2(0.5742496f, 0.5608215f), new Vector2(0.6753554f, 0.5624012f), new Vector2(0.7496051f, 0.5718799f), new Vector2(0.8080569f, 0.5892575f), new Vector2(0.8349131f, 0.6097946f), new Vector2(0.8412322f, 0.6350711f), new Vector2(0.8238547f, 0.6603476f), new Vector2(0.7827804f, 0.685624f), new Vector2(0.7053713f, 0.6982622f), new Vector2(0.6058452f, 0.7077409f), new Vector2(0.4889416f, 0.7014218f), new Vector2(0.4368089f, 0.6919431f) }, new Vector3[] { new Vector2(0.378357f, 0.4913112f), new Vector2(0.4241706f, 0.4707741f), new Vector2(0.4905213f, 0.4565561f), new Vector2(0.57109f, 0.4518167f), new Vector2(0.6627172f, 0.4533965f), new Vector2(0.7480253f, 0.4660348f), new Vector2(0.7954186f, 0.4834123f), new Vector2(0.8317536f, 0.5023696f), new Vector2(0.8412322f, 0.5276461f) }, new Vector3[] { new Vector2(0.8396524f, 0.6319115f), new Vector2(0.8396524f, 0.2906793f), new Vector2(0.8270142f, 0.2733018f), new Vector2(0.7985782f, 0.2527646f), new Vector2(0.7306477f, 0.2290679f), new Vector2(0.650079f, 0.2180095f), new Vector2(0.5505529f, 0.2164297f), new Vector2(0.457346f, 0.2274881f), new Vector2(0.3846762f, 0.257504f), new Vector2(0.3325434f, 0.300158f), new Vector2(0.3230648f, 0.3238547f), new Vector2(0.3230648f, 0.4075829f) }, new Vector3[] { new Vector2(0.3388626f, 0.4296998f), new Vector2(0.3467615f, 0.4060031f), new Vector2(0.3720379f, 0.3759874f), new Vector2(0.4273302f, 0.3507109f), new Vector2(0.514218f, 0.3333333f), new Vector2(0.6169037f, 0.3254344f), new Vector2(0.7053713f, 0.3333333f), new Vector2(0.7875198f, 0.3522907f), new Vector2(0.8175355f, 0.3680885f), new Vector2(0.8380727f, 0.3981043f) } }; } return icon_coins; }
        Vector3[][] Icon_moneyBills() { if (icon_moneyBills == null) { icon_moneyBills = new Vector3[][] { new Vector3[] { new Vector2(0.2234043f, 0.5159575f), new Vector2(0.2624114f, 0.535461f), new Vector2(0.2925532f, 0.5638298f), new Vector2(0.3085106f, 0.5886525f), new Vector2(0.3173759f, 0.6223404f), new Vector2(0.8049645f, 0.6223404f), new Vector2(0.8173759f, 0.5797873f), new Vector2(0.8421986f, 0.5478724f), new Vector2(0.8705674f, 0.5248227f), new Vector2(0.9024823f, 0.5124114f), new Vector2(0.9024823f, 0.3812057f), new Vector2(0.8705674f, 0.3705674f), new Vector2(0.8421986f, 0.3492908f), new Vector2(0.819149f, 0.3191489f), new Vector2(0.8031915f, 0.2730497f), new Vector2(0.3244681f, 0.2730497f), new Vector2(0.3102837f, 0.3120568f), new Vector2(0.2836879f, 0.3510638f), new Vector2(0.2535461f, 0.3723404f), new Vector2(0.2198582f, 0.3847518f), new Vector2(0.2198582f, 0.5195035f) }, new Vector3[] { new Vector2(0.5696161f, 0.3376006f), new Vector2(0.5260596f, 0.346312f), new Vector2(0.4935902f, 0.3672983f), new Vector2(0.4702281f, 0.4001637f), new Vector2(0.4595369f, 0.4457f), new Vector2(0.4674563f, 0.4884646f), new Vector2(0.4947781f, 0.5280614f), new Vector2(0.5276435f, 0.5490477f), new Vector2(0.5712f, 0.5565711f), new Vector2(0.6183202f, 0.54588f), new Vector2(0.6511856f, 0.520538f), new Vector2(0.672172f, 0.4864847f), new Vector2(0.6789034f, 0.4457f), new Vector2(0.6690042f, 0.4005596f), new Vector2(0.64683f, 0.3684862f), new Vector2(0.6123807f, 0.3467079f), new Vector2(0.5696161f, 0.3376006f) }, new Vector3[] { new Vector2(0.9485816f, 0.6595744f), new Vector2(0.9485816f, 0.2340426f), new Vector2(0.1702128f, 0.2340426f), new Vector2(0.1702128f, 0.6595744f), new Vector2(0.9485816f, 0.6595744f) }, new Vector3[] { new Vector2(0.1702128f, 0.2358156f), new Vector2(0.09219858f, 0.6542553f), new Vector2(0.8368794f, 0.819149f), new Vector2(0.8652482f, 0.6613475f) }, new Vector3[] { new Vector2(0.1684397f, 0.2340426f), new Vector2(0.01241135f, 0.6347518f), new Vector2(0.7234042f, 0.9237589f), new Vector2(0.7641844f, 0.8067376f) } }; } return icon_moneyBills; }
        Vector3[][] Icon_moneyBag() { if (icon_moneyBag == null) { icon_moneyBag = new Vector3[][] { new Vector3[] { new Vector2(0.4379432f, 0.7907801f), new Vector2(0.6099291f, 0.7907801f), new Vector2(0.7340425f, 0.6702127f), new Vector2(0.8475177f, 0.5177305f), new Vector2(0.9042553f, 0.4007092f), new Vector2(0.9202127f, 0.2429078f), new Vector2(0.8882979f, 0.1312057f), new Vector2(0.8351064f, 0.08687943f), new Vector2(0.7375886f, 0.07269503f), new Vector2(0.3031915f, 0.07269503f), new Vector2(0.2092199f, 0.08687943f), new Vector2(0.1507092f, 0.1294326f), new Vector2(0.1223404f, 0.2216312f), new Vector2(0.1187943f, 0.3280142f), new Vector2(0.1453901f, 0.4379432f), new Vector2(0.2198582f, 0.569149f), new Vector2(0.3138298f, 0.6897163f), new Vector2(0.3723404f, 0.7553192f), new Vector2(0.4379432f, 0.7907801f) }, new Vector3[] { new Vector2(0.4379432f, 0.8138298f), new Vector2(0.6010638f, 0.8138298f), new Vector2(0.6968085f, 0.9432624f), new Vector2(0.3546099f, 0.9432624f), new Vector2(0.4379432f, 0.8138298f) }, new Vector3[] { new Vector2(0.6099291f, 0.5f), new Vector2(0.5762411f, 0.5248227f), new Vector2(0.5336879f, 0.5425532f), new Vector2(0.4858156f, 0.5390071f), new Vector2(0.4485815f, 0.5195035f), new Vector2(0.4414894f, 0.4893617f), new Vector2(0.4521277f, 0.4574468f), new Vector2(0.4840426f, 0.4255319f), new Vector2(0.5656028f, 0.3812057f), new Vector2(0.5762411f, 0.356383f), new Vector2(0.5762411f, 0.320922f), new Vector2(0.5585107f, 0.2890071f), new Vector2(0.5230497f, 0.2765957f), new Vector2(0.4804965f, 0.2801418f), new Vector2(0.4503546f, 0.2925532f), new Vector2(0.4219858f, 0.3244681f) }, new Vector3[] { new Vector2(0.5124114f, 0.5425532f), new Vector2(0.5124114f, 0.643617f) }, new Vector3[] { new Vector2(0.5159575f, 0.2783688f), new Vector2(0.5159575f, 0.1737589f) } }; } return icon_moneyBag; }
        Vector3[][] Icon_chestTreasureBox_closed() { if (icon_chestTreasureBox_closed == null) { icon_chestTreasureBox_closed = new Vector3[][] { new Vector3[] { new Vector2(0.320922f, 0.6489362f), new Vector2(0.320922f, 0.5177305f), new Vector2(0.4343972f, 0.4929078f), new Vector2(0.4343972f, 0.6294326f), new Vector2(0.320922f, 0.6489362f) }, new Vector3[] { new Vector2(0.6613475f, 0.07092199f), new Vector2(0.6613475f, 0.5319149f), new Vector2(0.6719858f, 0.6347518f), new Vector2(0.6968085f, 0.7234042f), new Vector2(0.7287234f, 0.7730497f), new Vector2(0.7677305f, 0.8156028f), new Vector2(0.7960993f, 0.8386525f), new Vector2(0.8297873f, 0.8386525f), new Vector2(0.8546099f, 0.8262411f), new Vector2(0.8812057f, 0.7907801f), new Vector2(0.9042553f, 0.7429078f), new Vector2(0.9166667f, 0.6897163f), new Vector2(0.9202127f, 0.6205674f), new Vector2(0.9202127f, 0.2553191f), new Vector2(0.6613475f, 0.07092199f) }, new Vector3[] { new Vector2(0.6613475f, 0.07092199f), new Vector2(0.08687943f, 0.2588652f), new Vector2(0.08687943f, 0.6595744f), new Vector2(0.09397163f, 0.7198582f), new Vector2(0.1152482f, 0.7748227f), new Vector2(0.1453901f, 0.8280142f), new Vector2(0.1719858f, 0.8634752f), new Vector2(0.2039007f, 0.8847518f), new Vector2(0.2393617f, 0.8989362f), new Vector2(0.2677305f, 0.9024823f), new Vector2(0.285461f, 0.8989362f), new Vector2(0.2960993f, 0.8847518f) }, new Vector3[] { new Vector2(0.4840426f, 0.1258865f), new Vector2(0.4840426f, 0.5886525f), new Vector2(0.5f, 0.6666667f), new Vector2(0.5283688f, 0.7340425f), new Vector2(0.5602837f, 0.7801418f), new Vector2(0.5992908f, 0.819149f), new Vector2(0.6471631f, 0.8492908f), new Vector2(0.6843972f, 0.858156f), new Vector2(0.7074468f, 0.8546099f), new Vector2(0.7216312f, 0.8439716f) }, new Vector3[] { new Vector2(0.248227f, 0.2056738f), new Vector2(0.248227f, 0.608156f), new Vector2(0.2606383f, 0.6826241f), new Vector2(0.2836879f, 0.7517731f), new Vector2(0.3138298f, 0.7996454f), new Vector2(0.3439716f, 0.8351064f), new Vector2(0.3794326f, 0.8670213f), new Vector2(0.4131206f, 0.8812057f), new Vector2(0.4414894f, 0.8829787f), new Vector2(0.4592199f, 0.8794326f), new Vector2(0.4680851f, 0.8687943f) }, new Vector3[] { new Vector2(0.2180851f, 0.893617f), new Vector2(0.7960993f, 0.8368794f) }, new Vector3[] { new Vector2(0.4379432f, 0.535461f), new Vector2(0.6648936f, 0.4840426f), new Vector2(0.9202127f, 0.6117021f) }, new Vector3[] { new Vector2(0.4361702f, 0.5762411f), new Vector2(0.6631206f, 0.5265958f), new Vector2(0.9184397f, 0.6507092f) }, new Vector3[] { new Vector2(0.320922f, 0.5975177f), new Vector2(0.08687943f, 0.6507092f) }, new Vector3[] { new Vector2(0.3191489f, 0.5585107f), new Vector2(0.08687943f, 0.6117021f) }, new Vector3[] { new Vector2(0.3776596f, 0.5975177f), new Vector2(0.3776596f, 0.5407801f) } }; } return icon_chestTreasureBox_closed; }
        Vector3[][] Icon_lootbox() { if (icon_lootbox == null) { icon_lootbox = new Vector3[][] { new Vector3[] { new Vector2(0.2777778f, 0.8529412f), new Vector2(0.2777778f, 0.1748366f), new Vector2(0.3676471f, 0.1748366f), new Vector2(0.3676471f, 0.8513072f), new Vector2(0.2777778f, 0.8529412f) }, new Vector3[] { new Vector2(0.6372549f, 0.8464052f), new Vector2(0.6372549f, 0.1748366f), new Vector2(0.7254902f, 0.1748366f), new Vector2(0.7254902f, 0.8513072f), new Vector2(0.6372549f, 0.8464052f) }, new Vector3[] { new Vector2(0.4166667f, 0.5833333f), new Vector2(0.5849673f, 0.5833333f), new Vector2(0.5849673f, 0.4150327f), new Vector2(0.4166667f, 0.4150327f), new Vector2(0.4166667f, 0.5833333f) }, new Vector3[] { new Vector2(0.2794118f, 0.5294118f), new Vector2(0.0751634f, 0.5294118f), new Vector2(0.0751634f, 0.4722222f), new Vector2(0.2777778f, 0.4722222f) }, new Vector3[] { new Vector2(0.7254902f, 0.5294118f), new Vector2(0.9183006f, 0.5294118f), new Vector2(0.9183006f, 0.4705882f), new Vector2(0.7271242f, 0.4705882f) }, new Vector3[] { new Vector2(0.372549f, 0.8202614f), new Vector2(0.4035948f, 0.877451f), new Vector2(0.5947713f, 0.877451f), new Vector2(0.6372549f, 0.8055556f) }, new Vector3[] { new Vector2(0.3937908f, 0.7941176f), new Vector2(0.6029412f, 0.7941176f), new Vector2(0.5751634f, 0.8447713f), new Vector2(0.4232026f, 0.8447713f), new Vector2(0.3937908f, 0.7941176f) }, new Vector3[] { new Vector2(0.2761438f, 0.7892157f), new Vector2(0.1911765f, 0.7892157f), new Vector2(0.1127451f, 0.5310457f) }, new Vector3[] { new Vector2(0.2777778f, 0.2107843f), new Vector2(0.1911765f, 0.2107843f), new Vector2(0.1127451f, 0.4705882f) }, new Vector3[] { new Vector2(0.3692811f, 0.2124183f), new Vector2(0.6372549f, 0.2124183f) }, new Vector3[] { new Vector2(0.7271242f, 0.2173203f), new Vector2(0.8022876f, 0.2173203f), new Vector2(0.8823529f, 0.4673203f) }, new Vector3[] { new Vector2(0.7254902f, 0.7908497f), new Vector2(0.8006536f, 0.7908497f), new Vector2(0.8839869f, 0.5294118f) }, new Vector3[] { new Vector2(0.4542484f, 0.4624183f), new Vector2(0.5441176f, 0.4624183f) }, new Vector3[] { new Vector2(0.498366f, 0.4607843f), new Vector2(0.498366f, 0.5424837f) }, new Vector3[] { new Vector2(0.3676471f, 0.5294118f), new Vector2(0.4166667f, 0.5294118f) }, new Vector3[] { new Vector2(0.3676471f, 0.4705882f), new Vector2(0.4150327f, 0.4705882f) }, new Vector3[] { new Vector2(0.6372549f, 0.5261438f), new Vector2(0.5833333f, 0.5261438f) }, new Vector3[] { new Vector2(0.6372549f, 0.4689542f), new Vector2(0.5833333f, 0.4689542f) } }; } return icon_lootbox; }
        Vector3[][] Icon_crown() { if (icon_crown == null) { icon_crown = new Vector3[][] { new Vector3[] { new Vector2(0.5028f, 0.117123f), new Vector2(0.4456886f, 0.1408033f), new Vector2(0.4231227f, 0.1929f), new Vector2(0.4448529f, 0.2475041f), new Vector2(0.501407f, 0.2717415f), new Vector2(0.5560111f, 0.2500114f), new Vector2(0.5780199f, 0.1926214f), new Vector2(0.5557325f, 0.138296f), new Vector2(0.5028f, 0.117123f) }, new Vector3[] { new Vector2(0.7322695f, 0.2748227f), new Vector2(0.7677305f, 0.2624114f), new Vector2(0.7943262f, 0.2216312f), new Vector2(0.7960993f, 0.1861702f), new Vector2(0.7801418f, 0.1507092f), new Vector2(0.7624114f, 0.1294326f), new Vector2(0.7287234f, 0.1205674f), new Vector2(0.6914893f, 0.1400709f), new Vector2(0.6719858f, 0.179078f), new Vector2(0.6737589f, 0.2251773f), new Vector2(0.6985816f, 0.2606383f), new Vector2(0.7322695f, 0.2748227f) }, new Vector3[] { new Vector2(0.2712766f, 0.2748227f), new Vector2(0.3049645f, 0.2606383f), new Vector2(0.3244681f, 0.2411347f), new Vector2(0.3368794f, 0.2074468f), new Vector2(0.3351064f, 0.1755319f), new Vector2(0.3173759f, 0.1471631f), new Vector2(0.2960993f, 0.1241135f), new Vector2(0.2641844f, 0.1187943f), new Vector2(0.2287234f, 0.1365248f), new Vector2(0.212766f, 0.1826241f), new Vector2(0.2163121f, 0.2304965f), new Vector2(0.2411347f, 0.2606383f), new Vector2(0.2712766f, 0.2748227f) }, new Vector3[] { new Vector2(0.1329787f, 0.3156028f), new Vector2(0.1329787f, 0.07978723f), new Vector2(0.8599291f, 0.07978723f), new Vector2(0.8599291f, 0.3156028f), new Vector2(0.1329787f, 0.3156028f), new Vector2(0.05496454f, 0.8634752f), new Vector2(0.3404255f, 0.5975177f), new Vector2(0.5f, 0.9007092f), new Vector2(0.6595744f, 0.5992908f), new Vector2(0.9379433f, 0.858156f), new Vector2(0.858156f, 0.3138298f) } }; } return icon_crown; }
        Vector3[][] Icon_trophy() { if (icon_trophy == null) { icon_trophy = new Vector3[][] { new Vector3[] { new Vector2(0.2259259f, 0.9518518f), new Vector2(0.7740741f, 0.9518518f), new Vector2(0.7462963f, 0.7611111f), new Vector2(0.6981481f, 0.587037f), new Vector2(0.6462963f, 0.4925926f), new Vector2(0.6018519f, 0.4314815f), new Vector2(0.5537037f, 0.4f), new Vector2(0.5037037f, 0.387037f), new Vector2(0.4481482f, 0.3981481f), new Vector2(0.3981481f, 0.4296296f), new Vector2(0.3351852f, 0.5166667f), new Vector2(0.2796296f, 0.65f), new Vector2(0.2388889f, 0.7962963f), new Vector2(0.2259259f, 0.9518518f) }, new Vector3[] { new Vector2(0.7611111f, 0.8481482f), new Vector2(0.9f, 0.8481482f), new Vector2(0.8925926f, 0.7592593f), new Vector2(0.8574074f, 0.6740741f), new Vector2(0.8111111f, 0.6f), new Vector2(0.7666667f, 0.55f), new Vector2(0.712963f, 0.5277778f), new Vector2(0.6685185f, 0.5296296f) }, new Vector3[] { new Vector2(0.2333333f, 0.8444445f), new Vector2(0.09814814f, 0.8444445f), new Vector2(0.1055556f, 0.7574074f), new Vector2(0.1462963f, 0.6574074f), new Vector2(0.2018518f, 0.5796296f), new Vector2(0.2555556f, 0.5388889f), new Vector2(0.2907407f, 0.5296296f), new Vector2(0.3314815f, 0.5277778f) }, new Vector3[] { new Vector2(0.2722222f, 0.1425926f), new Vector2(0.7240741f, 0.1425926f), new Vector2(0.7240741f, 0.03888889f), new Vector2(0.2722222f, 0.03888889f), new Vector2(0.2722222f, 0.1462963f) }, new Vector3[] { new Vector2(0.3111111f, 0.1444445f), new Vector2(0.3537037f, 0.1611111f), new Vector2(0.3981481f, 0.212963f), new Vector2(0.4222222f, 0.2722222f), new Vector2(0.4425926f, 0.3425926f), new Vector2(0.45f, 0.3962963f) }, new Vector3[] { new Vector2(0.6851852f, 0.1425926f), new Vector2(0.6277778f, 0.1703704f), new Vector2(0.587037f, 0.2333333f), new Vector2(0.5574074f, 0.3148148f), new Vector2(0.5481482f, 0.3962963f) } }; } return icon_trophy; }
        Vector3[][] Icon_awardMedal() { if (icon_awardMedal == null) { icon_awardMedal = new Vector3[][] { new Vector3[] { new Vector2(0.497568f, 0.4011138f), new Vector2(0.4086871f, 0.4188899f), new Vector2(0.3424304f, 0.4617144f), new Vector2(0.294758f, 0.5287791f), new Vector2(0.2729417f, 0.6217f), new Vector2(0.2891019f, 0.7089649f), new Vector2(0.3448544f, 0.7897657f), new Vector2(0.4119191f, 0.8325902f), new Vector2(0.5008f, 0.8479423f), new Vector2(0.5969529f, 0.8261261f), new Vector2(0.6640177f, 0.7744135f), new Vector2(0.7068421f, 0.7049249f), new Vector2(0.7205783f, 0.6217f), new Vector2(0.700378f, 0.5295871f), new Vector2(0.6551296f, 0.4641384f), new Vector2(0.5848328f, 0.419698f), new Vector2(0.497568f, 0.4011138f) }, new Vector3[] { new Vector2(0.4929577f, 0.2869718f), new Vector2(0.5721831f, 0.3626761f), new Vector2(0.681338f, 0.3433098f), new Vector2(0.6954225f, 0.4295775f), new Vector2(0.7887324f, 0.4542254f), new Vector2(0.7623239f, 0.5616197f), new Vector2(0.8327465f, 0.6302817f), new Vector2(0.7658451f, 0.7024648f), new Vector2(0.7922535f, 0.7957746f), new Vector2(0.693662f, 0.818662f), new Vector2(0.6637324f, 0.9242958f), new Vector2(0.5757042f, 0.8908451f), new Vector2(0.4964789f, 0.9665493f), new Vector2(0.4278169f, 0.8943662f), new Vector2(0.3309859f, 0.9172535f), new Vector2(0.3045775f, 0.8204225f), new Vector2(0.2112676f, 0.8045775f), new Vector2(0.2271127f, 0.6919014f), new Vector2(0.1619718f, 0.6232395f), new Vector2(0.2306338f, 0.540493f), new Vector2(0.2112676f, 0.4507042f), new Vector2(0.306338f, 0.4295775f), new Vector2(0.3274648f, 0.3327465f), new Vector2(0.4260563f, 0.3626761f), new Vector2(0.4929577f, 0.2869718f) }, new Vector3[] { new Vector2(0.3133803f, 0.3978873f), new Vector2(0.2341549f, 0.08626761f), new Vector2(0.3644366f, 0.1760563f), new Vector2(0.4295775f, 0.0334507f), new Vector2(0.4982394f, 0.290493f) }, new Vector3[] { new Vector2(0.4929577f, 0.2799296f), new Vector2(0.556338f, 0.03521127f), new Vector2(0.6285211f, 0.165493f), new Vector2(0.7658451f, 0.06514084f), new Vector2(0.6866197f, 0.3785211f) } }; } return icon_awardMedal; }
        Vector3[][] Icon_sword() { if (icon_sword == null) { icon_sword = new Vector3[][] { new Vector3[] { new Vector2(0.2018518f, 0.4185185f), new Vector2(0.4185185f, 0.1981481f), new Vector2(0.3888889f, 0.1685185f), new Vector2(0.1740741f, 0.3833333f), new Vector2(0.2018518f, 0.4185185f) }, new Vector3[] { new Vector2(0.07037037f, 0.2092593f), new Vector2(0.2074074f, 0.07037037f), new Vector2(0.1685185f, 0.03518518f), new Vector2(0.03888889f, 0.1685185f), new Vector2(0.07037037f, 0.2092593f) }, new Vector3[] { new Vector2(0.2555556f, 0.3703704f), new Vector2(0.7407407f, 0.85f), new Vector2(0.9388889f, 0.9296296f), new Vector2(0.85f, 0.7314815f), new Vector2(0.3685185f, 0.2518519f) }, new Vector3[] { new Vector2(0.1092593f, 0.1740741f), new Vector2(0.2481481f, 0.3074074f) }, new Vector3[] { new Vector2(0.1759259f, 0.1055556f), new Vector2(0.3166667f, 0.2407407f) }, new Vector3[] { new Vector2(0.3111111f, 0.3111111f), new Vector2(0.8333333f, 0.8314815f) } }; } return icon_sword; }
        Vector3[][] Icon_shield() { if (icon_shield == null) { icon_shield = new Vector3[][] { new Vector3[] { new Vector2(0.2592593f, 0.8981481f), new Vector2(0.7333333f, 0.8981481f), new Vector2(0.7407407f, 0.8407407f), new Vector2(0.7703704f, 0.7925926f), new Vector2(0.8092592f, 0.7629629f), new Vector2(0.8592592f, 0.7425926f), new Vector2(0.9055555f, 0.7370371f), new Vector2(0.887037f, 0.4462963f), new Vector2(0.8611111f, 0.337037f), new Vector2(0.7981482f, 0.2259259f), new Vector2(0.6907408f, 0.1240741f), new Vector2(0.5f, 0.04629629f), new Vector2(0.3203704f, 0.1166667f), new Vector2(0.1944444f, 0.2277778f), new Vector2(0.1351852f, 0.3407407f), new Vector2(0.1074074f, 0.5222222f), new Vector2(0.09259259f, 0.7444444f), new Vector2(0.1351852f, 0.7518519f), new Vector2(0.1833333f, 0.7740741f), new Vector2(0.2277778f, 0.8092592f), new Vector2(0.2481481f, 0.85f), new Vector2(0.2592593f, 0.8981481f) }, new Vector3[] { new Vector2(0.3555556f, 0.7851852f), new Vector2(0.6462963f, 0.7851852f), new Vector2(0.6574074f, 0.7296296f), new Vector2(0.6907408f, 0.6888889f), new Vector2(0.7333333f, 0.6592593f), new Vector2(0.7796296f, 0.6425926f), new Vector2(0.7703704f, 0.5f), new Vector2(0.7444444f, 0.3740741f), new Vector2(0.7074074f, 0.3018518f), new Vector2(0.6277778f, 0.237037f), new Vector2(0.5f, 0.1703704f), new Vector2(0.3796296f, 0.2203704f), new Vector2(0.2833333f, 0.3111111f), new Vector2(0.2388889f, 0.4111111f), new Vector2(0.2185185f, 0.5277778f), new Vector2(0.2111111f, 0.6537037f), new Vector2(0.2555556f, 0.6611111f), new Vector2(0.3018518f, 0.6888889f), new Vector2(0.3333333f, 0.7277778f), new Vector2(0.3555556f, 0.7851852f) }, new Vector3[] { new Vector2(0.4981481f, 0.1703704f), new Vector2(0.4981481f, 0.7833334f) } }; } return icon_shield; }
        Vector3[][] Icon_gun() { if (icon_gun == null) { icon_gun = new Vector3[][] { new Vector3[] { new Vector2(0.4185185f, 0.5166667f), new Vector2(0.637037f, 0.5166667f), new Vector2(0.6648148f, 0.5685185f), new Vector2(0.9055555f, 0.5685185f), new Vector2(0.9055555f, 0.6333333f), new Vector2(0.9388889f, 0.6333333f), new Vector2(0.9388889f, 0.7333333f), new Vector2(0.9f, 0.7333333f), new Vector2(0.8666667f, 0.7703704f), new Vector2(0.8314815f, 0.7333333f), new Vector2(0.2648148f, 0.7333333f), new Vector2(0.2277778f, 0.7722222f), new Vector2(0.1888889f, 0.7296296f), new Vector2(0.06666667f, 0.7296296f), new Vector2(0.06666667f, 0.6796296f), new Vector2(0.1166667f, 0.6351852f), new Vector2(0.1111111f, 0.5796296f), new Vector2(0.06851852f, 0.5796296f), new Vector2(0.06851852f, 0.5462963f), new Vector2(0.1f, 0.5388889f), new Vector2(0.1092593f, 0.5018519f), new Vector2(0.1055556f, 0.412963f), new Vector2(0.05925926f, 0.2888889f), new Vector2(0.05925926f, 0.2037037f), new Vector2(0.3111111f, 0.2037037f), new Vector2(0.4185185f, 0.5166667f) }, new Vector3[] { new Vector2(0.3851852f, 0.4166667f), new Vector2(0.5092593f, 0.4166667f), new Vector2(0.5444444f, 0.4370371f), new Vector2(0.5703704f, 0.4740741f), new Vector2(0.5759259f, 0.5166667f) }, new Vector3[] { new Vector2(0.4462963f, 0.5148148f), new Vector2(0.4518518f, 0.4722222f), new Vector2(0.4796296f, 0.4481482f) }, new Vector3[] { new Vector2(0.8351852f, 0.6444445f), new Vector2(0.4462963f, 0.6444445f) } }; } return icon_gun; }
        Vector3[][] Icon_bullet() { if (icon_bullet == null) { icon_bullet = new Vector3[][] { new Vector3[] { new Vector2(0.1148148f, 0.6574074f), new Vector2(0.1148148f, 0.3518519f), new Vector2(0.04814815f, 0.3518519f), new Vector2(0.04814815f, 0.6555555f), new Vector2(0.1148148f, 0.6555555f) }, new Vector3[] { new Vector2(0.1148148f, 0.6333333f), new Vector2(0.6888889f, 0.6333333f), new Vector2(0.6888889f, 0.3740741f), new Vector2(0.112963f, 0.3740741f) }, new Vector3[] { new Vector2(0.6907408f, 0.6055555f), new Vector2(0.7851852f, 0.6074074f), new Vector2(0.8481482f, 0.5925926f), new Vector2(0.9185185f, 0.5592592f), new Vector2(0.9481481f, 0.5222222f), new Vector2(0.9574074f, 0.5018519f), new Vector2(0.9407408f, 0.4703704f), new Vector2(0.8944445f, 0.4388889f), new Vector2(0.8314815f, 0.4111111f), new Vector2(0.7518519f, 0.3944444f), new Vector2(0.6888889f, 0.3944444f) } }; } return icon_bullet; }
        Vector3[][] Icon_rocket() { if (icon_rocket == null) { icon_rocket = new Vector3[][] { new Vector3[] { new Vector2(0.4743178f, 0.2744783f), new Vector2(0.4390048f, 0.2487962f), new Vector2(0.4213483f, 0.2182986f), new Vector2(0.4197432f, 0.1573034f), new Vector2(0.453451f, 0.08507223f), new Vector2(0.5f, 0.006420546f), new Vector2(0.5481541f, 0.09470305f), new Vector2(0.5786517f, 0.1621188f), new Vector2(0.5786517f, 0.200642f), new Vector2(0.5658106f, 0.2359551f), new Vector2(0.5433387f, 0.2600321f), new Vector2(0.5176565f, 0.2744783f), new Vector2(0.5385233f, 0.247191f), new Vector2(0.5401284f, 0.2166934f), new Vector2(0.5272873f, 0.176565f), new Vector2(0.4983949f, 0.141252f), new Vector2(0.4662921f, 0.1861958f), new Vector2(0.4550562f, 0.2166934f), new Vector2(0.4566613f, 0.2455859f), new Vector2(0.4759229f, 0.2728732f) }, new Vector3[] { new Vector2(0.503f, 0.6394702f), new Vector2(0.4668765f, 0.6544483f), new Vector2(0.4526033f, 0.6874f), new Vector2(0.4663479f, 0.7219376f), new Vector2(0.5021189f, 0.7372681f), new Vector2(0.5366566f, 0.7235235f), new Vector2(0.5505773f, 0.6872238f), new Vector2(0.5364804f, 0.6528624f), new Vector2(0.503f, 0.6394702f) }, new Vector3[] { new Vector2(0.5016052f, 0.9919743f), new Vector2(0.581862f, 0.8057785f), new Vector2(0.5722311f, 0.3434992f), new Vector2(0.4245586f, 0.3434992f), new Vector2(0.4085072f, 0.8057785f), new Vector2(0.5016052f, 0.9919743f) }, new Vector3[] { new Vector2(0.4566613f, 0.3451043f), new Vector2(0.4325843f, 0.2728732f), new Vector2(0.5690209f, 0.2728732f), new Vector2(0.5433387f, 0.3451043f) }, new Vector3[] { new Vector2(0.5738363f, 0.4125201f), new Vector2(0.6813804f, 0.3515249f), new Vector2(0.6813804f, 0.4654896f), new Vector2(0.5770466f, 0.5922953f) }, new Vector3[] { new Vector2(0.4213483f, 0.4173355f), new Vector2(0.3202247f, 0.3547352f), new Vector2(0.3202247f, 0.4670947f), new Vector2(0.418138f, 0.5842696f) }, new Vector3[] { new Vector2(0.4085072f, 0.8073837f), new Vector2(0.581862f, 0.8073837f) } }; } return icon_rocket; }
        Vector3[][] Icon_crosshair() { if (icon_crosshair == null) { icon_crosshair = new Vector3[][] { new Vector3[] { new Vector2(0.4964707f, 0.1216253f), new Vector2(0.3444149f, 0.1520364f), new Vector2(0.2310643f, 0.2252997f), new Vector2(0.1495071f, 0.3400327f), new Vector2(0.1121843f, 0.499f), new Vector2(0.1398308f, 0.6482912f), new Vector2(0.2352112f, 0.7865236f), new Vector2(0.3499442f, 0.8597869f), new Vector2(0.502f, 0.8860511f), new Vector2(0.6664966f, 0.8487283f), new Vector2(0.7812297f, 0.7602595f), new Vector2(0.8544929f, 0.6413796f), new Vector2(0.8779925f, 0.499f), new Vector2(0.8434343f, 0.341415f), new Vector2(0.7660241f, 0.2294466f), new Vector2(0.6457618f, 0.1534187f), new Vector2(0.4964707f, 0.1216253f) }, new Vector3[] { new Vector2(0.501f, 0.4098177f), new Vector2(0.4330317f, 0.4379997f), new Vector2(0.4061759f, 0.5f), new Vector2(0.4320371f, 0.5649843f), new Vector2(0.4993422f, 0.5938294f), new Vector2(0.5643265f, 0.5679682f), new Vector2(0.5905192f, 0.4996684f), new Vector2(0.563995f, 0.4350157f), new Vector2(0.501f, 0.4098177f) }, new Vector3[] { new Vector2(0.02407407f, 0.5055556f), new Vector2(0.2259259f, 0.5055556f) }, new Vector3[] { new Vector2(0.3481481f, 0.5018519f), new Vector2(0.65f, 0.5018519f) }, new Vector3[] { new Vector2(0.7666667f, 0.5018519f), new Vector2(0.9685185f, 0.5018519f) }, new Vector3[] { new Vector2(0.5f, 0.9777778f), new Vector2(0.5f, 0.7777778f) }, new Vector3[] { new Vector2(0.5f, 0.6518518f), new Vector2(0.5f, 0.3444445f) }, new Vector3[] { new Vector2(0.4981481f, 0.2296296f), new Vector2(0.4981481f, 0.02962963f) } }; } return icon_crosshair; }
        Vector3[][] Icon_arrow() { if (icon_arrow == null) { icon_arrow = new Vector3[][] { new Vector3[] { new Vector2(0.7814815f, 0.5629629f), new Vector2(0.7814815f, 0.4388889f), new Vector2(0.9611111f, 0.5018519f), new Vector2(0.7814815f, 0.5629629f) }, new Vector3[] { new Vector2(0.9537037f, 0.5018519f), new Vector2(0.1055556f, 0.5018519f), new Vector2(0.04814815f, 0.5666667f), new Vector2(0.2592593f, 0.5666667f), new Vector2(0.3222222f, 0.5037037f), new Vector2(0.2518519f, 0.4222222f), new Vector2(0.04259259f, 0.4222222f), new Vector2(0.1037037f, 0.5018519f) } }; } return icon_arrow; }
        Vector3[][] Icon_arrowBow() { if (icon_arrowBow == null) { icon_arrowBow = new Vector3[][] { new Vector3[] { new Vector2(0.8759259f, 0.4259259f), new Vector2(0.8759259f, 0.5740741f), new Vector2(0.9574074f, 0.5037037f), new Vector2(0.8759259f, 0.4259259f) }, new Vector3[] { new Vector2(0.95f, 0.5037037f), new Vector2(0.08148148f, 0.5037037f), new Vector2(0.03888889f, 0.5648148f), new Vector2(0.1944444f, 0.5648148f), new Vector2(0.2388889f, 0.5037037f), new Vector2(0.187037f, 0.4259259f), new Vector2(0.03148148f, 0.4259259f), new Vector2(0.08148148f, 0.5018519f) }, new Vector3[] { new Vector2(0.4611111f, 0.9703704f), new Vector2(0.5740741f, 0.9259259f), new Vector2(0.6703704f, 0.8555555f), new Vector2(0.7481481f, 0.7611111f), new Vector2(0.8018519f, 0.6537037f), new Vector2(0.8240741f, 0.5462963f), new Vector2(0.8203704f, 0.4240741f), new Vector2(0.7888889f, 0.3185185f), new Vector2(0.7351852f, 0.2240741f), new Vector2(0.6574074f, 0.137037f), new Vector2(0.5740741f, 0.07777778f), new Vector2(0.462963f, 0.04074074f) }, new Vector3[] { new Vector2(0.5222222f, 0.06111111f), new Vector2(0.337037f, 0.5055556f), new Vector2(0.5148148f, 0.9481481f) } }; } return icon_arrowBow; }
        Vector3[][] Icon_bomb() { if (icon_bomb == null) { icon_bomb = new Vector3[][] { new Vector3[] { new Vector2(0.3073632f, 0.06078675f), new Vector2(0.2073505f, 0.08078927f), new Vector2(0.1327956f, 0.1289772f), new Vector2(0.07915244f, 0.2044413f), new Vector2(0.05460387f, 0.309f), new Vector2(0.072788f, 0.4071943f), new Vector2(0.1355232f, 0.4981149f), new Vector2(0.2109873f, 0.5463028f), new Vector2(0.311f, 0.5635777f), new Vector2(0.4191955f, 0.5390291f), new Vector2(0.4946597f, 0.48084f), new Vector2(0.5428475f, 0.4026483f), new Vector2(0.5583041f, 0.309f), new Vector2(0.5355739f, 0.2053505f), new Vector2(0.4846584f, 0.1317048f), new Vector2(0.4055574f, 0.08169849f), new Vector2(0.3073632f, 0.06078675f) }, new Vector3[] { new Vector2(0.2944444f, 0.5629629f), new Vector2(0.3333333f, 0.6796296f), new Vector2(0.4907407f, 0.6277778f), new Vector2(0.4518518f, 0.5166667f) }, new Vector3[] { new Vector2(0.412963f, 0.6537037f), new Vector2(0.4407407f, 0.7370371f), new Vector2(0.4851852f, 0.7925926f), new Vector2(0.5518519f, 0.8129629f), new Vector2(0.6222222f, 0.7888889f), new Vector2(0.6666667f, 0.7407407f), new Vector2(0.7185185f, 0.7277778f), new Vector2(0.7722222f, 0.7407407f), new Vector2(0.8203704f, 0.7814815f) }, new Vector3[] { new Vector2(0.7629629f, 0.7759259f), new Vector2(0.7018518f, 0.8166667f), new Vector2(0.7777778f, 0.8148148f), new Vector2(0.7907407f, 0.9055555f), new Vector2(0.8351852f, 0.837037f), new Vector2(0.9296296f, 0.8685185f), new Vector2(0.8722222f, 0.7925926f), new Vector2(0.9296296f, 0.7333333f), new Vector2(0.8462963f, 0.7444444f), new Vector2(0.8314815f, 0.6537037f), new Vector2(0.8f, 0.7425926f) } }; } return icon_bomb; }
        Vector3[][] Icon_shovel() { if (icon_shovel == null) { icon_shovel = new Vector3[][] { new Vector3[] { new Vector2(0.2185185f, 0.4685185f), new Vector2(0.4722222f, 0.2074074f), new Vector2(0.3777778f, 0.1185185f), new Vector2(0.287037f, 0.06666667f), new Vector2(0.2092593f, 0.04259259f), new Vector2(0.1407407f, 0.05185185f), new Vector2(0.08518519f, 0.07222223f), new Vector2(0.06111111f, 0.1185185f), new Vector2(0.05f, 0.1907407f), new Vector2(0.06851852f, 0.2648148f), new Vector2(0.1166667f, 0.3611111f), new Vector2(0.2185185f, 0.4685185f) }, new Vector3[] { new Vector2(0.3203704f, 0.3685185f), new Vector2(0.7944444f, 0.8481482f), new Vector2(0.7259259f, 0.9185185f), new Vector2(0.7759259f, 0.9703704f), new Vector2(0.9740741f, 0.7759259f), new Vector2(0.9240741f, 0.7203704f), new Vector2(0.8537037f, 0.7907407f), new Vector2(0.3740741f, 0.3092593f) } }; } return icon_shovel; }
        Vector3[][] Icon_hammer() { if (icon_hammer == null) { icon_hammer = new Vector3[][] { new Vector3[] { new Vector2(0.6055555f, 0.9703704f), new Vector2(0.7685185f, 0.8407407f), new Vector2(0.85f, 0.7592593f), new Vector2(0.962963f, 0.6055555f), new Vector2(0.8425926f, 0.4814815f), new Vector2(0.6907408f, 0.5981482f), new Vector2(0.6037037f, 0.6814815f), new Vector2(0.4777778f, 0.8425926f), new Vector2(0.6055555f, 0.9703704f) }, new Vector3[] { new Vector2(0.6037037f, 0.6796296f), new Vector2(0.04629629f, 0.1092593f), new Vector2(0.1148148f, 0.02777778f), new Vector2(0.6851852f, 0.6f) }, new Vector3[] { new Vector2(0.7703704f, 0.8444445f), new Vector2(0.7981482f, 0.8740741f), new Vector2(0.8740741f, 0.7962963f), new Vector2(0.8481482f, 0.7629629f) } }; } return icon_hammer; }
        Vector3[][] Icon_axe() { if (icon_axe == null) { icon_axe = new Vector3[][] { new Vector3[] { new Vector2(0.5802568f, 0.6420546f), new Vector2(0.5176565f, 0.6741573f), new Vector2(0.4550562f, 0.6902087f), new Vector2(0.3988764f, 0.6886035f), new Vector2(0.3699839f, 0.6725522f), new Vector2(0.3715891f, 0.7399679f), new Vector2(0.3892456f, 0.8025682f), new Vector2(0.4197432f, 0.8571429f), new Vector2(0.4630819f, 0.9036918f), new Vector2(0.5160514f, 0.9325843f), new Vector2(0.5802568f, 0.9518459f), new Vector2(0.6492777f, 0.9518459f), new Vector2(0.6348315f, 0.8988764f), new Vector2(0.6380417f, 0.8378812f), new Vector2(0.6556982f, 0.7961476f), new Vector2(0.6845907f, 0.7512038f) }, new Vector3[] { new Vector2(0.6364366f, 0.5874799f), new Vector2(0.6669342f, 0.5361156f), new Vector2(0.6813804f, 0.4654896f), new Vector2(0.6813804f, 0.423756f), new Vector2(0.6701444f, 0.3756019f), new Vector2(0.7279294f, 0.3756019f), new Vector2(0.7905297f, 0.3900481f), new Vector2(0.85313f, 0.423756f), new Vector2(0.9028893f, 0.4751204f), new Vector2(0.9333869f, 0.5248796f), new Vector2(0.9446228f, 0.5858748f), new Vector2(0.9430177f, 0.6532905f), new Vector2(0.8980739f, 0.6404495f), new Vector2(0.8338684f, 0.6436597f), new Vector2(0.7841092f, 0.6613162f), new Vector2(0.7375602f, 0.6886035f) }, new Vector3[] { new Vector2(0.03451043f, 0.1107544f), new Vector2(0.1067416f, 0.03852328f), new Vector2(0.3025682f, 0.2343499f), new Vector2(0.2287319f, 0.3097913f), new Vector2(0.03611557f, 0.1139647f) }, new Vector3[] { new Vector2(0.2367576f, 0.3033708f), new Vector2(0.711878f, 0.7736757f), new Vector2(0.7680578f, 0.7158908f), new Vector2(0.2961477f, 0.2439807f) } }; } return icon_axe; }
        Vector3[][] Icon_magnet() { if (icon_magnet == null) { icon_magnet = new Vector3[][] { new Vector3[] { new Vector2(0.3555556f, 0.8203704f), new Vector2(0.4518518f, 0.7185185f), new Vector2(0.2240741f, 0.4962963f), new Vector2(0.1833333f, 0.4240741f), new Vector2(0.1722222f, 0.3222222f), new Vector2(0.2148148f, 0.2333333f), new Vector2(0.2981482f, 0.1888889f), new Vector2(0.3981481f, 0.1833333f), new Vector2(0.4796296f, 0.2351852f), new Vector2(0.7092593f, 0.4648148f), new Vector2(0.8074074f, 0.362963f), new Vector2(0.5777778f, 0.137037f), new Vector2(0.487037f, 0.07222223f), new Vector2(0.362963f, 0.04074074f), new Vector2(0.25f, 0.06111111f), new Vector2(0.1574074f, 0.1055556f), new Vector2(0.07037037f, 0.2111111f), new Vector2(0.03333334f, 0.3314815f), new Vector2(0.04074074f, 0.45f), new Vector2(0.08518519f, 0.5351852f), new Vector2(0.1333333f, 0.6f), new Vector2(0.3555556f, 0.8222222f) }, new Vector3[] { new Vector2(0.2685185f, 0.7314815f), new Vector2(0.3648148f, 0.6333333f) }, new Vector3[] { new Vector2(0.6240741f, 0.3777778f), new Vector2(0.7203704f, 0.2777778f) }, new Vector3[] { new Vector2(0.1314815f, 0.1407407f), new Vector2(0.2203704f, 0.2296296f) }, new Vector3[] { new Vector2(0.8666667f, 0.4092593f), new Vector2(0.9592593f, 0.4462963f) }, new Vector3[] { new Vector2(0.8129629f, 0.4703704f), new Vector2(0.887037f, 0.5462963f) }, new Vector3[] { new Vector2(0.7537037f, 0.5148148f), new Vector2(0.7888889f, 0.6166667f) }, new Vector3[] { new Vector2(0.5259259f, 0.9f), new Vector2(0.4444444f, 0.8222222f) }, new Vector3[] { new Vector2(0.3888889f, 0.8722222f), new Vector2(0.4296296f, 0.9722222f) }, new Vector3[] { new Vector2(0.5018519f, 0.7685185f), new Vector2(0.6018519f, 0.8055556f) } }; } return icon_magnet; }
        Vector3[][] Icon_compass() { if (icon_compass == null) { icon_compass = new Vector3[][] { new Vector3[] { new Vector2(0.4999586f, 0.07192704f), new Vector2(0.3613212f, 0.09965453f), new Vector2(0.2579732f, 0.1664526f), new Vector2(0.1836131f, 0.2710609f), new Vector2(0.1495839f, 0.416f), new Vector2(0.1747907f, 0.5521169f), new Vector2(0.2617542f, 0.6781509f), new Vector2(0.3663625f, 0.744949f), new Vector2(0.505f, 0.7688954f), new Vector2(0.6549805f, 0.7348663f), new Vector2(0.7595888f, 0.6542044f), new Vector2(0.8263869f, 0.5458152f), new Vector2(0.8478127f, 0.416f), new Vector2(0.8163041f, 0.2723212f), new Vector2(0.745725f, 0.1702336f), new Vector2(0.6360754f, 0.1009149f), new Vector2(0.4999586f, 0.07192704f) }, new Vector3[] { new Vector2(0.499521f, 0.03553063f), new Vector2(0.3460974f, 0.06621531f), new Vector2(0.2317271f, 0.1401376f), new Vector2(0.1494363f, 0.2559027f), new Vector2(0.1117778f, 0.4163f), new Vector2(0.139673f, 0.5669341f), new Vector2(0.2359114f, 0.7064101f), new Vector2(0.3516764f, 0.7803323f), new Vector2(0.5051f, 0.8068328f), new Vector2(0.6710764f, 0.7691742f), new Vector2(0.7868415f, 0.6799096f), new Vector2(0.8607638f, 0.5599603f), new Vector2(0.8844747f, 0.4163f), new Vector2(0.8496057f, 0.2572975f), new Vector2(0.7714991f, 0.1443219f), new Vector2(0.650155f, 0.06761009f), new Vector2(0.499521f, 0.03553063f) }, new Vector3[] { new Vector2(0.4994f, 0.3544254f), new Vector2(0.4518622f, 0.3741362f), new Vector2(0.4330789f, 0.4175f), new Vector2(0.4511665f, 0.4629508f), new Vector2(0.4982405f, 0.4831254f), new Vector2(0.5436913f, 0.4650378f), new Vector2(0.5620108f, 0.4172681f), new Vector2(0.5434595f, 0.3720492f), new Vector2(0.4994f, 0.3544254f) }, new Vector3[] { new Vector2(0.5021f, 0.8412322f), new Vector2(0.4540825f, 0.8610972f), new Vector2(0.4351098f, 0.9048f), new Vector2(0.4533798f, 0.9506062f), new Vector2(0.5009288f, 0.9709385f), new Vector2(0.5468382f, 0.9527096f), new Vector2(0.5653425f, 0.9045663f), new Vector2(0.546604f, 0.8589938f), new Vector2(0.5021f, 0.8412322f) }, new Vector3[] { new Vector2(0.2737643f, 0.1939164f), new Vector2(0.4258555f, 0.4923954f), new Vector2(0.7224334f, 0.6425856f), new Vector2(0.5665399f, 0.3441065f), new Vector2(0.2737643f, 0.1939164f) }, new Vector3[] { new Vector2(0.148289f, 0.4163498f), new Vector2(0.2072243f, 0.4163498f) }, new Vector3[] { new Vector2(0.8460076f, 0.4163498f), new Vector2(0.7832699f, 0.4163498f) }, new Vector3[] { new Vector2(0.5038023f, 0.769962f), new Vector2(0.5038023f, 0.7072243f) }, new Vector3[] { new Vector2(0.5f, 0.07224335f), new Vector2(0.5f, 0.134981f) }, new Vector3[] { new Vector2(0.5038023f, 0.8079848f), new Vector2(0.5038023f, 0.8460076f) } }; } return icon_compass; }
        Vector3[][] Icon_fuelStation() { if (icon_fuelStation == null) { icon_fuelStation = new Vector3[][] { new Vector3[] { new Vector2(0.6825095f, 0.6197718f), new Vector2(0.7167301f, 0.6159696f), new Vector2(0.7604563f, 0.5988593f), new Vector2(0.7832699f, 0.5570342f), new Vector2(0.7965779f, 0.4885932f), new Vector2(0.7965779f, 0.3897339f), new Vector2(0.7851711f, 0.2984791f), new Vector2(0.7851711f, 0.2205323f), new Vector2(0.7984791f, 0.1996198f), new Vector2(0.8288974f, 0.1920152f), new Vector2(0.8555133f, 0.2091255f), new Vector2(0.8802282f, 0.3117871f), new Vector2(0.8954372f, 0.4448669f), new Vector2(0.8954372f, 0.595057f), new Vector2(0.8764259f, 0.6958175f), new Vector2(0.9144487f, 0.7034221f), new Vector2(0.9372624f, 0.6178707f), new Vector2(0.9391635f, 0.5475285f), new Vector2(0.9372624f, 0.391635f), new Vector2(0.9144487f, 0.2490494f), new Vector2(0.9011407f, 0.1939164f), new Vector2(0.8821293f, 0.1634981f), new Vector2(0.8536122f, 0.1444867f), new Vector2(0.8098859f, 0.1444867f), new Vector2(0.7680609f, 0.1634981f), new Vector2(0.7395437f, 0.2148289f), new Vector2(0.7376426f, 0.2718631f), new Vector2(0.7509506f, 0.3707224f), new Vector2(0.7528517f, 0.4486692f), new Vector2(0.7471483f, 0.5152091f), new Vector2(0.7376426f, 0.5475285f), new Vector2(0.7110266f, 0.5665399f), new Vector2(0.6844106f, 0.5722433f) }, new Vector3[] { new Vector2(0.9144487f, 0.7034221f), new Vector2(0.9239544f, 0.7319391f), new Vector2(0.904943f, 0.8079848f), new Vector2(0.7870722f, 0.878327f), new Vector2(0.7756654f, 0.8555133f), new Vector2(0.8593156f, 0.8079848f), new Vector2(0.8193916f, 0.7794677f), new Vector2(0.8346007f, 0.7091255f), new Vector2(0.8764259f, 0.6939163f) }, new Vector3[] { new Vector2(0.1444867f, 0.121673f), new Vector2(0.1444867f, 0.9068441f), new Vector2(0.1596958f, 0.9353612f), new Vector2(0.1958175f, 0.9562737f), new Vector2(0.634981f, 0.9562737f), new Vector2(0.6634981f, 0.9391635f), new Vector2(0.6806084f, 0.9011407f), new Vector2(0.6806084f, 0.121673f) }, new Vector3[] { new Vector2(0.07794677f, 0.121673f), new Vector2(0.7528517f, 0.121673f), new Vector2(0.7528517f, 0.03231939f), new Vector2(0.07794677f, 0.03231939f), new Vector2(0.07794677f, 0.121673f) }, new Vector3[] { new Vector2(0.21673f, 0.8878327f), new Vector2(0.6102661f, 0.8878327f), new Vector2(0.6102661f, 0.5874525f), new Vector2(0.21673f, 0.5874525f), new Vector2(0.21673f, 0.8878327f) }, new Vector3[] { new Vector2(0.8745247f, 0.7851711f), new Vector2(0.8935362f, 0.7319391f), new Vector2(0.8593156f, 0.7281369f), new Vector2(0.8498099f, 0.7718631f), new Vector2(0.8745247f, 0.7851711f) } }; } return icon_fuelStation; }
        Vector3[][] Icon_fuelCan() { if (icon_fuelCan == null) { icon_fuelCan = new Vector3[][] { new Vector3[] { new Vector2(0.1178707f, 0.6634981f), new Vector2(0.1178707f, 0.1444867f), new Vector2(0.1254753f, 0.09695818f), new Vector2(0.1387833f, 0.07984791f), new Vector2(0.1596958f, 0.06273764f), new Vector2(0.2015209f, 0.04752852f), new Vector2(0.8117871f, 0.04752852f), new Vector2(0.8403042f, 0.0608365f), new Vector2(0.8593156f, 0.08174905f), new Vector2(0.8745247f, 0.1064639f), new Vector2(0.8802282f, 0.1444867f), new Vector2(0.8802282f, 0.851711f), new Vector2(0.8707224f, 0.8954372f), new Vector2(0.8479087f, 0.9277567f), new Vector2(0.8117871f, 0.9562737f), new Vector2(0.7604563f, 0.9657795f), new Vector2(0.4125475f, 0.9657795f), new Vector2(0.1178707f, 0.6634981f) }, new Vector3[] { new Vector2(0.4619772f, 0.7756654f), new Vector2(0.7585551f, 0.7756654f), new Vector2(0.7870722f, 0.7908745f), new Vector2(0.8041825f, 0.8326996f), new Vector2(0.7889734f, 0.8726236f), new Vector2(0.7547529f, 0.891635f), new Vector2(0.4657795f, 0.891635f), new Vector2(0.4372624f, 0.8707224f), new Vector2(0.4220532f, 0.8346007f), new Vector2(0.4353612f, 0.7965779f), new Vector2(0.4619772f, 0.7756654f) }, new Vector3[] { new Vector2(0.1178707f, 0.7642586f), new Vector2(0.1178707f, 0.8422053f), new Vector2(0.230038f, 0.9543726f), new Vector2(0.3098859f, 0.9543726f), new Vector2(0.1178707f, 0.7642586f) }, new Vector3[] { new Vector2(0.1577947f, 0.8041825f), new Vector2(0.2034221f, 0.756654f) }, new Vector3[] { new Vector2(0.2680608f, 0.9125475f), new Vector2(0.3117871f, 0.865019f) }, new Vector3[] { new Vector2(0.2661597f, 0.6444867f), new Vector2(0.7756654f, 0.1330798f) }, new Vector3[] { new Vector2(0.256654f, 0.1387833f), new Vector2(0.7813688f, 0.6634981f) } }; } return icon_fuelCan; }
        Vector3[][] Icon_lockLocked() { if (icon_lockLocked == null) { icon_lockLocked = new Vector3[][] { new Vector3[] { new Vector2(0.2163121f, 0.4982269f), new Vector2(0.2163121f, 0.06028369f), new Vector2(0.2411347f, 0.03723404f), new Vector2(0.7517731f, 0.03723404f), new Vector2(0.7783688f, 0.07092199f), new Vector2(0.7783688f, 0.5f), new Vector2(0.7517731f, 0.5301418f), new Vector2(0.2429078f, 0.5301418f), new Vector2(0.2163121f, 0.4982269f) }, new Vector3[] { new Vector2(0.5301418f, 0.1702128f), new Vector2(0.5301418f, 0.2641844f), new Vector2(0.5567376f, 0.2801418f), new Vector2(0.5797873f, 0.3120568f), new Vector2(0.5851064f, 0.3510638f), new Vector2(0.570922f, 0.391844f), new Vector2(0.5425532f, 0.4202128f), new Vector2(0.5f, 0.4343972f), new Vector2(0.4592199f, 0.4202128f), new Vector2(0.4237589f, 0.393617f), new Vector2(0.4113475f, 0.358156f), new Vector2(0.4131206f, 0.3280142f), new Vector2(0.4219858f, 0.3014185f), new Vector2(0.4414894f, 0.2801418f), new Vector2(0.464539f, 0.2659574f), new Vector2(0.464539f, 0.1702128f), new Vector2(0.4751773f, 0.1560284f), new Vector2(0.4964539f, 0.1453901f), new Vector2(0.5177305f, 0.1578014f), new Vector2(0.5301418f, 0.1702128f) }, new Vector3[] { new Vector2(0.7180851f, 0.5336879f), new Vector2(0.7180851f, 0.6365248f), new Vector2(0.6950355f, 0.7021276f), new Vector2(0.6489362f, 0.7588652f), new Vector2(0.572695f, 0.8102837f), new Vector2(0.5053192f, 0.8244681f), new Vector2(0.4255319f, 0.8067376f), new Vector2(0.3510638f, 0.7695035f), new Vector2(0.3102837f, 0.7109929f), new Vector2(0.287234f, 0.6471631f), new Vector2(0.2801418f, 0.6046099f), new Vector2(0.2801418f, 0.5283688f) }, new Vector3[] { new Vector2(0.6347518f, 0.5319149f), new Vector2(0.6347518f, 0.6187943f), new Vector2(0.6170213f, 0.6684397f), new Vector2(0.5744681f, 0.7163121f), new Vector2(0.5301418f, 0.7340425f), new Vector2(0.4751773f, 0.7340425f), new Vector2(0.4202128f, 0.7092199f), new Vector2(0.3847518f, 0.6737589f), new Vector2(0.3652482f, 0.6276596f), new Vector2(0.3599291f, 0.5851064f), new Vector2(0.3599291f, 0.5301418f) } }; } return icon_lockLocked; }
        Vector3[][] Icon_lockUnlocked() { if (icon_lockUnlocked == null) { icon_lockUnlocked = new Vector3[][] { new Vector3[] { new Vector2(0.2163121f, 0.4982269f), new Vector2(0.2163121f, 0.06028369f), new Vector2(0.2411347f, 0.03723404f), new Vector2(0.7517731f, 0.03723404f), new Vector2(0.7783688f, 0.07092199f), new Vector2(0.7783688f, 0.5f), new Vector2(0.7517731f, 0.5301418f), new Vector2(0.2429078f, 0.5301418f), new Vector2(0.2163121f, 0.4982269f) }, new Vector3[] { new Vector2(0.5301418f, 0.1702128f), new Vector2(0.5301418f, 0.2641844f), new Vector2(0.5567376f, 0.2801418f), new Vector2(0.5797873f, 0.3120568f), new Vector2(0.5851064f, 0.3510638f), new Vector2(0.570922f, 0.391844f), new Vector2(0.5425532f, 0.4202128f), new Vector2(0.5f, 0.4343972f), new Vector2(0.4592199f, 0.4202128f), new Vector2(0.4237589f, 0.393617f), new Vector2(0.4113475f, 0.358156f), new Vector2(0.4131206f, 0.3280142f), new Vector2(0.4219858f, 0.3014185f), new Vector2(0.4414894f, 0.2801418f), new Vector2(0.464539f, 0.2659574f), new Vector2(0.464539f, 0.1702128f), new Vector2(0.4751773f, 0.1560284f), new Vector2(0.4964539f, 0.1453901f), new Vector2(0.5177305f, 0.1578014f), new Vector2(0.5301418f, 0.1702128f) }, new Vector3[] { new Vector2(0.7180851f, 0.5336879f), new Vector2(0.7180851f, 0.7819149f), new Vector2(0.6950355f, 0.8457447f), new Vector2(0.6542553f, 0.9078014f), new Vector2(0.5833333f, 0.9521276f), new Vector2(0.5f, 0.9716312f), new Vector2(0.4237589f, 0.9592199f), new Vector2(0.356383f, 0.9202127f), new Vector2(0.3085106f, 0.8687943f), new Vector2(0.285461f, 0.8085107f), new Vector2(0.2765957f, 0.7446808f), new Vector2(0.2765957f, 0.714539f), new Vector2(0.2907801f, 0.7021276f), new Vector2(0.3173759f, 0.6897163f), new Vector2(0.3439716f, 0.7056738f), new Vector2(0.3617021f, 0.7287234f), new Vector2(0.3617021f, 0.7641844f), new Vector2(0.3723404f, 0.8138298f), new Vector2(0.4113475f, 0.8546099f), new Vector2(0.462766f, 0.8812057f), new Vector2(0.5141844f, 0.8812057f), new Vector2(0.570922f, 0.8670213f), new Vector2(0.6028369f, 0.8368794f), new Vector2(0.6258865f, 0.7960993f), new Vector2(0.6329787f, 0.7482269f), new Vector2(0.6329787f, 0.5319149f) } }; } return icon_lockUnlocked; }
        Vector3[][] Icon_key() { if (icon_key == null) { icon_key = new Vector3[][] { new Vector3[] { new Vector2(0.4222038f, 0.4164213f), new Vector2(0.3742242f, 0.3486996f), new Vector2(0.3137213f, 0.3092331f), new Vector2(0.2398659f, 0.2943415f), new Vector2(0.1540156f, 0.3107094f), new Vector2(0.08704364f, 0.3567354f), new Vector2(0.04022795f, 0.4334701f), new Vector2(0.02939141f, 0.5055193f), new Vector2(0.04966111f, 0.5855823f), new Vector2(0.1037292f, 0.6578816f), new Vector2(0.1719675f, 0.6947129f), new Vector2(0.2460442f, 0.7046463f), new Vector2(0.3207743f, 0.685132f), new Vector2(0.3902963f, 0.6339207f), new Vector2(0.4237589f, 0.5780142f), new Vector2(0.4893617f, 0.6471631f), new Vector2(0.4893617f, 0.569149f), new Vector2(0.9219858f, 0.569149f), new Vector2(0.9663121f, 0.5053192f), new Vector2(0.8758865f, 0.4042553f), new Vector2(0.822695f, 0.462766f), new Vector2(0.7712766f, 0.4095745f), new Vector2(0.7163121f, 0.4680851f), new Vector2(0.6613475f, 0.4095745f), new Vector2(0.6010638f, 0.4698582f), new Vector2(0.5425532f, 0.4113475f), new Vector2(0.4911348f, 0.4663121f), new Vector2(0.4911348f, 0.358156f), new Vector2(0.4222038f, 0.4164213f) }, new Vector3[] { new Vector2(0.8971631f, 0.5141844f), new Vector2(0.4326241f, 0.5141844f) }, new Vector3[] { new Vector2(0.128f, 0.4449929f), new Vector2(0.08827589f, 0.4614639f), new Vector2(0.07258002f, 0.4977f), new Vector2(0.08769456f, 0.5356802f), new Vector2(0.1270311f, 0.5525387f), new Vector2(0.1650113f, 0.5374241f), new Vector2(0.1803196f, 0.4975062f), new Vector2(0.1648175f, 0.4597199f), new Vector2(0.128f, 0.4449929f) } }; } return icon_key; }
        Vector3[][] Icon_gemDiamond() { if (icon_gemDiamond == null) { icon_gemDiamond = new Vector3[][] { new Vector3[] { new Vector2(0.07222223f, 0.6092592f), new Vector2(0.9185185f, 0.6092592f), new Vector2(0.7759259f, 0.8555555f), new Vector2(0.2222222f, 0.8555555f), new Vector2(0.07592592f, 0.6074074f), new Vector2(0.5f, 0.08703703f), new Vector2(0.9185185f, 0.6111111f) }, new Vector3[] { new Vector2(0.4148148f, 0.8555555f), new Vector2(0.3f, 0.6092592f), new Vector2(0.5f, 0.08703703f), new Vector2(0.6981481f, 0.6111111f), new Vector2(0.587037f, 0.8537037f) } }; } return icon_gemDiamond; }
        Vector3[][] Icon_gold() { if (icon_gold == null) { icon_gold = new Vector3[][] { new Vector3[] { new Vector2(0.2407407f, 0.2740741f), new Vector2(0.4722222f, 0.212963f), new Vector2(0.7481481f, 0.4962963f), new Vector2(0.6962963f, 0.6185185f), new Vector2(0.5537037f, 0.6574074f), new Vector2(0.2833333f, 0.3777778f), new Vector2(0.2407407f, 0.2740741f) }, new Vector3[] { new Vector2(0.2796296f, 0.3777778f), new Vector2(0.4277778f, 0.3351852f), new Vector2(0.6944444f, 0.6185185f) }, new Vector3[] { new Vector2(0.4407407f, 0.5425926f), new Vector2(0.4203704f, 0.5537037f), new Vector2(0.1388889f, 0.2703704f), new Vector2(0.287037f, 0.2351852f), new Vector2(0.3277778f, 0.1074074f), new Vector2(0.09444445f, 0.1648148f), new Vector2(0.1407407f, 0.2740741f) }, new Vector3[] { new Vector2(0.7240741f, 0.4722222f), new Vector2(0.8351852f, 0.4407407f), new Vector2(0.8814815f, 0.3166667f), new Vector2(0.6111111f, 0.03888889f), new Vector2(0.3740741f, 0.09814814f), new Vector2(0.4222222f, 0.2018518f), new Vector2(0.5703704f, 0.162963f), new Vector2(0.8333333f, 0.4407407f) }, new Vector3[] { new Vector2(0.6111111f, 0.04259259f), new Vector2(0.5685185f, 0.162963f) }, new Vector3[] { new Vector2(0.3240741f, 0.1092593f), new Vector2(0.4407407f, 0.2203704f) }, new Vector3[] { new Vector2(0.4277778f, 0.3351852f), new Vector2(0.4722222f, 0.2148148f) }, new Vector3[] { new Vector2(0.8185185f, 0.612963f), new Vector2(0.9537037f, 0.7203704f) }, new Vector3[] { new Vector2(0.6907408f, 0.7166666f), new Vector2(0.7722222f, 0.8777778f) }, new Vector3[] { new Vector2(0.5037037f, 0.7592593f), new Vector2(0.5037037f, 0.9370371f) }, new Vector3[] { new Vector2(0.3481481f, 0.7166666f), new Vector2(0.2648148f, 0.8814815f) }, new Vector3[] { new Vector2(0.2111111f, 0.6018519f), new Vector2(0.06296296f, 0.7074074f) }, new Vector3[] { new Vector2(0.2851852f, 0.237037f), new Vector2(0.3092593f, 0.2574074f) } }; } return icon_gold; }
        Vector3[][] Icon_potion() { if (icon_potion == null) { icon_potion = new Vector3[][] { new Vector3[] { new Vector2(0.4185185f, 0.8092592f), new Vector2(0.4351852f, 0.7666667f), new Vector2(0.4296296f, 0.7074074f), new Vector2(0.412963f, 0.6481481f), new Vector2(0.3666667f, 0.5907407f), new Vector2(0.3111111f, 0.5518519f), new Vector2(0.2574074f, 0.4962963f), new Vector2(0.2111111f, 0.4185185f), new Vector2(0.1944444f, 0.3092593f), new Vector2(0.2111111f, 0.2148148f), new Vector2(0.2537037f, 0.1351852f), new Vector2(0.3129629f, 0.07592592f), new Vector2(0.3648148f, 0.04259259f), new Vector2(0.6222222f, 0.04259259f), new Vector2(0.6944444f, 0.08888889f), new Vector2(0.7555556f, 0.1592593f), new Vector2(0.7925926f, 0.2425926f), new Vector2(0.7944444f, 0.3425926f), new Vector2(0.7703704f, 0.4481482f), new Vector2(0.7185185f, 0.5203704f), new Vector2(0.6518518f, 0.5740741f), new Vector2(0.5925926f, 0.6296296f), new Vector2(0.5611111f, 0.6981481f), new Vector2(0.5592592f, 0.7611111f), new Vector2(0.5777778f, 0.8111111f) }, new Vector3[] { new Vector2(0.237037f, 0.3018518f), new Vector2(0.3055556f, 0.35f), new Vector2(0.3833333f, 0.3703704f), new Vector2(0.4611111f, 0.3666667f), new Vector2(0.5351852f, 0.3333333f), new Vector2(0.5925926f, 0.3f), new Vector2(0.6537037f, 0.2777778f), new Vector2(0.7074074f, 0.2888889f), new Vector2(0.7592593f, 0.3222222f), new Vector2(0.7518519f, 0.2481481f), new Vector2(0.7259259f, 0.1851852f), new Vector2(0.6870371f, 0.1314815f), new Vector2(0.65f, 0.1f), new Vector2(0.6055555f, 0.07592592f), new Vector2(0.3814815f, 0.07592592f), new Vector2(0.3314815f, 0.1092593f), new Vector2(0.2796296f, 0.162963f), new Vector2(0.2444444f, 0.2296296f), new Vector2(0.237037f, 0.3018518f) }, new Vector3[] { new Vector2(0.2425926f, 0.25f), new Vector2(0.2925926f, 0.2888889f), new Vector2(0.3666667f, 0.3148148f), new Vector2(0.4425926f, 0.3203704f), new Vector2(0.5111111f, 0.2944444f), new Vector2(0.5537037f, 0.2611111f), new Vector2(0.6055555f, 0.2333333f), new Vector2(0.6592593f, 0.2277778f), new Vector2(0.7074074f, 0.237037f), new Vector2(0.75f, 0.2666667f) }, new Vector3[] { new Vector2(0.6555555f, 0.5111111f), new Vector2(0.6925926f, 0.4648148f), new Vector2(0.7f, 0.4240741f), new Vector2(0.6888889f, 0.3907408f), new Vector2(0.6574074f, 0.3703704f), new Vector2(0.6111111f, 0.3888889f), new Vector2(0.5666667f, 0.4333333f), new Vector2(0.5518519f, 0.4777778f), new Vector2(0.5648148f, 0.5203704f), new Vector2(0.5888889f, 0.537037f), new Vector2(0.6277778f, 0.5259259f), new Vector2(0.6555555f, 0.5111111f) }, new Vector3[] { new Vector2(0.387037f, 0.8111111f), new Vector2(0.6037037f, 0.8111111f), new Vector2(0.6037037f, 0.8796296f), new Vector2(0.387037f, 0.8796296f), new Vector2(0.387037f, 0.8111111f) }, new Vector3[] { new Vector2(0.4388889f, 0.8796296f), new Vector2(0.4388889f, 0.9481481f), new Vector2(0.5611111f, 0.9481481f), new Vector2(0.5611111f, 0.8796296f) } }; } return icon_potion; }
        Vector3[][] Icon_presentGift() { if (icon_presentGift == null) { icon_presentGift = new Vector3[][] { new Vector3[] { new Vector2(0.5f, 0.05f), new Vector2(0.5f, 0.7555556f), new Vector2(0.4574074f, 0.8351852f), new Vector2(0.3851852f, 0.9222222f), new Vector2(0.3222222f, 0.9611111f), new Vector2(0.2777778f, 0.9703704f), new Vector2(0.2333333f, 0.9518518f), new Vector2(0.2166667f, 0.9037037f), new Vector2(0.25f, 0.8444445f), new Vector2(0.3222222f, 0.7962963f), new Vector2(0.4203704f, 0.7648148f), new Vector2(0.5018519f, 0.7518519f), new Vector2(0.6111111f, 0.7722222f), new Vector2(0.6981481f, 0.8055556f), new Vector2(0.7592593f, 0.862963f), new Vector2(0.7796296f, 0.9055555f), new Vector2(0.7685185f, 0.95f), new Vector2(0.7333333f, 0.9666666f), new Vector2(0.6722222f, 0.9611111f), new Vector2(0.6f, 0.9074074f), new Vector2(0.5407407f, 0.8388889f), new Vector2(0.5f, 0.7555556f) }, new Vector3[] { new Vector2(0.04444445f, 0.6074074f), new Vector2(0.04444445f, 0.7592593f), new Vector2(0.9481481f, 0.7592593f), new Vector2(0.9481481f, 0.6074074f), new Vector2(0.04444445f, 0.6074074f) }, new Vector3[] { new Vector2(0.0962963f, 0.6055555f), new Vector2(0.0962963f, 0.04629629f), new Vector2(0.9037037f, 0.04629629f), new Vector2(0.9037037f, 0.6074074f) } }; } return icon_presentGift; }
        Vector3[][] Icon_death() { if (icon_death == null) { icon_death = new Vector3[][] { new Vector3[] { new Vector2(0.1985816f, 0.2198582f), new Vector2(0.7606383f, 0.4840426f), new Vector2(0.7606383f, 0.535461f), new Vector2(0.7836879f, 0.5762411f), new Vector2(0.8067376f, 0.5868794f), new Vector2(0.8404256f, 0.5868794f), new Vector2(0.858156f, 0.570922f), new Vector2(0.8758865f, 0.5496454f), new Vector2(0.8723404f, 0.5212766f), new Vector2(0.8617021f, 0.4946809f), new Vector2(0.9060284f, 0.4787234f), new Vector2(0.9219858f, 0.4539007f), new Vector2(0.9219858f, 0.4202128f), new Vector2(0.9007092f, 0.3882979f), new Vector2(0.858156f, 0.3794326f), new Vector2(0.8031915f, 0.4113475f), new Vector2(0.2340426f, 0.1507092f), new Vector2(0.2358156f, 0.1187943f), new Vector2(0.2287234f, 0.08687943f), new Vector2(0.2074468f, 0.05673759f), new Vector2(0.1666667f, 0.04609929f), new Vector2(0.1329787f, 0.06914894f), new Vector2(0.1223404f, 0.09751773f), new Vector2(0.1365248f, 0.141844f), new Vector2(0.09397163f, 0.1542553f), new Vector2(0.06914894f, 0.1879433f), new Vector2(0.07624114f, 0.2287234f), new Vector2(0.1046099f, 0.248227f), new Vector2(0.143617f, 0.25f), new Vector2(0.1985816f, 0.2198582f) }, new Vector3[] { new Vector2(0.5957447f, 0.3138298f), new Vector2(0.8031915f, 0.2163121f), new Vector2(0.8333333f, 0.2375887f), new Vector2(0.8776596f, 0.25f), new Vector2(0.9148936f, 0.2358156f), new Vector2(0.927305f, 0.2039007f), new Vector2(0.9202127f, 0.1702128f), new Vector2(0.8989362f, 0.1471631f), new Vector2(0.858156f, 0.1453901f), new Vector2(0.8687943f, 0.1099291f), new Vector2(0.8634752f, 0.07624114f), new Vector2(0.8368794f, 0.04964539f), new Vector2(0.7978724f, 0.05141844f), new Vector2(0.7677305f, 0.08865248f), new Vector2(0.7588652f, 0.1507092f), new Vector2(0.4982269f, 0.2695035f) }, new Vector3[] { new Vector2(0.4929078f, 0.3599291f), new Vector2(0.2358156f, 0.4840426f), new Vector2(0.2304965f, 0.5301418f), new Vector2(0.2109929f, 0.572695f), new Vector2(0.1719858f, 0.5815603f), new Vector2(0.1329787f, 0.5673759f), new Vector2(0.1223404f, 0.5390071f), new Vector2(0.1382979f, 0.4858156f), new Vector2(0.1028369f, 0.4751773f), new Vector2(0.07978723f, 0.4485815f), new Vector2(0.07978723f, 0.4113475f), new Vector2(0.1099291f, 0.3829787f), new Vector2(0.1507092f, 0.3882979f), new Vector2(0.1932624f, 0.4184397f), new Vector2(0.4024823f, 0.3173759f) }, new Vector3[] { new Vector2(0.4024823f, 0.5656028f), new Vector2(0.391844f, 0.4804965f), new Vector2(0.6187943f, 0.4804965f), new Vector2(0.6046099f, 0.569149f), new Vector2(0.6666667f, 0.606383f), new Vector2(0.7021276f, 0.679078f), new Vector2(0.714539f, 0.7641844f), new Vector2(0.7021276f, 0.8528369f), new Vector2(0.6489362f, 0.9202127f), new Vector2(0.5762411f, 0.9485816f), new Vector2(0.4219858f, 0.9485816f), new Vector2(0.356383f, 0.9113475f), new Vector2(0.3138298f, 0.858156f), new Vector2(0.2925532f, 0.8067376f), new Vector2(0.285461f, 0.7322695f), new Vector2(0.3031915f, 0.6666667f), new Vector2(0.3368794f, 0.6187943f), new Vector2(0.4024823f, 0.5656028f) }, new Vector3[] { new Vector2(0.4132f, 0.6499658f), new Vector2(0.3675014f, 0.668914f), new Vector2(0.3494449f, 0.7106f), new Vector2(0.3668326f, 0.7542924f), new Vector2(0.4120854f, 0.7736864f), new Vector2(0.4557777f, 0.7562986f), new Vector2(0.4733884f, 0.7103771f), new Vector2(0.4555548f, 0.6669077f), new Vector2(0.4132f, 0.6499658f) }, new Vector3[] { new Vector2(0.5919f, 0.6495658f), new Vector2(0.5460985f, 0.668514f), new Vector2(0.5280012f, 0.7102f), new Vector2(0.5454282f, 0.7538924f), new Vector2(0.5907829f, 0.7732864f), new Vector2(0.6345736f, 0.7558986f), new Vector2(0.652224f, 0.7099771f), new Vector2(0.6343502f, 0.6665077f), new Vector2(0.5919f, 0.6495658f) }, new Vector3[] { new Vector2(0.4663121f, 0.5673759f), new Vector2(0.4769503f, 0.6152482f), new Vector2(0.5124114f, 0.6453901f), new Vector2(0.5443262f, 0.6099291f), new Vector2(0.5514184f, 0.5638298f), new Vector2(0.5088652f, 0.5868794f), new Vector2(0.4663121f, 0.5673759f) } }; } return icon_death; }
        Vector3[][] Icon_map() { if (icon_map == null) { icon_map = new Vector3[][] { new Vector3[] { new Vector2(0.3574074f, 0.7777778f), new Vector2(0.3574074f, 0.1185185f) }, new Vector3[] { new Vector2(0.65f, 0.8611111f), new Vector2(0.65f, 0.2f) }, new Vector3[] { new Vector2(0.9314815f, 0.7777778f), new Vector2(0.9314815f, 0.1222222f), new Vector2(0.65f, 0.1981481f), new Vector2(0.3592592f, 0.1185185f), new Vector2(0.06481481f, 0.1962963f), new Vector2(0.06481481f, 0.8537037f), new Vector2(0.3555556f, 0.7740741f), new Vector2(0.65f, 0.8592592f), new Vector2(0.9314815f, 0.7777778f) }, new Vector3[] { new Vector2(0.4481482f, 0.662963f), new Vector2(0.5796296f, 0.5222222f) }, new Vector3[] { new Vector2(0.4703704f, 0.4925926f), new Vector2(0.5740741f, 0.6888889f) }, new Vector3[] { new Vector2(0.5185185f, 0.4574074f), new Vector2(0.5166667f, 0.3981481f) }, new Vector3[] { new Vector2(0.4944444f, 0.3537037f), new Vector2(0.4925926f, 0.2907407f) }, new Vector3[] { new Vector2(0.537037f, 0.2611111f), new Vector2(0.5888889f, 0.2481481f) }, new Vector3[] { new Vector2(0.6351852f, 0.262963f), new Vector2(0.6518518f, 0.2777778f), new Vector2(0.6870371f, 0.2759259f) }, new Vector3[] { new Vector2(0.7407407f, 0.287037f), new Vector2(0.7907407f, 0.3074074f) }, new Vector3[] { new Vector2(0.8277778f, 0.3425926f), new Vector2(0.8555555f, 0.3814815f) } }; } return icon_map; }
        Vector3[][] Icon_mushroom() { if (icon_mushroom == null) { icon_mushroom = new Vector3[][] { new Vector3[] { new Vector2(0.6781701f, 0.3980738f), new Vector2(0.3009631f, 0.3980738f), new Vector2(0.1886035f, 0.4060995f), new Vector2(0.08747993f, 0.4333868f), new Vector2(0.04574639f, 0.4831461f), new Vector2(0.04574639f, 0.5489566f), new Vector2(0.07463884f, 0.6677368f), new Vector2(0.1372392f, 0.7736757f), new Vector2(0.2351525f, 0.8555377f), new Vector2(0.3443018f, 0.9133226f), new Vector2(0.4422151f, 0.9373997f), new Vector2(0.5385233f, 0.9373997f), new Vector2(0.6332263f, 0.9165329f), new Vector2(0.7182986f, 0.8764045f), new Vector2(0.7953451f, 0.8218299f), new Vector2(0.8739968f, 0.7447833f), new Vector2(0.9141252f, 0.6693419f), new Vector2(0.9430177f, 0.5698234f), new Vector2(0.9414125f, 0.4815409f), new Vector2(0.9093098f, 0.4382023f), new Vector2(0.8467095f, 0.4125201f), new Vector2(0.6781701f, 0.3980738f) }, new Vector3[] { new Vector2(0.3218299f, 0.3980738f), new Vector2(0.3378812f, 0.3483146f), new Vector2(0.3394864f, 0.2728732f), new Vector2(0.3202247f, 0.1829856f), new Vector2(0.3025682f, 0.1187801f), new Vector2(0.3057785f, 0.07865169f), new Vector2(0.3218299f, 0.05136437f), new Vector2(0.3619583f, 0.03691814f), new Vector2(0.6396469f, 0.03691814f), new Vector2(0.6701444f, 0.05617978f), new Vector2(0.6878009f, 0.08507223f), new Vector2(0.6845907f, 0.1348315f), new Vector2(0.6573034f, 0.2327448f), new Vector2(0.6540931f, 0.3354735f), new Vector2(0.6749599f, 0.3964687f) } }; } return icon_mushroom; }
        Vector3[][] Icon_star() { if (icon_star == null) { icon_star = new Vector3[][] { new Vector3[] { new Vector2(0.04814815f, 0.6037037f), new Vector2(0.3944444f, 0.6037037f), new Vector2(0.5018519f, 0.9240741f), new Vector2(0.6018519f, 0.6018519f), new Vector2(0.9481481f, 0.6018519f), new Vector2(0.662963f, 0.4092593f), new Vector2(0.7759259f, 0.05925926f), new Vector2(0.4962963f, 0.2851852f), new Vector2(0.2111111f, 0.08148148f), new Vector2(0.3222222f, 0.4018519f), new Vector2(0.04814815f, 0.6037037f) } }; } return icon_star; }
        Vector3[][] Icon_pill() { if (icon_pill == null) { icon_pill = new Vector3[][] { new Vector3[] { new Vector2(0.1444445f, 0.4296296f), new Vector2(0.5851852f, 0.8685185f), new Vector2(0.6481481f, 0.9092593f), new Vector2(0.7240741f, 0.9296296f), new Vector2(0.7814815f, 0.9203704f), new Vector2(0.8407407f, 0.8925926f), new Vector2(0.8962963f, 0.8481482f), new Vector2(0.9203704f, 0.7962963f), new Vector2(0.9388889f, 0.7222222f), new Vector2(0.9296296f, 0.6537037f), new Vector2(0.9055555f, 0.6037037f), new Vector2(0.8685185f, 0.5592592f), new Vector2(0.4462963f, 0.1351852f), new Vector2(0.3851852f, 0.09259259f), new Vector2(0.3203704f, 0.07037037f), new Vector2(0.25f, 0.07407407f), new Vector2(0.1907407f, 0.09444445f), new Vector2(0.1314815f, 0.1444445f), new Vector2(0.08703703f, 0.2166667f), new Vector2(0.08148148f, 0.2962963f), new Vector2(0.1018519f, 0.3740741f), new Vector2(0.1444445f, 0.4296296f) }, new Vector3[] { new Vector2(0.3444445f, 0.6333333f), new Vector2(0.3740741f, 0.6407408f), new Vector2(0.4111111f, 0.6277778f), new Vector2(0.4574074f, 0.6018519f), new Vector2(0.5092593f, 0.5629629f), new Vector2(0.5777778f, 0.5f), new Vector2(0.6277778f, 0.4370371f), new Vector2(0.65f, 0.3944444f), new Vector2(0.6574074f, 0.3685185f), new Vector2(0.6518518f, 0.3444445f) }, new Vector3[] { new Vector2(0.3074074f, 0.5314815f), new Vector2(0.3574074f, 0.487037f), new Vector2(0.2037037f, 0.3462963f), new Vector2(0.1740741f, 0.2962963f), new Vector2(0.162963f, 0.2537037f), new Vector2(0.1592593f, 0.2111111f), new Vector2(0.1648148f, 0.1777778f), new Vector2(0.1814815f, 0.1462963f), new Vector2(0.1351852f, 0.2111111f), new Vector2(0.1222222f, 0.2592593f), new Vector2(0.1277778f, 0.3185185f), new Vector2(0.1407407f, 0.3592592f), new Vector2(0.1666667f, 0.4f), new Vector2(0.3074074f, 0.5314815f) } }; } return icon_pill; }
        Vector3[][] Icon_health() { if (icon_health == null) { icon_health = new Vector3[][] { new Vector3[] { new Vector2(0.08518519f, 0.6351852f), new Vector2(0.3574074f, 0.6351852f), new Vector2(0.3574074f, 0.9111111f), new Vector2(0.6277778f, 0.9111111f), new Vector2(0.6277778f, 0.6351852f), new Vector2(0.9074074f, 0.6351852f), new Vector2(0.9074074f, 0.3592592f), new Vector2(0.6277778f, 0.3592592f), new Vector2(0.6277778f, 0.08703703f), new Vector2(0.3574074f, 0.08703703f), new Vector2(0.3574074f, 0.3592592f), new Vector2(0.08518519f, 0.3592592f), new Vector2(0.08518519f, 0.6351852f) } }; } return icon_health; }
        Vector3[][] Icon_foodPlate() { if (icon_foodPlate == null) { icon_foodPlate = new Vector3[][] { new Vector3[] { new Vector2(0.5105195f, 0.2482073f), new Vector2(0.4093063f, 0.2684499f), new Vector2(0.3338565f, 0.3172163f), new Vector2(0.2795694f, 0.3935862f), new Vector2(0.2547262f, 0.4994f), new Vector2(0.2731286f, 0.598773f), new Vector2(0.3366168f, 0.6907849f), new Vector2(0.4129868f, 0.7395513f), new Vector2(0.5142f, 0.7570336f), new Vector2(0.6236942f, 0.7321904f), new Vector2(0.7000642f, 0.6733027f), new Vector2(0.7488306f, 0.5941724f), new Vector2(0.7644726f, 0.4994f), new Vector2(0.7414696f, 0.3945064f), new Vector2(0.6899428f, 0.3199766f), new Vector2(0.6098924f, 0.26937f), new Vector2(0.5105195f, 0.2482073f) }, new Vector3[] { new Vector2(0.5093555f, 0.19074f), new Vector2(0.3843826f, 0.2157346f), new Vector2(0.291221f, 0.2759488f), new Vector2(0.2241901f, 0.3702465f), new Vector2(0.1935149f, 0.5009f), new Vector2(0.2162373f, 0.6236007f), new Vector2(0.2946293f, 0.7372124f), new Vector2(0.3889271f, 0.7974266f), new Vector2(0.5139f, 0.8190128f), new Vector2(0.6490979f, 0.7883376f), new Vector2(0.7433957f, 0.7156261f), new Vector2(0.8036098f, 0.6179201f), new Vector2(0.8229239f, 0.5009f), new Vector2(0.7945209f, 0.3713827f), new Vector2(0.7308984f, 0.2793571f), new Vector2(0.6320561f, 0.2168707f), new Vector2(0.5093555f, 0.19074f) }, new Vector3[] { new Vector2(0.9011407f, 0.4980989f), new Vector2(0.891635f, 0.1882129f), new Vector2(0.9372624f, 0.1882129f), new Vector2(0.9372624f, 0.8022814f), new Vector2(0.9277567f, 0.8212928f), new Vector2(0.9087452f, 0.8212928f), new Vector2(0.8878327f, 0.7946768f), new Vector2(0.8764259f, 0.743346f), new Vector2(0.8593156f, 0.539924f), new Vector2(0.8669202f, 0.5190114f), new Vector2(0.8821293f, 0.5019011f), new Vector2(0.9030418f, 0.4923954f), new Vector2(0.9353612f, 0.4923954f) }, new Vector3[] { new Vector2(0.04942966f, 0.8136882f), new Vector2(0.04942966f, 0.6539924f), new Vector2(0.08555133f, 0.6292776f), new Vector2(0.08555133f, 0.1901141f), new Vector2(0.121673f, 0.1901141f), new Vector2(0.121673f, 0.6311787f), new Vector2(0.1634981f, 0.6653993f), new Vector2(0.1634981f, 0.8155894f) }, new Vector3[] { new Vector2(0.04752852f, 0.6920152f), new Vector2(0.161597f, 0.6920152f) }, new Vector3[] { new Vector2(0.08365019f, 0.6939163f), new Vector2(0.08365019f, 0.8136882f) }, new Vector3[] { new Vector2(0.1235741f, 0.6920152f), new Vector2(0.1235741f, 0.8136882f) } }; } return icon_foodPlate; }
        Vector3[][] Icon_foodMeat() { if (icon_foodMeat == null) { icon_foodMeat = new Vector3[][] { new Vector3[] { new Vector2(0.5209125f, 0.9315589f), new Vector2(0.4942966f, 0.8897339f), new Vector2(0.4942966f, 0.8231939f), new Vector2(0.526616f, 0.7376426f), new Vector2(0.608365f, 0.6330798f), new Vector2(0.730038f, 0.5361217f), new Vector2(0.8022814f, 0.4980989f), new Vector2(0.8688213f, 0.4885932f), new Vector2(0.8954372f, 0.4961977f), new Vector2(0.9239544f, 0.5114068f), new Vector2(0.9448669f, 0.5513308f), new Vector2(0.9429658f, 0.6102661f), new Vector2(0.9125475f, 0.6920152f), new Vector2(0.8479087f, 0.7813688f), new Vector2(0.7642586f, 0.8612167f), new Vector2(0.6787072f, 0.9163498f), new Vector2(0.6026616f, 0.9429658f), new Vector2(0.5513308f, 0.9448669f), new Vector2(0.5209125f, 0.9315589f) }, new Vector3[] { new Vector2(0.6197718f, 0.8117871f), new Vector2(0.6520913f, 0.8231939f), new Vector2(0.7053232f, 0.8098859f), new Vector2(0.7661597f, 0.7623574f), new Vector2(0.8212928f, 0.6787072f), new Vector2(0.8231939f, 0.6577947f), new Vector2(0.8155894f, 0.6235741f), new Vector2(0.7870722f, 0.608365f), new Vector2(0.7547529f, 0.6178707f), new Vector2(0.6920152f, 0.648289f), new Vector2(0.6444867f, 0.6996198f), new Vector2(0.6159696f, 0.7604563f), new Vector2(0.6121673f, 0.7927757f), new Vector2(0.6197718f, 0.8117871f) }, new Vector3[] { new Vector2(0.5209125f, 0.9296578f), new Vector2(0.4467681f, 0.8403042f), new Vector2(0.3707224f, 0.7167301f), new Vector2(0.3288973f, 0.6026616f), new Vector2(0.3174905f, 0.5f), new Vector2(0.3288973f, 0.4353612f), new Vector2(0.3536122f, 0.3802281f), new Vector2(0.4011407f, 0.3403042f), new Vector2(0.4752852f, 0.3174905f), new Vector2(0.5684411f, 0.3136882f), new Vector2(0.6882129f, 0.3498099f), new Vector2(0.8117871f, 0.4182509f), new Vector2(0.9239544f, 0.5095057f) }, new Vector3[] { new Vector2(0.338403f, 0.4106464f), new Vector2(0.1692015f, 0.2357415f), new Vector2(0.1254753f, 0.2547528f), new Vector2(0.07794677f, 0.243346f), new Vector2(0.05323194f, 0.2034221f), new Vector2(0.0608365f, 0.1558935f), new Vector2(0.09885932f, 0.1292776f), new Vector2(0.1463878f, 0.134981f), new Vector2(0.1425855f, 0.08174905f), new Vector2(0.1692015f, 0.05323194f), new Vector2(0.2129278f, 0.03612167f), new Vector2(0.2604563f, 0.06653992f), new Vector2(0.2680608f, 0.1159696f), new Vector2(0.2509506f, 0.1577947f), new Vector2(0.4239544f, 0.3326996f) } }; } return icon_foodMeat; }
        Vector3[][] Icon_flag() { if (icon_flag == null) { icon_flag = new Vector3[][] { new Vector3[] { new Vector2(0.2314815f, 0.03518518f), new Vector2(0.2314815f, 0.9555556f), new Vector2(0.287037f, 0.9555556f), new Vector2(0.287037f, 0.03518518f) }, new Vector3[] { new Vector2(0.287037f, 0.9222222f), new Vector2(0.3259259f, 0.8759259f), new Vector2(0.3981481f, 0.8444445f), new Vector2(0.4722222f, 0.8314815f), new Vector2(0.5388889f, 0.8425926f), new Vector2(0.5907407f, 0.8759259f), new Vector2(0.6462963f, 0.9055555f), new Vector2(0.7018518f, 0.9203704f), new Vector2(0.7685185f, 0.9222222f), new Vector2(0.8222222f, 0.8907408f), new Vector2(0.8222222f, 0.5722222f), new Vector2(0.7759259f, 0.5925926f), new Vector2(0.7111111f, 0.6018519f), new Vector2(0.6425926f, 0.5851852f), new Vector2(0.5851852f, 0.5555556f), new Vector2(0.5333334f, 0.5166667f), new Vector2(0.4814815f, 0.5074074f), new Vector2(0.4203704f, 0.5129629f), new Vector2(0.3574074f, 0.5333334f), new Vector2(0.3129629f, 0.5592592f), new Vector2(0.287037f, 0.5925926f) }, new Vector3[] { new Vector2(0.1537037f, 0.03148148f), new Vector2(0.3592592f, 0.03148148f) } }; } return icon_flag; }
        Vector3[][] Icon_flagChequered() { if (icon_flagChequered == null) { icon_flagChequered = new Vector3[][] { new Vector3[] { new Vector2(0.3444445f, 0.9444444f), new Vector2(0.3796296f, 0.8777778f), new Vector2(0.4148148f, 0.8388889f), new Vector2(0.4740741f, 0.8055556f), new Vector2(0.5388889f, 0.8f), new Vector2(0.6092592f, 0.8074074f), new Vector2(0.6777778f, 0.8277778f), new Vector2(0.7296296f, 0.837037f), new Vector2(0.7925926f, 0.8351852f), new Vector2(0.862963f, 0.8092592f), new Vector2(0.9166667f, 0.7759259f), new Vector2(0.9407408f, 0.7333333f), new Vector2(0.8055556f, 0.2777778f), new Vector2(0.7666667f, 0.3314815f), new Vector2(0.7f, 0.3703704f), new Vector2(0.612963f, 0.3777778f), new Vector2(0.5388889f, 0.3666667f), new Vector2(0.4592593f, 0.3425926f), new Vector2(0.3925926f, 0.3407407f), new Vector2(0.3222222f, 0.3574074f), new Vector2(0.2611111f, 0.3962963f), new Vector2(0.2296296f, 0.4407407f), new Vector2(0.2092593f, 0.4833333f) }, new Vector3[] { new Vector2(0.3129629f, 0.8314815f), new Vector2(0.3388889f, 0.7759259f), new Vector2(0.3962963f, 0.7166666f), new Vector2(0.4685185f, 0.6907408f), new Vector2(0.5555556f, 0.6925926f), new Vector2(0.6351852f, 0.7148148f), new Vector2(0.7111111f, 0.7259259f), new Vector2(0.7814815f, 0.7185185f), new Vector2(0.8462963f, 0.6888889f), new Vector2(0.8851852f, 0.6592593f), new Vector2(0.9074074f, 0.6222222f) }, new Vector3[] { new Vector2(0.2759259f, 0.7074074f), new Vector2(0.3092593f, 0.6462963f), new Vector2(0.3611111f, 0.5962963f), new Vector2(0.4370371f, 0.5648148f), new Vector2(0.5203704f, 0.5666667f), new Vector2(0.5981482f, 0.5888889f), new Vector2(0.6833333f, 0.6018519f), new Vector2(0.7518519f, 0.6f), new Vector2(0.8074074f, 0.5703704f), new Vector2(0.8481482f, 0.5444444f), new Vector2(0.8722222f, 0.5074074f) }, new Vector3[] { new Vector2(0.2425926f, 0.5888889f), new Vector2(0.2759259f, 0.5240741f), new Vector2(0.3185185f, 0.4814815f), new Vector2(0.3907408f, 0.45f), new Vector2(0.4666667f, 0.4518518f), new Vector2(0.55f, 0.4703704f), new Vector2(0.6351852f, 0.487037f), new Vector2(0.7074074f, 0.4833333f), new Vector2(0.7777778f, 0.4537037f), new Vector2(0.8148148f, 0.4185185f), new Vector2(0.8351852f, 0.3814815f) }, new Vector3[] { new Vector2(0.3518519f, 0.9555556f), new Vector2(0.08518519f, 0.03518518f), new Vector2(0.03333334f, 0.0537037f), new Vector2(0.3055556f, 0.9722222f), new Vector2(0.3518519f, 0.9555556f) }, new Vector3[] { new Vector2(0.4333333f, 0.8259259f), new Vector2(0.2888889f, 0.3777778f) }, new Vector3[] { new Vector2(0.5481482f, 0.8018519f), new Vector2(0.412963f, 0.3407407f) }, new Vector3[] { new Vector2(0.7018518f, 0.8333333f), new Vector2(0.5611111f, 0.3722222f) }, new Vector3[] { new Vector2(0.8314815f, 0.8203704f), new Vector2(0.7f, 0.3703704f) }, new Vector3[] { new Vector2(0.3148148f, 0.8185185f), new Vector2(0.3611111f, 0.9092593f), new Vector2(0.3333333f, 0.7851852f), new Vector2(0.3907408f, 0.8703704f), new Vector2(0.3648148f, 0.7481481f), new Vector2(0.412963f, 0.8444445f), new Vector2(0.3944444f, 0.7203704f) }, new Vector3[] { new Vector2(0.5407407f, 0.6981481f), new Vector2(0.5722222f, 0.8018519f), new Vector2(0.5685185f, 0.7f), new Vector2(0.6092592f, 0.8111111f), new Vector2(0.6092592f, 0.7111111f), new Vector2(0.6407408f, 0.8166667f), new Vector2(0.6388889f, 0.7148148f), new Vector2(0.6777778f, 0.8240741f) }, new Vector3[] { new Vector2(0.8222222f, 0.7092593f), new Vector2(0.8574074f, 0.8166667f), new Vector2(0.8425926f, 0.6888889f), new Vector2(0.8888889f, 0.7925926f), new Vector2(0.8740741f, 0.6703704f), new Vector2(0.9240741f, 0.7574074f), new Vector2(0.9f, 0.637037f) }, new Vector3[] { new Vector2(0.3759259f, 0.5888889f), new Vector2(0.4203704f, 0.7055556f), new Vector2(0.4055555f, 0.5777778f), new Vector2(0.4537037f, 0.6944444f), new Vector2(0.4333333f, 0.5666667f), new Vector2(0.487037f, 0.6925926f), new Vector2(0.462963f, 0.5685185f) }, new Vector3[] { new Vector2(0.6481481f, 0.6037037f), new Vector2(0.6925926f, 0.7240741f), new Vector2(0.6777778f, 0.6018519f), new Vector2(0.7240741f, 0.7222222f), new Vector2(0.7148148f, 0.6f), new Vector2(0.7592593f, 0.7259259f), new Vector2(0.7518519f, 0.5962963f) }, new Vector3[] { new Vector2(0.2537037f, 0.5629629f), new Vector2(0.2925926f, 0.6740741f), new Vector2(0.2685185f, 0.5351852f), new Vector2(0.3074074f, 0.6462963f), new Vector2(0.2888889f, 0.5129629f), new Vector2(0.3296296f, 0.6259259f), new Vector2(0.3074074f, 0.4981481f), new Vector2(0.3481481f, 0.6055555f) }, new Vector3[] { new Vector2(0.462963f, 0.4518518f), new Vector2(0.5018519f, 0.5666667f), new Vector2(0.4981481f, 0.4611111f), new Vector2(0.5333334f, 0.5722222f), new Vector2(0.5314815f, 0.4648148f), new Vector2(0.5722222f, 0.5833333f), new Vector2(0.5648148f, 0.4777778f), new Vector2(0.6018519f, 0.5907407f) }, new Vector3[] { new Vector2(0.75f, 0.4685185f), new Vector2(0.787037f, 0.5814815f), new Vector2(0.7740741f, 0.4592593f), new Vector2(0.8129629f, 0.5685185f), new Vector2(0.7981482f, 0.4388889f), new Vector2(0.8388889f, 0.5481482f), new Vector2(0.8240741f, 0.4092593f), new Vector2(0.8574074f, 0.5203704f) }, new Vector3[] { new Vector2(0.3055556f, 0.3685185f), new Vector2(0.3462963f, 0.4722222f), new Vector2(0.3277778f, 0.3574074f), new Vector2(0.3759259f, 0.4574074f), new Vector2(0.3592592f, 0.3462963f), new Vector2(0.4018519f, 0.4518518f), new Vector2(0.3925926f, 0.3407407f), new Vector2(0.4259259f, 0.4462963f) }, new Vector3[] { new Vector2(0.5814815f, 0.3740741f), new Vector2(0.6259259f, 0.4851852f), new Vector2(0.6148148f, 0.3759259f), new Vector2(0.6555555f, 0.4851852f), new Vector2(0.6444445f, 0.3777778f), new Vector2(0.6925926f, 0.487037f), new Vector2(0.6851852f, 0.3796296f) } }; } return icon_flagChequered; }
        Vector3[][] Icon_ball() { if (icon_ball == null) { icon_ball = new Vector3[][] { new Vector3[] { new Vector2(0.4986212f, 0.06564754f), new Vector2(0.3232045f, 0.1007309f), new Vector2(0.1924393f, 0.1852499f), new Vector2(0.09835207f, 0.3176098f), new Vector2(0.05529523f, 0.501f), new Vector2(0.0871892f, 0.6732274f), new Vector2(0.1972233f, 0.8326972f), new Vector2(0.3295832f, 0.9172162f), new Vector2(0.505f, 0.9475154f), new Vector2(0.694769f, 0.9044585f), new Vector2(0.827129f, 0.8023978f), new Vector2(0.9116479f, 0.6652539f), new Vector2(0.9387578f, 0.501f), new Vector2(0.8988903f, 0.3192045f), new Vector2(0.8095872f, 0.190034f), new Vector2(0.6708485f, 0.1023256f), new Vector2(0.4986212f, 0.06564754f) }, new Vector3[] { new Vector2(0.112963f, 0.7092593f), new Vector2(0.3981481f, 0.4462963f), new Vector2(0.4796296f, 0.4481482f), new Vector2(0.6222222f, 0.4925926f), new Vector2(0.7296296f, 0.5574074f), new Vector2(0.7907407f, 0.6166667f), new Vector2(0.8296296f, 0.6796296f), new Vector2(0.8074074f, 0.7685185f), new Vector2(0.7851852f, 0.8129629f), new Vector2(0.7148148f, 0.8925926f) }, new Vector3[] { new Vector2(0.412963f, 0.08333334f), new Vector2(0.3796296f, 0.1648148f), new Vector2(0.362963f, 0.2685185f), new Vector2(0.3685185f, 0.3777778f), new Vector2(0.3925926f, 0.45f) }, new Vector3[] { new Vector2(0.2240741f, 0.1648148f), new Vector2(0.2074074f, 0.2518519f), new Vector2(0.2074074f, 0.3722222f), new Vector2(0.2240741f, 0.4555556f), new Vector2(0.2555556f, 0.5185185f), new Vector2(0.2814815f, 0.5555556f) }, new Vector3[] { new Vector2(0.06851852f, 0.4351852f), new Vector2(0.07592592f, 0.5351852f), new Vector2(0.1074074f, 0.612963f), new Vector2(0.1314815f, 0.6555555f), new Vector2(0.1574074f, 0.6703704f) }, new Vector3[] { new Vector2(0.8296296f, 0.6777778f), new Vector2(0.8740741f, 0.6425926f), new Vector2(0.9111111f, 0.5703704f), new Vector2(0.9277778f, 0.4592593f) }, new Vector3[] { new Vector2(0.3925926f, 0.1333333f), new Vector2(0.4370371f, 0.1148148f), new Vector2(0.5111111f, 0.1037037f), new Vector2(0.6037037f, 0.1148148f), new Vector2(0.6925926f, 0.15f), new Vector2(0.7777778f, 0.1962963f), new Vector2(0.8666667f, 0.2722222f) }, new Vector3[] { new Vector2(0.3666667f, 0.2481481f), new Vector2(0.4796296f, 0.2518519f), new Vector2(0.6185185f, 0.2907407f), new Vector2(0.7537037f, 0.3462963f), new Vector2(0.8388889f, 0.3981481f), new Vector2(0.887037f, 0.4555556f), new Vector2(0.9185185f, 0.5259259f) }, new Vector3[] { new Vector2(0.1888889f, 0.8277778f), new Vector2(0.237037f, 0.8351852f), new Vector2(0.2981482f, 0.8074074f), new Vector2(0.3907408f, 0.7259259f), new Vector2(0.4685185f, 0.6277778f), new Vector2(0.5277778f, 0.5407407f), new Vector2(0.5518519f, 0.4703704f) }, new Vector3[] { new Vector2(0.3888889f, 0.9296296f), new Vector2(0.4648148f, 0.9166667f), new Vector2(0.5407407f, 0.8685185f), new Vector2(0.6037037f, 0.8f), new Vector2(0.6611111f, 0.7203704f), new Vector2(0.7092593f, 0.612963f), new Vector2(0.7166666f, 0.5462963f) } }; } return icon_ball; }
        Vector3[][] Icon_dice() { if (icon_dice == null) { icon_dice = new Vector3[][] { new Vector3[] { new Vector2(0.09814814f, 0.712963f), new Vector2(0.4111111f, 0.9648148f), new Vector2(0.9055555f, 0.8018519f), new Vector2(0.9055555f, 0.2944444f), new Vector2(0.5851852f, 0.04259259f), new Vector2(0.5851852f, 0.5537037f), new Vector2(0.09814814f, 0.7148148f), new Vector2(0.09814814f, 0.2074074f), new Vector2(0.5851852f, 0.04259259f) }, new Vector3[] { new Vector2(0.7703704f, 0.4148148f), new Vector2(0.7740741f, 0.4592593f), new Vector2(0.7611111f, 0.4833333f), new Vector2(0.7370371f, 0.4759259f), new Vector2(0.7166666f, 0.45f), new Vector2(0.7166666f, 0.3944444f), new Vector2(0.7314815f, 0.3685185f), new Vector2(0.7518519f, 0.3833333f), new Vector2(0.7703704f, 0.4148148f) }, new Vector3[] { new Vector2(0.4388889f, 0.2888889f), new Vector2(0.4722222f, 0.2611111f), new Vector2(0.4833333f, 0.2259259f), new Vector2(0.4722222f, 0.2018518f), new Vector2(0.4370371f, 0.1981481f), new Vector2(0.4111111f, 0.2222222f), new Vector2(0.3962963f, 0.2555556f), new Vector2(0.3981481f, 0.2833333f), new Vector2(0.4166667f, 0.2925926f), new Vector2(0.4388889f, 0.2888889f) }, new Vector3[] { new Vector2(0.237037f, 0.35f), new Vector2(0.2611111f, 0.3203704f), new Vector2(0.2611111f, 0.2888889f), new Vector2(0.2462963f, 0.2666667f), new Vector2(0.212963f, 0.2722222f), new Vector2(0.1814815f, 0.3037037f), new Vector2(0.1759259f, 0.3462963f), new Vector2(0.1907407f, 0.3703704f), new Vector2(0.2203704f, 0.3648148f), new Vector2(0.237037f, 0.35f) }, new Vector3[] { new Vector2(0.2203704f, 0.5851852f), new Vector2(0.2518519f, 0.5629629f), new Vector2(0.2666667f, 0.5222222f), new Vector2(0.2555556f, 0.4962963f), new Vector2(0.2314815f, 0.4796296f), new Vector2(0.2018518f, 0.4888889f), new Vector2(0.1796296f, 0.5259259f), new Vector2(0.1796296f, 0.5611111f), new Vector2(0.1925926f, 0.5888889f), new Vector2(0.2203704f, 0.5851852f) }, new Vector3[] { new Vector2(0.45f, 0.5092593f), new Vector2(0.4685185f, 0.4851852f), new Vector2(0.4777778f, 0.4351852f), new Vector2(0.4592593f, 0.4185185f), new Vector2(0.4277778f, 0.4277778f), new Vector2(0.4018519f, 0.462963f), new Vector2(0.4018519f, 0.5037037f), new Vector2(0.412963f, 0.5277778f), new Vector2(0.4333333f, 0.5277778f), new Vector2(0.45f, 0.5092593f) }, new Vector3[] { new Vector2(0.4388889f, 0.8740741f), new Vector2(0.4777778f, 0.8722222f), new Vector2(0.5018519f, 0.8592592f), new Vector2(0.5092593f, 0.8388889f), new Vector2(0.4925926f, 0.8203704f), new Vector2(0.4574074f, 0.8111111f), new Vector2(0.4203704f, 0.8129629f), new Vector2(0.3944444f, 0.837037f), new Vector2(0.3962963f, 0.8611111f), new Vector2(0.4388889f, 0.8740741f) }, new Vector3[] { new Vector2(0.5092593f, 0.7055556f), new Vector2(0.5518519f, 0.7055556f), new Vector2(0.5814815f, 0.6796296f), new Vector2(0.5740741f, 0.6555555f), new Vector2(0.5296296f, 0.6462963f), new Vector2(0.4851852f, 0.6555555f), new Vector2(0.4648148f, 0.6740741f), new Vector2(0.4685185f, 0.6981481f), new Vector2(0.5092593f, 0.7055556f) }, new Vector3[] { new Vector2(0.5833333f, 0.5518519f), new Vector2(0.9055555f, 0.8037037f) } }; } return icon_dice; }
        Vector3[][] Icon_joystick() { if (icon_joystick == null) { icon_joystick = new Vector3[][] { new Vector3[] { new Vector2(0.4995688f, 0.61307f), new Vector2(0.4327105f, 0.6264416f), new Vector2(0.3828707f, 0.6586551f), new Vector2(0.3470104f, 0.7091027f), new Vector2(0.3305997f, 0.779f), new Vector2(0.3427557f, 0.8446427f), new Vector2(0.3846941f, 0.9054229f), new Vector2(0.4351417f, 0.9376364f), new Vector2(0.502f, 0.9491847f), new Vector2(0.5743284f, 0.932774f), new Vector2(0.6247761f, 0.8938746f), new Vector2(0.6569896f, 0.8416036f), new Vector2(0.6673223f, 0.779f), new Vector2(0.6521271f, 0.7097105f), new Vector2(0.6180903f, 0.6604785f), new Vector2(0.5652114f, 0.6270494f), new Vector2(0.4995688f, 0.61307f) }, new Vector3[] { new Vector2(0.04259259f, 0.04814815f), new Vector2(0.04259259f, 0.2611111f), new Vector2(0.9481481f, 0.2611111f), new Vector2(0.9481481f, 0.04814815f), new Vector2(0.04259259f, 0.04814815f) }, new Vector3[] { new Vector2(0.3185185f, 0.2611111f), new Vector2(0.4166667f, 0.3592592f), new Vector2(0.5851852f, 0.3592592f), new Vector2(0.6740741f, 0.2611111f) }, new Vector3[] { new Vector2(0.7666667f, 0.2611111f), new Vector2(0.7666667f, 0.2944444f), new Vector2(0.8759259f, 0.2944444f), new Vector2(0.8759259f, 0.2611111f) }, new Vector3[] { new Vector2(0.1333333f, 0.04814815f), new Vector2(0.1333333f, 0.02222222f), new Vector2(0.2518519f, 0.02222222f), new Vector2(0.2518519f, 0.04814815f) }, new Vector3[] { new Vector2(0.7481481f, 0.04629629f), new Vector2(0.7481481f, 0.02222222f), new Vector2(0.8537037f, 0.02222222f), new Vector2(0.8537037f, 0.04814815f) }, new Vector3[] { new Vector2(0.4537037f, 0.3592592f), new Vector2(0.4537037f, 0.6203704f) }, new Vector3[] { new Vector2(0.5462963f, 0.3611111f), new Vector2(0.5462963f, 0.6240741f) } }; } return icon_joystick; }
        Vector3[][] Icon_gamepad() { if (icon_gamepad == null) { icon_gamepad = new Vector3[][] { new Vector3[] { new Vector2(0.2018518f, 0.6925926f), new Vector2(0.7944444f, 0.6925926f), new Vector2(0.862963f, 0.6722222f), new Vector2(0.9240741f, 0.6166667f), new Vector2(0.9648148f, 0.5407407f), new Vector2(0.9666666f, 0.4462963f), new Vector2(0.9407408f, 0.3814815f), new Vector2(0.8962963f, 0.3277778f), new Vector2(0.8203704f, 0.2925926f), new Vector2(0.7388889f, 0.287037f), new Vector2(0.6685185f, 0.3111111f), new Vector2(0.6222222f, 0.3425926f), new Vector2(0.5962963f, 0.3814815f), new Vector2(0.4092593f, 0.3814815f), new Vector2(0.3648148f, 0.3333333f), new Vector2(0.2851852f, 0.2888889f), new Vector2(0.1981481f, 0.2851852f), new Vector2(0.1240741f, 0.3148148f), new Vector2(0.06481481f, 0.3666667f), new Vector2(0.02962963f, 0.4518518f), new Vector2(0.03703704f, 0.5462963f), new Vector2(0.07407407f, 0.6203704f), new Vector2(0.1351852f, 0.6740741f), new Vector2(0.2018518f, 0.6925926f) }, new Vector3[] { new Vector2(0.8331f, 0.4997008f), new Vector2(0.7961705f, 0.515013f), new Vector2(0.7815788f, 0.5487f), new Vector2(0.7956301f, 0.5840082f), new Vector2(0.8321993f, 0.5996807f), new Vector2(0.8675075f, 0.5856295f), new Vector2(0.8817389f, 0.5485198f), new Vector2(0.8673274f, 0.5133917f), new Vector2(0.8331f, 0.4997008f) }, new Vector3[] { new Vector2(0.71f, 0.377711f), new Vector2(0.6730704f, 0.3929888f), new Vector2(0.6584788f, 0.4266f), new Vector2(0.6725301f, 0.4618289f), new Vector2(0.7090992f, 0.4774662f), new Vector2(0.7444075f, 0.4634465f), new Vector2(0.7586389f, 0.4264203f), new Vector2(0.7442273f, 0.3913712f), new Vector2(0.71f, 0.377711f) }, new Vector3[] { new Vector2(0.2740741f, 0.6055555f), new Vector2(0.2740741f, 0.5425926f), new Vector2(0.3425926f, 0.5425926f), new Vector2(0.3425926f, 0.4833333f), new Vector2(0.2722222f, 0.4833333f), new Vector2(0.2722222f, 0.412963f), new Vector2(0.2185185f, 0.412963f), new Vector2(0.2185185f, 0.4833333f), new Vector2(0.1518518f, 0.4833333f), new Vector2(0.1518518f, 0.5425926f), new Vector2(0.2166667f, 0.5425926f), new Vector2(0.2166667f, 0.6037037f), new Vector2(0.2722222f, 0.6037037f) } }; } return icon_gamepad; }
        Vector3[][] Icon_jigsawPuzzle() { if (icon_jigsawPuzzle == null) { icon_jigsawPuzzle = new Vector3[][] { new Vector3[] { new Vector2(0.7259259f, 0.5777778f), new Vector2(0.7388889f, 0.5462963f), new Vector2(0.7629629f, 0.537037f), new Vector2(0.7888889f, 0.55f), new Vector2(0.8037037f, 0.5722222f), new Vector2(0.837037f, 0.5981482f), new Vector2(0.8888889f, 0.5944445f), new Vector2(0.9296296f, 0.5574074f), new Vector2(0.9462963f, 0.5f), new Vector2(0.9351852f, 0.4388889f), new Vector2(0.9074074f, 0.4055555f), new Vector2(0.8759259f, 0.3851852f), new Vector2(0.8277778f, 0.4f), new Vector2(0.7962963f, 0.4222222f), new Vector2(0.7555556f, 0.4259259f), new Vector2(0.7333333f, 0.4f), new Vector2(0.7277778f, 0.3611111f), new Vector2(0.7277778f, 0.08888889f), new Vector2(0.4925926f, 0.08888889f), new Vector2(0.4462963f, 0.1055556f), new Vector2(0.4388889f, 0.1333333f), new Vector2(0.4592593f, 0.1648148f), new Vector2(0.4851852f, 0.1833333f), new Vector2(0.4907407f, 0.2259259f), new Vector2(0.4740741f, 0.2703704f), new Vector2(0.4370371f, 0.3f), new Vector2(0.3759259f, 0.3185185f), new Vector2(0.3259259f, 0.2962963f), new Vector2(0.2888889f, 0.25f), new Vector2(0.2796296f, 0.2074074f), new Vector2(0.2981482f, 0.1648148f), new Vector2(0.3333333f, 0.1240741f), new Vector2(0.3314815f, 0.112963f), new Vector2(0.2851852f, 0.09259259f), new Vector2(0.03703704f, 0.09259259f), new Vector2(0.03703704f, 0.4037037f), new Vector2(0.05185185f, 0.4314815f), new Vector2(0.07962963f, 0.4388889f), new Vector2(0.1f, 0.4185185f), new Vector2(0.1277778f, 0.3981481f), new Vector2(0.1685185f, 0.387037f), new Vector2(0.2166667f, 0.4018519f), new Vector2(0.2462963f, 0.4481482f), new Vector2(0.2555556f, 0.5055556f), new Vector2(0.2407407f, 0.5574074f), new Vector2(0.1981481f, 0.5962963f), new Vector2(0.1388889f, 0.5981482f), new Vector2(0.1092593f, 0.5666667f), new Vector2(0.07222223f, 0.5518519f), new Vector2(0.04814815f, 0.5740741f), new Vector2(0.03888889f, 0.6314815f), new Vector2(0.03888889f, 0.912963f), new Vector2(0.3111111f, 0.912963f), new Vector2(0.3425926f, 0.8981481f), new Vector2(0.3481481f, 0.8703704f), new Vector2(0.3296296f, 0.8259259f), new Vector2(0.2925926f, 0.7925926f), new Vector2(0.2888889f, 0.7462963f), new Vector2(0.3166667f, 0.7f), new Vector2(0.362963f, 0.6703704f), new Vector2(0.4148148f, 0.6740741f), new Vector2(0.4759259f, 0.7074074f), new Vector2(0.4925926f, 0.7462963f), new Vector2(0.4888889f, 0.8037037f), new Vector2(0.4685185f, 0.8425926f), new Vector2(0.45f, 0.8703704f), new Vector2(0.4518518f, 0.9074074f), new Vector2(0.4981481f, 0.912963f), new Vector2(0.7259259f, 0.912963f), new Vector2(0.7259259f, 0.5759259f) } }; } return icon_jigsawPuzzle; }
        Vector3[][] Icon_fish() { if (icon_fish == null) { icon_fish = new Vector3[][] { new Vector3[] { new Vector2(0.9518518f, 0.4981481f), new Vector2(0.8740741f, 0.4074074f), new Vector2(0.7407407f, 0.3296296f), new Vector2(0.6148148f, 0.2925926f), new Vector2(0.487037f, 0.2962963f), new Vector2(0.3703704f, 0.3277778f), new Vector2(0.2814815f, 0.3722222f), new Vector2(0.2333333f, 0.4148148f), new Vector2(0.1574074f, 0.3555556f), new Vector2(0.04444445f, 0.3037037f), new Vector2(0.137037f, 0.4981481f), new Vector2(0.04814815f, 0.6851852f), new Vector2(0.1555556f, 0.6388889f), new Vector2(0.2314815f, 0.5759259f), new Vector2(0.3407407f, 0.6518518f), new Vector2(0.4814815f, 0.6944444f), new Vector2(0.6296296f, 0.6944444f), new Vector2(0.7462963f, 0.6592593f), new Vector2(0.8481482f, 0.6018519f), new Vector2(0.9111111f, 0.5444444f), new Vector2(0.9518518f, 0.4981481f) }, new Vector3[] { new Vector2(0.778f, 0.5082673f), new Vector2(0.7495618f, 0.5200588f), new Vector2(0.7383252f, 0.546f), new Vector2(0.7491456f, 0.5731897f), new Vector2(0.7773064f, 0.5852587f), new Vector2(0.8044961f, 0.5744382f), new Vector2(0.8154553f, 0.5458613f), new Vector2(0.8043574f, 0.5188103f), new Vector2(0.778f, 0.5082673f) }, new Vector3[] { new Vector2(0.7166666f, 0.6666667f), new Vector2(0.6740741f, 0.5851852f), new Vector2(0.662963f, 0.5055556f), new Vector2(0.6722222f, 0.4240741f), new Vector2(0.6962963f, 0.3703704f), new Vector2(0.7240741f, 0.3240741f) } }; } return icon_fish; }
        Vector3[][] Icon_car() { if (icon_car == null) { icon_car = new Vector3[][] { new Vector3[] { new Vector2(0.2527f, 0.2096296f), new Vector2(0.1857204f, 0.2374016f), new Vector2(0.1592553f, 0.2985f), new Vector2(0.1847402f, 0.362539f), new Vector2(0.2510664f, 0.3909645f), new Vector2(0.3151053f, 0.3654796f), new Vector2(0.340917f, 0.2981733f), new Vector2(0.3147786f, 0.234461f), new Vector2(0.2527f, 0.2096296f) }, new Vector3[] { new Vector2(0.6998f, 0.2065166f), new Vector2(0.6313001f, 0.2348552f), new Vector2(0.6042343f, 0.2972f), new Vector2(0.6302977f, 0.3625454f), new Vector2(0.6981293f, 0.3915507f), new Vector2(0.7636219f, 0.3655459f), new Vector2(0.7900194f, 0.2968666f), new Vector2(0.7632877f, 0.2318546f), new Vector2(0.6998f, 0.2065166f) }, new Vector3[] { new Vector2(0.3909091f, 0.3057851f), new Vector2(0.5495868f, 0.3057851f), new Vector2(0.5694215f, 0.3752066f), new Vector2(0.6140496f, 0.4247934f), new Vector2(0.6867769f, 0.4446281f), new Vector2(0.7545455f, 0.4297521f), new Vector2(0.8024793f, 0.3867769f), new Vector2(0.8256198f, 0.3371901f), new Vector2(0.8305785f, 0.3008265f), new Vector2(0.9545454f, 0.3008265f), new Vector2(0.9198347f, 0.4909091f), new Vector2(0.9016529f, 0.5157025f), new Vector2(0.8669422f, 0.5322314f), new Vector2(0.716529f, 0.5322314f), new Vector2(0.5826446f, 0.7256199f), new Vector2(0.2090909f, 0.7256199f), new Vector2(0.161157f, 0.5338843f), new Vector2(0.06033058f, 0.5338843f), new Vector2(0.06033058f, 0.3057851f), new Vector2(0.1099174f, 0.3057851f), new Vector2(0.1165289f, 0.3652893f), new Vector2(0.1545455f, 0.4181818f), new Vector2(0.2090909f, 0.446281f), new Vector2(0.2702479f, 0.4512397f), new Vector2(0.3198347f, 0.4363636f), new Vector2(0.3628099f, 0.3950413f), new Vector2(0.3909091f, 0.3487603f), new Vector2(0.3909091f, 0.3057851f) }, new Vector3[] { new Vector2(0.6454545f, 0.5338843f), new Vector2(0.392562f, 0.5338843f), new Vector2(0.392562f, 0.6876033f), new Vector2(0.5413223f, 0.6876033f), new Vector2(0.6454545f, 0.5338843f) }, new Vector3[] { new Vector2(0.2520661f, 0.6892562f), new Vector2(0.3429752f, 0.6892562f), new Vector2(0.3429752f, 0.5355372f), new Vector2(0.222314f, 0.5355372f), new Vector2(0.2520661f, 0.6892562f) } }; } return icon_car; }
        Vector3[][] Icon_tree() { if (icon_tree == null) { icon_tree = new Vector3[][] { new Vector3[] { new Vector2(0.5703704f, 0.3814815f), new Vector2(0.4537037f, 0.3814815f), new Vector2(0.3462963f, 0.3722222f), new Vector2(0.2925926f, 0.387037f), new Vector2(0.2333333f, 0.4314815f), new Vector2(0.2074074f, 0.4944444f), new Vector2(0.2148148f, 0.5592592f), new Vector2(0.1648148f, 0.5944445f), new Vector2(0.1407407f, 0.6555555f), new Vector2(0.1537037f, 0.7111111f), new Vector2(0.187037f, 0.75f), new Vector2(0.2166667f, 0.7592593f), new Vector2(0.2314815f, 0.8296296f), new Vector2(0.2685185f, 0.8777778f), new Vector2(0.3277778f, 0.9037037f), new Vector2(0.362963f, 0.9074074f), new Vector2(0.3981481f, 0.8962963f), new Vector2(0.4277778f, 0.9388889f), new Vector2(0.4833333f, 0.9611111f), new Vector2(0.5518519f, 0.9648148f), new Vector2(0.6018519f, 0.9425926f), new Vector2(0.6351852f, 0.9055555f), new Vector2(0.6481481f, 0.8759259f), new Vector2(0.712963f, 0.8888889f), new Vector2(0.7685185f, 0.8611111f), new Vector2(0.8111111f, 0.8018519f), new Vector2(0.8240741f, 0.7370371f), new Vector2(0.8055556f, 0.6796296f), new Vector2(0.8388889f, 0.662963f), new Vector2(0.8592592f, 0.6074074f), new Vector2(0.85f, 0.5537037f), new Vector2(0.8185185f, 0.5222222f), new Vector2(0.7759259f, 0.5055556f), new Vector2(0.7648148f, 0.4518518f), new Vector2(0.7296296f, 0.4037037f), new Vector2(0.6851852f, 0.3740741f), new Vector2(0.6314815f, 0.3648148f), new Vector2(0.5703704f, 0.3814815f) }, new Vector3[] { new Vector2(0.5703704f, 0.3777778f), new Vector2(0.5703704f, 0.1574074f), new Vector2(0.587037f, 0.03888889f), new Vector2(0.6277778f, 0.02222222f), new Vector2(0.3925926f, 0.02222222f), new Vector2(0.4444444f, 0.05f), new Vector2(0.4555556f, 0.1925926f), new Vector2(0.4555556f, 0.3814815f) } }; } return icon_tree; }
        Vector3[][] Icon_palm() { if (icon_palm == null) { icon_palm = new Vector3[][] { new Vector3[] { new Vector2(0.5537037f, 0.6518518f), new Vector2(0.5185185f, 0.6722222f), new Vector2(0.4388889f, 0.65f), new Vector2(0.3611111f, 0.5981482f), new Vector2(0.3129629f, 0.5425926f), new Vector2(0.2962963f, 0.4851852f), new Vector2(0.2962963f, 0.45f), new Vector2(0.262963f, 0.5074074f), new Vector2(0.2555556f, 0.5574074f), new Vector2(0.2740741f, 0.6240741f), new Vector2(0.3296296f, 0.6944444f), new Vector2(0.3833333f, 0.7370371f), new Vector2(0.4444444f, 0.7611111f), new Vector2(0.3740741f, 0.7981482f), new Vector2(0.2833333f, 0.8018519f), new Vector2(0.2462963f, 0.7851852f), new Vector2(0.2851852f, 0.8444445f), new Vector2(0.337037f, 0.8666667f), new Vector2(0.4166667f, 0.8740741f), new Vector2(0.4851852f, 0.8592592f), new Vector2(0.5240741f, 0.8407407f), new Vector2(0.4925926f, 0.9092593f), new Vector2(0.4555556f, 0.9462963f), new Vector2(0.4203704f, 0.9537037f), new Vector2(0.4555556f, 0.9740741f), new Vector2(0.5037037f, 0.9685185f), new Vector2(0.5666667f, 0.9203704f), new Vector2(0.5944445f, 0.8648148f), new Vector2(0.662963f, 0.9f), new Vector2(0.7240741f, 0.9074074f), new Vector2(0.7759259f, 0.8981481f), new Vector2(0.8092592f, 0.8685185f), new Vector2(0.8296296f, 0.8111111f), new Vector2(0.7759259f, 0.8388889f), new Vector2(0.7185185f, 0.8388889f), new Vector2(0.6518518f, 0.8055556f), new Vector2(0.7425926f, 0.7888889f), new Vector2(0.8092592f, 0.7407407f), new Vector2(0.8537037f, 0.6759259f), new Vector2(0.8648148f, 0.6092592f), new Vector2(0.85f, 0.5555556f), new Vector2(0.8203704f, 0.6222222f), new Vector2(0.7703704f, 0.6648148f), new Vector2(0.7092593f, 0.6907408f), new Vector2(0.6666667f, 0.6944444f), new Vector2(0.7333333f, 0.5981482f), new Vector2(0.7611111f, 0.4962963f), new Vector2(0.7481481f, 0.4314815f), new Vector2(0.7018518f, 0.3777778f), new Vector2(0.6462963f, 0.3518519f), new Vector2(0.6777778f, 0.4277778f), new Vector2(0.6759259f, 0.5018519f), new Vector2(0.6425926f, 0.5796296f), new Vector2(0.5925926f, 0.6425926f), new Vector2(0.5537037f, 0.6518518f) }, new Vector3[] { new Vector2(0.2759259f, 0.04629629f), new Vector2(0.5351852f, 0.04629629f) }, new Vector3[] { new Vector2(0.3388889f, 0.05185185f), new Vector2(0.3740741f, 0.2203704f), new Vector2(0.4592593f, 0.5f), new Vector2(0.5203704f, 0.6648148f) }, new Vector3[] { new Vector2(0.4685185f, 0.04814815f), new Vector2(0.4574074f, 0.1259259f), new Vector2(0.4648148f, 0.2685185f), new Vector2(0.487037f, 0.4407407f), new Vector2(0.5240741f, 0.5814815f), new Vector2(0.5481482f, 0.6537037f) } }; } return icon_palm; }
        Vector3[][] Icon_leaf() { if (icon_leaf == null) { icon_leaf = new Vector3[][] { new Vector3[] { new Vector2(0.7814815f, 0.6740741f), new Vector2(0.7055556f, 0.5462963f), new Vector2(0.6148148f, 0.4166667f), new Vector2(0.5148148f, 0.3092593f), new Vector2(0.3962963f, 0.2259259f), new Vector2(0.2611111f, 0.162963f), new Vector2(0.1611111f, 0.1425926f), new Vector2(0.08148148f, 0.1351852f), new Vector2(0.08148148f, 0.06111111f), new Vector2(0.1703704f, 0.06666667f), new Vector2(0.3f, 0.09814814f), new Vector2(0.4148148f, 0.162963f), new Vector2(0.5240741f, 0.262963f), new Vector2(0.6296296f, 0.3759259f), new Vector2(0.7037037f, 0.4925926f), new Vector2(0.7796296f, 0.6333333f) }, new Vector3[] { new Vector2(0.3092593f, 0.187037f), new Vector2(0.2185185f, 0.2611111f), new Vector2(0.1777778f, 0.3851852f), new Vector2(0.1981481f, 0.5074074f), new Vector2(0.2537037f, 0.6185185f), new Vector2(0.3259259f, 0.6962963f), new Vector2(0.4425926f, 0.7574074f), new Vector2(0.5907407f, 0.8055556f), new Vector2(0.7166666f, 0.8537037f), new Vector2(0.8129629f, 0.8981481f), new Vector2(0.8833333f, 0.9388889f), new Vector2(0.9055555f, 0.8259259f), new Vector2(0.9259259f, 0.6685185f), new Vector2(0.9222222f, 0.5074074f), new Vector2(0.9018518f, 0.3611111f), new Vector2(0.8555555f, 0.2407407f), new Vector2(0.7944444f, 0.1592593f), new Vector2(0.7018518f, 0.1092593f), new Vector2(0.6111111f, 0.09259259f), new Vector2(0.5148148f, 0.0962963f), new Vector2(0.3833333f, 0.1444445f) } }; } return icon_leaf; }
        Vector3[][] Icon_nukeNuclearWarning() { if (icon_nukeNuclearWarning == null) { icon_nukeNuclearWarning = new Vector3[][] { new Vector3[] { new Vector2(0.4140071f, 0.356383f), new Vector2(0.4512411f, 0.3368794f), new Vector2(0.4920213f, 0.3333333f), new Vector2(0.5398936f, 0.3386525f), new Vector2(0.5789007f, 0.3528369f), new Vector2(0.7207447f, 0.09042553f), new Vector2(0.6533688f, 0.05851064f), new Vector2(0.5877659f, 0.04255319f), new Vector2(0.518617f, 0.03191489f), new Vector2(0.4423759f, 0.03191489f), new Vector2(0.3590426f, 0.04787234f), new Vector2(0.3076241f, 0.06914894f), new Vector2(0.266844f, 0.09397163f), new Vector2(0.4140071f, 0.356383f) }, new Vector3[] { new Vector2(0.6673725f, 0.4973365f), new Vector2(0.665646f, 0.5393339f), new Vector2(0.6483269f, 0.5764235f), new Vector2(0.6197842f, 0.6152227f), new Vector2(0.5879967f, 0.6419116f), new Vector2(0.7443296f, 0.8959578f), new Vector2(0.8056566f, 0.853566f), new Vector2(0.8522776f, 0.804731f), new Vector2(0.8960651f, 0.7501655f), new Vector2(0.9341856f, 0.6841387f), new Vector2(0.9620328f, 0.6039912f), new Vector2(0.9693159f, 0.5488232f), new Vector2(0.9682089f, 0.5010952f), new Vector2(0.6673725f, 0.4973365f) }, new Vector3[] { new Vector2(0.4186204f, 0.6462805f), new Vector2(0.3831128f, 0.6237867f), new Vector2(0.3596518f, 0.5902431f), new Vector2(0.3403221f, 0.5461249f), new Vector2(0.3331026f, 0.5052515f), new Vector2(0.03492573f, 0.5136167f), new Vector2(0.04097456f, 0.5879234f), new Vector2(0.05995643f, 0.6527159f), new Vector2(0.08531785f, 0.7179197f), new Vector2(0.1234384f, 0.7839465f), new Vector2(0.1789246f, 0.8481365f), new Vector2(0.2230599f, 0.882028f), new Vector2(0.2649471f, 0.9049332f), new Vector2(0.4186204f, 0.6462805f) }, new Vector3[] { new Vector2(0.4977532f, 0.3959289f), new Vector2(0.4552153f, 0.4044364f), new Vector2(0.4235053f, 0.4249319f), new Vector2(0.4006896f, 0.4570287f), new Vector2(0.3902484f, 0.5015f), new Vector2(0.3979826f, 0.5432644f), new Vector2(0.4246655f, 0.5819352f), new Vector2(0.4567622f, 0.6024307f), new Vector2(0.4993f, 0.6097782f), new Vector2(0.5453182f, 0.599337f), new Vector2(0.5774149f, 0.5745878f), new Vector2(0.5979105f, 0.5413309f), new Vector2(0.6044845f, 0.5015f), new Vector2(0.5948168f, 0.4574153f), new Vector2(0.5731611f, 0.426092f), new Vector2(0.5395176f, 0.4048231f), new Vector2(0.4977532f, 0.3959289f) } }; } return icon_nukeNuclearWarning; }
        Vector3[][] Icon_biohazardWarning() { if (icon_biohazardWarning == null) { icon_biohazardWarning = new Vector3[][] { new Vector3[] { new Vector2(0.5246479f, 0.5361549f), new Vector2(0.5246479f, 0.4903803f), new Vector2(0.5440141f, 0.4850986f), new Vector2(0.5616197f, 0.4657324f), new Vector2(0.568662f, 0.4393239f), new Vector2(0.5616197f, 0.4164366f), new Vector2(0.6038733f, 0.3953098f), new Vector2(0.6285211f, 0.4287605f), new Vector2(0.6725352f, 0.4622113f), new Vector2(0.7147887f, 0.4798169f), new Vector2(0.7816901f, 0.483338f), new Vector2(0.8380282f, 0.4727746f), new Vector2(0.8873239f, 0.4481267f), new Vector2(0.9207746f, 0.4164366f), new Vector2(0.9507042f, 0.3724225f), new Vector2(0.9647887f, 0.3143239f), new Vector2(0.9647887f, 0.3847465f), new Vector2(0.9401408f, 0.4446057f), new Vector2(0.8926057f, 0.5009437f), new Vector2(0.8485916f, 0.5343943f), new Vector2(0.7975352f, 0.5608028f), new Vector2(0.7464789f, 0.5713662f), new Vector2(0.7623239f, 0.6171408f), new Vector2(0.7676057f, 0.6805211f), new Vector2(0.7588028f, 0.745662f), new Vector2(0.7359155f, 0.8090422f), new Vector2(0.6919014f, 0.8671408f), new Vector2(0.6443662f, 0.9041127f), new Vector2(0.5792254f, 0.9252395f), new Vector2(0.6408451f, 0.886507f), new Vector2(0.6725352f, 0.8477746f), new Vector2(0.6989437f, 0.7984789f), new Vector2(0.7059859f, 0.7386197f), new Vector2(0.6971831f, 0.6699578f), new Vector2(0.6707746f, 0.620662f), new Vector2(0.6320422f, 0.5784084f), new Vector2(0.5774648f, 0.5449578f), new Vector2(0.5246479f, 0.5361549f) }, new Vector3[] { new Vector2(0.4753521f, 0.5361549f), new Vector2(0.4753521f, 0.4903803f), new Vector2(0.4559859f, 0.4850986f), new Vector2(0.4383803f, 0.4657324f), new Vector2(0.431338f, 0.4393239f), new Vector2(0.4383803f, 0.4164366f), new Vector2(0.3961267f, 0.3953098f), new Vector2(0.3714789f, 0.4287605f), new Vector2(0.3274648f, 0.4622113f), new Vector2(0.2852113f, 0.4798169f), new Vector2(0.2183099f, 0.483338f), new Vector2(0.1619718f, 0.4727746f), new Vector2(0.1126761f, 0.4481267f), new Vector2(0.07922542f, 0.4164366f), new Vector2(0.04929578f, 0.3724225f), new Vector2(0.03521132f, 0.3143239f), new Vector2(0.03521132f, 0.3847465f), new Vector2(0.05985922f, 0.4446057f), new Vector2(0.1073943f, 0.5009437f), new Vector2(0.1514084f, 0.5343943f), new Vector2(0.2024648f, 0.5608028f), new Vector2(0.2535211f, 0.5713662f), new Vector2(0.2376761f, 0.6171408f), new Vector2(0.2323943f, 0.6805211f), new Vector2(0.2411972f, 0.745662f), new Vector2(0.2640845f, 0.8090422f), new Vector2(0.3080986f, 0.8671408f), new Vector2(0.3556338f, 0.9041127f), new Vector2(0.4207746f, 0.9252395f), new Vector2(0.3591549f, 0.886507f), new Vector2(0.3274648f, 0.8477746f), new Vector2(0.3010563f, 0.7984789f), new Vector2(0.2940141f, 0.7386197f), new Vector2(0.3028169f, 0.6699578f), new Vector2(0.3292254f, 0.620662f), new Vector2(0.3679578f, 0.5784084f), new Vector2(0.4225352f, 0.5449578f), new Vector2(0.4753521f, 0.5361549f) }, new Vector3[] { new Vector2(0.582207f, 0.3510769f), new Vector2(0.542565f, 0.3739641f), new Vector2(0.5283079f, 0.3598334f), new Vector2(0.5027334f, 0.3542696f), new Vector2(0.4763418f, 0.361375f), new Vector2(0.460042f, 0.3789175f), new Vector2(0.4206188f, 0.3528882f), new Vector2(0.4372641f, 0.3148172f), new Vector2(0.4442263f, 0.2599745f), new Vector2(0.4383465f, 0.2145791f), new Vector2(0.4079451f, 0.1548802f), new Vector2(0.3706279f, 0.1113717f), new Vector2(0.3246343f, 0.08100431f), new Vector2(0.2804645f, 0.0678802f), new Vector2(0.2273824f, 0.06396741f), new Vector2(0.1700253f, 0.0808192f), new Vector2(0.2310131f, 0.0456079f), new Vector2(0.2951766f, 0.037024f), new Vector2(0.3677343f, 0.0500216f), new Vector2(0.4187104f, 0.07141361f), new Vector2(0.467109f, 0.1024256f), new Vector2(0.5017853f, 0.1413599f), new Vector2(0.5335048f, 0.1047504f), new Vector2(0.5857528f, 0.06848609f), new Vector2(0.6465681f, 0.0435392f), new Vector2(0.7129006f, 0.03167f), new Vector2(0.7852224f, 0.0407381f), new Vector2(0.8410087f, 0.06341881f), new Vector2(0.8918754f, 0.109269f), new Vector2(0.8275222f, 0.0752711f), new Vector2(0.7781339f, 0.06719281f), new Vector2(0.7222384f, 0.0689702f), new Vector2(0.6668776f, 0.09280109f), new Vector2(0.6118161f, 0.1347555f), new Vector2(0.5823289f, 0.1822739f), new Vector2(0.5651024f, 0.2369439f), new Vector2(0.563422f, 0.3009346f), new Vector2(0.582207f, 0.3510769f) }, new Vector3[] { new Vector2(0.4070987f, 0.259048f), new Vector2(0.3858456f, 0.1940673f), new Vector2(0.3253296f, 0.237138f), new Vector2(0.291441f, 0.2770328f), new Vector2(0.265239f, 0.3267201f), new Vector2(0.251062f, 0.3796295f), new Vector2(0.2497901f, 0.4372857f), new Vector2(0.3067387f, 0.4267686f), new Vector2(0.3083097f, 0.3872361f), new Vector2(0.3251275f, 0.3389008f), new Vector2(0.3511568f, 0.2994776f), new Vector2(0.3802986f, 0.272488f), new Vector2(0.4070987f, 0.259048f) }, new Vector3[] { new Vector2(0.6919014f, 0.4305211f), new Vector2(0.7588028f, 0.4446057f), new Vector2(0.7517605f, 0.370662f), new Vector2(0.7341549f, 0.3213662f), new Vector2(0.7042254f, 0.273831f), new Vector2(0.665493f, 0.2350986f), new Vector2(0.6161972f, 0.205169f), new Vector2(0.596831f, 0.2597465f), new Vector2(0.6302817f, 0.2808733f), new Vector2(0.6637324f, 0.3196056f), new Vector2(0.6848592f, 0.3618592f), new Vector2(0.693662f, 0.4005915f), new Vector2(0.6919014f, 0.4305211f) }, new Vector3[] { new Vector2(0.4059011f, 0.5936667f), new Vector2(0.4409888f, 0.6112105f), new Vector2(0.4984051f, 0.6207799f), new Vector2(0.5478469f, 0.6175901f), new Vector2(0.5972887f, 0.5920718f), new Vector2(0.638756f, 0.6383237f), new Vector2(0.6036683f, 0.6606523f), new Vector2(0.562201f, 0.6750064f), new Vector2(0.5127591f, 0.6813859f), new Vector2(0.453748f, 0.6766013f), new Vector2(0.4027113f, 0.6606523f), new Vector2(0.3596491f, 0.6415135f), new Vector2(0.4059011f, 0.5936667f) } }; } return icon_biohazardWarning; }
        Vector3[][] Icon_fireWarning() { if (icon_fireWarning == null) { icon_fireWarning = new Vector3[][] { new Vector3[] { new Vector2(0.05685619f, 0.1722408f), new Vector2(0.05518395f, 0.1438127f), new Vector2(0.06187291f, 0.1137124f), new Vector2(0.08361204f, 0.09364548f), new Vector2(0.1120401f, 0.08026756f), new Vector2(0.861204f, 0.08026756f), new Vector2(0.8946488f, 0.09030101f), new Vector2(0.9197325f, 0.1086956f), new Vector2(0.9280937f, 0.1404682f), new Vector2(0.9247491f, 0.1755853f), new Vector2(0.5535117f, 0.8244147f), new Vector2(0.5317726f, 0.8494983f), new Vector2(0.4949833f, 0.8595318f), new Vector2(0.4548495f, 0.8511705f), new Vector2(0.4314381f, 0.8277592f), new Vector2(0.05685619f, 0.1722408f) }, new Vector3[] { new Vector2(0.404943f, 0.2376426f), new Vector2(0.3441065f, 0.269962f), new Vector2(0.3117871f, 0.3326996f), new Vector2(0.3022814f, 0.4144487f), new Vector2(0.3346007f, 0.4809886f), new Vector2(0.3897339f, 0.5418251f), new Vector2(0.3973384f, 0.5038023f), new Vector2(0.4201521f, 0.4847909f), new Vector2(0.4239544f, 0.5551331f), new Vector2(0.460076f, 0.6026616f), new Vector2(0.5494297f, 0.6596958f), new Vector2(0.5304183f, 0.5779468f), new Vector2(0.5475285f, 0.5285171f), new Vector2(0.5779468f, 0.4904943f), new Vector2(0.6007605f, 0.5361217f), new Vector2(0.6330798f, 0.5437263f), new Vector2(0.6178707f, 0.4809886f), new Vector2(0.6501901f, 0.4125475f), new Vector2(0.6692015f, 0.3498099f), new Vector2(0.6520913f, 0.2889734f), new Vector2(0.6007605f, 0.2585551f), new Vector2(0.526616f, 0.2395437f), new Vector2(0.5570342f, 0.2775666f), new Vector2(0.5627376f, 0.3231939f), new Vector2(0.5304183f, 0.365019f), new Vector2(0.473384f, 0.4125475f), new Vector2(0.4885932f, 0.473384f), new Vector2(0.4315589f, 0.4201521f), new Vector2(0.4068441f, 0.3555133f), new Vector2(0.4182509f, 0.2984791f), new Vector2(0.3669201f, 0.3250951f), new Vector2(0.3669201f, 0.2870722f), new Vector2(0.404943f, 0.2376426f) }, new Vector3[] { new Vector2(0.2984791f, 0.1711027f), new Vector2(0.6825095f, 0.1711027f) } }; } return icon_fireWarning; }
        Vector3[][] Icon_warning() { if (icon_warning == null) { icon_warning = new Vector3[][] { new Vector3[] { new Vector2(0.05685619f, 0.1722408f), new Vector2(0.05518395f, 0.1438127f), new Vector2(0.06187291f, 0.1137124f), new Vector2(0.08361204f, 0.09364548f), new Vector2(0.1120401f, 0.08026756f), new Vector2(0.861204f, 0.08026756f), new Vector2(0.8946488f, 0.09030101f), new Vector2(0.9197325f, 0.1086956f), new Vector2(0.9280937f, 0.1404682f), new Vector2(0.9247491f, 0.1755853f), new Vector2(0.5535117f, 0.8244147f), new Vector2(0.5317726f, 0.8494983f), new Vector2(0.4949833f, 0.8595318f), new Vector2(0.4548495f, 0.8511705f), new Vector2(0.4314381f, 0.8277592f), new Vector2(0.05685619f, 0.1722408f) }, new Vector3[] { new Vector2(0.4481605f, 0.2993311f), new Vector2(0.4481605f, 0.2123746f), new Vector2(0.5334448f, 0.2123746f), new Vector2(0.5334448f, 0.2976589f), new Vector2(0.4481605f, 0.2993311f) }, new Vector3[] { new Vector2(0.4481605f, 0.3879599f), new Vector2(0.4481605f, 0.6454849f), new Vector2(0.5334448f, 0.6454849f), new Vector2(0.5334448f, 0.3879599f), new Vector2(0.4481605f, 0.3879599f) } }; } return icon_warning; }
        Vector3[][] Icon_emergencyExit() { if (icon_emergencyExit == null) { icon_emergencyExit = new Vector3[][] { new Vector3[] { new Vector2(0.1054965f, 0.4698582f), new Vector2(0.1054965f, 0.7375886f), new Vector2(0.3891844f, 0.7375886f), new Vector2(0.3891844f, 0.5673759f), new Vector2(0.3466312f, 0.5602837f), new Vector2(0.3005319f, 0.6205674f), new Vector2(0.1710993f, 0.6205674f), new Vector2(0.1320922f, 0.5531915f), new Vector2(0.1356383f, 0.5372341f), new Vector2(0.162234f, 0.5443262f), new Vector2(0.1870567f, 0.5904256f), new Vector2(0.2260638f, 0.5904256f), new Vector2(0.1870567f, 0.5159575f), new Vector2(0.1870567f, 0.4432624f), new Vector2(0.1037234f, 0.4432624f), new Vector2(0.06294326f, 0.4007092f), new Vector2(0.2083333f, 0.4007092f), new Vector2(0.2260638f, 0.4202128f), new Vector2(0.2260638f, 0.4840426f), new Vector2(0.3058511f, 0.3492908f), new Vector2(0.3484043f, 0.3492908f), new Vector2(0.2721631f, 0.5124114f), new Vector2(0.2952128f, 0.570922f), new Vector2(0.3306738f, 0.5265958f), new Vector2(0.3909574f, 0.5265958f), new Vector2(0.3909574f, 0.3351064f), new Vector2(0.3608156f, 0.2978723f) }, new Vector3[] { new Vector2(0.2774823f, 0.6382979f), new Vector2(0.302305f, 0.6382979f), new Vector2(0.3218085f, 0.6542553f), new Vector2(0.3218085f, 0.680851f), new Vector2(0.3076241f, 0.7021276f), new Vector2(0.2757092f, 0.7021276f), new Vector2(0.2579787f, 0.6843972f), new Vector2(0.2579787f, 0.6560284f), new Vector2(0.2774823f, 0.6382979f) }, new Vector3[] { new Vector2(0.02216312f, 0.2943262f), new Vector2(0.9742908f, 0.2943262f), new Vector2(0.9742908f, 0.7712766f), new Vector2(0.02216312f, 0.7712766f), new Vector2(0.02216312f, 0.2943262f) }, new Vector3[] { new Vector2(0.9193262f, 0.535461f), new Vector2(0.7597518f, 0.3794326f), new Vector2(0.6462766f, 0.3794326f), new Vector2(0.7544326f, 0.4840426f), new Vector2(0.5735816f, 0.4840426f), new Vector2(0.5735816f, 0.5744681f), new Vector2(0.7544326f, 0.5744681f), new Vector2(0.6462766f, 0.6826241f), new Vector2(0.7579787f, 0.6826241f), new Vector2(0.9193262f, 0.535461f) }, new Vector3[] { new Vector2(0.07358156f, 0.2960993f), new Vector2(0.1072695f, 0.3297872f), new Vector2(0.1072695f, 0.3687943f) }, new Vector3[] { new Vector2(0.2650709f, 0.2960993f), new Vector2(0.304078f, 0.3333333f), new Vector2(0.3430851f, 0.3333333f), new Vector2(0.3058511f, 0.2978723f) } }; } return icon_emergencyExit; }
        Vector3[][] Icon_sun() { if (icon_sun == null) { icon_sun = new Vector3[][] { new Vector3[] { new Vector2(0.501125f, 0.2987834f), new Vector2(0.4220634f, 0.3145957f), new Vector2(0.3631265f, 0.3526891f), new Vector2(0.3207207f, 0.4123447f), new Vector2(0.3013147f, 0.495f), new Vector2(0.3156896f, 0.5726242f), new Vector2(0.3652828f, 0.6444983f), new Vector2(0.4249384f, 0.6825917f), new Vector2(0.504f, 0.6962478f), new Vector2(0.5895303f, 0.6768418f), new Vector2(0.649186f, 0.6308423f), new Vector2(0.6872793f, 0.5690305f), new Vector2(0.6994979f, 0.495f), new Vector2(0.6815293f, 0.4130634f), new Vector2(0.6412798f, 0.3548453f), new Vector2(0.5787492f, 0.3153145f), new Vector2(0.501125f, 0.2987834f) }, new Vector3[] { new Vector2(0.503268f, 0.753268f), new Vector2(0.503268f, 0.9526144f) }, new Vector3[] { new Vector2(0.5f, 0.253268f), new Vector2(0.5f, 0.04411765f) }, new Vector3[] { new Vector2(0.75f, 0.5f), new Vector2(0.9493464f, 0.5f) }, new Vector3[] { new Vector2(0.2418301f, 0.5f), new Vector2(0.0375817f, 0.5f) }, new Vector3[] { new Vector2(0.6830065f, 0.6748366f), new Vector2(0.8218954f, 0.8235294f) }, new Vector3[] { new Vector2(0.3251634f, 0.6830065f), new Vector2(0.1781046f, 0.8284314f) }, new Vector3[] { new Vector2(0.6748366f, 0.3186274f), new Vector2(0.8218954f, 0.1781046f) }, new Vector3[] { new Vector2(0.3120915f, 0.3300654f), new Vector2(0.1683007f, 0.1813726f) } }; } return icon_sun; }
        Vector3[][] Icon_rain() { if (icon_rain == null) { icon_rain = new Vector3[][] { new Vector3[] { new Vector2(0.20626f, 0.6853933f), new Vector2(0.1966292f, 0.7817014f), new Vector2(0.2191011f, 0.858748f), new Vector2(0.2833066f, 0.9309791f), new Vector2(0.3523274f, 0.9582664f), new Vector2(0.4357945f, 0.964687f), new Vector2(0.5176565f, 0.9309791f), new Vector2(0.570626f, 0.8812199f), new Vector2(0.6011236f, 0.8250401f), new Vector2(0.6460674f, 0.8651685f), new Vector2(0.711878f, 0.8715891f), new Vector2(0.7889246f, 0.8378812f), new Vector2(0.8370786f, 0.7720706f), new Vector2(0.8483146f, 0.6966292f), new Vector2(0.8258427f, 0.6388443f), new Vector2(0.8804173f, 0.6276084f), new Vector2(0.9414125f, 0.576244f), new Vector2(0.9670947f, 0.5152488f), new Vector2(0.9622793f, 0.4478331f), new Vector2(0.9382023f, 0.399679f), new Vector2(0.8964687f, 0.364366f), new Vector2(0.8322632f, 0.3467095f), new Vector2(0.1629214f, 0.3467095f), new Vector2(0.1019262f, 0.3756019f), new Vector2(0.0505618f, 0.4365971f), new Vector2(0.03611557f, 0.5152488f), new Vector2(0.06019261f, 0.6035313f), new Vector2(0.1019262f, 0.6597111f), new Vector2(0.1532905f, 0.6869984f), new Vector2(0.20626f, 0.6853933f) }, new Vector3[] { new Vector2(0.2704655f, 0.3113965f), new Vector2(0.2271268f, 0.2680578f), new Vector2(0.2399679f, 0.2423756f), new Vector2(0.2720706f, 0.258427f), new Vector2(0.2704655f, 0.3113965f) }, new Vector3[] { new Vector2(0.2239165f, 0.2022472f), new Vector2(0.1853933f, 0.1589085f), new Vector2(0.1998395f, 0.1364366f), new Vector2(0.2287319f, 0.1492777f), new Vector2(0.2239165f, 0.2022472f) }, new Vector3[] { new Vector2(0.1789727f, 0.09149278f), new Vector2(0.1452648f, 0.05457464f), new Vector2(0.1532905f, 0.03210273f), new Vector2(0.1789727f, 0.04173355f), new Vector2(0.1789727f, 0.09149278f) }, new Vector3[] { new Vector2(0.5192617f, 0.3113965f), new Vector2(0.4807384f, 0.2616372f), new Vector2(0.4935794f, 0.2439807f), new Vector2(0.5224719f, 0.2520064f), new Vector2(0.5192617f, 0.3113965f) }, new Vector3[] { new Vector2(0.4775281f, 0.2038523f), new Vector2(0.4422151f, 0.1605136f), new Vector2(0.4518459f, 0.1348315f), new Vector2(0.4759229f, 0.1476725f), new Vector2(0.4775281f, 0.2038523f) }, new Vector3[] { new Vector2(0.4309791f, 0.09149278f), new Vector2(0.3924558f, 0.0529695f), new Vector2(0.405297f, 0.02728732f), new Vector2(0.429374f, 0.04012841f), new Vector2(0.4309791f, 0.09149278f) }, new Vector3[] { new Vector2(0.7696629f, 0.3146068f), new Vector2(0.7279294f, 0.2664526f), new Vector2(0.7439808f, 0.2407705f), new Vector2(0.7680578f, 0.2552167f), new Vector2(0.7696629f, 0.3146068f) }, new Vector3[] { new Vector2(0.7247191f, 0.2022472f), new Vector2(0.6845907f, 0.1589085f), new Vector2(0.6942215f, 0.1348315f), new Vector2(0.723114f, 0.1444623f), new Vector2(0.7247191f, 0.2022472f) }, new Vector3[] { new Vector2(0.676565f, 0.09309791f), new Vector2(0.6428571f, 0.05136437f), new Vector2(0.6508828f, 0.02889246f), new Vector2(0.6749599f, 0.04333868f), new Vector2(0.676565f, 0.09309791f) } }; } return icon_rain; }
        Vector3[][] Icon_wind() { if (icon_wind == null) { icon_wind = new Vector3[][] { new Vector3[] { new Vector2(0.0505618f, 0.4060995f), new Vector2(0.1099518f, 0.3900481f), new Vector2(0.1837881f, 0.3916533f), new Vector2(0.2704655f, 0.4109149f), new Vector2(0.3443018f, 0.4333868f), new Vector2(0.4036918f, 0.4446228f), new Vector2(0.4855538f, 0.4430177f), new Vector2(0.5722311f, 0.4044944f), new Vector2(0.6091493f, 0.3659711f), new Vector2(0.6155698f, 0.3274478f), new Vector2(0.605939f, 0.3081862f), new Vector2(0.5770466f, 0.2889245f), new Vector2(0.5321027f, 0.2857143f), new Vector2(0.5032102f, 0.2953451f), new Vector2(0.488764f, 0.3274478f) }, new Vector3[] { new Vector2(0.3394864f, 0.5008026f), new Vector2(0.3956661f, 0.5216693f), new Vector2(0.4775281f, 0.5232745f), new Vector2(0.5497592f, 0.5008026f), new Vector2(0.6878009f, 0.4654896f), new Vector2(0.8226324f, 0.4430177f), new Vector2(0.8980739f, 0.4478331f), new Vector2(0.9414125f, 0.4686998f), new Vector2(0.9574639f, 0.5008026f), new Vector2(0.9622793f, 0.5393258f), new Vector2(0.9494382f, 0.5634029f), new Vector2(0.9157304f, 0.5826645f), new Vector2(0.8820225f, 0.5826645f), new Vector2(0.8451043f, 0.5714286f), new Vector2(0.829053f, 0.5569823f), new Vector2(0.8210273f, 0.529695f), new Vector2(0.8258427f, 0.5040128f) }, new Vector3[] { new Vector2(0.2688603f, 0.5714286f), new Vector2(0.3619583f, 0.6051365f), new Vector2(0.4277689f, 0.6147673f), new Vector2(0.4903692f, 0.5971107f), new Vector2(0.5642055f, 0.5714286f), new Vector2(0.6348315f, 0.5730337f), new Vector2(0.7038524f, 0.5987159f), new Vector2(0.7455859f, 0.6340289f), new Vector2(0.7680578f, 0.6725522f), new Vector2(0.7616372f, 0.7030498f), new Vector2(0.7391653f, 0.7223114f), new Vector2(0.6974318f, 0.7287319f), new Vector2(0.665329f, 0.7191011f), new Vector2(0.6428571f, 0.6982344f), new Vector2(0.6396469f, 0.6741573f), new Vector2(0.6508828f, 0.6484751f) } }; } return icon_wind; }
        Vector3[][] Icon_snow() { if (icon_snow == null) { icon_snow = new Vector3[][] { new Vector3[] { new Vector2(0.060477f, 0.5025554f), new Vector2(0.9412266f, 0.5025554f) }, new Vector3[] { new Vector2(0.7180579f, 0.8892674f), new Vector2(0.2785349f, 0.1243612f) }, new Vector3[] { new Vector2(0.2836457f, 0.8858603f), new Vector2(0.7231687f, 0.1192504f) }, new Vector3[] { new Vector2(0.8270869f, 0.7597955f), new Vector2(0.6448041f, 0.7597955f), new Vector2(0.5562181f, 0.9131175f) }, new Vector3[] { new Vector2(0.1729131f, 0.7614992f), new Vector2(0.3551959f, 0.7614992f), new Vector2(0.4420784f, 0.9165247f) }, new Vector3[] { new Vector2(0.1149915f, 0.350937f), new Vector2(0.2018739f, 0.5042589f), new Vector2(0.1132879f, 0.6575809f) }, new Vector3[] { new Vector2(0.883305f, 0.6626917f), new Vector2(0.7964225f, 0.5042589f), new Vector2(0.8867121f, 0.3492334f) }, new Vector3[] { new Vector2(0.1780238f, 0.2487223f), new Vector2(0.3500852f, 0.2487223f), new Vector2(0.4454855f, 0.09028961f) }, new Vector3[] { new Vector2(0.8270869f, 0.2453152f), new Vector2(0.6516184f, 0.2453152f), new Vector2(0.5579216f, 0.09028961f) } }; } return icon_snow; }
        Vector3[][] Icon_lightning() { if (icon_lightning == null) { icon_lightning = new Vector3[][] { new Vector3[] { new Vector2(0.3731942f, 0.8764045f), new Vector2(0.5545747f, 0.9711075f), new Vector2(0.4390048f, 0.5136437f), new Vector2(0.7905297f, 0.682183f), new Vector2(0.5914928f, 0.200642f), new Vector2(0.6781701f, 0.2150883f), new Vector2(0.4903692f, 0.01605137f), new Vector2(0.4614767f, 0.2873194f), new Vector2(0.5128411f, 0.2279294f), new Vector2(0.5802568f, 0.505618f), new Vector2(0.2897271f, 0.3467095f), new Vector2(0.3731942f, 0.8764045f) } }; } return icon_lightning; }
        Vector3[][] Icon_fire() { if (icon_fire == null) { icon_fire = new Vector3[][] { new Vector3[] { new Vector2(0.3651685f, 0.04815409f), new Vector2(0.2865168f, 0.07223114f), new Vector2(0.2126806f, 0.1235955f), new Vector2(0.14687f, 0.2215088f), new Vector2(0.1227929f, 0.3370787f), new Vector2(0.1372392f, 0.4462279f), new Vector2(0.170947f, 0.5473515f), new Vector2(0.2303371f, 0.6292135f), new Vector2(0.3154093f, 0.7110754f), new Vector2(0.312199f, 0.6597111f), new Vector2(0.3330658f, 0.6131621f), new Vector2(0.3747994f, 0.5858748f), new Vector2(0.3715891f, 0.6789727f), new Vector2(0.4069021f, 0.7624398f), new Vector2(0.4759229f, 0.8410915f), new Vector2(0.5529695f, 0.8956661f), new Vector2(0.6621188f, 0.9598716f), new Vector2(0.6252006f, 0.8715891f), new Vector2(0.6187801f, 0.7800963f), new Vector2(0.6348315f, 0.7030498f), new Vector2(0.7070626f, 0.5842696f), new Vector2(0.7134832f, 0.6452649f), new Vector2(0.7504013f, 0.6837881f), new Vector2(0.7873194f, 0.7030498f), new Vector2(0.8386838f, 0.70626f), new Vector2(0.8049759f, 0.6484751f), new Vector2(0.7953451f, 0.5971107f), new Vector2(0.8033708f, 0.540931f), new Vector2(0.8338684f, 0.4815409f), new Vector2(0.8804173f, 0.4093098f), new Vector2(0.9044944f, 0.3386838f), new Vector2(0.9060995f, 0.2808989f), new Vector2(0.8916533f, 0.2263242f), new Vector2(0.8515249f, 0.1685393f), new Vector2(0.79374f, 0.1252006f), new Vector2(0.7391653f, 0.09149278f), new Vector2(0.6749599f, 0.06741573f), new Vector2(0.5995185f, 0.05136437f), new Vector2(0.6540931f, 0.1043339f), new Vector2(0.6781701f, 0.1573034f), new Vector2(0.6797753f, 0.2150883f), new Vector2(0.6573034f, 0.2712681f), new Vector2(0.6123595f, 0.3194222f), new Vector2(0.5609952f, 0.3595506f), new Vector2(0.5208668f, 0.4012841f), new Vector2(0.4919743f, 0.4526485f), new Vector2(0.4983949f, 0.4991974f), new Vector2(0.5160514f, 0.5505618f), new Vector2(0.4470305f, 0.4911717f), new Vector2(0.3924558f, 0.4253612f), new Vector2(0.3619583f, 0.3739968f), new Vector2(0.3491172f, 0.3146068f), new Vector2(0.3507223f, 0.2568218f), new Vector2(0.3715891f, 0.1958266f), new Vector2(0.334671f, 0.1910112f), new Vector2(0.2929374f, 0.1974318f), new Vector2(0.2608347f, 0.2327448f), new Vector2(0.2576244f, 0.1717496f), new Vector2(0.2736758f, 0.1284109f), new Vector2(0.312199f, 0.0882825f), new Vector2(0.3651685f, 0.04815409f) } }; } return icon_fire; }
        Vector3[][] Icon_unitSquare() { if (icon_unitSquare == null) { icon_unitSquare = new Vector3[][] { new Vector3[] { new Vector2(0.0f, 0.0f), new Vector2(1.0f, 0.0f), new Vector2(1.0f, 1.0f), new Vector2(0.0f, 1.0f), new Vector2(0.0f, 0.0f) } }; } return icon_unitSquare; }
        Vector3[][] Icon_unitSquareIncl1Right() { if (icon_unitSquareIncl1Right == null) { icon_unitSquareIncl1Right = new Vector3[][] { new Vector3[] { new Vector2(0.0f, 0.0f), new Vector2(2.0f, 0.0f), new Vector2(2.0f, 1.0f), new Vector2(0.0f, 1.0f), new Vector2(0.0f, 0.0f) } }; } return icon_unitSquareIncl1Right; }
        Vector3[][] Icon_unitSquareIncl2Right() { if (icon_unitSquareIncl2Right == null) { icon_unitSquareIncl2Right = new Vector3[][] { new Vector3[] { new Vector2(0.0f, 0.0f), new Vector2(3.0f, 0.0f), new Vector2(3.0f, 1.0f), new Vector2(0.0f, 1.0f), new Vector2(0.0f, 0.0f) } }; } return icon_unitSquareIncl2Right; }
        Vector3[][] Icon_unitSquareIncl3Right() { if (icon_unitSquareIncl3Right == null) { icon_unitSquareIncl3Right = new Vector3[][] { new Vector3[] { new Vector2(0.0f, 0.0f), new Vector2(4.0f, 0.0f), new Vector2(4.0f, 1.0f), new Vector2(0.0f, 1.0f), new Vector2(0.0f, 0.0f) } }; } return icon_unitSquareIncl3Right; }
        Vector3[][] Icon_unitSquareIncl4Right() { if (icon_unitSquareIncl4Right == null) { icon_unitSquareIncl4Right = new Vector3[][] { new Vector3[] { new Vector2(0.0f, 0.0f), new Vector2(5.0f, 0.0f), new Vector2(5.0f, 1.0f), new Vector2(0.0f, 1.0f), new Vector2(0.0f, 0.0f) } }; } return icon_unitSquareIncl4Right; }
        Vector3[][] Icon_unitSquareIncl5Right() { if (icon_unitSquareIncl5Right == null) { icon_unitSquareIncl5Right = new Vector3[][] { new Vector3[] { new Vector2(0.0f, 0.0f), new Vector2(6.0f, 0.0f), new Vector2(6.0f, 1.0f), new Vector2(0.0f, 1.0f), new Vector2(0.0f, 0.0f) } }; } return icon_unitSquareIncl5Right; }
        Vector3[][] Icon_unitSquareIncl6Right() { if (icon_unitSquareIncl6Right == null) { icon_unitSquareIncl6Right = new Vector3[][] { new Vector3[] { new Vector2(0.0f, 0.0f), new Vector2(7.0f, 0.0f), new Vector2(7.0f, 1.0f), new Vector2(0.0f, 1.0f), new Vector2(0.0f, 0.0f) } }; } return icon_unitSquareIncl6Right; }
        Vector3[][] Icon_unitSquareCrossed() { if (icon_unitSquareCrossed == null) { icon_unitSquareCrossed = new Vector3[][] { new Vector3[] { new Vector2(0.5f, 1.0f), new Vector2(0.5f, 0.0f) }, new Vector3[] { new Vector2(0.0f, 1.0f), new Vector2(0.0f, 0.0f) }, new Vector3[] { new Vector2(1.0f, 1.0f), new Vector2(1.0f, 0.0f) }, new Vector3[] { new Vector2(0.0f, 0.5f), new Vector2(1.0f, 0.5f) }, new Vector3[] { new Vector2(0.0f, 0.0f), new Vector2(1.0f, 0.0f) }, new Vector3[] { new Vector2(0.0f, 1.0f), new Vector2(1.0f, 1.0f) } }; } return icon_unitSquareCrossed; }
        Vector3[][] Icon_unitCircle() { if (icon_unitCircle == null) { icon_unitCircle = new Vector3[][] { new Vector3[] { new Vector2(0.5f, 0.0f), new Vector2(0.30366f, 0.03569004f), new Vector2(0.15524f, 0.13162f), new Vector2(0.04845002f, 0.2818501f), new Vector2(0.0f, 0.5f), new Vector2(0.03578001f, 0.6854801f), new Vector2(0.16067f, 0.86648f), new Vector2(0.3109f, 0.9624101f), new Vector2(0.5f, 1.0f), new Vector2(0.7253899f, 0.94793f), new Vector2(0.87562f, 0.83209f), new Vector2(0.97155f, 0.6764301f), new Vector2(1.0f, 0.5f), new Vector2(0.95707f, 0.2836601f), new Vector2(0.85571f, 0.1370501f), new Vector2(0.69824f, 0.03750008f), new Vector2(0.5f, 0.0f) } }; } return icon_unitCircle; }
        Vector3[][] Icon_animal() { if (icon_animal == null) { icon_animal = new Vector3[][] { new Vector3[] { new Vector2(0.3593156f, 0.4923954f), new Vector2(0.3403042f, 0.3117871f), new Vector2(0.2642586f, 0.1977186f), new Vector2(0.1901141f, 0.1977186f), new Vector2(0.1920152f, 0.2338403f), new Vector2(0.2243346f, 0.2395437f), new Vector2(0.2794677f, 0.3307985f), new Vector2(0.2623574f, 0.4809886f), new Vector2(0.2224335f, 0.513308f), new Vector2(0.1882129f, 0.5665399f), new Vector2(0.161597f, 0.5931559f), new Vector2(0.07604562f, 0.581749f), new Vector2(0.05323194f, 0.6102661f), new Vector2(0.05893536f, 0.6634981f), new Vector2(0.1406844f, 0.6939163f), new Vector2(0.1463878f, 0.7224334f), new Vector2(0.1692015f, 0.7376426f), new Vector2(0.2053232f, 0.7414449f), new Vector2(0.2281369f, 0.7889734f), new Vector2(0.2585551f, 0.7870722f), new Vector2(0.2623574f, 0.7262357f), new Vector2(0.2851711f, 0.7110266f), new Vector2(0.2908745f, 0.6634981f), new Vector2(0.4144487f, 0.6235741f), new Vector2(0.539924f, 0.6159696f), new Vector2(0.621673f, 0.6406844f), new Vector2(0.6749049f, 0.6444867f), new Vector2(0.7357414f, 0.6273764f), new Vector2(0.7851711f, 0.581749f), new Vector2(0.7946768f, 0.5171103f), new Vector2(0.7832699f, 0.4562738f), new Vector2(0.7604563f, 0.404943f) }, new Vector3[] { new Vector2(0.5722433f, 0.4904943f), new Vector2(0.6235741f, 0.4068441f), new Vector2(0.6634981f, 0.3631179f), new Vector2(0.6463878f, 0.2414449f), new Vector2(0.6026616f, 0.230038f), new Vector2(0.608365f, 0.1863118f), new Vector2(0.6863118f, 0.1863118f), new Vector2(0.7376426f, 0.3479087f), new Vector2(0.7357414f, 0.4752852f) }, new Vector3[] { new Vector2(0.621673f, 0.4068441f), new Vector2(0.5475285f, 0.3859316f), new Vector2(0.4752852f, 0.391635f), new Vector2(0.3840304f, 0.4144487f), new Vector2(0.3935361f, 0.28327f), new Vector2(0.3612167f, 0.1977186f), new Vector2(0.2946768f, 0.2015209f), new Vector2(0.2984791f, 0.2357415f), new Vector2(0.3365019f, 0.2509506f), new Vector2(0.3460076f, 0.3365019f) }, new Vector3[] { new Vector2(0.7851711f, 0.5836502f), new Vector2(0.8079848f, 0.5779468f), new Vector2(0.838403f, 0.5418251f), new Vector2(0.8498099f, 0.4904943f), new Vector2(0.8612167f, 0.4220532f), new Vector2(0.8954372f, 0.3821293f), new Vector2(0.9334601f, 0.3669201f), new Vector2(0.9714829f, 0.3688213f), new Vector2(0.9277567f, 0.3346007f), new Vector2(0.8764259f, 0.3346007f), new Vector2(0.8422053f, 0.365019f), new Vector2(0.8193916f, 0.4296578f), new Vector2(0.8136882f, 0.486692f), new Vector2(0.8079848f, 0.5152091f), new Vector2(0.7927757f, 0.5323194f) }, new Vector3[] { new Vector2(0.7281369f, 0.3155894f), new Vector2(0.7642586f, 0.2509506f), new Vector2(0.7262357f, 0.2338403f), new Vector2(0.730038f, 0.1977186f), new Vector2(0.8060837f, 0.1996198f), new Vector2(0.8022814f, 0.3612167f), new Vector2(0.7813688f, 0.4486692f) }, new Vector3[] { new Vector2(0.07794677f, 0.5874525f), new Vector2(0.1064639f, 0.6102661f) }, new Vector3[] { new Vector2(0.1558935f, 0.7053232f), new Vector2(0.1577947f, 0.6787072f), new Vector2(0.1977186f, 0.6958175f), new Vector2(0.1558935f, 0.7053232f) } }; } return icon_animal; }
        Vector3[][] Icon_bird() { if (icon_bird == null) { icon_bird = new Vector3[][] { new Vector3[] { new Vector2(0.5722433f, 0.3897339f), new Vector2(0.5380228f, 0.4334601f), new Vector2(0.5304183f, 0.4581749f), new Vector2(0.5494297f, 0.4752852f), new Vector2(0.6387833f, 0.513308f), new Vector2(0.7262357f, 0.5304183f), new Vector2(0.8250951f, 0.526616f), new Vector2(0.8821293f, 0.5057034f), new Vector2(0.9676806f, 0.4467681f), new Vector2(0.8821293f, 0.5323194f), new Vector2(0.7984791f, 0.5798479f), new Vector2(0.7072243f, 0.5969582f), new Vector2(0.6311787f, 0.5893536f), new Vector2(0.5494297f, 0.5551331f), new Vector2(0.5323194f, 0.5779468f), new Vector2(0.5057034f, 0.5912548f), new Vector2(0.4676806f, 0.5931559f), new Vector2(0.4467681f, 0.5741445f), new Vector2(0.4334601f, 0.5513308f), new Vector2(0.3460076f, 0.5893536f), new Vector2(0.2528517f, 0.5931559f), new Vector2(0.1673004f, 0.5703422f), new Vector2(0.09315589f, 0.5285171f), new Vector2(0.03612167f, 0.460076f), new Vector2(0.1311787f, 0.5171103f), new Vector2(0.2205323f, 0.5342205f), new Vector2(0.3174905f, 0.5209125f), new Vector2(0.3954373f, 0.4923954f), new Vector2(0.4410646f, 0.4676806f), new Vector2(0.4562738f, 0.4391635f), new Vector2(0.4429658f, 0.4182509f), new Vector2(0.404943f, 0.3859316f), new Vector2(0.4695818f, 0.4144487f), new Vector2(0.5247148f, 0.4163498f), new Vector2(0.5722433f, 0.3897339f) }, new Vector3[] { new Vector2(0.473384f, 0.5076045f), new Vector2(0.4904943f, 0.4581749f), new Vector2(0.5114068f, 0.5076045f) } }; } return icon_bird; }
        Vector3[][] Icon_humanMale() { if (icon_humanMale == null) { icon_humanMale = new Vector3[][] { new Vector3[] { new Vector2(0.6440536f, 0.6448911f), new Vector2(0.6440536f, 0.3886097f), new Vector2(0.657454f, 0.3685092f), new Vector2(0.6943049f, 0.3685092f), new Vector2(0.7077052f, 0.3936349f), new Vector2(0.7077052f, 0.6917923f), new Vector2(0.697655f, 0.7185929f), new Vector2(0.6775544f, 0.7437186f), new Vector2(0.6524288f, 0.758794f), new Vector2(0.618928f, 0.7654941f), new Vector2(0.3659967f, 0.7654941f), new Vector2(0.3358459f, 0.7571189f), new Vector2(0.3107203f, 0.7353434f), new Vector2(0.2922948f, 0.7051926f), new Vector2(0.2906198f, 0.6683417f), new Vector2(0.2906198f, 0.3902847f), new Vector2(0.3056951f, 0.3701842f), new Vector2(0.3324958f, 0.3701842f), new Vector2(0.3525963f, 0.3902847f), new Vector2(0.3525963f, 0.6499162f), new Vector2(0.3877722f, 0.6666667f), new Vector2(0.3877722f, 0.04522613f), new Vector2(0.4061977f, 0.02345059f), new Vector2(0.4463986f, 0.02345059f), new Vector2(0.4648241f, 0.05527638f), new Vector2(0.4648241f, 0.3902847f), new Vector2(0.5301508f, 0.3902847f), new Vector2(0.5301508f, 0.04690117f), new Vector2(0.5469012f, 0.02680067f), new Vector2(0.5887772f, 0.02680067f), new Vector2(0.6072027f, 0.05025126f), new Vector2(0.6072027f, 0.6649916f), new Vector2(0.6440536f, 0.6448911f) }, new Vector3[] { new Vector2(0.499f, 0.7972193f), new Vector2(0.4394741f, 0.8219008f), new Vector2(0.4159542f, 0.8762f), new Vector2(0.438603f, 0.9331126f), new Vector2(0.4975482f, 0.9583747f), new Vector2(0.5544607f, 0.9357259f), new Vector2(0.5774f, 0.8759096f), new Vector2(0.5541703f, 0.8192875f), new Vector2(0.499f, 0.7972193f) } }; } return icon_humanMale; }
        Vector3[][] Icon_humanFemale() { if (icon_humanFemale == null) { icon_humanFemale = new Vector3[][] { new Vector3[] { new Vector2(0.7043551f, 0.4137353f), new Vector2(0.7294807f, 0.3919598f), new Vector2(0.7613065f, 0.4020101f), new Vector2(0.7680067f, 0.438861f), new Vector2(0.6792295f, 0.7018425f), new Vector2(0.6658291f, 0.7319933f), new Vector2(0.6340033f, 0.7554439f), new Vector2(0.5971524f, 0.7638191f), new Vector2(0.4011725f, 0.7638191f), new Vector2(0.3643216f, 0.7554439f), new Vector2(0.3324958f, 0.7303182f), new Vector2(0.3107203f, 0.6850922f), new Vector2(0.2319933f, 0.4321608f), new Vector2(0.2420436f, 0.4036851f), new Vector2(0.2621441f, 0.3969849f), new Vector2(0.298995f, 0.4170854f), new Vector2(0.3726968f, 0.6616415f), new Vector2(0.419598f, 0.6616415f), new Vector2(0.2906198f, 0.2546064f), new Vector2(0.3894472f, 0.2546064f), new Vector2(0.3894472f, 0.04355109f), new Vector2(0.4061977f, 0.02512563f), new Vector2(0.4463986f, 0.02512563f), new Vector2(0.4664991f, 0.05360134f), new Vector2(0.4664991f, 0.2546064f), new Vector2(0.5351759f, 0.2546064f), new Vector2(0.5351759f, 0.0519263f), new Vector2(0.5536013f, 0.02680067f), new Vector2(0.5871022f, 0.02680067f), new Vector2(0.6072027f, 0.0519263f), new Vector2(0.6072027f, 0.2529313f), new Vector2(0.7093802f, 0.2529313f), new Vector2(0.5837521f, 0.6566164f), new Vector2(0.6340033f, 0.6566164f), new Vector2(0.7043551f, 0.4137353f) }, new Vector3[] { new Vector2(0.499f, 0.7972193f), new Vector2(0.4394741f, 0.8219008f), new Vector2(0.4159542f, 0.8762f), new Vector2(0.438603f, 0.9331126f), new Vector2(0.4975482f, 0.9583747f), new Vector2(0.5544607f, 0.9357259f), new Vector2(0.5774f, 0.8759096f), new Vector2(0.5541703f, 0.8192875f), new Vector2(0.499f, 0.7972193f) } }; } return icon_humanFemale; }
        Vector3[][] Icon_bombExplosion() { if (icon_bombExplosion == null) { icon_bombExplosion = new Vector3[][] { new Vector3[] { new Vector2(0.7787418f, 0.4381779f), new Vector2(0.8980477f, 0.4316703f), new Vector2(0.9696313f, 0.5878525f), new Vector2(0.8655097f, 0.5140998f), new Vector2(0.9370933f, 0.7245119f), new Vector2(0.824295f, 0.6225597f), new Vector2(0.8264642f, 0.878525f), new Vector2(0.7006508f, 0.6789588f), new Vector2(0.6789588f, 0.8004338f), new Vector2(0.64859f, 0.6876356f), new Vector2(0.5704989f, 0.9414316f), new Vector2(0.5704989f, 0.6659436f), new Vector2(0.5032538f, 0.7353579f), new Vector2(0.505423f, 0.6334056f), new Vector2(0.3492408f, 0.7809111f), new Vector2(0.4099783f, 0.6073753f), new Vector2(0.1778742f, 0.7093276f), new Vector2(0.318872f, 0.5075922f), new Vector2(0.2039046f, 0.5401301f), new Vector2(0.2906725f, 0.4620391f), new Vector2(0.04555314f, 0.4750542f), new Vector2(0.2971801f, 0.3752711f), new Vector2(0.2017354f, 0.3383948f), new Vector2(0.2993492f, 0.308026f), new Vector2(0.1084599f, 0.2125814f), new Vector2(0.4251627f, 0.2060737f), new Vector2(0.362256f, 0.1193059f), new Vector2(0.4859002f, 0.1409978f), new Vector2(0.516269f, 0.2451193f), new Vector2(0.4121475f, 0.2798265f), new Vector2(0.494577f, 0.3167028f), new Vector2(0.4295011f, 0.3665944f), new Vector2(0.4707158f, 0.3687636f), new Vector2(0.4425163f, 0.4078091f), new Vector2(0.5032538f, 0.3926247f), new Vector2(0.5227765f, 0.4707158f), new Vector2(0.5553145f, 0.4425163f), new Vector2(0.5553145f, 0.4924078f), new Vector2(0.5878525f, 0.4446855f), new Vector2(0.5965293f, 0.4989154f), new Vector2(0.6182213f, 0.4859002f), new Vector2(0.6442516f, 0.5227765f), new Vector2(0.6659436f, 0.4815618f), new Vector2(0.7223427f, 0.5639913f), new Vector2(0.7288503f, 0.4360087f), new Vector2(0.791757f, 0.483731f), new Vector2(0.7787418f, 0.4381779f) }, new Vector3[] { new Vector2(0.8481562f, 0.2668113f), new Vector2(0.8047723f, 0.2841648f), new Vector2(0.8004338f, 0.3232104f), new Vector2(0.7830803f, 0.3600868f), new Vector2(0.7548807f, 0.3904555f), new Vector2(0.7288503f, 0.4121475f), new Vector2(0.7071583f, 0.3817787f), new Vector2(0.7006508f, 0.4251627f), new Vector2(0.6659436f, 0.4273319f), new Vector2(0.6572668f, 0.3839479f), new Vector2(0.6399133f, 0.4273319f), new Vector2(0.6138828f, 0.4229935f), new Vector2(0.6247289f, 0.3752711f), new Vector2(0.5856833f, 0.4078091f), new Vector2(0.5748373f, 0.3926247f), new Vector2(0.5878525f, 0.362256f), new Vector2(0.5466378f, 0.3600868f), new Vector2(0.5422993f, 0.3383948f), new Vector2(0.5639913f, 0.3232104f), new Vector2(0.527115f, 0.308026f), new Vector2(0.5336226f, 0.2689805f), new Vector2(0.5574837f, 0.2668113f), new Vector2(0.5422993f, 0.2299349f), new Vector2(0.5639913f, 0.197397f), new Vector2(0.5856833f, 0.1778742f), new Vector2(0.616052f, 0.1605206f), new Vector2(0.6442516f, 0.1518438f), new Vector2(0.6876356f, 0.1496746f), new Vector2(0.7245119f, 0.1583514f), new Vector2(0.7440347f, 0.1713666f), new Vector2(0.7592191f, 0.1930586f), new Vector2(0.8156182f, 0.1735358f), new Vector2(0.8481562f, 0.2668113f) }, new Vector3[] { new Vector2(0.8286334f, 0.2190889f), new Vector2(0.8763558f, 0.1930586f) }, new Vector3[] { new Vector2(0.8655097f, 0.1995662f), new Vector2(0.8720173f, 0.2407809f), new Vector2(0.8828633f, 0.2060737f), new Vector2(0.9045553f, 0.2299349f), new Vector2(0.8937093f, 0.2039046f), new Vector2(0.9197397f, 0.1887202f), new Vector2(0.8937093f, 0.1887202f), new Vector2(0.9067245f, 0.1561822f), new Vector2(0.878525f, 0.1800434f), new Vector2(0.8655097f, 0.1475054f), new Vector2(0.8611714f, 0.1735358f), new Vector2(0.8373102f, 0.1713666f), new Vector2(0.8655097f, 0.1995662f) } }; } return icon_bombExplosion; }
        Vector3[][] Icon_tower() { if (icon_tower == null) { icon_tower = new Vector3[][] { new Vector3[] { new Vector2(0.6792295f, 0.319933f), new Vector2(0.6792295f, 0.7001675f), new Vector2(0.7328308f, 0.8040201f), new Vector2(0.7328308f, 0.9698492f), new Vector2(0.6541039f, 0.9698492f), new Vector2(0.6541039f, 0.8844221f), new Vector2(0.5971524f, 0.8844221f), new Vector2(0.5971524f, 0.9698492f), new Vector2(0.5234506f, 0.9698492f), new Vector2(0.5234506f, 0.8844221f), new Vector2(0.4664991f, 0.8844221f), new Vector2(0.4664991f, 0.9698492f), new Vector2(0.3894472f, 0.9698492f), new Vector2(0.3894472f, 0.8844221f), new Vector2(0.3341708f, 0.8844221f), new Vector2(0.3341708f, 0.9698492f), new Vector2(0.258794f, 0.9698492f), new Vector2(0.258794f, 0.802345f), new Vector2(0.3174204f, 0.7085427f), new Vector2(0.3174204f, 0.319933f), new Vector2(0.2336683f, 0.1859296f), new Vector2(0.2336683f, 0.03685092f), new Vector2(0.4380234f, 0.03685092f), new Vector2(0.4380234f, 0.1859296f), new Vector2(0.4731993f, 0.2110553f), new Vector2(0.5351759f, 0.2110553f), new Vector2(0.5686767f, 0.1859296f), new Vector2(0.5686767f, 0.03685092f), new Vector2(0.7663317f, 0.03685092f), new Vector2(0.7663317f, 0.1859296f), new Vector2(0.6792295f, 0.319933f) }, new Vector3[] { new Vector2(0.4631491f, 0.6649916f), new Vector2(0.4631491f, 0.5159129f), new Vector2(0.3844221f, 0.5159129f), new Vector2(0.3844221f, 0.6633166f), new Vector2(0.4246231f, 0.6901172f), new Vector2(0.4631491f, 0.6649916f) }, new Vector3[] { new Vector2(0.6239531f, 0.6633166f), new Vector2(0.6239531f, 0.517588f), new Vector2(0.5452262f, 0.517588f), new Vector2(0.5452262f, 0.6633166f), new Vector2(0.5854272f, 0.6901172f), new Vector2(0.6239531f, 0.6633166f) } }; } return icon_tower; }
        Vector3[][] Icon_circleDotFilled() { if (icon_circleDotFilled == null) { icon_circleDotFilled = new Vector3[][] { new Vector3[] { new Vector2(0.4852f, 0.1091501f), new Vector2(0.3257f, 0.14105f), new Vector2(0.2068f, 0.2179f), new Vector2(0.12125f, 0.33825f), new Vector2(0.08209997f, 0.505f), new Vector2(0.1111f, 0.6616f), new Vector2(0.21115f, 0.8066f), new Vector2(0.3315f, 0.88345f), new Vector2(0.491f, 0.911f), new Vector2(0.66355f, 0.87185f), new Vector2(0.7839f, 0.77905f), new Vector2(0.86075f, 0.65435f), new Vector2(0.8854001f, 0.505f), new Vector2(0.84915f, 0.3397f), new Vector2(0.76795f, 0.22225f), new Vector2(0.6418f, 0.1425f), new Vector2(0.4852f, 0.1091501f) }, new Vector3[] { new Vector2(0.487665f, 0.269604f), new Vector2(0.3959525f, 0.288574f), new Vector2(0.327585f, 0.334273f), new Vector2(0.2783937f, 0.40584f), new Vector2(0.2558825f, 0.505f), new Vector2(0.2725575f, 0.598124f), new Vector2(0.3300862f, 0.68435f), new Vector2(0.3992875f, 0.730049f), new Vector2(0.491f, 0.7464319f), new Vector2(0.5902162f, 0.723151f), new Vector2(0.6594175f, 0.667967f), new Vector2(0.7036062f, 0.5938129f), new Vector2(0.71778f, 0.505f), new Vector2(0.6969362f, 0.406703f), new Vector2(0.6502463f, 0.3368599f), new Vector2(0.57771f, 0.289436f), new Vector2(0.487665f, 0.269604f) }, new Vector3[] { new Vector2(0.4874147f, 0.1863281f), new Vector2(0.3613202f, 0.211686f), new Vector2(0.2673224f, 0.272776f), new Vector2(0.1996899f, 0.368446f), new Vector2(0.1687393f, 0.501f), new Vector2(0.1916656f, 0.625486f), new Vector2(0.2707613f, 0.74075f), new Vector2(0.3659054f, 0.801841f), new Vector2(0.492f, 0.823741f), new Vector2(0.6284114f, 0.792619f), new Vector2(0.7235556f, 0.71885f), new Vector2(0.7843102f, 0.619723f), new Vector2(0.8037975f, 0.501f), new Vector2(0.7751396f, 0.369599f), new Vector2(0.710946f, 0.276234f), new Vector2(0.6112167f, 0.212839f), new Vector2(0.4874147f, 0.1863281f) }, new Vector3[] { new Vector2(0.4901f, 0.367462f), new Vector2(0.3861557f, 0.410537f), new Vector2(0.3450851f, 0.5053f), new Vector2(0.3846346f, 0.604624f), new Vector2(0.4875648f, 0.648712f), new Vector2(0.5869456f, 0.609185f), new Vector2(0.6270022f, 0.504793f), new Vector2(0.5864386f, 0.4059761f), new Vector2(0.4901f, 0.367462f) }, new Vector3[] { new Vector2(0.492f, 0.447932f), new Vector2(0.4475325f, 0.466328f), new Vector2(0.4299624f, 0.5068001f), new Vector2(0.4468817f, 0.54922f), new Vector2(0.4909154f, 0.568049f), new Vector2(0.5334307f, 0.551168f), new Vector2(0.550567f, 0.506584f), new Vector2(0.5332138f, 0.46438f), new Vector2(0.492f, 0.447932f) } }; } return icon_circleDotFilled; }
        Vector3[][] Icon_circleDotUnfilled() { if (icon_circleDotUnfilled == null) { icon_circleDotUnfilled = new Vector3[][] { new Vector3[] { new Vector2(0.4852f, 0.1091501f), new Vector2(0.3257f, 0.14105f), new Vector2(0.2068f, 0.2179f), new Vector2(0.12125f, 0.33825f), new Vector2(0.08209997f, 0.505f), new Vector2(0.1111f, 0.6616f), new Vector2(0.21115f, 0.8066f), new Vector2(0.3315f, 0.88345f), new Vector2(0.491f, 0.911f), new Vector2(0.66355f, 0.87185f), new Vector2(0.7839f, 0.77905f), new Vector2(0.86075f, 0.65435f), new Vector2(0.8854001f, 0.505f), new Vector2(0.84915f, 0.3397f), new Vector2(0.76795f, 0.22225f), new Vector2(0.6418f, 0.1425f), new Vector2(0.4852f, 0.1091501f) } }; } return icon_circleDotUnfilled; }
        Vector3[][] Icon_logMessage() { if (icon_logMessage == null) { icon_logMessage = new Vector3[][] { new Vector3[] { new Vector2(0.6321586f, 0.154185f), new Vector2(0.8303965f, 0.08370044f), new Vector2(0.8039647f, 0.284141f), new Vector2(0.8634361f, 0.3788546f), new Vector2(0.8986784f, 0.5330396f), new Vector2(0.8700441f, 0.6894273f), new Vector2(0.7643172f, 0.8281938f), new Vector2(0.6211454f, 0.9185022f), new Vector2(0.4625551f, 0.9427313f), new Vector2(0.2929516f, 0.8876652f), new Vector2(0.1519824f, 0.7599119f), new Vector2(0.09030837f, 0.6123348f), new Vector2(0.09030837f, 0.4537445f), new Vector2(0.1585903f, 0.3039648f), new Vector2(0.2863436f, 0.185022f), new Vector2(0.4140969f, 0.1299559f), new Vector2(0.5264317f, 0.123348f), new Vector2(0.6321586f, 0.154185f) }, new Vector3[] { new Vector2(0.4933921f, 0.7929515f), new Vector2(0.4361233f, 0.7555066f), new Vector2(0.4603524f, 0.4625551f), new Vector2(0.4955947f, 0.4449339f), new Vector2(0.5374449f, 0.4713656f), new Vector2(0.5550661f, 0.7621145f), new Vector2(0.4933921f, 0.7929515f) }, new Vector3[] { new Vector2(0.4713656f, 0.4118943f), new Vector2(0.4427313f, 0.3832599f), new Vector2(0.4427313f, 0.3281938f), new Vector2(0.4669603f, 0.2973568f), new Vector2(0.5154185f, 0.2973568f), new Vector2(0.5484582f, 0.3281938f), new Vector2(0.5484582f, 0.3876652f), new Vector2(0.5198238f, 0.4096916f), new Vector2(0.4713656f, 0.4118943f) } }; } return icon_logMessage; }
        Vector3[][] Icon_logMessageError() { if (icon_logMessageError == null) { icon_logMessageError = new Vector3[][] { new Vector3[] { new Vector2(0.326087f, 0.1404682f), new Vector2(0.6438127f, 0.1404682f), new Vector2(0.8645485f, 0.326087f), new Vector2(0.8645485f, 0.6488295f), new Vector2(0.6622074f, 0.8729097f), new Vector2(0.3361204f, 0.8729097f), new Vector2(0.1103679f, 0.6789297f), new Vector2(0.1103679f, 0.3444816f), new Vector2(0.326087f, 0.1404682f) }, new Vector3[] { new Vector2(0.4916388f, 0.7508361f), new Vector2(0.4230769f, 0.7090301f), new Vector2(0.4632107f, 0.4130435f), new Vector2(0.493311f, 0.3996656f), new Vector2(0.5217391f, 0.4197325f), new Vector2(0.5518395f, 0.7190635f), new Vector2(0.4916388f, 0.7508361f) }, new Vector3[] { new Vector2(0.496f, 0.2563437f), new Vector2(0.4610812f, 0.2699863f), new Vector2(0.447284f, 0.3f), new Vector2(0.4605702f, 0.3314583f), new Vector2(0.4951483f, 0.3454219f), new Vector2(0.5285341f, 0.3329028f), new Vector2(0.5419906f, 0.2998395f), new Vector2(0.5283638f, 0.2685418f), new Vector2(0.496f, 0.2563437f) } }; } return icon_logMessageError; }
        Vector3[][] Icon_logMessageException() { if (icon_logMessageException == null) { icon_logMessageException = new Vector3[][] { new Vector3[] { new Vector2(0.07929515f, 0.6696035f), new Vector2(0.07929515f, 0.345815f), new Vector2(0.2687225f, 0.1475771f), new Vector2(0.6916299f, 0.1475771f), new Vector2(0.9096916f, 0.3546256f), new Vector2(0.9096916f, 0.6563877f), new Vector2(0.7070485f, 0.8678414f), new Vector2(0.2819383f, 0.8678414f), new Vector2(0.07929515f, 0.6696035f) }, new Vector3[] { new Vector2(0.2753304f, 0.7202643f), new Vector2(0.3348018f, 0.7202643f), new Vector2(0.438326f, 0.5506608f), new Vector2(0.5440528f, 0.7202643f), new Vector2(0.6035242f, 0.7202643f), new Vector2(0.4669603f, 0.5132158f), new Vector2(0.6079295f, 0.2995595f), new Vector2(0.5418502f, 0.2995595f), new Vector2(0.4361233f, 0.4669603f), new Vector2(0.3259912f, 0.2995595f), new Vector2(0.2643172f, 0.2995595f), new Vector2(0.4052863f, 0.5154185f), new Vector2(0.2753304f, 0.7202643f) }, new Vector3[] { new Vector2(0.6806167f, 0.7202643f), new Vector2(0.6806167f, 0.4427313f), new Vector2(0.7246696f, 0.4427313f), new Vector2(0.7246696f, 0.7180617f), new Vector2(0.6806167f, 0.7202643f) }, new Vector3[] { new Vector2(0.7048458f, 0.3612335f), new Vector2(0.6806167f, 0.3524229f), new Vector2(0.6674009f, 0.3237886f), new Vector2(0.6784141f, 0.2995595f), new Vector2(0.7048458f, 0.2885463f), new Vector2(0.7268723f, 0.2995595f), new Vector2(0.7378855f, 0.3259912f), new Vector2(0.7246696f, 0.3524229f), new Vector2(0.7048458f, 0.3612335f) } }; } return icon_logMessageException; }
        Vector3[][] Icon_logMessageAssertion() { if (icon_logMessageAssertion == null) { icon_logMessageAssertion = new Vector3[][] { new Vector3[] { new Vector2(0.07929515f, 0.6696035f), new Vector2(0.07929515f, 0.345815f), new Vector2(0.2687225f, 0.1475771f), new Vector2(0.6916299f, 0.1475771f), new Vector2(0.9096916f, 0.3546256f), new Vector2(0.9096916f, 0.6563877f), new Vector2(0.7070485f, 0.8678414f), new Vector2(0.2819383f, 0.8678414f), new Vector2(0.07929515f, 0.6696035f) }, new Vector3[] { new Vector2(0.2253045f, 0.3058187f), new Vector2(0.2740189f, 0.3058187f), new Vector2(0.3173207f, 0.4154263f), new Vector2(0.5094723f, 0.4154263f), new Vector2(0.5581867f, 0.3058187f), new Vector2(0.6109608f, 0.3058187f), new Vector2(0.4458728f, 0.7280108f), new Vector2(0.3890392f, 0.7280108f), new Vector2(0.2253045f, 0.3058187f) }, new Vector3[] { new Vector2(0.4161029f, 0.6671177f), new Vector2(0.3403248f, 0.4654939f), new Vector2(0.4945873f, 0.4654939f), new Vector2(0.4161029f, 0.6671177f) }, new Vector3[] { new Vector2(0.6705007f, 0.7307172f), new Vector2(0.6705007f, 0.4519621f), new Vector2(0.7124493f, 0.4519621f), new Vector2(0.7124493f, 0.7320704f), new Vector2(0.6705007f, 0.7307172f) }, new Vector3[] { new Vector2(0.6894452f, 0.3707713f), new Vector2(0.6650879f, 0.3599459f), new Vector2(0.6529093f, 0.3328823f), new Vector2(0.6650879f, 0.308525f), new Vector2(0.6935048f, 0.2976996f), new Vector2(0.7151556f, 0.3098782f), new Vector2(0.7286874f, 0.3342355f), new Vector2(0.7124493f, 0.3626522f), new Vector2(0.6894452f, 0.3707713f) } }; } return icon_logMessageAssertion; }
        Vector3[][] Icon_up_oneStroke() { if (icon_up_oneStroke == null) { icon_up_oneStroke = new Vector3[][] { new Vector3[] { new Vector2(0.0945946f, 0.3260135f), new Vector2(0.4983108f, 0.722973f), new Vector2(0.8918919f, 0.3310811f) } }; } return icon_up_oneStroke; }
        Vector3[][] Icon_up_twoStroke() { if (icon_up_twoStroke == null) { icon_up_twoStroke = new Vector3[][] { new Vector3[] { new Vector2(0.0945946f, 0.4746622f), new Vector2(0.4966216f, 0.8682432f), new Vector2(0.8935811f, 0.4814189f) }, new Vector3[] { new Vector2(0.0945946f, 0.1452703f), new Vector2(0.5f, 0.5405405f), new Vector2(0.8935811f, 0.1554054f) } }; } return icon_up_twoStroke; }
        Vector3[][] Icon_up_threeStroke() { if (icon_up_threeStroke == null) { icon_up_threeStroke = new Vector3[][] { new Vector3[] { new Vector2(0.0929054f, 0.5625f), new Vector2(0.4932432f, 0.9543919f), new Vector2(0.8902027f, 0.5692568f) }, new Vector3[] { new Vector2(0.0945946f, 0.3023649f), new Vector2(0.4966216f, 0.6959459f), new Vector2(0.8902027f, 0.3125f) }, new Vector3[] { new Vector2(0.0929054f, 0.04391892f), new Vector2(0.4949324f, 0.4324324f), new Vector2(0.8885135f, 0.05236486f) } }; } return icon_up_threeStroke; }
        Vector3[][] Icon_down_oneStroke() { if (icon_down_oneStroke == null) { icon_down_oneStroke = new Vector3[][] { new Vector3[] { new Vector2(0.9054054f, 0.6739866f), new Vector2(0.5016892f, 0.277027f), new Vector2(0.1081081f, 0.6689188f) } }; } return icon_down_oneStroke; }
        Vector3[][] Icon_down_twoStroke() { if (icon_down_twoStroke == null) { icon_down_twoStroke = new Vector3[][] { new Vector3[] { new Vector2(0.9054054f, 0.5253378f), new Vector2(0.5033785f, 0.1317568f), new Vector2(0.1064189f, 0.518581f) }, new Vector3[] { new Vector2(0.9054054f, 0.8547298f), new Vector2(0.5f, 0.4594595f), new Vector2(0.1064189f, 0.8445946f) } }; } return icon_down_twoStroke; }
        Vector3[][] Icon_down_threeStroke() { if (icon_down_threeStroke == null) { icon_down_threeStroke = new Vector3[][] { new Vector3[] { new Vector2(0.9070946f, 0.4375f), new Vector2(0.5067568f, 0.0456081f), new Vector2(0.1097973f, 0.4307432f) }, new Vector3[] { new Vector2(0.9054054f, 0.6976352f), new Vector2(0.5033784f, 0.3040541f), new Vector2(0.1097973f, 0.6875f) }, new Vector3[] { new Vector2(0.9070946f, 0.9560812f), new Vector2(0.5050676f, 0.5675676f), new Vector2(0.1114865f, 0.9476351f) } }; } return icon_down_threeStroke; }
        Vector3[][] Icon_left_oneStroke() { if (icon_left_oneStroke == null) { icon_left_oneStroke = new Vector3[][] { new Vector3[] { new Vector2(0.6739864f, 0.0945946f), new Vector2(0.277027f, 0.4983108f), new Vector2(0.6689189f, 0.8918918f) } }; } return icon_left_oneStroke; }
        Vector3[][] Icon_left_twoStroke() { if (icon_left_twoStroke == null) { icon_left_twoStroke = new Vector3[][] { new Vector3[] { new Vector2(0.5253378f, 0.09459463f), new Vector2(0.1317568f, 0.4966216f), new Vector2(0.5185811f, 0.893581f) }, new Vector3[] { new Vector2(0.8547297f, 0.0945946f), new Vector2(0.4594595f, 0.5f), new Vector2(0.8445946f, 0.893581f) } }; } return icon_left_twoStroke; }
        Vector3[][] Icon_left_threeStroke() { if (icon_left_threeStroke == null) { icon_left_threeStroke = new Vector3[][] { new Vector3[] { new Vector2(0.4375f, 0.09290543f), new Vector2(0.04560813f, 0.4932432f), new Vector2(0.4307432f, 0.8902026f) }, new Vector3[] { new Vector2(0.6976351f, 0.0945946f), new Vector2(0.3040541f, 0.4966216f), new Vector2(0.6875f, 0.8902026f) }, new Vector3[] { new Vector2(0.956081f, 0.0929054f), new Vector2(0.5675676f, 0.4949324f), new Vector2(0.9476352f, 0.8885134f) } }; } return icon_left_threeStroke; }
        Vector3[][] Icon_right_oneStroke() { if (icon_right_oneStroke == null) { icon_right_oneStroke = new Vector3[][] { new Vector3[] { new Vector2(0.3260135f, 0.9054054f), new Vector2(0.722973f, 0.5016892f), new Vector2(0.3310811f, 0.1081081f) } }; } return icon_right_oneStroke; }
        Vector3[][] Icon_right_twoStroke() { if (icon_right_twoStroke == null) { icon_right_twoStroke = new Vector3[][] { new Vector3[] { new Vector2(0.4746622f, 0.9054054f), new Vector2(0.8682432f, 0.5033784f), new Vector2(0.4814189f, 0.1064189f) }, new Vector3[] { new Vector2(0.1452703f, 0.9054053f), new Vector2(0.5405405f, 0.5f), new Vector2(0.1554054f, 0.1064189f) } }; } return icon_right_twoStroke; }
        Vector3[][] Icon_right_threeStroke() { if (icon_right_threeStroke == null) { icon_right_threeStroke = new Vector3[][] { new Vector3[] { new Vector2(0.5625f, 0.9070946f), new Vector2(0.9543918f, 0.5067568f), new Vector2(0.5692568f, 0.1097973f) }, new Vector3[] { new Vector2(0.3023649f, 0.9054054f), new Vector2(0.6959459f, 0.5033784f), new Vector2(0.3125f, 0.1097973f) }, new Vector3[] { new Vector2(0.04391891f, 0.9070945f), new Vector2(0.4324324f, 0.5050676f), new Vector2(0.05236492f, 0.1114865f) } }; } return icon_right_threeStroke; }
        Vector3[][] Icon_fist() { if (icon_fist == null) { icon_fist = new Vector3[][] { new Vector3[] { new Vector2(0.4832496f, 0.2830821f), new Vector2(0.4664991f, 0.3802345f), new Vector2(0.4061977f, 0.4824121f), new Vector2(0.3442211f, 0.520938f), new Vector2(0.2822446f, 0.5376884f), new Vector2(0.2822446f, 0.5762144f), new Vector2(0.3559464f, 0.5896147f), new Vector2(0.3944724f, 0.6130653f), new Vector2(0.4112228f, 0.6649916f), new Vector2(0.4078727f, 0.7068677f), new Vector2(0.3710218f, 0.721943f), new Vector2(0.1281407f, 0.7068677f), new Vector2(0.06951424f, 0.6331658f), new Vector2(0.04271357f, 0.4271357f), new Vector2(0.09463987f, 0.318258f), new Vector2(0.2721943f, 0.1809045f), new Vector2(0.2554439f, 0.03350084f), new Vector2(0.7160804f, 0.03350084f), new Vector2(0.6876047f, 0.2127303f), new Vector2(0.7730318f, 0.318258f), new Vector2(0.8232831f, 0.4036851f), new Vector2(0.8618091f, 0.4874372f), new Vector2(0.8785595f, 0.7906198f), new Vector2(0.8651592f, 0.8559464f), new Vector2(0.8333333f, 0.8927973f), new Vector2(0.7613065f, 0.8927973f), new Vector2(0.7462311f, 0.879397f), new Vector2(0.7328308f, 0.8291457f), new Vector2(0.7227806f, 0.6750419f), new Vector2(0.7361809f, 0.6415411f), new Vector2(0.7629816f, 0.6281407f), new Vector2(0.8115578f, 0.6348408f), new Vector2(0.8534338f, 0.6599665f), new Vector2(0.8735343f, 0.7118928f) }, new Vector3[] { new Vector2(0.3726968f, 0.721943f), new Vector2(0.3978224f, 0.9078727f), new Vector2(0.4313233f, 0.9497488f), new Vector2(0.4631491f, 0.9614741f), new Vector2(0.5385259f, 0.9564489f), new Vector2(0.5653266f, 0.9262981f), new Vector2(0.5703518f, 0.8944724f), new Vector2(0.5636516f, 0.7185929f), new Vector2(0.5502512f, 0.6499162f), new Vector2(0.5201005f, 0.6030151f), new Vector2(0.4782245f, 0.5812395f), new Vector2(0.4162479f, 0.5862647f), new Vector2(0.3911223f, 0.6130653f) }, new Vector3[] { new Vector2(0.2068677f, 0.7118928f), new Vector2(0.220268f, 0.839196f), new Vector2(0.2319933f, 0.8827471f), new Vector2(0.2839196f, 0.9128978f), new Vector2(0.3274707f, 0.9229481f), new Vector2(0.3643216f, 0.9128978f), new Vector2(0.3927973f, 0.8760469f) }, new Vector3[] { new Vector2(0.5703518f, 0.8676717f), new Vector2(0.5938023f, 0.9078727f), new Vector2(0.6273032f, 0.9380234f), new Vector2(0.6792295f, 0.9380234f), new Vector2(0.7211055f, 0.9128978f), new Vector2(0.7361809f, 0.8492462f) }, new Vector3[] { new Vector2(0.5519263f, 0.6566164f), new Vector2(0.5603015f, 0.6214405f), new Vector2(0.5837521f, 0.5963149f), new Vector2(0.6557789f, 0.599665f), new Vector2(0.6943049f, 0.6281407f), new Vector2(0.7227806f, 0.6817421f) } }; } return icon_fist; }
        Vector3[][] Icon_boxingGlove() { if (icon_boxingGlove == null) { icon_boxingGlove = new Vector3[][] { new Vector3[] { new Vector2(0.2688442f, 0.6867672f), new Vector2(0.2688442f, 0.3031826f), new Vector2(0.2453936f, 0.2931323f), new Vector2(0.1582915f, 0.2830821f), new Vector2(0.07286432f, 0.2914573f), new Vector2(0.04438861f, 0.3149079f), new Vector2(0.04438861f, 0.6884422f), new Vector2(0.06281407f, 0.7169179f), new Vector2(0.1566164f, 0.7269682f), new Vector2(0.2554439f, 0.7102178f), new Vector2(0.2688442f, 0.6867672f) }, new Vector3[] { new Vector2(0.0879397f, 0.2914573f), new Vector2(0.0879397f, 0.5778894f), new Vector2(0.2236181f, 0.5778894f), new Vector2(0.2236181f, 0.2914573f) }, new Vector3[] { new Vector2(0.4296483f, 0.6750419f), new Vector2(0.5653266f, 0.7487437f), new Vector2(0.6407035f, 0.7755444f), new Vector2(0.778057f, 0.7772194f), new Vector2(0.8551089f, 0.7504188f), new Vector2(0.9103853f, 0.7102178f), new Vector2(0.9321608f, 0.6683417f), new Vector2(0.9472362f, 0.5829146f), new Vector2(0.9472362f, 0.4623116f), new Vector2(0.9371859f, 0.361809f), new Vector2(0.9154104f, 0.3082077f), new Vector2(0.8718593f, 0.2596315f), new Vector2(0.8115578f, 0.2294807f), new Vector2(0.7345059f, 0.2177554f), new Vector2(0.4916248f, 0.2211055f), new Vector2(0.4246231f, 0.2311558f), new Vector2(0.3726968f, 0.2529313f), new Vector2(0.2705193f, 0.3316583f) }, new Vector3[] { new Vector2(0.2671692f, 0.6934673f), new Vector2(0.3375209f, 0.7755444f), new Vector2(0.4061977f, 0.8341709f), new Vector2(0.4916248f, 0.8592965f), new Vector2(0.5703518f, 0.8743719f), new Vector2(0.6038526f, 0.8693467f), new Vector2(0.6356784f, 0.840871f), new Vector2(0.6641541f, 0.7788945f) } }; } return icon_boxingGlove; }
        Vector3[][] Icon_stars5Rate() { if (icon_stars5Rate == null) { icon_stars5Rate = new Vector3[][] { new Vector3[] { new Vector2(0.379397f, 0.5259631f), new Vector2(0.4681742f, 0.5259631f), new Vector2(0.498325f, 0.6147404f), new Vector2(0.5251256f, 0.5242881f), new Vector2(0.6172529f, 0.5242881f), new Vector2(0.5385259f, 0.4723618f), new Vector2(0.5703518f, 0.3802345f), new Vector2(0.5f, 0.440536f), new Vector2(0.421273f, 0.3819095f), new Vector2(0.4497488f, 0.4740368f), new Vector2(0.379397f, 0.5259631f) }, new Vector3[] { new Vector2(0.6139029f, 0.4974874f), new Vector2(0.6825796f, 0.4974874f), new Vector2(0.7043551f, 0.559464f), new Vector2(0.7294807f, 0.4974874f), new Vector2(0.7931323f, 0.4974874f), new Vector2(0.741206f, 0.4539363f), new Vector2(0.7596315f, 0.3902847f), new Vector2(0.7026801f, 0.4338358f), new Vector2(0.6457286f, 0.3886097f), new Vector2(0.6675042f, 0.4606365f), new Vector2(0.6139029f, 0.4974874f) }, new Vector3[] { new Vector2(0.2051926f, 0.4974874f), new Vector2(0.2721943f, 0.4974874f), new Vector2(0.2956449f, 0.5628141f), new Vector2(0.3174204f, 0.4991625f), new Vector2(0.3760469f, 0.4991625f), new Vector2(0.3257957f, 0.4522613f), new Vector2(0.3509213f, 0.3835846f), new Vector2(0.2939698f, 0.4321608f), new Vector2(0.2319933f, 0.3902847f), new Vector2(0.2537688f, 0.4556114f), new Vector2(0.2051926f, 0.4974874f) }, new Vector3[] { new Vector2(0.03768844f, 0.479062f), new Vector2(0.09463987f, 0.479062f), new Vector2(0.1113903f, 0.5293132f), new Vector2(0.1281407f, 0.4757119f), new Vector2(0.1867672f, 0.4757119f), new Vector2(0.1365159f, 0.440536f), new Vector2(0.1582915f, 0.3835846f), new Vector2(0.1097152f, 0.4237856f), new Vector2(0.06448911f, 0.3886097f), new Vector2(0.07956449f, 0.4489112f), new Vector2(0.03768844f, 0.479062f) }, new Vector3[] { new Vector2(0.8115578f, 0.4773869f), new Vector2(0.8685092f, 0.4773869f), new Vector2(0.8852596f, 0.5276382f), new Vector2(0.9053602f, 0.4757119f), new Vector2(0.9606365f, 0.4757119f), new Vector2(0.9120603f, 0.4422111f), new Vector2(0.9304858f, 0.3835846f), new Vector2(0.8852596f, 0.4237856f), new Vector2(0.8366834f, 0.3819095f), new Vector2(0.8567839f, 0.4489112f), new Vector2(0.8115578f, 0.4773869f) } }; } return icon_stars5Rate; }
        Vector3[][] Icon_stars3() { if (icon_stars3 == null) { icon_stars3 = new Vector3[][] { new Vector3[] { new Vector2(0.4346734f, 0.4304858f), new Vector2(0.659129f, 0.479062f), new Vector2(0.5033501f, 0.3132328f), new Vector2(0.6172529f, 0.09715243f), new Vector2(0.4095477f, 0.2110553f), new Vector2(0.2504188f, 0.03350084f), new Vector2(0.2772194f, 0.2579564f), new Vector2(0.07118928f, 0.3517588f), new Vector2(0.298995f, 0.3969849f), new Vector2(0.3274707f, 0.6298158f), new Vector2(0.4346734f, 0.4304858f) }, new Vector3[] { new Vector2(0.7881072f, 0.7922948f), new Vector2(0.9572864f, 0.7805695f), new Vector2(0.8082077f, 0.7018425f), new Vector2(0.8467337f, 0.5276382f), new Vector2(0.7278057f, 0.6499162f), new Vector2(0.5854272f, 0.5628141f), new Vector2(0.6474037f, 0.7152429f), new Vector2(0.5217755f, 0.8190955f), new Vector2(0.6909547f, 0.7973199f), new Vector2(0.7562814f, 0.9547738f), new Vector2(0.7881072f, 0.7922948f) }, new Vector3[] { new Vector2(0.2169179f, 0.8676717f), new Vector2(0.3241206f, 0.9061977f), new Vector2(0.2537688f, 0.8107203f), new Vector2(0.3257957f, 0.7068677f), new Vector2(0.2102178f, 0.7554439f), new Vector2(0.139866f, 0.6532663f), new Vector2(0.141541f, 0.7772194f), new Vector2(0.02931323f, 0.8056951f), new Vector2(0.1432161f, 0.8458961f), new Vector2(0.1465662f, 0.9648241f), new Vector2(0.2169179f, 0.8676717f) } }; } return icon_stars3; }
        Vector3[][] Icon_shootingStar() { if (icon_shootingStar == null) { icon_shootingStar = new Vector3[][] { new Vector3[] { new Vector2(0.8433836f, 0.3232831f), new Vector2(0.9505863f, 0.3232831f), new Vector2(0.8584589f, 0.2596315f), new Vector2(0.8936349f, 0.1574539f), new Vector2(0.8098828f, 0.2278057f), new Vector2(0.7177554f, 0.160804f), new Vector2(0.7562814f, 0.2663317f), new Vector2(0.6641541f, 0.3333333f), new Vector2(0.778057f, 0.3333333f), new Vector2(0.8115578f, 0.4304858f), new Vector2(0.8433836f, 0.3232831f) }, new Vector3[] { new Vector2(0.04606365f, 0.5242881f), new Vector2(0.180067f, 0.5276382f), new Vector2(0.3040201f, 0.5125628f), new Vector2(0.3944724f, 0.4874372f), new Vector2(0.461474f, 0.4556114f) }, new Vector3[] { new Vector2(0.5251256f, 0.4254606f), new Vector2(0.622278f, 0.3551089f) }, new Vector3[] { new Vector2(0.05276382f, 0.6700168f), new Vector2(0.1348409f, 0.6666667f), new Vector2(0.2169179f, 0.6515913f) }, new Vector3[] { new Vector2(0.2922948f, 0.6331658f), new Vector2(0.379397f, 0.6046901f), new Vector2(0.4765494f, 0.5661641f), new Vector2(0.5569514f, 0.519263f), new Vector2(0.6407035f, 0.4656616f), new Vector2(0.6926298f, 0.4170854f) }, new Vector3[] { new Vector2(0.04941374f, 0.8291457f), new Vector2(0.1649916f, 0.8040201f), new Vector2(0.2688442f, 0.7688442f), new Vector2(0.3592965f, 0.7319933f), new Vector2(0.4648241f, 0.6767169f) }, new Vector3[] { new Vector2(0.5452262f, 0.6264657f), new Vector2(0.618928f, 0.5695142f), new Vector2(0.6859297f, 0.5041876f), new Vector2(0.7361809f, 0.4489112f) } }; } return icon_shootingStar; }
        Vector3[][] Icon_moonHalf() { if (icon_moonHalf == null) { icon_moonHalf = new Vector3[][] { new Vector3[] { new Vector2(0.5217755f, 0.9564489f), new Vector2(0.4631491f, 0.8643216f), new Vector2(0.4262982f, 0.7420436f), new Vector2(0.4279732f, 0.6180905f), new Vector2(0.4782245f, 0.5008375f), new Vector2(0.5653266f, 0.400335f), new Vector2(0.6758794f, 0.3383585f), new Vector2(0.7747068f, 0.318258f), new Vector2(0.8685092f, 0.3266332f), new Vector2(0.9438861f, 0.3500837f), new Vector2(0.8986599f, 0.2579564f), new Vector2(0.8283082f, 0.1775544f), new Vector2(0.741206f, 0.1122278f), new Vector2(0.6340033f, 0.0720268f), new Vector2(0.5167504f, 0.05360134f), new Vector2(0.3994975f, 0.06867672f), new Vector2(0.2939698f, 0.1155779f), new Vector2(0.1867672f, 0.1959799f), new Vector2(0.1197655f, 0.2948074f), new Vector2(0.07286432f, 0.4204355f), new Vector2(0.06951424f, 0.5561139f), new Vector2(0.09798995f, 0.6733668f), new Vector2(0.1616415f, 0.7822446f), new Vector2(0.2370184f, 0.8576214f), new Vector2(0.3358459f, 0.9162479f), new Vector2(0.4329983f, 0.9480737f), new Vector2(0.5217755f, 0.9564489f) } }; } return icon_moonHalf; }
        Vector3[][] Icon_moonFullPlanet() { if (icon_moonFullPlanet == null) { icon_moonFullPlanet = new Vector3[][] { new Vector3[] { new Vector2(0.50252f, 0.05774003f), new Vector2(0.32432f, 0.09338003f), new Vector2(0.19148f, 0.17924f), new Vector2(0.0959f, 0.3137001f), new Vector2(0.05215999f, 0.5f), new Vector2(0.08456001f, 0.6749601f), new Vector2(0.19634f, 0.83696f), new Vector2(0.3308f, 0.9228201f), new Vector2(0.509f, 0.9536f), new Vector2(0.70178f, 0.90986f), new Vector2(0.8362401f, 0.80618f), new Vector2(0.9221f, 0.6668601f), new Vector2(0.94964f, 0.5f), new Vector2(0.90914f, 0.3153201f), new Vector2(0.81842f, 0.1841001f), new Vector2(0.67748f, 0.09500006f), new Vector2(0.50252f, 0.05774003f) }, new Vector3[] { new Vector2(0.1888686f, 0.6806569f), new Vector2(0.1596715f, 0.689781f), new Vector2(0.1195255f, 0.620438f), new Vector2(0.1012774f, 0.5127738f), new Vector2(0.1177007f, 0.4251825f), new Vector2(0.1833942f, 0.4014598f), new Vector2(0.2144161f, 0.4251825f), new Vector2(0.1979927f, 0.4835767f), new Vector2(0.1779197f, 0.5291971f), new Vector2(0.1870438f, 0.5894161f), new Vector2(0.2144161f, 0.6459854f), new Vector2(0.1888686f, 0.6806569f) }, new Vector3[] { new Vector2(0.2855839f, 0.8193431f), new Vector2(0.3640511f, 0.8357664f), new Vector2(0.4078467f, 0.8740876f), new Vector2(0.4954379f, 0.859489f), new Vector2(0.5282847f, 0.8120438f), new Vector2(0.5209854f, 0.7427008f), new Vector2(0.4206204f, 0.6660584f), new Vector2(0.314781f, 0.6751825f), new Vector2(0.2709854f, 0.7390511f), new Vector2(0.2855839f, 0.8193431f) }, new Vector3[] { new Vector2(0.6122262f, 0.8010949f), new Vector2(0.5885037f, 0.7591241f), new Vector2(0.6104015f, 0.6879562f), new Vector2(0.6833941f, 0.6678832f), new Vector2(0.7363139f, 0.7080292f), new Vector2(0.7016423f, 0.7846715f), new Vector2(0.6122262f, 0.8010949f) }, new Vector3[] { new Vector2(0.7089416f, 0.6167883f), new Vector2(0.7673358f, 0.6350365f), new Vector2(0.8020073f, 0.6167883f), new Vector2(0.8275548f, 0.5255474f), new Vector2(0.7381387f, 0.4744526f), new Vector2(0.6742701f, 0.5346715f), new Vector2(0.6687956f, 0.5894161f), new Vector2(0.7089416f, 0.6167883f) }, new Vector3[] { new Vector2(0.7582117f, 0.3850365f), new Vector2(0.7509124f, 0.3430657f), new Vector2(0.7892336f, 0.3357664f), new Vector2(0.8038321f, 0.3576642f), new Vector2(0.794708f, 0.3959854f), new Vector2(0.7582117f, 0.3850365f) }, new Vector3[] { new Vector2(0.185219f, 0.3321168f), new Vector2(0.1797445f, 0.2956204f), new Vector2(0.2125912f, 0.2463504f), new Vector2(0.2509124f, 0.2463504f), new Vector2(0.2636861f, 0.2810219f), new Vector2(0.2417883f, 0.3229927f), new Vector2(0.2089416f, 0.3394161f), new Vector2(0.185219f, 0.3321168f) }, new Vector3[] { new Vector2(0.334854f, 0.2737226f), new Vector2(0.3622263f, 0.2372263f), new Vector2(0.4260949f, 0.2372263f), new Vector2(0.4571168f, 0.2682482f), new Vector2(0.4114963f, 0.2937956f), new Vector2(0.3567518f, 0.3010949f), new Vector2(0.334854f, 0.2737226f) } }; } return icon_moonFullPlanet; }
        Vector3[][] Icon_leftHandRule() { if (icon_leftHandRule == null) { icon_leftHandRule = new Vector3[][] { new Vector3[] { new Vector2(0.1080402f, 0.4773869f), new Vector2(0.2370184f, 0.5410385f), new Vector2(0.340871f, 0.8559464f), new Vector2(0.3090452f, 0.9514238f), new Vector2(0.3659967f, 0.9648241f), new Vector2(0.4061977f, 0.9530988f), new Vector2(0.4313233f, 0.9262981f), new Vector2(0.4413735f, 0.8927973f), new Vector2(0.4380234f, 0.7822446f), new Vector2(0.4279732f, 0.7353434f), new Vector2(0.4430486f, 0.6750419f), new Vector2(0.4514238f, 0.6281407f), new Vector2(0.4413735f, 0.5845896f), new Vector2(0.419598f, 0.5443886f), new Vector2(0.3877722f, 0.5092127f), new Vector2(0.3442211f, 0.4991625f) }, new Vector3[] { new Vector2(0.4279732f, 0.7319933f), new Vector2(0.5871022f, 0.7319933f), new Vector2(0.8366834f, 0.8040201f), new Vector2(0.8651592f, 0.7973199f), new Vector2(0.8835846f, 0.7772194f), new Vector2(0.8819095f, 0.7470687f), new Vector2(0.8618091f, 0.7169179f), new Vector2(0.6256281f, 0.639866f), new Vector2(0.9137353f, 0.5443886f), new Vector2(0.9321608f, 0.5125628f), new Vector2(0.9321608f, 0.4740368f), new Vector2(0.8986599f, 0.4522613f), new Vector2(0.8584589f, 0.4539363f), new Vector2(0.5720268f, 0.5410385f), new Vector2(0.5670017f, 0.5845896f) }, new Vector3[] { new Vector2(0.6541039f, 0.517588f), new Vector2(0.6775544f, 0.4840871f), new Vector2(0.6842546f, 0.4355109f), new Vector2(0.6641541f, 0.400335f), new Vector2(0.5720268f, 0.3869347f), new Vector2(0.4648241f, 0.3835846f), new Vector2(0.4246231f, 0.39866f), new Vector2(0.421273f, 0.4355109f), new Vector2(0.4396985f, 0.4589615f), new Vector2(0.4731993f, 0.479062f), new Vector2(0.5536013f, 0.480737f), new Vector2(0.6055276f, 0.4924623f) }, new Vector3[] { new Vector2(0.6658291f, 0.4053601f), new Vector2(0.6892797f, 0.3785595f), new Vector2(0.6892797f, 0.3400335f), new Vector2(0.6725293f, 0.3165829f), new Vector2(0.5703518f, 0.3015075f), new Vector2(0.5150754f, 0.2998325f), new Vector2(0.458124f, 0.3082077f), new Vector2(0.4497488f, 0.3450586f), new Vector2(0.4648241f, 0.3869347f) }, new Vector3[] { new Vector2(0.138191f, 0.04857621f), new Vector2(0.340871f, 0.2562814f), new Vector2(0.4463986f, 0.2579564f), new Vector2(0.5117253f, 0.3015075f) }, new Vector3[] { new Vector2(0.5435511f, 0.4857621f), new Vector2(0.5351759f, 0.5075377f), new Vector2(0.5569514f, 0.5326633f) }, new Vector3[] { new Vector2(0.4396985f, 0.4623116f), new Vector2(0.4798995f, 0.4606365f), new Vector2(0.4949749f, 0.438861f), new Vector2(0.4916248f, 0.4036851f), new Vector2(0.4798995f, 0.3886097f) }, new Vector3[] { new Vector2(0.4564489f, 0.358459f), new Vector2(0.5f, 0.360134f), new Vector2(0.5217755f, 0.3433836f), new Vector2(0.5217755f, 0.3165829f), new Vector2(0.5150754f, 0.3082077f) }, new Vector3[] { new Vector2(0.6943049f, 0.5041876f), new Vector2(0.6926298f, 0.5427136f) }, new Vector3[] { new Vector2(0.622278f, 0.6415411f), new Vector2(0.6088777f, 0.6817421f) }, new Vector3[] { new Vector2(0.7144054f, 0.6716918f), new Vector2(0.701005f, 0.7035176f) }, new Vector3[] { new Vector2(0.8115578f, 0.7018425f), new Vector2(0.7931323f, 0.7336683f) }, new Vector3[] { new Vector2(0.3358459f, 0.8643216f), new Vector2(0.3592965f, 0.8894472f), new Vector2(0.3458962f, 0.9564489f) }, new Vector3[] { new Vector2(0.7857143f, 0.4789916f), new Vector2(0.7857143f, 0.5126051f) }, new Vector3[] { new Vector2(0.5672269f, 0.5831933f), new Vector2(0.5722689f, 0.605042f), new Vector2(0.5907563f, 0.6252101f) }, new Vector3[] { new Vector2(0.905042f, 0.5478992f), new Vector2(0.9218487f, 0.5109244f), new Vector2(0.920168f, 0.4789916f), new Vector2(0.910084f, 0.4588235f) } }; } return icon_leftHandRule; }
        Vector3[][] Icon_rightHandRule() { if (icon_rightHandRule == null) { icon_rightHandRule = new Vector3[][] { new Vector3[] { new Vector2(0.8919598f, 0.4773869f), new Vector2(0.7629816f, 0.5410385f), new Vector2(0.659129f, 0.8559464f), new Vector2(0.6909548f, 0.9514238f), new Vector2(0.6340033f, 0.9648241f), new Vector2(0.5938023f, 0.9530988f), new Vector2(0.5686767f, 0.9262981f), new Vector2(0.5586265f, 0.8927973f), new Vector2(0.5619766f, 0.7822446f), new Vector2(0.5720268f, 0.7353434f), new Vector2(0.5569514f, 0.6750419f), new Vector2(0.5485762f, 0.6281407f), new Vector2(0.5586265f, 0.5845896f), new Vector2(0.580402f, 0.5443886f), new Vector2(0.6122278f, 0.5092127f), new Vector2(0.6557789f, 0.4991625f) }, new Vector3[] { new Vector2(0.5720268f, 0.7319933f), new Vector2(0.4128978f, 0.7319933f), new Vector2(0.1633166f, 0.8040201f), new Vector2(0.1348408f, 0.7973199f), new Vector2(0.1164154f, 0.7772194f), new Vector2(0.1180905f, 0.7470687f), new Vector2(0.1381909f, 0.7169179f), new Vector2(0.3743719f, 0.639866f), new Vector2(0.08626473f, 0.5443886f), new Vector2(0.06783921f, 0.5125628f), new Vector2(0.06783921f, 0.4740368f), new Vector2(0.1013401f, 0.4522613f), new Vector2(0.1415411f, 0.4539363f), new Vector2(0.4279732f, 0.5410385f), new Vector2(0.4329983f, 0.5845896f) }, new Vector3[] { new Vector2(0.3458961f, 0.517588f), new Vector2(0.3224456f, 0.4840871f), new Vector2(0.3157454f, 0.4355109f), new Vector2(0.3358459f, 0.400335f), new Vector2(0.4279732f, 0.3869347f), new Vector2(0.5351759f, 0.3835846f), new Vector2(0.5753769f, 0.39866f), new Vector2(0.578727f, 0.4355109f), new Vector2(0.5603015f, 0.4589615f), new Vector2(0.5268007f, 0.479062f), new Vector2(0.4463987f, 0.480737f), new Vector2(0.3944724f, 0.4924623f) }, new Vector3[] { new Vector2(0.3341709f, 0.4053601f), new Vector2(0.3107203f, 0.3785595f), new Vector2(0.3107203f, 0.3400335f), new Vector2(0.3274707f, 0.3165829f), new Vector2(0.4296482f, 0.3015075f), new Vector2(0.4849246f, 0.2998325f), new Vector2(0.541876f, 0.3082077f), new Vector2(0.5502512f, 0.3450586f), new Vector2(0.5351759f, 0.3869347f) }, new Vector3[] { new Vector2(0.861809f, 0.04857621f), new Vector2(0.659129f, 0.2562814f), new Vector2(0.5536014f, 0.2579564f), new Vector2(0.4882747f, 0.3015075f) }, new Vector3[] { new Vector2(0.4564489f, 0.4857621f), new Vector2(0.4648241f, 0.5075377f), new Vector2(0.4430486f, 0.5326633f) }, new Vector3[] { new Vector2(0.5603015f, 0.4623116f), new Vector2(0.5201005f, 0.4606365f), new Vector2(0.5050251f, 0.438861f), new Vector2(0.5083752f, 0.4036851f), new Vector2(0.5201005f, 0.3886097f) }, new Vector3[] { new Vector2(0.5435511f, 0.358459f), new Vector2(0.5f, 0.360134f), new Vector2(0.4782245f, 0.3433836f), new Vector2(0.4782245f, 0.3165829f), new Vector2(0.4849246f, 0.3082077f) }, new Vector3[] { new Vector2(0.3056951f, 0.5041876f), new Vector2(0.3073702f, 0.5427136f) }, new Vector3[] { new Vector2(0.377722f, 0.6415411f), new Vector2(0.3911223f, 0.6817421f) }, new Vector3[] { new Vector2(0.2855946f, 0.6716918f), new Vector2(0.298995f, 0.7035176f) }, new Vector3[] { new Vector2(0.1884422f, 0.7018425f), new Vector2(0.2068677f, 0.7336683f) }, new Vector3[] { new Vector2(0.6641541f, 0.8643216f), new Vector2(0.6407035f, 0.8894472f), new Vector2(0.6541038f, 0.9564489f) }, new Vector3[] { new Vector2(0.2142857f, 0.4789916f), new Vector2(0.2142857f, 0.5126051f) }, new Vector3[] { new Vector2(0.4327731f, 0.5831933f), new Vector2(0.4277311f, 0.605042f), new Vector2(0.4092437f, 0.6252101f) }, new Vector3[] { new Vector2(0.09495801f, 0.5478992f), new Vector2(0.07815129f, 0.5109244f), new Vector2(0.07983196f, 0.4789916f), new Vector2(0.08991599f, 0.4588235f) } }; } return icon_rightHandRule; }
        Vector3[][] Icon_megaphone() { if (icon_megaphone == null) { icon_megaphone = new Vector3[][] { new Vector3[] { new Vector2(0.7116182f, 0.8008299f), new Vector2(0.7116182f, 0.1950208f), new Vector2(0.6473029f, 0.2634855f), new Vector2(0.5643154f, 0.3340249f), new Vector2(0.4958506f, 0.3651452f), new Vector2(0.4273859f, 0.3796681f), new Vector2(0.1369295f, 0.3796681f), new Vector2(0.1369295f, 0.6244813f), new Vector2(0.4315353f, 0.6244813f), new Vector2(0.4917012f, 0.6369295f), new Vector2(0.560166f, 0.6659751f), new Vector2(0.6203319f, 0.7116182f), new Vector2(0.7116182f, 0.8008299f) }, new Vector3[] { new Vector2(0.1369295f, 0.6037344f), new Vector2(0.1058091f, 0.6016598f), new Vector2(0.06431536f, 0.5705394f), new Vector2(0.0373444f, 0.5290456f), new Vector2(0.0373444f, 0.473029f), new Vector2(0.05809129f, 0.4294606f), new Vector2(0.09751038f, 0.4024896f), new Vector2(0.1348548f, 0.4004149f) }, new Vector3[] { new Vector2(0.7116182f, 0.5809129f), new Vector2(0.7365145f, 0.5622407f), new Vector2(0.7551867f, 0.5394191f), new Vector2(0.7655602f, 0.4958506f), new Vector2(0.7551867f, 0.4543568f), new Vector2(0.7365145f, 0.4294606f), new Vector2(0.713693f, 0.4128631f) }, new Vector3[] { new Vector2(0.7987552f, 0.6639004f), new Vector2(0.8319502f, 0.626556f), new Vector2(0.8609958f, 0.5539419f), new Vector2(0.8672199f, 0.5020747f), new Vector2(0.8589212f, 0.4439834f), new Vector2(0.8319502f, 0.3900415f), new Vector2(0.7966805f, 0.3443983f) }, new Vector3[] { new Vector2(0.8692946f, 0.7323651f), new Vector2(0.9045643f, 0.6825726f), new Vector2(0.9377593f, 0.6120332f), new Vector2(0.9543568f, 0.5560166f), new Vector2(0.9585062f, 0.5f), new Vector2(0.9502075f, 0.4294606f), new Vector2(0.9273859f, 0.3651452f), new Vector2(0.8941908f, 0.3195021f), new Vector2(0.8589212f, 0.280083f) }, new Vector3[] { new Vector2(0.1804979f, 0.3775934f), new Vector2(0.2717842f, 0.1618257f), new Vector2(0.4128631f, 0.1618257f), new Vector2(0.3174274f, 0.3796681f) } }; } return icon_megaphone; }
        Vector3[][] Icon_arrowLeft() { if (icon_arrowLeft == null) { icon_arrowLeft = new Vector3[][] { new Vector3[] { new Vector2(0.5063869f, 0.3777372f), new Vector2(0.915146f, 0.3777373f), new Vector2(0.915146f, 0.6441606f), new Vector2(0.5082117f, 0.6441606f), new Vector2(0.5082117f, 0.8448905f), new Vector2(0.05930662f, 0.5f), new Vector2(0.5063869f, 0.1368613f), new Vector2(0.5063869f, 0.3777372f) } }; } return icon_arrowLeft; }
        Vector3[][] Icon_arrowRight() { if (icon_arrowRight == null) { icon_arrowRight = new Vector3[][] { new Vector3[] { new Vector2(0.4936131f, 0.6222628f), new Vector2(0.08485401f, 0.6222628f), new Vector2(0.08485401f, 0.3558394f), new Vector2(0.4917883f, 0.3558394f), new Vector2(0.4917883f, 0.1551095f), new Vector2(0.9406934f, 0.5f), new Vector2(0.4936131f, 0.8631387f), new Vector2(0.4936131f, 0.6222628f) } }; } return icon_arrowRight; }
        Vector3[][] Icon_arrowUp() { if (icon_arrowUp == null) { icon_arrowUp = new Vector3[][] { new Vector3[] { new Vector2(0.3777372f, 0.4936131f), new Vector2(0.3777372f, 0.08485404f), new Vector2(0.6441606f, 0.08485404f), new Vector2(0.6441606f, 0.4917883f), new Vector2(0.8448905f, 0.4917883f), new Vector2(0.5f, 0.9406934f), new Vector2(0.1368614f, 0.4936131f), new Vector2(0.3777372f, 0.4936131f) } }; } return icon_arrowUp; }
        Vector3[][] Icon_arrowDown() { if (icon_arrowDown == null) { icon_arrowDown = new Vector3[][] { new Vector3[] { new Vector2(0.6222628f, 0.5063869f), new Vector2(0.6222627f, 0.915146f), new Vector2(0.3558394f, 0.915146f), new Vector2(0.3558394f, 0.5082117f), new Vector2(0.1551095f, 0.5082117f), new Vector2(0.5f, 0.05930665f), new Vector2(0.8631387f, 0.5063869f), new Vector2(0.6222628f, 0.5063869f) } }; } return icon_arrowDown; }
        Vector3[][] Icon_healthBox() { if (icon_healthBox == null) { icon_healthBox = new Vector3[][] { new Vector3[] { new Vector2(0.05186722f, 0.3008299f), new Vector2(0.05186722f, 0.5290456f), new Vector2(0.2655602f, 0.8402489f), new Vector2(0.9502075f, 0.6825726f), new Vector2(0.9502075f, 0.466805f), new Vector2(0.7365145f, 0.1514523f), new Vector2(0.05186722f, 0.3008299f) }, new Vector3[] { new Vector2(0.7365145f, 0.153527f), new Vector2(0.7365145f, 0.373444f), new Vector2(0.9481328f, 0.6846473f) }, new Vector3[] { new Vector2(0.3319502f, 0.4087137f), new Vector2(0.3319502f, 0.2904564f), new Vector2(0.4502075f, 0.2614108f), new Vector2(0.4502075f, 0.3796681f), new Vector2(0.3319502f, 0.4087137f) }, new Vector3[] { new Vector2(0.7344398f, 0.373444f), new Vector2(0.05186722f, 0.5311204f) }, new Vector3[] { new Vector2(0.7365145f, 0.2551867f), new Vector2(0.9502075f, 0.560166f) }, new Vector3[] { new Vector2(0.9502075f, 0.5767635f), new Vector2(0.7344398f, 0.2697096f), new Vector2(0.4502075f, 0.3319502f) }, new Vector3[] { new Vector2(0.7385892f, 0.253112f), new Vector2(0.4502075f, 0.3174274f) }, new Vector3[] { new Vector2(0.3298755f, 0.3609959f), new Vector2(0.04979253f, 0.4211618f) }, new Vector3[] { new Vector2(0.3278008f, 0.346473f), new Vector2(0.04979253f, 0.406639f) }, new Vector3[] { new Vector2(0.2987552f, 0.6120332f), new Vector2(0.4170125f, 0.5892116f), new Vector2(0.373444f, 0.5186722f), new Vector2(0.4958506f, 0.4896266f), new Vector2(0.5394191f, 0.560166f), new Vector2(0.6493776f, 0.5373444f), new Vector2(0.6991701f, 0.6037344f), new Vector2(0.5892116f, 0.6327801f), new Vector2(0.6286307f, 0.6929461f), new Vector2(0.5062241f, 0.719917f), new Vector2(0.4647303f, 0.653527f), new Vector2(0.3485477f, 0.6825726f), new Vector2(0.2987552f, 0.6120332f) }, new Vector3[] { new Vector2(0.3879668f, 0.3672199f), new Vector2(0.3879668f, 0.3008299f) } }; } return icon_healthBox; }
        Vector3[][] Icon_iceIcicle() { if (icon_iceIcicle == null) { icon_iceIcicle = new Vector3[][] { new Vector3[] { new Vector2(0.0746888f, 0.9647303f), new Vector2(0.9377593f, 0.9647303f), new Vector2(0.8319502f, 0.9439834f), new Vector2(0.7780083f, 0.9190871f), new Vector2(0.7219917f, 0.8485477f), new Vector2(0.6825726f, 0.6286307f), new Vector2(0.6431535f, 0.8921162f), new Vector2(0.6037344f, 0.4294606f), new Vector2(0.5290456f, 0.8983402f), new Vector2(0.473029f, 0.5580913f), new Vector2(0.4211618f, 0.906639f), new Vector2(0.373444f, 0.373444f), new Vector2(0.3112033f, 0.8921162f), new Vector2(0.2780083f, 0.7302905f), new Vector2(0.2323651f, 0.9315352f), new Vector2(0.2095436f, 0.8526971f), new Vector2(0.1970954f, 0.9149377f), new Vector2(0.1659751f, 0.9439834f), new Vector2(0.0746888f, 0.9647303f) }, new Vector3[] { new Vector2(0.3755187f, 0.2219917f), new Vector2(0.4211618f, 0.093361f), new Vector2(0.4253112f, 0.06846473f), new Vector2(0.4190871f, 0.04564315f), new Vector2(0.4045643f, 0.02697095f), new Vector2(0.3775934f, 0.02282158f), new Vector2(0.3423237f, 0.0373444f), new Vector2(0.3257262f, 0.05809129f), new Vector2(0.3257262f, 0.08298755f), new Vector2(0.3340249f, 0.1182573f), new Vector2(0.3755187f, 0.2219917f) }, new Vector3[] { new Vector2(0.6037344f, 0.373444f), new Vector2(0.6493776f, 0.2489627f), new Vector2(0.6514523f, 0.2240664f), new Vector2(0.6431535f, 0.1970954f), new Vector2(0.6182573f, 0.1825726f), new Vector2(0.5809129f, 0.1825726f), new Vector2(0.560166f, 0.2074689f), new Vector2(0.5518672f, 0.2365145f), new Vector2(0.5643154f, 0.2697096f), new Vector2(0.6037344f, 0.373444f) } }; } return icon_iceIcicle; }
        Vector3[][] Icon_pickAxe() { if (icon_pickAxe == null) { icon_pickAxe = new Vector3[][] { new Vector3[] { new Vector2(0.7095436f, 0.8298755f), new Vector2(0.8215768f, 0.7074689f), new Vector2(0.7074689f, 0.593361f), new Vector2(0.5871369f, 0.7116182f), new Vector2(0.7095436f, 0.8298755f) }, new Vector3[] { new Vector2(0.8029045f, 0.686722f), new Vector2(0.8568465f, 0.6058092f), new Vector2(0.9024896f, 0.5228216f), new Vector2(0.9273859f, 0.4460581f), new Vector2(0.9419087f, 0.3443983f), new Vector2(0.8900415f, 0.4522822f), new Vector2(0.8319502f, 0.5394191f), new Vector2(0.7738589f, 0.6016598f), new Vector2(0.7406639f, 0.6286307f) }, new Vector3[] { new Vector2(0.6784232f, 0.8008299f), new Vector2(0.5975104f, 0.8651452f), new Vector2(0.5124481f, 0.9128631f), new Vector2(0.43361f, 0.9377593f), new Vector2(0.3381743f, 0.9502075f), new Vector2(0.4315353f, 0.9045643f), new Vector2(0.5041494f, 0.8589212f), new Vector2(0.56639f, 0.8049793f), new Vector2(0.6161826f, 0.7448133f) }, new Vector3[] { new Vector2(0.6182573f, 0.686722f), new Vector2(0.03941909f, 0.1016598f), new Vector2(0.1016598f, 0.03319502f), new Vector2(0.6804979f, 0.6182573f) }, new Vector3[] { new Vector2(0.7344398f, 0.8029045f), new Vector2(0.7572614f, 0.8257262f), new Vector2(0.8195021f, 0.7593361f), new Vector2(0.7946058f, 0.7385892f) } }; } return icon_pickAxe; }
        Vector3[][] Icon_audioSpeakerMute() { if (icon_audioSpeakerMute == null) { icon_audioSpeakerMute = new Vector3[][] { new Vector3[] { new Vector2(0.3284314f, 0.6405229f), new Vector2(0.06045752f, 0.6405229f), new Vector2(0.06045752f, 0.3562092f), new Vector2(0.3284314f, 0.3562092f), new Vector2(0.509804f, 0.1846405f), new Vector2(0.509804f, 0.8202614f), new Vector2(0.3284314f, 0.6405229f) }, new Vector3[] { new Vector2(0.6307189f, 0.6290849f), new Vector2(0.8921568f, 0.3627451f) }, new Vector3[] { new Vector2(0.6339869f, 0.3594771f), new Vector2(0.8970588f, 0.627451f) } }; } return icon_audioSpeakerMute; }
        Vector3[][] Icon_chestTreasureBox_open() { if (icon_chestTreasureBox_open == null) { icon_chestTreasureBox_open = new Vector3[][] { new Vector3[] { new Vector2(0.1276596f, 0.5567376f), new Vector2(0.1276596f, 0.214539f), new Vector2(0.6382979f, 0.04609929f), new Vector2(0.856383f, 0.2216312f), new Vector2(0.856383f, 0.5567376f), new Vector2(0.6382979f, 0.4521277f), new Vector2(0.1276596f, 0.5567376f), new Vector2(0.3900709f, 0.641844f), new Vector2(0.2180851f, 0.8723404f), new Vector2(0.7234042f, 0.8280142f), new Vector2(0.856383f, 0.5638298f) }, new Vector3[] { new Vector2(0.7234042f, 0.8262411f), new Vector2(0.7446808f, 0.8617021f), new Vector2(0.7677305f, 0.8865248f), new Vector2(0.7960993f, 0.8953901f), new Vector2(0.822695f, 0.891844f), new Vector2(0.856383f, 0.8705674f), new Vector2(0.8758865f, 0.8368794f), new Vector2(0.891844f, 0.7765958f), new Vector2(0.8953901f, 0.7109929f), new Vector2(0.8882979f, 0.641844f), new Vector2(0.8741135f, 0.5975177f), new Vector2(0.8546099f, 0.5602837f) }, new Vector3[] { new Vector2(0.8173759f, 0.8953901f), new Vector2(0.3368794f, 0.9343972f), new Vector2(0.2978723f, 0.9255319f), new Vector2(0.2659574f, 0.9095744f), new Vector2(0.2429078f, 0.893617f), new Vector2(0.2198582f, 0.8723404f) }, new Vector3[] { new Vector2(0.3368794f, 0.5460993f), new Vector2(0.3758865f, 0.5833333f), new Vector2(0.4166667f, 0.6117021f), new Vector2(0.4680851f, 0.6400709f), new Vector2(0.5265958f, 0.6507092f), new Vector2(0.5921986f, 0.6507092f), new Vector2(0.643617f, 0.6312057f), new Vector2(0.6861702f, 0.6046099f), new Vector2(0.7163121f, 0.569149f), new Vector2(0.7393617f, 0.5336879f) }, new Vector3[] { new Vector2(0.8546099f, 0.5638298f), new Vector2(0.7517731f, 0.5815603f) }, new Vector3[] { new Vector2(0.6382979f, 0.4539007f), new Vector2(0.6382979f, 0.04255319f) }, new Vector3[] { new Vector2(0.4503546f, 0.4893617f), new Vector2(0.4503546f, 0.108156f) }, new Vector3[] { new Vector2(0.2819149f, 0.5230497f), new Vector2(0.2819149f, 0.1595745f) }, new Vector3[] { new Vector2(0.6719858f, 0.6613475f), new Vector2(0.714539f, 0.7322695f) }, new Vector3[] { new Vector2(0.5762411f, 0.6879433f), new Vector2(0.5762411f, 0.7872341f) }, new Vector3[] { new Vector2(0.462766f, 0.6879433f), new Vector2(0.4237589f, 0.7677305f) } }; } return icon_chestTreasureBox_open; }
        Vector3[][] Icon_doorClosed() { if (icon_doorClosed == null) { icon_doorClosed = new Vector3[][] { new Vector3[] { new Vector2(0.129085f, 0.1830065f), new Vector2(0.8660131f, 0.1830065f) }, new Vector3[] { new Vector2(0.2957516f, 0.1846405f), new Vector2(0.2957516f, 0.8169935f), new Vector2(0.6960784f, 0.8169935f), new Vector2(0.6960784f, 0.1813726f) }, new Vector3[] { new Vector2(0.2630719f, 0.1846405f), new Vector2(0.2630719f, 0.8464052f), new Vector2(0.7271242f, 0.8464052f), new Vector2(0.7271242f, 0.1813726f) }, new Vector3[] { new Vector2(0.3349673f, 0.5212418f), new Vector2(0.3349673f, 0.4738562f) }, new Vector3[] { new Vector2(0.3366013f, 0.509804f), new Vector2(0.4101307f, 0.509804f), new Vector2(0.4101307f, 0.4885621f), new Vector2(0.3349673f, 0.4885621f) } }; } return icon_doorClosed; }

        Vector3[][] char_a;
        Vector3[][] char_b;
        Vector3[][] char_c;
        Vector3[][] char_d;
        Vector3[][] char_e;
        Vector3[][] char_f;
        Vector3[][] char_g;
        Vector3[][] char_h;
        Vector3[][] char_i;
        Vector3[][] char_j;
        Vector3[][] char_k;
        Vector3[][] char_l;
        Vector3[][] char_m;
        Vector3[][] char_n;
        Vector3[][] char_o;
        Vector3[][] char_p;
        Vector3[][] char_q;
        Vector3[][] char_r;
        Vector3[][] char_s;
        Vector3[][] char_t;
        Vector3[][] char_u;
        Vector3[][] char_v;
        Vector3[][] char_w;
        Vector3[][] char_x;
        Vector3[][] char_y;
        Vector3[][] char_z;
        Vector3[][] char_ae;
        Vector3[][] char_oe;
        Vector3[][] char_ue;

        Vector3[][] char_A;
        Vector3[][] char_B;
        Vector3[][] char_C;
        Vector3[][] char_D;
        Vector3[][] char_E;
        Vector3[][] char_F;
        Vector3[][] char_G;
        Vector3[][] char_H;
        Vector3[][] char_I;
        Vector3[][] char_J;
        Vector3[][] char_K;
        Vector3[][] char_L;
        Vector3[][] char_M;
        Vector3[][] char_N;
        Vector3[][] char_O;
        Vector3[][] char_P;
        Vector3[][] char_Q;
        Vector3[][] char_R;
        Vector3[][] char_S;
        Vector3[][] char_T;
        Vector3[][] char_U;
        Vector3[][] char_V;
        Vector3[][] char_W;
        Vector3[][] char_X;
        Vector3[][] char_Y;
        Vector3[][] char_Z;
        Vector3[][] char_AE;
        Vector3[][] char_OE;
        Vector3[][] char_UE;

        Vector3[][] char_0;
        Vector3[][] char_1;
        Vector3[][] char_2;
        Vector3[][] char_3;
        Vector3[][] char_4;
        Vector3[][] char_5;
        Vector3[][] char_6;
        Vector3[][] char_7;
        Vector3[][] char_8;
        Vector3[][] char_9;

        Vector3[][] char_space;
        Vector3[][] char_unknown;
        Vector3[][] char_dollar;
        Vector3[][] char_euro;
        Vector3[][] char_hashtag;
        Vector3[][] char_exclamationMark;
        Vector3[][] char_questionMark;
        Vector3[][] char_quote;
        Vector3[][] char_doublequote;
        Vector3[][] char_plus;
        Vector3[][] char_minus;
        Vector3[][] char_comma;
        Vector3[][] char_asterisk;
        Vector3[][] char_underscore;
        Vector3[][] char_period;
        Vector3[][] char_forwardslash;
        Vector3[][] char_backwardslash;
        Vector3[][] char_colon;
        Vector3[][] char_semicolon;
        Vector3[][] char_lessthan;
        Vector3[][] char_equals;
        Vector3[][] char_greaterthan;
        Vector3[][] char_percent;
        Vector3[][] char_ampersand;
        Vector3[][] char_openbracket;
        Vector3[][] char_closebracket;
        Vector3[][] char_opensquarebracket;
        Vector3[][] char_closesquarebracket;
        Vector3[][] char_leftbrace;
        Vector3[][] char_rightbrace;
        Vector3[][] char_verticalbar;
        Vector3[][] char_at;
        Vector3[][] char_caret;
        Vector3[][] char_tilde;
        Vector3[][] char_degree;
        Vector3[][] char_section;

        Vector3[][] icon_profileFoto;
        Vector3[][] icon_imageLandscape;
        Vector3[][] icon_homeHouse;
        Vector3[][] icon_dataDisc;
        Vector3[][] icon_saveData;
        Vector3[][] icon_loadData;
        Vector3[][] icon_speechBubble;
        Vector3[][] icon_speechBubbleEmpty;
        Vector3[][] icon_thumbUp;
        Vector3[][] icon_thumbDown;
        Vector3[][] icon_lightBulbOn;
        Vector3[][] icon_lightBulbOff;
        Vector3[][] icon_videoCamera;
        Vector3[][] icon_camera;
        Vector3[][] icon_music;
        Vector3[][] icon_audioSpeaker;
        Vector3[][] icon_microphone;
        Vector3[][] icon_wlan_wifi;
        Vector3[][] icon_share;
        Vector3[][] icon_timeClock;
        Vector3[][] icon_telephone;
        Vector3[][] icon_doorOpen;
        Vector3[][] icon_doorEnter;
        Vector3[][] icon_doorLeave;
        Vector3[][] icon_locationPin;
        Vector3[][] icon_folder;
        Vector3[][] icon_saveToFolder;
        Vector3[][] icon_loadFromFolder;
        Vector3[][] icon_optionsSettingsGear;
        Vector3[][] icon_adjustOptionsSettings;
        Vector3[][] icon_pen;
        Vector3[][] icon_questionMark;
        Vector3[][] icon_exclamationMark;
        Vector3[][] icon_shoppingCart;
        Vector3[][] icon_checkmarkChecked;
        Vector3[][] icon_checkmarkUnchecked;
        Vector3[][] icon_battery;
        Vector3[][] icon_cloud;
        Vector3[][] icon_magnifier;
        Vector3[][] icon_magnifierPlus;
        Vector3[][] icon_magnifierMinus;
        Vector3[][] icon_timeHourglassCursor;
        Vector3[][] icon_cursorHand;
        Vector3[][] icon_cursorPointer;
        Vector3[][] icon_trashcan;
        Vector3[][] icon_switchOnOff;
        Vector3[][] icon_playButton;
        Vector3[][] icon_pauseButton;
        Vector3[][] icon_stopButton;
        Vector3[][] icon_playPauseButton;
        Vector3[][] icon_heart;
        Vector3[][] icon_coin;
        Vector3[][] icon_coins;
        Vector3[][] icon_moneyBills;
        Vector3[][] icon_moneyBag;
        Vector3[][] icon_chestTreasureBox_closed;
        Vector3[][] icon_lootbox;
        Vector3[][] icon_crown;
        Vector3[][] icon_trophy;
        Vector3[][] icon_awardMedal;
        Vector3[][] icon_sword;
        Vector3[][] icon_shield;
        Vector3[][] icon_gun;
        Vector3[][] icon_bullet;
        Vector3[][] icon_rocket;
        Vector3[][] icon_crosshair;
        Vector3[][] icon_arrow;
        Vector3[][] icon_arrowBow;
        Vector3[][] icon_bomb;
        Vector3[][] icon_shovel;
        Vector3[][] icon_hammer;
        Vector3[][] icon_axe;
        Vector3[][] icon_magnet;
        Vector3[][] icon_compass;
        Vector3[][] icon_fuelStation;
        Vector3[][] icon_fuelCan;
        Vector3[][] icon_lockLocked;
        Vector3[][] icon_lockUnlocked;
        Vector3[][] icon_key;
        Vector3[][] icon_gemDiamond;
        Vector3[][] icon_gold;
        Vector3[][] icon_potion;
        Vector3[][] icon_presentGift;
        Vector3[][] icon_death;
        Vector3[][] icon_map;
        Vector3[][] icon_mushroom;
        Vector3[][] icon_star;
        Vector3[][] icon_pill;
        Vector3[][] icon_health;
        Vector3[][] icon_foodPlate;
        Vector3[][] icon_foodMeat;
        Vector3[][] icon_flag;
        Vector3[][] icon_flagChequered;
        Vector3[][] icon_ball;
        Vector3[][] icon_dice;
        Vector3[][] icon_joystick;
        Vector3[][] icon_gamepad;
        Vector3[][] icon_jigsawPuzzle;
        Vector3[][] icon_fish;
        Vector3[][] icon_car;
        Vector3[][] icon_tree;
        Vector3[][] icon_palm;
        Vector3[][] icon_leaf;
        Vector3[][] icon_nukeNuclearWarning;
        Vector3[][] icon_biohazardWarning;
        Vector3[][] icon_fireWarning;
        Vector3[][] icon_warning;
        Vector3[][] icon_emergencyExit;
        Vector3[][] icon_sun;
        Vector3[][] icon_rain;
        Vector3[][] icon_wind;
        Vector3[][] icon_snow;
        Vector3[][] icon_lightning;
        Vector3[][] icon_fire;
        Vector3[][] icon_unitSquare;
        Vector3[][] icon_unitSquareIncl1Right;
        Vector3[][] icon_unitSquareIncl2Right;
        Vector3[][] icon_unitSquareIncl3Right;
        Vector3[][] icon_unitSquareIncl4Right;
        Vector3[][] icon_unitSquareIncl5Right;
        Vector3[][] icon_unitSquareIncl6Right;
        Vector3[][] icon_unitSquareCrossed;
        Vector3[][] icon_unitCircle;
        Vector3[][] icon_animal;
        Vector3[][] icon_bird;
        Vector3[][] icon_humanMale;
        Vector3[][] icon_humanFemale;
        Vector3[][] icon_bombExplosion;
        Vector3[][] icon_tower;
        Vector3[][] icon_circleDotFilled;
        Vector3[][] icon_circleDotUnfilled;
        Vector3[][] icon_logMessage;
        Vector3[][] icon_logMessageError;
        Vector3[][] icon_logMessageException;
        Vector3[][] icon_logMessageAssertion;
        Vector3[][] icon_up_oneStroke;
        Vector3[][] icon_up_twoStroke;
        Vector3[][] icon_up_threeStroke;
        Vector3[][] icon_down_oneStroke;
        Vector3[][] icon_down_twoStroke;
        Vector3[][] icon_down_threeStroke;
        Vector3[][] icon_left_oneStroke;
        Vector3[][] icon_left_twoStroke;
        Vector3[][] icon_left_threeStroke;
        Vector3[][] icon_right_oneStroke;
        Vector3[][] icon_right_twoStroke;
        Vector3[][] icon_right_threeStroke;
        Vector3[][] icon_fist;
        Vector3[][] icon_boxingGlove;
        Vector3[][] icon_stars5Rate;
        Vector3[][] icon_stars3;
        Vector3[][] icon_shootingStar;
        Vector3[][] icon_moonHalf;
        Vector3[][] icon_moonFullPlanet;
        Vector3[][] icon_leftHandRule;
        Vector3[][] icon_rightHandRule;
        Vector3[][] icon_megaphone;
        Vector3[][] icon_arrowLeft;
        Vector3[][] icon_arrowRight;
        Vector3[][] icon_arrowUp;
        Vector3[][] icon_arrowDown;
        Vector3[][] icon_healthBox;
        Vector3[][] icon_iceIcicle;
        Vector3[][] icon_pickAxe;
        Vector3[][] icon_audioSpeakerMute;
        Vector3[][] icon_chestTreasureBox_open;
        Vector3[][] icon_doorClosed;

        public Vector3[][] GetPointsArray(char requestedChar, out bool charIsMissing)
        {
            charIsMissing = false;
            switch (requestedChar)
            {
                //small letters:
                case 'a':
                    return Char_a();

                case 'b':
                    return Char_b();

                case 'c':
                    return Char_c();

                case 'd':
                    return Char_d();

                case 'e':
                    return Char_e();

                case 'f':
                    return Char_f();

                case 'g':
                    return Char_g();

                case 'h':
                    return Char_h();

                case 'i':
                    return Char_i();

                case 'j':
                    return Char_j();

                case 'k':
                    return Char_k();

                case 'l':
                    return Char_l();

                case 'm':
                    return Char_m();

                case 'n':
                    return Char_n();

                case 'o':
                    return Char_o();

                case 'p':
                    return Char_p();

                case 'q':
                    return Char_q();

                case 'r':
                    return Char_r();

                case 's':
                    return Char_s();

                case 't':
                    return Char_t();

                case 'u':
                    return Char_u();

                case 'v':
                    return Char_v();

                case 'w':
                    return Char_w();

                case 'x':
                    return Char_x();

                case 'y':
                    return Char_y();

                case 'z':
                    return Char_z();

                case 'ä':
                    return Char_ae();

                case 'ö':
                    return Char_oe();

                case 'ü':
                    return Char_ue();

                //capital letters:
                case 'A':
                    return Char_A();

                case 'B':
                    return Char_B();

                case 'C':
                    return Char_C();

                case 'D':
                    return Char_D();

                case 'E':
                    return Char_E();

                case 'F':
                    return Char_F();

                case 'G':
                    return Char_G();

                case 'H':
                    return Char_H();

                case 'I':
                    return Char_I();

                case 'J':
                    return Char_J();

                case 'K':
                    return Char_K();

                case 'L':
                    return Char_L();

                case 'M':
                    return Char_M();

                case 'N':
                    return Char_N();

                case 'O':
                    return Char_O();

                case 'P':
                    return Char_P();

                case 'Q':
                    return Char_Q();

                case 'R':
                    return Char_R();

                case 'S':
                    return Char_S();

                case 'T':
                    return Char_T();

                case 'U':
                    return Char_U();

                case 'V':
                    return Char_V();

                case 'W':
                    return Char_W();

                case 'X':
                    return Char_X();

                case 'Y':
                    return Char_Y();

                case 'Z':
                    return Char_Z();

                case 'Ä':
                    return Char_AE();

                case 'Ö':
                    return Char_OE();

                case 'Ü':
                    return Char_UE();

                //numbers:
                case '0':
                    return Char_0();

                case '1':
                    return Char_1();

                case '2':
                    return Char_2();

                case '3':
                    return Char_3();

                case '4':
                    return Char_4();

                case '5':
                    return Char_5();

                case '6':
                    return Char_6();

                case '7':
                    return Char_7();

                case '8':
                    return Char_8();

                case '9':
                    return Char_9();

                //special chars:
                case ' ':
                    return Char_space();

                case '$':
                    return Char_dollar();

                case '€':
                    return Char_euro();

                case '#':
                    return Char_hashtag();

                case '!':
                    return Char_exclamationMark();

                case '?':
                    return Char_questionMark();

                case '\'':
                    return Char_quote();

                case '\"':
                    return Char_doublequote();

                case '+':
                    return Char_plus();

                case '-':
                    return Char_minus();

                case ',':
                    return Char_comma();

                case '*':
                    return Char_asterisk();

                case '_':
                    return Char_underscore();

                case '.':
                    return Char_period();

                case '/':
                    return Char_forwardslash();

                case '\\':
                    return Char_backwardslash();

                case ':':
                    return Char_colon();

                case ';':
                    return Char_semicolon();

                case '<':
                    return Char_lessthan();

                case '=':
                    return Char_equals();

                case '>':
                    return Char_greaterthan();

                case '%':
                    return Char_percent();

                case '&':
                    return Char_ampersand();

                case '(':
                    return Char_openbracket();

                case ')':
                    return Char_closebracket();

                case '[':
                    return Char_opensquarebracket();

                case ']':
                    return Char_closesquarebracket();

                case '{':
                    return Char_leftbrace();

                case '}':
                    return Char_rightbrace();

                case '|':
                    return Char_verticalbar();

                case '@':
                    return Char_at();

                case '^':
                    return Char_caret();

                case '~':
                    return Char_tilde();

                case '°':
                    return Char_degree();

                case '§':
                    return Char_section();

                default:
                    charIsMissing = true;
                    return Char_unknown();
            }
        }

        public Vector3[][] GetPointsArray(string iconName, out bool charIsMissing)
        {
            charIsMissing = false;
            switch (iconName)
            {
                case "profileFoto":
                    return Icon_profileFoto();

                case "imageLandscape":
                    return Icon_imageLandscape();

                case "homeHouse":
                    return Icon_homeHouse();

                case "dataDisc":
                    return Icon_dataDisc();

                case "saveData":
                    return Icon_saveData();

                case "loadData":
                    return Icon_loadData();

                case "speechBubble":
                    return Icon_speechBubble();

                case "speechBubbleEmpty":
                    return Icon_speechBubbleEmpty();

                case "thumbUp":
                    return Icon_thumbUp();

                case "thumbDown":
                    return Icon_thumbDown();

                case "lightBulbOn":
                    return Icon_lightBulbOn();

                case "lightBulbOff":
                    return Icon_lightBulbOff();

                case "videoCamera":
                    return Icon_videoCamera();

                case "camera":
                    return Icon_camera();

                case "music":
                    return Icon_music();

                case "audioSpeaker":
                    return Icon_audioSpeaker();

                case "microphone":
                    return Icon_microphone();

                case "wlan_wifi":
                    return Icon_wlan_wifi();

                case "share":
                    return Icon_share();

                case "timeClock":
                    return Icon_timeClock();

                case "telephone":
                    return Icon_telephone();

                case "doorOpen":
                    return Icon_doorOpen();

                case "doorEnter":
                    return Icon_doorEnter();

                case "doorLeave":
                    return Icon_doorLeave();

                case "locationPin":
                    return Icon_locationPin();

                case "folder":
                    return Icon_folder();

                case "saveToFolder":
                    return Icon_saveToFolder();

                case "loadFromFolder":
                    return Icon_loadFromFolder();

                case "optionsSettingsGear":
                    return Icon_optionsSettingsGear();

                case "adjustOptionsSettings":
                    return Icon_adjustOptionsSettings();

                case "pen":
                    return Icon_pen();

                case "questionMark":
                    return Icon_questionMark();

                case "exclamationMark":
                    return Icon_exclamationMark();

                case "shoppingCart":
                    return Icon_shoppingCart();

                case "checkmarkChecked":
                    return Icon_checkmarkChecked();

                case "checkmarkUnchecked":
                    return Icon_checkmarkUnchecked();

                case "battery":
                    return Icon_battery();

                case "cloud":
                    return Icon_cloud();

                case "magnifier":
                    return Icon_magnifier();

                case "magnifierPlus":
                    return Icon_magnifierPlus();

                case "magnifierMinus":
                    return Icon_magnifierMinus();

                case "timeHourglassCursor":
                    return Icon_timeHourglassCursor();

                case "cursorHand":
                    return Icon_cursorHand();

                case "cursorPointer":
                    return Icon_cursorPointer();

                case "trashcan":
                    return Icon_trashcan();

                case "switchOnOff":
                    return Icon_switchOnOff();

                case "playButton":
                    return Icon_playButton();

                case "pauseButton":
                    return Icon_pauseButton();

                case "stopButton":
                    return Icon_stopButton();

                case "playPauseButton":
                    return Icon_playPauseButton();

                case "heart":
                    return Icon_heart();

                case "coin":
                    return Icon_coin();

                case "coins":
                    return Icon_coins();

                case "moneyBills":
                    return Icon_moneyBills();

                case "moneyBag":
                    return Icon_moneyBag();

                case "chestTreasureBox_closed":
                    return Icon_chestTreasureBox_closed();

                case "lootbox":
                    return Icon_lootbox();

                case "crown":
                    return Icon_crown();

                case "trophy":
                    return Icon_trophy();

                case "awardMedal":
                    return Icon_awardMedal();

                case "sword":
                    return Icon_sword();

                case "shield":
                    return Icon_shield();

                case "gun":
                    return Icon_gun();

                case "bullet":
                    return Icon_bullet();

                case "rocket":
                    return Icon_rocket();

                case "crosshair":
                    return Icon_crosshair();

                case "arrow":
                    return Icon_arrow();

                case "arrowBow":
                    return Icon_arrowBow();

                case "bomb":
                    return Icon_bomb();

                case "shovel":
                    return Icon_shovel();

                case "hammer":
                    return Icon_hammer();

                case "axe":
                    return Icon_axe();

                case "magnet":
                    return Icon_magnet();

                case "compass":
                    return Icon_compass();

                case "fuelStation":
                    return Icon_fuelStation();

                case "fuelCan":
                    return Icon_fuelCan();

                case "lockLocked":
                    return Icon_lockLocked();

                case "lockUnlocked":
                    return Icon_lockUnlocked();

                case "key":
                    return Icon_key();

                case "gemDiamond":
                    return Icon_gemDiamond();

                case "gold":
                    return Icon_gold();

                case "potion":
                    return Icon_potion();

                case "presentGift":
                    return Icon_presentGift();

                case "death":
                    return Icon_death();

                case "map":
                    return Icon_map();

                case "mushroom":
                    return Icon_mushroom();

                case "star":
                    return Icon_star();

                case "pill":
                    return Icon_pill();

                case "health":
                    return Icon_health();

                case "foodPlate":
                    return Icon_foodPlate();

                case "foodMeat":
                    return Icon_foodMeat();

                case "flag":
                    return Icon_flag();

                case "flagChequered":
                    return Icon_flagChequered();

                case "ball":
                    return Icon_ball();

                case "dice":
                    return Icon_dice();

                case "joystick":
                    return Icon_joystick();

                case "gamepad":
                    return Icon_gamepad();

                case "jigsawPuzzle":
                    return Icon_jigsawPuzzle();

                case "fish":
                    return Icon_fish();

                case "car":
                    return Icon_car();

                case "tree":
                    return Icon_tree();

                case "palm":
                    return Icon_palm();

                case "leaf":
                    return Icon_leaf();

                case "nukeNuclearWarning":
                    return Icon_nukeNuclearWarning();

                case "biohazardWarning":
                    return Icon_biohazardWarning();

                case "fireWarning":
                    return Icon_fireWarning();

                case "warning":
                    return Icon_warning();

                case "emergencyExit":
                    return Icon_emergencyExit();

                case "sun":
                    return Icon_sun();

                case "rain":
                    return Icon_rain();

                case "wind":
                    return Icon_wind();

                case "snow":
                    return Icon_snow();

                case "lightning":
                    return Icon_lightning();

                case "fire":
                    return Icon_fire();

                case "unitSquare":
                    return Icon_unitSquare();

                case "unitSquareIncl1Right":
                    return Icon_unitSquareIncl1Right();

                case "unitSquareIncl2Right":
                    return Icon_unitSquareIncl2Right();

                case "unitSquareIncl3Right":
                    return Icon_unitSquareIncl3Right();

                case "unitSquareIncl4Right":
                    return Icon_unitSquareIncl4Right();

                case "unitSquareIncl5Right":
                    return Icon_unitSquareIncl5Right();

                case "unitSquareIncl6Right":
                    return Icon_unitSquareIncl6Right();

                case "unitSquareCrossed":
                    return Icon_unitSquareCrossed();

                case "unitCircle":
                    return Icon_unitCircle();

                case "animal":
                    return Icon_animal();

                case "bird":
                    return Icon_bird();

                case "humanMale":
                    return Icon_humanMale();

                case "humanFemale":
                    return Icon_humanFemale();

                case "bombExplosion":
                    return Icon_bombExplosion();

                case "tower":
                    return Icon_tower();

                case "circleDotFilled":
                    return Icon_circleDotFilled();

                case "circleDotUnfilled":
                    return Icon_circleDotUnfilled();

                case "logMessage":
                    return Icon_logMessage();

                case "logMessageError":
                    return Icon_logMessageError();

                case "logMessageException":
                    return Icon_logMessageException();

                case "logMessageAssertion":
                    return Icon_logMessageAssertion();

                case "up_oneStroke":
                    return Icon_up_oneStroke();

                case "up_twoStroke":
                    return Icon_up_twoStroke();

                case "up_threeStroke":
                    return Icon_up_threeStroke();

                case "down_oneStroke":
                    return Icon_down_oneStroke();

                case "down_twoStroke":
                    return Icon_down_twoStroke();

                case "down_threeStroke":
                    return Icon_down_threeStroke();

                case "left_oneStroke":
                    return Icon_left_oneStroke();

                case "left_twoStroke":
                    return Icon_left_twoStroke();

                case "left_threeStroke":
                    return Icon_left_threeStroke();

                case "right_oneStroke":
                    return Icon_right_oneStroke();

                case "right_twoStroke":
                    return Icon_right_twoStroke();

                case "right_threeStroke":
                    return Icon_right_threeStroke();

                case "fist":
                    return Icon_fist();

                case "boxingGlove":
                    return Icon_boxingGlove();

                case "stars5Rate":
                    return Icon_stars5Rate();

                case "stars3":
                    return Icon_stars3();

                case "shootingStar":
                    return Icon_shootingStar();

                case "moonHalf":
                    return Icon_moonHalf();

                case "moonFullPlanet":
                    return Icon_moonFullPlanet();

                case "leftHandRule":
                    return Icon_leftHandRule();

                case "rightHandRule":
                    return Icon_rightHandRule();

                case "megaphone":
                    return Icon_megaphone();

                case "arrowLeft":
                    return Icon_arrowLeft();

                case "arrowRight":
                    return Icon_arrowRight();

                case "arrowUp":
                    return Icon_arrowUp();

                case "arrowDown":
                    return Icon_arrowDown();

                case "healthBox":
                    return Icon_healthBox();

                case "iceIcicle":
                    return Icon_iceIcicle();

                case "pickAxe":
                    return Icon_pickAxe();

                case "audioSpeakerMute":
                    return Icon_audioSpeakerMute();

                case "chestTreasureBox_open":
                    return Icon_chestTreasureBox_open();

                case "doorClosed":
                    return Icon_doorClosed();

                default:
                    charIsMissing = true;
                    return Char_unknown();
            }

        }

        public Vector3[][] GetPointsArray(DrawBasics.IconType requestedIcon)
        {
            switch (requestedIcon)
            {
                case DrawBasics.IconType.profileFoto:
                    return Icon_profileFoto();

                case DrawBasics.IconType.imageLandscape:
                    return Icon_imageLandscape();

                case DrawBasics.IconType.homeHouse:
                    return Icon_homeHouse();

                case DrawBasics.IconType.dataDisc:
                    return Icon_dataDisc();

                case DrawBasics.IconType.saveData:
                    return Icon_saveData();

                case DrawBasics.IconType.loadData:
                    return Icon_loadData();

                case DrawBasics.IconType.speechBubble:
                    return Icon_speechBubble();

                case DrawBasics.IconType.speechBubbleEmpty:
                    return Icon_speechBubbleEmpty();

                case DrawBasics.IconType.thumbUp:
                    return Icon_thumbUp();

                case DrawBasics.IconType.thumbDown:
                    return Icon_thumbDown();

                case DrawBasics.IconType.lightBulbOn:
                    return Icon_lightBulbOn();

                case DrawBasics.IconType.lightBulbOff:
                    return Icon_lightBulbOff();

                case DrawBasics.IconType.videoCamera:
                    return Icon_videoCamera();

                case DrawBasics.IconType.camera:
                    return Icon_camera();

                case DrawBasics.IconType.music:
                    return Icon_music();

                case DrawBasics.IconType.audioSpeaker:
                    return Icon_audioSpeaker();

                case DrawBasics.IconType.microphone:
                    return Icon_microphone();

                case DrawBasics.IconType.wlan_wifi:
                    return Icon_wlan_wifi();

                case DrawBasics.IconType.share:
                    return Icon_share();

                case DrawBasics.IconType.timeClock:
                    return Icon_timeClock();

                case DrawBasics.IconType.telephone:
                    return Icon_telephone();

                case DrawBasics.IconType.doorOpen:
                    return Icon_doorOpen();

                case DrawBasics.IconType.doorEnter:
                    return Icon_doorEnter();

                case DrawBasics.IconType.doorLeave:
                    return Icon_doorLeave();

                case DrawBasics.IconType.locationPin:
                    return Icon_locationPin();

                case DrawBasics.IconType.folder:
                    return Icon_folder();

                case DrawBasics.IconType.saveToFolder:
                    return Icon_saveToFolder();

                case DrawBasics.IconType.loadFromFolder:
                    return Icon_loadFromFolder();

                case DrawBasics.IconType.optionsSettingsGear:
                    return Icon_optionsSettingsGear();

                case DrawBasics.IconType.adjustOptionsSettings:
                    return Icon_adjustOptionsSettings();

                case DrawBasics.IconType.pen:
                    return Icon_pen();

                case DrawBasics.IconType.questionMark:
                    return Icon_questionMark();

                case DrawBasics.IconType.exclamationMark:
                    return Icon_exclamationMark();

                case DrawBasics.IconType.shoppingCart:
                    return Icon_shoppingCart();

                case DrawBasics.IconType.checkmarkChecked:
                    return Icon_checkmarkChecked();

                case DrawBasics.IconType.checkmarkUnchecked:
                    return Icon_checkmarkUnchecked();

                case DrawBasics.IconType.battery:
                    return Icon_battery();

                case DrawBasics.IconType.cloud:
                    return Icon_cloud();

                case DrawBasics.IconType.magnifier:
                    return Icon_magnifier();

                case DrawBasics.IconType.magnifierPlus:
                    return Icon_magnifierPlus();

                case DrawBasics.IconType.magnifierMinus:
                    return Icon_magnifierMinus();

                case DrawBasics.IconType.timeHourglassCursor:
                    return Icon_timeHourglassCursor();

                case DrawBasics.IconType.cursorHand:
                    return Icon_cursorHand();

                case DrawBasics.IconType.cursorPointer:
                    return Icon_cursorPointer();

                case DrawBasics.IconType.trashcan:
                    return Icon_trashcan();

                case DrawBasics.IconType.switchOnOff:
                    return Icon_switchOnOff();

                case DrawBasics.IconType.playButton:
                    return Icon_playButton();

                case DrawBasics.IconType.pauseButton:
                    return Icon_pauseButton();

                case DrawBasics.IconType.stopButton:
                    return Icon_stopButton();

                case DrawBasics.IconType.playPauseButton:
                    return Icon_playPauseButton();

                case DrawBasics.IconType.heart:
                    return Icon_heart();

                case DrawBasics.IconType.coin:
                    return Icon_coin();

                case DrawBasics.IconType.coins:
                    return Icon_coins();

                case DrawBasics.IconType.moneyBills:
                    return Icon_moneyBills();

                case DrawBasics.IconType.moneyBag:
                    return Icon_moneyBag();

                case DrawBasics.IconType.chestTreasureBox_closed:
                    return Icon_chestTreasureBox_closed();

                case DrawBasics.IconType.lootbox:
                    return Icon_lootbox();

                case DrawBasics.IconType.crown:
                    return Icon_crown();

                case DrawBasics.IconType.trophy:
                    return Icon_trophy();

                case DrawBasics.IconType.awardMedal:
                    return Icon_awardMedal();

                case DrawBasics.IconType.sword:
                    return Icon_sword();

                case DrawBasics.IconType.shield:
                    return Icon_shield();

                case DrawBasics.IconType.gun:
                    return Icon_gun();

                case DrawBasics.IconType.bullet:
                    return Icon_bullet();

                case DrawBasics.IconType.rocket:
                    return Icon_rocket();

                case DrawBasics.IconType.crosshair:
                    return Icon_crosshair();

                case DrawBasics.IconType.arrow:
                    return Icon_arrow();

                case DrawBasics.IconType.arrowBow:
                    return Icon_arrowBow();

                case DrawBasics.IconType.bomb:
                    return Icon_bomb();

                case DrawBasics.IconType.shovel:
                    return Icon_shovel();

                case DrawBasics.IconType.hammer:
                    return Icon_hammer();

                case DrawBasics.IconType.axe:
                    return Icon_axe();

                case DrawBasics.IconType.magnet:
                    return Icon_magnet();

                case DrawBasics.IconType.compass:
                    return Icon_compass();

                case DrawBasics.IconType.fuelStation:
                    return Icon_fuelStation();

                case DrawBasics.IconType.fuelCan:
                    return Icon_fuelCan();

                case DrawBasics.IconType.lockLocked:
                    return Icon_lockLocked();

                case DrawBasics.IconType.lockUnlocked:
                    return Icon_lockUnlocked();

                case DrawBasics.IconType.key:
                    return Icon_key();

                case DrawBasics.IconType.gemDiamond:
                    return Icon_gemDiamond();

                case DrawBasics.IconType.gold:
                    return Icon_gold();

                case DrawBasics.IconType.potion:
                    return Icon_potion();

                case DrawBasics.IconType.presentGift:
                    return Icon_presentGift();

                case DrawBasics.IconType.death:
                    return Icon_death();

                case DrawBasics.IconType.map:
                    return Icon_map();

                case DrawBasics.IconType.mushroom:
                    return Icon_mushroom();

                case DrawBasics.IconType.star:
                    return Icon_star();

                case DrawBasics.IconType.pill:
                    return Icon_pill();

                case DrawBasics.IconType.health:
                    return Icon_health();

                case DrawBasics.IconType.foodPlate:
                    return Icon_foodPlate();

                case DrawBasics.IconType.foodMeat:
                    return Icon_foodMeat();

                case DrawBasics.IconType.flag:
                    return Icon_flag();

                case DrawBasics.IconType.flagChequered:
                    return Icon_flagChequered();

                case DrawBasics.IconType.ball:
                    return Icon_ball();

                case DrawBasics.IconType.dice:
                    return Icon_dice();

                case DrawBasics.IconType.joystick:
                    return Icon_joystick();

                case DrawBasics.IconType.gamepad:
                    return Icon_gamepad();

                case DrawBasics.IconType.jigsawPuzzle:
                    return Icon_jigsawPuzzle();

                case DrawBasics.IconType.fish:
                    return Icon_fish();

                case DrawBasics.IconType.car:
                    return Icon_car();

                case DrawBasics.IconType.tree:
                    return Icon_tree();

                case DrawBasics.IconType.palm:
                    return Icon_palm();

                case DrawBasics.IconType.leaf:
                    return Icon_leaf();

                case DrawBasics.IconType.nukeNuclearWarning:
                    return Icon_nukeNuclearWarning();

                case DrawBasics.IconType.biohazardWarning:
                    return Icon_biohazardWarning();

                case DrawBasics.IconType.fireWarning:
                    return Icon_fireWarning();

                case DrawBasics.IconType.warning:
                    return Icon_warning();

                case DrawBasics.IconType.emergencyExit:
                    return Icon_emergencyExit();

                case DrawBasics.IconType.sun:
                    return Icon_sun();

                case DrawBasics.IconType.rain:
                    return Icon_rain();

                case DrawBasics.IconType.wind:
                    return Icon_wind();

                case DrawBasics.IconType.snow:
                    return Icon_snow();

                case DrawBasics.IconType.lightning:
                    return Icon_lightning();

                case DrawBasics.IconType.fire:
                    return Icon_fire();

                case DrawBasics.IconType.unitSquare:
                    return Icon_unitSquare();

                case DrawBasics.IconType.unitSquareIncl1Right:
                    return Icon_unitSquareIncl1Right();

                case DrawBasics.IconType.unitSquareIncl2Right:
                    return Icon_unitSquareIncl2Right();

                case DrawBasics.IconType.unitSquareIncl3Right:
                    return Icon_unitSquareIncl3Right();

                case DrawBasics.IconType.unitSquareIncl4Right:
                    return Icon_unitSquareIncl4Right();

                case DrawBasics.IconType.unitSquareIncl5Right:
                    return Icon_unitSquareIncl5Right();

                case DrawBasics.IconType.unitSquareIncl6Right:
                    return Icon_unitSquareIncl6Right();

                case DrawBasics.IconType.unitSquareCrossed:
                    return Icon_unitSquareCrossed();

                case DrawBasics.IconType.unitCircle:
                    return Icon_unitCircle();

                case DrawBasics.IconType.animal:
                    return Icon_animal();

                case DrawBasics.IconType.bird:
                    return Icon_bird();

                case DrawBasics.IconType.humanMale:
                    return Icon_humanMale();

                case DrawBasics.IconType.humanFemale:
                    return Icon_humanFemale();

                case DrawBasics.IconType.bombExplosion:
                    return Icon_bombExplosion();

                case DrawBasics.IconType.tower:
                    return Icon_tower();

                case DrawBasics.IconType.circleDotFilled:
                    return Icon_circleDotFilled();

                case DrawBasics.IconType.circleDotUnfilled:
                    return Icon_circleDotUnfilled();

                case DrawBasics.IconType.logMessage:
                    return Icon_logMessage();

                case DrawBasics.IconType.logMessageError:
                    return Icon_logMessageError();

                case DrawBasics.IconType.logMessageException:
                    return Icon_logMessageException();

                case DrawBasics.IconType.logMessageAssertion:
                    return Icon_logMessageAssertion();

                case DrawBasics.IconType.up_oneStroke:
                    return Icon_up_oneStroke();

                case DrawBasics.IconType.up_twoStroke:
                    return Icon_up_twoStroke();

                case DrawBasics.IconType.up_threeStroke:
                    return Icon_up_threeStroke();

                case DrawBasics.IconType.down_oneStroke:
                    return Icon_down_oneStroke();

                case DrawBasics.IconType.down_twoStroke:
                    return Icon_down_twoStroke();

                case DrawBasics.IconType.down_threeStroke:
                    return Icon_down_threeStroke();

                case DrawBasics.IconType.left_oneStroke:
                    return Icon_left_oneStroke();

                case DrawBasics.IconType.left_twoStroke:
                    return Icon_left_twoStroke();

                case DrawBasics.IconType.left_threeStroke:
                    return Icon_left_threeStroke();

                case DrawBasics.IconType.right_oneStroke:
                    return Icon_right_oneStroke();

                case DrawBasics.IconType.right_twoStroke:
                    return Icon_right_twoStroke();

                case DrawBasics.IconType.right_threeStroke:
                    return Icon_right_threeStroke();

                case DrawBasics.IconType.fist:
                    return Icon_fist();

                case DrawBasics.IconType.boxingGlove:
                    return Icon_boxingGlove();

                case DrawBasics.IconType.stars5Rate:
                    return Icon_stars5Rate();

                case DrawBasics.IconType.stars3:
                    return Icon_stars3();

                case DrawBasics.IconType.shootingStar:
                    return Icon_shootingStar();

                case DrawBasics.IconType.moonHalf:
                    return Icon_moonHalf();

                case DrawBasics.IconType.moonFullPlanet:
                    return Icon_moonFullPlanet();

                case DrawBasics.IconType.leftHandRule:
                    return Icon_leftHandRule();

                case DrawBasics.IconType.rightHandRule:
                    return Icon_rightHandRule();

                case DrawBasics.IconType.megaphone:
                    return Icon_megaphone();

                case DrawBasics.IconType.arrowLeft:
                    return Icon_arrowLeft();

                case DrawBasics.IconType.arrowRight:
                    return Icon_arrowRight();

                case DrawBasics.IconType.arrowUp:
                    return Icon_arrowUp();

                case DrawBasics.IconType.arrowDown:
                    return Icon_arrowDown();

                case DrawBasics.IconType.healthBox:
                    return Icon_healthBox();

                case DrawBasics.IconType.iceIcicle:
                    return Icon_iceIcicle();

                case DrawBasics.IconType.pickAxe:
                    return Icon_pickAxe();

                case DrawBasics.IconType.audioSpeakerMute:
                    return Icon_audioSpeakerMute();

                case DrawBasics.IconType.chestTreasureBox_open:
                    return Icon_chestTreasureBox_open();

                case DrawBasics.IconType.doorClosed:
                    return Icon_doorClosed();

                default:
                    Debug.LogError("Icon '" + requestedIcon + "' not implemented.");
                    return Char_unknown();
            }

        }

    }

}
