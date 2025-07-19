namespace DrawXXL
{
    using System;
    using UnityEngine;

    public class UtilitiesDXXL_Quaternion
    {
        static InternalDXXL_Line localTurnAxisLine_inGlobalSpaceUnits = new InternalDXXL_Line();
        public static void QuaternionRotation_local(Quaternion quaternionToDraw, Vector3 posWhereToDraw, Color color_ofTurnAxis, float lineWidth, string text, float length_ofUpAndForwardVectors_local, Vector3 customVectorToRotate_local, float durationInSec, bool hiddenByNearerObjects, bool isLocal, Transform parentTransform)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lineWidth, "lineWidth")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(length_ofUpAndForwardVectors_local, "length_ofUpAndForwardVectors")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(quaternionToDraw.x, "quaternion.x")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(quaternionToDraw.y, "quaternion.y")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(quaternionToDraw.z, "quaternion.z")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(quaternionToDraw.w, "quaternion.w")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(posWhereToDraw, "posWhereToDraw")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(customVectorToRotate_local, "customVectorToRotate")) { return; }

            if (UtilitiesDXXL_Math.QuaternionIsApproxNormalized(quaternionToDraw) == false)
            {
                float quaternionMagnitude = Mathf.Sqrt(quaternionToDraw.x * quaternionToDraw.x + quaternionToDraw.y * quaternionToDraw.y + quaternionToDraw.z * quaternionToDraw.z + quaternionToDraw.w * quaternionToDraw.w);
                UtilitiesDXXL_DrawBasics.PointFallback(posWhereToDraw, "[<color=#ce0e0eFF><icon=logMessageError></color> Invalid quaternion: quaternion is not normalized, but has magnitude = " + quaternionMagnitude + "]<br>" + text, color_ofTurnAxis, lineWidth, durationInSec, hiddenByNearerObjects);
                return;
            }

            color_ofTurnAxis = UtilitiesDXXL_Colors.OverwriteDefaultColor(color_ofTurnAxis);

            Quaternion rotation_ofLocalSpace = (parentTransform == null) ? Quaternion.identity : parentTransform.rotation;
            Quaternion quaternion_local = quaternionToDraw;
            bool rotation_ofLocalSpace_isIdentity = (UtilitiesDXXL_Math.IsDefaultInvalidQuaternion(rotation_ofLocalSpace) || UtilitiesDXXL_Math.IsQuaternionIdentity(rotation_ofLocalSpace));
            quaternion_local.ToAngleAxis(out float localTurnAngleDeg, out Vector3 localTurnAxis_inLocalSpace);

            if (localTurnAngleDeg > 180.0f)
            {
                //-> "ToAngleAxis()" return an angle between 0 and 360 (probably because it internally uses 'acos'), though the common communication on quaternions is that they can represent a span "from -180 to +180".
                localTurnAngleDeg = localTurnAngleDeg - 360.0f;
            }
            float abs_localTurnAngleDeg = Mathf.Abs(localTurnAngleDeg);

            Vector3 localTurnAxis_inLocalSpace_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(localTurnAxis_inLocalSpace);
            if (UtilitiesDXXL_Math.ApproximatelyZero(localTurnAxis_inLocalSpace_normalized))
            {
                //Unity seems to fallback to "Quaternion.identiy" in this case
                UtilitiesDXXL_DrawBasics.PointFallback(posWhereToDraw, "[<color=#ce0e0eFF><icon=logMessageError></color> Quaternion with invalid turn axis (length = 0)]<br>" + text, color_ofTurnAxis, lineWidth, durationInSec, hiddenByNearerObjects);
                return;
            }

            //Note that "normalized in local space" is the same as "normalized in global space", because scale_ofSpaces is not used here:
            Vector3 localTurnAxis_expressedInGlobalSpaceUnits_normalized = rotation_ofLocalSpace_isIdentity ? localTurnAxis_inLocalSpace_normalized : (rotation_ofLocalSpace * localTurnAxis_inLocalSpace_normalized);
            bool localQuaternion_isIdentity = UtilitiesDXXL_Math.IsQuaternionIdentity(quaternion_local);
            bool drawCustomVector = !UtilitiesDXXL_Math.ApproximatelyZero(customVectorToRotate_local);

            //grey dashed line:
            float halfLength_ofDashedLine = 10.0f;
            Color color_ofDashedLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(Color.white, 0.4f);
            Line_fadeableAnimSpeed.InternalDraw(posWhereToDraw - localTurnAxis_expressedInGlobalSpaceUnits_normalized * halfLength_ofDashedLine, posWhereToDraw + localTurnAxis_expressedInGlobalSpaceUnits_normalized * halfLength_ofDashedLine, color_ofDashedLine, 0.0f, null, DrawBasics.LineStyle.dashedLong, 5.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            //turn axis:
            lineWidth = Mathf.Max(lineWidth, 0.008f);
            string textAtTurnAxis;
            if (localQuaternion_isIdentity)
            {
                if (isLocal)
                {
                    textAtTurnAxis = "This <size=2>local</size>quaternion is identity<br><br>= no rotation,<br>x,y,z,w: ( " + quaternion_local.x + " , " + quaternion_local.y + " , " + quaternion_local.z + " , " + quaternion_local.w + " )<br><br>" + text;
                }
                else
                {
                    textAtTurnAxis = "This quaternion is identity<br><br>= no rotation,<br>x,y,z,w: ( " + quaternion_local.x + " , " + quaternion_local.y + " , " + quaternion_local.z + " , " + quaternion_local.w + " )<br><br>" + text;
                }
            }
            else
            {
                if (isLocal)
                {
                    textAtTurnAxis = "<size=2>local</size>Quaternion turn axis<br><br><size=6>x,y,z,w: ( " + quaternion_local.x + " , " + quaternion_local.y + " , " + quaternion_local.z + " , " + quaternion_local.w + " )<br><size=2>local</size>axis vector: ( " + localTurnAxis_inLocalSpace_normalized.x + " , " + localTurnAxis_inLocalSpace_normalized.y + " , " + localTurnAxis_inLocalSpace_normalized.z + " )</size><br>" + text;
                }
                else
                {
                    textAtTurnAxis = "Quaternion turn axis<br><br><size=6>x,y,z,w: ( " + quaternion_local.x + " , " + quaternion_local.y + " , " + quaternion_local.z + " , " + quaternion_local.w + " )<br>axis vector: ( " + localTurnAxis_inLocalSpace_normalized.x + " , " + localTurnAxis_inLocalSpace_normalized.y + " , " + localTurnAxis_inLocalSpace_normalized.z + " )</size><br>" + text;
                }
            }

            UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
            DrawBasics.Vector(posWhereToDraw - localTurnAxis_expressedInGlobalSpaceUnits_normalized, posWhereToDraw + localTurnAxis_expressedInGlobalSpaceUnits_normalized, color_ofTurnAxis, lineWidth, textAtTurnAxis, 0.11f, false, false, default(Vector3), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();

            DrawShapes.Sphere(posWhereToDraw, 2.0f * lineWidth, color_ofTurnAxis, localTurnAxis_expressedInGlobalSpaceUnits_normalized, default(Vector3), 0.0f, null, 8, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);

            if (localQuaternion_isIdentity == false)
            {
                //turn angle visualizer at turnAxisEnd:
                Vector3 aVectorPerpTo_localAxisExpressedInLocalUnits_normalized = UtilitiesDXXL_Math.Get_aNormalizedVector_perpToGivenVector(localTurnAxis_inLocalSpace_normalized);
                Vector3 aVectorPerpTo_localAxisExpressedInGlobalUnits_normalized = rotation_ofLocalSpace * aVectorPerpTo_localAxisExpressedInLocalUnits_normalized;
                Vector3 centerOfTurnAngleVisualizer_inGlobalSpace = posWhereToDraw - localTurnAxis_expressedInGlobalSpaceUnits_normalized;

                if (abs_localTurnAngleDeg < 0.5f)
                {
                    UtilitiesDXXL_Text.Write(" turn angle: " + localTurnAngleDeg + "°", centerOfTurnAngleVisualizer_inGlobalSpace, color_ofTurnAxis, 0.02f, aVectorPerpTo_localAxisExpressedInGlobalUnits_normalized, -localTurnAxis_expressedInGlobalSpaceUnits_normalized, DrawText.TextAnchorDXXL.UpperLeft, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                }
                else
                {
                    float radiusOfTurnAngleVisualizer = 0.1f;

                    Color color_turnAxisLowestAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofTurnAxis, 0.25f);
                    DrawShapes.Circle(centerOfTurnAngleVisualizer_inGlobalSpace, radiusOfTurnAngleVisualizer, color_turnAxisLowestAlpha, localTurnAxis_expressedInGlobalSpaceUnits_normalized, aVectorPerpTo_localAxisExpressedInGlobalUnits_normalized, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);

                    Vector3 centerOfTurnAngleVisualizer_to_startOfTurnAngleVisualizer_inLocalSpace = aVectorPerpTo_localAxisExpressedInLocalUnits_normalized * radiusOfTurnAngleVisualizer;
                    Vector3 centerOfTurnAngleVisualizer_to_startOfTurnAngleVisualizer_inGlobalSpace = rotation_ofLocalSpace * centerOfTurnAngleVisualizer_to_startOfTurnAngleVisualizer_inLocalSpace;

                    Vector3 centerOfTurnAngleVisualizer_to_endOfTurnAngleVisualizer_inLocalSpace = quaternion_local * centerOfTurnAngleVisualizer_to_startOfTurnAngleVisualizer_inLocalSpace;
                    Vector3 centerOfTurnAngleVisualizer_to_endOfTurnAngleVisualizer_inGlobalSpace = rotation_ofLocalSpace * centerOfTurnAngleVisualizer_to_endOfTurnAngleVisualizer_inLocalSpace;

                    Vector3 startOfTurnAngleVisualizer_inGlobalSpace = centerOfTurnAngleVisualizer_inGlobalSpace + centerOfTurnAngleVisualizer_to_startOfTurnAngleVisualizer_inGlobalSpace;
                    Vector3 endOfTurnAxisTurnVisualizer_inGlobalSpace = centerOfTurnAngleVisualizer_inGlobalSpace + centerOfTurnAngleVisualizer_to_endOfTurnAngleVisualizer_inGlobalSpace;

                    Color color_turnAxisLowAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofTurnAxis, 0.5f);
                    Line_fadeableAnimSpeed.InternalDraw(centerOfTurnAngleVisualizer_inGlobalSpace, startOfTurnAngleVisualizer_inGlobalSpace, color_turnAxisLowAlpha, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                    Line_fadeableAnimSpeed.InternalDraw(centerOfTurnAngleVisualizer_inGlobalSpace, endOfTurnAxisTurnVisualizer_inGlobalSpace, color_turnAxisLowAlpha, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                    DrawShapes.Sphere(centerOfTurnAngleVisualizer_inGlobalSpace, 1.3f * lineWidth, color_ofTurnAxis, localTurnAxis_expressedInGlobalSpaceUnits_normalized, default(Vector3), 0.0f, null, 8, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);

                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forCircledVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                    DrawBasics.VectorCircled(startOfTurnAngleVisualizer_inGlobalSpace, centerOfTurnAngleVisualizer_inGlobalSpace, localTurnAxis_expressedInGlobalSpaceUnits_normalized, localTurnAngleDeg, color_ofTurnAxis, 0.3f * lineWidth, null, 0.05f, false, false, true, 0.0f, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, durationInSec, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forCircledVectors();

                    string angleText = "" + localTurnAngleDeg + "°";
                    Color color_ofAngleText = UtilitiesDXXL_Colors.GetSimilarColorWithAdjustableOtherBrightnessValue(color_ofTurnAxis, 0.2f);
                    Vector3 startPos_ofText = centerOfTurnAngleVisualizer_inGlobalSpace + centerOfTurnAngleVisualizer_to_startOfTurnAngleVisualizer_inGlobalSpace * 1.15f;
                    Vector3 turnAxis_ofText = (localTurnAngleDeg < 0.0f) ? localTurnAxis_expressedInGlobalSpaceUnits_normalized : (-localTurnAxis_expressedInGlobalSpaceUnits_normalized); //-> prevent text from starting to away from circledArrow
                    float size_ofTurnAngleVisualizerText = 0.28f * radiusOfTurnAngleVisualizer;
                    UtilitiesDXXL_Text.WriteOnCircle(angleText, startPos_ofText, centerOfTurnAngleVisualizer_inGlobalSpace, turnAxis_ofText, color_ofAngleText, size_ofTurnAngleVisualizerText, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, 0.0f, true, durationInSec, hiddenByNearerObjects);
                }
            }

            //"forward" + "upward" + "custom" BEFORE rotation:
            Vector3 forward_unturnedInLocalSpace_inLocalSpaceUnits_normalized = Vector3.forward;
            Vector3 up_unturnedInLocalSpace_inLocalSpaceUnits_normalized = Vector3.up;

            Vector3 forward_unturnedInLocalSpace_expressedInGlobalSpaceUnits_normalized = rotation_ofLocalSpace * forward_unturnedInLocalSpace_inLocalSpaceUnits_normalized;
            Vector3 up_unturnedInLocalSpace_expressedInGlobalSpaceUnits_normalized = rotation_ofLocalSpace * up_unturnedInLocalSpace_inLocalSpaceUnits_normalized;


            float length_ofUpAndForwardVectors_global = (parentTransform == null) ? length_ofUpAndForwardVectors_local : (parentTransform.lossyScale.x * length_ofUpAndForwardVectors_local);
            Vector3 forward_unturnedInLocalSpace_inLocalSpaceUnits_scaled = forward_unturnedInLocalSpace_inLocalSpaceUnits_normalized * length_ofUpAndForwardVectors_global;
            Vector3 up_unturnedInLocalSpace_inLocalSpaceUnits_scaled = up_unturnedInLocalSpace_inLocalSpaceUnits_normalized * length_ofUpAndForwardVectors_global;

            Vector3 forward_unturnedInLocalSpace_expressedInGlobalSpaceUnits_scaled = rotation_ofLocalSpace * forward_unturnedInLocalSpace_inLocalSpaceUnits_scaled;
            Vector3 up_unturnedInLocalSpace_expressedInGlobalSpaceUnits_scaled = rotation_ofLocalSpace * up_unturnedInLocalSpace_inLocalSpaceUnits_scaled;

            Vector3 endPos_ofUnturnedLocalForwardVector_inGlobalSpace = posWhereToDraw + forward_unturnedInLocalSpace_expressedInGlobalSpaceUnits_scaled;
            Vector3 endPos_ofUnturnedLocalUpwardVector_inGlobalSpace = posWhereToDraw + up_unturnedInLocalSpace_expressedInGlobalSpaceUnits_scaled;

            Vector3 customVectorToRotate_local_butAlreadyScaledSoLengthFitsGlobalUnits = default;
            Vector3 customVector_unturnedInLocalSpace_expressedInGlobalSpaceUnits;
            if (parentTransform == null)
            {
                customVector_unturnedInLocalSpace_expressedInGlobalSpaceUnits = customVectorToRotate_local;
            }
            else
            {
                customVectorToRotate_local_butAlreadyScaledSoLengthFitsGlobalUnits = Vector3.Scale(parentTransform.lossyScale, customVectorToRotate_local);
                customVector_unturnedInLocalSpace_expressedInGlobalSpaceUnits = rotation_ofLocalSpace * customVectorToRotate_local_butAlreadyScaledSoLengthFitsGlobalUnits;
            }
            Vector3 endPos_ofUnturnedLocalCustomVector_inGlobalSpace = posWhereToDraw + customVector_unturnedInLocalSpace_expressedInGlobalSpaceUnits;

            Color color_forwardLowAlpha = default;
            Color color_forwardLowestAlpha = default;
            Color color_upLowAlpha = default;
            Color color_upLowestAlpha = default;
            Color color_customLowAlpha = default;
            Color color_customLowestAlpha = default;

            Color colorOfCustomRotatedVector = Color.yellow;

            bool drawUpAndForwardVectors = !UtilitiesDXXL_Math.ApproximatelyZero(length_ofUpAndForwardVectors_local);
            if (drawUpAndForwardVectors)
            {
                color_forwardLowAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(UtilitiesDXXL_Colors.blue_zAxisAlpha1, 0.8f);
                color_forwardLowestAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(UtilitiesDXXL_Colors.blue_zAxisAlpha1, 0.6f);
                color_upLowAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(UtilitiesDXXL_Colors.green_yAxisAlpha1, 0.8f);
                color_upLowestAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(UtilitiesDXXL_Colors.green_yAxisAlpha1, 0.3f);
                Color color_forwardDarkened = UtilitiesDXXL_Colors.Get_color_darkenedFromGivenColor(UtilitiesDXXL_Colors.blue_zAxisAlpha1, 2.5f);
                Color color_upDarkened = UtilitiesDXXL_Colors.Get_color_darkenedFromGivenColor(UtilitiesDXXL_Colors.green_yAxisAlpha1, 3.7f);

                string text_atUnrotatedForwardVector;
                string text_atUnrotatedUpVector;
                if (localQuaternion_isIdentity)
                {
                    text_atUnrotatedForwardVector = isLocal ? ("Vector3.forward<br><size=4> </size><br>unrotated<br>=after rotation<br><size=8><size=2>local</size>x = " + forward_unturnedInLocalSpace_inLocalSpaceUnits_scaled.x + "<br><size=2>local</size>y = " + forward_unturnedInLocalSpace_inLocalSpaceUnits_scaled.y + "<br><size=2>local</size>z = " + forward_unturnedInLocalSpace_inLocalSpaceUnits_scaled.z + "</size>") : ("Vector3.forward<br><size=4> </size><br>unrotated<br>=after rotation<br><size=8>x = " + forward_unturnedInLocalSpace_inLocalSpaceUnits_scaled.x + "<br>y = " + forward_unturnedInLocalSpace_inLocalSpaceUnits_scaled.y + "<br>z = " + forward_unturnedInLocalSpace_inLocalSpaceUnits_scaled.z + "</size>");
                    text_atUnrotatedUpVector = isLocal ? ("Vector3.up<br><size=4> </size><br>unrotated<br>=after rotation<br><size=8><size=2>local</size>x = " + up_unturnedInLocalSpace_inLocalSpaceUnits_scaled.x + "<br><size=2>local</size>y = " + up_unturnedInLocalSpace_inLocalSpaceUnits_scaled.y + "<br><size=2>local</size>z = " + up_unturnedInLocalSpace_inLocalSpaceUnits_scaled.z + "</size>") : ("Vector3.up<br><size=4> </size><br>unrotated<br>=after rotation<br><size=8>x = " + up_unturnedInLocalSpace_inLocalSpaceUnits_scaled.x + "<br>y = " + up_unturnedInLocalSpace_inLocalSpaceUnits_scaled.y + "<br>z = " + up_unturnedInLocalSpace_inLocalSpaceUnits_scaled.z + "</size>");
                }
                else
                {
                    text_atUnrotatedForwardVector = isLocal ? ("Vector3.forward<br><size=4> </size><br>unrotated (=identity)<br><size=8><size=2>local</size>x = " + forward_unturnedInLocalSpace_inLocalSpaceUnits_scaled.x + "<br><size=2>local</size>y = " + forward_unturnedInLocalSpace_inLocalSpaceUnits_scaled.y + "<br><size=2>local</size>z = " + forward_unturnedInLocalSpace_inLocalSpaceUnits_scaled.z + "</size>") : ("Vector3.forward<br><size=4> </size><br>unrotated (=identity)<br><size=8>x = " + forward_unturnedInLocalSpace_inLocalSpaceUnits_scaled.x + "<br>y = " + forward_unturnedInLocalSpace_inLocalSpaceUnits_scaled.y + "<br>z = " + forward_unturnedInLocalSpace_inLocalSpaceUnits_scaled.z + "</size>");
                    text_atUnrotatedUpVector = isLocal ? ("Vector3.up<br><size=4> </size><br>unrotated (=identity)<br><size=8><size=2>local</size>x = " + up_unturnedInLocalSpace_inLocalSpaceUnits_scaled.x + "<br><size=2>local</size>y = " + up_unturnedInLocalSpace_inLocalSpaceUnits_scaled.y + "<br><size=2>local</size>z = " + up_unturnedInLocalSpace_inLocalSpaceUnits_scaled.z + "</size>") : ("Vector3.up<br><size=4> </size><br>unrotated (=identity)<br><size=8>x = " + up_unturnedInLocalSpace_inLocalSpaceUnits_scaled.x + "<br>y = " + up_unturnedInLocalSpace_inLocalSpaceUnits_scaled.y + "<br>z = " + up_unturnedInLocalSpace_inLocalSpaceUnits_scaled.z + "</size>");
                }

                UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                DrawBasics.Vector(posWhereToDraw, endPos_ofUnturnedLocalForwardVector_inGlobalSpace, color_forwardDarkened, 0.0f, text_atUnrotatedForwardVector, 0.09f, false, false, default(Vector3), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                DrawBasics.Vector(posWhereToDraw, endPos_ofUnturnedLocalUpwardVector_inGlobalSpace, color_upDarkened, 0.0f, text_atUnrotatedUpVector, 0.09f, false, false, default(Vector3), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();

                //dashed axis extention of unrotated forward/up:
                float lengthOfDashedExtention = 2.0f;

                string text_atZAxis = isLocal ? ("          <size=2>local</size>z axis          ") : ("          z axis          ");
                string text_atYAxis = isLocal ? ("          <size=2>local</size>y axis          ") : ("          y axis          ");

                Line_fadeableAnimSpeed.InternalDraw(endPos_ofUnturnedLocalForwardVector_inGlobalSpace, endPos_ofUnturnedLocalForwardVector_inGlobalSpace + forward_unturnedInLocalSpace_expressedInGlobalSpaceUnits_normalized * lengthOfDashedExtention, color_forwardLowestAlpha, 0.0f, text_atZAxis, DrawBasics.LineStyle.dashedLong, 2.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                Line_fadeableAnimSpeed.InternalDraw(endPos_ofUnturnedLocalUpwardVector_inGlobalSpace, endPos_ofUnturnedLocalUpwardVector_inGlobalSpace + up_unturnedInLocalSpace_expressedInGlobalSpaceUnits_normalized * lengthOfDashedExtention, color_upLowestAlpha, 0.0f, text_atYAxis, DrawBasics.LineStyle.dashedLong, 2.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

                Color color_of90DegSymbolBeforeRotation = Color.Lerp(color_forwardDarkened, color_upDarkened, 0.5f);
                Draw90DegSymbolToQuaternionVectorPair(color_of90DegSymbolBeforeRotation, posWhereToDraw, forward_unturnedInLocalSpace_expressedInGlobalSpaceUnits_scaled, up_unturnedInLocalSpace_expressedInGlobalSpaceUnits_scaled, durationInSec, hiddenByNearerObjects);
                DrawSquareArea_spannedByUpAndForward(false, posWhereToDraw, forward_unturnedInLocalSpace_expressedInGlobalSpaceUnits_scaled, up_unturnedInLocalSpace_expressedInGlobalSpaceUnits_scaled, color_forwardDarkened, color_upDarkened, 0.25f, 0.1f, durationInSec, hiddenByNearerObjects);
            }

            float custom_magnitude = 0.0f;
            if (drawCustomVector)
            {
                custom_magnitude = customVector_unturnedInLocalSpace_expressedInGlobalSpaceUnits.magnitude;
                color_customLowAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorOfCustomRotatedVector, 0.8f);
                color_customLowestAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorOfCustomRotatedVector, 0.3f);
                if (custom_magnitude > lineWidth)
                {
                    string text_atUnrotatedCustomVector;
                    if (localQuaternion_isIdentity)
                    {
                        text_atUnrotatedCustomVector = isLocal ? ("customVector<br><size=4> </size><br>unrotated<br>=after rotation<br><size=8><size=2>local</size>x = " + customVectorToRotate_local.x + "<br><size=2>local</size>y = " + customVectorToRotate_local.y + "<br><size=2>local</size>z = " + customVectorToRotate_local.z + "</size>") : ("customVector<br><size=4> </size><br>unrotated<br>=after rotation<br><size=8>x = " + customVectorToRotate_local.x + "<br>y = " + customVectorToRotate_local.y + "<br>z = " + customVectorToRotate_local.z + "</size>");
                    }
                    else
                    {
                        text_atUnrotatedCustomVector = isLocal ? ("customVector<br><size=4> </size><br>unrotated<br><size=8><size=2>local</size>x = " + customVectorToRotate_local.x + "<br><size=2>local</size>y = " + customVectorToRotate_local.y + "<br><size=2>local</size>z = " + customVectorToRotate_local.z + "</size>") : ("customVector<br><size=4> </size><br>unrotated<br><size=8>x = " + customVectorToRotate_local.x + "<br>y = " + customVectorToRotate_local.y + "<br>z = " + customVectorToRotate_local.z + "</size>");
                    }
                    Color colorOfCustomRotatedVector_darkened = UtilitiesDXXL_Colors.Get_color_darkenedFromGivenColor(colorOfCustomRotatedVector, 3.5f);

                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                    DrawBasics.Vector(posWhereToDraw, endPos_ofUnturnedLocalCustomVector_inGlobalSpace, colorOfCustomRotatedVector_darkened, 0.0f, text_atUnrotatedCustomVector, 0.09f, false, false, default(Vector3), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                }
            }

            if (localQuaternion_isIdentity == false)
            {
                //"forward" + "upward" + "custom" AFTER rotation:
                if (drawUpAndForwardVectors)
                {
                    Vector3 forward_turnedInLocalSpace_inLocalSpaceUnits_scaled = quaternion_local * forward_unturnedInLocalSpace_inLocalSpaceUnits_scaled;
                    Vector3 up_turnedInLocalSpace_inLocalSpaceUnits_scaled = quaternion_local * up_unturnedInLocalSpace_inLocalSpaceUnits_scaled;

                    Vector3 forward_turnedInLocalSpace_expressedInGlobalSpaceUnits_scaled = rotation_ofLocalSpace * forward_turnedInLocalSpace_inLocalSpaceUnits_scaled;
                    Vector3 up_turnedInLocalSpace_expressedInGlobalSpaceUnits_scaled = rotation_ofLocalSpace * up_turnedInLocalSpace_inLocalSpaceUnits_scaled;

                    string text_atRotatedForwardVector = isLocal ? ("Vector3.forward<br><size=4> </size><br>after rotation<br><size=8><size=2>local</size>x = " + forward_turnedInLocalSpace_inLocalSpaceUnits_scaled.x + "<br><size=2>local</size>y = " + forward_turnedInLocalSpace_inLocalSpaceUnits_scaled.y + "<br><size=2>local</size>z = " + forward_turnedInLocalSpace_inLocalSpaceUnits_scaled.z + "</size>") : ("Vector3.forward<br><size=4> </size><br>after rotation<br><size=8>x = " + forward_turnedInLocalSpace_inLocalSpaceUnits_scaled.x + "<br>y = " + forward_turnedInLocalSpace_inLocalSpaceUnits_scaled.y + "<br>z = " + forward_turnedInLocalSpace_inLocalSpaceUnits_scaled.z + "</size>");
                    string text_atRotatedUpVector = isLocal ? ("Vector3.up<br><size=4> </size><br>after rotation<br><size=8><size=2>local</size>x = " + up_turnedInLocalSpace_inLocalSpaceUnits_scaled.x + "<br><size=2>local</size>y = " + up_turnedInLocalSpace_inLocalSpaceUnits_scaled.y + "<br><size=2>local</size>z = " + up_turnedInLocalSpace_inLocalSpaceUnits_scaled.z + "</size>") : ("Vector3.up<br><size=4> </size><br>after rotation<br><size=8>x = " + up_turnedInLocalSpace_inLocalSpaceUnits_scaled.x + "<br>y = " + up_turnedInLocalSpace_inLocalSpaceUnits_scaled.y + "<br>z = " + up_turnedInLocalSpace_inLocalSpaceUnits_scaled.z + "</size>");

                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                    DrawBasics.Vector(posWhereToDraw, posWhereToDraw + forward_turnedInLocalSpace_expressedInGlobalSpaceUnits_scaled, color_forwardLowAlpha, 0.0f, text_atRotatedForwardVector, 0.09f, false, false, default(Vector3), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                    DrawBasics.Vector(posWhereToDraw, posWhereToDraw + up_turnedInLocalSpace_expressedInGlobalSpaceUnits_scaled, color_upLowAlpha, 0.0f, text_atRotatedUpVector, 0.09f, false, false, default(Vector3), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();

                    Color color_of90DegSymbolAfterRotation = Color.Lerp(color_forwardLowAlpha, color_upLowAlpha, 0.5f);
                    Draw90DegSymbolToQuaternionVectorPair(color_of90DegSymbolAfterRotation, posWhereToDraw, forward_turnedInLocalSpace_expressedInGlobalSpaceUnits_scaled, up_turnedInLocalSpace_expressedInGlobalSpaceUnits_scaled, durationInSec, hiddenByNearerObjects);
                    DrawSquareArea_spannedByUpAndForward(true, posWhereToDraw, forward_turnedInLocalSpace_expressedInGlobalSpaceUnits_scaled, up_turnedInLocalSpace_expressedInGlobalSpaceUnits_scaled, color_forwardLowAlpha, color_upLowAlpha, 0.25f, 0.1f, durationInSec, hiddenByNearerObjects);
                }

                if (drawCustomVector)
                {
                    if (custom_magnitude > lineWidth)
                    {
                        Vector3 customVector_turnedInLocalSpace_inLocalSpaceUnits = quaternion_local * customVectorToRotate_local;
                        string text_atRotatedCustomVector = isLocal ? ("customVector<br><size=4> </size><br>after rotation<br><size=8><size=2>local</size>x = " + customVector_turnedInLocalSpace_inLocalSpaceUnits.x + "<br><size=2>local</size>y = " + customVector_turnedInLocalSpace_inLocalSpaceUnits.y + "<br><size=2>local</size>z = " + customVector_turnedInLocalSpace_inLocalSpaceUnits.z + "</size>") : ("customVector<br><size=4> </size><br>after rotation<br><size=8>x = " + customVector_turnedInLocalSpace_inLocalSpaceUnits.x + "<br>y = " + customVector_turnedInLocalSpace_inLocalSpaceUnits.y + "<br>z = " + customVector_turnedInLocalSpace_inLocalSpaceUnits.z + "</size>");

                        Vector3 customVector_turnedInLocalSpace_expressedInGlobalSpaceUnits;
                        if (parentTransform == null)
                        {
                            customVector_turnedInLocalSpace_expressedInGlobalSpaceUnits = customVector_turnedInLocalSpace_inLocalSpaceUnits;
                        }
                        else
                        {
                            customVector_turnedInLocalSpace_expressedInGlobalSpaceUnits = rotation_ofLocalSpace * quaternion_local * customVectorToRotate_local_butAlreadyScaledSoLengthFitsGlobalUnits;
                        }

                        UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                        DrawBasics.Vector(posWhereToDraw, posWhereToDraw + customVector_turnedInLocalSpace_expressedInGlobalSpaceUnits, color_customLowAlpha, 0.0f, text_atRotatedCustomVector, 0.09f, false, false, default(Vector3), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                        UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                    }
                }

                //circles/circled vectors:
                localTurnAxisLine_inGlobalSpaceUnits.Recreate(posWhereToDraw, localTurnAxis_expressedInGlobalSpaceUnits_normalized, true);

                if (drawUpAndForwardVectors)
                {
                    DrawCircleInclVector_forTurnedVector(posWhereToDraw, endPos_ofUnturnedLocalForwardVector_inGlobalSpace, localTurnAxis_expressedInGlobalSpaceUnits_normalized, localTurnAngleDeg, UtilitiesDXXL_Colors.blue_zAxisAlpha1, color_forwardLowestAlpha, lineWidth, durationInSec, hiddenByNearerObjects);
                    DrawCircleInclVector_forTurnedVector(posWhereToDraw, endPos_ofUnturnedLocalUpwardVector_inGlobalSpace, localTurnAxis_expressedInGlobalSpaceUnits_normalized, localTurnAngleDeg, UtilitiesDXXL_Colors.green_yAxisAlpha1, color_upLowestAlpha, lineWidth, durationInSec, hiddenByNearerObjects);
                }

                if (drawCustomVector)
                {
                    DrawCircleInclVector_forTurnedVector(posWhereToDraw, endPos_ofUnturnedLocalCustomVector_inGlobalSpace, localTurnAxis_expressedInGlobalSpaceUnits_normalized, localTurnAngleDeg, colorOfCustomRotatedVector, color_customLowestAlpha, lineWidth, durationInSec, hiddenByNearerObjects);
                }

                //"forward" + "upward" + "custom" startPosOfRotation:
                if (drawUpAndForwardVectors)
                {
                    DrawShapes.Sphere(endPos_ofUnturnedLocalForwardVector_inGlobalSpace, 1.0f * lineWidth, UtilitiesDXXL_Colors.blue_zAxisAlpha1, forward_unturnedInLocalSpace_expressedInGlobalSpaceUnits_normalized, default(Vector3), 0.0f, null, 8, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);
                    DrawShapes.Sphere(endPos_ofUnturnedLocalUpwardVector_inGlobalSpace, 1.0f * lineWidth, UtilitiesDXXL_Colors.green_yAxisAlpha1, up_unturnedInLocalSpace_expressedInGlobalSpaceUnits_normalized, default(Vector3), 0.0f, null, 8, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);
                }

                if (drawCustomVector)
                {
                    DrawShapes.Sphere(endPos_ofUnturnedLocalCustomVector_inGlobalSpace, 1.0f * lineWidth, colorOfCustomRotatedVector, default(Vector3), default(Vector3), 0.0f, null, 8, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);
                }
            }
        }

        public static void Draw90DegSymbolToQuaternionVectorPair(Color color, Vector3 quaternionCenterPos, Vector3 forwardVector_scaled, Vector3 upVector_scaled, float durationInSec, bool hiddenByNearerObjects)
        {
            float relSizeOf90degSymbol = 0.05f;
            Vector3 deg90_vertexOn_forwardVector = quaternionCenterPos + forwardVector_scaled * relSizeOf90degSymbol;
            Vector3 deg90_vertexOn_upVector = quaternionCenterPos + upVector_scaled * relSizeOf90degSymbol;
            Vector3 deg90_vertexAtKink = quaternionCenterPos + forwardVector_scaled * relSizeOf90degSymbol + upVector_scaled * relSizeOf90degSymbol;
            Line_fadeableAnimSpeed.InternalDraw(deg90_vertexOn_forwardVector, deg90_vertexAtKink, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(deg90_vertexOn_upVector, deg90_vertexAtKink, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
        }

        public static void DrawSquareArea_spannedByUpAndForward(bool weakenMostOuterGreenLine, Vector3 quaternionCenterPos, Vector3 forwardVector_scaled, Vector3 upVector_scaled, Color color_ofSpanningForwardVector, Color color_ofSpanningUpVector, float alphaFactor, float alphaFactor_weakenedOverdraw, float durationInSec, bool hiddenByNearerObjects)
        {
            int linesPerSide = 10;

            Color color_ofSpanningForwardVector_lowerAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofSpanningForwardVector, alphaFactor);
            Color color_ofSpanningUpVector_lowerAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofSpanningUpVector, alphaFactor);

            float lengthFactorBetweenLines = 1.0f / (float)linesPerSide;

            Vector3 startPos_ofCurrForwardParallelLine = default;
            Vector3 endPos_ofCurrForwardParallelLine = default;
            Vector3 startPos_ofCurrUpParallelLine = default;
            Vector3 endPos_ofCurrUpParallelLine = default;

            for (int i = 0; i < linesPerSide; i++)
            {
                startPos_ofCurrForwardParallelLine = quaternionCenterPos + upVector_scaled * (lengthFactorBetweenLines * (i + 1));
                endPos_ofCurrForwardParallelLine = startPos_ofCurrForwardParallelLine + forwardVector_scaled;
                Line_fadeableAnimSpeed.InternalDraw(startPos_ofCurrForwardParallelLine, endPos_ofCurrForwardParallelLine, color_ofSpanningForwardVector_lowerAlpha, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

                startPos_ofCurrUpParallelLine = quaternionCenterPos + forwardVector_scaled * (lengthFactorBetweenLines * (i + 1));
                endPos_ofCurrUpParallelLine = startPos_ofCurrUpParallelLine + upVector_scaled;
                Line_fadeableAnimSpeed.InternalDraw(startPos_ofCurrUpParallelLine, endPos_ofCurrUpParallelLine, color_ofSpanningUpVector_lowerAlpha, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            //most outer line has higher alpha (realised via this second overdraw):
            Line_fadeableAnimSpeed.InternalDraw(startPos_ofCurrForwardParallelLine, endPos_ofCurrForwardParallelLine, color_ofSpanningForwardVector_lowerAlpha, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            if (weakenMostOuterGreenLine) //case of quaternion: the green outer line of the turned sqaure appears slighly to dominant if the same strong overdraw alpha is used, therefore this weakening:
            {
                color_ofSpanningUpVector_lowerAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofSpanningUpVector, alphaFactor_weakenedOverdraw);
            }
            Line_fadeableAnimSpeed.InternalDraw(startPos_ofCurrUpParallelLine, endPos_ofCurrUpParallelLine, color_ofSpanningUpVector_lowerAlpha, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
        }

        static void DrawCircleInclVector_forTurnedVector(Vector3 posWhereToDraw, Vector3 endPos_ofUnturnedVector_inGlobalSpaceUnits, Vector3 localTurnAxis_expressedInGlobalSpaceUnits_normalized, float turnAngleDeg, Color color_ofThickVectorCircled, Color color_ofThinFullCircle, float lineWidth, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 plumbPos_ofUnturnedVectorOnTurnAxis_inGlobalSpaceUnits = localTurnAxisLine_inGlobalSpaceUnits.Get_perpProjectionOfPoint_ontoThisLine(endPos_ofUnturnedVector_inGlobalSpaceUnits);
            Vector3 fromPlumbPos_toUnturnedVectorsPeak_inGlobalSpaceUnits = endPos_ofUnturnedVector_inGlobalSpaceUnits - plumbPos_ofUnturnedVectorOnTurnAxis_inGlobalSpaceUnits;
            float radius = fromPlumbPos_toUnturnedVectorsPeak_inGlobalSpaceUnits.magnitude;
            if (radius > 0.01f)
            {
                DrawShapes.Circle(plumbPos_ofUnturnedVectorOnTurnAxis_inGlobalSpaceUnits, radius, color_ofThinFullCircle, localTurnAxis_expressedInGlobalSpaceUnits_normalized, fromPlumbPos_toUnturnedVectorsPeak_inGlobalSpaceUnits, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);

                float abs_turnAngleDeg = Math.Abs(turnAngleDeg);
                if (abs_turnAngleDeg >= 0.5f)
                {
                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forCircledVectors_reversible(DrawBasics.LengthInterpretation.absoluteUnits);
                    DrawBasics.VectorCircled(endPos_ofUnturnedVector_inGlobalSpaceUnits, posWhereToDraw, localTurnAxis_expressedInGlobalSpaceUnits_normalized, turnAngleDeg, color_ofThickVectorCircled, lineWidth, null, 0.11f, false, false, false, 0.0f, DrawText.TextAnchorCircledDXXL.LowerLeftOfFirstLine, durationInSec, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forCircledVectors();
                }
            }
        }

    }

}
