namespace DrawXXL
{
    using UnityEngine;
    using System;

#if UNITY_EDITOR
    using UnityEditor;

    [CustomEditor(typeof(DrawXXL_LinesManager))]
    public class DrawXXL_LinesManagerInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            int indentLevel_before = EditorGUI.indentLevel;
            serializedObject.Update();
            
            EditorGUILayout.HelpBox("The checkbox below has this effect:" + Environment.NewLine + Environment.NewLine + "Outside of Playmode the Scene View window and Game View window do not ongoingly refresh themselfes as they do in playmode. Instead they just repaint on certain occasions, e.g. when a gameobject with a mesh is moved. When drawing in Edit mode from code this creates the problem that what you see on screen in the Scene or Game View doesn't neccessarily represent what you are currently drawing from code. For example if you draw a 'BoolDisplayer' to debug a changing bool value the display on screen may be wrong and still display an old value." + Environment.NewLine + "The Gizmo Line Count Manager Component here can fix this and let the display always show the most current value by continuously repainting the Scene and Game view. The downside of this fix is that it costs performance. Disabling it may make sense if you know that the things you are drawing anyway only change if a visible object in the Scene changes, e.g. when the player model changes its position or rotation." + Environment.NewLine + Environment.NewLine + "A disabled checkbox here may have no effect if other components are already continuously repainting the windows, like 'Draw XXL Chart Inspector' or 'Line Drawer' (with animated lines).", MessageType.None, true);

            SerializedProperty sP_gizmoLineCountManagerAutomaticallyRepaintsRendering = serializedObject.FindProperty("gizmoLineCountManagerAutomaticallyRepaintsRendering");
            sP_gizmoLineCountManagerAutomaticallyRepaintsRendering.boolValue = EditorGUILayout.ToggleLeft(new GUIContent("Continuous Repaint in Edit Mode"), sP_gizmoLineCountManagerAutomaticallyRepaintsRendering.boolValue);

            serializedObject.ApplyModifiedProperties();
            EditorGUI.indentLevel = indentLevel_before;
        }
    }
#endif
}
