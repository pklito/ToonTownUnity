namespace DrawXXL
{
    using UnityEngine;
    public class InternalDXXL_LineParamsFromCamViewportSpace
    {
        public Vector3 startAnchor_worldSpace;
        public Vector3 endAnchor_worldSpace;
        public float width_worldSpace;
        public float animationSpeed_worldSpace;
        public DrawBasics.LineStyle lineStyleForcedTo2D;
        public float patternScaleFactor_worldSpace;
        public float endPlatesSize_inAbsoluteWorldSpaceUnits;
        public InternalDXXL_Plane camPlane = new InternalDXXL_Plane();
        public float enlargeSmallTextToThisMinTextSize_worldSpace;
    }
}
