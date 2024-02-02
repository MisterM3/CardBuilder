using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using CardBuilder.NewStructs;
using CardBuilder.Helpers;

namespace CardBuilder
{

    using Data;
    using System;

    public class TemplateDesignerListView : ScriptableObject , ILoadSaveTemplate<SO_CardData>
    {
        [SerializeField] public static VisualTreeAsset m_listItemUXML = default;

        private ListView m_visualList;

        public static List<PropertyInfo> m_propertyInfoList = new();

        public event EventHandler<IEnumerable<object>> OnChangeElementSelection;

        public event EventHandler<PropertyInfo> OnItemAdded;

        public event EventHandler<PropertyInfo> OnItemRemoved;

        private TemplateData currentTemplate;


        public void Initialize(ListView listToAddTo)
        {
            m_visualList = listToAddTo;

            m_propertyInfoList.Clear();

            m_listItemUXML = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI/UXML/Pages/Template/PropertyInDesinger.uxml");

            SetupListView();

        }

        

        public void AddStandardItem()
        {
            PropertyInfo info = new();
            HierarchyData newData = new();
            string nameProp = "New Element";

            int i = 0;

            bool inList = m_propertyInfoList.Exists((x) => (x.NameProperty == nameProp));

            while (m_propertyInfoList.Exists((x) => (x.NameProperty == nameProp)))
            {
                i++;
                nameProp = $"New Element ({i})";
            }

            info.NameProperty = nameProp;

            info.PropertyType = (PropertyType.Integer);
            info.OnValueChanged += Info_onValueChanged;
            AddItem(info);
            m_visualList.Rebuild();

        }

        public void AddItem(PropertyInfo info)
        {
            info.OnValueChanged += Info_onValueChanged;
            OnItemAdded.Invoke(this, info);
            m_propertyInfoList.Add(info);
            m_visualList.Rebuild();
        }

        public void RemoveAllItems()
        {
            m_propertyInfoList.Clear();
        }


        #region ListView

        private void SetupListView()
        {
            m_visualList.itemsSource = m_propertyInfoList;

            m_visualList.makeItem = () => m_listItemUXML.CloneTree();

            m_visualList.bindItem = (element, index) => BindItem(element, index);

            m_visualList.selectionChanged += List_selectionChanged;

            m_visualList.fixedItemHeight = 40;
        }

        private void BindItem(VisualElement element, int index)
        {
            string prefix = m_propertyInfoList[index].PropertyType.ToString();


            switch (m_propertyInfoList[index].PropertyType)
            {
                case PropertyType.Integer:
                    break;
                case PropertyType.String:
                    break;
                case PropertyType.Sprite:
                    break;
                case PropertyType.Enum:
                    break;
            }

            Label label = element.QLogged<Label>("Label");

            label.text = $"({prefix}) {m_propertyInfoList[index].NameProperty}";

            label.style.backgroundColor = m_propertyInfoList[index].BackgroundColour;

            label.style.color = m_propertyInfoList[index].TextColour;
        }

        /// <summary>
        /// Checks if everything is correct for saving
        /// </summary>
        /// <returns></returns>
        public bool CanSave()
        {
            foreach(PropertyInfo info in m_propertyInfoList)
            {
                if (info.PropertyType != PropertyType.Enum) continue;
                //If the enumProperty has a script attached it is working correctly
                if (info.EnumScript != null) continue;

                EditorUtility.DisplayDialog("Can't Save", $"The enum property called {info.NameProperty} has no enum attached, add a enum to the property!", "OK");
                return false;
            }


            return true;
        }

        public void RemoveSelectedItem()
        {

            if (!EditorUtility.DisplayDialog("Remove Property", "Are you sure you want to remove the selected Property?", "Yes", "No")) return;


            OnItemRemoved.Invoke(this, (PropertyInfo)m_visualList.selectedItem);
            m_propertyInfoList.Remove((PropertyInfo)m_visualList.selectedItem);
            m_visualList.Rebuild();
        }

        #endregion


        #region Events
        private void Info_onValueChanged(object sender, EventArgs e)
        {
            m_visualList.Rebuild();

            if (sender is not PropertyInfo info) return;
            
        }

        private void List_selectionChanged(IEnumerable<object> obj)
        {
            OnChangeElementSelection?.Invoke(this, obj);
        }

        #endregion


        #region Saving

        public void LoadTemplate(TemplateData templateDataToLoad)
        {
            currentTemplate = templateDataToLoad;

            LoadData(currentTemplate.cardDataSO);
        }

        public SO_CardData SaveTemplate()
        {
            return SaveData(currentTemplate.cardDataSO);
        }


        public void LoadData(SO_CardData fileToLoadFrom)
        {
            List<IntProperty> intProp = fileToLoadFrom.IntPropertyList;
            List<StringProperty> stringProp = fileToLoadFrom.StringPropertyList;
            List<SpriteProperty> spriteProp = fileToLoadFrom.SpritePropertyList;
            List<EnumProperty> enumProp = fileToLoadFrom.EnumPropertyList;

            foreach(IntProperty property in intProp)
            {
                PropertyInfo info = new();
                info.NameProperty = property.PropertyLabel;
                info.PropertyType = PropertyType.Integer;
                info.BackgroundColour = property.Style.backgroundColour;
                info.TextColour = property.Style.textColour;

                AddItem(info);
            }

            foreach (StringProperty property in stringProp)
            {
                PropertyInfo info = new();
                info.NameProperty = property.PropertyLabel;
                info.PropertyType = PropertyType.String;
                info.BackgroundColour = property.Style.backgroundColour;
                info.TextColour = property.Style.textColour;

                AddItem(info);
            }

            foreach (SpriteProperty property in spriteProp)
            {
                PropertyInfo info = new();
                info.NameProperty = property.PropertyLabel;
                info.PropertyType = PropertyType.Sprite;
                info.BackgroundColour = property.Style.backgroundColour;
                info.TextColour = property.Style.textColour;

                AddItem(info);
            }

            foreach (EnumProperty property in enumProp)
            {
                PropertyInfo info = new();
                info.NameProperty = property.PropertyLabel;
                info.PropertyType = PropertyType.Enum;
                info.BackgroundColour = property.Style.backgroundColour;
                info.TextColour = property.Style.textColour;
                info.EnumScript = property.Value;

                AddItem(info);
            }
        }


        public SO_CardData SaveData(SO_CardData fileToSaveTo)
        {
            if (fileToSaveTo == null)
            {
                Logs.Error("NO FILE TO SAVE TO; NAME INCORRECT");
            }

            fileToSaveTo.ClearAllLists();
            List<IntProperty> intProp = fileToSaveTo.IntPropertyList;
            List<StringProperty> stringProp = fileToSaveTo.StringPropertyList;
            List<SpriteProperty> spriteProp = fileToSaveTo.SpritePropertyList;
            List<EnumProperty> enumProp = fileToSaveTo.EnumPropertyList;


            foreach (PropertyInfo propertyInfo in m_propertyInfoList)
            {

                PropertyEditorStyle style = new PropertyEditorStyle(propertyInfo.BackgroundColour, propertyInfo.TextColour);

                switch (propertyInfo.PropertyType)
                {

                    case PropertyType.Integer:
                        IntProperty intProperty = new(propertyInfo.NameProperty, 0, style);
                        intProp.Add(intProperty);
                        break;
                    case PropertyType.String:
                        StringProperty stringproperty = new(propertyInfo.NameProperty, "", style);
                        stringProp.Add(stringproperty);
                        break;
                    case PropertyType.Sprite:
                        SpriteProperty spriteProperty = new(propertyInfo.NameProperty, null, style);
                        spriteProp.Add(spriteProperty);
                        break;
                    case PropertyType.Enum:
                        EnumProperty enumProperty = new(propertyInfo.NameProperty, propertyInfo.EnumScript, style);
                        enumProp.Add(enumProperty);
                        break;
                }
            }

            EditorUtility.SetDirty(fileToSaveTo);

            AssetDatabase.SaveAssets();



            return fileToSaveTo;


        }

        //Add warning for overrides
        public SO_CardData SaveData(string nameFile)
        {
           SO_CardData data = AssetDatabase.LoadAssetAtPath<SO_CardData>($"Assets/Editor/SavedScriptableObjects/CardData/{nameFile}.asset");

            if (data == null)
            {
                data = CreateInstance<SO_CardData>();
                AssetDatabase.CreateAsset(data, $"Assets/Editor/SavedScriptableObjects/CardData/{nameFile}.asset");
            }

            data.ClearAllLists();
            List<IntProperty> intProp = data.IntPropertyList;
            List<StringProperty> stringProp = data.StringPropertyList;
            List<SpriteProperty> spriteProp = data.SpritePropertyList;
            List<EnumProperty> enumProp = data.EnumPropertyList;


            foreach (PropertyInfo propertyInfo in m_propertyInfoList)
            {

                PropertyEditorStyle style = new PropertyEditorStyle(propertyInfo.BackgroundColour, propertyInfo.TextColour);

                switch(propertyInfo.PropertyType)
                {
                    
                    case PropertyType.Integer:
                        IntProperty intProperty = new(propertyInfo.NameProperty, 0, style);
                        intProp.Add(intProperty);
                        break;
                    case PropertyType.String:
                        StringProperty stringproperty = new(propertyInfo.NameProperty, "", style);
                        stringProp.Add(stringproperty);
                        break;
                    case PropertyType.Sprite:
                        SpriteProperty spriteProperty = new(propertyInfo.NameProperty, null, style);
                        spriteProp.Add(spriteProperty);
                        break;
                    case PropertyType.Enum:
                        EnumProperty enumProperty = new(propertyInfo.NameProperty, propertyInfo.EnumScript, style);
                        enumProp.Add(enumProperty);
                        break;
                }
            }

            AssetDatabase.SaveAssets();

            return data;


        }


        #endregion


    }

}
