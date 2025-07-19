namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class UtilitiesDXXL_DrawCollections
    {
        public delegate string FlexibleGetColumnContentAtIndexAsString<T>(T collection, int i_whereToObtain);
        static bool saveDrawnLines_forBoolCollections = false;
        static int boolCollectionLength_aboveWhichDrawnLinesAreSaved = 20;

        public static string GetBoolAsStringFromArray(bool[] boolArray, int i_whereToObtain)
        {
            return DrawText.MarkupBoolDisplayer(boolArray[i_whereToObtain], saveDrawnLines_forBoolCollections);
        }

        public static string GetBoolAsStringFromList(List<bool> boolList, int i_whereToObtain)
        {
            return DrawText.MarkupBoolDisplayer(boolList[i_whereToObtain], saveDrawnLines_forBoolCollections);
        }

        public static string GetIntAsStringFromArray(int[] intArray, int i_whereToObtain)
        {
            return "" + intArray[i_whereToObtain];
        }

        public static string GetIntAsStringFromList(List<int> intList, int i_whereToObtain)
        {
            return "" + intList[i_whereToObtain];
        }

        public static string GetFloatAsStringFromArray(float[] floatArray, int i_whereToObtain)
        {
            return "" + floatArray[i_whereToObtain];
        }

        public static string GetFloatAsStringFromList(List<float> floatList, int i_whereToObtain)
        {
            return "" + floatList[i_whereToObtain];
        }

        public static string GetStringFromArray(string[] stringArray, int i_whereToObtain)
        {
            return stringArray[i_whereToObtain];
        }

        public static string GetStringFromList(List<string> stringList, int i_whereToObtain)
        {
            return stringList[i_whereToObtain];
        }

        //Vector2:
        public static string GetVector2XAsStringFromArray(Vector2[] vector2Array, int i_whereToObtain)
        {
            return "" + vector2Array[i_whereToObtain].x;
        }

        public static string GetVector2XAsStringFromList(List<Vector2> vector2List, int i_whereToObtain)
        {
            return "" + vector2List[i_whereToObtain].x;
        }

        public static string GetVector2YAsStringFromArray(Vector2[] vector2Array, int i_whereToObtain)
        {
            return "" + vector2Array[i_whereToObtain].y;
        }

        public static string GetVector2YAsStringFromList(List<Vector2> vector2List, int i_whereToObtain)
        {
            return "" + vector2List[i_whereToObtain].y;
        }

        //Vector3:
        public static string GetVector3XAsStringFromArray(Vector3[] vector3Array, int i_whereToObtain)
        {
            return "" + vector3Array[i_whereToObtain].x;
        }

        public static string GetVector3XAsStringFromList(List<Vector3> vector3List, int i_whereToObtain)
        {
            return "" + vector3List[i_whereToObtain].x;
        }

        public static string GetVector3YAsStringFromArray(Vector3[] vector3Array, int i_whereToObtain)
        {
            return "" + vector3Array[i_whereToObtain].y;
        }

        public static string GetVector3YAsStringFromList(List<Vector3> vector3List, int i_whereToObtain)
        {
            return "" + vector3List[i_whereToObtain].y;
        }

        public static string GetVector3ZAsStringFromArray(Vector3[] vector3Array, int i_whereToObtain)
        {
            return "" + vector3Array[i_whereToObtain].z;
        }

        public static string GetVector3ZAsStringFromList(List<Vector3> vector3List, int i_whereToObtain)
        {
            return "" + vector3List[i_whereToObtain].z;
        }

        //Vector4:
        public static string GetVector4XAsStringFromArray(Vector4[] vector4Array, int i_whereToObtain)
        {
            return "" + vector4Array[i_whereToObtain].x;
        }

        public static string GetVector4XAsStringFromList(List<Vector4> vector4List, int i_whereToObtain)
        {
            return "" + vector4List[i_whereToObtain].x;
        }

        public static string GetVector4YAsStringFromArray(Vector4[] vector4Array, int i_whereToObtain)
        {
            return "" + vector4Array[i_whereToObtain].y;
        }

        public static string GetVector4YAsStringFromList(List<Vector4> vector4List, int i_whereToObtain)
        {
            return "" + vector4List[i_whereToObtain].y;
        }

        public static string GetVector4ZAsStringFromArray(Vector4[] vector4Array, int i_whereToObtain)
        {
            return "" + vector4Array[i_whereToObtain].z;
        }

        public static string GetVector4ZAsStringFromList(List<Vector4> vector4List, int i_whereToObtain)
        {
            return "" + vector4List[i_whereToObtain].z;
        }

        public static string GetVector4WAsStringFromArray(Vector4[] vector4Array, int i_whereToObtain)
        {
            return "" + vector4Array[i_whereToObtain].w;
        }

        public static string GetVector4WAsStringFromList(List<Vector4> vector4List, int i_whereToObtain)
        {
            return "" + vector4List[i_whereToObtain].w;
        }

        public static void WriteCollection_in3D<CollectionType>(CollectionType collectionToDraw, int countOfCollection, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn1ContentAsString, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn2ContentAsString, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn3ContentAsString, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn4ContentAsString, string nameOfColumn1, string nameOfColumn2, string nameOfColumn3, string nameOfColumn4, Vector3 position, bool position_isTopLeft_notLowLeft, float forceHeightOfWholeTableBox, float textSize, Color color, Quaternion rotation, string title, string titleFallback, bool collectionRepresentsBools, float durationInSec, bool hiddenByNearerObjects)
        {
            bool autoFlipToPreventMirrorInverted = true;
            WriteCollection(collectionToDraw, countOfCollection, GetColumn1ContentAsString, GetColumn2ContentAsString, GetColumn3ContentAsString, GetColumn4ContentAsString, nameOfColumn1, nameOfColumn2, nameOfColumn3, nameOfColumn4, position, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, autoFlipToPreventMirrorInverted, color, rotation, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteCollection_in2D<CollectionType>(CollectionType collectionToDraw, int countOfCollection, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn1ContentAsString, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn2ContentAsString, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn3ContentAsString, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn4ContentAsString, string nameOfColumn1, string nameOfColumn2, string nameOfColumn3, string nameOfColumn4, Vector2 position, bool position_isTopLeft_notLowLeft, float forceHeightOfWholeTableBox, float textSize, Color color, float custom_zPos, string title, string titleFallback, bool collectionRepresentsBools, float durationInSec, bool hiddenByNearerObjects)
        {
            float zPos = UtilitiesDXXL_DrawBasics2D.TryFallbackToDefaultZ(custom_zPos);
            Vector3 positionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(position, zPos);
            bool autoFlipToPreventMirrorInverted = true;
            WriteCollection(collectionToDraw, countOfCollection, GetColumn1ContentAsString, GetColumn2ContentAsString, GetColumn3ContentAsString, GetColumn4ContentAsString, nameOfColumn1, nameOfColumn2, nameOfColumn3, nameOfColumn4, positionV3, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox, textSize, autoFlipToPreventMirrorInverted, color, Quaternion.identity, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        public static void WriteCollection_inScreenspace<CollectionType>(Camera screenCamera, CollectionType collectionToDraw, int countOfCollection, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn1ContentAsString, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn2ContentAsString, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn3ContentAsString, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn4ContentAsString, string nameOfColumn1, string nameOfColumn2, string nameOfColumn3, string nameOfColumn4, Vector2 position_in2DViewportSpace, bool position_isTopLeft_notLowLeft, float forceHeightOfWholeTableBox_relToViewportHeight, float textSize_relToViewportHeight, Color color, string title, string titleFallback, bool collectionRepresentsBools, float durationInSec)
        {
            Vector3 positionV3 = UtilitiesDXXL_Screenspace.ViewportSpacePos_to_WorldPosOnDrawPlane(screenCamera, position_in2DViewportSpace, false);
            float textSize_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(screenCamera, position_in2DViewportSpace, true, textSize_relToViewportHeight);
            float forceHeightOfWholeTableBox_worldSpace = 0.0f;
            if (UtilitiesDXXL_Math.ApproximatelyZero(forceHeightOfWholeTableBox_relToViewportHeight) == false)
            {
                forceHeightOfWholeTableBox_worldSpace = UtilitiesDXXL_Screenspace.VertExtentInsideViewportSpace_to_WorldSpaceExtentOnDrawPlane(screenCamera, position_in2DViewportSpace, true, forceHeightOfWholeTableBox_relToViewportHeight);
            }
            Quaternion rotation = screenCamera.transform.rotation;
            bool autoFlipToPreventMirrorInverted = false;
            bool hiddenByNearerObjects = false;
            WriteCollection(collectionToDraw, countOfCollection, GetColumn1ContentAsString, GetColumn2ContentAsString, GetColumn3ContentAsString, GetColumn4ContentAsString, nameOfColumn1, nameOfColumn2, nameOfColumn3, nameOfColumn4, positionV3, position_isTopLeft_notLowLeft, forceHeightOfWholeTableBox_worldSpace, textSize_worldSpace, autoFlipToPreventMirrorInverted, color, rotation, title, titleFallback, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
        }

        static void WriteCollection<CollectionType>(CollectionType collectionToDraw, int countOfCollection, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn1ContentAsString, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn2ContentAsString, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn3ContentAsString, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn4ContentAsString, string nameOfColumn1, string nameOfColumn2, string nameOfColumn3, string nameOfColumn4, Vector3 position, bool position_isTopLeft_notLowLeft, float forceHeightOfWholeTableBox, float textSize, bool autoFlipToPreventMirrorInverted, Color color, Quaternion rotation, string title, string titleFallback, bool collectionRepresentsBools, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(textSize, "textSize")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(forceHeightOfWholeTableBox, "forceHeightOfWholeTableBox")) { return; }

            saveDrawnLines_forBoolCollections = (countOfCollection > boolCollectionLength_aboveWhichDrawnLinesAreSaved); //-> has only effect on bool collection. Doesn't harm other collections.
            color = UtilitiesDXXL_Colors.OverwriteDefaultColor(color);
            Color color_ofBoundaryLines = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.5f);
            Color color_ofLowerAlphaHorizLines = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.2f);
            int numberOfAllLinesInclTitles = countOfCollection + 2; //"2" -> "title line" and "column titles line"
            float factor_textSize_to_paddingSize = 1.15f;
            bool heightOfWholeOutlineBox_isForced = (UtilitiesDXXL_Math.ApproximatelyZero(forceHeightOfWholeTableBox) == false);
            float factor_letterWidth_to_lineHeight = 2.9f; //is similar to as "UtilitiesDXXL_Text.relLineDistance"

            float used_textSize;
            if (heightOfWholeOutlineBox_isForced)
            {
                forceHeightOfWholeTableBox = Mathf.Abs(forceHeightOfWholeTableBox);
                float linesThatNeedToFitInWholeOutline = (float)countOfCollection + 2.0f + (factor_textSize_to_paddingSize / factor_letterWidth_to_lineHeight);
                used_textSize = forceHeightOfWholeTableBox / (linesThatNeedToFitInWholeOutline * factor_letterWidth_to_lineHeight);
            }
            else
            {
                used_textSize = Mathf.Max(textSize, 2.0f * UtilitiesDXXL_Text.minTextSize);
            }

            float height_fromLineToLine = used_textSize * factor_letterWidth_to_lineHeight;
            float paddingBetween_outlines_and_text = used_textSize * factor_textSize_to_paddingSize;
            float heightOfWholeTableOutline;

            if (heightOfWholeOutlineBox_isForced)
            {
                heightOfWholeTableOutline = forceHeightOfWholeTableBox;
            }
            else
            {
                heightOfWholeTableOutline = height_fromLineToLine * numberOfAllLinesInclTitles + paddingBetween_outlines_and_text; //only one "paddingBetween_outlines_and_text" at the bottom. The top has no such padding because the text doesn't fill the whole vertical space of it's line.
            }

            bool dirAndUp_areAlreadyGuaranteed_perpAndNormalized = UtilitiesDXXL_TextDirAndUpCalculation.ConvertQuaternionToTextDirAndUpVectors(out Vector3 textDir_candidate, out Vector3 textUp_candidate, rotation);
            UtilitiesDXXL_TextDirAndUpCalculation.GetTextDirAndUpNormalized(out Vector3 rightward_ofCollection_normalized, out Vector3 upward_ofCollection_normalized, textDir_candidate, textUp_candidate, position, false, dirAndUp_areAlreadyGuaranteed_perpAndNormalized);
            dirAndUp_areAlreadyGuaranteed_perpAndNormalized = true;

            Vector3 downward_ofCollection_normalized = -upward_ofCollection_normalized;
            Vector3 vertVector_downwardFromLineToLine = height_fromLineToLine * downward_ofCollection_normalized;
            Vector3 vertVector_downwardFromTextEndToHorizLines = 0.32f * height_fromLineToLine * downward_ofCollection_normalized;
            Vector3 vertVector_offsetForHighlightedHorizLines = 0.02f * height_fromLineToLine * downward_ofCollection_normalized;
            Vector3 vertVector_offsetForTitleText = 0.2f * height_fromLineToLine * upward_ofCollection_normalized;

            Vector3 topLeftCorner_ofWholeOutlineBox;
            if (position_isTopLeft_notLowLeft)
            {
                topLeftCorner_ofWholeOutlineBox = position;
            }
            else
            {
                topLeftCorner_ofWholeOutlineBox = position + upward_ofCollection_normalized * heightOfWholeTableOutline;
            }

            Vector3 lowLeftPos_ofTitleTextLine_alreadyIndentedFromBoundaryLine = topLeftCorner_ofWholeOutlineBox + rightward_ofCollection_normalized * paddingBetween_outlines_and_text + vertVector_downwardFromLineToLine + vertVector_offsetForTitleText;
            string usedTitle = string.IsNullOrEmpty(title) ? titleFallback : title;
            usedTitle = (countOfCollection == 0) ? usedTitle + " (with zero elements)" : usedTitle;

            UtilitiesDXXL_Text.Write(usedTitle, lowLeftPos_ofTitleTextLine_alreadyIndentedFromBoundaryLine, color, used_textSize, rightward_ofCollection_normalized, upward_ofCollection_normalized, DrawText.TextAnchorDXXL.LowerLeft, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, false, false, dirAndUp_areAlreadyGuaranteed_perpAndNormalized);
            float widthOfTitleTextInclPaddings = paddingBetween_outlines_and_text + DrawText.parsedTextSpecs.widthOfLongestLine + paddingBetween_outlines_and_text;
            float widthOfAllColumnsInclPaddings = WriteColumns(collectionToDraw, countOfCollection, GetColumn1ContentAsString, GetColumn2ContentAsString, GetColumn3ContentAsString, GetColumn4ContentAsString, nameOfColumn1, nameOfColumn2, nameOfColumn3, nameOfColumn4, upward_ofCollection_normalized, rightward_ofCollection_normalized, vertVector_downwardFromTextEndToHorizLines, lowLeftPos_ofTitleTextLine_alreadyIndentedFromBoundaryLine, vertVector_downwardFromLineToLine, vertVector_offsetForTitleText, color, color_ofBoundaryLines, paddingBetween_outlines_and_text, used_textSize, autoFlipToPreventMirrorInverted, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            DrawBoxLines(countOfCollection, topLeftCorner_ofWholeOutlineBox, downward_ofCollection_normalized, rightward_ofCollection_normalized, vertVector_downwardFromLineToLine, vertVector_downwardFromTextEndToHorizLines, vertVector_offsetForHighlightedHorizLines, widthOfTitleTextInclPaddings, widthOfAllColumnsInclPaddings, heightOfWholeTableOutline, color_ofBoundaryLines, color_ofLowerAlphaHorizLines, durationInSec, hiddenByNearerObjects);
        }

        static float WriteColumns<CollectionType>(CollectionType collectionToDraw, int countOfCollection, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn1ContentAsString, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn2ContentAsString, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn3ContentAsString, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetColumn4ContentAsString, string nameOfColumn1, string nameOfColumn2, string nameOfColumn3, string nameOfColumn4, Vector3 upward_ofCollection_normalized, Vector3 rightward_ofCollection_normalized, Vector3 vertVector_downwardFromTextEndToHorizLines, Vector3 lowLeftPos_ofTitleTextLine_alreadyIndentedFromBoundaryLine, Vector3 vertVector_downwardFromLineToLine, Vector3 vertVector_offsetForTitleText, Color color, Color color_ofBoundaryLines, float paddingBetween_outlines_and_text, float used_textSize, bool autoFlipToPreventMirrorInverted, bool collectionRepresentsBools, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine = lowLeftPos_ofTitleTextLine_alreadyIndentedFromBoundaryLine + vertVector_downwardFromLineToLine - vertVector_offsetForTitleText;

            float width_ofTextInIndexColumn = DrawIndexColumn(lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine, countOfCollection, vertVector_downwardFromLineToLine, upward_ofCollection_normalized, rightward_ofCollection_normalized, used_textSize, color, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0.0f; }

            float widthOfAllColumnsInclPaddings = paddingBetween_outlines_and_text + width_ofTextInIndexColumn + paddingBetween_outlines_and_text;

            lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine = lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine + (width_ofTextInIndexColumn + paddingBetween_outlines_and_text + paddingBetween_outlines_and_text) * rightward_ofCollection_normalized;
            float width_ofTextInContentColumn1 = TryDrawContentColumn(lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine, collectionToDraw, countOfCollection, GetColumn1ContentAsString, nameOfColumn1, vertVector_downwardFromLineToLine, vertVector_downwardFromTextEndToHorizLines, upward_ofCollection_normalized, rightward_ofCollection_normalized, paddingBetween_outlines_and_text, used_textSize, color, autoFlipToPreventMirrorInverted, color_ofBoundaryLines, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0.0f; }
            widthOfAllColumnsInclPaddings = widthOfAllColumnsInclPaddings + paddingBetween_outlines_and_text + width_ofTextInContentColumn1 + paddingBetween_outlines_and_text;

            if (GetColumn2ContentAsString != null)
            {
                lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine = lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine + (width_ofTextInContentColumn1 + paddingBetween_outlines_and_text + paddingBetween_outlines_and_text) * rightward_ofCollection_normalized;
                float width_ofTextInContentColumn2 = TryDrawContentColumn(lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine, collectionToDraw, countOfCollection, GetColumn2ContentAsString, nameOfColumn2, vertVector_downwardFromLineToLine, vertVector_downwardFromTextEndToHorizLines, upward_ofCollection_normalized, rightward_ofCollection_normalized, paddingBetween_outlines_and_text, used_textSize, color, autoFlipToPreventMirrorInverted, color_ofBoundaryLines, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
                if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0.0f; }
                widthOfAllColumnsInclPaddings = widthOfAllColumnsInclPaddings + paddingBetween_outlines_and_text + width_ofTextInContentColumn2 + paddingBetween_outlines_and_text;

                if (GetColumn3ContentAsString != null)
                {
                    lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine = lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine + (width_ofTextInContentColumn2 + paddingBetween_outlines_and_text + paddingBetween_outlines_and_text) * rightward_ofCollection_normalized;
                    float width_ofTextInContentColumn3 = TryDrawContentColumn(lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine, collectionToDraw, countOfCollection, GetColumn3ContentAsString, nameOfColumn3, vertVector_downwardFromLineToLine, vertVector_downwardFromTextEndToHorizLines, upward_ofCollection_normalized, rightward_ofCollection_normalized, paddingBetween_outlines_and_text, used_textSize, color, autoFlipToPreventMirrorInverted, color_ofBoundaryLines, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
                    if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0.0f; }
                    widthOfAllColumnsInclPaddings = widthOfAllColumnsInclPaddings + paddingBetween_outlines_and_text + width_ofTextInContentColumn3 + paddingBetween_outlines_and_text;

                    if (GetColumn4ContentAsString != null)
                    {
                        lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine = lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine + (width_ofTextInContentColumn3 + paddingBetween_outlines_and_text + paddingBetween_outlines_and_text) * rightward_ofCollection_normalized;
                        float width_ofTextInContentColumn4 = TryDrawContentColumn(lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine, collectionToDraw, countOfCollection, GetColumn4ContentAsString, nameOfColumn4, vertVector_downwardFromLineToLine, vertVector_downwardFromTextEndToHorizLines, upward_ofCollection_normalized, rightward_ofCollection_normalized, paddingBetween_outlines_and_text, used_textSize, color, autoFlipToPreventMirrorInverted, color_ofBoundaryLines, collectionRepresentsBools, durationInSec, hiddenByNearerObjects);
                        if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0.0f; }
                        widthOfAllColumnsInclPaddings = widthOfAllColumnsInclPaddings + paddingBetween_outlines_and_text + width_ofTextInContentColumn4 + paddingBetween_outlines_and_text;
                    }
                }
            }
            return widthOfAllColumnsInclPaddings;
        }

        static float DrawIndexColumn(Vector3 lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine, int countOfCollection, Vector3 vertVector_downwardFromLineToLine, Vector3 upward_ofCollection_normalized, Vector3 rightward_ofCollection_normalized, float used_textSize, Color color, bool autoFlipToPreventMirrorInverted, float durationInSec, bool hiddenByNearerObjects)
        {
            float width_ofWidestTextInWholeColumn = 0.0f;
            UtilitiesDXXL_Text.Write("i", lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine, color, used_textSize, rightward_ofCollection_normalized, upward_ofCollection_normalized, DrawText.TextAnchorDXXL.LowerLeft, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, false, false, true);
            width_ofWidestTextInWholeColumn = Mathf.Max(width_ofWidestTextInWholeColumn, DrawText.parsedTextSpecs.widthOfLongestLine);

            Vector3 lowLeftPos_ofCurrentLinesTextLine = lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine;
            for (int i = 0; i < countOfCollection; i++)
            {
                if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return width_ofWidestTextInWholeColumn; }
                lowLeftPos_ofCurrentLinesTextLine = lowLeftPos_ofCurrentLinesTextLine + vertVector_downwardFromLineToLine;
                UtilitiesDXXL_Text.Write("" + i, lowLeftPos_ofCurrentLinesTextLine, color, used_textSize, rightward_ofCollection_normalized, upward_ofCollection_normalized, DrawText.TextAnchorDXXL.LowerLeft, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, false, false, true);
                width_ofWidestTextInWholeColumn = Mathf.Max(width_ofWidestTextInWholeColumn, DrawText.parsedTextSpecs.widthOfLongestLine);
            }

            return width_ofWidestTextInWholeColumn;
        }

        static float TryDrawContentColumn<CollectionType>(Vector3 lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine, CollectionType collectionToDraw, int countOfCollection, FlexibleGetColumnContentAtIndexAsString<CollectionType> GetContentOfColumnAtIndex, string nameOfColumn, Vector3 vertVector_downwardFromLineToLine, Vector3 vertVector_downwardFromTextEndToHorizLines, Vector3 upward_ofCollection_normalized, Vector3 rightward_ofCollection_normalized, float paddingBetween_outlines_and_text, float used_textSize, Color color, bool autoFlipToPreventMirrorInverted, Color color_ofBoundaryLines, bool collectionRepresentsBools, float durationInSec, bool hiddenByNearerObjects)
        {
            float width_ofWidestTextInWholeColumn = 0.0f;
            UtilitiesDXXL_Text.Write(nameOfColumn, lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine, color, used_textSize, rightward_ofCollection_normalized, upward_ofCollection_normalized, DrawText.TextAnchorDXXL.LowerLeft, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, false, false, true);
            width_ofWidestTextInWholeColumn = Mathf.Max(width_ofWidestTextInWholeColumn, DrawText.parsedTextSpecs.widthOfLongestLine);

            Vector3 vertShiftOffsetFor_sizeAmplifiedBools = collectionRepresentsBools ? (0.175f * vertVector_downwardFromLineToLine) : Vector3.zero;
            Vector3 lowLeftPos_ofCurrentLinesTextLine = lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine + vertShiftOffsetFor_sizeAmplifiedBools;
            float used_textSize_inclSizeAmplificationOfBools = collectionRepresentsBools ? (2.0f * used_textSize) : used_textSize;
            for (int i = 0; i < countOfCollection; i++)
            {
                if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return width_ofWidestTextInWholeColumn; }
                lowLeftPos_ofCurrentLinesTextLine = lowLeftPos_ofCurrentLinesTextLine + vertVector_downwardFromLineToLine;

                string collectionSlotContentAsString = GetContentOfColumnAtIndex(collectionToDraw, i);
                if (collectionSlotContentAsString == null) { collectionSlotContentAsString = "<i>null</i>"; }

                UtilitiesDXXL_Text.Write(collectionSlotContentAsString, lowLeftPos_ofCurrentLinesTextLine, color, used_textSize_inclSizeAmplificationOfBools, rightward_ofCollection_normalized, upward_ofCollection_normalized, DrawText.TextAnchorDXXL.LowerLeft, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, autoFlipToPreventMirrorInverted, durationInSec, hiddenByNearerObjects, false, false, true);
                width_ofWidestTextInWholeColumn = Mathf.Max(width_ofWidestTextInWholeColumn, DrawText.parsedTextSpecs.widthOfLongestLine);
            }

            Vector3 lowerEndOfText_atVertColumnBorderLineAtLeftSide = lowLeftPos_ofCurrentColumnsTitleTextLine_alreadyIndentedFromBoundaryLine - rightward_ofCollection_normalized * paddingBetween_outlines_and_text - vertVector_downwardFromLineToLine;
            Vector3 upperEnd_ofVertColumnBorderLineAtLeftSide = lowerEndOfText_atVertColumnBorderLineAtLeftSide + vertVector_downwardFromTextEndToHorizLines;
            Vector3 lowerEnd_ofVertColumnBorderLineAtLeftSide = lowerEndOfText_atVertColumnBorderLineAtLeftSide + vertVector_downwardFromLineToLine * (1 + countOfCollection) - upward_ofCollection_normalized * paddingBetween_outlines_and_text;
            Line_fadeableAnimSpeed.InternalDraw(upperEnd_ofVertColumnBorderLineAtLeftSide, lowerEnd_ofVertColumnBorderLineAtLeftSide, color_ofBoundaryLines, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            return width_ofWidestTextInWholeColumn;
        }

        static void DrawBoxLines(int countOfCollection, Vector3 topLeftCorner_ofWholeOutlineBox, Vector3 downward_ofCollection_normalized, Vector3 rightward_ofCollection_normalized, Vector3 vertVector_downwardFromLineToLine, Vector3 vertVector_downwardFromTextEndToHorizLines, Vector3 vertVector_offsetForHighlightedHorizLines, float widthOfTitleTextInclPaddings, float widthOfAllColumnsInclPaddings, float heightOfWholeTableOutline, Color color_ofBoundaryLines, Color color_ofLowerAlphaHorizLines, float durationInSec, bool hiddenByNearerObjects)
        {
            float width_fromLeftWholeTableOutline_toRightWholeTableOutline = Mathf.Max(widthOfTitleTextInclPaddings, widthOfAllColumnsInclPaddings);
            Vector3 downwardEdge_ofWholeTableOutLine = downward_ofCollection_normalized * heightOfWholeTableOutline;
            Vector3 rightwardEdge_ofWholeTableOutLine = rightward_ofCollection_normalized * width_fromLeftWholeTableOutline_toRightWholeTableOutline;
            Vector3 lowLeftCorner_ofWholeOutlineBox = topLeftCorner_ofWholeOutlineBox + downwardEdge_ofWholeTableOutLine;
            Vector3 topRightCorner_ofWholeOutlineBox = topLeftCorner_ofWholeOutlineBox + rightwardEdge_ofWholeTableOutLine;
            Vector3 lowRightCorner_ofWholeOutlineBox = topRightCorner_ofWholeOutlineBox + downwardEdge_ofWholeTableOutLine;
            Line_fadeableAnimSpeed.InternalDraw(topLeftCorner_ofWholeOutlineBox, lowLeftCorner_ofWholeOutlineBox, color_ofBoundaryLines, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(lowLeftCorner_ofWholeOutlineBox, lowRightCorner_ofWholeOutlineBox, color_ofBoundaryLines, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(lowRightCorner_ofWholeOutlineBox, topRightCorner_ofWholeOutlineBox, color_ofBoundaryLines, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            Line_fadeableAnimSpeed.InternalDraw(topRightCorner_ofWholeOutlineBox, topLeftCorner_ofWholeOutlineBox, color_ofBoundaryLines, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            for (int i = 1; i < countOfCollection + 2; i++)
            {
                Vector3 vertOffsetVector_fromTopEndOfWholeOutlineBox = vertVector_downwardFromLineToLine * i + vertVector_downwardFromTextEndToHorizLines;
                Vector3 leftEndOfHorizLine = topLeftCorner_ofWholeOutlineBox + vertOffsetVector_fromTopEndOfWholeOutlineBox;
                Vector3 rightEndOfHorizLine = topRightCorner_ofWholeOutlineBox + vertOffsetVector_fromTopEndOfWholeOutlineBox;

                Color used_color = (i >= 3) ? color_ofLowerAlphaHorizLines : color_ofBoundaryLines;
                Line_fadeableAnimSpeed.InternalDraw(leftEndOfHorizLine, rightEndOfHorizLine, used_color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

                if (i < 3)
                {
                    Line_fadeableAnimSpeed.InternalDraw(leftEndOfHorizLine + vertVector_offsetForHighlightedHorizLines, rightEndOfHorizLine + vertVector_offsetForHighlightedHorizLines, used_color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                }
            }
        }


    }

}
