using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using CardBuilder.Helpers;
namespace CardBuilder
{
    public class AEP_Style : ActiveElementPropertyBox
    {

        private ColorField backgroundColourField;
        private ColorField textColourField;


        public override void BindItem(PropertyInfo item)
        {
            base.BindItem(item);

            backgroundColourField.value = activeElement.BackgroundColour;
            textColourField.value = activeElement.TextColour;
        }


        #region Setup

        public override void SetupUI()
        {
            SetupBackgroundColour();
            SetupTextColour();
        }


        private void SetupBackgroundColour()
        {
            backgroundColourField = m_VisualElement.QLogged<ColorField>("BackgroundColourField");
            
        }

        private void SetupTextColour()
        {
            textColourField = m_VisualElement.QLogged<ColorField>("TextColourField");
        }

        #endregion


        #region RegisterEvents

        protected override void RegisterEvents()
        {
            RegisterBackgroundColour();
            RegisterTextColour();

        }

        private void RegisterBackgroundColour()
        {
            backgroundColourField.RegisterValueChangedCallback(ChangeBackgroundColour);
        }

        private void RegisterTextColour()
        {
            textColourField.RegisterValueChangedCallback(ChangeTextColour);
        }

        #endregion

        #region UnregisterEvents

        protected override void UnRegisterEvents()
        {
            UnRegisterBackgroundColour();
            UnRegisterTextColour();
        }

        private void UnRegisterTextColour()
        {
            backgroundColourField.UnregisterValueChangedCallback(ChangeBackgroundColour);
        }

        private void UnRegisterBackgroundColour()
        {
            textColourField.UnregisterValueChangedCallback(ChangeTextColour);
        }

        #endregion

        #region Events

        private void ChangeBackgroundColour(ChangeEvent<Color> backGroundColour)
        {
            if (activeElement == null)
            {
                Logs.NoActiveElementError();
                return;
            }

            activeElement.BackgroundColour = backGroundColour.newValue;
        }

        private void ChangeTextColour(ChangeEvent<Color> textColour) 
        {
            if (activeElement == null)
            {
                Logs.NoActiveElementError();
                return;
            }

            activeElement.TextColour = textColour.newValue; 
        }



        #endregion

    }
}

