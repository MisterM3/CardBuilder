using UnityEngine.UIElements;

namespace CardBuilder
{
    /// <summary>
    /// Element Page for a Enum Property (Used for UI to conenct enum files)
    /// </summary>
    public class EnumActiveElementPage : ActiveElementPage
    {
        public override void Initialize(VisualElement visualElement)
        {
            base.Initialize(visualElement);

            AddBox<AEP_Enum>();
        }
    }
}
