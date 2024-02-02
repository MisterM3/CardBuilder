using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardBuilder
{
    public abstract class UIPart : ScriptableObject
    {

        protected VisualElement m_viewWindow;

        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        protected VisualElement m_VisualElement;

        public virtual void Initialize(VisualElement viewWindow)
        {
            m_viewWindow = viewWindow;

            if (m_VisualTreeAsset != null)
                m_VisualElement = m_VisualTreeAsset.CloneTree();

            SetupUI();
        }

        public abstract void SetupUI();

        public void CreateGUI()
        {
            m_viewWindow.Add(m_VisualElement);
            RegisterEvents();
        }

        protected abstract void RegisterEvents();

        protected abstract void UnRegisterEvents();

        public virtual void OnGUI() { }

        public virtual void Remove()
        {
            UnRegisterEvents();

            if (m_VisualElement != null)
                m_viewWindow.Remove(m_VisualElement);

        }
    }
}
