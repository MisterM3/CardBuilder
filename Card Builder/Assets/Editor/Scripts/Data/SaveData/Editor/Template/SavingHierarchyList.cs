using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

namespace CardBuilder
{
    [Serializable]
    public class SavingHierarchyList
    {

        [SerializeField] List<HierarchyDataWithParentID> hierachyDataList = new();

        

        public List<HierarchyDataWithParentID> GetList()
        {
            return hierachyDataList;
        }

        public HierarchyData GetHierarchyDataFromList(int id)
        {
            return hierachyDataList[id].hierarchyData;
        }

        public int GetIDFromList(HierarchyData data)
        {

            foreach(HierarchyDataWithParentID hierarchyData in hierachyDataList)
            {
                if (hierarchyData.hierarchyData == data)
                    return hierachyDataList.IndexOf(hierarchyData);
            }

            Logs.Error("Data not in list:" + data);
            return -1;

        }


        public void AddToList(HierarchyData data, int parentID)
        {
            HierarchyDataWithParentID newSaveData = new(data, parentID);

            hierachyDataList.Add(newSaveData);

            
        }

        public void AddToList(HierarchyData data, HierarchyData parentData)
        {
            int parentID = GetIDFromList(parentData);

            AddToList(data, parentID);
        }

        public SavingHierarchyList Copy()
        {
            SavingHierarchyList newSavingHierarchyList = new SavingHierarchyList();

            newSavingHierarchyList.hierachyDataList = new List<HierarchyDataWithParentID>();

            foreach(HierarchyDataWithParentID hiarachyData in hierachyDataList)
            {
                newSavingHierarchyList.hierachyDataList.Add(hiarachyData.Copy());
            }

            return newSavingHierarchyList;
        }
    }


    [Serializable]
    public class HierarchyDataWithParentID
    {
        public HierarchyData hierarchyData;
        public int parentID;

        public HierarchyDataWithParentID(HierarchyData data, int parentID)
        {
            this.hierarchyData = data;
            this.parentID = parentID;
        }

        public HierarchyDataWithParentID Copy()
        {
            return new HierarchyDataWithParentID(hierarchyData.Copy(), parentID);
        }
    }
}
