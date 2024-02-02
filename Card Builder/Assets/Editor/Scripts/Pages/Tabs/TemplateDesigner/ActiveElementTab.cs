using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CardBuilder.Helpers;
namespace CardBuilder
{
    public class ActiveElementTab : UIPart
    {

        ActiveElementPanel activeElementPanel;

        ScrollView scrollView;


        public override void Initialize(VisualElement viewWindow)
        {
            activeElementPanel = CreateInstance<ActiveElementPanel>();

            base.Initialize(viewWindow);
        }

        public override void SetupUI()
        {
            scrollView = m_VisualElement.QLogged<ScrollView>("ActiveElementList");
            activeElementPanel.Initialize(scrollView);
        }

        public override void OnGUI()
        {
            activeElementPanel.OnGUI();
        }

        protected override void RegisterEvents()
        {
            
        }

        protected override void UnRegisterEvents()
        {
            activeElementPanel.ResetTab();
        }


        public void AddPropertyToList(object sender, PropertyInfo info) => activeElementPanel.AddPropertyToList(sender, info);
        public void RemovePropertyFromList(object sender, PropertyInfo info) => activeElementPanel.RemovePropertyFromList(sender, info);

        public void OnFocusData(object sender, EventArgs e) => activeElementPanel.OnFocusData();
        public void OnFocusData(object sender, FocusEvent e) => activeElementPanel.OnFocusData();
        public void OnFocusInfo(object sender, EventArgs e) => activeElementPanel.OnFocusInfo();

        public void ChangePropertyEvent(object sender, IEnumerable<object> obj) => activeElementPanel.ChangePropertyEvent(sender, obj);

        public void ChangeDataEvent(IEnumerable<object> obj) => activeElementPanel.ChangeDataEvent(obj);

        public void ChangeDataEvent(object sender, IEnumerable<object> obj) => activeElementPanel.ChangeDataEvent(obj);



    }
}
