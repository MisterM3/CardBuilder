using CardBuilder.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CardBuilder.Helpers;

namespace CardBuilder
{
    public class PropertyListUI : UIPart
    {

        TemplateDesignerListView listView;

        ListView list;
        Button addPropertyButton;
        Button removePropertyButton;

        public EventHandler<IEnumerable<object>> OnListChangeElementSelection;
        public EventHandler<PropertyInfo> OnListViewItemRemoved;
        public EventHandler<PropertyInfo> OnListViewItemAdded;
        public EventHandler OnPropertyListFocus;


        public override void Initialize(VisualElement viewWindow)
        {
            listView = CreateInstance<TemplateDesignerListView>();

            base.Initialize(viewWindow);
        }

        public override void SetupUI()
        {
            list = m_VisualElement.QLogged<ListView>("PropertyListView");
            listView.Initialize(list);

            addPropertyButton = m_VisualElement.QLogged<Button>("AddPropertyButton");
            removePropertyButton = m_VisualElement.QLogged<Button>("RemovePropertyButton");

        }

        protected override void RegisterEvents()
        {
            listView.OnChangeElementSelection += OnListChangeInvoke;
            listView.OnItemRemoved += OnListRemoved;
            listView.OnItemAdded += OnListAdded;
            list.RegisterCallback<FocusEvent>(OnFocusPassThrough);

           

            addPropertyButton.clicked += listView.AddStandardItem;
            removePropertyButton.clicked += listView.RemoveSelectedItem;
        }

        private void OnFocusPassThrough(FocusEvent evt)
        {
            OnPropertyListFocus?.Invoke(this, EventArgs.Empty);
        }

 


        protected override void UnRegisterEvents()
        {

            listView.OnChangeElementSelection -= OnListChangeInvoke;
            listView.OnItemRemoved -= OnListRemoved;
            listView.OnItemAdded -= OnListAdded;
            if (list == null) return;

                list.UnregisterCallback<FocusEvent>(OnFocusPassThrough);

                addPropertyButton.clicked -= listView.AddStandardItem;
                removePropertyButton.clicked -= listView.RemoveSelectedItem;
        }

        private void OnListChangeInvoke(object sender, IEnumerable<object> e)
        {
            OnListChangeElementSelection?.Invoke(sender, e);
        }
        private void OnListRemoved(object sender, PropertyInfo e)
        {
            OnListViewItemRemoved?.Invoke(sender, e);
        }
        private void OnListAdded(object sender, PropertyInfo e)
        {
            OnListViewItemAdded?.Invoke(sender, e);
        }

        public override void Remove()
        {
            listView.RemoveAllItems();

            base.Remove();

        }

        public void LoadTemplate(TemplateData templateDataToLoad) => listView.LoadTemplate(templateDataToLoad);
        public SO_CardData SaveTemplate() => listView.SaveTemplate();

        public bool CanSave() => listView.CanSave();
    }
}
