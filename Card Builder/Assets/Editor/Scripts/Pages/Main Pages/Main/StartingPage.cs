using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardBuilder
{
    using UI;
    using UnityEditor;
    using Data;
    using NewStructs;
    using UnityEditor.Callbacks;
    using CardBuilder.Helpers;
    using Unity.VisualScripting;

    public class StartingPage : PageSO
    {

        Button newCardButton;
        Button editCardButton;
        Button newTemplateButton;
        Button editTemplateButton;

        Button documentationButton;

        public override void SetupUI()
        {
            newCardButton = m_VisualElement.QLogged<Button>("NewCardButton");
            editCardButton = m_VisualElement.QLogged<Button>("EditCardButton");
            newTemplateButton = m_VisualElement.QLogged<Button>("NewTemplateButton");
            editTemplateButton = m_VisualElement.QLogged<Button>("EditTemplateButton");
            documentationButton = m_VisualElement.QLogged<Button>("Documentation");
        }

        private void OnClickEditTemplateButton()
        {
            string pathChosenItem = EditorUtility.OpenFilePanel("Card Template To Use", "Assets", "asset");

            pathChosenItem = IOMethods.GetRelativeAssetBasePath(pathChosenItem);

            TemplateData templateData = AssetDatabase.LoadAssetAtPath<TemplateData>(pathChosenItem);

            while (templateData == null )
            {
                bool cancel = !EditorUtility.DisplayDialog("Chosen File Not Template", "The chosen file is not a template, choose a template", "OK", "Cancel");

                if (cancel) return;

                pathChosenItem = EditorUtility.OpenFilePanel("Card Template To Use", "Assets", "asset");
                pathChosenItem = IOMethods.GetRelativeAssetBasePath(pathChosenItem);
                templateData = AssetDatabase.LoadAssetAtPath<TemplateData>(pathChosenItem);
                

            }

            m_editor.SwitchPage(EPages.TemplateEditorPage);
            m_editor.onLoadingTemplate?.Invoke(this, pathChosenItem);


        }

        protected override void RegisterEvents()
        {
            //Old switch pages, not used because of time constraint
            //newCardButton.clicked += () => m_editor.SwitchPage(EPages.NewCardChoosePage);
            //newTemplateButton.clicked += () => m_editor.SwitchPage(EPages.NewTemplateChoosePage);

            newCardButton.clickable = null;
            newTemplateButton.clickable = null;
            editCardButton.clickable = null;
            editTemplateButton.clickable = null;
            newCardButton.clicked += MakeCard;
            newTemplateButton.clicked += NewTemplateEvent;

            editCardButton.clicked += EditCard;
            editTemplateButton.clicked += OnClickEditTemplateButton;

        }

        protected override void UnRegisterEvents()
        {
            //newCardButton.clicked -= () => m_editor.SwitchPage(EPages.NewCardChoosePage);
            //newTemplateButton.clicked -= () => m_editor.SwitchPage(EPages.NewTemplateChoosePage);

            if (newCardButton == null) return;

            newCardButton.clicked -= MakeCard;
            newTemplateButton.clicked -= NewTemplateEvent;

            editCardButton.clicked -= EditCard;
            editTemplateButton.clicked -= OnClickEditTemplateButton;
        }
        private void NewTemplateEvent()
        {
            string data = MakeNewFile();

            if (string.IsNullOrEmpty(data)) return;
            m_editor.SwitchPage(EPages.TemplateEditorPage);
            m_editor.onLoadingTemplate?.Invoke(this, data);
        }



        private string MakeNewFile()
        {
            string savePath = EditorUtility.SaveFilePanel(
            "Save new Template",
            "Assets",
            "NewTemplate",
            "asset"
            );

            if (string.IsNullOrEmpty(savePath))
            {
                return null;
            }

            string templateData = StartingScripts.SaveLoadMainFile(savePath);

            return templateData;

        }
        public void MakeCard()
        {

            TemplateData templateData = OpenData();

            if (templateData == null) return;

            m_editor.SwitchPage(EPages.CardPropertyEditorPage);

            Card card = NewCard(templateData.templateName);

            if (card == null)
            {
                m_editor.SwitchPage(EPages.StartingPage);
                return;
            }

            m_editor.onTemplateChooses?.Invoke(this, templateData);
            m_editor.onCardChoosen?.Invoke(this, card);
        }

        public Card NewCard(string name)
        {
            string savePathEditor = EditorUtility.SaveFilePanel(
            "Name Editor Card",
            "Assets",
            "NewCard",
            "asset");

            if (string.IsNullOrEmpty(savePathEditor))
            {
                m_editor.SwitchPage(EPages.StartingPage);
                return null;
            }

            var saveEditorObject = CreateInstance<SavedCardDataEditor>();

            savePathEditor = IOMethods.GetRelativeAssetBasePath(savePathEditor);





            string savePath = EditorUtility.SaveFilePanel(
                "Name New Card",
                "Assets",
                "NewCard",
                "asset");


            if (string.IsNullOrEmpty(savePath))
            {
                m_editor.SwitchPage(EPages.StartingPage);
                return null;
            }

            savePath = IOMethods.GetRelativeAssetBasePath(savePath);

            int lastIindex = savePath.LastIndexOf('/');

            string lastPart = savePath.Substring(lastIindex + 1);

            lastPart = lastPart.Remove(lastPart.IndexOf('.'));



            var newScriptableObject = CreateInstance($"{name.FirstCharacterToUpper()}CardData");
            AssetDatabase.CreateAsset(newScriptableObject, savePath);

            Card card = AssetDatabase.LoadAssetAtPath<Card>(savePath);

            saveEditorObject.nameCard = lastPart;
            saveEditorObject.card = card;


            string[] assets = AssetDatabase.FindAssets($"{name}Prefab");
            string assetPath = AssetDatabase.GUIDToAssetPath(assets[0]);
            saveEditorObject.TemplatePrefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            string[] assets2 = AssetDatabase.FindAssets($"{name}CardDataLoading");
            string assetPath2 = AssetDatabase.GUIDToAssetPath(assets2[0]);
            saveEditorObject.cardDataSO = AssetDatabase.LoadAssetAtPath<SO_CardData>(assetPath2);

            newScriptableObject.GetType().GetProperties();



            AssetDatabase.CreateAsset(saveEditorObject, savePathEditor);

            m_editor.OnSaveChoosen?.Invoke(this, saveEditorObject);


            return card;
        }


        public TemplateData OpenData()
        {
            string pathChosenItem = EditorUtility.OpenFilePanel("Card Template To Use", "Assets", "asset");
            pathChosenItem = IOMethods.GetRelativeAssetBasePath(pathChosenItem);
            TemplateData templateData = AssetDatabase.LoadAssetAtPath<TemplateData>(pathChosenItem);




            while (templateData == null)
            {
                bool cancel = !EditorUtility.DisplayDialog("Chosen File Not Template", "The chosen file is not a template, choose a template", "OK", "Cancel");

                //Cancel entire method if player doesn't pick a card and cancels
                if (cancel) return null;

                pathChosenItem = EditorUtility.OpenFilePanel("Card Template To Use", "Assets", "asset");
                pathChosenItem = IOMethods.GetRelativeAssetBasePath(pathChosenItem);
                templateData = AssetDatabase.LoadAssetAtPath<TemplateData>(pathChosenItem);
            }

            return templateData;
        }



        public void EditCard()
        {
            string pathChosenItem = EditorUtility.OpenFilePanel("Card To Edit", "Assets", "asset");
            pathChosenItem = IOMethods.GetRelativeAssetBasePath(pathChosenItem);
            SavedCardDataEditor savedCardData = AssetDatabase.LoadAssetAtPath<SavedCardDataEditor>(pathChosenItem);

            while (savedCardData == null)
            {
                bool cancel = !EditorUtility.DisplayDialog("Chosen File Not CardSave", "The chosen file is not a cardSave, choose a cardSave", "OK", "Cancel");

                //Don't continue the entire method if player cancels choosing tempalte
                if (cancel) return;
                
                pathChosenItem = EditorUtility.OpenFilePanel("Card To Edit", "Assets", "asset");
                pathChosenItem = IOMethods.GetRelativeAssetBasePath(pathChosenItem);
                savedCardData = AssetDatabase.LoadAssetAtPath<SavedCardDataEditor>(pathChosenItem);
            }

            m_editor.SwitchPage(EPages.CardPropertyEditorPage);

            Debug.LogError(savedCardData);
            m_editor.OnSaveChoosen?.Invoke(this, savedCardData);
        }

        [DidReloadScripts]
        public static void OnCompileScripts()
        {
            string saveKey = EditorPrefs.GetString("SaveWay");
            EditorPrefs.SetString("SaveWay", "");


            switch (saveKey)
            {
                case "NewTemplate":
                    LoadNewScript();
                    break;
                case "SaveTemplate":
                    FinalizeSavePrefab();
                    break;
                case "TemplateHotReload":
                    CardBuilderEditor.didHotReloadTemplate = true;
                    break;
            }
        }

        public static void FinalizeSavePrefab()
        {
          //  TreeViewToCard.FinalizeScriptToPrefab();
        }


        public static void LoadNewScript()
        {

            string path = EditorPrefs.GetString("SavePath");

            TemplateData data = AssetDatabase.LoadAssetAtPath<TemplateData>(path);

        }

    }
}
