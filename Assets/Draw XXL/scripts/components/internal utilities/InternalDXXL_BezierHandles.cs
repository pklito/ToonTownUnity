namespace DrawXXL
{
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;

    public class InternalDXXL_BezierHandles
    {
        static float valueToSlide_duringMouseDown;
        static Vector3 direction_duringMouseDown;
        static Vector2 currentMousePosition;
        static Vector2 mousePosition_duringMouseDown;

        public static float ValueSliderAlongCurve(float valueToSlide, Vector3 position, Quaternion rotation, Vector3 direction_duringMouseDown, float size, Handles.CapFunction capFunction, float snap)
        {
            int control_ID = GUIUtility.GetControlID(FocusType.Passive);
            Event currentEvent = Event.current;
            switch (currentEvent.GetTypeForControl(control_ID))
            {
                case EventType.MouseDown:
                    if ((HandleUtility.nearestControl == control_ID) && (currentEvent.button == 0) && (currentEvent.alt == false))
                    {
                        GUIUtility.hotControl = control_ID;
                        valueToSlide_duringMouseDown = valueToSlide;
                        mousePosition_duringMouseDown = currentEvent.mousePosition;
                        currentMousePosition = currentEvent.mousePosition;
                        InternalDXXL_BezierHandles.direction_duringMouseDown = direction_duringMouseDown;
                        currentEvent.Use();
                        EditorGUIUtility.SetWantsMouseJumping(1);
                    }
                    break;
                case EventType.MouseUp:
                    if (GUIUtility.hotControl == control_ID)
                    {
                        GUIUtility.hotControl = 0;
                        currentEvent.Use();
                        EditorGUIUtility.SetWantsMouseJumping(0);
                    }
                    break;
                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == control_ID)
                    {
                        currentMousePosition = currentMousePosition + currentEvent.delta;
                        float travelledDistance_alongDirection = HandleUtility.CalcLineTranslation(mousePosition_duringMouseDown, currentMousePosition, position, InternalDXXL_BezierHandles.direction_duringMouseDown) / size;
                        float slideSpeed = 0.1f;
                        valueToSlide = (Handles.SnapValue(travelledDistance_alongDirection * slideSpeed, snap) + 1.0f) * valueToSlide_duringMouseDown;
                        GUI.changed = true;
                        currentEvent.Use();
                    }
                    break;
                case EventType.Repaint:
                    Color color_before = Handles.color;
                    if (control_ID == GUIUtility.hotControl)
                    {
                        Handles.color = Handles.selectedColor;
                    }
                    else
                    {
                        if (IsHovering(control_ID, currentEvent))
                        {
                            Handles.color = Handles.preselectionColor;
                        }
                    }
                    capFunction(control_ID, position, rotation, size, EventType.Repaint);
                    Handles.color = color_before;
                    break;
                case EventType.Layout:
                    capFunction(control_ID, position, rotation, size, EventType.Layout);
                    break;
                default:
                    break;
            }
            return valueToSlide;
        }

        static bool IsHovering(int control_ID, Event currentEvent)
        {
            return ((GUIUtility.hotControl == 0) && (control_ID == HandleUtility.nearestControl) && (currentEvent.alt == false));
        }

        public static bool PlusButton(Vector3 position_inUnitsOfGlobalSpace, float sizeScaleFactor, Color color, Vector3 buttonPlaneNormalAwayFromObserver_normalized, Vector3 buttonPlaneUp_normalized, Vector3 buttonPlaneRight_normalized, GUIContent plusSymbolIcon)
        {
            Quaternion rotation_ofButton = Quaternion.LookRotation(buttonPlaneNormalAwayFromObserver_normalized, Vector3.zero);
            float unmodified_handleSize = HandleUtility.GetHandleSize(position_inUnitsOfGlobalSpace);
            float radius_ofButton = 0.5f * sizeScaleFactor * unmodified_handleSize; //"0.5f" factor because "Handles.CircleHandleCap" as a 2D cap seems to interpret the specified size as "radius" differently form the 3D caps that interpret it as "diameter".

            Handles.color = color;
            Handles.DrawSolidDisc(position_inUnitsOfGlobalSpace, buttonPlaneNormalAwayFromObserver_normalized, radius_ofButton);

            float shiftOffset_ofPlusSign = 0.09375f * unmodified_handleSize;
            Vector3 posOfPlusSign_inUnitsOfGlobalSpace = position_inUnitsOfGlobalSpace + shiftOffset_ofPlusSign * buttonPlaneUp_normalized - shiftOffset_ofPlusSign * buttonPlaneRight_normalized;
            Handles.Label(posOfPlusSign_inUnitsOfGlobalSpace, plusSymbolIcon);

            Handles.color = UtilitiesDXXL_Colors.GetSimilarColorWithOtherBrightnessValue(color);
            bool buttonHasBeenClicked = Handles.Button(position_inUnitsOfGlobalSpace, rotation_ofButton, radius_ofButton, radius_ofButton, Handles.CircleHandleCap);

            return buttonHasBeenClicked;
        }

    }

#endif
}
