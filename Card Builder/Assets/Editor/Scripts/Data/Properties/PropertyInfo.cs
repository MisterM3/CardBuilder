namespace CardBuilder
{
    using UnityEngine;
    using System;
    using UnityEditor;
    using CardBuilder.NewStructs;

    //Used for active element in template designer
    [Serializable]
    public class PropertyInfo
    {

        #region Properties

        #region Main

        [SerializeField] private string nameProperty = "";
        public string NameProperty
        {
            get { return nameProperty; }

            set
            {
                nameProperty = value;
                onValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }


        [SerializeField] private PropertyType propertyType;
        public PropertyType PropertyType
        {
            get { return propertyType; }

            set
            {
                propertyType = value;
                onValueChanged?.Invoke(this, EventArgs.Empty);
                OnPropertyTypeChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        #endregion Main

        #region Style

        [SerializeField] private Color backgroundColour;
        public Color BackgroundColour
        {
            get { return backgroundColour; }

            set
            {
                backgroundColour = value;
                onValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        [SerializeField] private Color textColour = Color.white;
        public Color TextColour
        {
            get { return textColour; }

            set
            {
                textColour = value;
                onValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        #endregion Style

        #region Enums

        [SerializeField] private MonoScript enumScript;

        public MonoScript EnumScript
        {
            get { return enumScript; }
            set { enumScript = value; }
        }


        #endregion

        #region Events
        public EventHandler onValueChanged;
        public EventHandler OnValueChanged
        {
            get { return onValueChanged; }
            set { onValueChanged = value; }
        }


        private EventHandler onPropertyTypeChanged;

        public EventHandler OnPropertyTypeChanged
        {
            get { return onPropertyTypeChanged; }
            set { onPropertyTypeChanged = value; }
        }

        #endregion Events

        #endregion Properties

        #region Constructors

        public PropertyInfo(string name, PropertyType type)
        {
            nameProperty = name;
            this.propertyType = type;
            textColour = Color.white;
        }

        public PropertyInfo()
        {
            textColour = Color.white;
        }

        #endregion


        public PropertyInfo Copy()
        {
            return new PropertyInfo()
            {
                nameProperty = this.nameProperty,
                propertyType = this.propertyType,
                backgroundColour = this.backgroundColour,
                textColour = this.textColour,
                enumScript = this.enumScript,
            };
        }

    }
}
