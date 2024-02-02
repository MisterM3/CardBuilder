namespace CardBuilder
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEditor.UIElements;
    using UnityEngine;
    using UnityEngine.UIElements;
    using CardBuilder.NewStructs;
    using CardBuilder.Helpers;

    public class AEVE_Image : ActiveElementTreeViewBox
    {

        List<Sprite> currentSprites;

        PropertyField noConnectPropertyField;
        PropertyField propertyConnectPropertyField;
        PropertyField connectedEnumValues;

        PropertyField enumValusPropertyField;

        RadioButton noConnectRadioButton;
        RadioButton propertyConnectRadioButton;

        DropdownField propertyDropdownField;

        ObjectField spriteObjectField;

        Button scaleToSizeButton;

        TextField enumNameTF;

        #region Binding

        public override void BindItem(HierarchyData item)
        {
            base.BindItem(item);
            BindRadioButton();
            BindPropertyItem();

            BindImage();

            UpdateList(null, EventArgs.Empty);

        }

        private void BindRadioButton()
        {


            if (activeElement.VisualElement.ConnectedInfo != null && activeElement.VisualElement.ConnectedInfo.NameProperty != "")
            {
                noConnectRadioButton.value = false;
                noConnectPropertyField.style.display = DisplayStyle.None;
                propertyConnectRadioButton.value = true;
                propertyConnectPropertyField.style.display = DisplayStyle.Flex;

                if (activeElement.VisualElement.ConnectedInfo.PropertyType == PropertyType.Enum)
                {
                    connectedEnumValues.style.display = DisplayStyle.Flex;
                }
                else
                {
                    connectedEnumValues.style.display = DisplayStyle.None;
                }
            }
            else
            {
                noConnectRadioButton.value = true;
                noConnectPropertyField.style.display = DisplayStyle.Flex;
                propertyConnectRadioButton.value = false;
                propertyConnectPropertyField.style.display = DisplayStyle.None;
                connectedEnumValues.style.display = DisplayStyle.None;
            }
        }

        private void BindPropertyItem()
        {

            if (activeElement.VisualElement.ConnectedInfo != null &&
                activeElement.VisualElement.ConnectedInfo.NameProperty != "")
            {
                propertyDropdownField.value = activeElement.VisualElement.ConnectedInfo.NameProperty;
            }
            else
            {
                propertyDropdownField.value = "No Selected Property";
            }
        }

        private void BindImage()
        {
            if (activeElement.VisualElement is UndoRedoImage undoRedoImage)
            {
                if (activeElement.VisualElement.ConnectedInfo == null)
                {
                    spriteObjectField.value = undoRedoImage.Image;
                    return;
                }

                if (undoRedoImage.Image == null) return;

                if (undoRedoImage.SpriteList.Count <= 0) return;

            }
        }

        #endregion

        #region Setup

        public override void SetupUI()
        {
            SetupFields();
            SetupRadioButtons();
        }

        private void SetupFields()
        {
            SetupImageInput();
            SetupButton();
            SetupPropertyInput();

            SetupEnumNameField();
            SetupEnum();
        }

        private void SetupImageInput()
        {
            spriteObjectField = m_VisualElement.QLogged<ObjectField>("SpriteInputField");
            currentSprites = new(1);
        }

        private void SetupButton()
        {
            scaleToSizeButton = m_VisualElement.QLogged<Button>("ScaleToSize");
        }

        private void SetupRadioButtons()
        {
            noConnectRadioButton = m_VisualElement.QLogged<RadioButton>("NoConnect");
            noConnectPropertyField = m_VisualElement.QLogged<PropertyField>("PF_NoConnect");

            propertyConnectRadioButton = m_VisualElement.QLogged<RadioButton>("ConnectProperties");
            propertyConnectPropertyField = m_VisualElement.QLogged<PropertyField>("PF_ConnectProperty");
        }

        private void SetupPropertyInput()
        {
            propertyDropdownField = m_VisualElement.QLogged<DropdownField>("PropertyDropDown");
            propertyDropdownField.value = "No Connected Property";
        }

        private void SetupEnum()
        {
            connectedEnumValues = m_VisualElement.QLogged<PropertyField>("PF_EnumConnected");
            enumValusPropertyField = m_VisualElement.QLogged<PropertyField>("Enums");
        }
        private void SetupEnumNameField()
        {
            enumNameTF = m_VisualElement.QLogged<TextField>("TF_EnumName");
        }

        #endregion

        #region RegisterEvents
        protected override void RegisterEvents()
        {
            noConnectRadioButton.RegisterValueChangedCallback(OnNoConnectEvent);
            propertyConnectRadioButton.RegisterValueChangedCallback(OnPropertyConnectEvent);
            spriteObjectField.RegisterValueChangedCallback(OnObjectFieldChange);
            propertyDropdownField.RegisterValueChangedCallback(UpdateSelectedProperty);

            scaleToSizeButton.clicked += ScaleToSizeButtonEvent;
            ActiveElementPanelData.OnListChanged += UpdateList;
        }


        #endregion

        #region UnRegisterEvents
        protected override void UnRegisterEvents()
        {
            noConnectRadioButton.UnregisterValueChangedCallback(OnNoConnectEvent);
            propertyConnectRadioButton.UnregisterValueChangedCallback(OnPropertyConnectEvent);
            spriteObjectField.UnregisterValueChangedCallback(OnObjectFieldChange);
            propertyDropdownField.UnregisterValueChangedCallback(UpdateSelectedProperty);

            scaleToSizeButton.clicked -= ScaleToSizeButtonEvent;

            //Maybe change so it's an instance
            ActiveElementPanelData.OnListChanged -= UpdateList;
        }

        #endregion

        #region Events

        private void OnNoConnectEvent(ChangeEvent<bool> evt)
        {
            if (evt.newValue)
            {
                noConnectPropertyField.style.display = DisplayStyle.Flex;
                activeElement.VisualElement.ConnectedInfo = null;
                propertyConnectPropertyField.style.display = DisplayStyle.None;
                connectedEnumValues.style.display = DisplayStyle.None;

                if (activeElement.VisualElement is UndoRedoImage undoRedoImage)
                    spriteObjectField.value = undoRedoImage.Image;
            }
            else noConnectPropertyField.style.display = DisplayStyle.None;
        }

        private void OnPropertyConnectEvent(ChangeEvent<bool> evt)
        {
            if (evt.newValue)
            {
                propertyConnectPropertyField.style.display = DisplayStyle.Flex;
                propertyDropdownField.value = "No Selected Property";
            }
            else
            {
                propertyConnectPropertyField.style.display = DisplayStyle.None;
                activeElement.VisualElement.ConnectedInfo = null;
            }
        }

        private void OnObjectFieldChange(ChangeEvent<UnityEngine.Object> evt)
        {
            if (activeElement.VisualElement is UndoRedoImage undoRedoImage)
            {
                Logs.Warning("Fired OnObjectFieldChange");
                undoRedoImage.SpriteList = new(1);
                undoRedoImage.Image = (Sprite)evt.newValue;
            }

            //currentSprites[0] = (Sprite)evt.newValue;
        }

        private void ScaleToSizeButtonEvent()
        {
            if (activeElement.VisualElement is not UndoRedoImage undoImage || undoImage.Image == null)
            {
                EditorUtility.DisplayDialog("No Current Sprite", "Add a Sprite to be able to scale to size", "OK");
                return;
            }

            activeElement.Size = undoImage.Image.rect.size / 10;
            activeElement.Position = activeElement.Position;
        }

        private void ChangedEnumValue(int index, ChangeEvent<UnityEngine.Object> evt)
        {
            if (activeElement.VisualElement is not UndoRedoImage undoRedoImage) return;
            if (evt.newValue is not Sprite newSpriteValue) return;

            //Done so first one shows up as image
            if (index == 0) undoRedoImage.Image = newSpriteValue;
            else undoRedoImage.SpriteList[index] = newSpriteValue;
        }


        private void UpdateList(object sender, EventArgs e)
        {
            List<PropertyInfo> propertyList = ActiveElementPanelData.GetList();

            List<string> choicesList = new();

            foreach (PropertyInfo info in propertyList)
            {
                if (info.PropertyType == PropertyType.Sprite || info.PropertyType == PropertyType.Enum)
                    choicesList.Add(info.NameProperty);
            }

            propertyDropdownField.choices = choicesList;
        }


        private void UpdateSelectedProperty(ChangeEvent<string> evt)
        {
            if (activeElement.VisualElement is not UndoRedoImage undoRedoImage)
            {
                Logs.Error("Not supported element");
                return;
            }
            List<PropertyInfo> propertyList = ActiveElementPanelData.GetList();

            foreach (PropertyInfo info in propertyList)
            {
                if (info.NameProperty != evt.newValue) continue;
                
                undoRedoImage.ConnectedInfo = info;

                if (info.PropertyType == PropertyType.Enum)
                {
                    if (info.EnumScript == null) EditorUtility.DisplayDialog("Enum Property Doesn't have enum attached",
                                                                             "The property can't be attached as it has not been assigned an enum script yet",
                                                                             "OK");
                        
                    AddEnum(info.EnumScript);

                    connectedEnumValues.style.display = DisplayStyle.Flex;
                 //   noConnectPropertyField.style.display = DisplayStyle.None;
                }
                else 
                {
                    connectedEnumValues.style.display = DisplayStyle.None;
                   // noConnectPropertyField.style.display = DisplayStyle.Flex;
                }

                break;
            }

            propertyDropdownField.value = evt.newValue;

        }


        #endregion



        private void AddEnum(MonoScript propertyInfo)
        {

            Type typeEnum = IOMethods.GetTypeOfObjectUsingObject(propertyInfo);

            enumNameTF.value = typeEnum.Name;

            if (activeElement.VisualElement is not UndoRedoImage undoRedoImage) return;


            List<Sprite> oldSprites = new List<Sprite>(undoRedoImage.SpriteList);

            undoRedoImage.SpriteList.Clear();

            


            foreach (ObjectField child in enumValusPropertyField.Children())
            {

                child.UnregisterValueChangedCallback((evt) =>
                {
                    int index = enumValusPropertyField.IndexOf(child);
                    ChangedEnumValue(index, evt);
                });
            }

            enumValusPropertyField.Clear();

            

            foreach (var enumName in typeEnum.GetEnumNames())
            {
                ObjectField objectField = new ObjectField();
                enumValusPropertyField.Add(objectField);
                objectField.objectType = typeof(Sprite);
                objectField.label = enumName;


                int index = enumValusPropertyField.IndexOf(objectField);

                if (oldSprites.Count > index && oldSprites[index] != null)
                {
                    objectField.value = oldSprites[index];
                    undoRedoImage.SpriteList.Add(oldSprites[index]);
                } else undoRedoImage.SpriteList.Add(null);




                objectField.RegisterValueChangedCallback( (evt) =>
                {
                    int index = enumValusPropertyField.IndexOf(objectField);
                    ChangedEnumValue(index, evt);
                });


            }

        }

    }
}

