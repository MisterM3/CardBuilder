using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;
using CardBuilder.NewStructs;

namespace CardBuilder
{
    /// <summary>
    /// Main class to hold the elementPages for changing in the templateEditor
    /// </summary>
    public class ActiveElementPanel : ScriptableObject
    {
        Dictionary<PanelType, ActiveElementPage> propertyDictionary = new();
        Dictionary<PanelType, ActiveVisualElementPage> treeViewDictionary = new();

        PanelType currentType = PanelType.None;

        //Two different types of data
        PropertyInfo lastInfo;
        HierarchyData lastData;


        ActiveElementPanelData panelData;




        public void Initialize(VisualElement visualElement)
        {
            ActiveElementPage activeElementPage = CreateInstance<ActiveElementPage>();
            activeElementPage.Initialize(visualElement);
            propertyDictionary.Add(PanelType.Property, activeElementPage);

            EnumActiveElementPage enumActiveElementPage = CreateInstance<EnumActiveElementPage>();
            enumActiveElementPage.Initialize(visualElement);
            propertyDictionary.Add(PanelType.PropertyEnum, enumActiveElementPage);

            ActiveRootVisualElementPage rootVisualElementPage = CreateInstance<ActiveRootVisualElementPage>();
            rootVisualElementPage.Initialize(visualElement);
            treeViewDictionary.Add(PanelType.Root, rootVisualElementPage);

            ActiveImageVisualElementPage visualElementPage = CreateInstance<ActiveImageVisualElementPage>();
            visualElementPage.Initialize(visualElement);
            treeViewDictionary.Add(PanelType.Image, visualElementPage);

            ActiveTextVisualElementPage visualTextElementPage = CreateInstance<ActiveTextVisualElementPage>();
            visualTextElementPage.Initialize(visualElement);
            treeViewDictionary.Add(PanelType.Text, visualTextElementPage);


            panelData = CreateInstance<ActiveElementPanelData>();
            panelData.Init();
        }

        public void AddPropertyToList(object sender, PropertyInfo info)
        {
            panelData.AddProperty(info);
        }
        public void RemovePropertyFromList(object sender, PropertyInfo info)
        {
            panelData.RemoveProperty(info);
        }

        public void OnGUI()
        {
            if (currentType == PanelType.None) return;

            if (propertyDictionary.ContainsKey(currentType))
            {
                propertyDictionary[currentType].OnGUI();
            }
            else if (treeViewDictionary.ContainsKey(currentType))
            {
                treeViewDictionary[currentType].OnGUI();
            }
        }
        
        
        public void RemoveCurrentPage()
        {
            if (propertyDictionary.ContainsKey(currentType))
            {
                propertyDictionary[currentType].RemoveGUI();
            }
            else if (treeViewDictionary.ContainsKey(currentType))
            {
                treeViewDictionary[currentType].RemoveGUI();
            }
        }


        #region FocusVisualElement

        public void OnFocusData()
        {
            if (lastData == null) return;
            ChangeVisualElement(lastData);
        }

        public void OnFocusInfo()
        {
            if (lastInfo == null) return;
            ChangeProperty(lastInfo);
        }

        #endregion

        #region ChangeElement

        public void ChangeProperty(PropertyInfo info)
        {
            RemoveCurrentPage();

            if (lastInfo != null) lastInfo.OnPropertyTypeChanged -= ChangeProperty;


            if (info.PropertyType == PropertyType.Enum) currentType = PanelType.PropertyEnum;
            else currentType = PanelType.Property;

            propertyDictionary[currentType].CreateGUI();
            propertyDictionary[currentType].BindItem(info);

            lastInfo = info;

            lastInfo.OnPropertyTypeChanged += ChangeProperty;

        }

        public void ChangeProperty(object o, EventArgs evt)
        {
            ChangeProperty(lastInfo);
        }

        public void ChangeVisualElement(HierarchyData data)
        {
            RemoveCurrentPage();
            

            if (data.Name == "ROOT") currentType = PanelType.Root;
            else if (data.VisualElement.VisualElement is Label) currentType = PanelType.Text;
            else currentType = PanelType.Image;

            treeViewDictionary[currentType].CreateGUI();
            treeViewDictionary[currentType].BindItem(data);

            lastData = data;

        }

        public void ResetTab()
        {
            RemoveCurrentPage();
            lastData = null;
            lastInfo = null;
            panelData.Init();
            currentType = PanelType.None;
           // treeViewDictionary[currentType].CreateGUI();
        }

        #endregion

        #region Events

        public void ChangePropertyEvent(object sender, IEnumerable<object> obj)
        {
            var item = obj.First();

            if (item is not PropertyInfo propertyInfo)
            {
                Logs.Error("ChangedCard is not of type propertyInfo!" + name);
                return;
            }

            ChangeProperty(propertyInfo);
        }

        public void ChangeDataEvent(IEnumerable<object> obj)
        {
            var item = obj.First();

            if (item is not HierarchyData hierarchyData)
            {
                Logs.Error("ChangedCard is not of type propertyInfo!" + name);
                return;
            }

            ChangeVisualElement(hierarchyData);
        }

        #endregion
    }

}
