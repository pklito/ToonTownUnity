namespace DrawXXL
{
    using System;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(DrawXXLSplineConnection))]
    public class DrawXXLSplineConnectionInspector : Editor
    {
        DrawXXLSplineConnection drawXXLSplineConnection_unserializedMonoB;

        void OnEnable()
        {
            drawXXLSplineConnection_unserializedMonoB = (DrawXXLSplineConnection)target;
        }

        public override void OnInspectorGUI()
        {
            string helpBoxString;
            if ((drawXXLSplineConnection_unserializedMonoB != null) && (drawXXLSplineConnection_unserializedMonoB.bezierSplineDrawer_thatHasReferencedThisGameobject != null))
            {
                helpBoxString = "This component handles the connection of this gameobject to the spline at " + drawXXLSplineConnection_unserializedMonoB.bezierSplineDrawer_thatHasReferencedThisGameobject.gameObject.name + ", specifically to the " + InternalDXXL_BezierControlSubPoint.GetSubPointTypeAsString(drawXXLSplineConnection_unserializedMonoB.subPointType_whereThisGameobjectIsBoundTo) + " of control point " + drawXXLSplineConnection_unserializedMonoB.i_ofControlPointTriplet_thisGameobjectIsBoundTo + " there." + Environment.NewLine + "It was automatically created and will get automatically deleted if it is not used anymore." + Environment.NewLine + "You can end the assignment in the spline inspector or simply by deleting this component.";
            }
            else
            {
                helpBoxString = "This component handles the connection of this gameobject to a spline, but it seems that the spline reference got lost.";
            }
            EditorGUILayout.HelpBox(helpBoxString, MessageType.Info, true);
        }
    }
#endif

}