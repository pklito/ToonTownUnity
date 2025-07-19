namespace DrawXXL
{
    using UnityEngine;
    public class DrawPhysics
    {
        public static Color colorForNonHittingCasts = UtilitiesDXXL_Colors.red_boolFalse;
        public static Color colorForHittingCasts = UtilitiesDXXL_Colors.green_boolTrue;
        public static Color colorForCastLineBeyondHit = UtilitiesDXXL_Colors.orange_lineThresholdMiddleDistance;
        public static Color colorForCastsHitText = UtilitiesDXXL_Colors.purple_raycastHitTextDefault;
        public static Color overwriteColorForCastsHitNormals = default(Color);
        public static float scaleFactor_forCastHitTextSize = 1.0f;
        public static float castSilhouetteVisualizerDensity = 1.0f;
        public static int maxSilhouettesPerCastVisualization = 100;
        public static int hitResultsWithMoreDetailedDisplay = 2;
        public static bool drawCastNameTag_atCastOrigin = true;
        public static bool drawCastNameTag_atHitPositions = true;

        public enum VisualizationQuality 
        {
            high_withFullDetails, 
            medium_meaningReducedTextAndSilhouettes,
            low_withoutAnyTextOrSilhouettes
        };
        public static VisualizationQuality visualizationQuality = VisualizationQuality.high_withFullDetails;

        private static int maxListedColliders_inOverlapVolumesTextList = 10;
        public static int MaxListedColliders_inOverlapVolumesTextList
        {
            get { return maxListedColliders_inOverlapVolumesTextList; }
            set { maxListedColliders_inOverlapVolumesTextList = Mathf.Max(value, 1); }
        }

        public static int maxOverlapingCollidersWithUntruncatedText = 10;
        public static float forcedConstantScreenspaceTextSize_relToScreenHeight_forOverlapResultTexts = 0.0f;
        public static float forcedConstantWorldspaceTextSize_forOverlapResultTexts = 0.0f;
     
        public static Vector2 directionOfHitResultText = default(Vector2);

        public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation = default(Quaternion), float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            orientation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(orientation);
            bool hasHit = Physics.BoxCast(center, halfExtents, direction, out RaycastHit hitInfo, orientation, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hasHit; }
            UtilitiesDXXL_Physics.DrawBoxcastTillFirstHit(hasHit, center, halfExtents, orientation, direction, maxDistance, hitInfo, nameTag, durationInSec, hiddenByNearerObjects);
            return hasHit;
        }

        public static bool BoxCast(Vector3 center, Vector3 halfExtents, Vector3 direction, out RaycastHit hitInfo, Quaternion orientation = default(Quaternion), float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            orientation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(orientation);
            bool hasHit = Physics.BoxCast(center, halfExtents, direction, out hitInfo, orientation, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hasHit; }
            UtilitiesDXXL_Physics.DrawBoxcastTillFirstHit(hasHit, center, halfExtents, orientation, direction, maxDistance, hitInfo, nameTag, durationInSec, hiddenByNearerObjects);
            return hasHit;
        }

        public static RaycastHit[] BoxCastAll(Vector3 center, Vector3 halfExtents, Vector3 direction, Quaternion orientation = default(Quaternion), float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            orientation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(orientation);
            RaycastHit[] hitInfos = Physics.BoxCastAll(center, halfExtents, direction, orientation, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hitInfos; }
            int numberOfUsedSlotsInHitInfoArray = 0;
            if (hitInfos != null) { numberOfUsedSlotsInHitInfoArray = hitInfos.Length; }
            UtilitiesDXXL_Physics.DrawBoxcastPotMultipleHits(center, halfExtents, orientation, direction, maxDistance, hitInfos, numberOfUsedSlotsInHitInfoArray, nameTag, durationInSec, hiddenByNearerObjects);
            return hitInfos;
        }

        public static int BoxCastNonAlloc(Vector3 center, Vector3 halfExtents, Vector3 direction, RaycastHit[] results, Quaternion orientation = default(Quaternion), float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            orientation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(orientation);
            int numberOfUsedSlotsInHitInfoArray = Physics.BoxCastNonAlloc(center, halfExtents, direction, results, orientation, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoArray; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoArray, results);
            UtilitiesDXXL_Physics.DrawBoxcastPotMultipleHits(center, halfExtents, orientation, direction, maxDistance, results, resultsArrayIsNull ? 0 : numberOfUsedSlotsInHitInfoArray, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoArray;
        }

        public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            bool hasHit = Physics.CapsuleCast(point1, point2, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hasHit; }
            UtilitiesDXXL_Physics.DrawCapsulecastTillFirstHit(radius, hasHit, point1, point2, direction, maxDistance, hitInfo, nameTag, durationInSec, hiddenByNearerObjects);
            return hasHit;
        }

        public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            bool hasHit = Physics.CapsuleCast(point1, point2, radius, direction, out RaycastHit hitInfo, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hasHit; }
            UtilitiesDXXL_Physics.DrawCapsulecastTillFirstHit(radius, hasHit, point1, point2, direction, maxDistance, hitInfo, nameTag, durationInSec, hiddenByNearerObjects);
            return hasHit;
        }

        public static RaycastHit[] CapsuleCastAll(Vector3 point1, Vector3 point2, float radius, Vector3 direction, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            RaycastHit[] hitInfos = Physics.CapsuleCastAll(point1, point2, radius, direction, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hitInfos; }
            int numberOfUsedSlotsInHitInfoArray = 0;
            if (hitInfos != null) { numberOfUsedSlotsInHitInfoArray = hitInfos.Length; }
            UtilitiesDXXL_Physics.DrawCapsulecastPotMultipleHits(radius, point1, point2, direction, maxDistance, hitInfos, numberOfUsedSlotsInHitInfoArray, nameTag, durationInSec, hiddenByNearerObjects);
            return hitInfos;
        }

        public static int CapsuleCastNonAlloc(Vector3 point1, Vector3 point2, float radius, Vector3 direction, RaycastHit[] results, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            int numberOfUsedSlotsInHitInfoArray = Physics.CapsuleCastNonAlloc(point1, point2, radius, direction, results, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoArray; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoArray, results);
            UtilitiesDXXL_Physics.DrawCapsulecastPotMultipleHits(radius, point1, point2, direction, maxDistance, results, resultsArrayIsNull ? 0 : numberOfUsedSlotsInHitInfoArray, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoArray;
        }

        public static bool Linecast(Vector3 start, Vector3 end, out RaycastHit hitInfo, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            bool hasHit = Physics.Linecast(start, end, out hitInfo, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hasHit; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return hasHit; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(end, "end")) { return hasHit; }
            Vector3 startToEnd = end - start;
            float length = startToEnd.magnitude;
            UtilitiesDXXL_Physics.DrawRaycastTillFirstHit(hasHit, start, startToEnd, length, hitInfo, nameTag, durationInSec, hiddenByNearerObjects);
            return hasHit;
        }

        public static bool Linecast(Vector3 start, Vector3 end, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            bool hasHit = Physics.Linecast(start, end, out RaycastHit hitInfo, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hasHit; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(start, "start")) { return hasHit; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(end, "end")) { return hasHit; }
            Vector3 startToEnd = end - start;
            float length = startToEnd.magnitude;
            UtilitiesDXXL_Physics.DrawRaycastTillFirstHit(hasHit, start, startToEnd, length, hitInfo, nameTag, durationInSec, hiddenByNearerObjects);
            return hasHit;
        }

        public static bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            bool hasHit = Physics.Raycast(ray, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hasHit; }
            UtilitiesDXXL_Physics.DrawRaycastTillFirstHit(hasHit, ray.origin, ray.direction, maxDistance, hitInfo, nameTag, durationInSec, hiddenByNearerObjects);
            return hasHit;
        }

        public static bool Raycast(Ray ray, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            bool hasHit = Physics.Raycast(ray, out RaycastHit hitInfo, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hasHit; }
            UtilitiesDXXL_Physics.DrawRaycastTillFirstHit(hasHit, ray.origin, ray.direction, maxDistance, hitInfo, nameTag, durationInSec, hiddenByNearerObjects);
            return hasHit;
        }

        public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            bool hasHit = Physics.Raycast(origin, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hasHit; }
            UtilitiesDXXL_Physics.DrawRaycastTillFirstHit(hasHit, origin, direction, maxDistance, hitInfo, nameTag, durationInSec, hiddenByNearerObjects);
            return hasHit;
        }

        public static bool Raycast(Vector3 origin, Vector3 direction, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            bool hasHit = Physics.Raycast(origin, direction, out RaycastHit hitInfo, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hasHit; }
            UtilitiesDXXL_Physics.DrawRaycastTillFirstHit(hasHit, origin, direction, maxDistance, hitInfo, nameTag, durationInSec, hiddenByNearerObjects);
            return hasHit;
        }

        public static RaycastHit[] RaycastAll(Vector3 origin, Vector3 direction, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            RaycastHit[] hitInfos = Physics.RaycastAll(origin, direction, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hitInfos; }
            int numberOfUsedSlotsInHitInfoArray = 0;
            if (hitInfos != null) { numberOfUsedSlotsInHitInfoArray = hitInfos.Length; }
            UtilitiesDXXL_Physics.DrawRaycastPotMultipleHits(origin, direction, maxDistance, hitInfos, numberOfUsedSlotsInHitInfoArray, nameTag, durationInSec, hiddenByNearerObjects);
            return hitInfos;
        }

        public static RaycastHit[] RaycastAll(Ray ray, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            RaycastHit[] hitInfos = Physics.RaycastAll(ray, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hitInfos; }
            int numberOfUsedSlotsInHitInfoArray = 0;
            if (hitInfos != null) { numberOfUsedSlotsInHitInfoArray = hitInfos.Length; }
            UtilitiesDXXL_Physics.DrawRaycastPotMultipleHits(ray.origin, ray.direction, maxDistance, hitInfos, numberOfUsedSlotsInHitInfoArray, nameTag, durationInSec, hiddenByNearerObjects);
            return hitInfos;
        }

        public static int RaycastNonAlloc(Ray ray, RaycastHit[] results, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            int numberOfUsedSlotsInHitInfoArray = Physics.RaycastNonAlloc(ray, results, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoArray; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoArray, results);
            UtilitiesDXXL_Physics.DrawRaycastPotMultipleHits(ray.origin, ray.direction, maxDistance, results, resultsArrayIsNull ? 0 : numberOfUsedSlotsInHitInfoArray, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoArray;
        }

        public static int RaycastNonAlloc(Vector3 origin, Vector3 direction, RaycastHit[] results, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            int numberOfUsedSlotsInHitInfoArray = Physics.RaycastNonAlloc(origin, direction, results, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoArray; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoArray, results);
            UtilitiesDXXL_Physics.DrawRaycastPotMultipleHits(origin, direction, maxDistance, results, resultsArrayIsNull ? 0 : numberOfUsedSlotsInHitInfoArray, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoArray;
        }

        public static bool SphereCast(Ray ray, float radius, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            bool hasHit = Physics.SphereCast(ray, radius, out RaycastHit hitInfo, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hasHit; }
            UtilitiesDXXL_Physics.DrawSpherecastTillFirstHit(radius, hasHit, ray.origin, ray.direction, maxDistance, hitInfo, nameTag, durationInSec, hiddenByNearerObjects);
            return hasHit;
        }

        public static bool SphereCast(Ray ray, float radius, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            bool hasHit = Physics.SphereCast(ray, radius, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hasHit; }
            UtilitiesDXXL_Physics.DrawSpherecastTillFirstHit(radius, hasHit, ray.origin, ray.direction, maxDistance, hitInfo, nameTag, durationInSec, hiddenByNearerObjects);
            return hasHit;
        }

        public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            bool hasHit = Physics.SphereCast(origin, radius, direction, out RaycastHit hitInfo, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hasHit; }
            UtilitiesDXXL_Physics.DrawSpherecastTillFirstHit(radius, hasHit, origin, direction, maxDistance, hitInfo, nameTag, durationInSec, hiddenByNearerObjects);
            return hasHit;
        }

        public static bool SphereCast(Vector3 origin, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            bool hasHit = Physics.SphereCast(origin, radius, direction, out hitInfo, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hasHit; }
            UtilitiesDXXL_Physics.DrawSpherecastTillFirstHit(radius, hasHit, origin, direction, maxDistance, hitInfo, nameTag, durationInSec, hiddenByNearerObjects);
            return hasHit;
        }

        public static RaycastHit[] SphereCastAll(Ray ray, float radius, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            RaycastHit[] hitInfos = Physics.SphereCastAll(ray, radius, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hitInfos; }
            int numberOfUsedSlotsInHitInfoArray = 0;
            if (hitInfos != null) { numberOfUsedSlotsInHitInfoArray = hitInfos.Length; }
            UtilitiesDXXL_Physics.DrawSpherecastPotMultipleHits(radius, ray.origin, ray.direction, maxDistance, hitInfos, numberOfUsedSlotsInHitInfoArray, nameTag, durationInSec, hiddenByNearerObjects);
            return hitInfos;
        }

        public static RaycastHit[] SphereCastAll(Vector3 origin, float radius, Vector3 direction, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            RaycastHit[] hitInfos = Physics.SphereCastAll(origin, radius, direction, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return hitInfos; }
            int numberOfUsedSlotsInHitInfoArray = 0;
            if (hitInfos != null) { numberOfUsedSlotsInHitInfoArray = hitInfos.Length; }
            UtilitiesDXXL_Physics.DrawSpherecastPotMultipleHits(radius, origin, direction, maxDistance, hitInfos, numberOfUsedSlotsInHitInfoArray, nameTag, durationInSec, hiddenByNearerObjects);
            return hitInfos;
        }

        public static int SphereCastNonAlloc(Ray ray, float radius, RaycastHit[] results, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            int numberOfUsedSlotsInHitInfoArray = Physics.SphereCastNonAlloc(ray, radius, results, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoArray; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoArray, results);
            UtilitiesDXXL_Physics.DrawSpherecastPotMultipleHits(radius, ray.origin, ray.direction, maxDistance, results, resultsArrayIsNull ? 0 : numberOfUsedSlotsInHitInfoArray, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoArray;
        }

        public static int SphereCastNonAlloc(Vector3 origin, float radius, Vector3 direction, RaycastHit[] results, float maxDistance = Mathf.Infinity, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            int numberOfUsedSlotsInHitInfoArray = Physics.SphereCastNonAlloc(origin, radius, direction, results, maxDistance, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfUsedSlotsInHitInfoArray; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfUsedSlotsInHitInfoArray, results);
            UtilitiesDXXL_Physics.DrawSpherecastPotMultipleHits(radius, origin, direction, maxDistance, results, resultsArrayIsNull ? 0 : numberOfUsedSlotsInHitInfoArray, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfUsedSlotsInHitInfoArray;
        }

        public static bool CheckBox(Vector3 center, Vector3 halfExtents, Quaternion orientation = default(Quaternion), int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            orientation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(orientation);
            bool doesOverlap = Physics.CheckBox(center, halfExtents, orientation, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return doesOverlap; }
            UtilitiesDXXL_Physics.DrawCheckedBox(doesOverlap, center, halfExtents, orientation, nameTag, durationInSec, hiddenByNearerObjects);
            return doesOverlap;
        }

        public static bool CheckCapsule(Vector3 start, Vector3 end, float radius, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            bool doesOverlap = Physics.CheckCapsule(start, end, radius, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return doesOverlap; }
            UtilitiesDXXL_Physics.DrawCheckedCapsule(doesOverlap, start, end, radius, nameTag, durationInSec, hiddenByNearerObjects);
            return doesOverlap;
        }

        public static bool CheckSphere(Vector3 position, float radius, int layerMask = Physics.DefaultRaycastLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            bool doesOverlap = Physics.CheckSphere(position, radius, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return doesOverlap; }
            UtilitiesDXXL_Physics.DrawCheckedSphere(doesOverlap, position, radius, nameTag, durationInSec, hiddenByNearerObjects);
            return doesOverlap;
        }

        public static Collider[] OverlapBox(Vector3 center, Vector3 halfExtents, Quaternion orientation = default(Quaternion), int layerMask = Physics.AllLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            orientation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(orientation);
            Collider[] overlappingColliders = Physics.OverlapBox(center, halfExtents, orientation, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return overlappingColliders; }

            bool doesOverlap = false;
            int numberOfOverlappingColliders = 0;
            if (overlappingColliders != null && overlappingColliders.Length > 0)
            {
                doesOverlap = true;
                numberOfOverlappingColliders = overlappingColliders.Length;
            }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(center, "center")) { return overlappingColliders; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(halfExtents, "halfExtents")) { return overlappingColliders; }

            float approxSize_ofOverlapVolume =UtilitiesDXXL_Math.GetAverageBoxExtent(2.0f* halfExtents);
            UtilitiesDXXL_Physics.DrawMarkersAtOverlappingColliders(center, doesOverlap, overlappingColliders, numberOfOverlappingColliders, approxSize_ofOverlapVolume, durationInSec, hiddenByNearerObjects);
            UtilitiesDXXL_Physics.DrawOverlapResultBox(doesOverlap, numberOfOverlappingColliders, overlappingColliders, center, halfExtents, orientation, nameTag, durationInSec, hiddenByNearerObjects);
            return overlappingColliders;
        }

        public static int OverlapBoxNonAlloc(Vector3 center, Vector3 halfExtents, Collider[] results, Quaternion orientation = default(Quaternion), int layerMask = Physics.AllLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            orientation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(orientation);
            int numberOfOverlappingColliders = Physics.OverlapBoxNonAlloc(center, halfExtents, results, orientation, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfOverlappingColliders; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfOverlappingColliders, results);

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(center, "center")) { return numberOfOverlappingColliders; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(halfExtents, "halfExtents")) { return numberOfOverlappingColliders; }

            int used_numberOfOverlappingColliders = resultsArrayIsNull ? 0 : numberOfOverlappingColliders;
            bool doesOverlap = (used_numberOfOverlappingColliders > 0);
            float approxSize_ofOverlapVolume = UtilitiesDXXL_Math.GetAverageBoxExtent(2.0f * halfExtents);
            UtilitiesDXXL_Physics.DrawMarkersAtOverlappingColliders(center, doesOverlap, results, used_numberOfOverlappingColliders, approxSize_ofOverlapVolume, durationInSec, hiddenByNearerObjects); ;
            UtilitiesDXXL_Physics.DrawOverlapResultBox(doesOverlap, used_numberOfOverlappingColliders, results, center, halfExtents, orientation, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfOverlappingColliders;
        }

        public static Collider[] OverlapCapsule(Vector3 point0, Vector3 point1, float radius, int layerMask = Physics.AllLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            Collider[] overlappingColliders = Physics.OverlapCapsule(point0, point1, radius, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return overlappingColliders; }

            bool doesOverlap = false;
            int numberOfOverlappingColliders = 0;
            if (overlappingColliders != null && overlappingColliders.Length > 0)
            {
                doesOverlap = true;
                numberOfOverlappingColliders = overlappingColliders.Length;
            }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return overlappingColliders; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(point0, "point0")) { return overlappingColliders; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(point1, "point1")) { return overlappingColliders; }

            Vector3 capsuleCenter = 0.5f * (point0 + point1);
            float approxSize_ofOverlapVolume = 3.0f * radius;
            UtilitiesDXXL_Physics.DrawMarkersAtOverlappingColliders(capsuleCenter, doesOverlap, overlappingColliders, numberOfOverlappingColliders, approxSize_ofOverlapVolume, durationInSec, hiddenByNearerObjects); ;
            UtilitiesDXXL_Physics.DrawOverlapResultCapsule(doesOverlap, numberOfOverlappingColliders, overlappingColliders, point0, point1, radius, nameTag, durationInSec, hiddenByNearerObjects);
            return overlappingColliders;
        }

        public static int OverlapCapsuleNonAlloc(Vector3 point0, Vector3 point1, float radius, Collider[] results, int layerMask = Physics.AllLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfOverlappingColliders = Physics.OverlapCapsuleNonAlloc(point0, point1, radius, results, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfOverlappingColliders; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfOverlappingColliders, results);

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return numberOfOverlappingColliders; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(point0, "point0")) { return numberOfOverlappingColliders; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(point1, "point1")) { return numberOfOverlappingColliders; }

            int used_numberOfOverlappingColliders = resultsArrayIsNull ? 0 : numberOfOverlappingColliders;
            bool doesOverlap = (used_numberOfOverlappingColliders > 0);
            Vector3 capsuleCenter = 0.5f * (point0 + point1);
            float approxSize_ofOverlapVolume = 3.0f * radius;
            UtilitiesDXXL_Physics.DrawMarkersAtOverlappingColliders(capsuleCenter, doesOverlap, results, used_numberOfOverlappingColliders, approxSize_ofOverlapVolume, durationInSec, hiddenByNearerObjects); ;
            UtilitiesDXXL_Physics.DrawOverlapResultCapsule(doesOverlap, used_numberOfOverlappingColliders, results, point0, point1, radius, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfOverlappingColliders;
        }

        public static Collider[] OverlapSphere(Vector3 position, float radius, int layerMask = Physics.AllLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            Collider[] overlappingColliders = Physics.OverlapSphere(position, radius, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return overlappingColliders; }

            bool doesOverlap = false;
            int numberOfOverlappingColliders = 0;
            if (overlappingColliders != null && overlappingColliders.Length > 0)
            {
                doesOverlap = true;
                numberOfOverlappingColliders = overlappingColliders.Length;
            }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return overlappingColliders; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return overlappingColliders; }

            float approxSize_ofOverlapVolume = 2.0f * radius;
            UtilitiesDXXL_Physics.DrawMarkersAtOverlappingColliders(position, doesOverlap, overlappingColliders, numberOfOverlappingColliders, approxSize_ofOverlapVolume, durationInSec, hiddenByNearerObjects); ;
            UtilitiesDXXL_Physics.DrawOverlapResultSphere(doesOverlap, numberOfOverlappingColliders, overlappingColliders, position, radius, nameTag, durationInSec, hiddenByNearerObjects);
            return overlappingColliders;
        }

        public static int OverlapSphereNonAlloc(Vector3 position, float radius, Collider[] results, int layerMask = Physics.AllLayers, QueryTriggerInteraction queryTriggerInteraction = QueryTriggerInteraction.UseGlobal, string nameTag = null, float durationInSec = 0.0f, bool hiddenByNearerObjects = false)
        {
            int numberOfOverlappingColliders = Physics.OverlapSphereNonAlloc(position, radius, results, layerMask, queryTriggerInteraction);
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return numberOfOverlappingColliders; }
            bool resultsArrayIsNull = UtilitiesDXXL_Physics.ExtentNameTagForNonSuitingResultArray(ref nameTag, numberOfOverlappingColliders, results);

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return numberOfOverlappingColliders; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return numberOfOverlappingColliders; }

            int used_numberOfOverlappingColliders = resultsArrayIsNull ? 0 : numberOfOverlappingColliders;
            bool doesOverlap = (used_numberOfOverlappingColliders > 0);
            float approxSize_ofOverlapVolume = 2.0f * radius;
            UtilitiesDXXL_Physics.DrawMarkersAtOverlappingColliders(position, doesOverlap, results, used_numberOfOverlappingColliders, approxSize_ofOverlapVolume, durationInSec, hiddenByNearerObjects); ;
            UtilitiesDXXL_Physics.DrawOverlapResultSphere(doesOverlap, used_numberOfOverlappingColliders, results, position, radius, nameTag, durationInSec, hiddenByNearerObjects);
            return numberOfOverlappingColliders;
        }

    }

}
