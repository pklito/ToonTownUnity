namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class InternalDXXL_CharConfig
    {
        public char character;
        public bool hasMissingSymbolDefinition = false;
        public bool bold = false;
        public bool italic = false;
        public bool deleted = false;
        public bool underlined = false;
        public float size;
        public float sizeScalingFactor = 1.0f;
        public Color color;
        public Vector3 pos;
        public bool strippedDueToParsing = false;
        public int numberOfChars_thatThisCharMarksAsASingleLineBreakIncludingItself = 0; //"0" means "is not a lineBreak"
        public bool isIcon = false;
        public string iconString;
        public List<Vector3> duplicatesPrintOffsets = new List<Vector3>();
        public int usedSlots_inDuplicatesPrintOffsetList;
        ///for charsOnCircle:
        public float coveredAngleDeg_onTheLineAtTheReferenceRadius;
        public float coveredAngleDegOnOwnLine;
        public bool sizeHasBeenScaledViaRichtextMarkup = false;
        public Quaternion rotationFromCircleStart;
        public Vector3 charUp;
        //public Vector3 charDirection;

        public delegate void SetCharStyleProperty(ref InternalDXXL_CharConfig charToModify);

        public static void MarkAsBold(ref InternalDXXL_CharConfig charToMark)
        {
            charToMark.bold = true;
        }

        public static void MarkAsItalic(ref InternalDXXL_CharConfig charToMark)
        {
            charToMark.italic = true;
        }

        public static void MarkAsDeleted(ref InternalDXXL_CharConfig charToMark)
        {
            charToMark.deleted = true;
        }

        public static void MarkAsUnderlined(ref InternalDXXL_CharConfig charToMark)
        {
            charToMark.underlined = true;
        }
    }

}
