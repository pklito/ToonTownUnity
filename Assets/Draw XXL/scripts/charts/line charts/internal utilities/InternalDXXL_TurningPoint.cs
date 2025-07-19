namespace DrawXXL
{
    public class InternalDXXL_TurningPoint //This is "class" instead of "struct" because it should be nullable
    {
        public PointOfInterest pointOfInterest_thatRepresentsThisTurningPoint;
        public bool isTheEndOfAPlateau;
        public bool isTheMostRightOf_theNonPlataueEndTurningPointsAtSameYHeight;
    }
}
