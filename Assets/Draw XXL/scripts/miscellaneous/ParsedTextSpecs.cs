namespace DrawXXL
{

    using UnityEngine;

    public class ParsedTextSpecs
    {
        public float widthOfLongestLine; //default in world units. If the drawing was exectuted using "WriteScreenspace" then it is in viewportSpaceUnits. (note that viewportSpace mostly is non-square, resulting in same distances appearing with different lenths depending on wheather they are alined in x or y direction )
        public int numberOfChars_inLongestLine; //like "numberOfChars_afterParsingOutTheMarkupTags" this only contains characters that are still there after parsing out the richtext markup tags
        public int numberOfChars_afterParsingOutTheMarkupTags;
        public float sizeOfBiggestCharInFirstLine;//default in world units.If the drawing was exectuted using "WriteScreenspace" then it is in viewportSpaceUnits. (note that viewportSpace mostly is non-square, resulting in same distances appearing with different lenths depending on wheather they are alined in x or y direction )
        public float height_wholeTextBlock; //default in world units. If the drawing was exectuted using "WriteScreenspace" then it is in viewportSpaceUnits, and then (so only for SS) might be slightly imprecise. (note that viewportSpace mostly is non-square, resulting in same distances appearing with different lenths depending on wheather they are alined in x or y direction )
        public float height_lowFirstLine_toLowLastLine; //default in world units. If the drawing was exectuted using "WriteScreenspace" then it is in viewportSpaceUnits, and then (so only for SS) might be slightly imprecise. (note that viewportSpace mostly is non-square, resulting in same distances appearing with different lenths depending on wheather they are alined in x or y direction )
        public DrawText.TextAnchorDXXL usedTextAnchor; //may get unintuitively changed if "WriteScreenSpace(...autoFlipTextToPreventUpsideDown...)" is used
        public Vector3 lowLeftPos_ofFirstLine; //may get unintuitively shifted if "WriteScreenSpace(...autoFlipTextToPreventUpsideDown...)" is used
        public Vector3 used_textDirection_normalized; //If you didn't specify the textDirection/text-rotation by yourself then this is the direction that has been finally used by the automatic orientation (see "DrawText.automaticTextOrientation"(link)). This can be useful, if you want to draw automatically oriented text and then align other shapes relative to the text in an aligned layout. If you are drawing in screenspace then this is still in worldspace units, not in screenspace units, so you get a vector, that lies somehow in the camera plane.
        public Vector3 used_textUp_normalized; //If you didn't specify the textUp-direction/text-rotation by yourself then this is the up direction that has been finally used by the automatic orientation (see "DrawText.automaticTextOrientation"(link)). This can be useful, if you want to draw automatically oriented text and then align other shapes relative to the text in an aligned layout. If you are drawing in screenspace then this is still in worldspace units, not in screenspace units, so you get a vector, that lies somehow in the camera plane.
        public Vector3 lowLeftPos_ofEnclosingBox; //only filled for "boxStyle != invisible" 
        public Vector3 lowRightPos_ofEnclosingBox; //only filled for "boxStyle != invisible" 
        public Vector3 upperLeftPos_ofEnclosingBox; //only filled for "boxStyle != invisible" 
        public Vector3 upperRightPos_ofEnclosingBox; //only filled for "boxStyle != invisible" 

        public ParsedTextSpecs GetCopy()
        {
            ParsedTextSpecs copiedSpecs = new ParsedTextSpecs();
            copiedSpecs.widthOfLongestLine = widthOfLongestLine;
            copiedSpecs.numberOfChars_inLongestLine = numberOfChars_inLongestLine;
            copiedSpecs.numberOfChars_afterParsingOutTheMarkupTags = numberOfChars_afterParsingOutTheMarkupTags;
            copiedSpecs.sizeOfBiggestCharInFirstLine = sizeOfBiggestCharInFirstLine;
            copiedSpecs.height_wholeTextBlock = height_wholeTextBlock;
            copiedSpecs.height_lowFirstLine_toLowLastLine = height_lowFirstLine_toLowLastLine;
            copiedSpecs.usedTextAnchor = usedTextAnchor;
            copiedSpecs.lowLeftPos_ofFirstLine = lowLeftPos_ofFirstLine;
            copiedSpecs.used_textDirection_normalized = used_textDirection_normalized;
            copiedSpecs.used_textUp_normalized = used_textUp_normalized;
            copiedSpecs.lowLeftPos_ofEnclosingBox = lowLeftPos_ofEnclosingBox;
            copiedSpecs.lowRightPos_ofEnclosingBox = lowRightPos_ofEnclosingBox;
            copiedSpecs.upperLeftPos_ofEnclosingBox = upperLeftPos_ofEnclosingBox;
            copiedSpecs.upperRightPos_ofEnclosingBox = upperRightPos_ofEnclosingBox;
            return copiedSpecs;
        }

        public Vector3 RightEndOfWholeTextBlock_atLowEndOfFirstLine
        {
            //note that the y-value of the returned positon is at the low end of the first line, but the first line is not necessarly the longest line. The x-value is defined by the longest line.
            get { return (lowLeftPos_ofFirstLine + used_textDirection_normalized * widthOfLongestLine); }
            set { }
        }

    }

}
