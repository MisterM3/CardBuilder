using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;
using CardBuilder.Helpers;
namespace CardBuilder
{
    public class HierarchyTab : UIPart
    {

        HierarchyTreeView hierarchyTreeView;

        TreeView treeViewHierarchy;

        public EventHandler<IEnumerable<object>> OnTreeViewSelectionChanged;
        public EventHandler<VisualElement> OnTreeViewAddedElement;

        public EventHandler<FocusEvent> OnTreeViewFocus;

        public override void Initialize(VisualElement viewWindow)
        {
            hierarchyTreeView = CreateInstance<HierarchyTreeView>();

            base.Initialize(viewWindow);
        }

        public override void SetupUI()
        {
            treeViewHierarchy = m_VisualElement.QLogged<TreeView>("TreeView");
            
            hierarchyTreeView.Initialize(treeViewHierarchy);
        }


        protected override void RegisterEvents()
        {
            treeViewHierarchy.selectionChanged += OnTreeSelectionInvoke;
            treeViewHierarchy.RegisterCallback<FocusEvent>(OnTreeViewHierarchy);

            hierarchyTreeView.onAddedVisualElement += OnAddedVisualElementInvoke;
        }

        public void OnTreeSelectionInvoke(IEnumerable<object> e)
        {
            OnTreeViewSelectionChanged?.Invoke(this, e);
        }

        public void OnTreeViewHierarchy(FocusEvent e)
        {
            OnTreeViewFocus?.Invoke(this, e);
        }

        public void OnAddedVisualElementInvoke(object sender, VisualElement evt)
        {
            OnTreeViewAddedElement?.Invoke(sender, evt);
        }

        protected override void UnRegisterEvents()
        {
            if (treeViewHierarchy == null) return;
            treeViewHierarchy.selectionChanged -= OnTreeSelectionInvoke;
            treeViewHierarchy.UnregisterCallback<FocusEvent>(OnTreeViewHierarchy);

            hierarchyTreeView.onAddedVisualElement -= OnAddedVisualElementInvoke;
        }

        public void RemoveItem(object sender, EventArgs e)
        {
            if (!EditorUtility.DisplayDialog("Delete Element", "By Deleting this element, you will also delete all the child objects. This action can't be undone. Want to continue?", "Continue", "Cancel")) return;
            
            hierarchyTreeView.RemoveHighlightedItem();
        }

        public void CreateNewImage(object sender, EventArgs e) => hierarchyTreeView.MakeNewImage();


        public void CreateNewText(object sender, EventArgs e) => hierarchyTreeView.MakeNewText();

        public void LoadTemplate(TemplateData templateDataToLOad) => hierarchyTreeView.LoadTemplate(templateDataToLOad);

        public SavingHierarchyList SaveTemplate() => hierarchyTreeView.SaveTemplate();

        public TreeViewItemData<HierarchyData> GetRootItem() => hierarchyTreeView.rootList[0];

        public bool CanSave() => hierarchyTreeView.CanSave();
    }
}
