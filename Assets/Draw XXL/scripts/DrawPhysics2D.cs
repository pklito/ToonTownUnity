namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class DrawPhysics2D
    {
        public static Color colorForNonHittingCasts = UtilitiesDXXL_Colors.red_boolFalse;
        public static Color colorForHittingCasts = UtilitiesDXXL_Colors.green_boolTrue;
        public static Color colorForCastLineBeyondHit = UtilitiesDXXL_Colors.orange_lineThresholdMiddleDistance;
        public static Color colorForCastsHitText = UtilitiesDXXL_Colors.purple_raycastHitTextDefault;
        public static Color overwriteColorForCastsHitNormals = default(Color);
        public static float scaleFactor_forCastHitTextSize = 1.0f;
        public static float castCorridorVisualizerDensity = 1.0f;
        public static int maxCorridorVisualizersPerCastVisualization = 600;
        public static int hitResultsWithMoreDetailedDisplay = 2;
        public static bool drawCastNameTag_atCastOrigin = true;
        public static bool drawCastNameTag_atHitPositions = true;
        public static DrawPhysics.VisualizationQuality visualizationQuality = DrawPhysics.VisualizationQuality.high_withFullDetails;

        private static int maxListedColliders_inOverlapVolumesTextList = 10;
        public static int MaxListedColliders_inOverlapVolumesTextList 
        {
            get { return maxListedColliders_inOverlapVolumesTextList; }
            set { maxListedColliders_inOverlapVolumesTextList = Mathf.Max(value, 1); }
        }

        public static int maxOverlapingCollidersWithUntruncatedText = 10;

        private static int maxNumberOfPreallocatedHits = 100;
        public static int MaxNumberOfPreallocatedHits
        {
            get { return maxNumberOfPreallocatedHits; }
            set
            {
                maxNumberOfPreallocatedHits = value;
                if (maxNumberOfPreallocatedHits < 1)
                {
                    Debug.LogWarning("Minimum value for 'maxNumberOfPreallocatedHits' is 1. Delivered value of " + maxNumberOfPreallocatedHits + " has been rounded up to 1.");
                    maxNumberOfPreallocatedHits = 1;
                }
                if (maxNumberOfPreallocatedHits > 100000000)
                {
                    Debug.LogWarning("Maximum value for 'maxNumberOfPreallocatedHits' is 100000000. Delivered value of " + maxNumberOfPreallocatedHits + " has been rounded down to 100000000.");
                    maxNumberOfPreallocatedHits = 100000000;
                }
                UtilitiesDXXL_Physics2D.preallocatedRayHit2DResultsArray_copiedFromList = new RaycastHit2D[maxNumberOfPreallocatedHits];
                UtilitiesDXXL_Physics2D.preallocatedCollider2DResultsArray_copiedFromList = new Collider2D[maxNumberOfPreallocatedHits];
            }
        }

        public static float custom_zPos_forCastVisualisation = float.PositiveInfinity;
        public static float forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts = 0.0f;
        public static float forcedConstantWorldspaceTextSize_forOverlapResultTexts = 0.0f;

        public static Vector2 directionOfHitResultText = default(Vector2);

        public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance = Mathf.Infinity, int layerMask = Physics2D.AllLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            RaycastHit2D result = Physics2D.BoxCast(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return result; }
            bool hasHit = (result.collider != null);
            UtilitiesDXXL_Physics2D.DrawBoxcastTillFirstHit(hasHit, origin, size, angle, direction, distance, result, nameTag, durationInSec, hiddenByNearerObjects);
            return result;
        }

        public static int BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, float distance = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfUsedSlotsInHitInfoCollection = Physics2D.BoxCast(origin, size, angle, direction, contactFilter, results, distance);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoCollection; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoCollection, results);
            int used_numberOfUsedSlotsInHitInfoCollection = resultsArrayIsNull ? 0 : numberOfUsedSlotsInHitInfoCollection;
            UtilitiesDXXL_Physics2D.DrawBoxcastPotMultipleHits(origin, size, angle, direction, distance, results, used_numberOfUsedSlotsInHitInfoCollection, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoCollection;
        }

        public static int BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, float distance = float.PositiveInfinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfUsedSlotsInHitInfoCollection = Physics2D.BoxCast(origin, size, angle, direction, contactFilter, results, distance);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoCollection; }
            UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultList(ref nameTag, numberOfUsedSlotsInHitInfoCollection, results);
            int numberOfCopiedSlots = UtilitiesDXXL_Physics2D.CopyHitResultsFromListToPreallocatedArray(out bool preallocatedArrayIsTooSmall, results, numberOfUsedSlotsInHitInfoCollection);
            UtilitiesDXXL_Physics2D.ExtentNameTagForPreallocatedArrayIsTooSmallForFilledInListResults(preallocatedArrayIsTooSmall, ref nameTag, numberOfUsedSlotsInHitInfoCollection);
            UtilitiesDXXL_Physics2D.DrawBoxcastPotMultipleHits(origin, size, angle, direction, distance, UtilitiesDXXL_Physics2D.preallocatedRayHit2DResultsArray_copiedFromList, numberOfCopiedSlots, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoCollection;
        }

        public static RaycastHit2D[] BoxCastAll(Vector2 origin, Vector2 size, float angle, Vector2 direction, float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            RaycastHit2D[] results = Physics2D.BoxCastAll(origin, size, angle, direction, distance, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return results; }
            int numberOfUsedSlotsInHitInfoCollection = (results == null) ? 0 : results.Length;
            UtilitiesDXXL_Physics2D.DrawBoxcastPotMultipleHits(origin, size, angle, direction, distance, results, numberOfUsedSlotsInHitInfoCollection, nameTag, durationInSec, hiddenByNearerObjects);
            return results;
        }

        public static int BoxCastNonAlloc(Vector2 origin, Vector2 size, float angle, Vector2 direction, RaycastHit2D[] results, float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfUsedSlotsInHitInfoCollection = Physics2D.BoxCastNonAlloc(origin, size, angle, direction, results, distance, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoCollection; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoCollection, results);
            int used_numberOfUsedSlotsInHitInfoCollection = resultsArrayIsNull ? 0 : numberOfUsedSlotsInHitInfoCollection;
            UtilitiesDXXL_Physics2D.DrawBoxcastPotMultipleHits(origin, size, angle, direction, distance, results, used_numberOfUsedSlotsInHitInfoCollection, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoCollection;
        }

        public static RaycastHit2D CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            RaycastHit2D result = Physics2D.CapsuleCast(origin, size, capsuleDirection, angle, direction, distance, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return result; }
            bool hasHit = (result.collider != null);
            UtilitiesDXXL_Physics2D.DrawCapsulecastTillFirstHit(hasHit, origin, size, direction, capsuleDirection, angle, distance, result, nameTag, durationInSec, hiddenByNearerObjects);
            return result;
        }

        public static int CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, float distance = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfUsedSlotsInHitInfoCollection = Physics2D.CapsuleCast(origin, size, capsuleDirection, angle, direction, contactFilter, results, distance);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoCollection; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoCollection, results);
            int used_numberOfUsedSlotsInHitInfoCollection = resultsArrayIsNull ? 0 : numberOfUsedSlotsInHitInfoCollection;
            UtilitiesDXXL_Physics2D.DrawCapsulecastPotMultipleHits(origin, size, direction, capsuleDirection, angle, distance, results, used_numberOfUsedSlotsInHitInfoCollection, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoCollection;
        }

        public static int CapsuleCast(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, float distance = float.PositiveInfinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfUsedSlotsInHitInfoCollection = Physics2D.CapsuleCast(origin, size, capsuleDirection, angle, direction, contactFilter, results, distance);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoCollection; }
            UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultList(ref nameTag, numberOfUsedSlotsInHitInfoCollection, results);
            int numberOfCopiedSlots = UtilitiesDXXL_Physics2D.CopyHitResultsFromListToPreallocatedArray(out bool preallocatedArrayIsTooSmall, results, numberOfUsedSlotsInHitInfoCollection);
            UtilitiesDXXL_Physics2D.ExtentNameTagForPreallocatedArrayIsTooSmallForFilledInListResults(preallocatedArrayIsTooSmall, ref nameTag, numberOfUsedSlotsInHitInfoCollection);
            UtilitiesDXXL_Physics2D.DrawCapsulecastPotMultipleHits(origin, size, direction, capsuleDirection, angle, distance, UtilitiesDXXL_Physics2D.preallocatedRayHit2DResultsArray_copiedFromList, numberOfCopiedSlots, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoCollection;
        }

        public static RaycastHit2D[] CapsuleCastAll(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            RaycastHit2D[] results = Physics2D.CapsuleCastAll(origin, size, capsuleDirection, angle, direction, distance, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return results; }
            int numberOfUsedSlotsInHitInfoCollection = (results == null) ? 0 : results.Length;
            UtilitiesDXXL_Physics2D.DrawCapsulecastPotMultipleHits(origin, size, direction, capsuleDirection, angle, distance, results, numberOfUsedSlotsInHitInfoCollection, nameTag, durationInSec, hiddenByNearerObjects);
            return results;
        }

        public static int CapsuleCastNonAlloc(Vector2 origin, Vector2 size, CapsuleDirection2D capsuleDirection, float angle, Vector2 direction, RaycastHit2D[] results, float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfUsedSlotsInHitInfoCollection = Physics2D.CapsuleCastNonAlloc(origin, size, capsuleDirection, angle, direction, results, distance, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoCollection; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoCollection, results);
            int used_numberOfUsedSlotsInHitInfoCollection = resultsArrayIsNull ? 0 : numberOfUsedSlotsInHitInfoCollection;
            UtilitiesDXXL_Physics2D.DrawCapsulecastPotMultipleHits(origin, size, direction, capsuleDirection, angle, distance, results, used_numberOfUsedSlotsInHitInfoCollection, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoCollection;
        }

        public static RaycastHit2D CircleCast(Vector2 origin, float radius, Vector2 direction, float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            RaycastHit2D result = Physics2D.CircleCast(origin, radius, direction, distance, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return result; }
            bool hasHit = (result.collider != null);
            UtilitiesDXXL_Physics2D.DrawCirclecastTillFirstHit(radius, hasHit, origin, direction, distance, result, nameTag, durationInSec, hiddenByNearerObjects);
            return result;
        }

        public static int CircleCast(Vector2 origin, float radius, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, float distance = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfUsedSlotsInHitInfoCollection = Physics2D.CircleCast(origin, radius, direction, contactFilter, results, distance);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoCollection; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoCollection, results);
            int used_numberOfUsedSlotsInHitInfoCollection = resultsArrayIsNull ? 0 : numberOfUsedSlotsInHitInfoCollection;
            UtilitiesDXXL_Physics2D.DrawCirclecastPotMultipleHits(radius, origin, direction, distance, results, used_numberOfUsedSlotsInHitInfoCollection, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoCollection;
        }

        public static int CircleCast(Vector2 origin, float radius, Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, float distance = float.PositiveInfinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfUsedSlotsInHitInfoCollection = Physics2D.CircleCast(origin, radius, direction, contactFilter, results, distance);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoCollection; }
            UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultList(ref nameTag, numberOfUsedSlotsInHitInfoCollection, results);
            int numberOfCopiedSlots = UtilitiesDXXL_Physics2D.CopyHitResultsFromListToPreallocatedArray(out bool preallocatedArrayIsTooSmall, results, numberOfUsedSlotsInHitInfoCollection);
            UtilitiesDXXL_Physics2D.ExtentNameTagForPreallocatedArrayIsTooSmallForFilledInListResults(preallocatedArrayIsTooSmall, ref nameTag, numberOfUsedSlotsInHitInfoCollection);
            UtilitiesDXXL_Physics2D.DrawCirclecastPotMultipleHits(radius, origin, direction, distance, UtilitiesDXXL_Physics2D.preallocatedRayHit2DResultsArray_copiedFromList, numberOfCopiedSlots, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoCollection;
        }

        public static RaycastHit2D[] CircleCastAll(Vector2 origin, float radius, Vector2 direction, float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            RaycastHit2D[] results = Physics2D.CircleCastAll(origin, radius, direction, distance, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return results; }
            int numberOfUsedSlotsInHitInfoCollection = (results == null) ? 0 : results.Length;
            UtilitiesDXXL_Physics2D.DrawCirclecastPotMultipleHits(radius, origin, direction, distance, results, numberOfUsedSlotsInHitInfoCollection, nameTag, durationInSec, hiddenByNearerObjects);
            return results;
        }

        public static int CircleCastNonAlloc(Vector2 origin, float radius, Vector2 direction, RaycastHit2D[] results, float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfUsedSlotsInHitInfoCollection = Physics2D.CircleCastNonAlloc(origin, radius, direction, results, distance, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoCollection; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoCollection, results);
            int used_numberOfUsedSlotsInHitInfoCollection = resultsArrayIsNull ? 0 : numberOfUsedSlotsInHitInfoCollection;
            UtilitiesDXXL_Physics2D.DrawCirclecastPotMultipleHits(radius, origin, direction, distance, results, used_numberOfUsedSlotsInHitInfoCollection, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoCollection;
        }

        public static RaycastHit2D GetRayIntersection(Ray ray, float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            RaycastHit2D result = Physics2D.GetRayIntersection(ray, distance, layerMask);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return result; }
            bool hasHit = (result.collider != null);
            UtilitiesDXXL_Physics2D.DrawRaycast3DTillFirstHit(hasHit, ray, distance, result, nameTag, durationInSec, hiddenByNearerObjects);
            return result;

        }

        public static RaycastHit2D[] GetRayIntersectionAll(Ray ray, float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            RaycastHit2D[] results = Physics2D.GetRayIntersectionAll(ray, distance, layerMask);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return results; }
            int numberOfUsedSlotsInHitInfoCollection = (results == null) ? 0 : results.Length;
            UtilitiesDXXL_Physics2D.DrawRaycast3DPotMultipleHits(ray, distance, results, numberOfUsedSlotsInHitInfoCollection, nameTag, durationInSec, hiddenByNearerObjects);
            return results;
        }

        public static int GetRayIntersectionNonAlloc(Ray ray, RaycastHit2D[] results, float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfUsedSlotsInHitInfoCollection = Physics2D.GetRayIntersectionNonAlloc(ray, results, distance, layerMask);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoCollection; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoCollection, results);
            int used_numberOfUsedSlotsInHitInfoCollection = resultsArrayIsNull ? 0 : numberOfUsedSlotsInHitInfoCollection;
            UtilitiesDXXL_Physics2D.DrawRaycast3DPotMultipleHits(ray, distance, results, used_numberOfUsedSlotsInHitInfoCollection, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoCollection;
        }

        public static RaycastHit2D Linecast(Vector2 start, Vector2 end, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            RaycastHit2D result = Physics2D.Linecast(start, end, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return result; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return result; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(end, "end")) { return result; }

            bool hasHit = (result.collider != null);
            Vector2 startToEnd = end - start;
            float length = startToEnd.magnitude;
            UtilitiesDXXL_Physics2D.DrawRaycastTillFirstHit(hasHit, start, startToEnd, length, result, nameTag, durationInSec, hiddenByNearerObjects);
            return result;
        }

        public static int Linecast(Vector2 start, Vector2 end, ContactFilter2D contactFilter, RaycastHit2D[] results, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfUsedSlotsInHitInfoCollection = Physics2D.Linecast(start, end, contactFilter, results);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoCollection; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return numberOfUsedSlotsInHitInfoCollection; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(end, "end")) { return numberOfUsedSlotsInHitInfoCollection; }

            Vector2 startToEnd = end - start;
            float length = startToEnd.magnitude;
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoCollection, results);
            int used_numberOfUsedSlotsInHitInfoCollection = resultsArrayIsNull ? 0 : numberOfUsedSlotsInHitInfoCollection;
            UtilitiesDXXL_Physics2D.DrawRaycastPotMultipleHits(start, startToEnd, length, results, used_numberOfUsedSlotsInHitInfoCollection, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoCollection;
        }

        public static int Linecast(Vector2 start, Vector2 end, ContactFilter2D contactFilter, List<RaycastHit2D> results, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfUsedSlotsInHitInfoCollection = Physics2D.Linecast(start, end, contactFilter, results);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoCollection; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return numberOfUsedSlotsInHitInfoCollection; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(end, "end")) { return numberOfUsedSlotsInHitInfoCollection; }

            Vector2 startToEnd = end - start;
            float length = startToEnd.magnitude;
            UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultList(ref nameTag, numberOfUsedSlotsInHitInfoCollection, results);
            int numberOfCopiedSlots = UtilitiesDXXL_Physics2D.CopyHitResultsFromListToPreallocatedArray(out bool preallocatedArrayIsTooSmall, results, numberOfUsedSlotsInHitInfoCollection);
            UtilitiesDXXL_Physics2D.ExtentNameTagForPreallocatedArrayIsTooSmallForFilledInListResults(preallocatedArrayIsTooSmall, ref nameTag, numberOfUsedSlotsInHitInfoCollection);
            UtilitiesDXXL_Physics2D.DrawRaycastPotMultipleHits(start, startToEnd, length, UtilitiesDXXL_Physics2D.preallocatedRayHit2DResultsArray_copiedFromList, numberOfCopiedSlots, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoCollection;
        }

        public static RaycastHit2D[] LinecastAll(Vector2 start, Vector2 end, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            RaycastHit2D[] results = Physics2D.LinecastAll(start, end, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return results; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return results; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(end, "end")) { return results; }

            int numberOfUsedSlotsInHitInfoCollection = (results == null) ? 0 : results.Length;
            Vector2 startToEnd = end - start;
            float length = startToEnd.magnitude;
            UtilitiesDXXL_Physics2D.DrawRaycastPotMultipleHits(start, startToEnd, length, results, numberOfUsedSlotsInHitInfoCollection, nameTag, durationInSec, hiddenByNearerObjects);
            return results;
        }

        public static int LinecastNonAlloc(Vector2 start, Vector2 end, RaycastHit2D[] results, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfUsedSlotsInHitInfoCollection = Physics2D.LinecastNonAlloc(start, end, results, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoCollection; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return numberOfUsedSlotsInHitInfoCollection; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(end, "end")) { return numberOfUsedSlotsInHitInfoCollection; }

            Vector2 startToEnd = end - start;
            float length = startToEnd.magnitude;
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoCollection, results);
            int used_numberOfUsedSlotsInHitInfoCollection = resultsArrayIsNull ? 0 : numberOfUsedSlotsInHitInfoCollection;
            UtilitiesDXXL_Physics2D.DrawRaycastPotMultipleHits(start, startToEnd, length, results, used_numberOfUsedSlotsInHitInfoCollection, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoCollection;
        }

        public static RaycastHit2D Raycast(Vector2 origin, Vector2 direction, float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            RaycastHit2D result = Physics2D.Raycast(origin, direction, distance, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return result; }
            bool hasHit = (result.collider != null);
            UtilitiesDXXL_Physics2D.DrawRaycastTillFirstHit(hasHit, origin, direction, distance, result, nameTag, durationInSec, hiddenByNearerObjects);
            return result;
        }

        public static int Raycast(Vector2 origin, Vector2 direction, ContactFilter2D contactFilter, RaycastHit2D[] results, float distance = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfUsedSlotsInHitInfoCollection = Physics2D.Raycast(origin, direction, contactFilter, results, distance);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoCollection; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoCollection, results);
            int used_numberOfUsedSlotsInHitInfoCollection = resultsArrayIsNull ? 0 : numberOfUsedSlotsInHitInfoCollection;
            UtilitiesDXXL_Physics2D.DrawRaycastPotMultipleHits(origin, direction, distance, results, used_numberOfUsedSlotsInHitInfoCollection, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoCollection;
        }

        public static int Raycast(Vector2 origin, Vector2 direction, ContactFilter2D contactFilter, List<RaycastHit2D> results, float distance = float.PositiveInfinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfUsedSlotsInHitInfoCollection = Physics2D.Raycast(origin, direction, contactFilter, results, distance);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoCollection; }
            UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultList(ref nameTag, numberOfUsedSlotsInHitInfoCollection, results);
            int numberOfCopiedSlots = UtilitiesDXXL_Physics2D.CopyHitResultsFromListToPreallocatedArray(out bool preallocatedArrayIsTooSmall, results, numberOfUsedSlotsInHitInfoCollection);
            UtilitiesDXXL_Physics2D.ExtentNameTagForPreallocatedArrayIsTooSmallForFilledInListResults(preallocatedArrayIsTooSmall, ref nameTag, numberOfUsedSlotsInHitInfoCollection);
            UtilitiesDXXL_Physics2D.DrawRaycastPotMultipleHits(origin, direction, distance, UtilitiesDXXL_Physics2D.preallocatedRayHit2DResultsArray_copiedFromList, numberOfCopiedSlots, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoCollection;
        }

        public static RaycastHit2D[] RaycastAll(Vector2 origin, Vector2 direction, float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            RaycastHit2D[] results = Physics2D.RaycastAll(origin, direction, distance, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return results; }
            int numberOfUsedSlotsInHitInfoCollection = (results == null) ? 0 : results.Length;
            UtilitiesDXXL_Physics2D.DrawRaycastPotMultipleHits(origin, direction, distance, results, numberOfUsedSlotsInHitInfoCollection, nameTag, durationInSec, hiddenByNearerObjects);
            return results;
        }

        public static int RaycastNonAlloc(Vector2 origin, Vector2 direction, RaycastHit2D[] results, float distance = Mathf.Infinity, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfUsedSlotsInHitInfoCollection = Physics2D.RaycastNonAlloc(origin, direction, results, distance, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoCollection; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoCollection, results);
            int used_numberOfUsedSlotsInHitInfoCollection = resultsArrayIsNull ? 0 : numberOfUsedSlotsInHitInfoCollection;
            UtilitiesDXXL_Physics2D.DrawRaycastPotMultipleHits(origin, direction, distance, results, used_numberOfUsedSlotsInHitInfoCollection, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoCollection;
        }

        public static Collider2D OverlapArea(Vector2 pointA, Vector2 pointB, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            Collider2D overlappingCollider = Physics2D.OverlapArea(pointA, pointB, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return overlappingCollider; }
            UtilitiesDXXL_Physics2D.Area2D_to_Box2D(out Vector2 boxCenterPos_V2, out Vector2 boxSize_V2, pointA, pointB);
            UtilitiesDXXL_Physics2D.DrawBoxOverlapResultOfMax1Collision(true, boxCenterPos_V2, boxSize_V2, 0.0f, overlappingCollider, nameTag, durationInSec, hiddenByNearerObjects);
            return overlappingCollider;
        }

        public static int OverlapArea(Vector2 pointA, Vector2 pointB, ContactFilter2D contactFilter, Collider2D[] results, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfOverlappingColliders = Physics2D.OverlapArea(pointA, pointB, contactFilter, results);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfOverlappingColliders; }
            UtilitiesDXXL_Physics2D.Area2D_to_Box2D(out Vector2 boxCenterPos_V2, out Vector2 boxSize_V2, pointA, pointB);
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfOverlappingColliders, results);
            int used_numberOfOverlappingColliders = resultsArrayIsNull ? 0 : numberOfOverlappingColliders;
            UtilitiesDXXL_Physics2D.DrawBoxOverlapResultOfPotMultipleCollisions(true, boxCenterPos_V2, boxSize_V2, 0.0f, used_numberOfOverlappingColliders, results, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfOverlappingColliders;
        }

        public static int OverlapArea(Vector2 pointA, Vector2 pointB, ContactFilter2D contactFilter, List<Collider2D> results, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfOverlappingColliders = Physics2D.OverlapArea(pointA, pointB, contactFilter, results);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfOverlappingColliders; }
            UtilitiesDXXL_Physics2D.Area2D_to_Box2D(out Vector2 boxCenterPos_V2, out Vector2 boxSize_V2, pointA, pointB);
            UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultList(ref nameTag, numberOfOverlappingColliders, results);
            int numberOfCopiedSlots = UtilitiesDXXL_Physics2D.CopyHitResultsFromListToPreallocatedArray(out bool preallocatedArrayIsTooSmall, results, numberOfOverlappingColliders);
            UtilitiesDXXL_Physics2D.ExtentNameTagForPreallocatedArrayIsTooSmallForFilledInListResults(preallocatedArrayIsTooSmall, ref nameTag, numberOfOverlappingColliders);
            UtilitiesDXXL_Physics2D.DrawBoxOverlapResultOfPotMultipleCollisions(true, boxCenterPos_V2, boxSize_V2, 0.0f, numberOfCopiedSlots, UtilitiesDXXL_Physics2D.preallocatedCollider2DResultsArray_copiedFromList, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfOverlappingColliders;
        }

        public static Collider2D[] OverlapAreaAll(Vector2 pointA, Vector2 pointB, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            Collider2D[] overlappingColliders = Physics2D.OverlapAreaAll(pointA, pointB, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return overlappingColliders; }
            UtilitiesDXXL_Physics2D.Area2D_to_Box2D(out Vector2 boxCenterPos_V2, out Vector2 boxSize_V2, pointA, pointB);
            int numberOfOverlappingColliders = 0;
            if (overlappingColliders != null && overlappingColliders.Length > 0)
            {
                numberOfOverlappingColliders = overlappingColliders.Length;
            }
            UtilitiesDXXL_Physics2D.DrawBoxOverlapResultOfPotMultipleCollisions(true, boxCenterPos_V2, boxSize_V2, 0.0f, numberOfOverlappingColliders, overlappingColliders, nameTag, durationInSec, hiddenByNearerObjects);
            return overlappingColliders;
        }

        public static int OverlapAreaNonAlloc(Vector2 pointA, Vector2 pointB, Collider2D[] results, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfOverlappingColliders = Physics2D.OverlapAreaNonAlloc(pointA, pointB, results, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfOverlappingColliders; }
            UtilitiesDXXL_Physics2D.Area2D_to_Box2D(out Vector2 boxCenterPos_V2, out Vector2 boxSize_V2, pointA, pointB);
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfOverlappingColliders, results);
            int used_numberOfOverlappingColliders = resultsArrayIsNull ? 0 : numberOfOverlappingColliders;
            UtilitiesDXXL_Physics2D.DrawBoxOverlapResultOfPotMultipleCollisions(true, boxCenterPos_V2, boxSize_V2, 0.0f, used_numberOfOverlappingColliders, results, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfOverlappingColliders;
        }

        public static Collider2D OverlapBox(Vector2 point, Vector2 size, float angle, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            Collider2D overlappingCollider = Physics2D.OverlapBox(point, size, angle, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return overlappingCollider; }
            UtilitiesDXXL_Physics2D.DrawBoxOverlapResultOfMax1Collision(false, point, size, angle, overlappingCollider, nameTag, durationInSec, hiddenByNearerObjects);
            return overlappingCollider;
        }

        public static int OverlapBox(Vector2 point, Vector2 size, float angle, ContactFilter2D contactFilter, Collider2D[] results, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfOverlappingColliders = Physics2D.OverlapBox(point, size, angle, contactFilter, results);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfOverlappingColliders; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfOverlappingColliders, results);
            int used_numberOfOverlappingColliders = resultsArrayIsNull ? 0 : numberOfOverlappingColliders;
            UtilitiesDXXL_Physics2D.DrawBoxOverlapResultOfPotMultipleCollisions(false, point, size, angle, used_numberOfOverlappingColliders, results, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfOverlappingColliders;
        }

        public static int OverlapBox(Vector2 point, Vector2 size, float angle, ContactFilter2D contactFilter, List<Collider2D> results, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfOverlappingColliders = Physics2D.OverlapBox(point, size, angle, contactFilter, results);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfOverlappingColliders; }
            UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultList(ref nameTag, numberOfOverlappingColliders, results);
            int numberOfCopiedSlots = UtilitiesDXXL_Physics2D.CopyHitResultsFromListToPreallocatedArray(out bool preallocatedArrayIsTooSmall, results, numberOfOverlappingColliders);
            UtilitiesDXXL_Physics2D.ExtentNameTagForPreallocatedArrayIsTooSmallForFilledInListResults(preallocatedArrayIsTooSmall, ref nameTag, numberOfOverlappingColliders);
            UtilitiesDXXL_Physics2D.DrawBoxOverlapResultOfPotMultipleCollisions(false, point, size, angle, numberOfCopiedSlots, UtilitiesDXXL_Physics2D.preallocatedCollider2DResultsArray_copiedFromList, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfOverlappingColliders;
        }

        public static Collider2D[] OverlapBoxAll(Vector2 point, Vector2 size, float angle, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            Collider2D[] overlappingColliders = Physics2D.OverlapBoxAll(point, size, angle, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return overlappingColliders; }
            int numberOfOverlappingColliders = 0;
            if (overlappingColliders != null && overlappingColliders.Length > 0)
            {
                numberOfOverlappingColliders = overlappingColliders.Length;
            }
            UtilitiesDXXL_Physics2D.DrawBoxOverlapResultOfPotMultipleCollisions(false, point, size, angle, numberOfOverlappingColliders, overlappingColliders, nameTag, durationInSec, hiddenByNearerObjects);
            return overlappingColliders;
        }

        public static int OverlapBoxNonAlloc(Vector2 point, Vector2 size, float angle, Collider2D[] results, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfOverlappingColliders = Physics2D.OverlapBoxNonAlloc(point, size, angle, results, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfOverlappingColliders; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfOverlappingColliders, results);
            int used_numberOfOverlappingColliders = resultsArrayIsNull ? 0 : numberOfOverlappingColliders;
            UtilitiesDXXL_Physics2D.DrawBoxOverlapResultOfPotMultipleCollisions(false, point, size, angle, used_numberOfOverlappingColliders, results, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfOverlappingColliders;
        }

        public static Collider2D OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            Collider2D overlappingCollider = Physics2D.OverlapCapsule(point, size, direction, angle, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return overlappingCollider; }
            UtilitiesDXXL_Physics2D.DrawCapsuleOverlapResultOfMax1Collision(point, size, direction, angle, overlappingCollider, nameTag, durationInSec, hiddenByNearerObjects);
            return overlappingCollider;
        }

        public static int OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, ContactFilter2D contactFilter, Collider2D[] results, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfOverlappingColliders = Physics2D.OverlapCapsule(point, size, direction, angle, contactFilter, results);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfOverlappingColliders; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfOverlappingColliders, results);
            int used_numberOfOverlappingColliders = resultsArrayIsNull ? 0 : numberOfOverlappingColliders;
            UtilitiesDXXL_Physics2D.DrawCapsuleOverlapResultOfPotMultipleCollisions(point, size, direction, angle, used_numberOfOverlappingColliders, results, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfOverlappingColliders;
        }

        public static int OverlapCapsule(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, ContactFilter2D contactFilter, List<Collider2D> results, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfOverlappingColliders = Physics2D.OverlapCapsule(point, size, direction, angle, contactFilter, results);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfOverlappingColliders; }
            UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultList(ref nameTag, numberOfOverlappingColliders, results);
            int numberOfCopiedSlots = UtilitiesDXXL_Physics2D.CopyHitResultsFromListToPreallocatedArray(out bool preallocatedArrayIsTooSmall, results, numberOfOverlappingColliders);
            UtilitiesDXXL_Physics2D.ExtentNameTagForPreallocatedArrayIsTooSmallForFilledInListResults(preallocatedArrayIsTooSmall, ref nameTag, numberOfOverlappingColliders);
            UtilitiesDXXL_Physics2D.DrawCapsuleOverlapResultOfPotMultipleCollisions(point, size, direction, angle, numberOfCopiedSlots, UtilitiesDXXL_Physics2D.preallocatedCollider2DResultsArray_copiedFromList, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfOverlappingColliders;
        }

        public static Collider2D[] OverlapCapsuleAll(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            Collider2D[] overlappingColliders = Physics2D.OverlapCapsuleAll(point, size, direction, angle, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return overlappingColliders; }
            int numberOfOverlappingColliders = 0;
            if (overlappingColliders != null && overlappingColliders.Length > 0)
            {
                numberOfOverlappingColliders = overlappingColliders.Length;
            }
            UtilitiesDXXL_Physics2D.DrawCapsuleOverlapResultOfPotMultipleCollisions(point, size, direction, angle, numberOfOverlappingColliders, overlappingColliders, nameTag, durationInSec, hiddenByNearerObjects);
            return overlappingColliders;
        }

        public static int OverlapCapsuleNonAlloc(Vector2 point, Vector2 size, CapsuleDirection2D direction, float angle, Collider2D[] results, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfOverlappingColliders = Physics2D.OverlapCapsuleNonAlloc(point, size, direction, angle, results, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfOverlappingColliders; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfOverlappingColliders, results);
            int used_numberOfOverlappingColliders = resultsArrayIsNull ? 0 : numberOfOverlappingColliders;
            UtilitiesDXXL_Physics2D.DrawCapsuleOverlapResultOfPotMultipleCollisions(point, size, direction, angle, used_numberOfOverlappingColliders, results, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfOverlappingColliders;
        }

        public static Collider2D OverlapCircle(Vector2 point, float radius, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            Collider2D overlappingCollider = Physics2D.OverlapCircle(point, radius, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return overlappingCollider; }
            UtilitiesDXXL_Physics2D.DrawCircleOverlapResultOfMax1Collision(point, radius, overlappingCollider, nameTag, durationInSec, hiddenByNearerObjects);
            return overlappingCollider;
        }

        public static int OverlapCircle(Vector2 point, float radius, ContactFilter2D contactFilter, Collider2D[] results, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfOverlappingColliders = Physics2D.OverlapCircle(point, radius, contactFilter, results);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfOverlappingColliders; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfOverlappingColliders, results);
            int used_numberOfOverlappingColliders = resultsArrayIsNull ? 0 : numberOfOverlappingColliders;
            UtilitiesDXXL_Physics2D.DrawCircleOverlapResultOfPotMultipleCollisions(point, radius, used_numberOfOverlappingColliders, results, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfOverlappingColliders;
        }

        public static int OverlapCircle(Vector2 point, float radius, ContactFilter2D contactFilter, List<Collider2D> results, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfOverlappingColliders = Physics2D.OverlapCircle(point, radius, contactFilter, results);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfOverlappingColliders; }
            UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultList(ref nameTag, numberOfOverlappingColliders, results);
            int numberOfCopiedSlots = UtilitiesDXXL_Physics2D.CopyHitResultsFromListToPreallocatedArray(out bool preallocatedArrayIsTooSmall, results, numberOfOverlappingColliders);
            UtilitiesDXXL_Physics2D.ExtentNameTagForPreallocatedArrayIsTooSmallForFilledInListResults(preallocatedArrayIsTooSmall, ref nameTag, numberOfOverlappingColliders);
            UtilitiesDXXL_Physics2D.DrawCircleOverlapResultOfPotMultipleCollisions(point, radius, numberOfCopiedSlots, UtilitiesDXXL_Physics2D.preallocatedCollider2DResultsArray_copiedFromList, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfOverlappingColliders;
        }

        public static Collider2D[] OverlapCircleAll(Vector2 point, float radius, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            Collider2D[] overlappingColliders = Physics2D.OverlapCircleAll(point, radius, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return overlappingColliders; }
            int numberOfOverlappingColliders = 0;
            if (overlappingColliders != null && overlappingColliders.Length > 0)
            {
                numberOfOverlappingColliders = overlappingColliders.Length;
            }
            UtilitiesDXXL_Physics2D.DrawCircleOverlapResultOfPotMultipleCollisions(point, radius, numberOfOverlappingColliders, overlappingColliders, nameTag, durationInSec, hiddenByNearerObjects);
            return overlappingColliders;
        }

        public static int OverlapCircleNonAlloc(Vector2 point, float radius, Collider2D[] results, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfOverlappingColliders = Physics2D.OverlapCircleNonAlloc(point, radius, results, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfOverlappingColliders; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfOverlappingColliders, results);
            int used_numberOfOverlappingColliders = resultsArrayIsNull ? 0 : numberOfOverlappingColliders;
            UtilitiesDXXL_Physics2D.DrawCircleOverlapResultOfPotMultipleCollisions(point, radius, used_numberOfOverlappingColliders, results, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfOverlappingColliders;
        }

        public static Collider2D OverlapPoint(Vector2 point, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            Collider2D overlappingCollider = Physics2D.OverlapPoint(point, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return overlappingCollider; }
            UtilitiesDXXL_Physics2D.DrawPointOverlapResultOfMax1Collision(point, overlappingCollider, nameTag, durationInSec, hiddenByNearerObjects);
            return overlappingCollider;
        }

        public static int OverlapPoint(Vector2 point, ContactFilter2D contactFilter, Collider2D[] results, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfOverlappingColliders = Physics2D.OverlapPoint(point, contactFilter, results);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfOverlappingColliders; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfOverlappingColliders, results);
            int used_numberOfOverlappingColliders = resultsArrayIsNull ? 0 : numberOfOverlappingColliders;
            UtilitiesDXXL_Physics2D.DrawPointOverlapResultOfPotMultipleCollisions(point, used_numberOfOverlappingColliders, results, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfOverlappingColliders;
        }

        public static int OverlapPoint(Vector2 point, ContactFilter2D contactFilter, List<Collider2D> results, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfOverlappingColliders = Physics2D.OverlapPoint(point, contactFilter, results);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfOverlappingColliders; }
            UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultList(ref nameTag, numberOfOverlappingColliders, results);
            int numberOfCopiedSlots = UtilitiesDXXL_Physics2D.CopyHitResultsFromListToPreallocatedArray(out bool preallocatedArrayIsTooSmall, results, numberOfOverlappingColliders);
            UtilitiesDXXL_Physics2D.ExtentNameTagForPreallocatedArrayIsTooSmallForFilledInListResults(preallocatedArrayIsTooSmall, ref nameTag, numberOfOverlappingColliders);
            UtilitiesDXXL_Physics2D.DrawPointOverlapResultOfPotMultipleCollisions(point, numberOfCopiedSlots, UtilitiesDXXL_Physics2D.preallocatedCollider2DResultsArray_copiedFromList, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfOverlappingColliders;
        }

        public static Collider2D[] OverlapPointAll(Vector2 point, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            Collider2D[] overlappingColliders = Physics2D.OverlapPointAll(point, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return overlappingColliders; }
            int numberOfOverlappingColliders = 0;
            if (overlappingColliders != null && overlappingColliders.Length > 0)
            {
                numberOfOverlappingColliders = overlappingColliders.Length;
            }
            UtilitiesDXXL_Physics2D.DrawPointOverlapResultOfPotMultipleCollisions(point, numberOfOverlappingColliders, overlappingColliders, nameTag, durationInSec, hiddenByNearerObjects);
            return overlappingColliders;
        }

        public static int OverlapPointNonAlloc(Vector2 point, Collider2D[] results, int layerMask = Physics2D.DefaultRaycastLayers, float minDepth = Mathf.NegativeInfinity, float maxDepth = Mathf.Infinity, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfOverlappingColliders = Physics2D.OverlapPointNonAlloc(point, results, layerMask, minDepth, maxDepth);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfOverlappingColliders; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics2D.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfOverlappingColliders, results);
            int used_numberOfOverlappingColliders = resultsArrayIsNull ? 0 : numberOfOverlappingColliders;
            UtilitiesDXXL_Physics2D.DrawPointOverlapResultOfPotMultipleCollisions(point, used_numberOfOverlappingColliders, results, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfOverlappingColliders;
        }

    }

}
