namespace DrawXXL
{
    public class UtilitiesDXXL_Measurements2D
    {

        static float minimumLineLength_forDistancePointToLine_before;
        public static void Set_minimumLineLength_forDistancePointToLine_reversible(float new_minimumLineLength_forDistancePointToLine)
        {
            minimumLineLength_forDistancePointToLine_before = DrawMeasurements2D.minimumLineLength_forDistancePointToLine;
            DrawMeasurements2D.minimumLineLength_forDistancePointToLine = new_minimumLineLength_forDistancePointToLine;
        }
        public static void Reverse_minimumLineLength_forDistancePointToLine()
        {
            DrawMeasurements2D.minimumLineLength_forDistancePointToLine = minimumLineLength_forDistancePointToLine_before;
        }

        static float minimumLineLength_forAngleLineToLine_before;
        public static void Set_minimumLineLength_forAngleLineToLine_reversible(float new_minimumLineLength_forAngleLineToLine)
        {
            minimumLineLength_forAngleLineToLine_before = DrawMeasurements2D.minimumLineLength_forAngleLineToLine;
            DrawMeasurements2D.minimumLineLength_forAngleLineToLine = new_minimumLineLength_forAngleLineToLine;
        }
        public static void Reverse_minimumLineLength_forAngleLineToLine()
        {
            DrawMeasurements2D.minimumLineLength_forAngleLineToLine = minimumLineLength_forAngleLineToLine_before;
        }

    }

}
