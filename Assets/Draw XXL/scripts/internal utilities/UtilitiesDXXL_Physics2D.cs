namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class UtilitiesDXXL_Physics2D
    {
        public static RaycastHit2D[] preallocatedRayHit2DResultsArray_copiedFromList = new RaycastHit2D[DrawPhysics2D.MaxNumberOfPreallocatedHits];
        public static Collider2D[] preallocatedCollider2DResultsArray_copiedFromList = new Collider2D[DrawPhysics2D.MaxNumberOfPreallocatedHits];

        public static void DrawRaycastTillFirstHit(bool hasHit, Vector2 originV2, Vector2 directionV2, float maxDistance, RaycastHit2D hitInfo2D, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(directionV2))
            {
                UtilitiesDXXL_DrawBasics2D.PointFallback(originV2, GetZPosForDrawVisualisation(), "[<color=#adadadFF><icon=logMessage></color> DrawRaycast2D with direction of zero]<br>" + nameText, DrawPhysics2D.colorForNonHittingCasts, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            float distanceOfFarestHit = GetDistanceOfSingleHit(hasHit, hitInfo2D);
            DrawRayOfRaycast(hasHit ? 1 : 0, originV2, directionV2, maxDistance, nameText, distanceOfFarestHit, durationInSec, hiddenByNearerObjects);
            if (hasHit)
            {
                DrawRaycastHitInfo(hitInfo2D, 0, nameText, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void DrawRaycastPotMultipleHits(Vector2 originV2, Vector2 directionV2, float maxDistance, RaycastHit2D[] hitInfos2D, int numberOfUsedSlotsInHitInfoArray, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(directionV2))
            {
                UtilitiesDXXL_DrawBasics2D.PointFallback(originV2, GetZPosForDrawVisualisation(), "[<color=#adadadFF><icon=logMessage></color> DrawRaycast2D with direction of zero]<br>" + nameText, DrawPhysics2D.colorForNonHittingCasts, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            float distanceOfFarestHit = GetDistanceOfFarestHit(hitInfos2D, numberOfUsedSlotsInHitInfoArray);
            DrawRayOfRaycast(numberOfUsedSlotsInHitInfoArray, originV2, directionV2, maxDistance, nameText, distanceOfFarestHit, durationInSec, hiddenByNearerObjects);
            for (int i = 0; i < numberOfUsedSlotsInHitInfoArray; i++)
            {
                DrawRaycastHitInfo(hitInfos2D[i], i, nameText, durationInSec, hiddenByNearerObjects);
            }
        }

        static void DrawRayOfRaycast(int hitCount, Vector2 originV2, Vector2 directionV2, float maxDistance, string nameText, float distanceOfFarestHit, float durationInSec, bool hiddenByNearerObjects)
        {
            bool hasHit = hitCount > 0;
            Color color = hasHit ? DrawPhysics2D.colorForHittingCasts : DrawPhysics2D.colorForNonHittingCasts;
            Vector3 direction_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(new Vector3(directionV2.x, directionV2.y, 0.0f));
            bool hasUnlimitedLength = float.IsInfinity(maxDistance);
            float absMaxDistance = Mathf.Abs(maxDistance);
            float lengthOfRayDirIndicator = Mathf.Min(1.0f, 0.9f * absMaxDistance);
            maxDistance = Mathf.Min(maxDistance, 100000.0f);
            maxDistance = Mathf.Max(maxDistance, -100000.0f);
            Vector3 origin = new Vector3(originV2.x, originV2.y, GetZPosForDrawVisualisation());
            Vector3 endPos = origin + direction_normalized * maxDistance;
            Line_fadeableAnimSpeed.InternalDraw(origin, endPos, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            //at origin:
            if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                float width_ofBase = 0.1f * lengthOfRayDirIndicator;
                DrawShapes.Pyramid(origin, lengthOfRayDirIndicator, 0.0f, width_ofBase, color, direction_normalized, Vector3.up, DrawShapes.Shape2DType.square, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                string rayText = GetRayText(nameText, hitCount, maxDistance, "<size=27>Raycast 2D</size><br><size=5> </size><br>number of hits: ");
                DrawEngineBasics.RayLineExtended2D(origin, direction_normalized * lengthOfRayDirIndicator, color, 0.0f, rayText, GetZPosForDrawVisualisation(), 0.0f, false, 0.01f, 0.0f, durationInSec, hiddenByNearerObjects);
            }

            //overdraw line after last hit:
            if (hasHit)
            {
                if (maxDistance < 0.0f)
                {
                    //unity.RaycastHit2D always returns positve values for "distance", also if the cast goes backward due to negative "maxDistance"
                    distanceOfFarestHit = -distanceOfFarestHit;
                }
                Line_fadeableAnimSpeed.InternalDraw(origin + direction_normalized * distanceOfFarestHit, endPos, DrawPhysics2D.colorForCastLineBeyondHit, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            //end cap:
            if (hasUnlimitedLength == false)
            {
                if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
                {
                    DrawShapes.Decagon(endPos, 0.025f, hasHit ? DrawPhysics2D.colorForCastLineBeyondHit : DrawPhysics2D.colorForNonHittingCasts, direction_normalized, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
                }
            }
        }

        static void DrawRaycastHitInfo(RaycastHit2D hitInfo2D, int i_hit, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.VectorIsInvalid(hitInfo2D.point) || UtilitiesDXXL_Math.VectorIsInvalid(hitInfo2D.normal))
            {
                Debug.LogError("Draw XXL: A 'Physics2D.RayCast()' returned an invalid hit position (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.point) + ") or normal (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.normal) + "). The drawn cast visualization may be incorrect.");
            }
            else
            {
                bool saveDrawnLines = i_hit >= DrawPhysics2D.hitResultsWithMoreDetailedDisplay;
                if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.high_withFullDetails) { saveDrawnLines = true; }

                Vector3 impactPos_v3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(hitInfo2D.point, hitInfo2D.transform.position.z);

                //dashed connection line along z:
                if (UtilitiesDXXL_Math.CheckIf_twoFloatsAreApproximatelyEqual(hitInfo2D.transform.position.z, GetZPosForDrawVisualisation()) == false)
                {
                    float absZDistance = Mathf.Abs(hitInfo2D.transform.position.z - GetZPosForDrawVisualisation());
                    Vector3 impactPos_projectedOntoDrawVisualisation_V3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(hitInfo2D.point, GetZPosForDrawVisualisation());
                    Color color_forDashedLineAlongZ = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawPhysics2D.colorForHittingCasts, 0.6f);
                    DrawBasics.LineStyle lineStyleAlongZ = (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes) ? DrawBasics.LineStyle.solid : DrawBasics.LineStyle.dashedLong;
                    Line_fadeableAnimSpeed.InternalDraw(impactPos_v3, impactPos_projectedOntoDrawVisualisation_V3, color_forDashedLineAlongZ, 0.0f, null, lineStyleAlongZ, absZDistance, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                }

                //Normal:
                Vector3 normal_V3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(hitInfo2D.normal);
                Color color_ofNormal = Get_color_ofNormal();
                if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
                {
                    DrawBasics2D.LineFrom(impactPos_v3, normal_V3, color_ofNormal, 0.0f, null, DrawBasics.LineStyle.solid, hitInfo2D.transform.position.z, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                }
                else
                {
                    float relConeLength_ofNormalVector = 0.17f;
                    string normalText = (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) ? null : (saveDrawnLines ? "normal" : "normal<br><size=4>of hit surface</size>");
                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                    DrawBasics2D.VectorFrom(impactPos_v3, normal_V3, color_ofNormal, saveDrawnLines ? 0.0f : 0.006f, normalText, relConeLength_ofNormalVector, false, hitInfo2D.transform.position.z, false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                }

                //Normal Socket and Text Description:
                switch (DrawPhysics2D.visualizationQuality)
                {
                    case DrawPhysics.VisualizationQuality.high_withFullDetails:
                        DrawNormalSocketAndText_highQuality(impactPos_v3, normal_V3, color_ofNormal, hitInfo2D, i_hit, nameText, saveDrawnLines, durationInSec, hiddenByNearerObjects);
                        break;
                    case DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes:
                        DrawNormalSocketAndText_mediumQuality(impactPos_v3, normal_V3, color_ofNormal, nameText, durationInSec, hiddenByNearerObjects);
                        break;
                    case DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes:
                        //normal socket:
                        DrawShapes.Square(impactPos_v3, 0.12f, color_ofNormal, normal_V3, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
                        break;
                    default:
                        break;
                }
            }
        }

        static void DrawNormalSocketAndText_highQuality(Vector3 impactPos_v3, Vector3 normal_V3, Color color_ofNormal, RaycastHit2D hitInfo2D, int i_hit, string nameText, bool saveDrawnLines, float durationInSec, bool hiddenByNearerObjects)
        {
            //normal socket:
            DrawShapes.Decagon(impactPos_v3, 0.03f, color_ofNormal, normal_V3, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);

            //text description:
            float textOffsetDistance = DrawPhysics2D.scaleFactor_forCastHitTextSize;
            string text;
            if (DrawPhysics2D.drawCastNameTag_atHitPositions && nameText != null && nameText.Length != 0)
            {
                text = (saveDrawnLines ? (nameText + " / #" + i_hit + ":<br>hit GO: " + hitInfo2D.transform.gameObject.name + "<br>pos = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.point) + "<br>dist = " + hitInfo2D.distance) : (UtilitiesDXXL_Physics.GetStrokeWidthMarkupStartStringForHitPosDesctiptionHeaders(nameText) + nameText + " / hit #" + i_hit + ":</sw><br>GameObject that was hit: " + hitInfo2D.transform.gameObject.name + "<br>position = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.point) + "<br>distance = " + hitInfo2D.distance));
            }
            else
            {
                text = (saveDrawnLines ? ("hit #" + i_hit + ":<br>hit GO: " + hitInfo2D.transform.gameObject.name + "<br>pos = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.point) + "<br>dist = " + hitInfo2D.distance) : ("<sw=80000>Raycast2D hit #" + i_hit + ":</sw><br>GameObject that was hit: " + hitInfo2D.transform.gameObject.name + "<br>position = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.point) + "<br>distance = " + hitInfo2D.distance));
            }

            TrySet_default_textOffsetDirection_forPointTags_reversible();
            DrawBasics2D.PointTag(hitInfo2D.point, text, DrawPhysics2D.colorForCastsHitText, 0.0f, textOffsetDistance, default(Vector2), hitInfo2D.transform.position.z, 1.0f, false, durationInSec, hiddenByNearerObjects);
            TryReverse_default_textOffsetDirection_forPointTags();
        }

        static void DrawNormalSocketAndText_mediumQuality(Vector3 impactPos_v3, Vector3 normal_V3, Color color_ofNormal, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            //normal socket:
            DrawShapes.Decagon(impactPos_v3, 0.03f, color_ofNormal, normal_V3, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);

            //text description:
            if (DrawPhysics2D.drawCastNameTag_atHitPositions && nameText != null && nameText.Length != 0)
            {
                UtilitiesDXXL_Text.Write(nameText, impactPos_v3, DrawPhysics2D.colorForCastsHitText, 0.1f * DrawPhysics2D.scaleFactor_forCastHitTextSize, Vector3.right, Vector3.up, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
            }
        }

        public static void DrawRaycast3DTillFirstHit(bool hasHit, Ray ray, float maxDistance, RaycastHit2D hitInfo2D, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(ray.direction))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(ray.origin, "[<color=#adadadFF><icon=logMessage></color> DrawRay3D against 2D colliders with direction of zero]<br>" + nameText, DrawPhysics2D.colorForNonHittingCasts, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            float distanceOfFarestHit = GetDistanceOfSingleHit(hasHit, hitInfo2D);
            DrawRayOfRaycast3D(hasHit ? 1 : 0, ray, maxDistance, nameText, distanceOfFarestHit, durationInSec, hiddenByNearerObjects);
            if (hasHit)
            {
                DrawRaycast3DHitInfo(hitInfo2D, 0, nameText, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void DrawRaycast3DPotMultipleHits(Ray ray, float maxDistance, RaycastHit2D[] hitInfos2D, int numberOfUsedSlotsInHitInfoArray, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(ray.direction))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(ray.origin, "[<color=#adadadFF><icon=logMessage></color> DrawRay3D against 2D colliders with direction of zero]<br>" + nameText, DrawPhysics2D.colorForNonHittingCasts, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            float distanceOfFarestHit = GetDistanceOfFarestHit(hitInfos2D, numberOfUsedSlotsInHitInfoArray);
            DrawRayOfRaycast3D(numberOfUsedSlotsInHitInfoArray, ray, maxDistance, nameText, distanceOfFarestHit, durationInSec, hiddenByNearerObjects);
            for (int i = 0; i < numberOfUsedSlotsInHitInfoArray; i++)
            {
                DrawRaycast3DHitInfo(hitInfos2D[i], i, nameText, durationInSec, hiddenByNearerObjects);
            }
        }

        static void DrawRayOfRaycast3D(int hitCount, Ray ray, float maxDistance, string nameText, float distanceOfFarestHit, float durationInSec, bool hiddenByNearerObjects)
        {
            bool hasHit = hitCount > 0;
            Color color = hasHit ? DrawPhysics2D.colorForHittingCasts : DrawPhysics2D.colorForNonHittingCasts;
            Vector3 direction_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(ray.direction);
            bool hasUnlimitedLength = float.IsInfinity(maxDistance);
            float absMaxDistance = Mathf.Abs(maxDistance);
            float lengthOfRayDirIndicator = Mathf.Min(1.0f, 0.9f * absMaxDistance);
            maxDistance = Mathf.Min(maxDistance, 100000.0f);
            maxDistance = Mathf.Max(maxDistance, -100000.0f);
            Vector3 endPos = ray.origin + direction_normalized * maxDistance;
            Line_fadeableAnimSpeed.InternalDraw(ray.origin, endPos, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            //at origin:
            if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                float width_ofBase = 0.1f * lengthOfRayDirIndicator;
                DrawShapes.Pyramid(ray.origin, lengthOfRayDirIndicator, width_ofBase, width_ofBase, color, direction_normalized, Vector3.up, DrawShapes.Shape2DType.square, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                string rayText = GetRayText(nameText, hitCount, maxDistance, "<size=27>Ray3D against<br>2D colliders</size><br><size=5> </size><br>number of hits: ");
                DrawEngineBasics.RayLineExtended(ray.origin, direction_normalized * lengthOfRayDirIndicator, color, 0.0f, rayText, 0.0f, false, 0.01f, 0.0f, durationInSec, hiddenByNearerObjects);
            }

            //overdraw line after last hit:
            if (hasHit)
            {
                if (maxDistance < 0.0f)
                {
                    //unity.RaycastHit2D always returns positve values for "distance", also if the cast goes backward due to negative "maxDistance"
                    distanceOfFarestHit = -distanceOfFarestHit;
                }
                Line_fadeableAnimSpeed.InternalDraw(ray.origin + direction_normalized * distanceOfFarestHit, endPos, DrawPhysics2D.colorForCastLineBeyondHit, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            //end cap:
            if (hasUnlimitedLength == false)
            {
                if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
                {
                    DrawShapes.Decagon(endPos, 0.025f, hasHit ? DrawPhysics2D.colorForCastLineBeyondHit : DrawPhysics2D.colorForNonHittingCasts, direction_normalized, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
                }
            }
        }

        static string GetRayText(string nameText, int hitCount, float maxDistance, string fallbackTextFragment_forNoSpecifiedName)
        {
            if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                return null;
            }
            else
            {
                string rayText;
                if (DrawPhysics2D.drawCastNameTag_atCastOrigin && nameText != null && nameText.Length != 0)
                {
                    rayText = (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) ? UtilitiesDXXL_Physics.GetSizeMarkupStartStringForCastNames(nameText) + nameText + "</size><br><size=5> </size><br>hits: " + hitCount : UtilitiesDXXL_Physics.GetSizeMarkupStartStringForCastNames(nameText) + nameText + "</size><br><size=5> </size><br>number of hits: " + hitCount;
                }
                else
                {
                    rayText = (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) ? "hits: " + hitCount : fallbackTextFragment_forNoSpecifiedName + hitCount;
                }

                if (maxDistance < 0.0f)
                {
                    rayText = "[<color=#e2aa00FF><icon=warning></color> negative ray direction]<br>" + rayText;
                }
                return rayText;
            }
        }

        static void DrawRaycast3DHitInfo(RaycastHit2D hitInfo2D, int i_hit, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.VectorIsInvalid(hitInfo2D.point))
            {
                Debug.LogError("Draw XXL: A 'Physics2D.Cast()' returned an invalid hit position (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.point) + "). The drawn cast visualization may be incorrect.");
            }
            else
            {
                bool saveDrawnLines = i_hit >= DrawPhysics2D.hitResultsWithMoreDetailedDisplay;
                if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.high_withFullDetails) { saveDrawnLines = true; }

                Vector3 impactPos_v3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(hitInfo2D.point, hitInfo2D.transform.position.z);
                Color color_ofImpactPosCircle = (DrawPhysics2D.colorForHittingCasts.grayscale < 0.175f) ? Color.Lerp(DrawPhysics2D.colorForHittingCasts, Color.white, 0.7f) : Color.Lerp(DrawPhysics2D.colorForHittingCasts, Color.black, 0.7f);
                switch (DrawPhysics2D.visualizationQuality)
                {
                    case DrawPhysics.VisualizationQuality.high_withFullDetails:
                        DrawNormalSocketAndText_forRaycast3D_highQuality(hitInfo2D, i_hit, nameText, impactPos_v3, color_ofImpactPosCircle, saveDrawnLines, durationInSec, hiddenByNearerObjects);
                        break;
                    case DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes:
                        DrawNormalSocketAndText_forRaycast3D_mediumQuality(nameText, impactPos_v3, color_ofImpactPosCircle, durationInSec, hiddenByNearerObjects);
                        break;
                    case DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes:
                        DrawShapes.Square(impactPos_v3, 0.03f, color_ofImpactPosCircle, Vector3.forward, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
                        break;
                    default:
                        break;
                }
            }
        }

        static void DrawNormalSocketAndText_forRaycast3D_highQuality(RaycastHit2D hitInfo2D, int i_hit, string nameText, Vector3 impactPos_v3, Color color_ofImpactPosCircle, bool saveDrawnLines, float durationInSec, bool hiddenByNearerObjects)
        {
            DrawShapes.Decagon(impactPos_v3, 0.03f, color_ofImpactPosCircle, Vector3.forward, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
          
            //text description:
            float textOffsetDistance = DrawPhysics2D.scaleFactor_forCastHitTextSize;
            string text;
            if (DrawPhysics2D.drawCastNameTag_atHitPositions && nameText != null && nameText.Length != 0)
            {
                text = (saveDrawnLines ? (nameText + " / #" + i_hit + ":<br>hit GO: " + hitInfo2D.transform.gameObject.name + "<br>pos = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.point) + "<br>dist = " + hitInfo2D.distance) : (UtilitiesDXXL_Physics.GetStrokeWidthMarkupStartStringForHitPosDesctiptionHeaders(nameText) + nameText + " / hit #" + i_hit + ":</sw><br>GameObject that was hit: " + hitInfo2D.transform.gameObject.name + "<br>position = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.point) + "<br>distance = " + hitInfo2D.distance));
            }
            else
            {
                text = (saveDrawnLines ? ("hit #" + i_hit + ":<br>hit GO: " + hitInfo2D.transform.gameObject.name + "<br>pos = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.point) + "<br>dist = " + hitInfo2D.distance) : ("<sw=80000>Ray3D against 2D colliders / hit #" + i_hit + ":</sw><br>GameObject that was hit: " + hitInfo2D.transform.gameObject.name + "<br>position = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.point) + "<br>distance = " + hitInfo2D.distance));
            }

            TrySet_default_textOffsetDirection_forPointTags_reversible();
            DrawBasics2D.PointTag(hitInfo2D.point, text, DrawPhysics2D.colorForCastsHitText, 0.0f, textOffsetDistance, default(Vector2), hitInfo2D.transform.position.z, 1.0f, false, durationInSec, hiddenByNearerObjects);
            TryReverse_default_textOffsetDirection_forPointTags();
        }

        static void DrawNormalSocketAndText_forRaycast3D_mediumQuality(string nameText, Vector3 impactPos_v3, Color color_ofImpactPosCircle, float durationInSec, bool hiddenByNearerObjects)
        {
            DrawShapes.Decagon(impactPos_v3, 0.03f, color_ofImpactPosCircle, Vector3.forward, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
         
            //text description:
            if (DrawPhysics2D.drawCastNameTag_atHitPositions && nameText != null && nameText.Length != 0)
            {
                UtilitiesDXXL_Text.Write(nameText, impactPos_v3, DrawPhysics2D.colorForCastsHitText, 0.1f * DrawPhysics2D.scaleFactor_forCastHitTextSize, Vector3.right, Vector3.up, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
            }
        }

        public static void DrawCirclecastTillFirstHit(float circleRadius, bool hasHit, Vector2 origin_V2, Vector2 direction_V2, float maxDistance, RaycastHit2D hitInfo2D, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(direction_V2))
            {
                UtilitiesDXXL_DrawBasics2D.PointFallback(origin_V2, GetZPosForDrawVisualisation(), "[<color=#adadadFF><icon=logMessage></color> DrawCircleCast2D with direction of zero]<br>" + nameText, DrawPhysics2D.colorForNonHittingCasts, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            bool radiusIsZero = UtilitiesDXXL_Math.ApproximatelyZero(circleRadius);
            bool circleRadiusTooSmall = circleRadius < 0.00011f; //Unity returns no hits, if a radius is equal or smaller than "0.0001f"
            circleRadius = Mathf.Abs(circleRadius);
            float distanceOfFarestHit = GetDistanceOfSingleHit(hasHit, hitInfo2D);
            DrawRayOfCirclecast(circleRadius, circleRadiusTooSmall, radiusIsZero, hasHit ? 1 : 0, origin_V2, direction_V2, maxDistance, nameText, distanceOfFarestHit, durationInSec, hiddenByNearerObjects);
            if (hasHit)
            {
                DrawCirclecastHitInfo(circleRadius, hitInfo2D, 0, nameText, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void DrawCirclecastPotMultipleHits(float circleRadius, Vector2 origin_V2, Vector2 direction_V2, float maxDistance, RaycastHit2D[] hitInfos2D, int numberOfUsedSlotsInHitInfoArray, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(direction_V2))
            {
                UtilitiesDXXL_DrawBasics2D.PointFallback(origin_V2, GetZPosForDrawVisualisation(), "[<color=#adadadFF><icon=logMessage></color> DrawCircleCast2D with direction of zero]<br>" + nameText, DrawPhysics2D.colorForNonHittingCasts, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            bool radiusIsZero = UtilitiesDXXL_Math.ApproximatelyZero(circleRadius);
            bool circleRadiusTooSmall = circleRadius < 0.00011f; //Unity returns no hits, if a radius is equal or smaller than "0.0001f"
            circleRadius = Mathf.Abs(circleRadius);
            float distanceOfFarestHit = GetDistanceOfFarestHit(hitInfos2D, numberOfUsedSlotsInHitInfoArray);
            DrawRayOfCirclecast(circleRadius, circleRadiusTooSmall, radiusIsZero, numberOfUsedSlotsInHitInfoArray, origin_V2, direction_V2, maxDistance, nameText, distanceOfFarestHit, durationInSec, hiddenByNearerObjects);
            for (int i = 0; i < numberOfUsedSlotsInHitInfoArray; i++)
            {
                DrawCirclecastHitInfo(circleRadius, hitInfos2D[i], i, nameText, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void DrawBoxcastTillFirstHit(bool hasHit, Vector2 origin_V2, Vector2 size_V2, float angleDegCC, Vector2 direction_V2, float maxDistance, RaycastHit2D hitInfo2D, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(direction_V2))
            {
                UtilitiesDXXL_DrawBasics2D.PointFallback(origin_V2, GetZPosForDrawVisualisation(), "[<color=#adadadFF><icon=logMessage></color> DrawBoxcast2D with direction of zero]<br>" + nameText, DrawPhysics2D.colorForNonHittingCasts, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            bool atLeastOneBoxDimIsSmallerThan0d00011 = (size_V2.x < 0.00011f) || (size_V2.y < 0.00011f); //Unity returns no hits, if a box dimension is equal or smaller than "0.0001f"
            float distanceOfFarestHit = GetDistanceOfSingleHit(hasHit, hitInfo2D);
            DrawRayOfBoxcast(hasHit ? 1 : 0, origin_V2, size_V2, angleDegCC, atLeastOneBoxDimIsSmallerThan0d00011, direction_V2, maxDistance, nameText, distanceOfFarestHit, durationInSec, hiddenByNearerObjects);
            if (hasHit)
            {
                DrawBoxcastHitInfo(size_V2, angleDegCC, hitInfo2D, 0, nameText, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void DrawBoxcastPotMultipleHits(Vector2 origin_V2, Vector2 size_V2, float angleDegCC, Vector2 direction_V2, float maxDistance, RaycastHit2D[] hitInfos2D, int numberOfUsedSlotsInHitInfoArray, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(direction_V2))
            {
                UtilitiesDXXL_DrawBasics2D.PointFallback(origin_V2, GetZPosForDrawVisualisation(), "[<color=#adadadFF><icon=logMessage></color> DrawBoxcast2D with direction of zero]<br>" + nameText, DrawPhysics2D.colorForNonHittingCasts, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            bool atLeastOneBoxDimIsSmallerThan0d00011 = (size_V2.x < 0.00011f) || (size_V2.y < 0.00011f); //Unity returns no hits, if a box dimension is equal or smaller than "0.0001f"
            float distanceOfFarestHit = GetDistanceOfFarestHit(hitInfos2D, numberOfUsedSlotsInHitInfoArray);
            DrawRayOfBoxcast(numberOfUsedSlotsInHitInfoArray, origin_V2, size_V2, angleDegCC, atLeastOneBoxDimIsSmallerThan0d00011, direction_V2, maxDistance, nameText, distanceOfFarestHit, durationInSec, hiddenByNearerObjects);
            for (int i = 0; i < numberOfUsedSlotsInHitInfoArray; i++)
            {
                DrawBoxcastHitInfo(size_V2, angleDegCC, hitInfos2D[i], i, nameText, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void DrawCapsulecastTillFirstHit(bool hasHit, Vector2 origin_V2, Vector2 size_V2, Vector2 direction_V2, CapsuleDirection2D capsuleDirection, float angleDegCC, float maxDistance, RaycastHit2D hitInfo2D, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(direction_V2))
            {
                UtilitiesDXXL_DrawBasics2D.PointFallback(origin_V2, GetZPosForDrawVisualisation(), "[<color=#adadadFF><icon=logMessage></color> DrawCapsulecast2D with direction of zero]<br>" + nameText, DrawPhysics2D.colorForNonHittingCasts, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            bool atLeastOneBoxDimIsSmallerThan0d00011 = (size_V2.x < 0.00011f) || (size_V2.y < 0.00011f); //Unity returns no hits, if a capsule dimension is equal or smaller than "0.0001f"
            float distanceOfFarestHit = GetDistanceOfSingleHit(hasHit, hitInfo2D);
            DrawRayOfCapsulecast(origin_V2, size_V2, atLeastOneBoxDimIsSmallerThan0d00011, capsuleDirection, angleDegCC, hasHit ? 1 : 0, direction_V2, maxDistance, nameText, distanceOfFarestHit, durationInSec, hiddenByNearerObjects);
            if (hasHit)
            {
                DrawCapsulecastHitInfo(size_V2, capsuleDirection, angleDegCC, hitInfo2D, 0, nameText, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void DrawCapsulecastPotMultipleHits(Vector2 origin_V2, Vector2 size_V2, Vector2 direction_V2, CapsuleDirection2D capsuleDirection, float angleDegCC, float maxDistance, RaycastHit2D[] hitInfos2D, int numberOfUsedSlotsInHitInfoArray, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(direction_V2))
            {
                UtilitiesDXXL_DrawBasics2D.PointFallback(origin_V2, GetZPosForDrawVisualisation(), "[<color=#adadadFF><icon=logMessage></color> DrawCapsulecast2D with direction of zero]<br>" + nameText, DrawPhysics2D.colorForNonHittingCasts, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            bool atLeastOneBoxDimIsSmallerThan0d00011 = (size_V2.x < 0.00011f) || (size_V2.y < 0.00011f); //Unity returns no hits, if a capsule dimension is equal or smaller than "0.0001f"
            float distanceOfFarestHit = GetDistanceOfFarestHit(hitInfos2D, numberOfUsedSlotsInHitInfoArray);
            DrawRayOfCapsulecast(origin_V2, size_V2, atLeastOneBoxDimIsSmallerThan0d00011, capsuleDirection, angleDegCC, numberOfUsedSlotsInHitInfoArray, direction_V2, maxDistance, nameText, distanceOfFarestHit, durationInSec, hiddenByNearerObjects);
            for (int i = 0; i < numberOfUsedSlotsInHitInfoArray; i++)
            {
                DrawCapsulecastHitInfo(size_V2, capsuleDirection, angleDegCC, hitInfos2D[i], i, nameText, durationInSec, hiddenByNearerObjects);
            }
        }

        static Vector2 volumeCastOutlineVertice1_local;
        static Vector2 volumeCastOutlineVertice2_local;
        static Vector2 volumeCastOutlineVertice1_projectedIntoPerpToCastDirPlane_local;
        static Vector2 volumeCastOutlineVertice2_projectedIntoPerpToCastDirPlane_local;
        static float distanceOfOutlineVerticesToOrigin_alongCastsUp;

        static void DrawRayOfCirclecast(float circleRadius, bool circleRadiusTooSmall, bool radiusIsZero, int hitCount, Vector2 origin_V2, Vector2 direction_V2, float maxDistance, string nameText, float distanceOfFarestHit, float durationInSec, bool hiddenByNearerObjects)
        {
            bool hasHit = hitCount > 0;
            Vector2 direction_normalized_V2 = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(direction_V2);
            bool hasUnlimitedLength = float.IsInfinity(maxDistance);
            maxDistance = Mathf.Min(maxDistance, 100000.0f);
            maxDistance = Mathf.Max(maxDistance, -100000.0f);
            Vector2 endPos_V2 = origin_V2 + direction_normalized_V2 * maxDistance;
            Vector3 castsUp_insideXYPlane_90degCCFromCastDir_normalized_V3 = Vector3.Cross(Vector3.forward, direction_normalized_V2);

            Color color = hasHit ? DrawPhysics2D.colorForHittingCasts : DrawPhysics2D.colorForNonHittingCasts;
            Color color_lowAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.5f);
            Color color_lowerAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.25f);

            Color color_ofCastEnd = hasHit ? DrawPhysics2D.colorForCastLineBeyondHit : DrawPhysics2D.colorForNonHittingCasts;
            Color color_ofCastEnd_lowAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofCastEnd, 0.5f);
            Color color_ofCastEnd_lowerAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofCastEnd, 0.25f);

            if (radiusIsZero == false)
            {
                FillCircleCastOutlineVertices(direction_normalized_V2, circleRadius);
                FillVolumeCastOutlineVertices_projectedIntoPerpToCastDirPlane(direction_V2);
                float arrowWidth = circleRadius * 0.5f;
                float arrowLength = circleRadius * 1.0f;
                float arrowsRelConeLenth = 0.45f;
                float circleDiameter = 2.0f * circleRadius;

                DrawCircleAtCastStartAndEnd(origin_V2, endPos_V2, circleRadius, color, color_ofCastEnd_lowerAlpha, durationInSec, hiddenByNearerObjects);
                float startArrows_startDistanceFromStart = circleDiameter;
                DrawArrowsAtVolumeCastStartAndEnd(out float distance_ofStartArrowsStartPos, out float distance_ofEndArrowsStartPos, hasHit, origin_V2, direction_normalized_V2, startArrows_startDistanceFromStart, maxDistance, distanceOfFarestHit, arrowLength, color_lowerAlpha, color_ofCastEnd_lowerAlpha, arrowsRelConeLenth, arrowWidth, durationInSec, hiddenByNearerObjects);

                float sizeApproximationOfVolume = circleDiameter;
                DrawCascadeOfArrows(distance_ofStartArrowsStartPos, distance_ofEndArrowsStartPos, sizeApproximationOfVolume, hasHit, hasUnlimitedLength, origin_V2, direction_normalized_V2, distanceOfFarestHit, arrowLength, arrowWidth, arrowsRelConeLenth, color_lowerAlpha, color_ofCastEnd_lowerAlpha, durationInSec, hiddenByNearerObjects);
                DrawCascadeOfVolumeSilhouette2Dslices(sizeApproximationOfVolume, hasHit, hasUnlimitedLength, origin_V2, direction_normalized_V2, maxDistance, distanceOfFarestHit, color_lowAlpha, color_ofCastEnd_lowAlpha, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                volumeCastOutlineVertice1_local = Vector2.zero;
                volumeCastOutlineVertice2_local = Vector2.zero;
            }

            DrawVolumeCastDirOutline(hasHit, origin_V2, endPos_V2, direction_normalized_V2, distanceOfFarestHit, maxDistance, color, color_ofCastEnd, durationInSec, hiddenByNearerObjects);
            WriteTextAtCircleCastOrigin(origin_V2, direction_normalized_V2, circleRadius, castsUp_insideXYPlane_90degCCFromCastDir_normalized_V3, circleRadiusTooSmall, color, nameText, hitCount, maxDistance, durationInSec, hiddenByNearerObjects);
        }

        static void DrawRayOfBoxcast(int hitCount, Vector2 origin_V2, Vector2 boxSize_V2, float angleDegCC, bool atLeastOneBoxDimIsSmallerThan0d00011, Vector2 direction_V2, float maxDistance, string nameText, float distanceOfFarestHit, float durationInSec, bool hiddenByNearerObjects)
        {
            bool hasHit = hitCount > 0;
            Vector2 direction_normalized_V2 = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(direction_V2);
            bool hasUnlimitedLength = float.IsInfinity(maxDistance);
            maxDistance = Mathf.Min(maxDistance, 100000.0f);
            maxDistance = Mathf.Max(maxDistance, -100000.0f);
            Vector2 endPos_V2 = origin_V2 + direction_normalized_V2 * maxDistance;
            Vector3 castsUp_insideXYPlane_90degCCFromCastDir_normalized_V3 = Vector3.Cross(Vector3.forward, direction_normalized_V2);

            Color color = hasHit ? DrawPhysics2D.colorForHittingCasts : DrawPhysics2D.colorForNonHittingCasts;
            Color color_lowAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.5f);
            Color color_lowerAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.25f);

            Color color_ofCastEnd = hasHit ? DrawPhysics2D.colorForCastLineBeyondHit : DrawPhysics2D.colorForNonHittingCasts;
            Color color_ofCastEnd_lowAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofCastEnd, 0.5f);
            Color color_ofCastEnd_lowerAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofCastEnd, 0.25f);

            bool boxScaleIsZero = UtilitiesDXXL_Math.ApproximatelyZero(boxSize_V2);
            Vector2 absBoxSize_V2 = UtilitiesDXXL_Math.Abs(boxSize_V2);
            float averageBoxSize = 0.5f * (absBoxSize_V2.x + absBoxSize_V2.y);

            if (boxScaleIsZero == false)
            {
                FillBoxCastOutlineVertices(direction_normalized_V2, boxSize_V2, angleDegCC);
                FillVolumeCastOutlineVertices_projectedIntoPerpToCastDirPlane(direction_V2);
                float heightOfCastCorridor_perpToCastDir = (volumeCastOutlineVertice1_projectedIntoPerpToCastDirPlane_local - volumeCastOutlineVertice2_projectedIntoPerpToCastDirPlane_local).magnitude;
                float arrowWidth = heightOfCastCorridor_perpToCastDir * 0.25f;
                float arrowLength = heightOfCastCorridor_perpToCastDir * 0.5f;
                float arrowsRelConeLenth = 0.45f;

                DrawBoxAtCastStartAndEnd(origin_V2, endPos_V2, boxSize_V2, angleDegCC, color, color_ofCastEnd_lowerAlpha, durationInSec, hiddenByNearerObjects);
                float startArrows_startDistanceFromStart = averageBoxSize * 0.5f + arrowLength;
                DrawArrowsAtVolumeCastStartAndEnd(out float distance_ofStartArrowsStartPos, out float distance_ofEndArrowsStartPos, hasHit, origin_V2, direction_normalized_V2, startArrows_startDistanceFromStart, maxDistance, distanceOfFarestHit, arrowLength, color_lowerAlpha, color_ofCastEnd_lowerAlpha, arrowsRelConeLenth, arrowWidth, durationInSec, hiddenByNearerObjects);

                float sizeApproximationOfVolume = averageBoxSize;
                DrawCascadeOfArrows(distance_ofStartArrowsStartPos, distance_ofEndArrowsStartPos, sizeApproximationOfVolume, hasHit, hasUnlimitedLength, origin_V2, direction_normalized_V2, distanceOfFarestHit, arrowLength, arrowWidth, arrowsRelConeLenth, color_lowerAlpha, color_ofCastEnd_lowerAlpha, durationInSec, hiddenByNearerObjects);
                DrawCascadeOfVolumeSilhouette2Dslices(sizeApproximationOfVolume, hasHit, hasUnlimitedLength, origin_V2, direction_normalized_V2, maxDistance, distanceOfFarestHit, color_lowAlpha, color_ofCastEnd_lowAlpha, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                volumeCastOutlineVertice1_local = Vector2.zero;
                volumeCastOutlineVertice2_local = Vector2.zero;
            }

            DrawVolumeCastDirOutline(hasHit, origin_V2, endPos_V2, direction_normalized_V2, distanceOfFarestHit, maxDistance, color, color_ofCastEnd, durationInSec, hiddenByNearerObjects);
            WriteTextAtBoxCastOrigin(origin_V2, direction_normalized_V2, castsUp_insideXYPlane_90degCCFromCastDir_normalized_V3, atLeastOneBoxDimIsSmallerThan0d00011, color, nameText, hitCount, maxDistance, averageBoxSize, durationInSec, hiddenByNearerObjects);
        }


        static void DrawRayOfCapsulecast(Vector2 origin_V2, Vector2 size_V2, bool atLeastOneBoxDimIsSmallerThan0d00011, CapsuleDirection2D capsuleDirection, float angleDegCC, int hitCount, Vector2 direction_V2, float maxDistance, string nameText, float distanceOfFarestHit, float durationInSec, bool hiddenByNearerObjects)
        {
            bool hasHit = hitCount > 0;
            Vector2 direction_normalized_V2 = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(direction_V2);
            Vector3 castsUp_insideXYPlane_90degCCFromCastDir_normalized_V3 = Vector3.Cross(Vector3.forward, direction_normalized_V2);
            bool hasUnlimitedLength = float.IsInfinity(maxDistance);
            maxDistance = Mathf.Min(maxDistance, 100000.0f);
            maxDistance = Mathf.Max(maxDistance, -100000.0f);

            Color color = hasHit ? DrawPhysics2D.colorForHittingCasts : DrawPhysics2D.colorForNonHittingCasts;
            Color color_lowAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.5f);
            Color color_lowerAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.25f);

            Color color_ofCastEnd = hasHit ? DrawPhysics2D.colorForCastLineBeyondHit : DrawPhysics2D.colorForNonHittingCasts;
            Color color_ofCastEnd_lowAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofCastEnd, 0.5f);
            Color color_ofCastEnd_lowerAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofCastEnd, 0.25f);

            Vector2 endPos_V2 = origin_V2 + direction_normalized_V2 * maxDistance;
            bool capsuleSizeIsZero = UtilitiesDXXL_Math.ApproximatelyZero(size_V2);
            Vector2 absSize_V2 = UtilitiesDXXL_Math.Abs(size_V2);
            absSize_V2 = ExpandCapsuleDimsToSufficeCapsuleDirectionType(absSize_V2, capsuleDirection);
            float capsuleRadius = (capsuleDirection == CapsuleDirection2D.Vertical) ? (0.5f * absSize_V2.x) : (0.5f * absSize_V2.y);

            if (capsuleSizeIsZero == false)
            {
                float sizeAlongAbsLongerCapsuleDir = Mathf.Max(absSize_V2.x, absSize_V2.y);
                Vector2 unturned_vectorToACircleCenter_local = (capsuleDirection == CapsuleDirection2D.Vertical) ? (Vector2.up * (0.5f * sizeAlongAbsLongerCapsuleDir - capsuleRadius)) : (Vector2.right * (0.5f * sizeAlongAbsLongerCapsuleDir - capsuleRadius));
                float distanceBetweenCircles = (capsuleDirection == CapsuleDirection2D.Vertical) ? (absSize_V2.y - 2.0f * capsuleRadius) : (absSize_V2.x - 2.0f * capsuleRadius);
                float capsuleCirclesDiameter = 2.0f * capsuleRadius;
                float sizeApproximationOfCapsule = capsuleCirclesDiameter + 0.5f * distanceBetweenCircles;
                Quaternion capsuleRotation = Quaternion.AngleAxis(angleDegCC, Vector3.forward);
                Vector2 turned_vectorToACircleCenter_local = capsuleRotation * unturned_vectorToACircleCenter_local;

                FillCapsuleCastOutlineVertices(turned_vectorToACircleCenter_local, -turned_vectorToACircleCenter_local, direction_normalized_V2, capsuleRadius);
                FillVolumeCastOutlineVertices_projectedIntoPerpToCastDirPlane(direction_V2);
                float heightOfCastCorridor_perpToCastDir = (volumeCastOutlineVertice1_projectedIntoPerpToCastDirPlane_local - volumeCastOutlineVertice2_projectedIntoPerpToCastDirPlane_local).magnitude;
                float arrowWidth = heightOfCastCorridor_perpToCastDir * 0.25f;
                float arrowLength = heightOfCastCorridor_perpToCastDir * 0.5f;
                float arrowsRelConeLenth = 0.45f;

                DrawCapsuleAtCastStartAndEnd(origin_V2, endPos_V2, size_V2, capsuleDirection, angleDegCC, color, color_ofCastEnd_lowerAlpha, durationInSec, hiddenByNearerObjects);
                float distanceOfCapsulesFarestPosFromOrigin_alongCastDir = GetDistanceOfCapsulesFarestPosFromOrigin_alongCastDir(direction_normalized_V2, turned_vectorToACircleCenter_local, capsuleRadius);
                float startArrows_startDistanceFromStart = distanceOfCapsulesFarestPosFromOrigin_alongCastDir + capsuleRadius;
                DrawArrowsAtVolumeCastStartAndEnd(out float distance_ofStartArrowsStartPos, out float distance_ofEndArrowsStartPos, hasHit, origin_V2, direction_normalized_V2, startArrows_startDistanceFromStart, maxDistance, distanceOfFarestHit, arrowLength, color_lowerAlpha, color_ofCastEnd_lowerAlpha, arrowsRelConeLenth, arrowWidth, durationInSec, hiddenByNearerObjects);
                DrawCascadeOfArrows(distance_ofStartArrowsStartPos, distance_ofEndArrowsStartPos, sizeApproximationOfCapsule, hasHit, hasUnlimitedLength, origin_V2, direction_normalized_V2, distanceOfFarestHit, arrowLength, arrowWidth, arrowsRelConeLenth, color_lowerAlpha, color_ofCastEnd_lowerAlpha, durationInSec, hiddenByNearerObjects);
                DrawCascadeOfVolumeSilhouette2Dslices(sizeApproximationOfCapsule, hasHit, hasUnlimitedLength, origin_V2, direction_normalized_V2, maxDistance, distanceOfFarestHit, color_lowAlpha, color_ofCastEnd_lowAlpha, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                volumeCastOutlineVertice1_local = Vector2.zero;
                volumeCastOutlineVertice2_local = Vector2.zero;
            }

            DrawVolumeCastDirOutline(hasHit, origin_V2, endPos_V2, direction_normalized_V2, distanceOfFarestHit, maxDistance, color, color_ofCastEnd, durationInSec, hiddenByNearerObjects);
            WriteTextAtCapsuleCastOrigin(origin_V2, direction_normalized_V2, capsuleRadius, castsUp_insideXYPlane_90degCCFromCastDir_normalized_V3, atLeastOneBoxDimIsSmallerThan0d00011, color, nameText, hitCount, maxDistance, durationInSec, hiddenByNearerObjects);
        }

        static void DrawCircleAtCastStartAndEnd(Vector2 origin_V2, Vector2 endPos_V2, float circleRadius, Color color, Color color_ofCastEnd_lowerAlpha, float durationInSec, bool hiddenByNearerObjects)
        {
            //start circle:
            DrawShapes.Circle2D(origin_V2, circleRadius, color, GetZPosForDrawVisualisation(), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.solid, false, durationInSec, hiddenByNearerObjects);

            //end circle:
            if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                DrawShapes.Circle2D(endPos_V2, circleRadius, color_ofCastEnd_lowerAlpha, GetZPosForDrawVisualisation(), 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.solid, false, durationInSec, hiddenByNearerObjects);
            }
        }

        static void DrawBoxAtCastStartAndEnd(Vector2 origin_V2, Vector2 endPos_V2, Vector2 boxSize_V2, float angleDegCC, Color color, Color color_ofCastEnd_lowerAlpha, float durationInSec, bool hiddenByNearerObjects)
        {
            //start box:
            DrawShapes.Box2D(origin_V2, boxSize_V2, color, GetZPosForDrawVisualisation(), angleDegCC, DrawShapes.Shape2DType.square, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.solid, false, durationInSec, hiddenByNearerObjects);

            //end box:
            if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                DrawShapes.Box2D(endPos_V2, boxSize_V2, color_ofCastEnd_lowerAlpha, GetZPosForDrawVisualisation(), angleDegCC, DrawShapes.Shape2DType.square, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.solid, false, durationInSec, hiddenByNearerObjects);
            }
        }

        static void DrawCapsuleAtCastStartAndEnd(Vector2 origin_atCastStart_V2, Vector2 origin_atCastEnd_V2, Vector2 size_V2, CapsuleDirection2D capsuleDirection, float angleDegCC, Color color, Color color_ofCastEnd_lowerAlpha, float durationInSec, bool hiddenByNearerObjects)
        {
            //start capsule:
            DrawShapes.Capsule2D(origin_atCastStart_V2, size_V2, color, GetZPosForDrawVisualisation(), capsuleDirection, angleDegCC, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.solid, false, false, durationInSec, hiddenByNearerObjects);

            //end capsule:
            if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                DrawShapes.Capsule2D(origin_atCastEnd_V2, size_V2, color_ofCastEnd_lowerAlpha, GetZPosForDrawVisualisation(), capsuleDirection, angleDegCC, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.solid, false, false, durationInSec, hiddenByNearerObjects);
            }
        }

        static Vector2 ExpandCapsuleDimsToSufficeCapsuleDirectionType(Vector2 absSize_preExpanding_V2, CapsuleDirection2D capsuleDirection)
        {
            if (capsuleDirection == CapsuleDirection2D.Vertical)
            {
                absSize_preExpanding_V2.y = Mathf.Max(absSize_preExpanding_V2.x, absSize_preExpanding_V2.y);
            }
            else
            {
                absSize_preExpanding_V2.x = Mathf.Max(absSize_preExpanding_V2.x, absSize_preExpanding_V2.y);
            }
            return absSize_preExpanding_V2;
        }

        static float GetDistanceOfCapsulesFarestPosFromOrigin_alongCastDir(Vector2 direction_normalized_V2, Vector2 turned_vectorToACircleCenter_local, float capsuleRadius)
        {
            Vector2 circle1Pos_local = turned_vectorToACircleCenter_local;
            Vector2 circle2Pos_local = -turned_vectorToACircleCenter_local;

            if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized_V2, circle1Pos_local))
            {
                return circle1Pos_local.magnitude + capsuleRadius;
            }
            else
            {
                return circle2Pos_local.magnitude + capsuleRadius;
            }
        }

        static void DrawArrowsAtVolumeCastStartAndEnd(out float distance_ofStartArrowsStartPos, out float distance_ofEndArrowsStartPos, bool hasHit, Vector2 origin_V2, Vector2 direction_normalized_V2, float distanceFromOrigin_toStartOfStartArrow, float maxDistance, float distanceOfFarestHit, float arrowLength, Color color_lowerAlpha, Color color_ofCastEnd_lowerAlpha, float arrowsRelConeLenth, float arrowWidth, float durationInSec, bool hiddenByNearerObjects)
        {
            distance_ofStartArrowsStartPos = 0.0f;
            distance_ofEndArrowsStartPos = 0.0f;

            if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes) { return; }
            if (arrowLength > UtilitiesDXXL_Physics.minArrowLength)
            {
                //at start:
                float original_distanceFromOrigin_toStartOfStartArrow = distanceFromOrigin_toStartOfStartArrow;
                distanceFromOrigin_toStartOfStartArrow = Mathf.Min(distanceFromOrigin_toStartOfStartArrow, 0.5f * maxDistance);
                if (hasHit) { distanceFromOrigin_toStartOfStartArrow = Mathf.Min(distanceFromOrigin_toStartOfStartArrow, 0.5f * distanceOfFarestHit); }
                if (distanceFromOrigin_toStartOfStartArrow <= 0.0f) { distanceFromOrigin_toStartOfStartArrow = original_distanceFromOrigin_toStartOfStartArrow; }
                bool isAfterLastHit = hasHit && ((distanceFromOrigin_toStartOfStartArrow + 0.5f * arrowLength) > distanceOfFarestHit);
                distance_ofStartArrowsStartPos = distanceFromOrigin_toStartOfStartArrow;
                Vector2 startVector_startPos_V2 = origin_V2 + direction_normalized_V2 * distanceFromOrigin_toStartOfStartArrow;
                Vector2 startVector_endPos_V2 = origin_V2 + direction_normalized_V2 * (distanceFromOrigin_toStartOfStartArrow + arrowLength);
                UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                DrawBasics2D.Vector(startVector_startPos_V2, startVector_endPos_V2, isAfterLastHit ? color_ofCastEnd_lowerAlpha : color_lowerAlpha, arrowWidth, null, arrowsRelConeLenth, false, GetZPosForDrawVisualisation(), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();

                //at end:
                float distanceFromOrigin_toStartOfEndArrow = maxDistance - original_distanceFromOrigin_toStartOfStartArrow - arrowLength;
                distance_ofEndArrowsStartPos = distanceFromOrigin_toStartOfEndArrow;
                if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.high_withFullDetails)
                {
                    if (maxDistance > (distanceFromOrigin_toStartOfStartArrow + 12.0f * arrowLength))
                    {
                        isAfterLastHit = hasHit && ((distanceFromOrigin_toStartOfEndArrow + 0.5f * arrowLength) > distanceOfFarestHit);
                        Vector2 endVector_startPos_V2 = origin_V2 + direction_normalized_V2 * distanceFromOrigin_toStartOfEndArrow;
                        Vector2 endVector_endPos_V2 = origin_V2 + direction_normalized_V2 * (distanceFromOrigin_toStartOfEndArrow + arrowLength);
                        UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                        DrawBasics2D.Vector(endVector_startPos_V2, endVector_endPos_V2, isAfterLastHit ? color_ofCastEnd_lowerAlpha : color_lowerAlpha, arrowWidth, null, arrowsRelConeLenth, false, GetZPosForDrawVisualisation(), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                        UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                    }
                }
            }
        }

        static void FillCircleCastOutlineVertices(Vector2 direction_normalized_V2, float circleRadius)
        {
            volumeCastOutlineVertice1_local = Vector3.Cross(Vector3.forward, direction_normalized_V2) * circleRadius;
            volumeCastOutlineVertice2_local = -volumeCastOutlineVertice1_local;
        }

        ///cube definition:
        //viewed along z-forward:
        //starts with: nearer square, lowLeft, then counterclockwise
        static Vector2[] unscaledUnrotatedBox2D = new Vector2[4] { new Vector2(-0.5f, -0.5f), new Vector2(0.5f, -0.5f), new Vector2(0.5f, 0.5f), new Vector2(-0.5f, 0.5f) };
        static void FillBoxCastOutlineVertices(Vector2 direction_normalized_V2, Vector2 boxSize_V2, float angleDegCC)
        {
            Quaternion boxRotation = Quaternion.AngleAxis(angleDegCC, Vector3.forward);
            Vector2 toUp_ofBox = boxRotation * Vector3.up;
            Vector2 toRight_ofBox = boxRotation * Vector3.right;

            if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized_V2, toRight_ofBox))
            {
                if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized_V2, toUp_ofBox))
                {
                    volumeCastOutlineVertice1_local = boxRotation * Vector2.Scale(unscaledUnrotatedBox2D[1], boxSize_V2);
                    volumeCastOutlineVertice2_local = boxRotation * Vector2.Scale(unscaledUnrotatedBox2D[3], boxSize_V2);
                }
                else
                {
                    volumeCastOutlineVertice1_local = boxRotation * Vector2.Scale(unscaledUnrotatedBox2D[0], boxSize_V2);
                    volumeCastOutlineVertice2_local = boxRotation * Vector2.Scale(unscaledUnrotatedBox2D[2], boxSize_V2);
                }
            }
            else
            {
                if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized_V2, toUp_ofBox))
                {
                    volumeCastOutlineVertice1_local = boxRotation * Vector2.Scale(unscaledUnrotatedBox2D[0], boxSize_V2);
                    volumeCastOutlineVertice2_local = boxRotation * Vector2.Scale(unscaledUnrotatedBox2D[2], boxSize_V2);
                }
                else
                {
                    volumeCastOutlineVertice1_local = boxRotation * Vector2.Scale(unscaledUnrotatedBox2D[1], boxSize_V2);
                    volumeCastOutlineVertice2_local = boxRotation * Vector2.Scale(unscaledUnrotatedBox2D[3], boxSize_V2);
                }
            }
        }

        static void FillCapsuleCastOutlineVertices(Vector2 posOfCapsuleCircle1_local, Vector2 posOfCapsuleCircle2_local, Vector2 direction_normalized_V2, float capsuleRadius)
        {
            Vector2 up_insideXYPlane_ofCastDir_normalized = Vector3.Cross(Vector3.forward, direction_normalized_V2);
            bool circle1_isHigher_seenAlongCastDir = UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(posOfCapsuleCircle1_local, up_insideXYPlane_ofCastDir_normalized);
            if (circle1_isHigher_seenAlongCastDir)
            {
                volumeCastOutlineVertice1_local = posOfCapsuleCircle1_local + up_insideXYPlane_ofCastDir_normalized * capsuleRadius;
                volumeCastOutlineVertice2_local = posOfCapsuleCircle2_local - up_insideXYPlane_ofCastDir_normalized * capsuleRadius;
            }
            else
            {
                volumeCastOutlineVertice1_local = posOfCapsuleCircle1_local - up_insideXYPlane_ofCastDir_normalized * capsuleRadius;
                volumeCastOutlineVertice2_local = posOfCapsuleCircle2_local + up_insideXYPlane_ofCastDir_normalized * capsuleRadius;
            }
        }

        static InternalDXXL_Plane plane_throughWorldOrigin_containingZAxis_perpToCastDir = new InternalDXXL_Plane();
        static void FillVolumeCastOutlineVertices_projectedIntoPerpToCastDirPlane(Vector2 direction_V2)
        {
            Vector3 direction_V3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(direction_V2);
            plane_throughWorldOrigin_containingZAxis_perpToCastDir.Recreate(Vector3.zero, direction_V3);
            volumeCastOutlineVertice1_projectedIntoPerpToCastDirPlane_local = plane_throughWorldOrigin_containingZAxis_perpToCastDir.Get_perpProjectionOfPointOnPlane(volumeCastOutlineVertice1_local);
            volumeCastOutlineVertice2_projectedIntoPerpToCastDirPlane_local = -volumeCastOutlineVertice1_projectedIntoPerpToCastDirPlane_local;
            distanceOfOutlineVerticesToOrigin_alongCastsUp = volumeCastOutlineVertice1_projectedIntoPerpToCastDirPlane_local.magnitude;
        }

        static void DrawVolumeCastDirOutline(bool hasHit, Vector2 origin_V2, Vector2 endPos_V2, Vector2 direction_normalized_V2, float distanceOfFarestHit, float maxDistance, Color color, Color color_ofCastEnd, float durationInSec, bool hiddenByNearerObjects)
        {
            if (hasHit)
            {
                if (maxDistance < 0.0f)
                {
                    //unity.RaycastHit2D always returns positve values for "distance", also if the cast goes backward due to negative "maxDistance"
                    distanceOfFarestHit = -distanceOfFarestHit;
                }

                Vector2 posOfFarestHit_V2 = origin_V2 + direction_normalized_V2 * distanceOfFarestHit;

                //vertice1 toFarest hit, then to end:
                Line_fadeableAnimSpeed_2D.InternalDraw(origin_V2 + volumeCastOutlineVertice1_local, posOfFarestHit_V2 + volumeCastOutlineVertice1_local, color, 0.0f, null, DrawBasics.LineStyle.solid, GetZPosForDrawVisualisation(), 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                Line_fadeableAnimSpeed_2D.InternalDraw(posOfFarestHit_V2 + volumeCastOutlineVertice1_local, endPos_V2 + volumeCastOutlineVertice1_local, color_ofCastEnd, 0.0f, null, DrawBasics.LineStyle.solid, GetZPosForDrawVisualisation(), 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

                //vertice2 toFarest hit, then to end:
                Line_fadeableAnimSpeed_2D.InternalDraw(origin_V2 + volumeCastOutlineVertice2_local, posOfFarestHit_V2 + volumeCastOutlineVertice2_local, color, 0.0f, null, DrawBasics.LineStyle.solid, GetZPosForDrawVisualisation(), 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                Line_fadeableAnimSpeed_2D.InternalDraw(posOfFarestHit_V2 + volumeCastOutlineVertice2_local, endPos_V2 + volumeCastOutlineVertice2_local, color_ofCastEnd, 0.0f, null, DrawBasics.LineStyle.solid, GetZPosForDrawVisualisation(), 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }
            else
            {
                //vertice1 to end:
                Line_fadeableAnimSpeed_2D.InternalDraw(origin_V2 + volumeCastOutlineVertice1_local, endPos_V2 + volumeCastOutlineVertice1_local, color, 0.0f, null, DrawBasics.LineStyle.solid, GetZPosForDrawVisualisation(), 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                //vertice2 to end:
                Line_fadeableAnimSpeed_2D.InternalDraw(origin_V2 + volumeCastOutlineVertice2_local, endPos_V2 + volumeCastOutlineVertice2_local, color, 0.0f, null, DrawBasics.LineStyle.solid, GetZPosForDrawVisualisation(), 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }
        }

        static void DrawCascadeOfVolumeSilhouette2Dslices(float sizeApproximationOfVolume, bool hasHit, bool hasUnlimitedLength, Vector2 origin_V2, Vector2 direction_normalized_V2, float maxDistance, float distanceOfFarestHit, Color color_lowAlpha, Color color_ofCastEnd_lowAlpha, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes) { return; }
            if (UtilitiesDXXL_Math.ApproximatelyZero(DrawPhysics2D.castCorridorVisualizerDensity) == false)
            {
                float distanceBetweenSilhouettes = hasUnlimitedLength ? (4.0f * sizeApproximationOfVolume) : (2.2f * sizeApproximationOfVolume);
                int maxSilhouettesPerVolumeCast = hasUnlimitedLength ? 50 : 200;
                if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes)
                {
                    maxSilhouettesPerVolumeCast = hasUnlimitedLength ? 20 : 40;
                }

                float used_castSilhouetteVisualizerDensity = DrawPhysics2D.castCorridorVisualizerDensity;
                used_castSilhouetteVisualizerDensity = Mathf.Max(used_castSilhouetteVisualizerDensity, 0.01f);
                used_castSilhouetteVisualizerDensity = Mathf.Min(used_castSilhouetteVisualizerDensity, 1000.0f);
                distanceBetweenSilhouettes = distanceBetweenSilhouettes / used_castSilhouetteVisualizerDensity;
                if (used_castSilhouetteVisualizerDensity > 1.0f) { maxSilhouettesPerVolumeCast = Mathf.RoundToInt(used_castSilhouetteVisualizerDensity * maxSilhouettesPerVolumeCast); }
                maxSilhouettesPerVolumeCast = Mathf.Min(maxSilhouettesPerVolumeCast, DrawPhysics2D.maxCorridorVisualizersPerCastVisualization);

                distanceBetweenSilhouettes = Mathf.Max(distanceBetweenSilhouettes, 0.1f);
                float distance_ofCurrSilhouette = 0.0f;
                for (int i_silhouette = 0; i_silhouette < maxSilhouettesPerVolumeCast; i_silhouette++)
                {
                    distance_ofCurrSilhouette = distance_ofCurrSilhouette + distanceBetweenSilhouettes;
                    bool isAfterLastHit = hasHit && (distance_ofCurrSilhouette > distanceOfFarestHit);
                    if (distance_ofCurrSilhouette < maxDistance)
                    {
                        Vector2 posOfCurrSilhouetteOnRayLine_V2 = origin_V2 + distance_ofCurrSilhouette * direction_normalized_V2;
                        Line_fadeableAnimSpeed_2D.InternalDraw(posOfCurrSilhouetteOnRayLine_V2 + volumeCastOutlineVertice1_projectedIntoPerpToCastDirPlane_local, posOfCurrSilhouetteOnRayLine_V2 + volumeCastOutlineVertice2_projectedIntoPerpToCastDirPlane_local, isAfterLastHit ? color_ofCastEnd_lowAlpha : color_lowAlpha, 0.0f, null, DrawBasics.LineStyle.solid, GetZPosForDrawVisualisation(), 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                    }
                    else
                    {
                        break;
                    }

                    if (hasUnlimitedLength)
                    {
                        distanceBetweenSilhouettes = (1.0f + (0.4f / used_castSilhouetteVisualizerDensity)) * distanceBetweenSilhouettes;
                    }
                    else
                    {
                        if (i_silhouette > 3)
                        {
                            distanceBetweenSilhouettes = (1.0f + (0.25f / used_castSilhouetteVisualizerDensity)) * distanceBetweenSilhouettes;
                        }
                    }
                }
            }
        }

        static void DrawCascadeOfArrows(float distance_ofStartArrowsStartPos, float distance_ofEndArrowsStartPos, float sizeApproximationOfVolume, bool hasHit, bool hasUnlimitedLength, Vector2 origin_V2, Vector2 direction_normalized_V2, float distanceOfFarestHit, float arrowLength, float arrowWidth, float arrowsRelConeLenth, Color color_lowerAlpha, Color color_ofCastEnd_lowerAlpha, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.high_withFullDetails)
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(DrawPhysics2D.castCorridorVisualizerDensity) == false)
                {
                    if (arrowLength > UtilitiesDXXL_Physics.minArrowLength)
                    {
                        float distanceBetweenArrows = hasUnlimitedLength ? (6.0f * sizeApproximationOfVolume) : (3.25f * sizeApproximationOfVolume);
                        int maxArrowsPerVolumeCast = hasUnlimitedLength ? 5 : 15;

                        distanceBetweenArrows = Mathf.Max(distanceBetweenArrows, 0.5f);
                        float distance_ofCurrArrowsStart = distance_ofStartArrowsStartPos + distanceBetweenArrows;
                        for (int i_arrow = 0; i_arrow < maxArrowsPerVolumeCast; i_arrow++)
                        {
                            if (distance_ofCurrArrowsStart < distance_ofEndArrowsStartPos)
                            {
                                bool isAfterLastHit = hasHit && ((distance_ofCurrArrowsStart + 0.5f * arrowLength) > distanceOfFarestHit);
                                Vector2 vectorStartPos_V2 = origin_V2 + distance_ofCurrArrowsStart * direction_normalized_V2;
                                Vector2 vectorEndPos_V2 = origin_V2 + (distance_ofCurrArrowsStart + arrowLength) * direction_normalized_V2;
                                UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                                DrawBasics2D.Vector(vectorStartPos_V2, vectorEndPos_V2, isAfterLastHit ? color_ofCastEnd_lowerAlpha : color_lowerAlpha, arrowWidth, null, arrowsRelConeLenth, false, GetZPosForDrawVisualisation(), false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                            }
                            else
                            {
                                break;
                            }

                            if (hasUnlimitedLength)
                            {
                                distanceBetweenArrows = 2.0f * distanceBetweenArrows;
                            }
                            else
                            {
                                if (i_arrow > 3)
                                {
                                    distanceBetweenArrows = 1.5f * distanceBetweenArrows;
                                }
                            }
                            distance_ofCurrArrowsStart = distance_ofCurrArrowsStart + distanceBetweenArrows;
                        }
                    }
                }
            }
        }

        static void WriteTextAtCircleCastOrigin(Vector2 origin_V2, Vector2 direction_normalized_V2, float circleRadius, Vector3 castsUp_insideXYPlane_90degCCFromCastDir_normalized_V3, bool circleRadiusTooSmall, Color color, string nameText, int hitCount, float maxDistance, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes) { return; }
            float textSize = 0.2f * circleRadius;
            string additinalWarningText = circleRadiusTooSmall ? "[<color=#e2aa00FF><icon=warning></color> circle radius too small -> potentially collisions missing]<br>" : null;
            string castText = GetVolumeCastStartTextString("<size=27>Circlecast2D</size><br><size=5> </size><br>number of hits: ", nameText, hitCount, maxDistance, additinalWarningText);
            WriteTextAtVolumeCastOrigin(castText, origin_V2, castsUp_insideXYPlane_90degCCFromCastDir_normalized_V3, direction_normalized_V2, textSize, color, durationInSec, hiddenByNearerObjects);
        }

        static void WriteTextAtBoxCastOrigin(Vector2 origin_V2, Vector2 direction_normalized_V2, Vector3 castsUp_insideXYPlane_90degCCFromCastDir_normalized_V3, bool atLeastOneBoxDimIsSmallerThan0d00011, Color color, string nameText, int hitCount, float maxDistance, float averageBoxSize, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes) { return; }
            float textSize = 0.1f * averageBoxSize;
            string additinalWarningText = atLeastOneBoxDimIsSmallerThan0d00011 ? "[<color=#e2aa00FF><icon=warning></color> box dimension is very small -> potentially collisions missing]<br>" : null;
            string castText = GetVolumeCastStartTextString("<size=27>Boxcast2D</size><br><size=5> </size><br>number of hits: ", nameText, hitCount, maxDistance, additinalWarningText);
            WriteTextAtVolumeCastOrigin(castText, origin_V2, castsUp_insideXYPlane_90degCCFromCastDir_normalized_V3, direction_normalized_V2, textSize, color, durationInSec, hiddenByNearerObjects);
        }

        static void WriteTextAtCapsuleCastOrigin(Vector2 origin_V2, Vector2 direction_normalized_V2, float capsuleRadius, Vector3 castsUp_insideXYPlane_90degCCFromCastDir_normalized_V3, bool atLeastOneBoxDimIsSmallerThan0d00011, Color color, string nameText, int hitCount, float maxDistance, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes) { return; }
            float textSize = 0.2f * capsuleRadius;
            string additinalWarningText = atLeastOneBoxDimIsSmallerThan0d00011 ? "[<color=#e2aa00FF><icon=warning></color> capsule dimension is very small -> potentially collisions missing]<br>" : null;
            string castText = GetVolumeCastStartTextString("<size=27>Capsulecast2D</size><br><size=5> </size><br>number of hits: ", nameText, hitCount, maxDistance, additinalWarningText);
            WriteTextAtVolumeCastOrigin(castText, origin_V2, castsUp_insideXYPlane_90degCCFromCastDir_normalized_V3, direction_normalized_V2, textSize, color, durationInSec, hiddenByNearerObjects);
        }

        static void WriteTextAtVolumeCastOrigin(string castText, Vector2 origin_V2, Vector3 castsUp_insideXYPlane_90degCCFromCastDir_normalized_V3, Vector2 direction_normalized_V2, float textSize, Color color, float durationInSec, bool hiddenByNearerObjects)
        {
            textSize = Mathf.Max(textSize, 0.02f);
            Color textColor = Color.Lerp(color, Color.black, 0.75f);
            Vector2 textPos = origin_V2 + (Vector2)castsUp_insideXYPlane_90degCCFromCastDir_normalized_V3 * (distanceOfOutlineVerticesToOrigin_alongCastsUp + 0.65f * textSize);
            bool direction_isTowardsRight = (direction_normalized_V2.x >= 0.0f);
            Vector2 textDir = direction_isTowardsRight ? direction_normalized_V2 : (-direction_normalized_V2);
            DrawText.TextAnchorDXXL textAnchor = direction_isTowardsRight ? DrawText.TextAnchorDXXL.LowerLeft : DrawText.TextAnchorDXXL.UpperRight;
            UtilitiesDXXL_Text.Write2DFramed(castText, textPos, textColor, textSize, textDir, textAnchor, GetZPosForDrawVisualisation(), DrawBasics.LineStyle.solid, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
        }

        static string GetVolumeCastStartTextString(string volumeCastTypeIdentifyingStringPart, string nameText, int hitCount, float maxDistance, string additinalWarningText)
        {
            additinalWarningText = UtilitiesDXXL_Math.ApproximatelyZero(maxDistance) ? "[<color=#e2aa00FF><icon=warning></color> cast distance is zero]<br>" + additinalWarningText : additinalWarningText;
            additinalWarningText = (maxDistance < 0.0f) ? "[<color=#e2aa00FF><icon=warning></color> negative cast direction]<br>" + additinalWarningText : additinalWarningText;

            string startText;
            if (DrawPhysics2D.drawCastNameTag_atCastOrigin && nameText != null && nameText.Length != 0)
            {
                startText = (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) ? additinalWarningText + UtilitiesDXXL_Physics.GetSizeMarkupStartStringForCastNames(nameText) + nameText + "</size><br><size=5> </size><br>hits: " + hitCount : additinalWarningText + UtilitiesDXXL_Physics.GetSizeMarkupStartStringForCastNames(nameText) + nameText + "</size><br><size=5> </size><br>number of hits: " + hitCount;
            }
            else
            {
                startText = (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) ? additinalWarningText + "hits: " + hitCount : additinalWarningText + volumeCastTypeIdentifyingStringPart + hitCount;
            }

            return startText;
        }

        static void DrawCirclecastHitInfo(float circleRadius, RaycastHit2D hitInfo2D, int i_hit, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.VectorIsInvalid(hitInfo2D.point) || UtilitiesDXXL_Math.VectorIsInvalid(hitInfo2D.normal))
            {
                Debug.LogError("Draw XXL: A 'Physics2D.CircleCast()' returned an invalid hit position (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.point) + ") or normal (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.normal) + "). The drawn cast visualization may be incorrect.");
            }
            else
            {
                bool saveDrawnLines = i_hit >= DrawPhysics2D.hitResultsWithMoreDetailedDisplay;
                if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.high_withFullDetails) { saveDrawnLines = true; }

                Vector3 impactPos_V3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(hitInfo2D.point, hitInfo2D.transform.position.z);
                DrawCircleAtHitPos(hitInfo2D, circleRadius, durationInSec, hiddenByNearerObjects);
                DrawNormalAtVolumeCastHitPos(impactPos_V3, saveDrawnLines, hitInfo2D, durationInSec, hiddenByNearerObjects);
                DrawDashedLineAlongZ(impactPos_V3, hitInfo2D, durationInSec, hiddenByNearerObjects);
                string text = GetTextAtHitPos_forVolumeCast("<sw=80000>Circlecast2D hit #", saveDrawnLines, hitInfo2D, i_hit, nameText);
                string additionalWarningText = null;
                DrawTextDescriptionAtVolumeCastHitPos(impactPos_V3, hitInfo2D, nameText, text, durationInSec, hiddenByNearerObjects, additionalWarningText);
            }
        }

        static void DrawBoxcastHitInfo(Vector2 boxSize_V2, float angleDegCC, RaycastHit2D hitInfo2D, int i_hit, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.VectorIsInvalid(hitInfo2D.point) || UtilitiesDXXL_Math.VectorIsInvalid(hitInfo2D.normal))
            {
                Debug.LogError("Draw XXL: A 'Physics2D.BoxCast()' returned an invalid hit position (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.point) + ") or normal (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.normal) + "). The drawn cast visualization may be incorrect.");
            }
            else
            {
                bool saveDrawnLines = i_hit >= DrawPhysics2D.hitResultsWithMoreDetailedDisplay;
                if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.high_withFullDetails) { saveDrawnLines = true; }

                Vector3 impactPos_V3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(hitInfo2D.point, hitInfo2D.transform.position.z);
                DrawBoxAtHitPos(boxSize_V2, angleDegCC, hitInfo2D, durationInSec, hiddenByNearerObjects);
                DrawNormalAtVolumeCastHitPos(impactPos_V3, saveDrawnLines, hitInfo2D, durationInSec, hiddenByNearerObjects);
                DrawDashedLineAlongZ(impactPos_V3, hitInfo2D, durationInSec, hiddenByNearerObjects);
                string text = GetTextAtHitPos_forVolumeCast("<sw=80000>Boxcast2D hit #", saveDrawnLines, hitInfo2D, i_hit, nameText);
                string additionalWarningText = null;
                DrawTextDescriptionAtVolumeCastHitPos(impactPos_V3, hitInfo2D, nameText, text, durationInSec, hiddenByNearerObjects, additionalWarningText);
            }
        }

        static void DrawCapsulecastHitInfo(Vector2 size_V2, CapsuleDirection2D capsuleDirection, float angleDegCC, RaycastHit2D hitInfo2D, int i_hit, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.VectorIsInvalid(hitInfo2D.point) || UtilitiesDXXL_Math.VectorIsInvalid(hitInfo2D.normal))
            {
                Debug.LogError("Draw XXL: A 'Physics2D.CapsuleCast()' returned an invalid hit position (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.point) + ") or normal (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.normal) + "). The drawn cast visualization may be incorrect.");
            }
            else
            {
                bool saveDrawnLines = i_hit >= DrawPhysics2D.hitResultsWithMoreDetailedDisplay;
                if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.high_withFullDetails) { saveDrawnLines = true; }

                Vector3 impactPos_V3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(hitInfo2D.point, hitInfo2D.transform.position.z);
                DrawCapsuleAtHitPos(size_V2, capsuleDirection, angleDegCC, hitInfo2D, i_hit, durationInSec, hiddenByNearerObjects);
                DrawNormalAtVolumeCastHitPos(impactPos_V3, saveDrawnLines, hitInfo2D, durationInSec, hiddenByNearerObjects);
                DrawDashedLineAlongZ(impactPos_V3, hitInfo2D, durationInSec, hiddenByNearerObjects);
                string text = GetTextAtHitPos_forVolumeCast("<sw=80000>Capsulecast2D hit #", saveDrawnLines, hitInfo2D, i_hit, nameText);
                string additionalWarningText = null;
                DrawTextDescriptionAtVolumeCastHitPos(impactPos_V3, hitInfo2D, nameText, text, durationInSec, hiddenByNearerObjects, additionalWarningText);
            }
        }

        static void DrawCircleAtHitPos(RaycastHit2D hitInfo2D, float circleRadius, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.VectorIsInvalid(hitInfo2D.centroid))
            {
                Debug.LogError("Draw XXL: A 'Physics2D.CircleCast()' returned an invalid hit position centroid (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.centroid) + "). The drawn cast visualization may be incorrect.");
            }
            else
            {
                Color color_ofHittingVolume = Color.Lerp(DrawPhysics2D.colorForHittingCasts, Color.white, 0.6f);
                DrawShapes.Circle2D(hitInfo2D.centroid, circleRadius, color_ofHittingVolume, hitInfo2D.transform.position.z, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.solid, false, durationInSec, hiddenByNearerObjects);
            }
        }

        static void DrawBoxAtHitPos(Vector2 boxSize_V2, float angleDegCC, RaycastHit2D hitInfo2D, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.VectorIsInvalid(hitInfo2D.centroid))
            {
                Debug.LogError("Draw XXL: A 'Physics2D.BoxCast()' returned an invalid hit position centroid (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.centroid) + "). The drawn cast visualization may be incorrect.");
            }
            else
            {
                Color color_ofHittingVolume = Color.Lerp(DrawPhysics2D.colorForHittingCasts, Color.white, 0.6f);
                DrawShapes.Box2D(hitInfo2D.centroid, boxSize_V2, color_ofHittingVolume, hitInfo2D.transform.position.z, angleDegCC, DrawShapes.Shape2DType.square, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.solid, false, durationInSec, hiddenByNearerObjects);
            }
        }

        static void DrawCapsuleAtHitPos(Vector2 size_V2, CapsuleDirection2D capsuleDirection, float angleDegCC, RaycastHit2D hitInfo2D, int i_hit, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.VectorIsInvalid(hitInfo2D.centroid))
            {
                Debug.LogError("Draw XXL: A 'Physics2D.CapsuleCast()' returned an invalid hit position centroid (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.centroid) + "). The drawn cast visualization may be incorrect.");
            }
            else
            {
                Color color_ofHittingVolume = Color.Lerp(DrawPhysics2D.colorForHittingCasts, Color.white, 0.6f);
                DrawShapes.Capsule2D(hitInfo2D.centroid, size_V2, color_ofHittingVolume, hitInfo2D.transform.position.z, capsuleDirection, angleDegCC, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.solid, false, false, durationInSec, hiddenByNearerObjects);
            }
        }

        static void DrawNormalAtVolumeCastHitPos(Vector3 impactPos_V3, bool saveDrawnLines, RaycastHit2D hitInfo2D, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 normal_V3 = UtilitiesDXXL_DrawBasics2D.Direction_V2toV3(hitInfo2D.normal);
            Color color_ofNormal = Get_color_ofNormal();
            if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                //normal:
                DrawBasics2D.LineFrom(impactPos_V3, normal_V3, color_ofNormal, 0.0f, null, DrawBasics.LineStyle.solid, hitInfo2D.transform.position.z, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                //normal socket:
                DrawShapes.Square(impactPos_V3, 0.12f, color_ofNormal, normal_V3, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                //normal:
                float relConeLength_ofNormalVector = 0.17f;
                string normalText = (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) ? null : (saveDrawnLines ? "normal" : "normal<br><size=4>of hit surface</size>");
                UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                DrawBasics2D.VectorFrom(impactPos_V3, normal_V3, color_ofNormal, saveDrawnLines ? 0.0f : 0.006f, normalText, relConeLength_ofNormalVector, false, hitInfo2D.transform.position.z, false, 0.0f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();

                //normal socket:
                DrawShapes.Decagon(impactPos_V3, 0.03f, color_ofNormal, normal_V3, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
            }
        }

        static Color Get_color_ofNormal()
        {
            if (UtilitiesDXXL_Colors.IsDefaultColor(DrawPhysics2D.overwriteColorForCastsHitNormals))
            {
                return Get_defaultColor_ofNormal();
            }
            else
            {
                return DrawPhysics2D.overwriteColorForCastsHitNormals;
            }
        }

        public static Color Get_defaultColor_ofNormal()
        {
            return ((DrawPhysics2D.colorForHittingCasts.grayscale < 0.175f) ? Color.Lerp(DrawPhysics2D.colorForHittingCasts, Color.white, 0.7f) : Color.Lerp(DrawPhysics2D.colorForHittingCasts, Color.black, 0.7f));
        }

        static void DrawDashedLineAlongZ(Vector3 impactPos_V3, RaycastHit2D hitInfo2D, float durationInSec, bool hiddenByNearerObjects)
        {
            float absZDistance = Mathf.Abs(hitInfo2D.transform.position.z - GetZPosForDrawVisualisation());
            Color color_forDashedLineAlongZ = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawPhysics2D.colorForHittingCasts, 0.6f);
            Vector3 impactPos_projectedOntoDrawVisualisation_V3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(hitInfo2D.point, GetZPosForDrawVisualisation());
            DrawBasics.LineStyle lineStyleAlongZ = (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes) ? DrawBasics.LineStyle.solid : DrawBasics.LineStyle.dashedLong;
            Line_fadeableAnimSpeed.InternalDraw(impactPos_V3, impactPos_projectedOntoDrawVisualisation_V3, color_forDashedLineAlongZ, 0.0f, null, lineStyleAlongZ, absZDistance, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
        }

        static string GetTextAtHitPos_forVolumeCast(string volumeCastTypeSpecifyingStringPart, bool saveDrawnLines, RaycastHit2D hitInfo2D, int i_hit, string nameText)
        {
            if (DrawPhysics2D.drawCastNameTag_atHitPositions && nameText != null && nameText.Length != 0)
            {
                return (saveDrawnLines ? (nameText + " / #" + i_hit + ":<br>hit GO: " + hitInfo2D.transform.gameObject.name + "<br>pos = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.point) + "<br>dist = " + hitInfo2D.distance) : (UtilitiesDXXL_Physics.GetStrokeWidthMarkupStartStringForHitPosDesctiptionHeaders(nameText) + nameText + " / hit #" + i_hit + ":</sw><br>GameObject that was hit: " + hitInfo2D.transform.gameObject.name + "<br>hit pos = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.point) + "<br>distance = " + hitInfo2D.distance));
            }
            else
            {
                return (saveDrawnLines ? ("hit #" + i_hit + ":<br>hit GO: " + hitInfo2D.transform.gameObject.name + "<br>pos = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.point) + "<br>dist = " + hitInfo2D.distance) : (volumeCastTypeSpecifyingStringPart + i_hit + ":</sw><br>GameObject that was hit: " + hitInfo2D.transform.gameObject.name + "<br>hit pos = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo2D.point) + "<br>distance = " + hitInfo2D.distance));
            }
        }

        static void DrawTextDescriptionAtVolumeCastHitPos(Vector3 impactPos_V3, RaycastHit2D hitInfo2D, string nameText, string text, float durationInSec, bool hiddenByNearerObjects, string additionalWarningText)
        {
            if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes) { return; }
            if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes)
            {
                if (DrawPhysics2D.drawCastNameTag_atHitPositions && nameText != null && nameText.Length != 0)
                {
                    UtilitiesDXXL_Text.Write2DFramed(nameText + additionalWarningText, impactPos_V3, DrawPhysics2D.colorForCastsHitText, 0.1f * DrawPhysics2D.scaleFactor_forCastHitTextSize, 0.0f, DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, hitInfo2D.transform.position.z, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
                }
            }
            else
            {
                Vector2 textOffsetDir = default(Vector2);
                float textOffsetDistance = DrawPhysics2D.scaleFactor_forCastHitTextSize;

                TrySet_default_textOffsetDirection_forPointTags_reversible();
                DrawBasics2D.PointTag(impactPos_V3, text + additionalWarningText, DrawPhysics2D.colorForCastsHitText, 0.0f, textOffsetDistance, textOffsetDir, hitInfo2D.transform.position.z, 1.0f, false, durationInSec, hiddenByNearerObjects);
                TryReverse_default_textOffsetDirection_forPointTags();
            }
        }

        static float GetDistanceOfSingleHit(bool hasHit, RaycastHit2D hitInfo2D)
        {
            if (hasHit)
            {
                if (UtilitiesDXXL_Math.FloatIsInvalid(hitInfo2D.distance))
                {
                    Debug.LogError("Draw XXL: A 'Physics2D.Cast()' returned an invalid hit distance of '" + hitInfo2D.distance + "'. The drawn cast visualization may be incorrect.");
                    return 1.0f;
                }
                else
                {
                    return hitInfo2D.distance;
                }
            }
            else
            {
                return 0.0f;
            }
        }

        static float GetDistanceOfFarestHit(RaycastHit2D[] hitInfos2D, int numberOfUsedSlotsInHitInfoArray)
        {
            float farestDistance = 0.0f;
            if (hitInfos2D != null)
            {
                numberOfUsedSlotsInHitInfoArray = Mathf.Min(numberOfUsedSlotsInHitInfoArray, hitInfos2D.Length);
                for (int i = 0; i < numberOfUsedSlotsInHitInfoArray; i++)
                {
                    //"RaycastHit2D.distance" is always positive, also when the castDistance is negative.
                    if (UtilitiesDXXL_Math.FloatIsInvalid(hitInfos2D[i].distance))
                    {
                        Debug.LogError("Draw XXL: A 'Physics2D.Cast()' returned an invalid hit distance of '" + hitInfos2D[i].distance + "'. The drawn cast visualization may be incorrect.");
                    }
                    else
                    {
                        farestDistance = Mathf.Max(farestDistance, hitInfos2D[i].distance);
                    }
                }
            }
            return farestDistance;
        }

        public static void DrawBoxOverlapResultOfMax1Collision(bool calledAsArea_insteadOfBox, Vector2 pos_V2, Vector2 size_V2, float angleDegCC, Collider2D overlappingCollider, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            DrawOverlapResultBoxOfMax1Collision(calledAsArea_insteadOfBox, overlappingCollider, pos_V2, size_V2, angleDegCC, nameTag, durationInSec, hiddenByNearerObjects);
            bool shapeIsShrinkedToPoint = UtilitiesDXXL_Math.ApproximatelyZero(size_V2);
            float approxSize_ofOverlapVolume = UtilitiesDXXL_Math.GetAverageBoxExtent(size_V2);
            DrawMarkersAtOverlappingCollidersOfMax1Collision(shapeIsShrinkedToPoint, pos_V2, overlappingCollider, approxSize_ofOverlapVolume, durationInSec, hiddenByNearerObjects);
        }

        public static void DrawBoxOverlapResultOfPotMultipleCollisions(bool calledAsArea_insteadOfBox, Vector2 pos_V2, Vector2 size_V2, float angleDegCC, int numberOfOverlappingColliders, Collider2D[] overlappingColliders, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            bool doesOverlap = numberOfOverlappingColliders > 0;
            DrawOverlapResultBoxOfPotMultipleCollisions(calledAsArea_insteadOfBox, doesOverlap, numberOfOverlappingColliders, overlappingColliders, pos_V2, size_V2, angleDegCC, nameTag, durationInSec, hiddenByNearerObjects);
            bool shapeIsShrinkedToPoint = UtilitiesDXXL_Math.ApproximatelyZero(size_V2);
            float approxSize_ofOverlapVolume = UtilitiesDXXL_Math.GetAverageBoxExtent(size_V2);
            DrawMarkersAtOverlappingCollidersOfPotMultipleCollisions(shapeIsShrinkedToPoint, pos_V2, doesOverlap, overlappingColliders, numberOfOverlappingColliders, approxSize_ofOverlapVolume, durationInSec, hiddenByNearerObjects);
        }

        public static void DrawCapsuleOverlapResultOfMax1Collision(Vector2 pos_V2, Vector2 size_V2, CapsuleDirection2D capsuleDirection, float angleDegCC, Collider2D overlappingCollider, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            DrawOverlapResultCapsuleOfMax1Collision(overlappingCollider, pos_V2, size_V2, capsuleDirection, angleDegCC, nameTag, durationInSec, hiddenByNearerObjects);
            bool shapeIsShrinkedToPoint = UtilitiesDXXL_Math.ApproximatelyZero(size_V2);
            float approxSize_ofOverlapVolume = UtilitiesDXXL_Math.GetAverageBoxExtent(size_V2);
            DrawMarkersAtOverlappingCollidersOfMax1Collision(shapeIsShrinkedToPoint, pos_V2, overlappingCollider, approxSize_ofOverlapVolume, durationInSec, hiddenByNearerObjects);
        }

        public static void DrawCapsuleOverlapResultOfPotMultipleCollisions(Vector2 pos_V2, Vector2 size_V2, CapsuleDirection2D capsuleDirection, float angleDegCC, int numberOfOverlappingColliders, Collider2D[] overlappingColliders, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            bool doesOverlap = numberOfOverlappingColliders > 0;
            DrawOverlapResultCapsuleOfPotMultipleCollisions(doesOverlap, numberOfOverlappingColliders, overlappingColliders, pos_V2, size_V2, capsuleDirection, angleDegCC, nameTag, durationInSec, hiddenByNearerObjects);
            bool shapeIsShrinkedToPoint = UtilitiesDXXL_Math.ApproximatelyZero(size_V2);
            float approxSize_ofOverlapVolume = UtilitiesDXXL_Math.GetAverageBoxExtent(size_V2);
            DrawMarkersAtOverlappingCollidersOfPotMultipleCollisions(shapeIsShrinkedToPoint, pos_V2, doesOverlap, overlappingColliders, numberOfOverlappingColliders, approxSize_ofOverlapVolume, durationInSec, hiddenByNearerObjects);
        }

        public static void DrawCircleOverlapResultOfMax1Collision(Vector2 pos_V2, float radius, Collider2D overlappingCollider, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            DrawOverlapResultCircleOfMax1Collision(overlappingCollider, pos_V2, radius, nameTag, durationInSec, hiddenByNearerObjects);
            bool shapeIsShrinkedToPoint = UtilitiesDXXL_Math.ApproximatelyZero(radius);
            float approxSize_ofOverlapVolume = 2.0f * radius;
            DrawMarkersAtOverlappingCollidersOfMax1Collision(shapeIsShrinkedToPoint, pos_V2, overlappingCollider, approxSize_ofOverlapVolume, durationInSec, hiddenByNearerObjects);
        }

        public static void DrawCircleOverlapResultOfPotMultipleCollisions(Vector2 pos_V2, float radius, int numberOfOverlappingColliders, Collider2D[] overlappingColliders, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            bool doesOverlap = numberOfOverlappingColliders > 0;
            DrawOverlapResultCircleOfPotMultipleCollisions(doesOverlap, numberOfOverlappingColliders, overlappingColliders, pos_V2, radius, nameTag, durationInSec, hiddenByNearerObjects);
            bool shapeIsShrinkedToPoint = UtilitiesDXXL_Math.ApproximatelyZero(radius);
            float approxSize_ofOverlapVolume = 2.0f * radius;
            DrawMarkersAtOverlappingCollidersOfPotMultipleCollisions(shapeIsShrinkedToPoint, pos_V2, doesOverlap, overlappingColliders, numberOfOverlappingColliders, approxSize_ofOverlapVolume, durationInSec, hiddenByNearerObjects);
        }

        public static void DrawPointOverlapResultOfMax1Collision(Vector2 pos_V2, Collider2D overlappingCollider, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            DrawOverlapResultPointOfMax1Collision(overlappingCollider, pos_V2, nameTag, durationInSec, hiddenByNearerObjects);
            bool shapeIsShrinkedToPoint = true;
            float approxSize_ofOverlapVolume = 0.5f;
            DrawMarkersAtOverlappingCollidersOfMax1Collision(shapeIsShrinkedToPoint, pos_V2, overlappingCollider, approxSize_ofOverlapVolume, durationInSec, hiddenByNearerObjects);
        }

        public static void DrawPointOverlapResultOfPotMultipleCollisions(Vector2 pos_V2, int numberOfOverlappingColliders, Collider2D[] overlappingColliders, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            bool doesOverlap = numberOfOverlappingColliders > 0;
            DrawOverlapResultPointOfPotMultipleCollisions(doesOverlap, numberOfOverlappingColliders, overlappingColliders, pos_V2, nameTag, durationInSec, hiddenByNearerObjects);
            bool shapeIsShrinkedToPoint = true;
            float approxSize_ofOverlapVolume = 0.5f;
            DrawMarkersAtOverlappingCollidersOfPotMultipleCollisions(shapeIsShrinkedToPoint, pos_V2, doesOverlap, overlappingColliders, numberOfOverlappingColliders, approxSize_ofOverlapVolume, durationInSec, hiddenByNearerObjects);
        }

        static void DrawOverlapResultBoxOfMax1Collision(bool calledAsArea_insteadOfBox, Collider2D overlappingCollider, Vector2 pos_V2, Vector2 size_V2, float angleDegCC, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            bool doesOverlap = overlappingCollider != null;
            string text = null;
            if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                string additinalWarningText = null;
                if (calledAsArea_insteadOfBox == false)
                {
                    bool shapeContainsNegativeDimensions = UtilitiesDXXL_Math.ContainsNegativeComponents(size_V2);
                    additinalWarningText = shapeContainsNegativeDimensions ? "[<color=#e2aa00FF><icon=warning></color> box contains negative dimensions -> potentially collisions missing]<br>" : null;
                }
                text = additinalWarningText + GetTextForVolumeOverlapCheckOfMax1Collision(calledAsArea_insteadOfBox ? "Area2D: Overlap check (max 1)" : "Box2D: Overlap check (max 1)", doesOverlap, nameTag, overlappingCollider);
            }
            DrawOverlapBox(doesOverlap, pos_V2, size_V2, angleDegCC, text, durationInSec, hiddenByNearerObjects);
        }

        static void DrawOverlapResultCapsuleOfMax1Collision(Collider2D overlappingCollider, Vector2 pos_V2, Vector2 size_V2, CapsuleDirection2D capsuleDirection, float angleDegCC, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            bool doesOverlap = overlappingCollider != null;
            string text = null;
            if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                bool shapeContainsNegativeDimensions = UtilitiesDXXL_Math.ContainsNegativeComponents(size_V2);
                string additinalWarningText = shapeContainsNegativeDimensions ? "[<color=#e2aa00FF><icon=warning></color> capsule contains negative dimensions -> potentially collisions missing]<br>" : null;
                text = additinalWarningText + GetTextForVolumeOverlapCheckOfMax1Collision("Capsule2D: Overlap check (max 1)", doesOverlap, nameTag, overlappingCollider);
            }
            DrawOverlapCapsule(doesOverlap, pos_V2, size_V2, capsuleDirection, angleDegCC, text, durationInSec, hiddenByNearerObjects);
        }

        static void DrawOverlapResultCircleOfMax1Collision(Collider2D overlappingCollider, Vector2 pos_V2, float radius, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            bool doesOverlap = overlappingCollider != null;
            string text = null;
            if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                string additinalWarningText = (radius < 0.0f) ? "[<color=#e2aa00FF><icon=warning></color> negative circle radius -> collision test only with circle center and/or potentially collisions missing]<br>" : null;
                text = additinalWarningText + GetTextForVolumeOverlapCheckOfMax1Collision("Circle2D: Overlap check (max 1)", doesOverlap, nameTag, overlappingCollider);
            }
            DrawOverlapCircle(doesOverlap, pos_V2, radius, text, durationInSec, hiddenByNearerObjects);
        }

        static void DrawOverlapResultPointOfMax1Collision(Collider2D overlappingCollider, Vector2 pos_V2, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            bool doesOverlap = overlappingCollider != null;
            string text = null;
            if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                text = GetTextForVolumeOverlapCheckOfMax1Collision("Point2D: Overlap check (max 1)", doesOverlap, nameTag, overlappingCollider);
            }
            DrawOverlapPoint(doesOverlap, pos_V2, text, durationInSec, hiddenByNearerObjects);
        }

        static void DrawOverlapResultBoxOfPotMultipleCollisions(bool calledAsArea_insteadOfBox, bool doesOverlap, int numberOfOverlappingColliders, Collider2D[] overlappingColliders, Vector2 pos_V2, Vector2 size_V2, float angleDegCC, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            string text = null;
            if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                string additinalWarningText = null;
                if (calledAsArea_insteadOfBox == false)
                {
                    bool shapeContainsNegativeDimensions = UtilitiesDXXL_Math.ContainsNegativeComponents(size_V2);
                    additinalWarningText = shapeContainsNegativeDimensions ? "[<color=#e2aa00FF><icon=warning></color> box contains negative dimensions -> potentially collisions missing]<br>" : null;
                }
                string overlappingCollidersListAsText = GetOverlappingCollidersListAsText(overlappingColliders, numberOfOverlappingColliders);
                text = additinalWarningText + GetTextForVolumeOverlapCheckOfPotMultipleCollisions(calledAsArea_insteadOfBox ? "Area2D: Overlap check" : "Box2D: Overlap check", doesOverlap, numberOfOverlappingColliders, nameTag, overlappingCollidersListAsText);
            }
            DrawOverlapBox(doesOverlap, pos_V2, size_V2, angleDegCC, text, durationInSec, hiddenByNearerObjects);
        }

        static void DrawOverlapResultCapsuleOfPotMultipleCollisions(bool doesOverlap, int numberOfOverlappingColliders, Collider2D[] overlappingColliders, Vector2 pos_V2, Vector2 size_V2, CapsuleDirection2D capsuleDirection, float angleDegCC, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            string text = null;
            if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                bool shapeContainsNegativeDimensions = UtilitiesDXXL_Math.ContainsNegativeComponents(size_V2);
                string additinalWarningText = shapeContainsNegativeDimensions ? "[<color=#e2aa00FF><icon=warning></color> capsule contains negative dimensions -> potentially collisions missing]<br>" : null;
                string overlappingCollidersListAsText = GetOverlappingCollidersListAsText(overlappingColliders, numberOfOverlappingColliders);
                text = additinalWarningText + GetTextForVolumeOverlapCheckOfPotMultipleCollisions("Capsule2D: Overlap check", doesOverlap, numberOfOverlappingColliders, nameTag, overlappingCollidersListAsText);
            }
            DrawOverlapCapsule(doesOverlap, pos_V2, size_V2, capsuleDirection, angleDegCC, text, durationInSec, hiddenByNearerObjects);
        }

        static void DrawOverlapResultCircleOfPotMultipleCollisions(bool doesOverlap, int numberOfOverlappingColliders, Collider2D[] overlappingColliders, Vector2 pos_V2, float radius, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            string text = null;
            if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                string additinalWarningText = (radius < 0.0f) ? "[<color=#e2aa00FF><icon=warning></color> negative circle radius -> collision test only with circle center and/or potentially collisions missing]<br>" : null;
                string overlappingCollidersListAsText = GetOverlappingCollidersListAsText(overlappingColliders, numberOfOverlappingColliders);
                text = additinalWarningText + GetTextForVolumeOverlapCheckOfPotMultipleCollisions("Circle2D: Overlap check", doesOverlap, numberOfOverlappingColliders, nameTag, overlappingCollidersListAsText);
            }
            DrawOverlapCircle(doesOverlap, pos_V2, radius, text, durationInSec, hiddenByNearerObjects);
        }

        static void DrawOverlapResultPointOfPotMultipleCollisions(bool doesOverlap, int numberOfOverlappingColliders, Collider2D[] overlappingColliders, Vector2 pos_V2, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            string text = null;
            if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                string overlappingCollidersListAsText = GetOverlappingCollidersListAsText(overlappingColliders, numberOfOverlappingColliders);
                text = GetTextForVolumeOverlapCheckOfPotMultipleCollisions("Point2D: Overlap check", doesOverlap, numberOfOverlappingColliders, nameTag, overlappingCollidersListAsText);
            }

            UtilitiesDXXL_DrawBasics.Set_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM_reversible(0);
            DrawOverlapPoint(doesOverlap, pos_V2, text, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_DrawBasics.Reverse_strokeWidth_forCoordinateTexts_onPointVisualiation_inPPM();
        }

        static string GetOverlappingCollidersListAsText(Collider2D[] overlappingColliders, int numberOfOverlappingColliders)
        {
            if (overlappingColliders == null)
            {
                return null;
            }
            else
            {
                if (overlappingColliders.Length == 0)
                {
                    return null;
                }
                else
                {
                    if (numberOfOverlappingColliders <= 0)
                    {
                        return null;
                    }
                    else
                    {
                        string collidersList = null;
                        for (int i = 0; i < numberOfOverlappingColliders; i++)
                        {
                            if (i < DrawPhysics2D.MaxListedColliders_inOverlapVolumesTextList)
                            {
                                //collidersList = overlappingColliders[i].GetType().ToString() + " (on GameObject '" + overlappingColliders[i].gameObject.name + "')<br>" + collidersList; //-> first found collider is on bottom. This contradicts the "DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes"-hitPos-display, which only displays the index-number
                                collidersList = collidersList + "<br>" + overlappingColliders[i].GetType().ToString() + " (on GameObject '" + overlappingColliders[i].gameObject.name + "')"; //-> first found collider is on top. This corresponds to the "DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes"-hitPos-display, which only, which only displays the index-number
                            }
                            else
                            {
                                collidersList = collidersList + "<br>...and " + (numberOfOverlappingColliders - DrawPhysics2D.MaxListedColliders_inOverlapVolumesTextList) + " more.";
                                break;
                            }
                        }
                        return collidersList;
                    }
                }
            }
        }

        static string GetTextForVolumeOverlapCheckOfMax1Collision(string volumeTypeIdentifyingStringPart, bool doesOverlap, string nameTag, Collider2D overlappingCollider)
        {
            if (nameTag != null && nameTag.Length != 0)
            {
                //has user specified "nameTag":
                if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes)
                {
                    if (doesOverlap)
                    {
                        return (nameTag + "<br>Overlapping at least with:<br>" + overlappingCollider.GetType().ToString() + " (on GameObject '" + overlappingCollider.gameObject.name + "')");
                    }
                    else
                    {
                        return nameTag;
                    }
                }
                else
                {
                    if (doesOverlap)
                    {
                        return (nameTag + "<br>Overlapping at least with this collider:<br>" + overlappingCollider.GetType().ToString() + " (on GameObject '" + overlappingCollider.gameObject.name + "')");
                    }
                    else
                    {
                        return (nameTag + "<br>Result: Is not overlapping with any collider");
                    }
                }
            }
            else
            {
                //has NO user specified "nameTag":
                if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes)
                {
                    if (doesOverlap)
                    {
                        return ("Overlapping at least with:<br>" + overlappingCollider.GetType().ToString() + " (on GameObject '" + overlappingCollider.gameObject.name + "')");
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (doesOverlap)
                    {
                        return (volumeTypeIdentifyingStringPart + "<br>Overlapping at least with this collider:<br>" + overlappingCollider.GetType().ToString() + " (on GameObject '" + overlappingCollider.gameObject.name + "')");
                    }
                    else
                    {
                        return (volumeTypeIdentifyingStringPart + "<br>Result: Is not overlapping with any collider");
                    }
                }
            }
        }

        static string GetTextForVolumeOverlapCheckOfPotMultipleCollisions(string volumeTypeIdentifyingStringPart, bool doesOverlap, int numberOfOverlappingColliders, string nameTag, string overlappingCollidersListAsText)
        {
            if (nameTag != null && nameTag.Length != 0)
            {
                //has user specified "nameTag":
                if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes)
                {
                    if (doesOverlap)
                    {
                        return (nameTag + "<br>Overlapping with these " + numberOfOverlappingColliders + ":<br>" + overlappingCollidersListAsText);
                    }
                    else
                    {
                        return nameTag;
                    }
                }
                else
                {
                    if (doesOverlap)
                    {
                        return (nameTag + "<br>Overlapping with these " + numberOfOverlappingColliders + " collider(s):<br>" + overlappingCollidersListAsText);
                    }
                    else
                    {
                        return (nameTag + "<br>Result: Is not overlapping with any collider");
                    }
                }
            }
            else
            {
                //has NO user specified "nameTag":
                if (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes)
                {
                    if (doesOverlap)
                    {
                        return ("Overlapping with these " + numberOfOverlappingColliders + ":<br>" + overlappingCollidersListAsText);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (doesOverlap)
                    {
                        return (volumeTypeIdentifyingStringPart + "<br>Overlapping with these " + numberOfOverlappingColliders + " collider(s):<br>" + overlappingCollidersListAsText);
                    }
                    else
                    {
                        return (volumeTypeIdentifyingStringPart + "<br>Result: Is not overlapping with any collider");
                    }
                }
            }
        }

        static void DrawOverlapBox(bool doesOverlap, Vector2 pos_V2, Vector2 size_V2, float angleDegCC, string text, float durationInSec, bool hiddenByNearerObjects)
        {
            Color color = doesOverlap ? DrawPhysics2D.colorForHittingCasts : DrawPhysics2D.colorForNonHittingCasts;

            UtilitiesDXXL_Shapes.Set_bothForcedConstantTextSizes_forTextAtShapes_reversible(DrawPhysics2D.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts, DrawPhysics2D.forcedConstantWorldspaceTextSize_forOverlapResultTexts);
            DrawShapes.Box2D(pos_V2, size_V2, color, GetZPosForDrawVisualisation(), angleDegCC, DrawShapes.Shape2DType.square, 0.0f, text, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, true, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_Shapes.Reverse_disable_bothForcedConstantTextSizes_forTextAtShapes();
        }

        static void DrawOverlapCapsule(bool doesOverlap, Vector2 pos_V2, Vector2 size_V2, CapsuleDirection2D capsuleDirection, float angleDegCC, string text, float durationInSec, bool hiddenByNearerObjects)
        {
            Color color = doesOverlap ? DrawPhysics2D.colorForHittingCasts : DrawPhysics2D.colorForNonHittingCasts;

            UtilitiesDXXL_Shapes.Set_bothForcedConstantTextSizes_forTextAtShapes_reversible(DrawPhysics2D.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts, DrawPhysics2D.forcedConstantWorldspaceTextSize_forOverlapResultTexts);
            DrawShapes.Capsule2D(pos_V2, size_V2, color, GetZPosForDrawVisualisation(), capsuleDirection, angleDegCC, 0.0f, text, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, true, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_Shapes.Reverse_disable_bothForcedConstantTextSizes_forTextAtShapes();
        }

        static void DrawOverlapCircle(bool doesOverlap, Vector2 pos_V2, float radius, string text, float durationInSec, bool hiddenByNearerObjects)
        {
            Color color = doesOverlap ? DrawPhysics2D.colorForHittingCasts : DrawPhysics2D.colorForNonHittingCasts;

            UtilitiesDXXL_Shapes.Set_bothForcedConstantTextSizes_forTextAtShapes_reversible(DrawPhysics2D.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts, DrawPhysics2D.forcedConstantWorldspaceTextSize_forOverlapResultTexts);
            DrawShapes.Circle2D(pos_V2, radius, color, GetZPosForDrawVisualisation(), 0.0f, text, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, true, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_Shapes.Reverse_disable_bothForcedConstantTextSizes_forTextAtShapes();
        }

        static void DrawOverlapPoint(bool doesOverlap, Vector2 pos_V2, string text, float durationInSec, bool hiddenByNearerObjects)
        {
            Color color = doesOverlap ? DrawPhysics2D.colorForHittingCasts : DrawPhysics2D.colorForNonHittingCasts;
            bool drawCoordsAsText = (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes);
            DrawBasics2D.Point(pos_V2, text, color, 0.5f, 0.0f, GetZPosForDrawVisualisation(), color, 0.0f, true, drawCoordsAsText, durationInSec, hiddenByNearerObjects);
        }

        static void DrawMarkersAtOverlappingCollidersOfMax1Collision(bool shapeIsShrinkedToPoint, Vector2 posOfCheckingShape_V2, Collider2D overlappingCollider, float approxSize_ofOverlapVolume, float durationInSec, bool hiddenByNearerObjects)
        {
            bool doesOverlap = overlappingCollider != null;
            if (doesOverlap)
            {
                Color color_ofMarkers = (DrawPhysics2D.colorForHittingCasts.grayscale < 0.175f) ? Color.Lerp(DrawPhysics2D.colorForHittingCasts, Color.white, 0.7f) : Color.Lerp(DrawPhysics2D.colorForHittingCasts, Color.black, 0.7f);
                Color color_ofMarkerExtentionLineToShapeCenter = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawPhysics2D.colorForHittingCasts, 0.2f);
                Color color_forDashedLineAlongZ = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawPhysics2D.colorForHittingCasts, 0.6f);
                int i_collisionOfPointShrinkedShape = shapeIsShrinkedToPoint ? 0 : (-1);
                string shorterFallbackNameText = (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) ? "hit" : null;
                DrawMarkersAtOverlappingColliders(i_collisionOfPointShrinkedShape, posOfCheckingShape_V2, overlappingCollider, color_ofMarkers, color_ofMarkerExtentionLineToShapeCenter, color_forDashedLineAlongZ, shorterFallbackNameText, approxSize_ofOverlapVolume, durationInSec, hiddenByNearerObjects);
            }
        }

        static void DrawMarkersAtOverlappingCollidersOfPotMultipleCollisions(bool shapeIsShrinkedToPoint, Vector2 posOfCheckingShape_V2, bool doesOverlap, Collider2D[] overlappingColliders, int numberOfOverlappingColliders, float approxSize_ofOverlapVolume, float durationInSec, bool hiddenByNearerObjects)
        {
            if (doesOverlap)
            {
                Color color_ofMarkers = (DrawPhysics2D.colorForHittingCasts.grayscale < 0.175f) ? Color.Lerp(DrawPhysics2D.colorForHittingCasts, Color.white, 0.7f) : Color.Lerp(DrawPhysics2D.colorForHittingCasts, Color.black, 0.7f);
                Color color_ofMarkerExtentionLineToShapeCenter = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawPhysics2D.colorForHittingCasts, 0.1f);
                Color color_forDashedLineAlongZ = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawPhysics2D.colorForHittingCasts, 0.6f);
                numberOfOverlappingColliders = Mathf.Min(numberOfOverlappingColliders, overlappingColliders.Length);
                for (int i = 0; i < numberOfOverlappingColliders; i++)
                {
                    string shorterFallbackNameText = null;
                    if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
                    {
                        if ((DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) || (i >= DrawPhysics2D.maxOverlapingCollidersWithUntruncatedText))
                        {
                            shorterFallbackNameText = "" + i;
                        }
                    }

                    int i_collisionOfPointShrinkedShape = shapeIsShrinkedToPoint ? i : (-1);
                    DrawMarkersAtOverlappingColliders(i_collisionOfPointShrinkedShape, posOfCheckingShape_V2, overlappingColliders[i], color_ofMarkers, color_ofMarkerExtentionLineToShapeCenter, color_forDashedLineAlongZ, shorterFallbackNameText, approxSize_ofOverlapVolume, durationInSec, hiddenByNearerObjects);
                }
            }
        }

        static void DrawMarkersAtOverlappingColliders(int i_collisionOfPointShrinkedShape, Vector2 posOfCheckingShape_V2, Collider2D overlappingCollider, Color color_ofMarkers, Color color_ofMarkerExtentionLineToShapeCenter, Color color_forDashedLineAlongZ, string shorterFallbackNameText, float approxSize_ofOverlapVolume, float durationInSec, bool hiddenByNearerObjects)
        {
            //thin line from shape center:
            Vector2 nearestPosOnCollider_V2 = overlappingCollider.ClosestPoint(posOfCheckingShape_V2);
            Line_fadeableAnimSpeed_2D.InternalDraw(posOfCheckingShape_V2, nearestPosOnCollider_V2, color_ofMarkerExtentionLineToShapeCenter, 0.0f, null, DrawBasics.LineStyle.solid, GetZPosForDrawVisualisation(), 1.0f, 0.0f, null, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            //dashed line along z to collider:
            float absZDistance = Mathf.Abs(overlappingCollider.transform.position.z - GetZPosForDrawVisualisation());
            Vector3 nearestPosOnCollider_V3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(nearestPosOnCollider_V2, overlappingCollider.transform.position.z);
            Vector3 nearestPosOnCollider_insideDrawXYplane_V3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(nearestPosOnCollider_V2, GetZPosForDrawVisualisation());
            DrawBasics.LineStyle lineStyleAlongZ = (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes) ? DrawBasics.LineStyle.solid : DrawBasics.LineStyle.dashedLong;
            Line_fadeableAnimSpeed.InternalDraw(nearestPosOnCollider_V3, nearestPosOnCollider_insideDrawXYplane_V3, color_forDashedLineAlongZ, 0.0f, null, lineStyleAlongZ, absZDistance, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            //collider marking:
            float sizeOfMarkingCross = Mathf.Max(0.1f * approxSize_ofOverlapVolume, 0.001f);
            bool drawCoordsAsText = (DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.high_withFullDetails);
            DrawBasics2D.Point(nearestPosOnCollider_V2, DrawPhysics2D.colorForHittingCasts, sizeOfMarkingCross, 0.0f, 0.0f, null, DrawPhysics2D.colorForHittingCasts, overlappingCollider.transform.position.z, false, drawCoordsAsText, durationInSec, hiddenByNearerObjects);

            if (DrawPhysics2D.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                string text = (shorterFallbackNameText != null) ? shorterFallbackNameText : (overlappingCollider.GetType().ToString() + " (on GameObject '" + overlappingCollider.gameObject.name + "')");
                float textOffsetDistance;
                Vector2 textOffsetDir_V2;
                float relTextSizeScaling;
                if (i_collisionOfPointShrinkedShape < 0)
                {
                    textOffsetDir_V2 = nearestPosOnCollider_V2 - posOfCheckingShape_V2;
                    textOffsetDistance = 1.0f;
                    relTextSizeScaling = 1.0f;
                }
                else
                {
                    Quaternion rotation = Quaternion.AngleAxis(-75.0f - 2.22f * i_collisionOfPointShrinkedShape, Vector3.forward);
                    textOffsetDir_V2 = rotation * Vector3.right;
                    textOffsetDistance = 1.0f + 0.2f + i_collisionOfPointShrinkedShape;
                    relTextSizeScaling = 1.0f / textOffsetDistance;
                }
                DrawBasics2D.PointTag(nearestPosOnCollider_V2, text, color_ofMarkers, 0.0f, textOffsetDistance, textOffsetDir_V2, overlappingCollider.transform.position.z, relTextSizeScaling, true, durationInSec, hiddenByNearerObjects);
            }
        }

        static float GetZPosForDrawVisualisation()
        {
            if (float.IsNaN(DrawPhysics2D.custom_zPos_forCastVisualisation) || float.IsInfinity(DrawPhysics2D.custom_zPos_forCastVisualisation))
            {
                return DrawBasics2D.Default_zPos_forDrawing;

            }
            else
            {
                return DrawPhysics2D.custom_zPos_forCastVisualisation;
            }
        }

        public static bool ExtentNameTagForNonSuitingResultArray(ref string nameTag, int numberOfOverlappingColliders, Collider2D[] resultsArray)
        {
            bool resultsArrayIsNull = (resultsArray == null);
            UtilitiesDXXL_Physics.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfOverlappingColliders, resultsArrayIsNull, resultsArrayIsNull ? 0 : resultsArray.Length);
            return resultsArrayIsNull;
        }

        public static bool ExtentNameTagForNonSuitingResultArray(ref string nameTag, int numberOfUsedSlotsInHitInfoArray, RaycastHit2D[] resultsArray)
        {
            bool resultsArrayIsNull = (resultsArray == null);
            UtilitiesDXXL_Physics.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoArray, resultsArrayIsNull, resultsArrayIsNull ? 0 : resultsArray.Length);
            return resultsArrayIsNull;
        }

        public static bool ExtentNameTagForNonSuitingResultList(ref string nameTag, int numberOfUsedSlotsInList, List<RaycastHit2D> resultsList)
        {
            bool resultsListIsNull = (resultsList == null);
            UtilitiesDXXL_Physics.ExtentNameTagForNonSuitingResultList(ref nameTag, resultsListIsNull, resultsListIsNull ? 0 : numberOfUsedSlotsInList);
            return resultsListIsNull;
        }

        public static bool ExtentNameTagForNonSuitingResultList(ref string nameTag, int numberOfUsedSlotsInList, List<Collider2D> resultsList)
        {
            bool resultsListIsNull = (resultsList == null);
            UtilitiesDXXL_Physics.ExtentNameTagForNonSuitingResultList(ref nameTag, resultsListIsNull, resultsListIsNull ? 0 : numberOfUsedSlotsInList);
            return resultsListIsNull;
        }

        public static int CopyHitResultsFromListToPreallocatedArray(out bool preallocatedArrayIsTooSmall, List<RaycastHit2D> resultsList, int numberOfUsedSlotsInResultsList)
        {
            if (resultsList != null)
            {
                preallocatedArrayIsTooSmall = numberOfUsedSlotsInResultsList > preallocatedRayHit2DResultsArray_copiedFromList.Length;
                int numberOfCopiedSlots = Mathf.Min(numberOfUsedSlotsInResultsList, preallocatedRayHit2DResultsArray_copiedFromList.Length);
                for (int i = 0; i < numberOfCopiedSlots; i++)
                {
                    preallocatedRayHit2DResultsArray_copiedFromList[i] = resultsList[i];
                }
                return numberOfCopiedSlots;
            }
            else
            {
                preallocatedArrayIsTooSmall = false;
                return 0;
            }
        }

        public static int CopyHitResultsFromListToPreallocatedArray(out bool preallocatedArrayIsTooSmall, List<Collider2D> colliderList, int numberOfUsedSlotsInResultsList)
        {
            if (colliderList != null)
            {
                preallocatedArrayIsTooSmall = numberOfUsedSlotsInResultsList > preallocatedCollider2DResultsArray_copiedFromList.Length;
                int numberOfCopiedSlots = Mathf.Min(numberOfUsedSlotsInResultsList, preallocatedCollider2DResultsArray_copiedFromList.Length);
                for (int i = 0; i < numberOfCopiedSlots; i++)
                {
                    preallocatedCollider2DResultsArray_copiedFromList[i] = colliderList[i];
                }
                return numberOfCopiedSlots;
            }
            else
            {
                preallocatedArrayIsTooSmall = false;
                return 0;
            }
        }
        
        public static void ExtentNameTagForPreallocatedArrayIsTooSmallForFilledInListResults(bool preallocatedArrayIsTooSmall, ref string nameTag, int numberOfUsedSlotsInResultsList)
        {
            //"ExtentNameTagForNonSuitingResultList" already cares for this
            return;
            //if (preallocatedArrayIsTooSmall)
            //{
            //    int numberOfNotDrawnCollisions = numberOfUsedSlotsInResultsList - DrawPhysics2D.MaxNumberOfPreallocatedHits;
            //    nameTag = "[<color=#e2aa00FF><icon=warning></color> DrawPhysics2D internal result buffer (of " + DrawPhysics2D.MaxNumberOfPreallocatedHits + ") is full<br>-> " + numberOfNotDrawnCollisions + " results are not drawn<br>-> Fix by increasing 'DrawPhysics2D.MaxNumberOfPreallocatedHits']<br>" + nameTag;
            //}
        }

        public static void Area2D_to_Box2D(out Vector2 boxCenterPos_V2, out Vector2 boxSize_V2, Vector2 areaPointA, Vector2 areaPointB)
        {
            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(areaPointA, areaPointB))
            {
                boxSize_V2 = Vector2.zero;
            }
            else
            {
                boxSize_V2 = UtilitiesDXXL_Math.Abs(areaPointB - areaPointA);
            }
            boxCenterPos_V2 = 0.5f * (areaPointA + areaPointB);
        }

        static float scaleFactor_forCastHitTextSize_before;
        public static void Set_scaleFactor_forCastHitTextSize_reversible(float new_scaleFactor_forCastHitTextSize)
        {
            scaleFactor_forCastHitTextSize_before = DrawPhysics2D.scaleFactor_forCastHitTextSize;
            DrawPhysics2D.scaleFactor_forCastHitTextSize = new_scaleFactor_forCastHitTextSize;
        }
        public static void Reverse_scaleFactor_forCastHitTextSize()
        {
            DrawPhysics2D.scaleFactor_forCastHitTextSize = scaleFactor_forCastHitTextSize_before;
        }

        static float castCorridorVisualizerDensity_before;
        public static void Set_castCorridorVisualizerDensity_reversible(float new_castCorridorVisualizerDensity)
        {
            castCorridorVisualizerDensity_before = DrawPhysics2D.castCorridorVisualizerDensity;
            DrawPhysics2D.castCorridorVisualizerDensity = new_castCorridorVisualizerDensity;
        }
        public static void Reverse_castCorridorVisualizerDensity()
        {
            DrawPhysics2D.castCorridorVisualizerDensity = castCorridorVisualizerDensity_before;
        }

        static float forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts_before;
        public static void Set_forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts_reversible(float new_forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts)
        {
            forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts_before = DrawPhysics2D.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts;
            DrawPhysics2D.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts = new_forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts;
        }
        public static void Reverse_forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts()
        {
            DrawPhysics2D.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts = forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts_before;
        }

        static float forcedConstantWorldspaceTextSize_forOverlapResultTexts_before;
        public static void Set_forcedConstantWorldspaceTextSize_forOverlapResultTexts_reversible(float new_forcedConstantWorldspaceTextSize_forOverlapResultTexts)
        {
            forcedConstantWorldspaceTextSize_forOverlapResultTexts_before = DrawPhysics2D.forcedConstantWorldspaceTextSize_forOverlapResultTexts;
            DrawPhysics2D.forcedConstantWorldspaceTextSize_forOverlapResultTexts = new_forcedConstantWorldspaceTextSize_forOverlapResultTexts;
        }
        public static void Reverse_forcedConstantWorldspaceTextSize_forOverlapResultTexts()
        {
            DrawPhysics2D.forcedConstantWorldspaceTextSize_forOverlapResultTexts = forcedConstantWorldspaceTextSize_forOverlapResultTexts_before;
        }

        static Vector2 directionOfHitResultText_before;
        public static void Set_directionOfHitResultText_reversible(Vector2 new_directionOfHitResultText)
        {
            directionOfHitResultText_before = DrawPhysics2D.directionOfHitResultText;
            DrawPhysics2D.directionOfHitResultText = new_directionOfHitResultText;
        }
        public static void Reverse_directionOfHitResultText()
        {
            DrawPhysics2D.directionOfHitResultText = directionOfHitResultText_before;
        }

        static Vector3 default_textOffsetDirection_forPointTags_before;
        public static void TrySet_default_textOffsetDirection_forPointTags_reversible()
        {
            if (UtilitiesDXXL_Math.IsDefaultVector(DrawPhysics2D.directionOfHitResultText) == false)
            {
                default_textOffsetDirection_forPointTags_before = DrawBasics.Default_textOffsetDirection_forPointTags;
                DrawBasics.Default_textOffsetDirection_forPointTags = new Vector3(DrawPhysics2D.directionOfHitResultText.x, DrawPhysics2D.directionOfHitResultText.y, 0.0f);
            }
        }
        public static void TryReverse_default_textOffsetDirection_forPointTags()
        {
            if (UtilitiesDXXL_Math.IsDefaultVector(DrawPhysics2D.directionOfHitResultText) == false)
            {
                DrawBasics.Default_textOffsetDirection_forPointTags = default_textOffsetDirection_forPointTags_before;
            }
        }

    }

}
