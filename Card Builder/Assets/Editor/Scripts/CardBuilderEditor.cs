namespace CardBuilder
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;

    using UI;
    /// <summary>
    /// 
    /// </summary>
    
    //First Run
    public class CardBuilderEditor : EditorWindow
    {


        private Dictionary<EPages, PageSO> pages;
        private PageSO currentPage;

        //Change these (dunno how yet)
        public EventHandler<string> onLoadingTemplate;
        public EventHandler<TemplateData> onTemplateChooses;
        public EventHandler<Card> onCardChoosen;
        public EventHandler<SavedCardDataEditor> OnSaveChoosen;

        public static bool didHotReloadTemplate = false;

        [MenuItem("Tools/Card Builder")]
        public static void OpenWindow()
        {
            
            EditorWindow window = GetWindow<CardBuilderEditor>();

            window.minSize = new Vector2(500, 420);
            window.titleContent = new GUIContent("Card Builder");

            
        }

        public void OnGUI()
        {
            if (currentPage != null)
                currentPage.OnGUI();
        }

        public void CreateGUI()
        {
            SetupPages();

            if (didHotReloadTemplate)
                HotReload();
        }

        public void HotReload()
        {
            string templatePath = EditorPrefs.GetString("OldTemplate");
            EditorPrefs.SetString("OldTemplate", "");


            SwitchPage(EPages.TemplateEditorPage);
            onLoadingTemplate?.Invoke(null, templatePath);

            didHotReloadTemplate = false;

        }


        #region Pages

        public void SetupPages()
        {
            pages = new Dictionary<EPages, PageSO>();

            AddPageToDictionary<StartingPage>(EPages.StartingPage);

            
            AddPageToDictionary<BlankTestPage>(EPages.BlankPage);
            AddPageToDictionary<NewCardChoosePage>(EPages.NewCardChoosePage);
            AddPageToDictionary<NewTemplateChoosePage>(EPages.NewTemplateChoosePage);

            AddPageToDictionary<CardPropertyPage>(EPages.CardPropertyEditorPage);
            AddPageToDictionary<TemplateDesignerPage>(EPages.TemplateEditorPage);
            AddPageToDictionary<SettingsPage>(EPages.Settings);


            foreach (PageSO page in pages.Values) page.Initialize(this);

            SwitchPage(EPages.StartingPage);

            
        }

        public void AddPageToDictionary<Tpage>(EPages pageType) where Tpage : PageSO
        {
            Tpage page = ScriptableObject.CreateInstance<Tpage>();
            pages.Add(pageType, page);
        }

        public PageSO GetPage(EPages pageEnum)
        {
            return pages[pageEnum];
        }

        public void SwitchPage(EPages pageEnum)
        {
            PageSO pageToLoad = pages[pageEnum];


            if (pageToLoad == null)
            {
                Logs.Error("New Page Not Found!");
                return;
            }

            if (currentPage != null) currentPage.Remove();
           // if (oldPage != null) root.Remove(oldPage);


            pageToLoad.CreateGUI();
            currentPage = pageToLoad;
        }

        #endregion


        public void OnDestroy()
        {
            if (currentPage != null) currentPage.Remove();
        }
    }
}
