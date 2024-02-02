namespace CardBuilder
{
    using System;
    using UnityEditor;
    using UnityEngine;

    //Base class for all propertyTypes (types listed below)
    [Serializable]
    public class Properties<T>
    {
        [SerializeField]
        private string propertyLabel;
        public string PropertyLabel { get => propertyLabel; set => propertyLabel = value; }
        [SerializeField]
        private T value;
        public T Value { get => value; set => this.value = value; }
        [SerializeField]
        private PropertyEditorStyle style;
        public PropertyEditorStyle Style { get => style; set => style = value; }

        public Properties(string label, T value, PropertyEditorStyle style)
        {
            PropertyLabel = label;
            Value = value;
            Style = style;
        }
    }

    [Serializable]
    public class IntProperty : Properties<int>
    {
        public IntProperty(string label, int value, PropertyEditorStyle style) 
                            : base(label, value, style) { }
    }

    [Serializable]
    public class StringProperty : Properties<string>
    {
        public StringProperty(string label, string value, PropertyEditorStyle style)
                            : base(label, value, style) { }
    }

    [Serializable]
    public class SpriteProperty : Properties<Sprite>
    {
        public SpriteProperty(string label, Sprite value, PropertyEditorStyle style)
                    : base(label, value, style) { }
    }

    [Serializable]
    public class EnumProperty : Properties<MonoScript>
    {
        public EnumProperty(string label, MonoScript enumScript, PropertyEditorStyle style)
                    : base(label, enumScript, style)
        {
        }
    }
}
