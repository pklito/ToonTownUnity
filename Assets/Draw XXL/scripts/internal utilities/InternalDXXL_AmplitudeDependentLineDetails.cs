namespace DrawXXL
{
    using UnityEngine;
    public struct InternalDXXL_AmplitudeDependentLineDetails
    {
        public float lineWidth;
        public bool isThinLine;
        public bool enlargeSmallText;
        public bool textDrawingIsSkipped_dueToLineIsTooShort;
        public DrawBasics.LineStyle style;
        public bool uses_endPlates;
        public float endPlates_size;
        public Vector3 amplitudeUp_normalized;
        public Vector3 textDir_normalized;
        public bool lengthOfDrawnLine_isFilled;
        public float lengthOfDrawnLine;
    }
}
