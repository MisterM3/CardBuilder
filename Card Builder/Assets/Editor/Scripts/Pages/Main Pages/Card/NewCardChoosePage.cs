
namespace CardBuilder
{
    using UnityEngine;
    using UI;
    using UnityEditor;
    using UnityEngine.UIElements;
    using Unity.VisualScripting;
    using CardBuilder.Data;
    using CardBuilder.Helpers;

    public class NewCardChoosePage : PageSO
    {

        Button startTemplateButton;

        public override void SetupUI()
        {
            startTemplateButton = m_VisualElement.QLogged<Button>("StartTemplateButton");
        }


        protected override void RegisterEvents()
        {
            startTemplateButton.clicked += MakeCard;

            ConnectButtonToPage(m_VisualElement, "BackButton", EPages.StartingPage);
            ConnectButtonToPage(m_VisualElement, "StartVariatyButton", EPages.CardPropertyEditorPage);

        }

        protected override void UnRegisterEvents()
        {
            startTemplateButton.clicked -= MakeCard;

            DisconnectButtonToPage(m_VisualElement, "BackButton", EPages.StartingPage);
            DisconnectButtonToPage(m_VisualElement, "StartVariatyButton", EPages.CardPropertyEditorPage);
        }

        public void MakeCard()
        {

            TemplateData templateData = OpenData();

            m_editor.SwitchPage(EPages.CardPropertyEditorPage);

            Card card = NewCard(templateData.templateName);


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


            var saveEditorObject = CreateInstance<SavedCardDataEditor>();

            savePathEditor = IOMethods.GetRelativeAssetBasePath(savePathEditor);





            string savePath = EditorUtility.SaveFilePanel(
                "Name New Card",
                "Assets",
                "NewCard",
                "asset");

            


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


    }
}
