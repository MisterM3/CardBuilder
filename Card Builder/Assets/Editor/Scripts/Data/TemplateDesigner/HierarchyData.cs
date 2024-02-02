using System;
using UnityEngine;

namespace CardBuilder
{
    [Serializable]
    public class HierarchyData : IHierarchyListData
    {
        [SerializeField] private string nameData;
        public string Name 
        { 
            get => nameData; 
            set => nameData = value; 
        }

        [SerializeField] private UndoRedoText visualText;
        [SerializeField] private UndoRedoImage visualImage;
        [SerializeField] public UndoRedoVisualElement VisualElement
        {
            get 
            {
                switch (VisualElementType)
                {
                    case VisualElementType.Image:
                        return visualImage;
                    case VisualElementType.Text:
                        return visualText;
                    default:
                        Logs.Error("Not known VisualElementType");
                        return null;
                }
            }
            set
            {
                switch (VisualElementType)
                {
                    case VisualElementType.Image:
                        visualImage = (UndoRedoImage)value;
                        break;
                    case VisualElementType.Text:
                        visualText = (UndoRedoText)value;
                        break;
                    default:
                        Logs.Error("Not known VisualElementType");
                        break;
                }
            }
        }

        [SerializeField] private VisualElementType visualElementType;

        public VisualElementType VisualElementType 
        { 
            get => visualElementType; 
            set => visualElementType = value; 
        }

        public Vector2 Position 
        { 
            get => VisualElement.Position;
            set => VisualElement.Position = value;
        }

        public Vector2 Size
        {
            get => VisualElement.Size;
            set => VisualElement.Size = value;
        }

        public HierarchyData Copy()
        {
            HierarchyData newData = new();

            newData.Name = Name;
            newData.VisualElementType = VisualElementType;
            if (newData.visualText != null)
                 newData.visualText = visualText.Copy();
            if (newData.visualImage != null)
                newData.visualImage = visualImage.Copy();
            newData.Position = Position;
            newData.Size = Size;
            
            return newData;
        }

    }
}
