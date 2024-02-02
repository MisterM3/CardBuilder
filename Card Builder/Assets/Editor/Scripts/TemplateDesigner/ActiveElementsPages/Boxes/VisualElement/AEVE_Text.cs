using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using CardBuilder.NewStructs;
using CardBuilder.Helpers;
using UnityEditor;

namespace CardBuilder
{
    public class AEVE_Text : ActiveElementTreeViewBox
    {

        PropertyField noConnectPropertyField;
        PropertyField propertyConnectPropertyField;
        PropertyField connectedEnumValues;

        PropertyField enumValusPropertyField;

        RadioButton noConnectRadioButton;
        RadioButton propertyConnectRadioButton;

        DropdownField propertyDropdownField;

        TextField textField;

        Button scaleToSizeButton;

        TextField enumNameTF;

        #region Binding

        public override void BindItem(HierarchyData item)
        {
            base.BindItem(item);
            BindRadioButton();
            BindPropertyItem();

            BindText();

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

        private void BindText()
        {
            if (activeElement.VisualElement is UndoRedoText undoRedoTEXT)
            {
                if (activeElement.VisualElement.ConnectedInfo == null)
                {
                    textField.value = undoRedoTEXT.Text;
                    return;
                }

                if (string.IsNullOrEmpty(undoRedoTEXT.Text)) return;

                textField.value = undoRedoTEXT.Text;
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
            SetupTextInput();
            SetupButton();
            SetupPropertyInput();

            SetupEnumNameField();
            SetupEnum();
        }

        private void SetupTextInput()
        {
            textField = m_VisualElement.QLogged<TextField>("TF_BaseText");
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
            textField.RegisterValueChangedCallback(OnTextFieldChange);
            propertyDropdownField.RegisterValueChangedCallback(UpdateSelectedProperty);

            ActiveElementPanelData.OnListChanged += UpdateList;
        }


        #endregion

        #region UnRegisterEvents
        protected override void UnRegisterEvents()
        {
            noConnectRadioButton.UnregisterValueChangedCallback(OnNoConnectEvent);
            propertyConnectRadioButton.UnregisterValueChangedCallback(OnPropertyConnectEvent);
            textField.UnregisterValueChangedCallback(OnTextFieldChange);
            propertyDropdownField.UnregisterValueChangedCallback(UpdateSelectedProperty);



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

                if (activeElement.VisualElement is UndoRedoText undoRedoText)
                    textField.value = undoRedoText.Text;
            }
            else noConnectPropertyField.style.display = DisplayStyle.None;
        }

        private void OnPropertyConnectEvent(ChangeEvent<bool> evt)
        {
            if (evt.newValue)
            {
                propertyConnectPropertyField.style.display = DisplayStyle.Flex;
            }
            else
            {
                propertyConnectPropertyField.style.display = DisplayStyle.None;
                activeElement.VisualElement.ConnectedInfo = null;
            }
        }

        private void OnTextFieldChange(ChangeEvent<string> evt)
        {
            if (activeElement.VisualElement is UndoRedoText undoRedoImage)
            {
                Logs.Warning("Fired OnObjectFieldChange");
                undoRedoImage.Text = evt.newValue;
            }

            //currentSprites[0] = (Sprite)evt.newValue;
        }


        private void ChangedEnumValue(int index, ChangeEvent<string> evt)
        {
            if (activeElement.VisualElement is not UndoRedoText undoRedoText) return;
            //  if (evt.newValue is not Sprite newSpriteValue) return;

            //Done so the first index updates the visuals
            if (index == 0) undoRedoText.Text = evt.newValue;
            else undoRedoText.TextList[index] = evt.newValue;
        }


        private void UpdateList(object sender, EventArgs e)
        {
            List<PropertyInfo> propertyList = ActiveElementPanelData.GetList();

            List<string> choicesList = new();

            foreach (PropertyInfo info in propertyList)
            {
                if (info.PropertyType == PropertyType.String || info.PropertyType == PropertyType.Enum || info.PropertyType == PropertyType.Integer)
                    choicesList.Add(info.NameProperty);
            }

            propertyDropdownField.choices = choicesList;
        }


        private void UpdateSelectedProperty(ChangeEvent<string> evt)
        {
            if (activeElement.VisualElement is not UndoRedoText undoRedoText)
            {
                Logs.Error("Not supported element");
                return;
            }
            List<PropertyInfo> propertyList = ActiveElementPanelData.GetList();

            foreach (PropertyInfo info in propertyList)
            {
                if (info.NameProperty != evt.newValue) continue;

                undoRedoText.ConnectedInfo = info;

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

            if (activeElement.VisualElement is not UndoRedoText undoRedoText) return;

            //   Sprite tempSprite = undoRedoImage.Image;

            //  Sprite lastImage = undoRedoImage.Image;

            List<string> oldText = new List<string>(undoRedoText.TextList);

            //  undoRedoImage. = propertyInfo;
            undoRedoText.TextList.Clear();




            foreach (TextField child in enumValusPropertyField.Children())
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
                TextField textField = new TextField();
                enumValusPropertyField.Add(textField);
                textField.label = enumName;


                int index = enumValusPropertyField.IndexOf(textField);

                if (oldText.Count > index && oldText[index] != null)
                {
                    textField.value = oldText[index];
                    undoRedoText.TextList.Add(oldText[index]);
                }
                else undoRedoText.TextList.Add(null);




                textField.RegisterValueChangedCallback((evt) =>
                {
                    int index = enumValusPropertyField.IndexOf(textField);
                    ChangedEnumValue(index, evt);
                });


            }

        }


    }
}
