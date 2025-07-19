namespace DrawXXL
{
    using UnityEngine;
    
    public class DrawShapes
    {
        public enum Shape2DType { square, triangle, pentagon, hexagon, septagon, octagon, decagon, circle, circle4struts, star3, star4, star5, star6, star8, star10, star16, star32, star64, ellipse05, ellipse025, ellipse0125 };
        public enum PlaneNormalFromTransform { right, up, forward, left, down, back };

        public enum AutomaticOrientationOfFlatShapes
        {
            screen, 
            screen_butVerticalInWorldSpace,  
            xyPlane,
            xzPlane,
            zyPlane
        }
        public static AutomaticOrientationOfFlatShapes automaticOrientationOfFlatShapes = AutomaticOrientationOfFlatShapes.screen;
        public static float forcedConstantScreenspaceTextSize_relToScreenHeight_forTextAtShapes = 0.0f;
        public static float forcedConstantWorldspaceTextSize_forTextAtShapes = 0.0f;

        private static float shapeFillDensity = 1.0f;
        public static float ShapeFillDensity  
        {
            get { return shapeFillDensity; }
            set
            {
                shapeFillDensity = value;
                shapeFillDensity = Mathf.Abs(shapeFillDensity);
                shapeFillDensity = Mathf.Max(shapeFillDensity, 0.01f);
            }
        }

        private static int linesPerSphereCircle = 64;
        public static int LinesPerSphereCircle
        {
            get { return linesPerSphereCircle; }
            set
            {
                if ((value == 64) || (value == 32) || (value == 16) || (value == 8))
                {
                    linesPerSphereCircle = value;
                }
                else
                {
                    Debug.LogError("It is not supported to set 'LinesPerSphereAndCapsuleCircle' to anything other than 64, 32, 16 or 8.");
                }
            }
        }

        public static int RegularPolygon(int corners, Vector3 centerPosition, float hullRadius = 0.5f, Color color = default(Color), Quaternion rotation = default(Quaternion), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            UtilitiesDXXL_Shapes.ConvertQuaternionToFlatShapesNormalAndUpInsideFlatPlane(out Vector3 normal, out Vector3 up_insideFlatPlane, rotation);
            return RegularPolygon(corners, centerPosition, hullRadius, color, normal, up_insideFlatPlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }
        public static int RegularPolygon(int corners, Vector3 centerPosition, float hullRadius, Color color, Vector3 normal, Vector3 up_insidePolygonPlane = default(Vector3), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(hullRadius, "hullRadius")) { return 0; }
            return UtilitiesDXXL_Shapes.DrawFlatPolygon(0.0f, corners, centerPosition, Mathf.Abs(hullRadius), normal, up_insidePolygonPlane, color, lineWidth, text, durationInSec, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, hiddenByNearerObjects, textBlockAboveLine, false);
        }

        public static int Triangle(Vector3 centerPosition, float hullRadius = 0.5f, Color color = default(Color), Quaternion rotation = default(Quaternion), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            UtilitiesDXXL_Shapes.ConvertQuaternionToFlatShapesNormalAndUpInsideFlatPlane(out Vector3 normal, out Vector3 up_insideFlatPlane, rotation);
            return Triangle(centerPosition, hullRadius, color, normal, up_insideFlatPlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }
        public static int Triangle(Vector3 centerPosition, float hullRadius, Color color, Vector3 normal, Vector3 up_insideTrianglePlane = default(Vector3), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            return UtilitiesDXXL_Shapes.Triangle(centerPosition, hullRadius, color, normal, up_insideTrianglePlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects, false);
        }

        public static int Square(Vector3 centerPosition, float sideLength = 1.0f, Color color = default(Color), Quaternion rotation = default(Quaternion), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            UtilitiesDXXL_Shapes.ConvertQuaternionToFlatShapesNormalAndUpInsideFlatPlane(out Vector3 normal, out Vector3 up_insideFlatPlane, rotation);
            return Square(centerPosition, sideLength, color, normal, up_insideFlatPlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }
        public static int Square(Vector3 centerPosition, float sideLength, Color color, Vector3 normal, Vector3 up_insideSquarePlane = default(Vector3), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            return UtilitiesDXXL_Shapes.Square(centerPosition, sideLength, color, normal, up_insideSquarePlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects, false);
        }

        public static int Pentagon(Vector3 centerPosition, float hullRadius = 0.5f, Color color = default(Color), Quaternion rotation = default(Quaternion), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            UtilitiesDXXL_Shapes.ConvertQuaternionToFlatShapesNormalAndUpInsideFlatPlane(out Vector3 normal, out Vector3 up_insideFlatPlane, rotation);
            return Pentagon(centerPosition, hullRadius, color, normal, up_insideFlatPlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }
        public static int Pentagon(Vector3 centerPosition, float hullRadius, Color color, Vector3 normal, Vector3 up_insidePentagonPlane = default(Vector3), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            return UtilitiesDXXL_Shapes.Pentagon(centerPosition, hullRadius, color, normal, up_insidePentagonPlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects, false);
        }

        public static int Hexagon(Vector3 centerPosition, float hullRadius = 0.5f, Color color = default(Color), Quaternion rotation = default(Quaternion), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            UtilitiesDXXL_Shapes.ConvertQuaternionToFlatShapesNormalAndUpInsideFlatPlane(out Vector3 normal, out Vector3 up_insideFlatPlane, rotation);
            return Hexagon(centerPosition, hullRadius, color, normal, up_insideFlatPlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }
        public static int Hexagon(Vector3 centerPosition, float hullRadius, Color color, Vector3 normal, Vector3 up_insideHexagonPlane = default(Vector3), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            return UtilitiesDXXL_Shapes.Hexagon(centerPosition, hullRadius, color, normal, up_insideHexagonPlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects, false);
        }

        public static int Septagon(Vector3 centerPosition, float hullRadius = 0.5f, Color color = default(Color), Quaternion rotation = default(Quaternion), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            UtilitiesDXXL_Shapes.ConvertQuaternionToFlatShapesNormalAndUpInsideFlatPlane(out Vector3 normal, out Vector3 up_insideFlatPlane, rotation);
            return Septagon(centerPosition, hullRadius, color, normal, up_insideFlatPlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }
        public static int Septagon(Vector3 centerPosition, float hullRadius, Color color, Vector3 normal, Vector3 up_insideSeptagonPlane = default(Vector3), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            return UtilitiesDXXL_Shapes.Septagon(centerPosition, hullRadius, color, normal, up_insideSeptagonPlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects, false);
        }

        public static int Octagon(Vector3 centerPosition, float hullRadius = 0.5f, Color color = default(Color), Quaternion rotation = default(Quaternion), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            UtilitiesDXXL_Shapes.ConvertQuaternionToFlatShapesNormalAndUpInsideFlatPlane(out Vector3 normal, out Vector3 up_insideFlatPlane, rotation);
            return Octagon(centerPosition, hullRadius, color, normal, up_insideFlatPlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }
        public static int Octagon(Vector3 centerPosition, float hullRadius, Color color, Vector3 normal, Vector3 up_insideOctagonPlane = default(Vector3), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            return UtilitiesDXXL_Shapes.Octagon(centerPosition, hullRadius, color, normal, up_insideOctagonPlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects, false);
        }

        public static int Decagon(Vector3 centerPosition, float hullRadius = 0.5f, Color color = default(Color), Quaternion rotation = default(Quaternion), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            UtilitiesDXXL_Shapes.ConvertQuaternionToFlatShapesNormalAndUpInsideFlatPlane(out Vector3 normal, out Vector3 up_insideFlatPlane, rotation);
            return Decagon(centerPosition, hullRadius, color, normal, up_insideFlatPlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }
        public static int Decagon(Vector3 centerPosition, float hullRadius, Color color, Vector3 normal, Vector3 up_insideDecagonPlane = default(Vector3), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            return UtilitiesDXXL_Shapes.Decagon(centerPosition, hullRadius, color, normal, up_insideDecagonPlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects, false);
        }

        public static int Circle(Vector3 centerPosition, float radius = 0.5f, Color color = default(Color), Quaternion rotation = default(Quaternion), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            UtilitiesDXXL_Shapes.ConvertQuaternionToFlatShapesNormalAndUpInsideFlatPlane(out Vector3 normal, out Vector3 up_insideFlatPlane, rotation);
            return Circle(centerPosition, radius, color, normal, up_insideFlatPlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static int Circle(Vector3 centerPosition, float radius, Color color, Vector3 normal, Vector3 up_insideCirclePlane = default(Vector3), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            return UtilitiesDXXL_Shapes.Circle(centerPosition, radius, color, normal, up_insideCirclePlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects, false);
        }

        public static int Ellipse(Vector3 centerPosition, float radiusSideward = 0.25f, float radiusUpward = 0.5f, Color color = default(Color), Quaternion rotation = default(Quaternion), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            UtilitiesDXXL_Shapes.ConvertQuaternionToFlatShapesNormalAndUpInsideFlatPlane(out Vector3 normal, out Vector3 up_insideFlatPlane, rotation);
            return Ellipse(centerPosition, radiusSideward, radiusUpward, color, normal, up_insideFlatPlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static int Ellipse(Vector3 centerPosition, float radiusSideward, float radiusUpward, Color color, Vector3 normal, Vector3 up_insideEllipsePlane = default(Vector3), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            return UtilitiesDXXL_Shapes.Ellipse(centerPosition, radiusSideward, radiusUpward, color, normal, up_insideEllipsePlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects, false);
        }

        public static int Star(Vector3 centerPosition, float outerRadius = 0.5f, Color color = default(Color), int corners = 5, float innerRadiusFactor = 0.5f, Quaternion rotation = default(Quaternion), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            UtilitiesDXXL_Shapes.ConvertQuaternionToFlatShapesNormalAndUpInsideFlatPlane(out Vector3 normal, out Vector3 up_insideFlatPlane, rotation);
            return Star(centerPosition, outerRadius, color, corners, innerRadiusFactor, normal, up_insideFlatPlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static int Star(Vector3 centerPosition, float outerRadius, Color color, int corners, float innerRadiusFactor, Vector3 normal, Vector3 up_insideStarPlane = default(Vector3), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            return UtilitiesDXXL_Shapes.Star(centerPosition, outerRadius, color, corners, innerRadiusFactor, normal, up_insideStarPlane, lineWidth, text, outlineStyle, stylePatternScaleFactor, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects, false);
        }

        public static int FlatCapsule(Vector3 posOfCircle1, Vector3 posOfCircle2, float radius = 0.5f, Color color = default(Color), Vector3 normal = default(Vector3), float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            return UtilitiesDXXL_Shapes.FlatCapsule(posOfCircle1, posOfCircle2, radius, color, normal, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static int FlatCapsule(Vector3 centerPosition, float width = 0.5f, float height = 1.0f, Color color = default(Color), Quaternion rotation = default(Quaternion), CapsuleDirection2D direction = CapsuleDirection2D.Vertical, float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            UtilitiesDXXL_Shapes.ConvertQuaternionToFlatShapesNormalAndUpInsideFlatPlane(out Vector3 normal, out Vector3 up_insideFlatPlane, rotation);
            return FlatCapsule(centerPosition, width, height, color, normal, up_insideFlatPlane, direction, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static int FlatCapsule(Vector3 centerPosition, float width, float height, Color color, Vector3 normal, Vector3 upAlongVert_insideCapsulePlane = default(Vector3), CapsuleDirection2D direction = CapsuleDirection2D.Vertical, float lineWidth = 0.0f, string text = null, DrawBasics.LineStyle outlineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            return UtilitiesDXXL_Shapes.FlatCapsule(centerPosition, width, height, color, normal, upAlongVert_insideCapsulePlane, direction, lineWidth, text, outlineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Plane(Transform planeTransform, PlaneNormalFromTransform normal = PlaneNormalFromTransform.up, Vector3 planeAreaExtentionPosition = default(Vector3), Color color = default(Color), float widthFactor = 10.0f, float lengthFactor = 10.0f, float linesWidth = 0.0f, string text = null, float subSegments_signFlipsInterpretation = 10.0f, bool pointer_as_textAttachStyle = false, float anchorVisualizationSize = 0.0f, bool drawPlumbLine_fromExtentionPosition = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(planeTransform, "planeTransform")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(widthFactor, "widthFactor")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(lengthFactor, "lengthFactor")) { return; }

            Vector3 normal_asV3 = UtilitiesDXXL_Shapes.GetPlaneNormalFromTransformEnum(planeTransform, normal);
            Vector3 forward_insidePlane = UtilitiesDXXL_Shapes.Get_forwardInsidePlane_FromPlameTransformEnum(planeTransform, normal);
            float width = widthFactor * UtilitiesDXXL_Shapes.Get_width_FromPlameTransformEnum(planeTransform, normal);
            float length = lengthFactor * UtilitiesDXXL_Shapes.Get_length_FromPlameTransformEnum(planeTransform, normal);
            Plane(planeTransform.position, normal_asV3, planeAreaExtentionPosition, color, width, length, forward_insidePlane, linesWidth, text, subSegments_signFlipsInterpretation, pointer_as_textAttachStyle, anchorVisualizationSize, drawPlumbLine_fromExtentionPosition, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Plane(Plane plane, Vector3 drawPositionApproximately, Color color = default(Color), float width = 10.0f, float length = 10.0f, Vector3 forward_insidePlane = default(Vector3), float linesWidth = 0.0f, string text = null, float subSegments_signFlipsInterpretation = 10.0f, bool pointer_as_textAttachStyle = false, bool drawPlumbLine_fromApproximateDrawPosition = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(drawPositionApproximately, "drawPositionApproximately")) { return; }

            Vector3 closestPointOnPlane = plane.ClosestPointOnPlane(drawPositionApproximately);
            float anchorVisualizationSize = 0.0f; //-> this overload doesn't need the "anchorVisualization", since there is no "planeAreaExtentionPosition" that could vary the plane extent. "drawPlumbLine_fromApproximateDrawPosition" already shows the mounting point.
            Plane(closestPointOnPlane, plane.normal, drawPositionApproximately, color, width, length, forward_insidePlane, linesWidth, text, subSegments_signFlipsInterpretation, pointer_as_textAttachStyle, anchorVisualizationSize, drawPlumbLine_fromApproximateDrawPosition, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        static InternalDXXL_Plane plane_toDraw = new InternalDXXL_Plane();
        public static void Plane(Vector3 planeMountingPoint, Vector3 normal = default(Vector3), Vector3 planeAreaExtentionPosition = default(Vector3), Color color = default(Color), float width = 10.0f, float length = 10.0f, Vector3 forward_insidePlane = default(Vector3), float linesWidth = 0.0f, string text = null, float subSegments_signFlipsInterpretation = 10.0f, bool pointer_as_textAttachStyle = false, float anchorVisualizationSize = 0.0f, bool drawPlumbLine_fromExtentionPosition = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(planeMountingPoint, "planeMountingPoint")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(planeAreaExtentionPosition, "planeAreaExtentionPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(normal, "normal")) { return; }

            if (UtilitiesDXXL_Math.IsDefaultVector(planeAreaExtentionPosition))
            {
                UtilitiesDXXL_Shapes.Plane(planeMountingPoint, default(Vector3), normal, color, width, length, forward_insidePlane, linesWidth, text, subSegments_signFlipsInterpretation, pointer_as_textAttachStyle, anchorVisualizationSize, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width, "width")) { return; }
                if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(length, "length")) { return; }

                normal = UtilitiesDXXL_Math.OverwriteDefaultVectors(normal, Vector3.up);
                plane_toDraw.Recreate(planeMountingPoint, normal);
                Vector3 closestPointOnPlane = plane_toDraw.Get_perpProjectionOfPointOnPlane(planeAreaExtentionPosition);

                if (drawPlumbLine_fromExtentionPosition)
                {
                    float sphereSize = 0.005f * Mathf.Min(width, length);
                    Color plumbColor = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, 0.4f);
                    Sphere(closestPointOnPlane, sphereSize, plumbColor, normal, forward_insidePlane, 0.0f, null, 2, false, lineStyle, stylePatternScaleFactor, false, false, durationInSec, hiddenByNearerObjects);
                    Sphere(planeAreaExtentionPosition, sphereSize, plumbColor, normal, forward_insidePlane, 0.0f, null, 2, false, lineStyle, stylePatternScaleFactor, false, false, durationInSec, hiddenByNearerObjects);
                    Line_fadeableAnimSpeed.InternalDraw(planeAreaExtentionPosition, closestPointOnPlane, plumbColor, 0.0f, null, DrawBasics.LineStyle.dashedLong, 3.0f, 0.0f, null, default, false, 0.0f, 0.0f, 0.0f, durationInSec, hiddenByNearerObjects, false, false);
                }

                UtilitiesDXXL_Shapes.Plane(planeMountingPoint, closestPointOnPlane, normal, color, width, length, forward_insidePlane, linesWidth, text, subSegments_signFlipsInterpretation, pointer_as_textAttachStyle, anchorVisualizationSize, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void RhombusAroundCenter(Vector3 centerPosition, Vector3 firstEdge, Vector3 secondEdge, Color color = default(Color), float linesWidth = 0.0f, string text = null, int subSegments = 10, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(centerPosition, "centerPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(firstEdge, "firstEdge")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(secondEdge, "secondEdge")) { return; }

            Vector3 startCornerPosition = centerPosition - 0.5f * firstEdge - 0.5f * secondEdge;
            Rhombus(startCornerPosition, firstEdge, secondEdge, color, linesWidth, text, subSegments, lineStyle, stylePatternScaleFactor, durationInSec, hiddenByNearerObjects);
        }

        public static void Rhombus(Vector3 startCornerPosition, Vector3 firstEdge, Vector3 secondEdge, Color color = default(Color), float linesWidth = 0.0f, string text = null, int subSegments = 10, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Shapes.Rhombus(startCornerPosition, firstEdge, secondEdge, color, linesWidth, text, subSegments, lineStyle, stylePatternScaleFactor, durationInSec, hiddenByNearerObjects);
        }

        public static int Cube(Transform transform, Color color = default(Color), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transform, "transform")) { return 0; }
            return Cube(transform.position, transform.lossyScale, color, transform.up, transform.forward, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static int Cube(Vector3 position, Vector3 scale, Color color = default(Color), Quaternion rotation = default(Quaternion), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            return UtilitiesDXXL_Shapes.Cube(position, scale, color, color, rotation * Vector3.up, rotation * Vector3.forward, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects, false, null);
        }

        public static int Cube(Vector3 position, Vector3 scale, Color color, Vector3 up, Vector3 forward, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            return UtilitiesDXXL_Shapes.Cube(position, scale, color, color, up, forward, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects, false, null);
        }

        public static void CubeFilled(Transform transform, Color color = default(Color), float alphaFactor_forInnerLines = 0.3f, float linesWidthOfEdges = 0.0f, int segmentsPerSide = 6, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(alphaFactor_forInnerLines, "alphaFactor_forInnerLines")) { return; }

            color = UtilitiesDXXL_Colors.OverwriteDefaultColor(color);
            Color colorOfInnerFrames = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, alphaFactor_forInnerLines);
            CubeFilled(transform, colorOfInnerFrames, 0.0f, segmentsPerSide, text, lineStyle, color, linesWidthOfEdges, stylePatternScaleFactor, true, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void CubeFilled(Vector3 position, Vector3 scale, Color color = default(Color), float alphaFactor_forInnerLines = 0.3f, Quaternion rotation = default(Quaternion), float linesWidthOfEdges = 0.0f, int segmentsPerSide = 6, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(alphaFactor_forInnerLines, "alphaFactor_forInnerLines")) { return; }

            color = UtilitiesDXXL_Colors.OverwriteDefaultColor(color);
            Color colorOfInnerFrames = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, alphaFactor_forInnerLines);
            CubeFilled(position, scale, colorOfInnerFrames, rotation, 0.0f, segmentsPerSide, text, lineStyle, color, linesWidthOfEdges, stylePatternScaleFactor, true, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void CubeFilled(Vector3 position, Vector3 scale, Color color, float alphaFactor_forInnerLines, Vector3 up, Vector3 forward, float linesWidthOfEdges = 0.0f, int segmentsPerSide = 6, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(alphaFactor_forInnerLines, "alphaFactor_forInnerLines")) { return; }

            color = UtilitiesDXXL_Colors.OverwriteDefaultColor(color);
            Color colorOfInnerFrames = UtilitiesDXXL_Colors.Get_color_butWithAdjustedAlpha(color, alphaFactor_forInnerLines);
            CubeFilled(position, scale, colorOfInnerFrames, up, forward, 0.0f, segmentsPerSide, text, lineStyle, color, linesWidthOfEdges, stylePatternScaleFactor, true, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void CubeFilled(Transform transform, Color color, float linesWidth, int segmentsPerSide, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, Color colorOfEdges = default(Color), float linesWidthOfEdges = 0.01f, float stylePatternScaleFactor = 1.0f, bool useEdgesColorAsTextColor_ifAvailable = true, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transform, "transform")) { return; }
            CubeFilled(transform.position, transform.lossyScale, color, transform.up, transform.forward, linesWidth, segmentsPerSide, text, lineStyle, colorOfEdges, linesWidthOfEdges, stylePatternScaleFactor, useEdgesColorAsTextColor_ifAvailable, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void CubeFilled(Vector3 position, Vector3 scale, Color color, Quaternion rotation, float linesWidth = 0.0f, int segmentsPerSide = 6, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, Color colorOfEdges = default(Color), float linesWidthOfEdges = 0.01f, float stylePatternScaleFactor = 1.0f, bool useEdgesColorAsTextColor_ifAvailable = true, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            CubeFilled(position, scale, color, rotation * Vector3.up, rotation * Vector3.forward, linesWidth, segmentsPerSide, text, lineStyle, colorOfEdges, linesWidthOfEdges, stylePatternScaleFactor, useEdgesColorAsTextColor_ifAvailable, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void CubeFilled(Vector3 position, Vector3 scale, Color color, Vector3 up, Vector3 forward, float linesWidth = 0.0f, int segmentsPerSide = 6, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, Color colorOfEdges = default(Color), float linesWidthOfEdges = 0.01f, float stylePatternScaleFactor = 1.0f, bool useEdgesColorAsTextColor_ifAvailable = true, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Shapes.CubeFilled(position, scale, color, up, forward, linesWidth, segmentsPerSide, text, lineStyle, colorOfEdges, linesWidthOfEdges, stylePatternScaleFactor, useEdgesColorAsTextColor_ifAvailable, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static int Sphere(Transform transform, Color color = default(Color), float linesWidth = 0.0f, string text = null, int struts = 2, bool onlyUpperHalf = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool skipDrawingEquator = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transform, "transform")) { return 0; }
            return Sphere(transform.position, 0.5f * UtilitiesDXXL_Math.GetBiggestAbsComponent(transform.lossyScale), color, transform.rotation * Vector3.up, transform.rotation * Vector3.forward, linesWidth, text, struts, onlyUpperHalf, lineStyle, stylePatternScaleFactor, skipDrawingEquator, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static int Sphere(Vector3 position, float radius = 0.5f, Color color = default(Color), Quaternion rotation = default(Quaternion), float linesWidth = 0.0f, string text = null, int struts = 2, bool onlyUpperHalf = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool skipDrawingEquator = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            return Sphere(position, radius, color, rotation * Vector3.up, rotation * Vector3.forward, linesWidth, text, struts, onlyUpperHalf, lineStyle, stylePatternScaleFactor, skipDrawingEquator, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static int Sphere(Vector3 position, float radius, Color color, Vector3 up, Vector3 forward, float linesWidth = 0.0f, string text = null, int struts = 2, bool onlyUpperHalf = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool skipDrawingEquator = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            return UtilitiesDXXL_Shapes.Sphere(position, radius, color, up, forward, linesWidth, text, struts, onlyUpperHalf, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects, skipDrawingEquator);
        }

        public static int Ellipsoid(Transform transform, Color color = default(Color), float linesWidth = 0.0f, string text = null, int struts = 2, bool onlyUpperHalf = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool skipDrawingEquator = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transform, "transform")) { return 0; }
            return Ellipsoid(transform.position, transform.lossyScale, color, transform.rotation * Vector3.up, transform.rotation * Vector3.forward, linesWidth, text, struts, onlyUpperHalf, lineStyle, stylePatternScaleFactor, skipDrawingEquator, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static int Ellipsoid(Vector3 position, Vector3 radius, Color color = default(Color), Quaternion rotation = default(Quaternion), float linesWidth = 0.0f, string text = null, int struts = 2, bool onlyUpperHalf = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool skipDrawingEquator = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            return Ellipsoid(position, radius, color, rotation * Vector3.up, rotation * Vector3.forward, linesWidth, text, struts, onlyUpperHalf, lineStyle, stylePatternScaleFactor, skipDrawingEquator, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static int Ellipsoid(Vector3 position, Vector3 radius, Color color, Vector3 up, Vector3 forward, float linesWidth = 0.0f, string text = null, int struts = 2, bool onlyUpperHalf = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool skipDrawingEquator = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            return UtilitiesDXXL_Shapes.Ellipsoid(position, radius, color, up, forward, linesWidth, text, struts, onlyUpperHalf, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects, skipDrawingEquator);
        }

        public static void EllipsoidNonUniform(Vector3 position, float radius_x, float radius_y_upward, float radius_y_downward, float radius_z, Color color = default(Color), Quaternion rotation = default(Quaternion), float linesWidth = 0.0f, string text = null, int struts = 2, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool skipDrawingEquator = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            EllipsoidNonUniform(position, radius_x, radius_y_upward, radius_y_downward, radius_z, color, rotation * Vector3.up, rotation * Vector3.forward, linesWidth, text, struts, lineStyle, stylePatternScaleFactor, skipDrawingEquator, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void EllipsoidNonUniform(Vector3 position, float radius_x, float radius_y_upward, float radius_y_downward, float radius_z, Color color, Vector3 up, Vector3 forward, float linesWidth = 0.0f, string text = null, int struts = 2, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool skipDrawingEquator = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius_x, "radius_x")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius_y_upward, "radius_y_upward")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius_y_downward, "radius_y_downward")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius_z, "radius_z")) { return; }
            if (UtilitiesDXXL_Math.ApproximatelyZero(radius_x) && UtilitiesDXXL_Math.ApproximatelyZero(radius_y_upward) && UtilitiesDXXL_Math.ApproximatelyZero(radius_y_downward) && UtilitiesDXXL_Math.ApproximatelyZero(radius_z))
            {
                //-> already outside of "UtilitiesDXXL_Shapes.Ellipsoid()", because otherwise the message would already be displayed if only one halfShell is zero:
                if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(linesWidth, "linesWidth")) { return; }
                if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }

                UtilitiesDXXL_DrawBasics.PointFallback(position, "[<color=#adadadFF><icon=logMessage></color> Ellipsoid with extent of 0]<br>" + text, color, linesWidth, durationInSec, hiddenByNearerObjects);
                return;
            }

            if ((UtilitiesDXXL_Math.ApproximatelyZero(radius_x) == false) || (UtilitiesDXXL_Math.ApproximatelyZero(radius_y_upward) == false) || (UtilitiesDXXL_Math.ApproximatelyZero(radius_z) == false))
            {
                Vector3 radius_ofUpperHalfShell = new Vector3(radius_x, radius_y_upward, radius_z);
                UtilitiesDXXL_Shapes.Ellipsoid(position, radius_ofUpperHalfShell, color, up, forward, linesWidth, text, struts, true, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects, skipDrawingEquator);
            }

            if ((UtilitiesDXXL_Math.ApproximatelyZero(radius_x) == false) || (UtilitiesDXXL_Math.ApproximatelyZero(radius_y_downward) == false) || (UtilitiesDXXL_Math.ApproximatelyZero(radius_z) == false))
            {
                Vector3 radius_ofLowerHalfShell = new Vector3(radius_x, -radius_y_downward, radius_z);
                UtilitiesDXXL_Shapes.Ellipsoid(position, radius_ofLowerHalfShell, color, up, forward, linesWidth, null, struts, true, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects, true);
            }
        }

        public static void Capsule(Vector3 posOfCapsuleSphere1, Vector3 posOfCapsuleSphere2, float radius, Color color = default(Color), Vector3 forward_insideCrosssectionPlane = default(Vector3), float linesWidth = 0.0f, string text = null, int struts = 2, bool onlyUpperHalfSphere = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(posOfCapsuleSphere1, "posOfCapsuleSphere1")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(posOfCapsuleSphere2, "posOfCapsuleSphere2")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(forward_insideCrosssectionPlane, "forward_insideCrosssectionPlane")) { return; }
            if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(posOfCapsuleSphere1, posOfCapsuleSphere2))
            {
                Vector3 up = default(Vector3);
                UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref up, ref forward_insideCrosssectionPlane, false);
                Sphere(posOfCapsuleSphere1, radius, color, up, forward_insideCrosssectionPlane, linesWidth, text, struts, onlyUpperHalfSphere, lineStyle, stylePatternScaleFactor, false, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
            else
            {
                Vector3 up = posOfCapsuleSphere2 - posOfCapsuleSphere1;
                UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref up, ref forward_insideCrosssectionPlane, true);
                Vector3 centerPos = 0.5f * (posOfCapsuleSphere2 + posOfCapsuleSphere1);
                Vector3 startToEndSphere = posOfCapsuleSphere2 - posOfCapsuleSphere1;
                float height = startToEndSphere.magnitude + 2.0f * radius;
                Capsule(centerPos, radius, height, color, up, forward_insideCrosssectionPlane, linesWidth, text, struts, onlyUpperHalfSphere, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
            }
        }

        public static void Capsule(Transform transform, Color color = default(Color), float linesWidth = 0.0f, string text = null, int struts = 2, bool onlyUpperHalfSphere = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(transform, "transform")) { return; }
            Capsule(transform.position, transform.lossyScale, color, transform.rotation * Vector3.up, transform.rotation * Vector3.forward, linesWidth, text, struts, onlyUpperHalfSphere, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Capsule(Vector3 position, Vector3 scale, Color color = default(Color), Quaternion rotation = default(Quaternion), float linesWidth = 0.0f, string text = null, int struts = 2, bool onlyUpperHalfSphere = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Capsule(position, scale, color, rotation * Vector3.up, rotation * Vector3.forward, linesWidth, text, struts, onlyUpperHalfSphere, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Capsule(Vector3 position, Vector3 scale, Color color, Vector3 up, Vector3 forward_insideCrosssectionPlane = default(Vector3), float linesWidth = 0.0f, string text = null, int struts = 2, bool onlyUpperHalfSphere = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            float absBiggestNonYDim = Mathf.Max(Mathf.Abs(scale.x), Mathf.Abs(scale.z));
            float absDiameter = absBiggestNonYDim;
            float absRadius = 0.5f * absDiameter;
            float heightInclBothCaps = 2.0f * scale.y;
            if (Mathf.Abs(heightInclBothCaps) < absDiameter)
            {
                heightInclBothCaps = Mathf.Sign(scale.y) * absDiameter;
            }
            UtilitiesDXXL_Shapes.Capsule(position, color, absRadius, heightInclBothCaps, up, forward_insideCrosssectionPlane, linesWidth, text, struts, onlyUpperHalfSphere, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Capsule(Vector3 position, float radius = 0.5f, float height = 1.0f, Color color = default(Color), Quaternion rotation = default(Quaternion), float linesWidth = 0.0f, string text = null, int struts = 2, bool onlyUpperHalfSphere = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Capsule(position, radius, height, color, rotation * Vector3.up, rotation * Vector3.forward, linesWidth, text, struts, onlyUpperHalfSphere, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Capsule(Vector3 position, float radius, float height, Color color, Vector3 up, Vector3 forward_insideCrosssectionPlane = default(Vector3), float linesWidth = 0.0f, string text = null, int struts = 2, bool onlyUpperHalfSphere = false, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(height, "height")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(position, "position")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up, "up")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(forward_insideCrosssectionPlane, "forward_insideCrosssectionPlane")) { return; }

            float absRadius = Mathf.Abs(radius);
            float absDiameter = 2.0f * absRadius;
            Vector3 shiftedPosition = position;
            float heightInclBothCaps = height;

            if (onlyUpperHalfSphere == false)
            {
                if (Mathf.Abs(height) < absDiameter)
                {
                    heightInclBothCaps = Mathf.Sign(height) * absDiameter;
                }
            }
            else
            {
                if (Mathf.Abs(height) < absRadius)
                {
                    heightInclBothCaps = Mathf.Sign(height) * absDiameter;
                }
                else
                {
                    heightInclBothCaps = height + Mathf.Sign(height) * absRadius;
                    if (Mathf.Abs(height) < absDiameter)
                    {
                        float heightExclBothCaps = heightInclBothCaps - Mathf.Sign(height) * absDiameter;
                        shiftedPosition = position - UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(up) * heightExclBothCaps * 0.5f;
                    }
                    else
                    {
                        shiftedPosition = position - Mathf.Sign(height) * UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(up) * absRadius * 0.5f;
                    }
                }
            }

            UtilitiesDXXL_Shapes.Capsule(shiftedPosition, color, absRadius, heightInclBothCaps, up, forward_insideCrosssectionPlane, linesWidth, text, struts, onlyUpperHalfSphere, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Pyramid(Vector3 apexPosition, float height, Quaternion rotation = default(Quaternion), float angleDeg_inVertDir = 90.0f, float angleDeg_inHorizDir = 90.0f, Color color = default(Color), Shape2DType baseShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Pyramid(apexPosition, height, rotation * Vector3.forward, rotation * Vector3.up, angleDeg_inVertDir, angleDeg_inHorizDir, color, baseShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Pyramid(Vector3 apexPosition, float height, Vector3 forward_fromApexTowardsBase, Vector3 up_insideBasePlane = default(Vector3), float angleDeg_inVertDir = 90.0f, float angleDeg_inHorizDir = 90.0f, Color color = default(Color), Shape2DType baseShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(height, "height")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(angleDeg_inVertDir, "angleDeg_inVertDir")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(angleDeg_inHorizDir, "angleDeg_inHorizDir")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(apexPosition, "apexPosition")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(forward_fromApexTowardsBase, "forward_fromApexTowardsBase")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up_insideBasePlane, "up_insideBasePlane")) { return; }

            UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref up_insideBasePlane, ref forward_fromApexTowardsBase, false);
            Vector3 forwardNormalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(forward_fromApexTowardsBase);
            Vector3 baseCenterPosition = apexPosition + forwardNormalized * height;
            angleDeg_inVertDir = Mathf.Clamp(angleDeg_inVertDir, 0.0f, 179.99f);
            float tanOfHalfVertFieldOfView = Mathf.Tan(0.5f * angleDeg_inVertDir * Mathf.Deg2Rad);
            float heightOfBaseRect = 2.0f * height * tanOfHalfVertFieldOfView;
            angleDeg_inHorizDir = Mathf.Clamp(angleDeg_inHorizDir, 0.0f, 179.99f);
            float tanOfHalfHorizFieldOfView = Mathf.Tan(0.5f * angleDeg_inHorizDir * Mathf.Deg2Rad);
            float widthOfBaseRect = 2.0f * height * tanOfHalfHorizFieldOfView;
            Pyramid(baseCenterPosition, height, widthOfBaseRect, heightOfBaseRect, color, (-forward_fromApexTowardsBase), up_insideBasePlane, baseShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Pyramid(Rect baseRect, float zPos_ofBaseRectCenter, float height, Quaternion rotation = default(Quaternion), Color color = default(Color), Shape2DType baseShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(zPos_ofBaseRectCenter, "zPos_ofBaseRectCenter")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(height, "height")) { return; }

            Vector3 baseCenterPosition = new Vector3(baseRect.center.x, baseRect.center.y, zPos_ofBaseRectCenter);
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Pyramid(baseCenterPosition, height, baseRect.width, baseRect.height, color, rotation * Vector3.forward, rotation * Vector3.up, baseShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Pyramid(Rect baseRect, float zPos_ofBaseRectCenter, float height, Vector3 normal_ofBaseTowardsApex, Vector3 up_insideBaseRectPlane = default(Vector3), Color color = default(Color), Shape2DType baseShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(zPos_ofBaseRectCenter, "zPos_ofBaseRectCenter")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up_insideBaseRectPlane, "up_insideBaseRectPlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(normal_ofBaseTowardsApex, "normal_ofBaseTowardsApex")) { return; }

            UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref up_insideBaseRectPlane, ref normal_ofBaseTowardsApex, false);
            Vector3 baseCenterPosition = new Vector3(baseRect.center.x, baseRect.center.y, zPos_ofBaseRectCenter);
            Pyramid(baseCenterPosition, height, baseRect.width, baseRect.height, color, normal_ofBaseTowardsApex, up_insideBaseRectPlane, baseShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Pyramid(Vector3 center_ofPyramidHullVolume, Vector3 scale_ofPyramidHullVolume, Quaternion rotation = default(Quaternion), Color color = default(Color), Shape2DType baseShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(center_ofPyramidHullVolume, "center_ofPyramidHullVolume")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(scale_ofPyramidHullVolume, "scale_ofPyramidHullVolume")) { return; }

            //scale = OverwriteDefaultVectors(scale, new Vector3(1.0f, 1.0f, 1.0f));
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Vector3 vectorUpNormalized = rotation * Vector3.up;
            Vector3 baseCenterPosition = center_ofPyramidHullVolume - 0.5f * vectorUpNormalized * scale_ofPyramidHullVolume.y;
            Pyramid(baseCenterPosition, scale_ofPyramidHullVolume.y, scale_ofPyramidHullVolume.x, scale_ofPyramidHullVolume.z, color, vectorUpNormalized, rotation * Vector3.forward, baseShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Pyramid(Vector3 center_ofBasePlane, float height, float width_ofBase, float length_ofBase, Color color = default(Color), Quaternion rotation = default(Quaternion), Shape2DType baseShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Pyramid(center_ofBasePlane, height, width_ofBase, length_ofBase, color, rotation * Vector3.up, rotation * Vector3.forward, baseShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Pyramid(Vector3 center_ofBasePlane, float height, float width_ofBase, float length_ofBase, Color color, Vector3 normal_ofBaseTowardsApex, Vector3 up_insideBasePlane = default(Vector3), Shape2DType baseShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Shapes.Pyramid(center_ofBasePlane, height, width_ofBase, length_ofBase, color, normal_ofBaseTowardsApex, up_insideBasePlane, baseShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Cone(Vector3 apexPosition, float height, Quaternion rotation = default(Quaternion), float angleDeg_inVertDir = 90.0f, float angleDeg_inHorizDir = 90.0f, Color color = default(Color), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Cone(apexPosition, height, rotation * Vector3.forward, rotation * Vector3.up, angleDeg_inVertDir, angleDeg_inHorizDir, color, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Cone(Vector3 apexPosition, float height, Vector3 forward_fromApexTowardsBase, Vector3 up_insideBaseCircle = default(Vector3), float angleDeg_inVertDir = 90.0f, float angleDeg_inHorizDir = 90.0f, Color color = default(Color), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Pyramid(apexPosition, height, forward_fromApexTowardsBase, up_insideBaseCircle, angleDeg_inVertDir, angleDeg_inHorizDir, color, Shape2DType.circle4struts, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Cone(Vector3 center_ofConeHullVolume, Vector3 scale_ofConeHullVolume, Quaternion rotation = default(Quaternion), Color color = default(Color), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Pyramid(center_ofConeHullVolume, scale_ofConeHullVolume, rotation, color, Shape2DType.circle4struts, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Cone(Vector3 center_ofBaseCircle, float height, float width_ofBaseCircle, float length_ofBaseCircle, Color color = default(Color), Quaternion rotation = default(Quaternion), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Cone(center_ofBaseCircle, height, width_ofBaseCircle, length_ofBaseCircle, color, rotation * Vector3.up, rotation * Vector3.forward, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Cone(Vector3 center_ofBaseCircle, float height, float width_ofBaseCircle, float length_ofBaseCircle, Color color, Vector3 normal_ofBaseCircleTowardsApex, Vector3 up_insideBaseCircle = default(Vector3), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Pyramid(center_ofBaseCircle, height, width_ofBaseCircle, length_ofBaseCircle, color, normal_ofBaseCircleTowardsApex, up_insideBaseCircle, Shape2DType.circle4struts, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void ConeFilled(Vector3 apexPosition, float height, Quaternion rotation = default(Quaternion), float angleDeg_inVertDir = 90.0f, float angleDeg_inHorizDir = 90.0f, Color color = default(Color), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            ConeFilled(apexPosition, height, rotation * Vector3.forward, rotation * Vector3.up, angleDeg_inVertDir, angleDeg_inHorizDir, color, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void ConeFilled(Vector3 apexPosition, float height, Vector3 forward_fromApexTowardsBase, Vector3 up_insideBaseCircle = default(Vector3), float angleDeg_inVertDir = 90.0f, float angleDeg_inHorizDir = 90.0f, Color color = default(Color), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Pyramid(apexPosition, height, forward_fromApexTowardsBase, up_insideBaseCircle, angleDeg_inVertDir, angleDeg_inHorizDir, color, Shape2DType.circle, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void ConeFilled(Vector3 center_ofConeHullVolume, Vector3 scale_ofConeHullVolume, Quaternion rotation = default(Quaternion), Color color = default(Color), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Pyramid(center_ofConeHullVolume, scale_ofConeHullVolume, rotation, color, Shape2DType.circle, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void ConeFilled(Vector3 center_ofBaseCircle, float height, float width_ofBaseCircle, float length_ofBaseCircle, Color color = default(Color), Quaternion rotation = default(Quaternion), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            ConeFilled(center_ofBaseCircle, height, width_ofBaseCircle, length_ofBaseCircle, color, rotation * Vector3.up, rotation * Vector3.forward, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void ConeFilled(Vector3 center_ofBaseCircle, float height, float width_ofBaseCircle, float length_ofBaseCircle, Color color, Vector3 normal_ofBaseCircleTowardsApex, Vector3 up_insideBaseCircle = default(Vector3), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Pyramid(center_ofBaseCircle, height, width_ofBaseCircle, length_ofBaseCircle, color, normal_ofBaseCircleTowardsApex, up_insideBaseCircle, Shape2DType.circle, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Bipyramid(Rect baseRect, float zPos_ofBaseRectCenter, float heightUp, float heightDown, Quaternion rotation = default(Quaternion), Color color = default(Color), Shape2DType baseShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(zPos_ofBaseRectCenter, "zPos_ofBaseRectCenter")) { return; }

            Vector3 baseCenterPosition = new Vector3(baseRect.center.x, baseRect.center.y, zPos_ofBaseRectCenter);
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Bipyramid(baseCenterPosition, heightUp, heightDown, baseRect.width, baseRect.height, color, rotation * Vector3.forward, rotation * Vector3.up, baseShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Bipyramid(Rect baseRect, float zPos_ofBaseRectCenter, float heightUp, float heightDown, Vector3 normal_ofBaseTowardsUpperApex, Vector3 up_insideBaseRectPlane = default(Vector3), Color color = default(Color), Shape2DType baseShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(zPos_ofBaseRectCenter, "zPos_ofBaseRectCenter")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up_insideBaseRectPlane, "up_insideBaseRectPlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(normal_ofBaseTowardsUpperApex, "normal_ofBaseTowardsUpperApex")) { return; }

            UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref up_insideBaseRectPlane, ref normal_ofBaseTowardsUpperApex, false);
            Vector3 baseCenterPosition = new Vector3(baseRect.center.x, baseRect.center.y, zPos_ofBaseRectCenter);
            Bipyramid(baseCenterPosition, heightUp, heightDown, baseRect.width, baseRect.height, color, normal_ofBaseTowardsUpperApex, up_insideBaseRectPlane, baseShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Bipyramid(Vector3 center_ofBasePlane, Vector3 scale_ofBipyramidHullVolume, Quaternion rotation = default(Quaternion), Color color = default(Color), Shape2DType baseShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(scale_ofBipyramidHullVolume, "scale_ofBipyramidHullVolume")) { return; }

            //scale = OverwriteDefaultVectors(scale, new Vector3(1.0f, 1.0f, 1.0f));
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Bipyramid(center_ofBasePlane, 0.5f * scale_ofBipyramidHullVolume.y, -0.5f * scale_ofBipyramidHullVolume.y, scale_ofBipyramidHullVolume.x, scale_ofBipyramidHullVolume.z, color, rotation * Vector3.up, rotation * Vector3.forward, baseShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Bipyramid(Vector3 center_ofBasePlane, float heightUp, float heightDown, float width_ofBase, float length_ofBase, Color color = default(Color), Quaternion rotation = default(Quaternion), Shape2DType baseShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Bipyramid(center_ofBasePlane, heightUp, heightDown, width_ofBase, length_ofBase, color, rotation * Vector3.up, rotation * Vector3.forward, baseShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Bipyramid(Vector3 center_ofBasePlane, float heightUp, float heightDown, float width_ofBase, float length_ofBase, Color color, Vector3 normal_ofBaseTowardsUpperApex, Vector3 up_insideBasePlane = default(Vector3), Shape2DType baseShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Shapes.Bipyramid(center_ofBasePlane, heightUp, heightDown, width_ofBase, length_ofBase, color, normal_ofBaseTowardsUpperApex, up_insideBasePlane, baseShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Cylinder(Rect crossSectionHullRect, float zPos_ofHullRectCenter, float height, Quaternion rotation = default(Quaternion), Color color = default(Color), Shape2DType baseShape = Shape2DType.circle4struts, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Vector3 baseCenterPosition = new Vector3(crossSectionHullRect.center.x, crossSectionHullRect.center.y, zPos_ofHullRectCenter);
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Cylinder(baseCenterPosition, height, crossSectionHullRect.width, crossSectionHullRect.height, color, rotation * Vector3.forward, rotation * Vector3.up, baseShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Cylinder(Rect crossSectionHullRect, float zPos_ofHullRectCenter, float height, Vector3 extrusionDirection, Vector3 up_insideCrossSectionPlane = default(Vector3), Color color = default(Color), Shape2DType baseShape = Shape2DType.circle4struts, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up_insideCrossSectionPlane, "up_insideCrossSectionPlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(extrusionDirection, "extrusionDirection")) { return; }

            UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref up_insideCrossSectionPlane, ref extrusionDirection, false);
            Vector3 baseCenterPosition = new Vector3(crossSectionHullRect.center.x, crossSectionHullRect.center.y, zPos_ofHullRectCenter);
            Cylinder(baseCenterPosition, height, crossSectionHullRect.width, crossSectionHullRect.height, color, extrusionDirection, up_insideCrossSectionPlane, baseShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Cylinder(Vector3 center_ofCylinderHullVolume, Vector3 scale_ofCylinderHullVolume, Quaternion rotation = default(Quaternion), Color color = default(Color), Shape2DType baseShape = Shape2DType.circle4struts, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(scale_ofCylinderHullVolume, "scale_ofCylinderHullVolume")) { return; }

            //scale = OverwriteDefaultVectors(scale, new Vector3(1.0f, 1.0f, 1.0f));
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Cylinder(center_ofCylinderHullVolume, scale_ofCylinderHullVolume.y, scale_ofCylinderHullVolume.x, scale_ofCylinderHullVolume.z, color, rotation * Vector3.up, rotation * Vector3.forward, baseShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Cylinder(Vector3 center_ofCylinderHullVolume, float height, float width_ofBase, float length_ofBase, Color color = default(Color), Quaternion rotation = default(Quaternion), Shape2DType baseShape = Shape2DType.circle4struts, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Cylinder(center_ofCylinderHullVolume, height, width_ofBase, length_ofBase, color, rotation * Vector3.up, rotation * Vector3.forward, baseShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Cylinder(Vector3 center_ofCylinderHullVolume, float height, float width_ofBase, float length_ofBase, Color color, Vector3 extrusionDirection, Vector3 up_insideCrossSectionPlane = default(Vector3), Shape2DType baseShape = Shape2DType.circle4struts, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Shapes.Cylinder(center_ofCylinderHullVolume, height, width_ofBase, length_ofBase, color, extrusionDirection, up_insideCrossSectionPlane, baseShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Extrusion(Rect hullRect_ofExtrudedCrosssection, float zPos_ofExtrudedRect, float extrusionDistanceForward, float extrusionDistanceBackward, Quaternion rotation = default(Quaternion), Color color = default(Color), Shape2DType extrusionShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(zPos_ofExtrudedRect, "zPos_ofExtrudedRect")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(extrusionDistanceForward, "extrusionDistanceForward")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(extrusionDistanceBackward, "extrusionDistanceBackward")) { return; }

            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Vector3 forwardNormalized = rotation * Vector3.forward;
            float cumulatedExtrusionHeight = extrusionDistanceForward - extrusionDistanceBackward;
            Vector3 rectCenterPosition = new Vector3(hullRect_ofExtrudedCrosssection.center.x, hullRect_ofExtrudedCrosssection.center.y, zPos_ofExtrudedRect);
            Vector3 negativeEndOfExtrusionCylinder = rectCenterPosition + forwardNormalized * extrusionDistanceBackward;
            Vector3 cylinderCenterPos = negativeEndOfExtrusionCylinder + forwardNormalized * (0.5f * cumulatedExtrusionHeight);
            Cylinder(cylinderCenterPos, cumulatedExtrusionHeight, hullRect_ofExtrudedCrosssection.width, hullRect_ofExtrudedCrosssection.height, color, rotation * Vector3.forward, rotation * Vector3.up, extrusionShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Extrusion(Rect hullRect_ofExtrudedCrosssection, float zPos_ofExtrudedRect, float extrusionDistanceForward, float extrusionDistanceBackward, Vector3 extrusionDirection, Vector3 up_insideCrosssectionPlane = default(Vector3), Color color = default(Color), Shape2DType extrusionShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(zPos_ofExtrudedRect, "zPos_ofExtrudedRect")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(extrusionDistanceForward, "extrusionDistanceForward")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(extrusionDistanceBackward, "extrusionDistanceBackward")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up_insideCrosssectionPlane, "up_insideCrosssectionPlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(extrusionDirection, "extrusionDirection")) { return; }

            UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref up_insideCrosssectionPlane, ref extrusionDirection, false);
            Vector3 forwardNormalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(extrusionDirection);
            float cumulatedExtrusionHeight = extrusionDistanceForward - extrusionDistanceBackward;
            Vector3 rectCenterPosition = new Vector3(hullRect_ofExtrudedCrosssection.center.x, hullRect_ofExtrudedCrosssection.center.y, zPos_ofExtrudedRect);
            Vector3 negativeEndOfExtrusionCylinder = rectCenterPosition + forwardNormalized * extrusionDistanceBackward;
            Vector3 cylinderCenterPos = negativeEndOfExtrusionCylinder + forwardNormalized * (0.5f * cumulatedExtrusionHeight);
            Cylinder(cylinderCenterPos, cumulatedExtrusionHeight, hullRect_ofExtrudedCrosssection.width, hullRect_ofExtrudedCrosssection.height, color, extrusionDirection, up_insideCrosssectionPlane, extrusionShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Extrusion(Vector3 centerPos_ofExtrusionBase, float extrusionHeightUp, float extrusionHeightDown, float width_ofExtrudedCrosssection, float length_ofExtrudedCrosssection, Color color = default(Color), Quaternion rotation = default(Quaternion), Shape2DType extrusionShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Extrusion(centerPos_ofExtrusionBase, extrusionHeightUp, extrusionHeightDown, width_ofExtrudedCrosssection, length_ofExtrudedCrosssection, color, rotation * Vector3.up, rotation * Vector3.forward, extrusionShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Extrusion(Vector3 centerPos_ofExtrusionBase, float extrusionHeightUp, float extrusionHeightDown, float width_ofExtrudedCrosssection, float length_ofExtrudedCrosssection, Color color, Vector3 extrusionDirection, Vector3 up_insideCrossSectionPlane = default(Vector3), Shape2DType extrusionShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(extrusionHeightUp, "extrusionHeight")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(extrusionHeightDown, "extrusionHeightDown")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(centerPos_ofExtrusionBase, "centerPos_ofExtrusionBase")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(extrusionDirection, "extrusionDirection")) { return; }

            extrusionDirection = UtilitiesDXXL_Math.OverwriteDefaultVectors(extrusionDirection, Vector3.up);
            Vector3 extrusionDirNormalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(extrusionDirection);
            float cumulatedExtrusionHeight = extrusionHeightUp - extrusionHeightDown;
            Vector3 negativeEndOfExtrusionCylinder = centerPos_ofExtrusionBase + extrusionDirNormalized * extrusionHeightDown;
            Vector3 cylinderCenterPos = negativeEndOfExtrusionCylinder + extrusionDirNormalized * (0.5f * cumulatedExtrusionHeight);
            Cylinder(cylinderCenterPos, cumulatedExtrusionHeight, width_ofExtrudedCrosssection, length_ofExtrudedCrosssection, color, extrusionDirection, up_insideCrossSectionPlane, extrusionShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Frustum(Camera camera, Color color = default(Color), Shape2DType clipPlanesShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForNullUnityObjects(camera, "camera")) { return; }
            Frustum(camera.transform.position, camera.transform.forward, camera.transform.up, camera.fieldOfView, camera.aspect, camera.nearClipPlane, camera.farClipPlane, color, clipPlanesShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Frustum(Vector3 cameraPosition_frustumApex, Quaternion cameraRotation, float angleDeg_verticalFieldOfView = 60.0f, float aspectRatio = (16.0f / 9.0f), float distanceApexToNearPlane = 0.5f, float distanceApexToFarPlane = 1.0f, Color color = default(Color), Shape2DType clipPlanesShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            cameraRotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(cameraRotation);
            Vector3 cameraForward = cameraRotation * Vector3.forward;
            Vector3 cameraUp = cameraRotation * Vector3.up;
            Frustum(cameraPosition_frustumApex, cameraForward, cameraUp, angleDeg_verticalFieldOfView, aspectRatio, distanceApexToNearPlane, distanceApexToFarPlane, color, clipPlanesShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }
        public static void Frustum(Vector3 cameraPosition_frustumApex, Vector3 cameraForward, Vector3 cameraUp, float angleDeg_verticalFieldOfView = 60.0f, float aspectRatio = (16.0f / 9.0f), float distanceApexToNearPlane = 0.5f, float distanceApexToFarPlane = 1.0f, Color color = default(Color), Shape2DType clipPlanesShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(angleDeg_verticalFieldOfView, "angleDeg_verticalFieldOfView")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(aspectRatio, "aspectRatio")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(distanceApexToNearPlane, "distanceApexToNearPlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(distanceApexToFarPlane, "distanceApexToFarPlane")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(cameraPosition_frustumApex, "cameraPosition_frustumApex")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(cameraForward, "cameraForward")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(cameraUp, "cameraUp")) { return; }

            UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref cameraUp, ref cameraForward, false);
            Vector3 forwardNormalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(cameraForward);
            Vector3 bigPlaneCenterPos = cameraPosition_frustumApex + forwardNormalized * distanceApexToFarPlane;
            Vector3 smallPlaneCenterPos = cameraPosition_frustumApex + forwardNormalized * distanceApexToNearPlane;
            angleDeg_verticalFieldOfView = Mathf.Clamp(angleDeg_verticalFieldOfView, 0.0f, 179.99f);
            float tanOfHalfFieldOfView = Mathf.Tan(0.5f * angleDeg_verticalFieldOfView * Mathf.Deg2Rad);
            float heightAtFarPlane = 2.0f * distanceApexToFarPlane * tanOfHalfFieldOfView;
            float heightAtNearPlane = 2.0f * distanceApexToNearPlane * tanOfHalfFieldOfView;
            Frustum(bigPlaneCenterPos, smallPlaneCenterPos, heightAtFarPlane * aspectRatio, heightAtFarPlane, heightAtNearPlane * aspectRatio, heightAtNearPlane, color, cameraUp, cameraForward, clipPlanesShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Frustum(Rect bigClipPlaneRect, float zPos_ofBigClipPlaneCenter, float distanceBetweenClipPlanes, float scalingFactor_forSmallClipPlane, Quaternion rotation = default(Quaternion), Color color = default(Color), Shape2DType clipPlanesShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(zPos_ofBigClipPlaneCenter, "zPos_ofBigClipPlaneCenter")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(distanceBetweenClipPlanes, "distanceBetweenClipPlanes")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(scalingFactor_forSmallClipPlane, "scalingFactor_forSmallClipPlane")) { return; }

            Vector3 baseCenterPosition = new Vector3(bigClipPlaneRect.center.x, bigClipPlaneRect.center.y, zPos_ofBigClipPlaneCenter);
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Vector3 forwardNormalized = rotation * Vector3.forward;
            Vector3 smallPlaneCenterPos = baseCenterPosition + forwardNormalized * distanceBetweenClipPlanes;
            Frustum(baseCenterPosition, smallPlaneCenterPos, bigClipPlaneRect.width, bigClipPlaneRect.height, scalingFactor_forSmallClipPlane * bigClipPlaneRect.width, scalingFactor_forSmallClipPlane * bigClipPlaneRect.height, color, rotation * Vector3.up, forwardNormalized, clipPlanesShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Frustum(Rect bigClipPlaneRect, float zPos_ofBigClipPlaneCenter, float distanceBetweenClipPlanes, float scalingFactor_forSmallClipPlane, Vector3 normal_ofClipPlaneRects_towardsApex, Vector3 up_insideClippedRects = default(Vector3), Color color = default(Color), Shape2DType clipPlanesShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(zPos_ofBigClipPlaneCenter, "zPos_ofBigClipPlaneCenter")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(distanceBetweenClipPlanes, "distanceBetweenClipPlanes")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(scalingFactor_forSmallClipPlane, "scalingFactor_forSmallClipPlane")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(up_insideClippedRects, "up_insideClippedRects")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(normal_ofClipPlaneRects_towardsApex, "normal_ofClipPlaneRects_towardsApex")) { return; }

            Vector3 baseCenterPosition = new Vector3(bigClipPlaneRect.center.x, bigClipPlaneRect.center.y, zPos_ofBigClipPlaneCenter);
            UtilitiesDXXL_DrawBasics.OverwriteDefaultVectorsWithStandardIdentity(ref up_insideClippedRects, ref normal_ofClipPlaneRects_towardsApex, false);
            Vector3 forwardNormalized = UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(normal_ofClipPlaneRects_towardsApex);
            Vector3 smallPlaneCenterPos = baseCenterPosition + forwardNormalized * distanceBetweenClipPlanes;
            Frustum(baseCenterPosition, smallPlaneCenterPos, bigClipPlaneRect.width, bigClipPlaneRect.height, scalingFactor_forSmallClipPlane * bigClipPlaneRect.width, scalingFactor_forSmallClipPlane * bigClipPlaneRect.height, color, up_insideClippedRects, forwardNormalized, clipPlanesShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Frustum(Vector3 center_ofFrustumHullVolume, Vector3 scale_ofFrustumHullVolume, Quaternion rotation, float scalingFactor_forSmallClipPlane = 0.5f, Color color = default(Color), Shape2DType clipPlanesShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(scalingFactor_forSmallClipPlane, "scalingFactor_forSmallClipPlane")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(center_ofFrustumHullVolume, "center_ofFrustumHullVolume")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(scale_ofFrustumHullVolume, "scale_ofFrustumHullVolume")) { return; }

            //scale = OverwriteDefaultVectors(scale, new Vector3(1.0f, 1.0f, 1.0f));
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Vector3 upNormalized = rotation * Vector3.up;
            Vector3 bigPlaneCenterPos = center_ofFrustumHullVolume - upNormalized * 0.5f * scale_ofFrustumHullVolume.y;
            Vector3 smallPlaneCenterPos = center_ofFrustumHullVolume + upNormalized * 0.5f * scale_ofFrustumHullVolume.y;
            Frustum(bigPlaneCenterPos, smallPlaneCenterPos, scale_ofFrustumHullVolume.x, scale_ofFrustumHullVolume.z, scalingFactor_forSmallClipPlane * scale_ofFrustumHullVolume.x, scalingFactor_forSmallClipPlane * scale_ofFrustumHullVolume.z, color, rotation * Vector3.forward, upNormalized, clipPlanesShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Frustum(float distance_bigClipPlaneToApex, float distanceBetweenClipPlanes, Vector3 center_ofBigClipPlane, Quaternion rotation = default(Quaternion), float width_ofBigClipPlane = 1.0f, float height_ofBigClipPlane = 1.0f, Color color = default(Color), Shape2DType clipPlanesShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Vector3 normal_ofClipPlanes_towardsApex = rotation * Vector3.up;
            Vector3 up_insideClippedPlanes = rotation * Vector3.forward;
            Frustum(distance_bigClipPlaneToApex, distanceBetweenClipPlanes, center_ofBigClipPlane, normal_ofClipPlanes_towardsApex, up_insideClippedPlanes, width_ofBigClipPlane, height_ofBigClipPlane, color, clipPlanesShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Frustum(float distance_bigClipPlaneToApex, float distanceBetweenClipPlanes, Vector3 center_ofBigClipPlane, Vector3 normal_ofClipPlanes_towardsApex, Vector3 up_insideClippedPlanes = default(Vector3), float width_ofBigClipPlane = 1.0f, float height_ofBigClipPlane = 1.0f, Color color = default(Color), Shape2DType clipPlanesShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width_ofBigClipPlane, "width_ofBigClipPlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(height_ofBigClipPlane, "height_ofBigClipPlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(distanceBetweenClipPlanes, "distanceBetweenClipPlanes")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(distance_bigClipPlaneToApex, "distance_bigClipPlaneToApex")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(center_ofBigClipPlane, "center_ofBigClipPlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(normal_ofClipPlanes_towardsApex, "normal_ofClipPlanes_towardsApex")) { return; }

            float smallPlaneScaleFactor = (distance_bigClipPlaneToApex - distanceBetweenClipPlanes) / distance_bigClipPlaneToApex;
            Frustum(center_ofBigClipPlane, center_ofBigClipPlane + UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(normal_ofClipPlanes_towardsApex) * distanceBetweenClipPlanes, width_ofBigClipPlane, height_ofBigClipPlane, width_ofBigClipPlane * smallPlaneScaleFactor, height_ofBigClipPlane * smallPlaneScaleFactor, color, up_insideClippedPlanes, normal_ofClipPlanes_towardsApex, clipPlanesShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Frustum(Vector3 center_ofBigClipPlane, float distanceBetweenClipPlanes, float scalingFactor_forSmallClipPlane, Quaternion rotation = default(Quaternion), float width_ofBigClipPlane = 1.0f, float height_ofBigClipPlane = 1.0f, Color color = default(Color), Shape2DType clipPlanesShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            rotation = UtilitiesDXXL_Math.OverwriteDefaultQuaternionToIdentity(rotation);
            Vector3 normal_ofClipPlanes_towardsApex = rotation * Vector3.up;
            Vector3 up_insideClippedPlanes = rotation * Vector3.forward;
            Frustum(center_ofBigClipPlane, distanceBetweenClipPlanes, scalingFactor_forSmallClipPlane, normal_ofClipPlanes_towardsApex, up_insideClippedPlanes, width_ofBigClipPlane, height_ofBigClipPlane, color, clipPlanesShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Frustum(Vector3 center_ofBigClipPlane, float distanceBetweenClipPlanes, float scalingFactor_forSmallClipPlane, Vector3 normal_ofClipPlanes_towardsApex, Vector3 up_insideClippedPlanes = default(Vector3), float width_ofBigClipPlane = 1.0f, float height_ofBigClipPlane = 1.0f, Color color = default(Color), Shape2DType clipPlanesShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(width_ofBigClipPlane, "width_ofBigClipPlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(height_ofBigClipPlane, "height_ofBigClipPlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(distanceBetweenClipPlanes, "distanceBetweenClipPlanes")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(scalingFactor_forSmallClipPlane, "scalingFactor_forSmallClipPlane")) { return; }

            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(center_ofBigClipPlane, "center_ofBigClipPlane")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(normal_ofClipPlanes_towardsApex, "normal_ofClipPlanes_towardsApex")) { return; }

            Frustum(center_ofBigClipPlane, center_ofBigClipPlane + UtilitiesDXXL_Math.GetNormalized_afterScalingIntoRegionOfFloatPrecicion(normal_ofClipPlanes_towardsApex) * distanceBetweenClipPlanes, width_ofBigClipPlane, height_ofBigClipPlane, scalingFactor_forSmallClipPlane * width_ofBigClipPlane, scalingFactor_forSmallClipPlane * height_ofBigClipPlane, color, up_insideClippedPlanes, normal_ofClipPlanes_towardsApex, clipPlanesShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Frustum(Vector3 center_ofBigClipPlane, Vector3 center_ofSmallClipPlane, float width_ofBigClipPlane, float height_ofBigClipPlane, float width_ofSmallClipPlane, float height_ofSmallClipPlane, Color color = default(Color), Vector3 up_insideClippedPlanes = default(Vector3), Vector3 fallback_for_normalOfClipPlanesTowardsApex = default(Vector3), Shape2DType clipPlanesShape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            UtilitiesDXXL_Shapes.Frustum(center_ofBigClipPlane, center_ofSmallClipPlane, width_ofBigClipPlane, height_ofBigClipPlane, width_ofSmallClipPlane, height_ofSmallClipPlane, color, up_insideClippedPlanes, fallback_for_normalOfClipPlanesTowardsApex, clipPlanesShape, linesWidth, text, lineStyle, stylePatternScaleFactor, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static int FlatShape(Rect hullRect, Shape2DType shapeType = Shape2DType.square, float zPos_ofRectCenter = 0.0f, Quaternion rotation = default(Quaternion), Color color = default(Color), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool flattenRoundLines_intoShapePlane = true, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(zPos_ofRectCenter, "zPos_ofRectCenter")) { return 0; }

            Vector3 baseCenterPosition = new Vector3(hullRect.center.x, hullRect.center.y, zPos_ofRectCenter);
            UtilitiesDXXL_Shapes.ConvertQuaternionToFlatShapesNormalAndUpInsideFlatPlane(out Vector3 normal, out Vector3 up_insideFlatPlane, rotation);
            return FlatShape(baseCenterPosition, shapeType, hullRect.width, hullRect.height, color, normal, up_insideFlatPlane, linesWidth, text, lineStyle, stylePatternScaleFactor, flattenRoundLines_intoShapePlane, fillStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static int FlatShape(Rect hullRect, Shape2DType shapeType, float zPos_ofRectCenter, Vector3 normal, Vector3 up_insideShapePlane = default(Vector3), Color color = default(Color), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool flattenRoundLines_intoShapePlane = true, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(zPos_ofRectCenter, "zPos_ofRectCenter")) { return 0; }

            Vector3 baseCenterPosition = new Vector3(hullRect.center.x, hullRect.center.y, zPos_ofRectCenter);
            return FlatShape(baseCenterPosition, shapeType, hullRect.width, hullRect.height, color, normal, up_insideShapePlane, linesWidth, text, lineStyle, stylePatternScaleFactor, flattenRoundLines_intoShapePlane, fillStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static int FlatShape(Vector3 centerPosition, Shape2DType shapeType = Shape2DType.square, float width = 1.0f, float height = 1.0f, Color color = default(Color), Quaternion rotation = default(Quaternion), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool flattenRoundLines_intoShapePlane = true, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            UtilitiesDXXL_Shapes.ConvertQuaternionToFlatShapesNormalAndUpInsideFlatPlane(out Vector3 normal, out Vector3 up_insideFlatPlane, rotation);
            return FlatShape(centerPosition, shapeType, width, height, color, normal, up_insideFlatPlane, linesWidth, text, lineStyle, stylePatternScaleFactor, flattenRoundLines_intoShapePlane, fillStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static int FlatShape(Vector3 centerPosition, Shape2DType shapeType, float width, float height, Color color, Vector3 normal, Vector3 up_insideShapePlane = default(Vector3), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool flattenRoundLines_intoShapePlane = true, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return 0; }
            return UtilitiesDXXL_Shapes.FlatShape(centerPosition, width, height, color, normal, up_insideShapePlane, shapeType, linesWidth, text, lineStyle, stylePatternScaleFactor, flattenRoundLines_intoShapePlane, fillStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Rectangle(Rect rect, float zPos_ofRectCenter = 0.0f, Quaternion rotation = default(Quaternion), Color color = default(Color), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool flattenRoundLines_intoShapePlane = true, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            FlatShape(rect, Shape2DType.square, zPos_ofRectCenter, rotation, color, linesWidth, text, lineStyle, stylePatternScaleFactor, flattenRoundLines_intoShapePlane, fillStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Rectangle(Rect rect, float zPos_ofRectCenter, Vector3 normal, Vector3 up_insideRectPlane = default(Vector3), Color color = default(Color), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool flattenRoundLines_intoShapePlane = true, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            FlatShape(rect, Shape2DType.square, zPos_ofRectCenter, normal, up_insideRectPlane, color, linesWidth, text, lineStyle, stylePatternScaleFactor, flattenRoundLines_intoShapePlane, fillStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Rectangle(Vector3 centerPosition, Vector2 size, Quaternion rotation = default(Quaternion), Color color = default(Color), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool flattenRoundLines_intoShapePlane = true, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            FlatShape(centerPosition, Shape2DType.square, size.x, size.y, color, rotation, linesWidth, text, lineStyle, stylePatternScaleFactor, flattenRoundLines_intoShapePlane, fillStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Rectangle(Vector3 centerPosition, Vector2 size, Vector3 normal, Vector3 up_insideBoxPlane = default(Vector3), Color color = default(Color), float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, bool flattenRoundLines_intoShapePlane = true, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            FlatShape(centerPosition, Shape2DType.square, size.x, size.y, color, normal, up_insideBoxPlane, linesWidth, text, lineStyle, stylePatternScaleFactor, flattenRoundLines_intoShapePlane, fillStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Box2D(Rect boxRect, Color color = default(Color), float zPos = 0.0f, float angleDegCC = 0.0f, Shape2DType shape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            //-> not documented, because it creates confusion with "DrawBasics2D.Box".
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Box2D(boxRect.center, boxRect.size, color, zPos, angleDegCC, shape, linesWidth, text, lineStyle, stylePatternScaleFactor, fillStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Box2D(Vector2 centerPosition, Vector2 size, Color color = default(Color), float zPos = 0.0f, float angleDegCC = 0.0f, Shape2DType shape = Shape2DType.square, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            //-> not documented, because it creates confusion with "DrawBasics2D.Box".
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(angleDegCC, "angleDeg")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(size, "size")) { return; }

            Vector3 up_insideBoxPlane = UtilitiesDXXL_Math.ApproximatelyZero(angleDegCC) ? Vector3.up : Quaternion.AngleAxis(angleDegCC, Vector3.forward) * Vector3.up;
            Vector3 positionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(centerPosition, zPos);
            lineStyle = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(lineStyle);
            fillStyle = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(fillStyle);
            UtilitiesDXXL_Shapes.FlatShape(positionV3, size.x, size.y, color, Vector3.forward, up_insideBoxPlane, shape, linesWidth, text, lineStyle, stylePatternScaleFactor, true, fillStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Circle2D(Rect hullRect, Color color = default(Color), float zPos = 0.0f, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            //-> not documented, because it creates confusion with "DrawBasics2D.Circle".
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Box2D(hullRect.center, hullRect.size, color, zPos, 0.0f, Shape2DType.circle, linesWidth, text, lineStyle, stylePatternScaleFactor, fillStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Circle2D(Vector2 centerPosition, float radius = 0.5f, Color color = default(Color), float zPos = 0.0f, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            //-> not documented, because it creates confusion with "DrawBasics2D.Circle".
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(radius, "radius")) { return; }

            float diameter = 2.0f * radius;
            Box2D(centerPosition, new Vector2(diameter, diameter), color, zPos, 0.0f, Shape2DType.circle, linesWidth, text, lineStyle, stylePatternScaleFactor, fillStyle, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Capsule2D(Vector2 posOfCircle1, Vector2 posOfCircle2, float radius = 0.5f, Color color = default(Color), float zPos = 0.0f, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            //-> not documented, because it creates confusion with "DrawBasics2D.Capsule".
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Vector3 posOfCircle1_asV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(posOfCircle1, zPos);
            Vector3 posOfCircle2_asV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(posOfCircle2, zPos);
            lineStyle = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(lineStyle);
            fillStyle = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(fillStyle);
            FlatCapsule(posOfCircle1_asV3, posOfCircle2_asV3, radius, color, Vector3.forward, linesWidth, text, lineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Capsule2D(Rect hullRect, Color color = default(Color), float zPos = 0.0f, CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Vertical, float angleDegCC = 0.0f, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            //-> not documented, because it creates confusion with "DrawBasics2D.Capsule".
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            Capsule2D(hullRect.center, hullRect.size, color, zPos, capsuleDirection, angleDegCC, linesWidth, text, lineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

        public static void Capsule2D(Vector2 centerPosition, Vector2 size, Color color = default(Color), float zPos = 0.0f, CapsuleDirection2D capsuleDirection = CapsuleDirection2D.Vertical, float angleDegCC = 0.0f, float linesWidth = 0.0f, string text = null, DrawBasics.LineStyle lineStyle = DrawBasics.LineStyle.solid, float stylePatternScaleFactor = 1.0f, DrawBasics.LineStyle fillStyle = DrawBasics.LineStyle.invisible, bool filledWithSpokes = false, bool textBlockAboveLine = false, float durationInSec = 0.0f, bool hiddenByNearerObjects = true)
        {
            //-> not documented, because it creates confusion with "DrawBasics2D.Capsule".
            if (DXXLWrapperForUntiysBuildInDrawLines.CheckIfDrawingIsCurrentlySkipped()) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidFloats(angleDegCC, "angleDeg")) { return; }
            if (UtilitiesDXXL_Log.ErrorLogForInvalidVectors(size, "size")) { return; }

            Vector3 upAlongVert_insideCapsulePlane = UtilitiesDXXL_Math.ApproximatelyZero(angleDegCC) ? Vector3.up : Quaternion.AngleAxis(angleDegCC, Vector3.forward) * Vector3.up;
            Vector3 positionV3 = UtilitiesDXXL_DrawBasics2D.Position_V2toV3(centerPosition, zPos);
            lineStyle = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(lineStyle);
            fillStyle = UtilitiesDXXL_LineStyles.FallbackTo2DLineStyle(fillStyle);
            FlatCapsule(positionV3, size.x, size.y, color, Vector3.forward, upAlongVert_insideCapsulePlane, capsuleDirection, linesWidth, text, lineStyle, stylePatternScaleFactor, fillStyle, filledWithSpokes, textBlockAboveLine, durationInSec, hiddenByNearerObjects);
        }

    }

}





