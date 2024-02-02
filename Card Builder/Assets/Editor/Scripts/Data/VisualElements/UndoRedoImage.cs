using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace CardBuilder
{
    [Serializable]
    public class UndoRedoImage : UndoRedoVisualElement
    {

    //    [SerializeField] Sprite image;


        [SerializeField] private List<Sprite> spriteList;
        public Sprite Image
        {
            get {
                if (spriteList == null) spriteList = new();
                if (spriteList.Count == 0) spriteList.Add(null);
                return spriteList[0];
            }

            set {
                if (spriteList == null) spriteList = new();
                if (spriteList.Count == 0) spriteList.Add(value);
                else spriteList[0] = value;
                VisualElement.style.backgroundImage = new StyleBackground(value);
            }
        }

        public List<Sprite> SpriteList
        {
            get => spriteList;

            set
            {
                spriteList = value;
            }
        }


        public new void Init()
        {
            base.Init();

            if (spriteList != null && spriteList.Count > 0)
                VisualElement.style.backgroundImage = new StyleBackground(Image);
        }

        public UndoRedoImage Copy()
        {
            return new UndoRedoImage
            {
                nameObject = this.nameObject,
                Position = this.Position,
                Size = this.Size,
                ConnectedInfo = this.ConnectedInfo.Copy(),
                SpriteList = new List<Sprite>(spriteList)
            };
        }
    }
}
