namespace DrawXXL
{
    using UnityEngine;

    public class UtilitiesDXXL_TextTagForPointCollection
    {
        public static void TagPointCollection(string text, string headerText, Vector3 position, int usedSlotsInVerticesLocalList, float linesWidth, float textScalingFactor, Color colorForLinesAndHeader, Color colorForText, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 virtualTransformScale = new Vector3(textScalingFactor, textScalingFactor, textScalingFactor);
            TagPointCollection(text, headerText, position, usedSlotsInVerticesLocalList, linesWidth, virtualTransformScale, colorForLinesAndHeader, colorForText, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void TagPointCollection(string text, string headerText, Vector3 position, int usedSlotsInVerticesLocalList, float linesWidth, Vector3 scaleOfTaggedTransform, Color colorForLinesAndHeader, Color colorForText, bool textBlockAboveLine, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (usedSlotsInVerticesLocalList <= 0)
            {
                UtilitiesDXXL_Log.PrintErrorCode("26-" + usedSlotsInVerticesLocalList + "(no textTagDrawing)");
                return;
            }

            if ((text != null && text != "") || (headerText != null && headerText != ""))
            {
                GetTextPosAndOrientation(out Vector3 lineKinkWhereTextStarts_local, out Vector3 nearestVertexToText_local, out Vector3 textDir_normalized, out Vector3 textUp_normalized, usedSlotsInVerticesLocalList, position);
                float biggestAbsDim = UtilitiesDXXL_Math.GetBiggestAbsComponent(scaleOfTaggedTransform);
                biggestAbsDim = Mathf.Max(biggestAbsDim, 0.0001f);
                Vector3 approxTextPosition = position + lineKinkWhereTextStarts_local;
                float textSize_unclamped = Get_textSize_unclamped(biggestAbsDim, approxTextPosition);
                float minTextSize = 0.01f;
                float textSize = Mathf.Max(textSize_unclamped, minTextSize);
                float maxWidthOfLinesTowardsText = textSize * 0.05f;
                float widthOfLinesTowardsText = linesWidth;
                widthOfLinesTowardsText = Mathf.Min(widthOfLinesTowardsText, maxWidthOfLinesTowardsText);
                Color color_ofConnectionLine = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(colorForLinesAndHeader, 0.5f);
                Line_fadeableAnimSpeed.InternalDraw(position + nearestVertexToText_local, position + lineKinkWhereTextStarts_local, color_ofConnectionLine, widthOfLinesTowardsText, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

                float lengthOfHorizLine = textSize;
                Vector3 textPosition = position + lineKinkWhereTextStarts_local + textUp_normalized * (0.5f * widthOfLinesTowardsText + 0.32f * textSize);
                if (text != null && text != "")
                {
                    DrawText.TextAnchorDXXL textAnchor = textBlockAboveLine ? DrawText.TextAnchorDXXL.LowerLeft : DrawText.TextAnchorDXXL.UpperLeft;
                    UtilitiesDXXL_Text.Write(text, textPosition, colorForText, textSize, textDir_normalized, textUp_normalized, textAnchor, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                    float lengthOfLongestLine_inText = DrawText.parsedTextSpecs.widthOfLongestLine;
                    lengthOfHorizLine = Mathf.Max(lengthOfLongestLine_inText, textSize);
                }

                if (headerText != null && headerText != "")
                {
                    //no strokeWidth-markup: trading execution time and code readability for GC.Alloc()-prevention:
                    DrawText.TextAnchorDXXL textAnchor_forHeader = textBlockAboveLine ? DrawText.TextAnchorDXXL.UpperLeft : DrawText.TextAnchorDXXL.LowerLeft;
                    Vector3 offsetForDoubledPrint = textDir_normalized * textSize * 0.11f;
                    Vector3 offsetForTripledPrint = textDir_normalized * textSize * 0.055f + textUp_normalized * textSize * 0.08f;
                    UtilitiesDXXL_Text.Write(headerText, textPosition, colorForLinesAndHeader, textSize, textDir_normalized, textUp_normalized, textAnchor_forHeader, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                    float lengthOfLongestLine_inHeaderText = DrawText.parsedTextSpecs.widthOfLongestLine;
                    UtilitiesDXXL_Text.Write(headerText, textPosition + offsetForDoubledPrint, colorForLinesAndHeader, textSize, textDir_normalized, textUp_normalized, textAnchor_forHeader, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                    UtilitiesDXXL_Text.Write(headerText, textPosition + offsetForTripledPrint, colorForLinesAndHeader, textSize, textDir_normalized, textUp_normalized, textAnchor_forHeader, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
                    lengthOfHorizLine = Mathf.Max(lengthOfLongestLine_inHeaderText, lengthOfHorizLine);
                }

                Line_fadeableAnimSpeed.InternalDraw(position + lineKinkWhereTextStarts_local, position + lineKinkWhereTextStarts_local + textDir_normalized * lengthOfHorizLine, color_ofConnectionLine, widthOfLinesTowardsText, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }
        }

        static void GetTextPosAndOrientation(out Vector3 lineKinkWhereTextStarts_local, out Vector3 nearestVertexToText_local, out Vector3 textDir_normalized, out Vector3 textUp_normalized, int usedSlotsInVerticesLocalList, Vector3 position)
        {
            switch (DrawText.automaticTextOrientation)
            {
                case DrawText.AutomaticTextOrientation.screen:
                    GetTextPosAndOrientation_forAutoOrientationCase_screen(out lineKinkWhereTextStarts_local, out nearestVertexToText_local, out textDir_normalized, out textUp_normalized, usedSlotsInVerticesLocalList, position);
                    return;
                case DrawText.AutomaticTextOrientation.screen_butVerticalInWorldSpace:
                    GetTextPosAndOrientation_forAutoOrientationCase_screen_butVerticalInWorldSpace(out lineKinkWhereTextStarts_local, out nearestVertexToText_local, out textDir_normalized, out textUp_normalized, usedSlotsInVerticesLocalList, position);
                    return;
                case DrawText.AutomaticTextOrientation.xyPlane:
                    GetTextPosAndOrientation_forAutoOrientationCase_xyPlane(out lineKinkWhereTextStarts_local, out nearestVertexToText_local, out textDir_normalized, out textUp_normalized, usedSlotsInVerticesLocalList);
                    return;
                case DrawText.AutomaticTextOrientation.xzPlane:
                    GetTextPosAndOrientation_forAutoOrientationCase_xzPlane(out lineKinkWhereTextStarts_local, out nearestVertexToText_local, out textDir_normalized, out textUp_normalized, usedSlotsInVerticesLocalList);
                    return;
                case DrawText.AutomaticTextOrientation.zyPlane:
                    GetTextPosAndOrientation_forAutoOrientationCase_zyPlane(out lineKinkWhereTextStarts_local, out nearestVertexToText_local, out textDir_normalized, out textUp_normalized, usedSlotsInVerticesLocalList);
                    return;
                default:
                    Debug.LogError("DrawText.automaticTextOrientation of " + DrawText.automaticTextOrientation + " is not implemented.");
                    GetTextPosAndOrientation_forAutoOrientationCase_xyPlane(out lineKinkWhereTextStarts_local, out nearestVertexToText_local, out textDir_normalized, out textUp_normalized, usedSlotsInVerticesLocalList);
                    return;
            }
        }

        static void GetTextPosAndOrientation_forAutoOrientationCase_screen(out Vector3 lineKinkWhereTextStarts_local, out Vector3 nearestVertexToText_local, out Vector3 textDir_normalized, out Vector3 textUp_normalized, int usedSlotsInVerticesLocalList, Vector3 position)
        {
            UtilitiesDXXL_TextDirAndUpCalculation.GetTextDirAndUpNormalized(out textDir_normalized, out textUp_normalized, default(Vector3), default(Vector3), position, false, false);
            Vector3 virtualRawPosToWhichTextShouldHead_local = textDir_normalized * 1000.0f + Vector3.up; //-> adding "Vector3.up" so that the bistable horizontal case consistently chooses the upper vertices over the lower ones
            nearestVertexToText_local = UtilitiesDXXL_Math.GetNearestVertex(virtualRawPosToWhichTextShouldHead_local, UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            float positiveXEnd_local = UtilitiesDXXL_Math.GetHighestXComponent(UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            float positiveYEnd_local = UtilitiesDXXL_Math.GetHighestYComponent(UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            float positiveZEnd_local = UtilitiesDXXL_Math.GetHighestZComponent(UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            float textOffsetDistance = UtilitiesDXXL_Math.Max(positiveXEnd_local, positiveYEnd_local, positiveZEnd_local) * 0.6f;
            Vector3 observerCamera_forward = Vector3.Cross(textDir_normalized, textUp_normalized);
            Quaternion rotation_aroundObserverCamForward_kinkingDirTowardsText = Quaternion.AngleAxis(45.0f, observerCamera_forward);
            Vector3 offsetDir_normalized = rotation_aroundObserverCamForward_kinkingDirTowardsText * textDir_normalized;
            lineKinkWhereTextStarts_local = nearestVertexToText_local + offsetDir_normalized * textOffsetDistance;
        }

        static void GetTextPosAndOrientation_forAutoOrientationCase_screen_butVerticalInWorldSpace(out Vector3 lineKinkWhereTextStarts_local, out Vector3 nearestVertexToText_local, out Vector3 textDir_normalized, out Vector3 textUp_normalized, int usedSlotsInVerticesLocalList, Vector3 position)
        {
            UtilitiesDXXL_TextDirAndUpCalculation.GetTextDirAndUpNormalized(out textDir_normalized, out textUp_normalized, default(Vector3), default(Vector3), position, false, false);
            Vector3 virtualRawPosToWhichTextShouldHead_local = textDir_normalized * 1000.0f + Vector3.up;//-> adding "Vector3.up" so that the bistable horizontal case consistently chooses the upper vertices over the lower ones
            nearestVertexToText_local = UtilitiesDXXL_Math.GetNearestVertex(virtualRawPosToWhichTextShouldHead_local, UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            float positiveXEnd_local = UtilitiesDXXL_Math.GetHighestXComponent(UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            float positiveYEnd_local = UtilitiesDXXL_Math.GetHighestYComponent(UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            float textOffsetDistance = Mathf.Max(positiveXEnd_local, positiveYEnd_local) * 0.6f;
            Vector3 textOffset_unscaled = new Vector3(textDir_normalized.x, 1.0f, textDir_normalized.z);
            lineKinkWhereTextStarts_local = nearestVertexToText_local + textOffset_unscaled * textOffsetDistance;
        }

        static void GetTextPosAndOrientation_forAutoOrientationCase_xyPlane(out Vector3 lineKinkWhereTextStarts_local, out Vector3 nearestVertexToText_local, out Vector3 textDir_normalized, out Vector3 textUp_normalized, int usedSlotsInVerticesLocalList)
        {
            float positiveXEnd_local = UtilitiesDXXL_Math.GetHighestXComponent(UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            float positiveYEnd_local = UtilitiesDXXL_Math.GetHighestYComponent(UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            float negativeZEnd_local = UtilitiesDXXL_Math.GetLowestZComponent(UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            Vector3 textOffset = new Vector3(positiveXEnd_local * 0.5f, positiveYEnd_local * 0.5f, 0.0f);
            lineKinkWhereTextStarts_local = new Vector3(positiveXEnd_local, positiveYEnd_local, negativeZEnd_local) + textOffset;
            nearestVertexToText_local = UtilitiesDXXL_Math.GetNearestVertex(lineKinkWhereTextStarts_local, UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            textDir_normalized = Vector3.right;
            textUp_normalized = Vector3.up;
        }

        static void GetTextPosAndOrientation_forAutoOrientationCase_xzPlane(out Vector3 lineKinkWhereTextStarts_local, out Vector3 nearestVertexToText_local, out Vector3 textDir_normalized, out Vector3 textUp_normalized, int usedSlotsInVerticesLocalList)
        {
            float positiveXEnd_local = UtilitiesDXXL_Math.GetHighestXComponent(UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            float positiveYEnd_local = UtilitiesDXXL_Math.GetHighestYComponent(UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            float positiveZEnd_local = UtilitiesDXXL_Math.GetLowestZComponent(UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            Vector3 textOffset = new Vector3(positiveXEnd_local * 0.5f, 0.0f, positiveZEnd_local * 0.5f);
            lineKinkWhereTextStarts_local = new Vector3(positiveXEnd_local, positiveYEnd_local, positiveZEnd_local) + textOffset;
            nearestVertexToText_local = UtilitiesDXXL_Math.GetNearestVertex(lineKinkWhereTextStarts_local, UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            textDir_normalized = Vector3.right;
            textUp_normalized = Vector3.forward;
        }

        static void GetTextPosAndOrientation_forAutoOrientationCase_zyPlane(out Vector3 lineKinkWhereTextStarts_local, out Vector3 nearestVertexToText_local, out Vector3 textDir_normalized, out Vector3 textUp_normalized, int usedSlotsInVerticesLocalList)
        {
            float negativeXEnd_local = UtilitiesDXXL_Math.GetLowestXComponent(UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            float positiveYEnd_local = UtilitiesDXXL_Math.GetHighestYComponent(UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            float negativeZEnd_local = UtilitiesDXXL_Math.GetLowestZComponent(UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            Vector3 textOffset = new Vector3(0.0f, positiveYEnd_local * 0.5f, negativeZEnd_local * 0.5f);
            lineKinkWhereTextStarts_local = new Vector3(negativeXEnd_local, positiveYEnd_local, negativeZEnd_local) + textOffset;
            nearestVertexToText_local = UtilitiesDXXL_Math.GetNearestVertex(lineKinkWhereTextStarts_local, UtilitiesDXXL_Shapes.verticesLocal, usedSlotsInVerticesLocalList);
            textDir_normalized = Vector3.back;
            textUp_normalized = Vector3.up;
        }

        static float Get_textSize_unclamped(float biggestAbsDim, Vector3 approxTextPosition)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes) == false)
            {
                if (DrawBasics.cameraForAutomaticOrientation == DrawBasics.CameraForAutomaticOrientation.sceneViewCamera)
                {
#if UNITY_EDITOR
                    if (UnityEditor.SceneView.lastActiveSceneView != null)
                    {
                        float distanceFromCam = (approxTextPosition - UnityEditor.SceneView.lastActiveSceneView.camera.transform.position).magnitude;
                        float lengthOfScreenHeight_atDrawnObjectsPosition = UtilitiesDXXL_Screenspace.Get_vertExtentOfViewport_at_distanceFromCam(UnityEditor.SceneView.lastActiveSceneView.camera, distanceFromCam);
                        return lengthOfScreenHeight_atDrawnObjectsPosition * DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes;
                    }
                    else
                    {
                        return Get_textSizeRelativeToPointCollectionExtent(biggestAbsDim);
                    }
#else
                            return Get_textSizeRelativeToPointCollectionExtent(biggestAbsDim);
#endif
                }
                else
                {
                    bool gameviewCameraIsAvailable = UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out Camera gameviewCameraForDrawing, null, false);
                    if (gameviewCameraIsAvailable)
                    {
                        float distanceFromCam = (approxTextPosition - gameviewCameraForDrawing.transform.position).magnitude;
                        float lengthOfScreenHeight_atDrawnObjectsPosition = UtilitiesDXXL_Screenspace.Get_vertExtentOfViewport_at_distanceFromCam(gameviewCameraForDrawing, distanceFromCam);
                        return lengthOfScreenHeight_atDrawnObjectsPosition * DrawShapes.forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes;
                    }
                    else
                    {
                        return Get_textSizeRelativeToPointCollectionExtent(biggestAbsDim);
                    }
                }
            }
            else
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes) == false)
                {
                    return DrawShapes.forcedConstantWorldspaceTextSize_forTextAtShapes;
                }
                else
                {
                    return Get_textSizeRelativeToPointCollectionExtent(biggestAbsDim);
                }
            }
        }

        static float Get_textSizeRelativeToPointCollectionExtent(float biggestAbsDim)
        {
            return (biggestAbsDim * 0.2f);
        }

    }

}
