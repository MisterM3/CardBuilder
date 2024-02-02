

namespace CardBuilder
{
    using System;
    using System.Collections;
    using System.IO;
    using UI;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.UIElements;
    using CardBuilder.Helpers;
    public class NewTemplateChoosePage : PageSO
    {

        Button backButton;
        Button newTemplateButton;

        public override void SetupUI()
        {
            backButton = m_VisualElement.QLogged<Button>("BackButton");
            newTemplateButton = m_VisualElement.QLogged<Button>("BlankTemplateButton");
        }

        protected override void RegisterEvents()
        {
            backButton.clicked += BackButtonClicked;

            newTemplateButton.clicked += NewTemplateEvent;
            
            ConnectButtonToPage(m_VisualElement, "VariantTemplateButton", EPages.TemplateEditorPage);

        }
        protected override void UnRegisterEvents()
        {
            backButton.clicked -= BackButtonClicked;

            newTemplateButton.clicked -= NewTemplateEvent;

            DisconnectButtonToPage(m_VisualElement, "VariantTemplateButton", EPages.TemplateEditorPage);
        }

        private void BackButtonClicked()
        {
            m_editor.SwitchPage(EPages.StartingPage);
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

    }


}
