namespace CardBuilder.NewStructs
{
    using UnityEngine.UIElements;

    //Used to list properties
    public struct PropertyShow
    {
        public string label;
        public VisualElement connectedVisualElement;
        public PropertyType type;
        public PropertyEditorStyle style;
        public int indexOfList;

        public PropertyShow(string label, PropertyType type, PropertyEditorStyle style, int index, VisualElement connectedVisualElement)
        {
            this.label = label;
            this.type = type;
            this.style = style;
            indexOfList = index;

            this.connectedVisualElement = connectedVisualElement;

        }
    }
}
