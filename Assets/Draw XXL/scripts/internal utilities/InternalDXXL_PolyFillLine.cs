namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;

    public class InternalDXXL_PolyFillLine
    {
        public InternalDXXL_Plane plane_perpToPolygon;
        public List<Vector3> fillLineAnchors = new List<Vector3>();
        public int usedSlotsInFillLineAnchorsList = 0;

        public InternalDXXL_PolyFillLine(Vector3 posOfPerpPlane, Vector3 normalOfPerpPlane)
        {
            plane_perpToPolygon = new InternalDXXL_Plane(posOfPerpPlane, normalOfPerpPlane);
        }

        public void IntersectWithEdge(InternalDXXL_Edge intersectingEdge)
        {
            if (intersectingEdge.CheckIfLengthIsZero() == false)
            {
                if (plane_perpToPolygon.CheckIfLineIsParallel(intersectingEdge.line) == false)
                {
                    Vector3 intersection_ofCurrCheckedEdge = plane_perpToPolygon.GetIntersectionWithLine(intersectingEdge.line);
                    Vector3 intersection_to_edgeStart = intersectingEdge.start - intersection_ofCurrCheckedEdge;
                    Vector3 intersection_to_edgeEnd = intersectingEdge.end - intersection_ofCurrCheckedEdge;

                    if (UtilitiesDXXL_Math.ApproximatelyZero(intersection_to_edgeStart))
                    {
                        AddToFillLineAnchorsList(intersection_ofCurrCheckedEdge);
                    }
                    else
                    {
                        if (UtilitiesDXXL_Math.ApproximatelyZero(intersection_to_edgeEnd))
                        {
                            AddToFillLineAnchorsList(intersection_ofCurrCheckedEdge);
                        }
                        else
                        {
                            if (UtilitiesDXXL_Math.Check_ifVectorsPointAwayFromEachOther_perpCountsAsPointingAwayFromEachOther(intersection_to_edgeStart, intersection_to_edgeEnd))
                            {
                                AddToFillLineAnchorsList(intersection_ofCurrCheckedEdge);
                            }
                        }
                    }
                }
            }
        }


        public void RemoveDuplicateIntersections()
        {
            for (int i_ref = 0; i_ref < usedSlotsInFillLineAnchorsList; i_ref++)
            {
                for (int i_potDuplicate = usedSlotsInFillLineAnchorsList - 1; i_potDuplicate > i_ref; i_potDuplicate--)
                {
                    if (UtilitiesDXXL_Math.CheckIf_twoVectorsAreApproximatelyEqual(fillLineAnchors[i_ref], fillLineAnchors[i_potDuplicate]))
                    {
                        RemoveAt_fromAStaticVectorList(i_potDuplicate);
                    }
                }
            }
        }



        void AddToFillLineAnchorsList(Vector3 posToAdd)
        {
            //function returns "i_nextFreeSlot"
            //function is not ensuring yet if addSlot is the next higher nonExisting-slot
            if (usedSlotsInFillLineAnchorsList < fillLineAnchors.Count)
            {
                fillLineAnchors[usedSlotsInFillLineAnchorsList] = posToAdd;
            }
            else
            {
                fillLineAnchors.Add(posToAdd);
            }
            usedSlotsInFillLineAnchorsList++;
        }


        void RemoveAt_fromAStaticVectorList(int i_toRemove)
        {
            //function returns "i_nextFreeSlot"
            //function is not checking yet if removeSlot already exists
            fillLineAnchors.RemoveAt(i_toRemove);
            usedSlotsInFillLineAnchorsList--;
        }


    }


}
