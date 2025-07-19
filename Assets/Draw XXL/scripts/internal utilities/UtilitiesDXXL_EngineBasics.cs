namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class UtilitiesDXXL_EngineBasics
    {

        public static void VectorFrom_local(Vector3 origin_ofLocalSpace_inGlobalSpace, Vector3 scale_ofLocalSpace, Quaternion rotation_ofLocalSpace, Vector3 vectorStartPos, Vector3 vector, Color color, float lineWidth, string text, float durationInSec, bool hiddenByNearerObjects, bool isLocal)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lineWidth, "lineWidth")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vectorStartPos, "vectorStartPos")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vector, "vector")) { return; }

            color = UtilitiesDXXL_Colors.OverwriteDefaultColor(color);
            Color colorOfBoxLines = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(Color.white, 0.2f);

            Vector3 vector_inLocalSpace = vector;
            Vector3 vectorStartPos_inLocalSpace = vectorStartPos;
            Vector3 vectorEndPos_inLocalSpace = vectorStartPos_inLocalSpace + vector_inLocalSpace;

            Vector3 vectorStartPos_inGlobalSpace = origin_ofLocalSpace_inGlobalSpace + rotation_ofLocalSpace * Vector3.Scale(scale_ofLocalSpace, vectorStartPos_inLocalSpace);
            Vector3 vectorEndPos_inGlobalSpace = origin_ofLocalSpace_inGlobalSpace + rotation_ofLocalSpace * Vector3.Scale(scale_ofLocalSpace, vectorEndPos_inLocalSpace);
            Vector3 vector_inGlobalSpace = vectorEndPos_inGlobalSpace - vectorStartPos_inGlobalSpace;

            Vector3 localVectorsXComponentAsVector_inLocalSpace = new Vector3(vector_inLocalSpace.x, 0.0f, 0.0f);
            Vector3 localVectorsYComponentAsVector_inLocalSpace = new Vector3(0.0f, vector_inLocalSpace.y, 0.0f);
            Vector3 localVectorsZComponentAsVector_inLocalSpace = new Vector3(0.0f, 0.0f, vector_inLocalSpace.z);

            Vector3 localVectorsLocalXComponentAsVector_inGlobalSpace = rotation_ofLocalSpace * Vector3.Scale(scale_ofLocalSpace, localVectorsXComponentAsVector_inLocalSpace);
            Vector3 localVectorsLocalYComponentAsVector_inGlobalSpace = rotation_ofLocalSpace * Vector3.Scale(scale_ofLocalSpace, localVectorsYComponentAsVector_inLocalSpace);
            Vector3 localVectorsLocalZComponentAsVector_inGlobalSpace = rotation_ofLocalSpace * Vector3.Scale(scale_ofLocalSpace, localVectorsZComponentAsVector_inLocalSpace);

            Vector3 endPos_ofXDirComponentFromVectorStart_inGlobalSpace = vectorStartPos_inGlobalSpace + localVectorsLocalXComponentAsVector_inGlobalSpace;
            Vector3 endPos_ofYDirComponentFromVectorStart_inGlobalSpace = vectorStartPos_inGlobalSpace + localVectorsLocalYComponentAsVector_inGlobalSpace;
            Vector3 endPos_ofZDirComponentFromVectorStart_inGlobalSpace = vectorStartPos_inGlobalSpace + localVectorsLocalZComponentAsVector_inGlobalSpace;

            //grey box lines:
            Line_fadeableAnimSpeed.InternalDraw(vectorStartPos_inGlobalSpace, endPos_ofXDirComponentFromVectorStart_inGlobalSpace, colorOfBoxLines, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(vectorStartPos_inGlobalSpace, endPos_ofYDirComponentFromVectorStart_inGlobalSpace, colorOfBoxLines, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(vectorStartPos_inGlobalSpace, endPos_ofZDirComponentFromVectorStart_inGlobalSpace, colorOfBoxLines, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            Line_fadeableAnimSpeed.InternalDraw(endPos_ofXDirComponentFromVectorStart_inGlobalSpace, endPos_ofXDirComponentFromVectorStart_inGlobalSpace + localVectorsLocalYComponentAsVector_inGlobalSpace, colorOfBoxLines, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(endPos_ofXDirComponentFromVectorStart_inGlobalSpace, endPos_ofXDirComponentFromVectorStart_inGlobalSpace + localVectorsLocalZComponentAsVector_inGlobalSpace, colorOfBoxLines, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            Line_fadeableAnimSpeed.InternalDraw(endPos_ofYDirComponentFromVectorStart_inGlobalSpace, endPos_ofYDirComponentFromVectorStart_inGlobalSpace + localVectorsLocalXComponentAsVector_inGlobalSpace, colorOfBoxLines, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(endPos_ofYDirComponentFromVectorStart_inGlobalSpace, endPos_ofYDirComponentFromVectorStart_inGlobalSpace + localVectorsLocalZComponentAsVector_inGlobalSpace, colorOfBoxLines, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            Line_fadeableAnimSpeed.InternalDraw(endPos_ofZDirComponentFromVectorStart_inGlobalSpace, endPos_ofZDirComponentFromVectorStart_inGlobalSpace + localVectorsLocalXComponentAsVector_inGlobalSpace, colorOfBoxLines, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(endPos_ofZDirComponentFromVectorStart_inGlobalSpace, endPos_ofZDirComponentFromVectorStart_inGlobalSpace + localVectorsLocalYComponentAsVector_inGlobalSpace, colorOfBoxLines, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            Line_fadeableAnimSpeed.InternalDraw(vectorEndPos_inGlobalSpace, vectorEndPos_inGlobalSpace - localVectorsLocalXComponentAsVector_inGlobalSpace, colorOfBoxLines, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(vectorEndPos_inGlobalSpace, vectorEndPos_inGlobalSpace - localVectorsLocalYComponentAsVector_inGlobalSpace, colorOfBoxLines, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(vectorEndPos_inGlobalSpace, vectorEndPos_inGlobalSpace - localVectorsLocalZComponentAsVector_inGlobalSpace, colorOfBoxLines, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            //three component vectors:
            Vector3 absLocalVector_scaledToGlobalUnits_butNotYetRotatedToGlobalSpace = UtilitiesDXXL_Math.Abs(Vector3.Scale(scale_ofLocalSpace, vector_inLocalSpace));
            float biggestAbsComponent_inGlobalSpace = UtilitiesDXXL_Math.GetBiggestAbsComponent(vector_inGlobalSpace);
            float componentVectorsConeLength = 0.03f * biggestAbsComponent_inGlobalSpace;
            float minTextSize_ofNonZeroComponents = Mathf.Max(biggestAbsComponent_inGlobalSpace * 0.02f, 0.02f);
            float textSize_ifComponentIsZero = Mathf.Max(biggestAbsComponent_inGlobalSpace * 0.05f, 0.02f);

            string text_ofXComponent = isLocal ? ("<size=2>local</size>x = " + vector_inLocalSpace.x) : ("x = " + vector_inLocalSpace.x);
            if (absLocalVector_scaledToGlobalUnits_butNotYetRotatedToGlobalSpace.x < 0.0001f)
            {
                float textSize = (vector_inLocalSpace.x == 0.0f) ? textSize_ifComponentIsZero : minTextSize_ofNonZeroComponents; //"==" instead of "ApproxZero"-Check is intentional
                UtilitiesDXXL_Text.Write(text_ofXComponent, vectorStartPos_inGlobalSpace, UtilitiesDXXL_Colors.red_xAxis, textSize, rotation_ofLocalSpace * Vector3.right, rotation_ofLocalSpace * Vector3.up, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
            }
            else
            {
                UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                DrawBasics.Vector(vectorStartPos_inGlobalSpace, endPos_ofXDirComponentFromVectorStart_inGlobalSpace, UtilitiesDXXL_Colors.red_xAxis, 0.0f, text_ofXComponent, componentVectorsConeLength, false, false, default(Vector3), false, minTextSize_ofNonZeroComponents, false, 0.0f, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
            }

            Vector3 startPosOfFinalZSegment_inGlobalSpace = endPos_ofXDirComponentFromVectorStart_inGlobalSpace + localVectorsLocalYComponentAsVector_inGlobalSpace;
            string text_ofYComponent = isLocal ? ("<size=2>local</size>y = " + vector_inLocalSpace.y) : ("y = " + vector_inLocalSpace.y);
            if (absLocalVector_scaledToGlobalUnits_butNotYetRotatedToGlobalSpace.y < 0.0001f)
            {
                float textSize = (vector_inLocalSpace.y == 0.0f) ? textSize_ifComponentIsZero : minTextSize_ofNonZeroComponents; //"==" instead of "ApproxZero"-Check is intentional
                UtilitiesDXXL_Text.Write(text_ofYComponent, endPos_ofXDirComponentFromVectorStart_inGlobalSpace, UtilitiesDXXL_Colors.green_yAxis, textSize, rotation_ofLocalSpace * Vector3.up, rotation_ofLocalSpace * Vector3.left, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
            }
            else
            {
                UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                DrawBasics.Vector(endPos_ofXDirComponentFromVectorStart_inGlobalSpace, startPosOfFinalZSegment_inGlobalSpace, UtilitiesDXXL_Colors.green_yAxis, 0.0f, text_ofYComponent, componentVectorsConeLength, false, false, default(Vector3), false, minTextSize_ofNonZeroComponents, false, 0.0f, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
            }

            string text_ofZComponent = isLocal ? ("<size=2>local</size>z = " + vector_inLocalSpace.z) : ("z = " + vector_inLocalSpace.z);
            if (absLocalVector_scaledToGlobalUnits_butNotYetRotatedToGlobalSpace.z < 0.0001f)
            {
                float textSize = (vector_inLocalSpace.z == 0.0f) ? textSize_ifComponentIsZero : minTextSize_ofNonZeroComponents; //"==" instead of "ApproxZero"-Check is intentional
                UtilitiesDXXL_Text.Write(text_ofZComponent, startPosOfFinalZSegment_inGlobalSpace, UtilitiesDXXL_Colors.blue_zAxis, textSize, rotation_ofLocalSpace * Vector3.forward, rotation_ofLocalSpace * Vector3.up, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
            }
            else
            {
                UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                DrawBasics.Vector(startPosOfFinalZSegment_inGlobalSpace, vectorEndPos_inGlobalSpace, UtilitiesDXXL_Colors.blue_zAxis, 0.0f, text_ofZComponent, componentVectorsConeLength, false, false, default(Vector3), false, minTextSize_ofNonZeroComponents, false, 0.0f, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
            }

            //main vector:
            float lineWidth_inGlobalSpace = lineWidth;
            lineWidth_inGlobalSpace = Mathf.Max(lineWidth_inGlobalSpace, 0.005f);
            float length_inGlobalSpace = vector_inGlobalSpace.magnitude;
            float length_inLocalSpace = vector_inLocalSpace.magnitude;
            lineWidth_inGlobalSpace = Mathf.Min(lineWidth_inGlobalSpace, 0.2f * length_inGlobalSpace);
            bool addNormalizedMarkingText = !isLocal;
            string mainVectorText = isLocal ? ("<size=2>local</size>length = " + length_inLocalSpace + "<br><br>" + text) : ("length = " + length_inLocalSpace + "<br><br>" + text);

            UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
            DrawBasics.Vector(vectorStartPos_inGlobalSpace, vectorEndPos_inGlobalSpace, color, lineWidth_inGlobalSpace, mainVectorText, 0.17f, false, false, default(Vector3), addNormalizedMarkingText, 0.01f, false, 0.0f, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
        }

        public static void LocalScale(Vector3 localPosition, Vector3 localScale, Transform parentTransform, Quaternion localRotation, float lineWidth, string text, bool drawXDim, bool drawYDim, bool drawZDim, float relSizeOfPlanes, Color overwriteColor, float durationInSec, bool hiddenByNearerObjects, bool isGlobalNotLocalScale)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lineWidth, "lineWidth")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(relSizeOfPlanes, "relSizeOfPlanes")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(localPosition, "localPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(localScale, "localScale")) { return; }

            Color color_forX = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor, UtilitiesDXXL_Colors.red_xAxis);
            Color color_forY = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor, UtilitiesDXXL_Colors.green_yAxis);
            Color color_forZ = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor, UtilitiesDXXL_Colors.blue_zAxis);

            bool noParent = parentTransform == null;
            bool noLocalRotation = UtilitiesDXXL_Math.IsDefaultInvalidQuaternion(localRotation) || UtilitiesDXXL_Math.IsQuaternionIdentity(localRotation);
            Vector3 worldPosition = noParent ? localPosition : parentTransform.TransformPoint(localPosition);
            Vector3 lossyScale = noParent ? localScale : Vector3.Scale(localScale, parentTransform.lossyScale);

            Vector3 rightOfChildTransform_insideHisLocalSpace_normalized = noLocalRotation ? Vector3.right : localRotation * Vector3.right;
            Vector3 upOfChildTransform_insideHisLocalSpace_normalized = noLocalRotation ? Vector3.up : localRotation * Vector3.up;
            Vector3 forwardOfChildTransform_insideHisLocalSpace_normalized = noLocalRotation ? Vector3.forward : localRotation * Vector3.forward;

            Vector3 localRight_expressedInWorldSpaceUnits_normalized = noParent ? rightOfChildTransform_insideHisLocalSpace_normalized : parentTransform.rotation * rightOfChildTransform_insideHisLocalSpace_normalized;
            Vector3 localUp_expressedInWorldSpaceUnits_normalized = noParent ? upOfChildTransform_insideHisLocalSpace_normalized : parentTransform.rotation * upOfChildTransform_insideHisLocalSpace_normalized;
            Vector3 localForward_expressedInWorldSpaceUnits_normalized = noParent ? forwardOfChildTransform_insideHisLocalSpace_normalized : parentTransform.rotation * forwardOfChildTransform_insideHisLocalSpace_normalized;

            Vector3 absLossyScale = UtilitiesDXXL_Math.Abs(lossyScale);
            Vector3 halfAbsLossyScale = 0.5f * lossyScale;

            Vector3 x_negativeEnd_worldSpace = worldPosition - localRight_expressedInWorldSpaceUnits_normalized * halfAbsLossyScale.x;
            Vector3 x_positiveEnd_worldSpace = worldPosition + localRight_expressedInWorldSpaceUnits_normalized * halfAbsLossyScale.x;

            float vectorLenghtThreshold_belowWhichToUseRelConeLengths = 0.45f;

            if (drawXDim)
            {
                if (absLossyScale.x > 0.002f)
                {
                    bool setConeLengthToRelative_notToAbsolute = (absLossyScale.x < vectorLenghtThreshold_belowWhichToUseRelConeLengths);
                    float coneLength_ifSetToRelative = 0.1f / vectorLenghtThreshold_belowWhichToUseRelConeLengths;
                    float coneLength_ifSetToAbsolute = 0.1f;
                    float coneLength = UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(setConeLengthToRelative_notToAbsolute, coneLength_ifSetToRelative, coneLength_ifSetToAbsolute);
                    DrawBasics.Vector(x_negativeEnd_worldSpace, x_positiveEnd_worldSpace, color_forX, lineWidth, isGlobalNotLocalScale ? "x = " + localScale.x : "<size=2>local</size>x = " + localScale.x, coneLength, true, false, default(Vector3), false, 0.005f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                }
                else
                {
                    UtilitiesDXXL_Text.Write(isGlobalNotLocalScale ? "x = " + localScale.x : "<size=2>local</size>x = " + localScale.x, worldPosition, color_forX, 0.01f, localRight_expressedInWorldSpaceUnits_normalized, localUp_expressedInWorldSpaceUnits_normalized, DrawText.TextAnchorDXXL.LowerLeft, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                }
            }

            Vector3 y_negativeEnd_worldSpace = worldPosition - localUp_expressedInWorldSpaceUnits_normalized * halfAbsLossyScale.y;
            Vector3 y_positiveEnd_worldSpace = worldPosition + localUp_expressedInWorldSpaceUnits_normalized * halfAbsLossyScale.y;
            if (drawYDim)
            {
                if (absLossyScale.y > 0.002f)
                {
                    bool setConeLengthToRelative_notToAbsolute = (absLossyScale.y < vectorLenghtThreshold_belowWhichToUseRelConeLengths);
                    float coneLength_ifSetToRelative = 0.1f / vectorLenghtThreshold_belowWhichToUseRelConeLengths;
                    float coneLength_ifSetToAbsolute = 0.1f;
                    float coneLength = UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(setConeLengthToRelative_notToAbsolute, coneLength_ifSetToRelative, coneLength_ifSetToAbsolute);
                    DrawBasics.Vector(y_negativeEnd_worldSpace, y_positiveEnd_worldSpace, color_forY, lineWidth, isGlobalNotLocalScale ? "y = " + localScale.y : "<size=2>local</size>y = " + localScale.y, coneLength, true, false, default(Vector3), false, 0.005f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                }
                else
                {
                    UtilitiesDXXL_Text.Write(isGlobalNotLocalScale ? "y = " + localScale.y : "<size=2>local</size>y = " + localScale.y, worldPosition, color_forY, 0.01f, localUp_expressedInWorldSpaceUnits_normalized, -localRight_expressedInWorldSpaceUnits_normalized, DrawText.TextAnchorDXXL.LowerLeft, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                }
            }

            Vector3 z_negativeEnd_worldSpace = worldPosition - localForward_expressedInWorldSpaceUnits_normalized * halfAbsLossyScale.z;
            Vector3 z_positiveEnd_worldSpace = worldPosition + localForward_expressedInWorldSpaceUnits_normalized * halfAbsLossyScale.z;
            if (drawZDim)
            {
                if (absLossyScale.z > 0.002f)
                {
                    bool setConeLengthToRelative_notToAbsolute = (absLossyScale.z < vectorLenghtThreshold_belowWhichToUseRelConeLengths);
                    float coneLength_ifSetToRelative = 0.1f / vectorLenghtThreshold_belowWhichToUseRelConeLengths;
                    float coneLength_ifSetToAbsolute = 0.1f;
                    float coneLength = UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(setConeLengthToRelative_notToAbsolute, coneLength_ifSetToRelative, coneLength_ifSetToAbsolute);
                    DrawBasics.Vector(z_negativeEnd_worldSpace, z_positiveEnd_worldSpace, color_forZ, lineWidth, isGlobalNotLocalScale ? "z = " + localScale.z : "<size=2>local</size>z = " + localScale.z, coneLength, true, false, default(Vector3), false, 0.005f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                }
                else
                {
                    UtilitiesDXXL_Text.Write(isGlobalNotLocalScale ? "z = " + localScale.z : "<size=2>local</size>z = " + localScale.z, worldPosition, color_forZ, 0.01f, localForward_expressedInWorldSpaceUnits_normalized, localUp_expressedInWorldSpaceUnits_normalized, DrawText.TextAnchorDXXL.LowerLeft, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                }
            }

            relSizeOfPlanes = Mathf.Abs(relSizeOfPlanes);
            bool drawPlanes = (relSizeOfPlanes > 0.01f);
            if (drawPlanes)
            {
                relSizeOfPlanes = Mathf.Min(relSizeOfPlanes, 1.0f);
                Vector3 planesScale_worldSpace = relSizeOfPlanes * lossyScale;

                if (drawXDim)
                {
                    if (absLossyScale.y > 0.002 || absLossyScale.z > 0.002)
                    {
                        DrawShapes.Plane(x_negativeEnd_worldSpace, localRight_expressedInWorldSpaceUnits_normalized, default(Vector3), color_forX, planesScale_worldSpace.z, planesScale_worldSpace.y, localUp_expressedInWorldSpaceUnits_normalized, 0.0f, null, 6, false, 0.0f, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
                        DrawShapes.Plane(x_positiveEnd_worldSpace, localRight_expressedInWorldSpaceUnits_normalized, default(Vector3), color_forX, planesScale_worldSpace.z, planesScale_worldSpace.y, localUp_expressedInWorldSpaceUnits_normalized, 0.0f, null, 6, false, 0.0f, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
                    }
                }

                if (drawYDim)
                {
                    if (absLossyScale.x > 0.002 || absLossyScale.z > 0.002)
                    {
                        DrawShapes.Plane(y_negativeEnd_worldSpace, localUp_expressedInWorldSpaceUnits_normalized, default(Vector3), color_forY, planesScale_worldSpace.x, planesScale_worldSpace.z, localForward_expressedInWorldSpaceUnits_normalized, 0.0f, null, 6, false, 0.0f, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
                        DrawShapes.Plane(y_positiveEnd_worldSpace, localUp_expressedInWorldSpaceUnits_normalized, default(Vector3), color_forY, planesScale_worldSpace.x, planesScale_worldSpace.z, localForward_expressedInWorldSpaceUnits_normalized, 0.0f, null, 6, false, 0.0f, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
                    }
                }

                if (drawZDim)
                {
                    if (absLossyScale.x > 0.002 || absLossyScale.y > 0.002)
                    {
                        DrawShapes.Plane(z_negativeEnd_worldSpace, localForward_expressedInWorldSpaceUnits_normalized, default(Vector3), color_forZ, planesScale_worldSpace.x, planesScale_worldSpace.y, localUp_expressedInWorldSpaceUnits_normalized, 0.0f, null, 6, false, 0.0f, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
                        DrawShapes.Plane(z_positiveEnd_worldSpace, localForward_expressedInWorldSpaceUnits_normalized, default(Vector3), color_forZ, planesScale_worldSpace.x, planesScale_worldSpace.y, localUp_expressedInWorldSpaceUnits_normalized, 0.0f, null, 6, false, 0.0f, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
                    }
                }
            }

            if (drawXDim == false && drawYDim == false && drawZDim == false)
            {
                if (isGlobalNotLocalScale)
                {
                    text = "[<color=#adadadFF><icon=logMessage></color> Scale with all dimensions deactivated]<br>" + text;
                }
                else
                {
                    text = "[<color=#adadadFF><icon=logMessage></color> LocalScale with all dimensions deactivated]<br>" + text;
                }
            }

            if (CheckIf_transformOrAParentHasNonUniformScale(parentTransform))
            {
                text = "[<color=#e2aa00FF><icon=warning></color> LocalScale: Transform has a parent with non-uniform scale<br>   -> possibly weird results]<br>" + text;
            }

            if (text != null && text != "")
            {
                Vector3 textPos = worldPosition + localRight_expressedInWorldSpaceUnits_normalized * halfAbsLossyScale.x + localUp_expressedInWorldSpaceUnits_normalized * halfAbsLossyScale.y - localForward_expressedInWorldSpaceUnits_normalized * halfAbsLossyScale.z;
                Vector3 textOffsetDir = textPos - worldPosition;
                DrawBasics.PointTag(worldPosition, text, Color.Lerp(color_forX, Color.white, 0.5f), 0.3f * lineWidth, textOffsetDir.magnitude, textOffsetDir, 1.0f, true, durationInSec, hiddenByNearerObjects);
            }
        }

        public static bool CheckIf_transformOrAParentHasNonUniformScale(Transform transformToCheck)
        {
            if (transformToCheck != null)
            {
                if (UtilitiesDXXL_Math.IsVectorApproxUniform(transformToCheck.localScale) == false)
                {
                    return true;
                }
                else
                {
                    Transform[] transformsOfParents = transformToCheck.GetComponentsInParent<Transform>(true);
                    if (transformsOfParents != null)
                    {
                        for (int i = 0; i < transformsOfParents.Length; i++)
                        {
                            if (transformsOfParents[i] != null)
                            {
                                if (UtilitiesDXXL_Math.IsVectorApproxUniform(transformsOfParents[i].localScale) == false)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static bool CheckIf_transformOrAParentHasNonUniformScale_2D(Transform transformToCheck)
        {
            if (transformToCheck != null)
            {
                Vector2 parentLocalScale_withoutZ = new Vector2(transformToCheck.localScale.x, transformToCheck.localScale.y);
                if (UtilitiesDXXL_Math.IsVectorApproxUniform(parentLocalScale_withoutZ) == false)
                {
                    return true;
                }
                else
                {
                    Transform[] transformsOfParents = transformToCheck.GetComponentsInParent<Transform>(true);
                    if (transformsOfParents != null)
                    {
                        for (int i = 0; i < transformsOfParents.Length; i++)
                        {
                            if (transformsOfParents[i] != null)
                            {
                                Vector2 currParentLocalScale_withoutZ = new Vector2(transformsOfParents[i].localScale.x, transformsOfParents[i].localScale.y);
                                if (UtilitiesDXXL_Math.IsVectorApproxUniform(currParentLocalScale_withoutZ) == false)
                                {
                                    return true;
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        public static bool CheckIfThisOrAParentHasANonZRotation_2D(Transform thisTransform)
        {
            if (thisTransform != null)
            {
                Vector3 localEulerAngles_ofThisTransform = thisTransform.localRotation.eulerAngles;
                if (CheckIfAnEulerAngleSetContainsANonZRotation(localEulerAngles_ofThisTransform))
                {
                    return true;
                }
                else
                {
                    if (thisTransform.parent != null)
                    {
                        Transform[] transformsOfParents = thisTransform.parent.GetComponentsInParent<Transform>(true);
                        if (transformsOfParents != null)
                        {
                            for (int i = 0; i < transformsOfParents.Length; i++)
                            {
                                if (transformsOfParents[i] != null)
                                {
                                    Vector3 localEulerAngles_ofCurrParentTransform = transformsOfParents[i].localRotation.eulerAngles;
                                    if (CheckIfAnEulerAngleSetContainsANonZRotation(localEulerAngles_ofCurrParentTransform))
                                    {
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        static bool CheckIfAnEulerAngleSetContainsANonZRotation(Vector3 eulerAngles_toCheck)
        {
            if (CheckIfAnEulerAngleMeansApproxNoRotation(eulerAngles_toCheck.x) == false)
            {
                return true;
            }
            if (CheckIfAnEulerAngleMeansApproxNoRotation(eulerAngles_toCheck.y) == false)
            {
                return true;
            }
            return false;
        }

        static bool CheckIfAnEulerAngleMeansApproxNoRotation(float eulerAngle_toCheck)
        {
            if (UtilitiesDXXL_Math.CheckIfValueLiesInsideDistanceNearAnotherValue(0.0f, eulerAngle_toCheck, 0.001f))
            {
                return true;
            }
            else
            {
                if (UtilitiesDXXL_Math.CheckIfValueLiesInsideDistanceNearAnotherValue(360.0f, eulerAngle_toCheck, 0.001f))
                {
                    return true;
                }
                else
                {
                    if (UtilitiesDXXL_Math.CheckIfValueLiesInsideDistanceNearAnotherValue(-360.0f, eulerAngle_toCheck, 0.001f))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        static Color colorOfVector1_forDotProduct_before;
        public static void Set_colorOfVector1_forDotProduct_reversible(Color new_colorOfVector1_forDotProduct)
        {
            colorOfVector1_forDotProduct_before = DrawEngineBasics.colorOfVector1_forDotProduct;
            DrawEngineBasics.colorOfVector1_forDotProduct = new_colorOfVector1_forDotProduct;
        }
        public static void Reverse_colorOfVector1_forDotProduct()
        {
            DrawEngineBasics.colorOfVector1_forDotProduct = colorOfVector1_forDotProduct_before;
        }

        static Color colorOfVector2_forDotProduct_before;
        public static void Set_colorOfVector2_forDotProduct_reversible(Color new_colorOfVector2_forDotProduct)
        {
            colorOfVector2_forDotProduct_before = DrawEngineBasics.colorOfVector2_forDotProduct;
            DrawEngineBasics.colorOfVector2_forDotProduct = new_colorOfVector2_forDotProduct;
        }
        public static void Reverse_colorOfVector2_forDotProduct()
        {
            DrawEngineBasics.colorOfVector2_forDotProduct = colorOfVector2_forDotProduct_before;
        }

        static Color colorOfAngle_forDotProduct_before;
        public static void Set_colorOfAngle_forDotProduct_reversible(Color new_colorOfAngle_forDotProduct)
        {
            colorOfAngle_forDotProduct_before = DrawEngineBasics.colorOfAngle_forDotProduct;
            DrawEngineBasics.colorOfAngle_forDotProduct = new_colorOfAngle_forDotProduct;
        }
        public static void Reverse_colorOfAngle_forDotProduct()
        {
            DrawEngineBasics.colorOfAngle_forDotProduct = colorOfAngle_forDotProduct_before;
        }

        static Color colorOfResult_forDotProduct_before;
        public static void Set_colorOfResult_forDotProduct_reversible(Color new_colorOfResult_forDotProduct)
        {
            colorOfResult_forDotProduct_before = DrawEngineBasics.colorOfResult_forDotProduct;
            DrawEngineBasics.colorOfResult_forDotProduct = new_colorOfResult_forDotProduct;
        }
        public static void Reverse_colorOfResult_forDotProduct()
        {
            DrawEngineBasics.colorOfResult_forDotProduct = colorOfResult_forDotProduct_before;
        }

        //static float minTextSize_atVectorFrom_dotAndCrossProduct = 0.04f; //-> bigger text size: better readable, but interfering with vectorCone
        static float minTextSize_atVectorFrom_dotAndCrossProduct = 0.03f;
        static InternalDXXL_Plane planePerpTo_vector1 = new InternalDXXL_Plane();
        static InternalDXXL_Plane planePerpTo_vector2 = new InternalDXXL_Plane();

        public static void DotProduct(Vector3 vector1_lhs, Vector3 vector2_rhs, Vector3 posWhereToDraw = default(Vector3), float linesWidth = 0.0025f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            //"lhs" = "left hand side (of equation)"
            //"rhs" = "right hand side (of equation)"

            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vector1_lhs, "vector1_lhs")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vector2_rhs, "vector2_rhs")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(posWhereToDraw, "posWhereToDraw")) { return; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);

            float vector1_magnitude = vector1_lhs.magnitude;
            float vector2_magnitude = vector2_rhs.magnitude;

            Vector3 vector1_scaledIntoRegionOfFloatPrecision = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(vector1_lhs);
            Vector3 vector2_scaledIntoRegionOfFloatPrecision = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(vector2_rhs);

            float angleDeg = Vector3.Angle(vector1_scaledIntoRegionOfFloatPrecision, vector2_scaledIntoRegionOfFloatPrecision);
            float angleRad = Mathf.Deg2Rad * angleDeg;
            bool angleIsTooSmallForStableDrawing = angleDeg < 0.1f;

            Color color_ofVector1 = DrawEngineBasics.colorOfVector1_forDotProduct;
            Color color_ofVector2 = DrawEngineBasics.colorOfVector2_forDotProduct;
            Color color_ofAngle = DrawEngineBasics.colorOfAngle_forDotProduct;
            Color color_ofVertAxis = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(Color.white, 0.2f);

            Vector3 perpVector = Vector3.Cross(vector1_lhs, vector2_rhs);
            if (perpVector.y < 0.0f) { perpVector = -perpVector; }
            Vector3 perpVector_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(perpVector);
            bool perpVectorIsTooShortForNormalizing = UtilitiesDXXL_Math.GetBiggestAbsComponent(perpVector_normalized) < 0.001f;
            if (perpVectorIsTooShortForNormalizing) { perpVector_normalized = Vector3.up; }

            //thin short grey line through turnAxisCenter:
            Line_fadeableAnimSpeed.InternalDraw(posWhereToDraw - 0.03f * perpVector_normalized, posWhereToDraw + 0.03f * perpVector_normalized, color_ofVertAxis, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            bool vector1_isTooShortForStableDrawing = UtilitiesDXXL_Math.GetBiggestAbsComponent(vector1_lhs) < 0.00001f;
            bool vector2_isTooShortForStableDrawing = UtilitiesDXXL_Math.GetBiggestAbsComponent(vector2_rhs) < 0.00001f;

            if (vector1_isTooShortForStableDrawing == false)
            {
                planePerpTo_vector1.Recreate(posWhereToDraw, posWhereToDraw + vector1_lhs, posWhereToDraw + perpVector_normalized);
            }

            if (vector2_isTooShortForStableDrawing == false)
            {
                planePerpTo_vector2.Recreate(posWhereToDraw, posWhereToDraw + vector2_rhs, posWhereToDraw + perpVector_normalized);
            }

            float vectorLengthThreshold_belowWhichToUseRelConeLengths = 0.45f;
            if (vector1_isTooShortForStableDrawing == false)
            {
                bool setConeLengthToRelative_notToAbsolute = (vector1_magnitude < vectorLengthThreshold_belowWhichToUseRelConeLengths);
                float coneLength_ifSetToRelative = (0.1f / vectorLengthThreshold_belowWhichToUseRelConeLengths);
                float coneLength_ifSetToAbsolute = 0.1f;
                float coneLength = UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(setConeLengthToRelative_notToAbsolute, coneLength_ifSetToRelative, coneLength_ifSetToAbsolute);
                UtilitiesDXXL_DrawBasics.VectorFrom(posWhereToDraw, vector1_lhs, color_ofVector1, linesWidth, "length(lhs) = " + vector1_magnitude, coneLength, false, false, true, minTextSize_atVectorFrom_dotAndCrossProduct, false, durationInSec, hiddenByNearerObjects, vector1_isTooShortForStableDrawing ? null : planePerpTo_vector1, false, 0.0f);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
            }

            if (vector2_isTooShortForStableDrawing == false)
            {
                bool setConeLengthToRelative_notToAbsolute = (vector2_magnitude < vectorLengthThreshold_belowWhichToUseRelConeLengths);
                float coneLength_ifSetToRelative = (0.1f / vectorLengthThreshold_belowWhichToUseRelConeLengths);
                float coneLength_ifSetToAbsolute = 0.1f;
                float coneLength = UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(setConeLengthToRelative_notToAbsolute, coneLength_ifSetToRelative, coneLength_ifSetToAbsolute);
                UtilitiesDXXL_DrawBasics.VectorFrom(posWhereToDraw, vector2_rhs, color_ofVector2, linesWidth, "length(rhs) = " + vector2_magnitude, coneLength, false, false, true, minTextSize_atVectorFrom_dotAndCrossProduct, false, durationInSec, hiddenByNearerObjects, vector2_isTooShortForStableDrawing ? null : planePerpTo_vector2, false, 0.0f);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
            }

            string text;
            if (vector1_isTooShortForStableDrawing || vector2_isTooShortForStableDrawing || angleIsTooSmallForStableDrawing)
            {
                text = "dot product=<br><color=#" + ColorUtility.ToHtmlStringRGBA(DrawEngineBasics.colorOfVector1_forDotProduct) + ">length(lhs)</color> * <color=#" + ColorUtility.ToHtmlStringRGBA(DrawEngineBasics.colorOfVector2_forDotProduct) + ">length(rhs)</color> * cos(<color=#" + ColorUtility.ToHtmlStringRGBA(DrawEngineBasics.colorOfAngle_forDotProduct) + ">angleBeweenVectors[rad]</color>)=<br><color=#" + ColorUtility.ToHtmlStringRGBA(DrawEngineBasics.colorOfVector1_forDotProduct) + ">" + vector1_magnitude + "</color> * <color=#" + ColorUtility.ToHtmlStringRGBA(DrawEngineBasics.colorOfVector2_forDotProduct) + ">" + vector2_magnitude + "</color> * cos(<color=#" + ColorUtility.ToHtmlStringRGBA(DrawEngineBasics.colorOfAngle_forDotProduct) + ">" + angleRad + "</color>)=<br><color=#" + ColorUtility.ToHtmlStringRGBA(DrawEngineBasics.colorOfVector1_forDotProduct) + ">" + vector1_magnitude + "</color> * <color=#" + ColorUtility.ToHtmlStringRGBA(DrawEngineBasics.colorOfVector2_forDotProduct) + ">" + vector2_magnitude + "</color> * " + Mathf.Cos(angleRad) + "=<br><b>" + Vector3.Dot(vector1_lhs, vector2_rhs) + "</b>";
            }
            else
            {
                UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forCircledVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                UtilitiesDXXL_Measurements.Set_defaultColors_reversible(color_ofAngle);
                DrawMeasurements.AngleSpan(vector1_lhs, vector2_rhs, posWhereToDraw, color_ofAngle, 0.8f, linesWidth, null, false, true, 0.05f, false, true, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_Measurements.Reverse_defaultColors();
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forCircledVectors();
                text = "dot product=<br><color=#" + ColorUtility.ToHtmlStringRGBA(DrawEngineBasics.colorOfVector1_forDotProduct) + ">" + vector1_magnitude + "</color> * <color=#" + ColorUtility.ToHtmlStringRGBA(DrawEngineBasics.colorOfVector2_forDotProduct) + ">" + vector2_magnitude + "</color> * cos(<color=#" + ColorUtility.ToHtmlStringRGBA(DrawEngineBasics.colorOfAngle_forDotProduct) + ">" + angleRad + "</color>)=<br><color=#" + ColorUtility.ToHtmlStringRGBA(DrawEngineBasics.colorOfVector1_forDotProduct) + ">" + vector1_magnitude + "</color> * <color=#" + ColorUtility.ToHtmlStringRGBA(DrawEngineBasics.colorOfVector2_forDotProduct) + ">" + vector2_magnitude + "</color> * " + Mathf.Cos(angleRad) + "=<br><b>" + Vector3.Dot(vector1_lhs, vector2_rhs) + "</b>";
            }

            Vector3 vector1_normalizedOrZero = (UtilitiesDXXL_Math.ApproximatelyZero(vector1_magnitude)) ? Vector3.zero : (vector1_lhs / vector1_magnitude);
            Vector3 vector2_normalizedOrZero = (UtilitiesDXXL_Math.ApproximatelyZero(vector2_magnitude)) ? Vector3.zero : (vector2_rhs / vector2_magnitude);
            Vector3 textDir = vector1_normalizedOrZero + vector2_normalizedOrZero;
            //Vector3 textUp = perpVector_normalized;
            Vector3 textUp = Vector3.up;
            UtilitiesDXXL_Text.WriteFramed(text, posWhereToDraw, DrawEngineBasics.colorOfResult_forDotProduct, 0.03f, textDir, textUp, DrawText.TextAnchorDXXL.LowerRight, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);

            float radius_ofTurnCenterDot = 1.5f * linesWidth;
            radius_ofTurnCenterDot = Mathf.Max(radius_ofTurnCenterDot, 0.0025f);
            DrawShapes.Sphere(posWhereToDraw, radius_ofTurnCenterDot, color_ofAngle, Vector3.up, Vector3.forward, 0.0f, null, 8, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);
        }

        static Color colorOfVector1_forCrossProduct_before;
        public static void Set_colorOfVector1_forCrossProduct_reversible(Color new_colorOfVector1_forCrossProduct)
        {
            colorOfVector1_forCrossProduct_before = DrawEngineBasics.colorOfVector1_forCrossProduct;
            DrawEngineBasics.colorOfVector1_forCrossProduct = new_colorOfVector1_forCrossProduct;
        }
        public static void Reverse_colorOfVector1_forCrossProduct()
        {
            DrawEngineBasics.colorOfVector1_forCrossProduct = colorOfVector1_forCrossProduct_before;
        }

        static Color colorOfVector2_forCrossProduct_before;
        public static void Set_colorOfVector2_forCrossProduct_reversible(Color new_colorOfVector2_forCrossProduct)
        {
            colorOfVector2_forCrossProduct_before = DrawEngineBasics.colorOfVector2_forCrossProduct;
            DrawEngineBasics.colorOfVector2_forCrossProduct = new_colorOfVector2_forCrossProduct;
        }
        public static void Reverse_colorOfVector2_forCrossProduct()
        {
            DrawEngineBasics.colorOfVector2_forCrossProduct = colorOfVector2_forCrossProduct_before;
        }

        static Color colorOfAngle_forCrossProduct_before;
        public static void Set_colorOfAngle_forCrossProduct_reversible(Color new_colorOfAngle_forCrossProduct)
        {
            colorOfAngle_forCrossProduct_before = DrawEngineBasics.colorOfAngle_forCrossProduct;
            DrawEngineBasics.colorOfAngle_forCrossProduct = new_colorOfAngle_forCrossProduct;
        }
        public static void Reverse_colorOfAngle_forCrossProduct()
        {
            DrawEngineBasics.colorOfAngle_forCrossProduct = colorOfAngle_forCrossProduct_before;
        }

        static Color colorOfResultVector_forCrossProduct_before;
        public static void Set_colorOfResultVector_forCrossProduct_reversible(Color new_colorOfResultVector_forCrossProduct)
        {
            colorOfResultVector_forCrossProduct_before = DrawEngineBasics.colorOfResultVector_forCrossProduct;
            DrawEngineBasics.colorOfResultVector_forCrossProduct = new_colorOfResultVector_forCrossProduct;
        }
        public static void Reverse_colorOfResultVector_forCrossProduct()
        {
            DrawEngineBasics.colorOfResultVector_forCrossProduct = colorOfResultVector_forCrossProduct_before;
        }

        static Color colorOfResultText_forCrossProduct_before;
        public static void Set_colorOfResultText_forCrossProduct_reversible(Color new_colorOfResultText_forCrossProduct)
        {
            colorOfResultText_forCrossProduct_before = DrawEngineBasics.colorOfResultText_forCrossProduct;
            DrawEngineBasics.colorOfResultText_forCrossProduct = new_colorOfResultText_forCrossProduct;
        }
        public static void Reverse_colorOfResultText_forCrossProduct()
        {
            DrawEngineBasics.colorOfResultText_forCrossProduct = colorOfResultText_forCrossProduct_before;
        }

        static Color colorOfResultPlane_forCrossProduct_before;
        public static void Set_colorOfResultPlane_forCrossProduct_reversible(Color new_colorOfResultPlane_forCrossProduct)
        {
            colorOfResultPlane_forCrossProduct_before = DrawEngineBasics.colorOfResultPlane_forCrossProduct;
            DrawEngineBasics.colorOfResultPlane_forCrossProduct = new_colorOfResultPlane_forCrossProduct;
        }
        public static void Reverse_colorOfResultPlane_forCrossProduct()
        {
            DrawEngineBasics.colorOfResultPlane_forCrossProduct = colorOfResultPlane_forCrossProduct_before;
        }

        static Color overwriteColorForFrustumsHighlightedPlane_before;
        public static void Set_overwriteColorForFrustumsHighlightedPlane_reversible(Color new_overwriteColorForFrustumsHighlightedPlane)
        {
            overwriteColorForFrustumsHighlightedPlane_before = DrawEngineBasics.overwriteColorForFrustumsHighlightedPlane;
            DrawEngineBasics.overwriteColorForFrustumsHighlightedPlane = new_overwriteColorForFrustumsHighlightedPlane;
        }
        public static void Reverse_overwriteColorForFrustumsHighlightedPlane()
        {
            DrawEngineBasics.overwriteColorForFrustumsHighlightedPlane = overwriteColorForFrustumsHighlightedPlane_before;
        }

        static float distanceOfFrustumsHighlightedPlane_before;
        public static void Set_distanceOfFrustumsHighlightedPlane_reversible(float new_distanceOfFrustumsHighlightedPlane)
        {
            distanceOfFrustumsHighlightedPlane_before = DrawEngineBasics.distanceOfFrustumsHighlightedPlane;
            DrawEngineBasics.distanceOfFrustumsHighlightedPlane = new_distanceOfFrustumsHighlightedPlane;
        }
        public static void Reverse_distanceOfFrustumsHighlightedPlane()
        {
            DrawEngineBasics.distanceOfFrustumsHighlightedPlane = distanceOfFrustumsHighlightedPlane_before;
        }

        static bool drawFrustumsHighlightedPlaneAlsoIfFarerThanFarClipPlane_before;
        public static void Set_drawFrustumsHighlightedPlaneAlsoIfFarerThanFarClipPlane_reversible(bool new_drawFrustumsHighlightedPlaneAlsoIfFarerThanFarClipPlane)
        {
            drawFrustumsHighlightedPlaneAlsoIfFarerThanFarClipPlane_before = DrawEngineBasics.drawFrustumsHighlightedPlaneAlsoIfFarerThanFarClipPlane;
            DrawEngineBasics.drawFrustumsHighlightedPlaneAlsoIfFarerThanFarClipPlane = new_drawFrustumsHighlightedPlaneAlsoIfFarerThanFarClipPlane;
        }
        public static void Reverse_drawFrustumsHighlightedPlaneAlsoIfFarerThanFarClipPlane()
        {
            DrawEngineBasics.drawFrustumsHighlightedPlaneAlsoIfFarerThanFarClipPlane = drawFrustumsHighlightedPlaneAlsoIfFarerThanFarClipPlane_before;
        }

        public static void CrossProduct(Vector3 vector1_lhs_leftThumb, Vector3 vector2_rhs_leftIndexFinger, Vector3 posWhereToDraw = default(Vector3), float linesWidth = 0.0025f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            //"lhs" = "left hand side (of equation)"
            //"rhs" = "right hand side (of equation)"
            //cross product result is "left middle finger" according to the left hand rule

            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vector1_lhs_leftThumb, "vector1_lhs_leftThumb")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(vector2_rhs_leftIndexFinger, "vector2_rhs_leftIndexFinger")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(posWhereToDraw, "posWhereToDraw")) { return; }

            linesWidth = UtilitiesDXXL_Math.AbsNonZeroValue(linesWidth);

            float vector1_magnitude = vector1_lhs_leftThumb.magnitude;
            float vector2_magnitude = vector2_rhs_leftIndexFinger.magnitude;

            Vector3 vector1_scaledIntoRegionOfFloatPrecision = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(vector1_lhs_leftThumb);
            Vector3 vector2_scaledIntoRegionOfFloatPrecision = UtilitiesDXXL_Math.ScaleNonZeroVectorIntoRegionOfFloatPrecision(vector2_rhs_leftIndexFinger);

            float angleDeg = Vector3.Angle(vector1_scaledIntoRegionOfFloatPrecision, vector2_scaledIntoRegionOfFloatPrecision);
            bool angleIsTooSmallForStableDrawing = angleDeg < 0.1f;

            Color color_ofVector1 = DrawEngineBasics.colorOfVector1_forCrossProduct;
            Color color_ofVector2 = DrawEngineBasics.colorOfVector2_forCrossProduct;
            Color color_ofAngle = DrawEngineBasics.colorOfAngle_forCrossProduct;
            Color color_ofCrossProduct = DrawEngineBasics.colorOfResultVector_forCrossProduct;

            Vector3 crossProduct = Vector3.Cross(vector1_lhs_leftThumb, vector2_rhs_leftIndexFinger);
            if (UtilitiesDXXL_Math.ApproximatelyZero(crossProduct) == false)
            {
                DrawShapes.Rhombus(posWhereToDraw, vector1_lhs_leftThumb, vector2_rhs_leftIndexFinger, DrawEngineBasics.colorOfResultPlane_forCrossProduct, 0.0f, null, 10, DrawBasics.LineStyle.solid, 1.0f, durationInSec, hiddenByNearerObjects);
            }

            Vector3 perpVector = crossProduct;

            if (perpVector.y < 0.0f) { perpVector = -perpVector; }
            Vector3 perpVector_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(perpVector);
            bool perpVectorIsTooShortForNormalizing = UtilitiesDXXL_Math.GetBiggestAbsComponent(perpVector_normalized) < 0.001f;
            if (perpVectorIsTooShortForNormalizing) { perpVector_normalized = Vector3.up; }

            bool vector1_isTooShortForStableDrawing = UtilitiesDXXL_Math.GetBiggestAbsComponent(vector1_lhs_leftThumb) < 0.00001f;
            bool vector2_isTooShortForStableDrawing = UtilitiesDXXL_Math.GetBiggestAbsComponent(vector2_rhs_leftIndexFinger) < 0.00001f;

            if (vector1_isTooShortForStableDrawing == false)
            {
                planePerpTo_vector1.Recreate(posWhereToDraw, posWhereToDraw + vector1_lhs_leftThumb, posWhereToDraw + perpVector_normalized);
            }

            if (vector2_isTooShortForStableDrawing == false)
            {
                planePerpTo_vector2.Recreate(posWhereToDraw, posWhereToDraw + vector2_rhs_leftIndexFinger, posWhereToDraw + perpVector_normalized);
            }

            float vectorLenghtThreshold_belowWhichToUseRelConeLengths = 0.45f;
            if (vector1_isTooShortForStableDrawing == false)
            {
                bool setConeLengthToRelative_notToAbsolute = (vector1_magnitude < vectorLenghtThreshold_belowWhichToUseRelConeLengths);
                float coneLength_ifSetToRelative = 0.1f / vectorLenghtThreshold_belowWhichToUseRelConeLengths;
                float coneLength_ifSetToAbsolute = 0.1f;
                float coneLength = UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(setConeLengthToRelative_notToAbsolute, coneLength_ifSetToRelative, coneLength_ifSetToAbsolute);
                UtilitiesDXXL_DrawBasics.VectorFrom(posWhereToDraw, vector1_lhs_leftThumb, color_ofVector1, linesWidth, "length (lhs[=<size=3>left</size><icon=thumbUp>]) = " + vector1_magnitude, coneLength, false, false, true, minTextSize_atVectorFrom_dotAndCrossProduct, false, durationInSec, hiddenByNearerObjects, vector1_isTooShortForStableDrawing ? null : planePerpTo_vector1, false, 0.0f);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
            }

            if (vector2_isTooShortForStableDrawing == false)
            {
                bool setConeLengthToRelative_notToAbsolute = (vector2_magnitude < vectorLenghtThreshold_belowWhichToUseRelConeLengths);
                float coneLength_ifSetToRelative = 0.1f / vectorLenghtThreshold_belowWhichToUseRelConeLengths;
                float coneLength_ifSetToAbsolute = 0.1f;
                float coneLength = UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(setConeLengthToRelative_notToAbsolute, coneLength_ifSetToRelative, coneLength_ifSetToAbsolute);
                UtilitiesDXXL_DrawBasics.VectorFrom(posWhereToDraw, vector2_rhs_leftIndexFinger, color_ofVector2, linesWidth, "length (rhs[=<size=3>left</size><icon=cursorHand>]) = " + vector2_magnitude, coneLength, false, false, true, minTextSize_atVectorFrom_dotAndCrossProduct, false, durationInSec, hiddenByNearerObjects, vector2_isTooShortForStableDrawing ? null : planePerpTo_vector2, false, 0.0f);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
            }

            float crossProduct_magnitude = crossProduct.magnitude;
            string text = "<color=#" + ColorUtility.ToHtmlStringRGBA(DrawEngineBasics.colorOfResultText_forCrossProduct) + "><color=#" + ColorUtility.ToHtmlStringRGBA(DrawEngineBasics.colorOfResultVector_forCrossProduct) + "><b>cross product</b>=<br><size=7> </size><br><size=6><size=4>Vector3</size><sw=60000>(" + crossProduct.x + " , " + crossProduct.y + " , " + crossProduct.z + ")</sw></color><br>direction:<size=36><icon=leftHandRule></size>left middle finger<size=5> (left-hand rule)</size><br>length (= <color=#" + ColorUtility.ToHtmlStringRGBA(DrawEngineBasics.colorOfResultPlane_forCrossProduct) + ">area</color>) = " + crossProduct_magnitude + "</size></color>";
            if (crossProduct_magnitude < 0.002f)
            {
                UtilitiesDXXL_Text.WriteFramed(text, posWhereToDraw, color_ofCrossProduct, 0.03f, crossProduct, -(vector1_lhs_leftThumb + vector2_rhs_leftIndexFinger), DrawText.TextAnchorDXXL.LowerLeft, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                bool setConeLengthToRelative_notToAbsolute = (crossProduct_magnitude < vectorLenghtThreshold_belowWhichToUseRelConeLengths);
                float coneLength_ifSetToRelative = 0.1f / vectorLenghtThreshold_belowWhichToUseRelConeLengths;
                float coneLength_ifSetToAbsolute = 0.1f;
                float coneLength = UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(setConeLengthToRelative_notToAbsolute, coneLength_ifSetToRelative, coneLength_ifSetToAbsolute);
                UtilitiesDXXL_DrawBasics.VectorFrom(posWhereToDraw, crossProduct, color_ofCrossProduct, linesWidth, text, coneLength, false, false, true, 0.025f, false, durationInSec, hiddenByNearerObjects, null, false, 0.0f);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
            }

            if (vector1_isTooShortForStableDrawing == false && vector2_isTooShortForStableDrawing == false && angleIsTooSmallForStableDrawing == false)
            {
                UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forCircledVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                UtilitiesDXXL_Measurements.Set_defaultColors_reversible(color_ofAngle);
                DrawMeasurements.AngleSpan(vector1_lhs_leftThumb, vector2_rhs_leftIndexFinger, posWhereToDraw, color_ofAngle, 0.8f, linesWidth, null, false, true, 0.05f, false, true, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_Measurements.Reverse_defaultColors();
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forCircledVectors();
            }

            float radius_ofTurnCenterDot = 1.5f * linesWidth;
            radius_ofTurnCenterDot = Mathf.Max(radius_ofTurnCenterDot, 0.0025f);
            DrawShapes.Sphere(posWhereToDraw, radius_ofTurnCenterDot, color_ofAngle, Vector3.up, Vector3.forward, 0.0f, null, 8, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);
        }


        static InternalDXXL_Line rayline = new InternalDXXL_Line();
        public static void RayLineExtended(bool is2D, Vector3 rayOrigin, Vector3 rayDirection, Color color = default(Color), float width = 0.0f, string text = null, float forceFixedConeLength = 0.0f, bool addNormalizedMarkingText = true, float enlargeSmallTextToThisMinTextSize = 0.005f, float extentionLength = 1000.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width, "width")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(extentionLength, "extentionLength")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(rayOrigin, "rayOrigin")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(rayDirection, "rayDirection")) { return; }

            width = UtilitiesDXXL_Math.AbsNonZeroValue(width);
            if (UtilitiesDXXL_Math.ApproximatelyZero(rayDirection))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(rayOrigin, "[<color=#adadadFF><icon=logMessage></color> RayLineExtended with length of 0]<br>" + text, color, width, durationInSec, hiddenByNearerObjects);
                return;
            }
            width = Mathf.Max(width, 0.006f);

            Color colorOfProlongedLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.55f);

            rayline.Recreate(rayOrigin, rayDirection, false);
            if (rayline.originHasBeenRelocated)
            {
                text = "[<color=#adadadFF><icon=logMessage></color> RayOrigin (" + UtilitiesDXXL_Log.Get_vectorComponentsAsString(rayOrigin) + ") was too far off<br>(float world positions get uncertain in this high region)<br>-> auto-relocate to " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(rayline.origin) + "<br>to prevent undefined behaviour]<br>" + text;
            }

            bool setConeLengthToRelative_notToAbsolute = UtilitiesDXXL_Math.ApproximatelyZero(forceFixedConeLength);
            float coneLength_ifSetToRelative = 0.17f;
            float coneLength_ifSetToAbsolute = forceFixedConeLength;
            float coneLength = UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(setConeLengthToRelative_notToAbsolute, coneLength_ifSetToRelative, coneLength_ifSetToAbsolute);
            if (is2D)
            {
                DrawBasics2D.VectorFrom(rayline.origin, rayline.direction, color, width, text, coneLength, false, rayline.origin.z, addNormalizedMarkingText, enlargeSmallTextToThisMinTextSize, false, 0.0f, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                DrawBasics.VectorFrom(rayline.origin, rayline.direction, color, width, text, coneLength, false, false, default(Vector3), addNormalizedMarkingText, enlargeSmallTextToThisMinTextSize, false, 0.0f, durationInSec, hiddenByNearerObjects);
            }
            UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();

            Vector3 extentionVector = rayline.direction_normalized * extentionLength;
            Line_fadeableAnimSpeed.InternalDraw(rayline.origin - extentionVector, rayline.origin + extentionVector, colorOfProlongedLine, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, enlargeSmallTextToThisMinTextSize, durationInSec, hiddenByNearerObjects, false, false);
            float originSphere_radius = Mathf.Min(1.1f * width, 0.1f * rayline.length);
            if (is2D)
            {
                DrawShapes.Circle(rayOrigin, originSphere_radius, color, Vector3.forward, Vector3.up, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, true, false, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                DrawShapes.Sphere(rayOrigin, originSphere_radius, color, rayline.direction_normalized, default, 0.0f, null, 8, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);
            }

        }


        static InternalDXXL_Line2D rayLineViewportSpace = new InternalDXXL_Line2D();
        public static void RayLineExtendedScreenspace(Camera camera, Vector2 rayOrigin, Vector2 rayDirection, Color color = default(Color), float width_relToViewportHeight = 0.0f, string text = null, bool interpretDirectionAsUnwarped = false, float coneLength_relToViewportHeight = 0.05f, bool displayDistanceOutsideScreenBorder = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(camera, "camera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(camera)) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width_relToViewportHeight, "width_relToViewportHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(coneLength_relToViewportHeight, "coneLength_relToViewportHeight")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(rayOrigin, "rayOrigin")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(rayDirection, "rayDirection")) { return; }

            width_relToViewportHeight = UtilitiesDXXL_Math.AbsNonZeroValue(width_relToViewportHeight);

            if (UtilitiesDXXL_Math.ApproximatelyZero(rayDirection))
            {
                UtilitiesDXXL_Screenspace.PointFallback(camera, InternalDXXL_BoundsCamViewportSpace.ClampIntoViewport(rayOrigin), "[<color=#adadadFF><icon=logMessage></color> RayLineExtendedScreenspace with length of 0]<br>" + text, color, width_relToViewportHeight, durationInSec);
                return;
            }

            float minWidth_relTViewportHeight = 0.003f;
            width_relToViewportHeight = Mathf.Max(width_relToViewportHeight, minWidth_relTViewportHeight);

            Color colorOfProlongedLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.55f);

            Vector2 rayDirection_inNonSquareViewportSpace = interpretDirectionAsUnwarped ? DrawScreenspace.DirectionInUnitsOfUnwarpedSpace_to_sameLookingDirectionInUnitsOfWarpedSpace(rayDirection, camera) : rayDirection;
            DrawScreenspace.VectorFrom(camera, rayOrigin, rayDirection_inNonSquareViewportSpace, color, width_relToViewportHeight, text, false, coneLength_relToViewportHeight, false, false, 0.0f, durationInSec);

            Vector2 rayPeak_inNonSquareViewportSpace = rayOrigin + rayDirection_inNonSquareViewportSpace;
            if (InternalDXXL_BoundsCamViewportSpace.IsInsideViewportExclBorder(rayOrigin) == false && InternalDXXL_BoundsCamViewportSpace.IsInsideViewportExclBorder(rayPeak_inNonSquareViewportSpace) == false)
            {
                rayLineViewportSpace.Recalc_line_throughTwoPoints_returnSteepForVertLines(rayOrigin, rayPeak_inNonSquareViewportSpace);
                Vector2 viewportCenterProjectionOntoLine_inNonSquareViewportSpace = rayLineViewportSpace.GetProjectionOfPointOntoLine(InternalDXXL_BoundsCamViewportSpace.viewportCenter);
                Vector2 nearestViewportCorner = InternalDXXL_BoundsCamViewportSpace.wholeViewportAsBounds.GetNearestCorner(viewportCenterProjectionOntoLine_inNonSquareViewportSpace);
                nearestViewportCorner.y = UtilitiesDXXL_Math.ApproximatelyZero(rayDirection.x) ? 0.0f : nearestViewportCorner.y; //->prevents flickering
                Vector2 projectionOfNearestViewportCornerOntoLine_inNonSquareViewportSpace = rayLineViewportSpace.GetProjectionOfPointOntoLine(nearestViewportCorner);
                projectionOfNearestViewportCornerOntoLine_inNonSquareViewportSpace.y = UtilitiesDXXL_Math.ApproximatelyZero(rayDirection.x) ? 0.0f : projectionOfNearestViewportCornerOntoLine_inNonSquareViewportSpace.y; //->prevents flickering
                bool preventFlickerOfHorizLinesThroughViewport = (Mathf.Abs(rayDirection.y) < 0.0001f) && (projectionOfNearestViewportCornerOntoLine_inNonSquareViewportSpace.y > 0.0f && projectionOfNearestViewportCornerOntoLine_inNonSquareViewportSpace.y < 1.0f);
                bool isVertLineThroughViewport = UtilitiesDXXL_Math.ApproximatelyZero(rayDirection.x) && (projectionOfNearestViewportCornerOntoLine_inNonSquareViewportSpace.x > 0.0f && projectionOfNearestViewportCornerOntoLine_inNonSquareViewportSpace.x < 1.0f);
                if (isVertLineThroughViewport || preventFlickerOfHorizLinesThroughViewport || InternalDXXL_BoundsCamViewportSpace.IsInsideViewportExclBorder(projectionOfNearestViewportCornerOntoLine_inNonSquareViewportSpace))
                {
                    if (text != null && text != "")
                    {
                        DrawText.TextAnchorDXXL textAnchor = (nearestViewportCorner.y < 0.5f) ? DrawText.TextAnchorDXXL.LowerCenter : DrawText.TextAnchorDXXL.UpperCenter;
                        float textSize_relToViewportHeight = 0.02f;
                        float half_textSize_relToViewportHeight = 0.5f * textSize_relToViewportHeight;
                        Vector2 textPos = new Vector2(Mathf.Clamp(projectionOfNearestViewportCornerOntoLine_inNonSquareViewportSpace.x, half_textSize_relToViewportHeight, 1.0f - half_textSize_relToViewportHeight), Mathf.Clamp(projectionOfNearestViewportCornerOntoLine_inNonSquareViewportSpace.y, half_textSize_relToViewportHeight, 1.0f - half_textSize_relToViewportHeight)); //clamping due to: prevent flickering of "autoLineBreakWidth_relToViewportWidth"
                        Vector2 rayDirection_inAspectCorrected1by1SquareViewportSpace = DrawScreenspace.DirectionInUnitsOfWarpedSpace_to_sameLookingDirectionInUnitsOfUnwarpedSpace(rayDirection_inNonSquareViewportSpace, camera);
                        UtilitiesDXXL_Text.WriteScreenSpace(camera, text, textPos, color, textSize_relToViewportHeight, rayDirection_inAspectCorrected1by1SquareViewportSpace, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, true, 0.0f, true, durationInSec, false);
                    }
                }
                else
                {
                    DrawScreenspace.PointTag(camera, projectionOfNearestViewportCornerOntoLine_inNonSquareViewportSpace, text, null, color, true, 0.0f, 0.2f, default(Vector2), 1.0f, false, displayDistanceOutsideScreenBorder, durationInSec, default(Vector2));
                }
            }

            float extentionLength = 1.0f + (rayOrigin - InternalDXXL_BoundsCamViewportSpace.viewportCenter).magnitude;
            Vector2 rayDirection_inNonSquareViewportSpace_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(rayDirection_inNonSquareViewportSpace);
            Vector2 extentionVector = rayDirection_inNonSquareViewportSpace_normalized * extentionLength;
            Line_fadeableAnimSpeed_screenspace.InternalDraw(camera, rayOrigin - extentionVector, rayOrigin + extentionVector, colorOfProlongedLine, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, 0.0f, 0.0f, DrawScreenspace.minTextSize_relToViewportHeight, durationInSec);

            float originDot_size_relToViewportHeight = 1.6f * width_relToViewportHeight;
            originDot_size_relToViewportHeight = Mathf.Max(originDot_size_relToViewportHeight, 2.6f * minWidth_relTViewportHeight);
            float originDot_size_relToViewportHeight_075 = 0.75f * originDot_size_relToViewportHeight;
            float originDot_size_relToViewportHeight_05 = 0.5f * originDot_size_relToViewportHeight;
            DrawScreenspace.Shape(camera, rayOrigin, DrawShapes.Shape2DType.circle, color, originDot_size_relToViewportHeight_075, originDot_size_relToViewportHeight_075, 0.0f, originDot_size_relToViewportHeight_05, null, false, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, true, durationInSec);
        }

        public static void Camera(Vector3 position, Vector3 forward, Vector3 up, bool isOrthographic, float orthographicSize, float fieldOfView, float nearClipPlane, float aspect, Color color, string text, float linesWidth, float durationInSec, bool hiddenByNearerObjects)
        {
            //"fieldOfView" can be vertical or horizontal. UnityEngine.Camera.fieldOfView" always returns the vertical fieldOfView, also if the camera inspector component has set the "FOV Axis"-dropdown to "horizontal"

            color = UtilitiesDXXL_Colors.OverwriteDefaultColor(color);

            float tanOfHalfFieldOfView = Mathf.Tan(0.5f * fieldOfView * Mathf.Deg2Rad);
            float heightOfCamsNearPlane;
            Vector3 offsetForOrthograficCams = Vector3.zero;
            if (isOrthographic)
            {
                heightOfCamsNearPlane = 2.0f * orthographicSize;
            }
            else
            {
                heightOfCamsNearPlane = 2.0f * nearClipPlane * tanOfHalfFieldOfView;
            }

            float heightOfDrawnFrustumsNearPlane = 0.5f * heightOfCamsNearPlane;
            float widthOfCamsNearPlane = heightOfCamsNearPlane * aspect;
            float widthOfDrawnFrustumsNearPlane = 0.5f * widthOfCamsNearPlane;
            float distanceToNearClipPlaneOfCamFrustum = 0.5f * heightOfCamsNearPlane / tanOfHalfFieldOfView;
            float distanceToNearClipPlaneOfDrawnFrustum = 0.5f * distanceToNearClipPlaneOfCamFrustum;

            if (isOrthographic)
            {
                offsetForOrthograficCams = -forward * (distanceToNearClipPlaneOfCamFrustum - nearClipPlane);
            }

            DrawShapes.Frustum(position + offsetForOrthograficCams, forward, up, fieldOfView, aspect, distanceToNearClipPlaneOfDrawnFrustum, distanceToNearClipPlaneOfCamFrustum, color, DrawShapes.Shape2DType.square, linesWidth, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
            if (isOrthographic == false)
            {
                float angleDeg_horiz = 2.0f * Mathf.Rad2Deg * Mathf.Atan(0.5f * (widthOfCamsNearPlane / nearClipPlane));
                DrawShapes.Pyramid(position + offsetForOrthograficCams, distanceToNearClipPlaneOfDrawnFrustum, forward, up, fieldOfView, angleDeg_horiz, UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.25f), DrawShapes.Shape2DType.square, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
            }

            float cubeHeight = 1.2f * heightOfDrawnFrustumsNearPlane;
            float cubeWidth = 1.2f * widthOfDrawnFrustumsNearPlane;
            DrawShapes.Cylinder(position - forward * 0.0f * distanceToNearClipPlaneOfCamFrustum + offsetForOrthograficCams, 1.0f * distanceToNearClipPlaneOfCamFrustum, cubeWidth, cubeHeight, color, forward, up, DrawShapes.Shape2DType.square, linesWidth, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);

            Vector3 cameraLeftNormalized = Vector3.Cross(forward, up);
            Vector3 cameraRightNormalized = -cameraLeftNormalized;
            float cylSize = 0.9f * distanceToNearClipPlaneOfDrawnFrustum;
            Vector3 cyl1_position = position + up * (0.5f * cubeHeight + 0.5f * cylSize) + forward * (0.25f * cylSize);

            DrawShapes.Cylinder(cyl1_position + offsetForOrthograficCams, 0.25f * cubeWidth, cylSize, cylSize, color, cameraLeftNormalized, up, DrawShapes.Shape2DType.circle, linesWidth, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
            DrawShapes.Cylinder(cyl1_position - forward * cylSize + offsetForOrthograficCams, 0.25f * cubeWidth, cylSize, cylSize, color, cameraLeftNormalized, up, DrawShapes.Shape2DType.circle, linesWidth, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);

            if (text != null && text != "")
            {
                float lineHeight = 0.08f * heightOfCamsNearPlane;
                Vector3 centerOfCamsNearPlane = position + forward * nearClipPlane;
                Vector3 topLeftCorner_ofCamsNearPlane = centerOfCamsNearPlane + cameraLeftNormalized * (0.5f * widthOfCamsNearPlane) + up * (0.5f * heightOfCamsNearPlane);
                Vector3 textPosition = topLeftCorner_ofCamsNearPlane + (0.02f * widthOfCamsNearPlane) * cameraRightNormalized - (1.7f * lineHeight) * up;
                UtilitiesDXXL_Text.WriteFramed(text, textPosition + forward * 0.0001f, color, lineHeight, cameraRightNormalized, up, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.96f * widthOfCamsNearPlane, 0.0f, false, durationInSec, false);
            }
        }

        static InternalDXXL_Plane frustums_farPlane = new InternalDXXL_Plane();
        static InternalDXXL_Plane frustums_highightedPlane = new InternalDXXL_Plane();
        static InternalDXXL_Plane camPlane_throughCamPos = new InternalDXXL_Plane();
        static InternalDXXL_Line frustums_lowLeftEdge = new InternalDXXL_Line();
        static InternalDXXL_Line frustums_topLeftEdge = new InternalDXXL_Line();
        static InternalDXXL_Line frustums_lowRightEdge = new InternalDXXL_Line();
        static InternalDXXL_Line frustums_topRightEdge = new InternalDXXL_Line();
        public static void CameraFrustum(Vector3 position, Vector3 forward, Vector3 up, bool isOrthographic, float orthographicSize, float fieldOfView, float nearClipPlane, float farClipPlane, float aspect, Color color, string text, bool forceTextOnNearPlaneUnmirroredTowardsCam, float linesWidth_ofEdges, float alphaFactor_forBoundarySurfaceLines, int linesPerBoundarySurface, Vector3 positionOnHighlightedPlane, float durationInSec, bool hiddenByNearerObjects)
        {
            //"fieldOfView" can be vertical or horizontal. UnityEngine.Camera.fieldOfView" always returns the vertical fieldOfView, also if the camera inspector component has set the "FOV Axis"-dropdown to "horizontal"

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth_ofEdges, "linesWidth_ofEdges")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(alphaFactor_forBoundarySurfaceLines, "alphaFactor_forBoundarySurfaceLines")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(positionOnHighlightedPlane, "positionOnHighlightedPlane")) { return; }

            Color color_ofEdgeLines = UtilitiesDXXL_Colors.OverwriteDefaultColor(color);
            Color color_ofBoundarySurfaceLines = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofEdgeLines, alphaFactor_forBoundarySurfaceLines);

            Vector3 forward_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(forward);
            Vector3 up_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(up);
            Vector3 right_normalized = Vector3.Cross(up_normalized, forward_normalized);

            Vector3 center_ofNearPlane = position + forward_normalized * nearClipPlane;
            Vector3 center_ofFarPlane = position + forward_normalized * farClipPlane;

            float tanOfHalfFieldOfView = Mathf.Tan(0.5f * fieldOfView * Mathf.Deg2Rad);
            float heightOfCamsNearPlane;
            if (isOrthographic)
            {
                heightOfCamsNearPlane = 2.0f * orthographicSize;
            }
            else
            {
                heightOfCamsNearPlane = 2.0f * nearClipPlane * tanOfHalfFieldOfView;
            }
            float widthOfCamsNearPlane = heightOfCamsNearPlane * aspect;

            float half_heightOfCamsNearPlane = 0.5f * heightOfCamsNearPlane;
            float half_widthOfCamsNearPlane = 0.5f * widthOfCamsNearPlane;

            Vector3 nearPlanes_lowLeftVertex = center_ofNearPlane - up_normalized * half_heightOfCamsNearPlane - right_normalized * half_widthOfCamsNearPlane;
            Vector3 nearPlanes_topLeftVertex = center_ofNearPlane + up_normalized * half_heightOfCamsNearPlane - right_normalized * half_widthOfCamsNearPlane;
            Vector3 nearPlanes_lowRightVertex = center_ofNearPlane - up_normalized * half_heightOfCamsNearPlane + right_normalized * half_widthOfCamsNearPlane;
            Vector3 nearPlanes_topRightVertex = center_ofNearPlane + up_normalized * half_heightOfCamsNearPlane + right_normalized * half_widthOfCamsNearPlane;

            if (isOrthographic)
            {
                frustums_lowLeftEdge.Recreate(nearPlanes_lowLeftVertex, forward_normalized, true);
                frustums_topLeftEdge.Recreate(nearPlanes_topLeftVertex, forward_normalized, true);
                frustums_lowRightEdge.Recreate(nearPlanes_lowRightVertex, forward_normalized, true);
                frustums_topRightEdge.Recreate(nearPlanes_topRightVertex, forward_normalized, true);
            }
            else
            {
                frustums_lowLeftEdge.Recreate(position, nearPlanes_lowLeftVertex - position, false);
                frustums_topLeftEdge.Recreate(position, nearPlanes_topLeftVertex - position, false);
                frustums_lowRightEdge.Recreate(position, nearPlanes_lowRightVertex - position, false);
                frustums_topRightEdge.Recreate(position, nearPlanes_topRightVertex - position, false);
            }

            frustums_farPlane.Recreate(center_ofFarPlane, forward);
            Vector3 farPlanes_lowLeftVertex = frustums_farPlane.GetIntersectionWithLine(frustums_lowLeftEdge);
            Vector3 farPlanes_topLeftVertex = frustums_farPlane.GetIntersectionWithLine(frustums_topLeftEdge);
            Vector3 farPlanes_lowRightVertex = frustums_farPlane.GetIntersectionWithLine(frustums_lowRightEdge);
            Vector3 farPlanes_topRightVertex = frustums_farPlane.GetIntersectionWithLine(frustums_topRightEdge);

            Line_fadeableAnimSpeed.InternalDraw(nearPlanes_lowLeftVertex, nearPlanes_topLeftVertex, color_ofEdgeLines, linesWidth_ofEdges, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(nearPlanes_topLeftVertex, nearPlanes_topRightVertex, color_ofEdgeLines, linesWidth_ofEdges, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(nearPlanes_topRightVertex, nearPlanes_lowRightVertex, color_ofEdgeLines, linesWidth_ofEdges, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(nearPlanes_lowRightVertex, nearPlanes_lowLeftVertex, color_ofEdgeLines, linesWidth_ofEdges, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            Line_fadeableAnimSpeed.InternalDraw(farPlanes_lowLeftVertex, farPlanes_topLeftVertex, color_ofEdgeLines, linesWidth_ofEdges, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(farPlanes_topLeftVertex, farPlanes_topRightVertex, color_ofEdgeLines, linesWidth_ofEdges, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(farPlanes_topRightVertex, farPlanes_lowRightVertex, color_ofEdgeLines, linesWidth_ofEdges, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(farPlanes_lowRightVertex, farPlanes_lowLeftVertex, color_ofEdgeLines, linesWidth_ofEdges, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            Line_fadeableAnimSpeed.InternalDraw(nearPlanes_lowLeftVertex, farPlanes_lowLeftVertex, color_ofEdgeLines, linesWidth_ofEdges, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(nearPlanes_topLeftVertex, farPlanes_topLeftVertex, color_ofEdgeLines, linesWidth_ofEdges, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(nearPlanes_lowRightVertex, farPlanes_lowRightVertex, color_ofEdgeLines, linesWidth_ofEdges, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(nearPlanes_topRightVertex, farPlanes_topRightVertex, color_ofEdgeLines, linesWidth_ofEdges, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            DrawFrustumsBoundarySurfaces(nearPlanes_lowLeftVertex, nearPlanes_topLeftVertex, farPlanes_lowLeftVertex, farPlanes_topLeftVertex, color_ofBoundarySurfaceLines, linesPerBoundarySurface, durationInSec, hiddenByNearerObjects);
            DrawFrustumsBoundarySurfaces(nearPlanes_topLeftVertex, nearPlanes_topRightVertex, farPlanes_topLeftVertex, farPlanes_topRightVertex, color_ofBoundarySurfaceLines, linesPerBoundarySurface, durationInSec, hiddenByNearerObjects);
            DrawFrustumsBoundarySurfaces(nearPlanes_topRightVertex, nearPlanes_lowRightVertex, farPlanes_topRightVertex, farPlanes_lowRightVertex, color_ofBoundarySurfaceLines, linesPerBoundarySurface, durationInSec, hiddenByNearerObjects);
            DrawFrustumsBoundarySurfaces(nearPlanes_lowRightVertex, nearPlanes_lowLeftVertex, farPlanes_lowRightVertex, farPlanes_lowLeftVertex, color_ofBoundarySurfaceLines, linesPerBoundarySurface, durationInSec, hiddenByNearerObjects);

            TryDrawHighlightedPlane(position, forward_normalized, positionOnHighlightedPlane, nearClipPlane, farClipPlane, color, durationInSec, hiddenByNearerObjects);

            if (text != null && text != "")
            {
                DrawTextAtCameraFrustum(position, forward_normalized, nearClipPlane, farClipPlane, widthOfCamsNearPlane, text, forceTextOnNearPlaneUnmirroredTowardsCam, color_ofEdgeLines, up_normalized, right_normalized, farPlanes_lowRightVertex, farPlanes_lowLeftVertex, durationInSec, hiddenByNearerObjects);
            }
        }

        static void TryDrawHighlightedPlane(Vector3 position, Vector3 forward_normalized, Vector3 positionOnHighlightedPlane, float nearClipPlane, float farClipPlane, Color colorOfFrustumItself, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.IsDefaultVector(positionOnHighlightedPlane))
            {
                if (DrawEngineBasics.distanceOfFrustumsHighlightedPlane >= nearClipPlane)
                {
                    DrawHighlightedPlane(position, forward_normalized, DrawEngineBasics.distanceOfFrustumsHighlightedPlane, farClipPlane, colorOfFrustumItself, durationInSec, hiddenByNearerObjects);
                }
            }
            else
            {
                Vector3 cam_to_highlightedPlaneAnchor = positionOnHighlightedPlane - position;
                if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(cam_to_highlightedPlaneAnchor, forward_normalized))
                {
                    camPlane_throughCamPos.Recreate(position, forward_normalized);
                    Vector3 positionOnHighlightedPlane_projectedOntoCamPlaneThroughCamPos = camPlane_throughCamPos.Get_perpProjectionOfPointOnPlane(positionOnHighlightedPlane);
                    float perpDistance_fromHighlightedPlanePos_toCamPlaneThroughCamPos = (positionOnHighlightedPlane - positionOnHighlightedPlane_projectedOntoCamPlaneThroughCamPos).magnitude;
                    if (perpDistance_fromHighlightedPlanePos_toCamPlaneThroughCamPos >= nearClipPlane)
                    {
                        DrawHighlightedPlane(position, forward_normalized, perpDistance_fromHighlightedPlanePos_toCamPlaneThroughCamPos, farClipPlane, colorOfFrustumItself, durationInSec, hiddenByNearerObjects);
                    }
                }
            }
        }

        static void DrawHighlightedPlane(Vector3 position, Vector3 forward_normalized, float distanceOfHighlightedPlane, float farClipPlane, Color colorOfFrustumItself, float durationInSec, bool hiddenByNearerObjects)
        {
            if ((distanceOfHighlightedPlane <= farClipPlane) || DrawEngineBasics.drawFrustumsHighlightedPlaneAlsoIfFarerThanFarClipPlane)
            {
                Color colorOfHighlightedPlane = UtilitiesDXXL_Colors.IsDefaultColor(DrawEngineBasics.overwriteColorForFrustumsHighlightedPlane) ? Get_defaultColor_ofFrustumsHighlightedPlane(colorOfFrustumItself) : DrawEngineBasics.overwriteColorForFrustumsHighlightedPlane;
                Color colorFor01 = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorOfHighlightedPlane, 0.5f);
                Color colorFor001 = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorOfHighlightedPlane, 0.15f);

                Vector3 center_ofHighlightedPlane = position + forward_normalized * distanceOfHighlightedPlane;
                frustums_highightedPlane.Recreate(center_ofHighlightedPlane, forward_normalized);
                Vector3 highlightedPlanes_lowLeftVertex = frustums_highightedPlane.GetIntersectionWithLine(frustums_lowLeftEdge);
                Vector3 highlightedPlanes_topLeftVertex = frustums_highightedPlane.GetIntersectionWithLine(frustums_topLeftEdge);
                Vector3 highlightedPlanes_lowRightVertex = frustums_highightedPlane.GetIntersectionWithLine(frustums_lowRightEdge);
                Vector3 highlightedPlanes_topRightVertex = frustums_highightedPlane.GetIntersectionWithLine(frustums_topRightEdge);

                //three main horiz lines:
                UtilitiesDXXL_DrawBasics.Line(highlightedPlanes_lowLeftVertex, highlightedPlanes_lowRightVertex, colorOfHighlightedPlane, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f, 1.0f);
                UtilitiesDXXL_DrawBasics.Line(0.5f * (highlightedPlanes_lowLeftVertex + highlightedPlanes_topLeftVertex), 0.5f * (highlightedPlanes_lowRightVertex + highlightedPlanes_topRightVertex), colorOfHighlightedPlane, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f, 1.0f);
                UtilitiesDXXL_DrawBasics.Line(highlightedPlanes_topLeftVertex, highlightedPlanes_topRightVertex, colorOfHighlightedPlane, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f, 1.0f);

                //three main vert lines:
                UtilitiesDXXL_DrawBasics.Line(highlightedPlanes_lowLeftVertex, highlightedPlanes_topLeftVertex, colorOfHighlightedPlane, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f, 1.0f);
                UtilitiesDXXL_DrawBasics.Line(0.5f * (highlightedPlanes_lowLeftVertex + highlightedPlanes_lowRightVertex), 0.5f * (highlightedPlanes_topLeftVertex + highlightedPlanes_topRightVertex), colorOfHighlightedPlane, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f, 1.0f);
                UtilitiesDXXL_DrawBasics.Line(highlightedPlanes_lowRightVertex, highlightedPlanes_topRightVertex, colorOfHighlightedPlane, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f, 1.0f);

                Vector3 lowerEndToUpperEndOnHighlightedPlane_inWorldSpace = highlightedPlanes_topLeftVertex - highlightedPlanes_lowLeftVertex;
                Vector3 leftEndToRightEndOnHighlightedPlane_inWorldSpace = highlightedPlanes_lowRightVertex - highlightedPlanes_lowLeftVertex;

                for (int i = 1; i < 10; i++)
                {
                    float currentProgress = (0.1f * i);

                    Vector3 currentRightShiftVector = leftEndToRightEndOnHighlightedPlane_inWorldSpace * currentProgress;
                    UtilitiesDXXL_DrawBasics.Line(highlightedPlanes_lowLeftVertex + currentRightShiftVector, highlightedPlanes_topLeftVertex + currentRightShiftVector, colorFor01, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f, 1.0f);

                    Vector3 currentUpwardShiftVector = lowerEndToUpperEndOnHighlightedPlane_inWorldSpace * currentProgress;
                    UtilitiesDXXL_DrawBasics.Line(highlightedPlanes_lowLeftVertex + currentUpwardShiftVector, highlightedPlanes_lowRightVertex + currentUpwardShiftVector, colorFor01, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f, 1.0f);
                }

                for (int i = 1; i < 100; i++)
                {
                    float currentProgress = (0.01f * i);

                    Vector3 currentRightShiftVector = leftEndToRightEndOnHighlightedPlane_inWorldSpace * currentProgress;
                    UtilitiesDXXL_DrawBasics.Line(highlightedPlanes_lowLeftVertex + currentRightShiftVector, highlightedPlanes_topLeftVertex + currentRightShiftVector, colorFor001, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f, 1.0f);

                    Vector3 currentUpwardShiftVector = lowerEndToUpperEndOnHighlightedPlane_inWorldSpace * currentProgress;
                    UtilitiesDXXL_DrawBasics.Line(highlightedPlanes_lowLeftVertex + currentUpwardShiftVector, highlightedPlanes_lowRightVertex + currentUpwardShiftVector, colorFor001, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false, null, false, 0.0f, 1.0f);
                }
            }
        }

        public static Color Get_defaultColor_ofFrustumsHighlightedPlane(Color colorOfFrustumItself)
        {
            return ((colorOfFrustumItself.grayscale < 0.175f) ? Color.Lerp(colorOfFrustumItself, Color.white, 0.7f) : Color.Lerp(colorOfFrustumItself, Color.black, 0.7f));
        }

        static void DrawTextAtCameraFrustum(Vector3 position, Vector3 forward_normalized, float nearClipPlane, float farClipPlane, float widthOfCamsNearPlane, string text, bool forceTextOnNearPlaneUnmirroredTowardsCam, Color color_ofEdgeLines, Vector3 up_normalized, Vector3 right_normalized, Vector3 farPlanes_lowRightVertex, Vector3 farPlanes_lowLeftVertex, float durationInSec, bool hiddenByNearerObjects)
        {
            //on nearPlane:
            //(slightly inside frustum = readable in cameras generated image)
            bool autoFlipToPreventMirrorInverted = !forceTextOnNearPlaneUnmirroredTowardsCam;
            Vector3 textPosition = position + forward_normalized * nearClipPlane * 1.01f;
            float textSize = widthOfCamsNearPlane * 0.05f;
            float autoLineBreakWidth = widthOfCamsNearPlane * 0.9f;
            UtilitiesDXXL_Text.Write(text, textPosition, color_ofEdgeLines, textSize, right_normalized, up_normalized, DrawText.TextAnchorDXXL.MiddleCenter, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, autoLineBreakWidth, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, false, false, true);

            //on farPlane:
            //(slightly outside frustum = not readable in cameras generated image)
            float widthOfCamsFarPlane = (farPlanes_lowRightVertex - farPlanes_lowLeftVertex).magnitude;
            autoFlipToPreventMirrorInverted = true;
            textPosition = position + forward_normalized * farClipPlane * 1.001f;
            textSize = widthOfCamsFarPlane * 0.05f;
            autoLineBreakWidth = widthOfCamsFarPlane * 0.9f;
            UtilitiesDXXL_Text.Write(text, textPosition, color_ofEdgeLines, textSize, right_normalized, up_normalized, DrawText.TextAnchorDXXL.MiddleCenter, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, autoLineBreakWidth, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, false, false, true);
        }

        static void DrawFrustumsBoundarySurfaces(Vector3 nearPlanes_anchor1, Vector3 nearPlanes_anchor2, Vector3 farPlanes_anchor1, Vector3 farPlanes_anchor2, Color color_ofBoundarySurfaceLines, int linesPerBoundarySurface, float durationInSec, bool hiddenByNearerObjects)
        {
            if (linesPerBoundarySurface > 0)
            {
                Vector3 nearPlane_fromAnchor1_toAnchor2 = nearPlanes_anchor2 - nearPlanes_anchor1;
                Vector3 farPlane_fromAnchor1_toAnchor2 = farPlanes_anchor2 - farPlanes_anchor1;

                Vector3 nearPlane_fromSubLineAnchorToSubLineAnchor = nearPlane_fromAnchor1_toAnchor2 / (float)(linesPerBoundarySurface + 1);
                Vector3 farPlane_fromSubLineAnchorToSubLineAnchor = farPlane_fromAnchor1_toAnchor2 / (float)(linesPerBoundarySurface + 1);

                for (int i = 1; i <= linesPerBoundarySurface; i++)
                {
                    Line_fadeableAnimSpeed.InternalDraw(nearPlanes_anchor1 + nearPlane_fromSubLineAnchorToSubLineAnchor * i, farPlanes_anchor1 + farPlane_fromSubLineAnchorToSubLineAnchor * i, color_ofBoundarySurfaceLines, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                }
            }
        }

        static InternalDXXL_Plane drawPlane = new InternalDXXL_Plane();
        public static void TagGameObjectScreenspace(Camera camera, GameObject gameObject, string text = null, Color colorForText = default(Color), Color colorForTagBox = default(Color), float linesWidth_relToViewportHeight = 0.0f, bool drawPointerIfOffscreen = true, float relTextSizeScaling = 1.0f, bool encapsulateChildren = true, float durationInSec = 0.0f)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(camera, "camera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(camera)) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(gameObject, "gameObject")) { return; }

            FillBounds(gameObject, encapsulateChildren, out Vector3 globalExtents, out Vector3 globalCenter, out bool rotateBoundingBox);
            FillTagBoxOrientationVectors(gameObject, rotateBoundingBox, out Vector3 tagBoxUp, out Vector3 tagBoxForward);

            if (UtilitiesDXXL_Colors.IsDefaultColor(colorForText))
            {
                colorForText = UtilitiesDXXL_Colors.Get_randomColorSeeded(gameObject.GetInstanceID());
            }

            if (UtilitiesDXXL_Colors.IsDefaultColor(colorForTagBox))
            {
                colorForTagBox = UtilitiesDXXL_Colors.Get_randomColorSeeded(gameObject.GetInstanceID());
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(globalExtents))
            {
                UtilitiesDXXL_Screenspace.PointFallback(camera, gameObject.transform.position, "[<color=#adadadFF><icon=logMessage></color> GameObject with extent of zero]<br>" + text, colorForTagBox, linesWidth_relToViewportHeight, durationInSec);
                return;
            }

            int usedSlotsIn_verticesGlobal = UtilitiesDXXL_Shapes.Cube(globalCenter, 2.0f * globalExtents, colorForTagBox, colorForTagBox, tagBoxUp, tagBoxForward, 0.0f, null, DrawBasics.LineStyle.disconnectedAnchors, 1.0f, false, durationInSec, false, true, null);
            drawPlane.Recreate(camera.transform.position + camera.transform.forward * (camera.nearClipPlane + DrawScreenspace.drawOffsetBehindCamsNearPlane), camera.transform.forward);

            InternalDXXL_BoundsCamViewportSpace boundsViewportSpace_ofFrontOfCamVertices = null;
            InternalDXXL_BoundsCamViewportSpace boundsViewportSpace_ofBackOfCamAndOutsideOrthoScreenCorridorVertices = null;
            InternalDXXL_BoundsCamViewportSpace boundsViewportSpace_ofBackOfCamAndInsideOrthoScreenCorridorVertices = null;

            for (int i = 0; i < usedSlotsIn_verticesGlobal; i++)
            {
                if (drawPlane.CheckIf_twoPoints_lieOnDifferentSidesOfThePlane_returnsFalseIfAGivenPointIsONplane(camera.transform.position, UtilitiesDXXL_Shapes.verticesGlobal[i]))
                {
                    //vertex in front of cam:
                    Vector2 vertex_viewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(camera, UtilitiesDXXL_Shapes.verticesGlobal[i], false);
                    InternalDXXL_BoundsCamViewportSpace.ConstructAndOrEncapsulate(ref boundsViewportSpace_ofFrontOfCamVertices, vertex_viewportSpace, false);
                }
                else
                {
                    //vertex behind cam:
                    Vector3 vertex_perpProjectedOntoDrawPlane = drawPlane.Get_perpProjectionOfPointOnPlane(UtilitiesDXXL_Shapes.verticesGlobal[i]);
                    Vector2 vertex_viewportSpace = UtilitiesDXXL_Screenspace.WorldPos_to_ViewportPos0to1(camera, vertex_perpProjectedOntoDrawPlane, false);
                    if (InternalDXXL_BoundsCamViewportSpace.IsInsideViewportInclBorder(vertex_viewportSpace))
                    {
                        InternalDXXL_BoundsCamViewportSpace.ConstructAndOrEncapsulate(ref boundsViewportSpace_ofBackOfCamAndInsideOrthoScreenCorridorVertices, vertex_viewportSpace, false);
                    }
                    else
                    {
                        InternalDXXL_BoundsCamViewportSpace.ConstructAndOrEncapsulate(ref boundsViewportSpace_ofBackOfCamAndOutsideOrthoScreenCorridorVertices, vertex_viewportSpace, false);
                    }
                }
            }

            if (boundsViewportSpace_ofFrontOfCamVertices != null)
            {
                InternalDXXL_BoundsCamViewportSpace.ConstructAndOrEncapsulate(ref boundsViewportSpace_ofFrontOfCamVertices, boundsViewportSpace_ofBackOfCamAndOutsideOrthoScreenCorridorVertices, true);
                InternalDXXL_BoundsCamViewportSpace.ConstructAndOrEncapsulate(ref boundsViewportSpace_ofFrontOfCamVertices, boundsViewportSpace_ofBackOfCamAndInsideOrthoScreenCorridorVertices, true);
            }
            else
            {
                if (boundsViewportSpace_ofBackOfCamAndOutsideOrthoScreenCorridorVertices != null)
                {
                    Vector2 startPosOfOutsideCamBounds = InternalDXXL_BoundsCamViewportSpace.IsInsideViewportInclBorder(boundsViewportSpace_ofBackOfCamAndOutsideOrthoScreenCorridorVertices.center) ? InternalDXXL_BoundsCamViewportSpace.GetViewportCenterPlumbIntersectionWithViewportBorderShifted(boundsViewportSpace_ofBackOfCamAndOutsideOrthoScreenCorridorVertices.center, 0.01f) : boundsViewportSpace_ofBackOfCamAndOutsideOrthoScreenCorridorVertices.center;
                    boundsViewportSpace_ofFrontOfCamVertices = new InternalDXXL_BoundsCamViewportSpace(startPosOfOutsideCamBounds, Vector2.zero);
                    InternalDXXL_BoundsCamViewportSpace.ConstructAndOrEncapsulate(ref boundsViewportSpace_ofFrontOfCamVertices, boundsViewportSpace_ofBackOfCamAndOutsideOrthoScreenCorridorVertices, true);
                    InternalDXXL_BoundsCamViewportSpace.ConstructAndOrEncapsulate(ref boundsViewportSpace_ofFrontOfCamVertices, boundsViewportSpace_ofBackOfCamAndInsideOrthoScreenCorridorVertices, true);
                }
                else
                {
                    boundsViewportSpace_ofFrontOfCamVertices = new InternalDXXL_BoundsCamViewportSpace(InternalDXXL_BoundsCamViewportSpace.GetViewportCenterPlumbIntersectionWithViewportBorderShifted(boundsViewportSpace_ofBackOfCamAndInsideOrthoScreenCorridorVertices.center, 0.01f), Vector2.zero);
                }
            }

            float height_relToViewportHeight = boundsViewportSpace_ofFrontOfCamVertices.yMax - boundsViewportSpace_ofFrontOfCamVertices.yMin;
            float width_relToViewportWidth = (boundsViewportSpace_ofFrontOfCamVertices.xMax - boundsViewportSpace_ofFrontOfCamVertices.xMin);
            float width_relToViewportHeight = width_relToViewportWidth * camera.aspect;
            Vector2 boxCenterPos = boundsViewportSpace_ofFrontOfCamVertices.center;
            if (boundsViewportSpace_ofFrontOfCamVertices.IsCompletelyOutsideViewport())
            {
                if (drawPointerIfOffscreen == false)
                {
                    return;
                }

                height_relToViewportHeight = 0.001f;
                width_relToViewportHeight = 0.001f;
                boxCenterPos = InternalDXXL_BoundsCamViewportSpace.GetViewportCenterPlumbIntersectionWithViewportBorderShifted(boxCenterPos, 0.01f);
            }
            else
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(height_relToViewportHeight) && UtilitiesDXXL_Math.ApproximatelyZero(width_relToViewportWidth))
                {
                    UtilitiesDXXL_Screenspace.PointFallback(camera, boxCenterPos, "[<color=#adadadFF><icon=logMessage></color> TagGameObjectScreenspace: Extent of gameobjects projection onto screen is zero]<br>" + text, colorForTagBox, linesWidth_relToViewportHeight, durationInSec);
                    return;
                }
            }

            bool addTextForOutsideDistance_toOffscreenPointer = false; //questionable if activating this would make sense, since this would display the 2D distance inside the camera plane. So gameObject that are far away (in 3D worldspace) could be displayed with the same small distance than GameOjects right beside the camera.
            UtilitiesDXXL_Screenspace.DrawShape(camera, boxCenterPos, DrawShapes.Shape2DType.square, colorForTagBox, colorForText, width_relToViewportHeight, height_relToViewportHeight, 0.0f, linesWidth_relToViewportHeight, text, DrawBasics.LineStyle.disconnectedAnchors, 1.0f, DrawBasics.LineStyle.invisible, drawPointerIfOffscreen, addTextForOutsideDistance_toOffscreenPointer, durationInSec, relTextSizeScaling, gameObject.name, true);
        }

        public static void FillBounds(GameObject gameObject, bool encapsulateChildren, out Vector3 globalExtents, out Vector3 globalCenter, out bool rotateBoundingBox)
        {
            globalCenter = gameObject.transform.position;
            rotateBoundingBox = true;

            Vector3 localExtents = 0.5f * Vector3.one;
            MeshFilter meshfilter = gameObject.GetComponent<MeshFilter>();
            if (meshfilter != null)
            {
                if (Application.isPlaying)
                {
                    if (meshfilter.mesh != null)
                    {
                        if (meshfilter.mesh.bounds != null)
                        {
                            localExtents = meshfilter.mesh.bounds.extents;
                        }
                    }
                }
                else
                {
                    if (meshfilter.sharedMesh != null)
                    {
                        if (meshfilter.sharedMesh.bounds != null)
                        {
                            localExtents = meshfilter.sharedMesh.bounds.extents;
                        }
                    }
                }
            }

            SkinnedMeshRenderer skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
            if (skinnedMeshRenderer != null)
            {
                if (skinnedMeshRenderer.localBounds != null)
                {
                    //-> acts only as a fallback for the unexpected (or impossible?) case where a SkinnedMeshRenderer has ".localBounds", but no ".bounds". If it has ".bounds" this will get overwritten below
                    localExtents = skinnedMeshRenderer.localBounds.extents;
                }
            }

            globalExtents = Vector3.Scale(localExtents, gameObject.transform.lossyScale);

            if ((encapsulateChildren == false) || gameObject.transform.childCount == 0)
            {
                MeshRenderer meshRenderer = gameObject.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    if (meshRenderer.bounds != null)
                    {
                        globalCenter = meshRenderer.bounds.center;
                    }
                }

                if (skinnedMeshRenderer != null)
                {
                    if (skinnedMeshRenderer.bounds != null)
                    {
                        //-> if you want to see the local bounds box of a SkinnedMeshRenderer: Use "EngineBasics.LocalBounds()" instead
                        rotateBoundingBox = false;
                        globalCenter = skinnedMeshRenderer.bounds.center;
                        globalExtents = skinnedMeshRenderer.bounds.extents;
                    }
                }
            }
            else
            {
                rotateBoundingBox = false;
                Bounds boundsOfWholeChildHierarchyGlobal = new Bounds(gameObject.transform.position, gameObject.transform.lossyScale);
                foreach (Transform childTransform in gameObject.GetComponentsInChildren<Transform>())
                {
                    Bounds childTranformBoundsGlobal = new Bounds(childTransform.position, childTransform.lossyScale);
                    boundsOfWholeChildHierarchyGlobal.Encapsulate(childTranformBoundsGlobal);

                    MeshRenderer childsMeshRenderer = childTransform.GetComponent<MeshRenderer>();
                    if (childsMeshRenderer != null)
                    {
                        if (childsMeshRenderer.bounds != null)
                        {
                            boundsOfWholeChildHierarchyGlobal.Encapsulate(childsMeshRenderer.bounds); //"MeshRenderer" delivers GLOBAL bounds
                        }
                    }

                    SkinnedMeshRenderer childsSkinnedMeshRenderer = childTransform.GetComponent<SkinnedMeshRenderer>();
                    if (childsSkinnedMeshRenderer != null)
                    {
                        if (childsSkinnedMeshRenderer.bounds != null)
                        {
                            boundsOfWholeChildHierarchyGlobal.Encapsulate(childsSkinnedMeshRenderer.bounds); //"skinnedMeshRenderer.bounds" delivers GLOBAL bounds
                        }
                    }
                }
                globalExtents = boundsOfWholeChildHierarchyGlobal.extents;
                globalCenter = boundsOfWholeChildHierarchyGlobal.center;
            }
        }

        public static void FillTagBoxOrientationVectors(GameObject gameObject, bool rotateBoundingBox, out Vector3 tagBoxUp, out Vector3 tagBoxForward)
        {
            if (rotateBoundingBox)
            {
                tagBoxUp = gameObject.transform.up;
                tagBoxForward = gameObject.transform.forward;
            }
            else
            {
                tagBoxUp = Vector3.up;
                tagBoxForward = Vector3.forward;
            }
        }

        public static void BoolDisplayer(bool boolValueToDisplay, Vector3 position = default(Vector3), string boolName = null, float size = 1.0f, Quaternion rotation = default(Quaternion), Color color_forTextAndFrame = default(Color), Color overwriteColor_forTrue = default(Color), Color overwriteColor_forFalse = default(Color), float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(size, "size")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }


            if (UtilitiesDXXL_Math.ApproximatelyZero(size))
            {
                Color color_forTrue = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor_forTrue, UtilitiesDXXL_Colors.green_boolTrue);
                Color color_forFalse = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor_forFalse, UtilitiesDXXL_Colors.red_boolFalse);
                UtilitiesDXXL_DrawBasics.PointFallback(position, "[<color=#adadadFF><icon=logMessage></color> BoolDisplayer with extent of 0]<br>" + boolName, boolValueToDisplay ? color_forTrue : color_forFalse, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            rotation = UtilitiesDXXL_FlatShapesNormaAndUpCalculation.GetQuaternion(rotation, position);
            color_forTextAndFrame = UtilitiesDXXL_Colors.OverwriteDefaultColor(color_forTextAndFrame);
            size = Mathf.Abs(size);

            string boolTrafficLightAsText = GetBoolTrafficLightAsText(boolValueToDisplay, overwriteColor_forTrue, overwriteColor_forFalse);
            string headlineText = "unnamed   bool:";
            if (boolName != null && boolName != "")
            {
                headlineText = "" + boolName + ":";
            }

            float autoLineBreakWidth = 0.86f * size;
            float halfSize = 0.5f * size;
            Vector3 textDir_normalized = rotation * Vector3.right;
            Vector3 up_normalized = rotation * Vector3.up;

            UtilitiesDXXL_Text.Write(headlineText, position, color_forTextAndFrame, 0.0859f * size, textDir_normalized, up_normalized, DrawText.TextAnchorDXXL.LowerCenter, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, autoLineBreakWidth, 0.0f, autoLineBreakWidth, true, durationInSec, hiddenByNearerObjects, false, false, true);
            float height_wholeTextBlock_ofHeadlineText = DrawText.parsedTextSpecs.height_wholeTextBlock;
            UtilitiesDXXL_Text.Write(boolTrafficLightAsText, position - up_normalized * halfSize, color_forTextAndFrame, size, textDir_normalized, up_normalized, DrawText.TextAnchorDXXL.LowerCenter, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);

            float rectHeight = halfSize + height_wholeTextBlock_ofHeadlineText;
            float rectWidth = size;
            Vector3 rectCenter = position + up_normalized * (-halfSize + 0.5f * rectHeight);
            Vector3 forward_normalized = rotation * Vector3.forward;
            DrawShapes.FlatShape(rectCenter, DrawShapes.Shape2DType.square, rectWidth, rectHeight, color_forTextAndFrame, forward_normalized, up_normalized, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, true, DrawBasics.LineStyle.invisible, false, durationInSec, hiddenByNearerObjects);
        }

        static List<string> screenspaceBoolDisplayNames = new List<string>();
        public static void BoolDisplayerScreenspace(Camera camera, bool boolValueToDisplay, string boolName = null, Vector2 position = default(Vector2), float size_relToViewportHeight = 0.175f, Color color_forTextAndFrame = default(Color), Color overwriteColor_forTrue = default(Color), Color overwriteColor_forFalse = default(Color), float durationInSec = 0.0f)
        {

            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(camera, "camera")) { return; }
            if (UtilitiesDXXL_Screenspace.CheckIfViewportIsTooSmall(camera)) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(size_relToViewportHeight, "size_relToViewportHeight")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }


            if (UtilitiesDXXL_Math.ApproximatelyZero(size_relToViewportHeight))
            {
                Color color_forTrue = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor_forTrue, UtilitiesDXXL_Colors.green_boolTrue);
                Color color_forFalse = UtilitiesDXXL_Colors.OverwriteDefaultColor(overwriteColor_forFalse, UtilitiesDXXL_Colors.red_boolFalse);
                UtilitiesDXXL_Screenspace.PointFallback(camera, position, "[<color=#adadadFF><icon=logMessage></color> BoolDisplayerScreenspace with extent of 0]<br>" + boolName, boolValueToDisplay ? color_forTrue : color_forFalse, 0.0f, durationInSec);
                return;
            }

            color_forTextAndFrame = UtilitiesDXXL_Colors.OverwriteDefaultColor(color_forTextAndFrame);
            size_relToViewportHeight = Mathf.Abs(size_relToViewportHeight);
            float halfSize_relToViewportHeight = 0.5f * size_relToViewportHeight;
            float size_relToViewportWidth = size_relToViewportHeight / camera.aspect;

            string headlineText;
            if (boolName != null && boolName != "")
            {
                if (UtilitiesDXXL_Math.IsDefaultVector(position))
                {
                    float initialXPos = 0.7f * size_relToViewportWidth;
                    float initialYPos = 0.888f;
                    float xDistance = size_relToViewportWidth * 1.25f;
                    float yDistance = 0.195f;
                    float halfYDistance = 0.5f * yDistance;

                    position = new Vector2(initialXPos, initialYPos);
                    bool nameIsAlreadyRegistered = false;
                    for (int i = 0; i < screenspaceBoolDisplayNames.Count; i++)
                    {
                        if (boolName == screenspaceBoolDisplayNames[i])
                        {
                            nameIsAlreadyRegistered = true;
                            break;
                        }
                        else
                        {
                            position.y = position.y - yDistance;
                            if (position.y < halfYDistance)
                            {
                                position.x = position.x + xDistance;
                                position.y = initialYPos;
                            }
                        }
                    }

                    if (nameIsAlreadyRegistered == false)
                    {
                        screenspaceBoolDisplayNames.Add(boolName);
                    }

                }
                headlineText = "" + boolName + ":";
            }
            else
            {
                if (UtilitiesDXXL_Math.IsDefaultVector(position))
                {
                    headlineText = "bool      without   name or   pos*<br><size=5>*no auto positioning</size><br><size=6>*danger of overlay</size>";
                    position = new Vector2(1.0f - 0.7f * size_relToViewportWidth, 0.125f);
                }
                else
                {
                    headlineText = "unnamed   bool:";
                }
            }

            string boolTrafficLightAsText = GetBoolTrafficLightAsText(boolValueToDisplay, overwriteColor_forTrue, overwriteColor_forFalse);
            float autoLineBreakWidth_relToViewportWidth = 0.86f * size_relToViewportWidth;
            UtilitiesDXXL_Text.WriteScreenspace(camera, headlineText, position, color_forTextAndFrame, 0.0859f * size_relToViewportHeight, 0.0f, DrawText.TextAnchorDXXL.LowerCenter, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, autoLineBreakWidth_relToViewportWidth, 0.0f, false, autoLineBreakWidth_relToViewportWidth, true, durationInSec, false);
            float height_wholeTextBlock_ofHeadlineText = DrawText.parsedTextSpecs.height_wholeTextBlock;
            UtilitiesDXXL_Text.WriteScreenspace(camera, boolTrafficLightAsText, position - Vector2.up * halfSize_relToViewportHeight, color_forTextAndFrame, size_relToViewportHeight, 0.0f, DrawText.TextAnchorDXXL.LowerCenter, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, false, 0.0f, true, durationInSec, false);

            float rectHeight_relToViewportHeight = halfSize_relToViewportHeight + height_wholeTextBlock_ofHeadlineText;
            Vector2 rectCenter = position + Vector2.up * (-halfSize_relToViewportHeight + 0.5f * rectHeight_relToViewportHeight);
            DrawScreenspace.Shape(camera, rectCenter, DrawShapes.Shape2DType.square, color_forTextAndFrame, size_relToViewportHeight, rectHeight_relToViewportHeight, 0.0f, 0.0f, null, false, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, durationInSec);
        }

        static string GetBoolTrafficLightAsText(bool boolValueToDisplay, Color overwriteColor_forTrue, Color overwriteColor_forFalse)
        {
            if (UtilitiesDXXL_Colors.IsDefaultColor(overwriteColor_forTrue) && UtilitiesDXXL_Colors.IsDefaultColor(overwriteColor_forFalse))
            {
                if (boolValueToDisplay)
                {
                    return "<size=5><sw=70000><color=#ed47314C><icon=circleDotUnfilled></color><color=#83e24aFF><icon=circleDotFilled></color></sw></size>";
                }
                else
                {
                    return "<size=5><sw=70000><color=#ed4731FF><icon=circleDotFilled></color><color=#83e24a4C><icon=circleDotUnfilled></color></sw></size>";
                }
            }
            else
            {
                if (boolValueToDisplay)
                {
                    return "<size=5><sw=70000><color=#" + ColorUtility.ToHtmlStringRGBA(overwriteColor_forFalse) + "><icon=circleDotUnfilled></color><color=#" + ColorUtility.ToHtmlStringRGBA(overwriteColor_forTrue) + "><icon=circleDotFilled></color></sw></size>";
                }
                else
                {
                    return "<size=5><sw=70000><color=#" + ColorUtility.ToHtmlStringRGBA(overwriteColor_forFalse) + "><icon=circleDotFilled></color><color=#" + ColorUtility.ToHtmlStringRGBA(overwriteColor_forTrue) + "><icon=circleDotUnfilled></color></sw></size>";
                }
            }
        }

        public static void CoordinateAxesGizmoLocal(Vector3 position_OfLocalCoordinateSystem, Quaternion rotation_OfLocalCoordinateSystem, Vector3 scale_OfLocalCoordinateSystem, float forceAllAxesLength, float lineWidth_inGlobalUnits, string text, bool drawXYZchars, bool skipConeDrawing, float durationInSec, bool hiddenByNearerObjects, bool aParentHasANonUniformScale)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position_OfLocalCoordinateSystem, "position_OfLocalCoordinateSystem")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(scale_OfLocalCoordinateSystem, "scale_OfLocalCoordinateSystem")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(forceAllAxesLength, "forceAllAxesLength")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lineWidth_inGlobalUnits, "linesWidth")) { return; }

            rotation_OfLocalCoordinateSystem = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation_OfLocalCoordinateSystem);

            if (UtilitiesDXXL_Math.IsDefaultVector(scale_OfLocalCoordinateSystem)) { scale_OfLocalCoordinateSystem = Vector3.one; }
            if (UtilitiesDXXL_Math.ApproximatelyZero(forceAllAxesLength) == false) { scale_OfLocalCoordinateSystem = new Vector3(forceAllAxesLength, forceAllAxesLength, forceAllAxesLength); }

            float enlargeSmallTextToThisMinTextSize_x = 0.25f * scale_OfLocalCoordinateSystem.x;
            float enlargeSmallTextToThisMinTextSize_y = 0.25f * scale_OfLocalCoordinateSystem.y;
            float enlargeSmallTextToThisMinTextSize_z = 0.25f * scale_OfLocalCoordinateSystem.z;
            string text_x;
            string text_y;
            string text_z;
            float lineWidth_x = lineWidth_inGlobalUnits;
            float lineWidth_y = lineWidth_inGlobalUnits;
            float lineWidth_z = lineWidth_inGlobalUnits;
            bool skipsPointer_x = skipConeDrawing;
            bool skipsPointer_y = skipConeDrawing;
            bool skipsPointer_z = skipConeDrawing;

            if (UtilitiesDXXL_Math.ApproximatelyZero(scale_OfLocalCoordinateSystem.x))
            {
                text_x = "<color=#adadadFF><icon=logMessage></color> X axis has<br><size=4> </size><br>  zero length";
                lineWidth_x = 0.0f;
                scale_OfLocalCoordinateSystem.x = 1.0f;
                skipsPointer_x = true;
                enlargeSmallTextToThisMinTextSize_x = 0.0f;
            }
            else
            {
                text_x = drawXYZchars ? "X" : null;
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(scale_OfLocalCoordinateSystem.y))
            {
                text_y = "<color=#adadadFF><icon=logMessage></color> Y axis has<br><size=4> </size><br>  zero length";
                lineWidth_y = 0.0f;
                scale_OfLocalCoordinateSystem.y = 1.0f;
                skipsPointer_y = true;
                enlargeSmallTextToThisMinTextSize_y = 0.0f;
            }
            else
            {
                text_y = drawXYZchars ? "Y" : null;
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(scale_OfLocalCoordinateSystem.z))
            {
                text_z = "<color=#adadadFF><icon=logMessage></color> Z axis has<br><size=4> </size><br>  zero length";
                lineWidth_z = 0.0f;
                scale_OfLocalCoordinateSystem.z = 1.0f;
                skipsPointer_z = true;
                enlargeSmallTextToThisMinTextSize_z = 0.0f;
            }
            else
            {
                text_z = drawXYZchars ? "Z" : null;
            }

            Vector3 vector_xAxis_normalized = rotation_OfLocalCoordinateSystem * Vector3.right;
            Vector3 vector_yAxis_normalized = rotation_OfLocalCoordinateSystem * Vector3.up;
            Vector3 vector_zAxis_normalized = rotation_OfLocalCoordinateSystem * Vector3.forward;
            Vector3 vector_xAxis = vector_xAxis_normalized * scale_OfLocalCoordinateSystem.x;
            Vector3 vector_yAxis = vector_yAxis_normalized * scale_OfLocalCoordinateSystem.y;
            Vector3 vector_zAxis = vector_zAxis_normalized * scale_OfLocalCoordinateSystem.z;

            Vector3 customAmplitudeAndTextDir_x = vector_yAxis_normalized;
            Vector3 customAmplitudeAndTextDir_y = (-vector_xAxis_normalized);
            Vector3 customAmplitudeAndTextDir_z = vector_yAxis_normalized;

            //-> y is drawn last, because there are probably more cases where the user looks at the gizmo from camPosHigherThanGizmo...and then the y axis shouldn't be hidden by the other axes (only significant for nonZero-lineWidthes)
            //-> same reason: x is drawn after z, because the view dir may be mostly along positive z
            //-> could be refactored similar to "UtilitiesDXXL_Euler.EulerRotation_local()" where the observer camera is taken into account

            if (skipsPointer_z)
            {
                Line_fadeableAnimSpeed.InternalDraw(position_OfLocalCoordinateSystem, position_OfLocalCoordinateSystem + vector_zAxis, UtilitiesDXXL_Colors.blue_zAxisAlpha1, lineWidth_z, text_z, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, customAmplitudeAndTextDir_z, false, 0.0f, 0.0f, enlargeSmallTextToThisMinTextSize_z, durationInSec, hiddenByNearerObjects, false, false);
            }
            else
            {
                UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                DrawBasics.VectorFrom(position_OfLocalCoordinateSystem, vector_zAxis, UtilitiesDXXL_Colors.blue_zAxisAlpha1, lineWidth_z, text_z, 0.17f, false, false, customAmplitudeAndTextDir_z, false, enlargeSmallTextToThisMinTextSize_z, false, 0.0f, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
            }

            if (skipsPointer_x)
            {
                Line_fadeableAnimSpeed.InternalDraw(position_OfLocalCoordinateSystem, position_OfLocalCoordinateSystem + vector_xAxis, UtilitiesDXXL_Colors.red_xAxisAlpha1, lineWidth_x, text_x, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, customAmplitudeAndTextDir_x, false, 0.0f, 0.0f, enlargeSmallTextToThisMinTextSize_x, durationInSec, hiddenByNearerObjects, false, false);
            }
            else
            {
                UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                DrawBasics.VectorFrom(position_OfLocalCoordinateSystem, vector_xAxis, UtilitiesDXXL_Colors.red_xAxisAlpha1, lineWidth_x, text_x, 0.17f, false, false, customAmplitudeAndTextDir_x, false, enlargeSmallTextToThisMinTextSize_x, false, 0.0f, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
            }

            if (skipsPointer_y)
            {
                Line_fadeableAnimSpeed.InternalDraw(position_OfLocalCoordinateSystem, position_OfLocalCoordinateSystem + vector_yAxis, UtilitiesDXXL_Colors.green_yAxisAlpha1, lineWidth_y, text_y, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, customAmplitudeAndTextDir_y, false, 0.0f, 0.0f, enlargeSmallTextToThisMinTextSize_y, durationInSec, hiddenByNearerObjects, false, false);
            }
            else
            {
                UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                DrawBasics.VectorFrom(position_OfLocalCoordinateSystem, vector_yAxis, UtilitiesDXXL_Colors.green_yAxisAlpha1, lineWidth_y, text_y, 0.17f, false, false, customAmplitudeAndTextDir_y, false, enlargeSmallTextToThisMinTextSize_y, false, 0.0f, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
            }

            if (UtilitiesDXXL_Math.ApproximatelyZero(lineWidth_inGlobalUnits) == false)
            {
                int struts = 8;
                float radius = 0.5f * lineWidth_inGlobalUnits;
                DrawShapes.Sphere(position_OfLocalCoordinateSystem, radius, Color.white, rotation_OfLocalCoordinateSystem, 0.0f, null, struts, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);
            }

            if (aParentHasANonUniformScale)
            {
                text = "[<color=#e2aa00FF><icon=warning></color> A parent transform that defines the local space has a non-uniform scale<br>   -> possibly weird results]<br>" + text;
            }

            if (text != null && text != "")
            {
                float averageScale_ofLocalCoordinateSystem = 0.33333f * (scale_OfLocalCoordinateSystem.x + scale_OfLocalCoordinateSystem.y + scale_OfLocalCoordinateSystem.z);
                float textSize = Mathf.Max(0.1f * averageScale_ofLocalCoordinateSystem, 0.01f);
                UtilitiesDXXL_Text.WriteFramed(text, position_OfLocalCoordinateSystem, Color.white, textSize, default(Vector3), default(Vector3), DrawText.TextAnchorDXXL.UpperRight, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
            }

        }

    }

}
