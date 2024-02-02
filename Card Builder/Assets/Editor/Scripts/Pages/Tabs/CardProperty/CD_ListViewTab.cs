using CardBuilder.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CardBuilder.Helpers;

namespace CardBuilder
{
    public class CD_ListViewTab : UIPart
    {
        PropertyList m_propertyList;

        public event EventHandler<ChangedValue> onValueChangedEvent;

        public override void Initialize(VisualElement viewWindow)
        {
            base.Initialize(viewWindow);

            ListView listView = m_VisualElement.QLogged<ListView>("PropertyList");

            m_propertyList = CreateInstance<PropertyList>();
            m_propertyList.Initialize(listView);
        }


        public override void SetupUI()
        {

        }

        protected override void RegisterEvents()
        {
            m_propertyList.onValueChangedEvent += OnValueChangedInvoke;
        }

        protected override void UnRegisterEvents()
        {
            m_propertyList.onValueChangedEvent -= OnValueChangedInvoke;
            m_propertyList.Remove();


        }

        public void OnValueChangedInvoke(object o, ChangedValue e)
        {
            onValueChangedEvent?.Invoke(o, e);
        }

        

        public void ResetUnsavedChanges() => m_propertyList.HasUnsavedChanges = false;
        public bool HasUnsavedChanges { get => m_propertyList.HasUnsavedChanges; }

        public void SaveProperties() => m_propertyList.SaveProperties();

        public void DifferentCard(SO_CardData cardData) => m_propertyList.DifferentCard(cardData);

        public void DifferentCard(ChangeEvent<UnityEngine.Object> evt) => m_propertyList.DifferentCard(evt);


        public void DifferentCard(Card card) => m_propertyList.DifCard(card);

        public void ChangeSave(SavedCardDataEditor savedCard) => m_propertyList.ChangeSave(savedCard);




    }
}
