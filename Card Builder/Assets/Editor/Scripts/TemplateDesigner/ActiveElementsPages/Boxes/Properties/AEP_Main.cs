using System;
using UnityEngine;
using UnityEngine.UIElements;
using CardBuilder.NewStructs;
using CardBuilder.Helpers;
using UnityEditor;

namespace CardBuilder
{
    public class AEP_Main : ActiveElementPropertyBox
    {

        private TextField textField;
        private EnumField enumField;
        


        public override void BindItem(PropertyInfo item)
        {
            base.BindItem(item);

            textField.value = item.NameProperty;
            enumField.value = activeElement.PropertyType;

      
        }



        #region Setup
        public override void SetupUI()
        {
            SetupText();
            SetupPropertyTypeField();
        }

        private void SetupText()
        {
            textField = m_VisualElement.QLogged<TextField>("Name");
        }
        private void SetupPropertyTypeField()
        {
            enumField = m_VisualElement.QLogged<EnumField>("PropertyTypeField");
        }

        #endregion

        #region RegisterEvents

        protected override void RegisterEvents()
        {
            RegisterText();
            RegisterEnum();
        }

        private void RegisterText()
        {
            textField.RegisterCallback<KeyDownEvent>(OnEnterTextField);
        }

        private void RegisterEnum()
        {
            enumField.RegisterValueChangedCallback(ChangePropertyType);
        }

        #endregion

        #region UnregisterEvents

        protected override void UnRegisterEvents()
        {
            UnRegisterText();
            UnRegisterEnum();
        }

        private void UnRegisterText()
        {
            textField.UnregisterCallback<KeyDownEvent>(OnEnterTextField);
        }

        private void UnRegisterEnum()
        {
            enumField.UnregisterValueChangedCallback(ChangePropertyType);
        }

        #endregion;


       

        #region Events

        private void OnEnterTextField(KeyDownEvent evt)
        {
            if (activeElement == null)
            {
                Logs.NoActiveElementError();
                return;
            }

            bool hasPressedEnter = (evt.keyCode == KeyCode.Return || evt.keyCode == KeyCode.KeypadEnter);

            if (!hasPressedEnter) return;

            if (string.IsNullOrEmpty(textField.value) || string.IsNullOrWhiteSpace(textField.value))
            {

                EditorUtility.DisplayDialog("Empty name!", "The name of a property can't be empty!", "OK");
                textField.value = activeElement.NameProperty;
                return;
            }

            foreach (PropertyInfo info in TemplateDesignerListView.m_propertyInfoList)
            {
                if (info.NameProperty != textField.value) continue;

                EditorUtility.DisplayDialog("Duplicate Name", "This name is already being used by another property! You can't have duplicate names!", "OK");
                textField.value = activeElement.NameProperty;
                return;
            }

            HierarchyTreeView.Instance.SwitchProperty(activeElement.NameProperty, textField.value);
            
            activeElement.NameProperty = textField.value;



        }


        private void ChangePropertyType(ChangeEvent<Enum> evt)
        {
            if (activeElement == null)
            {
                Logs.NoActiveElementError();
                return;
            }
            
            if (evt.newValue is not PropertyType newPropertyType)
            {
                Logs.Error("Enum is not propertytype!" + name);
                return;
            }

            activeElement.PropertyType = newPropertyType;
        }

        #endregion;

    }
}
