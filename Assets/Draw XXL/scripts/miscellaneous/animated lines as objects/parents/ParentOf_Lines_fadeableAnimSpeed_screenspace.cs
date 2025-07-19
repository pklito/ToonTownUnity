namespace DrawXXL
{
    using UnityEngine;

    public class ParentOf_Lines_fadeableAnimSpeed_screenspace : ParentOf_Lines_fadeableAnimSpeed
    {
        public Camera targetCamera; //the camera that defines the viewport to which the line is drawn. If it is not specified then a camera is automatically searched based on "DrawScreenspace.defaultScreenspaceWindowForDrawing"
        public float width_relToViewportHeight = 0.0f;
        public float endPlatesSize_relToViewportHeight = 0.0f;
        public bool TryFetchCamera(string nameOfRequestingFunction)
        {
            //returns "camera is available"
            if (targetCamera == null)
            {
                return UtilitiesDXXL_Screenspace.GetAutomaticCameraForDrawing(out targetCamera, nameOfRequestingFunction);
            }
            else
            {
                return true;
            }
        }

        public virtual void Draw()
        {
        }


    }

}
