using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace CardBuilder
{
    [Serializable]
    public class TreeViewData<TitemInTree>
    {

        public List<TreeViewData<TitemInTree>> children;

        public TitemInTree data;
        public bool hasChildren;

        public static TreeViewData<TitemInTree> CreateTreeViewFromTreeViewItemData(TreeViewItemData<TitemInTree> treeViewItemData)
        {

            TreeViewData<TitemInTree> rootData = new();

            rootData.data = treeViewItemData.data;


            rootData.children = new();

            if (!treeViewItemData.hasChildren) return rootData;

            rootData.hasChildren = true;

            foreach(TreeViewItemData<TitemInTree> item in treeViewItemData.children)
            {

                TreeViewData<TitemInTree> dataChild = new();

                dataChild = CreateTreeViewFromTreeViewItemData(item);


                rootData.children.Add(dataChild);
            }


            return rootData;
        }
    }

    [Serializable]
    public class TreeViewHierarchyData : TreeViewData<HierarchyData>
    {
        public new static TreeViewHierarchyData CreateTreeViewFromTreeViewItemData(TreeViewItemData<HierarchyData> treeViewItemData)
        {

            TreeViewHierarchyData rootData = new();

            Logs.Error("testing2" + treeViewItemData.data.Name);

            //rootData.data.Name = treeViewItemData.data.Name;
            rootData.data = new HierarchyData() {

                VisualElementType = treeViewItemData.data.VisualElementType,
                Name = treeViewItemData.data.Name,
                Position = treeViewItemData.data.Position,
                VisualElement = treeViewItemData.data.VisualElement,
                Size = treeViewItemData.data.Size
            };


            rootData.children = new();

            if (!treeViewItemData.hasChildren) return rootData;

            rootData.hasChildren = true;

            foreach (TreeViewItemData<HierarchyData> item in treeViewItemData.children)
            {

                TreeViewData<HierarchyData> dataChild = new();

                dataChild = CreateTreeViewFromTreeViewItemData(item);


                rootData.children.Add(dataChild);
            }


            return rootData;
        }
    }
}
