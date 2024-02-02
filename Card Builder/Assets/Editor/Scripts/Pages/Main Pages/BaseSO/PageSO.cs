using CardBuilder.Helpers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardBuilder.UI
{

    public abstract class PageSO : ScriptableObject
    {
        [SerializeField]
        protected VisualTreeAsset m_VisualTreeAsset = default;

        protected VisualElement m_VisualElement;

        protected CardBuilderEditor m_editor;

        public virtual void Initialize(CardBuilderEditor editor)
        {
            m_editor = editor;

            if (m_VisualTreeAsset == null)
            {
                Logs.Error("No visualTreeAsset attached to page!");
                return;
            }
            
            m_VisualElement = m_VisualTreeAsset.CloneTree();

            SetupUI();
        }

        public abstract void SetupUI();
        
        public virtual void CreateGUI()
        {
            m_editor.rootVisualElement.Add(m_VisualElement);
            RegisterEvents();
        }

        protected abstract void RegisterEvents();

        protected abstract void UnRegisterEvents();

        public virtual void OnGUI() { }

        public virtual void Remove()
        {
            UnRegisterEvents();

            if (m_VisualElement == null)
            {
                Logs.Error("No VisualElement active on page!");
                return;
            }

            m_editor.rootVisualElement.Remove(m_VisualElement);
        }



        //Maybe change or remove
        protected void ConnectButtonToPage(VisualElement visualTree, string buttonName, EPages pageToGoTo)
        {
            Button button = visualTree.QLogged<Button>(buttonName);

            if (button == null)
            {
                Logs.Error("No Button Found, button name incorrect?" + buttonName);
                return;
            }

            button.clicked += () => m_editor.SwitchPage(pageToGoTo);
        }

        protected void DisconnectButtonToPage(VisualElement visualTree, string buttonName, EPages pageToGoTo)
        {
            Button button = visualTree.QLogged<Button>(buttonName);

            if (button == null)
            {
                Logs.Error("No Button Found, button name incorrect?" + buttonName);
                return;
            }

            button.clicked -= () => m_editor.SwitchPage(pageToGoTo);
        }
    }
}
