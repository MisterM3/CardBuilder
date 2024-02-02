using UnityEngine.UIElements;

namespace CardBuilder
{
    /// <summary>
    /// ElementPage for Properties other than enums
    /// </summary>
    public class ActiveElementPage : ElementPage<ActiveElementPropertyBox, PropertyInfo>
    {
        public override void Initialize(VisualElement visualElement)
        {
            base.Initialize(visualElement);

            AddBox<AEP_Main>();
            AddBox<AEP_Style>();
        }
    }
}
