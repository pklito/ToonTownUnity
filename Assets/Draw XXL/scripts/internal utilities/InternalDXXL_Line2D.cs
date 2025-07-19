namespace DrawXXL
{
    using UnityEngine;


    public class InternalDXXL_Line2D
    {
        public float m;
        public float t;

        public InternalDXXL_Line2D()
        {
            m = 1.0f;
            t = 0.0f;
        }

        public void Recalc_line_throughTwoPoints_notVertLineProof(Vector2 firstPoint, Vector2 secondPoint)
        {
            m = (secondPoint.y - firstPoint.y) / (secondPoint.x - firstPoint.x);
            t = firstPoint.y - m * firstPoint.x;
        }

        public void Recalc_line_throughTwoPoints_returnSteepForVertLines(Vector2 firstPoint, Vector2 secondPoint)
        {
            float delta_x = secondPoint.x - firstPoint.x;
            if (UtilitiesDXXL_Math.ApproximatelyZero(delta_x))
            {
                m = 1000000.0f;
            }
            else
            {
                m = (secondPoint.y - firstPoint.y) / delta_x;
            }

            t = firstPoint.y - m * firstPoint.x;
        }




        public float GetYatX(float givenX)
        {
            return (m * givenX + t);
        }

        public float GetXatY(float givenY)
        {
            return (givenY - t) / m;
        }

        InternalDXXL_Line2D perpLineThroughGivenPoint;
        public Vector2 GetProjectionOfPointOntoLine(Vector2 pointToProject)
        {
            if (perpLineThroughGivenPoint == null)
            {
                perpLineThroughGivenPoint = Create_perpendicularLine_throughPoint_proofForGivenLineWithMOfZero(this, pointToProject);
            }
            else
            {
                perpLineThroughGivenPoint.Recalc_perpendicularLine_throughPoint_proofForGivenLineWithMOfZero(this, pointToProject);
            }
            return Get_intersectionPoint_ofTwoLines_notProofForParallel(this, perpLineThroughGivenPoint);
        }

        public static InternalDXXL_Line2D Create_perpendicularLine_throughPoint_proofForGivenLineWithMOfZero(InternalDXXL_Line2D line_perpendicularToResultingLine, Vector2 throughtThisPoint)
        {
            InternalDXXL_Line2D line = new InternalDXXL_Line2D();
            line.m = Get_perpendicular_m_returnsSteepForZeros(line_perpendicularToResultingLine.m);
            line.t = Get_t_ofLineThruPoint(throughtThisPoint, line.m);
            return line;
        }

        public void Recalc_perpendicularLine_throughPoint_proofForGivenLineWithMOfZero(InternalDXXL_Line2D line_perpendicularToResultingLine, Vector2 throughtThisPoint)
        {
            m = Get_perpendicular_m_returnsSteepForZeros(line_perpendicularToResultingLine.m);
            t = Get_t_ofLineThruPoint(throughtThisPoint, m);
        }

        public static float Get_perpendicular_m_returnsSteepForZeros(float given_m)
        {
            if (UtilitiesDXXL_Math.ApproximatelyZero(given_m))
            {
                return (100000.0f);
            }
            else
            {
                return (-1.0f / given_m);
            }
        }

        public static float Get_t_ofLineThruPoint(Vector2 givenPoint, float given_m)
        {
            return (givenPoint.y - given_m * givenPoint.x);
        }

        public static Vector2 Get_intersectionPoint_ofTwoLines_notProofForParallel(InternalDXXL_Line2D firstLine, InternalDXXL_Line2D secondLine)
        {
            Vector2 intersectionPoint = new Vector2();
            intersectionPoint.x = (secondLine.t - firstLine.t) / (firstLine.m - secondLine.m);
            intersectionPoint.y = firstLine.m * intersectionPoint.x + firstLine.t;
            return intersectionPoint;
        }

    }

}
