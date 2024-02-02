namespace CardBuilder
{
    using System;
    using UnityEngine;
    using UnityEngine.UIElements;
    using UnityEditor.UIElements;
    using UnityEditor;
    using CardBuilder.Helpers;

    public class AEP_Enum : ActiveElementPropertyBox
    {
        ObjectField currentEnumObjectField;
        Button changeEnumButton;


        #region Binding

        public override void BindItem(PropertyInfo item)
        {
            base.BindItem(item);

            currentEnumObjectField.value = (UnityEngine.Object)activeElement.EnumScript;


        }

        #endregion

        #region Setup

        public override void SetupUI()
        {
            SetupFields();
        }

        private void SetupFields()
        {
            SetupStartButtons();
        }

        private void SetupStartButtons()
        {
            currentEnumObjectField = m_VisualElement.QLogged<ObjectField>("CurrentEnumField");
            currentEnumObjectField.SetEnabled(false);
            changeEnumButton = m_VisualElement.QLogged<Button>("ChangeEnumButton");
        }

        #endregion


        #region RegisterEvents

        protected override void RegisterEvents()
        {
            changeEnumButton.clicked += ChangeEnumButtonClicked;
        }
        #endregion

        #region UnregisterEvents

        protected override void UnRegisterEvents()
        {
            changeEnumButton.clicked -= ChangeEnumButtonClicked;
        }

        #endregion

        #region Events
        private void ChangeEnumButtonClicked()
        {
            string pathName = EditorUtility.OpenFilePanel("Select Enum To Use", "Assets/Editor/SavedScriptableObjects/Enums", "cs");

            Type typeEnum = IOMethods.GetTypeOfObjectUsingPath(pathName);

            bool cancel = false;

            while ((typeEnum == null && !cancel) || (!typeEnum.IsEnum && !cancel))
            {

                cancel = !EditorUtility.DisplayDialog("Script not Enum", "Chosen script is not a enum", "OK", "Cancel");

                if (cancel) return;

                pathName = EditorUtility.OpenFilePanel("Select Enum To Use", "Assets/Editor/SavedScriptableObjects/Enums", "cs");

                typeEnum = IOMethods.GetTypeOfObjectUsingPath(pathName);
            }


            pathName = IOMethods.GetRelativeAssetBasePath(pathName);

            MonoScript enumScript = (MonoScript)AssetDatabase.LoadMainAssetAtPath(pathName);

            currentEnumObjectField.value = enumScript;
            activeElement.EnumScript = enumScript;
        }


        #endregion




    }
}
