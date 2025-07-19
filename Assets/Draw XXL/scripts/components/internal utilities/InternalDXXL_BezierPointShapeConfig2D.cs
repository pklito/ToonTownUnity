namespace DrawXXL
{
    using UnityEngine;
    public struct InternalDXXL_BezierPointShapeConfig2D
    {
        public Vector2 anchorPos;
        public Vector2 direction_toForward_normalized;
        public Vector2 direction_toBackward_normalized;
        public Vector2 forwardHelperPos;
        public float absDistanceToForwardAnchorPoint;
        public Vector2 backwardHelperPos;
        public float absDistanceToBackwardAnchorPoint;
    }
}
