namespace CardBuilder
{
    using UnityEngine;
    using UnityEngine.UIElements;

    public interface IPartUI
    {
        [property: SerializeField] VisualTreeAsset m_VisualTreeAsset { get; }
        protected VisualElement m_VisualElement { get; set; }

        protected VisualElement m_viewWindow { get; set; }

        public void Initialize(VisualElement rootWindow)
        {
            m_viewWindow = rootWindow;

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

        public virtual void OnGUI() { }

    }
}
