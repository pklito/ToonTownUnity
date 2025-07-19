namespace DrawXXL
{
    public struct InternalDXXL_SubMeshIdentifier
    {
        public int lengthOfSubMesh_inVertices;
        public int i_startOfSubMesh_insideTheFinalVertsList;

        public enum DepthTestType { meshIsHidableBehindOtherGeometry, meshAlwaysOverlaysOtherGeometry };
        public DepthTestType depthTestType;
    }

}
