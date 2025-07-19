namespace DrawXXL
{
    public class ParsedTextOnCircleSpecs
    {
        public float angleDegOfLongestLine;
        public int numberOfChars_inLongestLine; //like "numberOfChars_afterParsingOutTheMarkupTags" this only contains characters that are still there after parsing out the richtext markup tags
        public int numberOfChars_afterParsingOutTheMarkupTags;
        public float sizeOfBiggestCharInFirstLine;

        public ParsedTextOnCircleSpecs GetCopy()
        {
            ParsedTextOnCircleSpecs copiedSpecs = new ParsedTextOnCircleSpecs();
            copiedSpecs.angleDegOfLongestLine = angleDegOfLongestLine;
            copiedSpecs.numberOfChars_inLongestLine = numberOfChars_inLongestLine;
            copiedSpecs.numberOfChars_afterParsingOutTheMarkupTags = numberOfChars_afterParsingOutTheMarkupTags;
            copiedSpecs.sizeOfBiggestCharInFirstLine = sizeOfBiggestCharInFirstLine;//default in world units. If the drawing was exectuted using "WriteScreenspace" then it is relative to the viewport height, and might then by slighly inaccurate.
            return copiedSpecs;
        }
    }

}
