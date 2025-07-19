namespace DrawXXL
{
    using UnityEngine;
    using System.Collections.Generic;
    
    public class UtilitiesDXXL_List
    {
        public static void CopyContentOfVectorLists(ref List<Vector3> listWhereToCopyTo, ref List<Vector3> listWhereToCopyFrom, int numberOfSlotsToCopy)
        {
            for (int i = 0; i < numberOfSlotsToCopy; i++)
            {
                AddToAVectorList(ref listWhereToCopyTo, listWhereToCopyFrom[i], i);
            }
        }

        public static void CopyContentOfVector2Lists(ref List<Vector2> listWhereToCopyTo, ref List<Vector2> listWhereToCopyFrom, int numberOfSlotsToCopy)
        {
            for (int i = 0; i < numberOfSlotsToCopy; i++)
            {
                AddToAVector2List(ref listWhereToCopyTo, listWhereToCopyFrom[i], i);
            }
        }

        public static void CopyContentOfVector2ArrayToList(ref List<Vector2> listWhereToCopyTo, ref Vector2[] arrayWhereToCopyFrom, int numberOfSlotsToCopy)
        {
            for (int i = 0; i < numberOfSlotsToCopy; i++)
            {
                AddToAVector2List(ref listWhereToCopyTo, arrayWhereToCopyFrom[i], i);
            }
        }

        public static int AddToAVectorList(ref List<Vector3> targetList, Vector3 posToAdd, int i_ofSlotWhereToAdd)
        {
            //function returns "i_nextFreeSlot"
            if (i_ofSlotWhereToAdd < targetList.Count)
            {
                targetList[i_ofSlotWhereToAdd] = posToAdd;
            }
            else
            {
                while (targetList.Count <= i_ofSlotWhereToAdd)
                {
                    targetList.Add(posToAdd);
                }
            }
            i_ofSlotWhereToAdd++;
            return i_ofSlotWhereToAdd;
        }

        public static int AddToAVector2List(ref List<Vector2> targetList, Vector2 posToAdd, int i_ofSlotWhereToAdd)
        {
            //function returns "i_nextFreeSlot"
            if (i_ofSlotWhereToAdd < targetList.Count)
            {
                targetList[i_ofSlotWhereToAdd] = posToAdd;
            }
            else
            {
                while (targetList.Count <= i_ofSlotWhereToAdd)
                {
                    targetList.Add(posToAdd);
                }
            }
            i_ofSlotWhereToAdd++;
            return i_ofSlotWhereToAdd;
        }

        public static int AddToABoolList(ref List<bool> targetList, bool boolToAdd, int i_ofSlotWhereToAdd)
        {
            //function returns "i_nextFreeSlot"
            if (i_ofSlotWhereToAdd < targetList.Count)
            {
                targetList[i_ofSlotWhereToAdd] = boolToAdd;
            }
            else
            {
                while (targetList.Count <= i_ofSlotWhereToAdd)
                {
                    targetList.Add(boolToAdd);
                }
            }
            i_ofSlotWhereToAdd++;
            return i_ofSlotWhereToAdd;
        }

        public static int AddRangeToAVectorList(ref List<Vector3> targetList, List<Vector3> rangeToAdd, int i_ofSlotWhereToAdd, int slotsToAddFromRangeList)
        {
            //function returns "i_nextFreeSlotAfterInsertedRange"
            for (int i = 0; i < slotsToAddFromRangeList; i++)
            {
                i_ofSlotWhereToAdd = AddToAVectorList(ref targetList, rangeToAdd[i], i_ofSlotWhereToAdd);
            }
            return i_ofSlotWhereToAdd;
        }

        public static int InsertToAVectorList(ref List<Vector3> targetList, int i_whereToInsert, Vector3 posToInsert, int i_nextFreeSlot)
        {
            //function returns "i_nextFreeSlot"
            //function is not checking yet if insertSlot already exists
            targetList.Insert(i_whereToInsert, posToInsert);
            i_nextFreeSlot++;
            return i_nextFreeSlot;
        }

        public static int RemoveAt_fromAVectorList(ref List<Vector3> targetList, int i_toRemove, int i_nextFreeSlot)
        {
            //function returns "i_nextFreeSlot"
            //function is not checking yet if removeSlot already exists
            targetList.RemoveAt(i_toRemove);
            i_nextFreeSlot--;
            return i_nextFreeSlot;
        }

    }

}
