namespace DrawXXL
{
    using UnityEngine;
    public struct InternalDXXL_BezierPointShapeConfig
    {
        public Vector3 anchorPos;
        public Vector3 direction_toForward_normalized;
        public Vector3 direction_toBackward_normalized;
        public Vector3 forwardHelperPos;
        public float absDistanceToForwardAnchorPoint;
        public Vector3 backwardHelperPos;
        public float absDistanceToBackwardAnchorPoint;
    }
}
