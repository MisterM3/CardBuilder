using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CardBuilder.Helpers;

namespace CardBuilder
{
    public class CardViewerTab : UIPart
    {

        CardViewMaker cardVisualMaker;

        public override void Initialize(VisualElement viewWindow)
        {
            cardVisualMaker = CreateInstance<CardViewMaker>();

            base.Initialize(viewWindow);
        }


        public override void SetupUI()
        {
            VisualElement cardVisualElement = m_viewWindow.QLogged<VisualElement>("Testr");
            cardVisualMaker.Initialize(cardVisualElement);
        }

        protected override void RegisterEvents()
        {
            
        }

        protected override void UnRegisterEvents()
        {
            
        }

        public void AddElementToCardViewer(object sender, VisualElement newElement)
        {
            m_VisualElement.Add(newElement);
        }

        public override void Remove()
        {
            base.Remove();

            if (m_VisualElement == null) return;
            m_VisualElement.Clear();
        }

        public override void OnGUI()
        {
            cardVisualMaker.OnGUI();
        }

        public void ChangeElement(object sender, IEnumerable<object> obj) => cardVisualMaker.ChangeElement(obj);


    }
}
