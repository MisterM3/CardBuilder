using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using CardBuilder.Helpers;

namespace CardBuilder
{
    /// <summary>
    /// A Generic TreeView with easy ways of adding and removing elements (based on strings so no duplicate names)
    /// </summary>
    /// <typeparam name="Tdata">Data that is stored in each bar</typeparam>
    public class HierarchyTreeView<Tdata> : ScriptableObject where Tdata : IHierarchyListData
    {

        

        //Unique id of bars starts at 1 
        protected int id = 1;

        private const float treeViewLenght = 100.0f;

        //Standard list where roots are stored
        public List<TreeViewItemData<Tdata>> rootList = new();


        //Treeview used
        public TreeView treeView;


        public virtual void Initialize(TreeView treeView)
        {
            this.treeView = treeView;

            //Reordering too buggy for now, switch it off, add after minor finishes
            this.treeView.reorderable = false;
            this.treeView.SetRootItems(rootList);


            Length width = new Length(treeViewLenght, LengthUnit.Percent);

            treeView.QLogged<ScrollView>().style.width = new StyleLength(width);

            treeView.itemIndexChanged += TreeView_itemIndexChanged;

            SetupMakeItem();
            SetupBind();
        }

        public void Rebuild()
        {
            this.treeView.SetRootItems(rootList);
            treeView.Rebuild();
        }

        public void Rebuild(object o,EventArgs arg)
        {
            Rebuild();
        }

        public void RemoveSelectedItem()
        {
            Tdata selectedItem = (Tdata)treeView.selectedItem;

            var parentItem = FindItemParent(rootList[0], selectedItem.Name);
            RemoveItem(selectedItem.Name, parentItem.data.Name);

            Rebuild();
        }





        private void SetupMakeItem()
        {

            // Set TreeView.makeItem to initialize each node in the tree.
            treeView.makeItem = () => new Label();
        }

        private void SetupBind()
        {
            // Set TreeView.bindItem to bind an initialized node to a data item.
            treeView.bindItem = (VisualElement element, int index) => (element as Label).text = treeView.GetItemDataForIndex<HierarchyData>(index).Name;
        }

        public Tdata GetSelectedItem()
        {
            var SelectedItem = treeView.selectedItem;
            return (Tdata)SelectedItem;
        }

        /// <summary>
        /// Says if the item is already in the list
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool AlreadyInList(string name)
        {
            TreeViewItemData<Tdata> data = FindItem(rootList, name);
            if (data.id != -1) return true;
            return false;
        }


        #region AddItems

        /// <summary>
        /// Adds a new item to the treelist
        /// </summary>
        /// <param name="data">Data that is saved in the bar (Makes it into a new TreeViewItemData)</param>
        /// <param name="parentName">Name of parent where the item is added</param>
        public void AddItem(Tdata data, string parentName)
        {
            //Add new item with a unique id
            TreeViewItemData<Tdata> newItem = new(id++, data, null);

            AddItem(newItem, parentName);
        }

        public void AddItem(Tdata data, string parentName, int index)
        {
            //Add new item with a unique id
            TreeViewItemData<Tdata> newItem = new(id++, data, null);

            AddItem(newItem, parentName, index);
        }

        /// <summary>
        /// Adds a new item to the treelist
        /// </summary>
        /// <param name="data">Data that is saved in the bar</param>
        public void AddItem(Tdata data)
        {
            TreeViewItemData<Tdata> newItem = new(id++, data, null);

            //Add element to the base of the treelist
            rootList.Add(newItem);

            treeView.SetRootItems(rootList);
        }

        /// <summary>
        /// Adds a new item to the treelist
        /// </summary>
        /// <param name="newItem">TreeViewItemData that is added to the list</param>
        /// <param name="parentName">Name of parent where the item is added</param>
        private void AddItem(TreeViewItemData<Tdata> newItem, string parentName)
        {
            //Find parent (need to change list)
            TreeViewItemData<Tdata> parentItem = FindItem(rootList, parentName);

            //id == -1 means the parent has not been found
            if (parentItem.id == -1) return;


            List<TreeViewItemData<Tdata>> childList;


            if (parentItem.hasChildren)
            {
                IEnumerable<TreeViewItemData<Tdata>> childrenParent = parentItem.children;
                childList = childrenParent.ToList();
            }
            else
            {
                childList = new();
            }

            childList.Add(newItem);

            //Make new parentItem (id and data same, but new childlist added with added element)
            TreeViewItemData<Tdata> newParentItem = new(parentItem.id, parentItem.data, childList);

            //Replaces outdated parent with new parent
            ReplaceItem(newParentItem);


        }

        private void AddItem(TreeViewItemData<Tdata> newItem, string parentName, int index)
        {
            //Find parent (need to change list)
            TreeViewItemData<Tdata> parentItem = FindItem(rootList, parentName);

            //id == -1 means the parent has not been found
            if (parentItem.id == -1) return;


            List<TreeViewItemData<Tdata>> childList;


            if (parentItem.hasChildren)
            {
                IEnumerable<TreeViewItemData<Tdata>> childrenParent = parentItem.children;
                childList = childrenParent.ToList();
            }
            else
            {
                childList = new();
            }



            childList.Insert(index, newItem);

            //Make new parentItem (id and data same, but new childlist added with added element)
            TreeViewItemData<Tdata> newParentItem = new(parentItem.id, parentItem.data, childList);

            //Replaces outdated parent with new parent
            ReplaceItem(newParentItem);


        }

        #endregion

        #region FindItems

        /// <summary>
        /// Recursive Method to find a TreeViewItemData from a list using a name
        /// </summary>
        /// <param name="list">List in which the item needs to be found</param>
        /// <param name="name">Name of item to find</param>
        /// <returns></returns>
        public TreeViewItemData<Tdata> FindItem(IEnumerable<TreeViewItemData<Tdata>> list, string name)
        {

            foreach (TreeViewItemData<Tdata> item in list.ToList())
            {

                if (item.data.Name == name) return item;

                if (!item.hasChildren) continue;


                TreeViewItemData<Tdata> test = FindItem(item.children, name);
                if (test.id != -1) return test;
            }


            return new(-1, list.First().data);
        }

        /// <summary>
        /// Recursive Method to find the parent TreeViewItemData from a list using the name of the child
        /// </summary>
        /// <param name="list">List in which the item needs to be found</param>
        /// <param name="name">Name of child item to search for</param>
        /// <returns></returns>
        private TreeViewItemData<Tdata> FindItemParent(TreeViewItemData<Tdata> parent, string name)
        {

            foreach (var item in parent.children)
            {

                if (item.data.Name == name) return parent;

                if (!item.hasChildren) continue;

                TreeViewItemData<Tdata> test = FindItemParent(item, name);
                if (test.id != -1) return test;
            }

            return new(-1, parent.data);
        }

        #endregion



        /// <summary>
        /// Check if element is root item
        /// </summary>
        /// <param name="checkIfRoot">Root item</param>
        /// <param name="name">Name of item to see if it's the same as root</param>
        /// <returns></returns>
        private bool AtRoot(TreeViewItemData<Tdata> checkIfRoot, string name)
        {
            return checkIfRoot.data.Name == name;
        }


        /// <summary>
        /// Removes an item from a parent using strings
        /// </summary>
        /// <param name="itemToRemove">Name of item to remove</param>
        /// <param name="itemToRemoveFrom">Name of parent from where the item needs to be removed from</param>
        public void RemoveItem(string itemToRemove, string itemToRemoveFrom)
        {
            TreeViewItemData<Tdata> parentItem = FindItem(rootList, itemToRemoveFrom);

            if (parentItem.id == -1) return;

            IEnumerable<TreeViewItemData<Tdata>> childrenParent = parentItem.children;


            List<TreeViewItemData<Tdata>> childList = childrenParent.ToList();

            //Find item to remove
            TreeViewItemData<Tdata> removeableItem = childList.Find(x => x.data.Name == itemToRemove);

            childList.Remove(removeableItem);

            //Make new parent item to replace 
            TreeViewItemData<Tdata> newParentItem = new(parentItem.id, parentItem.data, childList);

            //Replace obsolete parent with new parent
            ReplaceItem(newParentItem);

        }



        /// <summary>
        /// Replaces old obsolete item with new item (Recursive)
        /// </summary>
        /// <param name="newItem">New Item To replace the old obsolete with</param>
        private void ReplaceItem(TreeViewItemData<Tdata> newItem)
        {
            string itemName = newItem.data.Name;

            //Logs.Info(newItem.children.ToList().Count);

            if (AtRoot(rootList[0], itemName))
            {
                rootList.Clear();
                rootList.Add(newItem);
              //  treeView.SetRootItems(rootList);
                treeView.Rebuild();
                return;
            }


            TreeViewItemData<Tdata> parentToReplace = FindItemParent(rootList[0], itemName);



            string parentName = parentToReplace.data.Name;


            TreeViewItemData<Tdata> item = parentToReplace.children.ToList().Find((x) => x.id == newItem.id);

            int index = parentToReplace.children.ToList().IndexOf(item);


            index = Mathf.Max(0, index);

            


            RemoveItem(itemName, parentName);
            AddItem(newItem, parentName, index);

            treeView.SetRootItems(rootList);
        }





        #region Events

        private void TreeView_itemIndexChanged(int idChanged, int ParentID)
        {


            bool tryToAddAsRoot = (ParentID == -1);

            if (tryToAddAsRoot)
            {
                Tdata data = treeView.GetItemDataForId<Tdata>(idChanged);
                treeView.RemoveFromSelectionById(idChanged);


                string parentName = rootList[0].data.Name;

                AddItem(data, parentName);

                treeView.SetRootItems(rootList);

                EditorUtility.DisplayDialog("Can't change root Item", "The root item of the hierarchy can't be changed, and there can only be one root", "OK");
            }
        }


        #endregion
    }

    public class HierarchyTreeView : HierarchyTreeView<HierarchyData>, ILoadSaveTemplate<SavingHierarchyList>
    {

        public static HierarchyTreeView Instance;

        private VisualElement rootVisualElement;

        [SerializeField] private Sprite defaultSprite = default;

        [SerializeField] private string defaultString = "Base Text";

        public EventHandler<VisualElement> onAddedVisualElement;


        public override void Initialize(TreeView treeView)
        {
            Instance = this;
            base.Initialize(treeView);
        }

        public void RemoveHighlightedItem()
        {
            GetSelectedItem().VisualElement.RemoveVisualElement();
            RemoveSelectedItem();
        }

        /// <summary>
        /// Called when the property name is switched
        /// </summary>
        public void SwitchProperty(string oldName, string replaceName)
        {
            TreeViewItemData<HierarchyData> dataWithOldName = FindItemWithPropertyName(oldName, rootList[0].children);

            if (dataWithOldName.data == null) return;
            if (dataWithOldName.data.VisualElement == null) return;
            if (dataWithOldName.data.VisualElement.ConnectedInfo == null) return;
            //Don't know why this is allowed, maybe just doesn't work.
            dataWithOldName.data.VisualElement.ConnectedInfo.NameProperty = replaceName;
        }

        private TreeViewItemData<HierarchyData> FindItemWithPropertyName(string propertyNameToFind, IEnumerable<TreeViewItemData<HierarchyData>> children)
        {

            foreach(TreeViewItemData<HierarchyData> child in children)
            {
                string oldConnectedPropertyName = child.data.VisualElement?.ConnectedInfo?.NameProperty;

                if (oldConnectedPropertyName == propertyNameToFind) return child;

                if (child.children == null) continue;

                TreeViewItemData<HierarchyData> childFind = FindItemWithPropertyName(propertyNameToFind, child.children);

                //Return if it's valid data (-10 is used as null)
                if (childFind.id != -10) return childFind;
            }

            return new TreeViewItemData<HierarchyData>(id = -10, null);
        }

        

        public void MakeNewImage()
        {
            HierarchyData data = GetSelectedItem();

            if (data == null) return;

            HierarchyData newData = new();
            newData.Name = "New Image";
            newData.VisualElementType = VisualElementType.Image;

            newData.VisualElement = new UndoRedoImage();


            if (newData.VisualElement is UndoRedoImage undo)
            {
                undo.Init();
                undo.Position = Vector2.zero;
                undo.Image = defaultSprite;
            }

            data.VisualElement.VisualElement.Add(newData.VisualElement.VisualElement);
            int i = 0;
            while (AlreadyInList(newData.Name))
            {
                i++;
                newData.Name = $"New Image ({i})";
            }

            AddItem(newData, data.Name);
            Rebuild();
        }

        public void MakeNewText()
        {
            HierarchyData data = GetSelectedItem();
            if (data == null) return;

            HierarchyData newData = new();
            newData.Name = "New Text";

            newData.VisualElementType = VisualElementType.Text;

            newData.VisualElement = new UndoRedoText();


            if (newData.VisualElement is UndoRedoText undo)
            {
                undo.Init();
                undo.Position = Vector2.zero;
                undo.Text = defaultString;
            }

            data.VisualElement.VisualElement.Add(newData.VisualElement.VisualElement);


            int i = 0;

            while (AlreadyInList(newData.Name))
            {
                i++;
                newData.Name = $"New Text ({i})";
            }

            AddItem(newData, data.Name);
            Rebuild();
        }


        private void MakeImage(HierarchyData currentData, HierarchyData parentData)
        {
            if (parentData == null) return;

            if (currentData.VisualElement is UndoRedoImage undo)
            {
                undo.Init();
                //Updates the position of the visuals
                parentData.VisualElement.VisualElement.Add(currentData.VisualElement.VisualElement);
                undo.Position = undo.Position;
            }

            AddItem(currentData, parentData.Name);

            Rebuild();
        }


        private void MakeText(HierarchyData currentData, HierarchyData parentData)
        {
            if (parentData == null) return;

            if (currentData.VisualElement is UndoRedoText undo)
            {
                undo.Init();
                //Updates the position of the visuals
                parentData.VisualElement.VisualElement.Add(currentData.VisualElement.VisualElement);
                undo.Position = undo.Position;
            }

            AddItem(currentData, parentData.Name);

            Rebuild();
        }



        public void LoadTemplate(TemplateData templateDataToLoad)
        {
 
            SavingHierarchyList savedHierarchyList = templateDataToLoad.hierarchyData;

            rootList.Clear();

            if (savedHierarchyList.GetList().Count == 0)
            {
                HierarchyData data = new();
                data.Name = "ROOT";

                data.VisualElementType = VisualElementType.Image;

                data.VisualElement = new UndoRedoImage();
                data.VisualElement.Init();

                data.VisualElement.VisualElement.style.position = Position.Relative;

                onAddedVisualElement?.Invoke(this, data.VisualElement.VisualElement);

                TreeViewItemData<HierarchyData> root = new(0, data);




                rootList.Add(root);
                treeView.SetRootItems(rootList);
            }




            foreach (HierarchyDataWithParentID hierchyList in savedHierarchyList.GetList())
            {


                if (hierchyList.parentID < 0)
                {


                    if (hierchyList.hierarchyData.VisualElement is UndoRedoImage undo)
                    {
                        undo.Init();

                        undo.VisualElement.style.position = Position.Relative;
                    }

                    if (hierchyList.hierarchyData.VisualElement is UndoRedoText undoText)
                    {
                        undoText.Init();

                        undoText.VisualElement.style.position = Position.Relative;
                    }

                    TreeViewItemData<HierarchyData> root = new(0, hierchyList.hierarchyData);


                    rootList.Add(root);
                    treeView.SetRootItems(rootList);

                    onAddedVisualElement?.Invoke(this, root.data.VisualElement.VisualElement);

                    continue;
                }

                HierarchyData parent = savedHierarchyList.GetHierarchyDataFromList(hierchyList.parentID);

                if (hierchyList.hierarchyData.VisualElementType == VisualElementType.Image) MakeImage(hierchyList.hierarchyData, parent);
                else MakeText(hierchyList.hierarchyData, parent);

            }

            Rebuild();

        }

        public SavingHierarchyList SaveTemplate()
        {

            SavingHierarchyList savingHierarchyList = new();

            TreeViewItemData<HierarchyData> treeViewData = rootList[0];

            savingHierarchyList.AddToList(treeViewData.data, -1);


            

            RecursiveAddingToList(treeViewData, savingHierarchyList);





            return savingHierarchyList;
            //Logs.Info(rootList[0].data.Name);
            //return TreeViewHierarchyData.CreateTreeViewFromTreeViewItemData(rootList[0]);
        }

        public void RecursiveAddingToList(TreeViewItemData<HierarchyData> treeViewDataToAdd, SavingHierarchyList listToAddTo)
        {

            if (!treeViewDataToAdd.hasChildren) return;

            foreach (TreeViewItemData<HierarchyData> childTreeViewData in treeViewDataToAdd.children)
            {
                listToAddTo.AddToList(childTreeViewData.data, treeViewDataToAdd.data);

                RecursiveAddingToList(childTreeViewData, listToAddTo);
            }
        }



        public TreeViewItemData<HierarchyData> TreeViewItemDataToTreeView(TreeViewData<HierarchyData> treeView)
        {
            if (!treeView.hasChildren) return new(id++, treeView.data, null);

            List<TreeViewItemData<HierarchyData>> childrenList = new();
            foreach (TreeViewData<HierarchyData> item in treeView.children)
            {

                TreeViewItemData<HierarchyData> childItem = TreeViewItemDataToTreeView(item);

                childrenList.Add(childItem);
            }


            return new(id++, treeView.data, childrenList);


        }

        public bool CanSave()
        {
            TreeViewItemData<HierarchyData> treeViewData = rootList[0];

            if (!CheckSingularSave(treeViewData)) return false;
            if (treeViewData.children == null) return true;

            return RecursiveSaveChecking(treeViewData.children);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="treeView"></param>
        /// <returns>Returns true if fine, false if can't save</returns>
        public bool CheckSingularSave(TreeViewItemData<HierarchyData> treeView)
        {


            if (treeView.data.VisualElement is UndoRedoText undoRedoText)
            {

                if (undoRedoText.ConnectedInfo?.PropertyType == PropertyType.String || undoRedoText.ConnectedInfo?.PropertyType == PropertyType.Integer) return true;

                    foreach (string text in undoRedoText.TextList)
                    {
                        if (!string.IsNullOrEmpty(text)) continue;



                        EditorUtility.DisplayDialog("Can't Save", $"Not all text(s) are filled in in the element: {treeView.data.Name}! Fill in what text(s) you want!", "OK");
                        return false;
                    }
            }

            if (treeView.data.VisualElement is UndoRedoImage undoRedoImage)
            {
                if (undoRedoImage.ConnectedInfo?.PropertyType == PropertyType.Sprite) return true;

                if (undoRedoImage.SpriteList == null) return true;

                foreach (Sprite sprite in undoRedoImage.SpriteList)
                {
                    if (sprite != null) continue;

                    EditorUtility.DisplayDialog("Can't Save", $"Not all sprite(s) are filled in in the element: {treeView.data.Name}! Fill in what sprite(s) you want!", "OK");
                    return false;
                }
            }

            return true;
        }

        public bool RecursiveSaveChecking(IEnumerable<TreeViewItemData<HierarchyData>> children)
        {
        
            foreach (TreeViewItemData<HierarchyData> child in children)
            {

                if (!CheckSingularSave(child)) return false;

                if (child.children == null) continue;

                return RecursiveSaveChecking(child.children);
            }

            return true;
        }
    }
}
