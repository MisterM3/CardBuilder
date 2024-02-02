using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardBuilder
{
    public abstract class ActiveElementBox<Tdata> : ScriptableObject
    {
        public Tdata activeElement;

        protected VisualElement m_viewWindow;

        [SerializeField]
        private VisualTreeAsset m_VisualTreeAsset = default;

        protected VisualElement m_VisualElement;
        public virtual void BindItem(Tdata item)
        {
            activeElement = item;
        }

        public void Initialize(VisualElement rootWindow)
        {
            m_viewWindow = rootWindow;
            
            if (m_VisualTreeAsset != null)
                m_VisualElement = m_VisualTreeAsset.CloneTree();

            SetupUI();
        }

        public abstract void SetupUI();

        public abstract void RemoveUI();


        public void CreateGUI()
        {
            m_viewWindow.Add(m_VisualElement);
            RegisterEvents();
        }


        /// <summary>
        /// Used to add events to text
        /// </summary>
        protected abstract void RegisterEvents();

        /// <summary>
        /// Used to remove events
        /// </summary>
        protected abstract void UnRegisterEvents();

        public void Remove()
        {

            if (m_VisualElement != null)
                m_viewWindow.Remove(m_VisualElement);

            UnRegisterEvents();
        }

        public virtual void OnGUI()
        {

        }
    }
}
