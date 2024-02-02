using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using CardBuilder.Helpers;
using UnityEditor;

namespace CardBuilder
{
    public class AEVE_Main : ActiveElementTreeViewBox
    {

        TextField nameField;

        public override void BindItem(HierarchyData item)
        {
            base.BindItem(item);

            activeElement = item;

            nameField.value = activeElement.Name;
        }

        #region Setup
        public override void SetupUI()
        {
            SetupFields();
        }

        private void SetupFields()
        {
            SetupText();
            SetupPropertyTypeField();
        }

        private void SetupText()
        {
            nameField = m_VisualElement.QLogged<TextField>("Name");
        }

        private void SetupPropertyTypeField()
        {
            /*
            EnumField enumField = m_viewWindow.QLogged<EnumField>("PropertyTypeField");

            enumField.RegisterValueChangedCallback(ChangePropertyType);

            enumField.value = activeElement.visualElementType;

            */
        }
        #endregion

        #region RegisterEvents

        protected override void RegisterEvents()
        {
            nameField.RegisterCallback<KeyDownEvent>(OnEnterTextField);
        }

        #endregion


        #region UnRegisterEvents

        protected override void UnRegisterEvents()
        {
            nameField.UnregisterCallback<KeyDownEvent>(OnEnterTextField);
        }

        #endregion

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

            if (string.IsNullOrEmpty(nameField.value) || string.IsNullOrWhiteSpace(nameField.value))
            {

                EditorUtility.DisplayDialog("Empty name!", "The name of a element can't be empty!", "OK");
                nameField.value = activeElement.Name;
                return;
            }

            if (HierarchyTreeView.Instance.AlreadyInList(nameField.value))
            {
                EditorUtility.DisplayDialog("Duplicate Name", "This name is already being used by another element! You can't have duplicate names!", "OK");
                nameField.value = activeElement.Name;
                return;
            }

            activeElement.Name = nameField.value;
            HierarchyTreeView.Instance.Rebuild();

        }

        #endregion







    }
}

