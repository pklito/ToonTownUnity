namespace DrawXXL
{
    using UnityEngine;

    public class UtilitiesDXXL_Physics
    {
        public static float minArrowLength = 0.001f;

        public static void DrawRaycastTillFirstHit(bool hasHit, Vector3 origin, Vector3 direction, float maxDistance, RaycastHit hitInfo, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(direction))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(origin, "[<color=#adadadFF><icon=logMessage></color> DrawRaycast with direction of zero]<br>" + nameText, DrawPhysics.colorForNonHittingCasts, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            float distanceOfFarestHit = GetDistanceOfSingleHit(hasHit, hitInfo);
            DrawRayOfRaycast(hasHit ? 1 : 0, origin, direction, maxDistance, nameText, distanceOfFarestHit, durationInSec, hiddenByNearerObjects);
            if (hasHit)
            {
                DrawRaycastHitInfo(hitInfo, 0, nameText, direction, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void DrawRaycastPotMultipleHits(Vector3 origin, Vector3 direction, float maxDistance, RaycastHit[] hitInfos, int numberOfUsedSlotsInHitInfoArray, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(direction))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(origin, "[<color=#adadadFF><icon=logMessage></color> DrawRaycast with direction of zero]<br>" + nameText, DrawPhysics.colorForNonHittingCasts, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            float distanceOfFarestHit = GetDistanceOfFarestHit(hitInfos, numberOfUsedSlotsInHitInfoArray);
            DrawRayOfRaycast(numberOfUsedSlotsInHitInfoArray, origin, direction, maxDistance, nameText, distanceOfFarestHit, durationInSec, hiddenByNearerObjects);
            for (int i = 0; i < numberOfUsedSlotsInHitInfoArray; i++)
            {
                DrawRaycastHitInfo(hitInfos[i], i, nameText, direction, durationInSec, hiddenByNearerObjects);
            }
        }

        static void DrawRayOfRaycast(int hitCount, Vector3 origin, Vector3 direction, float maxDistance, string nameText, float distanceOfFarestHit, float durationInSec, bool hiddenByNearerObjects)
        {
            bool hasHit = hitCount > 0;
            Color color = hasHit ? DrawPhysics.colorForHittingCasts : DrawPhysics.colorForNonHittingCasts;
            Vector3 direction_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(direction);
            bool hasUnlimitedLength = float.IsInfinity(maxDistance);
            float absMaxDistance = Mathf.Abs(maxDistance);
            float lengthOfRayDirIndicator = Mathf.Min(1.0f, 0.9f * absMaxDistance);
            maxDistance = Mathf.Min(maxDistance, 100000.0f);
            maxDistance = Mathf.Max(maxDistance, -100000.0f);
            Vector3 endPos = origin + direction_normalized * maxDistance;
            Line_fadeableAnimSpeed.InternalDraw(origin, endPos, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

            //at origin:
            if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                float width_ofBase = 0.1f * lengthOfRayDirIndicator;
                DrawShapes.Pyramid(origin, lengthOfRayDirIndicator, width_ofBase, width_ofBase, color, direction_normalized, Vector3.up, DrawShapes.Shape2DType.square, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                string rayText = GetRayText(nameText, hitCount, maxDistance);
                DrawEngineBasics.RayLineExtended(origin, direction_normalized * lengthOfRayDirIndicator, color, 0.0f, rayText, 0.0f, false, 0.01f, 0.0f, durationInSec, hiddenByNearerObjects);
            }

            //overdraw line after last hit:
            if (hasHit)
            {
                Line_fadeableAnimSpeed.InternalDraw(origin + direction_normalized * distanceOfFarestHit, endPos, DrawPhysics.colorForCastLineBeyondHit, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }

            //end cap:
            if (hasUnlimitedLength == false)
            {
                if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
                {
                    DrawShapes.Square(endPos, 0.025f, hasHit ? DrawPhysics.colorForCastLineBeyondHit : DrawPhysics.colorForNonHittingCasts, direction_normalized, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
                }
                else
                {
                    DrawShapes.Decagon(endPos, 0.025f, hasHit ? DrawPhysics.colorForCastLineBeyondHit : DrawPhysics.colorForNonHittingCasts, direction_normalized, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
                }
            }
        }

        static string GetRayText(string nameText, int hitCount, float maxDistance)
        {
            if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                return null;
            }
            else
            {
                string rayText;
                if (DrawPhysics.drawCastNameTag_atCastOrigin && nameText != null && nameText.Length != 0)
                {
                    rayText = (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) ? GetSizeMarkupStartStringForCastNames(nameText) + nameText + "</size><br><size=5> </size><br>hits: " + hitCount : GetSizeMarkupStartStringForCastNames(nameText) + nameText + "</size><br><size=5> </size><br>number of hits: " + hitCount;
                }
                else
                {
                    rayText = (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) ? "hits: " + hitCount : "<size=27>Raycast</size><br><size=5> </size><br>number of hits: " + hitCount;
                }

                if (maxDistance < 0.0f)
                {
                    rayText = "[<color=#e2aa00FF><icon=warning></color> negative ray direction<br><br>-> no hits will be detected]<br>" + rayText;
                }
                return rayText;
            }
        }

        static void DrawRaycastHitInfo(RaycastHit hitInfo, int i_hit, string nameText, Vector3 direction, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.VectorIsInvalid(hitInfo.point) || UtilitiesDXXL_Math.VectorIsInvalid(hitInfo.normal))
            {
                Debug.LogError("Draw XXL: A 'Physics.RayCast()' returned an invalid hit position (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo.point) + ") or normal (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo.normal) + "). The drawn cast visualization may be incorrect.");
            }
            else
            {
                bool saveDrawnLines = i_hit >= DrawPhysics.hitResultsWithMoreDetailedDisplay;
                if (DrawPhysics.visualizationQuality != DrawPhysics.VisualizationQuality.high_withFullDetails) { saveDrawnLines = true; }

                //Normal:
                Color color_ofNormal = Get_color_ofNormal();
                if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
                {
                    DrawBasics.LineFrom(hitInfo.point, hitInfo.normal, color_ofNormal, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                }
                else
                {
                    float relConeLength_ofNormalVector = 0.17f;
                    string normalText = (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) ? null : (saveDrawnLines ? "normal" : "normal<br><size=4>of hit surface</size>");

                    UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                    DrawBasics.VectorFrom(hitInfo.point, hitInfo.normal, color_ofNormal, saveDrawnLines ? 0.0f : 0.006f, normalText, relConeLength_ofNormalVector, false, false, default(Vector3), false, 0.01f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                    UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                }

                //Normal Socket and Text Description:
                switch (DrawPhysics.visualizationQuality)
                {
                    case DrawPhysics.VisualizationQuality.high_withFullDetails:
                        DrawNormalSocketAndText_highQuality(hitInfo, i_hit, nameText, color_ofNormal, saveDrawnLines, durationInSec, hiddenByNearerObjects);
                        break;
                    case DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes:
                        DrawNormalSocketAndText_mediumQuality(hitInfo, nameText, color_ofNormal, durationInSec, hiddenByNearerObjects);
                        break;
                    case DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes:
                        //normal socket:
                        DrawShapes.Square(hitInfo.point, 0.12f, color_ofNormal, hitInfo.normal, direction, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
                        break;
                    default:
                        break;
                }

            }
        }

        static void DrawNormalSocketAndText_highQuality(RaycastHit hitInfo, int i_hit, string nameText, Color color_ofNormal, bool saveDrawnLines, float durationInSec, bool hiddenByNearerObjects)
        {
            //normal socket:
            DrawShapes.Decagon(hitInfo.point, 0.02f, color_ofNormal, hitInfo.normal, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
            DrawShapes.Decagon(hitInfo.point, 0.04f, color_ofNormal, hitInfo.normal, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
            DrawShapes.Decagon(hitInfo.point, 0.06f, color_ofNormal, hitInfo.normal, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);

            //text description:
            Vector3 textOffsetDir = default(Vector3);
            float textOffsetDistance = DrawPhysics.scaleFactor_forCastHitTextSize;
            string text;
            if (DrawPhysics.drawCastNameTag_atHitPositions && nameText != null && nameText.Length != 0)
            {
                text = (saveDrawnLines ? (nameText + " / #" + i_hit + ":<br>hit GO: " + hitInfo.transform.gameObject.name + "<br>pos = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo.point) + "<br>dist = " + hitInfo.distance) : (GetStrokeWidthMarkupStartStringForHitPosDesctiptionHeaders(nameText) + nameText + " / hit #" + i_hit + ":</sw><br>GameObject that was hit: " + hitInfo.transform.gameObject.name + "<br>position = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo.point) + "<br>distance = " + hitInfo.distance));
            }
            else
            {
                text = (saveDrawnLines ? ("hit #" + i_hit + ":<br>hit GO: " + hitInfo.transform.gameObject.name + "<br>pos = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo.point) + "<br>dist = " + hitInfo.distance) : ("<sw=80000>Raycast hit #" + i_hit + ":</sw><br>GameObject that was hit: " + hitInfo.transform.gameObject.name + "<br>position = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo.point) + "<br>distance = " + hitInfo.distance));
            }

            TrySet_default_textOffsetDirection_forPointTags_reversible();
            DrawBasics.PointTag(hitInfo.point, text, DrawPhysics.colorForCastsHitText, 0.0f, textOffsetDistance, textOffsetDir, 1.0f, false, durationInSec, hiddenByNearerObjects);
            TryReverse_default_textOffsetDirection_forPointTags();
        }

        static void DrawNormalSocketAndText_mediumQuality(RaycastHit hitInfo, string nameText, Color color_ofNormal, float durationInSec, bool hiddenByNearerObjects)
        {
            //normal socket:
            DrawShapes.Decagon(hitInfo.point, 0.06f, color_ofNormal, hitInfo.normal, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);

            //text description:
            if (DrawPhysics.drawCastNameTag_atHitPositions && nameText != null && nameText.Length != 0)
            {
                UtilitiesDXXL_Text.WriteFramed(nameText, hitInfo.point, DrawPhysics.colorForCastsHitText, 0.1f * DrawPhysics.scaleFactor_forCastHitTextSize, default(Vector3), default(Vector3), DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void DrawSpherecastTillFirstHit(float sphereRadius, bool hasHit, Vector3 origin, Vector3 direction, float maxDistance, RaycastHit hitInfo, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(direction))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(origin, "[<color=#adadadFF><icon=logMessage></color> DrawSpherecast with direction of zero]<br>" + nameText, DrawPhysics.colorForNonHittingCasts, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            bool radiusIsZero = UtilitiesDXXL_Math.ApproximatelyZero(sphereRadius);
            bool sphereHasNegativeRadius = sphereRadius < 0.0f;
            sphereRadius = Mathf.Abs(sphereRadius);
            float distanceOfFarestHit = GetDistanceOfSingleHit(hasHit, hitInfo);
            Vector3 direction_normalized = DrawRayOfSpherecast(sphereRadius, sphereHasNegativeRadius, radiusIsZero, hasHit ? 1 : 0, origin, direction, maxDistance, nameText, distanceOfFarestHit, durationInSec, hiddenByNearerObjects);
            if (hasHit)
            {
                DrawSpherecastHitInfo(origin, direction_normalized, sphereRadius, sphereHasNegativeRadius, hitInfo, 0, nameText, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void DrawSpherecastPotMultipleHits(float sphereRadius, Vector3 origin, Vector3 direction, float maxDistance, RaycastHit[] hitInfos, int numberOfUsedSlotsInHitInfoArray, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(direction))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(origin, "[<color=#adadadFF><icon=logMessage></color> DrawSpherecast with direction of zero]<br>" + nameText, DrawPhysics.colorForNonHittingCasts, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            bool radiusIsZero = UtilitiesDXXL_Math.ApproximatelyZero(sphereRadius);
            bool sphereHasNegativeRadius = sphereRadius < 0.0f;
            sphereRadius = Mathf.Abs(sphereRadius);
            float distanceOfFarestHit = GetDistanceOfFarestHit(hitInfos, numberOfUsedSlotsInHitInfoArray);
            Vector3 direction_normalized = DrawRayOfSpherecast(sphereRadius, sphereHasNegativeRadius, radiusIsZero, numberOfUsedSlotsInHitInfoArray, origin, direction, maxDistance, nameText, distanceOfFarestHit, durationInSec, hiddenByNearerObjects);
            for (int i = 0; i < numberOfUsedSlotsInHitInfoArray; i++)
            {
                DrawSpherecastHitInfo(origin, direction_normalized, sphereRadius, sphereHasNegativeRadius, hitInfos[i], i, nameText, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void DrawBoxcastTillFirstHit(bool hasHit, Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float maxDistance, RaycastHit hitInfo, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(direction))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(origin, "[<color=#adadadFF><icon=logMessage></color> DrawBoxcast with direction of zero]<br>" + nameText, DrawPhysics.colorForNonHittingCasts, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            bool boxScaleIsZero = UtilitiesDXXL_Math.ApproximatelyZero(halfExtents);
            bool atLeastOneBoxDimIsNegative = UtilitiesDXXL_Math.ContainsNegativeComponents(halfExtents);
            Vector3 boxSize = 2.0f * halfExtents; //Note: inconsistent naming inside Unity: Here: "halfExtents" = "halfSize", while in "Bounds": "extents" = "halfSize"
            Vector3 boxForward_normalized = orientation * Vector3.forward;
            Vector3 boxUp_normalized = orientation * Vector3.up;

            float distanceOfFarestHit = GetDistanceOfSingleHit(hasHit, hitInfo);
            Vector3 direction_normalized = DrawRayOfBoxcast(hasHit ? 1 : 0, origin, boxSize, atLeastOneBoxDimIsNegative, boxScaleIsZero, boxForward_normalized, boxUp_normalized, orientation, direction, maxDistance, nameText, distanceOfFarestHit, durationInSec, hiddenByNearerObjects);
            if (hasHit)
            {
                DrawBoxcastHitInfo(origin, direction_normalized, boxSize, atLeastOneBoxDimIsNegative, boxForward_normalized, boxUp_normalized, hitInfo, 0, nameText, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void DrawBoxcastPotMultipleHits(Vector3 origin, Vector3 halfExtents, Quaternion orientation, Vector3 direction, float maxDistance, RaycastHit[] hitInfos, int numberOfUsedSlotsInHitInfoArray, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(direction))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(origin, "[<color=#adadadFF><icon=logMessage></color> DrawBoxcast with direction of zero]<br>" + nameText, DrawPhysics.colorForNonHittingCasts, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            bool boxScaleIsZero = UtilitiesDXXL_Math.ApproximatelyZero(halfExtents);
            bool atLeastOneBoxDimIsNegative = UtilitiesDXXL_Math.ContainsNegativeComponents(halfExtents);
            Vector3 boxSize = 2.0f * halfExtents; //Note: inconsistent naming inside Unity: Here: "halfExtents" = "halfSize", while in "Bounds": "extents" = "halfSize"
            Vector3 boxForward_normalized = orientation * Vector3.forward;
            Vector3 boxUp_normalized = orientation * Vector3.up;

            float distanceOfFarestHit = GetDistanceOfFarestHit(hitInfos, numberOfUsedSlotsInHitInfoArray);
            Vector3 direction_normalized = DrawRayOfBoxcast(numberOfUsedSlotsInHitInfoArray, origin, boxSize, atLeastOneBoxDimIsNegative, boxScaleIsZero, boxForward_normalized, boxUp_normalized, orientation, direction, maxDistance, nameText, distanceOfFarestHit, durationInSec, hiddenByNearerObjects);
            for (int i = 0; i < numberOfUsedSlotsInHitInfoArray; i++)
            {
                DrawBoxcastHitInfo(origin, direction_normalized, boxSize, atLeastOneBoxDimIsNegative, boxForward_normalized, boxUp_normalized, hitInfos[i], i, nameText, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void DrawCapsulecastTillFirstHit(float capsuleRadius, bool hasHit, Vector3 posOfCapsuleSphere1_atCastStart, Vector3 posOfCapsuleSphere2_atCastStart, Vector3 direction, float maxDistance, RaycastHit hitInfo, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(direction))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(posOfCapsuleSphere1_atCastStart, "[<color=#adadadFF><icon=logMessage></color> DrawCapsulecast with direction of zero]<br>" + nameText, DrawPhysics.colorForNonHittingCasts, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            bool radiusIsZero = UtilitiesDXXL_Math.ApproximatelyZero(capsuleRadius);
            bool capsuleHasNegativeRadius = capsuleRadius < 0.0f;
            capsuleRadius = Mathf.Abs(capsuleRadius);
            float distanceOfFarestHit = GetDistanceOfSingleHit(hasHit, hitInfo);
            Vector3 direction_normalized = DrawRayOfCapsulecast(posOfCapsuleSphere1_atCastStart, posOfCapsuleSphere2_atCastStart, capsuleRadius, capsuleHasNegativeRadius, radiusIsZero, hasHit ? 1 : 0, direction, maxDistance, nameText, distanceOfFarestHit, durationInSec, hiddenByNearerObjects);
            if (hasHit)
            {
                DrawCapsulecastHitInfo(posOfCapsuleSphere1_atCastStart, posOfCapsuleSphere2_atCastStart, direction_normalized, capsuleRadius, capsuleHasNegativeRadius, hitInfo, 0, nameText, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void DrawCapsulecastPotMultipleHits(float capsuleRadius, Vector3 posOfCapsuleSphere1_atCastStart, Vector3 posOfCapsuleSphere2_atCastStart, Vector3 direction, float maxDistance, RaycastHit[] hitInfos, int numberOfUsedSlotsInHitInfoArray, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(direction))
            {
                UtilitiesDXXL_DrawBasics.PointFallback(posOfCapsuleSphere1_atCastStart, "[<color=#adadadFF><icon=logMessage></color> DrawCapsulecast with direction of zero]<br>" + nameText, DrawPhysics.colorForNonHittingCasts, 0.0f, durationInSec, hiddenByNearerObjects);
                return;
            }

            bool radiusIsZero = UtilitiesDXXL_Math.ApproximatelyZero(capsuleRadius);
            bool capsuleHasNegativeRadius = capsuleRadius < 0.0f;
            capsuleRadius = Mathf.Abs(capsuleRadius);
            float distanceOfFarestHit = GetDistanceOfFarestHit(hitInfos, numberOfUsedSlotsInHitInfoArray);
            Vector3 direction_normalized = DrawRayOfCapsulecast(posOfCapsuleSphere1_atCastStart, posOfCapsuleSphere2_atCastStart, capsuleRadius, capsuleHasNegativeRadius, radiusIsZero, numberOfUsedSlotsInHitInfoArray, direction, maxDistance, nameText, distanceOfFarestHit, durationInSec, hiddenByNearerObjects);
            for (int i = 0; i < numberOfUsedSlotsInHitInfoArray; i++)
            {
                DrawCapsulecastHitInfo(posOfCapsuleSphere1_atCastStart, posOfCapsuleSphere2_atCastStart, direction_normalized, capsuleRadius, capsuleHasNegativeRadius, hitInfos[i], i, nameText, durationInSec, hiddenByNearerObjects);
            }
        }

        static int strutsPerCastSphere = 8;
        static int strutsPerCastCapsule = 8;
        static Vector3[] volumeCastOutlineVertices_local = new Vector3[34];
        static Vector3[] volumeCastOutlineVerticesReduced_local = new Vector3[6];
        static InternalDXXL_Plane planePerpToCastDir_throughCapsulesSphere1AtCastStartPos = new InternalDXXL_Plane();
        static InternalDXXL_Plane castDirPlaneThroughWorldZeroOrigin = new InternalDXXL_Plane();
        static Vector3 DrawRayOfSpherecast(float sphereRadius, bool sphereHasNegativeRadius, bool radiusIsZero, int hitCount, Vector3 origin, Vector3 direction, float maxDistance, string nameText, float distanceOfFarestHit, float durationInSec, bool hiddenByNearerObjects)
        {
            bool hasHit = hitCount > 0;
            Vector3 direction_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(direction);
            bool hasUnlimitedLength = float.IsInfinity(maxDistance);
            maxDistance = Mathf.Min(maxDistance, 100000.0f);
            maxDistance = Mathf.Max(maxDistance, -100000.0f);
            Vector3 endPos = origin + direction_normalized * maxDistance;

            Color color = hasHit ? DrawPhysics.colorForHittingCasts : DrawPhysics.colorForNonHittingCasts;
            Color color_lowAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.5f);
            Color color_lowerAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.25f);

            Color color_ofCastEnd = hasHit ? DrawPhysics.colorForCastLineBeyondHit : DrawPhysics.colorForNonHittingCasts;
            Color color_ofCastEnd_lowAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofCastEnd, 0.5f);
            Color color_ofCastEnd_lowerAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofCastEnd, 0.25f);

            if (radiusIsZero == false)
            {
                float arrowWidth = sphereRadius * 0.5f;
                float arrowLength = sphereRadius * 1.0f;
                float arrowsRelConeLenth = 0.45f;
                float sphereDiameter = 2.0f * sphereRadius;

                DrawSphereAtCastStartAndEnd(origin, endPos, direction_normalized, sphereRadius, color, color_ofCastEnd_lowerAlpha, durationInSec, hiddenByNearerObjects);
                float startArrows_startDistanceFromStart = sphereDiameter;
                DrawArrowsAtVolumeCastStartAndEnd(out float distance_ofStartArrowsStartPos, out float distance_ofEndArrowsStartPos, hasHit, origin, direction_normalized, startArrows_startDistanceFromStart, maxDistance, distanceOfFarestHit, arrowLength, color_lowerAlpha, color_ofCastEnd_lowerAlpha, arrowsRelConeLenth, arrowWidth, durationInSec, hiddenByNearerObjects);
                int usedSlotsIn_verticesPerOutlineCircle = FillSphereCastOutlineVerticesArray(origin, direction_normalized, sphereRadius);
                float sizeApproximationOfVolume = sphereDiameter;
                DrawCascadeOfArrows(distance_ofStartArrowsStartPos, distance_ofEndArrowsStartPos, sizeApproximationOfVolume, hasHit, hasUnlimitedLength, origin, direction_normalized, distanceOfFarestHit, arrowLength, arrowWidth, arrowsRelConeLenth, color_lowerAlpha, color_ofCastEnd_lowerAlpha, durationInSec, hiddenByNearerObjects);
                DrawCascadeOfVolumeSilhouettes(usedSlotsIn_verticesPerOutlineCircle, sizeApproximationOfVolume, hasHit, hasUnlimitedLength, origin, direction_normalized, maxDistance, distanceOfFarestHit, color, color_ofCastEnd, durationInSec, hiddenByNearerObjects);
            }

            int usedSlotsIn_verticesPerReducedOutlineCircle = FillSphereCastReducedOutlineVerticesArray(origin, direction_normalized, sphereRadius, radiusIsZero);
            DrawVolumeCastDirOutline(usedSlotsIn_verticesPerReducedOutlineCircle, hasHit, origin, endPos, direction_normalized, distanceOfFarestHit, color_lowAlpha, color_ofCastEnd_lowAlpha, durationInSec, hiddenByNearerObjects);
            WriteTextAtSphereCastOrigin(origin, direction_normalized, sphereRadius, sphereHasNegativeRadius, color, nameText, hitCount, maxDistance, durationInSec, hiddenByNearerObjects);
            return direction_normalized;
        }

        static Vector3 DrawRayOfBoxcast(int hitCount, Vector3 origin, Vector3 boxSize, bool atLeastOneBoxDimIsNegative, bool boxScaleIsZero, Vector3 boxForward_normalized, Vector3 boxUp_normalized, Quaternion boxRotation, Vector3 direction, float maxDistance, string nameText, float distanceOfFarestHit, float durationInSec, bool hiddenByNearerObjects)
        {
            bool hasHit = hitCount > 0;
            Vector3 direction_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(direction);
            bool hasUnlimitedLength = float.IsInfinity(maxDistance);
            maxDistance = Mathf.Min(maxDistance, 100000.0f);
            maxDistance = Mathf.Max(maxDistance, -100000.0f);
            Vector3 endPos = origin + direction_normalized * maxDistance;

            Color color = hasHit ? DrawPhysics.colorForHittingCasts : DrawPhysics.colorForNonHittingCasts;
            Color color_lowAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.5f);
            Color color_lowerAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.25f);

            Color color_ofCastEnd = hasHit ? DrawPhysics.colorForCastLineBeyondHit : DrawPhysics.colorForNonHittingCasts;
            Color color_ofCastEnd_lowAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofCastEnd, 0.5f);
            Color color_ofCastEnd_lowerAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofCastEnd, 0.25f);

            Vector3 absBoxSize = UtilitiesDXXL_Math.Abs(boxSize);
            float averageBoxSize = 0.3333f * (absBoxSize.x + absBoxSize.y + absBoxSize.z);

            int usedSlotsIn_verticesPerReducedOutlineCircle = 1;
            if (boxScaleIsZero == false)
            {
                float arrowWidth = averageBoxSize * 0.25f;
                float arrowLength = averageBoxSize * 0.5f;
                float arrowsRelConeLenth = 0.45f;

                Vector3 boxRight_normalized = Vector3.Cross(boxUp_normalized, boxForward_normalized);
                DrawBoxAtCastStartAndEnd(origin, endPos, boxSize, boxForward_normalized, boxUp_normalized, color, color_ofCastEnd_lowerAlpha, durationInSec, hiddenByNearerObjects);
                float startArrows_startDistanceFromStart = averageBoxSize * 0.5f + arrowLength;
                DrawArrowsAtVolumeCastStartAndEnd(out float distance_ofStartArrowsStartPos, out float distance_ofEndArrowsStartPos, hasHit, origin, direction_normalized, startArrows_startDistanceFromStart, maxDistance, distanceOfFarestHit, arrowLength, color_lowerAlpha, color_ofCastEnd_lowerAlpha, arrowsRelConeLenth, arrowWidth, durationInSec, hiddenByNearerObjects);
                usedSlotsIn_verticesPerReducedOutlineCircle = FillBoxCastReducedOutlineVerticesArray(direction_normalized, boxSize, boxRotation, boxForward_normalized, boxUp_normalized, boxRight_normalized);
                int usedSlotsIn_verticesPerOutlineCircle = FillBoxCastOutlineVerticesArray(direction_normalized, usedSlotsIn_verticesPerReducedOutlineCircle);
                float sizeApproximationOfVolume = averageBoxSize;
                DrawCascadeOfArrows(distance_ofStartArrowsStartPos, distance_ofEndArrowsStartPos, sizeApproximationOfVolume, hasHit, hasUnlimitedLength, origin, direction_normalized, distanceOfFarestHit, arrowLength, arrowWidth, arrowsRelConeLenth, color_lowerAlpha, color_ofCastEnd_lowerAlpha, durationInSec, hiddenByNearerObjects);
                DrawCascadeOfVolumeSilhouettes(usedSlotsIn_verticesPerOutlineCircle, sizeApproximationOfVolume, hasHit, hasUnlimitedLength, origin, direction_normalized, maxDistance, distanceOfFarestHit, color, color_ofCastEnd, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                volumeCastOutlineVerticesReduced_local[0] = Vector3.zero;
            }

            DrawVolumeCastDirOutline(usedSlotsIn_verticesPerReducedOutlineCircle, hasHit, origin, endPos, direction_normalized, distanceOfFarestHit, color_lowAlpha, color_ofCastEnd_lowAlpha, durationInSec, hiddenByNearerObjects);
            WriteTextAtBoxCastOrigin(origin, direction_normalized, boxSize, atLeastOneBoxDimIsNegative, boxUp_normalized, color, nameText, hitCount, maxDistance, averageBoxSize, durationInSec, hiddenByNearerObjects);
            return direction_normalized;
        }

        static Vector3 DrawRayOfCapsulecast(Vector3 posOfCapsuleSphere1_atCastStart, Vector3 posOfCapsuleSphere2_atCastStart, float capsuleRadius, bool capsuleHasNegativeRadius, bool radiusIsZero, int hitCount, Vector3 direction, float maxDistance, string nameText, float distanceOfFarestHit, float durationInSec, bool hiddenByNearerObjects)
        {
            bool hasHit = hitCount > 0;
            Vector3 direction_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(direction);
            bool hasUnlimitedLength = float.IsInfinity(maxDistance);
            maxDistance = Mathf.Min(maxDistance, 100000.0f);
            maxDistance = Mathf.Max(maxDistance, -100000.0f);

            Color color = hasHit ? DrawPhysics.colorForHittingCasts : DrawPhysics.colorForNonHittingCasts;
            Color color_lowAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.5f);
            Color color_lowerAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.25f);

            Color color_ofCastEnd = hasHit ? DrawPhysics.colorForCastLineBeyondHit : DrawPhysics.colorForNonHittingCasts;
            Color color_ofCastEnd_lowAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofCastEnd, 0.5f);
            Color color_ofCastEnd_lowerAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofCastEnd, 0.25f);

            float capsuleSpheresDiameter = 2.0f * capsuleRadius;
            bool capsuleIsSqueezedToSphere = UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(posOfCapsuleSphere1_atCastStart, posOfCapsuleSphere2_atCastStart);
            Vector3 capsuleUp_normalized; //sphere1 is "upper" sphere
            float distanceBetweenSpheres;
            if (capsuleIsSqueezedToSphere)
            {
                capsuleUp_normalized = UtilitiesDXXL_Math.Get_aNormalizedVector_perpToGivenVector(direction_normalized);
                distanceBetweenSpheres = 0.0f;
            }
            else
            {
                Vector3 capsuleUp = posOfCapsuleSphere1_atCastStart - posOfCapsuleSphere2_atCastStart;
                capsuleUp_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(capsuleUp, out distanceBetweenSpheres);
            }
            bool capsuleAppearsAsSphereAlongCastDir = capsuleIsSqueezedToSphere || UtilitiesDXXL_Math.Check_ifTwoNormalizedVectorsAreApproxParallel_butCanHeadToDifferntDirs_padding(direction_normalized, capsuleUp_normalized);

            Vector3 posOfCapsuleSphere1_atCastEnd = posOfCapsuleSphere1_atCastStart + direction_normalized * maxDistance;
            Vector3 posOfCapsuleSphere2_atCastEnd = posOfCapsuleSphere2_atCastStart + direction_normalized * maxDistance;

            Vector3 towardsRight_ofSphere1_viewedAlongCastDir_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(Vector3.Cross(capsuleUp_normalized, direction_normalized));
            //is only used if "capsuleAppearsAsSphereAlongCastDir == false" (an in these cases cannot become zero):
            Vector3 towardsRight_ofSphere1_viewedAlongCastDir = towardsRight_ofSphere1_viewedAlongCastDir_normalized * capsuleRadius;

            if (radiusIsZero == false)
            {
                Vector3 capsuleSphere1ToCapsuleSilhoutteCenter_perpToCastDir;
                float sizeApproximationOfCapsule = capsuleSpheresDiameter + 0.5f * distanceBetweenSpheres;
                float sizeApproximationOfCapsule_insidePerpToCastDirPlane;
                Vector3 sphere1_to_sphere2 = posOfCapsuleSphere2_atCastStart - posOfCapsuleSphere1_atCastStart;
                float distance_fromSphere1ActingAsOrigin_toActualCapsuleCenter_alongCastDir = 0.5f * Vector3.Project(sphere1_to_sphere2, direction_normalized).magnitude;
                float signedDistance_fromSphere1ActingAsOrigin_toActualCapsuleCenter_alongCastDir = UtilitiesDXXL_Math.GetSign_trueGivesPlus1_falseGivesMinus1(UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(sphere1_to_sphere2, direction_normalized)) * distance_fromSphere1ActingAsOrigin_toActualCapsuleCenter_alongCastDir;
                if (capsuleAppearsAsSphereAlongCastDir)
                {
                    capsuleSphere1ToCapsuleSilhoutteCenter_perpToCastDir = Vector3.zero;
                    sizeApproximationOfCapsule_insidePerpToCastDirPlane = capsuleSpheresDiameter;
                }
                else
                {
                    Vector3 capsuleCenter_atStartPos = 0.5f * (posOfCapsuleSphere1_atCastStart + posOfCapsuleSphere2_atCastStart);
                    planePerpToCastDir_throughCapsulesSphere1AtCastStartPos.Recreate(posOfCapsuleSphere1_atCastStart, direction_normalized);
                    Vector3 capsuleSphere1ToCapsuleSilhoutteCenter = capsuleCenter_atStartPos - posOfCapsuleSphere1_atCastStart;
                    capsuleSphere1ToCapsuleSilhoutteCenter_perpToCastDir = planePerpToCastDir_throughCapsulesSphere1AtCastStartPos.Get_projectionOfVectorOntoPlane(capsuleSphere1ToCapsuleSilhoutteCenter);
                    sizeApproximationOfCapsule_insidePerpToCastDirPlane = capsuleSpheresDiameter + 2.0f * capsuleSphere1ToCapsuleSilhoutteCenter_perpToCastDir.magnitude;
                }
                float arrowWidth = sizeApproximationOfCapsule_insidePerpToCastDirPlane * 0.25f;
                float arrowLength = sizeApproximationOfCapsule_insidePerpToCastDirPlane * 0.5f;
                float arrowsRelConeLenth = 0.45f;

                DrawCapsuleAtCastStartAndEnd(direction_normalized, posOfCapsuleSphere1_atCastStart, posOfCapsuleSphere2_atCastStart, posOfCapsuleSphere1_atCastEnd, posOfCapsuleSphere2_atCastEnd, capsuleRadius, color, color_ofCastEnd_lowerAlpha, durationInSec, hiddenByNearerObjects);
                float distanceFromOrigin_toStartOfStartArrow_excludingCompensationForCapsulesWhereOriginIsNotCapsuleCenter = capsuleSpheresDiameter;
                DrawArrowsAtVolumeCastStartAndEnd(out float distance_ofStartArrowsStartPos, out float distance_ofEndArrowsStartPos, hasHit, posOfCapsuleSphere1_atCastStart, direction_normalized, distanceFromOrigin_toStartOfStartArrow_excludingCompensationForCapsulesWhereOriginIsNotCapsuleCenter, maxDistance, distanceOfFarestHit, arrowLength, color_lowerAlpha, color_ofCastEnd_lowerAlpha, arrowsRelConeLenth, arrowWidth, durationInSec, hiddenByNearerObjects, capsuleSphere1ToCapsuleSilhoutteCenter_perpToCastDir, signedDistance_fromSphere1ActingAsOrigin_toActualCapsuleCenter_alongCastDir);
                int usedSlotsIn_verticesPerOutlineCircle = FillCapsuleCastOutlineVerticesArray(capsuleAppearsAsSphereAlongCastDir, posOfCapsuleSphere1_atCastStart, posOfCapsuleSphere2_atCastStart, direction_normalized, capsuleRadius, towardsRight_ofSphere1_viewedAlongCastDir);
                DrawCascadeOfArrows(distance_ofStartArrowsStartPos, distance_ofEndArrowsStartPos, sizeApproximationOfCapsule, hasHit, hasUnlimitedLength, posOfCapsuleSphere1_atCastStart, direction_normalized, distanceOfFarestHit, arrowLength, arrowWidth, arrowsRelConeLenth, color_lowerAlpha, color_ofCastEnd_lowerAlpha, durationInSec, hiddenByNearerObjects, capsuleSphere1ToCapsuleSilhoutteCenter_perpToCastDir);
                DrawCascadeOfVolumeSilhouettes(usedSlotsIn_verticesPerOutlineCircle, sizeApproximationOfCapsule, hasHit, hasUnlimitedLength, posOfCapsuleSphere1_atCastStart, direction_normalized, maxDistance, distanceOfFarestHit, color, color_ofCastEnd, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                DrawCapsuleAtCastStartAndEnd_fallbackForZeroRadiusLine(direction_normalized, posOfCapsuleSphere1_atCastStart, posOfCapsuleSphere2_atCastStart, posOfCapsuleSphere1_atCastEnd, posOfCapsuleSphere2_atCastEnd, color, color_ofCastEnd_lowerAlpha, durationInSec, hiddenByNearerObjects);
            }

            int usedSlotsIn_verticesPerReducedOutlineCircle = FillCapsuleCastReducedOutlineVerticesArray(capsuleAppearsAsSphereAlongCastDir, posOfCapsuleSphere1_atCastStart, posOfCapsuleSphere2_atCastStart, direction_normalized, capsuleRadius, radiusIsZero, towardsRight_ofSphere1_viewedAlongCastDir);
            DrawVolumeCastDirOutline(usedSlotsIn_verticesPerReducedOutlineCircle, hasHit, posOfCapsuleSphere1_atCastStart, posOfCapsuleSphere1_atCastEnd, direction_normalized, distanceOfFarestHit, color_lowAlpha, color_ofCastEnd_lowAlpha, durationInSec, hiddenByNearerObjects);
            WriteTextAtCapsuleCastOrigin(capsuleAppearsAsSphereAlongCastDir, posOfCapsuleSphere1_atCastStart, posOfCapsuleSphere2_atCastStart, direction_normalized, capsuleRadius, capsuleHasNegativeRadius, color, nameText, hitCount, maxDistance, durationInSec, hiddenByNearerObjects);
            return direction_normalized;
        }

        static void DrawSphereAtCastStartAndEnd(Vector3 origin, Vector3 endPos, Vector3 direction_normalized, float sphereRadius, Color color, Color color_ofCastEnd_lowerAlpha, float durationInSec, bool hiddenByNearerObjects)
        {
            switch (DrawPhysics.visualizationQuality)
            {
                case DrawPhysics.VisualizationQuality.high_withFullDetails:
                    UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(64);

                    //start sphere:
                    DrawShapes.Sphere(origin, sphereRadius, color, direction_normalized, default(Vector3), 0.0f, null, strutsPerCastSphere, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);

                    //end sphere:
                    DrawShapes.Sphere(endPos, sphereRadius, color_ofCastEnd_lowerAlpha, direction_normalized, default(Vector3), 0.0f, null, strutsPerCastSphere, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);

                    UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                    break;
                case DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes:
                    UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(32);

                    //start sphere:
                    DrawShapes.Sphere(origin, sphereRadius, color, direction_normalized, default(Vector3), 0.0f, null, strutsPerCastSphere, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);

                    //end sphere:
                    DrawShapes.Sphere(endPos, sphereRadius, color_ofCastEnd_lowerAlpha, direction_normalized, default(Vector3), 0.0f, null, 2, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);

                    UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                    break;
                case DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes:
                    UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(16);

                    //start sphere:
                    DrawShapes.Sphere(origin, sphereRadius, color, direction_normalized, default(Vector3), 0.0f, null, 2, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);

                    //end sphere:
                    //-> not supported

                    UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                    break;
                default:
                    break;
            }
        }

        static void DrawBoxAtCastStartAndEnd(Vector3 origin, Vector3 endPos, Vector3 boxSize, Vector3 boxForward_normalized, Vector3 boxUp_normalized, Color color, Color color_ofCastEnd_lowerAlpha, float durationInSec, bool hiddenByNearerObjects)
        {
            //start box:
            DrawShapes.Cube(origin, boxSize, color, boxUp_normalized, boxForward_normalized, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);

            //end box:
            switch (DrawPhysics.visualizationQuality)
            {
                case DrawPhysics.VisualizationQuality.high_withFullDetails:
                    DrawShapes.CubeFilled(endPos, boxSize, color_ofCastEnd_lowerAlpha, boxUp_normalized, boxForward_normalized, 0.0f, 4, null, DrawBasics.LineStyle.solid, default(Color), 0.01f, 1.0f, true, false, durationInSec, hiddenByNearerObjects);
                    break;
                case DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes:
                    DrawShapes.Cube(endPos, boxSize, color_ofCastEnd_lowerAlpha, boxUp_normalized, boxForward_normalized, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
                    break;
                case DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes:
                    break;
                default:
                    break;
            }
        }

        static void DrawCapsuleAtCastStartAndEnd(Vector3 direction_normalized, Vector3 posOfCapsuleSphere1_atCastStart, Vector3 posOfCapsuleSphere2_atCastStart, Vector3 posOfCapsuleSphere1_atCastEnd, Vector3 posOfCapsuleSphere2_atCastEnd, float capsuleRadius, Color color, Color color_ofCastEnd_lowerAlpha, float durationInSec, bool hiddenByNearerObjects)
        {
            switch (DrawPhysics.visualizationQuality)
            {
                case DrawPhysics.VisualizationQuality.high_withFullDetails:
                    UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(64);

                    //start capsule:
                    DrawShapes.Capsule(posOfCapsuleSphere1_atCastStart, posOfCapsuleSphere2_atCastStart, capsuleRadius, color, direction_normalized, 0.0f, null, strutsPerCastCapsule, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);

                    //end capsule:
                    DrawShapes.Capsule(posOfCapsuleSphere1_atCastEnd, posOfCapsuleSphere2_atCastEnd, capsuleRadius, color_ofCastEnd_lowerAlpha, direction_normalized, 0.0f, null, strutsPerCastCapsule, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);

                    UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                    break;
                case DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes:
                    UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(32);

                    //start capsule:
                    DrawShapes.Capsule(posOfCapsuleSphere1_atCastStart, posOfCapsuleSphere2_atCastStart, capsuleRadius, color, direction_normalized, 0.0f, null, strutsPerCastCapsule, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);

                    //end capsule:
                    DrawShapes.Capsule(posOfCapsuleSphere1_atCastEnd, posOfCapsuleSphere2_atCastEnd, capsuleRadius, color_ofCastEnd_lowerAlpha, direction_normalized, 0.0f, null, 2, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);

                    UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                    break;
                case DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes:
                    UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(16);

                    //start capsule:
                    DrawShapes.Capsule(posOfCapsuleSphere1_atCastStart, posOfCapsuleSphere2_atCastStart, capsuleRadius, color, direction_normalized, 0.0f, null, 2, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);

                    //end capsule:
                    //-> not supported

                    UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                    break;
                default:
                    break;
            }
        }

        static void DrawCapsuleAtCastStartAndEnd_fallbackForZeroRadiusLine(Vector3 direction_normalized, Vector3 posOfCapsuleSphere1_atCastStart, Vector3 posOfCapsuleSphere2_atCastStart, Vector3 posOfCapsuleSphere1_atCastEnd, Vector3 posOfCapsuleSphere2_atCastEnd, Color color, Color color_ofCastEnd_lowerAlpha, float durationInSec, bool hiddenByNearerObjects)
        {
            //This is also called if the capsule didn't only shrink to line, but to point. Calling "Draw.Line()" instead of " DrawShapes.Capsule" here has the advantage that no zeroExtentShape-Fallback is displayed.
            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(posOfCapsuleSphere1_atCastStart, posOfCapsuleSphere2_atCastStart) == false)
            {
                //start capsule:
                Line_fadeableAnimSpeed.InternalDraw(posOfCapsuleSphere1_atCastStart, posOfCapsuleSphere2_atCastStart, color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);

                //end capsule:
                Line_fadeableAnimSpeed.InternalDraw(posOfCapsuleSphere1_atCastEnd, posOfCapsuleSphere2_atCastEnd, color_ofCastEnd_lowerAlpha, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }
        }

        static void DrawArrowsAtVolumeCastStartAndEnd(out float distance_ofStartArrowsStartPos, out float distance_ofEndArrowsStartPos, bool hasHit, Vector3 origin, Vector3 direction_normalized, float distanceFromOrigin_toStartOfStartArrow_excludingCompensationForCapsulesWhereOriginIsNotCapsuleCenter, float maxDistance, float distanceOfFarestHit, float arrowLength, Color color_lowerAlpha, Color color_ofCastEnd_lowerAlpha, float arrowsRelConeLenth, float arrowWidth, float durationInSec, bool hiddenByNearerObjects, Vector3 capsuleSphere1ToCapsuleSilhoutteCenter_perpToCastDir = default(Vector3), float signedDistance_fromSphere1ActingAsOrigin_toActualCapsuleCenter_alongCastDir = 0.0f)
        {
            //"_excludingCompensationForCapsulesWhereOriginIsNotCapsuleCenter" -> capsules use sphere1(=upperSphere) as orgin here instead of the actual center of the capsule

            distance_ofStartArrowsStartPos = 0.0f;
            distance_ofEndArrowsStartPos = 0.0f;

            if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes) { return; }
            if (arrowLength > minArrowLength)
            {
                //at start:
                float original_distanceFromOrigin_toStartOfStartArrow_excludingCompensationForCapsules = distanceFromOrigin_toStartOfStartArrow_excludingCompensationForCapsulesWhereOriginIsNotCapsuleCenter;
                float distanceFromOrigin_toStartOfStartArrow = distanceFromOrigin_toStartOfStartArrow_excludingCompensationForCapsulesWhereOriginIsNotCapsuleCenter;
                if (signedDistance_fromSphere1ActingAsOrigin_toActualCapsuleCenter_alongCastDir > 0.0f)
                {
                    distanceFromOrigin_toStartOfStartArrow = distanceFromOrigin_toStartOfStartArrow + 2.0f * signedDistance_fromSphere1ActingAsOrigin_toActualCapsuleCenter_alongCastDir;
                }
                distanceFromOrigin_toStartOfStartArrow = Mathf.Min(distanceFromOrigin_toStartOfStartArrow, 0.5f * maxDistance);
                if (hasHit) { distanceFromOrigin_toStartOfStartArrow = Mathf.Min(distanceFromOrigin_toStartOfStartArrow, 0.5f * distanceOfFarestHit); }
                if (distanceFromOrigin_toStartOfStartArrow <= 0.0f) { distanceFromOrigin_toStartOfStartArrow = original_distanceFromOrigin_toStartOfStartArrow_excludingCompensationForCapsules; }
                bool isAfterLastHit = hasHit && ((distanceFromOrigin_toStartOfStartArrow + 0.5f * arrowLength) > distanceOfFarestHit);
                distance_ofStartArrowsStartPos = distanceFromOrigin_toStartOfStartArrow;
                Vector3 startVector_startPos = origin + direction_normalized * distanceFromOrigin_toStartOfStartArrow + capsuleSphere1ToCapsuleSilhoutteCenter_perpToCastDir;
                Vector3 startVector_endPos = origin + direction_normalized * (distanceFromOrigin_toStartOfStartArrow + arrowLength) + capsuleSphere1ToCapsuleSilhoutteCenter_perpToCastDir;

                UtilitiesDXXL_DrawBasics.Set_automaticAmplitudeAndTextAlignment_reversible(DrawBasics.AutomaticAmplitudeAndTextAlignment.perpendicularToCamera);
                UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                UtilitiesDXXL_DrawBasics.Vector(startVector_startPos, startVector_endPos, isAfterLastHit ? color_ofCastEnd_lowerAlpha : color_lowerAlpha, arrowWidth, null, arrowsRelConeLenth, false, true, false, 0.0f, false, durationInSec, hiddenByNearerObjects, null, false, 0.0f);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                UtilitiesDXXL_DrawBasics.Reverse_automaticAmplitudeAndTextAlignment();

                //at end:
                float distanceFromOrigin_toStartOfEndArrow = maxDistance - original_distanceFromOrigin_toStartOfStartArrow_excludingCompensationForCapsules - arrowLength;
                if (signedDistance_fromSphere1ActingAsOrigin_toActualCapsuleCenter_alongCastDir < 0.0f)
                {
                    distanceFromOrigin_toStartOfEndArrow = distanceFromOrigin_toStartOfEndArrow + 2.0f * signedDistance_fromSphere1ActingAsOrigin_toActualCapsuleCenter_alongCastDir;
                }
                distance_ofEndArrowsStartPos = distanceFromOrigin_toStartOfEndArrow;
                if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.high_withFullDetails)
                {
                    if (maxDistance > (distanceFromOrigin_toStartOfStartArrow + 12.0f * arrowLength))
                    {
                        isAfterLastHit = hasHit && ((distanceFromOrigin_toStartOfEndArrow + 0.5f * arrowLength) > distanceOfFarestHit);
                        Vector3 endVector_startPos = origin + direction_normalized * distanceFromOrigin_toStartOfEndArrow + capsuleSphere1ToCapsuleSilhoutteCenter_perpToCastDir;
                        Vector3 endVector_endPos = origin + direction_normalized * (distanceFromOrigin_toStartOfEndArrow + arrowLength) + capsuleSphere1ToCapsuleSilhoutteCenter_perpToCastDir;

                        UtilitiesDXXL_DrawBasics.Set_automaticAmplitudeAndTextAlignment_reversible(DrawBasics.AutomaticAmplitudeAndTextAlignment.perpendicularToCamera);
                        UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                        UtilitiesDXXL_DrawBasics.Vector(endVector_startPos, endVector_endPos, isAfterLastHit ? color_ofCastEnd_lowerAlpha : color_lowerAlpha, arrowWidth, null, arrowsRelConeLenth, false, true, false, 0.0f, false, durationInSec, hiddenByNearerObjects, null, false, 0.0f);
                        UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                        UtilitiesDXXL_DrawBasics.Reverse_automaticAmplitudeAndTextAlignment();
                    }
                }
            }
        }

        static int FillSphereCastOutlineVerticesArray(Vector3 origin, Vector3 direction_normalized, float sphereRadius)
        {
            int usedSlotsIn_verticesPerOutlineCircle = 32;
            UtilitiesDXXL_Shapes.DrawFlatPolygon(0.0f, usedSlotsIn_verticesPerOutlineCircle, origin, sphereRadius, direction_normalized, default, default, 0.0f, null, 0.0f, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, false, true);
            for (int i = 0; i < usedSlotsIn_verticesPerOutlineCircle; i++)
            {
                volumeCastOutlineVertices_local[i] = UtilitiesDXXL_Shapes.verticesGlobal[i] - origin;
            }
            return usedSlotsIn_verticesPerOutlineCircle;
        }

        static int FillSphereCastReducedOutlineVerticesArray(Vector3 origin, Vector3 direction_normalized, float sphereRadius, bool radiusIsZero)
        {
            if (radiusIsZero)
            {
                int usedSlotsIn_verticesPerReducedOutlineCircle = 1;
                volumeCastOutlineVerticesReduced_local[0] = Vector3.zero;
                return usedSlotsIn_verticesPerReducedOutlineCircle;
            }
            else
            {
                int usedSlotsIn_verticesPerReducedOutlineCircle = 4;
                UtilitiesDXXL_Shapes.DrawFlatPolygon(0.0f, usedSlotsIn_verticesPerReducedOutlineCircle, origin, sphereRadius, direction_normalized, default, default, 0.0f, null, 0.0f, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, false, true);
                for (int i = 0; i < usedSlotsIn_verticesPerReducedOutlineCircle; i++)
                {
                    volumeCastOutlineVerticesReduced_local[i] = UtilitiesDXXL_Shapes.verticesGlobal[i] - origin;
                }
                return usedSlotsIn_verticesPerReducedOutlineCircle;
            }
        }

        static int FillBoxCastOutlineVerticesArray(Vector3 direction_normalized, int usedSlotsIn_verticesPerReducedOutlineCircle)
        {
            int usedSlotsIn_verticesPerOutlineCircle = usedSlotsIn_verticesPerReducedOutlineCircle;
            castDirPlaneThroughWorldZeroOrigin.Recreate(Vector3.zero, direction_normalized);
            for (int i = 0; i < usedSlotsIn_verticesPerOutlineCircle; i++)
            {
                volumeCastOutlineVertices_local[i] = castDirPlaneThroughWorldZeroOrigin.Get_perpProjectionOfPointOnPlane(volumeCastOutlineVerticesReduced_local[i]);
            }
            return usedSlotsIn_verticesPerOutlineCircle;
        }

        ///cube definition:
        //viewed along z-forward:
        //starts with: nearer square, lowLeft, then counterclockwise
        //then: farer square, same pattern
        static Vector3[] unscaledUnrotatedBox = new Vector3[8] { new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(0.5f, -0.5f, -0.5f), new Vector3(0.5f, 0.5f, -0.5f), new Vector3(-0.5f, 0.5f, -0.5f), new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, 0.5f, 0.5f), new Vector3(-0.5f, 0.5f, 0.5f) };
        static int FillBoxCastReducedOutlineVerticesArray(Vector3 direction_normalized, Vector3 boxSize, Quaternion boxRotation, Vector3 boxForward_normalized, Vector3 boxUp_normalized, Vector3 boxRight_normalized)
        {
            int usedSlotsIn_verticesPerReducedOutlineCircle = 4;
            if (UtilitiesDXXL_Math.Check_ifTwoNormalizedVectorsAreApproxParallel_butCanHeadToDifferntDirs_padding(direction_normalized, boxForward_normalized))
            {
                //castDir parallel to box.forward/backward:
                volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[0], boxSize);
                volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[1], boxSize);
                volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[2], boxSize);
                volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[3], boxSize);
            }
            else
            {
                if (UtilitiesDXXL_Math.Check_ifTwoNormalizedVectorsAreApproxParallel_butCanHeadToDifferntDirs_padding(direction_normalized, boxUp_normalized))
                {
                    //castDir parallel to box.up/down:
                    volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[0], boxSize);
                    volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[1], boxSize);
                    volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[5], boxSize);
                    volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[4], boxSize);
                }
                else
                {
                    if (UtilitiesDXXL_Math.Check_ifTwoNormalizedVectorsAreApproxParallel_butCanHeadToDifferntDirs_padding(direction_normalized, boxRight_normalized))
                    {
                        //castDir parallel to box.right/left:
                        volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[0], boxSize);
                        volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[3], boxSize);
                        volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[7], boxSize);
                        volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[4], boxSize);
                    }
                    else
                    {
                        if (UtilitiesDXXL_Math.Check_ifTwoNormalizedVectorsAreApproxPerp_DXXL(direction_normalized, boxUp_normalized))
                        {
                            //box rotated around boxUp (seen along castDir):
                            if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized, boxForward_normalized))
                            {
                                if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized, boxRight_normalized))
                                {
                                    volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[1], boxSize);
                                    volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[2], boxSize);
                                    volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[7], boxSize);
                                    volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[4], boxSize);
                                }
                                else
                                {
                                    volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[0], boxSize);
                                    volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[3], boxSize);
                                    volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[6], boxSize);
                                    volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[5], boxSize);
                                }
                            }
                            else
                            {
                                if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized, boxRight_normalized))
                                {
                                    volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[0], boxSize);
                                    volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[3], boxSize);
                                    volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[6], boxSize);
                                    volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[5], boxSize);
                                }
                                else
                                {
                                    volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[1], boxSize);
                                    volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[2], boxSize);
                                    volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[7], boxSize);
                                    volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[4], boxSize);
                                }
                            }

                        }
                        else
                        {
                            if (UtilitiesDXXL_Math.Check_ifTwoNormalizedVectorsAreApproxPerp_DXXL(direction_normalized, boxForward_normalized))
                            {
                                //box rotated around boxForward (seen along castDir):
                                if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized, boxUp_normalized))
                                {
                                    if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized, boxRight_normalized))
                                    {
                                        volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[5], boxSize);
                                        volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[1], boxSize);
                                        volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[3], boxSize);
                                        volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[7], boxSize);
                                    }
                                    else
                                    {
                                        volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[4], boxSize);
                                        volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[6], boxSize);
                                        volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[2], boxSize);
                                        volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[0], boxSize);
                                    }
                                }
                                else
                                {
                                    if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized, boxRight_normalized))
                                    {
                                        volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[4], boxSize);
                                        volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[6], boxSize);
                                        volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[2], boxSize);
                                        volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[0], boxSize);
                                    }
                                    else
                                    {
                                        volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[5], boxSize);
                                        volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[1], boxSize);
                                        volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[3], boxSize);
                                        volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[7], boxSize);
                                    }
                                }
                            }
                            else
                            {
                                if (UtilitiesDXXL_Math.Check_ifTwoNormalizedVectorsAreApproxPerp_DXXL(direction_normalized, boxRight_normalized))
                                {
                                    //box rotated around boxRight (seen along castDir):
                                    if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized, boxForward_normalized))
                                    {
                                        if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized, boxUp_normalized))
                                        {
                                            volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[4], boxSize);
                                            volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[5], boxSize);
                                            volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[2], boxSize);
                                            volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[3], boxSize);
                                        }
                                        else
                                        {
                                            volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[0], boxSize);
                                            volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[1], boxSize);
                                            volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[6], boxSize);
                                            volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[7], boxSize);
                                        }
                                    }
                                    else
                                    {
                                        if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized, boxUp_normalized))
                                        {
                                            volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[0], boxSize);
                                            volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[1], boxSize);
                                            volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[6], boxSize);
                                            volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[7], boxSize);
                                        }
                                        else
                                        {
                                            volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[4], boxSize);
                                            volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[5], boxSize);
                                            volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[2], boxSize);
                                            volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[3], boxSize);
                                        }
                                    }
                                }
                                else
                                {
                                    //a cube corner points along castDir:
                                    usedSlotsIn_verticesPerReducedOutlineCircle = 6;
                                    if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized, boxForward_normalized))
                                    {
                                        if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized, boxUp_normalized))
                                        {
                                            if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized, boxRight_normalized))
                                            {
                                                volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[4], boxSize);
                                                volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[5], boxSize);
                                                volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[1], boxSize);
                                                volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[2], boxSize);
                                                volumeCastOutlineVerticesReduced_local[4] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[3], boxSize);
                                                volumeCastOutlineVerticesReduced_local[5] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[7], boxSize);
                                            }
                                            else
                                            {
                                                volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[0], boxSize);
                                                volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[4], boxSize);
                                                volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[5], boxSize);
                                                volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[6], boxSize);
                                                volumeCastOutlineVerticesReduced_local[4] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[2], boxSize);
                                                volumeCastOutlineVerticesReduced_local[5] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[3], boxSize);
                                            }
                                        }
                                        else
                                        {
                                            if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized, boxRight_normalized))
                                            {
                                                volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[0], boxSize);
                                                volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[1], boxSize);
                                                volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[2], boxSize);
                                                volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[6], boxSize);
                                                volumeCastOutlineVerticesReduced_local[4] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[7], boxSize);
                                                volumeCastOutlineVerticesReduced_local[5] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[4], boxSize);
                                            }
                                            else
                                            {
                                                volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[0], boxSize);
                                                volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[1], boxSize);
                                                volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[5], boxSize);
                                                volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[6], boxSize);
                                                volumeCastOutlineVerticesReduced_local[4] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[7], boxSize);
                                                volumeCastOutlineVerticesReduced_local[5] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[3], boxSize);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized, boxUp_normalized))
                                        {
                                            if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized, boxRight_normalized))
                                            {
                                                volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[0], boxSize);
                                                volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[1], boxSize);
                                                volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[5], boxSize);
                                                volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[6], boxSize);
                                                volumeCastOutlineVerticesReduced_local[4] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[7], boxSize);
                                                volumeCastOutlineVerticesReduced_local[5] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[3], boxSize);
                                            }
                                            else
                                            {
                                                volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[0], boxSize);
                                                volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[1], boxSize);
                                                volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[2], boxSize);
                                                volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[6], boxSize);
                                                volumeCastOutlineVerticesReduced_local[4] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[7], boxSize);
                                                volumeCastOutlineVerticesReduced_local[5] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[4], boxSize);
                                            }
                                        }
                                        else
                                        {
                                            if (UtilitiesDXXL_Math.Check_ifVectorsPointInSameDirection_perpCountsAsPointingInSameDir(direction_normalized, boxRight_normalized))
                                            {
                                                volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[0], boxSize);
                                                volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[4], boxSize);
                                                volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[5], boxSize);
                                                volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[6], boxSize);
                                                volumeCastOutlineVerticesReduced_local[4] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[2], boxSize);
                                                volumeCastOutlineVerticesReduced_local[5] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[3], boxSize);
                                            }
                                            else
                                            {
                                                volumeCastOutlineVerticesReduced_local[0] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[4], boxSize);
                                                volumeCastOutlineVerticesReduced_local[1] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[5], boxSize);
                                                volumeCastOutlineVerticesReduced_local[2] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[1], boxSize);
                                                volumeCastOutlineVerticesReduced_local[3] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[2], boxSize);
                                                volumeCastOutlineVerticesReduced_local[4] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[3], boxSize);
                                                volumeCastOutlineVerticesReduced_local[5] = boxRotation * Vector3.Scale(unscaledUnrotatedBox[7], boxSize);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return usedSlotsIn_verticesPerReducedOutlineCircle;
        }

        static int FillCapsuleCastOutlineVerticesArray(bool capsuleAppearsAsSphereAlongCastDir, Vector3 posOfCapsuleSphere1_atCastStart, Vector3 posOfCapsuleSphere2_atCastStart, Vector3 direction_normalized, float capsuleRadius, Vector3 towardsRight_ofSphere1_viewedAlongCastDir)
        {
            if (capsuleAppearsAsSphereAlongCastDir)
            {
                return FillSphereCastOutlineVerticesArray(posOfCapsuleSphere1_atCastStart, direction_normalized, capsuleRadius);
            }
            else
            {
                int usedSlotsIn_verticesPerOutlineCircle = 34;
                Vector3 towardsLeft_ofSphere1_viewedAlongCastDir = -towardsRight_ofSphere1_viewedAlongCastDir;

                Vector3 posOfCapsuleSphere2_projectedOntoCastDirPlaneThroughSphere1 = planePerpToCastDir_throughCapsulesSphere1AtCastStartPos.Get_perpProjectionOfPointOnPlane(posOfCapsuleSphere2_atCastStart);
                Vector3 sphere2projectionOntoCastDirPlane_localToSphere1 = posOfCapsuleSphere2_projectedOntoCastDirPlaneThroughSphere1 - posOfCapsuleSphere1_atCastStart;

                float angleDeg_perVertext = 180.0f / (float)(16 - 1);
                for (int i = 0; i < 17; i++)
                {
                    Quaternion rotation_ofCurrVertex = UnityEngine.Quaternion.AngleAxis(angleDeg_perVertext * i, direction_normalized);
                    volumeCastOutlineVertices_local[i] = rotation_ofCurrVertex * towardsRight_ofSphere1_viewedAlongCastDir;
                    volumeCastOutlineVertices_local[i + 17] = sphere2projectionOntoCastDirPlane_localToSphere1 + rotation_ofCurrVertex * towardsLeft_ofSphere1_viewedAlongCastDir;
                }
                return usedSlotsIn_verticesPerOutlineCircle;
            }
        }

        static int FillCapsuleCastReducedOutlineVerticesArray(bool capsuleAppearsAsSphereAlongCastDir, Vector3 posOfCapsuleSphere1_atCastStart, Vector3 posOfCapsuleSphere2_atCastStart, Vector3 direction_normalized, float capsuleRadius, bool radiusIsZero, Vector3 towardsRight_ofSphere1_viewedAlongCastDir)
        {
            Vector3 capsuleSphere1_toSphere2 = posOfCapsuleSphere2_atCastStart - posOfCapsuleSphere1_atCastStart;
            if (radiusIsZero)
            {
                int usedSlotsIn_verticesPerReducedOutlineCircle = 2;
                volumeCastOutlineVerticesReduced_local[0] = Vector3.zero;
                volumeCastOutlineVerticesReduced_local[1] = capsuleSphere1_toSphere2;
                return usedSlotsIn_verticesPerReducedOutlineCircle;
            }
            else
            {
                if (capsuleAppearsAsSphereAlongCastDir)
                {
                    return FillSphereCastReducedOutlineVerticesArray(posOfCapsuleSphere1_atCastStart, direction_normalized, capsuleRadius, false);
                }
                else
                {
                    int usedSlotsIn_verticesPerReducedOutlineCircle = 6;

                    Quaternion rotationFromRightToUp = UnityEngine.Quaternion.AngleAxis(90.0f, direction_normalized);
                    Vector3 towardsUp_ofSphere1_viewedAlongCastDir = rotationFromRightToUp * towardsRight_ofSphere1_viewedAlongCastDir;

                    volumeCastOutlineVerticesReduced_local[0] = towardsRight_ofSphere1_viewedAlongCastDir;
                    volumeCastOutlineVerticesReduced_local[1] = -towardsRight_ofSphere1_viewedAlongCastDir;
                    volumeCastOutlineVerticesReduced_local[2] = towardsUp_ofSphere1_viewedAlongCastDir;

                    volumeCastOutlineVerticesReduced_local[3] = capsuleSphere1_toSphere2 + towardsRight_ofSphere1_viewedAlongCastDir;
                    volumeCastOutlineVerticesReduced_local[4] = capsuleSphere1_toSphere2 - towardsRight_ofSphere1_viewedAlongCastDir;
                    volumeCastOutlineVerticesReduced_local[5] = capsuleSphere1_toSphere2 - towardsUp_ofSphere1_viewedAlongCastDir;

                    return usedSlotsIn_verticesPerReducedOutlineCircle;
                }
            }
        }

        static void DrawVolumeCastDirOutline(int usedSlotsIn_verticesPerReducedOutlineCircle, bool hasHit, Vector3 origin, Vector3 endPos, Vector3 direction_normalized, float distanceOfFarestHit, Color color_lowAlpha, Color color_ofCastEnd_lowAlpha, float durationInSec, bool hiddenByNearerObjects)
        {
            for (int i = 0; i < usedSlotsIn_verticesPerReducedOutlineCircle; i++)
            {
                if (hasHit)
                {
                    Vector3 posOfFarestHit = origin + direction_normalized * distanceOfFarestHit;
                    Line_fadeableAnimSpeed.InternalDraw(origin + volumeCastOutlineVerticesReduced_local[i], posOfFarestHit + volumeCastOutlineVerticesReduced_local[i], color_lowAlpha, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                    Line_fadeableAnimSpeed.InternalDraw(posOfFarestHit + volumeCastOutlineVerticesReduced_local[i], endPos + volumeCastOutlineVerticesReduced_local[i], color_ofCastEnd_lowAlpha, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                }
                else
                {
                    Line_fadeableAnimSpeed.InternalDraw(origin + volumeCastOutlineVerticesReduced_local[i], endPos + volumeCastOutlineVerticesReduced_local[i], color_lowAlpha, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                }
            }
        }

        static void DrawCascadeOfVolumeSilhouettes(int usedSlotsIn_verticesPerOutlineCircle, float sizeApproximationOfVolume, bool hasHit, bool hasUnlimitedLength, Vector3 origin, Vector3 direction_normalized, float maxDistance, float distanceOfFarestHit, Color color, Color color_ofCastEnd, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes) { return; }
            if (UtilitiesDXXL_Math.ApproximatelyZero(DrawPhysics.castSilhouetteVisualizerDensity) == false)
            {
                float distanceBetweenSilhouettes = hasUnlimitedLength ? (6.0f * sizeApproximationOfVolume) : (3.25f * sizeApproximationOfVolume);
                int maxSilhouettesPerVolumeCast = hasUnlimitedLength ? 5 : 15;
                if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes)
                {
                    distanceBetweenSilhouettes = hasUnlimitedLength ? (6.0f * sizeApproximationOfVolume) : (4.5f * sizeApproximationOfVolume);
                    maxSilhouettesPerVolumeCast = hasUnlimitedLength ? 2 : 5;
                }

                float used_castSilhouetteVisualizerDensity = DrawPhysics.castSilhouetteVisualizerDensity;
                used_castSilhouetteVisualizerDensity = Mathf.Max(used_castSilhouetteVisualizerDensity, 0.01f);
                used_castSilhouetteVisualizerDensity = Mathf.Min(used_castSilhouetteVisualizerDensity, 1000.0f);
                distanceBetweenSilhouettes = distanceBetweenSilhouettes / used_castSilhouetteVisualizerDensity;
                if (used_castSilhouetteVisualizerDensity > 1.0f) { maxSilhouettesPerVolumeCast = Mathf.RoundToInt(used_castSilhouetteVisualizerDensity * maxSilhouettesPerVolumeCast); }
                maxSilhouettesPerVolumeCast = Mathf.Min(maxSilhouettesPerVolumeCast, DrawPhysics.maxSilhouettesPerCastVisualization);

                distanceBetweenSilhouettes = Mathf.Max(distanceBetweenSilhouettes, 0.5f);
                float distance_ofCurrSilhouette = 0.0f;
                for (int i_silhouette = 0; i_silhouette < maxSilhouettesPerVolumeCast; i_silhouette++)
                {
                    distance_ofCurrSilhouette = distance_ofCurrSilhouette + distanceBetweenSilhouettes;
                    bool isAfterLastHit = hasHit && (distance_ofCurrSilhouette > distanceOfFarestHit);
                    if (distance_ofCurrSilhouette < maxDistance)
                    {
                        Vector3 posOfCurrSilhouetteOnRayLine = origin + distance_ofCurrSilhouette * direction_normalized;
                        for (int i_lineOfSilhouette = 0; i_lineOfSilhouette < usedSlotsIn_verticesPerOutlineCircle; i_lineOfSilhouette++)
                        {
                            Line_fadeableAnimSpeed.InternalDraw(posOfCurrSilhouetteOnRayLine + volumeCastOutlineVertices_local[i_lineOfSilhouette], posOfCurrSilhouetteOnRayLine + volumeCastOutlineVertices_local[UtilitiesDXXL_Math.LoopOvershootingIndexIntoCollectionSize(i_lineOfSilhouette + 1, usedSlotsIn_verticesPerOutlineCircle)], isAfterLastHit ? color_ofCastEnd : color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                        }
                    }
                    else
                    {
                        break;
                    }

                    if (hasUnlimitedLength)
                    {
                        distanceBetweenSilhouettes = (1.0f + (1.0f / used_castSilhouetteVisualizerDensity)) * distanceBetweenSilhouettes;
                    }
                    else
                    {
                        if (i_silhouette > 3 || (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes))
                        {
                            distanceBetweenSilhouettes = (1.0f + (0.5f / used_castSilhouetteVisualizerDensity)) * distanceBetweenSilhouettes;
                        }
                    }
                }
            }
        }

        static void DrawCascadeOfArrows(float distance_ofStartArrowsStartPos, float distance_ofEndArrowsStartPos, float sizeApproximationOfVolume, bool hasHit, bool hasUnlimitedLength, Vector3 origin, Vector3 direction_normalized, float distanceOfFarestHit, float arrowLength, float arrowWidth, float arrowsRelConeLenth, Color color_lowerAlpha, Color color_ofCastEnd_lowerAlpha, float durationInSec, bool hiddenByNearerObjects, Vector3 capsuleSphere1ToCapsuleSilhoutteCenter_perpToCastDir = default(Vector3))
        {
            if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.high_withFullDetails)
            {
                if (UtilitiesDXXL_Math.ApproximatelyZero(DrawPhysics.castSilhouetteVisualizerDensity) == false)
                {
                    if (arrowLength > minArrowLength)
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
                                Vector3 vectorStartPos = origin + distance_ofCurrArrowsStart * direction_normalized + capsuleSphere1ToCapsuleSilhoutteCenter_perpToCastDir;
                                Vector3 vectorEndPos = origin + (distance_ofCurrArrowsStart + arrowLength) * direction_normalized + capsuleSphere1ToCapsuleSilhoutteCenter_perpToCastDir;

                                UtilitiesDXXL_DrawBasics.Set_automaticAmplitudeAndTextAlignment_reversible(DrawBasics.AutomaticAmplitudeAndTextAlignment.perpendicularToCamera);
                                UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                                UtilitiesDXXL_DrawBasics.Vector(vectorStartPos, vectorEndPos, isAfterLastHit ? color_ofCastEnd_lowerAlpha : color_lowerAlpha, arrowWidth, null, arrowsRelConeLenth, false, true, false, 0.0f, false, durationInSec, hiddenByNearerObjects, null, false, 0.0f);
                                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
                                UtilitiesDXXL_DrawBasics.Reverse_automaticAmplitudeAndTextAlignment();
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

        static void WriteTextAtSphereCastOrigin(Vector3 origin, Vector3 direction_normalized, float sphereRadius, bool sphereHasNegativeRadius, Color color, string nameText, int hitCount, float maxDistance, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes) { return; }
            Vector3 sphereUp_normalized = UtilitiesDXXL_Math.Get_aNormalizedVector_perpToGivenVector(direction_normalized);
            if (sphereUp_normalized.y < 0.0f) { sphereUp_normalized = -sphereUp_normalized; }
            float textSize = 0.2f * sphereRadius;
            textSize = Mathf.Max(textSize, 0.02f);
            Color textColor = Color.Lerp(color, Color.black, 0.75f);
            string additinalWarningText = sphereHasNegativeRadius ? "[<color=#e2aa00FF><icon=warning></color> negative sphere radius -> invalid results and/or hitting sphere will be <i>inside</i> other colliders]<br>" : null;
            string castText = GetVolumeCastStartTextString("<size=27>Spherecast</size><br><size=5> </size><br>number of hits: ", nameText, hitCount, maxDistance, additinalWarningText);
            Vector3 textPos = origin + sphereUp_normalized * (sphereRadius + 0.65f * textSize);
            UtilitiesDXXL_Text.Write(castText, textPos, textColor, textSize, direction_normalized, sphereUp_normalized, DrawText.TextAnchorDXXL.LowerLeft, DrawBasics.LineStyle.solid, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects, false, false, true);
        }

        static void WriteTextAtBoxCastOrigin(Vector3 origin, Vector3 direction_normalized, Vector3 boxSize, bool atLeastOneBoxDimIsNegative, Vector3 boxUp_normalized, Color color, string nameText, int hitCount, float maxDistance, float averageBoxSize, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes) { return; }
            if (boxUp_normalized.y < 0.0f) { boxUp_normalized = -boxUp_normalized; }
            float absBoxHalfHeight = Mathf.Abs(0.5f * boxSize.y);
            float textSize = 0.1f * averageBoxSize;
            textSize = Mathf.Max(textSize, 0.02f);
            Color textColor = Color.Lerp(color, Color.black, 0.75f);
            string additinalWarningText = atLeastOneBoxDimIsNegative ? "[<color=#e2aa00FF><icon=warning></color> box contains negative dimensions<br>-> invalid results and/or hitting box may be <i>inside</i> other colliders<br>-> cast visualisation displays box silhouette errorneous]<br>" : null;
            string castText = GetVolumeCastStartTextString("<size=27>Boxcast</size><br><size=5> </size><br>number of hits: ", nameText, hitCount, maxDistance, additinalWarningText);
            Vector3 textPos = origin + boxUp_normalized * (absBoxHalfHeight + 0.65f * textSize);
            bool directionAndUp_areApproxParallel = UtilitiesDXXL_Math.Check_ifTwoNormalizedVectorsAreApproxParallel_butCanHeadToDifferntDirs_padding(direction_normalized, boxUp_normalized);
            Vector3 textUp = directionAndUp_areApproxParallel ? default(Vector3) : boxUp_normalized;
            UtilitiesDXXL_Text.WriteFramed(castText, textPos, textColor, textSize, direction_normalized, textUp, DrawText.TextAnchorDXXL.LowerLeft, DrawBasics.LineStyle.solid, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
        }

        static void WriteTextAtCapsuleCastOrigin(bool capsuleAppearsAsSphereAlongCastDir, Vector3 posOfCapsuleSphere1_atCastStart, Vector3 posOfCapsuleSphere2_atCastStart, Vector3 direction_normalized, float capsuleRadius, bool capsuleHasNegativeRadius, Color color, string nameText, int hitCount, float maxDistance, float durationInSec, bool hiddenByNearerObjects)
        {
            if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes) { return; }
            Vector3 posOfHigherCapsule = (posOfCapsuleSphere1_atCastStart.y > posOfCapsuleSphere2_atCastStart.y) ? posOfCapsuleSphere1_atCastStart : posOfCapsuleSphere2_atCastStart;
            Vector3 higherSphereUp = (posOfCapsuleSphere1_atCastStart.y > posOfCapsuleSphere2_atCastStart.y) ? (posOfCapsuleSphere1_atCastStart - posOfCapsuleSphere2_atCastStart) : (posOfCapsuleSphere2_atCastStart - posOfCapsuleSphere1_atCastStart);
            Vector3 higherSphereUp_normalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(higherSphereUp);
            if (capsuleAppearsAsSphereAlongCastDir)
            {
                higherSphereUp_normalized = UtilitiesDXXL_Math.Get_aNormalizedVector_perpToGivenVector(direction_normalized);
            }
            if (higherSphereUp_normalized.y < 0.0f) { higherSphereUp_normalized = -higherSphereUp_normalized; }
            float textSize = 0.2f * capsuleRadius;
            textSize = Mathf.Max(textSize, 0.02f);
            Color textColor = Color.Lerp(color, Color.black, 0.75f);
            string additinalWarningText = capsuleHasNegativeRadius ? "[<color=#e2aa00FF><icon=warning></color> negative capsule radius -> invalid results and/or hitting capsule will be <i>inside</i> other colliders]<br>" : null;
            string castText = GetVolumeCastStartTextString("<size=27>Capsulecast</size><br><size=5> </size><br>number of hits: ", nameText, hitCount, maxDistance, additinalWarningText);
            Vector3 textPos = posOfHigherCapsule + higherSphereUp_normalized * (capsuleRadius + 0.65f * textSize);
            UtilitiesDXXL_Text.WriteFramed(castText, textPos, textColor, textSize, direction_normalized, higherSphereUp_normalized, DrawText.TextAnchorDXXL.LowerLeft, DrawBasics.LineStyle.solid, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
        }

        static string GetVolumeCastStartTextString(string volumeCastTypeIdentifyingStringPart, string nameText, int hitCount, float maxDistance, string additinalWarningText)
        {
            additinalWarningText = UtilitiesDXXL_Math.ApproximatelyZero(maxDistance) ? "[<color=#e2aa00FF><icon=warning></color> cast distance is zero]<br>" + additinalWarningText : additinalWarningText;
            additinalWarningText = (maxDistance < 0.0f) ? "[<color=#e2aa00FF><icon=warning></color> negative cast direction -> no hits will be detected]<br>" + additinalWarningText : additinalWarningText;

            string startText;
            if (DrawPhysics.drawCastNameTag_atCastOrigin && nameText != null && nameText.Length != 0)
            {
                startText = (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) ? additinalWarningText + GetSizeMarkupStartStringForCastNames(nameText) + nameText + "</size><br><size=5> </size><br>hits: " + hitCount : additinalWarningText + GetSizeMarkupStartStringForCastNames(nameText) + nameText + "</size><br><size=5> </size><br>number of hits: " + hitCount;
            }
            else
            {
                startText = (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) ? additinalWarningText + "hits: " + hitCount : additinalWarningText + volumeCastTypeIdentifyingStringPart + hitCount;
            }

            return startText;
        }

        static void DrawSpherecastHitInfo(Vector3 castOrigin, Vector3 direction_normalized, float sphereRadius, bool sphereHasNegativeRadius, RaycastHit hitInfo, int i_hit, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.FloatIsInvalid(hitInfo.distance))
            {
                Debug.LogError("Draw XXL: A 'Physics.SphereCast()' returned an invalid hit distance of '" + hitInfo.distance + "'. The drawn cast visualization may be incorrect.");
            }
            else
            {
                if (UtilitiesDXXL_Math.VectorIsInvalid(hitInfo.point) || UtilitiesDXXL_Math.VectorIsInvalid(hitInfo.normal))
                {
                    Debug.LogError("Draw XXL: A 'Physics.SphereCast()' returned an invalid hit position (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo.point) + ") or normal (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo.normal) + "). The drawn cast visualization may be incorrect.");
                }
                else
                {
                    bool saveDrawnLines = i_hit >= DrawPhysics.hitResultsWithMoreDetailedDisplay;
                    if (DrawPhysics.visualizationQuality != DrawPhysics.VisualizationQuality.high_withFullDetails) { saveDrawnLines = true; }

                    DrawSphereAtHitPos(castOrigin, direction_normalized, hitInfo, i_hit, sphereRadius, durationInSec, hiddenByNearerObjects);
                    DrawNormalAtVolumeCastHitPos(saveDrawnLines, hitInfo, direction_normalized, durationInSec, hiddenByNearerObjects);
                    string text = GetTextAtHitPos_forVolumeCast("<sw=80000>Spherecast hit #", saveDrawnLines, hitInfo, i_hit, nameText);
                    string additionalWarningText = sphereHasNegativeRadius ? "<br>[<color=#e2aa00FF><icon=warning></color> negative sphere radius -> hitting sphere <i>inside</i> other collider]" : null;
                    DrawTextDescriptionAtVolumeCastHitPos(hitInfo, nameText, text, durationInSec, hiddenByNearerObjects, additionalWarningText);
                }
            }
        }

        static void DrawBoxcastHitInfo(Vector3 castOrigin, Vector3 direction_normalized, Vector3 boxSize, bool atLeastOneBoxDimIsNegative, Vector3 boxForward_normalized, Vector3 boxUp_normalized, RaycastHit hitInfo, int i_hit, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.FloatIsInvalid(hitInfo.distance))
            {
                Debug.LogError("Draw XXL: A 'Physics.BoxCast()' returned an invalid hit distance of '" + hitInfo.distance + "'. The drawn cast visualization may be incorrect.");
            }
            else
            {
                if (UtilitiesDXXL_Math.VectorIsInvalid(hitInfo.point) || UtilitiesDXXL_Math.VectorIsInvalid(hitInfo.normal))
                {
                    Debug.LogError("Draw XXL: A 'Physics.BoxCast()' returned an invalid hit position (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo.point) + ") or normal (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo.normal) + "). The drawn cast visualization may be incorrect.");
                }
                else
                {
                    bool saveDrawnLines = i_hit >= DrawPhysics.hitResultsWithMoreDetailedDisplay;
                    if (DrawPhysics.visualizationQuality != DrawPhysics.VisualizationQuality.high_withFullDetails) { saveDrawnLines = true; }

                    DrawBoxAtHitPos(castOrigin, direction_normalized, boxSize, boxForward_normalized, boxUp_normalized, hitInfo, durationInSec, hiddenByNearerObjects);
                    DrawNormalAtVolumeCastHitPos(saveDrawnLines, hitInfo, direction_normalized, durationInSec, hiddenByNearerObjects);
                    string text = GetTextAtHitPos_forVolumeCast("<sw=80000>Boxcast hit #", saveDrawnLines, hitInfo, i_hit, nameText);
                    string additionalWarningText = atLeastOneBoxDimIsNegative ? "<br>[<color=#e2aa00FF><icon=warning></color> box contains negative dimensions -> hitting box may be <i>inside</i> other collider]" : null;
                    DrawTextDescriptionAtVolumeCastHitPos(hitInfo, nameText, text, durationInSec, hiddenByNearerObjects, additionalWarningText);
                }
            }
        }

        static void DrawCapsulecastHitInfo(Vector3 posOfCapsuleSphere1_atCastStart, Vector3 posOfCapsuleSphere2_atCastStart, Vector3 direction_normalized, float capsuleRadius, bool capsuleHasNegativeRadius, RaycastHit hitInfo, int i_hit, string nameText, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Math.FloatIsInvalid(hitInfo.distance))
            {
                Debug.LogError("Draw XXL: A 'Physics.CapsuleCast()' returned an invalid hit distance of '" + hitInfo.distance + "'. The drawn cast visualization may be incorrect.");
            }
            else
            {
                if (UtilitiesDXXL_Math.VectorIsInvalid(hitInfo.point) || UtilitiesDXXL_Math.VectorIsInvalid(hitInfo.normal))
                {
                    Debug.LogError("Draw XXL: A 'Physics.CapsuleCast()' returned an invalid hit position (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo.point) + ") or normal (namely " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo.normal) + "). The drawn cast visualization may be incorrect.");
                }
                else
                {
                    bool saveDrawnLines = i_hit >= DrawPhysics.hitResultsWithMoreDetailedDisplay;
                    if (DrawPhysics.visualizationQuality != DrawPhysics.VisualizationQuality.high_withFullDetails) { saveDrawnLines = true; }

                    DrawCapsuleAtHitPos(posOfCapsuleSphere1_atCastStart, posOfCapsuleSphere2_atCastStart, direction_normalized, capsuleRadius, hitInfo, i_hit, durationInSec, hiddenByNearerObjects);
                    DrawNormalAtVolumeCastHitPos(saveDrawnLines, hitInfo, direction_normalized, durationInSec, hiddenByNearerObjects);
                    string text = GetTextAtHitPos_forVolumeCast("<sw=80000>Capsulecast hit #", saveDrawnLines, hitInfo, i_hit, nameText);
                    string additionalWarningText = capsuleHasNegativeRadius ? "<br>[<color=#e2aa00FF><icon=warning></color> negative capsule radius -> hitting capsule <i>inside</i> other collider]" : null;
                    DrawTextDescriptionAtVolumeCastHitPos(hitInfo, nameText, text, durationInSec, hiddenByNearerObjects, additionalWarningText);
                }
            }
        }

        public static void DrawSilhouettesAroundHitPos(float volumeSize, Vector3 origin, Vector3 direction_normalized, RaycastHit hitInfo, float maxDistance, int usedSlotsIn_verticesPerOutlineCircle, Color color, Color color_ofCastEnd, float distanceOfFarestHit, float durationInSec, bool hiddenByNearerObjects)
        {
            //"DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes" not defined yet
            if (UtilitiesDXXL_Math.FloatIsInvalid(hitInfo.distance))
            {
                Debug.LogError("Draw XXL: A 'Physics.Cast()' returned an invalid hit distance of '" + hitInfo.distance + "'. The drawn cast visualization may be incorrect.");
            }
            else
            {
                if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.high_withFullDetails)
                {
                    int silhouettesPerHitPos = 7;
                    float distanceBetweenSilhouettes = 0.75f * volumeSize;
                    float distanceOfFirstSilhouette = hitInfo.distance - distanceBetweenSilhouettes * 3;
                    for (int i_silhouette = 0; i_silhouette < silhouettesPerHitPos; i_silhouette++)
                    {
                        float distance_ofCurrSilhouette = distanceOfFirstSilhouette + distanceBetweenSilhouettes * i_silhouette;
                        if (distance_ofCurrSilhouette > 0.0f && distance_ofCurrSilhouette < maxDistance)
                        {
                            Vector3 posOfCurrSilhouetteOnRayLine = origin + distance_ofCurrSilhouette * direction_normalized;
                            bool isAfterLastHit = (distance_ofCurrSilhouette > distanceOfFarestHit);
                            for (int i_lineOfSilhouette = 0; i_lineOfSilhouette < usedSlotsIn_verticesPerOutlineCircle; i_lineOfSilhouette++)
                            {
                                Line_fadeableAnimSpeed.InternalDraw(posOfCurrSilhouetteOnRayLine + volumeCastOutlineVertices_local[i_lineOfSilhouette], posOfCurrSilhouetteOnRayLine + volumeCastOutlineVertices_local[UtilitiesDXXL_Math.LoopOvershootingIndexIntoCollectionSize(i_lineOfSilhouette + 1, usedSlotsIn_verticesPerOutlineCircle)], isAfterLastHit ? color_ofCastEnd : color, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                            }
                        }
                    }
                }
            }
        }

        static void DrawSphereAtHitPos(Vector3 castOrigin, Vector3 direction_normalized, RaycastHit hitInfo, int i_hit, float sphereRadius, float durationInSec, bool hiddenByNearerObjects)
        {
            int usedSphereStruts = strutsPerCastSphere;
            Vector3 hittingSpherePosition = castOrigin + direction_normalized * hitInfo.distance;

            if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes) { usedSphereStruts = 2; }
            if ((DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) && i_hit > 1) { usedSphereStruts = 6; }
            if ((DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) && i_hit > 3) { usedSphereStruts = 4; }
            if ((DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) && i_hit > 5) { usedSphereStruts = 2; }

            switch (DrawPhysics.visualizationQuality)
            {
                case DrawPhysics.VisualizationQuality.high_withFullDetails:
                    UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(64);
                    break;
                case DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes:
                    UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(32);
                    break;
                case DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes:
                    UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(16);
                    break;
                default:
                    UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(64);
                    break;
            }

            Color color_ofHittingVolume = Color.Lerp(DrawPhysics.colorForHittingCasts, Color.white, 0.6f);
            DrawShapes.Sphere(hittingSpherePosition, sphereRadius, color_ofHittingVolume, direction_normalized, default(Vector3), 0.0f, null, usedSphereStruts, false, DrawBasics.LineStyle.solid, 1.0f, false, false, durationInSec, hiddenByNearerObjects);

            UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
        }

        static void DrawBoxAtHitPos(Vector3 castOrigin, Vector3 direction_normalized, Vector3 boxSize, Vector3 boxForward, Vector3 boxUp, RaycastHit hitInfo, float durationInSec, bool hiddenByNearerObjects)
        {
            Vector3 hittingBoxPosition = castOrigin + direction_normalized * hitInfo.distance;
            Color color_ofHittingVolume_ofEdges = Color.Lerp(DrawPhysics.colorForHittingCasts, Color.white, 0.6f);
            Color color_ofHittingVolume_ofPlaneFillLines = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color_ofHittingVolume_ofEdges, 0.4f); //-> This helps to distinguish boxes that intersect each other.

            if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                DrawShapes.Cube(hittingBoxPosition, boxSize, color_ofHittingVolume_ofPlaneFillLines, boxUp, boxForward, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                DrawShapes.CubeFilled(hittingBoxPosition, boxSize, color_ofHittingVolume_ofPlaneFillLines, boxUp, boxForward, 0.0f, 4, null, DrawBasics.LineStyle.solid, color_ofHittingVolume_ofEdges, 0.0f, 1.0f, true, false, durationInSec, hiddenByNearerObjects);
            }
        }

        static void DrawCapsuleAtHitPos(Vector3 posOfCapsuleSphere1_atCastStart, Vector3 posOfCapsuleSphere2_atCastStart, Vector3 direction_normalized, float capsuleRadius, RaycastHit hitInfo, int i_hit, float durationInSec, bool hiddenByNearerObjects)
        {
            int usedCapsuleStruts = strutsPerCastCapsule;
            Vector3 hittingCapsuleSphere1Position = posOfCapsuleSphere1_atCastStart + direction_normalized * hitInfo.distance;
            Vector3 hittingCapsuleSphere2Position = posOfCapsuleSphere2_atCastStart + direction_normalized * hitInfo.distance;

            if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes) { usedCapsuleStruts = 2; }
            if ((DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) && i_hit > 1) { usedCapsuleStruts = 6; }
            if ((DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) && i_hit > 3) { usedCapsuleStruts = 4; }
            if ((DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) && i_hit > 5) { usedCapsuleStruts = 2; }

            switch (DrawPhysics.visualizationQuality)
            {
                case DrawPhysics.VisualizationQuality.high_withFullDetails:
                    UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(64);
                    break;
                case DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes:
                    UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(32);
                    break;
                case DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes:
                    UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(16);
                    break;
                default:
                    UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(64);
                    break;
            }

            Color color_ofHittingVolume = Color.Lerp(DrawPhysics.colorForHittingCasts, Color.white, 0.6f);
            DrawShapes.Capsule(hittingCapsuleSphere1Position, hittingCapsuleSphere2Position, capsuleRadius, color_ofHittingVolume, direction_normalized, 0.0f, null, usedCapsuleStruts, false, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);

            UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
        }

        static void DrawNormalAtVolumeCastHitPos(bool saveDrawnLines, RaycastHit hitInfo, Vector3 direction_normalized, float durationInSec, bool hiddenByNearerObjects)
        {
            Color color_ofNormal = Get_color_ofNormal();
            if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                DrawBasics.LineFrom(hitInfo.point, hitInfo.normal, color_ofNormal, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, default(Vector3), false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
            }
            else
            {
                float relConeLength_ofNormalVector = 0.17f;
                string normalText = (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) ? null : (saveDrawnLines ? "normal" : "normal<br><size=4>of hit surface</size>");

                UtilitiesDXXL_DrawBasics.Set_coneLength_interpretation_forStraightVectors_reversible(DrawBasics.LengthInterpretation.relativeToLineLength);
                DrawBasics.VectorFrom(hitInfo.point, hitInfo.normal, color_ofNormal, saveDrawnLines ? 0.0f : 0.006f, normalText, relConeLength_ofNormalVector, false, false, default(Vector3), false, 0.01f, false, 0.0f, durationInSec, hiddenByNearerObjects);
                UtilitiesDXXL_DrawBasics.Reverse_coneLength_interpretation_forStraightVectors();
            }

            //normal socket:
            switch (DrawPhysics.visualizationQuality)
            {
                case DrawPhysics.VisualizationQuality.high_withFullDetails:
                    DrawShapes.Decagon(hitInfo.point, 0.02f, color_ofNormal, hitInfo.normal, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
                    DrawShapes.Decagon(hitInfo.point, 0.04f, color_ofNormal, hitInfo.normal, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
                    DrawShapes.Decagon(hitInfo.point, 0.06f, color_ofNormal, hitInfo.normal, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
                    break;
                case DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes:
                    DrawShapes.Decagon(hitInfo.point, 0.06f, color_ofNormal, hitInfo.normal, default, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
                    break;
                case DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes:
                    DrawShapes.Square(hitInfo.point, 0.12f, color_ofNormal, hitInfo.normal, direction_normalized, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, DrawBasics.LineStyle.invisible, false, false, durationInSec, hiddenByNearerObjects);
                    break;
                default:
                    break;
            }

        }

        static Color Get_color_ofNormal()
        {
            if (UtilitiesDXXL_Colors.IsDefaultColor(DrawPhysics.overwriteColorForCastsHitNormals))
            {
                return Get_defaultColor_ofNormal();
            }
            else
            {
                return DrawPhysics.overwriteColorForCastsHitNormals;
            }
        }

        public static Color Get_defaultColor_ofNormal()
        {
            return ((DrawPhysics.colorForHittingCasts.grayscale < 0.175f) ? Color.Lerp(DrawPhysics.colorForHittingCasts, Color.white, 0.7f) : Color.Lerp(DrawPhysics.colorForHittingCasts, Color.black, 0.7f));
        }

        static string GetTextAtHitPos_forVolumeCast(string volumeCastTypeSpecifyingStringPart, bool saveDrawnLines, RaycastHit hitInfo, int i_hit, string nameText)
        {
            if (DrawPhysics.drawCastNameTag_atHitPositions && nameText != null && nameText.Length != 0)
            {
                return (saveDrawnLines ? (nameText + " / #" + i_hit + ":<br>hit GO: " + hitInfo.transform.gameObject.name + "<br>pos = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo.point) + "<br>dist = " + hitInfo.distance) : (GetStrokeWidthMarkupStartStringForHitPosDesctiptionHeaders(nameText) + nameText + " / hit #" + i_hit + ":</sw><br>GameObject that was hit: " + hitInfo.transform.gameObject.name + "<br>hit pos = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo.point) + "<br>distance = " + hitInfo.distance));
            }
            else
            {
                return (saveDrawnLines ? ("hit #" + i_hit + ":<br>hit GO: " + hitInfo.transform.gameObject.name + "<br>pos = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo.point) + "<br>dist = " + hitInfo.distance) : (volumeCastTypeSpecifyingStringPart + i_hit + ":</sw><br>GameObject that was hit: " + hitInfo.transform.gameObject.name + "<br>hit pos = " + UtilitiesDXXL_Log.Get_vectorComponentsAsString(hitInfo.point) + "<br>distance = " + hitInfo.distance));
            }
        }

        static void DrawTextDescriptionAtVolumeCastHitPos(RaycastHit hitInfo, string nameText, string text, float durationInSec, bool hiddenByNearerObjects, string additionalWarningText)
        {
            if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes) { return; }
            if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes)
            {
                if (DrawPhysics.drawCastNameTag_atHitPositions && nameText != null && nameText.Length != 0)
                {
                    UtilitiesDXXL_Text.WriteFramed(nameText + additionalWarningText, hitInfo.point, DrawPhysics.colorForCastsHitText, 0.1f * DrawPhysics.scaleFactor_forCastHitTextSize, default(Vector3), default(Vector3), DrawText.TextAnchorDXXL.LowerLeftOfFirstLine, DrawBasics.LineStyle.invisible, 0.0f, 0.0f, 0.0f, 0.0f, 0.0f, true, durationInSec, hiddenByNearerObjects);
                }
            }
            else
            {
                Vector3 textOffsetDir = default(Vector3);
                float textOffsetDistance = DrawPhysics.scaleFactor_forCastHitTextSize;

                TrySet_default_textOffsetDirection_forPointTags_reversible();
                DrawBasics.PointTag(hitInfo.point, text + additionalWarningText, DrawPhysics.colorForCastsHitText, 0.0f, textOffsetDistance, textOffsetDir, 1.0f, false, durationInSec, hiddenByNearerObjects);
                TryReverse_default_textOffsetDirection_forPointTags();
            }
        }

        static float GetDistanceOfSingleHit(bool hasHit, RaycastHit hitInfo)
        {
            if (hasHit)
            {
                if (UtilitiesDXXL_Math.FloatIsInvalid(hitInfo.distance))
                {
                    Debug.LogError("Draw XXL: A 'Physics.Cast()' returned an invalid hit distance of '" + hitInfo.distance + "'. The drawn cast visualization may be incorrect.");
                    return 1.0f;
                }
                else
                {
                    return hitInfo.distance;
                }
            }
            else
            {
                return 0.0f;
            }
        }

        static float GetDistanceOfFarestHit(RaycastHit[] hitInfos, int numberOfUsedSlotsInHitInfoArray)
        {
            float farestDistance = 0.0f;
            if (hitInfos != null)
            {
                numberOfUsedSlotsInHitInfoArray = Mathf.Min(numberOfUsedSlotsInHitInfoArray, hitInfos.Length);
                for (int i = 0; i < numberOfUsedSlotsInHitInfoArray; i++)
                {
                    if (UtilitiesDXXL_Math.FloatIsInvalid(hitInfos[i].distance))
                    {
                        Debug.LogError("Draw XXL: A 'Physics.Cast()' returned an invalid hit distance of '" + hitInfos[i].distance + "'. The drawn cast visualization may be incorrect.");
                    }
                    else
                    {
                        farestDistance = Mathf.Max(farestDistance, hitInfos[i].distance);
                    }
                }
            }
            return farestDistance;
        }

        public static void DrawCheckedBox(bool doesOverlap, Vector3 center, Vector3 halfExtents, Quaternion orientation, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(halfExtents, "halfExtents")) { return; }

            Color color = doesOverlap ? DrawPhysics.colorForHittingCasts : DrawPhysics.colorForNonHittingCasts;
            Color color_lowAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.3f);
            bool atLeastOneBoxDimIsNegative = UtilitiesDXXL_Math.ContainsNegativeComponents(halfExtents);
            Vector3 boxSize = 2.0f * halfExtents; //Note: inconsistent naming inside Unity: Here: "halfExtents" = "halfSize", while in "Bounds": "extents" = "halfSize"

            string text = null;
            if (DrawPhysics.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                string additinalWarningText = atLeastOneBoxDimIsNegative ? "[<color=#e2aa00FF><icon=warning></color> box contains negative dimensions -> invalid results and/or box must be <i>inside</i> other colliders]<br>" : null;
                text = additinalWarningText + GetTextForVolumeCheck("Box: Check collider intersections<br>Result: ", doesOverlap, nameTag);
            }

            UtilitiesDXXL_Shapes.Set_bothForcedConstantTextSizes_forTextAtShapes_reversible(DrawPhysics.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts, DrawPhysics.forcedConstantWorldspaceTextSize_forOverlapResultTexts);

            switch (DrawPhysics.visualizationQuality)
            {
                case DrawPhysics.VisualizationQuality.high_withFullDetails:
                    DrawShapes.CubeFilled(center, boxSize, color_lowAlpha, orientation, 0.0f, 4, text, DrawBasics.LineStyle.solid, color, 0.0f, 1.0f, true, true, durationInSec, hiddenByNearerObjects);
                    break;
                case DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes:
                    DrawShapes.CubeFilled(center, boxSize, color_lowAlpha, orientation, 0.0f, 4, text, DrawBasics.LineStyle.solid, color, 0.0f, 1.0f, true, true, durationInSec, hiddenByNearerObjects);
                    break;
                case DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes:
                    DrawShapes.Cube(center, boxSize, color_lowAlpha, orientation, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
                    break;
                default:
                    break;
            }

            UtilitiesDXXL_Shapes.Reverse_disable_bothForcedConstantTextSizes_forTextAtShapes();
        }

        public static void DrawCheckedCapsule(bool doesOverlap, Vector3 start, Vector3 end, float radius, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(end, "end")) { return; }

            Color color = doesOverlap ? DrawPhysics.colorForHittingCasts : DrawPhysics.colorForNonHittingCasts;

            string text = null;
            if (DrawPhysics.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                string additinalWarningText = (radius < 0.0f) ? "[<color=#e2aa00FF><icon=warning></color> negative capsule radius -> invalid results and/or capsule must be <i>inside</i> other colliders]<br>" : null;
                text = additinalWarningText + GetTextForVolumeCheck("Capsule: Check collider intersections<br>Result: ", doesOverlap, nameTag);
            }

            if (Mathf.Abs(radius) < 0.0001f && UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(start, end))
            {
                DrawBasics.PointTag(start, text, color, 0.0f, 1.0f, default(Vector3), 1.0f, false, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                UtilitiesDXXL_Shapes.Set_bothForcedConstantTextSizes_forTextAtShapes_reversible(DrawPhysics.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts, DrawPhysics.forcedConstantWorldspaceTextSize_forOverlapResultTexts);

                switch (DrawPhysics.visualizationQuality)
                {
                    case DrawPhysics.VisualizationQuality.high_withFullDetails:
                        UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(64);
                        DrawShapes.Capsule(start, end, radius, color, default(Vector3), 0.0f, text, strutsPerCastCapsule, false, DrawBasics.LineStyle.solid, 1.0f, true, durationInSec, hiddenByNearerObjects);
                        UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                        break;
                    case DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes:
                        UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(32);
                        DrawShapes.Capsule(start, end, radius, color, default(Vector3), 0.0f, text, 4, false, DrawBasics.LineStyle.solid, 1.0f, true, durationInSec, hiddenByNearerObjects);
                        UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                        break;
                    case DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes:
                        UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(16);
                        DrawShapes.Capsule(start, end, radius, color, default(Vector3), 0.0f, text, 2, false, DrawBasics.LineStyle.solid, 1.0f, true, durationInSec, hiddenByNearerObjects);
                        UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                        break;
                    default:
                        break;
                }

                UtilitiesDXXL_Shapes.Reverse_disable_bothForcedConstantTextSizes_forTextAtShapes();
            }
        }

        public static void DrawCheckedSphere(bool doesOverlap, Vector3 position, float radius, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            Color color = doesOverlap ? DrawPhysics.colorForHittingCasts : DrawPhysics.colorForNonHittingCasts;

            string text = null;
            if (DrawPhysics.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                string additinalWarningText = (radius < 0.0f) ? "[<color=#e2aa00FF><icon=warning></color> negative sphere radius -> invalid results and/or sphere must be <i>inside</i> other colliders]<br>" : null;
                text = additinalWarningText + GetTextForVolumeCheck("Sphere: Check collider intersections<br>Result: ", doesOverlap, nameTag);
            }

            if (Mathf.Abs(radius) < 0.0001f)
            {
                DrawBasics.PointTag(position, text, color, 0.0f, 1.0f, default(Vector3), 1.0f, false, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                UtilitiesDXXL_Shapes.Set_bothForcedConstantTextSizes_forTextAtShapes_reversible(DrawPhysics.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts, DrawPhysics.forcedConstantWorldspaceTextSize_forOverlapResultTexts);

                switch (DrawPhysics.visualizationQuality)
                {
                    case DrawPhysics.VisualizationQuality.high_withFullDetails:
                        UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(64);
                        DrawShapes.Sphere(position, radius, color, Vector3.up, Vector3.forward, 0.0f, text, strutsPerCastSphere, false, DrawBasics.LineStyle.solid, 1.0f, false, true, durationInSec, hiddenByNearerObjects);
                        UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                        break;
                    case DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes:
                        UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(32);
                        DrawShapes.Sphere(position, radius, color, Vector3.up, Vector3.forward, 0.0f, text, 4, false, DrawBasics.LineStyle.solid, 1.0f, false, true, durationInSec, hiddenByNearerObjects);
                        UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                        break;
                    case DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes:
                        UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(16);
                        DrawShapes.Sphere(position, radius, color, Vector3.up, Vector3.forward, 0.0f, text, 2, false, DrawBasics.LineStyle.solid, 1.0f, false, true, durationInSec, hiddenByNearerObjects);
                        UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                        break;
                    default:
                        break;
                }

                UtilitiesDXXL_Shapes.Reverse_disable_bothForcedConstantTextSizes_forTextAtShapes();
            }
        }

        static string GetTextForVolumeCheck(string volumeTypeIdentifyingStringPart, bool doesOverlap, string nameTag)
        {
            if (nameTag != null && nameTag.Length != 0)
            {
                if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes)
                {
                    return nameTag;
                }
                else
                {
                    return (nameTag + "<br>Result: " + (doesOverlap ? "Is overlapping with at least one collider" : "Is not overlapping with any collider"));
                }
            }
            else
            {
                if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes)
                {
                    return null;
                }
                else
                {
                    return (volumeTypeIdentifyingStringPart + (doesOverlap ? "Is overlapping with at least one collider" : "Is not overlapping with any collider"));
                }
            }
        }

        public static void DrawOverlapResultBox(bool doesOverlap, int numberOfOverlappingColliders, Collider[] overlappingColliders, Vector3 center, Vector3 halfExtents, Quaternion orientation, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            Color color = doesOverlap ? DrawPhysics.colorForHittingCasts : DrawPhysics.colorForNonHittingCasts;
            Color color_lowAlpha = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.3f);
            bool atLeastOneBoxDimIsNegative = UtilitiesDXXL_Math.ContainsNegativeComponents(halfExtents);
            Vector3 boxSize = 2.0f * halfExtents; //Note: inconsistent naming inside Unity: Here: "halfExtents" = "halfSize", while in "Bounds": "extents" = "halfSize"

            string text = null;
            if (DrawPhysics.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                string additinalWarningText = atLeastOneBoxDimIsNegative ? "[<color=#e2aa00FF><icon=warning></color> box contains negative dimensions -> invalid results and/or box must be <i>inside</i> other colliders]<br>" : null;
                string overlappingCollidersListAsText = GetOverlappingCollidersListAsText(overlappingColliders, numberOfOverlappingColliders);
                text = additinalWarningText + GetTextForVolumeOverlapCheck("Box: Overlap check", doesOverlap, numberOfOverlappingColliders, nameTag, overlappingCollidersListAsText);
            }

            UtilitiesDXXL_Shapes.Set_bothForcedConstantTextSizes_forTextAtShapes_reversible(DrawPhysics.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts, DrawPhysics.forcedConstantWorldspaceTextSize_forOverlapResultTexts);

            switch (DrawPhysics.visualizationQuality)
            {
                case DrawPhysics.VisualizationQuality.high_withFullDetails:
                    DrawShapes.CubeFilled(center, boxSize, color_lowAlpha, orientation, 0.0f, 4, text, DrawBasics.LineStyle.solid, color, 0.0f, 1.0f, true, true, durationInSec, hiddenByNearerObjects);
                    break;
                case DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes:
                    DrawShapes.CubeFilled(center, boxSize, color_lowAlpha, orientation, 0.0f, 4, text, DrawBasics.LineStyle.solid, color, 0.0f, 1.0f, true, true, durationInSec, hiddenByNearerObjects);
                    break;
                case DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes:
                    DrawShapes.Cube(center, boxSize, color_lowAlpha, orientation, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, false, durationInSec, hiddenByNearerObjects);
                    break;
                default:
                    break;
            }

            UtilitiesDXXL_Shapes.Reverse_disable_bothForcedConstantTextSizes_forTextAtShapes();
        }

        public static void DrawOverlapResultCapsule(bool doesOverlap, int numberOfOverlappingColliders, Collider[] overlappingColliders, Vector3 point0, Vector3 point1, float radius, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            Color color = doesOverlap ? DrawPhysics.colorForHittingCasts : DrawPhysics.colorForNonHittingCasts;

            string text = null;
            if (DrawPhysics.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                string additinalWarningText = (radius < 0.0f) ? "[<color=#e2aa00FF><icon=warning></color> negative capsule radius -> invalid results and/or capsule must be <i>inside</i> other colliders]<br>" : null;
                string overlappingCollidersListAsText = GetOverlappingCollidersListAsText(overlappingColliders, numberOfOverlappingColliders);
                text = additinalWarningText + GetTextForVolumeOverlapCheck("Capsule: Overlap check", doesOverlap, numberOfOverlappingColliders, nameTag, overlappingCollidersListAsText);
            }

            if (Mathf.Abs(radius) < 0.0001f && UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(point0, point1))
            {
                DrawBasics.PointTag(point0, text, color, 0.0f, 1.0f, default(Vector3), 1.0f, false, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                UtilitiesDXXL_Shapes.Set_bothForcedConstantTextSizes_forTextAtShapes_reversible(DrawPhysics.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts, DrawPhysics.forcedConstantWorldspaceTextSize_forOverlapResultTexts);

                switch (DrawPhysics.visualizationQuality)
                {
                    case DrawPhysics.VisualizationQuality.high_withFullDetails:
                        UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(64);
                        DrawShapes.Capsule(point0, point1, radius, color, default(Vector3), 0.0f, text, strutsPerCastCapsule, false, DrawBasics.LineStyle.solid, 1.0f, true, durationInSec, hiddenByNearerObjects);
                        UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                        break;
                    case DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes:
                        UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(32);
                        DrawShapes.Capsule(point0, point1, radius, color, default(Vector3), 0.0f, text, 4, false, DrawBasics.LineStyle.solid, 1.0f, true, durationInSec, hiddenByNearerObjects);
                        UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                        break;
                    case DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes:
                        UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(16);
                        DrawShapes.Capsule(point0, point1, radius, color, default(Vector3), 0.0f, text, 2, false, DrawBasics.LineStyle.solid, 1.0f, true, durationInSec, hiddenByNearerObjects);
                        UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                        break;
                    default:
                        break;
                }

                UtilitiesDXXL_Shapes.Reverse_disable_bothForcedConstantTextSizes_forTextAtShapes();
            }
        }

        public static void DrawOverlapResultSphere(bool doesOverlap, int numberOfOverlappingColliders, Collider[] overlappingColliders, Vector3 position, float radius, string nameTag, float durationInSec, bool hiddenByNearerObjects)
        {
            Color color = doesOverlap ? DrawPhysics.colorForHittingCasts : DrawPhysics.colorForNonHittingCasts;

            string text = null;
            if (DrawPhysics.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
            {
                string additinalWarningText = (radius < 0.0f) ? "[<color=#e2aa00FF><icon=warning></color> negative sphere radius -> invalid results and/or sphere must be <i>inside</i> other colliders]<br>" : null;
                string overlappingCollidersListAsText = GetOverlappingCollidersListAsText(overlappingColliders, numberOfOverlappingColliders);
                text = additinalWarningText + GetTextForVolumeOverlapCheck("Sphere: Overlap check", doesOverlap, numberOfOverlappingColliders, nameTag, overlappingCollidersListAsText);
            }

            if (Mathf.Abs(radius) < 0.0001f)
            {
                DrawBasics.PointTag(position, text, color, 0.0f, 1.0f, default(Vector3), 1.0f, false, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                UtilitiesDXXL_Shapes.Set_bothForcedConstantTextSizes_forTextAtShapes_reversible(DrawPhysics.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts, DrawPhysics.forcedConstantWorldspaceTextSize_forOverlapResultTexts);

                switch (DrawPhysics.visualizationQuality)
                {
                    case DrawPhysics.VisualizationQuality.high_withFullDetails:
                        UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(64);
                        DrawShapes.Sphere(position, radius, color, Vector3.up, Vector3.forward, 0.0f, text, strutsPerCastSphere, false, DrawBasics.LineStyle.solid, 1.0f, false, true, durationInSec, hiddenByNearerObjects);
                        UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                        break;
                    case DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes:
                        UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(32);
                        DrawShapes.Sphere(position, radius, color, Vector3.up, Vector3.forward, 0.0f, text, 4, false, DrawBasics.LineStyle.solid, 1.0f, false, true, durationInSec, hiddenByNearerObjects);
                        UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                        break;
                    case DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes:
                        UtilitiesDXXL_Shapes.Set_linesPerSphereCircle_reversible(16);
                        DrawShapes.Sphere(position, radius, color, Vector3.up, Vector3.forward, 0.0f, text, 2, false, DrawBasics.LineStyle.solid, 1.0f, false, true, durationInSec, hiddenByNearerObjects);
                        UtilitiesDXXL_Shapes.Reverse_linesPerSphereCircle();
                        break;
                    default:
                        break;
                }

                UtilitiesDXXL_Shapes.Reverse_disable_bothForcedConstantTextSizes_forTextAtShapes();
            }
        }

        static string GetOverlappingCollidersListAsText(Collider[] overlappingColliders, int numberOfOverlappingColliders)
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
                            if (i < DrawPhysics.MaxListedColliders_inOverlapVolumesTextList)
                            {
                                //collidersList = overlappingColliders[i].GetType().ToString() + " (on GameObject '" + overlappingColliders[i].gameObject.name + "')<br>" + collidersList; //-> first found collider is on bottom. This contradicts the "DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes"-hitPos-display, which only displays the index-number
                                collidersList = collidersList + "<br>" + overlappingColliders[i].GetType().ToString() + " (on GameObject '" + overlappingColliders[i].gameObject.name + "')"; //-> first found collider is on top. This corresponds to the "DrawPhysics2D.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes"-hitPos-display, which only, which only displays the index-number
                            }
                            else
                            {
                                collidersList = collidersList + "<br>...and " + (numberOfOverlappingColliders - DrawPhysics.MaxListedColliders_inOverlapVolumesTextList) + " more.";
                                break;
                            }
                        }
                        return collidersList;
                    }
                }
            }
        }

        static string GetTextForVolumeOverlapCheck(string volumeTypeIdentifyingStringPart, bool doesOverlap, int numberOfOverlappingColliders, string nameTag, string overlappingCollidersListAsText)
        {
            if (nameTag != null && nameTag.Length != 0)
            {
                //has user specified "nameTag":
                if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes)
                {
                    if (doesOverlap)
                    {
                        return (nameTag + "<br>Overlapping with these " + numberOfOverlappingColliders + " collider(s):<br>" + overlappingCollidersListAsText);
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
                if (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes)
                {
                    if (doesOverlap)
                    {
                        return ("Overlapping with these " + numberOfOverlappingColliders + " collider(s):<br>" + overlappingCollidersListAsText);
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

        public static void DrawMarkersAtOverlappingColliders(Vector3 volumeCenter, bool doesOverlap, Collider[] overlappingColliders, int numberOfOverlappingColliders, float approxSize_ofOverlapVolume, float durationInSec, bool hiddenByNearerObjects)
        {
            if (doesOverlap)
            {
                Color color_ofMarkers = (DrawPhysics.colorForHittingCasts.grayscale < 0.175f) ? Color.Lerp(DrawPhysics.colorForHittingCasts, Color.white, 0.7f) : Color.Lerp(DrawPhysics.colorForHittingCasts, Color.black, 0.7f);
                Color color_ofMarkerExtentionLinesToShapeCenter = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(DrawPhysics.colorForHittingCasts, 0.1f);
                float relTextSizeScaling = 1.0f;
                float textOffsetDistance = 1.0f;
                float sizeOfMarkingCross = Mathf.Max(0.1f * approxSize_ofOverlapVolume, 0.001f);
                numberOfOverlappingColliders = Mathf.Min(numberOfOverlappingColliders, overlappingColliders.Length);
                for (int i = 0; i < numberOfOverlappingColliders; i++)
                {
                    Vector3 nearestPosOnCollider = overlappingColliders[i].ClosestPoint(volumeCenter);
                    Line_fadeableAnimSpeed.InternalDraw(volumeCenter, nearestPosOnCollider, color_ofMarkerExtentionLinesToShapeCenter, 0.0f, null, DrawBasics.LineStyle.solid, 1.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                    bool drawCoordsAsText = (DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.high_withFullDetails);
                    DrawBasics.Point(nearestPosOnCollider, DrawPhysics.colorForHittingCasts, sizeOfMarkingCross, default(Quaternion), 0.0f, null, DrawPhysics.colorForHittingCasts, false, drawCoordsAsText, false, durationInSec, hiddenByNearerObjects);

                    if (DrawPhysics.visualizationQuality != DrawPhysics.VisualizationQuality.low_withoutAnyTextOrSilhouettes)
                    {
                        string text;
                        if ((DrawPhysics.visualizationQuality == DrawPhysics.VisualizationQuality.medium_meaningReducedTextAndSilhouettes) || (i >= DrawPhysics.maxOverlapingCollidersWithUntruncatedText))
                        {
                            text = "" + i;
                        }
                        else
                        {
                            text = overlappingColliders[i].GetType().ToString() + " (on GameObject '" + overlappingColliders[i].gameObject.name + "')";
                        }
                        Vector3 textOffsetDir = nearestPosOnCollider - volumeCenter;
                        DrawBasics.PointTag(nearestPosOnCollider, text, color_ofMarkers, 0.0f, textOffsetDistance, textOffsetDir, relTextSizeScaling, true, durationInSec, hiddenByNearerObjects);
                    }
                }
            }
        }

        public static bool ExtentNameTagForNonSuitingResultArray(ref string nameTag, int numberOfUsedSlotsInHitInfoArray, RaycastHit[] resultsArray)
        {
            bool resultsArrayIsNull = (resultsArray == null);
            ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoArray, resultsArrayIsNull, resultsArrayIsNull ? 0 : resultsArray.Length);
            return resultsArrayIsNull;
        }

        public static bool ExtentNameTagForNonSuitingResultArray(ref string nameTag, int numberOfOverlappingColliders, Collider[] resultsArray)
        {
            bool resultsArrayIsNull = (resultsArray == null);
            ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfOverlappingColliders, resultsArrayIsNull, resultsArrayIsNull ? 0 : resultsArray.Length);
            return resultsArrayIsNull;
        }

        public static void ExtentNameTagForNonSuitingResultArray(ref string nameTag, int numberOfUsedSlotsInResultsArray, bool resultsArrayIsNull, int length_ofResultsArray)
        {
            if (resultsArrayIsNull)
            {
                nameTag = "[<color=#ce0e0eFF><icon=logMessageError></color> results buffer array is null]<br>" + nameTag;
            }
            else
            {
                if (length_ofResultsArray == 0)
                {
                    nameTag = "[<color=#e2aa00FF><icon=warning></color> results buffer array has 0 slots -> no ray results possible]<br>" + nameTag;
                }
                else
                {
                    if (numberOfUsedSlotsInResultsArray >= length_ofResultsArray)
                    {
                        //note: the "int numberOfCollisions" return value of Unitys physics cast is already limited to "resultsArray.Length" -> no outOfBounds-Checks necessary
                        nameTag = "[<color=#e2aa00FF><icon=warning></color> results buffer array has all " + length_ofResultsArray + " slots filled -> further results potentially missing]<br>" + nameTag;
                    }
                }
            }
        }

        public static void ExtentNameTagForNonSuitingResultList(ref string nameTag, bool resultsListIsNull, int numberOfUsedSlotsInList)
        {
            if (resultsListIsNull)
            {
                nameTag = "[<color=#ce0e0eFF><icon=logMessageError></color> results buffer list is null]<br>" + nameTag;
            }
            else
            {
                if (numberOfUsedSlotsInList > DrawPhysics2D.MaxNumberOfPreallocatedHits)
                {
                    nameTag = "[<color=#e2aa00FF><icon=warning></color> Only " + DrawPhysics2D.MaxNumberOfPreallocatedHits + " of the " + numberOfUsedSlotsInList + " hit results are displayed<br>-> Increase 'DrawXXL.DrawPhysics2D.MaxNumberOfPreallocatedHits' to see all hits]<br>" + nameTag;
                }
            }
        }

        public static string GetSizeMarkupStartStringForCastNames(string nameText)
        {
            if (nameText.Length < 25)
            {
                return "<size=27>";
            }
            else
            {
                //additionalWarningMessages get encoded into the "nameText", which would lead to very big text walls, if the would be magnified to "27"
                return "<size=11>";
            }
        }

        public static string GetStrokeWidthMarkupStartStringForHitPosDesctiptionHeaders(string nameText)
        {
            if (nameText.Length < 25)
            {
                return "<sw=80000>";
            }
            else
            {
                //additionalWarningMessages get encoded into the "nameText", which would lead to many strokeWidth duplicate lines, if a stroke width of non-0 would be used.
                return "<sw=0>";
            }
        }

        static float scaleFactor_forCastHitTextSize_before;
        public static void Set_scaleFactor_forCastHitTextSize_reversible(float new_scaleFactor_forCastHitTextSize)
        {
            scaleFactor_forCastHitTextSize_before = DrawPhysics.scaleFactor_forCastHitTextSize;
            DrawPhysics.scaleFactor_forCastHitTextSize = new_scaleFactor_forCastHitTextSize;
        }
        public static void Reverse_scaleFactor_forCastHitTextSize()
        {
            DrawPhysics.scaleFactor_forCastHitTextSize = scaleFactor_forCastHitTextSize_before;
        }

        static float castSilhouetteVisualizerDensity_before;
        public static void Set_castSilhouetteVisualizerDensity_reversible(float new_castSilhouetteVisualizerDensity)
        {
            castSilhouetteVisualizerDensity_before = DrawPhysics.castSilhouetteVisualizerDensity;
            DrawPhysics.castSilhouetteVisualizerDensity = new_castSilhouetteVisualizerDensity;
        }
        public static void Reverse_castSilhouetteVisualizerDensity()
        {
            DrawPhysics.castSilhouetteVisualizerDensity = castSilhouetteVisualizerDensity_before;
        }

        static DrawPhysics.VisualizationQuality visualizationQuality_before;
        public static void Set_visualizationQuality_reversible(DrawPhysics.VisualizationQuality new_visualizationQuality)
        {
            visualizationQuality_before = DrawPhysics.visualizationQuality;
            DrawPhysics.visualizationQuality = new_visualizationQuality;
        }
        public static void Reverse_visualizationQuality()
        {
            DrawPhysics.visualizationQuality = visualizationQuality_before;
        }

        static float forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts_before;
        public static void Set_forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts_reversible(float new_forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts)
        {
            forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts_before = DrawPhysics.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts;
            DrawPhysics.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts = new_forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts;
        }
        public static void Reverse_forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts()
        {
            DrawPhysics.forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts = forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts_before;
        }

        static float forcedConstantWorldspaceTextSize_forOverlapResultTexts_before;
        public static void Set_forcedConstantWorldspaceTextSize_forOverlapResultTexts_reversible(float new_forcedConstantWorldspaceTextSize_forOverlapResultTexts)
        {
            forcedConstantWorldspaceTextSize_forOverlapResultTexts_before = DrawPhysics.forcedConstantWorldspaceTextSize_forOverlapResultTexts;
            DrawPhysics.forcedConstantWorldspaceTextSize_forOverlapResultTexts = new_forcedConstantWorldspaceTextSize_forOverlapResultTexts;
        }
        public static void Reverse_forcedConstantWorldspaceTextSize_forOverlapResultTexts()
        {
            DrawPhysics.forcedConstantWorldspaceTextSize_forOverlapResultTexts = forcedConstantWorldspaceTextSize_forOverlapResultTexts_before;
        }

        static Vector2 directionOfHitResultText_before;
        public static void Set_directionOfHitResultText_reversible(Vector2 new_directionOfHitResultText)
        {
            directionOfHitResultText_before = DrawPhysics.directionOfHitResultText;
            DrawPhysics.directionOfHitResultText = new_directionOfHitResultText;
        }
        public static void Reverse_directionOfHitResultText()
        {
            DrawPhysics.directionOfHitResultText = directionOfHitResultText_before;
        }

        static Vector3 default_textOffsetDirection_forPointTags_before;
        public static void TrySet_default_textOffsetDirection_forPointTags_reversible()
        {
            if (UtilitiesDXXL_Math.IsDefaultVector(DrawPhysics.directionOfHitResultText) == false)
            {
                default_textOffsetDirection_forPointTags_before = DrawBasics.Default_textOffsetDirection_forPointTags;
                DrawBasics.Default_textOffsetDirection_forPointTags = new Vector3(DrawPhysics.directionOfHitResultText.x, DrawPhysics.directionOfHitResultText.y, 0.0f);
            }
        }
        public static void TryReverse_default_textOffsetDirection_forPointTags()
        {
            if (UtilitiesDXXL_Math.IsDefaultVector(DrawPhysics.directionOfHitResultText) == false)
            {
                DrawBasics.Default_textOffsetDirection_forPointTags = default_textOffsetDirection_forPointTags_before;
            }
        }

    }

}
