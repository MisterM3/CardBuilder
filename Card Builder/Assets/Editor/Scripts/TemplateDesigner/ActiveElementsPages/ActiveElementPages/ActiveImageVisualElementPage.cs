using UnityEngine.UIElements;

namespace CardBuilder
{
    public class ActiveImageVisualElementPage : ActiveVisualElementPage
    {
        public override void Initialize(VisualElement visualElement)
        {
            base.Initialize(visualElement);
            AddBox<AEVE_Image>();
        }

    }
}
