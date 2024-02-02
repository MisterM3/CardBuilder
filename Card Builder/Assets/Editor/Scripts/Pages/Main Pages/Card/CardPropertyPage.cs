using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardBuilder
{
    using Data;
    using System;
    using System.Linq;
    using UI;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine.UIElements;
    using CardBuilder.Helpers;
    public class CardPropertyPage : PageSO
    {

        CD_ListViewTab m_listViewTab;
        CD_PreviewCardTab m_previewCardTab;

        public SavedCardDataEditor currentCard;




        public static EventHandler OnCardUpdated;


        ObjectField objectField;

        Button menuButton;
        Button saveButton;


        TextField cardNameTF;
        TextField templateNameTF;


        public override void CreateGUI()
        {
            base.CreateGUI();

            m_listViewTab.CreateGUI();
            m_previewCardTab.CreateGUI();

        }

        public override void Initialize(CardBuilderEditor editor)
        {
            m_previewCardTab = CreateInstance<CD_PreviewCardTab>();

            base.Initialize(editor);

            VisualElement image = m_VisualElement.QLogged<VisualElement>("PreviewText");
            m_previewCardTab.Initialize(image);

            cardNameTF = m_VisualElement.QLogged<TextField>("TF_CardName");
            templateNameTF = m_VisualElement.QLogged<TextField>("TF_TemplateName");

            VisualElement middle = m_VisualElement.QLogged<VisualElement>("NodeGraphSide");
            m_listViewTab = CreateInstance<CD_ListViewTab>();
            m_listViewTab.Initialize(middle);
        }


        /*
        public override void OnDestroyGUI()
        {
            Button menuButton = visualElement.QLogged<Button>("MenuButton");
            menuButton.clicked -= GoMenuButtonClicked;
        }
        */


        public void GoMenuButtonClicked()
        {
            if (m_listViewTab.HasUnsavedChanges)
            {
                int option = EditorUtility.DisplayDialogComplex("Unsaved Progress", "Do you want to save the card before going to the effect page?", "Save", "Discard", "Cancel");

                switch (option)
                {
                    case 0:
                        Logs.Info("Save File");
                        m_listViewTab.SaveProperties();
                        m_editor.SwitchPage(EPages.StartingPage);
                        break;
                    case 1:
                        m_listViewTab.ResetUnsavedChanges();
                        m_editor.SwitchPage(EPages.StartingPage);
                        break;
                    case 2:
                        break;
                }
            }
            else m_editor.SwitchPage(EPages.StartingPage);
        }

        public void OnSaveButtonClicked()
        {
            m_listViewTab.SaveProperties();
        }

        public void DifferentTemplate(ChangeEvent<UnityEngine.Object> evt)
        {
            m_listViewTab.DifferentCard(evt);
        }

        public void ChangeSave(SavedCardDataEditor savedCard)
        {
            currentCard = savedCard;

            Debug.Log(currentCard.nameCard);

            cardNameTF.value = currentCard.nameCard;
            templateNameTF.value = currentCard.cardDataSO.name.Replace("CardDataLoading", "");

            m_listViewTab.ChangeSave(currentCard);
            m_previewCardTab.ChangePrefab(currentCard);
        }

        public override void OnGUI()
        {
            base.OnGUI();
            m_previewCardTab.OnGUI();
        }


        public void DifferentTemplate(SO_CardData cardData)
        {
            m_listViewTab.DifferentCard(cardData);
        }

        public void DifferentCard(Card card)
        {
            m_listViewTab.DifferentCard(card);
        }


        #region SplitView

        private void CreateSplitView(VisualElement visualTree)
        {
            VisualElement twoPanePropertyGraphView = visualTree.QLogged<VisualElement>("TwoPaneReplace");
            twoPanePropertyGraphView.ConvertToSplitView();
        }


        

        protected override void RegisterEvents()
        {

            m_listViewTab.onValueChangedEvent += m_previewCardTab.ChangeValues;

            menuButton.clicked += GoMenuButtonClicked;
            saveButton.clicked += OnSaveButtonClicked;
            objectField.RegisterValueChangedCallback(DifferentTemplate);

            m_editor.OnSaveChoosen += ChangeSave;

            m_editor.onTemplateChooses += DifferentTemplate;

            m_editor.onCardChoosen += DifferentCard;

            


        }
        protected override void UnRegisterEvents()
        {


            m_listViewTab.onValueChangedEvent -= m_previewCardTab.ChangeValues;

            menuButton.clicked -= GoMenuButtonClicked;
           saveButton.clicked -= OnSaveButtonClicked;
            objectField.UnregisterValueChangedCallback(DifferentTemplate);

            m_editor.OnSaveChoosen -= ChangeSave;

            m_editor.onTemplateChooses -= DifferentTemplate;

            m_editor.onCardChoosen -= DifferentCard;

            
        }

        private void DifferentTemplate(object o, TemplateData newTemplateData) => DifferentTemplate(newTemplateData.cardDataSO);
        private void DifferentCard(object o, Card newCard) => DifferentCard(newCard);
        private void ChangeSave(object o, SavedCardDataEditor savedCard) => ChangeSave(savedCard);

        public override void Remove()
        {
            m_previewCardTab.Remove();
            m_listViewTab.Remove();
            base.Remove();
        }




        public override void SetupUI()
        {
            saveButton = m_VisualElement.QLogged<Button>("SaveButton");
            menuButton = m_VisualElement.QLogged<Button>("MenuButton");

            m_listViewTab = CreateInstance<CD_ListViewTab>();

            objectField = m_VisualElement.QLogged<ObjectField>("ObjectField");

       //     previewElement = m_VisualElement.QLogged<VisualElement>("PreviewImage");

            CreateSplitView(m_VisualElement);
        }



        #endregion
    }
}
