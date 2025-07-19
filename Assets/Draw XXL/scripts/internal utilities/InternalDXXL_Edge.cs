namespace DrawXXL
{
    using UnityEngine;
    public class InternalDXXL_Edge
    {
        public Vector3 start;
        public Vector3 end;
        public InternalDXXL_Line line = new InternalDXXL_Line();

        public InternalDXXL_Edge()
        {

        }

        public InternalDXXL_Edge(Vector3 startPos, Vector3 endPos)
        {
            start = startPos;
            end = endPos;
        }

        public void Recreate(Vector3 startPos, Vector3 endPos)
        {
            start = startPos;
            end = endPos;
        }

        public void CalcLine()
        {
            line.RecreateLineFromTwoPoints(start, end);
        }

        public bool CheckIfLengthIsZero()
        {
            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(start, end))
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
