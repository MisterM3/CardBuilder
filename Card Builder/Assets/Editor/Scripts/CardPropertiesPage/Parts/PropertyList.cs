using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CardBuilder.Data;
using UnityEditor;
using UnityEditor.UIElements;
using System;
using CardBuilder.Helpers;
using CardBuilder.NewStructs;

namespace CardBuilder
{

    public class ChangedValue : EventArgs
    {
        public string valueName;
        public string newValue;
    }

    public class PropertyList : ScriptableObject
    {


        private ListView m_visualList;
        private SO_CardData m_cardData;

        public Card m_Card;

        private bool hasUnsavedChanges;
        public bool HasUnsavedChanges
        {
            get => hasUnsavedChanges;
            set => hasUnsavedChanges = value;
        }


        private static List<PropertyShow> m_ElementBase = new();

        [SerializeField]
        private VisualTreeAsset m_propertyVisual = default;

        public event EventHandler<ChangedValue> onValueChangedEvent;

       // public static VisualTreeAsset m_propertyVisual;




        public void Initialize(ListView listToAddTo)
        {
            m_visualList = listToAddTo;

            m_visualList.Clear();

            m_visualList.itemsSource = m_ElementBase;
       //     m_propertyVisual = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI/UXML/Property/Property.uxml");

            m_visualList.fixedItemHeight = 40;

            m_visualList.makeItem = () => m_propertyVisual.CloneTree();


            m_visualList.bindItem = (e, i) => BindItemToList(e, i);

        }

        #region Binding

        public void Remove()
        {
         //   m_visualList.Clear();
            m_ElementBase.Clear();
            m_visualList.Rebuild();

        }
        private void BindItemToList(VisualElement element, int i)
        {

            PropertyShow propertyShow = m_ElementBase[i];

            PropertyType propertyType = propertyShow.type;

            int index = m_ElementBase[i].indexOfList;


            VisualElement visElement = null;

            switch(propertyType)
            {
                case PropertyType.Integer:
                    visElement = BindInt(element, index, propertyShow);

                    break;
                case PropertyType.String:
                    visElement = BindString(element, index, propertyShow);

                    break;
                case PropertyType.Sprite:
                    visElement = BindSprite(element, index, propertyShow);

                    break;
                case PropertyType.Enum:
                    visElement = BindEnum(element, index, propertyShow);

                    break;
            }

            propertyShow.connectedVisualElement = visElement;

            m_ElementBase[i] = propertyShow;


            if (m_Card != null)
                SetValueVisual(propertyShow);

        }

        private void SetValueVisual(PropertyShow propertyShow)
        {
            Logs.Info("________");

            foreach (System.Reflection.FieldInfo info in m_Card.GetType().GetFields())
            {


                string propertyToLine = StreamWriterMethods.ConvertPropertyToLine(propertyShow.label);

                if (info.Name != propertyToLine) continue;


                switch (propertyShow.connectedVisualElement)
                {
                    case IntegerField:
                        IntegerField i = (IntegerField)propertyShow.connectedVisualElement;
                        i.value = (int)info.GetValue(m_Card);

                        break;
                    case TextField:
                        TextField t = (TextField)propertyShow.connectedVisualElement;
                        t.value = (string)info.GetValue(m_Card);
                        break;
                    case ObjectField:
                        ObjectField o = (ObjectField)propertyShow.connectedVisualElement;
                        o.value = (Sprite)info.GetValue(m_Card);
                        break;
                    case DropdownField:
                        DropdownField d = (DropdownField)propertyShow.connectedVisualElement;
                        d.value = info.GetValue(m_Card).ToString();
                        break;
                }
               
            }
        }

        internal void ChangeSave(SavedCardDataEditor savedCard)
        {
            Debug.Log(savedCard.name);

            DifCard(savedCard.card);
            DifferentCard(savedCard.cardDataSO);
        }


        //Rewrite it into generic maybe?
        private VisualElement BindInt(VisualElement element, int index, PropertyShow propertyShow)
        {
            VisualElement changeElement = element.QLogged<IntegerField>("IF_IntegerProperty");
            IntegerField intField = changeElement as IntegerField;

            changeElement.AddToClassList("enabled");
            //   intField.RegisterValueChangedCallback((evt) => SavePropertiesToCard());


            intField.label = propertyShow.label;
        //    intField.bindingPath = $"m_intPropertyList.Array.data[{index}].value";

            changeElement.visible = true;

            changeElement.style.backgroundColor = propertyShow.style.backgroundColour;

            // VisualElement style = element.QLogged<VisualElement>("Style");
            ///  style.style.backgroundColor = propertyShow.style.backgroundColour;
            //   style.style.color = propertyShow.style.textColour;

            intField.RegisterValueChangedCallback((evt) => {
                HasUnsavedChanges = true;
                onValueChangedEvent?.Invoke(this, new ChangedValue { valueName = intField.label, newValue = evt.newValue.ToString() });
                });
            return intField;
        }
        private VisualElement BindString(VisualElement element, int index, PropertyShow propertyShow)
        {
            VisualElement changeElement = element.QLogged<TextField>("TF_StringProperty");

            changeElement.AddToClassList("enabled");

            TextField textField = changeElement as TextField;
            textField.label = propertyShow.label;
            //  textField.bindingPath = $"m_stringPropertyList.Array.data[{index}].value";

            // textField.RegisterValueChangedCallback((evt) => SavePropertiesToCard());



            changeElement.visible = true;

            changeElement.style.backgroundColor = propertyShow.style.backgroundColour;

        //    VisualElement style = element.QLogged<VisualElement>("Style");
        //    style.style.backgroundColor = propertyShow.style.backgroundColour;
        //    style.style.color = propertyShow.style.textColour;

            textField.RegisterValueChangedCallback((evt) =>
            { 
                HasUnsavedChanges = true;
                if (evt.newValue == null) return;
                onValueChangedEvent?.Invoke(this, new ChangedValue { valueName = textField.label, newValue = evt.newValue.ToString() });
            });


            return textField;
        }

        private VisualElement BindSprite(VisualElement element, int index, PropertyShow propertyShow)
        {
            VisualElement changeElement = element.QLogged<ObjectField>("OF_SpriteProperty");

            changeElement.AddToClassList("enabled");

            ObjectField spriteField = changeElement as ObjectField;
            spriteField.label = propertyShow.label;
            spriteField.objectType = Sprite.Create(Texture2D.whiteTexture, Rect.zero, Vector2.zero).GetType();
            spriteField.bindingPath = $"m_spritePropertyList.Array.data[{index}].value";

        //    spriteField.RegisterValueChangedCallback((evt) => SavePropertiesToCard());


            //changeElement.visible = true;
            changeElement.style.backgroundColor = propertyShow.style.backgroundColour;

            spriteField.RegisterValueChangedCallback((evt) =>
            {
                HasUnsavedChanges = true;
                onValueChangedEvent?.Invoke(this, new ChangedValue { valueName = spriteField.label, newValue = Helpers.IOMethods.GetGUIDFromObject(evt.newValue) });
            });
            return spriteField;
        }

        private VisualElement BindEnum(VisualElement element, int index, PropertyShow propertyShow)
        {
            VisualElement changeElement = element.QLogged<DropdownField>("DF_EnumProperty");

            changeElement.AddToClassList("enabled");

            DropdownField enumField = changeElement as DropdownField;
               enumField.label = propertyShow.label;

        //    enumField.RegisterValueChangedCallback((evt) => SavePropertiesToCard());


            // Get the asset path of the MonoScript
            string assetPath = AssetDatabase.GetAssetPath(m_cardData.EnumPropertyList[index].Value);

            // Extract the file name from the asset path
            string fileName = System.IO.Path.GetFileName(assetPath);

            int csIndex = fileName.LastIndexOf(".cs");

            string nameType = fileName.Remove(csIndex, 3);

            Type typeMonoScript = Type.GetType($"{nameType}, Assembly-CSharp");

            enumField.choices = new List<string>(typeMonoScript.GetEnumNames());
         //   enumField.bindingPath = $"m_enumPropertyList.Array.data[{index}].value";

        //    changeElement.visible = true;

            changeElement.style.backgroundColor = propertyShow.style.backgroundColour;

           // VisualElement style = element.QLogged<VisualElement>("Style");
           // style.style.backgroundColor = propertyShow.style.backgroundColour;
           // style.style.color = propertyShow.style.textColour;

            enumField.RegisterValueChangedCallback((evt) =>
            {
                HasUnsavedChanges = true;
                onValueChangedEvent?.Invoke(this, new ChangedValue { valueName = enumField.label, newValue = evt.newValue.ToString() });
            });

            return enumField;
        }


        private void SetupBindCard(VisualElement element, PropertyShow propertyShow)
        {
       //     element.label = propertyShow.label;
        }

        public void DifCard(Card card)
        {
            m_Card = card;
        }




        #endregion

        public void DifferentCard(SO_CardData cardData)
        {
            m_cardData = cardData;

          //  Debug.Log(m_cardData.StringPropertyList[0].Value);

            GenerateList();

       //     SerializedObject so = new(m_cardData);

        //    m_visualList.Bind(so);

        //    m_visualList.Unbind();
        }

        public void DifferentCard(ChangeEvent<UnityEngine.Object> evt)
        {
            if (!(evt.newValue is SO_CardData cardData))
            {
                Logs.Error("Put in card is not of cardData" + name);
                return;
            }

            DifferentCard(cardData);
        }


        


        public void SavePropertiesToCard()
        {


            Type cardType = m_Card.GetType();


            foreach (System.Reflection.FieldInfo info in m_Card.GetType().GetFields())
            {

                foreach(PropertyShow propertyShow  in m_ElementBase)
                {
                    string propertyToLine = StreamWriterMethods.ConvertPropertyToLine(propertyShow.label);

                    if (info.Name != propertyToLine) continue;

                    switch (propertyShow.connectedVisualElement)
                    {
                        case IntegerField:
                            IntegerField i = (IntegerField)propertyShow.connectedVisualElement;
                            info.SetValue(m_Card, i.value);

                            break;
                        case TextField:
                            TextField t = (TextField)propertyShow.connectedVisualElement;
                            info.SetValue(m_Card, t.value);
                            break;
                        case ObjectField:
                            ObjectField o = (ObjectField)propertyShow.connectedVisualElement;
                            info.SetValue(m_Card, o.value);
                            break;
                        case DropdownField:
                            DropdownField d = (DropdownField)propertyShow.connectedVisualElement;
                            SaveEnumValue(info, d.value);
                            break;
                    }
                }


            }

            EditorUtility.SetDirty(m_Card);

            AssetDatabase.SaveAssetIfDirty(m_Card);

            CardPropertyPage.OnCardUpdated?.Invoke(this, EventArgs.Empty);
            CardPropertyPage.OnCardUpdated?.Invoke(this, EventArgs.Empty);

        }


        public void SaveEnumValue(System.Reflection.FieldInfo info, string value)
        {

            Type enumType = info.GetValue(m_Card).GetType();

            if (Enum.IsDefined(enumType, value))
            {
                Enum enumValue = (Enum)Enum.Parse(enumType, value);

                // Set the value of the property using SetValue
                info.SetValue(m_Card, enumValue);
            }
            else Logs.Warning("Invalid Enum Value");

        }


        public void SaveProperties()
        {
            HasUnsavedChanges = false;
            SavePropertiesToCard();
        }




        private void GenerateList()
        {
            m_ElementBase.Clear();



            List<IntProperty> intPropertyList = m_cardData.IntPropertyList;

            for (int i = 0; i < intPropertyList.Count; i++)
            {
                IntProperty property = intPropertyList[i];


                PropertyShow propertyShow = new(property.PropertyLabel, CardBuilder.PropertyType.Integer, property.Style, i, null);

                m_ElementBase.Add(propertyShow);

            }

            List<StringProperty> stringPropertyList = m_cardData.StringPropertyList;

            for (int i = 0; i < stringPropertyList.Count; i++)
            {
                StringProperty property = stringPropertyList[i];


                PropertyShow propertyShow = new(property.PropertyLabel, PropertyType.String, property.Style, i, null);

                m_ElementBase.Add(propertyShow);

            }

            List<SpriteProperty> spritePropertyList = m_cardData.SpritePropertyList;

            for (int i = 0; i < spritePropertyList.Count; i++)
            {
                SpriteProperty property = spritePropertyList[i];

                PropertyShow propertyShow = new(property.PropertyLabel, PropertyType.Sprite, property.Style, i, null);

                m_ElementBase.Add(propertyShow);
            }

            List<EnumProperty> enumPropertyList = m_cardData.EnumPropertyList;

            for (int i = 0; i < enumPropertyList.Count; i++)
            {
                EnumProperty property = enumPropertyList[i];

                PropertyShow propertyShow = new(property.PropertyLabel, PropertyType.Enum, property.Style, i, null);

                m_ElementBase.Add(propertyShow);
            }


            m_visualList.Rebuild();
        }

        


    }



   
}
