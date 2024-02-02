using CardBuilder.Data;
using CardBuilder.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardBuilder
{
    using CardBuilder.Helpers;
    public class TemplateDesignerPage : PageSO, ILoadSaveTemplate<TemplateData>
    {

        TemplateData currentTemplate;

        TemplateData TemplateDataSaveTo;

        PropertyListUI propertyListUI;
        AddRemoveElementTab addRemoveElementTab;
        HierarchyTab hierarchyTab;
        ActiveElementTab activeElementTab;
        CardViewerTab cardViewerTab;

        Button menuButton;
        Button saveButton;
        Button makeCardWithTemplateButton;


        public override void Initialize(CardBuilderEditor editor)
        {
            propertyListUI = CreateInstance<PropertyListUI>();
            hierarchyTab = CreateInstance<HierarchyTab>();
            activeElementTab = CreateInstance<ActiveElementTab>();
            addRemoveElementTab = CreateInstance<AddRemoveElementTab>();
            cardViewerTab = CreateInstance<CardViewerTab>();

            base.Initialize(editor);
        }

        public override void OnGUI()
        {
            base.OnGUI();
            cardViewerTab.OnGUI();
            activeElementTab.OnGUI();

        }

        public override void SetupUI()
        {
            SetupToolbar();

            VisualElement parentPropertyList = m_VisualElement.QLogged<VisualElement>("PropertyListParent");
            propertyListUI.Initialize(parentPropertyList);

            VisualElement parentHierarchyList = m_VisualElement.QLogged<VisualElement>("ReplaceHierarchyList");
            hierarchyTab.Initialize(parentHierarchyList);

            VisualElement parentActiveElement = m_VisualElement.QLogged<VisualElement>("ActiveElement");
            activeElementTab.Initialize(parentActiveElement);

            VisualElement parentCardMaker = m_VisualElement.QLogged<VisualElement>("Testr");
            cardViewerTab.Initialize(parentCardMaker);

            VisualElement addElementsParentElement = m_VisualElement.QLogged<VisualElement>("AddElements");
            addRemoveElementTab.Initialize(addElementsParentElement);

            CreateSplitView(m_VisualElement);

        }
        public void SetupToolbar()
        {
            menuButton = m_VisualElement.QLogged<Button>("MenuButton");
            saveButton = m_VisualElement.QLogged<Button>("SaveButton");
            makeCardWithTemplateButton = m_VisualElement.QLogged<Button>("MakeCardWithTemplate");
        }



        public override void CreateGUI()
        {
            base.CreateGUI();
            propertyListUI.CreateGUI();
            addRemoveElementTab.CreateGUI();
            hierarchyTab.CreateGUI();
            activeElementTab.CreateGUI();
            cardViewerTab.CreateGUI();
        }

        public override void Remove()
        {
            UnRegisterEvents();

            propertyListUI.Remove();
            addRemoveElementTab.Remove();
            hierarchyTab.Remove();
            activeElementTab.Remove();
            cardViewerTab.Remove();
            if (m_VisualElement != null)
                m_editor.rootVisualElement.Remove(m_VisualElement);
            currentTemplate = null;
        }


        protected override void RegisterEvents()
        {
            //PropertyList
            propertyListUI.OnListChangeElementSelection += activeElementTab.ChangePropertyEvent;
            propertyListUI.OnPropertyListFocus += activeElementTab.OnFocusInfo;
            propertyListUI.OnListViewItemAdded += activeElementTab.AddPropertyToList;
            propertyListUI.OnListViewItemRemoved += activeElementTab.RemovePropertyFromList;

            //TreeView
            hierarchyTab.OnTreeViewSelectionChanged += activeElementTab.ChangeDataEvent;
            hierarchyTab.OnTreeViewSelectionChanged += cardViewerTab.ChangeElement;
            hierarchyTab.OnTreeViewFocus += activeElementTab.OnFocusData;
            hierarchyTab.OnTreeViewAddedElement += cardViewerTab.AddElementToCardViewer;

            //AddRemovePanel
            addRemoveElementTab.onAddImageClicked += hierarchyTab.CreateNewImage;
            addRemoveElementTab.onAddTextClicked += hierarchyTab.CreateNewText;
            addRemoveElementTab.onRemoveImageClicked += hierarchyTab.RemoveItem;

            //Register Toolbar
            saveButton.clicked += OnClickSaveButton;
            menuButton.clicked += OnClickMenuButton;
            makeCardWithTemplateButton.clicked += OnClickMakeCardWithTemplateButton;

            //Editor
            m_editor.onLoadingTemplate += LoadTemplateByName;
        }

        protected override void UnRegisterEvents()
        {

            //PropertyList
            propertyListUI.OnListChangeElementSelection -= activeElementTab.ChangePropertyEvent;
            propertyListUI.OnPropertyListFocus -= activeElementTab.OnFocusInfo;
            propertyListUI.OnListViewItemAdded -= activeElementTab.AddPropertyToList;
            propertyListUI.OnListViewItemRemoved -= activeElementTab.RemovePropertyFromList;

            //TreeView
            hierarchyTab.OnTreeViewSelectionChanged -= activeElementTab.ChangeDataEvent;
            hierarchyTab.OnTreeViewSelectionChanged -= cardViewerTab.ChangeElement;
            hierarchyTab.OnTreeViewFocus -= activeElementTab.OnFocusData;
            hierarchyTab.OnTreeViewAddedElement -= cardViewerTab.AddElementToCardViewer;

            //AddRemovePanle
            addRemoveElementTab.onAddImageClicked -= hierarchyTab.CreateNewImage;
            addRemoveElementTab.onAddTextClicked -= hierarchyTab.CreateNewText;
            addRemoveElementTab.onRemoveImageClicked -= hierarchyTab.RemoveItem;


            if (saveButton == null) return;
            //Unregister Toolbar
            saveButton.clicked -= OnClickSaveButton;
            menuButton.clicked -= OnClickMenuButton;
            makeCardWithTemplateButton.clicked -= OnClickMakeCardWithTemplateButton;

            //Editor
            m_editor.onLoadingTemplate -= LoadTemplateByName;
        }

        public void OnClickMenuButton()
        {
            int option = EditorUtility.DisplayDialogComplex("Unsaved Progress", "Do you want to save the Template before going to the menu?", "Save", "Discard", "Cancel");

            switch (option)
            {
                case 0:
                    SaveTemplateData();
                    m_editor.SwitchPage(EPages.StartingPage);
                    break;
                case 1:
                    m_editor.SwitchPage(EPages.StartingPage);
                    break;
                case 2:
                    break;
            }
        }

        public void OnClickSaveButton()
        {
            SaveTemplateData();


            //Saves it so the hot reload knows which template to load
            string templatePath = AssetDatabase.GetAssetPath(TemplateDataSaveTo);

            Logs.Info(templatePath);

            EditorPrefs.SetString("OldTemplate", templatePath);
            EditorPrefs.SetString("SaveWay", "TemplateHotReload");
        }

        public void SaveTemplateData()
        {
            TemplateData templateData = SaveTemplate();
            if (templateData == null) return;
            SaveTemplateDataToSO.MakeFile(currentTemplate.templateName, templateData.cardDataSO);
            TreeViewToCard.CreatePrefabFromTreeView(hierarchyTab.GetRootItem(), templateData.templateName);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public void OnClickMakeCardWithTemplateButton()
        {
            int option = EditorUtility.DisplayDialogComplex("Unsaved Progress", "Do you want to save the Template before going to the card page?", "Save", "Discard", "Cancel");

            switch (option)
            {
                case 0:
                    Logs.Info("Save File");
                    //SO_CardData data = listView.SaveData("testingDataSaveFromFile");
                    //TextField field = visualTree.QLogged<TextField>("NameTemplate");
                    //SaveTemplateDataToSO.MakeFile(field.value, data);

                    OnClickSaveButton();
                    //   m_editor.SwitchPage(EPages.CardPropertyEditorPage);
                    break;
                case 1:
                    m_editor.SwitchPage(EPages.CardPropertyEditorPage);
                    break;
                case 2:
                    break;
            }

        
    }


        private void LoadTemplateByName(object sender, string e)
        {
            TemplateData templateData = AssetDatabase.LoadAssetAtPath<TemplateData>(e);

            if (templateData == null)
            {
                Logs.Error("File Chosen not templateData");
            }

            TextField name = m_VisualElement.QLogged<TextField>("NameTemplate");

            name.value = templateData.templateName;

            LoadTemplate(templateData);
        }

        public void LoadTemplate(TemplateData templateToLoad)
        {
            TemplateDataSaveTo = templateToLoad;
            currentTemplate = Instantiate(templateToLoad);
            propertyListUI.LoadTemplate(currentTemplate);
            hierarchyTab.LoadTemplate(currentTemplate);
        }

        public TemplateData SaveTemplate()
        {
            bool canSaveProperties = propertyListUI.CanSave();
            if (!canSaveProperties) return null;

            bool canSaveHierarchy = hierarchyTab.CanSave();
            if (!canSaveHierarchy) return null;


            TemplateDataSaveTo.cardDataSO = propertyListUI.SaveTemplate();
            TemplateDataSaveTo.hierarchyData = hierarchyTab.SaveTemplate();

           

            EditorUtility.SetDirty(TemplateDataSaveTo);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return TemplateDataSaveTo;
        }



        #region SplitView

        private void CreateSplitView(VisualElement visualTree)
        {
            VisualElement twoPanePropertyGraphView = visualTree.QLogged<VisualElement>("TwoPaneReplace");
            twoPanePropertyGraphView.ConvertToSplitView();

            VisualElement previewTwoPane = visualTree.QLogged<VisualElement>("PreviewTwoPane");
            previewTwoPane.ConvertToSplitView(TwoPaneSplitViewOrientation.Vertical);


            VisualElement rightTwoPanePropertyGraphView = visualTree.QLogged<VisualElement>("RightTwoPaneReplace");
            rightTwoPanePropertyGraphView.ConvertToSplitView();


            VisualElement twoRightPanePropertyGraphView = visualTree.QLogged<VisualElement>("RightSideVerticalTwoPane");
            twoRightPanePropertyGraphView.ConvertToSplitView(TwoPaneSplitViewOrientation.Vertical);
        }

        #endregion
    }
}
