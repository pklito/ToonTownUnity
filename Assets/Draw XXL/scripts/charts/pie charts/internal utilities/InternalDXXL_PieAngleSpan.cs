namespace DrawXXL
{
    public struct InternalDXXL_PieAngleSpan
    {
        public float startAngleDegCCFromUp;
        public float endAngleDegCCFromUp;

        public bool DoesIntersect(float startAngleDegCCFromUp_ofSpanThatIsCheckedIfItIntersectsWithThisSpan, float endAngleDegCCFromUp_ofSpanThatIsCheckedIfItIntersectsWithThisSpan)
        {
            //This function expects:
            //-> startAngleDegCCFromUp is smaller than endAngleDegCCFromUp
            //-> at least one angle lies between 0 and 360, and the other angle is not looped into this span, but is either negative or bigger thatn 360
            //(both is guaranteed by "PieChartSegment.PrepareTextDrawing")

            if (startAngleDegCCFromUp_ofSpanThatIsCheckedIfItIntersectsWithThisSpan < startAngleDegCCFromUp)
            {
                if (endAngleDegCCFromUp_ofSpanThatIsCheckedIfItIntersectsWithThisSpan < startAngleDegCCFromUp)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (startAngleDegCCFromUp_ofSpanThatIsCheckedIfItIntersectsWithThisSpan < endAngleDegCCFromUp)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
