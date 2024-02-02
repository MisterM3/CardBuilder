using System;
using UnityEngine.UIElements;
using CardBuilder.Helpers;
using UnityEngine;

namespace CardBuilder
{
    public class AddRemoveElementTab : UIPart
    {

        Button addImageButton;
        Button addTextButton;
        Button addRemoveElement;

        public EventHandler onAddImageClicked;
        public EventHandler onAddTextClicked;
        public EventHandler onRemoveImageClicked;

        public override void SetupUI()
        {
            addImageButton = m_VisualElement.QLogged<Button>("AddImageButton");
            addTextButton = m_VisualElement.QLogged<Button>("AddTextButton");
            addRemoveElement = m_VisualElement.QLogged<Button>("RemoveElementButton");
        }

        protected override void RegisterEvents()
        {
            addImageButton.clickable = null;
            addTextButton.clickable = null;
            addRemoveElement.clickable = null;

            addImageButton.clicked += AddImagePassthrough;
            addTextButton.clicked += AddTextPassthrough;
            addRemoveElement.clicked += AddRemovePassthrough;
        }

        private void AddImagePassthrough()
        {
            onAddImageClicked?.Invoke(this, EventArgs.Empty);
        }

        private void AddTextPassthrough()
        {
            onAddTextClicked?.Invoke(this, EventArgs.Empty);
        }

        private void AddRemovePassthrough()
        {
            onRemoveImageClicked?.Invoke(this, EventArgs.Empty);
        }

        protected override void UnRegisterEvents()
        {

            if (addImageButton != null)
                addImageButton.clicked += AddImagePassthrough;
            if (addTextButton != null)
                addTextButton.clicked += AddTextPassthrough;
            if (addRemoveElement != null)
                addRemoveElement.clicked += AddRemovePassthrough;
        }
    }
}
