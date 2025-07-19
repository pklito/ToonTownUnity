namespace DrawXXL
{
    using UnityEngine;
    using System;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(BezierSplineDrawer2D))]
    public class BezierSplineDrawer2DInspector : VisualizerParentInspector
    {
        BezierSplineDrawer2D bezierSplineDrawer_unserializedMonoB;
        Tool toolToSelectAfterThisInspectorGetsUnselcted = Tool.None;
        Tool selectedToolDuringPreviousOnSceneGUI = Tool.None;
        bool sheduleFocusSceneViewOnControlPoint = false;
        int delayCounter_afterShedulingFocusSceneViewOnControlPoint = 0;
        float dotLength_ofDottedLines = 4.0f;
        bool hasRegisteredUndo_sinceMouseDown = false;
        GUIContent plusSymbolIcon;
        Vector3 sceneViewCamForward_normalized;
        Vector3 sceneViewCamUp_normalized;
        Vector3 sceneViewCamRight_normalized;
        Vector3 sceneViewCam_to_anchorPoint;
        BezierSplineDrawerInspector.ManyDrawnLinesWarningState manyDrawnLinesWarningState = BezierSplineDrawerInspector.ManyDrawnLinesWarningState.noWarning;

        void OnEnable()
        {
            OnEnable_base();
            bezierSplineDrawer_unserializedMonoB = (BezierSplineDrawer2D)target;
            plusSymbolIcon = EditorGUIUtility.TrIconContent("Toolbar Plus", "Add new control point");

            toolToSelectAfterThisInspectorGetsUnselcted = Tools.current;
            ProcessChangingEditorTool();
            selectedToolDuringPreviousOnSceneGUI = Tools.current;
        }

        public void OnDisable()
        {
            Tools.current = toolToSelectAfterThisInspectorGetsUnselcted;
        }

        public void OnSceneGUI()
        {
            if (bezierSplineDrawer_unserializedMonoB != null) //-> sometimes after deleting the spline component in the inspector this "OnSceneGUI()" is still called, which would result in a missingRefException without this check
            {
                bezierSplineDrawer_unserializedMonoB.sheduledSceneViewRepaint_hasBeenExecuted = true;

                if (bezierSplineDrawer_unserializedMonoB.enabled) //-> GizmoLines automatically hide if a component is disabled, Handles do not: Therefore manual disabling here
                {
                    TryFocusControlPointInSceneView_onKeypressF();
                    TryProcessChangingEditorTool();

                    Matrix4x4 handlesMatrix_before = Handles.matrix;
                    Color handlesColor_before = Handles.color;

                    DrawHandles();

                    Handles.matrix = handlesMatrix_before;
                    Handles.color = handlesColor_before;
                }
            }
        }

        void TryFocusControlPointInSceneView_onKeypressF()
        {
            delayCounter_afterShedulingFocusSceneViewOnControlPoint++;
            SheduleFocusing_onKeypressF();
            ExecuteSheduledFocusingAfterDelay();
        }

        void SheduleFocusing_onKeypressF()
        {
            Event currentEvent = Event.current;
            if (currentEvent.type == EventType.KeyDown)
            {
                if (currentEvent.keyCode == KeyCode.F)
                {
                    //this only works when the mouse is in the scene view window (in contrast to Unitys build-in behaviour, where you can focus the scene view camera via pressing "F" even when the mouse is not in the scene view)
                    sheduleFocusSceneViewOnControlPoint = true;
                    delayCounter_afterShedulingFocusSceneViewOnControlPoint = 0;
                }
            }
        }

        void ExecuteSheduledFocusingAfterDelay()
        {
            if (sheduleFocusSceneViewOnControlPoint)
            {
                //This overwrites the focusing on the gameobject center (which is automatically executed by Unity onKeypressF) with focusing on the the selected control point instead.
                //The sheduling is because otherwise Unitys automatic focussing on the gameobject will overwrite the here executed focusing
                int delayValue = 20; //This is a guessed trial-an-error value.
                if (delayCounter_afterShedulingFocusSceneViewOnControlPoint > delayValue)
                {
                    sheduleFocusSceneViewOnControlPoint = false;
                    if (SceneView.lastActiveSceneView != null)
                    {
                        int i_ofFirstHighlightedControlPoint = bezierSplineDrawer_unserializedMonoB.Get_i_ofFirstHighlightedControlPoint();
                        if (i_ofFirstHighlightedControlPoint == (-1))
                        {
                            FrameSceneViewCam_soItSeesAllControlPoints(i_ofFirstHighlightedControlPoint);
                        }
                        else
                        {
                            FrameSceneViewCam_soItSeesSpecifiedControlPoints(i_ofFirstHighlightedControlPoint, true);
                        }
                    }
                }
            }
        }

        void FrameSceneViewCam_soItSeesAllControlPoints(int i_ofFirstHighlightedControlPoint)
        {
            if (bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets.Count == 0)
            {
                Bounds boundsOfSelection = new Bounds(bezierSplineDrawer_unserializedMonoB.Get_originPos_ofActiveDrawSpace_inUnitsOfGlobalSpace(), Vector3.zero);
                SceneView.lastActiveSceneView.Frame(boundsOfSelection, false);
            }
            else
            {
                FrameSceneViewCam_soItSeesSpecifiedControlPoints(i_ofFirstHighlightedControlPoint, false);
            }
        }

        void FrameSceneViewCam_soItSeesSpecifiedControlPoints(int i_ofFirstHighlightedControlPoint, bool includeOnlySelectedControlPoints_notAllControlPoints)
        {
            Vector3 posGlobal_ofFirstFramedControlPoint;
            if (includeOnlySelectedControlPoints_notAllControlPoints)
            {
                posGlobal_ofFirstFramedControlPoint = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i_ofFirstHighlightedControlPoint].anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos();
            }
            else
            {
                //"listOfControlPointTriplets.Count" is guaranteed bigger than 0 here:
                posGlobal_ofFirstFramedControlPoint = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[0].anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos();
            }
            Bounds boundsOfSelection = new Bounds(posGlobal_ofFirstFramedControlPoint, Vector3.zero);

            for (int i = 0; i < bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets.Count; i++)
            {
                if ((includeOnlySelectedControlPoints_notAllControlPoints == false) || bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].isHighlighted)
                {
                    boundsOfSelection.Encapsulate(bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos());

                    InternalDXXL_BezierControlPointTriplet2D nextControlPoint = bezierSplineDrawer_unserializedMonoB.GetNextControlPointTriplet(i, false);
                    if (nextControlPoint != null) { boundsOfSelection.Encapsulate(nextControlPoint.anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos()); }

                    InternalDXXL_BezierControlPointTriplet2D previousControlPoint = bezierSplineDrawer_unserializedMonoB.GetPreviousControlPointTriplet(i, false);
                    if (previousControlPoint != null) { boundsOfSelection.Encapsulate(previousControlPoint.anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos()); }
                }
            }
            SceneView.lastActiveSceneView.Frame(boundsOfSelection, false);
        }

        void DrawHandles()
        {
            if (bezierSplineDrawer_unserializedMonoB.hideAllHandles == false)
            {
                if (bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets.Count == 0)
                {
                    DrawPlusButton_asFallbackIfNoControlPointsExist();
                }
                else
                {
                    for (int i = 0; i < bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets.Count; i++)
                    {
                        Handles.matrix = Matrix4x4.identity;

                        AssignControlHandleIDs(i);
                        DrawLinesBetweenSubPoints(i);
                        DrawLinesAlongZToBoundGameobjects(i);
                        DrawIndexAsText(i);
                        UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out sceneViewCamForward_normalized, out sceneViewCamUp_normalized, out sceneViewCamRight_normalized, out sceneViewCam_to_anchorPoint, bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), DrawBasics.CameraForAutomaticOrientation.sceneViewCamera);
                        DrawNonInteractableAnchorPointVisualizer(i);
                        DrawNonInteractableHelperPointVisualizer(i, true, bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].forwardHelperPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos());
                        DrawNonInteractableHelperPointVisualizer(i, false, bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].backwardHelperPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos());
                        DrawPlusButtonsForAddingNewControlPoints_atSplineStartAndEnd(i);
                        DrawPlusButtonsForAddingNewControlPoints_somewhereOnUpcomingSplineSegment(i);
                        DrawUnitysBuildInHandlesAtSubPoints(i);
                        DrawCustomHandlesAtSubPoints(i);
                        TrySetSelectedListSlot_dueToHandlesInteraction(i);
                        Reset_recalculationFlags_duringNoHandleClickedOrDraggedPhases(i);
                    }
                }
            }

            ResetUndoRegistrationFlag_duringNoInteractionPhases();
        }

        void DrawPlusButton_asFallbackIfNoControlPointsExist()
        {
            Vector3 posOfPlusButton_inUnitsOfGlobalSpace = bezierSplineDrawer_unserializedMonoB.Get_originPos_ofActiveDrawSpace_inUnitsOfGlobalSpace();
            UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out sceneViewCamForward_normalized, out sceneViewCamUp_normalized, out sceneViewCamRight_normalized, out sceneViewCam_to_anchorPoint, posOfPlusButton_inUnitsOfGlobalSpace, DrawBasics.CameraForAutomaticOrientation.sceneViewCamera);
            bool buttonHasBeenClicked = InternalDXXL_BezierHandles.PlusButton(posOfPlusButton_inUnitsOfGlobalSpace, bezierSplineDrawer_unserializedMonoB.handleSizeOf_plusButtons, bezierSplineDrawer_unserializedMonoB.color_ofAnchorPoints, sceneViewCamForward_normalized, sceneViewCamUp_normalized, sceneViewCamRight_normalized, plusSymbolIcon);
            if (buttonHasBeenClicked)
            {
                bezierSplineDrawer_unserializedMonoB.CreateNewControlPoint_atSplineEnd();
            }
        }

        void AssignControlHandleIDs(int i)
        {
            InternalDXXL_BezierControlPointTriplet2D concernedControlPoint = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i];
            int hint_base = 100 * i; //probably also working without the int-hints

            concernedControlPoint.anchorPoint.controlID_ofCustomHandles_sphere = GUIUtility.GetControlID(hint_base + 1, FocusType.Passive);
            concernedControlPoint.anchorPoint.controlID_ofCustomHandles_forwardCone = GUIUtility.GetControlID(hint_base + 2, FocusType.Passive);
            concernedControlPoint.anchorPoint.controlID_ofCustomHandles_backwardCone = GUIUtility.GetControlID(hint_base + 3, FocusType.Passive);
            concernedControlPoint.anchorPoint.controlID_ofUnityStylePositionHandleUp = GUIUtility.GetControlID(hint_base + 4, FocusType.Passive);
            concernedControlPoint.anchorPoint.controlID_ofUnityStylePositionHandleRight = GUIUtility.GetControlID(hint_base + 5, FocusType.Passive);
            concernedControlPoint.anchorPoint.controlID_ofUnityStyleRotationHandle2D = GUIUtility.GetControlID(hint_base + 6, FocusType.Passive);

            concernedControlPoint.forwardHelperPoint.controlID_ofCustomHandles_sphere = GUIUtility.GetControlID(hint_base + 7, FocusType.Passive);
            concernedControlPoint.forwardHelperPoint.controlID_ofCustomHandles_coneAlongLineWithAnchor = GUIUtility.GetControlID(hint_base + 8, FocusType.Passive);
            concernedControlPoint.forwardHelperPoint.controlID_ofCustomHandles_coneAlongLineWithNeighborsHelper = GUIUtility.GetControlID(hint_base + 9, FocusType.Passive);
            concernedControlPoint.forwardHelperPoint.controlID_ofCustomHandles_cylinderAlongLineWithAnchor = GUIUtility.GetControlID(hint_base + 10, FocusType.Passive);
            concernedControlPoint.forwardHelperPoint.controlID_ofCustomHandles_cylinderAlongLineWithNeighborsHelper = GUIUtility.GetControlID(hint_base + 11, FocusType.Passive);
            concernedControlPoint.forwardHelperPoint.controlID_ofUnityStylePositionHandleUp = GUIUtility.GetControlID(hint_base + 12, FocusType.Passive);
            concernedControlPoint.forwardHelperPoint.controlID_ofUnityStylePositionHandleRight = GUIUtility.GetControlID(hint_base + 13, FocusType.Passive);

            concernedControlPoint.backwardHelperPoint.controlID_ofCustomHandles_sphere = GUIUtility.GetControlID(hint_base + 14, FocusType.Passive);
            concernedControlPoint.backwardHelperPoint.controlID_ofCustomHandles_coneAlongLineWithAnchor = GUIUtility.GetControlID(hint_base + 15, FocusType.Passive);
            concernedControlPoint.backwardHelperPoint.controlID_ofCustomHandles_coneAlongLineWithNeighborsHelper = GUIUtility.GetControlID(hint_base + 16, FocusType.Passive);
            concernedControlPoint.backwardHelperPoint.controlID_ofCustomHandles_cylinderAlongLineWithAnchor = GUIUtility.GetControlID(hint_base + 17, FocusType.Passive);
            concernedControlPoint.backwardHelperPoint.controlID_ofCustomHandles_cylinderAlongLineWithNeighborsHelper = GUIUtility.GetControlID(hint_base + 18, FocusType.Passive);
            concernedControlPoint.backwardHelperPoint.controlID_ofUnityStylePositionHandleUp = GUIUtility.GetControlID(hint_base + 19, FocusType.Passive);
            concernedControlPoint.backwardHelperPoint.controlID_ofUnityStylePositionHandleRight = GUIUtility.GetControlID(hint_base + 20, FocusType.Passive);
        }

        void DrawLinesBetweenSubPoints(int i)
        {
            TryDrawLineToForwardWeight(i);
            TryDrawLineToBackwardWeight(i);
            TryDrawLineFromForwardHelperPoint_toBackwardHelperPointOfNextControlPoint(i);
            TryDrawLineFromForwardHelperPoint_toAnchorPointOfNextControlPoint(i);
            TryDrawLineFromAnchorPoint_toBackwardHelperPointOfNextControlPoint(i);
        }

        void TryDrawLineToForwardWeight(int i)
        {
            if (bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].forwardHelperPoint.isUsed)
            {
                Vector3 lineStartPos_inUnitsOfGlobalSpace = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos();
                Vector3 lineEndPos_inUnitsOfGlobalSpace = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].forwardHelperPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos();

                TryDrawExpandedLowAlphaLine_fromAnchorToHelper(i, lineStartPos_inUnitsOfGlobalSpace, lineEndPos_inUnitsOfGlobalSpace);

                Handles.color = bezierSplineDrawer_unserializedMonoB.color_ofHelperPoints;
                Handles.DrawLine(lineStartPos_inUnitsOfGlobalSpace, lineEndPos_inUnitsOfGlobalSpace);
            }
        }

        void TryDrawLineToBackwardWeight(int i)
        {
            if (bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].backwardHelperPoint.isUsed)
            {
                Vector3 lineStartPos_inUnitsOfGlobalSpace = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos();
                Vector3 lineEndPos_inUnitsOfGlobalSpace = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].backwardHelperPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos();

                TryDrawExpandedLowAlphaLine_fromAnchorToHelper(i, lineStartPos_inUnitsOfGlobalSpace, lineEndPos_inUnitsOfGlobalSpace);

                Handles.color = bezierSplineDrawer_unserializedMonoB.color_ofHelperPoints;
                Handles.DrawLine(lineStartPos_inUnitsOfGlobalSpace, lineEndPos_inUnitsOfGlobalSpace);
            }
        }

        void TryDrawExpandedLowAlphaLine_fromAnchorToHelper(int i, Vector3 lineStartPos_inUnitsOfGlobalSpace, Vector3 lineEndPos_inUnitsOfGlobalSpace)
        {
            if (bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].isHighlighted)
            {
                Handles.color = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(bezierSplineDrawer_unserializedMonoB.color_ofHelperPoints, BezierSplineDrawerInspector.alpha_ofAdditionalExpandedLowAlphaLineToHelpers_ofSelectedControlPoints);
                Handles.DrawAAPolyLine(BezierSplineDrawerInspector.width_ofAdditionalExpandedLowAlphaLineToHelpers_ofSelectedControlPoints, lineStartPos_inUnitsOfGlobalSpace, lineEndPos_inUnitsOfGlobalSpace);
            }
        }

        void TryDrawLineFromForwardHelperPoint_toBackwardHelperPointOfNextControlPoint(int i)
        {
            if (CheckIf_drawLineFromForwardHelperPoint_toBackwardHelperPointOfNextControlPoint(i))
            {
                Handles.color = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(bezierSplineDrawer_unserializedMonoB.color_ofHelperPoints, BezierSplineDrawerInspector.alphaOfDottedLineBetweenHelperPoints);
                int i_ofNextControlPoint = UtilitiesDXXL_Math.LoopOvershootingIndexIntoCollectionSize(i + 1, bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets.Count);
                Handles.DrawDottedLine(bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].forwardHelperPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i_ofNextControlPoint].backwardHelperPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), dotLength_ofDottedLines);
            }
        }

        bool CheckIf_drawLineFromForwardHelperPoint_toBackwardHelperPointOfNextControlPoint(int i)
        {
            if (bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].forwardHelperPoint.isUsed)
            {
                InternalDXXL_BezierControlPointTriplet2D nextControlPointTriplet = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].GetNextControlPointTripletAlongSplineDir(false);
                if (nextControlPointTriplet == null)
                {
                    return false;
                }
                else
                {
                    return nextControlPointTriplet.backwardHelperPoint.isUsed;
                }
            }
            else
            {
                return false;
            }
        }

        void TryDrawLineFromForwardHelperPoint_toAnchorPointOfNextControlPoint(int i)
        {
            if (CheckIf_drawLineFromForwardHelperPoint_toAnchorPointOfNextControlPoint(i))
            {
                Handles.color = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(bezierSplineDrawer_unserializedMonoB.color_ofHelperPoints, BezierSplineDrawerInspector.alphaOfDottedLineBetweenHelperPoints);
                int i_ofNextControlPoint = UtilitiesDXXL_Math.LoopOvershootingIndexIntoCollectionSize(i + 1, bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets.Count);
                Handles.DrawDottedLine(bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].forwardHelperPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i_ofNextControlPoint].anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), dotLength_ofDottedLines);
            }
        }

        bool CheckIf_drawLineFromForwardHelperPoint_toAnchorPointOfNextControlPoint(int i)
        {
            if (bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].forwardHelperPoint.isUsed)
            {
                InternalDXXL_BezierControlPointTriplet2D nextControlPointTriplet = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].GetNextControlPointTripletAlongSplineDir(false);
                if (nextControlPointTriplet == null)
                {
                    return false;
                }
                else
                {
                    return (nextControlPointTriplet.backwardHelperPoint.isUsed == false);
                }
            }
            else
            {
                return false;
            }
        }

        void TryDrawLineFromAnchorPoint_toBackwardHelperPointOfNextControlPoint(int i)
        {
            if (CheckIf_drawLineFromAnchorPoint_toBackwardHelperPointOfNextControlPoint(i))
            {
                Handles.color = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(bezierSplineDrawer_unserializedMonoB.color_ofHelperPoints, BezierSplineDrawerInspector.alphaOfDottedLineBetweenHelperPoints);
                int i_ofNextControlPoint = UtilitiesDXXL_Math.LoopOvershootingIndexIntoCollectionSize(i + 1, bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets.Count);
                Handles.DrawDottedLine(bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i_ofNextControlPoint].backwardHelperPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), dotLength_ofDottedLines);
            }
        }

        bool CheckIf_drawLineFromAnchorPoint_toBackwardHelperPointOfNextControlPoint(int i)
        {
            if (bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].forwardHelperPoint.isUsed == false)
            {
                InternalDXXL_BezierControlPointTriplet2D nextControlPointTriplet = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].GetNextControlPointTripletAlongSplineDir(false);
                if (nextControlPointTriplet == null)
                {
                    return false;
                }
                else
                {
                    return nextControlPointTriplet.backwardHelperPoint.isUsed;
                }
            }
            else
            {
                return false;
            }
        }

        void DrawLinesAlongZToBoundGameobjects(int i)
        {
            if (bezierSplineDrawer_unserializedMonoB.showDottedLinesAlongZToBoundGameobjects)
            {
                Handles.color = bezierSplineDrawer_unserializedMonoB.colorOfLinesAlongZToBoundGameobjects;
                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.SetBoolOf_boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled();
                if (bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled)
                {
                    Handles.DrawDottedLine(bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.boundGameobject.transform.position, bezierSplineDrawer_unserializedMonoB.dotLength_ofDottedLinesAlongZToBoundGameobjects);
                }

                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].backwardHelperPoint.SetBoolOf_boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled();
                if (bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].backwardHelperPoint.boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled)
                {
                    Handles.DrawDottedLine(bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].backwardHelperPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].backwardHelperPoint.boundGameobject.transform.position, bezierSplineDrawer_unserializedMonoB.dotLength_ofDottedLinesAlongZToBoundGameobjects);
                }

                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].forwardHelperPoint.SetBoolOf_boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled();
                if (bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].forwardHelperPoint.boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled)
                {
                    Handles.DrawDottedLine(bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].forwardHelperPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].forwardHelperPoint.boundGameobject.transform.position, bezierSplineDrawer_unserializedMonoB.dotLength_ofDottedLinesAlongZToBoundGameobjects);
                }
            }
        }

        void TryProcessChangingEditorTool()
        {
            if (selectedToolDuringPreviousOnSceneGUI != Tools.current)
            {
                ProcessChangingEditorTool();
            }
            selectedToolDuringPreviousOnSceneGUI = Tools.current;
        }

        void ProcessChangingEditorTool()
        {
            if (Tools.current == Tool.Move)
            {
                if (bezierSplineDrawer_unserializedMonoB.showCustomHandleFor_anchorPoints == bezierSplineDrawer_unserializedMonoB.showCustomHandleFor_helperPoints)
                {
                    //-> custom handles are "both on" or "both off"
                    bezierSplineDrawer_unserializedMonoB.showHandleFor_position_atAnchors = true;
                    bezierSplineDrawer_unserializedMonoB.showHandleFor_position_atHelpers = true;
                }
                else
                {
                    //-> one custom handle is on, the other is off
                    bezierSplineDrawer_unserializedMonoB.showHandleFor_position_atAnchors = bezierSplineDrawer_unserializedMonoB.showCustomHandleFor_anchorPoints;
                    bezierSplineDrawer_unserializedMonoB.showHandleFor_position_atHelpers = bezierSplineDrawer_unserializedMonoB.showCustomHandleFor_helperPoints;
                }

                bezierSplineDrawer_unserializedMonoB.showHandleFor_rotation = false;
            }
            else
            {
                if (Tools.current == Tool.Rotate)
                {
                    bezierSplineDrawer_unserializedMonoB.showHandleFor_position_atAnchors = false;
                    bezierSplineDrawer_unserializedMonoB.showHandleFor_position_atHelpers = false;
                    bezierSplineDrawer_unserializedMonoB.showHandleFor_rotation = true;
                }
                else
                {
                    bezierSplineDrawer_unserializedMonoB.showHandleFor_position_atAnchors = false;
                    bezierSplineDrawer_unserializedMonoB.showHandleFor_position_atHelpers = false;
                    bezierSplineDrawer_unserializedMonoB.showHandleFor_rotation = false;
                }
            }
        }

        void DrawNonInteractableAnchorPointVisualizer(int i)
        {
            Vector3 position_ofAnchorPoint_inUnitsOfGlobalSpace = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos();
            DrawNonInteractableSubPointVisualizer(i, position_ofAnchorPoint_inUnitsOfGlobalSpace, bezierSplineDrawer_unserializedMonoB.color_ofAnchorPoints, bezierSplineDrawer_unserializedMonoB.handleSizeOf_customHandle_atAnchors, bezierSplineDrawer_unserializedMonoB.showCustomHandleFor_anchorPoints);
        }

        void DrawNonInteractableHelperPointVisualizer(int i, bool concerncedHelperPoint_isForward_notBackward, Vector3 position_ofHelperPoint_inUnitsOfGlobalSpace)
        {
            if (CheckIf_drawNonInteractableHelperPointVisualizer(i, concerncedHelperPoint_isForward_notBackward))
            {
                DrawNonInteractableSubPointVisualizer(i, position_ofHelperPoint_inUnitsOfGlobalSpace, bezierSplineDrawer_unserializedMonoB.color_ofHelperPoints, bezierSplineDrawer_unserializedMonoB.handleSizeOf_customHandle_atHelpers, bezierSplineDrawer_unserializedMonoB.showCustomHandleFor_helperPoints);
            }
        }

        void DrawNonInteractableSubPointVisualizer(int i, Vector3 position_ofSubPoint_inUnitsOfGlobalSpace, Color color, float handleSize, bool handlesAreActivated)
        {
            Handles.color = color;
            float radius_ofSubPointIndicator = 0.5f * handleSize * HandleUtility.GetHandleSize(position_ofSubPoint_inUnitsOfGlobalSpace);

            if (handlesAreActivated == false)
            {
                Handles.DrawSolidDisc(position_ofSubPoint_inUnitsOfGlobalSpace, Vector3.back, radius_ofSubPointIndicator);
            }

            if (bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].isHighlighted)
            {
                Handles.color = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, BezierSplineDrawerInspector.alpha_ofExpandedNonInteractableFlatSubPointVisualizer_forHighlightedControlPoints);
                Handles.DrawSolidDisc(position_ofSubPoint_inUnitsOfGlobalSpace, Vector3.back, BezierSplineDrawerInspector.sizeFactor_ofNonInteracalbeFlatSubPointVisualizer_forHighlightedPoints * radius_ofSubPointIndicator);
            }
        }

        bool CheckIf_drawNonInteractableHelperPointVisualizer(int i, bool concerncedHelperPoint_isForward_notBackward)
        {
            return bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].GetAHelperPoint(concerncedHelperPoint_isForward_notBackward).isUsed;
        }

        void DrawIndexAsText(int i)
        {
            GUIStyle style_ofTextAtControlPoints = new GUIStyle();
            string space_beforeTextString = "      ";

            float scaleFactorOfText = bezierSplineDrawer_unserializedMonoB.handleSizeOf_customHandle_atAnchors / BezierSplineDrawer2D.default_handleSizeOf_customHandle_atAnchors;
            int scale_ofSpaceBeforeTextString = Mathf.RoundToInt(scaleFactorOfText * 11);
            int scale_ofNumberItself = Mathf.RoundToInt(scaleFactorOfText * 25);

            string text_atControlPoint = "<size=" + scale_ofSpaceBeforeTextString + ">" + space_beforeTextString + "</size><size=" + scale_ofNumberItself + "><color=#" + ColorUtility.ToHtmlStringRGBA(bezierSplineDrawer_unserializedMonoB.color_ofAnchorPoints) + "><b>" + i + "</b></color></size>";
            Handles.Label(bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), text_atControlPoint, style_ofTextAtControlPoints);
        }

        void DrawPlusButtonsForAddingNewControlPoints_atSplineStartAndEnd(int i)
        {
            if (bezierSplineDrawer_unserializedMonoB.gapFromEndToStart_isClosed == false)
            {
                if (bezierSplineDrawer_unserializedMonoB.showHandleFor_plusButtons_atSplineStartAndEnd)
                {
                    DrawPlusButtonsForAddingNewControlPoints_atSplineStart(i);
                    DrawPlusButtonsForAddingNewControlPoints_atSplineEnd(i);
                }
            }
        }

        void DrawPlusButtonsForAddingNewControlPoints_atSplineStart(int i)
        {
            if (bezierSplineDrawer_unserializedMonoB.IsFirstControlPoint(i))
            {
                InternalDXXL_BezierControlPointTriplet2D currControlPointTriplet = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i];
                Vector2 currentFirstControlPoint_backTo_newlyCreatableControlPointAtSplineStart_inUnitsOfGlobalSpace_normalized = bezierSplineDrawer_unserializedMonoB.Get_firstControlPoint_to_newlyCreatedControlPointAtSplineStart_inUnitsOfGlobalSpace_normalized(currControlPointTriplet);
                if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(currentFirstControlPoint_backTo_newlyCreatableControlPointAtSplineStart_inUnitsOfGlobalSpace_normalized))
                {
                    currentFirstControlPoint_backTo_newlyCreatableControlPointAtSplineStart_inUnitsOfGlobalSpace_normalized = (-bezierSplineDrawer_unserializedMonoB.Get_right_ofActiveDrawSpace_inUnitsOfGlobalSpace_normalized());
                }
                Vector3 currentFirstControlPoint_backTo_newlyCreatableControlPointAtSplineStart_inUnitsOfGlobalSpace_normalized_asV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(currentFirstControlPoint_backTo_newlyCreatableControlPointAtSplineStart_inUnitsOfGlobalSpace_normalized);
                float distanceToCurrentFirstControlPoint_inUnitsOfGlobalSpace = bezierSplineDrawer_unserializedMonoB.Get_distance_from_prevLastControlPoint_to_newlyCreatedControlPoint_inUnitsOfGlobalSpace();
                Vector3 posOf_plusButtonAtSplineStart_inUnitsOfGlobalSpace = currControlPointTriplet.anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos() + currentFirstControlPoint_backTo_newlyCreatableControlPointAtSplineStart_inUnitsOfGlobalSpace_normalized_asV3 * distanceToCurrentFirstControlPoint_inUnitsOfGlobalSpace;

                Handles.color = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(bezierSplineDrawer_unserializedMonoB.color_ofHelperPoints, BezierSplineDrawerInspector.alphaOfSolidBackgroundLine_ofLinesTowardsPlusButtonsAtSplineEnds);
                Handles.DrawLine(currControlPointTriplet.anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), posOf_plusButtonAtSplineStart_inUnitsOfGlobalSpace);
                Handles.color = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(bezierSplineDrawer_unserializedMonoB.color_ofHelperPoints, BezierSplineDrawerInspector.alphaOfDottedLine_ofLinesTowardsPlusButtonsAtSplineEnds);
                Handles.DrawDottedLine(currControlPointTriplet.anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), posOf_plusButtonAtSplineStart_inUnitsOfGlobalSpace, dotLength_ofDottedLines);

                bool buttonHasBeenClicked = InternalDXXL_BezierHandles.PlusButton(posOf_plusButtonAtSplineStart_inUnitsOfGlobalSpace, bezierSplineDrawer_unserializedMonoB.handleSizeOf_plusButtons, bezierSplineDrawer_unserializedMonoB.color_ofAnchorPoints, sceneViewCamForward_normalized, sceneViewCamUp_normalized, sceneViewCamRight_normalized, plusSymbolIcon);
                if (buttonHasBeenClicked)
                {
                    bezierSplineDrawer_unserializedMonoB.CreateNewControlPoint_atSplineStart();
                }
            }
        }

        void DrawPlusButtonsForAddingNewControlPoints_atSplineEnd(int i)
        {
            if (bezierSplineDrawer_unserializedMonoB.IsLastControlPoint(i))
            {
                InternalDXXL_BezierControlPointTriplet2D currControlPointTriplet = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i];
                Vector2 currentLastControlPoint_to_newlyCreatableControlPoint_inUnitsOfGlobalSpace_normalized = bezierSplineDrawer_unserializedMonoB.Get_previouslyLastControlPoint_to_newlyCreatedControlPointAtSplineEnd_inUnitsOfGlobalSpace_normalized(currControlPointTriplet);
                if (UtilitiesDXXL_Math.CheckIfNormalizationFailed_meaningLineStayedTooShort(currentLastControlPoint_to_newlyCreatableControlPoint_inUnitsOfGlobalSpace_normalized))
                {
                    currentLastControlPoint_to_newlyCreatableControlPoint_inUnitsOfGlobalSpace_normalized = bezierSplineDrawer_unserializedMonoB.Get_right_ofActiveDrawSpace_inUnitsOfGlobalSpace_normalized();
                }
                Vector3 currentLastControlPoint_to_newlyCreatableControlPoint_inUnitsOfGlobalSpace_normalized_asV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(currentLastControlPoint_to_newlyCreatableControlPoint_inUnitsOfGlobalSpace_normalized);
                float distanceToCurrentLastControlPoint_inUnitsOfGlobalSpace = bezierSplineDrawer_unserializedMonoB.Get_distance_from_prevLastControlPoint_to_newlyCreatedControlPoint_inUnitsOfGlobalSpace();
                Vector3 posOf_plusButtonAtSplineEnd_inUnitsOfGlobalSpace = currControlPointTriplet.anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos() + currentLastControlPoint_to_newlyCreatableControlPoint_inUnitsOfGlobalSpace_normalized_asV3 * distanceToCurrentLastControlPoint_inUnitsOfGlobalSpace;

                Handles.color = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(bezierSplineDrawer_unserializedMonoB.color_ofHelperPoints, BezierSplineDrawerInspector.alphaOfSolidBackgroundLine_ofLinesTowardsPlusButtonsAtSplineEnds);
                Handles.DrawLine(currControlPointTriplet.anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), posOf_plusButtonAtSplineEnd_inUnitsOfGlobalSpace);
                Handles.color = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(bezierSplineDrawer_unserializedMonoB.color_ofHelperPoints, BezierSplineDrawerInspector.alphaOfDottedLine_ofLinesTowardsPlusButtonsAtSplineEnds);
                Handles.DrawDottedLine(currControlPointTriplet.anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), posOf_plusButtonAtSplineEnd_inUnitsOfGlobalSpace, dotLength_ofDottedLines);

                bool buttonHasBeenClicked = InternalDXXL_BezierHandles.PlusButton(posOf_plusButtonAtSplineEnd_inUnitsOfGlobalSpace, bezierSplineDrawer_unserializedMonoB.handleSizeOf_plusButtons, bezierSplineDrawer_unserializedMonoB.color_ofAnchorPoints, sceneViewCamForward_normalized, sceneViewCamUp_normalized, sceneViewCamRight_normalized, plusSymbolIcon);
                if (buttonHasBeenClicked)
                {
                    bezierSplineDrawer_unserializedMonoB.CreateNewControlPoint_atSplineEnd();
                }
            }
        }

        void DrawPlusButtonsForAddingNewControlPoints_somewhereOnUpcomingSplineSegment(int i)
        {
            if (bezierSplineDrawer_unserializedMonoB.showHandleFor_plusButtons_insideSegments)
            {
                InternalDXXL_BezierControlPointTriplet2D concernedControlPoint = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i];
                if (bezierSplineDrawer_unserializedMonoB.gapFromEndToStart_isClosed || concernedControlPoint.IsLastControlPoint() == false)
                {
                    float pos0to1_insideSegment_ofPlusButton_beforeDrag = concernedControlPoint.progress0to1_ofPlusButtonPosition_inUpcomingBezierSegment;
                    Vector2 posOfPlusButton_beforeDrag_inUnitsOfGlobalSpace_asV2 = concernedControlPoint.GetPosAtPlusButton_onUpcomingBezierSegment_inUnitsOfGlobalSpace();
                    Vector3 posOfPlusButton_beforeDrag_inUnitsOfGlobalSpace = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(posOfPlusButton_beforeDrag_inUnitsOfGlobalSpace_asV2, bezierSplineDrawer_unserializedMonoB.GetZPos_global_for2D());
                    Vector2 directionOfConeForShifting_inUnitsOfGlobalSpace_normalized = concernedControlPoint.GetTangentAtPlusButton_onUpcomingBezierSegment_inUnitsOfGlobalSpace(true);
                    float handleSize = bezierSplineDrawer_unserializedMonoB.handleSizeOf_plusButtons * HandleUtility.GetHandleSize(posOfPlusButton_beforeDrag_inUnitsOfGlobalSpace);

                    Handles.color = bezierSplineDrawer_unserializedMonoB.color_ofAnchorPoints;
                    Quaternion rotation_ofForwardCone = Quaternion.LookRotation(directionOfConeForShifting_inUnitsOfGlobalSpace_normalized, Vector3.zero);
                    Quaternion rotation_ofBackwardCone = Quaternion.LookRotation(-directionOfConeForShifting_inUnitsOfGlobalSpace_normalized, Vector3.zero);
                    Get_posOfPlusButtonCones(out Vector3 posOf_forwardCone_beforeDrag_inUnitsOfGlobalSpace, out Vector3 posOf_backwardCone_beforeDrag_inUnitsOfGlobalSpace, posOfPlusButton_beforeDrag_inUnitsOfGlobalSpace, directionOfConeForShifting_inUnitsOfGlobalSpace_normalized, handleSize);

                    Start_handlesChangeCheck();
                    float pos0to1_insideSegment_ofPlusButton_afterDragOfForwardCone = InternalDXXL_BezierHandles.ValueSliderAlongCurve(pos0to1_insideSegment_ofPlusButton_beforeDrag, posOf_forwardCone_beforeDrag_inUnitsOfGlobalSpace, rotation_ofForwardCone, directionOfConeForShifting_inUnitsOfGlobalSpace_normalized, handleSize, Handles.ConeHandleCap, 1.0f);
                    bool forwardCone_hasChanged = End_handlesChangeCheck("Shift Button on Spline", i, false);

                    Start_handlesChangeCheck();
                    float pos0to1_insideSegment_ofPlusButton_afterDragOfBackwardCone = InternalDXXL_BezierHandles.ValueSliderAlongCurve(pos0to1_insideSegment_ofPlusButton_beforeDrag, posOf_backwardCone_beforeDrag_inUnitsOfGlobalSpace, rotation_ofBackwardCone, directionOfConeForShifting_inUnitsOfGlobalSpace_normalized, handleSize, Handles.ConeHandleCap, 1.0f);
                    bool backwardCone_hasChanged = End_handlesChangeCheck("Shift Button on Spline", i, false);

                    if (forwardCone_hasChanged) { Update_progress0to1_ofPlusButtonPosition_inUpcomingBezierSegment(pos0to1_insideSegment_ofPlusButton_afterDragOfForwardCone, concernedControlPoint); }
                    if (backwardCone_hasChanged) { Update_progress0to1_ofPlusButtonPosition_inUpcomingBezierSegment(pos0to1_insideSegment_ofPlusButton_afterDragOfBackwardCone, concernedControlPoint); }

                    bool buttonHasBeenClicked = InternalDXXL_BezierHandles.PlusButton(posOfPlusButton_beforeDrag_inUnitsOfGlobalSpace, bezierSplineDrawer_unserializedMonoB.handleSizeOf_plusButtons, bezierSplineDrawer_unserializedMonoB.color_ofAnchorPoints, sceneViewCamForward_normalized, sceneViewCamUp_normalized, sceneViewCamRight_normalized, plusSymbolIcon);
                    if (buttonHasBeenClicked)
                    {
                        bezierSplineDrawer_unserializedMonoB.CreateNewControlPoint_somewhereOnUpcomingSplineSegment(i);
                    }
                }
            }
        }

        void Get_posOfPlusButtonCones(out Vector3 posOf_forwardCone_beforeDrag_inUnitsOfGlobalSpace, out Vector3 posOf_backwardCone_beforeDrag_inUnitsOfGlobalSpace, Vector3 posOfPlusButton_beforeDrag_inUnitsOfGlobalSpace, Vector2 directionOfConeForShifting_inUnitsOfGlobalSpace_normalized_asV2, float handleSize)
        {
            Vector3 directionOfConeForShifting_inUnitsOfGlobalSpace_normalized = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(directionOfConeForShifting_inUnitsOfGlobalSpace_normalized_asV2);

            UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out Vector3 unused1, out Vector3 unused2, out Vector3 unused3, out Vector3 sceneViewCam_to_plusButton, posOfPlusButton_beforeDrag_inUnitsOfGlobalSpace, DrawBasics.CameraForAutomaticOrientation.sceneViewCamera);
            float acuteAngleDeg0to90_betweenObserverViewDir_andCurveTangent = UtilitiesDXXL_Math.AcuteAngle_0to90(sceneViewCam_to_plusButton, directionOfConeForShifting_inUnitsOfGlobalSpace_normalized);
            acuteAngleDeg0to90_betweenObserverViewDir_andCurveTangent = Mathf.Max(acuteAngleDeg0to90_betweenObserverViewDir_andCurveTangent, 15.0f);

            float offsetFactor_toCompensateViewAngle_andMakeConesStillPeekOutBehindPlusButton = 1.0f / Mathf.Sin(acuteAngleDeg0to90_betweenObserverViewDir_andCurveTangent * Mathf.Deg2Rad);
            float offsetFactor_toCompensateViewAngle_andMakeConesStillPeekOutBehindPlusButton_ofForwardConeHandle;
            float offsetFactor_toCompensateViewAngle_andMakeConesStillPeekOutBehindPlusButton_ofBackwardConeHandle;

            bool curveDirectionGoesAwayFromSceneViewCamera = (Vector3.Dot(sceneViewCam_to_plusButton, directionOfConeForShifting_inUnitsOfGlobalSpace_normalized) > 0.0f);
            if (curveDirectionGoesAwayFromSceneViewCamera)
            {
                offsetFactor_toCompensateViewAngle_andMakeConesStillPeekOutBehindPlusButton_ofForwardConeHandle = 1.0f;
                offsetFactor_toCompensateViewAngle_andMakeConesStillPeekOutBehindPlusButton_ofBackwardConeHandle = offsetFactor_toCompensateViewAngle_andMakeConesStillPeekOutBehindPlusButton;
            }
            else
            {
                offsetFactor_toCompensateViewAngle_andMakeConesStillPeekOutBehindPlusButton_ofForwardConeHandle = offsetFactor_toCompensateViewAngle_andMakeConesStillPeekOutBehindPlusButton;
                offsetFactor_toCompensateViewAngle_andMakeConesStillPeekOutBehindPlusButton_ofBackwardConeHandle = 1.0f;
            }

            float relConeOffset_ifObservingCamViesPerp = 0.75f;
            float offsetOfForwardConeHandle = handleSize * relConeOffset_ifObservingCamViesPerp * offsetFactor_toCompensateViewAngle_andMakeConesStillPeekOutBehindPlusButton_ofForwardConeHandle;
            float offsetOfBackwardConeHandle = handleSize * relConeOffset_ifObservingCamViesPerp * offsetFactor_toCompensateViewAngle_andMakeConesStillPeekOutBehindPlusButton_ofBackwardConeHandle;

            posOf_forwardCone_beforeDrag_inUnitsOfGlobalSpace = posOfPlusButton_beforeDrag_inUnitsOfGlobalSpace + directionOfConeForShifting_inUnitsOfGlobalSpace_normalized * offsetOfForwardConeHandle;
            posOf_backwardCone_beforeDrag_inUnitsOfGlobalSpace = posOfPlusButton_beforeDrag_inUnitsOfGlobalSpace - directionOfConeForShifting_inUnitsOfGlobalSpace_normalized * offsetOfBackwardConeHandle;
        }

        void Update_progress0to1_ofPlusButtonPosition_inUpcomingBezierSegment(float pos0to1_insideSegment_ofPlusButton_afterDrag, InternalDXXL_BezierControlPointTriplet2D concernedControlPoint)
        {
            pos0to1_insideSegment_ofPlusButton_afterDrag = Mathf.Clamp(pos0to1_insideSegment_ofPlusButton_afterDrag, 0.05f, 0.95f);
            concernedControlPoint.progress0to1_ofPlusButtonPosition_inUpcomingBezierSegment = pos0to1_insideSegment_ofPlusButton_afterDrag;
        }

        BezierSplineDrawerInspector.PartOfTripleHandleCompound nearest_anchorSubHandle;
        BezierSplineDrawerInspector.PartOfTripleHandleCompound middle_anchorSubHandle;
        BezierSplineDrawerInspector.PartOfTripleHandleCompound farest_anchorSubHandle;

        void DrawCustomHandlesAtSubPoints(int i)
        {
            Determine_nearestMiddleFarestSubHandle(i);

            DrawCustomHandleAtASubPoint(farest_anchorSubHandle, i);
            DrawCustomHandleAtASubPoint(middle_anchorSubHandle, i);
            DrawCustomHandleAtASubPoint(nearest_anchorSubHandle, i);
        }

        void Determine_nearestMiddleFarestSubHandle(int i)
        {
            float dotProduct_camViewDir_directionOfForwardCone = Vector3.Dot(sceneViewCam_to_anchorPoint, bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.directionForHandles_forwardCone_inUnitsOfGlobalSpace_normalized);
            float dotProduct_camViewDir_directionOfBackwardCone = Vector3.Dot(sceneViewCam_to_anchorPoint, bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.directionForHandles_backwardCone_inUnitsOfGlobalSpace_normalized);

            if ((dotProduct_camViewDir_directionOfForwardCone > 0.0f) && (dotProduct_camViewDir_directionOfBackwardCone > 0.0f))
            {
                nearest_anchorSubHandle = BezierSplineDrawerInspector.PartOfTripleHandleCompound.center;
                if (dotProduct_camViewDir_directionOfForwardCone > dotProduct_camViewDir_directionOfBackwardCone)
                {
                    middle_anchorSubHandle = BezierSplineDrawerInspector.PartOfTripleHandleCompound.backward;
                    farest_anchorSubHandle = BezierSplineDrawerInspector.PartOfTripleHandleCompound.forward;
                }
                else
                {
                    middle_anchorSubHandle = BezierSplineDrawerInspector.PartOfTripleHandleCompound.forward;
                    farest_anchorSubHandle = BezierSplineDrawerInspector.PartOfTripleHandleCompound.backward;
                }
            }
            else
            {
                if ((dotProduct_camViewDir_directionOfForwardCone < 0.0f) && (dotProduct_camViewDir_directionOfBackwardCone < 0.0f))
                {
                    farest_anchorSubHandle = BezierSplineDrawerInspector.PartOfTripleHandleCompound.center;
                    if (dotProduct_camViewDir_directionOfForwardCone < dotProduct_camViewDir_directionOfBackwardCone)
                    {
                        nearest_anchorSubHandle = BezierSplineDrawerInspector.PartOfTripleHandleCompound.forward;
                        middle_anchorSubHandle = BezierSplineDrawerInspector.PartOfTripleHandleCompound.backward;
                    }
                    else
                    {
                        nearest_anchorSubHandle = BezierSplineDrawerInspector.PartOfTripleHandleCompound.backward;
                        middle_anchorSubHandle = BezierSplineDrawerInspector.PartOfTripleHandleCompound.forward;
                    }
                }
                else
                {
                    middle_anchorSubHandle = BezierSplineDrawerInspector.PartOfTripleHandleCompound.center;
                    if (dotProduct_camViewDir_directionOfForwardCone < 0.0f)
                    {
                        nearest_anchorSubHandle = BezierSplineDrawerInspector.PartOfTripleHandleCompound.forward;
                        farest_anchorSubHandle = BezierSplineDrawerInspector.PartOfTripleHandleCompound.backward;
                    }
                    else
                    {
                        nearest_anchorSubHandle = BezierSplineDrawerInspector.PartOfTripleHandleCompound.backward;
                        farest_anchorSubHandle = BezierSplineDrawerInspector.PartOfTripleHandleCompound.forward;
                    }
                }
            }
        }

        void DrawCustomHandleAtASubPoint(BezierSplineDrawerInspector.PartOfTripleHandleCompound subHandleToDraw, int i)
        {
            switch (subHandleToDraw)
            {
                case BezierSplineDrawerInspector.PartOfTripleHandleCompound.center:
                    DrawAnchorPointsCustomHandle(i);
                    break;
                case BezierSplineDrawerInspector.PartOfTripleHandleCompound.forward:
                    DrawAHelperPointHandle(i, true);
                    break;
                case BezierSplineDrawerInspector.PartOfTripleHandleCompound.backward:
                    DrawAHelperPointHandle(i, false);
                    break;
                default:
                    break;
            }
        }

        void DrawAnchorPointsCustomHandle(int i)
        {
            if (bezierSplineDrawer_unserializedMonoB.showCustomHandleFor_anchorPoints)
            {
                Handles.color = bezierSplineDrawer_unserializedMonoB.color_ofAnchorPoints;

                float size_ofAnchorPointsCustomHandle = bezierSplineDrawer_unserializedMonoB.handleSizeOf_customHandle_atAnchors * HandleUtility.GetHandleSize(bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos());

                bool drawForwardCone_ofAnchorPointsCustomHandle = CheckIf_drawForwardCone_ofAnchorPointsCustomHandle(i);
                bool drawBackwardCone_ofAnchorPointsCustomHandle = CheckIf_drawBackwardCone_ofAnchorPointsCustomHandle(i);

                if (drawForwardCone_ofAnchorPointsCustomHandle) { TryRecalcHandleDirectionOfForwardConeOnAnchorHandle(i); }
                if (drawBackwardCone_ofAnchorPointsCustomHandle) { TryRecalcHandleDirectionOfBackwardConeOnAnchorHandle(i); }

                bool flipBackwardCone_dueToIsLastControlPointOfNonClosedSpline = FlipDirectionOfAnchorPointsCustomHandleBackwardCone(i);
                if (flipBackwardCone_dueToIsLastControlPointOfNonClosedSpline)
                {
                    //-> "forward cone" (whichever of the three handles it may be) is always skipped here
                    //-> so this just flips the draw order of the remaining two subHandles: sphere and the backward cone
                    DrawASubHandle_ofAnchorPointsCustomHandle(nearest_anchorSubHandle, i, drawForwardCone_ofAnchorPointsCustomHandle, drawBackwardCone_ofAnchorPointsCustomHandle, size_ofAnchorPointsCustomHandle);
                    DrawASubHandle_ofAnchorPointsCustomHandle(middle_anchorSubHandle, i, drawForwardCone_ofAnchorPointsCustomHandle, drawBackwardCone_ofAnchorPointsCustomHandle, size_ofAnchorPointsCustomHandle);
                    DrawASubHandle_ofAnchorPointsCustomHandle(farest_anchorSubHandle, i, drawForwardCone_ofAnchorPointsCustomHandle, drawBackwardCone_ofAnchorPointsCustomHandle, size_ofAnchorPointsCustomHandle);
                }
                else
                {
                    DrawASubHandle_ofAnchorPointsCustomHandle(farest_anchorSubHandle, i, drawForwardCone_ofAnchorPointsCustomHandle, drawBackwardCone_ofAnchorPointsCustomHandle, size_ofAnchorPointsCustomHandle);
                    DrawASubHandle_ofAnchorPointsCustomHandle(middle_anchorSubHandle, i, drawForwardCone_ofAnchorPointsCustomHandle, drawBackwardCone_ofAnchorPointsCustomHandle, size_ofAnchorPointsCustomHandle);
                    DrawASubHandle_ofAnchorPointsCustomHandle(nearest_anchorSubHandle, i, drawForwardCone_ofAnchorPointsCustomHandle, drawBackwardCone_ofAnchorPointsCustomHandle, size_ofAnchorPointsCustomHandle);
                }
            }
        }

        bool CheckIf_drawForwardCone_ofAnchorPointsCustomHandle(int i)
        {
            if ((bezierSplineDrawer_unserializedMonoB.gapFromEndToStart_isClosed == false) && bezierSplineDrawer_unserializedMonoB.IsLastControlPoint(i))
            {
                return false;
            }
            return true;
        }

        void TryRecalcHandleDirectionOfForwardConeOnAnchorHandle(int i)
        {
            if (bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.recalc_directionForHandles_forwardCone_duringNextOnSceneGUI)
            {
                if (bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].forwardHelperPoint.isUsed)
                {
                    bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.directionForHandles_forwardCone_inUnitsOfGlobalSpace_normalized = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.Get_direction_toForward_inUnitsOfGlobalSpace_normalized();
                }
                else
                {
                    InternalDXXL_BezierControlSubPoint2D nextUsedNonSuperimposedSubPointAlongSplineDir = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.GetNextUsedNonSuperimposedSubPointAlongSplineDir(false);
                    if (nextUsedNonSuperimposedSubPointAlongSplineDir != null)
                    {
                        bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.directionForHandles_forwardCone_inUnitsOfGlobalSpace_normalized = (nextUsedNonSuperimposedSubPointAlongSplineDir.GetPos_inUnitsOfGlobalSpace() - bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.GetPos_inUnitsOfGlobalSpace()).normalized;
                    }
                    else
                    {
                        bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.directionForHandles_forwardCone_inUnitsOfGlobalSpace_normalized = bezierSplineDrawer_unserializedMonoB.Get_right_ofActiveDrawSpace_inUnitsOfGlobalSpace_normalized();
                    }
                }
            }
        }

        bool CheckIf_drawBackwardCone_ofAnchorPointsCustomHandle(int i)
        {
            if ((bezierSplineDrawer_unserializedMonoB.gapFromEndToStart_isClosed == false) && (bezierSplineDrawer_unserializedMonoB.IsFirstControlPoint(i)))
            {
                return false;
            }

            //note 1: for non-kinked juncture types the backwardHelper cannot be disabled (meaning "isUsed == true" is always guaranteed). Therefore the backward direction cone is always redundant and can be skipped, because it is the same as the forward direction.
            //note 2: the one case, where no forward direction for non-kinked juncture-types would be possible is the splineEnd of non-closed splines. But such spline end points are always forced to "juncture=kinked", and therefore don't need special treatment here 
            return (bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.junctureType == InternalDXXL_BezierControlAnchorSubPoint.JunctureType.kinked);
        }

        void TryRecalcHandleDirectionOfBackwardConeOnAnchorHandle(int i)
        {
            if (bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.recalc_directionForHandles_backwardCone_duringNextOnSceneGUI)
            {
                if (bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].backwardHelperPoint.isUsed)
                {
                    bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.directionForHandles_backwardCone_inUnitsOfGlobalSpace_normalized = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.Get_direction_toBackward_inUnitsOfGlobalSpace_normalized();
                }
                else
                {
                    InternalDXXL_BezierControlSubPoint2D previousUsedNonSuperimposedSubPointAlongSplineDir = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.GetPreviousUsedNonSuperimposedSubPointAlongSplineDir(false);
                    if (previousUsedNonSuperimposedSubPointAlongSplineDir != null)
                    {
                        bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.directionForHandles_backwardCone_inUnitsOfGlobalSpace_normalized = (previousUsedNonSuperimposedSubPointAlongSplineDir.GetPos_inUnitsOfGlobalSpace() - bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.GetPos_inUnitsOfGlobalSpace()).normalized;
                    }
                    else
                    {
                        bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.directionForHandles_backwardCone_inUnitsOfGlobalSpace_normalized = (-bezierSplineDrawer_unserializedMonoB.Get_right_ofActiveDrawSpace_inUnitsOfGlobalSpace_normalized());
                    }
                }
            }
        }

        void DrawASubHandle_ofAnchorPointsCustomHandle(BezierSplineDrawerInspector.PartOfTripleHandleCompound subHandleToDraw, int i, bool drawForwardCone_ofAnchorPointsCustomHandle, bool drawBackwardCone_ofAnchorPointsCustomHandle, float size_ofAnchorPointsCustomHandle)
        {
            switch (subHandleToDraw)
            {
                case BezierSplineDrawerInspector.PartOfTripleHandleCompound.center:
                    DrawAnchorPointsCustomHandles_freeMoveSubHandle(i, size_ofAnchorPointsCustomHandle);
                    break;
                case BezierSplineDrawerInspector.PartOfTripleHandleCompound.forward:
                    if (drawForwardCone_ofAnchorPointsCustomHandle) { DrawAnchorPointsCustomHandles_forwardConeSubHandle(i, size_ofAnchorPointsCustomHandle); }
                    break;
                case BezierSplineDrawerInspector.PartOfTripleHandleCompound.backward:
                    if (drawBackwardCone_ofAnchorPointsCustomHandle) { DrawAnchorPointsCustomHandles_backwardConeSubHandle(i, size_ofAnchorPointsCustomHandle); }
                    break;
                default:
                    break;
            }
        }

        void DrawAnchorPointsCustomHandles_freeMoveSubHandle(int i, float size_ofAnchorPointsCustomHandle)
        {
            Start_handlesChangeCheck();
            Vector3 pos_shiftedByFreeMoveHandle_inUnitsOfGlobalSpace = Handles.Slider2D(bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.controlID_ofCustomHandles_sphere, bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), Vector3.back, Vector3.up, Vector3.right, size_ofAnchorPointsCustomHandle, Handles.SphereHandleCap, Vector2.one);
            bool hasChanged = End_handlesChangeCheck("Position of Bezier Point", i, true);

            if (hasChanged)
            {
                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.SetPos_inUnitsOfGlobalSpace(pos_shiftedByFreeMoveHandle_inUnitsOfGlobalSpace, true, null);
            }
        }

        void DrawAnchorPointsCustomHandles_forwardConeSubHandle(int i, float size_ofAnchorPointsCustomHandle)
        {
            Vector3 posOffset_fromAnchorPoint_inUnitsOfGlobalSpace = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.directionForHandles_forwardCone_inUnitsOfGlobalSpace_normalized * size_ofAnchorPointsCustomHandle;
            Vector3 pos_ofForwardCone_beforeDrag_inUnitsOfGlobalSpace = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos() + posOffset_fromAnchorPoint_inUnitsOfGlobalSpace;

            Start_handlesChangeCheck();
            Vector3 pos_ofForwardCone_afterDrag_inUnitsOfGlobalSpace = Handles.Slider(bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.controlID_ofCustomHandles_forwardCone, pos_ofForwardCone_beforeDrag_inUnitsOfGlobalSpace, bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.directionForHandles_forwardCone_inUnitsOfGlobalSpace_normalized, size_ofAnchorPointsCustomHandle, Handles.ConeHandleCap, 1.0f);
            bool hasChanged = End_handlesChangeCheck("Position of Bezier Point", i, true);

            if (hasChanged)
            {
                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.recalc_directionForHandles_forwardCone_duringNextOnSceneGUI = false; //Without this there is strange flickering behaviour, see notes at "recalc_globalRotation_ofPositionHandle_duringNextOnSceneGUI = false"
                Vector3 posDifference_inUnitsOfGlobalSpace = pos_ofForwardCone_afterDrag_inUnitsOfGlobalSpace - pos_ofForwardCone_beforeDrag_inUnitsOfGlobalSpace;
                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.AddPosOffset_inUnitsOfGlobalSpace(posDifference_inUnitsOfGlobalSpace, true, null);
            }
        }

        void DrawAnchorPointsCustomHandles_backwardConeSubHandle(int i, float size_ofAnchorPointsCustomHandle)
        {
            bool flipDirectionOfCone = FlipDirectionOfAnchorPointsCustomHandleBackwardCone(i);
            Vector3 posOffset_fromAnchorPoint_inUnitsOfGlobalSpace = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.directionForHandles_backwardCone_inUnitsOfGlobalSpace_normalized * size_ofAnchorPointsCustomHandle;
            if (flipDirectionOfCone) { posOffset_fromAnchorPoint_inUnitsOfGlobalSpace = -posOffset_fromAnchorPoint_inUnitsOfGlobalSpace; }
            Vector3 pos_ofBackwardCone_beforeDrag_inUnitsOfGlobalSpace = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos() + posOffset_fromAnchorPoint_inUnitsOfGlobalSpace;
            Vector3 usedDirection_inUnitsOfGlobalSpace = flipDirectionOfCone ? (-bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.directionForHandles_backwardCone_inUnitsOfGlobalSpace_normalized) : bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.directionForHandles_backwardCone_inUnitsOfGlobalSpace_normalized;

            Start_handlesChangeCheck();
            Vector3 pos_ofBackwardCone_afterDrag_inUnitsOfGlobalSpace = Handles.Slider(bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.controlID_ofCustomHandles_backwardCone, pos_ofBackwardCone_beforeDrag_inUnitsOfGlobalSpace, usedDirection_inUnitsOfGlobalSpace, size_ofAnchorPointsCustomHandle, Handles.ConeHandleCap, 1.0f);
            bool hasChanged = End_handlesChangeCheck("Position of Bezier Point", i, true);

            if (hasChanged)
            {
                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.recalc_directionForHandles_backwardCone_duringNextOnSceneGUI = false; //Without this there is strange flickering behaviour, see notes at "recalc_globalRotation_ofPositionHandle_duringNextOnSceneGUI = false"
                Vector3 posDifference_inUnitsOfGlobalSpace = pos_ofBackwardCone_afterDrag_inUnitsOfGlobalSpace - pos_ofBackwardCone_beforeDrag_inUnitsOfGlobalSpace;
                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.AddPosOffset_inUnitsOfGlobalSpace(posDifference_inUnitsOfGlobalSpace, true, null);
            }
        }

        bool FlipDirectionOfAnchorPointsCustomHandleBackwardCone(int i)
        {
            //-> this is only for visual continuity (because the default case is that the conesAtAnchorHandles point along forwardOfSpline). 
            if (bezierSplineDrawer_unserializedMonoB.gapFromEndToStart_isClosed == false)
            {
                if (bezierSplineDrawer_unserializedMonoB.IsLastControlPoint(i))
                {
                    return true;
                }
            }
            return false;
        }

        BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle nearest_helperSubHandle;
        BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle secondNearest_helperSubHandle;
        BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle middle_helperSubHandle;
        BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle secondFarest_helperSubHandle;
        BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle farest_helperSubHandle;
        bool nearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints;
        bool secondNearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints;
        bool secondFarest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints;
        bool farest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints;
        void DrawAHelperPointHandle(int i, bool helperHandleToDraw_isForwardNotBackward)
        {
            if (bezierSplineDrawer_unserializedMonoB.showCustomHandleFor_helperPoints)
            {
                InternalDXXL_BezierControlHelperSubPoint2D concernedHelperPoint = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].GetAHelperPoint(helperHandleToDraw_isForwardNotBackward);
                if (concernedHelperPoint.isUsed)
                {
                    Handles.color = bezierSplineDrawer_unserializedMonoB.color_ofHelperPoints;

                    bool draw_helperPointsCustomHandles_cylinderAlongHelperDirFromAnchor = CheckIf_draw_helperPointsCustomHandles_cylinderAlongHelperDirFromAnchor(i, concernedHelperPoint);
                    bool draw_helperPointsCustomHandles_cylinderAlongToNeighborsHelperDir = CheckIf_draw_helperPointsCustomHandles_cylinderAlongToNeighborsHelperDir(i, concernedHelperPoint);

                    UtilitiesDXXL_ObserverCamera.GetObserverCamSpecs(out Vector3 observerCamForward_normalized, out Vector3 observerCamUp_normalized, out Vector3 observerCamRight_normalized, out Vector3 cam_to_helperPosition, concernedHelperPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), DrawBasics.CameraForAutomaticOrientation.sceneViewCamera);

                    TryRecalc_directionForHandles_alongLineWithMountingAnchor_toForwardDirOfWholeSpline_inUnitsOfGlobalSpace_normalized(concernedHelperPoint);
                    TryRecalc_directionForHandles_alongLineWithNeighboringHelperOfNeighboringControlPoint_toForwardDirOfWholeSpline_inUnitsOfGlobalSpace_normalized(i, concernedHelperPoint);

                    Determine_nearestMiddleAndFarestSubHandles_ofHelperPointsCustomHandle(concernedHelperPoint, cam_to_helperPosition);

                    DrawASubHandle_ofHelperPointsCustomHandle(farest_helperSubHandle, farest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints, i, draw_helperPointsCustomHandles_cylinderAlongHelperDirFromAnchor, draw_helperPointsCustomHandles_cylinderAlongToNeighborsHelperDir, concernedHelperPoint);
                    DrawASubHandle_ofHelperPointsCustomHandle(secondFarest_helperSubHandle, secondFarest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints, i, draw_helperPointsCustomHandles_cylinderAlongHelperDirFromAnchor, draw_helperPointsCustomHandles_cylinderAlongToNeighborsHelperDir, concernedHelperPoint);
                    DrawASubHandle_ofHelperPointsCustomHandle(middle_helperSubHandle, false, i, draw_helperPointsCustomHandles_cylinderAlongHelperDirFromAnchor, draw_helperPointsCustomHandles_cylinderAlongToNeighborsHelperDir, concernedHelperPoint);
                    DrawASubHandle_ofHelperPointsCustomHandle(secondNearest_helperSubHandle, secondNearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints, i, draw_helperPointsCustomHandles_cylinderAlongHelperDirFromAnchor, draw_helperPointsCustomHandles_cylinderAlongToNeighborsHelperDir, concernedHelperPoint);
                    DrawASubHandle_ofHelperPointsCustomHandle(nearest_helperSubHandle, nearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints, i, draw_helperPointsCustomHandles_cylinderAlongHelperDirFromAnchor, draw_helperPointsCustomHandles_cylinderAlongToNeighborsHelperDir, concernedHelperPoint);
                }
            }
        }

        bool CheckIf_draw_helperPointsCustomHandles_cylinderAlongHelperDirFromAnchor(int i, InternalDXXL_BezierControlHelperSubPoint2D concernedHelperPoint)
        {
            if (concernedHelperPoint.GetMountingAnchorPoint().junctureType == InternalDXXL_BezierControlAnchorSubPoint.JunctureType.mirrored)
            {
                return false;
            }
            else
            {
                return (concernedHelperPoint.GetOppositeHelperPoint().isUsed);
            }
        }

        bool CheckIf_draw_helperPointsCustomHandles_cylinderAlongToNeighborsHelperDir(int i, InternalDXXL_BezierControlHelperSubPoint2D concernedHelperPoint)
        {
            InternalDXXL_BezierControlPointTriplet2D neighboringControlPoint = concernedHelperPoint.Get_neighboringControlPoint(false);
            if (neighboringControlPoint != null)
            {
                if (concernedHelperPoint.isForward_notBackward)
                {
                    return neighboringControlPoint.backwardHelperPoint.isUsed;
                }
                else
                {
                    return neighboringControlPoint.forwardHelperPoint.isUsed;
                }
            }
            else
            {
                return false;
            }
        }

        void TryRecalc_directionForHandles_alongLineWithMountingAnchor_toForwardDirOfWholeSpline_inUnitsOfGlobalSpace_normalized(InternalDXXL_BezierControlHelperSubPoint2D concernedHelperPoint)
        {
            if (concernedHelperPoint.recalc_directionForHandles_alongLineWithMountingAnchor_duringNextOnSceneGUI)
            {
                if (concernedHelperPoint.isForward_notBackward)
                {
                    concernedHelperPoint.directionForHandles_alongLineWithMountingAnchor_toForwardDirOfWholeSpline_inUnitsOfGlobalSpace_normalized = concernedHelperPoint.Get_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized();
                }
                else
                {
                    concernedHelperPoint.directionForHandles_alongLineWithMountingAnchor_toForwardDirOfWholeSpline_inUnitsOfGlobalSpace_normalized = -concernedHelperPoint.Get_direction_fromMountingAnchorToThisHelperPoint_inUnitsOfGlobalSpace_normalized();
                }
            }
        }

        void TryRecalc_directionForHandles_alongLineWithNeighboringHelperOfNeighboringControlPoint_toForwardDirOfWholeSpline_inUnitsOfGlobalSpace_normalized(int i, InternalDXXL_BezierControlHelperSubPoint2D concernedHelperPoint)
        {
            if (concernedHelperPoint.recalc_directionForHandles_alongLineWithNeighboringHelperOfNeighboringControlPoint_duringNextOnSceneGUI)
            {
                if (concernedHelperPoint.isForward_notBackward)
                {
                    InternalDXXL_BezierControlSubPoint2D nextUsedNonSuperimposedSubPointAlongSplineDir = concernedHelperPoint.GetNextUsedNonSuperimposedSubPointAlongSplineDir(false);

                    if (nextUsedNonSuperimposedSubPointAlongSplineDir != null)
                    {
                        concernedHelperPoint.directionForHandles_alongLineWithNeighboringHelperOfNeighboringControlPoint_toForwardDirOfWholeSpline_inUnitsOfGlobalSpace_normalized = (nextUsedNonSuperimposedSubPointAlongSplineDir.GetPos_inUnitsOfGlobalSpace_asV3DrawPos() - concernedHelperPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos()).normalized;
                    }
                    else
                    {
                        concernedHelperPoint.directionForHandles_alongLineWithNeighboringHelperOfNeighboringControlPoint_toForwardDirOfWholeSpline_inUnitsOfGlobalSpace_normalized = bezierSplineDrawer_unserializedMonoB.Get_right_ofActiveDrawSpace_inUnitsOfGlobalSpace_normalized();
                    }
                }
                else
                {
                    InternalDXXL_BezierControlSubPoint2D previousUsedNonSuperimposedSubPointAlongSplineDir = concernedHelperPoint.GetPreviousUsedNonSuperimposedSubPointAlongSplineDir(false);
                    if (previousUsedNonSuperimposedSubPointAlongSplineDir != null)
                    {
                        concernedHelperPoint.directionForHandles_alongLineWithNeighboringHelperOfNeighboringControlPoint_toForwardDirOfWholeSpline_inUnitsOfGlobalSpace_normalized = (concernedHelperPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos() - previousUsedNonSuperimposedSubPointAlongSplineDir.GetPos_inUnitsOfGlobalSpace_asV3DrawPos()).normalized;
                    }
                    else
                    {
                        concernedHelperPoint.directionForHandles_alongLineWithNeighboringHelperOfNeighboringControlPoint_toForwardDirOfWholeSpline_inUnitsOfGlobalSpace_normalized = bezierSplineDrawer_unserializedMonoB.Get_right_ofActiveDrawSpace_inUnitsOfGlobalSpace_normalized();
                    }
                }
            }
        }

        void Determine_nearestMiddleAndFarestSubHandles_ofHelperPointsCustomHandle(InternalDXXL_BezierControlHelperSubPoint2D concernedHelperPoint, Vector3 cam_to_helperPosition)
        {
            middle_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.sphere;

            float dotProduct_camViewDir_directionOfConeAlongHelperDirFromAnchor = Vector3.Dot(cam_to_helperPosition, concernedHelperPoint.directionForHandles_alongLineWithMountingAnchor_toForwardDirOfWholeSpline_inUnitsOfGlobalSpace_normalized);
            float dotProduct_camViewDir_directionOfConeAlongToNeighborsHelperDir = Vector3.Dot(cam_to_helperPosition, concernedHelperPoint.directionForHandles_alongLineWithNeighboringHelperOfNeighboringControlPoint_toForwardDirOfWholeSpline_inUnitsOfGlobalSpace_normalized);
            if ((dotProduct_camViewDir_directionOfConeAlongHelperDirFromAnchor > 0.0f) && (dotProduct_camViewDir_directionOfConeAlongToNeighborsHelperDir > 0.0f))
            {
                //both dirs point AWAY from camera:
                if (dotProduct_camViewDir_directionOfConeAlongHelperDirFromAnchor > dotProduct_camViewDir_directionOfConeAlongToNeighborsHelperDir)
                {
                    //"dirAlongLineWithAnchor" points STEEPER away from camera than "dir_toNeighborOfNeighbor":
                    nearest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.cylinderAlongHelperDirFromAnchor;
                    secondNearest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.cylinderAlongToNeighborsHelperDir;
                    secondFarest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.coneAlongToNeighborsHelperDir;
                    farest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.coneAlongHelperDirFromAnchor;

                    nearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = true;
                    secondNearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = false;
                    secondFarest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = false;
                    farest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = true;
                }
                else
                {
                    //"dir_toNeighborOfNeighbor" points STEEPER away from camera than "dirAlongLineWithAnchor":
                    nearest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.cylinderAlongToNeighborsHelperDir;
                    secondNearest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.cylinderAlongHelperDirFromAnchor;
                    secondFarest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.coneAlongHelperDirFromAnchor;
                    farest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.coneAlongToNeighborsHelperDir;

                    nearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = false;
                    secondNearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = true;
                    secondFarest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = true;
                    farest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = false;
                }
            }
            else
            {
                if ((dotProduct_camViewDir_directionOfConeAlongHelperDirFromAnchor < 0.0f) && (dotProduct_camViewDir_directionOfConeAlongToNeighborsHelperDir < 0.0f))
                {
                    //both dirs point TOWARDS camera:
                    if (dotProduct_camViewDir_directionOfConeAlongHelperDirFromAnchor < dotProduct_camViewDir_directionOfConeAlongToNeighborsHelperDir)
                    {
                        //"dirAlongLineWithAnchor" points STEEPER towards camera than "dir_toNeighborOfNeighbor":
                        nearest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.coneAlongHelperDirFromAnchor;
                        secondNearest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.coneAlongToNeighborsHelperDir;
                        secondFarest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.cylinderAlongToNeighborsHelperDir;
                        farest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.cylinderAlongHelperDirFromAnchor;

                        nearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = true;
                        secondNearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = false;
                        secondFarest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = false;
                        farest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = true;
                    }
                    else
                    {
                        //"dir_toNeighborOfNeighbor" points STEEPER towards camera than "dirAlongLineWithAnchor":
                        nearest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.coneAlongToNeighborsHelperDir;
                        secondNearest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.coneAlongHelperDirFromAnchor;
                        secondFarest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.cylinderAlongHelperDirFromAnchor;
                        farest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.cylinderAlongToNeighborsHelperDir;

                        nearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = false;
                        secondNearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = true;
                        secondFarest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = true;
                        farest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = false;
                    }
                }
                else
                {
                    if ((dotProduct_camViewDir_directionOfConeAlongHelperDirFromAnchor > 0.0f) && (dotProduct_camViewDir_directionOfConeAlongToNeighborsHelperDir < 0.0f))
                    {
                        //"dirAlongLineWithAnchor" points AWAY from camera, "dir_toNeighborOfNeighbor" points TOWARDS camera:
                        float abs_dotProduct_camViewDir_directionOfConeAlongToNeighborsHelperDir = -dotProduct_camViewDir_directionOfConeAlongToNeighborsHelperDir;
                        if (dotProduct_camViewDir_directionOfConeAlongHelperDirFromAnchor > abs_dotProduct_camViewDir_directionOfConeAlongToNeighborsHelperDir)
                        {
                            //"dirAlongLineWithAnchor" points STEEPER away from camera than "(cylinder)dir_toNeighborOfNeighbor":
                            nearest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.cylinderAlongHelperDirFromAnchor;
                            secondNearest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.coneAlongToNeighborsHelperDir;
                            secondFarest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.cylinderAlongToNeighborsHelperDir;
                            farest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.coneAlongHelperDirFromAnchor;

                            nearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = true;
                            secondNearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = false;
                            secondFarest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = false;
                            farest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = true;
                        }
                        else
                        {
                            //"(cylinder)dir_toNeighborOfNeighbor" points STEEPER away from camera than "dirAlongLineWithAnchor":
                            nearest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.coneAlongToNeighborsHelperDir;
                            secondNearest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.cylinderAlongHelperDirFromAnchor;
                            secondFarest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.coneAlongHelperDirFromAnchor;
                            farest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.cylinderAlongToNeighborsHelperDir;

                            nearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = false;
                            secondNearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = true;
                            secondFarest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = true;
                            farest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = false;
                        }
                    }
                    else
                    {
                        //"dirAlongLineWithAnchor" points TOWARDS camera, "dir_toNeighborOfNeighbor" points AWAY from camera:
                        float abs_dotProduct_camViewDir_directionOfConeAlongHelperDirFromAnchor = -dotProduct_camViewDir_directionOfConeAlongHelperDirFromAnchor;
                        if (abs_dotProduct_camViewDir_directionOfConeAlongHelperDirFromAnchor > dotProduct_camViewDir_directionOfConeAlongToNeighborsHelperDir)
                        {
                            //"(cylinder)dirAlongLineWithAnchor" points STEEPER away from camera than "dir_toNeighborOfNeighbor":
                            nearest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.coneAlongHelperDirFromAnchor;
                            secondNearest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.cylinderAlongToNeighborsHelperDir;
                            secondFarest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.coneAlongToNeighborsHelperDir;
                            farest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.cylinderAlongHelperDirFromAnchor;

                            nearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = true;
                            secondNearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = false;
                            secondFarest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = false;
                            farest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = true;
                        }
                        else
                        {
                            //"dir_toNeighborOfNeighbor" points STEEPER away from camera than "(cylinder)dirAlongLineWithAnchor":
                            nearest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.cylinderAlongToNeighborsHelperDir;
                            secondNearest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.coneAlongHelperDirFromAnchor;
                            secondFarest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.cylinderAlongHelperDirFromAnchor;
                            farest_helperSubHandle = BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.coneAlongToNeighborsHelperDir;

                            nearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = false;
                            secondNearest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = true;
                            secondFarest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = true;
                            farest_helperSubHandle_belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints = false;
                        }
                    }
                }
            }
        }

        void DrawASubHandle_ofHelperPointsCustomHandle(BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle subHandleToDraw, bool belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints, int i, bool draw_helperPointsCustomHandles_cylinderAlongHelperDirFromAnchor, bool draw_helperPointsCustomHandles_cylinderAlongToNeighborsHelperDir, InternalDXXL_BezierControlHelperSubPoint2D concernedHelperPoint)
        {
            float handleSize_unmodified = bezierSplineDrawer_unserializedMonoB.handleSizeOf_customHandle_atHelpers * HandleUtility.GetHandleSize(concernedHelperPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos());
            switch (subHandleToDraw)
            {
                case BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.sphere:
                    DrawHelperPointsCustomHandles_sphereHandle(i, concernedHelperPoint.controlID_ofCustomHandles_sphere, concernedHelperPoint, handleSize_unmodified);
                    break;
                case BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.coneAlongHelperDirFromAnchor:
                    DrawAnUnidirectionalSubHandle_ofHelperPointsCustomHandle(i, concernedHelperPoint.controlID_ofCustomHandles_coneAlongLineWithAnchor, concernedHelperPoint, 1.0f, handleSize_unmodified, Handles.ConeHandleCap, belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints, true, false, false);
                    break;
                case BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.coneAlongToNeighborsHelperDir:
                    DrawAnUnidirectionalSubHandle_ofHelperPointsCustomHandle(i, concernedHelperPoint.controlID_ofCustomHandles_coneAlongLineWithNeighborsHelper, concernedHelperPoint, 1.0f, handleSize_unmodified, Handles.ConeHandleCap, belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints, false, false, false);
                    break;
                case BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.cylinderAlongHelperDirFromAnchor:
                    if (draw_helperPointsCustomHandles_cylinderAlongHelperDirFromAnchor)
                    {
                        DrawAnUnidirectionalSubHandle_ofHelperPointsCustomHandle(i, concernedHelperPoint.controlID_ofCustomHandles_cylinderAlongLineWithAnchor, concernedHelperPoint, -1.0f * BezierSplineDrawerInspector.offsetFactor_forCylinderHandles, BezierSplineDrawerInspector.scaleFactor_forCylinderHandles * handleSize_unmodified, Handles.CylinderHandleCap, belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints, false, true, false);
                    }
                    break;
                case BezierSplineDrawerInspector.PartOfHelperPointsCustomHandle.cylinderAlongToNeighborsHelperDir:
                    if (draw_helperPointsCustomHandles_cylinderAlongToNeighborsHelperDir)
                    {
                        DrawAnUnidirectionalSubHandle_ofHelperPointsCustomHandle(i, concernedHelperPoint.controlID_ofCustomHandles_cylinderAlongLineWithNeighborsHelper, concernedHelperPoint, -1.0f * BezierSplineDrawerInspector.offsetFactor_forCylinderHandles, BezierSplineDrawerInspector.scaleFactor_forCylinderHandles * handleSize_unmodified, Handles.CylinderHandleCap, belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints, false, false, true);
                    }
                    break;
                default:
                    break;
            }
        }

        void DrawHelperPointsCustomHandles_sphereHandle(int i, int controlID_ofHandle, InternalDXXL_BezierControlHelperSubPoint2D concernedHelperPoint, float handleSize)
        {
            Start_handlesChangeCheck();
            Vector3 posOfSphereHandle_afterDrag_shifedInsideCamPlane_inUnitsOfGlobalSpace = Handles.Slider2D(controlID_ofHandle, concernedHelperPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), Vector3.back, Vector3.up, Vector3.right, handleSize, Handles.SphereHandleCap, Vector2.one);
            bool hasChanged = End_handlesChangeCheck("Position of Bezier Point", i, true);

            if (hasChanged)
            {
                concernedHelperPoint.SetPos_inUnitsOfGlobalSpace(posOfSphereHandle_afterDrag_shifedInsideCamPlane_inUnitsOfGlobalSpace, true, null);
            }
        }

        void DrawAnUnidirectionalSubHandle_ofHelperPointsCustomHandle(int i, int controlID_ofHandle, InternalDXXL_BezierControlHelperSubPoint2D concernedHelperPoint, float handlePosOffsetFactorAlongDir, float handleSize, Handles.CapFunction capFunction, bool belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints, bool tryChangeStateOf_helperPointsAreOnSameSide_afterPassingTheAnchorPoint, bool mirrorDistanceChangeRatio_ontoOtherHelperPointOfSameControlPoint, bool mirrorPosChange_ontoNeighboringHelperPointOfNeighboringControlPoint)
        {
            Vector3 dragDirection_inUnitsOfGlobalSpace_normalized = belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints ? concernedHelperPoint.directionForHandles_alongLineWithMountingAnchor_toForwardDirOfWholeSpline_inUnitsOfGlobalSpace_normalized : concernedHelperPoint.directionForHandles_alongLineWithNeighboringHelperOfNeighboringControlPoint_toForwardDirOfWholeSpline_inUnitsOfGlobalSpace_normalized;
            Vector3 handleOffset = dragDirection_inUnitsOfGlobalSpace_normalized * handleSize * handlePosOffsetFactorAlongDir;
            Vector3 posOfHelperPoint_beforeDrag_inUnitsOfGlobalSpace = concernedHelperPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos();
            Vector3 posOfConeHandle_beforeDrag_inUnitsOfGlobalSpace = posOfHelperPoint_beforeDrag_inUnitsOfGlobalSpace + handleOffset;

            Start_handlesChangeCheck();
            Vector3 posOfConeHandle_afterDrag_inUnitsOfGlobalSpace = Handles.Slider(controlID_ofHandle, posOfConeHandle_beforeDrag_inUnitsOfGlobalSpace, dragDirection_inUnitsOfGlobalSpace_normalized, handleSize, capFunction, 1.0f);
            bool hasChanged = End_handlesChangeCheck("Position of Bezier Point", i, true);

            if (hasChanged)
            {
                if (belongsTo_lineFromAnchor_notTo_lineBetweenNeighboringHelperPoints)
                {
                    concernedHelperPoint.recalc_directionForHandles_alongLineWithMountingAnchor_duringNextOnSceneGUI = false;
                }
                else
                {
                    concernedHelperPoint.recalc_directionForHandles_alongLineWithNeighboringHelperOfNeighboringControlPoint_duringNextOnSceneGUI = false;
                }

                Vector3 shiftOffset_throughDragSlider_inUnitsOfGlobalSpace = posOfConeHandle_afterDrag_inUnitsOfGlobalSpace - posOfConeHandle_beforeDrag_inUnitsOfGlobalSpace;
                Vector3 posOfHelperPoint_afterDrag_inUnitsOfGlobalSpace = posOfConeHandle_afterDrag_inUnitsOfGlobalSpace - handleOffset;

                TryChangeStateOf_helperPointsAreOnSameSide_afterPassingTheAnchorPoint(concernedHelperPoint, tryChangeStateOf_helperPointsAreOnSameSide_afterPassingTheAnchorPoint, posOfHelperPoint_beforeDrag_inUnitsOfGlobalSpace, posOfHelperPoint_afterDrag_inUnitsOfGlobalSpace);

                float absDistanceToMountingAnchorPoint_beforeDrag_inUnitsOfGlobalSpace = concernedHelperPoint.Get_absDistanceToAnchorPoint_inUnitsOfGlobalSpace();
                concernedHelperPoint.SetPos_inUnitsOfGlobalSpace(posOfHelperPoint_afterDrag_inUnitsOfGlobalSpace, true, null);
                float absDistanceToMountingAnchorPoint_afterDrag_inUnitsOfGlobalSpace = concernedHelperPoint.Get_absDistanceToAnchorPoint_inUnitsOfGlobalSpace();

                TryMirrorDistanceChangeRatio_ontoOtherHelperPointOfSameControlPoint(concernedHelperPoint, mirrorDistanceChangeRatio_ontoOtherHelperPointOfSameControlPoint, absDistanceToMountingAnchorPoint_beforeDrag_inUnitsOfGlobalSpace, absDistanceToMountingAnchorPoint_afterDrag_inUnitsOfGlobalSpace);
                TryMirrorPosChange_ontoNeighboringHelperPointOfNeighboringControlPoint(i, concernedHelperPoint, mirrorPosChange_ontoNeighboringHelperPointOfNeighboringControlPoint, shiftOffset_throughDragSlider_inUnitsOfGlobalSpace);
            }
        }

        void TryChangeStateOf_helperPointsAreOnSameSide_afterPassingTheAnchorPoint(InternalDXXL_BezierControlHelperSubPoint2D concernedHelperPoint, bool tryChangeStateOf_helperPointsAreOnSameSide_afterPassingTheAnchorPoint, Vector3 posOfHelperPoint_beforeDrag_inUnitsOfGlobalSpace, Vector3 posOfHelperPoint_afterDrag_inUnitsOfGlobalSpace)
        {
            if (tryChangeStateOf_helperPointsAreOnSameSide_afterPassingTheAnchorPoint)
            {
                if (concernedHelperPoint.isUsed && concernedHelperPoint.GetOppositeHelperPoint().isUsed)
                {
                    if (concernedHelperPoint.GetMountingAnchorPoint().junctureType == InternalDXXL_BezierControlAnchorSubPoint.JunctureType.aligned)
                    {
                        Vector2 posOfHelperPoint_beforeDrag_inUnitsOfGlobalSpace_asV2 = posOfHelperPoint_beforeDrag_inUnitsOfGlobalSpace;
                        Vector2 posOfHelperPoint_afterDrag_inUnitsOfGlobalSpace_asV2 = posOfHelperPoint_afterDrag_inUnitsOfGlobalSpace;
                        Vector3 anchor_to_helperPosBeforeDrag = posOfHelperPoint_beforeDrag_inUnitsOfGlobalSpace_asV2 - concernedHelperPoint.GetMountingAnchorPoint().GetPos_inUnitsOfGlobalSpace();
                        Vector3 anchor_to_helperPosAfterDrag = posOfHelperPoint_afterDrag_inUnitsOfGlobalSpace_asV2 - concernedHelperPoint.GetMountingAnchorPoint().GetPos_inUnitsOfGlobalSpace();
                        float dotProduct_ofDirectionFromAnchorToConcernedHelper_beforeAndAfterDrag = Vector3.Dot(anchor_to_helperPosBeforeDrag, anchor_to_helperPosAfterDrag);
                        bool coneSliderPassedTheMountingAnchorPoint = (dotProduct_ofDirectionFromAnchorToConcernedHelper_beforeAndAfterDrag < 0.0f);
                        if (coneSliderPassedTheMountingAnchorPoint)
                        {
                            concernedHelperPoint.Get_controlPointTriplet_thisSubPointIsPartOf().Invert_alignedHelperPoints_areOnTheSameSideOfTheAnchor();
                        }
                    }
                }
            }
        }

        void TryMirrorDistanceChangeRatio_ontoOtherHelperPointOfSameControlPoint(InternalDXXL_BezierControlHelperSubPoint2D concernedHelperPoint, bool mirrorDistanceChangeRatio_ontoOtherHelperPointOfSameControlPoint, float absDistanceToMountingAnchorPoint_beforeDrag_inUnitsOfGlobalSpace, float absDistanceToMountingAnchorPoint_afterDrag_inUnitsOfGlobalSpace)
        {
            if (mirrorDistanceChangeRatio_ontoOtherHelperPointOfSameControlPoint)
            {
                float absChangeRatio_ofDistance_throughSliderDrag = absDistanceToMountingAnchorPoint_afterDrag_inUnitsOfGlobalSpace / absDistanceToMountingAnchorPoint_beforeDrag_inUnitsOfGlobalSpace;
                if (UtilitiesDXXL_Math.FloatIsValid(absChangeRatio_ofDistance_throughSliderDrag))
                {
                    float newAbsDistanceToAnchorPoint_ofOppositeHelperPoint_inUnitsOfGlobalSpace = concernedHelperPoint.GetOppositeHelperPoint().Get_absDistanceToAnchorPoint_inUnitsOfGlobalSpace() * absChangeRatio_ofDistance_throughSliderDrag;
                    concernedHelperPoint.GetOppositeHelperPoint().Set_absDistanceToAnchorPoint_inUnitsOfGlobalSpace(newAbsDistanceToAnchorPoint_ofOppositeHelperPoint_inUnitsOfGlobalSpace, true, null); //-> oppositeHelperPoint is guaranteed "isUsed = true" here
                    //bool cylinderSliderPassedTheMountingAnchorPoint -> No further action required because "concernedHelperPoint.SetPos_inUnitsOfGlobalSpace()" already executed the "flip" of the other helper side.
                }
            }
        }

        void TryMirrorPosChange_ontoNeighboringHelperPointOfNeighboringControlPoint(int i, InternalDXXL_BezierControlHelperSubPoint2D concernedHelperPoint, bool mirrorPosChange_ontoNeighboringHelperPointOfNeighboringControlPoint, Vector3 shiftOffset_throughDragSlider_inUnitsOfGlobalSpace)
        {
            if (mirrorPosChange_ontoNeighboringHelperPointOfNeighboringControlPoint)
            {
                InternalDXXL_BezierControlHelperSubPoint2D neighboringHelperPoint_ofNeighboringControlPoint = concernedHelperPoint.Get_neighboringHelperPoint_ofNeighboringControlPoint(false);
                if (neighboringHelperPoint_ofNeighboringControlPoint != null)
                {
                    neighboringHelperPoint_ofNeighboringControlPoint.AddPosOffset_inUnitsOfGlobalSpace(-shiftOffset_throughDragSlider_inUnitsOfGlobalSpace, true, null);
                }
            }
        }

        void DrawUnitysBuildInHandlesAtSubPoints(int i)
        {
            //Could be improved: The handles here in the 2D version could be drawn "back-to-front"-order as "DrawCustomHandlesAtSubPoints()" does, because here custom handles are used and not Unitys build-in PositionHandle and RotatoinHandle (see explanation in the corresponding function of the 3D-version)
            //Though in 2D it is anyway not as important, because all handles lie in a plane and therefore the readability what's "nearer or farer" is anyway clearer.
            InternalDXXL_BezierControlPointTriplet2D concernedControlPoint = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i];
            TryDrawAUnityRotationHandle(i);
            TryDrawAUnityPositionHandle(i, concernedControlPoint.backwardHelperPoint);
            TryDrawAUnityPositionHandle(i, concernedControlPoint.forwardHelperPoint);
            TryDrawAUnityPositionHandle(i, concernedControlPoint.anchorPoint);
        }

        void TryDrawAUnityPositionHandle(int i, InternalDXXL_BezierControlSubPoint2D concernedSubPoint)
        {
            if (CheckIf_drawPositionHandle(concernedSubPoint))
            {
                if (concernedSubPoint.recalc_globalRotation_ofPositionHandle_duringNextOnSceneGUI) { Recalc_globalRotation_ofPositionHandle(concernedSubPoint); }

                float handleSizeFactor = (concernedSubPoint.subPointType == InternalDXXL_BezierControlSubPoint.SubPointType.anchor) ? bezierSplineDrawer_unserializedMonoB.handleSizeFor_position_atAnchors : bezierSplineDrawer_unserializedMonoB.handleSizeFor_position_atHelpers;
                float handleSize = handleSizeFactor * HandleUtility.GetHandleSize(concernedSubPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos());

                Vector3 direction_ofUpHandle = concernedSubPoint.globalRotation_ofPositionHandle * Vector3.up;
                Handles.color = Handles.yAxisColor;

                Start_handlesChangeCheck();
                Vector3 pos_shiftedByPositionUpHandle_inUnitsOfGlobalSpace = Handles.Slider(concernedSubPoint.controlID_ofUnityStylePositionHandleUp, concernedSubPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), direction_ofUpHandle, handleSize, Handles.ArrowHandleCap, 1.0f);
                bool up_hasChanged = End_handlesChangeCheck("Position of Bezier Point", i, true);

                Vector3 direction_ofRightHandle = concernedSubPoint.globalRotation_ofPositionHandle * Vector3.right;
                Handles.color = Handles.xAxisColor;

                Start_handlesChangeCheck();
                Vector3 pos_shiftedByPositionRightHandle_inUnitsOfGlobalSpace = Handles.Slider(concernedSubPoint.controlID_ofUnityStylePositionHandleRight, concernedSubPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), direction_ofRightHandle, handleSize, Handles.ArrowHandleCap, 1.0f);
                bool right_hasChanged = End_handlesChangeCheck("Position of Bezier Point", i, true);

                if (up_hasChanged)
                {
                    concernedSubPoint.recalc_globalRotation_ofPositionHandle_duringNextOnSceneGUI = false; //Without this there is strange flickering behaviour, maybe due to some internals of how "Handles.PositionHandle()" works. The flickering appears for "Editor.pivotMode=local" when you grab a the position handle that points to the neighboring control point and then get nearer towards this other control point.
                    concernedSubPoint.SetPos_inUnitsOfGlobalSpace(pos_shiftedByPositionUpHandle_inUnitsOfGlobalSpace, true, null);
                }

                if (right_hasChanged)
                {
                    concernedSubPoint.recalc_globalRotation_ofPositionHandle_duringNextOnSceneGUI = false; //Without this there is strange flickering behaviour, maybe due to some internals of how "Handles.PositionHandle()" works. The flickering appears for "Editor.pivotMode=local" when you grab a the position handle that points to the neighboring control point and then get nearer towards this other control point.
                    concernedSubPoint.SetPos_inUnitsOfGlobalSpace(pos_shiftedByPositionRightHandle_inUnitsOfGlobalSpace, true, null);
                }
            }
        }

        bool CheckIf_drawPositionHandle(InternalDXXL_BezierControlSubPoint2D concernedSubPoint)
        {
            if (concernedSubPoint.subPointType == InternalDXXL_BezierControlSubPoint.SubPointType.anchor)
            {
                return (bezierSplineDrawer_unserializedMonoB.showHandleFor_position_atAnchors && concernedSubPoint.isUsed);
            }
            else
            {
                return (bezierSplineDrawer_unserializedMonoB.showHandleFor_position_atHelpers && concernedSubPoint.isUsed);
            }
        }

        void Recalc_globalRotation_ofPositionHandle(InternalDXXL_BezierControlSubPoint2D concernedSubPoint)
        {
            switch (Tools.pivotRotation)
            {
                case PivotRotation.Local:
                    concernedSubPoint.SetBoolOf_boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled();
                    if (concernedSubPoint.boundGameobjectInclConnectionComponent_isAssignedActiveAndEnabled && (concernedSubPoint.subPointType == InternalDXXL_BezierControlSubPoint.SubPointType.anchor))
                    {
                        if (UtilitiesDXXL_EngineBasics.CheckIfThisOrAParentHasANonZRotation_2D(concernedSubPoint.boundGameobject.transform))
                        {
                            Recalc_globalRotation_ofPositionHandle_caseLocalPivotWithoutBoundGameobject(concernedSubPoint);
                        }
                        else
                        {
                            concernedSubPoint.globalRotation_ofPositionHandle = concernedSubPoint.boundGameobject.transform.rotation;
                        }
                    }
                    else
                    {
                        Recalc_globalRotation_ofPositionHandle_caseLocalPivotWithoutBoundGameobject(concernedSubPoint);
                    }
                    break;
                case PivotRotation.Global:
                    Recalc_globalRotation_ofPositionHandle_caseGlobalPivot(concernedSubPoint);
                    break;
                default:
                    break;
            }
        }

        void Recalc_globalRotation_ofPositionHandle_caseLocalPivotWithoutBoundGameobject(InternalDXXL_BezierControlSubPoint2D concernedSubPoint)
        {
            Vector3 posOfConcernedSubPoint_inUnitsOfGlobalSpace = concernedSubPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos();
            Vector3 posOfNextNonSuperimposedSubPointAlongSpline_inUnitsOfGlobalSpace = posOfConcernedSubPoint_inUnitsOfGlobalSpace;
            Vector3 posOfPreviousNonSuperimposedSubPointAlongSpline_inUnitsOfGlobalSpace = posOfConcernedSubPoint_inUnitsOfGlobalSpace;
            bool the3MountingPointsLieOnALine_soThePlaneIsUndefined = false;

            InternalDXXL_BezierControlSubPoint2D nextUsedNonSuperimposedSubPointAlongSplineDir = concernedSubPoint.GetNextUsedNonSuperimposedSubPointAlongSplineDir(false);
            if (nextUsedNonSuperimposedSubPointAlongSplineDir != null)
            {
                posOfNextNonSuperimposedSubPointAlongSpline_inUnitsOfGlobalSpace = nextUsedNonSuperimposedSubPointAlongSplineDir.GetPos_inUnitsOfGlobalSpace_asV3DrawPos();
            }
            else
            {
                the3MountingPointsLieOnALine_soThePlaneIsUndefined = true;
            }

            InternalDXXL_BezierControlSubPoint2D previousUsedNonSuperimposedSubPointAlongSplineDir = concernedSubPoint.GetPreviousUsedNonSuperimposedSubPointAlongSplineDir(false);
            if (previousUsedNonSuperimposedSubPointAlongSplineDir != null)
            {
                posOfPreviousNonSuperimposedSubPointAlongSpline_inUnitsOfGlobalSpace = previousUsedNonSuperimposedSubPointAlongSplineDir.GetPos_inUnitsOfGlobalSpace_asV3DrawPos();
            }
            else
            {
                the3MountingPointsLieOnALine_soThePlaneIsUndefined = true;
            }

            Vector3 normalOfPlane_inUnitsOfGlobalSpace_notNormalized = Vector3.forward;
            if (the3MountingPointsLieOnALine_soThePlaneIsUndefined == false)
            {
                Vector3 planeMountingVector1_inUnitsOfGlobalSpace = posOfConcernedSubPoint_inUnitsOfGlobalSpace - posOfNextNonSuperimposedSubPointAlongSpline_inUnitsOfGlobalSpace;
                Vector3 planeMountingVector2_inUnitsOfGlobalSpace = posOfConcernedSubPoint_inUnitsOfGlobalSpace - posOfPreviousNonSuperimposedSubPointAlongSpline_inUnitsOfGlobalSpace;
                normalOfPlane_inUnitsOfGlobalSpace_notNormalized = Vector3.Cross(planeMountingVector1_inUnitsOfGlobalSpace, planeMountingVector2_inUnitsOfGlobalSpace);

                the3MountingPointsLieOnALine_soThePlaneIsUndefined = (UtilitiesDXXL_Math.GetBiggestAbsComponent(normalOfPlane_inUnitsOfGlobalSpace_notNormalized) < 0.001f);
            }

            if (the3MountingPointsLieOnALine_soThePlaneIsUndefined)
            {
                Vector3 forwardDirectionPreferablyAlongHandleAxis_inUnitsOfGlobalSpace = concernedSubPoint.GetDirectionAlongSplineForwardBasedOnNeighborPoints_inUnitsOfGlobalSpace();
                if (UtilitiesDXXL_Math.ApproximatelyZero(forwardDirectionPreferablyAlongHandleAxis_inUnitsOfGlobalSpace))
                {
                    Recalc_globalRotation_ofPositionHandle_caseGlobalPivot(concernedSubPoint);
                }
                else
                {
                    Vector3 up_ofCreatedRotation = Vector3.Cross(Vector3.forward, forwardDirectionPreferablyAlongHandleAxis_inUnitsOfGlobalSpace);
                    concernedSubPoint.globalRotation_ofPositionHandle = Quaternion.LookRotation(Vector3.forward, up_ofCreatedRotation);
                }
            }
            else
            {
                Vector3 rotation_forward = normalOfPlane_inUnitsOfGlobalSpace_notNormalized;
                Vector3 rotation_up = posOfNextNonSuperimposedSubPointAlongSpline_inUnitsOfGlobalSpace - posOfPreviousNonSuperimposedSubPointAlongSpline_inUnitsOfGlobalSpace;
                concernedSubPoint.globalRotation_ofPositionHandle = Quaternion.LookRotation(rotation_forward, rotation_up);
            }
        }

        void Recalc_globalRotation_ofPositionHandle_caseGlobalPivot(InternalDXXL_BezierControlSubPoint2D concernedSubPoint)
        {
            switch (bezierSplineDrawer_unserializedMonoB.drawSpace)
            {
                case BezierSplineDrawer.DrawSpace.global:
                    concernedSubPoint.globalRotation_ofPositionHandle = Quaternion.identity;
                    break;
                case BezierSplineDrawer.DrawSpace.localDefinedByThisGameobject:
                    Recalc_globalRotation_ofPositionHandle_caseGlobalPivotButLocalDrawSpace(concernedSubPoint);
                    break;
                default:
                    break;
            }
        }

        void Recalc_globalRotation_ofPositionHandle_caseGlobalPivotButLocalDrawSpace(InternalDXXL_BezierControlSubPoint2D concernedSubPoint)
        {
            switch (bezierSplineDrawer_unserializedMonoB.positionHandleOrientation_forEditorPivotIsGlobal_butDrawSpaceIsLocal)
            {
                case BezierSplineDrawer.PositionHandleOrientation_forEditorPivotIsGlobal_butDrawSpaceIsLocal.localDrawSpace:
                    concernedSubPoint.globalRotation_ofPositionHandle = bezierSplineDrawer_unserializedMonoB.transform.rotation;
                    break;
                case BezierSplineDrawer.PositionHandleOrientation_forEditorPivotIsGlobal_butDrawSpaceIsLocal.globalSpace:
                    concernedSubPoint.globalRotation_ofPositionHandle = Quaternion.identity;
                    break;
                default:
                    break;
            }
        }

        void TryDrawAUnityRotationHandle(int i)
        {
            InternalDXXL_BezierControlPointTriplet2D concernedControlPoint = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i];
            if (CheckIf_drawRotationHandle(concernedControlPoint))
            {
                InternalDXXL_BezierControlAnchorSubPoint2D concernedAnchorPoint = concernedControlPoint.anchorPoint;
                float handleSize = bezierSplineDrawer_unserializedMonoB.handleSizeFor_rotation * HandleUtility.GetHandleSize(concernedAnchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos());

                if (concernedAnchorPoint.recalc_rotation_ofRotationHandleDuringRotationDragPhases_duringNextOnSceneGUI) { concernedAnchorPoint.rotation_ofRotationHandleDuringRotationDragPhases = Quaternion.identity; }

                Handles.color = Handles.zAxisColor;

                Start_handlesChangeCheck();
                Quaternion rotation_ofRotationHandle_afterRotate_inUnitsOfGlobalSpace = Handles.Disc(bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.controlID_ofUnityStyleRotationHandle2D, concernedAnchorPoint.rotation_ofRotationHandleDuringRotationDragPhases, concernedAnchorPoint.GetPos_inUnitsOfGlobalSpace_asV3DrawPos(), Vector3.forward, handleSize, false, 1.0f);
                bool hasChanged = End_handlesChangeCheck("Rotation of Bezier Point", i, true);

                if (hasChanged)
                {
                    concernedAnchorPoint.recalc_rotation_ofRotationHandleDuringRotationDragPhases_duringNextOnSceneGUI = false;
                    Set_rotation_afterChangeThroughRotationHandle(rotation_ofRotationHandle_afterRotate_inUnitsOfGlobalSpace, concernedAnchorPoint.rotation_ofRotationHandleDuringRotationDragPhases, concernedAnchorPoint);
                    concernedAnchorPoint.rotation_ofRotationHandleDuringRotationDragPhases = rotation_ofRotationHandle_afterRotate_inUnitsOfGlobalSpace;
                }
            }
        }

        bool CheckIf_drawRotationHandle(InternalDXXL_BezierControlPointTriplet2D concernedControlPoint)
        {
            if (bezierSplineDrawer_unserializedMonoB.showHandleFor_rotation)
            {
                return (concernedControlPoint.forwardHelperPoint.isUsed || concernedControlPoint.backwardHelperPoint.isUsed);
            }
            else
            {
                return false;
            }
        }

        void Set_rotation_afterChangeThroughRotationHandle(Quaternion rotation_ofRotationHandle_afterRotate_inUnitsOfGlobalSpace, Quaternion rotation_ofRotationHandle_beforeRotate_inUnitsOfGlobalSpace, InternalDXXL_BezierControlAnchorSubPoint2D concernedAnchorPoint)
        {
            Quaternion rotationIncrement = GetRotationIncrement(rotation_ofRotationHandle_beforeRotate_inUnitsOfGlobalSpace, rotation_ofRotationHandle_afterRotate_inUnitsOfGlobalSpace);

            if (concernedAnchorPoint.junctureType == InternalDXXL_BezierControlAnchorSubPoint.JunctureType.kinked)
            {
                if (concernedAnchorPoint.GetBackwardHelperPoint().isUsed)
                {
                    concernedAnchorPoint.AddRotation_toBackwardDirection(rotationIncrement, true, null);
                }

                if (concernedAnchorPoint.GetForwardHelperPoint().isUsed)
                {
                    concernedAnchorPoint.AddRotation_toForwardDirection(rotationIncrement, true, null);
                }
            }
            else
            {
                if (concernedAnchorPoint.GetForwardHelperPoint().isUsed)
                {
                    concernedAnchorPoint.AddRotation_toForwardDirection(rotationIncrement, true, null);
                }
                else
                {
                    if (concernedAnchorPoint.GetBackwardHelperPoint().isUsed)
                    {
                        concernedAnchorPoint.AddRotation_toBackwardDirection(rotationIncrement, true, null);
                    }
                }
            }
        }

        Quaternion GetRotationIncrement(Quaternion rotation_ofRotationHandle_beforeRotate_inUnitsOfGlobalSpace, Quaternion rotation_ofRotationHandle_afterRotate_inUnitsOfGlobalSpace)
        {
            Quaternion rotationIncrement = rotation_ofRotationHandle_afterRotate_inUnitsOfGlobalSpace * Quaternion.Inverse(rotation_ofRotationHandle_beforeRotate_inUnitsOfGlobalSpace);
            rotationIncrement.Normalize(); //-> the quaternion difference multiplication seems to introduce non-normalized quaternions somehow, therefore normalizing here.
            return rotationIncrement;
        }

        void Reset_recalculationFlags_duringNoHandleClickedOrDraggedPhases(int i)
        {
            if (GUIUtility.hotControl == 0) //-> no handle is selcted or dragged = mouse button is not held down
            {
                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.recalc_directionForHandles_forwardCone_duringNextOnSceneGUI = true;
                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.recalc_directionForHandles_backwardCone_duringNextOnSceneGUI = true;
                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.recalc_rotation_ofRotationHandleDuringRotationDragPhases_duringNextOnSceneGUI = true;

                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].forwardHelperPoint.recalc_directionForHandles_alongLineWithMountingAnchor_duringNextOnSceneGUI = true;
                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].forwardHelperPoint.recalc_directionForHandles_alongLineWithNeighboringHelperOfNeighboringControlPoint_duringNextOnSceneGUI = true;

                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].backwardHelperPoint.recalc_directionForHandles_alongLineWithMountingAnchor_duringNextOnSceneGUI = true;
                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].backwardHelperPoint.recalc_directionForHandles_alongLineWithNeighboringHelperOfNeighboringControlPoint_duringNextOnSceneGUI = true;

                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint.recalc_globalRotation_ofPositionHandle_duringNextOnSceneGUI = true;
                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].forwardHelperPoint.recalc_globalRotation_ofPositionHandle_duringNextOnSceneGUI = true;
                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].backwardHelperPoint.recalc_globalRotation_ofPositionHandle_duringNextOnSceneGUI = true;
            }
        }

        void ResetUndoRegistrationFlag_duringNoInteractionPhases()
        {
            if (GUIUtility.hotControl == 0) //-> no handle is selcted or dragged = mouse button is not held down
            {
                hasRegisteredUndo_sinceMouseDown = false;
            }
        }

        void Start_handlesChangeCheck()
        {
            EditorGUI.BeginChangeCheck();
        }

        bool End_handlesChangeCheck(string nameOfUndoEntry, int i_controlPointWithInteraction, bool markConcernedControlPoint_asSelected)
        {
            bool hasChanged = EditorGUI.EndChangeCheck();
            if (hasChanged)
            {
                TryRegisterStateForUndo(nameOfUndoEntry, true, false);
                if (markConcernedControlPoint_asSelected)
                {
                    bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i_controlPointWithInteraction);
                }
            }
            return hasChanged;
        }

        void TryRegisterStateForUndo(string nameOfUndoEntry, bool includeTransformsOfAllBoundGameobjects, bool includeConnectionComponentsOfAllBoundGameobjects)
        {
            if (hasRegisteredUndo_sinceMouseDown == false)
            {
                bezierSplineDrawer_unserializedMonoB.RegisterStateForUndo(nameOfUndoEntry, includeTransformsOfAllBoundGameobjects, includeConnectionComponentsOfAllBoundGameobjects);
                hasRegisteredUndo_sinceMouseDown = true;
            }
        }

        void TrySetSelectedListSlot_dueToHandlesInteraction(int i)
        {
            InternalDXXL_BezierControlAnchorSubPoint2D anchorPoint = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].anchorPoint;
            InternalDXXL_BezierControlHelperSubPoint2D forwardHelperPoint = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].forwardHelperPoint;
            InternalDXXL_BezierControlHelperSubPoint2D backwardHelperPoint = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].backwardHelperPoint;

            if (anchorPoint.controlID_ofCustomHandles_sphere == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }
            if (anchorPoint.controlID_ofCustomHandles_forwardCone == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }
            if (anchorPoint.controlID_ofCustomHandles_backwardCone == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }
            if (anchorPoint.controlID_ofUnityStylePositionHandleUp == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }
            if (anchorPoint.controlID_ofUnityStylePositionHandleRight == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }
            if (anchorPoint.controlID_ofUnityStyleRotationHandle2D == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }

            if (forwardHelperPoint.controlID_ofCustomHandles_sphere == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }
            if (forwardHelperPoint.controlID_ofCustomHandles_coneAlongLineWithAnchor == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }
            if (forwardHelperPoint.controlID_ofCustomHandles_coneAlongLineWithNeighborsHelper == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }
            if (forwardHelperPoint.controlID_ofCustomHandles_cylinderAlongLineWithAnchor == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }
            if (forwardHelperPoint.controlID_ofCustomHandles_cylinderAlongLineWithNeighborsHelper == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }
            if (forwardHelperPoint.controlID_ofUnityStylePositionHandleUp == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }
            if (forwardHelperPoint.controlID_ofUnityStylePositionHandleRight == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }

            if (backwardHelperPoint.controlID_ofCustomHandles_sphere == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }
            if (backwardHelperPoint.controlID_ofCustomHandles_coneAlongLineWithAnchor == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }
            if (backwardHelperPoint.controlID_ofCustomHandles_coneAlongLineWithNeighborsHelper == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }
            if (backwardHelperPoint.controlID_ofCustomHandles_cylinderAlongLineWithAnchor == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }
            if (backwardHelperPoint.controlID_ofCustomHandles_cylinderAlongLineWithNeighborsHelper == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }
            if (backwardHelperPoint.controlID_ofUnityStylePositionHandleUp == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }
            if (backwardHelperPoint.controlID_ofUnityStylePositionHandleRight == GUIUtility.hotControl) { bezierSplineDrawer_unserializedMonoB.SetSelectedListSlot(i); }
        }

        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;

            serializedObject.Update();

            float allowedConsumedLines_0to1 = DrawConsumedLines("spline curve");
            TryDrawInfoTextHowToReducedDrawnLines(allowedConsumedLines_0to1);

            EditorGUILayout.PropertyField(serializedObject.FindProperty("color"), new GUIContent("Color"));

            SerializedProperty sP_lineWidth = serializedObject.FindProperty("lineWidth");
            EditorGUILayout.PropertyField(sP_lineWidth, new GUIContent("Width"));
            sP_lineWidth.floatValue = Mathf.Max(sP_lineWidth.floatValue, 0.0f);

            SerializedProperty sP_straightSubDivisionsPerSegment = serializedObject.FindProperty("straightSubDivisionsPerSegment");
            EditorGUILayout.PropertyField(sP_straightSubDivisionsPerSegment, new GUIContent("Resolution (=straight lines per bezier segment)"));
            sP_straightSubDivisionsPerSegment.intValue = Mathf.Max(sP_straightSubDivisionsPerSegment.intValue, 3);

            DrawCloseRingToggle();
            DrawDrawSpaceSection();
            DrawHandlesSection();
            DrawControlPointsSection();
            DrawZPosChooserFor2D();
            DrawTextInputInclMarkupHelper();
            DrawCheckboxFor_drawOnlyIfSelected("Spline curve");
            DrawCheckboxFor_hiddenByNearerObjects("Spline curve");

            serializedObject.ApplyModifiedProperties();
            ResetUndoRegistrationFlag_duringNoInteractionPhases();

            EditorGUI.indentLevel = indentLevel_before;
        }

        void TryDrawInfoTextHowToReducedDrawnLines(float allowedConsumedLines_0to1)
        {
            if (GUIUtility.hotControl == 0) //-> no handle is selcted or dragged = mouse button is not held down. Otherwise the warning box would disappear while the user drags the mentioned "Width"- or "Resolution"-field, which results in unconvenient line jumps.
            {
                if (allowedConsumedLines_0to1 > 0.5f)
                {
                    if (UtilitiesDXXL_Math.ApproximatelyZero(serializedObject.FindProperty("lineWidth").floatValue) == false)
                    {
                        if (serializedObject.FindProperty("straightSubDivisionsPerSegment").intValue > 20)
                        {
                            manyDrawnLinesWarningState = BezierSplineDrawerInspector.ManyDrawnLinesWarningState.reduceWidthAndResolution;
                        }
                        else
                        {
                            manyDrawnLinesWarningState = BezierSplineDrawerInspector.ManyDrawnLinesWarningState.reduceWidth;
                        }
                    }
                    else
                    {
                        if (serializedObject.FindProperty("straightSubDivisionsPerSegment").intValue > 10)
                        {
                            manyDrawnLinesWarningState = BezierSplineDrawerInspector.ManyDrawnLinesWarningState.reduceResolution;
                        }
                        else
                        {
                            manyDrawnLinesWarningState = BezierSplineDrawerInspector.ManyDrawnLinesWarningState.noWarning;
                        }
                    }
                }
                else
                {
                    manyDrawnLinesWarningState = BezierSplineDrawerInspector.ManyDrawnLinesWarningState.noWarning;
                }
            }

            switch (manyDrawnLinesWarningState)
            {
                case BezierSplineDrawerInspector.ManyDrawnLinesWarningState.noWarning:
                    break;
                case BezierSplineDrawerInspector.ManyDrawnLinesWarningState.reduceWidthAndResolution:
                    EditorGUILayout.HelpBox("Many drawn lines could be saved if 'Width' would be set to 0. Another option is to decrease the 'Resolution'.", MessageType.Info, true);
                    break;
                case BezierSplineDrawerInspector.ManyDrawnLinesWarningState.reduceWidth:
                    EditorGUILayout.HelpBox("Many drawn lines could be saved if 'Width' would be set to 0.", MessageType.Info, true);
                    break;
                case BezierSplineDrawerInspector.ManyDrawnLinesWarningState.reduceResolution:
                    EditorGUILayout.HelpBox("To save drawn lines it may help to decrease the 'Resolution'.", MessageType.Info, true);
                    break;
                default:
                    break;
            }
        }

        void DrawDrawSpaceSection() //two times "Draw" in the name is in meant this way
        {
            SerializedProperty sP_drawSpaceSection_isOutfolded = serializedObject.FindProperty("drawSpaceSection_isOutfolded");
            sP_drawSpaceSection_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_drawSpaceSection_isOutfolded.boolValue, "Draw Space", true);
            if (sP_drawSpaceSection_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                DrawDrawSpaceEnum();

                SerializedProperty sP_keepWorldPos_duringDrawSpaceChange = serializedObject.FindProperty("keepWorldPos_duringDrawSpaceChange");
                sP_keepWorldPos_duringDrawSpaceChange.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Keep world position during space change", "If this is selected then the global position and shape of the spline will stay the same when the draw space gets changed." + Environment.NewLine + Environment.NewLine + "If it is unselected then the spline will keep it's shape but will be scaled and rotated to fit the new draw space."), sP_keepWorldPos_duringDrawSpaceChange.boolValue);

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
                GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);
            }
        }

        void DrawDrawSpaceEnum() //two times "Draw" in the name is in meant this way
        {
            serializedObject.ApplyModifiedProperties();

            if (bezierSplineDrawer_unserializedMonoB.drawSpace == BezierSplineDrawer.DrawSpace.localDefinedByThisGameobject)
            {
                if (UtilitiesDXXL_EngineBasics.CheckIf_transformOrAParentHasNonUniformScale_2D(bezierSplineDrawer_unserializedMonoB.transform))
                {
                    EditorGUILayout.HelpBox("The transform of this gameobject or of a parent has a non-uniform scale. This may lead to wrong or weird results, when drawing in local space.", MessageType.Warning, true);
                }

                if (UtilitiesDXXL_EngineBasics.CheckIfThisOrAParentHasANonZRotation_2D(bezierSplineDrawer_unserializedMonoB.transform))
                {
                    EditorGUILayout.HelpBox("The transform of this gameobject or of a parent has a non-z rotation. This may lead to wrong or weird results, when drawing in local space.", MessageType.Warning, true);
                }
            }

            BezierSplineDrawer.DrawSpace drawSpace_after = (BezierSplineDrawer.DrawSpace)EditorGUILayout.EnumPopup(GUIContent.none, bezierSplineDrawer_unserializedMonoB.drawSpace);
            if (drawSpace_after != bezierSplineDrawer_unserializedMonoB.drawSpace)
            {
                bezierSplineDrawer_unserializedMonoB.RegisterStateForUndo("Change Spline Space", true, false);
                bezierSplineDrawer_unserializedMonoB.ChangeDrawSpace(drawSpace_after);
            }

            serializedObject.Update();
        }

        void DrawCloseRingToggle()
        {
            serializedObject.ApplyModifiedProperties();

            bool closeGapState_after = EditorGUILayout.Toggle(new GUIContent("Close ring from end to start"), bezierSplineDrawer_unserializedMonoB.gapFromEndToStart_isClosed);
            if (closeGapState_after != bezierSplineDrawer_unserializedMonoB.gapFromEndToStart_isClosed)
            {
                bezierSplineDrawer_unserializedMonoB.ChangeCloseGapState(closeGapState_after);
            }

            serializedObject.Update();
        }

        void DrawControlPointsSection()
        {
            SerializedProperty sP_controlPointsList_isOutfolded = serializedObject.FindProperty("controlPointsList_isOutfolded");
            sP_controlPointsList_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_controlPointsList_isOutfolded.boolValue, "Control Points", true);
            if (sP_controlPointsList_isOutfolded.boolValue)
            {
                DrawNonSerializedControlPointsList();
                DrawSectionWithDefaultValuesOfNewlyCreatedPoints();
                GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);
            }
        }

        void DrawNonSerializedControlPointsList()
        {
            //-> exceptionally: no indent here to have more display space for the list 

            serializedObject.ApplyModifiedProperties();

            bezierSplineDrawer_unserializedMonoB.TryResheduleSceneViewRepaint();

            Rect firstControlPointRect = default;
            bool firstControlPointRect_hasBeenFilled = false;

            for (int i = 0; i < bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets.Count; i++)
            {
                float height_ofCurrentControlPoint = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].GetPropertyHeightForInspectorList();
                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].inspectorRect_reservedForThisTriplet = EditorGUILayout.GetControlRect(true, height_ofCurrentControlPoint);

                if (firstControlPointRect_hasBeenFilled == false)
                {
                    firstControlPointRect = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].inspectorRect_reservedForThisTriplet;
                    firstControlPointRect_hasBeenFilled = true;
                }
            }

            float height_ofEmptyControlPointHoldingOnlyPlusButton = InternalDXXL_BezierControlPointTriplet.GetPropertyHeightForEmptyControlPointHoldingOnlyAPlusButtonAndFoldAllButtons();
            Rect rect_ofEmptyControlPointHoldingOnlyPlusButtonAndFoldAllButtons = EditorGUILayout.GetControlRect(true, height_ofEmptyControlPointHoldingOnlyPlusButton);

            if (firstControlPointRect_hasBeenFilled == false)
            {
                firstControlPointRect = rect_ofEmptyControlPointHoldingOnlyPlusButtonAndFoldAllButtons;
            }

            float y_ofListsLowerEnd = rect_ofEmptyControlPointHoldingOnlyPlusButtonAndFoldAllButtons.y + rect_ofEmptyControlPointHoldingOnlyPlusButtonAndFoldAllButtons.height;
            float heightOfListBackground = y_ofListsLowerEnd - firstControlPointRect.y;
            Rect space_ofBackgroundColorRect = new Rect(firstControlPointRect.x, firstControlPointRect.y, firstControlPointRect.width, heightOfListBackground);
            Rect space_ofBackgroundColorRectFrame = new Rect(space_ofBackgroundColorRect.x - 1.0f, space_ofBackgroundColorRect.y - 1.0f, space_ofBackgroundColorRect.width + 2.0f, space_ofBackgroundColorRect.height + 2.0f);
            EditorGUI.DrawRect(space_ofBackgroundColorRectFrame, BezierSplineDrawer.color_ofControlPointListBackgroundFrameInInspecor);
            EditorGUI.DrawRect(space_ofBackgroundColorRect, BezierSplineDrawer.color_ofControlPointListBackgroundInInspecor);

            for (int i = 0; i < bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets.Count; i++)
            {
                bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].DrawValuesToInspector();
            }

            for (int i = 0; i < bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets.Count; i++)
            {
                if (bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].minusButtonAtThisListItem_hasBeenClickedInInspector)
                {
                    bezierSplineDrawer_unserializedMonoB.TryDeleteControlPoint_dueToMinusButtonAtControlPointListItemHasBeenClicked(i);
                    bezierSplineDrawer_unserializedMonoB.SheduleSceneViewRepaint();
                    break; //-> only one change at a time
                }

                bool didChangeSomething = bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets[i].TryApplyChangesAfterInspectorInput();
                if (didChangeSomething)
                {
                    bezierSplineDrawer_unserializedMonoB.SheduleSceneViewRepaint();
                    break; //-> only one change at a time
                }
            }

            DrawEmptyControlPointBelowControlPointsList_thatHoldsOnlyAPlusButtonAndFoldAllButtons(rect_ofEmptyControlPointHoldingOnlyPlusButtonAndFoldAllButtons);

            serializedObject.Update();
        }

        void DrawEmptyControlPointBelowControlPointsList_thatHoldsOnlyAPlusButtonAndFoldAllButtons(Rect rect_ofEmptyControlPointHoldingOnlyPlusButtonAndFoldAllButtons)
        {
            bool greyOutBothFoldAllButtons = ((bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets.Count == 0) || ((bezierSplineDrawer_unserializedMonoB.listOfControlPointTriplets.Count == 1) && (bezierSplineDrawer_unserializedMonoB.gapFromEndToStart_isClosed == false)));
            bool greyOutUnfoldAllButton = greyOutBothFoldAllButtons || bezierSplineDrawer_unserializedMonoB.CheckIf_allFoldableHelperPoints_areUnfolded_inTheInspectorList();
            bool greyOutCollapseAllButton = greyOutBothFoldAllButtons || bezierSplineDrawer_unserializedMonoB.CheckIf_allFoldableHelperPoints_areCollapsed_inTheInspectorList();

            InternalDXXL_BezierControlPointTriplet.DrawEmptyControlPointHoldingOnlyAPlusButton_forInspector(out bool plusButtonBelowListOfControlPoints_hasBeenClicked, out bool unfoldAllWeightsBelowListOfControlPoints_hasBeenClicked, out bool collapseAllWeightsBelowListOfControlPoints_hasBeenClicked, rect_ofEmptyControlPointHoldingOnlyPlusButtonAndFoldAllButtons, bezierSplineDrawer_unserializedMonoB.color_ofAnchorPoints, plusSymbolIcon, greyOutUnfoldAllButton, greyOutCollapseAllButton);
            if (plusButtonBelowListOfControlPoints_hasBeenClicked)
            {
                bezierSplineDrawer_unserializedMonoB.CreateNewControlPoint_dueToPlusButtonBelowControlPointsListHasBeenClicked();
                bezierSplineDrawer_unserializedMonoB.SheduleSceneViewRepaint();
            }

            if (unfoldAllWeightsBelowListOfControlPoints_hasBeenClicked)
            {
                bezierSplineDrawer_unserializedMonoB.UnfoldAllHelperPointInTheInspectorList();
            }

            if (collapseAllWeightsBelowListOfControlPoints_hasBeenClicked)
            {
                bezierSplineDrawer_unserializedMonoB.CollapseAllHelperPointInTheInspectorList();
            }
        }

        void DrawSectionWithDefaultValuesOfNewlyCreatedPoints()
        {
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            SerializedProperty sP_defaultPosRotWeightOffsetOfNewlyCreatedPoints_section_isOutfolded = serializedObject.FindProperty("defaultPosRotWeightOffsetOfNewlyCreatedPoints_section_isOutfolded");
            sP_defaultPosRotWeightOffsetOfNewlyCreatedPoints_section_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_defaultPosRotWeightOffsetOfNewlyCreatedPoints_section_isOutfolded.boolValue, "Default values of newly created control points", true);
            if (sP_defaultPosRotWeightOffsetOfNewlyCreatedPoints_section_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                DrawSectionWithDefaultOffsetOfNewlyCreatedPoints();
                DrawSectionWithDefaultOrientationOfNewlyCreatedPoints();
                DrawSectionWithDefaultWeightDistancesOfNewlyCreatedPoints();
                DrawSectionWithDefaultJunctureTypeOfNewlyCreatedPoints();

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

        void DrawSectionWithDefaultOffsetOfNewlyCreatedPoints()
        {
            SerializedProperty sP_defaultPosOffsetOfNewlyCreatedPoints_subSection_isOutfolded = serializedObject.FindProperty("defaultPosOffsetOfNewlyCreatedPoints_subSection_isOutfolded");
            sP_defaultPosOffsetOfNewlyCreatedPoints_subSection_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_defaultPosOffsetOfNewlyCreatedPoints_subSection_isOutfolded.boolValue, "Default Position Offset", true);
            if (sP_defaultPosOffsetOfNewlyCreatedPoints_subSection_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                SerializedProperty sP_definitionType_ofDefaultPosOffset = serializedObject.FindProperty("definitionType_ofDefaultPosOffset");
                EditorGUILayout.PropertyField(sP_definitionType_ofDefaultPosOffset, new GUIContent("Offset source"));
                switch (sP_definitionType_ofDefaultPosOffset.enumValueIndex)
                {
                    case (int)BezierSplineDrawer.DefinitionType_ofDefaultPosOffset.straightExtentionOfCurveEnd:
                        SerializedProperty sP_distanceBetweenTriplets_forNewlyCreatedTriplets_caseOf_straightExtentionOfCurveEnd_inUnitsOfActiveDrawSpace = serializedObject.FindProperty("distanceBetweenTriplets_forNewlyCreatedTriplets_caseOf_straightExtentionOfCurveEnd_inUnitsOfActiveDrawSpace");
                        EditorGUILayout.PropertyField(sP_distanceBetweenTriplets_forNewlyCreatedTriplets_caseOf_straightExtentionOfCurveEnd_inUnitsOfActiveDrawSpace, new GUIContent("Distance"));
                        sP_distanceBetweenTriplets_forNewlyCreatedTriplets_caseOf_straightExtentionOfCurveEnd_inUnitsOfActiveDrawSpace.floatValue = Mathf.Max(sP_distanceBetweenTriplets_forNewlyCreatedTriplets_caseOf_straightExtentionOfCurveEnd_inUnitsOfActiveDrawSpace.floatValue, 0.0f);
                        GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);
                        break;
                    case (int)BezierSplineDrawer.DefinitionType_ofDefaultPosOffset.customOffset:
                        DrawSpecificationOf_customVector2_1("Custom offset value", false, null, false, false, true, false);
                        break;
                    default:
                        break;
                }

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        void DrawSectionWithDefaultOrientationOfNewlyCreatedPoints()
        {
            SerializedProperty sP_defaultRotOfNewlyCreatedPoints_subSection_isOutfolded = serializedObject.FindProperty("defaultRotOfNewlyCreatedPoints_subSection_isOutfolded");
            sP_defaultRotOfNewlyCreatedPoints_subSection_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_defaultRotOfNewlyCreatedPoints_subSection_isOutfolded.boolValue, "Default Initial Orientation", true);
            if (sP_defaultRotOfNewlyCreatedPoints_subSection_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                SerializedProperty sP_definitionType_ofDefaultRot = serializedObject.FindProperty("definitionType_ofDefaultRot");
                EditorGUILayout.PropertyField(sP_definitionType_ofDefaultRot, new GUIContent("Orientation source"));
                switch (sP_definitionType_ofDefaultRot.enumValueIndex)
                {
                    case (int)BezierSplineDrawer.DefinitionType_ofDefaultRot.sameAsCurveEnd:
                        GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);
                        break;
                    case (int)BezierSplineDrawer.DefinitionType_ofDefaultRot.customOrientation:
                        DrawSpecificationOf_customVector2_2("Custom forward vector that defines the orientation", false, null, true, false, true, false);
                        break;
                    default:
                        break;
                }

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        void DrawSectionWithDefaultWeightDistancesOfNewlyCreatedPoints()
        {
            SerializedProperty sP_defaultWeightsOfNewlyCreatedPoints_subSection_isOutfolded = serializedObject.FindProperty("defaultWeightsOfNewlyCreatedPoints_subSection_isOutfolded");
            sP_defaultWeightsOfNewlyCreatedPoints_subSection_isOutfolded.boolValue = EditorGUILayout.Foldout(sP_defaultWeightsOfNewlyCreatedPoints_subSection_isOutfolded.boolValue, "Default Initial Weight Distances", true);
            if (sP_defaultWeightsOfNewlyCreatedPoints_subSection_isOutfolded.boolValue)
            {
                EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

                SerializedProperty sP_forwardWeightDistance_ofNewlyCreatedPoints_inUnitsOfActiveDrawSpace = serializedObject.FindProperty("forwardWeightDistance_ofNewlyCreatedPoints_inUnitsOfActiveDrawSpace");
                EditorGUILayout.PropertyField(sP_forwardWeightDistance_ofNewlyCreatedPoints_inUnitsOfActiveDrawSpace, new GUIContent("Forward"));
                sP_forwardWeightDistance_ofNewlyCreatedPoints_inUnitsOfActiveDrawSpace.floatValue = Mathf.Max(sP_forwardWeightDistance_ofNewlyCreatedPoints_inUnitsOfActiveDrawSpace.floatValue, 0.0f);

                SerializedProperty sP_backwardWeightDistance_ofNewlyCreatedPoints_inUnitsOfActiveDrawSpace = serializedObject.FindProperty("backwardWeightDistance_ofNewlyCreatedPoints_inUnitsOfActiveDrawSpace");
                EditorGUILayout.PropertyField(sP_backwardWeightDistance_ofNewlyCreatedPoints_inUnitsOfActiveDrawSpace, new GUIContent("Backward"));
                sP_backwardWeightDistance_ofNewlyCreatedPoints_inUnitsOfActiveDrawSpace.floatValue = Mathf.Max(sP_backwardWeightDistance_ofNewlyCreatedPoints_inUnitsOfActiveDrawSpace.floatValue, 0.0f);

                GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);

                EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
            }
        }

        void DrawSectionWithDefaultJunctureTypeOfNewlyCreatedPoints()
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("junctureType_ofNewlyCreatedPoints"), new GUIContent("Default Juncture Type"));
        }

        void DrawHandlesSection()
        {
            SerializedProperty sP_handlesSection_isOutfolded = serializedObject.FindProperty("handlesSection_isOutfolded");
            SerializedProperty sP_hideAllHandles = serializedObject.FindProperty("hideAllHandles");

            Rect rect_ofHandlesHeadline = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
            Rect rect_ofHandlesHeadlineFoldout = new Rect(rect_ofHandlesHeadline.x, rect_ofHandlesHeadline.y, EditorGUIUtility.singleLineHeight, rect_ofHandlesHeadline.height);
            Rect rect_ofHandlesHeadlineTextWithCheckbox = new Rect(rect_ofHandlesHeadline.x, rect_ofHandlesHeadline.y, rect_ofHandlesHeadline.width, rect_ofHandlesHeadline.height);

            sP_hideAllHandles.boolValue = !EditorGUI.ToggleLeft(rect_ofHandlesHeadlineTextWithCheckbox, new GUIContent("Handles"), !sP_hideAllHandles.boolValue);
            sP_handlesSection_isOutfolded.boolValue = EditorGUI.Foldout(rect_ofHandlesHeadlineFoldout, sP_handlesSection_isOutfolded.boolValue, GUIContent.none, true);

            if (sP_handlesSection_isOutfolded.boolValue == true)
            {
                EditorGUI.BeginDisabledGroup(sP_hideAllHandles.boolValue);
                DrawHandlesSection_outfoldedPart();
                EditorGUI.EndDisabledGroup();
            }
        }

        void DrawHandlesSection_outfoldedPart()
        {
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            GUIStyle labelStyle_withRichtext = new GUIStyle();

            DrawPositionHandlesSection(labelStyle_withRichtext);
            DrawRotationHandlesSection(labelStyle_withRichtext);
            DrawCustomHandlesSection(labelStyle_withRichtext);
            DrawPlusButtonHandleSection(labelStyle_withRichtext);
            DrawDottedLineAlongZToBoundGameobjectsSection(labelStyle_withRichtext);

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;
        }

        void DrawPositionHandlesSection(GUIStyle labelStyle_withRichtext)
        {
            EditorGUILayout.LabelField("<b>Move Position Handle</b> (Unity style)", labelStyle_withRichtext);
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            SerializedProperty sP_showHandleFor_position_atAnchors = serializedObject.FindProperty("showHandleFor_position_atAnchors");
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sP_showHandleFor_position_atAnchors, new GUIContent("At Anchor Points (show/size)"));
            EditorGUI.BeginDisabledGroup(!sP_showHandleFor_position_atAnchors.boolValue);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("handleSizeFor_position_atAnchors"), GUIContent.none);
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            SerializedProperty sP_showHandleFor_position_atHelpers = serializedObject.FindProperty("showHandleFor_position_atHelpers");
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sP_showHandleFor_position_atHelpers, new GUIContent("At Helper Points (show/size)"));
            EditorGUI.BeginDisabledGroup(!sP_showHandleFor_position_atHelpers.boolValue);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("handleSizeFor_position_atHelpers"), GUIContent.none);
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            bool drawSpaceIsLocal = (serializedObject.FindProperty("drawSpace").enumValueIndex == (int)BezierSplineDrawer.DrawSpace.localDefinedByThisGameobject);
            bool positionHandleOrientation_forEditorPivotIsGlobal_butDrawSpaceIsLocal_chooserIsAvailable = (Tools.pivotRotation == PivotRotation.Global) && drawSpaceIsLocal;
            string positionHandleOrientation_forEditorPivotIsGlobal_butDrawSpaceIsLocal_displayName = "Global orientation in local draw space";
            if (positionHandleOrientation_forEditorPivotIsGlobal_butDrawSpaceIsLocal_chooserIsAvailable)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("positionHandleOrientation_forEditorPivotIsGlobal_butDrawSpaceIsLocal"), new GUIContent(positionHandleOrientation_forEditorPivotIsGlobal_butDrawSpaceIsLocal_displayName, "If your Editor tool handle rotation is set to 'global' orientation, but you work in a local draw space for a spline then the question arises which space should be considered as 'global' in den local draw space and accordingly how the position handle should be displayed." + Environment.NewLine + Environment.NewLine + "Chose 'global space' if you want the position handles aligned with the global world space." + Environment.NewLine + Environment.NewLine + "Chose 'local draw space' if you want the position handles aligned with the local draw space."));
            }
            else
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("positionHandleOrientation_forEditorPivotIsGlobal_butDrawSpaceIsLocal"), new GUIContent(positionHandleOrientation_forEditorPivotIsGlobal_butDrawSpaceIsLocal_displayName, "Only available if Editor tool handle rotation is set to 'global' and if draw space is 'local'."));
                EditorGUI.EndDisabledGroup();
            }

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

            GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);
        }

        void DrawRotationHandlesSection(GUIStyle labelStyle_withRichtext)
        {
            EditorGUILayout.LabelField("<b>Rotate Handle</b>", labelStyle_withRichtext);
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            SerializedProperty sP_showHandleFor_rotation = serializedObject.FindProperty("showHandleFor_rotation");
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sP_showHandleFor_rotation, new GUIContent("Show / Size"));
            EditorGUI.BeginDisabledGroup(!sP_showHandleFor_rotation.boolValue);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("handleSizeFor_rotation"), GUIContent.none);
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

            GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);
        }

        void DrawCustomHandlesSection(GUIStyle labelStyle_withRichtext)
        {
            EditorGUILayout.LabelField("<b>Spline Custom Handles</b>", labelStyle_withRichtext);
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            EditorGUILayout.LabelField("At Anchor Points");
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            SerializedProperty sP_showCustomHandleFor_anchorPoints = serializedObject.FindProperty("showCustomHandleFor_anchorPoints");
            EditorGUILayout.PropertyField(sP_showCustomHandleFor_anchorPoints, new GUIContent("Show"));
            EditorGUI.BeginDisabledGroup(!sP_showCustomHandleFor_anchorPoints.boolValue);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("handleSizeOf_customHandle_atAnchors"), new GUIContent("Size"));
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("color_ofAnchorPoints"), new GUIContent("Color"));
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

            GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);

            EditorGUILayout.LabelField("At Helper Points");
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;
            SerializedProperty sP_showCustomHandleFor_helperPoints = serializedObject.FindProperty("showCustomHandleFor_helperPoints");
            EditorGUILayout.PropertyField(sP_showCustomHandleFor_helperPoints, new GUIContent("Show"));
            EditorGUI.BeginDisabledGroup(!sP_showCustomHandleFor_helperPoints.boolValue);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("handleSizeOf_customHandle_atHelpers"), new GUIContent("Size"));
            EditorGUI.EndDisabledGroup();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("color_ofHelperPoints"), new GUIContent("Color"));
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

            GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);
        }

        void DrawPlusButtonHandleSection(GUIStyle labelStyle_withRichtext)
        {
            EditorGUILayout.LabelField("<b>Add Point Buttons ('+')</b>", labelStyle_withRichtext);
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            SerializedProperty sP_showHandleFor_plusButtons_atSplineStartAndEnd = serializedObject.FindProperty("showHandleFor_plusButtons_atSplineStartAndEnd");
            SerializedProperty sP_showHandleFor_plusButtons_insideSegments = serializedObject.FindProperty("showHandleFor_plusButtons_insideSegments");
            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(sP_showHandleFor_plusButtons_atSplineStartAndEnd, new GUIContent("At Start/End"));
            EditorGUILayout.PropertyField(sP_showHandleFor_plusButtons_insideSegments, new GUIContent("Inside Curve"));
            GUILayout.EndHorizontal();

            bool atLeastOnePlusButtonOption_isChecked = (sP_showHandleFor_plusButtons_atSplineStartAndEnd.boolValue || sP_showHandleFor_plusButtons_insideSegments.boolValue);
            EditorGUI.BeginDisabledGroup(!atLeastOnePlusButtonOption_isChecked);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("handleSizeOf_plusButtons"), new GUIContent("Size"));
            EditorGUI.EndDisabledGroup();
            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

            GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);
        }

        void DrawDottedLineAlongZToBoundGameobjectsSection(GUIStyle labelStyle_withRichtext)
        {
            EditorGUILayout.LabelField("<b>Dotted Lines along Z to bound Gameobjects</b>", labelStyle_withRichtext);
            EditorGUI.indentLevel = EditorGUI.indentLevel + 1;

            GUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(serializedObject.FindProperty("showDottedLinesAlongZToBoundGameobjects"), new GUIContent("Show"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("colorOfLinesAlongZToBoundGameobjects"), GUIContent.none);
            GUILayout.EndHorizontal();

            EditorGUILayout.PropertyField(serializedObject.FindProperty("dotLength_ofDottedLinesAlongZToBoundGameobjects"), new GUIContent("Dash Length"));

            EditorGUI.indentLevel = EditorGUI.indentLevel - 1;

            GUILayout.Space(1.0f * EditorGUIUtility.singleLineHeight);
        }

    }
#endif
}
