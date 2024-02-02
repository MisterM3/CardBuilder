using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using CardBuilder.Helpers;

namespace CardBuilder
{
    public class CD_PreviewCardTab : UIPart
    {

        PreviewCard m_previewCard;



        public override void Initialize(VisualElement viewWindow)
        {


            base.Initialize(viewWindow);

            m_previewCard = CreateInstance<PreviewCard>();

            VisualElement previewElement = m_VisualElement.QLogged<VisualElement>("PreviewImage");
            m_previewCard.Initialize(previewElement);
            m_previewCard.SetupCamera();

        }

        public override void OnGUI()
        {
            base.OnGUI();
            m_previewCard.OnGUI();
        }

       



        protected override void RegisterEvents()
        {
          
            //   throw new System.NotImplementedException();
        }

        protected override void UnRegisterEvents()
        {
           // m_previewCard.Remove();
        }

        public override void Remove()
        {
            m_previewCard.Remove();
            base.Remove();
        }



        public void ChangePrefab(SavedCardDataEditor savedCard)
        {

            Type type = savedCard.card.GetType();

            Debug.Log(savedCard.card);

            Debug.Log(type);

            MethodInfo methodInfo = typeof(PreviewCard).GetMethod("NewPrefab");

            Logs.Info(methodInfo.Name);

 
            MethodInfo genericMethodInfo = methodInfo.MakeGenericMethod(type);


            genericMethodInfo.Invoke(m_previewCard, new object[] { savedCard });

            //  m_previewCard.NewPrefab<type>(savedCard);

        }

        public void RenderPreviewCard()  {
        }

        public override void SetupUI()
        {
           
        }

        internal void ChangeValues(object sender, ChangedValue e)
        {
            m_previewCard.ChangeValue(sender, e);
        }
    }
}
