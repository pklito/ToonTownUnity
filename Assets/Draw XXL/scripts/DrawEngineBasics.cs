namespace DrawXXL
{
    using UnityEngine;

    public class DrawEngineBasics
    {
        public static Color colorOfVector1_forDotProduct = UtilitiesDXXL_Colors.green_boolTrue; //This only affects the dot product drawings. It specifies the color of "vector1_lhs".
        public static Color colorOfVector2_forDotProduct = UtilitiesDXXL_Colors.red_boolFalse; //This only affects the dot product drawings. It specifies the color of "vector2_rhs".
        public static Color colorOfAngle_forDotProduct = UtilitiesDXXL_Colors.orange_lineThresholdMiddleDistance; //This only affects the dot product drawings. It specifies the color of the displayed angle when using "DotProduct".
        public static Color colorOfResult_forDotProduct = Color.white; //This only affects the dot product drawings. It specifies the color of the displayed result text when using "DotProduct". 

        public static Color colorOfVector1_forCrossProduct = UtilitiesDXXL_Colors.green_boolTrue; //This only affects the cross product drawings. It specifies the color of "vector1_lhs_leftThumb".
        public static Color colorOfVector2_forCrossProduct = UtilitiesDXXL_Colors.red_boolFalse; //This only affects the cross product drawings. It specifies the color of "vector2_rhs_leftIndexFinger".
        public static Color colorOfAngle_forCrossProduct = UtilitiesDXXL_Colors.orange_lineThresholdMiddleDistance; //This only affects the cross product drawings. It specifies the color of the displayed angle when using "CrossProduct".
        public static Color colorOfResultVector_forCrossProduct = UtilitiesDXXL_Colors.darkBlue; //This only affects the cross product drawings. It specifies the color of the displayed result vector when using "CrossProduct".
        public static Color colorOfResultText_forCrossProduct = Color.white; //This only affects the cross product drawings. It specifies the color of the displayed result text when using "CrossProduct".
        public static Color colorOfResultPlane_forCrossProduct = UtilitiesDXXL_Colors.violet; //This only affects the cross product drawings. It specifies the color of the displayed result plane when using "CrossProduct".

        public static Color overwriteColorForFrustumsHighlightedPlane = default(Color); //The default color for the highlighted plane of camera frustums is the color that has been specfied for the frustum itself, but with an adjusted brightness. You can overwrite this behaviour by setting this field.
        public static float distanceOfFrustumsHighlightedPlane = 0.0f;
        public static bool drawFrustumsHighlightedPlaneAlsoIfFarerThanFarClipPlane = true;

        public static bool hide_positionAroundWhichToDraw_forGrids = false;
        public static bool hide_distanceDisplay_forGrids = UtilitiesDXXL_Grid.default_hide_distanceDisplay_forGrids;
        public static float offsetForDistanceDisplays_inGrids = UtilitiesDXXL_Grid.default_offsetForDistanceDisplays_inGrids;
        public static float offsetForCoordinateTextDisplays_inGrids = UtilitiesDXXL_Grid.default_offsetForCoordinateTextDisplays_inGrids;
        public static float coveredGridUnits_rel_forGridPlanes = UtilitiesDXXL_Grid.default_coveredGridUnits_rel_forGridPlanes;

        private static float sizeScalingForCoordinateTexts_inGrids = UtilitiesDXXL_Grid.default_sizeScalingForCoordinateTexts_inGrids;
        public static float SizeScalingForCoordinateTexts_inGrids
        {
            get { return sizeScalingForCoordinateTexts_inGrids; }
            set
            {
                if (value < UtilitiesDXXL_Grid.min_sizeScalingForCoordinateTexts_inGrids)
                {
                    Debug.Log("Cannot set 'DrawEngineBasics.SizeScalingForCoordinateTexts_inGrids' to a smaller value than " + UtilitiesDXXL_Grid.min_sizeScalingForCoordinateTexts_inGrids);
                    sizeScalingForCoordinateTexts_inGrids = UtilitiesDXXL_Grid.min_sizeScalingForCoordinateTexts_inGrids;
                }
                else
                {
                    sizeScalingForCoordinateTexts_inGrids = value;
                }
            }
        }

        public static bool skipXYZAxisIdentifier_inCoordinateTextsOnGridAxes = false;
        public static bool skipLocalPrefix_inCoordinateTextsOnGridAxes = false;

        public static void Vector(Vector3 vectorStartPos, Vector3 vectorEndPos, Color color = default(Color), float lineWidth = 0.0f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vectorEndPos, "vectorEndPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vectorEndPos, "vectorEndPos")) { return; }
            VectorFrom(vectorStartPos, vectorEndPos - vectorStartPos, color, lineWidth, text, durationInSec, hiddenByNearerObjects);
        }

        public static void VectorFrom(Vector3 vectorStartPos, Vector3 vector, Color color = default(Color), float lineWidth = 0.0f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_EngineBasics.VectorFrom_local(Vector3.zero, Vector3.one, Quaternion.identity, vectorStartPos, vector, color, lineWidth, text, durationInSec, hiddenByNearerObjects, false);
        }

        public static void VectorTo(Vector3 vector, Vector3 vectorEndPos, Color color = default(Color), float lineWidth = 0.0f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vectorEndPos, "vectorEndPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vector, "vector")) { return; }
            VectorFrom(vectorEndPos - vector, vector, color, lineWidth, text, durationInSec, hiddenByNearerObjects);
        }

        public static void Position(GameObject gameObject, Color color = default(Color), float lineWidth = 0.0f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            VectorFrom(Vector3.zero, gameObject.transform.position, color, lineWidth, text, durationInSec, hiddenByNearerObjects);
        }

        public static void Position(Transform transform, Color color = default(Color), float lineWidth = 0.0f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            VectorFrom(Vector3.zero, transform.position, color, lineWidth, text, durationInSec, hiddenByNearerObjects);
        }

        public static void Position(Vector3 position, Color color = default(Color), float lineWidth = 0.0f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            VectorFrom(Vector3.zero, position, color, lineWidth, text, durationInSec, hiddenByNearerObjects);
        }

        public static void Vector_local(Transform parentTransform_thatDefinesTheLocalSpace, Vector3 vectorStartPos, Vector3 vectorEndPos, Color color = default(Color), float lineWidth_inGlobalUnits = 0.0f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vectorEndPos, "vectorEndPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vectorEndPos, "vectorEndPos")) { return; }
            VectorFrom_local(parentTransform_thatDefinesTheLocalSpace, vectorStartPos, vectorEndPos - vectorStartPos, color, lineWidth_inGlobalUnits, text, durationInSec, hiddenByNearerObjects);
        }

        public static void VectorFrom_local(Transform parentTransform_thatDefinesTheLocalSpace, Vector3 vectorStartPos, Vector3 vector, Color color = default(Color), float lineWidth_inGlobalUnits = 0.0f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

            if (parentTransform_thatDefinesTheLocalSpace == null)
            {
                text = "[<color=#adadadFF><icon=logMessage></color> parent transform that defines the local space is 'null'<br>   -> fallback to global space]<br>" + text;
                UtilitiesDXXL_EngineBasics.VectorFrom_local(Vector3.zero, Vector3.one, Quaternion.identity, vectorStartPos, vector, color, lineWidth_inGlobalUnits, text, durationInSec, hiddenByNearerObjects, false);
            }
            else
            {
                bool aParentHasANonUniformScale = UtilitiesDXXL_EngineBasics.CheckIf_transformOrAParentHasNonUniformScale(parentTransform_thatDefinesTheLocalSpace);
                if (aParentHasANonUniformScale)
                {
                    text = "[<color=#e2aa00FF><icon=warning></color> A parent transform that defines the local space has a non-uniform scale<br>   -> possibly weird results]<br>" + text;
                }
                UtilitiesDXXL_EngineBasics.VectorFrom_local(parentTransform_thatDefinesTheLocalSpace.position, parentTransform_thatDefinesTheLocalSpace.lossyScale, parentTransform_thatDefinesTheLocalSpace.rotation, vectorStartPos, vector, color, lineWidth_inGlobalUnits, text, durationInSec, hiddenByNearerObjects, true);
            }
        }

        public static void VectorTo_local(Transform parentTransform_thatDefinesTheLocalSpace, Vector3 vector, Vector3 vectorEndPos, Color color = default(Color), float lineWidth_inGlobalUnits = 0.0f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vectorEndPos, "vectorEndPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vector, "vector")) { return; }
            VectorFrom_local(parentTransform_thatDefinesTheLocalSpace, vectorEndPos - vector, vector, color, lineWidth_inGlobalUnits, text, durationInSec, hiddenByNearerObjects);
        }

        public static void Position_local(GameObject gameObject_insideLocalSpace, Color color = default(Color), float lineWidth_inGlobalUnits = 0.0f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(gameObject_insideLocalSpace, "gameObject_insideLocalSpace")) { return; }
            Position_local(gameObject_insideLocalSpace.transform, color, lineWidth_inGlobalUnits, text, durationInSec, hiddenByNearerObjects);
        }

        public static void Position_local(Transform transform_insideLocalSpace, Color color = default(Color), float lineWidth_inGlobalUnits = 0.0f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transform_insideLocalSpace, "transform_insideLocalSpace")) { return; }
            VectorFrom_local(transform_insideLocalSpace.parent, Vector3.zero, transform_insideLocalSpace.localPosition, color, lineWidth_inGlobalUnits, text, durationInSec, hiddenByNearerObjects);
        }

        public static void Position_local(Transform parentTransform_thatDefinesTheLocalSpace, Vector3 positionToDraw, Color color = default(Color), float lineWidth_inGlobalUnits = 0.0f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            VectorFrom_local(parentTransform_thatDefinesTheLocalSpace, Vector3.zero, positionToDraw, color, lineWidth_inGlobalUnits, text, durationInSec, hiddenByNearerObjects);
        }

        public static void Scale(GameObject gameObject, float lineWidth = 0.0035f, string text = null, bool drawXDim = true, bool drawYDim = true, bool drawZDim = true, float relSizeOfPlanes = 0.5f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(gameObject, "gameObject")) { return; }
            Scale(gameObject.transform, lineWidth, text, drawXDim, drawYDim, drawZDim, relSizeOfPlanes, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void Scale(Transform transform, float lineWidth = 0.0035f, string text = null, bool drawXDim = true, bool drawYDim = true, bool drawZDim = true, float relSizeOfPlanes = 0.5f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transform, "transform")) { return; }

            if (transform.parent != null)
            {
                text = "[<color=#adadadFF><icon=logMessage></color> Try drawing 'Scale', but the transform has a parent<br>   -> fallback to 'LocalScale', but the displayed length units fit global space]<br>" + text;
            }
            Scale(transform.position, transform.lossyScale, lineWidth, text, transform.rotation, drawXDim, drawYDim, drawZDim, relSizeOfPlanes, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void Scale(Vector3 centerPos, Vector3 scale, float lineWidth = 0.0035f, string text = null, Quaternion rotation = default(Quaternion), bool drawXDim = true, bool drawYDim = true, bool drawZDim = true, float relSizeOfPlanes = 0.5f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_EngineBasics.LocalScale(centerPos, scale, null, rotation, lineWidth, text, drawXDim, drawYDim, drawZDim, relSizeOfPlanes, overwriteColor, durationInSec, hiddenByNearerObjects, true);
        }

        public static void LocalScale(GameObject gameObject_insideLocalSpace, float lineWidth_inGlobalUnits = 0.0035f, string text = null, bool drawXDim = true, bool drawYDim = true, bool drawZDim = true, float relSizeOfPlanes = 0.5f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(gameObject_insideLocalSpace, "gameObject_insideLocalSpace")) { return; }
            LocalScale(gameObject_insideLocalSpace.transform, lineWidth_inGlobalUnits, text, drawXDim, drawYDim, drawZDim, relSizeOfPlanes, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void LocalScale(Transform transform_insideLocalSpace, float lineWidth_inGlobalUnits = 0.0035f, string text = null, bool drawXDim = true, bool drawYDim = true, bool drawZDim = true, float relSizeOfPlanes = 0.5f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transform_insideLocalSpace, "transform_insideLocalSpace")) { return; }
            LocalScale(transform_insideLocalSpace.localPosition, transform_insideLocalSpace.localScale, transform_insideLocalSpace.parent, transform_insideLocalSpace.localRotation, lineWidth_inGlobalUnits, text, drawXDim, drawYDim, drawZDim, relSizeOfPlanes, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void LocalScale(Vector3 localPosition, Vector3 localScale, Transform parentTransform, Quaternion localRotation = default(Quaternion), float lineWidth_inGlobalUnits = 0.0035f, string text = null, bool drawXDim = true, bool drawYDim = true, bool drawZDim = true, float relSizeOfPlanes = 0.5f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (parentTransform == null)
            {
                text = "[<color=#adadadFF><icon=logMessage></color> parent transform that defines the local space is 'null'<br>   -> fallback to global scale]<br>" + text;
                UtilitiesDXXL_EngineBasics.LocalScale(localPosition, localScale, parentTransform, localRotation, lineWidth_inGlobalUnits, text, drawXDim, drawYDim, drawZDim, relSizeOfPlanes, overwriteColor, durationInSec, hiddenByNearerObjects, true);
            }
            else
            {
                UtilitiesDXXL_EngineBasics.LocalScale(localPosition, localScale, parentTransform, localRotation, lineWidth_inGlobalUnits, text, drawXDim, drawYDim, drawZDim, relSizeOfPlanes, overwriteColor, durationInSec, hiddenByNearerObjects, false);
            }
        }

        public static void QuaternionRotation(GameObject gameObject, Vector3 customVectorToRotate = default(Vector3), float length_ofUpAndForwardVectors = 1.0f, Color color_ofTurnAxis = default(Color), float lineWidth = 0.0f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(gameObject, "gameObject")) { return; }
            QuaternionRotation(gameObject.transform, customVectorToRotate, length_ofUpAndForwardVectors, color_ofTurnAxis, lineWidth, text, durationInSec, hiddenByNearerObjects);
        }

        public static void QuaternionRotation(Transform transform, Vector3 customVectorToRotate = default(Vector3), float length_ofUpAndForwardVectors = 1.0f, Color color_ofTurnAxis = default(Color), float lineWidth = 0.0f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transform, "transform")) { return; }
            QuaternionRotation(transform.rotation, transform.position, customVectorToRotate, length_ofUpAndForwardVectors, color_ofTurnAxis, lineWidth, text, durationInSec, hiddenByNearerObjects);
        }

        public static void QuaternionRotation(Quaternion quaternion, Vector3 posWhereToDraw = default(Vector3), Vector3 customVectorToRotate = default(Vector3), float length_ofUpAndForwardVectors = 1.0f, Color color_ofTurnAxis = default(Color), float lineWidth = 0.0f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Quaternion.QuaternionRotation_local(quaternion, posWhereToDraw, color_ofTurnAxis, lineWidth, text, length_ofUpAndForwardVectors, customVectorToRotate, durationInSec, hiddenByNearerObjects, false, null);
        }

        public static void QuaternionRotation_local(GameObject gameObject_insideLocalSpace, Vector3 customVectorToRotate_local = default(Vector3), float length_ofUpAndForwardVectors_local = 1.0f, Color color_ofTurnAxis = default(Color), float lineWidth_inGlobalUnits = 0.0f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(gameObject_insideLocalSpace, "gameObject_insideLocalSpace")) { return; }
            QuaternionRotation_local(gameObject_insideLocalSpace.transform, customVectorToRotate_local, length_ofUpAndForwardVectors_local, color_ofTurnAxis, lineWidth_inGlobalUnits, text, durationInSec, hiddenByNearerObjects);
        }

        public static void QuaternionRotation_local(Transform transform_insideLocalSpace, Vector3 customVectorToRotate_local = default(Vector3), float length_ofUpAndForwardVectors_local = 1.0f, Color color_ofTurnAxis = default(Color), float lineWidth_inGlobalUnits = 0.0f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transform_insideLocalSpace, "transform_insideLocalSpace")) { return; }

            if (transform_insideLocalSpace.parent == null)
            {
                text = "[<color=#adadadFF><icon=logMessage></color> parent transform that defines<br>   the local space is 'null'<br>   -> fallback to global rotation]<br>" + text;
                UtilitiesDXXL_Quaternion.QuaternionRotation_local(transform_insideLocalSpace.localRotation, transform_insideLocalSpace.position, color_ofTurnAxis, lineWidth_inGlobalUnits, text, length_ofUpAndForwardVectors_local, customVectorToRotate_local, durationInSec, hiddenByNearerObjects, false, null);
            }
            else
            {
                bool aParentHasANonUniformScale = UtilitiesDXXL_EngineBasics.CheckIf_transformOrAParentHasNonUniformScale(transform_insideLocalSpace.parent);
                if (aParentHasANonUniformScale)
                {
                    text = "[<color=#e2aa00FF><icon=warning></color> A parent transform that defines the<br>   local space has a non-uniform scale<br>   -> possibly weird results]<br>" + text;
                }
                UtilitiesDXXL_Quaternion.QuaternionRotation_local(transform_insideLocalSpace.localRotation, transform_insideLocalSpace.position, color_ofTurnAxis, lineWidth_inGlobalUnits, text, length_ofUpAndForwardVectors_local, customVectorToRotate_local, durationInSec, hiddenByNearerObjects, true, transform_insideLocalSpace.parent);
            }
        }

        public static void QuaternionRotation_local(Transform parentTransform_thatDefinesTheLocalSpace, Quaternion quaternionToDraw_local, Vector3 posWhereToDraw_global = default(Vector3), Vector3 customVectorToRotate_local = default(Vector3), float length_ofUpAndForwardVectors_local = 1.0f, Color color_ofTurnAxis = default(Color), float lineWidth_inGlobalUnits = 0.0f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

            if (parentTransform_thatDefinesTheLocalSpace == null)
            {
                text = "[<color=#adadadFF><icon=logMessage></color> parent transform that defines<br>   the local space is 'null'<br>   -> fallback to global rotation]<br>" + text;
                UtilitiesDXXL_Quaternion.QuaternionRotation_local(quaternionToDraw_local, posWhereToDraw_global, color_ofTurnAxis, lineWidth_inGlobalUnits, text, length_ofUpAndForwardVectors_local, customVectorToRotate_local, durationInSec, hiddenByNearerObjects, false, null);
            }
            else
            {
                bool aParentHasANonUniformScale = UtilitiesDXXL_EngineBasics.CheckIf_transformOrAParentHasNonUniformScale(parentTransform_thatDefinesTheLocalSpace);
                if (aParentHasANonUniformScale)
                {
                    text = "[<color=#e2aa00FF><icon=warning></color> A parent transform that defines the<br>   local space has a non-uniform scale<br>   -> possibly weird results]<br>" + text;
                }
                UtilitiesDXXL_Quaternion.QuaternionRotation_local(quaternionToDraw_local, posWhereToDraw_global, color_ofTurnAxis, lineWidth_inGlobalUnits, text, length_ofUpAndForwardVectors_local, customVectorToRotate_local, durationInSec, hiddenByNearerObjects, true, parentTransform_thatDefinesTheLocalSpace);
            }
        }

        public static void EulerRotation(GameObject gameObject, Vector3 customVectorToRotate = default(Vector3), float length_ofUpAndForwardVectors = 0.0f, float alpha_ofSquareSpannedByForwardAndUp = 0.0f, string text = null, bool useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay = false, float alpha_ofUnrotatedGimbalAxes = 0.06f, float gimbalSize = 1.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(gameObject, "gameObject")) { return; }
            EulerRotation(gameObject.transform, customVectorToRotate, length_ofUpAndForwardVectors, alpha_ofSquareSpannedByForwardAndUp, text, useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay, alpha_ofUnrotatedGimbalAxes, gimbalSize, durationInSec, hiddenByNearerObjects);
        }

        public static void EulerRotation(Transform transform, Vector3 customVectorToRotate = default(Vector3), float length_ofUpAndForwardVectors = 0.0f, float alpha_ofSquareSpannedByForwardAndUp = 0.0f, string text = null, bool useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay = false, float alpha_ofUnrotatedGimbalAxes = 0.06f, float gimbalSize = 1.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transform, "transform")) { return; }

            if (useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay == false)
            {
                if (transform.parent != null)
                {
                    text = "[<color=#adadadFF><icon=logMessage></color> The transform has a parent, but the angle values from the inspector show the local rotation<br>   -> fallback to values from transform.eulerAngles<br>   -> for further infos see documentation of 'useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay']<br>" + text;
                    useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay = true;
                }
            }
            Vector3 eulerAnglesToDraw = UtilitiesDXXL_Euler.GetEulerAnglesFromNonNullTransform(transform, useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay, false);
            UtilitiesDXXL_Euler.EulerRotation_local(eulerAnglesToDraw, transform.position, customVectorToRotate, length_ofUpAndForwardVectors, alpha_ofSquareSpannedByForwardAndUp, alpha_ofUnrotatedGimbalAxes, gimbalSize, text, false, durationInSec, hiddenByNearerObjects, null);
        }

        public static void EulerRotation(Vector3 eulerAnglesToDraw, Vector3 posWhereToDraw = default(Vector3), Vector3 customVectorToRotate = default(Vector3), float length_ofUpAndForwardVectors = 0.0f, float alpha_ofSquareSpannedByForwardAndUp = 0.0f, string text = null, float alpha_ofUnrotatedGimbalAxes = 0.06f, float gimbalSize = 1.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Euler.EulerRotation_local(eulerAnglesToDraw, posWhereToDraw, customVectorToRotate, length_ofUpAndForwardVectors, alpha_ofSquareSpannedByForwardAndUp, alpha_ofUnrotatedGimbalAxes, gimbalSize, text, false, durationInSec, hiddenByNearerObjects, null);
        }

        public static void EulerRotation(Quaternion quaternionToDrawAsEulerAngles, Vector3 posWhereToDraw = default(Vector3), Vector3 customVectorToRotate = default(Vector3), float length_ofUpAndForwardVectors = 0.0f, float alpha_ofSquareSpannedByForwardAndUp = 0.0f, string text = null, float alpha_ofUnrotatedGimbalAxes = 0.06f, float gimbalSize = 1.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            EulerRotation(quaternionToDrawAsEulerAngles.eulerAngles, posWhereToDraw, customVectorToRotate, length_ofUpAndForwardVectors, alpha_ofSquareSpannedByForwardAndUp, text, alpha_ofUnrotatedGimbalAxes, gimbalSize, durationInSec, hiddenByNearerObjects);
        }

        public static void EulerRotation_local(GameObject gameObject_insideLocalSpace, Vector3 customVectorToRotate_local = default(Vector3), float length_ofUpAndForwardVectors_local = 0.0f, float alpha_ofSquareSpannedByForwardAndUp = 0.0f, string text = null, bool useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay = false, float alpha_ofUnrotatedGimbalAxes = 0.06f, float gimbalSize_local = 1.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(gameObject_insideLocalSpace, "gameObject_insideLocalSpace")) { return; }
            EulerRotation_local(gameObject_insideLocalSpace.transform, customVectorToRotate_local, length_ofUpAndForwardVectors_local, alpha_ofSquareSpannedByForwardAndUp, text, useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay, alpha_ofUnrotatedGimbalAxes, gimbalSize_local, durationInSec, hiddenByNearerObjects);
        }

        public static void EulerRotation_local(Transform transform_insideLocalSpace, Vector3 customVectorToRotate_local = default(Vector3), float length_ofUpAndForwardVectors_local = 0.0f, float alpha_ofSquareSpannedByForwardAndUp = 0.0f, string text = null, bool useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay = false, float alpha_ofUnrotatedGimbalAxes = 0.06f, float gimbalSize_local = 1.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transform_insideLocalSpace, "transform_insideLocalSpace")) { return; }
            Vector3 eulerAnglesToDraw = UtilitiesDXXL_Euler.GetEulerAnglesFromNonNullTransform(transform_insideLocalSpace, useAnglesFromQuaternion_notFromEditorsTransformInspectorDisplay, true);
            if (transform_insideLocalSpace.parent == null)
            {
                text = "[<color=#adadadFF><icon=logMessage></color> parent transform that defines the local space is 'null'<br>   -> fallback to global rotation]<br>" + text;
                UtilitiesDXXL_Euler.EulerRotation_local(eulerAnglesToDraw, transform_insideLocalSpace.position, customVectorToRotate_local, length_ofUpAndForwardVectors_local, alpha_ofSquareSpannedByForwardAndUp, alpha_ofUnrotatedGimbalAxes, gimbalSize_local, text, false, durationInSec, hiddenByNearerObjects, null);
            }
            else
            {
                bool aParentHasANonUniformScale = UtilitiesDXXL_EngineBasics.CheckIf_transformOrAParentHasNonUniformScale(transform_insideLocalSpace.parent);
                if (aParentHasANonUniformScale)
                {
                    text = "[<color=#e2aa00FF><icon=warning></color> A parent transform that defines the local space has a non-uniform scale<br>   -> possibly weird results]<br>" + text;
                }
                float gimbalSize_global = transform_insideLocalSpace.parent.lossyScale.x * gimbalSize_local;
                UtilitiesDXXL_Euler.EulerRotation_local(eulerAnglesToDraw, transform_insideLocalSpace.position, customVectorToRotate_local, length_ofUpAndForwardVectors_local, alpha_ofSquareSpannedByForwardAndUp, alpha_ofUnrotatedGimbalAxes, gimbalSize_global, text, true, durationInSec, hiddenByNearerObjects, transform_insideLocalSpace.parent);
            }
        }

        public static void EulerRotation_local(Transform parentTransform_thatDefinesTheLocalSpace, Vector3 eulerAnglesToDraw_local, Vector3 posWhereToDraw_global = default(Vector3), Vector3 customVectorToRotate_local = default(Vector3), float length_ofUpAndForwardVectors_local = 0.0f, float alpha_ofSquareSpannedByForwardAndUp = 0.0f, string text = null, float alpha_ofUnrotatedGimbalAxes = 0.06f, float gimbalSize_local = 1.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

            if (parentTransform_thatDefinesTheLocalSpace == null)
            {
                text = "[<color=#adadadFF><icon=logMessage></color> parent transform that defines the local space is 'null'<br>   -> fallback to global rotation]<br>" + text;
                UtilitiesDXXL_Euler.EulerRotation_local(eulerAnglesToDraw_local, posWhereToDraw_global, customVectorToRotate_local, length_ofUpAndForwardVectors_local, alpha_ofSquareSpannedByForwardAndUp, alpha_ofUnrotatedGimbalAxes, gimbalSize_local, text, false, durationInSec, hiddenByNearerObjects, null);
            }
            else
            {
                bool aParentHasANonUniformScale = UtilitiesDXXL_EngineBasics.CheckIf_transformOrAParentHasNonUniformScale(parentTransform_thatDefinesTheLocalSpace);
                if (aParentHasANonUniformScale)
                {
                    text = "[<color=#e2aa00FF><icon=warning></color> A parent transform that defines the local space has a non-uniform scale<br>   -> possibly weird results]<br>" + text;
                }
                float gimbalSize_global = parentTransform_thatDefinesTheLocalSpace.lossyScale.x * gimbalSize_local;
                UtilitiesDXXL_Euler.EulerRotation_local(eulerAnglesToDraw_local, posWhereToDraw_global, customVectorToRotate_local, length_ofUpAndForwardVectors_local, alpha_ofSquareSpannedByForwardAndUp, alpha_ofUnrotatedGimbalAxes, gimbalSize_global, text, true, durationInSec, hiddenByNearerObjects, parentTransform_thatDefinesTheLocalSpace);
            }
        }

        public static void EulerRotation_local(Transform parentTransform_thatDefinesTheLocalSpace, Quaternion quaternionToDrawAsEulerAngles_local, Vector3 posWhereToDraw_global = default(Vector3), Vector3 customVectorToRotate_local = default(Vector3), float length_ofUpAndForwardVectors_local = 0.0f, float alpha_ofSquareSpannedByForwardAndUp = 0.0f, string text = null, float alpha_ofUnrotatedGimbalAxes = 0.06f, float gimbalSize_local = 1.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            EulerRotation_local(parentTransform_thatDefinesTheLocalSpace, quaternionToDrawAsEulerAngles_local.eulerAngles, posWhereToDraw_global, customVectorToRotate_local, length_ofUpAndForwardVectors_local, alpha_ofSquareSpannedByForwardAndUp, text, alpha_ofUnrotatedGimbalAxes, gimbalSize_local, durationInSec, hiddenByNearerObjects);
        }

        public static void Bounds(GameObject gameObject, Color color = default(Color), bool alsoDrawLocalBounds = true, bool showAlsoBoundsOfChildren = true, float linesWidth = 0.01f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(gameObject, "gameObject")) { return; }
            Bounds(gameObject.transform, color, alsoDrawLocalBounds, showAlsoBoundsOfChildren, linesWidth, text, durationInSec, hiddenByNearerObjects);
        }

        public static void LocalBounds(GameObject gameObject, Color color = default(Color), bool showAlsoBoundsOfChildren = true, float lineWidth_inGlobalUnits = 0.01f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(gameObject, "gameObject")) { return; }
            LocalBounds(gameObject.transform, color, showAlsoBoundsOfChildren, lineWidth_inGlobalUnits, text, durationInSec, hiddenByNearerObjects);
        }

        public static void Bounds(Transform transform, Color color = default(Color), bool alsoDrawLocalBounds = true, bool showAlsoBoundsOfChildren = true, float linesWidth = 0.01f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transform, "transform")) { return; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);
            if (alsoDrawLocalBounds)
            {
                linesWidth = Mathf.Max(linesWidth, 0.01f);
            }

            color = UtilitiesDXXL_Colors.OverwriteDefaultColor(color);

            Transform[] thisAndAllChildTransforms = transform.GetComponentsInChildren<Transform>();
            int numberOfDrawnBounds = 0;

            if (thisAndAllChildTransforms == null)
            {
                UtilitiesDXXL_Log.PrintErrorCode("25");
                return;
            }
            else
            {
                bool textHasToBeDrawnYet = true;
                DrawGlobalBoundsIfTransformHasAMeshRenderer(ref numberOfDrawnBounds, out textHasToBeDrawnYet, transform, color, linesWidth, text, durationInSec, hiddenByNearerObjects);

                if (showAlsoBoundsOfChildren)
                {
                    for (int i = 0; i < thisAndAllChildTransforms.Length; i++)
                    {
                        if (thisAndAllChildTransforms[i] != transform)
                        {
                            DrawGlobalBoundsIfTransformHasAMeshRenderer(ref numberOfDrawnBounds, out textHasToBeDrawnYet, thisAndAllChildTransforms[i], color, linesWidth, textHasToBeDrawnYet ? text : null, durationInSec, hiddenByNearerObjects);
                        }
                    }
                }

                if (numberOfDrawnBounds == 0)
                {
                    UtilitiesDXXL_DrawBasics.PointFallback(transform.position, "[<color=#adadadFF><icon=logMessage></color> No bounds found on this GameObject]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                    return;
                }
            }

            if (alsoDrawLocalBounds)
            {
                if (numberOfDrawnBounds > 0)
                {
                    Color colorForLocalBounds = UtilitiesDXXL_Colors.GetSimilarColorWithOtherBrightnessValue(color);
                    LocalBounds(transform, colorForLocalBounds, showAlsoBoundsOfChildren, 0.15f * linesWidth, null, durationInSec, hiddenByNearerObjects);
                }
            }

        }

        static void DrawGlobalBoundsIfTransformHasAMeshRenderer(ref int numberOfDrawnBounds, out bool textHasToBeDrawnYet, Transform transformToCheck, Color color, float linesWidth, string text, float durationInSec, bool hiddenByNearerObjects)
        {
            textHasToBeDrawnYet = (text != null);
            MeshRenderer meshRenderer = transformToCheck.GetComponent<MeshRenderer>();
            if (meshRenderer != null)
            {
                if (meshRenderer.bounds != null)
                {
                    Bounds(meshRenderer.bounds, color, linesWidth, text, durationInSec, hiddenByNearerObjects);
                    numberOfDrawnBounds++;
                    textHasToBeDrawnYet = false;
                }
            }

            SkinnedMeshRenderer skinnedMeshRenderer = transformToCheck.gameObject.GetComponent<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer != null)
            {
                if (skinnedMeshRenderer.bounds != null)
                {
                    Bounds(skinnedMeshRenderer.bounds, color, linesWidth, text, durationInSec, hiddenByNearerObjects);
                    numberOfDrawnBounds++;
                    textHasToBeDrawnYet = false;
                }
            }
        }

        public static void LocalBounds(Transform transform, Color color = default(Color), bool showAlsoBoundsOfChildren = true, float lineWidth_inGlobalUnits = 0.01f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transform, "transform")) { return; }

            Transform[] thisAndAllChildTransforms = transform.GetComponentsInChildren<Transform>();
            int numberOfDrawnBounds = 0;
            bool textHasToBeDrawnYet = true;

            DrawLocalBoundsIfTransformHasAMeshFilterOrMeshSkinnedRenderer(ref numberOfDrawnBounds, out textHasToBeDrawnYet, transform, color, lineWidth_inGlobalUnits, text, durationInSec, hiddenByNearerObjects);

            if (showAlsoBoundsOfChildren)
            {
                for (int i = 0; i < thisAndAllChildTransforms.Length; i++)
                {
                    if (thisAndAllChildTransforms[i] != transform)
                    {
                        DrawLocalBoundsIfTransformHasAMeshFilterOrMeshSkinnedRenderer(ref numberOfDrawnBounds, out textHasToBeDrawnYet, thisAndAllChildTransforms[i], color, lineWidth_inGlobalUnits, textHasToBeDrawnYet ? text : null, durationInSec, hiddenByNearerObjects);
                    }
                }
            }

            if (numberOfDrawnBounds == 0)
            {
                UtilitiesDXXL_DrawBasics.PointFallback(transform.position, "[<color=#adadadFF><icon=logMessage></color> No bounds found on this GameObject]<br>" + text, color, lineWidth_inGlobalUnits, durationInSec, hiddenByNearerObjects);
                return;
            }
        }

        static void DrawLocalBoundsIfTransformHasAMeshFilterOrMeshSkinnedRenderer(ref int numberOfDrawnBounds, out bool textHasToBeDrawnYet, Transform transformToCheck, Color color, float lineWidth_inGlobalUnits, string text, float durationInSec, bool hiddenByNearerObjects)
        {
            textHasToBeDrawnYet = (text != null);

            MeshFilter meshfilter = transformToCheck.gameObject.GetComponent<MeshFilter>();
            if (meshfilter != null)
            {
                if (Application.isPlaying)
                {
                    if (meshfilter.mesh != null)
                    {
                        if (meshfilter.mesh.bounds != null)
                        {
                            LocalBounds(meshfilter.mesh.bounds, transformToCheck, color, lineWidth_inGlobalUnits, text, durationInSec, hiddenByNearerObjects);
                            numberOfDrawnBounds++;
                            textHasToBeDrawnYet = false;
                        }
                    }
                }
                else
                {
                    if (meshfilter.sharedMesh != null)
                    {
                        if (meshfilter.sharedMesh.bounds != null)
                        {
                            LocalBounds(meshfilter.sharedMesh.bounds, transformToCheck, color, lineWidth_inGlobalUnits, text, durationInSec, hiddenByNearerObjects);
                            numberOfDrawnBounds++;
                            textHasToBeDrawnYet = false;
                        }
                    }
                }
            }

            SkinnedMeshRenderer skinnedMeshRenderer = transformToCheck.gameObject.GetComponent<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer != null)
            {
                if (skinnedMeshRenderer.localBounds != null)
                {
                    if (skinnedMeshRenderer.rootBone == null)
                    {
                        text = "[<color=#e2aa00FF><icon=warning></color> Local Bounds potentially at the wrong place, because the Skinned Mesh Renderer has no root bone assigned]<br>" + text;
                        LocalBounds(skinnedMeshRenderer.localBounds, transformToCheck, color, lineWidth_inGlobalUnits, text, durationInSec, hiddenByNearerObjects);
                    }
                    else
                    {
                        LocalBounds(skinnedMeshRenderer.localBounds, skinnedMeshRenderer.rootBone, color, lineWidth_inGlobalUnits, text, durationInSec, hiddenByNearerObjects);
                    }

                    numberOfDrawnBounds++;
                    textHasToBeDrawnYet = false;
                }
            }
        }

        public static void Bounds(Bounds bounds, Color color = default(Color), float linesWidth = 0.01f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return; }

            color = UtilitiesDXXL_Colors.OverwriteDefaultColor(color);

            if (UtilitiesDXXL_Math.ApproximatelyZero(bounds.size))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(bounds.center, "[<color=#adadadFF><icon=logMessage></color> Bounds with size of 0]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return;
            }

            if (UtilitiesDXXL_Math.GetSmallestComponent(bounds.extents) < 0.0f)
            {
                text = "[<color=#e2aa00FF><icon=warning></color> Bounds.extents ( " + bounds.extents.x + " , " + bounds.extents.y + " , " + bounds.extents.z + " ) contains negative values -> 'Bounds.Contains()' will always return 'false']<br>" + text;
            }

            //text ABOVE line (in contrast to "LocalBounds" which may often be superimposed):
            UtilitiesDXXL_Shapes.Cube(bounds.center, bounds.size, color, color, Vector3.up, Vector3.forward, linesWidth, text + "<br><size=3> </size><br><sw=80000>Bounds</sw>", DrawBasics.LineStyle.disconnectedAnchors, 1.0f, true, durationInSec, hiddenByNearerObjects, false, null);
        }

        public static void LocalBounds(Bounds localBounds, Transform transformDefiningLocalSpace, Color color = default(Color), float lineWidth_inGlobalUnits = 0.01f, string text = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transformDefiningLocalSpace, "transformDefiningLocalSpace")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lineWidth_inGlobalUnits, "linesWidth")) { return; }

            color = UtilitiesDXXL_Colors.OverwriteDefaultColor(color);
            Vector3 boundsCenter_worldSpace = transformDefiningLocalSpace.TransformPoint(localBounds.center);

            if (UtilitiesDXXL_Math.ApproximatelyZero(localBounds.size))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(boundsCenter_worldSpace, "[<color=#adadadFF><icon=logMessage></color> LocalBounds with size of 0 (in localSpace)]<br>" + text, color, lineWidth_inGlobalUnits, durationInSec, hiddenByNearerObjects);
                return;
            }

            Vector3 globalSize = Vector3.Scale(localBounds.size, transformDefiningLocalSpace.lossyScale);
            if (UtilitiesDXXL_Math.ApproximatelyZero(globalSize))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(boundsCenter_worldSpace, "[<color=#adadadFF><icon=logMessage></color> LocalBounds with size of 0 (in worldspace)]<br>" + text, color, lineWidth_inGlobalUnits, durationInSec, hiddenByNearerObjects);
                return;
            }

            if (UtilitiesDXXL_Math.GetSmallestComponent(localBounds.extents) < 0.0f)
            {
                text = "[<color=#e2aa00FF><icon=warning></color> LocalBounds.extents ( " + localBounds.extents.x + " , " + localBounds.extents.y + " , " + localBounds.extents.z + " ) contains negative values -> 'Bounds.Contains()' will always return 'false']<br>" + text;
            }

            if (UtilitiesDXXL_EngineBasics.CheckIf_transformOrAParentHasNonUniformScale(transformDefiningLocalSpace.parent))
            {
                text = "[<color=#e2aa00FF><icon=warning></color> LocalBounds: The local transform has a parent with non-uniform scale<br>   -> possibly weird results]<br>" + text;
            }

            //text BELOW line (in contrast to "(Global)Bounds" which may often be superimposed):
            UtilitiesDXXL_Shapes.Cube(boundsCenter_worldSpace, globalSize, color, color, transformDefiningLocalSpace.up, transformDefiningLocalSpace.forward, lineWidth_inGlobalUnits, "<sw=80000>Local Bounds</sw><br><size=3> </size><br>" + text, DrawBasics.LineStyle.disconnectedAnchors, 1.0f, false, durationInSec, hiddenByNearerObjects, false, null);
        }

        public static void DotProduct(Vector3 vector1_lhs, Vector3 vector2_rhs, Vector3 posWhereToDraw = default(Vector3), float linesWidth = 0.0025f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            //"lhs" = "left hand side (of equation)". 
            //"rhs" = "right hand side (of equation)"

            UtilitiesDXXL_EngineBasics.DotProduct(vector1_lhs, vector2_rhs, posWhereToDraw, linesWidth, durationInSec, hiddenByNearerObjects);
        }

        public static void CrossProduct(Vector3 vector1_lhs_leftThumb, Vector3 vector2_rhs_leftIndexFinger, Vector3 posWhereToDraw = default(Vector3), float linesWidth = 0.0025f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            //"lhs" = "left hand side (of equation)"
            //"rhs" = "right hand side (of equation)"
            //cross product result is "left middle finger" according to the left hand rule

            UtilitiesDXXL_EngineBasics.CrossProduct(vector1_lhs_leftThumb, vector2_rhs_leftIndexFinger, posWhereToDraw, linesWidth, durationInSec, hiddenByNearerObjects);
        }

        public static void TagGameObject(GameObject gameObject, string text = null, Color colorForText = default(Color), Color colorForTagBox = default(Color), float textSize = 0.2f, float linesWidth = 0.0f, bool encapsulateChildren = true, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(gameObject, "gameObject")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(textSize, "textSize")) { return; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);

            if (UtilitiesDXXL_Colors.IsDefaultColor(colorForText))
            {
                colorForText = UtilitiesDXXL_Colors.Get_randomColorSeeded(gameObject.GetInstanceID());
            }

            if (UtilitiesDXXL_Colors.IsDefaultColor(colorForTagBox))
            {
                colorForTagBox = UtilitiesDXXL_Colors.Get_randomColorSeeded(gameObject.GetInstanceID());
            }

            UtilitiesDXXL_EngineBasics.FillBounds(gameObject, encapsulateChildren, out Vector3 globalExtents, out Vector3 globalCenter, out bool rotateBoundingBox);
            UtilitiesDXXL_EngineBasics.FillTagBoxOrientationVectors(gameObject, rotateBoundingBox, out Vector3 tagBoxUp, out Vector3 tagBoxForward);

            if (UtilitiesDXXL_Math.ApproximatelyZero(globalExtents))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(gameObject.transform.position, "[<color=#adadadFF><icon=logMessage></color> GameObject with extent of zero]<br>" + text, colorForTagBox, linesWidth, durationInSec, hiddenByNearerObjects);
                return;
            }

            bool textSize_isZero = UtilitiesDXXL_Math.ApproximatelyZero(textSize);
            if (textSize_isZero)
            {
                UtilitiesDXXL_Shapes.Set_forcedConstantWorldspaceTextSize_forTextAtShapes_reversible(0.0f); //-> text will be relative to gameobject size, or to screenspace
            }
            else
            {
                UtilitiesDXXL_Shapes.Set_forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes_reversible(0.0f); //-> disable fixed screenspace size, since it would overrule the fixed worldspace size
                UtilitiesDXXL_Shapes.Set_forcedConstantWorldspaceTextSize_forTextAtShapes_reversible(textSize);
            }

            UtilitiesDXXL_Shapes.Cube(globalCenter, 2.0f * globalExtents, colorForTagBox, colorForText, tagBoxUp, tagBoxForward, linesWidth, text, DrawBasics.LineStyle.disconnectedAnchors, 1.0f, textBlockAboveLine, durationInSec, hiddenByNearerObjects, false, gameObject.name);

            if (textSize_isZero)
            {
                UtilitiesDXXL_Shapes.Reverse_forcedConstantWorldspaceTextSize_forTextAtShapes();
            }
            else
            {
                UtilitiesDXXL_Shapes.Reverse_forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes();
                UtilitiesDXXL_Shapes.Reverse_forcedConstantWorldspaceTextSize_forTextAtShapes();
            }

        }

        public static void TagGameObjectScreenspace(GameObject gameObject, string text = null, Color colorForText = default(Color), Color colorForTagBox = default(Color), float linesWidth_relToViewportHeight = 0.0f, bool drawPointerIfOffscreen = true, float relTextSizeScaling = 1.0f, bool encapsulateChildren = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawEngineBasics.TagGameObjectScreenspace") == false) { return; }
            TagGameObjectScreenspace(automaticallyFoundCamera, gameObject, text, colorForText, colorForTagBox, linesWidth_relToViewportHeight, drawPointerIfOffscreen, relTextSizeScaling, encapsulateChildren, durationInSec);
        }

        public static void TagGameObjectScreenspace(Camera screenCamera, GameObject gameObject, string text = null, Color colorForText = default(Color), Color colorForTagBox = default(Color), float linesWidth_relToViewportHeight = 0.0f, bool drawPointerIfOffscreen = true, float relTextSizeScaling = 1.0f, bool encapsulateChildren = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_TagGameObjectScreenspace.Add(new TagGameObjectScreenspace(screenCamera, gameObject, text, colorForText, colorForTagBox, linesWidth_relToViewportHeight, drawPointerIfOffscreen, relTextSizeScaling, encapsulateChildren, durationInSec));
                return;
            }

            UtilitiesDXXL_EngineBasics.TagGameObjectScreenspace(screenCamera, gameObject, text, colorForText, colorForTagBox, linesWidth_relToViewportHeight, drawPointerIfOffscreen, relTextSizeScaling, encapsulateChildren, durationInSec);
        }

        public static void Camera(Vector3 position, Quaternion rotation = default(Quaternion), Color color = default(Color), string text = null, float linesWidth = 0.0f, float nearClipPlaneDistance = 0.3f, float fieldOfViewAngleDeg = 60.0f, float aspectRatioOfScreen = 16.0f / 9.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Camera(position, rotation * Vector3.forward, rotation * Vector3.up, color, text, linesWidth, nearClipPlaneDistance, fieldOfViewAngleDeg, aspectRatioOfScreen, durationInSec, hiddenByNearerObjects);
        }

        static InternalDXXL_Plane camPlane = new InternalDXXL_Plane();
        public static void Camera(Vector3 position, Vector3 forward = default(Vector3), Vector3 up = default(Vector3), Color color = default(Color), string text = null, float linesWidth = 0.0f, float nearClipPlaneDistance = 0.3f, float fieldOfViewAngleDeg = 60.0f, float aspectRatioOfScreen = 16.0f / 9.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(forward, "forward")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up, "up")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(nearClipPlaneDistance, "nearClipPlaneDistance")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(fieldOfViewAngleDeg, "fieldOfViewAngleDeg")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(aspectRatioOfScreen, "aspectRatioOfScreen")) { return; }

            forward = UtilitiesDXXL_Math.OverwriteDefaultVectors(forward, Vector3.forward);
            camPlane.Recreate(position, forward);
            up = UtilitiesDXXL_Math.OverwriteDefaultVectors(up, Vector3.up);
            up = UtilitiesDXXL_Shapes.ForceVectorPerpToOtherVector(up, camPlane);

            aspectRatioOfScreen = UtilitiesDXXL_Math.AbsNonZeroValue(aspectRatioOfScreen);
            if (aspectRatioOfScreen < 0.001f)
            {
                UtilitiesDXXL_DrawBasics.PointFallback(position, "[<color=#adadadFF><icon=logMessage></color> Camera with aspectRatioOfScreen near 0]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return;
            }

            nearClipPlaneDistance = Mathf.Max(nearClipPlaneDistance, 0.01f);
            fieldOfViewAngleDeg = Mathf.Clamp(fieldOfViewAngleDeg, 0.1f, 179.9f);

            UtilitiesDXXL_EngineBasics.Camera(position, forward, up, false, 5.0f, fieldOfViewAngleDeg, nearClipPlaneDistance, aspectRatioOfScreen, color, text, linesWidth, durationInSec, hiddenByNearerObjects);
        }

        public static void Camera(Camera camera, Color color = default(Color), string text = null, float linesWidth = 0.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(camera, "camera")) { return; }
            UtilitiesDXXL_EngineBasics.Camera(camera.transform.position, camera.transform.forward, camera.transform.up, camera.orthographic, camera.orthographicSize, camera.fieldOfView, camera.nearClipPlane, camera.aspect, color, text, linesWidth, durationInSec, hiddenByNearerObjects);
        }

        public static void CameraFrustum(Vector3 position_ofCamera, Quaternion rotation_ofCamera = default(Quaternion), Color color = default(Color), float nearClipPlaneDistance = 0.3f, float farClipPlaneDistance = 1000.0f, float fieldOfViewAngleDeg = 60.0f, float aspectRatioOfScreen = 16.0f / 9.0f, float alphaFactor_forBoundarySurfaceLines = 0.18f, float linesWidth_ofEdges = 0.0f, int linesPerBoundarySurface = 60, string text = null, bool forceTextOnNearPlaneUnmirroredTowardsCam = true, Vector3 positionOnHighlightedPlane = default(Vector3), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            rotation_ofCamera = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation_ofCamera);
            CameraFrustum(position_ofCamera, rotation_ofCamera * Vector3.forward, rotation_ofCamera * Vector3.up, color, nearClipPlaneDistance, farClipPlaneDistance, fieldOfViewAngleDeg, aspectRatioOfScreen, alphaFactor_forBoundarySurfaceLines, linesWidth_ofEdges, linesPerBoundarySurface, text, forceTextOnNearPlaneUnmirroredTowardsCam, positionOnHighlightedPlane, durationInSec, hiddenByNearerObjects);
        }

        public static void CameraFrustum(Vector3 position_ofCamera, Vector3 forward_ofCamera = default(Vector3), Vector3 up_ofCamera = default(Vector3), Color color = default(Color), float nearClipPlaneDistance = 0.3f, float farClipPlaneDistance = 1000.0f, float fieldOfViewAngleDeg = 60.0f, float aspectRatioOfScreen = 16.0f / 9.0f, float alphaFactor_forBoundarySurfaceLines = 0.18f, float linesWidth_ofEdges = 0.0f, int linesPerBoundarySurface = 60, string text = null, bool forceTextOnNearPlaneUnmirroredTowardsCam = true, Vector3 positionOnHighlightedPlane = default(Vector3), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position_ofCamera, "position_ofCamera")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(forward_ofCamera, "forward_ofCamera")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up_ofCamera, "up_ofCamera")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(nearClipPlaneDistance, "nearClipPlaneDistance")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(farClipPlaneDistance, "farClipPlaneDistance")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(fieldOfViewAngleDeg, "fieldOfViewAngleDeg")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(aspectRatioOfScreen, "aspectRatioOfScreen")) { return; }

            forward_ofCamera = UtilitiesDXXL_Math.OverwriteDefaultVectors(forward_ofCamera, Vector3.forward);
            camPlane.Recreate(position_ofCamera, forward_ofCamera);
            up_ofCamera = UtilitiesDXXL_Math.OverwriteDefaultVectors(up_ofCamera, Vector3.up);
            up_ofCamera = UtilitiesDXXL_Shapes.ForceVectorPerpToOtherVector(up_ofCamera, camPlane);

            aspectRatioOfScreen = UtilitiesDXXL_Math.AbsNonZeroValue(aspectRatioOfScreen);
            if (aspectRatioOfScreen < 0.001f)
            {
                UtilitiesDXXL_DrawBasics.PointFallback(position_ofCamera, "[<color=#adadadFF><icon=logMessage></color> CameraFrustum with aspectRatioOfScreen near 0]<br>" + text, color, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            nearClipPlaneDistance = Mathf.Max(nearClipPlaneDistance, 0.01f);
            farClipPlaneDistance = Mathf.Max(farClipPlaneDistance, 0.011f);
            fieldOfViewAngleDeg = Mathf.Clamp(fieldOfViewAngleDeg, 0.1f, 179.9f);

            UtilitiesDXXL_EngineBasics.CameraFrustum(position_ofCamera, forward_ofCamera, up_ofCamera, false, 5.0f, fieldOfViewAngleDeg, nearClipPlaneDistance, farClipPlaneDistance, aspectRatioOfScreen, color, text, forceTextOnNearPlaneUnmirroredTowardsCam, linesWidth_ofEdges, alphaFactor_forBoundarySurfaceLines, linesPerBoundarySurface, positionOnHighlightedPlane, durationInSec, hiddenByNearerObjects);
        }

        public static void CameraFrustum(Camera camera, Color color = default(Color), float alphaFactor_forBoundarySurfaceLines = 0.18f, float linesWidth_ofEdges = 0.0f, int linesPerBoundarySurface = 60, string text = null, bool forceTextOnNearPlaneUnmirroredTowardsCam = true, Vector3 positionOnHighlightedPlane = default(Vector3), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(camera, "camera")) { return; }
            UtilitiesDXXL_EngineBasics.CameraFrustum(camera.transform.position, camera.transform.forward, camera.transform.up, camera.orthographic, camera.orthographicSize, camera.fieldOfView, camera.nearClipPlane, camera.farClipPlane, camera.aspect, color, text, forceTextOnNearPlaneUnmirroredTowardsCam, linesWidth_ofEdges, alphaFactor_forBoundarySurfaceLines, linesPerBoundarySurface, positionOnHighlightedPlane, durationInSec, hiddenByNearerObjects);
        }

        public static void BoolDisplayer2D(bool boolValueToDisplay, Vector2 position = default(Vector2), string boolName = null, float size = 1.0f, float custom_zPos = float.PositiveInfinity, Color color_forTextAndFrame = default(Color), Color overwriteColor_forTrue = default(Color), Color overwriteColor_forFalse = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 positionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(position, zPos);
            BoolDisplayer(boolValueToDisplay, positionV3, boolName, size, UnityEngine.Quaternion.identity, color_forTextAndFrame, overwriteColor_forTrue, overwriteColor_forFalse, durationInSec, hiddenByNearerObjects);
        }

        public static void BoolDisplayer(bool boolValueToDisplay, Vector3 position = default(Vector3), string boolName = null, float size = 1.0f, Quaternion rotation = default(Quaternion), Color color_forTextAndFrame = default(Color), Color overwriteColor_forTrue = default(Color), Color overwriteColor_forFalse = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            UtilitiesDXXL_EngineBasics.BoolDisplayer(boolValueToDisplay, position, boolName, size, rotation, color_forTextAndFrame, overwriteColor_forTrue, overwriteColor_forFalse, durationInSec, hiddenByNearerObjects);
        }

        public static void BoolDisplayerScreenspace(bool boolValueToDisplay, string boolName, Vector3 position_in3DWorldspace, float size_relToViewportHeight = 0.175f, Color color_forTextAndFrame = default(Color), Color overwriteColor_forTrue = default(Color), Color overwriteColor_forFalse = default(Color), float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_BoolDisplayerScreenspace_3Dpos.Add(new BoolDisplayerScreenspace_3Dpos(boolValueToDisplay, boolName, position_in3DWorldspace, size_relToViewportHeight, color_forTextAndFrame, overwriteColor_forTrue, overwriteColor_forFalse, durationInSec));
                return;
            }

            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawEngineBasics.BoolDisplayerScreenspace") == false) { return; }
            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(automaticallyFoundCamera, position_in3DWorldspace, false);
            BoolDisplayerScreenspace(boolValueToDisplay, boolName, position_in2DViewportSpace, size_relToViewportHeight, color_forTextAndFrame, overwriteColor_forTrue, overwriteColor_forFalse, durationInSec);
        }

        public static void BoolDisplayerScreenspace(Camera screenCamera, bool boolValueToDisplay, string boolName, Vector3 position_in3DWorldspace, float size_relToViewportHeight = 0.175f, Color color_forTextAndFrame = default(Color), Color overwriteColor_forTrue = default(Color), Color overwriteColor_forFalse = default(Color), float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_BoolDisplayerScreenspace_3Dpos_cam.Add(new BoolDisplayerScreenspace_3Dpos_cam(screenCamera, boolValueToDisplay, boolName, position_in3DWorldspace, size_relToViewportHeight, color_forTextAndFrame, overwriteColor_forTrue, overwriteColor_forFalse, durationInSec));
                return;
            }

            Vector2 position_in2DViewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(screenCamera, position_in3DWorldspace, false);
            BoolDisplayerScreenspace(screenCamera, boolValueToDisplay, boolName, position_in2DViewportSpace, size_relToViewportHeight, color_forTextAndFrame, overwriteColor_forTrue, overwriteColor_forFalse, durationInSec);
        }

        public static void BoolDisplayerScreenspace(bool boolValueToDisplay, string boolName = null, Vector2 position_in2DViewportSpace = default(Vector2), float size_relToViewportHeight = 0.175f, Color color_forTextAndFrame = default(Color), Color overwriteColor_forTrue = default(Color), Color overwriteColor_forFalse = default(Color), float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawEngineBasics.BoolDisplayerScreenspace") == false) { return; }
            BoolDisplayerScreenspace(automaticallyFoundCamera, boolValueToDisplay, boolName, position_in2DViewportSpace, size_relToViewportHeight, color_forTextAndFrame, overwriteColor_forTrue, overwriteColor_forFalse, durationInSec);
        }

        public static void BoolDisplayerScreenspace(Camera screenCamera, bool boolValueToDisplay, string boolName = null, Vector2 position_in2DViewportSpace = default(Vector2), float size_relToViewportHeight = 0.175f, Color color_forTextAndFrame = default(Color), Color overwriteColor_forTrue = default(Color), Color overwriteColor_forFalse = default(Color), float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_BoolDisplayerScreenspace_2Dpos_cam.Add(new BoolDisplayerScreenspace_2Dpos_cam(screenCamera, boolValueToDisplay, boolName, position_in2DViewportSpace, size_relToViewportHeight, color_forTextAndFrame, overwriteColor_forTrue, overwriteColor_forFalse, durationInSec));
                return;
            }

            UtilitiesDXXL_EngineBasics.BoolDisplayerScreenspace(screenCamera, boolValueToDisplay, boolName, position_in2DViewportSpace, size_relToViewportHeight, color_forTextAndFrame, overwriteColor_forTrue, overwriteColor_forFalse, durationInSec);
        }

        public static void RayLineExtended(Ray ray, Color color = default(Color), float width = 0.0f, string text = null, float forceFixedConeLength = 0.0f, bool addNormalizedMarkingText = true, float enlargeSmallTextToThisMinTextSize = 0.005f, float extentionLength = 1000.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            RayLineExtended(ray.origin, ray.direction, color, width, text, forceFixedConeLength, addNormalizedMarkingText, enlargeSmallTextToThisMinTextSize, extentionLength, durationInSec, hiddenByNearerObjects);
        }

        public static void RayLineExtended(Vector3 rayOrigin, Vector3 rayDirection, Color color = default(Color), float width = 0.0f, string text = null, float forceFixedConeLength = 0.0f, bool addNormalizedMarkingText = true, float enlargeSmallTextToThisMinTextSize = 0.005f, float extentionLength = 1000.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_EngineBasics.RayLineExtended(false, rayOrigin, rayDirection, color, width, text, forceFixedConeLength, addNormalizedMarkingText, enlargeSmallTextToThisMinTextSize, extentionLength, durationInSec, hiddenByNearerObjects);
        }

        public static void RayLineExtended2D(Ray2D ray, Color color = default(Color), float width = 0.0f, string text = null, float custom_zPos = float.PositiveInfinity, float forceFixedConeLength = 0.0f, bool addNormalizedMarkingText = true, float enlargeSmallTextToThisMinTextSize = 0.005f, float extentionLength = 1000.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            RayLineExtended2D(ray.origin, ray.direction, color, width, text, custom_zPos, forceFixedConeLength, addNormalizedMarkingText, enlargeSmallTextToThisMinTextSize, extentionLength, durationInSec, hiddenByNearerObjects);
        }

        public static void RayLineExtended2D(Vector2 rayOrigin, Vector2 rayDirection, Color color = default(Color), float width = 0.0f, string text = null, float custom_zPos = float.PositiveInfinity, float forceFixedConeLength = 0.0f, bool addNormalizedMarkingText = true, float enlargeSmallTextToThisMinTextSize = 0.005f, float extentionLength = 1000.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 rayOriginV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(rayOrigin, zPos);
            Vector3 rayDirectionV3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(rayDirection);
            UtilitiesDXXL_EngineBasics.RayLineExtended(true, rayOriginV3, rayDirectionV3, color, width, text, forceFixedConeLength, addNormalizedMarkingText, enlargeSmallTextToThisMinTextSize, extentionLength, durationInSec, hiddenByNearerObjects);
        }

        public static void RayLineExtendedScreenspace(Vector2 rayOrigin, Vector2 rayDirection, Color color = default(Color), float width_relToViewportHeight = 0.0f, string text = null, bool interpretDirectionAsUnwarped = false, float coneLength_relToViewportHeight = 0.05f, bool displayDistanceOutsideScreenBorder = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawEngineBasics.RayLineExtendedScreenspace") == false) { return; }
            RayLineExtendedScreenspace(automaticallyFoundCamera, rayOrigin, rayDirection, color, width_relToViewportHeight, text, interpretDirectionAsUnwarped, coneLength_relToViewportHeight, displayDistanceOutsideScreenBorder, durationInSec);
        }

        public static void RayLineExtendedScreenspace(Camera screenCamera, Vector2 rayOrigin, Vector2 rayDirection, Color color = default(Color), float width_relToViewportHeight = 0.0f, string text = null, bool interpretDirectionAsUnwarped = false, float coneLength_relToViewportHeight = 0.05f, bool displayDistanceOutsideScreenBorder = true, float durationInSec = 0.0f)
        {
            UtilitiesDXXL_EngineBasics.RayLineExtendedScreenspace(screenCamera, rayOrigin, rayDirection, color, width_relToViewportHeight, text, interpretDirectionAsUnwarped, coneLength_relToViewportHeight, displayDistanceOutsideScreenBorder, durationInSec);
        }

        public static void CoordinateAxesGizmo(Vector3 position, float lengthPerAxis = 1.0f, float linesWidth = 0.025f, string text = null, bool drawXYZchars = true, bool skipConeDrawing = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lengthPerAxis, "lengthPerAxis")) { return; } //-> would also be checked in "CoordinateAxesGizmoLocal", but has different name there
            CoordinateAxesGizmoLocal(position, UnityEngine.Quaternion.identity, default(Vector3), lengthPerAxis, linesWidth, text, drawXYZchars, skipConeDrawing, durationInSec, hiddenByNearerObjects);
        }

        public static void CoordinateAxesGizmoLocal(GameObject gameObject_whoseLocalSpaceIsDisplayed, float forceAllAxesLength = 0.0f, float lineWidth_inGlobalUnits = 0.025f, string text = null, bool drawXYZchars = true, bool skipConeDrawing = false, bool skipWarningForNonUniformParentScale = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(gameObject_whoseLocalSpaceIsDisplayed, "gameObject_whoseLocalSpaceIsDisplayed")) { return; }
            CoordinateAxesGizmoLocal(gameObject_whoseLocalSpaceIsDisplayed.transform, forceAllAxesLength, lineWidth_inGlobalUnits, text, drawXYZchars, skipConeDrawing, skipWarningForNonUniformParentScale, durationInSec, hiddenByNearerObjects);
        }

        public static void CoordinateAxesGizmoLocal(Transform transform_whoseLocalSpaceIsDisplayed, float forceAllAxesLength = 0.0f, float lineWidth_inGlobalUnits = 0.025f, string text = null, bool drawXYZchars = true, bool skipConeDrawing = false, bool skipWarningForNonUniformParentScale = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transform_whoseLocalSpaceIsDisplayed, "transform_whoseLocalSpaceIsDisplayed")) { return; }

            bool aParentHasANonUniformScale = UtilitiesDXXL_EngineBasics.CheckIf_transformOrAParentHasNonUniformScale(transform_whoseLocalSpaceIsDisplayed.parent);
            if (skipWarningForNonUniformParentScale) { aParentHasANonUniformScale = false; }
            UtilitiesDXXL_EngineBasics.CoordinateAxesGizmoLocal(transform_whoseLocalSpaceIsDisplayed.position, transform_whoseLocalSpaceIsDisplayed.rotation, transform_whoseLocalSpaceIsDisplayed.lossyScale, forceAllAxesLength, lineWidth_inGlobalUnits, text, drawXYZchars, skipConeDrawing, durationInSec, hiddenByNearerObjects, aParentHasANonUniformScale);
        }

        public static void CoordinateAxesGizmoLocal(Vector3 position_OfLocalCoordinateSystem, Quaternion rotation_OfLocalCoordinateSystem, Vector3 scale_OfLocalCoordinateSystem = default(Vector3), float forceAllAxesLength = 0.0f, float lineWidth_inGlobalUnits = 0.025f, string text = null, bool drawXYZchars = true, bool skipConeDrawing = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            UtilitiesDXXL_EngineBasics.CoordinateAxesGizmoLocal(position_OfLocalCoordinateSystem, rotation_OfLocalCoordinateSystem, scale_OfLocalCoordinateSystem, forceAllAxesLength, lineWidth_inGlobalUnits, text, drawXYZchars, skipConeDrawing, durationInSec, hiddenByNearerObjects, false);
        }

        public static void GridPlanes(Transform transformAroundWhichToDraw, float extentOfEachGridPlane_rel = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColorForX = default(Color), Color overwriteColorForY = default(Color), Color overwriteColorForZ = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transformAroundWhichToDraw, "transformAroundWhichToDraw")) { return; }
            GridPlanes(transformAroundWhichToDraw.position, extentOfEachGridPlane_rel, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColorForX, overwriteColorForY, overwriteColorForZ, durationInSec, hiddenByNearerObjects);
        }

        public static void GridPlanes(Vector3 positionAroundWhichToDraw, float extentOfEachGridPlane_rel = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColorForX = default(Color), Color overwriteColorForY = default(Color), Color overwriteColorForZ = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Grid.GridPlanes(true, true, true, positionAroundWhichToDraw, extentOfEachGridPlane_rel, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColorForX, overwriteColorForY, overwriteColorForZ, durationInSec, hiddenByNearerObjects);
        }

        public static void XGridPlanes(Transform transformAroundWhichToDraw, float extentOfEachGridPlane_rel = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transformAroundWhichToDraw, "transformAroundWhichToDraw")) { return; }
            XGridPlanes(transformAroundWhichToDraw.position, extentOfEachGridPlane_rel, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void XGridPlanes(Vector3 positionAroundWhichToDraw, float extentOfEachGridPlane_rel = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Grid.GridPlanes(true, false, false, positionAroundWhichToDraw, extentOfEachGridPlane_rel, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void YGridPlanes(Transform transformAroundWhichToDraw, float extentOfEachGridPlane_rel = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transformAroundWhichToDraw, "transformAroundWhichToDraw")) { return; }
            YGridPlanes(transformAroundWhichToDraw.position, extentOfEachGridPlane_rel, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void YGridPlanes(Vector3 positionAroundWhichToDraw, float extentOfEachGridPlane_rel = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Grid.GridPlanes(false, true, false, positionAroundWhichToDraw, extentOfEachGridPlane_rel, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void ZGridPlanes(Transform transformAroundWhichToDraw, float extentOfEachGridPlane_rel = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transformAroundWhichToDraw, "transformAroundWhichToDraw")) { return; }
            ZGridPlanes(transformAroundWhichToDraw.position, extentOfEachGridPlane_rel, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void ZGridPlanes(Vector3 positionAroundWhichToDraw, float extentOfEachGridPlane_rel = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Grid.GridPlanes(false, false, true, positionAroundWhichToDraw, extentOfEachGridPlane_rel, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void GridPlanesLocal(Transform childTransformAroundWhichToDrawGridOfParent, float extentOfEachGridPlane_rel_inLocalSpaceUnits = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColorForX = default(Color), Color overwriteColorForY = default(Color), Color overwriteColorForZ = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(childTransformAroundWhichToDrawGridOfParent, "childTransformAroundWhichToDrawGridOfParent")) { return; }
            if (childTransformAroundWhichToDrawGridOfParent.parent == null)
            {
                GridPlanesLocal(Vector3.zero, Vector3.one, Quaternion.identity, childTransformAroundWhichToDrawGridOfParent.localPosition, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColorForX, overwriteColorForY, overwriteColorForZ, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                GridPlanesLocal(childTransformAroundWhichToDrawGridOfParent.parent.position, childTransformAroundWhichToDrawGridOfParent.parent.lossyScale, childTransformAroundWhichToDrawGridOfParent.parent.rotation, childTransformAroundWhichToDrawGridOfParent.localPosition, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColorForX, overwriteColorForY, overwriteColorForZ, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void GridPlanesLocal(Transform parentTransformThatDefinesTheLocalSpace, Vector3 localPositionAroundWhichToDraw, float extentOfEachGridPlane_rel_inLocalSpaceUnits = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColorForX = default(Color), Color overwriteColorForY = default(Color), Color overwriteColorForZ = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (parentTransformThatDefinesTheLocalSpace == null)
            {
                UtilitiesDXXL_Grid.GridPlanesLocal(Vector3.zero, Vector3.one, Quaternion.identity, true, true, true, localPositionAroundWhichToDraw, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColorForX, overwriteColorForY, overwriteColorForZ, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                UtilitiesDXXL_Grid.GridPlanesLocal(parentTransformThatDefinesTheLocalSpace.position, parentTransformThatDefinesTheLocalSpace.lossyScale, parentTransformThatDefinesTheLocalSpace.rotation, true, true, true, localPositionAroundWhichToDraw, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColorForX, overwriteColorForY, overwriteColorForZ, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void XGridPlanesLocal(Transform childTransformAroundWhichToDrawGridOfParent, float extentOfEachGridPlane_rel_inLocalSpaceUnits = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(childTransformAroundWhichToDrawGridOfParent, "childTransformAroundWhichToDrawGridOfParent")) { return; }
            if (childTransformAroundWhichToDrawGridOfParent.parent == null)
            {
                XGridPlanesLocal(Vector3.zero, Vector3.one, Quaternion.identity, childTransformAroundWhichToDrawGridOfParent.localPosition, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                XGridPlanesLocal(childTransformAroundWhichToDrawGridOfParent.parent.position, childTransformAroundWhichToDrawGridOfParent.parent.lossyScale, childTransformAroundWhichToDrawGridOfParent.parent.rotation, childTransformAroundWhichToDrawGridOfParent.localPosition, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void XGridPlanesLocal(Transform parentTransformThatDefinesTheLocalSpace, Vector3 localPositionAroundWhichToDraw, float extentOfEachGridPlane_rel_inLocalSpaceUnits = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (parentTransformThatDefinesTheLocalSpace == null)
            {
                UtilitiesDXXL_Grid.GridPlanesLocal(Vector3.zero, Vector3.one, Quaternion.identity, true, false, false, localPositionAroundWhichToDraw, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                UtilitiesDXXL_Grid.GridPlanesLocal(parentTransformThatDefinesTheLocalSpace.position, parentTransformThatDefinesTheLocalSpace.lossyScale, parentTransformThatDefinesTheLocalSpace.rotation, true, false, false, localPositionAroundWhichToDraw, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void YGridPlanesLocal(Transform childTransformAroundWhichToDrawGridOfParent, float extentOfEachGridPlane_rel_inLocalSpaceUnits = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(childTransformAroundWhichToDrawGridOfParent, "childTransformAroundWhichToDrawGridOfParent")) { return; }
            if (childTransformAroundWhichToDrawGridOfParent.parent == null)
            {
                YGridPlanesLocal(Vector3.zero, Vector3.one, Quaternion.identity, childTransformAroundWhichToDrawGridOfParent.localPosition, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                YGridPlanesLocal(childTransformAroundWhichToDrawGridOfParent.parent.position, childTransformAroundWhichToDrawGridOfParent.parent.lossyScale, childTransformAroundWhichToDrawGridOfParent.parent.rotation, childTransformAroundWhichToDrawGridOfParent.localPosition, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void YGridPlanesLocal(Transform parentTransformThatDefinesTheLocalSpace, Vector3 localPositionAroundWhichToDraw, float extentOfEachGridPlane_rel_inLocalSpaceUnits = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (parentTransformThatDefinesTheLocalSpace == null)
            {
                UtilitiesDXXL_Grid.GridPlanesLocal(Vector3.zero, Vector3.one, Quaternion.identity, false, true, false, localPositionAroundWhichToDraw, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                UtilitiesDXXL_Grid.GridPlanesLocal(parentTransformThatDefinesTheLocalSpace.position, parentTransformThatDefinesTheLocalSpace.lossyScale, parentTransformThatDefinesTheLocalSpace.rotation, false, true, false, localPositionAroundWhichToDraw, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void ZGridPlanesLocal(Transform childTransformAroundWhichToDrawGridOfParent, float extentOfEachGridPlane_rel_inLocalSpaceUnits = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(childTransformAroundWhichToDrawGridOfParent, "childTransformAroundWhichToDrawGridOfParent")) { return; }
            if (childTransformAroundWhichToDrawGridOfParent.parent == null)
            {
                ZGridPlanesLocal(Vector3.zero, Vector3.one, Quaternion.identity, childTransformAroundWhichToDrawGridOfParent.localPosition, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                ZGridPlanesLocal(childTransformAroundWhichToDrawGridOfParent.parent.position, childTransformAroundWhichToDrawGridOfParent.parent.lossyScale, childTransformAroundWhichToDrawGridOfParent.parent.rotation, childTransformAroundWhichToDrawGridOfParent.localPosition, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void ZGridPlanesLocal(Transform parentTransformThatDefinesTheLocalSpace, Vector3 localPositionAroundWhichToDraw, float extentOfEachGridPlane_rel_inLocalSpaceUnits = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (parentTransformThatDefinesTheLocalSpace == null)
            {
                UtilitiesDXXL_Grid.GridPlanesLocal(Vector3.zero, Vector3.one, Quaternion.identity, false, false, true, localPositionAroundWhichToDraw, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                UtilitiesDXXL_Grid.GridPlanesLocal(parentTransformThatDefinesTheLocalSpace.position, parentTransformThatDefinesTheLocalSpace.lossyScale, parentTransformThatDefinesTheLocalSpace.rotation, false, false, true, localPositionAroundWhichToDraw, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void GridPlanesLocal(Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Transform childTransformAroundWhichToDraw, float extentOfEachGridPlane_rel_inLocalSpaceUnits = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColorForX = default(Color), Color overwriteColorForY = default(Color), Color overwriteColorForZ = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(childTransformAroundWhichToDraw, "childTransformAroundWhichToDraw")) { return; }
            GridPlanesLocal(originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, childTransformAroundWhichToDraw.localPosition, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColorForX, overwriteColorForY, overwriteColorForZ, durationInSec, hiddenByNearerObjects);
        }

        public static void GridPlanesLocal(Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Vector3 localPositionAroundWhichToDraw = default(Vector3), float extentOfEachGridPlane_rel_inLocalSpaceUnits = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColorForX = default(Color), Color overwriteColorForY = default(Color), Color overwriteColorForZ = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Grid.GridPlanesLocal(originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, true, true, true, localPositionAroundWhichToDraw, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColorForX, overwriteColorForY, overwriteColorForZ, durationInSec, hiddenByNearerObjects);
        }

        public static void XGridPlanesLocal(Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Transform childTransformAroundWhichToDraw, float extentOfEachGridPlane_rel_inLocalSpaceUnits = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(childTransformAroundWhichToDraw, "childTransformAroundWhichToDraw")) { return; }
            XGridPlanesLocal(originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, childTransformAroundWhichToDraw.localPosition, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void XGridPlanesLocal(Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Vector3 localPositionAroundWhichToDraw = default(Vector3), float extentOfEachGridPlane_rel_inLocalSpaceUnits = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Grid.GridPlanesLocal(originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, true, false, false, localPositionAroundWhichToDraw, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void YGridPlanesLocal(Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Transform childTransformAroundWhichToDraw, float extentOfEachGridPlane_rel_inLocalSpaceUnits = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(childTransformAroundWhichToDraw, "childTransformAroundWhichToDraw")) { return; }
            YGridPlanesLocal(originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, childTransformAroundWhichToDraw.localPosition, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void YGridPlanesLocal(Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Vector3 localPositionAroundWhichToDraw = default(Vector3), float extentOfEachGridPlane_rel_inLocalSpaceUnits = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Grid.GridPlanesLocal(originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, false, true, false, localPositionAroundWhichToDraw, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects);
        }
        public static void ZGridPlanesLocal(Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Transform childTransformAroundWhichToDraw, float extentOfEachGridPlane_rel_inLocalSpaceUnits = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(childTransformAroundWhichToDraw, "childTransformAroundWhichToDraw")) { return; }
            ZGridPlanesLocal(originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, childTransformAroundWhichToDraw.localPosition, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void ZGridPlanesLocal(Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Vector3 localPositionAroundWhichToDraw = default(Vector3), float extentOfEachGridPlane_rel_inLocalSpaceUnits = 10.0f, float drawDensity = 1.0f, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = false, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Grid.GridPlanesLocal(originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, false, false, true, localPositionAroundWhichToDraw, extentOfEachGridPlane_rel_inLocalSpaceUnits, drawDensity, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public enum XGridLinesOrientation { alongY, alongZ };
        public enum YGridLinesOrientation { alongX, alongZ };
        public enum ZGridLinesOrientation { alongX, alongY };
        public static void GridLines(Transform transformAroundWhichToDraw, float coveredGridUnits_rel = 10.0f, float lengthOfEachGridLine_rel = 10.0f, float linesWidth_signFlipsPerp = 0.0f, XGridLinesOrientation orientation_ofXLines = XGridLinesOrientation.alongY, YGridLinesOrientation orientation_ofYLines = YGridLinesOrientation.alongX, ZGridLinesOrientation orientation_ofZLines = ZGridLinesOrientation.alongY, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColorForX = default(Color), Color overwriteColorForY = default(Color), Color overwriteColorForZ = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transformAroundWhichToDraw, "transformAroundWhichToDraw")) { return; }
            GridLines(transformAroundWhichToDraw.position, coveredGridUnits_rel, lengthOfEachGridLine_rel, linesWidth_signFlipsPerp, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColorForX, overwriteColorForY, overwriteColorForZ, durationInSec, hiddenByNearerObjects);
        }

        public static void GridLines(Vector3 positionAroundWhichToDraw, float coveredGridUnits_rel = 10.0f, float lengthOfEachGridLine_rel = 10.0f, float linesWidth_signFlipsPerp = 0.0f, XGridLinesOrientation orientation_ofXLines = XGridLinesOrientation.alongY, YGridLinesOrientation orientation_ofYLines = YGridLinesOrientation.alongX, ZGridLinesOrientation orientation_ofZLines = ZGridLinesOrientation.alongY, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColorForX = default(Color), Color overwriteColorForY = default(Color), Color overwriteColorForZ = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Grid.GridLines(positionAroundWhichToDraw, coveredGridUnits_rel, lengthOfEachGridLine_rel, linesWidth_signFlipsPerp, true, true, true, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColorForX, overwriteColorForY, overwriteColorForZ, durationInSec, hiddenByNearerObjects, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);
        }

        public static void XGridLines(Transform transformAroundWhichToDraw, float coveredGridUnits_rel = 10.0f, float lengthOfEachGridLine_rel = 10.0f, float linesWidth_signFlipsPerp = 0.0f, XGridLinesOrientation orientation = XGridLinesOrientation.alongY, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transformAroundWhichToDraw, "transformAroundWhichToDraw")) { return; }
            XGridLines(transformAroundWhichToDraw.position, coveredGridUnits_rel, lengthOfEachGridLine_rel, linesWidth_signFlipsPerp, orientation, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void XGridLines(Vector3 positionAroundWhichToDraw, float coveredGridUnits_rel = 10.0f, float lengthOfEachGridLine_rel = 10.0f, float linesWidth_signFlipsPerp = 0.0f, XGridLinesOrientation orientation = XGridLinesOrientation.alongY, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Grid.GridLines(positionAroundWhichToDraw, coveredGridUnits_rel, lengthOfEachGridLine_rel, linesWidth_signFlipsPerp, true, false, false, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects, orientation, YGridLinesOrientation.alongX, ZGridLinesOrientation.alongX);
        }

        public static void YGridLines(Transform transformAroundWhichToDraw, float coveredGridUnits_rel = 10.0f, float lengthOfEachGridLine_rel = 10.0f, float linesWidth_signFlipsPerp = 0.0f, YGridLinesOrientation orientation = YGridLinesOrientation.alongX, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transformAroundWhichToDraw, "transformAroundWhichToDraw")) { return; }
            YGridLines(transformAroundWhichToDraw.position, coveredGridUnits_rel, lengthOfEachGridLine_rel, linesWidth_signFlipsPerp, orientation, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void YGridLines(Vector3 positionAroundWhichToDraw, float coveredGridUnits_rel = 10.0f, float lengthOfEachGridLine_rel = 10.0f, float linesWidth_signFlipsPerp = 0.0f, YGridLinesOrientation orientation = YGridLinesOrientation.alongX, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Grid.GridLines(positionAroundWhichToDraw, coveredGridUnits_rel, lengthOfEachGridLine_rel, linesWidth_signFlipsPerp, false, true, false, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects, XGridLinesOrientation.alongZ, orientation, ZGridLinesOrientation.alongX);
        }
        public static void ZGridLines(Transform transformAroundWhichToDraw, float coveredGridUnits_rel = 10.0f, float lengthOfEachGridLine_rel = 10.0f, float linesWidth_signFlipsPerp = 0.0f, ZGridLinesOrientation orientation = ZGridLinesOrientation.alongY, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transformAroundWhichToDraw, "transformAroundWhichToDraw")) { return; }
            ZGridLines(transformAroundWhichToDraw.position, coveredGridUnits_rel, lengthOfEachGridLine_rel, linesWidth_signFlipsPerp, orientation, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void ZGridLines(Vector3 positionAroundWhichToDraw, float coveredGridUnits_rel = 10.0f, float lengthOfEachGridLine_rel = 10.0f, float linesWidth_signFlipsPerp = 0.0f, ZGridLinesOrientation orientation = ZGridLinesOrientation.alongY, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Grid.GridLines(positionAroundWhichToDraw, coveredGridUnits_rel, lengthOfEachGridLine_rel, linesWidth_signFlipsPerp, false, false, true, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects, XGridLinesOrientation.alongZ, YGridLinesOrientation.alongZ, orientation);
        }

        public static void GridLinesLocal(Transform childTransformAroundWhichToDrawGridOfParent, float coveredGridUnits_rel_inLocalSpaceUnits = 10.0f, float lengthOfEachGridLine_rel_inLocalSpaceUnits = 10.0f, float linesWidth_inLocalSpaceUnits_signFlipsPerp = 0.0f, XGridLinesOrientation orientation_ofXLines = XGridLinesOrientation.alongY, YGridLinesOrientation orientation_ofYLines = YGridLinesOrientation.alongX, ZGridLinesOrientation orientation_ofZLines = ZGridLinesOrientation.alongY, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColorForX = default(Color), Color overwriteColorForY = default(Color), Color overwriteColorForZ = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(childTransformAroundWhichToDrawGridOfParent, "childTransformAroundWhichToDrawGridOfParent")) { return; }
            if (childTransformAroundWhichToDrawGridOfParent.parent == null)
            {
                GridLinesLocal(Vector3.zero, Vector3.one, Quaternion.identity, childTransformAroundWhichToDrawGridOfParent.localPosition, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColorForX, overwriteColorForY, overwriteColorForZ, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                GridLinesLocal(childTransformAroundWhichToDrawGridOfParent.parent.position, childTransformAroundWhichToDrawGridOfParent.parent.lossyScale, childTransformAroundWhichToDrawGridOfParent.parent.rotation, childTransformAroundWhichToDrawGridOfParent.localPosition, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColorForX, overwriteColorForY, overwriteColorForZ, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void GridLinesLocal(Transform parentTransformThatDefinesTheLocalSpace, Vector3 localPositionAroundWhichToDraw, float coveredGridUnits_rel_inLocalSpaceUnits = 10.0f, float lengthOfEachGridLine_rel_inLocalSpaceUnits = 10.0f, float linesWidth_inLocalSpaceUnits_signFlipsPerp = 0.0f, XGridLinesOrientation orientation_ofXLines = XGridLinesOrientation.alongY, YGridLinesOrientation orientation_ofYLines = YGridLinesOrientation.alongX, ZGridLinesOrientation orientation_ofZLines = ZGridLinesOrientation.alongY, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColorForX = default(Color), Color overwriteColorForY = default(Color), Color overwriteColorForZ = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (parentTransformThatDefinesTheLocalSpace == null)
            {
                UtilitiesDXXL_Grid.GridLinesLocal(Vector3.zero, Vector3.one, Quaternion.identity, localPositionAroundWhichToDraw, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, true, true, true, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColorForX, overwriteColorForY, overwriteColorForZ, durationInSec, hiddenByNearerObjects, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);
            }
            else
            {
                UtilitiesDXXL_Grid.GridLinesLocal(parentTransformThatDefinesTheLocalSpace.position, parentTransformThatDefinesTheLocalSpace.lossyScale, parentTransformThatDefinesTheLocalSpace.rotation, localPositionAroundWhichToDraw, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, true, true, true, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColorForX, overwriteColorForY, overwriteColorForZ, durationInSec, hiddenByNearerObjects, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);
            }
        }

        public static void XGridLinesLocal(Transform childTransformAroundWhichToDrawGridOfParent, float coveredGridUnits_rel_inLocalSpaceUnits = 10.0f, float lengthOfEachGridLine_rel_inLocalSpaceUnits = 10.0f, float linesWidth_inLocalSpaceUnits_signFlipsPerp = 0.0f, XGridLinesOrientation orientation = XGridLinesOrientation.alongY, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(childTransformAroundWhichToDrawGridOfParent, "childTransformAroundWhichToDrawGridOfParent")) { return; }
            if (childTransformAroundWhichToDrawGridOfParent.parent == null)
            {
                XGridLinesLocal(Vector3.zero, Vector3.one, Quaternion.identity, childTransformAroundWhichToDrawGridOfParent.localPosition, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, orientation, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                XGridLinesLocal(childTransformAroundWhichToDrawGridOfParent.parent.position, childTransformAroundWhichToDrawGridOfParent.parent.lossyScale, childTransformAroundWhichToDrawGridOfParent.parent.rotation, childTransformAroundWhichToDrawGridOfParent.localPosition, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, orientation, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void XGridLinesLocal(Transform parentTransformThatDefinesTheLocalSpace, Vector3 localPositionAroundWhichToDraw, float coveredGridUnits_rel_inLocalSpaceUnits = 10.0f, float lengthOfEachGridLine_rel_inLocalSpaceUnits = 10.0f, float linesWidth_inLocalSpaceUnits_signFlipsPerp = 0.0f, XGridLinesOrientation orientation = XGridLinesOrientation.alongY, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (parentTransformThatDefinesTheLocalSpace == null)
            {
                UtilitiesDXXL_Grid.GridLinesLocal(Vector3.zero, Vector3.one, Quaternion.identity, localPositionAroundWhichToDraw, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, true, false, false, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects, orientation, YGridLinesOrientation.alongX, ZGridLinesOrientation.alongX);
            }
            else
            {
                UtilitiesDXXL_Grid.GridLinesLocal(parentTransformThatDefinesTheLocalSpace.position, parentTransformThatDefinesTheLocalSpace.lossyScale, parentTransformThatDefinesTheLocalSpace.rotation, localPositionAroundWhichToDraw, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, true, false, false, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects, orientation, YGridLinesOrientation.alongX, ZGridLinesOrientation.alongX);
            }
        }

        public static void YGridLinesLocal(Transform childTransformAroundWhichToDrawGridOfParent, float coveredGridUnits_rel_inLocalSpaceUnits = 10.0f, float lengthOfEachGridLine_rel_inLocalSpaceUnits = 10.0f, float linesWidth_inLocalSpaceUnits_signFlipsPerp = 0.0f, YGridLinesOrientation orientation = YGridLinesOrientation.alongX, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(childTransformAroundWhichToDrawGridOfParent, "childTransformAroundWhichToDrawGridOfParent")) { return; }
            if (childTransformAroundWhichToDrawGridOfParent.parent == null)
            {
                YGridLinesLocal(Vector3.zero, Vector3.one, Quaternion.identity, childTransformAroundWhichToDrawGridOfParent.localPosition, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, orientation, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                YGridLinesLocal(childTransformAroundWhichToDrawGridOfParent.parent.position, childTransformAroundWhichToDrawGridOfParent.parent.lossyScale, childTransformAroundWhichToDrawGridOfParent.parent.rotation, childTransformAroundWhichToDrawGridOfParent.localPosition, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, orientation, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void YGridLinesLocal(Transform parentTransformThatDefinesTheLocalSpace, Vector3 localPositionAroundWhichToDraw, float coveredGridUnits_rel_inLocalSpaceUnits = 10.0f, float lengthOfEachGridLine_rel_inLocalSpaceUnits = 10.0f, float linesWidth_inLocalSpaceUnits_signFlipsPerp = 0.0f, YGridLinesOrientation orientation = YGridLinesOrientation.alongX, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (parentTransformThatDefinesTheLocalSpace == null)
            {
                UtilitiesDXXL_Grid.GridLinesLocal(Vector3.zero, Vector3.one, Quaternion.identity, localPositionAroundWhichToDraw, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, false, true, false, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects, XGridLinesOrientation.alongZ, orientation, ZGridLinesOrientation.alongX);
            }
            else
            {
                UtilitiesDXXL_Grid.GridLinesLocal(parentTransformThatDefinesTheLocalSpace.position, parentTransformThatDefinesTheLocalSpace.lossyScale, parentTransformThatDefinesTheLocalSpace.rotation, localPositionAroundWhichToDraw, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, false, true, false, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects, XGridLinesOrientation.alongZ, orientation, ZGridLinesOrientation.alongX);
            }
        }

        public static void ZGridLinesLocal(Transform childTransformAroundWhichToDrawGridOfParent, float coveredGridUnits_rel_inLocalSpaceUnits = 10.0f, float lengthOfEachGridLine_rel_inLocalSpaceUnits = 10.0f, float linesWidth_inLocalSpaceUnits_signFlipsPerp = 0.0f, ZGridLinesOrientation orientation = ZGridLinesOrientation.alongY, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(childTransformAroundWhichToDrawGridOfParent, "childTransformAroundWhichToDrawGridOfParent")) { return; }
            if (childTransformAroundWhichToDrawGridOfParent.parent == null)
            {
                ZGridLinesLocal(Vector3.zero, Vector3.one, Quaternion.identity, childTransformAroundWhichToDrawGridOfParent.localPosition, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, orientation, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                ZGridLinesLocal(childTransformAroundWhichToDrawGridOfParent.parent.position, childTransformAroundWhichToDrawGridOfParent.parent.lossyScale, childTransformAroundWhichToDrawGridOfParent.parent.rotation, childTransformAroundWhichToDrawGridOfParent.localPosition, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, orientation, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void ZGridLinesLocal(Transform parentTransformThatDefinesTheLocalSpace, Vector3 localPositionAroundWhichToDraw, float coveredGridUnits_rel_inLocalSpaceUnits = 10.0f, float lengthOfEachGridLine_rel_inLocalSpaceUnits = 10.0f, float linesWidth_inLocalSpaceUnits_signFlipsPerp = 0.0f, ZGridLinesOrientation orientation = ZGridLinesOrientation.alongY, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (parentTransformThatDefinesTheLocalSpace == null)
            {
                UtilitiesDXXL_Grid.GridLinesLocal(Vector3.zero, Vector3.one, Quaternion.identity, localPositionAroundWhichToDraw, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, false, false, true, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects, XGridLinesOrientation.alongZ, YGridLinesOrientation.alongZ, orientation);
            }
            else
            {
                UtilitiesDXXL_Grid.GridLinesLocal(parentTransformThatDefinesTheLocalSpace.position, parentTransformThatDefinesTheLocalSpace.lossyScale, parentTransformThatDefinesTheLocalSpace.rotation, localPositionAroundWhichToDraw, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, false, false, true, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects, XGridLinesOrientation.alongZ, YGridLinesOrientation.alongZ, orientation);
            }
        }

        public static void GridLinesLocal(Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Transform childTransformAroundWhichToDraw, float coveredGridUnits_rel_inLocalSpaceUnits = 10.0f, float lengthOfEachGridLine_rel_inLocalSpaceUnits = 10.0f, float linesWidth_inLocalSpaceUnits_signFlipsPerp = 0.0f, XGridLinesOrientation orientation_ofXLines = XGridLinesOrientation.alongY, YGridLinesOrientation orientation_ofYLines = YGridLinesOrientation.alongX, ZGridLinesOrientation orientation_ofZLines = ZGridLinesOrientation.alongY, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColorForX = default(Color), Color overwriteColorForY = default(Color), Color overwriteColorForZ = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(childTransformAroundWhichToDraw, "childTransformAroundWhichToDraw")) { return; }
            GridLinesLocal(originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, childTransformAroundWhichToDraw.localPosition, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColorForX, overwriteColorForY, overwriteColorForZ, durationInSec, hiddenByNearerObjects);
        }

        public static void GridLinesLocal(Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Vector3 localPositionAroundWhichToDraw = default(Vector3), float coveredGridUnits_rel_inLocalSpaceUnits = 10.0f, float lengthOfEachGridLine_rel_inLocalSpaceUnits = 10.0f, float linesWidth_inLocalSpaceUnits_signFlipsPerp = 0.0f, XGridLinesOrientation orientation_ofXLines = XGridLinesOrientation.alongY, YGridLinesOrientation orientation_ofYLines = YGridLinesOrientation.alongX, ZGridLinesOrientation orientation_ofZLines = ZGridLinesOrientation.alongY, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColorForX = default(Color), Color overwriteColorForY = default(Color), Color overwriteColorForZ = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Grid.GridLinesLocal(originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, localPositionAroundWhichToDraw, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, true, true, true, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColorForX, overwriteColorForY, overwriteColorForZ, durationInSec, hiddenByNearerObjects, orientation_ofXLines, orientation_ofYLines, orientation_ofZLines);
        }

        public static void XGridLinesLocal(Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Transform childTransformAroundWhichToDraw, float coveredGridUnits_rel_inLocalSpaceUnits = 10.0f, float lengthOfEachGridLine_rel_inLocalSpaceUnits = 10.0f, float linesWidth_inLocalSpaceUnits_signFlipsPerp = 0.0f, XGridLinesOrientation orientation = XGridLinesOrientation.alongY, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(childTransformAroundWhichToDraw, "childTransformAroundWhichToDraw")) { return; }
            XGridLinesLocal(originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, childTransformAroundWhichToDraw.localPosition, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, orientation, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
        }


        public static void XGridLinesLocal(Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Vector3 localPositionAroundWhichToDraw = default(Vector3), float coveredGridUnits_rel_inLocalSpaceUnits = 10.0f, float lengthOfEachGridLine_rel_inLocalSpaceUnits = 10.0f, float linesWidth_inLocalSpaceUnits_signFlipsPerp = 0.0f, XGridLinesOrientation orientation = XGridLinesOrientation.alongY, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Grid.GridLinesLocal(originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, localPositionAroundWhichToDraw, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, true, false, false, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects, orientation, YGridLinesOrientation.alongX, ZGridLinesOrientation.alongX);
        }

        public static void YGridLinesLocal(Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Transform childTransformAroundWhichToDraw, float coveredGridUnits_rel_inLocalSpaceUnits = 10.0f, float lengthOfEachGridLine_rel_inLocalSpaceUnits = 10.0f, float linesWidth_inLocalSpaceUnits_signFlipsPerp = 0.0f, YGridLinesOrientation orientation = YGridLinesOrientation.alongX, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(childTransformAroundWhichToDraw, "childTransformAroundWhichToDraw")) { return; }
            YGridLinesLocal(originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, childTransformAroundWhichToDraw.localPosition, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, orientation, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void YGridLinesLocal(Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Vector3 localPositionAroundWhichToDraw = default(Vector3), float coveredGridUnits_rel_inLocalSpaceUnits = 10.0f, float lengthOfEachGridLine_rel_inLocalSpaceUnits = 10.0f, float linesWidth_inLocalSpaceUnits_signFlipsPerp = 0.0f, YGridLinesOrientation orientation = YGridLinesOrientation.alongX, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Grid.GridLinesLocal(originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, localPositionAroundWhichToDraw, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, false, true, false, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects, XGridLinesOrientation.alongZ, orientation, ZGridLinesOrientation.alongX);
        }

        public static void ZGridLinesLocal(Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Transform childTransformAroundWhichToDraw, float coveredGridUnits_rel_inLocalSpaceUnits = 10.0f, float lengthOfEachGridLine_rel_inLocalSpaceUnits = 10.0f, float linesWidth_inLocalSpaceUnits_signFlipsPerp = 0.0f, ZGridLinesOrientation orientation = ZGridLinesOrientation.alongY, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(childTransformAroundWhichToDraw, "childTransformAroundWhichToDraw")) { return; }
            ZGridLinesLocal(originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, childTransformAroundWhichToDraw.localPosition, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, orientation, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, durationInSec, hiddenByNearerObjects);
        }

        public static void ZGridLinesLocal(Vector3 originOfLocalSpace, Vector3 scaleOfLocalSpace, Quaternion rotationOfLocalSpace, Vector3 localPositionAroundWhichToDraw = default(Vector3), float coveredGridUnits_rel_inLocalSpaceUnits = 10.0f, float lengthOfEachGridLine_rel_inLocalSpaceUnits = 10.0f, float linesWidth_inLocalSpaceUnits_signFlipsPerp = 0.0f, ZGridLinesOrientation orientation = ZGridLinesOrientation.alongY, bool draw1000grid = false, bool draw100grid = false, bool draw10grid = false, bool draw1grid = true, bool draw0p1grid = true, bool draw0p01grid = false, bool draw0p001grid = false, float distanceBetweenRepeatingCoordsTexts_relToGridDistance = 20.0f, Color overwriteColor = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Grid.GridLinesLocal(originOfLocalSpace, scaleOfLocalSpace, rotationOfLocalSpace, localPositionAroundWhichToDraw, coveredGridUnits_rel_inLocalSpaceUnits, lengthOfEachGridLine_rel_inLocalSpaceUnits, linesWidth_inLocalSpaceUnits_signFlipsPerp, false, false, true, draw1000grid, draw100grid, draw10grid, draw1grid, draw0p1grid, draw0p01grid, draw0p001grid, distanceBetweenRepeatingCoordsTexts_relToGridDistance, overwriteColor, overwriteColor, overwriteColor, durationInSec, hiddenByNearerObjects, XGridLinesOrientation.alongZ, YGridLinesOrientation.alongZ, orientation);
        }

        public enum GridScreenspaceMode
        {
            warpWidthAndHeightIndividuallyToFitScreenInBothAxes,
            screenHeightDefinesSquareBoxes_alignLeft,
            screenHeightDefinesSquareBoxes_alignRight,
            screenWidthDefinesSquareBoxes_alignAtBottom,
            screenWidthDefinesSquareBoxes_alignAtTop,
        }

        public static void GridScreenspace(Color color = default(Color), float linesWidth_relToViewportHeight = 0.0f, bool drawTenthLines = true, bool drawHundredthLines = true, GridScreenspaceMode gridScreenspaceMode = GridScreenspaceMode.warpWidthAndHeightIndividuallyToFitScreenInBothAxes, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera automaticallyFoundCamera, "DrawEngineBasics.GridScreenspace") == false) { return; }
            GridScreenspace(automaticallyFoundCamera, color, linesWidth_relToViewportHeight, drawTenthLines, drawHundredthLines, gridScreenspaceMode, durationInSec);
        }

        public static void GridScreenspace(Camera camera, Color color = default(Color), float linesWidth_relToViewportHeight = 0.0f, bool drawTenthLines = true, bool drawHundredthLines = true, GridScreenspaceMode gridScreenspaceMode = GridScreenspaceMode.warpWidthAndHeightIndividuallyToFitScreenInBothAxes, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(camera, "camera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(camera)) { return; }

            if (DrawXXL_LinesManager.instance.noteAllScreenspaceDrawsToSheduler_insteadOfImmediatelyDrawingThem)
            {
                DrawXXL_LinesManager.instance.listOfSheduled_GridScreenspace.Add(new GridScreenspace(camera, color, linesWidth_relToViewportHeight, drawTenthLines, drawHundredthLines, gridScreenspaceMode, durationInSec));
                return;
            }

            color = UtilitiesDXXL_Colors.OverwriteDefaultColor(color);
            Color colorFor01 = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.5f);
            Color colorFor001 = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.15f);
            float usedAspectCorrectionFactor_toMakeBoxesSquare;

            switch (gridScreenspaceMode)
            {
                case GridScreenspaceMode.warpWidthAndHeightIndividuallyToFitScreenInBothAxes:
                    DrawMainScreenspaceGridLines(camera, 1.0f, 1.0f, false, false, color, linesWidth_relToViewportHeight, durationInSec);
                    DrawConfigurableScreenspaceGrid(camera, drawTenthLines, 10, 1.0f, 1.0f, false, false, colorFor01, durationInSec);
                    DrawConfigurableScreenspaceGrid(camera, drawHundredthLines, 100, 1.0f, 1.0f, false, false, colorFor001, durationInSec);
                    break;
                case GridScreenspaceMode.screenHeightDefinesSquareBoxes_alignLeft:
                    usedAspectCorrectionFactor_toMakeBoxesSquare = 1.0f / camera.aspect;
                    DrawMainScreenspaceGridLines(camera, 1.0f, usedAspectCorrectionFactor_toMakeBoxesSquare, false, false, color, linesWidth_relToViewportHeight, durationInSec);
                    DrawConfigurableScreenspaceGrid(camera, drawTenthLines, 10, 1.0f, usedAspectCorrectionFactor_toMakeBoxesSquare, false, false, colorFor01, durationInSec);
                    DrawConfigurableScreenspaceGrid(camera, drawHundredthLines, 100, 1.0f, usedAspectCorrectionFactor_toMakeBoxesSquare, false, false, colorFor001, durationInSec);
                    break;
                case GridScreenspaceMode.screenHeightDefinesSquareBoxes_alignRight:
                    usedAspectCorrectionFactor_toMakeBoxesSquare = 1.0f / camera.aspect;
                    DrawMainScreenspaceGridLines(camera, 1.0f, usedAspectCorrectionFactor_toMakeBoxesSquare, true, false, color, linesWidth_relToViewportHeight, durationInSec);
                    DrawConfigurableScreenspaceGrid(camera, drawTenthLines, 10, 1.0f, usedAspectCorrectionFactor_toMakeBoxesSquare, true, false, colorFor01, durationInSec);
                    DrawConfigurableScreenspaceGrid(camera, drawHundredthLines, 100, 1.0f, usedAspectCorrectionFactor_toMakeBoxesSquare, true, false, colorFor001, durationInSec);
                    break;
                case GridScreenspaceMode.screenWidthDefinesSquareBoxes_alignAtBottom:
                    usedAspectCorrectionFactor_toMakeBoxesSquare = camera.aspect;
                    DrawMainScreenspaceGridLines(camera, usedAspectCorrectionFactor_toMakeBoxesSquare, 1.0f, false, false, color, linesWidth_relToViewportHeight, durationInSec);
                    DrawConfigurableScreenspaceGrid(camera, drawTenthLines, 10, usedAspectCorrectionFactor_toMakeBoxesSquare, 1.0f, false, false, colorFor01, durationInSec);
                    DrawConfigurableScreenspaceGrid(camera, drawHundredthLines, 100, usedAspectCorrectionFactor_toMakeBoxesSquare, 1.0f, false, false, colorFor001, durationInSec);
                    break;
                case GridScreenspaceMode.screenWidthDefinesSquareBoxes_alignAtTop:
                    usedAspectCorrectionFactor_toMakeBoxesSquare = camera.aspect;
                    DrawMainScreenspaceGridLines(camera, usedAspectCorrectionFactor_toMakeBoxesSquare, 1.0f, false, true, color, linesWidth_relToViewportHeight, durationInSec);
                    DrawConfigurableScreenspaceGrid(camera, drawTenthLines, 10, usedAspectCorrectionFactor_toMakeBoxesSquare, 1.0f, false, true, colorFor01, durationInSec);
                    DrawConfigurableScreenspaceGrid(camera, drawHundredthLines, 100, usedAspectCorrectionFactor_toMakeBoxesSquare, 1.0f, false, true, colorFor001, durationInSec);
                    break;
                default:
                    break;
            }

        }

        static void DrawMainScreenspaceGridLines(Camera camera, float scaleFactor_forHorizLines, float scaleFactor_forVertLines, bool startAtRightBorder_insteadOfLeft, bool startAtTopBorder_insteadOfBottom, Color color, float linesWidth_relToViewportHeight, float durationInSec)
        {
            //horiz lines:
            if (startAtTopBorder_insteadOfBottom)
            {
                Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(0.0f, 1.0f), new Vector2(1.0f, 1.0f), color, linesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, DrawScreenspace.minTextSize_relToViewportHeight, durationInSec);
                Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(0.0f, 1.0f - (0.5f * scaleFactor_forHorizLines)), new Vector2(1.0f, 1.0f - (0.5f * scaleFactor_forHorizLines)), color, linesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, DrawScreenspace.minTextSize_relToViewportHeight, durationInSec);
                Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(0.0f, 1.0f - (1.0f * scaleFactor_forHorizLines)), new Vector2(1.0f, 1.0f - (1.0f * scaleFactor_forHorizLines)), color, linesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, DrawScreenspace.minTextSize_relToViewportHeight, durationInSec);
            }
            else
            {
                Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(0.0f, 0.0f), new Vector2(1.0f, 0.0f), color, linesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, DrawScreenspace.minTextSize_relToViewportHeight, durationInSec);
                Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(0.0f, 0.5f * scaleFactor_forHorizLines), new Vector2(1.0f, 0.5f * scaleFactor_forHorizLines), color, linesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, DrawScreenspace.minTextSize_relToViewportHeight, durationInSec);
                Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(0.0f, 1.0f * scaleFactor_forHorizLines), new Vector2(1.0f, 1.0f * scaleFactor_forHorizLines), color, linesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, DrawScreenspace.minTextSize_relToViewportHeight, durationInSec);
            }

            //vert lines:
            if (startAtRightBorder_insteadOfLeft)
            {
                Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(1.0f, 0.0f), new Vector2(1.0f, 1.0f), color, linesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, DrawScreenspace.minTextSize_relToViewportHeight, durationInSec);
                Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(1.0f - (0.5f * scaleFactor_forVertLines), 0.0f), new Vector2(1.0f - (0.5f * scaleFactor_forVertLines), 1.0f), color, linesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, DrawScreenspace.minTextSize_relToViewportHeight, durationInSec);
                Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(1.0f - (1.0f * scaleFactor_forVertLines), 0.0f), new Vector2(1.0f - (1.0f * scaleFactor_forVertLines), 1.0f), color, linesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, DrawScreenspace.minTextSize_relToViewportHeight, durationInSec);
            }
            else
            {
                Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(0.0f, 0.0f), new Vector2(0.0f, 1.0f), color, linesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, DrawScreenspace.minTextSize_relToViewportHeight, durationInSec);
                Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(0.5f * scaleFactor_forVertLines, 0.0f), new Vector2(0.5f * scaleFactor_forVertLines, 1.0f), color, linesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, DrawScreenspace.minTextSize_relToViewportHeight, durationInSec);
                Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(1.0f * scaleFactor_forVertLines, 0.0f), new Vector2(1.0f * scaleFactor_forVertLines, 1.0f), color, linesWidth_relToViewportHeight, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, DrawScreenspace.minTextSize_relToViewportHeight, durationInSec);
            }
        }

        static void DrawConfigurableScreenspaceGrid(Camera camera, bool drawThis, int numberOfLines, float scaleFactor_forHorizLines, float scaleFactor_forVertLines, bool startAtRightBorder_insteadOfLeft, bool startAtTopBorder_insteadOfBottom, Color color, float durationInSec)
        {
            if (drawThis)
            {
                float progress0to1_perLine = 1.0f / (float)numberOfLines;

                //horiz lines:
                float progress0to1_perLine_forHorizLines = progress0to1_perLine * scaleFactor_forHorizLines;

                int numberOfHorizLines_minus1_overshootingToFillWholeScreen;
                if (UtilitiesDXXL_Math.ApproximatelyZero(scaleFactor_forHorizLines))
                {
                    numberOfHorizLines_minus1_overshootingToFillWholeScreen = numberOfLines - 1; //-> "minus 1" is for not overdrawing the mainLine once more to not alter mainLines alpha value
                }
                else
                {
                    numberOfHorizLines_minus1_overshootingToFillWholeScreen = Mathf.RoundToInt((float)numberOfLines / scaleFactor_forHorizLines);
                }

                for (int i = 1; i <= numberOfHorizLines_minus1_overshootingToFillWholeScreen; i++)
                {
                    float progress0to1_forHorizLines = progress0to1_perLine_forHorizLines * i;
                    if (startAtTopBorder_insteadOfBottom) { progress0to1_forHorizLines = 1.0f - progress0to1_forHorizLines; }
                    Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(0.0f, progress0to1_forHorizLines), new Vector2(1.0f, progress0to1_forHorizLines), color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, DrawScreenspace.minTextSize_relToViewportHeight, durationInSec);
                }

                //vert lines:
                float progress0to1_perLine_forVertLines = progress0to1_perLine * scaleFactor_forVertLines;

                int numberOfVertLines_minus1_overshootingToFillWholeScreen;
                if (UtilitiesDXXL_Math.ApproximatelyZero(scaleFactor_forVertLines))
                {
                    numberOfVertLines_minus1_overshootingToFillWholeScreen = numberOfLines - 1; //-> "minus 1" is for not overdrawing the mainLine once more to not alter mainLines alpha value
                }
                else
                {
                    numberOfVertLines_minus1_overshootingToFillWholeScreen = Mathf.RoundToInt((float)numberOfLines / scaleFactor_forVertLines);
                }

                for (int i = 1; i <= numberOfVertLines_minus1_overshootingToFillWholeScreen; i++)
                {
                    float progress0to1_forVertLines = progress0to1_perLine_forVertLines * i;
                    if (startAtRightBorder_insteadOfLeft) { progress0to1_forVertLines = 1.0f - progress0to1_forVertLines; }
                    Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, new Vector2(progress0to1_forVertLines, 0.0f), new Vector2(progress0to1_forVertLines, 1.0f), color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, DrawScreenspace.minTextSize_relToViewportHeight, durationInSec);
                }
            }
        }

    }

}
